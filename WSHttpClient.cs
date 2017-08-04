using ClassLibrary2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebServicesTest
{
    public class WSHttpClient
    {
        public void Test(int choix)

        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            if (choix == 1)
            {
                var response = client.GetAsync("http://localhost:52484/api/Product").Result;
                Console.WriteLine(response);
            }
            else if(choix==2)
            {
                var response = client.GetAsync("http://localhost:52484/api/OrderJson").Result;
                Console.WriteLine(response);
            }
            else if (choix == 3)
            {
                OrderSet order = new OrderSet();
                order.Date = DateTime.Now;
                order.IsCanceled = false;
                order.Note = "Commande de produits";
                order.SupplierNumber = "45114";
                order.TotalOfOrderLine = 2;
                StringContent content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                
                var response = client.PostAsync("http://localhost:52484/api/OrderJson",content).Result;
                Console.WriteLine(response);

            }

        }
    }
}
