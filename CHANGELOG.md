# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.3.0]

### Added
- `GrowthBookClient` — new thread-safe singleton client for multiuser server-side scenarios (ASP.NET Core, Azure Functions, workers). Accepts a per-request `UserContext` for each evaluation, with no per-user object allocation.
- `UserContext` — per-request DTO for user attributes, forced variations, forced feature values, tracking callback, and sticky bucket data.
- `Options` — configuration class for `GrowthBookClient` with support for global attributes, forced variations, forced feature values, tracking callback, and sticky bucket service.
- `ForcedFeatureValues` support in `Context` and `GrowthBook` for overriding feature evaluation results (source: "override").
- `SetGlobalAttributes()`, `SetGlobalForcedVariations()`, `SetGlobalForcedFeatureValues()`, `SetTrackingCallback()` — runtime setters on `GrowthBookClient`.
- `GetFeatures()`, `GetGlobalAttributes()` — getters on `GrowthBookClient`.

- `BackgroundSync` — alias for `PreferServerSentEvents` on `Context` and `GrowthBookConfigurationOptions`.
- `RequestHeaders` — custom headers for polling requests.
- `StreamingRequestHeaders` — custom headers for SSE connection (e.g. `Authorization`, `Last-Event-ID`).
- `OnFeaturesRefreshed` on `Context` — fires for both manual and background SSE updates.
- `OnStreamingEventId` — callback for persisting `Last-Event-ID` across restarts.
- `AddGrowthBookClient` DI extension for ASP.NET Core — registers `GrowthBookClient` as singleton.

### Improved
- `FeatureRefreshWorker` now propagates errors to `OnFeaturesRefreshed` callback (fires `false` on failures).
- SSE event listener filters specifically for `"features"` events and deduplicates via `Last-Event-ID`.
- `SSEClient` auto-reconnects on 2xx status codes, stops on 410 Gone.

### Fixed
- Sticky bucket assignment docs now update correctly in-memory after save.
- Empty string fallback attribute no longer causes incorrect bucket assignment.
- `ForcedVariations` null reference in `GrowthBook` constructor.
- `RefreshStickyBuckets` is now called after features refresh in `LoadFeaturesWithResult`.
- `GrowthBookFactory.CreateForUser` assigned the per-user tracking callback unconditionally, so calling it without
  one — the default — wiped the base context's callback and silently stopped tracking that user's experiment
  exposures. It now only overrides when a callback is actually supplied.
- The shared repository `GrowthBookFactory` builds internally was missing the remote evaluation service, so a
  context with `RemoteEval` set evaluated remotely through `new GrowthBook(context)` but not through the factory.
- `GrowthBookFactory.Dispose` now disposes a logger factory it created itself. One supplied through
  `Context.LoggerFactory` is left alone, since the caller may still be using it.

### Deprecated
- `GrowthBookFactory` — use `GrowthBookClient` instead.
- `AddGrowthBook` DI extension — use `AddGrowthBookClient` instead.

## [1.2.0]

- Added custom fields support for experiments.
- Added non-breaking ETag caching support for feature API refreshes.
- Fixed sticky bucket min bucket version handling.
- Fixed invariant numeric condition parsing.
- Fixed bucket range serialization.
- Improved CI build, test, and packaging coverage.

## [1.1.0]

- Upated SDK to comply with the 0.7.0 SDK spec.

## [1.0.7]

- Added async versions of EvalFeature/GetFeatureValue and a flag on the originals for backwards compatibility.

## [1.0.6]

- Set hash version with rule

## [1.0.5]

- Fixed missing readme image to use trusted domain (when viewed from nuget.org)

## [1.0.4]

- Added package readme reference

## [1.0.3]

- Fixed null reference exception when forcing a rule with a tracking callback set and null tracking data.
- Experiment assignments are included in EvalFeature as well as Run.
- IGrowthBook interface fixed to inherit IDisposable.

## [1.0.2]

- Fixed issue with incorrect logic in GrowthBook.GetFeatureResult<T>() call.

## [1.0.1]

- Fixed issue with empty string value sent to IsIn condition evaluation.

## [1.0.0]

- Fully implemented version 0.5.2 of the GrowthBook SDK spec.
- Added support for retrieving features (both regular and encrypted) from the GrowthBook API with in-memory caching.
- Added support for retrieving features (both regular and encrypted) in near-realtime with Server Sent Events (when preferred and available).
- Added extensive support for logging.
- Added more robust error handling.
- New unit test structure for easier use of the standard cases.json test suite.

## [0.2.0]

- Corrected name of `IGrowthbook` to `IGrowthBook`

## [0.1.2]

- ci: moved from MSTest to Xunit

## [0.1.1]

- Handle null namespace property.

## [0.1.0]

- Added a CHANGELOG.md based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)
- Added standard rules for markdown files in .editorconfig
- Ensured that all files have a consistent line-ending (based on what they already have)
- Added `IGrowthBook` interface

## [0.0.6] - 2022-06-07

- Correct package repo

## [0.0.5] - 2022-06-07

- Update package repository link, bump version number for new license inclusion

## [0.0.4] - 2022-06-07

- Initial upload
