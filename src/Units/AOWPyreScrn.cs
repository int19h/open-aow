// Translated from AOWPyreScrn.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// game-over event, animation frame
public partial class TPyreScreen : TDataModule
{
    public TPyreScreen(TComponent owner) : base(owner)
    { }

    public TAOWPanel DeadPnl;
    public TAOWImage PyreTop;
    public TAOWImage LeaderFaceFr;
    public TaowUserDrawBox PyreAnimation;
    public object FAnimation;
    public TSongStatus FSavedSongStatus;
    public TExecuteDoneEvent FDone;
    public TGameOverEventLog FGameOverEvent;
    public int FAnimationFrame;

    // AOWPyreScrn.pas:114
    public void ClearFire()
    {
        if (FAnimation != null)
        {
            (FAnimation as IDisposable)?.Dispose();
            FAnimation = null;
        }
    }

    // AOWPyreScrn.pas:119
    public void FinishScreen()
    {
        ClearFire();
        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.PyreFrames.ReleaseDiscardableImageData();
        Aow.Game.Ui.Dialogs.DialogHosts.PyreScreen.DeadPnl.Visible = false;
        Aow.Game.Ui.Dialogs.TPyreScreen.ShowResults(Aow.Game.Ui.Dialogs.DialogHosts.ResultsWin);
    }

    // AOWPyreScrn.pas:127
    public void ShowGameOver(TGameOverEventLog Event, TExecuteDoneEvent Done)
    {
        FAnimationFrame = 0;
        FGameOverEvent = Event;
        FGameOverEvent.AddRef();
        FDone = Done;
        FSavedSongStatus = Aow.Game.Audio.AudioPlayback.BackupSongStatus();
        Aow.Game.Audio.AudioPlayback.FadeMusicVolume(0, 1);
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 249;
        switch (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Event.FPlayer).FGameOverStatus)
        {
            case TGameOverStatus.goLost:
                FAnimation = Aow.Game.Audio.AudioPlayback.PlaySampleEx("Sfx\\Attack\\Fireloop.wav", 64, true, 0, TSamplePanning.spCenter, null);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(68, true, true);
                break;
            case TGameOverStatus.goWon:
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(67, true, true);
                break;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.PyreFrames.LoadDiscardableImageData();
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitlePnl.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.PyreScreen.DeadPnl.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.SetSceneSize(640, 480);
        Aow.Game.Ui.Dialogs.TVideoDialog.SetVideoMode(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, TScreenMode.scmTitle);
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ApplyMode();
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = true;
        SetCurrentDir(ExtractFileDir(Application.ExeName));
    }

    // AOWPyreScrn.pas:48
    public void DeadPnlUpdate()
    {
    }

    // AOWPyreScrn.pas:51
    public void DeadPnlMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FinishScreen();
    }

    // AOWPyreScrn.pas:54
    public void LeaderFaceFrPreDraw(TSurfaceDesc SD)
    {
        TRect R = default;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            R.Left = DeadPnl.WinLeft + LeaderFaceFr.WinLeft + 45;
            R.Right = R.Left + 103;
            R.Top = DeadPnl.WinTop + LeaderFaceFr.WinTop + 35;
            R.Bottom = R.Top + 128;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FLeader.ShowFace(R, SD);
        }
    }

    // AOWPyreScrn.pas:69
    public void PyreAnimationUpdate()
    {
        PyreAnimation.FDrawn = false;
        DeadPnl.FDrawn = false;
        FAnimationFrame += 1;
        if (FAnimationFrame > 450)
        {
            FinishScreen();
        }

        if (FAnimationFrame > 16777215)
        {
            FAnimationFrame = 0;
        }
    }

    // AOWPyreScrn.pas:77
    public void PyreAnimationDraw(TSurfaceDesc SD)
    {
        int X;
        int Y;
        int Frame;
        string Text;
        X = PyreAnimation.WinWidth / 2;
        Y = PyreAnimation.WinHeight + 20 - FAnimationFrame / 2;
        if (Y < 400)
        {
            Y = 400;
        }

        Y += DeadPnl.WinTop;
        X += DeadPnl.WinLeft;
        switch (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer).FGameOverStatus)
        {
            case TGameOverStatus.goLost:
                Frame = FAnimationFrame / 3 % 9;
                if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo)
                {
                    Frame = 0;
                }

                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.PyreFrames[Frame].Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop + PyreAnimation.WinTop, SD);
                Text = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer).GetNameEx() + ", " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TPyreScreen.YourReignHasEndedRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule.AoW15BlackBrown.SetTextCentered(X, Y, Text, SD);
                break;
            case TGameOverStatus.goWon:
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.VictoryFrames[0].Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop, SD);
                Text = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer).GetNameEx() + ", " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TPyreScreen.YouWhereVictoriousRStr);
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule.AoW15WhiteGrey.SetTextCentered(X, Y, Text, SD);
                break;
        }
    }

    // AOWPyreScrn.pas:110
    public void DeadPnlKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        switch (Key)
        {
            case 13:
            case 27:
                FinishScreen();
                break;
        }
    }

    private static readonly string ContinueRStr = "Continue";

    private static readonly string ReplayRStr = "Replay";

    private static readonly string YouWhereVictoriousRStr = "you were victorious!";

    private static readonly string YourReignHasEndedRStr = "your reign has ended!";

    private static void ShowResults(TResultsWin Window)
    {
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo)
        {
            Aow.Game.Audio.AudioPlayback.PlaySongEx("Songs\\Special\\end2.s3m", 2, 2, true);
        }
        else
        {
            Aow.Game.Audio.AudioPlayback.PlaySongEx("Songs\\Special\\Sunset.it", 2, 2, true);
        }

        Aow.Game.Audio.AudioPlayback.SetMusicVolume(0);
        Aow.Game.Audio.AudioPlayback.FadeMusicVolume(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.MusicVolume.Position, 1);
        Window.FPlayers.Clear();
        for (int I = 1, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count - 1; I <= last; I++)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(I).FPlayerType != TPlayerType.ptIndependent)
            {
                Window.FPlayers.Add((byte)I);
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            ((TAoWComponent)Window.FControls[I]).Visible = I < Window.FPlayers.Count;
            bool condition = I < Window.FPlayers.Count && Window.FPlayers[I] == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer;
            ((TAOWCheckBox)Window.FControls[I]).Checked = condition;
        }

        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FGameOverStatus == TGameOverStatus.goLost)
        {
            var replay = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ReplayRStr);
            Window.ContinueBtn.Text = replay;
            Window.ExitBtn.Visible = true;
        }
        else
        {
            var continueText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ContinueRStr);
            Window.ContinueBtn.Text = continueText;
            Window.ExitBtn.Visible = false;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.ResultsWin.ResultsPnl.Visible = true;
    }

    internal static void ClearPyre(TPyreScreen Dialog)
    {
        if (Dialog.FAnimation != null)
        {
            (Dialog.FAnimation as IDisposable)?.Dispose();
            Dialog.FAnimation = null;
        }
    }

}
