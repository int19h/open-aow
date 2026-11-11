// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// (mode, location, spell ID, value, enchantment name)
public partial class PlayerMagicEventLog : EventLog
{
    // Spells.PAS:1847
    // mode, location, spell ID, value, enchantment name
    public PlayerMagicEventLog(ECustomNode owner) : base(owner)
    {
        FLocation.Packed = 16777215;
    }

    public new const int ClassID = 131824;

    public EventMode FMode;
    public MapCoordinate FLocation;
    public int FSpellID;
    public int FValue;
    public string FEnchantmentName = "";

    // Spells.PAS:1854
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FMode, 1), 1, 20);
        Stream.rwInteger(ref FSpellID, 21);
        Stream.rwInteger(ref FValue, 22);
        Stream.rwString(ref FEnchantmentName, 23);
        Stream.rwInteger(ref FLocation.Packed, 24);
    }

    // Spells.PAS:1892
    public override string GetText()
    {
        string Result = "";
        switch (FMode)
        {
            case EventMode.SpellResearched:
                var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                Result = AbilityRules.Format(AbilityRules.TranslateRStr(P4XResearchedRStr), [spell.FSpellName]);
                break;
            case EventMode.OutOfMana:
                if (AoWHSMap.Current.FSeatedPlayer == FPlayer)
                {
                    Result = AbilityRules.Format(AbilityRules.TranslateRStr(P4XRemovedOutOfManaRStr), [FEnchantmentName]);
                }
                else
                {
                    Result = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.P4XRemovedRStr), [FEnchantmentName]);
                }

                break;
        }

        return Result;
    }

    // Spells.PAS:1874
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        bool Show;
        switch (FMode)
        {
            case EventMode.OutOfMana:
                Show = Trigger == ExecuteTrigger.User;
                break;
            case EventMode.SpellResearched:
                if (Trigger != ExecuteTrigger.User)
                {
                    if (AoWHSMap.Current.FPlayMode == PlayMode.Turn)
                    {
                        Show = GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.SpellResearched);
                    }
                    else
                    {
                        Show = GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.SpellResearched);
                    }
                }
                else
                {
                    Show = true;
                }

                break;
            case EventMode.StartResearch:
                Show = true;
                break;
            default:
                Show = false;
                break;
        }

        if (Show)
        {
            Result = base.Execute(Mode, Trigger, Done);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Spells.PAS:1863
    public override void ViewLocation()
    {
        sbyte X;
        X = FLocation.X;
        if (X != -1)
        {
            AoWHSMap.Current.CenterViewTo(X, FLocation.Y, FLocation.L);
            AoWHSMap.Current.PlayCenterAnimation(FLocation.X, FLocation.Y, FLocation.L);
        }
    }

    private static readonly string P4XResearchedRStr = "%s researched";
    private static readonly string P4XRemovedOutOfManaRStr = "%s removed (out of mana)";

    public enum EventMode : byte
    {
        SpellResearched = 0, OutOfMana = 1, StartResearch = 2
    }

}
