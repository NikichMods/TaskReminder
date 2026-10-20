# Design and Current State

Current phase: prerelease candidate0.1.4 on codex/feature/native-reminder. The user selected the tuned0.1.3 view as final and requested removal of F2. PRERELEASE.md owns this decision, final values, readiness gate and remaining acceptance. TEST_LOG.md owns exact artifact/install identities. No stable promotion/publication yet.

## Data and rendering

The static journal-source hypothesis is supported: GameSave.known_npcs -> KnownNPC.tasks supplies localized, disclosed objectives without gameplay dialogue parsing. TASK_DATA_RESEARCH.md records native storage/rendering and calendar evidence. ReminderPolicy reads explicitly Visible records and maps current/next save day to the six weekday NPCs. Missing inventory never suppresses a reminder.

Plugin checks current-save/player ownership and language readiness, observes native task state changes plus bounded quarter-second lifecycle checks, and redraws only when dirty. ReminderView owns labels below the stable calendar frame; native HUD ancestry supplies scaling/alpha and native menu visibility. TextDecorations measures native processed wraps and owns pooled, separate per-line shading textures. Icons remain separate from text backing. No host font, atlas or quest mutation occurs.

The final profile is manual Journal14/raster24/Outline, names hidden, width130; all saved values are pinned by tests/FinalAppearance.cfg. The optional documented Planner profile remains readable through existing config compatibility, but is off in the selected view. Neither Planner nor the related Day Wheel Quest Markers is a dependency. Original Plan-style alternatives and the interactive mockup are historical design evidence; the lightweight direction and final in-game tuning now govern.

## Player surface and recovery

0.1.4 has no SettingsPanel, OnGUI editor, F2/Esc handler or input Harmony guards. Options binds final defaults and respects existing configuration without saving. The native task-state postfix and renderer lifecycle remain. Editor source can be recovered from68b0abc55f26e173574100c8b9b396f411bd0430 only after a new request; this does not authorize shipping it again now.

Plugin.Version owns0.1.4; assembly/file metadata and immutable filename derive from it. Earlier handed versions remain frozen. build.ps1 performs exact-source SDK6.0.406 Roslyn compile/version/actual-assembly checks using private installed references; no hosted reference provisioning is selected.

## Next useful step

Run the installed0.1.4 with the same save: confirm the accepted view survives removal of the editor, F2 opens no Task Reminder UI, input and native HUD/menu return work. Natural task/day/save transitions then extend functional coverage without reopening accepted styling. A concrete defect may require a new candidate; an explicit stable acceptance later authorizes the normal main/tag/exact-byte Release sequence. DEVELOPMENT.md owns mechanics, TEST_LOG.md the evidence. Reusable host findings remain distinct from this product's acceptance and can be promoted to shared research under its own safe workflow.
