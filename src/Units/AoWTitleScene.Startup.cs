#nullable disable
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.AoWFileDialog;
using Aow.Units.AoWMessageDlg;
using Aow.Units.AoWSelFactionWin;
using Aow.Units.AoWVideoDlg;
using Aow.Units.E2GameFlow;
using Aow.Units.E2Globals;
using Aow.Units.Sound;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWVideoDlg.AoWVideoDlgUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;

namespace Aow.Units.AoWTitleScene;

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
