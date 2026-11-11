#nullable disable
using Aow.Game.Audio;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class TitleScene
{
    public void ShowScene(bool playMusic)
    {
        var player = AudioHost.SongPlayer;
        if (playMusic && !(player.CurrentIndex >= 0 && player.CurrentIndex < player.Songs.Count && player.Songs[player.CurrentIndex] == "SONGS\\TITLE.IT"))
        {
            player.Songs.Clear();
            player.Songs.Add("SONGS\\TITLE.IT");
            player.Play(0);
        }
        DialogHosts.General.Manager.OverlayPriority = 0;
        TitleSceneIL.LoadDiscardableImageData();
        DialogHosts.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyMode();
        GameHost.MainForm.Display.SetFrameRate(30);
        GameHost.MainForm.DXDC.UseVerticalBlank = true;
        FRedrawBackground = true;
        FPressedButton = FHoverButton = -1;
        FFont = FCreditsFont = DialogHosts.FontModule.AoW15WhiteGrey;
        Window.WinManager.ILIndexTBack = 4;
        Window.Visible = true;
    }

    public void WindowHide()
    {
        Window.WinManager.ILIndexTBack = 0;
        GameHost.MainForm?.Display.SetFrameRate(30);
        if (GameHost.MainForm is { } form) form.DXDC.UseVerticalBlank = false;
        Window.Visible = false;
        TitleSceneIL.ReleaseDiscardableImageData();
    }

    public void WindowMouseUp(MouseButton button, Set<ShiftState> shift, int x, int y)
    {
        FMouseX = x;
        FMouseY = y;
        FMouseButton = button;
        if (HitTest() == FPressedButton)
        {
            FSelectedButton = FPressedButton;
            switch (FSelectedButton)
            {
                case 0:
                    Window.Visible = false;
                    DialogHosts.General.PlayButtonSound(0);
                    DialogHosts.MultiWindow.Execute("");
                    break;
                case 1:
                    if (!GameTables.AoWEngine.FDemo)
                    {
                        Window.Visible = false;
                        DialogHosts.General.PlayButtonSound(0);
                        ShowFactionSelection(DialogHosts.SelFactionWin);
                    }
                    else MessageDialog.ShowDemoMessage();
                    break;
                case 2:
                    LoadGame(DialogHosts.General, FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + "Scenario\\Custom\\TUTORIAL.HSM"));
                    if (AoWHSMap.Current is { } map)
                    {
                        map.FFixedLeaders = true;
                        StartGame(DialogHosts.General);
                        Window.Visible = false;
                    }
                    break;
                case 3:
                    DialogHosts.General.PlayButtonSound(0);
                    DialogHosts.FileDlgWindow.FMode = 0;
                    DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    break;
                case 4:
                    Window.Visible = false;
                    DialogHosts.General.PlayButtonSound(0);
                    if (GameTables.AoWEngine.FDemo) DialogHosts.TitleWindow.ShowExitSplash();
                    else GameHost.MainForm.CloseRequested();
                    break;
            }
        }
        FPressedButton = -1;
    }
}
