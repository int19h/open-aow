// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Vortex : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:2069
    public Vortex(ECustomNode owner) : base(owner)
    {
        this.SpellName = AbilityRules.TranslateResourceString(GameText.VortexText);
        SpellId = 70;
        AboveGroundOnly = false;
    }

    // GlobalTargetSpells.pas:2120
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        // { Use the incoming target. }
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        targetSelection.OnValidateTarget = ValidateTargetEvent;
    }

    // GlobalTargetSpells.pas:2100
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        bool result;
        VortexTokenEvent eventValue;
        ArmyHexSprite army;
        if (!AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new VortexTokenEvent(null);
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetArmyHexSprite();
            if (army != null)
            {
                eventValue.ArmyId = army.Army.CreateId(255);
            }

            AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            // Original queues through AddEvent
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // GlobalTargetSpells.pas:2077
    public override bool ValidTargetSelection(TargetSelectionControl targetSelection)
    {
        bool result;
        MapField mapField;
        if (targetSelection.TargetX != -1)
        {
            mapField = AoWHSMap.Current.MapContainer.MapLevels[targetSelection.TargetL].GetField(targetSelection.TargetX, targetSelection.TargetY);
            result = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)mapField.TerrainType);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // GlobalTargetSpells.pas:2088
    public void ValidateTargetEvent(TargetSelectionControl target, sbyte X, sbyte Y, sbyte layer, ref bool valid)
    {
        MapLevel mapLevel;
        if (X != -1)
        {
            mapLevel = AoWHSMap.Current.MapContainer.MapLevels[layer];
            var fieldValue = mapLevel.GetField(X, Y);
            valid = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
        }
        else
        {
            valid = false;
        }
    }
}
