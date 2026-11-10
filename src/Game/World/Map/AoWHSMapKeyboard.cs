// Translated from AoWE.pas.
using Aow.Engine.Buffers;

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
public partial class AoWHSMapKeyboard : EObject
{
    public AoWHSMapKeyboard(ECustomNode owner) : base(owner)
    { }

    public string FCheatText = "";
    public Mode FMode;
    public int FLockCount;

    // AoWHSMap.PAS:946
    public virtual void KeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        ushort TranslatedKey;
        int I;
        bool Prefix;
        bool Complete;
        CheatTE Cheat;
        if (FLockCount != 0)
        {
            return;
        }

        // { Original uses OR between the digit bounds, admitting every key. }
        if ((Key >= 65 && Key <= 90) || Key >= 48 || Key <= 57 || Key == 188 || Key == 190 || Key == 13)
        {
            switch (FMode)
            {
                case Mode.Default:
                    TranslatedKey = Key;
                    switch (GameTables.AoWEngine.FLanguage)
                    {
                        case 3:
                            switch (Key)
                            {
                                case 83:
                                    TranslatedKey = 78;
                                    break;
                                case 67:
                                    TranslatedKey = 76;
                                    break;
                                case 84:
                                    TranslatedKey = 68;
                                    break;
                                case 86:
                                    TranslatedKey = 67;
                                    break;
                                case 68:
                                    TranslatedKey = 77;
                                    break;
                            }

                            break;
                        case 5:
                            switch (Key)
                            {
                                case 86:
                                    TranslatedKey = 78;
                                    break;
                                case 69:
                                    TranslatedKey = 72;
                                    break;
                                case 67:
                                    TranslatedKey = 76;
                                    break;
                                case 70:
                                    TranslatedKey = 68;
                                    break;
                                case 83:
                                    TranslatedKey = 71;
                                    break;
                                case 84:
                                    TranslatedKey = 67;
                                    break;
                            }

                            break;
                        case 4:
                            switch (Key)
                            {
                                case 65:
                                    TranslatedKey = 68;
                                    break;
                            }

                            break;
                    }

                    if (!Shift.Contains(ShiftState.Ctrl))
                    {
                        switch (TranslatedKey)
                        {
                            case 79:
                                AoWHSMap.Current.FShowIndependentMoves = !AoWHSMap.Current.FShowIndependentMoves;
                                if (AoWHSMap.Current.FShowIndependentMoves)
                                {
                                    var observeEnabled = AbilityRules.TranslateRStr(ObserveEnabledRStr);
                                    AoWHSMap.Current.ShowMessage(observeEnabled);
                                }
                                else
                                {
                                    var observeDisabled = AbilityRules.TranslateRStr(ObserveDisabledRStr);
                                    AoWHSMap.Current.ShowMessage(observeDisabled);
                                }

                                break;
                            case 78:
                            case 188:
                                AoWHSMap.Current.SelectNextArmy();
                                break;
                            case 80:
                            case 190:
                                AoWHSMap.Current.SelectPrevArmy();
                                break;
                            case 72:
                                AoWHSMap.Current.SelectNextHero();
                                break;
                            case 77:
                                AoWHSMap.Current.FSelectedArmy.ExecuteMove();
                                break;
                            case 68:
                                AoWHSMap.Current.FSelectedArmy.TurnDone();
                                break;
                            case 71:
                                AoWHSMap.Current.FSelectedArmy.ToggleGuard();
                                break;
                            case 76:
                                AoWHSMap.Current.CenterOnLeader(AoWHSMap.Current.FSeatedPlayer, true, false);
                                break;
                            case 67:
                                if (!Shift.Contains(ShiftState.Ctrl))
                                {
                                    AoWHSMap.Current.FSelectedArmy.Center();
                                }

                                break;
                            case 82:
                                if (GameTables.AoWEngine.FDebug)
                                {
                                    GameTables.AoWEngine.OutputDebugStr("Random Seed: " + IntToStr(AoWHSMap.Current.FDefendersSeed));
                                }

                                break;
                            case 83:
                                if (!Shift.Contains(ShiftState.Ctrl) && GameTables.AoWEngine.FRegistry.GetDebugMode())
                                {
                                    RunDefaultTest();
                                }

                                break;
                            case 65:
                                if (GameTables.AoWEngine.FRegistry.GetDeveloper() && AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FPlayerType != PlayerType.Human)
                                {
                                    AoWHSMap.Current.FAIExecuter.ActivatePlayer(AoWHSMap.Current.FSeatedPlayer);
                                }

                                break;
                            case 73:
                                if (GameTables.AoWEngine.FDebug)
                                {
                                    var heapStatus = GetHeapStatus();
                                    AoWHSMap.Current.ShowMessage("Allocated Memory: " + IntToStr(heapStatus.TotalAllocated));
                                }

                                break;
                        }
                    }

                    // AoWHSMap.PAS:1004 called AoWHSMap.EndTurn for Ctrl+E and Ctrl+Enter.
                    // The port presses the end-turn button from the map window instead.
                    // A port addition the original does not have.

                    break;
                case Mode.Cheat:
                    FCheatText = FCheatText + Cp1252.Text((byte)Key);
                    I = 10;
                    Complete = false;
                    Prefix = false;
                    while (I != 0 && !Complete)
                    {
                        I -= 1;
                        Prefix = Prefix || CompareText(FCheatText, RtlUnit.Copy(CheatNames[I], 1, FCheatText.Length)) == 0;
                        Complete = CompareText(FCheatText, CheatNames[I]) == 0;
                    }

                    if (Complete && AoWHSMap.Current.FSeatedPlayer != -1 && !AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetBusy())
                    {
                        Cheat = new CheatTE(null);
                        Cheat.FPlayer = AoWHSMap.Current.FSeatedPlayer;
                        Cheat.FCheatID = I;
                        AoWHSMap.Current.FTokenManager.GetTokenControl(AoWHSMap.Current.FSeatedPlayer).AddEvent(Cheat);
                        Cheat.Release();
                        FCheatText = "";
                        FMode = Mode.Default;
                    }
                    else
                    {
                        if (!Prefix)
                        {
                            FMode = Mode.Default;
                            FCheatText = "";
                        }
                    }

                    break;
            }
        }

