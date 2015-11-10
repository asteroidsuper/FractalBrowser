using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public class FractalCloudPoints
    {
        public FractalCloudPoints(int MaxAmmountAtTrace,FractalCloudPoint[][][] fractalCloudPoint)
        {
            this.MaxAmmountAtTrace = MaxAmmountAtTrace;
            this.fractalCloudPoint = fractalCloudPoint;
        }
        public int MaxAmmountAtTrace;
        public FractalCloudPoint[][][] fractalCloudPoint;
    }
}
