#nullable disable warnings
// Translated from GlobalSpells.pas.

using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public class Crusade : Spell
{
    // GlobalSpells.pas:1302
    public Crusade(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_crusadeText);
        SpellId = 75;
        SpellType = SpellType.Global;
        BaseManaCost = 0;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Life;
    }

    // GlobalSpells.pas:1340
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        if (executeCount == 0)
        {
            MapCoordinate coordinate61 = source.GetLocation();
            sbyte x = coordinate61.X;
            sbyte y = coordinate61.Y;
            sbyte layer = coordinate61.MapLevel;
            _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, layer, radius: 0);
            HexGeometry.HexToPixel(x, y, out int pixelX, out int pixelY);
            WorldEffectHexSprite worldEffectHexSprite = StrategicMap.Current.AddEffect(WorldEffectType.Crusade);
            worldEffectHexSprite.X = pixelX + 16;
            worldEffectHexSprite.Y = pixelY + 16;
            worldEffectHexSprite.Layer = layer;

            int count;
            int unitIndex;
            ArmyHexSprite party;
            Unit newUnit;
            if (StrategicMap.Current.Random(100) < 20)
            {
                party = new ArmyHexSprite(null) { Holder = CreationHolder() };
                party.Army.SetPlayer(0);
                count = 1;
                unitIndex = StrategicMap.Current.Random(1);
                for (int j = 0; j <= count - 1; j++)
                {
                    newUnit = new Unit(null) { Holder = CreationHolder() };
                    newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(LargeUnitResourceIds[unitIndex]));
                    _ = newUnit.ExpandAbility(101);
                    _ = party.Army.AddUnit(newUnit);
                    _ = newUnit.Release();
                }

                if (!PlaceParty(source, party))
                {
                    party?.Dispose();
                }
            }
            else
            {
                for (int i = 0; i <= 1; i++)
                {
                    party = new ArmyHexSprite(null) { Holder = CreationHolder() };
                    party.Army.SetPlayer(0);
                    count = StrategicMap.Current.Random(3) + 4;
                    for (int j = 0; j <= count - 1; j++)
                    {
                        newUnit = new Unit(null) { Holder = CreationHolder() };
                        unitIndex = StrategicMap.Current.Random(2);
                        newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(FootUnitResourceIds[unitIndex]));
                        _ = newUnit.ExpandAbility(101);
                        _ = party.Army.AddUnit(newUnit);
                        _ = newUnit.Release();
                    }

                    if (!PlaceParty(source, party))
                    {
                        party?.Dispose();
                    }
                }

                party = new ArmyHexSprite(null) { Holder = CreationHolder() };
                party.Army.SetPlayer(0);
                unitIndex = StrategicMap.Current.Random(3);
                count = 2;
                if (unitIndex == 2)
                {
                    count += 1;
                }

                for (int j = 0; j <= count - 1; j++)
                {
                    newUnit = new Unit(null) { Holder = CreationHolder() };
                    newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(MountedUnitResourceIds[unitIndex]));
                    _ = newUnit.ExpandAbility(101);
                    _ = party.Army.AddUnit(newUnit);
                    _ = newUnit.Release();
                }

                if (!PlaceParty(source, party))
                {
                    party?.Dispose();
                }
            }

            GamePack.Current.SpellControl[SpellId].CastingDone(source, mana: BaseManaCost);
            _ = GamePack.Current.SpellControl[SpellId].CastSounds.Play(0);
        }

        return false;
    }

    // GlobalSpells.pas:1417
    public override bool Activate(Troop source)
    {
        string reason = "";
        _ = base.Activate(source);
        if (CanActivate(source, messageText: ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            SpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // GlobalSpells.pas:1313
    public bool PlaceParty(Troop source, ArmyHexSprite party)
    {
        int targetX = default;
        int targetY = default;
        MapCoordinate coordinate62 = source.GetLocation();
        sbyte x = coordinate62.X;
        sbyte y = coordinate62.Y;
        sbyte layer = coordinate62.MapLevel;
        int radiusToHexNumber = HexGeometry.RadiusToHexNumber(5);
        HexGeometry.CenterHexNumberToHex(hexNumber: StrategicMap.Current.Random(radiusToHexNumber), hexX: x, hexY: y, newHexX: ref targetX, newHexY: ref targetY);
        if (party.Place((sbyte)targetX, (sbyte)targetY, layer, mode: ArmyPlaceModes.Safe | ArmyPlaceModes.ScanArea))
        {
            party.Activate();
            party.Army.SetBehavior(255, behavior: GroupBehavior.Suicidal);
            sbyte hexLevel = party.HexLevel;
            sbyte hexY = party.HexY;
            sbyte hexX = party.HexX;
            if (StrategicMap.Current.Watching(hexX, hexY, L: hexLevel, radius: 0))
            {
                if (GamePack.Current.SpellControl[34].CastImages.GetImageSequence(0) is not null)
                {
                    // { Original uses Warp Party's images for Crusade parties. }
                    ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.SpellControl[34].CastImages.GetImageSequence(0));
                    animation.AnimationLevel = 28;
                    sbyte partyHexLevel = party.HexLevel;
                    sbyte partyHexY = party.HexY;
                    sbyte partyHexX = party.HexX;
                    _ = StrategicMap.Current.PlaceHex(animation, partyHexX, partyHexY, L: partyHexLevel);
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_crusadeText = "Crusade";

    private static ReadOnlySpan<int> LargeUnitResourceIds => [133];
    private static ReadOnlySpan<int> MountedUnitResourceIds => [132, 134, 128];
    private static ReadOnlySpan<int> FootUnitResourceIds => [129, 126];
}
