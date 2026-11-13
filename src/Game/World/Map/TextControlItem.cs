// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// ID, Text, Name
public partial class TextControlItem : EngineObject
{
    public new const int ClassId = 131738;

    // Structur.PAS:4394
    public TextControlItem(ECustomNode owner) : base(owner)
    {
        Text = new StringList();
    }

    public int Id;

    public StringCollection Text { get; }

    public string Name = "";

    // Structur.PAS:4400
    public override void Dispose()
    {
        Text?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:4411
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref Id, 9);
        stream.Lines(Text, 10);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.String(ref Name, 11);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Id = persister.Int32("Id", value: Id);
        PersistSupport.Lines(persister, name: "Text", lines: Text);
        Name = persister.String("Name", value: Name);
    }

    // intf_P1_04.inc:67

}
