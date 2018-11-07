////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DecimalToIntSearchExpressionProvider.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 11/07/2018
 * Last Modified: 
 * Description: Allows for searching against a byte value. 
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
    public class ByteSearchExpressionProvider : DefaultSearchExpressionProvider
    {
        public override ConstantExpression GetValue(string input)
        {
            byte value;
            switch (input)
            {
                case "0":
                    value = 0;
                    break;
                case "1":
                    value = 1;
                    break;
                case "2":
                    value = 2;
                    break;
                case "3":
                    value = 3;
                    break;
                default:
                    value = 0;
                    break;
            }

            return Expression.Constant(value);
        }
    }
}
