#nullable disable warnings
// Translated from AoWE.pas.
using System.Collections.Immutable;
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Rtl;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.World.Map;

// TNotifyEvent target passed directly
// cheat text, mode, lock count
public sealed class HexSpriteMapKeyboard(CustomNode? owner) : EngineObject(owner)
{
    private string _cheatText = "";

    public HexSpriteMapKeyboardMode Mode { get; private set; }
    public int LockCount { get; private set; }

    // AoWHSMap.PAS:946
    public void KeyDown(in int key, ShiftStates shift)
    {
        if (LockCount != 0)
        {
            return;
        }

        // AoWHSMap.PAS tests the key with OR between the digit bounds, which admits every key, so
        // no test remains here.
        switch (Mode)
        {
            case HexSpriteMapKeyboardMode.Default:
                int translatedKey = key;
                switch (GetEngine().Language)
                {
                    case (byte)GameSettings.Language.French:
                        switch (key)
                        {
                            case 83:
                                translatedKey = 78;
                                break;
                            case 67:
                                translatedKey = 76;
                                break;
                            case 84:
                                translatedKey = 68;
                                break;
                            case 86:
                                translatedKey = 67;
                                break;
                            case 68:
                                translatedKey = 77;
                                break;
                            default:
                                break;
                        }

                        break;
                    case (byte)GameSettings.Language.Italian:
                        switch (key)
                        {
                            case 86:
                                translatedKey = 78;
                                break;
                            case 69:
                                translatedKey = 72;
                                break;
                            case 67:
                                translatedKey = 76;
                                break;
                            case 70:
                                translatedKey = 68;
                                break;
                            case 83:
                                translatedKey = 71;
                                break;
                            case 84:
                                translatedKey = 67;
                                break;
                            default:
                                break;
                        }

                        break;
                    case (byte)GameSettings.Language.Spanish:
                        if (key is 65)
                        {
                            translatedKey = 68;
                        }

                        break;
                    default:
                        break;
                }

                if (!shift.Contains(ShiftStates.Ctrl))
                {
                    switch (translatedKey)
                    {
                        case 79:
                            GetEngine().StrategicMap.ShowIndependentMoves = !GetEngine().StrategicMap.ShowIndependentMoves;
                            if (GetEngine().StrategicMap.ShowIndependentMoves)
                            {
                                string observeEnabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_observeEnabledText);
                                GetEngine().StrategicMap.ShowMessage(observeEnabled);
                            }
                            else
                            {
                                string observeDisabled = AbilityRules.TranslateResourceString(GetEngine(), S: s_observeDisabledText);
                                GetEngine().StrategicMap.ShowMessage(observeDisabled);
                            }

                            break;
                        case 78:
                        case 188:
                            _ = GetEngine().StrategicMap.SelectNextArmy();
                            break;
                        case 80:
                        case 190:
                            _ = GetEngine().StrategicMap.SelectPreviousArmy();
                            break;
                        case 72:
                            _ = GetEngine().StrategicMap.SelectNextHero();
                            break;
                        case 77:
                            GetEngine().StrategicMap.SelectedArmy.ExecuteMove();
                            break;
                        case 68:
                            GetEngine().StrategicMap.SelectedArmy.TurnDone();
                            break;
                        case 71:
                            GetEngine().StrategicMap.SelectedArmy.ToggleGuard();
                            break;
                        case 76:
                            GetEngine().StrategicMap.CenterOnLeader(GetEngine().StrategicMap.SeatedPlayer, select: true, animate: false);
                            break;
                        case 67:
                            if (!shift.Contains(ShiftStates.Ctrl))
                            {
                                GetEngine().StrategicMap.SelectedArmy.Center();
                            }

                            break;
                        case 82:
                            if (GetEngine().Debug)
                            {
                                GetEngine().OutputDebugString($"Random Seed: {GetEngine().StrategicMap.DefendersSeed.ToString(CultureInfo.InvariantCulture)}");
                            }

                            break;
                        case 83:
                            if (!shift.Contains(ShiftStates.Ctrl) && GetEngine().Registry.GetDebugMode())
                            {
                                RunDefaultTest();
                            }

                            break;
                        case 65:
                            if (GetEngine().Registry.Developer && GetEngine().StrategicMap.Players.GetPlayers(GetEngine().StrategicMap.SeatedPlayer).PlayerType != PlayerType.Human)
                            {
                                StrategicMap strategicMap = GetEngine().StrategicMap;
                                _ = strategicMap.AIExecutor.ActivatePlayer(strategicMap.SeatedPlayer);
                            }

                            break;
                        case 73:
                            if (GetEngine().Debug)
                            {
                                HeapStatus heapStatus = new(unchecked((int)GC.GetTotalMemory(false)));
                                GetEngine().StrategicMap.ShowMessage($"Allocated Memory: {heapStatus.TotalAllocated.ToString(CultureInfo.InvariantCulture)}");
                            }

                            break;
                        default:
                            break;
                    }
                }

                // AoWHSMap.PAS:1004 called AoWHSMap.EndTurn for Ctrl+E and Ctrl+Enter.
                // The port presses the end-turn button from the map window instead.
                // A port addition the original does not have.

                break;
            case HexSpriteMapKeyboardMode.Cheat:
                _cheatText += Cp1252.Text((byte)key);
                int i = 10;
                bool complete = false;
                bool prefix = false;
                while (i != 0 && !complete)
                {
                    i -= 1;
                    prefix = prefix || CompareText(_cheatText, DelphiStrings.Copy(s_cheatNames[i], index: 1, count: _cheatText.Length)) == 0;
                    complete = CompareText(_cheatText, s_cheatNames[i]) == 0;
                }

                if (complete && GetEngine().StrategicMap.SeatedPlayer is not null && !GetEngine().StrategicMap.Players.GetPlayers(GetEngine().StrategicMap.SeatedPlayer).GetBusy())
                {
                    StrategicMap strategicMap = GetEngine().StrategicMap;
                    CheatTokenEvent cheat = new(null)
                    {
                        Holder = CreationHolder(),
                        Player = strategicMap.SeatedPlayer,
                        CheatId = i
                    };
                    strategicMap.TokenManager.GetTokenControl(strategicMap.SeatedPlayer).AddEvent(cheat);
                    _ = cheat.Release();
                    _cheatText = "";
                    Mode = HexSpriteMapKeyboardMode.Default;
                }
                else
                {
                    if (!prefix)
                    {
                        Mode = HexSpriteMapKeyboardMode.Default;
                        _cheatText = "";
                    }
                }

                break;
            default:
                break;
        }

