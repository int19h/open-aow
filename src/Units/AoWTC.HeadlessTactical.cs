using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.NetworkE;

namespace Aow.Units.AoWTC;

public partial class TTacticalCombat
{
        public override TCombatResult Execute()
        {
            if (HeadlessHooks.Tactical || FrameFixtureInputs.Tactical)
            {
                FHostPlayer = 0;
                HeadlessHooks.TacticalExecutions++;
            }
            TCombatResult Result = default;
            TRndTacHsm Generator = default;
            int I = default;
            int Seated = default;
            if (!ValidateWallCombat())
            {
                SetLogCombatActions(1);
                var call__1130 = AoWE.AoWE.TranslateRStr(AoWE.AoWE.CannotBreachWallRStr);
                SetLogbookHeader(call__1130);
                FCombatResult = TCombatResult.crCannotBreachWall;
                Result = FCombatResult;
            }
            else
            {
                var call__1131 = FSides[(int)TCombatSideIndex.csDefender].GetCount();
                if (call__1131 == FSides[(int)TCombatSideIndex.csDefender].FConquerCount)
                {
                    SetLogCombatActions(1);
                    var call__1132 = AoWE.AoWE.TranslateRStr(AoWE.AoWE.CannotInflictDamageRStr);
                    SetLogbookHeader(call__1132);
                    FCombatResult = TCombatResult.crCannotInflictDamage;
                    Result = FCombatResult;
                }
                else
                {
                    var call__1133 = FSides[(int)TCombatSideIndex.csDefender].GetCount();
                    if (call__1133 == 0)
                    {
                        Result = TCombatResult.crNoDefenders;
                    }
                    else
                    {
                        AoWE.AoWE.AoWHSMap.FTokenManager.TokenExecuter.HoldTC = true;
                        Generator = new TRndTacHsm();
                        Generator.RandomMap(this, FCombatData.FX, FCombatData.FY, FCombatData.FL);
                        Generator.Free();
                        bool bool__1134 = AoWE.AoWE.AoWHSMap.FStartingGame;
                        if (!bool__1134)
                        {
                            var call__1135 = AoWE.AoWE.AoWHSMap.VisibleForPlayer(FCombatData.FX, FCombatData.FY, FCombatData.FL, AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            bool__1134 = call__1135;
                        }

                        AoWTC.AoWCombatMap.FPlaySounds = bool__1134;
                        if (!AoWTC.AoWCombatMap.FPlaySounds)
                        {
                            {
                                I = 0;
                                int end__1136 = FCombatData.FPlayers.GetCount() - 1;
                                for (; I <= end__1136; I += 1)
                                {
                                    var call__1137 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                    var call__1138 = AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1137.FPlayer);
                                    if (call__1138.FPlayerType == TPlayerType.ptHuman)
                                    {
                                        var call__1139 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                        var call__1140 = AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1139.FPlayer);
                                        var call__1141 = call__1140.GetLocal();
                                        if (call__1141)
                                        {
                                            AoWTC.AoWCombatMap.FPlaySounds = true;
                                        }
                                    }
                                }
                            }
                        }

                        AoWTC.AoWCombatMap.FDeselectEvents.TriggerNotifyEvents(this);
                        Seated = -1;
                        {
                            I = 0;
                            int end__1142 = FCombatData.FPlayers.GetCount() - 1;
                            for (; I <= end__1142; I += 1)
                            {
                                if (Seated == -1)
                                {
                                    var call__1143 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                    var call__1144 = AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1143.FPlayer);
                                    if (call__1144.FPlayerType == TPlayerType.ptHuman)
                                    {
                                        var call__1145 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                        var call__1146 = AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(call__1145.FPlayer);
                                        if (call__1146.ConnectionMode != TConnectionMode.cmRemote)
                                        {
                                            var call__1147 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                            var call__1148 = AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1147.FPlayer);
                                            if (call__1148.FStatus != TPlayerStatus.psTerminated)
                                            {
                                                Seated = I;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        AoWTC.AoWCombatMap.SetSeatedCombatPlayer((sbyte)Seated);
                        AoWTC.AoWCombatMap.CenterViewTo(32, 29, 0);
                        AoWTC.AoWCombatMap.FCombat.NewRound();
                        var call__1149 = FCombatData.FTurnOrder[0];
                        AoWTC.AoWCombatMap.NewTurn((sbyte)call__1149);
                        Result = TCombatResult.crBusy;
                        FOnTerminated = AoWTC.AoWCombatMap.EndCombat;
                        SetLogCombatActions(1);
                    }
                }
            }

            FrameFixtureInputs.TacticalInitialized?.Invoke();
            return Result;
        }
}
