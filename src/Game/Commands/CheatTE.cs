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

public partial class CheatTE : TokenEvent
{
    public CheatTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131829;

    public sbyte FPlayer;
    public int FCheatID;

    // MoveT.PAS:3460
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 20);
        Stream.rwInteger(ref FCheatID, 21);
    }

    // MoveT.PAS:3467
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        base.Execute(TE);
        switch (FCheatID)
        {
            case 0:
                {
                    for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                    {
                        var players = AoWHSMap.Current.FPlayers.GetPlayers(I);
                        AoWHSMap.Current.FPlayers.GetPlayers(I).SetGems(players.FGems + 1000);
                    }
                }

                break;
            case 1:
                {
                    for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                    {
                        var players = AoWHSMap.Current.FPlayers.GetPlayers(I);
                        AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.SetMana(players.FMagicControl.FMana + 1000);
                    }
                }

                break;
            case 2:
                AoWHSMap.Current.SetExplorationEnabled(!AoWHSMap.Current.FExplorationEnabled);
                if (AoWHSMap.Current.FExplorationEnabled)
                {
                    var p5ExplorationEnabled = AbilityRules.TranslateRStr(P5ExplorationEnabledRStr);
                    AoWHSMap.Current.ShowMessage(p5ExplorationEnabled);
                }
                else
                {
                    var p5ExplorationDisabled = AbilityRules.TranslateRStr(P5ExplorationDisabledRStr);
                    AoWHSMap.Current.ShowMessage(p5ExplorationDisabled);
                }

                break;
            case 3:
                AoWHSMap.Current.SetFogEnabled(!AoWHSMap.Current.FFogEnabled);
                if (AoWHSMap.Current.FFogEnabled)
                {
                    var p5FogEnabled = AbilityRules.TranslateRStr(P5FogEnabledRStr);
                    AoWHSMap.Current.ShowMessage(p5FogEnabled);
                }
                else
                {
                    var p5FogDisabled = AbilityRules.TranslateRStr(P5FogDisabledRStr);
                    AoWHSMap.Current.ShowMessage(p5FogDisabled);
                }

                break;
            case 4:
                var cheatEnabled = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled();
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FCheatEnabled = !cheatEnabled;
                if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled())
                {
                    var p5SpellCheatEnabled = AbilityRules.TranslateRStr(P5SpellCheatEnabledRStr);
                    AoWHSMap.Current.ShowMessage(p5SpellCheatEnabled);
                }
                else
                {
                    var p5SpellCheatDisabled = AbilityRules.TranslateRStr(P5SpellCheatDisabledRStr);
                    AoWHSMap.Current.ShowMessage(p5SpellCheatDisabled);
                }

                {
                    for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                    {
                        var fMagicControlCheatEnabled = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.GetCheatEnabled();
                        AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.FCheatEnabled = fMagicControlCheatEnabled;
                    }
                }

                break;
            case 5:
                AoWHSMap.Current.FPlayerControl.GameOver(FPlayer, GameOverStatus.Won);
                break;
            case 6:
                AoWHSMap.Current.FUnlimitedMoves = !AoWHSMap.Current.FUnlimitedMoves;
                if (AoWHSMap.Current.FUnlimitedMoves)
                {
                    var p5FreeMovementEnabled = AbilityRules.TranslateRStr(P5FreeMovementEnabledRStr);
                    AoWHSMap.Current.ShowMessage(p5FreeMovementEnabled);
                }
                else
                {
                    var p5FreeMovementDisabled = AbilityRules.TranslateRStr(P5FreeMovementDisabledRStr);
                    AoWHSMap.Current.ShowMessage(p5FreeMovementDisabled);
                }

                break;
            case 7:
                {
                    for (int I = AoWHSMap.Current.FPlayers.GetPlayers(0).FStructures.Count - 1; I >= 0; I--)
                    {
                        if (AoWHSMap.Current.FPlayers.GetPlayers(0).FStructures.GetStructure(I).EClass.ClassID == 131336)
                        {
                            AoWHSMap.Current.FPlayers.GetPlayers(0).FStructures.GetStructure(I).SetPlayer(FPlayer);
                        }
                    }
                }

                break;
            case 8:
                AoWHSMap.Current.FPlayerControl.GameOver(FPlayer, GameOverStatus.Lost);
                break;
            case 9:
                {
                    for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                    {
                        AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.FSpells.Clear();
                        for (int J = 1, lastJ = AoWHSSet.Current.FSpellControl.GetCount() - 1; J <= lastJ; J++)
                        {
                            if (AoWHSSet.Current.FSpellControl.GetSpell(J) != null)
                            {
                                AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.FSpells.Add(J);
                            }
                        }
                    }
                }

                break;
        }

        AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB01.WAV");
        return ExecuteResult.Done;
    }

    private static readonly string P5ExplorationEnabledRStr = "Exploration Enabled";
    private static readonly string P5ExplorationDisabledRStr = "Exploration Disabled";
    private static readonly string P5FogEnabledRStr = "Fog Enabled";
    private static readonly string P5FogDisabledRStr = "Fog Disabled";
    private static readonly string P5SpellCheatEnabledRStr = "All spells enabled";
    private static readonly string P5SpellCheatDisabledRStr = "All spells disabled";
    private static readonly string P5FreeMovementEnabledRStr = "Free movement enabled";
    private static readonly string P5FreeMovementDisabledRStr = "Free movement disabled";

    // NetworkE.pas:230

    // NetworkE.pas:230

}
