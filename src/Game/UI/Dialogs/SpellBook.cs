#nullable disable
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

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.UI.Dialogs;

// mode, page kind, spell type, spells, page, hero, activation, researched spell, callback,
// event, saved player pages
public partial class SpellBook(UIContext ui) : UIModule(ui)
{
    public Label LeftHeaderLabel { get; private set; }
    public Panel LeftSpellPage { get; private set; }
    public Panel Spell1Panel { get; private set; }
    public Label Spell1SpellLabel { get; private set; }
    public Label Spell1ClassLabel { get; private set; }
    public Memo Spell1Memo { get; private set; }
    public Label Spell1CostLabel { get; private set; }
    public Image Spell1Icon { get; private set; }
    public Panel Spell2Panel { get; private set; }
    public Label Spell2SpellLabel { get; private set; }
    public Label Spell2ClassLabel { get; private set; }
    public Memo Spell2Memo { get; private set; }
    public Label Spell2CostLabel { get; private set; }
    public Image Spell2Icon { get; private set; }
    public Panel Spell3Panel { get; private set; }
    public Label Spell3SpellLabel { get; private set; }
    public Label Spell3ClassLabel { get; private set; }
    public Memo Spell3Memo { get; private set; }
    public Label Spell3CostLabel { get; private set; }
    public Image Spell3Icon { get; private set; }
    public Panel Spell4Panel { get; private set; }
    public Label Spell4SpellLabel { get; private set; }
    public Label Spell4ClassLabel { get; private set; }
    public Memo Spell4Memo { get; private set; }
    public Label Spell4CostLabel { get; private set; }
    public Image Spell4Icon { get; private set; }
    public Label RightHeaderLabel { get; private set; }
    public Panel RightSpellPage { get; private set; }
    public Panel Spell5Panel { get; private set; }
    public Label Spell5SpellLabel { get; private set; }
    public Label Spell5ClassLabel { get; private set; }
    public Memo Spell5Memo { get; private set; }
    public Label Spell5CostLabel { get; private set; }
    public Image Spell5Icon { get; private set; }
    public Panel Spell6Panel { get; private set; }
    public Label Spell6SpellLabel { get; private set; }
    public Label Spell6ClassLabel { get; private set; }
    public Memo Spell6Memo { get; private set; }
    public Label Spell6CostLabel { get; private set; }
    public Image Spell6Icon { get; private set; }
    public Panel Spell7Panel { get; private set; }
    public Label Spell7SpellLabel { get; private set; }
    public Label Spell7ClassLabel { get; private set; }
    public Memo Spell7Memo { get; private set; }
    public Label Spell7CostLabel { get; private set; }
    public Image Spell7Icon { get; private set; }
    public Panel Spell8Panel { get; private set; }
    public Label Spell8SpellLabel { get; private set; }
    public Label Spell8ClassLabel { get; private set; }
    public Memo Spell8Memo { get; private set; }
    public Label Spell8CostLabel { get; private set; }
    public Image Spell8Icon { get; private set; }
    public Button Spell1Button { get; private set; }
    public Label CurrentPointsLabel { get; private set; }
    public Label SlashLabel { get; private set; }
    public Label CastingPointsMaxLabel { get; private set; }
    public Button GlobalButton { get; private set; }
    public Button UnitButton { get; private set; }
    public Button CombatButton { get; private set; }
    public Button InfoButton { get; private set; }
    public Button PreviousButton { get; private set; }
    public Button NextButton { get; private set; }
    public Button Spell2Button { get; private set; }
    public Button Spell3Button { get; private set; }
    public Button Spell4Button { get; private set; }
    public Button Spell5Button { get; private set; }
    public Button Spell6Button { get; private set; }
    public Button Spell7Button { get; private set; }
    public Button Spell8Button { get; private set; }
    public Panel CastingPage { get; private set; }
    public Panel CastingPanel { get; private set; }
    public Label CastingSpellLabel { get; private set; }
    public Label CastingClassLabel { get; private set; }
    public Memo CastingMemo { get; private set; }
    public Label CastingCostLabel { get; private set; }
    public Panel LeftInfoPage { get; private set; }
    public Label CastingStatusLabel { get; private set; }
    public Label CastingPointsLabel { get; private set; }
    public Button CastingButton { get; private set; }
    public Button Sphere4Button { get; private set; }
    public Button Sphere3Button { get; private set; }
    public Button Sphere2Button { get; private set; }
    public Button Sphere1Button { get; private set; }
    public Button Sphere5Button { get; private set; }
    public Button Sphere6Button { get; private set; }
    public Button Sphere7Button { get; private set; }
    public Label SpellBookLabel { get; private set; }
    public Label NameLabel { get; private set; }
    public Label ManaPoolLabel { get; private set; }
    public BookWindow Book { get; private set; }
    public Label NoneCastingLabel { get; private set; }
    public Panel ResearchPage { get; private set; }
    public Label ResearchNameLabel { get; private set; }
    public Label ResearchClassLabel { get; private set; }
    public Memo ResearchMemo { get; private set; }
    public Label ResearchCostLabel { get; private set; }
    public Label ResearchStatusLabel { get; private set; }
    public Label ResearchNoneLabel { get; private set; }
    public Panel ResearchPanel { get; private set; }
    public Label ResearchLabel { get; private set; }
    public Panel RightInfoPage { get; private set; }
    public Label ResearchPointsLabel { get; private set; }
    public Label ResearchManaPoolLabel { get; private set; }
    public Label ResearchBookLabel { get; private set; }
    public Button CancelResearchButton { get; private set; }

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
    private event ExecuteDoneEvent Done;
    private PlayerMagicEventLog _event;
    private DoubleItemList _playerPages;

