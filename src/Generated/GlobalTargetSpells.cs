// Generated from GlobalTargetSpells.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
                Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
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
        public static new Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE o = new Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE();
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

        public new partial class Meta : Aow.Units.AoWE.TSpellTE.Meta
        {
            // GlobalTargetSpells.pas:401
            public override int ClassID()
            {
                int Result = default;
                Result = 135976;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE o = (Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)NewInstance();
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
        public int FCasterID;
        public sbyte FCasterPlayer;
        public bool FAboveGroundOnly;
        public Aow.Units.FixedArray_byte_2 F_GlobalTargetSpell_3A;
        // GlobalTargetSpells.pas:439
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // GlobalTargetSpells.pas:492
        // { +09C }
        public virtual void OnTargetSelectionChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.GlobalTargetSpells.TTargetSelectionView Target = default;
            Aow.Units.AoWE.TAbstractUnit Caster = default;
            Target = (Aow.Units.GlobalTargetSpells.TTargetSelectionView)Sender;
            var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterID);
            Caster = call__10;
            if (Caster != null)
            {
                switch (((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Sender)).FChange)
                {
                    case Aow.Units.AoWE.TTargetSelectionChange.tscDeactivated:
                        ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Sender)).FOnChange.Remove(OnTargetSelectionChange);
                        ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Target)).Deactivate();
                        break;
                    case Aow.Units.AoWE.TTargetSelectionChange.tscSelected:
                        if (this.ValidTargetSelection((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Target)))
                        {
                            var call__11 = this.TargetSelected((Aow.Units.AoWE.TAbstractUnit)((System.Object)Caster), ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Sender)).FTargetX, ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Sender)).FTargetY, ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Sender)).FTargetL);
                            if (call__11)
                            {
                                ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Sender)).FOnChange.Remove(OnTargetSelectionChange);
                                ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Target)).Deactivate();
                            }
                        }

                        break;
                }
            }
            else
            {
                ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Sender)).FOnChange.Remove(OnTargetSelectionChange);
                ((Aow.Units.AoWE.TTargetSelectionControl)((System.Object)Target)).Deactivate();
            }
        }

        // GlobalTargetSpells.pas:521
        // { +0A0 }
        public virtual void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            sbyte UnitX = default;
            sbyte UnitY = default;
            sbyte UnitLayer = default;
            {
                var w__12 = A2;
                w__12.Initialize(A1);
                w__12.FTargetMode = w__12.FTargetMode + TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.Of(Aow.Units.AoWE.TTargetSelectionModeElement.tmOwnerDraw, Aow.Units.AoWE.TTargetSelectionModeElement.tmValidateDistance, Aow.Units.AoWE.TTargetSelectionModeElement.tmExternalFloater);
                A1.GetLocation(ref UnitX, ref UnitY, ref UnitLayer);
                w__12.FSourceX = UnitX;
                w__12.FSourceY = UnitY;
                w__12.FSourceL = UnitLayer;
                w__12.FRange = 12;
                w__12.FOnChange.Add(OnTargetSelectionChange);
                w__12.FOnShow = ShowTargetSelection;
                var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectTargetLocationForXOnMapRStr);
                var call__14 = Aow.Rtl.Rtl.Format(call__13, [FSpellName]);
                w__12.FText.Text = call__14;
                w__12.FTitle = FSpellName;
                var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
                w__12.FCancelText = call__15;
                var call__16 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__17 = call__16.FCastImages.ImageSequences[20];
                var call__18 = call__17.GetPreviewImage();
                w__12.FSourceImage = call__18;
                var call__19 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__20 = call__19.FCastImages.ImageSequences[21];
                var call__21 = call__20.GetPreviewImage();
                w__12.FValidTargetImage = call__21;
                var call__22 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__23 = call__22.FCastImages.ImageSequences[22];
                var call__24 = call__23.GetPreviewImage();
                w__12.FInvalidTargetImage = call__24;
            }
        }

        // GlobalTargetSpells.pas:544
        // { +0A4 }
        public virtual bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

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
            bool bool__25 = Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled;
            if (bool__25)
            {
                var call__26 = ((Aow.Units.AoWE.TAoWMapField)A1).GetExplored(FCasterPlayer);
                bool__25 = !call__26;
            }

            if (bool__25)
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
        public virtual bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:62
        public static new Aow.Units.GlobalTargetSpells.TGlobalTargetSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TGlobalTargetSpell o = new Aow.Units.GlobalTargetSpells.TGlobalTargetSpell();
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
    }

    // { +0B4  }
    // { TFreezeWaterTE (instance size 36, VMT 5579CDE0) }
    public partial class TFreezeWaterTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:679
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            base.Process(A1, A2);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
            var call__27 = Aow.Units.AoWE.AoWE.AoWHSMap.TerrainVisible(unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
            if (call__27)
            {
                Twinkle = Aow.Units.Animation.TTwinkleAnimation.Create(null);
                var call__28 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(10);
                var call__29 = call__28.FCastImages.ImageSequences[0];
                Twinkle.Setup(call__29, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmExpand), 100, 64, 50, 30);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
            }

            // { needs: TAbstractAoWHSMap.$A4 contains the seated player at byte $A5. }
            bool bool__30 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
            if (!bool__30)
            {
                var call__31 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
                bool__30 = call__31;
            }

            if (bool__30)
            {
                var call__32 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(10);
                call__32.FCastSounds.Play(0);
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, TerrainChanged);
            var call__33 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__33.CastingDone(A1, FEventMana);
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
                                var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                                Ice.FDaysRemaining = unchecked((sbyte)(((call__34 - 5) / 5) + 3));
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

                var call__35 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                Ice.FDaysRemaining = unchecked((sbyte)(((call__35 - 5) / 5) + 3));
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
            public override int ClassID()
            {
                int Result = default;
                Result = 135939;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TFreezeWaterTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TFreezeWaterTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TFreezeWaterTE o = (Aow.Units.GlobalTargetSpells.TFreezeWaterTE)NewInstance();
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
        }
    }

    // { TFreezeWater (instance size 60, VMT 5579CEC0) }
    public partial class TFreezeWater : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:710
        // { needs: TArmyHS.VisibleForSeatedPlayer: Boolean; $5579EC2B tests AL. }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__36 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FreezeWaterRStr);
            this.FSpellName = call__36;
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
                int end__37 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__37; I += 1)
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
            var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__38.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:86
        public static new Aow.Units.GlobalTargetSpells.TFreezeWater Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TFreezeWater o = new Aow.Units.GlobalTargetSpells.TFreezeWater();
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
    }

    // { TFireBarrierTE (instance size 36, VMT 5579CFFC) }
    public partial class TFireBarrierTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:873
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            CreateFireBarrier();
            var call__39 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__40 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__40.CastingDone(A1, call__39.FBaseManaCost);
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
            var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__41;
            var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer];
            Level = call__42;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__43 = Level.Field[Walk.X, Walk.Y];
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__43;
                    var call__44 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(11);
                    var call__45 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpell)((System.Object)call__44)).ValidTargetMapF((Aow.Units.HSEngine.TMapField)((System.Object)MapF));
                    bool bool__46 = call__45;
                    if (bool__46)
                    {
                        var call__47 = MapF.GetArmyHS();
                        bool__46 = (call__47 == null);
                    }

                    if (bool__46)
                    {
                        var call__48 = Aow.Units.AoWE.AoWE.AoWHSSet.FFireControl.PlaceFire(Walk.X, Walk.Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, Aow.Units.AoWE.TFireType.ftLarge);
                        Fire = call__48;
                        if (Fire != null)
                        {
                            var call__49 = Aow.Rtl.Rtl.Random(35);
                            Fire.SetupExplosion(call__49);
                            Fire.FTurnsLeft = 3;
                            var call__50 = this.GetSource();
                            Fire.FPlayer = unchecked((sbyte)call__50.FPlayer);
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
            public override int ClassID()
            {
                int Result = default;
                Result = 135940;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TFireBarrierTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TFireBarrierTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TFireBarrierTE o = (Aow.Units.GlobalTargetSpells.TFireBarrierTE)NewInstance();
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
        }
    }

    // { draft }
    // { TFireBarrier (instance size 60, VMT 5579D0DC) }
    public partial class TFireBarrier : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:882
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireBarrierRStr);
            this.FSpellName = call__51;
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
                int end__52 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__52; I += 1)
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
            var call__53 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__53.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:900
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__54 = base.ValidTargetSelectionMapF(A1);
            bool bool__55 = call__54 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__55)
            {
                var call__56 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__55 = (call__56 == null);
            }

            Result = bool__55;
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
            var call__57 = base.ValidTargetMapF(A1);
            bool bool__58 = call__57 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__58)
            {
                var call__59 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__58 = (call__59 == null);
            }

            Result = bool__58;
            if (Result)
            {
                Result = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS() == null;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:104
        public static new Aow.Units.GlobalTargetSpells.TFireBarrier Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TFireBarrier o = new Aow.Units.GlobalTargetSpells.TFireBarrier();
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
    }

    // { TLevelTerrainTE (instance size 36, VMT 5579D218) }
    public partial class TLevelTerrainTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:968
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            PlayDefaultEffects(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer));
            LevelTerrain();
            var call__60 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__61 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__61.CastingDone(A1, call__60.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalTargetSpells.pas:943
        public void LevelTerrain()
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            var call__62 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer];
            Level = call__62;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__63 = Level.Field[Walk.X, Walk.Y];
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__63;
                    var call__64 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(18);
                    var call__65 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpell)((System.Object)call__64)).ValidTargetMapF((Aow.Units.HSEngine.TMapField)((System.Object)MapF));
                    bool bool__66 = call__65;
                    if (bool__66)
                    {
                        var call__67 = MapF.GetArmyHS();
                        bool__66 = (call__67 == null);
                    }

                    if (bool__66)
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
            public override int ClassID()
            {
                int Result = default;
                Result = 135945;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TLevelTerrainTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TLevelTerrainTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TLevelTerrainTE o = (Aow.Units.GlobalTargetSpells.TLevelTerrainTE)NewInstance();
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
        }
    }

    // { draft }
    // { TLevelTerrain (instance size 60, VMT 5579D2F8) }
    public partial class TLevelTerrain : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:978
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__68 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelTerrainRStr);
            this.FSpellName = call__68;
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
                int end__69 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__69; I += 1)
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
            var call__70 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__70.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:996
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__71 = base.ValidTargetSelectionMapF(A1);
            bool bool__72 = call__71 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__72)
            {
                var call__73 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__72 = (call__73 == null);
            }

            Result = bool__72;
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
            var call__74 = base.ValidTargetMapF(A1);
            bool bool__75 = call__74 && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)A1.TerrainType);
            if (bool__75)
            {
                var call__76 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__75 = (call__76 == null);
            }

            Result = bool__75;
            if (Result)
            {
                Result = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS() == null;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:122
        public static new Aow.Units.GlobalTargetSpells.TLevelTerrain Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TLevelTerrain o = new Aow.Units.GlobalTargetSpells.TLevelTerrain();
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
    }

    // { TRejuvenateTE (instance size 36, VMT 5579D434) }
    public partial class TRejuvenateTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1044
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            base.Process(A1, A2);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
            var call__77 = Aow.Units.AoWE.AoWE.AoWHSMap.TerrainVisible(unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
            if (call__77)
            {
                Twinkle = Aow.Units.Animation.TTwinkleAnimation.Create(null);
                var call__78 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(41);
                var call__79 = call__78.FCastImages.ImageSequences[0];
                Twinkle.Setup(call__79, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 100, 64, 50, 30);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
            }

            // { needs: TAbstractAoWHSMap.$A4 contains the seated player at byte $A5. }
            bool bool__80 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
            if (!bool__80)
            {
                var call__81 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y)), unchecked((sbyte)unchecked((byte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer)), 2);
                bool__80 = call__81;
            }

            if (bool__80)
            {
                var call__82 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(41);
                call__82.FCastSounds.Play(0);
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, 2, RejuvenateTerrain, null);
            var call__83 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__84 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__84.CastingDone(A1, call__83.FBaseManaCost);
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
            public override int ClassID()
            {
                int Result = default;
                Result = 135952;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TRejuvenateTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TRejuvenateTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TRejuvenateTE o = (Aow.Units.GlobalTargetSpells.TRejuvenateTE)NewInstance();
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
        }
    }

    // { TRejuvenate (instance size 60, VMT 5579D514) }
    public partial class TRejuvenate : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1074
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__85 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RejuvenateRStr);
            this.FSpellName = call__85;
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
                int end__86 = Aow.Units.HSEngine.HSEngine.RadToHN(3) - 1;
                for (; I <= end__86; I += 1)
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
            var call__87 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__87.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // GlobalTargetSpells.pas:1090
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__88 = base.ValidTargetSelectionMapF(A1);
            Result = call__88 && TSet<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)A1.TerrainType);
            return Result;
        }

        // GlobalTargetSpells.pas:1085
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__89 = base.ValidTargetMapF(A1);
            Result = call__89 && TSet<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)A1.TerrainType);
            return Result;
        }

        // GlobalTargetSpells.pas:140
        public static new Aow.Units.GlobalTargetSpells.TRejuvenate Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TRejuvenate o = new Aow.Units.GlobalTargetSpells.TRejuvenate();
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
    }

    // { TWatcherEnchantment (instance size 40, VMT 5579D648): $24 FWatcher. }
    public partial class TWatcherEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        public Aow.Units.GlobalTargetSpells.TWatcherHS FWatcher;
        // GlobalTargetSpells.pas:1121
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FEnchantmentMode = TSet<Aow.Units.AoWE.TGlobalEnchantmentModeElement>.FromMask((ulong)FEnchantmentMode.Mask) - TSet<Aow.Units.AoWE.TGlobalEnchantmentModeElement>.Of(Aow.Units.AoWE.TGlobalEnchantmentModeElement.gemTrueGlobal, Aow.Units.AoWE.TGlobalEnchantmentModeElement.gemOwned);
            FDispelMana = 50;
        }

        // GlobalTargetSpells.pas:1129
        public override Aow.Rtl.AnsiString GetName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__90 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
            Result = call__90.FSpellName;
            return Result;
        }

        // GlobalTargetSpells.pas:151
        public static new Aow.Units.GlobalTargetSpells.TWatcherEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TWatcherEnchantment o = new Aow.Units.GlobalTargetSpells.TWatcherEnchantment();
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
    }

    // { TWatcherHS (instance size 32, VMT 5579D7A0): $0C state, $0D unfog
