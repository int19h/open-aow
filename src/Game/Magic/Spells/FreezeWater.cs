// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

public partial class FreezeWater : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:710
    public FreezeWater(ECustomNode owner) : base(owner)
    {
        this.SpellName = AbilityRules.TranslateResourceString(s_freezeWaterText);
        SpellId = 10;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Water;
    }

    // GlobalTargetSpells.pas:811
    // { Original Boolean-return ABI; foreign P7 overrides remain procedures. }
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

    // GlobalTargetSpells.pas:823
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        FreezeWaterTokenEvent eventValue;
        eventValue = new FreezeWaterTokenEvent(null);
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    public override bool ValidTargetMapField(MapField field)
    {
        if (!base.ValidTargetMapField(field)) return false;
        sbyte terrain = field._terrainType;
        return (terrain == 0 || terrain == 10) && field.FindNoneTransparentHexSprite(typeof(Structure)) == null && ((AoWMapField)field).GetArmyHexSprite() == null;
    }

    public override bool ValidTargetSelectionMapField(MapField field)
    {
        if (!base.ValidTargetSelectionMapField(field)) return false;
        sbyte terrain = field._terrainType;
        if ((terrain != 0 && terrain != 10) || field.FindNoneTransparentHexSprite(typeof(Structure)) != null) return false;
        var army = ((AoWMapField)field).GetArmyHexSprite();
        return army == null || !army.VisibleForSeatedPlayer();
    }

    private static readonly string s_freezeWaterText = "Freeze Water";
}
