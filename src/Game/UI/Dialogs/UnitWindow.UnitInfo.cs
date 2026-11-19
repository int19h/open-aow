#nullable disable
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.UI.Dialogs;

public partial class UnitWindow
{
    public void ShowUnit()
    {
        Hero h = default;
        Troop u = default;
        if (UI.General.SelectedUnit != null)
        {
            UI.General.BuildAbilities();
            u = UI.General.SelectedUnit;
            if (u != null && StrategicMap.Current.SeatedPlayer == u.Player)
            {
                StrategicMap.Current.TutorialControl.ShowMessage(64, true, true);
            }

            h = u is Hero heroUnit ? heroUnit : null;

            if (CombatMap.Current == null && UI.General.UnitList != null && UI.General.IndexOfUnit(u) > -1 && UI.General.SelectedUnits[1] > -1)
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
            string abilities = UI.Engine.TranslateResourceString(s_abilitiesText);
            _ = UI.UnitWindow.UnitTabs.Items.Add(abilities);
            string info = UI.Engine.TranslateResourceString(s_infoText);
            _ = UI.UnitWindow.UnitTabs.Items.Add(info);
            if (u.GetRace() > -1)
            {
                sbyte race = u.GetRace();
                string raceNames = UI.Engine.TranslateResourceString(GameTables.RaceNames[race]);
                UI.UnitWindow.TypeValueLabel.Text = raceNames;
                UI.UnitWindow.RelationIcon.Visible = true;
                sbyte uRace = u.GetRace();
                Status playerRelation = StrategicMap.Current.Races.GetRace(uRace).GetPlayerRelation(StrategicMap.Current.SeatedPlayer);
                UI.UnitWindow.RelationIcon.CurrentImage = (int)playerRelation + 11;
                sbyte race2 = u.GetRace();
                Status playerRelation2 = StrategicMap.Current.Races.GetRace(race2).GetPlayerRelation(StrategicMap.Current.SeatedPlayer);
                string status = UI.Engine.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation2]);
                UI.UnitWindow.RelationValueLabel.Text = status;
            }
            else
            {
                UI.UnitWindow.TypeValueLabel.Text = "";
                UI.UnitWindow.RelationIcon.Visible = false;
                string none = UI.Engine.TranslateResourceString(s_noneText);
                UI.UnitWindow.RelationValueLabel.Text = none;
            }

            if (StrategicMap.Current.SeatedPlayer == u.Player && u is not Leader && StrategicMap.Current.CombatMap == null)
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
            switch (u.GetUnitType())
            {
                case UnitType.Creature:
                    string creature = UI.Engine.TranslateResourceString(s_creatureText);
                    UI.UnitWindow.TypeValueLabel.Text = $"{UI.UnitWindow.TypeValueLabel.Text} {creature}";
                    UI.UnitWindow.TypeIcon.CurrentImage = 21;
                    break;
                case UnitType.Machine:
                    string machine = UI.Engine.TranslateResourceString(s_machineText);
                    UI.UnitWindow.TypeValueLabel.Text = $"{UI.UnitWindow.TypeValueLabel.Text} {machine}";
                    UI.UnitWindow.TypeIcon.CurrentImage = 22;
                    break;
                case UnitType.Humanoid:
                default:
                    if (h == null)
                    {
                        string unit = UI.Engine.TranslateResourceString(s_unitText);
                        UI.UnitWindow.TypeValueLabel.Text = $"{UI.UnitWindow.TypeValueLabel.Text} {unit}";
                    }
                    else
                    {
                        if (h is Leader)
                        {
                            string leader = UI.Engine.TranslateResourceString(s_leaderText);
                            UI.UnitWindow.TypeValueLabel.Text = $"{UI.UnitWindow.TypeValueLabel.Text} {leader}";
                        }
                        else
                        {
                            string hero = UI.Engine.TranslateResourceString(s_heroText);
                            UI.UnitWindow.TypeValueLabel.Text = $"{UI.UnitWindow.TypeValueLabel.Text} {hero}";
                        }
                    }

                    UI.UnitWindow.TypeIcon.CurrentImage = 23;
                    break;
            }

