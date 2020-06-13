using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WEB2Project.Data;
using WEB2Project.Dtos;
using WEB2Project.Helpers;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRentACarRepository _carRepo;
        private readonly IFlightsRepository _avioRepo;
        private readonly IUsersRepository _usersRepo;

        public AuthController(IHttpClientFactory clientFactory, IRentACarRepository carRepo, IFlightsRepository avioRepo,
                              IUsersRepository usersRepo)
        {
            _clientFactory = clientFactory;
            _carRepo = carRepo;
            _avioRepo = avioRepo;
            _usersRepo = usersRepo;
        }

        public string GetAuthorizationToken()
        {
            var client = new RestClient("https://pusgs.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"i1ZqGVSnFuJOSsJxe00MhRp1UZ5CQDlw\",\"client_secret\":\"863wgBE7Yh0KG5TELRqCvoww926UD_5TftkBAY__F2LnSsh3nuB56OjAyI3PqolQ\",\"audience\":\"https://pusgs.eu.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic data = JObject.Parse(response.Content);

            return data.access_token;
        }

        [HttpGet("getUsers")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://pusgs.eu.auth0.com/api/v2/users");

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            List<UserFromServer> usersFromServer = JsonConvert.DeserializeObject<List<UserFromServer>>(toReturn);
            List<User> users = new List<User>();

            foreach (var us in usersFromServer)
            {
                User user = new User();
                user.AuthId = us.user_id;
                user.Email = us.email;

                if (us.user_metadata != null)
                {
                    user.FirstName = us.user_metadata.first_name;
                    user.LastName = us.user_metadata.last_name;
                }
                else
                {
                    user.FirstName = us.given_name;
                    user.LastName = us.family_name;
                }
                users.Add(user);
            }
            return Ok(users);

        }

        [HttpPost("getUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] EmailDto data)
        {
            var token = GetAuthorizationToken();

            var id = await GetUserId(data.Email);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != id && 
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var userFromRepository = _usersRepo.GetUser(id);

            if(userFromRepository != null)
            {
                return Ok(userFromRepository);
            }
            else
            {
                string requestUri = "https://pusgs.eu.auth0.com/api/v2/users-by-email?email=" + data.Email;
                requestUri.Replace("@", "%40");

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                request.Headers.Add("Authorization", "Bearer " + token);

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                var toReturn = await response.Content.ReadAsStringAsync();

                UserFromServer user = JsonConvert.DeserializeObject<UserFromServer>(toReturn);

                if (user.user_metadata == null)
                {
                    UserMetadata userMetadata = new UserMetadata();
                    user.user_metadata = userMetadata;
                }

                User userToReturn = new User();

                userToReturn.AuthId = user.user_id;
                userToReturn.Email = user.email;
                userToReturn.FirstName = user.user_metadata.first_name;
                userToReturn.LastName = user.user_metadata.last_name;
                userToReturn.City = user.user_metadata.city;
                userToReturn.PhoneNumber = user.user_metadata.phone_number;
                userToReturn.NeedToChangePassword = user.user_metadata.needToChangePassword;

                _usersRepo.Add(userToReturn);
                await _usersRepo.SaveAll();

                return Ok(userToReturn);
            }
        }

        public async Task<IActionResult> NeedToChangePassword(string userId, bool flag)
        {
            var token = GetAuthorizationToken();

            var uri = "https://pusgs.eu.auth0.com/api/v2/users/" + userId;

            uri = uri.Replace("|", "%7C");

            var request = new HttpRequestMessage(HttpMethod.Patch, uri);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            string body;
            
            if(flag)
            {
               body = "{ \"user_metadata\" : { \"needToChangePassword\": true} }";
            }
            else
            {
               body = "{ \"user_metadata\" : { \"needToChangePassword\": false} }";
            }
            

            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var user = _usersRepo.GetUser(userId);
                if(user != null)
                {
                    user.NeedToChangePassword = flag;
                    await _usersRepo.SaveAll();
                    return Ok();
                }
            }

            return BadRequest("Failed to update user metadata");
        }
     
        [HttpPost("updateUserMetadata")]
        [Authorize]
        public async Task<IActionResult> UpdateUserMetadata(UserToUpdate userToUpdate)
        {
            var token = GetAuthorizationToken();

            var userId = await GetUserId(userToUpdate.email);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userId &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var uri = "https://pusgs.eu.auth0.com/api/v2/users/" + userId;

            uri = uri.Replace("|", "%7C");

            var request = new HttpRequestMessage(HttpMethod.Patch, uri);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var metadataInJson = JsonConvert.SerializeObject(userToUpdate.user_metadata);

            string body = "{\"user_metadata\":" + metadataInJson + "}";

            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var response = await client.SendAsync(request);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var user = _usersRepo.GetUser(userId);
                user.City = userToUpdate.user_metadata.city;
                user.PhoneNumber = userToUpdate.user_metadata.phone_number;
                user.FirstName = userToUpdate.user_metadata.first_name;
                user.LastName = userToUpdate.user_metadata.last_name;

                await _usersRepo.SaveAll();

                return Ok();
            }

            return BadRequest("Failed to update user metadata");
        }

        [HttpPost("updatePassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(UpdatePassword updatePassword)
        {
            var token = GetAuthorizationToken();

            var userId = await GetUserId(updatePassword.email);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userId &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var uri = "https://pusgs.eu.auth0.com/api/v2/users/" + userId;

            uri = uri.Replace("|", "%7C");

            var request = new HttpRequestMessage(HttpMethod.Patch, uri);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            string body = "{\"password\": \"" + updatePassword.password + "\"}";

            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                User user = await GetUserByEmail(updatePassword.email);

                if (!user.NeedToChangePassword)
                    return Ok();
                else
                {
                    await NeedToChangePassword(userId, false);
                    return Ok();
                }
            }

            throw new Exception("Failed to update user password");
        }

        [HttpPost("createAdminUser")]
        [Authorize]
        public async Task<IActionResult> CreateAdminUser(CompanyAdmin userFromSpa)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pusgs.eu.auth0.com/api/v2/users");

            request.Headers.Add("Authorization", "Bearer " + token);

            UserToCreate userToCreate = new UserToCreate();

            userToCreate.email = userFromSpa.Email;
            userToCreate.email_verified = false;
            userToCreate.given_name = userFromSpa.FirstName;
            userToCreate.family_name = userFromSpa.LastName;
            userToCreate.name = userFromSpa.FirstName + " " + userFromSpa.LastName;
            userToCreate.connection = "Username-Password-Authentication";
            userToCreate.password = userFromSpa.Password;

            var client = _clientFactory.CreateClient();

            var userInJson = JsonConvert.SerializeObject(userToCreate);

            var stringContent = new StringContent(userInJson, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var response = await client.SendAsync(request);

            HttpStatusCode statusCode = response.StatusCode;

            if(statusCode == HttpStatusCode.Created)
            {
                var toReturn = await response.Content.ReadAsStringAsync();

                UserFromServer user = JsonConvert.DeserializeObject<UserFromServer>(toReturn);
                if (user.user_metadata == null)
                {
                    UserMetadata userMetadata = new UserMetadata();
                    user.user_metadata = userMetadata;
                }

                await NeedToChangePassword(user.user_id, true);
                string roleId = await CreateRole(userFromSpa.CompanyId.ToString(), userFromSpa.Type);
                HttpStatusCode statusCode2 = await AssignRole(roleId, user.user_id);

                if (statusCode2 == HttpStatusCode.OK)
                {
                    if(userFromSpa.Type.ToLower() == "car")
                    {
                        var companyFromRepo = await _carRepo.GetCompany(userFromSpa.CompanyId);
                        User admin = new User();

                        admin.AuthId = user.user_id;
                        admin.Email = user.email;
                        admin.FirstName = user.given_name;
                        admin.LastName = user.family_name;
                        admin.City = user.user_metadata.city;
                        admin.PhoneNumber = user.user_metadata.phone_number;
                        admin.NeedToChangePassword = true;

                        companyFromRepo.Admin = admin;

                        await _carRepo.SaveAll();
                    }
                    else
                    {
                        var companyFromRepo = _avioRepo.GetCompany(userFromSpa.CompanyId);
                        User admin = new User();

                        admin.AuthId = user.user_id;
                        admin.Email = user.email;
                        admin.FirstName = user.given_name;
                        admin.LastName = user.family_name;
                        admin.City = user.user_metadata.city;
                        admin.PhoneNumber = user.user_metadata.phone_number;
                        admin.NeedToChangePassword = true;

                        companyFromRepo.Admin = admin;

                        await _avioRepo.SaveAll();
                    }
                    return Ok();
                }
                    
                else
                    throw new Exception("Error while creating new admin user");
            }
            else
                throw new Exception("Error while creating new admin user");

        }

        public async Task<string> CreateRole(string companyId, string type)
        {
            RoleToCreate role = new RoleToCreate();
            role.description = "Manage Company With Id:" + companyId;

            if(type.ToLower() == "car")
            {
                role.name = "managerCarNo" + companyId;
            }
            else
            {
                role.name = "managerAvioNo" + companyId;
            }

            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pusgs.eu.auth0.com/api/v2/roles");

            request.Headers.Add("Authorization", "Bearer " + token);
            var roleInJson = JsonConvert.SerializeObject(role);

            var stringContent = new StringContent(roleInJson, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            UserRole roleToReturn = JsonConvert.DeserializeObject<UserRole>(toReturn);

            return roleToReturn.id;
        }

        [HttpPost("getUserRole")]
        public async Task<IActionResult> GetUserRoles([FromBody] EmailDto data)
        {
            string userId = await GetUserId(data.Email);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userId && 
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var user = _usersRepo.GetUser(userId);

            if(user != null && user.Role != null)
            {
                return Ok(user.Role);
            }
            else if(user == null)
            {
                user = await GetUserByEmail(data.Email);
                _usersRepo.Add(user);

                var userRole = await LoadRoleFromAuth0(userId);
                user.Role = userRole;

                await _usersRepo.SaveAll();

                return Ok(userRole);
            }
            else
            {
                var userRole = await LoadRoleFromAuth0(userId);
                user.Role = userRole;

                await _usersRepo.SaveAll();

                return Ok(userRole);
            }
 
        }

        public async Task<UserRole> LoadRoleFromAuth0(string userId)
        {
            string uriString = "https://pusgs.eu.auth0.com/api/v2/users/" + userId + "/roles";
            uriString = uriString.Replace("|", "%7C");

            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Get, uriString);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            toReturn = toReturn.Substring(1, toReturn.Length - 2);

            UserRole role = JsonConvert.DeserializeObject<UserRole>(toReturn);

            return role;
        }

        public async Task<HttpStatusCode> AssignRole(string roleId, string userId)
        {
            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pusgs.eu.auth0.com/api/v2/roles/" + roleId + "/users");

            request.Headers.Add("Authorization", "Bearer " + token);

            string body = "{\"users\": [\"" + userId + "\"]}";
         
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = response.StatusCode;

            return toReturn;
        }

        public async Task<string> GetUserId(string email)
        {
            var token = GetAuthorizationToken();

            string requestUri = "https://pusgs.eu.auth0.com/api/v2/users-by-email?email=" + email;
            requestUri.Replace("@", "%40");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            List<UserFromServer> users = JsonConvert.DeserializeObject<List<UserFromServer>>(toReturn);

            UserFromServer user = users.First();
           
            return user.user_id;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var token = GetAuthorizationToken();

            string requestUri = "https://pusgs.eu.auth0.com/api/v2/users-by-email?email=" + email;
            requestUri.Replace("@", "%40");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            List<UserFromServer> users = JsonConvert.DeserializeObject<List<UserFromServer>>(toReturn);

            UserFromServer user = users.First();
            if (user.user_metadata == null)
            {
                UserMetadata userMetadata = new UserMetadata();
                user.user_metadata = userMetadata;
            }

            User userToReturn = new User();

            userToReturn.AuthId = user.user_id;
            userToReturn.Email = user.email;
            userToReturn.FirstName = user.user_metadata.first_name;
            userToReturn.LastName = user.user_metadata.last_name;
            userToReturn.City = user.user_metadata.city;
            userToReturn.PhoneNumber = user.user_metadata.phone_number;
            userToReturn.NeedToChangePassword = user.user_metadata.needToChangePassword;

            return userToReturn;
        }

    }
}