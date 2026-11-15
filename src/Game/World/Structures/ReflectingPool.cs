// Translated from ReflectingPool.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// CenterFrame (-1 when inactive)
public partial class ReflectingPool : PlayerStructure
{
    public ReflectingPool(CustomNode owner) : base(owner)
    {
        _centerFrame = -1;
    }

    // ReflectingPool.pas:58
    public new const int ClassId = 132670;

    private int _centerFrame;

    // ReflectingPool.pas:80
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
    }

    // ReflectingPool.pas:52

    // ReflectingPool.pas:241
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId == 4362 && _resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
        {
            if (VisibilityRange() >= message.Param[0].Int && PoolVisionPlayer((sbyte)message.Param[1].Int))
            {
                message.Result = 1;
            }
        }
        else
        {
            if (message.MessageId == 4365 && _resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                if (VisibilityRange() >= message.Param[0].Int)
                {
                    NotifyPlayerChanged(message.MapField, message.Param[0].Int);
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
            StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.AddChild(this);
        }
    }

    // ReflectingPool.pas:89
    public override void Deactivate()
    {
        base.Deactivate();
        DeactivatePoolCentering();
        if (Player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.RemoveChild(this);
        }
    }

    // ReflectingPool.pas:143
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.RemoveChild(this);
            }

            base.SetPlayer(player);
            if (Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).KnownStructures.AddChild(this);
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
        if (GetHexLevel() != 0 || Player > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments.Image) & 8) != 0)
        {
            result = (int)(((uint)(result + 1)) >> 1);
        }

        return result;
    }

    // ReflectingPool.pas:185
    public override void UpdateUnfog()
    {
        int range;
        int x = default;
        int y = default;
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            if (Status.Contains(MapObjectStatusElement.Activated) && StrategicMap.Current.SeatedPlayer != -1)
            {
                if (PoolVisionPlayer(StrategicMap.Current.SeatedPlayer))
                {
                    range = VisibilityRange();
                }
                else
                {
                    range = 0;
                }
            }
            else
            {
                range = 0;
            }

            if (range != UnfogRange)
            {
                if (UnfogRange != 0)
                {
                    for (int i = 0, last = _resource.TerrainList.Count - 1; i <= last; i++)
                    {
                        if (_resource.TerrainList.GetTerrain(i) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, i, ref x, ref y);
                            StrategicMap.Current.FogArea(x, y, Owner.HexLevel, UnfogRange);
                        }
                    }
                }

                UnfogRange = (byte)range;
                if (UnfogRange != 0)
                {
                    for (int i = 0, last = _resource.TerrainList.Count - 1; i <= last; i++)
                    {
                        if (_resource.TerrainList.GetTerrain(i) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, i, ref x, ref y);
                            StrategicMap.Current.UnFogArea(x, y, Owner.HexLevel, UnfogRange);
                        }
                    }
                }
            }
        }
    }

    // ReflectingPool.pas:157
    public override void UpdateExploration()
    {
        int x = default;
        int y = default;
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            for (int p = 1, last = StrategicMap.Current.Players.Count - 1; p <= last; p++)
            {
                if (PoolVisionPlayer((sbyte)p))
                {
                    for (int i = 0, lastI = _resource.TerrainList.Count - 1; i <= lastI; i++)
                    {
                        if (_resource.TerrainList.GetTerrain(i) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, i, ref x, ref y);
                            var visibilityRange = VisibilityRange();
                            StrategicMap.Current.ExploreArea(x, y, Owner.HexLevel, (sbyte)p, visibilityRange);
                        }
                    }
                }
            }
        }
    }

    // ReflectingPool.pas:77
    public override int GetDefaultAITargetPriority() => 1;

    // ReflectingPool.pas:61
    public void ListReflectingPools(ref PlayerStructureList list)
    {
        for (int p = StrategicMap.Current.Players.Count - 1; p >= 0; p--)
        {
            for (int i = StrategicMap.Current.Players.GetPlayers(p).KnownStructures.Count - 1; i >= 0; i--)
            {
                var structure = StrategicMap.Current.Players.GetPlayers(p).KnownStructures.GetStructure(i);
                list.AddChild(structure);
            }
        }
    }

    // ReflectingPool.pas:99
    public void NewFrame(object sender)
    {
        PlayerStructureList list = default;
        PlayerStructure pool = default;
        if (StrategicMap.Current.SeatedPlayer == Player)
        {
            _centerFrame += 1;
            if (_centerFrame % 8 == 0)
            {
                list = new PlayerStructureList(null) { Holder = CreationHolder() };
                ListReflectingPools(ref list);
                list.RemoveChild(this);
                list.SortStable(ComparePools);
                var count = list.Count;
                if (_centerFrame / 8 < count)
                {
                    pool = list.GetStructure(_centerFrame / 8);
                    var hexLevel = pool.GetHexLevel();
                    var hexY = pool.GetHexY();
                    var hexX = pool.GetHexX();
                    StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
                }
                else
                {
                    var hexLevel = GetHexLevel();
                    var hexY = GetHexY();
                    var hexX = GetHexX();
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
        Player player;
        for (int p = StrategicMap.Current.Players.Count - 1; p >= 0; p--)
        {
            player = StrategicMap.Current.Players.GetPlayers(p);
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
        int p;
        bool result = StrategicMap.Current.Players.GetPlayers(player).KnownStructures.Count != 0;
        if (!result)
        {
            p = StrategicMap.Current.Players.Count;
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
                var trueVisionRange = TrueVisionRange();
                StrategicMap.Current.UpdatePlayerSeen(p, mapField.HexX, mapField.HexY, mapField.HexLevel, distance, trueVisionRange);
            }
        }
    }

    internal static int ComparePools(object item1, object item2)
    {
        return 0;
    }

    public new partial class Resource : PlayerStructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = true;
            ConnectRoads = false;
            Claimed = false;
            Razeable = false;
        }

        // ReflectingPool.pas:266
        public new const int ClassId = 132671;

        // ReflectingPool.pas:254

        // ReflectingPool.pas:269
        public override int ResourceUserClassId() => 132670;

        // ReflectingPool.pas:42

        // ReflectingPool.pas:263
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_reflectingPoolText);

        private static readonly string s_reflectingPoolText = "Reflecting Pool";

    }

    // ReflectingPool.pas:20

    // ReflectingPool.pas:20

}
