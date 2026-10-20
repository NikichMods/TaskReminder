# Prerelease candidate 0.1.5

On 2026-10-20 the user accepts the tuned 0.1.3 appearance as the final visual direction and requests removal of F2 settings. This is prerelease readiness for continued testing, explicitly not stable promotion. Historical0.1.4 is installed and reaches native TR_READY;0.1.5 is the prepared narrow inline-symbol fix, with installation/return state in TEST_LOG.md. Exact artifact/install identities belong to TEST_LOG.md.

## Production changes — READY before implementation

- **Freeze appearance:** Options owns configuration defaults; BepInEx Bind consumes persisted values before rendering through Appearance/ReminderView/TextDecorations. The saved 0.1.3 configuration and final TR_SETTINGS record agree on width130/Journal14/Outline/names hidden/quest9/scale100/Planner matching off. Promote all saved appearance values to shipped defaults, preserve existing overrides and rendering code. Compare every fresh bound entry with the pinned final-appearance fixture through actual BepInEx, and verify loading old config writes no bytes. Configuration remains readable for compatibility; no automatic reset or config rewrite.
- **Remove editor:** SettingsPanel, Plugin.Update/OnGUI and the two input Harmony guards exclusively own F2 editing/control capture. Delete that surface and its config transaction helpers, remove all editor entry/close calls. Preserve the native task-state postfix, renderer, readiness, hide/rebind and teardown. Verify compiled absence of editor/guards/OnGUI/input members, remaining patch ownership, build and policy tests. A direct native F2 press and normal movement/click check completes the changed runtime boundary; no synthetic gameplay setup is needed.

F2 restoration requires a later explicit request and a new immutable candidate. The editor remains recoverable at source68b0abc55f26e173574100c8b9b396f411bd0430, not hidden in the player DLL. No main merge, stable tag or stable Release is authorized by this limited prerelease approval.

## Final appearance baseline

The authored fixture `tests/FinalAppearance.cfg` pins every saved appearance value used for the accepted view. It contains only mod parameters, no host assets, saves, paths or private context.

| Group | Adopted values |
| --- | --- |
| Position and width | X10, Y0, calendar gap14, width130, scale100% |
| Text | Journal; body14, names16, captions8; raster24 profile on; Outline1 at100% |
| Rows | Line spacing-2, task gap1, section gap12; captions and dividers on; divider45×1 at45% |
| Names and icons | Names/day row off; day icon16 if enabled; quest icon9, quest shadow off |
| Backing | Per-line only; opacity25%, X padding3, Y padding0, feather6, Y offset-3 |
| Opacity and profile | Text100%, tomorrow70%, Building Planner match off |

## Acceptance and remaining work

User acceptance covers the tuned appearance of exact0.1.3. Its native log reaches TR_READY and records the final settings; source/installed hash match the handed candidate. It does not establish every task/DLC/language route, save switch, day transition or every menu/cutscene.

0.1.4 needs a focused native return: unchanged final view, F2 opens nothing in this mod, normal input remains available, native menu/HUD return and a reload preserve the view. Natural task completion/day change/save switching remain broader test observations to collect when encountered. Do not reopen the visual design or add features without a concrete defect/request. Continued testing and explicit stable acceptance precede promotion/publication of exact bytes.

## 0.1.5 inline-symbol effect correction — READY (2026-10-21)

User reports thick added outlines on graveyard/church quality symbols inside task text; all other appearance is invariant. ReminderView.Line copies the native template symbolStyle and applies text Outline to the complete mixed label. Native UILabel.OnFill is the final effect consumer, not task markup or the atlas.

Prior shared UI_INPUT_TIME_AND_ENVIRONMENT.md already establishes whole-label effects. Targeted reflection-only inspection of installed firstpass SHA2569dc6def3b7715dd27eeb168ddc0af47e31c6f38d3fbee24bf592899392026498 then establishes the smaller native seam: NGUIText.SymbolStyle.NoEffect=3. NGUIText.Print tags only symbol vertices with transparent magenta; ApplyShadow leaves their copied effect vertices transparent, then UILabel.OnFill restores original symbol vertices to white/finalAlpha. UIWidget.UpdateFinalAlpha includes label.color.a and parent alpha, preserving tomorrow/HUD fading. Symbol geometry/advance and text vertices use the same native path. No font/atlas mutation or string splitting is needed.

Set NoEffect only on owned mixed text labels, after language-font/template setup. Separate quest/day-marker labels retain their existing mode, effects and size. Per-line measurement follows the source mode but wrapping/geometry is unchanged. No text/content/config/default/line backing/input/selection changes. Native original icon artwork may contain its own border; this removes the added text effect only. Build/old policy/default/no-editor checks plus focused in-game comparison of the two reported rows prove the changed property; no new probe or broad gameplay matrix is needed. Hand immutable0.1.5 on the feature branch; no stable promotion. Do not replace the loaded0.1.4 DLL.
