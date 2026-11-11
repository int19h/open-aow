// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Commands;

using Aow.Game.Units;

namespace Aow.Game.Events;

// selection. location. FTokenEvent. owned object
public partial class CombatTypeRequestEventLog : EventLog
{
    public CombatTypeRequestEventLog(ECustomNode owner) : base(owner)
    { }

    public CombatType FCombatType;
    public new sbyte FPlayer;
    public sbyte FDefenderPlayer;
    public sbyte FX;
    public sbyte FY;
    public sbyte FL;
    public EngineObject FTokenEvent;
    public bool FCreateNewMove;
    public ArmyId FArmyID;

    // Combat2.pas:1
    public override void Dispose()
    {
        object Obj;
        SetTokenEvent(null);
        Obj = FArmyID;
        if (Obj != null)
        {
            (Obj as IDisposable)?.Dispose();
        }

        base.Dispose();
    }

    // Combat2.pas:18
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done) => base.Execute(Mode, Trigger, Done);

    // Combat2.pas:10
    public void SetTokenEvent(TokenEvent TokenEvent)
    {
        // { Original releases the old token before retaining its replacement. }
        if (FTokenEvent != null)
        {
            FTokenEvent.Release();
        }

        FTokenEvent = TokenEvent;
        if (FTokenEvent != null)
        {
            FTokenEvent.AddRef();
        }
    }

    // Combat2.pas:27
    public void Select(CombatType CombatType)
    {
        FCombatType = CombatType;
    }

    // Combat2.pas:30
    public void SetCenterLocation(byte X, byte Y, int L)
    {
        FX = (sbyte)X;
        FY = (sbyte)Y;
        FL = (sbyte)L;
    }

}
