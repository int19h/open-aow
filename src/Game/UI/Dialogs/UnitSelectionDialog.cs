#nullable disable
// Translated from AoWUnitSelDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// unit selector. retained event log
public partial class UnitSelectionDialog : UIModule
{
    public UnitSelectionDialog(UIContext ui) : base(ui)
    { }

    public Window Window { get; private set; }
    public Panel UnitPanel { get; set; }
    public Panel SourcePanel { get; private set; }
    public Label SourceNameLabel { get; private set; }
    public CheckBox Unit1CheckBox { get; set; }
    public CheckBox Unit2CheckBox { get; private set; }
    public CheckBox Unit3CheckBox { get; private set; }
    public CheckBox Unit4CheckBox { get; private set; }
    public CheckBox Unit5CheckBox { get; private set; }
    public CheckBox Unit6CheckBox { get; private set; }
    public CheckBox Unit7CheckBox { get; private set; }
    public CheckBox Unit8CheckBox { get; private set; }
    public Image SourceFace { get; private set; }
    public Label SelectionInfoLabel { get; private set; }
    public Label SelectionLabel { get; private set; }

    public UnitSelector Event { get; set; }

    private UnitSelectionActivation _eventLog;

    // AoWUnitSelDlg.pas:66
    public void DrawUnit(int index, int X, int Y, ref LockedSurface lockedSurface)
    {
        Rect saveRectangle;
        UnitShowInfo info;
        LibraryImage image;
        Units.Alignment alignment;
        if (index < Event.Units.GetCount())
        {
            saveRectangle = lockedSurface.ClipRectangle;
            var rectangle = new Rect(X + 5, Y + 4, X + 46, Y + 56);
            lockedSurface.ClipRectangle = rectangle;
            alignment = Event.Units.GetUnit(index).GetAlignment();
            if ((int)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            Event.Units.GetUnit(index).Show(X - 8, Y - 5, lockedSurface, info);
            if ((((int)(((uint)Event.Selectable) >> index)) & 1) == 0)
            {
                image = UI.Graphics.UnitBackgrounds[7];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, lockedSurface);
            }

            lockedSurface.ClipRectangle = saveRectangle;
        }
    }

    // AoWUnitSelDlg.pas:177
    public void ResetSelection()
    {
        if (Event != null)
        {
            Event.Release();
            Event = null;
        }

        if (_eventLog != null)
        {
            _eventLog.Release();
            _eventLog = null;
        }

        Unit1CheckBox.Checked = false;
        Unit2CheckBox.Checked = false;
        Unit3CheckBox.Checked = false;
        Unit4CheckBox.Checked = false;
        Unit5CheckBox.Checked = false;
        Unit6CheckBox.Checked = false;
        Unit7CheckBox.Checked = false;
        Unit8CheckBox.Checked = false;
        SelectionInfoLabel.Text = "";
    }

    // AoWUnitSelDlg.pas:186
    public void Execute(UnitSelectionActivation request)
    {
        Troop u;
        ResetSelection();
        _eventLog = request;
        _eventLog.AddRef();
        Event = request.UnitSelector;
        Event.AddRef();
        u = StrategicMap.Current.UnitControl.FindUnit(Event.SourceUnitId);
        if (u != null)
        {
            SourcePanel.Visible = true;
            var name = u.GetName();
            SourceNameLabel.Text = name;
        }
        else
        {
            SourcePanel.Visible = false;
        }

        SelectionLabel.Text = Event.Caption;
        Window.Visible = true;
    }

    // AoWUnitSelDlg.pas:95
    public void ClearEvent()
    {
        if (Event != null)
        {
            Event.Release();
            Event = null;
        }

        if (_eventLog != null)
        {
            _eventLog.Release();
            _eventLog = null;
        }
    }

    // AoWUnitSelDlg.pas:101
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWUnitSelDlg.pas:107
    public void SelectBtnClick()
    {
        string reason = "";
        UI.General.PlayButtonSound(0);
        if (Event.CanSelect(ref reason))
        {
            Event.Select();
            Window.Visible = false;
        }
        else
        {
            StrategicMap.Current.ShowWarning(reason);
        }
    }

    // AoWUnitSelDlg.pas:120
    public void U1Change(CheckBox sender, int slot)
    {
        string reason = "";
        if (sender.Checked)
        {
            if (!Event.Canceled)
            {
                Event.SetSelection((byte)(1 << slot));
            }
            else
            {
                Event.SetSelection((byte)(Event.Selection | (1 << slot)));
            }
        }
        else
        {
            Event.SetSelection((byte)(Event.Selection & ~(1 << slot)));
        }

        Unit1CheckBox.Checked = (Event.Selection & 1) != 0;
        Unit2CheckBox.Checked = (Event.Selection & 2) != 0;
        Unit3CheckBox.Checked = (Event.Selection & 4) != 0;
        Unit4CheckBox.Checked = (Event.Selection & 8) != 0;
        Unit5CheckBox.Checked = (Event.Selection & 16) != 0;
        Unit6CheckBox.Checked = (Event.Selection & 32) != 0;
        Unit7CheckBox.Checked = (Event.Selection & 64) != 0;
        Unit8CheckBox.Checked = (Event.Selection & 128) != 0;
        if ((Event.Selectable & (1 << slot)) != 0)
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

        if (Event.Selection != 0)
        {
            if (Event.CanSelect(ref reason))
            {
                SelectionInfoLabel.TextColor = 0;
            }
            else
            {
                SelectionInfoLabel.TextColor = 149;
            }
        }
        else
        {
            SelectionInfoLabel.Text = "";
        }
    }

    // AoWUnitSelDlg.pas:146
    public void U1PreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        DrawUnit(slot, UnitPanel.WinLeft + sender.WinLeft, UnitPanel.WinTop + sender.WinTop, ref lockedSurface);
    }

    // AoWUnitSelDlg.pas:151
    public void SourceFacePreDraw(LockedSurface lockedSurface)
    {
        Rect r = default;
        Troop u;
        if (Event != null)
        {
            u = StrategicMap.Current.UnitControl.FindUnit(Event.SourceUnitId);
            if (u != null)
            {
                r.Left = this.SourceFace.WinLeft + SourcePanel.WinLeft;
                r.Right = this.SourceFace.WinWidth + r.Left;
                r.Top = this.SourceFace.WinTop + SourcePanel.WinTop;
                r.Bottom = this.SourceFace.WinHeight + r.Top;

                if (u.GetFace() != null)
                {
                    u.GetFace().ShowStretched(r, lockedSurface);
                }
            }
        }
    }

    // AoWUnitSelDlg.pas:171
    public void DlgHide()
    {
        ClearEvent();
    }
}
