# Product Requirements

## Adopted outcome

Show the current weekday NPC's already known tasks below the left calendar, followed by the next weekday NPC's tasks. Include preparation tasks even when items are missing. The user selected lightweight text, authorized the native mod and calibrated its final appearance in0.1.3 on2026-10-20.

Eligibility is explicitly Visible native journal records only. Complete, Unknown and null records are excluded. Group by journal owner, not current interaction readiness or the next intermediate NPC. Tomorrow means tomorrow's weekday NPC with tasks known now; it does not predict unlocks or NPC presence at a particular hour. Native wording and rich-text coloring are retained without live inventory counters or shortening.

Missing NPCs remain unnamed; an empty section reports no known tasks. Full task wording wraps at the chosen width. Both sections stay present. Long-list overflow remains an observed-testing boundary, not a promise of scrolling, paging or shrinking.

## Final appearance and controls

The user accepts the saved0.1.3 appearance as final for prerelease testing and explicitly removes the F2 editor in0.1.4. PRERELEASE.md and tests/FinalAppearance.cfg own the complete adopted baseline: width130, Journal14, Outline1, captions8, thin dividers, names hidden, quest circles9 without shadow, raster24 profile on, scale100%, Building Planner matching off, separate soft per-line backing.

No in-game settings window, key interception or control capture ships. Existing BepInEx configuration is read for compatibility; saved values are not reset or rewritten by this candidate. New installs receive the adopted defaults. Reintroducing an editor or changing the final style needs a later user request and a new candidate. Historical tuning controls in0.1.0–0.1.3 remain in Git history, not in the player DLL.

## Preserved boundaries

Native progression, schedules, time, inventory and saves remain unchanged. No remote hand-ins, auto-completion, undisclosed objectives, route planner, non-weekly-NPC expansion or dependency on Building Planner/Day Wheel Quest Markers. The marker mod's readiness-only policy is not this reminder's task filter. Assets/fonts/localization remain host-owned at runtime and are not bundled.

Native task text uses the game language. Authored section/empty labels are Russian for ru and English otherwise; full editor localization is irrelevant with the editor removed. Additional counters, quality/readiness details, input interactions or overflow redesign are outside the selected scope unless reopened by a concrete request.

## Acceptance boundary

0.1.3 visual selection is accepted;0.1.4 is a prerelease for continued testing, not stable promotion. Verify unchanged final view and absent F2 editor first, then collect natural task-completion/day-change/menu/save-reload observations. Exact status and limitations live in TEST_LOG.md; successful compilation does not establish native behavior. Project identity is Task Reminder / NikichMods/TaskReminder, confirmed2026-10-20.
