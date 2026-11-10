// Translated from WizardsTower.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures
{

    // generate, spheres, level, spell ID
    public partial class WizardsTower : Structure
    {
        public WizardsTower(ECustomNode owner) : base(owner)
        {
            FGenerateSpell = true;
            FMagicSpheres = AllMagicSpheres;
            FSpellLevel = 2;
        }

        // WizardsTower.pas:84
        public new const int ClassID = 133278;

        public bool FGenerateSpell;
        public Set<MagicSphere> FMagicSpheres;
        public byte FSpellLevel;
        public int FSpellID;

        // WizardsTower.pas:87
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwBool(ref FGenerateSpell, 30);
            RwSet(ref FMagicSpheres, 2, 31, Stream.rwData, 0);
            Stream.rwByte(ref FSpellLevel, 32);
            Stream.rwData(Bytes.Of(ref FSpellID, 4), 4, 33);
        }

        // WizardsTower.pas:76

        // WizardsTower.pas:196
        public override ExecuteResult ExecuteTE(StructureTE TE, TokenExecuter Executer)
        {
            Player Player;
            string Reason = "";
            ExecuteResult Result = base.ExecuteTE(TE, Executer);
            if (TE.FAction == 1 && CanBuy((sbyte)TE.FParam, ref Reason))
            {
                Player = AoWHSMap.Current.FPlayers.GetPlayers(TE.FParam);
                var buyCost = BuyCost();
                Player.SetGems(Player.FGems - buyCost);
                Player.FMagicControl.FSpells.Add(FSpellID);
                if (AoWHSMap.Current.FSeatedPlayer == TE.FParam)
                {
                    var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                    var format = RtlUnit.Format(AbilityRules.TranslateRStr(TheyTaughtYouTheSpellXRStr), [spell.FSpellName]);
                    AoWHSMap.Current.ShowMessage(format);
                }
            }

            return Result;
        }

        // WizardsTower.pas:168
        public override void NewDay()
        {
            base.NewDay();
            if (AoWHSMap.Current.FDay == 1 && (FGenerateSpell || FSpellID == 0))
            {
                GenerateSettings();
            }
        }

        // WizardsTower.pas:126
        public override void ArmyPlaced(ArmyHS PlacedArmy)
        {
            Army Army;
            AIGroup Group;
            int I;
            string Reason = "";
            int Cost;
            bool Found;
            int Location;
            base.ArmyPlaced(PlacedArmy);
            var player = AoWHSMap.Current.FPlayers.GetPlayers(PlacedArmy.FArmy.FPlayer);
            if (Set<PlayerType>.Of(PlayerType.CPU1, PlayerType.CPU2, PlayerType.CPU3, PlayerType.CPU4, PlayerType.CPU5).Contains(player.FPlayerType) && ValidAITarget(PlacedArmy.FArmy.FPlayer))
            {
                Army = PlacedArmy.FArmy;
                Group = null;
                I = 0;
                Cost = BuyCost();
                Found = false;
                var lhx = GetLhx();
                var yhx = GetYhx();
                Location = Unsafe.BitCast<XYL, int>(HexGeometry.PackXYL(GetXhx(), yhx, lhx));
                while (!Found && I != Army.Count)
                {
                    Group = Army.GetUnit(I).FGroup;
                    Found = Group != null && Unsafe.As<XYL, int>(ref Group.FTargetXYL) == Location && Group.FGoldBudget == Cost;
                    I += 1;
                }

                if (Found && CanBuy(Army.FPlayer, ref Reason))
                {
                    Group.ReturnTargetExpense();
                    var playerGems = AoWHSMap.Current.FPlayers.GetPlayers(Army.FPlayer);
                    AoWHSMap.Current.FPlayers.GetPlayers(Army.FPlayer).SetGems(playerGems.FGems - Cost);
                    AoWHSMap.Current.FPlayers.GetPlayers(Army.FPlayer).FMagicControl.FSpells.Add(FSpellID);
                }
            }
        }

        // WizardsTower.pas:107
        public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
        {
            sbyte Player;
            if (!GetRazed() || GetBuilding())
            {
                switch (Info.Mode)
                {
                    case AITargetMode.Attack:
                        Player = ((AoWMapField)Owner).GetPlayer();
                        if (Player != Info.Player && (Player <= 0 || AbilityRules.ValidAttackRelation(Info.Player, 0, Player, 0)) && ValidAITarget(Info.Player))
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
        public override bool CanDblClick() => CanEnter(AoWHSMap.Current.FSeatedPlayer);

        // WizardsTower.pas:99
        public bool ValidAITarget(sbyte Player)
        {
            bool Result;
            if (Player > 0 && !GetRazed() && FSpellID != 0 && AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FSpells.IndexOf(FSpellID) == -1)
            {
                Result = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastDuration > 0;
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
            FSpellID = AoWHSSet.Current.FSpellControl.GetRandomSpell(FSpellLevel, FSpellLevel, FMagicSpheres);
        }

        // WizardsTower.pas:174
        public bool CanEnter(sbyte Player)
        {
            bool Result;
            ArmyHS ArmyHS;
            ArmyHS = (ArmyHS)Owner.FindOwnedChild(131607);
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
            EnterWizardsTowerEventLog Log;
            if (CanEnter(AoWHSMap.Current.FSeatedPlayer))
            {
                Log = new EnterWizardsTowerEventLog(null);
                Log.FTower = this;
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

        // WizardsTower.pas:215
        public int BuyCost()
        {
            int Result;
            if (FSpellID != 0)
            {
                Result = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FResearchCost;
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
                if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FSpells.IndexOf(FSpellID) == -1)
                {
                    var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                    var buyCost = BuyCost();
                    if (player.FGems >= buyCost)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        Reason = AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
                    }
                }
                else
                {
                    Result = false;
                    Reason = AbilityRules.TranslateRStr(SpellIsAlreadyResearchedRStr);
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
            StructureTE TE;
            Reason = "";
            if (CanBuy(Player, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Player).GetBusy())
            {
                TE = CreateTE();
                SetupTE(TE);
                TE.FAction = 1;
                TE.FParam = Player;
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

        private static readonly string SpellIsAlreadyResearchedRStr = "Spell already researched";
        private static readonly string TheyTaughtYouTheSpellXRStr = "They taught you the spell %s";
        private static Set<MagicSphere> AllMagicSpheres = Set<MagicSphere>.FromMask(255uL);

        public new partial class Resource : StructureResource
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            // WizardsTower.pas:263
            public new const int ClassID = 133279;

            // WizardsTower.pas:266
            public override int ResourceUserClassID() => 133278;

            // WizardsTower.pas:260
            public override string ResourceEditName() => AbilityRules.TranslateRStr(WizardsTowerRStr);

            private static readonly string WizardsTowerRStr = "Wizards Tower";

        }

        // WizardsTower.pas:24

        // WizardsTower.pas:24

    }
}

namespace Aow.Game.Events
{

    // tower
    public partial class EnterWizardsTowerEventLog : EventLog
    {
        // WizardsTower.pas:70
        public EnterWizardsTowerEventLog(ECustomNode owner) : base(owner)
        { }

        public WizardsTower FTower;

    }
}
