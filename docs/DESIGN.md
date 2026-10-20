# Design and Current State

## Recovery entry point

Phase: documentation/bootstrap. Read AGENTS.md, PRODUCT_REQUIREMENTS.md and VERIFIED_RUNTIME_DATA.md before new research. No mod has been implemented, installed or tested.

Project identity is confirmed: Task Reminder / [NikichMods/TaskReminder](https://github.com/NikichMods/TaskReminder), public. The user created the remote; its initial main commit is 08e9edcf69c96712e97f7010bbd3c5536b5bd289, containing the project title and weekday-task description in README.md. Preserve that history. The existing Codex project and checkout are reused. The repository contract, product/design/evidence documents and license form the documentation baseline; no production implementation is implied.

## Why a separate mod

The user asked for a small text reminder below the calendar. Day Wheel Quest Markers is an existing related mod with accepted interaction-marker semantics. Its research can save work, but it does not answer the same product question. Treat this as a separate proposed mod; merging it into the marker mod or requiring that DLL would be a consequential design decision.

## Proposed design direction

Use native disclosed tasks rather than maintain a second quest database, provided the host exposes the needed text/state/weekday relationship cleanly. Prefer native HUD/font/language behavior if the actual lifecycle supports it. Neither the source path nor the rendering seam has yet been verified for this project.

Useful early visual comparison after product discussion:

- current day only, compact NPC heading and task rows;
- current day plus a visually quieter next-day section.

An interactive mockup can compare long text, multiple tasks, an empty day and contrast/density. It is illustrative and must not be described as verified in-game rendering. No UI implementation is authorized by this design note.

## Next investigation and its decision value

Question: can the native journal supply the currently disclosed, localized task rows and their NPC ownership without parsing dialogue graphs during ordinary gameplay?

Start with accepted shared task/HUD evidence and the marker mod's current source. Then inspect the narrow native journal rendering path if still necessary. Identify task state and localized row-text owners, NPC-to-weekday ownership, refresh boundaries and current-save readiness. Do not create a new probe until direct evidence is insufficient.

If this native path is sufficient, a small text-view extension may satisfy the goal with little duplicated state. If task text or day relevance depends on authored dialogue beyond journal records, surface the extra scope before selecting a classifier or importing marker architecture. Do not silently replace preparation reminders with actionable interactions merely because the latter already exists.

## Open decisions / next actions

1. Investigate the bounded native journal path described above; record derived facts and their limits.
2. Discuss eligibility (preparation versus ready-to-act), next-day default and content size; present an early mockup when it can inform these choices.
3. Only after product selection and the DevRules readiness gate, establish the minimal build and implement a first candidate on a development branch.

No release number, plugin GUID, build toolchain, hosted runner, dependency graph or test DLL has been selected. Set those from executable needs rather than scaffolding them speculatively.
