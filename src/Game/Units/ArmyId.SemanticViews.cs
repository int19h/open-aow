using System.Runtime.CompilerServices;
using Aow.Engine.Storage;

namespace Aow.Game.Units;

public partial class ArmyId
{
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 10);
        int location = (byte)X | ((byte)Y << 8) | ((byte)MapLevel << 16) | (_locationFlag << 24);
        stream.Int32(ref location, 11);
        X = (sbyte)location;
        Y = (sbyte)(location >> 8);
        MapLevel = (sbyte)(location >> 16);
        _locationFlag = (byte)(location >> 24);
        stream.Payload(12, UnitIds.LoadFromStream, UnitIds.SaveToStream);
    }
}
