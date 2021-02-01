namespace Genie
{
    using System;
    using System.Linq.Expressions;

    internal static class ExpressionUtil
    {
        public static Func<TArg1, TArg2, TResult> CreateMethodCall<TArg1, TArg2, TResult>(string methodName)
        {
            var x = Expression.Parameter(typeof(TArg1), "x");
            var y = Expression.Parameter(typeof(TArg2), "y");

            var method = typeof(TArg1).GetMethod(
                methodName,
                genericParameterCount: 0,
                new[] { typeof(TArg2) });

            return Expression
                .Lambda<Func<TArg1, TArg2, TResult>>(
                    Expression.Call(x, method, y), x, y)
                .Compile();
        }

        public static Func<TArg1, TResult> CreateStaticCall<TArg1, TResult>(Type type, string methodName)
        {
            var x = Expression.Parameter(typeof(TArg1), "x");

            return Expression
                .Lambda<Func<TArg1, TResult>>(
                    Expression.Call(null, type.GetMethod(methodName), x), x)
                .Compile();
        }

        public static Func<TArg1, TArg2, TResult> CreateStaticCall<TArg1, TArg2, TResult>(Type type, string methodName)
        {
            var x = Expression.Parameter(typeof(TArg1), "x");
            var y = Expression.Parameter(typeof(TArg2), "y");

            return Expression
                .Lambda<Func<TArg1, TArg2, TResult>>(
                    Expression.Call(null, type.GetMethod(methodName), x, y), x, y)
                .Compile();
        }

        public static Func<TArg1, TResult> CreateExpression<TArg1, TResult>(Func<Expression, UnaryExpression> body)
        {
            ParameterExpression inp = Expression.Parameter(typeof(TArg1), "inp");
            try
            {
                return Expression.Lambda<Func<TArg1, TResult>>(body(inp), inp).Compile();
            }
            catch (Exception ex)
            {
                string msg = ex.Message; // avoid capture of ex itself
                return delegate { throw new InvalidOperationException(msg); };
            }
        }

        public static Func<TArg1, TArg2, TResult> CreateExpression<TArg1, TArg2, TResult>(
            Func<Expression, Expression, BinaryExpression> body)
        {
            return CreateExpression<TArg1, TArg2, TResult>(body, false);
        }

        public static Func<TArg1, TArg2, TResult> CreateExpression<TArg1, TArg2, TResult>(
            Func<Expression, Expression, BinaryExpression> body, bool castArgsToResultOnFailure)
        {
            ParameterExpression lhs = Expression.Parameter(typeof(TArg1), "lhs");
            ParameterExpression rhs = Expression.Parameter(typeof(TArg2), "rhs");
            
            try
            {
                try
                {
                    return Expression.Lambda<Func<TArg1, TArg2, TResult>>(body(lhs, rhs), lhs, rhs).Compile();
                }
                catch (InvalidOperationException)
                {
                    if (castArgsToResultOnFailure && !(             // if we show retry                                                        
                            typeof(TArg1) == typeof(TResult) &&     // and the args aren't
                            typeof(TArg2) == typeof(TResult)))
                    {
                        // already "TValue, TValue, TValue"...
                        // convert both lhs and rhs to TResult (as appropriate)
                        Expression castLhs = typeof(TArg1) == typeof(TResult) ?
                                (Expression)lhs :
                                (Expression)Expression.Convert(lhs, typeof(TResult));
                        Expression castRhs = typeof(TArg2) == typeof(TResult) ?
                                (Expression)rhs :
                                (Expression)Expression.Convert(rhs, typeof(TResult));

                        return Expression.Lambda<Func<TArg1, TArg2, TResult>>(
                            body(castLhs, castRhs), lhs, rhs).Compile();
                    }
                    else throw;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message; // avoid capture of ex itself
                return delegate { throw new InvalidOperationException(msg); };
            }
        }
    }
}