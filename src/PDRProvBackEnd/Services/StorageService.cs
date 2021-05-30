using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDRProvBackEnd.DTOModels;
using PDRProvBackEnd.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PDRProvBackEnd.Repository;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azure.Storage;


namespace PDRProvBackEnd.Services
{
    public class StorageService : IStorageService
    {
        private IConfiguration _configuration;
        private readonly ILogger<StorageService> _log;
        private Azure.Storage.Blobs.BlobContainerClient blobContainer;
        public StorageService(IConfiguration configuration, ILogger<StorageService> logger)
        {
            this._configuration = configuration;
            this._log = logger;

            if (string.IsNullOrWhiteSpace(_configuration.GetValue<string>("Storage:StorageConnectionString"))
                || string.IsNullOrWhiteSpace(_configuration.GetValue<string>("Storage:ContainerBlobName")))
            {
                this._log.LogWarning("No se definió conexión a Storage Azure.");
            }
            else
            {
                blobContainer = new Azure.Storage.Blobs.BlobContainerClient(
                    connectionString: _configuration.GetValue<string>("Storage:StorageConnectionString"),
                    blobContainerName: _configuration.GetValue<string>("Storage:ContainerBlobName"));

                try
                {
                    blobContainer.CreateIfNotExists();
                }
                catch (Exception ex)
                {
                    this._log.LogError(ex, "No es posible conectar con Storage Azure.");
                }
            }
        }
        /// <summary>
        /// Sube un archivo a un Container Blob de Azure
        /// </summary>
        /// <param name="blobName">Nombre del archivo a subir.</param>
        /// <param name="msBlobData">Stream del contenido del archivo a subir.</param>
        /// <exception cref="System.ArgumentNullException">blobName</exception>
        /// <exception cref="System.ArgumentNullException">msBlobData</exception>
        /// <exception cref="System.ArgumentException">msBlobData</exception>
        public async Task<Azure.Response> UploadFileAsync(string blobName, System.IO.Stream msBlobData)
        {
            if (string.IsNullOrWhiteSpace(blobName)) throw new ArgumentNullException(nameof(blobName));
            if (msBlobData == null) throw new ArgumentNullException(nameof(msBlobData));
            if (msBlobData.Length == 0) throw new ArgumentException("Stream vacío", nameof(msBlobData));

            try
            {
                _log.LogDebug("Intento de subida de {fileName} con tamaño de {fileSize}", blobName, msBlobData.Length);
                var response = await blobContainer.UploadBlobAsync(blobName: blobName,
                                                    content: msBlobData);
                if (!response.GetRawResponse().Status.ToString().StartsWith("2"))
                {
                    _log.LogError("No subió archivo Status {status} " +
                        "{ReasonPhrase} - blob {blobName}", 
                        response.GetRawResponse().Status,
                        response.GetRawResponse().ReasonPhrase,
                        blobName);
                }
                else
                    _log.LogInformation("Archivo {fileName} subido OK. Tamaño: {fileSize}", blobName, msBlobData.Length);
                return response.GetRawResponse();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error subiendo {fileName} Tamaño: {fileSize}", blobName, msBlobData.Length);
                return null;
            }
        }
    }
}
