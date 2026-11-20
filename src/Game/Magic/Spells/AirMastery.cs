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

public class AirMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:688
    public AirMastery(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.AirMasteryText);
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
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Air);
        MapCoordinate coordinate50 = source.GetLocation();
        sbyte x = coordinate50.X;
        sbyte y = coordinate50.Y;
        sbyte l = coordinate50.MapLevel;
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
        _ = GamePack.Current.SpellControl[60].CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:747
    public override GlobalEnchantment CreateEnchantment() => new AirMasteryEnchantment(null, holder: CreationHolder());

    // GlobalEnchantmentSpells.pas:701
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (StrategicMap.Current.ViewMapLevel == 0)
        {
            RandSeed = StrategicMap.Current.Frame - animation.FrameCounter;
            int count = (int)GetEngine().AnimationDetail switch
            {
                0 => 5,
                2 => 20,
                _ => 10,
            };
            for (int i = 0; i <= count - 1; i++)
            {
                int x = Random(animation.Tag & 65535);
                int y = Random((int)(((uint)(int)(uint)animation.Tag) >> 16));
                int speed = Random(15) + 5;
                int image = Random(3);
                x += (animation.FrameCounter - 45) * speed * 2;
                y -= (animation.FrameCounter - 45) * speed;
                _ = CastImages.Show(image, lockedSurface.ClipRectangle.Left + x, lockedSurface.ClipRectangle.Top + y, 0, lockedSurface);
            }
        }

        if (animation.FrameCounter == 90)
        {
            animation.Dispose();
        }
    }
}
