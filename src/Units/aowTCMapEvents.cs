// Translated from aowTCMapEvents.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.CMapCtrl;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AOWMWindow.AOWMWindow;
using static Aow.Units.AoWTCUnit.AoWTCUnit;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWComp;
using static Aow.Units.aowMapEvents.aowMapEvents;

namespace Aow.Units.aowTCMapEvents
{
    public partial class TTCMapEvents : TDataModule
    {
        public TTCMapEvents(TComponent owner) : base(owner)
        { }

        public TAoWCombatMapControl TCMap;
        public int FChatHeight;
        // aowTCMapEvents.pas:105
        public void TCMapDeActivateMap()
        {
            StopSong();
            SongPlayer.Resume();
            MWindow.MapViewer.HSMap = AoWHSMap;
            AoWSWindow.AoWSWindow.SWindow.Scanner.SetMap(AoWHSMap);
            AoWSWindow.AoWSWindow.SWindow.Scanner.FPreviousWidth = 0;
            AoWSWindow.AoWSWindow.SWindow.Scanner.FIncremental = false;
            AoWSWindow.AoWSWindow.SWindow.Scanner.FSized = false;
            AoWSWindow.AoWSWindow.SWindow.Scanner.FUpdateStep = -1;

            if (FChatHeight > 0)
            {
                ChatWindow.ChatWin.WinHeight = FChatHeight;
            }

            var chatWindow = ChatWindow;
            chatWindow.ChatWin.Alignment.AlignHeight = TAOWAlignHeight.ahBottom;
            chatWindow.ChatWin.Visible = false;

            ChatWindow.FScrollPending = true;
            MWindow.FMapAlignWidth = MWindow.MapWindow.Alignment.AlignWidth;
            MWindow.FMapAlignHeight = MWindow.MapWindow.Alignment.AlignHeight;
            MWindow.FMapRect.Left = MWindow.MapWindow.Alignment.LeftOffset;
            MWindow.FMapRect.Top = MWindow.MapWindow.Alignment.TopOffset;
            if (MWindow.FMapAlignWidth == TAOWAlignWidth.awBoth)
            {
                MWindow.FMapRect.Right = MWindow.MapWindow.Alignment.RightOffset;
            }
            else
            {
                MWindow.FMapRect.Right = MWindow.MapWindow.WinWidth;
            }

            if (MWindow.FMapAlignHeight == TAOWAlignHeight.ahBoth)
            {
                MWindow.FMapRect.Bottom = MWindow.MapWindow.Alignment.BottomOffset;
            }
            else
            {
                MWindow.FMapRect.Bottom = MWindow.MapWindow.WinHeight;
            }

            MWindow.MapWindow.Alignment.LeftOffset = MWindow.FWorldRect.Left;
            MWindow.MapWindow.Alignment.TopOffset = MWindow.FWorldRect.Top;
            MWindow.MapWindow.Alignment.RightOffset = MWindow.FWorldRect.Right;
            MWindow.MapWindow.Alignment.BottomOffset = MWindow.FWorldRect.Bottom;
            MWindow.MapWindow.WinWidth = MWindow.FWorldRect.Right;
            MWindow.MapWindow.WinHeight = MWindow.FWorldRect.Bottom;
            MWindow.MapWindow.Alignment.AlignWidth = MWindow.FWorldAlignWidth;
            MWindow.MapWindow.Alignment.AlignHeight = MWindow.FWorldAlignHeight;
            General.ShowWindows();
            General.Manager.FAligned = false;
            TBWindow.TBWorld.Visible = true;
            TBWindow.TBCombat.Visible = false;
            var player = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            TBWindow.TurnButton.FButtonChecked = player.FSingleProcessCycle;
            TCScanner.TCScannerWin.Visible = false;
            TCUnit.TCUnitWin.Visible = false;
            TCScanner.Scanner.SetMap(null);
            General.Manager.OverlayPriority = 0;
            MapEvents.TheMapGemsChanged(AoWHSMap.FSeatedPlayer);
            MapEvents.TheMapIncomeChanged(AoWHSMap.FSeatedPlayer);
            MapEvents.TheMapPlayerMagicChanged(AoWHSMap.FSeatedPlayer);
        }

        // aowTCMapEvents.pas:157
        public void TCMapDeselected(THexagonSprite HS)
        {
            if (HS is TTacticalCombatUnitHS)
            {
                General.FCombatUnit = null;
                General.FSelectedUnit = null;
                AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
                TBWindow.TCUnitBtn.Visible = false;
                TCUnit.TCUnitWin.Visible = false;
            }
        }

