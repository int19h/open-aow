#nullable disable warnings
// Translated from StartMenu.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Windows;

// whole-menu image library
public sealed class StartMenu(Component? owner) : Menu(owner)
{

    // StartMenu.pas:30
    public override void SetSize()
    {
        base.SetSize();
        SetWinHeightWithoutInvalidate(377);
        WindowManager? m = WindowManager;
        if (m is not null && OwnerMenuBar is not null)
        {
            SetWinTopWithoutInvalidate(m.WinHeight - 393);
        }

        SetWinLeftWithoutInvalidate(0);
    }

    // StartMenu.pas:41
    public override void SetVisible(bool value)
    {
        if (value != Visible)
        {
            SetVisibleWithoutUpdate(value);
            if (Visible)
            {
                Highlighted = -1;
                SetWinHeightWithoutInvalidate(377);
                WindowManager? m = WindowManager;
                if (m is not null && OwnerMenuBar is not null)
                {
                    SetWinTopWithoutInvalidate(m.WinHeight - 393);
                }

                SetWinLeftWithoutInvalidate(0);
                IsDrawn = false;
                if (!IsHidden)
                {
                    BackRectangle = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
                }

                RaiseShow();
            }
            else
            {
                MenuBar? b = OwnerMenuBar;
                if (b is not null)
                {
                    b.Selected = -1;
                    b.IsDrawn = false;
                }

                IsHidden = true;
                RaiseHide();
            }
        }

        if (Visible)
        {
            if (!Valid || !Aligned || !Sized)
            {
                Setup();
            }

            ClearBlitList();
            ClearDrawList();
            IsDrawn = false;
            SetPriority(Priority);
        }
    }

    // StartMenu.pas:87
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                if (!IsDrawn)
                {
                    Rect oldClip = lockedSurface.ClipRectangle;
                    Font.Color = Color;
                    ImageLibraryWholeMenu[Highlighted + 1].ShowClipped(0, 0, lockedSurface);
                    for (int i = 0, last = Items.Count - 1; i <= last; i++)
                    {
                        if (Items.Count > 0)
                        {
                            LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                            if (WinLeft + WinWidth - imageLibraryFrameWidth.Width < oldClip.Right)
                            {
                                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                                Rect clipRectangle = lockedSurface.ClipRectangle;
                                clipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame.Width;
                                lockedSurface.ClipRectangle = clipRectangle;
                            }

                            if (i == Highlighted)
                            {
                                Font.Color = SelectedColor;
                            }

                            string item = Items[i];
                            LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                            LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Font.SetText(WinLeft + BorderWidth + imageLibraryFrame2.Width, WinTop + BorderHeight + imageLibraryFrameHeight.Height + (ItemHeight * i), text: item, lockedSurface);
                            if (i == Highlighted)
                            {
                                Font.Color = Color;
                            }

                            int drawItem = i;
                            LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage? imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage? imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Rect drawItemRectangle = new(WinLeft + imageLibraryFrame5.Width, WinTop + imageLibraryFrame4.Height + (ItemHeight * i), WinLeft + WinWidth, WinTop + imageLibraryFrame3.Height + ((i + 1) * ItemHeight));
                            RaiseDrawItem(drawRectangle: drawItemRectangle, drawItem: drawItem, lockedSurface: ref lockedSurface);

                            lockedSurface.ClipRectangle = oldClip;
                        }
                    }
                }

                RaiseDraw(surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // StartMenu.pas:123
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (Valid)
        {
            LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            SetWinHeightWithoutInvalidate(WinHeight - imageLibraryFrameHeight.Height);
            base.CheckMouseDown(button, shift, X, Y);
            LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            SetWinHeightWithoutInvalidate(WinHeight + imageLibraryFrame.Height);
        }
    }

    [Published]
    public ImageLibrary ImageLibraryWholeMenu { get; set; }
}
