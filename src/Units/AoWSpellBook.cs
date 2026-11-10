// Translated from AoWSpellBook.pas.
using Aow.Engine.Collections;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine.Collections;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
// Translated from AoWSpellBook.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// mode, page kind, spell type, spells, page, hero, activation, researched spell, callback,
// event, saved player pages
public partial class SpellBook : DataModule
{
    public SpellBook(Component owner) : base(owner)
    { }

    public AOWLabel LeftHeader;
    public AOWPanel LeftSpellPage;
    public AOWPanel S1Pnl;
    public AOWLabel S1Spell;
    public AOWLabel S1Class;
    public AOWMemo S1Memo;
    public AOWLabel S1Cost;
    public AOWImage S1Icn;
    public AOWPanel S2Pnl;
    public AOWLabel S2Spell;
    public AOWLabel S2Class;
    public AOWMemo S2Memo;
    public AOWLabel S2Cost;
    public AOWImage S2Icn;
    public AOWPanel S3Pnl;
    public AOWLabel S3Spell;
    public AOWLabel S3Class;
    public AOWMemo S3Memo;
    public AOWLabel S3Cost;
    public AOWImage S3Icn;
    public AOWPanel S4Pnl;
    public AOWLabel S4Spell;
    public AOWLabel S4Class;
    public AOWMemo S4Memo;
    public AOWLabel S4Cost;
    public AOWImage S4Icn;
    public AOWLabel RightHeader;
    public AOWPanel RightSpellPage;
    public AOWPanel S5Pnl;
    public AOWLabel S5Spell;
    public AOWLabel S5Class;
    public AOWMemo S5Memo;
    public AOWLabel S5Cost;
    public AOWImage S5Icn;
    public AOWPanel S6Pnl;
    public AOWLabel S6Spell;
    public AOWLabel S6Class;
    public AOWMemo S6Memo;
    public AOWLabel S6Cost;
    public AOWImage S6Icn;
    public AOWPanel S7Pnl;
    public AOWLabel S7Spell;
    public AOWLabel S7Class;
    public AOWMemo S7Memo;
    public AOWLabel S7Cost;
    public AOWImage S7Icn;
    public AOWPanel S8Pnl;
    public AOWLabel S8Spell;
    public AOWLabel S8Class;
    public AOWMemo S8Memo;
    public AOWLabel S8Cost;
    public AOWImage S8Icn;
    public AOWButton S1Btn;
    public AOWLabel CurrentPointsLbl;
    public AOWLabel Line;
    public AOWLabel CastingPointsMaxLbl;
    public AOWButton GlobalBtn;
    public AOWButton UnitBtn;
    public AOWButton CombatBtn;
    public AOWButton InfoBtn;
    public AOWButton PrevBtn;
    public AOWButton NextBtn;
    public AOWButton Cancel;
    public AOWButton S2Btn;
    public AOWButton S3Btn;
    public AOWButton S4Btn;
    public AOWButton S5btn;
    public AOWButton S6Btn;
    public AOWButton S7Btn;
    public AOWButton S8Btn;
    public AOWPanel CastingPage;
    public AOWPanel CastingPnl;
    public AOWLabel CastingSpell;
    public AOWLabel CastingClass;
    public AOWMemo CastingMemo;
    public AOWLabel CastingCost;
    public AOWImage CastingIcn;
    public AOWPanel LeftInfoPage;
    public AOWLabel CastingStatus;
    public AOWLabel CastingPointsLbl;
    public AOWButton CastingBtn;
    public AOWButton Sel4;
    public AOWPanel SelPnl;
    public AOWButton Sel3;
    public AOWButton Sel2;
    public AOWButton Sel1;
    public AOWButton Sel5;
    public AOWButton Sel6;
    public AOWButton Sel7;
    public AOWLabel SelLbl;
    public AOWLabel SpellBookLbl;
    public AOWLabel NameLbl;
    public AOWLabel ManaPoolLbl;
    public AOWLabel CastingLbl;
    public AOWImage FaceFrame;
    public AOWImage Face;
    public AoWBookWindow Dlg;
    public AOWLabel NoneCastingLbl;
    public AOWPanel ResearchPage;
    public AOWLabel ResearchName;
    public AOWLabel ResearchClass;
    public AOWMemo ResearchMemo;
    public AOWLabel ResearchCost;
    public AOWImage ResearchIcn;
    public AOWLabel ResearchStatus;
    public AOWLabel ResearchNone;
    public AOWPanel ResearchPnl;
    public AOWLabel ResearchLbl;
    public AOWPanel RightInfoPage;
    public AOWLabel ManaInfo;
    public AOWImage Sep;
    public AOWLabel ResearchPointsLbl;
    public AOWLabel ResearchManaPoolLbl;
    public AOWLabel RManaInfo;
    public AOWImage AOWImage1;
    public AOWLabel ResearchBookLbl;
    public AOWButton CancelResearchBtn;
    public AOWButton CancelCastingBtn;
    public IvTranslator Translator;
    public byte FMode;
    // world casting, combat casting, research, researched
    public byte FPageKind;
    // information or spell list
    public SpellType FSpellType;
    public SpellList FSpells;
    public int FPage;
    public Hero FHero;
    public ActivateAbilityInfo FActivation;
    public int FResearchedSpell;
    public ExecuteDoneEvent FDone;
    public PlayerMagicEventLog FEvent;
    public DoubleItemList FPlayerPages;

