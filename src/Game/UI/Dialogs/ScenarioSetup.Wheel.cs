namespace Aow.Game.UI.Dialogs;

public sealed partial class ScenarioSetup
{
    public override void Loaded()
    {
        base.Loaded();
        PlayerList?.WheelScrollBar = PlayerListScrollBar;

        PingList?.WheelScrollBar = PlayerListScrollBar;
    }
}
