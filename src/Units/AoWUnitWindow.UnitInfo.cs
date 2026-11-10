#nullable disable
using Aow.Game.Engine;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Game.Combat.AoWTCUnit;
using static Aow.Game.Engine.Nodes.EngineUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Rtl.RtlUnit;
namespace Aow.Game.Ui.Dialogs;

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
                AoWUnitWindowUnit.UnitWindow.PPrev.Visible = true;
                AoWUnitWindowUnit.UnitWindow.PNext.Visible = true;
            }
            else
            {
                AoWUnitWindowUnit.UnitWindow.PPrev.Visible = false;
                AoWUnitWindowUnit.UnitWindow.PNext.Visible = false;
            }

            AoWUnitWindowUnit.UnitWindow.Mana.Visible = false;
            AoWUnitWindowUnit.UnitWindow.ManaIcon.Visible = false;
            AoWUnitWindowUnit.UnitWindow.UnitTab.Items.Clear();
            var abilities = TranslateRStr(AoWUnitWindowUnit.AbilitiesRStr);
            AoWUnitWindowUnit.UnitWindow.UnitTab.Items.Add(abilities);
            var info = TranslateRStr(AoWUnitWindowUnit.InfoRStr);
            AoWUnitWindowUnit.UnitWindow.UnitTab.Items.Add(info);
            if (U.GetRace() > -1)
            {
                var race = U.GetRace();
                var raceNames = TranslateRStr(RaceNamesRStr[race]);
                AoWUnitWindowUnit.UnitWindow.TypeV.Text = raceNames;
                AoWUnitWindowUnit.UnitWindow.RelationIcn.Visible = true;
                var uRace = U.GetRace();
                var playerRelation = AoWHSMap.FRaces.GetRace(uRace).GetPlayerRelation(AoWHSMap.FSeatedPlayer);
                AoWUnitWindowUnit.UnitWindow.RelationIcn.CurrentImage = (int)playerRelation + 11;
                var race2 = U.GetRace();
                var playerRelation2 = AoWHSMap.FRaces.GetRace(race2).GetPlayerRelation(AoWHSMap.FSeatedPlayer);
                var status = TranslateRStr(TStatusRStr[(int)playerRelation2]);
                AoWUnitWindowUnit.UnitWindow.RelationV.Text = status;
            }
            else
            {
                AoWUnitWindowUnit.UnitWindow.TypeV.Text = "";
                AoWUnitWindowUnit.UnitWindow.RelationIcn.Visible = false;
                var none = TranslateRStr(AoWUnitWindowUnit.NoneRStr);
                AoWUnitWindowUnit.UnitWindow.RelationV.Text = none;
            }

            if (AoWHSMap.FSeatedPlayer == U.FPlayer && !(U is TLeader) && AoWHSMap.FCombatMap == null)
            {
                AoWUnitWindowUnit.UnitWindow.DisbandBtn.Visible = true;
                AoWUnitWindowUnit.UnitWindow.BCloseBtn.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            }
            else
            {
                AoWUnitWindowUnit.UnitWindow.DisbandBtn.Visible = false;
                AoWUnitWindowUnit.UnitWindow.BCloseBtn.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            }

            AoWUnitWindowUnit.UnitWindow.BCloseBtn.FAligned = false;
            switch (U.GetUnitType())
            {
                case TUnitType.utCreature:
                    var creature = TranslateRStr(AoWUnitWindowUnit.CreatureRStr);
                    AoWUnitWindowUnit.UnitWindow.TypeV.Text = AoWUnitWindowUnit.UnitWindow.TypeV.Text + " " + creature;
                    AoWUnitWindowUnit.UnitWindow.TypeIcn.CurrentImage = 21;
                    break;
                case TUnitType.utMachine:
                    var machine = TranslateRStr(AoWUnitWindowUnit.MachineRStr);
                    AoWUnitWindowUnit.UnitWindow.TypeV.Text = AoWUnitWindowUnit.UnitWindow.TypeV.Text + " " + machine;
                    AoWUnitWindowUnit.UnitWindow.TypeIcn.CurrentImage = 22;
                    break;
                default:
                    if (H == null)
                    {
                        var unit = TranslateRStr(AoWUnitWindowUnit.UnitRStr);
                        AoWUnitWindowUnit.UnitWindow.TypeV.Text = AoWUnitWindowUnit.UnitWindow.TypeV.Text + " " + unit;
                    }
                    else
                    {
                        if (H is TLeader)
                        {
                            var leader = TranslateRStr(AoWUnitWindowUnit.LeaderRStr);
                            AoWUnitWindowUnit.UnitWindow.TypeV.Text = AoWUnitWindowUnit.UnitWindow.TypeV.Text + " " + leader;
                        }
                        else
                        {
                            var hero = TranslateRStr(AoWUnitWindowUnit.HeroRStr);
                            AoWUnitWindowUnit.UnitWindow.TypeV.Text = AoWUnitWindowUnit.UnitWindow.TypeV.Text + " " + hero;
                        }
                    }

                    AoWUnitWindowUnit.UnitWindow.TypeIcn.CurrentImage = 23;
                    break;
            }

            if (H == null)
            {
                var level = TranslateRStr(AoWUnitWindowUnit.LevelRStr);
                var unitLevel = U.GetUnitLevel();
                AoWUnitWindowUnit.UnitWindow.Level.Text = level + " " + IntToStr(unitLevel);
                AoWUnitWindowUnit.UnitWindow.T2Leader.Visible = false;
                AoWUnitWindowUnit.UnitWindow.T2Text.Visible = true;
                var description = U.GetDescription();
                AoWUnitWindowUnit.UnitWindow.T2Memo.Items.Assign(description);
                AoWUnitWindowUnit.UnitWindow.T2SBar.Position = 0;
                var name = U.GetName();
                AoWUnitWindowUnit.UnitWindow.UnitName.Text = name;
            }
            else
            {
                if (H.FPlayer == 0)
                {
                    AoWUnitWindowUnit.UnitWindow.SelPnl.Visible = false;
                    var noLeader = TranslateRStr(AoWUnitWindowUnit.NoLeaderRStr);
                    AoWUnitWindowUnit.UnitWindow.SphereLbl.Text = noLeader;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.SelPnl.Visible = true;
                    if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 0)
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel1.Visible = true;
                        var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(0);
                        var sphereToImage = General.SphereToImage(sphere);
                        AoWUnitWindowUnit.UnitWindow.Sel1.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel1.Visible = false;
                    }

                    if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 1)
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel2.Visible = true;
                        var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(1);
                        var sphereToImage = General.SphereToImage(sphere);
                        AoWUnitWindowUnit.UnitWindow.Sel2.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel2.Visible = false;
                    }

                    if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 2)
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel3.Visible = true;
                        var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(2);
                        var sphereToImage = General.SphereToImage(sphere);
                        AoWUnitWindowUnit.UnitWindow.Sel3.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel3.Visible = false;
                    }

                    if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 3)
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel4.Visible = true;
                        var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(3);
                        var sphereToImage = General.SphereToImage(sphere);
                        AoWUnitWindowUnit.UnitWindow.Sel4.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel4.Visible = false;
                    }

                    if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 4)
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel5.Visible = true;
                        var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(4);
                        var sphereToImage = General.SphereToImage(sphere);
                        AoWUnitWindowUnit.UnitWindow.Sel5.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel5.Visible = false;
                    }

                    if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 5)
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel6.Visible = true;
                        var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(5);
                        var sphereToImage = General.SphereToImage(sphere);
                        AoWUnitWindowUnit.UnitWindow.Sel6.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel6.Visible = false;
                    }

                    if (AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 6)
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel7.Visible = true;
                        var sphere = AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(6);
                        var sphereToImage = General.SphereToImage(sphere);
                        AoWUnitWindowUnit.UnitWindow.Sel7.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        AoWUnitWindowUnit.UnitWindow.Sel7.Visible = false;
                    }

                    var leaderSpheres = TranslateRStr(AoWUnitWindowUnit.LeaderSpheresRStr);
                    AoWUnitWindowUnit.UnitWindow.SphereLbl.Text = leaderSpheres;
                }

                if (AoWHSMap.FCombat == null && H.FPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.FCombat != null && AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer)
                {
                    var items = TranslateRStr(AoWUnitWindowUnit.ItemsRStr);
                    AoWUnitWindowUnit.UnitWindow.UnitTab.Items.Add(items);
                }

                if (H.GetCastingPointsMax() > 0)
                {
                    AoWUnitWindowUnit.UnitWindow.Mana.Visible = true;
                    var castingPoints = H.GetCastingPoints();
                    var castingPointsMax = H.GetCastingPointsMax();
                    AoWUnitWindowUnit.UnitWindow.Mana.Text = IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
                    AoWUnitWindowUnit.UnitWindow.ManaIcon.Visible = true;
                }

                AoWUnitWindowUnit.UnitWindow.ItemUseBtn.Visible = false;
                AoWUnitWindowUnit.UnitWindow.FGroundOffset = 0;
                var level = TranslateRStr(AoWUnitWindowUnit.LevelRStr);
                var hLevel = H.GetLevel();
                AoWUnitWindowUnit.UnitWindow.Level.Text = level + " " + IntToStr(hLevel);
                AoWUnitWindowUnit.UnitWindow.T2Leader.Visible = true;
                AoWUnitWindowUnit.UnitWindow.T2Text.Visible = false;
                var nickname = H.GetNickname();
                AoWUnitWindowUnit.UnitWindow.UnitName.Text = H.FHeroName + " " + nickname;
            }

            var exp = TranslateRStr(AoWUnitWindowUnit.ExpRStr);
            var experience = U.GetExperience();
            var nextLevelExperience = U.GetNextLevelExperience();
            AoWUnitWindowUnit.UnitWindow.XP.Text = exp + " (" + IntToStr(experience) + "/" + IntToStr(nextLevelExperience) + ")";
            AoWUnitWindowUnit.UnitWindow.UnitWin.Cursor = 0;
            if (AoWUnitWindowUnit.UnitWindow.UnitTab.Items.Count <= AoWUnitWindowUnit.UnitWindow.UnitTab.Selected || AoWUnitWindowUnit.UnitWindow.UnitTab.Selected < 0)
            {
                AoWUnitWindowUnit.UnitWindow.UnitTab.Selected = 0;
            }

            AoWUnitWindowUnit.UnitWindow.UnitWin.Visible = true;
            if (H != null && (AoWHSMap.FCombat == null && H.FPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.FCombat != null && AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer))
            {
                if (H.FInventory.GetItem(0) != null)
                {
                    var inventory = H.FInventory.GetItem(0);
                    AoWUnitWindowUnit.UnitWindow.BP1.Tag = inventory.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.BP1.Tag = -1;
                }

                if (H.FInventory.GetItem(1) != null)
                {
                    var inventory = H.FInventory.GetItem(1);
                    AoWUnitWindowUnit.UnitWindow.BP2.Tag = inventory.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.BP2.Tag = -1;
                }

                if (H.FInventory.GetItem(2) != null)
                {
                    var inventory = H.FInventory.GetItem(2);
                    AoWUnitWindowUnit.UnitWindow.BP3.Tag = inventory.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.BP3.Tag = -1;
                }

                if (H.FInventory.GetItem(3) != null)
                {
                    var inventory = H.FInventory.GetItem(3);
                    AoWUnitWindowUnit.UnitWindow.BP4.Tag = inventory.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.BP4.Tag = -1;
                }

                if (H.FInventory.GetItem(4) != null)
                {
                    var inventory = H.FInventory.GetItem(4);
                    AoWUnitWindowUnit.UnitWindow.BP5.Tag = inventory.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.BP5.Tag = -1;
                }

                if (H.FInventory.GetItem(5) != null)
                {
                    var inventory = H.FInventory.GetItem(5);
                    AoWUnitWindowUnit.UnitWindow.BP6.Tag = inventory.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.BP6.Tag = -1;
                }

                if (H.FInventory.GetItem(6) != null)
                {
                    var inventory = H.FInventory.GetItem(6);
                    AoWUnitWindowUnit.UnitWindow.BP7.Tag = inventory.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.BP7.Tag = -1;
                }

                if (H.FInventory.GetItem(7) != null)
                {
                    var inventory = H.FInventory.GetItem(7);
                    AoWUnitWindowUnit.UnitWindow.BP8.Tag = inventory.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.BP8.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipAttackSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipAttackSlot);
                    AoWUnitWindowUnit.UnitWindow.Attack.Tag = positionItem.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.Attack.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipDefenseSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipDefenseSlot);
                    AoWUnitWindowUnit.UnitWindow.Defend.Tag = positionItem.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.Defend.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipHeadSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipHeadSlot);
                    AoWUnitWindowUnit.UnitWindow.Head.Tag = positionItem.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.Head.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipTorsoSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipTorsoSlot);
                    AoWUnitWindowUnit.UnitWindow.Torso.Tag = positionItem.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.Torso.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipLeftRingSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipLeftRingSlot);
                    AoWUnitWindowUnit.UnitWindow.RingL.Tag = positionItem.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.RingL.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipRightRingSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipRightRingSlot);
                    AoWUnitWindowUnit.UnitWindow.RingR.Tag = positionItem.FItemID;
                }
                else
                {
                    AoWUnitWindowUnit.UnitWindow.RingR.Tag = -1;
                }

                AoWUnitWindowUnit.UnitWindow.UpdateGround(0);
                AoWUnitWindowUnit.SelectedItem = null;
                AoWUnitWindowUnit.UnitWindow.BP1.Checked = false;
                AoWUnitWindowUnit.UnitWindow.BP2.Checked = false;
                AoWUnitWindowUnit.UnitWindow.BP3.Checked = false;
                AoWUnitWindowUnit.UnitWindow.BP4.Checked = false;
                AoWUnitWindowUnit.UnitWindow.BP5.Checked = false;
                AoWUnitWindowUnit.UnitWindow.BP6.Checked = false;
                AoWUnitWindowUnit.UnitWindow.BP7.Checked = false;
                AoWUnitWindowUnit.UnitWindow.BP8.Checked = false;
                AoWUnitWindowUnit.UnitWindow.Attack.Checked = false;
                AoWUnitWindowUnit.UnitWindow.Defend.Checked = false;
                AoWUnitWindowUnit.UnitWindow.Head.Checked = false;
                AoWUnitWindowUnit.UnitWindow.Torso.Checked = false;
                AoWUnitWindowUnit.UnitWindow.RingL.Checked = false;
                AoWUnitWindowUnit.UnitWindow.RingR.Checked = false;
                AoWUnitWindowUnit.UnitWindow.ItemUseBtn.Visible = false;
            }

            if (U != null)
            {
                var hitPoints = U.GetHitPoints();
                var hits = U.GetHits();
                AoWUnitWindowUnit.UnitWindow.UHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                if (AoWHSMap.FCombatMap == null)
                {
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    AoWUnitWindowUnit.UnitWindow.UMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                }
                else
                {
                    if (General.FCombatUnit != null)
                    {
                        var moves = General.FCombatUnit.FUnit.GetMoves();
                        AoWUnitWindowUnit.UnitWindow.UMoves.Text = IntToStr(General.FCombatUnit.FUnit.FMovePoints) + "/" + IntToStr(moves);
                    }
                }

                var attack = U.GetAttack();
                AoWUnitWindowUnit.UnitWindow.UAttack.Text = IntToStr(attack);
                var damage = U.GetDamage();
                AoWUnitWindowUnit.UnitWindow.UDamage.Text = IntToStr(damage);
                var defense = U.GetDefense();
                AoWUnitWindowUnit.UnitWindow.UDefense.Text = IntToStr(defense);
                var resistance = U.GetResistance();
                AoWUnitWindowUnit.UnitWindow.UResistance.Text = IntToStr(resistance);
                var moveTypes = U.GetMoveTypes();
                var mTtoInt = General.MTtoInt(moveTypes);
                AoWUnitWindowUnit.UnitWindow.UMovesIcon.CurrentImage = mTtoInt;
                if (U.GetManaUpkeep() > 0)
                {
                    var manaUpkeep = U.GetManaUpkeep();
                    AoWUnitWindowUnit.UnitWindow.UpKeep.Text = IntToStr(manaUpkeep);
                    UpkeepIcon.CurrentImage = 52;
                }
                else
                {
                    var upkeep = U.GetUpkeep();
                    AoWUnitWindowUnit.UnitWindow.UpKeep.Text = IntToStr(upkeep);
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
                var alignmentText = TranslateRStr(TAlignmentRStr[(int)uAlignment]);
                AAlignmentLbl.Text = alignmentText;
                AoWUnitWindowUnit.UnitWindow.T1AbilityLB.Items.Clear();
                AoWUnitWindowUnit.UnitWindow.T1TypeLB.Items.Clear();
                AoWUnitWindowUnit.UnitWindow.T1SrcLB.Items.Clear();
                var morale = U.GetMorale();
                var moraleText = TranslateRStr(TMoraleRStr[(int)morale]);
                var format = AoWEUnit.Format(TranslateRStr(MoraleXRStr), [moraleText]);
                MoraleLbl.Text = format;
                var unitType = U.GetUnitType();
                MoraleLbl.Visible = unitType != TUnitType.utMachine;
                if (AoWHSMap.FCombatMap == null)
                {
                    U.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                    SortStringsEx(T1AbilityLB.Items, AoWUnitWindowUnit.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                }
                else
                {
                    if (General.FCombatUnit != null && General.FCombatUnit.FUnit != null && General.FCombatUnit.FUnit.FUnit != null)
                    {
                        General.FCombatUnit.FUnit.FUnit.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                        SortStringsEx(T1AbilityLB.Items, AoWUnitWindowUnit.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                    }
                }

                AoWUnitWindowUnit.UnitWindow.T1AbilityLB.Selected = -1;
                AoWUnitWindowUnit.UnitWindow.T1AbilityLBChange(T1AbilityLB);
                UnitWin.FDrawn = false;
            }
        }
    }
}
