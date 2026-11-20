#nullable disable
// Translated from AoWVideoDlg.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.UI.Dialogs;

namespace Aow.Game.Session;

// image library, path text, description, position, paths, pages, map kind
public sealed class WorldMapCampaignScenario : CampaignScenario
{
    // AoWVideoDlg.pas:304
    public new const int ClassId = 2_248_705;

    public string ImageLibrary { get; private set; } = "";
    public string PathDescription { get; private set; } = "";
    public string Description { get; private set; } = "";

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
        // Delphi TWMCampaignScenario.F_WMCampaignScenario_20 (id 15) is always false (omitted) and never read.
        persister.Skip("UnknownFlag");
        MapX = persister.Int32("MapX", value: MapX);
        MapY = persister.Int32("MapY", value: MapY);
        persister.Object("IntroPages", objectValue: IntroPages);
        persister.Object("OutroPages", objectValue: OutroPages);
        PersistSupport.Int32List(persister, name: "Paths", list: Paths);
        MapKind = persister.RawByte("MapKind", value: MapKind);
    }

    // AoWVideoDlg.pas:278
    public WorldMapCampaignScenario(CustomNode owner) : base(owner)
    {
        IntroPages = new WMCampaignScenarioPageList(null) { Holder = CreationHolder() };
        OutroPages = new WMCampaignScenarioPageList(null) { Holder = CreationHolder() };
        for (int i = 0; i <= 9; i++)
        {
            _ = new CampaignScenarioPage(IntroPages);
            _ = new CampaignScenarioPage(OutroPages);
        }

        Paths = new Int32List
        {
            Default = -1
        };
    }
}
