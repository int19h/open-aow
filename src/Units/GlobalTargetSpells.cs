// Translated from GlobalTargetSpells.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Ai.Groups;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.GlobalTarget;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw
{

    public enum TWatcherStateElement : byte
    {
        wsActive = 0
    }
}

namespace Aow.Game.Commands
{

    // FLocation: packed X/Y/layer coordinates
    public partial class TGlobalTargetSpellTE : TSpellTE
    {
        public TGlobalTargetSpellTE(TECustomNode owner) : base(owner)
        { }

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
            TImageSequenceAnimation Animation;
            if (Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
            {
                Animation = new TImageSequenceAnimation(null);
                Animation.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Animation, X, Y, Layer);
            }

            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    // FCasterID, FCasterPlayer, FAboveGroundOnly CanActivate/Activate and the target predicates
    // return Boolean. Procedure declarations retained for shared TSpell and P7 overrides.
    // ShowTargetSelection variants: reordered locals, split/inline radius, target with-scope, and
    // inline map-field lookup. SetupTargetSelection variants: explicit class local, repeated casts,
    // class with-scope, record fields, record with-scope and reversed coordinate local order
    public partial class TGlobalTargetSpell : Aow.Game.Magic.Spells.TSpell
    {

        public int FCasterID;
        public sbyte FCasterPlayer;
        public bool FAboveGroundOnly;

        // GlobalTargetSpells.pas:439
        public TGlobalTargetSpell(TECustomNode owner) : base(owner)
        {
            FSpellType = Aow.Game.Magic.Spells.TSpellType.stGlobal;
        }

        // GlobalTargetSpells.pas:569
        public override bool CanActivate(TAbstractUnit Source, ref string MessageText)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            bool Result = base.CanActivate(Source, ref MessageText);
            if (Result && FAboveGroundOnly)
            {
                Source.GetLocation(ref X, ref Y, ref Layer);
                if (Layer != 0)
                {
                    Result = false;
                    MessageText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:618
        public override bool Activate(TAbstractUnit Source)
        {
            bool Result;
            string MessageText = "";
            base.Activate(Source);
            if (CanActivate(Source, ref MessageText))
            {
                FCasterID = Source.FUnitID;
                FCasterPlayer = Source.FPlayer;
                SetupTargetSelection(Source, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTargetSelection);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTargetSelection.Activate();
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
            TMapLevel Level;
            TAoWMapField MapF;
            int X = default;
            int Y = default;
            int Intensity;
            int Radius;
            Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[Target.FTargetL];
            Aow.Game.World.Map.HexGeometry.CenterHNtoHX(Target.FTargetX, Target.FTargetY, HN, ref X, ref Y);
            // { Original accepts Width/Height themselves (strict > bounds test). }
            if (X >= 0 && Y >= 0 && X <= Level.Width && Y <= Level.Height)
            {
                MapF = (TAoWMapField)Level.Field[X, Y];
                if (this.ValidTargetSelectionMapF((TMapField)(object)MapF))
                {
                    Radius = Aow.Game.World.Map.HexGeometry.HNtoRad(HN);
                    Intensity = Math.Abs((Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame + Radius) % 9 - 4) * 20 + 20;
                    Target.FImageLibrary[0].ShowBlended(Xs - 8, Ys, SurfaceDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[Intensity]);
                }
                else
                {
                    Target.FImageLibrary[1].Show(Xs - 8, Ys, SurfaceDesc);
                }
            }
        }

        // GlobalTargetSpells.pas:521
        public virtual void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            sbyte UnitX = default;
            sbyte UnitY = default;
            sbyte UnitLayer = default;
            TargetSelection.Initialize(Source);
            TargetSelection.FTargetMode = TargetSelection.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmOwnerDraw, TTargetSelectionModeElement.tmValidateDistance, TTargetSelectionModeElement.tmExternalFloater);
            Source.GetLocation(ref UnitX, ref UnitY, ref UnitLayer);
            TargetSelection.FSourceX = UnitX;
            TargetSelection.FSourceY = UnitY;
            TargetSelection.FSourceL = UnitLayer;
            TargetSelection.FRange = 12;
            TargetSelection.FOnChange += OnTargetSelectionChange;
            TargetSelection.FOnShow = ShowTargetSelection;
            var format = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.SelectTargetLocationForXOnMapRStr), [FSpellName]);
            TargetSelection.FText.Text = format;
            TargetSelection.FTitle = FSpellName;
            TargetSelection.FCancelText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CancelRStr);
            TargetSelection.FSourceImage = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[20].GetPreviewImage();
            TargetSelection.FValidTargetImage = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[21].GetPreviewImage();
            TargetSelection.FInvalidTargetImage = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[22].GetPreviewImage();
        }

        // GlobalTargetSpells.pas:544
        public virtual bool TargetSelected(TAbstractUnit Source, int X, int Y, int L) => false;

        // GlobalTargetSpells.pas:487
        public virtual bool ValidTargetSelection(TTargetSelectionControl TargetSelection) => true;

