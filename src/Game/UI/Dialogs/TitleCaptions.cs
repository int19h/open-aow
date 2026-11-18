namespace Aow.Game.UI.Dialogs;

internal static class TitleCaptions
{
    private static readonly string s_titleNewGameText = "New Game";

    private static readonly string s_titleCampaignText = "Campaign";

    private static readonly string s_titleTutorialText = "Tutorial";

    private static readonly string s_titleLoadGameText = "Load Game";

    private static readonly string s_titleExitText = "Exit";

    internal static string[] TitleButtons { get; } = [s_titleNewGameText, s_titleCampaignText, s_titleTutorialText, s_titleLoadGameText, s_titleExitText];
}
