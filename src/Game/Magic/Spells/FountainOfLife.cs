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
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Spells;

public class FountainOfLife : CityEnchantmentSpell
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
        bool result = base.ExecuteTokenEvent(eventValue, source: source, executeCount: executeCount);
        if (executeCount == 0)
        {
            City targetCity = eventValue.GetSelectedCity();
            Animation a = new(null)
            {
                Holder = CreationHolder(),
                AnimationLevel = 27
            };
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
        HexGeometry.HexToPixel(animation.Tag & 255, (int)(((uint)animation.Tag) >> 8), out int cx, out int cy);
        cx = cx + 10 - X;
        cy = cy - Y + 10;
        // { Original resets the process-wide random seed for every draw. }
        RandSeed = 0;
        for (int i = 0; i <= 100; i++)
        {
            int vx = Random(100) - 50;
            int vy = Random(5) + 7;
            int offset = Round(vx * 0.1);
            int t = Random(30);
            if (t < animation.FrameCounter && animation.FrameCounter - t < 90)
            {
                t = (animation.FrameCounter - t) % 30;
                int deltaX = Round((double)(t * vx) / 30);
                int deltaY = (vy * t) - Round((double)(t * 0.25) * t);
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