        // GlobalTargetSpells.pas:475
        public virtual bool ValidTargetSelectionMapF(TMapField Field)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FExplorationEnabled && !((TAoWMapField)Field).GetExplored(FCasterPlayer))
            {
                // Preserve the result for callers.
                return false;
            }

            return true;
        }

        // GlobalTargetSpells.pas:469
        public virtual bool ValidTargetMapF(TMapField Field) => true;

        // GlobalTargetSpells.pas:492. The recovered view aliases this control.
        public virtual void OnTargetSelectionChange(TTargetSelectionControl target)
        {
            var caster = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FUnitControl.FindUnit(FCasterID);
            if (caster is null || target.FChange == TTargetSelectionChange.tscDeactivated)
            {
                target.FOnChange -= OnTargetSelectionChange;
                target.Deactivate();
            }
            else if (target.FChange == TTargetSelectionChange.tscSelected && ValidTargetSelection(target) &&
                TargetSelected(caster, target.FTargetX, target.FTargetY, target.FTargetL))
            {
                target.FOnChange -= OnTargetSelectionChange;
                target.Deactivate();
            }
        }

        internal static bool GlobalTargetCanActivate(TGlobalTargetSpell Spell, TAbstractUnit Caster, ref string MessageText)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            bool Result = Spell.CanActivate__static(Caster, ref MessageText);
            if (Result && Spell.FAboveGroundOnly)
            {
                Caster.GetLocation(ref X, ref Y, ref Layer);
                if (Layer != 0)
                {
                    Result = false;
                    MessageText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        internal static bool GlobalTargetActivate(TGlobalTargetSpell Spell, TAbstractUnit Caster, int Unused)
        {
            bool Result;
            string MessageText = "";
            Spell.Activate__static(Caster);
            if (Spell.CanActivate(Caster, ref MessageText))
            {
                Spell.FCasterID = Caster.FUnitID;
                Spell.FCasterPlayer = Caster.FPlayer;
                Spell.SetupTargetSelection(Caster, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTargetSelection);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

    }
}

namespace Aow.Game.Commands
{

    public partial class TFreezeWaterTE : TGlobalTargetSpellTE
    {
        public TFreezeWaterTE(TECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135939;

        // GlobalTargetSpells.pas:679
        public override bool Process(TAbstractUnit source, int value)
        {
            TTwinkleAnimation Twinkle = default;
            base.Process(source, value);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateTerrainObservation(source.FPlayer, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.TerrainVisible((sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2))
            {
                Twinkle = new TTwinkleAnimation(null);
                Twinkle.Setup(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(10).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmExpand), 100, 64, 50, 30);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Twinkle, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            }

            // Use the map's seated player.
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == source.FPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.WatchingTerrain((sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2))
            {
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(10).FCastSounds.Play(0);
            }

            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, TerrainChanged);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            // The original event result is zero.
            return false;
        }

        // GlobalTargetSpells.pas:641
        public void ChangeTerrain(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            TFrozenWaterHS Ice;
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
                                Ice.FDaysRemaining = (sbyte)((Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(11) - 5) / 5 + 3);
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
            TFrozenWaterHS Ice;
            if (TSet<byte>.WithBounds(0, 255).With((int)TAoWTerrainType.ttIce).With((int)TAoWTerrainType.ttUndergroundIce).Contains((byte)MapF.TerrainType) && !TSet<byte>.WithBounds(0, 255).With((int)TAoWTerrainType.ttIce).With((int)TAoWTerrainType.ttUndergroundIce).Contains((byte)OriginalTerrainType))
            {
                Ice = (TFrozenWaterHS)MapF.FindOwnedHS(typeof(TFrozenWaterHS));
                if (Ice == null)
                {
                    Ice = new TFrozenWaterHS(null);
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                }

                Ice.FDaysRemaining = (sbyte)((Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(11) - 5) / 5 + 3);
                Ice.FCasterPlayer = (sbyte)FEventPlayerIndex;
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TFreezeWater : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:710
        public TFreezeWater(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.GlobalTarget.TFreezeWater.FreezeWaterRStr);
            FSpellID = 10;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msWater;
        }

        // GlobalTargetSpells.pas:811
        // { Original Boolean-return ABI; foreign P7 overrides remain procedures. }
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            TTargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:823
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            TFreezeWaterTE Event;
            Event = new TFreezeWaterTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        public override bool ValidTargetMapF(TMapField field)
        {
            if (!base.ValidTargetMapF(field)) return false;
            sbyte terrain = field.FTerrainType;
            return (terrain == 0 || terrain == 10) && field.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)field).GetArmyHS() == null;
        }

        public override bool ValidTargetSelectionMapF(TMapField field)
        {
            if (!base.ValidTargetSelectionMapF(field)) return false;
            sbyte terrain = field.FTerrainType;
            if ((terrain != 0 && terrain != 10) || field.FindNoneTransparentHS(typeof(TStructure)) != null) return false;
            var army = ((TAoWMapField)field).GetArmyHS();
            return army == null || !army.VisibleForSeatedPlayer();
        }

        private static readonly string FreezeWaterRStr = "Freeze Water";
    }
}

namespace Aow.Game.Commands
{

    public partial class TFireBarrierTE : TGlobalTargetSpellTE
    {
        public TFireBarrierTE(TECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135940;

        // GlobalTargetSpells.pas:873
        public override bool Process(TAbstractUnit source, int value)
        {
            base.Process(source, value);
            CreateFireBarrier();
            var spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            return false;
        }

        // GlobalTargetSpells.pas:842
        public void CreateFireBarrier()
        {
            THNWalkData Walk = default;
            TMapLevel Level;
            TAoWMapField MapF;
            TFire Fire;
            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
            Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer];
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    MapF = (TAoWMapField)Level.Field[Walk.X, Walk.Y];
                    if (((TGlobalTargetSpell)(object)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(11)).ValidTargetMapF((TMapField)(object)MapF) && MapF.GetArmyHS() == null)
                    {
                        Fire = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FFireControl.PlaceFire(Walk.X, Walk.Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, TFireType.ftLarge);
                        if (Fire != null)
                        {
                            Fire.SetupExplosion(Random(35));
                            Fire.FTurnsLeft = 3;
                            Fire.FPlayer = this.GetSource().FPlayer;
                        }
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TFireBarrier : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:882
        public TFireBarrier(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.GlobalTarget.TFireBarrier.FireBarrierRStr);
            FSpellID = 11;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msFire;
        }

        // GlobalTargetSpells.pas:914
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            TTargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:926
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            TFireBarrierTE Event;
            Event = new TFireBarrierTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:900
        public override bool ValidTargetSelectionMapF(TMapField Field)
        {
            TArmyHS Army;
            bool Result = base.ValidTargetSelectionMapF(Field) && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(TStructure)) == null;
            if (Result)
            {
                Army = ((TAoWMapField)Field).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:892
        public override bool ValidTargetMapF(TMapField Field) => base.ValidTargetMapF(Field) && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)Field).GetArmyHS() == null;

        private static readonly string FireBarrierRStr = "Fire Barrier";
    }
}

namespace Aow.Game.Commands
{

    public partial class TLevelTerrainTE : TGlobalTargetSpellTE
    {
        public TLevelTerrainTE(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:940
        public new const int ClassID = 135945;

        // GlobalTargetSpells.pas:968
        public override bool Process(TAbstractUnit source, int value)
        {
            base.Process(source, value);
            PlayDefaultEffects(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            LevelTerrain();
            var spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            return false;
        }

        // GlobalTargetSpells.pas:943
        public void LevelTerrain()
        {
            THNWalkData Walk = default;
            TMapLevel Level;
            TAoWMapField MapF;
            Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer];
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    MapF = (TAoWMapField)Level.Field[Walk.X, Walk.Y];
                    if (((TGlobalTargetSpell)(object)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(18)).ValidTargetMapF((TMapField)(object)MapF) && MapF.GetArmyHS() == null)
                    {
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 0, -1);
                        if (MapF.TerrainType == (int)TAoWTerrainType.ttEarth)
                        {
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ChangeTerrain(null, Walk.X, Walk.Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 0, (int)TAoWTerrainType.ttDirt);
                        }
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TLevelTerrain : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:978
        public TLevelTerrain(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.GlobalTarget.TLevelTerrain.LevelTerrainRStr);
            FSpellID = 18;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msEarth;
        }

        // GlobalTargetSpells.pas:1010
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            TTargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:1022
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            TLevelTerrainTE Event;
            Event = new TLevelTerrainTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:996
        public override bool ValidTargetSelectionMapF(TMapField Field)
        {
            TArmyHS Army;
            bool Result = base.ValidTargetSelectionMapF(Field) && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(TStructure)) == null;
            if (Result)
            {
                Army = ((TAoWMapField)Field).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:988
        public override bool ValidTargetMapF(TMapField Field) => base.ValidTargetMapF(Field) && !TSet<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)Field).GetArmyHS() == null;

        private static readonly string LevelTerrainRStr = "Level Terrain";
    }
}

namespace Aow.Game.Commands
{

    public partial class TRejuvenateTE : TGlobalTargetSpellTE
    {
        public TRejuvenateTE(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:1036
        public new const int ClassID = 135952;

        // GlobalTargetSpells.pas:1044
        public override bool Process(TAbstractUnit source, int value)
        {
            TTwinkleAnimation Twinkle = default;
            base.Process(source, value);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateTerrainObservation(source.FPlayer, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.TerrainVisible((sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2))
            {
                Twinkle = new TTwinkleAnimation(null);
                Twinkle.Setup(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(41).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 100, 64, 50, 30);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Twinkle, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            }

            // Use the map's seated player.
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == source.FPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.WatchingTerrain((sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2))
            {
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(41).FCastSounds.Play(0);
            }

            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2, RejuvenateTerrain, null);
            var spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            // The original event result is zero.
            return false;
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
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TRejuvenate : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1074
        public TRejuvenate(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.GlobalTarget.TRejuvenate.RejuvenateRStr);
            FSpellID = 41;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msLife;
            FAboveGroundOnly = true;
        }

        // GlobalTargetSpells.pas:1095
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            TTargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(3) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:1107
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            TRejuvenateTE Event;
            Event = new TRejuvenateTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:1090
        public override bool ValidTargetSelectionMapF(TMapField Field) => base.ValidTargetSelectionMapF(Field) && TSet<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)Field.TerrainType);

        // GlobalTargetSpells.pas:1085
        public override bool ValidTargetMapF(TMapField Field) => base.ValidTargetMapF(Field) && TSet<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)Field.TerrainType);

        private static readonly string RejuvenateRStr = "Rejuvenate";
    }

    // FWatcher
    public partial class TWatcherEnchantment : Aow.Game.Magic.TGlobalEnchantment
    {
        public TWatcherHS FWatcher;

        // GlobalTargetSpells.pas:1121
        public TWatcherEnchantment(TECustomNode owner) : base(owner)
        {
            FEnchantmentMode = TSet<Aow.Game.Magic.TGlobalEnchantmentModeElement>.FromMask(FEnchantmentMode.Mask) - TSet<Aow.Game.Magic.TGlobalEnchantmentModeElement>.Of(Aow.Game.Magic.TGlobalEnchantmentModeElement.gemTrueGlobal, Aow.Game.Magic.TGlobalEnchantmentModeElement.gemOwned);
            FDispelMana = 50;
        }

        // GlobalTargetSpells.pas:1129
        public override string GetName() => Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(57).FSpellName;

        // The unused fourth byte in the original register result has no defined value.
        public override TXYL GetLocation() => new() { X = FWatcher.GetXhx(), Y = FWatcher.GetYhx(), L = FWatcher.GetLhx() };

        public override bool Deactivate(byte player)
        {
            if ((FEnchantmentStatus.Image & 1) == 0) return false;
            bool result = base.Deactivate(player);
            if ((FWatcher.FWatcherState & 1) != 0) FWatcher?.Dispose();
            return result;
        }
    }
}

