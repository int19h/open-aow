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

public partial class SelFactionWin : DataModule
{
    public SelFactionWin(Component owner) : base(owner)
    { }

    public AOWPanel FactionPnl;
    public AOWLabel TopLbl;
    public AOWPanel LightPnl;
    public AOWMemo LText;
    public AOWButton LStart;
    public AOWButton LBack;
    public AOWImage LSphere;
    public AOWPanel DarkPnl;
    public AOWButton DStart;
    public AOWButton DBack;
    public AOWLabel LName;
    public AOWMemo DText;
    public AOWLabel DName;
    public AOWImage DSphere;
    public IvTranslator Translator;
    public int FFade;
    public int FFadeImage;
    public int FFadeDirection;
    public FixedArray_byte_4 F_SelFactionWin_8C;

    // AoWSelFactionWin.pas:63
    public void CloseSelection()
    {
        DialogHosts.IntGfxMod.FactionWin.ReleaseDiscardableImageData();
        DialogHosts.TitleWindow.TitleWin.Visible = false;
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
        FactionPnl.Visible = false;
        FFade = 0;
    }

    // AoWSelFactionWin.pas:72
    public void LStartClick(int Side)
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.IntGfxMod.FactionWin.ReleaseDiscardableImageData();
        FactionPnl.Visible = false;
        FFade = 0;
        StartCampaignJoin(DialogHosts.CampaignJoinDlg, Side);
    }

    // AoWSelFactionWin.pas:82
    public void LightPnlMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
    public void DarkPnlMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
        DialogHosts.General.PlayButtonSound(0);
        LBack.CaptureInput = false;
        LStart.CaptureInput = false;
        FFade = 100;
        FFadeImage = 0;
        FFadeDirection = 0;
    }

    // AoWSelFactionWin.pas:128
    public void DBackClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DBack.CaptureInput = false;
        DStart.CaptureInput = false;
        FFade = 100;
        FFadeImage = 0;
        FFadeDirection = 0;
    }

    // AoWSelFactionWin.pas:138
    public void FactionPnlMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        if (!LightPnl.FHandled && !DName.Visible && !DarkPnl.FHandled && !LName.Visible)
        {
            DSphere.Visible = false;
            LSphere.Visible = false;
            var chooseFaction = Magic.Abilities.AbilityRules.TranslateRStr(LobbyText.ChooseFactionRStr);
            TopLbl.Text = chooseFaction;
        }
    }

    // AoWSelFactionWin.pas:148
    public void LightPnlMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        if (!LName.Visible && !DSphere.Visible)
        {
            DSphere.Visible = true;
            LSphere.Visible = false;
            var cultOfStorms = Magic.Abilities.AbilityRules.TranslateRStr(LobbyText.CultOfStormsRStr);
            TopLbl.Text = cultOfStorms;
        }
    }

    // AoWSelFactionWin.pas:158
    public void DarkPnlMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        if (!DName.Visible && !LSphere.Visible)
        {
            DSphere.Visible = false;
            LSphere.Visible = true;
            var keepers = Magic.Abilities.AbilityRules.TranslateRStr(LobbyText.KeepersRStr);
            TopLbl.Text = keepers;
        }
    }

    // AoWSelFactionWin.pas:168
    public void AbortBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        CloseSelection();
    }

    // AoWSelFactionWin.pas:174
    public void FactionPnlUpdate()
    {
        Rect R;
        if (DialogHosts.SelFactionWin.FFade >= 0)
        {
            DialogHosts.SelFactionWin.FFade -= 6;
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
                    var chooseFaction = Magic.Abilities.AbilityRules.TranslateRStr(LobbyText.ChooseFactionRStr);
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
                        var chooseFaction = Magic.Abilities.AbilityRules.TranslateRStr(LobbyText.ChooseFactionRStr);
                        TopLbl.Text = chooseFaction;
                        FactionPnl.ILIndexTexture = 0;
                    }
                }
            }

            R = new Rect(this.FactionPnl.WinLeft, this.FactionPnl.WinTop, this.FactionPnl.WinLeft + this.FactionPnl.WinWidth, FactionPnl.WinTop + this.FactionPnl.WinHeight);

            DialogHosts.TitleWindow.TitleWin.AddBltRect(ref R, null);
        }
    }

    // AoWSelFactionWin.pas:210
    public void FactionPnlKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        switch (Key)
        {
            case 27:
                CloseSelection();
                break;
        }
    }

    private static void StartCampaignJoin(CampaignJoinDlg Dialog, int Faction)
    {
        Dialog.ResetCampaignJoin();
        Dialog.FFaction = Faction;
        Dialog.FStage = 0;
        StringList val = new StringList();
        switch (Dialog.FFaction)
        {
            case 0:
                LoadWorldCampaign(DialogHosts.WorldMap, "Keepers.cam");
                Dialog.CharGenPnl.ILIndexTexture = 10;
                Engine.AoWHSSet.Current.FTextControl.RetrieveText(10, (StringCollection)(object)val);
                break;
            case 1:
                LoadWorldCampaign(DialogHosts.WorldMap, "Storm.cam");
                Dialog.CharGenPnl.ILIndexTexture = 11;
                Engine.AoWHSSet.Current.FTextControl.RetrieveText(11, (StringCollection)(object)val);
                break;
        }
        ParseFontText(((StringCollection)val).Text, (StringCollection)(object)Dialog.FScrollText, DialogHosts.FontModule.AoW15WhiteGrey, 300);
        Session.WMCampaign campaign = DialogHosts.WorldMap.GetCampaign();
        int iD = Session.WMCampaign.GetCampaignScenario(campaign, 0).FPaths[0];
        int index = campaign.FScenarios.IndexOfID(iD);
        Session.WMCampaignScenario campaignScenario = Session.WMCampaign.GetCampaignScenario(DialogHosts.WorldMap.GetCampaign(), index);
        World.Map.AoWHSMap tAoWHSMap = (World.Map.AoWHSMap)Magic.Abilities.GameTables.AoWEngine.CreatePreviewHSM(Saves.FileBoundary.ReadName(Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario.FFileName), Aow.Formats.GameProfile.WorldMap);
        int i;
        for (i = 0; i < tAoWHSMap.Count && tAoWHSMap.FPlayers.GetPlayers(i).FPlayerType != Session.PlayerType.Human; i++)
        {
        }
        Session.Player players = tAoWHSMap.FPlayers.GetPlayers(i);
        Dialog.FLeader.Assign(players.FLeaderSetup, Magic.Abilities.GameTables.AoWEngine);
        tAoWHSMap?.Dispose();
        if (val != null)
        {
            ((Persistent)val).Dispose();
        }
    }

    private static void LoadWorldCampaign(WorldMap Dialog, string Name)
    {
        Magic.Abilities.GameTables.AoWEngine.LoadCampaign(Saves.FileBoundary.ReadName(Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\" + Name));
    }

    private static void ParseFontText(string Text, StringCollection Strings, Draw.Images.ImageLibraryFont Font, int MaxWidth)
    {
        int i = 1;
        int num = 0;
        string text = "";
        int j;
        for (int length = Text.Length; i <= length; i += j)
        {
            if (Set<byte>.WithBounds(0, 255).With((byte)13).With((byte)32)
                .Contains(Cp1252.Byte(Text[i - 1])))
            {
                i++;
            }
            for (j = 0; i + j <= length && !Set<byte>.WithBounds(0, 255).With((byte)10).With((byte)13)
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
