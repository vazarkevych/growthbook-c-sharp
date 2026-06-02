using System;
using System.Collections.Generic;
using GrowthBook.Services;
using Newtonsoft.Json.Linq;

namespace GrowthBook.MultiUser.Configuration
{
    /// <summary>Per-request context containing user-specific data passed to each evaluation call.</summary>
    public class UserContext
    {
        /// <summary>User attributes used for targeting and experiment assignment (e.g. id, country, plan).</summary>
        public JObject Attributes { get; set; } = new JObject();

        /// <summary>Attribute overrides applied on top of <see cref="Attributes"/>. Takes highest precedence.</summary>
        public JObject AttributeOverrides { get; set; } = new JObject();

        /// <summary>The URL of the current page. Used for URL-based targeting rules.</summary>
        public string Url { get; set; }

        /// <summary>Force specific experiments to always assign a specific variation for this user. Used for QA.</summary>
        public IDictionary<string, int>   ForcedVariations { get; set; }

        /// <summary>Force specific feature values for this user. Takes precedence over global forced values.</summary>
        public IDictionary<string, JToken> ForcedFeatureValues { get; set; }

        /// <summary>Per-request tracking callback. Overrides <see cref="Options.TrackingCallback"/> if set.</summary>
        public Action<Experiment, ExperimentResult> TrackingCallback { get; set; }

        /// <summary>Per-request sticky bucket service. Overrides <see cref="Options.StickyBucketService"/> if set.</summary>
        public IStickyBucketService StickyBucketService { get; set; }

        /// <summary>Pre-loaded sticky bucket assignment docs. If null, loaded automatically from the service.</summary>
        public IDictionary<string, StickyAssignmentsDocument> StickyBucketAssignmentDocs { get; set; }
    }
}
