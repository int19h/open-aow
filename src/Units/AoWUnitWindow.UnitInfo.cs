#nullable disable
using Aow.Game.Engine;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
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
            Aow.Game.Ui.Dialogs.DialogHosts.General.BuildAbilities();
            U = DialogHosts.General.FSelectedUnit;
            if (U != null && World.Map.AoWHSMap.Current.FSeatedPlayer == U.FPlayer)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FTutorialControl.ShowMessage(64, true, true);
            }

            if (U is Hero)
            {
                H = (Hero)U;
            }
            else
            {
                H = null;
            }

            if (Combat.AoWCombatMap.Current == null && DialogHosts.General.FUnitList != null && Aow.Game.Ui.Dialogs.DialogHosts.General.IndexOfUnit(U) > -1 && DialogHosts.General.FSelectedUnits[1] > -1)
            {
                PPrev.Visible = true;
                PNext.Visible = true;
            }
            else
            {
                PPrev.Visible = false;
                PNext.Visible = false;
            }

            Mana.Visible = false;
            ManaIcon.Visible = false;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Clear();
            var abilities = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(AbilitiesRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Add(abilities);
            var info = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(InfoRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Add(info);
            if (U.GetRace() > -1)
            {
                var race = U.GetRace();
                var raceNames = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[race]);
                TypeV.Text = raceNames;
                RelationIcn.Visible = true;
                var uRace = U.GetRace();
                var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(uRace).GetPlayerRelation(World.Map.AoWHSMap.Current.FSeatedPlayer);
                RelationIcn.CurrentImage = (int)playerRelation + 11;
                var race2 = U.GetRace();
                var playerRelation2 = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(race2).GetPlayerRelation(World.Map.AoWHSMap.Current.FSeatedPlayer);
                var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.TStatusRStr[(int)playerRelation2]);
                RelationV.Text = status;
            }
            else
            {
                TypeV.Text = "";
                RelationIcn.Visible = false;
                var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(NoneRStr);
                RelationV.Text = none;
            }

            if (World.Map.AoWHSMap.Current.FSeatedPlayer == U.FPlayer && !(U is Leader) && World.Map.AoWHSMap.Current.FCombatMap == null)
            {
                DisbandBtn.Visible = true;
                BCloseBtn.Alignment.AlignWidth = AOWAlignWidth.Left;
            }
            else
            {
                DisbandBtn.Visible = false;
                BCloseBtn.Alignment.AlignWidth = AOWAlignWidth.Center;
            }

            BCloseBtn.FAligned = false;
            switch (U.GetUnitType())
            {
                case UnitType.Creature:
                    var creature = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CreatureRStr);
                    TypeV.Text = TypeV.Text + " " + creature;
                    TypeIcn.CurrentImage = 21;
                    break;
                case UnitType.Machine:
                    var machine = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MachineRStr);
                    TypeV.Text = TypeV.Text + " " + machine;
                    TypeIcn.CurrentImage = 22;
                    break;
                default:
                    if (H == null)
                    {
                        var unit = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(UnitRStr);
                        TypeV.Text = TypeV.Text + " " + unit;
                    }
                    else
                    {
                        if (H is Leader)
                        {
                            var leader = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LeaderRStr);
                            TypeV.Text = TypeV.Text + " " + leader;
                        }
                        else
                        {
                            var hero = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(HeroRStr);
                            TypeV.Text = TypeV.Text + " " + hero;
                        }
                    }

                    TypeIcn.CurrentImage = 23;
                    break;
            }

            if (H == null)
            {
                var level = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LevelRStr);
                var unitLevel = U.GetUnitLevel();
                Level.Text = level + " " + IntToStr(unitLevel);
                T2Leader.Visible = false;
                T2Text.Visible = true;
                var description = U.GetDescription();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T2Memo.Items.Assign(description);
                T2SBar.Position = 0;
                var name = U.GetName();
                UnitName.Text = name;
            }
            else
            {
                if (H.FPlayer == 0)
                {
                    SelPnl.Visible = false;
                    var noLeader = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(NoLeaderRStr);
                    SphereLbl.Text = noLeader;
                }
                else
                {
                    SelPnl.Visible = true;
                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 0)
                    {
                        Sel1.Visible = true;
                        var sphere = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(0);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Sel1.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Sel1.Visible = false;
                    }

                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 1)
                    {
                        Sel2.Visible = true;
                        var sphere = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(1);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Sel2.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Sel2.Visible = false;
                    }

                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 2)
                    {
                        Sel3.Visible = true;
                        var sphere = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(2);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Sel3.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Sel3.Visible = false;
                    }

                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 3)
                    {
                        Sel4.Visible = true;
                        var sphere = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(3);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Sel4.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Sel4.Visible = false;
                    }

                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 4)
                    {
                        Sel5.Visible = true;
                        var sphere = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(4);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Sel5.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Sel5.Visible = false;
                    }

                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 5)
                    {
                        Sel6.Visible = true;
                        var sphere = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(5);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Sel6.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Sel6.Visible = false;
                    }

                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSphereCount() > 6)
                    {
                        Sel7.Visible = true;
                        var sphere = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(H.FPlayer).FLeader.GetSpheres(6);
                        var sphereToImage = Aow.Game.Ui.Dialogs.DialogHosts.General.SphereToImage(sphere);
                        Sel7.ILIndexUp = sphereToImage;
                    }
                    else
                    {
                        Sel7.Visible = false;
                    }

                    var leaderSpheres = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LeaderSpheresRStr);
                    SphereLbl.Text = leaderSpheres;
                }

                if (World.Map.AoWHSMap.Current.FCombat == null && H.FPlayer == World.Map.AoWHSMap.Current.FSeatedPlayer || World.Map.AoWHSMap.Current.FCombat != null && Aow.Game.World.Map.AoWHSMap.Current.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)World.Map.AoWHSMap.Current.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer)
                {
                    var items = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ItemsRStr);
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Items.Add(items);
                }

                if (H.GetCastingPointsMax() > 0)
                {
                    Mana.Visible = true;
                    var castingPoints = H.GetCastingPoints();
                    var castingPointsMax = H.GetCastingPointsMax();
                    Mana.Text = IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
                    ManaIcon.Visible = true;
                }

                ItemUseBtn.Visible = false;
                FGroundOffset = 0;
                var level = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LevelRStr);
                var hLevel = H.GetLevel();
                Level.Text = level + " " + IntToStr(hLevel);
                T2Leader.Visible = true;
                T2Text.Visible = false;
                var nickname = H.GetNickname();
                UnitName.Text = H.FHeroName + " " + nickname;
            }

            var exp = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ExpRStr);
            var experience = U.GetExperience();
            var nextLevelExperience = U.GetNextLevelExperience();
            XP.Text = exp + " (" + IntToStr(experience) + "/" + IntToStr(nextLevelExperience) + ")";
            UnitWin.Cursor = 0;
            if (UnitTab.Items.Count <= UnitTab.Selected || UnitTab.Selected < 0)
            {
                UnitTab.Selected = 0;
            }

            UnitWin.Visible = true;
            if (H != null && (World.Map.AoWHSMap.Current.FCombat == null && H.FPlayer == World.Map.AoWHSMap.Current.FSeatedPlayer || World.Map.AoWHSMap.Current.FCombat != null && Aow.Game.World.Map.AoWHSMap.Current.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)World.Map.AoWHSMap.Current.FCombat.FCombatData.FCurrentPlayer).FPlayer == H.FPlayer))
            {
                if (H.FInventory.GetItem(0) != null)
                {
                    var inventory = H.FInventory.GetItem(0);
                    Tag = inventory.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FInventory.GetItem(1) != null)
                {
                    var inventory = H.FInventory.GetItem(1);
                    Tag = inventory.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FInventory.GetItem(2) != null)
                {
                    var inventory = H.FInventory.GetItem(2);
                    Tag = inventory.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FInventory.GetItem(3) != null)
                {
                    var inventory = H.FInventory.GetItem(3);
                    Tag = inventory.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FInventory.GetItem(4) != null)
                {
                    var inventory = H.FInventory.GetItem(4);
                    Tag = inventory.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FInventory.GetItem(5) != null)
                {
                    var inventory = H.FInventory.GetItem(5);
                    Tag = inventory.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FInventory.GetItem(6) != null)
                {
                    var inventory = H.FInventory.GetItem(6);
                    Tag = inventory.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FInventory.GetItem(7) != null)
                {
                    var inventory = H.FInventory.GetItem(7);
                    Tag = inventory.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.AttackSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.AttackSlot);
                    Tag = positionItem.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.DefenseSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.DefenseSlot);
                    Tag = positionItem.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.HeadSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.HeadSlot);
                    Tag = positionItem.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.TorsoSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.TorsoSlot);
                    Tag = positionItem.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.LeftRingSlot);
                    Tag = positionItem.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                if (H.FHeroItems.GetPositionItem(HeroItemPosition.RightRingSlot) != null)
                {
                    var positionItem = H.FHeroItems.GetPositionItem(HeroItemPosition.RightRingSlot);
                    Tag = positionItem.FItemID;
                }
                else
                {
                    Tag = -1;
                }

                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UpdateGround(0);
                SelectedItem = null;
                BP1.Checked = false;
                BP2.Checked = false;
                BP3.Checked = false;
                BP4.Checked = false;
                BP5.Checked = false;
                BP6.Checked = false;
                BP7.Checked = false;
                BP8.Checked = false;
                Attack.Checked = false;
                Defend.Checked = false;
                Head.Checked = false;
                Torso.Checked = false;
                RingL.Checked = false;
                RingR.Checked = false;
                ItemUseBtn.Visible = false;
            }

            if (U != null)
            {
                var hitPoints = U.GetHitPoints();
                var hits = U.GetHits();
                UHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                if (World.Map.AoWHSMap.Current.FCombatMap == null)
                {
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    UMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                }
                else
                {
                    if (DialogHosts.General.FCombatUnit != null)
                    {
                        var moves = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.GetMoves();
                        UMoves.Text = IntToStr(DialogHosts.General.FCombatUnit.FUnit.FMovePoints) + "/" + IntToStr(moves);
                    }
                }

                var attack = U.GetAttack();
                UAttack.Text = IntToStr(attack);
                var damage = U.GetDamage();
                UDamage.Text = IntToStr(damage);
                var defense = U.GetDefense();
                UDefense.Text = IntToStr(defense);
                var resistance = U.GetResistance();
                UResistance.Text = IntToStr(resistance);
                var moveTypes = U.GetMoveTypes();
                var mTtoInt = Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(moveTypes);
                UMovesIcon.CurrentImage = mTtoInt;
                if (U.GetManaUpkeep() > 0)
                {
                    var manaUpkeep = U.GetManaUpkeep();
                    UpKeep.Text = IntToStr(manaUpkeep);
                    UpkeepIcon.CurrentImage = 52;
                }
                else
                {
                    var upkeep = U.GetUpkeep();
                    UpKeep.Text = IntToStr(upkeep);
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
                var alignmentText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.TAlignmentRStr[(int)uAlignment]);
                AAlignmentLbl.Text = alignmentText;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1AbilityLB.Items.Clear();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1TypeLB.Items.Clear();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1SrcLB.Items.Clear();
                var morale = U.GetMorale();
                var moraleText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(TMoraleRStr[(int)morale]);
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MoraleXRStr), [moraleText]);
                MoraleLbl.Text = format;
                var unitType = U.GetUnitType();
                MoraleLbl.Visible = unitType != UnitType.Machine;
                if (World.Map.AoWHSMap.Current.FCombatMap == null)
                {
                    U.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                    Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(T1AbilityLB.Items, Aow.Game.Ui.Dialogs.UnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                }
                else
                {
                    if (DialogHosts.General.FCombatUnit != null && DialogHosts.General.FCombatUnit.FUnit != null && DialogHosts.General.FCombatUnit.FUnit.FUnit != null)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                        Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(T1AbilityLB.Items, Aow.Game.Ui.Dialogs.UnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                    }
                }

                T1AbilityLB.Selected = -1;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.T1AbilityLBChange(T1AbilityLB);
                UnitWin.FDrawn = false;
            }
        }
    }
}
