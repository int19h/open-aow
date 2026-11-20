#nullable disable
// Translated from FastCombatWin.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.UI.Dialogs;

// phase, phase frame, animation frame, and shield indices/opacities, combat viewer, completion
// event, retained event
public partial class FastBattleWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; private set; }
    public Label LeftSideLabel { get; private set; }
    public Label RightSideLabel { get; private set; }
    public Panel Party1Panel { get; private set; }
    public Image Party1Background { get; private set; }
    public Panel Party2Panel { get; private set; }
    public Image Party2Background { get; private set; }
    public Panel Party3Panel { get; private set; }
    public Image Party3Background { get; private set; }
    public Panel Party4Panel { get; private set; }
    public Image Party4Background { get; private set; }
    public Panel Party5Panel { get; private set; }
    public Image Party5Background { get; private set; }
    public Panel Party6Panel { get; private set; }
    public Image Party6Background { get; private set; }
    public Panel Party7Panel { get; private set; }
    public Image Party7Background { get; private set; }
    public HorizontalScrollBar SequencePositionSlider { get; private set; }
    public HorizontalScrollBar SequenceSpeedSlider { get; private set; }
    public Image GlowImage { get; private set; }
    public Label ModeLabel { get; private set; }
    public UserDrawBox LeftShieldView { get; private set; }
    public UserDrawBox RightShieldView { get; private set; }

    private byte _phase;
    private int _phaseFrame;
    private int _animationFrame;
    private CombatLogbook _logbook;
    private int _leftShield;
    private byte _leftOpacity;
    private int _rightShield;
    private byte _rightOpacity;
    private CombatViewer _viewer;
    private event ExecuteDoneEvent Done;
    private EventLog _event;

    // FastCombatWin.pas:231
    public void GetPartyPosition(int index, ref int X, ref int Y)
    {
        switch (index)
        {
            case 0:
                X = Party1Panel.WinLeft;
                Y = Party1Panel.WinTop;
                break;
            case 1:
            case 7:
                X = Party2Panel.WinLeft;
                Y = Party2Panel.WinTop;
                break;
            case 2:
                X = Party3Panel.WinLeft;
                Y = Party3Panel.WinTop;
                break;
            case 3:
                X = Party4Panel.WinLeft;
                Y = Party4Panel.WinTop;
                break;
            case 4:
                X = Party5Panel.WinLeft;
                Y = Party5Panel.WinTop;
                break;
            case 5:
                X = Party6Panel.WinLeft;
                Y = Party6Panel.WinTop;
                break;
            case 6:
                X = Party7Panel.WinLeft;
                Y = Party7Panel.WinTop;
                break;
            default:
                break;
        }
    }

    // FastCombatWin.pas:244
    public void DrawParties(int X, int Y, in LockedSurface surface)
    {
        if (_viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            Battle combat = _viewer.Combat;
            int pixelX = default;
            int pixelY = default;
            for (int i = 0, last = combat.CombatData.GetCount() - 1; i <= last; i++)
            {
                CombatObject objectValue = combat.CombatData.GetObjects(i);
                if (objectValue.Position != 128)
                {
                    GetPartyPosition(objectValue.Position >>> 4, ref pixelX, ref pixelY);
                    pixelX = pixelX + X - 15;
                    pixelY = pixelY + Y - 32;
                    int position = objectValue.Position & 15;
                    pixelX += position % 4 * 40;
                    if (position >= 4)
                    {
                        pixelY += 38;
                    }

                    CombatObject combatObject = combat.CombatData.GetObjects(i);
                    _viewer.ShowFieldObject(combatObject, frame: _animationFrame, direction: 3, pixelX, pixelY, surface);
                }
            }
        }
    }

    // FastCombatWin.pas:270
    public void DrawWalls(int X, int Y, in LockedSurface surface)
    {
        bool HasWall(int A, int B)
        {
            CombatParty pa = _viewer.Combat.CombatData.Parties.GetParty((byte)A);
            CombatParty pb = _viewer.Combat.CombatData.Parties.GetParty((byte)B);
            bool wa = pa is not null && pa.Wall != 0;
            bool wb = pb is not null && pb.Wall != 0;
            return wa ^ wb;
        }

        Y -= 19;
        int image = _viewer.Combat.CombatData.WallType == 2 ? 80 : 60;

        if (_viewer.Combat.CombatData.WallBreached)
        {
            image += 10;
        }

        if (HasWall(1, 6))
        {
            GamePack.Current.CombatImages[image].Show(X + 180, Y + 218 - 50, lockedSurface: surface);
        }

        if (HasWall(1, 2))
        {
            GamePack.Current.CombatImages[image].Show(X + 372, Y + 218 - 50, lockedSurface: surface);
        }

        if (HasWall(0, 1))
        {
            GamePack.Current.CombatImages[image + 4].Show(X + 180, Y + 218, lockedSurface: surface);
        }

        if (HasWall(0, 6))
        {
            GamePack.Current.CombatImages[image + 5].Show(X + 180, Y + 218, lockedSurface: surface);
        }

        if (HasWall(0, 2))
        {
            GamePack.Current.CombatImages[image + 5].Show(X + 372, Y + 218, lockedSurface: surface);
        }

        if (HasWall(5, 6))
        {
            GamePack.Current.CombatImages[image + 3].Show(X + 0, Y + 275, lockedSurface: surface);
        }

        if (HasWall(2, 3))
        {
            GamePack.Current.CombatImages[image + 2].Show(X + 374, Y + 275, lockedSurface: surface);
        }

        if (HasWall(0, 5))
        {
            GamePack.Current.CombatImages[image + 5].Show(X + 180, Y + 275, lockedSurface: surface);
        }

        if (HasWall(0, 3))
        {
            GamePack.Current.CombatImages[image + 5].Show(X + 372, Y + 275, lockedSurface: surface);
        }

        if (HasWall(0, 4))
        {
            GamePack.Current.CombatImages[image + 4].Show(X + 180, Y + 332, lockedSurface: surface);
        }

        if (HasWall(4, 5))
        {
            GamePack.Current.CombatImages[image + 1].Show(X + 180, Y + 342, lockedSurface: surface);
        }

        if (HasWall(4, 3))
        {
            GamePack.Current.CombatImages[image + 1].Show(X + 372, Y + 342, lockedSurface: surface);
        }

        CombatData combatData = _viewer.Combat.CombatData;
        if (combatData.WallType != 0)
        {
            CombatWall wall = combatData.Wall;
            if (wall is not null)
            {
                if (HasWall(0, 4))
                {
                    HitChance.ShowHitPercent(percent: HitChance.HitPointsToHitPercent(hits: wall.GetHits(), hitPoints: wall.HitPoints), x: X + 250, y: Y + 332 - 45, surface: surface);
                }
                else
                {
                    HitChance.ShowHitPercent(percent: HitChance.HitPointsToHitPercent(hits: wall.GetHits(), hitPoints: wall.HitPoints), x: X + 250, y: Y + 218 - 45, surface: surface);
                }
            }
        }
    }

    // FastCombatWin.pas:316
    public void DrawCombat(int X, int Y, ref readonly LockedSurface surface)
    {
        Battle combat = _viewer.Combat;
        UpdateShields();
        if (ModeLabel.Text != combat.CombatData.Header)
        {
            ModeLabel.Text = combat.CombatData.Header;
        }

        DrawParties(X, Y + 20, surface);
        if (combat.CombatData.WallType != 0)
        {
            DrawWalls(X, Y + 20, surface);
        }

        _viewer.ShowFightingObjects(_animationFrame, X + 234, Y + 15, surface);
        if (_phase is 2 or 3)
        {
            int pixelX = default;
            int pixelY = default;
            for (int i = 0; i <= 7; i++)
            {
                if (combat.CombatData.Parties.GetParty((byte)i) is not null)
                {
                    if (combat.CombatData.Parties.GetParty((byte)i).Army.Units.Count == 0)
                    {
                        if (!Set<byte>.WithBounds(0, 255).With(2).With(7).With(8).Contains(_viewer.Logbook.FinalData.CombatResult))
                        {
                            int glow = _phaseFrame / 2;
                            glow = Math.Abs(((glow + 7) % 15) - 7);
                            GetPartyPosition(i, ref pixelX, ref pixelY);
                            GamePack.Current.CombatImages[glow + 100].ShowClipped(pixelX + X - 10, pixelY + Y - 9, lockedSurface: surface);
                        }
                    }
                }
            }
        }
    }

    // FastCombatWin.pas:340
    public void UpdateShields()
    {
        CombatData data = _viewer.Combat.CombatData;
        int left;
        if (data.Players.GetPlayers(data.AttackerPlayer).Player == 0)
        {
            left = 12;
        }
        else
        {
            CombatPlayer player = data.Players.GetPlayers(data.AttackerPlayer);
            left = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
        }

        int leftOpacity = data.CurrentPlayer == 255 || data.Players.GetPlayers((sbyte)data.CurrentPlayer).SideIndex != 0 ? 60 : 100;

        if (_phase == 3)
        {
            leftOpacity = 100;
        }

        int right;
        if (data.Players.GetPlayers(data.DefenderPlayer).Player == 0)
        {
            right = 12;
        }
        else
        {
            CombatPlayer player = data.Players.GetPlayers(data.DefenderPlayer);
            right = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
        }

        int rightOpacity = data.CurrentPlayer == 255 || data.Players.GetPlayers((sbyte)data.CurrentPlayer).SideIndex != 1 ? 60 : 100;

        if (_phase == 3)
        {
            rightOpacity = 100;
        }

        if (left != _leftShield || leftOpacity != _leftOpacity)
        {
            _leftShield = left;
            _leftOpacity = (byte)leftOpacity;
            LeftShieldView.Drawn = false;
        }

        if (right != _rightShield || rightOpacity != _rightOpacity)
        {
            _rightShield = right;
            _rightOpacity = (byte)rightOpacity;
            RightShieldView.Drawn = false;
        }
    }

    // FastCombatWin.pas:373
    public void FinishDialog()
    {
        _viewer.Finalize();
        UI.General.Manager.OverlayPriority = 0;
        ExecuteDoneEvent done = Done;
        EventLog log = _event;
        Done = null;
        _event = null;
        done?.Invoke(log);

        _ = (log?.Release());
    }

    // FastCombatWin.pas:549
    public void SetPhase(byte phase)
    {
        _phase = phase;
        _phaseFrame = 0;
    }

    // FastCombatWin.pas:389
    public void FastBattleWindowCreate() => _viewer = new CombatViewer(UI.Engine);

    // FastCombatWin.pas:394
    public void FastBattleWindowDestroy() => _viewer?.Dispose();

    // FastCombatWin.pas:399
    public void CloseButClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // FastCombatWin.pas:405
    public void FCWinDrawSurface(Surface surface, int left, int top)
    {
        LockedSurface lockedSurface = default;
        try
        {
            if (!_viewer.Status.Contains(CombatViewerStatusElement.Paused))
            {
                _animationFrame += 1;
            }

            if (_phase == 1)
            {
                _viewer.Update();
            }

            if (surface.Lock(ref lockedSurface) >= 0)
            {
                if (SequencePositionSlider.Position != _viewer.Position)
                {
                    SequencePositionSlider.Position = _viewer.Position;
                }

                Rect clipRectangle = lockedSurface.ClipRectangle;
                clipRectangle.Top = MainWindow.WinTop + 24;
                lockedSurface.ClipRectangle = clipRectangle;
                DrawCombat(MainWindow.WinLeft + 10, MainWindow.WinTop + 10, surface: in lockedSurface);
                _ = surface.Unlock();
            }
        }
        catch
        {
            throw new InvalidOperationException("Exception during FastCombatWin.DrawSurface");
        }
    }

    // FastCombatWin.pas:424
    public void FCWinUpdate()
    {
        try
        {
            int image;
            switch (_phase)
            {
                case 0:
                    image = _phaseFrame / 2;
                    if (image > 9)
                    {
                        image = 9;
                    }

                    {
                        if (image != GlowImage.CurrentImage)
                        {
                            GlowImage.CurrentImage = image + 20;
                        }
                    }

                    if (!GlowImage.Visible)
                    {
                        GlowImage.Visible = true;
                    }

                    if (_phaseFrame > 20)
                    {
                        SetPhase(1);
                    }

                    break;
                case 1:
                    if (GlowImage.CurrentImage != 29)
                    {
                        GlowImage.CurrentImage = 29;
                    }

                    if (!GlowImage.Visible)
                    {
                        GlowImage.Visible = true;
                    }

                    {
                        int endPosition = _viewer.EndPosition;
                        if (_viewer.Position >= endPosition)
                        {
                            SetPhase(2);
                        }
                    }

                    break;
                case 2:
                    image = 9 - (_phaseFrame / 2);
                    if (image < 0)
                    {
                        image = 0;
                    }

                    {
                        if (image != GlowImage.CurrentImage)
                        {
                            GlowImage.CurrentImage = image + 20;
                        }
                    }

                    if (!GlowImage.Visible)
                    {
                        GlowImage.Visible = true;
                    }

                    if (_phaseFrame > 20)
                    {
                        SetPhase(3);
                        _phaseFrame = 21;
                    }

                    break;
                case 3:
                    if (GlowImage.Visible)
                    {
                        GlowImage.Visible = false;
                        MainWindow.Drawn = false;
                    }

                    break;
                default:
                    break;
            }

            MainWindow.SurfaceValid = false;
            _phaseFrame += 1;
        }
        catch
        {
            throw new InvalidOperationException("Exception during FastCombatWin.Update");
        }
    }

    // FastCombatWin.pas:466
    public void InitialViewClick()
    {
        UI.General.PlayButtonSound(0);
        CombatViewer viewer = _viewer;
        if (viewer is not null && viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            viewer.SetPosition(0);
            SetPhase(1);
        }
    }

    // FastCombatWin.pas:479
    public void FinalViewClick()
    {
        UI.General.PlayButtonSound(0);
        CombatViewer viewer = _viewer;
        if (viewer is not null && viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            viewer.SetPosition(viewer.EndPosition + 1);
            SetPhase(3);
        }
    }

    // FastCombatWin.pas:492
    public void PauseBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_viewer is not null && _viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            _viewer.Pause();
        }
    }

    // FastCombatWin.pas:498
    public void PlayBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_viewer is not null && _viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            _viewer.Play();
        }
    }

    // FastCombatWin.pas:504
    public void SequencePositionSldChange()
    {
        CombatViewer viewer = _viewer;
        if (viewer is not null && viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            viewer.SetPosition(SequencePositionSlider.Position);
            SetPhase(1);
        }
    }

    // FastCombatWin.pas:516
    public void SequenceSpeedSldChange() => _viewer?.SetSpeed((byte)(20 - SequenceSpeedSlider.Position));

    // FastCombatWin.pas:521
    public void LShieldUserDraw(UIComponent sender, LockedSurface surface)
    {
        if (_leftOpacity == 100)
        {
            UI.Graphics.Shields[_leftShield].Show(MainWindow.WinLeft + sender.WinLeft, MainWindow.WinTop + sender.WinTop, lockedSurface: surface);
        }
        else
        {
            UI.Graphics.Shields[_leftShield].ShowBlended(MainWindow.WinLeft + sender.WinLeft, MainWindow.WinTop + sender.WinTop, lockedSurface: surface, table: GraphicsState.AlphaBlendTable.LookupTables[_leftOpacity]);
        }
    }

    // FastCombatWin.pas:530
    public void RShieldUserDraw(UIComponent sender, LockedSurface surface)
    {
        if (_rightOpacity == 100)
        {
            UI.Graphics.Shields[_rightShield].Show(MainWindow.WinLeft + sender.WinLeft, MainWindow.WinTop + sender.WinTop, lockedSurface: surface);
        }
        else
        {
            UI.Graphics.Shields[_rightShield].ShowBlended(MainWindow.WinLeft + sender.WinLeft, MainWindow.WinTop + sender.WinTop, lockedSurface: surface, table: GraphicsState.AlphaBlendTable.LookupTables[_rightOpacity]);
        }
    }

    // FastCombatWin.pas:539
    public void FCWinHide() => FinishDialog();

    // FastCombatWin.pas:544
    public void FCWinKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key is 27 or 32)
        {
            MainWindow.Visible = false;
        }
    }

    internal static void ShowCombatEvent(FastBattleWindow dialog, CombatEventLog eventValue, ExecuteDoneEvent done)
    {
        Assert(dialog._event is null, message: "", file: "FastCombatWin.pas", line: 159);
        dialog.UI.General.Manager.OverlayPriority = 49;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        dialog.Done = done;
        dialog._logbook = eventValue.Logbook;
        InitializeCombatDialog(dialog, dialog._logbook);
        dialog.MainWindow.Visible = true;
    }

    internal static void InitializeCombatDialog(FastBattleWindow dialog, CombatLogbook logbook)
    {
        dialog._viewer.Initialize(logbook);
        Battle combat = dialog._viewer.Combat;
        dialog._animationFrame = 0;
        for (int i = 0; i <= 7; i++)
        {
            Image image = i switch
            {
                0 => dialog.Party1Background,
                1 => dialog.Party2Background,
                2 => dialog.Party3Background,
                3 => dialog.Party4Background,
                4 => dialog.Party5Background,
                5 => dialog.Party6Background,
                6 => dialog.Party7Background,
                7 => dialog.Party2Background,// { Original reuses the second background for the eighth party. }
                _ => null,
            };
            CombatParty party = combat.CombatData.Parties.GetParty((byte)i);
            if (party is not null)
            {
                sbyte playerIndex = combat.CombatData.Players.GetPlayers((sbyte)party.PlayerIndex).Player;
                int partyRace = playerIndex != 0 ? StrategicMap.Current.Players.GetPlayers(playerIndex).PlayerRace : 12;

                image.CurrentImage = partyRace + 21;
                image.Visible = true;
            }
            else
            {
                if (i != 7)
                {
                    image.Visible = false;
                }
            }

            // { Original refreshes both side labels inside the party loop. }
            int raceIndex;
            if (combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer).Player == 0)
            {
                string independents = dialog.UI.Engine.TranslateResourceString(ScenarioText.IndependentsText);
                dialog.LeftSideLabel.Text = independents;
            }
            else
            {
                CombatPlayer player = combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer);
                raceIndex = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
                RaceResource raceResource = GamePack.Current.RaceResources.GetRaceResource(raceIndex);
                dialog.LeftSideLabel.Text = raceResource.RaceName;
            }

            if (combat.CombatData.Players.GetPlayers(combat.CombatData.DefenderPlayer).Player == 0)
            {
                string independents = dialog.UI.Engine.TranslateResourceString(ScenarioText.IndependentsText);
                dialog.RightSideLabel.Text = independents;
            }
            else
            {
                CombatPlayer player = combat.CombatData.Players.GetPlayers(combat.CombatData.DefenderPlayer);
                raceIndex = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
                RaceResource raceResource = GamePack.Current.RaceResources.GetRaceResource(raceIndex);
                dialog.RightSideLabel.Text = raceResource.RaceName;
            }
        }

        dialog._viewer.Play();
        dialog.MainWindow.Drawn = false;
        int endPosition = dialog._viewer.EndPosition;
        dialog.SequencePositionSlider.Max = endPosition;
        dialog.SequenceSpeedSlider.Position = 20 - dialog._viewer.Delay;
        if (dialog._viewer.EndPosition != -1)
        {
            dialog.SetPhase(0);
        }
        else
        {
            dialog.SetPhase(3);
        }
    }
}
