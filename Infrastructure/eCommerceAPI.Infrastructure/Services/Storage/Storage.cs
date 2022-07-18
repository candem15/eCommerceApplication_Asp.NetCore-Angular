using eCommerceAPI.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected async Task<string> FileRenameAsync(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string oldFileName = Path.GetFileNameWithoutExtension(fileName);
            string newFileName = $"{NameOperation.CharRegulatory(oldFileName)}{Guid.NewGuid()}{extension}";

            return await Task.FromResult(newFileName);
        }
    }
}
