# Native Task Data and Weekday Mapping

## Question and result

Investigated on 2026-10-20 for Graveyard Keeper 1.407: how the NPC task interface stores and renders objectives, and whether those objectives can be grouped by current/next weekday without reimplementing dialogue logic.

**Static finding:** the native NPC journal already provides task identity, state, NPC ownership and localized display text. The six weekly NPCs have matching day-icon metadata, and a native FlowCanvas node explicitly maps the calendar day to their sin/day. A journal-based reminder can therefore be investigated independently of Day Wheel Quest Markers' interaction classifier. No mod implementation or runtime acceptance is included in this finding.

The supported interpretation is **journal tasks owned by the NPC associated with this weekday**. This is not a deadline field, proof of NPC presence at the current hour, a route to complete every task today, or a readiness classifier.

## Method and input identity

Prior research lookup: project evidence, GraveyardKeeperResearch's RESEARCH_INDEX.md and DIALOGUE_QUEST_AND_FLOWCANVAS.md, and DayWheelQuestMarkers' accepted 1.1.16 contract/current source. Missing journal/text/calendar details were then inspected directly.

Method: reflection-only metadata and bounded method IL inspection of the installed assemblies, plus read-only UnityPy inspection of `game_data` and localization assets. The helper never invokes game methods. No game process interaction, probe installation, save read/write or production patch was required. Private IL/resource projections and the static-inspection helper remain in ignored work/; only derived facts and identifiers are published.

| Input | SHA-256 |
| --- | --- |
| Assembly-CSharp.dll | e72e4270e4b88dd0a87ca23c9cf1750aec4c4a0fedb40b6d2dae7902fc9c7fd8 |
| Assembly-CSharp-firstpass.dll | 9dc6def3b7715dd27eeb168ddc0af47e31c6f38d3fbee24bf592899392026498 |
| resources.assets | 215c7981901a4b72d5db717666ba47ad3cc032527c95f58dc39d8af1293a69ca |

These are the actually inspected local bytes. No claim of a new installed-game test or complete DLC/quest-route audit is made.

## 1. Saved state and identity

The native NPC journal path is:

`MainGame.me.save -> GameSave.known_npcs -> KnownNPCList.npcs -> KnownNPC.tasks`

Each `KnownNPC` has `npc_id` and a list of `KnownNPC.TaskState`. Each task record contains only `id` and `state` on the inspected declared-field surface; it has no declared weekday, deadline, required-item list, required quantity or carried-quantity field.

`KnownNPC.TaskState.State` values are `Visible = 0`, `Complete = 1`, `Unknown = 2`. `KnownNPC.GetQuestState(taskId)` searches that NPC's stored tasks; an absent task returns Unknown. `SetQuestState` changes the existing record by ID or appends a record with the supplied ID/state. Completion updates the record; it does not remove it on that inspected path. Use the NPC/task pair as identity rather than treating a translated string as a key.

`GameSave.ToBinary()` delegates to `SmartSerializer.Serialize<GameSave>` after native scene capture; `FromBinary(byte[])` delegates to the corresponding deserializer. Existing shared evidence separately establishes a native KnownNPCList task/state binary roundtrip. This investigation does not write a save or independently retest full-game persistence.

`GameSave.quests` / `QuestSystem` / `QuestDefinition` also exist, with trigger/reward/current-quest state. They are a different subsystem. The inspected NPC task-card renderer obtains its rows from known_npcs, not from QuestSystem.GetCurrentQuests. Do not select a similarly named quest API as the journal source merely because its name sounds appropriate.

### Read-only access limits

`KnownNPCList.GetNPC` is the existing pure lookup; it returns null for an absent NPC. `GetOrCreateNPC` resolves `ObjectDefinition.npc_alias` and can add a known NPC and trigger achievement checks. `RelationGUI.Open` can call `GameSave.OnMetNPC`, which uses that creation path. Opening native UI is therefore not a pure way to query task data.

Aliases are relevant to ownership normalization: dialogue with an alternate object may use a canonical NPC record. In the inspected game_data, all six weekday owners have empty npc_alias. Do not assume that is true for every NPC or modded object.

## 2. Text and the native journal renderer

The inspected rendering chain is:

`NPCsListGUI.Open -> NPCItemGUI.Draw(KnownNPC) -> NPCListQuestText.Draw(TaskState) -> TaskState.GetTaskText()`

