#nullable disable warnings
// Translated from GlobalSpells.pas.

using Aow.Engine.Nodes;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

// inherited name, ID, mana, sphere, level, spell type
public class WarpParty : Spell
{
    // GlobalSpells.pas:632
    public WarpParty(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_warpPartyText);
        SpellId = 34;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Fire;
    }

    // GlobalSpells.pas:719
    public override bool Activate(Troop source)
    {
        string reason = "";
        _ = base.Activate(source);
        bool result;
        if (CanActivate(source, messageText: ref reason) && !GetEngine().StrategicMap.Players.GetPlayers(source.Player).GetBusy())
        {
            // Without an army the source has no party to move.
            if (source.ArmyOwner is not Army armyOwner)
            {
                return false;
            }

            WarpPartyTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
            eventValue.ArmyId = armyOwner.CreateId(255);
            MapCoordinate warpLocation = CalculateWarpLocation(armyOwner.GetArmyHexSprite());
            eventValue.WarpLocation = warpLocation;
            GetEngine().StrategicMap.SelectedArmy.Unselect();
            if (eventValue.WarpLocation != MapCoordinate.None)
            {
                GetEngine().StrategicMap.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
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
            return false;
        }

        return result;
    }

    // GlobalSpells.pas:643
    public bool ValidTarget(ArmyHexSprite? army, int X, int Y, int layer)
    {
        MapFieldMessage message = default;
        MapField field = GetEngine().StrategicMap.MapContainer.GetField(X, Y, layer);
        bool result = field.GetArmyHexSprite() is null && field.GetPlayer() is null;
        if (result)
        {
            message.MessageId = 4384;
            MapFieldMessageParameters param = message.Param;
            param[0] = army;
            message.Param = param;
            message.Result = 0;
            field.SendMapFieldMessage(ref message);
            return message.Result.Int == 0 && army.Army.CanPlace(X, Y, L: layer);
        }

        return result;
    }

    // GlobalSpells.pas:660
    public MapCoordinate CalculateWarpLocation(ArmyHexSprite? army)
    {
        MapFieldMessage message = default;
        DelphiList<MapField> candidates = [];
        MovePointTable table = army.Army.CreateMovePointTable(255);
        int hexY = army.HexY;
        int hexX = army.HexX;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 10);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        MapLevel level = strategicMap.MapContainer.MapLevels[army.HexLevel];
        MapField? field;
        while (walk.Radius < 20)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                field = level.GetField(walk.X, walk.Y);
                if (table[field.TerrainNumber][field.OverlayType.ToOriginal() - MovementTables.FirstOverlay] != -1)
                {
                    _ = candidates.Add(field);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        for (int l = 0, last = strategicMap.MapContainer.LevelCount - 1; l <= last; l++)
        {
            if (Math.Abs(l - army.HexLevel) == 1)
            {
                level = strategicMap.MapContainer.MapLevels[l];
                int armyHexY = army.HexY;
                int armyHexX = army.HexX;
                HexNumberWalkData.InitializeHexNumberWalk(out walk, armyHexX, armyHexY, radius: 0);
                while (walk.Radius < 10)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        field = level.GetField(walk.X, walk.Y);
                        if (table[field.TerrainNumber][field.OverlayType.ToOriginal() - MovementTables.FirstOverlay] != -1)
                        {
                            _ = candidates.Add(field);
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
        }

        bool found = false;
        MapCoordinate result = MapCoordinate.None;
        while (candidates.Count != 0 && !found)
        {
            int i = Random(candidates.Count);
            field = candidates[i];
            if (field.GetArmyHexSprite() is null && field.GetPlayer() is null)
            {
                message.MessageId = 4384;
                MapFieldMessageParameters param = message.Param;
                param[0] = army;
                message.Param = param;
                message.Result = 0;
                field.SendMapFieldMessage(ref message);
                found = message.Result.Int == 0 && army.Army.CanPlace(field.HexX, field.HexY, L: field.HexLevel);
            }

            if (found)
            {
                result = field.MapCoordinate;
            }
            else
            {
                candidates.Delete(i);
            }
        }
        return result;
    }

    private static readonly string s_warpPartyText = "Warp Party";
}