    // AoWSpellBook.pas:393
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        PlayerMagicEventLog Event;
        AudioPlayback.PlaySample("SFX\\INT\\BOOKCLSE.WAV");
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

        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.General.Manager.FDrawOverlay = true;
        DialogHosts.MagicWin.Dlg.FDrawn = false;
    }

    // AoWSpellBook.pas:405
    public bool ValidateHero()
    {
        bool Result;
        if (FActivation != null)
        {
            FHero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FActivation.FUnitID);
            Result = FHero != null && !FHero.FUnitStatus.Contains(UnitStatusElement.Killed);
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
        Spell Spell;
        if (ValidateHero())
        {
            switch (FMode)
            {
                case 2:
                case 3:
                    FSpells.Clear();
                    AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.ListResearchSpells(ref FSpells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                    FSpells.SortOnLevel();
                    var spellType = AbilityRules.TranslateRStr(GameTables.TSpellTypeRStr[(int)FSpellType]);
                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(ResearchXspellRStr), [spellType]);
                    LeftHeader.Text = format;
                    var spellTypeText = AbilityRules.TranslateRStr(GameTables.TSpellTypeRStr[(int)FSpellType]);
                    var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(ResearchXspellRStr), [spellTypeText]);
                    RightHeader.Text = aoWEFormat;
                    // { Original overwrites the type-specific headers. }
                    var researchSpell = AbilityRules.TranslateRStr(ResearchSpellRStr);
                    LeftHeader.Text = researchSpell;
                    var researchSpellText = AbilityRules.TranslateRStr(ResearchSpellRStr);
                    RightHeader.Text = researchSpellText;
                    break;
                case 0:
                    FSpells.Clear();
                    AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, Set<SpellType>.Of(FSpellType));
                    FSpells.SortOnLevel();
                    var spellType3 = AbilityRules.TranslateRStr(GameTables.TSpellTypeRStr[(int)FSpellType]);
                    var format2 = AbilityRules.Format(AbilityRules.TranslateRStr(CastXSpellRStr), [spellType3]);
                    LeftHeader.Text = format2;
                    var spellType4 = AbilityRules.TranslateRStr(GameTables.TSpellTypeRStr[(int)FSpellType]);
                    var format3 = AbilityRules.Format(AbilityRules.TranslateRStr(CastXSpellRStr), [spellType4]);
                    RightHeader.Text = format3;
                    break;
                case 1:
                    FSpells.Clear();
                    AoWHSMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, Set<SpellType>.Of(FSpellType));
                    FSpells.SortOnLevel();
                    var spellType5 = AbilityRules.TranslateRStr(GameTables.TSpellTypeRStr[(int)FSpellType]);
                    var format4 = AbilityRules.Format(AbilityRules.TranslateRStr(CastXSpellRStr), [spellType5]);
                    LeftHeader.Text = format4;
                    var spellType6 = AbilityRules.TranslateRStr(GameTables.TSpellTypeRStr[(int)FSpellType]);
                    var format5 = AbilityRules.Format(AbilityRules.TranslateRStr(CastXSpellRStr), [spellType6]);
                    RightHeader.Text = format5;
                    break;
            }

            for (int I = FSpells.GetCount() - 1; I >= 0; I--)
            {
                bool condition = FSpells.GetSpell(I).FSpellID >= 100;
                if (condition)
                {
                    var spell = FSpells.GetSpell(I);
                    condition = TacticalTables.SpellTypes[spell.FSpellID] == TfxType.None;
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
                FPlayerPages.Count = AoWHSMap.Current.FSeatedPlayer + 1;
                FPlayerPages.List[AoWHSMap.Current.FSeatedPlayer].A = (int)FSpellType;
                FPlayerPages.List[AoWHSMap.Current.FSeatedPlayer].B = FPage;
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
        Leader Leader;
        AOWButton Button;
        PlayerMagicControl Control;
        Spell Spell;
        if (ValidateHero())
        {
            if (FMode == 1)
            {
                Player = AoWCombatMap.Current.FSeatedPlayer;
            }
            else
            {
                Player = AoWHSMap.Current.FSeatedPlayer;
            }

            Leader = AoWHSMap.Current.FPlayers.GetPlayers(Player).FLeader;
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
            var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            var manaPool = AbilityRules.TranslateRStr(ManaPoolRStr);
            ManaPoolLbl.Text = manaPool + ": " + IntToStr(player.FMagicControl.FMana);
            var castingPoints = FHero.GetCastingPoints();
            var castingPointsText = AbilityRules.TranslateRStr(GameText.CastingPointsRStr);
            var castingPointsMax = FHero.GetCastingPointsMax();
            CastingPointsLbl.Text = castingPointsText + ": " + IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
            ResearchManaPoolLbl.Text = ManaPoolLbl.Text;
            var netPowerToResearch = AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.GetNetPowerToResearch();
            var researchPoints = AbilityRules.TranslateRStr(ResearchPointsRStr);
            ResearchPointsLbl.Text = researchPoints + ": " + IntToStr(netPowerToResearch);
            if (Set<byte>.WithBounds(0, 255).With(2).With(3).Contains(FMode))
            {
                CurrentPointsLbl.Text = "";
                CastingPointsMaxLbl.Text = "";
                SpellBookLbl.Visible = false;
                ResearchBookLbl.Visible = true;
                CastingPage.Visible = false;
                Control = AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl;
                if (Control.FResearchSpellID != 0)
                {
                    ResearchPnl.Visible = true;
                    ResearchNone.Visible = false;
                    Spell = AoWHSSet.Current.FSpellControl.GetSpell(Control.FResearchSpellID);
                    var currentlyResearching = AbilityRules.TranslateRStr(CurrentlyResearchingRStr);
                    ResearchLbl.Text = currentlyResearching + ":";
                    ResearchMemo.Items = Spell.FSpellText;
                    ResearchName.Text = Spell.FSpellName;
                    var magicSphere = AbilityRules.TranslateRStr(GameTables.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                    var level = AbilityRules.TranslateRStr(GameText.LevelRStr);
                    ResearchClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                    var castingMana = FHero.CastingMana(Spell);
                    var castingCost = AbilityRules.TranslateRStr(GameText.CastingCostRStr);
                    var mana = AbilityRules.TranslateRStr(GameText.ManaRStr);
                    ResearchCost.Text = castingCost + ": " + IntToStr(castingMana) + " " + mana;
                    if (Control.GetResearchTurnsLeft() < 999)
                    {
                        var fmtTurnsLeftStr = DurationText.FmtTurnsLeftStr(Control.GetResearchTurnsLeft());
                        ResearchStatus.Text = fmtTurnsLeftStr;
                    }
                    else
                    {
                        var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XTurnsRStr), [(byte)63]);
                        ResearchStatus.Text = format;
                    }

                    var cancel = AbilityRules.TranslateRStr(GameText.CancelRStr);
                    CancelResearchBtn.Text = cancel;
                    ResearchStatus.Visible = true;
                }
                else
                {
                    if (FMode == 3)
                    {
                        ResearchPnl.Visible = true;
                        ResearchNone.Visible = false;
                        Spell = AoWHSSet.Current.FSpellControl.GetSpell(FResearchedSpell);
                        var researched = AbilityRules.TranslateRStr(ResearchedRStr);
                        ResearchLbl.Text = researched + ":";
                        ResearchMemo.Items = Spell.FSpellText;
                        ResearchName.Text = Spell.FSpellName;
                        var magicSphere = AbilityRules.TranslateRStr(GameTables.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                        var level = AbilityRules.TranslateRStr(GameText.LevelRStr);
                        ResearchClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                        var castingCost = AbilityRules.TranslateRStr(GameText.CastingCostRStr);
                        var mana = AbilityRules.TranslateRStr(GameText.ManaRStr);
                        ResearchCost.Text = castingCost + ": " + IntToStr(Spell.FBaseManaCost) + " " + mana;
                        ResearchStatus.Visible = false;
                        var research = AbilityRules.TranslateRStr(ResearchRStr);
                        CancelResearchBtn.Text = research;
                        var researchNewSpell = AbilityRules.TranslateRStr(ResearchNewSpellRStr);
                        CancelResearchBtn.ToolTipText.Text = researchNewSpell;
                    }
                    else
                    {
                        ResearchPnl.Visible = false;
                        ResearchNone.Visible = true;
                        var research = AbilityRules.TranslateRStr(ResearchRStr);
                        CancelResearchBtn.Text = research;
                        var cancelSpellResearch = AbilityRules.TranslateRStr(CancelSpellResearchRStr);
                        CancelResearchBtn.ToolTipText.Text = cancelSpellResearch;
                    }
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode))
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
                        Spell = AoWHSSet.Current.FSpellControl.GetSpell(FHero.FCastingSpellID);
                        CastingSpell.Text = Spell.FSpellName;
                        var magicSphere = AbilityRules.TranslateRStr(GameTables.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                        var level = AbilityRules.TranslateRStr(GameText.LevelRStr);
                        CastingClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                        var castingCost = AbilityRules.TranslateRStr(GameText.CastingCostRStr);
                        var mana = AbilityRules.TranslateRStr(GameText.ManaRStr);
                        CastingCost.Text = castingCost + ": " + IntToStr(FHero.FCastingProgress) + " " + mana;
                        CastingMemo.Items = Spell.FSpellText;
                        var fmtTurnsLeftStr = DurationText.FmtTurnsLeftStr(FHero.CastingTurnsLeft());
                        CastingStatus.Text = "(" + fmtTurnsLeftStr + ")";
                        CastingPnl.Visible = true;
                        if (FHero.CastingReady())
                        {
                            CastingBtn.Visible = true;
                            var castingReady = AbilityRules.TranslateRStr(CastingReadyRStr);
                            CastingStatus.Text = castingReady;
                        }
                        else
                        {
                            CastingBtn.Visible = false;
                            var fmtTurnsLeftStr2 = DurationText.FmtTurnsLeftStr(FHero.CastingTurnsLeft());
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
    public void ShowSpellEntry(Spell Spell, AOWPanel Panel, AOWButton Button, AOWLabel Name, AOWLabel SpellClass, AOWLabel Cost, AOWMemo Memo, AOWImage Icon)
    {
        string Reason = "";
        int Turns;
        PlayerMagicControl Control;
        if (Spell != null)
        {
            Panel.Visible = true;
            Name.Text = Spell.FSpellName;
            SpellClass.Text = AbilityRules.TranslateRStr(GameTables.TMagicSphereRStr[(int)Spell.FMagicSphere]) + ", " + AbilityRules.TranslateRStr(GameText.LevelRStr) + " " + IntToStr(Spell.FSpellLevel);
            if (FMode == 2)
            {
                if (Spell.FSpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global))
                {
                    var global = AbilityRules.TranslateRStr(GameTables.GlobalRStr);
                    SpellClass.Text = SpellClass.Text + ", " + global;
                }
                else
                {
                    if (Spell.FSpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Combat))
                    {
                        var combat = AbilityRules.TranslateRStr(GameTables.CombatRStr);
                        SpellClass.Text = SpellClass.Text + ", " + combat;
                    }
                    else
                    {
                        if (Spell.FSpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat))
                        {
                            var global = AbilityRules.TranslateRStr(GameTables.GlobalRStr);
                            var combat = AbilityRules.TranslateRStr(GameTables.CombatRStr);
                            SpellClass.Text = SpellClass.Text + ", " + global + "/" + combat;
                        }
                    }
                }
            }

            Memo.Items = Spell.FSpellText;
            if (FHero == null)
            {
                Cost.Text = AbilityRules.Format(AbilityRules.TranslateRStr(CostXRStr), [Spell.FBaseManaCost]);
            }
            else
            {
                var castingMana = FHero.CastingMana(Spell);
                Cost.Text = AbilityRules.Format(AbilityRules.TranslateRStr(CostXRStr), [castingMana]);
            }

            if (FMode == 0 && (!Spell.FSpellControlTypes.Contains(SpellControlTypeElement.Global) || FHero.Casting()) || FMode == 1 && (!Spell.FSpellControlTypes.Contains(SpellControlTypeElement.Combat) || FHero.Casting() || !FHero.CanCastSpellInstantly(Spell.FSpellID, Spell.FBaseManaCost, ref Reason)) || FMode == 2 && AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.FResearchSpellID != 0)
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
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.UpkeepXRStr), [Spell.FManaUpkeep]);
                Cost.Text = Cost.Text + " " + format;
            }

            if (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode))
            {
                Turns = FHero.CastingTurns(Spell);
                if (Turns == 0)
                {
                    Reason = AbilityRules.TranslateRStr(InstantRStr);
                }
                else
                {
                    Reason = AbilityRules.Format(AbilityRules.TranslateRStr(TurnsXRStr), [IntToStr(Turns)]);
                }

                Cost.Text = Cost.Text + " " + Reason;
            }
            else
            {
                if (FMode == 2 || FMode == 3)
                {
                    Control = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl;
                    Turns = Control.ResearchTurns(Control.GetNetPowerToResearch(), Spell.FResearchCost);
                    if (Turns < 999)
                    {
                        var format = AbilityRules.Format(AbilityRules.TranslateRStr(TurnsXRStr), [IntToStr(Turns)]);
                        Cost.Text = Cost.Text + " " + format;
                    }
                    else
                    {
                        var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XTurnsRStr), [(byte)63]);
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
    public void SelectSpell(Spell Spell)
    {
        string Reason = "";
        if (ValidateHero())
        {
            switch (FMode)
            {
                case 2:
                case 3:
                    if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.ResearchSpell(Spell.FSpellID))
                    {
                        if (FEvent == null || FEvent.FMode != PlayerMagicEventLog.PlayerMagicEventMode.StartResearch)
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
                    if (FHero.GetCastingPoints() >= Spell.FBaseManaCost || AoWHSMap.Current.FPlayers.GetPlayers(FHero.FPlayer).FMagicControl.GetCheatEnabled())
                    {
                        if (FHero.CastSpell(Spell.FSpellID, ref Reason))
                        {
                            Dlg.Visible = false;
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
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
                            AoWHSMap.Current.ShowWarning(Reason);
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
                        AoWCombatMap.Current.ShowWarning(Reason);
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
        if (Dlg.Visible && Player == AoWHSMap.Current.FSeatedPlayer)
        {
            UpdateSpellPages();
            UpdateInfoPages();
        }
    }

    // AoWSpellBook.pas:421
    public void UnitChanged(AbstractUnit U)
    {
        if (Dlg.Visible && (U == null || (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(FMode) && U == FHero)))
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
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.FResearchSpellID != 0)
        {
            SetPageKind(0);
        }
        else
        {
            SetPageKind(1);
        }

        FMode = 2;
        FSpellType = SpellType.Global;
        FPage = 0;
        FHero = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FLeader;
        UpdateSpellPages();
        UpdateInfoPages();
        ShowBook();
    }

    // AoWSpellBook.pas:501
    public void ShowResearchEvent(PlayerMagicEventLog Event, ExecuteDoneEvent Done)
    {
        FEvent = Event;
        FEvent.AddRef();
        FDone = Done;
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.FResearchSpellID == 0)
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
            FSpellType = SpellType.Global;
            FPage = 0;
            FHero = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FLeader;
            UpdateSpellPages();
            UpdateInfoPages();
            ShowBook();
            AoWHSMap.Current.FTutorialControl.ShowMessage(51, true, true);
        }
        else
        {
            ShowResearch();
        }
    }

    // AoWSpellBook.pas:518
    public void ShowResearchedEvent(PlayerMagicEventLog Event, ExecuteDoneEvent Done)
    {
        FEvent = Event;
        FEvent.AddRef();
        FDone = Done;
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.FResearchSpellID == 0)
        {
            Reset();
            FResearchedSpell = Event.FSpellID;
            SetPageKind(0);
            FMode = 3;
            FSpellType = SpellType.Global;
            FPage = 0;
            FHero = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FLeader;
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
    public void ShowCasting(ActivateAbilityInfo Activation)
    {
        Reset();
        FActivation = Activation;
        FActivation.AddRef();
        FMode = 0;
        if (ValidateHero())
        {
            FSpellType = (SpellType)FPlayerPages[AoWHSMap.Current.FSeatedPlayer].A;
            FPage = FPlayerPages[AoWHSMap.Current.FSeatedPlayer].B;
            if (FHero.Casting())
            {
                SetPageKind(0);
            }
            else
            {
                FSpells.Clear();
                AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, Set<SpellType>.Of(FSpellType));
                if (FSpells.GetCount() == 0 || FSpells.GetCount() <= FPage * 8)
                {
                    FSpells.Clear();
                    FSpellType = (SpellType)3;
                    while (FSpells.GetCount() == 0 && (int)FSpellType != 0)
                    {
                        FSpellType = (SpellType)((int)FSpellType - 1);
                        AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, Set<SpellType>.Of(FSpellType));
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
    public void ShowCombatCasting(ActivateAbilityInfo Activation)
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
                FSpellType = (SpellType)3;
                while (FSpells.GetCount() == 0 && (int)FSpellType != 0)
                {
                    FSpellType = (SpellType)((int)FSpellType - 1);
                    AoWHSMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FSeatedPlayer).FMagicControl.ListSpells(ref FSpells, Set<SpellType>.Of(FSpellType));
                }

                SetPageKind(1);
            }

            FSpellType = SpellType.Combat;
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
        DoubleItemList.DoubleItem Defaults = default;
        FSpells = new SpellList();
        Defaults.A = 2;
        Defaults.B = 0;
        FPlayerPages = new DoubleItemList();
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
        if (AoWHSMap.Current.FCombatMap != null && DialogHosts.General.FCombatUnit != null)
        {
            DialogHosts.General.FCombatUnit.SelectAbility(-1);
        }

        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWSpellBook.pas:222
    public void NextBtnClick()
    {
        FPage += 1;
        UpdateSpellPages();
        Dlg.StartAnim(PageTurnMode.Left);
        Dlg.FPageTurn.FSpeed = 25;
        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:229
    public void PrevBtnClick()
    {
        FPage -= 1;
        UpdateSpellPages();
        Dlg.StartAnim(PageTurnMode.Right);
        Dlg.FPageTurn.FSpeed = 25;
        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:236
    public void SpellIconDraw(AOWImage Sender, SurfaceDesc SD, int Slot)
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
        if (FSpellType != SpellType.Global || FPage != 0 || FPageKind != 1)
        {
            if (FPageKind == 0)
            {
                Dlg.StartAnim(PageTurnMode.Left);
            }
            else
            {
                Dlg.StartAnim(PageTurnMode.Right);
            }

            AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            FSpellType = SpellType.Global;
            FPage = 0;
            UpdateSpellPages();
            SetPageKind(1);
        }
    }

    // AoWSpellBook.pas:264
    public void UnitBtnClick()
    {
        if (FSpellType != SpellType.Unit || FPage != 0 || FPageKind != 1)
        {
            if (FPageKind == 0 || FSpellType == SpellType.Global)
            {
                Dlg.StartAnim(PageTurnMode.Left);
            }
            else
            {
                Dlg.StartAnim(PageTurnMode.Right);
            }

            AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            FSpellType = SpellType.Unit;
            FPage = 0;
            UpdateSpellPages();
            SetPageKind(1);
        }
    }

    // AoWSpellBook.pas:275
    public void CombatBtnClick()
    {
        if (FSpellType != SpellType.Combat || FPage != 0 || FPageKind != 1)
        {
            Dlg.StartAnim(PageTurnMode.Left);
            AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            FSpellType = SpellType.Combat;
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
            Dlg.StartAnim(PageTurnMode.Right);
            AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            SetPageKind(0);
        }
    }

    // AoWSpellBook.pas:295
    public void CastingBtnClick()
    {
        string Reason = "";
        DialogHosts.General.PlayButtonSound(0);
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
                        AoWHSMap.Current.ShowWarning(Reason);
                    }

                    break;
                case 1:
                    SelectSpell(AoWHSSet.Current.FSpellControl.GetSpell(FHero.FCastingSpellID));
                    break;
            }
        }
    }

    // AoWSpellBook.pas:309
    public void CancelCastingBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (ValidateHero())
        {
            FHero.CancelCasting();
        }
    }

    // AoWSpellBook.pas:315
    public void FaceDraw(AOWImage Sender, SurfaceDesc SD)
    {
        Rect R;
        R = new Rect(LeftInfoPage.WinLeft + Sender.WinLeft, LeftInfoPage.WinTop + Sender.WinTop, LeftInfoPage.WinLeft + Sender.WinLeft + Sender.WinWidth, LeftInfoPage.WinTop + Sender.WinTop + Sender.WinHeight);

        FHero.GetFace().ShowStretched(R, SD);
    }

    // AoWSpellBook.pas:325
    public void ResearchIcnDraw(AOWControl Sender, SurfaceDesc SD)
    {
        Spell Spell;
        int X;
        var player = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
        Spell = AoWHSSet.Current.FSpellControl.GetSpell(player.FMagicControl.FResearchSpellID);
        if (Spell == null && FMode == 3)
        {
            Spell = AoWHSSet.Current.FSpellControl.GetSpell(FResearchedSpell);
        }

        if (Spell != null)
        {
            X = Sender.FAOWWindow.WinLeft;
            Spell.FCastImages.Show(10, Sender.WinLeft + X, Sender.FAOWWindow.WinTop + Sender.WinTop, 0, SD);
        }
    }

    // AoWSpellBook.pas:337
    public void CastingIcnDraw(AOWControl Sender, SurfaceDesc SD)
    {
        Spell Spell;
        int X;
        if (FHero != null)
        {
            if (FHero.Casting())
            {
                Spell = AoWHSSet.Current.FSpellControl.GetSpell(FHero.FCastingSpellID);
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
        PlayerMagicControl Control;
        DialogHosts.General.PlayButtonSound(0);
        if (ValidateHero())
        {
            Control = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl;
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
        DialogHosts.General.PlayButtonSound(0);
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
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            if (AoWHSMap.Current.FCombatMap != null && DialogHosts.General.FCombatUnit != null)
            {
                DialogHosts.General.FCombatUnit.SelectAbility(-1);
            }

            Dlg.Visible = false;
        }
    }

    // AoWSpellBook.pas:385. The host surfaces reside in system memory.
    public void ShowBook()
    {
        DialogHosts.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }

    private static readonly string CancelSpellResearchRStr = "Cancels current spell research";
    private static readonly string CastXSpellRStr = "Cast %s Spell";
    private static readonly string CastingReadyRStr = "Casting Ready";
    private static readonly string CostXRStr = "Cost: %d";
    private static readonly string CurrentlyResearchingRStr = "Currently Researching";
    private static readonly string InstantRStr = "Instant";
    private static readonly string ManaPoolRStr = "Mana Pool";
    private static readonly string ResearchNewSpellRStr = "Research new spell";
    private static readonly string ResearchPointsRStr = "Research Points";
    private static readonly string ResearchRStr = "Research";
    private static readonly string ResearchSpellRStr = "Research Spell";
    private static readonly string ResearchXspellRStr = "Research %s Spell";
    private static readonly string ResearchedRStr = "Researched";
    private static readonly string TurnsXRStr = "Turns: %s";
}
