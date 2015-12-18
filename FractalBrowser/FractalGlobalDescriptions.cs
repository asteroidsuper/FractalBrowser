using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    public static class FractalGlobalDescriptions
    {
        public static readonly string RightEdgeOf2DFractal="Правая граница двухмерного фрактала, в данном системе, обычно является позицией на оси абcцисс в декартовой системе координат"+
            "(правым пределом фрактала),\nдо которой по оси абcцисс (координата \"X\") строиться двухмерный фрактал, представляет число обычно большее чем левая граница, используется действительное число.";
        public static readonly string LeftEdgeOf2DFractal = "Левая граница двухмерного фрактала, в данной системе, обычно является позицией на оси абcцисс в декартовой системе координат (левым пределом фрактала),\n"+
            "с которой начинаеться построение фрактала по оси абcцисс (координата \"X\"), представляет число обычно меньшее чем правая граница фрактала, используется действительное число.";
        public static readonly string TopEdgeOf2DFractal="Верхняя граница двухмерного фрактала";
    }
}