namespace Aow.Game.Draw
{

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
        public override void Dispose()
        {
            base.Dispose();
            FUnexplored?.Dispose();
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
            FEnchantment = new TWatcherEnchantment(null);
            FEnchantment.FWatcher = this;
            FUnexplored = new TIntegerList();
            FVisibleForSeatedPlayer = false;
            FAnimationFrame = -1;
        }

        // GlobalTargetSpells.pas:1422
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            int ID;
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
        public override byte GetLevel() => 27;

        // GlobalTargetSpells.pas:1255
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            int DX = default;
            int DY = default;
            int Radius;
            int Angle;
            int PX = default;
            int PY = default;
            int Intensity;
            int Delay;
            bool condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewL == Lhx;
            if (condition)
            {
                var yhx = Yhx;
                var xhx = Xhx;
                condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Visible(xhx, yhx, Lhx, 0);
            }

            if (condition)
            {
                var yhx = Yhx;
                Aow.Game.World.Map.HexGeometry.HXtoHP(Xhx, yhx, ref X, ref Y);
                X = X - XhpSurf + 16;
                Y = Y - YhpSurf + 16;
                if (Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(57).FCastImages != null)
                {
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(57).FCastImages.ShowLooped(1, X, Y, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame, SurfaceDesc);
                }

                if (FAnimationFrame != -1)
                {
                    FAnimationFrame += 1;
                    // The original reads the shared AoWEngine animation detail. The map engine is
                    // not the global read by this routine.
                    if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail == 2 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewL == Lhx)
                    {
                        Intensity = 100 - FAnimationFrame;
                        var yhx2 = Yhx;
                        Aow.Game.World.Map.HexGeometry.HXtoHP(Xhx, yhx2, ref X, ref Y);
                        X = X - XhpSurf - 8;
                        Y = Y - YhpSurf;
                        for (int I = FUnexplored.Count - 1; I >= 0; I--)
                        {
                            Aow.Game.World.Map.HexGeometry.HNtoHP(FUnexplored[I], ref PX, ref PY);
                            PX += X;
                            PY += Y;
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FGeneralImages[5].ShowBlended(PX, PY, SurfaceDesc, Aow.Game.Draw.GfxState.AlphaBlendTable.LookupTables[Intensity]);
                        }
                    }

                    // { Original resets the shared random generator for deterministic particles. }
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FInitialRandomSeed;
                    for (int I = 0; I <= 300; I++)
                    {
                        Radius = Random(VisibilityRange() << 5);
                        Angle = Random(360);
                        Delay = Random(80);
                        if (FAnimationFrame - Delay >= 0)
                        {
                            Aow.Game.Magic.Abilities.AbilityRules.DegreesRadToCo(Angle, Radius, ref DX, ref DY);
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(57).FCastImages.Show(0, X + DX, Y + DY, FAnimationFrame - Delay, SurfaceDesc);
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
            bool Result = base.PlaceOnMap();
            ((TMapField)Owner).UpdateMapField();
            return Result;
        }

        // GlobalTargetSpells.pas:1394
        public override void RemoveFromMap()
        {
            TMapField MapF;
            MapF = (TMapField)Owner;
            base.RemoveFromMap();
            MapF.UpdateMapField();
        }

        // GlobalTargetSpells.pas:1438
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            int OwnerPlayer;
            ref var mapFieldMsg = ref Msg;
            base.MapFieldMsgProc(ref Msg);
            if (mapFieldMsg.MsgID == 4363)
            {
                if (Activated())
                {
                    OwnerPlayer = FEnchantment.FPlayerIndex;
                    if (OwnerPlayer == mapFieldMsg.Param[1].Int || (byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer << 0).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMsg.Param[1].Int) == 2)
                    {
                        var visibilityRange = VisibilityRange();
                        if (mapFieldMsg.Param[0].Int <= visibilityRange)
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
                    if (OwnerPlayer == mapFieldMsg.Param[1].Int || (byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer << 0).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMsg.Param[1].Int) == 2)
                    {
                        var visibilityRange = VisibilityRange();
                        if (mapFieldMsg.Param[0].Int <= visibilityRange)
                        {
                            mapFieldMsg.Result = 1;
                        }
                    }
                }
            }

            if (mapFieldMsg.MsgID == 4365)
            {
                if (VisibilityRange() >= mapFieldMsg.Param[0].Int)
                {
                    // { Param[0] is already available. }
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UpdatePlayerSeen(FEnchantment.FPlayerIndex, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ((TMapField)Owner).Lhx, mapFieldMsg.Param[0].Int, mapFieldMsg.Param[0].Int);
                }

                UpdateVisibleForSeatedPlayer();
            }
        }

