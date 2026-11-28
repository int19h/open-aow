#nullable disable warnings
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
    public new const int ClassId = 133_278;

    private bool _generateSpell = true;
    private MagicSpheres _magicSpheres = s_allMagicSpheres;
    private int _spellLevel = 2;

    public int SpellId { get; private set; }

    // WizardsTower.pas:87
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _generateSpell = persister.Boolean("GenerateSpell", value: _generateSpell);
        _magicSpheres = persister.Set("MagicSpheres", value: _magicSpheres);
        _spellLevel = persister.Byte("SpellLevel", value: _spellLevel.TruncateToByte());
        SpellId = persister.RawInt32("SpellId", value: SpellId);
    }

    // WizardsTower.pas:196
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        string reason = "";
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 1 && CanBuy(PlayerId.FromOriginal((sbyte)tokenEvent.Param), ref reason))
        {
            Player? player = GetEngine().StrategicMap.Players.GetPlayers(tokenEvent.Param);
            int buyCost = BuyCost();
            player.SetGems(player.Gems - buyCost);
            _ = player.MagicControl.Spells.Add(SpellId);
            if (PlayerIdExtensions.ToOriginal(GetEngine().StrategicMap.SeatedPlayer) == tokenEvent.Param)
            {
                Spell? spell = GetEngine().GamePack.SpellControl[SpellId];
                string format = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_theyTaughtYouSpellFormat), args: [spell.SpellName]);
                GetEngine().StrategicMap.ShowMessage(format);
            }
        }

        return result;
    }

    // WizardsTower.pas:168
    public override void NewDay()
    {
        base.NewDay();
        if (GetEngine().StrategicMap.Day == 1 && (_generateSpell || SpellId == 0))
        {
            GenerateSettings();
        }
    }

    // WizardsTower.pas:126
    public override void ArmyPlaced(ArmyHexSprite? placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        Player? player = GetEngine().StrategicMap.Players.GetPlayers(placedArmy.Army.Player);
        if (player.PlayerType is (>= PlayerType.Cpu1 and <= PlayerType.Cpu3) or PlayerType.Cpu4 or PlayerType.Cpu5 && ValidAITarget(placedArmy.Army.Player))
        {
            Army army = placedArmy.Army;
            AIGroup? group = null;
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
                StrategicMap strategicMap = GetEngine().StrategicMap;
                Player? playerGems = strategicMap.Players.GetPlayers(army.Player);
                strategicMap.Players.GetPlayers(army.Player).SetGems(playerGems.Gems - cost);
                _ = GetEngine().StrategicMap.Players.GetPlayers(army.Player).MagicControl.Spells.Add(SpellId);
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
                    sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(Owner.GetPlayer());
                    if (player != PlayerIdExtensions.ToOriginal(info.Player) && (player <= 0 || AbilityRules.ValidAttackRelation(info.Player, independentRelation: 0, targetPlayer: PlayerId.FromOriginal(player), targetRace: 0, engine: GetEngine()!)) && ValidAITarget(info.Player))
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
    public override bool CanDoubleClick() => CanEnter(GetEngine().StrategicMap.SeatedPlayer);

    // WizardsTower.pas:99
    public bool ValidAITarget(PlayerId? player) => PlayerIdExtensions.ToOriginal(player) > 0 && !GetRazed() && SpellId != 0 && GetEngine().StrategicMap.Players.GetPlayers(player).MagicControl.Spells.IndexOf(SpellId) == -1 && GetEngine().GamePack.SpellControl[SpellId].CastDuration > 0;

    // WizardsTower.pas:163
    public void GenerateSettings() => SpellId = GetEngine().GamePack.SpellControl.GetRandomSpell(_spellLevel, _spellLevel, spheres: _magicSpheres);

    // WizardsTower.pas:174
    public bool CanEnter(PlayerId? player)
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        return armyHexSprite is not null && armyHexSprite.Army.Player == player;
    }

    // WizardsTower.pas:182
    public bool Enter()
    {
        if (CanEnter(GetEngine().StrategicMap.SeatedPlayer))
        {
            EnterWizardsTowerEventLog log = new(null, holder: CreationHolder())
            {
                Tower = this
            };
            _ = log.Execute(true, trigger: ExecuteTrigger.User, done: null);
            _ = log.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // WizardsTower.pas:215
    public int BuyCost() => SpellId != 0 ? GetEngine().GamePack.SpellControl[SpellId].ResearchCost : -1;

    // WizardsTower.pas:221
    public bool CanBuy(PlayerId? player, ref string reason)
    {
        bool result;
        if (SpellId != 0)
        {
            if (GetEngine().StrategicMap.Players.GetPlayers(player).MagicControl.Spells.IndexOf(SpellId) == -1)
            {
                Player? playerLocal = GetEngine().StrategicMap.Players.GetPlayers(player);
                int buyCost = BuyCost();
                if (playerLocal.Gems >= buyCost)
                {
                    return true;
                }
                else
                {
                    result = false;
                    reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughGoldText);
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_spellIsAlreadyResearchedText);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // WizardsTower.pas:243
    public bool Buy(PlayerId? player, out string reason)
    {
        reason = "";
        if (CanBuy(player, ref reason) && !GetEngine().StrategicMap.Players.GetPlayers(player).GetBusy())
        {
            StructureTokenEvent tokenEvent = CreateTokenEvent();
            SetupTokenEvent(tokenEvent);
            tokenEvent.Action = 1;
            tokenEvent.Param = PlayerIdExtensions.ToOriginal(player);
            GetEngine().StrategicMap.TokenManager.GetTokenControl(player).AddEvent(tokenEvent);
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
    private static readonly MagicSpheres s_allMagicSpheres = MagicSpheres.Cosmos | MagicSpheres.Life | MagicSpheres.Death | MagicSpheres.Earth | MagicSpheres.Air | MagicSpheres.Fire | MagicSpheres.Water | MagicSpheres.Secret;

    public new class Resource(CustomNode owner) : StructureResource(owner)
    {

        // WizardsTower.pas:263
        public new const int ClassId = 133_279;

        // WizardsTower.pas:266
        public override int ResourceUserClassId() => WizardsTower.ClassId;

        // WizardsTower.pas:260
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_wizardsTowerText);

        private static readonly string s_wizardsTowerText = "Wizards Tower";

    }
}
