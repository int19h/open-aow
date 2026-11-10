// Translated from HolyG.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.GlobalTarget;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells
{

    public enum HolySummoning : byte
    {
        Summoning = 0
    }
}

namespace Aow.Game.World.Terrain
{

    // FMode, FPlayer, FDuration, FOriginalOverlay
    public partial class HolyGround : ILTerrainMO
    {
        public HolyGround(ECustomNode owner) : base(owner)
        { }

        // HolyG.pas:92
        public new const int ClassID = 131987;

        public Set<HolySummoning> FMode;
        public sbyte FPlayer;
        public sbyte FDuration;
        public sbyte FOriginalOverlay;

        // HolyG.pas:201
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, -1, 40);
            Stream.rwShortintEx(ref FDuration, -1, 41);
            Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
        }

        // HolyG.pas:243
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Payload.Int);
            }
        }

        // HolyG.pas:249
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(HolySummoning.Summoning))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // HolyG.pas:187
        public override bool PlaceOnMap()
        {
            bool Result = base.PlaceOnMap();
            if (Result)
            {
                var yhx = GetYhx();
                var xhx = GetXhx();
                UpdateMapFields((MapLevel)Owner.Owner, xhx, yhx);
            }

            return Result;
        }

        // HolyG.pas:193
        public override void RemoveFromMap()
        {
            MapLevel Level;
            Level = (MapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // HolyG.pas:219
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                if (FResource.TerrainList.Terrain[Msg.HN] >= 0 && !AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
                {
                    ArmyPlaced(Msg.Param[0].As<ArmyHS>());
                    return;
                }

                return;
            }

            if (Msg.MsgID == 4384)
            {
                if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
                {
                    if (CanDoDamage(Msg.Param[0].As<ArmyHS>().FArmy))
                    {
                        Msg.Result = 1;
                    }
                }
            }
            else
            {
                if (Msg.MsgID == 4402)
                {
                    if (FResource.TerrainList.Terrain[Msg.HN] >= 0 && FPlayer > 0 && FDuration > 0)
                    {
                        RestoreTerrain();
                    }
                }
            }
        }

        // HolyG.pas:209
        public override int CanMoveOver(in HSSenderInfo Info)
        {
            int Result;
            if (FResource.TerrainList.Terrain[Info.HN] >= 0)
            {
                if (!CanDoDamage(((MoveControl)Info.Sender).FSettings.FUnits))
                {
                    Result = 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                Result = 1;
            }

            return Result;
        }

        // HolyG.pas:181
        public override void UpdateMapField(in HSMapFieldInfo Info)
        {
            if (FResource.TerrainList.Terrain[Info.HN] >= 0)
            {
                ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // HolyG.pas:95
        public void TriggerArmyDamage(ArmyHS Army)
        {
            ArmyEventLog Log;
            int Damage;
            int Damaged;
            AbstractUnit AUnit;
            Damaged = 0;
            RandSeed = AoWHSMap.Current.Random(16777215);
            Log = new ArmyEventLog(null);
            Log.FArmyText = AbilityRules.TranslateRStr(HolyGroundDamageRStr);
            var holyGroundDamage = AbilityRules.TranslateRStr(HolyGroundDamageRStr);
            Log.FArmyMessages.Add(holyGroundDamage);
            Log.FArmyIcon = 2;
            Log.FPlayer = Army.FArmy.FPlayer;
            Log.FArmyView.Setup(Army.FArmy, 255);
            var army = Army.FArmy;
            for (int I = 0, last = army.Count - 1; I <= last; I++)
            {
                AUnit = army.GetUnit(I);
                bool condition = AUnit.GetAlignment() != Alignment.None;
                if (condition)
                {
                    var alignment = AUnit.GetAlignment();
                    condition = AlignmentTables.alsPureEvil.Contains(alignment);
                }

                if (condition && !AUnit.GetAbilityEnabled(1))
                {
                    Damage = AUnit.ExecuteDamageRole(3, 4, Set<DamageType>.Of(DamageType.Holy));
                    if (Damage > AUnit.GetHitPoints())
                    {
                        Damage = AUnit.GetHitPoints();
                    }

                    if (Damage != 0)
                    {
                        AUnit.SetHitPoints((sbyte)(AUnit.GetHitPoints() - Damage));
                        Damaged += 1;
                    }

                    Log.FDamageValues.Add((byte)Damage);
                    Log.FDamageTypes.Add((int)DamageType.Holy);
                }
                else
                {
                    Log.FDamageValues.Add(0);
                    Log.FDamageTypes.Add((int)DamageType.None);
                }
            }

            Log.ValidateGameOver();
            if (Damaged != 0)
            {
                var yhx = GetYhx();
                var lhx = GetLhx();
                var xhx = GetXhx();
                EventLog.DistributeLocationEventLog(Log, Log.FPlayer, xhx, yhx, lhx, true);
            }

            Log.Release();
            Army.FArmy.Validate();
        }

        // HolyG.pas:139
        public void ArmyPlaced(ArmyHS Army)
        {
            if (!AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
            {
                TriggerArmyDamage(Army);
            }
        }

        // HolyG.pas:144
        public void RestoreTerrain()
        {
            MapField Field;
            sbyte Overlay;
            Field = (MapField)Owner;
            Overlay = FOriginalOverlay;
            Dispose();
            if (Overlay > 0)
            {
                AoWHSMap.Current.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
            }
        }

        // HolyG.pas:154
        public void NewTurn(sbyte Player)
        {
            ArmyHS Army;
            Army = (ArmyHS)Owner.FindOwnedChild(131607);
            if (Army != null && Player == Army.FArmy.FPlayer)
            {
                TriggerArmyDamage(Army);
            }

            if (Player == FPlayer && FDuration > 0)
            {
                FDuration = (sbyte)(FDuration - 1);
                if (FDuration == 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // HolyG.pas:166
        public bool CanDoDamage(UnitList Units)
        {
            int I;
            AbstractUnit AUnit;
            bool Result = false;
            I = Units.Count;
            while (!Result && I != 0)
            {
                I -= 1;
                AUnit = Units.GetUnit(I);
                bool condition = !AUnit.GetImmunityTypes().Contains(DamageType.Holy) && AUnit.GetAlignment() != Alignment.None;
                if (condition)
                {
                    var alignment = AUnit.GetAlignment();
                    condition = AlignmentTables.alsPureEvil.Contains(alignment);
                }

                Result = condition && !AUnit.GetAbilityEnabled(1);
            }

            return Result;
        }

        // HolyG.pas:254
        public void ShowSummonAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            MapField Field;
            var lhx = Anim.GetLhx();
            var yhx = Anim.GetYhx();
            var xhx = Anim.GetXhx();
            Field = AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
            if (this == Field.FindOwnedChild(131987) && GameTables.AoWEngine.FAnimationDetail != 0 && Anim.FFrameCounter == 0)
            {
                bool condition = AoWHSMap.Current.FSeatedPlayer == Anim.FTag;
                if (!condition)
                {
                    var lhx2 = GetLhx();
                    var yhx2 = GetYhx();
                    var xhx2 = GetXhx();
                    condition = AoWHSMap.Current.WatchingTerrain(xhx2, yhx2, lhx2, 0);
                }

                if (condition)
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(13).FCastSounds.Play(0);
                }
            }
        }

        // HolyG.pas:265
        public void NewFrameSummonAnimation(Animation Anim, int FrameCounter)
        {
            var lhx = Anim.GetLhx();
            var yhx = Anim.GetYhx();
            var xhx = Anim.GetXhx();
            var field = AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
            if (this == field.FindOwnedChild(131987) && FrameCounter == 9)
            {
                FMode = FMode - Set<HolySummoning>.Of(HolySummoning.Summoning);
            }
        }

        private static readonly string HolyGroundDamageRStr = "Holy Ground Damage";

        public new partial class Resource : ILTerrainMOR
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            // HolyG.pas:276
            public new const int ClassID = 131988;

            // HolyG.pas:279
            public override int ResourceUserClassID() => 131987;

            // HolyG.pas:273
            public override string ResourceEditName() => AbilityRules.TranslateRStr(HolyGroundRStr);

            private static readonly string HolyGroundRStr = "Holy Ground";

        }

    }
}

namespace Aow.Game.Commands
{

    public partial class HolyGroundSpellTE : GlobalTargetSpellTE
    {
        public HolyGroundSpellTE(ECustomNode owner) : base(owner)
        { }

        // HolyG.pas:282
        public new const int ClassID = 135942;

        // HolyG.pas:329
        public override bool Process(AbstractUnit source, int value)
        {
            base.Process(source, value);
            CreateHolyGround(source);
            AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            return false;
        }

        // HolyG.pas:285
        public void CreateHolyGround(AbstractUnit Caster)
        {
            MapLevel Level = default;
            HNWalkData Walk = default;
            HolyGround.Resource Resource = default;
            AoWMapField Field = default;
            HolyGround Ground = default;
            ImageSequenceAnimation Anim = default;
            Level = AoWHSMap.Current.MapContainer.GetMapLevel(FLocation.L);
            HNWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            Resource = (HolyGround.Resource)AoWHSSet.Current.MapObjectResourceList.FindChild(131988);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Field = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                    if (((GlobalTargetSpell)AoWHSSet.Current.FSpellControl.GetSpell(13)).ValidTargetMapF(Field))
                    {
                        if (Field.GetArmyHS() == null)
                        {
                            Field.RestoreTerrain();
                            Ground = (HolyGround)Resource.CreateResourceUser();
                            Ground.FDuration = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                            Ground.FPlayer = Caster.FPlayer;
                            Ground.FOriginalOverlay = Field.OverlayType;
                            AoWHSMap.Current.ClearTerrain(Walk.X, Walk.Y, FLocation.L, 0, -1);
                            if (AoWHSMap.Current.PlaceHX(Ground, Walk.X, Walk.Y, FLocation.L))
                            {
                                Ground.FMode = Set<HolySummoning>.Of(HolySummoning.Summoning);
                                Anim = new ImageSequenceAnimation(null);
                                Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(13).FCastImages.ImageSequences[0]);
                                Anim.FOnShow = Ground.ShowSummonAnimation;
                                Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                                Anim.FFrameCounter = -AoWHSMap.Current.Random(15);
                                Anim.FTag = Caster.FPlayer;
                                AoWHSMap.Current.PlaceHX(Anim, Walk.X, Walk.Y, FLocation.L);
                            }
                            else
                            {
                                Ground?.Dispose();
                            }
                        }
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells
{

    public partial class HolyGroundSpell : GlobalTargetSpell
    {
        public HolyGroundSpell(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(HolyGroundsRStr);
            FSpellID = 13;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Life;
            FAboveGroundOnly = false;
        }

        // HolyG.pas:339

        // HolyG.pas:375
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FHNs.Clear();
            for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                TargetSelection.FHNs.Add(0);
            }
        }

        // HolyG.pas:384
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            HolyGroundSpellTE Event;
            Event = new HolyGroundSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // HolyG.pas:360
        public override bool ValidTargetSelectionMapF(MapField Field)
        {
            ArmyHS Army;
            bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
            // { Original overlay test allows these overlays even when the terrain check failed. }
            Result = (Result && Field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
            Result = Result && Field.FindNoneTransparentHS(typeof(Structure)) == null;
            if (Result)
            {
                Army = ((AoWMapField)Field).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // HolyG.pas:350
        public override bool ValidTargetMapF(MapField Field)
        {
            bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
            // { Original overlay test allows these overlays even when the terrain check failed. }
            Result = (Result && Field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
            return Result && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;
        }

        private static readonly string HolyGroundsRStr = "Holy Woods";

        // HolyG.pas:63

        // HolyG.pas:63

    }
}
