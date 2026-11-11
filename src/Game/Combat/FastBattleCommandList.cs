// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// source, source side, target side, combat, command list
public partial class FastBattleCommandList : IDisposable
{
    // AIGroup.pas:6716
    public FastBattleCommandList(CombatObject Source, FastBattle Combat, CombatSide Side)
    {
        FCommands = new DelphiList<FastBattleCommand>();
        FSource = Source;
        FCombat = Combat;
        FSourceSide = Side;
        if (FSourceSide.FSideIndex == 0)
        {
            FTargetSide = FCombat.FSides[(int)CombatSideIndex.Defender];
        }
        else
        {
            FTargetSide = FCombat.FSides[(int)CombatSideIndex.Attacker];
        }
    }

    public CombatObject FSource;
    public CombatSide FSourceSide;
    public CombatSide FTargetSide;
    public FastBattle FCombat;
    public DelphiList<FastBattleCommand> FCommands;

    // AIGroup.pas:6727
    public virtual void Dispose()
    {
        Clear();
        FCommands?.Dispose();
    }

    // AIGroup.pas:6734
    public void Clear()
    {
        for (int I = FCommands.Count - 1; I >= 0; I--)
        {
            (FCommands.List[I] as IDisposable)?.Dispose();
        }

        FCommands.Clear();
    }

    // AIGroup.pas:6742
    public FastBattleCommand GetCommands(int Index) => FCommands.List[Index];

    // AIGroup.pas:6747
    public int GetCount() => FCommands.Count;

    // AIGroup.pas:6752
    public void Add(FastBattleCommand Command)
    {
        FCommands.Add(Command);
    }

    // AIGroup.pas:6757
    public void Delete(int Index)
    {
        (FCommands.List[Index] as IDisposable)?.Dispose();
        FCommands.Delete(Index);
    }

    // AIGroup.pas:6768
    public void Sort()
    {
        FCommands.Sort(CompareFastCombatCommands);
    }

    private static int CompareFastCombatCommands(FastBattleCommand Item1, FastBattleCommand Item2)
    {
        return Item2.FExpectedDV - Item1.FExpectedDV;
    }

    // intf_P5_05.inc:23

    // intf_P5_05.inc:23

}
