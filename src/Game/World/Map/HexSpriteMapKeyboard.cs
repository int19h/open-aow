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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// TNotifyEvent target passed directly
// cheat text, mode, lock count
public partial class HexSpriteMapKeyboard : EngineObject
{
    public HexSpriteMapKeyboard(ECustomNode owner) : base(owner)
    { }

    private string _cheatText = "";

    public HexSpriteMapKeyboardMode Mode { get; private set; }
    public int LockCount { get; private set; }

    // AoWHSMap.PAS:946
    public virtual void KeyDown(ref ushort key, Set<ShiftState> shift)
    {
        ushort translatedKey;
        int I;
        bool prefix;
        bool complete;
        CheatTokenEvent cheat;
        if (LockCount != 0)
        {
            return;
        }

        // { Original uses OR between the digit bounds, admitting every key. }
        if ((key >= 65 && key <= 90) || key >= 48 || key <= 57 || key == 188 || key == 190 || key == 13)
        {
            switch (Mode)
            {
                case HexSpriteMapKeyboardMode.Default:
                    translatedKey = key;
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
                            }

                            break;
                        case 4:
                            switch (key)
                            {
                                case 65:
                                    translatedKey = 68;
                                    break;
                            }

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
                                    var observeEnabled = AbilityRules.TranslateResourceString(GetEngine(), s_observeEnabledText);
                                    StrategicMap.Current.ShowMessage(observeEnabled);
                                }
                                else
                                {
                                    var observeDisabled = AbilityRules.TranslateResourceString(GetEngine(), s_observeDisabledText);
                                    StrategicMap.Current.ShowMessage(observeDisabled);
                                }

                                break;
                            case 78:
                            case 188:
                                StrategicMap.Current.SelectNextArmy();
                                break;
                            case 80:
                            case 190:
                                StrategicMap.Current.SelectPreviousArmy();
                                break;
                            case 72:
                                StrategicMap.Current.SelectNextHero();
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
                                    GetEngine().OutputDebugString("Random Seed: " + IntToStr(StrategicMap.Current.DefendersSeed));
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
                                    StrategicMap.Current.AIExecutor.ActivatePlayer(StrategicMap.Current.SeatedPlayer);
                                }

                                break;
                            case 73:
                                if (GetEngine().Debug)
                                {
                                    var heapStatus = GetHeapStatus();
                                    StrategicMap.Current.ShowMessage("Allocated Memory: " + IntToStr(heapStatus.TotalAllocated));
                                }

                                break;
                        }
                    }

                    // AoWHSMap.PAS:1004 called AoWHSMap.EndTurn for Ctrl+E and Ctrl+Enter.
                    // The port presses the end-turn button from the map window instead.
                    // A port addition the original does not have.

                    break;
                case HexSpriteMapKeyboardMode.Cheat:
                    _cheatText = _cheatText + Cp1252.Text((byte)key);
                    I = 10;
                    complete = false;
                    prefix = false;
                    while (I != 0 && !complete)
                    {
                        I -= 1;
                        prefix = prefix || CompareText(_cheatText, RtlUnit.Copy(s_cheatNames[I], 1, _cheatText.Length)) == 0;
                        complete = CompareText(_cheatText, s_cheatNames[I]) == 0;
                    }

                    if (complete && StrategicMap.Current.SeatedPlayer != -1 && !StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetBusy())
                    {
                        cheat = new CheatTokenEvent(null) { Holder = CreationHolder() };
                        cheat.Player = StrategicMap.Current.SeatedPlayer;
                        cheat.CheatId = I;
                        StrategicMap.Current.TokenManager.GetTokenControl(StrategicMap.Current.SeatedPlayer).AddEvent(cheat);
                        cheat.Release();
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
            }
        }

        if (key == 67 && (Set<ShiftState>.Of(ShiftState.Shift, ShiftState.Ctrl) & shift) == Set<ShiftState>.Of(ShiftState.Shift, ShiftState.Ctrl) && CompareText(ParamStr(1), "beatrix") == 0 && StrategicMap.Current.ConnectionType != PlayerConnectionType.Network && StrategicMap.Current.ConnectionType != PlayerConnectionType.Email && !GetEngine().Demo)
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB02.WAV");
            Mode = HexSpriteMapKeyboardMode.Cheat;
            _cheatText = "";
        }
    }

    // AoWHSMap.PAS:928
    public void Lock()
    {
        LockCount += 1;
    }

    // AoWHSMap.PAS:933
    public void Unlock()
    {
        LockCount -= 1;
    }

    // AoWHSMap.PAS:938
    public void RunDefaultTest()
    {
        string fileName;
        fileName = "autosave" + FileBoundary.WriteExtension(DocumentKind.Save);
        // { Original saves the autosave file. }
        GetEngine().SaveHsm(GetEngine().StartupDirectory + "Save\\" + fileName, null, GameProfile.WorldMap);
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
