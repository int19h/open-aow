// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// Removed, RestoreDays, DispelPlayer, TurnsActive, FloodControl
public partial class FloodEnchantment : GlobalEnchantment
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
    public FloodEnchantment(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), GameText.FloodText);
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
                    base.Deactivate((byte)DispelPlayer);
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
            result = result + " (" + LowerCase(DurationText.FormatDaysLeftString(GetEngine(), days: RestoreDays)) + ")";
        }
        else
        {
            if (DaysBeforeActive() != 0)
            {
                var daysBeforeActive = DaysBeforeActive();
                result = RtlUnit.Format(s_dueInDaysFormat, [result, daysBeforeActive]);
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1276
    public override int GetUpkeep()
    {
        int result;
        if (!Removed)
        {
            result = base.GetUpkeep();
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1245
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:1328
    public override bool CanDeactivate(byte player, ref string reason)
    {
        bool result;
        if (!Removed)
        {
            result = base.CanDeactivate(player, ref reason);
        }
        else
        {
            reason = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_alreadyRemovedFormat), [Name]);
            result = false;
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1338
    public override bool Deactivate(byte player)
    {
        bool result;
        if (!Removed)
        {
            DispelPlayer = (sbyte)player;
            Removed = true;
            if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
            {
                RestoreDays = 2;
            }
            else
            {
                RestoreDays = 1;
            }

            StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.Update();
            StrategicMap.Current.GlobalMagicControl.Update();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1320
    public override bool Activate(sbyte player)
    {
        base.Activate(player);
        Removed = false;
        FloodControl.Setup();
        return true;
    }

    // GlobalEnchantmentSpells.pas:1260
    public int DaysBeforeActive()
    {
        int result;
        if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && TurnsActive < 2)
        {
            result = 2 - TurnsActive;
        }
        else
        {
            if (StrategicMap.Current.PlayMode == PlayMode.Turn && TurnsActive == 0)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1282
    public void NewTurn()
    {
        if (!Removed)
        {
            TurnsActive += 1;
            if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
            {
                if (TurnsActive > 1 && TurnsActive < 4)
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
