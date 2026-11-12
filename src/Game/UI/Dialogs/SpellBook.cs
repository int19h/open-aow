// Translated from AoWSpellBook.pas.
using Aow.Engine.Collections;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// mode, page kind, spell type, spells, page, hero, activation, researched spell, callback,
// event, saved player pages
public partial class SpellBook : DataModule
{
    public SpellBook(Component owner) : base(owner)
    { }

    public Label LeftHeader;
    public Panel LeftSpellPage;
    public Panel S1Pnl;
    public Label S1Spell;
    public Label S1Class;
    public Memo S1Memo;
    public Label S1Cost;
    public Image S1Icn;
    public Panel S2Pnl;
    public Label S2Spell;
    public Label S2Class;
    public Memo S2Memo;
    public Label S2Cost;
    public Image S2Icn;
    public Panel S3Pnl;
    public Label S3Spell;
    public Label S3Class;
    public Memo S3Memo;
    public Label S3Cost;
    public Image S3Icn;
    public Panel S4Pnl;
    public Label S4Spell;
    public Label S4Class;
    public Memo S4Memo;
    public Label S4Cost;
    public Image S4Icn;
    public Label RightHeader;
    public Panel RightSpellPage;
    public Panel S5Pnl;
    public Label S5Spell;
    public Label S5Class;
    public Memo S5Memo;
    public Label S5Cost;
    public Image S5Icn;
    public Panel S6Pnl;
    public Label S6Spell;
    public Label S6Class;
    public Memo S6Memo;
    public Label S6Cost;
    public Image S6Icn;
    public Panel S7Pnl;
    public Label S7Spell;
    public Label S7Class;
    public Memo S7Memo;
    public Label S7Cost;
    public Image S7Icn;
    public Panel S8Pnl;
    public Label S8Spell;
    public Label S8Class;
    public Memo S8Memo;
    public Label S8Cost;
    public Image S8Icn;
    public Button S1Btn;
    public Label CurrentPointsLbl;
    public Label Line;
    public Label CastingPointsMaxLbl;
    public Button GlobalBtn;
    public Button UnitBtn;
    public Button CombatBtn;
    public Button InfoBtn;
    public Button PrevBtn;
    public Button NextBtn;
    public Button S2Btn;
    public Button S3Btn;
    public Button S4Btn;
    public Button S5btn;
    public Button S6Btn;
    public Button S7Btn;
    public Button S8Btn;
    public Panel CastingPage;
    public Panel CastingPnl;
    public Label CastingSpell;
    public Label CastingClass;
    public Memo CastingMemo;
    public Label CastingCost;
    public Panel LeftInfoPage;
    public Label CastingStatus;
    public Label CastingPointsLbl;
    public Button CastingBtn;
    public Button Sel4;
    public Button Sel3;
    public Button Sel2;
    public Button Sel1;
    public Button Sel5;
    public Button Sel6;
    public Button Sel7;
    public Label SpellBookLbl;
    public Label NameLbl;
    public Label ManaPoolLbl;
    public BookWindow Dlg;
    public Label NoneCastingLbl;
    public Panel ResearchPage;
    public Label ResearchName;
    public Label ResearchClass;
    public Memo ResearchMemo;
    public Label ResearchCost;
    public Label ResearchStatus;
    public Label ResearchNone;
    public Panel ResearchPnl;
    public Label ResearchLbl;
    public Panel RightInfoPage;
    public Label ResearchPointsLbl;
    public Label ResearchManaPoolLbl;
    public Label ResearchBookLbl;
    public Button CancelResearchBtn;

    public byte Mode { get; set; }
    // world casting, combat casting, research, researched
    public byte PageKind { get; set; }

    // information or spell list
    private SpellType _spellType;
    private SpellList _spells;

    public int Page { get; set; }

    private Hero _hero;
    private AbilityActivation _activation;
    private int _researchedSpell;
    private ExecuteDoneEvent _done;
    private PlayerMagicEventLog _event;
    private DoubleItemList _playerPages;

