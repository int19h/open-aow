#nullable disable warnings
// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

public class RaiseTerrain : GlobalTargetSpell
{
    public RaiseTerrain(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_raiseTerrainText);
        SpellId = 19;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Earth;
        AboveGroundOnly = true;
    }

    // Mountain.pas:364
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; i <= last; i++)
        {
            _ = targetSelection.HexNumbers.Add(0);
        }
    }

    // Mountain.pas:373
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        RaiseTerrainTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, layer: (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // Mountain.pas:347
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        bool valid = base.ValidTargetSelectionMapField(field) && field.TerrainType is not ((sbyte)TerrainType.Water or (sbyte)TerrainType.Ice or (sbyte)TerrainType.UndergroundWater or (sbyte)TerrainType.UndergroundIce) && (byte)field.OverlayType != 0 && field.FindNoneTransparentHexSprite(typeof(Structure)) is null;
        if (valid)
        {
            ArmyHexSprite army = field.GetArmyHexSprite();
            if (army is not null)
            {
                valid = !army.VisibleForSeatedPlayer();
            }
        }

        return valid;
    }

    // Mountain.pas:338
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field) && field.TerrainType is not ((sbyte)TerrainType.Water or (sbyte)TerrainType.Ice or (sbyte)TerrainType.UndergroundWater or (sbyte)TerrainType.UndergroundIce) && (byte)field.OverlayType != 0 && field.FindNoneTransparentHexSprite(typeof(Structure)) is null && field.GetArmyHexSprite() is null;

    private static readonly string s_raiseTerrainText = "Raise Terrain";
}
