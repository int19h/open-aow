// Translated from AoWTitleScene.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AowUserWindow;
using Aow.Units.E2Globals;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWFileDialog.AoWFileDialogUnit;
using static Aow.Units.GFXE.GFXEUnit;
using Exception = Aow.Rtl.Exception;

namespace Aow.Units.AoWTitleScene;

// frame counters, mouse button, mouse position, selected/pressed/hover button, scene rectangle,
// redraw/credits, credits, credit page/frame, intro flag/frame, font
public partial class TTitleScene : TDataModule
{
    public TTitleScene(TComponent owner) : base(owner)
    { }

    public TImageLibrary TitleSceneIL;
    public TAoWUserWindow Window;
    public TAOWMemo LegalNotice;
    public TAOWLabel StrLbl;
    public TAOWLabel NameLbl;
    public TAOWLabel Copyright;
    public TAOWLabel Version;
    public int FFrame;
    public int FTotalFrame;
    public TMouseButton FMouseButton;
    public int FMouseX;
    public int FMouseY;
    public int FSelectedButton;
    public int FPressedButton;
    public int FHoverButton;
    public TRect FSceneRect;
    public bool FRedrawBackground;
    public bool FShowCredits;
    public TList FCredits;
    public int FCreditPage;
    public int FCreditFrame;
    public bool FShowIntro;
    public FixedArray_byte_3 FPadA5;
    public int FIntroFrame;
    public TImageLibraryFont FFont;
    public TImageLibraryFont FCreditsFont;

    // AoWTitleScene.pas:103
    public void DrawCredits(ref TSurfaceDesc Surface)
    {
        int X;
        int Y;
        int Duration;
        int Alpha;
        TShowInfo Info = default;
        TStrings Lines;
        FCreditFrame += 1;
        Info.ShowMode = TShowMode.smBlended;
        Info.BlendMode = TBlendMode.bmAlpha;
        Lines = (TStrings)FCredits[FCreditPage];
        X = Surface.Width / 2;
        var count = Lines.Count;
        Y = Surface.Height / 2 - ((count * 25 + FCreditsFont.Height) / 2 + 35);
        Duration = Lines.Count * 30 + 20 + 50;
        if (Lines.Count > 0)
        {
            Alpha = FCreditFrame;
            if (Alpha > Duration)
            {
                Alpha = 100 - (Alpha - Duration) * 5;
            }
            else
            {
                Alpha = Alpha * 10;
            }

            Info.Blend = Alpha;
            if (Alpha > 0)
            {
                if (Alpha < 100)
                {
                    var line = Lines[0];
                    FCreditsFont.SetTextEx(X, Y, line, Surface, TSet<TSetTextModeElement>.Of(TSetTextModeElement.stmCenter), Info);
                }
                else
                {
                    var line = Lines[0];
                    FCreditsFont.SetTextCentered(X, Y, line, Surface);
                }
            }

            for (int I = 1, last = Lines.Count - 1; I <= last; I++)
            {
                if (FCreditFrame > Duration)
                {
                    Alpha = 100 - (FCreditFrame - Duration) * 5;
                }
                else
                {
                    Alpha = (FCreditFrame - (I * 30 + 20)) * 10;
                }

                if (Alpha > 0)
                {
                    Info.Blend = Alpha;
                    if (Alpha < 100)
                    {
                        var line = Lines[I];
                        FCreditsFont.SetTextEx(X, Y + I * 25 + 5, line, Surface, TSet<TSetTextModeElement>.Of(TSetTextModeElement.stmCenter), Info);
                    }
                    else
                    {
                        var line = Lines[I];
                        FCreditsFont.SetTextCentered(X, Y + I * 25 + 5, line, Surface);
                    }
                }
            }
        }

        if (FCreditFrame > Duration + 20)
        {
            FCreditPage += 1;
            if (FCreditPage < FCredits.Count)
            {
                FCreditFrame = 0;
            }
            else
            {
                FTotalFrame = 0;
                FShowCredits = false;
            }
        }
    }