            if (h == null)
            {
                string level = UI.Engine.TranslateResourceString(s_levelText);
                sbyte unitLevel = u.GetUnitLevel();
                UI.UnitWindow.LevelLabel.Text = $"{level} {IntToStr(unitLevel)}";
                UI.UnitWindow.InfoLeaderPanel.Visible = false;
                UI.UnitWindow.InfoTextPanel.Visible = true;
                DelphiStringCollection description = u.GetDescription();
                UI.UnitWindow.InfoMemo.Items.Assign(description);
                UI.UnitWindow.InfoScrollBar.Position = 0;
                string name = u.GetName();
                UI.UnitWindow.UnitNameLabel.Text = name;
            }
            else
            {
                if (h.Player == 0)
                {
                    UI.UnitWindow.SpherePanel.Visible = false;
                    string noLeader = UI.Engine.TranslateResourceString(s_noLeaderText);
                    UI.UnitWindow.SphereLabel.Text = noLeader;
                }
                else
                {
                    UI.UnitWindow.SpherePanel.Visible = true;
                    if (StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSphereCount() > 0)
                    {
                        UI.UnitWindow.Sphere1Button.Visible = true;
                        MagicSphere sphere = StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSpheres(0);
                        int sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere1Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere1Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSphereCount() > 1)
                    {
                        UI.UnitWindow.Sphere2Button.Visible = true;
                        MagicSphere sphere = StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSpheres(1);
                        int sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere2Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere2Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSphereCount() > 2)
                    {
                        UI.UnitWindow.Sphere3Button.Visible = true;
                        MagicSphere sphere = StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSpheres(2);
                        int sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere3Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere3Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSphereCount() > 3)
                    {
                        UI.UnitWindow.Sphere4Button.Visible = true;
                        MagicSphere sphere = StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSpheres(3);
                        int sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere4Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere4Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSphereCount() > 4)
                    {
                        UI.UnitWindow.Sphere5Button.Visible = true;
                        MagicSphere sphere = StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSpheres(4);
                        int sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere5Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere5Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSphereCount() > 5)
                    {
                        UI.UnitWindow.Sphere6Button.Visible = true;
                        MagicSphere sphere = StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSpheres(5);
                        int sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere6Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere6Button.Visible = false;
                    }

                    if (StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSphereCount() > 6)
                    {
                        UI.UnitWindow.Sphere7Button.Visible = true;
                        MagicSphere sphere = StrategicMap.Current.Players.GetPlayers(h.Player).Leader.GetSpheres(6);
                        int sphereToImage = UI.General.SphereToImage(sphere);
                        UI.UnitWindow.Sphere7Button.ImageLibraryIndexUp = sphereToImage;
                    }
                    else
                    {
                        UI.UnitWindow.Sphere7Button.Visible = false;
                    }

                    string leaderSpheres = UI.Engine.TranslateResourceString(s_leaderSpheresText);
                    UI.UnitWindow.SphereLabel.Text = leaderSpheres;
                }

                if ((StrategicMap.Current.Combat == null && h.Player == StrategicMap.Current.SeatedPlayer) || (StrategicMap.Current.Combat != null && StrategicMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)StrategicMap.Current.Combat.CombatData.CurrentPlayer).Player == h.Player))
                {
                    string items = UI.Engine.TranslateResourceString(s_itemsText);
                    _ = UI.UnitWindow.UnitTabs.Items.Add(items);
                }

                if (h.GetCastingPointsMax() > 0)
                {
                    UI.UnitWindow.ManaLabel.Visible = true;
                    sbyte castingPoints = h.GetCastingPoints();
                    sbyte castingPointsMax = h.GetCastingPointsMax();
                    UI.UnitWindow.ManaLabel.Text = $"{IntToStr(castingPoints)}/{IntToStr(castingPointsMax)}";
                    UI.UnitWindow.ManaIcon.Visible = true;
                }

                UI.UnitWindow.ItemUseButton.Visible = false;
                UI.UnitWindow._groundOffset = 0;
                string level = UI.Engine.TranslateResourceString(s_levelText);
                sbyte hLevel = h.GetLevel();
                UI.UnitWindow.LevelLabel.Text = $"{level} {IntToStr(hLevel)}";
                UI.UnitWindow.InfoLeaderPanel.Visible = true;
                UI.UnitWindow.InfoTextPanel.Visible = false;
                string nickname = h.GetNickname();
                UI.UnitWindow.UnitNameLabel.Text = $"{h.HeroName} {nickname}";
            }

            string exp = UI.Engine.TranslateResourceString(s_experienceText);
            int experience = u.GetExperience();
            int nextLevelExperience = u.GetNextLevelExperience();
            UI.UnitWindow.ExperienceLabel.Text = $"{exp} ({IntToStr(experience)}/{IntToStr(nextLevelExperience)})";
            UI.UnitWindow.MainWindow.Cursor = 0;
            if (UI.UnitWindow.UnitTabs.Items.Count <= UI.UnitWindow.UnitTabs.Selected || UI.UnitWindow.UnitTabs.Selected < 0)
            {
                UI.UnitWindow.UnitTabs.Selected = 0;
            }

            UI.UnitWindow.MainWindow.Visible = true;
            if (h != null && ((StrategicMap.Current.Combat == null && h.Player == StrategicMap.Current.SeatedPlayer) || (StrategicMap.Current.Combat != null && StrategicMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)StrategicMap.Current.Combat.CombatData.CurrentPlayer).Player == h.Player)))
            {
                if (h.Inventory.Items.GetItem(0) != null)
                {
                    Item inventory = h.Inventory.Items.GetItem(0);
                    UI.UnitWindow.Backpack1CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack1CheckBox.Tag = -1;
                }

                if (h.Inventory.Items.GetItem(1) != null)
                {
                    Item inventory = h.Inventory.Items.GetItem(1);
                    UI.UnitWindow.Backpack2CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack2CheckBox.Tag = -1;
                }

                if (h.Inventory.Items.GetItem(2) != null)
                {
                    Item inventory = h.Inventory.Items.GetItem(2);
                    UI.UnitWindow.Backpack3CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack3CheckBox.Tag = -1;
                }

                if (h.Inventory.Items.GetItem(3) != null)
                {
                    Item inventory = h.Inventory.Items.GetItem(3);
                    UI.UnitWindow.Backpack4CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack4CheckBox.Tag = -1;
                }

                if (h.Inventory.Items.GetItem(4) != null)
                {
                    Item inventory = h.Inventory.Items.GetItem(4);
                    UI.UnitWindow.Backpack5CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack5CheckBox.Tag = -1;
                }

                if (h.Inventory.Items.GetItem(5) != null)
                {
                    Item inventory = h.Inventory.Items.GetItem(5);
                    UI.UnitWindow.Backpack6CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack6CheckBox.Tag = -1;
                }

                if (h.Inventory.Items.GetItem(6) != null)
                {
                    Item inventory = h.Inventory.Items.GetItem(6);
                    UI.UnitWindow.Backpack7CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack7CheckBox.Tag = -1;
                }

                if (h.Inventory.Items.GetItem(7) != null)
                {
                    Item inventory = h.Inventory.Items.GetItem(7);
                    UI.UnitWindow.Backpack8CheckBox.Tag = inventory.ItemId;
                }
                else
                {
                    UI.UnitWindow.Backpack8CheckBox.Tag = -1;
                }

                if (h.HeroItems.GetPositionItem(HeroItemPosition.AttackSlot) != null)
                {
                    Item positionItem = h.HeroItems.GetPositionItem(HeroItemPosition.AttackSlot);
                    UI.UnitWindow.AttackSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.AttackSlotCheckBox.Tag = -1;
                }

                if (h.HeroItems.GetPositionItem(HeroItemPosition.DefenseSlot) != null)
                {
                    Item positionItem = h.HeroItems.GetPositionItem(HeroItemPosition.DefenseSlot);
                    UI.UnitWindow.DefendSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.DefendSlotCheckBox.Tag = -1;
                }

                if (h.HeroItems.GetPositionItem(HeroItemPosition.HeadSlot) != null)
                {
                    Item positionItem = h.HeroItems.GetPositionItem(HeroItemPosition.HeadSlot);
                    UI.UnitWindow.HeadSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.HeadSlotCheckBox.Tag = -1;
                }

                if (h.HeroItems.GetPositionItem(HeroItemPosition.TorsoSlot) != null)
                {
                    Item positionItem = h.HeroItems.GetPositionItem(HeroItemPosition.TorsoSlot);
                    UI.UnitWindow.TorsoSlotCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.TorsoSlotCheckBox.Tag = -1;
                }

                if (h.HeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot) != null)
                {
                    Item positionItem = h.HeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot);
                    UI.UnitWindow.LeftRingCheckBox.Tag = positionItem.ItemId;
                }
                else
                {
                    UI.UnitWindow.LeftRingCheckBox.Tag = -1;
                }

                if (h.HeroItems.GetPositionItem(HeroItemPosition.RightRingSlot) != null)
                {
                    Item positionItem = h.HeroItems.GetPositionItem(HeroItemPosition.RightRingSlot);
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

            if (u != null)
            {
                sbyte hitPoints = u.GetHitPoints();
                sbyte hits = u.GetHits();
                UI.UnitWindow.UnitHitsLabel.Text = $"{IntToStr(hitPoints)}/{IntToStr(hits)}";
                if (StrategicMap.Current.CombatMap == null)
                {
                    sbyte movePoints = u.GetMovePoints();
                    sbyte moves = u.GetMoves();
                    UI.UnitWindow.UnitMovesLabel.Text = $"{IntToStr(movePoints)}/{IntToStr(moves)}";
                }
                else
                {
                    if (UI.General.CombatUnit != null)
                    {
                        int moves = UI.General.CombatUnit.Unit.GetMoves();
                        UI.UnitWindow.UnitMovesLabel.Text = $"{IntToStr(UI.General.CombatUnit.Unit.MovePoints)}/{IntToStr(moves)}";
                    }
                }

                sbyte attack = u.GetAttack();
                UI.UnitWindow.UnitAttackLabel.Text = IntToStr(attack);
                sbyte damage = u.GetDamage();
                UI.UnitWindow.UnitDamageLabel.Text = IntToStr(damage);
                sbyte defense = u.GetDefense();
                UI.UnitWindow.UnitDefenseLabel.Text = IntToStr(defense);
                sbyte resistance = u.GetResistance();
                UI.UnitWindow.UnitResistanceLabel.Text = IntToStr(resistance);
                Set<MoveType> moveTypes = u.GetMoveTypes();
                int moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
                UI.UnitWindow.UnitMovesIcon.CurrentImage = moveTypesToInt32;
                if (u.GetManaUpkeep() > 0)
                {
                    int manaUpkeep = u.GetManaUpkeep();
                    UI.UnitWindow.UpkeepLabel.Text = IntToStr(manaUpkeep);
                    UpkeepIcon.CurrentImage = 52;
                }
                else
                {
                    byte upkeep = u.GetUpkeep();
                    UI.UnitWindow.UpkeepLabel.Text = IntToStr(upkeep);
                    UpkeepIcon.CurrentImage = 51;
                }

                Units.Alignment alignment = u.GetAlignment();
                AlignmentImage.Visible = alignment != Units.Alignment.None;
                switch (u.GetAlignment())
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
                    case Units.Alignment.None:
                    default:
                        break;
                }

                Units.Alignment uAlignment = u.GetAlignment();
                string alignmentText = UI.Engine.TranslateResourceString(GameText.AlignmentTexts[(int)uAlignment]);
                AlignmentLabel.Text = alignmentText;
                UI.UnitWindow.AbilityAbilityList.Items.Clear();
                UI.UnitWindow.AbilityTypeList.Items.Clear();
                UI.UnitWindow.AbilitySourceList.Items.Clear();
                Troop.Morale morale = u.GetMorale();
                string moraleText = UI.Engine.TranslateResourceString(s_moraleTexts[(int)morale]);
                string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_moraleFormat), [moraleText]);
                MoraleLabel.Text = format;
                UnitType unitType = u.GetUnitType();
                MoraleLabel.Visible = unitType != UnitType.Machine;
                if (StrategicMap.Current.CombatMap == null)
                {
                    u.ListAbilityInfo(AbilityAbilityList.Items, AbilityTypeList.Items, AbilitySourceList.Items);
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
