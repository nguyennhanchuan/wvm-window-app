using System;
using System.Linq.Expressions;

namespace VendingMachine.Helpers
{
    public static class ExpressionHelper
    {
        public static string Name<T>(Expression<Func<T>> expression)
        {
            var lambda = expression as LambdaExpression;
            var memberExpression = (MemberExpression) lambda.Body;

            return memberExpression.Member.Name;
        }
    }
}