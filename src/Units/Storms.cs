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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Animation
{

    // player
    public partial class Storm : Animation
    {
        public Storm(ECustomNode owner) : base(owner)
        { }

        public sbyte FPlayer;

        // Storms.pas:34
        public virtual void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Storms.pas:236
        public virtual void UpdateTargetSelection(TargetSelectionControl Selection)
        {
            Selection.FOnShow = ShowTargetSelection;
            Selection.FTargetMode = Selection.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.OwnerDraw);
        }

        // Storms.pas:209
        public virtual void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            FPlayer = Player;
            FAnimationLevel = 28;
            Aow.Game.World.Map.AoWHSMap.Current.LockExecuteEventLog();
        }

        // Storms.pas:216
        public virtual bool UpdateStorm() => false;

        // Storms.pas:221
        public virtual void FinalizeStorm()
        {
            Aow.Game.World.Map.AoWHSMap.Current.UnlockExecuteEventLog();
        }

        // Storms.pas:226
        public void ShowTargetSelection(TargetSelectionControl Selection, int HN, int Xs, int Ys, in SurfaceDesc SurfaceDesc)
        {
            int Intensity;
            HN = Aow.Game.World.Map.HexGeometry.HNtoRad(HN);
            Intensity = Math.Abs((AoWHSMap.Current.Frame + HN) % 9 - 4) * 20 + 20;
            var lookupTable = Aow.Game.Draw.GfxState.IntensityBlendTable.GetLookupTables(Intensity);
            Selection.FImageLibrary[0].ShowBlended(Xs - 8, Ys, SurfaceDesc, lookupTable);
        }

        // Storms.pas:242
        public bool GetBestTarget(sbyte Player, int X, int Y, int L, int Range, ref XYL Target, ref StormDamageInfo Info)
        {
            StormDamageInfo Candidate;
            HNWalkData Walk = default;
            MapLevel Level;
            ArmyHS ArmyHS;
            Level = AoWHSMap.Current.MapContainer.MapLevels[L];
            Target.Packed = 16777215;
            bool Result = false;
            Info = default;
            Candidate = default;
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Range)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    ArmyHS = ((AoWMapField)Level.Field[Walk.X, Walk.Y]).GetArmyHS();
                    if (ArmyHS != null && ArmyHS.FArmy.FPlayer != 0 && Aow.Game.Magic.Abilities.AbilityRules.ValidAttackRelation(Player, 0, ArmyHS.FArmy.FPlayer, 0))
                    {
                        GetTargetDamageInfo(Player, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L, ref Candidate);
                        if (Info.EnemyDamage - Info.FriendlyDamage < Candidate.EnemyDamage - Candidate.FriendlyDamage)
                        {
                            Info = Candidate;
                            Target = Aow.Game.World.Map.HexGeometry.PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                            Result = true;
                        }
                    }
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // Storms.pas:39
        public virtual string Name()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

    }

    // storm image library
    public partial class BlastStorm : Storm
    {
        public BlastStorm(ECustomNode owner) : base(owner)
        { }

        public CustomImageLibrary FStormImages;

        // Storms.pas:338
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame;
            Frame = FFrameCounter % FStormImages.Count;
            var yhx = GetYhx();
            Aow.Game.World.Map.HexGeometry.HXtoHP(GetXhx(), yhx, ref X, ref Y);
            var stormImageWidth = FStormImages[Frame];
            X = X - (int)(((uint)stormImageWidth.Width) >> 1) + 10;
            var stormImageHeight = FStormImages[Frame];
            Y = Y - (int)(((uint)stormImageHeight.Height) >> 1) - 42;
            FStormImages[Frame].Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
        }

        // Storms.pas:358
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
        {
            Aow.Game.Magic.Abilities.AbilityRules.RetrieveStormDamageInfo(Player, StormType(), X, Y, L, 0, 4, ref Info);
        }

        // Storms.pas:349
        public override void UpdateTargetSelection(TargetSelectionControl Selection)
        {
            base.UpdateTargetSelection(Selection);
            Selection.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(5) - 1; I <= last; I++)
            {
                Selection.FHNs.Add(0);
            }
        }

        // Storms.pas:276
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            Rect R = default;
            base.InitializeStorm(Player, X, Y, L);
            var stormFileName = StormFileName();
            FStormImages = Aow.Game.Engine.AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), stormFileName);
            if (GetVisible())
            {
                FStormImages.LoadDiscardableImageData();
            }

            R.Left = -(int)(((uint)FStormImages[0].Width) >> 1);
            R.Right = (int)(((uint)FStormImages[0].Width) >> 1);
            R.Top = -(int)(((uint)FStormImages[0].Height) >> 1);
            R.Bottom = (int)(((uint)FStormImages[0].Height) >> 1);
            SetDimensions(R);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.FCurrentPlayer, X, Y, L, 4);
            if (AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer || Aow.Game.World.Map.AoWHSMap.Current.TerrainVisible(X, Y, L, 4))
            {
                Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\ATTACK\\STORMBIG.WAV");
            }

            PlaceHX(AoWHSMap.Current.MapContainer, X, Y, L);
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
                Aow.Game.World.Map.AoWHSMap.Current.ChangeTerrainEx(null, xhx, yhx, lhx, 1, ChangeStormTerrain, StormTerrainChanged);
            }
            else
            {
                if (FFrameCounter == 12)
                {
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    Aow.Game.World.Map.AoWHSMap.Current.ChangeTerrainEx(null, xhx, yhx, lhx, 2, ChangeStormTerrain, StormTerrainChanged);
                }
                else
                {
                    if (FFrameCounter == 18)
                    {
                        var lhx = GetLhx();
                        var yhx = GetYhx();
                        var xhx = GetXhx();
                        Aow.Game.World.Map.AoWHSMap.Current.ChangeTerrainEx(null, xhx, yhx, lhx, 3, ChangeStormTerrain, StormTerrainChanged);
                    }
                    else
                    {
                        if (FFrameCounter == 22)
                        {
                            var lhx = GetLhx();
                            var yhx = GetYhx();
                            var xhx = GetXhx();
                            Aow.Game.World.Map.AoWHSMap.Current.ChangeTerrainEx(null, xhx, yhx, lhx, 4, ChangeStormTerrain, StormTerrainChanged);
                        }
                    }
                }
            }

            if (FStormImages.Count == FFrameCounter)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                Aow.Game.Magic.Abilities.AbilityRules.TriggerStormDamage(StormType(), xhx, yhx, lhx, 0, 4, AoWHSMap.Current.FCurrentPlayer);
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
            Aow.Game.Engine.AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), stormFileName);
            base.FinalizeStorm();
        }

        // Storms.pas:304
        public virtual void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
        }

        // Storms.pas:308
        public virtual void StormTerrainChanged(MapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
        }

        // Storms.pas:294
        public virtual string StormFileName() => "";

        // Storms.pas:299
        public virtual StormType StormType() => Magic.Spells.StormType.None;
    }

    public partial class IceStorm : BlastStorm
    {
        public IceStorm(ECustomNode owner) : base(owner)
        { }

        // Storms.pas:363
        public new const int ClassID = 135738;

        // Storms.pas:390
        public override void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            FrozenWaterHS Ice;
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
                    if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)TerrainType))
                    {
                        Ice = (FrozenWaterHS)MapF.FindOwnedHS(typeof(FrozenWaterHS));
                        if (Ice != null)
                        {
                            Ice.FDaysRemaining = (sbyte)(3 + (Aow.Game.World.Map.AoWHSMap.Current.Random(11) - 5) / 5);
                            Ice.FCasterPlayer = FPlayer;
                        }
                    }
                }
            }
        }

        // Storms.pas:373
        public override void StormTerrainChanged(MapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
            FrozenWaterHS Ice;
            if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)MapF.TerrainType) && !Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)OriginalTerrainType))
            {
                Ice = (FrozenWaterHS)MapF.FindOwnedHS(typeof(FrozenWaterHS));
                if (Ice == null)
                {
                    Ice = new FrozenWaterHS(null);
                    Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                }

                Ice.FDaysRemaining = (sbyte)(3 + (Aow.Game.World.Map.AoWHSMap.Current.Random(11) - 5) / 5);
                Ice.FCasterPlayer = FPlayer;
            }
        }

        // Storms.pas:409
        public override string StormFileName() => "EFFECTS\\STORM1.ILB";

        // Storms.pas:414
        public override StormType StormType() => Magic.Spells.StormType.Ice;

        // Animation.pas:43

        // Storms.pas:368
        public override string Name() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.IceStormRStr);

    }

    public partial class DeathStorm : BlastStorm
    {
        public DeathStorm(ECustomNode owner) : base(owner)
        { }

        // Storms.pas:492
        public new const int ClassID = 135741;

        // Storms.pas:502
        public override void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (TerrainType != 0 && TerrainType != 6)
            {
                TerrainType = 5;
            }
        }

        // Storms.pas:507
        public override string StormFileName() => "EFFECTS\\STORMD.ILB";

        // Storms.pas:512
        public override StormType StormType() => Magic.Spells.StormType.Death;

        // Animation.pas:43

        // Storms.pas:497
        public override string Name() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.DeathStormRStr);

    }

    public partial class DivineStorm : BlastStorm
    {
        public DivineStorm(ECustomNode owner) : base(owner)
        { }

        // Storms.pas:608
        public new const int ClassID = 135740;

        // Storms.pas:618
        public override void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (TerrainType != 6 && TerrainType != 0)
            {
                TerrainType = 1;
            }
        }

        // Storms.pas:624
        public override string StormFileName() => "EFFECTS\\STORMLGT.ILB";

        // Storms.pas:629
        public override StormType StormType() => Magic.Spells.StormType.Holy;

        // Animation.pas:43

        // Storms.pas:613
        public override string Name() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.DivineStormRStr);

    }

    // storm image library
    public partial class LightningStorm : Storm
    {
        public LightningStorm(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135739;

        public CustomImageLibrary FStormImages;

        // Storms.pas:429
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame;
            Frame = FFrameCounter % FStormImages.Count;
            if (FStormImages[Frame] != null)
            {
                var yhx = GetYhx();
                Aow.Game.World.Map.HexGeometry.HXtoHP(GetXhx(), yhx, ref X, ref Y);
                var stormImageWidth = FStormImages[Frame];
                X = X - (int)(((uint)stormImageWidth.Width) >> 1) + 20;
                var stormImageHeight = FStormImages[Frame];
                Y = Y - stormImageHeight.Height + 32 + 40;
                FStormImages[Frame].Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
            }
        }

        // Storms.pas:487
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
        {
            Aow.Game.Magic.Abilities.AbilityRules.RetrieveStormDamageInfo(Player, StormType.Lightning, X, Y, L, 0, 1, ref Info);
        }

        // Storms.pas:478
        public override void UpdateTargetSelection(TargetSelectionControl Selection)
        {
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection, without clearing its existing hexes. }
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FTargetSelection.FHNs.Add(0);
            }
        }

        // Storms.pas:443
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            Rect R = default;
            base.InitializeStorm(Player, X, Y, L);
            FStormImages = Aow.Game.Engine.AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "EFFECTS\\LIGHTN.ILB");
            if (GetVisible())
            {
                FStormImages.LoadDiscardableImageData();
            }

            R.Left = -(int)(((uint)FStormImages[0].Width) >> 1);
            R.Right = (int)(((uint)FStormImages[0].Width) >> 1);
            R.Top = -FStormImages[0].Height + 32;
            R.Bottom = 32;
            SetDimensions(R);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.FCurrentPlayer, X, Y, L, 2);
            PlaceHX(AoWHSMap.Current.MapContainer, X, Y, L);
        }

        // Storms.pas:466
        public override bool UpdateStorm()
        {
            bool Result;
            NewFrame();
            if (FStormImages.GetSafe(FFrameCounter) != null)
            {
                Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\ATTACK\\LIGHTNG.WAV");
            }

            if (FStormImages.Count == FFrameCounter)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                Aow.Game.Magic.Abilities.AbilityRules.TriggerStormDamage(StormType.Lightning, GetXhx(), yhx, lhx, 0, 1, AoWHSMap.Current.FCurrentPlayer);
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
            Aow.Game.Engine.AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "EFFECTS\\LIGHTN.ILB");
            base.FinalizeStorm();
        }

        // Animation.pas:43

        // Storms.pas:424
        public override string Name() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.LightningStormRStr);

    }

    public partial class FireStorm : Storm
    {
        public FireStorm(ECustomNode owner) : base(owner)
        { }

        // Storms.pas:517
        public new const int ClassID = 135742;

        // Storms.pas:527
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame;
            ImageSequence Sequence;
            var yhx = GetYhx();
            Aow.Game.World.Map.HexGeometry.HXtoHP(GetXhx(), yhx, ref X, ref Y);
            X = X - XhpSurf + 8;
            Y = Y - YhpSurf;
            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[0];
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
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
        {
            Aow.Game.Magic.Abilities.AbilityRules.RetrieveStormDamageInfo(Player, StormType.Fire, X, Y, L, 0, 2, ref Info);
        }

        // Storms.pas:594
        public override void UpdateTargetSelection(TargetSelectionControl Selection)
        {
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection, without clearing its existing hexes. }
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(3) - 1; I <= last; I++)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FTargetSelection.FHNs.Add(0);
            }
        }

        // Storms.pas:545
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            Rect R = default;
            base.InitializeStorm(Player, X, Y, L);
            R.Left = -80;
            R.Right = 96;
            R.Top = -80;
            R.Bottom = 96;
            SetDimensions(R);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.FCurrentPlayer, X, Y, L, 2);
            PlaceHX(AoWHSMap.Current.MapContainer, X, Y, L);
        }

        // Storms.pas:569
        public override bool UpdateStorm()
        {
            bool Result = default;
            ImageSequence Sequence = default;
            int Frame = default;
            NewFrame();
            if (FFrameCounter == 20)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                Aow.Game.Magic.Abilities.AbilityRules.TriggerStormDamage(StormType.Fire, GetXhx(), yhx, lhx, 0, 2, AoWHSMap.Current.FCurrentPlayer);
                var lhx2 = GetLhx();
                var yhx2 = GetYhx();
                var xhx = GetXhx();
                Aow.Game.World.Map.AoWHSMap.Current.ChangeTerrainEx(this, xhx, yhx2, lhx2, 2, ChangeStormTerrain, null);
            }

            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[0];
            Frame = -1;
            while (Sequence != null && Frame == -1)
            {
                Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                Sequence = Sequence.Next;
            }

            if (Frame == 1)
            {
                bool condition = AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer;
                if (!condition)
                {
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    condition = Aow.Game.World.Map.AoWHSMap.Current.TerrainVisible(xhx, yhx, lhx, 2);
                }

                if (condition)
                {
                    var random = Random(2);
                    Aow.Game.Audio.AudioPlayback.PlaySample(AbilityRules.ExplosionSFX[random]);
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
        public void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (TerrainType == 6)
            {
                TerrainType = 0;
            }
        }

        // Animation.pas:43

        // Storms.pas:522
        public override string Name() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.FireStormRStr);

    }

    // poison cloud resource
    public partial class PestilenceStorm : Storm
    {
        public PestilenceStorm(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135745;

        public PoisonCloud.Resource FCloudResource;

        // Storms.pas:644
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int Frame;
            int Blend;
            ImageSequence Sequence;
            var yhx = GetYhx();
            Aow.Game.World.Map.HexGeometry.HXtoHP(GetXhx(), yhx, ref X, ref Y);
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

                    Sequence.ImageLibrary[Frame].ShowBlended(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc, GfxState.ShadowBlendTable.LookupTables[Blend]);
                }

                Sequence = Sequence.Next;
            }
        }

        // Storms.pas:725
        public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
        {
            Aow.Game.Magic.Abilities.AbilityRules.RetrieveStormDamageInfo(Player, StormType.Pestilence, X, Y, L, 0, 2, ref Info);
        }

        // Storms.pas:716
        public override void UpdateTargetSelection(TargetSelectionControl Selection)
        {
            base.UpdateTargetSelection(Selection);
            // { Original adds to the map selection without clearing its existing hexes. }
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(3) - 1; I <= last; I++)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FTargetSelection.FHNs.Add(0);
            }
        }

        // Storms.pas:668
        public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
        {
            Rect R = default;
            FCloudResource = (PoisonCloud.Resource)Aow.Game.Engine.AoWHSSet.Current.MapObjectResourceList.FindChild(135744);
            base.InitializeStorm(Player, X, Y, L);
            R.Left = -80;
            R.Right = 96;
            R.Top = -80;
            R.Bottom = 96;
            SetDimensions(R);
            Aow.Game.World.Map.AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.FCurrentPlayer, X, Y, L, 2);
            if (AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer || Aow.Game.World.Map.AoWHSMap.Current.TerrainVisible(X, Y, L, 2))
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(9).FCastSounds.Play(0);
            }

            PlaceHX(AoWHSMap.Current.MapContainer, X, Y, L);
        }

        // Storms.pas:690
        public override bool UpdateStorm()
        {
            bool Result;
            ImageSequence Sequence;
            int Frame;
            PoisonCloud Cloud;
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
                Cloud = (PoisonCloud)FCloudResource.CreateResourceUser();
                Cloud.FTurnsRemaining = 3;
                Cloud.FPlayer = AoWHSMap.Current.FCurrentPlayer;
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                if (Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Cloud, xhx, yhx, lhx))
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
        public override string Name() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.PestilenceStormRStr);

    }
}

