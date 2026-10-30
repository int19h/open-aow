// Translated from AoWTitleScene.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AowUserWindow;
using static Aow.Units.AowUserWindow.AowUserWindow;
using Aow.Units.AoWMessageDlg;
using static Aow.Units.AoWMessageDlg.AoWMessageDlg;
using Aow.Units.AoWSelFactionWin;
using static Aow.Units.AoWSelFactionWin.AoWSelFactionWin;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;

namespace Aow.Units.AoWTitleScene
{
    // frame counters, mouse button, mouse position, selected/pressed/hover button, scene rectangle,
    // redraw/credits, credits, credit page/frame, intro flag/frame, font
    public partial class TTitleScene : Aow.Rtl.TDataModule
    {
        public TTitleScene(TComponent owner) : base(owner)
        { }

        public Aow.Units.ImageLib.TImageLibrary TitleSceneIL;
        public Aow.Units.AowUserWindow.TAoWUserWindow Window;
        public Aow.Units.AoWMemo.TAOWMemo LegalNotice;
        public Aow.Units.AoWLabel.TAOWLabel StrLbl;
        public Aow.Units.AoWLabel.TAOWLabel NameLbl;
        public Aow.Units.AoWLabel.TAOWLabel Copyright;
        public Aow.Units.AoWLabel.TAOWLabel Version;
        public int FFrame;
        public int FTotalFrame;
        public Aow.Rtl.TMouseButton FMouseButton;
        public int FMouseX;
        public int FMouseY;
        public int FSelectedButton;
        public int FPressedButton;
        public int FHoverButton;
        public Aow.Rtl.TRect FSceneRect;
        public bool FRedrawBackground;
        public bool FShowCredits;
        public Aow.Rtl.TList FCredits;
        public int FCreditPage;
        public int FCreditFrame;
        public bool FShowIntro;
        public Aow.Units.FixedArray_byte_3 FPadA5;
        public int FIntroFrame;
        public Aow.Units.ImageLib.TImageLibraryFont FFont;
        public Aow.Units.ImageLib.TImageLibraryFont FCreditsFont;
        // AoWTitleScene.pas:103
        public void DrawCredits(ref Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int X = default;
            int Y = default;
            int I = default;
            int Duration = default;
            int Alpha = default;
            Aow.Units.ImageLib.TShowInfo Info = default;
            Aow.Rtl.TStrings Lines = default;
            FCreditFrame += 1;
            Info.ShowMode = Aow.Units.ImageLib.TShowMode.smBlended;
            Info.BlendMode = Aow.Units.ImageLib.TBlendMode.bmAlpha;
            var call__1 = FCredits[FCreditPage];
            Lines = (Aow.Rtl.TStrings)call__1;
            X = Surface.Width / 2;
            var call__2 = Lines.Count;
            Y = (Surface.Height / 2) - ((((call__2 * 25) + FCreditsFont.Height) / 2) + 35);
            var call__3 = Lines.Count;
            Duration = ((call__3 * 30) + 20) + 50;
            var call__4 = Lines.Count;
            if (call__4 > 0)
            {
                Alpha = FCreditFrame;
                if (Alpha > Duration)
                {
                    Alpha = 100 - ((Alpha - Duration) * 5);
                }
                else
                {
                    Alpha = Alpha * 10;
                }

                Info.BlendPercentage = Alpha;
                if (Alpha > 0)
                {
                    if (Alpha < 100)
                    {
                        var call__5 = Lines[0];
                        FCreditsFont.SetTextEx(X, Y, call__5, Surface, TSet<Aow.Units.ImageLib.TSetTextModeElement>.Of(Aow.Units.ImageLib.TSetTextModeElement.stmCenter), Info);
                    }
                    else
                    {
                        var call__6 = Lines[0];
                        FCreditsFont.SetTextCentered(X, Y, call__6, Surface);
                    }
                }

                {
                    I = 1;
                    int end__7 = Lines.Count - 1;
                    for (; I <= end__7; I += 1)
                    {
                        if (FCreditFrame > Duration)
                        {
                            Alpha = 100 - ((FCreditFrame - Duration) * 5);
                        }
                        else
                        {
                            Alpha = (FCreditFrame - ((I * 30) + 20)) * 10;
                        }

                        if (Alpha > 0)
                        {
                            Info.BlendPercentage = Alpha;
                            if (Alpha < 100)
                            {
                                var call__8 = Lines[I];
                                FCreditsFont.SetTextEx(X, (Y + (I * 25)) + 5, call__8, Surface, TSet<Aow.Units.ImageLib.TSetTextModeElement>.Of(Aow.Units.ImageLib.TSetTextModeElement.stmCenter), Info);
                            }
                            else
                            {
                                var call__9 = Lines[I];
                                FCreditsFont.SetTextCentered(X, (Y + (I * 25)) + 5, call__9, Surface);
                            }
                        }
                    }
                }
            }

            if (FCreditFrame > (Duration + 20))
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
        public void DrawIntro(ref Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int Page = default;
            int X = default;
            int Y = default;
            int Frame = default;
            int Opacity = default;
            Aow.Units.ImageLib.TShowInfo Info = default;
            Aow.Rtl.AnsiString Text = default;
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
                    Opacity = 100 - ((Frame - 30) * 10);
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
                        Text = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CoDevelopedByRStr);
                        break;
                    case 1:
                        Text = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CoPublishedByRStr);
                        break;
                    case 2:
                        Text = (AnsiString)"";
                        break;
                }

