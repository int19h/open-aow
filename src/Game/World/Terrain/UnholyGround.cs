// Translated from UnHolyG.pas.
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

// Mode, Player, Duration, OriginalOverlay
public partial class UnholyGround : ImageLibraryTerrainMapObject
{
    public UnholyGround(ECustomNode owner) : base(owner)
    { }

    // UnHolyG.pas:93
    public new const int ClassId = 131985;

    public Set<UnholySummoning> Mode { get; set; }

    public sbyte Player { get; set; }
    public sbyte Duration { get; set; }
    public sbyte OriginalOverlay { get; set; }

    // UnHolyG.pas:204
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.SByte("Player", value: Player, fallback: -1);
        Duration = persister.SByte("Duration", value: Duration, fallback: -1);
        OriginalOverlay = persister.SByte("OriginalOverlay", value: OriginalOverlay, fallback: -1);
    }

    // UnHolyG.pas:243
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn((sbyte)message.Payload.Int);
        }
    }

    // UnHolyG.pas:249
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (!Mode.Contains(UnholySummoning.Summoning))
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
            var hexY = GetHexY();
            var hexX = GetHexX();
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // UnHolyG.pas:196
    public override void RemoveFromMap()
    {
        MapLevel level;
        level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, _hexX, _hexY);
    }

    // UnHolyG.pas:222
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4418)
        {
            if (_resource.TerrainList.Terrain[message.HexNumber] >= 0)
            {
                ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
                return;
            }
        }
        else
        {
            if (message.MessageId == 4384)
            {
                if (_resource.TerrainList.Terrain[message.HexNumber] >= 0)
                {
                    if (CanDoDamage(message.Param[0].As<ArmyHexSprite>().Army))
                    {
                        message.Result = 1;
                    }
                }
            }
            else
            {
                if (message.MessageId == 4402)
                {
                    if (_resource.TerrainList.Terrain[message.HexNumber] >= 0 && Player > 0 && Duration > 0)
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
        int result;
        if (_resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            if (!CanDoDamage(((MoveControl)info.Sender).Settings.Units))
            {
                result = 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            result = 1;
        }

        return result;
    }

    // UnHolyG.pas:184
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (_resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // UnHolyG.pas:96
    public void TriggerArmyDamage(ArmyHexSprite army)
    {
        ArmyEventLog log;
        int damage;
        int damaged;
        Troop troop;
        damaged = 0;
        RandSeed = StrategicMap.Current.Random(16777215);
        log = new ArmyEventLog(null) { Holder = CreationHolder() };
        log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_unholyGroundDamageText);
        var unholyGroundDamage = AbilityRules.TranslateResourceString(GetEngine(), s_unholyGroundDamageText);
        log.ArmyMessages.Add(unholyGroundDamage);
        log.ArmyIcon = 2;
        var armyLocal = army.Army;
        for (int I = 0, last = armyLocal.Count - 1; I <= last; I++)
        {
            troop = armyLocal.GetUnit(I);
            bool condition = troop.GetAlignment() != Alignment.None;
            if (condition)
            {
                var alignment = troop.GetAlignment();
                condition = AlignmentTables.PureGoodAlignments.Contains(alignment);
            }

            if (condition && !troop.GetAbilityEnabled(1))
            {
                damage = troop.ExecuteDamageRole(4, 3, Set<DamageType>.Of(DamageType.Death));
                if (damage > troop.GetHitPoints())
                {
                    damage = troop.GetHitPoints();
                }

                if (damage != 0)
                {
                    troop.SetHitPoints((sbyte)(troop.GetHitPoints() - damage));
                    damaged += 1;
                }

                log.DamageValues.Add((byte)damage);
                log.DamageTypes.Add((int)DamageType.Death);
            }
            else
            {
                log.DamageValues.Add(0);
                log.DamageTypes.Add((int)DamageType.Death);
            }
        }

        log._player = armyLocal.Player;
        var armyHexSprite = armyLocal.GetArmyHexSprite();
        log.ArmyView.Setup(armyHexSprite.Army, 255);

        log.ValidateGameOver();
        if (damaged != 0)
        {
            var hexY = GetHexY();
            var hexLevel = GetHexLevel();
            var hexX = GetHexX();
            EventLog.DistributeLocationEventLog(log, log._player, hexX, hexY, hexLevel, true);
        }

        log.Release();
        army.Army.Validate();
    }

    // UnHolyG.pas:142
    public void ArmyPlaced(ArmyHexSprite army)
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            TriggerArmyDamage(army);
        }
    }

    // UnHolyG.pas:147
    public void RestoreTerrain()
    {
        MapField field;
        sbyte overlay;
        field = Owner;
        overlay = OriginalOverlay;
        Dispose();
        if (overlay > 0)
        {
            StrategicMap.Current.PlaceTerrain(field.HexX, field.HexY, field.HexLevel, field.TerrainType, overlay);
        }
    }

    // UnHolyG.pas:157
    public void NewTurn(sbyte player)
    {
        ArmyHexSprite army;
        army = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        if (army != null && player == army.Army.Player)
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

    // UnHolyG.pas:169
    public bool CanDoDamage(UnitList units)
    {
        int I;
        Troop troop;
        bool result = false;
        I = units.Count;
        while (!result && I != 0)
        {
            I -= 1;
            troop = units.GetUnit(I);
            bool condition = !troop.GetImmunityTypes().Contains(DamageType.Death) && troop.GetAlignment() != Alignment.None;
            if (condition)
            {
                var alignment = troop.GetAlignment();
                condition = AlignmentTables.PureGoodAlignments.Contains(alignment);
            }

            result = condition && !troop.GetAbilityEnabled(1);
        }

        return result;
    }

    // UnHolyG.pas:254
    public void ShowSummonAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        MapField field;
        var hexLevel = animation.GetHexLevel();
        var hexY = animation.GetHexY();
        var hexX = animation.GetHexX();
        field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(131985) && GetEngine().AnimationDetail != 0 && animation.FrameCounter == 0)
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
                GamePack.Current.SpellControl.GetSpell(14).CastSounds.Play(0);
            }
        }
    }

    // UnHolyG.pas:265
    public void NewFrameSummonAnimation(Animation animation, int frameCounter)
    {
        var hexLevel = animation.GetHexLevel();
        var hexY = animation.GetHexY();
        var hexX = animation.GetHexX();
        var field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(131985) && frameCounter == 9)
        {
            Mode = Mode - Set<UnholySummoning>.Of(UnholySummoning.Summoning);
        }
    }

    private static readonly string s_unholyGroundDamageText = "Unholy Ground Damage";

    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // UnHolyG.pas:278
        public new const int ClassId = 131986;

        // UnHolyG.pas:283
        public override int ResourceUserClassId() => 131985;

        // UnHolyG.pas:273
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_unholyGroundText);

        private static readonly string s_unholyGroundText = "Unholy Ground";

    }

}
