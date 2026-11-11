// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Commands;

using Aow.Game.Events;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// (next ID, cheat, player, updates, external mana/power, external source, mana, spheres,
// research percentage, spells, research spell, points, enchantments, power sources, enchantment
// sets, changed events)
public partial class PlayerMagicControl : EngineObject
{

    // Spells.PAS:1908
    // next enchantment ID, cheat, player, update count, external mana, external power, external
    // source, mana, spheres, research percentage, spells, research spell, points, enchantments,
    // power sources, active/enemy enchantments, change events
    public PlayerMagicControl(ECustomNode owner) : base(owner)
    {
        ExternalPowerSource Source;

        FSpells = new Int32List();
        FChangedEventsStore = new();
        FEnchantments = new EnchantmentList(null);
        FPowerSources = new PowerSourceList(null);
        Source = new ExternalPowerSource(null);
        FExternalPowerSource = Source;
        Source.FExternalPowerControl = this;
        FPowerSources.AddChild(Source);
        FResearchPointsLeft = 0;
        FResearchSpellID = 0;
        FResearchPercentage = 50;
        FSpheres = Set<MagicSphere>.Of().WithRange(MagicSphere.Cosmos, MagicSphere.Secret);
        FNextEnchantmentID = 1;
    }

    public int FNextEnchantmentID;
    public bool FCheatEnabled;
    public Player FPlayer;
    public int FUpdateCount;
    public int FExternalMana;
    public int FExternalPower;
    public ExternalPowerSource FExternalPowerSource;
    public int FMana;
    public Set<MagicSphere> FSpheres;
    public int FResearchPercentage;
    public Int32List FSpells;
    public int FResearchSpellID;
    public int FResearchPointsLeft;
    public EnchantmentList FEnchantments;
    public PowerSourceList FPowerSources;
    public Set<PlayerEnchantmentType> FActiveEnchantments;
    public Set<PlayerEnchantmentType> FEnemyEnchantments;

    internal EventStore<PlayerMagicControl> FChangedEventsStore;

    public event NotifyEvent<PlayerMagicControl> FChangedEvents
    {
        add => FChangedEventsStore.Add(value);
        remove => FChangedEventsStore.Remove(value);
    }

