// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Commands;

// FUnitIndices
public partial class SummonSpellTokenEvent : SpellTokenEvent
{
    // SummonSpells.pas:171
    public new const int ClassID = 135947;

    public UnitIndexList FUnitIndices;

    // SummonSpells.pas:165
    public override void Dispose()
    {
        FUnitIndices?.Dispose();
        base.Dispose();
    }

    // SummonSpells.pas:176
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(30, FUnitIndices.LoadFromStream, FUnitIndices.SaveToStream);
    }

    // SummonSpells.pas:159
    public SummonSpellTokenEvent(ECustomNode owner) : base(owner)
    {
        FUnitIndices = new UnitIndexList();
    }

    // SummonSpells.pas:182
    public override bool Process(Troop source, int value)
    {
        bool Done;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Unit U;
        ArmyHexSprite Army;
        UnitEnchantment Enchantment;
        AIGroup Group;
        ImageSequenceAnimation Anim;
        base.Process(source, value);
        Done = false;
        source.GetLocation(ref X, ref Y, ref L);
        for (int I = 0, last = FUnitIndices.Count - 1; I <= last; I++)
        {
            U = new Unit(null);
            var unitIndice = FUnitIndices[I];
            U.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(unitIndice));
            Army = U.Place(X, Y, L, source.FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
            if (Army != null)
            {
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                if (HexGeometry.dHXtoRad(X, Y, xhx, yhx) <= 2)
                {
                    if (U.ExpandAbility(155))
                    {
                        Enchantment = ((UnitEnchantmentAbilityData)U.GetAbilityData(155)).FEnchantment;
                        Enchantment.SetUpkeep(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FManaUpkeep);
                        Enchantment.SetSource(source.FPlayer);
                        Enchantment.FDispelResistance = (sbyte)((SummonSpell)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).GetDispelResistance(U);
                        Group = source.FGroup;
                        if (Group != null)
                        {
                            Group.AddUnit(U);
                        }
                    }

                    Done = true;
                }
                else
                {
                    U.SetOwner(null);
                }
            }

            U.Release();
        }

        if (Done)
        {
            if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                AoWHSMap.Current.MakeVisible(X, Y, L, 3);
            }

            AoWHSMap.Current.ValidateObservation(source.FPlayer, X, Y, L, 0);
            if (AoWHSMap.Current.Visible(X, Y, L, 0))
            {
                if (AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
                {
                    Anim = new ImageSequenceAnimation(null);
                    Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
                    AoWHSMap.Current.PlaceHX(Anim, X, Y, L);
                }

                if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, L, 0))
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
                }
            }

            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
        }
        else
        {
            if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                var noSummonLocationFound = AbilityRules.TranslateRStr(NoSummonLocationFoundRStr);
                AoWHSMap.Current.ShowMessage(noSummonLocationFound);
            }
        }

        return false;
    }

    private static readonly string NoSummonLocationFoundRStr = "No nearby summon location found";

    // SummonSpells.pas:34

    // SummonSpells.pas:34

}
