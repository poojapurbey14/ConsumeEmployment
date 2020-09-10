using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsumeEmployment
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/");
            client.Timeout = new TimeSpan(0, 2, 0);
            var defaultRequestHeaders = client.DefaultRequestHeaders; 
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("no auth", result.AccessToken);
            CreateEmployee(client);
            var emp = GetEmployeeById(11, client);
            
            
        }

        public static void CreateEmployee(HttpClient client)
        {
            var employee = new Employee();
            employee.FirstName = "RIshi Nayak2";
            employee.LastName = "Kumar";
            employee.Gender = "Male";
            employee.Age = 22;
            employee.Race = "Indiana";
            employee.DateOfBirth = DateTime.Now;
            HttpResponseMessage response = HttpClientExtensions.SendAsJsonAsync<Employee>(client, HttpMethod.Post, "emp/v1/emp/details/new", employee).Result;
        }

        public static Employee GetEmployeeById(int id, HttpClient client)
        {
            var response = client.GetAsync(client.BaseAddress + "emp/v1/emp/details" + "?empId="+ id.ToString()).Result;
            var entity = JsonConvert.DeserializeObject<Employee>(response.Content.ReadAsStringAsync().Result);
            return entity;
        }
    }

    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sends an HTTP message containing a JSON payload to the target URL.  
        /// </summary>
        /// <typeparam name="T">The type of the data to send in the message content (payload).</typeparam>
        /// <param name="client">A preconfigured HTTP client.</param>
        /// <param name="method">The HTTP method to invoke.</param>
        /// <param name="requestUri">The relative URL of the message request.</param>
        /// <param name="value">The data to send in the payload. The data will be converted to a serialized JSON payload.</param>
        /// <returns>An HTTP response message.</returns>
        public static Task<HttpResponseMessage> SendAsJsonAsync<T>(this HttpClient client, HttpMethod method, string requestUri, T value)
        {
            string content = String.Empty;
            if (value.GetType().Name.Equals("JObject"))
                content = value.ToString();
            else
                content = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });

            HttpRequestMessage request = new HttpRequestMessage(method, requestUri);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = client.SendAsync(request);
            return response;
        }
    }

    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Race { get; set; }
        public DateTime DateOfBirth { get; set; }
    }


}
