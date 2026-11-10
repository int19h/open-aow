// Translated from AoWTitleScene.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
                    Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.CoDevelopedByRStr);
                    break;
                case 1:
                    Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.CoPublishedByRStr);
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
                    var northAmerica = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.NorthAmericaRStr);
                    var titleSceneIL = TitleSceneIL[6];
                    FCreditsFont.SetTextCentered(X - 75, Y - titleSceneIL.Height / 2 - 20, northAmerica, Surface);
                    var europe = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.EuropeRStr);
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
            Aow.Game.Draw.PixelFormats.ColorFillSD(R, 0, Surface);
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
                var lookupTable = Aow.Game.Draw.GfxState.AlphaBlendTable.GetLookupTables(Frame);
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
                var translateRStr = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TitleCaptions.TitleButtons[I]);
                var titleSceneIL = TitleSceneIL[10];
                FFont.SetTextCentered(X + Spacing * I + titleSceneIL.Width / 2, Y + 445, translateRStr, Surface);
            }

            if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail == 2)
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
                if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.RegisteredVersion() && !FShowCredits)
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
            throw new InvalidOperationException("Failure during TitleScene DrawSurface");
        }
    }

    // AoWTitleScene.pas:338
    public void TitleSceneCreate()
    {
        TStringList Lines;
        ResetScene();
        var registeredTo = Aow.Game.Magic.Abilities.GameTables.AoWEngine.RegisteredTo();
        NameLbl.Text = registeredTo;
        var versionStr = Aow.Game.Magic.Abilities.GameTables.AoWEngine.GetVersionStr();
        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.VersionXRStr), [versionStr]);
        Version.Text = format;
        FCredits = new TList();
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.DesignerAndDirectorRStr));
        Lines.Add("Lennart Sas");
        Lines.Add("");
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.AdditionalDesignRStr));
        Lines.Add("Arno van Wingerden");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.LeadProgrammerRStr));
        Lines.Add("Arno van Wingerden");
        Lines.Add("");
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.ProgrammerRStr));
        Lines.Add("Josh Farley");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.ArtistsRStr));
        Lines.Add("Lennart Sas");
        Lines.Add("Thomas Cardin");
        Lines.Add("Roy Postma");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.LeadScenarioDesignerRStr));
        Lines.Add("Menno Sas");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.ScenarioDesignersRStr));
        Lines.Add("Doug Gibson");
        Lines.Add("Arnout Sas");
        Lines.Add("Hugo van Wingerden");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.WriterRStr));
        Lines.Add("Raymond Bingham");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.MusicRStr));
        Lines.Add("Michiel van den Bos");
        Lines.Add("");
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.SoundFXRStr));
        Lines.Add("Nando Eweg");
        FCredits.Add(Lines);
        Lines.Add("");
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.IntroVoiceRStr));
        Lines.Add("John McGinn (English)");
        Lines.Add("Norbert Gastell (German)");
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.GalaxySoundSystemRStr));
        Lines.Add("Carlo Vogelsang");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.InternetMailSuiteRStr));
        Lines.Add("Argo Software Design");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.QACoreRStr));
        Lines.Add("Cliff Benjamin");
        Lines.Add("Ian Calderon");
        Lines.Add("Ted Stephenson");
        Lines.Add("Hugo van Wingerden");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr("Take 2 Europe"));
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
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.BizzRStr));
        Lines.Add("Jay Wilbur");
        Lines.Add("Mark Rein");
        FCredits.Add(Lines);
        Lines = new TStringList();
        Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TTitleScene.SpecialThanksRStr));
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
            (FCredits.List[I] as IDisposable)?.Dispose();
        }

        FCredits?.Dispose();
    }

    // AoWTitleScene.pas:456
    public void WindowUpdate()
    {
        Window.FSurfaceValid = false;
        if (!Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible && !Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.ProgDlg.Visible)
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

    private static readonly string AdditionalDesignRStr = "Additional Design";

    private static readonly string ArtistsRStr = "Artists";

    private static readonly string BizzRStr = "Bizz";

    private static readonly string CampaignRStr = "Campaign";

    private static readonly string CoDevelopedByRStr = "Co-developed by:";

    private static readonly string CoPublishedByRStr = "\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00";

    private static readonly string DesignerAndDirectorRStr = "Designer & Director";

    private static readonly string EuropeRStr = "\x00\x00\x00\x00\x00\x00";

    private static readonly string GalaxySoundSystemRStr = "Galaxy Sound System";

    private static readonly string InternetMailSuiteRStr = "Internet Mail Suite";

    private static readonly string IntroVoiceRStr = "Intro Voice";

    private static readonly string LeadProgrammerRStr = "Lead Programmer";

    private static readonly string LeadScenarioDesignerRStr = "Lead Scenario Designer";

    private static readonly string MusicRStr = "Music";

    private static readonly string NorthAmericaRStr = "\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00";

    private static readonly string ProgrammerRStr = "Programmer";

    private static readonly string QACoreRStr = "QA Core";

    private static readonly string ScenarioDesignersRStr = "Scenario Designers";

    private static readonly string SoundFXRStr = "Sound FX";

    private static readonly string SpecialThanksRStr = "Special Thanks";

    private static readonly string VersionXRStr = "Version: %s";

    private static readonly string WriterRStr = "Writer";

    private static void ShowFactionSelection(TSelFactionWin Dialog)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FactionPnl.Visible = true;
        Dialog.TopLbl.Visible = true;
        var chooseFaction = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.ChooseFactionRStr);
        Dialog.TopLbl.Text = chooseFaction;
        Dialog.LName.Visible = false;
        Dialog.LText.Visible = false;
        Dialog.LBack.Visible = false;
        Dialog.LStart.Visible = false;
        Dialog.LSphere.Visible = false;
        Dialog.DName.Visible = false;
        Dialog.DText.Visible = false;
        Dialog.DBack.Visible = false;
        Dialog.DStart.Visible = false;
        Dialog.DSphere.Visible = false;
        Dialog.FFade = -1;
        Dialog.FactionPnl.ILIndexTexture = 0;
        Dialog.F_SelFactionWin_8C[0] = 0;
    }

    private static void HideGameWindows(TGeneral Controller)
    {
        if (!Controller.FWindowsHidden)
        {
            Controller.FWindowsHidden = true;
            Controller.FInfoVisible = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Visible;
            Controller.FRealmVisible = Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible;
            Controller.FMagicVisible = Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Visible;
            Controller.FUnitVisible = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible;
            Controller.FTargetVisible = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible;
            Controller.FScannerVisible = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible;
            Controller.FControlVisible = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible;
            Controller.FChatVisible = Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Visible;
            Controller.FMapVisible = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
            for (int I = 0, last = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren.Count - 1; I <= last; I++)
            {
                if (Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I] != Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow)
                {
                    ((TAoWComponent)Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I]).Visible = false;
                }
            }
        }
    }

    private static void LoadGame(TGeneral Controller, string FileName)
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
        }

        var loading = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LoadingRStr);
        var fileName = ExtractFileName(FileName);
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Title.Text = loading + ": " + fileName + "...";
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.StatusLbl.Text = "";
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Bar.Position = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Bar.Max = 100;
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.ProgDlg.Visible = true;
        if (!Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.Window.Visible)
        {
            Controller.Manager.OverlayPriority = 5000;
        }

        Aow.Game.Audio.AudioHost.SongPlayer.Stop(false);
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        Aow.Game.Audio.AudioPlayback.FadeMusicVolume(0, 1);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.LoadHSM(FileName, false, (_, Message, Progression) => Controller.MapProgress(Message, Progression), Aow.Formats.GameProfile.WorldMap);
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo && Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FShowAllPlayers)
        {
            CloseGame(Controller);
        }

        Controller.Manager.OverlayPriority = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.ProgDlg.Visible = false;
    }

    private static void StartGame(TGeneral Controller)
    {
        Controller.FWindowsHidden = false;
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGameSongs.Songs.Count != 0)
        {
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.AddStrings(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGameSongs.Songs);
        }
        else
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FStartupSongs.Songs.Count != 0)
            {
                for (int I = 0, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FStartupSongs.Songs.Count - 1; I <= last; I++)
                {
                    var fileNotFound = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(FileNotFoundRStr);
                    if (CompareText(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FStartupSongs.SongNames[I], fileNotFound) != 0)
                    {
                        var song = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FStartupSongs.Songs[I];
                        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add(song);
                    }
                }
            }
            else
            {
                Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add("Songs\\Heartwoo.it");
            }
        }

        Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
        Aow.Game.Audio.AudioPlayback.SetMusicVolume(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.MusicVolume.Position);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.StartMenuBar.Items.Clear();
        var background = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(BackgroundRStr);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.StartMenuBar.Items.Add(background);
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null)
        {
            var campaign = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CampaignRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.StartMenuBar.Items.Add(campaign);
        }

        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.Window.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SetHSMap(Aow.Game.World.Map.TAoWHSMap.AoWHSMap);
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetMap(Aow.Game.World.Map.TAoWHSMap.AoWHSMap);
        var mapLevel = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[0];
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetLevel(mapLevel);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FButtonChecked = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ChatBtn.Visible = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.Connected;
        Controller.Manager.FAligned = false;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FPreviousWidth = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FIncremental = false;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FSized = false;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab.Items.Clear();
        var surface = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(SurfaceRStr);
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab.Items.Add(surface);
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.LevelCount > 1)
        {
            var caverns = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CavernsRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab.Items.Add(caverns);
        }

        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.LevelCount > 2)
        {
            var depths = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(DepthsRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab.Items.Add(depths);
        }

        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab.Selected = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab.FSized = false;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTWidth(0);
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTHeight(0);
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTLeft(-1);
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTTop(-1);
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FUpdateStep = -1;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetMap(Aow.Game.World.Map.TAoWHSMap.AoWHSMap);
        var mapLevel2 = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.MapLevels[0];
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetLevel(mapLevel2);
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SetHSMap(Aow.Game.World.Map.TAoWHSMap.AoWHSMap);
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth - 24;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight - 24;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Activate();
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapArmyDeselected();
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapStructureDeselected();
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TBWorldUpdate();
        Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.UpdatePlayer();
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msActivated))
        {
            var aowTurnButton = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == -1)
            {
                aowTurnButton.FCurPlayer = 12;
                aowTurnButton.FLastPlayer = 12;
                aowTurnButton.FAnimationFrame = aowTurnButton.FCurPlayer * 7;
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.AutoBtn.Visible = false;
            }
            else
            {
                aowTurnButton.FLastPlayer = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FPlayerRace;
                if (aowTurnButton.FLastPlayer == -1)
                {
                    aowTurnButton.FLastPlayer = 12;
                }

                aowTurnButton.FCurPlayer = aowTurnButton.FLastPlayer;
                aowTurnButton.FAnimationFrame = aowTurnButton.FLastPlayer * 7;
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.AutoBtn.Visible = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayMode == TPlayMode.pmSimultaneous;
            }
        }

        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.FWorldClockDelay = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ChatBtn.TextColor = 14;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.FChatFlash = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ChangeResolution(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.FDisplayWidth, Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.FDisplayHeight);
    }

    private static void CloseGame(TGeneral Controller)
    {
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseCampaign();
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetMap(null);
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetMap(null);
        Aow.Game.Audio.AudioHost.SongPlayer.Stop(false);
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory);
        HideGameWindows(Aow.Game.Ui.Dialogs.DialogHosts.General);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.ResetScene();
        Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(Aow.Game.Ui.Dialogs.DialogHosts.TitleScene, true);
    }

    private static readonly string LoadingRStr = "Loading";

    private static readonly string FileNotFoundRStr = "File not found";

    private static readonly string BackgroundRStr = "Background";

    private static readonly string SurfaceRStr = "Surface";

    private static readonly string CavernsRStr = "Caverns";

    private static readonly string DepthsRStr = "Depths";

    internal static void ShowTitleScene(TTitleScene Scene, bool Music)
    {
        if (Music)
        {
            bool condition = Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex == -1;
            if (!condition)
            {
                var count = Aow.Game.Audio.AudioHost.SongPlayer.Songs.Count;
                condition = Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex >= count;
            }

            if (condition || Aow.Game.Audio.AudioHost.SongPlayer.Songs[Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex] != "SONGS\\TITLE.IT")
            {
                Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
                Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
                Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
            }
        }

        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        Scene.TitleSceneIL.LoadDiscardableImageData();
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.SetSceneSize(640, 480);
        Aow.Game.Ui.Dialogs.TVideoDialog.SetVideoMode(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, TScreenMode.scmTitle);
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ApplyMode();
        GameHost.MainForm.Display.FrameRate = 30;
        GameHost.MainForm.DXDC.UseVerticalBlank = true;
        Scene.FRedrawBackground = true;
        Scene.FPressedButton = -1;
        Scene.FHoverButton = -1;
        Scene.FFont = Aow.Game.Ui.Dialogs.DialogHosts.FontModule.AoW15WhiteGrey;
        Scene.FCreditsFont = Aow.Game.Ui.Dialogs.DialogHosts.FontModule.AoW15WhiteGrey;
        Scene.Window.WinManager.SetILBIndex(4);
        Scene.Window.Visible = true;
    }

}
