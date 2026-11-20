#nullable disable
// Translated from HolyG.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Terrain;

// Mode, Player, Duration, OriginalOverlay
public partial class HolyGround(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

    // HolyG.pas:92
    public new const int ClassId = 131987;

    public Set<HolySummoning> Mode { get; set; }

    public sbyte Player { get; set; }
    public sbyte Duration { get; set; }
    public sbyte OriginalOverlay { get; set; }

    // HolyG.pas:201
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.SByte("Player", value: Player, fallback: -1);
        Duration = persister.SByte("Duration", value: Duration, fallback: -1);
        OriginalOverlay = persister.SByte("OriginalOverlay", value: OriginalOverlay, fallback: -1);
    }

    // HolyG.pas:243
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn((sbyte)message.Payload.Int);
        }
    }

    // HolyG.pas:249
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (!Mode.Contains(HolySummoning.Summoning))
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // HolyG.pas:187
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            sbyte hexY = GetHexY();
            sbyte hexX = GetHexX();
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // HolyG.pas:193
    public override void RemoveFromMap()
    {
        MapLevel level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, StoredHexX, StoredHexY);
    }

    // HolyG.pas:219
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4418)
        {
            if (base.Resource.TerrainList.Terrain[message.HexNumber] >= 0 && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
                return;
            }

            return;
        }

        if (message.MessageId == 4384)
        {
            if (base.Resource.TerrainList.Terrain[message.HexNumber] >= 0)
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
                if (base.Resource.TerrainList.Terrain[message.HexNumber] >= 0 && Player > 0 && Duration > 0)
                {
                    RestoreTerrain();
                }
            }
        }
    }

    // HolyG.pas:209
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        return base.Resource.TerrainList.Terrain[info.HexNumber] >= 0
            ? !CanDoDamage(((MoveControl)info.Sender).Settings.Units.Units) ? 1 : 0
            : 1;
    }

    // HolyG.pas:181
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (base.Resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // HolyG.pas:95
    public void TriggerArmyDamage(ArmyHexSprite army)
    {
        int damaged = 0;
        RandSeed = StrategicMap.Current.Random(16777215);
        ArmyEventLog log = new(null)
        {
            Holder = CreationHolder(),
            ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_holyGroundDamageText)
        };
        string holyGroundDamage = AbilityRules.TranslateResourceString(GetEngine(), s_holyGroundDamageText);
        _ = log.ArmyMessages.Add(holyGroundDamage);
        log.ArmyIcon = 2;
        log.Player = army.Army.Player;
        log.ArmyView.Setup(army.Army, 255);
        Army armyLocal = army.Army;
        for (int i = 0, last = armyLocal.Units.Count - 1; i <= last; i++)
        {
            Troop troop = armyLocal.Units.GetUnit(i);
            bool condition = troop.GetAlignment() != Alignment.None;
            if (condition)
            {
                Alignment alignment = troop.GetAlignment();
                condition = AlignmentTables.PureEvilAlignments.Contains(alignment);
            }

            if (condition && !troop.GetAbilityEnabled(1))
            {
                int damage = troop.ExecuteDamageRole(3, 4, Set<DamageType>.Of(DamageType.Holy));
                if (damage > troop.GetHitPoints())
                {
                    damage = troop.GetHitPoints();
                }

                if (damage != 0)
                {
                    troop.SetHitPoints((sbyte)(troop.GetHitPoints() - damage));
                    damaged += 1;
                }

                _ = log.DamageValues.Add((byte)damage);
                _ = log.DamageTypes.Add((int)DamageType.Holy);
            }
            else
            {
                _ = log.DamageValues.Add(0);
                _ = log.DamageTypes.Add((int)DamageType.None);
            }
        }

        log.ValidateGameOver();
        if (damaged != 0)
        {
            EventLog.DistributeLocationEventLog(y: GetHexY(), level: GetHexLevel(), x: GetHexX(), log: log, player: log.Player, execute: true);
        }

        _ = log.Release();
        army.Army.Validate();
    }

    // HolyG.pas:139
    public void ArmyPlaced(ArmyHexSprite army)
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            TriggerArmyDamage(army);
        }
    }

    // HolyG.pas:144
    public void RestoreTerrain()
    {
        MapField field = Owner;
        sbyte overlay = OriginalOverlay;
        Dispose();
        if (overlay > 0)
        {
            StrategicMap.Current.PlaceTerrain(field.HexX, field.HexY, field.HexLevel, field.TerrainType, overlay);
        }
    }

    // HolyG.pas:154
    public void NewTurn(sbyte player)
    {
        ArmyHexSprite army = Owner.GetArmyHexSprite();
        if (army is not null && player == army.Army.Player)
        {
            TriggerArmyDamage(army);
        }

        if (player == Player && Duration > 0)
        {
            Duration = (sbyte)(Duration - 1);
            if (Duration == 0)
            {
                RestoreTerrain();
            }
        }
    }

    // HolyG.pas:166
    public bool CanDoDamage(UnitCollection units)
    {
        bool result = false;
        int i = units.Count;
        while (!result && i != 0)
        {
            i -= 1;
            Troop troop = units.GetUnit(i);
            bool condition = !troop.GetImmunityTypes().Contains(DamageType.Holy) && troop.GetAlignment() != Alignment.None;
            if (condition)
            {
                Alignment alignment = troop.GetAlignment();
                condition = AlignmentTables.PureEvilAlignments.Contains(alignment);
            }

            result = condition && !troop.GetAbilityEnabled(1);
        }

        return result;
    }

    // HolyG.pas:254
    public void ShowSummonAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        sbyte hexLevel = animation.GetHexLevel();
        sbyte hexY = animation.GetHexY();
        sbyte hexX = animation.GetHexX();
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(131987) && GetEngine().AnimationDetail != 0 && animation.FrameCounter == 0)
        {
            bool condition = StrategicMap.Current.SeatedPlayer == animation.Tag;
            if (!condition)
            {
                sbyte hexLevel2 = GetHexLevel();
                sbyte hexY2 = GetHexY();
                sbyte hexX2 = GetHexX();
                condition = StrategicMap.Current.WatchingTerrain(hexX2, hexY2, hexLevel2, 0);
            }

            if (condition)
            {
                _ = GamePack.Current.SpellControl.GetSpell(13).CastSounds.Play(0);
            }
        }
    }

    // HolyG.pas:265
    public void NewFrameSummonAnimation(Animation animation, int frameCounter)
    {
        sbyte hexLevel = animation.GetHexLevel();
        sbyte hexY = animation.GetHexY();
        sbyte hexX = animation.GetHexX();
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(131987) && frameCounter == 9)
        {
            Mode -= Set<HolySummoning>.Of(HolySummoning.Summoning);
        }
    }

    private static readonly string s_holyGroundDamageText = "Holy Ground Damage";

    public new partial class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {

        // HolyG.pas:276
        public new const int ClassId = 131988;

        // HolyG.pas:279
        public override int ResourceUserClassId() => 131987;

        // HolyG.pas:273
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_holyGroundText);

        private static readonly string s_holyGroundText = "Holy Ground";

    }

}
