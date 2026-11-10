using System.Runtime.CompilerServices;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Engine;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Rtl;

namespace Aow.Game.Units
{

    public partial class TArmyID
    {
        public override void ReadWrite(TEStorageStream stream)
        {
            base.ReadWrite(stream);
            stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 10);
            int location = (byte)FX | ((byte)FY << 8) | ((byte)FL << 16) | (F_ArmyID_13 << 24);
            stream.rwInteger(ref location, 11);
            FX = (sbyte)location;
            FY = (sbyte)(location >> 8);
            FL = (sbyte)(location >> 16);
            F_ArmyID_13 = (byte)(location >> 24);
            stream.rwStreamProc(12, FUnitIDs.LoadFromStream, FUnitIDs.SaveToStream);
        }
    }
}

namespace Aow.Game.Combat
{

    public partial class TCombatPredictorUnit
    {
        public void Setup(TAbstractUnit unit, TCombatPredictor predictor, byte side)
        {
            FUnit = unit;
            unit.AddRef();
            FPredictor = predictor;
            FCPUID = predictor.CreateUniqueCPUID();
            FSide = side;
            FInitialHitPoints = unit.GetHitPoints() * 100;
            FEnabled = 1;
            FTargetsInvalid = 1;
            FWallTargetsInvalid = 1;
            FOpponents = predictor.FSides[side == 0 ? 1 : 0];
        }
    }
}

namespace Aow.Game.Magic.Abilities
{

    public partial class TWallCrushingAbility
    {
        // AoWAb.PAS:2081
        public override bool fcExecuteCombatCommand(TCombatObject source, TFastCombatCommand command)
        {
            var target = command.FTarget;
            if (target.GetEnabled())
            {
                var action = Aow.Game.Magic.Abilities.TWallCrushingAbility.CreateDamageCA(source, target, 6, 6, TSet<TDamageType>.Of(TDamageType.dtWall));
                source.FCombat.ExecuteCombatAction(action);
                action.Release();
            }
            return true;
        }
    }
}
