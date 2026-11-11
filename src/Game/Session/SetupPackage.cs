// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Session;

public partial class SetupPackage : EngineObject
{
    // AoWSetup.pas:254
    public new const int ClassID = 131681;

    public int FMessageID;
    public int FParam2;
    public int FParam1;
    public EngineObject FData;

    // AoWSetup.pas:245
    public override void Dispose()
    {
        EngineObject Data;
        Data = FData;
        if (Data != null)
        {
            Data?.Dispose();
        }

        base.Dispose();
    }

    // AoWSetup.pas:257
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FMessageID, 20);
        Stream.rwInteger(ref FParam2, 21);
        Stream.rwInteger(ref FParam1, 22);
        Stream.rwCreateEObject(ref FData, null, 23);
    }

    // AoWSetup.pas:242
    public SetupPackage(ECustomNode owner) : base(owner)
    {
    }

}
