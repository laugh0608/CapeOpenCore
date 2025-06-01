/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.01
 */

using System;
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
    private ParameterCollection _mParameters;
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
    [System.ComponentModel.BrowsableAttribute(false)]
    object ICapeUtilitiesCOM.parameters => _mParameters;

    /// <summary>设置组件的模拟上下文。</summary>
    /// <remarks>此方法提供对 COSE 的接口的访问 <see cref="ICapeDiagnostic"/>, <see cref="ICapeMaterialTemplateSystem"/> 和 <see cref="ICapeCOSEUtilities"/>。</remarks>
    /// <value>流程环境指定的模拟上下文。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [System.ComponentModel.BrowsableAttribute(false)]
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
        _mParameters = [];
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
        _mParameters = [];
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
        _mParameters = [];
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
        _mParameters.Clear();
        // 从 'ICapeParameter' 类型转换为 'CapeParameter' 时可能为 'System.InvalidCastException'
        foreach (CapeParameter parameter in objectToBeCopied.Parameters)
        {
            _mParameters.Add((CapeParameter)parameter.Clone());
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
            _mParameters.Clear();
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

    /// <summary>
    ///	Clean-up tasks can be performed here. 
    /// </summary>
    /// <remarks>
    /// <para>The CAPE-OPEN object should releases all of its allocated resources during this call. This is 
    /// called before the object destructor by the PME. Terminate may check if the data has been 
    /// saved and return an error if not.</para>
    /// <para>该方法没有输入或输出参数。</para>
    /// </remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    virtual public void Terminate()
    {
        Dispose();
    }

    /// <summary>
    ///	Gets the component's collection of parameters. 
    /// </summary>
    /// <remarks>
    /// <para>Return the collection of Public Parameters (i.e. 
    /// <see cref="ICapeCollection"/>.</para>
    /// <para>These are delivered as a collection of elements exposing the interface 
    /// <see cref="ICapeParameter"/>. From there, the client could extract the 
    /// <see cref="ICapeParameterSpec"/> interface or any of the typed
    /// interfaces such as <see cref="ICapeRealParameterSpec"/>, once the client 
    /// establishes that the Parameter is of type double.</para>
    /// </remarks>
    /// <value>The parameter collection of the unit operation.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    //[System.ComponentModel.EditorAttribute(typeof(ParameterCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [System.ComponentModel.CategoryAttribute("Parameter Collection")]
    [System.ComponentModel.TypeConverter(typeof(ParameterCollectionTypeConverter))]
    public ParameterCollection Parameters
    {
        get
        {
            return _mParameters;
        }
    }

    /// <summary>
    /// Validates the PMC. 
    /// </summary>
    /// <remarks>
    /// <para>Validates the parameter collection. This base-class implementation of this method 
    /// traverses the parameter collections and calls the  <see cref="Validate"/> method of each 
    /// member parameter. The PMC is valid if all parameters are valid, which is 
    /// signified by the Validate method returning <c>true</c>.</para>
    /// </remarks>
    /// <returns>
    /// <para>true, if the unit is valid.</para>
    /// <para>false, if the unit is not valid.</para>
    /// </returns>
    /// <param name = "message">Reference to a string that will conain a message regarding the validation of the parameter.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadCOParameter">ECapeBadCOParameter</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    public virtual bool Validate(ref string message)
    {
        message = "Object is valid.";
        MValidationMessage = message;
        for (int i = 0; i < Parameters.Count; i++)
        {
            string testString = string.Empty;
            if (!_mParameters[i].Validate(ref testString))
            {
                message = testString;
                MValidationMessage = message;
                return false;
            }
        }
        return true;
    }

    /// <summary>
    ///	Displays the PMC graphic interface, if available.
    /// </summary>
    /// <remarks>
    /// <para>By default, this method throws a <see cref="CapeNoImplException">CapeNoImplException</see>
    /// that according to the CAPE-OPEN specification, is interpreted by the process
    /// modeling environment as indicating that the PMC does not have a editor 
    /// GUI, and the PME must perform editing steps.</para>
    /// <para>In order for a PMC to provide its own editor, the Edit method will
    /// need to be overridden to create a graphical editor. When the user requests the flowheet
    /// to show the editor, this method will be called to edit the unit. Overriden classes should
    /// not return a failure (throw and exception) as this will be interpreted by the flowsheeting 
    /// tool as the unit not providing its own editor.</para>
    /// </remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    public virtual DialogResult Edit()
    {
        throw new CapeNoImplException("No Object Editor");
    }


    /// <summary>
    ///	Gets and sets the component's simulation context.
    /// </summary>
    /// <remarks>
    /// This method provides access to the COSE's interfaces <see cref="ICapeDiagnostic"/>, 
    /// <see cref="ICapeMaterialTemplateSystem"/> and <see cref="ICapeCOSEUtilities"/>.
    /// </remarks>
    /// <value>The simulation context assigned by the Flowsheeting Environment.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [System.ComponentModel.BrowsableAttribute(false)]
    public ICapeSimulationContext SimulationContext
    {
        get
        {
            return _mSimulationContext;
        }
        set
        {
            _mSimulationContext = value;
        }
    }
    /// <summary>
    ///	Gets the component's flowsheet monitoring object.
    /// </summary>
    /// <remarks>
    /// This method provides access to the COSE's interfaces <see cref="ICapeDiagnostic"/>, 
    /// <see cref="ICapeMaterialTemplateSystem"/> and <see cref="ICapeCOSEUtilities"/>.
    /// </remarks>
    /// <value>The simulation context assigned by the Flowsheeting Environment.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [System.ComponentModel.BrowsableAttribute(false)]
    public ICapeFlowsheetMonitoring FlowsheetMonitoring
    {
        get
        {
            if (_mSimulationContext is ICapeFlowsheetMonitoring)
            {
                return (ICapeFlowsheetMonitoring)_mSimulationContext;
            }
            return null;
        }
    }

    /// <summary>
    /// Throws an exception and exposes the exception object.
    /// </summary>
    /// <remarks>
    /// This method allows the derived class to conform to the CAPE-OPEN error handling standards and still use .Net 
    /// exception handling. In order to use this class, create an exception object that derives from <see cref="ECapeUser"/>.
    /// Use the exception object as the argument to this function. As a result, the information in the expcetion will be exposed using the CAPE-OPEN 
    /// exception handing and will be thrown to .Net clients.
    /// </remarks>
    /// <param name="exception">The exception that will the throw.</param>
    public void throwException(Exception exception)
    {
        _pException = exception;
        throw _pException;
    }

    // ECapeRoot method
    // returns the message string in the System.ApplicationException.
    /// <summary>
    /// The name of the exception being thrown.
    /// </summary>
    /// <remarks>
    /// The name of the exception being thrown.
    /// </remarks>
    /// <value>
    /// The name of the exception being thrown.
    /// </value>
    [System.ComponentModel.BrowsableAttribute(false)]
    string ECapeRoot.Name
    {
        get
        {
            if (_pException is ECapeRoot) return ((ECapeRoot)_pException).Name;
            return "";
        }
    }

    /// <summary>
    /// Code to designate the subcategory of the error. 
    /// </summary>
    /// <remarks>
    /// The assignment of values is left to each implementation. So that is a 
    /// proprietary code specific to the CO component provider. By default, set to 
    /// the CAPE-OPEN error HRESULT <see cref="CapeErrorInterfaceHR"/>.
    /// </remarks>
    /// <value>
    /// The HRESULT value for the exception.
    /// </value>
    [System.ComponentModel.BrowsableAttribute(false)]
    int ECapeUser.code
    {
        get
        {
            return ((ECapeUser)_pException).code;
        }
    }

    /// <summary>
    /// The description of the error.
    /// </summary>
    /// <remarks>
    /// The error description can include a more verbose description of the condition that
    /// caused the error.
    /// </remarks>
    /// <value>
    /// A string description of the exception.
    /// </value>
    [System.ComponentModel.BrowsableAttribute(false)]
    string ECapeUser.description
    {
        get
        {
            return ((ECapeUser)_pException).description;
        }
    }

    /// <summary>
    /// The scope of the error.
    /// </summary>
    /// <remarks>
    /// This property provides a list of packages where the error occurred. 
    /// For example <see cref="ICapeIdentification"/>.
    /// </remarks>
    /// <value>The source of the error.</value>
    [System.ComponentModel.BrowsableAttribute(false)]
    string ECapeUser.scope
    {
        get
        {
            return ((ECapeUser)_pException).scope;
        }
    }

    /// <summary>
    /// The name of the interface where the error is thrown. This is a mandatory field."
    /// </summary>
    /// <remarks>
    /// The interface that the error was thrown.
    /// </remarks>
    /// <value>The name of the interface.</value>
    [System.ComponentModel.BrowsableAttribute(false)]
    string ECapeUser.interfaceName
    {
        get
        {
            return ((ECapeUser)_pException).interfaceName;
        }
    }

    /// <summary>
    /// The name of the operation where the error is thrown. This is a mandatory field.
    /// </summary>
    /// <remarks>
    /// This field provides the name of the operation being perfomed when the exception was raised.
    /// </remarks>
    /// <value>The operation name.</value>
    [System.ComponentModel.BrowsableAttribute(false)]
    string ECapeUser.operation
    {
        get
        {
            return ((ECapeUser)_pException).operation;
        }
    }

    /// <summary>
    /// An URL to a page, document, web site,  where more information on the error can be found. The content of this information is obviously implementation dependent.
    /// </summary>
    /// <remarks>
    /// This field provides an internet URL where more information about the error can be found.
    /// </remarks>
    /// <value>The URL.</value>
    [System.ComponentModel.BrowsableAttribute(false)]
    string ECapeUser.moreInfo
    {
        get
        {
            return ((ECapeUser)_pException).moreInfo;
        }
    }

    /// <summary>
    /// Writes a message to the terminal.
    /// </summary>
    /// <remarks>
    /// <para>Write a string to the terminal.</para>
    /// <para>This method is called when a message needs to be brought to the user’s attention.
    /// The implementation should ensure that the string is written out to a dialogue box or 
    /// to a message list that the user can easily see.</para>
    /// <para>A priori this message has to be displayed as soon as possible to the user.</para>
    /// </remarks>
    /// <param name = "message">The text to be displayed.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    public void PopUpMessage(string message)
    {
        if (_mSimulationContext != null)
        {
            if (_mSimulationContext is ICapeDiagnostic)
            {
                ((ICapeDiagnostic)_mSimulationContext).PopUpMessage(message);
            }
        }
    }
    /// <summary>
    /// Writes a string to the PME's log file.
    /// </summary>
    /// <remarks>
    /// <para>Write a string to a log.</para>
    /// <para>This method is called when a message needs to be recorded for logging purposes. 
    /// The implementation is expected to write the string to a log file or other journaling 
    /// device.</para>
    /// </remarks>
    /// <param name = "message">The text to be logged.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    public void LogMessage(string message)
    {
        if (_mSimulationContext != null)
        {
            if (_mSimulationContext is ICapeDiagnostic)
            {
                ((ICapeDiagnostic)_mSimulationContext).LogMessage(message);
            }
        }
    }
};