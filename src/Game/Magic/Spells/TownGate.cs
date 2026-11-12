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
    public TownGate(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_townGateText);
        SpellId = 38;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own, CityTargetModeElement.SkipSource);
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Cosmos;
    }

    // CitySpells.pas:486
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool showSource;
        bool showTarget;
        byte selection = default;
        ArmyHexSprite army = default;
        MapField field;
        ArmyHexSprite newArmy;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ImageSequenceAnimation A;
        source.GetLocation(out X, out Y, out L);
        ((TownGateTokenEvent)eventValue).ArmyId.RetrieveArmy(ref army, ref selection);
        field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(((TownGateTokenEvent)eventValue).TeleportLocation.Packed);
        army.Army.ResetDestinationPosition();
        if (StrategicMap.Current.SelectedArmy.GetArmyHexSprite() == army)
        {
            StrategicMap.Current.SelectedArmy.Select(army);
        }

        if (army.Army.ValidateSelection(selection) != army.Army.ValidateSelection(255))
        {
            newArmy = new ArmyHexSprite(null);
            newArmy.Army.SetPlayer(army.Army.Player);
            newArmy.Army.AddUnitSelection(army.Army, selection);
            StrategicMap.Current.PlaceHex(newArmy, ((TownGateTokenEvent)eventValue).TeleportLocation.X, ((TownGateTokenEvent)eventValue).TeleportLocation.Y, ((TownGateTokenEvent)eventValue).TeleportLocation.MapLevel);
        }
        else
        {
            if (field.GetArmyHexSprite() != null)
            {
                field.GetArmyHexSprite().Army.AddUnitSelection(army.Army, selection);
            }
            else
            {
                army.MoveTo(field, 0);
            }
        }

        showSource = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Visible(X, Y, L, 0);
        showTarget = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Visible(field.HexX, field.HexY, field.HexLevel, 0);
        if (showSource)
        {
            if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0] != null)
            {
                A = new ImageSequenceAnimation(null);
                A.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0]);
                StrategicMap.Current.PlaceHex(A, X, Y, L);
            }
        }

        if (showTarget)
        {
            if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0] != null)
            {
                A = new ImageSequenceAnimation(null);
                A.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[0]);
                StrategicMap.Current.PlaceHex(A, field.HexX, field.HexY, field.HexLevel);
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
                StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
            }
        }

        if (StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Watching(X, Y, L, 0) || StrategicMap.Current.Watching(field.HexX, field.HexY, field.HexLevel, 0))
        {
            GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
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
        targetSelection.TargetMode = targetSelection.TargetMode - Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel, TargetSelectionModeElement.ValidateDistance);
    }

    // CitySpells.pas:445
    public override CityTargetSpellTokenEvent CreateTokenEvent() => new TownGateTokenEvent(null);

    // CitySpells.pas:450
    public override bool SetupTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int X, int Y, int L)
    {
        byte selection;
        ArmyHexSprite army;
        bool found;
        MapLevel level;
        HexNumberWalkData walk = default;
        bool result = base.SetupTokenEvent(eventValue, source, X, Y, L);
        if (result)
        {
            if (source.Owner is HeroList)
            {
                return false;
            }

            army = source.ArmyOwner.GetArmyHexSprite();
            if (army.Army.Player == StrategicMap.Current.SeatedPlayer && StrategicMap.Current.SelectedArmy.GetArmyHexSprite() == army)
            {
                selection = StrategicMap.Current.SelectedArmy.GetSelection();
            }
            else
            {
                selection = 255;
            }

            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
            found = false;
            level = StrategicMap.Current.MapContainer.GetMapLevel(L);
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
                var id = army.Army.CreateId(selection);
                ((TownGateTokenEvent)eventValue).ArmyId = id;
                var packMapCoordinate = HexGeometry.PackMapCoordinate((sbyte)walk.X, (sbyte)walk.Y, (sbyte)L);
                ((TownGateTokenEvent)eventValue).TeleportLocation = packMapCoordinate;
                result = true;
            }
            else
            {
                result = false;
            }
        }

        return result;
    }

    // CitySpells.pas:432
    public bool ValidTeleportLocation(ArmyHexSprite army, byte selection, sbyte X, sbyte Y, sbyte L)
    {
        bool result;
        MapField field;
        field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
        if (field.GetPlayer() != -1 && field.GetPlayer() != army.Army.Player)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        if (result)
        {
            result = army.Army.ValidTerrain(selection, field.TerrainType, field.OverlayType);
            // { CanAddUnitSelection returns arOK (zero) when the destination accepts the selection. }
            result = result && (field.GetArmyHexSprite() == null || field.GetArmyHexSprite().Army.CanAddUnitSelection(army.Army, selection) == AddUnitsResult.Ok);
        }

        return result;
    }

    private static readonly string s_townGateText = "Town Gate";
}
