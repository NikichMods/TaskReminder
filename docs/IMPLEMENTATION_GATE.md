> Historical candidate gates. The current final-appearance/no-editor prerelease decision is owned by [PRERELEASE.md](PRERELEASE.md); earlier F2 tuning requirements below are superseded for the player candidate.

# First native candidate — readiness checkpoint

The user accepted **Лёгкий текст** and authorized in-game implementation plus persistent F2 controls on 2026-10-20. The preview's compact/260 setting is an initial tuning reference, not verified Unity geometry.

## Read-only task view — READY for first candidate, runtime acceptance pending

- Property: render the disclosed Visible tasks of today's known NPC, then tomorrow's, without item-readiness filtering. Unknown/Complete records are excluded; no native task mutation. A missing/unmet NPC shows an empty section without revealing its name.
- Owner: current GameSave.known_npcs; KnownNPC/TaskState fields and native GetTaskText; native day conversion documented in TASK_DATA_RESEARCH.md.
- Commit point: KnownNPC.SetQuestState postfix observes the actual write (including delayed completion callbacks), not just entry/return of GameSave.SetTaskState. Save identity, weekday and GJL language are checked at a bounded quarter-second lifecycle tick. Current-save player inventory reference ownership gates activation.
- Blast radius: own text objects only; task postfix only marks this view dirty. No graph parser, inventory query, save writer or other mod dependency.
- Invariants: native task/order/text/localization semantics and progression stay game-owned; no undisclosed future objectives or ready-only filtering.
- Acceptance: actual saved tasks versus journal, day/next-day sequence, completion disappearance, save reload and menu hide/restore. Pure selection/read-only behavior receives offline checks against actual game task records.

## Native text placement and typography — READY for candidate/calibration, visual acceptance pending

- Property: borderless text below the left calendar with adjustable position/width/font/spacing/effects.
- Owner: installed HUD.sins_circle has a 3x3 holder; its exact child `test circle (1)` is the centered 120x120 UI2DSprite. Native prefab inspected read-only. Use its native extents in the stable parent frame; ignore the holder's transient rotation during day tween so the reminder does not orbit/jitter.
- Final consumer: owned UILabels under the existing HUD transform/UIPanel, sharing native alpha/active lifecycle and HUD scaling. Reposition in LateUpdate using stable native coordinates. Native ResizeHeight/ProcessText owns wrapping and measured row height; no fixed one-line clipping or font shrinking.
- Fonts: native journal task label references resources UIFont151302; craft-item label references UIFont151301, both nominal16. Copy only presentation fields from these verified templates, then apply native GJL font correction with uncached original-template state before user size/spacing overrides. Different nominal sizes are not a screen-pixel guarantee; CJK may map both to one native language font.
- Effects: native UILabel None/Shadow/Outline with adjustable dark effect opacity/distance. New labels have no colliders, event handlers or independent panel alpha.
- Invariants: do not move native calendar/other HUD elements, clone live dialog state, or override another mod's panel. Missing required anchor/template fails closed once.
- Acceptance: user positions/reads real long tasks in game via F2; representative native menu/cinematic alpha, resolution and language transitions remain first-candidate runtime checks.

## Persistent F2 panel — READY for first candidate, runtime acceptance pending

- Property: requested live controls; changes saved to the plugin's own BepInEx config on close, with restore-defaults and close controls. No save data or native time settings are written.
- Owner/consumer: own Unity IMGUI window on the right, native Input F2; verified BaseCharacterComponent.control_enabled and the HUD's UICamera mouse/touch/keyboard/controller flags guard gameplay while open. Capture and restore the exact original values.
- Integration evidence: existing Building Planner calibration uses this bounded guard plus LazyInput bool/direction suppression. The first candidate uses that same verified method family only while its own panel is open; closes before native windows/loading and on disable/destroy/failure. This is persistent user configuration as explicitly requested, not a temporary probe copied into a stable release.
- Blast radius: host input is suppressed only while this panel is open; all guard state is restored on close. Time continues natively. Other mods using raw Unity F2 can still conflict; do not claim exclusive key ownership.
- Acceptance: F2 opens/closes, arrows/sliders update live, world clicks/movement do not leak, closing restores input, and settings survive a full restart. No native window is opened to query task data.

## Candidate identity and build gate

Initial candidate 0.1.0. Canonical version owner: Plugin.Version; assembly/file metadata derive from it. Installed filename TaskReminder.dll. Clean exact-source local Release compile with SDK6.0.406 Roslyn and private installed references, version checks and meaningful selection tests precede immutable numbered handoff. No host libraries/assets are redistributed. No stable promotion or Release before explicit runtime acceptance.
