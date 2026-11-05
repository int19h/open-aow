// Translated from GlobalTargetSpells.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.SmokeCloud;
using Aow.Units.AoWReg;

namespace Aow.Units.GlobalTargetSpells
{
    public enum TWatcherStateElement : byte
    {
        wsActive = 0
    }

    // FLocation: packed X/Y/layer coordinates
    public partial class TGlobalTargetSpellTE : TSpellTE
    {
        public TGlobalTargetSpellTE(TECustomNode owner) : base(owner)
        {}

        // GlobalTargetSpells.pas:401
        public new const int ClassID = 135976;

        public TXYL FLocation;
        // GlobalTargetSpells.pas:406
        // packed coordinates
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FLocation.XYL, 20);
        }

        // GlobalTargetSpells.pas:412
        public void SetLocation(sbyte X, sbyte Y, sbyte Layer)
        {
            Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X = X;
            Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y = Y;
            Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer = Layer;
        }

        // GlobalTargetSpells.pas:419
        public void PlayDefaultEffects(sbyte X, sbyte Y, sbyte Layer)
        {
            TImageSequenceAnimation Animation = default;
            var call__1 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__2 = call__1.FCastImages.ImageSequences[0];
            if (call__2 != null)
            {
                Animation = new TImageSequenceAnimation(null);
                var call__3 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__4 = call__3.FCastImages.ImageSequences[0];
                Animation.SetImageSequence(call__4);
                AoWHSMap.PlaceHX(Animation, X, Y, Layer);
            }

            var call__5 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__5.FCastSounds.Play(0);
        }

        // NetworkE.pas:230


    }

    // FCasterID, FCasterPlayer, FAboveGroundOnly CanActivate/Activate and the target predicates
    // return Boolean. Procedure declarations retained for shared TSpell and P7 overrides.
    // ShowTargetSelection variants: reordered locals, split/inline radius, target with-scope, and
    // inline map-field lookup. SetupTargetSelection variants: explicit class local, repeated casts,
    // class with-scope, record fields, record with-scope and reversed coordinate local order
    public partial class TGlobalTargetSpell : TSpell
    {

        public int FCasterID;
        public sbyte FCasterPlayer;
        public bool FAboveGroundOnly;
        // GlobalTargetSpells.pas:439
        public TGlobalTargetSpell(TECustomNode owner) : base(owner)
        {
            FSpellType = TSpellType.stGlobal;
        }

        // GlobalTargetSpells.pas:569
        public override bool CanActivate(TAbstractUnit A1, ref AnsiString A2)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Result = base.CanActivate(A1, ref A2);
            if (Result && FAboveGroundOnly)
            {
                A1.GetLocation(ref X, ref Y, ref Layer);
                if (Layer != 0)
                {
                    Result = false;
                    A2 = TranslateRStr(CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:618
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString MessageText = default;
            base.Activate(A1);
            if (CanActivate(A1, ref MessageText))
            {
                FCasterID = A1.FUnitID;
                FCasterPlayer = A1.FPlayer;
                SetupTargetSelection(A1, AoWHSMap.FTargetSelection);
                AoWHSMap.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:445
        public virtual void ShowTargetSelection(TTargetSelectionControl Target, int HN, int Xs, int Ys, in TSurfaceDesc SurfaceDesc)
        {
            TMapLevel Level = default;
            TAoWMapField MapF = default;
            int X = default;
            int Y = default;
            int Intensity = default;
            int Radius = default;
            var call__6 = AoWHSMap.MapContainer.MapLevels[Target.FTargetL];
            Level = call__6;
            CenterHNtoHX(Target.FTargetX, Target.FTargetY, HN, ref X, ref Y);
            // { Original accepts Width/Height themselves (strict > bounds test). }
            if (X >= 0 && Y >= 0 && X <= Level.Width && Y <= Level.Height)
            {
                var call__7 = Level.Field[X, Y];
                MapF = (TAoWMapField)call__7;
                if (this.ValidTargetSelectionMapF((TMapField)(object)MapF))
                {
                    Radius = HNtoRad(HN);
                    Intensity = Math.Abs((AoWHSMap.Frame + Radius) % 9 - 4) * 20 + 20;
                    var call__8 = Target.FImageLibrary[0];
                    call__8.ShowBlended(Xs - 8, Ys, SurfaceDesc, IntensityBlendTable.LookupTables[Intensity]);
                }
                else
                {
                    var call__9 = Target.FImageLibrary[1];
                    call__9.Show(Xs - 8, Ys, SurfaceDesc);
                }
            }
        }

        // GlobalTargetSpells.pas:521
        public virtual void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            sbyte UnitX = default;
            sbyte UnitY = default;
            sbyte UnitLayer = default;
            A2.Initialize(A1);
            A2.FTargetMode = A2.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmOwnerDraw, TTargetSelectionModeElement.tmValidateDistance, TTargetSelectionModeElement.tmExternalFloater);
            A1.GetLocation(ref UnitX, ref UnitY, ref UnitLayer);
            A2.FSourceX = UnitX;
            A2.FSourceY = UnitY;
            A2.FSourceL = UnitLayer;
            A2.FRange = 12;
            A2.FOnChange.Add(OnTargetSelectionChange);
            A2.FOnShow = ShowTargetSelection;
            var call__11 = TranslateRStr(SelectTargetLocationForXOnMapRStr);
            var call__12 = Rtl.Rtl.Format(call__11, [FSpellName]);
            A2.FText.Text = call__12;
            A2.FTitle = FSpellName;
            var call__13 = TranslateRStr(CancelRStr);
            A2.FCancelText = call__13;
            var call__14 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__15 = call__14.FCastImages.ImageSequences[20];
            var call__16 = call__15.GetPreviewImage();
            A2.FSourceImage = call__16;
            var call__17 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__18 = call__17.FCastImages.ImageSequences[21];
            var call__19 = call__18.GetPreviewImage();
            A2.FValidTargetImage = call__19;
            var call__20 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__21 = call__20.FCastImages.ImageSequences[22];
            var call__22 = call__21.GetPreviewImage();
            A2.FInvalidTargetImage = call__22;
        }

        // GlobalTargetSpells.pas:544
        public virtual bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4) => false;

        // GlobalTargetSpells.pas:487
        public virtual bool ValidTargetSelection(TTargetSelectionControl A1)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:475
        public virtual bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            bool bool__23 = AoWHSMap.FExplorationEnabled;
            if (bool__23)
            {
                var call__24 = ((TAoWMapField)A1).GetExplored(FCasterPlayer);
                bool__23 = !call__24;
            }

            if (bool__23)
            {
                // Preserve the result for callers.
                Result = false;
                return Result;
            }

            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:469
        public virtual bool ValidTargetMapF(TMapField A1) => true;
    }

    public partial class TFreezeWaterTE : TGlobalTargetSpellTE
    {
        public TFreezeWaterTE(TECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135939;

        // GlobalTargetSpells.pas:679
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            TTwinkleAnimation Twinkle = default;
            base.Process(A1, A2);
            AoWHSMap.ValidateTerrainObservation(A1.FPlayer, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
            var call__25 = AoWHSMap.TerrainVisible((sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
            if (call__25)
            {
                Twinkle = new TTwinkleAnimation(null);
                var call__26 = AoWHSSet.FSpellControl.GetSpell(10);
                var call__27 = call__26.FCastImages.ImageSequences[0];
                Twinkle.Setup(call__27, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmExpand), 100, 64, 50, 30);
                AoWHSMap.PlaceHX(Twinkle, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            }

            // Use the map's seated player.
            bool bool__28 = AoWHSMap.FSeatedPlayer == A1.FPlayer;
            if (!bool__28)
            {
                var call__29 = AoWHSMap.WatchingTerrain((sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                bool__28 = call__29;
            }

            if (bool__28)
            {
                var call__30 = AoWHSSet.FSpellControl.GetSpell(10);
                call__30.FCastSounds.Play(0);
            }

            AoWHSMap.ChangeTerrainEx(null, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, TerrainChanged);
            var call__31 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__31.CastingDone(A1, FEventMana);
            // The original event result is zero.
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:641
        public void ChangeTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            TFrozenWaterHS Ice = default;
            if (((TAoWMapField)MapF).GetArmyHS() == null)
            {
                if (TerrainType == (int)TAoWTerrainType.ttWater)
                {
                    TerrainType = (int)TAoWTerrainType.ttIce;
                }
                else
                {
                    if (TerrainType == (int)TAoWTerrainType.ttUndergroundWater)
                    {
                        TerrainType = (int)TAoWTerrainType.ttUndergroundIce;
                    }
                    else
                    {
                        if (TSet<byte>.WithBounds(0, 255).With((int)TAoWTerrainType.ttIce).With((int)TAoWTerrainType.ttUndergroundIce).Contains((byte)TerrainType))
                        {
                            Ice = (TFrozenWaterHS)MapF.FindOwnedHS(typeof(TFrozenWaterHS));
                            if (Ice != null)
                            {
                                var call__32 = AoWHSMap.Random(11);
                                Ice.FDaysRemaining = (sbyte)((call__32 - 5) / 5 + 3);
                                Ice.FCasterPlayer = (sbyte)FEventPlayerIndex;
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:661
        public void TerrainChanged(TMapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
            TFrozenWaterHS Ice = default;
            if (TSet<byte>.WithBounds(0, 255).With((int)TAoWTerrainType.ttIce).With((int)TAoWTerrainType.ttUndergroundIce).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With((int)TAoWTerrainType.ttIce).With((int)TAoWTerrainType.ttUndergroundIce).Contains((byte)OriginalTerrainType))
            {
                Ice = (TFrozenWaterHS)MapF.FindOwnedHS(typeof(TFrozenWaterHS));
                if (Ice == null)
                {
                    Ice = new TFrozenWaterHS(null);
                    AoWHSMap.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                }

                var call__33 = AoWHSMap.Random(11);
                Ice.FDaysRemaining = (sbyte)((call__33 - 5) / 5 + 3);
                Ice.FCasterPlayer = (sbyte)FEventPlayerIndex;
            }
        }

        // NetworkE.pas:230


    }

    public partial class TFreezeWater : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:710
        public TFreezeWater(TECustomNode owner) : base(owner)
        {
            var call__34 = TranslateRStr(FreezeWaterRStr);
            this.FSpellName = call__34;
            FSpellID = 10;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msWater;
        }

        // GlobalTargetSpells.pas:811
        // { Original Boolean-return ABI; foreign P7 overrides remain procedures. }
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__35 = RadToHN(2) - 1;
                for (; I <= end__35; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:823
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TFreezeWaterTE Event = default;
            Event = new TFreezeWaterTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__36 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__36.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }
    }

    public partial class TFireBarrierTE : TGlobalTargetSpellTE
    {
        public TFireBarrierTE(TECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135940;

        // GlobalTargetSpells.pas:873
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            CreateFireBarrier();
            var call__37 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__38 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__38.CastingDone(A1, call__37.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:842
        public void CreateFireBarrier()
        {
            THNWalkData Walk = default;
            TMapLevel Level = default;
            TAoWMapField MapF = default;
            TFire Fire = default;
            var call__39 = AoWHSMap.Random(16777215);
            RandSeed = call__39;
            var call__40 = AoWHSMap.MapContainer.MapLevels[Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer];
            Level = call__40;
            InitHNWalk(ref Walk, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__41 = Level.Field[Walk.X, Walk.Y];
                    MapF = (TAoWMapField)call__41;
                    var call__42 = AoWHSSet.FSpellControl.GetSpell(11);
                    var call__43 = ((TGlobalTargetSpell)(object)call__42).ValidTargetMapF((TMapField)(object)MapF);
                    bool bool__44 = call__43;
                    if (bool__44)
                    {
                        var call__45 = MapF.GetArmyHS();
                        bool__44 = call__45 == null;
                    }

                    if (bool__44)
                    {
                        var call__46 = AoWHSSet.FFireControl.PlaceFire(Walk.X, Walk.Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, TFireType.ftLarge);
                        Fire = call__46;
                        if (Fire != null)
                        {
                            var call__47 = Random(35);
                            Fire.SetupExplosion(call__47);
                            Fire.FTurnsLeft = 3;
                            var call__48 = this.GetSource();
                            Fire.FPlayer = call__48.FPlayer;
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230


    }

    public partial class TFireBarrier : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:882
        public TFireBarrier(TECustomNode owner) : base(owner)
        {
            var call__49 = TranslateRStr(FireBarrierRStr);
            this.FSpellName = call__49;
            FSpellID = 11;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msFire;
        }

        // GlobalTargetSpells.pas:914
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__50 = RadToHN(2) - 1;
                for (; I <= end__50; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:926
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TFireBarrierTE Event = default;
            Event = new TFireBarrierTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__51 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__51.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:900
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            TArmyHS Army = default;
            var call__52 = base.ValidTargetSelectionMapF(A1);
            bool bool__53 = call__52 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__53)
            {
                var call__54 = A1.FindNoneTransparentHS(typeof(TStructure));
                bool__53 = call__54 == null;
            }

            Result = bool__53;
            if (Result)
            {
                Army = ((TAoWMapField)A1).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:892
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = default;
            var call__55 = base.ValidTargetMapF(A1);
            bool bool__56 = call__55 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__56)
            {
                var call__57 = A1.FindNoneTransparentHS(typeof(TStructure));
                bool__56 = call__57 == null;
            }

            Result = bool__56;
            if (Result)
            {
                Result = ((TAoWMapField)A1).GetArmyHS() == null;
            }

            return Result;
        }
    }

    public partial class TLevelTerrainTE : TGlobalTargetSpellTE
    {
        public TLevelTerrainTE(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:940
        public new const int ClassID = 135945;

        // GlobalTargetSpells.pas:968
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            PlayDefaultEffects(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            LevelTerrain();
            var call__58 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__59 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__59.CastingDone(A1, call__58.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:943
        public void LevelTerrain()
        {
            THNWalkData Walk = default;
            TMapLevel Level = default;
            TAoWMapField MapF = default;
            var call__60 = AoWHSMap.MapContainer.MapLevels[Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer];
            Level = call__60;
            InitHNWalk(ref Walk, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__61 = Level.Field[Walk.X, Walk.Y];
                    MapF = (TAoWMapField)call__61;
                    var call__62 = AoWHSSet.FSpellControl.GetSpell(18);
                    var call__63 = ((TGlobalTargetSpell)(object)call__62).ValidTargetMapF((TMapField)(object)MapF);
                    bool bool__64 = call__63;
                    if (bool__64)
                    {
                        var call__65 = MapF.GetArmyHS();
                        bool__64 = call__65 == null;
                    }

                    if (bool__64)
                    {
                        AoWHSMap.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 0, -1);
                        if (MapF.TerrainType == (int)TAoWTerrainType.ttEarth)
                        {
                            AoWHSMap.ChangeTerrain(null, Walk.X, Walk.Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 0, (int)TAoWTerrainType.ttDirt);
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230


    }

    public partial class TLevelTerrain : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:978
        public TLevelTerrain(TECustomNode owner) : base(owner)
        {
            var call__66 = TranslateRStr(LevelTerrainRStr);
            this.FSpellName = call__66;
            FSpellID = 18;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msEarth;
        }

        // GlobalTargetSpells.pas:1010
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__67 = RadToHN(2) - 1;
                for (; I <= end__67; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:1022
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TLevelTerrainTE Event = default;
            Event = new TLevelTerrainTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__68 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__68.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:996
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            TArmyHS Army = default;
            var call__69 = base.ValidTargetSelectionMapF(A1);
            bool bool__70 = call__69 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__70)
            {
                var call__71 = A1.FindNoneTransparentHS(typeof(TStructure));
                bool__70 = call__71 == null;
            }

            Result = bool__70;
            if (Result)
            {
                Army = ((TAoWMapField)A1).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:988
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = default;
            var call__72 = base.ValidTargetMapF(A1);
            bool bool__73 = call__72 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__73)
            {
                var call__74 = A1.FindNoneTransparentHS(typeof(TStructure));
                bool__73 = call__74 == null;
            }

            Result = bool__73;
            if (Result)
            {
                Result = ((TAoWMapField)A1).GetArmyHS() == null;
            }

            return Result;
        }
    }

    public partial class TRejuvenateTE : TGlobalTargetSpellTE
    {
        public TRejuvenateTE(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:1036
        public new const int ClassID = 135952;

        // GlobalTargetSpells.pas:1044
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            TTwinkleAnimation Twinkle = default;
            base.Process(A1, A2);
            AoWHSMap.ValidateTerrainObservation(A1.FPlayer, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
            var call__75 = AoWHSMap.TerrainVisible((sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
            if (call__75)
            {
                Twinkle = new TTwinkleAnimation(null);
                var call__76 = AoWHSSet.FSpellControl.GetSpell(41);
                var call__77 = call__76.FCastImages.ImageSequences[0];
                Twinkle.Setup(call__77, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 100, 64, 50, 30);
                AoWHSMap.PlaceHX(Twinkle, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            }

            // Use the map's seated player.
            bool bool__78 = AoWHSMap.FSeatedPlayer == A1.FPlayer;
            if (!bool__78)
            {
                var call__79 = AoWHSMap.WatchingTerrain((sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                bool__78 = call__79;
            }

            if (bool__78)
            {
                var call__80 = AoWHSSet.FSpellControl.GetSpell(41);
                call__80.FCastSounds.Play(0);
            }

            AoWHSMap.ChangeTerrainEx(null, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2, RejuvenateTerrain, null);
            var call__81 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__82 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__82.CastingDone(A1, call__81.FBaseManaCost);
            // The original event result is zero.
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:1039
        public void RejuvenateTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (TSet<byte>.WithBounds(0, 255).WithRange((int)TAoWTerrainType.ttDesert, (int)TAoWTerrainType.ttWasteland).Contains((byte)TerrainType))
            {
                TerrainType = (int)TAoWTerrainType.ttGrass;
            }
        }

        // NetworkE.pas:230


    }

    public partial class TRejuvenate : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1074
        public TRejuvenate(TECustomNode owner) : base(owner)
        {
            var call__83 = TranslateRStr(RejuvenateRStr);
            this.FSpellName = call__83;
            FSpellID = 41;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msLife;
            FAboveGroundOnly = true;
        }

        // GlobalTargetSpells.pas:1095
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__84 = RadToHN(3) - 1;
                for (; I <= end__84; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:1107
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TRejuvenateTE Event = default;
            Event = new TRejuvenateTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__85 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__85.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:1090
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            var call__86 = base.ValidTargetSelectionMapF(A1);
            Result = call__86 && TSet<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)A1.TerrainType);
            return Result;
        }

        // GlobalTargetSpells.pas:1085
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = default;
            var call__87 = base.ValidTargetMapF(A1);
            Result = call__87 && TSet<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)A1.TerrainType);
            return Result;
        }
    }

    // FWatcher
    public partial class TWatcherEnchantment : TGlobalEnchantment
    {
        public TWatcherHS FWatcher;
        // GlobalTargetSpells.pas:1121
        public TWatcherEnchantment(TECustomNode owner) : base(owner)
        {
            FEnchantmentMode = TSet<TGlobalEnchantmentModeElement>.FromMask(FEnchantmentMode.Mask) - TSet<TGlobalEnchantmentModeElement>.Of(TGlobalEnchantmentModeElement.gemTrueGlobal, TGlobalEnchantmentModeElement.gemOwned);
            FDispelMana = 50;
        }

        // GlobalTargetSpells.pas:1129
        public override AnsiString GetName()
        {
            AnsiString Result = default;
            var call__88 = AoWHSSet.FSpellControl.GetSpell(57);
            Result = call__88.FSpellName;
            return Result;
        }

    }

    // state, unfog radius, enchantment, animation frame, unexplored HNs, visibility flag
    public partial class TWatcherHS : TSingleHS
    {
        // GlobalTargetSpells.pas:1220
        public new const int ClassID = 132989;

        public byte FWatcherState;
        public byte FUnfogRadius;
        public TWatcherEnchantment FEnchantment;
        public int FAnimationFrame;
        public TIntegerList FUnexplored;
        public bool FVisibleForSeatedPlayer;
        // GlobalTargetSpells.pas:1213
        public override void Destroy()
        {
            base.Destroy();
            FUnexplored.Free();
            FEnchantment.Release();
        }

        // GlobalTargetSpells.pas:1344
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FEnchantment, 40);
        }

        // GlobalTargetSpells.pas:1203
        public TWatcherHS(TECustomNode owner) : base(owner)
        {
            var call__89 = new TWatcherEnchantment(null);
            FEnchantment = call__89;
            FEnchantment.FWatcher = this;
            var call__90 = new TIntegerList();
            FUnexplored = call__90;
            FVisibleForSeatedPlayer = false;
            FAnimationFrame = -1;
        }

        // GlobalTargetSpells.pas:1422
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            int ID = default;
            base.MsgProc(ref Msg, Sender);
            ID = Msg.MsgID;
            if (ID == 537001992)
            {
                UpdateExploration();
                UpdateUnfog();
                return;
            }

            if (ID == 537001990)
            {
                UpdateUnfog();
                UpdateVisibleForSeatedPlayer();
            }
        }

        // GlobalTargetSpells.pas:1223
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 27;
            return Result;
        }

        // GlobalTargetSpells.pas:1255
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int DX = default;
            int DY = default;
            int Radius = default;
            int Angle = default;
            int PX = default;
            int PY = default;
            int Intensity = default;
            int I = default;
            int Delay = default;
            bool bool__91 = AoWHSMap.ViewL == Lhx;
            if (bool__91)
            {
                var call__92 = Yhx;
                var call__93 = Xhx;
                var call__94 = AoWHSMap.Visible(call__93, call__92, Lhx, 0);
                bool__91 = call__94;
            }

            if (bool__91)
            {
                var call__95 = Yhx;
                var call__96 = Xhx;
                HXtoHP(call__96, call__95, ref X, ref Y);
                X = X - XhpSurf + 16;
                Y = Y - YhpSurf + 16;
                var call__97 = AoWHSSet.FSpellControl.GetSpell(57);
                if (call__97.FCastImages != null)
                {
                    var call__98 = AoWHSSet.FSpellControl.GetSpell(57);
                    call__98.FCastImages.ShowLooped(1, X, Y, AoWHSMap.Frame, SurfaceDesc);
                }

                if (FAnimationFrame != -1)
                {
                    FAnimationFrame += 1;
                    // The original reads the shared AoWEngine animation detail. The map engine is
                    // not the global read by this routine.
                    if (AoWEngine.FAnimationDetail == 2 && AoWHSMap.ViewL == Lhx)
                    {
                        Intensity = 100 - FAnimationFrame;
                        var call__99 = Yhx;
                        var call__100 = Xhx;
                        HXtoHP(call__100, call__99, ref X, ref Y);
                        X = X - XhpSurf - 8;
                        Y = Y - YhpSurf;
                        {
                            I = FUnexplored.Count - 1;
                            byte end__101 = 0;
                            for (; I >= end__101; I -= 1)
                            {
                                var call__102 = FUnexplored[I];
                                HNtoHP(call__102, ref PX, ref PY);
                                PX += X;
                                PY += Y;
                                var call__103 = AoWHSSet.FGeneralImages[5];
                                call__103.ShowBlended(PX, PY, SurfaceDesc, AlphaBlendTable.LookupTables[Intensity]);
                            }
                        }
                    }

                    // { Original resets the shared random generator for deterministic particles. }
                    RandSeed = AoWHSMap.FInitialRandomSeed;
                    {
                        I = 0;
                        short end__104 = 300;
                        for (; I <= end__104; I += 1)
                        {
                            var call__105 = VisibilityRange();
                            var call__106 = Random(call__105 << 5);
                            Radius = call__106;
                            Angle = Random(360);
                            Delay = Random(80);
                            if (FAnimationFrame - Delay >= 0)
                            {
                                DegreesRadToCo(Angle, Radius, ref DX, ref DY);
                                var call__107 = AoWHSSet.FSpellControl.GetSpell(57);
                                call__107.FCastImages.Show(0, X + DX, Y + DY, FAnimationFrame - Delay, SurfaceDesc);
                            }
                        }
                    }

                    if (FAnimationFrame == 100)
                    {
                        StopAnimation();
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:1388
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            ((TMapField)Owner).UpdateMapField();
            return Result;
        }

        // GlobalTargetSpells.pas:1394
        public override void RemoveFromMap()
        {
            TMapField MapF = default;
            MapF = (TMapField)Owner;
            base.RemoveFromMap();
            MapF.UpdateMapField();
        }

        // GlobalTargetSpells.pas:1438
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            int OwnerPlayer = default;
            int Range = default;
            ;
            ref var mapFieldMsg = ref Msg;
            base.MapFieldMsgProc(ref Msg);
            if (mapFieldMsg.MsgID == 4363)
            {
                if (Activated())
                {
                    OwnerPlayer = FEnchantment.FPlayerIndex;
                    bool bool__109 = OwnerPlayer == mapFieldMsg.Param[1].Value;
                    if (!bool__109)
                    {
                        var call__110 = AoWHSMap.FPlayers.GetPlayers(OwnerPlayer << 0);
                        var call__111 = (byte)call__110.FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMsg.Param[1].Value);
                        bool__109 = call__111 == 2;
                    }

                    if (bool__109)
                    {
                        var call__112 = VisibilityRange();
                        if (mapFieldMsg.Param[0].Value <= call__112)
                        {
                            mapFieldMsg.Result = 1;
                        }
                    }
                }
            }

            if (mapFieldMsg.MsgID == 4362)
            {
                if (Activated())
                {
                    OwnerPlayer = FEnchantment.FPlayerIndex;
                    bool bool__113 = OwnerPlayer == mapFieldMsg.Param[1].Value;
                    if (!bool__113)
                    {
                        var call__114 = AoWHSMap.FPlayers.GetPlayers(OwnerPlayer << 0);
                        var call__115 = (byte)call__114.FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMsg.Param[1].Value);
                        bool__113 = call__115 == 2;
                    }

                    if (bool__113)
                    {
                        var call__116 = VisibilityRange();
                        if (mapFieldMsg.Param[0].Value <= call__116)
                        {
                            mapFieldMsg.Result = 1;
                        }
                    }
                }
            }

            if (mapFieldMsg.MsgID == 4365)
            {
                var call__117 = VisibilityRange();
                if (call__117 >= mapFieldMsg.Param[0].Value)
                {
                    // { Param[0] is already available. }
                    AoWHSMap.UpdatePlayerSeen(FEnchantment.FPlayerIndex, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ((TMapField)Owner).Lhx, mapFieldMsg.Param[0].Value, mapFieldMsg.Param[0].Value);
                }

                UpdateVisibleForSeatedPlayer();
            }
        }

        // GlobalTargetSpells.pas:1350
        public override void Activate()
        {
            TMapField MapF = default;
            base.Activate();
            FWatcherState = (byte)(TSet<TWatcherStateElement>.FromMask(FWatcherState) + TSet<TWatcherStateElement>.Of(TWatcherStateElement.wsActive)).Image;
            FEnchantment.Activate(FEnchantment.FPlayerIndex);
            var mapField = (TMapField)Owner;
            AoWHSMap.NotifyPlayerChanged(mapField.Xhx, mapField.Yhx, mapField.Lhx);

            MapF = (TMapField)Owner;
            var call__119 = VisibilityRange();
            var call__120 = VisibilityRange();
            AoWHSMap.UpdatePlayerSeen(FEnchantment.FPlayerIndex, MapF.Xhx, MapF.Yhx, MapF.Lhx, call__119, call__120);
            UpdateExploration();
            UpdateUnfog();
        }

        // GlobalTargetSpells.pas:1366
        public override void Deactivate()
        {
            base.Deactivate();
            FWatcherState = (byte)(TSet<TWatcherStateElement>.FromMask(FWatcherState) - TSet<TWatcherStateElement>.Of(TWatcherStateElement.wsActive)).Image;
            if (FEnchantment.FEnchantmentStatus.Contains(TGlobalEnchantmentStatusElement.getActivated))
            {
                FEnchantment.Deactivate((byte)FEnchantment.FPlayerIndex);
            }

            AoWHSMap.NotifyPlayerChanged(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ((TMapField)Owner).Lhx);
            UpdateUnfog();
        }

        // GlobalTargetSpells.pas:1403
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((TAoWMapField)(object)Info.MapField).FFieldFlags = ((TAoWMapField)(object)Info.MapField).FFieldFlags | 4096 | 16777216;
            }
        }

        // GlobalTargetSpells.pas:1226
        public void StartAnimation(sbyte X, sbyte Y, sbyte Layer)
        {
            THNWalkData Walk = default;
            TMapLevel Level = default;
            FAnimationFrame = 0;
            AoWHSMap.DynamicFixedVisibleHS.AddChild(this);
            var call__121 = AoWHSSet.FSpellControl.GetSpell(57);
            call__121.FCastSounds.Play(0);
            FUnexplored.Clear();
            if (AoWHSMap.FExplorationEnabled)
            {
                InitHNWalk(ref Walk, X, Y, 0);
                var call__122 = AoWHSMap.MapContainer.MapLevels[Layer];
                Level = call__122;
                while (true)
                {
                    var call__123 = VisibilityRange();
                    if (!(Walk.Radius <= call__123))
                        break;
                    {
                        if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                        {
                            var call__124 = Level.Field[Walk.X, Walk.Y];
                            var call__125 = ((TAoWMapField)call__124).GetExplored(AoWHSMap.FSeatedPlayer);
                            if (!call__125)
                            {
                                FUnexplored.Add(Walk.HN);
                            }
                        }

                        WalkToNextHN(ref Walk);
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:1248
        public void StopAnimation()
        {
            AoWHSMap.DynamicFixedVisibleHS.RemoveChild(this);
            FAnimationFrame = -1;
            FUnexplored.Clear();
        }

        // GlobalTargetSpells.pas:1305
        public void UpdateExploration()
        {
            int I = default;
            int OwnerPlayer = default;
            if (Activated())
            {
                {
                    I = 1;
                    int end__126 = AoWHSMap.FPlayers.Count - 1;
                    for (; I <= end__126; I += 1)
                    {
                        OwnerPlayer = FEnchantment.FPlayerIndex;
                        bool bool__127 = OwnerPlayer == I;
                        if (!bool__127)
                        {
                            var call__128 = AoWHSMap.FPlayers.GetPlayers(OwnerPlayer);
                            var call__129 = (byte)call__128.FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)I);
                            bool__127 = call__129 == 2;
                        }

                        if (bool__127)
                        {
                            var call__130 = VisibilityRange();
                            var call__131 = Yhx;
                            var call__132 = Xhx;
                            AoWHSMap.ExploreArea(call__132, call__131, Lhx, (sbyte)I, call__130);
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:1321
        public void UpdateUnfog()
        {
            int Radius = default;
            sbyte OwnerPlayer = default;
            bool bool__133 = (byte)AoWHSMap.FSeatedPlayer != 255;
            if (bool__133)
            {
                bool bool__134 = FEnchantment.FPlayerIndex == AoWHSMap.FSeatedPlayer;
                if (!bool__134)
                {
                    var call__135 = AoWHSMap.FPlayers.GetPlayers(FEnchantment.FPlayerIndex);
                    var call__136 = (byte)call__135.FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)AoWHSMap.FSeatedPlayer);
                    bool__134 = call__136 == 2;
                }

                bool__133 = bool__134;
            }

            bool bool__137 = bool__133;
            if (bool__137)
            {
                var call__138 = Activated();
                bool__137 = call__138;
            }

            if (bool__137)
            {
                Radius = VisibilityRange();
            }
            else
            {
                Radius = 0;
            }

            if (Radius != FUnfogRadius)
            {
                if (FUnfogRadius != 0)
                {
                    var call__139 = Yhx;
                    var call__140 = Xhx;
                    AoWHSMap.FogArea(call__140, call__139, Lhx, FUnfogRadius);
                }

                FUnfogRadius = (byte)Radius;
                if (FUnfogRadius != 0)
                {
                    var call__141 = Yhx;
                    var call__142 = Xhx;
                    AoWHSMap.UnFogArea(call__142, call__141, Lhx, FUnfogRadius);
                }
            }
        }

        // GlobalTargetSpells.pas:1376
        public int VisibilityRange()
        {
            int Result = default;
            Result = 8;
            bool bool__143 = Lhx != 0;
            if (!bool__143)
            {
                bool bool__144 = FEnchantment.FPlayerIndex > 0;
                if (bool__144)
                {
                    var call__145 = AoWHSMap.FPlayers.GetPlayers(FEnchantment.FPlayerIndex);
                    bool__144 = call__145.FMagicControl.FEnemyEnchantments.Contains(TPlayerEnchantmentType.petWaterMastery);
                }

                bool__143 = bool__144;
            }

            if (bool__143)
            {
                Result += 1;
                Result = (int)(((uint)Result) >> 1);
            }

            return Result;
        }

        // GlobalTargetSpells.pas:1410
        public void UpdateVisibleForSeatedPlayer()
        {
            bool Visible = default;
            var call__146 = FEnchantment.VisibleForPlayer(AoWHSMap.FSeatedPlayer);
            Visible = call__146;
            if (FVisibleForSeatedPlayer ^ Visible)
            {
                FVisibleForSeatedPlayer = Visible;
                AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
            }
        }

        // GlobalTargetSpells.pas:172


    }

    public partial class TWatcherTE : TGlobalTargetSpellTE
    {
        public TWatcherTE(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:1476
        public new const int ClassID = 135963;

        // GlobalTargetSpells.pas:1479
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            TWatcherHS Watcher = default;
            base.Process(A1, A2);
            PlayDefaultEffects(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            Watcher = new TWatcherHS(null);
            Watcher.FEnchantment.FPlayerIndex = A1.FPlayer;
            var call__147 = AoWHSSet.FSpellControl.GetSpell(57);
            Watcher.FEnchantment.FEnchantmentManaUpkeep = call__147.FManaUpkeep;
            AoWHSMap.PlaceHX(Watcher, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            if ((byte)AoWHSMap.FSeatedPlayer == FEventPlayerIndex)
            {
                Watcher.StartAnimation(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            }

            var call__148 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__149 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__149.CastingDone(A1, call__148.FBaseManaCost);
            Result = false;
            return Result;
        }

        // NetworkE.pas:230


    }

    public partial class TWatcher : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1498
        public TWatcher(TECustomNode owner) : base(owner)
        {
            var call__150 = TranslateRStr(WatcherRStr);
            this.FSpellName = call__150;
            FSpellID = 57;
            FAboveGroundOnly = false;
        }

        // GlobalTargetSpells.pas:1523
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            base.SetupTargetSelection(A1, A2);
            A2.FTargetMode = TSet<TTargetSelectionModeElement>.FromMask(A2.FTargetMode.Mask) - TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmFixedLevel, TTargetSelectionModeElement.tmValidateDistance);
            A2.FHNs.Clear();
        }

        // GlobalTargetSpells.pas:1506
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TWatcherTE Event = default;
            var call__151 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__152 = call__151.GetBusy();
            if (!call__152)
            {
                Event = new TWatcherTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
                var call__153 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__153.AddEvent(Event);
                // Original queues through AddEvent
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    // angle, animation, selected army identity
    public partial class TTornadoTE : TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1559
        public new const int ClassID = 135972;

        public int FAngle;
        public TAnimation FAnimation;
        public TArmyID FArmyID;
        // GlobalTargetSpells.pas:1553
        public override void Destroy()
        {
            FArmyID.Free();
            base.Destroy();
        }

        // GlobalTargetSpells.pas:1593
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FArmyID, null, 40);
        }

        // GlobalTargetSpells.pas:1547
        public TTornadoTE(TECustomNode owner) : base(owner)
        {
            FArmyID = null;
        }

        // GlobalTargetSpells.pas:1660
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            TUnitUserList MovedUnits = default;
            byte Owner = default;
            byte Selection = default;
            TXYL Location = default;
            int Chance = default;
            bool Exposed = default;
            TArmyHS Army = default;
            TAbstractUnit UnitObj = default;
            TAoWMapField MapF = default;
            int I = default;
            TArmyEventLog Log = default;
            TImageSequenceAnimation Anim = default;
            TArmyHS Placed = default;
            TAIGroup Group = default;
            if (A2 == 0)
            {
                FAngle = 0;
                var call__154 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__155 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__155.CastingDone(A1, call__154.FBaseManaCost);
                AoWHSMap.ValidateObservation(A1.FPlayer, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                bool bool__156 = A1.FPlayer == AoWHSMap.FSeatedPlayer;
                if (!bool__156)
                {
                    var call__157 = AoWHSMap.Watching(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                    bool__156 = call__157;
                }

                if (bool__156)
                {
                    var call__158 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                    call__158.FCastSounds.Play(0);
                }

                Anim = (TImageSequenceAnimation)new TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = AoWHSMap;
                Anim.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
                Anim.Activate();
            }

            if (A2 == 80)
            {
                var call__159 = AoWHSMap.MapContainer.GetField(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
                MapF = (TAoWMapField)call__159;
                bool bool__160 = FArmyID != null;
                if (bool__160)
                {
                    var call__161 = MapF.GetMagicShieldActive();
                    bool__160 = !call__161;
                }

                if (bool__160)
                {
                    FArmyID.RetrieveArmy(ref Army, ref Selection);
                    bool bool__162 = TSet<byte>.WithBounds(0, 255).With(1).Contains((byte)MapF.OverlayType);
                    if (!bool__162)
                    {
                        var call__163 = MapF.FindNoneTransparentHS(typeof(TStructure));
                        bool__162 = call__163 != null;
                    }

                    Exposed = !bool__162;
                    {
                        I = 0;
                        int end__164 = Army.FArmy.Count - 1;
                        for (; I <= end__164; I += 1)
                        {
                            if ((Selection & (1 << I)) != 0)
                            {
                                var call__165 = Army.FArmy.GetUnit(I);
                                UnitObj = call__165;
                                switch (UnitObj.GetUnitLevel())
                                {
                                    case 1:
                                        Chance = 90;
                                        break;
                                    case 2:
                                        Chance = 70;
                                        break;
                                    case 3:
                                        Chance = 50;
                                        break;
                                    case 4:
                                        Chance = 30;
                                        break;
                                    default:
                                        Chance = 30;
                                        break;
                                }

                                if (!Exposed)
                                {
                                    Chance = Chance / 2;
                                }

                                var call__166 = AoWHSMap.Random(100);
                                if (Chance < call__166)
                                {
                                    Selection = (byte)(Selection & ~(1 << I));
                                }
                            }
                        }
                    }

                    var call__167 = Army.FArmy.Transporter(255);
                    if (call__167 != null)
                    {
                        var call__168 = Army.FArmy.Transporter(Selection);
                        if (call__168 != null)
                        {
                            Selection = 255;
                        }
                        else
                        {
                            Selection = 0;
                        }
                    }

                    if (Selection != 0)
                    {
                        Log = new TArmyEventLog(null);
                        Log.FPlayer = A1.FPlayer;
                        var call__169 = TranslateRStr(TornadoRStr);
                        var call__170 = TranslateRStr(XCastRStr);
                        var call__171 = Rtl.Rtl.Format(call__170, [call__169]);
                        Log.FArmyText = call__171;
                        var call__172 = TranslateRStr(TornadoRStr);
                        Log.FArmyMessages.Add(call__172);
                        Log.FArmyIcon = 2;
                        Log.FArmyView.Setup(Army.FArmy, Selection);
                        var call__173 = Army.Yhx;
                        var call__174 = Army.Xhx;
                        DistributeLocationEventLog(Log, (sbyte)FEventPlayerIndex, call__174, call__173, Army.Lhx, true);
                        Log.Release();
                        var call__175 = AoWHSSet.FSpellControl.GetSpell(68);
                        var call__176 = ((TTornado)call__175).CalculateWarpLocation(Army, Selection);
                        Location.XYL = call__176;
                        if (Location.XYL != 16777215)
                        {
                            bool bool__177 = A1.FPlayer == AoWHSMap.FSeatedPlayer;
                            if (!bool__177)
                            {
                                var call__178 = AoWHSMap.Watching(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                                bool__177 = call__178;
                            }

                            if (bool__177)
                            {
                                var call__179 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                call__179.FCastSounds.Play(1);
                            }

                            bool bool__180 = AoWHSMap.FSeatedPlayer == A1.FPlayer;
                            if (!bool__180)
                            {
                                var call__181 = AoWHSMap.VisibleForPlayer(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, AoWHSMap.FSeatedPlayer);
                                bool__180 = call__181;
                            }

                            if (bool__180)
                            {
                                var call__182 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                var call__183 = call__182.FCastImages.ImageSequences[1];
                                if (call__183 != null)
                                {
                                    Anim = new TImageSequenceAnimation(null);
                                    var call__184 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                    var call__185 = call__184.FCastImages.ImageSequences[1];
                                    Anim.SetImageSequence(call__185);
                                    AoWHSMap.PlaceHX(Anim, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
                                }
                            }

                            bool bool__186 = AoWHSMap.FSeatedPlayer == A1.FPlayer;
                            if (!bool__186)
                            {
                                var call__187 = AoWHSMap.VisibleForPlayer(Location.X, Location.Y, Location.L, AoWHSMap.FSeatedPlayer);
                                bool__186 = call__187;
                            }

                            if (bool__186)
                            {
                                var call__188 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                var call__189 = call__188.FCastImages.ImageSequences[1];
                                if (call__189 != null)
                                {
                                    Anim = new TImageSequenceAnimation(null);
                                    var call__190 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                    var call__191 = call__190.FCastImages.ImageSequences[1];
                                    Anim.SetImageSequence(call__191);
                                    AoWHSMap.PlaceHX(Anim, Location.X, Location.Y, Location.L);
                                }
                            }

                            MovedUnits = new TUnitUserList(null);
                            Owner = (byte)Army.FArmy.FPlayer;
                            var call__192 = AoWHSMap.MapContainer.GetFieldXYL(Location.XYL);
                            MapF = (TAoWMapField)call__192;
                            var call__193 = Army.FArmy.ValidateSelection(255);
                            if (call__193 != Selection)
                            {
                                {
                                    I = Army.FArmy.Count - 1;
                                    byte end__194 = 0;
                                    for (; I >= end__194; I -= 1)
                                    {
                                        if ((Selection & (1 << I)) != 0)
                                        {
                                            var call__195 = Army.FArmy.GetUnit(I);
                                            UnitObj = call__195;
                                            MovedUnits.AddUnit(UnitObj);
                                            var call__196 = UnitObj.Place(Location.X, Location.Y, Location.L, (sbyte)(byte)Army.FArmy.FPlayer, TSet<TUnitPlaceModeElement>.Of());
                                            Placed = call__196;
                                            if (Placed != null)
                                            {
                                                Placed.FArmy.UpdateExploration();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MovedUnits.AddUnits(Army.FArmy);
                                Army.MoveTo(MapF, 0);
                            }

                            if (Owner == 0)
                            {
                                var call__197 = UnitsToIndependentRelation(MovedUnits, 5);
                                var call__198 = AoWHSMap.FAIGroupManager.CreateGroup(0, call__197, TGroupBehavior.gbHunt);
                                Group = call__198;
                                Group.AddUnits(MovedUnits);
                                ((THuntAG)Group).FTargetUnitIDs.Add(A1.FUnitID);
                                Group.Release();
                            }

                            MovedUnits.Free();
                        }
                    }
                }

                FAnimation.Free();
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:1599
        public void ShowAnimation(TAnimation Animation, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int XShift = default;
            int Height = default;
            int X = default;
            int Y = default;
            float Angle = default;
            int I = default;
            int Direction = default;
            int Radius = default;
            int Count = default;
            int Last = default;
            if (AoWHSMap.ViewL == Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer)
            {
                HXtoHP(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                FAngle = FAngle + 20 + FAnimation.FFrameCounter / 5;
                RandSeed = AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    byte end__199 = 200;
                    for (; I <= end__199; I += 1)
                    {
                        Direction = Random(360);
                        if (Animation.FFrameCounter >= (int)(((uint)I) >> 2) && Animation.FFrameCounter <= (int)(((uint)I) >> 4) + 68)
                        {
                            Radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 7;
                            Height = 200 - I - 16;
                            var call__200 = Math.Sin((double)((double)FAnimation.FFrameCounter * 0.1) + (double)I * 0.02);
                            XShift = Round((double)((double)call__200 * (200 - I)) * 0.25) + 16;
                            var call__201 = Math.PI;
                            Angle = (float)((double)((double)(Direction + FAngle) * call__201) / 180);
                            if (AoWEngine.FAnimationDetail == 0)
                            {
                                Last = 1;
                            }
                            else
                            {
                                if (AoWEngine.FAnimationDetail == 1)
                                {
                                    Last = (int)(((uint)(200 - I)) >> 5);
                                }
                                else
                                {
                                    Last = (int)(((uint)(200 - I)) >> 4);
                                }
                            }

                            {
                                Count = Last;
                                byte end__202 = 0;
                                for (; Count >= end__202; Count -= 1)
                                {
                                    X = Round((double)Math.Cos(Angle) * Radius) + XShift;
                                    Y = Round((double)((double)Math.Sin(Angle) * Radius) * 0.5) + Height;
                                    var call__203 = AoWHSSet.FSpellControl.GetSpell(68);
                                    call__203.FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                                    Angle = (float)((double)0.035 + Angle);
                                }
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:215


    }

    public partial class TTornado : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1798
        public TTornado(TECustomNode owner) : base(owner)
        {
            var call__204 = TranslateRStr(TornadoRStr);
            this.FSpellName = call__204;
            FSpellID = 68;
            FAboveGroundOnly = true;
        }

        // GlobalTargetSpells.pas:1885
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            TTargetSelectionControl Target = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
        }

        // GlobalTargetSpells.pas:1865
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TTornadoTE Event = default;
            TArmyHS Army = default;
            var call__205 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__206 = call__205.GetBusy();
            if (!call__206)
            {
                Event = new TTornadoTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
                var call__207 = AoWHSMap.MapContainer.GetField(A2, A3, A4);
                var call__208 = ((TAoWMapField)call__207).GetArmyHS();
                Army = call__208;
                if (Army != null)
                {
                    var call__209 = Army.FArmy.CreateID(255);
                    Event.FArmyID = call__209;
                }

                var call__210 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__210.AddEvent(Event);
                // Original queues through AddEvent
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:1806
        public bool ValidWarpLocation(TArmyHS Army, sbyte X, sbyte Y, sbyte Layer)
        {
            bool Result = default;
            TAoWMapField MapF = default;
            TMapFieldMsg Msg = default;
            var call__211 = AoWHSMap.MapContainer.GetField(X, Y, Layer);
            MapF = (TAoWMapField)call__211;
            var call__212 = MapF.GetArmyHS();
            bool bool__213 = call__212 == null;
            if (bool__213)
            {
                var call__214 = MapF.GetPlayer();
                bool__213 = call__214 == -1;
            }

            Result = bool__213;
            if (Result)
            {
                Msg.MsgID = 4384;
                Msg.Param[0] = Army;
                Msg.Result = 0;
                MapF.SendMapFieldMsg(ref Msg);
                bool bool__215 = Msg.Result.Value == 0;
                if (bool__215)
                {
                    var call__216 = Army.FArmy.CanPlace(X, Y, Layer);
                    bool__215 = call__216;
                }

                Result = bool__215;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:1823
        public int CalculateWarpLocation(TArmyHS Army, byte Selection)
        {
            int Result = default;
            TSpellMoveTable Table = default;
            THNWalkData Walk = default;
            TMapLevel MapLevel = default;
            TMapField MapF = default;
            TList Locations = default;
            bool Found = default;
            int Index = default;
            int X = default;
            Locations = new TList();
            Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref Table));
            var call__217 = Army.Yhx;
            var call__218 = Army.Xhx;
            InitHNWalk(ref Walk, call__218, call__217, 10);
            var call__219 = AoWHSMap.MapContainer.MapLevels[Army.Lhx];
            MapLevel = call__219;
            while (Walk.Radius < 15)
            {
                X = Walk.X;
                if (X >= 0 && Walk.Y >= 0 && X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    var call__220 = MapLevel.GetField(X, Walk.Y);
                    MapF = call__220;
                    if (Table.Cost[MapF.TerrainType][MapF.OverlayType] != -1)
                    {
                        Locations.Add(MapF);
                    }
                }

                WalkToNextHN(ref Walk);
            }

            Found = false;
            Result = 16777215;
            while (Locations.Count != 0 && !Found)
            {
                var call__221 = Random(Locations.Count);
                Index = call__221;
                var call__222 = Locations[Index];
                MapF = (TMapField)call__222;
                var call__223 = ValidWarpLocation(Army, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                if (call__223)
                {
                    Found = true;
                    var call__224 = MapF.XYL;
                    Result = call__224;
                }
                else
                {
                    Locations.Delete(Index);
                }
            }

            Locations.Free();
            return Result;
        }
    }

    // angle, animation, selected army identity
    public partial class TVortexTE : TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1905
        public new const int ClassID = 135973;

        public int FAngle;
        public TAnimation FAnimation;
        public TArmyID FArmyID;
        // GlobalTargetSpells.pas:1899
        public override void Destroy()
        {
            FArmyID.Free();
            base.Destroy();
        }

        // GlobalTargetSpells.pas:1939
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FArmyID, null, 40);
        }

        // GlobalTargetSpells.pas:1893
        public TVortexTE(TECustomNode owner) : base(owner)
        {
            FArmyID = null;
        }

        // GlobalTargetSpells.pas:1991
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            TAnimation Anim = default;
            TArmyEventLog Log = default;
            byte Selection = default;
            bool Damaged = default;
            int Damage = default;
            int I = default;
            TArmyHS Army = default;
            TAbstractUnit UnitObj = default;
            if (A2 == 0)
            {
                FAngle = 0;
                var call__225 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__226 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__226.CastingDone(A1, call__225.FBaseManaCost);
                AoWHSMap.ValidateObservation(A1.FPlayer, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                bool bool__227 = A1.FPlayer == AoWHSMap.FSeatedPlayer;
                if (!bool__227)
                {
                    var call__228 = AoWHSMap.Watching(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                    bool__227 = call__228;
                }

                if (bool__227)
                {
                    var call__229 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                    call__229.FCastSounds.Play(0);
                }

                Anim = new TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = AoWHSMap;
                Anim.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
                Anim.Activate();
            }

            if (A2 == 80)
            {
                bool bool__230 = FArmyID != null;
                if (bool__230)
                {
                    var call__231 = FArmyID.RetrieveArmy(ref Army, ref Selection);
                    bool__230 = call__231;
                }

                if (bool__230)
                {
                    Log = new TArmyEventLog(null);
                    var call__232 = TranslateRStr(VortexDamageRStr);
                    Log.FArmyText = call__232;
                    Log.FPlayer = A1.FPlayer;
                    var call__233 = TranslateRStr(VortexRStr);
                    Log.FArmyMessages.Add(call__233);
                    Log.FArmyIcon = 2;
                    Log.FArmyView.Setup(Army.FArmy, 255);
                    Army.FArmy.BeginUpdate();
                    Damaged = false;
                    {
                        I = 0;
                        int end__234 = Army.FArmy.Count - 1;
                        for (; I <= end__234; I += 1)
                        {
                            var call__235 = Army.FArmy.GetUnit(I);
                            UnitObj = call__235;
                            if (!UnitObj.GetMoveTypes().Contains(TMoveType.mtFlying))
                            {
                                if (UnitObj.GetAbilityEnabled(2))
                                {
                                    Damage = 7;
                                }
                                else
                                {
                                    if (UnitObj.GetAbilityEnabled(23))
                                    {
                                        Damage = 10;
                                    }
                                    else
                                    {
                                        Damage = 0;
                                    }
                                }

                                Damage = UnitObj.ExecuteDamageRole(8, Damage, TSet<TDamageType>.Of(TDamageType.dtPhysical));
                                if (Damage > UnitObj.GetHitPoints())
                                {
                                    Damage = UnitObj.GetHitPoints();
                                }

                                if (Damage > 0)
                                {
                                    var call__236 = UnitObj.GetHitPoints();
                                    UnitObj.SetHitPoints((sbyte)(call__236 - Damage));
                                    Damaged = true;
                                }
                            }
                            else
                            {
                                Damage = 0;
                            }

                            Log.FDamageValues.Add((byte)Damage);
                            Log.FDamageTypes.Add(7);
                        }
                    }

                    if (Damaged)
                    {
                        Log.ValidateGameOver();
                        var call__237 = Army.Yhx;
                        var call__238 = Army.Xhx;
                        DistributeLocationEventLog(Log, Army.FArmy.FPlayer, call__238, call__237, Army.Lhx, true);
                    }

                    Log.Release();
                    Army.FArmy.Validate();
                    Army.FArmy.ValidateTerrainEx(((TMapField)Army.Owner).TerrainType, ((TMapField)Army.Owner).OverlayType, A1.FPlayer);
                    Army.FArmy.EndUpdate();
                }

                FAnimation.Free();
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:1945
        public void ShowAnimation(TAnimation Animation, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int XShift = default;
            int Height = default;
            int X = default;
            int Y = default;
            float Angle = default;
            int I = default;
            int Direction = default;
            int Radius = default;
            int Count = default;
            int J = default;
            if (AoWHSMap.ViewL == Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer)
            {
                HXtoHP(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                FAngle = FAngle + 20 + FAnimation.FFrameCounter / 5;
                RandSeed = AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    byte end__239 = 200;
                    for (; I <= end__239; I += 1)
                    {
                        Direction = Random(360);
                        if (Animation.FFrameCounter >= (int)(((uint)(200 - I)) >> 2) && Animation.FFrameCounter <= (int)(((uint)I) >> 4) + 68)
                        {
                            Radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 18;
                            Height = -16;
                            XShift = 16;
                            var call__240 = Math.PI;
                            Angle = (float)((double)((double)-(Direction + FAngle) * call__240) / 180);
                            if (AoWEngine.FAnimationDetail == 0)
                            {
                                Count = 1;
                            }
                            else
                            {
                                if (AoWEngine.FAnimationDetail == 1)
                                {
                                    Count = (int)(((uint)(200 - I)) >> 5);
                                }
                                else
                                {
                                    Count = (int)(((uint)(200 - I)) >> 4);
                                }
                            }

                            {
                                J = Count;
                                byte end__241 = 0;
                                for (; J >= end__241; J -= 1)
                                {
                                    X = Round((double)Math.Cos(Angle) * Radius) + XShift;
                                    Y = Round((double)((double)Math.Sin(Angle) * Radius) / 1.75) + Height;
                                    var call__242 = AoWHSSet.FSpellControl.GetSpell(70);
                                    call__242.FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                                    Angle = (float)((double)0.035 + Angle);
                                }
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:242


    }

    public partial class TVortex : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2069
        public TVortex(TECustomNode owner) : base(owner)
        {
            var call__243 = TranslateRStr(VortexRStr);
            this.FSpellName = call__243;
            FSpellID = 70;
            FAboveGroundOnly = false;
        }

        // GlobalTargetSpells.pas:2120
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            TTargetSelectionControl Target = default;
            // { Use the incoming target. }
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            A2.FOnValidateTarget = ValidateTargetEvent;
        }

        // GlobalTargetSpells.pas:2100
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TVortexTE Event = default;
            TArmyHS Army = default;
            var call__244 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__245 = call__244.GetBusy();
            if (!call__245)
            {
                Event = new TVortexTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
                var call__246 = AoWHSMap.MapContainer.GetField(A2, A3, A4);
                var call__247 = ((TAoWMapField)call__246).GetArmyHS();
                Army = call__247;
                if (Army != null)
                {
                    var call__248 = Army.FArmy.CreateID(255);
                    Event.FArmyID = call__248;
                }

                var call__249 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__249.AddEvent(Event);
                // Original queues through AddEvent
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:2077
        public override bool ValidTargetSelection(TTargetSelectionControl A1)
        {
            bool Result = default;
            TMapField MapF = default;
            if (A1.FTargetX != -1)
            {
                var call__250 = AoWHSMap.MapContainer.MapLevels[A1.FTargetL];
                var call__251 = call__250.GetField(A1.FTargetX, A1.FTargetY);
                MapF = call__251;
                Result = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)MapF.TerrainType);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:2088
        public void ValidateTargetEvent(TTargetSelectionControl Target, sbyte X, sbyte Y, sbyte Layer, ref bool Valid)
        {
            TMapLevel MapLevel = default;
            if (X != -1)
            {
                var call__252 = AoWHSMap.MapContainer.MapLevels[Layer];
                MapLevel = call__252;
                var call__253 = MapLevel.GetField(X, Y);
                Valid = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)call__253.TerrainType);
            }
            else
            {
                Valid = false;
            }
        }
    }

    // animation
    public partial class THealingShowersTE : TGlobalTargetSpellTE
    {
        public THealingShowersTE(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:2138
        public new const int ClassID = 135975;

        public TAnimation FAnimation;
        // GlobalTargetSpells.pas:2190
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            TAbstractUnit UnitObj = default;
            THNWalkData Walk = default;
            TArmyHS Army = default;
            TMapLevel Level = default;
            TImageSequenceAnimation Anim = default;
            bool Healed = default;
            bool PlaySound = default;
            int I = default;
            THexagonSprite Cloud = default;
            base.Process(A1, A2);
            if (A2 == 0)
            {
                AoWHSMap.ValidateTerrainObservation(A1.FPlayer, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                bool bool__254 = A1.FPlayer == AoWHSMap.FSeatedPlayer;
                if (!bool__254)
                {
                    var call__255 = AoWHSMap.WatchingTerrain(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                    bool__254 = call__255;
                }

                if (bool__254)
                {
                    var call__256 = AoWHSSet.FSpellControl.GetSpell(73);
                    call__256.FCastSounds.Play(0);
                }

                Anim = (TImageSequenceAnimation)new TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = AoWHSMap;
                Anim.FTag = (AoWHSMap.ViewWidth & 65535) | (AoWHSMap.ViewHeight << 16);
                Anim.Activate();
                var call__257 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__257.CastingDone(A1, FEventMana);
            }

            if (A2 == 50)
            {
                PlaySound = false;
                AoWHSMap.ChangeTerrainEx(null, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, null);
                var call__258 = AoWHSMap.MapContainer.MapLevels[Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer];
                Level = call__258;
                InitHNWalk(ref Walk, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, 0);
                while (Walk.Radius <= 1)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Level.Height > Walk.Y)
                    {
                        var call__259 = Level.GetField(Walk.X, Walk.Y);
                        var call__260 = ((TAoWMapField)call__259).GetMagicShieldActive();
                        if (!call__260)
                        {
                            Healed = false;
                            var call__261 = Level.GetField(Walk.X, Walk.Y);
                            var call__262 = ((TAoWMapField)call__261).GetArmyHS();
                            Army = call__262;
                            if (Army != null)
                            {
                                {
                                    I = 0;
                                    int end__263 = Army.FArmy.Count - 1;
                                    for (; I <= end__263; I += 1)
                                    {
                                        var call__264 = Army.FArmy.GetUnit(I);
                                        UnitObj = call__264;
                                        var call__265 = UnitObj.GetHitPoints();
                                        var call__266 = UnitObj.GetHits();
                                        if (call__265 < call__266)
                                        {
                                            var call__267 = UnitObj.GetHitPoints();
                                            UnitObj.SetHitPoints((sbyte)(call__267 + 5));
                                            Healed = true;
                                        }

                                        if (UnitObj.GetAbilityEnabled(96))
                                        {
                                            UnitObj.RemoveAbility(96);
                                            Healed = true;
                                        }
                                    }
                                }

                                Army.FArmy.Update();
                                bool bool__268 = Healed;
                                if (bool__268)
                                {
                                    var call__269 = Army.Yhx;
                                    var call__270 = Army.Xhx;
                                    var call__271 = AoWHSMap.VisibleForPlayer(call__270, call__269, Army.Lhx, AoWHSMap.FSeatedPlayer);
                                    bool__268 = call__271;
                                }

                                if (bool__268)
                                {
                                    bool bool__272 = PlaySound || (byte)Army.FArmy.FPlayer == (byte)AoWHSMap.FSeatedPlayer;
                                    if (!bool__272)
                                    {
                                        var call__273 = Army.Yhx;
                                        var call__274 = Army.Xhx;
                                        var call__275 = AoWHSMap.Watching(call__274, call__273, Army.Lhx, 1);
                                        bool__272 = call__275;
                                    }

                                    PlaySound = bool__272;
                                    Anim = new TImageSequenceAnimation(null);
                                    var call__276 = AoWHSSet.FAbilityControl.GetAbility(47);
                                    var call__277 = call__276.FImageSequences.ImageSequences[0];
                                    Anim.SetImageSequence(call__277);
                                    Anim.FAnimationLevel = 27;
                                    var call__278 = Army.Yhx;
                                    var call__279 = Army.Xhx;
                                    AoWHSMap.PlaceHX(Anim, call__279, call__278, Army.Lhx);
                                }
                            }

                            var call__280 = Level.GetField(Walk.X, Walk.Y);
                            var call__281 = call__280.FindOwnedChild(132990);
                            Cloud = (THexagonSprite)call__281;
                            if (Cloud != null)
                            {
                                Cloud.Free();
                            }
                        }
                    }

                    WalkToNextHN(ref Walk);
                }

                if (PlaySound)
                {
                    var call__282 = AoWHSSet.FAbilityControl.GetAbility(47);
                    call__282.FSFXLibrary.Play(0);
                }
            }

            if (A2 == 80)
            {
                FAnimation.Free();
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:2141
        public void ShowAnimation(TAnimation Animation, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int XShift = default;
            int YShift = default;
            int Speed = default;
            int Xhp = default;
            int Yhp = default;
            int Count = default;
            int I = default;
            int J = default;
            int Frame = default;
            if (AoWHSMap.ViewL == Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer)
            {
                HXtoHP(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                var call__283 = AoWEngine.FRegistry.GetDetailLevel();
                switch (call__283)
                {
                    case TDetailLevel.dtlLow:
                        Count = 50;
                        break;
                    case TDetailLevel.dtlHigh:
                        Count = 160;
                        break;
                    default:
                        Count = 100;
                        break;
                }

                RandSeed = AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    int end__284 = Count;
                    for (; I <= end__284; I += 1)
                    {
                        X = Random(100) - 50;
                        Y = Random(100) - 50;
                        Speed = Random(5) + 5;
                        var call__285 = Random(30);
                        Frame = Animation.FFrameCounter - call__285;
                        if (Frame >= 0 && Frame < 50)
                        {
                            Frame = Frame % 30;
                            X -= Round((double)(Frame * 4) * ((double)Speed * 0.2));
                            Y += Round((double)(Frame * 20) * ((double)Speed * 0.2));
                            if (Y < 500)
                            {
                                {
                                    J = 0;
                                    byte end__286 = 10;
                                    for (; J <= end__286; J += 1)
                                    {
                                        XShift = X - Round((double)(J * 2) * 0.1);
                                        YShift = Y + Round((double)(J * 10) * 0.1);
                                        var call__287 = AoWHSSet.FSpellControl.GetSpell(73);
                                        call__287.FCastImages.Show(0, Xhp + XShift + 120, Yhp + YShift - 500 + 30, 0, SurfaceDesc);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:2181
        public void ChangeTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            switch (TerrainType)
            {
                case 5:
                    TerrainType = 4;
                    break;
                case 2:
                    TerrainType = 4;
                    break;
                case 4:
                    TerrainType = 1;
                    break;
            }
        }

        // NetworkE.pas:230


    }

    public partial class THealingShowers : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2277
        public THealingShowers(TECustomNode owner) : base(owner)
        {
            var call__288 = TranslateRStr(HealingShowersRStr);
            this.FSpellName = call__288;
            FSpellID = 73;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msWater;
        }

        // GlobalTargetSpells.pas:2295
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__289 = RadToHN(2) - 1;
                for (; I <= end__289; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:2308
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            THealingShowersTE Event = default;
            Event = new THealingShowersTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__290 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__290.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:2291
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            Result = base.ValidTargetSelectionMapF(A1);
            return Result;
        }

        // GlobalTargetSpells.pas:2287
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = default;
            Result = base.ValidTargetMapF(A1);
            return Result;
        }
    }

    public partial class TSmokeCloudSpell : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2325
        public TSmokeCloudSpell(TECustomNode owner) : base(owner)
        {
            var call__291 = TranslateRStr(SmokeCloudRStr);
            this.FSpellName = call__291;
            FSpellID = 74;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msFire;
        }

        // GlobalTargetSpells.pas:2373
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            TEResource Resource = default;
            TSmokeCloud Cloud = default;
            var call__292 = ((TGlobalTargetSpellTE)A1).GetSource();
            AoWHSMap.ValidateObservation(call__292.FPlayer, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).Layer, 2);
            var call__293 = ((TGlobalTargetSpellTE)A1).GetSource();
            bool bool__294 = call__293.FPlayer == AoWHSMap.FSeatedPlayer;
            if (!bool__294)
            {
                var call__295 = AoWHSMap.Watching(Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).Layer, 2);
                bool__294 = call__295;
            }

            if (bool__294)
            {
                var call__296 = AoWHSSet.FSpellControl.GetSpell(((TGlobalTargetSpellTE)A1).FEventSpellID);
                call__296.FCastSounds.Play(0);
            }

            var call__297 = ((TMapObjectResourceList)(TObject)AoWHSSet.MapObjectResourceList).FindChild(135747);
            Resource = (TEResource)call__297;
            if (Resource != null)
            {
                Cloud = (TSmokeCloud)Resource.CreateResourceUser();
                Cloud.FPlayer = A2.FPlayer;
                Cloud.FTurnsRemaining = -1;
                Cloud.FEnchantment.FPlayerIndex = A2.FPlayer;
                Cloud.FEnchantment.FEnchantmentManaUpkeep = this.FManaUpkeep;
                Cloud.FDrifting = false;
                var call__298 = AoWHSMap.PlaceHX(Cloud, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)A1).FLocation).Layer);
                if (!call__298)
                {
                    Cloud.Free();
                }

                var call__299 = AoWHSSet.FSpellControl.GetSpell(this.FSpellID);
                call__299.CastingDone(A2, this.FBaseManaCost);
            }

            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:2343
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__300 = RadToHN(4) - 1;
                for (; I <= end__300; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:2356
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TGlobalTargetSpellTE Event = default;
            Event = new TGlobalTargetSpellTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__301 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__301.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:2339
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            Result = base.ValidTargetSelectionMapF(A1);
            return Result;
        }

        // GlobalTargetSpells.pas:2335
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = default;
            Result = base.ValidTargetMapF(A1);
            return Result;
        }
    }

    public partial class TGlobalTargetSpellCalls : TSpell
    {
        public TGlobalTargetSpellCalls(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:303
        public virtual void ShowSelection(TTargetSelectionControl Target, int HN, int X, int Y, in TSurfaceDesc SD)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:305
        public virtual void SelectionChanged(TObject Sender)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:306
        public virtual void SetupSelection(int Caster, int Target)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:307
        public virtual bool Selected(int Caster, int X, int Y, int Layer)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:308
        public virtual bool ValidSelection(int Target)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:309
        public virtual bool ValidSelectionMapF(int MapF)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:310
        public virtual bool ValidMapF(int MapF)
        {
            throw EAbstractError.Create("Abstract Error");
        }
    }

    public struct TSpellNameView
    {
        public FixedArray_byte_8 Reserved;
        public AnsiString Name;
    }

    public partial class TTargetSelectionView : TObject
    {
        public TCustomImageLibrary ImageLibrary;
        public byte Status;
        public TSet<TTargetSelectionModeElement> Mode;
        public TShowTargetSelectionEvent OnShow;
        public TValidateTargetEvent OnValidate;
        public int Distance;
        public int CenterX;
        public int CenterY;
        public int CenterLayer;
        public TEventList Changes;
        public TTargetSelectionChange Change;
        public sbyte X;
        public sbyte Y;
        public sbyte Layer;
        public TStrings Text;
        public AnsiString Caption;
        public AnsiString CancelCaption;
        public ImageLib.TLibraryImage Preview;
        public ImageLib.TLibraryImage ValidPreview;
        public ImageLib.TLibraryImage InvalidPreview;
    }

    public struct TTargetSelectionFields
    {
        public object VMT;
        public TCustomImageLibrary ImageLibrary;
        public int Reserved08;
        public byte Status;
        public TSet<TTargetSelectionModeElement> Mode;
        public ushort Padding0E;
        public TShowTargetSelectionEvent OnShow;
        public TValidateTargetEvent OnValidate;
        public int Distance;
        public int CenterX;
        public int CenterY;
        public int CenterLayer;
        public int Reserved30;
        public TEventList Changes;
        public TTargetSelectionChange Change;
        public sbyte X;
        public sbyte Y;
        public sbyte Layer;
        public TStrings Text;
        public AnsiString Caption;
        public AnsiString CancelCaption;
        public int Reserved48;
        public int Reserved4C;
        public ImageLib.TLibraryImage Preview;
        public ImageLib.TLibraryImage ValidPreview;
        public ImageLib.TLibraryImage InvalidPreview;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TMapSeatedPlayerView
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    // { Typed call-only methods retain original argument evaluation order without
// altering the foreign headers or their implementation signatures. }
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct TSpellLocation
    {
        [FieldOffset(0)]
        public sbyte X;
        [FieldOffset(1)]
        public sbyte Y;
        [FieldOffset(2)]
        public sbyte Layer;
        [FieldOffset(3)]
        public byte Reserved;
    }

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct TMapFieldFlagView
    {
        [FieldOffset(0)]
        public FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    [StructLayout(LayoutKind.Explicit, Size = 257)]
    public struct TSpellMoveTable
    {
        [FieldOffset(0)]
        public byte Flags;
        [FieldOffset(1)]
        public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Cost;
    }

    public struct TSpellAnimationFields
    {
        public FixedArray_byte_36 Reserved;
        public TShowAnimationEvent OnShow;
    }

    [StructLayout(LayoutKind.Explicit, Size = 110)]
    public struct TSpellEngineFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_109 Reserved;
        [FieldOffset(109)]
        public byte Detail;
    }

    public struct TSpellAIGroupFields
    {
        public FixedArray_byte_80 Reserved;
        public TIntegerList Enemies;
    }

    public struct TSpellRegistryFields
    {
        public FixedArray_byte_92 Reserved;
        public TAoWRegistry Registry;
    }

    public struct TSpellEngineSetFields
    {
        public FixedArray_byte_100 Reserved;
        public TAoWHSSet HSSet;
    }

    public static partial class GlobalTargetSpells
    {
        // GlobalTargetSpells.pas:2417
        public static void RegisterGlobalTargetSpells(TAoWEngine A1)
        {
            TSpellControl Control = default;
            Control = A1.FGameSet.FSpellControl;
            A1.RegisterEClasses([EClass.Of<TFreezeWaterTE>(), EClass.Of<TFrozenWaterHS>(), EClass.Of<TFireBarrierTE>(), EClass.Of<TLevelTerrainTE>(), EClass.Of<TRejuvenateTE>(), EClass.Of<TWatcherTE>(), EClass.Of<TWatcherHS>(), EClass.Of<TTornadoTE>(), EClass.Of<TVortexTE>(), EClass.Of<THealingShowersTE>(), EClass.Of<TGlobalTargetSpellTE>()]);
            var call__302 = new TFreezeWater(null);
            Control.RegisterSpell(call__302);
            var call__303 = new TFireBarrier(null);
            Control.RegisterSpell(call__303);
            var call__304 = new TLevelTerrain(null);
            Control.RegisterSpell(call__304);
            var call__305 = new TRejuvenate(null);
            Control.RegisterSpell(call__305);
            var call__306 = new TWatcher(null);
            Control.RegisterSpell(call__306);
            var call__307 = new TTornado(null);
            Control.RegisterSpell(call__307);
            var call__308 = new TVortex(null);
            Control.RegisterSpell(call__308);
            var call__309 = new THealingShowers(null);
            Control.RegisterSpell(call__309);
            var call__310 = new TSmokeCloudSpell(null);
            Control.RegisterSpell(call__310);
        }

        // GlobalTargetSpells.pas:2435
        public static void UnregisterGlobalTargetSpells(int A1)
        {
            TSpellControl Control = default;
            Control = ((TAoWEngine)(object)A1).FGameSet.FSpellControl;
            Control.UnregisterSpellID(10);
            Control.UnregisterSpellID(11);
            Control.UnregisterSpellID(18);
            Control.UnregisterSpellID(41);
            Control.UnregisterSpellID(57);
            Control.UnregisterSpellID(68);
            Control.UnregisterSpellID(70);
            Control.UnregisterSpellID(73);
            Control.UnregisterSpellID(74);
            ReferenceFromId<TEngine>(A1).UnregisterEClasses([EClass.Of<TFreezeWaterTE>(), EClass.Of<TFrozenWaterHS>(), EClass.Of<TFireBarrierTE>(), EClass.Of<TLevelTerrainTE>(), EClass.Of<TRejuvenateTE>(), EClass.Of<TWatcherTE>(), EClass.Of<TWatcherHS>(), EClass.Of<TTornadoTE>(), EClass.Of<TVortexTE>(), EClass.Of<THealingShowersTE>(), EClass.Of<TGlobalTargetSpellTE>()]);
        }

        // { Import cells bind to the shared globals; distinct references preserve the original reloads. }
        public static Type SpellStructureClass = typeof(TStructure);
        public static readonly AnsiString SpellCancelRStr = (AnsiString)"Cancel";

        // GlobalTargetSpells.pas:552
        public static bool GlobalTargetCanActivate(TGlobalTargetSpell Spell, int Caster, ref AnsiString MessageText)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Result = Spell.CanActivate__static(ReferenceFromId<TAbstractUnit>(Caster), ref MessageText);
            if (Result && Spell.FAboveGroundOnly)
            {
                ReferenceFromId<TAbstractUnit>(Caster).GetLocation(ref X, ref Y, ref Layer);
                if (Layer != 0)
                {
                    Result = false;
                    MessageText = TranslateRStr(CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:601
        public static bool GlobalTargetActivate(TGlobalTargetSpell Spell, int Caster, int A2)
        {
            bool Result = default;
            AnsiString MessageText = default;
            Spell.Activate__static(ReferenceFromId<TAbstractUnit>(Caster));
            if (Spell.CanActivate(ReferenceFromId<TAbstractUnit>(Caster), ref MessageText))
            {
                Spell.FCasterID = ReferenceFromId<TAbstractUnit>(Caster).FUnitID;
                Spell.FCasterPlayer = ReferenceFromId<TAbstractUnit>(Caster).FPlayer;
                Spell.SetupTargetSelection(ReferenceFromId<TAbstractUnit>(Caster), AoWHSMap.FTargetSelection);
                AoWHSMap.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        public static readonly AnsiString SpellXCastRStr = (AnsiString)"%s cast";

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
