// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class AoWCampaignScenarioList : ENode
{
    public AoWCampaignScenarioList(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:2214
    public CampaignScenario GetScenario(int index)
    {
        CampaignScenario result;
        if (index >= 0 && index < _children.Count)
        {
            result = (CampaignScenario)_children.List[index];
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
            if (((CampaignScenario)_children.List[result]).Id == id)
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
            result = (CampaignScenario)_children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:631

}
