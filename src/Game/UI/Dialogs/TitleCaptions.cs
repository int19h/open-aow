using Aow.Engine.Buffers;

namespace Aow.Game.UI.Dialogs;

internal static class TitleCaptions
{
    internal static readonly string TitleNewGameRStr = "New Game";

    internal static readonly string TitleCampaignRStr = "Campaign";

    internal static readonly string TitleTutorialRStr = "Tutorial";

    internal static readonly string TitleLoadGameRStr = "Load Game";

    internal static readonly string TitleExitRStr = "Exit";

    internal static StringArray5 TitleButtons = StringArray5.From([]);

    internal static void Initialize()
    {
        TitleButtons[0] = TitleNewGameRStr;
        TitleButtons[1] = TitleCampaignRStr;
        TitleButtons[2] = TitleTutorialRStr;
        TitleButtons[3] = TitleLoadGameRStr;
        TitleButtons[4] = TitleExitRStr;
    }

}
