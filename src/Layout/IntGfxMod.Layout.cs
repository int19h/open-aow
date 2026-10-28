// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.aowIntGfxMod
{
    public partial class TIntGfxMod
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "IntGfxMod";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            GenericT = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "GenericT",
                LoadOnDemand = true,
                FileName = "Int\\GenericT.ilb"
            };
            LayoutSession.EndRead(GenericT);
            GenericF = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "GenericF",
                LoadOnDemand = true,
                FileName = "Int\\GenericF.ILB"
            };
            LayoutSession.EndRead(GenericF);
            CeltDeco = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "CeltDeco",
                LoadOnDemand = true,
                FileName = "Int\\CeltDeco.ILB"
            };
            LayoutSession.EndRead(CeltDeco);
            Cursors = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "Cursors",
                LoadOnDemand = true,
                FileName = "IMAGES\\Cursors.ilb"
            };
            LayoutSession.EndRead(Cursors);
            ScanTiles = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "ScanTiles",
                LoadOnDemand = true,
                FileName = "Int\\ScanTile.ilb"
            };
            LayoutSession.EndRead(ScanTiles);
            ScanFog = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "ScanFog",
                LoadOnDemand = true,
                FileName = "Int\\ScanFog.ilb"
            };
            LayoutSession.EndRead(ScanFog);
            ScanHiLites = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "ScanHiLites",
                LoadOnDemand = true,
                FileName = "Int\\ScanHiLi.ilb"
            };
            LayoutSession.EndRead(ScanHiLites);
            PyreAnim = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "PyreAnim",
                LoadOnDemand = true,
                LayoutData = new byte[]
                {
                    4,
                    73,
                    76,
                    66,
                    0,
                    205,
                    93,
                    3,
                    0,
                    0,
                    128,
                    64,
                    24,
                    0,
                    0,
                    0,
                    32,
                    0,
                    0,
                    0,
                    32,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    255,
                    255,
                    255,
                    255
                }
            };
            LayoutSession.EndRead(PyreAnim);
            UnitBGs = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "UnitBGs",
                LoadOnDemand = true,
                FileName = "Int\\UnitBgs.ILB"
            };
            LayoutSession.EndRead(UnitBGs);
            FCWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "FCWin",
                LoadOnDemand = true,
                FileName = "Int\\FCWin.ILB"
            };
            LayoutSession.EndRead(FCWin);
            Menus = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "Menus",
                LoadOnDemand = true,
                FileName = "Int\\Menus.ILB"
            };
            LayoutSession.EndRead(Menus);
            DragonMenu = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "DragonMenu",
                LoadOnDemand = true,
                FileName = "Int\\DragMenu.ILB"
            };
            LayoutSession.EndRead(DragonMenu);
            ScanWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "ScanWin",
                LoadOnDemand = true,
                FileName = "Int\\ScanWin.ilb"
            };
            LayoutSession.EndRead(ScanWin);
            GenericI = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "GenericI",
                LoadOnDemand = true,
                FileName = "Int\\GenericI.ILB"
            };
            LayoutSession.EndRead(GenericI);
            UnitWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "UnitWin",
                LoadOnDemand = true,
                FileName = "Int\\UnitWin.ILB"
            };
            LayoutSession.EndRead(UnitWin);
            UnitIcons = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "UnitIcons",
                LoadOnDemand = true,
                FileName = "Int\\Icons.ILB"
            };
            LayoutSession.EndRead(UnitIcons);
            TurnButton = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "TurnButton",
                LoadOnDemand = true,
                FileName = "Int\\TBTurnBt.ILB"
            };
            LayoutSession.EndRead(TurnButton);
            TBWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "TBWin",
                LoadOnDemand = true,
                FileName = "Int\\TBWin.ILB"
            };
            LayoutSession.EndRead(TBWin);
            BookPL = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "BookPL",
                LoadOnDemand = true,
                LayoutData = new byte[]
                {
                    4,
                    73,
                    76,
                    66,
                    0,
                    205,
                    93,
                    3,
                    0,
                    0,
                    128,
                    64,
                    24,
                    0,
                    0,
                    0,
                    32,
                    0,
                    0,
                    0,
                    32,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    255,
                    255,
                    255,
                    255
                }
            };
            LayoutSession.EndRead(BookPL);
            BookPR = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "BookPR",
                LoadOnDemand = true,
                LayoutData = new byte[]
                {
                    4,
                    73,
                    76,
                    66,
                    0,
                    205,
                    93,
                    3,
                    0,
                    0,
                    128,
                    64,
                    24,
                    0,
                    0,
                    0,
                    32,
                    0,
                    0,
                    0,
                    32,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    255,
                    255,
                    255,
                    255
                }
            };
            LayoutSession.EndRead(BookPR);
            FactionWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "FactionWin",
                LoadOnDemand = true,
                FileName = "Int\\Scenes\\FactionS.ILB"
            };
            LayoutSession.EndRead(FactionWin);
            RelWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "RelWin",
                LoadOnDemand = true,
                FileName = "Int\\Scenes\\RelFr.ILB"
            };
            LayoutSession.EndRead(RelWin);
            WorldMap = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "WorldMap",
                LoadOnDemand = true,
                FileName = "Int\\Scenes\\WMBackg.ILB"
            };
            LayoutSession.EndRead(WorldMap);
            BookWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "BookWin",
                LoadOnDemand = true,
                FileName = "Int\\Scenes\\BookWin.ILB"
            };
            LayoutSession.EndRead(BookWin);
            TitleScreen = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "TitleScreen",
                LoadOnDemand = true,
                FileName = "Int\\Scenes\\TitleM.ILB"
            };
            LayoutSession.EndRead(TitleScreen);
            Shields = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "Shields",
                LoadOnDemand = true,
                FileName = "Int\\ShieldsM.ILB"
            };
            LayoutSession.EndRead(Shields);
            ProdWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "ProdWin",
                LoadOnDemand = true,
                FileName = "Int\\ProdWin.ILB"
            };
            LayoutSession.EndRead(ProdWin);
            FireLg = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "FireLg",
                LoadOnDemand = true,
                FileName = "IMAGES\\Effects\\FireLg.ILB"
            };
            LayoutSession.EndRead(FireLg);
            MiscArt = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "MiscArt",
                LoadOnDemand = true,
                FileName = "Int\\MiscArt.ILB"
            };
            LayoutSession.EndRead(MiscArt);
            Units = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "Units",
                LoadOnDemand = true,
                FileName = "Int\\Units.ILB"
            };
            LayoutSession.EndRead(Units);
            SoundWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "SoundWin",
                LoadOnDemand = true,
                FileName = "Int\\AudioImg.ILB"
            };
            LayoutSession.EndRead(SoundWin);
            TitleIcons = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "TitleIcons",
                LoadOnDemand = true,
                FileName = "Int\\WinTIcon.ilb"
            };
            LayoutSession.EndRead(TitleIcons);
            MoveIcons = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "MoveIcons",
                LoadOnDemand = true,
                FileName = "Int\\IconMove.ILB"
            };
            LayoutSession.EndRead(MoveIcons);
            LBIcons = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "LBIcons",
                LoadOnDemand = true,
                FileName = "Int\\LBIcon.ILB"
            };
            LayoutSession.EndRead(LBIcons);
            EventIcons = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "EventIcons",
                LoadOnDemand = true,
                FileName = "Int\\EventIcn.ILB"
            };
            LayoutSession.EndRead(EventIcons);
            ProdIcons = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "ProdIcons",
                LoadOnDemand = true,
                FileName = "Int\\ProdIcon.ILB"
            };
            LayoutSession.EndRead(ProdIcons);
            GenericB = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "GenericB",
                LoadOnDemand = true,
                FileName = "Int\\GenericB.ILB"
            };
            LayoutSession.EndRead(GenericB);
            GenericE = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "GenericE",
                LoadOnDemand = true,
                FileName = "Int\\GenericE.ILB"
            };
            LayoutSession.EndRead(GenericE);
            GenericS = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "GenericS",
                LoadOnDemand = true,
                FileName = "Int\\GenericS.ilb"
            };
            LayoutSession.EndRead(GenericS);
            VictoryFrames = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "VictoryFrames",
                LoadOnDemand = true,
                FileName = "INT\\Scenes\\Victory.ILB"
            };
            LayoutSession.EndRead(VictoryFrames);
            EndScene = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "EndScene",
                LoadOnDemand = true,
                FileName = "INT\\Scenes\\EndScene.ILB"
            };
            LayoutSession.EndRead(EndScene);
            PyreFrames = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "PyreFrames",
                LoadOnDemand = true,
                FileName = "INT\\Scenes\\PyreFrm.ILB"
            };
            LayoutSession.EndRead(PyreFrames);
            CityWin = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "CityWin",
                LoadOnDemand = true,
                FileName = "INT\\Scenes\\CityWin.ILB"
            };
            LayoutSession.EndRead(CityWin);
            CampaignTrans = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "CampaignTrans",
                LoadOnDemand = true,
                FileName = "INT\\Scenes\\CTrans.ILB"
            };
            LayoutSession.EndRead(CampaignTrans);
            ScanObj = new Aow.Units.ImageLib.TImageLibrary(this)
            {
                Name = "ScanObj",
                LoadOnDemand = true,
                FileName = "Int\\ScanObj.ilb"
            };
            LayoutSession.EndRead(ScanObj);
            PlyrIcns = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "PlyrIcns",
                LoadOnDemand = true,
                FileName = "Int\\Scenes\\PlyrIcns.ILB"
            };
            LayoutSession.EndRead(PlyrIcns);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            Loaded();
            GenericT.Loaded();
            GenericF.Loaded();
            CeltDeco.Loaded();
            Cursors.Loaded();
            ScanTiles.Loaded();
            ScanFog.Loaded();
            ScanHiLites.Loaded();
            PyreAnim.Loaded();
            UnitBGs.Loaded();
            FCWin.Loaded();
            Menus.Loaded();
            DragonMenu.Loaded();
            ScanWin.Loaded();
            GenericI.Loaded();
            UnitWin.Loaded();
            UnitIcons.Loaded();
            TurnButton.Loaded();
            TBWin.Loaded();
            BookPL.Loaded();
            BookPR.Loaded();
            FactionWin.Loaded();
            RelWin.Loaded();
            WorldMap.Loaded();
            BookWin.Loaded();
            TitleScreen.Loaded();
            Shields.Loaded();
            ProdWin.Loaded();
            FireLg.Loaded();
            MiscArt.Loaded();
            Units.Loaded();
            SoundWin.Loaded();
            TitleIcons.Loaded();
            MoveIcons.Loaded();
            LBIcons.Loaded();
            EventIcons.Loaded();
            ProdIcons.Loaded();
            GenericB.Loaded();
            GenericE.Loaded();
            GenericS.Loaded();
            VictoryFrames.Loaded();
            EndScene.Loaded();
            PyreFrames.Loaded();
            CityWin.Loaded();
            CampaignTrans.Loaded();
            ScanObj.Loaded();
            PlyrIcns.Loaded();
            session.Resolve(this);
        }
    }
}
