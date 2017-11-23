﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using Weixin.Netcore.Core.Exceptions;
using Weixin.Netcore.Model.WeixinInterface;

namespace Weixin.Netcore.Core.InterfaceCaller
{
    /// <summary>
    /// 微信接口调用
    /// </summary>
    public class OAuthInterfaceCaller
    {
        #region .ctor
        private readonly IRestClient _restClient;

        #region const
        private const string WeixinUri = "https://api.weixin.qq.com";
        #endregion

        public OAuthInterfaceCaller(IRestClient restClient)
        {
            _restClient = restClient;
            _restClient.BaseUrl = new Uri(WeixinUri);
        }
        #endregion

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public AccessToken GetAccessToken(string appId, string appSecret)
        {
            if(string.IsNullOrEmpty(appId))
            {
                throw new ArgumentException("AppId为空");
            }
            if(string.IsNullOrEmpty(appSecret))
            {
                throw new ArgumentException("AppSecret为空");
            }

            IRestRequest request = new RestRequest("cgi-bin/token", Method.GET);
            request.AddQueryParameter("grant_type", "client_credential");
            request.AddQueryParameter("appid", appId);
            request.AddQueryParameter("secret", appSecret);

            IRestResponse response = _restClient.Execute(request);

            if (!response.Content.Contains("access_token"))
            {
                var err = JsonConvert.DeserializeObject<Error>(response.Content);
                throw new WeixinInterfaceException(err.errmsg);
            }

            return JsonConvert.DeserializeObject<AccessToken>(response.Content);
        }

        /// <summary>
        /// 通过Code获取OpenId
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public OpenId GetOpenId(string appId, string appSecret, string code)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentException("AppId为空");
            }
            if (string.IsNullOrEmpty(appSecret))
            {
                throw new ArgumentException("AppSecret为空");
            }
            if(string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("code为空");
            }

            IRestRequest request = new RestRequest("sns/oauth2/access_token", Method.GET);
            request.AddQueryParameter("appid", appId);
            request.AddQueryParameter("secret", appSecret);
            request.AddQueryParameter("code", code);
            request.AddQueryParameter("grent_type", "authorization_code");

            IRestResponse response = _restClient.Execute(request);

            if (!response.Content.Contains("openid"))
            {
                var err = JsonConvert.DeserializeObject<Error>(response.Content);
                throw new WeixinInterfaceException(err.errmsg);
            }

            return JsonConvert.DeserializeObject<OpenId>(response.Content);
        }
    }
}