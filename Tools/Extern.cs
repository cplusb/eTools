using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Tools
{
    public static class StringBuilderClass
    {
        public static StringBuilder RemoveLast(this StringBuilder self, int length = 1)
        {
            if (self != null && self.Length > length)
                self.Remove(self.Length - length, length);
            return self;
        }
    }
}
