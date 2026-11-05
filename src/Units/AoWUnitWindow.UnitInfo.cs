#nullable disable
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Engine.Engine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWComp;
using static Aow.Units.AoWWinMan.AoWWinMan;
namespace Aow.Units.AoWUnitWindow
{
    public partial class TUnitWindow
    {
        public void ShowUnit()
        {
            THero H = default;
            TAbstractUnit U = default;
            if (General.FSelectedUnit != null)
            {
                General.BuildAbilities();
                U = General.FSelectedUnit;
                if (U != null && AoWHSMap.FSeatedPlayer == U.FPlayer)
                {
                    AoWHSMap.FTutorialControl.ShowMessage(64, true, true);
                }

                if (U is THero)
                {
                    H = (THero)U;
                }
                else
                {
                    H = null;
                }

                bool bool__1 = AoWCombatMap == null && General.FUnitList != null;
                if (bool__1)
                {
                    var call__2 = General.IndexOfUnit(U);
                    bool__1 = call__2 > -1;
                }

                if (bool__1 && General.FSelectedUnits[1] > -1)
                {
                    AoWUnitWindow.UnitWindow.PPrev.Visible = true;
                    AoWUnitWindow.UnitWindow.PNext.Visible = true;
                }
                else
                {
                    AoWUnitWindow.UnitWindow.PPrev.Visible = false;
                    AoWUnitWindow.UnitWindow.PNext.Visible = false;
                }

                AoWUnitWindow.UnitWindow.Mana.Visible = false;
                AoWUnitWindow.UnitWindow.ManaIcon.Visible = false;
                AoWUnitWindow.UnitWindow.UnitTab.Items.Clear();
                var call__3 = AoWUnitWindow.AbilitiesRStr;
                var call__4 = TranslateRStr(call__3);
                AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__4);
                var call__5 = AoWUnitWindow.InfoRStr;
                var call__6 = TranslateRStr(call__5);
                AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__6);
                if (U.GetRace() > -1)
                {
                    var call__7 = U.GetRace();
                    var call__8 = TranslateRStr(RaceNamesRStr[call__7]);
                    AoWUnitWindow.UnitWindow.TypeV.Text = call__8;
                    AoWUnitWindow.UnitWindow.RelationIcn.Visible = true;
                    var call__9 = U.GetRace();
                    var call__10 = AoWHSMap.FRaces.GetRace(call__9);
                    var call__11 = call__10.GetPlayerRelation(AoWHSMap.FSeatedPlayer);
                    AoWUnitWindow.UnitWindow.RelationIcn.CurrentImage = (int)call__11 + 11;
                    var call__12 = U.GetRace();
                    var call__13 = AoWHSMap.FRaces.GetRace(call__12);
                    var call__14 = call__13.GetPlayerRelation(AoWHSMap.FSeatedPlayer);
                    var call__15 = TranslateRStr(TStatusRStr[(int)call__14]);
                    AoWUnitWindow.UnitWindow.RelationV.Text = call__15;
                }
                else
                {
                    AoWUnitWindow.UnitWindow.TypeV.Text = (AnsiString)"";
                    AoWUnitWindow.UnitWindow.RelationIcn.Visible = false;
                    var call__16 = AoWUnitWindow.NoneRStr;
                    var call__17 = TranslateRStr(call__16);
                    AoWUnitWindow.UnitWindow.RelationV.Text = call__17;
                }

                if (AoWHSMap.FSeatedPlayer == U.FPlayer && !(U is TLeader) && AoWHSMap.FCombatMap == null)
                {
                    AoWUnitWindow.UnitWindow.DisbandBtn.Visible = true;
                    AoWUnitWindow.UnitWindow.BCloseBtn.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
                }
                else
                {
                    AoWUnitWindow.UnitWindow.DisbandBtn.Visible = false;
                    AoWUnitWindow.UnitWindow.BCloseBtn.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
                }

                AoWUnitWindow.UnitWindow.BCloseBtn.FAligned = false;
                switch (U.GetUnitType())
                {
                    case TUnitType.utCreature:
                        var call__18 = AoWUnitWindow.CreatureRStr;
                        var call__19 = TranslateRStr(call__18);
                        AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + call__19;
                        AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 21;
                        break;
                    case TUnitType.utMachine:
                        var call__20 = AoWUnitWindow.MachineRStr;
                        var call__21 = TranslateRStr(call__20);
                        AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + call__21;
                        AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 22;
                        break;
                    default:
                        if (H == null)
                        {
                            var call__22 = AoWUnitWindow.UnitRStr;
                            var call__23 = TranslateRStr(call__22);
                            AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + call__23;
                        }
                        else
                        {
                            if (H is TLeader)
                            {
                                var call__24 = AoWUnitWindow.LeaderRStr;
                                var call__25 = TranslateRStr(call__24);
                                AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + call__25;
                            }
                            else
                            {
                                var call__26 = AoWUnitWindow.HeroRStr;
                                var call__27 = TranslateRStr(call__26);
                                AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + call__27;
                            }
                        }

                        AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 23;
                        break;
                }

