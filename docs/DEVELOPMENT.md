# Development recovery

Branch: codex/feature/native-reminder. Current candidate0.1.3 adds coordinated Planner appearance after the bounded0.1.2 load/settings return. No stable runtime release/acceptance yet. TEST_LOG.md owns exact source/artifact/install identity. Initial gate: IMPLEMENTATION_GATE.md; visual-refinement gate: VISUAL_REFINEMENT.md; data owners: TASK_DATA_RESEARCH.md.

## Current behavior

Owned native UILabels below the calendar, with current day's known NPC tasks and next day's below. Include explicitly Visible task records without inventory/readiness filtering; omit Complete/Unknown. An unmet NPC's name is not disclosed. Empty sections say no known tasks. Native task text and marker symbols are reused at runtime; no dialogue parsing, asset bundling or game/save mutation.

The accepted **Лёгкий текст** layout is the direction, not installed visual acceptance. The preview's compact/260 choice supplies an initial width reference. Native units/bitmap glyph metrics and game HUD scaling differ from the browser preview; use F2 to calibrate the actual result.

## Build and evidence

Run `build.ps1 -GameRoot <local-game-root> -ExpectedSource <HEAD>` for an immutable handoff; add `-CheckOnly` for a development check. DotnetRoot defaults locally and can be passed explicitly. SDK6.0.406 Roslyn compiles against private installed references. No package restore or hosted references are required/selected. The output identity manifest records version, exact source and reference/artifact hashes.

The gate compiles Release, verifies assembly/file versions and runs tests using actual native GameSave/TaskState records without invoking a running game. Tests cover weekday/week wrap, Visible filtering, ordering, missing/null records, read-only selection and existence/signatures of the native input families. Actual BepInEx tests also cover config migration, save/reload, full option snapshot rollback without disk writes, and isolated slider defaults. They do not prove Unity draw/input integration. No copied host binary is tracked or distributed.

## F2 settings

Persistent plugin config: F2 or Save commits changes; Esc or Cancel restores every option to its value on opening, with no disk write. Forced loading/native-menu/disable/destroy/failure closure also cancels. Controls: X right/Y up, calendar gap, width, two native template fonts, body/name size, line/task/section gaps, None/Shadow/Outline, effect size/opacity, text opacity and tomorrow opacity. Each slider has its own Reset button using the shipped0.1.1 defaults (CaptionSize is fixed at fresh-install16). The global reset remains a cancellable preview. Existing saved values are preserved. Sliders and single-step +/- controls change position/integers live. The right-side window is draggable and scrollable to fit the screen.

0.1.1 adds independent day-caption size/visibility and divider visibility/length/thickness/opacity. Hiding text keeps enabled lines; both hidden consumes no caption row. Native weekday symbols use the NPC list's own icon template and no effects. Quest markers also have no glyph effect or soft backing. Marker size is bounded to its reserved column.

Readability comparisons in F2: ordinary text versus planner-style raster24/scaling, fractional shadow/outline distance, and soft per-printed-line backing with opacity/X-Y padding/feather. New profile/backing controls start enabled for the requested comparison, can be switched off independently, and preserve previously saved width/font-size/placement values. 0.1.2 adds a signed backing Y offset from -24 to24 HUD units, default0; positive moves up independently of text and layout. Backing uses native processed line wraps and measured line widths; left padding beside icons is limited so the effect stays under text. No whole-block rectangle is added. New appearance is not visually accepted until the user's return.

While open, capture/disable player control and the HUD event camera's mouse/touch/keyboard/controller input; guard native LazyInput families, including a closing-frame grace period. Restore captured flags on close/loading/native-window transition/disable/destroy/failure. Do not change native timeScale or saves. Raw Unity-key bindings in unrelated mods are not an exclusive-key contract.

Native text is localized by GJL. User controls set size/spacing after native language-font correction; journal/item templates may converge in CJK. Settings are Russian; authored section headings are Russian in ru and English otherwise. Broader editor localization/controller editing is not claimed for this candidate.

## Coordinated appearance (0.1.3)

F2 starts with an independent Show NPC names/day icons switch (off releases the full row/gap), quest-circle size8..48 and native glyph-shadow toggle. The symbol's measured width reserves the task indent; row height includes both text and symbol. Symbols never receive rectangular soft backing. Weekday icon size remains its own setting.

Whole-block scale60..150% scales all owned labels/icons/lines/shade about the calendar anchor; position/calendar offsets stay fixed.100% means the selected style's baseline. In manual mode this preserves the existing HUD scale inheritance.

Match Building Planner is an optional reversible local style overlay, not a runtime dependency. Reference: installed Planner1.0.4, frozen080ff539c0e2b439d7525b2eff4ecf4b008bd3b0; its PlannerStyle/PlannerPresentation files establish craft font24, root65%, width286, native spacing, Outline1/.8, text(.94,.90,.80), backing(.07,.067,.055)/.39. Matching uses the same native template and compensates actual HUD versus screen reference scaling. Axis-invalid frames disable this candidate rather than silently claim a match. Caption font21 matches the Planner column labels; names use body24. Its framed location-banner title is a separate surface and is not imported.

Matched mode overrides width, body/name/caption sizes, font/raster/effect, spacing, section/task gaps, text/tomorrow opacity and line backing style. Related manual controls are disabled while matched; values are never rewritten, so toggling off restores the custom appearance. Visibility, names, day icons/dividers and quest-circle size/shadow remain independently adjustable. The scale slider multiplies the matched65% baseline (100%=65%,150%=97.5%). This is a profile of the documented installed version, not automatic tracking of future Planner edits.

Per-line backing remains separate measured textures; Planner feather28 is capped to half each narrow line's height. Position remains under the left calendar. No full-card rectangle or Planner columns/banner/actions are created. Actual two-block visual matching, wrapping and input/lifecycle acceptance require the installed return; compile/profile/basis/config checks cannot prove subjective identity.

## First installed return

Check the real view below the calendar against the native journal, adjust it with F2, then close the panel and verify movement/clicks recover. Reopen once to verify retained settings, then restart/reload and confirm persistence. During ordinary play observe a task completion/day change and native menu hide/return when convenient; no synthetic quest progression is needed. Read accessible BepInEx log independently after the user reports the result. No stable promotion before explicit acceptance of exact tested bytes.

Long task text is fully wrapped with native ResizeHeight; no clipping/paging/automatic font shrink is introduced. Very long lists can exceed the available screen; the user can tune width/font/spacing in this first candidate. Revisit overflow semantics only with a real failing layout and product agreement.
