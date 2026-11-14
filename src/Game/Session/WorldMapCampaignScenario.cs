// Translated from AoWVideoDlg.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.UI.Dialogs;

namespace Aow.Game.Session;

// image library, path text, description, flag, position, paths, pages, map kind
public partial class WorldMapCampaignScenario : CampaignScenario
{
    // AoWVideoDlg.pas:304
    public new const int ClassId = 2248705;

    public string ImageLibrary { get; private set; } = "";
    public string PathDescription { get; private set; } = "";
    public string Description { get; private set; } = "";

    // A Boolean of unknown meaning that the original stores at id 15 and never reads. It round
    // trips unchanged.
    private bool _unknownFlag;

    public int MapX { get; private set; }
    public int MapY { get; private set; }

    public Int32List Paths { get; }
    public WMCampaignScenarioPageList IntroPages { get; }
    public WMCampaignScenarioPageList OutroPages { get; }

    public byte MapKind { get; private set; }

    // AoWVideoDlg.pas:295
    public override void Dispose()
    {
        IntroPages?.Dispose();
        OutroPages?.Dispose();
        Paths?.Dispose();
        base.Dispose();
    }

    // AoWVideoDlg.pas:308
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The original ReadWrite leaves out the image library under text only (the translate stream).
        if (!PersistSupport.Translating(persister))
        {
            ImageLibrary = persister.String("ImageLibrary", value: ImageLibrary);
        }

        PathDescription = persister.String("PathDescription", value: PathDescription);
        Description = persister.String("Description", value: Description);
        _unknownFlag = persister.Boolean("UnknownFlag", value: _unknownFlag);
        MapX = persister.Int32("MapX", value: MapX);
        MapY = persister.Int32("MapY", value: MapY);
        persister.Object("IntroPages", objectValue: IntroPages);
        persister.Object("OutroPages", objectValue: OutroPages);
        PersistSupport.Int32List(persister, name: "Paths", list: Paths);
        MapKind = persister.RawByte("MapKind", value: MapKind);
    }

    // AoWVideoDlg.pas:278
    public WorldMapCampaignScenario(ECustomNode owner) : base(owner)
    {
        IntroPages = new WMCampaignScenarioPageList(null) { Holder = CreationHolder() };
        OutroPages = new WMCampaignScenarioPageList(null) { Holder = CreationHolder() };
        for (int I = 0; I <= 9; I++)
        {
            new CampaignScenarioPage(IntroPages);
            new CampaignScenarioPage(OutroPages);
        }

        Paths = new Int32List();
        Paths.Default = -1;
    }

}
