////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: IonOutputFormatter.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified:
 * Description: This class wraps up the default JSON Output Formatter that ships with MVC but changes the media type to application/ion+json
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LandonApi.Infrastructure
{
    public class IonOutputFormatter : TextOutputFormatter
    {
        private readonly JsonOutputFormatter _jsonOutputFormatter;

        public IonOutputFormatter(JsonOutputFormatter jsonOutputFormatter)
        {
            _jsonOutputFormatter = jsonOutputFormatter ?? throw new ArgumentNullException(nameof(jsonOutputFormatter));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/ion+json"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
            => _jsonOutputFormatter.WriteResponseBodyAsync(context, selectedEncoding);
    }
}
