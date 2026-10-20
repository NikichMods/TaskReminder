# Design and Current State

## Recovery entry point

Phase: documentation and static data research. Read AGENTS.md, PRODUCT_REQUIREMENTS.md, VERIFIED_RUNTIME_DATA.md and TASK_DATA_RESEARCH.md before new research. No mod has been implemented, installed or tested.

Project identity is confirmed: Task Reminder / [NikichMods/TaskReminder](https://github.com/NikichMods/TaskReminder), public. The user created the remote; its initial main commit is 08e9edcf69c96712e97f7010bbd3c5536b5bd289, containing the project title and weekday-task description in README.md. Preserve that history. The existing Codex project and checkout are reused. The repository contract, product/design/evidence documents and license form the documentation baseline; no production implementation is implied.

## Why a separate mod

The user asked for a small text reminder below the calendar. Day Wheel Quest Markers is an existing related mod with accepted interaction-marker semantics. Its research can save work, but it does not answer the same product question. Treat this as a separate proposed mod; merging it into the marker mod or requiring that DLL would be a consequential design decision.

## Proposed design direction

The native disclosed-task source is now verified statically: KnownNPC task records expose state/identity and GetTaskText supplies localized formatted text; native calendar conversion and metadata identify the six weekly NPCs. Use this evidence when evaluating a journal-based reminder rather than building a second quest database. Product filtering and the new HUD rendering seam are still unselected/unverified. Prefer native HUD/font/language behavior if the actual lifecycle supports it.

Useful early visual comparison after product discussion:

- current day only, compact NPC heading and task rows;
- current day plus a visually quieter next-day section.

An interactive mockup can compare long text, multiple tasks, an empty day and contrast/density. It is illustrative and must not be described as verified in-game rendering. No UI implementation is authorized by this design note.

## Completed data investigation and its decision value

Question examined: can the native journal supply the currently disclosed, localized task rows and their NPC ownership without parsing dialogue graphs during ordinary gameplay?

Result: yes at the static data level for tasks owned by the six weekday NPCs. TASK_DATA_RESEARCH.md records exact assemblies/resources, storage and native renderer, localization/formatting, native chronological conversion and direct data corroboration. No probe or game-runtime test was needed. Native completion can commit after a UI callback; exact refresh/readiness/placement for the future overlay remains a separate investigation.

This makes a small text view plausible if the requested meaning is tasks belonging to today's NPC, including preparation. Journal ownership alone does not identify the next NPC for intermediate steps or current actionability; those meanings need authored-route semantics. Do not silently replace preparation reminders with actionable interactions merely because the latter already exists.

## Open decisions / next actions

1. Discuss eligibility (tasks belonging to today's NPC versus actions available today), strict Visible versus native non-Complete filtering, next-day default and content size. The user explicitly limited the current work to data research; do not start mod implementation.
2. Present an early mockup when it can inform the adopted scope; then investigate only the remaining native rendering/refresh/readiness boundaries needed by that scope.
3. Only after product selection and the DevRules readiness gate, establish the minimal build and implement a first candidate on a development branch. Promote reusable accepted host findings to GraveyardKeeperResearch through its own safe checkout/workflow; do not disturb its unrelated active research branch.

No release number, plugin GUID, build toolchain, hosted runner, dependency graph or test DLL has been selected. Set those from executable needs rather than scaffolding them speculatively.
