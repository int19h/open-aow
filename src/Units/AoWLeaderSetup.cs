// Translated from AoWLeaderSetup.pas.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWComboBox;
using static Aow.Units.AoWComboBox.AoWComboBox;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
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
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;

namespace Aow.Units.AoWLeaderSetup
{
    // { TLeaderSetupWin: size $2F0; $2D8 setup control, $2DC phase, $2E0 fixed spheres,
//     $2E4 working leader, $2E8 selected sort, $2EC available sort. VMT 00414BE0. }
    public partial class TLeaderSetupWin : Aow.Rtl.TDataModule
    {
        public TLeaderSetupWin(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWListBox.TAOWListBox SelectedAbilities;
        // { +0044 }
        public Aow.Units.AoWPanel.TAOWPanel Phase2Pnl;
        // { +0048 }
        public Aow.Units.AoWPanel.TAOWPanel LMainPnl;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton NextBtn;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton DefaultBtn;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton ClearBtn;
        // { +0058 }
        public Aow.Units.AoWButton.TAOWButton PrevBtn;
        // { +005C }
        public Aow.Units.AoWPanel.TAOWPanel EditStatisticsPnl;
        // { +0060 }
        public Aow.Units.AoWListBox.TAOWListBox AvailableAbilities;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel AttValue;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel AttLbl;
        // { +006C }
        public Aow.Units.AoWPanel.TAOWPanel AttPnl;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel DamValue;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel DamLbl;
        // { +0078 }
        public Aow.Units.AoWPanel.TAOWPanel DamPnl;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel DefValue;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel DefLbl;
        // { +0084 }
        public Aow.Units.AoWPanel.TAOWPanel DefPnl;
        // { +0088 }
        public Aow.Units.AoWLabel.TAOWLabel ResValue;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel ResLbl;
        // { +0090 }
        public Aow.Units.AoWPanel.TAOWPanel ResPnl;
        // { +0094 }
        public Aow.Units.AoWLabel.TAOWLabel HitValue;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel HitLbl;
        // { +009C }
        public Aow.Units.AoWPanel.TAOWPanel HitPnl;
        // { +00A0 }
        public Aow.Units.AoWLabel.TAOWLabel MovValue;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel MovLbl;
        // { +00A8 }
        public Aow.Units.AoWPanel.TAOWPanel MovPnl;
        // { +00AC }
        public Aow.Units.AoWLabel.TAOWLabel AttI;
        // { +00B0 }
        public Aow.Units.AoWLabel.TAOWLabel DamI;
        // { +00B4 }
        public Aow.Units.AoWLabel.TAOWLabel DefI;
        // { +00B8 }
        public Aow.Units.AoWLabel.TAOWLabel ResI;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel HitI;
        // { +00C0 }
        public Aow.Units.AoWLabel.TAOWLabel MovI;
        // { +00C4 }
        public Aow.Units.AoWLabel.TAOWLabel AttA;
        // { +00C8 }
        public Aow.Units.AoWLabel.TAOWLabel DamA;
        // { +00CC }
        public Aow.Units.AoWLabel.TAOWLabel DefA;
        // { +00D0 }
        public Aow.Units.AoWLabel.TAOWLabel ResA;
        // { +00D4 }
        public Aow.Units.AoWLabel.TAOWLabel HitA;
        // { +00D8 }
        public Aow.Units.AoWLabel.TAOWLabel MovA;
        // { +00DC }
        public Aow.Units.AoWLabel.TAOWLabel AttP;
        // { +00E0 }
        public Aow.Units.AoWLabel.TAOWLabel DamP;
        // { +00E4 }
        public Aow.Units.AoWLabel.TAOWLabel DefP;
        // { +00E8 }
        public Aow.Units.AoWLabel.TAOWLabel ResP;
        // { +00EC }
        public Aow.Units.AoWLabel.TAOWLabel HitP;
        // { +00F0 }
        public Aow.Units.AoWLabel.TAOWLabel MovP;
        // { +00F4 }
        public Aow.Units.AoWLabel.TAOWLabel AttB;
        // { +00F8 }
        public Aow.Units.AoWLabel.TAOWLabel DamB;
        // { +00FC }
        public Aow.Units.AoWLabel.TAOWLabel DefB;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel ResB;
        // { +0104 }
        public Aow.Units.AoWLabel.TAOWLabel HitB;
        // { +0108 }
        public Aow.Units.AoWLabel.TAOWLabel MovB;
        // { +010C }
        public Aow.Units.AoWLabel.TAOWLabel SkillPointsLbl;
        // { +0110 }
        public Aow.Units.AoWImage.TAOWImage UAttIcn;
        // { +0114 }
        public Aow.Units.AoWImage.TAOWImage UDefIcn;
        // { +0118 }
        public Aow.Units.AoWImage.TAOWImage UMovIcn;
        // { +011C }
        public Aow.Units.AoWImage.TAOWImage UDamIcn;
        // { +0120 }
        public Aow.Units.AoWImage.TAOWImage UResIcn;
        // { +0124 }
        public Aow.Units.AoWImage.TAOWImage UHitIcn;
        // { +0128 }
        public Aow.Units.AoWLabel.TAOWLabel UAttack;
        // { +012C }
        public Aow.Units.AoWLabel.TAOWLabel UDefense;
        // { +0130 }
        public Aow.Units.AoWLabel.TAOWLabel UMoves;
        // { +0134 }
        public Aow.Units.AoWLabel.TAOWLabel UDamage;
        // { +0138 }
        public Aow.Units.AoWLabel.TAOWLabel UResistance;
        // { +013C }
        public Aow.Units.AoWLabel.TAOWLabel UHits;
        // { +0140 }
        public Aow.Units.AoWPanel.TAOWPanel StatPnl;
        // { +0144 }
        public Aow.Units.AoWListBox.TAOWListBox UAbilities;
        // { +0148 }
        public Aow.Units.AoWPanel.TAOWPanel AbilityPnl;
        // { +014C }
        public Aow.Units.AoWPanel.TAOWPanel UStatP1;
        // { +0150 }
        public Aow.Units.AoWPanel.TAOWPanel UStatP2;
        // { +0154 }
        public Aow.Units.AoWLabel.TAOWLabel LeaderName;
        // { +0158 }
        public Aow.Units.AoWImage.TAOWImage ShieldL;
        // { +015C }
        public Aow.Units.AoWImage.TAOWImage ShieldR;
        // { +0160 }
        public Aow.Units.AoWPanel.TAOWPanel ButtonPnl;
        // { +0164 }
        public Aow.Units.AoWPanel.TAOWPanel LeftPnl;
        // { +0168 }
        public Aow.Units.AoWPanel.TAOWPanel RightPnl;
        // { +016C }
        public Aow.Units.AoWLabel.TAOWLabel LeaderStatsLbl;
        // { +0170 }
        public Aow.Units.AoWLabel.TAOWLabel LeaderAbilLbl;
        // { +0174 }
        public Aow.Units.AoWLabel.TAOWLabel FullAbilLbl;
        // { +0178 }
        public Aow.Units.AoWButton.TAOWButton RemoveAbility;
        // { +017C }
        public Aow.Units.AoWButton.TAOWButton AddAbility;
        // { +0180 }
        public Aow.Units.AoWImage.TAOWImage MainFace;
        // { +0184 }
        public Aow.Units.AoWPanel.TAOWPanel Phase1Pnl;
        // { +0188 }
        public Aow.Units.AoWImage.TAOWImage Face4;
        // { +018C }
        public Aow.Units.AoWImage.TAOWImage Face5;
        // { +0190 }
        public Aow.Units.AoWImage.TAOWImage Face6;
        // { +0194 }
        public Aow.Units.AoWImage.TAOWImage Face7;
        // { +0198 }
        public Aow.Units.AoWImage.TAOWImage Face3;
        // { +019C }
        public Aow.Units.AoWImage.TAOWImage Face2;
        // { +01A0 }
        public Aow.Units.AoWImage.TAOWImage Face1;
        // { +01A4 }
        public Aow.Units.AoWEdit.TAOWEdit NameEdit;
        // { +01A8 }
        public Aow.Units.AoWLabel.TAOWLabel NameLbl;
        // { +01AC }
        public Aow.Units.AoWLabel.TAOWLabel FaceLbl;
        // { +01B0 }
        public Aow.Units.AoWLabel.TAOWLabel RaceLbl;
        // { +01B4 }
        public Aow.Units.AoWLabel.TAOWLabel Phase1Header;
        // { +01B8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar UAbilSB;
        // { +01BC }
        public Aow.Units.AoWVScroll.TAOWVScrollBar AvailableAbilitiesLB;
        // { +01C0 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar SelectedAbilitiesLB;
        // { +01C4 }
        public Aow.Units.AoWPanel.TAOWPanel Phase3Pnl;
        // { +01C8 }
        public Aow.Units.AoWLabel.TAOWLabel SpheresLbl;
        // { +01CC }
        public Aow.Units.AoWButton.TAOWButton SelDeath;
        // { +01D0 }
        public Aow.Units.AoWPanel.TAOWPanel SpherePnl;
        // { +01D4 }
        public Aow.Units.AoWButton.TAOWButton SelLife;
        // { +01D8 }
        public Aow.Units.AoWButton.TAOWButton SelFire;
        // { +01DC }
        public Aow.Units.AoWButton.TAOWButton SelEarth;
        // { +01E0 }
        public Aow.Units.AoWButton.TAOWButton SelAir;
        // { +01E4 }
        public Aow.Units.AoWButton.TAOWButton SelWater;
        // { +01E8 }
        public Aow.Units.AoWLabel.TAOWLabel LifeLbl;
        // { +01EC }
        public Aow.Units.AoWButton.TAOWButton Sel4;
        // { +01F0 }
        public Aow.Units.AoWPanel.TAOWPanel SphereSelectionPnl;
        // { +01F4 }
        public Aow.Units.AoWButton.TAOWButton Sel3;
        // { +01F8 }
        public Aow.Units.AoWButton.TAOWButton Sel2;
        // { +01FC }
        public Aow.Units.AoWButton.TAOWButton Sel1;
        // { +0200 }
        public Aow.Units.AoWButton.TAOWButton Sel5;
        // { +0204 }
        public Aow.Units.AoWButton.TAOWButton Sel6;
        // { +0208 }
        public Aow.Units.AoWButton.TAOWButton Sel7;
        // { +020C }
        public Aow.Units.AoWLabel.TAOWLabel DeathLbl;
        // { +0210 }
        public Aow.Units.AoWLabel.TAOWLabel EarthLbl;
        // { +0214 }
        public Aow.Units.AoWLabel.TAOWLabel WaterLbl;
        // { +0218 }
        public Aow.Units.AoWLabel.TAOWLabel FireLbl;
        // { +021C }
        public Aow.Units.AoWLabel.TAOWLabel AirLbl;
        // { +0220 }
        public Aow.Units.AoWLabel.TAOWLabel SelectedSpheresLbl;
        // { +0224 }
        public Aow.Units.AoWButton.TAOWButton FaceNextBtn;
        // { +0228 }
        public Aow.Units.AoWButton.TAOWButton FacePrevBtn;
        // { +022C }
        public Aow.Units.AoWLabel.TAOWLabel LeaderLbl;
        // { +0230 }
        public Aow.Units.AoWLabel.TAOWLabel SideNameLbl;
        // { +0234 }
        public Aow.Units.AoWListBox.TAOWListBox SelectedAbilitiesCost;
        // { +0238 }
        public Aow.Units.AoWListBox.TAOWListBox AvailableAbilitiesCost;
        // { +023C }
        public Aow.Units.AoWButton.TAOWButton AttUp;
        // { +0240 }
        public Aow.Units.AoWButton.TAOWButton AttDn;
        // { +0244 }
        public Aow.Units.AoWButton.TAOWButton DamUp;
        // { +0248 }
        public Aow.Units.AoWButton.TAOWButton DamDn;
        // { +024C }
        public Aow.Units.AoWButton.TAOWButton DefUp;
        // { +0250 }
        public Aow.Units.AoWButton.TAOWButton DefDn;
        // { +0254 }
        public Aow.Units.AoWButton.TAOWButton ResUp;
        // { +0258 }
        public Aow.Units.AoWButton.TAOWButton ResDn;
        // { +025C }
        public Aow.Units.AoWButton.TAOWButton HitUp;
        // { +0260 }
        public Aow.Units.AoWButton.TAOWButton HitDn;
        // { +0264 }
        public Aow.Units.AoWButton.TAOWButton MovUp;
        // { +0268 }
        public Aow.Units.AoWButton.TAOWButton MovDn;
        // { +026C }
        public Aow.Units.AoWImage.TAOWImage AttIc;
        // { +0270 }
        public Aow.Units.AoWImage.TAOWImage DamIcn;
        // { +0274 }
        public Aow.Units.AoWImage.TAOWImage DefIc;
        // { +0278 }
        public Aow.Units.AoWImage.TAOWImage ResIc;
        // { +027C }
        public Aow.Units.AoWImage.TAOWImage HitIc;
        // { +0280 }
        public Aow.Units.AoWImage.TAOWImage MovIc;
        // { +0284 }
        public Aow.Units.AoWButton.TAOWButton SBase;
        // { +0288 }
        public Aow.Units.AoWButton.TAOWButton SExtra;
        // { +028C }
        public Aow.Units.AoWButton.TAOWButton SFinal;
        // { +0290 }
        public Aow.Units.AoWButton.TAOWButton SelectedAbilitiesSort;
        // { +0294 }
        public Aow.Units.AoWButton.TAOWButton SelectedAbilitiesCostSort;
        // { +0298 }
        public Aow.Units.AoWButton.TAOWButton AvailableAbilitiesSort;
        // { +029C }
        public Aow.Units.AoWButton.TAOWButton AvailableAbilitiesCostSort;
        // { +02A0 }
        public Aow.Units.AoWFrame.TAoWFrame MainFaceFr;
        // { +02A4 }
        public Aow.Units.AoWFrame.TAoWFrame FFr1;
        // { +02A8 }
        public Aow.Units.AoWFrame.TAoWFrame FFr7;
        // { +02AC }
        public Aow.Units.AoWFrame.TAoWFrame FFr2;
        // { +02B0 }
        public Aow.Units.AoWFrame.TAoWFrame FFr6;
        // { +02B4 }
        public Aow.Units.AoWFrame.TAoWFrame FFr3;
        // { +02B8 }
        public Aow.Units.AoWFrame.TAoWFrame FFr5;
        // { +02BC }
        public Aow.Units.AoWFrame.TAoWFrame FFr4;
        // { +02C0 }
        public Aow.Units.AoWLabel.TAOWLabel Phase2Header;
        // { +02C4 }
        public Aow.Units.AoWLabel.TAOWLabel Phase3Header;
        // { +02C8 }
        public Aow.Units.AoWPanel.TAOWPanel LeaderPnl;
        // { +02CC }
        public Aow.Units.AoWComboBox.TAOWComboBox RaceCB;
        // { +02D0 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +02D4 }
        public Aow.Units.AoWE.TLeaderSetupControl FSetup;
        // { $2D8 }
        public int FPhase;
        // { $2DC }
        public int FFixedSpheres;
        // { $2E0 }
        public Aow.Units.AoWE.TLeader FLeader;
        // { $2E4 }
        public int FSelectedSort;
        // { $2E8 }
        public int FAvailableSort;
        // AoWLeaderSetup.pas:1050
        // { $2EC }
        public void Execute(Aow.Units.AoWE.TLeaderSetupControl Control)
        {
            int I = default;
            Reset();
            if (FSetup != null)
            {
                FSetup.Release();
            }

            FSetup = Control;
            FSetup.AddRef();
            FSelectedSort = 0;
            FAvailableSort = 0;
            LeaderPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LeaderPnl.ILIndexTexture = 4;
            RaceCB.Visible = (FSetup.FItems.Count != 0) && FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign);
            RaceLbl.Visible = RaceCB.Visible;
            if (RaceCB.Visible)
            {
                RaceCB.Items.Clear();
                {
                    I = 0;
                    int end__1 = FSetup.FItems.Count - 1;
                    for (; I <= end__1; I += 1)
                    {
                        var call__2 = FSetup.FItems[I];
                        var call__3 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[call__2]);
                        var call__4 = FSetup.FItems[I];
                        RaceCB.Items.AddObject(call__3, Aow.Rtl.Rtl.IntToObject((int)call__4));
                    }
                }

                var call__5 = FSetup.FItems.IndexOf(FSetup.FOriginalLeader.FRace);
                RaceCB.Selected = call__5;
            }

