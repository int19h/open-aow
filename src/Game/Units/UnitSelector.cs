#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

public class UnitSelector : IDisposable
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

    private event Action<UnitSelector> OnSelect;
    private event Action<UnitSelector> OnCancel;

    // Ability.PAS:583
    public virtual void Dispose() => Units?.Dispose();

    // Ability.PAS:605
    public virtual void SetSelection(byte value) => Selection = (byte)(value & Selectable);

    // Ability.PAS:656
    public virtual bool CanSelectUnit(Troop troop) => true;

    // Ability.PAS:662
    public virtual void Setup(Troop source, UnitCollection units)
    {
        if (source is not null)
        {
            SourceUnitId = source.UnitId;
            Player = source.Player;
        }

        Units.Clear();
        _ = Units.AddUnits(units);
        Units.SortUnits();
        // { Original accumulates FSelectable rather than clearing it on a new setup. }
        for (int i = Units.GetCount() - 1; i >= 0; i--)
        {
            // { The original hard-casts to TUnit, which a hero passes through to the virtual calls. }
            if (CanSelectUnit(Units.GetUnit(i)))
            {
                Selectable = (byte)(Selectable | (1 << i));
            }
        }
    }

    // Ability.PAS:623
    public virtual bool CanSelect(ref string error)
    {
        if (Selection == 0)
        {
            error = AbilityRules.TranslateResourceString(Creator.GetEngine(), S: GameText.NoUnitSelectedText);
            return false;
        }
        else
        {
            return true;
        }
    }

    // Ability.PAS:651
    public virtual void Cancel() => OnCancel?.Invoke(this);

    // Ability.PAS:589
    public int AddRef()
    {
        _references += 1;
        return _references;
    }

    // Ability.PAS:595
    public int Release()
    {
        _references -= 1;
        int count = _references;
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
        int result = 0;
        uint bits = Selection;
        while ((int)bits != 0)
        {
            if ((((int)bits) & 1) != 0)
            {
                result += 1;
            }

            bits = (uint)((int)bits >>> 1);
        }

        return result;
    }

    // intf_P1_08.inc:65
    public virtual bool Select()
    {
        OnSelect?.Invoke(this);
        return true;
    }
}
