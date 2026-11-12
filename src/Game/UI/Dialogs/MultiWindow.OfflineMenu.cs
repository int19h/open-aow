#nullable disable
using Aow.Game.Audio;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class MultiWindow
{
    public void Execute(string mapName)
    {
        var songs = AudioHost.SongPlayer;
        if (songs.CurrentIndex == -1 || songs.CurrentIndex >= songs.Songs.Count || songs.Songs[songs.CurrentIndex] != "SONGS\\TITLE.IT")
        {
            songs.Songs.Clear();
            songs.Songs.Add("SONGS\\TITLE.IT");
            songs.Play(0);
        }
        DialogHosts.VideoDialog.SetSceneSize(640, 480);
        _pendingMap = mapName;
        var directPlay = DialogHosts.SetupEvents.DPlay;
        if (directPlay.Connected) directPlay.DisConnect();
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        DialogHosts.TitleWindow.TitlePnl.Visible = false;
        MultiPnl.Visible = GamePanel.Visible = true;
        TCPPnl.Visible = LanPnl.Visible = JoinPnl.Visible = TCPBtn.Visible = IPXBtn.Visible = PBEMPnl.Visible = false;
        // A provider button shows only when DirectPlay lists that provider. The offline component
        // lists the TCP/IP provider alone, as DirectPlay does on any current Windows, so TCP
        // shows and IPX stays hidden.
        if (directPlay.GetConnections() == 0)
            for (int i = 0; i < directPlay.ConnectionItems.Count; i++)
                switch (((GDConnection)directPlay.ConnectionItems[i]!).FType)
                {
                    case ConnectionType.TCP: TCPBtn.Visible = true; break;
                    case ConnectionType.IPX: IPXBtn.Visible = true; break;
                }
    }

    public void MultiWinUpdate()
    {
        if (MultiPnl.Visible && _menuHeight < GamePanel.WinHeight)
        {
            _menuHeight = Math.Min(_menuHeight + 10, GamePanel.WinHeight);
            GamePanel.WinTop = _menuHeight - GamePanel.WinHeight;
            BottomPnl.WinTop = 480 - _menuHeight + GamePanel.WinHeight - BottomPnl.WinHeight;
            GamePanel.SurfaceValid = BottomPnl.SurfaceValid = false;
        }
    }
}
