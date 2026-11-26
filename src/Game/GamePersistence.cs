using Aow.Engine;
using Aow.Formats.Tree;
using Aow.Game.Draw.Images;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game;

/// <summary>
/// The persistence helpers of game records: players, map coordinates, unit index lists, image
/// sequences, terrain lists and image libraries. The engine's helpers are in
/// <see cref="PersistSupport"/>.
/// </summary>
public static class GamePersistence
{
    public static PlayerId Player(this IPersister persister, string name, PlayerId value, IdWidth width, int fallback = 0)
        => persister.Id(name, value: value, width, fallback: fallback);

    public static PlayerId? OptionalPlayer(this IPersister persister, string name, PlayerId? value, IdWidth width, int? fallback = null)
        => persister.OptionalId(name, value: value, width, fallback: fallback);

    /// <summary>
    /// A map coordinate stored as X, Y and MapLevel. Byte width is an unsigned byte.
    /// Int32 width is a signed 32-bit integer, and a loaded value outside -128 to 127 is refused.
    /// </summary>
    public static MapCoordinate Coordinate(this IPersister persister, string name, MapCoordinate value, CoordinateWidth width)
    {
        MapCoordinate loaded = value;
        persister.Record(name, body =>
        {
            if (width == CoordinateWidth.Byte)
            {
                int x = body.UnsignedByte("X", value.X);
                int y = body.UnsignedByte("Y", value.Y);
                int level = body.UnsignedByte("MapLevel", value.MapLevel);
                if (body.Loading)
                {
                    loaded = new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)level);
                }
            }
            else
            {
                int x = body.Int32("X", value.X);
                int y = body.Int32("Y", value.Y);
                int level = body.Int32("MapLevel", value.MapLevel);
                if (body.Loading)
                {
                    loaded = new MapCoordinate((sbyte)InRange(name, component: "X", stored: x), (sbyte)InRange(name, component: "Y", stored: y), (sbyte)InRange(name, component: "MapLevel", stored: level));
                }
            }
        });
        return persister.Loading ? loaded : value;
    }

    private static int InRange(string name, string component, int stored)
    {
        return stored is < -128 or > 127
            ? throw new InvalidOperationException(string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"{name}.{component} is {stored}, outside -128 to 127."))
            : stored.TruncateToSByte();
    }

    /// <summary>
    /// A TUnitIndexList stream. The image is the TIntegerList image, and the constructor's default
    /// is -1. A stored default that differs from it is kept.
    /// </summary>
    public static void UnitIndexList(this IPersister persister, string name, UnitIndexList list)
        => persister.Int32List(name, list);

    /// <summary>
    /// A TUnitIndexListCollection stream: the flat TUnitIndexList of its groups. The document
    /// cuts it into groups of the schema's size.
    /// </summary>
    public static void UnitIndexListCollection(this IPersister persister, string name, UnitIndexListCollection collection)
        => persister.Int32List(name, list: collection.Units);

    /// <summary>
    /// A TImageSequenceList stream (Delphi <c>rwStreamProc</c> with <c>LoadFromStream</c> and
    /// <c>SaveToStream</c>). The save reduces the count first, as <c>SaveToStream</c> does. A
    /// present member replaces the sequences, and each frame-table layer draws its image index
    /// from <c>Random</c> in entry and layer order, at the point of the load where
    /// <c>LoadFromStream</c> draws it. An absent member leaves the list as it was. The translate
    /// stream visits no payload (the original stream procedure does nothing there), so the
    /// list is not reduced either.
    /// </summary>
    public static void ImageSequences(this IPersister persister, string name, ImageSequenceList? list)
    {
        if (persister.Translating())
        {
            return;
        }

        if (!persister.Loading)
        {
            persister.ImageSequences(name, entries: list!.SaveLayers());
            return;
        }
        if (!persister.Has(name))
        {
            return;
        }

        List<IReadOnlyList<SequenceLayer>?> entries = [];
        persister.ImageSequences(name, entries: entries);
        PersistSupport.LoadPayload(() => list!.LoadLayers(entries));
    }

    /// <summary>
    /// A TTerrainList stream. The save packs the trailing pairs of none first, as
    /// <c>SaveToStream</c> does. A present member replaces the pairs, an absent member leaves them.
    /// The translate stream visits no payload, so it packs nothing.
    /// </summary>
    public static void TerrainList(this IPersister persister, string name, TerrainList list)
    {
        if (persister.Translating())
        {
            return;
        }

        List<(sbyte First, sbyte Second)> pairs = [];
        if (!persister.Loading)
        {
            list.Pack();
            for (int i = 0; i < list.Count; i++)
            {
                pairs.Add(list.GetStoredPair(i));
            }

            persister.SBytePairs(name, pairs: pairs);
            return;
        }
        if (!persister.Has(name))
        {
            return;
        }

        persister.SBytePairs(name, pairs: pairs);
        PersistSupport.LoadPayload(() =>
        {
            list.Count = pairs.Count;
            for (int i = 0; i < pairs.Count; i++)
            {
                list.SetTerrain(i, value: pairs[i].First);
                list.SetOverlay(i, value: pairs[i].Second);
            }
        });
    }

    /// <summary>
    /// An ILB library stream. The save writes the library as <c>SaveToStream</c> writes it, and a
    /// present member loads through <c>LoadFromStream</c>. An absent member leaves the library.
    /// The translate stream visits no payload.
    /// </summary>
    public static void ImageLibrary(this IPersister persister, string name, CustomImageLibrary library)
    {
        if (persister.Translating())
        {
            return;
        }

        if (!persister.Loading)
        {
            using MemoryStream output = new();
            library.SaveToStream(output);
            _ = persister.Blob(name, data: output.ToArray());
            return;
        }
        if (persister.Blob(name, data: []) is not { } stored)
        {
            return;
        }

        using MemoryStream input = new(stored, writable: false);
        PersistSupport.LoadPayload(() => library.LoadFromStream(input));
    }
}