        // GlobalTargetSpells.pas:1350
        public override void Activate()
        {
            TMapField MapF;
            base.Activate();
            FWatcherState = (byte)(TSet<TWatcherStateElement>.FromMask(FWatcherState) + TSet<TWatcherStateElement>.Of(TWatcherStateElement.wsActive)).Image;
            FEnchantment.Activate(FEnchantment.FPlayerIndex);
            var mapField = (TMapField)Owner;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.NotifyPlayerChanged(mapField.Xhx, mapField.Yhx, mapField.Lhx);

            MapF = (TMapField)Owner;
            var visibilityRange = VisibilityRange();
            var visibilityRange2 = VisibilityRange();
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UpdatePlayerSeen(FEnchantment.FPlayerIndex, MapF.Xhx, MapF.Yhx, MapF.Lhx, visibilityRange, visibilityRange2);
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

            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.NotifyPlayerChanged(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ((TMapField)Owner).Lhx);
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
            TMapLevel Level;
            FAnimationFrame = 0;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.DynamicFixedVisibleHS.AddChild(this);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(57).FCastSounds.Play(0);
            FUnexplored.Clear();
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FExplorationEnabled)
            {
                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
                Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[Layer];
                while (true)
                {
                    var visibilityRange = VisibilityRange();
                    if (!(Walk.Radius <= visibilityRange))
                        break;
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        if (!((TAoWMapField)Level.Field[Walk.X, Walk.Y]).GetExplored(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer))
                        {
                            FUnexplored.Add(Walk.HN);
                        }
                    }

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }
            }
        }

        // GlobalTargetSpells.pas:1248
        public void StopAnimation()
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.DynamicFixedVisibleHS.RemoveChild(this);
            FAnimationFrame = -1;
            FUnexplored.Clear();
        }

        // GlobalTargetSpells.pas:1305
        public void UpdateExploration()
        {
            int OwnerPlayer;
            if (Activated())
            {
                for (int I = 1, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count - 1; I <= last; I++)
                {
                    OwnerPlayer = FEnchantment.FPlayerIndex;
                    if (OwnerPlayer == I || (byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(OwnerPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)I) == 2)
                    {
                        var visibilityRange = VisibilityRange();
                        var yhx = Yhx;
                        var xhx = Xhx;
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ExploreArea(xhx, yhx, Lhx, (sbyte)I, visibilityRange);
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:1321
        public void UpdateUnfog()
        {
            int Radius;
            if ((byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer != 255 && (FEnchantment.FPlayerIndex == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || (byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FEnchantment.FPlayerIndex).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer) == 2) && Activated())
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
                    var yhx = Yhx;
                    var xhx = Xhx;
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FogArea(xhx, yhx, Lhx, FUnfogRadius);
                }

                FUnfogRadius = (byte)Radius;
                if (FUnfogRadius != 0)
                {
                    var yhx = Yhx;
                    var xhx = Xhx;
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UnFogArea(xhx, yhx, Lhx, FUnfogRadius);
                }
            }
        }

        // GlobalTargetSpells.pas:1376
        public int VisibilityRange()
        {
            int Result = 8;
            if (Lhx != 0 || FEnchantment.FPlayerIndex > 0 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FEnchantment.FPlayerIndex).FMagicControl.FEnemyEnchantments.Contains(TPlayerEnchantmentType.petWaterMastery))
            {
                Result += 1;
                Result = (int)(((uint)Result) >> 1);
            }

            return Result;
        }

        // GlobalTargetSpells.pas:1410
        public void UpdateVisibleForSeatedPlayer()
        {
            bool Visible;
            Visible = FEnchantment.VisibleForPlayer(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
            if (FVisibleForSeatedPlayer ^ Visible)
            {
                FVisibleForSeatedPlayer = Visible;
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
            }
        }

        // GlobalTargetSpells.pas:172

    }
}

