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

    public PlayerId? Player { get; set; } = PlayerId.Independent;
    public int CheatId { get; set; }

    // MoveT.PAS:3460
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.OptionalPlayer(name: "Player", value: Player, width: IdWidth.UnsignedByte, fallback: 0);
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
                    for (int i = 1, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
                    {
                        StrategicMap strategicMap = GetEngine().StrategicMap;
                        Player players = strategicMap.Players.GetPlayers(i);
                        strategicMap.Players.GetPlayers(i).SetGems(players.Gems + 1_000);
                    }
                }

                break;
            case 1:
                {
                    for (int i = 1, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
                    {
                        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                        Player players = engineStrategicMap.Players.GetPlayers(i);
                        engineStrategicMap.Players.GetPlayers(i).MagicControl.SetMana(players.MagicControl.Mana + 1_000);
                    }
                }

                break;
            case 2:
                GetEngine().StrategicMap.SetExplorationEnabled(!GetEngine().StrategicMap.ExplorationEnabled);
                if (GetEngine().StrategicMap.ExplorationEnabled)
                {
                    string explorationEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_explorationEnabledText);
                    GetEngine().StrategicMap.ShowMessage(explorationEnabled);
                }
                else
                {
                    string explorationDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_explorationDisabledText);
                    GetEngine().StrategicMap.ShowMessage(explorationDisabled);
                }

                break;
            case 3:
                GetEngine().StrategicMap.SetFogEnabled(!GetEngine().StrategicMap.FogEnabled);
                if (GetEngine().StrategicMap.FogEnabled)
                {
                    string fogEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_fogEnabledText);
                    GetEngine().StrategicMap.ShowMessage(fogEnabled);
                }
                else
                {
                    string fogDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_fogDisabledText);
                    GetEngine().StrategicMap.ShowMessage(fogDisabled);
                }

                break;
            case 4:
                bool cheatEnabled = GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.GetCheatEnabled();
                GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.CheatEnabled = !cheatEnabled;
                if (GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
                {
                    string spellCheatEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_spellCheatEnabledText);
                    GetEngine().StrategicMap.ShowMessage(spellCheatEnabled);
                }
                else
                {
                    string spellCheatDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_spellCheatDisabledText);
                    GetEngine().StrategicMap.ShowMessage(spellCheatDisabled);
                }

                {
                    foreach (Player? player in GetEngine().StrategicMap.Players.Skip(1))
                    {
                        bool fMagicControlCheatEnabled = GetEngine().StrategicMap.Players.GetPlayers(Player).MagicControl.GetCheatEnabled();
                        player.MagicControl.CheatEnabled = fMagicControlCheatEnabled;
                    }
                }

                break;
            case 5:
                GetEngine().StrategicMap.PlayerControl.GameOver(Player, status: GameOverStatus.Won);
                break;
            case 6:
                GetEngine().StrategicMap.UnlimitedMoves = !GetEngine().StrategicMap.UnlimitedMoves;
                if (GetEngine().StrategicMap.UnlimitedMoves)
                {
                    string freeMovementEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_freeMovementEnabledText);
                    GetEngine().StrategicMap.ShowMessage(freeMovementEnabled);
                }
                else
                {
                    string freeMovementDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_freeMovementDisabledText);
                    GetEngine().StrategicMap.ShowMessage(freeMovementDisabled);
                }

                break;
            case 7:
                {
                    for (int i = GetEngine().StrategicMap.Players.GetPlayers(0).Structures.Count - 1; i >= 0; i--)
                    {
                        StrategicMap getStructureStrategicMap = GetEngine().StrategicMap;
                        if (getStructureStrategicMap.Players.GetPlayers(0).Structures.GetStructure(i).EngineClass.ClassId == World.Structures.City.ClassId)
                        {
                            getStructureStrategicMap.Players.GetPlayers(0).Structures.GetStructure(i).SetPlayer(Player);
                        }
                    }
                }

                break;
            case 8:
                GetEngine().StrategicMap.PlayerControl.GameOver(Player, status: GameOverStatus.Lost);
                break;
            case 9:
                {
                    foreach (Player? player in GetEngine().StrategicMap.Players.Skip(1))
                    {
                        player.MagicControl.Spells.Clear();
                        GamePack gamePack = GetEngine().GamePack;
                        for (int j = 1, lastJ = gamePack.SpellControl.GetCount() - 1; j <= lastJ; j++)
                        {
                            if (gamePack.SpellControl[j] is not null)
                            {
                                _ = player.MagicControl.Spells.Add(j);
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