//     radius, $10 enchantment, $14 animation frame, $18 unexplored HNs,
//     $1C visibility flag; $0E/$1D padding. }
    public partial class TWatcherHS : Aow.Units.HSEngine.TSingleHS
    {
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
            var call__91 = Aow.Units.GlobalTargetSpells.TWatcherEnchantment.Create(null);
            FEnchantment = call__91;
            FEnchantment.FWatcher = this;
            var call__92 = Aow.Units.Engine.TIntegerList.Create();
            FUnexplored = call__92;
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
            bool bool__93 = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == Lhx);
            if (bool__93)
            {
                var call__94 = Yhx;
                var call__95 = Xhx;
                var call__96 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)call__95), unchecked((sbyte)call__94), unchecked((sbyte)Lhx), 0);
                bool__93 = call__96;
            }

            if (bool__93)
            {
                var call__97 = Yhx;
                var call__98 = Xhx;
                Aow.Units.HSEngine.HSEngine.HXtoHP(call__98, call__97, ref X, ref Y);
                X = (X - XhpSurf) + 16;
                Y = (Y - YhpSurf) + 16;
                var call__99 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
                if (call__99.FCastImages != null)
                {
                    var call__100 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
                    call__100.FCastImages.ShowLooped(1, X, Y, Aow.Units.AoWE.AoWE.AoWHSMap.Frame, SurfaceDesc);
                }

                if (FAnimationFrame != -1)
                {
                    FAnimationFrame += 1;
                    // { Original reads the shared AoWEngine animation detail at byte $6D.
//         The map engine is not the global read by this routine. }
                    if ((Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 2) && (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == Lhx))
                    {
                        Intensity = 100 - FAnimationFrame;
                        var call__101 = Yhx;
                        var call__102 = Xhx;
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__102, call__101, ref X, ref Y);
                        X = (X - XhpSurf) - 8;
                        Y = Y - YhpSurf;
                        {
                            I = FUnexplored.Count - 1;
                            byte end__103 = 0;
                            for (; I >= end__103; I -= 1)
                            {
                                var call__104 = FUnexplored[I];
                                Aow.Units.HSEngine.HSEngine.HNtoHP(call__104, ref PX, ref PY);
                                PX += X;
                                PY += Y;
                                var call__105 = Aow.Units.AoWE.AoWE.AoWHSSet.FGeneralImages[5];
                                call__105.ShowBlended(PX, PY, SurfaceDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[Intensity]);
                            }
                        }
                    }

                    // { Original resets the shared random generator for deterministic particles. }
                    Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FInitialRandomSeed;
                    {
                        I = 0;
                        short end__106 = 300;
                        for (; I <= end__106; I += 1)
                        {
                            var call__107 = VisibilityRange();
                            var call__108 = Aow.Rtl.Rtl.Random(call__107 << 5);
                            Radius = call__108;
                            Angle = Aow.Rtl.Rtl.Random(360);
                            Delay = Aow.Rtl.Rtl.Random(80);
                            if ((FAnimationFrame - Delay) >= 0)
                            {
                                Aow.Units.AoWE.AoWE.DegreesRadToCo(Angle, Radius, ref DX, ref DY);
                                var call__109 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
                                call__109.FCastImages.Show(0, X + DX, Y + DY, FAnimationFrame - Delay, SurfaceDesc);
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
            {
                ref var w__110 = ref Msg;
                base.MapFieldMsgProc(ref Msg);
                if (w__110.MsgID == 4363)
                {
                    if (Activated())
                    {
                        OwnerPlayer = FEnchantment.FPlayerIndex;
                        bool bool__111 = (OwnerPlayer == w__110.Param[1].Value);
                        if (!bool__111)
                        {
                            var call__112 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer << 0);
                            var call__113 = Aow.Units.GlobalTargetSpells.GlobalTargetSpells.SpellRelation(call__112.FDiplomaticRelations, unchecked((byte)w__110.Param[1].Value));
                            bool__111 = (call__113 == 2);
                        }

                        if (bool__111)
                        {
                            var call__114 = VisibilityRange();
                            if (w__110.Param[0].Value <= call__114)
                            {
                                w__110.Result = 1;
                            }
                        }
                    }
                }

                if (w__110.MsgID == 4362)
                {
                    if (Activated())
                    {
                        OwnerPlayer = FEnchantment.FPlayerIndex;
                        bool bool__115 = (OwnerPlayer == w__110.Param[1].Value);
                        if (!bool__115)
                        {
                            var call__116 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer << 0);
                            var call__117 = Aow.Units.GlobalTargetSpells.GlobalTargetSpells.SpellRelation(call__116.FDiplomaticRelations, unchecked((byte)w__110.Param[1].Value));
                            bool__115 = (call__117 == 2);
                        }

                        if (bool__115)
                        {
                            var call__118 = VisibilityRange();
                            if (w__110.Param[0].Value <= call__118)
                            {
                                w__110.Result = 1;
                            }
                        }
                    }
                }

                if (w__110.MsgID == 4365)
                {
                    var call__119 = VisibilityRange();
                    if (call__119 >= w__110.Param[0].Value)
                    {
                        // { Param[0] is already available. }
                        Aow.Units.AoWE.AoWE.AoWHSMap.UpdatePlayerSeen(FEnchantment.FPlayerIndex, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx, w__110.Param[0].Value, w__110.Param[0].Value);
                    }

                    UpdateVisibleForSeatedPlayer();
                }
            }
        }

        // GlobalTargetSpells.pas:1350
        public override void Activate()
        {
            Aow.Units.HSEngine.TMapField MapF = default;
            base.Activate();
            FWatcherState = (byte)(TSet<Aow.Units.GlobalTargetSpells.TWatcherStateElement>.FromMask((ulong)FWatcherState) + TSet<Aow.Units.GlobalTargetSpells.TWatcherStateElement>.Of(Aow.Units.GlobalTargetSpells.TWatcherStateElement.wsActive)).Image;
            FEnchantment.Activate(unchecked((sbyte)FEnchantment.FPlayerIndex));
            {
                var w__120 = (Aow.Units.HSEngine.TMapField)Owner;
                Aow.Units.AoWE.AoWE.AoWHSMap.NotifyPlayerChanged(w__120.Xhx, w__120.Yhx, w__120.Lhx);
            }

            MapF = (Aow.Units.HSEngine.TMapField)Owner;
            var call__121 = VisibilityRange();
            var call__122 = VisibilityRange();
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdatePlayerSeen(FEnchantment.FPlayerIndex, MapF.Xhx, MapF.Yhx, MapF.Lhx, call__121, call__122);
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
            var call__123 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
            call__123.FCastSounds.Play(0);
            FUnexplored.Clear();
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled)
            {
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 0);
                var call__124 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Layer];
                Level = call__124;
                while (true)
                {
                    var call__125 = VisibilityRange();
                    if (!(Walk.Radius <= call__125))
                        break;
                    {
                        if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                        {
                            var call__126 = Level.Field[Walk.X, Walk.Y];
                            var call__127 = ((Aow.Units.AoWE.TAoWMapField)call__126).GetExplored(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            if (!call__127)
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
                    int end__128 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                    for (; I <= end__128; I += 1)
                    {
                        OwnerPlayer = FEnchantment.FPlayerIndex;
                        bool bool__129 = (OwnerPlayer == I);
                        if (!bool__129)
                        {
                            var call__130 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer);
                            var call__131 = Aow.Units.GlobalTargetSpells.GlobalTargetSpells.SpellRelation(call__130.FDiplomaticRelations, unchecked((byte)I));
                            bool__129 = (call__131 == 2);
                        }

                        if (bool__129)
                        {
                            var call__132 = VisibilityRange();
                            var call__133 = Yhx;
                            var call__134 = Xhx;
                            Aow.Units.AoWE.AoWE.AoWHSMap.ExploreArea(call__134, call__133, Lhx, unchecked((sbyte)I), call__132);
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
            bool bool__135 = (unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) != 255);
            if (bool__135)
            {
                bool bool__136 = (FEnchantment.FPlayerIndex == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__136)
                {
                    var call__137 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FEnchantment.FPlayerIndex);
                    var call__138 = Aow.Units.GlobalTargetSpells.GlobalTargetSpells.SpellRelation(call__137.FDiplomaticRelations, unchecked((byte)unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)));
                    bool__136 = (call__138 == 2);
                }

                bool__135 = bool__136;
            }

            bool bool__139 = bool__135;
            if (bool__139)
            {
                var call__140 = Activated();
                bool__139 = call__140;
            }

            if (bool__139)
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
                    var call__141 = Yhx;
                    var call__142 = Xhx;
                    Aow.Units.AoWE.AoWE.AoWHSMap.FogArea(call__142, call__141, Lhx, FUnfogRadius);
                }

                FUnfogRadius = unchecked((byte)Radius);
                if (FUnfogRadius != 0)
                {
                    var call__143 = Yhx;
                    var call__144 = Xhx;
                    Aow.Units.AoWE.AoWE.AoWHSMap.UnFogArea(call__144, call__143, Lhx, FUnfogRadius);
                }
            }
        }

        // GlobalTargetSpells.pas:1376
        // { draft }
        public int VisibilityRange()
        {
            int Result = default;
            Result = 8;
            bool bool__145 = (Lhx != 0);
            if (!bool__145)
            {
                bool bool__146 = (FEnchantment.FPlayerIndex > 0);
                if (bool__146)
                {
                    var call__147 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FEnchantment.FPlayerIndex);
                    bool__146 = call__147.FMagicControl.FEnemyEnchantments.Contains(Aow.Units.AoWE.TPlayerEnchantmentType.petWaterMastery);
                }

                bool__145 = bool__146;
            }

            if (bool__145)
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
            var call__148 = FEnchantment.VisibleForPlayer(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Visible = call__148;
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
            public override int ClassID()
            {
                int Result = default;
                Result = 132989;
                return Result;
            }

            // GlobalTargetSpells.pas:172
            public new Aow.Units.GlobalTargetSpells.TWatcherHS Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TWatcherHS o = (Aow.Units.GlobalTargetSpells.TWatcherHS)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TWatcherHS => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { draft }
    // { TWatcherTE (instance size 36, VMT 5579D92C) }
    public partial class TWatcherTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1479
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.GlobalTargetSpells.TWatcherHS Watcher = default;
            base.Process(A1, A2);
            PlayDefaultEffects(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer));
            Watcher = Aow.Units.GlobalTargetSpells.TWatcherHS.Create(null);
            Watcher.FEnchantment.FPlayerIndex = unchecked((sbyte)A1.FPlayer);
            var call__149 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
            Watcher.FEnchantment.FEnchantmentManaUpkeep = call__149.FManaUpkeep;
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Watcher, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
            if (unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) == FEventPlayerIndex)
            {
                Watcher.StartAnimation(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer));
            }

            var call__150 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__151 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__151.CastingDone(A1, call__150.FBaseManaCost);
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
            public override int ClassID()
            {
                int Result = default;
                Result = 135963;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TWatcherTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.TWatcherTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TWatcherTE o = (Aow.Units.GlobalTargetSpells.TWatcherTE)NewInstance();
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
        }
    }

    // { TWatcher (instance size 60, VMT 5579DA04) }
    public partial class TWatcher : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1498
        // { needs: TPlayer.GetBusy: Boolean, tested at $557A0266. }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__152 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WatcherRStr);
            this.FSpellName = call__152;
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
            var call__153 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__154 = call__153.GetBusy();
            if (!call__154)
            {
                Event = Aow.Units.GlobalTargetSpells.TWatcherTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
                var call__155 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__155.AddEvent(Event);
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

        // GlobalTargetSpells.pas:201
        public static new Aow.Units.GlobalTargetSpells.TWatcher Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TWatcher o = new Aow.Units.GlobalTargetSpells.TWatcher();
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
    }

    // { TTornadoTE (instance size 48, VMT 5579DB38): $24 angle,
