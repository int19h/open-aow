// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class PopupWindows : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "PopupWindows";
        StartMenu = new StartMenu(this);
        session.BeginRead(StartMenu);
        StartMenu.Name = "StartMenu";
        StartMenu.Visible = false;
        StartMenu.Priority = 305;
        StartMenu.WinLeft = 0;
        StartMenu.WinTop = 12900;
        StartMenu.WinWidth = 250;
        StartMenu.Masked = true;
        StartMenu.Alignment.AlignWidth = AlignWidth.None;
        StartMenu.Alignment.AlignHeight = AlignHeight.None;
        StartMenu.Alignment.HeightPercent = 0;
        StartMenu.Alignment.WidthPercent = 0;
        StartMenu.Alignment.LeftOffsetPercent = 0;
        StartMenu.Alignment.RightOffsetPercent = 0;
        StartMenu.Alignment.TopOffsetPercent = 0;
        StartMenu.Alignment.BottomOffsetPercent = 0;
        StartMenu.Alignment.LeftOffset = 0;
        StartMenu.Alignment.RightOffset = 0;
        StartMenu.Alignment.TopOffset = 0;
        StartMenu.Alignment.BottomOffset = 0;
        StartMenu.UseVideoMemory = false;
        StartMenu.BorderWidth = 50;
        StartMenu.BorderHeight = 15;
        StartMenu.OverlapTopCorners = false;
        StartMenu.Cursor = 0;
        StartMenu.DoubleBuffered = true;
        StartMenu.MaskColor = 0;
        StartMenu.FreeSurfaceOnHide = true;
        StartMenu.ImageLibraryIndexFrame = 7;
        StartMenu.ImageLibraryIndexTNormal = 2;
        StartMenu.ImageLibraryIndexTSelected = 0;
        StartMenu.LayoutItems = new LayoutStrings()
        {
            "Overviews",
            "Settings",
            "End Game",
            "Disk",
            "Help"
        };
        StartMenu.TextColor = 6291456;
        StartMenu.TextSelectedColor = 16728128;
        StartMenu.ItemHeight = 39;
        StartMenu.OwnerItem = 0;
        StartMenu.SubMenuShowDelay = 5;
        StartMenu.WinHeight = 0;
        session.EndRead(StartMenu);
        OverViewsMenu = new Menu(this);
        session.BeginRead(OverViewsMenu);
        OverViewsMenu.Name = "OverViewsMenu";
        OverViewsMenu.Visible = false;
        OverViewsMenu.Priority = 310;
        OverViewsMenu.WinLeft = 0;
        OverViewsMenu.WinTop = 0;
        OverViewsMenu.WinWidth = 200;
        OverViewsMenu.Masked = true;
        OverViewsMenu.Alignment.AlignWidth = AlignWidth.None;
        OverViewsMenu.Alignment.AlignHeight = AlignHeight.None;
        OverViewsMenu.Alignment.HeightPercent = 0;
        OverViewsMenu.Alignment.WidthPercent = 0;
        OverViewsMenu.Alignment.LeftOffsetPercent = 0;
        OverViewsMenu.Alignment.RightOffsetPercent = 0;
        OverViewsMenu.Alignment.TopOffsetPercent = 0;
        OverViewsMenu.Alignment.BottomOffsetPercent = 0;
        OverViewsMenu.Alignment.LeftOffset = 0;
        OverViewsMenu.Alignment.RightOffset = 0;
        OverViewsMenu.Alignment.TopOffset = 0;
        OverViewsMenu.Alignment.BottomOffset = 0;
        OverViewsMenu.UseVideoMemory = false;
        OverViewsMenu.BorderWidth = 10;
        OverViewsMenu.BorderHeight = 5;
        OverViewsMenu.OverlapTopCorners = false;
        OverViewsMenu.Cursor = 0;
        OverViewsMenu.DoubleBuffered = true;
        OverViewsMenu.MaskColor = 128;
        OverViewsMenu.FreeSurfaceOnHide = true;
        OverViewsMenu.ImageLibraryIndexFrame = 0;
        OverViewsMenu.ImageLibraryIndexTNormal = 2;
        OverViewsMenu.ImageLibraryIndexTSelected = 9;
        OverViewsMenu.LayoutItems = new LayoutStrings()
        {
            "Background"
        };
        OverViewsMenu.TextColor = 0;
        OverViewsMenu.TextSelectedColor = 0;
        OverViewsMenu.ItemHeight = 20;
        OverViewsMenu.OwnerItem = 0;
        OverViewsMenu.SubMenuShowDelay = 0;
        OverViewsMenu.OnSelect = OverViewsMenuSelect;
        OverViewsMenu.WinHeight = 0;
        session.EndRead(OverViewsMenu);
        SettingsMenu = new Menu(this);
        session.BeginRead(SettingsMenu);
        SettingsMenu.Name = "SettingsMenu";
        SettingsMenu.Visible = false;
        SettingsMenu.Priority = 310;
        SettingsMenu.WinLeft = 0;
        SettingsMenu.WinTop = 0;
        SettingsMenu.WinWidth = 200;
        SettingsMenu.Masked = true;
        SettingsMenu.Alignment.AlignWidth = AlignWidth.None;
        SettingsMenu.Alignment.AlignHeight = AlignHeight.None;
        SettingsMenu.Alignment.HeightPercent = 0;
        SettingsMenu.Alignment.WidthPercent = 0;
        SettingsMenu.Alignment.LeftOffsetPercent = 0;
        SettingsMenu.Alignment.RightOffsetPercent = 0;
        SettingsMenu.Alignment.TopOffsetPercent = 0;
        SettingsMenu.Alignment.BottomOffsetPercent = 0;
        SettingsMenu.Alignment.LeftOffset = 0;
        SettingsMenu.Alignment.RightOffset = 0;
        SettingsMenu.Alignment.TopOffset = 0;
        SettingsMenu.Alignment.BottomOffset = 0;
        SettingsMenu.UseVideoMemory = false;
        SettingsMenu.BorderWidth = 10;
        SettingsMenu.BorderHeight = 5;
        SettingsMenu.OverlapTopCorners = false;
        SettingsMenu.Cursor = 0;
        SettingsMenu.DoubleBuffered = true;
        SettingsMenu.MaskColor = 128;
        SettingsMenu.FreeSurfaceOnHide = true;
        SettingsMenu.ImageLibraryIndexFrame = 0;
        SettingsMenu.ImageLibraryIndexTNormal = 2;
        SettingsMenu.ImageLibraryIndexTSelected = 9;
        SettingsMenu.LayoutItems = new LayoutStrings()
        {
            "Display",
            "Sound",
            "Gameplay",
            "Reset Layout",
            "Email"
        };
        SettingsMenu.TextColor = 0;
        SettingsMenu.TextSelectedColor = 0;
        SettingsMenu.ItemHeight = 20;
        SettingsMenu.OwnerItem = 1;
        SettingsMenu.SubMenuShowDelay = 0;
        SettingsMenu.OnSelect = SettingsMenuSelect;
        SettingsMenu.WinHeight = 112;
        session.EndRead(SettingsMenu);
        EndMenu = new Menu(this);
        session.BeginRead(EndMenu);
        EndMenu.Name = "EndMenu";
        EndMenu.Visible = false;
        EndMenu.Priority = 310;
        EndMenu.WinLeft = 0;
        EndMenu.WinTop = 0;
        EndMenu.WinWidth = 200;
        EndMenu.Masked = true;
        EndMenu.Alignment.AlignWidth = AlignWidth.None;
        EndMenu.Alignment.AlignHeight = AlignHeight.None;
        EndMenu.Alignment.HeightPercent = 0;
        EndMenu.Alignment.WidthPercent = 0;
        EndMenu.Alignment.LeftOffsetPercent = 0;
        EndMenu.Alignment.RightOffsetPercent = 0;
        EndMenu.Alignment.TopOffsetPercent = 0;
        EndMenu.Alignment.BottomOffsetPercent = 0;
        EndMenu.Alignment.LeftOffset = 0;
        EndMenu.Alignment.RightOffset = 0;
        EndMenu.Alignment.TopOffset = 0;
        EndMenu.Alignment.BottomOffset = 0;
        EndMenu.UseVideoMemory = false;
        EndMenu.BorderWidth = 10;
        EndMenu.BorderHeight = 5;
        EndMenu.OverlapTopCorners = false;
        EndMenu.Cursor = 0;
        EndMenu.DoubleBuffered = true;
        EndMenu.MaskColor = 128;
        EndMenu.FreeSurfaceOnHide = true;
        EndMenu.ImageLibraryIndexFrame = 0;
        EndMenu.ImageLibraryIndexTNormal = 2;
        EndMenu.ImageLibraryIndexTSelected = 9;
        EndMenu.LayoutItems = new LayoutStrings()
        {
            "Surrender",
            "Exit to Title",
            "Exit to Windows"
        };
        EndMenu.TextColor = 0;
        EndMenu.TextSelectedColor = 0;
        EndMenu.ItemHeight = 20;
        EndMenu.OwnerItem = 2;
        EndMenu.SubMenuShowDelay = 0;
        EndMenu.OnSelect = EndMenuSelect;
        EndMenu.WinHeight = 92;
        session.EndRead(EndMenu);
        DiskMenu = new Menu(this);
        session.BeginRead(DiskMenu);
        DiskMenu.Name = "DiskMenu";
        DiskMenu.Visible = false;
        DiskMenu.Priority = 310;
        DiskMenu.WinLeft = 0;
        DiskMenu.WinTop = 0;
        DiskMenu.WinWidth = 200;
        DiskMenu.Masked = true;
        DiskMenu.Alignment.AlignWidth = AlignWidth.None;
        DiskMenu.Alignment.AlignHeight = AlignHeight.None;
        DiskMenu.Alignment.HeightPercent = 0;
        DiskMenu.Alignment.WidthPercent = 0;
        DiskMenu.Alignment.LeftOffsetPercent = 0;
        DiskMenu.Alignment.RightOffsetPercent = 0;
        DiskMenu.Alignment.TopOffsetPercent = 0;
        DiskMenu.Alignment.BottomOffsetPercent = 0;
        DiskMenu.Alignment.LeftOffset = 0;
        DiskMenu.Alignment.RightOffset = 0;
        DiskMenu.Alignment.TopOffset = 0;
        DiskMenu.Alignment.BottomOffset = 0;
        DiskMenu.UseVideoMemory = false;
        DiskMenu.BorderWidth = 10;
        DiskMenu.BorderHeight = 5;
        DiskMenu.OverlapTopCorners = false;
        DiskMenu.Cursor = 0;
        DiskMenu.DoubleBuffered = true;
        DiskMenu.MaskColor = 128;
        DiskMenu.FreeSurfaceOnHide = true;
        DiskMenu.ImageLibraryIndexFrame = 0;
        DiskMenu.ImageLibraryIndexTNormal = 2;
        DiskMenu.ImageLibraryIndexTSelected = 9;
        DiskMenu.LayoutItems = new LayoutStrings()
        {
            "Load Game",
            "Save Game"
        };
        DiskMenu.TextColor = 0;
        DiskMenu.TextSelectedColor = 0;
        DiskMenu.ItemHeight = 20;
        DiskMenu.OwnerItem = 3;
        DiskMenu.SubMenuShowDelay = 0;
        DiskMenu.OnSelect = DiskMenuSelect;
        DiskMenu.WinHeight = 0;
        session.EndRead(DiskMenu);
        TCStartMenu = new StartMenu(this);
        session.BeginRead(TCStartMenu);
        TCStartMenu.Name = "TCStartMenu";
        TCStartMenu.Visible = false;
        TCStartMenu.Priority = 305;
        TCStartMenu.WinLeft = 0;
        TCStartMenu.WinTop = 1000;
        TCStartMenu.WinWidth = 250;
        TCStartMenu.Masked = true;
        TCStartMenu.Alignment.AlignWidth = AlignWidth.None;
        TCStartMenu.Alignment.AlignHeight = AlignHeight.None;
        TCStartMenu.Alignment.HeightPercent = 0;
        TCStartMenu.Alignment.WidthPercent = 0;
        TCStartMenu.Alignment.LeftOffsetPercent = 0;
        TCStartMenu.Alignment.RightOffsetPercent = 0;
        TCStartMenu.Alignment.TopOffsetPercent = 0;
        TCStartMenu.Alignment.BottomOffsetPercent = 0;
        TCStartMenu.Alignment.LeftOffset = 0;
        TCStartMenu.Alignment.RightOffset = 0;
        TCStartMenu.Alignment.TopOffset = 0;
        TCStartMenu.Alignment.BottomOffset = 0;
        TCStartMenu.UseVideoMemory = false;
        TCStartMenu.BorderWidth = 50;
        TCStartMenu.BorderHeight = 15;
        TCStartMenu.OverlapTopCorners = false;
        TCStartMenu.Cursor = 0;
        TCStartMenu.DoubleBuffered = true;
        TCStartMenu.MaskColor = 0;
        TCStartMenu.FreeSurfaceOnHide = true;
        TCStartMenu.ImageLibraryIndexFrame = 7;
        TCStartMenu.ImageLibraryIndexTNormal = 2;
        TCStartMenu.ImageLibraryIndexTSelected = 0;
        TCStartMenu.LayoutItems = new LayoutStrings()
        {
            "Overviews",
            "Settings",
            "End Combat",
            "Disk",
            "Help"
        };
        TCStartMenu.TextColor = 6291456;
        TCStartMenu.TextSelectedColor = 16728128;
        TCStartMenu.ItemHeight = 39;
        TCStartMenu.OwnerItem = 0;
        TCStartMenu.SubMenuShowDelay = 5;
        TCStartMenu.WinHeight = 0;
        session.EndRead(TCStartMenu);
        LayoutIvTranslator createdTranslator = new LayoutIvTranslator(this);
        Translator = createdTranslator;
        session.BeginRead(createdTranslator);
        createdTranslator.Name = "Translator";
        createdTranslator.DictionaryName = "AoW";
        createdTranslator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        createdTranslator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        createdTranslator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(createdTranslator);
        HelpMenu = new Menu(this);
        session.BeginRead(HelpMenu);
        HelpMenu.Name = "HelpMenu";
        HelpMenu.Visible = false;
        HelpMenu.Priority = 310;
        HelpMenu.WinLeft = 0;
        HelpMenu.WinTop = 0;
        HelpMenu.WinWidth = 200;
        HelpMenu.Masked = true;
        HelpMenu.Alignment.AlignWidth = AlignWidth.None;
        HelpMenu.Alignment.AlignHeight = AlignHeight.None;
        HelpMenu.Alignment.HeightPercent = 0;
        HelpMenu.Alignment.WidthPercent = 0;
        HelpMenu.Alignment.LeftOffsetPercent = 0;
        HelpMenu.Alignment.RightOffsetPercent = 0;
        HelpMenu.Alignment.TopOffsetPercent = 0;
        HelpMenu.Alignment.BottomOffsetPercent = 0;
        HelpMenu.Alignment.LeftOffset = 0;
        HelpMenu.Alignment.RightOffset = 0;
        HelpMenu.Alignment.TopOffset = 0;
        HelpMenu.Alignment.BottomOffset = 0;
        HelpMenu.UseVideoMemory = false;
        HelpMenu.BorderWidth = 10;
        HelpMenu.BorderHeight = 5;
        HelpMenu.OverlapTopCorners = false;
        HelpMenu.Cursor = 0;
        HelpMenu.DoubleBuffered = true;
        HelpMenu.MaskColor = 128;
        HelpMenu.FreeSurfaceOnHide = true;
        HelpMenu.ImageLibraryIndexFrame = 0;
        HelpMenu.ImageLibraryIndexTNormal = 2;
        HelpMenu.ImageLibraryIndexTSelected = 9;
        HelpMenu.LayoutItems = new LayoutStrings()
        {
            "General Interface",
            "Map Interface",
            "www.ageofwonders.com"
        };
        HelpMenu.TextColor = 0;
        HelpMenu.TextSelectedColor = 0;
        HelpMenu.ItemHeight = 20;
        HelpMenu.OwnerItem = 4;
        HelpMenu.SubMenuShowDelay = 0;
        HelpMenu.OnSelect = HelpMenuSelect;
        HelpMenu.WinHeight = 0;
        session.EndRead(HelpMenu);
        TCOVMenu = new Menu(this);
        session.BeginRead(TCOVMenu);
        TCOVMenu.Name = "TCOVMenu";
        TCOVMenu.Visible = false;
        TCOVMenu.Priority = 310;
        TCOVMenu.WinLeft = 0;
        TCOVMenu.WinTop = 0;
        TCOVMenu.WinWidth = 200;
        TCOVMenu.Masked = true;
        TCOVMenu.Alignment.AlignWidth = AlignWidth.None;
        TCOVMenu.Alignment.AlignHeight = AlignHeight.None;
        TCOVMenu.Alignment.HeightPercent = 0;
        TCOVMenu.Alignment.WidthPercent = 0;
        TCOVMenu.Alignment.LeftOffsetPercent = 0;
        TCOVMenu.Alignment.RightOffsetPercent = 0;
        TCOVMenu.Alignment.TopOffsetPercent = 0;
        TCOVMenu.Alignment.BottomOffsetPercent = 0;
        TCOVMenu.Alignment.LeftOffset = 0;
        TCOVMenu.Alignment.RightOffset = 0;
        TCOVMenu.Alignment.TopOffset = 0;
        TCOVMenu.Alignment.BottomOffset = 0;
        TCOVMenu.UseVideoMemory = false;
        TCOVMenu.BorderWidth = 10;
        TCOVMenu.BorderHeight = 5;
        TCOVMenu.OverlapTopCorners = false;
        TCOVMenu.Cursor = 0;
        TCOVMenu.DoubleBuffered = true;
        TCOVMenu.MaskColor = 128;
        TCOVMenu.FreeSurfaceOnHide = true;
        TCOVMenu.ImageLibraryIndexFrame = 0;
        TCOVMenu.ImageLibraryIndexTNormal = 2;
        TCOVMenu.ImageLibraryIndexTSelected = 9;
        TCOVMenu.LayoutItems = new LayoutStrings()
        {
            "Background"
        };
        TCOVMenu.TextColor = 0;
        TCOVMenu.TextSelectedColor = 0;
        TCOVMenu.ItemHeight = 20;
        TCOVMenu.OwnerItem = 0;
        TCOVMenu.SubMenuShowDelay = 0;
        TCOVMenu.OnSelect = OverViewsMenuSelect;
        TCOVMenu.WinHeight = 0;
        session.EndRead(TCOVMenu);
        TCDiskMenu = new Menu(this);
        session.BeginRead(TCDiskMenu);
        TCDiskMenu.Name = "TCDiskMenu";
        TCDiskMenu.Visible = false;
        TCDiskMenu.Priority = 310;
        TCDiskMenu.WinLeft = 0;
        TCDiskMenu.WinTop = 0;
        TCDiskMenu.WinWidth = 200;
        TCDiskMenu.Masked = true;
        TCDiskMenu.Alignment.AlignWidth = AlignWidth.None;
        TCDiskMenu.Alignment.AlignHeight = AlignHeight.None;
        TCDiskMenu.Alignment.HeightPercent = 0;
        TCDiskMenu.Alignment.WidthPercent = 0;
        TCDiskMenu.Alignment.LeftOffsetPercent = 0;
        TCDiskMenu.Alignment.RightOffsetPercent = 0;
        TCDiskMenu.Alignment.TopOffsetPercent = 0;
        TCDiskMenu.Alignment.BottomOffsetPercent = 0;
        TCDiskMenu.Alignment.LeftOffset = 0;
        TCDiskMenu.Alignment.RightOffset = 0;
        TCDiskMenu.Alignment.TopOffset = 0;
        TCDiskMenu.Alignment.BottomOffset = 0;
        TCDiskMenu.UseVideoMemory = false;
        TCDiskMenu.BorderWidth = 10;
        TCDiskMenu.BorderHeight = 5;
        TCDiskMenu.OverlapTopCorners = false;
        TCDiskMenu.Cursor = 0;
        TCDiskMenu.DoubleBuffered = true;
        TCDiskMenu.MaskColor = 128;
        TCDiskMenu.FreeSurfaceOnHide = true;
        TCDiskMenu.ImageLibraryIndexFrame = 0;
        TCDiskMenu.ImageLibraryIndexTNormal = 2;
        TCDiskMenu.ImageLibraryIndexTSelected = 9;
        TCDiskMenu.LayoutItems = new LayoutStrings()
        {
            "Load Game"
        };
        TCDiskMenu.TextColor = 0;
        TCDiskMenu.TextSelectedColor = 0;
        TCDiskMenu.ItemHeight = 20;
        TCDiskMenu.OwnerItem = 3;
        TCDiskMenu.SubMenuShowDelay = 0;
        TCDiskMenu.OnSelect = DiskMenuSelect;
        TCDiskMenu.WinHeight = 0;
        session.EndRead(TCDiskMenu);
        TCHelpMenu = new Menu(this);
        session.BeginRead(TCHelpMenu);
        TCHelpMenu.Name = "TCHelpMenu";
        TCHelpMenu.Visible = false;
        TCHelpMenu.Priority = 310;
        TCHelpMenu.WinLeft = 0;
        TCHelpMenu.WinTop = 0;
        TCHelpMenu.WinWidth = 200;
        TCHelpMenu.Masked = true;
        TCHelpMenu.Alignment.AlignWidth = AlignWidth.None;
        TCHelpMenu.Alignment.AlignHeight = AlignHeight.None;
        TCHelpMenu.Alignment.HeightPercent = 0;
        TCHelpMenu.Alignment.WidthPercent = 0;
        TCHelpMenu.Alignment.LeftOffsetPercent = 0;
        TCHelpMenu.Alignment.RightOffsetPercent = 0;
        TCHelpMenu.Alignment.TopOffsetPercent = 0;
        TCHelpMenu.Alignment.BottomOffsetPercent = 0;
        TCHelpMenu.Alignment.LeftOffset = 0;
        TCHelpMenu.Alignment.RightOffset = 0;
        TCHelpMenu.Alignment.TopOffset = 0;
        TCHelpMenu.Alignment.BottomOffset = 0;
        TCHelpMenu.UseVideoMemory = false;
        TCHelpMenu.BorderWidth = 10;
        TCHelpMenu.BorderHeight = 5;
        TCHelpMenu.OverlapTopCorners = false;
        TCHelpMenu.Cursor = 0;
        TCHelpMenu.DoubleBuffered = true;
        TCHelpMenu.MaskColor = 128;
        TCHelpMenu.FreeSurfaceOnHide = true;
        TCHelpMenu.ImageLibraryIndexFrame = 0;
        TCHelpMenu.ImageLibraryIndexTNormal = 2;
        TCHelpMenu.ImageLibraryIndexTSelected = 9;
        TCHelpMenu.LayoutItems = new LayoutStrings()
        {
            "General Interface",
            "Map Interface",
            "www.ageofwonders.com"
        };
        TCHelpMenu.TextColor = 0;
        TCHelpMenu.TextSelectedColor = 0;
        TCHelpMenu.ItemHeight = 20;
        TCHelpMenu.OwnerItem = 4;
        TCHelpMenu.SubMenuShowDelay = 0;
        TCHelpMenu.OnSelect = HelpMenuSelect;
        TCHelpMenu.WinHeight = 0;
        session.EndRead(TCHelpMenu);
        TCSettingsMenu = new Menu(this);
        session.BeginRead(TCSettingsMenu);
        TCSettingsMenu.Name = "TCSettingsMenu";
        TCSettingsMenu.Visible = false;
        TCSettingsMenu.Priority = 310;
        TCSettingsMenu.WinLeft = 0;
        TCSettingsMenu.WinTop = 0;
        TCSettingsMenu.WinWidth = 200;
        TCSettingsMenu.Masked = true;
        TCSettingsMenu.Alignment.AlignWidth = AlignWidth.None;
        TCSettingsMenu.Alignment.AlignHeight = AlignHeight.None;
        TCSettingsMenu.Alignment.HeightPercent = 0;
        TCSettingsMenu.Alignment.WidthPercent = 0;
        TCSettingsMenu.Alignment.LeftOffsetPercent = 0;
        TCSettingsMenu.Alignment.RightOffsetPercent = 0;
        TCSettingsMenu.Alignment.TopOffsetPercent = 0;
        TCSettingsMenu.Alignment.BottomOffsetPercent = 0;
        TCSettingsMenu.Alignment.LeftOffset = 0;
        TCSettingsMenu.Alignment.RightOffset = 0;
        TCSettingsMenu.Alignment.TopOffset = 0;
        TCSettingsMenu.Alignment.BottomOffset = 0;
        TCSettingsMenu.UseVideoMemory = false;
        TCSettingsMenu.BorderWidth = 10;
        TCSettingsMenu.BorderHeight = 5;
        TCSettingsMenu.OverlapTopCorners = false;
        TCSettingsMenu.Cursor = 0;
        TCSettingsMenu.DoubleBuffered = true;
        TCSettingsMenu.MaskColor = 128;
        TCSettingsMenu.FreeSurfaceOnHide = true;
        TCSettingsMenu.ImageLibraryIndexFrame = 0;
        TCSettingsMenu.ImageLibraryIndexTNormal = 2;
        TCSettingsMenu.ImageLibraryIndexTSelected = 9;
        TCSettingsMenu.LayoutItems = new LayoutStrings()
        {
            "Display",
            "Sound",
            "Gameplay",
            "Reset Layout",
            "Email"
        };
        TCSettingsMenu.TextColor = 0;
        TCSettingsMenu.TextSelectedColor = 0;
        TCSettingsMenu.ItemHeight = 20;
        TCSettingsMenu.OwnerItem = 1;
        TCSettingsMenu.SubMenuShowDelay = 0;
        TCSettingsMenu.OnSelect = SettingsMenuSelect;
        TCSettingsMenu.WinHeight = 0;
        session.EndRead(TCSettingsMenu);
        TCEndMenu = new Menu(this);
        session.BeginRead(TCEndMenu);
        TCEndMenu.Name = "TCEndMenu";
        TCEndMenu.Visible = false;
        TCEndMenu.Priority = 310;
        TCEndMenu.WinLeft = 0;
        TCEndMenu.WinTop = 0;
        TCEndMenu.WinWidth = 200;
        TCEndMenu.Masked = true;
        TCEndMenu.Alignment.AlignWidth = AlignWidth.None;
        TCEndMenu.Alignment.AlignHeight = AlignHeight.None;
        TCEndMenu.Alignment.HeightPercent = 0;
        TCEndMenu.Alignment.WidthPercent = 0;
        TCEndMenu.Alignment.LeftOffsetPercent = 0;
        TCEndMenu.Alignment.RightOffsetPercent = 0;
        TCEndMenu.Alignment.TopOffsetPercent = 0;
        TCEndMenu.Alignment.BottomOffsetPercent = 0;
        TCEndMenu.Alignment.LeftOffset = 0;
        TCEndMenu.Alignment.RightOffset = 0;
        TCEndMenu.Alignment.TopOffset = 0;
        TCEndMenu.Alignment.BottomOffset = 0;
        TCEndMenu.UseVideoMemory = false;
        TCEndMenu.BorderWidth = 10;
        TCEndMenu.BorderHeight = 5;
        TCEndMenu.OverlapTopCorners = false;
        TCEndMenu.Cursor = 0;
        TCEndMenu.DoubleBuffered = true;
        TCEndMenu.MaskColor = 128;
        TCEndMenu.FreeSurfaceOnHide = true;
        TCEndMenu.ImageLibraryIndexFrame = 0;
        TCEndMenu.ImageLibraryIndexTNormal = 2;
        TCEndMenu.ImageLibraryIndexTSelected = 9;
        TCEndMenu.LayoutItems = new LayoutStrings()
        {
            "Surrender"
        };
        TCEndMenu.TextColor = 0;
        TCEndMenu.TextSelectedColor = 0;
        TCEndMenu.ItemHeight = 20;
        TCEndMenu.OwnerItem = 2;
        TCEndMenu.SubMenuShowDelay = 0;
        TCEndMenu.OnSelect = TCEndMenuSelect;
        TCEndMenu.WinHeight = 0;
        session.EndRead(TCEndMenu);
        session.EndRead(this);
        OverViewsMenu.OwnerMenu = StartMenu;
        SettingsMenu.OwnerMenu = StartMenu;
        EndMenu.OwnerMenu = StartMenu;
        DiskMenu.OwnerMenu = StartMenu;
        HelpMenu.OwnerMenu = StartMenu;
        TCOVMenu.OwnerMenu = TCStartMenu;
        TCDiskMenu.OwnerMenu = TCStartMenu;
        TCHelpMenu.OwnerMenu = TCStartMenu;
        TCSettingsMenu.OwnerMenu = TCStartMenu;
        TCEndMenu.OwnerMenu = TCStartMenu;
        Loaded();
        StartMenu.Loaded();
        OverViewsMenu.Loaded();
        SettingsMenu.Loaded();
        EndMenu.Loaded();
        DiskMenu.Loaded();
        TCStartMenu.Loaded();
        Translator.Loaded();
        HelpMenu.Loaded();
        TCOVMenu.Loaded();
        TCDiskMenu.Loaded();
        TCHelpMenu.Loaded();
        TCSettingsMenu.Loaded();
        TCEndMenu.Loaded();
        session.Resolve(this);
        StartMenu.WindowManager = DialogHosts.General.Manager;
        StartMenu.ImageLibraryFrame = DialogHosts.UIGraphics.DragonMenu;
        StartMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        StartMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.GenericT;
        StartMenu.Font = DialogHosts.UIFonts.Age10b;
        session.Defer("TBWindow", (TaskBar module) => StartMenu.OwnerMenuBar = module.StartMenuBar);
        StartMenu.ImageLibraryWholeMenu = DialogHosts.UIGraphics.DragonMenu;
        OverViewsMenu.WindowManager = DialogHosts.General.Manager;
        OverViewsMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        OverViewsMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        OverViewsMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        OverViewsMenu.Font = DialogHosts.UIFonts.Age10;
        SettingsMenu.WindowManager = DialogHosts.General.Manager;
        SettingsMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        SettingsMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        SettingsMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        SettingsMenu.Font = DialogHosts.UIFonts.Age10;
        EndMenu.WindowManager = DialogHosts.General.Manager;
        EndMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        EndMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        EndMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        EndMenu.Font = DialogHosts.UIFonts.Age10;
        DiskMenu.WindowManager = DialogHosts.General.Manager;
        DiskMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        DiskMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        DiskMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        DiskMenu.Font = DialogHosts.UIFonts.Age10;
        TCStartMenu.WindowManager = DialogHosts.General.Manager;
        TCStartMenu.ImageLibraryFrame = DialogHosts.UIGraphics.DragonMenu;
        TCStartMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        TCStartMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.GenericT;
        TCStartMenu.Font = DialogHosts.UIFonts.Age10b;
        session.Defer("TBWindow", (TaskBar module) => TCStartMenu.OwnerMenuBar = module.TCStartBar);
        TCStartMenu.ImageLibraryWholeMenu = DialogHosts.UIGraphics.DragonMenu;
        HelpMenu.WindowManager = DialogHosts.General.Manager;
        HelpMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        HelpMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        HelpMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        HelpMenu.Font = DialogHosts.UIFonts.Age10;
        TCOVMenu.WindowManager = DialogHosts.General.Manager;
        TCOVMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        TCOVMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        TCOVMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        TCOVMenu.Font = DialogHosts.UIFonts.Age10;
        TCDiskMenu.WindowManager = DialogHosts.General.Manager;
        TCDiskMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        TCDiskMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        TCDiskMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        TCDiskMenu.Font = DialogHosts.UIFonts.Age10;
        TCHelpMenu.WindowManager = DialogHosts.General.Manager;
        TCHelpMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        TCHelpMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        TCHelpMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        TCHelpMenu.Font = DialogHosts.UIFonts.Age10;
        TCSettingsMenu.WindowManager = DialogHosts.General.Manager;
        TCSettingsMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        TCSettingsMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        TCSettingsMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        TCSettingsMenu.Font = DialogHosts.UIFonts.Age10;
        TCEndMenu.WindowManager = DialogHosts.General.Manager;
        TCEndMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        TCEndMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericT;
        TCEndMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        TCEndMenu.Font = DialogHosts.UIFonts.Age10;
    }

}
