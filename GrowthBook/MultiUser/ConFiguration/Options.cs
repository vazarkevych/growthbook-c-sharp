using System;
using Microsoft.Extensions.Logging;

namespace GrowthBook.MultiUser
{
    public class Options
    {
        public string ClientKey { get; set; }
        public string ApiHost { get; set; } = "https://cdn.growthbook.io";
        public string DecryptionKey { get; set; }
        public bool Enabled { get; set; } = true;
        public bool QaMode { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }
        public IGrowthBookFeatureCache FeatureCache { get; set; }
        public IGrowthBookFeatureRepository FeatureRepository { get; set; }
        public Action<bool> OnFeaturesRefreshed { get; set; }
        public Action<Experiment, ExperimentResult> TrackingCallback { get; set; }
    }
}
