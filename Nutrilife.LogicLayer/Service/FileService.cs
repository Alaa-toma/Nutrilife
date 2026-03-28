using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public class FileService :IFileService
    {
        async Task<string?> IFileService.UploadAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var FilePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "images", FileName
                    );

                using (var stream = File.Create(FilePath))
                {
                    await file.CopyToAsync(stream);
                }

                return FileName;
            }
            return null;
        }




    }
}
