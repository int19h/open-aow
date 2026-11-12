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
        ExternalPowerSource source;

        Spells = new Int32List();
        _changedEventsStore = new();
        Enchantments = new EnchantmentList(null);
        PowerSources = new PowerSourceList(null);
        source = new ExternalPowerSource(null);
        ExternalPowerSource = source;
        source.ExternalPowerControl = this;
        PowerSources.AddChild(source);
        ResearchPointsLeft = 0;
        ResearchSpellId = 0;
        ResearchPercentage = 50;
        Spheres = Set<MagicSphere>.Of().WithRange(MagicSphere.Cosmos, MagicSphere.Secret);
        NextEnchantmentId = 1;
    }

    public int NextEnchantmentId;
    public bool CheatEnabled;
    public Player Player;
    public int UpdateCount;
    public int ExternalMana;
    public int ExternalPower;
    public ExternalPowerSource ExternalPowerSource;
    public int Mana;
    public Set<MagicSphere> Spheres;
    public int ResearchPercentage;
    public Int32List Spells;
    public int ResearchSpellId;
    public int ResearchPointsLeft;
    public EnchantmentList Enchantments;
    public PowerSourceList PowerSources;
    public Set<PlayerEnchantmentType> ActiveEnchantments;
    public Set<PlayerEnchantmentType> EnemyEnchantments;

    internal EventStore<PlayerMagicControl> _changedEventsStore;

    public event NotifyEvent<PlayerMagicControl> FChangedEvents
    {
        add => _changedEventsStore.Add(value);
        remove => _changedEventsStore.Remove(value);
    }

    // Spells.PAS:1927
    public override void Dispose()
    {
        try
        {
            Spells?.Dispose();
            Enchantments?.Dispose();
            _changedEventsStore.Clear();
            PowerSources.RemoveChild(ExternalPowerSource);
            if (PowerSources.GetCount() != 0 && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
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
            PowerSources?.Dispose();
            ExternalPowerSource?.Dispose();
            base.Dispose();
        }
        catch
        {
        }
    }

    // Spells.PAS:1947
    // { Original bug: cleanup errors are silently swallowed. }
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Mana, 20);
        stream.rwIntegerEx(ref ResearchPercentage, 50, 21);
        stream.rwStreamProc(22, Spells.LoadFromStream, Spells.SaveToStream);
        stream.rwInteger(ref ResearchSpellId, 23);
        stream.rwInteger(ref ResearchPointsLeft, 24);
        stream.rwEObject(Enchantments, 25);
        RwSet(ref ActiveEnchantments, 1, 26, stream.rwData, 0);
        stream.rwInteger(ref ExternalMana, 27);
        stream.rwInteger(ref ExternalPower, 28);
        stream.rwBool(ref CheatEnabled, 29);
        stream.rwInteger(ref NextEnchantmentId, 30);
    }

    // Spells.PAS:2243
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == Player.PlayerIndex)
        {
            NewTurn();
        }

        Enchantments.MessageProc(ref message, sender);
    }

    // Spells.PAS:1962
    public bool GetCheatEnabled()
    {
        bool result;
        if (Player.PlayerType == PlayerType.Human)
        {
            result = CheatEnabled;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:1967
    public void SetActiveEnchantments(Set<PlayerEnchantmentType> value)
    {
        ActiveEnchantments = value;
        for (int I = AoWHSMap.Current.Players.GetCount() - 1; I >= 1; I--)
        {
            if (I != Player.PlayerIndex)
            {
                AoWHSMap.Current.Players.GetPlayers(I).MagicControl.UpdateEnemyEnchantments();
            }
        }
    }

    // Spells.PAS:1976
    public void UpdateEnemyEnchantments()
    {
        PlayerMagicControl magic;
        EnemyEnchantments = Set<PlayerEnchantmentType>.Of();
        for (int I = AoWHSMap.Current.Players.GetCount() - 1; I >= 1; I--)
        {
            if (I != Player.PlayerIndex)
            {
                if (AoWHSMap.Current.Players.GetPlayers(I).DiplomaticRelations.GetDiplomaticRelation(Player.PlayerIndex) == DiplomaticRelation.War)
                {
                    magic = AoWHSMap.Current.Players.GetPlayers(I).MagicControl;
                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
                    {
                        EnemyEnchantments = EnemyEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery);
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
                    {
                        EnemyEnchantments = EnemyEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery);
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
                    {
                        EnemyEnchantments = EnemyEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery);
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery))
                    {
                        EnemyEnchantments = EnemyEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery);
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
        int result = 0;
        for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
        {
            result += Enchantments.GetEnchantments(I).Upkeep;
        }

        result += AoWHSMap.Current.GlobalMagicControl.GetEnchantmentUpkeep(Player.PlayerIndex);
        return result;
    }

    // Spells.PAS:2004
    public void SetMana(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value != Mana)
        {
            Mana = value;
            if (AoWHSMap.Current.VictoryCondition == AoWHSMapVictoryCondition.AccumulateMana && AoWHSMap.Current.VictoryMana <= Mana)
            {
                AoWHSMap.Current.GameOver(Player.PlayerIndex, GameOverStatus.Won);
            }

            Update();
        }
    }

    // Spells.PAS:2015
    public int GetPower()
    {
        int result = 0;
        for (int I = PowerSources.GetCount() - 1; I >= 0; I--)
        {
            result += PowerSources.GetItems(I).Power();
        }

        return result;
    }

    // Spells.PAS:2023
    public void OutOfMana(int mana)
    {
        int I;
        int index;
        PlayerMagicEventLog log;
        while (mana > 0 && Enchantments.GetCount() != 0)
        {
            I = Enchantments.GetCount();
            index = I - 1;
            while (I != 0)
            {
                I -= 1;
                var enchantments = Enchantments.GetEnchantments(I);
                var fEnchantmentsEnchantments = Enchantments.GetEnchantments(index);
                if (enchantments.Upkeep < fEnchantmentsEnchantments.Upkeep)
                {
                    index = I;
                }
            }

            if (index != -1)
            {
                mana -= Enchantments.GetEnchantments(index).Upkeep;
                log = new PlayerMagicEventLog(null);
                log._player = Player.PlayerIndex;
                log.Mode = PlayerMagicEventLog.EventMode.OutOfMana;
                Enchantments.GetEnchantments(index).GetLocation(ref log.Location.X, ref log.Location.Y, ref log.Location.MapLevel);
                log.EnchantmentName = Enchantments.GetEnchantments(index).GetName();
                AoWHSMap.Current.Players.GetPlayers(Player.PlayerIndex).EventLogbook.AddEvent(log);
                log.Release();
                ExecuteDispelEnchantment(index);
            }
        }

        if (mana > 0)
        {
            AoWHSMap.Current.GlobalMagicControl.OutOfMana((sbyte)(byte)Player.PlayerIndex, mana);
        }
    }

    // Spells.PAS:2052
    public void RemoveEnchantments()
    {
        while (Enchantments.GetCount() != 0)
        {
            ExecuteDispelEnchantment(Enchantments.GetCount() - 1);
        }
    }

    // Spells.PAS:2056
    public bool ValidateEnchantmentsUpkeep()
    {
        bool result;
        int balance;
        var manaIncome = GetManaIncome();
        var manaUpkeep = GetManaUpkeep();
        balance = Mana + manaIncome - manaUpkeep;
        if (balance < 0 && GetManaUpkeep() != 0)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Spells.PAS:2064
    public bool ExecuteSpellResearched(int spellId)
    {
        if (Spells.IndexOf(spellId) == -1)
        {
            Spells.Add(spellId);
        }

        if (spellId == ResearchSpellId)
        {
            ResearchPointsLeft = 0;
            ResearchSpellId = 0;
        }

        return true;
    }

    // Spells.PAS:2074
    public void NewTurn()
    {
        SpellType kind = default;
        Int32List candidates = default;
        int index = default;
        Spell spell = default;
        PlayerMagicEventLog log;
        if (AoWHSMap.Current.Day == 1 && Player.PlayerIndex != 0 && (AoWHSMap.Current.GameType != GameType.Campaign || Spells.Count == 0))
        {
            candidates = new Int32List();
            for (int kindOrdinal = (int)(SpellType)0; kindOrdinal <= (int)(SpellType)2; kindOrdinal++)
            {
                kind = (SpellType)kindOrdinal;
                candidates.Clear();
                for (int I = 0, last = AoWHSSet.Current.SpellControl.GetCount() - 1; I <= last; I++)
                {
                    spell = AoWHSSet.Current.SpellControl.GetSpell(I);
                    if (spell != null && Spells.IndexOf(I) == -1 && spell.SpellType == kind && (spell.MagicSphere == MagicSphere.Cosmos || Player.Leader.GetSpherePicks(spell.MagicSphere) > 0) && spell.SpellLevel == 1)
                    {
                        candidates.Add(I);
                    }
                }

                if (candidates.Count > 0)
                {
                    index = AoWHSMap.Current.Random(candidates.Count);
                    var item = candidates[index];
                    Spells.Add(item);
                }
            }

            candidates?.Dispose();
        }

        if (ResearchSpellId != 0)
        {
            if (Spells.IndexOf(ResearchSpellId) == -1)
            {
                var netPowerToResearch = GetNetPowerToResearch();
                ResearchPointsLeft -= netPowerToResearch;
                if (ResearchPointsLeft <= 0)
                {
                    if (ResearchPointsLeft < 0)
                    {
                        Mana += Math.Abs(ResearchPointsLeft);
                    }

                    log = new PlayerMagicEventLog(null);
                    log._player = Player.PlayerIndex;
                    log.Mode = PlayerMagicEventLog.EventMode.SpellResearched;
                    log.SpellId = ResearchSpellId;
                    Player.EventLogbook.AddEvent(log);
                    ExecuteSpellResearched(ResearchSpellId);
                    if (Player.PlayerIndex == AoWHSMap.Current.SeatedPlayer && Player.PlayerType == PlayerType.Human)
                    {
                        log.Execute(false, ExecuteTrigger.Event, null);
                    }

                    log.Release();
                }
            }
            else
            {
                ResearchSpellId = 0;
                var netPowerToResearch = GetNetPowerToResearch();
                Mana += netPowerToResearch;
            }
        }
        else
        {
            var netPowerToResearch = GetNetPowerToResearch();
            Mana += netPowerToResearch;
        }

        var manaIncome = GetManaIncome();
        var manaUpkeep = GetManaUpkeep();
        Mana = Mana + manaIncome - manaUpkeep;
        if (Mana < 0)
        {
            OutOfMana(-Mana);
            Mana = 0;
        }

        // { Original bug: SetMana sees the value already stored and skips its victory check. }
        SetMana(Mana);
        Update();
    }

    // Spells.PAS:2140
    public void SetSpheres(Set<MagicSphere> value)
    {
        if ((value & Set<MagicSphere>.Of(MagicSphere.Earth, MagicSphere.Air)) != Set<MagicSphere>.Of(MagicSphere.Earth, MagicSphere.Air) && (value & Set<MagicSphere>.Of(MagicSphere.Fire, MagicSphere.Water)) != Set<MagicSphere>.Of(MagicSphere.Fire, MagicSphere.Water) && (value & Set<MagicSphere>.Of(MagicSphere.Life, MagicSphere.Death)) != Set<MagicSphere>.Of(MagicSphere.Life, MagicSphere.Death))
        {
            Spheres = value;
        }
    }

    // Spells.PAS:2146
    public int GetNetPower()
    {
        int result = GetPower();
        if (AoWHSMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.PowerLeak))
        {
            result = (result + 1) / 2;
        }

        return result;
    }

    // Spells.PAS:2151
    public int GetNetPowerToMana()
    {
        int result = GetNetPower() - GetNetPowerToResearch();
        switch (Player.PlayerType)
        {
            case PlayerType.Cpu2:
                result = Round((double)result * 1.25);
                break;
            case PlayerType.Cpu3:
                result = Round((double)result * 1.5);
                break;
            case PlayerType.Cpu4:
                result = Round((double)result * 1.75);
                break;
            case PlayerType.Cpu5:
                result = Round((double)result * 2);
                break;
        }

        return result;
    }

    // Spells.PAS:2161
    public int GetNetPowerToResearch() => Round((double)(GetNetPower() * ResearchPercentage) * 0.01);

    // Spells.PAS:2163
    public int ResearchTurns(int power, int points)
    {
        int result;
        if (power > 0)
        {
            result = points / power;
            if (points % power != 0)
            {
                result += 1;
            }
        }
        else
        {
            result = 999;
        }

        return result;
    }

    // Spells.PAS:2172
    public int GetResearchTurnsLeft() => ResearchTurns(GetNetPowerToResearch(), ResearchPointsLeft);

    // Spells.PAS:2176
    public bool ValidResearchSpell(Spell spell)
    {
        bool result;
        Int32Array4 counts;
        int level;
        Spell known;
        if (Player.Leader.GetSpherePicks(spell.MagicSphere) >= spell.SpellLevel && Spells.IndexOf(spell.SpellId) == -1)
        {
            counts = default;
            for (int I = Spells.Count - 1; I >= 0; I--)
            {
                var item = Spells[I];
                known = AoWHSSet.Current.SpellControl.GetSpell(item);
                if (known.SpellLevel > 0 && known.GetEnabled() && spell.MagicSphere == known.MagicSphere)
                {
                    counts[known.SpellLevel - 1] += 1;
                }
            }

            level = 1;
            while (counts[level - 1] >= 2 && level < 4)
            {
                level += 1;
            }

            if (GameTables.GameEngine.Demo)
            {
                level = 2;
            }

            result = level >= spell.SpellLevel;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2199
    public void ListResearchSpells(ref SpellList list, Set<SpellType> spellTypes)
    {
        Spell spell;
        for (int I = 0, last = AoWHSSet.Current.SpellControl.GetCount() - 1; I <= last; I++)
        {
            spell = AoWHSSet.Current.SpellControl.GetSpell(I);
            if (spell != null && spellTypes.Contains(spell.SpellType) && ValidResearchSpell(spell))
            {
                list.Add(spell);
            }
        }
    }

    // Spells.PAS:2211
    public void ListSpells(ref SpellList list, Set<SpellType> spellTypes)
    {
        Spell spell;
        if (CheatEnabled && Player.PlayerType == PlayerType.Human)
        {
            for (int I = 0, last = AoWHSSet.Current.SpellControl.GetCount() - 1; I <= last; I++)
            {
                spell = AoWHSSet.Current.SpellControl.GetSpell(I);
                if (spell != null && spell.GetEnabled() && spellTypes.Contains(spell.SpellType))
                {
                    list.Add(spell);
                }
            }
        }
        else
        {
            for (int I = 0, last = Spells.Count - 1; I <= last; I++)
            {
                var item = Spells[I];
                spell = AoWHSSet.Current.SpellControl.GetSpell(item);
                if (spell != null && spell.GetEnabled() && spellTypes.Contains(spell.SpellType))
                {
                    list.Add(spell);
                }
            }
        }
    }

    // Spells.PAS:2231
    public void BeginUpdate()
    {
        UpdateCount += 1;
    }

    // Spells.PAS:2233
    public void EndUpdate()
    {
        UpdateCount -= 1;
        if (UpdateCount == 0)
        {
            Update();
        }
    }

    // Spells.PAS:2235
    public void Update()
    {
        if (UpdateCount == 0)
        {
            UpdateEnemyEnchantments();
            _changedEventsStore.Raise(this);
        }
    }

    // Spells.PAS:2249
    public void ValidateEnchantmentId(Enchantment enchantment)
    {
        int id;
        // { Preserve a valid unused ID before allocating a new one. }
        if (enchantment.EnchantmentId != 0 && (int)(((uint)enchantment.EnchantmentId) >> 24) == Player.PlayerIndex)
        {
            id = enchantment.EnchantmentId;
            enchantment.EnchantmentId = 0;
            if (Enchantments.IndexOfId(id) == -1)
            {
                enchantment.EnchantmentId = id;
            }
        }
        else
        {
            enchantment.EnchantmentId = 0;
        }

        if (enchantment.EnchantmentId == 0)
        {
            if (NextEnchantmentId == 0)
            {
                NextEnchantmentId = 1;
            }

            id = (Player.PlayerIndex << 24) + NextEnchantmentId;
            NextEnchantmentId += 1;
            if (NextEnchantmentId > 1048575)
            {
                NextEnchantmentId = 1;
            }

            while (Enchantments.IndexOfId(id) != -1)
            {
                id = (Player.PlayerIndex << 24) + NextEnchantmentId;
                NextEnchantmentId += 1;
                if (NextEnchantmentId > 1048575)
                {
                    NextEnchantmentId = 1;
                }
            }

            enchantment.EnchantmentId = id;
        }
    }

    // Spells.PAS:2275
    public bool RegisterEnchantment(Enchantment enchantment)
    {
        bool result;
        if (Enchantments._children.IndexOf(enchantment) == -1)
        {
            Enchantments.AddChild(enchantment);
            ValidateEnchantmentId(enchantment);
            Update();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2286
    public bool UnregisterEnchantment(Enchantment enchantment)
    {
        bool result = Enchantments._children.Remove(enchantment) != -1;
        if (result)
        {
            Update();
        }

        return result;
    }

    // Spells.PAS:2291
    public bool RegisterPowerSource(PowerSource source)
    {
        bool result;
        if (PowerSources.FindChildIndex(source) == -1)
        {
            PowerSources.AddChild(source);
            Update();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2301
    public bool UnregisterPowerSource(PowerSource source)
    {
        bool result = PowerSources.RemoveChild(source) != -1;
        if (result)
        {
            Update();
        }

        return result;
    }

    // Spells.PAS:2306
    public ChangePlayerMagicTokenEvent CreateChangeMagicTokenEvent()
    {
        ChangePlayerMagicTokenEvent result = new ChangePlayerMagicTokenEvent(null);
        result.PlayerIndex = (byte)Player.PlayerIndex;
        return result;
    }

    // Spells.PAS:2311
    public bool ExecuteDispelEnchantment(int index)
    {
        bool result;
        if (index < Enchantments.GetCount())
        {
            result = Enchantments.GetEnchantments(index).Dispel();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2316
    public bool DispelEnchantment(Enchantment enchantment)
    {
        bool result;
        int index;
        ChangePlayerMagicTokenEvent eventValue;
        index = Enchantments._children.IndexOf(enchantment);
        if (!Player.GetBusy() && index != -1)
        {
            eventValue = CreateChangeMagicTokenEvent();
            eventValue.Mode = ChangePlayerMagicTokenEventMode.DispelEnchantment;
            eventValue.Value = index;
            AoWHSMap.Current.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2333
    public bool ExecuteChangePowerSettings(int percentage)
    {
        ResearchPercentage = percentage;
        Update();
        return true;
    }

    // Spells.PAS:2339
    public bool ChangePowerSettings(int percentage)
    {
        bool result;
        ChangePlayerMagicTokenEvent eventValue;
        if (!Player.GetBusy())
        {
            if (percentage != ResearchPercentage)
            {
                eventValue = CreateChangeMagicTokenEvent();
                eventValue.Mode = ChangePlayerMagicTokenEventMode.ChangePowerSettings;
                eventValue.Value = percentage;
                AoWHSMap.Current.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
                eventValue.Release();
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2357
    public bool ExecuteResearchSpell(int spellId)
    {
        bool result;
        if (Spells.IndexOf(spellId) == -1 && spellId != ResearchSpellId)
        {
            ResearchSpellId = spellId;
            if (spellId != 0)
            {
                ResearchPointsLeft = AoWHSSet.Current.SpellControl.GetSpell(spellId).ResearchCost;
            }
            else
            {
                ResearchPointsLeft = 0;
            }

            Update();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2369
    public bool ResearchSpell(int spellId)
    {
        bool result;
        ChangePlayerMagicTokenEvent eventValue;
        if (!Player.GetBusy())
        {
            eventValue = CreateChangeMagicTokenEvent();
            eventValue.Mode = ChangePlayerMagicTokenEventMode.ResearchSpell;
            eventValue.Value = spellId;
            AoWHSMap.Current.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:2384
    public int AIRequestOverrideUpkeep(int priority)
    {
        GlobalEnchantment enchantment;
        GlobalMagicControl globalControl;
        int result = 0;
        for (int I = 0, last = Enchantments.GetCount() - 1; I <= last; I++)
        {
            if (priority >= Enchantments.GetEnchantments(I).GetAIUpkeepPriority())
            {
                result += Enchantments.GetEnchantments(I).Upkeep;
            }
        }

        // { Original bug: this estimate also counts global enchantments owned by other players. }
        globalControl = AoWHSMap.Current.GlobalMagicControl;
        for (int I = globalControl.Enchantments.GetCount() - 1; I >= 0; I--)
        {
            enchantment = AoWHSMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(I);
            if (priority >= enchantment.GetAIUpkeepPriority())
            {
                result += enchantment.GetUpkeep();
            }
        }

        return result;
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
        for (int i = 0; i < Enchantments.GetCount(); i++)
        {
            var enchantment = Enchantments.GetEnchantments(i);
            int order = enchantment.GetAIUpkeepPriority();
            if (order <= priority) list.Add(new UpkeepCandidate { Order = order, Upkeep = enchantment.Upkeep, Enchantment = enchantment });
        }
        var global = AoWHSMap.Current.GlobalMagicControl;
        for (int i = global.Enchantments.GetCount() - 1; i >= 0; i--)
        {
            var enchantment = AoWHSMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(i);
            if (enchantment.PlayerIndex != Player.PlayerIndex) continue;
            int order = enchantment.GetAIUpkeepPriority();
            if (order <= priority) list.Add(new UpkeepCandidate { Order = order, Upkeep = enchantment.GetUpkeep(), GlobalEnchantment = enchantment });
        }
        if (list.Count != 0)
        {
            int total = 0;
            list.Sort(static (in item1, in item2) => item1.Order - item2.Order);
            while (total < amount && list.Count != 0 && Player.GameOverStatus != GameOverStatus.Lost)
            {
                var item = list[0];
                total += item.Upkeep;
                if (item.Enchantment != null)
                    ExecuteDispelEnchantment(Enchantments._children.IndexOf(item.Enchantment));
                else
                {
                    var enchantment = item.GlobalEnchantment;
                    enchantment.AddRef();
                    enchantment.Remove((byte)Player.PlayerIndex);
                    enchantment.Release();
                }
                list.Delete(0);
            }
        }
        list?.Dispose();
        return 0;
    }
}
