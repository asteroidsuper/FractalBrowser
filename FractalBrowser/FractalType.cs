using System;

namespace FractalBrowser
{
    /// <summary>
    /// Классификация фракталов, классифицированные по виду матрицы итераций и используемому уникальному параметру.
    /// </summary>
    public enum FractalType
    {
        /// <summary>
        /// Обычные двухмерные итерационные фракталы, обычно строяться на комплексной плоскости, к ним относяться такие фракталы как mandelbrot Julia newthon и т.д. (Отсуствует уникальный параметр)
        /// </summary>
        _2DStandartIterationType,
        /// <summary>
        /// Модифицированные двухмерные итерационные фракталы, в качестве уникально параметра хранить трхмерную матрицу FractalCloudPoint (мандельбротовы облака).
        /// </summary>
        _2DStandartIterationTypeWithCloudPoints,
        /// <summary>
        /// Двухмерный фрактал с уникальным элементов double матрицей NxMx3
        /// </summary>
        _2DTrioDoubleUniqueMatrix
    }
}
