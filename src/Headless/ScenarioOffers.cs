using System.Globalization;
using Aow.Game.Session;

namespace Aow.Headless;

/// <summary>One next node the world map draws from the current scenario's first three paths.</summary>
internal sealed record ScenarioOffer(int Id, int Index, string FileName, string PathDescription);

/// <summary>
/// The paths the world map offers, and the name the host accepts for one of them. The fourth path
/// is the link back, which the screen does not start. A name is the path description, the scenario
/// file name, or the scenario id.
/// </summary>
internal static class ScenarioOffers
{
    public const string BetweenMaps = "between-maps";

    public static IReadOnlyList<int> OfferedIds(int first, int second, int third)
    {
        List<int> ids = [];
        if (first > -1)
        {
            ids.Add(first);
        }

        if (second > -1)
        {
            ids.Add(second);
        }

        if (third > -1)
        {
            ids.Add(third);
        }

        return ids;
    }

    public static IReadOnlyList<ScenarioOffer> From(Campaign campaign)
    {
        WorldMapCampaignScenario? current = campaign.Scenarios.GetScenario<WorldMapCampaignScenario>(campaign.CurrentScenarioId);
        if (current is null)
        {
            return [];
        }

        List<ScenarioOffer> offers = [];
        foreach (int id in OfferedIds(current.Paths.Get(0), second: current.Paths.Get(1), third: current.Paths.Get(2)))
        {
            int index = campaign.Scenarios.IndexOfId(id);
            WorldMapCampaignScenario? next = index < 0 ? null : campaign.Scenarios.GetScenario<WorldMapCampaignScenario>(index);
            if (next is null)
            {
                continue;
            }

            offers.Add(new ScenarioOffer(id, Index: index, FileName: next.FileName, PathDescription: next.PathDescription));
        }

        return offers;
    }

    public static ScenarioOffer Select(IReadOnlyList<ScenarioOffer> offers, string name)
    {
        foreach (ScenarioOffer offer in offers)
        {
            if (Matches(offer, name))
            {
                return offer;
            }
        }

        string listed = offers.Count == 0
            ? "none"
            : string.Join(separator: ", ", values: offers.Select(static offer => Label(offer)));
        throw new ArgumentException($"The world map does not offer \"{name}\". Offered nodes: {listed}.");
    }

    public static string Label(ScenarioOffer offer)
    {
        return string.IsNullOrEmpty(offer.PathDescription)
            ? $"{offer.Id} {offer.FileName}"
            : $"{offer.PathDescription} ({offer.FileName})";
    }

    private static bool Matches(ScenarioOffer offer, string name)
    {
        return string.Equals(offer.PathDescription, name, comparisonType: StringComparison.OrdinalIgnoreCase)
            || string.Equals(offer.FileName, name, comparisonType: StringComparison.OrdinalIgnoreCase)
            || string.Equals(offer.Id.ToString(CultureInfo.InvariantCulture), name, comparisonType: StringComparison.Ordinal);
    }
}
