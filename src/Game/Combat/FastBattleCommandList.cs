// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// source, source side, target side, combat, command list
public partial class FastBattleCommandList : IDisposable
{
    // AIGroup.pas:6716
    public FastBattleCommandList(CombatObject source, FastBattle combat, CombatSide side)
    {
        _commands = new DelphiList<FastBattleCommand>();
        Source = source;
        Combat = combat;
        SourceSide = side;
        if (SourceSide.SideIndex == 0)
        {
            TargetSide = Combat.Sides[(int)CombatSideIndex.Defender];
        }
        else
        {
            TargetSide = Combat.Sides[(int)CombatSideIndex.Attacker];
        }
    }

    public CombatObject Source { get; }
    public CombatSide SourceSide { get; }
    public CombatSide TargetSide { get; }
    public FastBattle Combat { get; }

    private DelphiList<FastBattleCommand> _commands;

    // AIGroup.pas:6727
    public virtual void Dispose()
    {
        Clear();
        _commands?.Dispose();
    }

    // AIGroup.pas:6734
    public void Clear()
    {
        for (int I = _commands.Count - 1; I >= 0; I--)
        {
            (_commands.List[I] as IDisposable)?.Dispose();
        }

        _commands.Clear();
    }

    // AIGroup.pas:6742
    public FastBattleCommand GetCommands(int index) => _commands.List[index];

    // AIGroup.pas:6747
    public int GetCount() => _commands.Count;

    // AIGroup.pas:6752
    public void Add(FastBattleCommand command)
    {
        _commands.Add(command);
    }

    // AIGroup.pas:6757
    public void Delete(int index)
    {
        (_commands.List[index] as IDisposable)?.Dispose();
        _commands.Delete(index);
    }

    // AIGroup.pas:6768
    public void Sort()
    {
        _commands.Sort(CompareFastBattleCommands);
    }

    private static int CompareFastBattleCommands(FastBattleCommand item1, FastBattleCommand item2)
    {
        return item2.ExpectedDamageValue - item1.ExpectedDamageValue;
    }

    // intf_P5_05.inc:23

    // intf_P5_05.inc:23

}
