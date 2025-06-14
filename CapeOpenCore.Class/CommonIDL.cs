/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.14
 */

// This idl file was ported from the CAPE-OPEN common.idl file and 
// the interfaces were updated to conform to .NET format.
/* IMPORTANT NOTICE
(c) The CAPE-OPEN Laboratory Network, 2002.
All rights are reserved unless specifically stated otherwise

Visit the web site at www.colan.org

This file has been edited using the editor from Microsoft Visual Studio 6.0
This file can view properly with any basic editors and browsers (validation done under MS Windows and Unix)
*/

// This file was developed/modified by JEAN-PIERRE-BELAUD for CO-LaN organisation - August 2003

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CapeOpenCore.Class;

/// <summary>枚举标志，用于指示参数验证状态。</summary>
/// <remarks><para>该枚举具有以下含义：</para>
/// <para>1. notValidated(CAPE_NOT_VALIDATED)：在最后一次更改其值之后，PMC 的 Validate() 方法尚未被调用。</para>
/// <para>2. invalid(CAPE_INVALID): 上次调用 PMC 的 Validate() 方法时，它返回了 false。</para>
/// <para>3. valid(CAPE_VALID): 上次调用 PMC 的 Validate() 方法时，它返回了 true。</para></remarks>
[Serializable, ComVisible(true)]
[Guid(COGuids.CapeValidationStatus_IID)]
public enum CapeValidationStatus
{
    /// <summary>PMC 的 Validate() 方法在最后一次更改其值后尚未被调用。</summary>
    CAPE_NOT_VALIDATED = 0,
    /// <summary>上次调用 PMC 的 Validate() 方法时，它返回了 false。</summary>
    CAPE_INVALID = 1,
    /// <summary>上次调用 PMC 的 Validate() 方法时，它返回了 true。</summary>
    CAPE_VALID = 2
}

