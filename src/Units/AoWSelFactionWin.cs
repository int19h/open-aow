// Translated from AoWSelFactionWin.pas.
using Aow.Game.Engine.Buffers;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWSelFactionWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class TSelFactionWin : TDataModule
{
    public TSelFactionWin(TComponent owner) : base(owner)
    { }

    public TAOWPanel FactionPnl;
    public TAOWLabel TopLbl;
    public TAOWPanel LightPnl;
    public TAOWMemo LText;
    public TAOWButton LStart;
    public TAOWButton LBack;
    public TAOWImage LSphere;
    public TAOWPanel DarkPnl;
    public TAOWButton DStart;
    public TAOWButton DBack;
    public TAOWLabel LName;
    public TAOWMemo DText;
    public TAOWLabel DName;
    public TAOWImage DSphere;
    public TIvTranslator Translator;
    public int FFade;
    public int FFadeImage;
    public int FFadeDirection;
    public FixedArray_byte_4 F_SelFactionWin_8C;

    // AoWSelFactionWin.pas:63
    public void CloseSelection()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.FactionWin.ReleaseDiscardableImageData();
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
        Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(Aow.Game.Ui.Dialogs.DialogHosts.TitleScene, true);
        FactionPnl.Visible = false;
        FFade = 0;
    }

    // AoWSelFactionWin.pas:72
    public void LStartClick(int Side)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.FactionWin.ReleaseDiscardableImageData();
        FactionPnl.Visible = false;
        FFade = 0;
        Aow.Game.Ui.Dialogs.TSelFactionWin.StartCampaignJoin(Aow.Game.Ui.Dialogs.DialogHosts.CampaignJoinDlg, Side);
    }

    // AoWSelFactionWin.pas:82
    public void LightPnlMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!DName.Visible && !LName.Visible)
        {
            DName.Visible = true;
            DText.Visible = true;
            DBack.Visible = true;
            DStart.Visible = true;
            DBack.CaptureInput = true;
            DStart.CaptureInput = true;
            TopLbl.Visible = false;
            FFade = 100;
            FactionPnl.ILIndexTexture = 2;
            FFadeImage = 0;
            FFadeDirection = 1;
        }
    }

    // AoWSelFactionWin.pas:100
    public void DarkPnlMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!LName.Visible && !DName.Visible)
        {
            LName.Visible = true;
            LText.Visible = true;
            LBack.Visible = true;
            LStart.Visible = true;
            LBack.CaptureInput = true;
            LStart.CaptureInput = true;
            TopLbl.Visible = false;
            FFade = 100;
            FactionPnl.ILIndexTexture = 1;
            FFadeImage = 0;
            FFadeDirection = 1;
        }
    }

    // AoWSelFactionWin.pas:118
    public void LBackClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        LBack.CaptureInput = false;
        LStart.CaptureInput = false;
        FFade = 100;
        FFadeImage = 0;
        FFadeDirection = 0;
    }

    // AoWSelFactionWin.pas:128
    public void DBackClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        DBack.CaptureInput = false;
        DStart.CaptureInput = false;
        FFade = 100;
        FFadeImage = 0;
        FFadeDirection = 0;
    }

    // AoWSelFactionWin.pas:138
    public void FactionPnlMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        if (!LightPnl.FHandled && !DName.Visible && !DarkPnl.FHandled && !LName.Visible)
        {
            DSphere.Visible = false;
            LSphere.Visible = false;
            var chooseFaction = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.ChooseFactionRStr);
            TopLbl.Text = chooseFaction;
        }
    }

    // AoWSelFactionWin.pas:148
    public void LightPnlMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        if (!LName.Visible && !DSphere.Visible)
        {
            DSphere.Visible = true;
            LSphere.Visible = false;
            var cultOfStorms = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.CultOfStormsRStr);
            TopLbl.Text = cultOfStorms;
        }
    }

    // AoWSelFactionWin.pas:158
    public void DarkPnlMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        if (!DName.Visible && !LSphere.Visible)
        {
            DSphere.Visible = false;
            LSphere.Visible = true;
            var keepers = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.KeepersRStr);
            TopLbl.Text = keepers;
        }
    }

    // AoWSelFactionWin.pas:168
    public void AbortBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        CloseSelection();
    }

    // AoWSelFactionWin.pas:174
    public void FactionPnlUpdate()
    {
        TRect R;
        if (Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FFade >= 0)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FFade -= 6;
            if (FFade < 6)
            {
                if (LStart.Visible && !LStart.CaptureInput)
                {
                    LName.Visible = false;
                    LText.Visible = false;
                    LBack.Visible = false;
                    LStart.Visible = false;
                    LSphere.Visible = false;
                    TopLbl.Visible = true;
                    var chooseFaction = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.ChooseFactionRStr);
                    TopLbl.Text = chooseFaction;
                    FactionPnl.ILIndexTexture = 0;
                }
                else
                {
                    if (DStart.Visible && !DStart.CaptureInput)
                    {
                        DName.Visible = false;
                        DText.Visible = false;
                        DBack.Visible = false;
                        DStart.Visible = false;
                        DSphere.Visible = false;
                        TopLbl.Visible = true;
                        var chooseFaction = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.ChooseFactionRStr);
                        TopLbl.Text = chooseFaction;
                        FactionPnl.ILIndexTexture = 0;
                    }
                }
            }

            R = Rect(this.FactionPnl.WinLeft, this.FactionPnl.WinTop, this.FactionPnl.WinLeft + this.FactionPnl.WinWidth, FactionPnl.WinTop + this.FactionPnl.WinHeight);

            Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.AddBltRect(ref R, null);
        }
    }

    // AoWSelFactionWin.pas:210
    public void FactionPnlKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        switch (Key)
        {
            case 27:
                CloseSelection();
                break;
        }
    }

    private static void StartCampaignJoin(global::Aow.Game.Ui.Dialogs.TCampaignJoinDlg Dialog, int Faction)
    {
        Dialog.ResetCampaignJoin();
        Dialog.FFaction = Faction;
        Dialog.FStage = 0;
        TStringList val = new TStringList();
        switch (Dialog.FFaction)
        {
            case 0:
                LoadWorldCampaign(global::Aow.Game.Ui.Dialogs.DialogHosts.WorldMap, "Keepers.cam");
                Dialog.CharGenPnl.ILIndexTexture = 10;
                global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FTextControl.RetrieveText(10, (TStrings)(object)val);
                break;
            case 1:
                LoadWorldCampaign(global::Aow.Game.Ui.Dialogs.DialogHosts.WorldMap, "Storm.cam");
                Dialog.CharGenPnl.ILIndexTexture = 11;
                global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FTextControl.RetrieveText(11, (TStrings)(object)val);
                break;
        }
        ParseFontText(((TStrings)val).Text, (TStrings)(object)Dialog.FScrollText, global::Aow.Game.Ui.Dialogs.DialogHosts.FontModule.AoW15WhiteGrey, 300);
        global::Aow.Game.Session.TWMCampaign campaign = global::Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.GetCampaign();
        int iD = global::Aow.Game.Session.TWMCampaign.GetCampaignScenario(campaign, 0).FPaths[0];
        int index = campaign.FScenarios.IndexOfID(iD);
        global::Aow.Game.Session.TWMCampaignScenario campaignScenario = global::Aow.Game.Session.TWMCampaign.GetCampaignScenario(global::Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.GetCampaign(), index);
        global::Aow.Game.World.Map.TAoWHSMap tAoWHSMap = (global::Aow.Game.World.Map.TAoWHSMap)global::Aow.Game.Magic.Abilities.GameTables.AoWEngine.CreatePreviewHSM(global::Aow.Game.Saves.FileBoundary.ReadName(global::Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario.FFileName), Aow.Formats.GameProfile.WorldMap);
        int i;
        for (i = 0; i < tAoWHSMap.Count && tAoWHSMap.FPlayers.GetPlayers(i).FPlayerType != global::Aow.Game.Session.TPlayerType.ptHuman; i++)
        {
        }
        global::Aow.Game.Session.TPlayer players = tAoWHSMap.FPlayers.GetPlayers(i);
        Dialog.FLeader.Assign(players.FLeaderSetup, global::Aow.Game.Magic.Abilities.GameTables.AoWEngine);
        tAoWHSMap?.Dispose();
        if (val != null)
        {
            ((TPersistent)val).Dispose();
        }
    }

    private static void LoadWorldCampaign(global::Aow.Game.Ui.Dialogs.TWorldMap Dialog, string Name)
    {
        global::Aow.Game.Magic.Abilities.GameTables.AoWEngine.LoadCampaign(global::Aow.Game.Saves.FileBoundary.ReadName(global::Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\" + Name));
    }

    private static void ParseFontText(string Text, TStrings Strings, global::Aow.Game.Draw.Images.TImageLibraryFont Font, int MaxWidth)
    {
        int i = 1;
        int num = 0;
        string text = "";
        int j;
        for (int length = Text.Length; i <= length; i += j)
        {
            if (TSet<byte>.WithBounds(0, 255).With((byte)13).With((byte)32)
                .Contains(Cp1252.Byte(Text[i - 1])))
            {
                i++;
            }
            for (j = 0; i + j <= length && !TSet<byte>.WithBounds(0, 255).With((byte)10).With((byte)13)
                .With((byte)32)
                .Contains(Cp1252.Byte(Text[i + j - 1])); j++)
            {
            }
            string text2 = RtlUnit.Copy(Text, i, j);
            int textWidth = Font.GetTextWidth(text2);
            if (num + textWidth > MaxWidth && !RtlUnit.IsEmpty(text))
            {
                Strings.Add(text);
                text = "";
                num = 0;
            }
            if (!RtlUnit.IsEmpty(text))
            {
                text = text + " " + text2;
                num = num + textWidth + Font.GetTextWidth(" ");
            }
            else
            {
                text += text2;
                num += textWidth;
            }
            if (i + j <= length && Text[i + j - 1] == '\n')
            {
                Strings.Add(text);
                text = "";
                num = 0;
                i++;
            }
        }
        if (!RtlUnit.IsEmpty(text))
        {
            Strings.Add(text);
        }
    }
}
