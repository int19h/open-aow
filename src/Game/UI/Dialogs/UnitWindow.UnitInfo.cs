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
                DialogHosts.UnitWindow.PreviousUnitButton.Visible = true;
                DialogHosts.UnitWindow.NextUnitButton.Visible = true;
            }
            else
            {
                DialogHosts.UnitWindow.PreviousUnitButton.Visible = false;
                DialogHosts.UnitWindow.NextUnitButton.Visible = false;
            }

            DialogHosts.UnitWindow.ManaLabel.Visible = false;
            DialogHosts.UnitWindow.ManaIcon.Visible = false;
            DialogHosts.UnitWindow.UnitTabs.Items.Clear();
            var abilities = AbilityRules.TranslateResourceString(s_abilitiesText);
            DialogHosts.UnitWindow.UnitTabs.Items.Add(abilities);
            var info = AbilityRules.TranslateResourceString(s_infoText);
            DialogHosts.UnitWindow.UnitTabs.Items.Add(info);
            if (U.GetRace() > -1)
            {
                var race = U.GetRace();
                var raceNames = AbilityRules.TranslateResourceString(GameTables.RaceNames[race]);
                DialogHosts.UnitWindow.TypeValueLabel.Text = raceNames;
                DialogHosts.UnitWindow.RelationIcon.Visible = true;
                var uRace = U.GetRace();
                var playerRelation = AoWHSMap.Current.Races.GetRace(uRace).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer);
                DialogHosts.UnitWindow.RelationIcon.CurrentImage = (int)playerRelation + 11;
                var race2 = U.GetRace();
                var playerRelation2 = AoWHSMap.Current.Races.GetRace(race2).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer);
                var status = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation2]);
                DialogHosts.UnitWindow.RelationValueLabel.Text = status;
            }
            else
            {
                DialogHosts.UnitWindow.TypeValueLabel.Text = "";
                DialogHosts.UnitWindow.RelationIcon.Visible = false;
                var none = AbilityRules.TranslateResourceString(s_noneText);
                DialogHosts.UnitWindow.RelationValueLabel.Text = none;
            }

            if (AoWHSMap.Current.SeatedPlayer == U.Player && !(U is Leader) && AoWHSMap.Current.CombatMap == null)
            {
                DialogHosts.UnitWindow.DisbandButton.Visible = true;
                DialogHosts.UnitWindow.BottomCloseButton.Alignment.AlignWidth = AlignWidth.Left;
            }
            else
            {
                DialogHosts.UnitWindow.DisbandButton.Visible = false;
                DialogHosts.UnitWindow.BottomCloseButton.Alignment.AlignWidth = AlignWidth.Center;
            }

            DialogHosts.UnitWindow.BottomCloseButton.Aligned = false;
            switch (U.GetUnitType())
            {
                case UnitType.Creature:
                    var creature = AbilityRules.TranslateResourceString(s_creatureText);
                    DialogHosts.UnitWindow.TypeValueLabel.Text = DialogHosts.UnitWindow.TypeValueLabel.Text + " " + creature;
                    DialogHosts.UnitWindow.TypeIcon.CurrentImage = 21;
                    break;
                case UnitType.Machine:
                    var machine = AbilityRules.TranslateResourceString(s_machineText);
                    DialogHosts.UnitWindow.TypeValueLabel.Text = DialogHosts.UnitWindow.TypeValueLabel.Text + " " + machine;
                    DialogHosts.UnitWindow.TypeIcon.CurrentImage = 22;
                    break;
                default:
                    if (H == null)
                    {
                        var unit = AbilityRules.TranslateResourceString(s_unitText);
                        DialogHosts.UnitWindow.TypeValueLabel.Text = DialogHosts.UnitWindow.TypeValueLabel.Text + " " + unit;
                    }
                    else
                    {
                        if (H is Leader)
                        {
                            var leader = AbilityRules.TranslateResourceString(s_leaderText);
                            DialogHosts.UnitWindow.TypeValueLabel.Text = DialogHosts.UnitWindow.TypeValueLabel.Text + " " + leader;
                        }
                        else
                        {
                            var hero = AbilityRules.TranslateResourceString(s_heroText);
                            DialogHosts.UnitWindow.TypeValueLabel.Text = DialogHosts.UnitWindow.TypeValueLabel.Text + " " + hero;
                        }
                    }

                    DialogHosts.UnitWindow.TypeIcon.CurrentImage = 23;
                    break;
            }

            if (H == null)
            {
                var level = AbilityRules.TranslateResourceString(s_levelText);
                var unitLevel = U.GetUnitLevel();
                DialogHosts.UnitWindow.LevelLabel.Text = level + " " + IntToStr(unitLevel);
                DialogHosts.UnitWindow.InfoLeaderPanel.Visible = false;
                DialogHosts.UnitWindow.InfoTextPanel.Visible = true;
                var description = U.GetDescription();
                DialogHosts.UnitWindow.InfoMemo.Items.Assign(description);
                DialogHosts.UnitWindow.InfoScrollBar.Position = 0;
                var name = U.GetName();
                DialogHosts.UnitWindow.UnitNameLabel.Text = name;
            }
            else
            {
                if (H.Player == 0)
                {
                    DialogHosts.UnitWindow.SpherePanel.Visible = false;
                    var noLeader = AbilityRules.TranslateResourceString(s_noLeaderText);
                    DialogHosts.UnitWindow.SphereLabel.Text = noLeader;
                }
                else
                {
                    DialogHosts.UnitWindow.SpherePanel.Visible = true;
                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 0)
                    {
                        DialogHosts.UnitWindow.Sphere1Button.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(0);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sphere1Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sphere1Button.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 1)
                    {
                        DialogHosts.UnitWindow.Sphere2Button.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(1);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sphere2Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sphere2Button.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 2)
                    {
                        DialogHosts.UnitWindow.Sphere3Button.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(2);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sphere3Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sphere3Button.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 3)
                    {
                        DialogHosts.UnitWindow.Sphere4Button.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(3);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sphere4Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sphere4Button.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 4)
                    {
                        DialogHosts.UnitWindow.Sphere5Button.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(4);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sphere5Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sphere5Button.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 5)
                    {
                        DialogHosts.UnitWindow.Sphere6Button.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(5);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sphere6Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sphere6Button.Visible = false;
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 6)
                    {
                        DialogHosts.UnitWindow.Sphere7Button.Visible = true;
                        var sphere = AoWHSMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(6);
                        var sphereToImage = DialogHosts.General.SphereToImage(sphere);
                        DialogHosts.UnitWindow.Sphere7Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        DialogHosts.UnitWindow.Sphere7Button.Visible = false;
                    }

                    var leaderSpheres = AbilityRules.TranslateResourceString(s_leaderSpheresText);
                    DialogHosts.UnitWindow.SphereLabel.Text = leaderSpheres;
                }

                if (AoWHSMap.Current.Combat == null && H.Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Combat != null && AoWHSMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)AoWHSMap.Current.Combat.CombatData.CurrentPlayer).Player == H.Player)
                {
                    var items = AbilityRules.TranslateResourceString(s_itemsText);
                    DialogHosts.UnitWindow.UnitTabs.Items.Add(items);
                }

                if (H.GetCastingPointsMax() > 0)
                {
                    DialogHosts.UnitWindow.ManaLabel.Visible = true;
                    var castingPoints = H.GetCastingPoints();
                    var castingPointsMax = H.GetCastingPointsMax();
                    DialogHosts.UnitWindow.ManaLabel.Text = IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
                    DialogHosts.UnitWindow.ManaIcon.Visible = true;
                }

                DialogHosts.UnitWindow.ItemUseButton.Visible = false;
                DialogHosts.UnitWindow._groundOffset = 0;
                var level = AbilityRules.TranslateResourceString(s_levelText);
                var hLevel = H.GetLevel();
                DialogHosts.UnitWindow.LevelLabel.Text = level + " " + IntToStr(hLevel);
                DialogHosts.UnitWindow.InfoLeaderPanel.Visible = true;
                DialogHosts.UnitWindow.InfoTextPanel.Visible = false;
                var nickname = H.GetNickname();
                DialogHosts.UnitWindow.UnitNameLabel.Text = H.HeroName + " " + nickname;
            }

            var exp = AbilityRules.TranslateResourceString(s_experienceText);
            var experience = U.GetExperience();
            var nextLevelExperience = U.GetNextLevelExperience();
            DialogHosts.UnitWindow.ExperienceLabel.Text = exp + " (" + IntToStr(experience) + "/" + IntToStr(nextLevelExperience) + ")";
            DialogHosts.UnitWindow.MainWindow.Cursor = 0;
            if (DialogHosts.UnitWindow.UnitTabs.Items.Count <= DialogHosts.UnitWindow.UnitTabs.Selected || DialogHosts.UnitWindow.UnitTabs.Selected < 0)
            {
                DialogHosts.UnitWindow.UnitTabs.Selected = 0;
            }

            DialogHosts.UnitWindow.MainWindow.Visible = true;
            if (H != null && (AoWHSMap.Current.Combat == null && H.Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Combat != null && AoWHSMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)AoWHSMap.Current.Combat.CombatData.CurrentPlayer).Player == H.Player))
            {
                if (H.Inventory.GetItem(0) != null)
                {
                    var inventory = H.Inventory.GetItem(0);
                    DialogHosts.UnitWindow.Backpack1CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Backpack1CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(1) != null)
                {
                    var inventory = H.Inventory.GetItem(1);
                    DialogHosts.UnitWindow.Backpack2CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Backpack2CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(2) != null)
                {
                    var inventory = H.Inventory.GetItem(2);
                    DialogHosts.UnitWindow.Backpack3CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Backpack3CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(3) != null)
                {
                    var inventory = H.Inventory.GetItem(3);
                    DialogHosts.UnitWindow.Backpack4CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Backpack4CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(4) != null)
                {
                    var inventory = H.Inventory.GetItem(4);
                    DialogHosts.UnitWindow.Backpack5CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Backpack5CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(5) != null)
                {
                    var inventory = H.Inventory.GetItem(5);
                    DialogHosts.UnitWindow.Backpack6CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Backpack6CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(6) != null)
                {
                    var inventory = H.Inventory.GetItem(6);
                    DialogHosts.UnitWindow.Backpack7CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Backpack7CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(7) != null)
                {
                    var inventory = H.Inventory.GetItem(7);
                    DialogHosts.UnitWindow.Backpack8CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.Backpack8CheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.AttackSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.AttackSlot);
                    DialogHosts.UnitWindow.AttackSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.AttackSlotCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.DefenseSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.DefenseSlot);
                    DialogHosts.UnitWindow.DefendSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.DefendSlotCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.HeadSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.HeadSlot);
                    DialogHosts.UnitWindow.HeadSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.HeadSlotCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.TorsoSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.TorsoSlot);
                    DialogHosts.UnitWindow.TorsoSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.TorsoSlotCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot);
                    DialogHosts.UnitWindow.LeftRingCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.LeftRingCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.RightRingSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.RightRingSlot);
                    DialogHosts.UnitWindow.RightRingCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    DialogHosts.UnitWindow.RightRingCheckBox.Tag = -1;
                }

                DialogHosts.UnitWindow.UpdateGround(0);
                s_selectedItem = null;
                DialogHosts.UnitWindow.Backpack1CheckBox.Checked = false;
                DialogHosts.UnitWindow.Backpack2CheckBox.Checked = false;
                DialogHosts.UnitWindow.Backpack3CheckBox.Checked = false;
                DialogHosts.UnitWindow.Backpack4CheckBox.Checked = false;
                DialogHosts.UnitWindow.Backpack5CheckBox.Checked = false;
                DialogHosts.UnitWindow.Backpack6CheckBox.Checked = false;
                DialogHosts.UnitWindow.Backpack7CheckBox.Checked = false;
                DialogHosts.UnitWindow.Backpack8CheckBox.Checked = false;
                DialogHosts.UnitWindow.AttackSlotCheckBox.Checked = false;
                DialogHosts.UnitWindow.DefendSlotCheckBox.Checked = false;
                DialogHosts.UnitWindow.HeadSlotCheckBox.Checked = false;
                DialogHosts.UnitWindow.TorsoSlotCheckBox.Checked = false;
                DialogHosts.UnitWindow.LeftRingCheckBox.Checked = false;
                DialogHosts.UnitWindow.RightRingCheckBox.Checked = false;
                DialogHosts.UnitWindow.ItemUseButton.Visible = false;
            }

            if (U != null)
            {
                var hitPoints = U.GetHitPoints();
                var hits = U.GetHits();
                DialogHosts.UnitWindow.UnitHitsLabel.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                if (AoWHSMap.Current.CombatMap == null)
                {
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    DialogHosts.UnitWindow.UnitMovesLabel.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                }
                else
                {
                    if (DialogHosts.General.CombatUnit != null)
                    {
                        var moves = DialogHosts.General.CombatUnit.Unit.GetMoves();
                        DialogHosts.UnitWindow.UnitMovesLabel.Text = IntToStr(DialogHosts.General.CombatUnit.Unit.MovePoints) + "/" + IntToStr(moves);
                    }
                }

                var attack = U.GetAttack();
                DialogHosts.UnitWindow.UnitAttackLabel.Text = IntToStr(attack);
                var damage = U.GetDamage();
                DialogHosts.UnitWindow.UnitDamageLabel.Text = IntToStr(damage);
                var defense = U.GetDefense();
                DialogHosts.UnitWindow.UnitDefenseLabel.Text = IntToStr(defense);
                var resistance = U.GetResistance();
                DialogHosts.UnitWindow.UnitResistanceLabel.Text = IntToStr(resistance);
                var moveTypes = U.GetMoveTypes();
                var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(moveTypes);
                DialogHosts.UnitWindow.UnitMovesIcon.CurrentImage = moveTypesToInt32;
                if (U.GetManaUpkeep() > 0)
                {
                    var manaUpkeep = U.GetManaUpkeep();
                    DialogHosts.UnitWindow.UpkeepLabel.Text = IntToStr(manaUpkeep);
                    UpkeepIcon.CurrentImage = 52;
                }
                else
                {
                    var upkeep = U.GetUpkeep();
                    DialogHosts.UnitWindow.UpkeepLabel.Text = IntToStr(upkeep);
                    UpkeepIcon.CurrentImage = 51;
                }

                var alignment = U.GetAlignment();
                AlignmentImage.Visible = alignment != Units.Alignment.None;
                switch (U.GetAlignment())
                {
                    case Units.Alignment.PureGood:
                        AlignmentImage.CurrentImage = 31;
                        break;
                    case Units.Alignment.Good:
                        AlignmentImage.CurrentImage = 32;
                        break;
                    case Units.Alignment.PureNeutral:
                    case Units.Alignment.Neutral:
                        AlignmentImage.CurrentImage = 33;
                        break;
                    case Units.Alignment.Evil:
                        AlignmentImage.CurrentImage = 34;
                        break;
                    case Units.Alignment.PureEvil:
                        AlignmentImage.CurrentImage = 35;
                        break;
                }

                var uAlignment = U.GetAlignment();
                var alignmentText = AbilityRules.TranslateResourceString(GameText.AlignmentTexts[(int)uAlignment]);
                AlignmentLabel.Text = alignmentText;
                DialogHosts.UnitWindow.AbilityAbilityList.Items.Clear();
                DialogHosts.UnitWindow.AbilityTypeList.Items.Clear();
                DialogHosts.UnitWindow.AbilitySourceList.Items.Clear();
                var morale = U.GetMorale();
                var moraleText = AbilityRules.TranslateResourceString(s_moraleTexts[(int)morale]);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_moraleFormat), [moraleText]);
                MoraleLabel.Text = format;
                var unitType = U.GetUnitType();
                MoraleLabel.Visible = unitType != UnitType.Machine;
                if (AoWHSMap.Current.CombatMap == null)
                {
                    U.ListAbilityInfo(AbilityAbilityList.Items, AbilityTypeList.Items, AbilitySourceList.Items);
                    StringSorting.SortStrings(AbilityAbilityList.Items, CompareUnitAbilities, [AbilityTypeList.Items, AbilitySourceList.Items]);
                }
                else
                {
                    if (DialogHosts.General.CombatUnit != null && DialogHosts.General.CombatUnit.Unit != null && DialogHosts.General.CombatUnit.Unit.Troop != null)
                    {
                        DialogHosts.General.CombatUnit.Unit.Troop.ListAbilityInfo(AbilityAbilityList.Items, AbilityTypeList.Items, AbilitySourceList.Items);
                        StringSorting.SortStrings(AbilityAbilityList.Items, CompareUnitAbilities, [AbilityTypeList.Items, AbilitySourceList.Items]);
                    }
                }

                DialogHosts.UnitWindow.AbilityAbilityList.Selected = -1;
                DialogHosts.UnitWindow.T1AbilityLBChange(AbilityAbilityList);
                MainWindow.Drawn = false;
            }
        }
    }
}