    // AoWSpellBook.pas:393
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        PlayerMagicEventLog eventValue;
        AudioPlayback.PlaySample("SFX\\INT\\BOOKCLSE.WAV");
        done = _done;
        eventValue = _event;
        _done = null;
        _event = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }

        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.General.Manager.DrawOverlay = true;
        DialogHosts.MagicWindow.Dlg.Drawn = false;
    }

    // AoWSpellBook.pas:405
    public bool ValidateHero()
    {
        bool result;
        if (_activation != null)
        {
            _hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(_activation.UnitId);
            result = _hero != null && !_hero.UnitStatus.Contains(UnitStatusElement.Killed);
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AoWSpellBook.pas:427
    public void Reset()
    {
        if (_activation != null)
        {
            _activation.Release();
            _activation = null;
        }

        _spells.Clear();
        Page = 0;
        _hero = null;
        ResearchPage.Visible = true;
        CastingPage.Visible = true;
        GlobalBtn.Visible = true;
        GlobalBtn.ImageLibraryIndexDown = 17;
        GlobalBtn.ImageLibraryIndexLit = 18;
        GlobalBtn.ImageLibraryIndexUp = 16;
        CombatBtn.Visible = true;
        UnitBtn.Visible = true;
        InfoBtn.Visible = true;
        CurrentPointsLbl.Visible = true;
        CastingPointsMaxLbl.Visible = true;
        Line.Visible = true;
    }

    // AoWSpellBook.pas:439
    public void SetPageKind(byte kind)
    {
        PageKind = kind;
        LeftSpellPage.Visible = kind == 1;
        RightSpellPage.Visible = kind == 1;
        LeftInfoPage.Visible = kind == 0;
        RightInfoPage.Visible = kind == 0;
        switch (kind)
        {
            case 1:
                var count = _spells.GetCount();
                NextBtn.Visible = count - (Page + 1) * 8 > 0;
                PrevBtn.Visible = Page != 0;
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
            switch (Mode)
            {
                case 2:
                case 3:
                    _spells.Clear();
                    AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ListResearchSpells(ref _spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                    _spells.SortOnLevel();
                    var spellType = AbilityRules.TranslateResourceString(GameTables.s_spellTypeTexts[(int)_spellType]);
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_researchSpellFormat), [spellType]);
                    LeftHeader.Text = format;
                    var spellTypeText = AbilityRules.TranslateResourceString(GameTables.s_spellTypeTexts[(int)_spellType]);
                    var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(s_researchSpellFormat), [spellTypeText]);
                    RightHeader.Text = formattedText;
                    // { Original overwrites the type-specific headers. }
                    var researchSpell = AbilityRules.TranslateResourceString(s_researchSpellText);
                    LeftHeader.Text = researchSpell;
                    var researchSpellText = AbilityRules.TranslateResourceString(s_researchSpellText);
                    RightHeader.Text = researchSpellText;
                    break;
                case 0:
                    _spells.Clear();
                    AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
                    _spells.SortOnLevel();
                    var spellType3 = AbilityRules.TranslateResourceString(GameTables.s_spellTypeTexts[(int)_spellType]);
                    var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castSpellFormat), [spellType3]);
                    LeftHeader.Text = format2;
                    var spellType4 = AbilityRules.TranslateResourceString(GameTables.s_spellTypeTexts[(int)_spellType]);
                    var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castSpellFormat), [spellType4]);
                    RightHeader.Text = format3;
                    break;
                case 1:
                    _spells.Clear();
                    AoWHSMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
                    _spells.SortOnLevel();
                    var spellType5 = AbilityRules.TranslateResourceString(GameTables.s_spellTypeTexts[(int)_spellType]);
                    var format4 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castSpellFormat), [spellType5]);
                    LeftHeader.Text = format4;
                    var spellType6 = AbilityRules.TranslateResourceString(GameTables.s_spellTypeTexts[(int)_spellType]);
                    var format5 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castSpellFormat), [spellType6]);
                    RightHeader.Text = format5;
                    break;
            }

            for (int I = _spells.GetCount() - 1; I >= 0; I--)
            {
                bool condition = _spells.GetSpell(I).SpellId >= 100;
                if (condition)
                {
                    var spell = _spells.GetSpell(I);
                    condition = TacticalTables.SpellTypes[spell.SpellId] == BattleEffectType.None;
                }

                if (condition)
                {
                    _spells.Delete(I);
                }
            }

            Spell = _spells.GetSpell(Page * 8);
            ShowSpellEntry(Spell, S1Pnl, S1Btn, S1Spell, S1Class, S1Cost, S1Memo, S1Icn);
            Spell = _spells.GetSpell(Page * 8 + 1);
            ShowSpellEntry(Spell, S2Pnl, S2Btn, S2Spell, S2Class, S2Cost, S2Memo, S2Icn);
            Spell = _spells.GetSpell(Page * 8 + 2);
            ShowSpellEntry(Spell, S3Pnl, S3Btn, S3Spell, S3Class, S3Cost, S3Memo, S3Icn);
            Spell = _spells.GetSpell(Page * 8 + 3);
            ShowSpellEntry(Spell, S4Pnl, S4Btn, S4Spell, S4Class, S4Cost, S4Memo, S4Icn);
            Spell = _spells.GetSpell(Page * 8 + 4);
            ShowSpellEntry(Spell, S5Pnl, S5btn, S5Spell, S5Class, S5Cost, S5Memo, S5Icn);
            Spell = _spells.GetSpell(Page * 8 + 5);
            ShowSpellEntry(Spell, S6Pnl, S6Btn, S6Spell, S6Class, S6Cost, S6Memo, S6Icn);
            Spell = _spells.GetSpell(Page * 8 + 6);
            ShowSpellEntry(Spell, S7Pnl, S7Btn, S7Spell, S7Class, S7Cost, S7Memo, S7Icn);
            Spell = _spells.GetSpell(Page * 8 + 7);
            ShowSpellEntry(Spell, S8Pnl, S8Btn, S8Spell, S8Class, S8Cost, S8Memo, S8Icn);
            if (Mode == 0)
            {
                _playerPages.Count = AoWHSMap.Current.SeatedPlayer + 1;
                _playerPages.List[AoWHSMap.Current.SeatedPlayer].First = (int)_spellType;
                _playerPages.List[AoWHSMap.Current.SeatedPlayer].Second = Page;
            }

            if (PageKind == 1)
            {
                var count = _spells.GetCount();
                NextBtn.Visible = count - (Page + 1) * 8 > 0;
                PrevBtn.Visible = Page != 0;
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
        Leader leader;
        Button button;
        PlayerMagicControl control;
        Spell spell;
        if (ValidateHero())
        {
            if (Mode == 1)
            {
                Player = CombatMap.Current.SeatedPlayer;
            }
            else
            {
                Player = AoWHSMap.Current.SeatedPlayer;
            }

            leader = AoWHSMap.Current.Players.GetPlayers(Player).Leader;
            for (int I = 0; I <= 6; I++)
            {
                switch (I)
                {
                    case 0:
                        button = Sel1;
                        break;
                    case 1:
                        button = Sel2;
                        break;
                    case 2:
                        button = Sel3;
                        break;
                    case 3:
                        button = Sel4;
                        break;
                    case 4:
                        button = Sel5;
                        break;
                    case 5:
                        button = Sel6;
                        break;
                    case 6:
                        button = Sel7;
                        break;
                    default:
                        button = null;
                        break;
                }

                if (I < leader.GetSphereCount())
                {
                    button.Visible = true;
                    switch ((int)leader.GetSpheres(I) & 127)
                    {
                        case 1:
                            button.ImageLibraryIndexDown = 28;
                            button.ImageLibraryIndexLit = 29;
                            button.ImageLibraryIndexUp = 27;
                            break;
                        case 5:
                            button.ImageLibraryIndexDown = 43;
                            button.ImageLibraryIndexLit = 44;
                            button.ImageLibraryIndexUp = 42;
                            break;
                        case 3:
                            button.ImageLibraryIndexDown = 34;
                            button.ImageLibraryIndexLit = 35;
                            button.ImageLibraryIndexUp = 33;
                            break;
                        case 4:
                            button.ImageLibraryIndexDown = 37;
                            button.ImageLibraryIndexLit = 38;
                            button.ImageLibraryIndexUp = 36;
                            break;
                        case 6:
                            button.ImageLibraryIndexDown = 40;
                            button.ImageLibraryIndexLit = 41;
                            button.ImageLibraryIndexUp = 39;
                            break;
                        case 2:
                            button.ImageLibraryIndexDown = 31;
                            button.ImageLibraryIndexLit = 32;
                            button.ImageLibraryIndexUp = 30;
                            break;
                    }
                }
                else
                {
                    button.Visible = false;
                }
            }

            NameLbl.Text = _hero.HeroName;
            var player = AoWHSMap.Current.Players.GetPlayers(Player);
            var manaPool = AbilityRules.TranslateResourceString(s_manaPoolText);
            ManaPoolLbl.Text = manaPool + ": " + IntToStr(player.MagicControl.Mana);
            var castingPoints = _hero.GetCastingPoints();
            var castingPointsText = AbilityRules.TranslateResourceString(GameText.CastingPointsText);
            var castingPointsMax = _hero.GetCastingPointsMax();
            CastingPointsLbl.Text = castingPointsText + ": " + IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
            ResearchManaPoolLbl.Text = ManaPoolLbl.Text;
            var netPowerToResearch = AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetNetPowerToResearch();
            var researchPoints = AbilityRules.TranslateResourceString(s_researchPointsText);
            ResearchPointsLbl.Text = researchPoints + ": " + IntToStr(netPowerToResearch);
            if (Set<byte>.WithBounds(0, 255).With(2).With(3).Contains(Mode))
            {
                CurrentPointsLbl.Text = "";
                CastingPointsMaxLbl.Text = "";
                SpellBookLbl.Visible = false;
                ResearchBookLbl.Visible = true;
                CastingPage.Visible = false;
                control = AoWHSMap.Current.Players.GetPlayers(Player).MagicControl;
                if (control.ResearchSpellId != 0)
                {
                    ResearchPnl.Visible = true;
                    ResearchNone.Visible = false;
                    spell = AoWHSSet.Current.SpellControl.GetSpell(control.ResearchSpellId);
                    var currentlyResearching = AbilityRules.TranslateResourceString(s_currentlyResearchingText);
                    ResearchLbl.Text = currentlyResearching + ":";
                    ResearchMemo.Items = spell.SpellText;
                    ResearchName.Text = spell.SpellName;
                    var magicSphere = AbilityRules.TranslateResourceString(GameTables.s_magicSphereTexts[(int)spell.MagicSphere]);
                    var level = AbilityRules.TranslateResourceString(GameText.LevelText);
                    ResearchClass.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
                    var castingMana = _hero.CastingMana(spell);
                    var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
                    var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
                    ResearchCost.Text = castingCost + ": " + IntToStr(castingMana) + " " + mana;
                    if (control.GetResearchTurnsLeft() < 999)
                    {
                        var formatTurnsLeftString = DurationText.FormatTurnsLeftString(control.GetResearchTurnsLeft());
                        ResearchStatus.Text = formatTurnsLeftString;
                    }
                    else
                    {
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.TurnsFormat), [(byte)63]);
                        ResearchStatus.Text = format;
                    }

                    var cancel = AbilityRules.TranslateResourceString(GameText.CancelText);
                    CancelResearchBtn.Text = cancel;
                    ResearchStatus.Visible = true;
                }
                else
                {
                    if (Mode == 3)
                    {
                        ResearchPnl.Visible = true;
                        ResearchNone.Visible = false;
                        spell = AoWHSSet.Current.SpellControl.GetSpell(_researchedSpell);
                        var researched = AbilityRules.TranslateResourceString(s_researchedText);
                        ResearchLbl.Text = researched + ":";
                        ResearchMemo.Items = spell.SpellText;
                        ResearchName.Text = spell.SpellName;
                        var magicSphere = AbilityRules.TranslateResourceString(GameTables.s_magicSphereTexts[(int)spell.MagicSphere]);
                        var level = AbilityRules.TranslateResourceString(GameText.LevelText);
                        ResearchClass.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
                        var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
                        var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
                        ResearchCost.Text = castingCost + ": " + IntToStr(spell.BaseManaCost) + " " + mana;
                        ResearchStatus.Visible = false;
                        var research = AbilityRules.TranslateResourceString(s_researchText);
                        CancelResearchBtn.Text = research;
                        var researchNewSpell = AbilityRules.TranslateResourceString(s_researchNewSpellText);
                        CancelResearchBtn.ToolTipText.Text = researchNewSpell;
                    }
                    else
                    {
                        ResearchPnl.Visible = false;
                        ResearchNone.Visible = true;
                        var research = AbilityRules.TranslateResourceString(s_researchText);
                        CancelResearchBtn.Text = research;
                        var cancelSpellResearch = AbilityRules.TranslateResourceString(s_cancelSpellResearchText);
                        CancelResearchBtn.ToolTipText.Text = cancelSpellResearch;
                    }
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(Mode))
                {
                    var fHeroCastingPoints = _hero.GetCastingPoints();
                    CurrentPointsLbl.Text = IntToStr(fHeroCastingPoints);
                    var fHeroCastingPointsMax = _hero.GetCastingPointsMax();
                    CastingPointsMaxLbl.Text = IntToStr(fHeroCastingPointsMax);
                    SpellBookLbl.Visible = true;
                    ResearchBookLbl.Visible = false;
                    ResearchPage.Visible = false;
                    if (_hero.Casting())
                    {
                        NoneCastingLbl.Visible = false;
                        spell = AoWHSSet.Current.SpellControl.GetSpell(_hero.CastingSpellId);
                        CastingSpell.Text = spell.SpellName;
                        var magicSphere = AbilityRules.TranslateResourceString(GameTables.s_magicSphereTexts[(int)spell.MagicSphere]);
                        var level = AbilityRules.TranslateResourceString(GameText.LevelText);
                        CastingClass.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
                        var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
                        var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
                        CastingCost.Text = castingCost + ": " + IntToStr(_hero.CastingProgress) + " " + mana;
                        CastingMemo.Items = spell.SpellText;
                        var formatTurnsLeftString = DurationText.FormatTurnsLeftString(_hero.CastingTurnsLeft());
                        CastingStatus.Text = "(" + formatTurnsLeftString + ")";
                        CastingPnl.Visible = true;
                        if (_hero.CastingReady())
                        {
                            CastingBtn.Visible = true;
                            var castingReady = AbilityRules.TranslateResourceString(s_castingReadyText);
                            CastingStatus.Text = castingReady;
                        }
                        else
                        {
                            CastingBtn.Visible = false;
                            var formatTurnsLeftString2 = DurationText.FormatTurnsLeftString(_hero.CastingTurnsLeft());
                            CastingStatus.Text = "(" + formatTurnsLeftString2 + ")";
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
    public void ShowSpellEntry(Spell spell, Panel panel, Button button, Label name, Label spellClass, Label cost, Memo memo, Image icon)
    {
        string reason = "";
        int turns;
        PlayerMagicControl control;
        if (spell != null)
        {
            panel.Visible = true;
            name.Text = spell.SpellName;
            spellClass.Text = AbilityRules.TranslateResourceString(GameTables.s_magicSphereTexts[(int)spell.MagicSphere]) + ", " + AbilityRules.TranslateResourceString(GameText.LevelText) + " " + IntToStr(spell.SpellLevel);
            if (Mode == 2)
            {
                if (spell.SpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global))
                {
                    var global = AbilityRules.TranslateResourceString(GameTables.GlobalText);
                    spellClass.Text = spellClass.Text + ", " + global;
                }
                else
                {
                    if (spell.SpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Combat))
                    {
                        var combat = AbilityRules.TranslateResourceString(GameTables.CombatText);
                        spellClass.Text = spellClass.Text + ", " + combat;
                    }
                    else
                    {
                        if (spell.SpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat))
                        {
                            var global = AbilityRules.TranslateResourceString(GameTables.GlobalText);
                            var combat = AbilityRules.TranslateResourceString(GameTables.CombatText);
                            spellClass.Text = spellClass.Text + ", " + global + "/" + combat;
                        }
                    }
                }
            }

            memo.Items = spell.SpellText;
            if (_hero == null)
            {
                cost.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_costFormat), [spell.BaseManaCost]);
            }
            else
            {
                var castingMana = _hero.CastingMana(spell);
                cost.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_costFormat), [castingMana]);
            }

            if (Mode == 0 && (!spell.SpellControlTypes.Contains(SpellControlTypeElement.Global) || _hero.Casting()) || Mode == 1 && (!spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat) || _hero.Casting() || !_hero.CanCastSpellInstantly(spell.SpellId, spell.BaseManaCost, ref reason)) || Mode == 2 && AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ResearchSpellId != 0)
            {
                memo.TextColor = 5000268;
                name.TextColor = 5000268;
                spellClass.TextColor = 5000268;
                cost.TextColor = 5000268;
                button.Visible = false;
                icon.Tag = icon.Tag | 16;
            }
            else
            {
                memo.TextColor = 1254460;
                name.TextColor = 1254460;
                spellClass.TextColor = 5908276;
                cost.TextColor = 5908276;
                button.Visible = true;
                icon.Tag = icon.Tag & 15;
            }

            if (spell.ManaUpkeep > 0)
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.UpkeepFormat), [spell.ManaUpkeep]);
                cost.Text = cost.Text + " " + format;
            }

            if (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(Mode))
            {
                turns = _hero.CastingTurns(spell);
                if (turns == 0)
                {
                    reason = AbilityRules.TranslateResourceString(s_instantText);
                }
                else
                {
                    reason = AbilityRules.Format(AbilityRules.TranslateResourceString(s_turnsFormat), [IntToStr(turns)]);
                }

                cost.Text = cost.Text + " " + reason;
            }
            else
            {
                if (Mode == 2 || Mode == 3)
                {
                    control = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl;
                    turns = control.ResearchTurns(control.GetNetPowerToResearch(), spell.ResearchCost);
                    if (turns < 999)
                    {
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_turnsFormat), [IntToStr(turns)]);
                        cost.Text = cost.Text + " " + format;
                    }
                    else
                    {
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.TurnsFormat), [(byte)63]);
                        cost.Text = cost.Text + " " + format;
                    }
                }
            }
        }
        else
        {
            panel.Visible = false;
        }
    }

    // AoWSpellBook.pas:455
    public void SelectSpell(Spell spell)
    {
        string reason = "";
        if (ValidateHero())
        {
            switch (Mode)
            {
                case 2:
                case 3:
                    if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ResearchSpell(spell.SpellId))
                    {
                        if (_event == null || _event.Mode != PlayerMagicEventLog.EventMode.StartResearch)
                        {
                            Mode = 2;
                            InfoBtnClick();
                        }
                        else
                        {
                            Dlg.Visible = false;
                        }
                    }

                    break;
                case 0:
                    if (_hero.GetCastingPoints() >= spell.BaseManaCost || AoWHSMap.Current.Players.GetPlayers(_hero.Player).MagicControl.GetCheatEnabled())
                    {
                        if (_hero.CastSpell(spell.SpellId, ref reason))
                        {
                            Dlg.Visible = false;
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }
                    }
                    else
                    {
                        if (_hero.CastSpell(spell.SpellId, ref reason))
                        {
                            InfoBtnClick();
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }
                    }

                    break;
                case 1:
                    if (_hero.CastCombatSpell(spell.SpellId, ref reason))
                    {
                        Dlg.Visible = false;
                    }
                    else
                    {
                        CombatMap.Current.ShowWarning(reason);
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
    public void PlayerChanged(sbyte player)
    {
        if (Dlg.Visible && player == AoWHSMap.Current.SeatedPlayer)
        {
            UpdateSpellPages();
            UpdateInfoPages();
        }
    }

    // AoWSpellBook.pas:421
    public void UnitChanged(Troop U)
    {
        if (Dlg.Visible && (U == null || (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(Mode) && U == _hero)))
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
        GlobalBtn.ImageLibraryIndexDown = 14;
        GlobalBtn.ImageLibraryIndexLit = 15;
        GlobalBtn.ImageLibraryIndexUp = 13;
        CombatBtn.Visible = false;
        UnitBtn.Visible = false;
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ResearchSpellId != 0)
        {
            SetPageKind(0);
        }
        else
        {
            SetPageKind(1);
        }

        Mode = 2;
        _spellType = SpellType.Global;
        Page = 0;
        _hero = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Leader;
        UpdateSpellPages();
        UpdateInfoPages();
        ShowBook();
    }

    // AoWSpellBook.pas:501
    public void ShowResearchEvent(PlayerMagicEventLog eventValue, ExecuteDoneEvent done)
    {
        _event = eventValue;
        _event.AddRef();
        _done = done;
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ResearchSpellId == 0)
        {
            Reset();
            GlobalBtn.ImageLibraryIndexDown = 14;
            GlobalBtn.ImageLibraryIndexLit = 15;
            GlobalBtn.ImageLibraryIndexUp = 13;
            CombatBtn.Visible = false;
            UnitBtn.Visible = false;
            _researchedSpell = 0;
            SetPageKind(1);
            Mode = 2;
            _spellType = SpellType.Global;
            Page = 0;
            _hero = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Leader;
            UpdateSpellPages();
            UpdateInfoPages();
            ShowBook();
            AoWHSMap.Current.TutorialControl.ShowMessage(51, true, true);
        }
        else
        {
            ShowResearch();
        }
    }

    // AoWSpellBook.pas:518
    public void ShowResearchedEvent(PlayerMagicEventLog eventValue, ExecuteDoneEvent done)
    {
        _event = eventValue;
        _event.AddRef();
        _done = done;
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ResearchSpellId == 0)
        {
            Reset();
            _researchedSpell = eventValue.SpellId;
            SetPageKind(0);
            Mode = 3;
            _spellType = SpellType.Global;
            Page = 0;
            _hero = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Leader;
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
    public void ShowCasting(AbilityActivation activation)
    {
        Reset();
        _activation = activation;
        _activation.AddRef();
        Mode = 0;
        if (ValidateHero())
        {
            _spellType = (SpellType)_playerPages[AoWHSMap.Current.SeatedPlayer].First;
            Page = _playerPages[AoWHSMap.Current.SeatedPlayer].Second;
            if (_hero.Casting())
            {
                SetPageKind(0);
            }
            else
            {
                _spells.Clear();
                AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
                if (_spells.GetCount() == 0 || _spells.GetCount() <= Page * 8)
                {
                    _spells.Clear();
                    _spellType = (SpellType)3;
                    while (_spells.GetCount() == 0 && (int)_spellType != 0)
                    {
                        _spellType = (SpellType)((int)_spellType - 1);
                        AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
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
    public void ShowCombatCasting(AbilityActivation activation)
    {
        Reset();
        _activation = activation;
        _activation.AddRef();
        Mode = 1;
        if (ValidateHero())
        {
            GlobalBtn.Visible = true;
            if (_hero.Casting())
            {
                SetPageKind(0);
            }
            else
            {
                _spells.Clear();
                _spellType = (SpellType)3;
                while (_spells.GetCount() == 0 && (int)_spellType != 0)
                {
                    _spellType = (SpellType)((int)_spellType - 1);
                    AoWHSMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
                }

                SetPageKind(1);
            }

            _spellType = SpellType.Combat;
            Page = 0;
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
        DoubleItemList.DoubleItem defaults = default;
        _spells = new SpellList();
        defaults.First = 2;
        defaults.Second = 0;
        _playerPages = new DoubleItemList();
        _playerPages.Default = defaults;
    }

    // AoWSpellBook.pas:206
    public void SpellBookDestroy()
    {
        if (_activation != null)
        {
            _activation.Release();
            _activation = null;
        }

        _playerPages?.Dispose();
        _spells?.Dispose();
    }

    // AoWSpellBook.pas:214
    public void CancelClick()
    {
        if (AoWHSMap.Current.CombatMap != null && DialogHosts.General.CombatUnit != null)
        {
            DialogHosts.General.CombatUnit.SelectAbility(-1);
        }

        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWSpellBook.pas:222
    public void NextBtnClick()
    {
        Page += 1;
        UpdateSpellPages();
        Dlg.StartAnimation(PageTurnMode.Left);
        Dlg.PageTurn.Speed = 25;
        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:229
    public void PrevBtnClick()
    {
        Page -= 1;
        UpdateSpellPages();
        Dlg.StartAnimation(PageTurnMode.Right);
        Dlg.PageTurn.Speed = 25;
        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:236
    public void SpellIconDraw(Image sender, LockedSurface lockedSurface, int slot)
    {
        int index;
        int X;
        int Y;
        index = Page * 8 + slot;
        if (index >= 0 && index < _spells.GetCount())
        {
            if (_spells.GetSpell(index).CastImages.GetImageSequence(10) != null)
            {
                var image = sender;
                X = image.Window.WinLeft;
                Y = image.Window.WinTop;
                _spells.GetSpell(index).CastImages.Show(10, image.WinLeft + X, image.WinTop + Y, 0, lockedSurface);
            }
        }
    }

    // AoWSpellBook.pas:249
    public void SelectSpellBtnClick(int slot)
    {
        SelectSpell(_spells.GetSpell(Page * 8 + slot));
    }

    // AoWSpellBook.pas:254
    public void GlobalBtnClick()
    {
        if (_spellType != SpellType.Global || Page != 0 || PageKind != 1)
        {
            if (PageKind == 0)
            {
                Dlg.StartAnimation(PageTurnMode.Left);
            }
            else
            {
                Dlg.StartAnimation(PageTurnMode.Right);
            }

            AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            _spellType = SpellType.Global;
            Page = 0;
            UpdateSpellPages();
            SetPageKind(1);
        }
    }

    // AoWSpellBook.pas:264
    public void UnitBtnClick()
    {
        if (_spellType != SpellType.Unit || Page != 0 || PageKind != 1)
        {
            if (PageKind == 0 || _spellType == SpellType.Global)
            {
                Dlg.StartAnimation(PageTurnMode.Left);
            }
            else
            {
                Dlg.StartAnimation(PageTurnMode.Right);
            }

            AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            _spellType = SpellType.Unit;
            Page = 0;
            UpdateSpellPages();
            SetPageKind(1);
        }
    }

    // AoWSpellBook.pas:275
    public void CombatBtnClick()
    {
        if (_spellType != SpellType.Combat || Page != 0 || PageKind != 1)
        {
            Dlg.StartAnimation(PageTurnMode.Left);
            AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            _spellType = SpellType.Combat;
            Page = 0;
            UpdateSpellPages();
            SetPageKind(1);
        }
    }

    // AoWSpellBook.pas:285
    public void InfoBtnClick()
    {
        if (PageKind != 0)
        {
            Dlg.StartAnimation(PageTurnMode.Right);
            AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            SetPageKind(0);
        }
    }

    // AoWSpellBook.pas:295
    public void CastingBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (ValidateHero())
        {
            switch (Mode)
            {
                case 0:
                    if (_hero.CastSpell(_hero.CastingSpellId, ref reason))
                    {
                        Dlg.Visible = false;
                    }
                    else
                    {
                        AoWHSMap.Current.ShowWarning(reason);
                    }

                    break;
                case 1:
                    SelectSpell(AoWHSSet.Current.SpellControl.GetSpell(_hero.CastingSpellId));
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
            _hero.CancelCasting();
        }
    }

    // AoWSpellBook.pas:315
    public void FaceDraw(Image sender, LockedSurface lockedSurface)
    {
        Rect R;
        R = new Rect(LeftInfoPage.WinLeft + sender.WinLeft, LeftInfoPage.WinTop + sender.WinTop, LeftInfoPage.WinLeft + sender.WinLeft + sender.WinWidth, LeftInfoPage.WinTop + sender.WinTop + sender.WinHeight);

        _hero.GetFace().ShowStretched(R, lockedSurface);
    }

    // AoWSpellBook.pas:325
    public void ResearchIcnDraw(Controls.Control sender, LockedSurface lockedSurface)
    {
        Spell spell;
        int X;
        var player = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        spell = AoWHSSet.Current.SpellControl.GetSpell(player.MagicControl.ResearchSpellId);
        if (spell == null && Mode == 3)
        {
            spell = AoWHSSet.Current.SpellControl.GetSpell(_researchedSpell);
        }

        if (spell != null)
        {
            X = sender.Window.WinLeft;
            spell.CastImages.Show(10, sender.WinLeft + X, sender.Window.WinTop + sender.WinTop, 0, lockedSurface);
        }
    }

    // AoWSpellBook.pas:337
    public void CastingIcnDraw(Controls.Control sender, LockedSurface lockedSurface)
    {
        Spell spell;
        int X;
        if (_hero != null)
        {
            if (_hero.Casting())
            {
                spell = AoWHSSet.Current.SpellControl.GetSpell(_hero.CastingSpellId);
                if (spell != null)
                {
                    X = sender.Window.WinLeft;
                    spell.CastImages.Show(10, sender.WinLeft + X, sender.Window.WinTop + sender.WinTop, 0, lockedSurface);
                }
            }
        }
    }

    // AoWSpellBook.pas:352
    public void CancelResearchBtnClick()
    {
        PlayerMagicControl control;
        DialogHosts.General.PlayButtonSound(0);
        if (ValidateHero())
        {
            control = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl;
            if (control.ResearchSpellId != 0)
            {
                control.ResearchSpell(0);
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
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            if (AoWHSMap.Current.CombatMap != null && DialogHosts.General.CombatUnit != null)
            {
                DialogHosts.General.CombatUnit.SelectAbility(-1);
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

    private static readonly string s_cancelSpellResearchText = "Cancels current spell research";
    private static readonly string s_castSpellFormat = "Cast %s Spell";
    private static readonly string s_castingReadyText = "Casting Ready";
    private static readonly string s_costFormat = "Cost: %d";
    private static readonly string s_currentlyResearchingText = "Currently Researching";
    private static readonly string s_instantText = "Instant";
    private static readonly string s_manaPoolText = "Mana Pool";
    private static readonly string s_researchNewSpellText = "Research new spell";
    private static readonly string s_researchPointsText = "Research Points";
    private static readonly string s_researchText = "Research";
    private static readonly string s_researchSpellText = "Research Spell";
    private static readonly string s_researchSpellFormat = "Research %s Spell";
    private static readonly string s_researchedText = "Researched";
    private static readonly string s_turnsFormat = "Turns: %s";
}
