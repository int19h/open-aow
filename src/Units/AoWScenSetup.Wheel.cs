namespace Aow.Game.Ui.Dialogs;

public partial class TScenSetup
{
    public override void Loaded()
    {
        base.Loaded();
        if (PlayerList != null) PlayerList.WheelScrollBar = PlayerListSB;
        if (PingList != null) PingList.WheelScrollBar = PlayerListSB;
    }
}
