// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class AirMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:688
    public AirMastery(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.AirMasteryText);
        SpellId = 60;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 3;
        // { Original also assigns Earth to Air Mastery. }
        MagicSphere = MagicSphere.Earth;
        CastDuration = 40;
        AIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:727
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Animation animation = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Air);
        source.GetLocation(out X, out Y, out L);
        StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        animation = new Animation(null);
        animation.AnimationLevel = 28;
        animation.OnShow += ShowAnimation;
        animation.AnimationMap = StrategicMap.Current;
        animation.Tag = (StrategicMap.Current.ViewWidth & 65535) | (StrategicMap.Current.ViewHeight << 16);
        animation.Activate();
        GamePack.Current.SpellControl.GetSpell(60).CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:747
    public override GlobalEnchantment CreateEnchantment() => new AirMasteryEnchantment(null);

    // GlobalEnchantmentSpells.pas:701
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int count;
        int X;
        int Y;
        int speed;
        int image;
        if (StrategicMap.Current.ViewMapLevel == 0)
        {
            RandSeed = StrategicMap.Current.Frame - animation.FrameCounter;
            switch ((int)GameTables.GameEngine.AnimationDetail)
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
                X += (animation.FrameCounter - 45) * speed * 2;
                Y -= (animation.FrameCounter - 45) * speed;
                CastImages.Show(image, lockedSurface.ClipRectangle.Left + X, lockedSurface.ClipRectangle.Top + Y, 0, lockedSurface);
            }
        }

        if (animation.FrameCounter == 90)
        {
            animation.Dispose();
        }
    }
}
