// Translated from WizardsTower.pas.
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// generate, spheres, level, spell ID
public partial class WizardsTower : Structure
{
    public WizardsTower(ECustomNode owner) : base(owner)
    {
        GenerateSpell = true;
        MagicSpheres = s_allMagicSpheres;
        SpellLevel = 2;
    }

    // WizardsTower.pas:84
    public new const int ClassId = 133278;

    public bool GenerateSpell;
    public Set<MagicSphere> MagicSpheres;
    public byte SpellLevel;
    public int SpellId;

    // WizardsTower.pas:87
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwBool(ref GenerateSpell, 30);
        RwSet(ref MagicSpheres, 2, 31, stream.rwData, 0);
        stream.rwByte(ref SpellLevel, 32);
        stream.rwData(Bytes.Of(ref SpellId, 4), 4, 33);
    }

    // WizardsTower.pas:76

    // WizardsTower.pas:196
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        Player player;
        string reason = "";
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 1 && CanBuy((sbyte)tokenEvent.Param, ref reason))
        {
            player = AoWHSMap.Current.Players.GetPlayers(tokenEvent.Param);
            var buyCost = BuyCost();
            player.SetGems(player.Gems - buyCost);
            player.MagicControl.Spells.Add(SpellId);
            if (AoWHSMap.Current.SeatedPlayer == tokenEvent.Param)
            {
                var spell = AoWHSSet.Current.SpellControl.GetSpell(SpellId);
                var format = RtlUnit.Format(AbilityRules.TranslateResourceString(s_theyTaughtYouSpellFormat), [spell.SpellName]);
                AoWHSMap.Current.ShowMessage(format);
            }
        }

        return result;
    }

    // WizardsTower.pas:168
    public override void NewDay()
    {
        base.NewDay();
        if (AoWHSMap.Current.Day == 1 && (GenerateSpell || SpellId == 0))
        {
            GenerateSettings();
        }
    }

    // WizardsTower.pas:126
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        Army army;
        AIGroup group;
        int I;
        string reason = "";
        int cost;
        bool found;
        int location;
        base.ArmyPlaced(placedArmy);
        var player = AoWHSMap.Current.Players.GetPlayers(placedArmy.Army.Player);
        if (Set<PlayerType>.Of(PlayerType.Cpu1, PlayerType.Cpu2, PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(player.PlayerType) && ValidAITarget(placedArmy.Army.Player))
        {
            army = placedArmy.Army;
            group = null;
            I = 0;
            cost = BuyCost();
            found = false;
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            location = Unsafe.BitCast<MapCoordinate, int>(HexGeometry.PackMapCoordinate(GetHexX(), hexY, hexLevel));
            while (!found && I != army.Count)
            {
                group = army.GetUnit(I).Group;
                found = group != null && Unsafe.As<MapCoordinate, int>(ref group.TargetMapCoordinate) == location && group.GoldBudget == cost;
                I += 1;
            }

            if (found && CanBuy(army.Player, ref reason))
            {
                group.ReturnTargetExpense();
                var playerGems = AoWHSMap.Current.Players.GetPlayers(army.Player);
                AoWHSMap.Current.Players.GetPlayers(army.Player).SetGems(playerGems.Gems - cost);
                AoWHSMap.Current.Players.GetPlayers(army.Player).MagicControl.Spells.Add(SpellId);
            }
        }
    }

    // WizardsTower.pas:107
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        sbyte player;
        if (!GetRazed() || GetBuilding())
        {
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                    player = ((AoWMapField)Owner).GetPlayer();
                    if (player != info.Player && (player <= 0 || AbilityRules.ValidAttackRelation(info.Player, 0, player, 0)) && ValidAITarget(info.Player))
                    {
                        if (data.Priority < 10)
                        {
                            data.Priority = 10;
                        }

                        var buyCost = BuyCost();
                        data.Cost = buyCost;
                    }

                    break;
            }
        }
    }

    // WizardsTower.pas:96
    public override bool CanDoubleClick() => CanEnter(AoWHSMap.Current.SeatedPlayer);

    // WizardsTower.pas:99
    public bool ValidAITarget(sbyte player)
    {
        bool result;
        if (player > 0 && !GetRazed() && SpellId != 0 && AoWHSMap.Current.Players.GetPlayers(player).MagicControl.Spells.IndexOf(SpellId) == -1)
        {
            result = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastDuration > 0;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // WizardsTower.pas:163
    public void GenerateSettings()
    {
        SpellId = AoWHSSet.Current.SpellControl.GetRandomSpell(SpellLevel, SpellLevel, MagicSpheres);
    }

    // WizardsTower.pas:174
    public bool CanEnter(sbyte player)
    {
        bool result;
        ArmyHexSprite armyHexSprite;
        armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        if (armyHexSprite == null || armyHexSprite.Army.Player != player)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // WizardsTower.pas:182
    public bool Enter()
    {
        bool result = default;
        EnterWizardsTowerEventLog log;
        if (CanEnter(AoWHSMap.Current.SeatedPlayer))
        {
            log = new EnterWizardsTowerEventLog(null);
            log.Tower = this;
            log.Execute(true, ExecuteTrigger.User, null);
            log.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // WizardsTower.pas:215
    public int BuyCost()
    {
        int result;
        if (SpellId != 0)
        {
            result = AoWHSSet.Current.SpellControl.GetSpell(SpellId).ResearchCost;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // WizardsTower.pas:221
    public bool CanBuy(sbyte player, ref string reason)
    {
        bool result;
        if (SpellId != 0)
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).MagicControl.Spells.IndexOf(SpellId) == -1)
            {
                var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
                var buyCost = BuyCost();
                if (playerLocal.Gems >= buyCost)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    reason = AbilityRules.TranslateResourceString(GameText.s_notEnoughGoldText);
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(s_spellIsAlreadyResearchedText);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // WizardsTower.pas:243
    public bool Buy(sbyte player, ref string reason)
    {
        bool result;
        StructureTokenEvent tokenEvent;
        reason = "";
        if (CanBuy(player, ref reason) && !AoWHSMap.Current.Players.GetPlayers(player).GetBusy())
        {
            tokenEvent = CreateTokenEvent();
            SetupTokenEvent(tokenEvent);
            tokenEvent.Action = 1;
            tokenEvent.Param = player;
            AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_spellIsAlreadyResearchedText = "Spell already researched";
    private static readonly string s_theyTaughtYouSpellFormat = "They taught you the spell %s";
    private static Set<MagicSphere> s_allMagicSpheres = Set<MagicSphere>.FromMask(255uL);

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // WizardsTower.pas:263
        public new const int ClassId = 133279;

        // WizardsTower.pas:266
        public override int ResourceUserClassId() => 133278;

        // WizardsTower.pas:260
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_wizardsTowerText);

        private static readonly string s_wizardsTowerText = "Wizards Tower";

    }

    // WizardsTower.pas:24

    // WizardsTower.pas:24

}