// typedef CapeValidationStatus eCapeValidationStatus;
/// <summary>事件触发以指示组件的名称已更改。</summary>
[ComVisible(true)]
[Guid("F79EA405-4002-4fb2-AED0-C1E48793637D")]
[Description("CapeIdentificationEvents Interface")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
internal interface IComponentNameChangedEventArgs
{
    /// <summary>更名前 PMC 的名称。</summary>
    /// <remarks>该单元的旧名称可用于更新 PMC 的图形用户界面（GUI）信息。</remarks>
    /// <value>名称变更前的单位名称。</value>
    string OldName { get; }
    
    /// <summary>更名后的 PMC 名称。</summary>
    /// <remarks>该单元的全新名称可用于更新 PMC 的图形用户界面（GUI）相关信息。</remarks>
    /// <value>更名后的单位名称。</value>
    string NewName { get; }
}

/// <summary>事件触发以指示组件的描述已发生更改。</summary>
[ComVisible(true)]
[Guid("34C43BD3-86B2-46d4-8639-E0FA5721EC5C")]
[Description("CapeIdentificationEvents Interface")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
internal interface IComponentDescriptionChangedEventArgs
{
    /// <summary>PMC 更名前的相关描述。</summary>
    /// <remarks>对该单元的先前描述可用于更新 PMC 的图形用户界面（GUI）信息。</remarks>
    /// <value>在描述变更前的单元描述。</value>
    string OldDescription { get; }
    
    /// <summary>更名后的 PMC 名称。</summary>
    /// <remarks>该单元的描述名称可用于更新 PMC 的图形用户界面（GUI）信息。</remarks>
    /// <value>单元描述在描述更改后的内容。</value>
    string NewDescription { get; }
}

/// <summary>为 CapeIdentification.ComponentNameChanged 事件提供数据。</summary>
/// <remarks>CapeIdentification.NameChangedEventArgs 事件指定 PMC 的旧名称和新名称。</remarks>
[Serializable, ComVisible(true)]
[Guid("D78014E7-FB1D-43ab-B807-B219FAB97E8B")]
[ClassInterface(ClassInterfaceType.None)]
public class ComponentNameChangedEventArgs : EventArgs  //, IComponentNameChangedEventArgs
{
    /// <summary>创建一个 NameChangedEventArgs 类的实例，其中包含旧名称和新名称。</summary>
    /// <remarks>您可以在运行时触发 NameChangedEvent 事件时使用此构造函数，以指定名称被更改的 PMC 的具体名称。</remarks>
    /// <param name="oldName">更名前 PMC 的名称。</param>
    /// <param name="newName">更名后的 PMC 名称。</param>
    public ComponentNameChangedEventArgs(string oldName, string newName)
    {
        OldName = oldName;
        NewName = newName;
    }

    /// <summary>更名前 PMC 的名称。</summary>
    /// <remarks>该单元的旧名称可用于更新 PMC 的图形用户界面（GUI）信息。</remarks>
    /// <value>名称变更前的单位名称。</value>
    public string OldName { get; }

    /// <summary>更名后的 PMC 名称。</summary>
    /// <remarks>该单元的全新名称可用于更新 PMC 的图形用户界面（GUI）相关信息。</remarks>
    /// <value>更名后的单位名称。</value>
    public string NewName { get; }
}

/// <summary>为 CapeIdentification.ComponentDescriptionChanged 事件提供数据。</summary>
/// <remarks>CapeIdentification.DescriptionChangedEventArgs 事件指定 PMC 的旧描述和新描述。</remarks>
[Serializable, ComVisible(true)]
[Guid("0C51C4F1-20E8-413d-93E1-4704B888354A")]
[ClassInterface(ClassInterfaceType.None)]
public class ComponentDescriptionChangedEventArgs : EventArgs, IComponentDescriptionChangedEventArgs
{
    /// <summary>创建一个 DescriptionChangedEventArgs 类的实例，其中包含旧名称和新名称。</summary>
    /// <remarks>您可以在运行时触发 DescriptionChangedEvent 事件时使用此构造函数，以指定名称被更改的 PMC 的具体描述。</remarks>
    /// <param name="oldDescription">PMC 描述变更前的描述内容。</param>
    /// <param name="newDescription">PMC 描述变更后的新描述内容。</param>
    public ComponentDescriptionChangedEventArgs(string oldDescription, string newDescription)
    {
        OldDescription = oldDescription;
        NewDescription = newDescription;
    }

    /// <summary>PMC 更名前的相关描述。</summary>
    /// <remarks>对该单元的先前描述可用于更新 PMC 的图形用户界面（GUI）信息。</remarks>
    /// <value>在描述变更前的单元描述。</value>
    public string OldDescription { get; }

    /// <summary>更名后的 PMC 名称。</summary>
    /// <remarks>该单元的描述名称可用于更新 PMC 的图形用户界面（GUI）信息。</remarks>
    /// <value>单元描述在描述更改后的内容。</value>
    public string NewDescription { get; }
}

/// <summary>提供用于识别和描述 CAPE-OPEN 组件的方法。</summary>
/// <remarks><para>例如，我们提醒来自现有接口规范且与识别概念相关的以下要求：</para>
/// <para>单元操作接口具有以下要求：</para>
/// <para>1. 如果流程图中包含某个类别的单元操作的两个实例，COSE 需要为用户提供一个文本标识符来区分每个实例。
/// 例如，当 COSE 需要报告其中一个单元操作中发生的错误时。</para>
/// <para>2. 当 COSE 向用户显示其图形用户界面（GUI）以将 COSE 的流连接到单元操作端口时，COSE需要向单元请求其可用端口列表。
/// 为了让用户识别这些端口，每个端口都需要一些独特的文本信息。</para>
/// <para>3. 当 COSE 向用户提供接口以浏览或设置单元操作的内部参数值时，COSE 需要向单元请求其可用参数列表。无论该 COSE 的接口是
/// 图形用户界面（GUI）还是编程接口，每个参数都必须通过文本字符串进行标识。</para>
/// <para>ICapeThermoMaterialObject（同时用于 Unit 和 Thermo 接口）：</para>
/// <para>如果单元操作在访问流时遇到错误，参见 <see cref="ICapeThermoMaterialObject"/>，该单元可能会决定向用户报告此错误。
/// 理想情况下，流应具有文本标识符，以便用户能够快速了解是哪个流出现了问题。</para>
/// <para>热力学接口具有以下要求：</para>
/// <para><see cref="ICapeThermoSystem"/> 和 <see cref="ICapeThermoPropertyPackage"/> 接口不需要一个标识接口，因为两者都
/// 被设计为单例（每个组件类需要一个实例）。这意味着没有必要标识这个实例：其类描述就足够了。然而，用户可能仍然决定为在其流程图中使用
/// 的 CAPE-OPEN 属性系统或属性包分配一个名称或描述。此外，如果这些接口发生演变，可以移除单例模式。在这种情况下，必须为每个实例进行标识。</para>
/// <para>求解器接口具有以下要求：</para>
/// <para>许多对象应提供来自识别通用接口的功能。</para>
/// <para>SMST 接口具有以下要求：</para>
/// <para>CO SMST 组件包依赖于识别接口包。接口 ICapeSMSTFactory 必须提供识别功能。</para>
/// <para>参考文档：Identification Common Interface。</para></remarks>
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[ComVisible(true)]
[Guid("5F5087A7-B27B-4b4f-902D-5F66E34A0CBE")]
[Description("CapeIdentificationEvents Interface")]
internal interface ICapeIdentificationEvents
{
    /// <summary>获取并设置组件的名称。</summary>
    /// <remarks><para>在一个系统中，特定的用例可能包含多个同类的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的名称和描述，
    /// 以便以明确且用户友好的方式加以指代。由于并非所有能够设定这些标识的软件组件，以及需要这些信息的其他软件组件，都是由同一家
    /// 供应商开发的，因此需要一个 CAPE-OPEN 标准来设定和获取这些信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的唯一名称。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <param name="sender">引发该事件的 PMC。</param>
    /// <param name="args">一个包含事件信息的 <see cref="ComponentNameChangedEventArgs">ParameterDefaultValueChanged</see>。</param>
    void ComponentNameChanged(
        [MarshalAs(UnmanagedType.IDispatch)]object sender,
        [MarshalAs(UnmanagedType.IDispatch)]object args
    );

    /// <summary>获取并设置组件的描述。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的名称和描述，以便以无歧义
    /// 且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并不总是由同一供应商开发，
    /// 因此需要制定一个 CAPE-OPEN 标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的描述。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <param name="sender">引发该事件的 PMC。</param>
    /// <param name="args">一个包含事件信息的 <see cref="ComponentDescriptionChangedEventArgs">ParameterDefaultValueChanged</see>。</param>
    void ComponentDescriptionChanged(
        [MarshalAs(UnmanagedType.IDispatch)]object sender,
        [MarshalAs(UnmanagedType.IDispatch)]object args
    );
}

/// <summary>提供用于识别和描述 CAPE-OPEN 组件的方法。</summary>
/// <remarks><para>例如，我们提醒来自现有接口规范且与识别概念相关的以下要求：</para>
/// <para>单元操作接口具有以下要求：</para>
/// <para>1. 如果流程图中包含某个类别的单元操作的两个实例，COSE 需要为用户提供一个文本标识符来区分每个实例。
/// 例如，当 COSE 需要报告其中一个单元操作中发生的错误时。</para>
/// <para>2. 当 COSE 向用户显示其图形用户界面（GUI）以将 COSE 的流连接到单元操作端口时，COSE需要向单元请求其可用端口列表。
/// 为了让用户识别这些端口，每个端口都需要一些独特的文本信息。</para>
/// <para>3. 当 COSE 向用户提供接口以浏览或设置单元操作的内部参数值时，COSE 需要向单元请求其可用参数列表。无论该 COSE 的接口
/// 是图形用户界面（GUI）还是编程接口，每个参数都必须通过文本字符串进行标识。</para>
/// <para>ICapeThermoMaterialObject（同时用于Unit和Thermo接口）：</para>
/// <para>如果单元操作在访问流时遇到错误（<see cref="ICapeThermoMaterialObject"/>），该单元可能会决定向用户报告此错误。
/// 理想情况下，流应具有文本标识符，以便用户能够快速了解是哪个流出现了问题。</para>
/// <para>热力学接口具有以下要求：</para>
/// <para><see cref="ICapeThermoSystem"/> 和 <see cref="ICapeThermoPropertyPackage"/> 接口不需要一个标识接口，因为两者都
/// 被设计为单例（每个组件类需要一个实例）。这意味着没有必要标识这个实例：其类描述就足够了。然而，用户可能仍然决定为在其流程图中使用
/// 的 CAPE-OPEN 属性系统或属性包分配一个名称或描述。此外，如果这些接口发生演变，可以移除单例模式。在这种情况下，必须为每个实例进行标识。</para>
/// <para>求解器接口具有以下要求：</para>
/// <para>许多对象应提供来自识别通用接口的功能。</para>
/// <para>SMST 接口具有以下要求：</para>
/// <para>CO SMST 组件包依赖于识别接口包。接口 ICapeSMSTFactory 必须提供识别功能。</para>
/// <para>参考文档：Identification Common Interface。</para></remarks>
[ComImport, ComVisible(true)]
[Guid(COGuids.CapeIdentification_IID)]
[Description("CapeIdentification Interface")]
public interface ICapeIdentification
{
    /// <summary>获取并设置组件的名称。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的名称和描述，
    /// 以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并不总是由同一供应商开发，
    /// 因此需要制定一个 CAPE-OPEN 标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的唯一名称。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [DispId(1), Description("Property ComponentName")]
    string ComponentName { get; set; }

    /// <summary>获取并设置组件的描述。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的名称和描述，
    /// 以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并不总是由同一供应商开发，
    /// 因此需要制定一个 CAPE-OPEN 标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的描述。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [DispId(1), Description("Property ComponentName")]
    string ComponentDescription { get; set; }
}

/// <summary>表示用于处理组件名称更改的方法。</summary>
/// <remarks>当您创建一个 ComponentNameChangedHandler 委托时，您需要指定一个方法来处理该事件。要将事件与您的事件处理程序关联，
/// 请将该委托的实例添加到事件中。每当事件发生时，事件处理程序都会被调用，除非您移除了该委托。有关委托的更多信息，请参阅《事件与委托》。</remarks>
/// <param name="sender">作为源头的 PMC。</param>
/// <param name="args">一个 <see cref="ComponentNameChangedEventArgs">NameChangedEventArgs</see>，其中提供了有关名称更改的信息。</param>
[ComVisible(true)]
public delegate void ComponentNameChangedHandler(object sender, ComponentNameChangedEventArgs args);

/// <summary>表示用于处理组件描述更改的方法。</summary>
/// <remarks>当您创建一个 ComponentNameChangedHandler 委托时，您需要指定一个方法来处理该事件。要将事件与您的事件处理程序关联，
/// 请将该委托的实例添加到事件中。每当事件发生时，事件处理程序都会被调用，除非您移除了该委托。有关委托的更多信息，请参阅《事件与委托》。</remarks>
/// <param name="sender">引发该事件的 PMC。</param>
/// <param name="args">一个 <see cref="ComponentDescriptionChangedEventArgs">DescriptionChangedEventArgs</see>，其中提供了关于描述更改的信息。</param>
[ComVisible(true)]
public delegate void ComponentDescriptionChangedHandler(object sender, ComponentDescriptionChangedEventArgs args);

/// <summary>提供用于识别和描述 CAPE-OPEN 组件的方法。</summary>
/// <remarks><para>允许用户为每个 PMC 实例分配不同的名称和描述，以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与
/// 需要此信息的软件组件并不总是由同一供应商开发，因此需要一个 CAPE-OPEN 标准来设置和获取此信息。</para>
/// <para>参考文档：Identification Common Interface。</para></remarks>
[Serializable, ComVisible(true)]
[ComSourceInterfaces(typeof(ICapeIdentificationEvents), typeof(INotifyPropertyChanged))]
[Guid("BF54DF05-924C-49a5-8EBB-733E37C38085")]
[Description("CapeIdentification Interface")]
[ClassInterface(ClassInterfaceType.None)]
public abstract class CapeIdentification : //System.ComponentModel.Component,
    ICapeIdentification, IDisposable, ICloneable, INotifyPropertyChanged
{
    /// <summary>组件的名称。</summary>
    private string _mComponentName;
    /// <summary>组件的描述。</summary>
    private string _mComponentDescription;        
    /// <summary>跟踪是否已调用 Dispose 方法。</summary>
    private bool _disposed;

    /// <summary>创建一个 CapeIdentification 类的实例，并为 PMC 的名称和描述设置默认值。</summary>
    /// <remarks>这个构造函数使用正在被构造的 PMC 对象的 <see cref="System.Type"/> 作为 ComponentName 和 ComponentDescription
    /// 属性的默认值。如果 PMC 对象具有 <see cref="CapeNameAttribute"/>，那么将使用 <see cref="CapeNameAttribute.Name"/> 属性来获取名称。
    /// 同样，如果对象具有 <see cref=" CapeDescriptionAttribute"/>，那么将使用 <see cref="CapeDescriptionAttribute.Description"/> 属性来获取描述。</remarks>
    protected CapeIdentification()
    {
        _disposed = false;
        _mComponentName = GetType().FullName;
        _mComponentDescription = GetType().FullName;
        var attributes = GetType().GetCustomAttributes(false);
        foreach (var pT in attributes)
        {
            switch (pT)
            {
                case CapeNameAttribute pNameAttribute:
                    _mComponentName = pNameAttribute.Name;
                    break;
                case CapeDescriptionAttribute pDescriptionAttribute:
                    _mComponentDescription = pDescriptionAttribute.Description;
                    break;
            }
        }
    }

    /// <summary>Creates an instance of the CapeIdentification class with the name and a default description of the PMC.</summary>
    /// <remarks>This constructor uses the provided name for the ComponentName of the PMC object being constructed. A 
    /// default value for the ComponentDescription properties is then assigned. If the PMC object has a 
    /// <see cref="CapeDescriptionAttribute"/>, then the <see cref="CapeDescriptionAttribute.Description"/> 
    /// property is used for the description.</remarks>
    /// <param name="name">The name of the PMC.</param>
    public CapeIdentification(string name)
    {
        _disposed = false;
        _mComponentName = name;
        _mComponentDescription = GetType().FullName;
        object[] attributes = GetType().GetCustomAttributes(false);
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i] is CapeDescriptionAttribute) _mComponentDescription = ((CapeDescriptionAttribute)attributes[i]).Description;
        }
    }

    /// <summary
    /// >Creates an instance of the CapeIdentification class with the name and description of the PMC.</summary>
    /// <remarks>You can use this constructor to specify a specific name and description of the PMC.</remarks>
    /// <param name="name">The name of the PMC.</param>
    /// <param name="description">The description of the PMC.</param>
    public CapeIdentification(string name, string description)
    {
        _disposed = false;
        _mComponentName = name;
        _mComponentDescription = description;
    }


    /// <summary>Copy constructor of the CapeIdentification class.</summary>
    /// <remarks>Creates an instance of the CapeIdentification class with ComponentName equal to the original PMC's 
    /// ComponentName + (Copy). The copy has the same CapeDescription as the original.</remarks>
    /// <param name="objectToBeCopied">The object being copied.</param>
    protected CapeIdentification(CapeIdentification objectToBeCopied)
    {
        _disposed = false;
        _mComponentName = objectToBeCopied.ComponentName + "(Copy)";
        _mComponentDescription = objectToBeCopied.ComponentDescription;
    }
        
    /// <summary>Creates a new object that is a copy of the current instance.</summary>
    /// <remarks><para>
    /// Clone can be implemented either as a deep copy or a shallow copy. In a deep copy, all objects are duplicated; 
    /// in a shallow copy, only the top-level objects are duplicated and the lower levels contain references.
    /// </para>
    /// <para>
    /// The resulting clone must be of the same type as, or compatible with, the original instance.
    /// </para>
    /// <para>
    /// See <see cref="Object.MemberwiseClone"/> for more information on cloning, deep versus shallow copies, and examples.
    /// </para></remarks>
    /// <returns>A new object that is a copy of this instance.</returns>
    public abstract object Clone();

    // Implement IDisposable.
    // Do not make this method virtual.
    // A derived class should not be able to override this method.
    /// <summary>Releases all resources used by the CapeIdentification object.</summary>
    /// <remarks>Call Dispose when you are finished using the CapeIdentification object. The Dispose method 
    /// leaves the CapeIdentification object in an unusable state. After calling Dispose, you must release 
    /// all references to the Component so the garbage collector can reclaim the memory that the CapeIdentification 
    /// object was occupying. For more information, see <see href="http://msdn.microsoft.com/en-us/library/498928w2.aspx">
    /// Cleaning Up Unmanaged Resources and Implementing a Dispose Method.</see></remarks> 
    public void Dispose()
    {
        Dispose(true);
        // This object will be cleaned up by the Dispose method.
        // Therefore, you should call GC.SupressFinalize to
        // take this object off the finalization queue
        // and prevent finalization code for this object
        // from executing a second time.
        GC.SuppressFinalize(this);
    }

    // Dispose(bool disposing) executes in two distinct scenarios.
    // If disposing equals true, the method has been called directly
    // or indirectly by a user's code. Managed and unmanaged resources
    // can be disposed.
    // If disposing equals false, the method has been called by the
    // runtime from inside the finalizer and you should not reference
    // other objects. Only unmanaged resources can be disposed.
    /// <summary>Releases the unmanaged resources used by the CapeIdentification object and optionally releases 
    /// the managed resources.</summary>
    /// <remarks><para>This method is called by the public <see href="http://msdn.microsoft.com/en-us/library/system.componentmodel.component.dispose.aspx">Dispose</see>see> 
    /// method and the <see href="http://msdn.microsoft.com/en-us/library/system.object.finalize.aspx">Finalize</see> method. 
    /// <bold>Dispose()</bold> invokes the protected <bold>Dispose(Boolean)</bold> method with the disposing
    /// parameter set to <bold>true</bold>. <see href="http://msdn.microsoft.com/en-us/library/system.object.finalize.aspx">Finalize</see> 
    /// invokes <bold>Dispose</bold> with disposing set to <bold>false</bold>.</para>
    /// <para>When the <italic>disposing</italic> parameter is <bold>true</bold>, this method releases all 
    /// resources held by any managed objects that this Component references. This method invokes the 
    /// <bold>Dispose()</bold> method of each referenced object.</para>
    /// <para><bold>Notes to Inheritors</bold></para>
    /// <para><bold>Dispose</bold> can be called multiple times by other objects. When overriding 
    /// <bold>Dispose(Boolean)</bold>, be careful not to reference objects that have been previously 
    /// disposed of in an earlier call to <bold>Dispose</bold>. For more information about how to 
    /// implement <bold>Dispose(Boolean)</bold>, see <see href="http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx">Implementing a Dispose Method</see>.</para>
    /// <para>For more information about <bold>Dispose</bold> and <see href="http://msdn.microsoft.com/en-us/library/system.object.finalize.aspx">Finalize</see>, 
    /// see <see href="http://msdn.microsoft.com/en-us/library/498928w2.aspx">Cleaning Up Unmanaged Resources</see> 
    /// and <see href="http://msdn.microsoft.com/en-us/library/ddae83kx.aspx">Overriding the Finalize Method</see>.</para></remarks> 
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if (!_disposed)
        {
            //If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
                //component.Dispose();
            }
            // Note disposing has been done.
            _disposed = true;
        }
    }

    /// <summary>Notifies the collection that the value of a property of the parameter has been changed.</summary>
    /// <remarks>The PropertyChanged event can indicate all properties on the object have changed by using either 
    /// null or String.Empty as the property name in the PropertyChangedEventArgs.</remarks>
    public event PropertyChangedEventHandler PropertyChanged;

    // This method is called by the Set accessor of each property. 
    // The CallerMemberName attribute that is applied to the optional propertyName 
    // parameter causes the property name of the caller to be substituted as an argument. 
    /// <summary>Notifies the collection that the value of a proparty of the parameter has been changed.</summary>
    /// <remarks>The PropertyChanged event can indicate all properties on the object have changed by using either 
    /// null or String.Empty as the property name in the PropertyChangedEventArgs.</remarks>
    /// <param name="propertyName">The name of the property that was chnaged.</param>
    protected void NotifyPropertyChanged(/* .Net 4.5 [System.Runtime.CompilerServices.CallerMemberName]*/ string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>Occurs when the user changes of the name of a component.</summary>
    /// <remarks>The event to be handles when the name of the PMC is changed.</remarks> 
    public event ComponentNameChangedHandler ComponentNameChanged;

    /// <summary>Occurs when the user changes of the description of a component.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnComponentNameChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnComponentNameChanged</c> in a derived class, be sure to call the base class's <c>OnComponentNameChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ComponentNameChangedEventArgs">NameChangedEventArgs</see> that contains information about the event.</param>
    protected void OnComponentNameChanged(ComponentNameChangedEventArgs args)
    {
        if (ComponentNameChanged != null)
        {
            ComponentNameChanged(this, args);
        }
    }

    /// <summary>Occurs when the user changes of the description of a component.</summary>
    /// <remarks>The event to be handles when the description of the PMC is changed.</remarks> 
    public event ComponentDescriptionChangedHandler ComponentDescriptionChanged;

    /// <summary>Occurs when the user changes of the description of a component.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnComponentDescriptionChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnComponentDescriptionChanged</c> in a derived class, be sure to call the base class's <c>OnComponentDescriptionChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ComponentDescriptionChangedEventArgs">DescriptionChangedEventArgs</see> that contains information about the event.</param>
    protected void OnComponentDescriptionChanged(ComponentDescriptionChangedEventArgs args)
    {
        if (ComponentDescriptionChanged != null)
        {
            ComponentDescriptionChanged(this, args);
        }
    }

    /// <summary>获取并设置组件的名称。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的CAPE-OPEN组件。用户应能够为每个实例分配不同的名称和描述，以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并不总是由同一供应商开发，因此需要制定一个CAPE-OPEN标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的唯一名称。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("Identification")]
    public virtual string ComponentName
    {
        get
        {
            return _mComponentName;
        }

        set
        {
            ComponentNameChangedEventArgs args = new ComponentNameChangedEventArgs(_mComponentName, value);
            _mComponentName = value;
            NotifyPropertyChanged("ComponentName");
            OnComponentNameChanged(args);
        }
    }

    /// <summary> 获取并设置组件的描述。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的CAPE-OPEN组件。用户应能够为每个实例分配不同的名称和描述，以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并不总是由同一供应商开发，因此需要制定一个CAPE-OPEN标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的描述。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("Identification")]
    public virtual string ComponentDescription
    {
        get
        {
            return _mComponentDescription;
        }
        set
        {
            ComponentDescriptionChangedEventArgs args = new ComponentDescriptionChangedEventArgs(_mComponentDescription, value);
            _mComponentDescription = value;
            NotifyPropertyChanged("ComponentDescription");
            OnComponentDescriptionChanged(args);
        }
    }
}


