// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Session;

// ID. name. map file name
public partial class AoWCampaignScenario : EObject
{
    // Diplomacy.pas:2200
    public new const int ClassID = 131588;

    // Diplomacy.pas:2194
    public AoWCampaignScenario(ECustomNode owner) : base(owner)
    {

    }

    public int FID;
    public string FName = "";
    public string FFileName = "";

    // Diplomacy.pas:2203
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FID, 9);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FName, 10);
            Stream.rwString(ref FFileName, 11);
        }
    }

    // intf_P5_08.inc:71

}
