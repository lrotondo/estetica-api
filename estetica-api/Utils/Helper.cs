using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Amazon.S3;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3.Transfer;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Amazon.S3.Model;
using SendGrid;
using Newtonsoft.Json;
using static dentist_panel_api.Services.MessagesServices;
using System.Net.Http.Headers;
using System.Text;
using dentist_panel_api.Entities;

namespace dentist_panel_api
{
    public static class Helper    {
        

        internal static bool ValidateToken(HttpContext httpContext, string Username)
        {            
            var identity = httpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = httpContext.User.Claims;            
            var usernameClaim = claim
                .Where(x => x.Type == ClaimTypes.Name)
                .FirstOrDefault();          
            if (Username != usernameClaim.Value)            
                return false;  
            return true;
        }     

        public static AmazonS3Client GetClient(IConfiguration conf)
        {
             AWSCredentials credentials = new BasicAWSCredentials(conf["AWS_AccessKeyID"], conf["AWS_SecretAccessKey"]);
            var Region = RegionEndpoint.GetBySystemName(conf["AWS_Region"]);
            AmazonS3Client s3Client = new AmazonS3Client(credentials, Region);
            return s3Client;
        }

        public async static Task<ActionResult> UploadObjectToBucket(String key, IConfiguration conf, IFormFile file)
        {         

            using (var newMemoryStream = new MemoryStream())
            {
                try
                {
                    file.CopyTo(newMemoryStream);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = key,
                        BucketName = conf["AWS_BucketName"],
                        CannedACL = S3CannedACL.PublicRead,
                        ContentType = file.ContentType,

                    };
                    var fileTransferUtility = new TransferUtility(Helper.GetClient(conf));
                    //if (Helper.ExistsInBucket(client, key, conf["AWS_BucketName"]))
                    //    Helper.Delete(client, key, conf["AWS_BucketName"]);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
                catch (AmazonS3Exception s3Ex)
                {
                    int error = 0;
                }
                catch (Exception ex)
                {
                    int error = 1;
                }
                return new OkResult();
            }            
        }

        

        public static async Task<Stream> ReadObjectFromBucket(String Key, IConfiguration conf)
        {
            try
            {
                AmazonS3Client client = Helper.GetClient(conf);
                using (client)
                {
                    var request = new GetObjectRequest
                    {
                        BucketName = conf["AWS_BucketName"],
                        Key = Key
                    };
                    using (var getObjectResponse = await client.GetObjectAsync(request))
                    {
                        using (var responseStream = getObjectResponse.ResponseStream)
                        {
                            var stream = new MemoryStream();
                            await responseStream.CopyToAsync(stream);
                            stream.Position = 0;
                            return stream;
                        }
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                // If bucket or object does not exist
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool ExistsInBucket(String Key, IConfiguration conf)//AmazonS3Client client, string key, string bucketName)
        {
            AmazonS3Client client = Helper.GetClient(conf);
            using (client)
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = conf["AWS_BucketName"],
                    Key = Key
                };
                using (var getObjectResponse = client.GetObjectMetadataAsync(request))
                {
                    if (getObjectResponse != null)
                        return true;
                }
                return false;
            }
        }

        public static void DeleteObjectFromBucket(String key, IConfiguration conf)
        {
            AmazonS3Client client = Helper.GetClient(conf);
            using (client)
            {
                DeleteObjectRequest request = new DeleteObjectRequest();
                request.BucketName = conf["AWS_BucketName"];
                request.Key = key;
                client.DeleteObjectAsync(request);
            }
        }

        
    }
}
