#nullable disable
// Translated from ReflectingPool.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// CenterFrame (-1 when inactive)
public partial class ReflectingPool(CustomNode owner) : PlayerStructure(owner)
{

    // ReflectingPool.pas:58
    public new const int ClassId = 132_670;

    private int _centerFrame = -1;

    // ReflectingPool.pas:80
    public override void PersistFields(IPersister persister) => base.PersistFields(persister);

    // ReflectingPool.pas:241
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId == 4362 && base.Resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
        {
            if (VisibilityRange() >= message.Param[0].Int && PoolVisionPlayer((sbyte)message.Param[1].Int))
            {
                message.Result = 1;
            }
        }
        else
        {
            if (message.MessageId == 4365 && base.Resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                if (VisibilityRange() >= message.Param[0].Int)
                {
                    NotifyPlayerChanged(message.MapField, distance: message.Param[0].Int);
                }
            }
            else
            {
                base.MapFieldMessageProc(ref message);
            }
        }
    }

    // ReflectingPool.pas:83
    public override void Activate()
    {
        base.Activate();
        if (Player != -1)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.AddChild(this);
        }
    }

    // ReflectingPool.pas:89
    public override void Deactivate()
    {
        base.Deactivate();
        DeactivatePoolCentering();
        if (Player != -1)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.RemoveChild(this);
        }
    }

    // ReflectingPool.pas:143
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (Player != -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.RemoveChild(this);
            }

            base.SetPlayer(player);
            if (Player != -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.AddChild(this);
            }

            UpdateReflectingPoolsExploration();
            if (Player != -1 && Player == StrategicMap.Current.SeatedPlayer && StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.Count == 1)
            {
                ActivatePoolCentering();
            }
        }
    }

    // ReflectingPool.pas:69
    public override int VisibilityRange()
    {
        int result = 8;
        return HexLevel != 0 || (Player > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments) & 8) != 0)
            ? (result + 1) >>> 1
            : result;
    }

    // ReflectingPool.pas:185
    public override void UpdateUnfog()
    {
        if (Status.Contains(MapObjectStatuses.PlacedOnMap))
        {
            int range = Status.Contains(MapObjectStatuses.Activated) && StrategicMap.Current.SeatedPlayer != -1
                ? PoolVisionPlayer(StrategicMap.Current.SeatedPlayer) ? VisibilityRange() : 0
                : 0;

            if (range != UnfogRange)
            {
                int x = default;
                int y = default;
                if (UnfogRange != 0)
                {
                    for (int i = 0, last = base.Resource.TerrainList.Count - 1; i <= last; i++)
                    {
                        if (base.Resource.TerrainList.GetTerrain(i) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: i, ref x, ref y);
                            _ = StrategicMap.Current.FogArea(x, y, Owner.HexLevel, range: UnfogRange);
                        }
                    }
                }

                UnfogRange = (byte)range;
                if (UnfogRange != 0)
                {
                    for (int i = 0, last = base.Resource.TerrainList.Count - 1; i <= last; i++)
                    {
                        if (base.Resource.TerrainList.GetTerrain(i) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: i, ref x, ref y);
                            _ = StrategicMap.Current.UnFogArea(x, y, Owner.HexLevel, range: UnfogRange);
                        }
                    }
                }
            }
        }
    }

    // ReflectingPool.pas:157
    public override void UpdateExploration()
    {
        if (Status.Contains(MapObjectStatuses.PlacedOnMap))
        {
            int x = default;
            int y = default;
            for (int p = 1, last = StrategicMap.Current.Players.Count - 1; p <= last; p++)
            {
                if (PoolVisionPlayer((sbyte)p))
                {
                    for (int i = 0, lastI = base.Resource.TerrainList.Count - 1; i <= lastI; i++)
                    {
                        if (base.Resource.TerrainList.GetTerrain(i) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: i, ref x, ref y);
                            int visibilityRange = VisibilityRange();
                            _ = StrategicMap.Current.ExploreArea(x, y, Owner.HexLevel, player: (sbyte)p, range: visibilityRange);
                        }
                    }
                }
            }
        }
    }

    // ReflectingPool.pas:77
    public override int DefaultAITargetPriority => 1;

    // ReflectingPool.pas:61
    public void ListReflectingPools(PlayerStructureList list)
    {
        for (int p = StrategicMap.Current.Players.Count - 1; p >= 0; p--)
        {
            for (int i = StrategicMap.Current.Players.GetPlayers(p).KnownStructures.Count - 1; i >= 0; i--)
            {
                PlayerStructure structure = StrategicMap.Current.Players.GetPlayers(p).KnownStructures.GetStructure(i);
                _ = list.AddChild(structure);
            }
        }
    }

    // ReflectingPool.pas:99
    public void NewFrame(object sender)
    {
        if (StrategicMap.Current.SeatedPlayer == Player)
        {
            _centerFrame += 1;
            if (_centerFrame % 8 == 0)
            {
                PlayerStructureList list = new(null) { Holder = CreationHolder() };
                ListReflectingPools(list);
                _ = list.RemoveChild(this);
                list.SortStable(ComparePools);
                int count = list.Count;
                if (_centerFrame / 8 < count)
                {
                    PlayerStructure pool = list.GetStructure(_centerFrame / 8);
                    sbyte hexLevel = pool.HexLevel;
                    sbyte hexY = pool.HexY;
                    sbyte hexX = pool.HexX;
                    StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
                }
                else
                {
                    sbyte hexLevel = HexLevel;
                    sbyte hexY = HexY;
                    sbyte hexX = HexX;
                    StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
                    DeactivatePoolCentering();
                }

                list?.Dispose();
            }
        }
        else
        {
            DeactivatePoolCentering();
        }
    }

    // ReflectingPool.pas:127
    public void ActivatePoolCentering()
    {
        DeactivatePoolCentering();
        _centerFrame = 0;
        StrategicMap.Current.NewFrameEvents += NewFrame;
    }

    // ReflectingPool.pas:134
    public void DeactivatePoolCentering()
    {
        if (_centerFrame != -1)
        {
            StrategicMap.Current.NewFrameEvents -= NewFrame;
            _centerFrame = -1;
        }
    }

    // ReflectingPool.pas:171
    public void UpdateReflectingPoolsExploration()
    {
        for (int p = StrategicMap.Current.Players.Count - 1; p >= 0; p--)
        {
            Player player = StrategicMap.Current.Players.GetPlayers(p);
            for (int i = player.KnownStructures.Count - 1; i >= 0; i--)
            {
                player.KnownStructures.GetStructure(i).UpdateExploration();
                player.KnownStructures.GetStructure(i).UpdateUnfog();
            }
        }
    }

    // ReflectingPool.pas:217
    public bool PoolVisionPlayer(sbyte player)
    {
        bool result = StrategicMap.Current.Players.GetPlayers(player).KnownStructures.Count != 0;
        if (!result)
        {
            int p = StrategicMap.Current.Players.Count;
            while (p > 1 && !result)
            {
                p -= 1;
                result = StrategicMap.Current.Players.GetPlayers(p).KnownStructures.Count != 0 && StrategicMap.Current.Players.GetPlayers(p).DiplomaticRelations.GetDiplomaticRelation(player) == DiplomaticRelation.Alliance;
            }
        }

        return result;
    }

    // ReflectingPool.pas:233
    public void NotifyPlayerChanged(MapField mapField, int distance)
    {
        for (int p = 1, last = StrategicMap.Current.Players.Count - 1; p <= last; p++)
        {
            if (PoolVisionPlayer((sbyte)p))
            {
                int trueVisionRange = TrueVisionRange();
                StrategicMap.Current.UpdatePlayerSeen(p, mapField.HexX, mapField.HexY, L: mapField.HexLevel, sightRadius: distance, radius: trueVisionRange);
            }
        }
    }

    internal static int ComparePools(object item1, object item2) => 0;

    public new class Resource : PlayerStructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = true;
            ConnectRoads = false;
            Claimed = false;
            Razeable = false;
        }

        // ReflectingPool.pas:266
        public new const int ClassId = 132_671;

        // ReflectingPool.pas:269
        public override int ResourceUserClassId() => ReflectingPool.ClassId;

        // ReflectingPool.pas:263
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_reflectingPoolText);

        private static readonly string s_reflectingPoolText = "Reflecting Pool";

    }
}
