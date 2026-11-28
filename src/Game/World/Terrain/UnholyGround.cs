#nullable disable warnings
// Translated from UnHolyG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Terrain;

// Mode, Player, Duration, OriginalOverlay
public class UnholyGround(CustomNode? owner) : ImageLibraryTerrainMapObject(owner)
{

    // UnHolyG.pas:93
    public new const int ClassId = 131_985;

    public UnholySummonings Mode { get; set; }

    public PlayerId? Player { get; set; } = PlayerId.Independent;
    public int Duration { get; set; }
    public int OriginalOverlay { get; set; }

    // UnHolyG.pas:204
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.OptionalPlayer("Player", value: Player, fallback: -1, width: IdWidth.SByte);
        Duration = persister.SByte("Duration", value: Duration.TruncateToSByte(), fallback: -1);
        OriginalOverlay = persister.SByte("OriginalOverlay", value: OriginalOverlay.TruncateToSByte(), fallback: -1);
    }

    // UnHolyG.pas:243
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn(PlayerId.FromInt32((sbyte)message.Payload.Int));
        }
    }

    // UnHolyG.pas:249
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (!Mode.Contains(UnholySummonings.Summoning))
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // UnHolyG.pas:190
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            int hexY = HexY;
            int hexX = HexX;
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // UnHolyG.pas:196
    public override void RemoveFromMap()
    {
        MapLevel level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, StoredHexX, StoredHexY);
    }

    // UnHolyG.pas:222
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4418)
        {
            if (base.Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
            {
                ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
                return;
            }
        }
        else
        {
            if (message.MessageId == 4384)
            {
                if (base.Resource.TerrainList.Terrain[message.HexNumber].Value is not null)
                {
                    if (CanDoDamage(message.Param[0].As<ArmyHexSprite>().Army.Units))
                    {
                        message.Result = 1;
                    }
                }
            }
            else
            {
                if (message.MessageId == 4402)
                {
                    if (base.Resource.TerrainList.Terrain[message.HexNumber].Value is not null && PlayerIdExtensions.ToOriginal(Player) > 0 && Duration > 0)
                    {
                        RestoreTerrain();
                    }
                }
            }
        }
    }

    // UnHolyG.pas:212
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        return base.Resource.TerrainList.Terrain[info.HexNumber].Value is not null
            ? !CanDoDamage(((MoveControl)info.Sender).Settings.Units.Units) ? 1 : 0
            : 1;
    }

    // UnHolyG.pas:184
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (base.Resource.TerrainList.Terrain[info.HexNumber].Value is not null)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // UnHolyG.pas:96
    public void TriggerArmyDamage(ArmyHexSprite? army)
    {
        int damaged = 0;
        RandSeed = GetEngine().StrategicMap.Random(16777215);
        ArmyEventLog log = new(null, holder: CreationHolder())
        {
            ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: s_unholyGroundDamageText)
        };
        string unholyGroundDamage = AbilityRules.TranslateResourceString(GetEngine(), S: s_unholyGroundDamageText);
        _ = log.ArmyMessages.Add(unholyGroundDamage);
        log.ArmyIcon = 2;
        Army? armyLocal = army.Army;
        for (int i = 0, last = armyLocal.Units.Count - 1; i <= last; i++)
        {
            Troop? troop = armyLocal.Units.GetUnit(i);
            bool condition = troop.GetAlignment() != Alignment.Unaligned;
            if (condition)
            {
                Alignment alignment = troop.GetAlignment();
                condition = AlignmentTables.PureGoodAlignments.Contains(alignment.ToFlag());
            }

            if (condition && !troop.GetAbilityEnabled(1))
            {
                int damage = troop.ExecuteDamageRole(4, damage: 3, damageTypes: DamageTypes.Death);
                if (damage > troop.GetHitPoints())
                {
                    damage = troop.GetHitPoints();
                }

                if (damage != 0)
                {
                    troop.SetHitPoints((sbyte)(troop.GetHitPoints() - damage));
                    damaged += 1;
                }

                _ = log.DamageValues.Add(damage.TruncateToByte());
                _ = log.DamageTypes.Add((int)DamageType.Death);
            }
            else
            {
                _ = log.DamageValues.Add(0);
                _ = log.DamageTypes.Add((int)DamageType.Death);
            }
        }

        log.Player = armyLocal.Player;
        ArmyHexSprite? armyHexSprite = armyLocal.GetArmyHexSprite();
        log.ArmyView.Setup(armyHexSprite.Army, selection: 255);

        log.ValidateGameOver();
        if (damaged != 0)
        {
            EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, x: HexX, log: log, player: log.Player, execute: true);
        }

        _ = log.Release();
        army.Army.Validate();
    }

    // UnHolyG.pas:142
    public void ArmyPlaced(ArmyHexSprite? army)
    {
        if (!GetEngine().StrategicMap.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            TriggerArmyDamage(army);
        }
    }

    // UnHolyG.pas:147
    public void RestoreTerrain()
    {
        MapField? field = Owner;
        int overlay = OriginalOverlay;
        Dispose();
        if (overlay > 0)
        {
            GetEngine().StrategicMap.PlaceTerrain(field.HexX, field.HexY, L: field.HexLevel, terrainType: field.TerrainNumber, overlayType: overlay);
        }
    }

    // UnHolyG.pas:157
    public void NewTurn(PlayerId player)
    {
        ArmyHexSprite army = Owner.GetArmyHexSprite();
        if (army is not null && player == army.Army.Player)
        {
            TriggerArmyDamage(army);
        }

        if (player == Player && Duration > 0)
        {
            Duration = (Duration - 1).TruncateToSByte();
            if (Duration == 0)
            {
                RestoreTerrain();
            }
        }
    }

    // UnHolyG.pas:169
    public bool CanDoDamage(UnitCollection units)
    {
        bool result = false;
        int i = units.Count;
        while (!result && i != 0)
        {
            i -= 1;
            Troop? troop = units.GetUnit(i);
            bool condition = !troop.GetImmunityTypes().Contains(DamageTypes.Death) && troop.GetAlignment() != Alignment.Unaligned;
            if (condition)
            {
                Alignment alignment = troop.GetAlignment();
                condition = AlignmentTables.PureGoodAlignments.Contains(alignment.ToFlag());
            }

            result = condition && !troop.GetAbilityEnabled(1);
        }

        return result;
    }

    // UnHolyG.pas:254
    public void ShowSummonAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int hexLevel = animation.HexLevel;
        int hexY = animation.HexY;
        int hexX = animation.HexX;
        StrategicMap strategicMap = GetEngine().StrategicMap;
        MapField field = strategicMap.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(ClassId) && GetEngine().AnimationDetail != 0 && animation.FrameCounter == 0)
        {
            bool condition = PlayerIdExtensions.ToOriginal(strategicMap.SeatedPlayer) == animation.Tag;
            if (!condition)
            {
                int hexLevel2 = HexLevel;
                int hexY2 = HexY;
                int hexX2 = HexX;
                condition = strategicMap.WatchingTerrain(hexX2, hexY2, L: hexLevel2, radius: 0);
            }

            if (condition)
            {
                _ = GetEngine().GamePack.SpellControl[14].CastSounds.Play(0);
            }
        }
    }

    // UnHolyG.pas:265
    public void NewFrameSummonAnimation(Animation animation, int frameCounter)
    {
        int hexLevel = animation.HexLevel;
        int hexY = animation.HexY;
        int hexX = animation.HexX;
        MapField field = GetEngine().StrategicMap.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(ClassId) && frameCounter == 9)
        {
            Mode &= ~UnholySummonings.Summoning;
        }
    }

    private static readonly string s_unholyGroundDamageText = "Unholy Ground Damage";

    public new class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {

        // UnHolyG.pas:278
        public new const int ClassId = 131_986;

        // UnHolyG.pas:283
        public override int ResourceUserClassId() => UnholyGround.ClassId;

        // UnHolyG.pas:273
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_unholyGroundText);

        private static readonly string s_unholyGroundText = "Unholy Ground";

    }
}
