// Translated from AoWE.pas.
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class UnitSelector : IDisposable
{
    // Ability.PAS:575
    public UnitSelector()
    {
        FUnits = new UnitUserList(null);
        FReferences = 1;
        FCanceled = false;
    }

    public int FReferences;
    public sbyte FPlayer;
    public string FCaption = "";
    public UnitUserList FUnits;
    public byte FSelection;
    public bool FCanceled;
    public byte FSelectable;
    public int FSourceUnitID;
    public NotifyEvent FOnSelect;
    public NotifyEvent FOnCancel;

    // Ability.PAS:583
    public virtual void Dispose()
    {
        FUnits?.Dispose();
    }

    // Ability.PAS:605
    public virtual void SetSelection(byte Value)
    {
        FSelection = (byte)(Value & FSelectable);
    }

    // Ability.PAS:656
    public virtual bool CanSelectUnit(AbstractUnit AUnit) => true;

    // Ability.PAS:662
    public virtual void Setup(AbstractUnit Source, UnitList Units)
    {
        if (Source != null)
        {
            FSourceUnitID = Source.FUnitID;
            FPlayer = Source.FPlayer;
        }

        FUnits.Clear();
        FUnits.AddUnits(Units);
        FUnits.SortUnits();
        // { Original accumulates FSelectable rather than clearing it on a new setup. }
        for (int I = FUnits.GetCount() - 1; I >= 0; I--)
        {
            // { The original hard-casts to TUnit, which a hero passes through to the virtual calls. }
            if (CanSelectUnit(FUnits.GetUnit(I)))
            {
                FSelectable = (byte)(FSelectable | (1 << I));
            }
        }
    }

    // Ability.PAS:623
    public virtual bool CanSelect(ref string Error)
    {
        bool Result;
        if (FSelection == 0)
        {
            Error = AbilityRules.TranslateRStr(GameText.NoUnitSelectedRStr);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Ability.PAS:651
    public virtual void Cancel()
    {
        if (FOnCancel != null)
        {
            FOnCancel(this);
        }
    }

    // Ability.PAS:589
    public int AddRef()
    {
        FReferences += 1;
        return FReferences;
    }

    // Ability.PAS:595
    public int Release()
    {
        int Count;
        FReferences -= 1;
        Count = FReferences;
        int Result = Count;
        if (Count == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Ability.PAS:610
    public int GetSelectionCount()
    {
        uint Bits;
        int Result = 0;
        Bits = FSelection;
        while ((int)Bits != 0)
        {
            if ((((int)Bits) & 1) != 0)
            {
                Result += 1;
            }

            Bits = (uint)(int)(((uint)(int)Bits) >> 1);
        }

        return Result;
    }

    // intf_P1_08.inc:65
    public virtual bool Select() { FOnSelect?.Invoke(this); return true; }

    public delegate void NotifyEvent(UnitSelector Selector);
}