        string[] commandLine = Environment.GetCommandLineArgs();
        if (key == 67 && ((ShiftStates.Shift | ShiftStates.Ctrl) & shift) == (ShiftStates.Shift | ShiftStates.Ctrl) && commandLine.Length > 1 && CompareText(commandLine[1], "beatrix") == 0 && GetEngine().StrategicMap.ConnectionType is not PlayerConnectionType.Network and not PlayerConnectionType.Email && !GetEngine().Demo)
        {
            _ = AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB02.WAV");
            Mode = HexSpriteMapKeyboardMode.Cheat;
            _cheatText = "";
        }
    }

    // AoWHSMap.PAS:928
    public void Lock() => LockCount += 1;

    // AoWHSMap.PAS:933
    public void Unlock() => LockCount -= 1;

    // AoWHSMap.PAS:938
    public void RunDefaultTest()
    {
        string fileName = $"autosave{FileBoundary.WriteExtension(DocumentKind.Save)}";
        // { Original saves the autosave file. }
        GetEngine().SaveHsm($"{GetEngine().StartupDirectory}Save\\{fileName}", progress: null, profile: GameProfile.WorldMap);
    }

    private static readonly string s_observeEnabledText = "Observe enabled";
    private static readonly string s_observeDisabledText = "Observe disabled";
    private static readonly ImmutableArray<string> s_cheatNames = ["gold", "mana", "explore", "fog", "spells", "win", "freemove", "towns", "lose", "research"];
}

public enum HexSpriteMapKeyboardMode : byte
{
    Default = 0, Cheat = 1
}
