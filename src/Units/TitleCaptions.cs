using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine.Buffers;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

internal static class TitleCaptions
{
    internal static readonly string TitleNewGameRStr = "New Game";

    internal static readonly string TitleCampaignRStr = "Campaign";

    internal static readonly string TitleTutorialRStr = "Tutorial";

    internal static readonly string TitleLoadGameRStr = "Load Game";

    internal static readonly string TitleExitRStr = "Exit";

    internal static FixedArray_string_5 TitleButtons = FixedArray_string_5.From([]);

    internal static void Initialize()
    {
        TitleButtons[0] = TitleNewGameRStr;
        TitleButtons[1] = TitleCampaignRStr;
        TitleButtons[2] = TitleTutorialRStr;
        TitleButtons[3] = TitleLoadGameRStr;
        TitleButtons[4] = TitleExitRStr;
    }

}
