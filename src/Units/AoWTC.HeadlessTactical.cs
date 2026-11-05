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
            int Seated = default;
            if (!ValidateWallCombat())
            {
                SetLogCombatActions(1);
                SetLogbookHeader(AoWE.AoWE.TranslateRStr(AoWE.AoWE.CannotBreachWallRStr));
                FCombatResult = TCombatResult.crCannotBreachWall;
                Result = FCombatResult;
            }
            else
            {
                if (FSides[(int)TCombatSideIndex.csDefender].GetCount() == FSides[(int)TCombatSideIndex.csDefender].FConquerCount)
                {
                    SetLogCombatActions(1);
                    SetLogbookHeader(AoWE.AoWE.TranslateRStr(AoWE.AoWE.CannotInflictDamageRStr));
                    FCombatResult = TCombatResult.crCannotInflictDamage;
                    Result = FCombatResult;
                }
                else
                {
                    if (FSides[(int)TCombatSideIndex.csDefender].GetCount() == 0)
                    {
                        Result = TCombatResult.crNoDefenders;
                    }
                    else
                    {
                        AoWE.AoWE.AoWHSMap.FTokenManager.TokenExecuter.HoldTC = true;
                        Generator = new TRndTacHsm();
                        Generator.RandomMap(this, FCombatData.FX, FCombatData.FY, FCombatData.FL);
                        Generator.Free();
                        bool condition = AoWE.AoWE.AoWHSMap.FStartingGame || AoWE.AoWE.AoWHSMap.VisibleForPlayer(FCombatData.FX, FCombatData.FY, FCombatData.FL, AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        AoWTC.AoWCombatMap.FPlaySounds = condition;
                        if (!AoWTC.AoWCombatMap.FPlaySounds)
                        {
                            for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
                            {
                                var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                if (AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == TPlayerType.ptHuman)
                                {
                                    var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                    if (AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(player.FPlayer).GetLocal())
                                    {
                                        AoWTC.AoWCombatMap.FPlaySounds = true;
                                    }
                                }
                            }
                        }

                        AoWTC.AoWCombatMap.FDeselectEvents.TriggerNotifyEvents(this);
                        Seated = -1;
                        for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
                        {
                            if (Seated == -1)
                            {
                                var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                if (AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == TPlayerType.ptHuman)
                                {
                                    var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                    if (AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(player.FPlayer).ConnectionMode != TConnectionMode.cmRemote)
                                    {
                                        var player2 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                        if (AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(player2.FPlayer).FStatus != TPlayerStatus.psTerminated)
                                        {
                                            Seated = I;
                                        }
                                    }
                                }
                            }
                        }

                        AoWTC.AoWCombatMap.SetSeatedCombatPlayer((sbyte)Seated);
                        AoWTC.AoWCombatMap.CenterViewTo(32, 29, 0);
                        AoWTC.AoWCombatMap.FCombat.NewRound();
                        var turnOrder = FCombatData.FTurnOrder[0];
                        AoWTC.AoWCombatMap.NewTurn((sbyte)turnOrder);
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
