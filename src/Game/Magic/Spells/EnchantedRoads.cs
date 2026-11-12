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
    public EnchantedRoads(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.EnchantedRoadsText);
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
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Animation animation = default;
        TwinkleAnimation twinkle = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        animation = new Animation(null);
        animation.AnimationLevel = 27;
        animation.OnShow = ShowAnimation;
        animation.AnimationMap = AoWHSMap.Current;
        animation.Activate();
        twinkle = new TwinkleAnimation(null);
        twinkle.Setup(AoWHSSet.Current.SpellControl.GetSpell(44).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
        AoWHSMap.Current.PlaceHex(twinkle, X, Y, L);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:636
    public override GlobalEnchantment CreateEnchantment() => new EnchantedRoadsEnchantment(null);

    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface surface)
    {
        var map = AoWHSMap.Current;
        var level = map.MapContainer.GetMapLevel(map.ViewL);
        int maxX = map.ViewHexX + map.ViewWidthHex - 1, maxY = map.ViewHexY + map.ViewHeightHex - 1;
        for (int x = map.ViewHexX; x <= maxX; x++)
        {
            int pixelX = 0, pixelY = 0;
            HexGeometry.HexToPixel(x, map.ViewHexY, ref pixelX, ref pixelY);
            pixelX = pixelX - pixelXSurface + 16;
            pixelY = pixelY - pixelYSurface + 16;
            for (int y = map.ViewHexY; y <= maxY; y++, pixelY += 32)
            {
                var field = (AoWMapField)level.GetField(x, y);
                if (field.OverlayType == 4 && map.SeatedPlayer != -1 && field.GetExplored(map.SeatedPlayer))
                    CastImages.Show(0, pixelX, pixelY, animation.FrameCounter - ((field.HexX + field.HexY * 8 + ((field.FieldFlags >> 16) & 255)) & 15), surface);
            }
        }
        if (animation.FrameCounter == 50) animation.Dispose();
    }
}
