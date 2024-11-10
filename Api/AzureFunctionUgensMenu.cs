using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net;

namespace Api
{
    public class AzureFunctionUgensMenu
    {
        private readonly ILogger<AzureFunctionUgensMenu> _logger;

        private readonly string[] _dagsretter = new string[]
        {
            "Pizza", "Pasta", "Tacos", "Burger", "Kylling",
            "Frikadeller", "Medister Pølse", "Thai", "Koteletter i fad",
            "Tomat Suppe", "Vegetar Dag", " Ørred med Citron", "SmørreBrød"
        };

        public AzureFunctionUgensMenu(ILogger<AzureFunctionUgensMenu> logger)
        {
            _logger = logger;
        }

        [Function("UgensMenu")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {                    
            var valgteRetter = new HashSet<string>();  

            // Opret array af 7 retter
            var ugensMenu = Enumerable.Range(1, 7).Select(index => new DagensRet
            {                
                DDagensRet = FindTilfældigRet(valgteRetter),
                Dato = DateOnly.FromDateTime(DateTime.Now.AddDays(index)) 
                 
            }).ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(ugensMenu);
            return response;                                
        }

        private string FindTilfældigRet(HashSet<string> valgteRetter)
        {
            while (true)
            {
                var randomDish = _dagsretter[new Random().Next(_dagsretter.Length)];
                if (!valgteRetter.Contains(randomDish))
                {
                    valgteRetter.Add(randomDish);
                    return randomDish;
                }
            }
        }        

        public class DagensRet
        {            
            public DateOnly Dato { get; set; }
            public string DDagensRet { get; set; }           

            public string UgeDag
            {                
                get
                {
                    CultureInfo daDK = new CultureInfo("da-DK");
                    return Dato.ToString("dddd", daDK);                  
                }
            }
        }
    }
}
