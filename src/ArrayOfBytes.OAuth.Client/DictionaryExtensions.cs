﻿using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace ArrayOfBytes.OAuth.Client
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Merge everything in dictionary "b" into dictionary "a"
        /// </summary>
        /// <param name="a">Destination</param>
        /// <param name="b">Source</param>
        public static void Merge(this Dictionary<string, StringValues> a, Dictionary<string, StringValues> b)
        {
            foreach (var key in b.Keys)
            {
                if (!a.ContainsKey(key))
                {
                    a.Add(key, b[key]);
                }
                else
                {
                    var values = a[key];
                    var mergedValues = values.Concat(b[key]);
                    a[key] = new StringValues(mergedValues.ToArray());
                }
            }
        }
    }
}
