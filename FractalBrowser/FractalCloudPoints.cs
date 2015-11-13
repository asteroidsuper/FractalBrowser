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
        public void Clear()
        {
            int take = fractalCloudPoint.Length;
            for(int i=take-1;i>=0;i--)
            {
                if (fractalCloudPoint[i][0] == null) take--;
            }
            this.fractalCloudPoint = fractalCloudPoint.Take(take).ToArray();
        }
    }
}
