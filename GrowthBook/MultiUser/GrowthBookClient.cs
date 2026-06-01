using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GrowthBook.Api;
using GrowthBook.Extensions;
using GrowthBook.Providers;
using GrowthBook.Utilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GrowthBook.MultiUser
{
    public class GrowthBookClient : IDisposable
    {
        private readonly Options _options;
        private readonly IGrowthBookFeatureRepository _repository;
        private readonly IConditionEvaluationProvider _conditionEvaluator;
        private readonly FeatureEvaluationProvider _featureEvaluator;
        private readonly ExperimentEvaluationProvider _experimentEvaluator;
        private readonly ILoggerFactory _loggerFactory;
        private readonly bool _ownsLoggerFactory;
        private readonly bool _ownsRepository;
        private volatile IDictionary<string, Feature> _currentFeatures = new Dictionary<string, Feature>();
        private bool _disposed;

        public GrowthBookClient(Options options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));

            if (options.LoggerFactory != null)
            {
                _loggerFactory = options.LoggerFactory;
                _ownsLoggerFactory = false;
            }
            else
            {
                _loggerFactory = LoggerFactory.Create(builder => { });
                _ownsLoggerFactory = true;
            }

            _conditionEvaluator = new ConditionEvaluationProvider(_loggerFactory.CreateLogger<ConditionEvaluationProvider>());
            _experimentEvaluator = new ExperimentEvaluationProvider(
                _loggerFactory.CreateLogger<ExperimentEvaluationProvider>(), _conditionEvaluator);
            _featureEvaluator = new FeatureEvaluationProvider(
                _loggerFactory.CreateLogger<FeatureEvaluationProvider>(), _conditionEvaluator);

            if (options.FeatureRepository != null)
            {
                _repository = options.FeatureRepository;
                _ownsRepository = false;
            }
            else
            {
                _repository = CreateRepository(options,  _loggerFactory);
                _ownsRepository = true;
            }
        }

        public async Task InitializeAsync(CancellationToken ct = default)
        {
            var features = await _repository.GetFeatures(null, ct);
            if (features != null)
            {
                _currentFeatures = features;
            }
        }

        public async Task RefreshFeaturesAsync(CancellationToken ct = default)
        {
            var features = await _repository.GetFeatures(
                new GrowthBookRetrievalOptions { ForceRefresh = true, WaitForCompletion = true}, ct);

            if (features != null)
            {
                _currentFeatures = features;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_ownsRepository)
            {
                _repository.Cancel();
            }

            if (_ownsLoggerFactory && _loggerFactory is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public bool IsOn(string key, UserContext userContext)
            => EvalFeature(key, userContext).On;

        public bool IsOff(string key, UserContext userContext)
            => !IsOn(key, userContext);

        public FeatureResult EvalFeature(string key, UserContext userContext)
        {
            var context = BuildEvaluationContext(userContext);
            return _featureEvaluator.EvaluateFeature(key, context);
        }

        public ExperimentResult Run(Experiment experiment, UserContext userContext)
        {
            var context = BuildEvaluationContext(userContext);
            return _experimentEvaluator.RunExperiment(experiment, null, context);
        }

        public T GetFeature<T>(string key, T fallback, UserContext userContext)
        {
            var result = EvalFeature(key, userContext);
            if (result.Value == null)
            {
                return fallback;
            }

            try
            {
                return result.Value.ToObject<T>();
            }
            catch
            {
                return fallback;
            }

        }

        private EvaluationContext BuildEvaluationContext(UserContext userContext)
        {
            var stickyBucketService = userContext?.StickyBucketService ?? _options.StickyBucketService;
            var stickyBucketDocs = userContext?.StickyBucketAssignmentDocs;

            if (stickyBucketService != null && stickyBucketDocs == null)
            {
                var attrs = ExperimentUtilities.DeriveIdentifierAttributes(
                    _currentFeatures,
                    null,
                    userContext?.Attributes ?? new JObject());
                stickyBucketDocs = stickyBucketService.GetAllAssignments(attrs);
            }

            var global = new GlobalContext
            {
                Features = _currentFeatures,
                Enabled = _options.Enabled,
                QaMode = _options.QaMode,
                TrackingCallback = _options.TrackingCallback,
                StickyBucketService = stickyBucketService
            };

            var user = new UserContext
            {
                Attributes = userContext?.Attributes ?? new JObject(),
                ForcedVariations = userContext?.ForcedVariations,
                TrackingCallback = userContext?.TrackingCallback,
                StickyBucketService = stickyBucketService,
                Url = userContext?.Url,
                StickyBucketAssignmentDocs = stickyBucketDocs ?? new Dictionary<string, StickyAssignmentsDocument>()
            };

            return new EvaluationContext(global, user);
        }

        private static IGrowthBookFeatureRepository CreateRepository(Options options, ILoggerFactory loggerFactory)
        {
            var config = new GrowthBookConfigurationOptions
            {
                ApiHost = options.ApiHost,
                ClientKey = options.ClientKey,
                DecryptionKey = options.DecryptionKey,
                CacheExpirationInSeconds = 60,
                PreferServerSentEvents = true
            };

            var cache = options.FeatureCache ?? new InMemoryFeatureCache(cacheExpirationInSeconds: 60);
            var httpClientFactory = new HttpClientFactory(requestTimeoutInSeconds: 60);
            var workLogger = loggerFactory.CreateLogger<FeatureRefreshWorker>();
            var repoLogger = loggerFactory.CreateLogger<FeatureRepository>();
            var worker = new FeatureRefreshWorker(workLogger, httpClientFactory, config, cache);

            return new FeatureRepository(repoLogger, cache, worker);
        }
    }
}
