#nullable disable
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWTitleScene;

public partial class TTitleScene
{
    public void ShowScene(bool playMusic)
    {
        General.Manager.OverlayPriority = 0;
        TitleSceneIL.LoadDiscardableImageData();
        VideoDialog.SetSceneSize(640, 480);
        Aow.Units.AoWExeForms.AoWExeForms.SetVideoMode(VideoDialog, Aow.Units.AoWVideoDlg.TScreenMode.scmTitle);
        VideoDialog.ApplyMode();
        AoWUnit.AoWUnit.MainForm.Display.SetFrameRate(30);
        AoWUnit.AoWUnit.MainForm.DXDC.UseVerticalBlank = true;
        FRedrawBackground = true;
        FPressedButton = FHoverButton = -1;
        FFont = FCreditsFont = FontModule.AoW15WhiteGrey;
        Window.WinManager.ILIndexTBack = 4;
        Window.Visible = true;
    }
    public void WindowHide()
    {
        Window.WinManager.ILIndexTBack = 0;
        AoWUnit.AoWUnit.MainForm?.Display.SetFrameRate(30);
        if (AoWUnit.AoWUnit.MainForm is { } form) form.DXDC.UseVerticalBlank = false;
        Window.Visible = false;
        TitleSceneIL.ReleaseDiscardableImageData();
    }
    public void WindowMouseUp(Aow.Rtl.TObject sender, Aow.Rtl.TMouseButton button, Aow.Rtl.TSet<Aow.Rtl.TShiftState> shift, int x, int y)
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
                    if (!AoWE.AoWE.AoWEngine.FDemo)
                    {
                        Window.Visible = false;
                        General.PlayButtonSound(0);
                        AoWSelFactionWin.AoWSelFactionWin.ShowFactionSelection(AoWSelFactionWin.AoWSelFactionWin.SelFactionWin);
                    }
                    else AoWMessageDlg.AoWMessageDlg.ShowDemoMessage();
                    break;
                case 2:
                    E2GameFlow.E2GameFlow.LoadGame(General, AoWE.AoWE.AoWEngine.StartupDirectory + "Scenario\\Custom\\TUTORIAL.HSM");
                    if (AoWE.AoWE.AoWHSMap is { } map)
                    {
                        map.FFixedLeaders = true;
                        E2GameFlow.E2GameFlow.StartGame(General);
                        Window.Visible = false;
                    }
                    break;
                case 3:
                    General.PlayButtonSound(0);
                    AoWFileDialog.AoWFileDialog.FileDlgWindow.FMode = 0;
                    AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = true;
                    break;
                case 4:
                    Window.Visible = false;
                    General.PlayButtonSound(0);
                    if (AoWE.AoWE.AoWEngine.FDemo) E2Globals.E2Globals.TitleWindow.ShowExitSplash();
                    else AoWUnit.AoWUnit.MainForm.CloseRequested();
                    break;
            }
        }
        FPressedButton = -1;
    }
}
