namespace Genie
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class Operations
    {
        public static U Convert<T, U>(T x)
            where T : IComparable<T>
            where U : IComparable<U> =>
            Operations<T, U>.Convert(x);

        public static T Add<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.Add(x, y);

        public static T Subtract<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.Subtract(x, y);

        public static T Multiply<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.Multiply(x, y);

        public static T Divide<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.Divide(x, y);

        public static T Negate<T>(in T x)
            where T : IComparable<T> =>
            Operations<T>.Negate(x);

        public static T Sqrt<T>(in T x)
            where T : IComparable<T> =>
            Operations<T>.Sqrt(x);

        public static T Abs<T>(in T x)
            where T : IComparable<T> =>
            Operations<T>.Abs(x);

        public static T Pow<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.Pow(x, y);

        public static T Sin<T>(in T x)
            where T : IComparable<T> =>
            Operations<T>.Sin(x);

        public static T Cos<T>(in T x)
            where T : IComparable<T> =>
            Operations<T>.Cos(x);

        public static T Tan<T>(in T x)
            where T : IComparable<T> =>
            Operations<T>.Tan(x);

        public static T Atan2<T>(in T y, in T x)
            where T : IComparable<T> =>
            Operations<T>.Atan2(y, x);

        public static bool LessThan<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.LessThan(x, y);

        public static bool LessThanOrEqual<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.LessThanOrEqual(x, y);

        public static bool GreaterThan<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.GreaterThan(x, y);

        public static bool GreaterThanOrEqual<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.GreaterThanOrEqual(x, y);

        public static bool Equal<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.Equal(x, y);

        public static bool NotEqual<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.NotEqual(x, y);

        public static int CompareTo<T>(in T x, in T y)
            where T : IComparable<T> =>
            Operations<T>.CompareTo(x, y);
    }

    internal static class Operations<T, U>
        where T : IComparable<T>
    {
        private static readonly Lazy<Func<T, U>> convert;

        public static Func<T, U> Convert => convert.Value;

        static Operations()
        {
            convert = new Lazy<Func<T, U>>(() =>
                ExpressionUtil.CreateExpression<T, U>(
                    body => Expression.Convert(body, typeof(U))), true);
        }
    }

    internal static class Operations<T>
        where T : IComparable<T>
    {
        private static IDictionary<Type, Type> providers = new Dictionary<Type, Type>
        {
            [typeof(double)] = typeof(Math),
            [typeof(float)] = typeof(MathF),
            [typeof(int)] = typeof(Math),
            [typeof(EFloat)] = typeof(EFloat),
        };

        private static Lazy<Func<T, T>> neg, abs, sqrt;

        private static Lazy<Func<T, T>> sin, cos, tan;

        private static Lazy<Func<T, T>> sinh, cosh, tanh;

        private static Lazy<Func<T, T, T>> add, sub, mul, div, pow, atan2;

        private static Lazy<Func<T, T, bool>> gt, gte, lt, lte, eq, ne;

        private static Lazy<Func<T, T, int>> cmp;

        public static Func<T, T, T> Add => add.Value;

        public static Func<T, T, T> Subtract => sub.Value;

        public static Func<T, T, T> Multiply => mul.Value;

        public static Func<T, T, T> Divide => div.Value;

        public static Func<T, T> Negate => neg.Value;

        public static Func<T, T> Sqrt => sqrt.Value;

        public static Func<T, T> Abs => abs.Value;

        public static Func<T, T, T> Pow => pow.Value;

        public static Func<T, T> Sin => sin.Value;

        public static Func<T, T> Cos => cos.Value;

        public static Func<T, T> Tan => tan.Value;

        public static Func<T, T, T> Atan2 => atan2.Value;

        public static Func<T, T, bool> LessThan => lt.Value;

        public static Func<T, T, bool> LessThanOrEqual => lte.Value;

        public static Func<T, T, bool> GreaterThan => gt.Value;

        public static Func<T, T, bool> GreaterThanOrEqual => gte.Value;

        public static Func<T, T, bool> Equal => eq.Value;

        public static Func<T, T, bool> NotEqual => ne.Value;

        public static Func<T, T, int> CompareTo => cmp.Value;

        static Operations()
        {
            var math = providers[typeof(T)];

            neg = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateExpression<T, T>(Expression.Negate), true);

            add = new Lazy<Func<T, T, T>>(() =>
                ExpressionUtil.CreateExpression<T, T, T>(Expression.Add), true);

            sub = new Lazy<Func<T, T, T>>(() =>
                ExpressionUtil.CreateExpression<T, T, T>(Expression.Subtract), true);

            mul = new Lazy<Func<T, T, T>>(() =>
                ExpressionUtil.CreateExpression<T, T, T>(Expression.Multiply), true);

            div = new Lazy<Func<T, T, T>>(() =>
                ExpressionUtil.CreateExpression<T, T, T>(Expression.Divide), true);

            sqrt = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T>(math, "Sqrt"));

            abs = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T>(math, "Abs"));

            pow = new Lazy<Func<T, T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T, T>(math, "Pow"));

            sin = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T>(math, "Sin"));

            cos = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T>(math, "Cos"));

            tan = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T>(math, "Tan"));

            sinh = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T>(math, "Sinh"));

            cosh = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T>(math, "Cosh"));

            tanh = new Lazy<Func<T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T>(math, "Tanh"));

            atan2 = new Lazy<Func<T, T, T>>(() =>
                ExpressionUtil.CreateStaticCall<T, T, T>(math, "Atan2"));

            gt = new Lazy<Func<T, T, bool>>(() =>
                ExpressionUtil.CreateExpression<T, T, bool>(Expression.GreaterThan), true);

            lt = new Lazy<Func<T, T, bool>>(() =>
                ExpressionUtil.CreateExpression<T, T, bool>(Expression.LessThan), true);

            gte = new Lazy<Func<T, T, bool>>(() =>
               ExpressionUtil.CreateExpression<T, T, bool>(Expression.GreaterThanOrEqual), true);

            lte = new Lazy<Func<T, T, bool>>(() =>
                ExpressionUtil.CreateExpression<T, T, bool>(Expression.LessThanOrEqual), true);

            eq = new Lazy<Func<T, T, bool>>(() =>
                ExpressionUtil.CreateExpression<T, T, bool>(Expression.Equal), true);

            ne = new Lazy<Func<T, T, bool>>(() =>
                ExpressionUtil.CreateExpression<T, T, bool>(Expression.NotEqual), true);

            cmp = new Lazy<Func<T, T, int>>(() =>
                ExpressionUtil.CreateMethodCall<T, T, int>("CompareTo"), true);
        }
    }
}