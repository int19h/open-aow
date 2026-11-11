// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// ID, Text, Name
public partial class TextControlItem : EngineObject
{
    public new const int ClassID = 131738;

    // Structur.PAS:4394
    public TextControlItem(ECustomNode owner) : base(owner)
    {
        FText = new StringList();
    }

    public int FID;
    public StringCollection FText;
    public string FName = "";

    // Structur.PAS:4400
    public override void Dispose()
    {
        FText?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:4411
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FID, 9);
        Stream.rwStrings(FText, 10);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FName, 11);
        }
    }

    // intf_P1_04.inc:67

}
