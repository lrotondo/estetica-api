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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace dentist_panel_api.Services
{
    public class AWSBucketServices : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private String BucketName;
        private AmazonS3Client s3Client;

        public AWSBucketServices(IConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));            
            AWSCredentials credentials = new BasicAWSCredentials(_configuration["AWS_AccessKeyID"], _configuration["AWS_SecretAccessKey"]);
            var region = RegionEndpoint.GetBySystemName(_configuration["AWS_Region"]);
            s3Client = new AmazonS3Client(credentials, region);
            BucketName = _configuration["AWS_BucketName"];
        }

        public async Task<ActionResult> UploadObject(String username, UploadObject model, IFormFile file)
        {
            //Helper.UploadObjectToBucket(username+"/"+model.Name, _configuration, file);
            Helper.UploadObjectToBucket(model.Name, _configuration, file);
            return Ok();
        }        

        public async Task<Stream> ReadObjectData(String Key)
        {
            return await Helper.ReadObjectFromBucket(Key, _configuration);
        }        

        public void DeleteObjectNonVersionedBucketAsync(String username, String key)
        {
            try
            {
                using (s3Client)
                {
                    var deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = BucketName,
                        Key = username + "/" + key
                    };
                    Console.WriteLine("Deleting an object");
                    s3Client.DeleteObjectAsync(deleteObjectRequest);                    
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
                
            }            
        }

        public async Task<ActionResult<List<BucketObjectDTO>>> ListingObjectsAsync(String username)
        {
            List<BucketObjectDTO> list = new List<BucketObjectDTO>();
            try
            {
                ListObjectsRequest request = new ListObjectsRequest();
                request.BucketName = BucketName;
                //request.Prefix = username+"/";   
                do
                {
                    ListObjectsResponse response = await s3Client.ListObjectsAsync(request);
                    foreach (S3Object entry in response.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}",
                            entry.Key, entry.Size);
                        var keyValue = entry.Key.Replace(username + "/", "");
                        list.Add(new BucketObjectDTO() { Key = keyValue, Size = entry.Size, Description = "Description" });
                    }
                    if (response.IsTruncated)
                    {
                        request.Marker = response.NextMarker;
                    }
                    else
                    {
                        request = null;
                    }
                } while (request != null);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return new OkObjectResult(list);

            
        }
    }
}
