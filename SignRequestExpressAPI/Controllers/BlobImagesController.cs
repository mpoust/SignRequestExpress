//COMMENT

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignRequestExpressAPI.Infrastructure;
using SignRequestExpressAPI.Models;
using SignRequestExpressAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SignRequestExpressAPI.Controllers
{
    // "/blobimages"
    [Route("/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BlobImagesController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthorizationService _authzService;
        private readonly PagingOptions _defaultPagingOptions;
        private readonly StorageAccountOptions _storageAccountOptions;
        private readonly BlobUtility _blobUtility;


        public BlobImagesController(
            IAccountService accountService,
            IAuthorizationService authorizationService,
            IOptions<PagingOptions> defaultPagingOptions,
            IOptions<StorageAccountOptions> storageAccountOptions)
        {
            _accountService = accountService;
            _authzService = authorizationService;
            _defaultPagingOptions = defaultPagingOptions.Value;
            _storageAccountOptions = storageAccountOptions.Value;
            _blobUtility = new BlobUtility(
                _storageAccountOptions.StorageAccountNameOption,
                _storageAccountOptions.StorageAccountKeyOption);
        }
     
        // TODO: create Azure WebJob to resize images as they are uploaded.

        // TODO: create UploadBlod
    }
}
