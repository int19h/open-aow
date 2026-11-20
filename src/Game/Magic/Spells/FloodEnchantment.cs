#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Magic.Spells;

// Removed, RestoreDays, DispelPlayer, TurnsActive, FloodControl
public class FloodEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1240
    public new const int ClassId = 135960;

    public bool Removed { get; set; }
    public sbyte RestoreDays { get; set; }
    public sbyte DispelPlayer { get; set; }
    // alignment
    public int TurnsActive { get; set; }

    public FloodControl FloodControl { get; }

    // GlobalEnchantmentSpells.pas:1234
    public override void Dispose()
    {
        FloodControl?.Dispose();
        base.Dispose();
    }

    // GlobalEnchantmentSpells.pas:1250
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        TurnsActive = persister.Int32("TurnsActive", value: TurnsActive);
        persister.Object("FloodControl", objectValue: FloodControl);
        Removed = persister.Boolean("Removed", value: Removed);
        DispelPlayer = PersistSupport.UnsignedByte(persister, name: "DispelPlayer", value: DispelPlayer);
        RestoreDays = persister.SByte("RestoreDays", value: RestoreDays, fallback: -1);
    }

    // GlobalEnchantmentSpells.pas:1224
    public FloodEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FloodText);
        EnchantmentType = GlobalEnchantmentType.Flood;
        FloodControl = new FloodControl(null) { Holder = CreationHolder() };
        DispelMana = 100;
        RestoreDays = -1;
    }

    // GlobalEnchantmentSpells.pas:1302
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001987)
        {
            if (message.Payload.Int == PlayerIndex && !Removed)
            {
                NewTurn();
            }

            if (message.Payload.Int == DispelPlayer && Removed)
            {
                RestoreDays = (sbyte)(RestoreDays - 1);
                if (RestoreDays <= 0)
                {
                    FloodControl.Restore();
                    _ = base.Deactivate((byte)DispelPlayer);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1268
    public override string GetName()
    {
        string result = base.GetName();
        if (Removed)
        {
            return $"{result} ({LowerCase(DurationText.FormatDaysLeftString(GetEngine(), days: RestoreDays))})";
        }
        else
        {
            if (DaysBeforeActive() != 0)
            {
                int daysBeforeActive = DaysBeforeActive();
                return Format(s_dueInDaysFormat, args: [result, daysBeforeActive]);
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1276
    public override int GetUpkeep() => !Removed ? base.GetUpkeep() : 0;

    // GlobalEnchantmentSpells.pas:1245
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:1328
    public override bool CanDeactivate(byte player, ref string reason)
    {
        if (!Removed)
        {
            return base.CanDeactivate(player, ref reason);
        }
        else
        {
            reason = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_alreadyRemovedFormat), args: [Name]);
            return false;
        }
    }

    // GlobalEnchantmentSpells.pas:1338
    public override bool Deactivate(byte player)
    {
        if (!Removed)
        {
            DispelPlayer = (sbyte)player;
            Removed = true;
            RestoreDays = StrategicMap.Current.PlayMode == PlayMode.Simultaneous ? (sbyte)2 : (sbyte)1;

            StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.Update();
            StrategicMap.Current.GlobalMagicControl.Update();
            return true;
        }
        else
        {
            return false;
        }
    }

    // GlobalEnchantmentSpells.pas:1320
    public override bool Activate(sbyte player)
    {
        _ = base.Activate(player);
        Removed = false;
        FloodControl.Setup();
        return true;
    }

    // GlobalEnchantmentSpells.pas:1260
    public int DaysBeforeActive()
    {
        return StrategicMap.Current.PlayMode == PlayMode.Simultaneous && TurnsActive < 2
            ? 2 - TurnsActive
            : StrategicMap.Current.PlayMode == PlayMode.Turn && TurnsActive == 0 ? 1 : 0;
    }

    // GlobalEnchantmentSpells.pas:1282
    public void NewTurn()
    {
        if (!Removed)
        {
            TurnsActive += 1;
            if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
            {
                if (TurnsActive is > 1 and < 4)
                {
                    FloodControl.Flood();
                }
                else
                {
                    if (TurnsActive == 4)
                    {
                        if (StrategicMap.Current.Random(2) == 1)
                        {
                            FloodControl.Flood();
                        }
                    }
                }
            }
            else
            {
                if (StrategicMap.Current.PlayMode == PlayMode.Turn)
                {
                    if (TurnsActive < 3)
                    {
                        FloodControl.Flood();
                    }
                    else
                    {
                        if (TurnsActive == 3)
                        {
                            if (StrategicMap.Current.Random(2) == 1)
                            {
                                FloodControl.Flood();
                            }
                        }
                    }
                }
            }
        }
    }

    private static readonly string s_alreadyRemovedFormat = "%s already removed";
    private static readonly string s_dueInDaysFormat = "%s in %d day(s)";

}
