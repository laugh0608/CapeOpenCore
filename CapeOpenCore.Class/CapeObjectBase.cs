/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.01
 */

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CapeOpenCore.Class;

/// <summary>实现 ICapeIdentification 和 ICapeUtilities 的抽象基类。</summary>
/// <remarks><para>这个抽象类包含了 ICapeIdentification 和 ICapeUtilities 所需的所有功能。它可以被继承并使用，就像任何通用的PMC一样。</para>
/// <para>派生类将注册为 CAPE-OPEN 组件（类别 GUID 为 678c09a1-7d66-11d2-a67d-00105a42887f）和
/// 流程图监控对象（类别 GUID 为 7BA1AF89-B2E4-493d-BD80-2970BF4CBE99）。</para></remarks>
[Serializable]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public abstract class CapeObjectBase : CapeIdentification, ICapeUtilities, ICapeUtilitiesCOM, ECapeUser, ECapeRoot
{
    /// <summary>设备运行最后一次验证时返回的信息。</summary>
    protected string MValidationMessage;

    [NonSerialized]
    private Exception _pException;

    // 跟踪是否已调用 Dispose。
    private bool _disposed;
    /// <summary>PMC 可以使用的模拟上下文。</summary>
    /// <remarks>仿真上下文提供对 PME 的访问，使 PMC 能够访问 PME 的 COSE 接口 <see cref="ICapeDiagnostic"/>, 
    /// <see cref="ICapeMaterialTemplateSystem"/> 和 <see cref="ICapeCOSEUtilities"/>。</remarks>
    [NonSerialized]
    private ICapeSimulationContext _mSimulationContext;

    private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
    {
        return typeof(CapeObjectBase).Assembly;
    }

    /// <summary>显示 PMC 图形界面（如果有）。</summary>
    /// <remarks><para>默认情况下，此方法会抛出一个 <see cref="CapeNoImplException"/>，根据 CAPE-OPEN 规范，
    /// 该异常被过程建模环境解释为 PMC 没有编辑器 GUI，而 PME 必须执行编辑步骤。</para>
    /// <para>为了使 PMC 提供自己的编辑器，需要重写 Edit 方法以创建图形编辑器。当用户请求显示编辑器的流程图时，将调用此方法来编辑单元。
    /// 重写类不应返回失败（抛出和异常），因为这将被流程图工具解释为单元没有提供自己的编辑器。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    int ICapeUtilitiesCOM.Edit()
    {
        try
        {
            var result = Edit();
            return result == DialogResult.OK ? 0 : 1;
        }
        catch(Exception) // catch(Exception p_Ex)
        {
            throw new CapeNoImplException("No editor available");
        }
    }
        
    /// <summary>获取组件的参数集合。</summary>
    /// <value>返回类型为 System.Object，此方法仅用于基于经典的 COM 的 CAPE-OPEN 互操作。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [Browsable(false)]
    object ICapeUtilitiesCOM.parameters => Parameters;

    /// <summary>设置组件的模拟上下文。</summary>
    /// <remarks>此方法提供对 COSE 的接口的访问 <see cref="ICapeDiagnostic"/>, <see cref="ICapeMaterialTemplateSystem"/> 和 <see cref="ICapeCOSEUtilities"/>。</remarks>
    /// <value>流程环境指定的模拟上下文。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [Browsable(false)]
    object ICapeUtilitiesCOM.simulationContext
    {
        set
        {
            if (value is ICapeSimulationContext pContext)
                _mSimulationContext = pContext;
        }
    }

    /// <summary>可以在这里执行清理任务。</summary>
    /// <remarks><para>CAPE-OPEN 对象应该在这次调用期间释放所有分配的资源。这是由 PME 在对象析构函数之前调用的。Terminate() 可以检查数据是否已保存，如果未保存则返回错误。</para>
    /// <para>该方法没有输入或输出参数。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    void ICapeUtilitiesCOM.Terminate()
    {
        Terminate();
    }

    /// <summary>可在此处进行初始化。</summary>
    /// <remarks><para>CAPE-OPEN 对象可在此方法中分配资源。PME 会在对象构造函数之后调用该方法。</para>
    /// <para>该方法没有输入或输出参数。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    void ICapeUtilitiesCOM.Initialize()
    {
        Initialize();
    }

    /// <summary>单元操作的构造函数。</summary>
    /// <remarks>该方法正在为对象创建参数集合。因此参数可在派生对象的构造函数中或在调用 <c>Initialize()</c> 时添加。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    protected CapeObjectBase()
    {
        // _mParameters = new ParameterCollection();
        Parameters = [];
        _mSimulationContext = null;
        MValidationMessage = "This object has not been validated.";
        _disposed = false;
    }

    /// <summary><see cref="CapeObjectBase"/> 类的终结器。</summary>
    /// <remarks>这将最终确定类的当前实例。</remarks>
    ~CapeObjectBase()
    {
        Dispose();
    }

    /// <summary>单元操作的构造函数。</summary>
    /// <remarks>该方法正在为对象创建参数集合。因此，可以在派生对象的构造函数中或在调用 <c>Initialize()</c> 时添加参数。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <param name = "name">PMC 的名字。</param>
    protected CapeObjectBase(string name) : base(name)
    {
        // _mParameters = new ParameterCollection();
        Parameters = [];
        _mSimulationContext = null;
        MValidationMessage = "This object has not been validated.";
        _disposed = false;
    }

    /// <summary>单元操作的构造函数。</summary>
    /// <remarks>该方法正在为对象创建参数集合。因此，可以在派生对象的构造函数中或在调用 <c>Initialize()</c> 时添加参数。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <param name = "name">PMC 的名字。</param>
    /// <param name = "description">PMC 的描述。</param>
    protected CapeObjectBase(string name, string description) : base(name, description)
    {
        // _mParameters = new ParameterCollection();
        Parameters = [];
        _mSimulationContext = null;
        MValidationMessage = "This object has not been validated.";
        _disposed = false;
    }

    /// <summary>创建一个新对象，该对象是当前实例的副本。</summary>
    /// <remarks><para>克隆可以以深度复制或浅层复制的方式实现。在深度复制中，所有对象都被复制； 在浅层复制中，只有顶层对象被复制，低层对象包含引用。</para>
    /// <para>生成的克隆必须与原始实例的类型相同或兼容。</para>
    /// <para>有关克隆、深拷贝与浅拷贝以及示例的更多信息，请参见 <see cref="Object.MemberwiseClone"/>。</para></remarks>
    /// <param name = "objectToBeCopied">被复制的对象。</param>
    protected CapeObjectBase(CapeObjectBase objectToBeCopied) : base(objectToBeCopied)  // : base((CapeIdentification)objectToBeCopied)
    {
        _mSimulationContext = objectToBeCopied._mSimulationContext;
        Parameters.Clear();
        // 从 'ICapeParameter' 类型转换为 'CapeParameter' 时可能为 'System.InvalidCastException'
        foreach (CapeParameter parameter in objectToBeCopied.Parameters)
        {
            Parameters.Add((CapeParameter)parameter.Clone());
        }
        MValidationMessage = "This object has not been validated.";
        _disposed = false;
    }

    /// <summary>创建一个新对象，该对象是当前实例的副本。</summary>
    /// <remarks><para>克隆可以以深度复制或浅层复制的方式实现。在深度复制中，所有对象都被复制； 在浅层复制中，只有顶层对象被复制，低层对象包含引用。</para>
    /// <para>生成的克隆必须与原始实例的类型相同或兼容。</para>
    /// <para>有关克隆、深拷贝与浅拷贝以及示例的更多信息，请参见 <see cref="Object.MemberwiseClone"/>。</para></remarks>
    /// <returns>一个新对象，是该实例的副本。</returns>
    public override object Clone()
    {
        // 可能将 'null' 赋给不可以为 null 的实体
        var retVal = (CapeObjectBase)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(GetType().AssemblyQualifiedName, GetType().FullName);
        retVal.Parameters.Clear();
        // 从 'ICapeParameter' 类型转换为 'CapeParameter' 时可能为 'System.InvalidCastException'
        foreach (CapeParameter param in Parameters)
        {
            retVal.Parameters.Add((CapeParameter)param.Clone());
        }
        retVal.SimulationContext = null;
        if (retVal.GetType().IsAssignableFrom(typeof(ICapeSimulationContext)))
            retVal.SimulationContext = _mSimulationContext;
            // retVal.SimulationContext = (ICapeSimulationContext)_mSimulationContext;
        return retVal;
    }
                        
    // Dispose(bool disposing) 在两种不同的情况下执行。
    // 如果 disposing 等于 true，则该方法已被用户代码直接或间接调用。托管和非托管资源都可以被处置。
    // 如果 disposing 等于 false，则表示该方法已由 运行时调用，因此不应引用其他对象。只有非托管资源才能被处置。
    /// <summary>释放 CapeIdentification 对象使用的非托管资源，并可选择释放 托管资源。</summary>
    /// <remarks><para>此方法由公共的 Dispose 方法和 Finalize 方法调用。<c>Dispose()</c> 调用受保护的 <c>Dispose(Boolean)</c> 方法，
    /// 将 disposing 参数设置为 <c>true</c>。Finalize 调用 <c>Dispose</c>，将 disposing 参数设置为 false。</para>
    /// <para>当 disposing 参数为 <c>true</c> 时，该方法会释放由该组件引用的任何托管对象持有的所有资源。该方法会调用每个引用对象的 Dispose() 方法。</para>
    /// <para>继承该方法的开发人员须知：</para>
    /// <para><c>Dispose</c> 可以被其他对象多次调用。在重写 <c>Dispose(Boolean)</c> 时，要小心不要引用在之前调用 <c>Dispose</c> 时已经处理过的对象。
    /// 有关如何实现 <c>Dispose(Boolean)</c> 的详细信息，请参阅实现一个 Dispose 方法。</para>
    /// <para>有关 <c>Dispose</c> 和 <c>Finalize</c> 的更多信息，请参阅清理未托管的资源和重写 Finalize 方法。</para></remarks> 
    /// <param name = "disposing">true 释放托管和非托管资源；false 只释放非托管资源。</param>
    protected override void Dispose(bool disposing)
    {
        // 检查是否已调用 Dispose。
        if (_disposed) return;
        // 如果处置为 true，则处置所有托管 和非托管资源。
        if (disposing)
        {
            if (_mSimulationContext != null)
            {
                if (_mSimulationContext.GetType().IsCOMObject)
                    Marshal.FinalReleaseComObject(_mSimulationContext);
            }
            _mSimulationContext = null;
            Parameters.Clear();
            _disposed = true;
        }
        base.Dispose(disposing);
    }

    /// <summary>控制 COM 注册的函数。</summary>
    /// <remarks>该功能用于添加 CAPE-OPEN 方法和工具规范中指定的注册密钥。特别是，它表示该单元操作执行了 CAPE-OPEN 单元操作类别标识。
    ///	它还使用 <see cref="CapeNameAttribute"/>、<see cref="CapeDescriptionAttribute"/>、<see cref="CapeVersionAttribute"/>、
    /// <see cref="CapeVendorURLAttribute"/>、<see cref="CapeHelpURLAttribute"/>、<see ref="CapeAboutAttribute"/> 属性添加 CapeDescription 注册表键值。</remarks>
    /// <param name = "t">注册类的类型。</param> 
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    [ComRegisterFunction]
    public static void RegisterFunction(Type t)
    {
        RegistrationHelper(RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32), t);
        RegistrationHelper(RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64), t);
    }
    private static void RegistrationHelper(RegistryKey baseKey, Type t)
    {
        var assembly = t.Assembly;
        var versionNumber = (new AssemblyName(assembly.FullName)).Version.ToString();

        var keyName = string.Concat("CLSID\\{", t.GUID.ToString(), "}");
        var classKey = baseKey.CreateSubKey(keyName);
        // 可能的 'System.NullReferenceException'
        var catidKey = classKey.CreateSubKey("Implemented Categories");
        catidKey.CreateSubKey(COGuids.CapeOpenComponent_CATID);

        var attributes = t.GetCustomAttributes(false);
        var nameInfoString = t.FullName;
        var descriptionInfoString = "";
        var versionInfoString = "";
        var companyUrlInfoString = "";
        var helpUrlInfoString = "";
        var aboutInfoString = "";
        foreach (var pT in attributes)
        {
            switch (pT)
            {
                case CapeUnitOperationAttribute:
                    catidKey.CreateSubKey(COGuids.CapeUnitOperation_CATID);
                    break;
                case CapeFlowsheetMonitoringAttribute:
                    catidKey.CreateSubKey(COGuids.CATID_MONITORING_OBJECT);
                    break;
                case CapeConsumesThermoAttribute:
                    catidKey.CreateSubKey(COGuids.Consumes_Thermo_CATID);
                    break;
                case CapeSupportsThermodynamics10Attribute:
                    catidKey.CreateSubKey(COGuids.SupportsThermodynamics10_CATID);
                    break;
                case CapeSupportsThermodynamics11Attribute:
                    catidKey.CreateSubKey(COGuids.SupportsThermodynamics11_CATID);
                    break;
                case CapeNameAttribute nAttribute:
                    nameInfoString = nAttribute.Name;
                    break;
                case CapeDescriptionAttribute dAttribute:
                    descriptionInfoString = dAttribute.Description;
                    break;
                case CapeVersionAttribute vAttribute:
                    versionInfoString = vAttribute.Version;
                    break;
                case CapeVendorURLAttribute cAttribute:
                    companyUrlInfoString = cAttribute.VendorURL;
                    break;
                case CapeHelpURLAttribute hAttribute:
                    helpUrlInfoString = hAttribute.HelpURL;
                    break;
                case CapeAboutAttribute aAttribute:
                    aboutInfoString = aAttribute.About;
                    break;
            }
        }

        var descriptionKey = classKey.CreateSubKey("CapeDescription");
        descriptionKey.SetValue("Name", nameInfoString);
        descriptionKey.SetValue("Description", descriptionInfoString);
        descriptionKey.SetValue("CapeVersion", versionInfoString);
        descriptionKey.SetValue("ComponentVersion", versionNumber);
        descriptionKey.SetValue("VendorURL", companyUrlInfoString);
        descriptionKey.SetValue("HelpURL", helpUrlInfoString);
        descriptionKey.SetValue("About", aboutInfoString);
        catidKey.Close();
        descriptionKey.Close();
        classKey.Close();
    }
    
    /// <summary>该函数控制在卸载类时从 COM 注册表中删除该类。</summary>
    /// <remarks>该方法将删除所有添加到类注册中的子键，包括在 <see cref="RegisterFunction"/> 方法中添加的 CAPE-OPEN 特定键。</remarks>
    /// <param name = "t">未注册类的类型。</param> 
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    [ComUnregisterFunction]
    public static void UnregisterFunction(Type t)
    {
        UnregisterHelper(RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32), t);
        UnregisterHelper(RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64), t);
    }

    private static void UnregisterHelper(RegistryKey baseKey, Type t)
    {
        var keyName = string.Concat("CLSID\\{", t.GUID.ToString(), "}");
        baseKey.DeleteSubKeyTree(keyName, false);
    }

    /// <summary>可在此处进行初始化。</summary>
    /// <remarks><para>CAPE_OPEN 对象可在此方法中分配资源。PME 会在对象构造函数之后调用该方法。</para>
    /// <para>该方法没有输入或输出参数。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    public virtual void Initialize() { }

    /// <summary>可以在这里执行清理任务。</summary>
    /// <remarks><para>在调用过程中，CAPE-OPEN 对象应释放所有已分配的资源。此调用 在 PME 调用对象析构函数之前调用。终止时可能会检查数据是否已保存，如果未保存则返回错误信息。</para>
    /// <para>该方法没有输入或输出参数。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    public virtual void Terminate()
    {
        Dispose();
    }

    /// <summary>获取组件的参数集合。</summary>
    /// <remarks><para>返回公共参数集合（即 <see cref="ICapeCollection"/>）。</para>
    /// <para>这些作为一组暴露接口 <see cref="ICapeParameter"/> 的元素交付。从那里，客户端可以提取 <see cref="ICapeParameterSpec"/> 接口或任何类型化接口，
    /// 例如 <see cref="ICapeRealParameterSpec"/>，一旦客户端确定参数的类型为 double。</para></remarks>
    /// <value>单元操作的参数集。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    //[EditorAttribute(typeof(ParameterCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [Category("Parameter Collection")]
    [TypeConverter(typeof(ParameterCollectionTypeConverter))]
    public ParameterCollection Parameters { get; }

    /// <summary>验证 PMC。</summary>
    /// <remarks>验证参数集合。此方法的基类实现会遍历参数集合并调用每个成员参数的 <see cref="Validate"/> 方法。如果所有参数都是有效的，则 PMC 是有效的，
    /// 这由 Validate 方法返回 <c>true</c> 表示。</remarks>
    /// <returns><para>如果单位有效，则为 true。</para>
    /// <para>如果单位无效，则为 false。</para></returns>
    /// <param name = "message">指向字符串的引用，该字符串将包含与参数验证相关的信息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadCOParameter">ECapeBadCOParameter</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    public virtual bool Validate(ref string message)
    {
        message = "Object is valid.";
        MValidationMessage = message;
        foreach (var pT in Parameters)
        {
            var testString = string.Empty;
            if (pT.Validate(ref testString)) continue;
            message = testString;
            MValidationMessage = message;
            return false;
        }
        return true;
    }

    /// <summary>显示 PMC 图形界面（如果有）。</summary>
    /// <remarks><para>默认情况下，此方法会抛出一个 <see cref="CapeNoImplException"/>，根据 CAPE-OPEN 规范，
    /// 该异常被过程建模环境解释为 PMC 没有编辑器 GUI，而 PME 必须执行编辑步骤。</para>
    /// <para>为了使 PMC 提供自己的编辑器，需要重写 Edit 方法以创建图形编辑器。当用户请求显示编辑器的流程图时，将调用此方法来编辑单元。
    /// 重写类不应返回失败（抛出和异常），因为这将被流程图工具解释为单元没有提供自己的编辑器。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    public virtual DialogResult Edit()
    {
        throw new CapeNoImplException("No Object Editor");
    }

    /// <summary>获取和设置组件的模拟上下文。</summary>
    /// <remarks>此方法提供对 COSE 的接口的访问 <see cref="ICapeDiagnostic"/>, <see cref="ICapeMaterialTemplateSystem"/> 和 <see cref="ICapeCOSEUtilities"/>。</remarks>
    /// <value>流程模拟环境指定的模拟上下文。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [Browsable(false)]
    public ICapeSimulationContext SimulationContext
    {
        get => _mSimulationContext;
        set => _mSimulationContext = value;
    }
    
    /// <summary>获取组件的流程表监控对象。</summary>
    /// <remarks>此方法提供对 COSE 的接口的访问 <see cref="ICapeDiagnostic"/>, <see cref="ICapeMaterialTemplateSystem"/> 和 <see cref="ICapeCOSEUtilities"/>。</remarks>
    /// <value>流程模拟环境指定的模拟上下文。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [Browsable(false)]
    public ICapeFlowsheetMonitoring FlowsheetMonitoring
    {
        get
        {
            // 可疑类型检查: 解决方案中没有从 'CapeOpenCore.Class.ICapeSimulationContext' 和 'CapeOpenCore.Class.ICapeFlowsheetMonitoring' 继承的类型
            if (_mSimulationContext is ICapeFlowsheetMonitoring)
            {
                return (ICapeFlowsheetMonitoring)_mSimulationContext;
            }
            return null;
        }
    }

    /// <summary>抛出异常并公开异常对象。</summary>
    /// <remarks>这种方法允许派生类遵循 CAPE-OPEN 错误处理标准，同时仍然使用 .Net 异常处理。为了使用此类，创建一个从 <see cref="ECapeUser"/> 派生的异常对象。
    /// 使用异常对象作为此函数的参数。这样，异常中的信息将使用 CAPE-OPEN 异常处理进行暴露，并将抛给 .Net 客户端。</remarks>
    /// <param name="exception">将抛出的异常。</param>
    // 名称 'throwException' 与规则 'Methods' 不匹配。建议的名称为 'ThrowException'。
    public void throwException(Exception exception)
    {
        _pException = exception;
        throw _pException;
    }

    // ECapeRoot method 返回 System.ApplicationException 中的消息字符串。
    /// <summary>抛出的异常名称。</summary>
    /// <remarks>抛出的异常名称。</remarks>
    /// <value>抛出的异常名称。</value>
    [Browsable(false)]
    string ECapeRoot.Name => _pException is ECapeRoot pRoot ? pRoot.Name : "";

    /// <summary>指定错误子类别的代码。</summary>
    /// <remarks>值的分配留给每个实现。因此，这是 CO 组件提供程序特有的专有代码。默认情况下，设置为 CAPE-OPEN 错误 HRESULT <see cref="CapeErrorInterfaceHR"/>。</remarks>
    /// <value>异常的 HRESULT 值。</value>
    [Browsable(false)]
    int ECapeUser.code => ((ECapeUser)_pException).code;

    /// <summary>错误的描述。</summary>
    /// <remarks>错误描述可以包括对导致错误的条件的详细描述。</remarks>
    /// <value>对异常的字符串描述。</value>
    [Browsable(false)]
    string ECapeUser.description => ((ECapeUser)_pException).description;

    /// <summary>错误的范围。</summary>
    /// <remarks>此属性提供包含错误发生位置的包列表。例如 <see cref="ICapeIdentification"/>。</remarks>
    /// <value>错误的范围。</value>
    [Browsable(false)]
    string ECapeUser.scope => ((ECapeUser)_pException).scope;

    /// <summary>发生错误的接口名称。这是一个必填字段。</summary>
    /// <remarks>发生错误的接口。</remarks>
    /// <value>接口的名称。</value>
    [Browsable(false)]
    string ECapeUser.interfaceName => ((ECapeUser)_pException).interfaceName;

    /// <summary>发生错误的操作名称。这是必填字段。</summary>
    /// <remarks>该字段提供了异常发生时正在执行的操作的名称。</remarks>
    /// <value>操作的名称。</value>
    [Browsable(false)]
    string ECapeUser.operation => ((ECapeUser)_pException).operation;

    /// <summary>指向页面、文档或网站的 URL，可在其中找到有关该错误的更多信息。这些信息的内容显然取决于实施情况。</summary>
    /// <remarks>该字段提供了一个互联网 URL，可在其中找到有关该错误的更多信息。</remarks>
    /// <value>链接 URL。</value>
    [Browsable(false)]
    string ECapeUser.moreInfo => ((ECapeUser)_pException).moreInfo;

    /// <summary>向终端写入信息。</summary>
    /// <remarks><para>向终端写入字符串。</para>
    /// <para>当需要将消息提请用户注意时，会调用此方法。实现应确保将字符串写入对话框或消息列表，以便用户能够轻松查看。</para>
    /// <para>该信息必须尽快显示给用户。</para></remarks>
    /// <param name = "message">要显示的文本。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    public void PopUpMessage(string message)
    {
        switch (_mSimulationContext)
        {
            case null:
                return;
            // 可疑类型检查: 解决方案中没有从 'CapeOpenCore.Class.ICapeSimulationContext' 和 'CapeOpenCore.Class.ICapeDiagnostic' 继承的类型
            case ICapeDiagnostic pDiagnostic:
                pDiagnostic.PopUpMessage(message);
                break;
        }
    }
    
    /// <summary>将字符串写入 PME 的日志文件。</summary>
    /// <remarks><para>将字符串写入日志。</para>
    /// <para>当需要记录消息进行日志记录时，会调用此方法。预计实现会将字符串写入日志文件或其他日志设备。</para></remarks>
    /// <param name = "message">要记录的文本。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    public void LogMessage(string message)
    {
        switch (_mSimulationContext)
        {
            case null:
                return;
            // 可疑类型检查: 解决方案中没有从 'CapeOpenCore.Class.ICapeSimulationContext' 和 'CapeOpenCore.Class.ICapeDiagnostic' 继承的类型
            case ICapeDiagnostic pDiagnostic:
                pDiagnostic.LogMessage(message);
                break;
        }
    }
}