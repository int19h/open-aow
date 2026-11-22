#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class Vortex : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:2069
    public Vortex(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.VortexText);
        SpellId = 70;
        AboveGroundOnly = false;
    }

    // GlobalTargetSpells.pas:2120
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        // { Use the incoming target. }
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        targetSelection.SetValidateTargetHandler(ValidateTargetEvent);
    }

    // GlobalTargetSpells.pas:2100
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            VortexTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
            eventValue.SetLocation(X.TruncateToSByte(), Y.TruncateToSByte(), layer: (sbyte)L);
            ArmyHexSprite army = StrategicMap.Current.MapContainer.GetField(X, Y, L).GetArmyHexSprite();
            if (army is not null)
            {
                eventValue.ArmyId = army.Army.CreateId(255);
            }

            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            // Original queues through AddEvent
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // GlobalTargetSpells.pas:2077
    public override bool ValidTargetSelection(TargetSelectionControl targetSelection)
    {
        if (targetSelection.Target.X != -1)
        {
            MapField mapField = StrategicMap.Current.MapContainer.MapLevels[targetSelection.Target.MapLevel].GetField(targetSelection.Target.X, targetSelection.Target.Y);
            return mapField.TerrainType is (sbyte)TerrainType.Water or (sbyte)TerrainType.UndergroundWater;
        }
        else
        {
            return false;
        }
    }

    // GlobalTargetSpells.pas:2088
    public void ValidateTargetEvent(TargetSelectionControl target, sbyte X, sbyte Y, sbyte layer, ref bool valid)
    {
        if (X != -1)
        {
            MapLevel mapLevel = StrategicMap.Current.MapContainer.MapLevels[layer];
            MapField fieldValue = mapLevel.GetField(X, Y);
            valid = fieldValue.TerrainType is (sbyte)TerrainType.Water or (sbyte)TerrainType.UndergroundWater;
        }
        else
        {
            valid = false;
        }
    }
}
