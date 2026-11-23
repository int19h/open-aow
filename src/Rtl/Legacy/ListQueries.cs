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
        /// A startIndex at or past the count finds nothing, as the loop did.</summary>
        public int FindIndex(int startIndex, Func<T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
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

        /// <summary>The first matching index from startIndex on, walking backwards, or -1.</summary>
        public int FindLastIndex(int startIndex, Func<T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
            for (int i = startIndex; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>The last matching element, walking backwards, or the default.</summary>
        public T? FindLast(Func<T, bool> match)
        {
            ArgumentNullException.ThrowIfNull(match);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    return list[i];
                }
            }

            return default;
        }
    }
}
