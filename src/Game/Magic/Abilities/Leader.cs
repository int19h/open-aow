// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// FSpherePicksMax. FSpheres
public partial class Leader : Hero
{
    public new const int ClassID = 131634;

    public byte FSpherePicksMax;
    public ByteList FSpheres;

    // Hero.PAS:2766
    public override void Dispose()
    {
        if (AoWHSMap.Current != null && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer) != null && this == AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FLeader)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).SetLeader(null);
        }

        base.Dispose();
        FSpheres?.Dispose();
    }

    // Hero.PAS:2898
    public override GameEngine GetEngine() => GameTables.AoWEngine;

    // Hero.PAS:2891
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByteEx(ref FSpherePicksMax, 5, 45);
        Stream.rwStreamProc(46, FSpheres.LoadFromStream, FSpheres.SaveToStream);
    }

    // Hero.PAS:2759
    public Leader(ECustomNode owner) : base(owner)
    {
        FSpheres = new ByteList();
        FSpherePicksMax = 5;
    }

    // Hero.PAS:2845
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001989)
        {
            SetSpherePicksMax((byte)AoWHSMap.Current.FSphereCount);
            if (AoWHSMap.Current.FPlayers.GetCount() > FPlayer)
            {
                // { reload FPlayer }
                if (AoWHSMap.Current.FPlayers.GetPlayers((sbyte)(byte)this.FPlayer).FPlayerRace != FRace)
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                    SetHeroResourceIndex(AoWHSSet.Current.FHeroResources.Find(players.FPlayerRace, HeroType.HeroLeader));
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
        if (AoWHSMap.Current != null && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            AoWHSMap.Current.FOnLeaderChangedStore.Raise(this);
        }
    }

    // Hero.PAS:2975
    public override byte GetUpkeep() => 0;

    // Hero.PAS:2886
    public override int GetPowerGeneration()
    {
        int Result;
        if (GetAbilityEnabled(52))
        {
            Result = GetAbilityLevel(52) * 5 + 5;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Hero.PAS:2908
    public override void SetPlayer(sbyte Player)
    {
        if (Player != FPlayer)
        {
            if (FPlayer != 0)
            {
                SetOwner(null);
                Changed();
            }
            else
            {
                base.SetPlayer(Player);
            }
        }
    }

    // Hero.PAS:2784
    public override int GetUnitMoraleValue(sbyte Player, sbyte IndependentRelation) => 90;

    // Hero.PAS:2921
    public override bool CanActivate()
    {
        Leader Leader;
        bool Result = base.CanActivate();
        if (Result)
        {
            Leader = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FLeader;
            if (Leader != null && !CompatibleHero(Leader))
            {
                Result = false;
            }
        }

        return Result;
    }

    // Hero.PAS:2932
    public override void Activate()
    {
        Leader Leader;
        FLibraryID = -1;
        if (FPlayer == 0)
        {
            FPlayer = (sbyte)AoWHSMap.Current.FPlayers.RaceToPlayer(FRace);
        }

        base.Activate();
        if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer) != null)
        {
            Leader = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FLeader;
            if (Leader != this)
            {
                if (Leader != null && Leader.Owner != null)
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
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).SetLeader(this);
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
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // Hero.PAS:2972
    public override bool CanDisband(ref string Reason) => false;

    // Hero.PAS:2915
    public override void Killed()
    {
        if (FUnitStatus.Contains(UnitStatusElement.Activated) && AoWHSMap.Current.FCombat == null)
        {
            AoWHSMap.Current.GameOver(FPlayer, GameOverStatus.Lost);
        }

        base.Killed();
    }

    // Hero.PAS:2787
    public override string GetNickname()
    {
        string Result;
        if (AoWHSMap.Current != null)
        {
            if (AoWHSMap.Current.FGameType != GameType.Campaign)
            {
                Result = "";
            }
            else
            {
                return FNickname;
            }
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    // Hero.PAS:2795
    public override void SetNickname(string Value)
    {
        if (AoWHSMap.Current != null && AoWHSMap.Current.FGameType == GameType.Campaign)
        {
            base.SetNickname(Value);
        }
    }

    // Hero.PAS:2901
    public override void SetHeroResourceIndex(int Value)
    {
        base.SetHeroResourceIndex(Value);
        if (FHeroResource != null && AoWHSMap.Current != null && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            FHeroName = FHeroResource.FName;
        }
    }

    // Hero.PAS:2960
    public override void EditDropMsg(ref EngineMessage Msg)
    {
        MapLevel Level;
        int L;
        int X = default;
        int Y = default;
        bool Placed;
        Level = Msg.Param[0].As<MapLevel>();
        L = Msg.Data[1];
        HexGeometry.HPtoHX(Msg.Payload.Int, Msg.Data[0], ref X, ref Y);
        if (X >= 0 && Y >= 0 && X < Level.Width && Y < Level.Height)
        {
            Placed = Place(X, Y, L, FPlayer, Set<UnitPlaceModeElement>.Of()) != null;
        }
        else
        {
            Placed = false;
        }

        if (Placed)
        {
            Msg.Result = 0;
        }
        else
        {
            Msg.Result = -1;
        }
    }

    // Hero.PAS:2800
    public byte GetSpherePicks(MagicSphere Sphere)
    {
        byte Result;
        if (Sphere != MagicSphere.Cosmos)
        {
            Result = 0;
            for (int I = FSpheres.Count - 1; I >= 0; I--)
            {
                if (FSpheres.List[I] == (byte)Sphere)
                {
                    Result = (byte)(Result + 1);
                }
            }
        }
        else
        {
            Result = 4;
        }

        return Result;
    }

    // Hero.PAS:2812
    public MagicSphere GetSpheres(int Index) => (MagicSphere)FSpheres.List[Index];

    // Hero.PAS:2815
    public int GetSphereCount() => FSpheres.Count;

    // Hero.PAS:2818
    public void SetSpherePicksMax(byte Value)
    {
        FSpherePicksMax = Value;
        while (FSpheres.Count > FSpherePicksMax)
        {
            FSpheres.Delete(FSpheres.Count - 1);
        }
    }

    // Hero.PAS:2824
    public bool CanAddSphere(MagicSphere Sphere)
    {
        bool Result;
        if (FSpheres.Count < FSpherePicksMax)
        {
            if (Sphere == MagicSphere.Cosmos)
            {
                Result = false;
            }
            else
            {
                if (GameTables.AoWEngine.FDemo && GetSpherePicks(Sphere) >= 2)
                {
                    Result = false;
                }
                else
                {
                    if (GetSpherePicks(Sphere) == 4)
                    {
                        Result = false;
                    }
                    else
                    {
                        if (Sphere == MagicSphere.Fire && GetSpherePicks(MagicSphere.Water) != 0)
                        {
                            Result = false;
                        }
                        else
                        {
                            if (Sphere == MagicSphere.Water && GetSpherePicks(MagicSphere.Fire) != 0)
                            {
                                Result = false;
                            }
                            else
                            {
                                if (Sphere == MagicSphere.Life && GetSpherePicks(MagicSphere.Death) != 0)
                                {
                                    Result = false;
                                }
                                else
                                {
                                    if (Sphere == MagicSphere.Death && GetSpherePicks(MagicSphere.Life) != 0)
                                    {
                                        Result = false;
                                    }
                                    else
                                    {
                                        if (Sphere == MagicSphere.Earth && GetSpherePicks(MagicSphere.Air) != 0)
                                        {
                                            Result = false;
                                        }
                                        else
                                        {
                                            if (Sphere == MagicSphere.Air && GetSpherePicks(MagicSphere.Earth) != 0)
                                            {
                                                Result = false;
                                            }
                                            else
                                            {
                                                Result = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:2842
    public bool AddSphere(MagicSphere Sphere)
    {
        bool Result = CanAddSphere(Sphere);
        if (Result)
        {
            FSpheres.Add((byte)Sphere);
        }

        return Result;
    }

    // Hero.PAS:2863
    public bool DeleteSphereIndex(int Index)
    {
        bool Result;
        if (Index >= 0 && Index < FSpheres.Count)
        {
            FSpheres.Delete(Index);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Hero.PAS:2870
    public bool DeleteSphere(MagicSphere Sphere)
    {
        int I;
        I = FSpheres.Count;
        bool Result = false;
        while (!Result && I != 0)
        {
            I -= 1;
            Result = FSpheres[I] == (byte)Sphere;
        }

        if (Result)
        {
            FSpheres.Delete(I);
        }

        return Result;
    }

    // Hero.PAS:2883
    public void ClearSpheres()
    {
        FSpheres.Clear();
    }

}
