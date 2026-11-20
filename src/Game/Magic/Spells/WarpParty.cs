#nullable disable
// Translated from GlobalSpells.pas.

using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

// inherited name, ID, mana, sphere, level, spell type
public partial class WarpParty : Spell
{
    // GlobalSpells.pas:632
    public WarpParty(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_warpPartyText);
        SpellId = 34;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Fire;
    }

    // GlobalSpells.pas:719
    public override bool Activate(Troop source)
    {
        bool result;
        string reason = "";
        WarpPartyTokenEvent eventValue;
        _ = base.Activate(source);
        if (CanActivate(source, ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            // Without an army the source has no party to move.
            if (source.ArmyOwner is not Army armyOwner)
            {
                return false;
            }

            eventValue = new WarpPartyTokenEvent(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            eventValue.ArmyId = armyOwner.CreateId(255);
            MapCoordinate warpLocation = CalculateWarpLocation(armyOwner.GetArmyHexSprite());
            eventValue.WarpLocation = warpLocation;
            StrategicMap.Current.SelectedArmy.Unselect();
            if (eventValue.WarpLocation != MapCoordinate.None)
            {
                StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                result = true;
            }
            else
            {
                result = false;
            }

            _ = eventValue.Release();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // GlobalSpells.pas:643
    public bool ValidTarget(ArmyHexSprite army, sbyte X, sbyte Y, sbyte layer)
    {
        MapField field;
        MapFieldMessage message = default;
        field = StrategicMap.Current.MapContainer.GetField(X, Y, layer);
        bool result = field.GetArmyHexSprite() is null && field.GetPlayer() == -1;
        if (result)
        {
            message.MessageId = 4384;
            MapFieldMessageParameters param = message.Param;
            param[0] = army;
            message.Param = param;
            message.Result = 0;
            field.SendMapFieldMessage(ref message);
            result = message.Result.Int == 0 && army.Army.CanPlace(X, Y, layer);
        }

        return result;
    }

    // GlobalSpells.pas:660
    public MapCoordinate CalculateWarpLocation(ArmyHexSprite army)
    {
        DelphiList<MapField> candidates;
        MapFieldMessage message = default;
        MapLevel level;
        MapField field;
        int i;
        bool found;
        candidates = new DelphiList<MapField>();
        army.Army.CreateMovePointTable(255, out MovePointTable table);
        sbyte hexY = army.HexY;
        sbyte hexX = army.HexX;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, 10);
        level = StrategicMap.Current.MapContainer.MapLevels[army.HexLevel];
        while (walk.Radius < 20)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                field = level.GetField(walk.X, walk.Y);
                if (table[field.TerrainType][field.OverlayType - MovementTables.FirstOverlay] != -1)
                {
                    _ = candidates.Add(field);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        for (int l = 0, last = StrategicMap.Current.MapContainer.LevelCount - 1; l <= last; l++)
        {
            if (Math.Abs(l - army.HexLevel) == 1)
            {
                level = StrategicMap.Current.MapContainer.MapLevels[l];
                sbyte armyHexY = army.HexY;
                sbyte armyHexX = army.HexX;
                HexNumberWalkData.InitializeHexNumberWalk(out walk, armyHexX, armyHexY, 0);
                while (walk.Radius < 10)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        field = level.GetField(walk.X, walk.Y);
                        if (table[field.TerrainType][field.OverlayType - MovementTables.FirstOverlay] != -1)
                        {
                            _ = candidates.Add(field);
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
        }

        found = false;
        MapCoordinate result = MapCoordinate.None;
        while (candidates.Count != 0 && !found)
        {
            i = Random(candidates.Count);
            field = candidates[i];
            if (field.GetArmyHexSprite() is null && field.GetPlayer() == -1)
            {
                message.MessageId = 4384;
                MapFieldMessageParameters param = message.Param;
                param[0] = army;
                message.Param = param;
                message.Result = 0;
                field.SendMapFieldMessage(ref message);
                found = message.Result.Int == 0 && army.Army.CanPlace(field.HexX, field.HexY, field.HexLevel);
            }

            if (found)
            {
                result = field.GetMapCoordinate();
            }
            else
            {
                candidates.Delete(i);
            }
        }

        candidates?.Dispose();
        return result;
    }

    private static readonly string s_warpPartyText = "Warp Party";
}
