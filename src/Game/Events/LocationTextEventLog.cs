// Translated from EventLog.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Events;

// location
public partial class LocationTextEventLog : TextEventLog
{
    // EventLog.pas:725
    public LocationTextEventLog(ECustomNode owner) : base(owner)
    {
        FXhx = -1;
        FYhx = -1;
        FLhx = -1;
    }

    // EventLog.pas:728
    public new const int ClassID = 131816;

    public sbyte FXhx;
    public sbyte FYhx;
    public sbyte FLhx;

    // EventLog.pas:731
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 21);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 22);
    }

    // EventLog.pas:740
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = FXhx;
        Y = FYhx;
        L = FLhx;
    }

    // EventLog.pas:743
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        base.Execute(Mode, Trigger, Done);
        if (Trigger == ExecuteTrigger.User)
        {
            ViewLocation();
        }

        return false;
    }

    // EventLog.pas:737
    public void SetLocation(sbyte X, sbyte Y, sbyte L)
    {
        FXhx = X;
        FYhx = Y;
        FLhx = L;
    }

}
