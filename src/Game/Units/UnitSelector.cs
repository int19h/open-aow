// Translated from AoWE.pas.
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class UnitSelector : IDisposable
{
    // Ability.PAS:575
    public UnitSelector()
    {
        Units = new UnitUserList(null);
        References = 1;
        Canceled = false;
    }

    public int References;
    public sbyte Player;
    public string Caption = "";
    public UnitUserList Units;
    public byte Selection;
    public bool Canceled;
    public byte Selectable;
    public int SourceUnitId;
    public NotifyEvent OnSelect;
    public NotifyEvent OnCancel;

    // Ability.PAS:583
    public virtual void Dispose()
    {
        Units?.Dispose();
    }

    // Ability.PAS:605
    public virtual void SetSelection(byte value)
    {
        Selection = (byte)(value & Selectable);
    }

    // Ability.PAS:656
    public virtual bool CanSelectUnit(Troop troop) => true;

    // Ability.PAS:662
    public virtual void Setup(Troop source, UnitList units)
    {
        if (source != null)
        {
            SourceUnitId = source.UnitId;
            Player = source.Player;
        }

        Units.Clear();
        Units.AddUnits(units);
        Units.SortUnits();
        // { Original accumulates FSelectable rather than clearing it on a new setup. }
        for (int I = Units.GetCount() - 1; I >= 0; I--)
        {
            // { The original hard-casts to TUnit, which a hero passes through to the virtual calls. }
            if (CanSelectUnit(Units.GetUnit(I)))
            {
                Selectable = (byte)(Selectable | (1 << I));
            }
        }
    }

    // Ability.PAS:623
    public virtual bool CanSelect(ref string error)
    {
        bool result;
        if (Selection == 0)
        {
            error = AbilityRules.TranslateResourceString(GameText.s_noUnitSelectedText);
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Ability.PAS:651
    public virtual void Cancel()
    {
        if (OnCancel != null)
        {
            OnCancel(this);
        }
    }

    // Ability.PAS:589
    public int AddRef()
    {
        References += 1;
        return References;
    }

    // Ability.PAS:595
    public int Release()
    {
        int count;
        References -= 1;
        count = References;
        int result = count;
        if (count == 0)
        {
            Dispose();
        }

        return result;
    }

    // Ability.PAS:610
    public int GetSelectionCount()
    {
        uint bits;
        int result = 0;
        bits = Selection;
        while ((int)bits != 0)
        {
            if ((((int)bits) & 1) != 0)
            {
                result += 1;
            }

            bits = (uint)(int)(((uint)(int)bits) >> 1);
        }

        return result;
    }

    // intf_P1_08.inc:65
    public virtual bool Select() { OnSelect?.Invoke(this); return true; }

    public delegate void NotifyEvent(UnitSelector Selector);
}
