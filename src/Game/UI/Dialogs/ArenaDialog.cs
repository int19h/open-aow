#nullable disable
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

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.UI.Dialogs;

// arena event. completion callback
public partial class ArenaDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Panel UnitPanel { get; private set; }
    public CheckBox Unit1CheckBox { get; private set; }
    public CheckBox Unit2CheckBox { get; private set; }
    public CheckBox Unit3CheckBox { get; private set; }
    public CheckBox Unit4CheckBox { get; private set; }
    public CheckBox Unit5CheckBox { get; private set; }
    public CheckBox Unit6CheckBox { get; private set; }
    public CheckBox Unit7CheckBox { get; private set; }
    public CheckBox Unit8CheckBox { get; private set; }
    public Label SelectionInfoLabel { get; private set; }
    public UserDrawBox MapView { get; private set; }

    private EnterArenaEventLog _event;
    private event ExecuteDoneEvent OnClose;

    // AoWArenaDlg.pas:104
    public void DrawUnit(int index, int X, int Y, ref LockedSurface lockedSurface)
    {
        UnitShowInfo info;
        if (index < _event.GetCount())
        {
            Troop u = _event.GetUnit(index);
            Rect saveRectangle = lockedSurface.ClipRectangle;
            Rect rectangle = new(X + 5, Y + 4, X + 46, Y + 56);
            lockedSurface.ClipRectangle = rectangle;
            Units.Alignment alignment = u.GetAlignment();
            LibraryImage image;
            if ((byte)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X + 25 - (image.Width / 2), Y + 30 - (image.Height / 2), lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _ = u.Show(X - 8, Y - 5, lockedSurface, info);
            if ((((int)(((uint)_event.Trainable) >> index)) & 1) == 0)
            {
                image = UI.Graphics.UnitBackgrounds[7];
                image.ShowClipped(X + 25 - (image.Width / 2), Y + 30 - (image.Height / 2), lockedSurface);
            }

            lockedSurface.ClipRectangle = saveRectangle;
            if ((((int)(((uint)_event.Trainable) >> index)) & 1) != 0)
            {
                UI.Fonts.Age8.Color = 0;
                int unitTrainCost = _event.Arena.UnitTrainCost(u);
                UI.Fonts.Age8.SetTextCentered(X + 25, Y + 62, IntToStr(unitTrainCost), lockedSurface);
            }
        }
    }

    // AoWArenaDlg.pas:140
    public void ClearEvent()
    {
        UI.General.Manager.OverlayPriority = 0;
        ExecuteDoneEvent notify = OnClose;
        EnterArenaEventLog eventValue = _event;
        OnClose = null;
        _event = null;
        notify?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWArenaDlg.pas:152
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWArenaDlg.pas:158
    public void SelectBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_event.Train(out string reason))
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
                UI.General.PlayButtonSound(1);
            }
            else
            {
                UI.General.PlayButtonSound(2);
            }
        }

        if (_event.Selection != 0)
        {
            string reason = "";
            SelectionInfoLabel.TextColor = _event.CanTrain(ref reason) ? 0 : 149;

            string selectionInfo = _event.GetSelectionInfo();
            SelectionInfoLabel.Text = selectionInfo;
        }
        else
        {
            SelectionInfoLabel.Text = "";
        }
    }

    // AoWArenaDlg.pas:196
    public void U1PreDraw(CheckBox sender, LockedSurface lockedSurface, int slot) => DrawUnit(slot, UnitPanel.AbsoluteLeft + sender.WinLeft, UnitPanel.AbsoluteTop + sender.WinTop, ref lockedSurface);

    // AoWArenaDlg.pas:201
    public void DlgHide() => ClearEvent();

    // AoWArenaDlg.pas:206
    public void MapViewDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Rect r = new(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth, Window.WinTop + sender.WinTop + sender.WinHeight);
        int w = r.Right - r.Left;
        int h = r.Bottom - r.Top;
        sbyte hexY = _event.Arena.GetHexY();
        HexGeometry.HexToPixel(_event.Arena.GetHexX(), hexY, out int x, out int y);
        x = x + 16 - (int)(((uint)w) >> 1);
        y = y + 16 - (int)(((uint)h) >> 1);
        if (x < 16)
        {
            x = 16;
        }

        if (y < 16)
        {
            y = 16;
        }

        LockedSurface.CreateFrom(lockedSurface, r, out LockedSurface mapSurfaceDescription);
        sbyte hexLevel = _event.Arena.GetHexLevel();
        _ = StrategicMap.Current.SetViewPort(x, y, hexLevel, r.Right - r.Left, r.Bottom - r.Top);
        StrategicMap.Current.UpdateStaticScene();
        StrategicMap.Current.UpdateDynamicScene();
        StrategicMap.Current.DrawStaticScene(in mapSurfaceDescription);
        StrategicMap.Current.DrawDynamicScene(ref mapSurfaceDescription);
    }

    // AoWArenaDlg.pas:224
    public void MapViewUpdate() => MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;

    // AoWArenaDlg.pas:229
    public void DlgDrawBack()
    {
    }

    // AoWArenaDlg.pas:232
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowArena(ArenaDialog dialog, EnterArenaEventLog eventValue, ExecuteDoneEvent done)
    {
        Assert(dialog._event is null, "", "AoWArenaDlg.pas", 94);
        ResetArena(dialog);
        dialog.OnClose = done;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        dialog.UI.General.Manager.OverlayPriority = 49;
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
