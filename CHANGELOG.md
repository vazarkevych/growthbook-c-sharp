# Changelog

All notable changes to this project will be documented in this file.

This project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html). From 2.2.0 on,
[release-please](https://github.com/googleapis/release-please) writes the entries from merged PR titles.

## [3.0.0](https://github.com/vazarkevych/growthbook-c-sharp/compare/v2.1.1...v3.0.0) (2026-11-20)


### ⚠ BREAKING CHANGES

* conditions using an unsupported operator or saved group type no longer pass when negated

### Features

* add CustomFields property to Experiment.cs, update Equals function, add custom DictionariesEqual function ([f756bf7](https://github.com/vazarkevych/growthbook-c-sharp/commit/f756bf747176ad45b5af7e3152699bb7a1493f2c))
* align with spec 0.12.0 (strict types, condition edge cases, bucketing, payload reduction) ([#114](https://github.com/vazarkevych/growthbook-c-sharp/issues/114)) ([63be0ce](https://github.com/vazarkevych/growthbook-c-sharp/commit/63be0cecf84f54fb1eb257e69b5f20a0fffbfa6c))
* Di registration ([#118](https://github.com/vazarkevych/growthbook-c-sharp/issues/118)) ([0f36a1c](https://github.com/vazarkevych/growthbook-c-sharp/commit/0f36a1c6f37023290fc72d387e22d45a1d6a91e4))
* Growthbook client ([#117](https://github.com/vazarkevych/growthbook-c-sharp/issues/117)) ([be71c15](https://github.com/vazarkevych/growthbook-c-sharp/commit/be71c1583f6a43946ade1a8836084a1d8107171c))
* implement CustomFieldsTests ([540241c](https://github.com/vazarkevych/growthbook-c-sharp/commit/540241c3a73437dfe86c13d9b618622b36f0c176))
* implement ExperimentExtensions class ([731eb94](https://github.com/vazarkevych/growthbook-c-sharp/commit/731eb94a92294ca81b3327cb0b0b8977dbc5deda))
* Logging to a structured logging ([adf1f5f](https://github.com/vazarkevych/growthbook-c-sharp/commit/adf1f5feabdcfca814fe53278ce6e11ba071a8d0))
* Logging to a structured logging ([394bdb1](https://github.com/vazarkevych/growthbook-c-sharp/commit/394bdb1e1102122126f354ed19eb48867557904a))


### Bug Fixes

* apply reconnect backoff when the SSE stream closes cleanly ([#91](https://github.com/vazarkevych/growthbook-c-sharp/issues/91)) ([e7e90b0](https://github.com/vazarkevych/growthbook-c-sharp/commit/e7e90b06bae3b50e5e996a67a364e90d20704b1a))
* avoid quadratic regex when stripping version build metadata ([#123](https://github.com/vazarkevych/growthbook-c-sharp/issues/123)) ([e90a92b](https://github.com/vazarkevych/growthbook-c-sharp/commit/e90a92bfb347b346e8d3bad23575b1f2c45f149a))
* BucketRangeTupleConverter.WriteJson casts BucketRange to Namespace ([6a59cc0](https://github.com/vazarkevych/growthbook-c-sharp/commit/6a59cc083bb3127af3d4e413b5c1cacf71f62662))
* ci crash after fixing conflicts ([d1495af](https://github.com/vazarkevych/growthbook-c-sharp/commit/d1495afdc4885f560577bcae7761d926195de9fc))
* correct BucketRange serialization in BucketRangeTupleConverter ([eb48245](https://github.com/vazarkevych/growthbook-c-sharp/commit/eb482454e48e98c479a18a87686983d656c5b0da))
* correct sticky bucket change detection by comparing assignments only ([#76](https://github.com/vazarkevych/growthbook-c-sharp/issues/76)) ([df9a68a](https://github.com/vazarkevych/growthbook-c-sharp/commit/df9a68a3edf9e2998e7c2fb8c3a34a7753cfd765))
* decrypt Context.EncryptedFeatures and stop logging the decryption key ([71eb941](https://github.com/vazarkevych/growthbook-c-sharp/commit/71eb941e297dfc5b5020078571f4f23b6893a4cf))
* decrypt Context.EncryptedFeatures and stop logging the decryptn key ([fb07707](https://github.com/vazarkevych/growthbook-c-sharp/commit/fb0770745a30a08d56d7c314b6f53d42bc5daa32))
* evaluate a context payload without reaching for an API ([cec7d76](https://github.com/vazarkevych/growthbook-c-sharp/commit/cec7d769ba75415fee2b23084f5348d68cc91a04))
* implement GetHashCode on the five types that override Equals ([#102](https://github.com/vazarkevych/growthbook-c-sharp/issues/102)) ([15ec84f](https://github.com/vazarkevych/growthbook-c-sharp/commit/15ec84f5e451d08446399636dce4d8db96d3144b))
* make cache expiry testable and stop pinning test assertions to log statements- [#32](https://github.com/vazarkevych/growthbook-c-sharp/issues/32) ([#107](https://github.com/vazarkevych/growthbook-c-sharp/issues/107)) ([a5989ce](https://github.com/vazarkevych/growthbook-c-sharp/commit/a5989ceda276f2f2c39ce8052036bfa8fc1d91d9))
* NullReferenceException in ElemMatch array condition logging ([#74](https://github.com/vazarkevych/growthbook-c-sharp/issues/74)) ([1f7761f](https://github.com/vazarkevych/growthbook-c-sharp/commit/1f7761f158062d2841ecd0ef7af4b81270fafc3b))
* preserve SSE data following heartbeat comments ([#132](https://github.com/vazarkevych/growthbook-c-sharp/issues/132)) ([5e62be8](https://github.com/vazarkevych/growthbook-c-sharp/commit/5e62be881e34f3cd1e6655af9d57094b5db10967))
* remove subscriptions, add _subscribers and _asyncSubscribers ([b920bd1](https://github.com/vazarkevych/growthbook-c-sharp/commit/b920bd19152f6a301eed0dcb90abb67e9bce4e55))
* removed preview features from code ([eaa6708](https://github.com/vazarkevych/growthbook-c-sharp/commit/eaa67089b7b94f94813ffc7cad57ce54f9464372))
* renamed IGrowthbook to IGrowthBook to match the implementation a… ([3ffd14b](https://github.com/vazarkevych/growthbook-c-sharp/commit/3ffd14b5c4d65eecb7ad3c9af6f96e4962b7b86a))
* renamed IGrowthbook to IGrowthBook to match the implementation and added versioning ([6cdeeed](https://github.com/vazarkevych/growthbook-c-sharp/commit/6cdeeedc2a18f4f6627996b636457586bff74081))
* resolve $inGroup and $notInGroup independently for an absent attribute ([#90](https://github.com/vazarkevych/growthbook-c-sharp/issues/90)) ([a447429](https://github.com/vazarkevych/growthbook-c-sharp/commit/a447429b7ae2809b39f94089bd0b7a27e823896a))
* return the fallback from GetFeatureValue when the value cannot be converted ([#99](https://github.com/vazarkevych/growthbook-c-sharp/issues/99)) ([e7a0ec2](https://github.com/vazarkevych/growthbook-c-sharp/commit/e7a0ec235f42b13431d3bb4deec7e81821ca060d))
* stop concurrent evaluations corrupting sticky buckets and the SSE ([#115](https://github.com/vazarkevych/growthbook-c-sharp/issues/115)) ([909b139](https://github.com/vazarkevych/growthbook-c-sharp/commit/909b13920c3c2a0c3ba6ff0bb9a7daf8a097e248))
* stop writing secrets and payload bodies to the log ([492de61](https://github.com/vazarkevych/growthbook-c-sharp/commit/492de61d9eff3eedb64171caef6a39a523b5957f))
* stop writing secrets and payload bodies to the log ([4c1d7d1](https://github.com/vazarkevych/growthbook-c-sharp/commit/4c1d7d119de7d7a6a4622bc7dc483d1f158456ad))

## [2.1.1](https://github.com/growthbook/growthbook-c-sharp/compare/v2.1.0...v2.1.1) (2026-11-19)


### Bug Fixes

* preserve SSE data following heartbeat comments ([#132](https://github.com/growthbook/growthbook-c-sharp/issues/132)) ([5e62be8](https://github.com/growthbook/growthbook-c-sharp/commit/5e62be881e34f3cd1e6655af9d57094b5db10967))

## [2.1.0] - 2026-11-17

- Added `GrowthBookClient`, which loads features once and evaluates for a `UserContext` passed to each call, so one
  instance serves many users. `UserContext` is now public.
- Added `AddGrowthBookClient` to register `GrowthBookClient` as a singleton with dependency injection. It loads features
  before the first use. The package now depends on `Microsoft.Extensions.DependencyInjection.Abstractions`.
- `GrowthBookFactory` is obsolete. Use `GrowthBookClient` instead.
- `GetFeatureValue` and `GetFeatureValueAsync` return the fallback when the feature's value can't be converted to the
  requested type, instead of throwing. For example, reading `"off"` as a `bool` used to throw `FormatException`.
- `BucketRange`, `Namespace`, `Experiment`, `ExperimentResult` and `FeatureResult` no longer throw from `GetHashCode`,
  so they work in a `HashSet`, as dictionary keys and with `Distinct`. `Equals(null)` returns `false` instead of
  throwing.
- A streaming (SSE) connection that the server closes cleanly now waits before reconnecting, with the same backoff as
  a failed connection. Before, it reconnected immediately in a loop.
- Concurrent evaluations sharing an `InMemoryStickyBucketService` no longer miss saved assignments, and the background
  refresh no longer races `Dispose` over the streaming connection.
- The feature repository takes the cache lock once per call instead of twice.
- Version operators (`$veq`, `$vgt` and the rest) no longer slow down on an attribute with many `+` characters
  followed by a newline. The regex that stripped build metadata backtracked quadratically, taking about 1 s on a
  20,000-character value; it is now a linear scan. Build metadata is now everything after the first `+`, even when a
  newline follows it. Other versions compare as before.
- A version part with a trailing newline, such as the `3\n` in `1.0.0-3\n`, is no longer treated as a number, matching
  the JavaScript SDK. It now sorts as a pre-release tag, so `1.0.0-3\n` is greater than `1.0.0-40`.
- Moved feature and experiment evaluation out of `GrowthBook` into stateless providers. Nothing public changed, but
  three behaviours now match the JavaScript SDK where they previously did not:
  - A forced rule's `tracks` entries are deduplicated along with everything else, instead of calling the tracking
    callback on every evaluation. Applications that count exposures will see fewer events for features that force a
    value and carry tracks. Entries without an experiment or a result are skipped.
  - A sticky bucket assignment is merged with the user's assignments already held in memory and in the service, and
    written back to both, so a second evaluation in the same process finds it instead of re-hashing, and saving one
    experiment no longer drops another's preloaded assignment. Saves for the same user are serialized, so concurrent
    evaluations cannot each write a document missing the other's assignment. Those documents are now held in a
    concurrent dictionary, copied from the one supplied on the `Context`: evaluation writes to them now, and concurrent
    evaluations were losing documents - a lost document means the user is re-hashed, which is what sticky bucketing
    exists to prevent.
  - An experiment's prerequisites are evaluated against a snapshot of the features already being evaluated rather
    than an empty set, so a cycle reached through an experiment is detected.

## [2.0.0] - 2026-10-30

- **Breaking:** condition evaluation is strict about types, matching SDK spec 0.10.0 (growthbook/growthbook#7379).
  When an attribute's type differs from the condition's, the operator fails and its negation (`$ne`, `$nin`, `$vne`,
  `$not`, `$nregex`) matches. Numeric strings are no longer compared as numbers (`"10" < "9"`), date strings are no
  longer parsed as dates (ISO dates still order correctly as text), booleans are never ordered, a missing attribute
  fails `$gt`/`$gte`/`$lt`/`$lte` instead of counting as 0, and version operators need both sides to be strings or
  both numbers. `$exists` and `$type` are unchanged.
- Added the `$savedGroup` operator for saved group references, with typed `list` and `condition` entries.
- **Breaking:** a condition that uses an operator or saved group type this SDK doesn't support no longer passes when
  negated with `$not` or `$nor` (spec 0.11.0, growthbook/growthbook#7346). `$elemMatch` with `$or`, `$and`, `$nor` or
  `$savedGroup` now tests each element's fields, and a malformed condition fails that rule, experiment or prerequisite
  instead of failing the whole evaluation.
- **Breaking:** a percentage rollout no longer includes the user whose hash equals its coverage, so a coverage of
  k/10000 includes exactly k hash buckets (spec 0.11.1, growthbook/growthbook#7425).
- **Breaking:** equal weights and payload bucket ranges are computed in double precision (growthbook-c-sharp#111), so
  users close to a range boundary can be bucketed differently.
- Added `PayloadReducer.ReducePayload` and `PayloadReducer.PruneCondition` to reduce a payload for client-side
  bootstrapping (growthbook/growthbook#7238). As in spec 0.11.3 (growthbook/growthbook#7452), a condition saved group
  that `$elemMatch` or a prerequisite references, or one on a reference cycle, keeps its condition as written; an auto
  experiment keeps its forced variation; a contextual bandit definition that isn't an object is kept as it is; and
  a list saved group's values are never read as references to other groups.
- Updated the standard test cases to spec 0.12.0.
- **Breaking:** only a non-empty string or a finite, non-zero number identifies a user for hashing, as in the JS SDK
  (spec 0.12.0). `0`, `""`, booleans, arrays and objects count as missing, numbers are hashed the way JavaScript
  writes them (`1.0` as `"1"`), and a whitespace-only string is hashed. The fallback attribute is only used along with
  sticky bucketing, now also for percentage rollouts.
- **Breaking:** `$eq` and `$ne` never treat an array or object as equal to anything, `$regex` and `$regexi` fail for a
  pattern that isn't a string, a dot path can index into an array (`"tags.0"`), and `FeatureResult.On` follows
  JavaScript truthiness (`"false"`, `"0"`, `[]` and `{}` are on).
- Fixed `Context.EncryptedFeatures` never being decrypted. The property was declared, documented and cloned, but
  nothing ever read it, so a caller who supplied an encrypted payload was left with an empty feature set and every
  flag reading as false - with nothing to distinguish a wrong key from a working one. The payload is now decrypted in
  the constructor and takes precedence over `Context.Features`. A payload supplied without `Context.DecryptionKey`
  throws `ArgumentException`, and one that will not decrypt or will not parse throws `DecryptionException`, rather
  than failing silently.
- Fixed the async evaluation methods ignoring a locally supplied payload. `IsOnAsync`, `IsOffAsync`, `EvalFeatureAsync`
  and `GetFeatureValueAsync` always loaded from the repository first, so a caller evaluating an offline payload hit an
  API they never configured - with no client key that is a failing request to the default host, and a response that did
  arrive replaced the payload they supplied. The load now happens only when there is somewhere to load from: a client
  key, or a repository the caller provided.
- Stopped writing the decryption key to the log. It was logged verbatim at debug level on both feature fetch paths,
  so anyone with log access could decrypt the payloads it protects. Rotate the key if debug logging has been enabled
  where those logs are retained or shipped elsewhere.
- Stopped logging feature payload bodies, both the raw API response and the decrypted one. A payload carries saved
  groups, which are typically lists of user identifiers, so it is personal data. Character counts are logged instead.
- Fixed deadlocks on single-threaded synchronization contexts (classic ASP.NET on .NET Framework, WPF, WinForms).
  No await in the SDK captures the caller's context any more, across `GrowthBook`, `FeatureRepository`,
  `FeatureRefreshWorker`, `RemoteEvaluationService`, `HttpClientExtensions` and `SSEClient`. An application that
  blocked on an SDK task could previously hang, and the SDK did it to itself through
  `EvalFeature(key, alwaysLoadFeatures: true)` and `GetFeatureValue(key, fallback, alwaysLoadFeatures: true)`,
  which block internally. Those two now also start the load off the calling thread, so a feature repository
  supplied on the `Context` cannot post a continuation back to the thread they are holding.
- **Behaviour change:** code after an await inside the SDK now resumes on a thread pool thread. A `TrackingCallback`
  or a callback registered via `Subscribe` that is reached through the async evaluation APIs (`EvalFeatureAsync`,
  `GetFeatureValueAsync`, `IsOnAsync`, `IsOffAsync`) therefore runs off the caller's synchronization context.
  Callbacks that touch context-affine state — UI controls on WPF/WinForms, `HttpContext.Current` on classic
  ASP.NET — have to marshal back themselves. The synchronous overloads are unaffected: they still invoke callbacks
  on the calling thread. `SubscribeAsync` was already dispatched off-context and is unchanged. No public API changed.

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
