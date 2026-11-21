#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

public class FireMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1733
    public FireMastery(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FireMasteryText);
        SpellId = 63;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Fire;
        CastDuration = 40;
        AIBasePriority = 1_000;
    }

    // GlobalEnchantmentSpells.pas:1792
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        if (executeCount == 0)
        {
            MapCoordinate coordinate66 = source.GetLocation();
            sbyte x = coordinate66.X;
            sbyte y = coordinate66.Y;
            sbyte l = coordinate66.MapLevel;
            SpellTargets.NotifySpellSphere(source.Player, sphere: MagicSphere.Fire);
            _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, radius: 0);
            Animation animation = new(null)
            {
                Holder = CreationHolder(),
                AnimationLevel = 27
            };
            animation.SetShowHandler(ShowAnimation);
            animation.AnimationMap = StrategicMap.Current;
            animation.Tag = (StrategicMap.Current.ViewWidth & 0xFFFF) | (StrategicMap.Current.ViewHeight << 16);
            animation.Activate();

            _ = CastSounds.Play(0);
            CastingDone(source, mana: eventValue.EventMana);
        }
        if (executeCount == 20)
        {
            _ = base.ExecuteTokenEvent(eventValue, source, executeCount: 0);
            return false;
        }
        else
        {
            return true;
        }
    }

    // GlobalEnchantmentSpells.pas:1746
    public override GlobalEnchantment CreateEnchantment() => new FireMasteryEnchantment(null, holder: CreationHolder());

    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface surface)
    {
        int chance = GetEngine().AnimationDetail switch { 0 => 2, 2 => 5, _ => 3 };
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
                if (field.TerrainType is 7 or 8 or 15 || map.SeatedPlayer == -1 || (map.ExplorationEnabled && !field.GetExplored(map.SeatedPlayer)))
                {
                    continue;
                }

                RandSeed = field.HexX + (field.HexY * 8) + (field.FogCount * 65_536);
                if (Random(100) >= chance)
                {
                    continue;
                }

                int delay = Random(40);
                _ = CastImages.Show(0, pixelX, pixelY, frame: animation.FrameCounter - delay, lockedSurface: surface);
                if (animation.FrameCounter == delay && GetEngine().AnimationDetail != 0)
                {
                    _ = AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[x % 2]);
                }
            }
        }
        if (animation.FrameCounter == 90)
        {
            animation.Dispose();
        }
    }
}
