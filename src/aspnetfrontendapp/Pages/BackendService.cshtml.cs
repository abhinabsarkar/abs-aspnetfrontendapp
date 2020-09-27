using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace aspnetfrontendapp.Pages
{
    public class BackendServiceModel : PageModel
    {
        private readonly ILogger<BackendServiceModel> _logger;
        [BindProperty] public string ApiResponse { get; set; }

        private IConfiguration _configuration;

        public BackendServiceModel(ILogger<BackendServiceModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            string backendServiceUrl = "";
            // flag to check if running locally in a container (not in k8s)
            bool standAloneContainer = true;
            var appConfig = _configuration.GetSection("EnvironmentConfig").Get<EnvironmentConfig>();
            // if running locally in a container which is not in k8s & no configmaps configured
            if (appConfig != null)
            {
                backendServiceUrl = appConfig.BackendServiceUrl;
                standAloneContainer = false;
            }

            // If running in kubernetes
            if (standAloneContainer == false)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = httpClient.GetAsync(backendServiceUrl))
                    {
                        try
                        {
                            ApiResponse = response.Result.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {
                            ApiResponse = ex.Message;
                        }
                    }
                }
            }
        }
    }
}
