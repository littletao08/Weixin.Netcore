﻿using System.Collections.Generic;
using Weixin.Netcore.Utility;

namespace Weixin.Netcore.Model.WeixinMessage.Receive
{
    /// <summary>
    /// 关注事件消息接收
    /// </summary>
    public class SubscribeEvtMessageReceive : IMessageReceive<SubscribeEvtMessage>
    {
        public SubscribeEvtMessage GetEntity(string xml)
        {
            var dic = UtilityHelper.Xml2Dictionary(xml);
            var message = new SubscribeEvtMessage()
            {
                ToUserName = dic["ToUserName"],
                FromUserName = dic["FromUserName"],
                CreateTime = long.Parse(dic["CreateTime"])
            };

            return message;
        }

        public SubscribeEvtMessage GetEntity(Dictionary<string, string> dic)
        {
            var message = new SubscribeEvtMessage()
            {
                ToUserName = dic["ToUserName"],
                FromUserName = dic["FromUserName"],
                CreateTime = long.Parse(dic["CreateTime"])
            };

            return message;
        }
    }
}
