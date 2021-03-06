﻿using Weixin.Netcore.Core.Exceptions;
using Weixin.Netcore.Core.Message.Handler;
using Weixin.Netcore.Core.MessageRepet;
using Weixin.Netcore.Model.WeixinMessage;

namespace Weixin.Netcore.Core.Message.Processer
{
    /// <summary>
    /// 消息处理器（语音消息）
    /// </summary>
    public class VoiceMessageProcesser : IMessageProcesser
    {
        private readonly IMessageRepetHandler _messageRepetHandler;
        private readonly IMessageRepetValidUsage _messageRepetValidUsage;
        private readonly IVoiceMessageHandler _voiceMessageHandlder;

        public VoiceMessageProcesser(IMessageRepetHandler messageRepetHandler,
            IMessageRepetValidUsage messageRepetValidUsage, IVoiceMessageHandler voiceMessageHandler)
        {
            _messageRepetHandler = messageRepetHandler;
            _messageRepetValidUsage = messageRepetValidUsage;
            _voiceMessageHandlder = voiceMessageHandler;
        }

        public string ProcessMessage(IMessage message)
        {
            if (message is VoiceMessage)//语音消息
            {
                if (_messageRepetValidUsage.IsRepetValidUse && !_messageRepetHandler.MessageRepetValid((message as VoiceMessage).MsgId.ToString()))
                    return "success";
                return _voiceMessageHandlder.VoiceMessageHandler(message as VoiceMessage);
            }
            else
            {
                throw new MessageNotSupportException("不支持的消息类型");
            }
        }
    }
}
