// Translated from UnHolyG.pas.
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
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

    public enum UnholySummoning : byte
    {
        Summoning = 0
    }
}

namespace Aow.Game.World.Terrain
{

    // FMode, FPlayer, FDuration, FOriginalOverlay
    public partial class UnHolyGround : ILTerrainMO
    {
        public UnHolyGround(ECustomNode owner) : base(owner)
        { }

        // UnHolyG.pas:93
        public new const int ClassID = 131985;

        public Set<UnholySummoning> FMode;
        public sbyte FPlayer;
        public sbyte FDuration;
        public sbyte FOriginalOverlay;

        // UnHolyG.pas:204
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, -1, 40);
            Stream.rwShortintEx(ref FDuration, -1, 41);
            Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
        }

        // UnHolyG.pas:243
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Payload.Int);
            }
        }

        // UnHolyG.pas:249
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(UnholySummoning.Summoning))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // UnHolyG.pas:190
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

        // UnHolyG.pas:196
        public override void RemoveFromMap()
        {
            MapLevel Level;
            Level = (MapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // UnHolyG.pas:222
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
                {
                    ArmyPlaced(Msg.Param[0].As<ArmyHS>());
                    return;
                }
            }
            else
            {
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
        }

        // UnHolyG.pas:212
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

        // UnHolyG.pas:184
        public override void UpdateMapField(in HSMapFieldInfo Info)
        {
            if (FResource.TerrainList.Terrain[Info.HN] >= 0)
            {
                ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // UnHolyG.pas:96
        public void TriggerArmyDamage(ArmyHS Army)
        {
            ArmyEventLog Log;
            int Damage;
            int Damaged;
            AbstractUnit AUnit;
            Damaged = 0;
            RandSeed = Aow.Game.World.Map.AoWHSMap.Current.Random(16777215);
            Log = new ArmyEventLog(null);
            Log.FArmyText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(UnholyGroundDamageRStr);
            var unholyGroundDamage = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(UnholyGroundDamageRStr);
            Log.FArmyMessages.Add(unholyGroundDamage);
            Log.FArmyIcon = 2;
            var army = Army.FArmy;
            for (int I = 0, last = army.Count - 1; I <= last; I++)
            {
                AUnit = army.GetUnit(I);
                bool condition = AUnit.GetAlignment() != Alignment.None;
                if (condition)
                {
                    var alignment = AUnit.GetAlignment();
                    condition = Aow.Game.Magic.Abilities.AlignmentTables.alsPureGood.Contains(alignment);
                }

                if (condition && !AUnit.GetAbilityEnabled(1))
                {
                    Damage = AUnit.ExecuteDamageRole(4, 3, Set<DamageType>.Of(DamageType.Death));
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
                    Log.FDamageTypes.Add((int)DamageType.Death);
                }
                else
                {
                    Log.FDamageValues.Add(0);
                    Log.FDamageTypes.Add((int)DamageType.Death);
                }
            }

            Log.FPlayer = army.FPlayer;
            var armyHS = army.GetArmyHS();
            Log.FArmyView.Setup(armyHS.FArmy, 255);

            Log.ValidateGameOver();
            if (Damaged != 0)
            {
                var yhx = GetYhx();
                var lhx = GetLhx();
                var xhx = GetXhx();
                Aow.Game.Events.EventLog.DistributeLocationEventLog(Log, Log.FPlayer, xhx, yhx, lhx, true);
            }

            Log.Release();
            Army.FArmy.Validate();
        }

        // UnHolyG.pas:142
        public void ArmyPlaced(ArmyHS Army)
        {
            if (!Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
            {
                TriggerArmyDamage(Army);
            }
        }

        // UnHolyG.pas:147
        public void RestoreTerrain()
        {
            MapField Field;
            sbyte Overlay;
            Field = (MapField)Owner;
            Overlay = FOriginalOverlay;
            Dispose();
            if (Overlay > 0)
            {
                Aow.Game.World.Map.AoWHSMap.Current.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
            }
        }

        // UnHolyG.pas:157
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

        // UnHolyG.pas:169
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
                bool condition = !AUnit.GetImmunityTypes().Contains(DamageType.Death) && AUnit.GetAlignment() != Alignment.None;
                if (condition)
                {
                    var alignment = AUnit.GetAlignment();
                    condition = Aow.Game.Magic.Abilities.AlignmentTables.alsPureGood.Contains(alignment);
                }

                Result = condition && !AUnit.GetAbilityEnabled(1);
            }

            return Result;
        }

        // UnHolyG.pas:254
        public void ShowSummonAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            MapField Field;
            var lhx = Anim.GetLhx();
            var yhx = Anim.GetYhx();
            var xhx = Anim.GetXhx();
            Field = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
            if (this == Field.FindOwnedChild(131985) && GameTables.AoWEngine.FAnimationDetail != 0 && Anim.FFrameCounter == 0)
            {
                bool condition = AoWHSMap.Current.FSeatedPlayer == Anim.FTag;
                if (!condition)
                {
                    var lhx2 = GetLhx();
                    var yhx2 = GetYhx();
                    var xhx2 = GetXhx();
                    condition = Aow.Game.World.Map.AoWHSMap.Current.WatchingTerrain(xhx2, yhx2, lhx2, 0);
                }

                if (condition)
                {
                    Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(14).FCastSounds.Play(0);
                }
            }
        }

        // UnHolyG.pas:265
        public void NewFrameSummonAnimation(Animation Anim, int FrameCounter)
        {
            var lhx = Anim.GetLhx();
            var yhx = Anim.GetYhx();
            var xhx = Anim.GetXhx();
            var field = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
            if (this == field.FindOwnedChild(131985) && FrameCounter == 9)
            {
                FMode = FMode - Set<UnholySummoning>.Of(UnholySummoning.Summoning);
            }
        }

        private static readonly string UnholyGroundDamageRStr = "Unholy Ground Damage";

        public new partial class Resource : ILTerrainMOR
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            // UnHolyG.pas:278
            public new const int ClassID = 131986;

            // UnHolyG.pas:283
            public override int ResourceUserClassID() => 131985;

            // UnHolyG.pas:273
            public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(UnholyGroundRStr);

            private static readonly string UnholyGroundRStr = "Unholy Ground";

        }

    }
}

