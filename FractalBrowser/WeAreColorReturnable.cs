using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public static class WeAreColorReturnable
    {
        public static IColorReturnable[] GetWith(IColorReturnable add)
        {
            if (add is My2DClassicColorMode) return new IColorReturnable[] {add,new Simple2DFractalColorMode()};
            if (add is Simple2DFractalColorMode) return new IColorReturnable[] {new Simple2DFractalColorMode() , add };
            return new IColorReturnable[] {new My2DClassicColorMode(),new Simple2DFractalColorMode(),add };
        }
    }
}
