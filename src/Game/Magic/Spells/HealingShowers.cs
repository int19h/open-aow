// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class HealingShowers : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:2277
    public HealingShowers(ECustomNode owner) : base(owner)
    {
        this.SpellName = AbilityRules.TranslateResourceString(s_healingShowersText);
        SpellId = 73;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Water;
    }

    // GlobalTargetSpells.pas:2295
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target;
        target = targetSelection;
        base.SetupTargetSelection(source, target);
        target.HexNumbers.Clear();
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; I <= last; I++)
        {
            target.HexNumbers.Add(0);
        }
    }

    // GlobalTargetSpells.pas:2308
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        HealingShowersTokenEvent eventValue;
        eventValue = new HealingShowersTokenEvent(null);
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:2291
    public override bool ValidTargetSelectionMapField(MapField field) => base.ValidTargetSelectionMapField(field);

    // GlobalTargetSpells.pas:2287
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field);

    private static readonly string s_healingShowersText = "Healing Showers";
}
