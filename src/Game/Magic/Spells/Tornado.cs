#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

public class Tornado : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:1798
    public Tornado(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.TornadoText);
        SpellId = 68;
        AboveGroundOnly = true;
    }

    // GlobalTargetSpells.pas:1885
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target = targetSelection;
        base.SetupTargetSelection(source, targetSelection: target);
        target.HexNumbers.Clear();
    }

    // GlobalTargetSpells.pas:1865
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            TornadoTokenEvent eventValue = new(null) { Holder = CreationHolder() };
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

    // GlobalTargetSpells.pas:1806
    public bool ValidWarpLocation(ArmyHexSprite army, int X, int Y, sbyte layer)
    {
        MapFieldMessage message = default;
        MapField mapField = StrategicMap.Current.MapContainer.GetField(X, Y, layer);
        bool result = mapField.GetArmyHexSprite() is null && mapField.GetPlayer() == -1;
        if (result)
        {
            message.MessageId = 4384;
            MapFieldMessageParameters param = message.Param;
            param[0] = army;
            message.Param = param;
            message.Result = 0;
            mapField.SendMapFieldMessage(ref message);
            return message.Result.Int == 0 && army.Army.CanPlace(X, Y, L: layer);
        }

        return result;
    }

    // GlobalTargetSpells.pas:1823
    public MapCoordinate CalculateWarpLocation(ArmyHexSprite army, byte selection)
    {
        DelphiList<MapField> locations = [];
        MovePointTable table = army.Army.CreateMovePointTable(selection);
        int hexY = army.HexY;
        int hexX = army.HexX;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 10);
        MapLevel mapLevel = StrategicMap.Current.MapContainer.MapLevels[army.HexLevel];
        MapField? mapField;
        while (walk.Radius < 15)
        {
            int x = walk.X;
            if (x >= 0 && walk.Y >= 0 && x < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                mapField = mapLevel.GetField(x, walk.Y);
                if (SpellTableCost(table, terrainType: mapField.TerrainType, overlayType: mapField.OverlayType) != -1)
                {
                    _ = locations.Add(mapField);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        bool found = false;
        MapCoordinate result = MapCoordinate.None;
        while (locations.Count != 0 && !found)
        {
            int index = Random(locations.Count);
            mapField = locations[index];
            if (ValidWarpLocation(army, mapField.HexX, mapField.HexY, layer: (sbyte)mapField.HexLevel))
            {
                found = true;
                result = mapField.Location;
            }
            else
            {
                locations.Delete(index);
            }
        }

        locations?.Dispose();
        return result;
    }

    // The original fills a TSpellMoveTable (a Flags byte, then Cost: array[0..15, 0..15]) through the
    // untyped table of TArmy.CreateMovePointTable, which writes 256 bytes from the start of the
    // record, and then reads Cost[TerrainType, OverlayType]. That entry is byte 1 + 16 t + o of the
    // army's table, one byte after where the Cost index points, which cancels the table's overlay base
    // of -1: overlays -1 to 14 read the army's own cost. Cost[15, 15] lies past the 256 written bytes
    // and reads 0 here (the original read whatever the stack held).
    internal static sbyte SpellTableCost(in MovePointTable table, int terrainType, int overlayType)
    {
        int offset = 1 + (16 * terrainType) + overlayType;
        return offset < MovePointTable.EntryCount ? table.AtOffset(offset) : (sbyte)0;
    }
}
