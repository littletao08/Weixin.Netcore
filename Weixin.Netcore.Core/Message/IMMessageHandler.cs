﻿namespace Weixin.Netcore.Core.Message
{
    /// <summary>
    /// 消息处理
    /// </summary>
    public interface IMMessageHandler : ITextMessageHandler, IImageMessageHandler, 
        IVoiceMessageHandler, IVideoMessageHandler, IShortVideoMessageHandler, 
        ILocationMessageHandler, ILinkMessageHandler
    { }
}
