// Translated from CitySpells.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

// { ot=1 }
// packed target coordinates. target owner
public partial class CityTargetSpellTokenEvent : SpellTokenEvent
{
    public CityTargetSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CitySpells.pas:170
    public new const int ClassId = 135949;

    private MapCoordinate _targetLocation;
    public sbyte TargetPlayer;

    // CitySpells.pas:175
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref _targetLocation.Packed, 20);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref TargetPlayer), 21);
    }

    // CitySpells.pas:189
    public override bool Validate(Hero source)
    {
        City targetCity;
        bool result = base.Validate(source);
        if (result)
        {
            targetCity = GetSelectedCity();
            if (targetCity == null || targetCity.Player != TargetPlayer)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    // CitySpells.pas:182
    public void SetLocation(sbyte X, sbyte Y, sbyte L)
    {
        _targetLocation.X = X;
        _targetLocation.Y = Y;
        _targetLocation.MapLevel = L;
    }

    // CitySpells.pas:204
    public City GetSelectedCity() => (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_targetLocation).FindNoneTransparentHexSprite(typeof(City));

    // NetworkE.pas:230

}
