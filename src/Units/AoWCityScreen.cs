// Translated from AoWCityScreen.pas.
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
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.OverviewUnit;
using static Aow.Units.OverviewUnit.OverviewUnit;

namespace Aow.Units.AoWCityScreen
{
    // { TCityScreen: size $2DC; $2B0 city, $2B4 preview, $2B8/$2BC view, $2C0 action,
//     $2C4 production, $2C8 queue, $2CC mode, $2D0 migrations, $2D4 sort, $2D8 images. VMT 00439F68. }
    public partial class TCityScreen : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWPanel.TAOWPanel UnitPnl;
        // { +0048 }
        public Aow.Units.AoWListBox.TAOWListBox UAbilitiesLB;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage UAttackIcon;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage UDefenseIcon;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage UMovesIcon;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage UDamageIcon;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage UResistanceIcon;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage UHitsIcon;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel UAttack;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel UDefense;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel UMoves;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel UDamage;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel UResistance;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel UHits;
        // { +007C }
        public Aow.Units.AoWPanel.TAOWPanel UStatPnl;
        // { +0080 }
        public Aow.Units.AoWPanel.TAOWPanel UStatP1;
        // { +0084 }
        public Aow.Units.AoWPanel.TAOWPanel UStatP2;
        // { +0088 }
        public Aow.Units.AoWLabel.TAOWLabel UAlignment;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel ULevel;
        // { +0090 }
        public Aow.Units.AoWPanel.TAOWPanel UPicturePnl;
        // { +0094 }
        public Aow.Units.AoWPanel.TAOWPanel UCostPnl;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel StructName;
        // { +009C }
        public Aow.Units.AoWLabel.TAOWLabel CostLbl;
        // { +00A0 }
        public Aow.Units.AoWLabel.TAOWLabel UCost;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel UnitLbl;
        // { +00A8 }
        public Aow.Units.AoWLabel.TAOWLabel TurnsLbl;
        // { +00AC }
        public Aow.Units.AoWLabel.TAOWLabel UTurns;
        // { +00B0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P1CB;
        // { +00B4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P2CB;
        // { +00B8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P3CB;
        // { +00BC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P4CB;
        // { +00C0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P5CB;
        // { +00C4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P6CB;
        // { +00C8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P7CB;
        // { +00CC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P8CB;
        // { +00D0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P9CB;
        // { +00D4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P10CB;
        // { +00D8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P11CB;
        // { +00DC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P12CB;
        // { +00E0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B4CB;
        // { +00E4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B5CB;
        // { +00E8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B6CB;
        // { +00EC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B1CB;
        // { +00F0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B2CB;
        // { +00F4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B3CB;
        // { +00F8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar T2SBar;
        // { +00FC }
        public Aow.Units.AoWPanel.TAOWPanel AbilPnl;
        // { +0100 }
        public Aow.Units.AoWPanel.TAOWPanel UpgradePnl;
        // { +0104 }
        public Aow.Units.AoWLabel.TAOWLabel UpgradeLbl;
        // { +0108 }
        public Aow.Units.AoWPanel.TAOWPanel UpgradeMemoFr;
        // { +010C }
        public Aow.Units.AoWMemo.TAOWMemo UpgradeMemo;
        // { +0110 }
        public Aow.Units.AoWPanel.TAOWPanel RightPnl;
        // { +0114 }
        public Aow.Units.AoWPanel.TAOWPanel QueuePnl;
        // { +0118 }
        public Aow.Units.AoWImage.TAOWImage Oval;
        // { +011C }
        public Aow.Units.AoWImage.TAOWImage ShadowL;
        // { +0120 }
        public Aow.Units.AoWImage.TAOWImage ShadowR;
        // { +0124 }
        public Aow.Units.AoWButton.TAOWButton CityBtn3;
        // { +0128 }
        public Aow.Units.AoWButton.TAOWButton CityBtn1;
        // { +012C }
        public Aow.Units.AoWListBox.TAOWListBox QueueLB;
        // { +0130 }
        public Aow.Units.AoWPanel.TAOWPanel ActivityPnl;
        // { +0134 }
        public Aow.Units.AoWPanel.TAOWPanel InfoPnl2;
        // { +0138 }
        public Aow.Units.AoWLabel.TAOWLabel ActiveAction;
        // { +013C }
        public Aow.Units.AoWLabel.TAOWLabel ActiveActionTurns;
        // { +0140 }
        public Aow.Units.AoWImage.TAOWImage SepBar;
        // { +0144 }
        public Aow.Units.AoWPanel.TAOWPanel ActivityPnl2;
        // { +0148 }
        public Aow.Units.AOWGauge.TAoWGauge ActiveActionTurnsBar;
        // { +014C }
        public Aow.Units.AoWButton.TAOWButton CityBtn2;
        // { +0150 }
        public Aow.Units.AoWPanel.TAOWPanel InfoPnl;
        // { +0154 }
        public Aow.Units.AoWPanel.TAOWPanel IPPnl;
        // { +0158 }
        public Aow.Units.AoWFrame.TAoWFrame IPPicFr;
        // { +015C }
        public Aow.Units.AoWMemo.TAOWMemo PopulationMemo;
        // { +0160 }
        public Aow.Units.AoWMemo.TAOWMemo PopulationHdr;
        // { +0164 }
        public Aow.Units.AoWPanel.TAOWPanel IIPnl;
        // { +0168 }
        public Aow.Units.AoWFrame.TAoWFrame IIPicFr;
        // { +016C }
        public Aow.Units.AoWMemo.TAOWMemo IncomeMemo;
        // { +0170 }
        public Aow.Units.AoWMemo.TAOWMemo IncomeHdr;
        // { +0174 }
        public Aow.Units.AoWPanel.TAOWPanel IEPnl;
        // { +0178 }
        public Aow.Units.AoWFrame.TAoWFrame IEPicFr;
        // { +017C }
        public Aow.Units.AoWMemo.TAOWMemo EnhancementsMemo;
        // { +0180 }
        public Aow.Units.AoWMemo.TAOWMemo EnhancementsHdr;
        // { +0184 }
        public Aow.Units.AoWPanel.TAOWPanel CurrentInfo;
        // { +0188 }
        public Aow.Units.AoWFrame.TAoWFrame RPillar;
        // { +018C }
        public Aow.Units.AoWFrame.TAoWFrame LPillar;
        // { +0190 }
        public Aow.Units.AoWPanel.TAOWPanel QueueInfo;
        // { +0194 }
        public Aow.Units.AoWLabel.TAOWLabel QueueLbl;
        // { +0198 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LoopQueueBox;
        // { +019C }
        public Aow.Units.AoWLabel.TAOWLabel RepeatLbl;
        // { +01A0 }
        public Aow.Units.AoWPanel.TAOWPanel ActiveActionPnl;
        // { +01A4 }
        public Aow.Units.AoWImage.TAOWImage CurrentActionImage;
        // { +01A8 }
        public Aow.Units.AoWImage.TAOWImage RacePortrait;
        // { +01AC }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +01B0 }
        public Aow.Units.AoWImage.TAOWImage BlackBg;
        // { +01B4 }
        public Aow.Units.AoWImage.TAOWImage BlackBg2;
        // { +01B8 }
        public Aow.Units.AoWPanel.TAOWPanel UPictureFramePnl;
        // { +01BC }
        public Aow.Units.AoWPanel.TAOWPanel UStatFr1;
        // { +01C0 }
        public Aow.Units.AoWPanel.TAOWPanel UStatFr2;
        // { +01C4 }
        public Aow.Units.AoWPanel.TAOWPanel UInfoFr;
        // { +01C8 }
        public Aow.Units.AoWPanel.TAOWPanel CostFr;
        // { +01CC }
        public Aow.Units.AoWPanel.TAOWPanel TurnsFr;
        // { +01D0 }
        public Aow.Units.AoWPanel.TAOWPanel UpgPicPnl;
        // { +01D4 }
        public Aow.Units.AoWImage.TAOWImage UpgradePic;
        // { +01D8 }
        public Aow.Units.AoWPanel.TAOWPanel UpgPicFr;
        // { +01DC }
        public Aow.Units.AoWPanel.TAOWPanel UpgCostPnl;
        // { +01E0 }
        public Aow.Units.AoWLabel.TAOWLabel UpgCostLbl;
        // { +01E4 }
        public Aow.Units.AoWLabel.TAOWLabel UpgradeCost;
        // { +01E8 }
        public Aow.Units.AoWLabel.TAOWLabel UpgTurnsLbl;
        // { +01EC }
        public Aow.Units.AoWLabel.TAOWLabel UpgradeTurns;
        // { +01F0 }
        public Aow.Units.AoWPanel.TAOWPanel UpgCostFr;
        // { +01F4 }
        public Aow.Units.AoWPanel.TAOWPanel UpgTurnsFr;
        // { +01F8 }
        public Aow.Units.AoWPanel.TAOWPanel MigratePnl;
        // { +01FC }
        public Aow.Units.AoWLabel.TAOWLabel NewRaceLbl;
        // { +0200 }
        public Aow.Units.AoWPanel.TAOWPanel MigrateMainFr;
        // { +0204 }
        public Aow.Units.AoWPanel.TAOWPanel MigPicPnl;
        // { +0208 }
        public Aow.Units.AoWImage.TAOWImage MigratePic;
        // { +020C }
        public Aow.Units.AoWPanel.TAOWPanel MigPicFr;
        // { +0210 }
        public Aow.Units.AoWPanel.TAOWPanel MigCostPnl;
        // { +0214 }
        public Aow.Units.AoWLabel.TAOWLabel MigCostLbl;
        // { +0218 }
        public Aow.Units.AoWLabel.TAOWLabel MigrateCost;
        // { +021C }
        public Aow.Units.AoWLabel.TAOWLabel MigTurnsLbl;
        // { +0220 }
        public Aow.Units.AoWLabel.TAOWLabel MigrateTurns;
        // { +0224 }
        public Aow.Units.AoWPanel.TAOWPanel MigCostFr;
        // { +0228 }
        public Aow.Units.AoWPanel.TAOWPanel MigTurnsFr;
        // { +022C }
        public Aow.Units.AoWListBox.TAOWListBox MigrationRaceLB;
        // { +0230 }
        public Aow.Units.AoWListBox.TAOWListBox MigrationRelationLB;
        // { +0234 }
        public Aow.Units.AoWListBox.TAOWListBox MigrationTurnsLB;
        // { +0238 }
        public Aow.Units.AoWButton.TAOWButton SortMigrationRaces;
        // { +023C }
        public Aow.Units.AoWButton.TAOWButton SortMigrationTurns;
        // { +0240 }
        public Aow.Units.AoWButton.TAOWButton SortMigrationRelation;
        // { +0244 }
        public Aow.Units.AoWPanel.TAOWPanel T3LBPnl;
        // { +0248 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar MigrationSB;
        // { +024C }
        public Aow.Units.AoWLabel.TAOWLabel T3RaceLbl;
        // { +0250 }
        public Aow.Units.AoWLabel.TAOWLabel CurrentRace;
        // { +0254 }
        public Aow.Units.AoWLabel.TAOWLabel T3RelLbl;
        // { +0258 }
        public Aow.Units.AoWLabel.TAOWLabel CurrentRelation;
        // { +025C }
        public Aow.Units.AoWPanel.TAOWPanel CurrentRacePnl;
        // { +0260 }
        public Aow.Units.AoWPanel.TAOWPanel CurrentRaceFr;
        // { +0264 }
        public Aow.Units.AoWLabel.TAOWLabel CurrentRaceLbl;
        // { +0268 }
        public Aow.Units.AoWPanel.TAOWPanel UpgBtm;
        // { +026C }
        public Aow.Units.AoWLabel.TAOWLabel UpgradeInfo;
        // { +0270 }
        public Aow.Units.AoWImage.TAOWImage RelIcn;
        // { +0274 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar QueueVSB;
        // { +0278 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar IPVSB;
        // { +027C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar IIVSB;
        // { +0280 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar IEVSB;
        // { +0284 }
        public Aow.Units.AoWMemo.TAOWMemo IncomeGoldMemo;
        // { +0288 }
        public Aow.Units.AoWLabel.TAOWLabel Migrate;
        // { +028C }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox UFace;
        // { +0290 }
        public Aow.Units.AoWPanel.TAOWPanel MigrateInfoPnl;
        // { +0294 }
        public Aow.Units.AoWLabel.TAOWLabel MigrateInfo;
        // { +0298 }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox MapView;
        // { +029C }
        public Aow.Units.AoWImage.TAOWImage InfoSep;
        // { +02A0 }
        public Aow.Units.AoWButton.TAOWButton PNext;
        // { +02A4 }
        public Aow.Units.AoWButton.TAOWButton PPrev;
        // { +02A8 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +02AC }
        public Aow.Units.City.TCity FCity;
        // { $2B0 }
        public Aow.Units.AoWTC.TAoWCombatMap FPreview;
        // { $2B4 }
        public int FViewX;
        // { $2B8 }
        public int FViewY;
        // { $2BC }
        public Aow.Units.City.TCityAction FAction;
        // { $2C0 }
        public Aow.Units.FixedArray_byte_3 F_CityScreen_2C1;
        public int FProductionIndex;
        // { $2C4 }
        public int FQueueIndex;
        // { $2C8 }
        public byte FMode;
        // { $2CC }
        public Aow.Units.FixedArray_byte_3 FReserved2CD;
        public Aow.Units.City.TMigrationList FMigrations;
        // { $2D0 }
        public int FMigrationSort;
        // { $2D4 }
        public Aow.Units.Engine.TIntegerList FImages;
        // AoWCityScreen.pas:748
        // { $2D8 }
        public void FreePreview()
        {
            if (FPreview != null)
            {
                FPreview.Free();
                FPreview = null;
            }
        }

