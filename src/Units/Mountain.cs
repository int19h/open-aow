// Translated from Mountain.pas.
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
using Aow.Units.AoWTer;
using static Aow.Units.AoWTer.AoWTer;
using Aow.Units.GlobalTargetSpells;
using static Aow.Units.GlobalTargetSpells.GlobalTargetSpells;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Mountain
{
    public enum AnonymousEnum_rmRaising : byte
    {
        rmRaising = 0
    }

    // { TMountainMO (instance size 20, VMT 557A266C) }
    public partial class TMountainMO : Aow.Units.AoWTer.TAoWTerrain
    {
        public TMountainMO(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Mountain.pas:101
        public new const int ClassID = 131432;

        // Mountain.pas:104
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            var call__1 = FResource.TerrainList.Overlay[Index];
            Result = unchecked((sbyte)call__1);
            if (Result >= 0)
            {
                Result = 0;
            }

            return Result;
        }

        // Mountain.pas:110
        public override bool CanChangeTerrain(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // Mountain.pas:136
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            int I = default;
            bool Valid = default;
            Aow.Units.HSEngine.TTerrainList List = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.THexagonSprite HS = default;
            I = 0;
            Valid = true;
            List = FResource.TerrainList;
            while (Valid && (I < List.Count))
            {
                var call__2 = List.Terrain[I];
                bool bool__3 = (call__2 >= 0);
                if (!bool__3)
                {
                    var call__4 = List.Overlay[I];
                    bool__3 = (call__4 >= 0);
                }

                if (bool__3)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
                    if ((((X >= 0) && (Y >= 0)) && (X < MapContainer.Width)) && (Y < MapContainer.Height))
                    {
                        MapField = MapContainer.GetField(X, Y, Lhx);
                        HS = MapField.GetMaxLevelNoneTransparentHS();
                        if ((HS != null) && (HS.EClass.ClassID == 131432))
                        {
                            var call__5 = List.Overlay[I];
                            var call__6 = ValidMountainOverlap(MapField, unchecked((sbyte)call__5));
                            Valid = call__6;
                        }
                        else
                        {
                            Valid = (HS == null) || (HS.GetLevel() < 10);
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
        public bool ValidMountainOverlap(Aow.Units.HSEngine.TMapField MapField, sbyte Overlay)
        {
            bool Result = default;
            bool Valid = default;
            int I = default;
            Aow.Units.HSEngine.TMapObject HS = default;
            sbyte OtherOverlay = default;
            Valid = true;
            I = 0;
            while (true)
            {
                var call__9 = MapField.Count;
                if (!((I != call__9) && Valid))
                    break;
                {
                    HS = (Aow.Units.HSEngine.TMapObject)MapField.HSList[I];
                    if (HS.EClass.ClassID == 131432)
                    {
                        var call__10 = MapField.GetHNofHS(HS);
                        var call__11 = HS.Resource.TerrainList.Overlay[call__10];
                        OtherOverlay = unchecked((sbyte)call__11);
                        Valid = !(((OtherOverlay == 0) && (Overlay == 0)) || ((unchecked((short)OtherOverlay) == 1) && (unchecked((short)Overlay) == 1)));
                    }

                    I += 1;
                }
            }

            Result = Valid;
            return Result;
        }

        public new partial class Meta : Aow.Units.Terrain.TTerrainMO.Meta
        {
            // Mountain.pas:101
            public override int ClassID() => Aow.Units.Mountain.TMountainMO.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Mountain.TMountainMO => (Meta)MetaRegistry.Of(typeof(T));
            // Terrain.pas:19
            public new Aow.Units.Mountain.TMountainMO Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Mountain.TMountainMO)base.Create(AOwner);
        }
    }

    // { TMountainMOR (instance size 76, VMT 557A2814) }
    public partial class TMountainMOR : Aow.Units.AoWTer.TAoWTerrainResource
    {
        public TMountainMOR(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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

        public new partial class Meta : Aow.Units.Terrain.TTerrainMOR.Meta
        {
            // Mountain.pas:169
            public override int ClassID() => Aow.Units.Mountain.TMountainMOR.ClassID;

            // Mountain.pas:172
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__12 = Aow.Units.AoWE.AoWE.MountainRStr;
                var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(call__12);
                Result = call__13;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Mountain.TMountainMOR => (Meta)MetaRegistry.Of(typeof(T));
            // Terrain.pas:42
            public new Aow.Units.Mountain.TMountainMOR Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Mountain.TMountainMOR)base.Create(AOwner);
        }
    }

    // { TRaisedMountain (instance size 32, VMT 557A2940): $1C FMode,
//     $1D FPlayer, $1E FDuration, $1F FOriginalOverlay. }
    public partial class TRaisedMountain : Aow.Units.ILTer.TILTerrainMO
    {
        public TRaisedMountain(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Mountain.pas:178
        public new const int ClassID = 131434;

        public TSet<Aow.Units.Mountain.AnonymousEnum_rmRaising> FMode;
        // { $1C }
        public sbyte FPlayer;
        // { $1D }
        public sbyte FDuration;
        // { $1E }
        public sbyte FOriginalOverlay;
        // Mountain.pas:181
        // { $1F }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, unchecked((sbyte)-1), 40);
            Stream.rwShortintEx(ref FDuration, unchecked((sbyte)-1), 41);
            Stream.rwShortintEx(ref FOriginalOverlay, unchecked((sbyte)-1), 42);
        }

        // Mountain.pas:210
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn(unchecked((sbyte)Msg.Value));
            }
        }

        // Mountain.pas:223
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(Aow.Units.Mountain.AnonymousEnum_rmRaising.rmRaising))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // Mountain.pas:216
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
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
            Aow.Units.HSEngine.TMapField Field = default;
            sbyte Overlay = default;
            Field = (Aow.Units.HSEngine.TMapField)Owner;
            Overlay = unchecked((sbyte)FOriginalOverlay);
            this.Destroy();
            if (Overlay > 0)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceTerrain(unchecked((sbyte)Field.Xhx), unchecked((sbyte)Field.Yhx), unchecked((sbyte)Field.Lhx), unchecked((sbyte)Field.TerrainType), unchecked((sbyte)Overlay));
            }
        }

        // Mountain.pas:201
        public void NewTurn(sbyte Player)
        {
            if ((Player == FPlayer) && (FDuration > 0))
            {
                FDuration = (sbyte)(((int)FDuration) - 1);
                if (FDuration == 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // Mountain.pas:228
        public void ShowRaiseTerrainAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.HSEngine.TMapField Field = default;
            var call__15 = Anim.GetLhx();
            var call__16 = Anim.GetYhx();
            var call__17 = Anim.GetXhx();
            var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__17, call__16, call__15);
            Field = call__18;
            // { Original tests the base mountain ID, although this object is raised terrain. }
            var call__19 = Field.FindOwnedChild(131432);
            if ((this == call__19) && (Anim.FFrameCounter == 0))
            {
                bool bool__20 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Anim.FTag);
                if (!bool__20)
                {
                    var call__21 = GetLhx();
                    var call__22 = GetYhx();
                    var call__23 = GetXhx();
                    var call__24 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)call__23), unchecked((sbyte)call__22), unchecked((sbyte)call__21), 0);
                    bool__20 = call__24;
                }

                if (bool__20)
                {
                    var call__25 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(19);
                    call__25.FCastSounds.Play(0);
                }
            }
        }

        // Mountain.pas:240
        public void NewFrameRaiseTerrainAnimation(Aow.Units.Animation.TAnimation Anim, int FrameCounter)
        {
            Aow.Units.HSEngine.TMapField Field = default;
            var call__26 = Anim.GetLhx();
            var call__27 = Anim.GetYhx();
            var call__28 = Anim.GetXhx();
            var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__28, call__27, call__26);
            Anim = (Aow.Units.Animation.TAnimation)((System.Object)call__29);
            if ((((Aow.Units.HSEngine.TMapField)((System.Object)Anim)).FindOwnedChild(131434) == this) && (FrameCounter == 9))
            {
                FMode = FMode - TSet<Aow.Units.Mountain.AnonymousEnum_rmRaising>.Of(Aow.Units.Mountain.AnonymousEnum_rmRaising.rmRaising);
            }
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMO.Meta
        {
            // Mountain.pas:178
            public override int ClassID() => Aow.Units.Mountain.TRaisedMountain.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Mountain.TRaisedMountain => (Meta)MetaRegistry.Of(typeof(T));
            // ILTer.pas:20
            public new Aow.Units.Mountain.TRaisedMountain Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Mountain.TRaisedMountain)base.Create(AOwner);
        }
    }

    // { TRaisedMountainResource (instance size 68, VMT 557A2AF4) }
    public partial class TRaisedMountainResource : Aow.Units.ILTer.TILTerrainMOR
    {
        public TRaisedMountainResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // Mountain.pas:249
            public override int ClassID() => Aow.Units.Mountain.TRaisedMountainResource.ClassID;

            // Mountain.pas:252
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaisedMountainRStr);
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Mountain.TRaisedMountainResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.Mountain.TRaisedMountainResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Mountain.TRaisedMountainResource)base.Create(AOwner);
        }
    }

    // { TRaiseTerrainTE (instance size 36, VMT 557A2BF8) }
    public partial class TRaiseTerrainTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        public TRaiseTerrainTE(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Mountain.pas:258
        public new const int ClassID = 135946;

        // Mountain.pas:317
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            RaiseTerrain(A1);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).Layer), 1);
            var call__30 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__30.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // Mountain.pas:261
        public void RaiseTerrain(Aow.Units.AoWE.TAbstractUnit Caster)
        {
            Aow.Rtl.TList Resources = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.Mountain.TRaisedMountain Mountain = default;
            Aow.Units.HSEngine.TMapObjectResource Resource = default;
            int I = default;
            bool Found = default;
            Aow.Units.Animation.TImageSequenceAnimation Anim = default;
            Resources = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FChildren;
            var call__31 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).Layer];
            Level = call__31;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__32 = Level.Field[Walk.X, Walk.Y];
                    Field = (Aow.Units.AoWE.TAoWMapField)call__32;
                    Resource = null;
                    var call__33 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(19);
                    var call__34 = ((Aow.Units.Mountain.TRaiseTerrain)call__33).ValidTargetMapF(Field);
                    if (call__34)
                    {
                        Found = false;
                        I = Resources.Count;
                        while (!Found && (I != 0))
                        {
                            I -= 1;
                            Resource = (Aow.Units.HSEngine.TMapObjectResource)Resources.List[I];
                            var call__35 = Resource.EClass.ClassID;
                            bool bool__36 = (call__35 == 131435);
                            if (bool__36)
                            {
                                var call__37 = Resource.TerrainList.UsedTerrain();
                                bool__36 = (call__37 == 1);
                            }

                            Found = bool__36;
                        }

                        if (Found)
                        {
                            Field.RestoreTerrain();
                            Mountain = (Aow.Units.Mountain.TRaisedMountain)Resource.CreateResourceUser();
                            var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                            Mountain.FDuration = unchecked((sbyte)(((call__38 - 5) / 5) + 3));
                            Mountain.FPlayer = unchecked((sbyte)Caster.FPlayer);
                            Mountain.FOriginalOverlay = unchecked((sbyte)Field.OverlayType);
                            Aow.Units.AoWE.AoWE.AoWHSMap.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).Layer, 0, -1);
                            var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Mountain, Walk.X, Walk.Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).Layer);
                            if (call__39)
                            {
                                Mountain.FMode = Mountain.FMode + TSet<Aow.Units.Mountain.AnonymousEnum_rmRaising>.Of(Aow.Units.Mountain.AnonymousEnum_rmRaising.rmRaising);
                                Anim = new Aow.Units.Animation.TImageSequenceAnimation(null);
                                var call__40 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(19);
                                var call__41 = call__40.FCastImages.ImageSequences[0];
                                Anim.SetImageSequence(call__41);
                                Anim.FOnShow = Mountain.ShowRaiseTerrainAnimation;
                                Anim.FOnNewFrame = Mountain.NewFrameRaiseTerrainAnimation;
                                var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(15);
                                Anim.FFrameCounter = -call__42;
                                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, Walk.X, Walk.Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.Mountain.TLocation>(ref FLocation).Layer);
                            }
                        }
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230


        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // Mountain.pas:258
            public override int ClassID() => Aow.Units.Mountain.TRaiseTerrainTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Mountain.TRaiseTerrainTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.Mountain.TRaiseTerrainTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Mountain.TRaiseTerrainTE)base.Create(AOwner);
        }
    }

    // { TRaiseTerrain (instance size 60, VMT 557A2CD0) }
    public partial class TRaiseTerrain : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        private protected TRaiseTerrain(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaiseTerrainRStr);
            FSpellName = call__43;
            FSpellID = 19;
            FBaseManaCost = 5;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FAboveGroundOnly = true;
        }

        public static new TRaiseTerrain Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TRaiseTerrain(context, owner));
        }

        // Mountain.pas:327


        // Mountain.pas:364
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            int I = default;
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            {
                I = 0;
                int end__44 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__44; I += 1)
                {
                    A2.FHNs.Add(0);
                }
            }
        }

        // Mountain.pas:373
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.Mountain.TRaiseTerrainTE Event = default;
            Event = new Aow.Units.Mountain.TRaiseTerrainTE(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__45 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__45.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // Mountain.pas:347
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            bool Valid = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__46 = base.ValidTargetSelectionMapF(A1);
            bool bool__47 = (call__46 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)((Aow.Units.AoWE.TAoWMapField)A1).TerrainType)) && !TSet<byte>.WithBounds(0, 255).With(0).Contains((byte)((Aow.Units.AoWE.TAoWMapField)A1).OverlayType);
            if (bool__47)
            {
                var call__48 = ((Aow.Units.AoWE.TAoWMapField)A1).FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__47 = (call__48 == null);
            }

            Valid = bool__47;
            if (Valid)
            {
                Army = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                if (Army != null)
                {
                    Valid = !Army.VisibleForSeatedPlayer();
                }
            }

            Result = Valid;
            return Result;
        }

        // Mountain.pas:338
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__49 = base.ValidTargetMapF(A1);
            bool bool__50 = (call__49 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)((Aow.Units.AoWE.TAoWMapField)A1).TerrainType)) && !TSet<byte>.WithBounds(0, 255).With(0).Contains((byte)((Aow.Units.AoWE.TAoWMapField)A1).OverlayType);
            if (bool__50)
            {
                var call__51 = ((Aow.Units.AoWE.TAoWMapField)A1).FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__50 = (call__51 == null);
            }

            Result = bool__50;
            if (Result)
            {
                Result = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS() == null;
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
        public static void RegisterMountain(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TMountainMO>(), Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TMountainMOR>(), Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TRaiseTerrainTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TRaisedMountain>(), Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TRaisedMountainResource>()]);
            var call__52 = Aow.Units.Mountain.TRaiseTerrain.Create(null);
            ((Aow.Units.AoWE.TAoWEngine)AEngine).FGameSet.FSpellControl.RegisterSpell(call__52);
        }

        // Mountain.pas:390
        public static void UnRegisterMountain(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TMountainMO>(), Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TMountainMOR>(), Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TRaiseTerrainTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TRaisedMountain>(), Aow.Units.Engine.EClass.Of<Aow.Units.Mountain.TRaisedMountainResource>()]);
            // { Original unregisters spell 12, although Raise Terrain registers as spell 19. }
            ((Aow.Units.AoWE.TAoWEngine)AEngine).FGameSet.FSpellControl.UnregisterSpellID(12);
        }

        public static readonly Aow.Rtl.AnsiString MountainNameRStr = (AnsiString)"Mountain";
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
