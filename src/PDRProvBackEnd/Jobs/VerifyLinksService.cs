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
using PDRProvBackEnd.Services;

namespace PDRProvBackEnd.Jobs
{
    public class VerifyLinksService : IVerifyLinksService
    {
        private IConfiguration _configuration;
        private readonly ILogger<VerifyLinksService> _log;
        private bool _activedExecutionLinks;
        private bool _activedExecutionSocial;
        private IBackgroundJobClient _backgroundJobClient;
       
    }
}
