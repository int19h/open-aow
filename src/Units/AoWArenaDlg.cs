// Translated from AoWArenaDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AoWArenaDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// arena event. completion callback
public partial class TArenaDlg : TDataModule
{
    public TArenaDlg(TComponent owner) : base(owner)
    { }

    public TAOWButton SelectBtn;
    public TAOWImage TitleIcon;
    public TAOWButton Close;
    public TAOWLabel Title;
    public TAOWButton CancelBtn;
    public TAOWWindow Dlg;
    public TAOWPanel UnitPnl;
    public TAOWPanel MapViewPnl;
    public TAOWCheckBox U1;
    public TAOWCheckBox U2;
    public TAOWCheckBox U3;
    public TAOWCheckBox U4;
    public TAOWCheckBox U5;
    public TAOWCheckBox U6;
    public TAOWCheckBox U7;
    public TAOWCheckBox U8;
    public TAOWImage DecoL;
    public TAOWImage DecoR;
    public TAoWFrame AoWFrame1;
    public TAOWPanel SelectionInfoPnl;
    public TAOWLabel SelectionInfoLbl;
    public TAOWLabel SelectionLbl;
    public TaowUserDrawBox MapView;
    public TAOWImage InfoSep;
    public TIvTranslator Translator;
    public TEnterArenaEventLog FEvent;
    public TExecuteDoneEvent FOnClose;

    // AoWArenaDlg.pas:104
    public void DrawUnit(int Index, int X, int Y, ref TSurfaceDesc SD)
    {
        TRect SaveRect;
        TP2UnitShowInfo Info;
        Aow.Game.Draw.Images.TLibraryImage Img;
        TAbstractUnit U;
        TAlignment Alignment;
        if (Index < FEvent.GetCount())
        {
            U = FEvent.GetUnit(Index);
            SaveRect = SD.ClipRect;
            var rect = Rect(X + 5, Y + 4, X + 46, Y + 56);
            SD.ClipRect = rect;
            Alignment = U.GetAlignment();
            if ((byte)Alignment < Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                Img = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                Img.ShowClipped(X + 25 - Img.Width / 2, Y + 30 - Img.Height / 2, SD);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
            U.ShowEx(X - 8, Y - 5, SD, Info);
            if ((((int)(((uint)FEvent.FTrainable) >> Index)) & 1) == 0)
            {
                Img = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[7];
                Img.ShowClipped(X + 25 - Img.Width / 2, Y + 30 - Img.Height / 2, SD);
            }

            SD.ClipRect = SaveRect;
            if ((((int)(((uint)FEvent.FTrainable) >> Index)) & 1) != 0)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8.Color = 0;
                var unitTrainCost = FEvent.FArena.UnitTrainCost(U);
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8.SetTextCentered(X + 25, Y + 62, IntToStr(unitTrainCost), SD);
            }
        }
    }

    // AoWArenaDlg.pas:140
    public void ClearEvent()
    {
        TEnterArenaEventLog Event;
        TExecuteDoneEvent Notify;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        Notify = FOnClose;
        Event = FEvent;
        FOnClose = null;
        FEvent = null;
        if (Notify != null)
        {
            Notify(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWArenaDlg.pas:152
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWArenaDlg.pas:158
    public void SelectBtnClick()
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FEvent.Train(ref Reason))
        {
            Dlg.Visible = false;
        }
        else
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
        }
    }

