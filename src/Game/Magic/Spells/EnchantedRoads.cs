#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class EnchantedRoads : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:571
    public EnchantedRoads(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.EnchantedRoadsText);
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
        sbyte x = coordinate65.X;
        sbyte y = coordinate65.Y;
        sbyte l = coordinate65.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
        Animation animation = new(null)
        {
            Holder = CreationHolder(),
            AnimationLevel = 27
        };
        animation.OnShow += ShowAnimation;
        animation.AnimationMap = StrategicMap.Current;
        animation.Activate();
        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
        twinkle.Setup(GamePack.Current.SpellControl.GetSpell(44).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
        _ = StrategicMap.Current.PlaceHex(twinkle, x, y, l);
        _ = CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:636
    public override GlobalEnchantment CreateEnchantment() => new EnchantedRoadsEnchantment(null, holder: CreationHolder());

    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface surface)
    {
        StrategicMap map = StrategicMap.Current;
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
                if (field.OverlayType == 4 && map.SeatedPlayer != -1 && field.GetExplored(map.SeatedPlayer))
                {
                    _ = CastImages.Show(0, pixelX, pixelY, animation.FrameCounter - ((field.HexX + (field.HexY * 8) + field.FogCount) & 15), surface);
                }
            }
        }
        if (animation.FrameCounter == 50)
        {
            animation.Dispose();
        }
    }
}
