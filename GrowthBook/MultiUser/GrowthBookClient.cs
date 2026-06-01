using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GrowthBook.Api;
using GrowthBook.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GrowthBook.MultiUser
{
    public class GrowthBookClient : IDisposable
    {
        private readonly Options _options;
        private readonly IGrowthBookFeatureRepository _repository;
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
            // Create light GrowthBook without repository - features already exist
            using var gb = CreateEvaluator(userContext);
            return gb.EvalFeature(key);
        }

        public ExperimentResult Run(Experiment experiment, UserContext userContext)
        {
            using var gb = CreateEvaluator(userContext);
            return gb.Run(experiment);
        }

        public T GetFeature<T>(string key, T fallback, UserContext userContext)
        {
            using var gb = CreateEvaluator(userContext);
            return gb.GetFeatureValue(key, fallback);
        }

        private GrowthBook CreateEvaluator(UserContext userContext)
        {
            var stickyBucketService = userContext?.StickyBucketService ?? _options.StickyBucketService;
            var stickyBucketDocs = userContext?.StickyBucketAssignmentDocs;

            if (stickyBucketService != null && stickyBucketDocs == null)
            {
                var attrs = userContext?.Attributes?.Properties()
                    .Where(p => !p.Value.IsNull() && !string.IsNullOrEmpty(p.Value.ToString()))
                    .Select(p => $"{p.Name}||{p.Value}");

                stickyBucketDocs = attrs != null
                    ? stickyBucketService.GetAllAssignments(attrs)
                    : null;
            }

            return new GrowthBook(new Context
            {
                Features = _currentFeatures,
                Enabled = _options.Enabled,
                QaMode = _options.QaMode,
                Attributes = userContext?.Attributes ?? new JObject(),
                Url = userContext?.Url,
                ForcedVariations = userContext?.ForcedVariations,
                TrackingCallback = userContext?.TrackingCallback ?? _options.TrackingCallback,
                StickyBucketService = stickyBucketService,
                StickyBucketAssignmentDocs = stickyBucketDocs,
                LoggerFactory = _loggerFactory
            });
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
