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
    private string _name = "";
    public string FileName = "";

    // Diplomacy.pas:2203
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref Id, 9);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.String(ref _name, 10);
            stream.String(ref FileName, 11);
        }
    }

    // intf_P5_08.inc:71

}