namespace Aow.Game.Commands
{

    // storm, X, Y, layer
    public partial class StormSpellTE : SpellTE
    {
        public StormSpellTE(ECustomNode owner) : base(owner)
        { }

        // Storms.pas:730
        public new const int ClassID = 135937;

        public Storm FStorm;
        public sbyte FX;
        public sbyte FY;
        public sbyte FL;

        // Storms.pas:735
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FX), 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FY), 21);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FL), 22);
        }

        // Storms.pas:743
        public override bool Process(AbstractUnit source, int value)
        {
            bool Result;
            base.Process(source, value);
            if (value == 0)
            {
                FStorm = ((StormSpell)Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).CreateStorm();
                FStorm.InitializeStorm(source.FPlayer, FX, FY, FL);
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
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
    public partial class StormSpell : Spell
    {
        public Storm FTargetStorm;
        public AbstractUnit FSource;

        // Storms.pas:762
        public StormSpell(ECustomNode owner) : base(owner)
        {
            FSpellType = SpellType.Global;
        }

        // Storms.pas:888
        public override void AIPrefetchCastSpellActions(AbstractUnit Source, AICastSpellActionList Actions)
        {
            int Priority;
            AICastSpellAction Action;
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
        public override bool AIExecuteCastSpellAction(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            XYL Target = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Storm Storm;
            StormDamageInfo Info = default;
            if (AICastSpellPriority(Source) != -1)
            {
                AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, 0);
                if (((Hero)Source).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
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
                    if (((Hero)Source).FCastingSpellID != FSpellID)
                    {
                        ((Hero)Source).ExecuteStartCasting(FSpellID);
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
        public override bool CanActivate(AbstractUnit Source, ref string MessageText)
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
                    MessageText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // Storms.pas:825
        public override bool Activate(AbstractUnit Source)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            string Reason = "";
            TargetSelectionControl Selection = default;
            base.Activate(Source);
            if (CanActivate(Source, ref Reason))
            {
                FSource = Source;
                Source.GetLocation(ref X, ref Y, ref L);
                Selection = AoWHSMap.Current.FTargetSelection;
                FTargetStorm = CreateStorm();
                Selection.Initialize(this);
                Selection.FTargetMode = Selection.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
                Selection.FSourceX = X;
                Selection.FSourceY = Y;
                Selection.FSourceL = L;
                Aow.Game.World.Map.AoWHSMap.Current.MakeVisible(X, Y, L, 5);
                var name = FTargetStorm.Name();
                var format = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.SelectTargetLocationForXOnMapRStr), [name]);
                Selection.FText.Text = format;
                Selection.FTitle = FSpellName;
                Selection.FCancelText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.CancelRStr);
                Selection.FRange = 12;
                Selection.FOnChange += OnTargetSelectionChange;
                Selection.FSourceImage = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[20].GetPreviewImage();
                Selection.FValidTargetImage = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[21].GetPreviewImage();
                Selection.FInvalidTargetImage = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[22].GetPreviewImage();
                Selection.Activate();
                FTargetStorm.UpdateTargetSelection(AoWHSMap.Current.FTargetSelection);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Storms.pas:156
        public virtual Storm CreateStorm()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // Storms.pas:768
        public virtual bool ExecuteStormSpell(AbstractUnit Source, sbyte X, sbyte Y, sbyte L)
        {
            bool Result;
            StormSpellTE TE;
            if (!Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                TE = new StormSpellTE(null);
                TE.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                TE.FX = X;
                TE.FY = Y;
                TE.FL = L;
                Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(TE);
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
        public virtual int AICastSpellPriority(AbstractUnit Source)
        {
            int Result;
            XYL Target = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            string Reason = "";
            Storm Storm;
            StormDamageInfo Info = default;
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
        public void OnTargetSelectionChange(TargetSelectionControl Selection)
        {
            switch (Selection.FChange)
            {
                case TargetSelectionChange.Deactivated:
                    FTargetStorm?.Dispose();
                    FTargetStorm = null;
                    Selection.FOnChange -= OnTargetSelectionChange;
                    break;
                case TargetSelectionChange.Selected:
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

    public partial class FireStormSpell : StormSpell
    {
        // Storms.pas:939
        public FireStormSpell(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.FireStormRStr);
            FSpellID = 3;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Fire;
        }

        // Storms.pas:949
        public override Storm CreateStorm() => new FireStorm(null);
    }

    public partial class DivineStormSpell : StormSpell
    {
        // Storms.pas:954
        public DivineStormSpell(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.DivineStormRStr);
            FSpellID = 6;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Life;
        }

        // Storms.pas:964
        public override Storm CreateStorm() => new DivineStorm(null);
    }

    public partial class DeathStormSpell : StormSpell
    {
        // Storms.pas:969
        public DeathStormSpell(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.DeathStormRStr);
            FSpellID = 5;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Death;
        }

        // Storms.pas:979
        public override Storm CreateStorm() => new DeathStorm(null);
    }

    public partial class IceStormSpell : StormSpell
    {
        // Storms.pas:984
        public IceStormSpell(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.IceStormRStr);
            FSpellID = 4;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Air;
        }

        // Storms.pas:994
        public override Storm CreateStorm() => new IceStorm(null);
    }

    public partial class LightningStormSpell : StormSpell
    {
        // Storms.pas:1014
        public LightningStormSpell(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.LightningStormRStr);
            FSpellID = 7;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Air;
        }

        // Storms.pas:1024
        public override Storm CreateStorm() => new LightningStorm(null);
    }

    public partial class PestilenceStormSpell : StormSpell
    {
        // Storms.pas:999
        public PestilenceStormSpell(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(PestilenceRStr);
            FSpellID = 9;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Death;
        }

        // Storms.pas:1009
        public override Storm CreateStorm() => new PestilenceStorm(null);

        private static readonly string PestilenceRStr = "Pestilence";
    }
}
