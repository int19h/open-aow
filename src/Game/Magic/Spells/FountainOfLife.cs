#nullable disable
// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class FountainOfLife : CityEnchantmentSpell
{
    // CitySpells.pas:649
    public FountainOfLife(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.FountainOfLifeText);
        SpellId = 52;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Water;
        EnchantmentId = CityEnchantmentId.FountainOfLife;
        CastDuration = 20;
        AIPriority = 500;
        AICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.LeaderInCity);
        AIBudget = 600;
        EnchantmentSpellId = 50;
    }

    // CitySpells.pas:692
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result = default;
        City targetCity = default;
        Animation a = default;
        result = base.ExecuteTokenEvent(eventValue, source: source, executeCount: executeCount);
        if (executeCount == 0)
        {
            targetCity = eventValue.GetSelectedCity();
            a = new Animation(null) { Holder = CreationHolder() };
            a.AnimationLevel = 27;
            a.OnShow += ShowFountain;
            a.AnimationMap = StrategicMap.Current;
            a.Tag = (int)(((uint)(int)targetCity.GetHexX()) | ((uint)(targetCity.GetHexY() << 8)));
            a.Activate();
        }

        return result;
    }

    // CitySpells.pas:666
    public void ShowFountain(Animation animation, int X, int Y, in LockedSurface lockedSurface)
    {
        int deltaX;
        int vy;
        int offset;
        int vx;
        int t;
        int deltaY;
        HexGeometry.HexToPixel(animation.Tag & 255, (int)(((uint)animation.Tag) >> 8), out int cx, out int cy);
        cx = cx + 10 - X;
        cy = cy - Y + 10;
        // { Original resets the process-wide random seed for every draw. }
        RandSeed = 0;
        for (int i = 0; i <= 100; i++)
        {
            vx = Random(100) - 50;
            vy = Random(5) + 7;
            offset = Round(vx * 0.1);
            t = Random(30);
            if (t < animation.FrameCounter && animation.FrameCounter - t < 90)
            {
                t = (animation.FrameCounter - t) % 30;
                deltaX = Round((double)(t * vx) / 30);
                deltaY = (vy * t) - Round((double)(t * 0.25) * t);
                if (t > 0)
                {
                    _ = GamePack.Current.SpellControl.GetSpell(52).CastImages.Show(0, cx + deltaX + offset, cy - deltaY, 0, lockedSurface);
                }
            }
        }

        if (animation.FrameCounter == 120)
        {
            animation?.Dispose();
        }
    }
}
