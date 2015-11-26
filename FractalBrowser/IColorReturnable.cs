using System;
using System.Drawing;

namespace FractalBrowser
{
    public interface IColorReturnable
    {
        Color GetColor(object optimizer,int X,int Y);
        object Optimize(FractalAssociationParametrs FAP,object Extra=null);
    }
}
