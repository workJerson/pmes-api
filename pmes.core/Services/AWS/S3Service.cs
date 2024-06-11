using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.AWS
{
    public interface IS3Service
    {
        Task<(PutObjectResponse, string)> UploadFile(IFormFile file, bool isPublic = false);
        Task<(PutObjectResponse, string)> UploadFile(MemoryStream file, string fileExtension, bool isPublic = false);
        Task<string> GetPreSignedUrl(string key);
    }
    public class S3Service(IConfiguration configuration) : IS3Service
    {
        private readonly string s3Region = configuration["AWS:S3:Region"]!;
        private readonly string s3Bucket = configuration["AWS:S3:BucketName"]!;
        private readonly IAmazonS3 s3Client = new AmazonS3Client(RegionEndpoint.GetBySystemName(configuration["AWS:S3:Region"]));
        private readonly double preSignedUrlDuration = double.Parse(configuration["AWS:S3:PreSignUrlDurationInDays"]!);

        public async Task<string> GetPreSignedUrl(string key)
        {
            var signedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = s3Bucket,
                Key = key,
                Expires = DateTime.UtcNow.AddHours(preSignedUrlDuration)
            };

            var result = await s3Client.GetPreSignedURLAsync(signedUrlRequest);

            return result;
        }

        public async Task<(PutObjectResponse, string)> UploadFile(IFormFile file, bool isPublic = false)
        {
            var fileBytes = new byte[file.Length];
            await file.OpenReadStream().ReadAsync(fileBytes.AsMemory(0, int.Parse(file.Length.ToString())));

            var key = $"global/{Guid.NewGuid()}{file.FileName}".Trim();

            await using var stream = new MemoryStream(fileBytes);

            var putRequest = new PutObjectRequest
            {
                BucketName = s3Bucket,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.Private
            };

            if (isPublic)
                putRequest.CannedACL = S3CannedACL.PublicRead;

            var response = await s3Client.PutObjectAsync(putRequest);

            return (response, key);
        }

        public async Task<(PutObjectResponse, string)> UploadFile(MemoryStream file, string fileExtension, bool isPublic = false)
        {
            string pathPrefix = isPublic ? "public" : "global";

            var key = $"{pathPrefix}/{Guid.NewGuid()}.{fileExtension}".Trim();

            var putRequest = new PutObjectRequest
            {
                BucketName = s3Bucket,
                Key = key,
                InputStream = file,
                CannedACL = S3CannedACL.Private
            };

            if (isPublic)
                putRequest.CannedACL = S3CannedACL.PublicRead;

            var response = await s3Client.PutObjectAsync(putRequest);

            return (response, key);
        }
    }
}
