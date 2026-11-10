// Translated from CombatTerrain.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw
{

    public partial class TCombatTerrainShadow : THexagonSprite
    {
        public TCombatTerrainShadow(TECustomNode owner) : base(owner)
        { }

        public TCombatTerrain FTerrain;

        // CombatTerrain.pas:72
        public override int GetShowPriority() => (FTerrain.GetShowPriority() & 67108863) - 134217728;

        // CombatTerrain.pas:77
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524

    }
}

namespace Aow.Game.World.Hexes
{

    public partial class TCombatTerrain : TTerrainMO
    {
        public TCombatTerrain(TECustomNode owner) : base(owner)
        { }

        // CombatTerrain.pas:82
        public new const int ClassID = 2228582;

        public TCombatTerrainShadow FShadow;
        public int FHitPoints;
        public bool FDamaged;
        public int FDefense;

        // CombatTerrain.pas:85
        public override void Dispose()
        {
            DestroyShadow();
            base.Dispose();
        }

        // CombatTerrain.pas:269
        public override bool PlaceOnMap()
        {
            bool Result = base.PlaceOnMap();
            if (Result)
            {
                var yhx = Yhx;
                var xhx = Xhx;
                UpdateMapFields((TMapLevel)Owner.Owner, xhx, yhx);
            }

            return Result;
        }

        // CombatTerrain.pas:275
        public override void RemoveFromMap()
        {
            TMapLevel MapLevel;
            MapLevel = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // CombatTerrain.pas:149
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            TAbilityCombatInfo AbilityInfo = default;
            TSpellCombatInfo SpellInfo = default;
            base.MapFieldMsgProc(ref Msg);
            if (Msg.HN == 0)
            {
                if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).Contains((byte)((TTerrainMOR)FResource).DecorationType))
                {
                    switch (Msg.MsgID)
                    {
                        case 200704:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(65).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole);
                            break;
                        case 200705:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(24).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole2 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole2);
                            break;
                        case 200706:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(44).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole3 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole3);
                            break;
                        case 200707:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(117).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole4 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole4);
                            break;
                        case 200714:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(31).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole5 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole5);
                            break;
                        case 200715:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(113).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole6 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole6);
                            break;
                        case 200722:
                            if (Msg.Param[1].Int == 30 || Msg.Param[1].Int == 86)
                            {
                                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Int).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref AbilityInfo);
                                RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                var executeDamageRole7 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(AbilityInfo.Damage, AbilityInfo.Attack - FDefense);
                                SetHitPoints(FHitPoints - executeDamageRole7);
                            }

                            break;
                        case 200723:
                            if (Msg.Param[1].Int == 114)
                            {
                                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Int).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                                RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                var executeDamageRole7 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                                SetHitPoints(FHitPoints - executeDamageRole7);
                            }

                            break;
                        case 200710:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(116).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole8 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole8);
                            break;
                        case 200717:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(127).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole9 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole9);
                            break;
                        case 200718:
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(128).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole10 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole10);
                            break;
                        case 200708:
                            FDamaged = Msg.Param[1].Int == 0;
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(112).GetCombatInfo(Msg.Param[0].As<TCombatUnit>().FUnit, ref SpellInfo);
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole11 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(SpellInfo.Damage, SpellInfo.Attack - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole11);
                            Msg.Param[1] = 1;
                            break;
                        case 200711:
                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                            var executeDamageRole12 = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(Aow.Game.Combat.TacticalTables.TCDamage[1], Aow.Game.Combat.TacticalTables.TCAttack[1] - FDefense);
                            SetHitPoints(FHitPoints - executeDamageRole12);
                            break;
                    }
                }

                switch (Msg.MsgID)
                {
                    case 135169:
                        if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0 && Msg.Param[0].Int < ((TCombatTerrainResource)FResource).FDefense)
                        {
                            Msg.Param[0] = ((TCombatTerrainResource)FResource).FDefense;
                        }

                        break;
                }
            }
        }

        // CombatTerrain.pas:291
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
            {
                if (FResource.TerrainList.GetOverlay(Info.HN) == 8)
                {
                    MoveResult.Code = -1;
                    MoveResult.Done = true;
                }
            }
        }

        // CombatTerrain.pas:254
        public override void Activate()
        {
            base.Activate();
            if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).Contains((byte)((TTerrainMOR)FResource).DecorationType))
            {
                FHitPoints = 3;
            }
            else
            {
                FHitPoints = 0;
            }

            FDefense = -2;
            CreateShadow();
        }

        // CombatTerrain.pas:263
        public override void Deactivate()
        {
            DestroyShadow();
            base.Deactivate();
        }

        // CombatTerrain.pas:284
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN < FResource.TerrainList.Count)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384;
            }
        }

        // CombatTerrain.pas:124
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                if (FShadow != null)
                {
                    var showPriority = FShadow.GetShowPriority();
                    VisibilityList.AddSortChild(FShadow, showPriority);
                }
            }
        }

        // CombatTerrain.pas:91
        public override Aow.Game.World.TMapObjectResourceList GetResourceList(TEngine AEngine) => Aow.Game.Engine.TAoWHSSet.AoWHSSet.FCombatResources;

        // CombatTerrain.pas:106
        public override bool CanPlaceOnHS(THexagonSprite HS) => base.CanPlaceOnHS(HS) || HS is TAbstractRoad;

        // CombatTerrain.pas:96
        public void DestroyShadow()
        {
            if (FShadow != null)
            {
                if (GetVisible())
                {
                    FResource.Engine.HSMap.DynamicVisibleHS.RemoveChild(FShadow);
                }

                FShadow?.Dispose();
                FShadow = null;
            }
        }

        // CombatTerrain.pas:111
        public void CreateShadow()
        {
            if (FResource.ImageSequenceList.GetImageSequence(40) != null)
            {
                if (FShadow != null)
                {
                    DestroyShadow();
                }

                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap == null || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FShowUnitShadows)
                {
                    FShadow = new TCombatTerrainShadow(null);
                    FShadow.FTerrain = this;
                }
            }
        }

        // CombatTerrain.pas:133
        public void SetHitPoints(int Value)
        {
            // { Original ignores Value and only marks the terrain damaged. }
            FDamaged = true;
        }

        // CombatTerrain.pas:138
        public void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, (uint)(((TMapField)Owner).Xhx + (((TMapField)Owner).Yhx << 4)));
        }

    }

    public partial class TCombatTerrainResource : TTerrainMOR
    {
        public TCombatTerrainResource(TECustomNode owner) : base(owner)
        { }

        // CombatTerrain.pas:301
        public new const int ClassID = 2228583;

        public sbyte FBurnValue;
        public sbyte FDefense;
        public byte FHitPoints;
        public bool FMeleeTarget;

        // CombatTerrain.pas:310
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FBurnValue), 21);
            Stream.rwByte(ref FHitPoints, 22);
            Stream.rwBool(ref FMeleeTarget, 23);
        }

        // CombatTerrain.pas:304
        public override int ResourceUserClassID() => 2228582;

        // CombatTerrain.pas:307
        public override string ResourceEditName() => "Combat Terrain";

    }
}
