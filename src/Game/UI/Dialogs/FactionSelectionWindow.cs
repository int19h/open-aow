
using Aow.Formats;
using Aow.Game.Draw.Images;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class FactionSelectionWindow : UIModule
{
    public FactionSelectionWindow(UIContext ui) : base(ui)
    { }

    public Panel FactionPanel;
    public Label TopLabel;
    public Panel LightPanel;
    public Memo LightTextMemo;
    public Button LightStartButton;
    public Button LightBackButton;
    public Image LightSphereImage;
    public Panel DarkPanel;
    public Button DarkStartButton;
    public Button DarkBackButton;
    public Label LightNameLabel;
    public Memo DarkTextMemo;
    public Label DarkNameLabel;
    public Image DarkSphereImage;

    public int Fade { get; set; }
    public int FadeImage { get; private set; }
    public int FadeDirection { get; private set; }

    public byte[] SelectionFlags { get; } = new byte[4];

    // AoWSelFactionWin.pas:63
    public void CloseSelection()
    {
        UI.Graphics.FactionWindow.ReleaseDiscardableImageData();
        UI.TitleWindow.MainWindow.Visible = false;
        WindowActions.ShowTitleScene(UI.TitleScene, true);
        FactionPanel.Visible = false;
        Fade = 0;
    }

    // AoWSelFactionWin.pas:72
    public void LStartClick(int side)
    {
        UI.General.PlayButtonSound(0);
        UI.Graphics.FactionWindow.ReleaseDiscardableImageData();
        FactionPanel.Visible = false;
        Fade = 0;
        StartCampaignJoin(UI.CampaignJoinDialog, side);
    }

    // AoWSelFactionWin.pas:82
    public void LightPnlMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!DarkNameLabel.Visible && !LightNameLabel.Visible)
        {
            DarkNameLabel.Visible = true;
            DarkTextMemo.Visible = true;
            DarkBackButton.Visible = true;
            DarkStartButton.Visible = true;
            DarkBackButton.CaptureInput = true;
            DarkStartButton.CaptureInput = true;
            TopLabel.Visible = false;
            Fade = 100;
            FactionPanel.ImageLibraryIndexTexture = 2;
            FadeImage = 0;
            FadeDirection = 1;
        }
    }

    // AoWSelFactionWin.pas:100
    public void DarkPnlMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!LightNameLabel.Visible && !DarkNameLabel.Visible)
        {
            LightNameLabel.Visible = true;
            LightTextMemo.Visible = true;
            LightBackButton.Visible = true;
            LightStartButton.Visible = true;
            LightBackButton.CaptureInput = true;
            LightStartButton.CaptureInput = true;
            TopLabel.Visible = false;
            Fade = 100;
            FactionPanel.ImageLibraryIndexTexture = 1;
            FadeImage = 0;
            FadeDirection = 1;
        }
    }

    // AoWSelFactionWin.pas:118
    public void LBackClick()
    {
        UI.General.PlayButtonSound(0);
        LightBackButton.CaptureInput = false;
        LightStartButton.CaptureInput = false;
        Fade = 100;
        FadeImage = 0;
        FadeDirection = 0;
    }

    // AoWSelFactionWin.pas:128
    public void DBackClick()
    {
        UI.General.PlayButtonSound(0);
        DarkBackButton.CaptureInput = false;
        DarkStartButton.CaptureInput = false;
        Fade = 100;
        FadeImage = 0;
        FadeDirection = 0;
    }

    // AoWSelFactionWin.pas:138
    public void FactionPnlMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (!LightPanel.Handled && !DarkNameLabel.Visible && !DarkPanel.Handled && !LightNameLabel.Visible)
        {
            DarkSphereImage.Visible = false;
            LightSphereImage.Visible = false;
            var chooseFaction = UI.Engine.TranslateResourceString(LobbyText.ChooseFactionText);
            TopLabel.Text = chooseFaction;
        }
    }

    // AoWSelFactionWin.pas:148
    public void LightPnlMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (!LightNameLabel.Visible && !DarkSphereImage.Visible)
        {
            DarkSphereImage.Visible = true;
            LightSphereImage.Visible = false;
            var cultOfStorms = UI.Engine.TranslateResourceString(LobbyText.CultOfStormsText);
            TopLabel.Text = cultOfStorms;
        }
    }

    // AoWSelFactionWin.pas:158
    public void DarkPnlMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (!DarkNameLabel.Visible && !LightSphereImage.Visible)
        {
            DarkSphereImage.Visible = false;
            LightSphereImage.Visible = true;
            var keepers = UI.Engine.TranslateResourceString(LobbyText.KeepersText);
            TopLabel.Text = keepers;
        }
    }

    // AoWSelFactionWin.pas:168
    public void AbortButtonClick()
    {
        UI.General.PlayButtonSound(0);
        CloseSelection();
    }

    // AoWSelFactionWin.pas:174
    public void FactionPnlUpdate()
    {
        Rect R;
        if (UI.FactionSelectionWindow.Fade >= 0)
        {
            UI.FactionSelectionWindow.Fade -= 6;
            if (Fade < 6)
            {
                if (LightStartButton.Visible && !LightStartButton.CaptureInput)
                {
                    LightNameLabel.Visible = false;
                    LightTextMemo.Visible = false;
                    LightBackButton.Visible = false;
                    LightStartButton.Visible = false;
                    LightSphereImage.Visible = false;
                    TopLabel.Visible = true;
                    var chooseFaction = UI.Engine.TranslateResourceString(LobbyText.ChooseFactionText);
                    TopLabel.Text = chooseFaction;
                    FactionPanel.ImageLibraryIndexTexture = 0;
                }
                else
                {
                    if (DarkStartButton.Visible && !DarkStartButton.CaptureInput)
                    {
                        DarkNameLabel.Visible = false;
                        DarkTextMemo.Visible = false;
                        DarkBackButton.Visible = false;
                        DarkStartButton.Visible = false;
                        DarkSphereImage.Visible = false;
                        TopLabel.Visible = true;
                        var chooseFaction = UI.Engine.TranslateResourceString(LobbyText.ChooseFactionText);
                        TopLabel.Text = chooseFaction;
                        FactionPanel.ImageLibraryIndexTexture = 0;
                    }
                }
            }

            R = new Rect(this.FactionPanel.WinLeft, this.FactionPanel.WinTop, this.FactionPanel.WinLeft + this.FactionPanel.WinWidth, FactionPanel.WinTop + this.FactionPanel.WinHeight);

            UI.TitleWindow.MainWindow.AddBlitRectangle(ref R, null);
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
                LoadWorldCampaign(dialog.UI.WorldMap, "Keepers.cam");
                dialog.CharacterGenerationPanel.ImageLibraryIndexTexture = 10;
                GamePack.Current.TextControl.RetrieveText(10, (StringCollection)(object)value);
                break;
            case 1:
                LoadWorldCampaign(dialog.UI.WorldMap, "Storm.cam");
                dialog.CharacterGenerationPanel.ImageLibraryIndexTexture = 11;
                GamePack.Current.TextControl.RetrieveText(11, (StringCollection)(object)value);
                break;
        }
        ParseFontText(((StringCollection)value).Text, (StringCollection)(object)dialog.ScrollText, dialog.UI.Fonts.Age15WhiteOnGrey, 300);
        WorldMapCampaign campaign = dialog.UI.WorldMap.GetCampaign();
        int id = WorldMapCampaign.GetCampaignScenario(campaign, 0).Paths[0];
        int index = campaign.Scenarios.IndexOfId(id);
        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(dialog.UI.WorldMap.GetCampaign(), index);
        StrategicMap hexSpriteMap = (StrategicMap)dialog.UI.Engine.CreatePreviewHsm(FileBoundary.ReadName(dialog.UI.Engine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario.FileName), GameProfile.WorldMap);
        int i;
        for (i = 0; i < hexSpriteMap.Count && hexSpriteMap.Players.GetPlayers(i).PlayerType != PlayerType.Human; i++)
        {
        }
        Player players = hexSpriteMap.Players.GetPlayers(i);
        dialog.Leader.Assign(players.LeaderSetup, dialog.UI.Engine);
        hexSpriteMap?.Dispose();
        if (value != null)
        {
            ((Persistent)value).Dispose();
        }
    }

    private static void LoadWorldCampaign(WorldMap dialog, string name)
    {
        dialog.UI.Engine.LoadCampaign(FileBoundary.ReadName(dialog.UI.Engine.StartupDirectory + "Scenario\\Campaign\\" + name));
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
