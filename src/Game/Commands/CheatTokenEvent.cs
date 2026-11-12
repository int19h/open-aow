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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 20);
        stream.rwInteger(ref CheatId, 21);
    }

    // MoveT.PAS:3467
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        base.Execute(tokenEvent);
        switch (CheatId)
        {
            case 0:
                {
                    for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        var players = AoWHSMap.Current.Players.GetPlayers(I);
                        AoWHSMap.Current.Players.GetPlayers(I).SetGems(players.Gems + 1000);
                    }
                }

                break;
            case 1:
                {
                    for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        var players = AoWHSMap.Current.Players.GetPlayers(I);
                        AoWHSMap.Current.Players.GetPlayers(I).MagicControl.SetMana(players.MagicControl.Mana + 1000);
                    }
                }

                break;
            case 2:
                AoWHSMap.Current.SetExplorationEnabled(!AoWHSMap.Current.ExplorationEnabled);
                if (AoWHSMap.Current.ExplorationEnabled)
                {
                    var explorationEnabled = AbilityRules.TranslateResourceString(s_explorationEnabledText);
                    AoWHSMap.Current.ShowMessage(explorationEnabled);
                }
                else
                {
                    var explorationDisabled = AbilityRules.TranslateResourceString(s_explorationDisabledText);
                    AoWHSMap.Current.ShowMessage(explorationDisabled);
                }

                break;
            case 3:
                AoWHSMap.Current.SetFogEnabled(!AoWHSMap.Current.FogEnabled);
                if (AoWHSMap.Current.FogEnabled)
                {
                    var fogEnabled = AbilityRules.TranslateResourceString(s_fogEnabledText);
                    AoWHSMap.Current.ShowMessage(fogEnabled);
                }
                else
                {
                    var fogDisabled = AbilityRules.TranslateResourceString(s_fogDisabledText);
                    AoWHSMap.Current.ShowMessage(fogDisabled);
                }

                break;
            case 4:
                var cheatEnabled = AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled();
                AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.CheatEnabled = !cheatEnabled;
                if (AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled())
                {
                    var spellCheatEnabled = AbilityRules.TranslateResourceString(s_spellCheatEnabledText);
                    AoWHSMap.Current.ShowMessage(spellCheatEnabled);
                }
                else
                {
                    var spellCheatDisabled = AbilityRules.TranslateResourceString(s_spellCheatDisabledText);
                    AoWHSMap.Current.ShowMessage(spellCheatDisabled);
                }

                {
                    for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        var fMagicControlCheatEnabled = AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.GetCheatEnabled();
                        AoWHSMap.Current.Players.GetPlayers(I).MagicControl.CheatEnabled = fMagicControlCheatEnabled;
                    }
                }

                break;
            case 5:
                AoWHSMap.Current.PlayerControl.GameOver(Player, GameOverStatus.Won);
                break;
            case 6:
                AoWHSMap.Current.UnlimitedMoves = !AoWHSMap.Current.UnlimitedMoves;
                if (AoWHSMap.Current.UnlimitedMoves)
                {
                    var freeMovementEnabled = AbilityRules.TranslateResourceString(s_freeMovementEnabledText);
                    AoWHSMap.Current.ShowMessage(freeMovementEnabled);
                }
                else
                {
                    var freeMovementDisabled = AbilityRules.TranslateResourceString(s_freeMovementDisabledText);
                    AoWHSMap.Current.ShowMessage(freeMovementDisabled);
                }

                break;
            case 7:
                {
                    for (int I = AoWHSMap.Current.Players.GetPlayers(0).Structures.Count - 1; I >= 0; I--)
                    {
                        if (AoWHSMap.Current.Players.GetPlayers(0).Structures.GetStructure(I).EngineClass.ClassId == 131336)
                        {
                            AoWHSMap.Current.Players.GetPlayers(0).Structures.GetStructure(I).SetPlayer(Player);
                        }
                    }
                }

                break;
            case 8:
                AoWHSMap.Current.PlayerControl.GameOver(Player, GameOverStatus.Lost);
                break;
            case 9:
                {
                    for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        AoWHSMap.Current.Players.GetPlayers(I).MagicControl.Spells.Clear();
                        for (int J = 1, lastJ = AoWHSSet.Current.SpellControl.GetCount() - 1; J <= lastJ; J++)
                        {
                            if (AoWHSSet.Current.SpellControl.GetSpell(J) != null)
                            {
                                AoWHSMap.Current.Players.GetPlayers(I).MagicControl.Spells.Add(J);
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
