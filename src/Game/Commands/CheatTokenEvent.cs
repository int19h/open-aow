#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Audio;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public class CheatTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 131_829;

    public sbyte Player { get; set; }
    public int CheatId { get; set; }

    // MoveT.PAS:3460
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.UnsignedByte(name: "Player", value: Player);
        CheatId = persister.Int32("CheatId", value: CheatId);
    }

    // MoveT.PAS:3467
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        _ = base.Execute(tokenEvent);
        switch (CheatId)
        {
            case 0:
                {
                    for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(i);
                        StrategicMap.Current.Players.GetPlayers(i).SetGems(players.Gems + 1_000);
                    }
                }

                break;
            case 1:
                {
                    for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(i);
                        StrategicMap.Current.Players.GetPlayers(i).MagicControl.SetMana(players.MagicControl.Mana + 1_000);
                    }
                }

                break;
            case 2:
                StrategicMap.Current.SetExplorationEnabled(!StrategicMap.Current.ExplorationEnabled);
                if (StrategicMap.Current.ExplorationEnabled)
                {
                    string explorationEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_explorationEnabledText);
                    StrategicMap.Current.ShowMessage(explorationEnabled);
                }
                else
                {
                    string explorationDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_explorationDisabledText);
                    StrategicMap.Current.ShowMessage(explorationDisabled);
                }

                break;
            case 3:
                StrategicMap.Current.SetFogEnabled(!StrategicMap.Current.FogEnabled);
                if (StrategicMap.Current.FogEnabled)
                {
                    string fogEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_fogEnabledText);
                    StrategicMap.Current.ShowMessage(fogEnabled);
                }
                else
                {
                    string fogDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_fogDisabledText);
                    StrategicMap.Current.ShowMessage(fogDisabled);
                }

                break;
            case 4:
                bool cheatEnabled = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled();
                StrategicMap.Current.Players.GetPlayers(Player).MagicControl.CheatEnabled = !cheatEnabled;
                if (StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
                {
                    string spellCheatEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_spellCheatEnabledText);
                    StrategicMap.Current.ShowMessage(spellCheatEnabled);
                }
                else
                {
                    string spellCheatDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_spellCheatDisabledText);
                    StrategicMap.Current.ShowMessage(spellCheatDisabled);
                }

                {
                    for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                    {
                        bool fMagicControlCheatEnabled = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled();
                        StrategicMap.Current.Players.GetPlayers(i).MagicControl.CheatEnabled = fMagicControlCheatEnabled;
                    }
                }

                break;
            case 5:
                StrategicMap.Current.PlayerControl.GameOver(Player, status: GameOverStatus.Won);
                break;
            case 6:
                StrategicMap.Current.UnlimitedMoves = !StrategicMap.Current.UnlimitedMoves;
                if (StrategicMap.Current.UnlimitedMoves)
                {
                    string freeMovementEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_freeMovementEnabledText);
                    StrategicMap.Current.ShowMessage(freeMovementEnabled);
                }
                else
                {
                    string freeMovementDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_freeMovementDisabledText);
                    StrategicMap.Current.ShowMessage(freeMovementDisabled);
                }

                break;
            case 7:
                {
                    for (int i = StrategicMap.Current.Players.GetPlayers(0).Structures.Count - 1; i >= 0; i--)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(0).Structures.GetStructure(i).EngineClass.ClassId == World.Structures.City.ClassId)
                        {
                            StrategicMap.Current.Players.GetPlayers(0).Structures.GetStructure(i).SetPlayer(Player);
                        }
                    }
                }

                break;
            case 8:
                StrategicMap.Current.PlayerControl.GameOver(Player, status: GameOverStatus.Lost);
                break;
            case 9:
                {
                    for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                    {
                        StrategicMap.Current.Players.GetPlayers(i).MagicControl.Spells.Clear();
                        for (int j = 1, lastJ = GamePack.Current.SpellControl.GetCount() - 1; j <= lastJ; j++)
                        {
                            if (GamePack.Current.SpellControl[j] is not null)
                            {
                                _ = StrategicMap.Current.Players.GetPlayers(i).MagicControl.Spells.Add(j);
                            }
                        }
                    }
                }

                break;
            default:
                break;
        }

        _ = AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB01.WAV");
        return ExecuteResult.Done;
    }

    private static readonly string s_explorationEnabledText = "Exploration Enabled";
    private static readonly string s_explorationDisabledText = "Exploration Disabled";
    private static readonly string s_fogEnabledText = "Fog Enabled";
    private static readonly string s_fogDisabledText = "Fog Disabled";
    private static readonly string s_spellCheatEnabledText = "All spells enabled";
    private static readonly string s_spellCheatDisabledText = "All spells disabled";
    private static readonly string s_freeMovementEnabledText = "Free movement enabled";
    private static readonly string s_freeMovementDisabledText = "Free movement disabled";
}
