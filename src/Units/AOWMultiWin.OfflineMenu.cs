#nullable disable
using Aow.Game.Audio;
using Aow.Game.Ui.Dialogs;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class TMultiWindow
{
    public void Execute(string mapName)
    {
        var songs = Aow.Game.Audio.AudioHost.SongPlayer;
        if (songs.CurrentIndex == -1 || songs.CurrentIndex >= songs.Songs.Count || songs.Songs[songs.CurrentIndex] != "SONGS\\TITLE.IT")
        {
            songs.Songs.Clear();
            songs.Songs.Add("SONGS\\TITLE.IT");
            songs.Play(0);
        }
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.SetSceneSize(640, 480);
        FPendingMap = mapName;
        var dplay = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay;
        if (dplay.Connected) dplay.DisConnect();
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitlePnl.Visible = false;
        MultiPnl.Visible = GamePanel.Visible = true;
        TCPPnl.Visible = LanPnl.Visible = JoinPnl.Visible = TCPBtn.Visible = IPXBtn.Visible = PBEMPnl.Visible = false;
        // A provider button shows only when DirectPlay lists that provider. The offline component
        // lists the TCP/IP provider alone, as DirectPlay does on any current Windows, so TCP
        // shows and IPX stays hidden.
        if (dplay.GetConnections() == 0)
            for (int i = 0; i < dplay.ConnectionItems.Count; i++)
                switch (((TGDConnection)dplay.ConnectionItems[i]!).FType)
                {
                    case TConnectionType.conTCP: TCPBtn.Visible = true; break;
                    case TConnectionType.conIPX: IPXBtn.Visible = true; break;
                }
    }

    public void MultiWinUpdate()
    {
        if (MultiPnl.Visible && FMenuHeight < GamePanel.WinHeight)
        {
            FMenuHeight = Math.Min(FMenuHeight + 10, GamePanel.WinHeight);
            GamePanel.WinTop = FMenuHeight - GamePanel.WinHeight;
            BottomPnl.WinTop = 480 - FMenuHeight + GamePanel.WinHeight - BottomPnl.WinHeight;
            GamePanel.FSurfaceValid = BottomPnl.FSurfaceValid = false;
        }
    }
}
