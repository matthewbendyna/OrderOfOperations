using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOfOperations
{
    public class Cutarray
    {
        public int[] cutArray(int[] arrayname, int index)
        {
            int current = index;
            while (current < arrayname.Length - 1)
            {
                arrayname[current] = arrayname[current + 1];
                current++;
            }
            Array.Resize(ref arrayname, arrayname.Length - 1);
            return arrayname;
        }
        public double[] cutArray(double[] arrayname, int index)
        {
            int current = index;
            while (current < arrayname.Length - 1)
            {
                arrayname[current] = arrayname[current + 1];
                current++;
            }
            Array.Resize(ref arrayname, arrayname.Length - 1);
            return arrayname;
        }
        public char[] cutArray(char[] arrayname, int index)
        {
            int current = index;
            while (current < arrayname.Length - 1)
            {
                arrayname[current] = arrayname[current + 1];
                current++;
            }
            Array.Resize(ref arrayname, arrayname.Length - 1);
            return arrayname;
        }
        public string[] cutArray(string[] arrayname, int index)
        {
            int current = index;
            while (current < arrayname.Length - 1)
            {
                arrayname[current] = arrayname[current + 1];
                current++;
            }
            Array.Resize(ref arrayname, arrayname.Length - 1);
            return arrayname;
        }
        public bool[] cutArray(bool[] arrayname, int index)
        {
            int current = index;
            while (current < arrayname.Length - 1)
            {
                arrayname[current] = arrayname[current + 1];
                current++;
            }
            Array.Resize(ref arrayname, arrayname.Length - 1);
            return arrayname;
        }
    }
}
