#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// SpherePicksMax. Spheres
public class Leader(CustomNode? owner) : Hero(owner)
{

    public new const int ClassId = 131_634;

    public int SpherePicksMax { get; private set; } = 5;

    private readonly ByteList _spheres = [];

    // Hero.PAS:2766
    public override void Dispose()
    {
        // The original tested the global map. This tests the map of the engine the leader reaches, and a
        // leader that reaches no engine takes the original's branch for no map.
        StrategicMap? strategicMap = GetEngine()?.StrategicMap;
        if (strategicMap is not null && strategicMap.Players.GetPlayers(Player) is not null && this == strategicMap.Players.GetPlayers(Player).Leader)
        {
            strategicMap.Players.GetPlayers(Player).SetLeader(null);
        }

        base.Dispose();
    }

    // Hero.PAS:2891
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SpherePicksMax = persister.Byte("SpherePicksMax", value: SpherePicksMax.TruncateToByte(), fallback: 5);
        persister.ByteList(name: "Spheres", list: _spheres);
    }

    // Hero.PAS:2845
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001989)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            SetSpherePicksMax(strategicMap.SphereCount.TruncateToByte());
            if (strategicMap.Players.GetCount() > PlayerIdExtensions.ToOriginal(Player))
            {
                // { reload FPlayer }
                if (strategicMap.Players.GetPlayers((sbyte)(byte)PlayerIdExtensions.ToOriginal(Player)).PlayerRace != Race)
                {
                    Player? players = strategicMap.Players.GetPlayers(Player);
                    SetHeroResourceIndex(GetEngine().GamePack.HeroResources.Find(players.PlayerRace, heroType: HeroType.HeroLeader));
                    Changed();
                }
            }
            else
            {
                Dispose();
            }
        }
    }

    // Hero.PAS:2774
    public override void Changed()
    {
        base.Changed();
        StrategicMap? strategicMap = GetEngine()?.StrategicMap;
        if (strategicMap is not null && !strategicMap.Status.Contains(HexSpriteMapStatuses.Destroying))
        {
            strategicMap.OnLeaderChanged(this);
        }
    }

    // Hero.PAS:2975
    public override int GetUpkeep() => 0;

    // Hero.PAS:2886
    public override int GetPowerGeneration() => GetAbilityEnabled(52) ? (GetAbilityLevel(52) * 5) + 5 : 0;

    // Hero.PAS:2908
    public override void SetPlayer(PlayerId? player)
    {
        if (player != Player)
        {
            if (Player != PlayerId.Independent)
            {
                SetOwner(null);
                Changed();
            }
            else
            {
                base.SetPlayer(player);
            }
        }
    }

    // Hero.PAS:2784
    public override int GetUnitMoraleValue(PlayerId? player, int independentRelation) => 90;

    // Hero.PAS:2921
    public override bool CanActivate()
    {
        bool result = base.CanActivate();
        if (result)
        {
            Leader? leader = GetEngine().StrategicMap.Players.GetPlayers(Player).Leader;
            if (leader is not null && !CompatibleHero(leader))
            {
                return false;
            }
        }

        return result;
    }

    // Hero.PAS:2932
    public override void Activate()
    {
        LibraryId = -1;
        if (Player == PlayerId.Independent)
        {
            Player = PlayerId.FromOriginal((sbyte)GetEngine().StrategicMap.Players.RaceToPlayer(Race));
        }

        base.Activate();
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Players.GetPlayers(Player) is not null)
        {
            Leader? leader = strategicMap.Players.GetPlayers(Player).Leader;
            if (leader != this)
            {
                if (leader is not null && leader.Owner is not null)
                {
                    try
                    {
                        throw new InvalidOperationException("Double leader used");
                    }
                    catch (Exception error)
                    {
                        HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
                    }
                }

                // { Original silently swallows this duplicate-leader exception. }
                GetEngine().StrategicMap.Players.GetPlayers(Player).SetLeader(this);
                Changed();
            }
            else
            {
                Changed();
            }
        }
        else
        {
            SetOwner(null);
            _ = Release();
        }
    }

    // Hero.PAS:2957
    public override void Deactivate() => base.Deactivate();

    // Hero.PAS:2972
    public override bool CanDisband(string reason) => false;

    // Hero.PAS:2915
    public override void Killed()
    {
        if (UnitStatus.Contains(UnitStatuses.Activated) && GetEngine().StrategicMap.Combat is null)
        {
            GetEngine().StrategicMap.GameOver(Player, status: GameOverStatus.Lost);
        }

        base.Killed();
    }

    // Hero.PAS:2787
    public override string GetNickname()
    {
        StrategicMap? strategicMap = GetEngine()?.StrategicMap;
        return strategicMap is not null ? strategicMap.GameType != GameType.Campaign ? "" : Nickname : "";
    }

    // Hero.PAS:2795
    public override void SetNickname(string value)
    {
        StrategicMap? strategicMap = GetEngine()?.StrategicMap;
        if (strategicMap is not null && strategicMap.GameType == GameType.Campaign)
        {
            base.SetNickname(value);
        }
    }

    // Hero.PAS:2901
    public override void SetHeroResourceIndex(int value)
    {
        base.SetHeroResourceIndex(value);
        if (HeroResource is not null && GetEngine()?.StrategicMap is not null && GetEngine().StrategicMap.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            HeroName = HeroResource.Name;
        }
    }

    // Hero.PAS:2960
    public override void EditDropMessage(ref EngineMessage message)
    {
        MapLevel? level = message.Param[0].As<MapLevel>();
        int l = message.Data[1];
        HexGeometry.PixelToHex(message.Payload.Int, vertical: message.Data[0], out int x, out int y);
        bool placed = x >= 0 && y >= 0 && x < level.Width && y < level.Height && Place(x, y, l, Player, mode: UnitPlaceModes.None) is not null;

        message.Result = placed ? 0 : -1;
    }

    // Hero.PAS:2800
    public int GetSpherePicks(MagicSphere sphere)
    {
        int result;
        if (sphere != MagicSphere.Cosmos)
        {
            result = 0;
            for (int i = _spheres.Count - 1; i >= 0; i--)
            {
                if (_spheres.List[i] == (byte)sphere)
                {
                    result = (result + 1).TruncateToByte();
                }
            }
        }
        else
        {
            return 4;
        }

        return result;
    }

    // Hero.PAS:2812
    public MagicSphere GetSpheres(int index) => (MagicSphere)_spheres.List[index];

    // Hero.PAS:2815
    public int SphereCount => _spheres.Count;

    // Hero.PAS:2818
    public void SetSpherePicksMax(int value)
    {
        SpherePicksMax = value;
        while (_spheres.Count > SpherePicksMax)
        {
            _spheres.Delete(_spheres.Count - 1);
        }
    }

    // Hero.PAS:2824
    public bool CanAddSphere(MagicSphere sphere) => _spheres.Count < SpherePicksMax && sphere != MagicSphere.Cosmos && (!GetEngine().Demo || GetSpherePicks(sphere) < 2) && GetSpherePicks(sphere) != 4 && (sphere != MagicSphere.Fire || GetSpherePicks(MagicSphere.Water) == 0) && (sphere != MagicSphere.Water || GetSpherePicks(MagicSphere.Fire) == 0) && (sphere != MagicSphere.Life || GetSpherePicks(MagicSphere.Death) == 0) && (sphere != MagicSphere.Death || GetSpherePicks(MagicSphere.Life) == 0) && (sphere != MagicSphere.Earth || GetSpherePicks(MagicSphere.Air) == 0) && (sphere != MagicSphere.Air || GetSpherePicks(MagicSphere.Earth) == 0);

    // Hero.PAS:2842
    public bool AddSphere(MagicSphere sphere)
    {
        bool result = CanAddSphere(sphere);
        if (result)
        {
            _ = _spheres.Add((byte)sphere);
        }

        return result;
    }

    // Hero.PAS:2863
    public bool DeleteSphereIndex(int index)
    {
        if (index >= 0 && index < _spheres.Count)
        {
            _spheres.Delete(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Hero.PAS:2870
    public bool DeleteSphere(MagicSphere sphere)
    {
        int i = _spheres.FindLastIndex(sphere, match: static (sought, stored) => stored == (byte)sought);
        if (i >= 0)
        {
            _spheres.Delete(i);
            return true;
        }

        return false;
    }

    // Hero.PAS:2883
    public void ClearSpheres() => _spheres.Clear();

}
