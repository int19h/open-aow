#nullable disable
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Engine;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
namespace Aow.Game.Ui.Dialogs;

public partial class UnitWindow
{
    public void ShowUnit()
    {
        Hero H = default;
        AbstractUnit U = default;
        if (DialogHosts.General.FSelectedUnit != null)
        {
            DialogHosts.General.BuildAbilities();
            U = DialogHosts.General.FSelectedUnit;
            if (U != null && AoWHSMap.Current.FSeatedPlayer == U.FPlayer)
            {
                AoWHSMap.Current.FTutorialControl.ShowMessage(64, true, true);
            }

            if (U is Hero)
            {
                H = (Hero)U;
            }
            else
            {
                H = null;
            }

            if (AoWCombatMap.Current == null && DialogHosts.General.FUnitList != null && DialogHosts.General.IndexOfUnit(U) > -1 && DialogHosts.General.FSelectedUnits[1] > -1)
            {
                DialogHosts.UnitWindow.PPrev.Visible = true;
                DialogHosts.UnitWindow.PNext.Visible = true;
            }
            else
            {
                DialogHosts.UnitWindow.PPrev.Visible = false;
                DialogHosts.UnitWindow.PNext.Visible = false;
            }

            DialogHosts.UnitWindow.Mana.Visible = false;
            DialogHosts.UnitWindow.ManaIcon.Visible = false;
            DialogHosts.UnitWindow.UnitTab.Items.Clear();
            var abilities = AbilityRules.TranslateRStr(AbilitiesRStr);
            DialogHosts.UnitWindow.UnitTab.Items.Add(abilities);
            var info = AbilityRules.TranslateRStr(InfoRStr);
            DialogHosts.UnitWindow.UnitTab.Items.Add(info);
            if (U.GetRace() > -1)
            {
                var race = U.GetRace();
                var raceNames = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[race]);
                DialogHosts.UnitWindow.TypeV.Text = raceNames;
                DialogHosts.UnitWindow.RelationIcn.Visible = true;
                var uRace = U.GetRace();
                var playerRelation = AoWHSMap.Current.FRaces.GetRace(uRace).GetPlayerRelation(AoWHSMap.Current.FSeatedPlayer);
                DialogHosts.UnitWindow.RelationIcn.CurrentImage = (int)playerRelation + 11;
                var race2 = U.GetRace();
                var playerRelation2 = AoWHSMap.Current.FRaces.GetRace(race2).GetPlayerRelation(AoWHSMap.Current.FSeatedPlayer);
                var status = AbilityRules.TranslateRStr(GameTables.TStatusRStr[(int)playerRelation2]);
                DialogHosts.UnitWindow.RelationV.Text = status;
            }
            else
            {
                DialogHosts.UnitWindow.TypeV.Text = "";
                DialogHosts.UnitWindow.RelationIcn.Visible = false;
                var none = AbilityRules.TranslateRStr(NoneRStr);
                DialogHosts.UnitWindow.RelationV.Text = none;
            }

            if (AoWHSMap.Current.FSeatedPlayer == U.FPlayer && !(U is Leader) && AoWHSMap.Current.FCombatMap == null)
            {
                DialogHosts.UnitWindow.DisbandBtn.Visible = true;
                DialogHosts.UnitWindow.BCloseBtn.Alignment.AlignWidth = AOWAlignWidth.Left;
            }
            else
            {
                DialogHosts.UnitWindow.DisbandBtn.Visible = false;
                DialogHosts.UnitWindow.BCloseBtn.Alignment.AlignWidth = AOWAlignWidth.Center;
            }

            DialogHosts.UnitWindow.BCloseBtn.FAligned = false;
            switch (U.GetUnitType())
            {
                case UnitType.Creature:
                    var creature = AbilityRules.TranslateRStr(CreatureRStr);
                    DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + creature;
                    DialogHosts.UnitWindow.TypeIcn.CurrentImage = 21;
                    break;
                case UnitType.Machine:
                    var machine = AbilityRules.TranslateRStr(MachineRStr);
                    DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + machine;
                    DialogHosts.UnitWindow.TypeIcn.CurrentImage = 22;
                    break;
                default:
                    if (H == null)
                    {
                        var unit = AbilityRules.TranslateRStr(UnitRStr);
                        DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + unit;
                    }
                    else
                    {
                        if (H is Leader)
                        {
                            var leader = AbilityRules.TranslateRStr(LeaderRStr);
                            DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + leader;
                        }
                        else
                        {
                            var hero = AbilityRules.TranslateRStr(HeroRStr);
                            DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + hero;
                        }
                    }

                    DialogHosts.UnitWindow.TypeIcn.CurrentImage = 23;
                    break;
            }

            if (H == null)
            {
                var level = AbilityRules.TranslateRStr(LevelRStr);
                var unitLevel = U.GetUnitLevel();
                DialogHosts.UnitWindow.Level.Text = level + " " + IntToStr(unitLevel);
                DialogHosts.UnitWindow.T2Leader.Visible = false;
                DialogHosts.UnitWindow.T2Text.Visible = true;
                var description = U.GetDescription();
                DialogHosts.UnitWindow.T2Memo.Items.Assign(description);
                DialogHosts.UnitWindow.T2SBar.Position = 0;
                var name = U.GetName();
                DialogHosts.UnitWindow.UnitName.Text = name;
            }
            else
            {
                if (H.FPlayer == 0)
                {
                    DialogHosts.UnitWindow.SelPnl.Visible = false;
                    var noLeader = AbilityRules.TranslateRStr(NoLeaderRStr);
                    DialogHosts.UnitWindow.SphereLbl.Text = noLeader;
                }
                else
                {
                    DialogHosts.UnitWindow.SelPnl.Visible = true;
                    if (AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 0)
                    {
                        DialogHosts.UnitWindow.Sel1.Visible = true;
                        var sphere = AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(0);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel1.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel1.Visible = false;
                    }

                    if (AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 1)
                    {
                        DialogHosts.UnitWindow.Sel2.Visible = true;
                        var sphere = AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(1);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel2.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel2.Visible = false;
                    }

                    if (AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 2)
                    {
                        DialogHosts.UnitWindow.Sel3.Visible = true;
                        var sphere = AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(2);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel3.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel3.Visible = false;
                    }

                    if (AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 3)
                    {
                        DialogHosts.UnitWindow.Sel4.Visible = true;
                        var sphere = AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(3);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel4.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel4.Visible = false;
                    }

                    if (AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 4)
                    {
                        DialogHosts.UnitWindow.Sel5.Visible = true;
                        var sphere = AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(4);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel5.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel5.Visible = false;
                    }

                    if (AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 5)
                    {
                        DialogHosts.UnitWindow.Sel6.Visible = true;
                        var sphere = AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(5);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel6.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel6.Visible = false;
                    }

                    if (AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 6)
                    {
                        DialogHosts.UnitWindow.Sel7.Visible = true;
                        var sphere = AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(6);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel7.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel7.Visible = false;
                    }

                    var leaderSpheres = AbilityRules.TranslateRStr(LeaderSpheresRStr);
                    DialogHosts.UnitWindow.SphereLbl.Text = leaderSpheres;
                }

                if (AoWHSMap.Current.FCombat == null && H.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.FCombat != null && AoWHSMap.Current.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.Current.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer)
                {
                    var items = AbilityRules.TranslateRStr(ItemsRStr);
                    DialogHosts.UnitWindow.UnitTab.Items.Add(items);
                }

                if (H.GetCastingPointsMax() > 0)
                {
                    DialogHosts.UnitWindow.Mana.Visible = true;
                    var castingPoints = H.GetCastingPoints();
                    var castingPointsMax = H.GetCastingPointsMax();
                    DialogHosts.UnitWindow.Mana.Text = IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
                    DialogHosts.UnitWindow.ManaIcon.Visible = true;
                }

                DialogHosts.UnitWindow.ItemUseBtn.Visible = false;
                DialogHosts.UnitWindow.FGroundOffset = 0;
                var level = AbilityRules.TranslateRStr(LevelRStr);
                var hLevel = H.GetLevel();
                DialogHosts.UnitWindow.Level.Text = level + " " + IntToStr(hLevel);
                DialogHosts.UnitWindow.T2Leader.Visible = true;
                DialogHosts.UnitWindow.T2Text.Visible = false;
                var nickname = H.GetNickname();
                DialogHosts.UnitWindow.UnitName.Text = H.FHeroName + " " + nickname;
            }

            var exp = AbilityRules.TranslateRStr(ExpRStr);
            var experience = U.GetExperience();
            var nextLevelExperience = U.GetNextLevelExperience();
            DialogHosts.UnitWindow.XP.Text = exp + " (" + IntToStr(experience) + "/" + IntToStr(nextLevelExperience) + ")";
            DialogHosts.UnitWindow.UnitWin.Cursor = 0;
            if (DialogHosts.UnitWindow.UnitTab.Items.Count <= DialogHosts.UnitWindow.UnitTab.Selected || DialogHosts.UnitWindow.UnitTab.Selected < 0)
            {
                DialogHosts.UnitWindow.UnitTab.Selected = 0;
            }

            DialogHosts.UnitWindow.UnitWin.Visible = true;
            if (H != null && (AoWHSMap.Current.FCombat == null && H.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.FCombat != null && AoWHSMap.Current.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.Current.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer))
            {
                if (H.FInventory.GetItem(0) != null)
                {
                    var inventory = H.FInventory.GetItem(0);
                    DialogHosts.UnitWindow.BP1.Tag = inventory.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.BP1.Tag = -1;
                }

                if (H.FInventory.GetItem(1) != null)
                {
                    var inventory = H.FInventory.GetItem(1);
                    DialogHosts.UnitWindow.BP2.Tag = inventory.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.BP2.Tag = -1;
                }

                if (H.FInventory.GetItem(2) != null)
                {
                    var inventory = H.FInventory.GetItem(2);
                    DialogHosts.UnitWindow.BP3.Tag = inventory.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.BP3.Tag = -1;
                }

                if (H.FInventory.GetItem(3) != null)
                {
                    var inventory = H.FInventory.GetItem(3);
                    DialogHosts.UnitWindow.BP4.Tag = inventory.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.BP4.Tag = -1;
                }

                if (H.FInventory.GetItem(4) != null)
                {
                    var inventory = H.FInventory.GetItem(4);
                    DialogHosts.UnitWindow.BP5.Tag = inventory.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.BP5.Tag = -1;
                }

                if (H.FInventory.GetItem(5) != null)
                {
                    var inventory = H.FInventory.GetItem(5);
                    DialogHosts.UnitWindow.BP6.Tag = inventory.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.BP6.Tag = -1;
                }

                if (H.FInventory.GetItem(6) != null)
                {
                    var inventory = H.FInventory.GetItem(6);
                    DialogHosts.UnitWindow.BP7.Tag = inventory.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.BP7.Tag = -1;
                }

                if (H.FInventory.GetItem(7) != null)
                {
                    var inventory = H.FInventory.GetItem(7);
                    DialogHosts.UnitWindow.BP8.Tag = inventory.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.BP8.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.AttackSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.AttackSlot);
                    DialogHosts.UnitWindow.Attack.Tag = positionItem.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.Attack.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.DefenseSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.DefenseSlot);
                    DialogHosts.UnitWindow.Defend.Tag = positionItem.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.Defend.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.HeadSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.HeadSlot);
                    DialogHosts.UnitWindow.Head.Tag = positionItem.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.Head.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.TorsoSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.TorsoSlot);
                    DialogHosts.UnitWindow.Torso.Tag = positionItem.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.Torso.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot);
                    DialogHosts.UnitWindow.RingL.Tag = positionItem.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.RingL.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.RightRingSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.RightRingSlot);
                    DialogHosts.UnitWindow.RingR.Tag = positionItem.FItemID;
                }
                else
                {
                    DialogHosts.UnitWindow.RingR.Tag = -1;
                }

                DialogHosts.UnitWindow.UpdateGround(0);
                SelectedItem = null;
                DialogHosts.UnitWindow.BP1.Checked = false;
                DialogHosts.UnitWindow.BP2.Checked = false;
                DialogHosts.UnitWindow.BP3.Checked = false;
                DialogHosts.UnitWindow.BP4.Checked = false;
                DialogHosts.UnitWindow.BP5.Checked = false;
                DialogHosts.UnitWindow.BP6.Checked = false;
                DialogHosts.UnitWindow.BP7.Checked = false;
                DialogHosts.UnitWindow.BP8.Checked = false;
                DialogHosts.UnitWindow.Attack.Checked = false;
                DialogHosts.UnitWindow.Defend.Checked = false;
                DialogHosts.UnitWindow.Head.Checked = false;
                DialogHosts.UnitWindow.Torso.Checked = false;
                DialogHosts.UnitWindow.RingL.Checked = false;
                DialogHosts.UnitWindow.RingR.Checked = false;
                DialogHosts.UnitWindow.ItemUseBtn.Visible = false;
            }

            if (U != null)
            {
                var hitPoints = U.GetHitPoints();
                var hits = U.GetHits();
                DialogHosts.UnitWindow.UHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                if (AoWHSMap.Current.FCombatMap == null)
                {
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    DialogHosts.UnitWindow.UMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                }
                else
                {
                    if (DialogHosts.General.FCombatUnit != null)
                    {
                        var moves = DialogHosts.General.FCombatUnit.FUnit.GetMoves();
                        DialogHosts.UnitWindow.UMoves.Text = IntToStr(DialogHosts.General.FCombatUnit.FUnit.FMovePoints) + "/" + IntToStr(moves);
                    }
                }

                var attack = U.GetAttack();
                DialogHosts.UnitWindow.UAttack.Text = IntToStr(attack);
                var damage = U.GetDamage();
                DialogHosts.UnitWindow.UDamage.Text = IntToStr(damage);
                var defense = U.GetDefense();
                DialogHosts.UnitWindow.UDefense.Text = IntToStr(defense);
                var resistance = U.GetResistance();
                DialogHosts.UnitWindow.UResistance.Text = IntToStr(resistance);
                var moveTypes = U.GetMoveTypes();
                var mTtoInt = DialogHosts.General.MTtoInt(moveTypes);
                DialogHosts.UnitWindow.UMovesIcon.CurrentImage = mTtoInt;
                if (U.GetManaUpkeep() > 0)
                {
                    var manaUpkeep = U.GetManaUpkeep();
                    DialogHosts.UnitWindow.UpKeep.Text = IntToStr(manaUpkeep);
                    UpkeepIcon.CurrentImage = 52;
                }
                else
                {
                    var upkeep = U.GetUpkeep();
                    DialogHosts.UnitWindow.UpKeep.Text = IntToStr(upkeep);
                    UpkeepIcon.CurrentImage = 51;
                }

                var alignment = U.GetAlignment();
                AAlignment.Visible = alignment != Alignment.None;
                switch (U.GetAlignment())
                {
                    case Alignment.PureGood:
                        AAlignment.CurrentImage = 31;
                        break;
                    case Alignment.Good:
                        AAlignment.CurrentImage = 32;
                        break;
                    case Alignment.PureNeutral:
                    case Alignment.Neutral:
                        AAlignment.CurrentImage = 33;
                        break;
                    case Alignment.Evil:
                        AAlignment.CurrentImage = 34;
                        break;
                    case Alignment.PureEvil:
                        AAlignment.CurrentImage = 35;
                        break;
                }

                var uAlignment = U.GetAlignment();
                var alignmentText = AbilityRules.TranslateRStr(GameText.TAlignmentRStr[(int)uAlignment]);
                AAlignmentLbl.Text = alignmentText;
                DialogHosts.UnitWindow.T1AbilityLB.Items.Clear();
                DialogHosts.UnitWindow.T1TypeLB.Items.Clear();
                DialogHosts.UnitWindow.T1SrcLB.Items.Clear();
                var morale = U.GetMorale();
                var moraleText = AbilityRules.TranslateRStr(TMoraleRStr[(int)morale]);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(MoraleXRStr), [moraleText]);
                MoraleLbl.Text = format;
                var unitType = U.GetUnitType();
                MoraleLbl.Visible = unitType != UnitType.Machine;
                if (AoWHSMap.Current.FCombatMap == null)
                {
                    U.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                    StringSorting.SortStringsEx(T1AbilityLB.Items, CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                }
                else
                {
                    if (DialogHosts.General.FCombatUnit != null && DialogHosts.General.FCombatUnit.FUnit != null && DialogHosts.General.FCombatUnit.FUnit.FUnit != null)
                    {
                        DialogHosts.General.FCombatUnit.FUnit.FUnit.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                        StringSorting.SortStringsEx(T1AbilityLB.Items, CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                    }
                }

                DialogHosts.UnitWindow.T1AbilityLB.Selected = -1;
                DialogHosts.UnitWindow.T1AbilityLBChange(T1AbilityLB);
                UnitWin.FDrawn = false;
            }
        }
    }
}
