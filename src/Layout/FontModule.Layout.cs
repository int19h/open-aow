// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Layout;
using Aow.Rtl;
using Aow.Units.ImageLib;

namespace Aow.Units.FontMod
{
    public partial class TFontModule : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "FontModule";
            Age8 = new TImageLibraryFont(this);
            session.BeginRead(Age8);
            Age8.Name = "Age8";
            Age8.CharacterDistance = 0;
            Age8.CharCase = TEditCharCase.ecNormal;
            Age8.Color = 7864183;
            Age8.FontColorPaletteIndex = 1;
            Age8.LoadOnDemand = true;
            Age8.FileName = "Int\\Fonts\\Age8.ILB";
            session.EndRead(Age8);
            Age10 = new TImageLibraryFont(this);
            session.BeginRead(Age10);
            Age10.Name = "Age10";
            Age10.CharacterDistance = 1;
            Age10.CharCase = TEditCharCase.ecNormal;
            Age10.Color = 12632256;
            Age10.FontColorPaletteIndex = 1;
            Age10.LoadOnDemand = true;
            Age10.FileName = "Int\\Fonts\\Age10.ILB";
            session.EndRead(Age10);
            Age16s = new TImageLibraryFont(this);
            session.BeginRead(Age16s);
            Age16s.Name = "Age16s";
            Age16s.CharacterDistance = 0;
            Age16s.CharCase = TEditCharCase.ecNormal;
            Age16s.Color = 16777215;
            Age16s.FontColorPaletteIndex = 176;
            Age16s.LoadOnDemand = true;
            Age16s.FileName = "Int\\Fonts\\Age16s.ILB";
            session.EndRead(Age16s);
            Age10s = new TImageLibraryFont(this);
            session.BeginRead(Age10s);
            Age10s.Name = "Age10s";
            Age10s.CharacterDistance = 1;
            Age10s.CharCase = TEditCharCase.ecNormal;
            Age10s.Color = 9236989;
            Age10s.FontColorPaletteIndex = 2;
            Age10s.LoadOnDemand = true;
            Age10s.FileName = "Int\\Fonts\\Age10s.ILB";
            session.EndRead(Age10s);
            Age8s = new TImageLibraryFont(this);
            session.BeginRead(Age8s);
            Age8s.Name = "Age8s";
            Age8s.CharacterDistance = 0;
            Age8s.CharCase = TEditCharCase.ecNormal;
            Age8s.Color = 9688812;
            Age8s.FontColorPaletteIndex = 1;
            Age8s.LoadOnDemand = true;
            Age8s.FileName = "Int\\Fonts\\Age8s.ILB";
            session.EndRead(Age8s);
            Sessis14G = new TImageLibraryFont(this);
            session.BeginRead(Sessis14G);
            Sessis14G.Name = "Sessis14G";
            Sessis14G.CharacterDistance = -2;
            Sessis14G.CharCase = TEditCharCase.ecNormal;
            Sessis14G.Color = 16777215;
            Sessis14G.FontColorPaletteIndex = -1;
            Sessis14G.LoadOnDemand = true;
            Sessis14G.FileName = "Int\\Fonts\\Age14G.ILB";
            session.EndRead(Sessis14G);
            AoW15WhiteGrey = new TImageLibraryFont(this);
            session.BeginRead(AoW15WhiteGrey);
            AoW15WhiteGrey.Name = "AoW15WhiteGrey";
            AoW15WhiteGrey.CharacterDistance = 0;
            AoW15WhiteGrey.CharCase = TEditCharCase.ecNormal;
            AoW15WhiteGrey.Color = 16777215;
            AoW15WhiteGrey.FontColorPaletteIndex = -1;
            AoW15WhiteGrey.LoadOnDemand = true;
            AoW15WhiteGrey.FileName = "Int\\Fonts\\Age15WhGr.ILB";
            session.EndRead(AoW15WhiteGrey);
            AoW15BlackBrown = new TImageLibraryFont(this);
            session.BeginRead(AoW15BlackBrown);
            AoW15BlackBrown.Name = "AoW15BlackBrown";
            AoW15BlackBrown.CharacterDistance = 0;
            AoW15BlackBrown.CharCase = TEditCharCase.ecNormal;
            AoW15BlackBrown.Color = 1254460;
            AoW15BlackBrown.FontColorPaletteIndex = -1;
            AoW15BlackBrown.LoadOnDemand = true;
            AoW15BlackBrown.FileName = "Int\\Fonts\\Age15BlBr.ILB";
            session.EndRead(AoW15BlackBrown);
            Age10LB = new TImageLibraryFont(this);
            session.BeginRead(Age10LB);
            Age10LB.Name = "Age10LB";
            Age10LB.CharacterDistance = 0;
            Age10LB.CharCase = TEditCharCase.ecNormal;
            Age10LB.Color = 0;
            Age10LB.FontColorPaletteIndex = 1;
            Age10LB.LoadOnDemand = true;
            Age10LB.FileName = "Int\\Fonts\\Age10LB.ILB";
            session.EndRead(Age10LB);
            Age10b = new TImageLibraryFont(this);
            session.BeginRead(Age10b);
            Age10b.Name = "Age10b";
            Age10b.CharacterDistance = 1;
            Age10b.CharCase = TEditCharCase.ecNormal;
            Age10b.Color = 1254460;
            Age10b.FontColorPaletteIndex = 1;
            Age10b.LoadOnDemand = true;
            Age10b.FileName = "INT\\Fonts\\Age10b.ILB";
            session.EndRead(Age10b);
            Age10bs = new TImageLibraryFont(this);
            session.BeginRead(Age10bs);
            Age10bs.Name = "Age10bs";
            Age10bs.CharacterDistance = 1;
            Age10bs.CharCase = TEditCharCase.ecNormal;
            Age10bs.Color = 8454143;
            Age10bs.FontColorPaletteIndex = 2;
            Age10bs.LoadOnDemand = true;
            Age10bs.FileName = "INT\\Fonts\\Age10bs.ILB";
            session.EndRead(Age10bs);
            TBarNumber = new TImageLibraryFont(this);
            session.BeginRead(TBarNumber);
            TBarNumber.Name = "TBarNumber";
            TBarNumber.CharacterDistance = 0;
            TBarNumber.CharCase = TEditCharCase.ecNormal;
            TBarNumber.Color = 0;
            TBarNumber.FontColorPaletteIndex = 1;
            TBarNumber.LoadOnDemand = true;
            TBarNumber.FileName = "INT\\Fonts\\Age10tb.ILB";
            session.EndRead(TBarNumber);
            session.EndRead(this);
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
