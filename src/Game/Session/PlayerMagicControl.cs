#nullable disable warnings
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
public sealed class PlayerMagicControl : EngineObject
{

    // Spells.PAS:1908
    // next enchantment ID, cheat, player, update count, external mana, external power, external
    // source, mana, spheres, research percentage, spells, research spell, points, enchantments,
    // power sources, active/enemy enchantments, change events
    public PlayerMagicControl(CustomNode? owner) : base(owner)
    {
        Spells = [];
        Enchantments = new EnchantmentList(null) { Holder = CreationHolder() };
        PowerSources = new PowerSourceList(null) { Holder = CreationHolder() };
        ExternalPowerSource source = new(null) { Holder = CreationHolder() };
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

    public PlayerEnchantmentTypes ActiveEnchantments { get; private set; }

    public PlayerEnchantmentTypes EnemyEnchantments { get; private set; }

    public event NotifyEvent<PlayerMagicControl>? Changed;

    // Spells.PAS:1927
    public override void Dispose()
    {
        try
        {
            Enchantments?.Dispose();
            Changed = null;
            _ = PowerSources.RemoveChild(_externalPowerSource);
            // A control that reaches no map has no destroying map to report and still disposes its sources.
            if (PowerSources.GetCount() != 0 && GetEngine()?.StrategicMap is { } strategicMap && strategicMap.Status.Contains(HexSpriteMapStatuses.Destroying))
            {
                try
                {
                    throw new InvalidOperationException("oohps");
                }
                catch (Exception error)
                {
                    HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
                }
            }

            // { Original bug: the diagnostic exception is swallowed. }
            PowerSources?.Dispose();
            _externalPowerSource?.Dispose();
            base.Dispose();
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
        }
    }

    // Spells.PAS:1947
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Mana = persister.Int32("Mana", value: Mana);
        ResearchPercentage = persister.Int32("ResearchPercentage", value: ResearchPercentage, fallback: 50);
        persister.Int32List(name: "Spells", list: Spells);
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
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == PlayerIdExtensions.ToOriginal(Player.PlayerIndex))
        {
            NewTurn();
        }

