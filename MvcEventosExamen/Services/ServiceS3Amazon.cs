using Amazon.S3;
using Amazon.S3.Model;
using MvcEventosExamen.Helpers;

namespace MvcEventosExamen.Services
{
    public class ServiceS3Amazon
    {
        private IAmazonS3 ClientS3;

        public ServiceS3Amazon(IAmazonS3 clientS3)
        {
            this.ClientS3 = clientS3;
        }

        public async Task<bool> UploadFileAsync(string fileName, Stream stream)
        {
            string bucketName = await HelperSecretManager.GetSecretAsync("BucketNameExamen");

            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = bucketName
            };

            PutObjectResponse response = await this.ClientS3.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
