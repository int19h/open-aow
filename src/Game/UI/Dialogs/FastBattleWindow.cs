// Translated from FastCombatWin.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// phase, phase frame, animation frame, and shield indices/opacities, combat viewer, completion
// event, retained event
public partial class FastBattleWindow : DataModule
{
    public FastBattleWindow(Component owner) : base(owner)
    { }

    public Window MainWindow;
    public Label LeftSideLabel;
    public Label RightSideLabel;
    public Panel Party1Panel;
    public Image Party1Background;
    public Panel Party2Panel;
    public Image Party2Background;
    public Panel Party3Panel;
    public Image Party3Background;
    public Panel Party4Panel;
    public Image Party4Background;
    public Panel Party5Panel;
    public Image Party5Background;
    public Panel Party6Panel;
    public Image Party6Background;
    public Panel Party7Panel;
    public Image Party7Background;
    public HorizontalScrollBar SequencePositionSlider;
    public HorizontalScrollBar SequenceSpeedSlider;
    public Image GlowImage;
    public Label ModeLabel;
    public UserDrawBox LeftShieldView;
    public UserDrawBox RightShieldView;
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
        }
    }

    // FastCombatWin.pas:244
    public void DrawParties(int X, int Y, in LockedSurface surface)
    {
        int pixelX = default;
        int pixelY = default;
        int position;
        Battle combat;
        CombatObject objectValue;
        if (_viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            combat = _viewer.Combat;
            for (int I = 0, last = combat.CombatData.GetCount() - 1; I <= last; I++)
            {
                objectValue = combat.CombatData.GetObjects(I);
                if (objectValue.Position != 128)
                {
                    GetPartyPosition((int)(((uint)objectValue.Position) >> 4), ref pixelX, ref pixelY);
                    pixelX = pixelX + X - 15;
                    pixelY = pixelY + Y - 32;
                    position = objectValue.Position & 15;
                    pixelX += position % 4 * 40;
                    if (position >= 4)
                    {
                        pixelY += 38;
                    }

                    var combatObject = combat.CombatData.GetObjects(I);
                    _viewer.ShowFieldObject(combatObject, _animationFrame, 3, pixelX, pixelY, surface);
                }
            }
        }
    }

    // FastCombatWin.pas:270
    public void DrawWalls(int X, int Y, in LockedSurface surface)
    {
        int image;
        CombatWall wall;
        bool HasWall(int A, int B)
        {
            CombatParty pa;
            CombatParty pb;
            bool wa;
            bool wb;
            pa = _viewer.Combat.CombatData.Parties.GetParty((byte)A);
            pb = _viewer.Combat.CombatData.Parties.GetParty((byte)B);
            wa = pa != null && pa.Wall != 0;
            wb = pb != null && pb.Wall != 0;
            return wa ^ wb;
        }

        Y -= 19;
        if (_viewer.Combat.CombatData.WallType == 2)
        {
            image = 80;
        }
        else
        {
            image = 60;
        }

        if (_viewer.Combat.CombatData.WallBreached)
        {
            image += 10;
        }

        if (HasWall(1, 6))
        {
            GamePack.Current.CombatImages[image].Show(X + 180, Y + 218 - 50, surface);
        }

        if (HasWall(1, 2))
        {
            GamePack.Current.CombatImages[image].Show(X + 372, Y + 218 - 50, surface);
        }

        if (HasWall(0, 1))
        {
            GamePack.Current.CombatImages[image + 4].Show(X + 180, Y + 218, surface);
        }

        if (HasWall(0, 6))
        {
            GamePack.Current.CombatImages[image + 5].Show(X + 180, Y + 218, surface);
        }

        if (HasWall(0, 2))
        {
            GamePack.Current.CombatImages[image + 5].Show(X + 372, Y + 218, surface);
        }

        if (HasWall(5, 6))
        {
            GamePack.Current.CombatImages[image + 3].Show(X + 0, Y + 275, surface);
        }

        if (HasWall(2, 3))
        {
            GamePack.Current.CombatImages[image + 2].Show(X + 374, Y + 275, surface);
        }

        if (HasWall(0, 5))
        {
            GamePack.Current.CombatImages[image + 5].Show(X + 180, Y + 275, surface);
        }

        if (HasWall(0, 3))
        {
            GamePack.Current.CombatImages[image + 5].Show(X + 372, Y + 275, surface);
        }

        if (HasWall(0, 4))
        {
            GamePack.Current.CombatImages[image + 4].Show(X + 180, Y + 332, surface);
        }

        if (HasWall(4, 5))
        {
            GamePack.Current.CombatImages[image + 1].Show(X + 180, Y + 342, surface);
        }

        if (HasWall(4, 3))
        {
            GamePack.Current.CombatImages[image + 1].Show(X + 372, Y + 342, surface);
        }

        var combatData = _viewer.Combat.CombatData;
        if (combatData.WallType != 0)
        {
            wall = combatData.Wall;
            if (wall != null)
            {
                if (HasWall(0, 4))
                {
                    var hits = wall.GetHits();
                    var hitPointsToHitPercent = HitChance.HitPointsToHitPercent(wall.GetHitPoints(), hits);
                    HitChance.ShowHitPercent(X + 250, Y + 332 - 45, surface, hitPointsToHitPercent);
                }
                else
                {
                    var hits = wall.GetHits();
                    var hitPointsToHitPercent = HitChance.HitPointsToHitPercent(wall.GetHitPoints(), hits);
                    HitChance.ShowHitPercent(X + 250, Y + 218 - 45, surface, hitPointsToHitPercent);
                }
            }
        }
    }

    // FastCombatWin.pas:316
    public void DrawCombat(int X, int Y, ref LockedSurface surface)
    {
        Battle combat;
        int pixelX = default;
        int pixelY = default;
        int glow;
        combat = _viewer.Combat;
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
        if (_phase == 2 || _phase == 3)
        {
            for (int I = 0; I <= 7; I++)
            {
                if (combat.CombatData.Parties.GetParty((byte)I) != null)
                {
                    if (combat.CombatData.Parties.GetParty((byte)I).Army.GetCount() == 0)
                    {
                        if (!Set<byte>.WithBounds(0, 255).With(2).With(7).With(8).Contains(_viewer.Logbook.FinalData.CombatResult))
                        {
                            glow = _phaseFrame / 2;
                            glow = Math.Abs((glow + 7) % 15 - 7);
                            GetPartyPosition(I, ref pixelX, ref pixelY);
                            GamePack.Current.CombatImages[glow + 100].ShowClipped(pixelX + X - 10, pixelY + Y - 9, surface);
                        }
                    }
                }
            }
        }
    }

    // FastCombatWin.pas:340
    public void UpdateShields()
    {
        CombatData data;
        int left;
        int right;
        int leftOpacity;
        int rightOpacity;
        data = _viewer.Combat.CombatData;
        if (data.Players.GetPlayers(data.AttackerPlayer).Player == 0)
        {
            left = 12;
        }
        else
        {
            var player = data.Players.GetPlayers(data.AttackerPlayer);
            left = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
        }

        if (data.CurrentPlayer == 255 || data.Players.GetPlayers((sbyte)data.CurrentPlayer).SideIndex != 0)
        {
            leftOpacity = 60;
        }
        else
        {
            leftOpacity = 100;
        }

        if (_phase == 3)
        {
            leftOpacity = 100;
        }

        if (data.Players.GetPlayers(data.DefenderPlayer).Player == 0)
        {
            right = 12;
        }
        else
        {
            var player = data.Players.GetPlayers(data.DefenderPlayer);
            right = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
        }

        if (data.CurrentPlayer == 255 || data.Players.GetPlayers((sbyte)data.CurrentPlayer).SideIndex != 1)
        {
            rightOpacity = 60;
        }
        else
        {
            rightOpacity = 100;
        }

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
        ExecuteDoneEvent done;
        EventLog log;
        _viewer.Finalize();
        DialogHosts.General.Manager.OverlayPriority = 0;
        done = Done;
        log = _event;
        Done = null;
        _event = null;
        if (done != null)
        {
            done(log);
        }

        if (log != null)
        {
            log.Release();
        }
    }

    // FastCombatWin.pas:549
    public void SetPhase(byte phase)
    {
        _phase = phase;
        _phaseFrame = 0;
    }

    // FastCombatWin.pas:389
    public void FastBattleWindowCreate()
    {
        _viewer = new CombatViewer();
    }

    // FastCombatWin.pas:394
    public void FastBattleWindowDestroy()
    {
        _viewer?.Dispose();
    }

    // FastCombatWin.pas:399
    public void CloseButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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

                lockedSurface.ClipRectangle.Top = MainWindow.WinTop + 24;
                DrawCombat(MainWindow.WinLeft + 10, MainWindow.WinTop + 10, ref lockedSurface);
                surface.Unlock();
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
        int image;
        try
        {
            switch (_phase)
            {
                case 0:
                    image = _phaseFrame / 2;
                    if (image > 9)
                    {
                        image = 9;
                    }

                    {
                        if (image != this.GlowImage.CurrentImage)
                        {
                            this.GlowImage.CurrentImage = image + 20;
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
                        var endPosition = this._viewer.GetEndPosition();
                        if (this._viewer.Position >= endPosition)
                        {
                            SetPhase(2);
                        }
                    }

                    break;
                case 2:
                    image = 9 - _phaseFrame / 2;
                    if (image < 0)
                    {
                        image = 0;
                    }

                    {
                        if (image != this.GlowImage.CurrentImage)
                        {
                            this.GlowImage.CurrentImage = image + 20;
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
        CombatViewer viewer;
        DialogHosts.General.PlayButtonSound(0);
        viewer = _viewer;
        if (viewer != null && viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            viewer.SetPosition(0);
            SetPhase(1);
        }
    }

    // FastCombatWin.pas:479
    public void FinalViewClick()
    {
        CombatViewer viewer;
        DialogHosts.General.PlayButtonSound(0);
        viewer = _viewer;
        if (viewer != null && viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            viewer.SetPosition(viewer.GetEndPosition() + 1);
            SetPhase(3);
        }
    }

    // FastCombatWin.pas:492
    public void PauseBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_viewer != null && _viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            _viewer.Pause();
        }
    }

    // FastCombatWin.pas:498
    public void PlayBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_viewer != null && _viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            _viewer.Play();
        }
    }

    // FastCombatWin.pas:504
    public void SequencePositionSldChange()
    {
        CombatViewer viewer;
        viewer = _viewer;
        if (viewer != null && viewer.Status.Contains(CombatViewerStatusElement.Initialized))
        {
            viewer.SetPosition(SequencePositionSlider.Position);
            SetPhase(1);
        }
    }

    // FastCombatWin.pas:516
    public void SequenceSpeedSldChange()
    {
        if (_viewer != null)
        {
            _viewer.SetSpeed((byte)(20 - SequenceSpeedSlider.Position));
        }
    }

    // FastCombatWin.pas:521
    public void LShieldUserDraw(UIComponent sender, LockedSurface surface)
    {
        if (_leftOpacity == 100)
        {
            DialogHosts.UIGraphics.Shields[_leftShield].Show(MainWindow.WinLeft + sender.WinLeft, MainWindow.WinTop + sender.WinTop, surface);
        }
        else
        {
            DialogHosts.UIGraphics.Shields[_leftShield].ShowBlended(MainWindow.WinLeft + sender.WinLeft, MainWindow.WinTop + sender.WinTop, surface, GraphicsState.AlphaBlendTable.LookupTables[_leftOpacity]);
        }
    }

    // FastCombatWin.pas:530
    public void RShieldUserDraw(UIComponent sender, LockedSurface surface)
    {
        if (_rightOpacity == 100)
        {
            DialogHosts.UIGraphics.Shields[_rightShield].Show(MainWindow.WinLeft + sender.WinLeft, MainWindow.WinTop + sender.WinTop, surface);
        }
        else
        {
            DialogHosts.UIGraphics.Shields[_rightShield].ShowBlended(MainWindow.WinLeft + sender.WinLeft, MainWindow.WinTop + sender.WinTop, surface, GraphicsState.AlphaBlendTable.LookupTables[_rightOpacity]);
        }
    }

    // FastCombatWin.pas:539
    public void FCWinHide()
    {
        FinishDialog();
    }

    // FastCombatWin.pas:544
    public void FCWinKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27 || key == 32)
        {
            MainWindow.Visible = false;
        }
    }

    internal static void ShowCombatEvent(FastBattleWindow dialog, CombatEventLog eventValue, ExecuteDoneEvent done)
    {
        Assert(dialog._event == null, "", "FastCombatWin.pas", 159);
        DialogHosts.General.Manager.OverlayPriority = 49;
        dialog._event = eventValue;
        dialog._event.AddRef();
        dialog.Done = done;
        dialog._logbook = eventValue.Logbook;
        InitializeCombatDialog(dialog, dialog._logbook);
        dialog.MainWindow.Visible = true;
    }

    internal static void InitializeCombatDialog(FastBattleWindow dialog, CombatLogbook logbook)
    {
        int raceIndex;
        int partyRace;
        sbyte playerIndex;
        Battle combat;
        CombatParty party;
        Image image;
        dialog._viewer.Initialize(logbook);
        combat = dialog._viewer.Combat;
        dialog._animationFrame = 0;
        for (int I = 0; I <= 7; I++)
        {
            switch (I)
            {
                case 0:
                    image = dialog.Party1Background;
                    break;
                case 1:
                    image = dialog.Party2Background;
                    break;
                case 2:
                    image = dialog.Party3Background;
                    break;
                case 3:
                    image = dialog.Party4Background;
                    break;
                case 4:
                    image = dialog.Party5Background;
                    break;
                case 5:
                    image = dialog.Party6Background;
                    break;
                case 6:
                    image = dialog.Party7Background;
                    break;
                case 7:
                    // { Original reuses the second background for the eighth party. }
                    image = dialog.Party2Background;
                    break;
                default:
                    image = null;
                    break;
            }

            party = combat.CombatData.Parties.GetParty((byte)I);
            if (party != null)
            {
                playerIndex = combat.CombatData.Players.GetPlayers((sbyte)party.PlayerIndex).Player;
                if (playerIndex != 0)
                {
                    partyRace = StrategicMap.Current.Players.GetPlayers(playerIndex).PlayerRace;
                }
                else
                {
                    partyRace = 12;
                }

                image.CurrentImage = partyRace + 21;
                image.Visible = true;
            }
            else
            {
                if (I != 7)
                {
                    image.Visible = false;
                }
            }

            // { Original refreshes both side labels inside the party loop. }
            if (combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer).Player == 0)
            {
                var independents = AbilityRules.TranslateResourceString(ScenarioText.IndependentsText);
                dialog.LeftSideLabel.Text = independents;
            }
            else
            {
                var player = combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer);
                raceIndex = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
                var raceResource = GamePack.Current.RaceResources.GetRaceResource(raceIndex);
                dialog.LeftSideLabel.Text = raceResource.RaceName;
            }

            if (combat.CombatData.Players.GetPlayers(combat.CombatData.DefenderPlayer).Player == 0)
            {
                var independents = AbilityRules.TranslateResourceString(ScenarioText.IndependentsText);
                dialog.RightSideLabel.Text = independents;
            }
            else
            {
                var player = combat.CombatData.Players.GetPlayers(combat.CombatData.DefenderPlayer);
                raceIndex = StrategicMap.Current.Players.GetPlayers(player.Player).PlayerRace;
                var raceResource = GamePack.Current.RaceResources.GetRaceResource(raceIndex);
                dialog.RightSideLabel.Text = raceResource.RaceName;
            }
        }

        dialog._viewer.Play();
        dialog.MainWindow.Drawn = false;
        var endPosition = dialog._viewer.GetEndPosition();
        dialog.SequencePositionSlider.Max = endPosition;
        dialog.SequenceSpeedSlider.Position = 20 - dialog._viewer.Delay;
        if (dialog._viewer.GetEndPosition() != -1)
        {
            dialog.SetPhase(0);
        }
        else
        {
            dialog.SetPhase(3);
        }
    }

}
