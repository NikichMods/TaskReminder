# Candidate test log

## 0.1.1 — captions, native day icons and lightweight readability

Status: source preparation and development checks passed; exact artifact/install identity to be recorded after the clean build. Native visual acceptance pending.

Scope/gate: VISUAL_REFINEMENT.md. Separate caption size/visibility and independent divider controls with height collapse; native weekday symbols beside NPC names; no glyph effects/soft bands on icons; optional planner-style raster profile, fractional effect offset and narrow measured per-line soft shading. Preserve the player's existing position/width/font/spacing settings. Policy/heading-combination and actual BepInEx old-config migration/save/reload checks pass. No claim that the new text looks clearer until the user compares it in game.

## 0.1.0 — first native candidate

Status: historical candidate, initial installed return observed; not accepted as stable. User requested visual refinements for the next candidate.

Scope: accepted lightweight current/next-day native-journal reminders and persistent F2 appearance settings. IMPLEMENTATION_GATE.md owns the per-property readiness checkpoint; DEVELOPMENT.md owns checks and the requested user return.

Exact source: `861b4fd2345aadc1cfe443fd64e612bd0c302ca1`, branch `codex/feature/native-reminder`. Immutable numbered artifact: `TaskReminder-0.1.0.dll`, **26,112 bytes**, SHA-256 `1f3de04955e43b76a9eadf4f2222599da1fce391fb9dd99afcce2fa71f545cb2`.

Clean exact-source local Release compile, assembly/file version `0.1.0.0` and actual-assembly policy checks pass. The gate includes native input-family signature checks; it does not prove runtime input suppression. No hosted CI run was used. The ignored build-identity manifest records exact input-reference hashes.

Installed canonical file: `TaskReminder.dll`; installed bytes/hash verified identical to the artifact. No prior project payload existed. Host was closed; no process was launched. Other mods/configuration/saves were not changed. Ignored private installation receipt and preinstall log snapshot are identified in LOCAL_ENVIRONMENT.md.

Requested first return: start/load the game, compare current/next NPC tasks with the journal, tune placement/width/fonts/effects using F2, close and verify controls recover, then restart/reload to verify saved settings. Read the accessible native BepInEx log independently after the user reports the result. Natural completion/day/menu transitions provide follow-up evidence; do not manufacture progression or claim unobserved coverage.

Return evidence: native log SHA-256 `b4dfa5cf3f9ad7784a4d240c9fa61872c974de45fb3ee23e4df551ffe6429d84` contains TR_READY with tiny_font/small_font and saved TR_SETTINGS returns (Items12/Outline, then Items13/Shadow). Preserved config SHA-256 `995cacabe88d297f81c8fd21b3d094f605e123d515882b662e42d39c77dabf09`. Screenshot shows rendered Snake objectives and tomorrow's empty Inquisitor block. Actual settings file records X12/Y0/width154/Items13/heading14/spacing-4/Shadow1 at.7. The user reports inadequate caption controls and text legibility; glyph effects on quest symbols are confirmed by the owned-label source path. No isolated outline-geometry bug is established by the screenshot, which uses Shadow. Full input-restoration/restart/language/scene/day-change acceptance is not claimed. Private snapshots are in LOCAL_ENVIRONMENT.md. Keep exact0.1.0 bytes immutable.