//     $28 animation, $2C selected army identity. }
    public partial class TTornadoTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
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
                var call__156 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__157 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__157.CastingDone(A1, call__156.FBaseManaCost);
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                bool bool__158 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__158)
                {
                    var call__159 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                    bool__158 = call__159;
                }

                if (bool__158)
                {
                    var call__160 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                    call__160.FCastSounds.Play(0);
                }

                Anim = (Aow.Units.Animation.TImageSequenceAnimation)Aow.Units.Animation.TAnimation.Create(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                Anim.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
            }

            if (A2 == 80)
            {
                var call__161 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
                MapF = (Aow.Units.AoWE.TAoWMapField)call__161;
                bool bool__162 = (FArmyID != null);
                if (bool__162)
                {
                    var call__163 = MapF.GetMagicShieldActive();
                    bool__162 = !call__163;
                }

                if (bool__162)
                {
                    FArmyID.RetrieveArmy(ref Army, ref Selection);
                    bool bool__164 = TSet<byte>.WithBounds(0, 255).With(1).Contains((byte)MapF.OverlayType);
                    if (!bool__164)
                    {
                        var call__165 = MapF.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                        bool__164 = (call__165 != null);
                    }

                    Exposed = !bool__164;
                    {
                        I = 0;
                        int end__166 = Army.FArmy.Count - 1;
                        for (; I <= end__166; I += 1)
                        {
                            if ((Selection & (1 << I)) != 0)
                            {
                                var call__167 = Army.FArmy.GetUnit(I);
                                UnitObj = call__167;
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

                                var call__168 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                                if (Chance < call__168)
                                {
                                    Selection = unchecked((byte)(Selection & ~(1 << I)));
                                }
                            }
                        }
                    }

                    var call__169 = Army.FArmy.Transporter(255);
                    if (call__169 != null)
                    {
                        var call__170 = Army.FArmy.Transporter(unchecked((byte)Selection));
                        if (call__170 != null)
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
                        var call__171 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TornadoRStr);
                        var call__172 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XCastRStr);
                        var call__173 = Aow.Rtl.Rtl.Format(call__172, [call__171]);
                        Log.FArmyText = call__173;
                        var call__174 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TornadoRStr);
                        Log.FArmyMessages.Add(call__174);
                        Log.FArmyIcon = 2;
                        Log.FArmyView.Setup(Army.FArmy, unchecked((byte)Selection));
                        var call__175 = Army.Yhx;
                        var call__176 = Army.Xhx;
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FEventPlayerIndex), unchecked((sbyte)call__176), unchecked((sbyte)call__175), unchecked((sbyte)Army.Lhx), true);
                        Log.Release();
                        var call__177 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(68);
                        var call__178 = ((Aow.Units.GlobalTargetSpells.TTornado)call__177).CalculateWarpLocation(Army, unchecked((byte)Selection));
                        Location.XYL = call__178;
                        if (Location.XYL != 16777215)
                        {
                            bool bool__179 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            if (!bool__179)
                            {
                                var call__180 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                                bool__179 = call__180;
                            }

                            if (bool__179)
                            {
                                var call__181 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                call__181.FCastSounds.Play(1);
                            }

                            bool bool__182 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
                            if (!bool__182)
                            {
                                var call__183 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                bool__182 = call__183;
                            }

                            if (bool__182)
                            {
                                var call__184 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                var call__185 = call__184.FCastImages.ImageSequences[1];
                                if (call__185 != null)
                                {
                                    Anim = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                                    var call__186 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                    var call__187 = call__186.FCastImages.ImageSequences[1];
                                    Anim.SetImageSequence(call__187);
                                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer);
                                }
                            }

                            bool bool__188 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
                            if (!bool__188)
                            {
                                var call__189 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(Location.X, Location.Y, Location.L, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                bool__188 = call__189;
                            }

                            if (bool__188)
                            {
                                var call__190 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                var call__191 = call__190.FCastImages.ImageSequences[1];
                                if (call__191 != null)
                                {
                                    Anim = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                                    var call__192 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                                    var call__193 = call__192.FCastImages.ImageSequences[1];
                                    Anim.SetImageSequence(call__193);
                                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, Location.X, Location.Y, Location.L);
                                }
                            }

                            MovedUnits = Aow.Units.AoWE.TUnitUserList.Create(null);
                            Owner = unchecked((byte)Army.FArmy.FPlayer);
                            var call__194 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(Location.XYL);
                            MapF = (Aow.Units.AoWE.TAoWMapField)call__194;
                            var call__195 = Army.FArmy.ValidateSelection(255);
                            if (call__195 != Selection)
                            {
                                {
                                    I = Army.FArmy.Count - 1;
                                    byte end__196 = 0;
                                    for (; I >= end__196; I -= 1)
                                    {
                                        if ((Selection & (1 << I)) != 0)
                                        {
                                            var call__197 = Army.FArmy.GetUnit(I);
                                            UnitObj = call__197;
                                            MovedUnits.AddUnit(UnitObj);
                                            var call__198 = UnitObj.Place(Location.X, Location.Y, Location.L, unchecked((sbyte)unchecked((byte)Army.FArmy.FPlayer)), TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of());
                                            Placed = call__198;
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
                                var call__199 = Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(MovedUnits, 5);
                                var call__200 = Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.CreateGroup(0, unchecked((sbyte)call__199), Aow.Units.AoWE.TGroupBehavior.gbHunt);
                                Group = call__200;
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
                    byte end__201 = 200;
                    for (; I <= end__201; I += 1)
                    {
                        Direction = Aow.Rtl.Rtl.Random(360);
                        if ((Animation.FFrameCounter >= ((int)(((uint)I) >> 2))) && (Animation.FFrameCounter <= (((int)(((uint)I) >> 4)) + 68)))
                        {
                            Radius = Aow.Rtl.Rtl.Round((2.997 - System.Math.Log(((I * 19) * 0.005) + 1)) * 35) + 7;
                            Height = (200 - I) - 16;
                            var call__202 = System.Math.Sin((FAnimation.FFrameCounter * 0.1) + (I * 0.02));
                            XShift = Aow.Rtl.Rtl.Round((call__202 * (200 - I)) * 0.25) + 16;
                            var call__203 = System.Math.PI;
                            Angle = (float)(((double)((Direction + FAngle) * call__203)) / 180);
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
                                byte end__204 = 0;
                                for (; Count >= end__204; Count -= 1)
                                {
                                    X = Aow.Rtl.Rtl.Round(System.Math.Cos(Angle) * Radius) + XShift;
                                    Y = Aow.Rtl.Rtl.Round((System.Math.Sin(Angle) * Radius) * 0.5) + Height;
                                    var call__205 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(68);
                                    call__205.FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                                    Angle = (float)(0.035 + Angle);
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
            public override int ClassID()
            {
                int Result = default;
                Result = 135972;
                return Result;
            }

            // GlobalTargetSpells.pas:215
            public new Aow.Units.GlobalTargetSpells.TTornadoTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TTornadoTE o = (Aow.Units.GlobalTargetSpells.TTornadoTE)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TTornadoTE => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TTornado (instance size 60, VMT 5579DC10) }
    public partial class TTornado : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1798
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__206 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TornadoRStr);
            this.FSpellName = call__206;
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
            var call__207 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__208 = call__207.GetBusy();
            if (!call__208)
            {
                Event = Aow.Units.GlobalTargetSpells.TTornadoTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
                var call__209 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(A2, A3, A4);
                var call__210 = ((Aow.Units.AoWE.TAoWMapField)call__209).GetArmyHS();
                Army = call__210;
                if (Army != null)
                {
                    var call__211 = Army.FArmy.CreateID(255);
                    Event.FArmyID = call__211;
                }

                var call__212 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__212.AddEvent(Event);
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
            var call__213 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, Layer);
            MapF = (Aow.Units.AoWE.TAoWMapField)call__213;
            var call__214 = MapF.GetArmyHS();
            bool bool__215 = (call__214 == null);
            if (bool__215)
            {
                var call__216 = MapF.GetPlayer();
                bool__215 = (call__216 == -1);
            }

            Result = bool__215;
            if (Result)
            {
                Msg.MsgID = 4384;
                Msg.Param[0] = Army;
                Msg.Result = 0;
                MapF.SendMapFieldMsg(ref Msg);
                bool bool__217 = (Msg.Result == 0);
                if (bool__217)
                {
                    var call__218 = Army.FArmy.CanPlace(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer));
                    bool__217 = call__218;
                }

                Result = bool__217;
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
            var call__219 = Army.Yhx;
            var call__220 = Army.Xhx;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__220, call__219, 10);
            var call__221 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Army.Lhx];
            MapLevel = call__221;
            while (Walk.Radius < 15)
            {
                X = Walk.X;
                if ((((X >= 0) && (Walk.Y >= 0)) && (X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                {
                    var call__222 = MapLevel.GetField(X, Walk.Y);
                    MapF = call__222;
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
                var call__223 = Aow.Rtl.Rtl.Random(Locations.Count);
                Index = call__223;
                var call__224 = Locations[Index];
                MapF = (Aow.Units.HSEngine.TMapField)call__224;
                var call__225 = ValidWarpLocation(Army, unchecked((sbyte)MapF.Xhx), unchecked((sbyte)MapF.Yhx), unchecked((sbyte)MapF.Lhx));
                if (call__225)
                {
                    Found = true;
                    var call__226 = MapF.XYL;
                    Result = call__226;
                }
                else
                {
                    Locations.Delete(Index);
                }
            }

            Locations.Free();
            return Result;
        }

        // GlobalTargetSpells.pas:226
        public static new Aow.Units.GlobalTargetSpells.TTornado Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TTornado o = new Aow.Units.GlobalTargetSpells.TTornado();
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
    }

    // { TVortexTE (instance size 48, VMT 5579DD44): $24 angle,
//     $28 animation, $2C selected army identity. }
    public partial class TVortexTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
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
                var call__227 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__228 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__228.CastingDone(A1, call__227.FBaseManaCost);
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                bool bool__229 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__229)
                {
                    var call__230 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                    bool__229 = call__230;
                }

                if (bool__229)
                {
                    var call__231 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                    call__231.FCastSounds.Play(0);
                }

                Anim = Aow.Units.Animation.TAnimation.Create(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                Anim.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
            }

            if (A2 == 80)
            {
                bool bool__232 = (FArmyID != null);
                if (bool__232)
                {
                    var call__233 = FArmyID.RetrieveArmy(ref Army, ref Selection);
                    bool__232 = call__233;
                }

                if (bool__232)
                {
                    Log = Aow.Units.AoWE.TArmyEventLog.Create(null);
                    var call__234 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VortexDamageRStr);
                    Log.FArmyText = call__234;
                    Log.FPlayer = unchecked((sbyte)A1.FPlayer);
                    var call__235 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VortexRStr);
                    Log.FArmyMessages.Add(call__235);
                    Log.FArmyIcon = 2;
                    Log.FArmyView.Setup(Army.FArmy, 255);
                    Army.FArmy.BeginUpdate();
                    Damaged = false;
                    {
                        I = 0;
                        int end__236 = Army.FArmy.Count - 1;
                        for (; I <= end__236; I += 1)
                        {
                            var call__237 = Army.FArmy.GetUnit(I);
                            UnitObj = call__237;
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
                                    var call__238 = UnitObj.GetHitPoints();
                                    UnitObj.SetHitPoints(unchecked((sbyte)(call__238 - Damage)));
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
                        var call__239 = Army.Yhx;
                        var call__240 = Army.Xhx;
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)Army.FArmy.FPlayer), unchecked((sbyte)call__240), unchecked((sbyte)call__239), unchecked((sbyte)Army.Lhx), true);
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
                    byte end__241 = 200;
                    for (; I <= end__241; I += 1)
                    {
                        Direction = Aow.Rtl.Rtl.Random(360);
                        if ((Animation.FFrameCounter >= ((int)(((uint)(200 - I)) >> 2))) && (Animation.FFrameCounter <= (((int)(((uint)I) >> 4)) + 68)))
                        {
                            Radius = Aow.Rtl.Rtl.Round((2.997 - System.Math.Log(((I * 19) * 0.005) + 1)) * 35) + 18;
                            Height = -16;
                            XShift = 16;
                            var call__242 = System.Math.PI;
                            Angle = (float)(((double)(-(Direction + FAngle) * call__242)) / 180);
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
                                byte end__243 = 0;
                                for (; J >= end__243; J -= 1)
                                {
                                    X = Aow.Rtl.Rtl.Round(System.Math.Cos(Angle) * Radius) + XShift;
                                    Y = Aow.Rtl.Rtl.Round(((double)(System.Math.Sin(Angle) * Radius)) / 1.75) + Height;
                                    var call__244 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(70);
                                    call__244.FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                                    Angle = (float)(0.035 + Angle);
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
            public override int ClassID()
            {
                int Result = default;
                Result = 135973;
                return Result;
            }

            // GlobalTargetSpells.pas:242
            public new Aow.Units.GlobalTargetSpells.TVortexTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.TVortexTE o = (Aow.Units.GlobalTargetSpells.TVortexTE)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.TVortexTE => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TVortex (instance size 60, VMT 5579DE1C) }
    public partial class TVortex : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2069
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__245 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VortexRStr);
            this.FSpellName = call__245;
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
            var call__246 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__247 = call__246.GetBusy();
            if (!call__247)
            {
                Event = Aow.Units.GlobalTargetSpells.TVortexTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
                var call__248 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(A2, A3, A4);
                var call__249 = ((Aow.Units.AoWE.TAoWMapField)call__248).GetArmyHS();
                Army = call__249;
                if (Army != null)
                {
                    var call__250 = Army.FArmy.CreateID(255);
                    Event.FArmyID = call__250;
                }

                var call__251 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__251.AddEvent(Event);
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
                var call__252 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[A1.FTargetL];
                var call__253 = call__252.GetField(A1.FTargetX, A1.FTargetY);
                MapF = call__253;
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
                var call__254 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Layer];
                MapLevel = call__254;
                var call__255 = MapLevel.GetField(X, Y);
                Valid = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)call__255.TerrainType);
            }
            else
            {
                Valid = false;
            }
        }

        // GlobalTargetSpells.pas:253
        public static new Aow.Units.GlobalTargetSpells.TVortex Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TVortex o = new Aow.Units.GlobalTargetSpells.TVortex();
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
    }

    // { THealingShowersTE (instance size 40, VMT 5579DF48): $24 animation. }
    public partial class THealingShowersTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
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
                bool bool__256 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__256)
                {
                    var call__257 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer), 2);
                    bool__256 = call__257;
                }

                if (bool__256)
                {
                    var call__258 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(73);
                    call__258.FCastSounds.Play(0);
                }

                Anim = (Aow.Units.Animation.TImageSequenceAnimation)Aow.Units.Animation.TAnimation.Create(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                Anim.FTag = (Aow.Units.AoWE.AoWE.AoWHSMap.ViewWidth & 65535) | (Aow.Units.AoWE.AoWE.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
                var call__259 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__259.CastingDone(A1, FEventMana);
            }

            if (A2 == 50)
            {
                PlaySound = false;
                Aow.Units.AoWE.AoWE.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, null);
                var call__260 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Layer];
                Level = call__260;
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref FLocation).Y, 0);
                while (Walk.Radius <= 1)
                {
                    if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Level.Height > Walk.Y))
                    {
                        var call__261 = Level.GetField(Walk.X, Walk.Y);
                        var call__262 = ((Aow.Units.AoWE.TAoWMapField)call__261).GetMagicShieldActive();
                        if (!call__262)
                        {
                            Healed = false;
                            var call__263 = Level.GetField(Walk.X, Walk.Y);
                            var call__264 = ((Aow.Units.AoWE.TAoWMapField)call__263).GetArmyHS();
                            Army = call__264;
                            if (Army != null)
                            {
                                {
                                    I = 0;
                                    int end__265 = Army.FArmy.Count - 1;
                                    for (; I <= end__265; I += 1)
                                    {
                                        var call__266 = Army.FArmy.GetUnit(I);
                                        UnitObj = call__266;
                                        var call__267 = UnitObj.GetHitPoints();
                                        var call__268 = UnitObj.GetHits();
                                        if (call__267 < call__268)
                                        {
                                            var call__269 = UnitObj.GetHitPoints();
                                            UnitObj.SetHitPoints(unchecked((sbyte)(call__269 + 5)));
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
                                bool bool__270 = Healed;
                                if (bool__270)
                                {
                                    var call__271 = Army.Yhx;
                                    var call__272 = Army.Xhx;
                                    var call__273 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(call__272, call__271, Army.Lhx, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                    bool__270 = call__273;
                                }

                                if (bool__270)
                                {
                                    bool bool__274 = PlaySound || (unchecked((byte)Army.FArmy.FPlayer) == unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                    if (!bool__274)
                                    {
                                        var call__275 = Army.Yhx;
                                        var call__276 = Army.Xhx;
                                        var call__277 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__276), unchecked((sbyte)call__275), unchecked((sbyte)Army.Lhx), 1);
                                        bool__274 = call__277;
                                    }

                                    PlaySound = bool__274;
                                    Anim = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                                    var call__278 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(47);
                                    var call__279 = call__278.FImageSequences.ImageSequences[0];
                                    Anim.SetImageSequence(call__279);
                                    Anim.FAnimationLevel = 27;
                                    var call__280 = Army.Yhx;
                                    var call__281 = Army.Xhx;
                                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, call__281, call__280, Army.Lhx);
                                }
                            }

                            var call__282 = Level.GetField(Walk.X, Walk.Y);
                            var call__283 = call__282.FindOwnedChild(132990);
                            Cloud = (Aow.Units.HSEngine.THexagonSprite)call__283;
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
                    var call__284 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(47);
                    call__284.FSFXLibrary.Play(0);
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
                var call__285 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetDetailLevel();
                switch (call__285)
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
                    int end__286 = Count;
                    for (; I <= end__286; I += 1)
                    {
                        X = Aow.Rtl.Rtl.Random(100) - 50;
                        Y = Aow.Rtl.Rtl.Random(100) - 50;
                        Speed = Aow.Rtl.Rtl.Random(5) + 5;
                        var call__287 = Aow.Rtl.Rtl.Random(30);
                        Frame = Animation.FFrameCounter - call__287;
                        if ((Frame >= 0) && (Frame < 50))
                        {
                            Frame = Frame % 30;
                            X -= Aow.Rtl.Rtl.Round((Frame * 4) * (Speed * 0.2));
                            Y += Aow.Rtl.Rtl.Round((Frame * 20) * (Speed * 0.2));
                            if (Y < 500)
                            {
                                {
                                    J = 0;
                                    byte end__288 = 10;
                                    for (; J <= end__288; J += 1)
                                    {
                                        XShift = X - Aow.Rtl.Rtl.Round((J * 2) * 0.1);
                                        YShift = Y + Aow.Rtl.Rtl.Round((J * 10) * 0.1);
                                        var call__289 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(73);
                                        call__289.FCastImages.Show(0, (Xhp + XShift) + 120, ((Yhp + YShift) - 500) + 30, 0, SurfaceDesc);
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
            public override int ClassID()
            {
                int Result = default;
                Result = 135975;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalTargetSpells.THealingShowersTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalTargetSpells.THealingShowersTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalTargetSpells.THealingShowersTE o = (Aow.Units.GlobalTargetSpells.THealingShowersTE)NewInstance();
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
        }
    }

    // { THealingShowers (instance size 60, VMT 5579E030) }
    public partial class THealingShowers : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2277
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__290 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HealingShowersRStr);
            this.FSpellName = call__290;
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
                int end__291 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__291; I += 1)
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
            var call__292 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__292.AddEvent(Event);
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

        // GlobalTargetSpells.pas:274
        public static new Aow.Units.GlobalTargetSpells.THealingShowers Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.THealingShowers o = new Aow.Units.GlobalTargetSpells.THealingShowers();
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
    }

    // { TSmokeCloudSpell (instance size 60, VMT 5579E16C) }
    public partial class TSmokeCloudSpell : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2325
        // { needs: TFireControl.PlaceFire(X,Y,Layer:Integer; Create:Boolean): TFire,
//   result consumed at $5579EDC3; GetSource returns TAbstractUnit at $5579EDE2. }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__293 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SmokeCloudRStr);
            this.FSpellName = call__293;
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
            var call__294 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).GetSource();
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)call__294.FPlayer), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Layer), 2);
            var call__295 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).GetSource();
            bool bool__296 = (call__295.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__296)
            {
                var call__297 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).X), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Y), unchecked((sbyte)Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Layer), 2);
                bool__296 = call__297;
            }

            if (bool__296)
            {
                var call__298 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FEventSpellID);
                call__298.FCastSounds.Play(0);
            }

            var call__299 = ((Aow.Units.HSEngine.TMapObjectResourceList)((Aow.Rtl.TObject)Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList)).FindChild(135747);
            Resource = (Aow.Units.Engine.TEResource)call__299;
            if (Resource != null)
            {
                Cloud = (Aow.Units.SmokeCloud.TSmokeCloud)Resource.CreateResourceUser();
                Cloud.FPlayer = unchecked((sbyte)A2.FPlayer);
                Cloud.FTurnsRemaining = -1;
                ((Aow.Units.AoWE.TGlobalEnchantment)Cloud.FEnchantment).FPlayerIndex = unchecked((sbyte)A2.FPlayer);
                ((Aow.Units.AoWE.TGlobalEnchantment)Cloud.FEnchantment).FEnchantmentManaUpkeep = this.FManaUpkeep;
                Cloud.FDrifting = false;
                var call__300 = Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Cloud, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).X, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Y, Unsafe.As<Aow.Units.HSEngine.TXYL, Aow.Units.GlobalTargetSpells.TSpellLocation>(ref ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE)A1).FLocation).Layer);
                if (!call__300)
                {
                    Cloud.Free();
                }

                var call__301 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(this.FSpellID);
                call__301.CastingDone(A2, this.FBaseManaCost);
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
                int end__302 = Aow.Units.HSEngine.HSEngine.RadToHN(4) - 1;
                for (; I <= end__302; I += 1)
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
            Event = Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Create(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__303 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__303.AddEvent(Event);
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

        // GlobalTargetSpells.pas:284
        public static new Aow.Units.GlobalTargetSpells.TSmokeCloudSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalTargetSpells.TSmokeCloudSpell o = new Aow.Units.GlobalTargetSpells.TSmokeCloudSpell();
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
            ((Aow.Units.Engine.TEngine)A1).RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TFreezeWaterTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWE.TFrozenWaterHS>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TFireBarrierTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TLevelTerrainTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TRejuvenateTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TWatcherTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TWatcherHS>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TTornadoTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TVortexTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.THealingShowersTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE>()]);
            var call__304 = Aow.Units.GlobalTargetSpells.TFreezeWater.Create(null);
            Control.RegisterSpell(call__304);
            var call__305 = Aow.Units.GlobalTargetSpells.TFireBarrier.Create(null);
            Control.RegisterSpell(call__305);
            var call__306 = Aow.Units.GlobalTargetSpells.TLevelTerrain.Create(null);
            Control.RegisterSpell(call__306);
            var call__307 = Aow.Units.GlobalTargetSpells.TRejuvenate.Create(null);
            Control.RegisterSpell(call__307);
            var call__308 = Aow.Units.GlobalTargetSpells.TWatcher.Create(null);
            Control.RegisterSpell(call__308);
            var call__309 = Aow.Units.GlobalTargetSpells.TTornado.Create(null);
            Control.RegisterSpell(call__309);
            var call__310 = Aow.Units.GlobalTargetSpells.TVortex.Create(null);
            Control.RegisterSpell(call__310);
            var call__311 = Aow.Units.GlobalTargetSpells.THealingShowers.Create(null);
            Control.RegisterSpell(call__311);
            var call__312 = Aow.Units.GlobalTargetSpells.TSmokeCloudSpell.Create(null);
            Control.RegisterSpell(call__312);
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
            Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.Engine.TEngine>((int)A1).UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TFreezeWaterTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWE.TFrozenWaterHS>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TFireBarrierTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TLevelTerrainTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TRejuvenateTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TWatcherTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TWatcherHS>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TTornadoTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TVortexTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.THealingShowersTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE>()]);
        }

        // { Import cells bind to the shared globals; distinct references preserve the original reloads. }
        public static System.Type SpellStructureClass = typeof(Aow.Units.AoWE.TStructure);
        // { needs: AoWE.CancelRStr, referenced at $5579E664; absent from draft. }
        public static readonly Aow.Rtl.AnsiString SpellCancelRStr = (AnsiString)"Cancel";
        // GlobalTargetSpells.pas:396
        // { needs: TSpellControl.GetSpell must return TSpell. Preserve its register ABI
