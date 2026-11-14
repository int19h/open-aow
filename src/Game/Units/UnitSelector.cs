// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public partial class UnitSelector : IDisposable
{
    // Ability.PAS:575
    public UnitSelector(EngineObject creator)
    {
        Creator = creator;
        Units = new UnitUserList(null) { Holder = Creator.CreationHolder() };
        _references = 1;
        Canceled = false;
    }

    // The node that created this selector. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; }

    private int _references;

    public sbyte Player { get; private set; }
    public string Caption { get; set; } = "";
    public UnitUserList Units { get; }
    public byte Selection { get; private set; }
    public bool Canceled { get; }
    public byte Selectable { get; private set; }
    public int SourceUnitId { get; private set; }

    private event NotifyEvent OnSelect;
    private event NotifyEvent OnCancel;

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
    public virtual void Setup(Troop source, UnitCollection units)
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
            error = AbilityRules.TranslateResourceString(Creator.GetEngine(), GameText.NoUnitSelectedText);
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
        _references += 1;
        return _references;
    }

    // Ability.PAS:595
    public int Release()
    {
        int count;
        _references -= 1;
        count = _references;
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
