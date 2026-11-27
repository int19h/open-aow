#nullable disable warnings
// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class TownGate : CityTargetSpell
{
    // CitySpells.pas:421
    public TownGate(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_townGateText);
        SpellId = 38;
        CityTargetMode = CityTargetModes.Own | CityTargetModes.SkipSource;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Cosmos;
    }

    // CitySpells.pas:486
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        // TownGate.CreateTokenEvent constructs TownGateTokenEvent.
        TownGateTokenEvent townGateEvent = (TownGateTokenEvent)eventValue;
        int selection = default;
        ArmyHexSprite? army = default;
        MapCoordinate coordinate84 = source.GetLocation();
        int x = coordinate84.X;
        int y = coordinate84.Y;
        int l = coordinate84.MapLevel;
        _ = townGateEvent.ArmyId.RetrieveArmy(ref army, ref selection);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        MapField field = strategicMap.MapContainer.GetFieldMapCoordinate(townGateEvent.TeleportLocation);
        army.Army.ResetDestinationPosition();
        if (strategicMap.SelectedArmy.ArmyHexSprite == army)
        {
            strategicMap.SelectedArmy.Select(army);
        }

        if (army.Army.ValidateSelection(selection) != army.Army.ValidateSelection(255))
        {
            ArmyHexSprite newArmy = new(null) { Holder = CreationHolder() };
            newArmy.Army.SetPlayer(army.Army.Player);
            _ = newArmy.Army.AddUnitSelection(army.Army.Units, selection);
            _ = GetEngine().StrategicMap.PlaceHex(newArmy, townGateEvent.TeleportLocation.X, townGateEvent.TeleportLocation.Y, L: townGateEvent.TeleportLocation.MapLevel);
        }
        else
        {
            if (field.GetArmyHexSprite() is not null)
            {
                _ = field.GetArmyHexSprite().Army.AddUnitSelection(army.Army.Units, selection);
            }
            else
            {
                _ = army.MoveTo(field, moveCost: 0);
            }
        }

        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        bool showSource = engineStrategicMap.SeatedPlayer == source.Player || engineStrategicMap.Visible(x, y, l, radius: 0);
        bool showTarget = engineStrategicMap.SeatedPlayer == source.Player || engineStrategicMap.Visible(field.HexX, field.HexY, L: field.HexLevel, radius: 0);
        ImageSequenceAnimation a;
        if (showSource)
        {
            GamePack gamePack = GetEngine().GamePack;
            if (gamePack.SpellControl[SpellId].CastImages.ImageSequences[0] is not null)
            {
                a = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                a.SetImageSequence(gamePack.SpellControl[SpellId].CastImages.ImageSequences[0]);
                _ = engineStrategicMap.PlaceHex(a, x, y, l);
            }
        }

        if (showTarget)
        {
            GamePack engineGamePack = GetEngine().GamePack;
            if (engineGamePack.SpellControl[SpellId].CastImages.ImageSequences[0] is not null)
            {
                a = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                a.SetImageSequence(engineGamePack.SpellControl[SpellId].CastImages.ImageSequences[0]);
                _ = GetEngine().StrategicMap.PlaceHex(a, field.HexX, field.HexY, L: field.HexLevel);
            }
        }

        StrategicMap centerViewToStrategicMap = GetEngine().StrategicMap;
        if (centerViewToStrategicMap.SeatedPlayer == source.Player)
        {
            centerViewToStrategicMap.CenterViewTo(field.HexX, field.HexY, field.HexLevel);
        }
        else
        {
            if (!GetEngine().StrategicMap.ValidateObservation(source.Player, field.HexX, field.HexY, field.HexLevel, radius: 0))
            {
                _ = GetEngine().StrategicMap.ValidateObservation(source.Player, x, y, l, radius: 0);
            }
        }

        StrategicMap watchingStrategicMap = GetEngine().StrategicMap;
        if (watchingStrategicMap.SeatedPlayer == source.Player || watchingStrategicMap.Watching(x, y, l, radius: 0) || watchingStrategicMap.Watching(field.HexX, field.HexY, L: field.HexLevel, radius: 0))
        {
            _ = GetEngine().GamePack.SpellControl[SpellId].CastSounds.Play(0);
        }

        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // CitySpells.pas:540
    public override bool CanActivate(Troop source, ref string messageText) => base.CanActivate(source, ref messageText);

    // CitySpells.pas:545
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.TargetMode &= ~(TargetSelectionModes.FixedLevel | TargetSelectionModes.ValidateDistance);
    }

    // CitySpells.pas:445
    public override TownGateTokenEvent CreateTokenEvent() => new(null) { Holder = CreationHolder() };

    // CitySpells.pas:450
    public override bool SetupTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int X, int Y, int L)
    {
        bool result = base.SetupTokenEvent(eventValue, source, X, Y, L);
        if (result)
        {
            // Without an army the source has no party to teleport.
            if (source.ArmyOwner is not Army armyOwner)
            {
                return false;
            }

            ArmyHexSprite? army = armyOwner.GetArmyHexSprite();
            StrategicMap strategicMap = GetEngine().StrategicMap;
            int selection = army.Army.Player == strategicMap.SeatedPlayer && strategicMap.SelectedArmy.ArmyHexSprite == army
                ? strategicMap.SelectedArmy.Selection
                : 255;

            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
            bool found = false;
            MapLevel level = strategicMap.MapContainer.GetMapLevel(L);
            while (walk.Radius < 4 && !found)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    found = ValidTeleportLocation(army, selection, walk.X.TruncateToSByte(), walk.Y.TruncateToSByte(), L: L.TruncateToSByte());
                }

                if (!found)
                {
                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }

            if (found)
            {
                // TownGate.CreateTokenEvent constructs TownGateTokenEvent.
                TownGateTokenEvent townGateEvent = (TownGateTokenEvent)eventValue;
                ArmyId id = army.Army.CreateId(selection);
                townGateEvent.ArmyId = id;
                MapCoordinate packMapCoordinate = new((sbyte)walk.X, (sbyte)walk.Y, (sbyte)L);
                townGateEvent.TeleportLocation = packMapCoordinate;
                return true;
            }
            else
            {
                return false;
            }
        }

        return result;
    }

    // CitySpells.pas:432
    public bool ValidTeleportLocation(ArmyHexSprite? army, int selection, int X, int Y, int L)
    {
        MapField field = GetEngine().StrategicMap.MapContainer.GetField(X, Y, L);
        bool result = field.GetPlayer() is null || field.GetPlayer() == army.Army.Player;

        if (result)
        {
            result = army.Army.ValidTerrain(selection, terrainType: field.TerrainNumber, overlayType: field.OverlayType.ToOriginal());
            // { CanAddUnitSelection returns arOK (zero) when the destination accepts the selection. }
            return result && (field.GetArmyHexSprite() is null || field.GetArmyHexSprite().Army.CanAddUnitSelection(army.Army.Units, selection) == AddUnitsResult.Ok);
        }

        return result;
    }

    private static readonly string s_townGateText = "Town Gate";
}
