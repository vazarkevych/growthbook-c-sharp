using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GrowthBook.Services;
using GrowthBook.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GrowthBook.Tests.StandardTests.GrowthBookTests;

public class StickyBucketTests : UnitTest
{
    [StandardCaseTestCategory("stickyBucket")]
    public class StickyBucketTestCase
    {
        [TestPropertyIndex(0)]
        public string TestName { get; set; }
        [TestPropertyIndex(1)]
        public Context Context { get; set; }
        [TestPropertyIndex(2)]
        public StickyAssignmentsDocument[] PreExistingAssignmentDocs { get; set; } = Array.Empty<StickyAssignmentsDocument>();
        [TestPropertyIndex(3)]
        public string FeatureName { get; set; }
        [TestPropertyIndex(4)]
        public JToken ExpectedResult { get; set; }
        [TestPropertyIndex(5)]
        public Dictionary<string, StickyAssignmentsDocument> ExpectedAssignmentDocs { get; set; } = new Dictionary<string, StickyAssignmentsDocument>();
    }

    [Theory]
    [MemberData(nameof(GetMappedTestsInCategory), typeof(StickyBucketTestCase))]
    public void Run(StickyBucketTestCase testCase)
    {
        var service = new InMemoryStickyBucketService();

        testCase.Context.StickyBucketService = service;
        testCase.Context.StickyBucketAssignmentDocs = testCase.PreExistingAssignmentDocs.ToDictionary(x => x.FormattedAttribute);

        // NOTE: Existing sticky bucket JSON tests in the JS SDK load this into the service up front
        //       but I wonder if that's correct because without that any assignment doc that exists
        //       other than those will not be stored and some of these test cases will fail.

        foreach (var document in testCase.PreExistingAssignmentDocs)
        {
            service.SaveAssignments(document);
        }

        var gb = new GrowthBook(testCase.Context);

        var result = gb.EvalFeature(testCase.FeatureName);

        var actualResult = JToken.Parse(JsonConvert.SerializeObject(result.ExperimentResult));

        if (testCase.ExpectedResult is JObject obj)
        {
            foreach (var property in obj.Properties())
            {
                actualResult[property.Name].ToString().Should().Be(property.Value.ToString());
            }
        }
        else
        {
            actualResult.ToString().Should().Be(testCase.ExpectedResult.ToString());
        }

        var storedDocuments = service.GetAllAssignments(testCase.ExpectedAssignmentDocs.Keys);

        storedDocuments.Should().BeEquivalentTo(testCase.ExpectedAssignmentDocs, "because those should have been stored correctly");
    }

    [Fact]
    public void MinBucketVersionAllowsAssignmentAtSameVersion()
    {
        var experiment = new Experiment
        {
            Key = "feature-exp",
            HashAttribute = "id",
            FallbackAttribute = "deviceId",
            BucketVersion = 3,
            MinBucketVersion = 3
        };
        var meta = new List<VariationMeta>
        {
            new VariationMeta { Key = "0" },
            new VariationMeta { Key = "1" },
            new VariationMeta { Key = "2" }
        };
        var attributes = JObject.FromObject(new { id = "i123" });
        var documents = new Dictionary<string, StickyAssignmentsDocument>
        {
            ["id||i123"] = new StickyAssignmentsDocument(
                "id",
                "i123",
                new Dictionary<string, string> { ["feature-exp__3"] = "2" })
        };

        var result = ExperimentUtilities.GetStickyBucketVariation(experiment, 3, 3, meta, attributes, documents);

        result.IsVersionBlocked.Should().BeFalse("because minBucketVersion blocks older versions only");
        result.VariationIndex.Should().Be(2);
    }
}
