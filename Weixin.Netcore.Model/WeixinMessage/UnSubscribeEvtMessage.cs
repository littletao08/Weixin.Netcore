﻿namespace Weixin.Netcore.Model.WeixinMessage
{
    /// <summary>
    /// 取消关注事件消息
    /// </summary>
    public class UnSubscribeEvtMessage : EventMessage
    {
        public UnSubscribeEvtMessage()
        {
            Event = "unsubscribe";
        }
    }
}
