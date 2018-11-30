using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Billow
{
    public static class RectExtra
    {
        public static bool Contains(this RectInt self, RectInt oth)
        {
            return self.Contains(oth.position) && self.xMax >= oth.xMax && self.yMax >= oth.yMax;
        }

        public static Collection<RectInt> sub(this RectInt self, RectInt oth)
        {
            if (!self.Contains(oth))
            {
                return null;
            }

            var res = new Collection<RectInt>();
            if (oth.xMin - self.xMin > 0)
            {
                res.Add(new RectInt(self.xMin, self.yMin, oth.xMin - self.xMin, self.height));
            }

            if (oth.yMin - self.yMin > 0)
            {
                res.Add(new RectInt(oth.xMin, self.yMin, self.xMax - oth.xMin, oth.yMin - self.yMin));
            }

            if (self.xMax - oth.xMax > 0)
            {
                res.Add(new RectInt(oth.xMax, oth.yMin, self.xMax - oth.xMax, self.yMax - oth.yMin));
            }

            if (self.yMax - oth.yMax > 0)
            {
                res.Add(new RectInt(oth.xMin, oth.yMax, oth.width, self.yMax - oth.yMax));
            }

            Debug.Log(string.Format("{0} - {1} =? {2}", self.ToString(), oth.ToString(),
                ToString(res)));
            return res;
        }

        public static RectInt inner(this RectInt self)
        {
            return new RectInt(self.position + Vector2Int.one, self.size - Vector2Int.one * 2);
        }

        public static string ToString<T>(IEnumerable<T> self)
        {
            return string.Format("[{0}]", string.Join(", ", self.Select(r => r.ToString()).ToArray()));
        }

        public static T Choice<T>(this Random random, IEnumerable<T> source)
        {
            var arr = source.ToArray();
            return arr.Length > 0 ? arr[random.Next(arr.Length - 1)] : default(T);
        }

    }
}