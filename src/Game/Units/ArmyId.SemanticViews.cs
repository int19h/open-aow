using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public partial class ArmyId
{
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 10);
        int location = StoredCoordinateImage.From(new MapCoordinate(X, Y, MapLevel), unknownBytes: UnknownBytes).ToInt32();
        stream.Int32(ref location, 11);
        (var stored, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
        (X, Y, MapLevel) = stored;
        stream.Payload(12, UnitIds.LoadFromStream, UnitIds.SaveToStream);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        int location = StoredCoordinateImage.From(new MapCoordinate(X, Y, MapLevel), unknownBytes: UnknownBytes).ToInt32();
        location = persister.Int32("Location", value: location);
        (var stored, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
        (X, Y, MapLevel) = stored;
        PersistSupport.Int32List(persister, name: "UnitIds", list: UnitIds);
    }
}
