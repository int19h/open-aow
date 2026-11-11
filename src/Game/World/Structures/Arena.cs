// Translated from Arena.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public partial class Arena : Structure
{
    public Arena(ECustomNode owner) : base(owner)
    { }

    // Arena.pas:95
    public new const int ClassID = 133276;

    // Arena.pas:212
    public override StructureTokenEvent CreateTE() => new ArenaTokenEvent(null);

    // Arena.pas:135
    public override ExecuteResult ExecuteTE(StructureTokenEvent TE, TokenExecutor Executer)
    {
        Troop AUnit;
        string ErrorText = "";
        ExecuteResult Result = base.ExecuteTE(TE, Executer);
        if (TE.FAction == 1)
        {
            if (CanTrainUnits((sbyte)TE.FParam, ((ArenaTokenEvent)TE).FUnitIDs, ref ErrorText))
            {
                for (int I = 0, last = ((ArenaTokenEvent)TE).FUnitIDs.Count - 1; I <= last; I++)
                {
                    var unitID = ((ArenaTokenEvent)TE).FUnitIDs[I];
                    AUnit = AoWHSMap.Current.FUnitControl.FindUnit(unitID);
                    if (AUnit != null)
                    {
                        var player = AoWHSMap.Current.FPlayers.GetPlayers(TE.FParam);
                        var unitTrainCost = UnitTrainCost(AUnit);
                        AoWHSMap.Current.FPlayers.GetPlayers(TE.FParam).SetGems(player.FGems - unitTrainCost);
                        ExecuteTrainUnit(AUnit);
                    }
                }

                if (AoWHSMap.Current.FSeatedPlayer == TE.FParam)
                {
                    var arenaTE = (ArenaTokenEvent)TE;
                    if (arenaTE.FUnitIDs.Count == 1)
                    {
                        var oneUnitTrained = AbilityRules.TranslateRStr(OneUnitTrainedRStr);
                        AoWHSMap.Current.ShowMessage(oneUnitTrained);
                    }
                    else
                    {
                        var format = RtlUnit.Format(AbilityRules.TranslateRStr(XUnitsTrainedRStr), [IntToStr(arenaTE.FUnitIDs.Count)]);
                        AoWHSMap.Current.ShowMessage(format);
                    }
                }
            }
        }

        return Result;
    }

    // Arena.pas:98
    public override bool CanDblClick() => CanEnter(AoWHSMap.Current.FSeatedPlayer);

    // Arena.pas:101
    public int UnitTrainCost(Troop AUnit)
    {
        int Result;
        if (AUnit is Hero)
        {
            // { The original rejects heroes, although ExecuteTrainUnit supports them. }
            return -1;
        }

        if (AUnit is Unit)
        {
            var unit = (Unit)AUnit;
            if (unit.GetRank() != Unit.Rank.Gold)
            {
                Result = (unit.GetNextLevelExperience() - AUnit.GetExperience()) * 5;
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Arena.pas:116
    public void ExecuteTrainUnit(Troop AUnit)
    {
        if (AUnit is Hero)
        {
            var hero = AUnit;
            hero.SetExperience(hero.GetNextLevelExperience());
            hero.Changed();

            return;
        }

        if (AUnit is Unit)
        {
            var unit = AUnit;
            unit.SetExperience(unit.GetNextLevelExperience());
            unit.Changed();
        }
    }

    // Arena.pas:159
    public bool CanTrainUnits(sbyte Player, Int32List Units, ref string ErrorText)
    {
        bool Result;
        int Total;
        int I;
        int Cost;
        Troop AUnit;
        if (Units.Count != 0)
        {
            Total = 0;
            I = Units.Count;
            Result = true;
            while (Result && I != 0)
            {
                I -= 1;
                var unit = Units[I];
                AUnit = AoWHSMap.Current.FUnitControl.FindUnit(unit);
                if (AUnit != null)
                {
                    Cost = UnitTrainCost(AUnit);
                    if (Cost != -1)
                    {
                        Total += Cost;
                    }
                    else
                    {
                        Result = false;
                    }
                }
                else
                {
                    Result = false;
                }
            }

            if (Result)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FGems >= Total)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                    ErrorText = AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
                }
            }
        }
        else
        {
            Result = false;
            ErrorText = AbilityRules.TranslateRStr(GameText.NoUnitSelectedRStr);
        }

        return Result;
    }

    // Arena.pas:194
    public bool TrainUnits(sbyte Player, Int32List Units, ref string ErrorText)
    {
        bool Result;
        ArenaTokenEvent TE;
        ErrorText = "";
        if (CanTrainUnits(Player, Units, ref ErrorText) && !AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy())
        {
            TE = (ArenaTokenEvent)CreateTE();
            SetupTE(TE);
            TE.FUnitIDs.Assign(Units);
            TE.FParam = Player;
            TE.FAction = 1;
            AoWHSMap.Current.FTokenManager.GetTokenControl(Player).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Arena.pas:215
    public bool CanEnter(sbyte Player)
    {
        bool Result;
        ArmyHexSprite Army;
        Army = (ArmyHexSprite)FOwner.FindOwnedChild(131607);
        if (Army == null || Army.FArmy.FPlayer != Player)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Arena.pas:223
    public bool Enter()
    {
        bool Result = default;
        EnterArenaEventLog Log;
        ArmyHexSprite Army;
        if (CanEnter(AoWHSMap.Current.FSeatedPlayer))
        {
            Log = new EnterArenaEventLog(null);
            Log.FArena = this;
            Army = (ArmyHexSprite)FOwner.FindOwnedChild(131607);
            for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
            {
                var unit = Army.FArmy.GetUnit(I);
                Log.FUnitIDs.Add(unit.FUnitID);
                if (UnitTrainCost(Army.FArmy.GetUnit(I)) != -1)
                {
                    Log.FTrainable = (byte)(Log.FTrainable | (1 << I));
                }
            }

            Log.Execute(true, ExecuteTrigger.User, null);
            Log.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string OneUnitTrainedRStr = "1 unit trained";
    private static readonly string XUnitsTrainedRStr = "%s units trained";

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FCastsShadow = true;
        }

        // Arena.pas:253
        public new const int ClassID = 133277;

        // Arena.pas:244

        // Arena.pas:256
        public override int ResourceUserClassID() => 133276;

        // Arena.pas:45

        // Arena.pas:250
        public override string ResourceEditName() => AbilityRules.TranslateRStr(ArenaRStr);

        private static readonly string ArenaRStr = "Arena";

    }

}
