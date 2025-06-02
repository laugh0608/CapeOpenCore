/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.02
 */

using System;
using System.Runtime.InteropServices;

// <summary>
// 这个命名空间提供了 CAPE-OPEN 接口的 .Net 翻译，并实现了 CAPE-OPEN 对象模型的核心组件。
// 有关 CAPE-OPEN 的更多信息，请访问 CO-Lan 网站：https:\\www.co-lan.org 。
// </summary>
namespace CapeOpenCore.Class;

/// <summary>为 CAPE-OPEN 对象的注册提供文本名称。</summary>
/// <remarks><para>该属性用于在 COM 注册表注册 CAPE-OPEN 对象时设置 CapeName[Name] 注册键的值。</para></remarks>
/// <code>
/// [Serializable, ComVisible(true)]
/// [Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeNameAttribute("MixerExample")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency. Cincinnati, Ohio")]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeNameAttribute : Attribute
{
    /// <summary>初始化 CapeDescriptionAttribute 类的新实例。</summary>
    /// <remarks>注册功能使用描述值设置 CapeDescription[Description] 注册密钥的值。</remarks>
    /// <param name="name">CAPE-OPEN 组件说明。</param>
    public CapeNameAttribute(string name)
    {
        Name = name;
    }

    /// <summary>获取名称信息。</summary>
    /// <remarks>名称的值。</remarks>
    /// <value>CAPE-OPEN 组件的名称。</value>
    public string Name { get; }
}

/// <summary>为 CAPE-OPEN 对象的注册提供文字说明。</summary>
/// <remarks>该属性用于在 COM 注册表注册 CAPE-OPEN 对象时设置 CapeDescription[Description] 注册键的值。</remarks>
/// <code>
/// [Serializable, ComVisible(true)]
/// [Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeDescriptionAttribute : Attribute
{
    /// <summary>初始化 CapeDescriptionAttribute 类的新实例。</summary>
    /// <remarks>注册功能使用描述值设置 CapeDescription[Description] 注册密钥的值。</remarks>
    /// <param name="description">CAPE-OPEN 组件的描述。</param>
    public CapeDescriptionAttribute(string description)
    {
        Description = description;
    }

    /// <summary>获取描述信息。</summary>
    /// <remarks>描述信息的值。</remarks>
    /// <value>CAPE-OPEN 组件的描述。</value>
    public string Description { get; }
}

/// <summary>为注册 CAPE-OPEN 对象提供 CAPE-OPEN 版本号。</summary>
/// <remarks>该属性用于在 COM 注册表注册 CAPE-OPEN 对象时设置 CapeDescription[CapeVersion] 注册键的值。</remarks>
/// <code>
/// [Serializable, ComVisible(true)]
/// [Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeVersionAttribute : Attribute
{
    /// <summary>初始化 CapeVersionAttribute 类的新实例。</summary>
    /// <remarks>注册函数使用描述值来设置 CapeDescription[CapeVersion] 注册密钥的值。</remarks>
    /// <param name="version">该对象支持的 CAPE-OPEN 接口的版本。</param>
    public CapeVersionAttribute(string version)
    {
        Version = version;
    }

    /// <summary>获取 CAPE-OPEN 接口的版本号。</summary>
    /// <remarks>CAPE-OPEN 接口版本号的值。</remarks>
    /// <value>CAPE-OPEN 组件的 CAPE-OPEN 接口版本号。</value>
    public string Version { get; }
}

/// <summary>为注册 CAPE-OPEN 对象提供供应商 URL。</summary>
/// <remarks>该属性用于在 CAPE-OPEN 对象与 COM 注册表注册 CAPE-OPEN 对象时使用，以设置 CapeDescription[VendorURL] 注册密钥的值。</remarks>
/// <code>
/// [Serializable, ComVisible(true)]
/// [Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeVendorURLAttribute : Attribute
{
    /// <summary>初始化 CapeVendorURLAttribute 类的新实例。</summary>
    /// <remarks>注册功能使用描述值来设置 CapeDescription[VendorURL] 注册密钥的值。</remarks>
    /// <param name="VendorURL">CAPE-OPEN 组件的供应商 URL。</param>
    public CapeVendorURLAttribute(string VendorURL)
    {
        this.VendorURL = VendorURL;
    }

    /// <summary>获取 VendorURL 信息。</summary>
    /// <remarks>VendorURL 的值</remarks>
    /// <value>CAPE-OPEN 组件的 VendorURL 值。</value>
    public string VendorURL { get; }
}

