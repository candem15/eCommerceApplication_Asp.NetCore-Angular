using eCommerceAPI.Application.Services;
using eCommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnviroment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnviroment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile formFile)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log;
                throw ex;
            }

        }

        private async Task<string> FileRenameAsync(string fileName)
        {

            string extension = Path.GetExtension(fileName);
            string oldFileName = Path.GetFileNameWithoutExtension(fileName);
            string newFileName = $"{NameOperation.CharRegulatory(oldFileName)}{Guid.NewGuid()}{extension}";

            return await Task.FromResult(newFileName);
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnviroment.WebRootPath, path);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(file.FileName);

                bool result = await CopyFileAsync(Path.Combine(uploadPath, fileNewName), file);
                datas.Add((fileNewName, Path.Combine(path, fileNewName)));
                results.Add(result);
                //string fullPath = Path.Combine(uploadPath, Guid.NewGuid() + file.Name);
                if (results.TrueForAll(x => x.Equals(true)))
                {
                    return datas;
                }
                else
                {
                    //todo if condition is not passed. Throw a message to client about file upload process is not successfull and tell unexpected error occurred at server side.
                    return null;
                }
            }


            return null;
        }
    }
}