            var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[FSetup.FOriginalLeader.FRace]);
            SideNameLbl.Text = call__6;
            ShieldL.CurrentImage = FSetup.FOriginalLeader.FRace + 13;
            ShieldR.CurrentImage = FSetup.FOriginalLeader.FRace + 13;
            ShieldR.Visible = !FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign);
            LoadLeader();
            SaveLeader();
            if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmLook))
            {
                SetPhase(1);
            }
            else
            {
                if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSettings))
                {
                    SetPhase(2);
                }
                else
                {
                    if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSpheres))
                    {
                        SetPhase(3);
                    }
                }
            }

            LeaderPnl.Visible = true;
        }

        // AoWLeaderSetup.pas:1083
        public void Finish()
        {
            Aow.Units.AoWE.TLeaderSetupControl Control = default;
            SaveLeader();
            Control = FSetup;
            Control.FStatus = Aow.Units.AoWE.TLeaderSetupStatus.lssDone;
            FSetup = null;
            LeaderPnl.Visible = false;
            Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.UpdatePlayerName(Control.FOriginalLeader.FHeroName);
            Control.Done();
            Control.Release();
        }

        // AoWLeaderSetup.pas:1097
        public void Cancel()
        {
            Aow.Units.AoWE.TLeaderSetupControl Control = default;
            SaveLeader();
            Control = FSetup;
            Control.FStatus = Aow.Units.AoWE.TLeaderSetupStatus.lssCanceled;
            FSetup = null;
            LeaderPnl.Visible = false;
            Control.Done();
            Control.Release();
        }

        // AoWLeaderSetup.pas:750
        public void LoadLeader()
        {
            FLeader.Assign(FSetup.FOriginalLeader, Aow.Units.AoWE.AoWE.AoWEngine);
        }

        // AoWLeaderSetup.pas:755
        public void SaveLeader()
        {
            Aow.Units.AoWE.TLeader Leader = default;
            FSetup.FOriginalLeader.Assign(FLeader, Aow.Units.AoWE.AoWE.AoWEngine);
            Leader = FSetup.FOriginalLeader;
            LeaderName.Text = Leader.FHeroName;
            var call__7 = Leader.GetInherentAttack();
            UAttack.Text = Aow.Rtl.Rtl.IntToStr(call__7);
            var call__8 = Leader.GetInherentDefense();
            UDefense.Text = Aow.Rtl.Rtl.IntToStr(call__8);
            var call__9 = Leader.GetInherentDamage();
            UDamage.Text = Aow.Rtl.Rtl.IntToStr(call__9);
            var call__10 = Leader.GetInherentResistance();
            UResistance.Text = Aow.Rtl.Rtl.IntToStr(call__10);
            var call__11 = Leader.GetHits();
            UHits.Text = Aow.Rtl.Rtl.IntToStr(call__11);
            var call__12 = Leader.GetMoves();
            UMoves.Text = Aow.Rtl.Rtl.IntToStr(call__12);
            UAbilities.Items.Clear();
            Leader.ListAbilities(UAbilities.Items);
            var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[Leader.FRace]);
            SideNameLbl.Text = call__13;
            ShieldL.CurrentImage = Leader.FRace + 13;
            ShieldR.CurrentImage = Leader.FRace + 13;
        }

        // AoWLeaderSetup.pas:775
        public void UpdateSkillPoints()
        {
            var call__14 = FLeader.GetSkillPoints();
            var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SkillPointsRStr);
            var call__16 = FLeader.GetSkillPointsMax();
            SkillPointsLbl.Text = (((call__15 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(call__14)) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__16);
        }

        // AoWLeaderSetup.pas:781
        public void UpdateStatistics()
        {
            Aow.Units.AoWE.THeroResource Res = default;
            Res = FLeader.FHeroResource;
            AttB.Text = Aow.Rtl.Rtl.IntToStr(Res.FAttack);
            DefB.Text = Aow.Rtl.Rtl.IntToStr(Res.FDefense);
            DamB.Text = Aow.Rtl.Rtl.IntToStr(Res.FDamage);
            ResB.Text = Aow.Rtl.Rtl.IntToStr(Res.FResistance);
            HitB.Text = Aow.Rtl.Rtl.IntToStr(Res.FHits);
            MovB.Text = Aow.Rtl.Rtl.IntToStr(Res.FMoves);
            AttA.Text = Aow.Rtl.Rtl.IntToStr(FLeader.FUnitAttack);
            DefA.Text = Aow.Rtl.Rtl.IntToStr(FLeader.FUnitDefense);
            DamA.Text = Aow.Rtl.Rtl.IntToStr(FLeader.FUnitDamage);
            ResA.Text = Aow.Rtl.Rtl.IntToStr(FLeader.FUnitResistance);
            HitA.Text = Aow.Rtl.Rtl.IntToStr(FLeader.FUnitHits);
            MovA.Text = Aow.Rtl.Rtl.IntToStr(FLeader.FUnitMoves);
            var call__17 = FLeader.GetInherentAttack();
            AttValue.Text = Aow.Rtl.Rtl.IntToStr(call__17);
            var call__18 = FLeader.GetInherentDefense();
            DefValue.Text = Aow.Rtl.Rtl.IntToStr(call__18);
            var call__19 = FLeader.GetInherentDamage();
            DamValue.Text = Aow.Rtl.Rtl.IntToStr(call__19);
            var call__20 = FLeader.GetInherentResistance();
            ResValue.Text = Aow.Rtl.Rtl.IntToStr(call__20);
            var call__21 = FLeader.GetHits();
            HitValue.Text = Aow.Rtl.Rtl.IntToStr(call__21);
            var call__22 = FLeader.GetMoves();
            MovValue.Text = Aow.Rtl.Rtl.IntToStr(call__22);
            UpdateSkillPoints();
        }

        // AoWLeaderSetup.pas:807
        public void UpdateLeader()
        {
            NameEdit.Text = FLeader.FHeroName;
            UpdateAbilities();
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:814
        public void SortSelectedAbilities()
        {
            switch (FSelectedSort)
            {
                case 0:
                    Aow.Units.Engine.Engine.SortStringsAscending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsDescending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsNumericalAscending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsNumericalDescending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                    break;
            }
        }

        // AoWLeaderSetup.pas:830
        public void SortAvailableAbilities()
        {
            switch (FAvailableSort)
            {
                case 0:
                    Aow.Units.Engine.Engine.SortStringsAscending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsDescending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsNumericalAscending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsNumericalDescending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                    break;
            }
        }

        // AoWLeaderSetup.pas:852
        public void UpdateAbilities()
        {
            int I = default;
            Aow.Rtl.AnsiString Name = default;
            Aow.Rtl.AnsiString Cost = default;
            Aow.Units.AoWE.TAbility Ability = default;
            AvailableAbilities.Items.Clear();
            AvailableAbilitiesCost.Items.Clear();
            {
                I = 0;
                int end__23 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetCount() - 1;
                for (; I <= end__23; I += 1)
                {
                    var call__24 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                    bool bool__25 = (call__24 != null);
                    if (bool__25)
                    {
                        var call__26 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                        bool__25 = ((call__26.FSelectionTypes & TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astCustomizeLeader, Aow.Units.AoWE.TAbilitySelectionType.astHeroUpgrade)) != TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of());
                    }

                    if (bool__25)
                    {
                        var call__27 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                        var call__28 = call__27.CanExpand(FLeader);
                        if (call__28)
                        {
                            var call__29 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__30 = call__29.ExpandName(FLeader);
                            AvailableAbilities.Items.AddObject(call__30, Aow.Rtl.Rtl.IntToObject((int)I));
                            var call__31 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__32 = call__31.ExpandCost(FLeader);
                            AvailableAbilitiesCost.Items.Add(Aow.Rtl.Rtl.IntToStr(call__32));
                        }
                    }
                }
            }

            SelectedAbilities.Items.Clear();
            SelectedAbilitiesCost.Items.Clear();
            {
                I = 0;
                int end__33 = FLeader.GetAbCount() - 1;
                for (; I <= end__33; I += 1)
                {
                    var call__34 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                    Ability = call__34;
                    var call__35 = FLeader.GetAbSet(I);
                    bool bool__36 = call__35;
                    if (bool__36)
                    {
                        var call__37 = FLeader.FHeroResource.FAbilities.GetAbSet(I);
                        bool__36 = (call__37 || ((Ability.FSelectionTypes & TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astCustomizeLeader, Aow.Units.AoWE.TAbilitySelectionType.astHeroUpgrade)) != TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of()));
                    }

                    if (bool__36)
                    {
                        var call__38 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                        var call__39 = call__38.GetName(FLeader);
                        Name = call__39;
                        var call__40 = FLeader.FHeroResource.FAbilities.GetAbSet(I);
                        bool bool__41 = call__40;
                        if (bool__41)
                        {
                            var call__42 = FLeader.FHeroResource.FAbilities.GetAbLevel(I);
                            var call__43 = FLeader.GetAbilityLevel(I);
                            bool__41 = (call__42 == call__43);
                        }

                        if (bool__41)
                        {
                            Name = ((Name + ((AnsiString)" (")) + Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FixedRStr)) + AnsiString.FromBytes(41);
                            Cost = AnsiString.FromBytes(45);
                        }
                        else
                        {
                            var call__44 = Ability.GetSkillPoints(FLeader);
                            var call__45 = Ability.GetSkillPoints(FLeader.FHeroResource.FAbilities);
                            Cost = Aow.Rtl.Rtl.IntToStr(call__44 - call__45);
                        }

                        SelectedAbilitiesCost.Items.Add(Cost);
                        SelectedAbilities.Items.AddObject(Name, Aow.Rtl.Rtl.IntToObject((int)I));
                    }
                }
            }

            var call__46 = AvailableAbilities.Items.Count;
            AvailableAbilitiesLB.Max = call__46 - 1;
            var call__47 = SelectedAbilities.Items.Count;
            SelectedAbilitiesLB.Max = call__47 - 1;
            SortAvailableAbilities();
            SortSelectedAbilities();
        }

        // AoWLeaderSetup.pas:840
        public void SelectAvailableAbility(int Index)
        {
            AvailableAbilities.Selected = Index;
            AvailableAbilitiesCost.Selected = Index;
        }

        // AoWLeaderSetup.pas:824
        public void SelectSelectedAbility(int Index)
        {
            SelectedAbilities.Selected = Index;
            SelectedAbilitiesCost.Selected = Index;
        }

        // AoWLeaderSetup.pas:846
        public bool CanAddAbility(int AbilityID)
        {
            bool Result = default;
            var call__48 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(AbilityID);
            var call__49 = call__48.CanExpand(FLeader);
            bool bool__50 = call__49;
            if (bool__50)
            {
                var call__51 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(AbilityID);
                var call__52 = call__51.ExpandCost(FLeader);
                var call__53 = FLeader.GetSkillPoints();
                bool__50 = (call__52 <= call__53);
            }

            Result = bool__50;
            return Result;
        }

        // AoWLeaderSetup.pas:913
        public void SetPhase(int Value)
        {
            FPhase = Value;
            Phase1Pnl.Visible = Value == 1;
            Phase2Pnl.Visible = Value == 2;
            Phase3Pnl.Visible = Value == 3;
            Phase1Header.Visible = Value == 1;
            Phase2Header.Visible = Value == 2;
            Phase3Header.Visible = Value == 3;
            StatPnl.Visible = Value > 2;
            AbilityPnl.Visible = Value > 2;
            LeaderName.Visible = Value > 1;
            LeaderLbl.Visible = !FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign);
            SideNameLbl.Visible = !FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign);
            ShieldL.Visible = !FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign) || (Value > 1);
            ShieldR.Visible = !FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign) || (Value > 1);
            MainFaceFr.Visible = !FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign) || (Value > 1);
            MainFace.Visible = MainFaceFr.Visible;
            if (((Value == 3) || ((Value == 2) && ((FSetup.FMode & TSet<Aow.Units.AoWE.TLeaderSetupModeElement>.Of(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSpheres)) == TSet<Aow.Units.AoWE.TLeaderSetupModeElement>.Of()))) || ((Value == 1) && ((FSetup.FMode & TSet<Aow.Units.AoWE.TLeaderSetupModeElement>.Of(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSettings, Aow.Units.AoWE.TLeaderSetupModeElement.lsmSpheres)) == TSet<Aow.Units.AoWE.TLeaderSetupModeElement>.Of())))
            {
                var call__54 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FinishRStr);
                NextBtn.Text = call__54;
            }
            else
            {
                var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NextRStr);
                NextBtn.Text = call__55;
            }

            switch (Value)
            {
                case 1:
                    UpdateLeader();
                    break;
                case 2:
                    UpdateLeader();
                    break;
                case 3:
                    UpdateSpheres();
                    break;
            }
        }

        // AoWLeaderSetup.pas:895
        public void SetFixedSpheres(int Value)
        {
            FFixedSpheres = Value;
            UpdateSpheres();
        }

        // AoWLeaderSetup.pas:901
        public void Reset()
        {
            ClearBtn.Visible = true;
            DefaultBtn.Visible = true;
            PrevBtn.Visible = true;
            NextBtn.Visible = true;
            var call__56 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.Step1CustomizeLeaderRStr);
            Phase1Header.Text = call__56;
            var call__57 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.Step2AssignSkillPointsRStr);
            Phase2Header.Text = call__57;
            var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.Step3AssignSpheresRStr);
            Phase3Header.Text = call__58;
            SetFixedSpheres(0);
        }

        // AoWLeaderSetup.pas:943
        public void UpdateSpheres()
        {
            int I = default;
            Aow.Units.AoWButton.TAOWButton Btn = default;
            {
                I = 0;
                byte end__59 = 6;
                for (; I <= end__59; I += 1)
                {
                    switch (I)
                    {
                        case 0:
                            Btn = Sel1;
                            break;
                        case 1:
                            Btn = Sel2;
                            break;
                        case 2:
                            Btn = Sel3;
                            break;
                        case 3:
                            Btn = Sel4;
                            break;
                        case 4:
                            Btn = Sel5;
                            break;
                        case 5:
                            Btn = Sel6;
                            break;
                        case 6:
                            Btn = Sel7;
                            break;
                        default:
                            Btn = null;
                            break;
                    }

                    var call__60 = FLeader.GetSphereCount();
                    if (I < call__60)
                    {
                        Btn.Visible = true;
                        var call__61 = FLeader.GetSpheres(I);
                        switch (unchecked((int)call__61) & 127)
                        {
                            case 1:
                                Btn.ILIndexDown = 28;
                                Btn.ILIndexLit = 29;
                                Btn.ILIndexUp = 27;
                                break;
                            case 5:
                                Btn.ILIndexDown = 43;
                                Btn.ILIndexLit = 44;
                                Btn.ILIndexUp = 42;
                                break;
                            case 3:
                                Btn.ILIndexDown = 34;
                                Btn.ILIndexLit = 35;
                                Btn.ILIndexUp = 33;
                                break;
                            case 4:
                                Btn.ILIndexDown = 37;
                                Btn.ILIndexLit = 38;
                                Btn.ILIndexUp = 36;
                                break;
                            case 6:
                                Btn.ILIndexDown = 40;
                                Btn.ILIndexLit = 41;
                                Btn.ILIndexUp = 39;
                                break;
                            case 2:
                                Btn.ILIndexDown = 31;
                                Btn.ILIndexLit = 32;
                                Btn.ILIndexUp = 30;
                                break;
                        }
                    }
                    else
                    {
                        Btn.Visible = false;
                    }

                    if (I < FFixedSpheres)
                    {
                        Btn.FCaptureInput = false;
                        Btn.ILIndexUp = Btn.ILIndexUp + 1;
                    }
                    else
                    {
                        Btn.FCaptureInput = true;
                    }
                }
            }

            var call__62 = FLeader.GetSphereCount();
            var call__63 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectedSpheresRStr);
            SelectedSpheresLbl.Text = ((((call__63 + ((AnsiString)" (")) + Aow.Rtl.Rtl.IntToStr(call__62)) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(FLeader.FSpherePicksMax)) + AnsiString.FromBytes(41);
            var call__64 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msLife);
            if (call__64)
            {
                SelLife.ILIndexUp = 27;
                SelLife.FCaptureInput = true;
            }
            else
            {
                SelLife.ILIndexUp = 28;
                SelLife.FCaptureInput = false;
            }

            var call__65 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msDeath);
            if (call__65)
            {
                SelDeath.ILIndexUp = 30;
                SelDeath.FCaptureInput = true;
            }
            else
            {
                SelDeath.ILIndexUp = 31;
                SelDeath.FCaptureInput = false;
            }

            var call__66 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msFire);
            if (call__66)
            {
                SelFire.ILIndexUp = 42;
                SelFire.FCaptureInput = true;
            }
            else
            {
                SelFire.ILIndexUp = 43;
                SelFire.FCaptureInput = false;
            }

            var call__67 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msWater);
            if (call__67)
            {
                SelWater.ILIndexUp = 39;
                SelWater.FCaptureInput = true;
            }
            else
            {
                SelWater.ILIndexUp = 40;
                SelWater.FCaptureInput = false;
            }

            var call__68 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msEarth);
            if (call__68)
            {
                SelEarth.ILIndexUp = 33;
                SelEarth.FCaptureInput = true;
            }
            else
            {
                SelEarth.ILIndexUp = 34;
                SelEarth.FCaptureInput = false;
            }

            var call__69 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msAir);
            if (call__69)
            {
                SelAir.ILIndexUp = 36;
                SelAir.FCaptureInput = true;
            }
            else
            {
                SelAir.ILIndexUp = 37;
                SelAir.FCaptureInput = false;
            }

            SelLife.FDrawn = false;
            SelDeath.FDrawn = false;
            SelFire.FDrawn = false;
            SelWater.FDrawn = false;
            SelEarth.FDrawn = false;
            SelAir.FDrawn = false;
        }

        // AoWLeaderSetup.pas:269
        public void LeaderSetupWinCreate()
        {
            var call__70 = new Aow.Units.AoWE.TLeader(null);
            FLeader = call__70;
        }

        // AoWLeaderSetup.pas:274
        public void LeaderSetupWinDestroy()
        {
            FLeader.Free();
        }

        // AoWLeaderSetup.pas:279
        public void AddAbilityClick()
        {
            int AbilityID = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (AvailableAbilities.Selected != -1)
            {
                var call__71 = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
                AbilityID = Aow.Rtl.Rtl.ObjectToInt(call__71);
                if (CanAddAbility(AbilityID))
                {
                    FLeader.ExpandAbility(AbilityID);
                    UpdateAbilities();
                    UpdateLeader();
                }
            }
        }

        // AoWLeaderSetup.pas:296
        public void RemoveAbilityClick()
        {
            int AbilityID = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (SelectedAbilities.Selected != -1)
            {
                var call__72 = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
                AbilityID = Aow.Rtl.Rtl.ObjectToInt(call__72);
                var call__73 = FLeader.FHeroResource.FAbilities.GetAbSet(AbilityID);
                bool bool__74 = call__73;
                if (bool__74)
                {
                    var call__75 = FLeader.FHeroResource.FAbilities.GetAbLevel(AbilityID);
                    var call__76 = FLeader.GetAbilityLevel(AbilityID);
                    bool__74 = (call__75 == call__76);
                }

                if (bool__74)
                {
                    return;
                }

                FLeader.RemoveAbility(AbilityID);
                UpdateAbilities();
                UpdateLeader();
            }
        }

        // AoWLeaderSetup.pas:312
        public void DrawSelectionFace(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TLeaderSetupControl Setup = default;
            int Index = default;
            int Offset = default;
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWE.TFaceResource Face = default;
            Setup = FSetup;
            Offset = ((Aow.Rtl.TComponent)Sender).Tag;
            Index = FLeader.FFaceIndex;
            while (Offset < 0)
            {
                Offset += 1;
                var call__77 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.PrevFace(Index);
                Index = call__77;
            }

            while (Offset > 0)
            {
                Offset -= 1;
                var call__78 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.NextFace(Index);
                Index = call__78;
            }

            var image = (Aow.Units.AoWImage.TAOWImage)Sender;
            var call__80 = Aow.Rtl.Rtl.Rect((Phase1Pnl.WinLeft + image.WinLeft) + 2, Phase1Pnl.WinTop + image.WinTop, ((Phase1Pnl.WinLeft + image.WinLeft) + image.WinWidth) - 2, ((Phase1Pnl.WinTop + image.WinTop) + image.WinHeight) - 2);
            R = call__80;

            var call__81 = Setup.FOriginalLeader.GetFace();
            if (call__81 != null)
            {
                var call__82 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.GetFaceResource(Index);
                Face = call__82;
                Face.AddRef();
                var call__83 = Face.GetFaceImage();
                call__83.ShowStretched(R, SD);
                Face.Release();
            }
        }

        // AoWLeaderSetup.pas:345
        public void FacePrevBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__84 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.PrevFace(FLeader.FFaceIndex);
            FLeader.SetFaceIndex(call__84);
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWLeaderSetup.pas:352
        public void FaceNextBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__85 = Aow.Units.AoWE.AoWE.AoWHSSet.FFaceResources.NextFace(FLeader.FFaceIndex);
            FLeader.SetFaceIndex(call__85);
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWLeaderSetup.pas:359
        public void PrevBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            switch (FPhase)
            {
                case 1:
                    if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmCampaign))
                    {
                        Cancel();
                    }

                    break;
                case 2:
                    SaveLeader();
                    if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmLook))
                    {
                        SetPhase(1);
                    }

                    break;
                case 3:
                    SaveLeader();
                    if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSettings))
                    {
                        SetPhase(2);
                    }
                    else
                    {
                        if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmLook))
                        {
                            SetPhase(1);
                        }
                    }

                    break;
            }
        }

        // AoWLeaderSetup.pas:376
        public void NextBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            switch (FPhase)
            {
                case 1:
                    SaveLeader();
                    if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSettings))
                    {
                        SetPhase(2);
                    }
                    else
                    {
                        if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSpheres))
                        {
                            SetPhase(3);
                        }
                        else
                        {
                            Finish();
                        }
                    }

                    Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
                    break;
                case 2:
                    SaveLeader();
                    if (FSetup.FMode.Contains(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSpheres))
                    {
                        SetPhase(3);
                    }
                    else
                    {
                        Finish();
                    }

                    Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
                    break;
                case 3:
                    var call__86 = FLeader.GetSphereCount();
                    if (call__86 == FLeader.FSpherePicksMax)
                    {
                        Finish();
                    }
                    else
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
                        Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                        var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PleaseAssignAllSpheresRStr);
                        Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__87);
                        Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                        Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
                    }

                    break;
            }
        }

        // AoWLeaderSetup.pas:405
        public void MainFaceFrPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TLeaderSetupControl Setup = default;
            Aow.Rtl.TRect R = default;
            Setup = FSetup;
            if (FPhase > 1)
            {
                var frame = (Aow.Units.AoWFrame.TAoWFrame)Sender;
                var call__89 = Aow.Rtl.Rtl.Rect((LMainPnl.WinLeft + frame.WinLeft) + 2, LMainPnl.WinTop + frame.WinTop, ((LMainPnl.WinLeft + frame.WinLeft) + frame.WinWidth) - 2, ((LMainPnl.WinTop + frame.WinTop) + frame.WinHeight) - 2);
                R = call__89;

                var call__90 = Setup.FOriginalLeader.GetFace();
                if (call__90 != null)
                {
                    var call__91 = Setup.FOriginalLeader.GetFace();
                    call__91.ShowStretched(R, SD);
                }
            }
        }

        // AoWLeaderSetup.pas:422
        public void NameEditChange()
        {
            FLeader.FHeroName = NameEdit.Text;
        }

        // AoWLeaderSetup.pas:427
        public void SelectedAbilitiesCostChange()
        {
            SelectSelectedAbility(SelectedAbilitiesCost.Selected);
        }

        // AoWLeaderSetup.pas:432
        public void SelectedAbilitiesChange()
        {
            SelectSelectedAbility(SelectedAbilities.Selected);
        }

        // AoWLeaderSetup.pas:437
        public void AvailableAbilitiesCostChange()
        {
            SelectAvailableAbility(AvailableAbilitiesCost.Selected);
        }

        // AoWLeaderSetup.pas:442
        public void AvailableAbilitiesChange()
        {
            SelectAvailableAbility(AvailableAbilities.Selected);
        }

        // AoWLeaderSetup.pas:447
        public void SelectedAbilitiesLBChange()
        {
            SelectedAbilities.ListOffset = SelectedAbilitiesLB.Position;
            SelectedAbilitiesCost.ListOffset = SelectedAbilitiesLB.Position;
        }

        // AoWLeaderSetup.pas:453
        public void AvailableAbilitiesLBChange()
        {
            AvailableAbilities.ListOffset = AvailableAbilitiesLB.Position;
            AvailableAbilitiesCost.ListOffset = AvailableAbilitiesLB.Position;
        }

        // AoWLeaderSetup.pas:459
        public void AttUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitAttack(unchecked((sbyte)(FLeader.FUnitAttack + 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:466
        public void AttDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitAttack(unchecked((sbyte)(FLeader.FUnitAttack - 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:473
        public void DefUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitDefense(unchecked((sbyte)(FLeader.FUnitDefense + 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:480
        public void DefDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitDefense(unchecked((sbyte)(FLeader.FUnitDefense - 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:487
        public void DamUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitDamage(unchecked((sbyte)(FLeader.FUnitDamage + 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:494
        public void DamDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitDamage(unchecked((sbyte)(FLeader.FUnitDamage - 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:501
        public void ResUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitResistance(unchecked((sbyte)(FLeader.FUnitResistance + 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:508
        public void ResDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitResistance(unchecked((sbyte)(FLeader.FUnitResistance - 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:515
        public void HitUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitHits(unchecked((sbyte)(FLeader.FUnitHits + 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:522
        public void HitDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitHits(unchecked((sbyte)(FLeader.FUnitHits - 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:529
        public void MovUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitMoves(unchecked((sbyte)(FLeader.FUnitMoves + 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:536
        public void MovDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FLeader.SetUnitMoves(unchecked((sbyte)(FLeader.FUnitMoves - 1)));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:543
        public void ClearBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            switch (FPhase)
            {
                case 1:
                {
                    var leader = FLeader;
                    leader.SetNickname((AnsiString)"");
                    leader.FHeroName = (AnsiString)"";
                    UpdateLeader();
                }

                    break;
                case 2:
                {
                    var leader2 = FLeader;
                    leader2.AssignAb(leader2.FHeroResource.FAbilities);
                    leader2.SetUnitAttack(0);
                    leader2.SetUnitDefense(0);
                    leader2.SetUnitDamage(0);
                    leader2.SetUnitHits(0);
                    leader2.SetUnitMoves(0);
                    leader2.SetUnitResistance(0);
                    UpdateLeader();
                }

                    break;
                case 3:
                    FLeader.ClearSpheres();
                    UpdateSpheres();
                    break;
            }
        }

        // AoWLeaderSetup.pas:568
        public void DefaultBtnClick(Aow.Rtl.TObject Sender)
        {
            int I = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
            switch (FPhase)
            {
                case 1:
                {
                    var leader = FLeader;
                    var call__95 = FSetup.FLeader.GetNickname();
                    leader.SetNickname(call__95);
                    leader.FHeroName = FSetup.FLeader.FHeroName;
                    leader.SetFaceIndex(FSetup.FLeader.FFaceIndex);
                    UpdateLeader();
                }

                    break;
                case 2:
                {
                    var leader2 = FLeader;
                    leader2.AssignAb(FSetup.FLeader);
                    leader2.SetUnitAttack(unchecked((sbyte)FSetup.FLeader.FUnitAttack));
                    leader2.SetUnitDefense(unchecked((sbyte)FSetup.FLeader.FUnitDefense));
                    leader2.SetUnitDamage(unchecked((sbyte)FSetup.FLeader.FUnitDamage));
                    leader2.SetUnitHits(unchecked((sbyte)FSetup.FLeader.FUnitHits));
                    leader2.SetUnitMoves(unchecked((sbyte)FSetup.FLeader.FUnitMoves));
                    leader2.SetUnitResistance(unchecked((sbyte)FSetup.FLeader.FUnitResistance));
                    UpdateLeader();
                }

                    break;
                case 3:
                {
                    var leader3 = FLeader;
                    leader3.ClearSpheres();
                    {
                        I = 0;
                        int end__98 = FSetup.FLeader.GetSphereCount() - 1;
                        for (; I <= end__98; I += 1)
                        {
                            var call__99 = FSetup.FLeader.GetSpheres(I);
                            leader3.AddSphere(call__99);
                        }
                    }

                    UpdateLeader();
                    UpdateSpheres();
                }

                    break;
            }
        }

        // AoWLeaderSetup.pas:604
        public void SelLifeClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__100 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msLife);
            if (call__100)
            {
                FLeader.AddSphere(Aow.Units.AoWE.TMagicSphere.msLife);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:614
        public void SelFireClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__101 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msFire);
            if (call__101)
            {
                FLeader.AddSphere(Aow.Units.AoWE.TMagicSphere.msFire);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:624
        public void SelDeathClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__102 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msDeath);
            if (call__102)
            {
                FLeader.AddSphere(Aow.Units.AoWE.TMagicSphere.msDeath);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:634
        public void SelWaterClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__103 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msWater);
            if (call__103)
            {
                FLeader.AddSphere(Aow.Units.AoWE.TMagicSphere.msWater);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:644
        public void SelAirClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__104 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msAir);
            if (call__104)
            {
                FLeader.AddSphere(Aow.Units.AoWE.TMagicSphere.msAir);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:654
        public void SelEarthClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__105 = FLeader.CanAddSphere(Aow.Units.AoWE.TMagicSphere.msEarth);
            if (call__105)
            {
                FLeader.AddSphere(Aow.Units.AoWE.TMagicSphere.msEarth);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:664
        public void DeleteSphereClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__106 = FLeader.GetSphereCount();
            if (((Aow.Rtl.TComponent)Sender).Tag < call__106)
            {
                FLeader.DeleteSphereIndex(((Aow.Rtl.TComponent)Sender).Tag);
            }

            UpdateSpheres();
        }

        // AoWLeaderSetup.pas:672
        public void RaceCBChange()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__107 = RaceCB.Items.Objects[RaceCB.Selected];
            FLeader.SetRace(unchecked((sbyte)Aow.Rtl.Rtl.ObjectToInt(call__107)));
        }

        // AoWLeaderSetup.pas:678
        public void SelectedAbilitiesSortClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FSelectedSort == 0)
            {
                FSelectedSort = 1;
            }
            else
            {
                FSelectedSort = 0;
            }

            SortSelectedAbilities();
        }

        // AoWLeaderSetup.pas:686
        public void SelectedAbilitiesCostSortClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FSelectedSort == 2)
            {
                FSelectedSort = 3;
            }
            else
            {
                FSelectedSort = 2;
            }

            SortSelectedAbilities();
        }

        // AoWLeaderSetup.pas:694
        public void AvailableAbilitiesSortClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FAvailableSort == 0)
            {
                FAvailableSort = 1;
            }
            else
            {
                FAvailableSort = 0;
            }

            SortAvailableAbilities();
        }

        // AoWLeaderSetup.pas:702
        public void AvailableAbilitiesCostSortClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FAvailableSort == 2)
            {
                FAvailableSort = 3;
            }
            else
            {
                FAvailableSort = 2;
            }

            SortAvailableAbilities();
        }

        // AoWLeaderSetup.pas:710
        public void SBaseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWLeaderSetup.pas:715
        public void SExtraClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWLeaderSetup.pas:720
        public void SFinalClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWLeaderSetup.pas:725
        public void SelectedAbilitiesCostMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility Ability = default;
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (SelectedAbilities.Selected != -1))
            {
                var call__108 = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
                var call__109 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__108));
                Ability = call__109;
                if (Ability != null)
                {
                    var call__110 = Ability.GetName(null);
                    Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__110, Ability.FDescription);
                }
            }
        }

        // AoWLeaderSetup.pas:737
        public void AvailableAbilitiesMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility Ability = default;
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (AvailableAbilities.Selected != -1))
            {
                var call__111 = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
                var call__112 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__111));
                Ability = call__112;
                if (Ability != null)
                {
                    var call__113 = Ability.GetName(null);
                    Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__113, Ability.FDescription);
                }
            }
        }
    }

    public static partial class AoWLeaderSetup
    {
        public static Aow.Units.AoWLeaderSetup.TLeaderSetupWin LeaderSetupWin;
    }
}
