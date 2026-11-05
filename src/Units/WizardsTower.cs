// Translated from WizardsTower.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.WizardsTower;

// generate, spheres, level, spell ID
public partial class TWizardsTower : TStructure
{
    public TWizardsTower(TECustomNode owner) : base(owner)
    {
        FGenerateSpell = true;
        FMagicSpheres = WizardsTower.AllMagicSpheres;
        FSpellLevel = 2;
    }

    // WizardsTower.pas:84
    public new const int ClassID = 133278;

    public bool FGenerateSpell;
    public TSet<TMagicSphere> FMagicSpheres;
    public byte FSpellLevel;
    public int FSpellID;
    // WizardsTower.pas:87
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwBool(ref FGenerateSpell, 30);
        RwSet(ref FMagicSpheres, 2, 31, Stream.rwData, 0);
        Stream.rwByte(ref FSpellLevel, 32);
        Stream.rwData(Bytes.Of(ref FSpellID, 4), 4, 33);
    }

    // WizardsTower.pas:76


    // WizardsTower.pas:196
    public override TExecuteResult ExecuteTE(TStructureTE TE, TTokenExecuter Executer)
    {
        TPlayer Player;
        string Reason = "";
        TExecuteResult Result = base.ExecuteTE(TE, Executer);
        if (TE.FAction == 1 && CanBuy((sbyte)TE.FParam, ref Reason))
        {
            Player = AoWHSMap.FPlayers.GetPlayers(TE.FParam);
            var buyCost = BuyCost();
            Player.SetGems(Player.FGems - buyCost);
            Player.FMagicControl.FSpells.Add(FSpellID);
            if (AoWHSMap.FSeatedPlayer == TE.FParam)
            {
                var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var format = Rtl.Rtl.Format(TranslateRStr(TheyTaughtYouTheSpellXRStr), [spell.FSpellName]);
                AoWHSMap.ShowMessage(format);
            }
        }

        return Result;
    }

    // WizardsTower.pas:168
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.FDay == 1 && (FGenerateSpell || FSpellID == 0))
        {
            GenerateSettings();
        }
    }

    // WizardsTower.pas:126
    public override void ArmyPlaced(TArmyHS PlacedArmy)
    {
        TArmy Army;
        TAIGroup Group;
        int I;
        string Reason = "";
        int Cost;
        bool Found;
        int Location;
        base.ArmyPlaced(PlacedArmy);
        var player = AoWHSMap.FPlayers.GetPlayers(PlacedArmy.FArmy.FPlayer);
        if (TSet<TPlayerType>.Of(TPlayerType.ptCPU1, TPlayerType.ptCPU2, TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(player.FPlayerType) && ValidAITarget(PlacedArmy.FArmy.FPlayer))
        {
            Army = PlacedArmy.FArmy;
            Group = null;
            I = 0;
            Cost = BuyCost();
            Found = false;
            var lhx = GetLhx();
            var yhx = GetYhx();
            Location = Unsafe.BitCast<TXYL, int>(PackXYL(GetXhx(), yhx, lhx));
            while (!Found && I != Army.Count)
            {
                Group = Army.GetUnit(I).FGroup;
                Found = Group != null && Unsafe.As<TXYL, int>(ref Group.FTargetXYL) == Location && Group.FGoldBudget == Cost;
                I += 1;
            }

            if (Found && CanBuy(Army.FPlayer, ref Reason))
            {
                Group.ReturnTargetExpense();
                var playerGems = AoWHSMap.FPlayers.GetPlayers(Army.FPlayer);
                AoWHSMap.FPlayers.GetPlayers(Army.FPlayer).SetGems(playerGems.FGems - Cost);
                AoWHSMap.FPlayers.GetPlayers(Army.FPlayer).FMagicControl.FSpells.Add(FSpellID);
            }
        }
    }

    // WizardsTower.pas:107
    public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
    {
        sbyte Player;
        if (!GetRazed() || GetBuilding())
        {
            switch (Info.Mode)
            {
                case TAITargetMode.tmAttack:
                    Player = ((TAoWMapField)Owner).GetPlayer();
                    if (Player != Info.Player && (Player <= 0 || ValidAttackRelation(Info.Player, 0, Player, 0)) && ValidAITarget(Info.Player))
                    {
                        if (Data.Priority < 10)
                        {
                            Data.Priority = 10;
                        }

                        var buyCost = BuyCost();
                        Data.Cost = buyCost;
                    }

                    break;
            }
        }
    }

    // WizardsTower.pas:96
    public override bool CanDblClick() => CanEnter(AoWHSMap.FSeatedPlayer);

    // WizardsTower.pas:99
    public bool ValidAITarget(sbyte Player)
    {
        bool Result;
        if (Player > 0 && !GetRazed() && FSpellID != 0 && AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FSpells.IndexOf(FSpellID) == -1)
        {
            Result = AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastDuration > 0;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // WizardsTower.pas:163
    public void GenerateSettings()
    {
        FSpellID = AoWHSSet.FSpellControl.GetRandomSpell(FSpellLevel, FSpellLevel, FMagicSpheres);
    }

    // WizardsTower.pas:174
    public bool CanEnter(sbyte Player)
    {
        bool Result;
        TArmyHS ArmyHS;
        ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
        if (ArmyHS == null || ArmyHS.FArmy.FPlayer != Player)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // WizardsTower.pas:182
    public bool Enter()
    {
        bool Result = default;
        TEnterWizardsTowerEventLog Log;
        if (CanEnter(AoWHSMap.FSeatedPlayer))
        {
            Log = new TEnterWizardsTowerEventLog(null);
            Log.FTower = this;
            Log.Execute(true, TExecuteTrigger.etUser, null);
            Log.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // WizardsTower.pas:215
    public int BuyCost()
    {
        int Result;
        if (FSpellID != 0)
        {
            Result = AoWHSSet.FSpellControl.GetSpell(FSpellID).FResearchCost;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // WizardsTower.pas:221
    public bool CanBuy(sbyte Player, ref string Reason)
    {
        bool Result;
        if (FSpellID != 0)
        {
            if (AoWHSMap.FPlayers.GetPlayers(Player).FMagicControl.FSpells.IndexOf(FSpellID) == -1)
            {
                var player = AoWHSMap.FPlayers.GetPlayers(Player);
                var buyCost = BuyCost();
                if (player.FGems >= buyCost)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                    Reason = TranslateRStr(NotEnoughGoldRStr);
                }
            }
            else
            {
                Result = false;
                Reason = TranslateRStr(SpellIsAlreadyResearchedRStr);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // WizardsTower.pas:243
    public bool Buy(sbyte Player, ref string Reason)
    {
        bool Result;
        TStructureTE TE;
        Reason = "";
        if (CanBuy(Player, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Player).GetBusy())
        {
            TE = CreateTE();
            SetupTE(TE);
            TE.FAction = 1;
            TE.FParam = Player;
            AoWHSMap.FTokenManager.GetTokenControl(Player).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // WizardsTower.pas:24


}

public partial class TWizardsTowerResource : TStructureResource
{
    public TWizardsTowerResource(TECustomNode owner) : base(owner)
    {}

    // WizardsTower.pas:263
    public new const int ClassID = 133279;

    // WizardsTower.pas:266
    public override int ResourceUserClassID() => 133278;

    // WizardsTower.pas:260
    public override string ResourceEditName() => TranslateRStr(WizardsTowerRStr);

}

// tower
public partial class TEnterWizardsTowerEventLog : TEventLog
{
    // WizardsTower.pas:70
    public TEnterWizardsTowerEventLog(TECustomNode owner) : base(owner)
    { }

    public TWizardsTower FTower;
    // WizardsTower.pas:73
    public override void Destroy()
    {
        base.Destroy();
    }

}

public static partial class WizardsTower
{
    // WizardsTower.pas:269
    public static void RegisterWizardsTower(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TWizardsTower>(), EClass.Of<TWizardsTowerResource>()]);
    }

    public static TSet<TMagicSphere> AllMagicSpheres = TSet<TMagicSphere>.FromMask(255UL);
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