    // AoWSpellBook.pas:393
    public void FinishDialog()
    {
        _ = AudioPlayback.PlaySample("SFX\\INT\\BOOKCLSE.WAV");
        ExecuteDoneEvent done = Done;
        PlayerMagicEventLog eventValue = _event;
        Done = null;
        _event = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());

        UI.General.Manager.OverlayPriority = 0;
        UI.General.Manager.DrawOverlay = true;
        UI.MagicWindow.Window.Drawn = false;
    }

    // AoWSpellBook.pas:405
    public bool ValidateHero()
    {
        if (_activation is not null)
        {
            _hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(_activation.UnitId);
            return _hero is not null && !_hero.UnitStatus.Contains(UnitStatusElement.Killed);
        }
        else
        {
            return true;
        }
    }

    // AoWSpellBook.pas:427
    public void Reset()
    {
        _ = (_activation?.Release());
        _activation = null;

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
                int count = _spells.GetCount();
                NextButton.Visible = count - ((Page + 1) * 8) > 0;
                PreviousButton.Visible = Page != 0;
                break;
            case 0:
                NextButton.Visible = false;
                PreviousButton.Visible = false;
                break;
            default:
                break;
        }
    }

    // AoWSpellBook.pas:582
    public void UpdateSpellPages()
    {
        if (ValidateHero())
        {
            switch (Mode)
            {
                case 2:
                case 3:
                    _spells.Clear();
                    StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ListResearchSpells(_spells, spellTypes: Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                    _spells.SortOnLevel();
                    string spellType = UI.Engine.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_researchSpellFormat), arguments: [spellType]);
                    LeftHeaderLabel.Text = format;
                    string spellTypeText = UI.Engine.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    string formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(s_researchSpellFormat), arguments: [spellTypeText]);
                    RightHeaderLabel.Text = formattedText;
                    // { Original overwrites the type-specific headers. }
                    string researchSpell = UI.Engine.TranslateResourceString(s_researchSpellText);
                    LeftHeaderLabel.Text = researchSpell;
                    string researchSpellText = UI.Engine.TranslateResourceString(s_researchSpellText);
                    RightHeaderLabel.Text = researchSpellText;
                    break;
                case 0:
                    _spells.Clear();
                    StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ListSpells(_spells, spellTypes: Set<SpellType>.Of(_spellType));
                    _spells.SortOnLevel();
                    string spellType3 = UI.Engine.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    string format2 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_castSpellFormat), arguments: [spellType3]);
                    LeftHeaderLabel.Text = format2;
                    string spellType4 = UI.Engine.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    string format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_castSpellFormat), arguments: [spellType4]);
                    RightHeaderLabel.Text = format3;
                    break;
                case 1:
                    _spells.Clear();
                    StrategicMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).MagicControl.ListSpells(_spells, spellTypes: Set<SpellType>.Of(_spellType));
                    _spells.SortOnLevel();
                    string spellType5 = UI.Engine.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    string format4 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_castSpellFormat), arguments: [spellType5]);
                    LeftHeaderLabel.Text = format4;
                    string spellType6 = UI.Engine.TranslateResourceString(GameTables.SpellTypeTexts[(int)_spellType]);
                    string format5 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_castSpellFormat), arguments: [spellType6]);
                    RightHeaderLabel.Text = format5;
                    break;
                default:
                    break;
            }

            for (int i = _spells.GetCount() - 1; i >= 0; i--)
            {
                bool condition = _spells[i].SpellId >= 100;
                if (condition)
                {
                    Spell spell = _spells[i];
                    condition = TacticalTables.SpellTypes[spell.SpellId] == BattleEffectType.None;
                }

                if (condition)
                {
                    _spells.Delete(i);
                }
            }

            Spell spellSpell = _spells[Page * 8];
            ShowSpellEntry(spellSpell, panel: Spell1Panel, button: Spell1Button, name: Spell1SpellLabel, spellClass: Spell1ClassLabel, cost: Spell1CostLabel, memo: Spell1Memo, icon: Spell1Icon);
            spellSpell = _spells[(Page * 8) + 1];
            ShowSpellEntry(spellSpell, panel: Spell2Panel, button: Spell2Button, name: Spell2SpellLabel, spellClass: Spell2ClassLabel, cost: Spell2CostLabel, memo: Spell2Memo, icon: Spell2Icon);
            spellSpell = _spells[(Page * 8) + 2];
            ShowSpellEntry(spellSpell, panel: Spell3Panel, button: Spell3Button, name: Spell3SpellLabel, spellClass: Spell3ClassLabel, cost: Spell3CostLabel, memo: Spell3Memo, icon: Spell3Icon);
            spellSpell = _spells[(Page * 8) + 3];
            ShowSpellEntry(spellSpell, panel: Spell4Panel, button: Spell4Button, name: Spell4SpellLabel, spellClass: Spell4ClassLabel, cost: Spell4CostLabel, memo: Spell4Memo, icon: Spell4Icon);
            spellSpell = _spells[(Page * 8) + 4];
            ShowSpellEntry(spellSpell, panel: Spell5Panel, button: Spell5Button, name: Spell5SpellLabel, spellClass: Spell5ClassLabel, cost: Spell5CostLabel, memo: Spell5Memo, icon: Spell5Icon);
            spellSpell = _spells[(Page * 8) + 5];
            ShowSpellEntry(spellSpell, panel: Spell6Panel, button: Spell6Button, name: Spell6SpellLabel, spellClass: Spell6ClassLabel, cost: Spell6CostLabel, memo: Spell6Memo, icon: Spell6Icon);
            spellSpell = _spells[(Page * 8) + 6];
            ShowSpellEntry(spellSpell, panel: Spell7Panel, button: Spell7Button, name: Spell7SpellLabel, spellClass: Spell7ClassLabel, cost: Spell7CostLabel, memo: Spell7Memo, icon: Spell7Icon);
            spellSpell = _spells[(Page * 8) + 7];
            ShowSpellEntry(spellSpell, panel: Spell8Panel, button: Spell8Button, name: Spell8SpellLabel, spellClass: Spell8ClassLabel, cost: Spell8CostLabel, memo: Spell8Memo, icon: Spell8Icon);
            if (Mode == 0)
            {
                _playerPages.Count = StrategicMap.Current.SeatedPlayer + 1;
                _playerPages.List[StrategicMap.Current.SeatedPlayer] = new DoubleItemList.DoubleItem(First: (int)_spellType, Second: Page);
            }

            if (PageKind == 1)
            {
                int count = _spells.GetCount();
                NextButton.Visible = count - ((Page + 1) * 8) > 0;
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
        if (ValidateHero())
        {
            sbyte player2 = Mode == 1 ? CombatMap.Current.SeatedPlayer : StrategicMap.Current.SeatedPlayer;

            Leader leader = StrategicMap.Current.Players.GetPlayers(player2).Leader;
            for (int i = 0; i <= 6; i++)
            {
                Button button = i switch
                {
                    0 => Sphere1Button,
                    1 => Sphere2Button,
                    2 => Sphere3Button,
                    3 => Sphere4Button,
                    4 => Sphere5Button,
                    5 => Sphere6Button,
                    6 => Sphere7Button,
                    _ => null,
                };
                if (i < leader.SphereCount)
                {
                    button.Visible = true;
                    switch ((int)leader.GetSpheres(i) & 127)
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
                        default:
                            break;
                    }
                }
                else
                {
                    button.Visible = false;
                }
            }

            NameLabel.Text = _hero.HeroName;
            Player player = StrategicMap.Current.Players.GetPlayers(player2);
            string manaPool = UI.Engine.TranslateResourceString(s_manaPoolText);
            ManaPoolLabel.Text = $"{manaPool}: {IntToStr(player.MagicControl.Mana)}";
            sbyte castingPoints = _hero.GetCastingPoints();
            string castingPointsText = UI.Engine.TranslateResourceString(GameText.CastingPointsText);
            sbyte castingPointsMax = _hero.GetCastingPointsMax();
            CastingPointsLabel.Text = $"{castingPointsText}: {IntToStr(castingPoints)}/{IntToStr(castingPointsMax)}";
            ResearchManaPoolLabel.Text = ManaPoolLabel.Text;
            int netPowerToResearch = StrategicMap.Current.Players.GetPlayers(player2).MagicControl.GetNetPowerToResearch();
            string researchPoints = UI.Engine.TranslateResourceString(s_researchPointsText);
            ResearchPointsLabel.Text = $"{researchPoints}: {IntToStr(netPowerToResearch)}";
            Spell spell;
            if (Set<byte>.WithBounds(0, 255).With(2).With(3).Contains(Mode))
            {
                CurrentPointsLabel.Text = "";
                CastingPointsMaxLabel.Text = "";
                SpellBookLabel.Visible = false;
                ResearchBookLabel.Visible = true;
                CastingPage.Visible = false;
                PlayerMagicControl control = StrategicMap.Current.Players.GetPlayers(player2).MagicControl;
                if (control.ResearchSpellId != 0)
                {
                    ResearchPanel.Visible = true;
                    ResearchNoneLabel.Visible = false;
                    spell = GamePack.Current.SpellControl[control.ResearchSpellId];
                    string currentlyResearching = UI.Engine.TranslateResourceString(s_currentlyResearchingText);
                    ResearchLabel.Text = $"{currentlyResearching}:";
                    ResearchMemo.Items = spell.SpellText;
                    ResearchNameLabel.Text = spell.SpellName;
                    string magicSphere = UI.Engine.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
                    string level = UI.Engine.TranslateResourceString(GameText.LevelText);
                    ResearchClassLabel.Text = $"{magicSphere}, {level} {IntToStr(spell.SpellLevel)}";
                    int castingMana = _hero.CastingMana(spell);
                    string castingCost = UI.Engine.TranslateResourceString(GameText.CastingCostText);
                    string mana = UI.Engine.TranslateResourceString(GameText.ManaText);
                    ResearchCostLabel.Text = $"{castingCost}: {IntToStr(castingMana)} {mana}";
                    if (control.GetResearchTurnsLeft() < 999)
                    {
                        string formatTurnsLeftString = DurationText.FormatTurnsLeftString(UI.Engine, turns: control.GetResearchTurnsLeft());
                        ResearchStatusLabel.Text = formatTurnsLeftString;
                    }
                    else
                    {
                        string format = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.TurnsFormat), arguments: [(byte)63]);
                        ResearchStatusLabel.Text = format;
                    }

                    string cancel = UI.Engine.TranslateResourceString(GameText.CancelText);
                    CancelResearchButton.Text = cancel;
                    ResearchStatusLabel.Visible = true;
                }
                else
                {
                    if (Mode == 3)
                    {
                        ResearchPanel.Visible = true;
                        ResearchNoneLabel.Visible = false;
                        spell = GamePack.Current.SpellControl[_researchedSpell];
                        string researched = UI.Engine.TranslateResourceString(s_researchedText);
                        ResearchLabel.Text = $"{researched}:";
                        ResearchMemo.Items = spell.SpellText;
                        ResearchNameLabel.Text = spell.SpellName;
                        string magicSphere = UI.Engine.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
                        string level = UI.Engine.TranslateResourceString(GameText.LevelText);
                        ResearchClassLabel.Text = $"{magicSphere}, {level} {IntToStr(spell.SpellLevel)}";
                        string castingCost = UI.Engine.TranslateResourceString(GameText.CastingCostText);
                        string mana = UI.Engine.TranslateResourceString(GameText.ManaText);
                        ResearchCostLabel.Text = $"{castingCost}: {IntToStr(spell.BaseManaCost)} {mana}";
                        ResearchStatusLabel.Visible = false;
                        string research = UI.Engine.TranslateResourceString(s_researchText);
                        CancelResearchButton.Text = research;
                        string researchNewSpell = UI.Engine.TranslateResourceString(s_researchNewSpellText);
                        CancelResearchButton.ToolTipText.Text = researchNewSpell;
                    }
                    else
                    {
                        ResearchPanel.Visible = false;
                        ResearchNoneLabel.Visible = true;
                        string research = UI.Engine.TranslateResourceString(s_researchText);
                        CancelResearchButton.Text = research;
                        string cancelSpellResearch = UI.Engine.TranslateResourceString(s_cancelSpellResearchText);
                        CancelResearchButton.ToolTipText.Text = cancelSpellResearch;
                    }
                }
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(Mode))
                {
                    sbyte fHeroCastingPoints = _hero.GetCastingPoints();
                    CurrentPointsLabel.Text = IntToStr(fHeroCastingPoints);
                    sbyte fHeroCastingPointsMax = _hero.GetCastingPointsMax();
                    CastingPointsMaxLabel.Text = IntToStr(fHeroCastingPointsMax);
                    SpellBookLabel.Visible = true;
                    ResearchBookLabel.Visible = false;
                    ResearchPage.Visible = false;
                    if (_hero.Casting())
                    {
                        NoneCastingLabel.Visible = false;
                        spell = GamePack.Current.SpellControl[_hero.CastingSpellId];
                        CastingSpellLabel.Text = spell.SpellName;
                        string magicSphere = UI.Engine.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
                        string level = UI.Engine.TranslateResourceString(GameText.LevelText);
                        CastingClassLabel.Text = $"{magicSphere}, {level} {IntToStr(spell.SpellLevel)}";
                        string castingCost = UI.Engine.TranslateResourceString(GameText.CastingCostText);
                        string mana = UI.Engine.TranslateResourceString(GameText.ManaText);
                        CastingCostLabel.Text = $"{castingCost}: {IntToStr(_hero.CastingProgress)} {mana}";
                        CastingMemo.Items = spell.SpellText;
                        string formatTurnsLeftString = DurationText.FormatTurnsLeftString(UI.Engine, turns: _hero.CastingTurnsLeft());
                        CastingStatusLabel.Text = $"({formatTurnsLeftString})";
                        CastingPanel.Visible = true;
                        if (_hero.CastingReady())
                        {
                            CastingButton.Visible = true;
                            string castingReady = UI.Engine.TranslateResourceString(s_castingReadyText);
                            CastingStatusLabel.Text = castingReady;
                        }
                        else
                        {
                            CastingButton.Visible = false;
                            string formatTurnsLeftString2 = DurationText.FormatTurnsLeftString(UI.Engine, turns: _hero.CastingTurnsLeft());
                            CastingStatusLabel.Text = $"({formatTurnsLeftString2})";
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
        if (spell is not null)
        {
            panel.Visible = true;
            name.Text = spell.SpellName;
            spellClass.Text = $"{UI.Engine.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere])}, {UI.Engine.TranslateResourceString(GameText.LevelText)} {IntToStr(spell.SpellLevel)}";
            if (Mode == 2)
            {
                if (spell.SpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global))
                {
                    string global = UI.Engine.TranslateResourceString(GameTables.GlobalText);
                    spellClass.Text = $"{spellClass.Text}, {global}";
                }
                else
                {
                    if (spell.SpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Combat))
                    {
                        string combat = UI.Engine.TranslateResourceString(GameTables.CombatText);
                        spellClass.Text = $"{spellClass.Text}, {combat}";
                    }
                    else
                    {
                        if (spell.SpellControlTypes == Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat))
                        {
                            string global = UI.Engine.TranslateResourceString(GameTables.GlobalText);
                            string combat = UI.Engine.TranslateResourceString(GameTables.CombatText);
                            spellClass.Text = $"{spellClass.Text}, {global}/{combat}";
                        }
                    }
                }
            }

            memo.Items = spell.SpellText;
            if (_hero is null)
            {
                cost.Text = AbilityRules.Format(UI.Engine.TranslateResourceString(s_costFormat), arguments: [spell.BaseManaCost]);
            }
            else
            {
                int castingMana = _hero.CastingMana(spell);
                cost.Text = AbilityRules.Format(UI.Engine.TranslateResourceString(s_costFormat), arguments: [castingMana]);
            }

            string reason = "";
            if ((Mode == 0 && (!spell.SpellControlTypes.Contains(SpellControlTypeElement.Global) || _hero.Casting())) || (Mode == 1 && (!spell.SpellControlTypes.Contains(SpellControlTypeElement.Combat) || _hero.Casting() || !_hero.CanCastSpellInstantly(spell.SpellId, mana: spell.BaseManaCost, ref reason))) || (Mode == 2 && StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpellId != 0))
            {
                memo.TextColor = 5000268;
                name.TextColor = 5000268;
                spellClass.TextColor = 5000268;
                cost.TextColor = 5000268;
                button.Visible = false;
                icon.Tag |= 16;
            }
            else
            {
                memo.TextColor = 1254460;
                name.TextColor = 1254460;
                spellClass.TextColor = 5908276;
                cost.TextColor = 5908276;
                button.Visible = true;
                icon.Tag &= 15;
            }

            if (spell.ManaUpkeep > 0)
            {
                string format = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.UpkeepFormat), arguments: [spell.ManaUpkeep]);
                cost.Text = $"{cost.Text} {format}";
            }

            int turns;
            if (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(Mode))
            {
                turns = _hero.CastingTurns(spell);
                reason = turns == 0
                    ? UI.Engine.TranslateResourceString(s_instantText)
                    : AbilityRules.Format(UI.Engine.TranslateResourceString(s_turnsFormat), arguments: [IntToStr(turns)]);

                cost.Text = $"{cost.Text} {reason}";
            }
            else
            {
                if (Mode is 2 or 3)
                {
                    PlayerMagicControl control = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl;
                    turns = control.ResearchTurns(control.GetNetPowerToResearch(), points: spell.ResearchCost);
                    if (turns < 999)
                    {
                        string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_turnsFormat), arguments: [IntToStr(turns)]);
                        cost.Text = $"{cost.Text} {format}";
                    }
                    else
                    {
                        string format = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.TurnsFormat), arguments: [(byte)63]);
                        cost.Text = $"{cost.Text} {format}";
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
        if (ValidateHero())
        {
            string reason = "";
            switch (Mode)
            {
                case 2:
                case 3:
                    if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpell(spell.SpellId))
                    {
                        if (_event is null || _event.Mode != PlayerMagicEventLog.EventMode.StartResearch)
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
                default:
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
        if (Book.Visible && (U is null || (Set<byte>.WithBounds(0, 255).With(0).With(1).Contains(Mode) && U == _hero)))
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
        _ = _event.AddRef();
        Done = done;
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
            StrategicMap.Current.TutorialControl.ShowMessage(51, immediate: true, onceOnly: true);
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
        _ = _event.AddRef();
        Done = done;
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
        _ = _activation.AddRef();
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
                StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ListSpells(_spells, spellTypes: Set<SpellType>.Of(_spellType));
                if (_spells.GetCount() == 0 || _spells.GetCount() <= Page * 8)
                {
                    _spells.Clear();
                    _spellType = (SpellType)3;
                    while (_spells.GetCount() == 0 && (int)_spellType != 0)
                    {
                        _spellType = (SpellType)((int)_spellType - 1);
                        StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ListSpells(_spells, spellTypes: Set<SpellType>.Of(_spellType));
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
        _ = _activation.AddRef();
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
                    StrategicMap.Current.Players.GetPlayers(CombatMap.Current.SeatedPlayer).MagicControl.ListSpells(_spells, spellTypes: Set<SpellType>.Of(_spellType));
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
        DoubleItemList.DoubleItem defaults = new(First: 2, Second: 0);
        _spells = new SpellList();
        _playerPages = new DoubleItemList
        {
            Default = defaults
        };
    }

    // AoWSpellBook.pas:206
    public void SpellBookDestroy()
    {
        _ = (_activation?.Release());
        _activation = null;

        _playerPages?.Dispose();
        _spells?.Dispose();
    }

    // AoWSpellBook.pas:214
    public void CancelClick()
    {
        if (StrategicMap.Current.CombatMap is not null && UI.General.CombatUnit is not null)
        {
            _ = UI.General.CombatUnit.SelectAbility(-1);
        }

        UI.General.PlayButtonSound(0);
        Book.Visible = false;
    }

    // AoWSpellBook.pas:222
    public void NextBtnClick()
    {
        Page += 1;
        UpdateSpellPages();
        Book.StartAnimation(PageTurnMode.Left);
        Book.PageTurn.Speed = 25;
        _ = AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:229
    public void PrevBtnClick()
    {
        Page -= 1;
        UpdateSpellPages();
        Book.StartAnimation(PageTurnMode.Right);
        Book.PageTurn.Speed = 25;
        _ = AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
    }

    // AoWSpellBook.pas:236
    public void SpellIconDraw(Image sender, LockedSurface lockedSurface, int slot)
    {
        int index = (Page * 8) + slot;
        if (index >= 0 && index < _spells.GetCount())
        {
            if (_spells[index].CastImages.GetImageSequence(10) is not null)
            {
                Image image = sender;
                int x = image.Window.AbsoluteLeft;
                int y = image.Window.AbsoluteTop;
                _ = _spells[index].CastImages.Show(10, image.WinLeft + x, image.WinTop + y, frame: 0, lockedSurface);
            }
        }
    }

    // AoWSpellBook.pas:249
    public void SpellButtonClick(int slot) => SelectSpell(_spells[(Page * 8) + slot]);

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

            _ = AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
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

            _ = AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
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
            _ = AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
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
            _ = AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            SetPageKind(0);
        }
    }

    // AoWSpellBook.pas:295
    public void CastingBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (ValidateHero())
        {
            switch (Mode)
            {
                case 0:
                    string reason = "";
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
                    SelectSpell(GamePack.Current.SpellControl[_hero.CastingSpellId]);
                    break;
                default:
                    break;
            }
        }
    }

    // AoWSpellBook.pas:309
    public void CancelCastingBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (ValidateHero())
        {
            _ = _hero.CancelCasting();
        }
    }

    // AoWSpellBook.pas:315
    public void FaceDraw(Image sender, LockedSurface lockedSurface)
    {
        Rect r = new(LeftInfoPage.AbsoluteLeft + sender.WinLeft, LeftInfoPage.AbsoluteTop + sender.WinTop, LeftInfoPage.AbsoluteLeft + sender.WinLeft + sender.WinWidth, LeftInfoPage.AbsoluteTop + sender.WinTop + sender.WinHeight);

        _hero.GetFace().ShowStretched(r, lockedSurface);
    }

    // AoWSpellBook.pas:325
    public void ResearchIcnDraw(Control sender, LockedSurface lockedSurface)
    {
        Player player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        Spell spell = GamePack.Current.SpellControl[player.MagicControl.ResearchSpellId];
        if (spell is null && Mode == 3)
        {
            spell = GamePack.Current.SpellControl[_researchedSpell];
        }

        if (spell is not null)
        {
            int x = sender.Window.AbsoluteLeft;
            _ = spell.CastImages.Show(10, sender.WinLeft + x, sender.Window.AbsoluteTop + sender.WinTop, frame: 0, lockedSurface);
        }
    }

    // AoWSpellBook.pas:337
    public void CastingIcnDraw(Control sender, LockedSurface lockedSurface)
    {
        if (_hero is not null)
        {
            if (_hero.Casting())
            {
                Spell spell = GamePack.Current.SpellControl[_hero.CastingSpellId];
                if (spell is not null)
                {
                    int x = sender.Window.AbsoluteLeft;
                    _ = spell.CastImages.Show(10, sender.WinLeft + x, sender.Window.AbsoluteTop + sender.WinTop, frame: 0, lockedSurface);
                }
            }
        }
    }

    // AoWSpellBook.pas:352
    public void CancelResearchBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (ValidateHero())
        {
            PlayerMagicControl control = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl;
            if (control.ResearchSpellId != 0)
            {
                _ = control.ResearchSpell(0);
            }
            else
            {
                GlobalBtnClick();
            }
        }
    }

    // AoWSpellBook.pas:363
    public void Sel1Click() => UI.General.PlayButtonSound(0);

    // AoWSpellBook.pas:366
    public void DlgHide() => FinishDialog();

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
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            if (StrategicMap.Current.CombatMap is not null && UI.General.CombatUnit is not null)
            {
                _ = UI.General.CombatUnit.SelectAbility(-1);
            }

            Book.Visible = false;
        }
    }

    // AoWSpellBook.pas:385. The host surfaces reside in system memory.
    public void ShowBook()
    {
        UI.General.Manager.OverlayPriority = 49;
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