namespace Aow.Game.Commands
{

    public partial class UnholyGroundSpellTE : GlobalTargetSpellTE
    {
        public UnholyGroundSpellTE(ECustomNode owner) : base(owner)
        { }

        // UnHolyG.pas:288
        public new const int ClassID = 135943;

        // UnHolyG.pas:340
        public override bool Process(AbstractUnit source, int value)
        {
            base.Process(source, value);
            CreateUnholyGround(source);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
            Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            return false;
        }

        // UnHolyG.pas:293
        public void CreateUnholyGround(AbstractUnit Source)
        {
            MapLevel Level = default;
            HNWalkData[] Walk__cell = new HNWalkData[1];
            ref HNWalkData Walk = ref Walk__cell[0];
            UnHolyGround.Resource Resource = default;
            AoWMapField MapF = default;
            UnHolyGround Ground = default;
            ImageSequenceAnimation Anim = default;
            var walk = Walk;
            Level = AoWHSMap.Current.MapContainer.MapLevels[FLocation.L];
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            Resource = (UnHolyGround.Resource)Aow.Game.Engine.AoWHSSet.Current.MapObjectResourceList.FindChild(131986);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && walk.Y < Level.Height)
                {
                    MapF = (AoWMapField)Level.GetField(walk.X, walk.Y);
                    if (((GlobalTargetSpell)Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(14)).ValidTargetMapF(MapF) && MapF.GetArmyHS() == null)
                    {
                        MapF.RestoreTerrain();
                        Ground = (UnHolyGround)Resource.CreateResourceUser();
                        Ground.FDuration = (sbyte)(3 + (Aow.Game.World.Map.AoWHSMap.Current.Random(11) - 5) / 5);
                        Ground.FPlayer = Source.FPlayer;
                        Ground.FOriginalOverlay = MapF.OverlayType;
                        Aow.Game.World.Map.AoWHSMap.Current.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                        if (Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Ground, walk.X, walk.Y, FLocation.L))
                        {
                            Ground.FMode = Set<UnholySummoning>.Of(UnholySummoning.Summoning);
                            Anim = new ImageSequenceAnimation(null);
                            Anim.SetImageSequence(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(14).FCastImages.ImageSequences[0]);
                            Anim.FOnShow = Ground.ShowSummonAnimation;
                            Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                            Anim.FFrameCounter = -Aow.Game.World.Map.AoWHSMap.Current.Random(15);
                            Anim.FTag = Source.FPlayer;
                            Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Anim, walk.X, walk.Y, FLocation.L);
                        }
                        else
                        {
                            Ground?.Dispose();
                        }
                    }
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class UnholyGroundSpell : GlobalTargetSpell
    {
        public UnholyGroundSpell(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(UnholyGroundsRStr);
            FSpellID = 14;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Death;
            FAboveGroundOnly = false;
        }

        // UnHolyG.pas:349

        // UnHolyG.pas:383
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                TargetSelection.FHNs.Add(0);
            }
        }

        // UnHolyG.pas:392
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            UnholyGroundSpellTE Event;
            Event = new UnholyGroundSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // UnHolyG.pas:368
        public override bool ValidTargetSelectionMapF(MapField Field)
        {
            ArmyHS Army;
            bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
            // { Original precedence accepts these overlays without the terrain test. }
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

        // UnHolyG.pas:360
        public override bool ValidTargetMapF(MapField Field)
        {
            bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
            // { Original precedence accepts these overlays without the terrain test. }
            Result = (Result && Field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
            return Result && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;
        }

        private static readonly string UnholyGroundsRStr = "Evil Woods";

        // UnHolyG.pas:62

        // UnHolyG.pas:62

    }
}
