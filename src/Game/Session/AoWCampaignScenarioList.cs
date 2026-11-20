#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public sealed class AoWCampaignScenarioList(CustomNode owner) : OwningNode(owner), IChildList<CampaignScenario>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CampaignScenario, so the cast never fails.
    public CampaignScenario this[int index] => GetChild<CampaignScenario>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is CampaignScenario;

    int IChildSink<CampaignScenario>.AddChild(CampaignScenario child) => AddChild(child);

    // The caller holds the campaign kind, so the scenario is that kind. An out-of-range index returns null, as GetScenario does.
    public T GetScenario<T>(int index) where T : CampaignScenario => index >= 0 && index < Count ? GetChild<T>(index) : null;

    // Diplomacy.pas:2214
    public CampaignScenario GetScenario(int index) => index >= 0 && index < Count ? this[index] : null;

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
        int index = IndexOfId(id);
        return index != -1 ? this[index] : null;
    }
}
