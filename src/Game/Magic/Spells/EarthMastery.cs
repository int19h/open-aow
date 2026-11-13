// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Host;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class EarthMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1877
    public EarthMastery(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.EarthMasteryText);
        SpellId = 64;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
        CastDuration = 40;
        AIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1918
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Earth);
        source.GetLocation(out X, out Y, out L);
        StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        StrategicMap.Current.AddEffect(WorldEffectType.EarthMastery);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1932
    public override GlobalEnchantment CreateEnchantment() => new EarthMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:1890
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int maxX;
        int maxY;
        int pixelX = default;
        int pixelY = default;
        MapLevel mapLevel;
        MapField mapField;
        mapLevel = StrategicMap.Current.MapContainer.GetMapLevel(StrategicMap.Current.ViewMapLevel);
        maxX = StrategicMap.Current.ViewHexX + StrategicMap.Current.ViewWidthHex - 1;
        maxY = StrategicMap.Current.ViewHexY + StrategicMap.Current.ViewHeightHex - 1;
        for (int X = StrategicMap.Current.ViewHexX; X <= maxX; X++)
        {
            HexGeometry.HexToPixel(X, StrategicMap.Current.ViewHexY, out pixelX, out pixelY);
            pixelX = pixelX - pixelXSurface + 16;
            pixelY = pixelY - pixelYSurface + 16;
            mapField = mapLevel.GetField(X, StrategicMap.Current.ViewHexY);
            for (int Y = StrategicMap.Current.ViewHexY; Y <= maxY; Y++)
            {
                // The original frame offset uses the field address.
                if (mapField.OverlayType == 4)
                {
                    CastImages.Show(0, pixelX, pixelY, animation.FrameCounter - FrameFixtureInputs.FieldFrameOffset(mapField), lockedSurface);
                }

                // The original steps the field pointer by FHNOffsets[0, 4], one row down. The step
                // after the last row is never read there, and the guard keeps it from reading here.
                if (Y < maxY)
                {
                    mapField = mapLevel.GetField(X, Y + 1);
                }

                pixelY += 32;
            }
        }

        if (animation.FrameCounter == 50)
        {
            animation.Dispose();
        }
    }
}
