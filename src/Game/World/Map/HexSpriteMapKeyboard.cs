#nullable disable
// Translated from AoWE.pas.
using System.Collections.Immutable;

using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Audio;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;
using Aow.Game.Session;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.Heap;

namespace Aow.Game.World.Map;

// TNotifyEvent target passed directly
// cheat text, mode, lock count
public partial class HexSpriteMapKeyboard(CustomNode owner) : EngineObject(owner)
{
    private string _cheatText = "";

    public HexSpriteMapKeyboardMode Mode { get; private set; }
    public int LockCount { get; private set; }

    // AoWHSMap.PAS:946
    public virtual void KeyDown(in ushort key, Set<ShiftState> shift)
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
                ushort translatedKey = key;
                switch (GetEngine().Language)
                {
                    case 3:
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
                    case 5:
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
                    case 4:
                        if (key is 65)
                        {
                            translatedKey = 68;
                        }

                        break;
                    default:
                        break;
                }

                if (!shift.Contains(ShiftState.Ctrl))
                {
                    switch (translatedKey)
                    {
                        case 79:
                            StrategicMap.Current.ShowIndependentMoves = !StrategicMap.Current.ShowIndependentMoves;
                            if (StrategicMap.Current.ShowIndependentMoves)
                            {
                                string observeEnabled = AbilityRules.TranslateResourceString(GetEngine(), s_observeEnabledText);
                                StrategicMap.Current.ShowMessage(observeEnabled);
                            }
                            else
                            {
                                string observeDisabled = AbilityRules.TranslateResourceString(GetEngine(), s_observeDisabledText);
                                StrategicMap.Current.ShowMessage(observeDisabled);
                            }

                            break;
                        case 78:
                        case 188:
                            _ = StrategicMap.Current.SelectNextArmy();
                            break;
                        case 80:
                        case 190:
                            _ = StrategicMap.Current.SelectPreviousArmy();
                            break;
                        case 72:
                            _ = StrategicMap.Current.SelectNextHero();
                            break;
                        case 77:
                            StrategicMap.Current.SelectedArmy.ExecuteMove();
                            break;
                        case 68:
                            StrategicMap.Current.SelectedArmy.TurnDone();
                            break;
                        case 71:
                            StrategicMap.Current.SelectedArmy.ToggleGuard();
                            break;
                        case 76:
                            StrategicMap.Current.CenterOnLeader(StrategicMap.Current.SeatedPlayer, true, false);
                            break;
                        case 67:
                            if (!shift.Contains(ShiftState.Ctrl))
                            {
                                StrategicMap.Current.SelectedArmy.Center();
                            }

                            break;
                        case 82:
                            if (GetEngine().Debug)
                            {
                                GetEngine().OutputDebugString($"Random Seed: {IntToStr(StrategicMap.Current.DefendersSeed)}");
                            }

                            break;
                        case 83:
                            if (!shift.Contains(ShiftState.Ctrl) && GetEngine().Registry.GetDebugMode())
                            {
                                RunDefaultTest();
                            }

                            break;
                        case 65:
                            if (GetEngine().Registry.GetDeveloper() && StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).PlayerType != PlayerType.Human)
                            {
                                _ = StrategicMap.Current.AIExecutor.ActivatePlayer(StrategicMap.Current.SeatedPlayer);
                            }

                            break;
                        case 73:
                            if (GetEngine().Debug)
                            {
                                HeapStatus heapStatus = GetHeapStatus();
                                StrategicMap.Current.ShowMessage($"Allocated Memory: {IntToStr(heapStatus.TotalAllocated)}");
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
                    prefix = prefix || CompareText(_cheatText, DelphiStrings.Copy(s_cheatNames[i], 1, _cheatText.Length)) == 0;
                    complete = CompareText(_cheatText, s_cheatNames[i]) == 0;
                }

                if (complete && StrategicMap.Current.SeatedPlayer != -1 && !StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetBusy())
                {
                    CheatTokenEvent cheat = new(null)
                    {
                        Holder = CreationHolder(),
                        Player = StrategicMap.Current.SeatedPlayer,
                        CheatId = i
                    };
                    StrategicMap.Current.TokenManager.GetTokenControl(StrategicMap.Current.SeatedPlayer).AddEvent(cheat);
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
        if (key == 67 && (Set<ShiftState>.Of(ShiftState.Shift, ShiftState.Ctrl) & shift) == Set<ShiftState>.Of(ShiftState.Shift, ShiftState.Ctrl) && commandLine.Length > 1 && CompareText(commandLine[1], "beatrix") == 0 && StrategicMap.Current.ConnectionType is not PlayerConnectionType.Network and not PlayerConnectionType.Email && !GetEngine().Demo)
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
        GetEngine().SaveHsm($"{GetEngine().StartupDirectory}Save\\{fileName}", null, GameProfile.WorldMap);
    }

    private static readonly string s_observeEnabledText = "Observe enabled";
    private static readonly string s_observeDisabledText = "Observe disabled";
    private static readonly ImmutableArray<string> s_cheatNames = ["gold", "mana", "explore", "fog", "spells", "win", "freemove", "towns", "lose", "research"];

    // Engine.pas:524

    // Engine.pas:524

}

public enum HexSpriteMapKeyboardMode : byte
{
    Default = 0, Cheat = 1
}
