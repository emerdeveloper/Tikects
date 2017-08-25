using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tikects.Models;

namespace Tikects.Services
{
    public class ApiService
    {
        #region Constructor
        public ApiService() { }
        #endregion

        //traera una lista de lo que queramos
        public async Task<Response> Get(
            string urlBase, 
            string servicePrefix, 
            string controller,
            string ticketCode) //Get<T> Será reemplazada por Cualquier clase
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);//Dirección base del servicio
                var url = string.Format("{0}{1}{2}", servicePrefix, controller, ticketCode);//servicePrefix = context root //controller Despues del contest root
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)//Si no hubo respuesta
                {
                    return new Response
                    {
                        //TODO: Mejorar  para pintar el mensaje que trae el API
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),//Solo Muestra 
                    };
                }

                var result = await response.Content.ReadAsStringAsync();//leemos la respuesta
                var list = JsonConvert.DeserializeObject<User>(result);//Desearealizamos el List<T> //Sera la lista que le pasaremos
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Post(
            string urlBase, 
            string servicePrefix, 
            string controller,
            User u
            )/*string email,
            string password,
            string userId,
            int action*/
        {
            try
            {
                string request = "";
                if (u.UserId == null || u.UserId.Equals(""))
                {
                    request = @"{Email: '" + u.Email + "', Password: '" + u.Password + "'}";
                }
                else { 
                    string time = $"{DateTime.Now.Year}-{DateTime.Now.ToString("MM")}-{DateTime.Now.ToString("dd")}T{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
                    request = @"{TicketCode: '" + u.TicketCode + "', DateTime: '" + time + "', UserId: " + u.UserId + "}";
                }
                 
                JObject o = JObject.Parse(request);
                request = o.ToString();
                var content = new StringContent(request, Encoding.UTF8, "application /json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<User>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        
    }
}