        // aowTCMapEvents.pas:169
        public void TCMapMapChanged(in TMapChangedInfo R)
        {
            TCScanner.Scanner.DrawMap(R.Bounds.Left - 8, R.Bounds.Top - 6, R.Bounds.Right - R.Bounds.Left, R.Bounds.Bottom - R.Bounds.Top);
        }

        // aowTCMapEvents.pas:174
        public void TCMapMouseOverTarget(THexagonSprite HS)
        {
            TAbstractUnit U;
            if (TCUnit.TCUnitWin.Visible)
            {
                TCUnit.TargetSelPnl.Visible = HS != null;
                TCUnit.NoTargetPnl.Visible = HS == null;
                if (HS != null)
                {
                    if (HS is TTacticalCombatUnitHS)
                    {
                        U = ((TTacticalCombatUnitHS)HS).FUnit.FUnit;
                        var name = U.GetName();
                        TCUnit.TargetULbl.Text = name;
                        var hitPoints = U.GetHitPoints();
                        var hits = U.GetHits();
                        TCUnit.AHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                        var movePoints = U.GetMovePoints();
                        var moves = U.GetMoves();
                        TCUnit.AMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                        var attack = U.GetAttack();
                        TCUnit.AAttack.Text = IntToStr(attack);
                        var damage = U.GetDamage();
                        TCUnit.ADamage.Text = IntToStr(damage);
                        var defense = U.GetDefense();
                        TCUnit.ADefense.Text = IntToStr(defense);
                        var resistance = U.GetResistance();
                        TCUnit.AResistance.Text = IntToStr(resistance);
                        var moveTypes = U.GetMoveTypes();
                        var mTtoInt = General.MTtoInt(moveTypes);
                        TCUnit.AMovesIcon.SetCurrent(mTtoInt);
                        U.ListAbilities(TCUnit.USelAbilLb.Items);
                    }
                }
            }
        }

        // aowTCMapEvents.pas:199
        public void TCMapSeatedPlayerChanged(sbyte Player)
        {
            MWindow.MapWindow.Visible = AoWCombatMap.FPlaySounds;
            TCScanner.TCScannerWin.Visible = AoWCombatMap.FPlaySounds;
            if (AoWCombatMap.FSeatedCombatPlayer == -1)
            {
                TBWindow.AutoBtn.Checked = false;
            }
            else
            {
                var player = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedCombatPlayer);
                TBWindow.AutoBtn.Checked = player.FAuto;
            }

            MapEvents.TheMapGemsChanged(Player);
            MapEvents.TheMapIncomeChanged(Player);
            MapEvents.TheMapPlayerMagicChanged(Player);
        }

        // aowTCMapEvents.pas:210
        public void TCMapSelected(THexagonSprite HS)
        {
            if (HS is TTacticalCombatUnitHS)
            {
                General.FCombatUnit = (TTacticalCombatUnitHS)HS;
                RefreshCombatUnit(TCUnit);
            }
        }

        // aowTCMapEvents.pas:219
        public void TCMapShowMessage(in TShowMessageInfo Info)
        {
            TBWindow.TCMsg.TextColor = Info.Color;
            TBWindow.TCMsg.Text = Info.Text;
        }

        // aowTCMapEvents.pas:225
        public void TCMapUnitChanged(TTacticalCombatUnitHS HS)
        {
            if (HS == General.FCombatUnit && HS != null)
            {
                RefreshCombatUnit(TCUnit);
            }
        }

        // aowTCMapEvents.pas:230
        public void TCMapCastSpell(TCombatSpellCaster Caster)
        {
            var selectSpell = General.FCombatUnit.SelectSpell(Caster.FCombatCasterSpellID);
            TCUnit.UAbilLB.Selected = selectSpell;
        }

        // aowTCMapEvents.pas:235
        public void TCMapActivateAbility(TActivateAbilityInfo Info)
        {
            if (Info.FAbilityID == 52)
            {
                SpellBook.ShowCombatCasting(Info);
            }
        }

        // aowTCMapEvents.pas:240
        public void TCMapTurnChanged()
        {
            var player = AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentPlayer);
            TBWindow.TCTurnButton.FCurPlayer = player.FPlayerRace;
            if (TBWindow.TCTurnButton.FCurPlayer == -1)
            {
                TBWindow.TCTurnButton.FCurPlayer = 12;
            }

            TBWindow.TCTurnButton.FLastPlayer = -1;

            General.Manager.OverlayPriority = 49;
            TBWindow.TCPInd.FDrawn = false;
        }
    }

    public static partial class aowTCMapEvents
    {
    }
}
