#nullable disable
using Aow.Game.Audio;
using Aow.Game.Host;
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
        UI.General.Manager.OverlayPriority = 0;
        TitleSceneImages.LoadDiscardableImageData();
        UI.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(UI.VideoDialog, VideoDialog.ScreenMode.Title);
        UI.VideoDialog.ApplyMode();
        GameHost.MainForm.Display.SetFrameRate(30);
        GameHost.MainForm.DisplayController.UseVerticalBlank = true;
        _redrawBackground = true;
        _pressedButton = _hoverButton = -1;
        _font = _creditsFont = UI.Fonts.Age15WhiteOnGrey;
        Window.WindowManager.ImageLibraryIndexTBack = 4;
        Window.Visible = true;
    }

    public void WindowHide()
    {
        Window.WindowManager.ImageLibraryIndexTBack = 0;
        GameHost.MainForm?.Display.SetFrameRate(30);
        if (GameHost.MainForm is { } form) form.DisplayController.UseVerticalBlank = false;
        Window.Visible = false;
        TitleSceneImages.ReleaseDiscardableImageData();
    }

    public void WindowMouseUp(MouseButton button, Set<ShiftState> shift, int x, int y)
    {
        _mouseX = x;
        _mouseY = y;
        if (HitTest() == _pressedButton)
        {
            _selectedButton = _pressedButton;
            switch (_selectedButton)
            {
                case 0:
                    Window.Visible = false;
                    UI.General.PlayButtonSound(0);
                    UI.MultiWindow.Execute("");
                    break;
                case 1:
                    if (!UI.Engine.Demo)
                    {
                        Window.Visible = false;
                        UI.General.PlayButtonSound(0);
                        ShowFactionSelection(UI.FactionSelectionWindow);
                    }
                    else UI.MessageDialog.ShowDemoMessage();
                    break;
                case 2:
                    LoadGame(UI.General, FileBoundary.ReadName(UI.Engine.StartupDirectory + "Scenario\\Custom\\TUTORIAL.HSM"));
                    if (StrategicMap.Current is { } map)
                    {
                        map.FixedLeaders = true;
                        StartGame(UI.General);
                        Window.Visible = false;
                    }
                    break;
                case 3:
                    UI.General.PlayButtonSound(0);
                    UI.FileDialogWindow.Mode = 0;
                    UI.FileDialogWindow.Window.Visible = true;
                    break;
                case 4:
                    Window.Visible = false;
                    UI.General.PlayButtonSound(0);
                    if (UI.Engine.Demo) UI.TitleWindow.ShowExitSplash();
                    else GameHost.MainForm.CloseRequested();
                    break;
            }
        }
        _pressedButton = -1;
    }
}
