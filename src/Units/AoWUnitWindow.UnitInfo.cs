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

                if (AoWCombatMap == null && General.FUnitList != null && General.IndexOfUnit(U) > -1 && General.FSelectedUnits[1] > -1)
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
                var abilities = TranslateRStr(AoWUnitWindow.AbilitiesRStr);
                AoWUnitWindow.UnitWindow.UnitTab.Items.Add(abilities);
                var info = TranslateRStr(AoWUnitWindow.InfoRStr);
                AoWUnitWindow.UnitWindow.UnitTab.Items.Add(info);
                if (U.GetRace() > -1)
                {
                    var race = U.GetRace();
                    var raceNames = TranslateRStr(RaceNamesRStr[race]);
                    AoWUnitWindow.UnitWindow.TypeV.Text = raceNames;
                    AoWUnitWindow.UnitWindow.RelationIcn.Visible = true;
                    var uRace = U.GetRace();
                    var playerRelation = AoWHSMap.FRaces.GetRace(uRace).GetPlayerRelation(AoWHSMap.FSeatedPlayer);
                    AoWUnitWindow.UnitWindow.RelationIcn.CurrentImage = (int)playerRelation + 11;
                    var race2 = U.GetRace();
                    var playerRelation2 = AoWHSMap.FRaces.GetRace(race2).GetPlayerRelation(AoWHSMap.FSeatedPlayer);
                    var status = TranslateRStr(TStatusRStr[(int)playerRelation2]);
                    AoWUnitWindow.UnitWindow.RelationV.Text = status;
                }
                else
                {
                    AoWUnitWindow.UnitWindow.TypeV.Text = (AnsiString)"";
                    AoWUnitWindow.UnitWindow.RelationIcn.Visible = false;
                    var none = TranslateRStr(AoWUnitWindow.NoneRStr);
                    AoWUnitWindow.UnitWindow.RelationV.Text = none;
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
                        var creature = TranslateRStr(AoWUnitWindow.CreatureRStr);
                        AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + creature;
                        AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 21;
                        break;
                    case TUnitType.utMachine:
                        var machine = TranslateRStr(AoWUnitWindow.MachineRStr);
                        AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + machine;
                        AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 22;
                        break;
                    default:
                        if (H == null)
                        {
                            var unit = TranslateRStr(AoWUnitWindow.UnitRStr);
                            AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + unit;
                        }
                        else
                        {
                            if (H is TLeader)
                            {
                                var leader = TranslateRStr(AoWUnitWindow.LeaderRStr);
                                AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + leader;
                            }
                            else
                            {
                                var hero = TranslateRStr(AoWUnitWindow.HeroRStr);
                                AoWUnitWindow.UnitWindow.TypeV.Text = AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32) + hero;
                            }
                        }

                        AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 23;
                        break;
                }

                if (H == null)
                {
                    var level = TranslateRStr(AoWUnitWindow.LevelRStr);
                    var unitLevel = U.GetUnitLevel();
                    AoWUnitWindow.UnitWindow.Level.Text = level + AnsiString.FromBytes(32) + IntToStr(unitLevel);
                    AoWUnitWindow.UnitWindow.T2Leader.Visible = false;
                    AoWUnitWindow.UnitWindow.T2Text.Visible = true;
                    var description = U.GetDescription();
                    AoWUnitWindow.UnitWindow.T2Memo.Items.Assign(description);
                    AoWUnitWindow.UnitWindow.T2SBar.Position = 0;
                    var name = U.GetName();
                    AoWUnitWindow.UnitWindow.UnitName.Text = name;
                }
                else
                {
                    if (H.FPlayer == 0)
                    {
                        AoWUnitWindow.UnitWindow.SelPnl.Visible = false;
                        var noLeader = TranslateRStr(AoWUnitWindow.NoLeaderRStr);
                        AoWUnitWindow.UnitWindow.SphereLbl.Text = noLeader;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.SelPnl.Visible = true;
                        if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 0)
                        {
                            AoWUnitWindow.UnitWindow.Sel1.Visible = true;
                            var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(0);
                            var sphereToImage = General.SphereToImage(sphere);
                            AoWUnitWindow.UnitWindow.Sel1.ILIndexUp = sphereToImage;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel1.Visible = false;
                        }

                        if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 1)
                        {
                            AoWUnitWindow.UnitWindow.Sel2.Visible = true;
                            var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(1);
                            var sphereToImage = General.SphereToImage(sphere);
                            AoWUnitWindow.UnitWindow.Sel2.ILIndexUp = sphereToImage;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel2.Visible = false;
                        }

                        if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 2)
                        {
                            AoWUnitWindow.UnitWindow.Sel3.Visible = true;
                            var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(2);
                            var sphereToImage = General.SphereToImage(sphere);
                            AoWUnitWindow.UnitWindow.Sel3.ILIndexUp = sphereToImage;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel3.Visible = false;
                        }

                        if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 3)
                        {
                            AoWUnitWindow.UnitWindow.Sel4.Visible = true;
                            var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(3);
                            var sphereToImage = General.SphereToImage(sphere);
                            AoWUnitWindow.UnitWindow.Sel4.ILIndexUp = sphereToImage;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel4.Visible = false;
                        }

                        if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 4)
                        {
                            AoWUnitWindow.UnitWindow.Sel5.Visible = true;
                            var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(4);
                            var sphereToImage = General.SphereToImage(sphere);
                            AoWUnitWindow.UnitWindow.Sel5.ILIndexUp = sphereToImage;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel5.Visible = false;
                        }

                        if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 5)
                        {
                            AoWUnitWindow.UnitWindow.Sel6.Visible = true;
                            var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(5);
                            var sphereToImage = General.SphereToImage(sphere);
                            AoWUnitWindow.UnitWindow.Sel6.ILIndexUp = sphereToImage;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel6.Visible = false;
                        }

                        if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 6)
                        {
                            AoWUnitWindow.UnitWindow.Sel7.Visible = true;
                            var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(6);
                            var sphereToImage = General.SphereToImage(sphere);
                            AoWUnitWindow.UnitWindow.Sel7.ILIndexUp = sphereToImage;
                        }
                        else
                        {
                            AoWUnitWindow.UnitWindow.Sel7.Visible = false;
                        }

                        var leaderSpheres = TranslateRStr(AoWUnitWindow.LeaderSpheresRStr);
                        AoWUnitWindow.UnitWindow.SphereLbl.Text = leaderSpheres;
                    }

                    if (AoWHSMap.FCombat == null && H.FPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.FCombat != null && AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer)
                    {
                        var items = TranslateRStr(AoWUnitWindow.ItemsRStr);
                        AoWUnitWindow.UnitWindow.UnitTab.Items.Add(items);
                    }

                    if (H.GetCastingPointsMax() > 0)
                    {
                        AoWUnitWindow.UnitWindow.Mana.Visible = true;
                        var castingPoints = H.GetCastingPoints();
                        var castingPointsMax = H.GetCastingPointsMax();
                        AoWUnitWindow.UnitWindow.Mana.Text = IntToStr(castingPoints) + AnsiString.FromBytes(47) + IntToStr(castingPointsMax);
                        AoWUnitWindow.UnitWindow.ManaIcon.Visible = true;
                    }

                    AoWUnitWindow.UnitWindow.ItemUseBtn.Visible = false;
                    AoWUnitWindow.UnitWindow.FGroundOffset = 0;
                    var level = TranslateRStr(AoWUnitWindow.LevelRStr);
                    var hLevel = H.GetLevel();
                    AoWUnitWindow.UnitWindow.Level.Text = level + AnsiString.FromBytes(32) + IntToStr(hLevel);
                    AoWUnitWindow.UnitWindow.T2Leader.Visible = true;
                    AoWUnitWindow.UnitWindow.T2Text.Visible = false;
                    var nickname = H.GetNickname();
                    AoWUnitWindow.UnitWindow.UnitName.Text = H.FHeroName + AnsiString.FromBytes(32) + nickname;
                }

                var exp = TranslateRStr(AoWUnitWindow.ExpRStr);
                var experience = U.GetExperience();
                var nextLevelExperience = U.GetNextLevelExperience();
                AoWUnitWindow.UnitWindow.XP.Text = exp + (AnsiString)" (" + IntToStr(experience) + AnsiString.FromBytes(47) + IntToStr(nextLevelExperience) + AnsiString.FromBytes(41);
                AoWUnitWindow.UnitWindow.UnitWin.Cursor = 0;
                if (AoWUnitWindow.UnitWindow.UnitTab.Items.Count <= AoWUnitWindow.UnitWindow.UnitTab.Selected || AoWUnitWindow.UnitWindow.UnitTab.Selected < 0)
                {
                    AoWUnitWindow.UnitWindow.UnitTab.Selected = 0;
                }

                AoWUnitWindow.UnitWindow.UnitWin.Visible = true;
                if (H != null && (AoWHSMap.FCombat == null && H.FPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.FCombat != null && AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer))
                {
                    if (H.FInventory.GetItem(0) != null)
                    {
                        var inventory = H.FInventory.GetItem(0);
                        AoWUnitWindow.UnitWindow.BP1.Tag = inventory.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP1.Tag = -1;
                    }

                    if (H.FInventory.GetItem(1) != null)
                    {
                        var inventory = H.FInventory.GetItem(1);
                        AoWUnitWindow.UnitWindow.BP2.Tag = inventory.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP2.Tag = -1;
                    }

                    if (H.FInventory.GetItem(2) != null)
                    {
                        var inventory = H.FInventory.GetItem(2);
                        AoWUnitWindow.UnitWindow.BP3.Tag = inventory.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP3.Tag = -1;
                    }

                    if (H.FInventory.GetItem(3) != null)
                    {
                        var inventory = H.FInventory.GetItem(3);
                        AoWUnitWindow.UnitWindow.BP4.Tag = inventory.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP4.Tag = -1;
                    }

                    if (H.FInventory.GetItem(4) != null)
                    {
                        var inventory = H.FInventory.GetItem(4);
                        AoWUnitWindow.UnitWindow.BP5.Tag = inventory.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP5.Tag = -1;
                    }

                    if (H.FInventory.GetItem(5) != null)
                    {
                        var inventory = H.FInventory.GetItem(5);
                        AoWUnitWindow.UnitWindow.BP6.Tag = inventory.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP6.Tag = -1;
                    }

                    if (H.FInventory.GetItem(6) != null)
                    {
                        var inventory = H.FInventory.GetItem(6);
                        AoWUnitWindow.UnitWindow.BP7.Tag = inventory.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP7.Tag = -1;
                    }

                    if (H.FInventory.GetItem(7) != null)
                    {
                        var inventory = H.FInventory.GetItem(7);
                        AoWUnitWindow.UnitWindow.BP8.Tag = inventory.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.BP8.Tag = -1;
                    }

                    if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipAttackSlot) != null)
                    {
                        var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipAttackSlot);
                        AoWUnitWindow.UnitWindow.Attack.Tag = positionItem.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.Attack.Tag = -1;
                    }

                    if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipDefenseSlot) != null)
                    {
                        var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipDefenseSlot);
                        AoWUnitWindow.UnitWindow.Defend.Tag = positionItem.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.Defend.Tag = -1;
                    }

                    if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipHeadSlot) != null)
                    {
                        var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipHeadSlot);
                        AoWUnitWindow.UnitWindow.Head.Tag = positionItem.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.Head.Tag = -1;
                    }

                    if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipTorsoSlot) != null)
                    {
                        var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipTorsoSlot);
                        AoWUnitWindow.UnitWindow.Torso.Tag = positionItem.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.Torso.Tag = -1;
                    }

                    if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipLeftRingSlot) != null)
                    {
                        var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipLeftRingSlot);
                        AoWUnitWindow.UnitWindow.RingL.Tag = positionItem.FItemID;
                    }
                    else
                    {
                        AoWUnitWindow.UnitWindow.RingL.Tag = -1;
                    }

                    if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipRightRingSlot) != null)
                    {
                        var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipRightRingSlot);
                        AoWUnitWindow.UnitWindow.RingR.Tag = positionItem.FItemID;
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
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    AoWUnitWindow.UnitWindow.UHits.Text = IntToStr(hitPoints) + AnsiString.FromBytes(47) + IntToStr(hits);
                    if (AoWHSMap.FCombatMap == null)
                    {
                        var movePoints = U.GetMovePoints();
                        var moves = U.GetMoves();
                        AoWUnitWindow.UnitWindow.UMoves.Text = IntToStr(movePoints) + AnsiString.FromBytes(47) + IntToStr(moves);
                    }
                    else
                    {
                        if (General.FCombatUnit != null)
                        {
                            var moves = General.FCombatUnit.FUnit.GetMoves();
                            AoWUnitWindow.UnitWindow.UMoves.Text = IntToStr(General.FCombatUnit.FUnit.FMovePoints) + AnsiString.FromBytes(47) + IntToStr(moves);
                        }
                    }

                    var attack = U.GetAttack();
                    AoWUnitWindow.UnitWindow.UAttack.Text = IntToStr(attack);
                    var damage = U.GetDamage();
                    AoWUnitWindow.UnitWindow.UDamage.Text = IntToStr(damage);
                    var defense = U.GetDefense();
                    AoWUnitWindow.UnitWindow.UDefense.Text = IntToStr(defense);
                    var resistance = U.GetResistance();
                    AoWUnitWindow.UnitWindow.UResistance.Text = IntToStr(resistance);
                    var moveTypes = U.GetMoveTypes();
                    var mTtoInt = General.MTtoInt(moveTypes);
                    AoWUnitWindow.UnitWindow.UMovesIcon.CurrentImage = mTtoInt;
                    if (U.GetManaUpkeep() > 0)
                    {
                        var manaUpkeep = U.GetManaUpkeep();
                        AoWUnitWindow.UnitWindow.UpKeep.Text = IntToStr(manaUpkeep);
                        UpkeepIcon.CurrentImage = 52;
                    }
                    else
                    {
                        var upkeep = U.GetUpkeep();
                        AoWUnitWindow.UnitWindow.UpKeep.Text = IntToStr(upkeep);
                        UpkeepIcon.CurrentImage = 51;
                    }

                    var alignment = U.GetAlignment();
                    AAlignment.Visible = alignment != TAlignment.alNone;
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

                    var uAlignment = U.GetAlignment();
                    var alignment2 = TranslateRStr(TAlignmentRStr[(int)uAlignment]);
                    AAlignmentLbl.Text = alignment2;
                    AoWUnitWindow.UnitWindow.T1AbilityLB.Items.Clear();
                    AoWUnitWindow.UnitWindow.T1TypeLB.Items.Clear();
                    AoWUnitWindow.UnitWindow.T1SrcLB.Items.Clear();
                    var morale = U.GetMorale();
                    var morale2 = TranslateRStr(TMoraleRStr[(int)morale]);
                    var format = AoWE.AoWE.Format(TranslateRStr(MoraleXRStr), [morale2]);
                    MoraleLbl.Text = format;
                    var unitType = U.GetUnitType();
                    MoraleLbl.Visible = unitType != TUnitType.utMachine;
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
