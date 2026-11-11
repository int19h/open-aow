// Translated from GlobalEnchantmentSpells.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// FRemoved, FRestoreDays, FDispelPlayer, FTurnsActive, FFloodControl
public partial class FloodEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1240
    public new const int ClassID = 135960;

    public bool FRemoved;
    public sbyte FRestoreDays;
    public sbyte FDispelPlayer;
    // alignment
    public int FTurnsActive;
    public FloodControl FFloodControl;

    // GlobalEnchantmentSpells.pas:1234
    public override void Dispose()
    {
        FFloodControl?.Dispose();
        base.Dispose();
    }

    // GlobalEnchantmentSpells.pas:1250
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTurnsActive, 20);
        Stream.rwEObject(FFloodControl, 21);
        Stream.rwBool(ref FRemoved, 22);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDispelPlayer), 23);
        Stream.rwShortintEx(ref FRestoreDays, -1, 24);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("TurnsActive", ref FTurnsActive);
        persister.Object("FloodControl", FFloodControl);
        persister.Bool("Removed", ref FRemoved);
        PersistSupport.UnsignedByte(persister, "DispelPlayer", ref FDispelPlayer);
        persister.SByte("RestoreDays", ref FRestoreDays, -1);
    }

    // GlobalEnchantmentSpells.pas:1224
    public FloodEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.FloodRStr);
        FEnchantmentType = GlobalEnchantmentType.Flood;
        FFloodControl = new FloodControl(null);
        FDispelMana = 100;
        FRestoreDays = -1;
    }

    // GlobalEnchantmentSpells.pas:1302
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001987)
        {
            if (Msg.Payload.Int == FPlayerIndex && !FRemoved)
            {
                NewTurn();
            }

            if (Msg.Payload.Int == FDispelPlayer && FRemoved)
            {
                FRestoreDays = (sbyte)(FRestoreDays - 1);
                if (FRestoreDays <= 0)
                {
                    FFloodControl.Restore();
                    base.Deactivate((byte)FDispelPlayer);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1268
    public override string GetName()
    {
        string Result = base.GetName();
        if (FRemoved)
        {
            Result = Result + " (" + LowerCase(DurationText.FmtDaysLeftStr(FRestoreDays)) + ")";
        }
        else
        {
            if (DaysBeforeActive() != 0)
            {
                var daysBeforeActive = DaysBeforeActive();
                Result = RtlUnit.Format(XinYdaysRStr, [Result, daysBeforeActive]);
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1276
    public override int GetUpkeep()
    {
        int Result;
        if (!FRemoved)
        {
            Result = base.GetUpkeep();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1245
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:1328
    public override bool CanDeactivate(byte Player, ref string Reason)
    {
        bool Result;
        if (!FRemoved)
        {
            Result = base.CanDeactivate(Player, ref Reason);
        }
        else
        {
            Reason = RtlUnit.Format(AbilityRules.TranslateRStr(XAlreadyRemovedRStr), [FName]);
            Result = false;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1338
    public override bool Deactivate(byte Player)
    {
        bool Result;
        if (!FRemoved)
        {
            FDispelPlayer = (sbyte)Player;
            FRemoved = true;
            if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous)
            {
                FRestoreDays = 2;
            }
            else
            {
                FRestoreDays = 1;
            }

            AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.Update();
            AoWHSMap.Current.FGlobalMagicControl.Update();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1320
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        FRemoved = false;
        FFloodControl.Setup();
        return true;
    }

    // GlobalEnchantmentSpells.pas:1260
    public int DaysBeforeActive()
    {
        int Result;
        if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && FTurnsActive < 2)
        {
            Result = 2 - FTurnsActive;
        }
        else
        {
            if (AoWHSMap.Current.FPlayMode == PlayMode.Turn && FTurnsActive == 0)
            {
                Result = 1;
            }
            else
            {
                Result = 0;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1282
    public void NewTurn()
    {
        if (!FRemoved)
        {
            FTurnsActive += 1;
            if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous)
            {
                if (FTurnsActive > 1 && FTurnsActive < 4)
                {
                    FFloodControl.Flood();
                }
                else
                {
                    if (FTurnsActive == 4)
                    {
                        if (AoWHSMap.Current.Random(2) == 1)
                        {
                            FFloodControl.Flood();
                        }
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.FPlayMode == PlayMode.Turn)
                {
                    if (FTurnsActive < 3)
                    {
                        FFloodControl.Flood();
                    }
                    else
                    {
                        if (FTurnsActive == 3)
                        {
                            if (AoWHSMap.Current.Random(2) == 1)
                            {
                                FFloodControl.Flood();
                            }
                        }
                    }
                }
            }
        }
    }

    private static readonly string XAlreadyRemovedRStr = "%s already removed";
    private static readonly string XinYdaysRStr = "%s in %d day(s)";

}
