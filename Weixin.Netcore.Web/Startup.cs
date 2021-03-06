﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Weixin.Netcore.Core.MessageRepet;
using Weixin.Netcore.Cache;
using Weixin.Netcore.Core.Message.Handler;
using Weixin.Netcore.Model.WeixinMessage.Reply;
using Weixin.Netcore.Model.WeixinMessage;
using Weixin.Netcore.Core.Message.Processer;
using Weixin.Netcore.Extensions.Message.Handler;
using Weixin.Netcore.Core.Debug;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Weixin.Netcore.Core.Message.Handler.NotImplement;
using Weixin.Netcore.Model;
using Weixin.Netcore.Core.MaintainContainer;
using Weixin.Netcore.Core.InterfaceCaller;
using Weixin.Netcore.Model.Enums;
using Weixin.Netcore.Core.Middleware;

namespace Weixin.Netcore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region 缓存模式
            //InMemory缓存
            services.AddMemoryCache();

            //Redis缓存
            //services.AddDistributedRedisCache(opt =>
            //{
            //    string redisHost = Configuration["RedisHost"];
            //    int redisPort = int.Parse(Configuration["RedisPort"] ?? "6379");
            //    string redisPasswd = Configuration["RedisPasswd"];
            //    opt.Configuration = $"{redisHost}:{redisPort}{(string.IsNullOrEmpty(redisPasswd) ? string.Empty : string.Concat(",password=", redisPasswd))}";
            //});
            #endregion

            services.AddMvc();

            #region Autofac
            //Add Autofac
            var builder = new ContainerBuilder();

            #region 注入
            #region 第三方
            //RestSharp
            builder.RegisterType<RestClient>().As<IRestClient>();
            #endregion

            #region 缓存
            //Cache
            //使用InMemory缓存
            builder.RegisterType<InMemoryCache>().As<ICache>();
            //使用Redis缓存
            //builder.RegisterType<RedisCache>().As<ICache>();
            #endregion

            #region 基础设置
            //微信设置
            builder.Register(context => new BaseSettings()
            {
                AppId = Configuration["AppId"],
                AppSecret = Configuration["AppSecret"],
                Token = Configuration["Token"],
                EncodingAESKey = Configuration["EncodingAESKey"]
            }).As<BaseSettings>();
            //调试模式
            builder.Register(context => new DebugMode(false)).As<IDebugMode>();
            //启用消息重复验证
            builder.Register(context => new MessageRepetValidUsage(true)).As<IMessageRepetValidUsage>();
            #endregion

            #region 接口
            //接口调用
            builder.RegisterType<OAuthInterfaceCaller>().As<OAuthInterfaceCaller>();
            builder.RegisterType<MenuInterfaceCaller>().As<MenuInterfaceCaller>();
            #endregion

            #region 容器
            //维护容器
            builder.RegisterType<AccessTokenContainer>().As<AccessTokenContainer>();
            //认证容器
            builder.RegisterType<AuthorizationContainer>().As<AuthorizationContainer>();
            #endregion

            #region 中间件
            //消息中间件
            builder.RegisterType<MessageMiddlePlain>().As<IMessageMiddleware>();
            #endregion

            #region 消息
            //消息重复处理
            builder.RegisterType<MessageRepetHandler>().As<IMessageRepetHandler>();

            //消息回复
            builder.RegisterType<TextMessageReply>().As<IMessageReply<TextMessage>>();
            builder.RegisterType<ImageMessageReply>().As<IMessageReply<ImageMessage>>();
            builder.RegisterType<MusicMessageReply>().As<IMessageReply<MusicMessage>>();
            builder.RegisterType<VoiceMessageReply>().As<IMessageReply<VoiceMessage>>();
            builder.RegisterType<VideoMessageReply>().As<IMessageReply<VideoMessage>>();
            builder.RegisterType<NewsMessageReply>().As<IMessageReply<NewsMessage>>();

            //消息处理器
            //根据需要添加相应的消息类型配置
            builder.RegisterType<NorEvtMessageProcesser>().As<IMessageProcesser>();

            //消息处理
            //在Weixin.Netcore.Extensions项目中
            //namespace：Weixin.Netcore.Extensions.Message.Handler
            //根据需要替换各种消息消息处理类
            builder.RegisterType<ClickEventReplyTextExtension>().As<IClickEvtMessageHandler>();
            builder.RegisterType<EchoTextExtension>().As<ITextMessageHandler>();
            builder.RegisterType<ImageMessageHandlerNotImp>().As<IImageMessageHandler>();
            builder.RegisterType<VoiceMessageHandlerNotImp>().As<IVoiceMessageHandler>();
            builder.RegisterType<VideoMessageHandlerNotImp>().As<IVideoMessageHandler>();
            builder.RegisterType<ShortVideoMessageHandlerNotImp>().As<IShortVideoMessageHandler>();
            builder.RegisterType<LocationMessageHandlerNotImp>().As<ILocationMessageHandler>();
            builder.RegisterType<LinkMessageHandlerNotImp>().As<ILinkMessageHandler>();
            builder.RegisterType<SubscribeEvtMessageHandlerNotImp>().As<ISubscribeEvtMessageHandler>();
            builder.RegisterType<UnsubscribeEvtMessageHandlerNotImp>().As<IUnsubscribeEvtMessageHandler>();
            builder.RegisterType<ScanEvtMessageHandlerNotImp>().As<IScanEvtMessageHandler>();
            builder.RegisterType<LocationEvtMessageHandlerNotImp>().As<ILocationEvtMessageHandler>();
            #endregion
            #endregion

            builder.Populate(services);
            var container = builder.Build();
            return new AutofacServiceProvider(container);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
