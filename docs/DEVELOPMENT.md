# Development recovery

Branch: codex/feature/native-reminder. Candidate: 0.1.0, built and installed, awaiting the user's first runtime return. No stable runtime release/acceptance yet. TEST_LOG.md owns exact source/artifact/install identity. The initial gate is in IMPLEMENTATION_GATE.md; data owners are in TASK_DATA_RESEARCH.md.

## Current behavior

Owned native UILabels below the calendar, with current day's known NPC tasks and next day's below. Include explicitly Visible task records without inventory/readiness filtering; omit Complete/Unknown. An unmet NPC's name is not disclosed. Empty sections say no known tasks. Native task text and marker symbols are reused at runtime; no dialogue parsing, asset bundling or game/save mutation.

The accepted **Лёгкий текст** layout is the direction, not installed visual acceptance. The preview's compact/260 choice supplies an initial width reference. Native units/bitmap glyph metrics and game HUD scaling differ from the browser preview; use F2 to calibrate the actual result.

## Build and evidence

Run `build.ps1 -GameRoot <local-game-root> -ExpectedSource <HEAD>` for an immutable handoff; add `-CheckOnly` for a development check. DotnetRoot defaults locally and can be passed explicitly. SDK6.0.406 Roslyn compiles against private installed references. No package restore or hosted references are required/selected. The output identity manifest records version, exact source and reference/artifact hashes.

The gate compiles Release, verifies assembly/file versions and runs tests using actual native GameSave/TaskState records without invoking a running game. Tests cover weekday/week wrap, Visible filtering, ordering, missing/null records, read-only selection and existence/signatures of the native input families. They do not prove Unity draw/input integration or saved F2 settings. No copied host binary is tracked or distributed.

## F2 settings

Persistent plugin config, saved when closed. Controls: X right/Y up, calendar gap, width, two native template fonts, body/name size, line/task/section gaps, None/Shadow/Outline, effect size/opacity, text opacity and tomorrow opacity. Reset restores plugin defaults. Sliders and single-step +/- controls change position/integers live. The right-side window is draggable and scrollable to fit the screen.

While open, capture/disable player control and the HUD event camera's mouse/touch/keyboard/controller input; guard native LazyInput families, including a closing-frame grace period. Restore captured flags on close/loading/native-window transition/disable/destroy/failure. Do not change native timeScale or saves. Raw Unity-key bindings in unrelated mods are not an exclusive-key contract.

Native text is localized by GJL. User controls set size/spacing after native language-font correction; journal/item templates may converge in CJK. Settings are Russian; authored section headings are Russian in ru and English otherwise. Broader editor localization/controller editing is not claimed for this candidate.

## First installed return

Check the real view below the calendar against the native journal, adjust it with F2, then close the panel and verify movement/clicks recover. Reopen once to verify retained settings, then restart/reload and confirm persistence. During ordinary play observe a task completion/day change and native menu hide/return when convenient; no synthetic quest progression is needed. Read accessible BepInEx log independently after the user reports the result. No stable promotion before explicit acceptance of exact tested bytes.

Long task text is fully wrapped with native ResizeHeight; no clipping/paging/automatic font shrink is introduced. Very long lists can exceed the available screen; the user can tune width/font/spacing in this first candidate. Revisit overflow semantics only with a real failing layout and product agreement.
