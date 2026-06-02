using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GrowthBook.MultiUser;
using GrowthBook.MultiUser.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace GrowthBook.Tests.MultiUser;

public class GrowthBookClientTests
{
    private static IDictionary<string, Feature> CreateFeatures() => new Dictionary<string, Feature>
    {
        ["dark-mode"] = new Feature
        {
            DefaultValue = JToken.FromObject(false),
            Rules = new List<FeatureRule>
            {
                new FeatureRule
                {
                    Variations = JArray.FromObject(new[] { false, true }),
                    HashAttribute = "id",
                    Coverage = 1.0,
                    Weights = new List<double> { 0.5, 0.5 }
                }
            }
        }
    };

    private static GrowthBookClient CreateClient()
    {
        var mockRepo = Substitute.For<IGrowthBookFeatureRepository>();
        mockRepo
            .GetFeatures(Arg.Any<GrowthBookRetrievalOptions>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(CreateFeatures()));

        return new GrowthBookClient(new Options
        {
            ClientKey = "sdk-test", LoggerFactory = NullLoggerFactory.Instance, FeatureRepository = mockRepo
        });
    }

    [Fact]
    public async Task EvalFeature_ConcurrentRequests_DoNotMixUserAttributes()
    {
        using var client = CreateClient();
        await client.InitializeAsync();

        var expected = Enumerable.Range(1, 100)
            .ToDictionary(
                i => $"user-{i}",
                i => client.EvalFeature("dark-mode",
                    new UserContext { Attributes = JObject.Parse($"{{\"id\": \"user-{i}\"}}") }
                ).On
            );

        var tasks = Enumerable.Range(1, 100)
            .Select(i => Task.Run(() =>
            {
                var userId = $"user-{i}";
                var result = client.EvalFeature("dark-mode",
                    new UserContext { Attributes = JObject.Parse($"{{\"id\": \"{userId}\"}}") });

                return (userId, result.On);
            }));

        var results = Task.WhenAll(tasks).Result;

        foreach (var (userId, actualValue) in results)
        {
            actualValue.Should().Be(expected[userId],
                because: $"{userId} should always get their own result, not another user's");
        }
    }

    [Fact]
    public async Task IsOn_ReturnsDeterministicResult_ForSameUser()
    {
        using var client = CreateClient();
        await client.InitializeAsync();

        var userCtx = new UserContext { Attributes = JObject.Parse("{\"id\": \"user-42\"}") };

        var first = client.IsOn("dark-mode", userCtx);
        var second = client.IsOn("dark-mode", userCtx);
        var third = client.IsOn("dark-mode", userCtx);

        second.Should().Be(first);
        third.Should().Be(first);
    }

    [Fact]
    public async Task EvalFeature_UnknownFeature_ReturnsOff()
    {
        using var client = CreateClient();
        await client.InitializeAsync();

        var result = client.EvalFeature("non-existent-feature",
            new UserContext { Attributes = JObject.Parse("{\"id\": \"user-1\"}") });

        result.On.Should().BeFalse();
        result.Source.Should().Be(FeatureResult.SourceId.UnknownFeature);
    }

    [Fact]
    public async Task InitializeAsync_WithNullFeatures_DoesNotCrash()
    {
        var mockRepo = Substitute.For<IGrowthBookFeatureRepository>();
        mockRepo
            .GetFeatures(Arg.Any<GrowthBookRetrievalOptions>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IDictionary<string, Feature>>(null));

        using var client = new GrowthBookClient(new Options
        {
            ClientKey = "sdk-test", LoggerFactory = NullLoggerFactory.Instance, FeatureRepository = mockRepo
        });

        await client.InitializeAsync();

        var result = client.IsOn("dark-mode", new UserContext
        {
            Attributes = JObject.Parse("{\"id\": \"user-1\"}")
        });

        result.Should().BeFalse();
    }
}
