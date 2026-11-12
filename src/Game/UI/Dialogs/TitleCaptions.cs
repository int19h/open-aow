using Aow.Engine.Buffers;

namespace Aow.Game.UI.Dialogs;

internal static class TitleCaptions
{
    private static readonly string s_titleNewGameText = "New Game";

    private static readonly string s_titleCampaignText = "Campaign";

    private static readonly string s_titleTutorialText = "Tutorial";

    private static readonly string s_titleLoadGameText = "Load Game";

    private static readonly string s_titleExitText = "Exit";

    internal static StringArray5 TitleButtons = StringArray5.From([]);

    internal static void Initialize()
    {
        TitleButtons[0] = s_titleNewGameText;
        TitleButtons[1] = s_titleCampaignText;
        TitleButtons[2] = s_titleTutorialText;
        TitleButtons[3] = s_titleLoadGameText;
        TitleButtons[4] = s_titleExitText;
    }

}
