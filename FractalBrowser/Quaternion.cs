using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalBrowser
{
    [Serializable]
    public class Quaternion:ICloneable
    {
        /*_________________________________________________________Конструкторы_класса____________________________________________________________*/
        #region Constructors
        private Quaternion() { }
        public Quaternion(double X,double Y,double Z)
        {
            _scalar = 0;
            _x = X;
            _y = Y;
            _z = Z;
        }
        public Quaternion(double Radian,double X=1D,double Y=0D,double Z=0D)
        {
            Radian /= 2D;
            _scalar = Math.Cos(Radian);
            double Mul=Math.Sin(Radian), Norm=Math.Sqrt(X*X+Y*Y+Z*Z);
            if (Norm == 0D) return;
            _x = X / Norm * Mul;
            _y = Y / Norm * Mul;
            _z = Z / Norm * Mul;
        }
        #endregion /Constructors

        /*_______________________________________________________Частные_атрибуты_класса__________________________________________________________*/
        #region Private atribytes
        private double _scalar;
        private double _x;
        private double _y;
        private double _z;
        #endregion /Private atribytes

        /*_______________________________________________________Перегруженные_операторы__________________________________________________________*/
        #region Overrided operators
        public static Quaternion operator +(Quaternion a,Quaternion b)
        {
            Quaternion result = new Quaternion();
            result._scalar = a._scalar + b._scalar;
            result._x = a._x + b._x;
            result._y = a._y + b._y;
            result._z = a._z + b._z;
            return result;
        }
        public static Quaternion operator*(Quaternion a,Quaternion b)
        {
            Quaternion result = new Quaternion();
            result._x=a._scalar*b._x+a._x*b._scalar+a._y*b._z-a._z*b._y;
            result._y=a._scalar*b._y-a._x*b._z+a._y*b._scalar+a._z*b._x;
            result._z=a._scalar*b._z+a._x*b._y-a._y*b._x+a._z*b._scalar;  //new Quaternion(addvec(addvec(mulvec(a.vec,b.vec),mulvec(a.w,b.vec)),mulvec(b.w,a.vec)));
            result._scalar = a._scalar * b._scalar - a._x * b._x - a._y * b._y - a._z * b._z; //scalarmul(a.vec, b.vec);
            return result;
        }
        public static Quaternion operator*(Quaternion q,double scalar)
        {
            Quaternion result = (Quaternion)q.Clone();
            result._scalar *= scalar;
            result._x *= scalar;
            result._y *= scalar;
            result._z *= scalar;
            return result;
        }
        public static Quaternion operator *(double scalar, Quaternion q)
        {
            Quaternion result = (Quaternion)q.Clone();
            result._scalar *= scalar;
            result._x *= scalar;
            result._y *= scalar;
            result._z *= scalar;
            return result;
        }
        #endregion /Overrided operators

        /*_______________________________________________________Реализация_интерфейсов___________________________________________________________*/
        #region Realization of interfaces
        public object Clone()
        {
            Quaternion result = new Quaternion();
            result._scalar = this._scalar;
            result._x = this._x;
            result._y = this._y;
            result._z = this._z;
            return result;
        }
        #endregion /Realization of interfaces

        /*______________________________________________________Общедоступные_поля_класса_________________________________________________________*/
        #region Public fields
        public double[] Vector
        {
            get
            {
                return new double[] {_x,_y,_z};
            }
        }
        public double Radian
        {
            get
            {
                return 2D*Math.Acos(_scalar);
            }
        }
        #endregion /Public fields

        /*_____________________________________________________Общедоступные_методы_класса________________________________________________________*/
        #region Public methods
        public virtual double[] Rotate(double[] Vector)
        {
            if (Vector == null) throw new ArgumentNullException();
            Quaternion result;
            if (Vector.Length > 2)
                result = new Quaternion(Vector[0], Vector[1], Vector[2]);
            else if (Vector.Length > 1)
                result = new Quaternion(Vector[0], Vector[1], 0);
            else if (Vector.Length > 0)
                result = new Quaternion(Vector[0], 0, 0);
            else throw new ArgumentException();
            result=negative_mul(this * result);
            return new double[]{result._x,result._y,result._z};
        }
        public virtual void Rotate(Triplex arg)
        {
            double[] vec = new double[] {arg.x,arg.y,arg.z};
            vec = Rotate(vec);
            arg.x = vec[0];
            arg.y = vec[1];
            arg.z = vec[2];
        }
        #endregion /Public methods

        /*__________________________________________________Общедоступные_статические_методы______________________________________________________*/
        #region Public static methods
        public static Quaternion GetAbcissRotater(double Radian)
        {
            return new Quaternion(Radian);
        }
        public static Quaternion GetOrdinateRotater(double Radian)
        {
            return new Quaternion(Radian,0,1);
        }
        public static Quaternion GetAplicateRotater(double Radian)
        {
            return new Quaternion(Radian,0,0,1);
        }
        #endregion /Public static methods

        /*_______________________________________________________Частные_утилиты_класса___________________________________________________________*/
        #region Private utilities of class
        private Quaternion negative_mul(Quaternion a)
        {
            Quaternion b = this;
            Quaternion result = new Quaternion();
            result._x=-a._scalar * b._x + a._x * b._scalar - a._y * b._z + a._z * b._y;
            result._y=-a._scalar*b._y+a._x*b._z+a._y*b._scalar-a._z*b._x;
            result._z=-a._scalar*b._z-a._x*b._y+a._y*b._x+a._z*b._scalar; 
            result._scalar = a._scalar * b._scalar-a._x*b._x-a._y*b._y-a._z*b._z;
            return result;
        }
        #endregion /Private utilities of class

        /*______________________________________________________Общедоступные_подклассы____________________________________________________________*/
        #region Public subclasses
        public class QuaternionNull:Quaternion,ICloneable
        {
            public override double[] Rotate(double[] Vector)
            {
                return (double[])Vector.Clone();
            }
            public override void Rotate(Triplex arg)
            {
            }
            public double Radian
            {
                get { return 0D; }
            }
            public double[] Vector
            {
                get
                {
                    return new double[] { 0, 0, 0 };
                }
            }
            public object Clone()
            {
                return QuaternionNull.Null;
            }
        }
        #endregion /Public subclasses

        /*_________________________________________________Общедоступные_статические_данные________________________________________________________*/
        #region Static data
        public static readonly QuaternionNull Null=new QuaternionNull();
        #endregion /Static data
    }
}
