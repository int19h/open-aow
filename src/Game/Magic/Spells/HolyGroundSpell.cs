// Translated from HolyG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class HolyGroundSpell : GlobalTargetSpell
{
    public HolyGroundSpell(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_holyGroundsText);
        SpellId = 13;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Life;
        AboveGroundOnly = false;
    }

    // HolyG.pas:339

    // HolyG.pas:375
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.HexNumbers.Clear();
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; I <= last; I++)
        {
            targetSelection.HexNumbers.Add(0);
        }
    }

    // HolyG.pas:384
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        HolyGroundSpellTokenEvent eventValue;
        eventValue = new HolyGroundSpellTokenEvent(null);
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    // HolyG.pas:360
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        ArmyHexSprite army;
        bool result = base.ValidTargetMapField(field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)field.TerrainType);
        // { Original overlay test allows these overlays even when the terrain check failed. }
        result = (result && field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)field.OverlayType);
        result = result && field.FindNoneTransparentHexSprite(typeof(Structure)) == null;
        if (result)
        {
            army = ((AoWMapField)field).GetArmyHexSprite();
            if (army != null)
            {
                result = !army.VisibleForSeatedPlayer();
            }
        }

        return result;
    }

    // HolyG.pas:350
    public override bool ValidTargetMapField(MapField field)
    {
        bool result = base.ValidTargetMapField(field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)field.TerrainType);
        // { Original overlay test allows these overlays even when the terrain check failed. }
        result = (result && field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)field.OverlayType);
        return result && field.FindNoneTransparentHexSprite(typeof(Structure)) == null && ((AoWMapField)field).GetArmyHexSprite() == null;
    }

    private static readonly string s_holyGroundsText = "Holy Woods";

    // HolyG.pas:63

    // HolyG.pas:63

}
