// Translated from GlobalTargetSpells.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai.Groups;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
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

namespace Aow.Game.Commands
{

    // FLocation: packed X/Y/layer coordinates
    public partial class GlobalTargetSpellTE : SpellTE
    {
        public GlobalTargetSpellTE(ECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:401
        public new const int ClassID = 135976;

        public XYL FLocation;

        // GlobalTargetSpells.pas:406
        // packed coordinates
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FLocation.Packed, 20);
        }

        // GlobalTargetSpells.pas:412
        public void SetLocation(sbyte X, sbyte Y, sbyte Layer)
        {
            Unsafe.As<XYL, SpellLocation>(ref FLocation).X = X;
            Unsafe.As<XYL, SpellLocation>(ref FLocation).Y = Y;
            Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer = Layer;
        }

        // GlobalTargetSpells.pas:419
        public void PlayDefaultEffects(sbyte X, sbyte Y, sbyte Layer)
        {
            ImageSequenceAnimation Animation;
            if (AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
            {
                Animation = new ImageSequenceAnimation(null);
                Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
                AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
            }

            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells
{

    // FCasterID, FCasterPlayer, FAboveGroundOnly CanActivate/Activate and the target predicates
    // return Boolean. Procedure declarations retained for shared TSpell and P7 overrides.
    // ShowTargetSelection variants: reordered locals, split/inline radius, target with-scope, and
    // inline map-field lookup. SetupTargetSelection variants: explicit class local, repeated casts,
    // class with-scope, record fields, record with-scope and reversed coordinate local order
    public partial class GlobalTargetSpell : Spell
    {

        public int FCasterID;
        public sbyte FCasterPlayer;
        public bool FAboveGroundOnly;

        // GlobalTargetSpells.pas:439
        public GlobalTargetSpell(ECustomNode owner) : base(owner)
        {
            FSpellType = SpellType.Global;
        }

        // GlobalTargetSpells.pas:569
        public override bool CanActivate(AbstractUnit Source, ref string MessageText)
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
                    MessageText = AbilityRules.TranslateRStr(GameText.CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:618
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string MessageText = "";
            base.Activate(Source);
            if (CanActivate(Source, ref MessageText))
            {
                FCasterID = Source.FUnitID;
                FCasterPlayer = Source.FPlayer;
                SetupTargetSelection(Source, AoWHSMap.Current.FTargetSelection);
                AoWHSMap.Current.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:445
        public virtual void ShowTargetSelection(TargetSelectionControl Target, int HN, int Xs, int Ys, in SurfaceDesc SurfaceDesc)
        {
            MapLevel Level;
            AoWMapField MapF;
            int X = default;
            int Y = default;
            int Intensity;
            int Radius;
            Level = AoWHSMap.Current.MapContainer.MapLevels[Target.FTargetL];
            HexGeometry.CenterHNtoHX(Target.FTargetX, Target.FTargetY, HN, ref X, ref Y);
            // { Original accepts Width/Height themselves (strict > bounds test). }
            if (X >= 0 && Y >= 0 && X <= Level.Width && Y <= Level.Height)
            {
                MapF = (AoWMapField)Level.Field[X, Y];
                if (this.ValidTargetSelectionMapF((MapField)(object)MapF))
                {
                    Radius = HexGeometry.HNtoRad(HN);
                    Intensity = Math.Abs((AoWHSMap.Current.Frame + Radius) % 9 - 4) * 20 + 20;
                    Target.FImageLibrary[0].ShowBlended(Xs - 8, Ys, SurfaceDesc, GfxState.IntensityBlendTable.LookupTables[Intensity]);
                }
                else
                {
                    Target.FImageLibrary[1].Show(Xs - 8, Ys, SurfaceDesc);
                }
            }
        }

        // GlobalTargetSpells.pas:521
        public virtual void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            sbyte UnitX = default;
            sbyte UnitY = default;
            sbyte UnitLayer = default;
            TargetSelection.Initialize(Source);
            TargetSelection.FTargetMode = TargetSelection.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.OwnerDraw, TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
            Source.GetLocation(ref UnitX, ref UnitY, ref UnitLayer);
            TargetSelection.FSourceX = UnitX;
            TargetSelection.FSourceY = UnitY;
            TargetSelection.FSourceL = UnitLayer;
            TargetSelection.FRange = 12;
            TargetSelection.FOnChange += OnTargetSelectionChange;
            TargetSelection.FOnShow = ShowTargetSelection;
            var format = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.SelectTargetLocationForXOnMapRStr), [FSpellName]);
            TargetSelection.FText.Text = format;
            TargetSelection.FTitle = FSpellName;
            TargetSelection.FCancelText = AbilityRules.TranslateRStr(GameText.CancelRStr);
            TargetSelection.FSourceImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[20].GetPreviewImage();
            TargetSelection.FValidTargetImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[21].GetPreviewImage();
            TargetSelection.FInvalidTargetImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[22].GetPreviewImage();
        }

        // GlobalTargetSpells.pas:544
        public virtual bool TargetSelected(AbstractUnit Source, int X, int Y, int L) => false;

        // GlobalTargetSpells.pas:487
        public virtual bool ValidTargetSelection(TargetSelectionControl TargetSelection) => true;

        // GlobalTargetSpells.pas:475
        public virtual bool ValidTargetSelectionMapF(MapField Field)
        {
            if (AoWHSMap.Current.FExplorationEnabled && !((AoWMapField)Field).GetExplored(FCasterPlayer))
            {
                // Preserve the result for callers.
                return false;
            }

            return true;
        }

        // GlobalTargetSpells.pas:469
        public virtual bool ValidTargetMapF(MapField Field) => true;

        // GlobalTargetSpells.pas:492. The recovered view aliases this control.
        public virtual void OnTargetSelectionChange(TargetSelectionControl target)
        {
            var caster = AoWHSMap.Current.FUnitControl.FindUnit(FCasterID);
            if (caster is null || target.FChange == TargetSelectionChange.Deactivated)
            {
                target.FOnChange -= OnTargetSelectionChange;
                target.Deactivate();
            }
            else if (target.FChange == TargetSelectionChange.Selected && ValidTargetSelection(target) &&
                TargetSelected(caster, target.FTargetX, target.FTargetY, target.FTargetL))
            {
                target.FOnChange -= OnTargetSelectionChange;
                target.Deactivate();
            }
        }

        internal static bool GlobalTargetCanActivate(GlobalTargetSpell Spell, AbstractUnit Caster, ref string MessageText)
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
                    MessageText = AbilityRules.TranslateRStr(GameText.CannotCastSpellInUndergoundRStr);
                }
            }

            return Result;
        }