/// <summary>This interface provides the behaviour for a read-only collection. It can be 
/// used for storing ports or parameters.</summary>
/// <remarks><para>The aim of the Collection interface is to give a CAPE-OPEN component 
/// the possibility to expose a list of objects to any client of the component. 
/// The client will not be able to modify the collection, i.e. removing, 
/// replacing or adding elements. However, since the client will have access to 
/// any CAPE-OPEN interface exposed by the items of the collection, it will be 
/// able to modify the state of any element.</para>
/// <para>CAPE-OPEN Collections don’t allow exposing basic types such as 
/// numerical values or strings. Indeed, using CapeArrays is more convenient 
/// here.</para>
/// <para>Not all the items of a collection must belong to the same class. It is 
/// enough if they implement the same interface or set of interfaces. A CAPE-OPEN 
/// specification a component that exposes a collection interface must state 
/// clearly which interfaces must be implemented by all the items of the 
/// collection.</para>
/// <para>Reference document: Collection Common Interface</para></remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeCollection_IID)]
[Description("ICapeCollection Interface")]
interface ICapeCollection
{
    /// <summary>
    ///	Gets the specific item stored within the collection, identified by its 
    /// ICapeIdentification.ComponentName or 1-based index passed as an argument 
    /// to the method.</summary>
    /// <remarks>Return an element from the collection. The requested element can be 
    /// identified by its actual name (e.g. type CapeString) or by its position 
    /// in the collection (e.g. type CapeLong). The name of an element is the 
    /// value returned by the ComponentName() method of its ICapeIdentification 
    /// interface. The advantage of retrieving an item by name rather than by 
    /// position is that it is much more efficient. This is because it is faster 
    /// to check all names from the server part than checking then from the 
    /// client, where a lot of COM/CORBA calls would be required.</remarks>
    /// <param name="index">
    /// <para>Identifier for the requested item:</para>
    /// <para>name of item (the variant contains a string)</para>
    /// <para>position in collection (it contains a long)</para>
    /// </param>
    /// <returns>
    /// System.Object containing the requested collection item.
    /// </returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeOutOfBounds">ECapeOutOfBounds</exception>
    [DispId(1)]
    [Description("gets an item specified by index or name")]
    [return: MarshalAs(UnmanagedType.IDispatch)]// This attribute specifices the return value is an IDispatch pointer.
    object Item(object index);

