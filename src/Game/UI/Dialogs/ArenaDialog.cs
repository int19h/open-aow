// Translated from AoWArenaDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// arena event. completion callback
public partial class ArenaDialog : DataModule
{
    public ArenaDialog(Component owner) : base(owner)
    { }

    public Button SelectBtn;
    public Image TitleIcon;
    public Button Close;
    public Label Title;
    public Button CancelBtn;
    public Window Dlg;
    public Panel UnitPnl;
    public Panel MapViewPnl;
    public CheckBox U1;
    public CheckBox U2;
    public CheckBox U3;
    public CheckBox U4;
    public CheckBox U5;
    public CheckBox U6;
    public CheckBox U7;
    public CheckBox U8;
    public Image DecoL;
    public Image DecoR;
    public Frame Frame1;
    public Panel SelectionInfoPnl;
    public Label SelectionInfoLbl;
    public Label SelectionLbl;
    public UserDrawBox MapView;
    public Image InfoSep;
    public IvTranslator Translator;
    private EnterArenaEventLog _event;
    private ExecuteDoneEvent _onClose;

    // AoWArenaDlg.pas:104
    public void DrawUnit(int index, int X, int Y, ref LockedSurface lockedSurface)
    {
        Rect saveRectangle;
        UnitShowInfo info;
        LibraryImage image;
        Troop U;
        Units.Alignment alignment;
        if (index < _event.GetCount())
        {
            U = _event.GetUnit(index);
            saveRectangle = lockedSurface.ClipRectangle;
            var rectangle = new Rect(X + 5, Y + 4, X + 46, Y + 56);
            lockedSurface.ClipRectangle = rectangle;
            alignment = U.GetAlignment();
            if ((byte)alignment < DialogHosts.UIGraphics.UnitBGs.Count)
            {
                image = DialogHosts.UIGraphics.UnitBGs[(int)alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            U.Show(X - 8, Y - 5, lockedSurface, info);
            if ((((int)(((uint)_event.Trainable) >> index)) & 1) == 0)
            {
                image = DialogHosts.UIGraphics.UnitBGs[7];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, lockedSurface);
            }

            lockedSurface.ClipRectangle = saveRectangle;
            if ((((int)(((uint)_event.Trainable) >> index)) & 1) != 0)
            {
                DialogHosts.UIFonts.Age8.Color = 0;
                var unitTrainCost = _event.Arena.UnitTrainCost(U);
                DialogHosts.UIFonts.Age8.SetTextCentered(X + 25, Y + 62, IntToStr(unitTrainCost), lockedSurface);
            }
        }
    }

    // AoWArenaDlg.pas:140
    public void ClearEvent()
    {
        EnterArenaEventLog eventValue;
        ExecuteDoneEvent notify;
        DialogHosts.General.Manager.OverlayPriority = 0;
        notify = _onClose;
        eventValue = _event;
        _onClose = null;
        _event = null;
        if (notify != null)
        {
            notify(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWArenaDlg.pas:152
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWArenaDlg.pas:158
    public void SelectBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (_event.Train(ref reason))
        {
            Dlg.Visible = false;
        }
        else
        {
            AoWHSMap.Current.ShowWarning(reason);
        }
    }

    // AoWArenaDlg.pas:170
    public void U1Change(CheckBox sender, int slot)
    {
        string reason = "";
        if (sender.Checked)
        {
            _event.SetSelection((byte)(_event.Selection | (1 << slot)));
        }
        else
        {
            _event.SetSelection((byte)(_event.Selection & ~(1 << slot)));
        }

        U1.Checked = (_event.Selection & 1) != 0;
        U2.Checked = (_event.Selection & 2) != 0;
        U3.Checked = (_event.Selection & 4) != 0;
        U4.Checked = (_event.Selection & 8) != 0;
        U5.Checked = (_event.Selection & 16) != 0;
        U6.Checked = (_event.Selection & 32) != 0;
        U7.Checked = (_event.Selection & 64) != 0;
        U8.Checked = (_event.Selection & 128) != 0;
        if ((_event.Trainable & (1 << slot)) != 0)
        {
            if (sender.Checked)
            {
                DialogHosts.General.PlayButtonSound(1);
            }
            else
            {
                DialogHosts.General.PlayButtonSound(2);
            }
        }

        if (_event.Selection != 0)
        {
            if (_event.CanTrain(ref reason))
            {
                SelectionInfoLbl.TextColor = 0;
            }
            else
            {
                SelectionInfoLbl.TextColor = 149;
            }

            var selectionInfo = _event.GetSelectionInfo();
            SelectionInfoLbl.Text = selectionInfo;
        }
        else
        {
            SelectionInfoLbl.Text = "";
        }
    }

    // AoWArenaDlg.pas:196
    public void U1PreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        DrawUnit(slot, UnitPnl.WinLeft + sender.WinLeft, UnitPnl.WinTop + sender.WinTop, ref lockedSurface);
    }

    // AoWArenaDlg.pas:201
    public void DlgHide()
    {
        ClearEvent();
    }

    // AoWArenaDlg.pas:206
    public void MapViewDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        LockedSurface mapSurfaceDescription = default;
        Rect R;
        int W;
        int H;
        R = new Rect(Dlg.WinLeft + sender.WinLeft, Dlg.WinTop + sender.WinTop, Dlg.WinLeft + sender.WinLeft + sender.WinWidth, Dlg.WinTop + sender.WinTop + sender.WinHeight);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        var hexY = _event.Arena.GetHexY();
        HexGeometry.HexToPixel(_event.Arena.GetHexX(), hexY, ref X, ref Y);
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

        LockedSurface.CreateFrom(lockedSurface, R, ref mapSurfaceDescription);
        var hexLevel = _event.Arena.GetHexLevel();
        AoWHSMap.Current.SetViewPort(X, Y, hexLevel, R.Right - R.Left, R.Bottom - R.Top);
        AoWHSMap.Current.UpdateStaticScene();
        AoWHSMap.Current.UpdateDynamicScene();
        AoWHSMap.Current.DrawStaticScene(ref mapSurfaceDescription);
        AoWHSMap.Current.DrawDynamicScene(ref mapSurfaceDescription);
    }

    // AoWArenaDlg.pas:224
    public void MapViewUpdate()
    {
        MapView.Drawn = MapView.Drawn && (AoWHSMap.Current.Frame & 1) != 0;
    }

    // AoWArenaDlg.pas:229
    public void DlgDrawBack()
    {
    }

    // AoWArenaDlg.pas:232
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowArena(ArenaDialog dialog, EnterArenaEventLog eventValue, ExecuteDoneEvent done)
    {
        Assert(dialog._event == null, "", "AoWArenaDlg.pas", 94);
        ResetArena(dialog);
        dialog._onClose = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        DialogHosts.General.Manager.OverlayPriority = 49;
        dialog.Dlg.Visible = true;
    }

    internal static void ResetArena(ArenaDialog dialog)
    {
        dialog.U1.Checked = false;
        dialog.U2.Checked = false;
        dialog.U3.Checked = false;
        dialog.U4.Checked = false;
        dialog.U5.Checked = false;
        dialog.U6.Checked = false;
        dialog.U7.Checked = false;
        dialog.U8.Checked = false;
        dialog.SelectionInfoLbl.Text = "";
    }

}
