/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.08
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>用于将基于 .NET 的对象暴露给基于 COM 的程序管理器（PMC）的包装类。</summary>
/// <remarks><para>这个类允许基于 COM 的遗留 PMC 访问基于 .NET 的物流对象。
/// 这个包装器将使用基于 .NET 的热力学接口的材料对象暴露给基于 COM 的 PMC。基于 COM 的 PMC 可以通过 CAPE-OPEN 接口的 COM 版本调用物流对象。</para>
/// <para>这个类由 <see cref="UnitPortWrapper"/> 类用于将端口连接的流股暴露给由 <see cref="UnitOperationWrapper"/> 类包装的基于 COM 的遗留单元操作。
/// 这个包装器处理在热力学接口的 .NET 版本中使用的强类型数组和 COM 版本中使用的 VARIANT 数据类型之间的转换。</para></remarks>
partial class COMMaterialObjectWrapper
{
    
}