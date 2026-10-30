// Translated from GlobalTargetSpells.pas.
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
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.SmokeCloud;
using static Aow.Units.SmokeCloud.SmokeCloud;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;

namespace Aow.Units.GlobalTargetSpells
{
    public enum TWatcherStateElement : byte
    {
        wsActive = 0
    }

    // { TGlobalTargetSpellTE (instance size 36, VMT 5579CBAC).
//     $20 FLocation: packed X/Y/layer coordinates; $23 reserved. }
    public partial class TGlobalTargetSpellTE : Aow.Units.AoWE.TSpellTE
    {
        public TGlobalTargetSpellTE(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Raw allocation for TFreezeWaterTE, TFireBarrierTE, TLevelTerrainTE.
        protected TGlobalTargetSpellTE() { }

        // GlobalTargetSpells.pas:401
        public new const int ClassID = 135976;

        public Aow.Units.HSEngine.TXYL FLocation;
        // GlobalTargetSpells.pas:406
        // { $20; packed coordinates, 557C493C }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FLocation.XYL, 20);
        }

        // GlobalTargetSpells.pas:412
        public void SetLocation(sbyte X, sbyte Y, sbyte Layer)
        {
            Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X = unchecked((sbyte)X);
            Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y = unchecked((sbyte)Y);
            Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer = unchecked((sbyte)Layer);
        }

        // GlobalTargetSpells.pas:419
        public void PlayDefaultEffects(sbyte X, sbyte Y, sbyte Layer)
        {
            Aow.Units.Animation.TImageSequenceAnimation Animation = default;
            // { needs: TSpellControl.GetSpell(ID: Integer): TSpell; original calls at
//     $5579E2CE, $5579E301, $5579E344 consume EAX as the returned spell.
//     TSpell.$28 is TSFXLibrary; TSpell.$2C is TImageSequenceList. }
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__2 = call__1.FCastImages.ImageSequences[0];
            if (call__2 != null)
            {
                Animation = new Aow.Units.Animation.TImageSequenceAnimation(null);
                // { needs: TImageSequenceAnimation.SetImageSequence(Value: TImageSequence). }
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__4 = call__3.FCastImages.ImageSequences[0];
                Animation.SetImageSequence(call__4);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, X, Y, Layer);
            }

            var call__5 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__5.FCastSounds.Play(0);
        }

        // NetworkE.pas:230


        public new partial class Meta : Aow.Units.AoWE.TSpellTE.Meta
        {
            // GlobalTargetSpells.pas:401
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)base.Create(AOwner);
        }
    }

    // { TGlobalTargetSpell (instance size 60, VMT 5579CC9C).
