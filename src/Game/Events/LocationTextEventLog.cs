// Translated from EventLog.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref HexX), 20);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref HexY), 21);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref HexLevel), 22);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        HexX = PersistSupport.UnsignedByte(persister, name: "HexX", value: HexX);
        HexY = PersistSupport.UnsignedByte(persister, name: "HexY", value: HexY);
        HexLevel = PersistSupport.UnsignedByte(persister, name: "HexLevel", value: HexLevel);
    }

    // EventLog.pas:740
    public override void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
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
