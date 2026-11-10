#nullable disable
using Aow.Game.Engine;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
namespace Aow.Game.Ui.Dialogs;

public partial class TUnitWindow
{
    public void ShowUnit()
    {
        THero H = default;
        TAbstractUnit U = default;
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit != null)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.BuildAbilities();
            U = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit;
            if (U != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == U.FPlayer)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(64, true, true);
            }

            if (U is THero)
            {
                H = (THero)U;
            }
            else
            {
                H = null;
            }

            if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap == null && Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList != null && Aow.Game.Ui.Dialogs.DialogHosts.General.IndexOfUnit(U) > -1 && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[1] > -1)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.PPrev.Visible = true;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.PNext.Visible = true;
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.PPrev.Visible = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.PNext.Visible = false;
            }

            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Mana.Visible = false;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ManaIcon.Visible = false;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Clear();
            var abilities = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.AbilitiesRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Add(abilities);
            var info = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.InfoRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Add(info);
            if (U.GetRace() > -1)
            {
                var race = U.GetRace();
                var raceNames = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesRStr[race]);
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text = raceNames;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RelationIcn.Visible = true;
                var uRace = U.GetRace();
                var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(uRace).GetPlayerRelation(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RelationIcn.CurrentImage = (int)playerRelation + 11;
                var race2 = U.GetRace();
                var playerRelation2 = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(race2).GetPlayerRelation(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TStatusRStr[(int)playerRelation2]);
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RelationV.Text = status;
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text = "";
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RelationIcn.Visible = false;
                var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.NoneRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RelationV.Text = none;
            }

            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == U.FPlayer && !(U is TLeader) && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.DisbandBtn.Visible = true;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BCloseBtn.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.DisbandBtn.Visible = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BCloseBtn.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            }

            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BCloseBtn.FAligned = false;
            switch (U.GetUnitType())
            {
                case TUnitType.utCreature:
                    var creature = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.CreatureRStr);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text + " " + creature;
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeIcn.CurrentImage = 21;
                    break;
                case TUnitType.utMachine:
                    var machine = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.MachineRStr);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text + " " + machine;
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeIcn.CurrentImage = 22;
                    break;
                default:
                    if (H == null)
                    {
                        var unit = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.UnitRStr);
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text + " " + unit;
                    }
                    else
                    {
                        if (H is TLeader)
                        {
                            var leader = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.LeaderRStr);
                            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text + " " + leader;
                        }
                        else
                        {
                            var hero = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.HeroRStr);
                            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeV.Text + " " + hero;
                        }
                    }

                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.TypeIcn.CurrentImage = 23;
                    break;
            }

            if (H == null)
            {
                var level = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.LevelRStr);
                var unitLevel = U.GetUnitLevel();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Level.Text = level + " " + IntToStr(unitLevel);
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T2Leader.Visible = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T2Text.Visible = true;
                var description = U.GetDescription();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T2Memo.Items.Assign(description);
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T2SBar.Position = 0;
                var name = U.GetName();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitName.Text = name;
            }
            else
            {
                if (H.FPlayer == 0)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.SelPnl.Visible = false;
                    var noLeader = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.NoLeaderRStr);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.SphereLbl.Text = noLeader;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.SelPnl.Visible = true;
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 0)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel1.Visible = true;
                        var sphere = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(0);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel1.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel1.Visible = false;
                    }

                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 1)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel2.Visible = true;
                        var sphere = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(1);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel2.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel2.Visible = false;
                    }

                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 2)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel3.Visible = true;
                        var sphere = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(2);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel3.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel3.Visible = false;
                    }

                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 3)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel4.Visible = true;
                        var sphere = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(3);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel4.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel4.Visible = false;
                    }

                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 4)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel5.Visible = true;
                        var sphere = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(4);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel5.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel5.Visible = false;
                    }

                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 5)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel6.Visible = true;
                        var sphere = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(5);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel6.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel6.Visible = false;
                    }

                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 6)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel7.Visible = true;
                        var sphere = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(6);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel7.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Sel7.Visible = false;
                    }

                    var leaderSpheres = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.LeaderSpheresRStr);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.SphereLbl.Text = leaderSpheres;
                }

                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombat == null && H.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombat != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer)
                {
                    var items = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.ItemsRStr);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Add(items);
                }

                if (H.GetCastingPointsMax() > 0)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Mana.Visible = true;
                    var castingPoints = H.GetCastingPoints();
                    var castingPointsMax = H.GetCastingPointsMax();
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Mana.Text = IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ManaIcon.Visible = true;
                }

                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ItemUseBtn.Visible = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.FGroundOffset = 0;
                var level = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.LevelRStr);
                var hLevel = H.GetLevel();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Level.Text = level + " " + IntToStr(hLevel);
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T2Leader.Visible = true;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T2Text.Visible = false;
                var nickname = H.GetNickname();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitName.Text = H.FHeroName + " " + nickname;
            }

            var exp = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.ExpRStr);
            var experience = U.GetExperience();
            var nextLevelExperience = U.GetNextLevelExperience();
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.XP.Text = exp + " (" + IntToStr(experience) + "/" + IntToStr(nextLevelExperience) + ")";
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Cursor = 0;
            if (Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Count <= Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Selected || Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Selected < 0)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Selected = 0;
            }

            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = true;
            if (H != null && (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombat == null && H.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombat != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer))
            {
                if (H.FInventory.GetItem(0) != null)
                {
                    var inventory = H.FInventory.GetItem(0);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP1.Tag = inventory.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP1.Tag = -1;
                }

                if (H.FInventory.GetItem(1) != null)
                {
                    var inventory = H.FInventory.GetItem(1);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP2.Tag = inventory.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP2.Tag = -1;
                }

                if (H.FInventory.GetItem(2) != null)
                {
                    var inventory = H.FInventory.GetItem(2);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP3.Tag = inventory.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP3.Tag = -1;
                }

                if (H.FInventory.GetItem(3) != null)
                {
                    var inventory = H.FInventory.GetItem(3);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP4.Tag = inventory.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP4.Tag = -1;
                }

                if (H.FInventory.GetItem(4) != null)
                {
                    var inventory = H.FInventory.GetItem(4);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP5.Tag = inventory.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP5.Tag = -1;
                }

                if (H.FInventory.GetItem(5) != null)
                {
                    var inventory = H.FInventory.GetItem(5);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP6.Tag = inventory.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP6.Tag = -1;
                }

                if (H.FInventory.GetItem(6) != null)
                {
                    var inventory = H.FInventory.GetItem(6);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP7.Tag = inventory.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP7.Tag = -1;
                }

                if (H.FInventory.GetItem(7) != null)
                {
                    var inventory = H.FInventory.GetItem(7);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP8.Tag = inventory.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP8.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipAttackSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipAttackSlot);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Attack.Tag = positionItem.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Attack.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipDefenseSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipDefenseSlot);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Defend.Tag = positionItem.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Defend.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipHeadSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipHeadSlot);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Head.Tag = positionItem.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Head.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipTorsoSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipTorsoSlot);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Torso.Tag = positionItem.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Torso.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipLeftRingSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipLeftRingSlot);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RingL.Tag = positionItem.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RingL.Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(THeroItemPosition.ipRightRingSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(THeroItemPosition.ipRightRingSlot);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RingR.Tag = positionItem.FItemID;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RingR.Tag = -1;
                }

                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UpdateGround(0);
                Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem = null;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP1.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP2.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP3.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP4.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP5.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP6.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP7.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.BP8.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Attack.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Defend.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Head.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.Torso.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RingL.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.RingR.Checked = false;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ItemUseBtn.Visible = false;
            }

            if (U != null)
            {
                var hitPoints = U.GetHitPoints();
                var hits = U.GetHits();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
                {
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                }
                else
                {
                    if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null)
                    {
                        var moves = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.GetMoves();
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UMoves.Text = IntToStr(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints) + "/" + IntToStr(moves);
                    }
                }

                var attack = U.GetAttack();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UAttack.Text = IntToStr(attack);
                var damage = U.GetDamage();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UDamage.Text = IntToStr(damage);
                var defense = U.GetDefense();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UDefense.Text = IntToStr(defense);
                var resistance = U.GetResistance();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UResistance.Text = IntToStr(resistance);
                var moveTypes = U.GetMoveTypes();
                var mTtoInt = Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(moveTypes);
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UMovesIcon.CurrentImage = mTtoInt;
                if (U.GetManaUpkeep() > 0)
                {
                    var manaUpkeep = U.GetManaUpkeep();
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UpKeep.Text = IntToStr(manaUpkeep);
                    UpkeepIcon.CurrentImage = 52;
                }
                else
                {
                    var upkeep = U.GetUpkeep();
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UpKeep.Text = IntToStr(upkeep);
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
                var alignmentText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TAlignmentRStr[(int)uAlignment]);
                AAlignmentLbl.Text = alignmentText;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1AbilityLB.Items.Clear();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1TypeLB.Items.Clear();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1SrcLB.Items.Clear();
                var morale = U.GetMorale();
                var moraleText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.TMoraleRStr[(int)morale]);
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.MoraleXRStr), [moraleText]);
                MoraleLbl.Text = format;
                var unitType = U.GetUnitType();
                MoraleLbl.Visible = unitType != TUnitType.utMachine;
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
                {
                    U.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                    Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(T1AbilityLB.Items, Aow.Game.Ui.Dialogs.TUnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                }
                else
                {
                    if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit != null && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit != null)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                        Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(T1AbilityLB.Items, Aow.Game.Ui.Dialogs.TUnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                    }
                }

                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1AbilityLB.Selected = -1;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1AbilityLBChange(T1AbilityLB);
                UnitWin.FDrawn = false;
            }
        }
    }
}
