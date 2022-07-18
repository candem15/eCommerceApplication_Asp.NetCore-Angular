using eCommerceAPI.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        readonly IStorage storage;
        public StorageService(IStorage storage)
        {
            this.storage = storage;
        }
        public string StorageName { get => storage.GetType().Name; }
        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            await storage.DeleteAsync(pathOrContainerName, fileName);
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            return storage.GetFiles(pathOrContainerName);
        }

        public bool HasFile(string pathOrContainerName, string fileName)
        {
            return storage.HasFile(pathOrContainerName, fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            return await storage.UploadAsync(pathOrContainerName, files);
        }
    }
}
