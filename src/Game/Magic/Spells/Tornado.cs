// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class Tornado : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:1798
    public Tornado(ECustomNode owner) : base(owner)
    {
        this.SpellName = AbilityRules.TranslateResourceString(GameText.TornadoText);
        SpellId = 68;
        AboveGroundOnly = true;
    }

    // GlobalTargetSpells.pas:1885
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target;
        target = targetSelection;
        base.SetupTargetSelection(source, target);
        target.HexNumbers.Clear();
    }

    // GlobalTargetSpells.pas:1865
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        bool result;
        TornadoTokenEvent eventValue;
        ArmyHexSprite army;
        if (!AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new TornadoTokenEvent(null);
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

    // GlobalTargetSpells.pas:1806
    public bool ValidWarpLocation(ArmyHexSprite army, sbyte X, sbyte Y, sbyte layer)
    {
        AoWMapField mapField;
        MapFieldMessage message = default;
        mapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, layer);
        bool result = mapField.GetArmyHexSprite() == null && mapField.GetPlayer() == -1;
        if (result)
        {
            message.MessageId = 4384;
            message.Param[0] = army;
            message.Result = 0;
            mapField.SendMapFieldMessage(ref message);
            result = message.Result.Int == 0 && army.Army.CanPlace(X, Y, layer);
        }

        return result;
    }

    // GlobalTargetSpells.pas:1823
    public int CalculateWarpLocation(ArmyHexSprite army, byte selection)
    {
        SpellMoveTable table = default;
        HexNumberWalkData walk = default;
        MapLevel mapLevel;
        MapField mapField;
        DelphiList<MapField> locations;
        bool found;
        int index;
        int X;
        locations = new DelphiList<MapField>();
        army.Army.CreateMovePointTable(selection, Bytes.Of(ref table));
        var hexY = army.HexY;
        var hexX = army.HexX;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, hexX, hexY, 10);
        mapLevel = AoWHSMap.Current.MapContainer.MapLevels[army.HexLevel];
        while (walk.Radius < 15)
        {
            X = walk.X;
            if (X >= 0 && walk.Y >= 0 && X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                mapField = mapLevel.GetField(X, walk.Y);
                if (table.Cost[mapField.TerrainType][mapField.OverlayType] != -1)
                {
                    locations.Add(mapField);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        found = false;
        int result = 16777215;
        while (locations.Count != 0 && !found)
        {
            index = Random(locations.Count);
            mapField = locations[index];
            if (ValidWarpLocation(army, mapField.HexX, mapField.HexY, mapField.HexLevel))
            {
                found = true;
                result = mapField.MapCoordinate;
            }
            else
            {
                locations.Delete(index);
            }
        }

        locations?.Dispose();
        return result;
    }
}