namespace Aow.Game.Commands
{

    public partial class TWatcherTE : TGlobalTargetSpellTE
    {
        public TWatcherTE(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:1476
        public new const int ClassID = 135963;

        // GlobalTargetSpells.pas:1479
        public override bool Process(TAbstractUnit source, int value)
        {
            TWatcherHS Watcher;
            base.Process(source, value);
            PlayDefaultEffects(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            Watcher = new TWatcherHS(null);
            Watcher.FEnchantment.FPlayerIndex = source.FPlayer;
            var spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(57);
            Watcher.FEnchantment.FEnchantmentManaUpkeep = spell.FManaUpkeep;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Watcher, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            if ((byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == FEventPlayerIndex)
            {
                Watcher.StartAnimation(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
            }

            var fSpellControlSpell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, fSpellControlSpell.FBaseManaCost);
            return false;
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TWatcher : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1498
        public TWatcher(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.GlobalTarget.TWatcher.WatcherRStr);
            FSpellID = 57;
            FAboveGroundOnly = false;
        }

        // GlobalTargetSpells.pas:1523
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FTargetMode = TSet<TTargetSelectionModeElement>.FromMask(TargetSelection.FTargetMode.Mask) - TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmFixedLevel, TTargetSelectionModeElement.tmValidateDistance);
            TargetSelection.FHNs.Clear();
        }

        // GlobalTargetSpells.pas:1506
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result;
            TWatcherTE Event;
            if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new TWatcherTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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

        private static readonly string WatcherRStr = "Watcher";
    }
}

namespace Aow.Game.Commands
{

    // angle, animation, selected army identity
    public partial class TTornadoTE : TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1559
        public new const int ClassID = 135972;

        public int FAngle;
        public TAnimation FAnimation;
        public TArmyID FArmyID;

