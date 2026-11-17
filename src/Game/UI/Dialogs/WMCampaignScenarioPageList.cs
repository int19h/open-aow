// Translated from AoWVideoDlg.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.UI.Dialogs;

public partial class WMCampaignScenarioPageList(CustomNode owner) : OwningNode(owner), IChildList<CampaignScenarioPage>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CampaignScenarioPage, so the read never fails.
    public CampaignScenarioPage this[int index] => GetChild<CampaignScenarioPage>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is CampaignScenarioPage;

    int IChildSink<CampaignScenarioPage>.AddChild(CampaignScenarioPage child) => AddChild(child);
}
