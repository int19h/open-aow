// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.FontMod
{
    public partial class TFontModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "FontModule";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Age8 = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Age8",
                CharacterDistance = 0,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 7864183,
                FontColorPaletteIndex = 1,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age8.ILB"
            };
            LayoutSession.EndRead(Age8);
            Age10 = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Age10",
                CharacterDistance = 1,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 12632256,
                FontColorPaletteIndex = 1,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age10.ILB"
            };
            LayoutSession.EndRead(Age10);
            Age16s = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Age16s",
                CharacterDistance = 0,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 16777215,
                FontColorPaletteIndex = 176,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age16s.ILB"
            };
            LayoutSession.EndRead(Age16s);
            Age10s = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Age10s",
                CharacterDistance = 1,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 9236989,
                FontColorPaletteIndex = 2,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age10s.ILB"
            };
            LayoutSession.EndRead(Age10s);
            Age8s = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Age8s",
                CharacterDistance = 0,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 9688812,
                FontColorPaletteIndex = 1,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age8s.ILB"
            };
            LayoutSession.EndRead(Age8s);
            Sessis14G = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Sessis14G",
                CharacterDistance = -2,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 16777215,
                FontColorPaletteIndex = -1,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age14G.ILB"
            };
            LayoutSession.EndRead(Sessis14G);
            AoW15WhiteGrey = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "AoW15WhiteGrey",
                CharacterDistance = 0,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 16777215,
                FontColorPaletteIndex = -1,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age15WhGr.ILB"
            };
            LayoutSession.EndRead(AoW15WhiteGrey);
            AoW15BlackBrown = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "AoW15BlackBrown",
                CharacterDistance = 0,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 1254460,
                FontColorPaletteIndex = -1,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age15BlBr.ILB"
            };
            LayoutSession.EndRead(AoW15BlackBrown);
            Age10LB = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Age10LB",
                CharacterDistance = 0,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 0,
                FontColorPaletteIndex = 1,
                LoadOnDemand = true,
                FileName = "Int\\Fonts\\Age10LB.ILB"
            };
            LayoutSession.EndRead(Age10LB);
            Age10b = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Age10b",
                CharacterDistance = 1,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 1254460,
                FontColorPaletteIndex = 1,
                LoadOnDemand = true,
                FileName = "INT\\Fonts\\Age10b.ILB"
            };
            LayoutSession.EndRead(Age10b);
            Age10bs = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "Age10bs",
                CharacterDistance = 1,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 8454143,
                FontColorPaletteIndex = 2,
                LoadOnDemand = true,
                FileName = "INT\\Fonts\\Age10bs.ILB"
            };
            LayoutSession.EndRead(Age10bs);
            TBarNumber = new Aow.Units.ImageLib.TImageLibraryFont(this)
            {
                Name = "TBarNumber",
                CharacterDistance = 0,
                CharCase = Aow.Rtl.TEditCharCase.ecNormal,
                Color = 0,
                FontColorPaletteIndex = 1,
                LoadOnDemand = true,
                FileName = "INT\\Fonts\\Age10tb.ILB"
            };
            LayoutSession.EndRead(TBarNumber);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            Loaded();
            Age8.Loaded();
            Age10.Loaded();
            Age16s.Loaded();
            Age10s.Loaded();
            Age8s.Loaded();
            Sessis14G.Loaded();
            AoW15WhiteGrey.Loaded();
            AoW15BlackBrown.Loaded();
            Age10LB.Loaded();
            Age10b.Loaded();
            Age10bs.Loaded();
            TBarNumber.Loaded();
            session.Resolve(this);
        }
    }
}
