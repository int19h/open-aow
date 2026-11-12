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

    private int _casterId;
    private sbyte _casterPlayer;

    public bool AboveGroundOnly { get; protected set; }

    // GlobalTargetSpells.pas:439
    public GlobalTargetSpell(ECustomNode owner) : base(owner)
    {
        SpellType = SpellType.Global;
    }

    // GlobalTargetSpells.pas:569
    public override bool CanActivate(Troop source, ref string messageText)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        bool result = base.CanActivate(source, ref messageText);
        if (result && AboveGroundOnly)
        {
            source.GetLocation(ref X, ref Y, ref layer);
            if (layer != 0)
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(GameText.CannotCastSpellInUndergroundText);
            }
        }

        return result;
    }

    // GlobalTargetSpells.pas:618
    public override bool Activate(Troop source)
    {
        bool result;
        string messageText = "";
        base.Activate(source);
        if (CanActivate(source, ref messageText))
        {
            _casterId = source.UnitId;
            _casterPlayer = source.Player;
            SetupTargetSelection(source, StrategicMap.Current.TargetSelection);
            StrategicMap.Current.TargetSelection.Activate();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // GlobalTargetSpells.pas:445
    public virtual void ShowTargetSelection(TargetSelectionControl target, int hexNumber, int xs, int ys, in LockedSurface lockedSurface)
    {
        MapLevel level;
        MapField mapField;
        int X = default;
        int Y = default;
        int intensity;
        int radius;
        level = StrategicMap.Current.MapContainer.MapLevels[target.TargetL];
        HexGeometry.CenterHexNumberToHex(target.TargetX, target.TargetY, hexNumber, ref X, ref Y);
        // { Original accepts Width/Height themselves (strict > bounds test). }
        if (X >= 0 && Y >= 0 && X <= level.Width && Y <= level.Height)
        {
            mapField = level.Field[X, Y];
            if (this.ValidTargetSelectionMapField(mapField))
            {
                radius = HexGeometry.HexNumberToRadius(hexNumber);
                intensity = Math.Abs((StrategicMap.Current.Frame + radius) % 9 - 4) * 20 + 20;
                target.ImageLibrary[0].ShowBlended(xs - 8, ys, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[intensity]);
            }
            else
            {
                target.ImageLibrary[1].Show(xs - 8, ys, lockedSurface);
            }
        }
    }

    // GlobalTargetSpells.pas:521
    public virtual void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        sbyte unitX = default;
        sbyte unitY = default;
        sbyte unitLayer = default;
        targetSelection.Initialize(source);
        targetSelection.TargetMode = targetSelection.TargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.OwnerDraw, TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
        source.GetLocation(ref unitX, ref unitY, ref unitLayer);
        targetSelection.SourceX = unitX;
        targetSelection.SourceY = unitY;
        targetSelection.SourceL = unitLayer;
        targetSelection.Range = 12;
        targetSelection.FOnChange += OnTargetSelectionChange;
        targetSelection.OnShow = ShowTargetSelection;
        var format = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.SelectTargetLocationForOnMapFormat), [SpellName]);
        targetSelection.Text.Text = format;
        targetSelection.Title = SpellName;
        targetSelection.CancelText = AbilityRules.TranslateResourceString(GameText.CancelText);
        targetSelection.SourceImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[20].GetPreviewImage();
        targetSelection.ValidTargetImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[21].GetPreviewImage();
        targetSelection.InvalidTargetImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[22].GetPreviewImage();
    }

    // GlobalTargetSpells.pas:544
    public virtual bool TargetSelected(Troop source, int X, int Y, int L) => false;

    // GlobalTargetSpells.pas:487
    public virtual bool ValidTargetSelection(TargetSelectionControl targetSelection) => true;

    // GlobalTargetSpells.pas:475
    public virtual bool ValidTargetSelectionMapField(MapField field)
    {
        if (StrategicMap.Current.ExplorationEnabled && !field.GetExplored(_casterPlayer))
        {
            // Preserve the result for callers.
            return false;
        }

        return true;
    }

    // GlobalTargetSpells.pas:469
    public virtual bool ValidTargetMapField(MapField field) => true;

    // GlobalTargetSpells.pas:492. The recovered view aliases this control.
    public virtual void OnTargetSelectionChange(TargetSelectionControl target)
    {
        var caster = StrategicMap.Current.UnitControl.FindUnit(_casterId);
        if (caster is null || target.Change == TargetSelectionChange.Deactivated)
        {
            target.FOnChange -= OnTargetSelectionChange;
            target.Deactivate();
        }
        else if (target.Change == TargetSelectionChange.Selected && ValidTargetSelection(target) &&
            TargetSelected(caster, target.TargetX, target.TargetY, target.TargetL))
        {
            target.FOnChange -= OnTargetSelectionChange;
            target.Deactivate();
        }
    }

    internal static bool GlobalTargetCanActivate(GlobalTargetSpell spell, Troop caster, ref string messageText)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        bool result = spell.CanActivateStatic(caster, ref messageText);
        if (result && spell.AboveGroundOnly)
        {
            caster.GetLocation(ref X, ref Y, ref layer);
            if (layer != 0)
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(GameText.CannotCastSpellInUndergroundText);
            }
        }

        return result;
    }

    internal static bool GlobalTargetActivate(GlobalTargetSpell spell, Troop caster, int unused)
    {
        bool result;
        string messageText = "";
        spell.ActivateStatic(caster);
        if (spell.CanActivate(caster, ref messageText))
        {
            spell._casterId = caster.UnitId;
            spell._casterPlayer = caster.Player;
            spell.SetupTargetSelection(caster, StrategicMap.Current.TargetSelection);
            StrategicMap.Current.TargetSelection.Activate();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

}
