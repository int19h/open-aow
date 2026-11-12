// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

// player
public partial class Storm : Animation
{
    public Storm(ECustomNode owner) : base(owner)
    { }

    public sbyte Player { get; private set; }

    // Storms.pas:34
    public virtual void GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo info)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Storms.pas:236
    public virtual void UpdateTargetSelection(TargetSelectionControl selection)
    {
        selection.SetShowHandler(ShowTargetSelection);
        selection.TargetMode = selection.TargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.OwnerDraw);
    }

    // Storms.pas:209
    public virtual void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Player = player;
        AnimationLevel = 28;
        StrategicMap.Current.LockExecuteEventLog();
    }

    // Storms.pas:216
    public virtual bool UpdateStorm() => false;

    // Storms.pas:221
    public virtual void FinalizeStorm()
    {
        StrategicMap.Current.UnlockExecuteEventLog();
    }

    // Storms.pas:226
    public void ShowTargetSelection(TargetSelectionControl selection, int hexNumber, int xs, int ys, in LockedSurface lockedSurface)
    {
        int intensity;
        hexNumber = HexGeometry.HexNumberToRadius(hexNumber);
        intensity = Math.Abs((StrategicMap.Current.Frame + hexNumber) % 9 - 4) * 20 + 20;
        var lookupTable = GraphicsState.IntensityBlendTable.GetLookupTables(intensity);
        selection.ImageLibrary[0].ShowBlended(xs - 8, ys, lockedSurface, lookupTable);
    }

    // Storms.pas:242
    public bool GetBestTarget(sbyte player, int X, int Y, int L, int range, ref MapCoordinate target, out StormDamageInfo info)
    {
        StormDamageInfo candidate;
        HexNumberWalkData walk = default;
        MapLevel level;
        ArmyHexSprite armyHexSprite;
        level = StrategicMap.Current.MapContainer.MapLevels[L];
        target.Packed = 16777215;
        bool result = false;
        info = default;
        candidate = default;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (walk.Radius <= range)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                armyHexSprite = level.Field[walk.X, walk.Y].GetArmyHexSprite();
                if (armyHexSprite != null && armyHexSprite.Army.Player != 0 && AbilityRules.ValidAttackRelation(player, 0, armyHexSprite.Army.Player, 0))
                {
                    GetTargetDamageInfo(player, (sbyte)walk.X, (sbyte)walk.Y, (sbyte)L, ref candidate);
                    if (info.EnemyDamage - info.FriendlyDamage < candidate.EnemyDamage - candidate.FriendlyDamage)
                    {
                        info = candidate;
                        target = HexGeometry.PackMapCoordinate((sbyte)walk.X, (sbyte)walk.Y, (sbyte)L);
                        result = true;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }

    // Storms.pas:39
    public virtual string Name()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

}
