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
        FSpellName = AbilityRules.TranslateRStr(GameText.FountainOfLifeRStr);
        FSpellID = 52;
        FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Water;
        FEnchantmentID = CityEnchantmentID.FountainOfLife;
        FCastDuration = 20;
        FAIPriority = 500;
        FAICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.LeaderInCity);
        FAIBudget = 600;
        FEnchantmentSpellID = 50;
    }

    // CitySpells.pas:692
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        City TargetCity = default;
        Animation A = default;
        Result = base.ExecuteTE(Event, Source, ExecuteCount);
        if (ExecuteCount == 0)
        {
            TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
            A = new Animation(null);
            A.FAnimationLevel = 27;
            A.FOnShow = ShowFountain;
            A.FAnimationMap = AoWHSMap.Current;
            A.FTag = (int)(((uint)(int)TargetCity.GetXhx()) | ((uint)(TargetCity.GetYhx() << 8)));
            A.Activate();
        }

        return Result;
    }

    // CitySpells.pas:666
    public void ShowFountain(Animation Animation, int X, int Y, in SurfaceDesc SD)
    {
        int DX;
        int VY;
        int Offset;
        int CY = default;
        int CX = default;
        int VX;
        int T;
        int DY;
        HexGeometry.HXtoHP(Animation.FTag & 255, (int)(((uint)Animation.FTag) >> 8), ref CX, ref CY);
        CX = CX + 10 - X;
        CY = CY - Y + 10;
        // { Original resets the process-wide random seed for every draw. }
        RandSeed = 0;
        for (int I = 0; I <= 100; I++)
        {
            VX = Random(100) - 50;
            VY = Random(5) + 7;
            Offset = Round((double)VX * 0.1);
            T = Random(30);
            if (T < Animation.FFrameCounter && Animation.FFrameCounter - T < 90)
            {
                T = (Animation.FFrameCounter - T) % 30;
                DX = Round((double)(T * VX) / 30);
                DY = VY * T - Round((double)((double)T * 0.25) * T);
                if (T > 0)
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(52).FCastImages.Show(0, CX + DX + Offset, CY - DY, 0, SD);
                }
            }
        }

        if (Animation.FFrameCounter == 120)
        {
            Animation?.Dispose();
        }
    }
}
