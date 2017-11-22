﻿using System;

/// <summary>
/// 微信异常
/// </summary>
namespace Weixin.Netcore.Core.Exceptions
{
    /// <summary>
    /// 微信接口调用异常
    /// </summary>
    public class WeixinInterfaceException : Exception
    {
        public WeixinInterfaceException(string message)
            : base(message)
        { }
    }
}