    // AoWTitleScene.pas:148
    public void DrawIntro(ref TSurfaceDesc Surface)
    {
        int Page;
        int X;
        int Y;
        int Frame;
        int Opacity;
        TShowInfo Info = default;
        string Text = "";
        FIntroFrame += 1;
        X = Surface.Width / 2;
        Y = Surface.Height / 2;
        Page = FIntroFrame / 130;
        Frame = FIntroFrame % 130;
        switch (Frame)
        {
            case >= 0 and <= 10:
                Opacity = Frame * 10;
                break;
            case >= 11 and <= 30:
                Opacity = 100;
                break;
            case >= 31 and <= 40:
                Opacity = 100 - (Frame - 30) * 10;
                break;
            default:
                Opacity = 0;
                break;
        }

        if (Opacity != 0)
        {
            switch (Page)
            {
                case 0:
                    Text = TranslateRStr(CoDevelopedByRStr);
                    break;
                case 1:
                    Text = TranslateRStr(CoPublishedByRStr);
                    break;
                case 2:
                    Text = "";
                    break;
            }

            if (Opacity < 100)
            {
                Info.ShowMode = TShowMode.smBlended;
                Info.BlendMode = TBlendMode.bmAlpha;
                Info.Blend = Opacity;
                FCreditsFont.SetTextEx(X, Y - FCreditsFont.Height / 2, Text, Surface, TSet<TSetTextModeElement>.Of(TSetTextModeElement.stmCenter), Info);
            }
            else
            {
                FCreditsFont.SetTextCentered(X, Y - FCreditsFont.Height / 2, Text, Surface);
            }
        }

        if (Frame > 45 && Frame < 130)
        {
            switch (Page)
            {
                case 0:
                    if (TitleSceneIL[4] != null)
                    {
                        var titleSceneILHeight = TitleSceneIL[4];
                        var titleSceneILWidth = TitleSceneIL[4];
                        TitleSceneIL[4].Show(X - 75 - titleSceneILWidth.Width / 2, Y - titleSceneILHeight.Height / 2, Surface);
                    }

                    if (TitleSceneIL[5] != null)
                    {
                        var titleSceneILHeight = TitleSceneIL[5];
                        var titleSceneILWidth = TitleSceneIL[5];
                        TitleSceneIL[5].Show(X + 75 - titleSceneILWidth.Width / 2, Y - titleSceneILHeight.Height / 2, Surface);
                    }

                    break;
                case 1:
                    var northAmerica = TranslateRStr(NorthAmericaRStr);
                    var titleSceneIL = TitleSceneIL[6];
                    FCreditsFont.SetTextCentered(X - 75, Y - titleSceneIL.Height / 2 - 20, northAmerica, Surface);
                    var europe = TranslateRStr(EuropeRStr);
                    var titleSceneIL2 = TitleSceneIL[6];
                    FCreditsFont.SetTextCentered(X + 75 + 15, Y - titleSceneIL2.Height / 2 - 20, europe, Surface);
                    if (TitleSceneIL[6] != null)
                    {
                        var titleSceneILHeight = TitleSceneIL[6];
                        var titleSceneILWidth = TitleSceneIL[6];
                        TitleSceneIL[6].Show(X - 75 - titleSceneILWidth.Width / 2, Y - titleSceneILHeight.Height / 2 + 20, Surface);
                    }

                    if (TitleSceneIL[2] != null)
                    {
                        var titleSceneILHeight = TitleSceneIL[2];
                        var titleSceneILWidth = TitleSceneIL[2];
                        TitleSceneIL[2].Show(X + 75 + 15 - titleSceneILWidth.Width / 2, Y - titleSceneILHeight.Height / 2 + 20, Surface);
                    }

                    break;
                case 2:
                    switch (Frame)
                    {
                        case >= 45 and <= 55:
                            Opacity = (Frame - 45) * 10;
                            break;
                        case >= 121 and <= 130:
                            Opacity = 100 - (Frame - 120) * 10;
                            break;
                        default:
                            Opacity = 101;
                            break;
                    }

                    if (Opacity <= 100)
                    {
                        Info.ShowMode = TShowMode.smBlended;
                        Info.BlendMode = TBlendMode.bmAlpha;
                        Info.Blend = Opacity;
                        FCreditsFont.SetTextEx(X, Y - FCreditsFont.Height / 2, "Copyright (C) 1999,2000 Triumph Studios", Surface, TSet<TSetTextModeElement>.Of(TSetTextModeElement.stmCenter), Info);
                    }
                    else
                    {
                        FCreditsFont.SetTextCentered(X, Y - FCreditsFont.Height / 2, "Copyright (C) 1999,2000 Triumph Studios", Surface);
                    }

                    break;
            }
        }

        if (Page == 3)
        {
            FShowIntro = false;
        }
    }

