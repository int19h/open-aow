// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// source, source side, target side, combat, command list
public partial class FastBattleCommandList : IDisposable
{
    // AIGroup.pas:6716
    public FastBattleCommandList(CombatObject source, FastBattle combat, CombatSide side)
    {
        Commands = new DelphiList<FastBattleCommand>();
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

    public CombatObject Source;
    public CombatSide SourceSide;
    public CombatSide TargetSide;
    public FastBattle Combat;
    public DelphiList<FastBattleCommand> Commands;

    // AIGroup.pas:6727
    public virtual void Dispose()
    {
        Clear();
        Commands?.Dispose();
    }

    // AIGroup.pas:6734
    public void Clear()
    {
        for (int I = Commands.Count - 1; I >= 0; I--)
        {
            (Commands.List[I] as IDisposable)?.Dispose();
        }

        Commands.Clear();
    }

    // AIGroup.pas:6742
    public FastBattleCommand GetCommands(int index) => Commands.List[index];

    // AIGroup.pas:6747
    public int GetCount() => Commands.Count;

    // AIGroup.pas:6752
    public void Add(FastBattleCommand command)
    {
        Commands.Add(command);
    }

    // AIGroup.pas:6757
    public void Delete(int index)
    {
        (Commands.List[index] as IDisposable)?.Dispose();
        Commands.Delete(index);
    }

    // AIGroup.pas:6768
    public void Sort()
    {
        Commands.Sort(CompareFastBattleCommands);
    }

    private static int CompareFastBattleCommands(FastBattleCommand item1, FastBattleCommand item2)
    {
        return item2.ExpectedDamageValue - item1.ExpectedDamageValue;
    }

    // intf_P5_05.inc:23

    // intf_P5_05.inc:23

}
