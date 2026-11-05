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
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                var call__1 = GetYhx();
                var call__2 = GetXhx();
                UpdateMapFields((TMapLevel)Owner.Owner, call__2, call__1);
            }

            return Result;
        }

        // PoisonP.pas:246
        public override void RemoveFromMap()
        {
            TMapLevel Level = default;
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
                var call__3 = FResource.TerrainList.GetTerrain(Msg.HN);
                if (call__3 >= 0)
                {
                    ArmyPlaced(Msg.Param[0].As<TArmyHS>());
                }
            }
            else
            {
                if (Msg.MsgID == 4384)
                {
                    var call__4 = FResource.TerrainList.GetTerrain(Msg.HN);
                    bool bool__5 = call__4 >= 0;
                    if (bool__5)
                    {
                        var call__6 = Msg.Param[0].As<TArmyHS>().FArmy.ImmunityTypes(255);
                        bool__5 = !call__6.Contains(TDamageType.dtPoison);
                    }

                    bool bool__7 = bool__5;
                    if (bool__7)
                    {
                        var call__8 = PoisonPlantActive();
                        bool__7 = call__8;
                    }

                    if (bool__7)
                    {
                        Msg.Result = 1;
                    }
                }
                else
                {
                    if (Msg.MsgID == 4402)
                    {
                        // { Original excludes player zero from early restoration. }
                        var call__9 = FResource.TerrainList.GetTerrain(Msg.HN);
                        if (call__9 >= 0 && FCasterPlayer > 0 && FDaysRemaining > 0)
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
            int Result = default;
            var call__10 = FResource.TerrainList.GetTerrain(Info.HN);
            bool bool__11 = call__10 >= 0;
            if (bool__11)
            {
                var call__12 = PoisonPlantActive();
                bool__11 = call__12;
            }

            if (bool__11)
            {
                var call__13 = CanDoDamage(((TMoveControl)Info.Sender).FSettings.FUnits);
                if (!call__13)
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
            var call__14 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__14 >= 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // PoisonP.pas:92
        public bool PoisonPlantActive()
        {
            bool Result = default;
            Result = !TSet<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)FTerrainType);
            return Result;
        }

        // PoisonP.pas:105
        public bool CanDoDamage(TUnitList Units)
        {
            bool Result = default;
            int I = default;
            TAbstractUnit U = default;
            Result = false;
            var call__15 = Units.Count;
            I = call__15;
            while (!Result && I != 0)
            {
                I -= 1;
                U = Units.GetUnit(I);
                var call__16 = U.GetImmunityTypes();
                bool bool__17 = !call__16.Contains(TDamageType.dtPoison);
                if (bool__17)
                {
                    var call__18 = U.GetAbilityEnabled(1);
                    bool__17 = !call__18;
                }

                Result = bool__17;
            }

            return Result;
        }

        // PoisonP.pas:120
        public void TriggerArmyDamage(TArmyHS ArmyHS)
        {
            int I = default;
            int Count = default;
            TUnitList Units = default;
            TArmyEventLog Log = default;
            TAbstractUnit U = default;
            int Damage = default;
            Count = 0;
            var call__19 = AoWHSMap.Random(16777215);
            RandSeed = call__19;
            Log = new TArmyEventLog(null);
            var call__20 = TranslateRStr(PoisonPlantDamageRStr);
            Log.FArmyText = call__20;
            Log.FPlayer = ArmyHS.FArmy.FPlayer;
            var call__21 = TranslateRStr(PoisonPlantDamageRStr);
            Log.FArmyMessages.Add(call__21);
            Log.FArmyIcon = 1;
            Log.FArmyView.Setup(ArmyHS.FArmy, 255);
            Units = ArmyHS.FArmy;
            {
                I = 0;
                int end__22 = Units.Count - 1;
                for (; I <= end__22; I += 1)
                {
                    U = Units.GetUnit(I);
                    U.AddRef();
                    var call__23 = U.GetImmunityTypes();
                    bool bool__24 = !call__23.Contains(TDamageType.dtPoison);
                    if (bool__24)
                    {
                        var call__25 = U.GetAbilityEnabled(1);
                        bool__24 = !call__25;
                    }

                    if (bool__24)
                    {
                        Damage = U.ExecuteDamageRole(4, 3, TSet<TDamageType>.Of(TDamageType.dtPoison));
                        if (Damage > U.GetHitPoints())
                        {
                            Damage = U.GetHitPoints();
                        }

                        if (Damage != 0)
                        {
                            var call__26 = U.GetHitPoints();
                            U.SetHitPoints((sbyte)(call__26 - Damage));
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
            }

            Log.ValidateGameOver();
            if (Count != 0)
            {
                var call__27 = GetYhx();
                var call__28 = GetLhx();
                var call__29 = GetXhx();
                DistributeLocationEventLog(Log, Log.FPlayer, call__29, call__27, call__28, true);
            }

            ArmyHS.FArmy.Validate();
            Log.Release();
        }

        // PoisonP.pas:168
        public void ArmyPlaced(TArmyHS ArmyHS)
        {
            var call__30 = PoisonPlantActive();
            if (call__30 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                TriggerArmyDamage(ArmyHS);
            }
        }

        // PoisonP.pas:174
        public void RestoreTerrain()
        {
            TMapField MapF = default;
            sbyte OriginalOverlay = default;
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
            TArmyHS ArmyHS = default;
            if (PoisonPlantActive())
            {
                var call__31 = Owner.FindOwnedChild(131607);
                ArmyHS = (TArmyHS)call__31;
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
            TMapField MapF = default;
            var call__32 = Animation.GetLhx();
            var call__33 = Animation.GetYhx();
            var call__34 = Animation.GetXhx();
            var call__35 = AoWHSMap.MapContainer.GetField(call__34, call__33, call__32);
            MapF = call__35;
            var call__36 = MapF.FindOwnedChild(131998);
            if (this == call__36 && AoWEngine.FAnimationDetail != 0 && Animation.FFrameCounter == 0)
            {
                bool bool__37 = AoWHSMap.FSeatedPlayer == Animation.FTag;
                if (!bool__37)
                {
                    var call__38 = GetLhx();
                    var call__39 = GetYhx();
                    var call__40 = GetXhx();
                    var call__41 = AoWHSMap.WatchingTerrain(call__40, call__39, call__38, 0);
                    bool__37 = call__41;
                }

                if (bool__37)
                {
                    var call__42 = AoWHSSet.FSpellControl.GetSpell(12);
                    call__42.FCastSounds.Play(0);
                }
            }
        }

        // PoisonP.pas:279
        public void NewFrameSummonAnimation(TAnimation Animation, int FrameCounter)
        {
            TMapField MapF = default;
            var call__43 = Animation.GetLhx();
            var call__44 = Animation.GetYhx();
            var call__45 = Animation.GetXhx();
            var call__46 = AoWHSMap.MapContainer.GetField(call__45, call__44, call__43);
            Animation = (TAnimation)(object)call__46;
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
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131998;
            return Result;
        }

        // PoisonP.pas:288
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(PoisonPlantRStr);
            return Result;
        }

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
            bool Result = default;
            base.Process(A1, A2);
            CreatePoisonPlant(A1);
            AoWHSMap.ValidateTerrainObservation(A1.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
            var call__47 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__47.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
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
            var call__49 = AoWHSMap.MapContainer.MapLevels[FLocation.L];
            Level = call__49;
            InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            var call__50 = AoWHSSet.MapObjectResourceList.FindChild(131999);
            Resource = (TPoisonPlantResource)call__50;
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && walk.Y < Level.Height)
                {
                    var call__51 = Level.GetField(walk.X, walk.Y);
                    MapF = (TAoWMapField)call__51;
                    var call__52 = AoWHSSet.FSpellControl.GetSpell(12);
                    var call__53 = ((TGlobalTargetSpell)call__52).ValidTargetMapF(MapF);
                    bool bool__54 = call__53;
                    if (bool__54)
                    {
                        var call__55 = MapF.GetArmyHS();
                        bool__54 = call__55 == null;
                    }

                    if (bool__54)
                    {
                        MapF.RestoreTerrain();
                        Plant = (TPoisonPlant)Resource.CreateResourceUser();
                        var call__56 = AoWHSMap.Random(11);
                        Plant.FDaysRemaining = (sbyte)(3 + (call__56 - 5) / 5);
                        Plant.FCasterPlayer = Source.FPlayer;
                        Plant.FOriginalOverlay = MapF.OverlayType;
                        AoWHSMap.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                        var call__57 = AoWHSMap.PlaceHX(Plant, walk.X, walk.Y, FLocation.L);
                        if (call__57)
                        {
                            Plant.FPlantStatus = TSet<TPoisonPlantStatusElement>.Of(TPoisonPlantStatusElement.ppsSummoning);
                            Animation = new TImageSequenceAnimation(null);
                            var call__58 = AoWHSSet.FSpellControl.GetSpell(12);
                            var call__59 = call__58.FCastImages.ImageSequences[0];
                            Animation.SetImageSequence(call__59);
                            Animation.FOnShow = Plant.ShowSummonAnimation;
                            Animation.FOnNewFrame = Plant.NewFrameSummonAnimation;
                            var call__60 = AoWHSMap.Random(15);
                            Animation.FFrameCounter = -call__60;
                            Animation.FAnimationLevel = 27;
                            AoWHSMap.PlaceHX(Animation, walk.X, walk.Y, FLocation.L);
                            Twinkle = new TTwinkleAnimation(null);
                            var call__61 = AoWHSSet.FSpellControl.GetSpell(2);
                            var call__62 = call__61.FCastImages.ImageSequences[0];
                            Twinkle.Setup(call__62, TSet<TTwinkleModeElement>.Of(), 2, 6, Math.Abs(Animation.FFrameCounter + 5), Math.Abs(Animation.FFrameCounter + 5) / 2);
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
            var call__63 = TranslateRStr(PoisonPlantsRStr);
            FSpellName = call__63;
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
            int I = default;
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            {
                I = 0;
                int end__64 = RadToHN(2) - 1;
                for (; I <= end__64; I += 1)
                {
                    A2.FHNs.Add(0);
                }
            }
        }

        // PoisonP.pas:414
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TPoisonPlantSpellTE Event = default;
            Event = new TPoisonPlantSpellTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__65 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__65.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // PoisonP.pas:390
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            TArmyHS ArmyHS = default;
            var call__66 = base.ValidTargetSelectionMapF(A1);
            Result = call__66 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence also accepts these overlays without the terrain test. }
            Result = (Result && A1.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            bool bool__67 = Result;
            if (bool__67)
            {
                var call__68 = A1.FindNoneTransparentHS(typeof(TStructure));
                bool__67 = call__68 == null;
            }

            bool bool__69 = bool__67;
            if (bool__69)
            {
                var call__70 = ((TAoWMapField)A1).GetArmyHS();
                bool__69 = call__70 == null;
            }

            Result = bool__69;
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
            bool Result = default;
            var call__71 = base.ValidTargetMapF(A1);
            Result = call__71 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence also accepts these overlays without the terrain test. }
            Result = (Result && A1.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            bool bool__72 = Result;
            if (bool__72)
            {
                var call__73 = A1.FindNoneTransparentHS(typeof(TStructure));
                bool__72 = call__73 == null;
            }

            bool bool__74 = bool__72;
            if (bool__74)
            {
                var call__75 = ((TAoWMapField)A1).GetArmyHS();
                bool__74 = call__75 == null;
            }

            Result = bool__74;
            return Result;
        }

        // PoisonP.pas:66

    }

    public static partial class PoisonP
    {
        // PoisonP.pas:426
        public static void RegisterPoisonPlant(TAoWEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TPoisonPlant>(), EClass.Of<TPoisonPlantResource>(), EClass.Of<TPoisonPlantSpellTE>()]);
            var call__76 = new TPoisonPlantSpell(null);
            AEngine.FGameSet.FSpellControl.RegisterSpell(call__76);
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
