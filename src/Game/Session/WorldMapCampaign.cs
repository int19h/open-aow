// Translated from AoWVideoDlg.pas.
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// faction, state, page
public partial class WorldMapCampaign : Campaign
{
    // AoWVideoDlg.pas:334
    public new const int ClassId = 2248704;

    public int Faction { get; private set; }

    public int State { get; set; }
    public int Page { get; set; }

    // AoWVideoDlg.pas:337
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Faction = persister.Int32("Faction", value: Faction);
    }

    // AoWVideoDlg.pas:324
    public WorldMapCampaign(ECustomNode owner) : base(owner)
    {
    }

    // AoWVideoDlg.pas:348
    public override void Clear()
    {
        base.Clear();
    }

    // AoWVideoDlg.pas:343
    public override CampaignScenario CreateScenario() => new WorldMapCampaignScenario(null) { Holder = CreationHolder() };

    // AoWVideoDlg.pas:353
    public override string GetAutoSaveFileName(CampaignAutoSaveType auto)
    {
        string result;
        if (auto == CampaignAutoSaveType.ScenarioFinished)
        {
            result = StrategicMap.Current.MapName;
            if (Faction == 0)
            {
                result = result + "_" + AbilityRules.TranslateResourceString(GetEngine(), s_wonText);
            }
            else
            {
                // { Original uses Won for both factions. }
                result = result + "-" + AbilityRules.TranslateResourceString(GetEngine(), s_wonText);
            }

            // The campaign autosave takes the extension the host writes (own-formats.md 4.2).
            result = result + FileBoundary.WriteExtension(DocumentKind.CampaignSave);
        }
        else
        {
            result = base.GetAutoSaveFileName(auto);
        }

        return result;
    }

    private static readonly string s_wonText = "Won";

    internal static WorldMapCampaignScenario GetCampaignScenario(WorldMapCampaign campaign, int index) => (WorldMapCampaignScenario)campaign.Scenarios.GetScenario(index);

}