/// <summary>为注册 CAPE-OPEN 对象提供帮助 URL。</summary>
/// <remarks>该属性用于在 CAPE-OPEN 对象与 COM 注册表注册 CAPE-OPEN 对象时使用，以设置 CapeDescription[HelpURL] 注册键的值。</remarks>
/// <code>
/// [Serializable, ComVisible(true)]
/// [Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeHelpURLAttribute : Attribute
{
    /// <summary>初始化 CapeHelpURLAttribute 类的新实例。</summary>
    /// <remarks>注册函数使用帮助 URL 的值来设置 CapeDescription[HelpURL] 注册密钥的值。</remarks>
    /// <param name="HelpURL">CAPE-OPEN 组件的帮助 URL。</param>
    public CapeHelpURLAttribute(string HelpURL)
    {
        this.HelpURL = HelpURL;
    }

    /// <summary>获取 HelpURL 的相关信息。</summary>
    /// <remarks>HelpURL 的值。</remarks>
    /// <value>CAPE-OPEN 组件的 HelpURL 值。</value>
    public string HelpURL { get; }
}

/// <summary>提供有关 CAPE-OPEN 对象注册信息的文本。</summary>
/// <remarks>该属性用于在 COM 注册表注册 CAPE-OPEN 对象时设置 CapeDescription[About] 注册键的值。</remarks>
/// <code>
/// [Serializable, InteropServices.ComVisible(true)]
/// [Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeAboutAttribute : Attribute
{
    /// <summary>初始化 CapeAboutAttribute 类的新实例。</summary>
    /// <remarks>注册函数使用关于文本的值来设置 CapeDescription[About] 注册密钥的值。</remarks>
    /// <param name="About">CAPE-OPEN 组件的关于信息。</param>
    public CapeAboutAttribute(string About)
    {
        this.About = About;
    }

    /// <summary>获取 About 关于信息。</summary>
    /// <remarks>关于 About 信息的值。</remarks>
    /// <value>CAPE-OPEN 组件的 About 关于信息的值。</value>
    public string About { get; }
}

/// <summary>提供有关对象是否为 CAPE-OPEN 单元的信息。在注册 CAPE-OPEN 对象时使用。</summary>
/// <remarks>该属性用于在 COM 注册表注册 CAPE-OPEN 对象时，将 CapeUnitOperation_CATID 添加到对象的注册密钥中。</remarks>
/// <code>
/// [Serializable, ComVisible(true)]
/// [Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// [CapeOpen.CapeUnitOperation_CATID(true)]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeUnitOperationAttribute : Attribute
{
    /// <summary>初始化 CapeUnitOperationAttribute 类的新实例。</summary>
    /// <remarks>该属性用于指示对象是否为 CAPE-OPEN 单元操作。COM 注册功能也使用它在系统注册表中为该对象设置适当的 CATID 值。</remarks>
    /// <param name="isUnit">CAPE-OPEN 组件是一个 CAPE-OPEN 单元操作对象。</param>
    public CapeUnitOperationAttribute(bool isUnit)
    {
        IsUnit = isUnit;
    }

    /// <summary>获取 About 关于的描述值。</summary>
    /// <remarks>该属性表示对象是否使用 CAPE-OPEN 单元操作接口。</remarks>
    /// <value>布尔值，表示 CAPE-OPEN 组件是否为单元操作。</value>
    public bool IsUnit { get; }
}

