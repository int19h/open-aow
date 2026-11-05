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

namespace Aow.Units.WizardsTower
{
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
        public override TExecuteResult ExecuteTE(TStructureTE A1, TTokenExecuter Executer)
        {
            TExecuteResult Result = default;
            TPlayer Player = default;
            AnsiString Reason = default;
            Result = base.ExecuteTE(A1, Executer);
            bool bool__1 = A1.FAction == 1;
            if (bool__1)
            {
                var call__2 = CanBuy((sbyte)A1.FParam, ref Reason);
                bool__1 = call__2;
            }

            if (bool__1)
            {
                var call__3 = AoWHSMap.FPlayers.GetPlayers(A1.FParam);
                Player = call__3;
                var call__4 = BuyCost();
                Player.SetGems(Player.FGems - call__4);
                Player.FMagicControl.FSpells.Add(FSpellID);
                if (AoWHSMap.FSeatedPlayer == A1.FParam)
                {
                    var call__5 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__6 = TranslateRStr(TheyTaughtYouTheSpellXRStr);
                    var call__7 = Rtl.Rtl.Format(call__6, [call__5.FSpellName]);
                    AoWHSMap.ShowMessage(call__7);
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
        public override void ArmyPlaced(TArmyHS A1)
        {
            TArmy Army = default;
            TAIGroup Group = default;
            int I = default;
            AnsiString Reason = default;
            int Cost = default;
            bool Found = default;
            int Location = default;
            base.ArmyPlaced(A1);
            var call__8 = AoWHSMap.FPlayers.GetPlayers(A1.FArmy.FPlayer);
            bool bool__9 = TSet<TPlayerType>.Of(TPlayerType.ptCPU1, TPlayerType.ptCPU2, TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(call__8.FPlayerType);
            if (bool__9)
            {
                var call__10 = ValidAITarget(A1.FArmy.FPlayer);
                bool__9 = call__10;
            }

            if (bool__9)
            {
                Army = A1.FArmy;
                Group = null;
                I = 0;
                Cost = BuyCost();
                Found = false;
                var call__11 = GetLhx();
                var call__12 = GetYhx();
                var call__13 = GetXhx();
                var call__14 = PackXYL(call__13, call__12, call__11);
                Location = Unsafe.BitCast<TXYL, int>(call__14);
                while (true)
                {
                    bool bool__15 = !Found;
                    if (bool__15)
                    {
                        var call__16 = Army.Count;
                        bool__15 = I != call__16;
                    }

                    if (!bool__15)
                        break;
                    {
                        var call__17 = Army.GetUnit(I);
                        Group = call__17.FGroup;
                        Found = Group != null && Unsafe.As<TXYL, int>(ref Group.FTargetXYL) == Location && Group.FGoldBudget == Cost;
                        I += 1;
                    }
                }

                bool bool__18 = Found;
                if (bool__18)
                {
                    var call__19 = CanBuy(Army.FPlayer, ref Reason);
                    bool__18 = call__19;
                }

                if (bool__18)
                {
                    Group.ReturnTargetExpense();
                    var call__20 = AoWHSMap.FPlayers.GetPlayers(Army.FPlayer);
                    var call__21 = AoWHSMap.FPlayers.GetPlayers(Army.FPlayer);
                    call__21.SetGems(call__20.FGems - Cost);
                    var call__22 = AoWHSMap.FPlayers.GetPlayers(Army.FPlayer);
                    call__22.FMagicControl.FSpells.Add(FSpellID);
                }
            }
        }

        // WizardsTower.pas:107
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
        {
            sbyte Player = default;
            var call__23 = GetRazed();
            bool bool__24 = !call__23;
            if (!bool__24)
            {
                var call__25 = GetBuilding();
                bool__24 = call__25;
            }

            if (bool__24)
            {
                switch (Info.Mode)
                {
                    case TAITargetMode.tmAttack:
                        var call__26 = ((TAoWMapField)Owner).GetPlayer();
                        Player = call__26;
                        bool bool__27 = Player != Info.Player;
                        if (bool__27)
                        {
                            bool bool__28 = Player <= 0;
                            if (!bool__28)
                            {
                                var call__29 = ValidAttackRelation(Info.Player, 0, Player, 0);
                                bool__28 = call__29;
                            }

                            bool__27 = bool__28;
                        }

                        bool bool__30 = bool__27;
                        if (bool__30)
                        {
                            var call__31 = ValidAITarget(Info.Player);
                            bool__30 = call__31;
                        }

                        if (bool__30)
                        {
                            if (Data.Priority < 10)
                            {
                                Data.Priority = 10;
                            }

                            var call__32 = BuyCost();
                            Data.Cost = call__32;
                        }

                        break;
                }
            }
        }

        // WizardsTower.pas:96
        public override bool CanDblClick()
        {
            bool Result = default;
            var call__33 = CanEnter(AoWHSMap.FSeatedPlayer);
            Result = call__33;
            return Result;
        }

        // WizardsTower.pas:99
        public bool ValidAITarget(sbyte Player)
        {
            bool Result = default;
            bool bool__34 = Player > 0;
            if (bool__34)
            {
                var call__35 = GetRazed();
                bool__34 = !call__35;
            }

            bool bool__36 = bool__34 && FSpellID != 0;
            if (bool__36)
            {
                var call__37 = AoWHSMap.FPlayers.GetPlayers(Player);
                var call__38 = call__37.FMagicControl.FSpells.IndexOf(FSpellID);
                bool__36 = call__38 == -1;
            }

            if (bool__36)
            {
                var call__39 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Result = call__39.FCastDuration > 0;
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
            var call__40 = AoWHSSet.FSpellControl.GetRandomSpell(FSpellLevel, FSpellLevel, FMagicSpheres);
            FSpellID = call__40;
        }

        // WizardsTower.pas:174
        public bool CanEnter(sbyte Player)
        {
            bool Result = default;
            TArmyHS ArmyHS = default;
            var call__41 = Owner.FindOwnedChild(131607);
            ArmyHS = (TArmyHS)call__41;
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
            TEnterWizardsTowerEventLog Log = default;
            var call__42 = CanEnter(AoWHSMap.FSeatedPlayer);
            if (call__42)
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
            int Result = default;
            if (FSpellID != 0)
            {
                var call__43 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Result = call__43.FResearchCost;
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // WizardsTower.pas:221
        public bool CanBuy(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            if (FSpellID != 0)
            {
                var call__44 = AoWHSMap.FPlayers.GetPlayers(Player);
                var call__45 = call__44.FMagicControl.FSpells.IndexOf(FSpellID);
                if (call__45 == -1)
                {
                    var call__46 = AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__47 = BuyCost();
                    if (call__46.FGems >= call__47)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        var call__48 = NotEnoughGoldRStr;
                        var call__49 = TranslateRStr(call__48);
                        Reason = call__49;
                    }
                }
                else
                {
                    Result = false;
                    var call__50 = SpellIsAlreadyResearchedRStr;
                    var call__51 = TranslateRStr(call__50);
                    Reason = call__51;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // WizardsTower.pas:243
        public bool Buy(sbyte Player, ref AnsiString Reason)
        {
            bool Result = default;
            TStructureTE TE = default;
            Reason = (AnsiString)"";
            var call__52 = CanBuy(Player, ref Reason);
            bool bool__53 = call__52;
            if (bool__53)
            {
                var call__54 = AoWHSMap.FPlayers.GetPlayers(Player);
                var call__55 = call__54.GetBusy();
                bool__53 = !call__55;
            }

            if (bool__53)
            {
                TE = CreateTE();
                SetupTE(TE);
                TE.FAction = 1;
                TE.FParam = Player;
                var call__56 = AoWHSMap.FTokenManager.GetTokenControl(Player);
                call__56.AddEvent(TE);
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
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133278;
            return Result;
        }

        // WizardsTower.pas:260
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__57 = WizardsTowerRStr;
            var call__58 = TranslateRStr(call__57);
            Result = call__58;
            return Result;
        }

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
}
