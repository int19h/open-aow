// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

namespace Aow.Game.Combat.Actions;

public partial class NewCombatTurnCA : CombatAction
{
    // Combat2.pas:982
    public new const int ClassID = 131770;

    public NewCombatTurnCA(ECustomNode owner) : base(owner)
    { }

    public byte F_NewCombatTurnCA_0C;

    // Combat2.pas:985
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref F_NewCombatTurnCA_0C, 20);
    }

    // Combat2.pas:991
    public override void Execute(Battle Combat)
    {
        Combat.FCombatData.FCurrentPlayer = F_NewCombatTurnCA_0C;
        for (int I = Combat.FCombatData.GetCount() - 1; I >= 0; I--)
        {
            Combat.FCombatData.GetObjects(I).NewTurn(F_NewCombatTurnCA_0C);
        }
    }

    // Combat2.pas:999
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            Execute(Viewer.FCombat);
            AudioPlayback.PlaySample("SFX\\EVENTS\\INFO2.WAV");
        }

        return Viewer.FDelay > Viewer.FTime;
    }

    // intf_P3_02.inc:38

}
