//COMMENT

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using SignRequestExpress.Models;
using SignRequestExpress.Models.Azure;
using SignRequestExpress.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SignRequestExpress.Controllers
{
    public class BlobImagesController : Controller
    {
        private readonly IAuthorizationService _authzService;
        private readonly StorageAccountOptions _storageAccountOptions;
        private readonly BlobUtility _blobUtility;


        public BlobImagesController(
            IAuthorizationService authorizationService,
            IOptions<StorageAccountOptions> storageAccountOptions)
        {
            _authzService = authorizationService;
            _storageAccountOptions = storageAccountOptions.Value;
            _blobUtility = new BlobUtility(
                _storageAccountOptions.StorageAccountNameOption,
                _storageAccountOptions.StorageAccountKeyOption);
        }

        public async Task<IActionResult> Index()
        {
            List<IListBlobItem> blobItems = await _blobUtility.GetBlobs("templates", "miller");

            //List<IListBlobItem> blobItems = await _blobUtility.GetBlobs("account-logos");

            return View(blobItems);
        }
    }
}