/// <summary>提供在注册 CAPE-OPEN 对象时使用的有关对象是否支持流表监控的信息。</summary>
/// <remarks>这个属性在将 CAPE-OPEN 对象注册到 COM 注册表时使用，以将 CATID_MONITORING_OBJECT 添加到对象的注册键中。</remarks>
/// <code>
/// [Serializable, ComVisible(true)]
/// [Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// [CapeOpen.CapeFlowsheetMonitoringAttribute(true)]
/// public class WARAddIn : CapeObjectBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeFlowsheetMonitoringAttribute : Attribute
{
    private bool _mMonitors;
    /// <summary>初始化 CapeFlowsheetMonitoringAttribute 类的新实例。</summary>
    /// <remarks>此属性用于指示对象是否使用 PME 的流程图监控功能。COM 注册函数也使用此属性将适当的 CATID 值放入此对象的系统注册表中。</remarks>
    /// <param name="monitors">The CAPE-OPEN component is a flowsheet monitoring object.</param>
    public CapeFlowsheetMonitoringAttribute(bool monitors)
    {
        _mMonitors = monitors;
    }

    /// <summary>Gets the the about information.</summary>
    /// <remarks>This property indicates whether the object uses the flowsheet monitoring interfaces of the PME.</remarks>
    /// <value>A boolean value indicating whether the CAPE-OPEN component supports flowsheet monitoring.</value>
    public bool Monitors
    {
        get
        {
            return _mMonitors;
        }
    }
}

/// <summary>Provides information regarding whether the object consumes Thermodynamics 
/// during registration of a CAPE-OPEN object.</summary>
/// <remarks><para>
/// This attribute is used during the registration of a CAPE-OPEN object with the
/// COM registry to add the Consumes_Thermo_CATID to the object's registration key. </para>
/// </remarks>
/// <code>
/// [Serializable]
/// [System.Runtime.InteropServices.ComVisible(true)]
/// [System.Runtime.InteropServices.Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// [CapeOpen.CapeConsumesThermoAttribute(true)]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeConsumesThermoAttribute : Attribute
{
    private bool m_ConsumesThermo;

    /// <summary>Initializes a new instance of the CapeConsumesThermoAttribute class.</summary>
    /// <remarks>This attribute is used to indicate whether the object consumes thermodynamic
    /// models. It is also used by the COM registration function to place the appropriate CATID
    /// value in the system registry.</remarks>
    /// <param name="consumes">A boolean value indicating whether the CAPE-OPEN component consumes thermodynamics.</param>
    public CapeConsumesThermoAttribute(bool consumes)
    {
        m_ConsumesThermo = consumes;
    }

    /// <summary>Gets information about whether the object consumes Thermodynamics models.</summary>
    /// <remarks>This property indicates whether the object consumes thermodynamics.</remarks>
    /// <value>The CAPE-OPEN component consumes thermodynamics.</value>
    public bool ConsumesThermo
    {
        get
        {
            return m_ConsumesThermo;
        }
    }
}

/// <summary>Provides information regarding whether the object supports Thermodynamics 
/// version 1.0 during registration of a CAPE-OPEN object.</summary>
/// <remarks><para>
/// This attribute is used during the registration of a CAPE-OPEN object with the
/// COM registry to add the SupportsThermodynamics10_CATID to the object's registration key. </para>
/// </remarks>
/// <code>
/// [Serializable]
/// [System.Runtime.InteropServices.ComVisible(true)]
/// [System.Runtime.InteropServices.Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// [CapeOpen.CapeConsumesThermoAttribute(true)]
/// [CapeOpen.CapeSupportsThermodynamics10Attribute(true)]
/// public class CMixerExample : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeSupportsThermodynamics10Attribute : Attribute
{
    private bool m_Supported;

    /// <summary>Initializes a new instance of the CapeSupportsThermodynamics10Attribute class.</summary>
    /// <remarks>This attribute is used to indicate whether the object supports thermodynamic
    /// version 1.0. It is also used by the COM registration function to place the appropriate CATID
    /// value in the system registry.</remarks>
    /// <param name="supported">The CAPE-OPEN component consumes thermodynamics.</param>
    public CapeSupportsThermodynamics10Attribute(bool supported)
    {
        m_Supported = supported;
    }

    /// <summary>Gets the the about information.</summary>
    /// <remarks>This property indicates whether the object consumes thermodynamics.</remarks>
    /// <value>The CAPE-OPEN component supports thermodynamics version 1.0.</value>
    public bool Supported
    {
        get
        {
            return m_Supported;
        }
    }
}

