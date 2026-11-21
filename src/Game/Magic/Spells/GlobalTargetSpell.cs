#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalTargetSpells.pas.
using System.Diagnostics.CodeAnalysis;
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
public class GlobalTargetSpell : Spell
{

    private int _casterId;
    private sbyte _casterPlayer;

    public bool AboveGroundOnly { get; protected init; }

    // GlobalTargetSpells.pas:439
    public GlobalTargetSpell(CustomNode owner) : base(owner) => SpellType = SpellType.Global;

    // GlobalTargetSpells.pas:569
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool result = base.CanActivate(source, ref messageText);
        if (result && AboveGroundOnly)
        {
            MapCoordinate coordinate68 = source.GetLocation();
            _ = coordinate68.X;
            _ = coordinate68.Y;
            sbyte layer = coordinate68.MapLevel;
            if (layer != 0)
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotCastSpellInUndergroundText);
            }
        }

        return result;
    }

    // GlobalTargetSpells.pas:618
    public override bool Activate(Troop source)
    {
        string messageText = "";
        _ = base.Activate(source);
        if (CanActivate(source, ref messageText))
        {
            _casterId = source.UnitId;
            _casterPlayer = source.Player;
            SetupTargetSelection(source, StrategicMap.Current.TargetSelection);
            StrategicMap.Current.TargetSelection.Activate();
            return true;
        }
        else
        {
            return false;
        }
    }

    // GlobalTargetSpells.pas:445
    public virtual void ShowTargetSelection(TargetSelectionControl target, int hexNumber, int xs, int ys, in LockedSurface lockedSurface)
    {
        int x = default;
        int y = default;
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[target.Target.MapLevel];
        HexGeometry.CenterHexNumberToHex(target.Target.X, target.Target.Y, hexNumber, ref x, ref y);
        // { Original accepts Width/Height themselves (strict > bounds test). }
        if (x >= 0 && y >= 0 && x <= level.Width && y <= level.Height)
        {
            MapField mapField = level.Field[x, y];
            if (ValidTargetSelectionMapField(mapField))
            {
                int radius = HexGeometry.HexNumberToRadius(hexNumber);
                int intensity = (Math.Abs(((StrategicMap.Current.Frame + radius) % 9) - 4) * 20) + 20;
                target.ImageLibrary[0].ShowBlended(xs - 8, ys, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[intensity]);
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
        targetSelection.Initialize(source);
        targetSelection.TargetMode |= TargetSelectionModes.OwnerDraw | TargetSelectionModes.ValidateDistance | TargetSelectionModes.ExternalFloater;
        MapCoordinate coordinate69 = source.GetLocation();
        sbyte unitX = coordinate69.X;
        sbyte unitY = coordinate69.Y;
        sbyte unitLayer = coordinate69.MapLevel;
        targetSelection.SourceLocation = new MapCoordinate(unitX, unitY, unitLayer);
        targetSelection.Range = 12;
        targetSelection.Changed += OnTargetSelectionChange;
        targetSelection.SetShowHandler(ShowTargetSelection);
        string format = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SelectTargetLocationForOnMapFormat), args: [SpellName]);
        targetSelection.Text.Text = format;
        targetSelection.Title = SpellName;
        targetSelection.CancelText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CancelText);
        targetSelection.SourceImage = GamePack.Current.SpellControl[SpellId].CastImages.ImageSequences[20].GetPreviewImage();
        targetSelection.ValidTargetImage = GamePack.Current.SpellControl[SpellId].CastImages.ImageSequences[21].GetPreviewImage();
        targetSelection.InvalidTargetImage = GamePack.Current.SpellControl[SpellId].CastImages.ImageSequences[22].GetPreviewImage();
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
        Troop? caster = StrategicMap.Current.UnitControl.FindUnit(_casterId);
        if (caster is null || target.Change == TargetSelectionChange.Deactivated)
        {
            target.Changed -= OnTargetSelectionChange;
            target.Deactivate();
        }
        else if (target.Change == TargetSelectionChange.Selected && ValidTargetSelection(target) &&
            TargetSelected(caster, target.Target.X, target.Target.Y, L: target.Target.MapLevel))
        {
            target.Changed -= OnTargetSelectionChange;
            target.Deactivate();
        }
    }

    internal static bool GlobalTargetCanActivate(GlobalTargetSpell spell, Troop caster, ref string messageText)
    {
        bool result = spell.CanActivateStatic(caster, ref messageText);
        if (result && spell.AboveGroundOnly)
        {
            MapCoordinate coordinate70 = caster.GetLocation();
            _ = coordinate70.X;
            _ = coordinate70.Y;
            sbyte layer = coordinate70.MapLevel;
            if (layer != 0)
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(spell.GetEngine(), S: GameText.CannotCastSpellInUndergroundText);
            }
        }

        return result;
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi signature keeps unused on purpose.")]
    internal static bool GlobalTargetActivate(GlobalTargetSpell spell, Troop caster, int unused)
    {
        string messageText = "";
        _ = spell.ActivateStatic(caster);
        if (spell.CanActivate(caster, ref messageText))
        {
            spell._casterId = caster.UnitId;
            spell._casterPlayer = caster.Player;
            spell.SetupTargetSelection(caster, StrategicMap.Current.TargetSelection);
            StrategicMap.Current.TargetSelection.Activate();
            return true;
        }
        else
        {
            return false;
        }
    }
}
