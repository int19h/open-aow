// Translated from E3Globals.pas.
using Aow.Game.Engine.Buffers;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Ui.Dialogs;
using Aow.Game.World.Structures;
using static Aow.Game.Audio.SoundUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWExeFormsUnit;
using static Aow.Game.Ui.Dialogs.AoWVideoDlgUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public static partial class E3GlobalsUnit
{
    public static TUnitSelectionDlg UnitSelectionDlg;
    public static TArmyInfoDlg ArmyInfoDlg;
    public static TDisjunctionDlg DisjunctionDlg;
    public static TEmailSettingsDlg EmailSettingsDlg;
    public static TEmailGameSettingsDlg EmailGameSettingsDlg;
    public static TPOP3Dlg POP3Dlg;
    public static TCastUnitSpellDlg CastUnitSpellDlg;
    public static TDeformFaceDlg DeformFaceDlg;

    // E3Globals.pas:222
    public static void HideItemSelection(TIWindow Window)
    {
        Window.InfoWin.Visible = false;
        Window.FItemBag = null;
    }

    // E3Globals.pas:130
    public static void RefreshMagicWindow(TMagicWin Window)
    {
        TPlayerMagicControl Control;
        int Turns;
        TSpell Spell;
        if (AoWHSMap.FSeatedPlayer != -1)
        {
            Control = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl;
            Window.EnchantmentList.Items.Clear();
            Window.BindingList.Items.Clear();
            Window.UpkeepList.Items.Clear();
            for (int I = 0, last = Control.FEnchantments.Count - 1; I <= last; I++)
            {
                var name = Control.FEnchantments.GetEnchantments(I).GetName();
                var enchantment = Control.FEnchantments.GetEnchantments(I);
                Window.EnchantmentList.Items.AddObject(name, enchantment);
                var binding = Control.FEnchantments.GetEnchantments(I).GetBinding();
                Window.BindingList.Items.Add(binding);
                var enchantmentUpkeep = Control.FEnchantments.GetEnchantments(I);
                Window.UpkeepList.Items.Add(IntToStr(enchantmentUpkeep.FUpkeep));
            }

            Window.PowerSourceList.Items.Clear();
            Window.PowerValueList.Items.Clear();
            for (int I = 0, last = Control.FPowerSources.Count - 1; I <= last; I++)
            {
                if (Control.FPowerSources.GetItems(I).Power() > 0)
                {
                    var name = Control.FPowerSources.GetItems(I).Name();
                    var powerSource = Control.FPowerSources.GetItems(I);
                    Window.PowerSourceList.Items.AddObject(name, powerSource);
                    var power = Control.FPowerSources.GetItems(I).Power();
                    Window.PowerValueList.Items.Add(IntToStr(power));
                }
            }

            var controlPower = Control.GetPower();
            Window.Power.Text = IntToStr(controlPower);
            var manaIncome = Control.GetManaIncome();
            var manaUpkeep = Control.GetManaUpkeep();
            var controlManaIncome = Control.GetManaIncome();
            var controlManaUpkeep = Control.GetManaUpkeep();
            Window.NetMana.Text = IntToStr(manaIncome) + "-" + IntToStr(manaUpkeep) + "=" + IntToStr(controlManaIncome - controlManaUpkeep);
            var netPowerToResearch = Control.GetNetPowerToResearch();
            Window.ResearchPoints.Text = IntToStr(netPowerToResearch);
            if (Control.FResearchSpellID != 0)
            {
                Spell = AoWHSSet.FSpellControl.GetSpell(Control.FResearchSpellID);
                Window.SpellName.Text = Spell.FSpellName;
                var magicSphere = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                var level = TranslateRStr(LevelRStr);
                Window.SpellClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                var castingCost = TranslateRStr(CastingCostRStr);
                var mana = TranslateRStr(ManaRStr);
                Window.SpellCost.Text = castingCost + ": " + IntToStr(Spell.FBaseManaCost) + " " + mana;
                Turns = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.GetResearchTurnsLeft();
                if (Turns < 999)
                {
                    var fmtTurnsLeftStr = FmtTurnsLeftStr(Turns);
                    Window.ResearchTurnsLeft.Text = fmtTurnsLeftStr;
                }
                else
                {
                    var format = AoWEUnit.Format(TranslateRStr(XTurnsRStr), [(byte)63]);
                    Window.ResearchTurnsLeft.Text = format;
                }

                Window.SpellPnl.Visible = true;
            }
            else
            {
                Window.SpellPnl.Visible = false;
            }

            Window.SortEnchantments();
            Window.SelectEnchantment(Window.EnchantmentList.Selected);
        }

        Window.Dlg.FDrawn = false;
    }

    // E3Globals.pas:172
    public static void MagicPlayerChanged(TMagicWin Window, sbyte Player)
    {
        if (Window.Dlg.Visible && Player == AoWHSMap.FSeatedPlayer)
        {
            RefreshMagicWindow(Window);
        }
    }

    // E3Globals.pas:177
    public static void RefreshGlobalMagic(TMagicWin Window)
    {
        TGlobalMagicControl Control;
        Control = AoWHSMap.FGlobalMagicControl;
        Window.GlobalEnchantmentList.Items.Clear();
        Window.GlobalNameList.Items.Clear();
        Window.GlobalUpkeepList.Items.Clear();
        for (int I = 0, last = Control.FEnchantments.Count - 1; I <= last; I++)
        {
            if (Control.FEnchantments.GetEnchantments(I).VisibleForPlayer(AoWHSMap.FSeatedPlayer))
            {
                var name = Control.FEnchantments.GetEnchantments(I).GetName();
                var enchantment = Control.FEnchantments.GetEnchantments(I);
                Window.GlobalEnchantmentList.Items.AddObject(name, enchantment);
                var enchantmentPlayerIndex = Control.FEnchantments.GetEnchantments(I);
                var player = AoWHSMap.FPlayers.GetPlayers(enchantmentPlayerIndex.FPlayerIndex);
                Window.GlobalNameList.Items.Add(player.FLeader.FHeroName);
                var upkeep = Control.FEnchantments.GetEnchantments(I).GetUpkeep();
                Window.GlobalUpkeepList.Items.Add(IntToStr(upkeep));
            }
        }

        Window.SortGlobalEnchantments();
        Window.SelectGlobalEnchantment(Window.GlobalEnchantmentList.Selected);
        Window.Dlg.FDrawn = false;
    }

    // E3Globals.pas:194
    public static void GlobalMagicChanged(TMagicWin Window)
    {
        if (Window.Dlg.Visible)
        {
            RefreshGlobalMagic(Window);
        }
    }

    // E3Globals.pas:109
    public static void ShowTitleScene(TTitleScene Scene, bool Music)
    {
        if (Music)
        {
            bool condition = SongPlayer.CurrentIndex == -1;
            if (!condition)
            {
                var count = SongPlayer.Songs.Count;
                condition = SongPlayer.CurrentIndex >= count;
            }

            if (condition || SongPlayer.Songs[SongPlayer.CurrentIndex] != "SONGS\\TITLE.IT")
            {
                SongPlayer.Songs.Clear();
                SongPlayer.Songs.Add("SONGS\\TITLE.IT");
                SongPlayer.Play(0);
            }
        }

        General.Manager.OverlayPriority = 0;
        Scene.TitleSceneIL.LoadDiscardableImageData();
        VideoDialog.SetSceneSize(640, 480);
        SetVideoMode(VideoDialog, TScreenMode.scmTitle);
        VideoDialog.ApplyMode();
        GameHost.MainForm.Display.FrameRate = 30;
        GameHost.MainForm.DXDC.UseVerticalBlank = true;
        Scene.FRedrawBackground = true;
        Scene.FPressedButton = -1;
        Scene.FHoverButton = -1;
        Scene.FFont = FontModule.AoW15WhiteGrey;
        Scene.FCreditsFont = FontModule.AoW15WhiteGrey;
        Scene.Window.WinManager.SetILBIndex(4);
        Scene.Window.Visible = true;
    }

    // E3Globals.pas:104
    public static void SetMainCursor(IGameHost Form, int Cursor)
    {
        Form.FCursor = (short)Math.Abs(Cursor);
    }

    // E3Globals.pas:95
    public static void ShowEmailSettings(TEmailSettingsDlg Dialog)
    {
        var localEmailAddress = AoWEngine.FRegistry.GetLocalEmailAddress();
        Dialog.LocalEmailAddressEdit.Text = localEmailAddress;
        var smtpServer = AoWEngine.FRegistry.GetSMTPServer();
        Dialog.SMTPServerEdit.Text = smtpServer;
        var attachmentDirectory = AoWEngine.FRegistry.GetAttachmentDirectory();
        Dialog.AttachmentDirectoryEdit.Text = attachmentDirectory;
        Dialog.Dlg.Visible = true;
    }

    public static readonly string DisbandUnitRStr = "Disband this unit?";
    public static readonly string ConfirmRStr = "Confirm";
    public static readonly string PartyActionRStr = "Party Action:";
    public static readonly string PatrollingRStr = "Patrolling";
    public static readonly string GuardingRStr = "Guarding";
    public static readonly string ScoutingRStr = "Scouting";
    public static readonly string SeekingRefugeRStr = "Seeking Refuge";
    public static readonly string RaidingRStr = "Raiding";
    public static readonly string HuntingRStr = "Hunting";
    public static readonly string ProtectingRStr = "Protecting";
    public static readonly string FortifiedRStr = "Fortified";
    public static readonly string BerserkRStr = "Berserk";
    public static readonly string WanderingRStr = "Wandering";
    public static readonly string PartyJoinRStr = "Party Join";
    public static readonly string XMadeALevelRStr = "%s made a level";
    public static readonly string XMadeYLevelsRStr = "%s made %d levels";
    public static FixedArray_string_3 TTacticalCombatAvailabilityRStr = FixedArray_string_3.From(["Never", "Between Humans", "Always"]);
    public static readonly string IndependentsRStr = "Independents";
    public static readonly string ItemBagRStr = "Item Bag";
    public static readonly string MapSettingsRStr = "Scenario Info:";
    public static readonly string DayLimitXRStr = "Turn Limit: %s";
    public static FixedArray_string_4 TVictoryConditionRStr = FixedArray_string_4.From(["None", "Conquer locations", "Accumulate gold", "Accumulate mana"]);
    public static FixedArray_string_3 TPlayerConnectionTypeRStr = FixedArray_string_3.From(["Single Computer", "Internet/Lan", "Play by email"]);
}