    // AoWTitleScene.pas:212
    public void DrawScene(ref TSurfaceDesc Surface)
    {
        int X;
        int Y;
        int Frame;
        int Spacing;
        int Image;
        TRect R;
        FSceneRect.Left = 0;
        FSceneRect.Right = Surface.Width;
        FSceneRect.Top = Surface.Height / 2 - 240;
        FSceneRect.Bottom = FSceneRect.Top + 480;
        if (FRedrawBackground)
        {
            R = FSceneRect;
            var titleSceneIL = TitleSceneIL[0];
            R.Top += titleSceneIL.Height;
            ColorFillSD(R, 0, Surface);
        }

        Y = FSceneRect.Top;
        Frame = FFrame - 25;
        if (Frame < 0)
        {
            Frame = 0;
        }

        Frame = -((Frame + 490) % TitleSceneIL[0].Width);
        TitleSceneIL[0].Show(Frame, Y, Surface);
        var titleSceneILWidth = TitleSceneIL[0];
        TitleSceneIL[0].Show(Frame + titleSceneILWidth.Width, Y, Surface);
        if (!FShowCredits && FFrame > 25)
        {
            if (FFrame < 75)
            {
                Frame = (FFrame - 25) * 2;
            }
            else
            {
                if (FTotalFrame <= 730)
                {
                    Frame = 100;
                }
                else
                {
                    Frame = (750 - FTotalFrame) * 5;
                }
            }

            var titleSceneIL = TitleSceneIL[1];
            X = (FSceneRect.Right - FSceneRect.Left - titleSceneIL.Width) / 2;
            if (Frame < 100)
            {
                var lookupTable = AlphaBlendTable.GetLookupTables(Frame);
                TitleSceneIL[1].ShowBlended(X, Y + 10, Surface, lookupTable);
            }
            else
            {
                TitleSceneIL[3].Show(X, Y + 10, Surface);
            }
        }

        Spacing = 120;
        X = (TitleSceneIL[10].Width + FSceneRect.Left + FSceneRect.Right) / 2 - 320;
        for (int I = 0; I <= 4; I++)
        {
            if (I == FPressedButton && I == HitTest())
            {
                Image = 2;
            }
            else
            {
                if (FPressedButton == -1 && I == FHoverButton)
                {
                    Image = 1;
                }
                else
                {
                    Image = 0;
                }
            }

            TitleSceneIL[I * 3 + 10 + Image].Show(X + Spacing * I, Y + 355, Surface);
            if (I == HitTest())
            {
                FFont.Color = 16777215;
                var translateRStr = TranslateRStr(AoWTitleSceneUnit.TitleButtons[I]);
                var titleSceneIL = TitleSceneIL[10];
                FFont.SetTextCentered(X + Spacing * I + titleSceneIL.Width / 2, Y + 445, translateRStr, Surface);
            }

            if (AoWEngine.FAnimationDetail == 2)
            {
                TitleSceneIL[25].Show(X + Spacing * I, Y + 355, Surface);
            }
        }

        if (FShowCredits)
        {
            DrawCredits(ref Surface);
        }

        if (FShowIntro)
        {
            DrawIntro(ref Surface);
        }
    }

