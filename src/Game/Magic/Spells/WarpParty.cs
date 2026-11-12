// Translated from GlobalSpells.pas.

using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited name, ID, mana, sphere, level, spell type
public partial class WarpParty : Spell
{
    // GlobalSpells.pas:632
    public WarpParty(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_warpPartyText);
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
        base.Activate(source);
        if (CanActivate(source, ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            if (source.Owner is HeroList)
            {
                return false;
            }

            eventValue = new WarpPartyTokenEvent(null);
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            eventValue.ArmyId = source.ArmyOwner.CreateId(255);
            var warpLocation = CalculateWarpLocation(source.ArmyOwner.GetArmyHexSprite());
            eventValue.WarpLocation.Packed = warpLocation;
            StrategicMap.Current.SelectedArmy.Unselect();
            if (eventValue.WarpLocation.Packed != 16777215)
            {
                StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                result = true;
            }
            else
            {
                result = false;
            }

            eventValue.Release();
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
        bool result = field.GetArmyHexSprite() == null && field.GetPlayer() == -1;
        if (result)
        {
            message.MessageId = 4384;
            message.Param[0] = army;
            message.Result = 0;
            field.SendMapFieldMessage(ref message);
            result = message.Result.Int == 0 && army.Army.CanPlace(X, Y, layer);
        }

        return result;
    }

    // GlobalSpells.pas:660
    public int CalculateWarpLocation(ArmyHexSprite army)
    {
        DelphiList<MapField> candidates;
        MovePointTable table = default;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        MapLevel level;
        MapField field;
        int I;
        bool found;
        candidates = new DelphiList<MapField>();
        army.Army.CreateMovePointTable(255, Bytes.Of(ref table));
        var hexY = army.HexY;
        var hexX = army.HexX;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 10);
        level = StrategicMap.Current.MapContainer.MapLevels[army.HexLevel];
        while (walk.Radius < 20)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                field = level.GetField(walk.X, walk.Y);
                if (table[field.TerrainType][field.OverlayType - MovementTables.FirstOverlay] != -1)
                {
                    candidates.Add(field);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        for (int L = 0, last = StrategicMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
        {
            if (Math.Abs(L - army.HexLevel) == 1)
            {
                level = StrategicMap.Current.MapContainer.MapLevels[L];
                var armyHexY = army.HexY;
                var armyHexX = army.HexX;
                HexNumberWalkData.InitializeHexNumberWalk(out walk, armyHexX, armyHexY, 0);
                while (walk.Radius < 10)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        field = level.GetField(walk.X, walk.Y);
                        if (table[field.TerrainType][field.OverlayType - MovementTables.FirstOverlay] != -1)
                        {
                            candidates.Add(field);
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
        }

        found = false;
        int result = 16777215;
        while (candidates.Count != 0 && !found)
        {
            I = Random(candidates.Count);
            field = candidates[I];
            if (field.GetArmyHexSprite() == null && field.GetPlayer() == -1)
            {
                message.MessageId = 4384;
                message.Param[0] = army;
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
                candidates.Delete(I);
            }
        }

        candidates?.Dispose();
        return result;
    }

    private static readonly string s_warpPartyText = "Warp Party";
}
