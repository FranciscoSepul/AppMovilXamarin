using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Services
{
    public interface IStorageService
    {
        Task<Azure.Response> UploadFileAsync(string blobName, System.IO.Stream msBlobData);
    }
}
