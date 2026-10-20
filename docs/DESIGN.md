# Design and Current State

## Recovery entry point

Phase: first native candidate 0.1.0 on codex/feature/native-reminder. The user accepted **Лёгкий текст** and authorized in-game implementation plus persistent F2 tuning. Source and development compile/policy checks exist; exact build/install/return identity is owned by TEST_LOG.md. Runtime acceptance and stable promotion remain pending. Start DEVELOPMENT.md for the current return procedure, then the owning requirements/evidence as needed.

Project identity is confirmed: Task Reminder / [NikichMods/TaskReminder](https://github.com/NikichMods/TaskReminder), public. The user created the remote; its initial main commit is 08e9edcf69c96712e97f7010bbd3c5536b5bd289, containing the project title and weekday-task description in README.md. Preserve that history. The existing Codex project and checkout are reused. The repository contract, product/design/evidence documents and license form the documentation baseline; no production implementation is implied.

## Why a separate mod

The user asked for a small text reminder below the calendar. Day Wheel Quest Markers is an existing related mod with accepted interaction-marker semantics. Its research can save work, but it does not answer the same product question. Treat this as a separate proposed mod; merging it into the marker mod or requiring that DLL would be a consequential design decision.

## Adopted scope and proposed visual design

The user adopted all known tasks owned by today's NPC, including missing-item preparation tasks, and next day's below. Known means explicitly Visible in the first implementation; Complete/Unknown are excluded to preserve disclosed-objective semantics. The user accepted the lightweight preview and requested native live tuning. IMPLEMENTATION_GATE.md records the verified native seam/first-candidate bounds; actual visual and input behavior is awaiting the installed return.

The authorized early mockup compares two presentations of the same adopted content:

- lightweight text with NPC/day headings and no task-block backing;
- two visually separated sections with restrained dark backing for busy terrain.

Historical preview states cover current weekday, ordinary/many/empty task lists, day/night contrast and density. Its calendar/world backdrop and some fixture texts are schematic. The user selected **Лёгкий текст**, then separately authorized in-game implementation and persistent F2 appearance controls. Browser rendering is not installed-game visual acceptance.

Preview names: **Лёгкий текст** (selected) and **Две секции** (not selected). Preview checks remain historical illustration evidence; the private path is in LOCAL_ENVIRONMENT.md. Native sizes/widths are initial values to tune through F2, not an assertion of pixel equivalence to the browser.

## Completed data investigation and its decision value

Question examined: can the native journal supply the currently disclosed, localized task rows and their NPC ownership without parsing dialogue graphs during ordinary gameplay?

Result: yes at the static data level for tasks owned by the six weekday NPCs. TASK_DATA_RESEARCH.md records exact assemblies/resources, storage and native renderer, localization/formatting, native chronological conversion and direct data corroboration. No probe or game-runtime test was needed. Native completion can commit after a UI callback; exact refresh/readiness/placement for the future overlay remains a separate investigation.

This makes a small text view plausible if the requested meaning is tasks belonging to today's NPC, including preparation. Journal ownership alone does not identify the next NPC for intermediate steps or current actionability; those meanings need authored-route semantics. Do not silently replace preparation reminders with actionable interactions merely because the latter already exists.

## Open decisions / next actions

1. Finish/return the exact 0.1.0 candidate under TEST_LOG.md; evaluate native placement/text/F2 controls and persistence. Compilation is not runtime acceptance.
2. Read the accessible installed log after the return; fix concrete defects or preserve accepted tuning values in the next immutable candidate. Eligibility, both day sections and lightweight direction are settled.
3. Promote reusable accepted host findings to GraveyardKeeperResearch through its own safe checkout/workflow; do not disturb its unrelated active research branch. Stable promotion requires explicit acceptance of the exact tested artifact.

Plugin GUID: nikich.gyk.taskreminder. Version owner: Plugin.Version (0.1.0). build.ps1 owns the clean exact-source SDK6.0.406 Roslyn gate against private installed references and actual-assembly policy checks. No hosted runner/reference provisioning is selected. DEVELOPMENT.md owns build instructions; TEST_LOG.md owns immutable artifact/source identity.
