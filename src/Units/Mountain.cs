// Translated from Mountain.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.ILTer;
using Aow.Units.AoWTer;
using Aow.Units.GlobalTargetSpells;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Mountain
{
    public enum AnonymousEnum_rmRaising : byte
    {
        rmRaising = 0
    }

    public partial class TMountainMO : TAoWTerrain
    {
        public TMountainMO(TECustomNode owner) : base(owner)
        {}

        // Mountain.pas:101
        public new const int ClassID = 131432;

        // Mountain.pas:104
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            var call__1 = FResource.TerrainList.Overlay[Index];
            Result = call__1;
            if (Result >= 0)
            {
                Result = 0;
            }

            return Result;
        }

        // Mountain.pas:110
        public override bool CanChangeTerrain(in THSSenderInfo Info)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // Mountain.pas:136
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            int I = default;
            bool Valid = default;
            TTerrainList List = default;
            TMapField MapField = default;
            int X = default;
            int Y = default;
            THexagonSprite HS = default;
            I = 0;
            Valid = true;
            List = FResource.TerrainList;
            while (Valid && I < List.Count)
            {
                var call__2 = List.Terrain[I];
                bool bool__3 = call__2 >= 0;
                if (!bool__3)
                {
                    var call__4 = List.Overlay[I];
                    bool__3 = call__4 >= 0;
                }

                if (bool__3)
                {
                    CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
                    if (X >= 0 && Y >= 0 && X < MapContainer.Width && Y < MapContainer.Height)
                    {
                        MapField = MapContainer.GetField(X, Y, Lhx);
                        HS = MapField.GetMaxLevelNoneTransparentHS();
                        if (HS != null && HS.EClass.ClassID == 131432)
                        {
                            var call__5 = List.Overlay[I];
                            var call__6 = ValidMountainOverlap(MapField, call__5);
                            Valid = call__6;
                        }
                        else
                        {
                            Valid = HS == null || HS.GetLevel() < 10;
                        }
                    }
                }

                I += 1;
            }

            bool bool__7 = Valid;
            if (bool__7)
            {
                var call__8 = MapContainer.Map.CanChangeTerrain(this, Xhx, Yhx, Lhx, FResource.TerrainList);
                bool__7 = call__8;
            }

            Valid = bool__7;
            Result = Valid;
            return Result;
        }

        // Mountain.pas:113
        public bool ValidMountainOverlap(TMapField MapField, sbyte Overlay)
        {
            bool Result = default;
            bool Valid = default;
            int I = default;
            TMapObject HS = default;
            sbyte OtherOverlay = default;
            Valid = true;
            I = 0;
            while (true)
            {
                var call__9 = MapField.Count;
                if (!(I != call__9 && Valid))
                    break;
                {
                    HS = (TMapObject)MapField.HSList[I];
                    if (HS.EClass.ClassID == 131432)
                    {
                        var call__10 = MapField.GetHNofHS(HS);
                        var call__11 = HS.Resource.TerrainList.Overlay[call__10];
                        OtherOverlay = call__11;
                        Valid = !((OtherOverlay == 0 && Overlay == 0) || (OtherOverlay == 1 && Overlay == 1));
                    }

                    I += 1;
                }
            }

            Result = Valid;
            return Result;
        }

    }

    public partial class TMountainMOR : TAoWTerrainResource
    {
        public TMountainMOR(TECustomNode owner) : base(owner)
        {}

        // Mountain.pas:169
        public new const int ClassID = 131433;

        // Mountain.pas:175
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131432;
            return Result;
        }

        // Mountain.pas:172
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__12 = MountainRStr;
            var call__13 = TranslateRStr(call__12);
            Result = call__13;
            return Result;
        }

    }

    // FMode, FPlayer, FDuration, FOriginalOverlay
    public partial class TRaisedMountain : TILTerrainMO
    {
        public TRaisedMountain(TECustomNode owner) : base(owner)
        {}

        // Mountain.pas:178
        public new const int ClassID = 131434;

        public TSet<AnonymousEnum_rmRaising> FMode;
        public sbyte FPlayer;
        public sbyte FDuration;
        public sbyte FOriginalOverlay;
        // Mountain.pas:181
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, -1, 40);
            Stream.rwShortintEx(ref FDuration, -1, 41);
            Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
        }

        // Mountain.pas:210
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Value);
            }
        }

        // Mountain.pas:223
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(AnonymousEnum_rmRaising.rmRaising))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // Mountain.pas:216
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4402)
            {
                var call__14 = FResource.TerrainList.Terrain[Msg.HN];
                if (call__14 >= 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // Mountain.pas:189
        public void RestoreTerrain()
        {
            TMapField Field = default;
            sbyte Overlay = default;
            Field = (TMapField)Owner;
            Overlay = FOriginalOverlay;
            this.Destroy();
            if (Overlay > 0)
            {
                AoWHSMap.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
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
        public void ShowRaiseTerrainAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TMapField Field = default;
            var call__15 = Anim.GetLhx();
            var call__16 = Anim.GetYhx();
            var call__17 = Anim.GetXhx();
            var call__18 = AoWHSMap.MapContainer.GetField(call__17, call__16, call__15);
            Field = call__18;
            // { Original tests the base mountain ID, although this object is raised terrain. }
            var call__19 = Field.FindOwnedChild(131432);
            if (this == call__19 && Anim.FFrameCounter == 0)
            {
                bool bool__20 = AoWHSMap.FSeatedPlayer == Anim.FTag;
                if (!bool__20)
                {
                    var call__21 = GetLhx();
                    var call__22 = GetYhx();
                    var call__23 = GetXhx();
                    var call__24 = AoWHSMap.WatchingTerrain(call__23, call__22, call__21, 0);
                    bool__20 = call__24;
                }

                if (bool__20)
                {
                    var call__25 = AoWHSSet.FSpellControl.GetSpell(19);
                    call__25.FCastSounds.Play(0);
                }
            }
        }

        // Mountain.pas:240
        public void NewFrameRaiseTerrainAnimation(TAnimation Anim, int FrameCounter)
        {
            TMapField Field = default;
            var call__26 = Anim.GetLhx();
            var call__27 = Anim.GetYhx();
            var call__28 = Anim.GetXhx();
            var call__29 = AoWHSMap.MapContainer.GetField(call__28, call__27, call__26);
            Anim = (TAnimation)(object)call__29;
            if (((TMapField)(object)Anim).FindOwnedChild(131434) == this && FrameCounter == 9)
            {
                FMode = FMode - TSet<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.rmRaising);
            }
        }

    }

    public partial class TRaisedMountainResource : TILTerrainMOR
    {
        public TRaisedMountainResource(TECustomNode owner) : base(owner)
        {}

        // Mountain.pas:249
        public new const int ClassID = 131435;

        // Mountain.pas:255
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131434;
            return Result;
        }

        // Mountain.pas:252
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(RaisedMountainRStr);
            return Result;
        }

    }

    public partial class TRaiseTerrainTE : TGlobalTargetSpellTE
    {
        public TRaiseTerrainTE(TECustomNode owner) : base(owner)
        {}

        // Mountain.pas:258
        public new const int ClassID = 135946;

        // Mountain.pas:317
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            RaiseTerrain(A1);
            AoWHSMap.ValidateTerrainObservation(A1.FPlayer, Unsafe.As<TXYL, TLocation>(ref FLocation).X, Unsafe.As<TXYL, TLocation>(ref FLocation).Y, Unsafe.As<TXYL, TLocation>(ref FLocation).Layer, 1);
            var call__30 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__30.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // Mountain.pas:261
        public void RaiseTerrain(TAbstractUnit Caster)
        {
            TList Resources = default;
            TMapLevel Level = default;
            THNWalkData Walk = default;
            TAoWMapField Field = default;
            TRaisedMountain Mountain = default;
            TMapObjectResource Resource = default;
            int I = default;
            bool Found = default;
            TImageSequenceAnimation Anim = default;
            Resources = AoWHSSet.MapObjectResourceList.FChildren;
            var call__31 = AoWHSMap.MapContainer.MapLevels[Unsafe.As<TXYL, TLocation>(ref FLocation).Layer];
            Level = call__31;
            InitHNWalk(ref Walk, Unsafe.As<TXYL, TLocation>(ref FLocation).X, Unsafe.As<TXYL, TLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__32 = Level.Field[Walk.X, Walk.Y];
                    Field = (TAoWMapField)call__32;
                    Resource = null;
                    var call__33 = AoWHSSet.FSpellControl.GetSpell(19);
                    var call__34 = ((TRaiseTerrain)call__33).ValidTargetMapF(Field);
                    if (call__34)
                    {
                        Found = false;
                        I = Resources.Count;
                        while (!Found && I != 0)
                        {
                            I -= 1;
                            Resource = (TMapObjectResource)Resources.List[I];
                            var call__35 = Resource.EClass.ClassID;
                            bool bool__36 = call__35 == 131435;
                            if (bool__36)
                            {
                                var call__37 = Resource.TerrainList.UsedTerrain();
                                bool__36 = call__37 == 1;
                            }

                            Found = bool__36;
                        }

                        if (Found)
                        {
                            Field.RestoreTerrain();
                            Mountain = (TRaisedMountain)Resource.CreateResourceUser();
                            var call__38 = AoWHSMap.Random(11);
                            Mountain.FDuration = (sbyte)((call__38 - 5) / 5 + 3);
                            Mountain.FPlayer = Caster.FPlayer;
                            Mountain.FOriginalOverlay = Field.OverlayType;
                            AoWHSMap.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<TXYL, TLocation>(ref FLocation).Layer, 0, -1);
                            var call__39 = AoWHSMap.PlaceHX(Mountain, Walk.X, Walk.Y, Unsafe.As<TXYL, TLocation>(ref FLocation).Layer);
                            if (call__39)
                            {
                                Mountain.FMode = Mountain.FMode + TSet<AnonymousEnum_rmRaising>.Of(AnonymousEnum_rmRaising.rmRaising);
                                Anim = new TImageSequenceAnimation(null);
                                var call__40 = AoWHSSet.FSpellControl.GetSpell(19);
                                var call__41 = call__40.FCastImages.ImageSequences[0];
                                Anim.SetImageSequence(call__41);
                                Anim.FOnShow = Mountain.ShowRaiseTerrainAnimation;
                                Anim.FOnNewFrame = Mountain.NewFrameRaiseTerrainAnimation;
                                var call__42 = AoWHSMap.Random(15);
                                Anim.FFrameCounter = -call__42;
                                AoWHSMap.PlaceHX(Anim, Walk.X, Walk.Y, Unsafe.As<TXYL, TLocation>(ref FLocation).Layer);
                            }
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230


    }

    public partial class TRaiseTerrain : TGlobalTargetSpell
    {
        public TRaiseTerrain(TECustomNode owner) : base(owner)
        {
            var call__43 = TranslateRStr(RaiseTerrainRStr);
            FSpellName = call__43;
            FSpellID = 19;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msEarth;
            FAboveGroundOnly = true;
        }

        // Mountain.pas:327


        // Mountain.pas:364
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            int I = default;
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            {
                I = 0;
                int end__44 = RadToHN(2) - 1;
                for (; I <= end__44; I += 1)
                {
                    A2.FHNs.Add(0);
                }
            }
        }

        // Mountain.pas:373
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TRaiseTerrainTE Event = default;
            Event = new TRaiseTerrainTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__45 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__45.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // Mountain.pas:347
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            bool Valid = default;
            TArmyHS Army = default;
            var call__46 = base.ValidTargetSelectionMapF(A1);
            bool bool__47 = call__46 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)((TAoWMapField)A1).TerrainType) && !TSet<byte>.WithBounds(0, 255).With(0).Contains((byte)((TAoWMapField)A1).OverlayType);
            if (bool__47)
            {
                var call__48 = ((TAoWMapField)A1).FindNoneTransparentHS(typeof(TStructure));
                bool__47 = call__48 == null;
            }

            Valid = bool__47;
            if (Valid)
            {
                Army = ((TAoWMapField)A1).GetArmyHS();
                if (Army != null)
                {
                    Valid = !Army.VisibleForSeatedPlayer();
                }
            }

            Result = Valid;
            return Result;
        }

        // Mountain.pas:338
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = default;
            var call__49 = base.ValidTargetMapF(A1);
            bool bool__50 = call__49 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)((TAoWMapField)A1).TerrainType) && !TSet<byte>.WithBounds(0, 255).With(0).Contains((byte)((TAoWMapField)A1).OverlayType);
            if (bool__50)
            {
                var call__51 = ((TAoWMapField)A1).FindNoneTransparentHS(typeof(TStructure));
                bool__50 = call__51 == null;
            }

            Result = bool__50;
            if (Result)
            {
                Result = ((TAoWMapField)A1).GetArmyHS() == null;
            }

            return Result;
        }

        // Mountain.pas:77

    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct TLocation
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

    public static partial class Mountain
    {
        // Mountain.pas:384
        public static void RegisterMountain(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TMountainMO>(), EClass.Of<TMountainMOR>(), EClass.Of<TRaiseTerrainTE>(), EClass.Of<TRaisedMountain>(), EClass.Of<TRaisedMountainResource>()]);
            var call__52 = new TRaiseTerrain(null);
            ((TAoWEngine)AEngine).FGameSet.FSpellControl.RegisterSpell(call__52);
        }

        // Mountain.pas:390
        public static void UnRegisterMountain(TEngine AEngine)
        {
            AEngine.UnregisterEClasses([EClass.Of<TMountainMO>(), EClass.Of<TMountainMOR>(), EClass.Of<TRaiseTerrainTE>(), EClass.Of<TRaisedMountain>(), EClass.Of<TRaisedMountainResource>()]);
            // { Original unregisters spell 12, although Raise Terrain registers as spell 19. }
            ((TAoWEngine)AEngine).FGameSet.FSpellControl.UnregisterSpellID(12);
        }

        public static readonly AnsiString MountainNameRStr = (AnsiString)"Mountain";
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
