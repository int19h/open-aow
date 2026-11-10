// Translated from AoWRelations.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
// Translated from AoWRelations.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// diplomatic actions, player indices, race indices, controls
public partial class TRelWindow : TDataModule
{
    public TRelWindow(TComponent owner) : base(owner)
    { }

    public TAOWWindow RelWin;
    public TAOWImage RShield;
    public TAOWImage LShield;
    public TAOWButton MExitBtn;
    public TAOWPanel RelPnl;
    public TAOWPanel RIncomingPnl;
    public TAOWButton MAcceptBtn;
    public TAOWButton MRejectBtn;
    public TAOWVScrollBar MScroll;
    public TAOWImage P1I;
    public TAOWImage P2I;
    public TAOWImage P3I;
    public TAOWImage P4I;
    public TAOWImage P5I;
    public TAOWImage P6I;
    public TAOWImage P7I;
    public TAOWImage P8I;
    public TAOWImage P9I;
    public TAOWImage P10I;
    public TAOWImage P11I;
    public TAOWImage P12I;
    public TAOWImage R1I;
    public TAOWImage R2I;
    public TAOWImage R3I;
    public TAOWImage R4I;
    public TAOWImage R5I;
    public TAOWImage R6I;
    public TAOWImage R7I;
    public TAOWImage R8I;
    public TAOWImage R9I;
    public TAOWImage R10I;
    public TAOWImage R11I;
    public TAOWImage R12I;
    public TAOWImage FaceShadow;
    public TAOWMemo MMemo;
    public TAOWLabel MBGoldLbl;
    public TAOWImage MIcon;
    public TAOWPanel RMsgEditPnl;
    public TAOWPanel RSBribePnl;
    public TAOWVScrollBar SScroll;
    public TAOWMemo SMemo;
    public TAOWHScrollBar SBGoldSlid;
    public TAOWPanel ActionPnl;
    public TAOWPanel LInfoPlayerPnl;
    public TAOWPanel RNewActionPnl;
    public TAOWButton AExit;
    public TAOWLabel APlayer;
    public TAOWPanel LInfoRacePnl;
    public TAOWLabel ARace2;
    public TAOWImage AAlignment;
    public TAOWLabel AAlignmentLbl;
    public TAOWCheckBox P1CB;
    public TAOWCheckBox P2CB;
    public TAOWCheckBox P3CB;
    public TAOWCheckBox P4CB;
    public TAOWCheckBox P5CB;
    public TAOWCheckBox P6CB;
    public TAOWCheckBox P7CB;
    public TAOWCheckBox P8CB;
    public TAOWCheckBox P9CB;
    public TAOWCheckBox P10CB;
    public TAOWCheckBox P11CB;
    public TAOWCheckBox P12CB;
    public TAOWImage FaceFr;
    public TAOWCheckBox R1CB;
    public TAOWCheckBox R2CB;
    public TAOWCheckBox R3CB;
    public TAOWCheckBox R4CB;
    public TAOWCheckBox R5CB;
    public TAOWCheckBox R6CB;
    public TAOWCheckBox R7CB;
    public TAOWCheckBox R8CB;
    public TAOWCheckBox R9CB;
    public TAOWCheckBox R10CB;
    public TAOWCheckBox R11CB;
    public TAOWCheckBox R12CB;
    public TAOWImage PlyrGlow;
    public TAOWLabel NoContact;
    public TAOWButton ASelect;
    public TAOWListBox ActionsLB;
    public TAOWVScrollBar ActionSB;
    public TAOWPanel LBActionsPnl;
    public TAOWLabel ActionLbl;
    public TAOWImage PIRelIcn;
    public TAOWLabel PIRelLbl;
    public TAOWPanel PIRelPnl;
    public TAOWLabel PIRelV;
    public TAOWImage APlyrIcon;
    public TAOWLabel APlyrType;
    public TAOWImage PlStatIcn;
    public TAOWLabel PlStatLbl;
    public TAOWPanel PIStatPnl;
    public TAOWLabel PlStatV;
    public TAOWPanel PTypePnl;
    public TAOWPanel PIFrame;
    public TAOWPanel RIFrame;
    public TAOWPanel RIRelPnl;
    public TAOWPanel RAlignPnl;
    public TAOWImage RlRelIcn;
    public TAOWLabel RIRelLbl;
    public TAOWLabel RlRelV;
    public TAOWLabel MBGoldBribeV;
    public TAOWImage MBGoldIcn;
    public TAOWPanel MBGoldPnl;
    public TAOWPanel MBManaPnl;
    public TAOWLabel MBManaLbl;
    public TAOWLabel MBManaBribeV;
    public TAOWImage MBManaIcn;
    public TAOWLabel MsgLbl;
    public TAOWButton MsgSendBtn;
    public TAOWButton MsgCancelBtn;
    public TAoWFrame RSBribe2;
    public TAOWLabel SBGoldV;
    public TAOWImage SBGoldIcn;
    public TAOWPanel SBGold;
    public TAOWPanel SBMana;
    public TAOWLabel SBManaV;
    public TAOWImage SBManaIcn;
    public TAoWFrame RSBSep1;
    public TAOWLabel SBribeBtn;
    public TAOWHScrollBar SBManaSlid;
    public TAOWImage SBIcon;
    public TAOWButton RSBSendBtn;
    public TAOWButton RBSCanlBtn;
    public TAOWImage StatusGlow;
    public TAOWLabel BribeHeaderLbl;
    public TAOWPanel RMsgRecPnl;
    public TAOWMemo RMMemo;
    public TAOWLabel RMHeaderLbl;
    public TAOWButton RMReplyBtn;
    public TAOWButton RMCancelBtn;
    public TAOWButton RMExitBtn;
    public TAOWPanel IncommingInfoPnl;
    public TAOWButton IIOkBtn;
    public TAOWButton IIExitBtn;
    public TAOWVScrollBar IRScroll;
    public TAOWMemo IIMemo;
    public TAOWImage IIIcon;
    public TIvTranslator Translator;
    public TAOWLabel OfTheLbl;
    public TAOWLabel PRaceLbl;
    public int FSelectedIndex;
    public TDiplomaticAction FAction;
    public TDiplomaticActionList FActions;
    public sbyte FInfoPlayer;
    public sbyte FInfoRace;
    public sbyte FSelectedPlayer;
    public sbyte FSelectedRace;
    public TIntegerList FPlayers;
    public TIntegerList FRaces;
    public TList FControls;
    public TExecuteDoneEvent FDone;
    public TDiplomaticActionEventLog FEvent;
    public int FGlowIntensity;
    public bool FGlowIncreasing;

