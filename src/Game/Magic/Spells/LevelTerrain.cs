#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class LevelTerrain : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:978
    public LevelTerrain(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_levelTerrainText);
        SpellId = 18;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
    }

    // GlobalTargetSpells.pas:1010
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target = targetSelection;
        base.SetupTargetSelection(source, target);
        target.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; i <= last; i++)
        {
            _ = target.HexNumbers.Add(0);
        }
    }

    // GlobalTargetSpells.pas:1022
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        LevelTerrainTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:996
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        bool result = base.ValidTargetSelectionMapField(field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)field.TerrainType) && field.FindNoneTransparentHexSprite(typeof(Structure)) is null;
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

    // GlobalTargetSpells.pas:988
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)field.TerrainType) && field.FindNoneTransparentHexSprite(typeof(Structure)) is null && field.GetArmyHexSprite() is null;

    private static readonly string s_levelTerrainText = "Level Terrain";
}
