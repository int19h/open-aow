#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Commands;

using Aow.Game.Events;

using Aow.Game.Magic;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.DelphiRounding;

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
    public PlayerMagicControl(CustomNode owner) : base(owner)
    {
        ExternalPowerSource source;

        Spells = new Int32List();
        _changedEventsStore = new();
        Enchantments = new EnchantmentList(null) { Holder = CreationHolder() };
        PowerSources = new PowerSourceList(null) { Holder = CreationHolder() };
        source = new ExternalPowerSource(null) { Holder = CreationHolder() };
        _externalPowerSource = source;
        source.ExternalPowerControl = this;
        _ = PowerSources.AddChild(source);
        ResearchPointsLeft = 0;
        ResearchSpellId = 0;
        ResearchPercentage = 50;
        _nextEnchantmentId = 1;
    }

    private int _nextEnchantmentId;

    public bool CheatEnabled { get; set; }

    public Player Player { get; init; }

    private int _updateCount;

    public int ExternalMana { get; set; }
    public int ExternalPower { get; private set; }

    private readonly ExternalPowerSource _externalPowerSource;

    public int Mana { get; private set; }

    public int ResearchPercentage { get; set; }

    public Int32List Spells { get; }

    public int ResearchSpellId { get; set; }
    public int ResearchPointsLeft { get; set; }

    public EnchantmentList Enchantments { get; init; }
    public PowerSourceList PowerSources { get; }

    public Set<PlayerEnchantmentType> ActiveEnchantments { get; private set; }

    public Set<PlayerEnchantmentType> EnemyEnchantments { get; private set; }

    private readonly EventStore<PlayerMagicControl> _changedEventsStore;

    public event NotifyEvent<PlayerMagicControl> Changed
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
            _ = PowerSources.RemoveChild(_externalPowerSource);
            if (PowerSources.GetCount() != 0 && StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
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
            _externalPowerSource?.Dispose();
            base.Dispose();
        }
        catch
        {
        }
    }

    // Spells.PAS:1947
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Mana = persister.Int32("Mana", value: Mana);
        ResearchPercentage = persister.Int32("ResearchPercentage", value: ResearchPercentage, fallback: 50);
        PersistSupport.Int32List(persister, name: "Spells", list: Spells);
        ResearchSpellId = persister.Int32("ResearchSpellId", value: ResearchSpellId);
        ResearchPointsLeft = persister.Int32("ResearchPointsLeft", value: ResearchPointsLeft);
        persister.Object("Enchantments", objectValue: Enchantments);
        ActiveEnchantments = persister.Set("ActiveEnchantments", value: ActiveEnchantments);
        ExternalMana = persister.Int32("ExternalMana", value: ExternalMana);
        ExternalPower = persister.Int32("ExternalPower", value: ExternalPower);
        CheatEnabled = persister.Boolean("CheatEnabled", value: CheatEnabled);
        _nextEnchantmentId = persister.Int32("NextEnchantmentId", value: _nextEnchantmentId);
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
        bool result = Player.PlayerType == PlayerType.Human && CheatEnabled;
        return result;
    }

    // Spells.PAS:1967
    public void SetActiveEnchantments(Set<PlayerEnchantmentType> value)
    {
        ActiveEnchantments = value;
        for (int i = StrategicMap.Current.Players.GetCount() - 1; i >= 1; i--)
        {
            if (i != Player.PlayerIndex)
            {
                StrategicMap.Current.Players.GetPlayers(i).MagicControl.UpdateEnemyEnchantments();
            }
        }
    }

    // Spells.PAS:1976
    public void UpdateEnemyEnchantments()
    {
        PlayerMagicControl magic;
        EnemyEnchantments = Set<PlayerEnchantmentType>.Of();
        for (int i = StrategicMap.Current.Players.GetCount() - 1; i >= 1; i--)
        {
            if (i != Player.PlayerIndex)
            {
                if (StrategicMap.Current.Players.GetPlayers(i).DiplomaticRelations.GetDiplomaticRelation(Player.PlayerIndex) == DiplomaticRelation.War)
                {
                    magic = StrategicMap.Current.Players.GetPlayers(i).MagicControl;
                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
                    {
                        EnemyEnchantments += Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery);
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
                    {
                        EnemyEnchantments += Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery);
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
                    {
                        EnemyEnchantments += Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery);
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery))
                    {
                        EnemyEnchantments += Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery);
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
        for (int i = Enchantments.GetCount() - 1; i >= 0; i--)
        {
            result += Enchantments.GetEnchantments(i).Upkeep;
        }

        result += StrategicMap.Current.GlobalMagicControl.GetEnchantmentUpkeep(Player.PlayerIndex);
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
            if (StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.AccumulateMana && StrategicMap.Current.VictoryMana <= Mana)
            {
                StrategicMap.Current.GameOver(Player.PlayerIndex, GameOverStatus.Won);
            }

            Update();
        }
    }

    // Spells.PAS:2015
    public int GetPower()
    {
        int result = 0;
        for (int i = PowerSources.GetCount() - 1; i >= 0; i--)
        {
            result += PowerSources.GetItems(i).Power();
        }

        return result;
    }

    // Spells.PAS:2023
    public void OutOfMana(int mana)
    {
        int i;
        int index;
        PlayerMagicEventLog log;
        while (mana > 0 && Enchantments.GetCount() != 0)
        {
            i = Enchantments.GetCount();
            index = i - 1;
            while (i != 0)
            {
                i -= 1;
                Enchantment enchantments = Enchantments.GetEnchantments(i);
                Enchantment fEnchantmentsEnchantments = Enchantments.GetEnchantments(index);
                if (enchantments.Upkeep < fEnchantmentsEnchantments.Upkeep)
                {
                    index = i;
                }
            }

            if (index != -1)
            {
                mana -= Enchantments.GetEnchantments(index).Upkeep;
                log = new PlayerMagicEventLog(null)
                {
                    Holder = CreationHolder(),
                    Player = Player.PlayerIndex,
                    Mode = PlayerMagicEventLog.EventMode.OutOfMana
                };
                MapCoordinate coordinate25 = Enchantments.GetEnchantments(index).GetLocation();
                sbyte locationX = coordinate25.X;
                sbyte locationY = coordinate25.Y;
                sbyte locationMapLevel = coordinate25.MapLevel;
                log.Location = new MapCoordinate(locationX, locationY, locationMapLevel);
                log.EnchantmentName = Enchantments.GetEnchantments(index).GetName();
                StrategicMap.Current.Players.GetPlayers(Player.PlayerIndex).EventLogbook.AddEvent(log);
                _ = log.Release();
                _ = ExecuteDispelEnchantment(index);
            }
        }

        if (mana > 0)
        {
            StrategicMap.Current.GlobalMagicControl.OutOfMana((sbyte)(byte)Player.PlayerIndex, mana);
        }
    }

    // Spells.PAS:2052
    public void RemoveEnchantments()
    {
        while (Enchantments.GetCount() != 0)
        {
            _ = ExecuteDispelEnchantment(Enchantments.GetCount() - 1);
        }
    }

    // Spells.PAS:2056
    public bool ValidateEnchantmentsUpkeep()
    {
        bool result;
        int balance;
        int manaIncome = GetManaIncome();
        int manaUpkeep = GetManaUpkeep();
        balance = Mana + manaIncome - manaUpkeep;
        result = balance >= 0 || GetManaUpkeep() == 0;

        return result;
    }

    // Spells.PAS:2064
    public bool ExecuteSpellResearched(int spellId)
    {
        if (Spells.IndexOf(spellId) == -1)
        {
            _ = Spells.Add(spellId);
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
        if (StrategicMap.Current.Day == 1 && Player.PlayerIndex != 0 && (StrategicMap.Current.GameType != GameType.Campaign || Spells.Count == 0))
        {
            candidates = new Int32List();
            for (int kindOrdinal = (int)(SpellType)0; kindOrdinal <= (int)(SpellType)2; kindOrdinal++)
            {
                kind = (SpellType)kindOrdinal;
                candidates.Clear();
                for (int i = 0, last = GamePack.Current.SpellControl.GetCount() - 1; i <= last; i++)
                {
                    spell = GamePack.Current.SpellControl.GetSpell(i);
                    if (spell is not null && Spells.IndexOf(i) == -1 && spell.SpellType == kind && (spell.MagicSphere == MagicSphere.Cosmos || Player.Leader.GetSpherePicks(spell.MagicSphere) > 0) && spell.SpellLevel == 1)
                    {
                        _ = candidates.Add(i);
                    }
                }

                if (candidates.Count > 0)
                {
                    index = StrategicMap.Current.Random(candidates.Count);
                    int item = candidates[index];
                    _ = Spells.Add(item);
                }
            }

            candidates?.Dispose();
        }

        if (ResearchSpellId != 0)
        {
            if (Spells.IndexOf(ResearchSpellId) == -1)
            {
                int netPowerToResearch = GetNetPowerToResearch();
                ResearchPointsLeft -= netPowerToResearch;
                if (ResearchPointsLeft <= 0)
                {
                    if (ResearchPointsLeft < 0)
                    {
                        Mana += Math.Abs(ResearchPointsLeft);
                    }

                    log = new PlayerMagicEventLog(null)
                    {
                        Holder = CreationHolder(),
                        Player = Player.PlayerIndex,
                        Mode = PlayerMagicEventLog.EventMode.SpellResearched,
                        SpellId = ResearchSpellId
                    };
                    Player.EventLogbook.AddEvent(log);
                    _ = ExecuteSpellResearched(ResearchSpellId);
                    if (Player.PlayerIndex == StrategicMap.Current.SeatedPlayer && Player.PlayerType == PlayerType.Human)
                    {
                        _ = log.Execute(false, ExecuteTrigger.Event, null);
                    }

                    _ = log.Release();
                }
            }
            else
            {
                ResearchSpellId = 0;
                int netPowerToResearch = GetNetPowerToResearch();
                Mana += netPowerToResearch;
            }
        }
        else
        {
            int netPowerToResearch = GetNetPowerToResearch();
            Mana += netPowerToResearch;
        }

        int manaIncome = GetManaIncome();
        int manaUpkeep = GetManaUpkeep();
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
        }
    }

    // Spells.PAS:2146
    public int GetNetPower()
    {
        int result = GetPower();
        if (StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.PowerLeak))
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
                result = Round(result * 1.25);
                break;
            case PlayerType.Cpu3:
                result = Round(result * 1.5);
                break;
            case PlayerType.Cpu4:
                result = Round(result * 1.75);
                break;
            case PlayerType.Cpu5:
                result = Round((double)result * 2);
                break;
            case PlayerType.Human:
            case PlayerType.Cpu1:
            case PlayerType.Independent:
            case PlayerType.Free:
            default:
                break;
        }

        return result;
    }

    // Spells.PAS:2161
    public int GetNetPowerToResearch() => Round(GetNetPower() * ResearchPercentage * 0.01);

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
        Span<int> counts = stackalloc int[4];
        int level;
        Spell known;
        if (Player.Leader.GetSpherePicks(spell.MagicSphere) >= spell.SpellLevel && Spells.IndexOf(spell.SpellId) == -1)
        {
            counts.Clear();
            for (int i = Spells.Count - 1; i >= 0; i--)
            {
                int item = Spells[i];
                known = GamePack.Current.SpellControl.GetSpell(item);
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

            if (GetEngine().Demo)
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
    public void ListResearchSpells(SpellList list, Set<SpellType> spellTypes)
    {
        Spell spell;
        for (int i = 0, last = GamePack.Current.SpellControl.GetCount() - 1; i <= last; i++)
        {
            spell = GamePack.Current.SpellControl.GetSpell(i);
            if (spell is not null && spellTypes.Contains(spell.SpellType) && ValidResearchSpell(spell))
            {
                list.Add(spell);
            }
        }
    }

    // Spells.PAS:2211
    public void ListSpells(SpellList list, Set<SpellType> spellTypes)
    {
        Spell spell;
        if (CheatEnabled && Player.PlayerType == PlayerType.Human)
        {
            for (int i = 0, last = GamePack.Current.SpellControl.GetCount() - 1; i <= last; i++)
            {
                spell = GamePack.Current.SpellControl.GetSpell(i);
                if (spell is not null && spell.GetEnabled() && spellTypes.Contains(spell.SpellType))
                {
                    list.Add(spell);
                }
            }
        }
        else
        {
            for (int i = 0, last = Spells.Count - 1; i <= last; i++)
            {
                int item = Spells[i];
                spell = GamePack.Current.SpellControl.GetSpell(item);
                if (spell is not null && spell.GetEnabled() && spellTypes.Contains(spell.SpellType))
                {
                    list.Add(spell);
                }
            }
        }
    }

    // Spells.PAS:2231
    public void BeginUpdate() => _updateCount += 1;

    // Spells.PAS:2233
    public void EndUpdate()
    {
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            Update();
        }
    }

    // Spells.PAS:2235
    public void Update()
    {
        if (_updateCount == 0)
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
            if (_nextEnchantmentId == 0)
            {
                _nextEnchantmentId = 1;
            }

            id = (Player.PlayerIndex << 24) + _nextEnchantmentId;
            _nextEnchantmentId += 1;
            if (_nextEnchantmentId > 1048575)
            {
                _nextEnchantmentId = 1;
            }

            while (Enchantments.IndexOfId(id) != -1)
            {
                id = (Player.PlayerIndex << 24) + _nextEnchantmentId;
                _nextEnchantmentId += 1;
                if (_nextEnchantmentId > 1048575)
                {
                    _nextEnchantmentId = 1;
                }
            }

            enchantment.EnchantmentId = id;
        }
    }

    // Spells.PAS:2275
    public bool RegisterEnchantment(Enchantment enchantment)
    {
        bool result;
        if (Enchantments.IndexOfChild(enchantment) == -1)
        {
            _ = Enchantments.AddChild(enchantment);
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
        bool result = Enchantments.DropChild(enchantment) != -1;
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
            _ = PowerSources.AddChild(source);
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
        ChangePlayerMagicTokenEvent result = new(null)
        {
            Holder = CreationHolder(),
            PlayerIndex = (byte)Player.PlayerIndex
        };
        return result;
    }

    // Spells.PAS:2311
    public bool ExecuteDispelEnchantment(int index)
    {
        bool result = index < Enchantments.GetCount() && Enchantments.GetEnchantments(index).Dispel();
        return result;
    }

    // Spells.PAS:2316
    public bool DispelEnchantment(Enchantment enchantment)
    {
        bool result;
        int index;
        ChangePlayerMagicTokenEvent eventValue;
        index = Enchantments.IndexOfChild(enchantment);
        if (!Player.GetBusy() && index != -1)
        {
            eventValue = CreateChangeMagicTokenEvent();
            eventValue.Mode = ChangePlayerMagicTokenEventMode.DispelEnchantment;
            eventValue.Value = index;
            StrategicMap.Current.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
            _ = eventValue.Release();
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
                StrategicMap.Current.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
                _ = eventValue.Release();
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
            ResearchPointsLeft = spellId != 0 ? GamePack.Current.SpellControl.GetSpell(spellId).ResearchCost : 0;

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
            StrategicMap.Current.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
            _ = eventValue.Release();
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
        for (int i = 0, last = Enchantments.GetCount() - 1; i <= last; i++)
        {
            if (priority >= Enchantments.GetEnchantments(i).GetAIUpkeepPriority())
            {
                result += Enchantments.GetEnchantments(i).Upkeep;
            }
        }

        // { Original bug: this estimate also counts global enchantments owned by other players. }
        globalControl = StrategicMap.Current.GlobalMagicControl;
        for (int i = globalControl.Enchantments.GetCount() - 1; i >= 0; i--)
        {
            enchantment = StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(i);
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
    private readonly struct UpkeepCandidate
    {
        public int Order { get; init; }
        public int Upkeep { get; init; }
        public Enchantment Enchantment { get; init; }
        public GlobalEnchantment GlobalEnchantment { get; init; }
    }

    public int AIOverrideUpkeep(int priority, int amount)
    {
        RecordList<UpkeepCandidate> list = new();
        for (int i = 0; i < Enchantments.GetCount(); i++)
        {
            Enchantment enchantment = Enchantments.GetEnchantments(i);
            int order = enchantment.GetAIUpkeepPriority();
            if (order <= priority)
            {
                _ = list.Add(new UpkeepCandidate { Order = order, Upkeep = enchantment.Upkeep, Enchantment = enchantment });
            }
        }
        GlobalMagicControl global = StrategicMap.Current.GlobalMagicControl;
        for (int i = global.Enchantments.GetCount() - 1; i >= 0; i--)
        {
            GlobalEnchantment enchantment = StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(i);
            if (enchantment.PlayerIndex != Player.PlayerIndex)
            {
                continue;
            }

            int order = enchantment.GetAIUpkeepPriority();
            if (order <= priority)
            {
                _ = list.Add(new UpkeepCandidate { Order = order, Upkeep = enchantment.GetUpkeep(), GlobalEnchantment = enchantment });
            }
        }
        if (list.Count != 0)
        {
            int total = 0;
            list.Sort(static (in item1, in item2) => item1.Order - item2.Order);
            while (total < amount && list.Count != 0 && Player.GameOverStatus != GameOverStatus.Lost)
            {
                UpkeepCandidate item = list[0];
                total += item.Upkeep;
                if (item.Enchantment is not null)
                {
                    _ = ExecuteDispelEnchantment(Enchantments.IndexOfChild(item.Enchantment));
                }
                else
                {
                    GlobalEnchantment enchantment = item.GlobalEnchantment;
                    _ = enchantment.AddRef();
                    _ = enchantment.Remove((byte)Player.PlayerIndex);
                    _ = enchantment.Release();
                }
                list.Delete(0);
            }
        }
        list?.Dispose();
        return 0;
    }
}
