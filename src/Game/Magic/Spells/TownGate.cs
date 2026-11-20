#nullable disable
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

public partial class TownGate : CityTargetSpell
{
    // CitySpells.pas:421
    public TownGate(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_townGateText);
        SpellId = 38;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own, CityTargetModeElement.SkipSource);
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Cosmos;
    }

    // CitySpells.pas:486
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        // TownGate.CreateTokenEvent constructs TownGateTokenEvent.
        TownGateTokenEvent townGateEvent = (TownGateTokenEvent)eventValue;
        byte selection = default;
        ArmyHexSprite army = default;
        MapCoordinate coordinate84 = source.GetLocation();
        sbyte x = coordinate84.X;
        sbyte y = coordinate84.Y;
        sbyte l = coordinate84.MapLevel;
        _ = townGateEvent.ArmyId.RetrieveArmy(ref army, ref selection);
        MapField field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(townGateEvent.TeleportLocation);
        army.Army.ResetDestinationPosition();
        if (StrategicMap.Current.SelectedArmy.GetArmyHexSprite() == army)
        {
            StrategicMap.Current.SelectedArmy.Select(army);
        }

        if (army.Army.ValidateSelection(selection) != army.Army.ValidateSelection(255))
        {
            ArmyHexSprite newArmy = new(null) { Holder = CreationHolder() };
            newArmy.Army.SetPlayer(army.Army.Player);
            _ = newArmy.Army.AddUnitSelection(army.Army.Units, selection);
            _ = StrategicMap.Current.PlaceHex(newArmy, townGateEvent.TeleportLocation.X, townGateEvent.TeleportLocation.Y, townGateEvent.TeleportLocation.MapLevel);
        }
        else
        {
            if (field.GetArmyHexSprite() is not null)
            {
                _ = field.GetArmyHexSprite().Army.AddUnitSelection(army.Army.Units, selection);
            }
            else
            {
                _ = army.MoveTo(field, 0);
            }
        }

        bool showSource = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Visible(x, y, l, 0);
        bool showTarget = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Visible(field.HexX, field.HexY, field.HexLevel, 0);
        ImageSequenceAnimation a;
        if (showSource)
        {
            if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0] is not null)
            {
                a = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                a.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0]);
                _ = StrategicMap.Current.PlaceHex(a, x, y, l);
            }
        }

        if (showTarget)
        {
            if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0] is not null)
            {
                a = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                a.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0]);
                _ = StrategicMap.Current.PlaceHex(a, field.HexX, field.HexY, field.HexLevel);
            }
        }

        if (StrategicMap.Current.SeatedPlayer == source.Player)
        {
            StrategicMap.Current.CenterViewTo(field.HexX, field.HexY, field.HexLevel);
        }
        else
        {
            if (!StrategicMap.Current.ValidateObservation(source.Player, field.HexX, field.HexY, field.HexLevel, 0))
            {
                _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
            }
        }

        if (StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Watching(x, y, l, 0) || StrategicMap.Current.Watching(field.HexX, field.HexY, field.HexLevel, 0))
        {
            _ = GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
        }

        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // CitySpells.pas:540
    public override bool CanActivate(Troop source, ref string messageText) => base.CanActivate(source, ref messageText);

    // CitySpells.pas:545
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.TargetMode -= Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel, TargetSelectionModeElement.ValidateDistance);
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

            ArmyHexSprite army = armyOwner.GetArmyHexSprite();
            byte selection = army.Army.Player == StrategicMap.Current.SeatedPlayer && StrategicMap.Current.SelectedArmy.GetArmyHexSprite() == army
                ? StrategicMap.Current.SelectedArmy.GetSelection()
                : (byte)255;

            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, 0);
            bool found = false;
            MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(L);
            while (walk.Radius < 4 && !found)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    found = ValidTeleportLocation(army, selection, (sbyte)walk.X, (sbyte)walk.Y, (sbyte)L);
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
                MapCoordinate packMapCoordinate = HexGeometry.PackMapCoordinate((sbyte)walk.X, (sbyte)walk.Y, (sbyte)L);
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
    public bool ValidTeleportLocation(ArmyHexSprite army, byte selection, sbyte X, sbyte Y, sbyte L)
    {
        MapField field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
        bool result = field.GetPlayer() == -1 || field.GetPlayer() == army.Army.Player;

        if (result)
        {
            result = army.Army.ValidTerrain(selection, field.TerrainType, field.OverlayType);
            // { CanAddUnitSelection returns arOK (zero) when the destination accepts the selection. }
            return result && (field.GetArmyHexSprite() is null || field.GetArmyHexSprite().Army.CanAddUnitSelection(army.Army.Units, selection) == AddUnitsResult.Ok);
        }

        return result;
    }

    private static readonly string s_townGateText = "Town Gate";
}
