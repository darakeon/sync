using System;
using System.Collections.Generic;
using System.Linq;

namespace Sync
{
    class CollectionToTest
    {
        public CollectionToTest(IEnumerable<String> mainItems)
        {
            items = mainItems.OrderBy(p => p).ToArray();
            count = items.Length;
        }

        private Int32 count { get; set; }
        private Int32 current { get; set; }
        private String[] items { get; set; }

        internal void Next()
        {
            current++;
        }

        internal String GetCurrent()
        {
            return items[current];
        }

        internal Boolean NotEnded()
        {
            return current < count;
        }


    }
}
