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

## Still unverified here

- Native journal row class/method, text/localization key path and composite formatting.
- Exact link from displayed task to NPC and semantic weekday; treatment of multi-NPC and non-weekly tasks.
- Current/next-day API and refresh timing.
- HUD insertion, font/scale, overlap, alpha/menu behavior and reconstruction for this new text block.
- Whether any readiness/count/detail computation is wanted, and its native owner if adopted.
- New-mod behavior on native task transitions, loading/switching saves and the user's mod combination.

No new static host inspection, game probe, build, installation or runtime verification has been performed for Task Reminder. The screenshot is user-interface context, not proof of internal mechanics.
