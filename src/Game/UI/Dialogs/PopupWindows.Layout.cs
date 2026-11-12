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
        StartMenu startMenu = new StartMenu(this);
        session.BeginRead(startMenu);
        startMenu.Name = "StartMenu";
        startMenu.Visible = false;
        startMenu.Priority = 305;
        startMenu.WinLeft = 0;
        startMenu.WinTop = 12900;
        startMenu.WinWidth = 250;
        startMenu.Masked = true;
        startMenu.Alignment.AlignWidth = AlignWidth.None;
        startMenu.Alignment.AlignHeight = AlignHeight.None;
        startMenu.Alignment.HeightPercent = 0;
        startMenu.Alignment.WidthPercent = 0;
        startMenu.Alignment.LeftOffsetPercent = 0;
        startMenu.Alignment.RightOffsetPercent = 0;
        startMenu.Alignment.TopOffsetPercent = 0;
        startMenu.Alignment.BottomOffsetPercent = 0;
        startMenu.Alignment.LeftOffset = 0;
        startMenu.Alignment.RightOffset = 0;
        startMenu.Alignment.TopOffset = 0;
        startMenu.Alignment.BottomOffset = 0;
        startMenu.UseVideoMemory = false;
        startMenu.BorderWidth = 50;
        startMenu.BorderHeight = 15;
        startMenu.OverlapTopCorners = false;
        startMenu.Cursor = 0;
        startMenu.DoubleBuffered = true;
        startMenu.MaskColor = 0;
        startMenu.FreeSurfaceOnHide = true;
        startMenu.ImageLibraryIndexFrame = 7;
        startMenu.ImageLibraryIndexTNormal = 2;
        startMenu.ImageLibraryIndexTSelected = 0;
        startMenu.LayoutItems = new LayoutStrings()
        {
            "Overviews",
            "Settings",
            "End Game",
            "Disk",
            "Help"
        };
        startMenu.TextColor = 6291456;
        startMenu.TextSelectedColor = 16728128;
        startMenu.ItemHeight = 39;
        startMenu.OwnerItem = 0;
        startMenu.SubMenuShowDelay = 5;
        startMenu.WinHeight = 0;
        session.EndRead(startMenu);
        OverviewsMenu = new Menu(this);
        session.BeginRead(OverviewsMenu);
        OverviewsMenu.Name = "OverViewsMenu";
        OverviewsMenu.Visible = false;
        OverviewsMenu.Priority = 310;
        OverviewsMenu.WinLeft = 0;
        OverviewsMenu.WinTop = 0;
        OverviewsMenu.WinWidth = 200;
        OverviewsMenu.Masked = true;
        OverviewsMenu.Alignment.AlignWidth = AlignWidth.None;
        OverviewsMenu.Alignment.AlignHeight = AlignHeight.None;
        OverviewsMenu.Alignment.HeightPercent = 0;
        OverviewsMenu.Alignment.WidthPercent = 0;
        OverviewsMenu.Alignment.LeftOffsetPercent = 0;
        OverviewsMenu.Alignment.RightOffsetPercent = 0;
        OverviewsMenu.Alignment.TopOffsetPercent = 0;
        OverviewsMenu.Alignment.BottomOffsetPercent = 0;
        OverviewsMenu.Alignment.LeftOffset = 0;
        OverviewsMenu.Alignment.RightOffset = 0;
        OverviewsMenu.Alignment.TopOffset = 0;
        OverviewsMenu.Alignment.BottomOffset = 0;
        OverviewsMenu.UseVideoMemory = false;
        OverviewsMenu.BorderWidth = 10;
        OverviewsMenu.BorderHeight = 5;
        OverviewsMenu.OverlapTopCorners = false;
        OverviewsMenu.Cursor = 0;
        OverviewsMenu.DoubleBuffered = true;
        OverviewsMenu.MaskColor = 128;
        OverviewsMenu.FreeSurfaceOnHide = true;
        OverviewsMenu.ImageLibraryIndexFrame = 0;
        OverviewsMenu.ImageLibraryIndexTNormal = 2;
        OverviewsMenu.ImageLibraryIndexTSelected = 9;
        OverviewsMenu.LayoutItems = new LayoutStrings()
        {
            "Background"
        };
        OverviewsMenu.TextColor = 0;
        OverviewsMenu.TextSelectedColor = 0;
        OverviewsMenu.ItemHeight = 20;
        OverviewsMenu.OwnerItem = 0;
        OverviewsMenu.SubMenuShowDelay = 0;
        OverviewsMenu.OnSelect += OverViewsMenuSelect;
        OverviewsMenu.WinHeight = 0;
        session.EndRead(OverviewsMenu);
        Menu settingsMenu = new Menu(this);
        session.BeginRead(settingsMenu);
        settingsMenu.Name = "SettingsMenu";
        settingsMenu.Visible = false;
        settingsMenu.Priority = 310;
        settingsMenu.WinLeft = 0;
        settingsMenu.WinTop = 0;
        settingsMenu.WinWidth = 200;
        settingsMenu.Masked = true;
        settingsMenu.Alignment.AlignWidth = AlignWidth.None;
        settingsMenu.Alignment.AlignHeight = AlignHeight.None;
        settingsMenu.Alignment.HeightPercent = 0;
        settingsMenu.Alignment.WidthPercent = 0;
        settingsMenu.Alignment.LeftOffsetPercent = 0;
        settingsMenu.Alignment.RightOffsetPercent = 0;
        settingsMenu.Alignment.TopOffsetPercent = 0;
        settingsMenu.Alignment.BottomOffsetPercent = 0;
        settingsMenu.Alignment.LeftOffset = 0;
        settingsMenu.Alignment.RightOffset = 0;
        settingsMenu.Alignment.TopOffset = 0;
        settingsMenu.Alignment.BottomOffset = 0;
        settingsMenu.UseVideoMemory = false;
        settingsMenu.BorderWidth = 10;
        settingsMenu.BorderHeight = 5;
        settingsMenu.OverlapTopCorners = false;
        settingsMenu.Cursor = 0;
        settingsMenu.DoubleBuffered = true;
        settingsMenu.MaskColor = 128;
        settingsMenu.FreeSurfaceOnHide = true;
        settingsMenu.ImageLibraryIndexFrame = 0;
        settingsMenu.ImageLibraryIndexTNormal = 2;
        settingsMenu.ImageLibraryIndexTSelected = 9;
        settingsMenu.LayoutItems = new LayoutStrings()
        {
            "Display",
            "Sound",
            "Gameplay",
            "Reset Layout",
            "Email"
        };
        settingsMenu.TextColor = 0;
        settingsMenu.TextSelectedColor = 0;
        settingsMenu.ItemHeight = 20;
        settingsMenu.OwnerItem = 1;
        settingsMenu.SubMenuShowDelay = 0;
        settingsMenu.OnSelect += SettingsMenuSelect;
        settingsMenu.WinHeight = 112;
        session.EndRead(settingsMenu);
        Menu endMenu = new Menu(this);
        session.BeginRead(endMenu);
        endMenu.Name = "EndMenu";
        endMenu.Visible = false;
        endMenu.Priority = 310;
        endMenu.WinLeft = 0;
        endMenu.WinTop = 0;
        endMenu.WinWidth = 200;
        endMenu.Masked = true;
        endMenu.Alignment.AlignWidth = AlignWidth.None;
        endMenu.Alignment.AlignHeight = AlignHeight.None;
        endMenu.Alignment.HeightPercent = 0;
        endMenu.Alignment.WidthPercent = 0;
        endMenu.Alignment.LeftOffsetPercent = 0;
        endMenu.Alignment.RightOffsetPercent = 0;
        endMenu.Alignment.TopOffsetPercent = 0;
        endMenu.Alignment.BottomOffsetPercent = 0;
        endMenu.Alignment.LeftOffset = 0;
        endMenu.Alignment.RightOffset = 0;
        endMenu.Alignment.TopOffset = 0;
        endMenu.Alignment.BottomOffset = 0;
        endMenu.UseVideoMemory = false;
        endMenu.BorderWidth = 10;
        endMenu.BorderHeight = 5;
        endMenu.OverlapTopCorners = false;
        endMenu.Cursor = 0;
        endMenu.DoubleBuffered = true;
        endMenu.MaskColor = 128;
        endMenu.FreeSurfaceOnHide = true;
        endMenu.ImageLibraryIndexFrame = 0;
        endMenu.ImageLibraryIndexTNormal = 2;
        endMenu.ImageLibraryIndexTSelected = 9;
        endMenu.LayoutItems = new LayoutStrings()
        {
            "Surrender",
            "Exit to Title",
            "Exit to Windows"
        };
        endMenu.TextColor = 0;
        endMenu.TextSelectedColor = 0;
        endMenu.ItemHeight = 20;
        endMenu.OwnerItem = 2;
        endMenu.SubMenuShowDelay = 0;
        endMenu.OnSelect += EndMenuSelect;
        endMenu.WinHeight = 92;
        session.EndRead(endMenu);
        Menu diskMenu = new Menu(this);
        session.BeginRead(diskMenu);
        diskMenu.Name = "DiskMenu";
        diskMenu.Visible = false;
        diskMenu.Priority = 310;
        diskMenu.WinLeft = 0;
        diskMenu.WinTop = 0;
        diskMenu.WinWidth = 200;
        diskMenu.Masked = true;
        diskMenu.Alignment.AlignWidth = AlignWidth.None;
        diskMenu.Alignment.AlignHeight = AlignHeight.None;
        diskMenu.Alignment.HeightPercent = 0;
        diskMenu.Alignment.WidthPercent = 0;
        diskMenu.Alignment.LeftOffsetPercent = 0;
        diskMenu.Alignment.RightOffsetPercent = 0;
        diskMenu.Alignment.TopOffsetPercent = 0;
        diskMenu.Alignment.BottomOffsetPercent = 0;
        diskMenu.Alignment.LeftOffset = 0;
        diskMenu.Alignment.RightOffset = 0;
        diskMenu.Alignment.TopOffset = 0;
        diskMenu.Alignment.BottomOffset = 0;
        diskMenu.UseVideoMemory = false;
        diskMenu.BorderWidth = 10;
        diskMenu.BorderHeight = 5;
        diskMenu.OverlapTopCorners = false;
        diskMenu.Cursor = 0;
        diskMenu.DoubleBuffered = true;
        diskMenu.MaskColor = 128;
        diskMenu.FreeSurfaceOnHide = true;
        diskMenu.ImageLibraryIndexFrame = 0;
        diskMenu.ImageLibraryIndexTNormal = 2;
        diskMenu.ImageLibraryIndexTSelected = 9;
        diskMenu.LayoutItems = new LayoutStrings()
        {
            "Load Game",
            "Save Game"
        };
        diskMenu.TextColor = 0;
        diskMenu.TextSelectedColor = 0;
        diskMenu.ItemHeight = 20;
        diskMenu.OwnerItem = 3;
        diskMenu.SubMenuShowDelay = 0;
        diskMenu.OnSelect += DiskMenuSelect;
        diskMenu.WinHeight = 0;
        session.EndRead(diskMenu);
        StartMenu tcStartMenu = new StartMenu(this);
        session.BeginRead(tcStartMenu);
        tcStartMenu.Name = "TCStartMenu";
        tcStartMenu.Visible = false;
        tcStartMenu.Priority = 305;
        tcStartMenu.WinLeft = 0;
        tcStartMenu.WinTop = 1000;
        tcStartMenu.WinWidth = 250;
        tcStartMenu.Masked = true;
        tcStartMenu.Alignment.AlignWidth = AlignWidth.None;
        tcStartMenu.Alignment.AlignHeight = AlignHeight.None;
        tcStartMenu.Alignment.HeightPercent = 0;
        tcStartMenu.Alignment.WidthPercent = 0;
        tcStartMenu.Alignment.LeftOffsetPercent = 0;
        tcStartMenu.Alignment.RightOffsetPercent = 0;
        tcStartMenu.Alignment.TopOffsetPercent = 0;
        tcStartMenu.Alignment.BottomOffsetPercent = 0;
        tcStartMenu.Alignment.LeftOffset = 0;
        tcStartMenu.Alignment.RightOffset = 0;
        tcStartMenu.Alignment.TopOffset = 0;
        tcStartMenu.Alignment.BottomOffset = 0;
        tcStartMenu.UseVideoMemory = false;
        tcStartMenu.BorderWidth = 50;
        tcStartMenu.BorderHeight = 15;
        tcStartMenu.OverlapTopCorners = false;
        tcStartMenu.Cursor = 0;
        tcStartMenu.DoubleBuffered = true;
        tcStartMenu.MaskColor = 0;
        tcStartMenu.FreeSurfaceOnHide = true;
        tcStartMenu.ImageLibraryIndexFrame = 7;
        tcStartMenu.ImageLibraryIndexTNormal = 2;
        tcStartMenu.ImageLibraryIndexTSelected = 0;
        tcStartMenu.LayoutItems = new LayoutStrings()
        {
            "Overviews",
            "Settings",
            "End Combat",
            "Disk",
            "Help"
        };
        tcStartMenu.TextColor = 6291456;
        tcStartMenu.TextSelectedColor = 16728128;
        tcStartMenu.ItemHeight = 39;
        tcStartMenu.OwnerItem = 0;
        tcStartMenu.SubMenuShowDelay = 5;
        tcStartMenu.WinHeight = 0;
        session.EndRead(tcStartMenu);
        LayoutIvTranslator translator = new LayoutIvTranslator(this);
        session.BeginRead(translator);
        translator.Name = "Translator";
        translator.DictionaryName = "AoW";
        translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(translator);
        Menu helpMenu = new Menu(this);
        session.BeginRead(helpMenu);
        helpMenu.Name = "HelpMenu";
        helpMenu.Visible = false;
        helpMenu.Priority = 310;
        helpMenu.WinLeft = 0;
        helpMenu.WinTop = 0;
        helpMenu.WinWidth = 200;
        helpMenu.Masked = true;
        helpMenu.Alignment.AlignWidth = AlignWidth.None;
        helpMenu.Alignment.AlignHeight = AlignHeight.None;
        helpMenu.Alignment.HeightPercent = 0;
        helpMenu.Alignment.WidthPercent = 0;
        helpMenu.Alignment.LeftOffsetPercent = 0;
        helpMenu.Alignment.RightOffsetPercent = 0;
        helpMenu.Alignment.TopOffsetPercent = 0;
        helpMenu.Alignment.BottomOffsetPercent = 0;
        helpMenu.Alignment.LeftOffset = 0;
        helpMenu.Alignment.RightOffset = 0;
        helpMenu.Alignment.TopOffset = 0;
        helpMenu.Alignment.BottomOffset = 0;
        helpMenu.UseVideoMemory = false;
        helpMenu.BorderWidth = 10;
        helpMenu.BorderHeight = 5;
        helpMenu.OverlapTopCorners = false;
        helpMenu.Cursor = 0;
        helpMenu.DoubleBuffered = true;
        helpMenu.MaskColor = 128;
        helpMenu.FreeSurfaceOnHide = true;
        helpMenu.ImageLibraryIndexFrame = 0;
        helpMenu.ImageLibraryIndexTNormal = 2;
        helpMenu.ImageLibraryIndexTSelected = 9;
        helpMenu.LayoutItems = new LayoutStrings()
        {
            "General Interface",
            "Map Interface",
            "www.ageofwonders.com"
        };
        helpMenu.TextColor = 0;
        helpMenu.TextSelectedColor = 0;
        helpMenu.ItemHeight = 20;
        helpMenu.OwnerItem = 4;
        helpMenu.SubMenuShowDelay = 0;
        helpMenu.OnSelect += HelpMenuSelect;
        helpMenu.WinHeight = 0;
        session.EndRead(helpMenu);
        Menu tcovMenu = new Menu(this);
        session.BeginRead(tcovMenu);
        tcovMenu.Name = "TCOVMenu";
        tcovMenu.Visible = false;
        tcovMenu.Priority = 310;
        tcovMenu.WinLeft = 0;
        tcovMenu.WinTop = 0;
        tcovMenu.WinWidth = 200;
        tcovMenu.Masked = true;
        tcovMenu.Alignment.AlignWidth = AlignWidth.None;
        tcovMenu.Alignment.AlignHeight = AlignHeight.None;
        tcovMenu.Alignment.HeightPercent = 0;
        tcovMenu.Alignment.WidthPercent = 0;
        tcovMenu.Alignment.LeftOffsetPercent = 0;
        tcovMenu.Alignment.RightOffsetPercent = 0;
        tcovMenu.Alignment.TopOffsetPercent = 0;
        tcovMenu.Alignment.BottomOffsetPercent = 0;
        tcovMenu.Alignment.LeftOffset = 0;
        tcovMenu.Alignment.RightOffset = 0;
        tcovMenu.Alignment.TopOffset = 0;
        tcovMenu.Alignment.BottomOffset = 0;
        tcovMenu.UseVideoMemory = false;
        tcovMenu.BorderWidth = 10;
        tcovMenu.BorderHeight = 5;
        tcovMenu.OverlapTopCorners = false;
        tcovMenu.Cursor = 0;
        tcovMenu.DoubleBuffered = true;
        tcovMenu.MaskColor = 128;
        tcovMenu.FreeSurfaceOnHide = true;
        tcovMenu.ImageLibraryIndexFrame = 0;
        tcovMenu.ImageLibraryIndexTNormal = 2;
        tcovMenu.ImageLibraryIndexTSelected = 9;
        tcovMenu.LayoutItems = new LayoutStrings()
        {
            "Background"
        };
        tcovMenu.TextColor = 0;
        tcovMenu.TextSelectedColor = 0;
        tcovMenu.ItemHeight = 20;
        tcovMenu.OwnerItem = 0;
        tcovMenu.SubMenuShowDelay = 0;
        tcovMenu.OnSelect += OverViewsMenuSelect;
        tcovMenu.WinHeight = 0;
        session.EndRead(tcovMenu);
        Menu tcDiskMenu = new Menu(this);
        session.BeginRead(tcDiskMenu);
        tcDiskMenu.Name = "TCDiskMenu";
        tcDiskMenu.Visible = false;
        tcDiskMenu.Priority = 310;
        tcDiskMenu.WinLeft = 0;
        tcDiskMenu.WinTop = 0;
        tcDiskMenu.WinWidth = 200;
        tcDiskMenu.Masked = true;
        tcDiskMenu.Alignment.AlignWidth = AlignWidth.None;
        tcDiskMenu.Alignment.AlignHeight = AlignHeight.None;
        tcDiskMenu.Alignment.HeightPercent = 0;
        tcDiskMenu.Alignment.WidthPercent = 0;
        tcDiskMenu.Alignment.LeftOffsetPercent = 0;
        tcDiskMenu.Alignment.RightOffsetPercent = 0;
        tcDiskMenu.Alignment.TopOffsetPercent = 0;
        tcDiskMenu.Alignment.BottomOffsetPercent = 0;
        tcDiskMenu.Alignment.LeftOffset = 0;
        tcDiskMenu.Alignment.RightOffset = 0;
        tcDiskMenu.Alignment.TopOffset = 0;
        tcDiskMenu.Alignment.BottomOffset = 0;
        tcDiskMenu.UseVideoMemory = false;
        tcDiskMenu.BorderWidth = 10;
        tcDiskMenu.BorderHeight = 5;
        tcDiskMenu.OverlapTopCorners = false;
        tcDiskMenu.Cursor = 0;
        tcDiskMenu.DoubleBuffered = true;
        tcDiskMenu.MaskColor = 128;
        tcDiskMenu.FreeSurfaceOnHide = true;
        tcDiskMenu.ImageLibraryIndexFrame = 0;
        tcDiskMenu.ImageLibraryIndexTNormal = 2;
        tcDiskMenu.ImageLibraryIndexTSelected = 9;
        tcDiskMenu.LayoutItems = new LayoutStrings()
        {
            "Load Game"
        };
        tcDiskMenu.TextColor = 0;
        tcDiskMenu.TextSelectedColor = 0;
        tcDiskMenu.ItemHeight = 20;
        tcDiskMenu.OwnerItem = 3;
        tcDiskMenu.SubMenuShowDelay = 0;
        tcDiskMenu.OnSelect += DiskMenuSelect;
        tcDiskMenu.WinHeight = 0;
        session.EndRead(tcDiskMenu);
        Menu tcHelpMenu = new Menu(this);
        session.BeginRead(tcHelpMenu);
        tcHelpMenu.Name = "TCHelpMenu";
        tcHelpMenu.Visible = false;
        tcHelpMenu.Priority = 310;
        tcHelpMenu.WinLeft = 0;
        tcHelpMenu.WinTop = 0;
        tcHelpMenu.WinWidth = 200;
        tcHelpMenu.Masked = true;
        tcHelpMenu.Alignment.AlignWidth = AlignWidth.None;
        tcHelpMenu.Alignment.AlignHeight = AlignHeight.None;
        tcHelpMenu.Alignment.HeightPercent = 0;
        tcHelpMenu.Alignment.WidthPercent = 0;
        tcHelpMenu.Alignment.LeftOffsetPercent = 0;
        tcHelpMenu.Alignment.RightOffsetPercent = 0;
        tcHelpMenu.Alignment.TopOffsetPercent = 0;
        tcHelpMenu.Alignment.BottomOffsetPercent = 0;
        tcHelpMenu.Alignment.LeftOffset = 0;
        tcHelpMenu.Alignment.RightOffset = 0;
        tcHelpMenu.Alignment.TopOffset = 0;
        tcHelpMenu.Alignment.BottomOffset = 0;
        tcHelpMenu.UseVideoMemory = false;
        tcHelpMenu.BorderWidth = 10;
        tcHelpMenu.BorderHeight = 5;
        tcHelpMenu.OverlapTopCorners = false;
        tcHelpMenu.Cursor = 0;
        tcHelpMenu.DoubleBuffered = true;
        tcHelpMenu.MaskColor = 128;
        tcHelpMenu.FreeSurfaceOnHide = true;
        tcHelpMenu.ImageLibraryIndexFrame = 0;
        tcHelpMenu.ImageLibraryIndexTNormal = 2;
        tcHelpMenu.ImageLibraryIndexTSelected = 9;
        tcHelpMenu.LayoutItems = new LayoutStrings()
        {
            "General Interface",
            "Map Interface",
            "www.ageofwonders.com"
        };
        tcHelpMenu.TextColor = 0;
        tcHelpMenu.TextSelectedColor = 0;
        tcHelpMenu.ItemHeight = 20;
        tcHelpMenu.OwnerItem = 4;
        tcHelpMenu.SubMenuShowDelay = 0;
        tcHelpMenu.OnSelect += HelpMenuSelect;
        tcHelpMenu.WinHeight = 0;
        session.EndRead(tcHelpMenu);
        Menu tcSettingsMenu = new Menu(this);
        session.BeginRead(tcSettingsMenu);
        tcSettingsMenu.Name = "TCSettingsMenu";
        tcSettingsMenu.Visible = false;
        tcSettingsMenu.Priority = 310;
        tcSettingsMenu.WinLeft = 0;
        tcSettingsMenu.WinTop = 0;
        tcSettingsMenu.WinWidth = 200;
        tcSettingsMenu.Masked = true;
        tcSettingsMenu.Alignment.AlignWidth = AlignWidth.None;
        tcSettingsMenu.Alignment.AlignHeight = AlignHeight.None;
        tcSettingsMenu.Alignment.HeightPercent = 0;
        tcSettingsMenu.Alignment.WidthPercent = 0;
        tcSettingsMenu.Alignment.LeftOffsetPercent = 0;
        tcSettingsMenu.Alignment.RightOffsetPercent = 0;
        tcSettingsMenu.Alignment.TopOffsetPercent = 0;
        tcSettingsMenu.Alignment.BottomOffsetPercent = 0;
        tcSettingsMenu.Alignment.LeftOffset = 0;
        tcSettingsMenu.Alignment.RightOffset = 0;
        tcSettingsMenu.Alignment.TopOffset = 0;
        tcSettingsMenu.Alignment.BottomOffset = 0;
        tcSettingsMenu.UseVideoMemory = false;
        tcSettingsMenu.BorderWidth = 10;
        tcSettingsMenu.BorderHeight = 5;
        tcSettingsMenu.OverlapTopCorners = false;
        tcSettingsMenu.Cursor = 0;
        tcSettingsMenu.DoubleBuffered = true;
        tcSettingsMenu.MaskColor = 128;
        tcSettingsMenu.FreeSurfaceOnHide = true;
        tcSettingsMenu.ImageLibraryIndexFrame = 0;
        tcSettingsMenu.ImageLibraryIndexTNormal = 2;
        tcSettingsMenu.ImageLibraryIndexTSelected = 9;
        tcSettingsMenu.LayoutItems = new LayoutStrings()
        {
            "Display",
            "Sound",
            "Gameplay",
            "Reset Layout",
            "Email"
        };
        tcSettingsMenu.TextColor = 0;
        tcSettingsMenu.TextSelectedColor = 0;
        tcSettingsMenu.ItemHeight = 20;
        tcSettingsMenu.OwnerItem = 1;
        tcSettingsMenu.SubMenuShowDelay = 0;
        tcSettingsMenu.OnSelect += SettingsMenuSelect;
        tcSettingsMenu.WinHeight = 0;
        session.EndRead(tcSettingsMenu);
        Menu tcEndMenu = new Menu(this);
        session.BeginRead(tcEndMenu);
        tcEndMenu.Name = "TCEndMenu";
        tcEndMenu.Visible = false;
        tcEndMenu.Priority = 310;
        tcEndMenu.WinLeft = 0;
        tcEndMenu.WinTop = 0;
        tcEndMenu.WinWidth = 200;
        tcEndMenu.Masked = true;
        tcEndMenu.Alignment.AlignWidth = AlignWidth.None;
        tcEndMenu.Alignment.AlignHeight = AlignHeight.None;
        tcEndMenu.Alignment.HeightPercent = 0;
        tcEndMenu.Alignment.WidthPercent = 0;
        tcEndMenu.Alignment.LeftOffsetPercent = 0;
        tcEndMenu.Alignment.RightOffsetPercent = 0;
        tcEndMenu.Alignment.TopOffsetPercent = 0;
        tcEndMenu.Alignment.BottomOffsetPercent = 0;
        tcEndMenu.Alignment.LeftOffset = 0;
        tcEndMenu.Alignment.RightOffset = 0;
        tcEndMenu.Alignment.TopOffset = 0;
        tcEndMenu.Alignment.BottomOffset = 0;
        tcEndMenu.UseVideoMemory = false;
        tcEndMenu.BorderWidth = 10;
        tcEndMenu.BorderHeight = 5;
        tcEndMenu.OverlapTopCorners = false;
        tcEndMenu.Cursor = 0;
        tcEndMenu.DoubleBuffered = true;
        tcEndMenu.MaskColor = 128;
        tcEndMenu.FreeSurfaceOnHide = true;
        tcEndMenu.ImageLibraryIndexFrame = 0;
        tcEndMenu.ImageLibraryIndexTNormal = 2;
        tcEndMenu.ImageLibraryIndexTSelected = 9;
        tcEndMenu.LayoutItems = new LayoutStrings()
        {
            "Surrender"
        };
        tcEndMenu.TextColor = 0;
        tcEndMenu.TextSelectedColor = 0;
        tcEndMenu.ItemHeight = 20;
        tcEndMenu.OwnerItem = 2;
        tcEndMenu.SubMenuShowDelay = 0;
        tcEndMenu.OnSelect += TCEndMenuSelect;
        tcEndMenu.WinHeight = 0;
        session.EndRead(tcEndMenu);
        session.EndRead(this);
        OverviewsMenu.OwnerMenu = startMenu;
        settingsMenu.OwnerMenu = startMenu;
        endMenu.OwnerMenu = startMenu;
        diskMenu.OwnerMenu = startMenu;
        helpMenu.OwnerMenu = startMenu;
        tcovMenu.OwnerMenu = tcStartMenu;
        tcDiskMenu.OwnerMenu = tcStartMenu;
        tcHelpMenu.OwnerMenu = tcStartMenu;
        tcSettingsMenu.OwnerMenu = tcStartMenu;
        tcEndMenu.OwnerMenu = tcStartMenu;
        Loaded();
        startMenu.Loaded();
        OverviewsMenu.Loaded();
        settingsMenu.Loaded();
        endMenu.Loaded();
        diskMenu.Loaded();
        tcStartMenu.Loaded();
        translator.Loaded();
        helpMenu.Loaded();
        tcovMenu.Loaded();
        tcDiskMenu.Loaded();
        tcHelpMenu.Loaded();
        tcSettingsMenu.Loaded();
        tcEndMenu.Loaded();
        session.Resolve(this);
        startMenu.WindowManager = DialogHosts.General.Manager;
        startMenu.ImageLibraryFrame = DialogHosts.UIGraphics.DragonMenu;
        startMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        startMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.GenericTextures;
        startMenu.Font = DialogHosts.UIFonts.Age10Bold;
        session.Defer("TBWindow", (TaskBar module) => startMenu.OwnerMenuBar = module.WorldStartMenuBar);
        startMenu.ImageLibraryWholeMenu = DialogHosts.UIGraphics.DragonMenu;
        OverviewsMenu.WindowManager = DialogHosts.General.Manager;
        OverviewsMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        OverviewsMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        OverviewsMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        OverviewsMenu.Font = DialogHosts.UIFonts.Age10;
        settingsMenu.WindowManager = DialogHosts.General.Manager;
        settingsMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        settingsMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        settingsMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        settingsMenu.Font = DialogHosts.UIFonts.Age10;
        endMenu.WindowManager = DialogHosts.General.Manager;
        endMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        endMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        endMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        endMenu.Font = DialogHosts.UIFonts.Age10;
        diskMenu.WindowManager = DialogHosts.General.Manager;
        diskMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        diskMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        diskMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        diskMenu.Font = DialogHosts.UIFonts.Age10;
        tcStartMenu.WindowManager = DialogHosts.General.Manager;
        tcStartMenu.ImageLibraryFrame = DialogHosts.UIGraphics.DragonMenu;
        tcStartMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        tcStartMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.GenericTextures;
        tcStartMenu.Font = DialogHosts.UIFonts.Age10Bold;
        session.Defer("TBWindow", (TaskBar module) => tcStartMenu.OwnerMenuBar = module.CombatStartMenuBar);
        tcStartMenu.ImageLibraryWholeMenu = DialogHosts.UIGraphics.DragonMenu;
        helpMenu.WindowManager = DialogHosts.General.Manager;
        helpMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        helpMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        helpMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        helpMenu.Font = DialogHosts.UIFonts.Age10;
        tcovMenu.WindowManager = DialogHosts.General.Manager;
        tcovMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        tcovMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        tcovMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        tcovMenu.Font = DialogHosts.UIFonts.Age10;
        tcDiskMenu.WindowManager = DialogHosts.General.Manager;
        tcDiskMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        tcDiskMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        tcDiskMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        tcDiskMenu.Font = DialogHosts.UIFonts.Age10;
        tcHelpMenu.WindowManager = DialogHosts.General.Manager;
        tcHelpMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        tcHelpMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        tcHelpMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        tcHelpMenu.Font = DialogHosts.UIFonts.Age10;
        tcSettingsMenu.WindowManager = DialogHosts.General.Manager;
        tcSettingsMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        tcSettingsMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        tcSettingsMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        tcSettingsMenu.Font = DialogHosts.UIFonts.Age10;
        tcEndMenu.WindowManager = DialogHosts.General.Manager;
        tcEndMenu.ImageLibraryFrame = DialogHosts.UIGraphics.Menus;
        tcEndMenu.ImageLibraryTextureNormal = DialogHosts.UIGraphics.GenericTextures;
        tcEndMenu.ImageLibraryTextureSelected = DialogHosts.UIGraphics.Menus;
        tcEndMenu.Font = DialogHosts.UIFonts.Age10;
    }

}