//     $34 FCasterID, $38 FCasterPlayer, $39 FAboveGroundOnly, $3A padding.
//     needs: CanActivate/Activate and the target predicates return Boolean.
//     Procedure declarations retained for shared TSpell and P7 overrides.
//     ShowTargetSelection variants: reordered locals, split/inline radius,
//     target with-scope, and inline map-field lookup. SetupTargetSelection
//     variants: explicit class local, repeated casts, class with-scope,
//     record fields, record with-scope and reversed coordinate local order. }
    public partial class TGlobalTargetSpell : Aow.Units.AoWE.TSpell
    {
        // Raw base for THolyGroundSpell, TUnholyGroundSpell, TRaiseTerrain and TPoisonPlantSpell.
        protected TGlobalTargetSpell() { }

        public int FCasterID;
        public sbyte FCasterPlayer;
        public bool FAboveGroundOnly;
        public Aow.Units.FixedArray_byte_2 F_GlobalTargetSpell_3A;
        // GlobalTargetSpells.pas:439
        private protected TGlobalTargetSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            InitializeConstruction();
        }

        // Legacy descendants outside this brief still enter this body through their initializer.
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            InitializeConstruction();
        }

        private void InitializeConstruction()
        {
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
        }

        // GlobalTargetSpells.pas:569
        public override bool CanActivate(Aow.Units.AoWE.TAbstractUnit A1, ref Aow.Rtl.AnsiString A2)
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
                    A2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:618
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString MessageText = default;
            base.Activate(A1);
            if (CanActivate(A1, ref MessageText))
            {
                FCasterID = A1.FUnitID;
                FCasterPlayer = unchecked((sbyte)A1.FPlayer);
                SetupTargetSelection(A1, Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection);
                Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:445
        public virtual void ShowTargetSelection(Aow.Units.AoWE.TTargetSelectionControl Target, int HN, int Xs, int Ys, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            int X = default;
            int Y = default;
            int Intensity = default;
            int Radius = default;
            var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Target.FTargetL];
            Level = call__6;
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Target.FTargetX, Target.FTargetY, HN, ref X, ref Y);
            // { Original accepts Width/Height themselves (strict > bounds test). }
            if ((((X >= 0) && (Y >= 0)) && (X <= Level.Width)) && (Y <= Level.Height))
            {
                var call__7 = Level.Field[X, Y];
                MapF = (Aow.Units.AoWE.TAoWMapField)call__7;
                if (this.ValidTargetSelectionMapF((Aow.Units.HSEngine.TMapField)((System.Object)MapF)))
                {
                    Radius = Aow.Units.HSEngine.HSEngine.HNtoRad(HN);
                    Intensity = (System.Math.Abs(((Aow.Units.AoWE.AoWE.AoWHSMap.Frame + Radius) % 9) - 4) * 20) + 20;
                    var call__8 = Target.FImageLibrary[0];
                    call__8.ShowBlended(Xs - 8, Ys, SurfaceDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Intensity]);
                }
                else
                {
                    var call__9 = Target.FImageLibrary[1];
                    call__9.Show(Xs - 8, Ys, SurfaceDesc);
                }
            }
        }

        // GlobalTargetSpells.pas:521
        // { +0A0 }
        public virtual void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            sbyte UnitX = default;
            sbyte UnitY = default;
            sbyte UnitLayer = default;
            A2.Initialize(A1);
            A2.FTargetMode = A2.FTargetMode + TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.Of(Aow.Units.AoWE.TTargetSelectionModeElement.tmOwnerDraw, Aow.Units.AoWE.TTargetSelectionModeElement.tmValidateDistance, Aow.Units.AoWE.TTargetSelectionModeElement.tmExternalFloater);
            A1.GetLocation(ref UnitX, ref UnitY, ref UnitLayer);
            A2.FSourceX = UnitX;
            A2.FSourceY = UnitY;
            A2.FSourceL = UnitLayer;
            A2.FRange = 12;
            A2.FOnChange.Add(OnTargetSelectionChange);
            A2.FOnShow = ShowTargetSelection;
            var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectTargetLocationForXOnMapRStr);
            var call__12 = Aow.Rtl.Rtl.Format(call__11, [FSpellName]);
            A2.FText.Text = call__12;
            A2.FTitle = FSpellName;
            var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
            A2.FCancelText = call__13;
            var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__15 = call__14.FCastImages.ImageSequences[20];
            var call__16 = call__15.GetPreviewImage();
            A2.FSourceImage = call__16;
            var call__17 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__18 = call__17.FCastImages.ImageSequences[21];
            var call__19 = call__18.GetPreviewImage();
            A2.FValidTargetImage = call__19;
            var call__20 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__21 = call__20.FCastImages.ImageSequences[22];
            var call__22 = call__21.GetPreviewImage();
            A2.FInvalidTargetImage = call__22;
        }

        // GlobalTargetSpells.pas:544
        // { +0A4 }
        public virtual bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4) => false;

        // GlobalTargetSpells.pas:487
        // { +0A8  }
        public virtual bool ValidTargetSelection(Aow.Units.AoWE.TTargetSelectionControl A1)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:475
        // { needs: Boolean result, retained in AL for the existing procedure ABI. }
        // { +0AC; 557A1808 }
        public virtual bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            bool bool__23 = Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled;
            if (bool__23)
            {
                var call__24 = ((Aow.Units.AoWE.TAoWMapField)A1).GetExplored(FCasterPlayer);
                bool__23 = !call__24;
            }

            if (bool__23)
            {
                // { needs: Boolean function declaration; preserve result for callers. }
                Result = false;
                return Result;
            }

            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:469
        // { +0B0  }
        public virtual bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1) => true;

        public static new Aow.Units.GlobalTargetSpells.TGlobalTargetSpell Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TGlobalTargetSpell(context, owner));
    }

    // { +0B4  }
    // { TFreezeWaterTE (instance size 36, VMT 5579CDE0) }
    public partial class TFreezeWaterTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        public new const int ClassID = 135939;

        // GlobalTargetSpells.pas:679
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            base.Process(A1, A2);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
            var call__25 = Aow.Units.AoWE.AoWE.AoWHSMap.TerrainVisible(unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
            if (call__25)
            {
                Twinkle = new Aow.Units.Animation.TTwinkleAnimation(null);
                var call__26 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(10);
                var call__27 = call__26.FCastImages.ImageSequences[0];
                Twinkle.Setup(call__27, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmExpand), 100, 64, 50, 30);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
            }

            // { needs: TAbstractAoWHSMap.$A4 contains the seated player at byte $A5. }
            bool bool__28 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
            if (!bool__28)
            {
                var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
                bool__28 = call__29;
            }

            if (bool__28)
            {
                var call__30 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(10);
                call__30.FCastSounds.Play(0);
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, TerrainChanged);
            var call__31 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__31.CastingDone(A1, FEventMana);
            // { needs: TSpellTE.Process returns a result in EAX (zero for this event). }
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:641
        public void ChangeTerrain(Aow.Units.HSEngine.TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            Aow.Units.AoWE.TFrozenWaterHS Ice = default;
            if (((Aow.Units.AoWE.TAoWMapField)MapF).GetArmyHS() == null)
            {
                if (unchecked((short)TerrainType) == ((int)Aow.Units.AoWE.TAoWTerrainType.ttWater))
                {
                    TerrainType = unchecked((sbyte)((int)Aow.Units.AoWE.TAoWTerrainType.ttIce));
                }
                else
                {
                    if (unchecked((short)TerrainType) == ((int)Aow.Units.AoWE.TAoWTerrainType.ttUndergroundWater))
                    {
                        TerrainType = unchecked((sbyte)((int)Aow.Units.AoWE.TAoWTerrainType.ttUndergroundIce));
                    }
                    else
                    {
                        if (TSet<byte>.WithBounds(0, 255).With((int)Aow.Units.AoWE.TAoWTerrainType.ttIce).With((int)Aow.Units.AoWE.TAoWTerrainType.ttUndergroundIce).Contains((byte)TerrainType))
                        {
                            Ice = (Aow.Units.AoWE.TFrozenWaterHS)MapF.FindOwnedHS(typeof(Aow.Units.AoWE.TFrozenWaterHS));
                            if (Ice != null)
                            {
                                var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                                Ice.FDaysRemaining = unchecked((sbyte)(((call__32 - 5) / 5) + 3));
                                Ice.FCasterPlayer = unchecked((sbyte)FEventPlayerIndex);
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:661
        public void TerrainChanged(Aow.Units.HSEngine.TMapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
            Aow.Units.AoWE.TFrozenWaterHS Ice = default;
            if (TSet<byte>.WithBounds(0, 255).With((int)Aow.Units.AoWE.TAoWTerrainType.ttIce).With((int)Aow.Units.AoWE.TAoWTerrainType.ttUndergroundIce).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With((int)Aow.Units.AoWE.TAoWTerrainType.ttIce).With((int)Aow.Units.AoWE.TAoWTerrainType.ttUndergroundIce).Contains((byte)OriginalTerrainType))
            {
                Ice = (Aow.Units.AoWE.TFrozenWaterHS)MapF.FindOwnedHS(typeof(Aow.Units.AoWE.TFrozenWaterHS));
                if (Ice == null)
                {
                    Ice = Aow.Units.AoWE.TFrozenWaterHS.Create(null);
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                }

                var call__33 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                Ice.FDaysRemaining = unchecked((sbyte)(((call__33 - 5) / 5) + 3));
                Ice.FCasterPlayer = unchecked((sbyte)FEventPlayerIndex);
            }
        }

        // NetworkE.pas:230
        public static new Aow.Units.GlobalTargetSpells.TFreezeWaterTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TFreezeWaterTE o = new Aow.Units.GlobalTargetSpells.TFreezeWaterTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // GlobalTargetSpells.pas:638
            // { needs: the following AoWE drafts return values at the original call sites.
//   The stack-argument shims discard their assembler EBP frame before tail calls. }
            // { needs: Animation.TTwinkleAnimation.Setup has TImageSequence/TTwinkleMode
//   arguments ($5579EA15-$5579EA1F); Animation is outside the interface exception. }
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TFreezeWaterTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TFreezeWaterTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TFreezeWaterTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TFreezeWaterTE)base.Create(AOwner);
        }
    }

    // { TFreezeWater (instance size 60, VMT 5579CEC0) }
    public partial class TFreezeWater : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:710
        // { needs: TArmyHS.VisibleForSeatedPlayer: Boolean; $5579EC2B tests AL. }
        private protected TFreezeWater(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FreezeWaterRStr);
            this.FSpellName = call__34;
            FSpellID = 10;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
        }

        // GlobalTargetSpells.pas:811
        // { Original Boolean-return ABI; foreign P7 overrides remain procedures. }
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            Aow.Units.AoWE.TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__35 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__35; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:823
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TFreezeWaterTE Event = default;
            Event = Aow.Units.GlobalTargetSpells.TFreezeWaterTE.Create(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__36.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        public static new Aow.Units.GlobalTargetSpells.TFreezeWater Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFreezeWater(context, owner));
    }

    // { TFireBarrierTE (instance size 36, VMT 5579CFFC) }
    public partial class TFireBarrierTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        public new const int ClassID = 135940;

        // GlobalTargetSpells.pas:873
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            CreateFireBarrier();
            var call__37 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__38 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__38.CastingDone(A1, call__37.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:842
        public void CreateFireBarrier()
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            Aow.Units.AoWE.TFire Fire = default;
            var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__39;
            var call__40 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer];
            Level = call__40;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__41 = Level.Field[Walk.X, Walk.Y];
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__41;
                    var call__42 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(11);
                    var call__43 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpell)((System.Object)call__42)).ValidTargetMapF((Aow.Units.HSEngine.TMapField)((System.Object)MapF));
                    bool bool__44 = call__43;
                    if (bool__44)
                    {
                        var call__45 = MapF.GetArmyHS();
                        bool__44 = (call__45 == null);
                    }

                    if (bool__44)
                    {
                        var call__46 = Aow.Units.AoWE.AoWE.AoWHSSet.FFireControl.PlaceFire(Walk.X, Walk.Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, Aow.Units.AoWE.TFireType.ftLarge);
                        Fire = call__46;
                        if (Fire != null)
                        {
                            var call__47 = Aow.Rtl.Rtl.Random(35);
                            Fire.SetupExplosion(call__47);
                            Fire.FTurnsLeft = 3;
                            var call__48 = this.GetSource();
                            Fire.FPlayer = unchecked((sbyte)call__48.FPlayer);
                        }
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230
        public static new Aow.Units.GlobalTargetSpells.TFireBarrierTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TFireBarrierTE o = new Aow.Units.GlobalTargetSpells.TFireBarrierTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // GlobalTargetSpells.pas:839
            // { needs: TFireControl.PlaceFire(X,Y,Layer:Integer; Create:Boolean): TFire,
//   result consumed at $5579EDC3; GetSource returns TAbstractUnit at $5579EDE2. }
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TFireBarrierTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TFireBarrierTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TFireBarrierTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TFireBarrierTE)base.Create(AOwner);
        }
    }

    // { draft }
    // { TFireBarrier (instance size 60, VMT 5579D0DC) }
    public partial class TFireBarrier : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:882
        private protected TFireBarrier(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireBarrierRStr);
            this.FSpellName = call__49;
            FSpellID = 11;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
        }

        // GlobalTargetSpells.pas:914
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            Aow.Units.AoWE.TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__50 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__50; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:926
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TFireBarrierTE Event = default;
            Event = Aow.Units.GlobalTargetSpells.TFireBarrierTE.Create(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__51 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__51.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:900
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__52 = base.ValidTargetSelectionMapF(A1);
            bool bool__53 = call__52 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__53)
            {
                var call__54 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__53 = (call__54 == null);
            }

            Result = bool__53;
            if (Result)
            {
                Army = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:892
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__55 = base.ValidTargetMapF(A1);
            bool bool__56 = call__55 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__56)
            {
                var call__57 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__56 = (call__57 == null);
            }

            Result = bool__56;
            if (Result)
            {
                Result = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS() == null;
            }

            return Result;
        }

        public static new Aow.Units.GlobalTargetSpells.TFireBarrier Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFireBarrier(context, owner));
    }

    // { TLevelTerrainTE (instance size 36, VMT 5579D218) }
    public partial class TLevelTerrainTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:940
        public new const int ClassID = 135945;

        // GlobalTargetSpells.pas:968
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            PlayDefaultEffects(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer));
            LevelTerrain();
            var call__58 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__59 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__59.CastingDone(A1, call__58.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:943
        public void LevelTerrain()
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            var call__60 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer];
            Level = call__60;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__61 = Level.Field[Walk.X, Walk.Y];
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__61;
                    var call__62 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(18);
                    var call__63 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpell)((System.Object)call__62)).ValidTargetMapF((Aow.Units.HSEngine.TMapField)((System.Object)MapF));
                    bool bool__64 = call__63;
                    if (bool__64)
                    {
                        var call__65 = MapF.GetArmyHS();
                        bool__64 = (call__65 == null);
                    }

                    if (bool__64)
                    {
                        Aow.Units.AoWE.AoWE.AoWHSMap.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, 0, -1);
                        if (unchecked((short)MapF.TerrainType) == ((int)Aow.Units.AoWE.TAoWTerrainType.ttEarth))
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrain(null, Walk.X, Walk.Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, 0, (int)Aow.Units.AoWE.TAoWTerrainType.ttDirt);
                        }
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230
        public static new Aow.Units.GlobalTargetSpells.TLevelTerrainTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TLevelTerrainTE o = new Aow.Units.GlobalTargetSpells.TLevelTerrainTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // GlobalTargetSpells.pas:940
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TLevelTerrainTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TLevelTerrainTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TLevelTerrainTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TLevelTerrainTE)base.Create(AOwner);
        }
    }

    // { draft }
    // { TLevelTerrain (instance size 60, VMT 5579D2F8) }
    public partial class TLevelTerrain : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:978
        private protected TLevelTerrain(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelTerrainRStr);
            this.FSpellName = call__66;
            FSpellID = 18;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
        }

        // GlobalTargetSpells.pas:1010
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            Aow.Units.AoWE.TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__67 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__67; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:1022
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TLevelTerrainTE Event = default;
            Event = Aow.Units.GlobalTargetSpells.TLevelTerrainTE.Create(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__68 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__68.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:996
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__69 = base.ValidTargetSelectionMapF(A1);
            bool bool__70 = call__69 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__70)
            {
                var call__71 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__70 = (call__71 == null);
            }

            Result = bool__70;
            if (Result)
            {
                Army = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:988
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__72 = base.ValidTargetMapF(A1);
            bool bool__73 = call__72 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__73)
            {
                var call__74 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__73 = (call__74 == null);
            }

            Result = bool__73;
            if (Result)
            {
                Result = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS() == null;
            }

            return Result;
        }

        public static new Aow.Units.GlobalTargetSpells.TLevelTerrain Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TLevelTerrain(context, owner));
    }

    // { TRejuvenateTE (instance size 36, VMT 5579D434) }
    public partial class TRejuvenateTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1036
        public new const int ClassID = 135952;

        // GlobalTargetSpells.pas:1044
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            base.Process(A1, A2);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
            var call__75 = Aow.Units.AoWE.AoWE.AoWHSMap.TerrainVisible(unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
            if (call__75)
            {
                Twinkle = new Aow.Units.Animation.TTwinkleAnimation(null);
                var call__76 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(41);
                var call__77 = call__76.FCastImages.ImageSequences[0];
                Twinkle.Setup(call__77, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 100, 64, 50, 30);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
            }

            // { needs: TAbstractAoWHSMap.$A4 contains the seated player at byte $A5. }
            bool bool__78 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
            if (!bool__78)
            {
                var call__79 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
                bool__78 = call__79;
            }

            if (bool__78)
            {
                var call__80 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(41);
                call__80.FCastSounds.Play(0);
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, 2, RejuvenateTerrain, null);
            var call__81 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__82 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__82.CastingDone(A1, call__81.FBaseManaCost);
            // { needs: TSpellTE.Process returns a result in EAX (zero for this event). }
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:1039
        public void RejuvenateTerrain(Aow.Units.HSEngine.TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (TSet<byte>.WithBounds(0, 255).WithRange((int)Aow.Units.AoWE.TAoWTerrainType.ttDesert, (int)Aow.Units.AoWE.TAoWTerrainType.ttWasteland).Contains((byte)TerrainType))
            {
                TerrainType = unchecked((sbyte)((int)Aow.Units.AoWE.TAoWTerrainType.ttGrass));
            }
        }

        // NetworkE.pas:230
        public static new Aow.Units.GlobalTargetSpells.TRejuvenateTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TRejuvenateTE o = new Aow.Units.GlobalTargetSpells.TRejuvenateTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // GlobalTargetSpells.pas:1036
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TRejuvenateTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TRejuvenateTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TRejuvenateTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TRejuvenateTE)base.Create(AOwner);
        }
    }

    // { TRejuvenate (instance size 60, VMT 5579D514) }
    public partial class TRejuvenate : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1074
        private protected TRejuvenate(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__83 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RejuvenateRStr);
            this.FSpellName = call__83;
            FSpellID = 41;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msLife;
            FAboveGroundOnly = true;
        }

        // GlobalTargetSpells.pas:1095
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            Aow.Units.AoWE.TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__84 = Aow.Units.HSEngine.HSEngine.RadToHN(3) - 1;
                for (; I <= end__84; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:1107
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TRejuvenateTE Event = default;
            Event = Aow.Units.GlobalTargetSpells.TRejuvenateTE.Create(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__85 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__85.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:1090
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__86 = base.ValidTargetSelectionMapF(A1);
            Result = call__86 && TSet<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)A1.TerrainType);
            return Result;
        }

        // GlobalTargetSpells.pas:1085
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__87 = base.ValidTargetMapF(A1);
            Result = call__87 && TSet<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)A1.TerrainType);
            return Result;
        }

        public static new Aow.Units.GlobalTargetSpells.TRejuvenate Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TRejuvenate(context, owner));
    }

    // { TWatcherEnchantment (instance size 40, VMT 5579D648): $24 FWatcher. }
    public partial class TWatcherEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        public Aow.Units.GlobalTargetSpells.TWatcherHS FWatcher;
        // GlobalTargetSpells.pas:1121
        public TWatcherEnchantment(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FEnchantmentMode = TSet<Aow.Units.AoWE.TGlobalEnchantmentModeElement>.FromMask((ulong)FEnchantmentMode.Mask) - TSet<Aow.Units.AoWE.TGlobalEnchantmentModeElement>.Of(Aow.Units.AoWE.TGlobalEnchantmentModeElement.gemTrueGlobal, Aow.Units.AoWE.TGlobalEnchantmentModeElement.gemOwned);
            FDispelMana = 50;
        }

        // GlobalTargetSpells.pas:1129
        public override Aow.Rtl.AnsiString GetName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__88 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
            Result = call__88.FSpellName;
            return Result;
        }

    }

    // { TWatcherHS (instance size 32, VMT 5579D7A0): $0C state, $0D unfog
