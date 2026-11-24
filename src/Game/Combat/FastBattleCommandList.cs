#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// source, source side, target side, combat, command list
public sealed class FastBattleCommandList : IDisposable
{
    // AIGroup.pas:6716
    public FastBattleCommandList(CombatObject source, FastBattle combat, CombatSide side)
    {
        _commands = [];
        Source = source;
        Combat = combat;
        SourceSide = side;
        TargetSide = SourceSide.SideIndex == 0 ? Combat.Sides[(int)CombatSideIndex.Defender] : Combat.Sides[(int)CombatSideIndex.Attacker];
    }

    public CombatObject Source { get; }
    public CombatSide SourceSide { get; }
    public CombatSide TargetSide { get; }
    public FastBattle Combat { get; }

    private readonly DelphiList<FastBattleCommand> _commands;

    // AIGroup.pas:6727
    public void Dispose() => Clear();

    // AIGroup.pas:6734
    public void Clear()
    {
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            (_commands.List[i] as IDisposable)?.Dispose();
        }

        _commands.Clear();
    }

    // AIGroup.pas:6742
    public FastBattleCommand? this[int index] => _commands.List[index];

    // AIGroup.pas:6747
    public int GetCount() => _commands.Count;

    // AIGroup.pas:6752
    public void Add(FastBattleCommand command) => _commands.Add(command);

    // AIGroup.pas:6757
    public void Delete(int index)
    {
        (_commands.List[index] as IDisposable)?.Dispose();
        _commands.Delete(index);
    }

    // AIGroup.pas:6768
    public void Sort() => _commands.Sort(CompareFastBattleCommands);

    private static int CompareFastBattleCommands(FastBattleCommand? item1, FastBattleCommand? item2) => item2.ExpectedDamageValue - item1.ExpectedDamageValue;
}
