// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

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
    public override bool CanActivate(Troop Source, ref string MessageText)
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
    public override bool Activate(Troop Source)
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
    public virtual void ShowTargetSelection(TargetSelectionControl Target, int HN, int Xs, int Ys, in LockedSurface SurfaceDesc)
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
                Target.FImageLibrary[0].ShowBlended(Xs - 8, Ys, SurfaceDesc, GraphicsState.IntensityBlendTable.LookupTables[Intensity]);
            }
            else
            {
                Target.FImageLibrary[1].Show(Xs - 8, Ys, SurfaceDesc);
            }
        }
    }

    // GlobalTargetSpells.pas:521
    public virtual void SetupTargetSelection(Troop Source, TargetSelectionControl TargetSelection)
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
    public virtual bool TargetSelected(Troop Source, int X, int Y, int L) => false;

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

    internal static bool GlobalTargetCanActivate(GlobalTargetSpell Spell, Troop Caster, ref string MessageText)
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

    internal static bool GlobalTargetActivate(GlobalTargetSpell Spell, Troop Caster, int Unused)
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
