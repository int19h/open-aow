#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class FreezeWater : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:710
    public FreezeWater(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_freezeWaterText);
        SpellId = 10;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Water;
    }

    // GlobalTargetSpells.pas:811
    // { Original Boolean-return ABI; foreign P7 overrides remain procedures. }
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

    // GlobalTargetSpells.pas:823
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        FreezeWaterTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
        eventValue.SetLocation(X.TruncateToSByte(), Y.TruncateToSByte(), layer: L.TruncateToSByte());
        GetEngine().StrategicMap.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    public override bool ValidTargetMapField(MapField field)
    {
        if (!base.ValidTargetMapField(field))
        {
            return false;
        }

        TerrainType terrain = field.Terrain;
        return (terrain is TerrainType.Water or TerrainType.UndergroundWater) && field.FindNoneTransparentHexSprite(typeof(Structure)) is null && field.GetArmyHexSprite() is null;
    }

    public override bool ValidTargetSelectionMapField(MapField field)
    {
        if (!base.ValidTargetSelectionMapField(field))
        {
            return false;
        }

        TerrainType terrain = field.Terrain;
        if ((terrain is not TerrainType.Water and not TerrainType.UndergroundWater) || field.FindNoneTransparentHexSprite(typeof(Structure)) is not null)
        {
            return false;
        }

        ArmyHexSprite army = field.GetArmyHexSprite();
        return army is null || !army.VisibleForSeatedPlayer();
    }

    private static readonly string s_freezeWaterText = "Freeze Water";
}
