# Product Requirements

## Requested outcome

Source: the user's initial request on 2026-10-20 (Europe/Moscow), accompanied by an image of the native NPC task interface.

The player should be able to glance at a small reminder on the left, below the calendar/day-and-week HUD, and see task information relevant to the current in-game weekday. Showing the next day's tasks is a tentative extension, not a settled default.

The image establishes the kind of information the user means: NPC-associated task text, including item hand-ins and non-item objectives. It does not establish internal task identifiers, weekday mappings, item quality rules, live quantities or a completed UI design. Do not publish the reference image as a project-owned asset.

## Authorized current work

Inspect NikichMods rules, related projects and shared research, then prepare the infrastructure for a separate mod using established conventions. The immediate scope is bootstrap and research preparation; production implementation is a later step.

## Requested foundation

- A compact task-information display below the left calendar HUD.
- Current-weekday relevance.
- A small mod focused on reminders rather than a broader planner.

## Tentative / unresolved

1. **Task eligibility:** all active journal tasks associated with the day's NPC, only currently actionable hand-ins/conversations, or another deliberate rule? The user's wording does not select a readiness-only filter.
2. **Next day:** whether to include it, whether it starts visible and how much space it receives.
3. **Content detail:** full native wording versus shortened text; whether to show carried/required counts, quality, readiness or only the objective. These would require separate verified data paths.
4. **Coverage:** multiple simultaneous tasks, non-item objectives, DLC, tasks involving another NPC, NPCs without a weekly visit, and any non-task interactions.
5. **Presentation:** wrapping, height/overflow, empty-day behavior, font/contrast and coexistence with other HUD mods.
6. **Input/configuration:** passive display versus hide/collapse controls; keyboard and controller expectations.

These are open decisions, not a list of promised features. Investigate native data availability first; resolve consequential choices before implementing them.

## Adopted project identity

The user confirmed Task Reminder / NikichMods/TaskReminder, public, on 2026-10-20 and created the GitHub repository. The existing local Codex project and checkout are reused.

## Proposed starting direction — not adopted

Use already disclosed native journal tasks as the source, grouped by their verified relationship to the current weekday. This would help the player prepare for a visit even when the requested items are missing. Give the current day priority; evaluate a quieter next-day section in an early mockup.

Avoid copying Day Wheel Quest Markers' readiness-only policy into this design without explicit agreement: an interaction marker answers whether a visit can progress now, while a preparation reminder may need to remain visible while resources are missing.

## Preserved behavior

No changes to native progression, NPC schedules, inventory, time or saves are requested. Auto-completion, remote hand-ins, a manual task editor, discovery of undisclosed future objectives and a hard dependency on another mod are not authorized by the bootstrap request.

## Future acceptance envelope

Once eligibility and display choices are adopted, exercise representative real paths: task creation/completion, weekday transition including week wrap, save/load and switching saves, hidden/restored HUD, long/multiple objectives and coexistence with the user's calendar markers. Compilation or an illustrative mockup alone cannot establish these runtime properties.
