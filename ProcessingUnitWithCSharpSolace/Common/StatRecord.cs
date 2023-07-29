using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piper.Common
{
    public class StatRecord
    {
        public long N = 0;
        public long max_x = 0;
        public long min_x = 0;
        public double mean_x = 0;
        public double M2 = 0;
        private static object _Lock = new object();

        public long Update(long x)
        {
            long recId;
            double delta;
            lock (_Lock)
            {
                if (N == 0)
                {
                    min_x = max_x = x;
                    mean_x = M2 = 0.0;
                }
                else if (x < min_x)
                {
                    min_x = x;
                }
                else if (x > max_x)
                {
                    max_x = x;
                }

                N++;
                recId = N;
                delta = (double)x - mean_x;
                mean_x += delta / (double)N;
                M2 += delta * ((double)x - mean_x);
            }

            return recId;
        }

        private double StdDev()
        {
            if (N > 0 && M2 >= 0.0)
                return Math.Sqrt(M2 / (double)N);
            else
                return 0.0;
        }
        public string getStat(double factor)
        {
            string res;
            lock (_Lock)
            {
                res = string.Format("min/max/avg/dev: {0}/{1}/{2}/{3}",
                            min_x * factor, max_x * factor, mean_x * factor, StdDev() * factor);
            }

            return res;
        }
    }
   
}
