#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class HealingShowers : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:2277
    public HealingShowers(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_healingShowersText);
        SpellId = 73;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Water;
    }

    // GlobalTargetSpells.pas:2295
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target = targetSelection;
        base.SetupTargetSelection(source, targetSelection: target);
        target.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; i <= last; i++)
        {
            _ = target.HexNumbers.Add(0);
        }
    }

    // GlobalTargetSpells.pas:2308
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        HealingShowersTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, layer: (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:2291
    public override bool ValidTargetSelectionMapField(MapField field) => base.ValidTargetSelectionMapField(field);

    // GlobalTargetSpells.pas:2287
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field);

    private static readonly string s_healingShowersText = "Healing Showers";
}
