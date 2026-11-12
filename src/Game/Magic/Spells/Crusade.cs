// Translated from GlobalSpells.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public partial class Crusade : Spell
{
    // GlobalSpells.pas:1302
    public Crusade(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_crusadeText);
        SpellId = 75;
        SpellType = SpellType.Global;
        BaseManaCost = 0;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Life;
    }

    // GlobalSpells.pas:1340
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        int count;
        int unitIndex;
        int pixelX = default;
        int pixelY = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        ArmyHexSprite party;
        Unit newUnit;
        if (executeCount == 0)
        {
            source.GetLocation(ref X, ref Y, ref layer);
            AoWHSMap.Current.ValidateObservation(source.Player, X, Y, layer, 0);
            HexGeometry.HexToPixel(X, Y, ref pixelX, ref pixelY);
            var worldEffectHexSprite = AoWHSMap.Current.AddEffect(WorldEffectType.Crusade);
            worldEffectHexSprite.X = pixelX + 16;
            worldEffectHexSprite.Y = pixelY + 16;
            worldEffectHexSprite.Layer = layer;

            if (AoWHSMap.Current.Random(100) < 20)
            {
                party = new ArmyHexSprite(null);
                party.Army.SetPlayer(0);
                count = 1;
                unitIndex = AoWHSMap.Current.Random(1);
                for (int J = 0; J <= count - 1; J++)
                {
                    newUnit = new Unit(null);
                    newUnit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(LargeUnitResourceIds[unitIndex]));
                    newUnit.ExpandAbility(101);
                    party.Army.AddUnit(newUnit);
                    newUnit.Release();
                }

                if (!PlaceParty(source, party))
                {
                    party?.Dispose();
                }
            }
            else
            {
                for (int I = 0; I <= 1; I++)
                {
                    party = new ArmyHexSprite(null);
                    party.Army.SetPlayer(0);
                    count = AoWHSMap.Current.Random(3) + 4;
                    for (int J = 0; J <= count - 1; J++)
                    {
                        newUnit = new Unit(null);
                        unitIndex = AoWHSMap.Current.Random(2);
                        newUnit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(FootUnitResourceIds[unitIndex]));
                        newUnit.ExpandAbility(101);
                        party.Army.AddUnit(newUnit);
                        newUnit.Release();
                    }

                    if (!PlaceParty(source, party))
                    {
                        party?.Dispose();
                    }
                }

                party = new ArmyHexSprite(null);
                party.Army.SetPlayer(0);
                unitIndex = AoWHSMap.Current.Random(3);
                count = 2;
                if (unitIndex == 2)
                {
                    count += 1;
                }

                for (int J = 0; J <= count - 1; J++)
                {
                    newUnit = new Unit(null);
                    newUnit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(MountedUnitResourceIds[unitIndex]));
                    newUnit.ExpandAbility(101);
                    party.Army.AddUnit(newUnit);
                    newUnit.Release();
                }

                if (!PlaceParty(source, party))
                {
                    party?.Dispose();
                }
            }

            AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastingDone(source, BaseManaCost);
            AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
        }

        return false;
    }

    // GlobalSpells.pas:1417
    public override bool Activate(Troop source)
    {
        bool result;
        string reason = "";
        SpellTokenEvent eventValue;
        base.Activate(source);
        if (CanActivate(source, ref reason) && !AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new SpellTokenEvent(null);
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // GlobalSpells.pas:1313
    public bool PlaceParty(Troop source, ArmyHexSprite party)
    {
        bool result;
        int targetX = default;
        int targetY = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        ImageSequenceAnimation animation;
        source.GetLocation(ref X, ref Y, ref layer);
        var radiusToHexNumber = HexGeometry.RadiusToHexNumber(5);
        var random = AoWHSMap.Current.Random(radiusToHexNumber);
        HexGeometry.CenterHexNumberToHex(X, Y, random, ref targetX, ref targetY);
        if (party.Place((sbyte)targetX, (sbyte)targetY, layer, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea)))
        {
            party.Activate();
            party.Army.SetBehavior(255, GroupBehavior.Suicidal);
            var hexLevel = party.GetHexLevel();
            var hexY = party.GetHexY();
            var hexX = party.GetHexX();
            if (AoWHSMap.Current.Watching(hexX, hexY, hexLevel, 0))
            {
                if (AoWHSSet.Current.SpellControl.GetSpell(34).CastImages.GetImageSequence(0) != null)
                {
                    // { Original uses Warp Party's images for Crusade parties. }
                    animation = new ImageSequenceAnimation(null);
                    animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(34).CastImages.GetImageSequence(0));
                    animation.AnimationLevel = 28;
                    var partyHexLevel = party.GetHexLevel();
                    var partyHexY = party.GetHexY();
                    var partyHexX = party.GetHexX();
                    AoWHSMap.Current.PlaceHex(animation, partyHexX, partyHexY, partyHexLevel);
                }
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_crusadeText = "Crusade";

    private static ReadOnlySpan<int> LargeUnitResourceIds => [133];
    private static ReadOnlySpan<int> MountedUnitResourceIds => [132, 134, 128];
    private static ReadOnlySpan<int> FootUnitResourceIds => [129, 126];
}
