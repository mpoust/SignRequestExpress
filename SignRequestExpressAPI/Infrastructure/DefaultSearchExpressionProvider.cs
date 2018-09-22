////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultSearchExpressionProvider.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/19/2018
 * Last Modified: 9/22/2018
 * Description: Implementation of the ISearchExpressionProvider interface.
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
    public class DefaultSearchExpressionProvider : ISearchExpressionProvider
    {
        protected const string EqualsOperator = "eq";

        public virtual IEnumerable<string> GetOperators()
        {
            yield return EqualsOperator;
        }

        /*
        public virtual Expression GetComparison(
            MemberExpression left,
            string op,
            ConstantExpression right)
        {
            if (!op.Equals("eq", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Invalid operator '{op}'.");

            return Expression.Equal(left, right);
        }
        */

        public virtual Expression GetComparison(
        MemberExpression left,
        string op,
        ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case EqualsOperator: return Expression.Equal(left, right);
                default: throw new ArgumentException($"Invalid operator '{op}'.");
            }
        }

        public virtual ConstantExpression GetValue(string input)
        => Expression.Constant(input);
    }
}
