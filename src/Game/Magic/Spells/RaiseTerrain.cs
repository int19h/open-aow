// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class RaiseTerrain : GlobalTargetSpell
{
    public RaiseTerrain(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_raiseTerrainText);
        SpellId = 19;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Earth;
        AboveGroundOnly = true;
    }

    // Mountain.pas:327

    // Mountain.pas:364
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; I <= last; I++)
        {
            targetSelection.HexNumbers.Add(0);
        }
    }

    // Mountain.pas:373
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        RaiseTerrainTokenEvent eventValue;
        eventValue = new RaiseTerrainTokenEvent(null);
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    // Mountain.pas:347
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        bool valid;
        ArmyHexSprite army;
        valid = base.ValidTargetSelectionMapField(field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)field.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).Contains((byte)field.OverlayType) && field.FindNoneTransparentHexSprite(typeof(Structure)) == null;
        if (valid)
        {
            army = field.GetArmyHexSprite();
            if (army != null)
            {
                valid = !army.VisibleForSeatedPlayer();
            }
        }

        return valid;
    }

    // Mountain.pas:338
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)field.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).Contains((byte)field.OverlayType) && field.FindNoneTransparentHexSprite(typeof(Structure)) == null && field.GetArmyHexSprite() == null;

    private static readonly string s_raiseTerrainText = "Raise Terrain";

    // Mountain.pas:77

    // Mountain.pas:77

}