    /// <summary>
    ///	Gets the number of items currently stored in the collection.</summary>
    /// <remarks>Return the number of items in the collection.</remarks>
    /// <returns>Return the number of items in the collection.</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    [DispId(2)]
    [Description("Number of items in the collection")]
    int Count();
}
/*
   /// <summary>
   /// Represents the method that will handle the changing the name of a component.
   /// </summary>
   /// <remarks>
   /// When you create a ComponentNameChangedHandler delegate, you identify the method that will handle the event. To associate the event with your event handler, add an
   /// instance of the delegate to the event. The event handler is called whenever the event occurs, unless you remove the delegate. For more information about delegates,
   /// see Events and Delegates.
   /// </remarks>
   /// <param name="sender">The PMC that is the source .</param>
   /// <param name="args">A <see cref="System.ComponentModel.AddingNewEventArgs">System.ComponentModel.AddingNewEventArgs</see> that provides information about the name change.</param>
   [System.Runtime.InteropServices.ComVisibleAttribute(true)]
   public delegate void CollectionAddingNewHandler(Object sender, System.ComponentModel.AddingNewEventArgs args);


   /// <summary>
   /// Represents the method that will handle the changing the name of a component.
   /// </summary>
   /// <remarks>
   /// When you create a ComponentNameChangedHandler delegate, you identify the method that will handle the event. To associate the event with your event handler, add an
   /// instance of the delegate to the event. The event handler is called whenever the event occurs, unless you remove the delegate. For more information about delegates,
   /// see Events and Delegates.
   /// </remarks>
   /// <param name="sender">The PMC that is the source .</param>
   /// <param name="args">A <see cref="System.ComponentModel.ListChangedEventArgs">System.ComponentModel.ListChangedEventArgs</see> that provides information about the name change.</param>
   [System.Runtime.InteropServices.ComVisibleAttribute(true)]
   public delegate void CollectionListChangedHandler(Object sender, System.ComponentModel.ListChangedEventArgs args);

   /// <summary>
   /// </summary>
   /// <remarks>
   /// </remarks>
   [System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIDispatch)]
   [System.Runtime.InteropServices.ComVisibleAttribute(true)]
   [System.Runtime.InteropServices.GuidAttribute("DE9CDE6E-A2D4-4BFF-AA3A-8699FCF3E0EB")]
   [System.ComponentModel.DescriptionAttribute("CapeCollectionEvents Interface")]
   interface ICapeCollectionEvents
   {
       /// <summary>
       /// Occurs when the user changes of the value of a paramter.
       /// </summary>
       /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
       /// <para>The <c>OnComponentNameChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred
       /// technique for handling the event in a derived class.</para>
       /// <para>Notes to Inheritors: </para>
       /// <para>When overriding <c>OnParameterValueChanged</c> in a derived class, be sure to call the base class's <c>OnParameterValueChanged</c> method so that registered
       /// delegates receive the event.</para>
       /// </remarks>
       /// <param name="sender">The <see cref="RealParameter">RealParameter</see> that raised the event.</param>
       /// <param name="args">A <see cref="CollectionAddingNew">CollectionAddingNew</see> that contains information about the event.</param>
       void CollectionAddingNew(object sender, object args);

       /// <summary>
       /// Occurs when the user changes of the mode of a parameter.
       /// </summary>
       /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
       /// <para>The <c>OnParameterModeChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred
       /// technique for handling the event in a derived class.</para>
       /// <para>Notes to Inheritors: </para>
       /// <para>When overriding <c>OnParameterModeChanged</c> in a derived class, be sure to call the base class's <c>OnParameterModeChanged</c> method so that registered
       /// delegates receive the event.</para>
       /// </remarks>
       /// <param name="sender">The <see cref="RealParameter">RealParameter</see> that raised the event.</param>
       /// <param name="args">A <see cref="CollectionListChanged">CollectionListChanged</see> that contains information about the event.</param>
       void CollectionListChanged(object sender, object args);
   }
*/
/// <summary>
///	Interface that exposes a PMC's parameters, controls the PMC's lifecycle, 
/// provides access to the PME through the simulation context, and provides a 
/// means for the PME to edit the PMC.</summary>
/// <remarks><para>When a PME requires some kind of functionality, with the help of the 
/// CAPE-OPEN categories, the user is able to select and create a CO class which 
/// will expose the required CO interfaces. There is the need for the PME to 
/// exchange some information with this instance of the PMC. This information 
/// consists in a set of simple unrelated functionalities that will be useful for 
/// any kind of CAPE-OPEN component, since they will allow maximum integration 
/// between clients and servers. All these functionalities can be grouped in a 
/// single interface. Some of the functionalities to fulfil consist in exchanging 
/// interface references between the PMC and the PME. Instead of adding these 
/// properties to each business interfaces, it is much more convenient to
/// add them to a single common interface which refers to the whole PMC.</para>
/// <para>Furthermore, there is a need for getting parameters, editing and 
/// lifecycling.</para>
/// <para>The interface should fulfil the following requirements:</para>
/// <para>Parameters:</para>
/// <para>So far, only Unit Operations can expose their public parameters, through 
/// property ICapeUnit.parameters, which returns a collection of parameters. This 
/// property allows COSEs to support design specs between two CAPE-OPEN Unit 
/// Operations. That means that the CAPE-OPEN interfaces are powerful enough to 
/// allow that the design spec of a given Unit Operation (exposed through public 
/// parameters) depends on transformations of public parameters exposed by other 
/// CAPE-OPEN Unit Operations. If also other components, such as Material Object, 
/// would be able to expose public parameters, the functionality the aforementioned 
/// described functionality could be extended. Other functionalities would be:
/// </para>
/// <para>(i) allowing optimizers to use a public variable exposed by any 
/// CAPE-OPEN component.</para>
/// <para>(ii) Allow performing regression on the interaction parameters of a 
/// CAPE-OPEN Property Package.</para>
/// <para>Centralising the property that accesses these collections in a single 
/// entry point, helps to clarify the life cycle usage standards for these 
/// collections. That means that it will be easier for the PMC clients to know 
/// how often they have to check whether the contents of these collections have 
/// changed (although the collection object will be valid until the PMC is 
/// destroyed). Setting general rules for the usage of these collections makes 
/// the business interface specifications more regular and simpler. Obviously, 
/// since too general rules may reduce flexibility, PMC specifications might point
/// out exceptions to the general rule. Let’s see how would this affect the 
/// particular PMC specifications:</para>
/// <para>Simulation context:</para>
/// <para>So far, most of CAPE-OPEN interfaces have been designed to allow a 
/// client to access the functionality of a CAPE-OPEN component. Since clients 
/// will often be Simulation Environment, CAPE-OPEN components would benefit from 
/// using functionality provided by their client, a COSE for instance. These 
/// services provided by any PMEs are defined within the Simulation Context COSE 
/// Interface specification document.</para>
/// <para>The following interfaces are designed:</para>
/// <para>(i) Thermo Material Template Systems: Theses interface allows a PMC to 
/// choose between all the Thermo Material factories supported by the PME. These 
/// factories will allow the PMC to create a thermo material object associated to 
/// the elected Property Package (which can be CAPE-OPEN or not).</para>
/// <para>(ii) Diagnostics: This interface will allow to integrate seamlessly the 
/// diagnostics messages generated by any PMC with the mechanisms supported by 
/// the PME to display this information to the user.</para>
/// <para>(iii) COSEUtilities: In the same idea of this specification document, 
/// PME has also its own utilities interface in order to gather many basic 
/// operations. For instance that allows the PME to supply a list of standardised 
/// values.</para>
/// <para>Edit:</para>
/// <para>The Edit method defined by the UNIT specification proved to be very useful in 
/// order to provide Graphical User Interface (GUI) capabilities highly customized 
/// to each type of UNIT implementation. There’s no reason why other PMCs could 
/// not benefit of this capability. Obviously, when a PMC provides Edit 
/// functionality, being able to persist its state is a desired requirements, to 
/// prevent the user from having to repeatedly reconfigure the PMC.</para>
/// <para>LifeCycle:</para>
/// <para>There is probably no strict necessity to expose directly initialization 
/// nor destruction functions, since these should be invoked automatically by the 
/// used middleware (COM/CORBA). That is, the initialization could be performed 
/// in the constructor of the class and the destroy in its destructor. However, 
/// in some cases the client could need to invoke them explicitly. For example, 
/// all actions that could fail should be invoked by these methods. If these 
/// actions were places in the constructor or destructor, potential failures 
/// would cause memory leak, and they would be difficult to track, since it would 
/// not be clear if the component has been created/destroyed or not. Examples of 
/// cases where they are useful:</para>
/// <para>(i) Initialize: The client might need to initialize a given set of PMC 
/// in a specific order, in case that there are dependencies between them. Some 
/// PMC may be wrappers to other components, or may need an external file to get 
/// initialized. This initialization process may often fail, or the user may even 
/// decide to cancel it. Moving these actions from the class constructor to the 
/// initialize method allows communicating the client that the construction of
/// the component must be aborted in some cases.</para>
/// <para>(ii) Destructors: The PMC primary object should destroy here all its secondary 
/// objects. Relying on the native destructor could cause deadlocks when loop 
/// references exist between PMC objects. See in the example diagram below that 
/// after the client releases its reference to the Unit Operation, both the Unit 
/// and the Parameter are being used by another objects. So, without an explicit 
/// terminate method, none of them would be ever terminated.</para>
/// <para>Reference document: Utilities Common Interface</para></remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeUtilities_IID)]
[Description("ICapeUtilities Interface")]
interface ICapeUtilitiesCOM
{
    /// <summary>
    ///	Gets the component's collection of parameters. </summary>
    /// <remarks><para>Return the collection of Public Unit Parameters (i.e. 
    /// <see cref="ICapeCollection"/>.</para>
    /// <para>These are delivered as a collection of elements exposing the interface 
    /// <see cref="ICapeParameter"/>. From there, the client could extract the 
    /// <see cref="ICapeParameterSpec"/> interface or any of the typed
    /// interfaces such as <see cref="ICapeRealParameterSpec"/>, once the client 
    /// establishes that the Parameter is of type double.</para></remarks>
    /// <value>The parameter collection of the unit operation.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [DispId(1), Description("Gets parameter collection")]
    //[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)]
    object parameters
    {
        [return: MarshalAs(UnmanagedType.IDispatch)]
        get;
    }

