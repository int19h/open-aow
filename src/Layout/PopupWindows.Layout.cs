// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWPWindows
{
    public partial class TPopupWindows
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "PopupWindows";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            StartMenu = new Aow.Units.StartMenu.TStartMenu(this)
            {
                Name = "StartMenu",
                Visible = false,
                Priority = 305,
                WinLeft = 0,
                WinTop = 12900,
                WinWidth = 250,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 50,
                BorderHeight = 15,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 0,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 7,
                ILIndexTNormal = 2,
                ILIndexTSelected = 0,
                LayoutItems = new LayoutStrings()
                {
                    "Overviews",
                    "Settings",
                    "End Game",
                    "Disk",
                    "Help"
                },
                TextColor = 6291456,
                TextSelColor = 16728128,
                ItemHeight = 39,
                OwnerItem = 0,
                SubMenuShowDelay = 5,
                WinHeight = 0
            };
            LayoutSession.EndRead(StartMenu);
            OverViewsMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "OverViewsMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "Background"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 0,
                SubMenuShowDelay = 0,
                OnSelect = Layout_OverViewsMenuSelect_0,
                WinHeight = 0
            };
            LayoutSession.EndRead(OverViewsMenu);
            SettingsMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "SettingsMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "Display",
                    "Sound",
                    "Gameplay",
                    "Reset Layout",
                    "Email"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 1,
                SubMenuShowDelay = 0,
                OnSelect = Layout_SettingsMenuSelect_1,
                WinHeight = 112
            };
            LayoutSession.EndRead(SettingsMenu);
            EndMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "EndMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "Surrender",
                    "Exit to Title",
                    "Exit to Windows"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 2,
                SubMenuShowDelay = 0,
                OnSelect = Layout_EndMenuSelect_2,
                WinHeight = 92
            };
            LayoutSession.EndRead(EndMenu);
            DiskMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "DiskMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "Load Game",
                    "Save Game"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 3,
                SubMenuShowDelay = 0,
                OnSelect = Layout_DiskMenuSelect_3,
                WinHeight = 0
            };
            LayoutSession.EndRead(DiskMenu);
            TCStartMenu = new Aow.Units.StartMenu.TStartMenu(this)
            {
                Name = "TCStartMenu",
                Visible = false,
                Priority = 305,
                WinLeft = 0,
                WinTop = 1000,
                WinWidth = 250,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 50,
                BorderHeight = 15,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 0,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 7,
                ILIndexTNormal = 2,
                ILIndexTSelected = 0,
                LayoutItems = new LayoutStrings()
                {
                    "Overviews",
                    "Settings",
                    "End Combat",
                    "Disk",
                    "Help"
                },
                TextColor = 6291456,
                TextSelColor = 16728128,
                ItemHeight = 39,
                OwnerItem = 0,
                SubMenuShowDelay = 5,
                WinHeight = 0
            };
            LayoutSession.EndRead(TCStartMenu);
            Translator = new Aow.Layout.LayoutTIvTranslator(this)
            {
                Name = "Translator",
                DictionaryName = "AoW",
                TargetsData =
                {
                    new TranslationTarget("", "Items", 0),
                    new TranslationTarget("", "Text", 0),
                    new TranslationTarget("", "ToolTipText", 0)
                }
            };
            LayoutSession.EndRead(Translator);
            HelpMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "HelpMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "General Interface",
                    "Map Interface",
                    "www.ageofwonders.com"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 4,
                SubMenuShowDelay = 0,
                OnSelect = Layout_HelpMenuSelect_4,
                WinHeight = 0
            };
            LayoutSession.EndRead(HelpMenu);
            TCOVMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "TCOVMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "Background"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 0,
                SubMenuShowDelay = 0,
                OnSelect = Layout_OverViewsMenuSelect_0,
                WinHeight = 0
            };
            LayoutSession.EndRead(TCOVMenu);
            TCDiskMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "TCDiskMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "Load Game"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 3,
                SubMenuShowDelay = 0,
                OnSelect = Layout_DiskMenuSelect_3,
                WinHeight = 0
            };
            LayoutSession.EndRead(TCDiskMenu);
            TCHelpMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "TCHelpMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "General Interface",
                    "Map Interface",
                    "www.ageofwonders.com"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 4,
                SubMenuShowDelay = 0,
                OnSelect = Layout_HelpMenuSelect_4,
                WinHeight = 0
            };
            LayoutSession.EndRead(TCHelpMenu);
            TCSettingsMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "TCSettingsMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "Display",
                    "Sound",
                    "Gameplay",
                    "Reset Layout",
                    "Email"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 1,
                SubMenuShowDelay = 0,
                OnSelect = Layout_SettingsMenuSelect_1,
                WinHeight = 0
            };
            LayoutSession.EndRead(TCSettingsMenu);
            TCEndMenu = new Aow.Units.AOWMenu.TAOWMenu(this)
            {
                Name = "TCEndMenu",
                Visible = false,
                Priority = 310,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 200,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
                BorderWidth = 10,
                BorderHeight = 5,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 128,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTNormal = 2,
                ILIndexTSelected = 9,
                LayoutItems = new LayoutStrings()
                {
                    "Surrender"
                },
                TextColor = 0,
                TextSelColor = 0,
                ItemHeight = 20,
                OwnerItem = 2,
                SubMenuShowDelay = 0,
                OnSelect = Layout_TCEndMenuSelect_5,
                WinHeight = 0
            };
            LayoutSession.EndRead(TCEndMenu);
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
            StartMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            StartMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.DragonMenu;
            StartMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            StartMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            StartMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            session.Defer("TBWindow", (Aow.Units.AoWTBWindow.TTBWindow module) => StartMenu.OwnerMenuBar = module.StartMenuBar);
            StartMenu.ILWholeMenu = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.DragonMenu;
            OverViewsMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            OverViewsMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            OverViewsMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            OverViewsMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            OverViewsMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SettingsMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            SettingsMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            SettingsMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SettingsMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            SettingsMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            EndMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            EndMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            EndMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EndMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            EndMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DiskMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            DiskMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            DiskMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DiskMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            DiskMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCStartMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCStartMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.DragonMenu;
            TCStartMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCStartMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCStartMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            session.Defer("TBWindow", (Aow.Units.AoWTBWindow.TTBWindow module) => TCStartMenu.OwnerMenuBar = module.TCStartBar);
            TCStartMenu.ILWholeMenu = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.DragonMenu;
            HelpMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            HelpMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            HelpMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            HelpMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            HelpMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCOVMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCOVMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCOVMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCOVMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCOVMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCDiskMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCDiskMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCDiskMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCDiskMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCDiskMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCHelpMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCHelpMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCHelpMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCHelpMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCHelpMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCSettingsMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCSettingsMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCSettingsMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCSettingsMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCSettingsMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCEndMenu.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCEndMenu.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCEndMenu.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCEndMenu.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            TCEndMenu.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
        }

        private void Layout_OverViewsMenuSelect_0(TObject p0)
        {
            OverViewsMenuSelect((Aow.Units.AOWMenu.TAOWMenu)p0);
        }

        private void Layout_SettingsMenuSelect_1(TObject p0)
        {
            SettingsMenuSelect((Aow.Units.AOWMenu.TAOWMenu)p0);
        }

        private void Layout_EndMenuSelect_2(TObject p0)
        {
            EndMenuSelect((Aow.Units.AOWMenu.TAOWMenu)p0);
        }

        private void Layout_DiskMenuSelect_3(TObject p0)
        {
            DiskMenuSelect((Aow.Units.AOWMenu.TAOWMenu)p0);
        }

        private void Layout_HelpMenuSelect_4(TObject p0)
        {
            HelpMenuSelect((Aow.Units.AOWMenu.TAOWMenu)p0);
        }

        private void Layout_TCEndMenuSelect_5(TObject p0)
        {
            TCEndMenuSelect((Aow.Units.AOWMenu.TAOWMenu)p0);
        }
    }
}
