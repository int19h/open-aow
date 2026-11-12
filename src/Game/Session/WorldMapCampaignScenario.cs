// Translated from AoWVideoDlg.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.UI.Dialogs;
using Aow.Rtl;

namespace Aow.Game.Session;

// image library, path text, description, flag, position, paths, pages, map kind
public partial class WorldMapCampaignScenario : CampaignScenario
{
    // AoWVideoDlg.pas:304
    public new const int ClassId = 2248705;

    public string ImageLibrary = "";
    public string PathDescription = "";
    public string Description = "";
    private bool _reserved20;
    // rwBool
    public int MapX;
    public int MapY;

    public Int32List Paths { get; }
    public WMCampaignScenarioPageList IntroPages { get; }
    public WMCampaignScenarioPageList OutroPages { get; }

    public byte MapKind;

    // AoWVideoDlg.pas:295
    public override void Dispose()
    {
        IntroPages?.Dispose();
        OutroPages?.Dispose();
        Paths?.Dispose();
        base.Dispose();
    }

    // AoWVideoDlg.pas:308
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.String(ref ImageLibrary, 12);
        }

        stream.String(ref PathDescription, 13);
        stream.String(ref Description, 14);
        stream.Boolean(ref _reserved20, 15);
        stream.Int32(ref MapX, 16);
        stream.Int32(ref MapY, 17);
        stream.Object(IntroPages, 19);
        stream.Object(OutroPages, 20);
        stream.Payload(21, Paths.LoadFromStream, Paths.SaveToStream);
        stream.Bytes(Bytes.Of(ref MapKind, 1), 1, 22);
    }

    // AoWVideoDlg.pas:278
    public WorldMapCampaignScenario(ECustomNode owner) : base(owner)
    {
        IntroPages = new WMCampaignScenarioPageList(null);
        OutroPages = new WMCampaignScenarioPageList(null);
        for (int I = 0; I <= 9; I++)
        {
            new CampaignScenarioPage(IntroPages);
            new CampaignScenarioPage(OutroPages);
        }

        Paths = new Int32List();
        Paths.Default = -1;
    }

}
