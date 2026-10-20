# Candidate test log

## 0.1.3 — coordinated appearance

Status: development compile/actual-assembly checks pass; exact clean-source build/install identity follows. Gate: VISUAL_REFINEMENT.md. All preceding artifacts remain immutable; no stable promotion.

Tests exercise persisted new settings, reversible manual/Planner profile values, actual ConfigFile snapshot rollback/isolated resets, root scale at simulated HUD .75/1/1.1/2, rejection of invalid/rotated coordinate bases and previous native weekday/task/header/input signatures. Runtime symbol wrapping/shadows, name-row collapse, actual coordinate placement and perceived two-block match remain unobserved.

Requested targeted return: toggle Planner style on/off beside the real right block; compare size/readability and retained manual style. Hide names/day symbols, enlarge quest circles and toggle their shadows. Change whole-block scale with long wrapped text and verify lines/backing/anchor stay coherent. Esc should restore these controls; F2 should save. Existing gameplay eligibility remains unchanged and needs no repeated broad matrix for this downstream change.

## 0.1.2 — settings refinement

Status: historical candidate loaded and reached TR_READY; settings saves at X10/Items14/Outline/shadeY-3 observed. Preserved return log SHA-256 3b1b6c7d10a33022e740b62e0f3174db1b5dd9d55b91ce84de8e3e7c5ff41c7e; config SHA-256 dabb2e7c11f1ef9e41ad6c48a847f13293e3bb6e9620a106643a693c65651a14. User requests further controls; full native input/cancel/restart/visual acceptance remains pending. Gate: VISUAL_REFINEMENT.md.

Exact source a979e2382a390e87b9afb815c3528a28fb6bf614. Immutable TaskReminder-0.1.2.dll, 35,328 bytes, SHA-256 dd56a2ea476f1ccd5b03e9cda6bcc6ff980c4ceb2af7b4b40c74428070dfb97c. Clean exact-source Release compile/version0.1.2.0/actual-assembly checks passed. Installed canonical TaskReminder.dll matches that hash; one project payload. Previous0.1.1/config backed up outside the load directory. Config SHA-256 774ad2fc54220903a95c5c99cbea417a25ec284d25be8e33f84a0a4929325732 remained unchanged. No game launched, saves or other mods changed; no0.1.2 runtime observation claimed.

Checks: actual BepInEx config snapshot restores every entry including toggles/font/effect, cancel leaves file bytes unchanged, saved caption/Y-offset survive reload, individual numeric resets leave other entries intact, reopening uses latest saved values. Existing native weekday/task/header/input-signature checks pass. Unity panel controls, vertical shade placement and input restoration still need the installed return.

Requested return: move soft backing both ways; reset one integer, percentage and effect-size slider; change several controls then Esc and reopen to verify rollback. Repeat with F2 to verify persistence and movement/click recovery.

## 0.1.1 — captions, native day icons and lightweight readability

Status: historical exact candidate loaded and reached TR_READY; settings save observed. Full native visual/input/lifecycle acceptance remains pending.

Exact source: `28fa0206decf87a117893bf67395e9c37575fd8b`. Immutable `TaskReminder-0.1.1.dll`, **34,304 bytes**, SHA-256 `3a0cf41ed23d8324d8a79eb238cabded9642490c244bd8a556535736457fe71e`. Clean Release compile, assembly/file version0.1.1.0 and the expanded actual-assembly/header/config migration gate pass. No hosted CI run.

Installed as canonical TaskReminder.dll with identical hash; one active project payload. Previous0.1.0 is backed up outside the plugin directory. Existing config digest remains `995cacabe88d297f81c8fd21b3d094f605e123d515882b662e42d39c77dabf09`; installation did not reset or write it. New controls use defaults on first load and persist when F2 closes. Installation launched no game and changed no other mod/save. Later native log contains version0.1.1 TR_READY and TR_SETTINGS. Preserved return log SHA-256 ef906fd1c7c78dc5ff8106c4bac78651c2f7fa65c35a2243bd59be964b774b47; settings SHA-256 774ad2fc54220903a95c5c99cbea417a25ec284d25be8e33f84a0a4929325732. This proves bounded load/settings-write, not full acceptance.

Requested return: compare the added native day symbols and clean quest markers; toggle day captions and lines independently to check height reclamation; adjust separate caption size. Compare the planner-style profile and per-line soft shade on/off at the preserved size/width. Shade should follow each wrapped line without covering icons or forming one block. Tune intensity/padding/feather in F2. Read the accessible log after the user reports the visual result; do not replay unrelated gameplay matrices.

Scope/gate: VISUAL_REFINEMENT.md. Separate caption size/visibility and independent divider controls with height collapse; native weekday symbols beside NPC names; no glyph effects/soft bands on icons; optional planner-style raster profile, fractional effect offset and narrow measured per-line soft shading. Preserve the player's existing position/width/font/spacing settings. Policy/heading-combination and actual BepInEx old-config migration/save/reload checks pass. No claim that the new text looks clearer until the user compares it in game.

## 0.1.0 — first native candidate

Status: historical candidate, initial installed return observed; not accepted as stable. User requested visual refinements for the next candidate.

Scope: accepted lightweight current/next-day native-journal reminders and persistent F2 appearance settings. IMPLEMENTATION_GATE.md owns the per-property readiness checkpoint; DEVELOPMENT.md owns checks and the requested user return.

Exact source: `861b4fd2345aadc1cfe443fd64e612bd0c302ca1`, branch `codex/feature/native-reminder`. Immutable numbered artifact: `TaskReminder-0.1.0.dll`, **26,112 bytes**, SHA-256 `1f3de04955e43b76a9eadf4f2222599da1fce391fb9dd99afcce2fa71f545cb2`.

Clean exact-source local Release compile, assembly/file version `0.1.0.0` and actual-assembly policy checks pass. The gate includes native input-family signature checks; it does not prove runtime input suppression. No hosted CI run was used. The ignored build-identity manifest records exact input-reference hashes.

Installed canonical file: `TaskReminder.dll`; installed bytes/hash verified identical to the artifact. No prior project payload existed. Host was closed; no process was launched. Other mods/configuration/saves were not changed. Ignored private installation receipt and preinstall log snapshot are identified in LOCAL_ENVIRONMENT.md.

Requested first return: start/load the game, compare current/next NPC tasks with the journal, tune placement/width/fonts/effects using F2, close and verify controls recover, then restart/reload to verify saved settings. Read the accessible native BepInEx log independently after the user reports the result. Natural completion/day/menu transitions provide follow-up evidence; do not manufacture progression or claim unobserved coverage.

Return evidence: native log SHA-256 `b4dfa5cf3f9ad7784a4d240c9fa61872c974de45fb3ee23e4df551ffe6429d84` contains TR_READY with tiny_font/small_font and saved TR_SETTINGS returns (Items12/Outline, then Items13/Shadow). Preserved config SHA-256 `995cacabe88d297f81c8fd21b3d094f605e123d515882b662e42d39c77dabf09`. Screenshot shows rendered Snake objectives and tomorrow's empty Inquisitor block. Actual settings file records X12/Y0/width154/Items13/heading14/spacing-4/Shadow1 at.7. The user reports inadequate caption controls and text legibility; glyph effects on quest symbols are confirmed by the owned-label source path. No isolated outline-geometry bug is established by the screenshot, which uses Shadow. Full input-restoration/restart/language/scene/day-change acceptance is not claimed. Private snapshots are in LOCAL_ENVIRONMENT.md. Keep exact0.1.0 bytes immutable.
