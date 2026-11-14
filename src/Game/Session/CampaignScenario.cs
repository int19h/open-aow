// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

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

    public int Id { get; set; }

    private string _name = "";

    public string FileName { get; private set; } = "";

    // Diplomacy.pas:2203
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Id = persister.Int32("Id", value: Id);
        // The original ReadWrite leaves out the name and the file name under text only (the
        // translate stream).
        if (!PersistSupport.Translating(persister))
        {
            _name = persister.String("Name", value: _name);
            FileName = persister.String("FileName", value: FileName);
        }
    }

    // intf_P5_08.inc:71

}
