#nullable disable warnings
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class EnchantedRoads : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:571
    public EnchantedRoads(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.EnchantedRoadsText);
        SpellId = 44;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
        CastDuration = 20;
        AIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:613
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        MapCoordinate coordinate65 = source.GetLocation();
        int x = coordinate65.X;
        int y = coordinate65.Y;
        int l = coordinate65.MapLevel;
        _ = GetEngine().StrategicMap.ValidateObservation(source.Player, x, y, l, radius: 0);
        Animation animation = new(null, show: ShowAnimation)
        {
            Holder = CreationHolder(),
            AnimationLevel = 27
,
            AnimationMap = GetEngine().StrategicMap
        };
        animation.Activate();
        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
        twinkle.Setup(GetEngine().GamePack.SpellControl[44].CastImages.ImageSequences[0], mode: TwinkleModes.Rotate, count: 50, radius: 64, duration: 50, startDelay: 30);
        _ = GetEngine().StrategicMap.PlaceHex(twinkle, x, y, l);
        _ = CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:636
    public override GlobalEnchantment CreateEnchantment() => new EnchantedRoadsEnchantment(null, holder: CreationHolder());

    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface surface)
    {
        StrategicMap? map = GetEngine().StrategicMap;
        MapLevel level = map.MapContainer.GetMapLevel(map.ViewMapLevel);
        int maxX = map.ViewHexX + map.ViewWidthHex - 1, maxY = map.ViewHexY + map.ViewHeightHex - 1;
        for (int x = map.ViewHexX; x <= maxX; x++)
        {
            HexGeometry.HexToPixel(x, map.ViewHexY, out int pixelX, out int pixelY);
            pixelX = pixelX - pixelXSurface + 16;
            pixelY = pixelY - pixelYSurface + 16;
            for (int y = map.ViewHexY; y <= maxY; y++, pixelY += 32)
            {
                MapField field = level.GetField(x, y);
                if (field.OverlayType == OverlayType.Road && map.SeatedPlayer is not null && field.GetExplored(map.SeatedPlayer))
                {
                    _ = CastImages.Show(0, pixelX, pixelY, frame: animation.FrameCounter - ((field.HexX + (field.HexY * 8) + field.FogCount) & 0xF), lockedSurface: surface);
                }
            }
        }
        if (animation.FrameCounter == 50)
        {
            animation.Dispose();
        }
    }
}
