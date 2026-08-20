using FluentAssertions;
using GrowthBook.Providers;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GrowthBook.Tests.CustomTests
{
    /// <summary>
    /// Tests for the $inGroup / $notInGroup saved group operators, with emphasis on the inputs the
    /// conformance fixtures don't reach. Every $inGroup case in standard-cases.json supplies the hash
    /// attribute, so the absent-attribute path is invisible to the spec suite and has to be pinned here.
    /// The reference behavior these assert against is the JS SDK's evalOperatorCondition, which applies
    /// no null guard at all: <c>isIn(actual, savedGroups[expected] || [])</c>.
    /// </summary>
    public class SavedGroupOperatorTests
    {
        private const string GroupName = "admins";

        private readonly ConditionEvaluationProvider _provider;

        public SavedGroupOperatorTests()
        {
            var logger = new NullLogger<ConditionEvaluationProvider>();
            _provider = new ConditionEvaluationProvider(logger);
        }

        private static JObject SavedGroups => JObject.Parse(@"{ ""admins"": [ ""user-1"", ""user-2"" ] }");

        private static JObject InGroup(string groupId) => JObject.Parse($@"{{ ""id"": {{ ""$inGroup"": {groupId} }} }}");

        private static JObject NotInGroup(string groupId) => JObject.Parse($@"{{ ""id"": {{ ""$notInGroup"": {groupId} }} }}");

        private bool Eval(JObject attributes, JObject condition) => _provider.EvalCondition(attributes, condition, SavedGroups);

        [Fact]
        public void InGroupMatchesAMemberOfTheGroup()
        {
            var attributes = JObject.Parse(@"{ ""id"": ""user-1"" }");

            Eval(attributes, InGroup($"\"{GroupName}\"")).Should().BeTrue("because user-1 is listed in the group");
        }

        [Fact]
        public void InGroupDoesNotMatchANonMember()
        {
            var attributes = JObject.Parse(@"{ ""id"": ""user-9"" }");

            Eval(attributes, InGroup($"\"{GroupName}\"")).Should().BeFalse("because user-9 is not listed in the group");
        }

        [Fact]
        public void NotInGroupIsTheExactNegationForAPresentAttribute()
        {
            var member = JObject.Parse(@"{ ""id"": ""user-1"" }");
            var nonMember = JObject.Parse(@"{ ""id"": ""user-9"" }");

            Eval(member, NotInGroup($"\"{GroupName}\"")).Should().BeFalse();
            Eval(nonMember, NotInGroup($"\"{GroupName}\"")).Should().BeTrue();
        }

        [Fact]
        public void NotInGroupIsTrueWhenTheAttributeIsAbsent()
        {
            var attributes = JObject.Parse(@"{ ""other"": ""value"" }");

            Eval(attributes, NotInGroup($"\"{GroupName}\""))
                .Should().BeTrue("because the reference SDK evaluates !isIn(undefined, [...]) as true - an absent attribute is not a member of anything");
        }

        [Fact]
        public void InGroupIsFalseWhenTheAttributeIsAbsent()
        {
            var attributes = JObject.Parse(@"{ ""other"": ""value"" }");

            Eval(attributes, InGroup($"\"{GroupName}\""))
                .Should().BeFalse("because an absent attribute cannot be a member of the group");
        }

        [Fact]
        public void TheTwoOperatorsNeverAgreeForAnAbsentAttribute()
        {
            var attributes = JObject.Parse(@"{ ""other"": ""value"" }");

            var isInGroup = Eval(attributes, InGroup($"\"{GroupName}\""));
            var isNotInGroup = Eval(attributes, NotInGroup($"\"{GroupName}\""));

            isInGroup.Should().NotBe(isNotInGroup,
                "because the operators are logical negations of each other - collapsing both to false asserts that the user is simultaneously in and not in the group");
        }

        [Fact]
        public void AnAbsentAttributeBehavesTheSameAsAnExplicitJsonNull()
        {
            var absent = JObject.Parse(@"{ ""other"": ""value"" }");
            var explicitNull = JObject.Parse(@"{ ""id"": null }");

            Eval(absent, InGroup($"\"{GroupName}\"")).Should().Be(Eval(explicitNull, InGroup($"\"{GroupName}\"")));
            Eval(absent, NotInGroup($"\"{GroupName}\"")).Should().Be(Eval(explicitNull, NotInGroup($"\"{GroupName}\"")),
                "because a JValue wrapping JSON null and a missing key are both 'no value' and must not diverge");
        }

        [Fact]
        public void AnUnknownGroupIdBehavesAsAnEmptyGroup()
        {
            var attributes = JObject.Parse(@"{ ""id"": ""user-1"" }");

            Eval(attributes, InGroup("\"no-such-group\"")).Should().BeFalse();
            Eval(attributes, NotInGroup("\"no-such-group\"")).Should().BeTrue("because savedGroups[expected] || [] makes an unknown group an empty one");
        }

        [Fact]
        public void ANullGroupIdBehavesAsAnEmptyGroup()
        {
            var attributes = JObject.Parse(@"{ ""id"": ""user-1"" }");

            Eval(attributes, InGroup("null")).Should().BeFalse();
            Eval(attributes, NotInGroup("null")).Should().BeTrue("because the reference SDK has no guard on the group id either");
        }

        [Fact]
        public void AnAbsentAttributeCombinedWithAnUnknownGroupStaysConsistent()
        {
            var attributes = JObject.Parse(@"{ ""other"": ""value"" }");

            Eval(attributes, InGroup("\"no-such-group\"")).Should().BeFalse();
            Eval(attributes, NotInGroup("\"no-such-group\"")).Should().BeTrue();
        }

        [Fact]
        public void GroupOperatorsStillWorkWithoutAnySavedGroups()
        {
            var attributes = JObject.Parse(@"{ ""id"": ""user-1"" }");

            _provider.EvalCondition(attributes, InGroup($"\"{GroupName}\""), new JObject()).Should().BeFalse();
            _provider.EvalCondition(attributes, NotInGroup($"\"{GroupName}\""), new JObject())
                .Should().BeTrue("because an empty saved group set must not make the operator fall through to a blanket false");
        }

        [Fact]
        public void InGroupMatchesWhenTheAttributeIsAnArraySharingAMember()
        {
            var attributes = JObject.Parse(@"{ ""id"": [ ""user-9"", ""user-2"" ] }");

            Eval(attributes, InGroup($"\"{GroupName}\"")).Should().BeTrue("because the reference isIn treats an array attribute as matching on any shared element");
            Eval(attributes, NotInGroup($"\"{GroupName}\"")).Should().BeFalse();
        }
    }
}
