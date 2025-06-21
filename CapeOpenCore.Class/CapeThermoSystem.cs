/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.07
 */

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

// 接口的类型定义，用于后续使用 typedef LPDISPATCH CapeThermoEquilibriumServerInterface;
/// <summary>一个实现 <see cref="ICapeThermoSystem"/> 接口的类，并提供对当前计算机上可用的基于 COM 和 .Net 的物性（属性翻译为物性，下同）包的访问。</summary>
/// <remarks>此类提供所有与 COM 注册的所有类物性包以及在全局程序集缓存中包含的所有基于 .NET 的物性包的列表。</remarks>
[ComVisible(true)]
[Guid("B5483FD2-E8AB-4ba4-9EA6-53BBDB77CE81")]
[Description("CapeThermoSystem Wrapper")]
[ClassInterface(ClassInterfaceType.None)]
public abstract class CapeThermoSystem : CapeIdentification, ICapeThermoSystem, ICapeThermoSystemCOM
{
    /// <summary>获取所有可用物性包。</summary>
    /// <remarks>返回一个包含热力学系统支持的物性包名称的字符串数组。</remarks>
    /// <returns>返回的受支持物性包集。包含从 COM 对象序列化的字符串数组的 System.Object 对象。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    object ICapeThermoSystemCOM.GetPropertyPackages()
    {
        return GetPropertyPackages();
    }

    /// <summary>解决特定的物性包。</summary>
    /// <remarks>将引用的物性包解析为物性包接口。</remarks>
    /// <returns>物性包接口。</returns>
    /// <param name="propertyPackage">待解决的物性包。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    object ICapeThermoSystemCOM.ResolvePropertyPackage(string propertyPackage)
    {
        return ResolvePropertyPackage(propertyPackage);
    }

    // Process Modeling Component 简称 PMC，为过程模拟组件，简单理解为单元模块，Process Modeling Environment 简称 PME，为过程模拟环境。
    /// <summary>创建一个名为 CapeThermoSystem 的类实例，并使用提供的名称和描述。</summary>
    /// <remarks>可以使用此构造函数指定热力学系统的特定名称和描述。</remarks>
    /// <param name="name">PMC 的名称。</param>
    /// <param name="description">PMC 的描述。</param>
    protected CapeThermoSystem(string name, string description) : base(name, description) { }

    /// <summary>获取可用物性包列表。</summary>
    /// <remarks>返回由热系统支持的物性包名称的字符串数组。</remarks>
    /// <returns>返回的受支持物性包集合。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    public abstract string[] GetPropertyPackages();

    /// <summary>解决特定的物性包。</summary>
    /// <remarks>将引用的物性包解析为物性包接口。</remarks>
    /// <returns>物性包接口。</returns>
    /// <param name="propertyPackage">待解决的物性包。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    public abstract ICapeThermoPropertyPackage ResolvePropertyPackage(string propertyPackage);
}