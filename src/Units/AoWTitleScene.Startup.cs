#nullable disable
using Aow.Game.Audio;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Dialogs;
using Aow.Rtl;
using static Aow.Game.Ui.Dialogs.AoWExeFormsUnit;
using static Aow.Game.Ui.Dialogs.AoWVideoDlgUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class TTitleScene
{
    public void ShowScene(bool playMusic)
    {
        var player = SoundUnit.SongPlayer;
        if (playMusic && !(player.CurrentIndex >= 0 && player.CurrentIndex < player.Songs.Count && player.Songs[player.CurrentIndex] == "SONGS\\TITLE.IT"))
        {
            player.Songs.Clear();
            player.Songs.Add("SONGS\\TITLE.IT");
            player.Play(0);
        }
        General.Manager.OverlayPriority = 0;
        TitleSceneIL.LoadDiscardableImageData();
        VideoDialog.SetSceneSize(640, 480);
        SetVideoMode(VideoDialog, TScreenMode.scmTitle);
        VideoDialog.ApplyMode();
        GameHost.MainForm.Display.SetFrameRate(30);
        GameHost.MainForm.DXDC.UseVerticalBlank = true;
        FRedrawBackground = true;
        FPressedButton = FHoverButton = -1;
        FFont = FCreditsFont = FontModule.AoW15WhiteGrey;
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
                    General.PlayButtonSound(0);
                    MultiWindow.Execute("");
                    break;
                case 1:
                    if (!AoWEUnit.AoWEngine.FDemo)
                    {
                        Window.Visible = false;
                        General.PlayButtonSound(0);
                        AoWSelFactionWinUnit.ShowFactionSelection(AoWSelFactionWinUnit.SelFactionWin);
                    }
                    else AoWMessageDlgUnit.ShowDemoMessage();
                    break;
                case 2:
                    E2GameFlowUnit.LoadGame(General, FileBoundary.ReadName(AoWEUnit.AoWEngine.StartupDirectory + "Scenario\\Custom\\TUTORIAL.HSM"));
                    if (AoWEUnit.AoWHSMap is { } map)
                    {
                        map.FFixedLeaders = true;
                        E2GameFlowUnit.StartGame(General);
                        Window.Visible = false;
                    }
                    break;
                case 3:
                    General.PlayButtonSound(0);
                    AoWFileDialogUnit.FileDlgWindow.FMode = 0;
                    AoWFileDialogUnit.FileDlgWindow.FileDialog.Visible = true;
                    break;
                case 4:
                    Window.Visible = false;
                    General.PlayButtonSound(0);
                    if (AoWEUnit.AoWEngine.FDemo) E2GlobalsUnit.TitleWindow.ShowExitSplash();
                    else GameHost.MainForm.CloseRequested();
                    break;
            }
        }
        FPressedButton = -1;
    }
}
