// Translated from AoWVideoDlg.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Ui.Dialogs;

using Aow.Rtl;

namespace Aow.Game.Session;

// image library, path text, description, flag, position, paths, pages, map kind
public partial class WMCampaignScenario : AoWCampaignScenario
{
    // AoWVideoDlg.pas:304
    public new const int ClassID = 2248705;

    public string FImageLibrary = "";
    public string FPathDescription = "";
    public string FDescription = "";
    public bool F_WMCampaignScenario_20;
    // rwBool
    public int FMapX;
    public int FMapY;
    public IntegerList FPaths;
    public WMCampaignScenarioPageList FIntroPages;
    public WMCampaignScenarioPageList FOutroPages;
    public byte FMapKind;

    // AoWVideoDlg.pas:295
    public override void Dispose()
    {
        FIntroPages?.Dispose();
        FOutroPages?.Dispose();
        FPaths?.Dispose();
        base.Dispose();
    }

    // AoWVideoDlg.pas:308
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FImageLibrary, 12);
        }

        Stream.rwString(ref FPathDescription, 13);
        Stream.rwString(ref FDescription, 14);
        Stream.rwBool(ref F_WMCampaignScenario_20, 15);
        Stream.rwInteger(ref FMapX, 16);
        Stream.rwInteger(ref FMapY, 17);
        Stream.rwEObject(FIntroPages, 19);
        Stream.rwEObject(FOutroPages, 20);
        Stream.rwStreamProc(21, FPaths.LoadFromStream, FPaths.SaveToStream);
        Stream.rwData(Bytes.Of(ref FMapKind, 1), 1, 22);
    }

    // AoWVideoDlg.pas:278
    public WMCampaignScenario(ECustomNode owner) : base(owner)
    {
        FIntroPages = new WMCampaignScenarioPageList(null);
        FOutroPages = new WMCampaignScenarioPageList(null);
        for (int I = 0; I <= 9; I++)
        {
            new WMCampaignScenarioPage(FIntroPages);
            new WMCampaignScenarioPage(FOutroPages);
        }

        FPaths = new IntegerList();
        FPaths.Default = -1;
    }

}
