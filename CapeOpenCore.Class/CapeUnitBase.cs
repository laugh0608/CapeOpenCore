/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.07
 */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CapeOpenCore.Class.CapeOpenUI;

namespace CapeOpenCore.Class;

internal class SelectedReportConverter : StringConverter
{
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        var unit = (CapeUnitBase)context.Instance;
        return new StandardValuesCollection(unit.Reports);
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
        return true;
    }
}

/// <summary>用于开发单元操作模型的抽象基类。</summary>
/// <remarks>这个抽象类包含了一个单元操作 PMC 所需的所有功能，除了 <c>Calculate()</c> 方法，这是一个必须被重写的纯虚函数。
/// 要使用它，请在适当的集合中添加参数和端口，并实现 <c>Calculate()</c> 方法。</remarks>
[Serializable, ComVisible(true)]
[ComSourceInterfaces(typeof(IUnitOperationValidatedEventArgs))]
[ClassInterface(ClassInterfaceType.None)]
public abstract class CapeUnitBase : CapeObjectBase, ICapeUnit, ICapeUnitCOM, ICapeUnitReport, ICapeUnitReportCOM
// IPersist, IPersistStream, IPersistStreamInit
{
    private CapeValidationStatus _mValStatus;
    // private bool m_dirty;
    private string _mSelecetdReport;
    private System.Collections.Generic.List<string> _mReports;
    // 跟踪是否已调用 Dispose 方法。
    private bool _disposed;
        
    /// <summary>获取单元操作端口集合。</summary>
    /// <remarks>返回类型为 System.Object，此方法仅用于基于经典的 COM 的 CAPE-OPEN 互操作。</remarks>
    /// <value>该单元操作的端口集合。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [Browsable(false)]
    object ICapeUnitCOM.ports => Ports;

    /// <summary>获取单元操作的可能报告列表。</summary>
    /// <value>返回类型为 System.Object，此方法仅用于基于经典的 COM 的 CAPE-OPEN 互操作。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [Browsable(false)]
    object ICapeUnitReportCOM.reports => _mReports.ToArray();
    
    /// <summary>生成该单元操作的活动报告。</summary>
    /// <remarks>生成指定的报告。如果未设置值，则生成默认报告。</remarks>
    /// <param name="message">包含当前选定报告文本的字符串。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    void ICapeUnitReportCOM.ProduceReport(ref string message)
    {
        message = ProduceReport();
    }
    
    /// <summary>单元操作的构造函数。</summary>
    /// <remarks>这种方法为单元操作创建了端口和参数集合。因此，可以在派生单元的构造函数中或在 <c>Initialize()</c> 调用期间添加端口和参数。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    protected CapeUnitBase()
    {
        // _mPorts = new PortCollection();
        Ports = [];
        // _mPorts.AddingNew += new AddingNewEventHandler(m_Ports_AddingNew);
        Ports.AddingNew += m_Ports_AddingNew;
        // _mPorts.ListChanged += new ListChangedEventHandler(m_Ports_ListChanged);
        Ports.ListChanged += m_Ports_ListChanged;
        _mValStatus = CapeValidationStatus.CAPE_NOT_VALIDATED;
        // _mReports = new System.Collections.Generic.List<string>();
        // _mReports.Add("Default Report");
        _mReports =
        [
            "Default Report"
        ];
        _mSelecetdReport = "Default Report";
    }

    /// <summary><see cref="CapeUnitBase"/> 类的终结器。</summary>
    /// <remarks>这将最终确定当前类的实例。</remarks>
    ~CapeUnitBase()
    {
        Dispose(true);
    }
         
    /// <summary>创建一个与当前实例相同的副本对象。</summary>
    /// <remarks><para>克隆可以以深度复制或浅度复制的方式实现。在深度复制中，所有对象都被复制；在浅度复制中，只有顶层对象被复制，较低层包含引用。</para>
    /// <para>生成的克隆必须与原始实例属于同一类型或与之兼容。</para>
    /// <para>请参阅 <see cref="Object.MemberwiseClone"/> 以获取有关克隆、深度复制与浅层复制以及示例的更多信息。</para></remarks>
    /// <returns>一个与该实例相同的副本对象。</returns>
    /// <param name="objectToBeCopied">正在克隆的单元操作。</param>
    protected CapeUnitBase(CapeUnitBase objectToBeCopied) : base(objectToBeCopied)
    {
        Ports = (PortCollection)objectToBeCopied.Ports.Clone();
        _mReports.AddRange(objectToBeCopied.Reports);
        _mValStatus = CapeValidationStatus.CAPE_NOT_VALIDATED;
        _mSelecetdReport = objectToBeCopied.selectedReport;
    }

    // Dispose(bool disposing) 在两种不同的情况下执行。如果 disposing 等于 true，则该方法已由用户的代码直接或间接调用。可以处理托管和非托管资源。
    // 如果 disposing 等于 false，则该方法已由运行时从 finalizer 内部调用，您不应引用其他对象。只有非托管资源可以处理。
    /// <summary>释放 CapeIdentification 对象使用的非托管资源，并可选地释放托管资源。</summary>
    /// <remarks><para>此方法由公共的 Dispose 方法以及 Finalize 方法调用。Dispose() 方法会调用受保护的 Dispose(Boolean) 方法，
    /// 并将 disposing 参数设置为 true。Finalize 方法会调用 Dispose，并将 disposing 参数设置为 false。</para>
    /// <para>当 disposing 参数为 true 时，此方法会释放此组件引用的任何托管对象持有的所有资源。此方法会调用每个引用对象的 Dispose() 方法。</para>
    /// <para>留给继承者的注释：Dispose 可以被其他对象多次调用。在重写 Dispose(Boolean) 方法时，要小心不要引用在之前调用 Dispose 时已经处理过的对象。
    /// 有关如何实现 Dispose(Boolean) 的更多信息，请参阅实现一个 Dispose 方法。</para>
    /// <para>有关 Dispose 和 Finalize 的更多信息，请参阅清理未托管资源和重写 Finalize 方法。</para></remarks> 
    /// <param name="disposing">true 表示释放受管和不受管资源；false 表示仅释放不受管资源。</param>
    protected override void Dispose(bool disposing)
    {
        // 检查是否已经调用了Dispose方法。
        if (_disposed) return;
        // 如果处置为真，则释放所有受管和非受管资源。
        if (disposing)
        {
            foreach (UnitPort port in Ports)
                port.Disconnect();
            Ports.Clear();
            _disposed = true;
        }
        base.Dispose(disposing);
    }

    /// <summary>创建一个与当前实例相同的副本对象。</summary>
    /// <remarks><para>克隆可以以深度复制或浅度复制的方式实现。在深度复制中，所有对象都被复制；在浅度复制中，只有顶层对象被复制，较低层包含引用。</para>
    /// <para>生成的克隆必须与原始实例属于同一类型或与之兼容。</para>
    /// <para>请参阅 <see cref="Object.MemberwiseClone"/> 以获取有关克隆、深度复制与浅层复制以及示例的更多信息。</para></remarks>
    /// <returns>一个与该实例相同的副本对象。</returns>
    public override object Clone()
    {
        var unitType = GetType();
        var retVal = (CapeUnitBase)Activator.CreateInstance(unitType);
        retVal.Parameters.Clear();
        foreach (CapeParameter param in Parameters)
        {
            retVal.Parameters.Add((CapeParameter)param.Clone());
        }
        retVal.Ports.Clear();
        foreach (UnitPort port in Ports)
        {
            retVal.Ports.Add((UnitPort)port.Clone());
        }
        retVal.Reports.Clear();
        retVal.Reports.AddRange(_mReports);
        retVal.selectedReport = _mSelecetdReport;
        retVal.SimulationContext = SimulationContext;
        return retVal;
    }

    /// <summary>表示用于处理组件名称更改的方法。</summary>
    /// <remarks>当您创建一个 ComponentNameChangedHandler 委托时，您确定将处理该事件的方法。要将事件与事件处理程序关联，
    /// 请向事件添加委托的实例。每当事件发生时，都会调用事件处理程序，除非您删除委托。有关委托的更多信息，请参阅“事件和委托”。</remarks>
    /// <param name="sender">PMC 作为数据源。</param>
    /// <param name="args">提供有关名称更改信息的 <see cref="System.ComponentModel.ListChangedEventArgs"/>。</param>
    private void m_Ports_ListChanged(object sender, ListChangedEventArgs args)
    {
        OnPortCollectionListChanged(args);
    }

    /// <summary>表示用于处理组件名称更改的方法。</summary>
    /// <remarks>当您创建一个 ComponentNameChangedHandler 委托时，您确定将处理该事件的方法。要将事件与事件处理程序关联，
    /// 请向事件添加委托的实例。每当事件发生时，都会调用事件处理程序，除非您删除委托。有关委托的更多信息，请参阅“事件和委托”。</remarks>
    /// <param name="sender">PMC 作为数据源。</param>
    /// <param name="args">提供有关名称更改信息的 <see cref="System.ComponentModel.AddingNewEventArgs"/>。</param>
    private void m_Ports_AddingNew(object sender, AddingNewEventArgs args)
    {
        OnPortCollectionAddingNew(args);
    }

    /// <summary>当列表或列表中的某项发生变化时触发。</summary>
    /// <remarks>只有当列表项类型实现 INotifyPropertyChanged 接口时，才会发出项目值更改的 ListChanged 通知。</remarks>
    public event ListChangedEventHandler PortCollectionListChanged;

    /// <summary>当列表或列表中的某项发生变化时触发。</summary>
    /// <remarks>只有当列表项类型实现 INotifyPropertyChanged 接口时，才会发出项目值更改的 ListChanged 通知。</remarks>
    /// <param name="args">提供有关名称更改信息的 <see cref="System.ComponentModel.ListChangedEventArgs"/>。</param>
    protected void OnPortCollectionListChanged(ListChangedEventArgs args)
    {
        // if (PortCollectionListChanged != null)
        // {
        //     PortCollectionListChanged(this, args);
        // }
        PortCollectionListChanged?.Invoke(this, args);
    }

    /// <summary>当用户向端口集合添加新元素时触发。</summary>
    /// <remarks>当 PMC 名称发生变更时触发的事件。</remarks> 
    public event AddingNewEventHandler PortCollectionAddingNew;

    /// <summary>当列表或列表中的某项发生变化时触发。</summary>
    /// <remarks>AddingNew 事件在将新对象添加到由 Items 属性表示的集合之前发生。该事件是在调用 AddNew 方法之后，但在创建新项并将其添加到内部列表之前触发的。
    /// 通过处理此事件，程序员可以提供自定义项创建和插入行为，而无需从 BindingList&lt;T&gt; 类派生。</remarks>
    /// <param name="args">包含有关事件信息的 <see cref="System.ComponentModel.AddingNewEventArgs"/>。</param>
    protected void OnPortCollectionAddingNew(AddingNewEventArgs args)
    {
        // if (PortCollectionAddingNew != null)
        // {
        //     PortCollectionAddingNew(this, args);
        // }
        PortCollectionAddingNew?.Invoke(this, args);
    }

    /// <summary>当用户验证单元操作时发生。</summary>
    public event UnitOperationValidatedHandler UnitOperationValidated;
    
    /// <summary>当单元操作被验证时发生。</summary>
    /// <remarks><para>通过委托调用事件处理程序时，会引发事件。<c>OnUnitOperationValidated</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>留给继承者的注释：当在派生类中重写<c>OnUnitOperationValidated</c>时，请务必调用基类的<c>OnUnitOperationValidate</c>方法，以便注册的委托接收事件。</para></remarks>
    /// <param name="args">包含有关此事件的信息的 <see cref="UnitOperationValidatedEventArgs"/>。</param>
    protected void OnUnitOperationValidated(UnitOperationValidatedEventArgs args)
    {
        // if (UnitOperationValidated != null)
        // {
        //     UnitOperationValidated(this, args);
        // }
        UnitOperationValidated?.Invoke(this, args);
    }

    /// <summary>当用户开始计算单元操作时发生。</summary>
    public event UnitOperationBeginCalculationHandler UnitOperationBeginCalculation;
    
    /// <summary>发生在单元操作计算过程的开始阶段。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnUnitOperationBeginCalculation</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>留给继承者的注释：当在派生类中重写 <c>OnUnitOperationBeginCalculation</c> 时，
    /// 请务必调用基类的 <c>OnUnitOperationBeginCalculation</c> 方法，以便注册的委托接收事件。</para></remarks>
    /// <param name="message">包含有关计算信息的字符串。</param>
    protected void OnUnitOperationBeginCalculation(string message)
    {
        var args = new UnitOperationBeginCalculationEventArgs(ComponentName, message);
        // if (UnitOperationBeginCalculation != null)
        // {
        //     UnitOperationBeginCalculation(this, args);
        // }
        UnitOperationBeginCalculation?.Invoke(this, args);
    }

    /// <summary> 在完成单一操作的计算后发生。</summary>
    public event UnitOperationEndCalculationHandler UnitOperationEndCalculation;
    
    /// <summary>在单元操作计算过程完成时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnUnitOperationEndCalculation</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>留给继承者的注释：当在派生类中重写 <c>OnUnitOperationEndCalculation</c> 时，
    /// 请务必调用基类的 <c>OnUnitOperationBeginCalculation</c> 方法，以便注册过的委托能够接收到事件。</para></remarks>
    /// <param name="message">包含计算信息的字符串。</param>
    protected void OnUnitOperationEndCalculation(string message)
    {
        var args = new UnitOperationEndCalculationEventArgs(ComponentName, message);
        // if (UnitOperationEndCalculation != null)
        // {
        //     UnitOperationEndCalculation(this, args);
        // }
        UnitOperationEndCalculation?.Invoke(this, args);
    }

    /// <summary>控制 COM 注册的功能。</summary>
    /// <remarks>此函数添加 CAPE-OPEN 方法和工具规范中指定的注册键。特别是，它表明此单元操作实现了 CAPE-OPEN 单元操作类别标识。
    /// 它还使用 <see cref="CapeNameAttribute"/>、<see cref="CapeDescriptionAttribute"/>、<see cref="CapeVersionAttribute"/>、
    /// <see cref="CapeVendorURLAttribute"/>、<see cref="CapeHelpURLAttribute"/>、<see ref="CapeAboutAttribute"/> 属性添加 CapeDescription 注册键。</remarks>
    /// <param name="t">正在注册的类类型。</param> 
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [ComRegisterFunction]
    public new static void RegisterFunction(Type t)
    {
        var assembly = t.Assembly;
        var versionNumber = (new System.Reflection.AssemblyName(assembly.FullName)).Version.ToString();

        var keyName = string.Concat("CLSID\\{", t.GUID.ToString(), "}\\Implemented Categories");
        var catidKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(keyName, true);
        catidKey.CreateSubKey(COGuids.CapeOpenComponent_CATID);
        catidKey.CreateSubKey(COGuids.CapeUnitOperation_CATID);

        keyName = string.Concat("CLSID\\{", t.GUID.ToString(), "}\\InprocServer32");
        var key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(keyName, true);
        var keys = key.GetSubKeyNames();
        foreach (var pT in keys)
        {
            if (pT == versionNumber)
            {
                key.DeleteSubKey(pT);
            }
        }
        key.SetValue("CodeBase", assembly.CodeBase);
        key.Close();

        key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(string.Concat("CLSID\\{", t.GUID.ToString(), "}"), true);
        keyName = string.Concat("CLSID\\{", t.GUID.ToString(), "}\\CapeDescription");

        var attributes = t.GetCustomAttributes(false);
        var nameInfoString = t.FullName;
        var descriptionInfoString = "";
        var versionInfoString = "";
        var companyURLInfoString = "";
        var helpURLInfoString = "";
        var aboutInfoString = "";
        foreach (var mT in attributes)
        {
            switch (mT)
            {
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
                case CapeNameAttribute capeNameAttribute:
                    nameInfoString = capeNameAttribute.Name;
                    break;
                case CapeDescriptionAttribute capeDescriptionAttribute:
                    descriptionInfoString = capeDescriptionAttribute.Description;
                    break;
                case CapeVersionAttribute capeVersionAttribute:
                    versionInfoString = capeVersionAttribute.Version;
                    break;
                case CapeVendorURLAttribute capeVendorUrlAttribute:
                    versionInfoString = capeVendorUrlAttribute.VendorURL;
                    break;
                case CapeHelpURLAttribute capeHelpUrlAttribute:
                    helpURLInfoString = capeHelpUrlAttribute.HelpURL;
                    break;
                case CapeAboutAttribute capeAboutAttribute:
                    aboutInfoString = capeAboutAttribute.About;
                    break;
            }
        }
        catidKey.Close();
        key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(keyName);
        key.SetValue("Name", nameInfoString);
        key.SetValue("Description", descriptionInfoString);
        key.SetValue("CapeVersion", versionInfoString);
        key.SetValue("ComponentVersion", versionNumber);
        key.SetValue("VendorURL", companyURLInfoString);
        key.SetValue("HelpURL", helpURLInfoString);
        key.SetValue("About", aboutInfoString);
        key.Close();
    }

    /// <summary>此功能用于控制在卸载类时从 COM 注册表中删除该类。</summary>
    /// <remarks>该方法将删除类注册中添加的所有子键，包括在 <see cref="RegisterFunction"/> 方法中添加的 CAPE-OPEN 特定键。</remarks>
    /// <param name="t">该类未注册。</param> 
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [ComUnregisterFunction]
    public new static void UnregisterFunction(Type t)
    {
        var keyName = string.Concat("CLSID\\{", t.GUID.ToString(), "}");
        var key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(keyName, true);
        var keyNames = key.GetSubKeyNames();
        foreach (var mT in keyNames)
        {
            key.DeleteSubKeyTree(mT);
        }
        var valueNames = key.GetValueNames();
        foreach (var pT in valueNames)
        {
            key.DeleteValue(pT);
        }
    }

    /// <summary>如果可用，显示 PMC 图形界面。</summary>
    /// <remarks><para>默认情况下，此方法会抛出一个 <see cref="CapeNoImplException"/>，根据 CAPE-OPEN 规范，
    /// 该异常被过程建模环境解释为 PMC 没有编辑器 GUI，而 PME 必须执行编辑步骤。</para>
    /// <para>为了使 PMC 提供自己的编辑器，需要重写 Edit 方法以创建图形编辑器。当用户请求显示编辑器的流程图时，将调用此方法来编辑单元。
    /// 重写类不应返回失败（抛出和异常），因为这将被流程图工具解释为单元没有提供自己的编辑器。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    public override DialogResult Edit()
    {
        var editor = new BaseUnitEditor(this);
        editor.ShowDialog();
        return editor.DialogResult;
    }

    /// <summary>获取单元操作端口集合。</summary>
    /// <remarks><para>将接口返回给包含单元端口列表的集合（例如 <see cref="PortCollection"/>）。</para>
    /// <para>返回单元端口的集合（即 ICapeCollection）。这些作为暴露接口 <see cref="ICapeUnitPort"/> 的元素的集合交付。</para></remarks>
    /// <value>The port collection of the unit operation.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    //  [System.ComponentModel.EditorAttribute(typeof(capePortCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [Category("ICapeUnit")]
    [Description("Unit Operation Port Collection. Click on the (...) button to edit collection.")]
    [TypeConverter(typeof(PortCollectionTypeConverter))]
    public PortCollection Ports { get; }  // get { return m_Ports; }

    /// <summary>获取标志位以指示单元操作的验证状态。</summary>
    /// <remarks><para>获取指示流程表单元是否有效的标志（例如，某些参数值已更改，但尚未使用 Validate 进行验证）。它有三种可能值：</para>
    /// <list type="bullet"> 
    /// <item>notValidated(CAPE_NOT_VALIDATED)</item>
    /// <description>该单元的 validate() 方法自上次可能更改该单元验证状态的操作（例如更新到端口的连接参数值）以来尚未调用。</description>
    /// <item>invalid(CAPE_INVALID)</item>
    /// <description>该单元的 validate() 方法上次被调用时返回了 false。</description>
    /// <item>valid(CAPE_VALID)</item>
    /// <description>该单元的 validate() 方法上次被调用时返回 true。</description>
    /// </list></remarks>
    /// <value>一个指示单元操作验证状态的标志。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    /// <see cref= "CapeValidationStatus">CapeValidationStatus</see>.
    [Category("ICapeUnit")]
    [Description("Validation status of the unit. Either CAPE_VALID, CAPE_NOT_VALIDATED or CAPE_INVALID")]
    public virtual CapeValidationStatus ValStatus => _mValStatus;

    /// <summary>获取在单元操作的最后一次验证过程中返回的消息。</summary>
    /// <remarks>获取从上次尝试验证流程图单元（例如，某些参数值已更改，但尚未使用 Validate 进行验证）返回的消息。</remarks>
    /// <value>在最后一次对单元操作进行验证时返回的消息。</value>
    /// <see cref= "CapeValidationStatus">CapeValidationStatus</see>.
    [Category("ICapeUnit")]
    [Description("Validation message of the unit.")]
    public virtual string ValidationMessage => MValidationMessage;

    /// <summary>执行单元操作模型中涉及的必要计算。</summary>
    /// <remarks>该方法由 PME 调用以执行单元操作的运算。计算过程首先触发 <see cref="UnitOperationBeginCalculation"/> 事件。
    /// 在事件触发后，调用 <see cref="OnCalculate"/> 方法。派生类必须实现 <see cref="OnCalculate"/> 方法。在单元完成其计算后，
    /// 该方法会触发 <see cref="UnitOperationEndCalculation"/>事件。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeTimeOut">ECapeTimeOut</exception>
    /// <exception cref="ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeUnitCOM.Calculate()
    {
        OnUnitOperationBeginCalculation("Starting Calculation");
        OnCalculate();
        OnUnitOperationEndCalculation("Calculation completed normally.");
    }
    
    /// <summary>执行单元操作模型中涉及的必要计算。</summary>
    /// <remarks>该方法由 PME 调用以执行单元操作的运算。计算过程首先触发 <see cref="UnitOperationBeginCalculation"/> 事件。
    /// 在事件触发后，调用 <see cref="OnCalculate"/> 方法。派生类必须实现 <see cref="OnCalculate"/> 方法。在单元完成其计算后，
    /// 该方法会触发 <see cref="UnitOperationEndCalculation"/>事件。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeTimeOut">ECapeTimeOut</exception>
    /// <exception cref="ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeUnit.Calculate()
    {
        OnUnitOperationBeginCalculation("Starting Calculation");
        OnCalculate();
        OnUnitOperationEndCalculation("Calculation completed normally.");
    }

    /// <summary>执行单元操作模型中涉及的必要计算。</summary>
    /// <remarks>该方法由 PME 调用以执行单元操作的运算。计算过程首先触发 <see cref="UnitOperationBeginCalculation"/> 事件。
    /// 在事件触发后，调用 <see cref="OnCalculate"/> 方法。派生类必须实现 <see cref="OnCalculate"/> 方法。在单元完成其计算后，
    /// 该方法会触发 <see cref="UnitOperationEndCalculation"/>事件。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeTimeOut">ECapeTimeOut</exception>
    /// <exception cref="ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    private void OnCalculate()
    {
        OnUnitOperationBeginCalculation("Starting Calculation");
        Calculate();
        OnUnitOperationEndCalculation("Calculation completed normally.");
    }

    /// <summary>该方法由 Calculate 方法调用，以执行单位操作模型中涉及的必要计算。</summary>
    /// <remarks><para>流程图单元执行其计算，即计算输入和输出流完整描述中在此阶段缺失的变量，并计算任何需要显示的公共参数值。
    /// OnCalculate 将能够根据需要使用模拟上下文进行进度监控和中断检查。目前，对此没有达成一致的标准。</para>
    /// <para>建议流程图单元对所有输出流进行适当的闪点计算。在某些情况下，模拟执行器将能够进行闪点计算，但流程图单元的作者最适合决定使用正确的闪点。</para>
    /// <para>在进行计算之前，此方法应执行任何必要的最终验证测试。例如，此时可以检查连接到端口的物流对象的合法性。</para>
    /// <para>此方法没有输入或输出参数。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeTimeOut">ECapeTimeOut</exception>
    /// <exception cref="ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    protected abstract void Calculate();

    /// <summary>验证单元操作。</summary>
    /// <remarks><para>设置标志，通过验证流程图单元的端口和参数来确定流程图单元是否有效。例如，此方法可以检查所有必填端口是否已连接，并且所有参数值是否在范围内。</para>
    /// <para>请注意，Simulation Executive 可以在任何时间调用 Validate 例行程序，特别是在 Executive 准备好调用 Calculate 方法之前。
    /// 这意味着在调用 Validate 时，连接到单元端口的材料对象可能没有正确配置。推荐的方法是使用此方法验证参数和端口，而不是材料对象配置。
    /// 可以在 Calculate 方法中实现检查材料对象的第二个验证级别，当可以合理预期连接到端口的材料对象将正确配置时。</para>
    /// <para>该方法的基类实现会遍历端口和参数集合，并调用每个成员的 <see cref="Validate"/> 方法。如果所有端口和参数都是有效的，
    /// 则单元是有效的，这由 Validate 方法返回 <c>true</c> 表示。</para></remarks>
    /// <returns><para>如果该单元操作有效，则返回 true。</para>
    /// <para>如果该单元操作无效，则返回 false。</para></returns>
    /// <param name="message">引用一个字符串，该字符串将包含关于参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadCOParameter">ECapeBadCOParameter</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    public override bool Validate(ref string message)
    {
        // m_dirty = true;
        UnitOperationValidatedEventArgs args;
        if (!base.Validate(ref message))
        {
            args = new UnitOperationValidatedEventArgs(ComponentName, message, _mValStatus, CapeValidationStatus.CAPE_INVALID);
            _mValStatus = CapeValidationStatus.CAPE_INVALID;
            OnUnitOperationValidated(args);
            MValidationMessage = message;
            return false;
        }
        foreach (var pT in Ports)
        {
            if (pT.connectedObject != null) continue;
            message = string.Concat("Port ", ((CapeIdentification)pT).ComponentName, " does not have a connected object.");
            MValidationMessage = message;
            args = new UnitOperationValidatedEventArgs(ComponentName, message, _mValStatus, CapeValidationStatus.CAPE_INVALID);
            _mValStatus = CapeValidationStatus.CAPE_INVALID;
            OnUnitOperationValidated(args);
            return false;
        }
        message = "Unit is valid.";
        MValidationMessage = message;
        args = new UnitOperationValidatedEventArgs(ComponentName, message, _mValStatus, CapeValidationStatus.CAPE_INVALID);
        _mValStatus = CapeValidationStatus.CAPE_VALID;
        OnUnitOperationValidated(args);
        return true;
    }

    /// <summary>获取单元操作的可能报告列表。</summary>
    /// <remarks>获取单元操作的可能报告列表。</remarks>
    /// <value>该单元操作的可能报告列表。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [Category("ICapeUnitReport")]
    [Description("Reports available for the unit.")]
    public virtual System.Collections.Generic.List<string> Reports => _mReports;

    /// <summary>获取并设置单元操作的当前活动报告。</summary>
    /// <remarks>获取并设置单元操作的当前活动报告。</remarks>
    /// <value>当前单元操作的活跃报告。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    [TypeConverter(typeof(SelectedReportConverter))]
    [Description("Name of the report generated by the unit.")]
    [Category("ICapeUnitReport")]
    public virtual string selectedReport
    {
        get => _mSelecetdReport;
        set => _mSelecetdReport = value;
    }

    /// <summary>生成该单元操作的活动报告。</summary>
    /// <remarks>生成指定的报告。如果未设置值，则生成默认报告。</remarks>
    /// <returns>包含当前选定报告文本的字符串。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    public virtual string ProduceReport()
    {
        var retVal = string.Empty;
        var validMessage = string.Empty;
        var valid = Validate(ref validMessage);
        var status = ValStatus;
        retVal = string.Concat(retVal, "Unit Operation: ", ComponentName, Environment.NewLine);
        retVal = string.Concat(retVal, "Description: ", ComponentDescription, Environment.NewLine, Environment.NewLine);
        retVal = string.Concat(retVal, "Validation Status: ", ValStatus.ToString(), Environment.NewLine);
        retVal = string.Concat(retVal, " ", validMessage, Environment.NewLine, Environment.NewLine);
        retVal = string.Concat(retVal, "Parameters: ", Environment.NewLine);

        for (var i = 0; i < Parameters.Count; i++)
        {
            var pParam = Parameters[i];
            var pParamId = (CapeIdentification)pParam;
            retVal = string.Concat(retVal, "Parameter", i.ToString(), ": ", pParamId.ComponentName, Environment.NewLine);
            retVal = string.Concat(retVal, "Description: ", pParamId.ComponentDescription, Environment.NewLine);
            var pSpec = (ICapeParameterSpec)pParam.Specification;
            retVal = string.Concat(retVal, "Type: ", pSpec.Type.ToString(), Environment.NewLine);
            valid = pParam.Validate(validMessage);
            retVal = string.Concat(retVal, "Validation Status: ", pParam.ValStatus.ToString(), Environment.NewLine);
            retVal = string.Concat(retVal, " ", validMessage, Environment.NewLine);
            switch (pSpec.Type)
            {
                case CapeParamType.CAPE_ARRAY:
                {
                    retVal = string.Concat(retVal, "Values: ", Environment.NewLine);
                    if (pParam.value is object[] pParamValue)
                    {
                        foreach (var pT in pParamValue)
                        {
                            if (pT is ICapeParameter pParamArrayElement)
                            {
                                retVal = string.Concat(retVal, string.Concat(" ", pParamArrayElement.value.ToString(), Environment.NewLine));
                            }
                            else
                            {
                                retVal = string.Concat(retVal, string.Concat(" ", pT.ToString(), Environment.NewLine));
                            }
                        }
                    }

                    break;
                }
                case CapeParamType.CAPE_REAL:
                {
                    var pReal = (ICapeRealParameterSpecCOM)pSpec;
                    if (pParam is RealParameter pRealParam)
                    {
                        retVal = string.Concat(retVal, "Value: ", pRealParam.DimensionedValue.ToString(CultureInfo.CurrentCulture), Environment.NewLine);
                        retVal = string.Concat(retVal, "Dimensionality: ", pRealParam.Unit, Environment.NewLine);
                    }
                    else
                    {
                        retVal = string.Concat(retVal, "Value: ", pParam.value.ToString(), Environment.NewLine);
                        retVal = string.Concat(retVal, "Dimensionality: ", pSpec.Dimensionality.ToString(), Environment.NewLine);
                    }
                    retVal = string.Concat(retVal, "Default Value: ", pReal.DefaultValue, Environment.NewLine);
                    retVal = string.Concat(retVal, "Lower Bound: ", pReal.LowerBound, Environment.NewLine);
                    retVal = string.Concat(retVal, "Upper Bound: ", pReal.UpperBound, Environment.NewLine);
                    break;
                }
                case CapeParamType.CAPE_INT:
                {
                    retVal = string.Concat(retVal, "Value: ", pParam.value.ToString(), Environment.NewLine);
                    var pIntParam = (ICapeIntegerParameterSpec)pSpec;
                    retVal = string.Concat(retVal, "Default Value: ", pIntParam.DefaultValue, Environment.NewLine);
                    retVal = string.Concat(retVal, "Lower Bound: ", pIntParam.LowerBound, Environment.NewLine);
                    retVal = string.Concat(retVal, "Upper Bound: ", pIntParam.UpperBound, Environment.NewLine);
                    break;
                }
                case CapeParamType.CAPE_BOOLEAN:
                {
                    retVal = string.Concat(retVal, "Value: ", pParam.value.ToString(), Environment.NewLine);
                    var pBool = (ICapeBooleanParameterSpec)pSpec;
                    retVal = string.Concat(retVal, "Default Value: ", pBool.DefaultValue, Environment.NewLine);
                    break;
                }
                case CapeParamType.CAPE_OPTION:
                {
                    retVal = string.Concat(retVal, "Value: ", pParam.value.ToString(), Environment.NewLine);
                    var pOpt = (ICapeOptionParameterSpec)pSpec;
                    retVal = string.Concat(retVal, "Default Value: ", pOpt.DefaultValue, Environment.NewLine);
                    retVal = string.Concat(retVal, pOpt.RestrictedToList 
                        ? "Restricted to List: TRUE" 
                        : "Restricted to List: FALSE", Environment.NewLine);
                    retVal = string.Concat(retVal, "Option List Values: ", Environment.NewLine);
                    var options = pOpt.OptionList;
                    for (var j = 0; j < options.Length; j++)
                    {
                        retVal = string.Concat(retVal, "Option[", j, "]: ", options[j], Environment.NewLine);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            retVal = string.Concat(retVal, Environment.NewLine);
        }
        retVal = string.Concat(retVal, Environment.NewLine, "Ports: ", Environment.NewLine);
        for (var i = 0; i < Ports.Count; i++)
        {
            var pPort = Ports[i];
            var pPortId = (CapeIdentification)pPort;
            retVal = string.Concat(retVal, "Port", i.ToString(), ": ", pPortId.ComponentName, Environment.NewLine);
            retVal = string.Concat(retVal, "Description: ", pPortId.ComponentDescription, Environment.NewLine);
            var pPortConnectedObjectId = (ICapeIdentification)pPort.connectedObject;
            retVal = string.Concat(retVal, "Port Type: ", pPort.portType.ToString(), Environment.NewLine);
            retVal = string.Concat(retVal, "Port Direction: ", pPort.direction.ToString(), Environment.NewLine);
            retVal = pPortConnectedObjectId != null 
                ? string.Concat(retVal, "Connected Object: ", pPortConnectedObjectId.ComponentName, Environment.NewLine) 
                : string.Concat(retVal, "No Connected Object.", Environment.NewLine);
            retVal = string.Concat(retVal, Environment.NewLine);
        }
        return retVal;
    }
}
