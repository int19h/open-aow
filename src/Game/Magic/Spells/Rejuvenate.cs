#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class Rejuvenate : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:1074
    public Rejuvenate(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_rejuvenateText);
        SpellId = 41;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Life;
        AboveGroundOnly = true;
    }

    // GlobalTargetSpells.pas:1095
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target = targetSelection;
        base.SetupTargetSelection(source, targetSelection: target);
        target.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(3) - 1; i <= last; i++)
        {
            _ = target.HexNumbers.Add(0);
        }
    }

    // GlobalTargetSpells.pas:1107
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        RejuvenateTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
        eventValue.SetLocation(X.TruncateToSByte(), Y.TruncateToSByte(), layer: L.TruncateToSByte());
        GetEngine().StrategicMap.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:1090
    public override bool ValidTargetSelectionMapField(MapField field) => base.ValidTargetSelectionMapField(field) && field.Terrain is >= TerrainType.Desert and <= TerrainType.Wasteland;

    // GlobalTargetSpells.pas:1085
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field) && field.Terrain is >= TerrainType.Desert and <= TerrainType.Wasteland;

    private static readonly string s_rejuvenateText = "Rejuvenate";
}
