using Aow.Rtl;
using Aow.Units;

namespace Aow.Units.AoWTC;

public partial class TTacticalCombat
{
        public override Aow.Units.AoWE.TCombatResult Execute()
        {
            if (Aow.Units.AoWE.HeadlessHooks.Tactical)
            {
                FHostPlayer = 0;
                Aow.Units.AoWE.HeadlessHooks.TacticalExecutions++;
            }
            Aow.Units.AoWE.TCombatResult Result = default;
            Aow.Units.AoWTC.TRndTacHsm Generator = default;
            int I = default;
            int Seated = default;
            if (!ValidateWallCombat())
            {
                SetLogCombatActions(1);
                var call__1130 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CannotBreachWallRStr);
                SetLogbookHeader(call__1130);
                FCombatResult = Aow.Units.AoWE.TCombatResult.crCannotBreachWall;
                Result = FCombatResult;
            }
            else
            {
                var call__1131 = FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetCount();
                if (call__1131 == FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].FConquerCount)
                {
                    SetLogCombatActions(1);
                    var call__1132 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CannotInflictDamageRStr);
                    SetLogbookHeader(call__1132);
                    FCombatResult = Aow.Units.AoWE.TCombatResult.crCannotInflictDamage;
                    Result = FCombatResult;
                }
                else
                {
                    var call__1133 = FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetCount();
                    if (call__1133 == 0)
                    {
                        Result = Aow.Units.AoWE.TCombatResult.crNoDefenders;
                    }
                    else
                    {
                        Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.TokenExecuter.HoldTC = true;
                        Generator = Aow.Units.AoWTC.TRndTacHsm.Create();
                        Generator.RandomMap(this, FCombatData.FX, FCombatData.FY, FCombatData.FL);
                        Generator.Free();
                        bool bool__1134 = Aow.Units.AoWE.AoWE.AoWHSMap.FStartingGame;
                        if (!bool__1134)
                        {
                            var call__1135 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(FCombatData.FX, FCombatData.FY, FCombatData.FL, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                            bool__1134 = call__1135;
                        }

                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds = bool__1134;
                        if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                        {
                            {
                                I = 0;
                                int end__1136 = FCombatData.FPlayers.GetCount() - 1;
                                for (; I <= end__1136; I += 1)
                                {
                                    var call__1137 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                    var call__1138 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1137.FPlayer);
                                    if (call__1138.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                                    {
                                        var call__1139 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                        var call__1140 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1139.FPlayer);
                                        var call__1141 = call__1140.GetLocal();
                                        if (call__1141)
                                        {
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds = true;
                                        }
                                    }
                                }
                            }
                        }

                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FDeselectEvents.TriggerNotifyEvents(this);
                        Seated = -1;
                        {
                            I = 0;
                            int end__1142 = FCombatData.FPlayers.GetCount() - 1;
                            for (; I <= end__1142; I += 1)
                            {
                                if (Seated == -1)
                                {
                                    var call__1143 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                    var call__1144 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1143.FPlayer);
                                    if (call__1144.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                                    {
                                        var call__1145 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                        var call__1146 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(call__1145.FPlayer);
                                        if (call__1146.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote)
                                        {
                                            var call__1147 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                            var call__1148 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1147.FPlayer);
                                            if (call__1148.FStatus != Aow.Units.AoWE.TPlayerStatus.psTerminated)
                                            {
                                                Seated = I;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.SetSeatedCombatPlayer(unchecked((sbyte)Seated));
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(32, 29, 0);
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.NewRound();
                        var call__1149 = FCombatData.FTurnOrder[0];
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewTurn(unchecked((sbyte)call__1149));
                        Result = Aow.Units.AoWE.TCombatResult.crBusy;
                        FOnTerminated = Aow.Units.AoWTC.AoWTC.AoWCombatMap.EndCombat;
                        SetLogCombatActions(1);
                    }
                }
            }

            return Result;
        }
}
