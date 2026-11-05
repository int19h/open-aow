// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Layout;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.ImageLib;

namespace Aow.Units.aowIntGfxMod
{
    public partial class TIntGfxMod : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "IntGfxMod";
            GenericT = new TInterfaceIL(this);
            session.BeginRead(GenericT);
            GenericT.Name = "GenericT";
            GenericT.LoadOnDemand = true;
            GenericT.FileName = "Int\\GenericT.ilb";
            session.EndRead(GenericT);
            GenericF = new TInterfaceIL(this);
            session.BeginRead(GenericF);
            GenericF.Name = "GenericF";
            GenericF.LoadOnDemand = true;
            GenericF.FileName = "Int\\GenericF.ILB";
            session.EndRead(GenericF);
            CeltDeco = new TInterfaceIL(this);
            session.BeginRead(CeltDeco);
            CeltDeco.Name = "CeltDeco";
            CeltDeco.LoadOnDemand = true;
            CeltDeco.FileName = "Int\\CeltDeco.ILB";
            session.EndRead(CeltDeco);
            Cursors = new TInterfaceIL(this);
            session.BeginRead(Cursors);
            Cursors.Name = "Cursors";
            Cursors.LoadOnDemand = true;
            Cursors.FileName = "IMAGES\\Cursors.ilb";
            session.EndRead(Cursors);
            ScanTiles = new TInterfaceIL(this);
            session.BeginRead(ScanTiles);
            ScanTiles.Name = "ScanTiles";
            ScanTiles.LoadOnDemand = true;
            ScanTiles.FileName = "Int\\ScanTile.ilb";
            session.EndRead(ScanTiles);
            ScanFog = new TInterfaceIL(this);
            session.BeginRead(ScanFog);
            ScanFog.Name = "ScanFog";
            ScanFog.LoadOnDemand = true;
            ScanFog.FileName = "Int\\ScanFog.ilb";
            session.EndRead(ScanFog);
            ScanHiLites = new TInterfaceIL(this);
            session.BeginRead(ScanHiLites);
            ScanHiLites.Name = "ScanHiLites";
            ScanHiLites.LoadOnDemand = true;
            ScanHiLites.FileName = "Int\\ScanHiLi.ilb";
            session.EndRead(ScanHiLites);
            PyreAnim = new TInterfaceIL(this);
            session.BeginRead(PyreAnim);
            PyreAnim.Name = "PyreAnim";
            PyreAnim.LoadOnDemand = true;
            PyreAnim.LayoutData = new byte[]
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
            };
            session.EndRead(PyreAnim);
            UnitBGs = new TInterfaceIL(this);
            session.BeginRead(UnitBGs);
            UnitBGs.Name = "UnitBGs";
            UnitBGs.LoadOnDemand = true;
            UnitBGs.FileName = "Int\\UnitBgs.ILB";
            session.EndRead(UnitBGs);
            FCWin = new TInterfaceIL(this);
            session.BeginRead(FCWin);
            FCWin.Name = "FCWin";
            FCWin.LoadOnDemand = true;
            FCWin.FileName = "Int\\FCWin.ILB";
            session.EndRead(FCWin);
            Menus = new TInterfaceIL(this);
            session.BeginRead(Menus);
            Menus.Name = "Menus";
            Menus.LoadOnDemand = true;
            Menus.FileName = "Int\\Menus.ILB";
            session.EndRead(Menus);
            DragonMenu = new TInterfaceIL(this);
            session.BeginRead(DragonMenu);
            DragonMenu.Name = "DragonMenu";
            DragonMenu.LoadOnDemand = true;
            DragonMenu.FileName = "Int\\DragMenu.ILB";
            session.EndRead(DragonMenu);
            ScanWin = new TInterfaceIL(this);
            session.BeginRead(ScanWin);
            ScanWin.Name = "ScanWin";
            ScanWin.LoadOnDemand = true;
            ScanWin.FileName = "Int\\ScanWin.ilb";
            session.EndRead(ScanWin);
            GenericI = new TInterfaceIL(this);
            session.BeginRead(GenericI);
            GenericI.Name = "GenericI";
            GenericI.LoadOnDemand = true;
            GenericI.FileName = "Int\\GenericI.ILB";
            session.EndRead(GenericI);
            UnitWin = new TInterfaceIL(this);
            session.BeginRead(UnitWin);
            UnitWin.Name = "UnitWin";
            UnitWin.LoadOnDemand = true;
            UnitWin.FileName = "Int\\UnitWin.ILB";
            session.EndRead(UnitWin);
            UnitIcons = new TInterfaceIL(this);
            session.BeginRead(UnitIcons);
            UnitIcons.Name = "UnitIcons";
            UnitIcons.LoadOnDemand = true;
            UnitIcons.FileName = "Int\\Icons.ILB";
            session.EndRead(UnitIcons);
            TurnButton = new TInterfaceIL(this);
            session.BeginRead(TurnButton);
            TurnButton.Name = "TurnButton";
            TurnButton.LoadOnDemand = true;
            TurnButton.FileName = "Int\\TBTurnBt.ILB";
            session.EndRead(TurnButton);
            TBWin = new TInterfaceIL(this);
            session.BeginRead(TBWin);
            TBWin.Name = "TBWin";
            TBWin.LoadOnDemand = true;
            TBWin.FileName = "Int\\TBWin.ILB";
            session.EndRead(TBWin);
            BookPL = new TInterfaceIL(this);
            session.BeginRead(BookPL);
            BookPL.Name = "BookPL";
            BookPL.LoadOnDemand = true;
            BookPL.LayoutData = new byte[]
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
            };
            session.EndRead(BookPL);
            BookPR = new TInterfaceIL(this);
            session.BeginRead(BookPR);
            BookPR.Name = "BookPR";
            BookPR.LoadOnDemand = true;
            BookPR.LayoutData = new byte[]
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
            };
            session.EndRead(BookPR);
            FactionWin = new TInterfaceIL(this);
            session.BeginRead(FactionWin);
            FactionWin.Name = "FactionWin";
            FactionWin.LoadOnDemand = true;
            FactionWin.FileName = "Int\\Scenes\\FactionS.ILB";
            session.EndRead(FactionWin);
            RelWin = new TInterfaceIL(this);
            session.BeginRead(RelWin);
            RelWin.Name = "RelWin";
            RelWin.LoadOnDemand = true;
            RelWin.FileName = "Int\\Scenes\\RelFr.ILB";
            session.EndRead(RelWin);
            WorldMap = new TInterfaceIL(this);
            session.BeginRead(WorldMap);
            WorldMap.Name = "WorldMap";
            WorldMap.LoadOnDemand = true;
            WorldMap.FileName = "Int\\Scenes\\WMBackg.ILB";
            session.EndRead(WorldMap);
            BookWin = new TInterfaceIL(this);
            session.BeginRead(BookWin);
            BookWin.Name = "BookWin";
            BookWin.LoadOnDemand = true;
            BookWin.FileName = "Int\\Scenes\\BookWin.ILB";
            session.EndRead(BookWin);
            TitleScreen = new TInterfaceIL(this);
            session.BeginRead(TitleScreen);
            TitleScreen.Name = "TitleScreen";
            TitleScreen.LoadOnDemand = true;
            TitleScreen.FileName = "Int\\Scenes\\TitleM.ILB";
            session.EndRead(TitleScreen);
            Shields = new TInterfaceIL(this);
            session.BeginRead(Shields);
            Shields.Name = "Shields";
            Shields.LoadOnDemand = true;
            Shields.FileName = "Int\\ShieldsM.ILB";
            session.EndRead(Shields);
            ProdWin = new TInterfaceIL(this);
            session.BeginRead(ProdWin);
            ProdWin.Name = "ProdWin";
            ProdWin.LoadOnDemand = true;
            ProdWin.FileName = "Int\\ProdWin.ILB";
            session.EndRead(ProdWin);
            FireLg = new TInterfaceIL(this);
            session.BeginRead(FireLg);
            FireLg.Name = "FireLg";
            FireLg.LoadOnDemand = true;
            FireLg.FileName = "IMAGES\\Effects\\FireLg.ILB";
            session.EndRead(FireLg);
            MiscArt = new TInterfaceIL(this);
            session.BeginRead(MiscArt);
            MiscArt.Name = "MiscArt";
            MiscArt.LoadOnDemand = true;
            MiscArt.FileName = "Int\\MiscArt.ILB";
            session.EndRead(MiscArt);
            Units = new TInterfaceIL(this);
            session.BeginRead(Units);
            Units.Name = "Units";
            Units.LoadOnDemand = true;
            Units.FileName = "Int\\Units.ILB";
            session.EndRead(Units);
            SoundWin = new TInterfaceIL(this);
            session.BeginRead(SoundWin);
            SoundWin.Name = "SoundWin";
            SoundWin.LoadOnDemand = true;
            SoundWin.FileName = "Int\\AudioImg.ILB";
            session.EndRead(SoundWin);
            TitleIcons = new TInterfaceIL(this);
            session.BeginRead(TitleIcons);
            TitleIcons.Name = "TitleIcons";
            TitleIcons.LoadOnDemand = true;
            TitleIcons.FileName = "Int\\WinTIcon.ilb";
            session.EndRead(TitleIcons);
            MoveIcons = new TInterfaceIL(this);
            session.BeginRead(MoveIcons);
            MoveIcons.Name = "MoveIcons";
            MoveIcons.LoadOnDemand = true;
            MoveIcons.FileName = "Int\\IconMove.ILB";
            session.EndRead(MoveIcons);
            LBIcons = new TInterfaceIL(this);
            session.BeginRead(LBIcons);
            LBIcons.Name = "LBIcons";
            LBIcons.LoadOnDemand = true;
            LBIcons.FileName = "Int\\LBIcon.ILB";
            session.EndRead(LBIcons);
            EventIcons = new TInterfaceIL(this);
            session.BeginRead(EventIcons);
            EventIcons.Name = "EventIcons";
            EventIcons.LoadOnDemand = true;
            EventIcons.FileName = "Int\\EventIcn.ILB";
            session.EndRead(EventIcons);
            ProdIcons = new TInterfaceIL(this);
            session.BeginRead(ProdIcons);
            ProdIcons.Name = "ProdIcons";
            ProdIcons.LoadOnDemand = true;
            ProdIcons.FileName = "Int\\ProdIcon.ILB";
            session.EndRead(ProdIcons);
            GenericB = new TInterfaceIL(this);
            session.BeginRead(GenericB);
            GenericB.Name = "GenericB";
            GenericB.LoadOnDemand = true;
            GenericB.FileName = "Int\\GenericB.ILB";
            session.EndRead(GenericB);
            GenericE = new TInterfaceIL(this);
            session.BeginRead(GenericE);
            GenericE.Name = "GenericE";
            GenericE.LoadOnDemand = true;
            GenericE.FileName = "Int\\GenericE.ILB";
            session.EndRead(GenericE);
            GenericS = new TInterfaceIL(this);
            session.BeginRead(GenericS);
            GenericS.Name = "GenericS";
            GenericS.LoadOnDemand = true;
            GenericS.FileName = "Int\\GenericS.ilb";
            session.EndRead(GenericS);
            VictoryFrames = new TInterfaceIL(this);
            session.BeginRead(VictoryFrames);
            VictoryFrames.Name = "VictoryFrames";
            VictoryFrames.LoadOnDemand = true;
            VictoryFrames.FileName = "INT\\Scenes\\Victory.ILB";
            session.EndRead(VictoryFrames);
            EndScene = new TInterfaceIL(this);
            session.BeginRead(EndScene);
            EndScene.Name = "EndScene";
            EndScene.LoadOnDemand = true;
            EndScene.FileName = "INT\\Scenes\\EndScene.ILB";
            session.EndRead(EndScene);
            PyreFrames = new TInterfaceIL(this);
            session.BeginRead(PyreFrames);
            PyreFrames.Name = "PyreFrames";
            PyreFrames.LoadOnDemand = true;
            PyreFrames.FileName = "INT\\Scenes\\PyreFrm.ILB";
            session.EndRead(PyreFrames);
            CityWin = new TInterfaceIL(this);
            session.BeginRead(CityWin);
            CityWin.Name = "CityWin";
            CityWin.LoadOnDemand = true;
            CityWin.FileName = "INT\\Scenes\\CityWin.ILB";
            session.EndRead(CityWin);
            CampaignTrans = new TInterfaceIL(this);
            session.BeginRead(CampaignTrans);
            CampaignTrans.Name = "CampaignTrans";
            CampaignTrans.LoadOnDemand = true;
            CampaignTrans.FileName = "INT\\Scenes\\CTrans.ILB";
            session.EndRead(CampaignTrans);
            ScanObj = new TImageLibrary(this);
            session.BeginRead(ScanObj);
            ScanObj.Name = "ScanObj";
            ScanObj.LoadOnDemand = true;
            ScanObj.FileName = "Int\\ScanObj.ilb";
            session.EndRead(ScanObj);
            PlyrIcns = new TInterfaceIL(this);
            session.BeginRead(PlyrIcns);
            PlyrIcns.Name = "PlyrIcns";
            PlyrIcns.LoadOnDemand = true;
            PlyrIcns.FileName = "Int\\Scenes\\PlyrIcns.ILB";
            session.EndRead(PlyrIcns);
            session.EndRead(this);
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
