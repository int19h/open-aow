// Translated from AoWVideoDlg.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Lines(Text, 10);
        stream.String(ref Heading, 11);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.String(ref ImageLibrary, 12);
        }

        stream.Int32(ref ImageIndex, 13);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Lines(persister, name: "Text", lines: Text);
        Heading = persister.String("Heading", value: Heading);
        ImageLibrary = persister.String("ImageLibrary", value: ImageLibrary);
        ImageIndex = persister.Int32("ImageIndex", value: ImageIndex);
    }

    // AoWVideoDlg.pas:250
    public CampaignScenarioPage(ECustomNode owner) : base(owner)
    {
        Text = new StringList();
    }

}
