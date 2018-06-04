using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days.Helper
{
    public class MemoryCleaner
    {
        public static bool isLockDown = false;

        public static void FreeUpMemory()
        {
            GC.Collect();
        }

        public static void CleanObjects(object obj)
        {
            obj = null;
        }
    }
}