    /// <summary>
    ///	Sets the component's simulation context.</summary>
    /// <remarks><para>Allows the PME to convey the PMC a reference to the former’s 
    /// simulation  context. The simulation context will be PME objects which will 
    /// expose a given set of CO interfaces. Each of these interfaces will allow 
    /// the PMC to call back the PME in order to benefit from its exposed services 
    /// (such as creation of material templates, diagnostics or measurement unit 
    /// conversion). If the PMC does not support accessing the simulation context, 
    /// it is recommended to raise the ECapeNoImpl error.</para>
    /// <para>Initially, this method was only present in the ICapeUnit interface. 
    /// Since ICapeUtilities.SetSimulationContext is now available for any kind of 
    /// PMC, ICapeUnit.SetSimulationContext is deprecated.</para></remarks>
    /// <value>
    /// The reference to the PME’s simulation context class. For the PMC to use 
    /// this class, this reference will have to be converted to each of the 
    /// defined CO Simulation Context interfaces.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [DispId(2), Description("Set the simulation context")]
    object simulationContext
    {
        [param: MarshalAs(UnmanagedType.IDispatch)]
        set;
    }

    /// <summary>
    ///	The component is asked to configure itself. For example a Unit Operation might create ports and parameters during this call</summary>
    /// <remarks><para>Initially, this method was only present in the ICapeUnit interface. 
    /// Since ICapeUtilities.Initialize is now available for any kind of PMC, 
    /// ICapeUnit. Initialize is deprecated.</para>
    /// <para>The PME will order the PMC to get initialized through this method. 
    /// Any initialisation that could fail must be placed here. Initialize is 
    /// guaranteed to be the first method called by the client (except low level 
    /// methods such as class constructors or initialization persistence methods).
    /// Initialize has to be called once when the PMC is instantiated in a 
    /// particular flowsheet.</para>
    /// <para>When the initialization fails, before signalling an error, the PMC 
    /// must free all the resources that were allocated before the failure 
    /// occurred. When the PME receives this error, it may not use the PMC 
    /// anymore.</para>
    /// <para>The method terminate of the current interface must not either be 
    /// called. Hence, the PME may only release the PMC through the middleware 
    /// native mechanisms.</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [DispId(3), Description("Configuration has to take place here")]
    void Initialize();

