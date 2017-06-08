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
        public async Task<Response> Get<T>(
            string urlBase, 
            string servicePrefix, 
            string controller,
            string ticketCode) //Get<T> Será reemplazada por Cualquier clase
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);//Dirección base del servicio
                var url = string.Format("{0}{1}", servicePrefix, controller);//servicePrefix = context root //controller Despues del contest root
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
                var list = JsonConvert.DeserializeObject<List<T>>(result);//Desearealizamos el List<T> //Sera la lista que le pasaremos
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
            string email,
            string password)
        {
            try
            {
                //var request = "{\nEmail:" + email + ",\nPassword:" + password + "\n}";
                string request = @"{Email: '"+email+ "', Password: '"+password+"'}";
                JObject o = JObject.Parse(request);
                var content = new StringContent(o.ToString(), Encoding.UTF8, "application /json");
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
