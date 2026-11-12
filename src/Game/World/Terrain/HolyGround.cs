// Translated from HolyG.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
public partial class HolyGround : ImageLibraryTerrainMapObject
{
    public HolyGround(ECustomNode owner) : base(owner)
    { }

    // HolyG.pas:92
    public new const int ClassId = 131987;

    public Set<HolySummoning> Mode { get; set; }

    public sbyte Player;
    public sbyte Duration;
    public sbyte OriginalOverlay;

    // HolyG.pas:201
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.SByte(ref Player, -1, 40);
        stream.SByte(ref Duration, -1, 41);
        stream.SByte(ref OriginalOverlay, -1, 42);
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
            var hexY = GetHexY();
            var hexX = GetHexX();
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // HolyG.pas:193
    public override void RemoveFromMap()
    {
        MapLevel level;
        level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, _hexX, _hexY);
    }

    // HolyG.pas:219
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4418)
        {
            if (_resource.TerrainList.Terrain[message.HexNumber] >= 0 && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
                return;
            }

            return;
        }

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

    // HolyG.pas:209
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

    // HolyG.pas:181
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (_resource.TerrainList.Terrain[info.HexNumber] >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | 8192;
        }
    }

    // HolyG.pas:95
    public void TriggerArmyDamage(ArmyHexSprite army)
    {
        ArmyEventLog log;
        int damage;
        int damaged;
        Troop troop;
        damaged = 0;
        RandSeed = StrategicMap.Current.Random(16777215);
        log = new ArmyEventLog(null);
        log.ArmyText = AbilityRules.TranslateResourceString(s_holyGroundDamageText);
        var holyGroundDamage = AbilityRules.TranslateResourceString(s_holyGroundDamageText);
        log.ArmyMessages.Add(holyGroundDamage);
        log.ArmyIcon = 2;
        log._player = army.Army.Player;
        log.ArmyView.Setup(army.Army, 255);
        var armyLocal = army.Army;
        for (int I = 0, last = armyLocal.Count - 1; I <= last; I++)
        {
            troop = armyLocal.GetUnit(I);
            bool condition = troop.GetAlignment() != Alignment.None;
            if (condition)
            {
                var alignment = troop.GetAlignment();
                condition = AlignmentTables.PureEvilAlignments.Contains(alignment);
            }

            if (condition && !troop.GetAbilityEnabled(1))
            {
                damage = troop.ExecuteDamageRole(3, 4, Set<DamageType>.Of(DamageType.Holy));
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
                log.DamageTypes.Add((int)DamageType.Holy);
            }
            else
            {
                log.DamageValues.Add(0);
                log.DamageTypes.Add((int)DamageType.None);
            }
        }

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

    // HolyG.pas:154
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

    // HolyG.pas:166
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
            bool condition = !troop.GetImmunityTypes().Contains(DamageType.Holy) && troop.GetAlignment() != Alignment.None;
            if (condition)
            {
                var alignment = troop.GetAlignment();
                condition = AlignmentTables.PureEvilAlignments.Contains(alignment);
            }

            result = condition && !troop.GetAbilityEnabled(1);
        }

        return result;
    }

    // HolyG.pas:254
    public void ShowSummonAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        MapField field;
        var hexLevel = animation.GetHexLevel();
        var hexY = animation.GetHexY();
        var hexX = animation.GetHexX();
        field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(131987) && GameTables.GameEngine.AnimationDetail != 0 && animation.FrameCounter == 0)
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
                GamePack.Current.SpellControl.GetSpell(13).CastSounds.Play(0);
            }
        }
    }

    // HolyG.pas:265
    public void NewFrameSummonAnimation(Animation animation, int frameCounter)
    {
        var hexLevel = animation.GetHexLevel();
        var hexY = animation.GetHexY();
        var hexX = animation.GetHexX();
        var field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
        if (this == field.FindOwnedChild(131987) && frameCounter == 9)
        {
            Mode = Mode - Set<HolySummoning>.Of(HolySummoning.Summoning);
        }
    }

    private static readonly string s_holyGroundDamageText = "Holy Ground Damage";

    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // HolyG.pas:276
        public new const int ClassId = 131988;

        // HolyG.pas:279
        public override int ResourceUserClassId() => 131987;

        // HolyG.pas:273
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_holyGroundText);

        private static readonly string s_holyGroundText = "Holy Ground";

    }

}
