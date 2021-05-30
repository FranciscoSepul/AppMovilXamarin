using System;
using Xunit;
using PDRProvBackEnd;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace PDRProvBackEnd.Tests
{
    public class UsersTests : IClassFixture<CustomWebApplicationFactory<PDRProvBackEnd.Startup>>
    {
        private readonly CustomWebApplicationFactory<PDRProvBackEnd.Startup> _factory;

        private const string username = "admin";
        private const string password = "123";
        private HttpClient _client { get; }

        public UsersTests(CustomWebApplicationFactory<PDRProvBackEnd.Startup> factory)  
        {
            _factory = factory;
        }

        [Fact]
        public async Task LoginAdminTest()
        {
            var client = _factory.CreateClient();
            var auth = new PDRProvBackEnd.DTOModels.AuthModel() { Username= "no", Password= "no" };
            StringContent stringContent = null;
            HttpResponseMessage resMsg = null;

            auth.Username += "_";
            stringContent = GetStringContent(auth);
            resMsg = await client.PostAsync("/pawa/api/User/authenticate", stringContent);
            Assert.Equal(HttpStatusCode.Unauthorized, resMsg.StatusCode);

            auth.Password += "_";
            stringContent = GetStringContent(auth);
            resMsg = await client.PostAsync("/pawa/api/User/authenticate", stringContent);
            Assert.Equal(HttpStatusCode.Unauthorized, resMsg.StatusCode);

            auth = new PDRProvBackEnd.DTOModels.AuthModel() { Username = username, Password = password };

            stringContent = GetStringContent(auth);
            resMsg = await client.PostAsync("/pawa/api/User/authenticate", stringContent);
            Assert.Equal(HttpStatusCode.OK, resMsg.StatusCode);

            var userModel = JsonConvert
                .DeserializeObject<DTOModels.UserModel>(await resMsg.Content.ReadAsStringAsync());

        }

        [Fact]
        public async Task GetUserByIdAdminTest()
        {
            var client = _factory.CreateClient();

            StringContent stringContent = null;
            HttpResponseMessage resMsg = null;

            var auth = new PDRProvBackEnd.DTOModels.AuthModel() { Username = username, Password = password };

            stringContent = GetStringContent(auth);
            resMsg = await client.PostAsync("/pawa/api/User/authenticate", stringContent);
            Assert.Equal(HttpStatusCode.OK, resMsg.StatusCode);

            var userModel = JsonConvert
                .DeserializeObject<DTOModels.UserModel>(await resMsg.Content.ReadAsStringAsync());
            
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + userModel.Token);
            resMsg = await client.GetAsync("/pawa/api/User/" + userModel.Id);
            Assert.Equal(HttpStatusCode.OK, resMsg.StatusCode);

            var userModelById = JsonConvert
                .DeserializeObject<DTOModels.UserModel>(await resMsg.Content.ReadAsStringAsync());

            Assert.Equal(userModel.Username, userModelById.Username);

        }

        [Fact]
        public async Task GetRolesByIdAdminTest()
        {
            var client = _factory.CreateClient();

            StringContent stringContent = null;
            HttpResponseMessage resMsg = null;

            var auth = new PDRProvBackEnd.DTOModels.AuthModel() { Username = username, Password = password };

            stringContent = GetStringContent(auth);
            resMsg = await client.PostAsync("/pawa/api/User/authenticate", stringContent);
            Assert.Equal(HttpStatusCode.OK, resMsg.StatusCode);

            var userModel = JsonConvert
                .DeserializeObject<DTOModels.UserModel>(await resMsg.Content.ReadAsStringAsync());

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + userModel.Token);
            resMsg = await client.GetAsync($"/pawa/api/User/{userModel.Id}/roles");
            Assert.Equal(HttpStatusCode.OK, resMsg.StatusCode);
            var roles = JsonConvert
                .DeserializeObject<List<DTOModels.RoleModel>>(await resMsg.Content.ReadAsStringAsync());
            Assert.IsType<List<DTOModels.RoleModel>>(roles);
        }

        private static StringContent GetStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj)
                , System.Text.Encoding.UTF8
                , "application/json");
        }
    }
}
