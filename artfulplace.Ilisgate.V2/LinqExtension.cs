using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artfulplace.Ilisgate.V2.Extensions
{
    /// <summary>
    /// IEnumerable に対する拡張メソッドを提供します。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// コレクションをランダムに並び替えます。
        /// </summary>
        /// <typeparam name="T">Type Parameter</typeparam>
        /// <param name="source">並び替えるコレクション</param>
        /// <returns>並び替えた後のコレクション</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

    }
}