//     radius, $10 enchantment, $14 animation frame, $18 unexplored HNs,
//     $1C visibility flag; $0E/$1D padding. }
    public partial class TWatcherHS : Aow.Units.HSEngine.TSingleHS
    {
        // GlobalTargetSpells.pas:1220
        public new const int ClassID = 132989;

        public byte FWatcherState;
        public byte FUnfogRadius;
        public Aow.Units.FixedArray_byte_2 FReserved0E;
        public Aow.Units.GlobalTargetSpells.TWatcherEnchantment FEnchantment;
        public int FAnimationFrame;
        public Aow.Units.Engine.TIntegerList FUnexplored;
        public bool FVisibleForSeatedPlayer;
        public Aow.Units.FixedArray_byte_3 FReserved1D;
        // GlobalTargetSpells.pas:1213
        public override void Destroy()
        {
            base.Destroy();
            FUnexplored.Free();
            FEnchantment.Release();
        }

        // GlobalTargetSpells.pas:1344
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FEnchantment, 40);
        }

        // GlobalTargetSpells.pas:1203
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__89 = new Aow.Units.GlobalTargetSpells.TWatcherEnchantment(null);
            FEnchantment = call__89;
            FEnchantment.FWatcher = this;
            var call__90 = new Aow.Units.Engine.TIntegerList();
            FUnexplored = call__90;
            FVisibleForSeatedPlayer = false;
            FAnimationFrame = -1;
        }

        // GlobalTargetSpells.pas:1422
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
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
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
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
            bool bool__91 = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == Lhx);
            if (bool__91)
            {
                var call__92 = Yhx;
                var call__93 = Xhx;
                var call__94 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)call__93), unchecked((sbyte)call__92), unchecked((sbyte)Lhx), 0);
                bool__91 = call__94;
            }

            if (bool__91)
            {
                var call__95 = Yhx;
                var call__96 = Xhx;
                Aow.Units.HSEngine.HSEngine.HXtoHP(call__96, call__95, ref X, ref Y);
                X = (X - XhpSurf) + 16;
                Y = (Y - YhpSurf) + 16;
                var call__97 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
                if (call__97.FCastImages != null)
                {
                    var call__98 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
                    call__98.FCastImages.ShowLooped(1, X, Y, Aow.Units.AoWE.AoWE.AoWHSMap.Frame, SurfaceDesc);
                }

                if (FAnimationFrame != -1)
                {
                    FAnimationFrame += 1;
                    // { Original reads the shared AoWEngine animation detail at byte $6D.
//         The map engine is not the global read by this routine. }
                    if ((Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 2) && (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == Lhx))
                    {
                        Intensity = 100 - FAnimationFrame;
                        var call__99 = Yhx;
                        var call__100 = Xhx;
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__100, call__99, ref X, ref Y);
                        X = (X - XhpSurf) - 8;
                        Y = Y - YhpSurf;
                        {
                            I = FUnexplored.Count - 1;
                            byte end__101 = 0;
                            for (; I >= end__101; I -= 1)
                            {
                                var call__102 = FUnexplored[I];
                                Aow.Units.HSEngine.HSEngine.HNtoHP(call__102, ref PX, ref PY);
                                PX += X;
                                PY += Y;
                                var call__103 = Aow.Units.AoWE.AoWE.AoWHSSet.FGeneralImages[5];
                                call__103.ShowBlended(PX, PY, SurfaceDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[Intensity]);
                            }
                        }
                    }

                    // { Original resets the shared random generator for deterministic particles. }
                    Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FInitialRandomSeed;
                    {
                        I = 0;
                        short end__104 = 300;
                        for (; I <= end__104; I += 1)
                        {
                            var call__105 = VisibilityRange();
                            var call__106 = Aow.Rtl.Rtl.Random(call__105 << 5);
                            Radius = call__106;
                            Angle = Aow.Rtl.Rtl.Random(360);
                            Delay = Aow.Rtl.Rtl.Random(80);
                            if ((FAnimationFrame - Delay) >= 0)
                            {
                                Aow.Units.AoWE.AoWE.DegreesRadToCo(Angle, Radius, ref DX, ref DY);
                                var call__107 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
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
            ((Aow.Units.HSEngine.TMapField)Owner).UpdateMapField();
            return Result;
        }

        // GlobalTargetSpells.pas:1394
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapField MapF = default;
            MapF = (Aow.Units.HSEngine.TMapField)Owner;
            base.RemoveFromMap();
            MapF.UpdateMapField();
        }

        // GlobalTargetSpells.pas:1438
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
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
                    bool bool__109 = (OwnerPlayer == mapFieldMsg.Param[1].Value);
                    if (!bool__109)
                    {
                        var call__110 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer << 0);
                        var call__111 = unchecked((byte)call__110.FDiplomaticRelations.GetDiplomaticRelation(unchecked((sbyte)unchecked((byte)mapFieldMsg.Param[1].Value))));
                        bool__109 = (call__111 == 2);
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
                    bool bool__113 = (OwnerPlayer == mapFieldMsg.Param[1].Value);
                    if (!bool__113)
                    {
                        var call__114 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer << 0);
                        var call__115 = unchecked((byte)call__114.FDiplomaticRelations.GetDiplomaticRelation(unchecked((sbyte)unchecked((byte)mapFieldMsg.Param[1].Value))));
                        bool__113 = (call__115 == 2);
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
                    Aow.Units.AoWE.AoWE.AoWHSMap.UpdatePlayerSeen(FEnchantment.FPlayerIndex, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx, mapFieldMsg.Param[0].Value, mapFieldMsg.Param[0].Value);
                }

                UpdateVisibleForSeatedPlayer();
            }
        }

        // GlobalTargetSpells.pas:1350
        public override void Activate()
        {
            Aow.Units.HSEngine.TMapField MapF = default;
            base.Activate();
            FWatcherState = (byte)(TSet<Aow.Units.GlobalTargetSpells.TWatcherStateElement>.FromMask((ulong)FWatcherState) + TSet<Aow.Units.GlobalTargetSpells.TWatcherStateElement>.Of(Aow.Units.GlobalTargetSpells.TWatcherStateElement.wsActive)).Image;
            FEnchantment.Activate(unchecked((sbyte)FEnchantment.FPlayerIndex));
            var mapField = (Aow.Units.HSEngine.TMapField)Owner;
            Aow.Units.AoWE.AoWE.AoWHSMap.NotifyPlayerChanged(mapField.Xhx, mapField.Yhx, mapField.Lhx);

            MapF = (Aow.Units.HSEngine.TMapField)Owner;
            var call__119 = VisibilityRange();
            var call__120 = VisibilityRange();
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdatePlayerSeen(FEnchantment.FPlayerIndex, MapF.Xhx, MapF.Yhx, MapF.Lhx, call__119, call__120);
            UpdateExploration();
            UpdateUnfog();
        }

        // GlobalTargetSpells.pas:1366
        public override void Deactivate()
        {
            base.Deactivate();
            FWatcherState = (byte)(TSet<Aow.Units.GlobalTargetSpells.TWatcherStateElement>.FromMask((ulong)FWatcherState) - TSet<Aow.Units.GlobalTargetSpells.TWatcherStateElement>.Of(Aow.Units.GlobalTargetSpells.TWatcherStateElement.wsActive)).Image;
            if (FEnchantment.FEnchantmentStatus.Contains(Aow.Units.AoWE.TGlobalEnchantmentStatusElement.getActivated))
            {
                FEnchantment.Deactivate(unchecked((byte)FEnchantment.FPlayerIndex));
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.NotifyPlayerChanged(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx);
            UpdateUnfog();
        }

        // GlobalTargetSpells.pas:1403
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags = (((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags | 4096) | 16777216;
            }
        }

        // GlobalTargetSpells.pas:1226
        public void StartAnimation(sbyte X, sbyte Y, sbyte Layer)
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            FAnimationFrame = 0;
            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicFixedVisibleHS.AddChild(this);
            var call__121 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
            call__121.FCastSounds.Play(0);
            FUnexplored.Clear();
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled)
            {
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 0);
                var call__122 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Layer];
                Level = call__122;
                while (true)
                {
                    var call__123 = VisibilityRange();
                    if (!(Walk.Radius <= call__123))
                        break;
                    {
                        if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                        {
                            var call__124 = Level.Field[Walk.X, Walk.Y];
                            var call__125 = ((Aow.Units.AoWE.TAoWMapField)call__124).GetExplored(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            if (!call__125)
                            {
                                FUnexplored.Add(Walk.HN);
                            }
                        }

                        Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:1248
        public void StopAnimation()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicFixedVisibleHS.RemoveChild(this);
            FAnimationFrame = -1;
            FUnexplored.Clear();
        }

        // GlobalTargetSpells.pas:1305
        // { draft }
        public void UpdateExploration()
        {
            int I = default;
            int OwnerPlayer = default;
            if (Activated())
            {
                {
                    I = 1;
                    int end__126 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                    for (; I <= end__126; I += 1)
                    {
                        OwnerPlayer = FEnchantment.FPlayerIndex;
                        bool bool__127 = (OwnerPlayer == I);
                        if (!bool__127)
                        {
                            var call__128 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer);
                            var call__129 = unchecked((byte)call__128.FDiplomaticRelations.GetDiplomaticRelation(unchecked((sbyte)unchecked((byte)I))));
                            bool__127 = (call__129 == 2);
                        }

                        if (bool__127)
                        {
                            var call__130 = VisibilityRange();
                            var call__131 = Yhx;
                            var call__132 = Xhx;
                            Aow.Units.AoWE.AoWE.AoWHSMap.ExploreArea(call__132, call__131, Lhx, unchecked((sbyte)I), call__130);
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:1321
        // { draft }
        public void UpdateUnfog()
        {
            int Radius = default;
            sbyte OwnerPlayer = default;
            bool bool__133 = (unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) != 255);
            if (bool__133)
            {
                bool bool__134 = (FEnchantment.FPlayerIndex == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__134)
                {
                    var call__135 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FEnchantment.FPlayerIndex);
                    var call__136 = unchecked((byte)call__135.FDiplomaticRelations.GetDiplomaticRelation(unchecked((sbyte)unchecked((byte)unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)))));
                    bool__134 = (call__136 == 2);
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
                    Aow.Units.AoWE.AoWE.AoWHSMap.FogArea(call__140, call__139, Lhx, FUnfogRadius);
                }

                FUnfogRadius = unchecked((byte)Radius);
                if (FUnfogRadius != 0)
                {
                    var call__141 = Yhx;
                    var call__142 = Xhx;
                    Aow.Units.AoWE.AoWE.AoWHSMap.UnFogArea(call__142, call__141, Lhx, FUnfogRadius);
                }
            }
        }

        // GlobalTargetSpells.pas:1376
        // { draft }
        public int VisibilityRange()
        {
            int Result = default;
            Result = 8;
            bool bool__143 = (Lhx != 0);
            if (!bool__143)
            {
                bool bool__144 = (FEnchantment.FPlayerIndex > 0);
                if (bool__144)
                {
                    var call__145 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FEnchantment.FPlayerIndex);
                    bool__144 = call__145.FMagicControl.FEnemyEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petWaterMastery);
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
            var call__146 = FEnchantment.VisibleForPlayer(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Visible = call__146;
            if (FVisibleForSeatedPlayer ^ Visible)
            {
                FVisibleForSeatedPlayer = Visible;
                Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
            }
        }

        // GlobalTargetSpells.pas:172
        public static new Aow.Units.GlobalTargetSpells.TWatcherHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TWatcherHS o = new Aow.Units.GlobalTargetSpells.TWatcherHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // GlobalTargetSpells.pas:1220
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TWatcherHS.ClassID;

            // GlobalTargetSpells.pas:172
            public new Aow.Units.GlobalTargetSpells.TWatcherHS Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TWatcherHS)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TWatcherHS => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { draft }
    // { TWatcherTE (instance size 36, VMT 5579D92C) }
    public partial class TWatcherTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1476
        public new const int ClassID = 135963;

        // GlobalTargetSpells.pas:1479
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TWatcherHS Watcher = default;
            base.Process(A1, A2);
            PlayDefaultEffects(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer));
            Watcher = Aow.Units.GlobalTargetSpells.TWatcherHS.Create(null);
            Watcher.FEnchantment.FPlayerIndex = unchecked((sbyte)A1.FPlayer);
            var call__147 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
            Watcher.FEnchantment.FEnchantmentManaUpkeep = call__147.FManaUpkeep;
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Watcher, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
            if (unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) == FEventPlayerIndex)
            {
                Watcher.StartAnimation(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer));
            }

            var call__148 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__149 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__149.CastingDone(A1, call__148.FBaseManaCost);
            Result = false;
            return Result;
        }

        // NetworkE.pas:230
        public static new Aow.Units.GlobalTargetSpells.TWatcherTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TWatcherTE o = new Aow.Units.GlobalTargetSpells.TWatcherTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // GlobalTargetSpells.pas:1476
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TWatcherTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TWatcherTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TWatcherTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TWatcherTE)base.Create(AOwner);
        }
    }

    // { TWatcher (instance size 60, VMT 5579DA04) }
    public partial class TWatcher : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1498
        // { needs: TPlayer.GetBusy: Boolean, tested at $557A0266. }
        private protected TWatcher(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__150 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WatcherRStr);
            this.FSpellName = call__150;
            FSpellID = 57;
            FAboveGroundOnly = false;
        }

        // GlobalTargetSpells.pas:1523
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            base.SetupTargetSelection(A1, A2);
            A2.FTargetMode = TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.FromMask((ulong)A2.FTargetMode.Mask) - TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.Of(Aow.Units.AoWE.TTargetSelectionModeElement.tmFixedLevel, Aow.Units.AoWE.TTargetSelectionModeElement.tmValidateDistance);
            A2.FHNs.Clear();
        }

        // GlobalTargetSpells.pas:1506
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TWatcherTE Event = default;
            var call__151 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__152 = call__151.GetBusy();
            if (!call__152)
            {
                Event = Aow.Units.GlobalTargetSpells.TWatcherTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
                var call__153 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__153.AddEvent(Event);
                // { Original queues through AddEvent, VMT +$10. }
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        public static new Aow.Units.GlobalTargetSpells.TWatcher Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TWatcher(context, owner));
    }

    // { TTornadoTE (instance size 48, VMT 5579DB38): $24 angle,
