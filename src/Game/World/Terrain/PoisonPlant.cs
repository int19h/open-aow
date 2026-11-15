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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// summoning status. caster player. remaining turns. original overlay
public partial class PoisonPlant : ImageLibraryTerrainMapObject
{
    public PoisonPlant(CustomNode owner) : base(owner)
    {
        CasterPlayer = -1;
        OriginalOverlay = -1;
        DaysRemaining = -1;
        PlantStatus = Set<PoisonPlantStatusElement>.Of();
    }

    // PoisonP.pas:87
    public new const int ClassId = 131998;

    public Set<PoisonPlantStatusElement> PlantStatus { get; set; }

    public sbyte CasterPlayer { get; set; }
    public sbyte DaysRemaining { get; set; }
    public sbyte OriginalOverlay { get; set; }

    // PoisonP.pas:97
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        CasterPlayer = persister.SByte("CasterPlayer", value: CasterPlayer, fallback: -1);
        DaysRemaining = persister.SByte("DaysRemaining", value: DaysRemaining, fallback: -1);
        OriginalOverlay = persister.SByte("OriginalOverlay", value: OriginalOverlay, fallback: -1);
    }

    // PoisonP.pas:78

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
            var hexY = GetHexY();
            var hexX = GetHexX();
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // PoisonP.pas:246
    public override void RemoveFromMap()
    {
        MapLevel level;
        level = Owner.Owner;
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
        int result;
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber) >= 0 && PoisonPlantActive())
        {
            if (!CanDoDamage(((MoveControl)info.Sender).Settings.Units))
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            result = 1;
        }

        return result;
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
        int i;
        Troop u;
        bool result = false;
        i = units.Count;
        while (!result && i != 0)
        {
            i -= 1;
            u = units.GetUnit(i);
            result = !u.GetImmunityTypes().Contains(DamageType.Poison) && !u.GetAbilityEnabled(1);
        }

        return result;
    }

    // PoisonP.pas:120
    public void TriggerArmyDamage(ArmyHexSprite armyHexSprite)
    {
        int count;
        UnitCollection units;
        ArmyEventLog log;
        Troop u;
        int damage;
        count = 0;
        RandSeed = StrategicMap.Current.Random(16777215);
        log = new ArmyEventLog(null) { Holder = CreationHolder() };
        log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_poisonPlantDamageText);
        log.Player = armyHexSprite.Army.Player;
        var poisonPlantDamage = AbilityRules.TranslateResourceString(GetEngine(), s_poisonPlantDamageText);
        log.ArmyMessages.Add(poisonPlantDamage);
        log.ArmyIcon = 1;
        log.ArmyView.Setup(armyHexSprite.Army, 255);
        units = armyHexSprite.Army.Units;
        for (int i = 0, last = units.Count - 1; i <= last; i++)
        {
            u = units.GetUnit(i);
            u.AddRef();
            if (!u.GetImmunityTypes().Contains(DamageType.Poison) && !u.GetAbilityEnabled(1))
            {
                damage = u.ExecuteDamageRole(4, 3, Set<DamageType>.Of(DamageType.Poison));
                if (damage > u.GetHitPoints())
                {
                    damage = u.GetHitPoints();
                }

                if (damage != 0)
                {
                    u.SetHitPoints((sbyte)(u.GetHitPoints() - damage));
                    if (u.GetHitPoints() != 0)
                    {
                        u.ExecuteDamageEffects(Set<DamageType>.Of(DamageType.Poison));
                    }

                    count += 1;
                }

                log.DamageValues.Add((byte)damage);
                log.DamageTypes.Add((int)DamageType.Poison);
            }
            else
            {
                log.DamageValues.Add(0);
                log.DamageTypes.Add((int)DamageType.None);
            }

            u.Release();
        }

        log.ValidateGameOver();
        if (count != 0)
        {
            EventLog.DistributeLocationEventLog(y: GetHexY(), level: GetHexLevel(), x: GetHexX(), log: log, player: log.Player, execute: true);
        }

        armyHexSprite.Army.Validate();
        log.Release();
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
        MapField mapField;
        sbyte originalOverlay;
        mapField = Owner;
        originalOverlay = OriginalOverlay;
        Dispose();
        if (originalOverlay > 0)
        {
            StrategicMap.Current.PlaceTerrain(mapField.HexX, mapField.HexY, mapField.HexLevel, mapField.TerrainType, originalOverlay);
        }
    }

    // PoisonP.pas:186
    public void NewTurn(sbyte player)
    {
        ArmyHexSprite armyHexSprite;
        if (PoisonPlantActive())
        {
            armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            if (armyHexSprite != null && player == armyHexSprite.Army.Player)
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
        MapField mapField;
        var hexLevel = animation.GetHexLevel();
        var hexY = animation.GetHexY();
        var hexX = animation.GetHexX();
        mapField = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == mapField.FindOwnedChild(131998) && GetEngine().AnimationDetail != 0 && animation.FrameCounter == 0)
        {
            bool condition = StrategicMap.Current.SeatedPlayer == animation.Tag;
            if (!condition)
            {
                var hexLevel2 = GetHexLevel();
                var hexY2 = GetHexY();
                var hexX2 = GetHexX();
                condition = StrategicMap.Current.WatchingTerrain(hexX2, hexY2, hexLevel2, 0);
            }

            if (condition)
            {
                GamePack.Current.SpellControl.GetSpell(12).CastSounds.Play(0);
            }
        }
    }

    // PoisonP.pas:279
    public void NewFrameSummonAnimation(Animation animation, int frameCounter)
    {
        var hexLevel = animation.GetHexLevel();
        var hexY = animation.GetHexY();
        var hexX = animation.GetHexX();
        var field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(131998) && frameCounter == 9)
        {
            PlantStatus = PlantStatus - Set<PoisonPlantStatusElement>.Of(PoisonPlantStatusElement.Summoning);
        }
    }

    private static readonly string s_poisonPlantDamageText = "Poison Plant Damage";

    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(CustomNode owner) : base(owner)
        { }

        // PoisonP.pas:293
        public new const int ClassId = 131999;

        // PoisonP.pas:298
        public override int ResourceUserClassId() => 131998;

        // PoisonP.pas:288
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_poisonPlantText);

        private static readonly string s_poisonPlantText = "Poison Plant";

    }

    // PoisonP.pas:28

    // PoisonP.pas:28

}
