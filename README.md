# Task Reminder

A lightweight Graveyard Keeper mod that keeps known tasks visible below the calendar on the left of the screen: today's weekday NPC first, tomorrow's below. Preparation tasks remain visible even when their items are missing.

**Status: prerelease candidate 0.1.5, under testing.** The appearance has been selected in game; this is not yet an accepted stable release.

The compact view uses native localized quest text and markers, Today/Tomorrow captions, thin dividers and soft shading under individual text lines. NPC names are hidden in the selected appearance. There is no whole-block background, settings window or mod hotkey: F2 is free for the game and other mods.

Target/test environment: Graveyard Keeper 1.407 on Windows with BepInEx 5. Install the supplied candidate as `BepInEx/plugins/TaskReminder.dll` with the game closed, then load a save. The mod does not change quests, inventory, schedules, time or saves. Building Planner and Day Wheel Quest Markers are not dependencies.

Task wording follows the game language; authored Today/Tomorrow/empty-state labels are Russian in Russian and English otherwise. Only already visible journal tasks of the six weekday NPCs are included. Very long lists may extend below the screen; broad language/mod-combination coverage is still under testing.

See [prerelease status](docs/PRERELEASE.md), [development and testing](docs/DEVELOPMENT.md) and [exact candidate evidence](docs/TEST_LOG.md). Original software and supporting documentation use [MPL-2.0](LICENSE); see [licensing](LICENSING.md).
