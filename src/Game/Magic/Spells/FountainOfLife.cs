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
    public FountainOfLife(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.FountainOfLifeText);
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
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result = default;
        City targetCity = default;
        Animation A = default;
        result = base.ExecuteTokenEvent(eventValue, source, executeCount);
        if (executeCount == 0)
        {
            targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
            A = new Animation(null);
            A.AnimationLevel = 27;
            A.OnShow = ShowFountain;
            A.AnimationMap = AoWHSMap.Current;
            A.Tag = (int)(((uint)(int)targetCity.GetHexX()) | ((uint)(targetCity.GetHexY() << 8)));
            A.Activate();
        }

        return result;
    }

    // CitySpells.pas:666
    public void ShowFountain(Animation animation, int X, int Y, in LockedSurface lockedSurface)
    {
        int deltaX;
        int vy;
        int offset;
        int cy = default;
        int cx = default;
        int vx;
        int T;
        int deltaY;
        HexGeometry.HexToPixel(animation.Tag & 255, (int)(((uint)animation.Tag) >> 8), ref cx, ref cy);
        cx = cx + 10 - X;
        cy = cy - Y + 10;
        // { Original resets the process-wide random seed for every draw. }
        RandSeed = 0;
        for (int I = 0; I <= 100; I++)
        {
            vx = Random(100) - 50;
            vy = Random(5) + 7;
            offset = Round((double)vx * 0.1);
            T = Random(30);
            if (T < animation.FrameCounter && animation.FrameCounter - T < 90)
            {
                T = (animation.FrameCounter - T) % 30;
                deltaX = Round((double)(T * vx) / 30);
                deltaY = vy * T - Round((double)((double)T * 0.25) * T);
                if (T > 0)
                {
                    AoWHSSet.Current.SpellControl.GetSpell(52).CastImages.Show(0, cx + deltaX + offset, cy - deltaY, 0, lockedSurface);
                }
            }
        }

        if (animation.FrameCounter == 120)
        {
            animation?.Dispose();
        }
    }
}
