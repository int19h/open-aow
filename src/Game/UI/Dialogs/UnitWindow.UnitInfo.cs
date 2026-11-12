#nullable disable
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.Units;
using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class UnitWindow
{
    public void ShowUnit()
    {
        Hero H = default;
        Troop U = default;
        if (DialogHosts.General.SelectedUnit != null)
        {
            DialogHosts.General.BuildAbilities();
            U = DialogHosts.General.SelectedUnit;
            if (U != null && AoWHSMap.Current.SeatedPlayer == U.Player)
            {
                AoWHSMap.Current.TutorialControl.ShowMessage(64, true, true);
            }

            if (U is Hero)
            {
                H = (Hero)U;
            }
            else
            {
                H = null;
            }

            if (CombatMap.Current == null && DialogHosts.General.UnitList != null && DialogHosts.General.IndexOfUnit(U) > -1 && DialogHosts.General.SelectedUnits[1] > -1)
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
            var abilities = AbilityRules.TranslateResourceString(s_abilitiesText);
            DialogHosts.UnitWindow.UnitTab.Items.Add(abilities);
            var info = AbilityRules.TranslateResourceString(s_infoText);
            DialogHosts.UnitWindow.UnitTab.Items.Add(info);
            if (U.GetRace() > -1)
            {
                var race = U.GetRace();
                var raceNames = AbilityRules.TranslateResourceString(GameTables.s_raceNames[race]);
                DialogHosts.UnitWindow.TypeV.Text = raceNames;
                DialogHosts.UnitWindow.RelationIcn.Visible = true;
                var uRace = U.GetRace();
                var playerRelation = AoWHSMap.Current.Races.GetRace(uRace).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer);
                DialogHosts.UnitWindow.RelationIcn.CurrentImage = (int)playerRelation + 11;
                var race2 = U.GetRace();
                var playerRelation2 = AoWHSMap.Current.Races.GetRace(race2).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer);
                var status = AbilityRules.TranslateResourceString(GameTables.s_statusTexts[(int)playerRelation2]);
                DialogHosts.UnitWindow.RelationV.Text = status;
            }
            else
            {
                DialogHosts.UnitWindow.TypeV.Text = "";
                DialogHosts.UnitWindow.RelationIcn.Visible = false;
                var none = AbilityRules.TranslateResourceString(s_noneText);
                DialogHosts.UnitWindow.RelationV.Text = none;
            }

            if (AoWHSMap.Current.SeatedPlayer == U.Player && !(U is Leader) && AoWHSMap.Current.CombatMap == null)
            {
                DialogHosts.UnitWindow.DisbandBtn.Visible = true;
                DialogHosts.UnitWindow.BCloseBtn.Alignment.AlignWidth = AlignWidth.Left;
            }
            else
            {
                DialogHosts.UnitWindow.DisbandBtn.Visible = false;
                DialogHosts.UnitWindow.BCloseBtn.Alignment.AlignWidth = AlignWidth.Center;
            }

            DialogHosts.UnitWindow.BCloseBtn.Aligned = false;
            switch (U.GetUnitType())
            {
                case UnitType.Creature:
                    var creature = AbilityRules.TranslateResourceString(s_creatureText);
                    DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + creature;
                    DialogHosts.UnitWindow.TypeIcn.CurrentImage = 21;
                    break;
                case UnitType.Machine:
                    var machine = AbilityRules.TranslateResourceString(s_machineText);
                    DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + machine;
                    DialogHosts.UnitWindow.TypeIcn.CurrentImage = 22;
                    break;
                default:
                    if (H == null)
                    {
                        var unit = AbilityRules.TranslateResourceString(s_unitText);
                        DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + unit;
                    }
                    else
                    {
                        if (H is Leader)
                        {
                            var leader = AbilityRules.TranslateResourceString(s_leaderText);
                            DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + leader;
                        }
                        else
                        {
                            var hero = AbilityRules.TranslateResourceString(s_heroText);
                            DialogHosts.UnitWindow.TypeV.Text = DialogHosts.UnitWindow.TypeV.Text + " " + hero;
                        }
                    }

                    DialogHosts.UnitWindow.TypeIcn.CurrentImage = 23;
                    break;
            }

            if (H == null)
            {
                var level = AbilityRules.TranslateResourceString(s_levelText);
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
                if (H.Player == 0)
                {
                    DialogHosts.UnitWindow.SelPnl.Visible = false;
                    var noLeader = AbilityRules.TranslateResourceString(s_noLeaderText);
                    DialogHosts.UnitWindow.SphereLbl.Text = noLeader;
                }
                else
                {
                    DialogHosts.UnitWindow.SelPnl.Visible = true;
                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 0)
                    {
                        DialogHosts.UnitWindow.Sel1.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(0);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel1.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel1.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 1)
                    {
                        DialogHosts.UnitWindow.Sel2.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(1);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel2.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel2.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 2)
                    {
                        DialogHosts.UnitWindow.Sel3.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(2);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel3.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel3.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 3)
                    {
                        DialogHosts.UnitWindow.Sel4.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(3);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel4.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel4.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 4)
                    {
                        DialogHosts.UnitWindow.Sel5.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(4);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel5.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel5.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 5)
                    {
                        DialogHosts.UnitWindow.Sel6.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(5);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel6.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel6.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 6)
                    {
                        DialogHosts.UnitWindow.Sel7.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(6);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sel7.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sel7.Visible = false;
                    }

                    var leaderSpheres = AbilityRules.TranslateResourceString(s_leaderSpheresText);
                    DialogHosts.UnitWindow.SphereLbl.Text = leaderSpheres;
                }

                if (AoWHSMap.Current.Combat == null && H.Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Combat != null && AoWHSMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)AoWHSMap.Current.Combat.CombatData.CurrentPlayer).Player == H.Player)
                {
                    var items = AbilityRules.TranslateResourceString(s_itemsText);
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
                DialogHosts.UnitWindow.GroundOffset = 0;
                var level = AbilityRules.TranslateResourceString(s_levelText);
                var hLevel = H.GetLevel();
                DialogHosts.UnitWindow.Level.Text = level + " " + IntToStr(hLevel);
                DialogHosts.UnitWindow.T2Leader.Visible = true;
                DialogHosts.UnitWindow.T2Text.Visible = false;
                var nickname = H.GetNickname();
                DialogHosts.UnitWindow.UnitName.Text = H.HeroName + " " + nickname;
            }

            var exp = AbilityRules.TranslateResourceString(s_experienceText);
            var experience = U.GetExperience();
            var nextLevelExperience = U.GetNextLevelExperience();
            DialogHosts.UnitWindow.XP.Text = exp + " (" + IntToStr(experience) + "/" + IntToStr(nextLevelExperience) + ")";
            DialogHosts.UnitWindow.UnitWin.Cursor = 0;
            if (DialogHosts.UnitWindow.UnitTab.Items.Count <= DialogHosts.UnitWindow.UnitTab.Selected || DialogHosts.UnitWindow.UnitTab.Selected < 0)
            {
                DialogHosts.UnitWindow.UnitTab.Selected = 0;
            }

            DialogHosts.UnitWindow.UnitWin.Visible = true;
            if (H != null && (AoWHSMap.Current.Combat == null && H.Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Combat != null && AoWHSMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)AoWHSMap.Current.Combat.CombatData.CurrentPlayer).Player == H.Player))
            {
                if (H.Inventory.GetItem(0) != null)
                {
                    var inventory = H.Inventory.GetItem(0);
                    DialogHosts.UnitWindow.BP1.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.BP1.Tag = -1;
                }

                if (H.Inventory.GetItem(1) != null)
                {
                    var inventory = H.Inventory.GetItem(1);
                    DialogHosts.UnitWindow.BP2.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.BP2.Tag = -1;
                }

                if (H.Inventory.GetItem(2) != null)
                {
                    var inventory = H.Inventory.GetItem(2);
                    DialogHosts.UnitWindow.BP3.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.BP3.Tag = -1;
                }

                if (H.Inventory.GetItem(3) != null)
                {
                    var inventory = H.Inventory.GetItem(3);
                    DialogHosts.UnitWindow.BP4.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.BP4.Tag = -1;
                }

                if (H.Inventory.GetItem(4) != null)
                {
                    var inventory = H.Inventory.GetItem(4);
                    DialogHosts.UnitWindow.BP5.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.BP5.Tag = -1;
                }

                if (H.Inventory.GetItem(5) != null)
                {
                    var inventory = H.Inventory.GetItem(5);
                    DialogHosts.UnitWindow.BP6.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.BP6.Tag = -1;
                }

                if (H.Inventory.GetItem(6) != null)
                {
                    var inventory = H.Inventory.GetItem(6);
                    DialogHosts.UnitWindow.BP7.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.BP7.Tag = -1;
                }

                if (H.Inventory.GetItem(7) != null)
                {
                    var inventory = H.Inventory.GetItem(7);
                    DialogHosts.UnitWindow.BP8.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.BP8.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.AttackSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.AttackSlot);
                    DialogHosts.UnitWindow.Attack.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Attack.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.DefenseSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.DefenseSlot);
                    DialogHosts.UnitWindow.Defend.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Defend.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.HeadSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.HeadSlot);
                    DialogHosts.UnitWindow.Head.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Head.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.TorsoSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.TorsoSlot);
                    DialogHosts.UnitWindow.Torso.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Torso.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot);
                    DialogHosts.UnitWindow.RingL.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.RingL.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.RightRingSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.RightRingSlot);
                    DialogHosts.UnitWindow.RingR.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.RingR.Tag = -1;
                }

                DialogHosts.UnitWindow.UpdateGround(0);
                s_selectedItem = null;
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
                if (AoWHSMap.Current.CombatMap == null)
                {
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    DialogHosts.UnitWindow.UMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                }
                else
                {
                    if (DialogHosts.General.CombatUnit != null)
                    {
                        var moves = DialogHosts.General.CombatUnit.Unit.GetMoves();
                        DialogHosts.UnitWindow.UMoves.Text = IntToStr(DialogHosts.General.CombatUnit.Unit.MovePoints) + "/" + IntToStr(moves);
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
                var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(moveTypes);
                DialogHosts.UnitWindow.UMovesIcon.CurrentImage = moveTypesToInt32;
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
                AAlignment.Visible = alignment != Units.Alignment.None;
                switch (U.GetAlignment())
                {
                    case Units.Alignment.PureGood:
                        AAlignment.CurrentImage = 31;
                        break;
                    case Units.Alignment.Good:
                        AAlignment.CurrentImage = 32;
                        break;
                    case Units.Alignment.PureNeutral:
                    case Units.Alignment.Neutral:
                        AAlignment.CurrentImage = 33;
                        break;
                    case Units.Alignment.Evil:
                        AAlignment.CurrentImage = 34;
                        break;
                    case Units.Alignment.PureEvil:
                        AAlignment.CurrentImage = 35;
                        break;
                }

                var uAlignment = U.GetAlignment();
                var alignmentText = AbilityRules.TranslateResourceString(GameText.s_alignmentTexts[(int)uAlignment]);
                AAlignmentLbl.Text = alignmentText;
                DialogHosts.UnitWindow.T1AbilityLB.Items.Clear();
                DialogHosts.UnitWindow.T1TypeLB.Items.Clear();
                DialogHosts.UnitWindow.T1SrcLB.Items.Clear();
                var morale = U.GetMorale();
                var moraleText = AbilityRules.TranslateResourceString(s_moraleTexts[(int)morale]);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_moraleFormat), [moraleText]);
                MoraleLbl.Text = format;
                var unitType = U.GetUnitType();
                MoraleLbl.Visible = unitType != UnitType.Machine;
                if (AoWHSMap.Current.CombatMap == null)
                {
                    U.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                    StringSorting.SortStrings(T1AbilityLB.Items, CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                }
                else
                {
                    if (DialogHosts.General.CombatUnit != null && DialogHosts.General.CombatUnit.Unit != null && DialogHosts.General.CombatUnit.Unit.Troop != null)
                    {
                        DialogHosts.General.CombatUnit.Unit.Troop.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                        StringSorting.SortStrings(T1AbilityLB.Items, CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                    }
                }

                DialogHosts.UnitWindow.T1AbilityLB.Selected = -1;
                DialogHosts.UnitWindow.T1AbilityLBChange(T1AbilityLB);
                UnitWin.Drawn = false;
            }
        }
    }
}