    /// <summary>
    ///	Clean-up tasks can be performed here. References to parameters and ports are released here.</summary>
    /// <remarks><para>Initially, this method was only present in the ICapeUnit interface. 
    /// Since ICapeUtilities.Terminate is now available for any kind of PMC, 
    /// ICapeUnit.Terminate is deprecated.</para>
    /// <para>The PME will order the PMC to get destroyed through this method. 
    /// Any uninitialization that could fail must be placed here. ‘Terminate’ is 
    /// guaranteed to be the last method called by the client (except low level 
    /// methods such as class destructors). ‘Terminate’ may be called at any time, 
    /// but may be only called once.</para>
    /// <para>When this method returns an error, the PME should report the user. 
    /// However, after that the PME is not allowed to use the PMC anymore.</para>
    /// <para>The Unit specification stated that “Terminate may check if the data 
    /// has been saved and return an error if not.” It is suggested not to follow 
    /// this recommendation, since it’s the PME responsibility to save the state 
    /// of the PMC before terminating it. In the case that a user wants to close 
    /// a simulation case without saving it, it’s better to leave the PME to 
    /// handle the situation instead of each PMC providing a different 
    /// implementation.</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [DispId(4), Description("Clean up has to take place here")]
    void Terminate();

    /// <summary>
    ///	Displays the PMC graphic interface, if available.</summary>
    /// <remarks>The PMC displays its user interface and allows the Flowsheet User to 
    /// interact with it. If no user interface is available it returns an error.</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [DispId(5), Description("Displays the graphic interface")]
    [PreserveSig]
    int Edit();
}


