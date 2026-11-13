// Translated from PoisonP.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class PoisonPlantSpell : GlobalTargetSpell
{
    public PoisonPlantSpell(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_poisonPlantsText);
        SpellId = 12;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Earth;
        AboveGroundOnly = false;
    }

    // PoisonP.pas:371

    // PoisonP.pas:405
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; I <= last; I++)
        {
            targetSelection.HexNumbers.Add(0);
        }
    }

    // PoisonP.pas:414
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        PoisonPlantSpellTokenEvent eventValue;
        eventValue = new PoisonPlantSpellTokenEvent(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    // PoisonP.pas:390
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        ArmyHexSprite armyHexSprite;
        bool result = base.ValidTargetSelectionMapField(field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)field.TerrainType);
        // { Original precedence also accepts these overlays without the terrain test. }
        result = (result && field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)field.OverlayType);
        result = result && field.FindNoneTransparentHexSprite(typeof(Structure)) == null && field.GetArmyHexSprite() == null;
        if (result)
        {
            armyHexSprite = field.GetArmyHexSprite();
            if (armyHexSprite != null)
            {
                result = !armyHexSprite.VisibleForSeatedPlayer();
            }
        }

        return result;
    }

    // PoisonP.pas:382
    public override bool ValidTargetMapField(MapField field)
    {
        bool result = base.ValidTargetMapField(field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)field.TerrainType);
        // { Original precedence also accepts these overlays without the terrain test. }
        result = (result && field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)field.OverlayType);
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) == null && field.GetArmyHexSprite() == null;
    }

    private static readonly string s_poisonPlantsText = "Poison Plants";

    // PoisonP.pas:66

    // PoisonP.pas:66

}