        internal static bool GlobalTargetActivate(GlobalTargetSpell Spell, AbstractUnit Caster, int Unused)
        {
            bool Result;
            string MessageText = "";
            Spell.Activate__static(Caster);
            if (Spell.CanActivate(Caster, ref MessageText))
            {
                Spell.FCasterID = Caster.FUnitID;
                Spell.FCasterPlayer = Caster.FPlayer;
                Spell.SetupTargetSelection(Caster, AoWHSMap.Current.FTargetSelection);
                AoWHSMap.Current.FTargetSelection.Activate();
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

    public partial class FreezeWaterTE : GlobalTargetSpellTE
    {
        public FreezeWaterTE(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135939;

        // GlobalTargetSpells.pas:679
        public override bool Process(AbstractUnit source, int value)
        {
            TwinkleAnimation Twinkle = default;
            base.Process(source, value);
            AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2);
            if (AoWHSMap.Current.TerrainVisible((sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
            {
                Twinkle = new TwinkleAnimation(null);
                Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(10).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand), 100, 64, 50, 30);
                AoWHSMap.Current.PlaceHX(Twinkle, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
            }

            // Use the map's seated player.
            if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.WatchingTerrain((sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
            {
                AoWHSSet.Current.FSpellControl.GetSpell(10).FCastSounds.Play(0);
            }

            AoWHSMap.Current.ChangeTerrainEx(null, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, TerrainChanged);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            // The original event result is zero.
            return false;
        }

        // GlobalTargetSpells.pas:641
        public void ChangeTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            FrozenWaterHS Ice;
            if (((AoWMapField)MapF).GetArmyHS() == null)
            {
                if (TerrainType == (int)AoWTerrainType.Water)
                {
                    TerrainType = (int)AoWTerrainType.Ice;
                }
                else
                {
                    if (TerrainType == (int)AoWTerrainType.UndergroundWater)
                    {
                        TerrainType = (int)AoWTerrainType.UndergroundIce;
                    }
                    else
                    {
                        if (Set<byte>.WithBounds(0, 255).With((int)AoWTerrainType.Ice).With((int)AoWTerrainType.UndergroundIce).Contains((byte)TerrainType))
                        {
                            Ice = (FrozenWaterHS)MapF.FindOwnedHS(typeof(FrozenWaterHS));
                            if (Ice != null)
                            {
                                Ice.FDaysRemaining = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                                Ice.FCasterPlayer = (sbyte)FEventPlayerIndex;
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:661
        public void TerrainChanged(MapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
        {
            FrozenWaterHS Ice;
            if (Set<byte>.WithBounds(0, 255).With((int)AoWTerrainType.Ice).With((int)AoWTerrainType.UndergroundIce).Contains((byte)MapF.TerrainType) && !Set<byte>.WithBounds(0, 255).With((int)AoWTerrainType.Ice).With((int)AoWTerrainType.UndergroundIce).Contains((byte)OriginalTerrainType))
            {
                Ice = (FrozenWaterHS)MapF.FindOwnedHS(typeof(FrozenWaterHS));
                if (Ice == null)
                {
                    Ice = new FrozenWaterHS(null);
                    AoWHSMap.Current.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
                }

                Ice.FDaysRemaining = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                Ice.FCasterPlayer = (sbyte)FEventPlayerIndex;
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells
{

    public partial class FreezeWater : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:710
        public FreezeWater(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(FreezeWaterRStr);
            FSpellID = 10;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Water;
        }

        // GlobalTargetSpells.pas:811
        // { Original Boolean-return ABI; foreign P7 overrides remain procedures. }
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            TargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:823
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            FreezeWaterTE Event;
            Event = new FreezeWaterTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        public override bool ValidTargetMapF(MapField field)
        {
            if (!base.ValidTargetMapF(field)) return false;
            sbyte terrain = field.FTerrainType;
            return (terrain == 0 || terrain == 10) && field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)field).GetArmyHS() == null;
        }

        public override bool ValidTargetSelectionMapF(MapField field)
        {
            if (!base.ValidTargetSelectionMapF(field)) return false;
            sbyte terrain = field.FTerrainType;
            if ((terrain != 0 && terrain != 10) || field.FindNoneTransparentHS(typeof(Structure)) != null) return false;
            var army = ((AoWMapField)field).GetArmyHS();
            return army == null || !army.VisibleForSeatedPlayer();
        }

        private static readonly string FreezeWaterRStr = "Freeze Water";
    }
}

namespace Aow.Game.Commands
{

    public partial class FireBarrierTE : GlobalTargetSpellTE
    {
        public FireBarrierTE(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 135940;

        // GlobalTargetSpells.pas:873
        public override bool Process(AbstractUnit source, int value)
        {
            base.Process(source, value);
            CreateFireBarrier();
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            return false;
        }

        // GlobalTargetSpells.pas:842
        public void CreateFireBarrier()
        {
            HNWalkData Walk = default;
            MapLevel Level;
            AoWMapField MapF;
            Fire Fire;
            RandSeed = AoWHSMap.Current.Random(16777215);
            Level = AoWHSMap.Current.MapContainer.MapLevels[Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer];
            HNWalkData.InitHNWalk(ref Walk, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    MapF = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                    if (((GlobalTargetSpell)(object)AoWHSSet.Current.FSpellControl.GetSpell(11)).ValidTargetMapF((MapField)(object)MapF) && MapF.GetArmyHS() == null)
                    {
                        Fire = AoWHSSet.Current.FFireControl.PlaceFire(Walk.X, Walk.Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, FireType.Large);
                        if (Fire != null)
                        {
                            Fire.SetupExplosion(Random(35));
                            Fire.FTurnsLeft = 3;
                            Fire.FPlayer = this.GetSource().FPlayer;
                        }
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells
{

    public partial class FireBarrier : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:882
        public FireBarrier(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(FireBarrierRStr);
            FSpellID = 11;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Fire;
        }

        // GlobalTargetSpells.pas:914
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            TargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:926
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            FireBarrierTE Event;
            Event = new FireBarrierTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:900
        public override bool ValidTargetSelectionMapF(MapField Field)
        {
            ArmyHS Army;
            bool Result = base.ValidTargetSelectionMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(Structure)) == null;
            if (Result)
            {
                Army = ((AoWMapField)Field).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:892
        public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;

        private static readonly string FireBarrierRStr = "Fire Barrier";
    }
}

namespace Aow.Game.Commands
{

    public partial class LevelTerrainTE : GlobalTargetSpellTE
    {
        public LevelTerrainTE(ECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:940
        public new const int ClassID = 135945;

        // GlobalTargetSpells.pas:968
        public override bool Process(AbstractUnit source, int value)
        {
            base.Process(source, value);
            PlayDefaultEffects(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
            LevelTerrain();
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            return false;
        }

        // GlobalTargetSpells.pas:943
        public void LevelTerrain()
        {
            HNWalkData Walk = default;
            MapLevel Level;
            AoWMapField MapF;
            Level = AoWHSMap.Current.MapContainer.MapLevels[Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer];
            HNWalkData.InitHNWalk(ref Walk, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, 0);
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    MapF = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                    if (((GlobalTargetSpell)(object)AoWHSSet.Current.FSpellControl.GetSpell(18)).ValidTargetMapF((MapField)(object)MapF) && MapF.GetArmyHS() == null)
                    {
                        AoWHSMap.Current.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 0, -1);
                        if (MapF.TerrainType == (int)AoWTerrainType.Earth)
                        {
                            AoWHSMap.Current.ChangeTerrain(null, Walk.X, Walk.Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 0, (int)AoWTerrainType.Dirt);
                        }
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells
{

    public partial class LevelTerrain : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:978
        public LevelTerrain(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(LevelTerrainRStr);
            FSpellID = 18;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Earth;
        }

        // GlobalTargetSpells.pas:1010
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            TargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:1022
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            LevelTerrainTE Event;
            Event = new LevelTerrainTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:996
        public override bool ValidTargetSelectionMapF(MapField Field)
        {
            ArmyHS Army;
            bool Result = base.ValidTargetSelectionMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(Structure)) == null;
            if (Result)
            {
                Army = ((AoWMapField)Field).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // GlobalTargetSpells.pas:988
        public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;

        private static readonly string LevelTerrainRStr = "Level Terrain";
    }
}

namespace Aow.Game.Commands
{

    public partial class RejuvenateTE : GlobalTargetSpellTE
    {
        public RejuvenateTE(ECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:1036
        public new const int ClassID = 135952;

        // GlobalTargetSpells.pas:1044
        public override bool Process(AbstractUnit source, int value)
        {
            TwinkleAnimation Twinkle = default;
            base.Process(source, value);
            AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2);
            if (AoWHSMap.Current.TerrainVisible((sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
            {
                Twinkle = new TwinkleAnimation(null);
                Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(41).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 100, 64, 50, 30);
                AoWHSMap.Current.PlaceHX(Twinkle, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
            }

            // Use the map's seated player.
            if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.WatchingTerrain((sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
            {
                AoWHSSet.Current.FSpellControl.GetSpell(41).FCastSounds.Play(0);
            }

            AoWHSMap.Current.ChangeTerrainEx(null, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2, RejuvenateTerrain, null);
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            // The original event result is zero.
            return false;
        }

        // GlobalTargetSpells.pas:1039
        public void RejuvenateTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
        {
            if (Set<byte>.WithBounds(0, 255).WithRange((int)AoWTerrainType.Desert, (int)AoWTerrainType.Wasteland).Contains((byte)TerrainType))
            {
                TerrainType = (int)AoWTerrainType.Grass;
            }
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells
{

    public partial class Rejuvenate : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1074
        public Rejuvenate(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(RejuvenateRStr);
            FSpellID = 41;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Life;
            FAboveGroundOnly = true;
        }

        // GlobalTargetSpells.pas:1095
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            TargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = HexGeometry.RadToHN(3) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:1107
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            RejuvenateTE Event;
            Event = new RejuvenateTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:1090
        public override bool ValidTargetSelectionMapF(MapField Field) => base.ValidTargetSelectionMapF(Field) && Set<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)Field.TerrainType);

        // GlobalTargetSpells.pas:1085
        public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)Field.TerrainType);

        private static readonly string RejuvenateRStr = "Rejuvenate";
    }

    // FWatcher
    public partial class WatcherEnchantment : GlobalEnchantment
    {
        public WatcherHS FWatcher;

        // GlobalTargetSpells.pas:1121
        public WatcherEnchantment(ECustomNode owner) : base(owner)
        {
            FEnchantmentMode = Set<GlobalEnchantmentModeElement>.FromMask(FEnchantmentMode.Mask) - Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
            FDispelMana = 50;
        }

        // GlobalTargetSpells.pas:1129
        public override string GetName() => AoWHSSet.Current.FSpellControl.GetSpell(57).FSpellName;

        // The unused fourth byte in the original register result has no defined value.
        public override XYL GetLocation() => new() { X = FWatcher.GetXhx(), Y = FWatcher.GetYhx(), L = FWatcher.GetLhx() };

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
    public partial class WatcherHS : SingleHS
    {
        // GlobalTargetSpells.pas:1220
        public new const int ClassID = 132989;

        public byte FWatcherState;
        public byte FUnfogRadius;
        public WatcherEnchantment FEnchantment;
        public int FAnimationFrame;
        public IntegerList FUnexplored;
        public bool FVisibleForSeatedPlayer;

        // GlobalTargetSpells.pas:1213
        public override void Dispose()
        {
            base.Dispose();
            FUnexplored?.Dispose();
            FEnchantment.Release();
        }

        // GlobalTargetSpells.pas:1344
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FEnchantment, 40);
        }

        // GlobalTargetSpells.pas:1203
        public WatcherHS(ECustomNode owner) : base(owner)
        {
            FEnchantment = new WatcherEnchantment(null);
            FEnchantment.FWatcher = this;
            FUnexplored = new IntegerList();
            FVisibleForSeatedPlayer = false;
            FAnimationFrame = -1;
        }

        // GlobalTargetSpells.pas:1422
        public override void MsgProc(ref EMsg Msg, EObject Sender)
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
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
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
            bool condition = AoWHSMap.Current.ViewL == Lhx;
            if (condition)
            {
                var yhx = Yhx;
                var xhx = Xhx;
                condition = AoWHSMap.Current.Visible(xhx, yhx, Lhx, 0);
            }

            if (condition)
            {
                var yhx = Yhx;
                HexGeometry.HXtoHP(Xhx, yhx, ref X, ref Y);
                X = X - XhpSurf + 16;
                Y = Y - YhpSurf + 16;
                if (AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages != null)
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages.ShowLooped(1, X, Y, AoWHSMap.Current.Frame, SurfaceDesc);
                }

                if (FAnimationFrame != -1)
                {
                    FAnimationFrame += 1;
                    // The original reads the shared AoWEngine animation detail. The map engine is
                    // not the global read by this routine.
                    if (GameTables.AoWEngine.FAnimationDetail == 2 && AoWHSMap.Current.ViewL == Lhx)
                    {
                        Intensity = 100 - FAnimationFrame;
                        var yhx2 = Yhx;
                        HexGeometry.HXtoHP(Xhx, yhx2, ref X, ref Y);
                        X = X - XhpSurf - 8;
                        Y = Y - YhpSurf;
                        for (int I = FUnexplored.Count - 1; I >= 0; I--)
                        {
                            HexGeometry.HNtoHP(FUnexplored[I], ref PX, ref PY);
                            PX += X;
                            PY += Y;
                            AoWHSSet.Current.FGeneralImages[5].ShowBlended(PX, PY, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[Intensity]);
                        }
                    }

                    // { Original resets the shared random generator for deterministic particles. }
                    RandSeed = AoWHSMap.Current.FInitialRandomSeed;
                    for (int I = 0; I <= 300; I++)
                    {
                        Radius = Random(VisibilityRange() << 5);
                        Angle = Random(360);
                        Delay = Random(80);
                        if (FAnimationFrame - Delay >= 0)
                        {
                            AbilityRules.DegreesRadToCo(Angle, Radius, ref DX, ref DY);
                            AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages.Show(0, X + DX, Y + DY, FAnimationFrame - Delay, SurfaceDesc);
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
            ((MapField)Owner).UpdateMapField();
            return Result;
        }

        // GlobalTargetSpells.pas:1394
        public override void RemoveFromMap()
        {
            MapField MapF;
            MapF = (MapField)Owner;
            base.RemoveFromMap();
            MapF.UpdateMapField();
        }

        // GlobalTargetSpells.pas:1438
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            int OwnerPlayer;
            ref var mapFieldMsg = ref Msg;
            base.MapFieldMsgProc(ref Msg);
            if (mapFieldMsg.MsgID == 4363)
            {
                if (Activated())
                {
                    OwnerPlayer = FEnchantment.FPlayerIndex;
                    if (OwnerPlayer == mapFieldMsg.Param[1].Int || (byte)AoWHSMap.Current.FPlayers.GetPlayers(OwnerPlayer << 0).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMsg.Param[1].Int) == 2)
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
                    if (OwnerPlayer == mapFieldMsg.Param[1].Int || (byte)AoWHSMap.Current.FPlayers.GetPlayers(OwnerPlayer << 0).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMsg.Param[1].Int) == 2)
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
                    AoWHSMap.Current.UpdatePlayerSeen(FEnchantment.FPlayerIndex, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ((MapField)Owner).Lhx, mapFieldMsg.Param[0].Int, mapFieldMsg.Param[0].Int);
                }

                UpdateVisibleForSeatedPlayer();
            }
        }

        // GlobalTargetSpells.pas:1350
        public override void Activate()
        {
            MapField MapF;
            base.Activate();
            FWatcherState = (byte)(Set<WatcherStateElement>.FromMask(FWatcherState) + Set<WatcherStateElement>.Of(WatcherStateElement.Active)).Image;
            FEnchantment.Activate(FEnchantment.FPlayerIndex);
            var mapField = (MapField)Owner;
            AoWHSMap.Current.NotifyPlayerChanged(mapField.Xhx, mapField.Yhx, mapField.Lhx);

            MapF = (MapField)Owner;
            var visibilityRange = VisibilityRange();
            var visibilityRange2 = VisibilityRange();
            AoWHSMap.Current.UpdatePlayerSeen(FEnchantment.FPlayerIndex, MapF.Xhx, MapF.Yhx, MapF.Lhx, visibilityRange, visibilityRange2);
            UpdateExploration();
            UpdateUnfog();
        }

        // GlobalTargetSpells.pas:1366
        public override void Deactivate()
        {
            base.Deactivate();
            FWatcherState = (byte)(Set<WatcherStateElement>.FromMask(FWatcherState) - Set<WatcherStateElement>.Of(WatcherStateElement.Active)).Image;
            if (FEnchantment.FEnchantmentStatus.Contains(GlobalEnchantmentStatusElement.Activated))
            {
                FEnchantment.Deactivate((byte)FEnchantment.FPlayerIndex);
            }

            AoWHSMap.Current.NotifyPlayerChanged(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ((MapField)Owner).Lhx);
            UpdateUnfog();
        }

        // GlobalTargetSpells.pas:1403
        public override void UpdateMapField(in HSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((AoWMapField)(object)Info.MapField).FFieldFlags = ((AoWMapField)(object)Info.MapField).FFieldFlags | 4096 | 16777216;
            }
        }

        // GlobalTargetSpells.pas:1226
        public void StartAnimation(sbyte X, sbyte Y, sbyte Layer)
        {
            HNWalkData Walk = default;
            MapLevel Level;
            FAnimationFrame = 0;
            AoWHSMap.Current.DynamicFixedVisibleHS.AddChild(this);
            AoWHSSet.Current.FSpellControl.GetSpell(57).FCastSounds.Play(0);
            FUnexplored.Clear();
            if (AoWHSMap.Current.FExplorationEnabled)
            {
                HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
                Level = AoWHSMap.Current.MapContainer.MapLevels[Layer];
                while (true)
                {
                    var visibilityRange = VisibilityRange();
                    if (!(Walk.Radius <= visibilityRange))
                        break;
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        if (!((AoWMapField)Level.Field[Walk.X, Walk.Y]).GetExplored(AoWHSMap.Current.FSeatedPlayer))
                        {
                            FUnexplored.Add(Walk.HN);
                        }
                    }

                    HNWalkData.WalkToNextHN(ref Walk);
                }
            }
        }

        // GlobalTargetSpells.pas:1248
        public void StopAnimation()
        {
            AoWHSMap.Current.DynamicFixedVisibleHS.RemoveChild(this);
            FAnimationFrame = -1;
            FUnexplored.Clear();
        }

        // GlobalTargetSpells.pas:1305
        public void UpdateExploration()
        {
            int OwnerPlayer;
            if (Activated())
            {
                for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                {
                    OwnerPlayer = FEnchantment.FPlayerIndex;
                    if (OwnerPlayer == I || (byte)AoWHSMap.Current.FPlayers.GetPlayers(OwnerPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)I) == 2)
                    {
                        var visibilityRange = VisibilityRange();
                        var yhx = Yhx;
                        var xhx = Xhx;
                        AoWHSMap.Current.ExploreArea(xhx, yhx, Lhx, (sbyte)I, visibilityRange);
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:1321
        public void UpdateUnfog()
        {
            int Radius;
            if ((byte)AoWHSMap.Current.FSeatedPlayer != 255 && (FEnchantment.FPlayerIndex == AoWHSMap.Current.FSeatedPlayer || (byte)AoWHSMap.Current.FPlayers.GetPlayers(FEnchantment.FPlayerIndex).FDiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)AoWHSMap.Current.FSeatedPlayer) == 2) && Activated())
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
                    AoWHSMap.Current.FogArea(xhx, yhx, Lhx, FUnfogRadius);
                }

                FUnfogRadius = (byte)Radius;
                if (FUnfogRadius != 0)
                {
                    var yhx = Yhx;
                    var xhx = Xhx;
                    AoWHSMap.Current.UnFogArea(xhx, yhx, Lhx, FUnfogRadius);
                }
            }
        }

        // GlobalTargetSpells.pas:1376
        public int VisibilityRange()
        {
            int Result = 8;
            if (Lhx != 0 || FEnchantment.FPlayerIndex > 0 && AoWHSMap.Current.FPlayers.GetPlayers(FEnchantment.FPlayerIndex).FMagicControl.FEnemyEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
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
            Visible = FEnchantment.VisibleForPlayer(AoWHSMap.Current.FSeatedPlayer);
            if (FVisibleForSeatedPlayer ^ Visible)
            {
                FVisibleForSeatedPlayer = Visible;
                AoWHSMap.Current.FGlobalMagicControl.TriggerChangedEvent();
            }
        }

        public enum WatcherStateElement : byte
        {
            Active = 0
        }

        // GlobalTargetSpells.pas:172

    }
}

namespace Aow.Game.Commands
{

    public partial class WatcherTE : GlobalTargetSpellTE
    {
        public WatcherTE(ECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:1476
        public new const int ClassID = 135963;

        // GlobalTargetSpells.pas:1479
        public override bool Process(AbstractUnit source, int value)
        {
            WatcherHS Watcher;
            base.Process(source, value);
            PlayDefaultEffects(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
            Watcher = new WatcherHS(null);
            Watcher.FEnchantment.FPlayerIndex = source.FPlayer;
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(57);
            Watcher.FEnchantment.FEnchantmentManaUpkeep = spell.FManaUpkeep;
            AoWHSMap.Current.PlaceHX(Watcher, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
            if ((byte)AoWHSMap.Current.FSeatedPlayer == FEventPlayerIndex)
            {
                Watcher.StartAnimation(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
            }

            var fSpellControlSpell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, fSpellControlSpell.FBaseManaCost);
            return false;
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells
{

    public partial class Watcher : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1498
        public Watcher(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(WatcherRStr);
            FSpellID = 57;
            FAboveGroundOnly = false;
        }

        // GlobalTargetSpells.pas:1523
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FTargetMode = Set<TargetSelectionModeElement>.FromMask(TargetSelection.FTargetMode.Mask) - Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel, TargetSelectionModeElement.ValidateDistance);
            TargetSelection.FHNs.Clear();
        }

        // GlobalTargetSpells.pas:1506
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            bool Result;
            WatcherTE Event;
            if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new WatcherTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
    public partial class TornadoTE : GlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1559
        public new const int ClassID = 135972;

        public int FAngle;
        public Animation FAnimation;
        public ArmyID FArmyID;

        // GlobalTargetSpells.pas:1553
        public override void Dispose()
        {
            FArmyID?.Dispose();
            base.Dispose();
        }

        // GlobalTargetSpells.pas:1593
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FArmyID, null, 40);
        }

        // GlobalTargetSpells.pas:1547
        public TornadoTE(ECustomNode owner) : base(owner)
        {
            FArmyID = null;
        }

        // GlobalTargetSpells.pas:1660
        public override bool Process(AbstractUnit source, int value)
        {
            bool Result = default;
            UnitUserList MovedUnits = default;
            byte Owner = default;
            byte Selection = default;
            XYL Location = default;
            int Chance = default;
            bool Exposed = default;
            ArmyHS Army = default;
            AbstractUnit UnitObj = default;
            AoWMapField MapF = default;
            ArmyEventLog Log = default;
            ImageSequenceAnimation Anim = default;
            ArmyHS Placed = default;
            AIGroup Group = default;
            if (value == 0)
            {
                FAngle = 0;
                var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
                AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
                AoWHSMap.Current.ValidateObservation(source.FPlayer, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2);
                if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
                }

                Anim = (ImageSequenceAnimation)new Animation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = AoWHSMap.Current;
                Anim.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
                Anim.Activate();
            }

            if (value == 80)
            {
                MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
                if (FArmyID != null && !MapF.GetMagicShieldActive())
                {
                    FArmyID.RetrieveArmy(ref Army, ref Selection);
                    Exposed = !(Set<byte>.WithBounds(0, 255).With(1).Contains((byte)MapF.OverlayType) || MapF.FindNoneTransparentHS(typeof(Structure)) != null);
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

                            if (Chance < AoWHSMap.Current.Random(100))
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
                        Log = new ArmyEventLog(null);
                        Log.FPlayer = source.FPlayer;
                        var tornado = AbilityRules.TranslateRStr(GameText.TornadoRStr);
                        Log.FArmyText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [tornado]);
                        var tornadoText = AbilityRules.TranslateRStr(GameText.TornadoRStr);
                        Log.FArmyMessages.Add(tornadoText);
                        Log.FArmyIcon = 2;
                        Log.FArmyView.Setup(Army.FArmy, Selection);
                        var yhx = Army.Yhx;
                        var xhx = Army.Xhx;
                        EventLog.DistributeLocationEventLog(Log, (sbyte)FEventPlayerIndex, xhx, yhx, Army.Lhx, true);
                        Log.Release();
                        Location.Packed = ((Tornado)AoWHSSet.Current.FSpellControl.GetSpell(68)).CalculateWarpLocation(Army, Selection);
                        if (Location.Packed != 16777215)
                        {
                            if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
                            {
                                AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(1);
                            }

                            if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.VisibleForPlayer(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, AoWHSMap.Current.FSeatedPlayer))
                            {
                                if (AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1] != null)
                                {
                                    Anim = new ImageSequenceAnimation(null);
                                    Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1]);
                                    AoWHSMap.Current.PlaceHX(Anim, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
                                }
                            }

                            if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.VisibleForPlayer(Location.X, Location.Y, Location.L, AoWHSMap.Current.FSeatedPlayer))
                            {
                                if (AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1] != null)
                                {
                                    Anim = new ImageSequenceAnimation(null);
                                    Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[1]);
                                    AoWHSMap.Current.PlaceHX(Anim, Location.X, Location.Y, Location.L);
                                }
                            }

                            MovedUnits = new UnitUserList(null);
                            Owner = (byte)Army.FArmy.FPlayer;
                            MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Location.Packed);
                            if (Army.FArmy.ValidateSelection(255) != Selection)
                            {
                                for (int I = Army.FArmy.Count - 1; I >= 0; I--)
                                {
                                    if ((Selection & (1 << I)) != 0)
                                    {
                                        UnitObj = Army.FArmy.GetUnit(I);
                                        MovedUnits.AddUnit(UnitObj);
                                        Placed = UnitObj.Place(Location.X, Location.Y, Location.L, (sbyte)(byte)Army.FArmy.FPlayer, Set<UnitPlaceModeElement>.Of());
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
                                var unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(MovedUnits, 5);
                                Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(0, unitsToIndependentRelation, GroupBehavior.Hunt);
                                Group.AddUnits(MovedUnits);
                                ((HuntAG)Group).FTargetUnitIDs.Add(source.FUnitID);
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
        public void ShowAnimation(Animation Animation, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
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
            if (AoWHSMap.Current.ViewL == Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer)
            {
                HexGeometry.HXtoHP(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                FAngle = FAngle + 20 + FAnimation.FFrameCounter / 5;
                RandSeed = AoWHSMap.Current.FInitialRandomSeed;
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
                        if (GameTables.AoWEngine.FAnimationDetail == 0)
                        {
                            Last = 1;
                        }
                        else
                        {
                            if (GameTables.AoWEngine.FAnimationDetail == 1)
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
                            AoWHSSet.Current.FSpellControl.GetSpell(68).FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                            Angle = (float)((double)0.035 + Angle);
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:215
        public override bool Validate(Hero source) => base.Validate(source) && (FArmyID == null || FArmyID.Valid());
    }
}

namespace Aow.Game.Magic.Spells
{

    public partial class Tornado : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:1798
        public Tornado(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(GameText.TornadoRStr);
            FSpellID = 68;
            FAboveGroundOnly = true;
        }

        // GlobalTargetSpells.pas:1885
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            TargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
        }

        // GlobalTargetSpells.pas:1865
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            bool Result;
            TornadoTE Event;
            ArmyHS Army;
            if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new TornadoTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
                Army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetArmyHS();
                if (Army != null)
                {
                    Event.FArmyID = Army.FArmy.CreateID(255);
                }

                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        public bool ValidWarpLocation(ArmyHS Army, sbyte X, sbyte Y, sbyte Layer)
        {
            AoWMapField MapF;
            MapFieldMsg Msg = default;
            MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, Layer);
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
        public int CalculateWarpLocation(ArmyHS Army, byte Selection)
        {
            SpellMoveTable Table = default;
            HNWalkData Walk = default;
            MapLevel MapLevel;
            MapField MapF;
            DelphiList Locations;
            bool Found;
            int Index;
            int X;
            Locations = new DelphiList();
            Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref Table));
            var yhx = Army.Yhx;
            var xhx = Army.Xhx;
            HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 10);
            MapLevel = AoWHSMap.Current.MapContainer.MapLevels[Army.Lhx];
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

                HNWalkData.WalkToNextHN(ref Walk);
            }

            Found = false;
            int Result = 16777215;
            while (Locations.Count != 0 && !Found)
            {
                Index = Random(Locations.Count);
                MapF = (MapField)Locations[Index];
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
    public partial class VortexTE : GlobalTargetSpellTE
    {
        // GlobalTargetSpells.pas:1905
        public new const int ClassID = 135973;

        public int FAngle;
        public Animation FAnimation;
        public ArmyID FArmyID;

        // GlobalTargetSpells.pas:1899
        public override void Dispose()
        {
            FArmyID?.Dispose();
            base.Dispose();
        }

        // GlobalTargetSpells.pas:1939
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwCreateEObject(ref FArmyID, null, 40);
        }

        // GlobalTargetSpells.pas:1893
        public VortexTE(ECustomNode owner) : base(owner)
        {
            FArmyID = null;
        }

        // GlobalTargetSpells.pas:1991
        public override bool Process(AbstractUnit source, int value)
        {
            bool Result = default;
            Animation Anim = default;
            ArmyEventLog Log = default;
            byte Selection = default;
            bool Damaged = default;
            int Damage = default;
            ArmyHS Army = default;
            AbstractUnit UnitObj = default;
            if (value == 0)
            {
                FAngle = 0;
                var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
                AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
                AoWHSMap.Current.ValidateObservation(source.FPlayer, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2);
                if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
                }

                Anim = new Animation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = AoWHSMap.Current;
                Anim.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
                Anim.Activate();
            }

            if (value == 80)
            {
                if (FArmyID != null && FArmyID.RetrieveArmy(ref Army, ref Selection))
                {
                    Log = new ArmyEventLog(null);
                    Log.FArmyText = AbilityRules.TranslateRStr(VortexDamageRStr);
                    Log.FPlayer = source.FPlayer;
                    var vortex = AbilityRules.TranslateRStr(GameText.VortexRStr);
                    Log.FArmyMessages.Add(vortex);
                    Log.FArmyIcon = 2;
                    Log.FArmyView.Setup(Army.FArmy, 255);
                    Army.FArmy.BeginUpdate();
                    Damaged = false;
                    for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
                    {
                        UnitObj = Army.FArmy.GetUnit(I);
                        if (!UnitObj.GetMoveTypes().Contains(MoveType.Flying))
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

                            Damage = UnitObj.ExecuteDamageRole(8, Damage, Set<DamageType>.Of(DamageType.Physical));
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
                        EventLog.DistributeLocationEventLog(Log, Army.FArmy.FPlayer, xhx, yhx, Army.Lhx, true);
                    }

                    Log.Release();
                    Army.FArmy.Validate();
                    Army.FArmy.ValidateTerrainEx(((MapField)Army.Owner).TerrainType, ((MapField)Army.Owner).OverlayType, source.FPlayer);
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
        public void ShowAnimation(Animation Animation, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
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
            if (AoWHSMap.Current.ViewL == Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer)
            {
                HexGeometry.HXtoHP(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                FAngle = FAngle + 20 + FAnimation.FFrameCounter / 5;
                RandSeed = AoWHSMap.Current.FInitialRandomSeed;
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
                        if (GameTables.AoWEngine.FAnimationDetail == 0)
                        {
                            Count = 1;
                        }
                        else
                        {
                            if (GameTables.AoWEngine.FAnimationDetail == 1)
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
                            AoWHSSet.Current.FSpellControl.GetSpell(70).FCastImages.Show(0, Xhp + X, Yhp - Y, 0, SurfaceDesc);
                            Angle = (float)((double)0.035 + Angle);
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:242
        public override bool Validate(Hero source) => base.Validate(source) && (FArmyID == null || FArmyID.Valid());

        private static readonly string VortexDamageRStr = "Vortex Damage";
    }
}

namespace Aow.Game.Magic.Spells
{

    public partial class Vortex : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2069
        public Vortex(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(GameText.VortexRStr);
            FSpellID = 70;
            FAboveGroundOnly = false;
        }

        // GlobalTargetSpells.pas:2120
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            // { Use the incoming target. }
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FHNs.Clear();
            TargetSelection.FOnValidateTarget = ValidateTargetEvent;
        }

        // GlobalTargetSpells.pas:2100
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            bool Result;
            VortexTE Event;
            ArmyHS Army;
            if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new VortexTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
                Army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetArmyHS();
                if (Army != null)
                {
                    Event.FArmyID = Army.FArmy.CreateID(255);
                }

                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        public override bool ValidTargetSelection(TargetSelectionControl TargetSelection)
        {
            bool Result;
            MapField MapF;
            if (TargetSelection.FTargetX != -1)
            {
                MapF = AoWHSMap.Current.MapContainer.MapLevels[TargetSelection.FTargetL].GetField(TargetSelection.FTargetX, TargetSelection.FTargetY);
                Result = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)MapF.TerrainType);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalTargetSpells.pas:2088
        public void ValidateTargetEvent(TargetSelectionControl Target, sbyte X, sbyte Y, sbyte Layer, ref bool Valid)
        {
            MapLevel MapLevel;
            if (X != -1)
            {
                MapLevel = AoWHSMap.Current.MapContainer.MapLevels[Layer];
                var fieldValue = MapLevel.GetField(X, Y);
                Valid = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
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
    public partial class HealingShowersTE : GlobalTargetSpellTE
    {
        public HealingShowersTE(ECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:2138
        public new const int ClassID = 135975;

        public Animation FAnimation;

        // GlobalTargetSpells.pas:2190
        public override bool Process(AbstractUnit source, int value)
        {
            bool Result = default;
            AbstractUnit UnitObj = default;
            HNWalkData Walk = default;
            ArmyHS Army = default;
            MapLevel Level = default;
            ImageSequenceAnimation Anim = default;
            bool Healed = default;
            bool PlaySound = default;
            HexagonSprite Cloud = default;
            base.Process(source, value);
            if (value == 0)
            {
                AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2);
                if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.WatchingTerrain(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(73).FCastSounds.Play(0);
                }

                Anim = (ImageSequenceAnimation)new Animation(null);
                FAnimation = Anim;
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowAnimation;
                Anim.FAnimationMap = AoWHSMap.Current;
                Anim.FTag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
                Anim.Activate();
                AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            }

            if (value == 50)
            {
                PlaySound = false;
                AoWHSMap.Current.ChangeTerrainEx(null, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, null);
                Level = AoWHSMap.Current.MapContainer.MapLevels[Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer];
                HNWalkData.InitHNWalk(ref Walk, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, 0);
                while (Walk.Radius <= 1)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Level.Height > Walk.Y)
                    {
                        if (!((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetMagicShieldActive())
                        {
                            Healed = false;
                            Army = ((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetArmyHS();
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
                                    healed = AoWHSMap.Current.VisibleForPlayer(xhx, yhx, Army.Lhx, AoWHSMap.Current.FSeatedPlayer);
                                }

                                if (healed)
                                {
                                    bool condition = PlaySound || (byte)Army.FArmy.FPlayer == (byte)AoWHSMap.Current.FSeatedPlayer;
                                    if (!condition)
                                    {
                                        var yhx = Army.Yhx;
                                        var xhx = Army.Xhx;
                                        condition = AoWHSMap.Current.Watching(xhx, yhx, Army.Lhx, 1);
                                    }

                                    PlaySound = condition;
                                    Anim = new ImageSequenceAnimation(null);
                                    Anim.SetImageSequence(AoWHSSet.Current.FAbilityControl.GetAbility(47).FImageSequences.ImageSequences[0]);
                                    Anim.FAnimationLevel = 27;
                                    var armyYhx = Army.Yhx;
                                    var armyXhx = Army.Xhx;
                                    AoWHSMap.Current.PlaceHX(Anim, armyXhx, armyYhx, Army.Lhx);
                                }
                            }

                            Cloud = (HexagonSprite)Level.GetField(Walk.X, Walk.Y).FindOwnedChild(132990);
                            if (Cloud != null)
                            {
                                Cloud?.Dispose();
                            }
                        }
                    }

                    HNWalkData.WalkToNextHN(ref Walk);
                }

                if (PlaySound)
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(47).FSFXLibrary.Play(0);
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
        public void ShowAnimation(Animation Animation, int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
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
            if (AoWHSMap.Current.ViewL == Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer)
            {
                HexGeometry.HXtoHP(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, ref Xhp, ref Yhp);
                Xhp -= XhpSurf;
                Yhp -= YhpSurf;
                switch (GameTables.AoWEngine.FRegistry.GetDetailLevel())
                {
                    case DetailLevel.Low:
                        Count = 50;
                        break;
                    case DetailLevel.High:
                        Count = 160;
                        break;
                    default:
                        Count = 100;
                        break;
                }

                RandSeed = AoWHSMap.Current.FInitialRandomSeed;
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
                                AoWHSSet.Current.FSpellControl.GetSpell(73).FCastImages.Show(0, Xhp + XShift + 120, Yhp + YShift - 500 + 30, 0, SurfaceDesc);
                            }
                        }
                    }
                }
            }
        }

        // GlobalTargetSpells.pas:2181
        public void ChangeTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
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

namespace Aow.Game.Magic.Spells
{

    public partial class HealingShowers : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2277
        public HealingShowers(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(HealingShowersRStr);
            FSpellID = 73;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Water;
        }

        // GlobalTargetSpells.pas:2295
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            TargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:2308
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            HealingShowersTE Event;
            Event = new HealingShowersTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:2291
        public override bool ValidTargetSelectionMapF(MapField Field) => base.ValidTargetSelectionMapF(Field);

        // GlobalTargetSpells.pas:2287
        public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field);

        private static readonly string HealingShowersRStr = "Healing Showers";
    }

    public partial class SmokeCloudSpell : GlobalTargetSpell
    {
        // GlobalTargetSpells.pas:2325
        public SmokeCloudSpell(ECustomNode owner) : base(owner)
        {
            this.FSpellName = AbilityRules.TranslateRStr(GameText.SmokeCloudRStr);
            FSpellID = 74;
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = MagicSphere.Fire;
        }

        // GlobalTargetSpells.pas:2373
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            EResource Resource;
            SmokeCloud Cloud;
            var source = Event.GetSource();
            AoWHSMap.Current.ValidateObservation(source.FPlayer, Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).X, Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).Layer, 2);
            if (Event.GetSource().FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).X, Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).Layer, 2))
            {
                AoWHSSet.Current.FSpellControl.GetSpell(Event.FEventSpellID).FCastSounds.Play(0);
            }

            Resource = (EResource)((ECustomNode)(object)AoWHSSet.Current.MapObjectResourceList).FindChild(135747);
            if (Resource != null)
            {
                Cloud = (SmokeCloud)Resource.CreateResourceUser();
                Cloud.FPlayer = Source.FPlayer;
                Cloud.FTurnsRemaining = -1;
                Cloud.FEnchantment.FPlayerIndex = Source.FPlayer;
                Cloud.FEnchantment.FEnchantmentManaUpkeep = this.FManaUpkeep;
                Cloud.FDrifting = false;
                if (!AoWHSMap.Current.PlaceHX(Cloud, Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).X, Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref ((GlobalTargetSpellTE)Event).FLocation).Layer))
                {
                    Cloud?.Dispose();
                }

                AoWHSSet.Current.FSpellControl.GetSpell(this.FSpellID).CastingDone(Source, this.FBaseManaCost);
            }

            return false;
        }

        // GlobalTargetSpells.pas:2343
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            TargetSelectionControl Target;
            Target = TargetSelection;
            base.SetupTargetSelection(Source, Target);
            Target.FHNs.Clear();
            for (int I = 0, last = HexGeometry.RadToHN(4) - 1; I <= last; I++)
            {
                Target.FHNs.Add(0);
            }
        }

        // GlobalTargetSpells.pas:2356
        public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            GlobalTargetSpellTE Event;
            Event = new GlobalTargetSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            return true;
        }

        // GlobalTargetSpells.pas:2339
        public override bool ValidTargetSelectionMapF(MapField Field) => base.ValidTargetSelectionMapF(Field);

        // GlobalTargetSpells.pas:2335
        public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field);
    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public partial class GlobalTargetSpellCalls : Spell
    {
        public GlobalTargetSpellCalls(ECustomNode owner) : base(owner)
        { }

        // GlobalTargetSpells.pas:303
        public virtual void ShowSelection(TargetSelectionControl Target, int HN, int X, int Y, in SurfaceDesc SD)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:305
        public virtual void SelectionChanged()
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:306
        public virtual void SetupSelection(int Caster, int Target)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:307
        public virtual bool Selected(int Caster, int X, int Y, int Layer)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:308
        public virtual bool ValidSelection(int Target)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:309
        public virtual bool ValidSelectionMapF(int MapF)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }

        // GlobalTargetSpells.pas:310
        public virtual bool ValidMapF(int MapF)
        {
            throw AbstractErrorException.Create("Abstract Error");
        }
    }

    public struct SpellNameView
    {
        public FixedArray_byte_8 Reserved;
        private string name;

        public string Name { readonly get => name ?? ""; set => name = value; }
    }

    public partial class TargetSelectionView
    {
        public CustomImageLibrary ImageLibrary;
        public byte Status;
        public Set<TargetSelectionModeElement> Mode;
        public ShowTargetSelectionEvent OnShow;
        public ValidateTargetEvent OnValidate;
        public int Distance;
        public int CenterX;
        public int CenterY;
        public int CenterLayer;
        public EventList Changes;
        public TargetSelectionChange Change;
        public sbyte X;
        public sbyte Y;
        public sbyte Layer;
        public StringCollection Text;
        public string Caption = "";
        public string CancelCaption = "";
        public LibraryImage Preview;
        public LibraryImage ValidPreview;
        public LibraryImage InvalidPreview;
    }

    public struct TargetSelectionFields
    {
        public object VMT;
        public CustomImageLibrary ImageLibrary;
        public int Reserved08;
        public byte Status;
        public Set<TargetSelectionModeElement> Mode;
        public ushort Padding0E;
        public ShowTargetSelectionEvent OnShow;
        public ValidateTargetEvent OnValidate;
        public int Distance;
        public int CenterX;
        public int CenterY;
        public int CenterLayer;
        public int Reserved30;
        public EventList Changes;
        public TargetSelectionChange Change;
        public sbyte X;
        public sbyte Y;
        public sbyte Layer;
        public StringCollection Text;
        private string caption;

        public string Caption { readonly get => caption ?? ""; set => caption = value; }

        private string cancelCaption;

        public string CancelCaption { readonly get => cancelCaption ?? ""; set => cancelCaption = value; }

        public int Reserved48;
        public int Reserved4C;
        public LibraryImage Preview;
        public LibraryImage ValidPreview;
        public LibraryImage InvalidPreview;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct MapSeatedPlayerView
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    // { Typed call-only methods retain original argument evaluation order without
    // altering the foreign headers or their implementation signatures. }
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct SpellLocation
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
    public struct MapFieldFlagView
    {
        [FieldOffset(0)]
        public FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }
}

namespace Aow.Game.Magic.Spells
{

    [StructLayout(LayoutKind.Explicit, Size = 257)]
    public struct SpellMoveTable
    {
        [FieldOffset(0)]
        public byte Flags;
        [FieldOffset(1)]
        public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Cost;
    }
}

namespace Aow.Game.Magic.Spells.GlobalTarget
{

    public struct SpellAnimationFields
    {
        public FixedArray_byte_36 Reserved;
        public ShowAnimationEvent OnShow;
    }

    [StructLayout(LayoutKind.Explicit, Size = 110)]
    public struct SpellEngineFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_109 Reserved;
        [FieldOffset(109)]
        public byte Detail;
    }

    public struct SpellAIGroupFields
    {
        public FixedArray_byte_80 Reserved;
        public IntegerList Enemies;
    }

    public struct SpellRegistryFields
    {
        public FixedArray_byte_92 Reserved;
        public AoWRegistry Registry;
    }

    public struct SpellEngineSetFields
    {
        public FixedArray_byte_100 Reserved;
        public AoWHSSet HSSet;
    }
}
