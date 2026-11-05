// Translated from E3Globals.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWUnitSelDlg;
using Aow.Units.AoWArmyInfoDlg;
using Aow.Units.AoWDisjunctionDlg;
using Aow.Units.AoWInfoWin;
using Aow.Units.AoWMagic;
using Aow.Units.AoWEmailSettingsDlg;
using Aow.Units.AoWEmailGameSettingsDlg;
using Aow.Units.AoWTitleScene;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWPOP3Dlg;
using Aow.Units.AoWCastUnitSpellDlg;
using Aow.Units.AoWDeformFaceDlg;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.Sound.Sound;
using Aow.Platform;

namespace Aow.Units.E3Globals
{
    public static partial class E3Globals
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
            TPlayerMagicControl Control = default;
            int I = default;
            int Turns = default;
            TSpell Spell = default;
            if (AoWHSMap.FSeatedPlayer != -1)
            {
                var call__1 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                Control = call__1.FMagicControl;
                Window.EnchantmentList.Items.Clear();
                Window.BindingList.Items.Clear();
                Window.UpkeepList.Items.Clear();
                {
                    I = 0;
                    int end__2 = Control.FEnchantments.Count - 1;
                    for (; I <= end__2; I += 1)
                    {
                        var call__3 = Control.FEnchantments.GetEnchantments(I);
                        var call__4 = call__3.GetName();
                        var call__5 = Control.FEnchantments.GetEnchantments(I);
                        Window.EnchantmentList.Items.AddObject(call__4, call__5);
                        var call__6 = Control.FEnchantments.GetEnchantments(I);
                        var call__7 = call__6.GetBinding();
                        Window.BindingList.Items.Add(call__7);
                        var call__8 = Control.FEnchantments.GetEnchantments(I);
                        Window.UpkeepList.Items.Add(IntToStr(call__8.FUpkeep));
                    }
                }

                Window.PowerSourceList.Items.Clear();
                Window.PowerValueList.Items.Clear();
                {
                    I = 0;
                    int end__9 = Control.FPowerSources.Count - 1;
                    for (; I <= end__9; I += 1)
                    {
                        var call__10 = Control.FPowerSources.GetItems(I);
                        var call__11 = call__10.Power();
                        if (call__11 > 0)
                        {
                            var call__12 = Control.FPowerSources.GetItems(I);
                            var call__13 = call__12.Name();
                            var call__14 = Control.FPowerSources.GetItems(I);
                            Window.PowerSourceList.Items.AddObject(call__13, call__14);
                            var call__15 = Control.FPowerSources.GetItems(I);
                            var call__16 = call__15.Power();
                            Window.PowerValueList.Items.Add(IntToStr(call__16));
                        }
                    }
                }

                var call__17 = Control.GetPower();
                Window.Power.Text = IntToStr(call__17);
                var call__18 = Control.GetManaIncome();
                var call__19 = Control.GetManaUpkeep();
                var call__20 = Control.GetManaIncome();
                var call__21 = Control.GetManaUpkeep();
                Window.NetMana.Text = IntToStr(call__18) + AnsiString.FromBytes(45) + IntToStr(call__19) + AnsiString.FromBytes(61) + IntToStr(call__20 - call__21);
                var call__22 = Control.GetNetPowerToResearch();
                Window.ResearchPoints.Text = IntToStr(call__22);
                if (Control.FResearchSpellID != 0)
                {
                    var call__23 = AoWHSSet.FSpellControl.GetSpell(Control.FResearchSpellID);
                    Spell = call__23;
                    Window.SpellName.Text = Spell.FSpellName;
                    var call__24 = TranslateRStr(TMagicSphereRStr[(int)Spell.FMagicSphere]);
                    var call__25 = TranslateRStr(LevelRStr);
                    Window.SpellClass.Text = call__24 + (AnsiString)", " + call__25 + AnsiString.FromBytes(32) + IntToStr(Spell.FSpellLevel);
                    var call__26 = TranslateRStr(CastingCostRStr);
                    var call__27 = TranslateRStr(ManaRStr);
                    Window.SpellCost.Text = call__26 + (AnsiString)": " + IntToStr(Spell.FBaseManaCost) + AnsiString.FromBytes(32) + call__27;
                    var call__28 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__29 = call__28.FMagicControl.GetResearchTurnsLeft();
                    Turns = call__29;
                    if (Turns < 999)
                    {
                        var call__30 = FmtTurnsLeftStr(Turns);
                        Window.ResearchTurnsLeft.Text = call__30;
                    }
                    else
                    {
                        var call__31 = TranslateRStr(XTurnsRStr);
                        var call__32 = AoWE.AoWE.Format(call__31, [(byte)63]);
                        Window.ResearchTurnsLeft.Text = call__32;
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
            TGlobalMagicControl Control = default;
            int I = default;
            Control = AoWHSMap.FGlobalMagicControl;
            Window.GlobalEnchantmentList.Items.Clear();
            Window.GlobalNameList.Items.Clear();
            Window.GlobalUpkeepList.Items.Clear();
            {
                I = 0;
                int end__33 = Control.FEnchantments.Count - 1;
                for (; I <= end__33; I += 1)
                {
                    var call__34 = Control.FEnchantments.GetEnchantments(I);
                    var call__35 = call__34.VisibleForPlayer(AoWHSMap.FSeatedPlayer);
                    if (call__35)
                    {
                        var call__36 = Control.FEnchantments.GetEnchantments(I);
                        var call__37 = call__36.GetName();
                        var call__38 = Control.FEnchantments.GetEnchantments(I);
                        Window.GlobalEnchantmentList.Items.AddObject(call__37, call__38);
                        var call__39 = Control.FEnchantments.GetEnchantments(I);
                        var call__40 = AoWHSMap.FPlayers.GetPlayers(call__39.FPlayerIndex);
                        Window.GlobalNameList.Items.Add(call__40.FLeader.FHeroName);
                        var call__41 = Control.FEnchantments.GetEnchantments(I);
                        var call__42 = call__41.GetUpkeep();
                        Window.GlobalUpkeepList.Items.Add(IntToStr(call__42));
                    }
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
                bool bool__43 = SongPlayer.CurrentIndex == -1;
                if (!bool__43)
                {
                    var call__44 = SongPlayer.Songs.Count;
                    bool__43 = SongPlayer.CurrentIndex >= call__44;
                }

                bool bool__45 = bool__43;
                if (!bool__45)
                {
                    var call__46 = SongPlayer.Songs[SongPlayer.CurrentIndex];
                    bool__45 = call__46 != (AnsiString)"SONGS\\TITLE.IT";
                }

                if (bool__45)
                {
                    SongPlayer.Songs.Clear();
                    SongPlayer.Songs.Add((AnsiString)"SONGS\\TITLE.IT");
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
            var call__47 = AoWEngine.FRegistry.GetLocalEmailAddress();
            Dialog.LocalEmailAddressEdit.Text = call__47;
            var call__48 = AoWEngine.FRegistry.GetSMTPServer();
            Dialog.SMTPServerEdit.Text = call__48;
            var call__49 = AoWEngine.FRegistry.GetAttachmentDirectory();
            Dialog.AttachmentDirectoryEdit.Text = call__49;
            Dialog.Dlg.Visible = true;
        }

        public static readonly AnsiString DisbandUnitRStr = (AnsiString)"Disband this unit?";
        public static readonly AnsiString ConfirmRStr = (AnsiString)"Confirm";
        public static readonly AnsiString PartyActionRStr = (AnsiString)"Party Action:";
        public static readonly AnsiString PatrollingRStr = (AnsiString)"Patrolling";
        public static readonly AnsiString GuardingRStr = (AnsiString)"Guarding";
        public static readonly AnsiString ScoutingRStr = (AnsiString)"Scouting";
        public static readonly AnsiString SeekingRefugeRStr = (AnsiString)"Seeking Refuge";
        public static readonly AnsiString RaidingRStr = (AnsiString)"Raiding";
        public static readonly AnsiString HuntingRStr = (AnsiString)"Hunting";
        public static readonly AnsiString ProtectingRStr = (AnsiString)"Protecting";
        public static readonly AnsiString FortifiedRStr = (AnsiString)"Fortified";
        public static readonly AnsiString BerserkRStr = (AnsiString)"Berserk";
        public static readonly AnsiString WanderingRStr = (AnsiString)"Wandering";
        public static readonly AnsiString PartyJoinRStr = (AnsiString)"Party Join";
        public static readonly AnsiString XMadeALevelRStr = (AnsiString)"%s made a level";
        public static readonly AnsiString XMadeYLevelsRStr = (AnsiString)"%s made %d levels";
        public static FixedArray_Aow_Rtl_AnsiString_3 TTacticalCombatAvailabilityRStr = FixedArray_Aow_Rtl_AnsiString_3.From([(AnsiString)"Never", (AnsiString)"Between Humans", (AnsiString)"Always"]);
        public static readonly AnsiString IndependentsRStr = (AnsiString)"Independents";
        public static readonly AnsiString ItemBagRStr = (AnsiString)"Item Bag";
        public static readonly AnsiString MapSettingsRStr = (AnsiString)"Scenario Info:";
        public static readonly AnsiString DayLimitXRStr = (AnsiString)"Turn Limit: %s";
        public static FixedArray_Aow_Rtl_AnsiString_4 TVictoryConditionRStr = FixedArray_Aow_Rtl_AnsiString_4.From([(AnsiString)"None", (AnsiString)"Conquer locations", (AnsiString)"Accumulate gold", (AnsiString)"Accumulate mana"]);
        public static FixedArray_Aow_Rtl_AnsiString_3 TPlayerConnectionTypeRStr = FixedArray_Aow_Rtl_AnsiString_3.From([(AnsiString)"Single Computer", (AnsiString)"Internet/Lan", (AnsiString)"Play by email"]);
    }
}