//     $28 animation, $2C selected army identity. }
    public partial class TTornadoTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1559
        public new const int ClassID = 135972;

        public int FAngle;
        public Aow.Units.Animation.TAnimation FAnimation;
        public Aow.Units.AoWE.TArmyID FArmyID;
        // GlobalTargetSpells.pas:1553
        public override void Destroy()
        {
            FArmyID.Free();
            base.Destroy();
        }

        // GlobalTargetSpells.pas:1593
        // { needs: Boolean result on the shared TSpellTE.Validate virtual. }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FArmyID, null, 40);
        }

        // GlobalTargetSpells.pas:1547
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FArmyID = null;
        }

        // GlobalTargetSpells.pas:1660
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.AoWE.TUnitUserList MovedUnits = default;
            byte Owner = default;
            byte Selection = default;
            Aow.Units.HSEngine.TXYL Location = default;
            int Chance = default;
            bool Exposed = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.AoWE.TAbstractUnit UnitObj = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            int I = default;
            Aow.Units.AoWE.TArmyEventLog Log = default;
            Aow.Units.Animation.TImageSequenceAnimation Anim = default;
            Aow.Units.AoWE.TArmyHS Placed = default;
            Aow.Units.AoWE.TAIGroup Group = default;
            if (A2 == 0)
            {
                FAngle = 0;
                var call__154 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__155 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__155.CastingDone(A1, call__154.FBaseManaCost);
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                bool bool__156 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__156)
                {
                    var call__157 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                    bool__156 = call__157;
                }

                if (bool__156)
                {
                    var call__158 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                    call__158.FCastSounds.Play(0);
                }

                Anim = (Aow.Units.Animation.TImageSequenceAnimation)new Aow.Units.Animation.TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                Anim.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
            }

            if (A2 == 80)
            {
                var call__159 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
                MapF = (Aow.Units.AoWE.TAoWMapField)call__159;
                bool bool__160 = (FArmyID != null);
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
                        var call__163 = MapF.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                        bool__162 = (call__163 != null);
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

                                var call__166 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                                if (Chance < call__166)
                                {
                                    Selection = unchecked((byte)(Selection & ~(1 << I)));
                                }
                            }
                        }
                    }

                    var call__167 = Army.FArmy.Transporter(255);
                    if (call__167 != null)
                    {
                        var call__168 = Army.FArmy.Transporter(unchecked((byte)Selection));
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
                        Log = Aow.Units.AoWE.TArmyEventLog.Create(null);
                        Log.FPlayer = unchecked((sbyte)A1.FPlayer);
                        var call__169 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TornadoRStr);
                        var call__170 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XCastRStr);
                        var call__171 = Aow.Rtl.Rtl.Format(call__170, [call__169]);
                        Log.FArmyText = call__171;
                        var call__172 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TornadoRStr);
                        Log.FArmyMessages.Add(call__172);
                        Log.FArmyIcon = 2;
                        Log.FArmyView.Setup(Army.FArmy, unchecked((byte)Selection));
                        var call__173 = Army.Yhx;
                        var call__174 = Army.Xhx;
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FEventPlayerIndex), unchecked((sbyte)call__174), unchecked((sbyte)call__173), unchecked((sbyte)Army.Lhx), true);
                        Log.Release();
                        var call__175 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(68);
                        var call__176 = ((Aow.Units.GlobalTargetSpells.TTornado)call__175).CalculateWarpLocation(Army, unchecked((byte)Selection));
                        Location.XYL = call__176;
                        if (Location.XYL != 16777215)
                        {
                            bool bool__177 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            if (!bool__177)
                            {
                                var call__178 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                                bool__177 = call__178;
                            }

                            if (bool__177)
                            {
                                var call__179 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                call__179.FCastSounds.Play(1);
                            }

                            bool bool__180 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
                            if (!bool__180)
                            {
                                var call__181 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                bool__180 = call__181;
                            }

                            if (bool__180)
                            {
                                var call__182 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                var call__183 = call__182.FCastImages.ImageSequences[1];
                                if (call__183 != null)
                                {
                                    Anim = new Aow.Units.Animation.TImageSequenceAnimation(null);
                                    var call__184 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                    var call__185 = call__184.FCastImages.ImageSequences[1];
                                    Anim.SetImageSequence(call__185);
                                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
                                }
                            }

                            bool bool__186 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
                            if (!bool__186)
                            {
                                var call__187 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(Location.X, Location.Y, Location.L, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                bool__186 = call__187;
                            }

                            if (bool__186)
                            {
                                var call__188 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                var call__189 = call__188.FCastImages.ImageSequences[1];
                                if (call__189 != null)
                                {
                                    Anim = new Aow.Units.Animation.TImageSequenceAnimation(null);
                                    var call__190 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                    var call__191 = call__190.FCastImages.ImageSequences[1];
                                    Anim.SetImageSequence(call__191);
                                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, Location.X, Location.Y, Location.L);
                                }
                            }

                            MovedUnits = new Aow.Units.AoWE.TUnitUserList(null);
                            Owner = unchecked((byte)Army.FArmy.FPlayer);
                            var call__192 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(Location.XYL);
                            MapF = (Aow.Units.AoWE.TAoWMapField)call__192;
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
                                            var call__196 = UnitObj.Place(Location.X, Location.Y, Location.L, unchecked((sbyte)unchecked((byte)Army.FArmy.FPlayer)), TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of());
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
                                var call__197 = Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(MovedUnits, 5);
                                var call__198 = Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.CreateGroup(0, unchecked((sbyte)call__197), Aow.Units.AoWE.TGroupBehavior.gbHunt);
                                Group = call__198;
                                Group.AddUnits(MovedUnits);
                                ((Aow.Units.AoWE.THuntAG)Group).FTargetUnitIDs.Add(A1.FUnitID);
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
        public void ShowAnimation(Aow.Units.Animation.TAnimation Animation, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
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
            if (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)
            {
                Aow.Units.HSEngine.HSEngine.HXtoHP(Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                FAngle = (FAngle + 20) + (FAnimation.FFrameCounter / 5);
                Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    byte end__199 = 200;
                    for (; I <= end__199; I += 1)
                    {
                        Direction = Aow.Rtl.Rtl.Random(360);
                        if ((Animation.FFrameCounter >= ((int)(((uint)I) >> 2))) && (Animation.FFrameCounter <= (((int)(((uint)I) >> 4)) + 68)))
                        {
                            Radius = Aow.Rtl.Rtl.Round(((double)(((double)2.997) - System.Math.Log(((double)(((double)(I * 19)) * 0.005)) + 1))) * 35) + 7;
                            Height = (200 - I) - 16;
                            var call__200 = System.Math.Sin(((double)(((double)FAnimation.FFrameCounter) * 0.1)) + (((double)I) * 0.02));
                            XShift = Aow.Rtl.Rtl.Round(((double)(((double)call__200) * (200 - I))) * 0.25) + 16;
                            var call__201 = System.Math.PI;
                            Angle = (float)(((double)(((double)(Direction + FAngle)) * call__201)) / 180);
                            if (((int)Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail) == 0)
                            {
                                Last = 1;
                            }
                            else
                            {
                                if (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 1)
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
                                    X = Aow.Rtl.Rtl.Round(((double)System.Math.Cos(Angle)) * Radius) + XShift;
                                    Y = Aow.Rtl.Rtl.Round(((double)(((double)System.Math.Sin(Angle)) * Radius)) * 0.5) + Height;
                                    var call__203 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(68);
                                    call__203.FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                                    Angle = (float)(((double)0.035) + Angle);
                                }
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:215
        public static new Aow.Units.GlobalTargetSpells.TTornadoTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TTornadoTE o = new Aow.Units.GlobalTargetSpells.TTornadoTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // GlobalTargetSpells.pas:1559
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TTornadoTE.ClassID;

            // GlobalTargetSpells.pas:215
            public new Aow.Units.GlobalTargetSpells.TTornadoTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TTornadoTE)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TTornadoTE => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TTornado (instance size 60, VMT 5579DC10) }
    public partial class TTornado : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1798
        private protected TTornado(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__204 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TornadoRStr);
            this.FSpellName = call__204;
            FSpellID = 68;
            FAboveGroundOnly = true;
        }

        // GlobalTargetSpells.pas:1885
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            Aow.Units.AoWE.TTargetSelectionControl Target = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
        }

        // GlobalTargetSpells.pas:1865
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TTornadoTE Event = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__205 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__206 = call__205.GetBusy();
            if (!call__206)
            {
                Event = Aow.Units.GlobalTargetSpells.TTornadoTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
                var call__207 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(A2, A3, A4);
                var call__208 = ((Aow.Units.AoWE.TAoWMapField)call__207).GetArmyHS();
                Army = call__208;
                if (Army != null)
                {
                    var call__209 = Army.FArmy.CreateID(255);
                    Event.FArmyID = call__209;
                }

                var call__210 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__210.AddEvent(Event);
                // { Original queues through AddEvent, VMT +$10. }
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
        public bool ValidWarpLocation(Aow.Units.AoWE.TArmyHS Army, sbyte X, sbyte Y, sbyte Layer)
        {
            bool Result = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            var call__211 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, Layer);
            MapF = (Aow.Units.AoWE.TAoWMapField)call__211;
            var call__212 = MapF.GetArmyHS();
            bool bool__213 = (call__212 == null);
            if (bool__213)
            {
                var call__214 = MapF.GetPlayer();
                bool__213 = (call__214 == -1);
            }

            Result = bool__213;
            if (Result)
            {
                Msg.MsgID = 4384;
                Msg.Param[0] = Army;
                Msg.Result = 0;
                MapF.SendMapFieldMsg(ref Msg);
                bool bool__215 = (Msg.Result.Value == 0);
                if (bool__215)
                {
                    var call__216 = Army.FArmy.CanPlace(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer));
                    bool__215 = call__216;
                }

                Result = bool__215;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:1823
        public int CalculateWarpLocation(Aow.Units.AoWE.TArmyHS Army, byte Selection)
        {
            int Result = default;
            Aow.Units.GlobalTargetSpells.TSpellMoveTable Table = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.HSEngine.TMapField MapF = default;
            Aow.Rtl.TList Locations = default;
            bool Found = default;
            int Index = default;
            int X = default;
            Locations = new Aow.Rtl.TList();
            Army.FArmy.CreateMovePointTable(unchecked((byte)Selection), Aow.Rtl.Bytes.Of(ref Table));
            var call__217 = Army.Yhx;
            var call__218 = Army.Xhx;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__218, call__217, 10);
            var call__219 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Army.Lhx];
            MapLevel = call__219;
            while (Walk.Radius < 15)
            {
                X = Walk.X;
                if ((((X >= 0) && (Walk.Y >= 0)) && (X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                {
                    var call__220 = MapLevel.GetField(X, Walk.Y);
                    MapF = call__220;
                    if (Table.Cost[MapF.TerrainType][MapF.OverlayType] != -1)
                    {
                        Locations.Add(MapF);
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            Found = false;
            Result = 16777215;
            while ((Locations.Count != 0) && !Found)
            {
                var call__221 = Aow.Rtl.Rtl.Random(Locations.Count);
                Index = call__221;
                var call__222 = Locations[Index];
                MapF = (Aow.Units.HSEngine.TMapField)call__222;
                var call__223 = ValidWarpLocation(Army, unchecked((sbyte)MapF.Xhx), unchecked((sbyte)MapF.Yhx), unchecked((sbyte)MapF.Lhx));
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

        public static new Aow.Units.GlobalTargetSpells.TTornado Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TTornado(context, owner));
    }

    // { TVortexTE (instance size 48, VMT 5579DD44): $24 angle,
//     $28 animation, $2C selected army identity. }
    public partial class TVortexTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1905
        public new const int ClassID = 135973;

        public int FAngle;
        public Aow.Units.Animation.TAnimation FAnimation;
        public Aow.Units.AoWE.TArmyID FArmyID;
        // GlobalTargetSpells.pas:1899
        public override void Destroy()
        {
            FArmyID.Free();
            base.Destroy();
        }

        // GlobalTargetSpells.pas:1939
        // { needs: Boolean result on the shared TSpellTE.Validate virtual. }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FArmyID, null, 40);
        }

        // GlobalTargetSpells.pas:1893
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FArmyID = null;
        }

        // GlobalTargetSpells.pas:1991
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.Animation.TAnimation Anim = default;
            Aow.Units.AoWE.TArmyEventLog Log = default;
            byte Selection = default;
            bool Damaged = default;
            int Damage = default;
            int I = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.AoWE.TAbstractUnit UnitObj = default;
            if (A2 == 0)
            {
                FAngle = 0;
                var call__225 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__226 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__226.CastingDone(A1, call__225.FBaseManaCost);
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                bool bool__227 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__227)
                {
                    var call__228 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                    bool__227 = call__228;
                }

                if (bool__227)
                {
                    var call__229 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                    call__229.FCastSounds.Play(0);
                }

                Anim = new Aow.Units.Animation.TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                Anim.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
            }

            if (A2 == 80)
            {
                bool bool__230 = (FArmyID != null);
                if (bool__230)
                {
                    var call__231 = FArmyID.RetrieveArmy(ref Army, ref Selection);
                    bool__230 = call__231;
                }

                if (bool__230)
                {
                    Log = Aow.Units.AoWE.TArmyEventLog.Create(null);
                    var call__232 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VortexDamageRStr);
                    Log.FArmyText = call__232;
                    Log.FPlayer = unchecked((sbyte)A1.FPlayer);
                    var call__233 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VortexRStr);
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
                            if (!UnitObj.GetMoveTypes().Contains(Aow.Units.AoWE.TMoveType.mtFlying))
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

                                Damage = UnitObj.ExecuteDamageRole(8, Damage, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical));
                                if (Damage > UnitObj.GetHitPoints())
                                {
                                    Damage = UnitObj.GetHitPoints();
                                }

                                if (Damage > 0)
                                {
                                    var call__236 = UnitObj.GetHitPoints();
                                    UnitObj.SetHitPoints(unchecked((sbyte)(call__236 - Damage)));
                                    Damaged = true;
                                }
                            }
                            else
                            {
                                Damage = 0;
                            }

                            Log.FDamageValues.Add(unchecked((byte)Damage));
                            Log.FDamageTypes.Add(7);
                        }
                    }

                    if (Damaged)
                    {
                        Log.ValidateGameOver();
                        var call__237 = Army.Yhx;
                        var call__238 = Army.Xhx;
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)Army.FArmy.FPlayer), unchecked((sbyte)call__238), unchecked((sbyte)call__237), unchecked((sbyte)Army.Lhx), true);
                    }

                    Log.Release();
                    Army.FArmy.Validate();
                    Army.FArmy.ValidateTerrainEx(unchecked((sbyte)((Aow.Units.HSEngine.TMapField)Army.Owner).TerrainType), unchecked((sbyte)((Aow.Units.HSEngine.TMapField)Army.Owner).OverlayType), unchecked((sbyte)A1.FPlayer));
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
        public void ShowAnimation(Aow.Units.Animation.TAnimation Animation, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
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
            if (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)
            {
                Aow.Units.HSEngine.HSEngine.HXtoHP(Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                FAngle = (FAngle + 20) + (FAnimation.FFrameCounter / 5);
                Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    byte end__239 = 200;
                    for (; I <= end__239; I += 1)
                    {
                        Direction = Aow.Rtl.Rtl.Random(360);
                        if ((Animation.FFrameCounter >= ((int)(((uint)(200 - I)) >> 2))) && (Animation.FFrameCounter <= (((int)(((uint)I) >> 4)) + 68)))
                        {
                            Radius = Aow.Rtl.Rtl.Round(((double)(((double)2.997) - System.Math.Log(((double)(((double)(I * 19)) * 0.005)) + 1))) * 35) + 18;
                            Height = -16;
                            XShift = 16;
                            var call__240 = System.Math.PI;
                            Angle = (float)(((double)(((double)-(Direction + FAngle)) * call__240)) / 180);
                            if (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 0)
                            {
                                Count = 1;
                            }
                            else
                            {
                                if (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 1)
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
                                    X = Aow.Rtl.Rtl.Round(((double)System.Math.Cos(Angle)) * Radius) + XShift;
                                    Y = Aow.Rtl.Rtl.Round(((double)(((double)System.Math.Sin(Angle)) * Radius)) / 1.75) + Height;
                                    var call__242 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(70);
                                    call__242.FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                                    Angle = (float)(((double)0.035) + Angle);
                                }
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:242
        public static new Aow.Units.GlobalTargetSpells.TVortexTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TVortexTE o = new Aow.Units.GlobalTargetSpells.TVortexTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // GlobalTargetSpells.pas:1905
            public override int ClassID() => Aow.Units.GlobalTargetSpells.TVortexTE.ClassID;

            // GlobalTargetSpells.pas:242
            public new Aow.Units.GlobalTargetSpells.TVortexTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.TVortexTE)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TVortexTE => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TVortex (instance size 60, VMT 5579DE1C) }
    public partial class TVortex : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2069
        private protected TVortex(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__243 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VortexRStr);
            this.FSpellName = call__243;
            FSpellID = 70;
            FAboveGroundOnly = false;
        }

        // GlobalTargetSpells.pas:2120
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            Aow.Units.AoWE.TTargetSelectionControl Target = default;
            // { Use the incoming target. }
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            A2.FOnValidateTarget = ValidateTargetEvent;
        }

        // GlobalTargetSpells.pas:2100
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TVortexTE Event = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__244 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__245 = call__244.GetBusy();
            if (!call__245)
            {
                Event = Aow.Units.GlobalTargetSpells.TVortexTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
                var call__246 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(A2, A3, A4);
                var call__247 = ((Aow.Units.AoWE.TAoWMapField)call__246).GetArmyHS();
                Army = call__247;
                if (Army != null)
                {
                    var call__248 = Army.FArmy.CreateID(255);
                    Event.FArmyID = call__248;
                }

                var call__249 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__249.AddEvent(Event);
                // { Original queues through AddEvent, VMT +$10. }
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
        public override bool ValidTargetSelection(Aow.Units.AoWE.TTargetSelectionControl A1)
        {
            bool Result = default;
            Aow.Units.HSEngine.TMapField MapF = default;
            if (A1.FTargetX != -1)
            {
                var call__250 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[A1.FTargetL];
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
        public void ValidateTargetEvent(Aow.Units.AoWE.TTargetSelectionControl Target, sbyte X, sbyte Y, sbyte Layer, ref bool Valid)
        {
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            if (X != -1)
            {
                var call__252 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Layer];
                MapLevel = call__252;
                var call__253 = MapLevel.GetField(X, Y);
                Valid = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)call__253.TerrainType);
            }
            else
            {
                Valid = false;
            }
        }

        public static new Aow.Units.GlobalTargetSpells.TVortex Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TVortex(context, owner));
    }

    // { THealingShowersTE (instance size 40, VMT 5579DF48): $24 animation. }
    public partial class THealingShowersTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:2138
        public new const int ClassID = 135975;

        public Aow.Units.Animation.TAnimation FAnimation;
        // GlobalTargetSpells.pas:2190
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.AoWE.TAbstractUnit UnitObj = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.Animation.TImageSequenceAnimation Anim = default;
            bool Healed = default;
            bool PlaySound = default;
            int I = default;
            Aow.Units.HSEngine.THexagonSprite Cloud = default;
            base.Process(A1, A2);
            if (A2 == 0)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                bool bool__254 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__254)
                {
                    var call__255 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                    bool__254 = call__255;
                }

                if (bool__254)
                {
                    var call__256 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(73);
                    call__256.FCastSounds.Play(0);
                }

                Anim = (Aow.Units.Animation.TImageSequenceAnimation)new Aow.Units.Animation.TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                Anim.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
                var call__257 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__257.CastingDone(A1, FEventMana);
            }

            if (A2 == 50)
            {
                PlaySound = false;
                Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, null);
                var call__258 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer];
                Level = call__258;
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, 0);
                while (Walk.Radius <= 1)
                {
                    if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Level.Height > Walk.Y))
                    {
                        var call__259 = Level.GetField(Walk.X, Walk.Y);
                        var call__260 = ((Aow.Units.AoWE.TAoWMapField)call__259).GetMagicShieldActive();
                        if (!call__260)
                        {
                            Healed = false;
                            var call__261 = Level.GetField(Walk.X, Walk.Y);
                            var call__262 = ((Aow.Units.AoWE.TAoWMapField)call__261).GetArmyHS();
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
                                            UnitObj.SetHitPoints(unchecked((sbyte)(call__267 + 5)));
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
                                    var call__271 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(call__270, call__269, Army.Lhx, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                    bool__268 = call__271;
                                }

                                if (bool__268)
                                {
                                    bool bool__272 = PlaySound || (unchecked((byte)Army.FArmy.FPlayer) == unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                    if (!bool__272)
                                    {
                                        var call__273 = Army.Yhx;
                                        var call__274 = Army.Xhx;
                                        var call__275 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__274), unchecked((sbyte)call__273), unchecked((sbyte)Army.Lhx), 1);
                                        bool__272 = call__275;
                                    }

                                    PlaySound = bool__272;
                                    Anim = new Aow.Units.Animation.TImageSequenceAnimation(null);
                                    var call__276 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(47);
                                    var call__277 = call__276.FImageSequences.ImageSequences[0];
                                    Anim.SetImageSequence(call__277);
                                    Anim.FAnimationLevel = 27;
                                    var call__278 = Army.Yhx;
                                    var call__279 = Army.Xhx;
                                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, call__279, call__278, Army.Lhx);
                                }
                            }

                            var call__280 = Level.GetField(Walk.X, Walk.Y);
                            var call__281 = call__280.FindOwnedChild(132990);
                            Cloud = (Aow.Units.HSEngine.THexagonSprite)call__281;
                            if (Cloud != null)
                            {
                                Cloud.Free();
                            }
                        }
                    }

                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }

                if (PlaySound)
                {
                    var call__282 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(47);
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
        public void ShowAnimation(Aow.Units.Animation.TAnimation Animation, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
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
            if (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)
            {
                Aow.Units.HSEngine.HSEngine.HXtoHP(Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                var call__283 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetDetailLevel();
                switch (call__283)
                {
                    case Aow.Units.AoWReg.TDetailLevel.dtlLow:
                        Count = 50;
                        break;
                    case Aow.Units.AoWReg.TDetailLevel.dtlHigh:
                        Count = 160;
                        break;
                    default:
                        Count = 100;
                        break;
                }

                Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    int end__284 = Count;
                    for (; I <= end__284; I += 1)
                    {
                        X = Aow.Rtl.Rtl.Random(100) - 50;
                        Y = Aow.Rtl.Rtl.Random(100) - 50;
                        Speed = Aow.Rtl.Rtl.Random(5) + 5;
                        var call__285 = Aow.Rtl.Rtl.Random(30);
                        Frame = Animation.FFrameCounter - call__285;
                        if ((Frame >= 0) && (Frame < 50))
                        {
                            Frame = Frame % 30;
                            X -= Aow.Rtl.Rtl.Round(((double)(Frame * 4)) * (((double)Speed) * 0.2));
                            Y += Aow.Rtl.Rtl.Round(((double)(Frame * 20)) * (((double)Speed) * 0.2));
                            if (Y < 500)
                            {
                                {
                                    J = 0;
                                    byte end__286 = 10;
                                    for (; J <= end__286; J += 1)
                                    {
                                        XShift = X - Aow.Rtl.Rtl.Round(((double)(J * 2)) * 0.1);
                                        YShift = Y + Aow.Rtl.Rtl.Round(((double)(J * 10)) * 0.1);
                                        var call__287 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(73);
                                        call__287.FCastImages.Show(0, (Xhp + XShift) + 120, ((Yhp + YShift) - 500) + 30, 0, SurfaceDesc);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:2181
        public void ChangeTerrain(Aow.Units.HSEngine.TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
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
        public static new Aow.Units.GlobalTargetSpells.THealingShowersTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.THealingShowersTE o = new Aow.Units.GlobalTargetSpells.THealingShowersTE();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // GlobalTargetSpells.pas:2138
            public override int ClassID() => Aow.Units.GlobalTargetSpells.THealingShowersTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.THealingShowersTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.THealingShowersTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.GlobalTargetSpells.THealingShowersTE)base.Create(AOwner);
        }
    }

    // { THealingShowers (instance size 60, VMT 5579E030) }
    public partial class THealingShowers : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2277
        private protected THealingShowers(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__288 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HealingShowersRStr);
            this.FSpellName = call__288;
            FSpellID = 73;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
        }

        // GlobalTargetSpells.pas:2295
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            Aow.Units.AoWE.TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__289 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__289; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:2308
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.THealingShowersTE Event = default;
            Event = Aow.Units.GlobalTargetSpells.THealingShowersTE.Create(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__290 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__290.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:2291
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Result = base.ValidTargetSelectionMapF(A1);
            return Result;
        }

        // GlobalTargetSpells.pas:2287
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Result = base.ValidTargetMapF(A1);
            return Result;
        }

        public static new Aow.Units.GlobalTargetSpells.THealingShowers Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new THealingShowers(context, owner));
    }

    // { TSmokeCloudSpell (instance size 60, VMT 5579E16C) }
    public partial class TSmokeCloudSpell : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2325
        // { needs: TFireControl.PlaceFire(X,Y,Layer:Integer; Create:Boolean): TFire,
//   result consumed at $5579EDC3; GetSource returns TAbstractUnit at $5579EDE2. }
        private protected TSmokeCloudSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__291 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SmokeCloudRStr);
            this.FSpellName = call__291;
            FSpellID = 74;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
        }

        // GlobalTargetSpells.pas:2373
        // { needs: TFireControl.PlaceFire(X,Y,Layer:Integer; Create:Boolean): TFire,
//   result consumed at $5579EDC3; GetSource returns TAbstractUnit at $5579EDE2. }
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            Aow.Units.Engine.TEResource Resource = default;
            Aow.Units.SmokeCloud.TSmokeCloud Cloud = default;
            var call__292 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).GetSource();
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)call__292.FPlayer), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Layer), 2);
            var call__293 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).GetSource();
            bool bool__294 = (call__293.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__294)
            {
                var call__295 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Layer), 2);
                bool__294 = call__295;
            }

            if (bool__294)
            {
                var call__296 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FEventSpellID);
                call__296.FCastSounds.Play(0);
            }

            var call__297 = ((Aow.Units.HSEngine.TMapObjectResourceList)((Aow.Rtl.TObject)Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList)).FindChild(135747);
            Resource = (Aow.Units.Engine.TEResource)call__297;
            if (Resource != null)
            {
                Cloud = (Aow.Units.SmokeCloud.TSmokeCloud)Resource.CreateResourceUser();
                Cloud.FPlayer = unchecked((sbyte)A2.FPlayer);
                Cloud.FTurnsRemaining = -1;
                ((Aow.Units.AoWE.TGlobalEnchantment)Cloud.FEnchantment).FPlayerIndex = unchecked((sbyte)A2.FPlayer);
                ((Aow.Units.AoWE.TGlobalEnchantment)Cloud.FEnchantment).FEnchantmentManaUpkeep = this.FManaUpkeep;
                Cloud.FDrifting = false;
                var call__298 = Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Cloud, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Layer);
                if (!call__298)
                {
                    Cloud.Free();
                }

                var call__299 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(this.FSpellID);
                call__299.CastingDone(A2, this.FBaseManaCost);
            }

            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:2343
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            Aow.Units.AoWE.TTargetSelectionControl Target = default;
            int I = default;
            Target = A2;
            base.SetupTargetSelection(A1, Target);
            Target.FHNs.Clear();
            {
                I = 0;
                int end__300 = Aow.Units.HSEngine.HSEngine.RadToHN(4) - 1;
                for (; I <= end__300; I += 1)
                {
                    Target.FHNs.Add(0);
                }
            }
        }

        // GlobalTargetSpells.pas:2356
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE Event = default;
            Event = new Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__301 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__301.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:2339
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Result = base.ValidTargetSelectionMapF(A1);
            return Result;
        }

        // GlobalTargetSpells.pas:2335
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Result = base.ValidTargetMapF(A1);
            return Result;
        }

        public static new Aow.Units.GlobalTargetSpells.TSmokeCloudSpell Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSmokeCloudSpell(context, owner));
    }

    // { draft }
    // { needs: corrected result/callback signatures on TGlobalTargetSpell and