    // Spells.PAS:1927
    public override void Dispose()
    {
        try
        {
            FSpells?.Dispose();
            FEnchantments?.Dispose();
            FChangedEventsStore.Clear();
            FPowerSources.RemoveChild(FExternalPowerSource);
            if (FPowerSources.GetCount() != 0 && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
            {
                try
                {
                    throw new InvalidOperationException("oohps");
                }
                catch
                {
                }
            }

            // { Original bug: the diagnostic exception is swallowed. }
            FPowerSources?.Dispose();
            FExternalPowerSource?.Dispose();
            base.Dispose();
        }
        catch
        {
        }
    }

    // Spells.PAS:1947
    // { Original bug: cleanup errors are silently swallowed. }
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FMana, 20);
        Stream.rwIntegerEx(ref FResearchPercentage, 50, 21);
        Stream.rwStreamProc(22, FSpells.LoadFromStream, FSpells.SaveToStream);
        Stream.rwInteger(ref FResearchSpellID, 23);
        Stream.rwInteger(ref FResearchPointsLeft, 24);
        Stream.rwEObject(FEnchantments, 25);
        RwSet(ref FActiveEnchantments, 1, 26, Stream.rwData, 0);
        Stream.rwInteger(ref FExternalMana, 27);
        Stream.rwInteger(ref FExternalPower, 28);
        Stream.rwBool(ref FCheatEnabled, 29);
        Stream.rwInteger(ref FNextEnchantmentID, 30);
    }

    // Spells.PAS:2243
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayer.FPlayerIndex)
        {
            NewTurn();
        }

        FEnchantments.MsgProc(ref Msg, Sender);
    }

    // Spells.PAS:1962
    public bool GetCheatEnabled()
    {
        bool Result;
        if (FPlayer.FPlayerType == PlayerType.Human)
        {
            Result = FCheatEnabled;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:1967
    public void SetActiveEnchantments(Set<PlayerEnchantmentType> Value)
    {
        FActiveEnchantments = Value;
        for (int I = AoWHSMap.Current.FPlayers.GetCount() - 1; I >= 1; I--)
        {
            if (I != FPlayer.FPlayerIndex)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.UpdateEnemyEnchantments();
            }
        }
    }

    // Spells.PAS:1976
    public void UpdateEnemyEnchantments()
    {
        PlayerMagicControl Magic;
        FEnemyEnchantments = Set<PlayerEnchantmentType>.Of();
        for (int I = AoWHSMap.Current.FPlayers.GetCount() - 1; I >= 1; I--)
        {
            if (I != FPlayer.FPlayerIndex)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(I).FDiplomaticRelations.GetDiplomaticRelation(FPlayer.FPlayerIndex) == DiplomaticRelation.War)
                {
                    Magic = AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl;
                    if (Magic.FActiveEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
                    {
                        FEnemyEnchantments = FEnemyEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery);
                    }

                    if (Magic.FActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
                    {
                        FEnemyEnchantments = FEnemyEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery);
                    }

                    if (Magic.FActiveEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
                    {
                        FEnemyEnchantments = FEnemyEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery);
                    }

                    if (Magic.FActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery))
                    {
                        FEnemyEnchantments = FEnemyEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery);
                    }
                }
            }
        }
    }

    // Spells.PAS:1993
    public int GetManaIncome() => GetNetPowerToMana();

    // Spells.PAS:1995
    public int GetManaUpkeep()
    {
        int Result = 0;
        for (int I = FEnchantments.GetCount() - 1; I >= 0; I--)
        {
            Result += FEnchantments.GetEnchantments(I).FUpkeep;
        }

        Result += AoWHSMap.Current.FGlobalMagicControl.GetEnchantmentUpkeep(FPlayer.FPlayerIndex);
        return Result;
    }

    // Spells.PAS:2004
    public void SetMana(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        if (Value != FMana)
        {
            FMana = Value;
            if (AoWHSMap.Current.FVictoryCondition == AoWHSMap.VictoryCondition.AccumulateMana && AoWHSMap.Current.FVictoryMana <= FMana)
            {
                AoWHSMap.Current.GameOver(FPlayer.FPlayerIndex, GameOverStatus.Won);
            }

            Update();
        }
    }

    // Spells.PAS:2015
    public int GetPower()
    {
        int Result = 0;
        for (int I = FPowerSources.GetCount() - 1; I >= 0; I--)
        {
            Result += FPowerSources.GetItems(I).Power();
        }

        return Result;
    }

    // Spells.PAS:2023
    public void OutOfMana(int Mana)
    {
        int I;
        int Index;
        PlayerMagicEventLog Log;
        while (Mana > 0 && FEnchantments.GetCount() != 0)
        {
            I = FEnchantments.GetCount();
            Index = I - 1;
            while (I != 0)
            {
                I -= 1;
                var enchantments = FEnchantments.GetEnchantments(I);
                var fEnchantmentsEnchantments = FEnchantments.GetEnchantments(Index);
                if (enchantments.FUpkeep < fEnchantmentsEnchantments.FUpkeep)
                {
                    Index = I;
                }
            }

            if (Index != -1)
            {
                Mana -= FEnchantments.GetEnchantments(Index).FUpkeep;
                Log = new PlayerMagicEventLog(null);
                Log.FPlayer = FPlayer.FPlayerIndex;
                Log.FMode = PlayerMagicEventLog.EventMode.OutOfMana;
                FEnchantments.GetEnchantments(Index).GetLocation(ref Log.FLocation.X, ref Log.FLocation.Y, ref Log.FLocation.L);
                Log.FEnchantmentName = FEnchantments.GetEnchantments(Index).GetName();
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer.FPlayerIndex).FEventLogbook.AddEvent(Log);
                Log.Release();
                ExecuteDispelEnchantment(Index);
            }
        }

        if (Mana > 0)
        {
            AoWHSMap.Current.FGlobalMagicControl.OutOfMana((sbyte)(byte)FPlayer.FPlayerIndex, Mana);
        }
    }

    // Spells.PAS:2052
    public void RemoveEnchantments()
    {
        while (FEnchantments.GetCount() != 0)
        {
            ExecuteDispelEnchantment(FEnchantments.GetCount() - 1);
        }
    }

    // Spells.PAS:2056
    public bool ValidateEnchantmentsUpkeep()
    {
        bool Result;
        int Balance;
        var manaIncome = GetManaIncome();
        var manaUpkeep = GetManaUpkeep();
        Balance = FMana + manaIncome - manaUpkeep;
        if (Balance < 0 && GetManaUpkeep() != 0)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Spells.PAS:2064
    public bool ExecuteSpellResearched(int SpellID)
    {
        if (FSpells.IndexOf(SpellID) == -1)
        {
            FSpells.Add(SpellID);
        }

        if (SpellID == FResearchSpellID)
        {
            FResearchPointsLeft = 0;
            FResearchSpellID = 0;
        }

        return true;
    }

    // Spells.PAS:2074
    public void NewTurn()
    {
        SpellType Kind = default;
        Int32List Candidates = default;
        int Index = default;
        Spell Spell = default;
        PlayerMagicEventLog Log;
        if (AoWHSMap.Current.FDay == 1 && FPlayer.FPlayerIndex != 0 && (AoWHSMap.Current.FGameType != GameType.Campaign || FSpells.Count == 0))
        {
            Candidates = new Int32List();
            for (int kindOrdinal = (int)(SpellType)0; kindOrdinal <= (int)(SpellType)2; kindOrdinal++)
            {
                Kind = (SpellType)kindOrdinal;
                Candidates.Clear();
                for (int I = 0, last = AoWHSSet.Current.FSpellControl.GetCount() - 1; I <= last; I++)
                {
                    Spell = AoWHSSet.Current.FSpellControl.GetSpell(I);
                    if (Spell != null && FSpells.IndexOf(I) == -1 && Spell.FSpellType == Kind && (Spell.FMagicSphere == MagicSphere.Cosmos || FPlayer.FLeader.GetSpherePicks(Spell.FMagicSphere) > 0) && Spell.FSpellLevel == 1)
                    {
                        Candidates.Add(I);
                    }
                }

                if (Candidates.Count > 0)
                {
                    Index = AoWHSMap.Current.Random(Candidates.Count);
                    var item = Candidates[Index];
                    FSpells.Add(item);
                }
            }

            Candidates?.Dispose();
        }

        if (FResearchSpellID != 0)
        {
            if (FSpells.IndexOf(FResearchSpellID) == -1)
            {
                var netPowerToResearch = GetNetPowerToResearch();
                FResearchPointsLeft -= netPowerToResearch;
                if (FResearchPointsLeft <= 0)
                {
                    if (FResearchPointsLeft < 0)
                    {
                        FMana += Math.Abs(FResearchPointsLeft);
                    }

                    Log = new PlayerMagicEventLog(null);
                    Log.FPlayer = FPlayer.FPlayerIndex;
                    Log.FMode = PlayerMagicEventLog.EventMode.SpellResearched;
                    Log.FSpellID = FResearchSpellID;
                    FPlayer.FEventLogbook.AddEvent(Log);
                    ExecuteSpellResearched(FResearchSpellID);
                    if (FPlayer.FPlayerIndex == AoWHSMap.Current.FSeatedPlayer && FPlayer.FPlayerType == PlayerType.Human)
                    {
                        Log.Execute(false, ExecuteTrigger.Event, null);
                    }

                    Log.Release();
                }
            }
            else
            {
                FResearchSpellID = 0;
                var netPowerToResearch = GetNetPowerToResearch();
                FMana += netPowerToResearch;
            }
        }
        else
        {
            var netPowerToResearch = GetNetPowerToResearch();
            FMana += netPowerToResearch;
        }

        var manaIncome = GetManaIncome();
        var manaUpkeep = GetManaUpkeep();
        FMana = FMana + manaIncome - manaUpkeep;
        if (FMana < 0)
        {
            OutOfMana(-FMana);
            FMana = 0;
        }

        // { Original bug: SetMana sees the value already stored and skips its victory check. }
        SetMana(FMana);
        Update();
    }

    // Spells.PAS:2140
    public void SetSpheres(Set<MagicSphere> Value)
    {
        if ((Value & Set<MagicSphere>.Of(MagicSphere.Earth, MagicSphere.Air)) != Set<MagicSphere>.Of(MagicSphere.Earth, MagicSphere.Air) && (Value & Set<MagicSphere>.Of(MagicSphere.Fire, MagicSphere.Water)) != Set<MagicSphere>.Of(MagicSphere.Fire, MagicSphere.Water) && (Value & Set<MagicSphere>.Of(MagicSphere.Life, MagicSphere.Death)) != Set<MagicSphere>.Of(MagicSphere.Life, MagicSphere.Death))
        {
            FSpheres = Value;
        }
    }

    // Spells.PAS:2146
    public int GetNetPower()
    {
        int Result = GetPower();
        if (AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.PowerLeak))
        {
            Result = (Result + 1) / 2;
        }

        return Result;
    }

    // Spells.PAS:2151
    public int GetNetPowerToMana()
    {
        int Result = GetNetPower() - GetNetPowerToResearch();
        switch (FPlayer.FPlayerType)
        {
            case PlayerType.CPU2:
                Result = Round((double)Result * 1.25);
                break;
            case PlayerType.CPU3:
                Result = Round((double)Result * 1.5);
                break;
            case PlayerType.CPU4:
                Result = Round((double)Result * 1.75);
                break;
            case PlayerType.CPU5:
                Result = Round((double)Result * 2);
                break;
        }

        return Result;
    }

    // Spells.PAS:2161
    public int GetNetPowerToResearch() => Round((double)(GetNetPower() * FResearchPercentage) * 0.01);

    // Spells.PAS:2163
    public int ResearchTurns(int Power, int Points)
    {
        int Result;
        if (Power > 0)
        {
            Result = Points / Power;
            if (Points % Power != 0)
            {
                Result += 1;
            }
        }
        else
        {
            Result = 999;
        }

        return Result;
    }

    // Spells.PAS:2172
    public int GetResearchTurnsLeft() => ResearchTurns(GetNetPowerToResearch(), FResearchPointsLeft);

    // Spells.PAS:2176
    public bool ValidResearchSpell(Spell Spell)
    {
        bool Result;
        Int32Array4 Counts;
        int Level;
        Spell Known;
        if (FPlayer.FLeader.GetSpherePicks(Spell.FMagicSphere) >= Spell.FSpellLevel && FSpells.IndexOf(Spell.FSpellID) == -1)
        {
            Counts = default;
            for (int I = FSpells.Count - 1; I >= 0; I--)
            {
                var item = FSpells[I];
                Known = AoWHSSet.Current.FSpellControl.GetSpell(item);
                if (Known.FSpellLevel > 0 && Known.GetEnabled() && Spell.FMagicSphere == Known.FMagicSphere)
                {
                    Counts[Known.FSpellLevel - 1] += 1;
                }
            }

            Level = 1;
            while (Counts[Level - 1] >= 2 && Level < 4)
            {
                Level += 1;
            }

            if (GameTables.AoWEngine.FDemo)
            {
                Level = 2;
            }

            Result = Level >= Spell.FSpellLevel;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2199
    public void ListResearchSpells(ref SpellList List, Set<SpellType> SpellTypes)
    {
        Spell Spell;
        for (int I = 0, last = AoWHSSet.Current.FSpellControl.GetCount() - 1; I <= last; I++)
        {
            Spell = AoWHSSet.Current.FSpellControl.GetSpell(I);
            if (Spell != null && SpellTypes.Contains(Spell.FSpellType) && ValidResearchSpell(Spell))
            {
                List.Add(Spell);
            }
        }
    }

    // Spells.PAS:2211
    public void ListSpells(ref SpellList List, Set<SpellType> SpellTypes)
    {
        Spell Spell;
        if (FCheatEnabled && FPlayer.FPlayerType == PlayerType.Human)
        {
            for (int I = 0, last = AoWHSSet.Current.FSpellControl.GetCount() - 1; I <= last; I++)
            {
                Spell = AoWHSSet.Current.FSpellControl.GetSpell(I);
                if (Spell != null && Spell.GetEnabled() && SpellTypes.Contains(Spell.FSpellType))
                {
                    List.Add(Spell);
                }
            }
        }
        else
        {
            for (int I = 0, last = FSpells.Count - 1; I <= last; I++)
            {
                var item = FSpells[I];
                Spell = AoWHSSet.Current.FSpellControl.GetSpell(item);
                if (Spell != null && Spell.GetEnabled() && SpellTypes.Contains(Spell.FSpellType))
                {
                    List.Add(Spell);
                }
            }
        }
    }

    // Spells.PAS:2231
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // Spells.PAS:2233
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            Update();
        }
    }

    // Spells.PAS:2235
    public void Update()
    {
        if (FUpdateCount == 0)
        {
            UpdateEnemyEnchantments();
            FChangedEventsStore.Raise(this);
        }
    }

    // Spells.PAS:2249
    public void ValidateEnchantmentID(Enchantment Enchantment)
    {
        int ID;
        // { Preserve a valid unused ID before allocating a new one. }
        if (Enchantment.FEnchantmentID != 0 && (int)(((uint)Enchantment.FEnchantmentID) >> 24) == FPlayer.FPlayerIndex)
        {
            ID = Enchantment.FEnchantmentID;
            Enchantment.FEnchantmentID = 0;
            if (FEnchantments.IndexOfID(ID) == -1)
            {
                Enchantment.FEnchantmentID = ID;
            }
        }
        else
        {
            Enchantment.FEnchantmentID = 0;
        }

        if (Enchantment.FEnchantmentID == 0)
        {
            if (FNextEnchantmentID == 0)
            {
                FNextEnchantmentID = 1;
            }

            ID = (FPlayer.FPlayerIndex << 24) + FNextEnchantmentID;
            FNextEnchantmentID += 1;
            if (FNextEnchantmentID > 1048575)
            {
                FNextEnchantmentID = 1;
            }

            while (FEnchantments.IndexOfID(ID) != -1)
            {
                ID = (FPlayer.FPlayerIndex << 24) + FNextEnchantmentID;
                FNextEnchantmentID += 1;
                if (FNextEnchantmentID > 1048575)
                {
                    FNextEnchantmentID = 1;
                }
            }

            Enchantment.FEnchantmentID = ID;
        }
    }

    // Spells.PAS:2275
    public bool RegisterEnchantment(Enchantment Enchantment)
    {
        bool Result;
        if (FEnchantments.FChildren.IndexOf(Enchantment) == -1)
        {
            FEnchantments.AddChild(Enchantment);
            ValidateEnchantmentID(Enchantment);
            Update();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2286
    public bool UnregisterEnchantment(Enchantment Enchantment)
    {
        bool Result = FEnchantments.FChildren.Remove(Enchantment) != -1;
        if (Result)
        {
            Update();
        }

        return Result;
    }

    // Spells.PAS:2291
    public bool RegisterPowerSource(PowerSource Source)
    {
        bool Result;
        if (FPowerSources.FindChildIndex(Source) == -1)
        {
            FPowerSources.AddChild(Source);
            Update();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2301
    public bool UnregisterPowerSource(PowerSource Source)
    {
        bool Result = FPowerSources.RemoveChild(Source) != -1;
        if (Result)
        {
            Update();
        }

        return Result;
    }

    // Spells.PAS:2306
    public ChangePlayerMagicTokenEvent CreateChangeMagicTE()
    {
        ChangePlayerMagicTokenEvent Result = new ChangePlayerMagicTokenEvent(null);
        Result.FPlayerIndex = (byte)FPlayer.FPlayerIndex;
        return Result;
    }

    // Spells.PAS:2311
    public bool ExecuteDispelEnchantment(int Index)
    {
        bool Result;
        if (Index < FEnchantments.GetCount())
        {
            Result = FEnchantments.GetEnchantments(Index).Dispel();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2316
    public bool DispelEnchantment(Enchantment Enchantment)
    {
        bool Result;
        int Index;
        ChangePlayerMagicTokenEvent Event;
        Index = FEnchantments.FChildren.IndexOf(Enchantment);
        if (!FPlayer.GetBusy() && Index != -1)
        {
            Event = CreateChangeMagicTE();
            Event.FMode = ChangePlayerMagicTokenEvent.Mode.DispelEnchantment;
            Event.FValue = Index;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer.FPlayerIndex).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2333
    public bool ExecuteChangePowerSettings(int Percentage)
    {
        FResearchPercentage = Percentage;
        Update();
        return true;
    }

    // Spells.PAS:2339
    public bool ChangePowerSettings(int Percentage)
    {
        bool Result;
        ChangePlayerMagicTokenEvent Event;
        if (!FPlayer.GetBusy())
        {
            if (Percentage != FResearchPercentage)
            {
                Event = CreateChangeMagicTE();
                Event.FMode = ChangePlayerMagicTokenEvent.Mode.ChangePowerSettings;
                Event.FValue = Percentage;
                AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer.FPlayerIndex).AddEvent(Event);
                Event.Release();
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2357
    public bool ExecuteResearchSpell(int SpellID)
    {
        bool Result;
        if (FSpells.IndexOf(SpellID) == -1 && SpellID != FResearchSpellID)
        {
            FResearchSpellID = SpellID;
            if (SpellID != 0)
            {
                FResearchPointsLeft = AoWHSSet.Current.FSpellControl.GetSpell(SpellID).FResearchCost;
            }
            else
            {
                FResearchPointsLeft = 0;
            }

            Update();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2369
    public bool ResearchSpell(int SpellID)
    {
        bool Result;
        ChangePlayerMagicTokenEvent Event;
        if (!FPlayer.GetBusy())
        {
            Event = CreateChangeMagicTE();
            Event.FMode = ChangePlayerMagicTokenEvent.Mode.ResearchSpell;
            Event.FValue = SpellID;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer.FPlayerIndex).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:2384
    public int AIRequestOverrideUpkeep(int Priority)
    {
        GlobalEnchantment Enchantment;
        GlobalMagicControl GlobalControl;
        int Result = 0;
        for (int I = 0, last = FEnchantments.GetCount() - 1; I <= last; I++)
        {
            if (Priority >= FEnchantments.GetEnchantments(I).GetAIUpkeepPriority())
            {
                Result += FEnchantments.GetEnchantments(I).FUpkeep;
            }
        }

        // { Original bug: this estimate also counts global enchantments owned by other players. }
        GlobalControl = AoWHSMap.Current.FGlobalMagicControl;
        for (int I = GlobalControl.FEnchantments.GetCount() - 1; I >= 0; I--)
        {
            Enchantment = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(I);
            if (Priority >= Enchantment.GetAIUpkeepPriority())
            {
                Result += Enchantment.GetUpkeep();
            }
        }

        return Result;
    }

    // intf_P4_01.inc:135
    // Spells.PAS:2400. The original list held the order and upkeep with the enchantment address in
    // column C for a player enchantment or D for a global one. An entry holds the enchantment itself.
    private struct UpkeepCandidate
    {
        public int Order;
        public int Upkeep;
        public Enchantment Enchantment;
        public GlobalEnchantment GlobalEnchantment;
    }

    public int AIOverrideUpkeep(int priority, int amount)
    {
        var list = new RecordList<UpkeepCandidate>();
        for (int i = 0; i < FEnchantments.GetCount(); i++)
        {
            var enchantment = FEnchantments.GetEnchantments(i);
            int order = enchantment.GetAIUpkeepPriority();
            if (order <= priority) list.Add(new UpkeepCandidate { Order = order, Upkeep = enchantment.FUpkeep, Enchantment = enchantment });
        }
        var global = AoWHSMap.Current.FGlobalMagicControl;
        for (int i = global.FEnchantments.GetCount() - 1; i >= 0; i--)
        {
            var enchantment = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(i);
            if (enchantment.FPlayerIndex != FPlayer.FPlayerIndex) continue;
            int order = enchantment.GetAIUpkeepPriority();
            if (order <= priority) list.Add(new UpkeepCandidate { Order = order, Upkeep = enchantment.GetUpkeep(), GlobalEnchantment = enchantment });
        }
        if (list.Count != 0)
        {
            int total = 0;
            list.Sort(static (in Item1, in Item2) => Item1.Order - Item2.Order);
            while (total < amount && list.Count != 0 && FPlayer.FGameOverStatus != GameOverStatus.Lost)
            {
                var item = list[0];
                total += item.Upkeep;
                if (item.Enchantment != null)
                    ExecuteDispelEnchantment(FEnchantments.FChildren.IndexOf(item.Enchantment));
                else
                {
                    var enchantment = item.GlobalEnchantment;
                    enchantment.AddRef();
                    enchantment.Remove((byte)FPlayer.FPlayerIndex);
                    enchantment.Release();
                }
                list.Delete(0);
            }
        }
        list?.Dispose();
        return 0;
    }
}
