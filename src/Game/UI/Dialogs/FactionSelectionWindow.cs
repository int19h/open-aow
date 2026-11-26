#nullable disable warnings

using Aow.Formats;
using Aow.Game.Draw.Images;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class FactionSelectionWindow(UIContext ui) : UIModule(ui)
{
    public Panel FactionPanel { get; private set; }
    public Label TopLabel { get; private set; }
    public Panel LightPanel { get; private set; }
    public Memo LightTextMemo { get; private set; }
    public Button LightStartButton { get; private set; }
    public Button LightBackButton { get; private set; }
    public Image LightSphereImage { get; private set; }
    public Panel DarkPanel { get; private set; }
    public Button DarkStartButton { get; private set; }
    public Button DarkBackButton { get; private set; }
    public Label LightNameLabel { get; private set; }
    public Memo DarkTextMemo { get; private set; }
    public Label DarkNameLabel { get; private set; }
    public Image DarkSphereImage { get; private set; }

    public int Fade { get; set; }
    public int FadeImage { get; private set; }
    public int FadeDirection { get; private set; }

    public byte[] SelectionFlags { get; } = new byte[4];

    // AoWSelFactionWin.pas:63
    public void CloseSelection()
    {
        UI.Graphics.FactionWindow.ReleaseDiscardableImageData();
        UI.TitleWindow.MainWindow.Visible = false;
        UI.TitleScene.ShowScene(true);
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
        StartCampaignJoin(UI.CampaignJoinDialog, faction: side);
    }

    // AoWSelFactionWin.pas:82
    public void LightPnlMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
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
    public void DarkPnlMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
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
    public void FactionPnlMouseMove(ShiftStates shift, int X, int Y)
    {
        if (!LightPanel.Handled && !DarkNameLabel.Visible && !DarkPanel.Handled && !LightNameLabel.Visible)
        {
            DarkSphereImage.Visible = false;
            LightSphereImage.Visible = false;
            string chooseFaction = UI.Engine.TranslateResourceString(LobbyText.ChooseFactionText);
            TopLabel.Text = chooseFaction;
        }
    }

    // AoWSelFactionWin.pas:148
    public void LightPnlMouseMove(ShiftStates shift, int X, int Y)
    {
        if (!LightNameLabel.Visible && !DarkSphereImage.Visible)
        {
            DarkSphereImage.Visible = true;
            LightSphereImage.Visible = false;
            string cultOfStorms = UI.Engine.TranslateResourceString(LobbyText.CultOfStormsText);
            TopLabel.Text = cultOfStorms;
        }
    }

    // AoWSelFactionWin.pas:158
    public void DarkPnlMouseMove(ShiftStates shift, int X, int Y)
    {
        if (!DarkNameLabel.Visible && !LightSphereImage.Visible)
        {
            DarkSphereImage.Visible = false;
            LightSphereImage.Visible = true;
            string keepers = UI.Engine.TranslateResourceString(LobbyText.KeepersText);
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
                    string chooseFaction = UI.Engine.TranslateResourceString(LobbyText.ChooseFactionText);
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
                        string chooseFaction = UI.Engine.TranslateResourceString(LobbyText.ChooseFactionText);
                        TopLabel.Text = chooseFaction;
                        FactionPanel.ImageLibraryIndexTexture = 0;
                    }
                }
            }

            Rect r = new(FactionPanel.AbsoluteLeft, FactionPanel.AbsoluteTop, FactionPanel.AbsoluteLeft + FactionPanel.WinWidth, FactionPanel.AbsoluteTop + FactionPanel.WinHeight);

            UI.TitleWindow.MainWindow.AddBlitRectangle(ref r, sender: null);
        }
    }

    // AoWSelFactionWin.pas:210
    public void FactionPnlKeyDown(in int key, ShiftStates shift)
    {
        switch (key)
        {
            case 27:
                CloseSelection();
                break;
            default:
                break;
        }
    }

    private static void StartCampaignJoin(CampaignJoinDialog dialog, int faction)
    {
        dialog.ResetCampaignJoin();
        dialog.Faction = faction;
        dialog.Stage = 0;
        DelphiStringList value = [];
        switch (dialog.Faction)
        {
            case 0:
                LoadWorldCampaign(dialog.UI.WorldMap, name: "Keepers.cam");
                dialog.CharacterGenerationPanel.ImageLibraryIndexTexture = 10;
                dialog.UI.Engine.GamePack.TextControl.RetrieveText(10, text: (DelphiStringCollection)(object)value);
                break;
            case 1:
                LoadWorldCampaign(dialog.UI.WorldMap, name: "Storm.cam");
                dialog.CharacterGenerationPanel.ImageLibraryIndexTexture = 11;
                dialog.UI.Engine.GamePack.TextControl.RetrieveText(11, text: (DelphiStringCollection)(object)value);
                break;
            default:
                break;
        }
        ParseFontText(value.Text, strings: (DelphiStringCollection)(object)dialog.ScrollText, font: dialog.UI.Fonts.Age15WhiteOnGrey, maxWidth: 300);
        WorldMapCampaign? campaign = dialog.UI.WorldMap.Campaign;
        int id = WorldMapCampaign.GetCampaignScenario(campaign, index: 0).Paths[0];
        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(index: campaign.Scenarios.IndexOfId(id), campaign: dialog.UI.WorldMap.Campaign);
        StrategicMap hexSpriteMap = dialog.UI.Engine.CreatePreviewHsm<StrategicMap>(FileBoundary.ReadName($"{dialog.UI.Engine.StartupDirectory}Scenario\\Campaign\\{campaignScenario.FileName}"), profile: GameProfile.WorldMap);
        int i;
        for (i = 0; i < hexSpriteMap.Count && hexSpriteMap.Players.GetPlayers(i).PlayerType != PlayerType.Human; i++)
        {
        }
        Player? players = hexSpriteMap.Players.GetPlayers(i);
        dialog.Leader.Assign(players.LeaderSetup, dialog.UI.Engine);
        hexSpriteMap?.Dispose();
    }

    private static void LoadWorldCampaign(WorldMap dialog, string name)
    {
        string fileName = FileBoundary.ReadName($"{dialog.UI.Engine.StartupDirectory}Scenario\\Campaign\\{name}");
        _ = DroppedObjectReport.Load(dialog.UI, fileName: fileName, load: dropped => dialog.UI.Engine.LoadCampaign(fileName, dropped: dropped));
    }

    private static void ParseFontText(string text, DelphiStringCollection strings, ImageLibraryFont font, int maxWidth)
    {
        int i = 1;
        int number = 0;
        string textLocal = "";
        int j;
        for (int length = text.Length; i <= length; i += j)
        {
            if (Cp1252.Byte(text[i - 1]) is 13 or 32)
            {
                i++;
            }
            for (j = 0; i + j <= length && Cp1252.Byte(text[i + j - 1]) is not (10 or 13 or 32); j++)
            {
            }
            string text2 = DelphiStrings.Copy(text, index: i, count: j);
            int textWidth = font.GetTextWidth(text2);
            if (number + textWidth > maxWidth && !DelphiStrings.IsEmpty(textLocal))
            {
                _ = strings.Add(textLocal);
                textLocal = "";
                number = 0;
            }
            if (!DelphiStrings.IsEmpty(textLocal))
            {
                textLocal = $"{textLocal} {text2}";
                number = number + textWidth + font.GetTextWidth(" ");
            }
            else
            {
                textLocal += text2;
                number += textWidth;
            }
            if (i + j <= length && text[i + j - 1] == '\n')
            {
                _ = strings.Add(textLocal);
                textLocal = "";
                number = 0;
                i++;
            }
        }
        if (!DelphiStrings.IsEmpty(textLocal))
        {
            _ = strings.Add(textLocal);
        }
    }
}
