// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public partial class CheatTokenEvent : TokenEvent
{
    public CheatTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131829;

    public sbyte Player;
    public int CheatId;

    // MoveT.PAS:3460
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 20);
        stream.Int32(ref CheatId, 21);
    }

    // MoveT.PAS:3467
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        base.Execute(tokenEvent);
        switch (CheatId)
        {
            case 0:
                {
                    for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        var players = StrategicMap.Current.Players.GetPlayers(I);
                        StrategicMap.Current.Players.GetPlayers(I).SetGems(players.Gems + 1000);
                    }
                }

                break;
            case 1:
                {
                    for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        var players = StrategicMap.Current.Players.GetPlayers(I);
                        StrategicMap.Current.Players.GetPlayers(I).MagicControl.SetMana(players.MagicControl.Mana + 1000);
                    }
                }

                break;
            case 2:
                StrategicMap.Current.SetExplorationEnabled(!StrategicMap.Current.ExplorationEnabled);
                if (StrategicMap.Current.ExplorationEnabled)
                {
                    var explorationEnabled = AbilityRules.TranslateResourceString(s_explorationEnabledText);
                    StrategicMap.Current.ShowMessage(explorationEnabled);
                }
                else
                {
                    var explorationDisabled = AbilityRules.TranslateResourceString(s_explorationDisabledText);
                    StrategicMap.Current.ShowMessage(explorationDisabled);
                }

                break;
            case 3:
                StrategicMap.Current.SetFogEnabled(!StrategicMap.Current.FogEnabled);
                if (StrategicMap.Current.FogEnabled)
                {
                    var fogEnabled = AbilityRules.TranslateResourceString(s_fogEnabledText);
                    StrategicMap.Current.ShowMessage(fogEnabled);
                }
                else
                {
                    var fogDisabled = AbilityRules.TranslateResourceString(s_fogDisabledText);
                    StrategicMap.Current.ShowMessage(fogDisabled);
                }

                break;
            case 4:
                var cheatEnabled = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled();
                StrategicMap.Current.Players.GetPlayers(Player).MagicControl.CheatEnabled = !cheatEnabled;
                if (StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
                {
                    var spellCheatEnabled = AbilityRules.TranslateResourceString(s_spellCheatEnabledText);
                    StrategicMap.Current.ShowMessage(spellCheatEnabled);
                }
                else
                {
                    var spellCheatDisabled = AbilityRules.TranslateResourceString(s_spellCheatDisabledText);
                    StrategicMap.Current.ShowMessage(spellCheatDisabled);
                }

                {
                    for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        var fMagicControlCheatEnabled = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled();
                        StrategicMap.Current.Players.GetPlayers(I).MagicControl.CheatEnabled = fMagicControlCheatEnabled;
                    }
                }

                break;
            case 5:
                StrategicMap.Current.PlayerControl.GameOver(Player, GameOverStatus.Won);
                break;
            case 6:
                StrategicMap.Current.UnlimitedMoves = !StrategicMap.Current.UnlimitedMoves;
                if (StrategicMap.Current.UnlimitedMoves)
                {
                    var freeMovementEnabled = AbilityRules.TranslateResourceString(s_freeMovementEnabledText);
                    StrategicMap.Current.ShowMessage(freeMovementEnabled);
                }
                else
                {
                    var freeMovementDisabled = AbilityRules.TranslateResourceString(s_freeMovementDisabledText);
                    StrategicMap.Current.ShowMessage(freeMovementDisabled);
                }

                break;
            case 7:
                {
                    for (int I = StrategicMap.Current.Players.GetPlayers(0).Structures.Count - 1; I >= 0; I--)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(0).Structures.GetStructure(I).EngineClass.ClassId == 131336)
                        {
                            StrategicMap.Current.Players.GetPlayers(0).Structures.GetStructure(I).SetPlayer(Player);
                        }
                    }
                }

                break;
            case 8:
                StrategicMap.Current.PlayerControl.GameOver(Player, GameOverStatus.Lost);
                break;
            case 9:
                {
                    for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        StrategicMap.Current.Players.GetPlayers(I).MagicControl.Spells.Clear();
                        for (int J = 1, lastJ = GamePack.Current.SpellControl.GetCount() - 1; J <= lastJ; J++)
                        {
                            if (GamePack.Current.SpellControl.GetSpell(J) != null)
                            {
                                StrategicMap.Current.Players.GetPlayers(I).MagicControl.Spells.Add(J);
                            }
                        }
                    }
                }

                break;
        }

        AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB01.WAV");
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

    // NetworkE.pas:230

    // NetworkE.pas:230

}
