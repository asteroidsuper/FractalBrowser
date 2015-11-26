using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public class RadianMatrix
    {
        public RadianMatrix(int StartWidth)
        {
            Matrix=new double[StartWidth][];
        }
        public double[][] Matrix;
    }
}
