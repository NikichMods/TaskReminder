# Design and Current State

## Recovery entry point

Phase: documentation and static data research. Read AGENTS.md, PRODUCT_REQUIREMENTS.md, VERIFIED_RUNTIME_DATA.md and TASK_DATA_RESEARCH.md before new research. No mod has been implemented, installed or tested.

Project identity is confirmed: Task Reminder / [NikichMods/TaskReminder](https://github.com/NikichMods/TaskReminder), public. The user created the remote; its initial main commit is 08e9edcf69c96712e97f7010bbd3c5536b5bd289, containing the project title and weekday-task description in README.md. Preserve that history. The existing Codex project and checkout are reused. The repository contract, product/design/evidence documents and license form the documentation baseline; no production implementation is implied.

## Why a separate mod

The user asked for a small text reminder below the calendar. Day Wheel Quest Markers is an existing related mod with accepted interaction-marker semantics. Its research can save work, but it does not answer the same product question. Treat this as a separate proposed mod; merging it into the marker mod or requiring that DLL would be a consequential design decision.

## Adopted scope and proposed visual design

The native disclosed-task source is verified statically. The user adopted all known tasks owned by today's NPC, including missing-item preparation tasks, and next day's NPC tasks below. Exact state-edge handling, appearance and the new HUD rendering seam remain unselected/unverified. Prefer native HUD/font/language behavior if the actual lifecycle supports it.

The authorized early mockup compares two presentations of the same adopted content:

- lightweight text with NPC/day headings and no task-block backing;
- two visually separated sections with restrained dark backing for busy terrain.

Interactive preview states cover current weekday, ordinary/many/empty task lists, day/night contrast and density. The calendar and world backdrop are schematic; task fixtures illustrate layout, not the user's live save. Preview settings and variant selection are not adopted in-game controls. The mockup is illustrative and must not be described as verified in-game rendering. No mod UI implementation is authorized by this design note.

Preview names: **Лёгкий текст** and **Две секции**. Both show the same adopted today-then-tomorrow content; the latter separates it with restrained backing and changes heading layout. The ordinary and long-list night variants were inspected in a browser, and offline render/Tweak callback checks cover initial content, week wrap, many tasks, empty today and density/width settings. This verifies only the illustrative preview. Its durable private path is recorded in LOCAL_ENVIRONMENT.md; no appearance has yet been accepted by the user.

## Completed data investigation and its decision value

Question examined: can the native journal supply the currently disclosed, localized task rows and their NPC ownership without parsing dialogue graphs during ordinary gameplay?

Result: yes at the static data level for tasks owned by the six weekday NPCs. TASK_DATA_RESEARCH.md records exact assemblies/resources, storage and native renderer, localization/formatting, native chronological conversion and direct data corroboration. No probe or game-runtime test was needed. Native completion can commit after a UI callback; exact refresh/readiness/placement for the future overlay remains a separate investigation.

This makes a small text view plausible if the requested meaning is tasks belonging to today's NPC, including preparation. Journal ownership alone does not identify the next NPC for intermediate steps or current actionability; those meanings need authored-route semantics. Do not silently replace preparation reminders with actionable interactions merely because the latter already exists.

## Open decisions / next actions

1. Review the early mockup for task width, separation of today/tomorrow, contrast and long-list behavior. Eligibility and inclusion of next day are settled; do not reopen them without new evidence/user direction. Strict Visible versus stored Unknown remains an edge decision.
2. After adopting visual scope, investigate only the remaining native rendering/refresh/readiness boundaries needed by it. The current authorization is mockup only; do not start mod implementation.
3. Only after product selection and the DevRules readiness gate, establish the minimal build and implement a first candidate on a development branch. Promote reusable accepted host findings to GraveyardKeeperResearch through its own safe checkout/workflow; do not disturb its unrelated active research branch.

No release number, plugin GUID, build toolchain, hosted runner, dependency graph or test DLL has been selected. Set those from executable needs rather than scaffolding them speculatively.