- NPCsListGUI sorts known_npcs and rebuilds the list on Open. The player entry is handled specially; other cards require an existing ObjectDefinition, IsRelationVisible and a non-worker_zombie ID.
- NPCItemGUI iterates that NPC's tasks in stored order and skips records whose state is Complete.
- GetTaskText looks up `task_` plus task ID through `GJL.L(string)`, then applies `LocalizedLabel.ColorizeTags(..., TextColor.SpeechBubble)`.
- NPCListQuestText adds the native base/DLC marker prefix and a language-dependent separating space. NPC headings use `GJL.L(npc_id)`; descriptions use `desc_` plus npc_id.
- The NPC card's `top_icon_txt` is set from `ObjectDefinition.day_icon`.

**Important exactness:** the renderer tests `state != Complete`, not `state == Visible`. A stored Unknown record would pass this filter. This investigation has not established that ordinary authored saves contain such records. A future decision between strict active-state filtering and exact native-journal parity must acknowledge this difference; do not change one predicate into the other silently.

### Localization and formatting

`GJL` lives in Assembly-CSharp-firstpass. It loads the selected `Locales/lng_<locale>` resource, builds its dictionary from txt_ids/txts, and resolves language aliases. `GJL.L(string)` returns the key itself if no translation/dictionary entry is available; a nonempty string is therefore not proof that localization succeeded.

ColorizeTags converts authored angle-bracket emphasis into NGUI color markup. On GetTaskText's SpeechBubble route the emphasized segment uses the native brown/orange color; the rest of the row retains its label styling. NPCListQuestText's marker prefixes distinguish base, Stories, refugee/event and Souls categories. These are native formatting inputs, not instructions to bundle game-owned sprites or localization into the mod.

The inspected resources contain eleven locale assets, each with 381 keys beginning `task_`, one empty value and no `%1`/`%2` substitution placeholders in those entries. That prefix count includes supporting task strings and is **not** a count of active objectives, unique quest steps, supported reminder rows or complete route coverage.

The task-text method itself reads no inventory/resource state. Item names, quantities and qualitative goals shown in ordinary task text are authored text, not a typed requirements model supplied by this API. The two Merchant task identifiers corresponding to the reference image are `merchant_garden` and `merchant_curse`. The first describes the multi-vegetable request; the second is a non-item goal. A display can reuse both texts, while a carried/required counter or readiness badge would need a separate verified source. Do not parse translated sentences into gameplay conditions.

## 3. Existing HUD task display and update ownership

`HUDTasksGUI` already renders task rows, but it belongs to the native NPC relationship/dialogue display:

`GUIElements.relation / relation_additional -> RelationGUI.npc_tasks -> HUDTasksGUI`

RelationGUI.Open selects/normalizes one NPC and calls HUDTasksGUI.Draw(npc_id). Hide clears it with an empty NPC ID. Draw rebuilds rows, uses GetOrCreateNPC, skips Complete, sets native text/markers and repositions the table. Redraw repeats Draw for its cached NPC. Its hidden-task list controls presentation during the native task animation; it is not the save's authoritative task-state list.

Consequences: this is useful evidence and a possible later source of native presentation references. Calling Draw to read data, reparenting the live dialogue component or treating it as an already available persistent calendar list is not justified by these findings. Exact below-calendar anchoring/font/alpha/lifecycle remains a separate UI investigation.

### Native mutation timing

Shared evidence identifies Flow_SetTaskState -> GameSave.SetTaskState. Direct inspection adds an important ordering limit:

- For Visible, GameSave.SetTaskState can set the KnownNPC task state before starting the native visual notification.
- For Complete, some paths write the final state in a flying-marker arrival callback; another path writes it directly when the animation path is unavailable.
- The callback updates KnownNPC.SetQuestState and redraws the native task view. Thus return from GameSave.SetTaskState is not a universal final-commit boundary for every state change.

For future refresh design, distinguish requested state, committed KnownNPC state and transient native UI animation. No hook/polling choice is selected here. Save replacement, day transition and language change also require their own verified refresh/readiness boundaries before production.

## 4. Calendar ownership and chronological order

`GameSave.day` is the saved day counter. Its `day_of_week` getter is `day % 6`, with normal nonnegative values 0..5.

The actual native `Flow_GetDayOfWeek` computes the semantic sin/day as:

