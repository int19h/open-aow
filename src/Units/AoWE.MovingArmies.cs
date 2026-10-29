using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public partial class TMoveArmyTE
{
    public TArmyHS GetNoneMovingArmyHS(TMapField field)
    {
        for (int i = field.Count - 1; i >= 0; i--)
        {
            var sprite = field.HSList[i];
            if (sprite.EClass.ClassID == 0x20217 && sprite.Owner == field)
            {
                var army = (TArmyHS)sprite;
                if (!army.FArmy.FMode.Contains(TArmyModeElement.amMoving)) return army;
            }
        }
        return null;
    }
}
