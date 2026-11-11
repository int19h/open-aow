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
    public new const int ClassID = 135949;

    public MapCoordinate FTargetLocation;
    public sbyte FTargetPlayer;

    // CitySpells.pas:175
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTargetLocation.Packed, 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetPlayer), 21);
    }

    // CitySpells.pas:189
    public override bool Validate(Hero source)
    {
        City TargetCity;
        bool Result = base.Validate(source);
        if (Result)
        {
            TargetCity = GetSelectedCity();
            if (TargetCity == null || TargetCity.FPlayer != FTargetPlayer)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // CitySpells.pas:182
    public void SetLocation(sbyte X, sbyte Y, sbyte L)
    {
        FTargetLocation.X = X;
        FTargetLocation.Y = Y;
        FTargetLocation.L = L;
    }

    // CitySpells.pas:204
    public City GetSelectedCity() => (City)AoWHSMap.Current.MapContainer.GetFieldXYL(FTargetLocation.Packed).FindNoneTransparentHS(typeof(City));

    // NetworkE.pas:230

}
