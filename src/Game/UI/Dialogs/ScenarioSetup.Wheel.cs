namespace Aow.Game.UI.Dialogs;

public partial class ScenarioSetup
{
    public override void Loaded()
    {
        base.Loaded();
        if (PlayerList != null) PlayerList.WheelScrollBar = PlayerListScrollBar;
        if (PingList != null) PingList.WheelScrollBar = PlayerListScrollBar;
    }
}
