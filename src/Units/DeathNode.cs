// Translated from DeathNode.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.PrdPlace;
using static Aow.Units.PrdPlace.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.DeathNode
{
    // { TDeathNode (instance size 76, VMT 557BFFA4) }
    public partial class TDeathNode : Aow.Units.PrdPlace.TProductionPlace
    {
        // DeathNode.pas:40
        public override Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msDeath;
            return Result;
        }

        public new partial class Meta : Aow.Units.PrdPlace.TProductionPlace.Meta
        {
            // DeathNode.pas:37
            public override int ClassID()
            {
                int Result = default;
                Result = 132270;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.DeathNode.TDeathNode => (Meta)MetaRegistry.Of(typeof(T));
            // PrdPlace.pas:51
            public new Aow.Units.DeathNode.TDeathNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.DeathNode.TDeathNode o = (Aow.Units.DeathNode.TDeathNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TDeathNodeResource (instance size 120, VMT 557C0258) }
    public partial class TDeathNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        // DeathNode.pas:43
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FRazeDefenderBudget = 200;
            FRebuildGold = 25;
            FRebuildDays = 2;
            FTerrainBound = false;
        }

        // DeathNode.pas:58
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132270;
            return Result;
        }

        // DeathNode.pas:27
        public static new Aow.Units.DeathNode.TDeathNodeResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.DeathNode.TDeathNodeResource o = new Aow.Units.DeathNode.TDeathNodeResource();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.PrdPlace.TProductionPlaceResource.Meta
        {
            // DeathNode.pas:55
            public override int ClassID()
            {
                int Result = default;
                Result = 132271;
                return Result;
            }

            // DeathNode.pas:52
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathNodeRStr);
                return Result;
            }

            // DeathNode.pas:27
            public new Aow.Units.DeathNode.TDeathNodeResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.DeathNode.TDeathNodeResource o = (Aow.Units.DeathNode.TDeathNodeResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.DeathNode.TDeathNodeResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class DeathNode
    {
        // DeathNode.pas:61
        public static void RegisterDeathNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.DeathNode.TDeathNode>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.DeathNode.TDeathNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