        if (Key == 67 && (Set<ShiftState>.Of(ShiftState.Shift, ShiftState.Ctrl) & Shift) == Set<ShiftState>.Of(ShiftState.Shift, ShiftState.Ctrl) && CompareText(ParamStr(1), "beatrix") == 0 && AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network && AoWHSMap.Current.FConnectionType != PlayerConnectionType.Email && !GameTables.AoWEngine.FDemo)
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB02.WAV");
            FMode = Mode.Cheat;
            FCheatText = "";
        }
    }

    // AoWHSMap.PAS:928
    public void Lock()
    {
        FLockCount += 1;
    }

    // AoWHSMap.PAS:933
    public void Unlock()
    {
        FLockCount -= 1;
    }

    // AoWHSMap.PAS:938
    public void RunDefaultTest()
    {
        string FileName;
        FileName = "autosave" + FileBoundary.WriteExtension(DocumentKind.Save);
        // { Original saves the autosave file. }
        GameTables.AoWEngine.SaveHSM(GameTables.AoWEngine.StartupDirectory + "Save\\" + FileName, null, GameProfile.WorldMap);
    }

    private static readonly string ObserveEnabledRStr = "Observe enabled";
    private static readonly string ObserveDisabledRStr = "Observe disabled";
    private static FixedArray_string_10 CheatNames = FixedArray_string_10.From(["gold", "mana", "explore", "fog", "spells", "win", "freemove", "towns", "lose", "research"]);

    public enum Mode : byte
    {
        Default = 0, Cheat = 1
    }

    // Engine.pas:524

    // Engine.pas:524

}
