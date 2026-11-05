// Translated from PoisonP.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.ILTer;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.GlobalTargetSpells;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.PoisonP
{
    public enum TPoisonPlantStatusElement : byte
    {
        ppsSummoning = 0
    }

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
                NewTurn((sbyte)Msg.Value);
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
            RandSeed = AoWHSMap.Random(16777215);
            Log = new TArmyEventLog(null);
            Log.FArmyText = TranslateRStr(PoisonPlantDamageRStr);
            Log.FPlayer = ArmyHS.FArmy.FPlayer;
            var poisonPlantDamage = TranslateRStr(PoisonPlantDamageRStr);
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
                DistributeLocationEventLog(Log, Log.FPlayer, xhx, yhx, lhx, true);
            }

            ArmyHS.FArmy.Validate();
            Log.Release();
        }

        // PoisonP.pas:168
        public void ArmyPlaced(TArmyHS ArmyHS)
        {
            if (PoisonPlantActive() && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
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
            Destroy();
            if (OriginalOverlay > 0)
            {
                AoWHSMap.PlaceTerrain(MapF.Xhx, MapF.Yhx, MapF.Lhx, MapF.TerrainType, OriginalOverlay);
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
            MapF = AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
            if (this == MapF.FindOwnedChild(131998) && AoWEngine.FAnimationDetail != 0 && Animation.FFrameCounter == 0)
            {
                bool condition = AoWHSMap.FSeatedPlayer == Animation.FTag;
                if (!condition)
                {
                    var lhx2 = GetLhx();
                    var yhx2 = GetYhx();
                    var xhx2 = GetXhx();
                    condition = AoWHSMap.WatchingTerrain(xhx2, yhx2, lhx2, 0);
                }

                if (condition)
                {
                    AoWHSSet.FSpellControl.GetSpell(12).FCastSounds.Play(0);
                }
            }
        }

        // PoisonP.pas:279
        public void NewFrameSummonAnimation(TAnimation Animation, int FrameCounter)
        {
            var lhx = Animation.GetLhx();
            var yhx = Animation.GetYhx();
            var xhx = Animation.GetXhx();
            Animation = (TAnimation)(object)AoWHSMap.MapContainer.GetField(xhx, yhx, lhx);
            if (this == ((TMapField)(object)Animation).FindOwnedChild(131998) && FrameCounter == 9)
            {
                FPlantStatus = FPlantStatus - TSet<TPoisonPlantStatusElement>.Of(TPoisonPlantStatusElement.ppsSummoning);
            }
        }

        // PoisonP.pas:28


    }

    public partial class TPoisonPlantResource : TILTerrainMOR
    {
        public TPoisonPlantResource(TECustomNode owner) : base(owner)
        {}

        // PoisonP.pas:293
        public new const int ClassID = 131999;

        // PoisonP.pas:298
        public override int ResourceUserClassID() => 131998;

        // PoisonP.pas:288
        public override AnsiString ResourceEditName() => TranslateRStr(PoisonPlantRStr);

    }

    public partial class TPoisonPlantSpellTE : TGlobalTargetSpellTE
    {
        public TPoisonPlantSpellTE(TECustomNode owner) : base(owner)
        {}

        // PoisonP.pas:303
        public new const int ClassID = 135941;

        // PoisonP.pas:361
        public override bool Process(TAbstractUnit A1, int A2)
        {
            base.Process(A1, A2);
            CreatePoisonPlant(A1);
            AoWHSMap.ValidateTerrainObservation(A1.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
            AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(A1, FEventMana);
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
            Level = AoWHSMap.MapContainer.MapLevels[FLocation.L];
            InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            Resource = (TPoisonPlantResource)AoWHSSet.MapObjectResourceList.FindChild(131999);
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && walk.Y < Level.Height)
                {
                    MapF = (TAoWMapField)Level.GetField(walk.X, walk.Y);
                    if (((TGlobalTargetSpell)AoWHSSet.FSpellControl.GetSpell(12)).ValidTargetMapF(MapF) && MapF.GetArmyHS() == null)
                    {
                        MapF.RestoreTerrain();
                        Plant = (TPoisonPlant)Resource.CreateResourceUser();
                        Plant.FDaysRemaining = (sbyte)(3 + (AoWHSMap.Random(11) - 5) / 5);
                        Plant.FCasterPlayer = Source.FPlayer;
                        Plant.FOriginalOverlay = MapF.OverlayType;
                        AoWHSMap.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                        if (AoWHSMap.PlaceHX(Plant, walk.X, walk.Y, FLocation.L))
                        {
                            Plant.FPlantStatus = TSet<TPoisonPlantStatusElement>.Of(TPoisonPlantStatusElement.ppsSummoning);
                            Animation = new TImageSequenceAnimation(null);
                            Animation.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(12).FCastImages.ImageSequences[0]);
                            Animation.FOnShow = Plant.ShowSummonAnimation;
                            Animation.FOnNewFrame = Plant.NewFrameSummonAnimation;
                            Animation.FFrameCounter = -AoWHSMap.Random(15);
                            Animation.FAnimationLevel = 27;
                            AoWHSMap.PlaceHX(Animation, walk.X, walk.Y, FLocation.L);
                            Twinkle = new TTwinkleAnimation(null);
                            Twinkle.Setup(AoWHSSet.FSpellControl.GetSpell(2).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(), 2, 6, Math.Abs(Animation.FFrameCounter + 5), Math.Abs(Animation.FFrameCounter + 5) / 2);
                            AoWHSMap.PlaceHX(Twinkle, walk.X, walk.Y, FLocation.L);
                        }
                        else
                        {
                            Plant.Free();
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230


    }

    public partial class TPoisonPlantSpell : TGlobalTargetSpell
    {
        public TPoisonPlantSpell(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(PoisonPlantsRStr);
            FSpellID = 12;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msEarth;
            FAboveGroundOnly = false;
        }

        // PoisonP.pas:371


        // PoisonP.pas:405
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            for (int I = 0, last = RadToHN(2) - 1; I <= last; I++)
            {
                A2.FHNs.Add(0);
            }
        }

        // PoisonP.pas:414
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            TPoisonPlantSpellTE Event;
            Event = new TPoisonPlantSpellTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // PoisonP.pas:390
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            TArmyHS ArmyHS;
            bool Result = base.ValidTargetSelectionMapF(A1) && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence also accepts these overlays without the terrain test. }
            Result = (Result && A1.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            Result = Result && A1.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)A1).GetArmyHS() == null;
            if (Result)
            {
                ArmyHS = ((TAoWMapField)A1).GetArmyHS();
                if (ArmyHS != null)
                {
                    Result = !ArmyHS.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // PoisonP.pas:382
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = base.ValidTargetMapF(A1) && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence also accepts these overlays without the terrain test. }
            Result = (Result && A1.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            return Result && A1.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)A1).GetArmyHS() == null;
        }

        // PoisonP.pas:66

    }

    public static partial class PoisonP
    {
        // PoisonP.pas:426
        public static void RegisterPoisonPlant(TAoWEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TPoisonPlant>(), EClass.Of<TPoisonPlantResource>(), EClass.Of<TPoisonPlantSpellTE>()]);
            var poisonPlantSpell = new TPoisonPlantSpell(null);
            AEngine.FGameSet.FSpellControl.RegisterSpell(poisonPlantSpell);
        }

        // PoisonP.pas:432
        public static void UnRegisterPoisonPlant(TAoWEngine AEngine)
        {
            AEngine.UnregisterEClasses([EClass.Of<TPoisonPlant>(), EClass.Of<TPoisonPlantResource>(), EClass.Of<TPoisonPlantSpellTE>()]);
            AEngine.FGameSet.FSpellControl.UnregisterSpellID(12);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
