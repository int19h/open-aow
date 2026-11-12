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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// arena event. completion callback
public partial class ArenaDialog : DataModule
{
    public ArenaDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Panel UnitPanel;
    public CheckBox Unit1CheckBox;
    public CheckBox Unit2CheckBox;
    public CheckBox Unit3CheckBox;
    public CheckBox Unit4CheckBox;
    public CheckBox Unit5CheckBox;
    public CheckBox Unit6CheckBox;
    public CheckBox Unit7CheckBox;
    public CheckBox Unit8CheckBox;
    public Label SelectionInfoLabel;
    public UserDrawBox MapView;
    private EnterArenaEventLog _event;
    private event ExecuteDoneEvent OnClose;

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
            if ((byte)alignment < DialogHosts.UIGraphics.UnitBackgrounds.Count)
            {
                image = DialogHosts.UIGraphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            U.Show(X - 8, Y - 5, lockedSurface, info);
            if ((((int)(((uint)_event.Trainable) >> index)) & 1) == 0)
            {
                image = DialogHosts.UIGraphics.UnitBackgrounds[7];
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
        notify = OnClose;
        eventValue = _event;
        OnClose = null;
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
        Window.Visible = false;
    }

    // AoWArenaDlg.pas:158
    public void SelectBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (_event.Train(ref reason))
        {
            Window.Visible = false;
        }
        else
        {
            StrategicMap.Current.ShowWarning(reason);
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

        Unit1CheckBox.Checked = (_event.Selection & 1) != 0;
        Unit2CheckBox.Checked = (_event.Selection & 2) != 0;
        Unit3CheckBox.Checked = (_event.Selection & 4) != 0;
        Unit4CheckBox.Checked = (_event.Selection & 8) != 0;
        Unit5CheckBox.Checked = (_event.Selection & 16) != 0;
        Unit6CheckBox.Checked = (_event.Selection & 32) != 0;
        Unit7CheckBox.Checked = (_event.Selection & 64) != 0;
        Unit8CheckBox.Checked = (_event.Selection & 128) != 0;
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
                SelectionInfoLabel.TextColor = 0;
            }
            else
            {
                SelectionInfoLabel.TextColor = 149;
            }

            var selectionInfo = _event.GetSelectionInfo();
            SelectionInfoLabel.Text = selectionInfo;
        }
        else
        {
            SelectionInfoLabel.Text = "";
        }
    }

    // AoWArenaDlg.pas:196
    public void U1PreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        DrawUnit(slot, UnitPanel.WinLeft + sender.WinLeft, UnitPanel.WinTop + sender.WinTop, ref lockedSurface);
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
        R = new Rect(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth, Window.WinTop + sender.WinTop + sender.WinHeight);
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
        StrategicMap.Current.SetViewPort(X, Y, hexLevel, R.Right - R.Left, R.Bottom - R.Top);
        StrategicMap.Current.UpdateStaticScene();
        StrategicMap.Current.UpdateDynamicScene();
        StrategicMap.Current.DrawStaticScene(ref mapSurfaceDescription);
        StrategicMap.Current.DrawDynamicScene(ref mapSurfaceDescription);
    }

    // AoWArenaDlg.pas:224
    public void MapViewUpdate()
    {
        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
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
            Window.Visible = false;
        }
    }

    internal static void ShowArena(ArenaDialog dialog, EnterArenaEventLog eventValue, ExecuteDoneEvent done)
    {
        Assert(dialog._event == null, "", "AoWArenaDlg.pas", 94);
        ResetArena(dialog);
        dialog.OnClose = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        DialogHosts.General.Manager.OverlayPriority = 49;
        dialog.Window.Visible = true;
    }

    internal static void ResetArena(ArenaDialog dialog)
    {
        dialog.Unit1CheckBox.Checked = false;
        dialog.Unit2CheckBox.Checked = false;
        dialog.Unit3CheckBox.Checked = false;
        dialog.Unit4CheckBox.Checked = false;
        dialog.Unit5CheckBox.Checked = false;
        dialog.Unit6CheckBox.Checked = false;
        dialog.Unit7CheckBox.Checked = false;
        dialog.Unit8CheckBox.Checked = false;
        dialog.SelectionInfoLabel.Text = "";
    }

}
