﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using eCommerceAPI.Application.Abstractions.Storage.Azure;
using eCommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        BlobContainerClient _blobContainerClient;
        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration.GetSection("Storage")["Azure:ConnectionString"]);
            _configuration = configuration;
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(x => x.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(x => x.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName); //container ismine karşılık nesne oluşturuyoruz.
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer); //Blob containerına erişim izni verildi.

            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string newFileName = await FileRenameAsync(file.FileName);
                BlobClient blobClient = _blobContainerClient.GetBlobClient(newFileName);
                await blobClient.UploadAsync(file.OpenReadStream()); // UploadAsync in overloadlarından bir tanesi stream şeklinde dosyaları alması burda onu kullandık.
                datas.Add((newFileName, $"{_configuration.GetSection("Storage")["Azure:BaseUrl"]}/{containerName}/{newFileName}"));
            }
            return datas;
        }
    }
}