//   with a tail call until the shared declaration is corrected. }
        // { needs: TAoWMapField.GetExplored(Player: Integer): Boolean ($5579E4C8). }
        // { needs: TUnitControl.FindUnit(ID: Integer): TAbstractUnit ($5579E4F7). }
        // { needs: AoWE.TranslateRStr(const S: string): string ($5579E62D). }
        // { needs: TSpell.CanActivate(Caster: TAbstractUnit; var Msg: string): Boolean. }
        public static bool BaseCanActivate(Aow.Units.AoWE.TSpell Spell, int Caster, int MessagePtr)
        {
            bool Result = default;
            Result = Spell.CanActivate__static(Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>(Caster), ref Unsafe.As<int, Aow.Rtl.AnsiString>(ref MessagePtr));
            return Result;
        }

        // GlobalTargetSpells.pas:552
        // { needs: Boolean result; stack arguments require the normal EBP frame. }
        // { needs: Boolean results on the shared TSpell virtuals. Helpers keep managed
//   string finalization before returning AL; the public methods tail-call them. }
        public static bool GlobalTargetCanActivate(Aow.Units.GlobalTargetSpells.TGlobalTargetSpell Spell, int Caster, int MessagePtr)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Result = Aow.Units.GlobalTargetSpells.GlobalTargetSpells.BaseCanActivate(Spell, Caster, MessagePtr);
            if (Result && Spell.FAboveGroundOnly)
            {
                Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>((int)Caster).GetLocation(ref X, ref Y, ref Layer);
                if (Layer != 0)
                {
                    Result = false;
                    /* TODO(T-A3) Integer address requires managed pointer provenance */
                    throw new System.NotImplementedException();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:585
        public static bool VirtualCanActivate(Aow.Units.AoWE.TSpell Spell, int Caster, int MessagePtr)
        {
            bool Result = default;
            Result = Spell.CanActivate(Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>(Caster), ref Unsafe.As<int, Aow.Rtl.AnsiString>(ref MessagePtr));
            return Result;
        }

        // GlobalTargetSpells.pas:596
        public static void BaseActivate(Aow.Units.AoWE.TSpell Spell, int Caster, int A2)
        {
            Spell.Activate__static(Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TAbstractUnit>(Caster));
        }

        // GlobalTargetSpells.pas:601
        public static bool GlobalTargetActivate(Aow.Units.GlobalTargetSpells.TGlobalTargetSpell Spell, int Caster, int A2)
        {
            bool Result = default;
            Aow.Rtl.AnsiString MessageText = default;
            Aow.Units.GlobalTargetSpells.GlobalTargetSpells.BaseActivate(Spell, Caster, A2);
            /* TODO(T-4.3) Address needs a cell or array backing */
            throw new System.NotImplementedException();
            return Result;
        }

        // GlobalTargetSpells.pas:1185
        // { needs: TGlobalEnchantment.Deactivate returns a value; P7 overrides retain
//     the procedure draft, so preserve the original ABI and conditional Free. }
        // { needs: typed result declarations in the AoWE interface and matching
//   implementation headers. Register ABI shims keep those owners untouched. }
        public static byte SpellRelation(Aow.Units.AoWE.TPlayerDiplomaticRelations Relations, byte Player)
        {
            byte Result = default;
            Result = unchecked((byte)Relations.GetDiplomaticRelation(unchecked((sbyte)Player)));
            return Result;
        }

        // GlobalTargetSpells.pas:1188
        public static bool SpellEnchantmentVisible(Aow.Units.AoWE.TGlobalEnchantment Enchantment, byte Player)
        {
            bool Result = default;
            Result = Enchantment.VisibleForPlayer(unchecked((sbyte)Player));
            return Result;
        }

        public static readonly Aow.Rtl.AnsiString SpellXCastRStr = (AnsiString)"%s cast";
        // GlobalTargetSpells.pas:1644
        public static byte SpellUnitLevel(Aow.Units.AoWE.TAbstractUnit UnitObj)
        {
            byte Result = default;
            Result = unchecked((byte)UnitObj.GetUnitLevel());
            return Result;
        }

        // GlobalTargetSpells.pas:1647
        public static byte SpellIndependentRelation(Aow.Units.AoWE.TUnitList Units, byte Default)
        {
            byte Result = default;
            Result = unchecked((byte)Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(Units, unchecked((sbyte)Default)));
            return Result;
        }

        // GlobalTargetSpells.pas:1650
        public static Aow.Units.AoWE.TAIGroup SpellCreateGroup(Aow.Units.AoWE.TAIGroupManager Manager, byte Player, byte Relation, int Behavior)
        {
            Aow.Units.AoWE.TAIGroup Result = default;
            Result = Manager.CreateGroup(unchecked((sbyte)Player), unchecked((sbyte)Relation), (Aow.Units.AoWE.TGroupBehavior)Behavior);
            return Result;
        }

        // GlobalTargetSpells.pas:1985
        public static byte SpellMoveTypes(Aow.Units.AoWE.TAbstractUnit UnitObj)
        {
            byte Result = default;
            Result = (byte)UnitObj.GetMoveTypes().Image;
            return Result;
        }

        // GlobalTargetSpells.pas:1988
        public static void SpellValidateTerrainObservation(Aow.Units.AoWE.TArmy Army, byte Terrain, byte Overlay, byte Player)
        {
            Army.ValidateTerrainEx(unchecked((sbyte)Terrain), unchecked((sbyte)Overlay), unchecked((sbyte)Player));
        }

        // GlobalTargetSpells.pas:2135
        public static sbyte SpellHitPointsMax(Aow.Units.AoWE.TAbstractUnit UnitObj)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)UnitObj.GetHits());
            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
