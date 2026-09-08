using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GrowthBook.Tests.CustomTests;

/// <summary>
/// Covers <see cref="GrowthBook.GetAllResults"/> handing out a snapshot rather than the dictionary the
/// instance evaluates against. It used to return the internal collection directly, so a caller reading
/// the results could clear or rewrite the instance's own assignment state.
/// </summary>
public class GetAllResultsSnapshotTests : UnitTest
{
    private static Feature ExperimentFeature(string experimentKey) => new Feature
    {
        DefaultValue = false,
        Rules = new List<FeatureRule>
        {
            new FeatureRule
            {
                Key = experimentKey,
                Variations = new JArray(false, true),
                Coverage = 1d,
                Meta = new List<VariationMeta> { new VariationMeta { Key = "0" }, new VariationMeta { Key = "1" } }
            }
        }
    };

    private static GrowthBook NewInstance() => new GrowthBook(new Context
    {
        Attributes = JObject.FromObject(new { id = "user-1" }),
        Features = new Dictionary<string, Feature>
        {
            ["first"] = ExperimentFeature("first-experiment"),
            ["second"] = ExperimentFeature("second-experiment")
        }
    });

    [Fact]
    public void ItIsEmptyRatherThanNullBeforeAnythingIsEvaluated()
    {
        using var growthBook = NewInstance();

        growthBook.GetAllResults().Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void ClearingTheReturnedMapLeavesTheInstanceIntact()
    {
        using var growthBook = NewInstance();

        growthBook.EvalFeature("first");

        growthBook.GetAllResults().Clear();

        growthBook.GetAllResults().Should().ContainKey("first-experiment",
            "because the caller was handed a copy - clearing it must not wipe the instance's own state");
    }

    [Fact]
    public void WritingIntoTheReturnedMapDoesNotReachTheInstance()
    {
        using var growthBook = NewInstance();

        growthBook.EvalFeature("first");

        growthBook.GetAllResults()["injected"] = new ExperimentAssignment();

        growthBook.GetAllResults().Should().NotContainKey("injected");
    }

    [Fact]
    public void AnEarlierSnapshotDoesNotSeeLaterEvaluations()
    {
        using var growthBook = NewInstance();

        growthBook.EvalFeature("first");

        var snapshot = growthBook.GetAllResults();

        snapshot.Should().HaveCount(1);

        growthBook.EvalFeature("second");

        snapshot.Should().HaveCount(1, "a snapshot is a point in time, not a live view");
        growthBook.GetAllResults().Should().HaveCount(2, "while a fresh call reflects both evaluations");
    }

    [Fact]
    public void TheSnapshotStillCarriesTheAssignments()
    {
        using var growthBook = NewInstance();

        growthBook.EvalFeature("first");

        var results = growthBook.GetAllResults();

        results.Should().ContainKey("first-experiment");
        results["first-experiment"].Experiment.Key.Should().Be("first-experiment");
        results["first-experiment"].Result.InExperiment.Should().BeTrue();
    }

    [Fact]
    public async Task ReadingWhileEvaluationIsRunningDoesNotThrow()
    {
        // Copying a Dictionary while another thread writes to it throws. The read has to be serialised
        // against the writes, not just defensive.
        using var growthBook = new GrowthBook(new Context
        {
            Attributes = JObject.FromObject(new { id = "user-1" }),
            Features = new Dictionary<string, Feature>()
        });

        for (var i = 0; i < 200; i++)
        {
            growthBook.Features[$"flag-{i}"] = ExperimentFeature($"experiment-{i}");
        }

        var deadline = DateTime.UtcNow.AddSeconds(2);

        var evaluating = Task.Run(() =>
        {
            while (DateTime.UtcNow < deadline)
            {
                for (var i = 0; i < 200; i++)
                {
                    growthBook.EvalFeature($"flag-{i}");
                }

                growthBook.Attributes = JObject.FromObject(new { id = Guid.NewGuid().ToString() });
            }
        });

        var reading = Task.Run(() =>
        {
            while (DateTime.UtcNow < deadline)
            {
                _ = growthBook.GetAllResults().Count;
            }
        });

        await Task.WhenAll(evaluating, reading);
    }
}
