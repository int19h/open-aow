using System.Runtime.CompilerServices;
using Aow.Engine.Storage;

namespace Aow.Game.Units;

public partial class ArmyId
{
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 10);
        int location = (byte)X | ((byte)Y << 8) | ((byte)MapLevel << 16) | (LocationFlag << 24);
        stream.rwInteger(ref location, 11);
        X = (sbyte)location;
        Y = (sbyte)(location >> 8);
        MapLevel = (sbyte)(location >> 16);
        LocationFlag = (byte)(location >> 24);
        stream.rwStreamProc(12, UnitIds.LoadFromStream, UnitIds.SaveToStream);
    }
}