                if (H == null)
                {
                    var call__28 = AoWUnitWindow.LevelRStr;
                    var call__29 = TranslateRStr(call__28);
                    var call__30 = U.GetUnitLevel();
                    AoWUnitWindow.UnitWindow.Level.Text = call__29 + AnsiString.FromBytes(32) + IntToStr(call__30);
                    AoWUnitWindow.UnitWindow.T2Leader.Visible = false;
                    AoWUnitWindow.UnitWindow.T2Text.Visible = true;
                    var call__31 = U.GetDescription();
                    AoWUnitWindow.UnitWindow.T2Memo.Items.Assign(call__31);
                    AoWUnitWindow.UnitWindow.T2SBar.Position = 0;
                    var call__32 = U.GetName();
                    AoWUnitWindow.UnitWindow.UnitName.Text = call__32;
                }
                else
                {
                    if (H.FPlayer == 0)
                    {
                        AoWUnitWindow.UnitWindow.SelPnl.Visible = false;
                        var call__33 = TranslateRStr(AoWUnitWindow.NoLeaderRStr);
                        AoWUnitWindow.UnitWindow.SphereLbl.Text = call__33;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.SelPnl.Visible = true;
                        var call__34 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__35 = call__34.FLeader.GetSphereCount();
                        if (call__35 > 0)
                        {
                            AoWUnitWindow.UnitWindow.Sel1.Visible = true;
                            var call__36 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__37 = call__36.FLeader.GetSpheres(0);
                            var call__38 = General.SphereToImage(call__37);
                            AoWUnitWindow.UnitWindow.Sel1.ILIndexUp = call__38;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel1.Visible = false;
                        }

                        var call__39 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__40 = call__39.FLeader.GetSphereCount();
                        if (call__40 > 1)
                        {
                            AoWUnitWindow.UnitWindow.Sel2.Visible = true;
                            var call__41 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__42 = call__41.FLeader.GetSpheres(1);
                            var call__43 = General.SphereToImage(call__42);
                            AoWUnitWindow.UnitWindow.Sel2.ILIndexUp = call__43;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel2.Visible = false;
                        }

                        var call__44 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__45 = call__44.FLeader.GetSphereCount();
                        if (call__45 > 2)
                        {
                            AoWUnitWindow.UnitWindow.Sel3.Visible = true;
                            var call__46 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__47 = call__46.FLeader.GetSpheres(2);
                            var call__48 = General.SphereToImage(call__47);
                            AoWUnitWindow.UnitWindow.Sel3.ILIndexUp = call__48;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel3.Visible = false;
                        }

                        var call__49 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__50 = call__49.FLeader.GetSphereCount();
                        if (call__50 > 3)
                        {
                            AoWUnitWindow.UnitWindow.Sel4.Visible = true;
                            var call__51 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__52 = call__51.FLeader.GetSpheres(3);
                            var call__53 = General.SphereToImage(call__52);
                            AoWUnitWindow.UnitWindow.Sel4.ILIndexUp = call__53;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel4.Visible = false;
                        }

                        var call__54 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__55 = call__54.FLeader.GetSphereCount();
                        if (call__55 > 4)
                        {
                            AoWUnitWindow.UnitWindow.Sel5.Visible = true;
                            var call__56 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__57 = call__56.FLeader.GetSpheres(4);
                            var call__58 = General.SphereToImage(call__57);
                            AoWUnitWindow.UnitWindow.Sel5.ILIndexUp = call__58;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel5.Visible = false;
                        }

                        var call__59 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__60 = call__59.FLeader.GetSphereCount();
                        if (call__60 > 5)
                        {
                            AoWUnitWindow.UnitWindow.Sel6.Visible = true;
                            var call__61 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__62 = call__61.FLeader.GetSpheres(5);
                            var call__63 = General.SphereToImage(call__62);
                            AoWUnitWindow.UnitWindow.Sel6.ILIndexUp = call__63;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel6.Visible = false;
                        }

                        var call__64 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__65 = call__64.FLeader.GetSphereCount();
                        if (call__65 > 6)
                        {
                            AoWUnitWindow.UnitWindow.Sel7.Visible = true;
                            var call__66 = AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__67 = call__66.FLeader.GetSpheres(6);
                            var call__68 = General.SphereToImage(call__67);
                            AoWUnitWindow.UnitWindow.Sel7.ILIndexUp = call__68;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel7.Visible = false;
                        }

                        var call__69 = TranslateRStr(AoWUnitWindow.LeaderSpheresRStr);
                        AoWUnitWindow.UnitWindow.SphereLbl.Text = call__69;
                    }

                    bool bool__70 = AoWHSMap.FCombat == null && H.FPlayer == AoWHSMap.FSeatedPlayer;
                    if (!bool__70)
                    {
                        bool bool__71 = AoWHSMap.FCombat != null;
                        if (bool__71)
                        {
                            var call__72 = AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.FCombat.FCombatData.FCurrentPlayer);
                            bool__71 = call__72.FPlayer == H.FPlayer;
                        }

                        bool__70 = bool__71;
                    }

