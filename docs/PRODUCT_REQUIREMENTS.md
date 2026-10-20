# Product Requirements

## Requested outcome

Source: the user's initial request on 2026-10-20 (Europe/Moscow), accompanied by an image of the native NPC task interface.

The player should be able to glance at a small reminder on the left, below the calendar/day-and-week HUD, and see all known tasks of the current weekday's NPC, followed below by those of the next weekday's NPC. Both sections and preparation-task inclusion were adopted on 2026-10-20.

The image establishes the kind of information the user means: NPC-associated task text, including item hand-ins and non-item objectives. It does not establish internal task identifiers, weekday mappings, item quality rules, live quantities or a completed UI design. Do not publish the reference image as a project-owned asset.

## Authorized current work

Inspect NikichMods rules, related projects and shared research, then prepare the infrastructure for a separate mod using established conventions. The immediate scope is bootstrap and research preparation; production implementation is a later step.

Follow-up on 2026-10-20: investigate how the game stores/displays tasks and relates them to weekdays, explicitly without implementing the mod. Static findings are recorded in TASK_DATA_RESEARCH.md; they inform the choices below without adopting a filter or UI design.

Subsequent decision on the same date: show all known tasks of the current day's NPC even when items are missing, with next day's NPC tasks below. The user authorized an early visual mockup only; production implementation remains deferred.

Later on the same date the user accepted **Лёгкий текст** and authorized its in-game implementation. A persistent F2 panel must adjust X/Y position, text size, native font choice where supported, shadow/outline and block width live. Task/section spacing and tomorrow contrast are useful bounded tuning controls. Save the plugin's own settings without changing game saves. The first installed candidate remains subject to runtime acceptance.

Visual return/refinement: independently tune and hide Today/Tomorrow text, add independently switchable/tunable horizontal lines, and reclaim hidden caption space. Hiding caption text must not hide enabled lines; hiding both consumes no caption row. Add native NPC weekday symbols. Glyph shadow/outline applies only to text, never quest/day symbols. Evaluate optional narrow, softly edged backing under each actual wrapped text line; preserve the lightweight presentation and avoid a whole-block backing. Preserve existing user tuning and expose comparisons in F2. VISUAL_REFINEMENT.md owns evidence and limits.

## Requested foundation

- A compact task-information display below the left calendar HUD.
- Current-weekday relevance.
- A small mod focused on reminders rather than a broader planner.

## Tentative / unresolved

1. **State edge:** strict Visible versus exact native non-Complete parity for a stored Unknown record; the all-known-task decision does not require exposing undisclosed objectives.
2. **Content detail:** full native wording versus shortened text; whether to add carried/required counts, quality or readiness details. These would require separate verified data paths and must not filter out missing-item tasks.
3. **Coverage:** non-weekly NPCs and non-task interactions are not automatically included. Known tasks of each selected NPC can include multiple simultaneous and non-item objectives; cross-NPC rerouting is not adopted.
4. **Presentation:** wrapping, height/overflow, empty-day behavior, font/contrast and coexistence with other HUD mods.
5. **Input/configuration:** passive display versus hide/collapse controls; keyboard and controller expectations.

These are open decisions, not a list of promised features. Investigate native data availability first; resolve consequential choices before implementing them.

## Adopted project identity

The user confirmed Task Reminder / NikichMods/TaskReminder, public, on 2026-10-20 and created the GitHub repository. The existing local Codex project and checkout are reused.

## Adopted reminder semantics

Use already disclosed native journal tasks owned by the current weekday's NPC, including preparation tasks with missing items. Show the next weekday's NPC tasks below. This is not an actionable-interaction-only list, and the missing-item case must not suppress a task. Exact appearance and empty-day behavior remain proposals for review.

Avoid copying Day Wheel Quest Markers' readiness-only policy into this design without explicit agreement: an interaction marker answers whether a visit can progress now, while a preparation reminder may need to remain visible while resources are missing.

## Preserved behavior

No changes to native progression, NPC schedules, inventory, time or saves are requested. Auto-completion, remote hand-ins, a manual task editor, discovery of undisclosed future objectives and a hard dependency on another mod are not authorized by the bootstrap request.

## Future acceptance envelope

Once eligibility and display choices are adopted, exercise representative real paths: task creation/completion, weekday transition including week wrap, save/load and switching saves, hidden/restored HUD, long/multiple objectives and coexistence with the user's calendar markers. Compilation or an illustrative mockup alone cannot establish these runtime properties.

## Adopted settings refinement (0.1.2)

Soft per-line backing can move vertically without moving text. Every slider has an individual reset to the shipped0.1.1 defaults; caption default is fixed at fresh-install16, independent of tuned body size. Current saved values are preserved. All changes preview live: explicit Save/F2 persists; Cancel/Esc restores the entire opening state, including toggles and font. Forced panel closure also discards edits.
