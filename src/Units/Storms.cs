// Translated from Storms.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.PoisonC;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Storms
{
    // player
    public partial class TStorm : TAnimation
    {
        public TStorm(TECustomNode owner) : base(owner)
        {}

        public sbyte FPlayer;
        // Storms.pas:34
        public virtual void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref TStormDamageInfo Info)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Storms.pas:236
        public virtual void UpdateTargetSelection(TTargetSelectionControl Selection)
        {
            Selection.FOnShow = ShowTargetSelection;
            Selection.FTargetMode = Selection.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmOwnerDraw);
        }

        // Storms.pas:209
        public virtual void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            FPlayer = Player;
            FAnimationLevel = 28;
            AoWHSMap.LockExecuteEventLog();
        }

        // Storms.pas:216
        public virtual bool UpdateStorm()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // Storms.pas:221
        public virtual void FinalizeStorm()
        {
            AoWHSMap.UnlockExecuteEventLog();
        }

        // Storms.pas:226
        public void ShowTargetSelection(TTargetSelectionControl Selection, int HN, int Xs, int Ys, in TSurfaceDesc SurfaceDesc)
        {
            int Intensity = default;
            HN = HNtoRad(HN);
            Intensity = Math.Abs((AoWHSMap.Frame + HN) % 9 - 4) * 20 + 20;
            var call__1 = IntensityBlendTable.GetLookupTables(Intensity);
            var call__2 = Selection.FImageLibrary[0];
            call__2.ShowBlended(Xs - 8, Ys, SurfaceDesc, call__1);
        }

        // Storms.pas:242
        public bool GetBestTarget(sbyte Player, int X, int Y, int L, int Range, ref TXYL Target, ref TStormDamageInfo Info)
        {
            bool Result = default;
            TStormDamageInfo Candidate = default;
            THNWalkData Walk = default;
            TMapLevel Level = default;
            TArmyHS ArmyHS = default;
            var call__3 = AoWHSMap.MapContainer.MapLevels[L];
            Level = call__3;
            Target.XYL = 16777215;
            Result = false;
            Info = default;
            Candidate = default;
            InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Range)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__4 = Level.Field[Walk.X, Walk.Y];
                    var call__5 = ((TAoWMapField)call__4).GetArmyHS();
                    ArmyHS = call__5;
                    bool bool__6 = ArmyHS != null && ArmyHS.FArmy.FPlayer != 0;
                    if (bool__6)
                    {
                        var call__7 = ValidAttackRelation(Player, 0, ArmyHS.FArmy.FPlayer, 0);
                        bool__6 = call__7;
                    }

                    if (bool__6)
                    {
                        GetTargetDamageInfo(Player, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L, ref Candidate);
                        if (Info.EnemyDamage - Info.FriendlyDamage < Candidate.EnemyDamage - Candidate.FriendlyDamage)
                        {
                            Info = Candidate;
                            var call__8 = PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                            Target = call__8;
                            Result = true;
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // Storms.pas:39
        public virtual AnsiString Name()
        {
            throw EAbstractError.Create("Abstract Error");
        }


    }

    // storm image library
    public partial class TBlastStorm : TStorm
    {
        public TBlastStorm(TECustomNode owner) : base(owner)
        {}

        public TCustomImageLibrary FStormImages;
        // Storms.pas:338
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            Frame = FFrameCounter % FStormImages.Count;
            var call__9 = GetYhx();
            var call__10 = GetXhx();
            HXtoHP(call__10, call__9, ref X, ref Y);
            var call__11 = FStormImages[Frame];
            X = X - (int)(((uint)call__11.Width) >> 1) + 10;
            var call__12 = FStormImages[Frame];
            Y = Y - (int)(((uint)call__12.Height) >> 1) - 42;
            var call__13 = FStormImages[Frame];
            call__13.Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
        }

        // Storms.pas:358
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref TStormDamageInfo Info)
        {
            var call__14 = StormType();
            RetrieveStormDamageInfo(Player, call__14, X, Y, L, 0, 4, ref Info);
        }

        // Storms.pas:349
        public override void UpdateTargetSelection(TTargetSelectionControl Selection)
        {
            int I = default;
            base.UpdateTargetSelection(Selection);
            Selection.FHNs.Clear();
            {
                I = 0;
                int end__15 = RadToHN(5) - 1;
                for (; I <= end__15; I += 1)
                {
                    Selection.FHNs.Add(0);
                }
            }
        }

        // Storms.pas:276
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            TRect R = default;
            base.InitializeStorm(Player, X, Y, L);
            var call__16 = StormFileName();
            var call__17 = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), call__16);
            FStormImages = call__17;
            if (GetVisible())
            {
                FStormImages.LoadDiscardableImageData();
            }

            var call__18 = FStormImages[0];
            R.Left = -(int)(((uint)call__18.Width) >> 1);
            var call__19 = FStormImages[0];
            R.Right = (int)(((uint)call__19.Width) >> 1);
            var call__20 = FStormImages[0];
            R.Top = -(int)(((uint)call__20.Height) >> 1);
            var call__21 = FStormImages[0];
            R.Bottom = (int)(((uint)call__21.Height) >> 1);
            SetDimensions(R);
            AoWHSMap.ValidateTerrainObservation(AoWHSMap.FCurrentPlayer, X, Y, L, 4);
            bool bool__22 = AoWHSMap.FCurrentPlayer == AoWHSMap.FSeatedPlayer;
            if (!bool__22)
            {
                var call__23 = AoWHSMap.TerrainVisible(X, Y, L, 4);
                bool__22 = call__23;
            }

            if (bool__22)
            {
                PlaySample((AnsiString)"SFX\\ATTACK\\STORMBIG.WAV");
            }

            PlaceHX(AoWHSMap.MapContainer, X, Y, L);
        }

        // Storms.pas:312
        public override bool UpdateStorm()
        {
            bool Result = default;
            NewFrame();
            if (FFrameCounter == 8)
            {
                var call__24 = GetLhx();
                var call__25 = GetYhx();
                var call__26 = GetXhx();
                AoWHSMap.ChangeTerrainEx(null, call__26, call__25, call__24, 1, ChangeStormTerrain, StormTerrainChanged);
            }
            else
            {
                if (FFrameCounter == 12)
                {
                    var call__27 = GetLhx();
                    var call__28 = GetYhx();
                    var call__29 = GetXhx();
                    AoWHSMap.ChangeTerrainEx(null, call__29, call__28, call__27, 2, ChangeStormTerrain, StormTerrainChanged);
                }
                else
                {
                    if (FFrameCounter == 18)
                    {
                        var call__30 = GetLhx();
                        var call__31 = GetYhx();
                        var call__32 = GetXhx();
                        AoWHSMap.ChangeTerrainEx(null, call__32, call__31, call__30, 3, ChangeStormTerrain, StormTerrainChanged);
                    }
                    else
                    {
                        if (FFrameCounter == 22)
                        {
                            var call__33 = GetLhx();
                            var call__34 = GetYhx();
                            var call__35 = GetXhx();
                            AoWHSMap.ChangeTerrainEx(null, call__35, call__34, call__33, 4, ChangeStormTerrain, StormTerrainChanged);
                        }
                    }
                }
            }

            if (FStormImages.Count == FFrameCounter)
            {
                var call__36 = GetLhx();
                var call__37 = GetYhx();
                var call__38 = GetXhx();
                var call__39 = StormType();
                TriggerStormDamage(call__39, call__38, call__37, call__36, 0, 4, AoWHSMap.FCurrentPlayer);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // Storms.pas:331
        public override void FinalizeStorm()
        {
            FStormImages.ReleaseDiscardableImageData();
            var call__40 = StormFileName();
            AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), call__40);
            base.FinalizeStorm();
        }

        // Storms.pas:304
        public virtual void ChangeStormTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
        }

        // Storms.pas:308
        public virtual void StormTerrainChanged(TMapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
        }

        // Storms.pas:294
        public virtual AnsiString StormFileName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            return Result;
        }

        // Storms.pas:299
        public virtual TStormType StormType()
        {
            TStormType Result = default;
            Result = TStormType.stNone;
            return Result;
        }
    }

    public partial class TIceStorm : TBlastStorm
    {
        public TIceStorm(TECustomNode owner) : base(owner)
        {}

        // Storms.pas:363
        public new const int ClassID = 135738;

        // Storms.pas:390
        public override void ChangeStormTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            TFrozenWaterHS Ice = default;
            // { Original sends terrain 13 to the default branch before the ice refresh test. }
            if (TerrainType == 0)
            {
                TerrainType = 6;
            }
            else
            {
                if (TerrainType != 6)
                {
                    TerrainType = 3;
                }
                else
                {
                    if (TSet<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)TerrainType))
                    {
                        Ice = (TFrozenWaterHS)MapF.FindOwnedHS(typeof(TFrozenWaterHS));
                        if (Ice != null)
                        {
                            var call__41 = AoWHSMap.Random(11);
                            Ice.FDaysRemaining = (sbyte)(3 + (call__41 - 5) / 5);
                            Ice.FCasterPlayer = FPlayer;
                        }
                    }
                }
            }
        }

        // Storms.pas:373
        public override void StormTerrainChanged(TMapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
            TFrozenWaterHS Ice = default;
            if (TSet<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)OriginalTerrainType))
            {
                Ice = (TFrozenWaterHS)MapF.FindOwnedHS(typeof(TFrozenWaterHS));
                if (Ice == null)
                {
                    Ice = new TFrozenWaterHS(null);
                    AoWHSMap.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                }

                var call__42 = AoWHSMap.Random(11);
                Ice.FDaysRemaining = (sbyte)(3 + (call__42 - 5) / 5);
                Ice.FCasterPlayer = FPlayer;
            }
        }

        // Storms.pas:409
        public override AnsiString StormFileName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"EFFECTS\\STORM1.ILB";
            return Result;
        }

        // Storms.pas:414
        public override TStormType StormType()
        {
            TStormType Result = default;
            Result = TStormType.stIce;
            return Result;
        }

        // Animation.pas:43


        // Storms.pas:368
        public override AnsiString Name()
        {
            AnsiString Result = default;
            Result = TranslateRStr(IceStormRStr);
            return Result;
        }

    }

    public partial class TDeathStorm : TBlastStorm
    {
        public TDeathStorm(TECustomNode owner) : base(owner)
        {}

        // Storms.pas:492
        public new const int ClassID = 135741;

        // Storms.pas:502
        public override void ChangeStormTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (TerrainType != 0 && TerrainType != 6)
            {
                TerrainType = 5;
            }
        }

        // Storms.pas:507
        public override AnsiString StormFileName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"EFFECTS\\STORMD.ILB";
            return Result;
        }

        // Storms.pas:512
        public override TStormType StormType()
        {
            TStormType Result = default;
            Result = TStormType.stDeath;
            return Result;
        }

        // Animation.pas:43


        // Storms.pas:497
        public override AnsiString Name()
        {
            AnsiString Result = default;
            Result = TranslateRStr(DeathStormRStr);
            return Result;
        }

    }

    public partial class TDivineStorm : TBlastStorm
    {
        public TDivineStorm(TECustomNode owner) : base(owner)
        {}

        // Storms.pas:608
        public new const int ClassID = 135740;

        // Storms.pas:618
        public override void ChangeStormTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (TerrainType != 6 && TerrainType != 0)
            {
                TerrainType = 1;
            }
        }

        // Storms.pas:624
        public override AnsiString StormFileName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"EFFECTS\\STORMLGT.ILB";
            return Result;
        }

        // Storms.pas:629
        public override TStormType StormType()
        {
            TStormType Result = default;
            Result = TStormType.stHoly;
            return Result;
        }

        // Animation.pas:43


        // Storms.pas:613
        public override AnsiString Name()
        {
            AnsiString Result = default;
            Result = TranslateRStr(DivineStormRStr);
            return Result;
        }

    }

    // storm image library
    public partial class TLightningStorm : TStorm
    {
        public TLightningStorm(TECustomNode owner) : base(owner)
        {}

        public new const int ClassID = 135739;

        public TCustomImageLibrary FStormImages;
        // Storms.pas:429
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            Frame = FFrameCounter % FStormImages.Count;
            var call__43 = FStormImages[Frame];
            if (call__43 != null)
            {
                var call__44 = GetYhx();
                var call__45 = GetXhx();
                HXtoHP(call__45, call__44, ref X, ref Y);
                var call__46 = FStormImages[Frame];
                X = X - (int)(((uint)call__46.Width) >> 1) + 20;
                var call__47 = FStormImages[Frame];
                Y = Y - call__47.Height + 32 + 40;
                var call__48 = FStormImages[Frame];
                call__48.Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
            }
        }

        // Storms.pas:487
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref TStormDamageInfo Info)
        {
            RetrieveStormDamageInfo(Player, TStormType.stLightning, X, Y, L, 0, 1, ref Info);
        }

        // Storms.pas:478
        public override void UpdateTargetSelection(TTargetSelectionControl Selection)
        {
            int I = default;
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection, without clearing its existing hexes. }
            {
                I = 0;
                int end__49 = RadToHN(2) - 1;
                for (; I <= end__49; I += 1)
                {
                    AoWHSMap.FTargetSelection.FHNs.Add(0);
                }
            }
        }

        // Storms.pas:443
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            TRect R = default;
            base.InitializeStorm(Player, X, Y, L);
            var call__50 = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), (AnsiString)"EFFECTS\\LIGHTN.ILB");
            FStormImages = call__50;
            if (GetVisible())
            {
                FStormImages.LoadDiscardableImageData();
            }

            var call__51 = FStormImages[0];
            R.Left = -(int)(((uint)call__51.Width) >> 1);
            var call__52 = FStormImages[0];
            R.Right = (int)(((uint)call__52.Width) >> 1);
            var call__53 = FStormImages[0];
            R.Top = -call__53.Height + 32;
            R.Bottom = 32;
            SetDimensions(R);
            AoWHSMap.ValidateTerrainObservation(AoWHSMap.FCurrentPlayer, X, Y, L, 2);
            PlaceHX(AoWHSMap.MapContainer, X, Y, L);
        }

        // Storms.pas:466
        public override bool UpdateStorm()
        {
            bool Result = default;
            NewFrame();
            var call__54 = FStormImages.GetSafe(FFrameCounter);
            if (call__54 != null)
            {
                PlaySample((AnsiString)"SFX\\ATTACK\\LIGHTNG.WAV");
            }

            if (FStormImages.Count == FFrameCounter)
            {
                var call__55 = GetLhx();
                var call__56 = GetYhx();
                var call__57 = GetXhx();
                TriggerStormDamage(TStormType.stLightning, call__57, call__56, call__55, 0, 1, AoWHSMap.FCurrentPlayer);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // Storms.pas:459
        public override void FinalizeStorm()
        {
            FStormImages.ReleaseDiscardableImageData();
            AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), (AnsiString)"EFFECTS\\LIGHTN.ILB");
            base.FinalizeStorm();
        }

        // Animation.pas:43


        // Storms.pas:424
        public override AnsiString Name()
        {
            AnsiString Result = default;
            Result = TranslateRStr(LightningStormRStr);
            return Result;
        }

    }

    public partial class TFireStorm : TStorm
    {
        public TFireStorm(TECustomNode owner) : base(owner)
        {}

        // Storms.pas:517
        public new const int ClassID = 135742;

        // Storms.pas:527
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            TImageSequence Sequence = default;
            var call__58 = GetYhx();
            var call__59 = GetXhx();
            HXtoHP(call__59, call__58, ref X, ref Y);
            X = X - XhpSurf + 8;
            Y = Y - YhpSurf;
            var call__60 = AoWHSSet.FImageSequences.ImageSequences[0];
            Sequence = call__60;
            while (Sequence != null)
            {
                var call__61 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Frame = call__61;
                bool bool__62 = Frame != -1;
                if (bool__62)
                {
                    var call__63 = Sequence.ImageLibrary.ImageExist(Frame);
                    bool__62 = call__63;
                }

                if (bool__62)
                {
                    var call__64 = Sequence.ImageLibrary[Frame];
                    call__64.Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        // Storms.pas:603
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref TStormDamageInfo Info)
        {
            RetrieveStormDamageInfo(Player, TStormType.stFire, X, Y, L, 0, 2, ref Info);
        }

        // Storms.pas:594
        public override void UpdateTargetSelection(TTargetSelectionControl Selection)
        {
            int I = default;
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection, without clearing its existing hexes. }
            {
                I = 0;
                int end__65 = RadToHN(3) - 1;
                for (; I <= end__65; I += 1)
                {
                    AoWHSMap.FTargetSelection.FHNs.Add(0);
                }
            }
        }

        // Storms.pas:545
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            TRect R = default;
            base.InitializeStorm(Player, X, Y, L);
            R.Left = -80;
            R.Right = 96;
            R.Top = -80;
            R.Bottom = 96;
            SetDimensions(R);
            AoWHSMap.ValidateTerrainObservation(AoWHSMap.FCurrentPlayer, X, Y, L, 2);
            PlaceHX(AoWHSMap.MapContainer, X, Y, L);
        }

        // Storms.pas:569
        public override bool UpdateStorm()
        {
            bool Result = default;
            TImageSequence Sequence = default;
            int Frame = default;
            NewFrame();
            if (FFrameCounter == 20)
            {
                var call__66 = GetLhx();
                var call__67 = GetYhx();
                var call__68 = GetXhx();
                TriggerStormDamage(TStormType.stFire, call__68, call__67, call__66, 0, 2, AoWHSMap.FCurrentPlayer);
                var call__69 = GetLhx();
                var call__70 = GetYhx();
                var call__71 = GetXhx();
                AoWHSMap.ChangeTerrainEx(this, call__71, call__70, call__69, 2, ChangeStormTerrain, null);
            }

            var call__72 = AoWHSSet.FImageSequences.ImageSequences[0];
            Sequence = call__72;
            Frame = -1;
            while (Sequence != null && Frame == -1)
            {
                var call__73 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Frame = call__73;
                Sequence = Sequence.Next;
            }

            if (Frame == 1)
            {
                bool bool__74 = AoWHSMap.FCurrentPlayer == AoWHSMap.FSeatedPlayer;
                if (!bool__74)
                {
                    var call__75 = GetLhx();
                    var call__76 = GetYhx();
                    var call__77 = GetXhx();
                    var call__78 = AoWHSMap.TerrainVisible(call__77, call__76, call__75, 2);
                    bool__74 = call__78;
                }

                if (bool__74)
                {
                    var call__79 = Random(2);
                    PlaySample(ExplosionSFX[call__79]);
                }
            }

            if (Frame == -1)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // Storms.pas:559
        public override void FinalizeStorm()
        {
            base.FinalizeStorm();
        }

        // Storms.pas:564
        public void ChangeStormTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (TerrainType == 6)
            {
                TerrainType = 0;
            }
        }

        // Animation.pas:43


        // Storms.pas:522
        public override AnsiString Name()
        {
            AnsiString Result = default;
            Result = TranslateRStr(FireStormRStr);
            return Result;
        }

    }

    // poison cloud resource
    public partial class TPestilenceStorm : TStorm
    {
        public TPestilenceStorm(TECustomNode owner) : base(owner)
        {}

        public new const int ClassID = 135745;

        public TPoisonCloudResource FCloudResource;
        // Storms.pas:644
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            int Blend = default;
            TImageSequence Sequence = default;
            var call__80 = GetYhx();
            var call__81 = GetXhx();
            HXtoHP(call__81, call__80, ref X, ref Y);
            X = X - XhpSurf + 8;
            Y = Y - YhpSurf;
            var call__82 = FCloudResource.ImageSequenceList.ImageSequences[0];
            Sequence = call__82;
            while (Sequence != null)
            {
                var call__83 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Frame = call__83;
                bool bool__84 = Frame != -1;
                if (bool__84)
                {
                    var call__85 = Sequence.ImageLibrary.ImageExist(Frame);
                    bool__84 = call__85;
                }

                if (bool__84)
                {
                    Blend = Sequence.GetTotalSequenceFrames();
                    if (Blend != 0)
                    {
                        Blend = FFrameCounter * 100 / Blend;
                    }
                    else
                    {
                        Blend = 100;
                    }

                    var call__86 = Sequence.ImageLibrary[Frame];
                    call__86.ShowBlended(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc, ShadowBlendTable.LookupTables[Blend]);
                }

                Sequence = Sequence.Next;
            }
        }

        // Storms.pas:725
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref TStormDamageInfo Info)
        {
            RetrieveStormDamageInfo(Player, TStormType.stPestilence, X, Y, L, 0, 2, ref Info);
        }

        // Storms.pas:716
        public override void UpdateTargetSelection(TTargetSelectionControl Selection)
        {
            int I = default;
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection without clearing its existing hexes. }
            {
                I = 0;
                int end__87 = RadToHN(3) - 1;
                for (; I <= end__87; I += 1)
                {
                    AoWHSMap.FTargetSelection.FHNs.Add(0);
                }
            }
        }

        // Storms.pas:668
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            TRect R = default;
            var call__88 = AoWHSSet.MapObjectResourceList.FindChild(135744);
            FCloudResource = (TPoisonCloudResource)call__88;
            base.InitializeStorm(Player, X, Y, L);
            R.Left = -80;
            R.Right = 96;
            R.Top = -80;
            R.Bottom = 96;
            SetDimensions(R);
            AoWHSMap.ValidateTerrainObservation(AoWHSMap.FCurrentPlayer, X, Y, L, 2);
            bool bool__89 = AoWHSMap.FCurrentPlayer == AoWHSMap.FSeatedPlayer;
            if (!bool__89)
            {
                var call__90 = AoWHSMap.TerrainVisible(X, Y, L, 2);
                bool__89 = call__90;
            }

            if (bool__89)
            {
                var call__91 = AoWHSSet.FSpellControl.GetSpell(9);
                call__91.FCastSounds.Play(0);
            }

            PlaceHX(AoWHSMap.MapContainer, X, Y, L);
        }

        // Storms.pas:690
        public override bool UpdateStorm()
        {
            bool Result = default;
            TImageSequence Sequence = default;
            int Frame = default;
            TPoisonCloud Cloud = default;
            NewFrame();
            var call__92 = FCloudResource.ImageSequenceList.ImageSequences[0];
            Sequence = call__92;
            Frame = -1;
            while (Sequence != null && Frame == -1)
            {
                var call__93 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Frame = call__93;
                Sequence = Sequence.Next;
            }

            if (Frame == -1)
            {
                var call__94 = FCloudResource.CreateResourceUser();
                Cloud = (TPoisonCloud)call__94;
                Cloud.FTurnsRemaining = 3;
                Cloud.FPlayer = AoWHSMap.FCurrentPlayer;
                var call__95 = GetLhx();
                var call__96 = GetYhx();
                var call__97 = GetXhx();
                var call__98 = AoWHSMap.PlaceHX(Cloud, call__97, call__96, call__95);
                if (call__98)
                {
                    Cloud.TriggerDamage();
                }
                else
                {
                    Cloud.Free();
                }

                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // Storms.pas:685
        public override void FinalizeStorm()
        {
            base.FinalizeStorm();
        }

        // Animation.pas:43


        // Storms.pas:639
        public override AnsiString Name()
        {
            AnsiString Result = default;
            Result = TranslateRStr(PestilenceStormRStr);
            return Result;
        }

    }

    // storm, X, Y, layer
    public partial class TStormSpellTE : TSpellTE
    {
        public TStormSpellTE(TECustomNode owner) : base(owner)
        {}

        // Storms.pas:730
        public new const int ClassID = 135937;

        public TStorm FStorm;
        public sbyte FX;
        public sbyte FY;
        public sbyte FL;
        // Storms.pas:735
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FX), 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FY), 21);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FL), 22);
        }

        // Storms.pas:743
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            if (A2 == 0)
            {
                var call__99 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__100 = ((TStormSpell)call__99).CreateStorm();
                FStorm = call__100;
                FStorm.InitializeStorm(A1.FPlayer, FX, FY, FL);
                var call__101 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__101.CastingDone(A1, FEventMana);
            }

            var call__102 = FStorm.UpdateStorm();
            if (!call__102)
            {
                FStorm.FinalizeStorm();
                FStorm.Free();
                FStorm = null;
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // NetworkE.pas:230


    }

    // targeting storm, caster
    public partial class TStormSpell : TSpell
    {
        public TStorm FTargetStorm;
        public TAbstractUnit FSource;
        // Storms.pas:762
        public TStormSpell(TECustomNode owner) : base(owner)
        {
            FSpellType = TSpellType.stGlobal;
        }

        // Storms.pas:888
        public override void AIPrefetchCastSpellActions(TAbstractUnit A1, TAICastSpellActionList A2)
        {
            int Priority = default;
            TAICastSpellAction Action = default;
            var call__103 = AIRequestCastSpellExpense(A1, FBaseManaCost, 0, 0);
            if (call__103)
            {
                Priority = AICastSpellPriority(A1);
                if (Priority != -1)
                {
                    Action = CreateCastSpellAction();
                    SetupCastSpellAction(A1, Action);
                    Action.FPriority = Priority;
                    Action.SetOwner(A2);
                }
            }
        }

        // Storms.pas:906
        public override bool AIExecuteCastSpellAction(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TXYL Target = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TStorm Storm = default;
            TStormDamageInfo Info = default;
            if (AICastSpellPriority(A1) != -1)
            {
                AICastSpellExpenseMade(A1, FBaseManaCost, FManaUpkeep, 0);
                var call__104 = ((THero)A1).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason);
                if (call__104)
                {
                    A1.GetLocation(ref X, ref Y, ref L);
                    Storm = CreateStorm();
                    var call__105 = Storm.GetBestTarget(A1.FPlayer, X, Y, L, 12, ref Target, ref Info);
                    if (call__105)
                    {
                        ExecuteStormSpell(A1, Target.X, Target.Y, Target.L);
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }

                    Storm.Free();
                }
                else
                {
                    if (((THero)A1).FCastingSpellID != FSpellID)
                    {
                        ((THero)A1).ExecuteStartCasting(FSpellID);
                        Result = true;
                    }
                    else
                    {
                        Result = true;
                    }
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Storms.pas:809
        public override bool CanActivate(TAbstractUnit A1, ref AnsiString A2)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.CanActivate(A1, ref A2);
            if (Result)
            {
                A1.GetLocation(ref X, ref Y, ref L);
                if (L != 0)
                {
                    Result = false;
                    A2 = TranslateRStr(CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // Storms.pas:825
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            AnsiString Reason = default;
            TTargetSelectionControl Selection = default;
            base.Activate(A1);
            if (CanActivate(A1, ref Reason))
            {
                FSource = A1;
                A1.GetLocation(ref X, ref Y, ref L);
                Selection = AoWHSMap.FTargetSelection;
                var call__106 = CreateStorm();
                FTargetStorm = call__106;
                Selection.Initialize(this);
                Selection.FTargetMode = Selection.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmValidateDistance, TTargetSelectionModeElement.tmExternalFloater);
                Selection.FSourceX = X;
                Selection.FSourceY = Y;
                Selection.FSourceL = L;
                AoWHSMap.MakeVisible(X, Y, L, 5);
                var call__107 = FTargetStorm.Name();
                var call__108 = TranslateRStr(SelectTargetLocationForXOnMapRStr);
                var call__109 = Rtl.Rtl.Format(call__108, [call__107]);
                Selection.FText.Text = call__109;
                Selection.FTitle = FSpellName;
                var call__110 = TranslateRStr(CancelRStr);
                Selection.FCancelText = call__110;
                Selection.FRange = 12;
                Selection.FOnChange.Add(OnTargetSelectionChange);
                var call__111 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__112 = call__111.FCastImages.ImageSequences[20];
                var call__113 = call__112.GetPreviewImage();
                Selection.FSourceImage = call__113;
                var call__114 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__115 = call__114.FCastImages.ImageSequences[21];
                var call__116 = call__115.GetPreviewImage();
                Selection.FValidTargetImage = call__116;
                var call__117 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__118 = call__117.FCastImages.ImageSequences[22];
                var call__119 = call__118.GetPreviewImage();
                Selection.FInvalidTargetImage = call__119;
                Selection.Activate();
                FTargetStorm.UpdateTargetSelection(AoWHSMap.FTargetSelection);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Storms.pas:156
        public virtual TStorm CreateStorm()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Storms.pas:768
        public virtual bool ExecuteStormSpell(TAbstractUnit Source, sbyte X, sbyte Y, sbyte L)
        {
            bool Result = default;
            TStormSpellTE TE = default;
            var call__120 = AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
            var call__121 = call__120.GetBusy();
            if (!call__121)
            {
                TE = new TStormSpellTE(null);
                TE.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                TE.FX = X;
                TE.FY = Y;
                TE.FL = L;
                var call__122 = AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer);
                call__122.AddEvent(TE);
                TE.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Storms.pas:860
        public virtual int AICastSpellPriority(TAbstractUnit Source)
        {
            int Result = default;
            TXYL Target = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            AnsiString Reason = default;
            TStorm Storm = default;
            TStormDamageInfo Info = default;
            int Damage = default;
            if (CanActivate(Source, ref Reason))
            {
                Result = -1;
                Source.GetLocation(ref X, ref Y, ref L);
                if (L == 0)
                {
                    Storm = CreateStorm();
                    var call__123 = Storm.GetBestTarget(Source.FPlayer, X, Y, L, 12, ref Target, ref Info);
                    if (call__123)
                    {
                        Damage = Info.EnemyDamage - Info.FriendlyDamage;
                        if ((double)((double)FBaseManaCost * 0.75) < Damage)
                        {
                            Result = Damage * 5 + 750;
                        }
                    }

                    Storm.Free();
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Storms.pas:786
        public void OnTargetSelectionChange(TObject Sender)
        {
            TTargetSelectionControl Selection = default;
            Selection = (TTargetSelectionControl)Sender;
            switch (Selection.FChange)
            {
                case TTargetSelectionChange.tscDeactivated:
                    FTargetStorm.Free();
                    FTargetStorm = null;
                    Selection.FOnChange.Remove(OnTargetSelectionChange);
                    break;
                case TTargetSelectionChange.tscSelected:
                    Selection.FOnChange.Remove(OnTargetSelectionChange);
                    var call__124 = ExecuteStormSpell(FSource, Selection.FTargetX, Selection.FTargetY, Selection.FTargetL);
                    if (call__124)
                    {
                        Selection.Deactivate();
                    }

                    FTargetStorm.Free();
                    FTargetStorm = null;
                    break;
            }
        }
    }

    public partial class TFireStormSpell : TStormSpell
    {
        // Storms.pas:939
        public TFireStormSpell(TECustomNode owner) : base(owner)
        {
            var call__125 = TranslateRStr(FireStormRStr);
            FSpellName = call__125;
            FSpellID = 3;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msFire;
        }

        // Storms.pas:949
        public override TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = new TFireStorm(null);
            return Result;
        }
    }

    public partial class TDivineStormSpell : TStormSpell
    {
        // Storms.pas:954
        public TDivineStormSpell(TECustomNode owner) : base(owner)
        {
            var call__126 = TranslateRStr(DivineStormRStr);
            FSpellName = call__126;
            FSpellID = 6;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msLife;
        }

        // Storms.pas:964
        public override TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = new TDivineStorm(null);
            return Result;
        }
    }

    public partial class TDeathStormSpell : TStormSpell
    {
        // Storms.pas:969
        public TDeathStormSpell(TECustomNode owner) : base(owner)
        {
            var call__127 = TranslateRStr(DeathStormRStr);
            FSpellName = call__127;
            FSpellID = 5;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msDeath;
        }

        // Storms.pas:979
        public override TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = new TDeathStorm(null);
            return Result;
        }
    }

    public partial class TIceStormSpell : TStormSpell
    {
        // Storms.pas:984
        public TIceStormSpell(TECustomNode owner) : base(owner)
        {
            var call__128 = TranslateRStr(IceStormRStr);
            FSpellName = call__128;
            FSpellID = 4;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msAir;
        }

        // Storms.pas:994
        public override TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = new TIceStorm(null);
            return Result;
        }
    }

    public partial class TLightningStormSpell : TStormSpell
    {
        // Storms.pas:1014
        public TLightningStormSpell(TECustomNode owner) : base(owner)
        {
            var call__129 = TranslateRStr(LightningStormRStr);
            FSpellName = call__129;
            FSpellID = 7;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msAir;
        }

        // Storms.pas:1024
        public override TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = new TLightningStorm(null);
            return Result;
        }
    }

    public partial class TPestilenceStormSpell : TStormSpell
    {
        // Storms.pas:999
        public TPestilenceStormSpell(TECustomNode owner) : base(owner)
        {
            var call__130 = TranslateRStr(PestilenceRStr);
            FSpellName = call__130;
            FSpellID = 9;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msDeath;
        }

        // Storms.pas:1009
        public override TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = new TPestilenceStorm(null);
            return Result;
        }
    }

    public static partial class Storms
    {
        // Storms.pas:1029
        public static void RegisterStorms(TEngine AEngine)
        {
            TSpellControl Spells = default;
            AEngine.RegisterEClasses([EClass.Of<TIceStorm>(), EClass.Of<TDeathStorm>(), EClass.Of<TDivineStorm>(), EClass.Of<TFireStorm>(), EClass.Of<TLightningStorm>(), EClass.Of<TPestilenceStorm>(), EClass.Of<TStormSpellTE>()]);
            Spells = ((TAoWEngine)AEngine).FGameSet.FSpellControl;
            var call__131 = new TFireStormSpell(null);
            Spells.RegisterSpell(call__131);
            var call__132 = new TDeathStormSpell(null);
            Spells.RegisterSpell(call__132);
            var call__133 = new TDivineStormSpell(null);
            Spells.RegisterSpell(call__133);
            var call__134 = new TLightningStormSpell(null);
            Spells.RegisterSpell(call__134);
            var call__135 = new TIceStormSpell(null);
            Spells.RegisterSpell(call__135);
            var call__136 = new TPestilenceStormSpell(null);
            Spells.RegisterSpell(call__136);
        }

        // Storms.pas:1044
        public static void UnRegisterStorms(TEngine AEngine)
        {
            TSpellControl Spells = default;
            Spells = ((TAoWEngine)AEngine).FGameSet.FSpellControl;
            Spells.UnregisterSpellID(3);
            Spells.UnregisterSpellID(5);
            Spells.UnregisterSpellID(6);
            Spells.UnregisterSpellID(7);
            Spells.UnregisterSpellID(4);
            Spells.UnregisterSpellID(9);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