        // GlobalTargetSpells.pas:1553
        public override void Dispose()
        {
            FArmyID?.Dispose();
            base.Dispose();
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
        public override bool Process(TAbstractUnit source, int value)
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
            TArmyEventLog Log = default;
            TImageSequenceAnimation Anim = default;
            TArmyHS Placed = default;
            TAIGroup Group = default;
            if (value == 0)
            {
                FAngle = 0;
                var spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateObservation(source.FPlayer, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                if (source.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2))
                {
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
                }

                Anim = (TImageSequenceAnimation)new TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
                Anim.FTag = (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewWidth & 65535) | (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
            }

            if (value == 80)
            {
                MapF = (TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
                if (FArmyID != null && !MapF.GetMagicShieldActive())
                {
                    FArmyID.RetrieveArmy(ref Army, ref Selection);
                    Exposed = !(TSet<byte>.WithBounds(0, 255).With(1).Contains((byte)MapF.OverlayType) || MapF.FindNoneTransparentHS(typeof(TStructure)) != null);
                    for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
                    {
                        if ((Selection & (1 << I)) != 0)
                        {
                            UnitObj = Army.FArmy.GetUnit(I);
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

                            if (Chance < Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(100))
                            {
                                Selection = (byte)(Selection & ~(1 << I));
                            }
                        }
                    }

                    if (Army.FArmy.Transporter(255) != null)
                    {
                        if (Army.FArmy.Transporter(Selection) != null)
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
                        Log.FPlayer = source.FPlayer;
                        var tornado = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TornadoRStr);
                        Log.FArmyText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XCastRStr), [tornado]);
                        var tornadoText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TornadoRStr);
                        Log.FArmyMessages.Add(tornadoText);
                        Log.FArmyIcon = 2;
                        Log.FArmyView.Setup(Army.FArmy, Selection);
                        var yhx = Army.Yhx;
                        var xhx = Army.Xhx;
                        Aow.Game.Events.TEventLog.DistributeLocationEventLog(Log, (sbyte)FEventPlayerIndex, xhx, yhx, Army.Lhx, true);
                        Log.Release();
                        Location.XYL = ((TTornado)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(68)).CalculateWarpLocation(Army, Selection);
                        if (Location.XYL != 16777215)
                        {
                            if (source.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2))
                            {
                                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(1);
                            }

                            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == source.FPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.VisibleForPlayer(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer))
                            {
                                if (Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1] != null)
                                {
                                    Anim = new TImageSequenceAnimation(null);
                                    Anim.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1]);
                                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer);
                                }
                            }

                            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == source.FPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.VisibleForPlayer(Location.X, Location.Y, Location.L, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer))
                            {
                                if (Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1] != null)
                                {
                                    Anim = new TImageSequenceAnimation(null);
                                    Anim.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1]);
                                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, Location.X, Location.Y, Location.L);
                                }
                            }

                            MovedUnits = new TUnitUserList(null);
                            Owner = (byte)Army.FArmy.FPlayer;
                            MapF = (TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetFieldXYL(Location.XYL);
                            if (Army.FArmy.ValidateSelection(255) != Selection)
                            {
                                for (int I = Army.FArmy.Count - 1; I >= 0; I--)
                                {
                                    if ((Selection & (1 << I)) != 0)
                                    {
                                        UnitObj = Army.FArmy.GetUnit(I);
                                        MovedUnits.AddUnit(UnitObj);
                                        Placed = UnitObj.Place(Location.X, Location.Y, Location.L, (sbyte)(byte)Army.FArmy.FPlayer, TSet<TUnitPlaceModeElement>.Of());
                                        if (Placed != null)
                                        {
                                            Placed.FArmy.UpdateExploration();
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
                                var unitsToIndependentRelation = Aow.Game.Units.TUnitList.UnitsToIndependentRelation(MovedUnits, 5);
                                Group = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FAIGroupManager.CreateGroup(0, unitsToIndependentRelation, TGroupBehavior.gbHunt);
                                Group.AddUnits(MovedUnits);
                                ((THuntAG)Group).FTargetUnitIDs.Add(source.FUnitID);
                                Group.Release();
                            }

                            MovedUnits?.Dispose();
                        }
                    }
                }

                FAnimation?.Dispose();
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
            int XShift;
            int Height;
            int X;
            int Y;
            float Angle;
            int Direction;
            int Radius;
            int Last;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewL == Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer)
            {
                Aow.Game.World.Map.HexGeometry.HXtoHP(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                FAngle = FAngle + 20 + FAnimation.FFrameCounter / 5;
                RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FInitialRandomSeed;
                for (int I = 0; I <= 200; I++)
                {
                    Direction = Random(360);
                    if (Animation.FFrameCounter >= (int)(((uint)I) >> 2) && Animation.FFrameCounter <= (int)(((uint)I) >> 4) + 68)
                    {
                        Radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 7;
                        Height = 200 - I - 16;
                        XShift = Round((double)((double)Math.Sin((double)((double)FAnimation.FFrameCounter * 0.1) + (double)I * 0.02) * (200 - I)) * 0.25) + 16;
                        var pi = Math.PI;
                        Angle = (float)((double)((double)(Direction + FAngle) * pi) / 180);
                        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail == 0)
                        {
                            Last = 1;
                        }
                        else
                        {
                            if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail == 1)
                            {
                                Last = (int)(((uint)(200 - I)) >> 5);
                            }
                            else
                            {
                                Last = (int)(((uint)(200 - I)) >> 4);
                            }
                        }

                        for (int Count = Last; Count >= 0; Count--)
                        {
                            X = Round((double)Math.Cos(Angle) * Radius) + XShift;
                            Y = Round((double)((double)Math.Sin(Angle) * Radius) * 0.5) + Height;
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(68).FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                            Angle = (float)((double)0.035 + Angle);
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:215
        public override bool Validate(THero source) => base.Validate(source) && (FArmyID == null || FArmyID.Valid());
    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TTornado : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1798
        public TTornado(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TornadoRStr);
            FSpellID = 68;
            FAboveGroundOnly = true;
        }

        // GlobalTargetSpells.pas:1885
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            TTargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
        }

        // GlobalTargetSpells.pas:1865
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result;
            TTornadoTE Event;
            TArmyHS Army;
            if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new TTornadoTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
                Army = ((TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(X, Y, L)).GetArmyHS();
                if (Army != null)
                {
                    Event.FArmyID = Army.FArmy.CreateID(255);
                }

                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
            TAoWMapField MapF;
            TMapFieldMsg Msg = default;
            MapF = (TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(X, Y, Layer);
            bool Result = MapF.GetArmyHS() == null && MapF.GetPlayer() == -1;
            if (Result)
            {
                Msg.MsgID = 4384;
                Msg.Param[0] = Army;
                Msg.Result = 0;
                MapF.SendMapFieldMsg(ref Msg);
                Result = Msg.Result.Int == 0 && Army.FArmy.CanPlace(X, Y, Layer);
            }

            return Result;
        }

        // GlobalTargetSpells.pas:1823
        public int CalculateWarpLocation(TArmyHS Army, byte Selection)
        {
            TSpellMoveTable Table = default;
            THNWalkData Walk = default;
            TMapLevel MapLevel;
            TMapField MapF;
            TList Locations;
            bool Found;
            int Index;
            int X;
            Locations = new TList();
            Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref Table));
            var yhx = Army.Yhx;
            var xhx = Army.Xhx;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, xhx, yhx, 10);
            MapLevel = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[Army.Lhx];
            while (Walk.Radius < 15)
            {
                X = Walk.X;
                if (X >= 0 && Walk.Y >= 0 && X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    MapF = MapLevel.GetField(X, Walk.Y);
                    if (Table.Cost[MapF.TerrainType][MapF.OverlayType] != -1)
                    {
                        Locations.Add(MapF);
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            Found = false;
            int Result = 16777215;
            while (Locations.Count != 0 && !Found)
            {
                Index = Random(Locations.Count);
                MapF = (TMapField)Locations[Index];
                if (ValidWarpLocation(Army, MapF.Xhx, MapF.Yhx, MapF.Lhx))
                {
                    Found = true;
                    Result = MapF.XYL;
                }
                else
                {
                    Locations.Delete(Index);
                }
            }

            Locations?.Dispose();
            return Result;
        }
    }
}

namespace Aow.Game.Commands
{

    // angle, animation, selected army identity
    public partial class TVortexTE : TGlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1905
        public new const int ClassID = 135973;

        public int FAngle;
        public TAnimation FAnimation;
        public TArmyID FArmyID;

