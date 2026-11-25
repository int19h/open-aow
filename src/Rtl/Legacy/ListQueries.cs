namespace Aow.Rtl;

/// <summary>Index searches on a read-only list that walk the indexer in the direction the
/// Delphi loop walked and stop at the first hit. A backward loop keeps its direction, so
/// <c>Enumerable.LastOrDefault</c> with a predicate is not the replacement.</summary>
public static class ListQueries
{
    extension<T>(IReadOnlyList<T> list)
    {
        /// <summary>The first index whose element matches, walking forwards, or -1.</summary>
        public int FindIndex(Func<T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>The first matching index from startIndex on, walking forwards, or -1.
        /// A startIndex equal to the count finds nothing. A startIndex below zero or
        /// past the count throws, as List's ranged FindIndex does.</summary>
        public int FindIndex(int startIndex, Func<T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
            if (startIndex < 0 || startIndex > list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            for (int i = startIndex; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>The last index whose element matches, walking backwards from the end,
        /// or -1.</summary>
        public int FindLastIndex(Func<T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>The last index whose element matches, walking backwards from the end,
        /// or -1. The state is an argument of the predicate, so a static predicate does not
        /// allocate a closure.</summary>
        public int FindLastIndex<TState>(TState state, Func<TState, T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(state, list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>The first matching index from startIndex on, walking backwards, or -1.
        /// A startIndex below zero or at or past the count throws, as List's ranged
        /// FindLastIndex does.</summary>
        public int FindLastIndex(int startIndex, Func<T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            for (int i = startIndex; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>The last matching element, walking backwards, or the default. A hit
        /// reads its element once.</summary>
        public T? FindLast(Func<T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                T element = list[i];
                if (match(element))
                {
                    return element;
                }
            }

            return default;
        }
    }
}
