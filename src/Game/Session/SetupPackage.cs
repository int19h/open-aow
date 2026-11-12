// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Session;

public partial class SetupPackage : EngineObject
{
    // AoWSetup.pas:254
    public new const int ClassId = 131681;

    public int MessageId;
    public int Param2;
    public int Param1;
    public EngineObject Data;

    // AoWSetup.pas:245
    public override void Dispose()
    {
        EngineObject data;
        data = Data;
        if (data != null)
        {
            data?.Dispose();
        }

        base.Dispose();
    }

    // AoWSetup.pas:257
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref MessageId, 20);
        stream.Int32(ref Param2, 21);
        stream.Int32(ref Param1, 22);
        stream.Created(ref Data, null, 23);
    }

    // AoWSetup.pas:242
    public SetupPackage(ECustomNode owner) : base(owner)
    {
    }

}
