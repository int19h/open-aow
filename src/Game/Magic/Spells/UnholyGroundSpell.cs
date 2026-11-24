#nullable disable warnings
// Translated from UnHolyG.pas.
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

public class UnholyGroundSpell : GlobalTargetSpell
{
    public UnholyGroundSpell(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_unholyGroundsText);
        SpellId = 14;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Death;
        AboveGroundOnly = false;
    }

    // UnHolyG.pas:383
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; i <= last; i++)
        {
            _ = targetSelection.HexNumbers.Add(0);
        }
    }

    // UnHolyG.pas:392
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        UnholyGroundSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
        eventValue.SetLocation(X.TruncateToSByte(), Y.TruncateToSByte(), layer: L.TruncateToSByte());
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // UnHolyG.pas:368
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        bool result = base.ValidTargetMapField(field) && field.TerrainType is (>= (sbyte)TerrainType.Grass and <= (sbyte)TerrainType.Wasteland) or (sbyte)TerrainType.Dirt;
        // { Original precedence accepts these overlays without the terrain test. }
        result = (result && field.OverlayType == -1) || (byte)field.OverlayType is (>= 1 and <= 4) or 9;
        result = result && field.FindNoneTransparentHexSprite(typeof(Structure)) is null;
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

    // UnHolyG.pas:360
    public override bool ValidTargetMapField(MapField field)
    {
        bool result = base.ValidTargetMapField(field) && field.TerrainType is (>= (sbyte)TerrainType.Grass and <= (sbyte)TerrainType.Wasteland) or (sbyte)TerrainType.Dirt;
        // { Original precedence accepts these overlays without the terrain test. }
        result = (result && field.OverlayType == -1) || (byte)field.OverlayType is (>= 1 and <= 4) or 9;
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) is null && field.GetArmyHexSprite() is null;
    }

    private static readonly string s_unholyGroundsText = "Evil Woods";
}
