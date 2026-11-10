// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// (target-unit users, selection bits)
public partial class UnitSpellCaster : SpellCaster
{
    // Spells.PAS:859
    public UnitSpellCaster(ECustomNode owner) : base(owner)
    {
        FTargetUnits = new UnitUserList(null);
    }

    public UnitUserList FTargetUnits;
    public byte FSelectionMask;
    public bool FAllowMultipleTargets;
    public byte FValidSelection;

    // Spells.PAS:864
    public override void Dispose()
    {
        FTargetUnits?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:947
    public override bool CanCast(ref string Reason)
    {
        bool Result;
        Hero Hero;
        int Mana;
        Hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FCasterSourceID);
        if (Hero != null && !Hero.FUnitStatus.Contains(UnitStatusElement.Killed))
        {
            if (FSelectionMask != 0)
            {
                if (!AoWHSMap.Current.FPlayers.GetPlayers(Hero.FPlayer).FMagicControl.GetCheatEnabled())
                {
                    Mana = GetRequiredMana();
                    if (Hero.CastingReady() && Hero.FCastingSpellID == FCasterSpellID)
                    {
                        Mana -= AoWHSSet.Current.FSpellControl.GetSpell(FCasterSpellID).FBaseManaCost;
                    }

                    if (Mana <= Hero.GetCastingPoints())
                    {
                        if (Mana <= AoWHSMap.Current.FPlayers.GetPlayers(Hero.FPlayer).FMagicControl.FMana)
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            Reason = AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
                        }
                    }
                    else
                    {
                        Result = false;
                        Reason = AbilityRules.TranslateRStr(GameText.P2NotEnoughCastingPointsRStr);
                    }
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                Result = false;
                Reason = AbilityRules.TranslateRStr(GameText.NoUnitSelectedRStr);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:981
    public override bool Cast()
    {
        bool Result;
        UnitUserList Targets;
        string Reason = "";
        AbstractUnit Source;
        if (CanCast(ref Reason))
        {
            Source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
            if (Source != null)
            {
                Targets = new UnitUserList(null);
                for (int I = 0, last = FTargetUnits.Count - 1; I <= last; I++)
                {
                    if (((1 << I) & FSelectionMask) != 0)
                    {
                        Targets.AddUnit(FTargetUnits.GetUnit(I));
                    }
                }

                Result = ((UnitSpell)AoWHSSet.Current.FSpellControl.GetSpell(FCasterSpellID)).CastSpell(Source, Targets);
                Targets?.Dispose();
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:896
    public int GetSelectionCount()
    {
        int Bits;
        int Result = 0;
        Bits = FSelectionMask;
        while (Bits != 0)
        {
            if ((Bits & 1) != 0)
            {
                Result += 1;
            }

            Bits = (int)(((uint)Bits) >> 1);
        }

        return Result;
    }

    // Spells.PAS:1085
    public bool Valid()
    {
        bool Result;
        AbstractUnit Source;
        Source = AoWHSMap.Current.FUnitControl.FindUnit<AbstractUnit>(FCasterSourceID);
        if (Source == null || (((byte)Source.FUnitStatus.Image) & 16) != 0)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // intf_P4_02.inc:1038
    public void SetSelection(byte selection)
    {
        byte value = selection;
        var source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
        ((UnitSpell)AoWHSSet.Current.FSpellControl.GetSpell(FCasterSpellID)).ValidateUnitSelection(source, FTargetUnits, ref value);
        FSelectionMask = value;
    }
}
