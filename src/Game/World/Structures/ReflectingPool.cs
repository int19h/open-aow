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
    public ReflectingPool(ECustomNode owner) : base(owner)
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
        int X = default;
        int Y = default;
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
                    for (int I = 0, last = _resource.TerrainList.Count - 1; I <= last; I++)
                    {
                        if (_resource.TerrainList.GetTerrain(I) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, I, ref X, ref Y);
                            StrategicMap.Current.FogArea(X, Y, Owner.HexLevel, UnfogRange);
                        }
                    }
                }

                UnfogRange = (byte)range;
                if (UnfogRange != 0)
                {
                    for (int I = 0, last = _resource.TerrainList.Count - 1; I <= last; I++)
                    {
                        if (_resource.TerrainList.GetTerrain(I) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, I, ref X, ref Y);
                            StrategicMap.Current.UnFogArea(X, Y, Owner.HexLevel, UnfogRange);
                        }
                    }
                }
            }
        }
    }

    // ReflectingPool.pas:157
    public override void UpdateExploration()
    {
        int X = default;
        int Y = default;
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            for (int P = 1, last = StrategicMap.Current.Players.Count - 1; P <= last; P++)
            {
                if (PoolVisionPlayer((sbyte)P))
                {
                    for (int I = 0, lastI = _resource.TerrainList.Count - 1; I <= lastI; I++)
                    {
                        if (_resource.TerrainList.GetTerrain(I) >= 0)
                        {
                            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, I, ref X, ref Y);
                            var visibilityRange = VisibilityRange();
                            StrategicMap.Current.ExploreArea(X, Y, Owner.HexLevel, (sbyte)P, visibilityRange);
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
        for (int P = StrategicMap.Current.Players.Count - 1; P >= 0; P--)
        {
            for (int I = StrategicMap.Current.Players.GetPlayers(P).KnownStructures.Count - 1; I >= 0; I--)
            {
                var structure = StrategicMap.Current.Players.GetPlayers(P).KnownStructures.GetStructure(I);
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
        for (int P = StrategicMap.Current.Players.Count - 1; P >= 0; P--)
        {
            player = StrategicMap.Current.Players.GetPlayers(P);
            for (int I = player.KnownStructures.Count - 1; I >= 0; I--)
            {
                player.KnownStructures.GetStructure(I).UpdateExploration();
                player.KnownStructures.GetStructure(I).UpdateUnfog();
            }
        }
    }

    // ReflectingPool.pas:217
    public bool PoolVisionPlayer(sbyte player)
    {
        int P;
        bool result = StrategicMap.Current.Players.GetPlayers(player).KnownStructures.Count != 0;
        if (!result)
        {
            P = StrategicMap.Current.Players.Count;
            while (P > 1 && !result)
            {
                P -= 1;
                result = StrategicMap.Current.Players.GetPlayers(P).KnownStructures.Count != 0 && StrategicMap.Current.Players.GetPlayers(P).DiplomaticRelations.GetDiplomaticRelation(player) == DiplomaticRelation.Alliance;
            }
        }

        return result;
    }

    // ReflectingPool.pas:233
    public void NotifyPlayerChanged(MapField mapField, int distance)
    {
        for (int P = 1, last = StrategicMap.Current.Players.Count - 1; P <= last; P++)
        {
            if (PoolVisionPlayer((sbyte)P))
            {
                var trueVisionRange = TrueVisionRange();
                StrategicMap.Current.UpdatePlayerSeen(P, mapField.HexX, mapField.HexY, mapField.HexLevel, distance, trueVisionRange);
            }
        }
    }

    internal static int ComparePools(object item1, object item2)
    {
        return 0;
    }

    public new partial class Resource : PlayerStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
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