                    if (bool__70)
                    {
                        var call__73 = AoWUnitWindow.ItemsRStr;
                        var call__74 = TranslateRStr(call__73);
                        AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__74);
                    }

                    if (H.GetCastingPointsMax() > 0)
                    {
                        AoWUnitWindow.UnitWindow.Mana.Visible = true;
                        var call__75 = H.GetCastingPoints();
                        var call__76 = H.GetCastingPointsMax();
                        AoWUnitWindow.UnitWindow.Mana.Text = IntToStr(call__75) + AnsiString.FromBytes(47) + IntToStr(call__76);
                        AoWUnitWindow.UnitWindow.ManaIcon.Visible = true;
                    }

                    AoWUnitWindow.UnitWindow.ItemUseBtn.Visible = false;
                    AoWUnitWindow.UnitWindow.FGroundOffset = 0;
                    var call__77 = AoWUnitWindow.LevelRStr;
                    var call__78 = TranslateRStr(call__77);
                    var call__79 = H.GetLevel();
                    AoWUnitWindow.UnitWindow.Level.Text = call__78 + AnsiString.FromBytes(32) + IntToStr(call__79);
                    AoWUnitWindow.UnitWindow.T2Leader.Visible = true;
                    AoWUnitWindow.UnitWindow.T2Text.Visible = false;
                    var call__80 = H.GetNickname();
                    AoWUnitWindow.UnitWindow.UnitName.Text = H.FHeroName + AnsiString.FromBytes(32) + call__80;
                }

                var call__81 = AoWUnitWindow.ExpRStr;
                var call__82 = TranslateRStr(call__81);
                var call__83 = U.GetExperience();
                var call__84 = U.GetNextLevelExperience();
                AoWUnitWindow.UnitWindow.XP.Text = call__82 + (AnsiString)" (" + IntToStr(call__83) + AnsiString.FromBytes(47) + IntToStr(call__84) + AnsiString.FromBytes(41);
                AoWUnitWindow.UnitWindow.UnitWin.Cursor = 0;
                var call__85 = AoWUnitWindow.UnitWindow.UnitTab.Items.Count;
                if (call__85 <= AoWUnitWindow.UnitWindow.UnitTab.Selected || AoWUnitWindow.UnitWindow.UnitTab.Selected < 0)
                {
                    AoWUnitWindow.UnitWindow.UnitTab.Selected = 0;
                }

                AoWUnitWindow.UnitWindow.UnitWin.Visible = true;
                bool bool__86 = H != null;
                if (bool__86)
                {
                    bool bool__87 = AoWHSMap.FCombat == null && H.FPlayer == AoWHSMap.FSeatedPlayer;
                    if (!bool__87)
                    {
                        bool bool__88 = AoWHSMap.FCombat != null;
                        if (bool__88)
                        {
                            var call__89 = AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.FCombat.FCombatData.FCurrentPlayer);
                            bool__88 = call__89.FPlayer == H.FPlayer;
                        }

                        bool__87 = bool__88;
                    }

                    bool__86 = bool__87;
                }

                if (bool__86)
                {
                    var call__90 = H.FInventory.GetItem(0);
                    if (call__90 != null)
                    {
                        var call__91 = H.FInventory.GetItem(0);
                        AoWUnitWindow.UnitWindow.BP1.Tag = call__91.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP1.Tag = -1;
                    }

                    var call__92 = H.FInventory.GetItem(1);
                    if (call__92 != null)
                    {
                        var call__93 = H.FInventory.GetItem(1);
                        AoWUnitWindow.UnitWindow.BP2.Tag = call__93.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP2.Tag = -1;
                    }

                    var call__94 = H.FInventory.GetItem(2);
                    if (call__94 != null)
                    {
                        var call__95 = H.FInventory.GetItem(2);
                        AoWUnitWindow.UnitWindow.BP3.Tag = call__95.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP3.Tag = -1;
                    }

                    var call__96 = H.FInventory.GetItem(3);
                    if (call__96 != null)
                    {
                        var call__97 = H.FInventory.GetItem(3);
                        AoWUnitWindow.UnitWindow.BP4.Tag = call__97.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP4.Tag = -1;
                    }

                    var call__98 = H.FInventory.GetItem(4);
                    if (call__98 != null)
                    {
                        var call__99 = H.FInventory.GetItem(4);
                        AoWUnitWindow.UnitWindow.BP5.Tag = call__99.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP5.Tag = -1;
                    }

                    var call__100 = H.FInventory.GetItem(5);
                    if (call__100 != null)
                    {
                        var call__101 = H.FInventory.GetItem(5);
                        AoWUnitWindow.UnitWindow.BP6.Tag = call__101.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP6.Tag = -1;
                    }

                    var call__102 = H.FInventory.GetItem(6);
                    if (call__102 != null)
                    {
                        var call__103 = H.FInventory.GetItem(6);
                        AoWUnitWindow.UnitWindow.BP7.Tag = call__103.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP7.Tag = -1;
                    }

                    var call__104 = H.FInventory.GetItem(7);
                    if (call__104 != null)
                    {
                        var call__105 = H.FInventory.GetItem(7);
                        AoWUnitWindow.UnitWindow.BP8.Tag = call__105.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP8.Tag = -1;
                    }

                    var call__106 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipAttackSlot);
                    if (call__106 != null)
                    {
                        var call__107 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipAttackSlot);
                        AoWUnitWindow.UnitWindow.Attack.Tag = call__107.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.Attack.Tag = -1;
                    }

                    var call__108 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipDefenseSlot);
                    if (call__108 != null)
                    {
                        var call__109 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipDefenseSlot);
                        AoWUnitWindow.UnitWindow.Defend.Tag = call__109.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.Defend.Tag = -1;
                    }

                    var call__110 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipHeadSlot);
                    if (call__110 != null)
                    {
                        var call__111 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipHeadSlot);
                        AoWUnitWindow.UnitWindow.Head.Tag = call__111.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.Head.Tag = -1;
                    }

                    var call__112 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipTorsoSlot);
                    if (call__112 != null)
                    {
                        var call__113 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipTorsoSlot);
                        AoWUnitWindow.UnitWindow.Torso.Tag = call__113.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.Torso.Tag = -1;
                    }

                    var call__114 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipLeftRingSlot);
                    if (call__114 != null)
                    {
                        var call__115 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipLeftRingSlot);
                        AoWUnitWindow.UnitWindow.RingL.Tag = call__115.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.RingL.Tag = -1;
                    }

                    var call__116 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipRightRingSlot);
                    if (call__116 != null)
                    {
                        var call__117 = H.FHeroItems.GetPositionItem(THeroItemPosition.ipRightRingSlot);
                        AoWUnitWindow.UnitWindow.RingR.Tag = call__117.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.RingR.Tag = -1;
                    }

                    AoWUnitWindow.UnitWindow.UpdateGround(0);
                    AoWUnitWindow.SelectedItem = null;
                    AoWUnitWindow.UnitWindow.BP1.Checked = false;
                    AoWUnitWindow.UnitWindow.BP2.Checked = false;
                    AoWUnitWindow.UnitWindow.BP3.Checked = false;
                    AoWUnitWindow.UnitWindow.BP4.Checked = false;
                    AoWUnitWindow.UnitWindow.BP5.Checked = false;
                    AoWUnitWindow.UnitWindow.BP6.Checked = false;
                    AoWUnitWindow.UnitWindow.BP7.Checked = false;
                    AoWUnitWindow.UnitWindow.BP8.Checked = false;
                    AoWUnitWindow.UnitWindow.Attack.Checked = false;
                    AoWUnitWindow.UnitWindow.Defend.Checked = false;
                    AoWUnitWindow.UnitWindow.Head.Checked = false;
                    AoWUnitWindow.UnitWindow.Torso.Checked = false;
                    AoWUnitWindow.UnitWindow.RingL.Checked = false;
                    AoWUnitWindow.UnitWindow.RingR.Checked = false;
                    AoWUnitWindow.UnitWindow.ItemUseBtn.Visible = false;
                }

                if (U != null)
                {
                    var call__118 = U.GetHitPoints();
                    var call__119 = U.GetHits();
                    AoWUnitWindow.UnitWindow.UHits.Text = IntToStr(call__118) + AnsiString.FromBytes(47) + IntToStr(call__119);
                    if (AoWHSMap.FCombatMap == null)
                    {
                        var call__120 = U.GetMovePoints();
                        var call__121 = U.GetMoves();
                        AoWUnitWindow.UnitWindow.UMoves.Text = IntToStr(call__120) + AnsiString.FromBytes(47) + IntToStr(call__121);
                    }
                    else
                    {
                        if (General.FCombatUnit != null)
                        {
                            var call__122 = General.FCombatUnit.FUnit.GetMoves();
                            AoWUnitWindow.UnitWindow.UMoves.Text = IntToStr(General.FCombatUnit.FUnit.FMovePoints) + AnsiString.FromBytes(47) + IntToStr(call__122);
                        }
                    }

                    var call__123 = U.GetAttack();
                    AoWUnitWindow.UnitWindow.UAttack.Text = IntToStr(call__123);
                    var call__124 = U.GetDamage();
                    AoWUnitWindow.UnitWindow.UDamage.Text = IntToStr(call__124);
                    var call__125 = U.GetDefense();
                    AoWUnitWindow.UnitWindow.UDefense.Text = IntToStr(call__125);
                    var call__126 = U.GetResistance();
                    AoWUnitWindow.UnitWindow.UResistance.Text = IntToStr(call__126);
                    var call__127 = U.GetMoveTypes();
                    var call__128 = General.MTtoInt(call__127);
                    AoWUnitWindow.UnitWindow.UMovesIcon.CurrentImage = call__128;
                    if (U.GetManaUpkeep() > 0)
                    {
                        var call__129 = U.GetManaUpkeep();
                        AoWUnitWindow.UnitWindow.UpKeep.Text = IntToStr(call__129);
                        UpkeepIcon.CurrentImage = 52;
                    }
                    else
                    {
                        var call__130 = U.GetUpkeep();
                        AoWUnitWindow.UnitWindow.UpKeep.Text = IntToStr(call__130);
                        UpkeepIcon.CurrentImage = 51;
                    }

                    var call__131 = U.GetAlignment();
                    AAlignment.Visible = call__131 != TAlignment.alNone;
                    switch (U.GetAlignment())
                    {
                        case TAlignment.alPureGood:
                            AAlignment.CurrentImage = 31;
                            break;
                        case TAlignment.alGood:
                            AAlignment.CurrentImage = 32;
                            break;
                        case TAlignment.alPureNeutral:
                        case TAlignment.alNeutral:
                            AAlignment.CurrentImage = 33;
                            break;
                        case TAlignment.alEvil:
                            AAlignment.CurrentImage = 34;
                            break;
                        case TAlignment.alPureEvil:
                            AAlignment.CurrentImage = 35;
                            break;
                    }

                    var call__132 = U.GetAlignment();
                    var call__133 = TranslateRStr(TAlignmentRStr[(int)call__132]);
                    AAlignmentLbl.Text = call__133;
                    AoWUnitWindow.UnitWindow.T1AbilityLB.Items.Clear();
                    AoWUnitWindow.UnitWindow.T1TypeLB.Items.Clear();
                    AoWUnitWindow.UnitWindow.T1SrcLB.Items.Clear();
                    var call__134 = U.GetMorale();
                    var call__135 = TranslateRStr(TMoraleRStr[(int)call__134]);
                    var call__136 = TranslateRStr(MoraleXRStr);
                    var call__137 = AoWE.AoWE.Format(call__136, [call__135]);
                    MoraleLbl.Text = call__137;
                    var call__138 = U.GetUnitType();
                    MoraleLbl.Visible = call__138 != TUnitType.utMachine;
                    if (AoWHSMap.FCombatMap == null)
                    {
                        U.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                        SortStringsEx(T1AbilityLB.Items, AoWUnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                    }
                    else
                    {
                        if (General.FCombatUnit != null && General.FCombatUnit.FUnit != null && General.FCombatUnit.FUnit.FUnit != null)
                        {
                            General.FCombatUnit.FUnit.FUnit.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                            SortStringsEx(T1AbilityLB.Items, AoWUnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                        }
                    }

                    AoWUnitWindow.UnitWindow.T1AbilityLB.Selected = -1;
                    AoWUnitWindow.UnitWindow.T1AbilityLBChange(T1AbilityLB);
                    UnitWin.FDrawn = false;
                }
            }
        }
    }
}
