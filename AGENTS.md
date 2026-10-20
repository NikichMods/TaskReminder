# Graveyard Keeper Task Reminder — Working Contract

Follow [NikichMods/DevRules/WORKING_RULES.md](https://github.com/NikichMods/DevRules/blob/main/WORKING_RULES.md) and its action-based reading map. Local checkout work also follows CODEX_WORKFLOW.md. Bootstrap, engineering, runtime, Git, CI and licensing rules apply when entering those subjects.

## Identity and phase

- Project: Task Reminder. Technical identity: TaskReminder. The user confirmed the name on 2026-10-20.
- Repository: [NikichMods/TaskReminder](https://github.com/NikichMods/TaskReminder), public; created by the user and verified through GitHub metadata and Git.
- Initial target: Graveyard Keeper 1.407, Windows PC, BepInEx 5, matching the existing NikichMods research baseline. This project's compatibility has not been tested.
- Phase: coordinated appearance candidate0.1.3 on codex/feature/native-reminder after bounded0.1.0/0.1.1 load/render/settings returns. VISUAL_REFINEMENT.md owns the new gate. Runtime acceptance and stable release remain pending; docs/TEST_LOG.md owns exact handed/installed artifact state.
- Purpose: reduce the need to reopen the NPC task list by displaying all known tasks of the current weekday's NPC below the left calendar HUD, followed by the next weekday's NPC tasks, including tasks whose items are still missing.

## Sources of truth

- docs/PRODUCT_REQUIREMENTS.md: requested outcome, tentative scope and unresolved product choices.
- docs/DESIGN.md: recovery entry point, proposed direction, research questions and next action.
- docs/VERIFIED_RUNTIME_DATA.md: prior evidence, provenance and applicability limits.
- docs/TASK_DATA_RESEARCH.md: static native journal storage/rendering, weekday mapping and their limits.
- docs/IMPLEMENTATION_GATE.md: initial readiness evidence and first-candidate acceptance boundaries.
- docs/DEVELOPMENT.md and docs/TEST_LOG.md: build/install/return recovery and exact artifact identity.
- LOCAL_ENVIRONMENT.md: ignored machine-specific paths and tooling/access observations.

Recover actual Git/files before changes. Do not infer acceptance from tentative discussion, compiled code or another mod's accepted behavior.

## Shared research and neighboring projects

Shared host facts belong to [NikichMods/GraveyardKeeperResearch](https://github.com/NikichMods/GraveyardKeeperResearch); start with its AGENTS.md and docs/RESEARCH_INDEX.md. Search accepted shared facts and history before fresh host inspection or probes.

Day Wheel Quest Markers is a related evidence source, not this project's product specification. Its actionable-interaction marker policy must not silently become a filter for this mod's task text. Do not modify that mod or introduce a runtime dependency on it without an agreed design and verified need. Keep product choices and acceptance evidence here; promote reusable new host findings to shared research.

## Product and implementation boundaries

- The adopted surface is lightweight text below the left calendar/day wheel: current day's NPC first, next day's NPC below. Both sections, missing-item task inclusion and the lightweight mockup were adopted on 2026-10-20. In-game visual values remain subject to F2 tuning and user acceptance.
- Do not silently restrict reminders to immediately hand-in-ready tasks, or expand them to undiscovered tasks/dialogue options.
- Use the statically verified native-journal path, explicitly Visible records only; Complete and Unknown records are excluded. Missing NPCs produce an unnamed empty section, without disclosing objectives.
- Preserve vanilla task progression, NPC schedules, time, inventory and saves. The user authorized a presentation mod and persistent F2 settings, not gameplay automation.
- Verify native localized task text, current-save ownership, weekday mapping, final layout ownership and refresh/hide lifecycle before production relies on them.
- Prefer a small native HUD extension if evidence supports it. Avoid recurring graph parsing, broad object scans and speculative parallel quest state.
- Do not commit proprietary game binaries, extracted assets, bulk dialogue/localization/decompiled source, personal context or local save/log data.

## Git, builds and acceptance

- Stable/documentation branch: main. Documentation/bootstrap may go directly to main.
- Use codex/research/<topic> for substantial investigation and codex/feature/<topic> for unaccepted runtime work.
- Runtime source reaches main only after explicit acceptance of the exact tested candidate under GIT_WORKFLOW.md.
- Current candidate version0.1.3, owned by Plugin.Version; assembly/file metadata derive from it.0.1.0 through 0.1.2 are frozen. build.ps1 performs the clean exact-source SDK6.0.406 Roslyn compile/version/actual-assembly policy/config migration gate using private installed references. Never overwrite a handed numbered artifact. No hosted CI/reference provisioning is selected.
- Canonical installed filename: TaskReminder.dll; numbered handoff: TaskReminder-<version>.dll. Handed artifacts are immutable and recorded with exact source, hash and test result.
- Accepted stable binaries use GitHub Releases; publishing exact accepted bytes follows DevRules acceptance semantics.
- No hosted CI is needed for these documents. Select build tooling, private host references and meaningful checks when executable code exists; do not copy another project's credentials, binaries or CI assumptions.

## Licensing

Original project software and software-supporting documentation use MPL-2.0 under LICENSE and LICENSING.md. Game/third-party rights remain separate. No game material is imported by the documentation bootstrap.