    // AoWTitleScene.pas:261
    public void HideScene()
    {
        Window.Visible = false;
    }

    // AoWTitleScene.pas:266
    public void ResetScene()
    {
        FHoverButton = -1;
        FFrame = 0;
        FTotalFrame = 0;
        FShowCredits = false;
        // { Original repeats the total-frame reset. }
        FTotalFrame = 0;
        FShowIntro = false;
    }

    // AoWTitleScene.pas:276
    public void AdvanceFrame()
    {
        FFrame += 1;
        FTotalFrame += 1;
        if (!FShowCredits && FTotalFrame == 750)
        {
            FCreditPage = 0;
            FCreditFrame = 0;
            FShowCredits = true;
        }

        if (!FShowIntro && FTotalFrame == 300)
        {
            FIntroFrame = 0;
            FShowIntro = true;
        }
    }

    // AoWTitleScene.pas:293
    public int HitTest()
    {
        int Result;
        bool Found;
        int X;
        int Spacing;
        if (FSceneRect.Bottom - 125 < FMouseY && FMouseY < FSceneRect.Bottom)
        {
            Spacing = 120;
            X = (TitleSceneIL[10].Width + (FSceneRect.Left + FSceneRect.Right)) / 2 - 320;
            Result = 5;
            Found = false;
            while (Result != 0 && !Found)
            {
                Result -= 1;
                Found = Spacing * Result + X < FMouseX && TitleSceneIL[10].Width + X + Spacing * Result > FMouseX;
            }

            if (!Found)
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // AoWTitleScene.pas:315
    public void WindowDrawSurfaceEx(TSurface Surface, ref bool Drawn)
    {
        TSurfaceDesc SD = default;
        try
        {
            if (Succeeded(Surface.Lock(ref SD)))
            {
                DrawScene(ref SD);
                if (AoWEngine.RegisteredVersion() && !FShowCredits)
                {
                    LegalNotice.Draw(SD);
                    StrLbl.Draw(SD);
                    NameLbl.Draw(SD);
                }

                Version.Draw(SD);
                Surface.Unlock();
            }

            Drawn = true;
        }
        catch
        {
            throw Exception.Create("Failure during TitleScene DrawSurface");
        }
    }

    // AoWTitleScene.pas:338
    public void TitleSceneCreate()
    {
        TStringList Lines;
        ResetScene();
        var registeredTo = AoWEngine.RegisteredTo();
        NameLbl.Text = registeredTo;
        var versionStr = AoWEngine.GetVersionStr();
        var format = AoWEUnit.Format(TranslateRStr(VersionXRStr), [versionStr]);
        Version.Text = format;
        FCredits = new TList();
        Lines = new TStringList();
        Lines.Add(TranslateRStr(DesignerAndDirectorRStr));
        Lines.Add("Lennart Sas");
        Lines.Add("");
        Lines.Add(TranslateRStr(AdditionalDesignRStr));
        Lines.Add("Arno van Wingerden");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(LeadProgrammerRStr));
        Lines.Add("Arno van Wingerden");
        Lines.Add("");
        Lines.Add(TranslateRStr(ProgrammerRStr));
        Lines.Add("Josh Farley");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(ArtistsRStr));
        Lines.Add("Lennart Sas");
        Lines.Add("Thomas Cardin");
        Lines.Add("Roy Postma");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(LeadScenarioDesignerRStr));
        Lines.Add("Menno Sas");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(ScenarioDesignersRStr));
        Lines.Add("Doug Gibson");
        Lines.Add("Arnout Sas");
        Lines.Add("Hugo van Wingerden");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(WriterRStr));
        Lines.Add("Raymond Bingham");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(MusicRStr));
        Lines.Add("Michiel van den Bos");
        Lines.Add("");
        Lines.Add(TranslateRStr(SoundFXRStr));
        Lines.Add("Nando Eweg");
        FCredits.Add(Lines);
        Lines.Add("");
        Lines.Add(TranslateRStr(IntroVoiceRStr));
        Lines.Add("John McGinn (English)");
        Lines.Add("Norbert Gastell (German)");
        Lines = new TStringList();
        Lines.Add(TranslateRStr(GalaxySoundSystemRStr));
        Lines.Add("Carlo Vogelsang");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(InternetMailSuiteRStr));
        Lines.Add("Argo Software Design");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(QACoreRStr));
        Lines.Add("Cliff Benjamin");
        Lines.Add("Ian Calderon");
        Lines.Add("Ted Stephenson");
        Lines.Add("Hugo van Wingerden");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr("Take 2 Europe"));
        Lines.Add("");
        Lines.Add("Producer");
        Lines.Add("Ken Jordan");
        Lines.Add("");
        Lines.Add("Manuals + Translations");
        Lines.Add("Frazer Nash");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add("Tarantula Studios testing suite");
        Lines.Add("");
        Lines.Add("Test Manager");
        Lines.Add("Mark Lloyd");
        Lines.Add("");
        Lines.Add("Lead Tester");
        Lines.Add("Paul Byers");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add("Testers");
        Lines.Add("");
        Lines.Add("Jim Thompson");
        Lines.Add("Kit Brown");
        Lines.Add("Julian Turner");
        Lines.Add("Jim Cree");
        Lines.Add("Lee Johnson");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(BizzRStr));
        Lines.Add("Jay Wilbur");
        Lines.Add("Mark Rein");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(TranslateRStr(SpecialThanksRStr));
        Lines.Add("Eelco van der Snoek");
        Lines.Add("Doug Gibson");
        Lines.Add("Dietmer Tan");
        Lines.Add("Arjan Brussee");
        Lines.Add("Daniel Cook");
        FCredits.Add(Lines);
    }

    // AoWTitleScene.pas:448
    public void TitleSceneDestroy()
    {
        for (int I = FCredits.Count - 1; I >= 0; I--)
        {
            ((TObject)FCredits.List[I]).Free();
        }

        FCredits.Free();
    }

    // AoWTitleScene.pas:456
    public void WindowUpdate()
    {
        Window.FSurfaceValid = false;
        if (!FileDlgWindow.FileDialog.Visible && !E2GlobalsUnit.ProgressDialog.ProgDlg.Visible)
        {
            AdvanceFrame();
        }
    }

    // AoWTitleScene.pas:462
    public void WindowMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FMouseX = X;
        FMouseY = Y;
        FMouseButton = Button;
        FPressedButton = HitTest();
    }

    // AoWTitleScene.pas:470
    public void WindowMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        FMouseX = X;
        FMouseY = Y;
        if (FHoverButton != -1)
        {
            FRedrawBackground = true;
        }

        FHoverButton = HitTest();
    }
}

public static partial class AoWTitleSceneUnit
{
    public static readonly string TitleNewGameRStr = "New Game";
    public static readonly string TitleCampaignRStr = "Campaign";
    public static readonly string TitleTutorialRStr = "Tutorial";
    public static readonly string TitleLoadGameRStr = "Load Game";
    public static readonly string TitleExitRStr = "Exit";
    public static FixedArray_string_5 TitleButtons = FixedArray_string_5.From([]);

    public static void Initialize()
    {
        TitleButtons[0] = TitleNewGameRStr;
        TitleButtons[1] = TitleCampaignRStr;
        TitleButtons[2] = TitleTutorialRStr;
        TitleButtons[3] = TitleLoadGameRStr;
        TitleButtons[4] = TitleExitRStr;
    }
}
