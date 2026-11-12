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
        HexX = -1;
        HexY = -1;
        HexLevel = -1;
    }

    // EventLog.pas:728
    public new const int ClassId = 131816;

    public sbyte HexX;
    public sbyte HexY;
    public sbyte HexLevel;

    // EventLog.pas:731
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref HexX), 20);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref HexY), 21);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref HexLevel), 22);
    }

    // EventLog.pas:740
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = HexX;
        Y = HexY;
        L = HexLevel;
    }

    // EventLog.pas:743
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        base.Execute(mode, trigger, done);
        if (trigger == ExecuteTrigger.User)
        {
            ViewLocation();
        }

        return false;
    }

    // EventLog.pas:737
    public void SetLocation(sbyte X, sbyte Y, sbyte L)
    {
        HexX = X;
        HexY = Y;
        HexLevel = L;
    }

}