//     its P7 descendants. This call-only view uses the original VMT slots. }
    public partial class TGlobalTargetSpellCalls : Aow.Units.AoWE.TSpell
    {
        // GlobalTargetSpells.pas:303
        public virtual void ShowSelection(Aow.Units.AoWE.TTargetSelectionControl Target, int HN, int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:305
        public virtual void SelectionChanged(Aow.Rtl.TObject Sender)
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
        public Aow.Units.FixedArray_byte_8 Reserved;
        public Aow.Rtl.AnsiString Name;
    }

    // { needs: typed TTargetSelectionControl fields. This view preserves all offsets;
//     $04 library, $0D mode, $10 show callback, $20 distance, $24/$28/$2C center,
//     $34 change events, $38 change, $39/$3A/$3B location, $3C text,
//     $40/$44 captions, $50/$54/$58 preview images. }
    public partial class TTargetSelectionView : Aow.Rtl.TObject
    {
        public Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary;
        public int Reserved08;
        public byte Status;
        public TSet<Aow.Units.AoWE.TTargetSelectionModeElement> Mode;
        public ushort Padding0E;
        public Aow.Units.AoWE.TShowTargetSelectionEvent OnShow;
        public Aow.Units.AoWE.TValidateTargetEvent OnValidate;
        public int Distance;
        public int CenterX;
        public int CenterY;
        public int CenterLayer;
        public int Reserved30;
        public Aow.Units.Engine.TEventList Changes;
        public Aow.Units.AoWE.TTargetSelectionChange Change;
        public sbyte X;
        public sbyte Y;
        public sbyte Layer;
        public Aow.Rtl.TStrings Text;
        public Aow.Rtl.AnsiString Caption;
        public Aow.Rtl.AnsiString CancelCaption;
        public int Reserved48;
        public int Reserved4C;
        public Aow.Units.ImageLib.TLibraryImage Preview;
        public Aow.Units.ImageLib.TLibraryImage ValidPreview;
        public Aow.Units.ImageLib.TLibraryImage InvalidPreview;
    }

    public struct TTargetSelectionFields
    {
        public object VMT;
        public Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary;
        public int Reserved08;
        public byte Status;
        public TSet<Aow.Units.AoWE.TTargetSelectionModeElement> Mode;
        public ushort Padding0E;
        public Aow.Units.AoWE.TShowTargetSelectionEvent OnShow;
        public Aow.Units.AoWE.TValidateTargetEvent OnValidate;
        public int Distance;
        public int CenterX;
        public int CenterY;
        public int CenterLayer;
        public int Reserved30;
        public Aow.Units.Engine.TEventList Changes;
        public Aow.Units.AoWE.TTargetSelectionChange Change;
        public sbyte X;
        public sbyte Y;
        public sbyte Layer;
        public Aow.Rtl.TStrings Text;
        public Aow.Rtl.AnsiString Caption;
        public Aow.Rtl.AnsiString CancelCaption;
        public int Reserved48;
        public int Reserved4C;
        public Aow.Units.ImageLib.TLibraryImage Preview;
        public Aow.Units.ImageLib.TLibraryImage ValidPreview;
        public Aow.Units.ImageLib.TLibraryImage InvalidPreview;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TMapSeatedPlayerView
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    // { Typed call-only methods retain original argument evaluation order without
//     altering the foreign draft headers or their implementation signatures. }
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
        public Aow.Units.FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    // { needs: the shared army and spell-event drafts do not declare results or
//   var parameters. These typed entry shims preserve their original register ABI. }
    [StructLayout(LayoutKind.Explicit, Size = 257)]
    public struct TSpellMoveTable
    {
        [FieldOffset(0)]
        public byte Flags;
        [FieldOffset(1)]
        public Aow.Units.FixedArray_Aow_Units_FixedArray_sbyte_16_16 Cost;
    }

    public struct TSpellAnimationFields
    {
        public Aow.Units.FixedArray_byte_36 Reserved;
        public Aow.Units.Animation.TShowAnimationEvent OnShow;
    }

    [StructLayout(LayoutKind.Explicit, Size = 110)]
    public struct TSpellEngineFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_109 Reserved;
        [FieldOffset(109)]
        public byte Detail;
    }

    public struct TSpellAIGroupFields
    {
        public Aow.Units.FixedArray_byte_80 Reserved;
        public Aow.Units.Engine.TIntegerList Enemies;
    }

    public struct TSpellRegistryFields
    {
        public Aow.Units.FixedArray_byte_92 Reserved;
        public Aow.Units.AoWReg.TAoWRegistry Registry;
    }

    public struct TSpellEngineSetFields
    {
        public Aow.Units.FixedArray_byte_100 Reserved;
        public Aow.Units.AoWE.TAoWHSSet HSSet;
    }

    public static partial class GlobalTargetSpells
    {
        // GlobalTargetSpells.pas:2417
        public static void RegisterGlobalTargetSpells(Aow.Units.AoWE.TAoWEngine A1)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = A1.FGameSet.FSpellControl;
            ((Aow.Units.Engine.TEngine)A1).RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TFreezeWaterTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.AoWE.TFrozenWaterHS>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TFireBarrierTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TLevelTerrainTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TRejuvenateTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TWatcherTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TWatcherHS>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TTornadoTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TVortexTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.THealingShowersTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE>()]);
            var call__302 = Aow.Units.GlobalTargetSpells.TFreezeWater.Create(null);
            Control.RegisterSpell(call__302);
            var call__303 = Aow.Units.GlobalTargetSpells.TFireBarrier.Create(null);
            Control.RegisterSpell(call__303);
            var call__304 = Aow.Units.GlobalTargetSpells.TLevelTerrain.Create(null);
            Control.RegisterSpell(call__304);
            var call__305 = Aow.Units.GlobalTargetSpells.TRejuvenate.Create(null);
            Control.RegisterSpell(call__305);
            var call__306 = Aow.Units.GlobalTargetSpells.TWatcher.Create(null);
            Control.RegisterSpell(call__306);
            var call__307 = Aow.Units.GlobalTargetSpells.TTornado.Create(null);
            Control.RegisterSpell(call__307);
            var call__308 = Aow.Units.GlobalTargetSpells.TVortex.Create(null);
            Control.RegisterSpell(call__308);
            var call__309 = Aow.Units.GlobalTargetSpells.THealingShowers.Create(null);
            Control.RegisterSpell(call__309);
            var call__310 = Aow.Units.GlobalTargetSpells.TSmokeCloudSpell.Create(null);
            Control.RegisterSpell(call__310);
        }

        // GlobalTargetSpells.pas:2435
        public static void UnregisterGlobalTargetSpells(int A1)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = ((Aow.Units.AoWE.TAoWEngine)((System.Object)A1)).FGameSet.FSpellControl;
            Control.UnregisterSpellID(10);
            Control.UnregisterSpellID(11);
            Control.UnregisterSpellID(18);
            Control.UnregisterSpellID(41);
            Control.UnregisterSpellID(57);
            Control.UnregisterSpellID(68);
            Control.UnregisterSpellID(70);
            Control.UnregisterSpellID(73);
            Control.UnregisterSpellID(74);
            Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.Engine.TEngine>((int)A1).UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TFreezeWaterTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.AoWE.TFrozenWaterHS>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TFireBarrierTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TLevelTerrainTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TRejuvenateTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TWatcherTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TWatcherHS>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TTornadoTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TVortexTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.THealingShowersTE>(), Aow.Units.Engine.EClass.Of<Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE>()]);
        }

        // { Import cells bind to the shared globals; distinct references preserve the original reloads. }
        public static System.Type SpellStructureClass = typeof(Aow.Units.AoWE.TStructure);
        // { needs: AoWE.CancelRStr, referenced at $5579E664; absent from draft. }
        public static readonly Aow.Rtl.AnsiString SpellCancelRStr = (AnsiString)"Cancel";

        // GlobalTargetSpells.pas:552
        // { needs: Boolean result; stack arguments require the normal EBP frame. }
        // { needs: Boolean results on the shared TSpell virtuals. Helpers keep managed