    // AoWRelations.pas:221
    public void FillPlayers()
    {
        FPlayers.Clear();
        for (int I = 1, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetCount() - 1; I <= last; I++)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen((sbyte)I) != -1 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(I).FStatus == TPlayerStatus.psPlaying)
            {
                FPlayers.Add(I);
            }
        }

        for (int I = 1; I <= 12; I++)
        {
            if (I <= FPlayers.Count)
            {
                ((TAoWComponent)FControls[I + 23]).Visible = true;
                ((Aow.Game.Ui.Controls.TAOWControl)FControls[I + 23]).ToolTipText.Clear();
                var player = FPlayers[I - 1];
                var name = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(player).GetName();
                ((Aow.Game.Ui.Controls.TAOWControl)FControls[I + 23]).ToolTipText.Add(name);
            }
            else
            {
                ((TAoWComponent)FControls[I + 23]).Visible = false;
            }
        }

        var indexOf = FPlayers.IndexOf(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        PlyrGlow.Alignment.LeftOffset = indexOf * 51 - 37 + 51;
        PlyrGlow.FAligned = false;
    }

    // AoWRelations.pas:241
    public void ShowRelations(int Player)
    {
        int Index;
        ClearActions();
        ResetChecks();
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 49;
        FRaces.Clear();
        for (int I = 0, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetCount() - 1; I <= last; I++)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(I).FEnabled)
            {
                FRaces.Add(I);
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            if (I < FRaces.Count)
            {
                ((TAoWComponent)FControls[I + 36]).Visible = true;
                ((Aow.Game.Ui.Controls.TAOWControl)FControls[I + 36]).ToolTipText.Clear();
                var race = FRaces[I];
                var fRacesRace = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(race);
                var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[fRacesRace.FRace]);
                ((Aow.Game.Ui.Controls.TAOWControl)FControls[I + 36]).ToolTipText.Add(raceNamesPlural);
            }
            else
            {
                ((TAoWComponent)FControls[I + 36]).Visible = false;
            }
        }

        FillPlayers();
        FGlowIntensity = 50;
        FGlowIncreasing = false;
        RelWin.Visible = true;
        if (FControls[Player + 23] is TAOWCheckBox playerCheck)
        {
            playerCheck.Checked = true;
        }

        Index = FPlayers.IndexOf(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        if (FControls[Index + 24] is TAOWCheckBox seatedCheck)
        {
            seatedCheck.Checked = true;
        }

        P1CBChange((TAoWComponent)FControls[Index + 24], Index);
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(65, true, true);
    }

    // AoWRelations.pas:270
    public void ResetChecks()
    {
        TAOWCheckBox Check;
        FSelectedIndex = -1;
        RelPnl.FAligned = false;
        RelWin.FDrawn = false;
        PlyrGlow.Alignment.TopOffset = 10;
        for (int I = 24; I <= 35; I++)
        {
            Check = (TAOWCheckBox)FControls[I];
            Check.Checked = false;
            Check.Alignment.TopOffset = 10;
        }

        for (int I = 36; I <= 47; I++)
        {
            Check = (TAOWCheckBox)FControls[I];
            Check.Checked = false;
            Check.Alignment.TopOffset = 185;
        }
    }

    // AoWRelations.pas:293
    public virtual void ShowPlayerActions(sbyte Player)
    {
        int Selected;
        ClearActions();
        HighlightPlayer(Player);
        ShowPlayerInformation(Player);
        Selected = ActionsLB.Selected;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).ListDiplomaticActions(Player, ref FActions);
        for (int I = 0, last = FActions.GetCount() - 1; I <= last; I++)
        {
            var name = FActions.GetDiplomaticActions(I).GetName(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
            var diplomaticAction = FActions.GetDiplomaticActions(I);
            ActionsLB.Items.AddObject(name, diplomaticAction);
        }

        ActionsLB.Selected = Selected;
        ActionsLBChange();
        ActionPnl.Visible = true;
    }

    // AoWRelations.pas:309
    public void ShowPlayerInformation(sbyte Player)
    {
        TDiplomaticRelation Relation;
        FInfoPlayer = Player;
        FInfoRace = -1;
        FaceShadow.Visible = true;
        NoContact.Visible = false;
        PlStatV.Visible = true;
        PlStatIcn.Visible = true;
        PlStatLbl.Visible = true;
        LInfoPlayerPnl.Visible = true;
        LInfoRacePnl.Visible = false;
        var playerPlayerRace = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        LShield.CurrentImage = playerPlayerRace.FPlayerRace + 26;
        LShield.Visible = true;
        RShield.CurrentImage = LShield.CurrentImage;
        RShield.Visible = true;
        var firstDaySeen = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen(Player);
        NoContact.Visible = firstDaySeen == -1;
        APlayer.Visible = !NoContact.Visible;
        OfTheLbl.Visible = APlayer.Visible;
        PRaceLbl.Visible = APlayer.Visible;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).GetFirstDaySeen(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer) != -1)
        {
            if (Player != Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
            {
                Relation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelation(Player, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                var diplomaticRelation = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TDiplomaticRelationRStr[(int)Relation]);
                PlStatV.Text = diplomaticRelation;
                if (Relation != TDiplomaticRelation.drUnknown)
                {
                    PlStatIcn.CurrentImage = (int)Relation + 8;
                }
                else
                {
                    PlStatIcn.CurrentImage = 50;
                }
            }
            else
            {
                PlStatIcn.Visible = false;
                PlStatLbl.Visible = false;
                PlStatV.Visible = false;
            }
        }
        else
        {
            PlStatIcn.Visible = false;
            var noContact = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TRelWindow.NoContactRStr);
            PlStatV.Text = noContact;
        }

        var name = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).GetName();
        APlayer.Text = name;
        OfTheLbl.Text = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.LeadL.Text;
        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[player.FPlayerRace]);
        PRaceLbl.Text = raceNamesPlural;
        var playerPlayerType = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        if (TSet<TPlayerType>.Of(TPlayerType.ptHuman, TPlayerType.ptCPU1, TPlayerType.ptCPU2, TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(playerPlayerType.FPlayerType))
        {
            switch (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FPlayerType)
            {
                case TPlayerType.ptCPU1:
                    APlyrIcon.CurrentImage = 1;
                    break;
                case TPlayerType.ptCPU2:
                    APlyrIcon.CurrentImage = 2;
                    break;
                case TPlayerType.ptCPU3:
                    APlyrIcon.CurrentImage = 3;
                    break;
                case TPlayerType.ptCPU4:
                    APlyrIcon.CurrentImage = 4;
                    break;
                case TPlayerType.ptCPU5:
                    APlyrIcon.CurrentImage = 5;
                    break;
                default:
                    APlyrIcon.CurrentImage = 0;
                    break;
            }

            APlyrIcon.Visible = true;
        }
        else
        {
            APlyrIcon.Visible = false;
        }

        var player2 = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
        var playerType = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TPlayerTypeRStr[(int)player2.FPlayerType]);
        APlyrType.Text = playerType;
    }

    // AoWRelations.pas:370
    public void HighlightPlayer(sbyte Player)
    {
        TAOWCheckBox Check;
        ResetChecks();
        FSelectedPlayer = Player;
        FSelectedRace = -1;
        FSelectedIndex = FPlayers.IndexOf(Player);
        if (FSelectedIndex != -1)
        {
            Check = (TAOWCheckBox)FControls[FSelectedIndex + 24];
            Check.Alignment.TopOffset = 20;
            Check.Checked = true;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == Player)
            {
                PlyrGlow.Alignment.TopOffset = Check.Alignment.TopOffset;
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            if (I < FPlayers.Count)
            {
                ((TAoWComponent)FControls[I]).Visible = true;
                if (FPlayers[I] == Player)
                {
                    ((TAOWImage)FControls[I]).CurrentImage = 2;
                }
                else
                {
                    if (I > FRaces.Count)
                    {
                        ((TAOWImage)FControls[I]).CurrentImage = 0;
                    }
                    else
                    {
                        ((TAOWImage)FControls[I]).CurrentImage = 1;
                    }
                }
            }
            else
            {
                ((TAOWImage)FControls[I]).CurrentImage = 2;
            }

            switch (((TAOWImage)FControls[I]).CurrentImage)
            {
                case 1:
                    ((Aow.Game.Ui.Controls.TAOWControl)FControls[I]).ToolTipText.Clear();
                    if (FSelectedPlayer != -1)
                    {
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
                        {
                            var player = FPlayers[I];
                            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                            {
                                var player2 = FPlayers[I];
                                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer).GetFirstDaySeen((sbyte)player2) != -1)
                                {
                                    var player3 = FPlayers[I];
                                    var diplomaticRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelation(FSelectedPlayer, (sbyte)player3);
                                    ((Aow.Game.Ui.Controls.TAOWControl)FControls[I]).ToolTipText.Add(Aow.Game.Magic.Abilities.GameTables.TDiplomaticRelationRStr[(int)diplomaticRelation]);
                                }
                            }
                        }
                    }

                    break;
                default:
                    ((Aow.Game.Ui.Controls.TAOWControl)FControls[I]).ToolTipText.Clear();
                    break;
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            if (I < FRaces.Count)
            {
                ((TAOWImage)FControls[I + 12]).CurrentImage = 4;
            }
            else
            {
                ((TAOWImage)FControls[I + 12]).CurrentImage = 5;
            }

            switch (((TAOWImage)FControls[I + 12]).CurrentImage)
            {
                case 4:
                    ((Aow.Game.Ui.Controls.TAOWControl)FControls[I + 12]).ToolTipText.Clear();
                    if (FSelectedPlayer != -1)
                    {
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
                        {
                            var race = FRaces[I];
                            var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(race).GetPlayerRelation(FSelectedPlayer);
                            var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TStatusRStr[(int)playerRelation]);
                            var race2 = FRaces[I];
                            var playerRelationValue = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(race2).GetPlayerRelationValue(FSelectedPlayer);
                            ((Aow.Game.Ui.Controls.TAOWControl)FControls[12 + I]).ToolTipText.Add(status + " (" + IntToStr(playerRelationValue) + ")");
                        }
                    }

                    break;
                default:
                    ((Aow.Game.Ui.Controls.TAOWControl)FControls[I + 12]).ToolTipText.Clear();
                    break;
            }
        }
    }

    // AoWRelations.pas:427
    public void ShowRaceInformation(byte Race)
    {
        FInfoPlayer = -1;
        FInfoRace = (sbyte)Race;
        LShield.Visible = false;
        RShield.Visible = false;
        FaceShadow.Visible = true;
        ActionPnl.Visible = true;
        LInfoPlayerPnl.Visible = false;
        LInfoRacePnl.Visible = true;
        var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace((sbyte)Race).GetPlayerRelation(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        RlRelIcn.CurrentImage = (int)playerRelation + 11;
        var playerRelation2 = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace((sbyte)Race).GetPlayerRelation(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TStatusRStr[(int)playerRelation2]);
        RlRelV.Text = status;
        var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[(sbyte)Race]);
        ARace2.Text = raceNamesPlural;
        switch (Race)
        {
            case >= 0 and <= 3:
                var neutral = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.NeutralRStr);
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AlignmentXRStr), [neutral]);
                AAlignmentLbl.Text = format;
                AAlignment.CurrentImage = 33;
                break;
            case >= 4 and <= 7:
                var good = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TRelWindow.GoodRStr);
                var aoWEFormat = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AlignmentXRStr), [good]);
                AAlignmentLbl.Text = aoWEFormat;
                AAlignment.CurrentImage = 31;
                break;
            case >= 8 and <= 11:
                var evil = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TRelWindow.EvilRStr);
                var format2 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AlignmentXRStr), [evil]);
                AAlignmentLbl.Text = format2;
                AAlignment.CurrentImage = 35;
                break;
        }
    }

    // AoWRelations.pas:456
    public void HighlightRace(sbyte Race)
    {
        int Index;
        TAOWCheckBox Check;
        ResetChecks();
        FSelectedPlayer = -1;
        FSelectedRace = Race;
        Index = FRaces.IndexOf(Race);
        Check = (TAOWCheckBox)FControls[Index + 36];
        Check.Alignment.TopOffset = 175;
        Check.Checked = true;
        FSelectedIndex = 20 + Index;
        for (int I = 0; I <= 11; I++)
        {
            if (I < FPlayers.Count)
            {
                ((TAOWImage)FControls[I]).CurrentImage = 1;
            }
            else
            {
                ((TAOWImage)FControls[I]).CurrentImage = 2;
            }

            switch (((TAOWImage)FControls[I]).CurrentImage)
            {
                case 1:
                    ((Aow.Game.Ui.Controls.TAOWControl)FControls[I]).ToolTipText.Clear();
                    var player = FPlayers[I];
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                    {
                        var player2 = FPlayers[I];
                        var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FSelectedRace).GetPlayerRelation((sbyte)player2);
                        var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TStatusRStr[(int)playerRelation]);
                        var player3 = FPlayers[I];
                        var playerRelationValue = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FSelectedRace).GetPlayerRelationValue((sbyte)player3);
                        ((Aow.Game.Ui.Controls.TAOWControl)FControls[I]).ToolTipText.Add(status + " (" + IntToStr(playerRelationValue) + ")");
                    }

                    break;
                default:
                    ((Aow.Game.Ui.Controls.TAOWControl)FControls[I]).ToolTipText.Clear();
                    break;
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            ((TAOWImage)FControls[I + 12]).CurrentImage = 5;
            ((Aow.Game.Ui.Controls.TAOWControl)FControls[I + 12]).ToolTipText.Clear();
        }
    }

    // AoWRelations.pas:490
    public void SetAction(TDiplomaticAction Action)
    {
        if (Action != FAction)
        {
            if (FAction != null)
            {
                FAction.Release();
            }

            FAction = Action;
            if (FAction != null)
            {
                FAction.AddRef();
            }
        }
    }

    // AoWRelations.pas:500
    public void FinishEvent()
    {
        TExecuteDoneEvent Done;
        TDiplomaticActionEventLog Pending;
        TDiplomaticActionEventLog Event;
        Pending = FEvent;
        if (Pending != null)
        {
            Done = FDone;
            Event = Pending;
            FDone = null;
            FEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }
    }

    // AoWRelations.pas:516
    public void FinishDialog()
    {
        SetAction(null);
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        FinishEvent();
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(71, true, true);
    }

    // AoWRelations.pas:524
    public void ClearActions()
    {
        SetAction(null);
        RNewActionPnl.Visible = true;
        RIncomingPnl.Visible = false;
        RMsgEditPnl.Visible = false;
        RSBribePnl.Visible = false;
        RMsgRecPnl.Visible = false;
        IncommingInfoPnl.Visible = false;
        FActions.Clear();
        ActionsLB.Items.Clear();
        SMemo.Items.Clear();
        MMemo.Items.Clear();
        ActionsLBChange();
    }

    // AoWRelations.pas:540
    public void RelWindowCreate()
    {
        FActions = new TDiplomaticActionList(null);
        FRaces = new TIntegerList();
        FPlayers = new TIntegerList();
        FControls = new TList();
        FControls.Add(P1I);
        FControls.Add(P2I);
        FControls.Add(P3I);
        FControls.Add(P4I);
        FControls.Add(P5I);
        FControls.Add(P6I);
        FControls.Add(P7I);
        FControls.Add(P8I);
        FControls.Add(P9I);
        FControls.Add(P10I);
        FControls.Add(P11I);
        FControls.Add(P12I);
        FControls.Add(R1I);
        FControls.Add(R2I);
        FControls.Add(R3I);
        FControls.Add(R4I);
        FControls.Add(R5I);
        FControls.Add(R6I);
        FControls.Add(R7I);
        FControls.Add(R8I);
        FControls.Add(R9I);
        FControls.Add(R10I);
        FControls.Add(R11I);
        FControls.Add(R12I);
        FControls.Add(P1CB);
        FControls.Add(P2CB);
        FControls.Add(P3CB);
        FControls.Add(P4CB);
        FControls.Add(P5CB);
        FControls.Add(P6CB);
        FControls.Add(P7CB);
        FControls.Add(P8CB);
        FControls.Add(P9CB);
        FControls.Add(P10CB);
        FControls.Add(P11CB);
        FControls.Add(P12CB);
        FControls.Add(R1CB);
        FControls.Add(R2CB);
        FControls.Add(R3CB);
        FControls.Add(R4CB);
        FControls.Add(R5CB);
        FControls.Add(R6CB);
        FControls.Add(R7CB);
        FControls.Add(R8CB);
        FControls.Add(R9CB);
        FControls.Add(R10CB);
        FControls.Add(R11CB);
        FControls.Add(R12CB);
    }

    // AoWRelations.pas:596
    public void RelWindowDestroy()
    {
        FActions?.Dispose();
        FControls?.Dispose();
        FPlayers?.Dispose();
        FRaces?.Dispose();
    }

    // AoWRelations.pas:604
    public void ExitClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        RelWin.Visible = false;
    }

    // AoWRelations.pas:610
    public void P1CBChange(TAoWComponent Sender, int Slot)
    {
        // The original SetChecked call on an image is undefined, so skipped. An image has FChecked and no SetChecked. The action branch runs only for a check box.
        if (Sender.FChecked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }

        RelPnl.FAligned = false;
        RelWin.FDrawn = false;
        if (Sender.FChecked && Sender is TAOWCheckBox)
        {
            if (FAction == null)
            {
                ShowPlayerActions((sbyte)FPlayers[Slot]);
            }
            else
            {
                HighlightPlayer((sbyte)FPlayers[Slot]);
            }
        }
    }

    // AoWRelations.pas:623
    public void R1CBChange(TAOWCheckBox Sender, int Slot)
    {
        TAOWCheckBox Check;
        Check = Sender;
        if (Check.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }

        RelPnl.FAligned = false;
        if (FAction == null)
        {
            ClearActions();
            HighlightRace((sbyte)FRaces[Slot - 20]);
            ShowRaceInformation((byte)FRaces[Slot - 20]);
        }
        else
        {
            HighlightRace((sbyte)FRaces[Slot - 20]);
        }
    }

    // AoWRelations.pas:639
    public void P1CBPreDraw(TAOWCheckBox Sender, TSurfaceDesc Surface, int Slot)
    {
        sbyte Player;
        TRect R;
        TAoWComponent Component;
        Component = Sender;
        if (Slot < FPlayers.Count)
        {
            Player = (sbyte)FPlayers[Slot];
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen(Player) == -1)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.RelWin[28].ShowClipped(Component.WinLeft, Component.WinTop, Surface);
            }
            else
            {
                R = Rect(Component.WinLeft + 1, Component.WinTop + 1, Component.WinLeft + Component.WinWidth - 1, Component.WinTop + Component.WinHeight - 1);
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).GetFace() != null)
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).GetFace().ShowStretched(R, Surface);
                }
            }

            var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.RelWin[player.FPlayerRace + 16].ShowClipped(Component.WinLeft, Component.WinTop, Surface);
        }
    }

    // AoWRelations.pas:659
    public void R1CBPreDraw(TAOWCheckBox Sender, TSurfaceDesc Surface, int Slot)
    {
        TAoWComponent Control;
        sbyte Race;
        TRect R;
        TDiplomaticAction Action;
        int Modifier;
        Control = Sender;
        if (Slot - 20 < FRaces.Count)
        {
            Race = (sbyte)FRaces[Slot - 20];
            R = Rect(Control.WinLeft + 1, Control.WinTop + 1, Control.WinLeft + Control.WinWidth - 1, Control.WinTop + Control.WinHeight - 1);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(Race).GetFace().ShowStretched(R, Surface);
            Action = FAction;
            if (Action == null && RNewActionPnl.Visible && ActionsLB.Selected != -1)
            {
                Action = (TDiplomaticAction)ActionsLB.Items.Objects[ActionsLB.Selected];
            }

            if (Action != null)
            {
                Modifier = Action.GetRelationModifier(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer, Race);
                if (Modifier != 0)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age10.Color = 16777215;
                    Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age10.SetTextCentered(R.Left + (R.Right - R.Left) / 2, R.Top + 50, IntToStr(Modifier), Surface);
                }
            }
        }
    }

    // AoWRelations.pas:689
    public void P1IDraw(TAOWImage Sender, TSurfaceDesc Surface, int Slot)
    {
        int Player;
        TDiplomaticRelation Relation;
        int Image;
        var component = Sender;
        if (FPlayers.Count > Slot && Slot != FSelectedIndex)
        {
            Player = FPlayers[Slot];
            if (FSelectedPlayer != -1)
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
                {
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen((sbyte)Player) != -1)
                    {
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer).GetFirstDaySeen((sbyte)Player) != -1)
                        {
                            Relation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelation(FSelectedPlayer, (sbyte)Player);
                            if (Relation != TDiplomaticRelation.drUnknown)
                            {
                                Image = (int)Relation + 8;
                            }
                            else
                            {
                                Image = 50;
                            }

                            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.RelWin[Image].Show(component.WinLeft, component.WinTop + 8, Surface);
                        }
                    }
                }
            }
            else
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen((sbyte)Player) != -1)
                {
                    var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FSelectedRace).GetPlayerRelation((sbyte)Player);
                    Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.RelWin[(int)playerRelation + 11].ShowClipped(component.WinLeft, component.WinTop + 8, Surface);
                }
            }
        }
    }

    // AoWRelations.pas:715
    public void R1IDraw(TAOWImage Sender, TSurfaceDesc Surface, int Slot)
    {
        int RaceIndex;
        TAoWComponent S;
        S = Sender;
        if (FSelectedPlayer != -1 && Slot - 20 < FRaces.Count)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
            {
                RaceIndex = FRaces[Slot - 20];
                var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(RaceIndex).GetPlayerRelation(FSelectedPlayer);
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.RelWin[(int)playerRelation + 11].ShowClipped(S.WinLeft, S.WinTop + 15, Surface);
            }
        }
    }

    // AoWRelations.pas:723
    public void MAcceptBtnClick()
    {
        string Reason = "";
        TDiplomaticAction Action;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Action = FAction;
        if (Action != null)
        {
            if (Action.Accept(ref Reason))
            {
                FinishEvent();
                ShowPlayerActions(FAction.FSource);
            }
            else
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(Reason);
            }
        }
    }

    // AoWRelations.pas:739
    public void MRejectBtnClick()
    {
        string Reason = "";
        TDiplomaticAction Action;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Action = FAction;
        if (Action != null)
        {
            if (Action.Reject(ref Reason))
            {
                FinishEvent();
                ShowPlayerActions(FAction.FSource);
            }
            else
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(Reason);
            }
        }
    }

    // AoWRelations.pas:755
    public void PlyrGlowUpdate()
    {
        if (FGlowIncreasing)
        {
            FGlowIntensity += 1;
        }
        else
        {
            FGlowIntensity -= 1;
        }

        if (FGlowIntensity == 20)
        {
            FGlowIncreasing = true;
        }
        else
        {
            if (FGlowIntensity == 50)
            {
                FGlowIncreasing = false;
            }
        }

        PlyrGlow.FDrawn = false;
    }

    // AoWRelations.pas:763
    public void PlyrGlowDraw(TSurfaceDesc Surface)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.RelWin[44].ShowBlended(PlyrGlow.WinLeft, PlyrGlow.WinTop, Surface, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[FGlowIntensity]);
    }

    // AoWRelations.pas:768
    public void FaceFrPreDraw(TSurfaceDesc Surface)
    {
        TRect R;
        R = Rect(FaceFr.WinLeft + 1, FaceFr.WinTop, FaceFr.WinLeft + FaceFr.WinWidth, FaceFr.WinTop + FaceFr.WinHeight);
        if (FAction == null)
        {
            if (FSelectedPlayer != -1)
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer).FLeader.ShowFace(R, Surface);
                }
            }
            else
            {
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FSelectedRace).GetFace().Show(FaceFr.WinLeft + 1, FaceFr.WinTop + 3, Surface);
            }
        }
        else
        {
            if (FEvent != null && FEvent.FAction.Proposal.SignedAsOtherPlayer)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FEvent.FAction.FOtherPlayer).FLeader.ShowFace(R, Surface);
            }
            else
            {
                if (FAction.FSource == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FAction.FTarget).FLeader.ShowFace(R, Surface);
                }
                else
                {
                    if (FAction.FReaction == TDiplomaticActionReaction.darNone)
                    {
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FAction.FSource).FLeader.ShowFace(R, Surface);
                    }
                    else
                    {
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FAction.FTarget).FLeader.ShowFace(R, Surface);
                    }
                }
            }
        }
    }

    // AoWRelations.pas:794. The offer enters the proposal when the player sends it (MsgSendActionBtn).
    public void SBGoldSlidChange()
    {
        SBGoldV.Text = IntToStr(SBGoldSlid.Position);
    }

    // AoWRelations.pas:800
    public void SBManaSlidChange()
    {
        SBManaV.Text = IntToStr(SBManaSlid.Position);
    }

    // AoWRelations.pas:806
    public void RelWinHide()
    {
        FinishDialog();
    }

    // AoWRelations.pas:811
    public void ASelectClick()
    {
        string Reason = "";
        if (ActionsLB.Selected != -1)
        {
            SetAction((TDiplomaticAction)ActionsLB.Items.Objects[ActionsLB.Selected]);
            var view = FAction.Proposal.View;
            if (view == ProposalView.Notice || view == ProposalView.Threat)
            {
                if (!FAction.Send(ref Reason))
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(Reason);
                }
            }
            else if (view == ProposalView.Offer)
            {
                RNewActionPnl.Visible = false;
                SBIcon.Visible = true;
                RSBribePnl.Visible = true;
                SBGoldSlid.Position = 0;
                SBManaSlid.Position = 0;
                var playerGems = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                SBGoldSlid.Max = playerGems.FGems;
                var playerMagicControl = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                SBManaSlid.Max = playerMagicControl.FMagicControl.FMana;
                SBManaSlidChange();
                SBGoldSlidChange();
                var name = FAction.GetName(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                BribeHeaderLbl.Text = name;
                if (FAction.Proposal.Icon >= 0)
                {
                    SBIcon.CurrentImage = FAction.Proposal.Icon;
                }
                else
                {
                    SBIcon.Visible = false;
                }
            }
            else if (view == ProposalView.Message)
            {
                RNewActionPnl.Visible = false;
                RMsgEditPnl.Visible = true;
            }
        }
    }

    // AoWRelations.pas:851
    public void ActionsLBChange()
    {
        RelPnl.FDrawn = false;
        if (ActionsLB.Selected != -1)
        {
            ASelect.ILIndexDown = 37;
            ASelect.ILIndexLit = 40;
            ASelect.ILIndexUp = 34;
        }
        else
        {
            ASelect.ILIndexDown = 46;
            ASelect.ILIndexLit = 49;
            ASelect.ILIndexUp = 43;
        }
    }

    // AoWRelations.pas:868
    public void CancelSelectedAction()
    {
        if (FEvent != null)
        {
            FinishEvent();
        }

        ShowPlayerActions(FAction.FTarget);
        RNewActionPnl.Visible = true;
        RSBribePnl.Visible = false;
        RMsgEditPnl.Visible = false;
    }

    // AoWRelations.pas:877
    public void ChangeActionMessage(TAOWMemo Sender)
    {
        if (FAction != null)
        {
            var text = Sender.Items.Text;
            FAction.Proposal = FAction.Proposal with { Message = DiplomaticProposal.Lines(text) };
        }
    }

    // AoWRelations.pas:882
    public void MsgSendActionBtn()
    {
        string Reason = "";
        sbyte Player;
        TDiplomaticAction Action;
        Action = FAction;
        if (Action != null)
        {
            Action.Proposal = Action.Proposal.WithOffer(SBGoldSlid.Position, SBManaSlid.Position);
            if (Action.Send(ref Reason))
            {
                Player = FAction.FTarget;
                if (FEvent != null && FEvent.FAction.Proposal.SignedAsOtherPlayer)
                {
                    Player = FEvent.FAction.FOtherPlayer;
                }

                FinishEvent();
                ShowPlayerActions(Player);
            }
            else
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(Reason);
            }
        }
    }

    // AoWRelations.pas:901
    public void ReplyBtnClick()
    {
        TDiplomaticAction Action;
        Action = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FAction.FTarget).CreateDiplomaticAction(new SendMessage(), FAction.FSource, -1);
        ClearActions();
        SetAction(Action);
        RNewActionPnl.Visible = false;
        RMsgEditPnl.Visible = true;
    }

    // AoWRelations.pas:912
    public void RMCancelBtnClick()
    {
        if (FEvent != null)
        {
            FinishEvent();
        }

        ShowPlayerActions(FAction.FSource);
    }

    // AoWRelations.pas:918
    public void IIOkBtnClick()
    {
        FinishEvent();
        if (FAction.FSource == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
        {
            ShowPlayerActions(FAction.FTarget);
        }
        else
        {
            ShowPlayerActions(FAction.FSource);
        }
    }

    // AoWRelations.pas:925
    public void RelWinKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            RelWin.Visible = false;
        }
    }

    private static readonly string EvilRStr = "Evil";

    private static readonly string GoodRStr = "Good";

    private static readonly string NoContactRStr = "No Contact";

    internal static void ShowDiplomaticEvent(TRelWindow Dialog, TDiplomaticActionEventLog Event, TExecuteDoneEvent Done)
    {
        Dialog.ClearActions();
        Dialog.ResetChecks();
        // { Original reports its source filename, independent of this checkout. }
        if (Dialog.FEvent != null)
        {
            Aow.Game.Ui.Dialogs.WindowActions.OriginalAssert("Assertion failure", "D:\\AoWDev\\AoWRelations.pas", 663);
        }

        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Event.AddRef();

        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FStatus == TPlayerStatus.psPlaying)
        {
            Dialog.ShowRelations(Event.FAction.FTarget);
            Dialog.SetAction(Event.FAction);
            var proposal = Dialog.FAction.Proposal;
            var view = proposal.View;
            if (view == ProposalView.Message)
            {
                var from = proposal.SignedAsOtherPlayer ? Dialog.FAction.FOtherPlayer : Dialog.FAction.FSource;
                Dialog.HighlightPlayer(from);
                Dialog.ShowPlayerInformation(from);
                Dialog.RNewActionPnl.Visible = false;
                Dialog.RMsgRecPnl.Visible = true;
                var text = proposal.MessageText;
                Dialog.RMMemo.Items.Text = text;
                var name = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(from).GetName();
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.MessageFromXRStr), [name]);
                Dialog.RMHeaderLbl.Text = format;
            }
            else if (view == ProposalView.Offer)
            {
                Dialog.RNewActionPnl.Visible = false;
                if (Dialog.FAction.FReaction == TDiplomaticActionReaction.darNone)
                {
                    Dialog.HighlightPlayer(Dialog.FAction.FSource);
                    Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                    Dialog.RIncomingPnl.Visible = true;
                    var (_, gold, mana) = proposal.ToFields();
                    Dialog.MBGoldBribeV.Text = IntToStr(gold);
                    Dialog.MBManaBribeV.Text = IntToStr(mana);
                    var text2 = proposal.MessageText;
                    Dialog.MMemo.Items.Text = text2;
                    if (proposal.Icon >= 0)
                    {
                        Dialog.MIcon.CurrentImage = proposal.Icon;
                    }
                }
                else
                {
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == Dialog.FAction.FSource && Dialog.FAction.FReaction != (TDiplomaticActionReaction)3)
                    {
                        Dialog.HighlightPlayer(Dialog.FAction.FTarget);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FTarget);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.IncommingInfoPnl.Visible = true;
                        var text2 = proposal.MessageText;
                        Dialog.IIMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            Dialog.IIIcon.CurrentImage = proposal.Icon;
                        }
                    }
                    else
                    {
                        Dialog.FinishEvent();
                        if (Dialog.FAction.FReaction == (TDiplomaticActionReaction)3)
                        {
                            Dialog.ShowPlayerActions(Dialog.FAction.FTarget);
                        }
                        else
                        {
                            Dialog.ShowPlayerActions(Dialog.FAction.FSource);
                        }
                    }
                }
            }
            else if (view == ProposalView.Notice)
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == Dialog.FAction.FSource)
                {
                    Dialog.FinishEvent();
                    Dialog.ShowPlayerActions(Dialog.FAction.FTarget);
                }
                else
                {
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == Dialog.FAction.FTarget)
                    {
                        Dialog.HighlightPlayer(Dialog.FAction.FSource);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.IncommingInfoPnl.Visible = true;
                        var text2 = proposal.MessageText;
                        Dialog.IIMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            Dialog.IIIcon.CurrentImage = proposal.Icon;
                        }
                    }
                    else
                    {
                        Dialog.FinishEvent();
                        Dialog.ShowPlayerActions(Dialog.FAction.FSource);
                    }
                }
            }
            else if (view == ProposalView.Threat)
            {
                Dialog.HighlightPlayer(Dialog.FAction.FSource);
                Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                Dialog.RNewActionPnl.Visible = false;
                Dialog.IncommingInfoPnl.Visible = true;
                var text3 = proposal.MessageText;
                Dialog.IIMemo.Items.Text = text3;
                if (proposal.Icon >= 0)
                {
                    Dialog.IIIcon.CurrentImage = proposal.Icon;
                }
            }
        }
        else
        {
            Dialog.FinishDialog();
        }
    }

}
