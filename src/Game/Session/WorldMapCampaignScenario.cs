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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref ImageLibrary, 12);
        }

        stream.rwString(ref PathDescription, 13);
        stream.rwString(ref Description, 14);
        stream.rwBool(ref _reserved20, 15);
        stream.rwInteger(ref MapX, 16);
        stream.rwInteger(ref MapY, 17);
        stream.rwEObject(IntroPages, 19);
        stream.rwEObject(OutroPages, 20);
        stream.rwStreamProc(21, Paths.LoadFromStream, Paths.SaveToStream);
        stream.rwData(Bytes.Of(ref MapKind, 1), 1, 22);
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
