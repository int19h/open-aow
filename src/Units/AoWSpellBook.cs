// Translated from AoWSpellBook.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine.Collections;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AoWSpellBook.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Audio.SoundUnit;
using static Aow.Game.Combat.AoWTCUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
        TExecuteDoneEvent Done;
        TPlayerMagicEventLog Event;
        PlaySample("SFX\\INT\\BOOKCLSE.WAV");
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
        AoWExeFormsUnit.MagicWin.Dlg.FDrawn = false;
    }

    // AoWSpellBook.pas:405
    public bool ValidateHero()
    {
        bool Result;
        if (FActivation != null)
        {
            FHero = AoWHSMap.FUnitControl.FindUnit<THero>(FActivation.FUnitID);
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
                var count = FSpells.GetCount();
                NextBtn.Visible = count - (FPage + 1) * 8 > 0;
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
        TSpell Spell;
        if (ValidateHero())
        {
            switch (FMode)
            {
                case 2:
                case 3:
                    FSpells.Clear();
                    AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.ListResearchSpells(ref FSpells, TSet<TSpellType>.Of(TSpellType.stCombat, TSpellType.stUnit, TSpellType.stGlobal));
                    FSpells.SortOnLevel();
                    var spellType = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                    var format = AoWEUnit.Format(TranslateRStr(ResearchXspellRStr), [spellType]);
                    LeftHeader.Text = format;
                    var spellTypeText = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                    var aoWEFormat = AoWEUnit.Format(TranslateRStr(ResearchXspellRStr), [spellTypeText]);
                    RightHeader.Text = aoWEFormat;
                    // { Original overwrites the type-specific headers. }
                    var researchSpell = TranslateRStr(ResearchSpellRStr);
                    LeftHeader.Text = researchSpell;
                    var researchSpellText = TranslateRStr(ResearchSpellRStr);
                    RightHeader.Text = researchSpellText;
                    break;
                case 0:
                    FSpells.Clear();
                    AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
                    FSpells.SortOnLevel();
                    var spellType3 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                    var format2 = AoWEUnit.Format(TranslateRStr(CastXSpellRStr), [spellType3]);
                    LeftHeader.Text = format2;
                    var spellType4 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                    var format3 = AoWEUnit.Format(TranslateRStr(CastXSpellRStr), [spellType4]);
                    RightHeader.Text = format3;
                    break;
                case 1:
                    FSpells.Clear();
                    AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
                    FSpells.SortOnLevel();
                    var spellType5 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                    var format4 = AoWEUnit.Format(TranslateRStr(CastXSpellRStr), [spellType5]);
                    LeftHeader.Text = format4;
                    var spellType6 = TranslateRStr(TSpellTypeRStr[(int)FSpellType]);
                    var format5 = AoWEUnit.Format(TranslateRStr(CastXSpellRStr), [spellType6]);
                    RightHeader.Text = format5;
                    break;
            }

            for (int I = FSpells.GetCount() - 1; I >= 0; I--)
            {
                bool condition = FSpells.GetSpell(I).FSpellID >= 100;
                if (condition)
                {
                    var spell = FSpells.GetSpell(I);
                    condition = SpellTypes[spell.FSpellID] == TfxType.fxNone;
                }

                if (condition)
                {
                    FSpells.Delete(I);
                }
            }

            Spell = FSpells.GetSpell(FPage * 8);
            ShowSpellEntry(Spell, S1Pnl, S1Btn, S1Spell, S1Class, S1Cost, S1Memo, S1Icn);
            Spell = FSpells.GetSpell(FPage * 8 + 1);
            ShowSpellEntry(Spell, S2Pnl, S2Btn, S2Spell, S2Class, S2Cost, S2Memo, S2Icn);
            Spell = FSpells.GetSpell(FPage * 8 + 2);
            ShowSpellEntry(Spell, S3Pnl, S3Btn, S3Spell, S3Class, S3Cost, S3Memo, S3Icn);
            Spell = FSpells.GetSpell(FPage * 8 + 3);
            ShowSpellEntry(Spell, S4Pnl, S4Btn, S4Spell, S4Class, S4Cost, S4Memo, S4Icn);
            Spell = FSpells.GetSpell(FPage * 8 + 4);
            ShowSpellEntry(Spell, S5Pnl, S5btn, S5Spell, S5Class, S5Cost, S5Memo, S5Icn);
            Spell = FSpells.GetSpell(FPage * 8 + 5);
            ShowSpellEntry(Spell, S6Pnl, S6Btn, S6Spell, S6Class, S6Cost, S6Memo, S6Icn);
            Spell = FSpells.GetSpell(FPage * 8 + 6);
            ShowSpellEntry(Spell, S7Pnl, S7Btn, S7Spell, S7Class, S7Cost, S7Memo, S7Icn);
            Spell = FSpells.GetSpell(FPage * 8 + 7);
            ShowSpellEntry(Spell, S8Pnl, S8Btn, S8Spell, S8Class, S8Cost, S8Memo, S8Icn);
            if (FMode == 0)
            {
                FPlayerPages.Count = AoWHSMap.FSeatedPlayer + 1;
                FPlayerPages.List[AoWHSMap.FSeatedPlayer].A = (int)FSpellType;
                FPlayerPages.List[AoWHSMap.FSeatedPlayer].B = FPage;
            }

            if (FPageKind == 1)
            {
                var count = FSpells.GetCount();
                NextBtn.Visible = count - (FPage + 1) * 8 > 0;
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
        sbyte Player;
        TLeader Leader;
        TAOWButton Button;
        TPlayerMagicControl Control;
        TSpell Spell;
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

            Leader = AoWHSMap.FPlayers.GetPlayers(Player).FLeader;
            for (int I = 0; I <= 6; I++)
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

            NameLbl.Text = FHero.FHeroName;
            var player = AoWHSMap.FPlayers.GetPlayers(Player);
            var manaPool = TranslateRStr(ManaPoolRStr);
            ManaPoolLbl.Text = manaPool + ": " + IntToStr(player.FMagicControl.FMana);
            var castingPoints = FHero.GetCastingPoints();
            var castingPointsText = TranslateRStr(CastingPointsRStr);
            var castingPointsMax = FHero.GetCastingPointsMax();
            CastingPointsLbl.Text = castingPointsText + ": " + IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
            ResearchManaPoolLbl.Text = ManaPoolLbl.Text;
            var netPowerToResearch = AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.GetNetPowerToResearch();
            var researchPoints = TranslateRStr(ResearchPointsRStr);
            ResearchPointsLbl.Text = researchPoints + ": " + IntToStr(netPowerToResearch);
            if (TSet<byte>.WithBounds(0, 255).With(2).With(3).Contains(FMode))
            {
                CurrentPointsLbl.Text = "";
                CastingPointsMaxLbl.Text = "";
                SpellBookLbl.Visible = false;
                ResearchBookLbl.Visible = true;
                CastingPage.Visible = false;
                Control = AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl;
                if (Control.FResearchSpellID != 0)
                {
                    ResearchPnl.Visible = true;
                    ResearchNone.Visible = false;
                    Spell = AoWHSSet.FSpellControl.GetSpell(Control.FResearchSpellID);
                    var currentlyResearching = TranslateRStr(CurrentlyResearchingRStr);
                    ResearchLbl.Text = currentlyResearching + ":";
                    ResearchMemo.Items = Spell.FSpellText;
                    ResearchName.Text = Spell.FSpellName;
                    var magicSphere = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                    var level = TranslateRStr(LevelRStr);
                    ResearchClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                    var castingMana = FHero.CastingMana(Spell);
                    var castingCost = TranslateRStr(CastingCostRStr);
                    var mana = TranslateRStr(ManaRStr);
                    ResearchCost.Text = castingCost + ": " + IntToStr(castingMana) + " " + mana;
                    if (Control.GetResearchTurnsLeft() < 999)
                    {
                        var fmtTurnsLeftStr = FmtTurnsLeftStr(Control.GetResearchTurnsLeft());
                        ResearchStatus.Text = fmtTurnsLeftStr;
                    }
                    else
                    {
                        var format = AoWEUnit.Format(TranslateRStr(XTurnsRStr), [(byte)63]);
                        ResearchStatus.Text = format;
                    }

                    var cancel = TranslateRStr(CancelRStr);
                    CancelResearchBtn.Text = cancel;
                    ResearchStatus.Visible = true;
                }
                else
                {
                    if (FMode == 3)
                    {
                        ResearchPnl.Visible = true;
                        ResearchNone.Visible = false;
                        Spell = AoWHSSet.FSpellControl.GetSpell(FResearchedSpell);
                        var researched = TranslateRStr(ResearchedRStr);
                        ResearchLbl.Text = researched + ":";
                        ResearchMemo.Items = Spell.FSpellText;
                        ResearchName.Text = Spell.FSpellName;
                        var magicSphere = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                        var level = TranslateRStr(LevelRStr);
                        ResearchClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                        var castingCost = TranslateRStr(CastingCostRStr);
                        var mana = TranslateRStr(ManaRStr);
                        ResearchCost.Text = castingCost + ": " + IntToStr(Spell.FBaseManaCost) + " " + mana;
                        ResearchStatus.Visible = false;
                        var research = TranslateRStr(ResearchRStr);
                        CancelResearchBtn.Text = research;
                        var researchNewSpell = TranslateRStr(ResearchNewSpellRStr);
                        CancelResearchBtn.ToolTipText.Text = researchNewSpell;
                    }
                    else
                    {
                        ResearchPnl.Visible = false;
                        ResearchNone.Visible = true;
                        var research = TranslateRStr(ResearchRStr);
                        CancelResearchBtn.Text = research;
                        var cancelSpellResearch = TranslateRStr(CancelSpellResearchRStr);
                        CancelResearchBtn.ToolTipText.Text = cancelSpellResearch;
                    }
                }
            }
            else
            {
                if (TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode))
                {
                    var fHeroCastingPoints = FHero.GetCastingPoints();
                    CurrentPointsLbl.Text = IntToStr(fHeroCastingPoints);
                    var fHeroCastingPointsMax = FHero.GetCastingPointsMax();
                    CastingPointsMaxLbl.Text = IntToStr(fHeroCastingPointsMax);
                    SpellBookLbl.Visible = true;
                    ResearchBookLbl.Visible = false;
                    ResearchPage.Visible = false;
                    if (FHero.Casting())
                    {
                        NoneCastingLbl.Visible = false;
                        Spell = AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID);
                        CastingSpell.Text = Spell.FSpellName;
                        var magicSphere = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                        var level = TranslateRStr(LevelRStr);
                        CastingClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                        var castingCost = TranslateRStr(CastingCostRStr);
                        var mana = TranslateRStr(ManaRStr);
                        CastingCost.Text = castingCost + ": " + IntToStr(FHero.FCastingProgress) + " " + mana;
                        CastingMemo.Items = Spell.FSpellText;
                        var fmtTurnsLeftStr = FmtTurnsLeftStr(FHero.CastingTurnsLeft());
                        CastingStatus.Text = "(" + fmtTurnsLeftStr + ")";
                        CastingPnl.Visible = true;
                        if (FHero.CastingReady())
                        {
                            CastingBtn.Visible = true;
                            var castingReady = TranslateRStr(CastingReadyRStr);
                            CastingStatus.Text = castingReady;
                        }
                        else
                        {
                            CastingBtn.Visible = false;
                            var fmtTurnsLeftStr2 = FmtTurnsLeftStr(FHero.CastingTurnsLeft());
                            CastingStatus.Text = "(" + fmtTurnsLeftStr2 + ")";
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
        string Reason = "";
        int Turns;
        TPlayerMagicControl Control;
        if (Spell != null)
        {
            Panel.Visible = true;
            Name.Text = Spell.FSpellName;
            SpellClass.Text = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]) + ", " + TranslateRStr(LevelRStr) + " " + IntToStr(Spell.FSpellLevel);
            if (FMode == 2)
            {
                if (Spell.FSpellControlTypes == TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal))
                {
                    var global = TranslateRStr(GlobalRStr);
                    SpellClass.Text = SpellClass.Text + ", " + global;
                }
                else
                {
                    if (Spell.FSpellControlTypes == TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctCombat))
                    {
                        var combat = TranslateRStr(CombatRStr);
                        SpellClass.Text = SpellClass.Text + ", " + combat;
                    }
                    else
                    {
                        if (Spell.FSpellControlTypes == TSet<TSpellControlTypeElement>.Of(TSpellControlTypeElement.sctGlobal, TSpellControlTypeElement.sctCombat))
                        {
                            var global = TranslateRStr(GlobalRStr);
                            var combat = TranslateRStr(CombatRStr);
                            SpellClass.Text = SpellClass.Text + ", " + global + "/" + combat;
                        }
                    }
                }
            }

            Memo.Items = Spell.FSpellText;
            if (FHero == null)
            {
                Cost.Text = AoWEUnit.Format(TranslateRStr(CostXRStr), [Spell.FBaseManaCost]);
            }
            else
            {
                var castingMana = FHero.CastingMana(Spell);
                Cost.Text = AoWEUnit.Format(TranslateRStr(CostXRStr), [castingMana]);
            }

            if (FMode == 0 && (!Spell.FSpellControlTypes.Contains(TSpellControlTypeElement.sctGlobal) || FHero.Casting()) || FMode == 1 && (!Spell.FSpellControlTypes.Contains(TSpellControlTypeElement.sctCombat) || FHero.Casting() || !FHero.CanCastSpellInstantly(Spell.FSpellID, Spell.FBaseManaCost, ref Reason)) || FMode == 2 && AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.FResearchSpellID != 0)
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
                var format = AoWEUnit.Format(TranslateRStr(UpkeepXRStr), [Spell.FManaUpkeep]);
                Cost.Text = Cost.Text + " " + format;
            }

            if (TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode))
            {
                Turns = FHero.CastingTurns(Spell);
                if (Turns == 0)
                {
                    Reason = TranslateRStr(InstantRStr);
                }
                else
                {
                    Reason = AoWEUnit.Format(TranslateRStr(TurnsXRStr), [IntToStr(Turns)]);
                }

                Cost.Text = Cost.Text + " " + Reason;
            }
            else
            {
                if (FMode == 2 || FMode == 3)
                {
                    Control = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl;
                    Turns = Control.ResearchTurns(Control.GetNetPowerToResearch(), Spell.FResearchCost);
                    if (Turns < 999)
                    {
                        var format = AoWEUnit.Format(TranslateRStr(TurnsXRStr), [IntToStr(Turns)]);
                        Cost.Text = Cost.Text + " " + format;
                    }
                    else
                    {
                        var format = AoWEUnit.Format(TranslateRStr(XTurnsRStr), [(byte)63]);
                        Cost.Text = Cost.Text + " " + format;
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
        string Reason = "";
        if (ValidateHero())
        {
            switch (FMode)
            {
                case 2:
                case 3:
                    if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.ResearchSpell(Spell.FSpellID))
                    {
                        if (FEvent == null || FEvent.FMode != TPlayerMagicEventMode.pmemStartResearch)
                        {
                            FMode = 2;
                            InfoBtnClick();
                        }
                        else
                        {
                            Dlg.Visible = false;
                        }
                    }

                    break;
                case 0:
                    if (FHero.GetCastingPoints() >= Spell.FBaseManaCost || AoWHSMap.FPlayers.GetPlayers(FHero.FPlayer).FMagicControl.GetCheatEnabled())
                    {
                        if (FHero.CastSpell(Spell.FSpellID, ref Reason))
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
                        if (FHero.CastSpell(Spell.FSpellID, ref Reason))
                        {
                            InfoBtnClick();
                        }
                        else
                        {
                            AoWHSMap.ShowWarning(Reason);
                        }
                    }

                    break;
                case 1:
                    if (FHero.CastCombatSpell(Spell.FSpellID, ref Reason))
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
        if (Dlg.Visible && (U == null || (TSet<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode) && U == FHero)))
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
        if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.FResearchSpellID != 0)
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
        FHero = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FLeader;
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
        if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.FResearchSpellID == 0)
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
            FHero = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FLeader;
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
        if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.FResearchSpellID == 0)
        {
            Reset();
            FResearchedSpell = Event.FSpellID;
            SetPageKind(0);
            FMode = 3;
            FSpellType = TSpellType.stGlobal;
            FPage = 0;
            FHero = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FLeader;
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
            FSpellType = (TSpellType)FPlayerPages[AoWHSMap.FSeatedPlayer].A;
            FPage = FPlayerPages[AoWHSMap.FSeatedPlayer].B;
            if (FHero.Casting())
            {
                SetPageKind(0);
            }
            else
            {
                FSpells.Clear();
                AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
                if (FSpells.GetCount() == 0 || FSpells.GetCount() <= FPage * 8)
                {
                    FSpells.Clear();
                    FSpellType = (TSpellType)3;
                    while (FSpells.GetCount() == 0 && (int)FSpellType != 0)
                    {
                        FSpellType = (TSpellType)((int)FSpellType - 1);
                        AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
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
            if (FHero.Casting())
            {
                SetPageKind(0);
            }
            else
            {
                FSpells.Clear();
                FSpellType = (TSpellType)3;
                while (FSpells.GetCount() == 0 && (int)FSpellType != 0)
                {
                    FSpellType = (TSpellType)((int)FSpellType - 1);
                    AoWHSMap.FPlayers.GetPlayers(AoWCombatMap.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, TSet<TSpellType>.Of(FSpellType));
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
        FSpells = new TSpellList();
        Defaults.A = 2;
        Defaults.B = 0;
        FPlayerPages = new TDoubleItemList();
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

        FPlayerPages?.Dispose();
        FSpells?.Dispose();
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
        Dlg.StartAnim(Aow.Game.Ui.Grids.TPageTurnMode.ptLeft);
        Dlg.FPageTurn.FSpeed = 25;
        PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:229
    public void PrevBtnClick()
    {
        FPage -= 1;
        UpdateSpellPages();
        Dlg.StartAnim(Aow.Game.Ui.Grids.TPageTurnMode.ptRight);
        Dlg.FPageTurn.FSpeed = 25;
        PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:236
    public void SpellIconDraw(TAOWImage Sender, TSurfaceDesc SD, int Slot)
    {
        int Index;
        int X;
        int Y;
        Index = FPage * 8 + Slot;
        if (Index >= 0 && Index < FSpells.GetCount())
        {
            if (FSpells.GetSpell(Index).FCastImages.GetImageSequence(10) != null)
            {
                var image = Sender;
                X = image.FAOWWindow.WinLeft;
                Y = image.FAOWWindow.WinTop;
                FSpells.GetSpell(Index).FCastImages.Show(10, image.WinLeft + X, image.WinTop + Y, 0, SD);
            }
        }
    }

    // AoWSpellBook.pas:249
    public void SelectSpellBtnClick(int Slot)
    {
        SelectSpell(FSpells.GetSpell(FPage * 8 + Slot));
    }

    // AoWSpellBook.pas:254
    public void GlobalBtnClick()
    {
        if (FSpellType != TSpellType.stGlobal || FPage != 0 || FPageKind != 1)
        {
            if (FPageKind == 0)
            {
                Dlg.StartAnim(Aow.Game.Ui.Grids.TPageTurnMode.ptLeft);
            }
            else
            {
                Dlg.StartAnim(Aow.Game.Ui.Grids.TPageTurnMode.ptRight);
            }

            PlaySample("Sfx\\Int\\Pagef1.wav");
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
                Dlg.StartAnim(Aow.Game.Ui.Grids.TPageTurnMode.ptLeft);
            }
            else
            {
                Dlg.StartAnim(Aow.Game.Ui.Grids.TPageTurnMode.ptRight);
            }

            PlaySample("Sfx\\Int\\Pagef1.wav");
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
            Dlg.StartAnim(Aow.Game.Ui.Grids.TPageTurnMode.ptLeft);
            PlaySample("Sfx\\Int\\Pagef1.wav");
            FSpellType = TSpellType.stCombat;
            FPage = 0;
            UpdateSpellPages();
            SetPageKind(1);
        }
    }

    // AoWSpellBook.pas:285
    public void InfoBtnClick()
    {
        if (FPageKind != 0)
        {
            Dlg.StartAnim(Aow.Game.Ui.Grids.TPageTurnMode.ptRight);
            PlaySample("Sfx\\Int\\Pagef1.wav");
            SetPageKind(0);
        }
    }

    // AoWSpellBook.pas:295
    public void CastingBtnClick()
    {
        string Reason = "";
        General.PlayButtonSound(0);
        if (ValidateHero())
        {
            switch (FMode)
            {
                case 0:
                    if (FHero.CastSpell(FHero.FCastingSpellID, ref Reason))
                    {
                        Dlg.Visible = false;
                    }
                    else
                    {
                        AoWHSMap.ShowWarning(Reason);
                    }

                    break;
                case 1:
                    SelectSpell(AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID));
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
    public void FaceDraw(TAOWImage Sender, TSurfaceDesc SD)
    {
        TRect R;
        R = Rect(LeftInfoPage.WinLeft + Sender.WinLeft, LeftInfoPage.WinTop + Sender.WinTop, LeftInfoPage.WinLeft + Sender.WinLeft + Sender.WinWidth, LeftInfoPage.WinTop + Sender.WinTop + Sender.WinHeight);

        FHero.GetFace().ShowStretched(R, SD);
    }

    // AoWSpellBook.pas:325
    public void ResearchIcnDraw(Aow.Game.Ui.Controls.TAOWControl Sender, TSurfaceDesc SD)
    {
        TSpell Spell;
        int X;
        var player = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
        Spell = AoWHSSet.FSpellControl.GetSpell(player.FMagicControl.FResearchSpellID);
        if (Spell == null && FMode == 3)
        {
            Spell = AoWHSSet.FSpellControl.GetSpell(FResearchedSpell);
        }

        if (Spell != null)
        {
            X = Sender.FAOWWindow.WinLeft;
            Spell.FCastImages.Show(10, Sender.WinLeft + X, Sender.FAOWWindow.WinTop + Sender.WinTop, 0, SD);
        }
    }

    // AoWSpellBook.pas:337
    public void CastingIcnDraw(Aow.Game.Ui.Controls.TAOWControl Sender, TSurfaceDesc SD)
    {
        TSpell Spell;
        int X;
        if (FHero != null)
        {
            if (FHero.Casting())
            {
                Spell = AoWHSSet.FSpellControl.GetSpell(FHero.FCastingSpellID);
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
        TPlayerMagicControl Control;
        General.PlayButtonSound(0);
        if (ValidateHero())
        {
            Control = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl;
            if (Control.FResearchSpellID != 0)
            {
                Control.ResearchSpell(0);
            }
            else
            {
                GlobalBtnClick();
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
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
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

    // AoWSpellBook.pas:385. The host surfaces reside in system memory.
    public void ShowBook()
    {
        General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }
}

public static partial class AoWSpellBookUnit
{
}
