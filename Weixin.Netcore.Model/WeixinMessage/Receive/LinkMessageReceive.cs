﻿using System.Collections.Generic;
using Weixin.Netcore.Utility;

namespace Weixin.Netcore.Model.WeixinMessage.Receive
{
    /// <summary>
    /// 链接消息接收
    /// </summary>
    public class LinkMessageReceive : IMessageReceive<LinkMessage>
    {
        public LinkMessage GetEntity(string xml)
        {
            var dic = UtilityHelper.Xml2Dictionary(xml);
            return new LinkMessage()
            {
                ToUserName = dic["ToUserName"],
                FromUserName = dic["FromUserName"],
                CreateTime = long.Parse(dic["CreateTime"]),
                MsgId = long.Parse(dic["MsgId"]),
                Title = dic["Title"],
                Description = dic["Description"],
                Url = dic["Url"]
            };
        }

        public LinkMessage GetEntity(Dictionary<string, string> dic)
        {
            return new LinkMessage()
            {
                ToUserName = dic["ToUserName"],
                FromUserName = dic["FromUserName"],
                CreateTime = long.Parse(dic["CreateTime"]),
                MsgId = long.Parse(dic["MsgId"]),
                Title = dic["Title"],
                Description = dic["Description"],
                Url = dic["Url"]
            };
        }
    }
}
