#nullable disable warnings
// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Session;

public sealed class SetupPackage(CustomNode? owner) : EngineObject(owner)
{
    // AoWSetup.pas:254
    public new const int ClassId = 131_681;

    public int MessageId { get; set; }
    public int Param2 { get; set; }
    public int Param1 { get; set; }
    public EngineObject? Data { get; set; }

    // AoWSetup.pas:245
    public override void Dispose()
    {
        EngineObject? data = Data;
        data?.Dispose();

        base.Dispose();
    }

    // AoWSetup.pas:257
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        MessageId = persister.Int32("MessageId", value: MessageId);
        Param2 = persister.Int32("Param2", value: Param2);
        Param1 = persister.Int32("Param1", value: Param1);
        Data = persister.Created("Data", objectValue: Data);
    }
}