    // AoWArenaDlg.pas:170
    public void U1Change(TAOWCheckBox Sender, int Slot)
    {
        string Reason = "";
        if (Sender.Checked)
        {
            FEvent.SetSelection((byte)(FEvent.FSelection | (1 << Slot)));
        }
        else
        {
            FEvent.SetSelection((byte)(FEvent.FSelection & ~(1 << Slot)));
        }

        U1.Checked = (FEvent.FSelection & 1) != 0;
        U2.Checked = (FEvent.FSelection & 2) != 0;
        U3.Checked = (FEvent.FSelection & 4) != 0;
        U4.Checked = (FEvent.FSelection & 8) != 0;
        U5.Checked = (FEvent.FSelection & 16) != 0;
        U6.Checked = (FEvent.FSelection & 32) != 0;
        U7.Checked = (FEvent.FSelection & 64) != 0;
        U8.Checked = (FEvent.FSelection & 128) != 0;
        if ((FEvent.FTrainable & (1 << Slot)) != 0)
        {
            if (Sender.Checked)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
            }
        }

        if (FEvent.FSelection != 0)
        {
            if (FEvent.CanTrain(ref Reason))
            {
                SelectionInfoLbl.TextColor = 0;
            }
            else
            {
                SelectionInfoLbl.TextColor = 149;
            }

            var selectionInfo = FEvent.GetSelectionInfo();
            SelectionInfoLbl.Text = selectionInfo;
        }
        else
        {
            SelectionInfoLbl.Text = "";
        }
    }

    // AoWArenaDlg.pas:196
    public void U1PreDraw(TAOWCheckBox Sender, TSurfaceDesc SD, int Slot)
    {
        DrawUnit(Slot, UnitPnl.WinLeft + Sender.WinLeft, UnitPnl.WinTop + Sender.WinTop, ref SD);
    }

    // AoWArenaDlg.pas:201
    public void DlgHide()
    {
        ClearEvent();
    }

    // AoWArenaDlg.pas:206
    public void MapViewDraw(TAoWComponent Sender, TSurfaceDesc SD)
    {
        int X = default;
        int Y = default;
        TSurfaceDesc MapSD = default;
        TRect R;
        int W;
        int H;
        R = Rect(Dlg.WinLeft + Sender.WinLeft, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth, Dlg.WinTop + Sender.WinTop + Sender.WinHeight);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        var yhx = FEvent.FArena.GetYhx();
        Aow.Game.World.Map.HexGeometry.HXtoHP(FEvent.FArena.GetXhx(), yhx, ref X, ref Y);
        X = X + 16 - (int)(((uint)W) >> 1);
        Y = Y + 16 - (int)(((uint)H) >> 1);
        if (X < 16)
        {
            X = 16;
        }

        if (Y < 16)
        {
            Y = 16;
        }

        Aow.Game.Draw.TSurfaceDesc.CreateSDfromSD(SD, R, ref MapSD);
        var lhx = FEvent.FArena.GetLhx();
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.SetViewPort(X, Y, lhx, R.Right - R.Left, R.Bottom - R.Top);
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UpdateStaticScene();
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UpdateDynamicScene();
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.DrawStaticScene(ref MapSD);
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.DrawDynamicScene(ref MapSD);
    }

    // AoWArenaDlg.pas:224
    public void MapViewUpdate()
    {
        MapView.FDrawn = MapView.FDrawn && (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame & 1) != 0;
    }

    // AoWArenaDlg.pas:229
    public void DlgDrawBack()
    {
    }

    // AoWArenaDlg.pas:232
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowArena(TArenaDlg Dialog, TEnterArenaEventLog Event, TExecuteDoneEvent Done)
    {
        Assert(Dialog.FEvent == null, "", "AoWArenaDlg.pas", 94);
        Aow.Game.Ui.Dialogs.TArenaDlg.ResetArena(Dialog);
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 49;
        Dialog.Dlg.Visible = true;
    }

    internal static void ResetArena(TArenaDlg Dialog)
    {
        Dialog.U1.Checked = false;
        Dialog.U2.Checked = false;
        Dialog.U3.Checked = false;
        Dialog.U4.Checked = false;
        Dialog.U5.Checked = false;
        Dialog.U6.Checked = false;
        Dialog.U7.Checked = false;
        Dialog.U8.Checked = false;
        Dialog.SelectionInfoLbl.Text = "";
    }

}
