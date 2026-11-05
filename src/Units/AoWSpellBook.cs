// Translated from AoWSpellBook.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.Sound.Sound;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.EventLog;
using Aow.Units.AoWBookWindow;
using Aow.Units.AoWButton;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWSpellBook
{
    // mode, page kind, spell type, spells, page, hero, activation, researched spell, callback,
    // event, saved player pages
    public partial class TSpellBook : TDataModule
    {
        public TSpellBook(TComponent owner) : base(owner)
        { }

        public TAOWLabel LeftHeader;
        public TAOWPanel LeftSpellPage;
        public TAOWPanel S1Pnl;
        public TAOWLabel S1Spell;
        public TAOWLabel S1Class;
        public TAOWMemo S1Memo;
        public TAOWLabel S1Cost;
        public TAOWImage S1Icn;
        public TAOWPanel S2Pnl;
        public TAOWLabel S2Spell;
        public TAOWLabel S2Class;
        public TAOWMemo S2Memo;
        public TAOWLabel S2Cost;
        public TAOWImage S2Icn;
        public TAOWPanel S3Pnl;
        public TAOWLabel S3Spell;
        public TAOWLabel S3Class;
        public TAOWMemo S3Memo;
        public TAOWLabel S3Cost;
        public TAOWImage S3Icn;
        public TAOWPanel S4Pnl;
        public TAOWLabel S4Spell;
        public TAOWLabel S4Class;
        public TAOWMemo S4Memo;
        public TAOWLabel S4Cost;
        public TAOWImage S4Icn;
        public TAOWLabel RightHeader;
        public TAOWPanel RightSpellPage;
        public TAOWPanel S5Pnl;
        public TAOWLabel S5Spell;
        public TAOWLabel S5Class;
        public TAOWMemo S5Memo;
        public TAOWLabel S5Cost;
        public TAOWImage S5Icn;
        public TAOWPanel S6Pnl;
        public TAOWLabel S6Spell;
        public TAOWLabel S6Class;
        public TAOWMemo S6Memo;
        public TAOWLabel S6Cost;
        public TAOWImage S6Icn;
        public TAOWPanel S7Pnl;
        public TAOWLabel S7Spell;
        public TAOWLabel S7Class;
        public TAOWMemo S7Memo;
        public TAOWLabel S7Cost;
        public TAOWImage S7Icn;
        public TAOWPanel S8Pnl;
        public TAOWLabel S8Spell;
        public TAOWLabel S8Class;
        public TAOWMemo S8Memo;
        public TAOWLabel S8Cost;
        public TAOWImage S8Icn;
        public TAOWButton S1Btn;
        public TAOWLabel CurrentPointsLbl;
        public TAOWLabel Line;
        public TAOWLabel CastingPointsMaxLbl;
        public TAOWButton GlobalBtn;
        public TAOWButton UnitBtn;
        public TAOWButton CombatBtn;
        public TAOWButton InfoBtn;
        public TAOWButton PrevBtn;
        public TAOWButton NextBtn;
        public TAOWButton Cancel;
        public TAOWButton S2Btn;
        public TAOWButton S3Btn;
        public TAOWButton S4Btn;
        public TAOWButton S5btn;
        public TAOWButton S6Btn;
        public TAOWButton S7Btn;
        public TAOWButton S8Btn;
        public TAOWPanel CastingPage;
        public TAOWPanel CastingPnl;
        public TAOWLabel CastingSpell;
        public TAOWLabel CastingClass;
        public TAOWMemo CastingMemo;
        public TAOWLabel CastingCost;
        public TAOWImage CastingIcn;
        public TAOWPanel LeftInfoPage;
        public TAOWLabel CastingStatus;
        public TAOWLabel CastingPointsLbl;
        public TAOWButton CastingBtn;
        public TAOWButton Sel4;
        public TAOWPanel SelPnl;
        public TAOWButton Sel3;
        public TAOWButton Sel2;
        public TAOWButton Sel1;
        public TAOWButton Sel5;
        public TAOWButton Sel6;
        public TAOWButton Sel7;
        public TAOWLabel SelLbl;
        public TAOWLabel SpellBookLbl;
        public TAOWLabel NameLbl;
        public TAOWLabel ManaPoolLbl;
        public TAOWLabel CastingLbl;
        public TAOWImage FaceFrame;
        public TAOWImage Face;
        public TAoWBookWindow Dlg;
        public TAOWLabel NoneCastingLbl;
        public TAOWPanel ResearchPage;
        public TAOWLabel ResearchName;
        public TAOWLabel ResearchClass;
        public TAOWMemo ResearchMemo;
        public TAOWLabel ResearchCost;
        public TAOWImage ResearchIcn;
        public TAOWLabel ResearchStatus;
        public TAOWLabel ResearchNone;
        public TAOWPanel ResearchPnl;
        public TAOWLabel ResearchLbl;
        public TAOWPanel RightInfoPage;
        public TAOWLabel ManaInfo;
        public TAOWImage Sep;
        public TAOWLabel ResearchPointsLbl;
        public TAOWLabel ResearchManaPoolLbl;
        public TAOWLabel RManaInfo;
        public TAOWImage AOWImage1;
        public TAOWLabel ResearchBookLbl;
        public TAOWButton CancelResearchBtn;
        public TAOWButton CancelCastingBtn;
        public TIvTranslator Translator;
        public byte FMode;
        // world casting, combat casting, research, researched
        public byte FPageKind;
        // information or spell list
        public TSpellType FSpellType;
        public TSpellList FSpells;
        public int FPage;
        public THero FHero;
        public TActivateAbilityInfo FActivation;
        public int FResearchedSpell;
        public TExecuteDoneEvent FDone;
        public TPlayerMagicEventLog FEvent;
        public TDoubleItemList FPlayerPages;
        // AoWSpellBook.pas:393
        public void FinishDialog()
        {
            TExecuteDoneEvent Done = default;
            TPlayerMagicEventLog Event = default;
            PlaySample((AnsiString)"SFX\\INT\\BOOKCLSE.WAV");
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

            General.Manager.OverlayPriority = 0;
            General.Manager.FDrawOverlay = true;
            AoWExeForms.AoWExeForms.MagicWin.Dlg.FDrawn = false;
        }

        // AoWSpellBook.pas:405
        public bool ValidateHero()
        {
            bool Result = default;
            if (FActivation != null)
            {
                var call__1 = AoWHSMap.FUnitControl.FindUnit(FActivation.FUnitID);
                FHero = (THero)call__1;
                Result = FHero != null && !FHero.FUnitStatus.Contains(TUnitStatusElement.usKilled);
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
            FPageKind = Kind;
            LeftSpellPage.Visible = Kind == 1;
            RightSpellPage.Visible = Kind == 1;
            LeftInfoPage.Visible = Kind == 0;
            RightInfoPage.Visible = Kind == 0;
            switch (Kind)
            {
                case 1:
                    var call__2 = FSpells.GetCount();
                    NextBtn.Visible = call__2 - (FPage + 1) * 8 > 0;
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
            TSpell Spell = default;
            if (ValidateHero())
            {
                switch (FMode)
                {
                    case 2:
                    case 3:
                        FSpells.Clear();
                        var call__3 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                        call__3.FMagicControl.ListResearchSpells(ref FSpells, TSet<TSpellType>.Of(TSpellType.stCombat, TSpellType.stUnit, TSpellType.stGlobal));
                        FSpells.SortOnLevel();
                        var call__4 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                        var call__5 = TranslateRStr(ResearchXspellRStr);
                        var call__6 = AoWE.AoWE.Format(call__5, [call__4]);
                        LeftHeader.Text = call__6;
                        var call__7 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                        var call__8 = TranslateRStr(ResearchXspellRStr);
                        var call__9 = AoWE.AoWE.Format(call__8, [call__7]);
                        RightHeader.Text = call__9;
                        // { Original overwrites the type-specific headers. }
                        var call__10 = TranslateRStr(ResearchSpellRStr);
                        LeftHeader.Text = call__10;
                        var call__11 = TranslateRStr(ResearchSpellRStr);
                        RightHeader.Text = call__11;
                        break;
                    case 0:
                        FSpells.Clear();
                        var call__12 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                        call__12.FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
                        FSpells.SortOnLevel();
                        var call__13 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                        var call__14 = TranslateRStr(CastXSpellRStr);
                        var call__15 = AoWE.AoWE.Format(call__14, [call__13]);
                        LeftHeader.Text = call__15;
                        var call__16 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                        var call__17 = TranslateRStr(CastXSpellRStr);
                        var call__18 = AoWE.AoWE.Format(call__17, [call__16]);
                        RightHeader.Text = call__18;
                        break;
                    case 1:
                        FSpells.Clear();
                        var call__19 = AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedPlayer);
                        call__19.FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
                        FSpells.SortOnLevel();
                        var call__20 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                        var call__21 = TranslateRStr(CastXSpellRStr);
                        var call__22 = AoWE.AoWE.Format(call__21, [call__20]);
                        LeftHeader.Text = call__22;
                        var call__23 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                        var call__24 = TranslateRStr(CastXSpellRStr);
                        var call__25 = AoWE.AoWE.Format(call__24, [call__23]);
                        RightHeader.Text = call__25;
                        break;
                }

                {
                    I = FSpells.GetCount() - 1;
                    byte end__26 = 0;
                    for (; I >= end__26; I -= 1)
                    {
                        var call__27 = FSpells.GetSpell(I);
                        bool bool__28 = call__27.FSpellID >= 100;
                        if (bool__28)
                        {
                            var call__29 = FSpells.GetSpell(I);
                            bool__28 = SpellTypes[call__29.FSpellID] == TfxType.fxNone;
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
                var call__31 = FSpells.GetSpell(FPage * 8 + 1);
                Spell = call__31;
                ShowSpellEntry(Spell, S2Pnl, S2Btn, S2Spell, S2Class, S2Cost, S2Memo, S2Icn);
                var call__32 = FSpells.GetSpell(FPage * 8 + 2);
                Spell = call__32;
                ShowSpellEntry(Spell, S3Pnl, S3Btn, S3Spell, S3Class, S3Cost, S3Memo, S3Icn);
                var call__33 = FSpells.GetSpell(FPage * 8 + 3);
                Spell = call__33;
                ShowSpellEntry(Spell, S4Pnl, S4Btn, S4Spell, S4Class, S4Cost, S4Memo, S4Icn);
                var call__34 = FSpells.GetSpell(FPage * 8 + 4);
                Spell = call__34;
                ShowSpellEntry(Spell, S5Pnl, S5btn, S5Spell, S5Class, S5Cost, S5Memo, S5Icn);
                var call__35 = FSpells.GetSpell(FPage * 8 + 5);
                Spell = call__35;
                ShowSpellEntry(Spell, S6Pnl, S6Btn, S6Spell, S6Class, S6Cost, S6Memo, S6Icn);
                var call__36 = FSpells.GetSpell(FPage * 8 + 6);
                Spell = call__36;
                ShowSpellEntry(Spell, S7Pnl, S7Btn, S7Spell, S7Class, S7Cost, S7Memo, S7Icn);
                var call__37 = FSpells.GetSpell(FPage * 8 + 7);
                Spell = call__37;
                ShowSpellEntry(Spell, S8Pnl, S8Btn, S8Spell, S8Class, S8Cost, S8Memo, S8Icn);
                if (FMode == 0)
                {
                    FPlayerPages.Count = AoWHSMap.FSeatedPlayer + 1;
                    FPlayerPages.List[AoWHSMap.FSeatedPlayer].A = (int)FSpellType;
                    FPlayerPages.List[AoWHSMap.FSeatedPlayer].B = FPage;
                }

                if (FPageKind == 1)
                {
                    var call__38 = FSpells.GetCount();
                    NextBtn.Visible = call__38 - (FPage + 1) * 8 > 0;
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
            TLeader Leader = default;
            int I = default;
            TAOWButton Button = default;
            TPlayerMagicControl Control = default;
            TSpell Spell = default;
            if (ValidateHero())
            {
                if (FMode == 1)
                {
                    Player = AoWCombatMap.FSeatedPlayer;
                }
                else
                {
                    Player = AoWHSMap.FSeatedPlayer;
                }

                var call__39 = AoWHSMap.FPlayers.GetPlayers(Player);
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
                            switch ((int)Leader.GetSpheres(I) & 127)
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
                var call__41 = AoWHSMap.FPlayers.GetPlayers(Player);
                var call__42 = TranslateRStr(ManaPoolRStr);
                ManaPoolLbl.Text = call__42 + (AnsiString)": " + IntToStr(call__41.FMagicControl.FMana);
                var call__43 = FHero.GetCastingPoints();
                var call__44 = TranslateRStr(CastingPointsRStr);
                var call__45 = FHero.GetCastingPointsMax();
                CastingPointsLbl.Text = call__44 + (AnsiString)": " + IntToStr(call__43) + AnsiString.FromBytes(47) + IntToStr(call__45);
                ResearchManaPoolLbl.Text = ManaPoolLbl.Text;
                var call__46 = AoWHSMap.FPlayers.GetPlayers(Player);
                var call__47 = call__46.FMagicControl.GetNetPowerToResearch();
                var call__48 = TranslateRStr(ResearchPointsRStr);
                ResearchPointsLbl.Text = call__48 + (AnsiString)": " + IntToStr(call__47);
                if (TSet<byte>.WithBounds(0, 255).With(2).With(3).Contains(FMode))
                {
                    CurrentPointsLbl.Text = (AnsiString)"";
                    CastingPointsMaxLbl.Text = (AnsiString)"";
                    SpellBookLbl.Visible = false;
                    ResearchBookLbl.Visible = true;
                    CastingPage.Visible = false;
                    var call__49 = AoWHSMap.FPlayers.GetPlayers(Player);
                    Control = call__49.FMagicControl;
                    if (Control.FResearchSpellID != 0)
                    {
                        ResearchPnl.Visible = true;
                        ResearchNone.Visible = false;
                        var call__50 = AoWHSSet.FSpellControl.GetSpell(Control.FResearchSpellID);
                        Spell = call__50;
                        var call__51 = TranslateRStr(CurrentlyResearchingRStr);
                        ResearchLbl.Text = call__51 + AnsiString.FromBytes(58);
                        ResearchMemo.Items = Spell.FSpellText;
                        ResearchName.Text = Spell.FSpellName;
                        var call__52 = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                        var call__53 = TranslateRStr(LevelRStr);
                        ResearchClass.Text = call__52 + (AnsiString)", " + call__53 + AnsiString.FromBytes(32) + IntToStr(Spell.FSpellLevel);
                        var call__54 = FHero.CastingMana(Spell);
                        var call__55 = TranslateRStr(CastingCostRStr);
                        var call__56 = TranslateRStr(ManaRStr);
                        ResearchCost.Text = call__55 + (AnsiString)": " + IntToStr(call__54) + AnsiString.FromBytes(32) + call__56;
                        if (Control.GetResearchTurnsLeft() < 999)
                        {
                            var call__57 = Control.GetResearchTurnsLeft();
                            var call__58 = FmtTurnsLeftStr(call__57);
                            ResearchStatus.Text = call__58;
                        }
                        else
                        {
                            var call__59 = TranslateRStr(XTurnsRStr);
                            var call__60 = AoWE.AoWE.Format(call__59, [(byte)63]);
                            ResearchStatus.Text = call__60;
                        }

                        var call__61 = TranslateRStr(CancelRStr);
                        CancelResearchBtn.Text = call__61;
                        ResearchStatus.Visible = true;
                    }
                    else
                    {
                        if (FMode == 3)
                        {
                            ResearchPnl.Visible = true;
                            ResearchNone.Visible = false;
                            var call__62 = AoWHSSet.FSpellControl.GetSpell(FResearchedSpell);
                            Spell = call__62;
                            var call__63 = TranslateRStr(ResearchedRStr);
                            ResearchLbl.Text = call__63 + AnsiString.FromBytes(58);
                            ResearchMemo.Items = Spell.FSpellText;
                            ResearchName.Text = Spell.FSpellName;
                            var call__64 = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                            var call__65 = TranslateRStr(LevelRStr);
                            ResearchClass.Text = call__64 + (AnsiString)", " + call__65 + AnsiString.FromBytes(32) + IntToStr(Spell.FSpellLevel);
                            var call__66 = TranslateRStr(CastingCostRStr);
                            var call__67 = TranslateRStr(ManaRStr);
                            ResearchCost.Text = call__66 + (AnsiString)": " + IntToStr(Spell.FBaseManaCost) + AnsiString.FromBytes(32) + call__67;
                            ResearchStatus.Visible = false;
                            var call__68 = TranslateRStr(ResearchRStr);
                            CancelResearchBtn.Text = call__68;
                            var call__69 = TranslateRStr(ResearchNewSpellRStr);
                            CancelResearchBtn.ToolTipText.Text = call__69;
                        }
                        else
                        {
                            ResearchPnl.Visible = false;
                            ResearchNone.Visible = true;
                            var call__70 = TranslateRStr(ResearchRStr);
                            CancelResearchBtn.Text = call__70;
                            var call__71 = TranslateRStr(CancelSpellResearchRStr);
                            CancelResearchBtn.ToolTipText.Text = call__71;
                        }
                    }
                }
                else
                {
                    if (TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode))
                    {
                        var call__72 = FHero.GetCastingPoints();
                        CurrentPointsLbl.Text = IntToStr(call__72);
                        var call__73 = FHero.GetCastingPointsMax();
                        CastingPointsMaxLbl.Text = IntToStr(call__73);
                        SpellBookLbl.Visible = true;
                        ResearchBookLbl.Visible = false;
                        ResearchPage.Visible = false;
                        var call__74 = FHero.Casting();
                        if (call__74)
                        {
                            NoneCastingLbl.Visible = false;
                            var call__75 = AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID);
                            Spell = call__75;
                            CastingSpell.Text = Spell.FSpellName;
                            var call__76 = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                            var call__77 = TranslateRStr(LevelRStr);
                            CastingClass.Text = call__76 + (AnsiString)", " + call__77 + AnsiString.FromBytes(32) + IntToStr(Spell.FSpellLevel);
                            var call__78 = TranslateRStr(CastingCostRStr);
                            var call__79 = TranslateRStr(ManaRStr);
                            CastingCost.Text = call__78 + (AnsiString)": " + IntToStr(FHero.FCastingProgress) + AnsiString.FromBytes(32) + call__79;
                            CastingMemo.Items = Spell.FSpellText;
                            var call__80 = FHero.CastingTurnsLeft();
                            var call__81 = FmtTurnsLeftStr(call__80);
                            CastingStatus.Text = AnsiString.FromBytes(40) + call__81 + AnsiString.FromBytes(41);
                            CastingPnl.Visible = true;
                            var call__82 = FHero.CastingReady();
                            if (call__82)
                            {
                                CastingBtn.Visible = true;
                                var call__83 = TranslateRStr(CastingReadyRStr);
                                CastingStatus.Text = call__83;
                            }
                            else
                            {
                                CastingBtn.Visible = false;
                                var call__84 = FHero.CastingTurnsLeft();
                                var call__85 = FmtTurnsLeftStr(call__84);
                                CastingStatus.Text = AnsiString.FromBytes(40) + call__85 + AnsiString.FromBytes(41);
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
        public void ShowSpellEntry(TSpell Spell, TAOWPanel Panel, TAOWButton Button, TAOWLabel Name, TAOWLabel SpellClass, TAOWLabel Cost, TAOWMemo Memo, TAOWImage Icon)
        {
            AnsiString Reason = default;
            int Turns = default;
            TPlayerMagicControl Control = default;
            if (Spell != null)
            {
                Panel.Visible = true;
                Name.Text = Spell.FSpellName;
                var call__86 = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                var call__87 = TranslateRStr(LevelRStr);
                SpellClass.Text = call__86 + (AnsiString)", " + call__87 + AnsiString.FromBytes(32) + IntToStr(Spell.FSpellLevel);
                if (FMode == 2)
                {
                    if (Spell.FSpellControlTypes == TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal))
                    {
                        var call__88 = TranslateRStr(GlobalRStr);
                        SpellClass.Text = SpellClass.Text + (AnsiString)", " + call__88;
                    }
                    else
                    {
                        if (Spell.FSpellControlTypes == TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctCombat))
                        {
                            var call__89 = TranslateRStr(CombatRStr);
                            SpellClass.Text = SpellClass.Text + (AnsiString)", " + call__89;
                        }
                        else
                        {
                            if (Spell.FSpellControlTypes == TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat))
                            {
                                var call__90 = TranslateRStr(GlobalRStr);
                                var call__91 = TranslateRStr(CombatRStr);
                                SpellClass.Text = SpellClass.Text + (AnsiString)", " + call__90 + AnsiString.FromBytes(47) + call__91;
                            }
                        }
                    }
                }

                Memo.Items = Spell.FSpellText;
                if (FHero == null)
                {
                    var call__92 = TranslateRStr(CostXRStr);
                    var call__93 = AoWE.AoWE.Format(call__92, [Spell.FBaseManaCost]);
                    Cost.Text = call__93;
                }
                else
                {
                    var call__94 = FHero.CastingMana(Spell);
                    var call__95 = TranslateRStr(CostXRStr);
                    var call__96 = AoWE.AoWE.Format(call__95, [call__94]);
                    Cost.Text = call__96;
                }

                bool bool__97 = FMode == 0;
                if (bool__97)
                {
                    bool bool__98 = !Spell.FSpellControlTypes.Contains(TSpellControlTypeElement.sctGlobal);
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
                    bool bool__101 = FMode == 1;
                    if (bool__101)
                    {
                        bool bool__102 = !Spell.FSpellControlTypes.Contains(TSpellControlTypeElement.sctCombat);
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
                    bool bool__107 = FMode == 2;
                    if (bool__107)
                    {
                        var call__108 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                        bool__107 = call__108.FMagicControl.FResearchSpellID != 0;
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
                    var call__109 = TranslateRStr(UpkeepXRStr);
                    var call__110 = AoWE.AoWE.Format(call__109, [Spell.FManaUpkeep]);
                    Cost.Text = Cost.Text + AnsiString.FromBytes(32) + call__110;
                }

                if (TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode))
                {
                    var call__111 = FHero.CastingTurns(Spell);
                    Turns = call__111;
                    if (Turns == 0)
                    {
                        Reason = TranslateRStr(InstantRStr);
                    }
                    else
                    {
                        var call__112 = TranslateRStr(TurnsXRStr);
                        var call__113 = AoWE.AoWE.Format(call__112, [IntToStr(Turns)]);
                        Reason = call__113;
                    }

                    Cost.Text = Cost.Text + AnsiString.FromBytes(32) + Reason;
                }
                else
                {
                    if (FMode == 2 || FMode == 3)
                    {
                        var call__114 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                        Control = call__114.FMagicControl;
                        var call__115 = Control.GetNetPowerToResearch();
                        var call__116 = Control.ResearchTurns(call__115, Spell.FResearchCost);
                        Turns = call__116;
                        if (Turns < 999)
                        {
                            var call__117 = TranslateRStr(TurnsXRStr);
                            var call__118 = AoWE.AoWE.Format(call__117, [IntToStr(Turns)]);
                            Cost.Text = Cost.Text + AnsiString.FromBytes(32) + call__118;
                        }
                        else
                        {
                            var call__119 = TranslateRStr(XTurnsRStr);
                            var call__120 = AoWE.AoWE.Format(call__119, [(byte)63]);
                            Cost.Text = Cost.Text + AnsiString.FromBytes(32) + call__120;
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
        public void SelectSpell(TSpell Spell)
        {
            AnsiString Reason = default;
            if (ValidateHero())
            {
                switch (FMode)
                {
                    case 2:
                    case 3:
                        var call__121 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                        var call__122 = call__121.FMagicControl.ResearchSpell(Spell.FSpellID);
                        if (call__122)
                        {
                            if (FEvent == null || FEvent.FMode != TPlayerMagicEventMode.pmemStartResearch)
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
                        bool bool__124 = call__123 >= Spell.FBaseManaCost;
                        if (!bool__124)
                        {
                            var call__125 = AoWHSMap.FPlayers.GetPlayers(FHero.FPlayer);
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
                                AoWHSMap.ShowWarning(Reason);
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
                                AoWHSMap.ShowWarning(Reason);
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
                            AoWCombatMap.ShowWarning(Reason);
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
            if (Dlg.Visible && Player == AoWHSMap.FSeatedPlayer)
            {
                UpdateSpellPages();
                UpdateInfoPages();
            }
        }

        // AoWSpellBook.pas:421
        public void UnitChanged(TAbstractUnit U)
        {
            if (Dlg.Visible && (U == null || TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode) && U == FHero))
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
            var call__130 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            if (call__130.FMagicControl.FResearchSpellID != 0)
            {
                SetPageKind(0);
            }
            else
            {
                SetPageKind(1);
            }

            FMode = 2;
            FSpellType = TSpellType.stGlobal;
            FPage = 0;
            var call__131 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            FHero = call__131.FLeader;
            UpdateSpellPages();
            UpdateInfoPages();
            ShowBook();
        }

        // AoWSpellBook.pas:501
        public void ShowResearchEvent(TPlayerMagicEventLog Event, TExecuteDoneEvent Done)
        {
            FEvent = Event;
            FEvent.AddRef();
            FDone = Done;
            var call__132 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
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
                FSpellType = TSpellType.stGlobal;
                FPage = 0;
                var call__133 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                FHero = call__133.FLeader;
                UpdateSpellPages();
                UpdateInfoPages();
                ShowBook();
                AoWHSMap.FTutorialControl.ShowMessage(51, true, true);
            }
            else
            {
                ShowResearch();
            }
        }

        // AoWSpellBook.pas:518
        public void ShowResearchedEvent(TPlayerMagicEventLog Event, TExecuteDoneEvent Done)
        {
            FEvent = Event;
            FEvent.AddRef();
            FDone = Done;
            var call__134 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            if (call__134.FMagicControl.FResearchSpellID == 0)
            {
                Reset();
                FResearchedSpell = Event.FSpellID;
                SetPageKind(0);
                FMode = 3;
                FSpellType = TSpellType.stGlobal;
                FPage = 0;
                var call__135 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
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
        public void ShowCasting(TActivateAbilityInfo Activation)
        {
            Reset();
            FActivation = Activation;
            FActivation.AddRef();
            FMode = 0;
            if (ValidateHero())
            {
                var call__136 = FPlayerPages[AoWHSMap.FSeatedPlayer];
                FSpellType = (TSpellType)call__136.A;
                var call__137 = FPlayerPages[AoWHSMap.FSeatedPlayer];
                FPage = call__137.B;
                var call__138 = FHero.Casting();
                if (call__138)
                {
                    SetPageKind(0);
                }
                else
                {
                    FSpells.Clear();
                    var call__139 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    call__139.FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
                    var call__140 = FSpells.GetCount();
                    bool bool__141 = call__140 == 0;
                    if (!bool__141)
                    {
                        var call__142 = FSpells.GetCount();
                        bool__141 = call__142 <= FPage * 8;
                    }

                    if (bool__141)
                    {
                        FSpells.Clear();
                        FSpellType = (TSpellType)3;
                        while (true)
                        {
                            var call__143 = FSpells.GetCount();
                            if (!(call__143 == 0 && (int)FSpellType != 0))
                                break;
                            {
                                FSpellType = (TSpellType)((int)FSpellType - 1);
                                var call__144 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                                call__144.FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
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
        public void ShowCombatCasting(TActivateAbilityInfo Activation)
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
                    FSpellType = (TSpellType)3;
                    while (true)
                    {
                        var call__146 = FSpells.GetCount();
                        if (!(call__146 == 0 && (int)FSpellType != 0))
                            break;
                        {
                            FSpellType = (TSpellType)((int)FSpellType - 1);
                            var call__147 = AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedPlayer);
                            call__147.FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
                        }
                    }

                    SetPageKind(1);
                }

                FSpellType = TSpellType.stCombat;
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
            TDoubleItem Defaults = default;
            var call__148 = new TSpellList();
            FSpells = call__148;
            Defaults.A = 2;
            Defaults.B = 0;
            var call__149 = new TDoubleItemList();
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
            if (AoWHSMap.FCombatMap != null && General.FCombatUnit != null)
            {
                General.FCombatUnit.SelectAbility(-1);
            }

            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWSpellBook.pas:222
        public void NextBtnClick()
        {
            FPage += 1;
            UpdateSpellPages();
            Dlg.StartAnim(PageTurnCtrl.TPageTurnMode.ptLeft);
            Dlg.FPageTurn.FSpeed = 25;
            PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
        }

        // AoWSpellBook.pas:229
        public void PrevBtnClick()
        {
            FPage -= 1;
            UpdateSpellPages();
            Dlg.StartAnim(PageTurnCtrl.TPageTurnMode.ptRight);
            Dlg.FPageTurn.FSpeed = 25;
            PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
        }

        // AoWSpellBook.pas:236
        public void SpellIconDraw(TObject Sender, TSurfaceDesc SD)
        {
            int Index = default;
            int X = default;
            int Y = default;
            Index = FPage * 8 + (((TComponent)Sender).Tag & 15);
            bool bool__150 = Index >= 0;
            if (bool__150)
            {
                var call__151 = FSpells.GetCount();
                bool__150 = Index < call__151;
            }

            if (bool__150)
            {
                var call__152 = FSpells.GetSpell(Index);
                var call__153 = call__152.FCastImages.GetImageSequence(10);
                if (call__153 != null)
                {
                    var image = (TAOWImage)Sender;
                    X = image.FAOWWindow.WinLeft;
                    Y = image.FAOWWindow.WinTop;
                    var call__155 = FSpells.GetSpell(Index);
                    call__155.FCastImages.Show(10, image.WinLeft + X, image.WinTop + Y, 0, SD);
                }
            }
        }

        // AoWSpellBook.pas:249
        public void SelectSpellBtnClick(TObject Sender)
        {
            var call__156 = FSpells.GetSpell(FPage * 8 + ((TComponent)Sender).Tag);
            SelectSpell(call__156);
        }

        // AoWSpellBook.pas:254
        public void GlobalBtnClick(TObject Sender)
        {
            if (FSpellType != TSpellType.stGlobal || FPage != 0 || FPageKind != 1)
            {
                if (FPageKind == 0)
                {
                    Dlg.StartAnim(PageTurnCtrl.TPageTurnMode.ptLeft);
                }
                else
                {
                    Dlg.StartAnim(PageTurnCtrl.TPageTurnMode.ptRight);
                }

                PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
                FSpellType = TSpellType.stGlobal;
                FPage = 0;
                UpdateSpellPages();
                SetPageKind(1);
            }
        }

        // AoWSpellBook.pas:264
        public void UnitBtnClick()
        {
            if (FSpellType != TSpellType.stUnit || FPage != 0 || FPageKind != 1)
            {
                if (FPageKind == 0 || FSpellType == TSpellType.stGlobal)
                {
                    Dlg.StartAnim(PageTurnCtrl.TPageTurnMode.ptLeft);
                }
                else
                {
                    Dlg.StartAnim(PageTurnCtrl.TPageTurnMode.ptRight);
                }

                PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
                FSpellType = TSpellType.stUnit;
                FPage = 0;
                UpdateSpellPages();
                SetPageKind(1);
            }
        }

        // AoWSpellBook.pas:275
        public void CombatBtnClick()
        {
            if (FSpellType != TSpellType.stCombat || FPage != 0 || FPageKind != 1)
            {
                Dlg.StartAnim(PageTurnCtrl.TPageTurnMode.ptLeft);
                PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
                FSpellType = TSpellType.stCombat;
                FPage = 0;
                UpdateSpellPages();
                SetPageKind(1);
            }
        }

        // AoWSpellBook.pas:285
        public void InfoBtnClick(TObject Sender)
        {
            if (FPageKind != 0)
            {
                Dlg.StartAnim(PageTurnCtrl.TPageTurnMode.ptRight);
                PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
                SetPageKind(0);
            }
        }

        // AoWSpellBook.pas:295
        public void CastingBtnClick()
        {
            AnsiString Reason = default;
            General.PlayButtonSound(0);
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
                            AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                    case 1:
                        var call__158 = AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID);
                        SelectSpell(call__158);
                        break;
                }
            }
        }

        // AoWSpellBook.pas:309
        public void CancelCastingBtnClick()
        {
            General.PlayButtonSound(0);
            if (ValidateHero())
            {
                FHero.CancelCasting();
            }
        }

        // AoWSpellBook.pas:315
        public void FaceDraw(TObject Sender, TSurfaceDesc SD)
        {
            TRect R = default;
            var image = (TAOWImage)Sender;
            var call__160 = Rect(LeftInfoPage.WinLeft + image.WinLeft, LeftInfoPage.WinTop + image.WinTop, LeftInfoPage.WinLeft + image.WinLeft + image.WinWidth, LeftInfoPage.WinTop + image.WinTop + image.WinHeight);
            R = call__160;

            var call__161 = FHero.GetFace();
            call__161.ShowStretched(R, SD);
        }

        // AoWSpellBook.pas:325
        public void ResearchIcnDraw(TAOWImage Sender, TSurfaceDesc SD)
        {
            TSpell Spell = default;
            int X = default;
            var call__162 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__163 = AoWHSSet.FSpellControl.GetSpell(call__162.FMagicControl.FResearchSpellID);
            Spell = call__163;
            if (Spell == null && FMode == 3)
            {
                var call__164 = AoWHSSet.FSpellControl.GetSpell(FResearchedSpell);
                Spell = call__164;
            }

            if (Spell != null)
            {
                X = Sender.FAOWWindow.WinLeft;
                Spell.FCastImages.Show(10, Sender.WinLeft + X, Sender.FAOWWindow.WinTop + Sender.WinTop, 0, SD);
            }
        }

        // AoWSpellBook.pas:337
        public void CastingIcnDraw(TAOWImage Sender, TSurfaceDesc SD)
        {
            TSpell Spell = default;
            int X = default;
            if (FHero != null)
            {
                var call__166 = FHero.Casting();
                if (call__166)
                {
                    var call__167 = AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID);
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
            TPlayerMagicControl Control = default;
            General.PlayButtonSound(0);
            if (ValidateHero())
            {
                var call__169 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
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
            General.PlayButtonSound(0);
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
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27)
            {
                if (AoWHSMap.FCombatMap != null && General.FCombatUnit != null)
                {
                    General.FCombatUnit.SelectAbility(-1);
                }

                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWSpellBook
    {
    }
}
