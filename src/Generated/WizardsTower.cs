// Generated from WizardsTower.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.WizardsTower
{
    // { TWizardsTower: size $38, VMT 557D5508; $30 generate, $31 spheres, $33 level, $34 spell ID. }
    public partial class TWizardsTower : Aow.Units.AoWE.TStructure
    {
        public bool FGenerateSpell;
        // { $30 }
        public TSet<Aow.Units.AoWE.TMagicSphere> FMagicSpheres;
        // { $31 }
        public byte FSpellLevel;
        // { $33 }
        public int FSpellID;
        // WizardsTower.pas:87
        // { $34 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwBool(ref FGenerateSpell, 30);
            Aow.Rtl.Rtl.RwSet(ref FMagicSpheres, 2, 31, Stream.rwData, 0);
            Stream.rwByte(ref FSpellLevel, 32);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FSpellID, 4), 4, 33);
        }

        // WizardsTower.pas:76
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FGenerateSpell = true;
            FMagicSpheres = Aow.Units.WizardsTower.WizardsTower.AllMagicSpheres;
            FSpellLevel = 2;
        }

        // WizardsTower.pas:196
        public override Aow.Units.NetworkE.TExecuteResult ExecuteTE(Aow.Units.AoWE.TStructureTE A1, Aow.Units.NetworkE.TTokenExecuter Executer)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Aow.Units.AoWE.TPlayer Player = default;
            Aow.Rtl.AnsiString Reason = default;
            Result = base.ExecuteTE(A1, Executer);
            bool bool__1 = (A1.FAction == 1);
            if (bool__1)
            {
                var call__2 = CanBuy(unchecked((sbyte)A1.FParam), ref Reason);
                bool__1 = call__2;
            }

            if (bool__1)
            {
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FParam);
                Player = call__3;
                var call__4 = BuyCost();
                Player.SetGems(Player.FGems - call__4);
                Player.FMagicControl.FSpells.Add(FSpellID);
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FParam)
                {
                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TheyTaughtYouTheSpellXRStr);
                    var call__7 = Aow.Rtl.Rtl.Format(call__6, [call__5.FSpellName]);
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__7);
                }
            }

            return Result;
        }

        // WizardsTower.pas:168
        public override void NewDay()
        {
            base.NewDay();
            if ((Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1) && (FGenerateSpell || (FSpellID == 0)))
            {
                GenerateSettings();
            }
        }

        // WizardsTower.pas:126
        public override void ArmyPlaced(Aow.Units.AoWE.TArmyHS A1)
        {
            Aow.Units.AoWE.TArmy Army = default;
            Aow.Units.AoWE.TAIGroup Group = default;
            int I = default;
            Aow.Rtl.AnsiString Reason = default;
            int Cost = default;
            bool Found = default;
            int Location = default;
            base.ArmyPlaced(A1);
            var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FArmy.FPlayer);
            bool bool__9 = TSet<Aow.Units.AoWE.TPlayerType>.Of(Aow.Units.AoWE.TPlayerType.ptCPU1, Aow.Units.AoWE.TPlayerType.ptCPU2, Aow.Units.AoWE.TPlayerType.ptCPU3, Aow.Units.AoWE.TPlayerType.ptCPU4, Aow.Units.AoWE.TPlayerType.ptCPU5).Contains(call__8.FPlayerType);
            if (bool__9)
            {
                var call__10 = ValidAITarget(unchecked((sbyte)A1.FArmy.FPlayer));
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
                var call__14 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)call__13), unchecked((sbyte)call__12), unchecked((sbyte)call__11));
                Location = Unsafe.BitCast<Aow.Units.HSEngine.TXYL, int>(call__14);
                while (true)
                {
                    bool bool__15 = !Found;
                    if (bool__15)
                    {
                        var call__16 = Army.Count;
                        bool__15 = (I != call__16);
                    }

                    if (!bool__15)
                        break;
                    {
                        var call__17 = Army.GetUnit(I);
                        Group = call__17.FGroup;
                        Found = ((Group != null) && (Unsafe.As<Aow.Units.HSEngine.TXYL, int>(ref Group.FTargetXYL) == Location)) && (Group.FGoldBudget == Cost);
                        I += 1;
                    }
                }

                bool bool__18 = Found;
                if (bool__18)
                {
                    var call__19 = CanBuy(unchecked((sbyte)Army.FPlayer), ref Reason);
                    bool__18 = call__19;
                }

                if (bool__18)
                {
                    Group.ReturnTargetExpense();
                    var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Army.FPlayer);
                    var call__21 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Army.FPlayer);
                    call__21.SetGems(call__20.FGems - Cost);
                    var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Army.FPlayer);
                    call__22.FMagicControl.FSpells.Add(FSpellID);
                }
            }
        }

        // WizardsTower.pas:107
        public override void UpdateAITarget(in Aow.Units.AoWE.TAITargetInfo Info, ref Aow.Units.AoWE.TAITargetData Data)
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
                    case Aow.Units.AoWE.TAITargetMode.tmAttack:
                        var call__26 = ((Aow.Units.AoWE.TAoWMapField)Owner).GetPlayer();
                        Player = unchecked((sbyte)call__26);
                        bool bool__27 = (Player != Info.Player);
                        if (bool__27)
                        {
                            bool bool__28 = (Player <= 0);
                            if (!bool__28)
                            {
                                var call__29 = Aow.Units.AoWE.AoWE.ValidAttackRelation(unchecked((sbyte)Info.Player), 0, unchecked((sbyte)Player), 0);
                                bool__28 = call__29;
                            }

                            bool__27 = bool__28;
                        }

                        bool bool__30 = bool__27;
                        if (bool__30)
                        {
                            var call__31 = ValidAITarget(unchecked((sbyte)Info.Player));
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
            var call__33 = CanEnter(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Result = call__33;
            return Result;
        }

        // WizardsTower.pas:99
        public bool ValidAITarget(sbyte Player)
        {
            bool Result = default;
            bool bool__34 = (Player > 0);
            if (bool__34)
            {
                var call__35 = GetRazed();
                bool__34 = !call__35;
            }

            bool bool__36 = bool__34 && (FSpellID != 0);
            if (bool__36)
            {
                var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                var call__38 = call__37.FMagicControl.FSpells.IndexOf(FSpellID);
                bool__36 = (call__38 == -1);
            }

            if (bool__36)
            {
                var call__39 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
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
            var call__40 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetRandomSpell(FSpellLevel, FSpellLevel, FMagicSpheres);
            FSpellID = call__40;
        }

        // WizardsTower.pas:174
        public bool CanEnter(sbyte Player)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            var call__41 = Owner.FindOwnedChild(131607);
            ArmyHS = (Aow.Units.AoWE.TArmyHS)call__41;
            if ((ArmyHS == null) || (ArmyHS.FArmy.FPlayer != Player))
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
            Aow.Units.WizardsTower.TEnterWizardsTowerEventLog Log = default;
            var call__42 = CanEnter(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            if (call__42)
            {
                Log = Aow.Units.WizardsTower.TEnterWizardsTowerEventLog.Create(null);
                Log.FTower = this;
                Log.Execute(true, Aow.Units.EventLog.TExecuteTrigger.etUser, null);
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
                var call__43 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                Result = call__43.FResearchCost;
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // WizardsTower.pas:221
        public bool CanBuy(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            if (FSpellID != 0)
            {
                var call__44 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                var call__45 = call__44.FMagicControl.FSpells.IndexOf(FSpellID);
                if (call__45 == -1)
                {
                    var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__47 = BuyCost();
                    if (call__46.FGems >= call__47)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        var call__48 = Aow.Units.AoWE.AoWE.NotEnoughGoldRStr;
                        var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(call__48);
                        Reason = call__49;
                    }
                }
                else
                {
                    Result = false;
                    var call__50 = Aow.Units.AoWE.AoWE.SpellIsAlreadyResearchedRStr;
                    var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(call__50);
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
        public bool Buy(sbyte Player, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Aow.Units.AoWE.TStructureTE TE = default;
            Reason = (AnsiString)"";
            var call__52 = CanBuy(unchecked((sbyte)Player), ref Reason);
            bool bool__53 = call__52;
            if (bool__53)
            {
                var call__54 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                var call__55 = call__54.GetBusy();
                bool__53 = !call__55;
            }

            if (bool__53)
            {
                TE = CreateTE();
                SetupTE(TE);
                TE.FAction = 1;
                TE.FParam = Player;
                var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(Player);
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
        public static new Aow.Units.WizardsTower.TWizardsTower Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.WizardsTower.TWizardsTower o = new Aow.Units.WizardsTower.TWizardsTower();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TStructure.Meta
        {
            // WizardsTower.pas:84
            public override int ClassID()
            {
                int Result = default;
                Result = 133278;
                return Result;
            }

            // WizardsTower.pas:24
            public new Aow.Units.WizardsTower.TWizardsTower Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.WizardsTower.TWizardsTower o = (Aow.Units.WizardsTower.TWizardsTower)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.WizardsTower.TWizardsTower => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TWizardsTowerResource: size $64, VMT 557D5774; no additional fields. }
    public partial class TWizardsTowerResource : Aow.Units.AoWE.TStructureResource
    {
        // WizardsTower.pas:266
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133278;
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWE.TStructureResource.Meta
        {
            // WizardsTower.pas:263
            public override int ClassID()
            {
                int Result = default;
                Result = 133279;
                return Result;
            }

            // WizardsTower.pas:260
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__57 = Aow.Units.AoWE.AoWE.WizardsTowerRStr;
                var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(call__57);
                Result = call__58;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.WizardsTower.TWizardsTowerResource => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:200
            public new Aow.Units.WizardsTower.TWizardsTowerResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.WizardsTower.TWizardsTowerResource o = (Aow.Units.WizardsTower.TWizardsTowerResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TEnterWizardsTowerEventLog: size $1C, VMT 557D587C; $18 tower. }
    public partial class TEnterWizardsTowerEventLog : Aow.Units.EventLog.TEventLog
    {
        public Aow.Units.WizardsTower.TWizardsTower FTower;
        // WizardsTower.pas:73
        // { $18 }
        public override void Destroy()
        {
            base.Destroy();
        }

        // WizardsTower.pas:70
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // WizardsTower.pas:53
        public static new Aow.Units.WizardsTower.TEnterWizardsTowerEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.WizardsTower.TEnterWizardsTowerEventLog o = new Aow.Units.WizardsTower.TEnterWizardsTowerEventLog();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class WizardsTower
    {
        // WizardsTower.pas:269
        public static void RegisterWizardsTower(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.WizardsTower.TWizardsTower>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.WizardsTower.TWizardsTowerResource>()]);
        }

        public static TSet<Aow.Units.AoWE.TMagicSphere> AllMagicSpheres = TSet<Aow.Units.AoWE.TMagicSphere>.FromMask(255UL);
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
