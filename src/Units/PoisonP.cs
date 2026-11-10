// Translated from PoisonP.pas.
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

    public enum TPoisonPlantStatusElement : byte
    {
        ppsSummoning = 0
    }
}

namespace Aow.Game.World.Terrain
{

    // summoning status. caster player. remaining turns. original overlay
    public partial class TPoisonPlant : TILTerrainMO
    {
        public TPoisonPlant(TECustomNode owner) : base(owner)
        {
            FCasterPlayer = -1;
            FOriginalOverlay = -1;
            FDaysRemaining = -1;
            FPlantStatus = TSet<TPoisonPlantStatusElement>.Of();
        }

        // PoisonP.pas:87
        public new const int ClassID = 131998;

        public TSet<TPoisonPlantStatusElement> FPlantStatus;
        public sbyte FCasterPlayer;
        public sbyte FDaysRemaining;
        public sbyte FOriginalOverlay;

        // PoisonP.pas:97
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FCasterPlayer, -1, 40);
            Stream.rwShortintEx(ref FDaysRemaining, -1, 41);
            Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
        }

        // PoisonP.pas:78

        // PoisonP.pas:255
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Payload.Int);
            }
        }

        // PoisonP.pas:261
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (!FPlantStatus.Contains(TPoisonPlantStatusElement.ppsSummoning))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // PoisonP.pas:240
        public override bool PlaceOnMap()
        {
            bool Result = base.PlaceOnMap();
            if (Result)
            {
                var yhx = GetYhx();
                var xhx = GetXhx();
                UpdateMapFields((TMapLevel)Owner.Owner, xhx, yhx);
            }

            return Result;
        }

        // PoisonP.pas:246
        public override void RemoveFromMap()
        {
            TMapLevel Level;
            Level = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // PoisonP.pas:218
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0)
                {
                    ArmyPlaced(Msg.Param[0].As<TArmyHS>());
                }
            }
            else
            {
                if (Msg.MsgID == 4384)
                {
                    if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0 && !Msg.Param[0].As<TArmyHS>().FArmy.ImmunityTypes(255).Contains(TDamageType.dtPoison) && PoisonPlantActive())
                    {
                        Msg.Result = 1;
                    }
                }
                else
                {
                    if (Msg.MsgID == 4402)
                    {
                        // { Original excludes player zero from early restoration. }
                        if (FResource.TerrainList.GetTerrain(Msg.HN) >= 0 && FCasterPlayer > 0 && FDaysRemaining > 0)
                        {
                            RestoreTerrain();
                        }
                    }
                }
            }
        }

        // PoisonP.pas:209
        public override int CanMoveOver(in THSSenderInfo Info)
        {
            int Result;
            if (FResource.TerrainList.GetTerrain(Info.HN) >= 0 && PoisonPlantActive())
            {
                if (!CanDoDamage(((TMoveControl)Info.Sender).FSettings.FUnits))
                {
                    Result = 1;
                }
                else
                {
                    Result = 0;
                }
            }
            else
            {
                Result = 1;
            }

            return Result;
        }

        // PoisonP.pas:203
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // PoisonP.pas:92
        public bool PoisonPlantActive() => !TSet<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)FTerrainType);

        // PoisonP.pas:105
        public bool CanDoDamage(TUnitList Units)
        {
            int I;
            TAbstractUnit U;
            bool Result = false;
            I = Units.Count;
            while (!Result && I != 0)
            {
                I -= 1;
                U = Units.GetUnit(I);
                Result = !U.GetImmunityTypes().Contains(TDamageType.dtPoison) && !U.GetAbilityEnabled(1);
            }

            return Result;
        }

        // PoisonP.pas:120
        public void TriggerArmyDamage(TArmyHS ArmyHS)
        {
            int Count;
            TUnitList Units;
            TArmyEventLog Log;
            TAbstractUnit U;
            int Damage;
            Count = 0;
            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
            Log = new TArmyEventLog(null);
            Log.FArmyText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.TPoisonPlant.PoisonPlantDamageRStr);
            Log.FPlayer = ArmyHS.FArmy.FPlayer;
            var poisonPlantDamage = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.TPoisonPlant.PoisonPlantDamageRStr);
            Log.FArmyMessages.Add(poisonPlantDamage);
            Log.FArmyIcon = 1;
            Log.FArmyView.Setup(ArmyHS.FArmy, 255);
            Units = ArmyHS.FArmy;
            for (int I = 0, last = Units.Count - 1; I <= last; I++)
            {
                U = Units.GetUnit(I);
                U.AddRef();
                if (!U.GetImmunityTypes().Contains(TDamageType.dtPoison) && !U.GetAbilityEnabled(1))
                {
                    Damage = U.ExecuteDamageRole(4, 3, TSet<TDamageType>.Of(TDamageType.dtPoison));
                    if (Damage > U.GetHitPoints())
                    {
                        Damage = U.GetHitPoints();
                    }

                    if (Damage != 0)
                    {
                        U.SetHitPoints((sbyte)(U.GetHitPoints() - Damage));
                        if (U.GetHitPoints() != 0)
                        {
                            U.ExecuteDamageEffects(TSet<TDamageType>.Of(TDamageType.dtPoison));
                        }

                        Count += 1;
                    }

                    Log.FDamageValues.Add((byte)Damage);
                    Log.FDamageTypes.Add((int)TDamageType.dtPoison);
                }
                else
                {
                    Log.FDamageValues.Add(0);
                    Log.FDamageTypes.Add((int)TDamageType.dtNone);
                }

                U.Release();
            }

            Log.ValidateGameOver();
            if (Count != 0)
            {
                var yhx = GetYhx();
                var lhx = GetLhx();
                var xhx = GetXhx();
                Aow.Game.Events.TEventLog.DistributeLocationEventLog(Log, Log.FPlayer, xhx, yhx, lhx, true);
            }

            ArmyHS.FArmy.Validate();
            Log.Release();
        }

        // PoisonP.pas:168
        public void ArmyPlaced(TArmyHS ArmyHS)
        {
            if (PoisonPlantActive() && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                TriggerArmyDamage(ArmyHS);
            }
        }

        // PoisonP.pas:174
        public void RestoreTerrain()
        {
            TMapField MapF;
            sbyte OriginalOverlay;
            MapF = (TMapField)Owner;
            OriginalOverlay = FOriginalOverlay;
            Dispose();
            if (OriginalOverlay > 0)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceTerrain(MapF.Xhx, MapF.Yhx, MapF.Lhx, MapF.TerrainType, OriginalOverlay);
            }
        }

        // PoisonP.pas:186
        public void NewTurn(sbyte Player)
        {
            TArmyHS ArmyHS;
            if (PoisonPlantActive())
            {
                ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
                if (ArmyHS != null && Player == ArmyHS.FArmy.FPlayer)
                {
                    TriggerArmyDamage(ArmyHS);
                }
            }

            if (Player == FCasterPlayer && FDaysRemaining > 0)
            {
                FDaysRemaining = (sbyte)(FDaysRemaining - 1);
                if (FDaysRemaining == 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // PoisonP.pas:267
        public void ShowSummonAnimation(TAnimation Animation, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TMapField MapF;
            var lhx = Animation.GetLhx();
            var yhx = Animation.GetYhx();
            var xhx = Animation.GetXhx();
            MapF = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
            if (this == MapF.FindOwnedChild(131998) && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail != 0 && Animation.FFrameCounter == 0)
            {
                bool condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == Animation.FTag;
                if (!condition)
                {
                    var lhx2 = GetLhx();
                    var yhx2 = GetYhx();
                    var xhx2 = GetXhx();
                    condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.WatchingTerrain(xhx2, yhx2, lhx2, 0);
                }

                if (condition)
                {
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(12).FCastSounds.Play(0);
                }
            }
        }

        // PoisonP.pas:279
        public void NewFrameSummonAnimation(TAnimation Animation, int FrameCounter)
        {
            var lhx = Animation.GetLhx();
            var yhx = Animation.GetYhx();
            var xhx = Animation.GetXhx();
            var field = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
            if (this == field.FindOwnedChild(131998) && FrameCounter == 9)
            {
                FPlantStatus = FPlantStatus - TSet<TPoisonPlantStatusElement>.Of(TPoisonPlantStatusElement.ppsSummoning);
            }
        }

        private static readonly string PoisonPlantDamageRStr = "Poison Plant Damage";

        // PoisonP.pas:28

        // PoisonP.pas:28

    }

    public partial class TPoisonPlantResource : TILTerrainMOR
    {
        public TPoisonPlantResource(TECustomNode owner) : base(owner)
        { }

        // PoisonP.pas:293
        public new const int ClassID = 131999;

        // PoisonP.pas:298
        public override int ResourceUserClassID() => 131998;

        // PoisonP.pas:288
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.TPoisonPlantResource.PoisonPlantRStr);

        private static readonly string PoisonPlantRStr = "Poison Plant";

    }
}

