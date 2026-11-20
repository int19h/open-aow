#nullable disable
using Aow.Game.Audio;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class MultiWindow
{
    public void Execute(string mapName)
    {
        SongPlayer songs = AudioHost.SongPlayer;
        if (songs.CurrentIndex == -1 || songs.CurrentIndex >= songs.Songs.Count || songs.Songs[songs.CurrentIndex] != "SONGS\\TITLE.IT")
        {
            songs.Songs.Clear();
            _ = songs.Songs.Add("SONGS\\TITLE.IT");
            _ = songs.Play(0);
        }
        UI.VideoDialog.SetSceneSize(640, H: 480);
        _pendingMap = mapName;
        GDPlay directPlay = UI.SetupEvents.DirectPlay;
        if (directPlay.Connected)
        {
            directPlay.DisConnect();
        }

        UI.TitleWindow.MainWindow.Visible = true;
        UI.TitleWindow.TitlePanel.Visible = false;
        MultiPanel.Visible = GamePanel.Visible = true;
        TcpPanel.Visible = LanPanel.Visible = JoinPanel.Visible = TcpButton.Visible = IpxButton.Visible = EmailPanel.Visible = false;
        // A provider button shows only when DirectPlay lists that provider. The offline component
        // lists the TCP/IP provider alone, as DirectPlay does on any current Windows, so TCP
        // shows and IPX stays hidden.
        if (directPlay.GetConnections() == 0)
        {
            for (int i = 0; i < directPlay.ConnectionItems.Count; i++)
            {
                switch (directPlay.ConnectionItems[i]!.FType)
                {
                    case ConnectionType.TCP: TcpButton.Visible = true; break;
                    case ConnectionType.IPX: IpxButton.Visible = true; break;
                    case ConnectionType.None:
                    case ConnectionType.Unknown:
                    case ConnectionType.Modem:
                    case ConnectionType.Serial:
                    default:
                        break;
                }
            }
        }
    }

    public void MultiWinUpdate()
    {
        if (MultiPanel.Visible && _menuHeight < GamePanel.WinHeight)
        {
            _menuHeight = Math.Min(_menuHeight + 10, GamePanel.WinHeight);
            GamePanel.WinTop = _menuHeight - GamePanel.WinHeight;
            BottomPanel.WinTop = 480 - _menuHeight + GamePanel.WinHeight - BottomPanel.WinHeight;
            GamePanel.SurfaceValid = BottomPanel.SurfaceValid = false;
        }
    }
}
