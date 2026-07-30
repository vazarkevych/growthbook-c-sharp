# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

- Fixed `MergeAttributes` mutating the attributes in place, which could expose a partially merged state to a
  concurrent evaluation, and fixed it throwing on null values.
- Added `UpdateAttributes`/`MergeAttributes` to `IGrowthBook`.
- Added `UpdateAttributesAsync`/`MergeAttributesAsync`, which wait for the remote evaluation that an attribute
  change triggers. The synchronous versions no longer leave that evaluation unobserved: the next feature load
  waits for it, so a stale in-flight response can't overwrite newer features.
- Fixed a superseded remote evaluation overwriting a newer one. Each evaluation is now tagged with a generation
  allocated together with the state it sends, and only the most recent one publishes its features, so a response
  built for attributes that have already been replaced is dropped instead of applied.
- Fixed changes to `ForcedVariations` not refreshing remotely evaluated features. They're part of the remote
  evaluation payload, so assigning the property now triggers an evaluation the same way attribute changes do, and
  `SetForcedVariations`/`SetForcedVariationsAsync` were added for callers that need to wait for it. Assigning
  `Attributes` directly triggers one as well, rather than only `UpdateAttributes`/`MergeAttributes` doing so.
- Added retries with exponential backoff to remote evaluation requests, so a single transient failure no longer
  leaves a remote-eval consumer on the previously evaluated features until something else triggers a refresh.
  Transport errors, timeouts, 408, 429 and 5xx are retried (honouring `Retry-After`); other 4xx are not, since
  repeating a rejected request only fails the same way. Configurable through `RemoteEvaluationRetryPolicy`;
  defaults to 3 attempts, 500ms initial backoff, capped at 5s. The whole round is bounded by a 60s budget, so
  retrying can't hold a waiting caller for longer than a single request already could.
- Fixed the async API capturing the caller's synchronization context, which could deadlock an application that
  blocks on a `GrowthBook` task (including `EvalFeature(key, alwaysLoadFeatures: true)`, which blocks internally).
  **Note:** as a consequence, a `TrackingCallback` or subscriber invoked by an async evaluation can now run on a
  thread pool thread rather than on the caller's context. Callbacks that touch UI controls or other
  context-affine state need to marshal back themselves.

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