`sin = ((12 - day_of_week) % 6) + 1`

Its registered outputs explicitly associate Sloth with Astrologer, Wrath with Inquisitor, Envy with Cultist/Snake, Gluttony with Merchant, Lust with Actress/Ms. Charm and Pride with Bishop. This is direct native corroboration of the accepted marker-mod mapping, rather than a deduction from translated NPC names.

`HUD.RedrawSinsIcons` independently uses the same relationship at its current-day/glow selection (physical index 3), and assigns the changing semantic value to each HUDSinIcon. Read a symbol's `_sin_type` for that symbol's current semantics; physical index alone is not a fixed weekday identity. A task-data reader can use the authoritative save/calendar relationship instead of depending on UI positions or animation completion.

| day_of_week | Semantic day / enum value | Weekday NPC | NPC ID | Native day_icon |
| --- | --- | --- | --- | --- |
| 1 | Pride / 6 | Bishop | npc_bishop | (d6) |
| 2 | Lust / 5 | Ms. Charm | npc_actress | (d5) |
| 3 | Gluttony / 4 | Merchant | npc_merchant | (d4) |
| 4 | Envy / 3 | Snake | npc_cultist | (d3) |
| 5 | Wrath / 2 | Inquisitor | npc_inquisitor | (d2) |
| 0 | Sloth / 1 | Astrologer | npc_astrologer | (d1) |

The table starts at remainder 1 to show the chronological cycle. The wrap is Sloth -> Pride, not Sloth -> Wrath. For next day use `(day_of_week + 1) % 6` and the same native conversion. Equivalently, the semantic sin value decreases, with 1 wrapping to 6. Greed = 0 exists in the enum but is not a seventh calendar day.

In the inspected game_data (MonoBehaviour path_id 150254), **exactly six ObjectDefinition records have nonempty day_icon**: the six rows above. Each has npc_in_list=true and empty npc_alias; sort_n also matches the corresponding sin value, but sort_n is a journal ordering field, not the calendar owner. The day_icon field is a presentation token that corroborates this bounded mapping, not a general future-proof spawn-schedule API.

`EnvironmentEngine.Update` advances its clock and calls OnEndOfDay when the internal day interval wraps. OnEndOfDay increments save.day before notifying HUD.OnEndOfDay. The HUD then animates its circle and redraws symbol semantics on tween completion. A save-based reader and the animated wheel can therefore be at different presentation stages during that brief transition. Do not wait for a guessed real-world clock time or infer progression solely from a still-animating symbol.

## 5. What grouping proves, and what remains a product decision

A small, read-only data view can be defined as: committed current-save journal state -> known weekday NPC -> chosen task-state filter -> native localized task text. The same path applies to the next weekday. This does not require classifying authored dialogue routes during gameplay or depending on the Day Wheel Quest Markers DLL.

The data alone does not resolve these meanings:

- **Preparation:** show the day's NPC's journal tasks even if items/prerequisites are missing. Supported by the direct journal-text source, subject to selecting its exact state/coverage policy.
- **Actionable visit:** show only interactions that can progress now. Requires authored resource/phrase/navigation checks; accepted marker research is relevant, but this is a stronger and different feature.
- **Next person to visit:** a task may be stored under one NPC while its intermediate step involves another. Journal ownership/day_icon alone cannot move it to that other NPC's day reliably. The marker mod's accepted cross-owner interaction rules are evidence of this distinction, not an automatically adopted route planner.

The snapshot has no typed task deadline. “Tomorrow” can mean tomorrow's weekly NPC with the tasks known **now**, not a prediction of tasks that will unlock overnight. NPCs outside the six-row mapping have no inferred weekday. Do not expose future task text just because it exists in localization resources.

## Evidence limits and next decision

No new DLL, build, install or game-runtime test was performed. No native state or files were changed. Current-save readiness, UI geometry/final layout, actual language-switch redraw, compatibility with installed mods and the complete multi-NPC/DLC task universe remain unverified for this project.

The native source hypothesis is supported at the static data level. The next useful product decision is whether the first display means **tasks belonging to today's NPC** (including preparation) or **actions available today**. If the former is adopted, native journal data is a substantially smaller starting mechanism; if the latter is required, use the verified interaction evidence deliberately and budget for its additional semantics. Exact implementation remains unselected.
