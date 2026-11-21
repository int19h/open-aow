#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

public class FireBarrier : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:882
    public FireBarrier(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_fireBarrierText);
        SpellId = 11;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Fire;
    }

    // GlobalTargetSpells.pas:914
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

    // GlobalTargetSpells.pas:926
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        FireBarrierTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, layer: (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:900
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        bool result = base.ValidTargetSelectionMapField(field) && field.TerrainType is not ((sbyte)TerrainType.Water or (sbyte)TerrainType.Ice or (sbyte)TerrainType.UndergroundWater or (sbyte)TerrainType.UndergroundIce) && field.FindNoneTransparentHexSprite(typeof(Structure)) is null;
        if (result)
        {
            ArmyHexSprite army = field.GetArmyHexSprite();
            if (army is not null)
            {
                return !army.VisibleForSeatedPlayer();
            }
        }

        return result;
    }

    // GlobalTargetSpells.pas:892
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field) && field.TerrainType is not ((sbyte)TerrainType.Water or (sbyte)TerrainType.Ice or (sbyte)TerrainType.UndergroundWater or (sbyte)TerrainType.UndergroundIce) && field.FindNoneTransparentHexSprite(typeof(Structure)) is null && field.GetArmyHexSprite() is null;

    private static readonly string s_fireBarrierText = "Fire Barrier";
}
