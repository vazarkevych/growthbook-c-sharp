using System;
using System.Collections;
using System.Collections.Generic;
using GrowthBook.Services;
using Newtonsoft.Json.Linq;

namespace GrowthBook.MultiUser
{
    public class UserContext
    {
        public JObject Attributes { get; set; } = new JObject();
        public JObject AttributeOverrides { get; set; } = new JObject();
        public string Url { get; set; }
        public IDictionary<string, int>   ForcedVariations { get; set; }
        public IDictionary<string, JToken> ForcedFeatureValues { get; set; }
        public Action<Experiment, ExperimentResult> TrackingCallback { get; set; }
        public IStickyBucketService StickyBucketService { get; set; }
        public IDictionary<string, StickyAssignmentsDocument> StickyBucketAssignmentDocs { get; set; }
    }
}
