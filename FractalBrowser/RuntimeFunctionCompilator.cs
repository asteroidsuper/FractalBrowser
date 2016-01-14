using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace FractalBrowser
{
    public class RuntimeFunctionCompilator
    {
        public const char MoreEqual = (char)12001,LessEqual=(char)12002;
        protected static int GetPrior(string arg)
        {
            if (IsSkoba(arg) != 0) return 0;
            if (arg == "!" || arg == ":" || arg == ";") return 1;
            if (arg == ">" || arg == "<"||arg=="="||arg==MoreEqual.ToString()||arg==LessEqual.ToString()) return 2;
            if (arg == "+" || arg == "-") return 3;
            if (arg == "*" || arg == "/") return 4;
            if (arg == "^") return 5;
            if (arg == "N") return 6;
            return -1;
        }
        protected static double GetDoubleValue(Expression arg)
        {
            if (arg is ConstantExpression)
            {
                if (((ConstantExpression)arg).Value is double)
                {
                    return (double)((ConstantExpression)arg).Value;
                }
            }
            return ((Func<double>)(Expression.Lambda(arg).Compile()))();
        }
        protected static int GetPrior(Stack<string> arg)
        {
            if (arg.Count < 1) return -1;
            return GetPrior(arg.Peek());
        }
        protected static int IsSkoba(string arg)
        {
            if (arg == "(") return 1;
            foreach (MathFuncTemplate mft in enabledmathfuncs) if (mft.operatorname == arg) return 1;
            if (arg == ")") return -1;
            return 0;
        }
        public static double ArgMax(double Arg1, double Arg2)
        {
            return Arg1 > Arg2 ? 1 : 0;
        }
        public static double ArgMin(double Arg1, double Arg2)
        {
            return Arg1 < Arg2 ? 1 : 0;
        }
        #region EnabledMathFuncs
        protected static readonly MathFuncTemplate[] enabledmathfuncs = new MathFuncTemplate[] {
            new MathFuncTemplate(typeof(Math),"Sin","sin(","0",true),
            new MathFuncTemplate(typeof(Math),"Cos","cos(","1",true),
            new MathFuncTemplate(typeof(Math),"Tan","tan(","2",true),
            new MathFuncTemplate(typeof(Math),"Exp","exp(","3",true),
            new MathFuncTemplate(typeof(Math),"Abs","abs(","4",true),
            new MathFuncTemplate(typeof(Math),"Asin","aSin(","5",true),
            new MathFuncTemplate(typeof(Math),"Acos","aCos(","6",true),
            new MathFuncTemplate(typeof(Math),"Atan","aTan(","7",true),
            new MathFuncTemplate(typeof(Math),"Round","round(","8",true)
        };
        #endregion EnabledMathFuncs
        protected class MathFuncTemplate
        {
            public MathFuncTemplate(Type classtype, string methodname, string operatorname, string rename)
            {
                TypeOfClass = classtype;
                MethodName = methodname;
                this.operatorname = operatorname;
                Rename = rename;
            }
            public MathFuncTemplate(Type classtype, string methodname, string operatorname, string rename, bool determined)
            {
                TypeOfClass = classtype;
                MethodName = methodname;
                this.operatorname = operatorname;
                Rename = rename;
                IsDetermined = determined;
            }
            public Type TypeOfClass;
            public string MethodName;
            public string operatorname;
            public string Rename;
            public bool IsDetermined;
            public System.Reflection.MethodInfo Method
            {
                get
                {
                    return TypeOfClass.GetMethod(MethodName, new Type[] { typeof(double) });
                }
            }
        }
        protected static bool IsMathFunc(string arg)
        {
            foreach (MathFuncTemplate mft in enabledmathfuncs) if (mft.operatorname == arg) return true;
            return false;
        }
        protected static string[] Operators
        {
            get
            {
                List<string> res = new List<string>(new string[] { "(", ")", "!", ">", "<","=",MoreEqual.ToString(),LessEqual.ToString(), ":", ";", "+", "-", "*", "/", "^", "N" });
                foreach (MathFuncTemplate mft in enabledmathfuncs) res.Add(mft.operatorname);
                return res.ToArray();
            }
        }
        public static int CheckScobes(string arg)
        {
            int result = 0;
            foreach (char ch in arg)
            {
                if (ch == '(') result++;
                else if (ch == ')') result--;
            }
            return result;
        }
        protected static string PrepareTo(string arg, string str, char OldValue, char Newvalue, string InsertingValue)
        {
            str = PrepareTo(arg, str, OldValue, Newvalue, InsertingValue, 0);
            return str;
        }
        protected static string PrepareTo(string arg, string str, char OldValue, char NewValue, string InsertValue, int StartIndex)
        {
            int index = str.IndexOf(arg, StartIndex);
            if (index < 0) return str;
            bool change = false;
            str = PrepareTo(arg, str, OldValue, NewValue, InsertValue, index + arg.Length);
            for (int i = index + arg.Length - 1, opens = 0; i < str.Length; i++)
            {
                if (str[i] == '(') opens++;
                else if (str[i] == ')') opens--;
                else if (str[i] == OldValue && opens == 1)
                {
                    index = i;
                    change = true;
                    break;
                }
                if (opens <= 0)
                {
                    index = i;
                    break;
                }
            }
            if (change)
            {
                char[] result = str.ToCharArray();
                result[index] = NewValue;
                return new string(result);
            }
            else
            {
                return str.Insert(index, InsertValue);
            }

        }
        static int getindex(Expression e)
        {
            int result = 0;
            result = (int)((ConstantExpression)((BinaryExpression)e).Right).Value;
            return result;
        }
        public static LambdaExpression GetLambda(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            int cs = CheckScobes(input);
            if (cs != 0) throw new ArgumentException(cs > 0 ? ("В строке " + cs + " незакрытых скобок") : ("В строке на " + cs + " закрывающих скобок больше чем открывающих!"), "input");
            input = input.Replace("argsin", "aSin");
            input = input.Replace("asin", "aSin");
            input = input.Replace("argcos", "aCos");
            input = input.Replace("acos", "aCos");
            input = input.Replace("argtan", "aTan");
            input = input.Replace("atan", "aTan");
            input = input.Replace("Argsin", "aSin");
            input = input.Replace("Asin", "aSin");
            input = input.Replace("Argcos", "aCos");
            input = input.Replace("Acos", "aCos");
            input = input.Replace("Argtan", "aTan");
            input = input.Replace("Atan", "aTan");
            input = LogPrepare(input);
            //input = PrepareTo("argmax(", input, ',', '>', ">0");
            //input = input.Replace("argmax(", "(");
            //input = PrepareTo("argmin(", input, ',', '<', "<0");
            //input = input.Replace("argmin(", "(");
            input = PrepareTo("max(", input, ',', ':', ":0");
            input = PrepareTo("min(", input, ',', ';', ";0");
            input = input.Replace("max(", "(");
            input = input.Replace("min(", "(");
            input = input.Replace("+-", "-");
            input = RebindForNegative(input);
            //input = input.Replace("log", "!");
            string[] intp = GetProcessed(input);
            Stack<string> ststack = new Stack<string>();
            List<string> outlist = new List<string>();
            foreach (string str in intp)
            {
                if (GetPrior(str) < 0) outlist.Add(str);
                else if (IsSkoba(str) > 0)
                {
                    ststack.Push(str);
                }
                else if (IsSkoba(str) < 0)
                {
                    while (GetPrior(ststack) > 0)
                    {
                        outlist.Add(ststack.Pop());
                    }
                    string stmath = ststack.Pop();
                    if (IsMathFunc(stmath)) outlist.Add(stmath);
                    //if (GetPrior(ststack) > 0) outlist.Add(ststack.Pop());
                }
                else if (GetPrior(ststack) < GetPrior(str))
                {
                    ststack.Push(str);
                }
                else
                {
                    while (GetPrior(ststack) >= GetPrior(str))
                    {
                        outlist.Add(ststack.Pop());
                    }
                    ststack.Push(str);
                }
            }
            while (ststack.Count > 0) outlist.Add(ststack.Pop());
            ParameterExpression array = Expression.Parameter(typeof(double[]));
            Stack<Expression> expstack = new Stack<Expression>();
            outlist.RemoveAll((arg) => arg == "(");
            string strparam;
            for (int i = 0; i < outlist.Count; i++)
            {
                strparam = outlist[i];
                if (GetPrior(strparam) < 0)
                {
                    expstack.Push(GetCommonExp(strparam, array));
                }
                else if (IsMathFunc(strparam))
                {
                    expstack.Push(GetFuncExp(strparam, expstack.Pop()));
                }
                else
                {
                    Expression exp2, exp1;
                    exp2 = expstack.Pop();
                    if (strparam == "N")
                    {
                        if (exp2 is ConstantExpression) expstack.Push(Expression.Constant(-GetDoubleValue(exp2)));
                        else expstack.Push(Expression.Negate(exp2));
                        continue;
                    }
                    exp1 = expstack.Pop();
                    expstack.Push(GetMathExp(strparam, exp1, exp2));
                }
            }
            return Expression.Lambda(expstack.Pop(), array);
        }
        public static Delegate getdifdel(string input, int index)
        {
            LambdaExpression le = GetLambda(input);
            Expression e = GetDif(le.Body, index);
            return Expression.Lambda(e, le.Parameters).Compile();
        }

        protected static bool IsConst(Expression e, int index)
        {
            if (e is ConstantExpression) return true;
            if (e.NodeType == ExpressionType.ArrayIndex) return getindex(e) != index;
            return false;
        }

        protected static Expression MiniOptimize(Expression e)
        {
            if (e is BinaryExpression)
            {
                BinaryExpression b = (BinaryExpression)e;
                if (b.Left.NodeType == ExpressionType.Constant && b.Right.NodeType == ExpressionType.Constant) return Expression.Constant(GetDoubleValue(e));
                if (b.Left.NodeType == ExpressionType.ArrayIndex && b.Right.NodeType == ExpressionType.ArrayIndex)
                {
                    if (getindex(b.Left) == getindex(b.Right)) return Expression.Power(b.Left, Expression.Constant(2D));
                }
                if ((b.Left.NodeType == ExpressionType.ArrayIndex || b.Right.NodeType == ExpressionType.ArrayIndex) && (b.Left.NodeType == ExpressionType.Power || b.Right.NodeType == ExpressionType.Power))
                {
                    BinaryExpression ind = (BinaryExpression)(b.Left.NodeType == ExpressionType.ArrayIndex ? b.Left : b.Right);
                    BinaryExpression power = (BinaryExpression)(b.Left.NodeType == ExpressionType.Power ? b.Left : b.Right);
                    if (power.Left.NodeType == ExpressionType.ArrayIndex)
                    {
                        if (getindex(power.Left) == getindex(ind))
                        {
                            return Expression.Power(ind, MiniOptimize(Expression.Add(power.Right, Expression.Constant(1D))));
                        }
                    }
                }
            }
            return e;
        }
        public static Expression GetDif(Expression arg, int index)
        {
            Expression left, right;
            if (arg is BinaryExpression)
            {
                BinaryExpression binexp = (BinaryExpression)arg;
                if (binexp.NodeType == ExpressionType.Add)
                {
                    left = GetDif(binexp.Left, index);
                    right = GetDif(binexp.Right, index);
                    return MiniOptimize(Expression.Add(left, right));
                }
                if (arg.NodeType == ExpressionType.Multiply)
                {
                    left = GetDif(binexp.Left, index);
                    right = GetDif(binexp.Right, index);
                    if (left is ConstantExpression && right is ConstantExpression)
                    {
                        double ld = GetDoubleValue(left), rd = GetDoubleValue(right);
                        if (ld == 0D && rd == 0D)
                        {
                            return Expression.Constant(0D);
                        }
                        if (ld == 1D && rd == 1D)
                        {
                            return Expression.Multiply(Expression.Constant(2D), binexp.Right);
                        }
                        if (ld == 1D)
                        {
                            return binexp.Right;
                        }
                        return binexp.Left;
                    }
                    if (left is ConstantExpression || right is ConstantExpression)
                    {
                        /*Expression other=left is ConstantExpression? right:left;
                        ConstantExpression constan = (ConstantExpression)(right is ConstantExpression ? right : left);*/
                        return left is ConstantExpression ? right : left;
                    }
                }
                if (arg.NodeType == ExpressionType.ArrayIndex) return Expression.Constant(getindex(arg) == index ? 1D : 0D);
            }
            if (IsConst(arg, index)) return Expression.Constant(0D);
            return arg;
        }
        protected static string LogPrepare(string arg, int startindex)
        {
            int index = arg.IndexOf("log(", startindex);
            bool change = false;
            if (index < 0) return arg;
            arg = LogPrepare(arg, index + 4);
            for (int i = index + 3, opens = 0; i < arg.Length; i++)
            {
                if (arg[i] == '(') opens++;
                else if (arg[i] == ')') opens--;
                else if (arg[i] == ',' && opens == 1)
                {
                    index = i;
                    change = true;
                    break;
                }
                if (opens <= 0)
                {
                    index = i;
                    break;
                }
            }
            if (change)
            {
                char[] res = arg.ToCharArray();
                res[index] = '!';
                return new string(res);
            }
            else
            {
                return arg.Insert(index, "!e");
            }
        }
        protected static string LogPrepare(string arg)
        {
            arg = LogPrepare(arg, 0);
            return arg.Replace("log(", "(");
        }
        protected static string RebindForNegative(string arg)
        {
            char[] result = arg.ToCharArray();
            if (result[0] == '-') result[0] = 'N';
            for (int i = 1; i < result.Length; i++)
            {
                if (result[i] == '-' && GetPrior(result[i - 1].ToString()) > -1) result[i] = 'N';
            }
            return new string(result);
        }

        public static Delegate CompileFunc(string input)
        {
            input = input.Replace(">=", MoreEqual.ToString()).Replace("<=",LessEqual.ToString());
            return GetLambda(input).Compile();
        }
        public static double rand()
        {
            return new Random().NextDouble();
        }

        protected static Expression GetCommonExp(string str, Expression array)
        {
            double res;
            if (str.ToLower() == "rand") return Expression.Call(typeof(RuntimeFunctionCompilator).GetMethod("rand"));
            if (str.ToLower() == "pinkie") return Expression.Constant(double.PositiveInfinity);
            if (str.ToLower() == "pi") return Expression.Constant(Math.PI);
            if (str.ToLower() == "e") return Expression.Constant(Math.E);
            if (double.TryParse(str.Replace('.', ','), out res)) return Expression.Constant(res);
            return Expression.ArrayIndex(array, Expression.Constant(int.Parse(str.Remove(0, 1))));
        }
        protected static Expression GetFuncExp(string str, Expression Exp)
        {
            System.Reflection.MethodInfo Method;
            foreach (MathFuncTemplate mft in enabledmathfuncs)
                if (mft.operatorname == str)
                {
                    Method = mft.TypeOfClass.GetMethod(mft.MethodName, new Type[] { typeof(double) });
                    if (mft.IsDetermined && Exp is ConstantExpression)
                    {
                        return Expression.Constant((double)Method.Invoke(null, new object[] { GetDoubleValue(Exp) }));
                    }
                    return Expression.Call(Method, Exp);
                }
            return null;
        }
        protected static Expression GetMathExp(string str, Expression Exp1, Expression Exp2)
        {
            System.Reflection.MethodInfo Method;
            switch (str[0])
            {
                case '!': { Method = typeof(Math).GetMethod("Log", new Type[] { typeof(double), typeof(double) }); break; }
                case '>': { Method = typeof(RuntimeFunctionCompilator).GetMethod("ArgMax"); break; }
                case '<': { Method = typeof(RuntimeFunctionCompilator).GetMethod("ArgMin"); break; }
                case '=': { return Expression.Condition(Expression.Equal(Exp1,Exp2),Expression.Constant(1D),Expression.Constant(0D)); }
                case LessEqual: {return Expression.Condition(Expression.LessThanOrEqual(Exp1, Exp2), Expression.Constant(1D), Expression.Constant(0D)); }
                case MoreEqual: { return Expression.Condition(Expression.LessThanOrEqual(Exp1, Exp2), Expression.Constant(0D), Expression.Constant(1D)); }
                case ':': { Method = typeof(Math).GetMethod("Max", new Type[] { typeof(double), typeof(double) }); break; }
                case ';': { Method = typeof(Math).GetMethod("Min", new Type[] { typeof(double), typeof(double) }); break; }
                case '^':
                    {
                        // Method = typeof(Math).GetMethod("Pow", new Type[] { typeof(double), typeof(double) }); break;

                        return MiniOptimize(Expression.Power(Exp1, Exp2));
                    }
                case '+':
                    {
                        if (Exp1 is ConstantExpression && Exp2 is ConstantExpression)
                        {
                            return Expression.Constant(GetDoubleValue(Exp1) + GetDoubleValue(Exp2));
                        }
                        if (Exp1 is ConstantExpression || Exp2 is ConstantExpression)
                        {
                            ConstantExpression constant = (ConstantExpression)((Exp1 is ConstantExpression) ? Exp1 : Exp2);
                            double Value = GetDoubleValue(constant);
                            if (Value == 0D)
                            {
                                return ((Exp1 is ConstantExpression) ? Exp2 : Exp1);
                            }
                            if (double.IsInfinity(Value) || double.IsNaN(Value))
                            {
                                return Expression.Constant(Value);
                            }
                        }
                        return Expression.Add(Exp1, Exp2);
                    }
                case '-':
                    {
                        if (Exp1 is ConstantExpression && Exp2 is ConstantExpression)
                        {
                            return Expression.Constant(GetDoubleValue(Exp1) - GetDoubleValue(Exp2));
                        }
                        if (Exp1 is ConstantExpression || Exp2 is ConstantExpression)
                        {
                            ConstantExpression constant = (ConstantExpression)((Exp1 is ConstantExpression) ? Exp1 : Exp2);
                            double Value = GetDoubleValue(constant);
                            if (Value == 0D)
                            {
                                return ((Exp1 is ConstantExpression) ? Exp2 : Exp1);
                            }
                            if (double.IsInfinity(Value) || double.IsNaN(Value))
                            {
                                return Expression.Constant(Value);
                            }
                        }
                        return Expression.Subtract(Exp1, Exp2);
                    }
                case '*':
                    {
                        if (Exp1 is ConstantExpression && Exp2 is ConstantExpression)
                        {
                            return Expression.Constant(GetDoubleValue(Exp1) * GetDoubleValue(Exp2));
                        }
                        if (Exp1 is ConstantExpression || Exp2 is ConstantExpression)
                        {
                            ConstantExpression constant = (ConstantExpression)((Exp1 is ConstantExpression) ? Exp1 : Exp2);
                            double Value = GetDoubleValue(constant);
                            if (Value == 0D)
                            {
                                return Expression.Constant(0D);
                            }
                            if (Value == 1D)
                            {
                                return ((Exp1 is ConstantExpression) ? Exp2 : Exp1);
                            }
                            if (double.IsInfinity(Value) || double.IsNaN(Value))
                            {
                                return Expression.Constant(Value);
                            }
                        }
                        return MiniOptimize(Expression.Multiply(Exp1, Exp2));
                    }
                case '/':
                    {
                        if (Exp1 is ConstantExpression && Exp2 is ConstantExpression)
                        {
                            return Expression.Constant(GetDoubleValue(Exp1) / GetDoubleValue(Exp2));
                        }
                        return Expression.Divide(Exp1, Exp2);
                    }
                default:
                    {
                        Method = null;
                        break;
                    }
            }
            if (Method != null)
            {
                if (Exp1 is ConstantExpression && Exp2 is ConstantExpression)
                {
                    return Expression.Constant(Method.Invoke(null, new object[] { GetDoubleValue(Exp1), GetDoubleValue(Exp2) }));
                }
                else
                {
                    return Expression.Call(Method, Exp1, Exp2);
                }

            }
            return null;
        }
        protected static string GetMathFuncTemplate(char arg)
        {
            foreach (MathFuncTemplate mft in enabledmathfuncs)
            {
                if (mft.Rename == arg + "") return mft.operatorname;
            }
            return arg + "";
        }
        protected static string[] Divide(string oper)
        {
            List<string> result = new List<string>();
            foreach (MathFuncTemplate mft in enabledmathfuncs)
            {
                oper = oper.Replace(mft.Rename.ToString(), "");
                oper = oper.Replace(mft.operatorname, mft.Rename);
            }
            foreach (char ch in oper)
            {
                result.Add(GetMathFuncTemplate(ch));
            }
            return result.ToArray();
        }
        protected static string[] GetProcessedOperators(string[] opers)
        {
            List<string> result = new List<string>();
            string[] operators = Operators;
            foreach (string oper in opers)
            {
                result.AddRange(Divide(oper));
            }
            return result.ToArray();
        }
        public static string[] GetProcessed(string input)
        {
            string[] parameters = input.Split(Operators, StringSplitOptions.RemoveEmptyEntries);
            string[] opers = input.Split(parameters, StringSplitOptions.RemoveEmptyEntries);
            opers = GetProcessedOperators(opers);
            List<string> linked = new List<string>();
            int p, o, ind = 0;
            for (p = 0, o = 0; p < parameters.Length && o < opers.Length;)
            {
                if (input.IndexOf(parameters[p], ind) < input.IndexOf(opers[o], ind))
                {
                    linked.Add(parameters[p]);
                    ind += parameters[p++].Length;
                }
                else
                {
                    linked.Add(opers[o]);
                    ind += opers[o++].Length;
                }
            }
            if (p < parameters.Length) linked.Add(parameters[p]);

            
            return linked.ToArray();
        }
    }
}
