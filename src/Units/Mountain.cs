// Translated from Mountain.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.GlobalTarget;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells
{

    public enum AnonymousEnum_rmRaising : byte
    {
        Raising = 0
    }
}

namespace Aow.Game.World
{

    public partial class MountainMO : AoWTerrain
    {
        public MountainMO(ECustomNode owner) : base(owner)
        { }

        // Mountain.pas:101
        public new const int ClassID = 131432;

        // Mountain.pas:104
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = FResource.TerrainList.Overlay[Index];
            if (Result >= 0)
            {
                Result = 0;
            }

            return Result;
        }

        // Mountain.pas:110
        public override bool CanChangeTerrain(in HSSenderInfo Info) => true;

        // Mountain.pas:136
        public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            int I;
            bool Valid;
            TerrainList List;
            MapField MapField;
            int X = default;
            int Y = default;
            HexagonSprite HS;
            I = 0;
            Valid = true;
            List = FResource.TerrainList;
            while (Valid && I < List.Count)
            {
                if (List.Terrain[I] >= 0 || List.Overlay[I] >= 0)
                {
                    HexGeometry.CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
                    if (X >= 0 && Y >= 0 && X < MapContainer.Width && Y < MapContainer.Height)
                    {
                        MapField = MapContainer.GetField(X, Y, Lhx);
                        HS = MapField.GetMaxLevelNoneTransparentHS();
                        if (HS != null && HS.EClass.ClassID == 131432)
                        {
                            Valid = ValidMountainOverlap(MapField, List.Overlay[I]);
                        }
                        else
                        {
                            Valid = HS == null || HS.GetLevel() < 10;
                        }
                    }
                }

                I += 1;
            }

            return Valid && MapContainer.Map.CanChangeTerrain(this, Xhx, Yhx, Lhx, FResource.TerrainList);
        }

        // Mountain.pas:113
        public bool ValidMountainOverlap(MapField MapField, sbyte Overlay)
        {
            bool Valid;
            int I;
            MapObject HS;
            sbyte OtherOverlay;
            Valid = true;
            I = 0;
            while (I != MapField.Count && Valid)
            {
                HS = (MapObject)MapField.HSList[I];
                if (HS.EClass.ClassID == 131432)
                {
                    var hNofHS = MapField.GetHNofHS(HS);
                    OtherOverlay = HS.Resource.TerrainList.Overlay[hNofHS];
                    Valid = !((OtherOverlay == 0 && Overlay == 0) || (OtherOverlay == 1 && Overlay == 1));
                }

                I += 1;
            }

            return Valid;
        }

    }

    public partial class MountainMOR : AoWTerrain.Resource
    {
        public MountainMOR(ECustomNode owner) : base(owner)
        { }

        // Mountain.pas:169
        public new const int ClassID = 131433;

        // Mountain.pas:175
        public override int ResourceUserClassID() => 131432;

        // Mountain.pas:172
        public override string ResourceEditName() => AbilityRules.TranslateRStr(GameText.MountainRStr);

    }
}

namespace Aow.Game.World.Terrain
{

    // FMode, FPlayer, FDuration, FOriginalOverlay
    public partial class RaisedMountain : ILTerrainMO
    {
        public RaisedMountain(ECustomNode owner) : base(owner)
        { }

        // Mountain.pas:178
        public new const int ClassID = 131434;

        public Set<AnonymousEnum_rmRaising> FMode;
        public sbyte FPlayer;
        public sbyte FDuration;
        public sbyte FOriginalOverlay;

        // Mountain.pas:181
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, -1, 40);
            Stream.rwShortintEx(ref FDuration, -1, 41);
            Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
        }

        // Mountain.pas:210
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Payload.Int);
            }
        }

        // Mountain.pas:223
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(AnonymousEnum_rmRaising.Raising))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // Mountain.pas:216
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4402)
            {
                if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // Mountain.pas:189
        public void RestoreTerrain()
        {
            MapField Field;
            sbyte Overlay;
            Field = (MapField)Owner;
            Overlay = FOriginalOverlay;
            this.Dispose();
            if (Overlay > 0)
            {
                AoWHSMap.Current.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
            }
        }

        // Mountain.pas:201
        public void NewTurn(sbyte Player)
        {
            if (Player == FPlayer && FDuration > 0)
            {
                FDuration = (sbyte)(FDuration - 1);
                if (FDuration == 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // Mountain.pas:228
        public void ShowRaiseTerrainAnimation(Animation Anim, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            MapField Field;
            var lhx = Anim.GetLhx();
            var yhx = Anim.GetYhx();
            var xhx = Anim.GetXhx();
            Field = AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
            // { Original tests the base mountain ID, although this object is raised terrain. }
            if (this == Field.FindOwnedChild(131432) && Anim.FFrameCounter == 0)
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
                    AoWHSSet.Current.FSpellControl.GetSpell(19).FCastSounds.Play(0);
                }
            }
        }

        // Mountain.pas:240
        public void NewFrameRaiseTerrainAnimation(Animation Anim, int FrameCounter)
        {
            var lhx = Anim.GetLhx();
            var yhx = Anim.GetYhx();
            var xhx = Anim.GetXhx();
            var field = AoWHSMap.Current.MapContainer.GetField(xhx, yhx, lhx);
            if (field.FindOwnedChild(131434) == this && FrameCounter == 9)
            {
                FMode = FMode - Set<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.Raising);
            }
        }

        public new partial class Resource : ILTerrainMOR
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            // Mountain.pas:249
            public new const int ClassID = 131435;

            // Mountain.pas:255
            public override int ResourceUserClassID() => 131434;

            // Mountain.pas:252
            public override string ResourceEditName() => AbilityRules.TranslateRStr(RaisedMountainRStr);

            private static readonly string RaisedMountainRStr = "Raised Mountain";

        }

    }
}

