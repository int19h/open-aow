#nullable disable
// Translated from AoWVideoDlg.pas.
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// faction, state, page
public class WorldMapCampaign(CustomNode owner) : Campaign(owner)
{
    // AoWVideoDlg.pas:334
    public new const int ClassId = 2_248_704;

    public int Faction { get; private set; }

    public int State { get; set; }
    public int Page { get; set; }

    // AoWVideoDlg.pas:337
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Faction = persister.Int32("Faction", value: Faction);
    }

    // AoWVideoDlg.pas:348
    public override void Clear() => base.Clear();

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
                result = $"{result}_{AbilityRules.TranslateResourceString(GetEngine(), S: s_wonText)}";
            }
            else
            {
                // { Original uses Won for both factions. }
                result = $"{result}-{AbilityRules.TranslateResourceString(GetEngine(), S: s_wonText)}";
            }

            // The campaign autosave takes the extension the host writes (own-formats.md 4.2).
            result += FileBoundary.WriteExtension(DocumentKind.CampaignSave);
        }
        else
        {
            return base.GetAutoSaveFileName(auto);
        }

        return result;
    }

    private static readonly string s_wonText = "Won";

    internal static WorldMapCampaignScenario GetCampaignScenario(WorldMapCampaign campaign, int index) => campaign.Scenarios.GetScenario<WorldMapCampaignScenario>(index);

}
