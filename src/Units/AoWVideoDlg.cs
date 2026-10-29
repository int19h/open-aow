// Translated from AoWVideoDlg.pas.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWVideoDlg
{
    // { TVideoDialog: size $A4; $90 full screen, $91 change flag, $9C/$A0 display size. VMT 0040ADD4. }
    public partial class TVideoDialog : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton CloseBut;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage TCeltLR;
        // { +0058 }
        public Aow.Units.AoWFrame.TAoWFrame TopLine;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel VideoLbl;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage TCeltPattern;
        // { +0064 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0068 }
        public Aow.Units.AoWImage.TAOWImage TCeltLL;
        // { +006C }
        public Aow.Units.AoWListBox.TAOWListBox DisplayModesLB;
        // { +0070 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox ResCB;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel ResSwLbl;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton ApplyBtn;
        // { +007C }
        public Aow.Units.AoWPanel.TAOWPanel T3LBPnl;
        // { +0080 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar VideoSB;
        // { +0084 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0088 }
        public Aow.Units.GFXE.TDisplayModeList FDisplayModes;
        // { $8C }
        public Aow.Units.AoWVideoDlg.TScreenMode FFullScreen;
        // { $90; 0040B034 }
        // { conflict: FFullScreen: Byte; }
        public bool FModeChanged;
        // { $91; 0040B034 }
        public ushort FReserved92;
        public int FSceneWidth;
        public int FSceneHeight;
        // { $94/$98; 0040B06C }
        public int FDisplayWidth;
        // { $9C; 0040B04C }
        // { conflict: FWidth: Integer; }
        public int FDisplayHeight;
        // AoWVideoDlg.pas:366
        // { $A0; 0040B04C }
        // { conflict: FHeight: Integer; }
        public void SetSceneSize(int W, int H)
        {
            FSceneWidth = W;
            FSceneHeight = H;
            if (FFullScreen == Aow.Units.AoWVideoDlg.TScreenMode.scmTitle)
            {
                FModeChanged = true;
            }
        }

        // AoWVideoDlg.pas:416
        public void ShowModes()
        {
            Dlg.Visible = true;
            UpdateModes();
        }

        // AoWVideoDlg.pas:172
        public void UpdateModes()
        {
            int I = default;
            Aow.Units.GFXE.TDisplayModeItem Mode = default;
            Aow.Rtl.AnsiString S = default;
            DisplayModesLB.Items.Clear();
            var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DesktopWindowRStr);
            DisplayModesLB.Items.AddObject(call__1, Aow.Rtl.Rtl.IntToObject((int)-1));
            {
                I = 0;
                int end__2 = FDisplayModes.Count - 1;
                for (; I <= end__2; I += 1)
                {
                    var call__3 = FDisplayModes.Items[I];
                    Mode = call__3;
                    if ((Mode.Width >= 640) && (Mode.Height >= 480))
                    {
                        var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FullScreenRStr);
                        DisplayModesLB.Items.AddObject((((call__4 + AnsiString.FromBytes(58)) + Aow.Rtl.Rtl.IntToStr(Mode.Width)) + AnsiString.FromBytes(120)) + Aow.Rtl.Rtl.IntToStr(Mode.Height), Aow.Rtl.Rtl.IntToObject((int)I));
                    }
                }
            }

            var call__5 = CurrentMode();
            DisplayModesLB.Selected = call__5;
            if (DisplayModesLB.Selected != -1)
            {
                var call__6 = DisplayModesLB.Items[DisplayModesLB.Selected];
                S = call__6;
                S = ((S + ((AnsiString)" (")) + Aow.Rtl.Rtl.LowerCase(Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CurrentRStr))) + AnsiString.FromBytes(41);
                DisplayModesLB.Items[DisplayModesLB.Selected] = S;
            }
        }

        // AoWVideoDlg.pas:195
        public void VideoDlgWindowCreate()
        {
            var call__7 = Aow.Units.GFXE.TDisplayModeList.Create();
            FDisplayModes = call__7;
            Aow.Units.GFXE.GFXE.ListDisplayModes(FDisplayModes, 0, 0, 16);
        }

        // AoWVideoDlg.pas:201
        // { draft }
        public void VideoDlgWindowDestroy()
        {
            FDisplayModes.Free();
        }

        // AoWVideoDlg.pas:206
        // { draft }
        public void CloseButClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWVideoDlg.pas:212
        // { draft }
        public void ApplyBtnClick()
        {
            int I = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (DisplayModesLB.Selected != -1)
            {
                var call__8 = DisplayModesLB.Items.Objects[DisplayModesLB.Selected];
                I = Aow.Rtl.Rtl.ObjectToInt(call__8);
                if (I != -1)
                {
                    var call__9 = FDisplayModes.Items[I];
                    FDisplayWidth = call__9.Width;
                    var call__10 = FDisplayModes.Items[I];
                    FDisplayHeight = call__10.Height;
                    ChangeResolution(FDisplayWidth, FDisplayHeight);
                    FFullScreen = Aow.Units.AoWVideoDlg.TScreenMode.scmGame;
                }
                else
                {
                    FFullScreen = Aow.Units.AoWVideoDlg.TScreenMode.scmWindowed;
                    ChangeResolution(0, 0);
                }
            }
        }

        // AoWVideoDlg.pas:233
        // { draft }
        public void ResCBChange(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }

            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetAllowResolutionSwitches(ResCB.Checked);
        }

        // AoWVideoDlg.pas:239
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FAligned = false;
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }

        // AoWVideoDlg.pas:245
        public void DlgShow()
        {
            var call__11 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetAllowResolutionSwitches();
            Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.ResCB.Checked = call__11;
        }
    }

    public enum TScreenMode : byte
    {
        scmWindowed = 0,
        scmGame = 1,
        scmTitle = 2
    }

    // { draft }
    // { TWMCampaignScenarioPage: size $18; $08 image library, $0C image index, $10 heading, $14 text. VMT 0040B804. }
    public partial class TWMCampaignScenarioPage : Aow.Units.Engine.TEObject
    {
        public Aow.Rtl.AnsiString FImageLibrary;
        // { $08 }
        // { Managed string at $08. }
        public int FImageIndex;
        // { $0C }
        // { Integer at $0C. }
        public Aow.Rtl.AnsiString FHeading;
        // { $10 }
        // { Managed string at $10. }
        public Aow.Rtl.TStringList FText;
        // AoWVideoDlg.pas:257
        // { $14 }
        public override void Destroy()
        {
            FText.Free();
            base.Destroy();
        }

        // AoWVideoDlg.pas:268
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FText, 10);
            Stream.rwString(ref FHeading, 11);
            if (!Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smTextOnly))
            {
                Stream.rwString(ref FImageLibrary, 12);
            }

            Stream.rwInteger(ref FImageIndex, 13);
        }

        // AoWVideoDlg.pas:250
        public TWMCampaignScenarioPage(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__12 = new Aow.Rtl.TStringList();
            FText = call__12;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // AoWVideoDlg.pas:264
            public override int ClassID()
            {
                int Result = default;
                Result = 2248706;
                return Result;
            }

            // AoWVideoDlg.pas:78
            public new Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TWMCampaignScenarioPageList (instance size 12, VMT 0040B8C4) }
    public partial class TWMCampaignScenarioPageList : Aow.Units.Engine.TENode
    {
        public TWMCampaignScenarioPageList(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

    }

    // { TWMCampaignScenario: size $3C; $14 image library, $18 path text, $1C description, $20 flag, $24/$28 position, $2C paths, $30/$34 pages, $38 map kind. VMT 0040B9A8. }
    public partial class TWMCampaignScenario : Aow.Units.AoWE.TAoWCampaignScenario
    {
        public Aow.Rtl.AnsiString FImageLibrary;
        // { $14 }
        // { Managed string at $14. }
        public Aow.Rtl.AnsiString FPathDescription;
        // { $18 }
        // { Managed string at $18. }
        public Aow.Rtl.AnsiString FDescription;
        // { $1C }
        // { Managed string at $1C. }
        public bool F_WMCampaignScenario_20;
        // { $20; 0040BCE7 rwBool }
        public Aow.Units.FixedArray_byte_3 F_WMCampaignScenario_21;
        public int FMapX;
        // { $24 }
        // { Integer at $24. }
        public int FMapY;
        // { $28 }
        // { Integer at $28. }
        public Aow.Units.Engine.TIntegerList FPaths;
        // { $2C }
        public Aow.Units.AoWVideoDlg.TWMCampaignScenarioPageList FIntroPages;
        // { $30 }
        public Aow.Units.AoWVideoDlg.TWMCampaignScenarioPageList FOutroPages;
        // { $34 }
        public byte FMapKind;
        // { $38 }
        public Aow.Units.FixedArray_byte_3 F_WMCampaignScenario_39;
        // AoWVideoDlg.pas:295
        public override void Destroy()
        {
            FIntroPages.Free();
            FOutroPages.Free();
            FPaths.Free();
            base.Destroy();
        }

        // AoWVideoDlg.pas:308
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smTextOnly))
            {
                Stream.rwString(ref FImageLibrary, 12);
            }

            Stream.rwString(ref FPathDescription, 13);
            Stream.rwString(ref FDescription, 14);
            Stream.rwBool(ref F_WMCampaignScenario_20, 15);
            Stream.rwInteger(ref FMapX, 16);
            Stream.rwInteger(ref FMapY, 17);
            Stream.rwEObject(FIntroPages, 19);
            Stream.rwEObject(FOutroPages, 20);
            Stream.rwStreamProc(21, FPaths.LoadFromStream, FPaths.SaveToStream);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FMapKind, 1), 1, 22);
        }

        // AoWVideoDlg.pas:278
        public TWMCampaignScenario(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            int I = default;
            var call__13 = new Aow.Units.AoWVideoDlg.TWMCampaignScenarioPageList(null);
            FIntroPages = call__13;
            var call__14 = new Aow.Units.AoWVideoDlg.TWMCampaignScenarioPageList(null);
            FOutroPages = call__14;
            {
                I = 0;
                byte end__15 = 9;
                for (; I <= end__15; I += 1)
                {
                    new Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage(FIntroPages);
                    new Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage(FOutroPages);
                }
            }

            var call__16 = new Aow.Units.Engine.TIntegerList();
            FPaths = call__16;
            FPaths.Default = -1;
        }

        public new partial class Meta : Aow.Units.AoWE.TAoWCampaignScenario.Meta
        {
            // AoWVideoDlg.pas:304
            public override int ClassID()
            {
                int Result = default;
                Result = 2248705;
                return Result;
            }

            // AoWVideoDlg.pas:109
            public new Aow.Units.AoWVideoDlg.TWMCampaignScenario Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.AoWVideoDlg.TWMCampaignScenario)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.AoWVideoDlg.TWMCampaignScenario => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TWMCampaign: size $40; $34 faction, $38 state, $3C page. VMT 0040BA6C. }
    public partial class TWMCampaign : Aow.Units.AoWE.TAoWCampaign
    {
        public int FFaction;
        // { $34; 0041D90E }
        public int FState;
        // { $38; 0041D968 }
        public int FPage;
        // AoWVideoDlg.pas:329
        // { $3C; 0041E43C }
        public override void Destroy()
        {
            base.Destroy();
        }

        // AoWVideoDlg.pas:337
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FFaction, 30);
        }

        // AoWVideoDlg.pas:324
        public TWMCampaign(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
        }

        // AoWVideoDlg.pas:348
        public override void Clear()
        {
            base.Clear();
        }

        // AoWVideoDlg.pas:343
        public override Aow.Units.AoWE.TAoWCampaignScenario CreateScenario()
        {
            Aow.Units.AoWE.TAoWCampaignScenario Result = default;
            Result = new Aow.Units.AoWVideoDlg.TWMCampaignScenario(null);
            return Result;
        }

        // AoWVideoDlg.pas:353
        public override Aow.Rtl.AnsiString GetAutoSaveFileName(Aow.Units.AoWE.TCampaignAutoSaveType Auto)
        {
            Aow.Rtl.AnsiString Result = default;
            if (Auto == Aow.Units.AoWE.TCampaignAutoSaveType.casScenarioFinished)
            {
                Result = Aow.Units.AoWE.AoWE.AoWHSMap.FMapName;
                if (FFaction == 0)
                {
                    Result = (Result + AnsiString.FromBytes(95)) + Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WonRStr);
                }
                else
                {
                    // { Original uses Won for both factions. }
                    Result = (Result + AnsiString.FromBytes(45)) + Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WonRStr);
                }

                Result = Result + ((AnsiString)".acg");
            }
            else
            {
                Result = base.GetAutoSaveFileName(Auto);
            }

            return Result;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // AoWVideoDlg.pas:334
            public override int ClassID()
            {
                int Result = default;
                Result = 2248704;
                return Result;
            }

            // AoWVideoDlg.pas:120
            public new Aow.Units.AoWVideoDlg.TWMCampaign Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.AoWVideoDlg.TWMCampaign)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.AoWVideoDlg.TWMCampaign => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class AoWVideoDlg
    {
        public static Aow.Units.AoWVideoDlg.TVideoDialog VideoDialog;
        // AoWVideoDlg.pas:422
        public static void RegisterWorldMapCampaign(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWVideoDlg.TWMCampaignScenario>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWVideoDlg.TWMCampaign>()]);
        }
    }
}