namespace Aow.Game.Commands
{

    public partial class RaiseTerrainTE : GlobalTargetSpellTE
    {
        public RaiseTerrainTE(ECustomNode owner) : base(owner)
        { }

        // Mountain.pas:258
        public new const int ClassID = 135946;

        // Mountain.pas:317
        public override bool Process(AbstractUnit source, int value)
        {
            base.Process(source, value);
            RaiseTerrain(source);
            AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, Unsafe.As<XYL, Location>(ref FLocation).X, Unsafe.As<XYL, Location>(ref FLocation).Y, Unsafe.As<XYL, Location>(ref FLocation).Layer, 1);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            return false;
        }

        // Mountain.pas:261
        public void RaiseTerrain(AbstractUnit Caster)
        {
            DelphiList Resources = default;
            MapLevel Level = default;
            HNWalkData Walk = default;
            AoWMapField Field = default;
            RaisedMountain Mountain = default;
            MapObjectResource Resource = default;
            int I = default;
            bool Found = default;
            ImageSequenceAnimation Anim = default;
            Resources = AoWHSSet.Current.MapObjectResourceList.FChildren;
            Level = AoWHSMap.Current.MapContainer.MapLevels[Unsafe.As<XYL, Location>(ref FLocation).Layer];
            HNWalkData.InitHNWalk(ref Walk, Unsafe.As<XYL, Location>(ref FLocation).X, Unsafe.As<XYL, Location>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Field = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                    Resource = null;
                    if (((GlobalTargetSpell)AoWHSSet.Current.FSpellControl.GetSpell(19)).ValidTargetMapF(Field))
                    {
                        Found = false;
                        I = Resources.Count;
                        while (!Found && I != 0)
                        {
                            I -= 1;
                            Resource = (MapObjectResource)Resources.List[I];
                            Found = Resource.EClass.ClassID == 131435 && Resource.TerrainList.UsedTerrain() == 1;
                        }

                        if (Found)
                        {
                            Field.RestoreTerrain();
                            Mountain = (RaisedMountain)Resource.CreateResourceUser();
                            Mountain.FDuration = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                            Mountain.FPlayer = Caster.FPlayer;
                            Mountain.FOriginalOverlay = Field.OverlayType;
                            AoWHSMap.Current.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<XYL, Location>(ref FLocation).Layer, 0, -1);
                            if (AoWHSMap.Current.PlaceHX(Mountain, Walk.X, Walk.Y, Unsafe.As<XYL, Location>(ref FLocation).Layer))
                            {
                                Mountain.FMode = Mountain.FMode + Set<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.Raising);
                                Anim = new ImageSequenceAnimation(null);
                                Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(19).FCastImages.ImageSequences[0]);
                                Anim.FOnShow = Mountain.ShowRaiseTerrainAnimation;
                                Anim.FOnNewFrame = Mountain.NewFrameRaiseTerrainAnimation;
                                Anim.FFrameCounter = -AoWHSMap.Current.Random(15);
                                AoWHSMap.Current.PlaceHX(Anim, Walk.X, Walk.Y, Unsafe.As<XYL, Location>(ref FLocation).Layer);
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

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class RaiseTerrain : GlobalTargetSpell
    {
        public RaiseTerrain(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(RaiseTerrainRStr);
            FSpellID = 19;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Earth;
            FAboveGroundOnly = true;
        }

        // Mountain.pas:327

        // Mountain.pas:364
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FHNs.Clear();
            for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                TargetSelection.FHNs.Add(0);
            }
        }

        // Mountain.pas:373
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            RaiseTerrainTE Event;
            Event = new RaiseTerrainTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // Mountain.pas:347
        public override bool ValidTargetSelectionMapF(MapField Field)
        {
            bool Valid;
            ArmyHS Army;
            Valid = base.ValidTargetSelectionMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).Contains((byte)Field.OverlayType) && Field.FindNoneTransparentHS(typeof(Structure)) == null;
            if (Valid)
            {
                Army = ((AoWMapField)Field).GetArmyHS();
                if (Army != null)
                {
                    Valid = !Army.VisibleForSeatedPlayer();
                }
            }

            return Valid;
        }

        // Mountain.pas:338
        public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).Contains((byte)Field.OverlayType) && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;

        private static readonly string RaiseTerrainRStr = "Raise Terrain";

        // Mountain.pas:77

        // Mountain.pas:77

    }
}

namespace Aow.Game.Magic.Spells
{

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Location
    {
        [FieldOffset(0)]
        public sbyte X;
        [FieldOffset(1)]
        public sbyte Y;
        [FieldOffset(2)]
        public sbyte Layer;
        [FieldOffset(3)]
        public sbyte Reserved;
    }
}
