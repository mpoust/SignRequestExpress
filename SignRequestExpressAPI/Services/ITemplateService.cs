////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: ITemplateService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/17/2018
 * Last Modified:
 * Description: This service wraps Template data access to keep the TemplateController as thin as possible. This way we rely on a service to 
 *  interact with the DB context and separate the data access concerns from the controller. This has the added benefit of making the controller
 *  easier to test because we can inject a mock service instead of having to mock the entire database context. 
 *  
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using SignRequestExpressAPI.Entities;
using SignRequestExpressAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Services
{
    public interface ITemplateService
    {
        Task<Template> GetTemplateAsync(
            Guid id,
            CancellationToken ct);

        Task<PagedResults<Template>> GetTemplatesAsync(
            PagingOptions pagingOptions,
            SortOptions<Template, TemplateEntity> sortOptions,
            SearchOptions<Template, TemplateEntity> searchOptions,
            CancellationToken ct);

        Task<Guid> GetTemplateBrandKeyAsync(
            Guid? templateId,
            CancellationToken ct);
    }
}