/// <summary>
///	Interface that exposes a PMC's parameters, controls the PMC's lifecycle, 
/// provides access to the PME through the simulation context, and provides a 
/// means for the PME to edit the PMC.</summary>
/// <remarks><para>When a PME requires some kind of functionality, with the help of the 
/// CAPE-OPEN categories, the user is able to select and create a CO class which 
/// will expose the required CO interfaces. There is the need for the PME to 
/// exchange some information with this instance of the PMC. This information 
/// consists in a set of simple unrelated functionalities that will be useful for 
/// any kind of CAPE-OPEN component, since they will allow maximum integration 
/// between clients and servers. All these functionalities can be grouped in a 
/// single interface. Some of the functionalities to fulfil consist in exchanging 
/// interface references between the PMC and the PME. Instead of adding these 
/// properties to each business interfaces, it is much more convenient to
/// add them to a single common interface which refers to the whole PMC.</para>
/// <para>Furthermore, there is a need for getting parameters, editing and 
/// lifecycling.</para>
/// <para>The interface should fulfil the following requirements:</para>
/// <para>Parameters:</para>
/// <para>So far, only Unit Operations can expose their public parameters, through 
/// property ICapeUnit.parameters, which returns a collection of parameters. This 
/// property allows COSEs to support design specs between two CAPE-OPEN Unit 
/// Operations. That means that the CAPE-OPEN interfaces are powerful enough to 
/// allow that the design spec of a given Unit Operation (exposed through public 
/// parameters) depends on transformations of public parameters exposed by other 
/// CAPE-OPEN Unit Operations. If also other components, such as Material Object, 
/// would be able to expose public parameters, the functionality the aforementioned 
/// described functionality could be extended. Other functionalities would be:
/// </para>
/// <para>(i) allowing optimizers to use a public variable exposed by any 
/// CAPE-OPEN component.</para>
/// <para>(ii) Allow performing regression on the interaction parameters of a 
/// CAPE-OPEN Property Package.</para>
/// <para>Centralising the property that accesses these collections in a single 
/// entry point, helps to clarify the life cycle usage standards for these 
/// collections. That means that it will be easier for the PMC clients to know 
/// how often they have to check whether the contents of these collections have 
/// changed (although the collection object will be valid until the PMC is 
/// destroyed). Setting general rules for the usage of these collections makes 
/// the business interface specifications more regular and simpler. Obviously, 
/// since too general rules may reduce flexibility, PMC specifications might point
/// out exceptions to the general rule. Let’s see how would this affect the 
/// particular PMC specifications:</para>
/// <para>Simulation context:</para>
/// <para>So far, most of CAPE-OPEN interfaces have been designed to allow a 
/// client to access the functionality of a CAPE-OPEN component. Since clients 
/// will often be Simulation Environment, CAPE-OPEN components would benefit from 
/// using functionality provided by their client, a COSE for instance. These 
/// services provided by any PMEs are defined within the Simulation Context COSE 
/// Interface specification document.</para>
/// <para>The following interfaces are designed:</para>
/// <para>(i) Thermo Material Template Systems: Theses interface allows a PMC to 
/// choose between all the Thermo Material factories supported by the PME. These 
/// factories will allow the PMC to create a thermo material object associated to 
/// the elected Property Package (which can be CAPE-OPEN or not).</para>
/// <para>(ii) Diagnostics: This interface will allow to integrate seamlessly the 
/// diagnostics messages generated by any PMC with the mechanisms supported by 
/// the PME to display this information to the user.</para>
/// <para>(iii) COSEUtilities: In the same idea of this specification document, 
/// PME has also its own utilities interface in order to gather many basic 
/// operations. For instance that allows the PME to supply a list of standardised 
/// values.</para>
/// <para>Edit:</para>
/// <para>The Edit method defined by the UNIT specification proved to be very useful in 
/// order to provide Graphical User Interface (GUI) capabilities highly customized 
/// to each type of UNIT implementation. There’s no reason why other PMCs could 
/// not benefit of this capability. Obviously, when a PMC provides Edit 
/// functionality, being able to persist its state is a desired requirements, to 
/// prevent the user from having to repeatedly reconfigure the PMC.</para>
/// <para>LifeCycle:</para>
/// <para>There is probably no strict necessity to expose directly initialization 
/// nor destruction functions, since these should be invoked automatically by the 
/// used middleware (COM/CORBA). That is, the initialization could be performed 
/// in the constructor of the class and the destroy in its destructor. However, 
/// in some cases the client could need to invoke them explicitly. For example, 
/// all actions that could fail should be invoked by these methods. If these 
/// actions were places in the constructor or destructor, potential failures 
/// would cause memory leak, and they would be difficult to track, since it would 
/// not be clear if the component has been created/destroyed or not. Examples of 
/// cases where they are useful:</para>
/// <para>(i) Initialize: The client might need to initialize a given set of PMC 
/// in a specific order, in case that there are dependencies between them. Some 
/// PMC may be wrappers to other components, or may need an external file to get 
/// initialized. This initialization process may often fail, or the user may even 
/// decide to cancel it. Moving these actions from the class constructor to the 
/// initialize method allows communicating the client that the construction of
/// the component must be aborted in some cases.</para>
/// <para>(ii) Destructors: The PMC primary object should destroy here all its secondary 
/// objects. Relying on the native destructor could cause deadlocks when loop 
/// references exist between PMC objects. See in the example diagram below that 
/// after the client releases its reference to the Unit Operation, both the Unit 
/// and the Parameter are being used by another objects. So, without an explicit 
/// terminate method, none of them would be ever terminated.</para>
/// <para>Reference document: Utilities Common Interface</para></remarks>
[ComVisible(false)]
[Description("ICapeUtilities Interface")]
public interface ICapeUtilities
{
    /// <summary>
    ///	Gets the component's collection of parameters. </summary>
    /// <remarks><para>Return the collection of Public Unit Parameters (i.e. 
    /// <see cref="ICapeCollection"/>.</para>
    /// <para>These are delivered as a collection of elements exposing the interface 
    /// <see cref="ICapeParameter"/>. From there, the client could extract the 
    /// <see cref="ICapeParameterSpec"/> interface or any of the typed
    /// interfaces such as <see cref="ICapeRealParameterSpec"/>, once the client 
    /// establishes that the Parameter is of type double.</para></remarks>
    /// <value>The parameter collection of the unit operation.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [DispId(1), Description("Gets parameter collection")]
    ParameterCollection Parameters
    {
        get;
    }

