#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Host;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class EarthMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1877
    public EarthMastery(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.EarthMasteryText);
        SpellId = 64;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
        CastDuration = 40;
        AIBasePriority = 1_000;
    }

    // GlobalEnchantmentSpells.pas:1918
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, sphere: MagicSphere.Earth);
        MapCoordinate coordinate64 = source.GetLocation();
        sbyte x = coordinate64.X;
        sbyte y = coordinate64.Y;
        sbyte l = coordinate64.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, radius: 0);
        _ = StrategicMap.Current.AddEffect(WorldEffectType.EarthMastery);
        _ = CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1932
    public override GlobalEnchantment CreateEnchantment() => new EarthMasteryEnchantment(null, holder: CreationHolder());

    // GlobalEnchantmentSpells.pas:1890
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        MapLevel mapLevel = StrategicMap.Current.MapContainer.GetMapLevel(StrategicMap.Current.ViewMapLevel);
        int maxX = StrategicMap.Current.ViewHexX + StrategicMap.Current.ViewWidthHex - 1;
        int maxY = StrategicMap.Current.ViewHexY + StrategicMap.Current.ViewHeightHex - 1;
        for (int x = StrategicMap.Current.ViewHexX; x <= maxX; x++)
        {
            HexGeometry.HexToPixel(x, StrategicMap.Current.ViewHexY, out int pixelX, out int pixelY);
            pixelX = pixelX - pixelXSurface + 16;
            pixelY = pixelY - pixelYSurface + 16;
            MapField mapField = mapLevel.GetField(x, StrategicMap.Current.ViewHexY);
            for (int y = StrategicMap.Current.ViewHexY; y <= maxY; y++)
            {
                // The original frame offset uses the field address.
                if (mapField.OverlayType == 4)
                {
                    _ = CastImages.Show(0, pixelX, pixelY, frame: animation.FrameCounter - FrameFixtureInputs.FieldFrameOffset(mapField), lockedSurface);
                }

                // The original steps the field pointer by FHNOffsets[0, 4], one row down. The step
                // after the last row is never read there, and the guard keeps it from reading here.
                if (y < maxY)
                {
                    mapField = mapLevel.GetField(x, y + 1);
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
