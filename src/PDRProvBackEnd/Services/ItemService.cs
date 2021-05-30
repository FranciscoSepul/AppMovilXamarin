using Hangfire;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PDRProvBackEnd.Models;
using PDRProvBackEnd.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDRProvBackEnd.DTOModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Reflection;
using PDRProvBackEnd.Builders;

namespace PDRProvBackEnd.Services
{
    public class ItemService 
    {       
        private IConfiguration _configuration;
        private readonly ILogger<ItemService> _log;
        private MemoryCacheEntryOptions policyCache;
        private readonly IAppCache _cache;
        private const string pf = "ii_";
        private IBackgroundJobClient _backgroundJobClient;   


    }
}
