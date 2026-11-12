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

    public int Fade { get; set; }
    public int FadeImage { get; private set; }
    public int FadeDirection { get; private set; }

    public ByteArray4 SelectionFlags;

    // AoWSelFactionWin.pas:63
    public void CloseSelection()
    {
        DialogHosts.UIGraphics.FactionWin.ReleaseDiscardableImageData();
        DialogHosts.TitleWindow.TitleWin.Visible = false;
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
        FactionPnl.Visible = false;
        Fade = 0;
    }

    // AoWSelFactionWin.pas:72
    public void LStartClick(int side)
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.UIGraphics.FactionWin.ReleaseDiscardableImageData();
        FactionPnl.Visible = false;
        Fade = 0;
        StartCampaignJoin(DialogHosts.CampaignJoinDialog, side);
    }

    // AoWSelFactionWin.pas:82
    public void LightPnlMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
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
            Fade = 100;
            FactionPnl.ImageLibraryIndexTexture = 2;
            FadeImage = 0;
            FadeDirection = 1;
        }
    }

    // AoWSelFactionWin.pas:100
    public void DarkPnlMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
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
            Fade = 100;
            FactionPnl.ImageLibraryIndexTexture = 1;
            FadeImage = 0;
            FadeDirection = 1;
        }
    }

    // AoWSelFactionWin.pas:118
    public void LBackClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        LBack.CaptureInput = false;
        LStart.CaptureInput = false;
        Fade = 100;
        FadeImage = 0;
        FadeDirection = 0;
    }

    // AoWSelFactionWin.pas:128
    public void DBackClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DBack.CaptureInput = false;
        DStart.CaptureInput = false;
        Fade = 100;
        FadeImage = 0;
        FadeDirection = 0;
    }

    // AoWSelFactionWin.pas:138
    public void FactionPnlMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (!LightPnl.Handled && !DName.Visible && !DarkPnl.Handled && !LName.Visible)
        {
            DSphere.Visible = false;
            LSphere.Visible = false;
            var chooseFaction = AbilityRules.TranslateResourceString(LobbyText.ChooseFactionText);
            TopLbl.Text = chooseFaction;
        }
    }

    // AoWSelFactionWin.pas:148
    public void LightPnlMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (!LName.Visible && !DSphere.Visible)
        {
            DSphere.Visible = true;
            LSphere.Visible = false;
            var cultOfStorms = AbilityRules.TranslateResourceString(LobbyText.CultOfStormsText);
            TopLbl.Text = cultOfStorms;
        }
    }

    // AoWSelFactionWin.pas:158
    public void DarkPnlMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (!DName.Visible && !LSphere.Visible)
        {
            DSphere.Visible = false;
            LSphere.Visible = true;
            var keepers = AbilityRules.TranslateResourceString(LobbyText.KeepersText);
            TopLbl.Text = keepers;
        }
    }

    // AoWSelFactionWin.pas:168
    public void AbortButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        CloseSelection();
    }

    // AoWSelFactionWin.pas:174
    public void FactionPnlUpdate()
    {
        Rect R;
        if (DialogHosts.FactionSelectionWindow.Fade >= 0)
        {
            DialogHosts.FactionSelectionWindow.Fade -= 6;
            if (Fade < 6)
            {
                if (LStart.Visible && !LStart.CaptureInput)
                {
                    LName.Visible = false;
                    LText.Visible = false;
                    LBack.Visible = false;
                    LStart.Visible = false;
                    LSphere.Visible = false;
                    TopLbl.Visible = true;
                    var chooseFaction = AbilityRules.TranslateResourceString(LobbyText.ChooseFactionText);
                    TopLbl.Text = chooseFaction;
                    FactionPnl.ImageLibraryIndexTexture = 0;
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
                        var chooseFaction = AbilityRules.TranslateResourceString(LobbyText.ChooseFactionText);
                        TopLbl.Text = chooseFaction;
                        FactionPnl.ImageLibraryIndexTexture = 0;
                    }
                }
            }

            R = new Rect(this.FactionPnl.WinLeft, this.FactionPnl.WinTop, this.FactionPnl.WinLeft + this.FactionPnl.WinWidth, FactionPnl.WinTop + this.FactionPnl.WinHeight);

            DialogHosts.TitleWindow.TitleWin.AddBlitRectangle(ref R, null);
        }
    }

    // AoWSelFactionWin.pas:210
    public void FactionPnlKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 27:
                CloseSelection();
                break;
        }
    }

    private static void StartCampaignJoin(CampaignJoinDialog dialog, int faction)
    {
        dialog.ResetCampaignJoin();
        dialog.Faction = faction;
        dialog.Stage = 0;
        StringList value = new StringList();
        switch (dialog.Faction)
        {
            case 0:
                LoadWorldCampaign(DialogHosts.WorldMap, "Keepers.cam");
                dialog.CharGenPnl.ImageLibraryIndexTexture = 10;
                AoWHSSet.Current.TextControl.RetrieveText(10, (StringCollection)(object)value);
                break;
            case 1:
                LoadWorldCampaign(DialogHosts.WorldMap, "Storm.cam");
                dialog.CharGenPnl.ImageLibraryIndexTexture = 11;
                AoWHSSet.Current.TextControl.RetrieveText(11, (StringCollection)(object)value);
                break;
        }
        ParseFontText(((StringCollection)value).Text, (StringCollection)(object)dialog.ScrollText, DialogHosts.UIFonts.Font15WhiteGrey, 300);
        WorldMapCampaign campaign = DialogHosts.WorldMap.GetCampaign();
        int id = WorldMapCampaign.GetCampaignScenario(campaign, 0).Paths[0];
        int index = campaign.Scenarios.IndexOfId(id);
        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(DialogHosts.WorldMap.GetCampaign(), index);
        AoWHSMap hexSpriteMap = (AoWHSMap)GameTables.GameEngine.CreatePreviewHsm(FileBoundary.ReadName(GameTables.GameEngine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario.FileName), GameProfile.WorldMap);
        int i;
        for (i = 0; i < hexSpriteMap.Count && hexSpriteMap.Players.GetPlayers(i).PlayerType != PlayerType.Human; i++)
        {
        }
        Player players = hexSpriteMap.Players.GetPlayers(i);
        dialog.Leader.Assign(players.LeaderSetup, GameTables.GameEngine);
        hexSpriteMap?.Dispose();
        if (value != null)
        {
            ((Persistent)value).Dispose();
        }
    }

    private static void LoadWorldCampaign(WorldMap dialog, string name)
    {
        GameTables.GameEngine.LoadCampaign(FileBoundary.ReadName(GameTables.GameEngine.StartupDirectory + "Scenario\\Campaign\\" + name));
    }

    private static void ParseFontText(string text, StringCollection strings, ImageLibraryFont font, int maxWidth)
    {
        int i = 1;
        int number = 0;
        string textLocal = "";
        int j;
        for (int length = text.Length; i <= length; i += j)
        {
            if (Set<byte>.WithBounds(0, 255).With((byte)13).With((byte)32)
                .Contains(Cp1252.Byte(text[i - 1])))
            {
                i++;
            }
            for (j = 0; i + j <= length && !Set<byte>.WithBounds(0, 255).With((byte)10).With((byte)13)
                .With((byte)32)
                .Contains(Cp1252.Byte(text[i + j - 1])); j++)
            {
            }
            string text2 = RtlUnit.Copy(text, i, j);
            int textWidth = font.GetTextWidth(text2);
            if (number + textWidth > maxWidth && !RtlUnit.IsEmpty(textLocal))
            {
                strings.Add(textLocal);
                textLocal = "";
                number = 0;
            }
            if (!RtlUnit.IsEmpty(textLocal))
            {
                textLocal = textLocal + " " + text2;
                number = number + textWidth + font.GetTextWidth(" ");
            }
            else
            {
                textLocal += text2;
                number += textWidth;
            }
            if (i + j <= length && text[i + j - 1] == '\n')
            {
                strings.Add(textLocal);
                textLocal = "";
                number = 0;
                i++;
            }
        }
        if (!RtlUnit.IsEmpty(textLocal))
        {
            strings.Add(textLocal);
        }
    }
}