//   string finalization before returning AL; the public methods tail-call them. }
        public static bool GlobalTargetCanActivate(Aow.Units.GlobalTargetSpells.TGlobalTargetSpell Spell, int Caster, ref Aow.Rtl.AnsiString MessageText)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Result = ((Aow.Units.AoWE.TSpell)(Spell)).CanActivate__static(Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>(Caster), ref MessageText);
            if (Result && Spell.FAboveGroundOnly)
            {
                Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>((int)Caster).GetLocation(ref X, ref Y, ref Layer);
                if (Layer != 0)
                {
                    Result = false;
                    MessageText = Aow.Units.AoWE.AoWE.TranslateRStr(CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:601
        public static bool GlobalTargetActivate(Aow.Units.GlobalTargetSpells.TGlobalTargetSpell Spell, int Caster, int A2)
        {
            bool Result = default;
            Aow.Rtl.AnsiString MessageText = default;
            ((Aow.Units.AoWE.TSpell)(Spell)).Activate__static(Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>(Caster));
            if (((Aow.Units.AoWE.TSpell)(Spell)).CanActivate(Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>(Caster), ref MessageText))
            {
                Spell.FCasterID = Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>((int)Caster).FUnitID;
                Spell.FCasterPlayer = unchecked((sbyte)Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>((int)Caster).FPlayer);
                Spell.SetupTargetSelection(Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>((int)Caster), Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection);
                Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        public static readonly Aow.Rtl.AnsiString SpellXCastRStr = (AnsiString)"%s cast";

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
