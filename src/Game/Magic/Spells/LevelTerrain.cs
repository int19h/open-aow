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
    public LevelTerrain(ECustomNode owner) : base(owner)
    {
        this.SpellName = AbilityRules.TranslateResourceString(s_levelTerrainText);
        SpellId = 18;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
    }

    // GlobalTargetSpells.pas:1010
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

    // GlobalTargetSpells.pas:1022
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        LevelTerrainTokenEvent eventValue;
        eventValue = new LevelTerrainTokenEvent(null);
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:996
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        ArmyHexSprite army;
        bool result = base.ValidTargetSelectionMapField(field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)field.TerrainType) && field.FindNoneTransparentHexSprite(typeof(Structure)) == null;
        if (result)
        {
            army = ((AoWMapField)field).GetArmyHexSprite();
            if (army != null)
            {
                result = !army.VisibleForSeatedPlayer();
            }
        }

        return result;
    }

    // GlobalTargetSpells.pas:988
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)field.TerrainType) && field.FindNoneTransparentHexSprite(typeof(Structure)) == null && ((AoWMapField)field).GetArmyHexSprite() == null;

    private static readonly string s_levelTerrainText = "Level Terrain";
}
