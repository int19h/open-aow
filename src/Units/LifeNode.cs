// Translated from LifeNode.pas.
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

namespace Aow.Units.LifeNode
{
    // { TLifeNode (instance size 76, no added fields, VMT 557C046C) }
    public partial class TLifeNode : Aow.Units.PrdPlace.TProductionPlace
    {
        // LifeNode.pas:40
        public override Aow.Units.AoWE.TMagicSphere GetMagicSphere()
        {
            Aow.Units.AoWE.TMagicSphere Result = default;
            Result = Aow.Units.AoWE.TMagicSphere.msLife;
            return Result;
        }

        public new partial class Meta : Aow.Units.PrdPlace.TProductionPlace.Meta
        {
            // LifeNode.pas:37
            public override int ClassID()
            {
                int Result = default;
                Result = 131953;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.LifeNode.TLifeNode => (Meta)MetaRegistry.Of(typeof(T));
            // PrdPlace.pas:51
            public new Aow.Units.LifeNode.TLifeNode Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.LifeNode.TLifeNode o = (Aow.Units.LifeNode.TLifeNode)NewInstance();
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

    // { TLifeNodeResource (instance size 120, no added fields, VMT 557C071C) }
    public partial class TLifeNodeResource : Aow.Units.PrdPlace.TProductionPlaceResource
    {
        // LifeNode.pas:43
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FRazeDefenderBudget = 200;
            FTerrainBound = false;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // LifeNode.pas:58
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131953;
            return Result;
        }

        // LifeNode.pas:27
        public static new Aow.Units.LifeNode.TLifeNodeResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.LifeNode.TLifeNodeResource o = new Aow.Units.LifeNode.TLifeNodeResource();
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
            // LifeNode.pas:55
            public override int ClassID()
            {
                int Result = default;
                Result = 131954;
                return Result;
            }

            // LifeNode.pas:52
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LifeNodeRStr);
                return Result;
            }

            // LifeNode.pas:27
            public new Aow.Units.LifeNode.TLifeNodeResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.LifeNode.TLifeNodeResource o = (Aow.Units.LifeNode.TLifeNodeResource)NewInstance();
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
                where T : Aow.Units.LifeNode.TLifeNodeResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class LifeNode
    {
        // LifeNode.pas:61
        public static void RegisterLifeNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.LifeNode.TLifeNode>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.LifeNode.TLifeNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
