using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Globalization;
using System.Net;

//// testing local culture to danish
//string culture = "da-DK";
//CultureInfo cultureInfo = new CultureInfo(culture);
//Thread.CurrentThread.CurrentUICulture = cultureInfo;

namespace Api
{    
    public class AzureFunctionUgensMenu   {
       
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
                MadRet = FindTilfaeldigRet(valgteRetter),
                Dato = DateOnly.FromDateTime(DateTime.Now.AddDays(index)) 
                 
            }).ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);            
            response.WriteAsJsonAsync(ugensMenu);
            return response;                                
        }

        private string FindTilfaeldigRet(HashSet<string> valgteRetter)
        {
            while (true)
            {
                var tilfældigRet = _dagsretter[new Random().Next(_dagsretter.Length)];
                if (!valgteRetter.Contains(tilfældigRet))
                {
                    valgteRetter.Add(tilfældigRet);
                    return tilfældigRet;
                }
            }
        }        

        public class DagensRet
        {            
            public DateOnly Dato { get; set; }

            // Maybe i can do as i did with dato for culture to work
            public string MadRet { get; set; }
            //public string MadRet
            //{
            //    get
            //    {
            //        CultureInfo daDk = new CultureInfo("da-Dk");
            //        return string.Format(daDk, "{0:G}", MadRet); // ToString("G",daDk);
            //    }
            //    set
            //    {
            //        MadRet = value;
            //    }
            //}

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
