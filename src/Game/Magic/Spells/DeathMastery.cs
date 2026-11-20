#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

public class DeathMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1577
    public DeathMastery(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.DeathMasteryText);
        SpellId = 62;
        BaseManaCost = 1;
        SpellLevel = 4;
        // { Original assigns Cosmos to Death Mastery. }
        MagicSphere = MagicSphere.Cosmos;
        CastDuration = 40;
        AIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1618
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Death);
        MapCoordinate coordinate63 = source.GetLocation();
        sbyte x = coordinate63.X;
        sbyte y = coordinate63.Y;
        sbyte l = coordinate63.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
        Animation animation = new(null)
        {
            Holder = CreationHolder(),
            AnimationLevel = 28
        };
        animation.OnShow += ShowAnimation;
        animation.AnimationMap = StrategicMap.Current;
        animation.Tag = (StrategicMap.Current.ViewWidth & 65535) | (StrategicMap.Current.ViewHeight << 16);
        animation.Activate();

        _ = CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1590
    public override GlobalEnchantment CreateEnchantment() => new DeathMasteryEnchantment(null, holder: CreationHolder());

    // GlobalEnchantmentSpells.pas:1593
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        RandSeed = StrategicMap.Current.Frame - animation.FrameCounter;
        int count = GetEngine().AnimationDetail switch
        {
            0 => 5,
            2 => 20,
            _ => 10,
        };
        for (int i = 0; i <= count - 1; i++)
        {
            int x = Random(animation.Tag & 65535);
            int y = Random((int)(uint)animation.Tag >>> 16);
            int speed = Random(15) + 5;
            int image = Random(3);
            // { Preserve the original image/displacement register lifetimes. }
            int d = (animation.FrameCounter - 45) * speed;
            x -= d * 2;
            y += d;
            _ = CastImages.Show(image, lockedSurface.ClipRectangle.Left + x, lockedSurface.ClipRectangle.Top + y, 0, lockedSurface);
        }

        if (animation.FrameCounter == 90)
        {
            animation.Dispose();
        }
    }
}