        Enchantments.MessageProc(ref message, sender);
    }

    // Spells.PAS:1962
    public bool GetCheatEnabled() => Player.PlayerType == PlayerType.Human && CheatEnabled;

    // Spells.PAS:1967
    public void SetActiveEnchantments(PlayerEnchantmentTypes value)
    {
        ActiveEnchantments = value;
        StrategicMap strategicMap = GetEngine().StrategicMap;
        for (int i = strategicMap.Players.GetCount() - 1; i >= 1; i--)
        {
            if (i != PlayerIdExtensions.ToOriginal(Player.PlayerIndex))
            {
                strategicMap.Players.GetPlayers(i).MagicControl.UpdateEnemyEnchantments();
            }
        }
    }

    // Spells.PAS:1976
    public void UpdateEnemyEnchantments()
    {
        EnemyEnchantments = PlayerEnchantmentTypes.None;
        StrategicMap strategicMap = GetEngine().StrategicMap;
        for (int i = strategicMap.Players.GetCount() - 1; i >= 1; i--)
        {
            if (i != PlayerIdExtensions.ToOriginal(Player.PlayerIndex))
            {
                if (strategicMap.Players.GetPlayers(i).DiplomaticRelations.GetDiplomaticRelation(Player.PlayerIndex) == DiplomaticRelation.War)
                {
                    PlayerMagicControl magic = strategicMap.Players.GetPlayers(i).MagicControl;
                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentTypes.WaterMastery))
                    {
                        EnemyEnchantments |= PlayerEnchantmentTypes.WaterMastery;
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentTypes.LifeMastery))
                    {
                        EnemyEnchantments |= PlayerEnchantmentTypes.LifeMastery;
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentTypes.DeathMastery))
                    {
                        EnemyEnchantments |= PlayerEnchantmentTypes.DeathMastery;
                    }

                    if (magic.ActiveEnchantments.Contains(PlayerEnchantmentTypes.AirMastery))
                    {
                        EnemyEnchantments |= PlayerEnchantmentTypes.AirMastery;
                    }
                }
            }
        }
    }

    // Spells.PAS:1993
    public int GetManaIncome() => GetNetPowerToMana();

    // Spells.PAS:1995
    public int GetManaUpkeep()
        => Enchantments.Sum(static enchantment => enchantment.Upkeep) + GetEngine().StrategicMap.GlobalMagicControl.GetEnchantmentUpkeep(Player.PlayerIndex);

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
            StrategicMap strategicMap = GetEngine().StrategicMap;
            if (strategicMap.VictoryCondition == StrategicMapVictoryCondition.AccumulateMana && strategicMap.VictoryMana <= Mana)
            {
                strategicMap.GameOver(Player.PlayerIndex, status: GameOverStatus.Won);
            }

            Update();
        }
    }

    // Spells.PAS:2015
    public int GetPower() => PowerSources.Sum(static source => source.Power());

    // Spells.PAS:2023
    public void OutOfMana(int mana)
    {
        while (mana > 0 && Enchantments.GetCount() != 0)
        {
            int i = Enchantments.GetCount();
            int index = i - 1;
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
                PlayerMagicEventLog log = new(null, holder: CreationHolder())
                {
                    Player = Player.PlayerIndex,
                    Mode = PlayerMagicEventLog.EventMode.OutOfMana
                };
                MapCoordinate coordinate25 = Enchantments.GetEnchantments(index).GetLocation();
                int locationX = coordinate25.X;
                int locationY = coordinate25.Y;
                int locationMapLevel = coordinate25.MapLevel;
                log.Location = new MapCoordinate((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
                log.EnchantmentName = Enchantments.GetEnchantments(index).GetName();
                GetEngine().StrategicMap.Players.GetPlayers(Player.PlayerIndex).EventLogbook.AddEvent(log);
                _ = log.Release();
                _ = ExecuteDispelEnchantment(index);
            }
        }

        if (mana > 0)
        {
            GetEngine().StrategicMap.GlobalMagicControl.OutOfMana(PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(Player.PlayerIndex)), mana);
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
        int manaIncome = GetManaIncome();
        int manaUpkeep = GetManaUpkeep();
        int balance = Mana + manaIncome - manaUpkeep;
        return balance >= 0 || GetManaUpkeep() == 0;
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
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Day == 1 && Player.PlayerIndex != PlayerId.Independent && (strategicMap.GameType != GameType.Campaign || Spells.Count == 0))
        {
            Int32List candidates = [];
            for (int kindOrdinal = (int)SpellType.Combat; kindOrdinal <= (int)SpellType.Global; kindOrdinal++)
            {
                SpellType kind = (SpellType)kindOrdinal;
                candidates.Clear();
                for (int i = 0, last = GetEngine().GamePack.SpellControl.GetCount() - 1; i <= last; i++)
                {
                    Spell? spell = GetEngine().GamePack.SpellControl[i];
                    if (spell is not null && Spells.IndexOf(i) == -1 && spell.SpellType == kind && (spell.MagicSphere == MagicSphere.Cosmos || Player.Leader.GetSpherePicks(spell.MagicSphere) > 0) && spell.SpellLevel == 1)
                    {
                        _ = candidates.Add(i);
                    }
                }

                if (candidates.Count > 0)
                {
                    int index = GetEngine().StrategicMap.Random(candidates.Count);
                    int item = candidates[index];
                    _ = Spells.Add(item);
                }
            }
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

                    PlayerMagicEventLog log = new(null, holder: CreationHolder())
                    {
                        Player = Player.PlayerIndex,
                        Mode = PlayerMagicEventLog.EventMode.SpellResearched,
                        SpellId = ResearchSpellId
                    };
                    Player.EventLogbook.AddEvent(log);
                    _ = ExecuteSpellResearched(ResearchSpellId);
                    if (Player.PlayerIndex == GetEngine().StrategicMap.SeatedPlayer && Player.PlayerType == PlayerType.Human)
                    {
                        _ = log.Execute(false, trigger: ExecuteTrigger.Event, done: null);
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
    public void SetSpheres(MagicSpheres value)
    {
        if ((value & (MagicSpheres.Earth | MagicSpheres.Air)) != (MagicSpheres.Earth | MagicSpheres.Air) && (value & (MagicSpheres.Fire | MagicSpheres.Water)) != (MagicSpheres.Fire | MagicSpheres.Water) && (value & (MagicSpheres.Life | MagicSpheres.Death)) != (MagicSpheres.Life | MagicSpheres.Death))
        {
        }
    }

    // Spells.PAS:2146
    public int GetNetPower()
    {
        int result = GetPower();
        return GetEngine().StrategicMap.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentTypes.PowerLeak)
            ? (result + 1) / 2
            : result;
    }

    // Spells.PAS:2151
    public int GetNetPowerToMana()
    {
        int result = GetNetPower() - GetNetPowerToResearch();
        switch (Player.PlayerType)
        {
            case PlayerType.Cpu2:
                return Round(result * 1.25);
            case PlayerType.Cpu3:
                return Round(result * 1.5);
            case PlayerType.Cpu4:
                return Round(result * 1.75);
            case PlayerType.Cpu5:
                return Round((double)result * 2);
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
            return 999;
        }

        return result;
    }

    // Spells.PAS:2172
    public int GetResearchTurnsLeft() => ResearchTurns(GetNetPowerToResearch(), points: ResearchPointsLeft);

    // Spells.PAS:2176
    public bool ValidResearchSpell(Spell spell)
    {
        Span<int> counts = stackalloc int[4];
        if (Player.Leader.GetSpherePicks(spell.MagicSphere) >= spell.SpellLevel && Spells.IndexOf(spell.SpellId) == -1)
        {
            counts.Clear();
            for (int i = Spells.Count - 1; i >= 0; i--)
            {
                int item = Spells[i];
                Spell? known = GetEngine().GamePack.SpellControl[item];
                if (known.SpellLevel > 0 && known.Enabled && spell.MagicSphere == known.MagicSphere)
                {
                    counts[known.SpellLevel - 1] += 1;
                }
            }

            int level = 1;
            while (counts[level - 1] >= 2 && level < 4)
            {
                level += 1;
            }

            if (GetEngine().Demo)
            {
                level = 2;
            }

            return level >= spell.SpellLevel;
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:2199
    public void ListResearchSpells(SpellList list, SpellTypes spellTypes)
    {
        for (int i = 0, last = GetEngine().GamePack.SpellControl.GetCount() - 1; i <= last; i++)
        {
            Spell? spell = GetEngine().GamePack.SpellControl[i];
            if (spell is not null && spellTypes.Contains(spell.SpellType.ToFlag()) && ValidResearchSpell(spell))
            {
                list.Add(spell);
            }
        }
    }

    // Spells.PAS:2211
    public void ListSpells(SpellList list, SpellTypes spellTypes)
    {
        Spell? spell;
        if (CheatEnabled && Player.PlayerType == PlayerType.Human)
        {
            GamePack gamePack = GetEngine().GamePack;
            for (int i = 0, last = gamePack.SpellControl.GetCount() - 1; i <= last; i++)
            {
                spell = gamePack.SpellControl[i];
                if (spell is not null && spell.Enabled && spellTypes.Contains(spell.SpellType.ToFlag()))
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
                spell = GetEngine().GamePack.SpellControl[item];
                if (spell is not null && spell.Enabled && spellTypes.Contains(spell.SpellType.ToFlag()))
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
            NotifyEvent<PlayerMagicControl>? handlers = Changed;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(Changed));
            handlers?.Invoke(this);
        }
    }

    // Spells.PAS:2249
    public void ValidateEnchantmentId(Enchantment enchantment)
    {
        int id;
        // { Preserve a valid unused ID before allocating a new one. }
        if (enchantment.EnchantmentId != 0 && enchantment.EnchantmentId >>> 24 == PlayerIdExtensions.ToOriginal(Player.PlayerIndex))
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

            id = (PlayerIdExtensions.ToOriginal(Player.PlayerIndex) << 24) + _nextEnchantmentId;
            _nextEnchantmentId += 1;
            if (_nextEnchantmentId > 1048575)
            {
                _nextEnchantmentId = 1;
            }

            while (Enchantments.IndexOfId(id) != -1)
            {
                id = (PlayerIdExtensions.ToOriginal(Player.PlayerIndex) << 24) + _nextEnchantmentId;
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
        if (Enchantments.IndexOfChild(enchantment) == -1)
        {
            _ = Enchantments.AddChild(enchantment);
            ValidateEnchantmentId(enchantment);
            Update();
            return true;
        }
        else
        {
            return false;
        }
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
        if (PowerSources.FindChildIndex(source) == -1)
        {
            _ = PowerSources.AddChild(source);
            Update();
            return true;
        }
        else
        {
            return false;
        }
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
        return new(null)
        {
            Holder = CreationHolder(),
            PlayerIndex = PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(Player.PlayerIndex))
        };
    }

    // Spells.PAS:2311
    public bool ExecuteDispelEnchantment(int index) => index < Enchantments.GetCount() && Enchantments.GetEnchantments(index).Dispel();

    // Spells.PAS:2316
    public bool DispelEnchantment(Enchantment? enchantment)
    {
        int index = Enchantments.IndexOfChild(enchantment);
        if (!Player.GetBusy() && index != -1)
        {
            ChangePlayerMagicTokenEvent eventValue = CreateChangeMagicTokenEvent();
            eventValue.Mode = ChangePlayerMagicTokenEventMode.DispelEnchantment;
            eventValue.Value = index;
            GetEngine().StrategicMap.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
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
        if (!Player.GetBusy())
        {
            if (percentage != ResearchPercentage)
            {
                ChangePlayerMagicTokenEvent eventValue = CreateChangeMagicTokenEvent();
                eventValue.Mode = ChangePlayerMagicTokenEventMode.ChangePowerSettings;
                eventValue.Value = percentage;
                GetEngine().StrategicMap.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
                _ = eventValue.Release();
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:2357
    public bool ExecuteResearchSpell(int spellId)
    {
        if (Spells.IndexOf(spellId) == -1 && spellId != ResearchSpellId)
        {
            ResearchSpellId = spellId;
            ResearchPointsLeft = spellId != 0 ? GetEngine().GamePack.SpellControl[spellId].ResearchCost : 0;

            Update();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:2369
    public bool ResearchSpell(int spellId)
    {
        if (!Player.GetBusy())
        {
            ChangePlayerMagicTokenEvent eventValue = CreateChangeMagicTokenEvent();
            eventValue.Mode = ChangePlayerMagicTokenEventMode.ResearchSpell;
            eventValue.Value = spellId;
            GetEngine().StrategicMap.TokenManager.GetTokenControl(Player.PlayerIndex).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Spells.PAS:2384
    public int AIRequestOverrideUpkeep(int priority)
    {
        int result = Enchantments.Sum(enchantment => priority >= enchantment.AIUpkeepPriority ? enchantment.Upkeep : 0);

        // { Original bug: this estimate also counts global enchantments owned by other players. }
        GlobalMagicControl? globalControl = GetEngine().StrategicMap.GlobalMagicControl;
        result += globalControl.Enchantments.Sum(enchantment => priority >= enchantment.GetAIUpkeepPriority() ? enchantment.GetUpkeep() : 0);

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
        RecordList<UpkeepCandidate> list = [];
        for (int i = 0; i < Enchantments.GetCount(); i++)
        {
            Enchantment enchantment = Enchantments.GetEnchantments(i);
            int order = enchantment.AIUpkeepPriority;
            if (order <= priority)
            {
                _ = list.Add(new UpkeepCandidate { Order = order, Upkeep = enchantment.Upkeep, Enchantment = enchantment });
            }
        }
        StrategicMap strategicMap = GetEngine().StrategicMap;
        GlobalMagicControl? global = strategicMap.GlobalMagicControl;
        for (int i = global.Enchantments.GetCount() - 1; i >= 0; i--)
        {
            GlobalEnchantment enchantment = strategicMap.GlobalMagicControl.Enchantments.GetEnchantments(i);
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
                    _ = enchantment.Remove(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(Player.PlayerIndex)));
                    _ = enchantment.Release();
                }
                list.Delete(0);
            }
        }
        return 0;
    }
}
