using System;
using System.Collections.Generic;
using GrowthBook.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GrowthBook.MultiUser.Configuration
{
    /// <summary>Singleton client for multiuser feature flagging. Register as a singleton in DI.</summary>
    public class Options
    {
        /// <summary>The GrowthBook SDK key used to fetch features from the API.</summary>
        public string ClientKey { get; set; }

        /// <summary>The GrowthBook API host. Defaults to https://cdn.growthbook.io.</summary>
        public string ApiHost { get; set; } = "https://cdn.growthbook.io";

        /// <summary>Optional decryption key. If set, the features payload is expected to be encrypted.</summary>
        public string DecryptionKey { get; set; }

        /// <summary>Whether all experiments are globally enabled. Set to false to disable all experiments. Defaults to true.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>If true, random assignment is disabled and only explicitly forced variations are used.</summary>
        public bool QaMode { get; set; }

        /// <summary>Attributes applied to every user evaluation. Per-request <see cref="UserContext.Attributes"/> take precedence.</summary>
        public JObject GlobalAttributes { get; set; }

        /// <summary>Force specific experiments to always assign a specific variation for all users. Used for QA.</summary>
        public IDictionary<string, int> GlobalForcedVariations { get; set; }

        /// <summary>Force specific feature values for all users. Used for QA and testing.</summary>
        public IDictionary<string, JToken> GlobalForcedFeatureValues { get; set; }

        /// <summary>Logger factory for internal diagnostics. If not set, a no-op logger is used.</summary>
        public ILoggerFactory LoggerFactory { get; set; }

        /// <summary>Custom feature cache. Defaults to <see cref="InMemoryFeatureCache"/>.</summary>
        public IGrowthBookFeatureCache FeatureCache { get; set; }

        /// <summary>Custom feature repository. If set, <see cref="ClientKey"/> and <see cref="ApiHost"/> are ignored.</summary>
        public IGrowthBookFeatureRepository FeatureRepository { get; set; }

        /// <summary>Invoked after features are successfully loaded or refreshed.</summary>
        public Action<bool> OnFeaturesRefreshed { get; set; }

        /// <summary>Invoked when a user is assigned to an experiment variation. Use to report to your analytics system.</summary>
        public Action<Experiment, ExperimentResult> TrackingCallback { get; set; }

        /// <summary>Service that provides sticky bucketing to ensure consistent experiment assignments across sessions.</summary>
        public IStickyBucketService StickyBucketService { get; set; }
    }
}
