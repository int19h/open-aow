#nullable disable warnings
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
    public AirMastery(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.AirMasteryText);
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
        SpellTargets.NotifySpellSphere(source.Player, sphere: MagicSphere.Air, engine: GetEngine()!);
        MapCoordinate coordinate50 = source.GetLocation();
        int x = coordinate50.X;
        int y = coordinate50.Y;
        int l = coordinate50.MapLevel;
        _ = GetEngine().StrategicMap.ValidateObservation(source.Player, x, y, l, radius: 0);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Animation animation = new(null, show: ShowAnimation)
        {
            Holder = CreationHolder(),
            AnimationLevel = 28
,
            AnimationMap = strategicMap,
            Tag = (strategicMap.ViewWidth & 0xFFFF) | (strategicMap.ViewHeight << 16)
        };
        animation.Activate();
        _ = GetEngine().GamePack.SpellControl[60].CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:747
    public override GlobalEnchantment CreateEnchantment() => new AirMasteryEnchantment(null, holder: CreationHolder());

    // GlobalEnchantmentSpells.pas:701
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.ViewMapLevel == 0)
        {
            RandSeed = strategicMap.Frame - animation.FrameCounter;
            int count = GetEngine().AnimationDetail switch
            {
                0 => 5,
                2 => 20,
                _ => 10,
            };
            for (int i = 0; i <= count - 1; i++)
            {
                int x = Random(animation.Tag & 0xFFFF);
                int y = Random((int)(uint)animation.Tag >>> 16);
                int speed = Random(15) + 5;
                int image = Random(3);
                x += (animation.FrameCounter - 45) * speed * 2;
                y -= (animation.FrameCounter - 45) * speed;
                _ = CastImages.Show(image, lockedSurface.ClipRectangle.Left + x, lockedSurface.ClipRectangle.Top + y, frame: 0, lockedSurface);
            }
        }

        if (animation.FrameCounter == 90)
        {
            animation.Dispose();
        }
    }
}
