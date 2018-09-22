////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: ComparableSearchExpressionProvider.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/22/2018
 * Last Modified: 
 * Description: 
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Infrastructure
{
    public abstract class ComparableSearchExpressionProvider : DefaultSearchExpressionProvider
    {
        private const string GreaterThanOperator = "gt";
        private const string GreaterThanEqualToOperator = "gte";
        private const string LessThanOperator = "lt";
        private const string LessThanEqualToOperator = "lte";

        public override IEnumerable<string> GetOperators()
            => base.GetOperators()
            .Concat(new[]
            {
                GreaterThanOperator,
                GreaterThanEqualToOperator,
                LessThanOperator,
                LessThanEqualToOperator
            });

        public override Expression GetComparison(
            MemberExpression left,
            string op,
            ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case GreaterThanOperator: return Expression.GreaterThan(left, right);
                case GreaterThanEqualToOperator: return Expression.GreaterThanOrEqual(left, right);
                case LessThanOperator: return Expression.LessThan(left, right);
                case LessThanEqualToOperator: return Expression.LessThanOrEqual(left, right);
                default: return base.GetComparison(left, op, right);
            }
        }
    }
}
