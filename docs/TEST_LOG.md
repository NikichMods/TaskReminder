# Candidate test log

## 0.1.0 — first native candidate

Status: exact candidate built and installed with the host closed on 2026-10-20. Runtime return/acceptance pending. No accepted stable build exists.

Scope: accepted lightweight current/next-day native-journal reminders and persistent F2 appearance settings. IMPLEMENTATION_GATE.md owns the per-property readiness checkpoint; DEVELOPMENT.md owns checks and the requested user return.

Exact source: `861b4fd2345aadc1cfe443fd64e612bd0c302ca1`, branch `codex/feature/native-reminder`. Immutable numbered artifact: `TaskReminder-0.1.0.dll`, **26,112 bytes**, SHA-256 `1f3de04955e43b76a9eadf4f2222599da1fce391fb9dd99afcce2fa71f545cb2`.

Clean exact-source local Release compile, assembly/file version `0.1.0.0` and actual-assembly policy checks pass. The gate includes native input-family signature checks; it does not prove runtime input suppression. No hosted CI run was used. The ignored build-identity manifest records exact input-reference hashes.

Installed canonical file: `TaskReminder.dll`; installed bytes/hash verified identical to the artifact. No prior project payload existed. Host was closed; no process was launched. Other mods/configuration/saves were not changed. Ignored private installation receipt and preinstall log snapshot are identified in LOCAL_ENVIRONMENT.md.

Requested first return: start/load the game, compare current/next NPC tasks with the journal, tune placement/width/fonts/effects using F2, close and verify controls recover, then restart/reload to verify saved settings. Read the accessible native BepInEx log independently after the user reports the result. Natural completion/day/menu transitions provide follow-up evidence; do not manufacture progression or claim unobserved coverage.

No `TR_READY`, actual rendering, input interaction, settings persistence, language-switch or scene-reload acceptance has yet been observed for these bytes. A later documentation-only commit does not change the artifact's source identity; do not rebuild/replace this numbered DLL to match a bookkeeping HEAD.