/// <summary>Provides information regarding whether the object supports Thermodynamics 
/// version 1.0 during registration of a CAPE-OPEN object.</summary>
/// <remarks><para>
/// This attribute is used during the registration of a CAPE-OPEN object with the
/// COM registry to add the SupportsThermodynamics10_CATID to the object's registration key. </para>
/// </remarks>
/// <code>
/// [Serializable]
/// [System.Runtime.InteropServices.ComVisible(true)]
/// [System.Runtime.InteropServices.Guid("C79CAFD3-493B-46dc-8585-1118A0B5B4AB")]  //ICapeThermoMaterialObject_IID)
/// [Description("")]
/// [CapeDescriptionAttribute("An example mixer unit operation.")]
/// [CapeVersionAttribute("1.0")]
/// [CapeVendorURLAttribute("http:\\www.epa.gov")]
/// [CapeHelpURLAttribute("http:\\www.epa.gov")]
/// [CapeAboutAttribute("US Environmental Protection Agency Cincinnati, Ohio")]
/// [CapeOpen.CapeConsumesThermoAttribute(true)]
/// [CapeOpen.CapeSupportsThermodynamics11Attribute(true)]
/// public class CMixerExample110 : public CUnitBase
/// </code>
[ComVisible(false)]
[AttributeUsage(AttributeTargets.Class)]
public class CapeSupportsThermodynamics11Attribute : Attribute
{
    private bool m_Supported;
    /// <summary>Initializes a new instance of the CapeSupportsThermodynamics10Attribute class.</summary>
    /// <remarks>This attribute is used to indicate whether the object supports thermodynamic
    /// version 1.0. It is also used by the COM registration function to place the appropriate CATID
    /// value in the system registry.</remarks>
    /// <param name="supported">The CAPE-OPEN component consumes thermodynamics.</param>
    public CapeSupportsThermodynamics11Attribute(bool supported)
    {
        m_Supported = supported;
    }

    /// <summary>Gets the the about information.</summary>
    /// <remarks>This property indicates whether the object consumes thermodynamics.</remarks>
    /// <value>The CAPE-OPEN component supports thermodynamics version 1.0.</value>
    public bool Supported
    {
        get
        {
            return m_Supported;
        }
    }
}

/*	[
odl,
uuid(B777A1BD-0C88-11D3-822E-00C04F4F66C9),
version(20.0),
helpstring("IATCapeXRealParameterSpec Interface"),
dual,
oleautomation
]
interface IATCapeXRealParameterSpec : IDispatch {
[id(0x60040003), propget, helpstring(" Provide the Aspen Plus display units for for this parameter.")]
HRESULT DisplayUnits([out, retval] BSTR* bsUOM);
};

typedef [version(1.0)]
enum {
ErrorSeverityTerminal = 0,
ErrorSeveritySevere = 1,
ErrorSeverityError = 2,
ErrorSeverityWarning = 3
} __MIDL___MIDL_itf_AspenCapeX_0244_0001;

[
odl,
uuid(B777A1B9-0C88-11D3-822E-00C04F4F66C9),
version(1.0),
hidden,
dual,
nonextensible,
oleautomation
]
interface IATCapeXDiagnostic : IDispatch {
[id(0x60040000), helpstring("Print a message to the history device.")]
HRESULT SendMsgToHistory([in] BSTR message);
[id(0x60040001), helpstring("Print a message to the terminal device.")]
HRESULT SendMsgToTerminal([in] BSTR message);
[id(0x60040002), helpstring("Signal a simulation error.")]
HRESULT RaiseError(
[in] ErrorSeverity severity,
[in] BSTR context,
[in] BSTR message);
};
*/