// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Commands;

using Aow.Game.Units;

namespace Aow.Game.Events;

// selection. location. TokenEvent. owned object
public partial class CombatTypeRequestEventLog : EventLog
{
    public CombatTypeRequestEventLog(ECustomNode owner) : base(owner)
    { }

    public CombatType CombatType { get; private set; }
    public sbyte Player { get; set; }
    public sbyte DefenderPlayer { get; set; }
    public sbyte X { get; set; }
    public sbyte Y { get; set; }
    public sbyte MapLevel { get; set; }

    private EngineObject _tokenEvent;

    public bool CreateNewMove { get; set; }
    public ArmyId ArmyId { get; set; }

    // Combat2.pas:1
    public override void Dispose()
    {
        object objectValue;
        SetTokenEvent(null);
        objectValue = ArmyId;
        if (objectValue != null)
        {
            (objectValue as IDisposable)?.Dispose();
        }

        base.Dispose();
    }

    // Combat2.pas:18
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done) => base.Execute(mode, trigger, done);

    // Combat2.pas:10
    public void SetTokenEvent(TokenEvent tokenEvent)
    {
        // { Original releases the old token before retaining its replacement. }
        if (_tokenEvent != null)
        {
            _tokenEvent.Release();
        }

        _tokenEvent = tokenEvent;
        if (_tokenEvent != null)
        {
            _tokenEvent.AddRef();
        }
    }

    // Combat2.pas:27
    public void Select(CombatType combatType)
    {
        CombatType = combatType;
    }

    // Combat2.pas:30
    public void SetCenterLocation(byte X, byte Y, int L)
    {
        this.X = (sbyte)X;
        this.Y = (sbyte)Y;
        MapLevel = (sbyte)L;
    }

}
