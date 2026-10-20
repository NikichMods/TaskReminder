# Evidence and Applicability

## Bootstrap evidence

Inspected on 2026-10-20 (Europe/Moscow). These are existing project/shared findings, not new runtime acceptance of Task Reminder.

| Source | Inspected local revision | Relevant state |
| --- | --- | --- |
| NikichMods/DevRules | f7c8c3c78b349c5c0b0ec4ebfe34c77d74157d60 | Clean main at inspection; bootstrap/engineering/Git/CI/licensing contracts consulted. |
| NikichMods/GraveyardKeeperResearch | 4d1725a02dc78fc0951022a65a1c1194f74fe497 | Clean research/carried-bag-inventory at inspection; shared canonical task/HUD docs read. This is an inspected source identity, not a claim that every branch finding is accepted. |
| NikichMods/DayWheelQuestMarkers | bc31dc8c3de69184c12efbf2afc037e70f828a70 | Clean main at inspection; AGENTS.md identifies accepted stable 1.1.16 and its exact runtime evidence. GitHub metadata independently confirms public repository/main and connector write access. |

Other local project contracts consulted for conventions: ResearchableAlchemyRecipes and Building Planner. Their private work, mechanics, candidate versions and runtime assumptions are not imported.

The user subsequently confirmed the project name and created NikichMods/TaskReminder. GitHub metadata verifies public visibility, main and connector write access; a live Git fetch verifies initial main 08e9edcf69c96712e97f7010bbd3c5536b5bd289 with only README.md. The initial title/description agree with the requested weekday-task reminder. That existing history is retained.

## Established shared facts for Graveyard Keeper 1.407

Canonical owner: [DIALOGUE_QUEST_AND_FLOWCANVAS.md](https://github.com/NikichMods/GraveyardKeeperResearch/blob/4d1725a02dc78fc0951022a65a1c1194f74fe497/docs/DIALOGUE_QUEST_AND_FLOWCANVAS.md).

- The inspected calendar root is `UI Root/HUD/hud left/hud spr/circle`.
- Physical weekday-symbol positions are fixed while their semantic weekdays change; `HUDSinIcon._sin_type` owns the semantic value for an inspected symbol. This does not by itself identify the API for this mod's current day, next day or every NPC's schedule.
- Normal menus hide/deactivate the inspected HUD. Actual HUD recreation requires rebinding/recreating mod-owned visuals. Exact text placement and final layout ownership below this HUD remain unverified.
- Native task mutation normally routes through `FlowCanvas.Nodes.Flow_SetTaskState -> GameSave.SetTaskState`.
- `KnownNPC.TaskState` contains task identity/state. Visible journal tasks are not proof of immediately actionable NPC interaction.
- Read-only `KnownNPCList.GetNPC` and `KnownNPC.GetQuestState` have bounded existing evidence. `GetOrCreateNPC` can mutate state and is unsuitable as a pure lookup.
- Phrase state and authored navigation/resource gates affect dialogue actionability. If a ready-to-act feature is adopted, preserve native gate semantics rather than infer readiness from text or visible task state.

Current-save player ownership and cinematic HUD alpha have additional shared evidence indexed by [RESEARCH_INDEX.md](https://github.com/NikichMods/GraveyardKeeperResearch/blob/4d1725a02dc78fc0951022a65a1c1194f74fe497/docs/RESEARCH_INDEX.md). Consult their owning documents when designing these paths; an index pointer is not implementation verification.

## Important neighboring-product distinction

[Day Wheel Quest Markers' contract](https://github.com/NikichMods/DayWheelQuestMarkers/blob/bc31dc8c3de69184c12efbf2afc037e70f828a70/AGENTS.md) marks currently actionable weekday-NPC interactions and deliberately excludes resource-blocked preparation steps. This is that mod's accepted product policy. It is not a universal definition of task relevance and has not been adopted for this reminder.

## Static task/calendar investigation — 2026-10-20

See [TASK_DATA_RESEARCH.md](TASK_DATA_RESEARCH.md) for exact input hashes, method owners and applicability limits. Bounded reflection-only IL and read-only game_data/localization inspection established:

- Saved journal source: GameSave.known_npcs -> npcs -> KnownNPC.tasks; task records hold ID/state, not typed items/deadlines/weekdays.
- Native card path: NPCsListGUI -> NPCItemGUI -> NPCListQuestText -> TaskState.GetTaskText. It skips Complete; it does not explicitly require Visible.
- Text: GJL.L("task_" + id), native ColorizeTags and base/DLC marker formatting. This method supplies authored wording, not live inventory counters.
- Native HUDTasksGUI belongs to RelationGUI/relation_additional, with mutating creation/encounter and animation behavior; it is not a passive calendar data API.
- GameSave.day_of_week = day % 6. Native Flow_GetDayOfWeek converts it to sin = ((12 - weekday) % 6) + 1 and explicitly names the six associated NPCs. The order is Pride -> Lust -> Gluttony -> Envy -> Wrath -> Sloth -> Pride.
- Exactly six game_data ObjectDefinition records carry nonempty day_icon; their values corroborate the same mapping and their aliases are empty.
- Native Complete state can be committed in a flying-marker callback after SetTaskState returns. Refresh design cannot assume the entry method's return is always the final writer.

Status: static host-data findings, not new installed-runtime acceptance. Private helper/output is ignored. Future shared promotion is subject to the shared repository's own workflow; this bounded project report does not silently change another mod's accepted policy.

## Current project evidence and remaining limits

- Product eligibility is settled: explicit Visible journal tasks grouped by weekday owner, including missing-item preparation, today and tomorrow.
- Full authored multi-NPC/non-weekly/DLC route coverage, physical NPC availability at each hour and any dynamic counters/readiness requirement.
- Source ownership/readiness/refresh and geometry are established under IMPLEMENTATION_GATE.md and current source; full native task/day/save/language transition coverage remains bounded.
- Native0.1.3 loading/render/settings are observed; the user accepts the final tuned appearance. This does not prove all menu/cutscene/reconstruction/overlap cases.
- Whether any readiness/count/detail computation is wanted, and its native owner if adopted.
- New-mod behavior on native task transitions, loading/switching saves and the user's mod combination.

The original data investigation did not build/install a mod or perform a game-runtime test. A later user request authorized the native candidate and F2 panel; IMPLEMENTATION_GATE.md records its additional prefab/font/input evidence, DEVELOPMENT.md the current no-editor build checks and TEST_LOG.md the exact build/install/return state.0.1.4 carries the final view without F2 under PRERELEASE.md; it remains prerelease testing, not stable acceptance.
