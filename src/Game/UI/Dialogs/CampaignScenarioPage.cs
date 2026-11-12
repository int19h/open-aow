// Translated from AoWVideoDlg.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// image library, image index, heading, text
public partial class CampaignScenarioPage : EngineObject
{
    // AoWVideoDlg.pas:264
    public new const int ClassId = 2248706;

    public string ImageLibrary = "";
    public int ImageIndex;
    public string Heading = "";

    public StringList Text { get; }

    // AoWVideoDlg.pas:257
    public override void Dispose()
    {
        Text?.Dispose();
        base.Dispose();
    }

    // AoWVideoDlg.pas:268
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStrings(Text, 10);
        stream.rwString(ref Heading, 11);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref ImageLibrary, 12);
        }

        stream.rwInteger(ref ImageIndex, 13);
    }

    // AoWVideoDlg.pas:250
    public CampaignScenarioPage(ECustomNode owner) : base(owner)
    {
        Text = new StringList();
    }

}
