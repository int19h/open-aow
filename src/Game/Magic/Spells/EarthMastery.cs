#nullable disable warnings
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
    public EarthMastery(CustomNode? owner, EngineObject holder) : base(owner)
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
        SpellTargets.NotifySpellSphere(source.Player, sphere: MagicSphere.Earth, engine: GetEngine()!);
        MapCoordinate coordinate64 = source.GetLocation();
        int x = coordinate64.X;
        int y = coordinate64.Y;
        int l = coordinate64.MapLevel;
        _ = GetEngine().StrategicMap.ValidateObservation(source.Player, x, y, l, radius: 0);
        _ = GetEngine().StrategicMap.AddEffect(WorldEffectType.EarthMastery);
        _ = CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1932
    public override GlobalEnchantment CreateEnchantment() => new EarthMasteryEnchantment(null, holder: CreationHolder());

    // GlobalEnchantmentSpells.pas:1890
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        MapLevel mapLevel = strategicMap.MapContainer.GetMapLevel(strategicMap.ViewMapLevel);
        int maxX = strategicMap.ViewHexX + strategicMap.ViewWidthHex - 1;
        int maxY = strategicMap.ViewHexY + strategicMap.ViewHeightHex - 1;
        for (int x = strategicMap.ViewHexX; x <= maxX; x++)
        {
            HexGeometry.HexToPixel(x, strategicMap.ViewHexY, out int pixelX, out int pixelY);
            pixelX = pixelX - pixelXSurface + 16;
            pixelY = pixelY - pixelYSurface + 16;
            MapField mapField = mapLevel.GetField(x, strategicMap.ViewHexY);
            for (int y = strategicMap.ViewHexY; y <= maxY; y++)
            {
                // The original frame offset uses the field address.
                if (mapField.OverlayType == OverlayType.Road)
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