                if (Opacity < 100)
                {
                    Info.ShowMode = Aow.Units.ImageLib.TShowMode.smBlended;
                    Info.BlendMode = Aow.Units.ImageLib.TBlendMode.bmAlpha;
                    Info.BlendPercentage = Opacity;
                    FCreditsFont.SetTextEx(X, Y - (FCreditsFont.Height / 2), Text, Surface, TSet<Aow.Units.ImageLib.TSetTextModeElement>.Of(Aow.Units.ImageLib.TSetTextModeElement.stmCenter), Info);
                }
                else
                {
                    FCreditsFont.SetTextCentered(X, Y - (FCreditsFont.Height / 2), Text, Surface);
                }
            }

            if ((Frame > 45) && (Frame < 130))
            {
                switch (Page)
                {
                    case 0:
                        var call__10 = TitleSceneIL[4];
                        if (call__10 != null)
                        {
                            var call__11 = TitleSceneIL[4];
                            var call__12 = TitleSceneIL[4];
                            var call__13 = TitleSceneIL[4];
                            call__13.Show((X - 75) - (call__12.Width / 2), Y - (call__11.Height / 2), Surface);
                        }

                        var call__14 = TitleSceneIL[5];
                        if (call__14 != null)
                        {
                            var call__15 = TitleSceneIL[5];
                            var call__16 = TitleSceneIL[5];
                            var call__17 = TitleSceneIL[5];
                            call__17.Show((X + 75) - (call__16.Width / 2), Y - (call__15.Height / 2), Surface);
                        }

                        break;
                    case 1:
                        var call__18 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NorthAmericaRStr);
                        var call__19 = TitleSceneIL[6];
                        FCreditsFont.SetTextCentered(X - 75, (Y - (call__19.Height / 2)) - 20, call__18, Surface);
                        var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EuropeRStr);
                        var call__21 = TitleSceneIL[6];
                        FCreditsFont.SetTextCentered((X + 75) + 15, (Y - (call__21.Height / 2)) - 20, call__20, Surface);
                        var call__22 = TitleSceneIL[6];
                        if (call__22 != null)
                        {
                            var call__23 = TitleSceneIL[6];
                            var call__24 = TitleSceneIL[6];
                            var call__25 = TitleSceneIL[6];
                            call__25.Show((X - 75) - (call__24.Width / 2), (Y - (call__23.Height / 2)) + 20, Surface);
                        }

                        var call__26 = TitleSceneIL[2];
                        if (call__26 != null)
                        {
                            var call__27 = TitleSceneIL[2];
                            var call__28 = TitleSceneIL[2];
                            var call__29 = TitleSceneIL[2];
                            call__29.Show(((X + 75) + 15) - (call__28.Width / 2), (Y - (call__27.Height / 2)) + 20, Surface);
                        }

                        break;
                    case 2:
                        switch (Frame)
                        {
                            case >= 45 and <= 55:
                                Opacity = (Frame - 45) * 10;
                                break;
                            case >= 121 and <= 130:
                                Opacity = 100 - ((Frame - 120) * 10);
                                break;
                            default:
                                Opacity = 101;
                                break;
                        }

                        if (Opacity <= 100)
                        {
                            Info.ShowMode = Aow.Units.ImageLib.TShowMode.smBlended;
                            Info.BlendMode = Aow.Units.ImageLib.TBlendMode.bmAlpha;
                            Info.BlendPercentage = Opacity;
                            FCreditsFont.SetTextEx(X, Y - (FCreditsFont.Height / 2), (AnsiString)"Copyright (C) 1999,2000 Triumph Studios", Surface, TSet<Aow.Units.ImageLib.TSetTextModeElement>.Of(Aow.Units.ImageLib.TSetTextModeElement.stmCenter), Info);
                        }
                        else
                        {
                            FCreditsFont.SetTextCentered(X, Y - (FCreditsFont.Height / 2), (AnsiString)"Copyright (C) 1999,2000 Triumph Studios", Surface);
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
        public void DrawScene(ref Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            int Spacing = default;
            int I = default;
            int Image = default;
            Aow.Rtl.TRect R = default;
            FSceneRect.Left = 0;
            FSceneRect.Right = Surface.Width;
            FSceneRect.Top = (Surface.Height / 2) - 240;
            FSceneRect.Bottom = FSceneRect.Top + 480;
            if (FRedrawBackground)
            {
                R = FSceneRect;
                var call__30 = TitleSceneIL[0];
                R.Top += call__30.Height;
                Aow.Units.GFXE.GFXE.ColorFillSD(R, 0, Surface);
            }

            Y = FSceneRect.Top;
            Frame = FFrame - 25;
            if (Frame < 0)
            {
                Frame = 0;
            }

            var call__31 = TitleSceneIL[0];
            Frame = -((Frame + 490) % call__31.Width);
            var call__32 = TitleSceneIL[0];
            call__32.Show(Frame, Y, Surface);
            var call__33 = TitleSceneIL[0];
            var call__34 = TitleSceneIL[0];
            call__34.Show(Frame + call__33.Width, Y, Surface);
            if (!FShowCredits && (FFrame > 25))
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

                var call__35 = TitleSceneIL[1];
                X = ((FSceneRect.Right - FSceneRect.Left) - call__35.Width) / 2;
                if (Frame < 100)
                {
                    var call__36 = Aow.Units.GFXE.GFXE.AlphaBlendTable.GetLookupTables(Frame);
                    var call__37 = TitleSceneIL[1];
                    call__37.ShowBlended(X, Y + 10, Surface, call__36);
                }
                else
                {
                    var call__38 = TitleSceneIL[3];
                    call__38.Show(X, Y + 10, Surface);
                }
            }

            Spacing = 120;
            var call__39 = TitleSceneIL[10];
            X = (((call__39.Width + FSceneRect.Left) + FSceneRect.Right) / 2) - 320;
            {
                I = 0;
                byte end__40 = 4;
                for (; I <= end__40; I += 1)
                {
                    bool bool__41 = (I == FPressedButton);
                    if (bool__41)
                    {
                        var call__42 = HitTest();
                        bool__41 = (I == call__42);
                    }

                    if (bool__41)
                    {
                        Image = 2;
                    }
                    else
                    {
                        if ((FPressedButton == -1) && (I == FHoverButton))
                        {
                            Image = 1;
                        }
                        else
                        {
                            Image = 0;
                        }
                    }

                    var call__43 = TitleSceneIL[((I * 3) + 10) + Image];
                    call__43.Show(X + (Spacing * I), Y + 355, Surface);
                    if (I == HitTest())
                    {
                        FFont.Color = 16777215;
                        var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTitleScene.AoWTitleScene.TitleButtons[I]);
                        var call__45 = TitleSceneIL[10];
                        FFont.SetTextCentered((X + (Spacing * I)) + (call__45.Width / 2), Y + 445, call__44, Surface);
                    }

                    if (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 2)
                    {
                        var call__46 = TitleSceneIL[25];
                        call__46.Show(X + (Spacing * I), Y + 355, Surface);
                    }
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
            if (!FShowCredits && (FTotalFrame == 750))
            {
                FCreditPage = 0;
                FCreditFrame = 0;
                FShowCredits = true;
            }

            if (!FShowIntro && (FTotalFrame == 300))
            {
                FIntroFrame = 0;
                FShowIntro = true;
            }
        }

        // AoWTitleScene.pas:293
        public int HitTest()
        {
            int Result = default;
            bool Found = default;
            int X = default;
            int Spacing = default;
            if (((FSceneRect.Bottom - 125) < FMouseY) && (FMouseY < FSceneRect.Bottom))
            {
                Spacing = 120;
                var call__47 = TitleSceneIL[10];
                X = ((call__47.Width + (FSceneRect.Left + FSceneRect.Right)) / 2) - 320;
                Result = 5;
                Found = false;
                while ((Result != 0) && !Found)
                {
                    Result -= 1;
                    bool bool__48 = (((Spacing * Result) + X) < FMouseX);
                    if (bool__48)
                    {
                        var call__49 = TitleSceneIL[10];
                        bool__48 = (((call__49.Width + X) + (Spacing * Result)) > FMouseX);
                    }

                    Found = bool__48;
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
        public void WindowDrawSurfaceEx(Aow.Units.GFXE.TSurface Surface, ref bool Drawn)
        {
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            try
            {
                var call__50 = Surface.Lock(ref SD);
                var call__51 = Aow.Rtl.Rtl.Succeeded(call__50);
                if (call__51)
                {
                    DrawScene(ref SD);
                    var call__52 = Aow.Units.AoWE.AoWE.AoWEngine.RegisteredVersion();
                    if (call__52 && !FShowCredits)
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
            catch (System.Exception)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"Failure during TitleScene DrawSurface");
            }
        }

        // AoWTitleScene.pas:338
        public void TitleSceneCreate()
        {
            Aow.Rtl.TStringList Lines = default;
            ResetScene();
            var call__53 = Aow.Units.AoWE.AoWE.AoWEngine.RegisteredTo();
            NameLbl.Text = call__53;
            var call__54 = Aow.Units.AoWE.AoWE.AoWEngine.GetVersionStr();
            var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VersionXRStr);
            var call__56 = Aow.Units.AoWE.AoWE.Format(call__55, [call__54]);
            Version.Text = call__56;
            var call__57 = new Aow.Rtl.TList();
            FCredits = call__57;
            Lines = new Aow.Rtl.TStringList();
            var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DesignerAndDirectorRStr);
            Lines.Add(call__58);
            Lines.Add((AnsiString)"Lennart Sas");
            Lines.Add((AnsiString)"");
            var call__59 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AdditionalDesignRStr);
            Lines.Add(call__59);
            Lines.Add((AnsiString)"Arno van Wingerden");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__60 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LeadProgrammerRStr);
            Lines.Add(call__60);
            Lines.Add((AnsiString)"Arno van Wingerden");
            Lines.Add((AnsiString)"");
            var call__61 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProgrammerRStr);
            Lines.Add(call__61);
            Lines.Add((AnsiString)"Josh Farley");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__62 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ArtistsRStr);
            Lines.Add(call__62);
            Lines.Add((AnsiString)"Lennart Sas");
            Lines.Add((AnsiString)"Thomas Cardin");
            Lines.Add((AnsiString)"Roy Postma");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__63 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LeadScenarioDesignerRStr);
            Lines.Add(call__63);
            Lines.Add((AnsiString)"Menno Sas");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__64 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ScenarioDesignersRStr);
            Lines.Add(call__64);
            Lines.Add((AnsiString)"Doug Gibson");
            Lines.Add((AnsiString)"Arnout Sas");
            Lines.Add((AnsiString)"Hugo van Wingerden");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__65 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WriterRStr);
            Lines.Add(call__65);
            Lines.Add((AnsiString)"Raymond Bingham");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MusicRStr);
            Lines.Add(call__66);
            Lines.Add((AnsiString)"Michiel van den Bos");
            Lines.Add((AnsiString)"");
            var call__67 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SoundFXRStr);
            Lines.Add(call__67);
            Lines.Add((AnsiString)"Nando Eweg");
            FCredits.Add(Lines);
            Lines.Add((AnsiString)"");
            var call__68 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IntroVoiceRStr);
            Lines.Add(call__68);
            Lines.Add((AnsiString)"John McGinn (English)");
            Lines.Add((AnsiString)"Norbert Gastell (German)");
            Lines = new Aow.Rtl.TStringList();
            var call__69 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GalaxySoundSystemRStr);
            Lines.Add(call__69);
            Lines.Add((AnsiString)"Carlo Vogelsang");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__70 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InternetMailSuiteRStr);
            Lines.Add(call__70);
            Lines.Add((AnsiString)"Argo Software Design");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__71 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QACoreRStr);
            Lines.Add(call__71);
            Lines.Add((AnsiString)"Cliff Benjamin");
            Lines.Add((AnsiString)"Ian Calderon");
            Lines.Add((AnsiString)"Ted Stephenson");
            Lines.Add((AnsiString)"Hugo van Wingerden");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__72 = Aow.Units.AoWE.AoWE.TranslateRStr((AnsiString)"Take 2 Europe");
            Lines.Add(call__72);
            Lines.Add((AnsiString)"");
            Lines.Add((AnsiString)"Producer");
            Lines.Add((AnsiString)"Ken Jordan");
            Lines.Add((AnsiString)"");
            Lines.Add((AnsiString)"Manuals + Translations");
            Lines.Add((AnsiString)"Frazer Nash");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            Lines.Add((AnsiString)"Tarantula Studios testing suite");
            Lines.Add((AnsiString)"");
            Lines.Add((AnsiString)"Test Manager");
            Lines.Add((AnsiString)"Mark Lloyd");
            Lines.Add((AnsiString)"");
            Lines.Add((AnsiString)"Lead Tester");
            Lines.Add((AnsiString)"Paul Byers");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            Lines.Add((AnsiString)"Testers");
            Lines.Add((AnsiString)"");
            Lines.Add((AnsiString)"Jim Thompson");
            Lines.Add((AnsiString)"Kit Brown");
            Lines.Add((AnsiString)"Julian Turner");
            Lines.Add((AnsiString)"Jim Cree");
            Lines.Add((AnsiString)"Lee Johnson");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__73 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BizzRStr);
            Lines.Add(call__73);
            Lines.Add((AnsiString)"Jay Wilbur");
            Lines.Add((AnsiString)"Mark Rein");
            FCredits.Add(Lines);
            Lines = new Aow.Rtl.TStringList();
            var call__74 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SpecialThanksRStr);
            Lines.Add(call__74);
            Lines.Add((AnsiString)"Eelco van der Snoek");
            Lines.Add((AnsiString)"Doug Gibson");
            Lines.Add((AnsiString)"Dietmer Tan");
            Lines.Add((AnsiString)"Arjan Brussee");
            Lines.Add((AnsiString)"Daniel Cook");
            FCredits.Add(Lines);
        }

        // AoWTitleScene.pas:448
        public void TitleSceneDestroy()
        {
            int I = default;
            {
                I = FCredits.Count - 1;
                byte end__75 = 0;
                for (; I >= end__75; I -= 1)
                {
                    ((Aow.Rtl.TObject)FCredits.List[I]).Free();
                }
            }

            FCredits.Free();
        }

        // AoWTitleScene.pas:456
        public void WindowUpdate()
        {
            Window.FSurfaceValid = false;
            if (!Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible && !Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible)
            {
                AdvanceFrame();
            }
        }

        // AoWTitleScene.pas:462
        public void WindowMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FMouseX = X;
            FMouseY = Y;
            FMouseButton = Button;
            var call__76 = HitTest();
            FPressedButton = call__76;
        }

        // AoWTitleScene.pas:470
        public void WindowMouseMove(Aow.Rtl.TObject Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FMouseX = X;
            FMouseY = Y;
            if (FHoverButton != -1)
            {
                FRedrawBackground = true;
            }

            var call__77 = HitTest();
            FHoverButton = call__77;
        }
    }

    public static partial class AoWTitleScene
    {
        public static readonly Aow.Rtl.AnsiString TitleNewGameRStr = (AnsiString)"New Game";
        public static readonly Aow.Rtl.AnsiString TitleCampaignRStr = (AnsiString)"Campaign";
        public static readonly Aow.Rtl.AnsiString TitleTutorialRStr = (AnsiString)"Tutorial";
        public static readonly Aow.Rtl.AnsiString TitleLoadGameRStr = (AnsiString)"Load Game";
        public static readonly Aow.Rtl.AnsiString TitleExitRStr = (AnsiString)"Exit";
        public static Aow.Units.FixedArray_Aow_Rtl_AnsiString_5 TitleButtons;
        public static void Initialize()
        {
            Aow.Units.AoWTitleScene.AoWTitleScene.TitleButtons[0] = Aow.Units.AoWTitleScene.AoWTitleScene.TitleNewGameRStr;
            Aow.Units.AoWTitleScene.AoWTitleScene.TitleButtons[1] = Aow.Units.AoWTitleScene.AoWTitleScene.TitleCampaignRStr;
            Aow.Units.AoWTitleScene.AoWTitleScene.TitleButtons[2] = Aow.Units.AoWTitleScene.AoWTitleScene.TitleTutorialRStr;
            Aow.Units.AoWTitleScene.AoWTitleScene.TitleButtons[3] = Aow.Units.AoWTitleScene.AoWTitleScene.TitleLoadGameRStr;
            Aow.Units.AoWTitleScene.AoWTitleScene.TitleButtons[4] = Aow.Units.AoWTitleScene.AoWTitleScene.TitleExitRStr;
        }
    }
}