    /// <summary>
    ///	Sets the component's simulation context.</summary>
    /// <remarks><para>Allows the PME to convey the PMC a reference to the former’s 
    /// simulation  context. The simulation context will be PME objects which will 
    /// expose a given set of CO interfaces. Each of these interfaces will allow 
    /// the PMC to call back the PME in order to benefit from its exposed services 
    /// (such as creation of material templates, diagnostics or measurement unit 
    /// conversion). If the PMC does not support accessing the simulation context, 
    /// it is recommended to raise the ECapeNoImpl error.</para>
    /// <para>Initially, this method was only present in the ICapeUnit interface. 
    /// Since ICapeUtilities.SetSimulationContext is now available for any kind of 
    /// PMC, ICapeUnit.SetSimulationContext is deprecated.</para></remarks>
    /// <value>
    /// The reference to the PME’s simulation context class. For the PMC to use 
    /// this class, this reference will have to be converted to each of the 
    /// defined CO Simulation Context interfaces.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [DispId(2), Description("Set the simulation context")]
    ICapeSimulationContext SimulationContext
    {
        get;
        set;
    }

    /// <summary>
    ///	The component is asked to configure itself. For example a Unit Operation might create ports and parameters during this call</summary>
    /// <remarks><para>Initially, this method was only present in the ICapeUnit interface. 
    /// Since ICapeUtilities.Initialize is now available for any kind of PMC, 
    /// ICapeUnit. Initialize is deprecated.</para>
    /// <para>The PME will order the PMC to get initialized through this method. 
    /// Any initialisation that could fail must be placed here. Initialize is 
    /// guaranteed to be the first method called by the client (except low level 
    /// methods such as class constructors or initialization persistence methods).
    /// Initialize has to be called once when the PMC is instantiated in a 
    /// particular flowsheet.</para>
    /// <para>When the initialization fails, before signalling an error, the PMC 
    /// must free all the resources that were allocated before the failure 
    /// occurred. When the PME receives this error, it may not use the PMC 
    /// anymore.</para>
    /// <para>The method terminate of the current interface must not either be 
    /// called. Hence, the PME may only release the PMC through the middleware 
    /// native mechanisms.</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [DispId(3), Description("Configuration has to take place here")]
    void Initialize();

    /// <summary>
    ///	Clean-up tasks can be performed here. References to parameters and ports are released here.</summary>
    /// <remarks><para>Initially, this method was only present in the ICapeUnit interface. 
    /// Since ICapeUtilities.Terminate is now available for any kind of PMC, 
    /// ICapeUnit.Terminate is deprecated.</para>
    /// <para>The PME will order the PMC to get destroyed through this method. 
    /// Any uninitialization that could fail must be placed here. ‘Terminate’ is 
    /// guaranteed to be the last method called by the client (except low level 
    /// methods such as class destructors). ‘Terminate’ may be called at any time, 
    /// but may be only called once.</para>
    /// <para>When this method returns an error, the PME should report the user. 
    /// However, after that the PME is not allowed to use the PMC anymore.</para>
    /// <para>The Unit specification stated that “Terminate may check if the data 
    /// has been saved and return an error if not.” It is suggested not to follow 
    /// this recommendation, since it’s the PME responsibility to save the state 
    /// of the PMC before terminating it. In the case that a user wants to close 
    /// a simulation case without saving it, it’s better to leave the PME to 
    /// handle the situation instead of each PMC providing a different 
    /// implementation.</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [DispId(4), Description("Clean up has to take place here")]
    void Terminate();

    /// <summary>
    ///	Displays the PMC graphic interface, if available.</summary>
    /// <remarks>The PMC displays its user interface and allows the Flowsheet User to 
    /// interact with it. If no user interface is available it returns an error.</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [DispId(5), Description("Displays the graphic interface")]
    DialogResult Edit();
}

/// <summary>Represents the method that will handle the changing of the simualtion context of a PMC.</summary>
[ComVisible(false)]
public delegate void SimulationContextChangedHandler(object sender, EventArgs args);