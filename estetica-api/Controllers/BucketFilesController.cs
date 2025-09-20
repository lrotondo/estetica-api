using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using dentist_panel_api.DTO;
using dentist_panel_api.Entities;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace dentist_panel_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketFilesController : ControllerBase
    {
        public AWSBucketServices _awsBucketServices;
        private readonly UserServices _userServices;

        public BucketFilesController(AWSBucketServices awsBucketServices, UserServices userServices)
        {
            this._awsBucketServices = awsBucketServices;
            this._userServices = userServices;
        }
        /// <summary>
        /// upload a file to S3 bucket      
        /// </summary>
        /// <param name="username"></param>
        /// <param name="model"></param>
        /// <param name="file"></param>
        /// <returns>S3 file key</returns>  
        [HttpPost("{username}")]
        public async Task<ActionResult> UploadObject(String username, [FromForm] UploadObject model, IFormFile photo)
        {
            ApplicationUser user = await _userServices.GetCurrentUser(this.User);
            return await _awsBucketServices.UploadObject(user.Id, model, photo);
            //return OkResult();      
        }        
        /// <summary>
        /// get a file from S3 bucket      
        /// </summary>
        /// <param name="username"></param>
        /// <param name="key"></param>
        /// <returns>the file</returns>  
        [HttpGet("{username}/image/{key}")]
        public async Task<ActionResult<Stream>> ReadObjectData(String username, String key)
        {            
            Stream stream = await _awsBucketServices.ReadObjectData(username + "/" + key);
            return new FileStreamResult(stream, new MediaTypeHeaderValue("image/jpeg"))
            {
                FileDownloadName = key + ".jpg"
            };            
        }
        /// <summary>
        /// delete a file from S3 bucket      
        /// </summary>
        /// <param name="username"></param>
        /// <param name="key"></param>
        [HttpDelete("{username}/image/{key}")]
        public void DeleteObject(String username, String key)
        {
            _awsBucketServices.DeleteObjectNonVersionedBucketAsync(username, key);            
        }
        /// <summary>
        /// get all user's files from S3 bucket      
        /// </summary>
        /// <param name="username"></param>
        /// <returns>all the files</returns>  
        [HttpGet("{username}")]
        public async Task<ActionResult<List<BucketObjectDTO>>> ListingObjectsAsync(String username){
            ActionResult<List<BucketObjectDTO>> result = await _awsBucketServices.ListingObjectsAsync(username);
            return result;
        }
    }
}
