#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Storms.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

// player
public class Storm(CustomNode owner) : Animation(owner)
{
    public sbyte Player { get; private set; }

    // Storms.pas:34
    public virtual StormDamageInfo GetTargetDamageInfo(sbyte player, int X, int Y, int L, StormDamageInfo info) => throw AbstractErrorException.Create("Abstract Error");

    // Storms.pas:236
    public virtual void UpdateTargetSelection(TargetSelectionControl selection)
    {
        selection.SetShowHandler(ShowTargetSelection);
        selection.TargetMode |= TargetSelectionModes.OwnerDraw;
    }

    // Storms.pas:209
    public virtual void InitializeStorm(sbyte player, int X, int Y, int L)
    {
        Player = player;
        AnimationLevel = 28;
        StrategicMap.Current.LockExecuteEventLog();
    }

    // Storms.pas:216
    public virtual bool UpdateStorm() => false;

    // Storms.pas:221
    public virtual void FinalizeStorm() => StrategicMap.Current.UnlockExecuteEventLog();

    // Storms.pas:226
    public void ShowTargetSelection(TargetSelectionControl selection, int hexNumber, int xs, int ys, in LockedSurface lockedSurface)
    {
        hexNumber = HexGeometry.HexNumberToRadius(hexNumber);
        int intensity = (Math.Abs(((StrategicMap.Current.Frame + hexNumber) % 9) - 4) * 20) + 20;
        BlendTable lookupTable = GraphicsState.IntensityBlendTable.GetLookupTables(intensity);
        selection.ImageLibrary[0].ShowBlended(xs - 8, ys, lockedSurface, table: lookupTable);
    }

    // Storms.pas:242
    public bool GetBestTarget(sbyte player, int X, int Y, int L, int range, out MapCoordinate target, out StormDamageInfo info)
    {
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[L];
        target = MapCoordinate.None;
        bool result = false;
        info = default;
        StormDamageInfo candidate = default;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        while (walk.Radius <= range)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                ArmyHexSprite armyHexSprite = level.Field[walk.X, walk.Y].GetArmyHexSprite();
                if (armyHexSprite is not null && armyHexSprite.Army.Player != 0 && AbilityRules.ValidAttackRelation(player, independentRelation: 0, targetPlayer: armyHexSprite.Army.Player, targetRace: 0))
                {
                    candidate = GetTargetDamageInfo(player, walk.X.TruncateToSByte(), walk.Y.TruncateToSByte(), L: L.TruncateToSByte(), info: candidate);
                    if (info.EnemyDamage - info.FriendlyDamage < candidate.EnemyDamage - candidate.FriendlyDamage)
                    {
                        info = candidate;
                        target = new MapCoordinate((sbyte)walk.X, (sbyte)walk.Y, (sbyte)L);
                        result = true;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }

    // Storms.pas:39
    public virtual string Name() => throw AbstractErrorException.Create("Abstract Error");

}
