#nullable disable
// Translated from PoisonP.pas.
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

// summoning status. caster player. remaining turns. original overlay
public class PoisonPlant(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

    // PoisonP.pas:87
    public new const int ClassId = 131_998;

    public Set<PoisonPlantStatusElement> PlantStatus { get; set; } = Set<PoisonPlantStatusElement>.Of();

    public sbyte CasterPlayer { get; set; } = -1;
    public sbyte DaysRemaining { get; set; } = -1;
    public sbyte OriginalOverlay { get; set; } = -1;

    // PoisonP.pas:97
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        CasterPlayer = persister.SByte("CasterPlayer", value: CasterPlayer, fallback: -1);
        DaysRemaining = persister.SByte("DaysRemaining", value: DaysRemaining, fallback: -1);
        OriginalOverlay = persister.SByte("OriginalOverlay", value: OriginalOverlay, fallback: -1);
    }

    // PoisonP.pas:255
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn((sbyte)message.Payload.Int);
        }
    }

    // PoisonP.pas:261
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (!PlantStatus.Contains(PoisonPlantStatusElement.Summoning))
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // PoisonP.pas:240
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // PoisonP.pas:246
    public override void RemoveFromMap()
    {
        MapLevel level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, StoredHexX, StoredHexY);
    }

    // PoisonP.pas:218
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4418)
        {
            if (base.Resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
            }
        }
        else
        {
            if (message.MessageId == 4384)
            {
                if (base.Resource.TerrainList.GetTerrain(message.HexNumber) >= 0 && !message.Param[0].As<ArmyHexSprite>().Army.ImmunityTypes(255).Contains(DamageType.Poison) && PoisonPlantActive())
                {
                    message.Result = 1;
                }
            }
            else
            {
                if (message.MessageId == 4402)
                {
                    // { Original excludes player zero from early restoration. }
                    if (base.Resource.TerrainList.GetTerrain(message.HexNumber) >= 0 && CasterPlayer > 0 && DaysRemaining > 0)
                    {
                        RestoreTerrain();
                    }
                }
            }
        }
    }

    // PoisonP.pas:209
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        return base.Resource.TerrainList.GetTerrain(info.HexNumber) >= 0 && PoisonPlantActive()
            ? !CanDoDamage(((MoveControl)info.Sender).Settings.Units) ? 1 : 0
            : 1;
    }

    // PoisonP.pas:203
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // PoisonP.pas:92
    public bool PoisonPlantActive() => !Set<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)TerrainType);

    // PoisonP.pas:105
    public bool CanDoDamage(UnitUserList units)
    {
        bool result = false;
        int i = units.Count;
        while (!result && i != 0)
        {
            i -= 1;
            Troop u = units.GetUnit(i);
            result = !u.GetImmunityTypes().Contains(DamageType.Poison) && !u.GetAbilityEnabled(1);
        }

        return result;
    }

    // PoisonP.pas:120
    public void TriggerArmyDamage(ArmyHexSprite armyHexSprite)
    {
        int count = 0;
        RandSeed = StrategicMap.Current.Random(16777215);
        ArmyEventLog log = new(null)
        {
            Holder = CreationHolder(),
            ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: s_poisonPlantDamageText),
            Player = armyHexSprite.Army.Player
        };
        string poisonPlantDamage = AbilityRules.TranslateResourceString(GetEngine(), S: s_poisonPlantDamageText);
        _ = log.ArmyMessages.Add(poisonPlantDamage);
        log.ArmyIcon = 1;
        log.ArmyView.Setup(armyHexSprite.Army, selection: 255);
        UnitCollection units = armyHexSprite.Army.Units;
        for (int i = 0, last = units.Count - 1; i <= last; i++)
        {
            Troop u = units.GetUnit(i);
            _ = u.AddRef();
            if (!u.GetImmunityTypes().Contains(DamageType.Poison) && !u.GetAbilityEnabled(1))
            {
                int damage = u.ExecuteDamageRole(4, damage: 3, damageTypes: Set<DamageType>.Of(DamageType.Poison));
                if (damage > u.GetHitPoints())
                {
                    damage = u.GetHitPoints();
                }

                if (damage != 0)
                {
                    u.SetHitPoints((sbyte)(u.GetHitPoints() - damage));
                    if (u.GetHitPoints() != 0)
                    {
                        _ = u.ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Poison));
                    }

                    count += 1;
                }

                _ = log.DamageValues.Add((byte)damage);
                _ = log.DamageTypes.Add((int)DamageType.Poison);
            }
            else
            {
                _ = log.DamageValues.Add(0);
                _ = log.DamageTypes.Add((int)DamageType.None);
            }

            _ = u.Release();
        }

        log.ValidateGameOver();
        if (count != 0)
        {
            EventLog.DistributeLocationEventLog(y: HexY, level: HexLevel, x: HexX, log: log, player: log.Player, execute: true);
        }

        armyHexSprite.Army.Validate();
        _ = log.Release();
    }

    // PoisonP.pas:168
    public void ArmyPlaced(ArmyHexSprite armyHexSprite)
    {
        if (PoisonPlantActive() && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            TriggerArmyDamage(armyHexSprite);
        }
    }

    // PoisonP.pas:174
    public void RestoreTerrain()
    {
        MapField mapField = Owner;
        sbyte originalOverlay = OriginalOverlay;
        Dispose();
        if (originalOverlay > 0)
        {
            StrategicMap.Current.PlaceTerrain(mapField.HexX, mapField.HexY, L: mapField.HexLevel, mapField.TerrainType, overlayType: originalOverlay);
        }
    }

    // PoisonP.pas:186
    public void NewTurn(sbyte player)
    {
        if (PoisonPlantActive())
        {
            ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
            if (armyHexSprite is not null && player == armyHexSprite.Army.Player)
            {
                TriggerArmyDamage(armyHexSprite);
            }
        }

        if (player == CasterPlayer && DaysRemaining > 0)
        {
            DaysRemaining = (sbyte)(DaysRemaining - 1);
            if (DaysRemaining == 0)
            {
                RestoreTerrain();
            }
        }
    }

    // PoisonP.pas:267
    public void ShowSummonAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        sbyte hexLevel = animation.HexLevel;
        sbyte hexY = animation.HexY;
        sbyte hexX = animation.HexX;
        MapField mapField = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == mapField.FindOwnedChild(ClassId) && GetEngine().AnimationDetail != 0 && animation.FrameCounter == 0)
        {
            bool condition = StrategicMap.Current.SeatedPlayer == animation.Tag;
            if (!condition)
            {
                sbyte hexLevel2 = HexLevel;
                sbyte hexY2 = HexY;
                sbyte hexX2 = HexX;
                condition = StrategicMap.Current.WatchingTerrain(hexX2, hexY2, L: hexLevel2, radius: 0);
            }

            if (condition)
            {
                _ = GamePack.Current.SpellControl[12].CastSounds.Play(0);
            }
        }
    }

    // PoisonP.pas:279
    public void NewFrameSummonAnimation(Animation animation, int frameCounter)
    {
        sbyte hexLevel = animation.HexLevel;
        sbyte hexY = animation.HexY;
        sbyte hexX = animation.HexX;
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(ClassId) && frameCounter == 9)
        {
            PlantStatus -= Set<PoisonPlantStatusElement>.Of(PoisonPlantStatusElement.Summoning);
        }
    }

    private static readonly string s_poisonPlantDamageText = "Poison Plant Damage";

    public new class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {

        // PoisonP.pas:293
        public new const int ClassId = 131_999;

        // PoisonP.pas:298
        public override int ResourceUserClassId() => PoisonPlant.ClassId;

        // PoisonP.pas:288
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_poisonPlantText);

        private static readonly string s_poisonPlantText = "Poison Plant";

    }
}
