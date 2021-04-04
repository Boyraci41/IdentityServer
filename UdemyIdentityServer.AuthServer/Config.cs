using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Components.Forms;

namespace UdemyIdentityServer.AuthServer
{
    public static class Config
    {
        #region Client-Server Bağlantısı

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return  new List<ApiResource>()
            {
                new ApiResource("resource_api1"){Scopes = {"api1.read","api1.write","api1.update"},
                    ApiSecrets = new[]{new Secret("secretapi1".Sha256()), }},
                new ApiResource("resource_api2")
                {
                    Scopes = {"api2.read","api2.write","api2.update"},
                    ApiSecrets = new[]{new Secret("secretapi2".Sha256()), }
                },
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","API 1 için güncelleme izni"),
                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni"),

            };
        }

        public static IEnumerable<Client> GEtClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "Client1",
                    ClientName = "Client 1  uygulamasi",
                    ClientSecrets = new[] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"api1.read","api1.write","api2.update"}

                },
                new Client()
                {
                    ClientId = "Client2",
                    ClientName = "Client 2 uygulamasi",
                    ClientSecrets = new[] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"api2.read","api2.write","api2.update"}

                },
                new Client()
                {
                    ClientId = "Client1-Mvc",
                    ClientName = "Client 1 MVC uygulamasi",
                    RequirePkce = false, //pkce spa ve mobil clientler için önemli
                    ClientSecrets = new[] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid, // Code ve id_token kullanıldığı için 
                    RedirectUris = new List<string>(){ "https://localhost:5006/signin-oidc" }, // Auth serverin clientın hangi uri sine gönderecek
                    PostLogoutRedirectUris = new List<string>(){"https://localhost:5006/signout-callback-oidc" },
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,"api1.read",
                        IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"}, //Üyelik Olduğu için , OpenId = userId si , Profile = ismi ve diğer detaylar, offline acces refresh token için
                    AccessTokenLifetime = 2*3600,
                    AllowOfflineAccess = true, //Refresh Token kullanımak istiyor
                    RefreshTokenUsage = TokenUsage.ReUse, // Refresh Token sadece birden fazla kullanılır.

                  //  RequireConsent = true // Onay ekranı için (hangi claimleri kullanıcak)

                },
                new Client()
                {
                    ClientId = "Client2-Mvc",
                    ClientName = "Client 2 MVC uygulamasi",
                    RequirePkce = false, //pkce spa ve mobil clientler için önemli
                    ClientSecrets = new[] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid, // Code ve id_token kullanıldığı için 
                    RedirectUris = new List<string>(){ "https://localhost:5011/signin-oidc" }, // Auth serverin clientın hangi uri sine gönderecek
                    PostLogoutRedirectUris = new List<string>(){"https://localhost:5011/signout-callback-oidc" },
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,"api1.read",
                        IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"}, //Üyelik Olduğu için , OpenId = userId si , Profile = ismi ve diğer detaylar, offline acces refresh token için
                    AccessTokenLifetime = 2*3600,
                    AllowOfflineAccess = true, //Refresh Token kullanımak istiyor
                    RefreshTokenUsage = TokenUsage.ReUse, // Refresh Token sadece birden fazla kullanılır.

                    //  RequireConsent = true // Onay ekranı için (hangi claimleri kullanıcak)

                }
            };
        }

        #endregion


        #region Üyelik Sistemi İçin
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
         
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(), // Muhakak kullanıcı id olması lazım. subjectId
                new IdentityResources.Profile(), // Kullanıcı Hakkında extra bilgiler tutar
                new IdentityResource(){Name = "CountryAndCity",DisplayName = "Country and City",
                    Description = "Kullanıcının ülke ve şehir bilgisi",UserClaims = new []{"country","city"}},
                new IdentityResource(){Name = "Roles",DisplayName = "Role",Description = "Kullanıcı Rolleri",UserClaims = new []{"role"}}
            };
        }

        public static List<TestUser> GetUsers()
        {
            return  new List<TestUser>()
            {
                new TestUser(){SubjectId ="1",Username = "fcakiroglu",Password = "password",Claims = new List<Claim>()
                {
                    new Claim("given_name","Fatih"),
                    new Claim("family_name","boyraci"),
                    new Claim("country","Türkiye"),
                    new Claim("city","Gebze"),
                    new Claim("role","Admin")

                }},
                new TestUser(){SubjectId ="2",Username = "mucahit55",Password = "password",Claims = new List<Claim>()
                {
                    new Claim("given_name","mucahit"),
                    new Claim("family_name","boyraci"),
                    new Claim("country","Türkiye"),
                    new Claim("city","Gebze"),
                    new Claim("role","Customer")

                }}
            };
        }
        

        #endregion
        
    }
}
