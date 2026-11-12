// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Draw.Images;
using Aow.Game.Layout;

namespace Aow.Game.UI.Dialogs;

public partial class UIGraphics : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "IntGfxMod";
        GenericTextures = new InterfaceImageLibrary(this);
        session.BeginRead(GenericTextures);
        GenericTextures.Name = "GenericT";
        GenericTextures.LoadOnDemand = true;
        GenericTextures.FileName = "Int\\GenericT.ilb";
        session.EndRead(GenericTextures);
        GenericFrames = new InterfaceImageLibrary(this);
        session.BeginRead(GenericFrames);
        GenericFrames.Name = "GenericF";
        GenericFrames.LoadOnDemand = true;
        GenericFrames.FileName = "Int\\GenericF.ILB";
        session.EndRead(GenericFrames);
        CelticDecorations = new InterfaceImageLibrary(this);
        session.BeginRead(CelticDecorations);
        CelticDecorations.Name = "CeltDeco";
        CelticDecorations.LoadOnDemand = true;
        CelticDecorations.FileName = "Int\\CeltDeco.ILB";
        session.EndRead(CelticDecorations);
        Cursors = new InterfaceImageLibrary(this);
        session.BeginRead(Cursors);
        Cursors.Name = "Cursors";
        Cursors.LoadOnDemand = true;
        Cursors.FileName = "IMAGES\\Cursors.ilb";
        session.EndRead(Cursors);
        ScannerTiles = new InterfaceImageLibrary(this);
        session.BeginRead(ScannerTiles);
        ScannerTiles.Name = "ScanTiles";
        ScannerTiles.LoadOnDemand = true;
        ScannerTiles.FileName = "Int\\ScanTile.ilb";
        session.EndRead(ScannerTiles);
        ScannerFog = new InterfaceImageLibrary(this);
        session.BeginRead(ScannerFog);
        ScannerFog.Name = "ScanFog";
        ScannerFog.LoadOnDemand = true;
        ScannerFog.FileName = "Int\\ScanFog.ilb";
        session.EndRead(ScannerFog);
        ScannerHighlights = new InterfaceImageLibrary(this);
        session.BeginRead(ScannerHighlights);
        ScannerHighlights.Name = "ScanHiLites";
        ScannerHighlights.LoadOnDemand = true;
        ScannerHighlights.FileName = "Int\\ScanHiLi.ilb";
        session.EndRead(ScannerHighlights);
        InterfaceImageLibrary pyreAnim = new InterfaceImageLibrary(this);
        session.BeginRead(pyreAnim);
        pyreAnim.Name = "PyreAnim";
        pyreAnim.LoadOnDemand = true;
        pyreAnim.LayoutData = new byte[]
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
        session.EndRead(pyreAnim);
        UnitBackgrounds = new InterfaceImageLibrary(this);
        session.BeginRead(UnitBackgrounds);
        UnitBackgrounds.Name = "UnitBGs";
        UnitBackgrounds.LoadOnDemand = true;
        UnitBackgrounds.FileName = "Int\\UnitBgs.ILB";
        session.EndRead(UnitBackgrounds);
        FastBattleWindow = new InterfaceImageLibrary(this);
        session.BeginRead(FastBattleWindow);
        FastBattleWindow.Name = "FCWin";
        FastBattleWindow.LoadOnDemand = true;
        FastBattleWindow.FileName = "Int\\FCWin.ILB";
        session.EndRead(FastBattleWindow);
        Menus = new InterfaceImageLibrary(this);
        session.BeginRead(Menus);
        Menus.Name = "Menus";
        Menus.LoadOnDemand = true;
        Menus.FileName = "Int\\Menus.ILB";
        session.EndRead(Menus);
        DragonMenu = new InterfaceImageLibrary(this);
        session.BeginRead(DragonMenu);
        DragonMenu.Name = "DragonMenu";
        DragonMenu.LoadOnDemand = true;
        DragonMenu.FileName = "Int\\DragMenu.ILB";
        session.EndRead(DragonMenu);
        ScannerWindow = new InterfaceImageLibrary(this);
        session.BeginRead(ScannerWindow);
        ScannerWindow.Name = "ScanWin";
        ScannerWindow.LoadOnDemand = true;
        ScannerWindow.FileName = "Int\\ScanWin.ilb";
        session.EndRead(ScannerWindow);
        GenericIcons = new InterfaceImageLibrary(this);
        session.BeginRead(GenericIcons);
        GenericIcons.Name = "GenericI";
        GenericIcons.LoadOnDemand = true;
        GenericIcons.FileName = "Int\\GenericI.ILB";
        session.EndRead(GenericIcons);
        UnitWindow = new InterfaceImageLibrary(this);
        session.BeginRead(UnitWindow);
        UnitWindow.Name = "UnitWin";
        UnitWindow.LoadOnDemand = true;
        UnitWindow.FileName = "Int\\UnitWin.ILB";
        session.EndRead(UnitWindow);
        UnitIcons = new InterfaceImageLibrary(this);
        session.BeginRead(UnitIcons);
        UnitIcons.Name = "UnitIcons";
        UnitIcons.LoadOnDemand = true;
        UnitIcons.FileName = "Int\\Icons.ILB";
        session.EndRead(UnitIcons);
        TurnButton = new InterfaceImageLibrary(this);
        session.BeginRead(TurnButton);
        TurnButton.Name = "TurnButton";
        TurnButton.LoadOnDemand = true;
        TurnButton.FileName = "Int\\TBTurnBt.ILB";
        session.EndRead(TurnButton);
        TaskBar = new InterfaceImageLibrary(this);
        session.BeginRead(TaskBar);
        TaskBar.Name = "TBWin";
        TaskBar.LoadOnDemand = true;
        TaskBar.FileName = "Int\\TBWin.ILB";
        session.EndRead(TaskBar);
        BookLeftPage = new InterfaceImageLibrary(this);
        session.BeginRead(BookLeftPage);
        BookLeftPage.Name = "BookPL";
        BookLeftPage.LoadOnDemand = true;
        BookLeftPage.LayoutData = new byte[]
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
        session.EndRead(BookLeftPage);
        BookRightPage = new InterfaceImageLibrary(this);
        session.BeginRead(BookRightPage);
        BookRightPage.Name = "BookPR";
        BookRightPage.LoadOnDemand = true;
        BookRightPage.LayoutData = new byte[]
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
        session.EndRead(BookRightPage);
        FactionWindow = new InterfaceImageLibrary(this);
        session.BeginRead(FactionWindow);
        FactionWindow.Name = "FactionWin";
        FactionWindow.LoadOnDemand = true;
        FactionWindow.FileName = "Int\\Scenes\\FactionS.ILB";
        session.EndRead(FactionWindow);
        RelationWindow = new InterfaceImageLibrary(this);
        session.BeginRead(RelationWindow);
        RelationWindow.Name = "RelWin";
        RelationWindow.LoadOnDemand = true;
        RelationWindow.FileName = "Int\\Scenes\\RelFr.ILB";
        session.EndRead(RelationWindow);
        WorldMap = new InterfaceImageLibrary(this);
        session.BeginRead(WorldMap);
        WorldMap.Name = "WorldMap";
        WorldMap.LoadOnDemand = true;
        WorldMap.FileName = "Int\\Scenes\\WMBackg.ILB";
        session.EndRead(WorldMap);
        BookWindow = new InterfaceImageLibrary(this);
        session.BeginRead(BookWindow);
        BookWindow.Name = "BookWin";
        BookWindow.LoadOnDemand = true;
        BookWindow.FileName = "Int\\Scenes\\BookWin.ILB";
        session.EndRead(BookWindow);
        TitleScreen = new InterfaceImageLibrary(this);
        session.BeginRead(TitleScreen);
        TitleScreen.Name = "TitleScreen";
        TitleScreen.LoadOnDemand = true;
        TitleScreen.FileName = "Int\\Scenes\\TitleM.ILB";
        session.EndRead(TitleScreen);
        Shields = new InterfaceImageLibrary(this);
        session.BeginRead(Shields);
        Shields.Name = "Shields";
        Shields.LoadOnDemand = true;
        Shields.FileName = "Int\\ShieldsM.ILB";
        session.EndRead(Shields);
        ProductionWindow = new InterfaceImageLibrary(this);
        session.BeginRead(ProductionWindow);
        ProductionWindow.Name = "ProdWin";
        ProductionWindow.LoadOnDemand = true;
        ProductionWindow.FileName = "Int\\ProdWin.ILB";
        session.EndRead(ProductionWindow);
        InterfaceImageLibrary fireLg = new InterfaceImageLibrary(this);
        session.BeginRead(fireLg);
        fireLg.Name = "FireLg";
        fireLg.LoadOnDemand = true;
        fireLg.FileName = "IMAGES\\Effects\\FireLg.ILB";
        session.EndRead(fireLg);
        MiscArt = new InterfaceImageLibrary(this);
        session.BeginRead(MiscArt);
        MiscArt.Name = "MiscArt";
        MiscArt.LoadOnDemand = true;
        MiscArt.FileName = "Int\\MiscArt.ILB";
        session.EndRead(MiscArt);
        Units = new InterfaceImageLibrary(this);
        session.BeginRead(Units);
        Units.Name = "Units";
        Units.LoadOnDemand = true;
        Units.FileName = "Int\\Units.ILB";
        session.EndRead(Units);
        SoundWindow = new InterfaceImageLibrary(this);
        session.BeginRead(SoundWindow);
        SoundWindow.Name = "SoundWin";
        SoundWindow.LoadOnDemand = true;
        SoundWindow.FileName = "Int\\AudioImg.ILB";
        session.EndRead(SoundWindow);
        TitleIcons = new InterfaceImageLibrary(this);
        session.BeginRead(TitleIcons);
        TitleIcons.Name = "TitleIcons";
        TitleIcons.LoadOnDemand = true;
        TitleIcons.FileName = "Int\\WinTIcon.ilb";
        session.EndRead(TitleIcons);
        MoveIcons = new InterfaceImageLibrary(this);
        session.BeginRead(MoveIcons);
        MoveIcons.Name = "MoveIcons";
        MoveIcons.LoadOnDemand = true;
        MoveIcons.FileName = "Int\\IconMove.ILB";
        session.EndRead(MoveIcons);
        ListIcons = new InterfaceImageLibrary(this);
        session.BeginRead(ListIcons);
        ListIcons.Name = "LBIcons";
        ListIcons.LoadOnDemand = true;
        ListIcons.FileName = "Int\\LBIcon.ILB";
        session.EndRead(ListIcons);
        EventIcons = new InterfaceImageLibrary(this);
        session.BeginRead(EventIcons);
        EventIcons.Name = "EventIcons";
        EventIcons.LoadOnDemand = true;
        EventIcons.FileName = "Int\\EventIcn.ILB";
        session.EndRead(EventIcons);
        ProductionIcons = new InterfaceImageLibrary(this);
        session.BeginRead(ProductionIcons);
        ProductionIcons.Name = "ProdIcons";
        ProductionIcons.LoadOnDemand = true;
        ProductionIcons.FileName = "Int\\ProdIcon.ILB";
        session.EndRead(ProductionIcons);
        GenericButtons = new InterfaceImageLibrary(this);
        session.BeginRead(GenericButtons);
        GenericButtons.Name = "GenericB";
        GenericButtons.LoadOnDemand = true;
        GenericButtons.FileName = "Int\\GenericB.ILB";
        session.EndRead(GenericButtons);
        GenericEdits = new InterfaceImageLibrary(this);
        session.BeginRead(GenericEdits);
        GenericEdits.Name = "GenericE";
        GenericEdits.LoadOnDemand = true;
        GenericEdits.FileName = "Int\\GenericE.ILB";
        session.EndRead(GenericEdits);
        GenericScrollBars = new InterfaceImageLibrary(this);
        session.BeginRead(GenericScrollBars);
        GenericScrollBars.Name = "GenericS";
        GenericScrollBars.LoadOnDemand = true;
        GenericScrollBars.FileName = "Int\\GenericS.ilb";
        session.EndRead(GenericScrollBars);
        VictoryFrames = new InterfaceImageLibrary(this);
        session.BeginRead(VictoryFrames);
        VictoryFrames.Name = "VictoryFrames";
        VictoryFrames.LoadOnDemand = true;
        VictoryFrames.FileName = "INT\\Scenes\\Victory.ILB";
        session.EndRead(VictoryFrames);
        EndScene = new InterfaceImageLibrary(this);
        session.BeginRead(EndScene);
        EndScene.Name = "EndScene";
        EndScene.LoadOnDemand = true;
        EndScene.FileName = "INT\\Scenes\\EndScene.ILB";
        session.EndRead(EndScene);
        PyreFrames = new InterfaceImageLibrary(this);
        session.BeginRead(PyreFrames);
        PyreFrames.Name = "PyreFrames";
        PyreFrames.LoadOnDemand = true;
        PyreFrames.FileName = "INT\\Scenes\\PyreFrm.ILB";
        session.EndRead(PyreFrames);
        CityWindow = new InterfaceImageLibrary(this);
        session.BeginRead(CityWindow);
        CityWindow.Name = "CityWin";
        CityWindow.LoadOnDemand = true;
        CityWindow.FileName = "INT\\Scenes\\CityWin.ILB";
        session.EndRead(CityWindow);
        CampaignTransfer = new InterfaceImageLibrary(this);
        session.BeginRead(CampaignTransfer);
        CampaignTransfer.Name = "CampaignTrans";
        CampaignTransfer.LoadOnDemand = true;
        CampaignTransfer.FileName = "INT\\Scenes\\CTrans.ILB";
        session.EndRead(CampaignTransfer);
        ScannerObjects = new ImageLibrary(this);
        session.BeginRead(ScannerObjects);
        ScannerObjects.Name = "ScanObj";
        ScannerObjects.LoadOnDemand = true;
        ScannerObjects.FileName = "Int\\ScanObj.ilb";
        session.EndRead(ScannerObjects);
        PlayerIcons = new InterfaceImageLibrary(this);
        session.BeginRead(PlayerIcons);
        PlayerIcons.Name = "PlyrIcns";
        PlayerIcons.LoadOnDemand = true;
        PlayerIcons.FileName = "Int\\Scenes\\PlyrIcns.ILB";
        session.EndRead(PlayerIcons);
        session.EndRead(this);
        Loaded();
        GenericTextures.Loaded();
        GenericFrames.Loaded();
        CelticDecorations.Loaded();
        Cursors.Loaded();
        ScannerTiles.Loaded();
        ScannerFog.Loaded();
        ScannerHighlights.Loaded();
        pyreAnim.Loaded();
        UnitBackgrounds.Loaded();
        FastBattleWindow.Loaded();
        Menus.Loaded();
        DragonMenu.Loaded();
        ScannerWindow.Loaded();
        GenericIcons.Loaded();
        UnitWindow.Loaded();
        UnitIcons.Loaded();
        TurnButton.Loaded();
        TaskBar.Loaded();
        BookLeftPage.Loaded();
        BookRightPage.Loaded();
        FactionWindow.Loaded();
        RelationWindow.Loaded();
        WorldMap.Loaded();
        BookWindow.Loaded();
        TitleScreen.Loaded();
        Shields.Loaded();
        ProductionWindow.Loaded();
        fireLg.Loaded();
        MiscArt.Loaded();
        Units.Loaded();
        SoundWindow.Loaded();
        TitleIcons.Loaded();
        MoveIcons.Loaded();
        ListIcons.Loaded();
        EventIcons.Loaded();
        ProductionIcons.Loaded();
        GenericButtons.Loaded();
        GenericEdits.Loaded();
        GenericScrollBars.Loaded();
        VictoryFrames.Loaded();
        EndScene.Loaded();
        PyreFrames.Loaded();
        CityWindow.Loaded();
        CampaignTransfer.Loaded();
        ScannerObjects.Loaded();
        PlayerIcons.Loaded();
        session.Resolve(this);
    }
}
