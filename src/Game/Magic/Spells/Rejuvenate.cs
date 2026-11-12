// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Rejuvenate : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:1074
    public Rejuvenate(ECustomNode owner) : base(owner)
    {
        this.SpellName = AbilityRules.TranslateResourceString(s_rejuvenateText);
        SpellId = 41;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Life;
        AboveGroundOnly = true;
    }

    // GlobalTargetSpells.pas:1095
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target;
        target = targetSelection;
        base.SetupTargetSelection(source, target);
        target.HexNumbers.Clear();
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(3) - 1; I <= last; I++)
        {
            target.HexNumbers.Add(0);
        }
    }

    // GlobalTargetSpells.pas:1107
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        RejuvenateTokenEvent eventValue;
        eventValue = new RejuvenateTokenEvent(null);
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:1090
    public override bool ValidTargetSelectionMapField(MapField field) => base.ValidTargetSelectionMapField(field) && Set<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)field.TerrainType);

    // GlobalTargetSpells.pas:1085
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field) && Set<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)field.TerrainType);

    private static readonly string s_rejuvenateText = "Rejuvenate";
}
