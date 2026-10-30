// Translated from Storms.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.PoisonC;
using static Aow.Units.PoisonC.PoisonC;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Storms
{
    // { TStorm (instance size 64, VMT 557CB0BC): $3C player, $3D padding. }
    public partial class TStorm : Aow.Units.Animation.TAnimation
    {
        public TStorm(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public sbyte FPlayer;
        // { $3C }
        public Aow.Units.FixedArray_byte_3 FReserved3D;
        // Storms.pas:34
        public virtual void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref Aow.Units.AoWE.TStormDamageInfo Info)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Storms.pas:236
        // { +124 }
        public virtual void UpdateTargetSelection(Aow.Units.AoWE.TTargetSelectionControl Selection)
        {
            Selection.FOnShow = ShowTargetSelection;
            Selection.FTargetMode = Selection.FTargetMode + TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.Of(Aow.Units.AoWE.TTargetSelectionModeElement.tmOwnerDraw);
        }

        // Storms.pas:209
        public virtual void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            FPlayer = unchecked((sbyte)Player);
            FAnimationLevel = 28;
            Aow.Units.AoWE.AoWE.AoWHSMap.LockExecuteEventLog();
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
            Aow.Units.AoWE.AoWE.AoWHSMap.UnlockExecuteEventLog();
        }

        // Storms.pas:226
        public void ShowTargetSelection(Aow.Units.AoWE.TTargetSelectionControl Selection, int HN, int Xs, int Ys, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Intensity = default;
            HN = Aow.Units.HSEngine.HSEngine.HNtoRad(HN);
            Intensity = (System.Math.Abs(((Aow.Units.AoWE.AoWE.AoWHSMap.Frame + HN) % 9) - 4) * 20) + 20;
            var call__1 = Aow.Units.GFXE.GFXE.IntensityBlendTable.GetLookupTables(Intensity);
            var call__2 = Selection.FImageLibrary[0];
            call__2.ShowBlended(Xs - 8, Ys, SurfaceDesc, call__1);
        }

        // Storms.pas:242
        public bool GetBestTarget(sbyte Player, int X, int Y, int L, int Range, ref Aow.Units.HSEngine.TXYL Target, ref Aow.Units.AoWE.TStormDamageInfo Info)
        {
            bool Result = default;
            Aow.Units.AoWE.TStormDamageInfo Candidate = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
            Level = call__3;
            Target.XYL = 16777215;
            Result = false;
            Info = default;
            Candidate = default;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Range)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__4 = Level.Field[Walk.X, Walk.Y];
                    var call__5 = ((Aow.Units.AoWE.TAoWMapField)call__4).GetArmyHS();
                    ArmyHS = call__5;
                    bool bool__6 = (ArmyHS != null) && (ArmyHS.FArmy.FPlayer != 0);
                    if (bool__6)
                    {
                        var call__7 = Aow.Units.AoWE.AoWE.ValidAttackRelation(unchecked((sbyte)Player), 0, unchecked((sbyte)ArmyHS.FArmy.FPlayer), 0);
                        bool__6 = call__7;
                    }

                    if (bool__6)
                    {
                        GetTargetDamageInfo(unchecked((sbyte)Player), unchecked((sbyte)Walk.X), unchecked((sbyte)Walk.Y), unchecked((sbyte)L), ref Candidate);
                        if ((Info.EnemyDamage - Info.FriendlyDamage) < (Candidate.EnemyDamage - Candidate.FriendlyDamage))
                        {
                            Info = Candidate;
                            var call__8 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)Walk.X), unchecked((sbyte)Walk.Y), unchecked((sbyte)L));
                            Target = call__8;
                            Result = true;
                        }
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // Storms.pas:39
        // { +134 }
        public virtual Aow.Rtl.AnsiString Name()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        public new partial class Meta : Aow.Units.Animation.TAnimation.Meta
        {
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    // { TBlastStorm (instance size 68, VMT 557CB260): $40 storm image library. }
    public partial class TBlastStorm : Aow.Units.Storms.TStorm
    {
        public TBlastStorm(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public Aow.Units.ImageLib.TCustomImageLibrary FStormImages;
        // Storms.pas:338
        // { $40 }
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            Frame = FFrameCounter % FStormImages.Count;
            var call__9 = GetYhx();
            var call__10 = GetXhx();
            Aow.Units.HSEngine.HSEngine.HXtoHP(call__10, call__9, ref X, ref Y);
            var call__11 = FStormImages[Frame];
            X = (X - ((int)(((uint)call__11.Width) >> 1))) + 10;
            var call__12 = FStormImages[Frame];
            Y = (Y - ((int)(((uint)call__12.Height) >> 1))) - 42;
            var call__13 = FStormImages[Frame];
            call__13.Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
        }

        // Storms.pas:358
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref Aow.Units.AoWE.TStormDamageInfo Info)
        {
            var call__14 = StormType();
            Aow.Units.AoWE.AoWE.RetrieveStormDamageInfo(unchecked((sbyte)Player), call__14, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0, 4, ref Info);
        }

        // Storms.pas:349
        public override void UpdateTargetSelection(Aow.Units.AoWE.TTargetSelectionControl Selection)
        {
            int I = default;
            base.UpdateTargetSelection(Selection);
            Selection.FHNs.Clear();
            {
                I = 0;
                int end__15 = Aow.Units.HSEngine.HSEngine.RadToHN(5) - 1;
                for (; I <= end__15; I += 1)
                {
                    Selection.FHNs.Add(0);
                }
            }
        }

        // Storms.pas:276
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            Aow.Rtl.TRect R = default;
            base.InitializeStorm(unchecked((sbyte)Player), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
            var call__16 = StormFileName();
            var call__17 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), call__16);
            FStormImages = call__17;
            if (GetVisible())
            {
                FStormImages.LoadDiscardableImageData();
            }

            var call__18 = FStormImages[0];
            R.Left = -((int)(((uint)call__18.Width) >> 1));
            var call__19 = FStormImages[0];
            R.Right = (int)(((uint)call__19.Width) >> 1);
            var call__20 = FStormImages[0];
            R.Top = -((int)(((uint)call__20.Height) >> 1));
            var call__21 = FStormImages[0];
            R.Bottom = (int)(((uint)call__21.Height) >> 1);
            SetDimensions(R);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 4);
            bool bool__22 = (Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__22)
            {
                var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.TerrainVisible(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 4);
                bool__22 = call__23;
            }

            if (bool__22)
            {
                Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\ATTACK\\STORMBIG.WAV");
            }

            PlaceHX(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
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
                Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, call__26, call__25, call__24, 1, ChangeStormTerrain, StormTerrainChanged);
            }
            else
            {
                if (FFrameCounter == 12)
                {
                    var call__27 = GetLhx();
                    var call__28 = GetYhx();
                    var call__29 = GetXhx();
                    Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, call__29, call__28, call__27, 2, ChangeStormTerrain, StormTerrainChanged);
                }
                else
                {
                    if (FFrameCounter == 18)
                    {
                        var call__30 = GetLhx();
                        var call__31 = GetYhx();
                        var call__32 = GetXhx();
                        Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, call__32, call__31, call__30, 3, ChangeStormTerrain, StormTerrainChanged);
                    }
                    else
                    {
                        if (FFrameCounter == 22)
                        {
                            var call__33 = GetLhx();
                            var call__34 = GetYhx();
                            var call__35 = GetXhx();
                            Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, call__35, call__34, call__33, 4, ChangeStormTerrain, StormTerrainChanged);
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
                Aow.Units.AoWE.AoWE.TriggerStormDamage(call__39, unchecked((sbyte)call__38), unchecked((sbyte)call__37), unchecked((sbyte)call__36), 0, 4, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer));
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
            Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), call__40);
            base.FinalizeStorm();
        }

        // Storms.pas:304
        public virtual void ChangeStormTerrain(Aow.Units.HSEngine.TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
        }

        // Storms.pas:308
        // { +13C }
        public virtual void StormTerrainChanged(Aow.Units.HSEngine.TMapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
        }

        // Storms.pas:294
        // { +140 }
        public virtual Aow.Rtl.AnsiString StormFileName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            return Result;
        }

        // Storms.pas:299
        // { +144 }
        public virtual Aow.Units.AoWE.TStormType StormType()
        {
            Aow.Units.AoWE.TStormType Result = default;
            Result = Aow.Units.AoWE.TStormType.stNone;
            return Result;
        }
    }

    // { +148 }
    // { TIceStorm (instance size 68, VMT 557CB41C) }
    public partial class TIceStorm : Aow.Units.Storms.TBlastStorm
    {
        public TIceStorm(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Storms.pas:363
        public new const int ClassID = 135738;

        // Storms.pas:390
        public override void ChangeStormTerrain(Aow.Units.HSEngine.TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            Aow.Units.AoWE.TFrozenWaterHS Ice = default;
            // { Original sends terrain 13 to the default branch before the ice refresh test. }
            if (unchecked((short)TerrainType) == 0)
            {
                TerrainType = 6;
            }
            else
            {
                if (unchecked((short)TerrainType) != 6)
                {
                    TerrainType = 3;
                }
                else
                {
                    if (TSet<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)TerrainType))
                    {
                        Ice = (Aow.Units.AoWE.TFrozenWaterHS)MapF.FindOwnedHS(typeof(Aow.Units.AoWE.TFrozenWaterHS));
                        if (Ice != null)
                        {
                            var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                            Ice.FDaysRemaining = unchecked((sbyte)(3 + ((call__41 - 5) / 5)));
                            Ice.FCasterPlayer = unchecked((sbyte)FPlayer);
                        }
                    }
                }
            }
        }

        // Storms.pas:373
        public override void StormTerrainChanged(Aow.Units.HSEngine.TMapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
            Aow.Units.AoWE.TFrozenWaterHS Ice = default;
            if (TSet<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)OriginalTerrainType))
            {
                Ice = (Aow.Units.AoWE.TFrozenWaterHS)MapF.FindOwnedHS(typeof(Aow.Units.AoWE.TFrozenWaterHS));
                if (Ice == null)
                {
                    Ice = Aow.Units.AoWE.TFrozenWaterHS.Create(null);
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                }

                var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                Ice.FDaysRemaining = unchecked((sbyte)(3 + ((call__42 - 5) / 5)));
                Ice.FCasterPlayer = unchecked((sbyte)FPlayer);
            }
        }

        // Storms.pas:409
        public override Aow.Rtl.AnsiString StormFileName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"EFFECTS\\STORM1.ILB";
            return Result;
        }

        // Storms.pas:414
        public override Aow.Units.AoWE.TStormType StormType()
        {
            Aow.Units.AoWE.TStormType Result = default;
            Result = Aow.Units.AoWE.TStormType.stIce;
            return Result;
        }

        // Animation.pas:43


        // Storms.pas:368
        public override Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IceStormRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.Storms.TStorm.Meta
        {
            // Storms.pas:363
            public override int ClassID() => Aow.Units.Storms.TIceStorm.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Storms.TIceStorm => (Meta)MetaRegistry.Of(typeof(T));
            // Animation.pas:43
            public new Aow.Units.Storms.TIceStorm Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Storms.TIceStorm)base.Create(AOwner);
        }
    }

    // { TDeathStorm (instance size 68, VMT 557CB5D8) }
    public partial class TDeathStorm : Aow.Units.Storms.TBlastStorm
    {
        public TDeathStorm(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Storms.pas:492
        public new const int ClassID = 135741;

        // Storms.pas:502
        public override void ChangeStormTerrain(Aow.Units.HSEngine.TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if ((unchecked((short)TerrainType) != 0) && (unchecked((short)TerrainType) != 6))
            {
                TerrainType = 5;
            }
        }

        // Storms.pas:507
        public override Aow.Rtl.AnsiString StormFileName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"EFFECTS\\STORMD.ILB";
            return Result;
        }

        // Storms.pas:512
        public override Aow.Units.AoWE.TStormType StormType()
        {
            Aow.Units.AoWE.TStormType Result = default;
            Result = Aow.Units.AoWE.TStormType.stDeath;
            return Result;
        }

        // Animation.pas:43


        // Storms.pas:497
        public override Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathStormRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.Storms.TStorm.Meta
        {
            // Storms.pas:492
            public override int ClassID() => Aow.Units.Storms.TDeathStorm.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Storms.TDeathStorm => (Meta)MetaRegistry.Of(typeof(T));
            // Animation.pas:43
            public new Aow.Units.Storms.TDeathStorm Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Storms.TDeathStorm)base.Create(AOwner);
        }
    }

    // { TDivineStorm (instance size 68, VMT 557CB794) }
    public partial class TDivineStorm : Aow.Units.Storms.TBlastStorm
    {
        public TDivineStorm(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Storms.pas:608
        public new const int ClassID = 135740;

        // Storms.pas:618
        public override void ChangeStormTerrain(Aow.Units.HSEngine.TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if ((unchecked((short)TerrainType) != 6) && (unchecked((short)TerrainType) != 0))
            {
                TerrainType = 1;
            }
        }

        // Storms.pas:624
        public override Aow.Rtl.AnsiString StormFileName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"EFFECTS\\STORMLGT.ILB";
            return Result;
        }

        // Storms.pas:629
        public override Aow.Units.AoWE.TStormType StormType()
        {
            Aow.Units.AoWE.TStormType Result = default;
            Result = Aow.Units.AoWE.TStormType.stHoly;
            return Result;
        }

        // Animation.pas:43


        // Storms.pas:613
        public override Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DivineStormRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.Storms.TStorm.Meta
        {
            // Storms.pas:608
            public override int ClassID() => Aow.Units.Storms.TDivineStorm.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Storms.TDivineStorm => (Meta)MetaRegistry.Of(typeof(T));
            // Animation.pas:43
            public new Aow.Units.Storms.TDivineStorm Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Storms.TDivineStorm)base.Create(AOwner);
        }
    }

    // { TLightningStorm (instance size 68, VMT 557CB958): $40 storm image library. }
    public partial class TLightningStorm : Aow.Units.Storms.TStorm
    {
        public TLightningStorm(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public new const int ClassID = 135739;

        public Aow.Units.ImageLib.TCustomImageLibrary FStormImages;
        // Storms.pas:429
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
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
                Aow.Units.HSEngine.HSEngine.HXtoHP(call__45, call__44, ref X, ref Y);
                var call__46 = FStormImages[Frame];
                X = (X - ((int)(((uint)call__46.Width) >> 1))) + 20;
                var call__47 = FStormImages[Frame];
                Y = ((Y - call__47.Height) + 32) + 40;
                var call__48 = FStormImages[Frame];
                call__48.Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
            }
        }

        // Storms.pas:487
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref Aow.Units.AoWE.TStormDamageInfo Info)
        {
            Aow.Units.AoWE.AoWE.RetrieveStormDamageInfo(unchecked((sbyte)Player), Aow.Units.AoWE.TStormType.stLightning, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0, 1, ref Info);
        }

        // Storms.pas:478
        public override void UpdateTargetSelection(Aow.Units.AoWE.TTargetSelectionControl Selection)
        {
            int I = default;
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection, without clearing its existing hexes. }
            {
                I = 0;
                int end__49 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__49; I += 1)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FHNs.Add(0);
                }
            }
        }

        // Storms.pas:443
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            Aow.Rtl.TRect R = default;
            base.InitializeStorm(unchecked((sbyte)Player), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
            var call__50 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"EFFECTS\\LIGHTN.ILB");
            FStormImages = call__50;
            if (GetVisible())
            {
                FStormImages.LoadDiscardableImageData();
            }

            var call__51 = FStormImages[0];
            R.Left = -((int)(((uint)call__51.Width) >> 1));
            var call__52 = FStormImages[0];
            R.Right = (int)(((uint)call__52.Width) >> 1);
            var call__53 = FStormImages[0];
            R.Top = -call__53.Height + 32;
            R.Bottom = 32;
            SetDimensions(R);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 2);
            PlaceHX(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
        }

        // Storms.pas:466
        public override bool UpdateStorm()
        {
            bool Result = default;
            NewFrame();
            var call__54 = FStormImages.GetSafe(FFrameCounter);
            if (call__54 != null)
            {
                Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\ATTACK\\LIGHTNG.WAV");
            }

            if (FStormImages.Count == FFrameCounter)
            {
                var call__55 = GetLhx();
                var call__56 = GetYhx();
                var call__57 = GetXhx();
                Aow.Units.AoWE.AoWE.TriggerStormDamage(Aow.Units.AoWE.TStormType.stLightning, unchecked((sbyte)call__57), unchecked((sbyte)call__56), unchecked((sbyte)call__55), 0, 1, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer));
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
            Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"EFFECTS\\LIGHTN.ILB");
            base.FinalizeStorm();
        }

        // Animation.pas:43


        // Storms.pas:424
        public override Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LightningStormRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.Storms.TStorm.Meta
        {
            // Storms.pas:419
            // { $40 }
            public override int ClassID() => Aow.Units.Storms.TLightningStorm.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Storms.TLightningStorm => (Meta)MetaRegistry.Of(typeof(T));
            // Animation.pas:43
            public new Aow.Units.Storms.TLightningStorm Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Storms.TLightningStorm)base.Create(AOwner);
        }
    }

    // { TFireStorm (instance size 64, VMT 557CBB0C) }
    public partial class TFireStorm : Aow.Units.Storms.TStorm
    {
        public TFireStorm(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Storms.pas:517
        public new const int ClassID = 135742;

        // Storms.pas:527
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            var call__58 = GetYhx();
            var call__59 = GetXhx();
            Aow.Units.HSEngine.HSEngine.HXtoHP(call__59, call__58, ref X, ref Y);
            X = (X - XhpSurf) + 8;
            Y = Y - YhpSurf;
            var call__60 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[0];
            Sequence = call__60;
            while (Sequence != null)
            {
                var call__61 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Frame = call__61;
                bool bool__62 = (Frame != -1);
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
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref Aow.Units.AoWE.TStormDamageInfo Info)
        {
            Aow.Units.AoWE.AoWE.RetrieveStormDamageInfo(unchecked((sbyte)Player), Aow.Units.AoWE.TStormType.stFire, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0, 2, ref Info);
        }

        // Storms.pas:594
        public override void UpdateTargetSelection(Aow.Units.AoWE.TTargetSelectionControl Selection)
        {
            int I = default;
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection, without clearing its existing hexes. }
            {
                I = 0;
                int end__65 = Aow.Units.HSEngine.HSEngine.RadToHN(3) - 1;
                for (; I <= end__65; I += 1)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FHNs.Add(0);
                }
            }
        }

        // Storms.pas:545
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            Aow.Rtl.TRect R = default;
            base.InitializeStorm(unchecked((sbyte)Player), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
            R.Left = -80;
            R.Right = 96;
            R.Top = -80;
            R.Bottom = 96;
            SetDimensions(R);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 2);
            PlaceHX(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
        }

        // Storms.pas:569
        public override bool UpdateStorm()
        {
            bool Result = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Frame = default;
            NewFrame();
            if (FFrameCounter == 20)
            {
                var call__66 = GetLhx();
                var call__67 = GetYhx();
                var call__68 = GetXhx();
                Aow.Units.AoWE.AoWE.TriggerStormDamage(Aow.Units.AoWE.TStormType.stFire, unchecked((sbyte)call__68), unchecked((sbyte)call__67), unchecked((sbyte)call__66), 0, 2, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer));
                var call__69 = GetLhx();
                var call__70 = GetYhx();
                var call__71 = GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(this, call__71, call__70, call__69, 2, ChangeStormTerrain, null);
            }

            var call__72 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[0];
            Sequence = call__72;
            Frame = -1;
            while ((Sequence != null) && (Frame == -1))
            {
                var call__73 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Frame = call__73;
                Sequence = Sequence.Next;
            }

            if (Frame == 1)
            {
                bool bool__74 = (Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__74)
                {
                    var call__75 = GetLhx();
                    var call__76 = GetYhx();
                    var call__77 = GetXhx();
                    var call__78 = Aow.Units.AoWE.AoWE.AoWHSMap.TerrainVisible(unchecked((sbyte)call__77), unchecked((sbyte)call__76), unchecked((sbyte)call__75), 2);
                    bool__74 = call__78;
                }

                if (bool__74)
                {
                    var call__79 = Aow.Rtl.Rtl.Random(2);
                    Aow.Units.Sound.Sound.PlaySample(Aow.Units.AoWE.AoWE.ExplosionSFX[call__79]);
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
        public void ChangeStormTerrain(Aow.Units.HSEngine.TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (unchecked((short)TerrainType) == 6)
            {
                TerrainType = 0;
            }
        }

        // Animation.pas:43


        // Storms.pas:522
        public override Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireStormRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.Storms.TStorm.Meta
        {
            // Storms.pas:517
            public override int ClassID() => Aow.Units.Storms.TFireStorm.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Storms.TFireStorm => (Meta)MetaRegistry.Of(typeof(T));
            // Animation.pas:43
            public new Aow.Units.Storms.TFireStorm Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Storms.TFireStorm)base.Create(AOwner);
        }
    }

    // { TPestilenceStorm (instance size 68, VMT 557CBCB8): $40 poison cloud resource. }
    public partial class TPestilenceStorm : Aow.Units.Storms.TStorm
    {
        public TPestilenceStorm(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public new const int ClassID = 135745;

        public Aow.Units.PoisonC.TPoisonCloudResource FCloudResource;
        // Storms.pas:644
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            int Blend = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            var call__80 = GetYhx();
            var call__81 = GetXhx();
            Aow.Units.HSEngine.HSEngine.HXtoHP(call__81, call__80, ref X, ref Y);
            X = (X - XhpSurf) + 8;
            Y = Y - YhpSurf;
            var call__82 = FCloudResource.ImageSequenceList.ImageSequences[0];
            Sequence = call__82;
            while (Sequence != null)
            {
                var call__83 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Frame = call__83;
                bool bool__84 = (Frame != -1);
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
                        Blend = (FFrameCounter * 100) / Blend;
                    }
                    else
                    {
                        Blend = 100;
                    }

                    var call__86 = Sequence.ImageLibrary[Frame];
                    call__86.ShowBlended(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc, Aow.Units.GFXE.GFXE.ShadowBlendTable.LookupTables[Blend]);
                }

                Sequence = Sequence.Next;
            }
        }

        // Storms.pas:725
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref Aow.Units.AoWE.TStormDamageInfo Info)
        {
            Aow.Units.AoWE.AoWE.RetrieveStormDamageInfo(unchecked((sbyte)Player), Aow.Units.AoWE.TStormType.stPestilence, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0, 2, ref Info);
        }

        // Storms.pas:716
        public override void UpdateTargetSelection(Aow.Units.AoWE.TTargetSelectionControl Selection)
        {
            int I = default;
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection without clearing its existing hexes. }
            {
                I = 0;
                int end__87 = Aow.Units.HSEngine.HSEngine.RadToHN(3) - 1;
                for (; I <= end__87; I += 1)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FHNs.Add(0);
                }
            }
        }

        // Storms.pas:668
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            Aow.Rtl.TRect R = default;
            var call__88 = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FindChild(135744);
            FCloudResource = (Aow.Units.PoisonC.TPoisonCloudResource)call__88;
            base.InitializeStorm(unchecked((sbyte)Player), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
            R.Left = -80;
            R.Right = 96;
            R.Top = -80;
            R.Bottom = 96;
            SetDimensions(R);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 2);
            bool bool__89 = (Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__89)
            {
                var call__90 = Aow.Units.AoWE.AoWE.AoWHSMap.TerrainVisible(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 2);
                bool__89 = call__90;
            }

            if (bool__89)
            {
                var call__91 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(9);
                call__91.FCastSounds.Play(0);
            }

            PlaceHX(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
        }

        // Storms.pas:690
        public override bool UpdateStorm()
        {
            bool Result = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Frame = default;
            Aow.Units.PoisonC.TPoisonCloud Cloud = default;
            NewFrame();
            var call__92 = FCloudResource.ImageSequenceList.ImageSequences[0];
            Sequence = call__92;
            Frame = -1;
            while ((Sequence != null) && (Frame == -1))
            {
                var call__93 = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Frame = call__93;
                Sequence = Sequence.Next;
            }

            if (Frame == -1)
            {
                var call__94 = FCloudResource.CreateResourceUser();
                Cloud = (Aow.Units.PoisonC.TPoisonCloud)call__94;
                Cloud.FTurnsRemaining = 3;
                Cloud.FPlayer = unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer);
                var call__95 = GetLhx();
                var call__96 = GetYhx();
                var call__97 = GetXhx();
                var call__98 = Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Cloud, call__97, call__96, call__95);
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
        public override Aow.Rtl.AnsiString Name()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PestilenceStormRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.Storms.TStorm.Meta
        {
            // Storms.pas:634
            // { $40 }
            public override int ClassID() => Aow.Units.Storms.TPestilenceStorm.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Storms.TPestilenceStorm => (Meta)MetaRegistry.Of(typeof(T));
            // Animation.pas:43
            public new Aow.Units.Storms.TPestilenceStorm Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Storms.TPestilenceStorm)base.Create(AOwner);
        }
    }

    // { TStormSpellTE (instance size 40, VMT 557CBE74): $20 storm, $24 X, $25 Y, $26 layer. }
    public partial class TStormSpellTE : Aow.Units.AoWE.TSpellTE
    {
        public TStormSpellTE(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Storms.pas:730
        public new const int ClassID = 135937;

        public Aow.Units.Storms.TStorm FStorm;
        // { $20 }
        public sbyte FX;
        // { $24 }
        public sbyte FY;
        // { $25 }
        public sbyte FL;
        // { $26 }
        public Aow.Units.FixedArray_byte_1 F_StormSpellTE_27;
        // Storms.pas:735
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FX), 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FY), 21);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FL), 22);
        }

        // Storms.pas:743
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            if (A2 == 0)
            {
                var call__99 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__100 = ((Aow.Units.Storms.TStormSpell)call__99).CreateStorm();
                FStorm = call__100;
                FStorm.InitializeStorm(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)FX), unchecked((sbyte)FY), unchecked((sbyte)FL));
                var call__101 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
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


        public new partial class Meta : Aow.Units.AoWE.TSpellTE.Meta
        {
            // Storms.pas:730
            public override int ClassID() => Aow.Units.Storms.TStormSpellTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Storms.TStormSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.Storms.TStormSpellTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Storms.TStormSpellTE)base.Create(AOwner);
        }
    }

    // { TStormSpell (instance size 60, VMT 557CBF48): $34 targeting storm, $38 caster. }
    public partial class TStormSpell : Aow.Units.AoWE.TSpell
    {
        public Aow.Units.Storms.TStorm FTargetStorm;
        // { $34 }
        public Aow.Units.AoWE.TAbstractUnit FSource;
        // Storms.pas:762
        // { $38 }
        private protected TStormSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
        }

        // Storms.pas:888
        public override void AIPrefetchCastSpellActions(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TAICastSpellActionList A2)
        {
            int Priority = default;
            Aow.Units.AoWE.TAICastSpellAction Action = default;
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
        public override bool AIExecuteCastSpellAction(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.HSEngine.TXYL Target = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.Storms.TStorm Storm = default;
            Aow.Units.AoWE.TStormDamageInfo Info = default;
            if (AICastSpellPriority(A1) != -1)
            {
                AICastSpellExpenseMade(A1, FBaseManaCost, FManaUpkeep, 0);
                var call__104 = ((Aow.Units.AoWE.THero)A1).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason);
                if (call__104)
                {
                    A1.GetLocation(ref X, ref Y, ref L);
                    Storm = CreateStorm();
                    var call__105 = Storm.GetBestTarget(unchecked((sbyte)A1.FPlayer), X, Y, L, 12, ref Target, ref Info);
                    if (call__105)
                    {
                        ExecuteStormSpell(A1, unchecked((sbyte)Target.X), unchecked((sbyte)Target.Y), unchecked((sbyte)Target.L));
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
                    if (((Aow.Units.AoWE.THero)A1).FCastingSpellID != FSpellID)
                    {
                        ((Aow.Units.AoWE.THero)A1).ExecuteStartCasting(FSpellID);
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
        public override bool CanActivate(Aow.Units.AoWE.TAbstractUnit A1, ref Aow.Rtl.AnsiString A2)
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
                    A2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // Storms.pas:825
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TTargetSelectionControl Selection = default;
            base.Activate(A1);
            if (CanActivate(A1, ref Reason))
            {
                FSource = A1;
                A1.GetLocation(ref X, ref Y, ref L);
                Selection = Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection;
                var call__106 = CreateStorm();
                FTargetStorm = call__106;
                Selection.Initialize(this);
                Selection.FTargetMode = Selection.FTargetMode + TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.Of(Aow.Units.AoWE.TTargetSelectionModeElement.tmValidateDistance, Aow.Units.AoWE.TTargetSelectionModeElement.tmExternalFloater);
                Selection.FSourceX = X;
                Selection.FSourceY = Y;
                Selection.FSourceL = L;
                Aow.Units.AoWE.AoWE.AoWHSMap.MakeVisible(X, Y, L, 5);
                var call__107 = FTargetStorm.Name();
                var call__108 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectTargetLocationForXOnMapRStr);
                var call__109 = Aow.Rtl.Rtl.Format(call__108, [call__107]);
                Selection.FText.Text = call__109;
                Selection.FTitle = FSpellName;
                var call__110 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
                Selection.FCancelText = call__110;
                Selection.FRange = 12;
                Selection.FOnChange.Add(OnTargetSelectionChange);
                var call__111 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__112 = call__111.FCastImages.ImageSequences[20];
                var call__113 = call__112.GetPreviewImage();
                Selection.FSourceImage = call__113;
                var call__114 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__115 = call__114.FCastImages.ImageSequences[21];
                var call__116 = call__115.GetPreviewImage();
                Selection.FValidTargetImage = call__116;
                var call__117 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__118 = call__117.FCastImages.ImageSequences[22];
                var call__119 = call__118.GetPreviewImage();
                Selection.FInvalidTargetImage = call__119;
                Selection.Activate();
                FTargetStorm.UpdateTargetSelection(Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Storms.pas:156
        public virtual Aow.Units.Storms.TStorm CreateStorm()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Storms.pas:768
        // { +09C }
        public virtual bool ExecuteStormSpell(Aow.Units.AoWE.TAbstractUnit Source, sbyte X, sbyte Y, sbyte L)
        {
            bool Result = default;
            Aow.Units.Storms.TStormSpellTE TE = default;
            var call__120 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
            var call__121 = call__120.GetBusy();
            if (!call__121)
            {
                TE = new Aow.Units.Storms.TStormSpellTE(null);
                TE.Initialize(FSpellID, unchecked((byte)Source.FPlayer), Source.FUnitID, FBaseManaCost);
                TE.FX = unchecked((sbyte)X);
                TE.FY = unchecked((sbyte)Y);
                TE.FL = unchecked((sbyte)L);
                var call__122 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer);
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
        // { +0A0 }
        public virtual int AICastSpellPriority(Aow.Units.AoWE.TAbstractUnit Source)
        {
            int Result = default;
            Aow.Units.HSEngine.TXYL Target = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.Storms.TStorm Storm = default;
            Aow.Units.AoWE.TStormDamageInfo Info = default;
            int Damage = default;
            if (CanActivate(Source, ref Reason))
            {
                Result = -1;
                Source.GetLocation(ref X, ref Y, ref L);
                if (L == 0)
                {
                    Storm = CreateStorm();
                    var call__123 = Storm.GetBestTarget(unchecked((sbyte)Source.FPlayer), X, Y, L, 12, ref Target, ref Info);
                    if (call__123)
                    {
                        Damage = Info.EnemyDamage - Info.FriendlyDamage;
                        if (((double)(((double)FBaseManaCost) * 0.75)) < Damage)
                        {
                            Result = (Damage * 5) + 750;
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
        // { +0A4 }
        public void OnTargetSelectionChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TTargetSelectionControl Selection = default;
            Selection = (Aow.Units.AoWE.TTargetSelectionControl)Sender;
            switch (Selection.FChange)
            {
                case Aow.Units.AoWE.TTargetSelectionChange.tscDeactivated:
                    FTargetStorm.Free();
                    FTargetStorm = null;
                    Selection.FOnChange.Remove(OnTargetSelectionChange);
                    break;
                case Aow.Units.AoWE.TTargetSelectionChange.tscSelected:
                    Selection.FOnChange.Remove(OnTargetSelectionChange);
                    var call__124 = ExecuteStormSpell(FSource, unchecked((sbyte)Selection.FTargetX), unchecked((sbyte)Selection.FTargetY), unchecked((sbyte)Selection.FTargetL));
                    if (call__124)
                    {
                        Selection.Deactivate();
                    }

                    FTargetStorm.Free();
                    FTargetStorm = null;
                    break;
            }
        }

        // Storms.pas:151
        public static new Aow.Units.Storms.TStormSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TStormSpell(context, owner));
        }
    }

    // { TFireStormSpell (instance size 60, VMT 557CC060) }
    public partial class TFireStormSpell : Aow.Units.Storms.TStormSpell
    {
        // Storms.pas:939
        private TFireStormSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__125 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireStormRStr);
            FSpellName = call__125;
            FSpellID = 3;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
        }

        // Storms.pas:949
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TFireStorm(null);
            return Result;
        }

        // Storms.pas:165
        public static new Aow.Units.Storms.TFireStormSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFireStormSpell(context, owner));
        }
    }

    // { TDivineStormSpell (instance size 60, VMT 557CC180) }
    public partial class TDivineStormSpell : Aow.Units.Storms.TStormSpell
    {
        // Storms.pas:954
        private TDivineStormSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__126 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DivineStormRStr);
            FSpellName = call__126;
            FSpellID = 6;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msLife;
        }

        // Storms.pas:964
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TDivineStorm(null);
            return Result;
        }

        // Storms.pas:172
        public static new Aow.Units.Storms.TDivineStormSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TDivineStormSpell(context, owner));
        }
    }

    // { TDeathStormSpell (instance size 60, VMT 557CC2A8) }
    public partial class TDeathStormSpell : Aow.Units.Storms.TStormSpell
    {
        // Storms.pas:969
        private TDeathStormSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__127 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathStormRStr);
            FSpellName = call__127;
            FSpellID = 5;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
        }

        // Storms.pas:979
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TDeathStorm(null);
            return Result;
        }

        // Storms.pas:179
        public static new Aow.Units.Storms.TDeathStormSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TDeathStormSpell(context, owner));
        }
    }

    // { TIceStormSpell (instance size 60, VMT 557CC3D0) }
    public partial class TIceStormSpell : Aow.Units.Storms.TStormSpell
    {
        // Storms.pas:984
        private TIceStormSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__128 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IceStormRStr);
            FSpellName = call__128;
            FSpellID = 4;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msAir;
        }

        // Storms.pas:994
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TIceStorm(null);
            return Result;
        }

        // Storms.pas:186
        public static new Aow.Units.Storms.TIceStormSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TIceStormSpell(context, owner));
        }
    }

    // { TLightningStormSpell (instance size 60, VMT 557CC4F0) }
    public partial class TLightningStormSpell : Aow.Units.Storms.TStormSpell
    {
        // Storms.pas:1014
        private TLightningStormSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__129 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LightningStormRStr);
            FSpellName = call__129;
            FSpellID = 7;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msAir;
        }

        // Storms.pas:1024
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TLightningStorm(null);
            return Result;
        }

        // Storms.pas:193
        public static new Aow.Units.Storms.TLightningStormSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TLightningStormSpell(context, owner));
        }
    }

    // { TPestilenceStormSpell (instance size 60, VMT 557CC620) }
    public partial class TPestilenceStormSpell : Aow.Units.Storms.TStormSpell
    {
        // Storms.pas:999
        private TPestilenceStormSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__130 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PestilenceRStr);
            FSpellName = call__130;
            FSpellID = 9;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
        }

        // Storms.pas:1009
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TPestilenceStorm(null);
            return Result;
        }

        // Storms.pas:200
        public static new Aow.Units.Storms.TPestilenceStormSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TPestilenceStormSpell(context, owner));
        }
    }

    public static partial class Storms
    {
        // Storms.pas:1029
        public static void RegisterStorms(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.AoWE.TSpellControl Spells = default;
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Storms.TIceStorm>(), Aow.Units.Engine.EClass.Of<Aow.Units.Storms.TDeathStorm>(), Aow.Units.Engine.EClass.Of<Aow.Units.Storms.TDivineStorm>(), Aow.Units.Engine.EClass.Of<Aow.Units.Storms.TFireStorm>(), Aow.Units.Engine.EClass.Of<Aow.Units.Storms.TLightningStorm>(), Aow.Units.Engine.EClass.Of<Aow.Units.Storms.TPestilenceStorm>(), Aow.Units.Engine.EClass.Of<Aow.Units.Storms.TStormSpellTE>()]);
            Spells = ((Aow.Units.AoWE.TAoWEngine)AEngine).FGameSet.FSpellControl;
            var call__131 = Aow.Units.Storms.TFireStormSpell.Create(null);
            Spells.RegisterSpell(call__131);
            var call__132 = Aow.Units.Storms.TDeathStormSpell.Create(null);
            Spells.RegisterSpell(call__132);
            var call__133 = Aow.Units.Storms.TDivineStormSpell.Create(null);
            Spells.RegisterSpell(call__133);
            var call__134 = Aow.Units.Storms.TLightningStormSpell.Create(null);
            Spells.RegisterSpell(call__134);
            var call__135 = Aow.Units.Storms.TIceStormSpell.Create(null);
            Spells.RegisterSpell(call__135);
            var call__136 = Aow.Units.Storms.TPestilenceStormSpell.Create(null);
            Spells.RegisterSpell(call__136);
        }

        // Storms.pas:1044
        public static void UnRegisterStorms(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.AoWE.TSpellControl Spells = default;
            Spells = ((Aow.Units.AoWE.TAoWEngine)AEngine).FGameSet.FSpellControl;
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
