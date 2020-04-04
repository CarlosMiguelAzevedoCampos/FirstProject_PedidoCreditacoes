﻿using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services.Service
{
    public class HttpRequest : IHttpRequest
    {
        private readonly ILog _log;
        public HttpRequest(ILog log)
        {
            _log = log;
        }

        public async Task<bool> PostNewCardAsync(CardDto card)
        {
            try
            {
                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(card);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage request = await client.PostAsync(ReturnApiUrl(), stringContent);
                return request.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
            }
            return false;
        }

        public string ReturnApiUrl()
        {
            var builder = new ConfigurationBuilder()
                                      .SetBasePath(Directory.GetCurrentDirectory()) // Directory where the json files are located
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();

            IConfiguration Configuration = builder.Build();
            return Configuration["Trello:Uri"].ToString();
        }
    }
}