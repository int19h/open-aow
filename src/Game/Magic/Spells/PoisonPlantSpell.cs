#nullable disable
// Translated from PoisonP.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

public class PoisonPlantSpell : GlobalTargetSpell
{
    public PoisonPlantSpell(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_poisonPlantsText);
        SpellId = 12;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Earth;
        AboveGroundOnly = false;
    }

    // PoisonP.pas:405
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; i <= last; i++)
        {
            _ = targetSelection.HexNumbers.Add(0);
        }
    }

    // PoisonP.pas:414
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        PoisonPlantSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, layer: (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // PoisonP.pas:390
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        bool result = base.ValidTargetSelectionMapField(field) && field.TerrainType is (>= (sbyte)TerrainType.Grass and <= (sbyte)TerrainType.Wasteland) or (sbyte)TerrainType.Dirt;
        // { Original precedence also accepts these overlays without the terrain test. }
        result = (result && field.OverlayType == -1) || (byte)field.OverlayType is (>= 1 and <= 4) or 9;
        result = result && field.FindNoneTransparentHexSprite(typeof(Structure)) is null && field.GetArmyHexSprite() is null;
        if (result)
        {
            ArmyHexSprite armyHexSprite = field.GetArmyHexSprite();
            if (armyHexSprite is not null)
            {
                return !armyHexSprite.VisibleForSeatedPlayer();
            }
        }

        return result;
    }

    // PoisonP.pas:382
    public override bool ValidTargetMapField(MapField field)
    {
        bool result = base.ValidTargetMapField(field) && field.TerrainType is (>= (sbyte)TerrainType.Grass and <= (sbyte)TerrainType.Wasteland) or (sbyte)TerrainType.Dirt;
        // { Original precedence also accepts these overlays without the terrain test. }
        result = (result && field.OverlayType == -1) || (byte)field.OverlayType is (>= 1 and <= 4) or 9;
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) is null && field.GetArmyHexSprite() is null;
    }

    private static readonly string s_poisonPlantsText = "Poison Plants";
}
