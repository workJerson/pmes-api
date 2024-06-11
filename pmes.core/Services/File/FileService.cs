using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using pmes.core.Common.Exceptions;
using pmes.core.Common.Models.Response;
using pmes.core.Services.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.File
{
    public interface IFileService
    {
        Task<GenericResponse<string>> Upload(IFormFile file);
        Task<GenericResponse<List<string>>> MultipleUpload(IFormFile[] files);
        Task<GenericResponse<string>> GetPreSignedUrl(string key);
        Task<GenericResponse<List<string>>> MultiplePresign(string[] paths);
    }

    public class FileService(IS3Service s3Service, IConfiguration configuration) : IFileService
    {
        private readonly IS3Service s3Service = s3Service;
        private readonly string s3Region = configuration["AWS:S3:Region"]!;
        private readonly string s3Bucket = configuration["AWS:S3:BucketName"]!;
        private readonly IConfiguration configuration = configuration;

        public async Task<GenericResponse<string>> GetPreSignedUrl(string key)
        {
            var response = await s3Service.GetPreSignedUrl(key);

            return new GenericResponse<string>(response, "Pre-signing success.", StatusCodes.Status200OK);
        }

        public async Task<GenericResponse<string>> Upload(IFormFile file)
        {
            var (response, fileName) = await s3Service.UploadFile(file);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return new GenericResponse<string>(null, $"Error on uploading content: {response.HttpStatusCode}", (int)response.HttpStatusCode);

            var fileUrl = $"https://{s3Bucket}.s3.{s3Region}.amazonaws.com/{fileName}";

            return new GenericResponse<string>(fileUrl, "File uploaded.", (int)response.HttpStatusCode);
        }

        public async Task<GenericResponse<List<string>>> MultiplePresign(string[] paths)
        {
            if (paths.Length <= 0)
                throw new CustomException(message: "No provided file urls", statusCode: System.Net.HttpStatusCode.BadRequest);

            List<string> presignedUrls = [];

            foreach (var path in paths)
            {
                var presignedUrl = await s3Service.GetPreSignedUrl(path);
                presignedUrls.Add(presignedUrl);
            }

            return new GenericResponse<List<string>>(presignedUrls, "File URLs presigned successfully", StatusCodes.Status200OK);
        }

        public async Task<GenericResponse<List<string>>> MultipleUpload(IFormFile[] files)
        {
            if (files.Length <= 0)
                throw new CustomException(message: "No selected files.", statusCode: System.Net.HttpStatusCode.BadRequest);

            List<string> urls = [];

            foreach (var file in files)
            {
                var (response, fileName) = await s3Service.UploadFile(file);
                var fileUrl = $"https://{s3Bucket}.s3.{s3Region}.amazonaws.com/{fileName}";
                urls.Add(fileUrl);
            }

            return new GenericResponse<List<string>>(urls, "File uploaded.", StatusCodes.Status200OK);
        }
    }
}
