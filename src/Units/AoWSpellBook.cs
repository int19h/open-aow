// Translated from AoWSpellBook.pas.
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
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.PageTurnCtrl;
using static Aow.Units.PageTurnCtrl.PageTurnCtrl;
using Aow.Units.AoWBookWindow;
using static Aow.Units.AoWBookWindow.AoWBookWindow;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWSpellBook
{
    // { TSpellBook (instance size $248, VMT 0042DDA8): $220 mode, $221 page kind, $222 spell type, $224 spells, $228 page, $22C hero, $230 activation, $234 researched spell, $238 callback, $240 event, $244 saved player pages. }
    public partial class TSpellBook : Aow.Rtl.TDataModule
    {
        public TSpellBook(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWLabel.TAOWLabel LeftHeader;
        // { +0044 }
        public Aow.Units.AoWPanel.TAOWPanel LeftSpellPage;
        // { +0048 }
        public Aow.Units.AoWPanel.TAOWPanel S1Pnl;
        // { +004C }
        public Aow.Units.AoWLabel.TAOWLabel S1Spell;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel S1Class;
        // { +0054 }
        public Aow.Units.AoWMemo.TAOWMemo S1Memo;
        // { +0058 }
        public Aow.Units.AoWLabel.TAOWLabel S1Cost;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage S1Icn;
        // { +0060 }
        public Aow.Units.AoWPanel.TAOWPanel S2Pnl;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel S2Spell;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel S2Class;
        // { +006C }
        public Aow.Units.AoWMemo.TAOWMemo S2Memo;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel S2Cost;
        // { +0074 }
        public Aow.Units.AoWImage.TAOWImage S2Icn;
        // { +0078 }
        public Aow.Units.AoWPanel.TAOWPanel S3Pnl;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel S3Spell;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel S3Class;
        // { +0084 }
        public Aow.Units.AoWMemo.TAOWMemo S3Memo;
        // { +0088 }
        public Aow.Units.AoWLabel.TAOWLabel S3Cost;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage S3Icn;
        // { +0090 }
        public Aow.Units.AoWPanel.TAOWPanel S4Pnl;
        // { +0094 }
        public Aow.Units.AoWLabel.TAOWLabel S4Spell;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel S4Class;
        // { +009C }
        public Aow.Units.AoWMemo.TAOWMemo S4Memo;
        // { +00A0 }
        public Aow.Units.AoWLabel.TAOWLabel S4Cost;
        // { +00A4 }
        public Aow.Units.AoWImage.TAOWImage S4Icn;
        // { +00A8 }
        public Aow.Units.AoWLabel.TAOWLabel RightHeader;
        // { +00AC }
        public Aow.Units.AoWPanel.TAOWPanel RightSpellPage;
        // { +00B0 }
        public Aow.Units.AoWPanel.TAOWPanel S5Pnl;
        // { +00B4 }
        public Aow.Units.AoWLabel.TAOWLabel S5Spell;
        // { +00B8 }
        public Aow.Units.AoWLabel.TAOWLabel S5Class;
        // { +00BC }
        public Aow.Units.AoWMemo.TAOWMemo S5Memo;
        // { +00C0 }
        public Aow.Units.AoWLabel.TAOWLabel S5Cost;
        // { +00C4 }
        public Aow.Units.AoWImage.TAOWImage S5Icn;
        // { +00C8 }
        public Aow.Units.AoWPanel.TAOWPanel S6Pnl;
        // { +00CC }
        public Aow.Units.AoWLabel.TAOWLabel S6Spell;
        // { +00D0 }
        public Aow.Units.AoWLabel.TAOWLabel S6Class;
        // { +00D4 }
        public Aow.Units.AoWMemo.TAOWMemo S6Memo;
        // { +00D8 }
        public Aow.Units.AoWLabel.TAOWLabel S6Cost;
        // { +00DC }
        public Aow.Units.AoWImage.TAOWImage S6Icn;
        // { +00E0 }
        public Aow.Units.AoWPanel.TAOWPanel S7Pnl;
        // { +00E4 }
        public Aow.Units.AoWLabel.TAOWLabel S7Spell;
        // { +00E8 }
        public Aow.Units.AoWLabel.TAOWLabel S7Class;
        // { +00EC }
        public Aow.Units.AoWMemo.TAOWMemo S7Memo;
        // { +00F0 }
        public Aow.Units.AoWLabel.TAOWLabel S7Cost;
        // { +00F4 }
        public Aow.Units.AoWImage.TAOWImage S7Icn;
        // { +00F8 }
        public Aow.Units.AoWPanel.TAOWPanel S8Pnl;
        // { +00FC }
        public Aow.Units.AoWLabel.TAOWLabel S8Spell;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel S8Class;
        // { +0104 }
        public Aow.Units.AoWMemo.TAOWMemo S8Memo;
        // { +0108 }
        public Aow.Units.AoWLabel.TAOWLabel S8Cost;
        // { +010C }
        public Aow.Units.AoWImage.TAOWImage S8Icn;
        // { +0110 }
        public Aow.Units.AoWButton.TAOWButton S1Btn;
        // { +0114 }
        public Aow.Units.AoWLabel.TAOWLabel CurrentPointsLbl;
        // { +0118 }
        public Aow.Units.AoWLabel.TAOWLabel Line;
        // { +011C }
        public Aow.Units.AoWLabel.TAOWLabel CastingPointsMaxLbl;
        // { +0120 }
        public Aow.Units.AoWButton.TAOWButton GlobalBtn;
        // { +0124 }
        public Aow.Units.AoWButton.TAOWButton UnitBtn;
        // { +0128 }
        public Aow.Units.AoWButton.TAOWButton CombatBtn;
        // { +012C }
        public Aow.Units.AoWButton.TAOWButton InfoBtn;
        // { +0130 }
        public Aow.Units.AoWButton.TAOWButton PrevBtn;
        // { +0134 }
        public Aow.Units.AoWButton.TAOWButton NextBtn;
        // { +0138 }
        public Aow.Units.AoWButton.TAOWButton Cancel;
        // { +013C }
        public Aow.Units.AoWButton.TAOWButton S2Btn;
        // { +0140 }
        public Aow.Units.AoWButton.TAOWButton S3Btn;
        // { +0144 }
        public Aow.Units.AoWButton.TAOWButton S4Btn;
        // { +0148 }
        public Aow.Units.AoWButton.TAOWButton S5btn;
        // { +014C }
        public Aow.Units.AoWButton.TAOWButton S6Btn;
        // { +0150 }
        public Aow.Units.AoWButton.TAOWButton S7Btn;
        // { +0154 }
        public Aow.Units.AoWButton.TAOWButton S8Btn;
        // { +0158 }
        public Aow.Units.AoWPanel.TAOWPanel CastingPage;
        // { +015C }
        public Aow.Units.AoWPanel.TAOWPanel CastingPnl;
        // { +0160 }
        public Aow.Units.AoWLabel.TAOWLabel CastingSpell;
        // { +0164 }
        public Aow.Units.AoWLabel.TAOWLabel CastingClass;
        // { +0168 }
        public Aow.Units.AoWMemo.TAOWMemo CastingMemo;
        // { +016C }
        public Aow.Units.AoWLabel.TAOWLabel CastingCost;
        // { +0170 }
        public Aow.Units.AoWImage.TAOWImage CastingIcn;
        // { +0174 }
        public Aow.Units.AoWPanel.TAOWPanel LeftInfoPage;
        // { +0178 }
        public Aow.Units.AoWLabel.TAOWLabel CastingStatus;
        // { +017C }
        public Aow.Units.AoWLabel.TAOWLabel CastingPointsLbl;
        // { +0180 }
        public Aow.Units.AoWButton.TAOWButton CastingBtn;
        // { +0184 }
        public Aow.Units.AoWButton.TAOWButton Sel4;
        // { +0188 }
        public Aow.Units.AoWPanel.TAOWPanel SelPnl;
        // { +018C }
        public Aow.Units.AoWButton.TAOWButton Sel3;
        // { +0190 }
        public Aow.Units.AoWButton.TAOWButton Sel2;
        // { +0194 }
        public Aow.Units.AoWButton.TAOWButton Sel1;
        // { +0198 }
        public Aow.Units.AoWButton.TAOWButton Sel5;
        // { +019C }
        public Aow.Units.AoWButton.TAOWButton Sel6;
        // { +01A0 }
        public Aow.Units.AoWButton.TAOWButton Sel7;
        // { +01A4 }
        public Aow.Units.AoWLabel.TAOWLabel SelLbl;
        // { +01A8 }
        public Aow.Units.AoWLabel.TAOWLabel SpellBookLbl;
        // { +01AC }
        public Aow.Units.AoWLabel.TAOWLabel NameLbl;
        // { +01B0 }
        public Aow.Units.AoWLabel.TAOWLabel ManaPoolLbl;
        // { +01B4 }
        public Aow.Units.AoWLabel.TAOWLabel CastingLbl;
        // { +01B8 }
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        // { +01BC }
        public Aow.Units.AoWImage.TAOWImage Face;
        // { +01C0 }
        public Aow.Units.AoWBookWindow.TAoWBookWindow Dlg;
        // { +01C4 }
        public Aow.Units.AoWLabel.TAOWLabel NoneCastingLbl;
        // { +01C8 }
        public Aow.Units.AoWPanel.TAOWPanel ResearchPage;
        // { +01CC }
        public Aow.Units.AoWLabel.TAOWLabel ResearchName;
        // { +01D0 }
        public Aow.Units.AoWLabel.TAOWLabel ResearchClass;
        // { +01D4 }
        public Aow.Units.AoWMemo.TAOWMemo ResearchMemo;
        // { +01D8 }
        public Aow.Units.AoWLabel.TAOWLabel ResearchCost;
        // { +01DC }
        public Aow.Units.AoWImage.TAOWImage ResearchIcn;
        // { +01E0 }
        public Aow.Units.AoWLabel.TAOWLabel ResearchStatus;
        // { +01E4 }
        public Aow.Units.AoWLabel.TAOWLabel ResearchNone;
        // { +01E8 }
        public Aow.Units.AoWPanel.TAOWPanel ResearchPnl;
        // { +01EC }
        public Aow.Units.AoWLabel.TAOWLabel ResearchLbl;
        // { +01F0 }
        public Aow.Units.AoWPanel.TAOWPanel RightInfoPage;
        // { +01F4 }
        public Aow.Units.AoWLabel.TAOWLabel ManaInfo;
        // { +01F8 }
        public Aow.Units.AoWImage.TAOWImage Sep;
        // { +01FC }
        public Aow.Units.AoWLabel.TAOWLabel ResearchPointsLbl;
        // { +0200 }
        public Aow.Units.AoWLabel.TAOWLabel ResearchManaPoolLbl;
        // { +0204 }
        public Aow.Units.AoWLabel.TAOWLabel RManaInfo;
        // { +0208 }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +020C }
        public Aow.Units.AoWLabel.TAOWLabel ResearchBookLbl;
        // { +0210 }
        public Aow.Units.AoWButton.TAOWButton CancelResearchBtn;
        // { +0214 }
        public Aow.Units.AoWButton.TAOWButton CancelCastingBtn;
        // { +0218 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +021C }
        public byte FMode;
        // { $220: world casting, combat casting, research, researched }
        public byte FPageKind;
        // { $221: information or spell list }
        public Aow.Units.AoWE.TSpellType FSpellType;
        // { $222 }
        public byte FReserved223;
        public Aow.Units.AoWE.TSpellList FSpells;
        // { $224 }
        public int FPage;
        // { $228 }
        public Aow.Units.AoWE.THero FHero;
        // { $22C }
        public Aow.Units.AoWE.TActivateAbilityInfo FActivation;
        // { $230 }
        public int FResearchedSpell;
        // { $234 }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $238 }
        public Aow.Units.AoWE.TPlayerMagicEventLog FEvent;
        // { $240 }
        public Aow.Units.Engine.TDoubleItemList FPlayerPages;
        // AoWSpellBook.pas:393
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.AoWE.TPlayerMagicEventLog Event = default;
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\INT\\BOOKCLSE.WAV");
            Done = FDone;
            Event = FEvent;
            FDone = null;
            FEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FDrawOverlay = true;
            Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.FDrawn = false;
        }

        // AoWSpellBook.pas:405
        public bool ValidateHero()
        {
            bool Result = default;
            if (FActivation != null)
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FActivation.FUnitID);
                FHero = (Aow.Units.AoWE.THero)call__1;
                Result = (FHero != null) && !FHero.FUnitStatus.Contains(Aow.Units.AoWE.TUnitStatusElement.usKilled);
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // AoWSpellBook.pas:427
        public void Reset()
        {
            if (FActivation != null)
            {
                FActivation.Release();
                FActivation = null;
            }

            FSpells.Clear();
            FPage = 0;
            FHero = null;
            ResearchPage.Visible = true;
            CastingPage.Visible = true;
            GlobalBtn.Visible = true;
            GlobalBtn.ILIndexDown = 17;
            GlobalBtn.ILIndexLit = 18;
            GlobalBtn.ILIndexUp = 16;
            CombatBtn.Visible = true;
            UnitBtn.Visible = true;
            InfoBtn.Visible = true;
            CurrentPointsLbl.Visible = true;
            CastingPointsMaxLbl.Visible = true;
            Line.Visible = true;
        }

        // AoWSpellBook.pas:439
        public void SetPageKind(byte Kind)
        {
            FPageKind = unchecked((byte)Kind);
            LeftSpellPage.Visible = Kind == 1;
            RightSpellPage.Visible = Kind == 1;
            LeftInfoPage.Visible = Kind == 0;
            RightInfoPage.Visible = Kind == 0;
            switch (Kind)
            {
                case 1:
                    var call__2 = FSpells.GetCount();
                    NextBtn.Visible = (call__2 - ((FPage + 1) * 8)) > 0;
                    PrevBtn.Visible = FPage != 0;
                    break;
                case 0:
                    NextBtn.Visible = false;
                    PrevBtn.Visible = false;
                    break;
            }
        }

        // AoWSpellBook.pas:582
        public void UpdateSpellPages()
        {
            int I = default;
            Aow.Units.AoWE.TSpell Spell = default;
            if (ValidateHero())
            {
                switch (FMode)
                {
                    case 2:
                    case 3:
                        FSpells.Clear();
                        var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        call__3.FMagicControl.ListResearchSpells(ref FSpells, TSet<Aow.Units.AoWE.TSpellType>.Of(Aow.Units.AoWE.TSpellType.stCombat, Aow.Units.AoWE.TSpellType.stUnit, Aow.Units.AoWE.TSpellType.stGlobal));
                        FSpells.SortOnLevel();
                        var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TSpellTypeRStr[(int)FSpellType]);
                        var call__5 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchXspellRStr);
                        var call__6 = Aow.Units.AoWE.AoWE.Format(call__5, [call__4]);
                        LeftHeader.Text = call__6;
                        var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TSpellTypeRStr[(int)FSpellType]);
                        var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchXspellRStr);
                        var call__9 = Aow.Units.AoWE.AoWE.Format(call__8, [call__7]);
                        RightHeader.Text = call__9;
                        // { Original overwrites the type-specific headers. }
                        var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchSpellRStr);
                        LeftHeader.Text = call__10;
                        var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchSpellRStr);
                        RightHeader.Text = call__11;
                        break;
                    case 0:
                        FSpells.Clear();
                        var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        call__12.FMagicControl.ListSpells(ref FSpells, TSet<Aow.Units.AoWE.TSpellType>.Of(FSpellType));
                        FSpells.SortOnLevel();
                        var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TSpellTypeRStr[(int)FSpellType]);
                        var call__14 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastXSpellRStr);
                        var call__15 = Aow.Units.AoWE.AoWE.Format(call__14, [call__13]);
                        LeftHeader.Text = call__15;
                        var call__16 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TSpellTypeRStr[(int)FSpellType]);
                        var call__17 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastXSpellRStr);
                        var call__18 = Aow.Units.AoWE.AoWE.Format(call__17, [call__16]);
                        RightHeader.Text = call__18;
                        break;
                    case 1:
                        FSpells.Clear();
                        var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                        call__19.FMagicControl.ListSpells(ref FSpells, TSet<Aow.Units.AoWE.TSpellType>.Of(FSpellType));
                        FSpells.SortOnLevel();
                        var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TSpellTypeRStr[(int)FSpellType]);
                        var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastXSpellRStr);
                        var call__22 = Aow.Units.AoWE.AoWE.Format(call__21, [call__20]);
                        LeftHeader.Text = call__22;
                        var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TSpellTypeRStr[(int)FSpellType]);
                        var call__24 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastXSpellRStr);
                        var call__25 = Aow.Units.AoWE.AoWE.Format(call__24, [call__23]);
                        RightHeader.Text = call__25;
                        break;
                }

                {
                    I = FSpells.GetCount() - 1;
                    byte end__26 = 0;
                    for (; I >= end__26; I -= 1)
                    {
                        var call__27 = FSpells.GetSpell(I);
                        bool bool__28 = (call__27.FSpellID >= 100);
                        if (bool__28)
                        {
                            var call__29 = FSpells.GetSpell(I);
                            bool__28 = (Aow.Units.AoWTC.AoWTC.SpellTypes[call__29.FSpellID] == Aow.Units.AoWTC.TfxType.fxNone);
                        }

                        if (bool__28)
                        {
                            FSpells.Delete(I);
                        }
                    }
                }

                var call__30 = FSpells.GetSpell(FPage * 8);
                Spell = call__30;
                ShowSpellEntry(Spell, S1Pnl, S1Btn, S1Spell, S1Class, S1Cost, S1Memo, S1Icn);
                var call__31 = FSpells.GetSpell((FPage * 8) + 1);
                Spell = call__31;
                ShowSpellEntry(Spell, S2Pnl, S2Btn, S2Spell, S2Class, S2Cost, S2Memo, S2Icn);
                var call__32 = FSpells.GetSpell((FPage * 8) + 2);
                Spell = call__32;
                ShowSpellEntry(Spell, S3Pnl, S3Btn, S3Spell, S3Class, S3Cost, S3Memo, S3Icn);
                var call__33 = FSpells.GetSpell((FPage * 8) + 3);
                Spell = call__33;
                ShowSpellEntry(Spell, S4Pnl, S4Btn, S4Spell, S4Class, S4Cost, S4Memo, S4Icn);
                var call__34 = FSpells.GetSpell((FPage * 8) + 4);
                Spell = call__34;
                ShowSpellEntry(Spell, S5Pnl, S5btn, S5Spell, S5Class, S5Cost, S5Memo, S5Icn);
                var call__35 = FSpells.GetSpell((FPage * 8) + 5);
                Spell = call__35;
                ShowSpellEntry(Spell, S6Pnl, S6Btn, S6Spell, S6Class, S6Cost, S6Memo, S6Icn);
                var call__36 = FSpells.GetSpell((FPage * 8) + 6);
                Spell = call__36;
                ShowSpellEntry(Spell, S7Pnl, S7Btn, S7Spell, S7Class, S7Cost, S7Memo, S7Icn);
                var call__37 = FSpells.GetSpell((FPage * 8) + 7);
                Spell = call__37;
                ShowSpellEntry(Spell, S8Pnl, S8Btn, S8Spell, S8Class, S8Cost, S8Memo, S8Icn);
                if (FMode == 0)
                {
                    FPlayerPages.Count = Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer + 1;
                    FPlayerPages.List[Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer].A = (int)FSpellType;
                    FPlayerPages.List[Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer].B = FPage;
                }

                if (FPageKind == 1)
                {
                    var call__38 = FSpells.GetCount();
                    NextBtn.Visible = (call__38 - ((FPage + 1) * 8)) > 0;
                    PrevBtn.Visible = FPage != 0;
                }
            }
            else
            {
                Dlg.Visible = false;
            }
        }

        // AoWSpellBook.pas:691
        public void UpdateInfoPages()
        {
            sbyte Player = default;
            Aow.Units.AoWE.TLeader Leader = default;
            int I = default;
            Aow.Units.AoWButton.TAOWButton Button = default;
            Aow.Units.AoWE.TPlayerMagicControl Control = default;
            Aow.Units.AoWE.TSpell Spell = default;
            if (ValidateHero())
            {
                if (FMode == 1)
                {
                    Player = unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                }
                else
                {
                    Player = unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                }

                var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                Leader = call__39.FLeader;
                {
                    I = 0;
                    byte end__40 = 6;
                    for (; I <= end__40; I += 1)
                    {
                        switch (I)
                        {
                            case 0:
                                Button = Sel1;
                                break;
                            case 1:
                                Button = Sel2;
                                break;
                            case 2:
                                Button = Sel3;
                                break;
                            case 3:
                                Button = Sel4;
                                break;
                            case 4:
                                Button = Sel5;
                                break;
                            case 5:
                                Button = Sel6;
                                break;
                            case 6:
                                Button = Sel7;
                                break;
                            default:
                                Button = null;
                                break;
                        }

                        if (I < Leader.GetSphereCount())
                        {
                            Button.Visible = true;
                            switch (unchecked((int)Leader.GetSpheres(I)) & 127)
                            {
                                case 1:
                                    Button.ILIndexDown = 28;
                                    Button.ILIndexLit = 29;
                                    Button.ILIndexUp = 27;
                                    break;
                                case 5:
                                    Button.ILIndexDown = 43;
                                    Button.ILIndexLit = 44;
                                    Button.ILIndexUp = 42;
                                    break;
                                case 3:
                                    Button.ILIndexDown = 34;
                                    Button.ILIndexLit = 35;
                                    Button.ILIndexUp = 33;
                                    break;
                                case 4:
                                    Button.ILIndexDown = 37;
                                    Button.ILIndexLit = 38;
                                    Button.ILIndexUp = 36;
                                    break;
                                case 6:
                                    Button.ILIndexDown = 40;
                                    Button.ILIndexLit = 41;
                                    Button.ILIndexUp = 39;
                                    break;
                                case 2:
                                    Button.ILIndexDown = 31;
                                    Button.ILIndexLit = 32;
                                    Button.ILIndexUp = 30;
                                    break;
                            }
                        }
                        else
                        {
                            Button.Visible = false;
                        }
                    }
                }

                NameLbl.Text = FHero.FHeroName;
                var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                var call__42 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaPoolRStr);
                ManaPoolLbl.Text = (call__42 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(call__41.FMagicControl.FMana);
                var call__43 = FHero.GetCastingPoints();
                var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastingPointsRStr);
                var call__45 = FHero.GetCastingPointsMax();
                CastingPointsLbl.Text = (((call__44 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(call__43)) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__45);
                ResearchManaPoolLbl.Text = ManaPoolLbl.Text;
                var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                var call__47 = call__46.FMagicControl.GetNetPowerToResearch();
                var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchPointsRStr);
                ResearchPointsLbl.Text = (call__48 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(call__47);
                if (TSet<byte>.WithBounds(0, 255).With(2).With(3).Contains(FMode))
                {
                    CurrentPointsLbl.Text = (AnsiString)"";
                    CastingPointsMaxLbl.Text = (AnsiString)"";
                    SpellBookLbl.Visible = false;
                    ResearchBookLbl.Visible = true;
                    CastingPage.Visible = false;
                    var call__49 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    Control = call__49.FMagicControl;
                    if (Control.FResearchSpellID != 0)
                    {
                        ResearchPnl.Visible = true;
                        ResearchNone.Visible = false;
                        var call__50 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Control.FResearchSpellID);
                        Spell = call__50;
                        var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CurrentlyResearchingRStr);
                        ResearchLbl.Text = call__51 + AnsiString.FromBytes(58);
                        ResearchMemo.Items = Spell.FSpellText;
                        ResearchName.Text = Spell.FSpellName;
                        var call__52 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                        var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelRStr);
                        ResearchClass.Text = (((call__52 + ((AnsiString)", ")) + call__53) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell.FSpellLevel);
                        var call__54 = FHero.CastingMana(Spell);
                        var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastingCostRStr);
                        var call__56 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaRStr);
                        ResearchCost.Text = (((call__55 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(call__54)) + AnsiString.FromBytes(32)) + call__56;
                        if (Control.GetResearchTurnsLeft() < 999)
                        {
                            var call__57 = Control.GetResearchTurnsLeft();
                            var call__58 = Aow.Units.AoWE.AoWE.FmtTurnsLeftStr(call__57);
                            ResearchStatus.Text = call__58;
                        }
                        else
                        {
                            var call__59 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XTurnsRStr);
                            var call__60 = Aow.Units.AoWE.AoWE.Format(call__59, [(byte)63]);
                            ResearchStatus.Text = call__60;
                        }

                        var call__61 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
                        CancelResearchBtn.Text = call__61;
                        ResearchStatus.Visible = true;
                    }
                    else
                    {
                        if (FMode == 3)
                        {
                            ResearchPnl.Visible = true;
                            ResearchNone.Visible = false;
                            var call__62 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FResearchedSpell);
                            Spell = call__62;
                            var call__63 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchedRStr);
                            ResearchLbl.Text = call__63 + AnsiString.FromBytes(58);
                            ResearchMemo.Items = Spell.FSpellText;
                            ResearchName.Text = Spell.FSpellName;
                            var call__64 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                            var call__65 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelRStr);
                            ResearchClass.Text = (((call__64 + ((AnsiString)", ")) + call__65) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell.FSpellLevel);
                            var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastingCostRStr);
                            var call__67 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaRStr);
                            ResearchCost.Text = (((call__66 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(Spell.FBaseManaCost)) + AnsiString.FromBytes(32)) + call__67;
                            ResearchStatus.Visible = false;
                            var call__68 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchRStr);
                            CancelResearchBtn.Text = call__68;
                            var call__69 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchNewSpellRStr);
                            CancelResearchBtn.ToolTipText.Text = call__69;
                        }
                        else
                        {
                            ResearchPnl.Visible = false;
                            ResearchNone.Visible = true;
                            var call__70 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResearchRStr);
                            CancelResearchBtn.Text = call__70;
                            var call__71 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelSpellResearchRStr);
                            CancelResearchBtn.ToolTipText.Text = call__71;
                        }
                    }
                }
                else
                {
                    if (TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode))
                    {
                        var call__72 = FHero.GetCastingPoints();
                        CurrentPointsLbl.Text = Aow.Rtl.Rtl.IntToStr(call__72);
                        var call__73 = FHero.GetCastingPointsMax();
                        CastingPointsMaxLbl.Text = Aow.Rtl.Rtl.IntToStr(call__73);
                        SpellBookLbl.Visible = true;
                        ResearchBookLbl.Visible = false;
                        ResearchPage.Visible = false;
                        var call__74 = FHero.Casting();
                        if (call__74)
                        {
                            NoneCastingLbl.Visible = false;
                            var call__75 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID);
                            Spell = call__75;
                            CastingSpell.Text = Spell.FSpellName;
                            var call__76 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                            var call__77 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelRStr);
                            CastingClass.Text = (((call__76 + ((AnsiString)", ")) + call__77) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell.FSpellLevel);
                            var call__78 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastingCostRStr);
                            var call__79 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaRStr);
                            CastingCost.Text = (((call__78 + ((AnsiString)": ")) + Aow.Rtl.Rtl.IntToStr(FHero.FCastingProgress)) + AnsiString.FromBytes(32)) + call__79;
                            CastingMemo.Items = Spell.FSpellText;
                            var call__80 = FHero.CastingTurnsLeft();
                            var call__81 = Aow.Units.AoWE.AoWE.FmtTurnsLeftStr(call__80);
                            CastingStatus.Text = (AnsiString.FromBytes(40) + call__81) + AnsiString.FromBytes(41);
                            CastingPnl.Visible = true;
                            var call__82 = FHero.CastingReady();
                            if (call__82)
                            {
                                CastingBtn.Visible = true;
                                var call__83 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CastingReadyRStr);
                                CastingStatus.Text = call__83;
                            }
                            else
                            {
                                CastingBtn.Visible = false;
                                var call__84 = FHero.CastingTurnsLeft();
                                var call__85 = Aow.Units.AoWE.AoWE.FmtTurnsLeftStr(call__84);
                                CastingStatus.Text = (AnsiString.FromBytes(40) + call__85) + AnsiString.FromBytes(41);
                            }
                        }
                        else
                        {
                            CastingPnl.Visible = false;
                            NoneCastingLbl.Visible = true;
                        }
                    }
                    else
                    {
                        CastingPnl.Visible = false;
                        NoneCastingLbl.Visible = true;
                    }
                }
            }
        }

        // AoWSpellBook.pas:638
        public void ShowSpellEntry(Aow.Units.AoWE.TSpell Spell, Aow.Units.AoWPanel.TAOWPanel Panel, Aow.Units.AoWButton.TAOWButton Button, Aow.Units.AoWLabel.TAOWLabel Name, Aow.Units.AoWLabel.TAOWLabel SpellClass, Aow.Units.AoWLabel.TAOWLabel Cost, Aow.Units.AoWMemo.TAOWMemo Memo, Aow.Units.AoWImage.TAOWImage Icon)
        {
            Aow.Rtl.AnsiString Reason = default;
            int Turns = default;
            Aow.Units.AoWE.TPlayerMagicControl Control = default;
            if (Spell != null)
            {
                Panel.Visible = true;
                Name.Text = Spell.FSpellName;
                var call__86 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelRStr);
                SpellClass.Text = (((call__86 + ((AnsiString)", ")) + call__87) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell.FSpellLevel);
                if (FMode == 2)
                {
                    if (Spell.FSpellControlTypes == TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal))
                    {
                        var call__88 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GlobalRStr);
                        SpellClass.Text = (SpellClass.Text + ((AnsiString)", ")) + call__88;
                    }
                    else
                    {
                        if (Spell.FSpellControlTypes == TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctCombat))
                        {
                            var call__89 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CombatRStr);
                            SpellClass.Text = (SpellClass.Text + ((AnsiString)", ")) + call__89;
                        }
                        else
                        {
                            if (Spell.FSpellControlTypes == TSet<Aow.Units.AoWE.TSpellControlTypeElement>.Of(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal, Aow.Units.AoWE.TSpellControlTypeElement.sctCombat))
                            {
                                var call__90 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GlobalRStr);
                                var call__91 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CombatRStr);
                                SpellClass.Text = (((SpellClass.Text + ((AnsiString)", ")) + call__90) + AnsiString.FromBytes(47)) + call__91;
                            }
                        }
                    }
                }

                Memo.Items = Spell.FSpellText;
                if (FHero == null)
                {
                    var call__92 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CostXRStr);
                    var call__93 = Aow.Units.AoWE.AoWE.Format(call__92, [(int)Spell.FBaseManaCost]);
                    Cost.Text = call__93;
                }
                else
                {
                    var call__94 = FHero.CastingMana(Spell);
                    var call__95 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CostXRStr);
                    var call__96 = Aow.Units.AoWE.AoWE.Format(call__95, [(int)call__94]);
                    Cost.Text = call__96;
                }

                bool bool__97 = (FMode == 0);
                if (bool__97)
                {
                    bool bool__98 = !Spell.FSpellControlTypes.Contains(Aow.Units.AoWE.TSpellControlTypeElement.sctGlobal);
                    if (!bool__98)
                    {
                        var call__99 = FHero.Casting();
                        bool__98 = call__99;
                    }

                    bool__97 = bool__98;
                }

                bool bool__100 = bool__97;
                if (!bool__100)
                {
                    bool bool__101 = (FMode == 1);
                    if (bool__101)
                    {
                        bool bool__102 = !Spell.FSpellControlTypes.Contains(Aow.Units.AoWE.TSpellControlTypeElement.sctCombat);
                        if (!bool__102)
                        {
                            var call__103 = FHero.Casting();
                            bool__102 = call__103;
                        }

                        bool bool__104 = bool__102;
                        if (!bool__104)
                        {
                            var call__105 = FHero.CanCastSpellInstantly(Spell.FSpellID, Spell.FBaseManaCost, ref Reason);
                            bool__104 = !call__105;
                        }

                        bool__101 = bool__104;
                    }

                    bool__100 = bool__101;
                }

                bool bool__106 = bool__100;
                if (!bool__106)
                {
                    bool bool__107 = (FMode == 2);
                    if (bool__107)
                    {
                        var call__108 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        bool__107 = (call__108.FMagicControl.FResearchSpellID != 0);
                    }

                    bool__106 = bool__107;
                }

                if (bool__106)
                {
                    Memo.TextColor = 5000268;
                    Name.TextColor = 5000268;
                    SpellClass.TextColor = 5000268;
                    Cost.TextColor = 5000268;
                    Button.Visible = false;
                    Icon.Tag = Icon.Tag | 16;
                }
                else
                {
                    Memo.TextColor = 1254460;
                    Name.TextColor = 1254460;
                    SpellClass.TextColor = 5908276;
                    Cost.TextColor = 5908276;
                    Button.Visible = true;
                    Icon.Tag = Icon.Tag & 15;
                }

                if (Spell.FManaUpkeep > 0)
                {
                    var call__109 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpkeepXRStr);
                    var call__110 = Aow.Units.AoWE.AoWE.Format(call__109, [(int)Spell.FManaUpkeep]);
                    Cost.Text = (Cost.Text + AnsiString.FromBytes(32)) + call__110;
                }

                if (TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode))
                {
                    var call__111 = FHero.CastingTurns(Spell);
                    Turns = call__111;
                    if (Turns == 0)
                    {
                        Reason = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InstantRStr);
                    }
                    else
                    {
                        var call__112 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TurnsXRStr);
                        var call__113 = Aow.Units.AoWE.AoWE.Format(call__112, [Aow.Rtl.Rtl.IntToStr(Turns)]);
                        Reason = call__113;
                    }

                    Cost.Text = (Cost.Text + AnsiString.FromBytes(32)) + Reason;
                }
                else
                {
                    if ((FMode == 2) || (FMode == 3))
                    {
                        var call__114 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        Control = call__114.FMagicControl;
                        var call__115 = Control.GetNetPowerToResearch();
                        var call__116 = Control.ResearchTurns(call__115, Spell.FResearchCost);
                        Turns = call__116;
                        if (Turns < 999)
                        {
                            var call__117 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TurnsXRStr);
                            var call__118 = Aow.Units.AoWE.AoWE.Format(call__117, [Aow.Rtl.Rtl.IntToStr(Turns)]);
                            Cost.Text = (Cost.Text + AnsiString.FromBytes(32)) + call__118;
                        }
                        else
                        {
                            var call__119 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XTurnsRStr);
                            var call__120 = Aow.Units.AoWE.AoWE.Format(call__119, [(byte)63]);
                            Cost.Text = (Cost.Text + AnsiString.FromBytes(32)) + call__120;
                        }
                    }
                }
            }
            else
            {
                Panel.Visible = false;
            }
        }

        // AoWSpellBook.pas:455
        public void SelectSpell(Aow.Units.AoWE.TSpell Spell)
        {
            Aow.Rtl.AnsiString Reason = default;
            if (ValidateHero())
            {
                switch (FMode)
                {
                    case 2:
                    case 3:
                        var call__121 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        var call__122 = call__121.FMagicControl.ResearchSpell(Spell.FSpellID);
                        if (call__122)
                        {
                            if ((FEvent == null) || (FEvent.FMode != Aow.Units.AoWE.TPlayerMagicEventMode.pmemStartResearch))
                            {
                                FMode = 2;
                                InfoBtnClick(this);
                            }
                            else
                            {
                                Dlg.Visible = false;
                            }
                        }

                        break;
                    case 0:
                        var call__123 = FHero.GetCastingPoints();
                        bool bool__124 = (call__123 >= Spell.FBaseManaCost);
                        if (!bool__124)
                        {
                            var call__125 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FHero.FPlayer);
                            var call__126 = call__125.FMagicControl.GetCheatEnabled();
                            bool__124 = call__126;
                        }

                        if (bool__124)
                        {
                            var call__127 = FHero.CastSpell(Spell.FSpellID, ref Reason);
                            if (call__127)
                            {
                                Dlg.Visible = false;
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }
                        }
                        else
                        {
                            var call__128 = FHero.CastSpell(Spell.FSpellID, ref Reason);
                            if (call__128)
                            {
                                InfoBtnClick(this);
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }
                        }

                        break;
                    case 1:
                        var call__129 = FHero.CastCombatSpell(Spell.FSpellID, ref Reason);
                        if (call__129)
                        {
                            Dlg.Visible = false;
                        }
                        else
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.ShowWarning(Reason);
                        }

                        break;
                }
            }
            else
            {
                Dlg.Visible = false;
            }
        }

        // AoWSpellBook.pas:415
        public void PlayerChanged(sbyte Player)
        {
            if (Dlg.Visible && (Player == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer))
            {
                UpdateSpellPages();
                UpdateInfoPages();
            }
        }

        // AoWSpellBook.pas:421
        public void UnitChanged(Aow.Units.AoWE.TAbstractUnit U)
        {
            if (Dlg.Visible && ((U == null) || (TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode) && (U == FHero))))
            {
                UpdateSpellPages();
                UpdateInfoPages();
            }
        }

        // AoWSpellBook.pas:488
        public void ShowResearch()
        {
            Reset();
            CastingPointsMaxLbl.Visible = false;
            CurrentPointsLbl.Visible = false;
            Line.Visible = false;
            GlobalBtn.ILIndexDown = 14;
            GlobalBtn.ILIndexLit = 15;
            GlobalBtn.ILIndexUp = 13;
            CombatBtn.Visible = false;
            UnitBtn.Visible = false;
            var call__130 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (call__130.FMagicControl.FResearchSpellID != 0)
            {
                SetPageKind(0);
            }
            else
            {
                SetPageKind(1);
            }

            FMode = 2;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FPage = 0;
            var call__131 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            FHero = call__131.FLeader;
            UpdateSpellPages();
            UpdateInfoPages();
            ShowBook();
        }

        // AoWSpellBook.pas:501
        public void ShowResearchEvent(Aow.Units.AoWE.TPlayerMagicEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            FEvent = Event;
            FEvent.AddRef();
            FDone = Done;
            var call__132 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (call__132.FMagicControl.FResearchSpellID == 0)
            {
                Reset();
                GlobalBtn.ILIndexDown = 14;
                GlobalBtn.ILIndexLit = 15;
                GlobalBtn.ILIndexUp = 13;
                CombatBtn.Visible = false;
                UnitBtn.Visible = false;
                FResearchedSpell = 0;
                SetPageKind(1);
                FMode = 2;
                FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
                FPage = 0;
                var call__133 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                FHero = call__133.FLeader;
                UpdateSpellPages();
                UpdateInfoPages();
                ShowBook();
                Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(51, true, true);
            }
            else
            {
                ShowResearch();
            }
        }

        // AoWSpellBook.pas:518
        public void ShowResearchedEvent(Aow.Units.AoWE.TPlayerMagicEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            FEvent = Event;
            FEvent.AddRef();
            FDone = Done;
            var call__134 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (call__134.FMagicControl.FResearchSpellID == 0)
            {
                Reset();
                FResearchedSpell = Event.FSpellID;
                SetPageKind(0);
                FMode = 3;
                FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
                FPage = 0;
                var call__135 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                FHero = call__135.FLeader;
                UpdateSpellPages();
                UpdateInfoPages();
                ShowBook();
            }
            else
            {
                ShowResearch();
            }
        }

        // AoWSpellBook.pas:531
        public void ShowCasting(Aow.Units.AoWE.TActivateAbilityInfo Activation)
        {
            Reset();
            FActivation = Activation;
            FActivation.AddRef();
            FMode = 0;
            if (ValidateHero())
            {
                var call__136 = FPlayerPages[Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer];
                FSpellType = (Aow.Units.AoWE.TSpellType)call__136.A;
                var call__137 = FPlayerPages[Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer];
                FPage = call__137.B;
                var call__138 = FHero.Casting();
                if (call__138)
                {
                    SetPageKind(0);
                }
                else
                {
                    FSpells.Clear();
                    var call__139 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    call__139.FMagicControl.ListSpells(ref FSpells, TSet<Aow.Units.AoWE.TSpellType>.Of(FSpellType));
                    var call__140 = FSpells.GetCount();
                    bool bool__141 = (call__140 == 0);
                    if (!bool__141)
                    {
                        var call__142 = FSpells.GetCount();
                        bool__141 = (call__142 <= (FPage * 8));
                    }

                    if (bool__141)
                    {
                        FSpells.Clear();
                        FSpellType = (Aow.Units.AoWE.TSpellType)3;
                        while (true)
                        {
                            var call__143 = FSpells.GetCount();
                            if (!((call__143 == 0) && (((int)FSpellType) != 0)))
                                break;
                            {
                                FSpellType = (Aow.Units.AoWE.TSpellType)(((int)FSpellType) - 1);
                                var call__144 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                                call__144.FMagicControl.ListSpells(ref FSpells, TSet<Aow.Units.AoWE.TSpellType>.Of(FSpellType));
                            }
                        }
                    }

                    SetPageKind(1);
                }

                UpdateSpellPages();
                UpdateInfoPages();
                ShowBook();
            }
            else
            {
                Reset();
            }
        }

        // AoWSpellBook.pas:559
        public void ShowCombatCasting(Aow.Units.AoWE.TActivateAbilityInfo Activation)
        {
            Reset();
            FActivation = Activation;
            FActivation.AddRef();
            FMode = 1;
            if (ValidateHero())
            {
                GlobalBtn.Visible = true;
                var call__145 = FHero.Casting();
                if (call__145)
                {
                    SetPageKind(0);
                }
                else
                {
                    FSpells.Clear();
                    FSpellType = (Aow.Units.AoWE.TSpellType)3;
                    while (true)
                    {
                        var call__146 = FSpells.GetCount();
                        if (!((call__146 == 0) && (((int)FSpellType) != 0)))
                            break;
                        {
                            FSpellType = (Aow.Units.AoWE.TSpellType)(((int)FSpellType) - 1);
                            var call__147 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                            call__147.FMagicControl.ListSpells(ref FSpells, TSet<Aow.Units.AoWE.TSpellType>.Of(FSpellType));
                        }
                    }

                    SetPageKind(1);
                }

                FSpellType = Aow.Units.AoWE.TSpellType.stCombat;
                FPage = 0;
                UpdateSpellPages();
                UpdateInfoPages();
                ShowBook();
            }
            else
            {
                Reset();
            }
        }

        // AoWSpellBook.pas:197
        public void SpellBookCreate()
        {
            Aow.Units.Engine.TDoubleItem Defaults = default;
            var call__148 = new Aow.Units.AoWE.TSpellList();
            FSpells = call__148;
            Defaults.A = 2;
            Defaults.B = 0;
            var call__149 = new Aow.Units.Engine.TDoubleItemList();
            FPlayerPages = call__149;
            FPlayerPages.Default = Defaults;
        }

        // AoWSpellBook.pas:206
        public void SpellBookDestroy()
        {
            if (FActivation != null)
            {
                FActivation.Release();
                FActivation = null;
            }

            FPlayerPages.Free();
            FSpells.Free();
        }

        // AoWSpellBook.pas:214
        public void CancelClick()
        {
            if ((Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null))
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.SelectAbility(-1);
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWSpellBook.pas:222
        public void NextBtnClick()
        {
            FPage += 1;
            UpdateSpellPages();
            Dlg.StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode.ptLeft);
            Dlg.FPageTurn.FSpeed = 25;
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
        }

        // AoWSpellBook.pas:229
        public void PrevBtnClick()
        {
            FPage -= 1;
            UpdateSpellPages();
            Dlg.StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode.ptRight);
            Dlg.FPageTurn.FSpeed = 25;
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
        }

        // AoWSpellBook.pas:236
        public void SpellIconDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int Index = default;
            int X = default;
            int Y = default;
            Index = (FPage * 8) + (((Aow.Rtl.TComponent)Sender).Tag & 15);
            bool bool__150 = (Index >= 0);
            if (bool__150)
            {
                var call__151 = FSpells.GetCount();
                bool__150 = (Index < call__151);
            }

            if (bool__150)
            {
                var call__152 = FSpells.GetSpell(Index);
                var call__153 = call__152.FCastImages.GetImageSequence(10);
                if (call__153 != null)
                {
                    var image = (Aow.Units.AoWImage.TAOWImage)Sender;
                    X = image.FAOWWindow.WinLeft;
                    Y = image.FAOWWindow.WinTop;
                    var call__155 = FSpells.GetSpell(Index);
                    call__155.FCastImages.Show(10, image.WinLeft + X, image.WinTop + Y, 0, SD);
                }
            }
        }

        // AoWSpellBook.pas:249
        public void SelectSpellBtnClick(Aow.Rtl.TObject Sender)
        {
            var call__156 = FSpells.GetSpell((FPage * 8) + ((Aow.Rtl.TComponent)Sender).Tag);
            SelectSpell(call__156);
        }

        // AoWSpellBook.pas:254
        public void GlobalBtnClick(Aow.Rtl.TObject Sender)
        {
            if (((FSpellType != Aow.Units.AoWE.TSpellType.stGlobal) || (FPage != 0)) || (FPageKind != 1))
            {
                if (FPageKind == 0)
                {
                    Dlg.StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode.ptLeft);
                }
                else
                {
                    Dlg.StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode.ptRight);
                }

                Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
                FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
                FPage = 0;
                UpdateSpellPages();
                SetPageKind(1);
            }
        }

        // AoWSpellBook.pas:264
        public void UnitBtnClick()
        {
            if (((FSpellType != Aow.Units.AoWE.TSpellType.stUnit) || (FPage != 0)) || (FPageKind != 1))
            {
                if ((FPageKind == 0) || (FSpellType == Aow.Units.AoWE.TSpellType.stGlobal))
                {
                    Dlg.StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode.ptLeft);
                }
                else
                {
                    Dlg.StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode.ptRight);
                }

                Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
                FSpellType = Aow.Units.AoWE.TSpellType.stUnit;
                FPage = 0;
                UpdateSpellPages();
                SetPageKind(1);
            }
        }

        // AoWSpellBook.pas:275
        public void CombatBtnClick()
        {
            if (((FSpellType != Aow.Units.AoWE.TSpellType.stCombat) || (FPage != 0)) || (FPageKind != 1))
            {
                Dlg.StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode.ptLeft);
                Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
                FSpellType = Aow.Units.AoWE.TSpellType.stCombat;
                FPage = 0;
                UpdateSpellPages();
                SetPageKind(1);
            }
        }

        // AoWSpellBook.pas:285
        public void InfoBtnClick(Aow.Rtl.TObject Sender)
        {
            if (FPageKind != 0)
            {
                Dlg.StartAnim(Aow.Units.PageTurnCtrl.TPageTurnMode.ptRight);
                Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
                SetPageKind(0);
            }
        }

        // AoWSpellBook.pas:295
        public void CastingBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (ValidateHero())
            {
                switch (FMode)
                {
                    case 0:
                        var call__157 = FHero.CastSpell(FHero.FCastingSpellID, ref Reason);
                        if (call__157)
                        {
                            Dlg.Visible = false;
                        }
                        else
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                    case 1:
                        var call__158 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID);
                        SelectSpell(call__158);
                        break;
                }
            }
        }

        // AoWSpellBook.pas:309
        public void CancelCastingBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (ValidateHero())
            {
                FHero.CancelCasting();
            }
        }

        // AoWSpellBook.pas:315
        public void FaceDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            var image = (Aow.Units.AoWImage.TAOWImage)Sender;
            var call__160 = Aow.Rtl.Rtl.Rect(LeftInfoPage.WinLeft + image.WinLeft, LeftInfoPage.WinTop + image.WinTop, (LeftInfoPage.WinLeft + image.WinLeft) + image.WinWidth, (LeftInfoPage.WinTop + image.WinTop) + image.WinHeight);
            R = call__160;

            var call__161 = FHero.GetFace();
            call__161.ShowStretched(R, SD);
        }

        // AoWSpellBook.pas:325
        public void ResearchIcnDraw(Aow.Units.AoWImage.TAOWImage Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TSpell Spell = default;
            int X = default;
            var call__162 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__163 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(call__162.FMagicControl.FResearchSpellID);
            Spell = call__163;
            if ((Spell == null) && (FMode == 3))
            {
                var call__164 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FResearchedSpell);
                Spell = call__164;
            }

            if (Spell != null)
            {
                X = Sender.FAOWWindow.WinLeft;
                Spell.FCastImages.Show(10, Sender.WinLeft + X, Sender.FAOWWindow.WinTop + Sender.WinTop, 0, SD);
            }
        }

        // AoWSpellBook.pas:337
        public void CastingIcnDraw(Aow.Units.AoWImage.TAOWImage Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TSpell Spell = default;
            int X = default;
            if (FHero != null)
            {
                var call__166 = FHero.Casting();
                if (call__166)
                {
                    var call__167 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID);
                    Spell = call__167;
                    if (Spell != null)
                    {
                        X = Sender.FAOWWindow.WinLeft;
                        Spell.FCastImages.Show(10, Sender.WinLeft + X, Sender.FAOWWindow.WinTop + Sender.WinTop, 0, SD);
                    }
                }
            }
        }

        // AoWSpellBook.pas:352
        public void CancelResearchBtnClick()
        {
            Aow.Units.AoWE.TPlayerMagicControl Control = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (ValidateHero())
            {
                var call__169 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                Control = call__169.FMagicControl;
                if (Control.FResearchSpellID != 0)
                {
                    Control.ResearchSpell(0);
                }
                else
                {
                    GlobalBtnClick(this);
                }
            }
        }

        // AoWSpellBook.pas:363
        public void Sel1Click()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWSpellBook.pas:366
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWSpellBook.pas:369
        public void DlgUpdate()
        {
            if (Dlg.Visible)
            {
                if (!ValidateHero())
                {
                    Dlg.Visible = false;
                }
            }
        }

        // AoWSpellBook.pas:375
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                if ((Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null))
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.SelectAbility(-1);
                }

                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWSpellBook
    {
    }
}
