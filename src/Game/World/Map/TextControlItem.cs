// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Id, 9);
        stream.rwStrings(Text, 10);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref Name, 11);
        }
    }

    // intf_P1_04.inc:67

}
