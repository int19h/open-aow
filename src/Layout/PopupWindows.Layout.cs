// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Layout;
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWExeForms;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AOWMenu;
using Aow.Units.AoWTBWindow;
using Aow.Units.AoWWinMan;
using Aow.Units.StartMenu;

namespace Aow.Units.AoWPWindows;

public partial class TPopupWindows : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "PopupWindows";
        StartMenu = new TStartMenu(this);
        session.BeginRead(StartMenu);
        StartMenu.Name = "StartMenu";
        StartMenu.Visible = false;
        StartMenu.Priority = 305;
        StartMenu.WinLeft = 0;
        StartMenu.WinTop = 12900;
        StartMenu.WinWidth = 250;
        StartMenu.Masked = true;
        StartMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        StartMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        StartMenu.Alignment.HeightPercent = 0;
        StartMenu.Alignment.WidthPercent = 0;
        StartMenu.Alignment.LeftOffPercent = 0;
        StartMenu.Alignment.RightOffPercent = 0;
        StartMenu.Alignment.TopOffPercent = 0;
        StartMenu.Alignment.BottomOffPercent = 0;
        StartMenu.Alignment.LeftOffset = 0;
        StartMenu.Alignment.RightOffset = 0;
        StartMenu.Alignment.TopOffset = 0;
        StartMenu.Alignment.BottomOffset = 0;
        StartMenu.UseVMem = false;
        StartMenu.BorderWidth = 50;
        StartMenu.BorderHeight = 15;
        StartMenu.OverlapTopCorners = false;
        StartMenu.Cursor = 0;
        StartMenu.DoubleBuffered = true;
        StartMenu.MaskColor = 0;
        StartMenu.FreeSurfaceOnHide = true;
        StartMenu.ILIndexFrame = 7;
        StartMenu.ILIndexTNormal = 2;
        StartMenu.ILIndexTSelected = 0;
        StartMenu.LayoutItems = new LayoutStrings()
        {
            "Overviews",
            "Settings",
            "End Game",
            "Disk",
            "Help"
        };
        StartMenu.TextColor = 6291456;
        StartMenu.TextSelColor = 16728128;
        StartMenu.ItemHeight = 39;
        StartMenu.OwnerItem = 0;
        StartMenu.SubMenuShowDelay = 5;
        StartMenu.WinHeight = 0;
        session.EndRead(StartMenu);
        OverViewsMenu = new TAOWMenu(this);
        session.BeginRead(OverViewsMenu);
        OverViewsMenu.Name = "OverViewsMenu";
        OverViewsMenu.Visible = false;
        OverViewsMenu.Priority = 310;
        OverViewsMenu.WinLeft = 0;
        OverViewsMenu.WinTop = 0;
        OverViewsMenu.WinWidth = 200;
        OverViewsMenu.Masked = true;
        OverViewsMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        OverViewsMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        OverViewsMenu.Alignment.HeightPercent = 0;
        OverViewsMenu.Alignment.WidthPercent = 0;
        OverViewsMenu.Alignment.LeftOffPercent = 0;
        OverViewsMenu.Alignment.RightOffPercent = 0;
        OverViewsMenu.Alignment.TopOffPercent = 0;
        OverViewsMenu.Alignment.BottomOffPercent = 0;
        OverViewsMenu.Alignment.LeftOffset = 0;
        OverViewsMenu.Alignment.RightOffset = 0;
        OverViewsMenu.Alignment.TopOffset = 0;
        OverViewsMenu.Alignment.BottomOffset = 0;
        OverViewsMenu.UseVMem = false;
        OverViewsMenu.BorderWidth = 10;
        OverViewsMenu.BorderHeight = 5;
        OverViewsMenu.OverlapTopCorners = false;
        OverViewsMenu.Cursor = 0;
        OverViewsMenu.DoubleBuffered = true;
        OverViewsMenu.MaskColor = 128;
        OverViewsMenu.FreeSurfaceOnHide = true;
        OverViewsMenu.ILIndexFrame = 0;
        OverViewsMenu.ILIndexTNormal = 2;
        OverViewsMenu.ILIndexTSelected = 9;
        OverViewsMenu.LayoutItems = new LayoutStrings()
        {
            "Background"
        };
        OverViewsMenu.TextColor = 0;
        OverViewsMenu.TextSelColor = 0;
        OverViewsMenu.ItemHeight = 20;
        OverViewsMenu.OwnerItem = 0;
        OverViewsMenu.SubMenuShowDelay = 0;
        OverViewsMenu.OnSelect = OverViewsMenuSelect;
        OverViewsMenu.WinHeight = 0;
        session.EndRead(OverViewsMenu);
        SettingsMenu = new TAOWMenu(this);
        session.BeginRead(SettingsMenu);
        SettingsMenu.Name = "SettingsMenu";
        SettingsMenu.Visible = false;
        SettingsMenu.Priority = 310;
        SettingsMenu.WinLeft = 0;
        SettingsMenu.WinTop = 0;
        SettingsMenu.WinWidth = 200;
        SettingsMenu.Masked = true;
        SettingsMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        SettingsMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        SettingsMenu.Alignment.HeightPercent = 0;
        SettingsMenu.Alignment.WidthPercent = 0;
        SettingsMenu.Alignment.LeftOffPercent = 0;
        SettingsMenu.Alignment.RightOffPercent = 0;
        SettingsMenu.Alignment.TopOffPercent = 0;
        SettingsMenu.Alignment.BottomOffPercent = 0;
        SettingsMenu.Alignment.LeftOffset = 0;
        SettingsMenu.Alignment.RightOffset = 0;
        SettingsMenu.Alignment.TopOffset = 0;
        SettingsMenu.Alignment.BottomOffset = 0;
        SettingsMenu.UseVMem = false;
        SettingsMenu.BorderWidth = 10;
        SettingsMenu.BorderHeight = 5;
        SettingsMenu.OverlapTopCorners = false;
        SettingsMenu.Cursor = 0;
        SettingsMenu.DoubleBuffered = true;
        SettingsMenu.MaskColor = 128;
        SettingsMenu.FreeSurfaceOnHide = true;
        SettingsMenu.ILIndexFrame = 0;
        SettingsMenu.ILIndexTNormal = 2;
        SettingsMenu.ILIndexTSelected = 9;
        SettingsMenu.LayoutItems = new LayoutStrings()
        {
            "Display",
            "Sound",
            "Gameplay",
            "Reset Layout",
            "Email"
        };
        SettingsMenu.TextColor = 0;
        SettingsMenu.TextSelColor = 0;
        SettingsMenu.ItemHeight = 20;
        SettingsMenu.OwnerItem = 1;
        SettingsMenu.SubMenuShowDelay = 0;
        SettingsMenu.OnSelect = SettingsMenuSelect;
        SettingsMenu.WinHeight = 112;
        session.EndRead(SettingsMenu);
        EndMenu = new TAOWMenu(this);
        session.BeginRead(EndMenu);
        EndMenu.Name = "EndMenu";
        EndMenu.Visible = false;
        EndMenu.Priority = 310;
        EndMenu.WinLeft = 0;
        EndMenu.WinTop = 0;
        EndMenu.WinWidth = 200;
        EndMenu.Masked = true;
        EndMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        EndMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        EndMenu.Alignment.HeightPercent = 0;
        EndMenu.Alignment.WidthPercent = 0;
        EndMenu.Alignment.LeftOffPercent = 0;
        EndMenu.Alignment.RightOffPercent = 0;
        EndMenu.Alignment.TopOffPercent = 0;
        EndMenu.Alignment.BottomOffPercent = 0;
        EndMenu.Alignment.LeftOffset = 0;
        EndMenu.Alignment.RightOffset = 0;
        EndMenu.Alignment.TopOffset = 0;
        EndMenu.Alignment.BottomOffset = 0;
        EndMenu.UseVMem = false;
        EndMenu.BorderWidth = 10;
        EndMenu.BorderHeight = 5;
        EndMenu.OverlapTopCorners = false;
        EndMenu.Cursor = 0;
        EndMenu.DoubleBuffered = true;
        EndMenu.MaskColor = 128;
        EndMenu.FreeSurfaceOnHide = true;
        EndMenu.ILIndexFrame = 0;
        EndMenu.ILIndexTNormal = 2;
        EndMenu.ILIndexTSelected = 9;
        EndMenu.LayoutItems = new LayoutStrings()
        {
            "Surrender",
            "Exit to Title",
            "Exit to Windows"
        };
        EndMenu.TextColor = 0;
        EndMenu.TextSelColor = 0;
        EndMenu.ItemHeight = 20;
        EndMenu.OwnerItem = 2;
        EndMenu.SubMenuShowDelay = 0;
        EndMenu.OnSelect = EndMenuSelect;
        EndMenu.WinHeight = 92;
        session.EndRead(EndMenu);
        DiskMenu = new TAOWMenu(this);
        session.BeginRead(DiskMenu);
        DiskMenu.Name = "DiskMenu";
        DiskMenu.Visible = false;
        DiskMenu.Priority = 310;
        DiskMenu.WinLeft = 0;
        DiskMenu.WinTop = 0;
        DiskMenu.WinWidth = 200;
        DiskMenu.Masked = true;
        DiskMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        DiskMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        DiskMenu.Alignment.HeightPercent = 0;
        DiskMenu.Alignment.WidthPercent = 0;
        DiskMenu.Alignment.LeftOffPercent = 0;
        DiskMenu.Alignment.RightOffPercent = 0;
        DiskMenu.Alignment.TopOffPercent = 0;
        DiskMenu.Alignment.BottomOffPercent = 0;
        DiskMenu.Alignment.LeftOffset = 0;
        DiskMenu.Alignment.RightOffset = 0;
        DiskMenu.Alignment.TopOffset = 0;
        DiskMenu.Alignment.BottomOffset = 0;
        DiskMenu.UseVMem = false;
        DiskMenu.BorderWidth = 10;
        DiskMenu.BorderHeight = 5;
        DiskMenu.OverlapTopCorners = false;
        DiskMenu.Cursor = 0;
        DiskMenu.DoubleBuffered = true;
        DiskMenu.MaskColor = 128;
        DiskMenu.FreeSurfaceOnHide = true;
        DiskMenu.ILIndexFrame = 0;
        DiskMenu.ILIndexTNormal = 2;
        DiskMenu.ILIndexTSelected = 9;
        DiskMenu.LayoutItems = new LayoutStrings()
        {
            "Load Game",
            "Save Game"
        };
        DiskMenu.TextColor = 0;
        DiskMenu.TextSelColor = 0;
        DiskMenu.ItemHeight = 20;
        DiskMenu.OwnerItem = 3;
        DiskMenu.SubMenuShowDelay = 0;
        DiskMenu.OnSelect = DiskMenuSelect;
        DiskMenu.WinHeight = 0;
        session.EndRead(DiskMenu);
        TCStartMenu = new TStartMenu(this);
        session.BeginRead(TCStartMenu);
        TCStartMenu.Name = "TCStartMenu";
        TCStartMenu.Visible = false;
        TCStartMenu.Priority = 305;
        TCStartMenu.WinLeft = 0;
        TCStartMenu.WinTop = 1000;
        TCStartMenu.WinWidth = 250;
        TCStartMenu.Masked = true;
        TCStartMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        TCStartMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        TCStartMenu.Alignment.HeightPercent = 0;
        TCStartMenu.Alignment.WidthPercent = 0;
        TCStartMenu.Alignment.LeftOffPercent = 0;
        TCStartMenu.Alignment.RightOffPercent = 0;
        TCStartMenu.Alignment.TopOffPercent = 0;
        TCStartMenu.Alignment.BottomOffPercent = 0;
        TCStartMenu.Alignment.LeftOffset = 0;
        TCStartMenu.Alignment.RightOffset = 0;
        TCStartMenu.Alignment.TopOffset = 0;
        TCStartMenu.Alignment.BottomOffset = 0;
        TCStartMenu.UseVMem = false;
        TCStartMenu.BorderWidth = 50;
        TCStartMenu.BorderHeight = 15;
        TCStartMenu.OverlapTopCorners = false;
        TCStartMenu.Cursor = 0;
        TCStartMenu.DoubleBuffered = true;
        TCStartMenu.MaskColor = 0;
        TCStartMenu.FreeSurfaceOnHide = true;
        TCStartMenu.ILIndexFrame = 7;
        TCStartMenu.ILIndexTNormal = 2;
        TCStartMenu.ILIndexTSelected = 0;
        TCStartMenu.LayoutItems = new LayoutStrings()
        {
            "Overviews",
            "Settings",
            "End Combat",
            "Disk",
            "Help"
        };
        TCStartMenu.TextColor = 6291456;
        TCStartMenu.TextSelColor = 16728128;
        TCStartMenu.ItemHeight = 39;
        TCStartMenu.OwnerItem = 0;
        TCStartMenu.SubMenuShowDelay = 5;
        TCStartMenu.WinHeight = 0;
        session.EndRead(TCStartMenu);
        var created__Translator = new LayoutTIvTranslator(this);
        Translator = created__Translator;
        session.BeginRead(created__Translator);
        created__Translator.Name = "Translator";
        created__Translator.DictionaryName = "AoW";
        created__Translator.TargetsData.Add(new TranslationTarget("", "Items", 0));
        created__Translator.TargetsData.Add(new TranslationTarget("", "Text", 0));
        created__Translator.TargetsData.Add(new TranslationTarget("", "ToolTipText", 0));
        session.EndRead(created__Translator);
        HelpMenu = new TAOWMenu(this);
        session.BeginRead(HelpMenu);
        HelpMenu.Name = "HelpMenu";
        HelpMenu.Visible = false;
        HelpMenu.Priority = 310;
        HelpMenu.WinLeft = 0;
        HelpMenu.WinTop = 0;
        HelpMenu.WinWidth = 200;
        HelpMenu.Masked = true;
        HelpMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        HelpMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        HelpMenu.Alignment.HeightPercent = 0;
        HelpMenu.Alignment.WidthPercent = 0;
        HelpMenu.Alignment.LeftOffPercent = 0;
        HelpMenu.Alignment.RightOffPercent = 0;
        HelpMenu.Alignment.TopOffPercent = 0;
        HelpMenu.Alignment.BottomOffPercent = 0;
        HelpMenu.Alignment.LeftOffset = 0;
        HelpMenu.Alignment.RightOffset = 0;
        HelpMenu.Alignment.TopOffset = 0;
        HelpMenu.Alignment.BottomOffset = 0;
        HelpMenu.UseVMem = false;
        HelpMenu.BorderWidth = 10;
        HelpMenu.BorderHeight = 5;
        HelpMenu.OverlapTopCorners = false;
        HelpMenu.Cursor = 0;
        HelpMenu.DoubleBuffered = true;
        HelpMenu.MaskColor = 128;
        HelpMenu.FreeSurfaceOnHide = true;
        HelpMenu.ILIndexFrame = 0;
        HelpMenu.ILIndexTNormal = 2;
        HelpMenu.ILIndexTSelected = 9;
        HelpMenu.LayoutItems = new LayoutStrings()
        {
            "General Interface",
            "Map Interface",
            "www.ageofwonders.com"
        };
        HelpMenu.TextColor = 0;
        HelpMenu.TextSelColor = 0;
        HelpMenu.ItemHeight = 20;
        HelpMenu.OwnerItem = 4;
        HelpMenu.SubMenuShowDelay = 0;
        HelpMenu.OnSelect = HelpMenuSelect;
        HelpMenu.WinHeight = 0;
        session.EndRead(HelpMenu);
        TCOVMenu = new TAOWMenu(this);
        session.BeginRead(TCOVMenu);
        TCOVMenu.Name = "TCOVMenu";
        TCOVMenu.Visible = false;
        TCOVMenu.Priority = 310;
        TCOVMenu.WinLeft = 0;
        TCOVMenu.WinTop = 0;
        TCOVMenu.WinWidth = 200;
        TCOVMenu.Masked = true;
        TCOVMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        TCOVMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        TCOVMenu.Alignment.HeightPercent = 0;
        TCOVMenu.Alignment.WidthPercent = 0;
        TCOVMenu.Alignment.LeftOffPercent = 0;
        TCOVMenu.Alignment.RightOffPercent = 0;
        TCOVMenu.Alignment.TopOffPercent = 0;
        TCOVMenu.Alignment.BottomOffPercent = 0;
        TCOVMenu.Alignment.LeftOffset = 0;
        TCOVMenu.Alignment.RightOffset = 0;
        TCOVMenu.Alignment.TopOffset = 0;
        TCOVMenu.Alignment.BottomOffset = 0;
        TCOVMenu.UseVMem = false;
        TCOVMenu.BorderWidth = 10;
        TCOVMenu.BorderHeight = 5;
        TCOVMenu.OverlapTopCorners = false;
        TCOVMenu.Cursor = 0;
        TCOVMenu.DoubleBuffered = true;
        TCOVMenu.MaskColor = 128;
        TCOVMenu.FreeSurfaceOnHide = true;
        TCOVMenu.ILIndexFrame = 0;
        TCOVMenu.ILIndexTNormal = 2;
        TCOVMenu.ILIndexTSelected = 9;
        TCOVMenu.LayoutItems = new LayoutStrings()
        {
            "Background"
        };
        TCOVMenu.TextColor = 0;
        TCOVMenu.TextSelColor = 0;
        TCOVMenu.ItemHeight = 20;
        TCOVMenu.OwnerItem = 0;
        TCOVMenu.SubMenuShowDelay = 0;
        TCOVMenu.OnSelect = OverViewsMenuSelect;
        TCOVMenu.WinHeight = 0;
        session.EndRead(TCOVMenu);
        TCDiskMenu = new TAOWMenu(this);
        session.BeginRead(TCDiskMenu);
        TCDiskMenu.Name = "TCDiskMenu";
        TCDiskMenu.Visible = false;
        TCDiskMenu.Priority = 310;
        TCDiskMenu.WinLeft = 0;
        TCDiskMenu.WinTop = 0;
        TCDiskMenu.WinWidth = 200;
        TCDiskMenu.Masked = true;
        TCDiskMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        TCDiskMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        TCDiskMenu.Alignment.HeightPercent = 0;
        TCDiskMenu.Alignment.WidthPercent = 0;
        TCDiskMenu.Alignment.LeftOffPercent = 0;
        TCDiskMenu.Alignment.RightOffPercent = 0;
        TCDiskMenu.Alignment.TopOffPercent = 0;
        TCDiskMenu.Alignment.BottomOffPercent = 0;
        TCDiskMenu.Alignment.LeftOffset = 0;
        TCDiskMenu.Alignment.RightOffset = 0;
        TCDiskMenu.Alignment.TopOffset = 0;
        TCDiskMenu.Alignment.BottomOffset = 0;
        TCDiskMenu.UseVMem = false;
        TCDiskMenu.BorderWidth = 10;
        TCDiskMenu.BorderHeight = 5;
        TCDiskMenu.OverlapTopCorners = false;
        TCDiskMenu.Cursor = 0;
        TCDiskMenu.DoubleBuffered = true;
        TCDiskMenu.MaskColor = 128;
        TCDiskMenu.FreeSurfaceOnHide = true;
        TCDiskMenu.ILIndexFrame = 0;
        TCDiskMenu.ILIndexTNormal = 2;
        TCDiskMenu.ILIndexTSelected = 9;
        TCDiskMenu.LayoutItems = new LayoutStrings()
        {
            "Load Game"
        };
        TCDiskMenu.TextColor = 0;
        TCDiskMenu.TextSelColor = 0;
        TCDiskMenu.ItemHeight = 20;
        TCDiskMenu.OwnerItem = 3;
        TCDiskMenu.SubMenuShowDelay = 0;
        TCDiskMenu.OnSelect = DiskMenuSelect;
        TCDiskMenu.WinHeight = 0;
        session.EndRead(TCDiskMenu);
        TCHelpMenu = new TAOWMenu(this);
        session.BeginRead(TCHelpMenu);
        TCHelpMenu.Name = "TCHelpMenu";
        TCHelpMenu.Visible = false;
        TCHelpMenu.Priority = 310;
        TCHelpMenu.WinLeft = 0;
        TCHelpMenu.WinTop = 0;
        TCHelpMenu.WinWidth = 200;
        TCHelpMenu.Masked = true;
        TCHelpMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        TCHelpMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        TCHelpMenu.Alignment.HeightPercent = 0;
        TCHelpMenu.Alignment.WidthPercent = 0;
        TCHelpMenu.Alignment.LeftOffPercent = 0;
        TCHelpMenu.Alignment.RightOffPercent = 0;
        TCHelpMenu.Alignment.TopOffPercent = 0;
        TCHelpMenu.Alignment.BottomOffPercent = 0;
        TCHelpMenu.Alignment.LeftOffset = 0;
        TCHelpMenu.Alignment.RightOffset = 0;
        TCHelpMenu.Alignment.TopOffset = 0;
        TCHelpMenu.Alignment.BottomOffset = 0;
        TCHelpMenu.UseVMem = false;
        TCHelpMenu.BorderWidth = 10;
        TCHelpMenu.BorderHeight = 5;
        TCHelpMenu.OverlapTopCorners = false;
        TCHelpMenu.Cursor = 0;
        TCHelpMenu.DoubleBuffered = true;
        TCHelpMenu.MaskColor = 128;
        TCHelpMenu.FreeSurfaceOnHide = true;
        TCHelpMenu.ILIndexFrame = 0;
        TCHelpMenu.ILIndexTNormal = 2;
        TCHelpMenu.ILIndexTSelected = 9;
        TCHelpMenu.LayoutItems = new LayoutStrings()
        {
            "General Interface",
            "Map Interface",
            "www.ageofwonders.com"
        };
        TCHelpMenu.TextColor = 0;
        TCHelpMenu.TextSelColor = 0;
        TCHelpMenu.ItemHeight = 20;
        TCHelpMenu.OwnerItem = 4;
        TCHelpMenu.SubMenuShowDelay = 0;
        TCHelpMenu.OnSelect = HelpMenuSelect;
        TCHelpMenu.WinHeight = 0;
        session.EndRead(TCHelpMenu);
        TCSettingsMenu = new TAOWMenu(this);
        session.BeginRead(TCSettingsMenu);
        TCSettingsMenu.Name = "TCSettingsMenu";
        TCSettingsMenu.Visible = false;
        TCSettingsMenu.Priority = 310;
        TCSettingsMenu.WinLeft = 0;
        TCSettingsMenu.WinTop = 0;
        TCSettingsMenu.WinWidth = 200;
        TCSettingsMenu.Masked = true;
        TCSettingsMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        TCSettingsMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        TCSettingsMenu.Alignment.HeightPercent = 0;
        TCSettingsMenu.Alignment.WidthPercent = 0;
        TCSettingsMenu.Alignment.LeftOffPercent = 0;
        TCSettingsMenu.Alignment.RightOffPercent = 0;
        TCSettingsMenu.Alignment.TopOffPercent = 0;
        TCSettingsMenu.Alignment.BottomOffPercent = 0;
        TCSettingsMenu.Alignment.LeftOffset = 0;
        TCSettingsMenu.Alignment.RightOffset = 0;
        TCSettingsMenu.Alignment.TopOffset = 0;
        TCSettingsMenu.Alignment.BottomOffset = 0;
        TCSettingsMenu.UseVMem = false;
        TCSettingsMenu.BorderWidth = 10;
        TCSettingsMenu.BorderHeight = 5;
        TCSettingsMenu.OverlapTopCorners = false;
        TCSettingsMenu.Cursor = 0;
        TCSettingsMenu.DoubleBuffered = true;
        TCSettingsMenu.MaskColor = 128;
        TCSettingsMenu.FreeSurfaceOnHide = true;
        TCSettingsMenu.ILIndexFrame = 0;
        TCSettingsMenu.ILIndexTNormal = 2;
        TCSettingsMenu.ILIndexTSelected = 9;
        TCSettingsMenu.LayoutItems = new LayoutStrings()
        {
            "Display",
            "Sound",
            "Gameplay",
            "Reset Layout",
            "Email"
        };
        TCSettingsMenu.TextColor = 0;
        TCSettingsMenu.TextSelColor = 0;
        TCSettingsMenu.ItemHeight = 20;
        TCSettingsMenu.OwnerItem = 1;
        TCSettingsMenu.SubMenuShowDelay = 0;
        TCSettingsMenu.OnSelect = SettingsMenuSelect;
        TCSettingsMenu.WinHeight = 0;
        session.EndRead(TCSettingsMenu);
        TCEndMenu = new TAOWMenu(this);
        session.BeginRead(TCEndMenu);
        TCEndMenu.Name = "TCEndMenu";
        TCEndMenu.Visible = false;
        TCEndMenu.Priority = 310;
        TCEndMenu.WinLeft = 0;
        TCEndMenu.WinTop = 0;
        TCEndMenu.WinWidth = 200;
        TCEndMenu.Masked = true;
        TCEndMenu.Alignment.AlignWidth = TAOWAlignWidth.awNone;
        TCEndMenu.Alignment.AlignHeight = TAOWAlignHeight.ahNone;
        TCEndMenu.Alignment.HeightPercent = 0;
        TCEndMenu.Alignment.WidthPercent = 0;
        TCEndMenu.Alignment.LeftOffPercent = 0;
        TCEndMenu.Alignment.RightOffPercent = 0;
        TCEndMenu.Alignment.TopOffPercent = 0;
        TCEndMenu.Alignment.BottomOffPercent = 0;
        TCEndMenu.Alignment.LeftOffset = 0;
        TCEndMenu.Alignment.RightOffset = 0;
        TCEndMenu.Alignment.TopOffset = 0;
        TCEndMenu.Alignment.BottomOffset = 0;
        TCEndMenu.UseVMem = false;
        TCEndMenu.BorderWidth = 10;
        TCEndMenu.BorderHeight = 5;
        TCEndMenu.OverlapTopCorners = false;
        TCEndMenu.Cursor = 0;
        TCEndMenu.DoubleBuffered = true;
        TCEndMenu.MaskColor = 128;
        TCEndMenu.FreeSurfaceOnHide = true;
        TCEndMenu.ILIndexFrame = 0;
        TCEndMenu.ILIndexTNormal = 2;
        TCEndMenu.ILIndexTSelected = 9;
        TCEndMenu.LayoutItems = new LayoutStrings()
        {
            "Surrender"
        };
        TCEndMenu.TextColor = 0;
        TCEndMenu.TextSelColor = 0;
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
        StartMenu.WinManager = AoWWinManUnit.General.Manager;
        StartMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.DragonMenu;
        StartMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        StartMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.GenericT;
        StartMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10b;
        session.Defer("TBWindow", (TTBWindow module) => StartMenu.OwnerMenuBar = module.StartMenuBar);
        StartMenu.ILWholeMenu = aowIntGfxModUnit.IntGfxMod.DragonMenu;
        OverViewsMenu.WinManager = AoWWinManUnit.General.Manager;
        OverViewsMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        OverViewsMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        OverViewsMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        OverViewsMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        SettingsMenu.WinManager = AoWWinManUnit.General.Manager;
        SettingsMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        SettingsMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        SettingsMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        SettingsMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        EndMenu.WinManager = AoWWinManUnit.General.Manager;
        EndMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        EndMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        EndMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        EndMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        DiskMenu.WinManager = AoWWinManUnit.General.Manager;
        DiskMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        DiskMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        DiskMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        DiskMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        TCStartMenu.WinManager = AoWWinManUnit.General.Manager;
        TCStartMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.DragonMenu;
        TCStartMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        TCStartMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.GenericT;
        TCStartMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10b;
        session.Defer("TBWindow", (TTBWindow module) => TCStartMenu.OwnerMenuBar = module.TCStartBar);
        TCStartMenu.ILWholeMenu = aowIntGfxModUnit.IntGfxMod.DragonMenu;
        HelpMenu.WinManager = AoWWinManUnit.General.Manager;
        HelpMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        HelpMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        HelpMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        HelpMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        TCOVMenu.WinManager = AoWWinManUnit.General.Manager;
        TCOVMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        TCOVMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        TCOVMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        TCOVMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        TCDiskMenu.WinManager = AoWWinManUnit.General.Manager;
        TCDiskMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        TCDiskMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        TCDiskMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        TCDiskMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        TCHelpMenu.WinManager = AoWWinManUnit.General.Manager;
        TCHelpMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        TCHelpMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        TCHelpMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        TCHelpMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        TCSettingsMenu.WinManager = AoWWinManUnit.General.Manager;
        TCSettingsMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        TCSettingsMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        TCSettingsMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        TCSettingsMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
        TCEndMenu.WinManager = AoWWinManUnit.General.Manager;
        TCEndMenu.ILFrame = aowIntGfxModUnit.IntGfxMod.Menus;
        TCEndMenu.ILTextureNormal = aowIntGfxModUnit.IntGfxMod.GenericT;
        TCEndMenu.ILTextureSelected = aowIntGfxModUnit.IntGfxMod.Menus;
        TCEndMenu.AOWFont = AoWExeFormsUnit.FontModule.Age10;
    }

}
