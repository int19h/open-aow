// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Session;

// ID. name. map file name
public partial class CampaignScenario : EngineObject
{
    // Diplomacy.pas:2200
    public new const int ClassId = 131588;

    // Diplomacy.pas:2194
    public CampaignScenario(ECustomNode owner) : base(owner)
    {

    }

    public int Id;
    public string Name = "";
    public string FileName = "";

    // Diplomacy.pas:2203
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Id, 9);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref Name, 10);
            stream.rwString(ref FileName, 11);
        }
    }

    // intf_P5_08.inc:71

}
