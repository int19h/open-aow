#nullable disable
using Aow.Rtl;

namespace Aow.Units.AOWMultiWin;

public partial class TMultiWindow
{
    public void Execute(string mapName)
    {
        var songs = Sound.Sound.SongPlayer;
        if (songs.CurrentIndex == -1 || songs.CurrentIndex >= songs.Songs.Count || songs.Songs[songs.CurrentIndex] != "SONGS\\TITLE.IT")
        {
            songs.Songs.Clear();
            songs.Songs.Add("SONGS\\TITLE.IT");
            songs.Play(0);
        }
        AoWVideoDlg.AoWVideoDlg.VideoDialog.SetSceneSize(640, 480);
        FPendingMap = mapName;
        E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
        E2Globals.E2Globals.TitleWindow.TitlePnl.Visible = false;
        MultiPnl.Visible = GamePanel.Visible = true;
        TCPPnl.Visible = LanPnl.Visible = JoinPnl.Visible = IPXBtn.Visible = PBEMPnl.Visible = false;
        TCPBtn.Visible = true;
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
