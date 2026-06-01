using System;
using System.Collections.Generic;
using GrowthBook.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GrowthBook.MultiUser
{
    public class Options
    {
        public string ClientKey { get; set; }
        public string ApiHost { get; set; } = "https://cdn.growthbook.io";
        public string DecryptionKey { get; set; }
        public bool Enabled { get; set; } = true;
        public bool QaMode { get; set; }
        public JObject GlobalAttributes { get; set; }
        public IDictionary<string, int> GlobalForcedVariations { get; set; }
        public IDictionary<string, JToken> GlobalForcedFeatureValues { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }
        public IGrowthBookFeatureCache FeatureCache { get; set; }
        public IGrowthBookFeatureRepository FeatureRepository { get; set; }
        public Action<bool> OnFeaturesRefreshed { get; set; }
        public Action<Experiment, ExperimentResult> TrackingCallback { get; set; }
        public IStickyBucketService StickyBucketService { get; set; }
    }
}
