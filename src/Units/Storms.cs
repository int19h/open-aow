// Translated from Storms.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Ai;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Game.Audio.SoundUnit;
using static Aow.Game.Draw.GFXEUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.World.Map.HSEngineUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Animation
{

    // player
    public partial class TStorm : TAnimation
    {
        public TStorm(TECustomNode owner) : base(owner)
        { }

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
        public virtual bool UpdateStorm() => false;

        // Storms.pas:221
        public virtual void FinalizeStorm()
        {
            AoWHSMap.UnlockExecuteEventLog();
        }

        // Storms.pas:226
        public void ShowTargetSelection(TTargetSelectionControl Selection, int HN, int Xs, int Ys, in Aow.Game.Draw.TSurfaceDesc SurfaceDesc)
        {
            int Intensity;
            HN = HNtoRad(HN);
            Intensity = Math.Abs((AoWHSMap.Frame + HN) % 9 - 4) * 20 + 20;
            var lookupTable = IntensityBlendTable.GetLookupTables(Intensity);
            Selection.FImageLibrary[0].ShowBlended(Xs - 8, Ys, SurfaceDesc, lookupTable);
        }

        // Storms.pas:242
        public bool GetBestTarget(sbyte Player, int X, int Y, int L, int Range, ref TXYL Target, ref TStormDamageInfo Info)
        {
            TStormDamageInfo Candidate;
            THNWalkData Walk = default;
            TMapLevel Level;
            Aow.Game.Draw.TArmyHS ArmyHS;
            Level = AoWHSMap.MapContainer.MapLevels[L];
            Target.XYL = 16777215;
            bool Result = false;
            Info = default;
            Candidate = default;
            InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Range)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    ArmyHS = ((TAoWMapField)Level.Field[Walk.X, Walk.Y]).GetArmyHS();
                    if (ArmyHS != null && ArmyHS.FArmy.FPlayer != 0 && ValidAttackRelation(Player, 0, ArmyHS.FArmy.FPlayer, 0))
                    {
                        GetTargetDamageInfo(Player, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L, ref Candidate);
                        if (Info.EnemyDamage - Info.FriendlyDamage < Candidate.EnemyDamage - Candidate.FriendlyDamage)
                        {
                            Info = Candidate;
                            Target = PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                            Result = true;
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // Storms.pas:39
        public virtual string Name()
        {
            throw EAbstractError.Create("Abstract Error");
        }

    }

    // storm image library
    public partial class TBlastStorm : TStorm
    {
        public TBlastStorm(TECustomNode owner) : base(owner)
        { }

        public TCustomImageLibrary FStormImages;

        // Storms.pas:338
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Game.Draw.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame;
            Frame = FFrameCounter % FStormImages.Count;
            var yhx = GetYhx();
            HXtoHP(GetXhx(), yhx, ref X, ref Y);
            var stormImageWidth = FStormImages[Frame];
            X = X - (int)(((uint)stormImageWidth.Width) >> 1) + 10;
            var stormImageHeight = FStormImages[Frame];
            Y = Y - (int)(((uint)stormImageHeight.Height) >> 1) - 42;
            FStormImages[Frame].Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
        }

        // Storms.pas:358
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref TStormDamageInfo Info)
        {
            RetrieveStormDamageInfo(Player, StormType(), X, Y, L, 0, 4, ref Info);
        }

        // Storms.pas:349
        public override void UpdateTargetSelection(TTargetSelectionControl Selection)
        {
            base.UpdateTargetSelection(Selection);
            Selection.FHNs.Clear();
            for (int I = 0, last = RadToHN(5) - 1; I <= last; I++)
            {
                Selection.FHNs.Add(0);
            }
        }

        // Storms.pas:276
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            TRect R = default;
            base.InitializeStorm(Player, X, Y, L);
            var stormFileName = StormFileName();
            FStormImages = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), stormFileName);
            if (GetVisible())
            {
                FStormImages.LoadDiscardableImageData();
            }

            R.Left = -(int)(((uint)FStormImages[0].Width) >> 1);
            R.Right = (int)(((uint)FStormImages[0].Width) >> 1);
            R.Top = -(int)(((uint)FStormImages[0].Height) >> 1);
            R.Bottom = (int)(((uint)FStormImages[0].Height) >> 1);
            SetDimensions(R);
            AoWHSMap.ValidateTerrainObservation(AoWHSMap.FCurrentPlayer, X, Y, L, 4);
            if (AoWHSMap.FCurrentPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.TerrainVisible(X, Y, L, 4))
            {
                PlaySample("SFX\\ATTACK\\STORMBIG.WAV");
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
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                AoWHSMap.ChangeTerrainEx(null, xhx, yhx, lhx, 1, ChangeStormTerrain, StormTerrainChanged);
            }
            else
            {
                if (FFrameCounter == 12)
                {
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    AoWHSMap.ChangeTerrainEx(null, xhx, yhx, lhx, 2, ChangeStormTerrain, StormTerrainChanged);
                }
                else
                {
                    if (FFrameCounter == 18)
                    {
                        var lhx = GetLhx();
                        var yhx = GetYhx();
                        var xhx = GetXhx();
                        AoWHSMap.ChangeTerrainEx(null, xhx, yhx, lhx, 3, ChangeStormTerrain, StormTerrainChanged);
                    }
                    else
                    {
                        if (FFrameCounter == 22)
                        {
                            var lhx = GetLhx();
                            var yhx = GetYhx();
                            var xhx = GetXhx();
                            AoWHSMap.ChangeTerrainEx(null, xhx, yhx, lhx, 4, ChangeStormTerrain, StormTerrainChanged);
                        }
                    }
                }
            }

            if (FStormImages.Count == FFrameCounter)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                TriggerStormDamage(StormType(), xhx, yhx, lhx, 0, 4, AoWHSMap.FCurrentPlayer);
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
            var stormFileName = StormFileName();
            AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), stormFileName);
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
        public virtual string StormFileName() => "";

        // Storms.pas:299
        public virtual TStormType StormType() => TStormType.stNone;
    }

    public partial class TIceStorm : TBlastStorm
    {
        public TIceStorm(TECustomNode owner) : base(owner)
        { }

        // Storms.pas:363
        public new const int ClassID = 135738;

        // Storms.pas:390
        public override void ChangeStormTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            Aow.Game.Draw.TFrozenWaterHS Ice;
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
                        Ice = (Aow.Game.Draw.TFrozenWaterHS)MapF.FindOwnedHS(typeof(Aow.Game.Draw.TFrozenWaterHS));
                        if (Ice != null)
                        {
                            Ice.FDaysRemaining = (sbyte)(3 + (AoWHSMap.Random(11) - 5) / 5);
                            Ice.FCasterPlayer = FPlayer;
                        }
                    }
                }
            }
        }

        // Storms.pas:373
        public override void StormTerrainChanged(TMapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
            Aow.Game.Draw.TFrozenWaterHS Ice;
            if (TSet<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)OriginalTerrainType))
            {
                Ice = (Aow.Game.Draw.TFrozenWaterHS)MapF.FindOwnedHS(typeof(Aow.Game.Draw.TFrozenWaterHS));
                if (Ice == null)
                {
                    Ice = new Aow.Game.Draw.TFrozenWaterHS(null);
                    AoWHSMap.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                }

                Ice.FDaysRemaining = (sbyte)(3 + (AoWHSMap.Random(11) - 5) / 5);
                Ice.FCasterPlayer = FPlayer;
            }
        }

        // Storms.pas:409
        public override string StormFileName() => "EFFECTS\\STORM1.ILB";

        // Storms.pas:414
        public override TStormType StormType() => TStormType.stIce;

        // Animation.pas:43

        // Storms.pas:368
        public override string Name() => TranslateRStr(IceStormRStr);

    }

    public partial class TDeathStorm : TBlastStorm
    {
        public TDeathStorm(TECustomNode owner) : base(owner)
        { }

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
        public override string StormFileName() => "EFFECTS\\STORMD.ILB";

        // Storms.pas:512
        public override TStormType StormType() => TStormType.stDeath;

        // Animation.pas:43

        // Storms.pas:497
        public override string Name() => TranslateRStr(DeathStormRStr);

    }

    public partial class TDivineStorm : TBlastStorm
    {
        public TDivineStorm(TECustomNode owner) : base(owner)
        { }

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
        public override string StormFileName() => "EFFECTS\\STORMLGT.ILB";

        // Storms.pas:629
        public override TStormType StormType() => TStormType.stHoly;

        // Animation.pas:43

        // Storms.pas:613
        public override string Name() => TranslateRStr(DivineStormRStr);

    }

    // storm image library
    public partial class TLightningStorm : TStorm
    {
        public TLightningStorm(TECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135739;

        public TCustomImageLibrary FStormImages;

        // Storms.pas:429
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Game.Draw.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame;
            Frame = FFrameCounter % FStormImages.Count;
            if (FStormImages[Frame] != null)
            {
                var yhx = GetYhx();
                HXtoHP(GetXhx(), yhx, ref X, ref Y);
                var stormImageWidth = FStormImages[Frame];
                X = X - (int)(((uint)stormImageWidth.Width) >> 1) + 20;
                var stormImageHeight = FStormImages[Frame];
                Y = Y - stormImageHeight.Height + 32 + 40;
                FStormImages[Frame].Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
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
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection, without clearing its existing hexes. }
            for (int I = 0, last = RadToHN(2) - 1; I <= last; I++)
            {
                AoWHSMap.FTargetSelection.FHNs.Add(0);
            }
        }

        // Storms.pas:443
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            TRect R = default;
            base.InitializeStorm(Player, X, Y, L);
            FStormImages = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "EFFECTS\\LIGHTN.ILB");
            if (GetVisible())
            {
                FStormImages.LoadDiscardableImageData();
            }

            R.Left = -(int)(((uint)FStormImages[0].Width) >> 1);
            R.Right = (int)(((uint)FStormImages[0].Width) >> 1);
            R.Top = -FStormImages[0].Height + 32;
            R.Bottom = 32;
            SetDimensions(R);
            AoWHSMap.ValidateTerrainObservation(AoWHSMap.FCurrentPlayer, X, Y, L, 2);
            PlaceHX(AoWHSMap.MapContainer, X, Y, L);
        }

        // Storms.pas:466
        public override bool UpdateStorm()
        {
            bool Result;
            NewFrame();
            if (FStormImages.GetSafe(FFrameCounter) != null)
            {
                PlaySample("SFX\\ATTACK\\LIGHTNG.WAV");
            }

            if (FStormImages.Count == FFrameCounter)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                TriggerStormDamage(TStormType.stLightning, GetXhx(), yhx, lhx, 0, 1, AoWHSMap.FCurrentPlayer);
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
            AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "EFFECTS\\LIGHTN.ILB");
            base.FinalizeStorm();
        }

        // Animation.pas:43

        // Storms.pas:424
        public override string Name() => TranslateRStr(LightningStormRStr);

    }

    public partial class TFireStorm : TStorm
    {
        public TFireStorm(TECustomNode owner) : base(owner)
        { }

        // Storms.pas:517
        public new const int ClassID = 135742;

        // Storms.pas:527
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Game.Draw.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame;
            TImageSequence Sequence;
            var yhx = GetYhx();
            HXtoHP(GetXhx(), yhx, ref X, ref Y);
            X = X - XhpSurf + 8;
            Y = Y - YhpSurf;
            Sequence = AoWHSSet.FImageSequences.ImageSequences[0];
            while (Sequence != null)
            {
                Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                if (Frame != -1 && Sequence.ImageLibrary.ImageExist(Frame))
                {
                    Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
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
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection, without clearing its existing hexes. }
            for (int I = 0, last = RadToHN(3) - 1; I <= last; I++)
            {
                AoWHSMap.FTargetSelection.FHNs.Add(0);
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
                var lhx = GetLhx();
                var yhx = GetYhx();
                TriggerStormDamage(TStormType.stFire, GetXhx(), yhx, lhx, 0, 2, AoWHSMap.FCurrentPlayer);
                var lhx2 = GetLhx();
                var yhx2 = GetYhx();
                var xhx = GetXhx();
                AoWHSMap.ChangeTerrainEx(this, xhx, yhx2, lhx2, 2, ChangeStormTerrain, null);
            }

            Sequence = AoWHSSet.FImageSequences.ImageSequences[0];
            Frame = -1;
            while (Sequence != null && Frame == -1)
            {
                Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Sequence = Sequence.Next;
            }

            if (Frame == 1)
            {
                bool condition = AoWHSMap.FCurrentPlayer == AoWHSMap.FSeatedPlayer;
                if (!condition)
                {
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    condition = AoWHSMap.TerrainVisible(xhx, yhx, lhx, 2);
                }

                if (condition)
                {
                    var random = Random(2);
                    PlaySample(ExplosionSFX[random]);
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
        public override string Name() => TranslateRStr(FireStormRStr);

    }

    // poison cloud resource
    public partial class TPestilenceStorm : TStorm
    {
        public TPestilenceStorm(TECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135745;

        public TPoisonCloudResource FCloudResource;

        // Storms.pas:644
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Game.Draw.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame;
            int Blend;
            TImageSequence Sequence;
            var yhx = GetYhx();
            HXtoHP(GetXhx(), yhx, ref X, ref Y);
            X = X - XhpSurf + 8;
            Y = Y - YhpSurf;
            Sequence = FCloudResource.ImageSequenceList.ImageSequences[0];
            while (Sequence != null)
            {
                Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                if (Frame != -1 && Sequence.ImageLibrary.ImageExist(Frame))
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

                    Sequence.ImageLibrary[Frame].ShowBlended(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc, ShadowBlendTable.LookupTables[Blend]);
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
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection without clearing its existing hexes. }
            for (int I = 0, last = RadToHN(3) - 1; I <= last; I++)
            {
                AoWHSMap.FTargetSelection.FHNs.Add(0);
            }
        }

        // Storms.pas:668
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            TRect R = default;
            FCloudResource = (TPoisonCloudResource)AoWHSSet.MapObjectResourceList.FindChild(135744);
            base.InitializeStorm(Player, X, Y, L);
            R.Left = -80;
            R.Right = 96;
            R.Top = -80;
            R.Bottom = 96;
            SetDimensions(R);
            AoWHSMap.ValidateTerrainObservation(AoWHSMap.FCurrentPlayer, X, Y, L, 2);
            if (AoWHSMap.FCurrentPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.TerrainVisible(X, Y, L, 2))
            {
                AoWHSSet.FSpellControl.GetSpell(9).FCastSounds.Play(0);
            }

            PlaceHX(AoWHSMap.MapContainer, X, Y, L);
        }

        // Storms.pas:690
        public override bool UpdateStorm()
        {
            bool Result;
            TImageSequence Sequence;
            int Frame;
            TPoisonCloud Cloud;
            NewFrame();
            Sequence = FCloudResource.ImageSequenceList.ImageSequences[0];
            Frame = -1;
            while (Sequence != null && Frame == -1)
            {
                Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Sequence = Sequence.Next;
            }

            if (Frame == -1)
            {
                Cloud = (TPoisonCloud)FCloudResource.CreateResourceUser();
                Cloud.FTurnsRemaining = 3;
                Cloud.FPlayer = AoWHSMap.FCurrentPlayer;
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                if (AoWHSMap.PlaceHX(Cloud, xhx, yhx, lhx))
                {
                    Cloud.TriggerDamage();
                }
                else
                {
                    Cloud?.Dispose();
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
        public override string Name() => TranslateRStr(PestilenceStormRStr);

    }
}

namespace Aow.Game.Commands
{

    // storm, X, Y, layer
    public partial class TStormSpellTE : TSpellTE
    {
        public TStormSpellTE(TECustomNode owner) : base(owner)
        { }

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
        public override bool Process(TAbstractUnit source, int value)
        {
            bool Result;
            base.Process(source, value);
            if (value == 0)
            {
                FStorm = ((TStormSpell)AoWHSSet.FSpellControl.GetSpell(FEventSpellID)).CreateStorm();
                FStorm.InitializeStorm(source.FPlayer, FX, FY, FL);
                AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            }

            if (!FStorm.UpdateStorm())
            {
                FStorm.FinalizeStorm();
                FStorm?.Dispose();
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
}

namespace Aow.Game.Magic.Spells
{

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
        public override void AIPrefetchCastSpellActions(TAbstractUnit Source, TAICastSpellActionList Actions)
        {
            int Priority;
            TAICastSpellAction Action;
            if (AIRequestCastSpellExpense(Source, FBaseManaCost, 0, 0))
            {
                Priority = AICastSpellPriority(Source);
                if (Priority != -1)
                {
                    Action = CreateCastSpellAction();
                    SetupCastSpellAction(Source, Action);
                    Action.FPriority = Priority;
                    Action.SetOwner(Actions);
                }
            }
        }

        // Storms.pas:906
        public override bool AIExecuteCastSpellAction(TAbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            TXYL Target = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TStorm Storm;
            TStormDamageInfo Info = default;
            if (AICastSpellPriority(Source) != -1)
            {
                AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, 0);
                if (((THero)Source).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
                {
                    Source.GetLocation(ref X, ref Y, ref L);
                    Storm = CreateStorm();
                    if (Storm.GetBestTarget(Source.FPlayer, X, Y, L, 12, ref Target, ref Info))
                    {
                        ExecuteStormSpell(Source, Target.X, Target.Y, Target.L);
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }

                    Storm?.Dispose();
                }
                else
                {
                    if (((THero)Source).FCastingSpellID != FSpellID)
                    {
                        ((THero)Source).ExecuteStartCasting(FSpellID);
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
        public override bool CanActivate(TAbstractUnit Source, ref string MessageText)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            bool Result = base.CanActivate(Source, ref MessageText);
            if (Result)
            {
                Source.GetLocation(ref X, ref Y, ref L);
                if (L != 0)
                {
                    Result = false;
                    MessageText = TranslateRStr(CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // Storms.pas:825
        public override bool Activate(TAbstractUnit Source)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            string Reason = "";
            TTargetSelectionControl Selection = default;
            base.Activate(Source);
            if (CanActivate(Source, ref Reason))
            {
                FSource = Source;
                Source.GetLocation(ref X, ref Y, ref L);
                Selection = AoWHSMap.FTargetSelection;
                FTargetStorm = CreateStorm();
                Selection.Initialize(this);
                Selection.FTargetMode = Selection.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmValidateDistance, TTargetSelectionModeElement.tmExternalFloater);
                Selection.FSourceX = X;
                Selection.FSourceY = Y;
                Selection.FSourceL = L;
                AoWHSMap.MakeVisible(X, Y, L, 5);
                var name = FTargetStorm.Name();
                var format = RtlUnit.Format(TranslateRStr(SelectTargetLocationForXOnMapRStr), [name]);
                Selection.FText.Text = format;
                Selection.FTitle = FSpellName;
                Selection.FCancelText = TranslateRStr(CancelRStr);
                Selection.FRange = 12;
                Selection.FOnChange += OnTargetSelectionChange;
                Selection.FSourceImage = AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[20].GetPreviewImage();
                Selection.FValidTargetImage = AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[21].GetPreviewImage();
                Selection.FInvalidTargetImage = AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[22].GetPreviewImage();
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
            bool Result;
            TStormSpellTE TE;
            if (!AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                TE = new TStormSpellTE(null);
                TE.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                TE.FX = X;
                TE.FY = Y;
                TE.FL = L;
                AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(TE);
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
            int Result;
            TXYL Target = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            string Reason = "";
            TStorm Storm;
            TStormDamageInfo Info = default;
            int Damage;
            if (CanActivate(Source, ref Reason))
            {
                Result = -1;
                Source.GetLocation(ref X, ref Y, ref L);
                if (L == 0)
                {
                    Storm = CreateStorm();
                    if (Storm.GetBestTarget(Source.FPlayer, X, Y, L, 12, ref Target, ref Info))
                    {
                        Damage = Info.EnemyDamage - Info.FriendlyDamage;
                        if ((double)((double)FBaseManaCost * 0.75) < Damage)
                        {
                            Result = Damage * 5 + 750;
                        }
                    }

                    Storm?.Dispose();
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Storms.pas:786
        public void OnTargetSelectionChange(TTargetSelectionControl Selection)
        {
            switch (Selection.FChange)
            {
                case TTargetSelectionChange.tscDeactivated:
                    FTargetStorm?.Dispose();
                    FTargetStorm = null;
                    Selection.FOnChange -= OnTargetSelectionChange;
                    break;
                case TTargetSelectionChange.tscSelected:
                    Selection.FOnChange -= OnTargetSelectionChange;
                    if (ExecuteStormSpell(FSource, Selection.FTargetX, Selection.FTargetY, Selection.FTargetL))
                    {
                        Selection.Deactivate();
                    }

                    FTargetStorm?.Dispose();
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
            FSpellName = TranslateRStr(FireStormRStr);
            FSpellID = 3;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msFire;
        }

        // Storms.pas:949
        public override TStorm CreateStorm() => new TFireStorm(null);
    }

    public partial class TDivineStormSpell : TStormSpell
    {
        // Storms.pas:954
        public TDivineStormSpell(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(DivineStormRStr);
            FSpellID = 6;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msLife;
        }

        // Storms.pas:964
        public override TStorm CreateStorm() => new TDivineStorm(null);
    }

    public partial class TDeathStormSpell : TStormSpell
    {
        // Storms.pas:969
        public TDeathStormSpell(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(DeathStormRStr);
            FSpellID = 5;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msDeath;
        }

        // Storms.pas:979
        public override TStorm CreateStorm() => new TDeathStorm(null);
    }

    public partial class TIceStormSpell : TStormSpell
    {
        // Storms.pas:984
        public TIceStormSpell(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(IceStormRStr);
            FSpellID = 4;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msAir;
        }

        // Storms.pas:994
        public override TStorm CreateStorm() => new TIceStorm(null);
    }

    public partial class TLightningStormSpell : TStormSpell
    {
        // Storms.pas:1014
        public TLightningStormSpell(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(LightningStormRStr);
            FSpellID = 7;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msAir;
        }

        // Storms.pas:1024
        public override TStorm CreateStorm() => new TLightningStorm(null);
    }

    public partial class TPestilenceStormSpell : TStormSpell
    {
        // Storms.pas:999
        public TPestilenceStormSpell(TECustomNode owner) : base(owner)
        {
            FSpellName = TranslateRStr(PestilenceRStr);
            FSpellID = 9;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msDeath;
        }

        // Storms.pas:1009
        public override TStorm CreateStorm() => new TPestilenceStorm(null);
    }
}

namespace Aow.Game.Draw.Animation
{

    public static partial class StormsUnit
    {
        // Storms.pas:1029
        public static void RegisterStorms(TEngine AEngine)
        {
            TSpellControl Spells;
            AEngine.RegisterEClasses([EClass.Of<TIceStorm>(), EClass.Of<TDeathStorm>(), EClass.Of<TDivineStorm>(), EClass.Of<TFireStorm>(), EClass.Of<TLightningStorm>(), EClass.Of<TPestilenceStorm>(), EClass.Of<TStormSpellTE>()]);
            Spells = ((TAoWEngine)AEngine).FGameSet.FSpellControl;
            Spells.RegisterSpell(new TFireStormSpell(null));
            Spells.RegisterSpell(new TDeathStormSpell(null));
            Spells.RegisterSpell(new TDivineStormSpell(null));
            Spells.RegisterSpell(new TLightningStormSpell(null));
            Spells.RegisterSpell(new TIceStormSpell(null));
            Spells.RegisterSpell(new TPestilenceStormSpell(null));
        }

        // Storms.pas:1044
        public static void UnRegisterStorms(TEngine AEngine)
        {
            TSpellControl Spells;
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
