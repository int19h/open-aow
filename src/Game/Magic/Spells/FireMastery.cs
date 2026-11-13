// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class FireMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1733
    public FireMastery(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.FireMasteryText);
        SpellId = 63;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Fire;
        CastDuration = 40;
        AIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1792
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        if (executeCount == 0)
        {
            source.GetLocation(out X, out Y, out L);
            SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Fire);
            StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
            var animation = new Animation(null) { Holder = CreationHolder() };
            animation.AnimationLevel = 27;
            animation.OnShow += ShowAnimation;
            animation.AnimationMap = StrategicMap.Current;
            animation.Tag = (StrategicMap.Current.ViewWidth & 65535) | (StrategicMap.Current.ViewHeight << 16);
            animation.Activate();

            CastSounds.Play(0);
            CastingDone(source, eventValue.EventMana);
        }

        if (executeCount == 20)
        {
            base.ExecuteTokenEvent(eventValue, source, 0);
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1746
    public override GlobalEnchantment CreateEnchantment() => new FireMasteryEnchantment(null, holder: CreationHolder());

    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface surface)
    {
        int chance = GetEngine().AnimationDetail switch { 0 => 2, 2 => 5, _ => 3 };
        var map = StrategicMap.Current;
        var level = map.MapContainer.GetMapLevel(map.ViewMapLevel);
        int maxX = map.ViewHexX + map.ViewWidthHex - 1, maxY = map.ViewHexY + map.ViewHeightHex - 1;
        for (int x = map.ViewHexX; x <= maxX; x++)
        {
            int pixelX = 0, pixelY = 0;
            HexGeometry.HexToPixel(x, map.ViewHexY, out pixelX, out pixelY);
            pixelX = pixelX - pixelXSurface + 16;
            pixelY = pixelY - pixelYSurface + 16;
            for (int y = map.ViewHexY; y <= maxY; y++, pixelY += 32)
            {
                var field = level.GetField(x, y);
                if (field.TerrainType is 7 or 8 or 15 || map.SeatedPlayer == -1 || map.ExplorationEnabled && !field.GetExplored(map.SeatedPlayer)) continue;
                RandSeed = field.HexX + field.HexY * 8 + ((field.FieldFlags >> 16) & 255) * 65536;
                if (Random(100) >= chance) continue;
                int delay = Random(40);
                CastImages.Show(0, pixelX, pixelY, animation.FrameCounter - delay, surface);
                if (animation.FrameCounter == delay && GetEngine().AnimationDetail != 0)
                    AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[x % 2]);
            }
        }
        if (animation.FrameCounter == 90) animation.Dispose();
    }
}
