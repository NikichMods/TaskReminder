# Task Reminder

A small Graveyard Keeper mod in development that keeps known weekday-NPC tasks visible below the calendar on the left of the screen.

The display lists today's NPC tasks first and tomorrow's below, including preparation tasks whose items are still missing. It uses the native task text and does not change progression or saves.

**Status:** first test candidate under development; no accepted stable release yet.

F2 opens live appearance settings: position, width, two native font templates, text size, line/task/section spacing, shadow or outline and opacity. Close with F2, Esc or the panel button to save settings to the plugin's BepInEx configuration. The settings panel uses the mouse; it blocks gameplay input while open. Native game time is not changed. Task text follows the game language; this candidate's settings are Russian, with Russian/English section headings.

The development target is Graveyard Keeper 1.407 on Windows with BepInEx 5. A clean compile is not installed-runtime acceptance; the first visual/input/restart pass is pending.

See [product requirements](docs/PRODUCT_REQUIREMENTS.md) and [design / current next step](docs/DESIGN.md). Original software and supporting documentation are covered by [MPL-2.0](LICENSE); see [licensing scope](LICENSING.md).
