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
        if (UI.General.SelectedUnit != null)
        {
            UI.General.BuildAbilities();
            U = UI.General.SelectedUnit;
            if (U != null && StrategicMap.Current.SeatedPlayer == U.Player)
            {
                StrategicMap.Current.TutorialControl.ShowMessage(64, true, true);
            }

            if (U is Hero)
            {
                H = (Hero)U;
            }
            else
            {
                H = null;
            }

            if (CombatMap.Current == null && UI.General.UnitList != null && UI.General.IndexOfUnit(U) > -1 && UI.General.SelectedUnits[1] > -1)
            {
                UI.UnitWindow.PreviousUnitButton.Visible = true;
                UI.UnitWindow.NextUnitButton.Visible = true;
            }
            else
            {
                UI.UnitWindow.PreviousUnitButton.Visible = false;
                UI.UnitWindow.NextUnitButton.Visible = false;
            }

            UI.UnitWindow.ManaLabel.Visible = false;
            UI.UnitWindow.ManaIcon.Visible = false;
            UI.UnitWindow.UnitTabs.Items.Clear();
            var abilities = AbilityRules.TranslateResourceString(s_abilitiesText);
            UI.UnitWindow.UnitTabs.Items.Add(abilities);
            var info = AbilityRules.TranslateResourceString(s_infoText);
            UI.UnitWindow.UnitTabs.Items.Add(info);
            if (U.GetRace() > -1)
            {
                var race = U.GetRace();
                var raceNames = AbilityRules.TranslateResourceString(GameTables.RaceNames[race]);
                UI.UnitWindow.TypeValueLabel.Text = raceNames;
                UI.UnitWindow.RelationIcon.Visible = true;
                var uRace = U.GetRace();
                var playerRelation = StrategicMap.Current.Races.GetRace(uRace).GetPlayerRelation(StrategicMap.Current.SeatedPlayer);
                UI.UnitWindow.RelationIcon.CurrentImage = (int)playerRelation + 11;
                var race2 = U.GetRace();
                var playerRelation2 = StrategicMap.Current.Races.GetRace(race2).GetPlayerRelation(StrategicMap.Current.SeatedPlayer);
                var status = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation2]);
                UI.UnitWindow.RelationValueLabel.Text = status;
            }
            else
            {
                UI.UnitWindow.TypeValueLabel.Text = "";
                UI.UnitWindow.RelationIcon.Visible = false;
                var none = AbilityRules.TranslateResourceString(s_noneText);
                UI.UnitWindow.RelationValueLabel.Text = none;
            }

            if (StrategicMap.Current.SeatedPlayer == U.Player && !(U is Leader) && StrategicMap.Current.CombatMap == null)
            {
                UI.UnitWindow.DisbandButton.Visible = true;
                UI.UnitWindow.BottomCloseButton.Alignment.AlignWidth = AlignWidth.Left;
            }
            else
            {
                UI.UnitWindow.DisbandButton.Visible = false;
                UI.UnitWindow.BottomCloseButton.Alignment.AlignWidth = AlignWidth.Center;
            }

            UI.UnitWindow.BottomCloseButton.Aligned = false;
            switch (U.GetUnitType())
            {
                case UnitType.Creature:
                    var creature = AbilityRules.TranslateResourceString(s_creatureText);
                    UI.UnitWindow.TypeValueLabel.Text = UI.UnitWindow.TypeValueLabel.Text + " " + creature;
                    UI.UnitWindow.TypeIcon.CurrentImage = 21;
                    break;
                case UnitType.Machine:
                    var machine = AbilityRules.TranslateResourceString(s_machineText);
                    UI.UnitWindow.TypeValueLabel.Text = UI.UnitWindow.TypeValueLabel.Text + " " + machine;
                    UI.UnitWindow.TypeIcon.CurrentImage = 22;
                    break;
                default:
                    if (H == null)
                    {
                        var unit = AbilityRules.TranslateResourceString(s_unitText);
                        UI.UnitWindow.TypeValueLabel.Text = UI.UnitWindow.TypeValueLabel.Text + " " + unit;
                    }
                    else
                    {
                        if (H is Leader)
                        {
                            var leader = AbilityRules.TranslateResourceString(s_leaderText);
                            UI.UnitWindow.TypeValueLabel.Text = UI.UnitWindow.TypeValueLabel.Text + " " + leader;
                        }
                        else
                        {
                            var hero = AbilityRules.TranslateResourceString(s_heroText);
                            UI.UnitWindow.TypeValueLabel.Text = UI.UnitWindow.TypeValueLabel.Text + " " + hero;
                        }
                    }

                    UI.UnitWindow.TypeIcon.CurrentImage = 23;
                    break;
            }

            if (H == null)
            {
                var level = AbilityRules.TranslateResourceString(s_levelText);
                var unitLevel = U.GetUnitLevel();
                UI.UnitWindow.LevelLabel.Text = level + " " + IntToStr(unitLevel);
                UI.UnitWindow.InfoLeaderPanel.Visible = false;
                UI.UnitWindow.InfoTextPanel.Visible = true;
                var description = U.GetDescription();
                UI.UnitWindow.InfoMemo.Items.Assign(description);
                UI.UnitWindow.InfoScrollBar.Position = 0;
                var name = U.GetName();
                UI.UnitWindow.UnitNameLabel.Text = name;
            }
            else
            {
                if (H.Player == 0)
                {
                    UI.UnitWindow.SpherePanel.Visible = false;
                    var noLeader = AbilityRules.TranslateResourceString(s_noLeaderText);
                    UI.UnitWindow.SphereLabel.Text = noLeader;
                }
                else
                {
                    UI.UnitWindow.SpherePanel.Visible = true;
                    if (StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 0)
                    {
                        UI.UnitWindow.Sphere1Button.Visible = true;
                        var sphere = StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(0);
                        var sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere1Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere1Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 1)
                    {
                        UI.UnitWindow.Sphere2Button.Visible = true;
                        var sphere = StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(1);
                        var sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere2Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere2Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 2)
                    {
                        UI.UnitWindow.Sphere3Button.Visible = true;
                        var sphere = StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(2);
                        var sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere3Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere3Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 3)
                    {
                        UI.UnitWindow.Sphere4Button.Visible = true;
                        var sphere = StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(3);
                        var sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere4Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere4Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 4)
                    {
                        UI.UnitWindow.Sphere5Button.Visible = true;
                        var sphere = StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(4);
                        var sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere5Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere5Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 5)
                    {
                        UI.UnitWindow.Sphere6Button.Visible = true;
                        var sphere = StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(5);
                        var sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere6Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere6Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSphereCount() > 6)
                    {
                        UI.UnitWindow.Sphere7Button.Visible = true;
                        var sphere = StrategicMap.Current.Players.GetPlayers(H.Player).Leader.GetSpheres(6);
                        var sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere7Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere7Button.Visible = false;
                    }

                    var leaderSpheres = AbilityRules.TranslateResourceString(s_leaderSpheresText);
                    UI.UnitWindow.SphereLabel.Text = leaderSpheres;
                }

                if (StrategicMap.Current.Combat == null && H.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Combat != null && StrategicMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)StrategicMap.Current.Combat.CombatData.CurrentPlayer).Player == H.Player)
                {
                    var items = AbilityRules.TranslateResourceString(s_itemsText);
                    UI.UnitWindow.UnitTabs.Items.Add(items);
                }

                if (H.GetCastingPointsMax() > 0)
                {
                    UI.UnitWindow.ManaLabel.Visible = true;
                    var castingPoints = H.GetCastingPoints();
                    var castingPointsMax = H.GetCastingPointsMax();
                    UI.UnitWindow.ManaLabel.Text = IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
                    UI.UnitWindow.ManaIcon.Visible = true;
                }

                UI.UnitWindow.ItemUseButton.Visible = false;
                UI.UnitWindow._groundOffset = 0;
                var level = AbilityRules.TranslateResourceString(s_levelText);
                var hLevel = H.GetLevel();
                UI.UnitWindow.LevelLabel.Text = level + " " + IntToStr(hLevel);
                UI.UnitWindow.InfoLeaderPanel.Visible = true;
                UI.UnitWindow.InfoTextPanel.Visible = false;
                var nickname = H.GetNickname();
                UI.UnitWindow.UnitNameLabel.Text = H.HeroName + " " + nickname;
            }

            var exp = AbilityRules.TranslateResourceString(s_experienceText);
            var experience = U.GetExperience();
            var nextLevelExperience = U.GetNextLevelExperience();
            UI.UnitWindow.ExperienceLabel.Text = exp + " (" + IntToStr(experience) + "/" + IntToStr(nextLevelExperience) + ")";
            UI.UnitWindow.MainWindow.Cursor = 0;
            if (UI.UnitWindow.UnitTabs.Items.Count <= UI.UnitWindow.UnitTabs.Selected || UI.UnitWindow.UnitTabs.Selected < 0)
            {
                UI.UnitWindow.UnitTabs.Selected = 0;
            }

            UI.UnitWindow.MainWindow.Visible = true;
            if (H != null && (StrategicMap.Current.Combat == null && H.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Combat != null && StrategicMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)StrategicMap.Current.Combat.CombatData.CurrentPlayer).Player == H.Player))
            {
                if (H.Inventory.GetItem(0) != null)
                {
                    var inventory = H.Inventory.GetItem(0);
                    UI.UnitWindow.Backpack1CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack1CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(1) != null)
                {
                    var inventory = H.Inventory.GetItem(1);
                    UI.UnitWindow.Backpack2CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack2CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(2) != null)
                {
                    var inventory = H.Inventory.GetItem(2);
                    UI.UnitWindow.Backpack3CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack3CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(3) != null)
                {
                    var inventory = H.Inventory.GetItem(3);
                    UI.UnitWindow.Backpack4CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack4CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(4) != null)
                {
                    var inventory = H.Inventory.GetItem(4);
                    UI.UnitWindow.Backpack5CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack5CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(5) != null)
                {
                    var inventory = H.Inventory.GetItem(5);
                    UI.UnitWindow.Backpack6CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack6CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(6) != null)
                {
                    var inventory = H.Inventory.GetItem(6);
                    UI.UnitWindow.Backpack7CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack7CheckBox.Tag = -1;
                }

                if (H.Inventory.GetItem(7) != null)
                {
                    var inventory = H.Inventory.GetItem(7);
                    UI.UnitWindow.Backpack8CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack8CheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.AttackSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.AttackSlot);
                    UI.UnitWindow.AttackSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.AttackSlotCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.DefenseSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.DefenseSlot);
                    UI.UnitWindow.DefendSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.DefendSlotCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.HeadSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.HeadSlot);
                    UI.UnitWindow.HeadSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.HeadSlotCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.TorsoSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.TorsoSlot);
                    UI.UnitWindow.TorsoSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.TorsoSlotCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot);
                    UI.UnitWindow.LeftRingCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.LeftRingCheckBox.Tag = -1;
                }

                if (H.HeroItems.GetPositionItem(HeroItemPosition.RightRingSlot) != null)
                {
                    var positionItem = H.HeroItems.GetPositionItem(HeroItemPosition.RightRingSlot);
                    UI.UnitWindow.RightRingCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.RightRingCheckBox.Tag = -1;
                }

                UI.UnitWindow.UpdateGround(0);
                s_selectedItem = null;
                UI.UnitWindow.Backpack1CheckBox.Checked = false;
                UI.UnitWindow.Backpack2CheckBox.Checked = false;
                UI.UnitWindow.Backpack3CheckBox.Checked = false;
                UI.UnitWindow.Backpack4CheckBox.Checked = false;
                UI.UnitWindow.Backpack5CheckBox.Checked = false;
                UI.UnitWindow.Backpack6CheckBox.Checked = false;
                UI.UnitWindow.Backpack7CheckBox.Checked = false;
                UI.UnitWindow.Backpack8CheckBox.Checked = false;
                UI.UnitWindow.AttackSlotCheckBox.Checked = false;
                UI.UnitWindow.DefendSlotCheckBox.Checked = false;
                UI.UnitWindow.HeadSlotCheckBox.Checked = false;
                UI.UnitWindow.TorsoSlotCheckBox.Checked = false;
                UI.UnitWindow.LeftRingCheckBox.Checked = false;
                UI.UnitWindow.RightRingCheckBox.Checked = false;
                UI.UnitWindow.ItemUseButton.Visible = false;
            }

            if (U != null)
            {
                var hitPoints = U.GetHitPoints();
                var hits = U.GetHits();
                UI.UnitWindow.UnitHitsLabel.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                if (StrategicMap.Current.CombatMap == null)
                {
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    UI.UnitWindow.UnitMovesLabel.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                }
                else
                {
                    if (UI.General.CombatUnit != null)
                    {
                        var moves = UI.General.CombatUnit.Unit.GetMoves();
                        UI.UnitWindow.UnitMovesLabel.Text = IntToStr(UI.General.CombatUnit.Unit.MovePoints) + "/" + IntToStr(moves);
                    }
                }

                var attack = U.GetAttack();
                UI.UnitWindow.UnitAttackLabel.Text = IntToStr(attack);
                var damage = U.GetDamage();
                UI.UnitWindow.UnitDamageLabel.Text = IntToStr(damage);
                var defense = U.GetDefense();
                UI.UnitWindow.UnitDefenseLabel.Text = IntToStr(defense);
                var resistance = U.GetResistance();
                UI.UnitWindow.UnitResistanceLabel.Text = IntToStr(resistance);
                var moveTypes = U.GetMoveTypes();
                var moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
                UI.UnitWindow.UnitMovesIcon.CurrentImage = moveTypesToInt32;
                if (U.GetManaUpkeep() > 0)
                {
                    var manaUpkeep = U.GetManaUpkeep();
                    UI.UnitWindow.UpkeepLabel.Text = IntToStr(manaUpkeep);
                    UpkeepIcon.CurrentImage = 52;
                }
                else
                {
                    var upkeep = U.GetUpkeep();
                    UI.UnitWindow.UpkeepLabel.Text = IntToStr(upkeep);
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
                UI.UnitWindow.AbilityAbilityList.Items.Clear();
                UI.UnitWindow.AbilityTypeList.Items.Clear();
                UI.UnitWindow.AbilitySourceList.Items.Clear();
                var morale = U.GetMorale();
                var moraleText = AbilityRules.TranslateResourceString(s_moraleTexts[(int)morale]);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_moraleFormat), [moraleText]);
                MoraleLabel.Text = format;
                var unitType = U.GetUnitType();
                MoraleLabel.Visible = unitType != UnitType.Machine;
                if (StrategicMap.Current.CombatMap == null)
                {
                    U.ListAbilityInfo(AbilityAbilityList.Items, AbilityTypeList.Items, AbilitySourceList.Items);
                    StringSorting.SortStrings(AbilityAbilityList.Items, CompareUnitAbilities, [AbilityTypeList.Items, AbilitySourceList.Items]);
                }
                else
                {
                    if (UI.General.CombatUnit != null && UI.General.CombatUnit.Unit != null && UI.General.CombatUnit.Unit.Troop != null)
                    {
                        UI.General.CombatUnit.Unit.Troop.ListAbilityInfo(AbilityAbilityList.Items, AbilityTypeList.Items, AbilitySourceList.Items);
                        StringSorting.SortStrings(AbilityAbilityList.Items, CompareUnitAbilities, [AbilityTypeList.Items, AbilitySourceList.Items]);
                    }
                }

                UI.UnitWindow.AbilityAbilityList.Selected = -1;
                UI.UnitWindow.T1AbilityLBChange(AbilityAbilityList);
                MainWindow.Drawn = false;
            }
        }
    }
}
