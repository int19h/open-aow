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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// mode, page kind, spell type, spells, page, hero, activation, researched spell, callback,
// event, saved player pages
public partial class SpellBook : DataModule
{
    public SpellBook(Component owner) : base(owner)
    { }

    public Label LeftHeaderLabel;
    public Panel LeftSpellPage;
    public Panel Spell1Panel;
    public Label Spell1SpellLabel;
    public Label Spell1ClassLabel;
    public Memo Spell1Memo;
    public Label Spell1CostLabel;
    public Image Spell1Icon;
    public Panel Spell2Panel;
    public Label Spell2SpellLabel;
    public Label Spell2ClassLabel;
    public Memo Spell2Memo;
    public Label Spell2CostLabel;
    public Image Spell2Icon;
    public Panel Spell3Panel;
    public Label Spell3SpellLabel;
    public Label Spell3ClassLabel;
    public Memo Spell3Memo;
    public Label Spell3CostLabel;
    public Image Spell3Icon;
    public Panel Spell4Panel;
    public Label Spell4SpellLabel;
    public Label Spell4ClassLabel;
    public Memo Spell4Memo;
    public Label Spell4CostLabel;
    public Image Spell4Icon;
    public Label RightHeaderLabel;
    public Panel RightSpellPage;
    public Panel Spell5Panel;
    public Label Spell5SpellLabel;
    public Label Spell5ClassLabel;
    public Memo Spell5Memo;
    public Label Spell5CostLabel;
    public Image Spell5Icon;
    public Panel Spell6Panel;
    public Label Spell6SpellLabel;
    public Label Spell6ClassLabel;
    public Memo Spell6Memo;
    public Label Spell6CostLabel;
    public Image Spell6Icon;
    public Panel Spell7Panel;
    public Label Spell7SpellLabel;
    public Label Spell7ClassLabel;
    public Memo Spell7Memo;
    public Label Spell7CostLabel;
    public Image Spell7Icon;
    public Panel Spell8Panel;
    public Label Spell8SpellLabel;
    public Label Spell8ClassLabel;
    public Memo Spell8Memo;
    public Label Spell8CostLabel;
    public Image Spell8Icon;
    public Button Spell1Button;
    public Label CurrentPointsLabel;
    public Label SlashLabel;
    public Label CastingPointsMaxLabel;
    public Button GlobalButton;
    public Button UnitButton;
    public Button CombatButton;
    public Button InfoButton;
    public Button PreviousButton;
    public Button NextButton;
    public Button Spell2Button;
    public Button Spell3Button;
    public Button Spell4Button;
    public Button Spell5Button;
    public Button Spell6Button;
    public Button Spell7Button;
    public Button Spell8Button;
    public Panel CastingPage;
    public Panel CastingPanel;
    public Label CastingSpellLabel;
    public Label CastingClassLabel;
    public Memo CastingMemo;
    public Label CastingCostLabel;
    public Panel LeftInfoPage;
    public Label CastingStatusLabel;
    public Label CastingPointsLabel;
    public Button CastingButton;
    public Button Sphere4Button;
    public Button Sphere3Button;
    public Button Sphere2Button;
    public Button Sphere1Button;
    public Button Sphere5Button;
    public Button Sphere6Button;
    public Button Sphere7Button;
    public Label SpellBookLabel;
    public Label NameLabel;
    public Label ManaPoolLabel;
    public BookWindow Book;
    public Label NoneCastingLabel;
    public Panel ResearchPage;
    public Label ResearchNameLabel;
    public Label ResearchClassLabel;
    public Memo ResearchMemo;
    public Label ResearchCostLabel;
    public Label ResearchStatusLabel;
    public Label ResearchNoneLabel;
    public Panel ResearchPanel;
    public Label ResearchLabel;
    public Panel RightInfoPage;
    public Label ResearchPointsLabel;
    public Label ResearchManaPoolLabel;
    public Label ResearchBookLabel;
    public Button CancelResearchButton;

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
        DialogHosts.MagicWindow.Window.Drawn = false;
    }

    // AoWSpellBook.pas:405
    public bool ValidateHero()
    {
        bool result;
        if (_activation != null)
        {
            _hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(_activation.UnitId);
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
        GlobalButton.Visible = true;
        GlobalButton.ImageLibraryIndexDown = 17;
        GlobalButton.ImageLibraryIndexLit = 18;
        GlobalButton.ImageLibraryIndexUp = 16;
        CombatButton.Visible = true;
        UnitButton.Visible = true;
        InfoButton.Visible = true;
        CurrentPointsLabel.Visible = true;
        CastingPointsMaxLabel.Visible = true;
        SlashLabel.Visible = true;
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
                NextButton.Visible = count - (Page + 1) * 8 > 0;
                PreviousButton.Visible = Page != 0;
                break;
            case 0:
                NextButton.Visible = false;
                PreviousButton.Visible = false;
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
                    StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ListResearchSpells(ref _spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                    _spells.SortOnLevel();
                    var spellType = AbilityRules.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_researchSpellFormat), [spellType]);
                    LeftHeaderLabel.Text = format;
                    var spellTypeText = AbilityRules.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(s_researchSpellFormat), [spellTypeText]);
                    RightHeaderLabel.Text = formattedText;
                    // { Original overwrites the type-specific headers. }
                    var researchSpell = AbilityRules.TranslateResourceString(s_researchSpellText);
                    LeftHeaderLabel.Text = researchSpell;
                    var researchSpellText = AbilityRules.TranslateResourceString(s_researchSpellText);
                    RightHeaderLabel.Text = researchSpellText;
                    break;
                case 0:
                    _spells.Clear();
                    StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
                    _spells.SortOnLevel();
                    var spellType3 = AbilityRules.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castSpellFormat), [spellType3]);
                    LeftHeaderLabel.Text = format2;
                    var spellType4 = AbilityRules.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castSpellFormat), [spellType4]);
                    RightHeaderLabel.Text = format3;
                    break;
                case 1:
                    _spells.Clear();
                    StrategicMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
                    _spells.SortOnLevel();
                    var spellType5 = AbilityRules.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    var format4 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castSpellFormat), [spellType5]);
                    LeftHeaderLabel.Text = format4;
                    var spellType6 = AbilityRules.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    var format5 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_castSpellFormat), [spellType6]);
                    RightHeaderLabel.Text = format5;
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
            ShowSpellEntry(Spell, Spell1Panel, Spell1Button, Spell1SpellLabel, Spell1ClassLabel, Spell1CostLabel, Spell1Memo, Spell1Icon);
            Spell = _spells.GetSpell(Page * 8 + 1);
            ShowSpellEntry(Spell, Spell2Panel, Spell2Button, Spell2SpellLabel, Spell2ClassLabel, Spell2CostLabel, Spell2Memo, Spell2Icon);
            Spell = _spells.GetSpell(Page * 8 + 2);
            ShowSpellEntry(Spell, Spell3Panel, Spell3Button, Spell3SpellLabel, Spell3ClassLabel, Spell3CostLabel, Spell3Memo, Spell3Icon);
            Spell = _spells.GetSpell(Page * 8 + 3);
            ShowSpellEntry(Spell, Spell4Panel, Spell4Button, Spell4SpellLabel, Spell4ClassLabel, Spell4CostLabel, Spell4Memo, Spell4Icon);
            Spell = _spells.GetSpell(Page * 8 + 4);
            ShowSpellEntry(Spell, Spell5Panel, Spell5Button, Spell5SpellLabel, Spell5ClassLabel, Spell5CostLabel, Spell5Memo, Spell5Icon);
            Spell = _spells.GetSpell(Page * 8 + 5);
            ShowSpellEntry(Spell, Spell6Panel, Spell6Button, Spell6SpellLabel, Spell6ClassLabel, Spell6CostLabel, Spell6Memo, Spell6Icon);
            Spell = _spells.GetSpell(Page * 8 + 6);
            ShowSpellEntry(Spell, Spell7Panel, Spell7Button, Spell7SpellLabel, Spell7ClassLabel, Spell7CostLabel, Spell7Memo, Spell7Icon);
            Spell = _spells.GetSpell(Page * 8 + 7);
            ShowSpellEntry(Spell, Spell8Panel, Spell8Button, Spell8SpellLabel, Spell8ClassLabel, Spell8CostLabel, Spell8Memo, Spell8Icon);
            if (Mode == 0)
            {
                _playerPages.Count = StrategicMap.Current.SeatedPlayer + 1;
                _playerPages.List[StrategicMap.Current.SeatedPlayer].First = (int)_spellType;
                _playerPages.List[StrategicMap.Current.SeatedPlayer].Second = Page;
            }

            if (PageKind == 1)
            {
                var count = _spells.GetCount();
                NextButton.Visible = count - (Page + 1) * 8 > 0;
                PreviousButton.Visible = Page != 0;
            }
        }
        else
        {
            Book.Visible = false;
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
                Player = StrategicMap.Current.SeatedPlayer;
            }

            leader = StrategicMap.Current.Players.GetPlayers(Player).Leader;
            for (int I = 0; I <= 6; I++)
            {
                switch (I)
                {
                    case 0:
                        button = Sphere1Button;
                        break;
                    case 1:
                        button = Sphere2Button;
                        break;
                    case 2:
                        button = Sphere3Button;
                        break;
                    case 3:
                        button = Sphere4Button;
                        break;
                    case 4:
                        button = Sphere5Button;
                        break;
                    case 5:
                        button = Sphere6Button;
                        break;
                    case 6:
                        button = Sphere7Button;
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

            NameLabel.Text = _hero.HeroName;
            var player = StrategicMap.Current.Players.GetPlayers(Player);
            var manaPool = AbilityRules.TranslateResourceString(s_manaPoolText);
            ManaPoolLabel.Text = manaPool + ": " + IntToStr(player.MagicControl.Mana);
            var castingPoints = _hero.GetCastingPoints();
            var castingPointsText = AbilityRules.TranslateResourceString(GameText.CastingPointsText);
            var castingPointsMax = _hero.GetCastingPointsMax();
            CastingPointsLabel.Text = castingPointsText + ": " + IntToStr(castingPoints) + "/" + IntToStr(castingPointsMax);
            ResearchManaPoolLabel.Text = ManaPoolLabel.Text;
            var netPowerToResearch = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetNetPowerToResearch();
            var researchPoints = AbilityRules.TranslateResourceString(s_researchPointsText);
            ResearchPointsLabel.Text = researchPoints + ": " + IntToStr(netPowerToResearch);
            if (Set<byte>.WithBounds(0, 255).With(2).With(3).Contains(Mode))
            {
                CurrentPointsLabel.Text = "";
                CastingPointsMaxLabel.Text = "";
                SpellBookLabel.Visible = false;
                ResearchBookLabel.Visible = true;
                CastingPage.Visible = false;
                control = StrategicMap.Current.Players.GetPlayers(Player).MagicControl;
                if (control.ResearchSpellId != 0)
                {
                    ResearchPanel.Visible = true;
                    ResearchNoneLabel.Visible = false;
                    spell = GamePack.Current.SpellControl.GetSpell(control.ResearchSpellId);
                    var currentlyResearching = AbilityRules.TranslateResourceString(s_currentlyResearchingText);
                    ResearchLabel.Text = currentlyResearching + ":";
                    ResearchMemo.Items = spell.SpellText;
                    ResearchNameLabel.Text = spell.SpellName;
                    var magicSphere = AbilityRules.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
                    var level = AbilityRules.TranslateResourceString(GameText.LevelText);
                    ResearchClassLabel.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
                    var castingMana = _hero.CastingMana(spell);
                    var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
                    var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
                    ResearchCostLabel.Text = castingCost + ": " + IntToStr(castingMana) + " " + mana;
                    if (control.GetResearchTurnsLeft() < 999)
                    {
                        var formatTurnsLeftString = DurationText.FormatTurnsLeftString(control.GetResearchTurnsLeft());
                        ResearchStatusLabel.Text = formatTurnsLeftString;
                    }
                    else
                    {
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.TurnsFormat), [(byte)63]);
                        ResearchStatusLabel.Text = format;
                    }

                    var cancel = AbilityRules.TranslateResourceString(GameText.CancelText);
                    CancelResearchButton.Text = cancel;
                    ResearchStatusLabel.Visible = true;
                }
                else
                {
                    if (Mode == 3)
                    {
                        ResearchPanel.Visible = true;
                        ResearchNoneLabel.Visible = false;
                        spell = GamePack.Current.SpellControl.GetSpell(_researchedSpell);
                        var researched = AbilityRules.TranslateResourceString(s_researchedText);
                        ResearchLabel.Text = researched + ":";
                        ResearchMemo.Items = spell.SpellText;
                        ResearchNameLabel.Text = spell.SpellName;
                        var magicSphere = AbilityRules.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
                        var level = AbilityRules.TranslateResourceString(GameText.LevelText);
                        ResearchClassLabel.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
                        var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
                        var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
                        ResearchCostLabel.Text = castingCost + ": " + IntToStr(spell.BaseManaCost) + " " + mana;
                        ResearchStatusLabel.Visible = false;
                        var research = AbilityRules.TranslateResourceString(s_researchText);
                        CancelResearchButton.Text = research;
                        var researchNewSpell = AbilityRules.TranslateResourceString(s_researchNewSpellText);
                        CancelResearchButton.ToolTipText.Text = researchNewSpell;
                    }
                    else
                    {
                        ResearchPanel.Visible = false;
                        ResearchNoneLabel.Visible = true;
                        var research = AbilityRules.TranslateResourceString(s_researchText);
                        CancelResearchButton.Text = research;
                        var cancelSpellResearch = AbilityRules.TranslateResourceString(s_cancelSpellResearchText);
                        CancelResearchButton.ToolTipText.Text = cancelSpellResearch;
                    }
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(Mode))
                {
                    var fHeroCastingPoints = _hero.GetCastingPoints();
                    CurrentPointsLabel.Text = IntToStr(fHeroCastingPoints);
                    var fHeroCastingPointsMax = _hero.GetCastingPointsMax();
                    CastingPointsMaxLabel.Text = IntToStr(fHeroCastingPointsMax);
                    SpellBookLabel.Visible = true;
                    ResearchBookLabel.Visible = false;
                    ResearchPage.Visible = false;
                    if (_hero.Casting())
                    {
                        NoneCastingLabel.Visible = false;
                        spell = GamePack.Current.SpellControl.GetSpell(_hero.CastingSpellId);
                        CastingSpellLabel.Text = spell.SpellName;
                        var magicSphere = AbilityRules.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
                        var level = AbilityRules.TranslateResourceString(GameText.LevelText);
                        CastingClassLabel.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
                        var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
                        var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
                        CastingCostLabel.Text = castingCost + ": " + IntToStr(_hero.CastingProgress) + " " + mana;
                        CastingMemo.Items = spell.SpellText;
                        var formatTurnsLeftString = DurationText.FormatTurnsLeftString(_hero.CastingTurnsLeft());
                        CastingStatusLabel.Text = "(" + formatTurnsLeftString + ")";
                        CastingPanel.Visible = true;
                        if (_hero.CastingReady())
                        {
                            CastingButton.Visible = true;
                            var castingReady = AbilityRules.TranslateResourceString(s_castingReadyText);
                            CastingStatusLabel.Text = castingReady;
                        }
                        else
                        {
                            CastingButton.Visible = false;
                            var formatTurnsLeftString2 = DurationText.FormatTurnsLeftString(_hero.CastingTurnsLeft());
                            CastingStatusLabel.Text = "(" + formatTurnsLeftString2 + ")";
                        }
                    }
                    else
                    {
                        CastingPanel.Visible = false;
                        NoneCastingLabel.Visible = true;
                    }
                }
                else
                {
                    CastingPanel.Visible = false;
                    NoneCastingLabel.Visible = true;
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
            spellClass.Text = AbilityRules.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]) + ", " + AbilityRules.TranslateResourceString(GameText.LevelText) + " " + IntToStr(spell.SpellLevel);
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

            if (Mode == 0 && (!spell.SpellControlTypes.Contains(SpellControlTypeElement.Global) || _hero.Casting()) || Mode == 1 && (!spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat) || _hero.Casting() || !_hero.CanCastSpellInstantly(spell.SpellId, spell.BaseManaCost, ref reason)) || Mode == 2 && StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpellId != 0)
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
                    control = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl;
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
                    if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpell(spell.SpellId))
                    {
                        if (_event == null || _event.Mode != PlayerMagicEventLog.EventMode.StartResearch)
                        {
                            Mode = 2;
                            InfoBtnClick();
                        }
                        else
                        {
                            Book.Visible = false;
                        }
                    }

                    break;
                case 0:
                    if (_hero.GetCastingPoints() >= spell.BaseManaCost || StrategicMap.Current.Players.GetPlayers(_hero.Player).MagicControl.GetCheatEnabled())
                    {
                        if (_hero.CastSpell(spell.SpellId, ref reason))
                        {
                            Book.Visible = false;
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
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
                            StrategicMap.Current.ShowWarning(reason);
                        }
                    }

                    break;
                case 1:
                    if (_hero.CastCombatSpell(spell.SpellId, ref reason))
                    {
                        Book.Visible = false;
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
            Book.Visible = false;
        }
    }

    // AoWSpellBook.pas:415
    public void PlayerChanged(sbyte player)
    {
        if (Book.Visible && player == StrategicMap.Current.SeatedPlayer)
        {
            UpdateSpellPages();
            UpdateInfoPages();
        }
    }

    // AoWSpellBook.pas:421
    public void UnitChanged(Troop U)
    {
        if (Book.Visible && (U == null || (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(Mode) && U == _hero)))
        {
            UpdateSpellPages();
            UpdateInfoPages();
        }
    }

    // AoWSpellBook.pas:488
    public void ShowResearch()
    {
        Reset();
        CastingPointsMaxLabel.Visible = false;
        CurrentPointsLabel.Visible = false;
        SlashLabel.Visible = false;
        GlobalButton.ImageLibraryIndexDown = 14;
        GlobalButton.ImageLibraryIndexLit = 15;
        GlobalButton.ImageLibraryIndexUp = 13;
        CombatButton.Visible = false;
        UnitButton.Visible = false;
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpellId != 0)
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
        _hero = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Leader;
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
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpellId == 0)
        {
            Reset();
            GlobalButton.ImageLibraryIndexDown = 14;
            GlobalButton.ImageLibraryIndexLit = 15;
            GlobalButton.ImageLibraryIndexUp = 13;
            CombatButton.Visible = false;
            UnitButton.Visible = false;
            _researchedSpell = 0;
            SetPageKind(1);
            Mode = 2;
            _spellType = SpellType.Global;
            Page = 0;
            _hero = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Leader;
            UpdateSpellPages();
            UpdateInfoPages();
            ShowBook();
            StrategicMap.Current.TutorialControl.ShowMessage(51, true, true);
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
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpellId == 0)
        {
            Reset();
            _researchedSpell = eventValue.SpellId;
            SetPageKind(0);
            Mode = 3;
            _spellType = SpellType.Global;
            Page = 0;
            _hero = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Leader;
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
            _spellType = (SpellType)_playerPages[StrategicMap.Current.SeatedPlayer].First;
            Page = _playerPages[StrategicMap.Current.SeatedPlayer].Second;
            if (_hero.Casting())
            {
                SetPageKind(0);
            }
            else
            {
                _spells.Clear();
                StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
                if (_spells.GetCount() == 0 || _spells.GetCount() <= Page * 8)
                {
                    _spells.Clear();
                    _spellType = (SpellType)3;
                    while (_spells.GetCount() == 0 && (int)_spellType != 0)
                    {
                        _spellType = (SpellType)((int)_spellType - 1);
                        StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
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
            GlobalButton.Visible = true;
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
                    StrategicMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).MagicControl.ListSpells(ref _spells, Set<SpellType>.Of(_spellType));
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
        if (StrategicMap.Current.CombatMap != null && DialogHosts.General.CombatUnit != null)
        {
            DialogHosts.General.CombatUnit.SelectAbility(-1);
        }

        DialogHosts.General.PlayButtonSound(0);
        Book.Visible = false;
    }

    // AoWSpellBook.pas:222
    public void NextBtnClick()
    {
        Page += 1;
        UpdateSpellPages();
        Book.StartAnimation(PageTurnMode.Left);
        Book.PageTurn.Speed = 25;
        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:229
    public void PrevBtnClick()
    {
        Page -= 1;
        UpdateSpellPages();
        Book.StartAnimation(PageTurnMode.Right);
        Book.PageTurn.Speed = 25;
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
    public void SpellButtonClick(int slot)
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
                Book.StartAnimation(PageTurnMode.Left);
            }
            else
            {
                Book.StartAnimation(PageTurnMode.Right);
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
                Book.StartAnimation(PageTurnMode.Left);
            }
            else
            {
                Book.StartAnimation(PageTurnMode.Right);
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
            Book.StartAnimation(PageTurnMode.Left);
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
            Book.StartAnimation(PageTurnMode.Right);
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
                        Book.Visible = false;
                    }
                    else
                    {
                        StrategicMap.Current.ShowWarning(reason);
                    }

                    break;
                case 1:
                    SelectSpell(GamePack.Current.SpellControl.GetSpell(_hero.CastingSpellId));
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
        var player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        spell = GamePack.Current.SpellControl.GetSpell(player.MagicControl.ResearchSpellId);
        if (spell == null && Mode == 3)
        {
            spell = GamePack.Current.SpellControl.GetSpell(_researchedSpell);
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
                spell = GamePack.Current.SpellControl.GetSpell(_hero.CastingSpellId);
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
            control = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl;
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
        if (Book.Visible)
        {
            if (!ValidateHero())
            {
                Book.Visible = false;
            }
        }
    }

    // AoWSpellBook.pas:375
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            if (StrategicMap.Current.CombatMap != null && DialogHosts.General.CombatUnit != null)
            {
                DialogHosts.General.CombatUnit.SelectAbility(-1);
            }

            Book.Visible = false;
        }
    }

    // AoWSpellBook.pas:385. The host surfaces reside in system memory.
    public void ShowBook()
    {
        DialogHosts.General.Manager.OverlayPriority = 49;
        Book.Visible = true;
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
