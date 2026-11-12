// Translated from aowIntGfxMod.pas.
using Aow.Game.Draw.Images;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class UIGraphics : DataModule
{
    public UIGraphics() : base(null)
    { }

    public InterfaceImageLibrary GenericTextures { get; private set; }
    public InterfaceImageLibrary GenericFrames { get; private set; }
    public InterfaceImageLibrary CelticDecorations { get; private set; }
    public InterfaceImageLibrary Cursors { get; private set; }
    public InterfaceImageLibrary ScannerTiles { get; private set; }
    public InterfaceImageLibrary ScannerFog { get; private set; }
    public InterfaceImageLibrary ScannerHighlights { get; private set; }
    public InterfaceImageLibrary UnitBackgrounds { get; private set; }
    public InterfaceImageLibrary FastBattleWindow { get; private set; }
    public InterfaceImageLibrary Menus { get; private set; }
    public InterfaceImageLibrary DragonMenu { get; private set; }
    public InterfaceImageLibrary ScannerWindow { get; private set; }
    public InterfaceImageLibrary GenericIcons { get; private set; }
    public InterfaceImageLibrary UnitWindow { get; private set; }
    public InterfaceImageLibrary UnitIcons { get; private set; }
    public InterfaceImageLibrary TurnButton { get; private set; }
    public InterfaceImageLibrary TaskBar { get; private set; }
    public InterfaceImageLibrary BookLeftPage { get; private set; }
    public InterfaceImageLibrary BookRightPage { get; private set; }
    public InterfaceImageLibrary FactionWindow { get; private set; }
    public InterfaceImageLibrary RelationWindow { get; private set; }
    public InterfaceImageLibrary WorldMap { get; private set; }
    public InterfaceImageLibrary BookWindow { get; private set; }
    public InterfaceImageLibrary TitleScreen { get; private set; }
    public InterfaceImageLibrary Shields { get; private set; }
    public InterfaceImageLibrary ProductionWindow { get; private set; }
    public InterfaceImageLibrary MiscArt { get; private set; }
    public InterfaceImageLibrary Units { get; private set; }
    public InterfaceImageLibrary SoundWindow { get; private set; }
    public InterfaceImageLibrary TitleIcons { get; private set; }
    public InterfaceImageLibrary MoveIcons { get; private set; }
    public InterfaceImageLibrary ListIcons { get; private set; }
    public InterfaceImageLibrary EventIcons { get; private set; }
    public InterfaceImageLibrary ProductionIcons { get; private set; }
    public InterfaceImageLibrary GenericButtons { get; private set; }
    public InterfaceImageLibrary GenericEdits { get; private set; }
    public InterfaceImageLibrary GenericScrollBars { get; private set; }
    public InterfaceImageLibrary VictoryFrames { get; private set; }
    public InterfaceImageLibrary EndScene { get; private set; }
    public InterfaceImageLibrary PyreFrames { get; private set; }
    public InterfaceImageLibrary CityWindow { get; private set; }
    public InterfaceImageLibrary CampaignTransfer { get; private set; }
    public ImageLibrary ScannerObjects { get; private set; }
    public InterfaceImageLibrary PlayerIcons { get; private set; }
}