        // GlobalTargetSpells.pas:1899
        public override void Dispose()
        {
            FArmyID?.Dispose();
            base.Dispose();
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
        public override bool Process(TAbstractUnit source, int value)
        {
            bool Result = default;
            TAnimation Anim = default;
            TArmyEventLog Log = default;
            byte Selection = default;
            bool Damaged = default;
            int Damage = default;
            TArmyHS Army = default;
            TAbstractUnit UnitObj = default;
            if (value == 0)
            {
                FAngle = 0;
                var spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateObservation(source.FPlayer, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                if (source.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2))
                {
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
                }

                Anim = new TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
                Anim.FTag = (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewWidth & 65535) | (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
            }

            if (value == 80)
            {
                if (FArmyID != null && FArmyID.RetrieveArmy(ref Army, ref Selection))
                {
                    Log = new TArmyEventLog(null);
                    Log.FArmyText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Commands.TVortexTE.VortexDamageRStr);
                    Log.FPlayer = source.FPlayer;
                    var vortex = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.VortexRStr);
                    Log.FArmyMessages.Add(vortex);
                    Log.FArmyIcon = 2;
                    Log.FArmyView.Setup(Army.FArmy, 255);
                    Army.FArmy.BeginUpdate();
                    Damaged = false;
                    for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
                    {
                        UnitObj = Army.FArmy.GetUnit(I);
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
                                UnitObj.SetHitPoints((sbyte)(UnitObj.GetHitPoints() - Damage));
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

                    if (Damaged)
                    {
                        Log.ValidateGameOver();
                        var yhx = Army.Yhx;
                        var xhx = Army.Xhx;
                        Aow.Game.Events.TEventLog.DistributeLocationEventLog(Log, Army.FArmy.FPlayer, xhx, yhx, Army.Lhx, true);
                    }

                    Log.Release();
                    Army.FArmy.Validate();
                    Army.FArmy.ValidateTerrainEx(((TMapField)Army.Owner).TerrainType, ((TMapField)Army.Owner).OverlayType, source.FPlayer);
                    Army.FArmy.EndUpdate();
                }

                FAnimation?.Dispose();
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
            int XShift;
            int Height;
            int X;
            int Y;
            float Angle;
            int Direction;
            int Radius;
            int Count;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewL == Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer)
            {
                Aow.Game.World.Map.HexGeometry.HXtoHP(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                FAngle = FAngle + 20 + FAnimation.FFrameCounter / 5;
                RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FInitialRandomSeed;
                for (int I = 0; I <= 200; I++)
                {
                    Direction = Random(360);
                    if (Animation.FFrameCounter >= (int)(((uint)(200 - I)) >> 2) && Animation.FFrameCounter <= (int)(((uint)I) >> 4) + 68)
                    {
                        Radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 18;
                        Height = -16;
                        XShift = 16;
                        var pi = Math.PI;
                        Angle = (float)((double)((double)-(Direction + FAngle) * pi) / 180);
                        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail == 0)
                        {
                            Count = 1;
                        }
                        else
                        {
                            if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail == 1)
                            {
                                Count = (int)(((uint)(200 - I)) >> 5);
                            }
                            else
                            {
                                Count = (int)(((uint)(200 - I)) >> 4);
                            }
                        }

                        for (int J = Count; J >= 0; J--)
                        {
                            X = Round((double)Math.Cos(Angle) * Radius) + XShift;
                            Y = Round((double)((double)Math.Sin(Angle) * Radius) / 1.75) + Height;
                            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(70).FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                            Angle = (float)((double)0.035 + Angle);
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:242
        public override bool Validate(THero source) => base.Validate(source) && (FArmyID == null || FArmyID.Valid());

        private static readonly string VortexDamageRStr = "Vortex Damage";
    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class TVortex : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2069
        public TVortex(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.VortexRStr);
            FSpellID = 70;
            FAboveGroundOnly = false;
        }

        // GlobalTargetSpells.pas:2120
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            // { Use the incoming target. }
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FHNs.Clear();
            TargetSelection.FOnValidateTarget = ValidateTargetEvent;
        }

        // GlobalTargetSpells.pas:2100
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result;
            TVortexTE Event;
            TArmyHS Army;
            if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new TVortexTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
                Army = ((TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(X, Y, L)).GetArmyHS();
                if (Army != null)
                {
                    Event.FArmyID = Army.FArmy.CreateID(255);
                }

                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        public override bool ValidTargetSelection(TTargetSelectionControl TargetSelection)
        {
            bool Result;
            TMapField MapF;
            if (TargetSelection.FTargetX != -1)
            {
                MapF = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[TargetSelection.FTargetL].GetField(TargetSelection.FTargetX, TargetSelection.FTargetY);
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
            TMapLevel MapLevel;
            if (X != -1)
            {
                MapLevel = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[Layer];
                var fieldValue = MapLevel.GetField(X, Y);
                Valid = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
            }
            else
            {
                Valid = false;
            }
        }
    }
}

namespace Aow.Game.Commands
{

    // animation
    public partial class THealingShowersTE : TGlobalTargetSpellTE
    {
        public THealingShowersTE(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:2138
        public new const int ClassID = 135975;

        public TAnimation FAnimation;

        // GlobalTargetSpells.pas:2190
        public override bool Process(TAbstractUnit source, int value)
        {
            bool Result = default;
            TAbstractUnit UnitObj = default;
            THNWalkData Walk = default;
            TArmyHS Army = default;
            TMapLevel Level = default;
            TImageSequenceAnimation Anim = default;
            bool Healed = default;
            bool PlaySound = default;
            THexagonSprite Cloud = default;
            base.Process(source, value);
            if (value == 0)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateTerrainObservation(source.FPlayer, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2);
                if (source.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.WatchingTerrain(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 2))
                {
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(73).FCastSounds.Play(0);
                }

                Anim = (TImageSequenceAnimation)new TAnimation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
                Anim.FTag = (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewWidth & 65535) | (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewHeight << 16);
                Anim.Activate();
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            }

            if (value == 50)
            {
                PlaySound = false;
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ChangeTerrainEx(null, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, null);
                Level = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer];
                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, 0);
                while (Walk.Radius <= 1)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Level.Height > Walk.Y)
                    {
                        if (!((TAoWMapField)Level.GetField(Walk.X, Walk.Y)).GetMagicShieldActive())
                        {
                            Healed = false;
                            Army = ((TAoWMapField)Level.GetField(Walk.X, Walk.Y)).GetArmyHS();
                            if (Army != null)
                            {
                                for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
                                {
                                    UnitObj = Army.FArmy.GetUnit(I);
                                    if (UnitObj.GetHitPoints() < UnitObj.GetHits())
                                    {
                                        UnitObj.SetHitPoints((sbyte)(UnitObj.GetHitPoints() + 5));
                                        Healed = true;
                                    }

                                    if (UnitObj.GetAbilityEnabled(96))
                                    {
                                        UnitObj.RemoveAbility(96);
                                        Healed = true;
                                    }
                                }

                                Army.FArmy.Update();
                                bool healed = Healed;
                                if (healed)
                                {
                                    var yhx = Army.Yhx;
                                    var xhx = Army.Xhx;
                                    healed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.VisibleForPlayer(xhx, yhx, Army.Lhx, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
                                }

                                if (healed)
                                {
                                    bool condition = PlaySound || (byte)Army.FArmy.FPlayer == (byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer;
                                    if (!condition)
                                    {
                                        var yhx = Army.Yhx;
                                        var xhx = Army.Xhx;
                                        condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(xhx, yhx, Army.Lhx, 1);
                                    }

                                    PlaySound = condition;
                                    Anim = new TImageSequenceAnimation(null);
                                    Anim.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(47).FImageSequences.ImageSequences[0]);
                                    Anim.FAnimationLevel = 27;
                                    var armyYhx = Army.Yhx;
                                    var armyXhx = Army.Xhx;
                                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, armyXhx, armyYhx, Army.Lhx);
                                }
                            }

                            Cloud = (THexagonSprite)Level.GetField(Walk.X, Walk.Y).FindOwnedChild(132990);
                            if (Cloud != null)
                            {
                                Cloud?.Dispose();
                            }
                        }
                    }

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }

                if (PlaySound)
                {
                    Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(47).FSFXLibrary.Play(0);
                }
            }

            if (value == 80)
            {
                FAnimation?.Dispose();
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
            int X;
            int Y;
            int XShift;
            int YShift;
            int Speed;
            int Xhp = default;
            int Yhp = default;
            int Count;
            int Frame;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewL == Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Layer)
            {
                Aow.Game.World.Map.HexGeometry.HXtoHP(Unsafe.As<TXYL, TSpellLocation>(ref FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                switch (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDetailLevel())
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

                RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FInitialRandomSeed;
                for (int I = 0; I <= Count; I++)
                {
                    X = Random(100) - 50;
                    Y = Random(100) - 50;
                    Speed = Random(5) + 5;
                    var random = Random(30);
                    Frame = Animation.FFrameCounter - random;
                    if (Frame >= 0 && Frame < 50)
                    {
                        Frame = Frame % 30;
                        X -= Round((double)(Frame * 4) * ((double)Speed * 0.2));
                        Y += Round((double)(Frame * 20) * ((double)Speed * 0.2));
                        if (Y < 500)
                        {
                            for (int J = 0; J <= 10; J++)
                            {
                                XShift = X - Round((double)(J * 2) * 0.1);
                                YShift = Y + Round((double)(J * 10) * 0.1);
                                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(73).FCastImages.Show(0, Xhp + XShift + 120, Yhp + YShift - 500 + 30, 0, SurfaceDesc);
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
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class THealingShowers : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2277
        public THealingShowers(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.GlobalTarget.THealingShowers.HealingShowersRStr);
            FSpellID = 73;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msWater;
        }

        // GlobalTargetSpells.pas:2295
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            TTargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:2308
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            THealingShowersTE Event;
            Event = new THealingShowersTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:2291
        public override bool ValidTargetSelectionMapF(TMapField Field) => base.ValidTargetSelectionMapF(Field);

        // GlobalTargetSpells.pas:2287
        public override bool ValidTargetMapF(TMapField Field) => base.ValidTargetMapF(Field);

        private static readonly string HealingShowersRStr = "Healing Showers";
    }

    public partial class TSmokeCloudSpell : TGlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2325
        public TSmokeCloudSpell(TECustomNode owner) : base(owner)
        {
            this.FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.SmokeCloudRStr);
            FSpellID = 74;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Game.Magic.TMagicSphere.msFire;
        }

        // GlobalTargetSpells.pas:2373
        public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
        {
            TEResource Resource;
            TSmokeCloud Cloud;
            var source = Event.GetSource();
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ValidateObservation(source.FPlayer, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).Layer, 2);
            if (Event.GetSource().FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).Layer, 2))
            {
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(Event.FEventSpellID).FCastSounds.Play(0);
            }

            Resource = (TEResource)((TECustomNode)(object)Aow.Game.Engine.TAoWHSSet.AoWHSSet.MapObjectResourceList).FindChild(135747);
            if (Resource != null)
            {
                Cloud = (TSmokeCloud)Resource.CreateResourceUser();
                Cloud.FPlayer = Source.FPlayer;
                Cloud.FTurnsRemaining = -1;
                Cloud.FEnchantment.FPlayerIndex = Source.FPlayer;
                Cloud.FEnchantment.FEnchantmentManaUpkeep = this.FManaUpkeep;
                Cloud.FDrifting = false;
                if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Cloud, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).X, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).Y, Unsafe.As<TXYL, TSpellLocation>(ref ((TGlobalTargetSpellTE)Event).FLocation).Layer))
                {
                    Cloud?.Dispose();
                }

                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(this.FSpellID).CastingDone(Source, this.FBaseManaCost);
            }

