// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class AoWCampaignScenarioList : ENode, IChildList<CampaignScenario>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CampaignScenario, so the cast never fails.
    public CampaignScenario this[int index] => (CampaignScenario)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is CampaignScenario;

    int IChildSink<CampaignScenario>.AddChild(CampaignScenario child) => AddChild(child);

    public AoWCampaignScenarioList(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:2214
    public CampaignScenario GetScenario(int index)
    {
        CampaignScenario result;
        if (index >= 0 && index < _children.Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Diplomacy.pas:2221
    public int CreateUniqueId()
    {
        int result = 1;
        while (IndexOfId(result) != -1)
        {
            result += 1;
        }

        return result;
    }

    // Diplomacy.pas:2227
    public int IndexOfId(int id)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].Id == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Diplomacy.pas:2238
    public CampaignScenario FindId(int id)
    {
        CampaignScenario result;
        int index;
        index = IndexOfId(id);
        if (index != -1)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:631

}
