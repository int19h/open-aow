// Translated from AoWVideoDlg.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// image library, image index, heading, text
public partial class CampaignScenarioPage : EngineObject
{
    // AoWVideoDlg.pas:264
    public new const int ClassID = 2248706;

    public string FImageLibrary = "";
    public int FImageIndex;
    public string FHeading = "";
    public StringList FText;

    // AoWVideoDlg.pas:257
    public override void Dispose()
    {
        FText?.Dispose();
        base.Dispose();
    }

    // AoWVideoDlg.pas:268
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FText, 10);
        Stream.rwString(ref FHeading, 11);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FImageLibrary, 12);
        }

        Stream.rwInteger(ref FImageIndex, 13);
    }

    // AoWVideoDlg.pas:250
    public CampaignScenarioPage(ECustomNode owner) : base(owner)
    {
        FText = new StringList();
    }

}
