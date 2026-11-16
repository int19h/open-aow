#nullable disable
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
public partial class Leader : Hero
{
    public new const int ClassId = 131634;

    public byte SpherePicksMax { get; private set; }

    private readonly ByteList _spheres;

    // Hero.PAS:2766
    public override void Dispose()
    {
        if (StrategicMap.Current != null && StrategicMap.Current.Players.GetPlayers(Player) != null && this == StrategicMap.Current.Players.GetPlayers(Player).Leader)
        {
            StrategicMap.Current.Players.GetPlayers(Player).SetLeader(null);
        }

        base.Dispose();
        _spheres?.Dispose();
    }

    // Hero.PAS:2891
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SpherePicksMax = persister.Byte("SpherePicksMax", value: SpherePicksMax, fallback: 5);
        PersistSupport.ByteList(persister, name: "Spheres", list: _spheres);
    }

    // Hero.PAS:2759
    public Leader(CustomNode owner) : base(owner)
    {
        _spheres = new ByteList();
        SpherePicksMax = 5;
    }

    // Hero.PAS:2845
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001989)
        {
            SetSpherePicksMax((byte)StrategicMap.Current.SphereCount);
            if (StrategicMap.Current.Players.GetCount() > Player)
            {
                // { reload FPlayer }
                if (StrategicMap.Current.Players.GetPlayers((sbyte)(byte)Player).PlayerRace != Race)
                {
                    Player players = StrategicMap.Current.Players.GetPlayers(Player);
                    SetHeroResourceIndex(GamePack.Current.HeroResources.Find(players.PlayerRace, HeroType.HeroLeader));
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
        if (StrategicMap.Current != null && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            StrategicMap.Current.OnLeaderChangedStore.Raise(this);
        }
    }

    // Hero.PAS:2975
    public override byte GetUpkeep() => 0;

    // Hero.PAS:2886
    public override int GetPowerGeneration()
    {
        int result = GetAbilityEnabled(52) ? (GetAbilityLevel(52) * 5) + 5 : 0;
        return result;
    }

    // Hero.PAS:2908
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (Player != 0)
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
    public override int GetUnitMoraleValue(sbyte player, sbyte independentRelation) => 90;

    // Hero.PAS:2921
    public override bool CanActivate()
    {
        Leader leader;
        bool result = base.CanActivate();
        if (result)
        {
            leader = StrategicMap.Current.Players.GetPlayers(Player).Leader;
            if (leader != null && !CompatibleHero(leader))
            {
                result = false;
            }
        }

        return result;
    }

    // Hero.PAS:2932
    public override void Activate()
    {
        Leader leader;
        LibraryId = -1;
        if (Player == 0)
        {
            Player = (sbyte)StrategicMap.Current.Players.RaceToPlayer(Race);
        }

        base.Activate();
        if (StrategicMap.Current.Players.GetPlayers(Player) != null)
        {
            leader = StrategicMap.Current.Players.GetPlayers(Player).Leader;
            if (leader != this)
            {
                if (leader != null && leader.Owner != null)
                {
                    try
                    {
                        throw new InvalidOperationException("Double leader used");
                    }
                    catch
                    {
                    }
                }

                // { Original silently swallows this duplicate-leader exception. }
                StrategicMap.Current.Players.GetPlayers(Player).SetLeader(this);
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
            Release();
        }
    }

    // Hero.PAS:2957
    public override void Deactivate() => base.Deactivate();

    // Hero.PAS:2972
    public override bool CanDisband(ref string reason) => false;

    // Hero.PAS:2915
    public override void Killed()
    {
        if (UnitStatus.Contains(UnitStatusElement.Activated) && StrategicMap.Current.Combat == null)
        {
            StrategicMap.Current.GameOver(Player, GameOverStatus.Lost);
        }

        base.Killed();
    }

    // Hero.PAS:2787
    public override string GetNickname()
    {
        string result;
        if (StrategicMap.Current != null)
        {
            if (StrategicMap.Current.GameType != GameType.Campaign)
            {
                result = "";
            }
            else
            {
                return Nickname;
            }
        }
        else
        {
            result = "";
        }

        return result;
    }

    // Hero.PAS:2795
    public override void SetNickname(string value)
    {
        if (StrategicMap.Current != null && StrategicMap.Current.GameType == GameType.Campaign)
        {
            base.SetNickname(value);
        }
    }

    // Hero.PAS:2901
    public override void SetHeroResourceIndex(int value)
    {
        base.SetHeroResourceIndex(value);
        if (HeroResource != null && StrategicMap.Current != null && StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HeroName = HeroResource.Name;
        }
    }

    // Hero.PAS:2960
    public override void EditDropMessage(ref EngineMessage message)
    {
        MapLevel level;
        int l;
        bool placed;
        level = message.Param[0].As<MapLevel>();
        l = message.Data[1];
        HexGeometry.PixelToHex(message.Payload.Int, message.Data[0], out int x, out int y);
        placed = x >= 0 && y >= 0 && x < level.Width && y < level.Height && Place(x, y, l, Player, Set<UnitPlaceModeElement>.Of()) != null;

        message.Result = placed ? 0 : -1;
    }

    // Hero.PAS:2800
    public byte GetSpherePicks(MagicSphere sphere)
    {
        byte result;
        if (sphere != MagicSphere.Cosmos)
        {
            result = 0;
            for (int i = _spheres.Count - 1; i >= 0; i--)
            {
                if (_spheres.List[i] == (byte)sphere)
                {
                    result = (byte)(result + 1);
                }
            }
        }
        else
        {
            result = 4;
        }

        return result;
    }

    // Hero.PAS:2812
    public MagicSphere GetSpheres(int index) => (MagicSphere)_spheres.List[index];

    // Hero.PAS:2815
    public int GetSphereCount() => _spheres.Count;

    // Hero.PAS:2818
    public void SetSpherePicksMax(byte value)
    {
        SpherePicksMax = value;
        while (_spheres.Count > SpherePicksMax)
        {
            _spheres.Delete(_spheres.Count - 1);
        }
    }

    // Hero.PAS:2824
    public bool CanAddSphere(MagicSphere sphere)
    {
        bool result = _spheres.Count < SpherePicksMax && sphere != MagicSphere.Cosmos && (!GetEngine().Demo || GetSpherePicks(sphere) < 2) && GetSpherePicks(sphere) != 4 && (sphere != MagicSphere.Fire || GetSpherePicks(MagicSphere.Water) == 0) && (sphere != MagicSphere.Water || GetSpherePicks(MagicSphere.Fire) == 0) && (sphere != MagicSphere.Life || GetSpherePicks(MagicSphere.Death) == 0) && (sphere != MagicSphere.Death || GetSpherePicks(MagicSphere.Life) == 0) && (sphere != MagicSphere.Earth || GetSpherePicks(MagicSphere.Air) == 0) && (sphere != MagicSphere.Air || GetSpherePicks(MagicSphere.Earth) == 0);
        return result;
    }

    // Hero.PAS:2842
    public bool AddSphere(MagicSphere sphere)
    {
        bool result = CanAddSphere(sphere);
        if (result)
        {
            _spheres.Add((byte)sphere);
        }

        return result;
    }

    // Hero.PAS:2863
    public bool DeleteSphereIndex(int index)
    {
        bool result;
        if (index >= 0 && index < _spheres.Count)
        {
            _spheres.Delete(index);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Hero.PAS:2870
    public bool DeleteSphere(MagicSphere sphere)
    {
        int i;
        i = _spheres.Count;
        bool result = false;
        while (!result && i != 0)
        {
            i -= 1;
            result = _spheres[i] == (byte)sphere;
        }

        if (result)
        {
            _spheres.Delete(i);
        }

        return result;
    }

    // Hero.PAS:2883
    public void ClearSpheres() => _spheres.Clear();

}
