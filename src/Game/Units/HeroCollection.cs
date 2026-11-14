using System.Collections;

using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Units;

/// <summary>
/// The heroes of a hero list or a hero library, in the holder's child slots and their stored
/// order. The holder checks each hero that enters, routes messages to its heroes and stores them
/// as references. It does not dispose them. This collection reads them typed. A default value has
/// no holder and throws on use, as a missing list does.
/// </summary>
public readonly struct HeroCollection : IReadOnlyList<Hero>
{
    private readonly EReferenceNode _holder;

    // Each holder admits only heroes (AcceptsChild), so every slot holds a hero or is empty.
    internal HeroCollection(HeroList holder) => _holder = holder;

    internal HeroCollection(HeroLibrary holder) => _holder = holder;

    /// <summary>The list or library that holds the heroes.</summary>
    public EReferenceNode Holder => _holder;

    public int Count => _holder.GetCount();

    // The hero in slot index, read as the original reads the list, without a range check. The
    // holder admits only heroes, so the cast never fails.
    public Hero this[int index] => (Hero)_holder.GetChild(index);

    public IEnumerator<Hero> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Hero.PAS:2407
    public Hero GetHero(int index)
    {
        Hero result;
        if (index >= 0 && index < Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Hero.PAS:2395
    public Hero FindHero(int id)
    {
        int I;
        I = 0;
        Hero result = null;
        while (I != Count && result == null)
        {
            if (id == this[I].UnitId)
            {
                result = this[I];
            }

            I += 1;
        }

        return result;
    }
}