        // AoWCityScreen.pas:757
        public void ReleaseImages()
        {
            int I = default;
            {
                I = FImages.Count - 1;
                byte end__1 = 0;
                for (; I >= end__1; I -= 1)
                {
                    var call__2 = FImages[I];
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__2);
                    call__3.Release();
                }
            }

            FImages.Clear();
        }

        // AoWCityScreen.pas:766
        public void LoadUnitImages(int Index)
        {
            int Gfx = default;
            var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(Index);
            Gfx = call__4.FUnitGFXResourceIndex;
            var call__5 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Gfx);
            call__5.AddRef();
            FImages.Add(Gfx);
        }

        // AoWCityScreen.pas:775
        public void ResetChecks()
        {
            P1CB.Checked = false;
            P2CB.Checked = false;
            P3CB.Checked = false;
            P4CB.Checked = false;
            P5CB.Checked = false;
            P6CB.Checked = false;
            P7CB.Checked = false;
            P8CB.Checked = false;
            P9CB.Checked = false;
            P10CB.Checked = false;
            P11CB.Checked = false;
            P12CB.Checked = false;
            B1CB.Checked = false;
            B2CB.Checked = false;
            B3CB.Checked = false;
            B4CB.Checked = false;
            B5CB.Checked = false;
            B6CB.Checked = false;
        }

        // AoWCityScreen.pas:797
        public void Reset()
        {
            ReleaseImages();
            FMode = 0;
            InfoPnl.Visible = true;
            InfoPnl2.Visible = true;
            ActivityPnl.Visible = false;
            ActivityPnl2.Visible = false;
            UnitPnl.Visible = false;
            UpgradePnl.Visible = false;
            MigratePnl.Visible = false;
            CityBtn1.Visible = true;
            var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.OptionsRStr);
            CityBtn1.Text = call__6;
            CityBtn2.Visible = false;
            PPrev.Visible = !CityBtn2.Visible;
            PNext.Visible = !CityBtn2.Visible;
            ShadowL.Visible = Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0;
            ShadowR.Visible = Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0;
            Oval.Visible = Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0;
            FMigrationSort = 0;
            FProductionIndex = 0;
            QueueLB.Selected = -1;
            FreePreview();
            var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
            QueueLbl.Text = call__7;
            var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RepeatRStr);
            RepeatLbl.Text = call__8;
            if (FMigrations != null)
            {
                FMigrations.Free();
                FMigrations = null;
            }
        }

        // AoWCityScreen.pas:829
        public void CloseCity()
        {
            Reset();
            FCity = null;
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
        }

        // AoWCityScreen.pas:836
        public void SetMode(byte Mode)
        {
            FMode = unchecked((byte)Mode);
            switch (FMode)
            {
                case 0:
                    InfoPnl.Visible = true;
                    InfoPnl2.Visible = true;
                    ActivityPnl.Visible = false;
                    ActivityPnl2.Visible = false;
                    var call__9 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ChangeRStr);
                    CityBtn1.Text = call__9;
                    CityBtn1.Visible = true;
                    CityBtn2.Visible = false;
                    PPrev.Visible = !CityBtn2.Visible;
                    PNext.Visible = !CityBtn2.Visible;
                    var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ExitRStr);
                    CityBtn3.Text = call__10;
                    QueueLB.Selected = -1;
                    LPillar.Visible = true;
                    RPillar.Visible = true;
                    MapView.WinHeight = InfoPnl.WinTop - MapView.WinTop;
                    break;
                case 1:
                    QueueLB.Selected = -1;
                    InfoPnl.Visible = false;
                    InfoPnl2.Visible = false;
                    ActivityPnl.Visible = true;
                    ActivityPnl2.Visible = true;
                    var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InfoRStr);
                    CityBtn1.Text = call__11;
                    var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DoneRStr);
                    CityBtn3.Text = call__12;
                    LPillar.Visible = false;
                    RPillar.Visible = false;
                    MapView.WinHeight = ActivityPnl.WinTop - MapView.WinTop;
                    MigrationRaceLB.CaptureInput = true;
                    MigrationTurnsLB.CaptureInput = true;
                    MigrationRelationLB.CaptureInput = true;
                    break;
                case 2:
                    ResetChecks();
                    QueueLB.Selected = -1;
                    InfoPnl.Visible = false;
                    InfoPnl2.Visible = false;
                    ActivityPnl.Visible = true;
                    ActivityPnl2.Visible = true;
                    LPillar.Visible = false;
                    RPillar.Visible = false;
                    var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InfoRStr);
                    CityBtn1.Text = call__13;
                    var call__14 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DoneRStr);
                    CityBtn3.Text = call__14;
                    MigrationRaceLB.CaptureInput = false;
                    MigrationTurnsLB.CaptureInput = false;
                    MigrationRelationLB.CaptureInput = false;
                    MigrationRaceLB.Selected = -1;
                    MigrationTurnsLB.Selected = -1;
                    MigrationRelationLB.Selected = -1;
                    break;
            }
        }

        // AoWCityScreen.pas:892
        public void SortMigration()
        {
            switch (FMigrationSort)
            {
                case 0:
                    Aow.Units.Engine.Engine.SortStringsNumericalAscending(MigrationTurnsLB.Items, [MigrationRaceLB.Items, MigrationRelationLB.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsNumericalDescending(MigrationTurnsLB.Items, [MigrationRaceLB.Items, MigrationRelationLB.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsAscending(MigrationRaceLB.Items, [MigrationTurnsLB.Items, MigrationRelationLB.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsDescending(MigrationRaceLB.Items, [MigrationTurnsLB.Items, MigrationRelationLB.Items]);
                    break;
                case 4:
                    Aow.Units.Engine.Engine.SortStringsAscending(MigrationRelationLB.Items, [MigrationTurnsLB.Items, MigrationRaceLB.Items]);
                    break;
                case 5:
                    Aow.Units.Engine.Engine.SortStringsDescending(MigrationRelationLB.Items, [MigrationTurnsLB.Items, MigrationRaceLB.Items]);
                    break;
            }
        }

        // AoWCityScreen.pas:904
        public int ActivityImage(Aow.Units.City.TCityAction Action)
        {
            int Result = default;
            switch (Action)
            {
                case Aow.Units.City.TCityAction.caTrade:
                    Result = 40;
                    break;
                case Aow.Units.City.TCityAction.caUpgrade:
                    Result = 43;
                    break;
                case Aow.Units.City.TCityAction.caFortify:
                    if (FCity.FWallType == 0)
                    {
                        Result = 41;
                    }
                    else
                    {
                        Result = 42;
                    }

                    break;
                case Aow.Units.City.TCityAction.caMigrate:
                    Result = 44;
                    break;
                case Aow.Units.City.TCityAction.caLoot:
                    Result = 45;
                    break;
                case Aow.Units.City.TCityAction.caRaze:
                    Result = 46;
                    break;
                case Aow.Units.City.TCityAction.caProduce:
                    Result = 4;
                    break;
                case Aow.Units.City.TCityAction.caResearch:
                    Result = 5;
                    break;
                default:
                    Result = 40;
                    break;
            }

            return Result;
        }

        // AoWCityScreen.pas:919
        public void SelectDefaultAction()
        {
            ResetChecks();
            B1CB.Checked = true;
            SelectAction(Aow.Units.City.TCityAction.caTrade);
        }

        // AoWCityScreen.pas:926
        public void SelectProduction(int Index)
        {
            Aow.Units.AoWE.TUnitProduction P = default;
            var call__15 = FCity.FProduction.FUnitProductions.GetItem(Index);
            P = call__15;
            if (P != null)
            {
                FProductionIndex = Index;
                if (P.FResearched)
                {
                    SelectAction(Aow.Units.City.TCityAction.caProduce);
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(58, true, true);
                }
                else
                {
                    SelectAction(Aow.Units.City.TCityAction.caResearch);
                }
            }
        }

        // AoWCityScreen.pas:943
        public void SelectMigration(int Index)
        {
            Aow.Units.City.TMigrationItem Item = default;
            bool bool__16 = (Index >= 0);
            if (bool__16)
            {
                var call__17 = MigrationRaceLB.Items.Count;
                bool__16 = (Index < call__17);
            }

            if (bool__16)
            {
                var call__18 = MigrationRaceLB.Items.Objects[Index];
                Item = (Aow.Units.City.TMigrationItem)call__18;
                MigrationRaceLB.Selected = Index;
                MigrationTurnsLB.Selected = Index;
                MigrationRelationLB.Selected = Index;
                MigrateTurns.Text = Aow.Rtl.Rtl.IntToStr(Item.FTime);
                var call__19 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[FCity.FRace]);
                var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XRelationMinusYRStr);
                var call__21 = Aow.Units.AoWE.AoWE.Format(call__20, [call__19, (int)-15]);
                MigrateInfo.Text = call__21;
                MigrateInfo.Text = MigrateInfo.Text + ((AnsiString)"   ");
                var call__22 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[Item.FRace]);
                var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XRelationPlusYRStr);
                var call__24 = Aow.Units.AoWE.AoWE.Format(call__23, [call__22, (int)10]);
                MigrateInfo.Text = MigrateInfo.Text + call__24;
                CityBtn1.Visible = true;
                var call__25 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MigrateRStr);
                CityBtn1.Text = call__25;
            }
            else
            {
                MigrationRaceLB.Selected = -1;
                MigrationTurnsLB.Selected = -1;
                MigrationRelationLB.Selected = -1;
                MigrateInfo.Text = (AnsiString)"";
                MigrateTurns.Text = AnsiString.FromBytes(45);
                UpgradeInfo.Text = (AnsiString)"";
                CityBtn1.Visible = false;
            }
        }

        // AoWCityScreen.pas:972
        public void SelectAction(Aow.Units.City.TCityAction Action)
        {
            Aow.Units.AoWE.TUnit U = default;
            if (FMode != 1)
            {
                SetMode(1);
            }

            FAction = Action;
            UpgradePnl.Visible = false;
            UnitPnl.Visible = false;
            MigratePnl.Visible = false;
            UpgradePic.CurrentImage = 21;
            UpgradeCost.Text = AnsiString.FromBytes(45);
            UpgradeTurns.Text = AnsiString.FromBytes(45);
            UpgradeInfo.Text = (AnsiString)"";
            CityBtn1.Visible = false;
            CityBtn2.Visible = false;
            switch (FAction)
            {
                case Aow.Units.City.TCityAction.caTrade:
                    CityBtn1.Visible = true;
                    var call__26 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceMerchandiseRStr);
                    CityBtn1.Text = call__26;
                    var call__27 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !call__27;
                    var call__28 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
                    CityBtn2.Text = call__28;
                    var call__29 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityTradeRStr);
                    UpgradeMemo.Items.Text = call__29;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 20;
                    var call__30 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceMerchandiseRStr);
                    UpgradeLbl.Text = call__30;
                    break;
                case Aow.Units.City.TCityAction.caUpgrade:
                    CityBtn1.Visible = true;
                    var call__31 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpgradeRStr);
                    CityBtn1.Text = call__31;
                    var call__32 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !call__32;
                    var call__33 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
                    CityBtn2.Text = call__33;
                    var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityUpgradeRStr);
                    UpgradeMemo.Items.Text = call__34;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 23;
                    var call__35 = FCity.UpgradeCost();
                    UpgradeCost.Text = Aow.Rtl.Rtl.IntToStr(call__35);
                    var call__36 = FCity.UpgradeTime();
                    UpgradeTurns.Text = Aow.Rtl.Rtl.IntToStr(call__36);
                    var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpgradeRStr);
                    UpgradeLbl.Text = call__37;
                    var call__38 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[FCity.FRace]);
                    var call__39 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XRelationPlusYRStr);
                    var call__40 = Aow.Units.AoWE.AoWE.Format(call__39, [call__38, (int)5]);
                    UpgradeInfo.Text = call__40;
                    break;
                case Aow.Units.City.TCityAction.caFortify:
                    CityBtn1.Visible = true;
                    var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FortifyRStr);
                    CityBtn1.Text = call__41;
                    var call__42 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !call__42;
                    var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
                    CityBtn2.Text = call__43;
                    var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityFortifyRStr);
                    UpgradeMemo.Items.Text = call__44;
                    UpgradePnl.Visible = true;
                    if (FCity.FWallType == 0)
                    {
                        UpgradePic.CurrentImage = 21;
                    }
                    else
                    {
                        UpgradePic.CurrentImage = 22;
                    }

                    var call__45 = FCity.FortifyCost();
                    if (call__45 > 0)
                    {
                        var call__46 = FCity.FortifyCost();
                        UpgradeCost.Text = Aow.Rtl.Rtl.IntToStr(call__46);
                        var call__47 = FCity.FortifyTime();
                        UpgradeTurns.Text = Aow.Rtl.Rtl.IntToStr(call__47);
                    }
                    else
                    {
                        UpgradeCost.Text = AnsiString.FromBytes(45);
                        UpgradeTurns.Text = AnsiString.FromBytes(45);
                    }

                    var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FortifyRStr);
                    UpgradeLbl.Text = call__48;
                    var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[FCity.FRace]);
                    var call__50 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XRelationPlusYRStr);
                    var call__51 = Aow.Units.AoWE.AoWE.Format(call__50, [call__49, (int)5]);
                    UpgradeInfo.Text = call__51;
                    break;
                case Aow.Units.City.TCityAction.caMigrate:
                    MigratePnl.Visible = true;
                    SelectMigration(0);
                    break;
                case Aow.Units.City.TCityAction.caLoot:
                    CityBtn1.Visible = true;
                    var call__52 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LootRStr);
                    CityBtn1.Text = call__52;
                    var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityLootRStr);
                    UpgradeMemo.Items.Text = call__53;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 25;
                    var call__54 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LootRStr);
                    UpgradeLbl.Text = call__54;
                    var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[FCity.FRace]);
                    var call__56 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XRelationMinusYRStr);
                    var call__57 = Aow.Units.AoWE.AoWE.Format(call__56, [call__55, (int)-30]);
                    UpgradeInfo.Text = call__57;
                    break;
                case Aow.Units.City.TCityAction.caRaze:
                    CityBtn1.Visible = true;
                    var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RazeRStr);
                    CityBtn1.Text = call__58 + ((AnsiString)"...");
                    var call__59 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityRazeRStr);
                    UpgradeMemo.Items.Text = call__59;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 26;
                    var call__60 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RazeRStr);
                    UpgradeLbl.Text = call__60;
                    var call__61 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[FCity.FRace]);
                    var call__62 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XRelationMinusYRStr);
                    var call__63 = Aow.Units.AoWE.AoWE.Format(call__62, [call__61, (int)-30]);
                    UpgradeInfo.Text = call__63;
                    break;
                case Aow.Units.City.TCityAction.caResearch:
                case Aow.Units.City.TCityAction.caProduce:
                    var call__64 = FCity.FProduction.CreateUnitProduction(FProductionIndex);
                    U = call__64;
                    UnitPnl.Visible = true;
                    var call__65 = U.GetAttack();
                    UAttack.Text = Aow.Rtl.Rtl.IntToStr(call__65);
                    var call__66 = U.GetDefense();
                    UDefense.Text = Aow.Rtl.Rtl.IntToStr(call__66);
                    var call__67 = U.GetResistance();
                    UResistance.Text = Aow.Rtl.Rtl.IntToStr(call__67);
                    var call__68 = U.GetDamage();
                    UDamage.Text = Aow.Rtl.Rtl.IntToStr(call__68);
                    var call__69 = U.GetHits();
                    UHits.Text = Aow.Rtl.Rtl.IntToStr(call__69);
                    var call__70 = U.GetMoves();
                    UMoves.Text = Aow.Rtl.Rtl.IntToStr(call__70);
                    var call__71 = U.GetMoveTypes();
                    var call__72 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__71);
                    UMovesIcon.CurrentImage = call__72;
                    var call__73 = U.GetAlignment();
                    var call__74 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TAlignmentRStr[(int)call__73]);
                    var call__75 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlignmentXRStr);
                    var call__76 = Aow.Units.AoWE.AoWE.Format(call__75, [call__74]);
                    UAlignment.Text = call__76;
                    var call__77 = U.GetUnitLevel();
                    var call__78 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelXRStr);
                    var call__79 = Aow.Units.AoWE.AoWE.Format(call__78, [Aow.Rtl.Rtl.IntToStr(call__77)]);
                    var call__80 = U.GetUpkeep();
                    var call__81 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpkeepXRStr);
                    var call__82 = Aow.Units.AoWE.AoWE.Format(call__81, [(int)call__80]);
                    ULevel.Text = (call__79 + ((AnsiString)" / ")) + call__82;
                    if (Aow.Units.AoWE.AoWE.AoWEngine.FDebug)
                    {
                        var call__83 = U.FUnitResource.UnitResourceIndex();
                        var call__84 = Aow.Units.AoWE.AoWE.GetUnitIndexAIPriority(call__83, unchecked((sbyte)FCity.FPlayer), 0);
                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(((AnsiString)"AI Prio:") + Aow.Rtl.Rtl.IntToStr(call__84));
                    }

                    UAbilitiesLB.Items.Clear();
                    U.ListAbilities(UAbilitiesLB.Items);
                    if (FAction == Aow.Units.City.TCityAction.caProduce)
                    {
                        CityBtn1.Visible = true;
                        var call__85 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceRStr);
                        CityBtn1.Text = call__85;
                        var call__86 = FCity.FProduction.GetQueueLocked();
                        CityBtn2.Visible = !call__86;
                        var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
                        CityBtn2.Text = call__87;
                        var call__88 = U.GetName();
                        var call__89 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceXRStr);
                        var call__90 = Aow.Units.AoWE.AoWE.Format(call__89, [call__88]);
                        UnitLbl.Text = call__90;
                        var call__91 = FCity.FProduction.UnitProductionGold(FProductionIndex);
                        UCost.Text = Aow.Rtl.Rtl.IntToStr(call__91);
                        var call__92 = FCity.FProduction.UnitProductionTime(FProductionIndex);
                        UTurns.Text = Aow.Rtl.Rtl.IntToStr(call__92);
                    }
                    else
                    {
                        CityBtn1.Visible = true;
                        var call__93 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InstallRStr);
                        CityBtn1.Text = call__93;
                        var call__94 = FCity.FProduction.GetQueueLocked();
                        CityBtn2.Visible = !call__94;
                        var call__95 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
                        CityBtn2.Text = call__95;
                        var call__96 = U.GetName();
                        var call__97 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InstallXRStr);
                        var call__98 = Aow.Units.AoWE.AoWE.Format(call__97, [call__96]);
                        UnitLbl.Text = call__98;
                        var call__99 = FCity.FProduction.UnitResearchGold(FProductionIndex);
                        UCost.Text = Aow.Rtl.Rtl.IntToStr(call__99);
                        var call__100 = FCity.FProduction.UnitResearchTime(FProductionIndex);
                        UTurns.Text = Aow.Rtl.Rtl.IntToStr(call__100);
                    }

                    U.Release();
                    break;
            }

            PPrev.Visible = false;
            PNext.Visible = false;
        }

        // AoWCityScreen.pas:1099
        public void UpdateInfo()
        {
            Aow.Rtl.AnsiString Reason = default;
            int I = default;
            int J = default;
            var call__101 = FCity.GetName();
            StructName.Text = call__101;
            PopulationHdr.Items.Clear();
            var call__102 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PopulationRStr);
            PopulationHdr.Items.Add(call__102 + AnsiString.FromBytes(58));
            var call__103 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[FCity.FRace]);
            PopulationHdr.Items.Add(AnsiString.FromBytes(32) + call__103);
            var call__104 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StatusRStr);
            PopulationHdr.Items.Add(call__104 + AnsiString.FromBytes(58));
            var call__105 = FCity.GetCivilStatus();
            var call__106 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.City.City.TCivilStatusRStr[(int)call__105]);
            PopulationHdr.Items.Add(AnsiString.FromBytes(32) + call__106);
            PopulationMemo.Items.Clear();
            FCity.ListCivilStatusInfo(PopulationMemo.Items);
            IncomeHdr.Items.Clear();
            var call__107 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IncomeRStr);
            IncomeHdr.Items.Add(call__107);
            var call__108 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XGoldRStr);
            var call__109 = Aow.Units.AoWE.AoWE.Format(AnsiString.FromBytes(32) + call__108, [Aow.Rtl.Rtl.IntToStr(FCity.FIncome)]);
            IncomeHdr.Items.Add(call__109);
            IncomeMemo.Items.Clear();
            IncomeGoldMemo.Items.Clear();
            FCity.ListIncomeInfo(IncomeMemo.Items, IncomeGoldMemo.Items);
            EnhancementsHdr.Items.Clear();
            var call__110 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnhancementsRStr);
            EnhancementsHdr.Items.Add(call__110);
            EnhancementsMemo.Items.Clear();
            FCity.ListEnhancementsInfo(EnhancementsMemo.Items);
            if (FMigrations != null)
            {
                FMigrations.Free();
                FMigrations = null;
            }

            var call__111 = FCity.CreateMigrationSelection();
            FMigrations = call__111;
            var call__112 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[FCity.FRace]);
            CurrentRace.Text = call__112;
            var call__113 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
            var call__114 = call__113.GetPlayerRelation(unchecked((sbyte)FCity.FPlayer));
            var call__115 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__114]);
            CurrentRelation.Text = call__115;
            MigrationRaceLB.Items.Clear();
            MigrationTurnsLB.Items.Clear();
            MigrationRelationLB.Items.Clear();
            if (FMigrations != null)
            {
                {
                    I = 0;
                    int end__116 = FMigrations.GetCount() - 1;
                    for (; I <= end__116; I += 1)
                    {
                        var call__117 = FMigrations.GetItems(I);
                        var call__118 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__117.FRace]);
                        var call__119 = FMigrations.GetItems(I);
                        MigrationRaceLB.Items.AddObject(call__118, call__119);
                        var call__120 = FMigrations.GetItems(I);
                        MigrationTurnsLB.Items.Add(Aow.Rtl.Rtl.IntToStr(call__120.FTime));
                        var call__121 = FMigrations.GetItems(I);
                        var call__122 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__121.FRelation]);
                        MigrationRelationLB.Items.Add(call__122);
                    }
                }
            }

            SortMigration();
            QueueLB.Items.Clear();
            {
                I = 0;
                int end__123 = FCity.FProduction.FQueue.GetCount() - 1;
                for (; I <= end__123; I += 1)
                {
                    var call__124 = FCity.FProduction.FQueue.GetItem(I);
                    var call__125 = call__124.GetName();
                    QueueLB.Items.Add((Aow.Rtl.Rtl.IntToStr(I + 1) + ((AnsiString)". ")) + call__125);
                }
            }

            {
                I = FCity.FProduction.FQueue.GetCount();
                byte end__126 = 4;
                for (; I <= end__126; I += 1)
                {
                    var call__127 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EmptyQueueRStr);
                    QueueLB.Items.Add((Aow.Rtl.Rtl.IntToStr(I + 1) + ((AnsiString)". ")) + call__127);
                }
            }

            var call__128 = FCity.FProduction.FQueue.GetCount();
            if (QueueLB.Selected >= call__128)
            {
                var call__129 = FCity.FProduction.FQueue.GetCount();
                QueueLB.Selected = call__129 - 1;
                FQueueIndex = QueueLB.Selected;
            }

            LoopQueueBox.Checked = FCity.FProduction.FLoopQueue;
            var call__130 = FCity.GetActiveAction();
            if (call__130 != Aow.Units.City.TCityAction.caNone)
            {
                var call__131 = FCity.FProduction.FQueue.GetItem(0);
                var call__132 = call__131.GetNameProgressive();
                ActiveAction.Text = call__132;
                if (FCity.FProduction.FSettings.FTurnsLeft != -1)
                {
                    var call__133 = Aow.Units.AoWE.AoWE.FmtTurnsLeftStr(FCity.FProduction.FSettings.FTurnsLeft);
                    ActiveActionTurns.Text = call__133;
                    ActiveActionTurns.Visible = true;
                    ActiveActionTurns.TextColor = 6783862;
                    this.ActiveActionTurnsBar.Min = 0;
                    this.ActiveActionTurnsBar.Max = FCity.FProduction.FSettings.FTurnsTotal;
                    this.ActiveActionTurnsBar.Position = FCity.FProduction.FSettings.FTurnsTotal - FCity.FProduction.FSettings.FTurnsLeft;

                    ActiveActionTurnsBar.Visible = true;
                }
                else
                {
                    ActiveActionTurns.Visible = false;
                    ActiveActionTurnsBar.Visible = false;
                }

                var call__135 = FCity.GetActiveAction();
                var call__136 = ActivityImage(call__135);
                CurrentActionImage.CurrentImage = call__136;
            }
            else
            {
                var call__137 = FCity.FProduction.FQueue.GetCount();
                if (call__137 == 0)
                {
                    var call__138 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr);
                    ActiveAction.Text = call__138;
                    var call__139 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DefaultRStr);
                    ActiveActionTurns.Text = (AnsiString.FromBytes(40) + Aow.Rtl.Rtl.LowerCase(call__139)) + AnsiString.FromBytes(41);
                    ActiveActionTurns.TextColor = 6783862;
                    ActiveActionTurnsBar.Visible = false;
                    var call__140 = FCity.GetActiveAction();
                    var call__141 = ActivityImage(call__140);
                    CurrentActionImage.CurrentImage = call__141;
                }
                else
                {
                    Reason = (AnsiString)"";
                    var call__142 = FCity.FProduction.FQueue.GetItem(0);
                    var call__143 = call__142.GetNameProgressive();
                    ActiveAction.Text = call__143;
                    var call__144 = FCity.FProduction.FQueue.GetItem(0);
                    FCity.FProduction.CanExecuteProduction(call__144, ref Reason);
                    ActiveActionTurns.Text = Reason;
                    ActiveActionTurns.TextColor = 255;
                    ActiveActionTurnsBar.Visible = false;
                    var call__145 = FCity.FProduction.FQueue.GetItem(0);
                    var call__146 = Aow.Units.City.City.ProductionIDToCityAction(call__145.FProductionID);
                    var call__147 = ActivityImage(call__146);
                    CurrentActionImage.CurrentImage = call__147;
                }
            }

            if ((FMode == 1) && (FAction != Aow.Units.City.TCityAction.caNone))
            {
                SelectAction(FAction);
            }

            Dlg.FDrawn = false;
        }

        // AoWCityScreen.pas:1200
        public void ProductionEvent(Aow.Units.City.TCityProductionEventLog Event)
        {
            var call__148 = Event.GetCity();
            ShowCity(call__148);
            SetMode(1);
            SelectDefaultAction();
        }

        // AoWCityScreen.pas:1207
        public void CityChanged(Aow.Rtl.TObject Sender)
        {
            // { Tried with dialog and original hide-first branch; named dialog and hide-first branch; hide-first branch and explicit window view; no exact match. }
            if (Dlg.Visible && (Sender == FCity))
            {
                if (FCity.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                {
                    UpdateInfo();
                }
                else
                {
                    Dlg.Visible = false;
                }
            }
        }

        // AoWCityScreen.pas:245
        public void ShowCity(Aow.Units.City.TCity City)
        {
            int I = default;
            int J = default;
            int Direction = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.AoWE.TArmyHSList Armies = default;
            Aow.Units.AoWTC.TRndTacHsm Generator = default;
            Reset();
            FCity = City;
            UpdateInfo();
            SetMode(0);
            FreePreview();
            {
                I = 0;
                int end__149 = FCity.FProduction.FUnitProductions.GetCount() - 1;
                for (; I <= end__149; I += 1)
                {
                    var call__150 = FCity.FProduction.FUnitProductions.GetItem(I);
                    LoadUnitImages(call__150.FUnitIndex);
                }
            }

            Generator = Aow.Units.AoWTC.TRndTacHsm.Create();
            var call__151 = FCity.GetLhx();
            var call__152 = FCity.GetYhx();
            var call__153 = FCity.GetXhx();
            var call__154 = Generator.RandomMap(null, call__153, call__152, call__151);
            FPreview = call__154;
            Generator.Free();
            Armies = new Aow.Units.AoWE.TArmyHSList(null);
            FCity.ListArmies(Armies);
            {
                I = 0;
                int end__155 = Armies.GetCount() - 1;
                for (; I <= end__155; I += 1)
                {
                    Army = Armies.GetArmyHS(I);
                    var call__156 = Army.GetYhx();
                    var call__157 = Army.GetXhx();
                    var call__158 = FCity.GetYhx();
                    var call__159 = FCity.GetXhx();
                    var call__160 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__159, call__158, call__157, call__156);
                    Direction = call__160;
                    {
                        J = 0;
                        int end__161 = Army.FArmy.GetCount() - 1;
                        for (; J <= end__161; J += 1)
                        {
                            var call__162 = Army.FArmy.GetUnit(J);
                            Aow.Units.OverviewUnit.OverviewUnit.PlaceOverviewUnit(FPreview, call__162, Direction, J);
                        }
                    }
                }
            }

            Armies.Free();
            var mapObject = FPreview.FCenterViewLocation;
            if (FPreview.FCenterViewLocation != null)
            {
                var call__164 = mapObject.GetYhx();
                var call__165 = mapObject.GetXhx();
                Aow.Units.HSEngine.HSEngine.HXtoHP(call__165, call__164, ref FViewX, ref FViewY);
                FViewX = (FViewX + 16) - (MapView.WinWidth / 2);
                FViewY = (FViewY + 16) - (MapView.WinHeight / 2);
            }
            else
            {
                FViewX = 800;
                FViewY = 800;
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(56, true, true);
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
            Dlg.Visible = true;
        }

        // AoWCityScreen.pas:292
        public void CityScreenCreate()
        {
            var call__166 = new Aow.Units.Engine.TIntegerList();
            FImages = call__166;
        }

        // AoWCityScreen.pas:297
        public void CityScreenDestroy()
        {
            FImages.Free();
        }

        // AoWCityScreen.pas:302
        public void ActivityChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                ResetChecks();
                SelectAction((Aow.Units.City.TCityAction)(unchecked((byte)((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag) + 1));
                ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = true;
            }
        }

        // AoWCityScreen.pas:313
        public void ActivityDoubleClick(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.City.TCityAction)(unchecked((byte)((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag) + 1)) != Aow.Units.City.TCityAction.caMigrate)
            {
                ResetChecks();
                SelectAction((Aow.Units.City.TCityAction)(unchecked((byte)((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag) + 1));
                ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = true;
                CityBtn1Click(null);
            }
        }

        // AoWCityScreen.pas:324
        public void ProductionChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                ResetChecks();
                SelectProduction(((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag);
                ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = true;
            }
        }

        // AoWCityScreen.pas:335
        public void ProductionDoubleClick(Aow.Rtl.TObject Sender)
        {
            ResetChecks();
            SelectProduction(((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag);
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = true;
            CityBtn1Click(null);
        }

        // AoWCityScreen.pas:343
        public void ProductionsUpdate(Aow.Rtl.TObject Sender)
        {
            var call__167 = FCity.FProduction.FUnitProductions.GetItem(((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag);
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).CaptureInput = call__167 != null;
        }

        // AoWCityScreen.pas:348
        public void ActivityPreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Image = default;
            var call__168 = ActivityImage((Aow.Units.City.TCityAction)(unchecked((byte)Sender.Tag) + 1));
            Image = call__168;
            var call__169 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[Image];
            call__169.Show((ActivityPnl2.WinLeft + Sender.WinLeft) + 2, (ActivityPnl2.WinTop + Sender.WinTop) + 2, SD);
        }

        // AoWCityScreen.pas:354
        public void CityBtn1Click(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Reason = default;
            sbyte Race = default;
            switch (FMode)
            {
                case 0:
                    SetMode(1);
                    SelectDefaultAction();
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(57, true, true);
                    break;
                case 1:
                    switch (FAction)
                    {
                        case Aow.Units.City.TCityAction.caTrade:
                            var call__170 = FCity.CanTrade(false, ref Reason);
                            if (call__170)
                            {
                                FCity.Trade(false);
                                SetMode(0);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caLoot:
                            var call__171 = FCity.CanLoot(ref Reason);
                            if (call__171)
                            {
                                FCity.Loot();
                                SetMode(0);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caRaze:
                            var call__172 = FCity.CanRaze(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), ref Reason);
                            if (call__172)
                            {
                                FCity.Raze(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                Dlg.Visible = false;
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caUpgrade:
                            var call__173 = FCity.CanUpgrade(false, ref Reason);
                            if (call__173)
                            {
                                FCity.Upgrade(false);
                                SetMode(0);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caFortify:
                            var call__174 = FCity.CanFortify(false, ref Reason);
                            if (call__174)
                            {
                                FCity.Fortify(false);
                                SetMode(0);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caProduce:
                            var call__175 = FCity.CanProduce(FProductionIndex, false, ref Reason);
                            if (call__175)
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(59, true, true);
                                FCity.Produce(FProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caResearch:
                            var call__176 = FCity.CanResearch(FProductionIndex, false, ref Reason);
                            if (call__176)
                            {
                                FCity.Research(FProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caMigrate:
                            var call__177 = MigrationRaceLB.Items.Objects[MigrationRaceLB.Selected];
                            Race = unchecked((sbyte)((Aow.Units.City.TMigrationItem)call__177).FRace);
                            var call__178 = FCity.CanMigrate(unchecked((sbyte)Race), ref Reason);
                            if (call__178)
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(66, true, true);
                                FCity.Migrate(unchecked((sbyte)Race));
                                SetMode(0);
                            }

                            break;
                    }

                    break;
                case 2:
                    if (FQueueIndex != -1)
                    {
                        FCity.FProduction.DeleteProduction(FQueueIndex);
                    }

                    break;
            }
        }

        // AoWCityScreen.pas:437
        public void CityBtn2Click()
        {
            Aow.Rtl.AnsiString Reason = default;
            switch (FMode)
            {
                case 1:
                    switch (FAction)
                    {
                        case Aow.Units.City.TCityAction.caTrade:
                            var call__179 = FCity.CanTrade(true, ref Reason);
                            if (call__179)
                            {
                                FCity.Trade(true);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caUpgrade:
                            var call__180 = FCity.CanUpgrade(true, ref Reason);
                            if (call__180)
                            {
                                FCity.Upgrade(true);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caFortify:
                            var call__181 = FCity.CanFortify(true, ref Reason);
                            if (call__181)
                            {
                                FCity.Fortify(true);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caProduce:
                            var call__182 = FCity.CanProduce(FProductionIndex, true, ref Reason);
                            if (call__182)
                            {
                                FCity.Produce(FProductionIndex, true);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case Aow.Units.City.TCityAction.caResearch:
                            var call__183 = FCity.CanResearch(FProductionIndex, true, ref Reason);
                            if (call__183)
                            {
                                FCity.Research(FProductionIndex, true);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                    }

                    break;
                case 2:
                    if (FQueueIndex == 0)
                    {
                        var call__184 = FCity.FProduction.RetryProductionExecution(ref Reason);
                        if (call__184)
                        {
                            SetMode(0);
                        }
                        else
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                        }
                    }

                    break;
            }
        }

        // AoWCityScreen.pas:465
        public void CityBtn3Click()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FMode == 0)
            {
                Dlg.Visible = false;
            }
            else
            {
                SetMode(0);
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(60, true, true);
        }

        // AoWCityScreen.pas:472
        public void PreDrawProductions(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Units.AoWE.TUnit U = default;
            Aow.Units.AoWE.TUnitProduction P = default;
            var call__185 = Aow.Rtl.Rtl.Rect(ActivityPnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft, ActivityPnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, (ActivityPnl2.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth, (ActivityPnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight);
            R = call__185;
            // { Original uses two different panels. }
            var call__186 = FCity.FProduction.FUnitProductions.GetItem(((Aow.Units.AoWComp.TAoWComponent)Sender).Tag);
            P = call__186;
            if (P != null)
            {
                var call__187 = FCity.FProduction.CreateUnitProduction(((Aow.Units.AoWComp.TAoWComponent)Sender).Tag);
                U = call__187;
                Info.Direction = 3;
                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of();
                Info.Animation = (Aow.Units.AoWE.TUnitAnimation)0;
                Info.Frame = 0;
                Info.Looped = false;
                if (!P.FResearched)
                {
                    var call__188 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__188.Show(R.Left + 2, R.Top + 2, SD);
                    Info.Direction = 5;
                    if (U != null)
                    {
                        U.ShowEx(R.Left - 4, R.Top, SD, Info);
                    }

                    var call__189 = Aow.Units.GFXE.GFXE.AlphaBlendTable.GetLookupTables(45);
                    var call__190 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__190.ShowBlended(R.Left + 2, R.Top + 2, SD, call__189);
                }
                else
                {
                    var call__191 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__191.Show(R.Left + 2, R.Top + 2, SD);
                    if (U != null)
                    {
                        U.ShowEx(R.Left - 4, R.Top, SD, Info);
                    }
                }

                if (U != null)
                {
                    U.Release();
                }
            }
            else
            {
                var call__192 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[62];
                call__192.Show(R.Left + 2, R.Top + 2, SD);
            }
        }

        // AoWCityScreen.pas:508
        public void SortMigrationRacesClick()
        {
            if (FMigrationSort == 2)
            {
                FMigrationSort = 3;
            }
            else
            {
                FMigrationSort = 2;
            }

            SortMigration();
        }

        // AoWCityScreen.pas:515
        public void SortMigrationTurnsClick()
        {
            if (FMigrationSort == 0)
            {
                FMigrationSort = 1;
            }
            else
            {
                FMigrationSort = 0;
            }

            SortMigration();
        }

        // AoWCityScreen.pas:522
        public void SortMigrationRelationClick()
        {
            if (FMigrationSort == 4)
            {
                FMigrationSort = 5;
            }
            else
            {
                FMigrationSort = 4;
            }

            SortMigration();
        }

        // AoWCityScreen.pas:529
        public void MigrationChange(Aow.Units.AoWListBox.TAOWListBox Sender)
        {
            if (FMode != 2)
            {
                SelectMigration(Sender.Selected);
            }
        }

        // AoWCityScreen.pas:534
        public void MigrationSBChange(Aow.Rtl.TObject Sender)
        {
            MigrationRaceLB.ListOffset = ((Aow.Units.AoWVScroll.TAOWVScrollBar)Sender).Position;
            MigrationTurnsLB.ListOffset = ((Aow.Units.AoWVScroll.TAOWVScrollBar)Sender).Position;
            MigrationRelationLB.ListOffset = ((Aow.Units.AoWVScroll.TAOWVScrollBar)Sender).Position;
        }

        // AoWCityScreen.pas:541
        public void UFaceDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TUnit U = default;
            Aow.Rtl.TRect R = default;
            var call__193 = FCity.FProduction.CreateUnitProduction(FProductionIndex);
            U = call__193;
            if (U != null)
            {
                var call__194 = Aow.Rtl.Rtl.Rect((UPictureFramePnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + 2, (UPictureFramePnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + 2, ((UPictureFramePnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth) - 4, ((UPictureFramePnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight) - 2);
                R = call__194;
                U.ShowFace(R, SD);
                U.Release();
            }
        }

        // AoWCityScreen.pas:558
        public void QueueLBChange(Aow.Units.AoWListBox.TAOWListBox Sender)
        {
            Aow.Units.City.TCityAction Action = default;
            var call__195 = FCity.FProduction.FQueue.GetCount();
            if ((QueueLB.Selected < call__195) && (FMode != 0))
            {
                if (QueueLB.Selected != -1)
                {
                    FQueueIndex = QueueLB.Selected;
                    var call__196 = FCity.FProduction.FQueue.GetItem(FQueueIndex);
                    var call__197 = Aow.Units.City.City.ProductionIDToCityAction(call__196.FProductionID);
                    Action = call__197;
                    if (Action == Aow.Units.City.TCityAction.caProduce)
                    {
                        var call__198 = FCity.FProduction.FQueue.GetItem(FQueueIndex);
                        FProductionIndex = call__198.FParameter;
                        bool bool__199 = (FProductionIndex < 0);
                        if (!bool__199)
                        {
                            var call__200 = FCity.FProduction.FUnitProductions.GetCount();
                            bool__199 = (FProductionIndex >= call__200);
                        }

                        if (bool__199)
                        {
                            FProductionIndex = 0;
                        }
                    }

                    SelectAction(Action);
                    SetMode(2);
                    QueueLB.Selected = FQueueIndex;
                    CityBtn1.Visible = true;
                    var call__201 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeleteRStr);
                    CityBtn1.Text = call__201;
                    if ((FQueueIndex == 0) && (FCity.FProduction.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsWaiting))
                    {
                        CityBtn2.Visible = true;
                        var call__202 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ExecuteRStr);
                        CityBtn2.Text = call__202;
                    }
                    else
                    {
                        CityBtn2.Visible = false;
                    }

                    PPrev.Visible = false;
                    PNext.Visible = false;
                }
                else
                {
                    SetMode(1);
                    SelectDefaultAction();
                }
            }
            else
            {
                if (FMode == 2)
                {
                    Sender.Selected = FQueueIndex;
                }
                else
                {
                    Sender.Selected = -1;
                }
            }
        }

        // AoWCityScreen.pas:597
        public void CurrentActionImageDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWE.TUnit U = default;
            var call__203 = FCity.FProduction.FQueue.GetCount();
            if (call__203 != 0)
            {
                var call__204 = FCity.FProduction.FQueue.GetItem(0);
                switch (call__204.FProductionID)
                {
                    case 1:
                    case 2:
                        var call__205 = Aow.Rtl.Rtl.Rect(ActiveActionPnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft, ActiveActionPnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, (ActiveActionPnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth, (ActiveActionPnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight);
                        R = call__205;
                        var call__206 = FCity.FProduction.FQueue.GetItem(0);
                        var call__207 = FCity.FProduction.CreateUnitProduction(call__206.FParameter);
                        U = call__207;
                        if (U != null)
                        {
                            Info.Direction = 3;
                            Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of();
                            Info.Animation = (Aow.Units.AoWE.TUnitAnimation)0;
                            Info.Frame = 0;
                            Info.Looped = false;
                            var call__208 = FCity.FProduction.FQueue.GetItem(0);
                            var call__209 = FCity.FProduction.FUnitProductions.GetItem(call__208.FParameter);
                            if (!call__209.FResearched)
                            {
                                Info.Direction = 5;
                            }

                            U.ShowEx(R.Left - 8, R.Top - 5, SD, Info);
                            U.Release();
                        }

                        break;
                }
            }
        }

        // AoWCityScreen.pas:626
        public void LoopQueueBoxChange()
        {
            FCity.FProduction.SetLoopQueueTo(LoopQueueBox.Checked);
        }

        // AoWCityScreen.pas:631
        public void RacePortraitDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            var call__210 = Aow.Rtl.Rtl.Rect(IPPnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft, IPPnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, (IPPnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth, (IPPnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight);
            R = call__210;
            var call__211 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FCity.FRace);
            var call__212 = call__211.GetFace();
            call__212.ShowStretched(R, SD);
        }

        // AoWCityScreen.pas:641
        public void MapViewDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.GFXE.TSurfaceDesc ViewSD = default;
            Aow.Rtl.TRect R = default;
            int W = default;
            int H = default;
            int Width = default;
            if (FPreview != null)
            {
                var call__213 = Aow.Rtl.Rtl.Rect(Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft, Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, (Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth, (Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight);
                R = call__213;
                Width = R.Right - R.Left;
                W = Width;
                H = R.Bottom - R.Top;
                var call__214 = FPreview.GetMaxXhp();
                if (((FViewX + W) + 16) > call__214)
                {
                    var call__215 = FPreview.GetMaxXhp();
                    FViewX = (call__215 - W) - 16;
                }

                var call__216 = FPreview.GetMaxYhp();
                if (((FViewY + H) + 16) > call__216)
                {
                    var call__217 = FPreview.GetMaxYhp();
                    FViewY = (call__217 - H) - 16;
                }

                if (FViewX < 16)
                {
                    FViewX = 16;
                }

                if (FViewY < 16)
                {
                    FViewY = 16;
                }

                Aow.Units.GFXE.GFXE.CreateSDfromSD(SD, R, ref ViewSD);
                FPreview.SetExplorationEnabled(false);
                FPreview.SetFogEnabled(false);
                FPreview.SetViewPort(FViewX, FViewY, 0, Width, R.Bottom - R.Top);
                FPreview.UpdateStaticScene();
                FPreview.UpdateDynamicScene();
                FPreview.DrawStaticScene(ref ViewSD);
                FPreview.DrawDynamicScene(ref ViewSD);
            }
        }

        // AoWCityScreen.pas:668
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            switch (Key)
            {
                case 37:
                    FViewX -= 16;
                    MapView.FDrawn = false;
                    break;
                case 39:
                    FViewX += 16;
                    MapView.FDrawn = false;
                    break;
                case 38:
                    FViewY -= 16;
                    MapView.FDrawn = false;
                    break;
                case 40:
                    FViewY += 16;
                    MapView.FDrawn = false;
                    break;
                case 27:
                    Dlg.Visible = false;
                    break;
            }
        }

        // AoWCityScreen.pas:679
        public void MapViewUpdate()
        {
            Aow.Units.AoWTC.TAoWCombatMap Preview = default;
            Preview = FPreview;
            if (Preview != null)
            {
                Preview.NewFrame();
            }

            MapView.FDrawn = MapView.FDrawn && ((Aow.Units.AoWE.AoWE.AoWHSMap.Frame & 1) != 0);
        }

        // AoWCityScreen.pas:688
        public void DlgHide()
        {
            CloseCity();
        }

        // AoWCityScreen.pas:693
        public void UAbilitiesLBMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility A = default;
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (UAbilitiesLB.Selected > -1))
            {
                var call__218 = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
                var call__219 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__218));
                A = call__219;
                if (A != null)
                {
                    var call__220 = A.GetName(null);
                    Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__220, A.FDescription);
                }
            }
        }

        // AoWCityScreen.pas:704
        public void PPrevClick()
        {
            int Start = default;
            int Index = default;
            Aow.Units.AoWE.TPlayerStructure S = default;
            var call__221 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__222 = call__221.FStructures.FindChildIndex(FCity);
            Start = call__222;
            Index = Start;
            Index -= 1;
            if (Index < 0)
            {
                var call__223 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__224 = call__223.FStructures.GetCount();
                Index = call__224 - 1;
            }

            while (Index != Start)
            {
                var call__225 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__226 = call__225.FStructures.GetStructure(Index);
                S = call__226;
                if (S is Aow.Units.City.TCity)
                {
                    Index = Start + 1;
                    ShowCity((Aow.Units.City.TCity)S);
                }

                Index += -1;
                if (Index < 0)
                {
                    var call__227 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__228 = call__227.FStructures.GetCount();
                    Index = call__228 - 1;
                }
            }
        }

        // AoWCityScreen.pas:726
        public void PNextClick()
        {
            int Start = default;
            int Index = default;
            Aow.Units.AoWE.TPlayerStructure S = default;
            var call__229 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__230 = call__229.FStructures.FindChildIndex(FCity);
            Start = call__230;
            Index = Start + 1;
            var call__231 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__232 = call__231.FStructures.GetCount();
            if (Index >= call__232)
            {
                Index = 0;
            }

            while (Index != Start)
            {
                var call__233 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__234 = call__233.FStructures.GetStructure(Index);
                S = call__234;
                if (S is Aow.Units.City.TCity)
                {
                    Index = Start - 1;
                    ShowCity((Aow.Units.City.TCity)S);
                }

                Index += 1;
                var call__235 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__236 = call__235.FStructures.GetCount();
                if (Index >= call__236)
                {
                    Index = 0;
                }
            }
        }
    }

    public static partial class AoWCityScreen
    {
        public static Aow.Units.AoWCityScreen.TCityScreen CityScreen;
    }
}
