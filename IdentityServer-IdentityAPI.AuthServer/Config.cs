// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer_IdentityAPI.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                //Hangi apilara  hangi izinler üzerinden erişileceğini belirtir
                new ApiResource("resource_api1"){
                 Scopes={"api1.read" },
                 ApiSecrets=new []{new Secret("secretapi1".Sha256())}
                },
                new ApiResource("resource_api2"){
                 Scopes={ "api2.read","api2.write","api2.update" },
                ApiSecrets=new []{new Secret("secretapi2".Sha256())}
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                //clientların hangi apilere hangi işlemleri gerçekleştireceğini belirtir
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","API 1 için güncelleme izni"),
                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        }

        //1-Token içerisinde neler tutacağım
        //2-Kullanıcıdan hangi bilgileri istiyorum
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.Email(),
                //Subject Id, kullanıcı id , zorunlu 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(){Name="CountryAndCity",DisplayName="Country and City",Description="Kullanıcının ülke ve şehir bilgisi",UserClaims=new []{ "country","city"} },
                new IdentityResource(){Name="Roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims= new []{"role"} }

            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser{SubjectId="1",Username="HakanBayazit",Password="password",Claims=new List<Claim>(){
                    new Claim("given_name","Hakan"),
                    new Claim("family_name","Habeş"),
                    new Claim("country","Türkiye"),
                    new Claim("city","Ankara"),
                    new Claim("role","admin")
                }
                },
                new TestUser{SubjectId="2",Username="HakanBayazit2",Password="password",Claims=new List<Claim>(){
                    new Claim("given_name","Ahmet"),
                    new Claim("family_name","Habeş"),
                    new Claim("role","customer")

                }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "Client1",
                    ClientName="Client 1 app uygulaması",
                    ClientSecrets=new []{ new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={"api1.read" }
                },
                new Client()
                {
                    ClientId = "Client2",
                    ClientName="Client 2 app uygulaması",
                    ClientSecrets=new []{ new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={"api1.read", "api1.update", "api2.write","api2.update"}
                },
                new Client()
                {
                    ClientId = "Client1-Mvc",
                    RequirePkce=false,
                    ClientName="Client 1 app mvc uygulaması",
                    ClientSecrets=new []{ new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.Hybrid,
                    //Autharizationserver dan token bu url dönüş yapar.Bu url otamatikman oluşur(OpenID connect protokolü ile)
                    RedirectUris=new List<string>{"https://localhost:5006/signin-oidc"},
                    //IdentityServer dan çıkış yaptığımızda yönlendirme yapmalyız
                    PostLogoutRedirectUris=new List<string>{ "https://localhost:5006/signout-callback-oidc" },
                    //izinler
                    AllowedScopes={IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId,    IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},

                    AccessTokenLifetime=2*60*60,
                    //refresh tokunu açıyoruz
                    AllowOfflineAccess=true,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RequireConsent = false
                },
                new Client()
                {
                    ClientId = "Client2-Mvc",
                    RequirePkce=false,
                    ClientName="Client 2 app mvc uygulaması",
                    ClientSecrets=new []{ new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.Hybrid,
                    //Autharizationserver dan token bu url dönüş yapar.Bu url otamatikman oluşur(OpenID connect protokolü ile)
                    RedirectUris=new List<string>{"https://localhost:5011/signin-oidc"},
                    //IdentityServer dan çıkış yaptığımızda yönlendirme yapmalyız
                    PostLogoutRedirectUris=new List<string>{ "https://localhost:5011/signout-callback-oidc" },
                    //izinler
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId,    IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},

                    AccessTokenLifetime=2*60*60,
                    //refresh tokunu açıyoruz
                    AllowOfflineAccess=true,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RequireConsent = false
                },
                new Client()
                {
                    ClientId="js-client",
                    RequireClientSecret=false,
                    AllowedGrantTypes=GrantTypes.Code,
                    ClientName="Js Client (Angular)",
                    AllowedScopes={IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId,    IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                    RedirectUris={"http://localhost:4200/callback" },
                    AllowedCorsOrigins={ "http://localhost:4200"},
                    PostLogoutRedirectUris={ "http://localhost:4200"}

                },
                new Client()
                {
                    ClientId = "Client1-ResourcesOwner-Mvc",
                    ClientName="Client 1 app mvc uygulaması",
                    ClientSecrets=new []{ new Secret("secret".Sha256())},
                    //Login olmak için ResourceOwnerPassword kullanıyoruz
                    //ClientCreentials'a sadece controller içindeki endpointlere erişmek için yaptık
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPasswordAndClientCredentials ,//ResourceOwnerPassword burada kast edilen username ve password kullanmak gerekir , yalnız burada username ve password'a gerek duymaz isek tokena ihtiyaç var, daha fazla bilgi için identityserver4 udemy 72.ders
                    AllowedScopes={IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId,    IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles",
                    IdentityServerConstants.LocalApi.ScopeName},
                    AccessTokenLifetime=2*60*60,
                    //refresh tokunu açıyoruz
                    AllowOfflineAccess=true,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds
                }

            };
        }

    }
}