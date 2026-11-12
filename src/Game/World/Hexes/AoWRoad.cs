// Translated from AoWRoad.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class AoWRoad : Road
{
    public AoWRoad(ECustomNode owner) : base(owner)
    { }

    // AoWRoad.pas:35
    public override void UpdateTransitions()
    {
        byte oldTransitions;
        int hexX = default;
        int hexY = default;
        MapFieldMessage message = default;
        MapLevel level;
        MapField mapField;
        bool hasRoad;
        oldTransitions = Transitions;
        level = Owner.Owner;
        Transitions = 0;
        message.MessageId = 4401;
        message.Param[0] = Owner;
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
        {
            HexGeometry.HexNumberToHex(hexNumber, ref hexX, ref hexY);
            HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, hexX, hexY, ref hexX, ref hexY);
            if (hexX >= 0 && hexY >= 0 && level.Width > hexX && level.Height > hexY)
            {
                mapField = level.GetField(hexX, hexY);
                if (mapField.FindOwnedHexSprite(typeof(AbstractRoad)) == null)
                {
                    // { Original message stores its response in Param[1], rather than Result. }
                    message.Param[1] = 0;
                    mapField.SendMapFieldMessage(ref message);
                    hasRoad = message.Param[1].Int != 0;
                }
                else
                {
                    hasRoad = true;
                }

                if (hasRoad)
                {
                    Transitions = (byte)(Transitions + (1 << (hexNumber - 1)));
                }
            }
        }

        if ((Transitions & 34) == 34)
        {
            Transitions = (byte)((Transitions & 221) + 128);
        }

        if ((Transitions & 20) == 20)
        {
            Transitions = (byte)((Transitions & 235) + 64);
        }

        if (oldTransitions != Transitions)
        {
            AoWHSMap.Current.InvalidateStaticScene(Owner.HexX, Owner.HexY, Owner.HexLevel);
        }
    }
}
