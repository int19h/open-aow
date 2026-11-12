// Translated from FontMod.pas.
using Aow.Game.Draw.Images;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class UIFonts : DataModule
{
    public UIFonts() : base(null)
    { }

    public ImageLibraryFont Age8 { get; private set; }
    public ImageLibraryFont Age10 { get; private set; }
    public ImageLibraryFont Age16Shadow { get; private set; }
    public ImageLibraryFont Age10Shadow { get; private set; }
    public ImageLibraryFont Age8Shadow { get; private set; }
    public ImageLibraryFont Age14Gold { get; private set; }
    public ImageLibraryFont Age15WhiteOnGrey { get; private set; }
    public ImageLibraryFont Age15BlackOnBrown { get; private set; }
    public ImageLibraryFont Age10List { get; private set; }
    public ImageLibraryFont Age10Bold { get; private set; }
    public ImageLibraryFont Age10BoldShadow { get; private set; }
    public ImageLibraryFont TaskbarDigits { get; private set; }
}
