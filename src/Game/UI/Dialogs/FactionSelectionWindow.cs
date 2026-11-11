using Aow.Engine.Buffers;
using Aow.Formats;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class FactionSelectionWindow : DataModule
{
    public FactionSelectionWindow(Component owner) : base(owner)
    { }

    public Panel FactionPnl;
    public Label TopLbl;
    public Panel LightPnl;
    public Memo LText;
    public Button LStart;
    public Button LBack;
    public Image LSphere;
    public Panel DarkPnl;
    public Button DStart;
    public Button DBack;
    public Label LName;
    public Memo DText;
    public Label DName;
    public Image DSphere;
    public IvTranslator Translator;
    public int FFade;
    public int FFadeImage;
    public int FFadeDirection;
    public ByteArray4 F_SelFactionWin_8C;

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
            var chooseFaction = AbilityRules.TranslateRStr(LobbyText.ChooseFactionRStr);
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
            var cultOfStorms = AbilityRules.TranslateRStr(LobbyText.CultOfStormsRStr);
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
            var keepers = AbilityRules.TranslateRStr(LobbyText.KeepersRStr);
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
                    var chooseFaction = AbilityRules.TranslateRStr(LobbyText.ChooseFactionRStr);
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
                        var chooseFaction = AbilityRules.TranslateRStr(LobbyText.ChooseFactionRStr);
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

    private static void StartCampaignJoin(CampaignJoinDialog Dialog, int Faction)
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
                AoWHSSet.Current.FTextControl.RetrieveText(10, (StringCollection)(object)val);
                break;
            case 1:
                LoadWorldCampaign(DialogHosts.WorldMap, "Storm.cam");
                Dialog.CharGenPnl.ILIndexTexture = 11;
                AoWHSSet.Current.FTextControl.RetrieveText(11, (StringCollection)(object)val);
                break;
        }
        ParseFontText(((StringCollection)val).Text, (StringCollection)(object)Dialog.FScrollText, DialogHosts.FontModule.AoW15WhiteGrey, 300);
        WorldMapCampaign campaign = DialogHosts.WorldMap.GetCampaign();
        int iD = WorldMapCampaign.GetCampaignScenario(campaign, 0).FPaths[0];
        int index = campaign.FScenarios.IndexOfID(iD);
        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(DialogHosts.WorldMap.GetCampaign(), index);
        AoWHSMap tAoWHSMap = (AoWHSMap)GameTables.AoWEngine.CreatePreviewHSM(FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario.FFileName), GameProfile.WorldMap);
        int i;
        for (i = 0; i < tAoWHSMap.Count && tAoWHSMap.FPlayers.GetPlayers(i).FPlayerType != PlayerType.Human; i++)
        {
        }
        Player players = tAoWHSMap.FPlayers.GetPlayers(i);
        Dialog.FLeader.Assign(players.FLeaderSetup, GameTables.AoWEngine);
        tAoWHSMap?.Dispose();
        if (val != null)
        {
            ((Persistent)val).Dispose();
        }
    }

    private static void LoadWorldCampaign(WorldMap Dialog, string Name)
    {
        GameTables.AoWEngine.LoadCampaign(FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\" + Name));
    }

    private static void ParseFontText(string Text, StringCollection Strings, ImageLibraryFont Font, int MaxWidth)
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
