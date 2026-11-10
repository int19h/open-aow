#nullable disable
using Aow.Game.Audio;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Dialogs;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class TTitleScene
{
    public void ShowScene(bool playMusic)
    {
        var player = Aow.Game.Audio.AudioHost.SongPlayer;
        if (playMusic && !(player.CurrentIndex >= 0 && player.CurrentIndex < player.Songs.Count && player.Songs[player.CurrentIndex] == "SONGS\\TITLE.IT"))
        {
            player.Songs.Clear();
            player.Songs.Add("SONGS\\TITLE.IT");
            player.Play(0);
        }
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        TitleSceneIL.LoadDiscardableImageData();
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.SetSceneSize(640, 480);
        Aow.Game.Ui.Dialogs.TVideoDialog.SetVideoMode(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, TScreenMode.scmTitle);
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ApplyMode();
        GameHost.MainForm.Display.SetFrameRate(30);
        GameHost.MainForm.DXDC.UseVerticalBlank = true;
        FRedrawBackground = true;
        FPressedButton = FHoverButton = -1;
        FFont = FCreditsFont = Aow.Game.Ui.Dialogs.DialogHosts.FontModule.AoW15WhiteGrey;
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

    public void WindowMouseUp(TMouseButton button, TSet<TShiftState> shift, int x, int y)
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
                    Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
                    Aow.Game.Ui.Dialogs.DialogHosts.MultiWindow.Execute("");
                    break;
                case 1:
                    if (!Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo)
                    {
                        Window.Visible = false;
                        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
                        Aow.Game.Ui.Dialogs.TTitleScene.ShowFactionSelection(Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin);
                    }
                    else Aow.Game.Ui.Dialogs.TMessageDlg.ShowDemoMessage();
                    break;
                case 2:
                    Aow.Game.Ui.Dialogs.TTitleScene.LoadGame(Aow.Game.Ui.Dialogs.DialogHosts.General, FileBoundary.ReadName(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Scenario\\Custom\\TUTORIAL.HSM"));
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap is { } map)
                    {
                        map.FFixedLeaders = true;
                        Aow.Game.Ui.Dialogs.TTitleScene.StartGame(Aow.Game.Ui.Dialogs.DialogHosts.General);
                        Window.Visible = false;
                    }
                    break;
                case 3:
                    Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
                    Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 0;
                    Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
                    break;
                case 4:
                    Window.Visible = false;
                    Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
                    if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo) Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.ShowExitSplash();
                    else GameHost.MainForm.CloseRequested();
                    break;
            }
        }
        FPressedButton = -1;
    }
}
