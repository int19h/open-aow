using System.Runtime.CompilerServices;
using Aow.Engine.Storage;

namespace Aow.Game.Units;

public partial class ArmyID
{
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 10);
        int location = (byte)FX | ((byte)FY << 8) | ((byte)FL << 16) | (F_ArmyID_13 << 24);
        stream.rwInteger(ref location, 11);
        FX = (sbyte)location;
        FY = (sbyte)(location >> 8);
        FL = (sbyte)(location >> 16);
        F_ArmyID_13 = (byte)(location >> 24);
        stream.rwStreamProc(12, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
    }
}
