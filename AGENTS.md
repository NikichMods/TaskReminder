# Graveyard Keeper Task Reminder — Working Contract

Follow [NikichMods/DevRules/WORKING_RULES.md](https://github.com/NikichMods/DevRules/blob/main/WORKING_RULES.md) and its action-based reading map. Local checkout work also follows CODEX_WORKFLOW.md. Bootstrap, engineering, runtime, Git, CI and licensing rules apply when entering those subjects.

## Identity and phase

- Project: Task Reminder. Technical identity: TaskReminder. The user confirmed the name on 2026-10-20.
- Repository: [NikichMods/TaskReminder](https://github.com/NikichMods/TaskReminder), public; created by the user and verified through GitHub metadata and Git.
- Initial target: Graveyard Keeper 1.407, Windows PC, BepInEx 5, matching the existing NikichMods research baseline. This project's compatibility has not been tested.
- Phase: documentation and static data research. Native journal/text and six-NPC weekday data paths are mapped; no production code, build, runtime acceptance or release exists.
- Purpose: reduce the need to reopen the NPC task list by displaying a compact task reminder below the left calendar HUD for the current weekday; next-day information is under consideration.

## Sources of truth

- docs/PRODUCT_REQUIREMENTS.md: requested outcome, tentative scope and unresolved product choices.
- docs/DESIGN.md: recovery entry point, proposed direction, research questions and next action.
- docs/VERIFIED_RUNTIME_DATA.md: prior evidence, provenance and applicability limits.
- docs/TASK_DATA_RESEARCH.md: static native journal storage/rendering, weekday mapping and their limits.
- LOCAL_ENVIRONMENT.md: ignored machine-specific paths and tooling/access observations.

Recover actual Git/files before changes. Do not infer acceptance from tentative discussion, compiled code or another mod's accepted behavior.

## Shared research and neighboring projects

Shared host facts belong to [NikichMods/GraveyardKeeperResearch](https://github.com/NikichMods/GraveyardKeeperResearch); start with its AGENTS.md and docs/RESEARCH_INDEX.md. Search accepted shared facts and history before fresh host inspection or probes.

Day Wheel Quest Markers is a related evidence source, not this project's product specification. Its actionable-interaction marker policy must not silently become a filter for this mod's task text. Do not modify that mod or introduce a runtime dependency on it without an agreed design and verified need. Keep product choices and acceptance evidence here; promote reusable new host findings to shared research.

## Product and implementation boundaries

- The requested surface is a small reminder below the left calendar/day wheel. Exact layout and next-day behavior are unselected.
- Do not silently restrict reminders to immediately hand-in-ready tasks, or expand them to undiscovered tasks/dialogue options.
- Automatic native-journal sourcing is a proposal to investigate, not a verified implementation.
- Preserve vanilla task progression, NPC schedules, time, inventory and saves. This bootstrap does not authorize production implementation or gameplay automation.
- Verify native localized task text, current-save ownership, weekday mapping, final layout ownership and refresh/hide lifecycle before production relies on them.
- Prefer a small native HUD extension if evidence supports it. Avoid recurring graph parsing, broad object scans and speculative parallel quest state.
- Do not commit proprietary game binaries, extracted assets, bulk dialogue/localization/decompiled source, personal context or local save/log data.

## Git, builds and acceptance

- Stable/documentation branch: main. Documentation/bootstrap may go directly to main.
- Use codex/research/<topic> for substantial investigation and codex/feature/<topic> for unaccepted runtime work.
- Runtime source reaches main only after explicit acceptance of the exact tested candidate under GIT_WORKFLOW.md.
- No version has been consumed. Define the semantic-version owner and executable build gate with the first real build system, before handing any DLL to the user.
- Canonical installed filename: TaskReminder.dll; numbered handoff: TaskReminder-<version>.dll. Handed artifacts are immutable and recorded with exact source, hash and test result.
- Accepted stable binaries use GitHub Releases; publishing exact accepted bytes follows DevRules acceptance semantics.
- No hosted CI is needed for these documents. Select build tooling, private host references and meaningful checks when executable code exists; do not copy another project's credentials, binaries or CI assumptions.

## Licensing

Original project software and software-supporting documentation use MPL-2.0 under LICENSE and LICENSING.md. Game/third-party rights remain separate. No game material is imported by the documentation bootstrap.
