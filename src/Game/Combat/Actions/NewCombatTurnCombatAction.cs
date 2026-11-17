// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Audio;

namespace Aow.Game.Combat.Actions;

public partial class NewCombatTurnCombatAction(CustomNode owner) : CombatAction(owner)
{
    // Combat2.pas:982
    public new const int ClassId = 131770;

    public byte CombatPlayer { get; set; }

    // Combat2.pas:985
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        CombatPlayer = persister.Byte("CombatPlayer", value: CombatPlayer);
    }

    // Combat2.pas:991
    public override void Execute(Battle combat)
    {
        combat.CombatData.CurrentPlayer = CombatPlayer;
        for (int i = combat.CombatData.GetCount() - 1; i >= 0; i--)
        {
            combat.CombatData.GetObjects(i).NewTurn(CombatPlayer);
        }
    }

    // Combat2.pas:999
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            Execute(viewer.Combat);
            _ = AudioPlayback.PlaySample("SFX\\EVENTS\\INFO2.WAV");
        }

        return viewer.Delay > viewer.Time;
    }

    // intf_P3_02.inc:38

}
