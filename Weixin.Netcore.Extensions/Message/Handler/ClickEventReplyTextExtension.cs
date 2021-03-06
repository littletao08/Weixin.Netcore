﻿using Weixin.Netcore.Core.Message.Handler;
using Weixin.Netcore.Model.WeixinMessage;
using Weixin.Netcore.Model.WeixinMessage.Reply;
using Weixin.Netcore.Utility;

namespace Weixin.Netcore.Extensions.Message.Handler
{
    /// <summary>
    /// 点击回复文本消息扩展
    /// </summary>
    public class ClickEventReplyTextExtension : IClickEvtMessageHandler
    {
        private readonly IMessageReply<TextMessage> _messageReply;

        public ClickEventReplyTextExtension(IMessageReply<TextMessage> messageReply)
        {
            _messageReply = messageReply;
        }

        public string ClickEventHandler(ClickEvtMessage message)
        {
            var textMessage = new TextMessage()
            {
                ToUserName = message.FromUserName,
                FromUserName = message.ToUserName,
                CreateTime = UtilityHelper.GetTimeStamp(),
                Content = $"你点击了{message.EventKey}按钮"
            };

            return _messageReply.CreateXml(textMessage);
        }
    }
}
