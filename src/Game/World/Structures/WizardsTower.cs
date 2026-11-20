#nullable disable
// Translated from WizardsTower.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// generate, spheres, level, spell ID
public partial class WizardsTower(CustomNode owner) : Structure(owner)
{

    // WizardsTower.pas:84
    public new const int ClassId = 133278;

    private bool _generateSpell = true;
    private Set<MagicSphere> _magicSpheres = s_allMagicSpheres;
    private byte _spellLevel = 2;

    public int SpellId { get; private set; }

    // WizardsTower.pas:87
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _generateSpell = persister.Boolean("GenerateSpell", value: _generateSpell);
        _magicSpheres = persister.Set("MagicSpheres", value: _magicSpheres);
        _spellLevel = persister.Byte("SpellLevel", value: _spellLevel);
        SpellId = persister.RawInt32("SpellId", value: SpellId);
    }

    // WizardsTower.pas:196
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        string reason = "";
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 1 && CanBuy((sbyte)tokenEvent.Param, ref reason))
        {
            Player player = StrategicMap.Current.Players.GetPlayers(tokenEvent.Param);
            int buyCost = BuyCost();
            player.SetGems(player.Gems - buyCost);
            _ = player.MagicControl.Spells.Add(SpellId);
            if (StrategicMap.Current.SeatedPlayer == tokenEvent.Param)
            {
                Spell spell = GamePack.Current.SpellControl[SpellId];
                string format = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), s_theyTaughtYouSpellFormat), [spell.SpellName]);
                StrategicMap.Current.ShowMessage(format);
            }
        }

        return result;
    }

    // WizardsTower.pas:168
    public override void NewDay()
    {
        base.NewDay();
        if (StrategicMap.Current.Day == 1 && (_generateSpell || SpellId == 0))
        {
            GenerateSettings();
        }
    }

    // WizardsTower.pas:126
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        Player player = StrategicMap.Current.Players.GetPlayers(placedArmy.Army.Player);
        if (Set<PlayerType>.Of(PlayerType.Cpu1, PlayerType.Cpu2, PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(player.PlayerType) && ValidAITarget(placedArmy.Army.Player))
        {
            Army army = placedArmy.Army;
            AIGroup group = null;
            int i = 0;
            int cost = BuyCost();
            bool found = false;
            MapCoordinate location = HexGeometry.PackMapCoordinate(mapLevel: HexLevel, y: HexY, x: HexX);
            while (!found && i != army.Units.Count)
            {
                group = army.Units.GetUnit(i).Group;
                found = group is not null && group.TargetLocation == location && group.GoldBudget == cost;
                i += 1;
            }

            string reason = "";
            if (found && CanBuy(army.Player, ref reason))
            {
                group.ReturnTargetExpense();
                Player playerGems = StrategicMap.Current.Players.GetPlayers(army.Player);
                StrategicMap.Current.Players.GetPlayers(army.Player).SetGems(playerGems.Gems - cost);
                _ = StrategicMap.Current.Players.GetPlayers(army.Player).MagicControl.Spells.Add(SpellId);
            }
        }
    }

    // WizardsTower.pas:107
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        if (!GetRazed() || Building)
        {
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                    sbyte player = Owner.GetPlayer();
                    if (player != info.Player && (player <= 0 || AbilityRules.ValidAttackRelation(info.Player, 0, player, 0)) && ValidAITarget(info.Player))
                    {
                        if (data.Priority < 10)
                        {
                            data.Priority = 10;
                        }

                        int buyCost = BuyCost();
                        data.Cost = buyCost;
                    }

                    break;
                case AITargetMode.Raid:
                case AITargetMode.Recruit:
                case AITargetMode.Produce:
                case AITargetMode.Hunt:
                case AITargetMode.Fortify:
                case AITargetMode.Defend:
                case AITargetMode.GuardArea:
                case AITargetMode.AttackArmies:
                default:
                    break;
            }
        }
    }

    // WizardsTower.pas:96
    public override bool CanDoubleClick() => CanEnter(StrategicMap.Current.SeatedPlayer);

    // WizardsTower.pas:99
    public bool ValidAITarget(sbyte player) => player > 0 && !GetRazed() && SpellId != 0 && StrategicMap.Current.Players.GetPlayers(player).MagicControl.Spells.IndexOf(SpellId) == -1 && GamePack.Current.SpellControl[SpellId].CastDuration > 0;

    // WizardsTower.pas:163
    public void GenerateSettings() => SpellId = GamePack.Current.SpellControl.GetRandomSpell(_spellLevel, _spellLevel, _magicSpheres);

    // WizardsTower.pas:174
    public bool CanEnter(sbyte player)
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        return armyHexSprite is not null && armyHexSprite.Army.Player == player;
    }

    // WizardsTower.pas:182
    public bool Enter()
    {
        if (CanEnter(StrategicMap.Current.SeatedPlayer))
        {
            EnterWizardsTowerEventLog log = new(null)
            {
                Holder = CreationHolder(),
                Tower = this
            };
            _ = log.Execute(true, ExecuteTrigger.User, null);
            _ = log.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // WizardsTower.pas:215
    public int BuyCost() => SpellId != 0 ? GamePack.Current.SpellControl[SpellId].ResearchCost : -1;

    // WizardsTower.pas:221
    public bool CanBuy(sbyte player, ref string reason)
    {
        bool result;
        if (SpellId != 0)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.Spells.IndexOf(SpellId) == -1)
            {
                Player playerLocal = StrategicMap.Current.Players.GetPlayers(player);
                int buyCost = BuyCost();
                if (playerLocal.Gems >= buyCost)
                {
                    return true;
                }
                else
                {
                    result = false;
                    reason = AbilityRules.TranslateResourceString(GetEngine(), GameText.NotEnoughGoldText);
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GetEngine(), s_spellIsAlreadyResearchedText);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // WizardsTower.pas:243
    public bool Buy(sbyte player, out string reason)
    {
        reason = "";
        if (CanBuy(player, ref reason) && !StrategicMap.Current.Players.GetPlayers(player).GetBusy())
        {
            StructureTokenEvent tokenEvent = CreateTokenEvent();
            SetupTokenEvent(tokenEvent);
            tokenEvent.Action = 1;
            tokenEvent.Param = player;
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_spellIsAlreadyResearchedText = "Spell already researched";
    private static readonly string s_theyTaughtYouSpellFormat = "They taught you the spell %s";
    private static readonly Set<MagicSphere> s_allMagicSpheres = Set<MagicSphere>.FromMask(255uL);

    public new class Resource(CustomNode owner) : StructureResource(owner)
    {

        // WizardsTower.pas:263
        public new const int ClassId = 133279;

        // WizardsTower.pas:266
        public override int ResourceUserClassId() => 133278;

        // WizardsTower.pas:260
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_wizardsTowerText);

        private static readonly string s_wizardsTowerText = "Wizards Tower";

    }
}
