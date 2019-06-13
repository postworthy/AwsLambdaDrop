using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Transfer;
using AwsLambdaDrop.Data;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsLambdaDrop
{
    public class Function
    {
        public class jsonresponse
        {
            public bool isBase64Encoded { get; set; } = false;
            public object headers { get; set; } = new object();
            public int statusCode { get; set; } = 200;
            public string body { get; set; }
        }
        private static string LINKED_ACCOUNTS = "sae.linked.accounts";

        public async Task<jsonresponse> FunctionHandler(AwsLinkingRequestObject data, ILambdaContext context)
        {
            try
            {
                var s3Client = new AmazonS3Client(Environment.GetEnvironmentVariable("awsAccessKeyId"), Environment.GetEnvironmentVariable("awsSecretAccessKey"));
                var fileTransferUtility = new TransferUtility(s3Client);
                if (Environment.GetEnvironmentVariable("allowOverride") != "true")
                {
                    try
                    {
                        await s3Client.GetObjectMetadataAsync(new Amazon.S3.Model.GetObjectMetadataRequest() { BucketName = LINKED_ACCOUNTS, Key = data.CallerIdentity.Account });
                        return new jsonresponse { statusCode = 304, body = data.CallerIdentity.Account };
                    }
                    catch { }
                }

                using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data))))
                {
                    await fileTransferUtility.UploadAsync(stream, LINKED_ACCOUNTS, data.CallerIdentity.Account);
                }

                return new jsonresponse { body = data.CallerIdentity.Account };

            }
            catch (Exception ex)
            {
                if(Environment.GetEnvironmentVariable("awsDebug") == "true")
                    return new jsonresponse { statusCode= 500, body = ex.ToString() };
                else
                    return new jsonresponse { statusCode = 500, body = "Something went wrong, set the environment variable awsDebug=true to see what it is." };
            }
        }
    }
}
