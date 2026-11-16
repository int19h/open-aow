namespace Aow.Game.UI.Dialogs;

public partial class ScenarioSetup
{
    public override void Loaded()
    {
        base.Loaded();
        PlayerList?.WheelScrollBar = PlayerListScrollBar;

        PingList?.WheelScrollBar = PlayerListScrollBar;
    }
}
