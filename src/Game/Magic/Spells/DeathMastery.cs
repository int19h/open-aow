// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class DeathMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1577
    public DeathMastery(ECustomNode owner) : base(owner)
    {
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GameText.DeathMasteryText);
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
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Death);
        source.GetLocation(out X, out Y, out L);
        StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        var animation = new Animation(null);
        animation.AnimationLevel = 28;
        animation.OnShow += ShowAnimation;
        animation.AnimationMap = StrategicMap.Current;
        animation.Tag = (StrategicMap.Current.ViewWidth & 65535) | (StrategicMap.Current.ViewHeight << 16);
        animation.Activate();

        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1590
    public override GlobalEnchantment CreateEnchantment() => new DeathMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:1593
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int count;
        int X;
        int Y;
        int speed;
        int D;
        int image;
        RandSeed = StrategicMap.Current.Frame - animation.FrameCounter;
        switch (GameTables.GameEngine.AnimationDetail)
        {
            case 0:
                count = 5;
                break;
            case 2:
                count = 20;
                break;
            default:
                count = 10;
                break;
        }

        for (int I = 0; I <= count - 1; I++)
        {
            X = Random(animation.Tag & 65535);
            Y = Random((int)(((uint)(int)(uint)animation.Tag) >> 16));
            speed = Random(15) + 5;
            image = Random(3);
            // { Preserve the original image/displacement register lifetimes. }
            D = (animation.FrameCounter - 45) * speed;
            X -= D * 2;
            Y += D;
            CastImages.Show(image, lockedSurface.ClipRectangle.Left + X, lockedSurface.ClipRectangle.Top + Y, 0, lockedSurface);
        }

        if (animation.FrameCounter == 90)
        {
            animation.Dispose();
        }
    }
}
