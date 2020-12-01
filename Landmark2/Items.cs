using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landmark2
{
    public class Items
    {
        public String[] meaning;
        public String[] spelling;
        public Boolean[] miss;
        public String[] speak;
        public int hint;
        public int repetitionMax;
        public Boolean random;
        
        public Items()
        {

        }
        public int getMissLength()
        {
            int missLength = 0;
            for (int i = 0; i < meaning.Length; i++)
            {
                if (miss[i])
                {
                    missLength++;
                }
            }
            return missLength;
        }
    }
}