namespace Aow.Game.Commands
{

    public partial class TPoisonPlantSpellTE : TGlobalTargetSpellTE
    {
        public TPoisonPlantSpellTE(TECustomNode owner) : base(owner)
        { }

        // PoisonP.pas:303
        public new const int ClassID = 135941;

        // PoisonP.pas:361
        public override bool Process(TAbstractUnit source, int value)
        {
            base.Process(source, value);
            CreatePoisonPlant(source);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            return false;
        }

        // PoisonP.pas:308
        public void CreatePoisonPlant(TAbstractUnit Source)
        {
            TMapLevel Level = default;
            THNWalkData Walk = default;
            TPoisonPlantResource Resource = default;
            TAoWMapField MapF = default;
            TPoisonPlant Plant = default;
            TImageSequenceAnimation Animation = default;
            TTwinkleAnimation Twinkle = default;
            ref var walk = ref Walk;
            Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[FLocation.L];
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            Resource = (TPoisonPlantResource)Aow.Game.Engine.TAoWHSSet.AoWHSSet.MapObjectResourceList.FindChild(131999);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && walk.Y < Level.Height)
                {
                    MapF = (TAoWMapField)Level.GetField(walk.X, walk.Y);
                    if (((TGlobalTargetSpell)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(12)).ValidTargetMapF(MapF) && MapF.GetArmyHS() == null)
                    {
                        MapF.RestoreTerrain();
                        Plant = (TPoisonPlant)Resource.CreateResourceUser();
                        Plant.FDaysRemaining = (sbyte)(3 + (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(11) - 5) / 5);
                        Plant.FCasterPlayer = Source.FPlayer;
                        Plant.FOriginalOverlay = MapF.OverlayType;
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Plant, walk.X, walk.Y, FLocation.L))
                        {
                            Plant.FPlantStatus = TSet<TPoisonPlantStatusElement>.Of(TPoisonPlantStatusElement.ppsSummoning);
                            Animation = new TImageSequenceAnimation(null);
                            Animation.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(12).FCastImages.ImageSequences[0]);
                            Animation.FOnShow = Plant.ShowSummonAnimation;
                            Animation.FOnNewFrame = Plant.NewFrameSummonAnimation;
                            Animation.FFrameCounter = -Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(15);
                            Animation.FAnimationLevel = 27;
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Animation, walk.X, walk.Y, FLocation.L);
                            Twinkle = new TTwinkleAnimation(null);
                            Twinkle.Setup(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(2).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(), 2, 6, Math.Abs(Animation.FFrameCounter + 5), Math.Abs(Animation.FFrameCounter + 5) / 2);
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Twinkle, walk.X, walk.Y, FLocation.L);
                        }
                        else
                        {
                            Plant?.Dispose();
                        }
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TPoisonPlantSpell : TGlobalTargetSpell
    {
        public TPoisonPlantSpell(TECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.GlobalTarget.TPoisonPlantSpell.PoisonPlantsRStr);
            FSpellID = 12;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msEarth;
            FAboveGroundOnly = false;
        }

        // PoisonP.pas:371

        // PoisonP.pas:405
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                TargetSelection.FHNs.Add(0);
            }
        }

        // PoisonP.pas:414
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            TPoisonPlantSpellTE Event;
            Event = new TPoisonPlantSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // PoisonP.pas:390
        public override bool ValidTargetSelectionMapF(TMapField Field)
        {
            TArmyHS ArmyHS;
            bool Result = base.ValidTargetSelectionMapF(Field) && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
            // { Original precedence also accepts these overlays without the terrain test. }
            Result = (Result && Field.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
            Result = Result && Field.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)Field).GetArmyHS() == null;
            if (Result)
            {
                ArmyHS = ((TAoWMapField)Field).GetArmyHS();
                if (ArmyHS != null)
                {
                    Result = !ArmyHS.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // PoisonP.pas:382
        public override bool ValidTargetMapF(TMapField Field)
        {
            bool Result = base.ValidTargetMapF(Field) && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
            // { Original precedence also accepts these overlays without the terrain test. }
            Result = (Result && Field.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
            return Result && Field.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)Field).GetArmyHS() == null;
        }

        private static readonly string PoisonPlantsRStr = "Poison Plants";

        // PoisonP.pas:66

        // PoisonP.pas:66

    }
}
