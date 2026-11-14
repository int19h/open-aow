// Translated from AoWVideoDlg.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// image library, image index, heading, text
public partial class CampaignScenarioPage : EngineObject
{
    // AoWVideoDlg.pas:264
    public new const int ClassId = 2248706;

    public string ImageLibrary { get; private set; } = "";
    public int ImageIndex { get; private set; }
    public string Heading { get; private set; } = "";

    public StringList Text { get; }

    // AoWVideoDlg.pas:257
    public override void Dispose()
    {
        Text?.Dispose();
        base.Dispose();
    }

    // AoWVideoDlg.pas:268
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Lines(persister, name: "Text", lines: Text);
        Heading = persister.String("Heading", value: Heading);
        // The original ReadWrite leaves out the image library under text only (the translate stream).
        if (!PersistSupport.Translating(persister))
        {
            ImageLibrary = persister.String("ImageLibrary", value: ImageLibrary);
        }

        ImageIndex = persister.Int32("ImageIndex", value: ImageIndex);
    }

    // AoWVideoDlg.pas:250
    public CampaignScenarioPage(ECustomNode owner) : base(owner)
    {
        Text = new StringList();
    }

}
