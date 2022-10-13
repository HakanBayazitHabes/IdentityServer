using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        private HttpClient _client;
        public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = new HttpClient();
            _configuration = configuration;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            //GetTokenAsync ile AccessToken alabiliriz
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            _client.SetBearerToken(accessToken);

            throw new NotImplementedException();
        }


        public async Task<List<string>> SaveUserViewModel(UserSaveViewModel userSaveViewModel)
        {
            //Bu kısımda IdentityServer'ımıza erişiyoruz
            var disco = await _client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);


            if (disco.IsError)
            {
                //loglama yap
            }

            //IdentityServer-IdentityAPI.AuthServer içerindeki UserController içerindeki SignUp'a istk yapabilmek için token lazım bunu da aşağıdaki belirleyeceğimiz id,secret ve address ile almış olazağız

            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();

            clientCredentialsTokenRequest.ClientId = _configuration["ClientResourceOwner:ClientId"];
            clientCredentialsTokenRequest.ClientSecret = _configuration["ClientResourceOwner:ClientSecret"];
            clientCredentialsTokenRequest.Address = disco.TokenEndpoint;

            //Token'ı aldığımız kısım

            var token = await _client.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (token.IsError)
            {
                //Eğer buraya girmişse token'ı alamamışsınızdır
                //loglama yap
            }

            //İstek yapılan Datayı json tipini çevirdik
            //Elimizde artık request'in body'si var

            var stringContent = new StringContent(JsonConvert.SerializeObject(userSaveViewModel), Encoding.UTF8, "application/json");

            //Göndermiş olduğumuz request'in Authorization'a bu token'ı ekleyeceğiz. Yani Token'imizi Set edeceğiz

            _client.SetBearerToken(token.AccessToken);

            //aşağıdaki metodun 2. argümanında HttpContent alıyor . Yukarıda(stringContent) tanımladığımız StringContent ile HtttpContent aynı interface'den implemente ediliyor. Dolayısıyla StringContent gönderebiliriz
            //burada IdentityServer-IdentityAPI.AuthServer veri mizi api ile göndermiş oluyoruz

            var response = await _client.PostAsync("https://localhost:5001/api/user/signup", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorList = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());

                return errorList;
            }

            return null;









        }



    }
}
