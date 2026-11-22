#nullable disable warnings
// Translated from HolyG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class HolyGroundSpell : GlobalTargetSpell
{
    public HolyGroundSpell(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_holyGroundsText);
        SpellId = 13;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Life;
        AboveGroundOnly = false;
    }

    // HolyG.pas:375
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; i <= last; i++)
        {
            _ = targetSelection.HexNumbers.Add(0);
        }
    }

    // HolyG.pas:384
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        HolyGroundSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
        eventValue.SetLocation(X.TruncateToSByte(), Y.TruncateToSByte(), layer: L.TruncateToSByte());
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // HolyG.pas:360
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        bool result = base.ValidTargetMapField(field) && field.TerrainType is (>= (sbyte)TerrainType.Grass and <= (sbyte)TerrainType.Wasteland) or (sbyte)TerrainType.Dirt;
        // { Original overlay test allows these overlays even when the terrain check failed. }
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

    // HolyG.pas:350
    public override bool ValidTargetMapField(MapField field)
    {
        bool result = base.ValidTargetMapField(field) && field.TerrainType is (>= (sbyte)TerrainType.Grass and <= (sbyte)TerrainType.Wasteland) or (sbyte)TerrainType.Dirt;
        // { Original overlay test allows these overlays even when the terrain check failed. }
        result = (result && field.OverlayType == -1) || (byte)field.OverlayType is (>= 1 and <= 4) or 9;
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) is null && field.GetArmyHexSprite() is null;
    }

    private static readonly string s_holyGroundsText = "Holy Woods";
}