            return false;
        }

        // GlobalTargetSpells.pas:2343
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            TTargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = Aow.Game.World.Map.HexGeometry.RadToHN(4) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:2356
        public override bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            TGlobalTargetSpellTE Event;
            Event = new TGlobalTargetSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:2339
        public override bool ValidTargetSelectionMapF(TMapField Field) => base.ValidTargetSelectionMapF(Field);

        // GlobalTargetSpells.pas:2335
        public override bool ValidTargetMapF(TMapField Field) => base.ValidTargetMapF(Field);
    }

    public partial class TGlobalTargetSpellCalls : Aow.Game.Magic.Spells.TSpell
    {
        public TGlobalTargetSpellCalls(TECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:303
        public virtual void ShowSelection(TTargetSelectionControl Target, int HN, int X, int Y, in TSurfaceDesc SD)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:305
        public virtual void SelectionChanged()
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
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }
    }

    [LegacyObject]
    public partial class TTargetSelectionView
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
        public string Caption = "";
        public string CancelCaption = "";
        public Aow.Game.Draw.Images.TLibraryImage Preview;
        public Aow.Game.Draw.Images.TLibraryImage ValidPreview;
        public Aow.Game.Draw.Images.TLibraryImage InvalidPreview;

        public TTargetSelectionView()
        {
            CaptureAddress.Assign(this);
        }
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
        private string caption;

        public string Caption { readonly get => caption ?? ""; set => caption = value; }

        private string cancelCaption;

        public string CancelCaption { readonly get => cancelCaption ?? ""; set => cancelCaption = value; }

        public int Reserved48;
        public int Reserved4C;
        public Aow.Game.Draw.Images.TLibraryImage Preview;
        public Aow.Game.Draw.Images.TLibraryImage ValidPreview;
        public Aow.Game.Draw.Images.TLibraryImage InvalidPreview;
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
}
