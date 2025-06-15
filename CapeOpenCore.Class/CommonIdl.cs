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

    /// <summary>创建一个 CapeIdentification 类的实例，该实例包含 PMC 的名称和默认描述。</summary>
    /// <remarks>这个构造函数使用提供的名称来构建 PMC 对象的 ComponentName 属性。随后会为 ComponentDescription 属性分配默认值。
    /// 如果 PMC 对象具有 <see cref="CapeDescriptionAttribute"/>，
    /// 那么将使用 <see cref="CapeDescriptionAttribute.Description"/> 属性来提供描述。</remarks>
    /// <param name="name">PMC 的名称。</param>
    protected CapeIdentification(string name)
    {
        _disposed = false;
        _mComponentName = name;
        _mComponentDescription = GetType().FullName;
        var attributes = GetType().GetCustomAttributes(false);
        foreach (var pT in attributes)
        {
            if (pT is CapeDescriptionAttribute pDescriptionAttribute) _mComponentDescription = pDescriptionAttribute.Description;
        }
    }

    /// <summary>创建一个 CapeIdentification 类的实例，该实例包含 PMC 的名称和默认描述。</summary>
    /// <remarks>您可以使用此构造函数来指定 PMC 的具体名称和描述。</remarks>
    /// <param name="name">PMC 的名称。</param>
    /// <param name="description">PMC 的描述。</param>
    protected CapeIdentification(string name, string description)
    {
        _disposed = false;
        _mComponentName = name;
        _mComponentDescription = description;
    }
    
    /// <summary>CapeIdentification 类的复制构造函数。</summary>
    /// <remarks>创建一个 CapeIdentification 类的实例，其 ComponentName 等于原始 PMC 的 ComponentName + (Copy)。
    /// 该副本与原始对象具有相同的 CapeDescription。</remarks>
    /// <param name="objectToBeCopied">正在复制的对象。</param>
    protected CapeIdentification(CapeIdentification objectToBeCopied)
    {
        _disposed = false;
        _mComponentName = objectToBeCopied.ComponentName + "(Copy)";
        _mComponentDescription = objectToBeCopied.ComponentDescription;
    }
        
    /// <summary>创建一个与当前实例相同的副本对象。</summary>
    /// <remarks><para>克隆可以实现为深度复制或浅度复制。在深度复制中，所有对象都会被复制；在浅度复制中，仅复制顶级对象，而较低级别的对象仅包含引用。</para>
    /// <para>生成的克隆必须与原始实例属于同一类型或与之兼容。</para>
    /// <para>请参阅 <see cref="Object.MemberwiseClone"/>，以获取有关克隆、深度复制与浅层复制以及示例的详细信息。</para></remarks>
    /// <returns>一个与该实例相同的副本对象。</returns>
    public abstract object Clone();

    // Implement IDisposable.
    // 不要将此方法设为虚函数。派生类不应能够重写此方法。
    /// <summary>释放 CapeIdentification 对象使用的所有资源。</summary>
    /// <remarks>在使用完 CapeIdentification 对象后，请调用 Dispose 方法。Dispose 方法会将 CapeIdentification 对象置于不可用状态。
    /// 调用 Dispose 方法后，您必须释放对 Component 的所有引用，以便垃圾回收器能够回收 CapeIdentification 对象占用的内存。
    /// 如需更多信息，请参阅 <see href="https://msdn.microsoft.com">清理未管理的资源并实现 Dispose 方法。</see></remarks> 
    public void Dispose()
    {
        Dispose(true);
        // 该对象将由 Dispose 方法进行清理。因此，您应调用 GC.SuppressFinalize 方法，
        // 将该对象从最终化队列中移除，并防止该对象的最终化代码再次执行。
        GC.SuppressFinalize(this);
    }

    // Dispose(bool disposing) 在两种不同的情况下执行。如果 disposing 等于 true，则该方法已由用户代码直接或间接调用。可以处理托管和非托管资源。
    // 如果 dispose 方法的返回值为 false，则表示该方法是由运行时从终结器内部调用的，此时不应引用其他对象。仅可释放未托管资源。
    /// <summary>释放 CapeIdentification 对象使用的未托管资源，并可选地释放托管资源。</summary>
    /// <remarks><para>这种方法由公共方法 <see href="https://msdn.microsoft.com">Dispose</see>
    /// 和 <see href="https://msdn.microsoft.com">Finalize</see> 调用。Dispose() 方法会调用受保护的 Dispose(Boolean) 方法，
    /// 同时将 disposing 参数设置为 true。Finalize 方法会调用 Dispose 方法，同时将 disposing 参数设置为 false。</para>
    /// <para>当处置参数为 true 时，此方法将释放由该组件引用的任何托管对象所占用的所有资源。此方法将调用每个引用对象的 Dispose() 方法。</para>
    /// <para>继承此方法的开发者须知：</para>
    /// <para>Dispose 方法可以被其他对象多次调用。在重写 Dispose(Boolean) 方法时，请注意不要引用在之前调用 Dispose 方法时
    /// 已经被释放的对象。有关如何实现 Dispose(Boolean) 方法的更多信息，
    /// 请参阅 <see href="https://msdn.microsoft.com">Implementing a Dispose Method</see>。</para>
    /// <para>有关 Dispose 和 Finalize 的更多信息请参阅 <see href="https://msdn.microsoft.com">Cleaning Up Unmanaged Resources</see> 
    /// 和 <see href="https://msdn.microsoft.com">Overriding the Finalize Method</see>。</para></remarks> 
    /// <param name="disposing">true 表示释放受管和不受管资源；false 表示仅释放不受管资源。</param>
    protected virtual void Dispose(bool disposing)
    {
        // 检查是否已经调用了 Dispose 方法。
        if (_disposed) return;
        // 如果处置为真，则释放所有受管和非受管资源。
        if (disposing)
        {
            // 释放已管理的资源。
            // component.Dispose();
        }
        // 已完成处理。
        _disposed = true;
    }

    /// <summary>通知集合，参数的某个属性的值已发生更改。</summary>
    /// <remarks>PropertyChanged 事件可以通过在 PropertyChangedEventArgs 中将属性名称设置为 null 或 String.Empty 来
    /// 指示对象上的所有属性均已发生更改。</remarks>
    public event PropertyChangedEventHandler PropertyChanged;

    // 此方法由每个属性的 Set 访问器调用。
    // 应用于可选的 propertyName 参数的 CallerMemberName 属性会将调用者的属性名称替换为参数值。
    /// <summary>通知集合，参数的某个属性的值已发生更改。</summary>
    /// <remarks>PropertyChanged 事件可以通过在 PropertyChangedEventArgs 中将属性名称设置为 null 或 String.Empty 来
    /// 指示对象上的所有属性均已发生更改。</remarks>
    /// <param name="propertyName">被更改的属性名称。</param>
    protected void NotifyPropertyChanged(string propertyName) // .Net 4.5 [System.Runtime.CompilerServices.CallerMemberName]
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>当用户更改组件的名称时发生。</summary>
    /// <remarks>当 PMC 名称发生变更时触发的事件。</remarks> 
    public event ComponentNameChangedHandler ComponentNameChanged;

    /// <summary>当用户修改组件的描述时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnComponentNameChanged</c> 方法还允许派生类在不附加委托的情况下处理该事件。这是在派生类中处理该事件的首选方法。</para>
    /// <para>继承此方法的开发者须知：</para>
    /// <para>当在派生类中重写 <c>OnComponentNameChanged</c> 时，务必调用基类的 <c>OnComponentNameChanged </c> 方法，
    /// 以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="args">一个包含事件信息的 <see cref="ComponentNameChangedEventArgs">NameChangedEventArgs</see>。</param>
    protected void OnComponentNameChanged(ComponentNameChangedEventArgs args)
    {
        ComponentNameChanged?.Invoke(this, args);
    }

    /// <summary>当用户修改组件的描述时发生。</summary>
    /// <remarks>当 PMC 的描述发生更改时触发的事件。</remarks> 
    public event ComponentDescriptionChangedHandler ComponentDescriptionChanged;

    /// <summary>当用户修改组件的描述时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnComponentDescriptionChanged</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>继承此方法的开发者须知：</para>
    /// <para>当在派生类中重写 <c>OnComponentDescriptionChanged</c> 时，
    /// 务必调用基类的 <c>OnComponentDescriptionChanged </c> 方法，以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="args">一个包含事件信息的 <see cref="ComponentDescriptionChangedEventArgs">DescriptionChangedEventArgs</see>。</param>
    protected void OnComponentDescriptionChanged(ComponentDescriptionChangedEventArgs args)
    {
        ComponentDescriptionChanged?.Invoke(this, args);
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
        get => _mComponentName;
        set
        {
            var args = new ComponentNameChangedEventArgs(_mComponentName, value);
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
        get => _mComponentDescription;
        set
        {
            var args = new ComponentDescriptionChangedEventArgs(_mComponentDescription, value);
            _mComponentDescription = value;
            NotifyPropertyChanged("ComponentDescription");
            OnComponentDescriptionChanged(args);
        }
    }
}

/// <summary>此介面提供唯讀集合的行為。可用於儲存端口或參數。</summary>
/// <remarks><para>集合接口的目的是为 CAPE-OPEN 组件提供向其任何客户端暴露对象列表的能力。客户端无法修改集合，
/// 即无法删除、替换或添加元素。然而，由于客户端可以访问集合中各元素暴露的任何 CAPE-OPEN 接口，因此能够修改任何元素的状态。</para>
/// <para>CAPE-OPEN 集合不允许暴露基本类型，如数值或字符串。事实上，在此情况下使用 CapeArrays 更为方便。</para>
/// <para>集合中的所有项不必都属于同一类。只要它们实现相同的接口或接口集即可。根据 CAPE-OPEN 规范，暴露集合接口的组件
/// 必须明确说明集合中的所有项必须实现哪些接口。</para>
/// <para>参阅文档：Collection Common Interface。</para></remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ICapeCollection_IID)]
[Description("ICapeCollection Interface")]
internal interface ICapeCollection
{
    /// <summary>获取集合中存储的特定项，该项通过其 ICapeIdentification.ComponentName 或作为方法参数传递的基于 1 的索引进行标识。</summary>
    /// <remarks>从集合中返回一个元素。请求的元素可以通过其实际名称（例如类型为 CapeString）或在集合中的位置（例如类型为 CapeLong）来识别。
    /// 元素的名称是其 ICapeIdentification 接口的 ComponentName() 方法返回的值。与通过位置检索项相比，通过名称检索项的优势在于效率更高。
    /// 这是因为从服务器端检查所有名称比从客户端检查更快，因为客户端需要进行大量 COM/CORBA 调用。</remarks>
    /// <param name="index">
    /// <para>请求项的标识符：</para>
    /// <para>项目名称（它包含一个 string）</para>
    /// <para>在集合中的位置（它包含一个 long）</para>
    /// </param>
    /// <returns>包含请求的集合项的 System.Object 对象。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeOutOfBounds">ECapeOutOfBounds</exception>
    [DispId(1)]
    [Description("Gets an item specified by index or name")]
    [return: MarshalAs(UnmanagedType.IDispatch)] // 此属性指定返回值是一个 IDispatch 指针。
    object Item(object index);

    /// <summary>获取集合中当前存储的项目数量。</summary>
    /// <remarks>返回集合中的项目数量。</remarks>
    /// <returns>返回集合中的项目数量。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    [DispId(2)]
    [Description("Number of items in the collection")]
    int Count();
}

/// <summary>接口用于暴露 PMC 的参数，控制 PMC 的生命周期，通过仿真上下文访问 PME，并为 PME 提供编辑 PMC 的手段。</summary>
/// <remarks><para>当一个 PME 需要某种功能时，借助 CAPE-OPEN 类别，用户可以选择并创建一个 CO 类，该类将暴露所需的 CO 接口。
/// PME 需要与该 PMC 实例交换一些信息。这些信息由一组简单的无关功能组成，这些功能对任何类型的 CAPE-OPEN 组件都非常有用，因为它们
/// 将实现客户端与服务器之间的最大集成。所有这些功能都可以归类到一个单一接口中。其中一些需要实现的功能包括在 PMC 和 PME 之间交换
/// 接口引用。与其将这些属性添加到每个业务接口中，不如将它们添加到一个通用的接口中，该接口引用整个 PMC。</para>
/// <para>此外，还需要获取参数、编辑和生命周期管理。</para>
/// <para>该界面应满足以下要求：</para><para>参数：</para>
/// <para>截至目前，只有“单元操作”能够通过属性 ICapeUnit.parameters 来展示其公共参数，该属性返回一个参数集合。
/// 这个特性使得 COSEs 能够在两个 CAPE-OPEN “单元操作”之间支持设计规范。这意味着，CAPE-OPEN 接口具有足够的强大功能，
/// 使得一个特定“单元操作”的设计规范（通过公共参数展示）能够依赖于其他 CAPE-OPEN “单元操作“所展示的公共参数的转换结果。
/// 如果还能让其他组件，如“物流对象”，也能够展示公共参数，那么上述功能将得到扩展，从而具备以下其他功能：</para>
/// <para>1. 允许优化器使用任何 CAPE-OPEN 组件暴露的公共变量。</para>
/// <para>2. 允许对 CAPE-OPEN 属性包的交互参数进行回归分析。</para>
/// <para>将访问这些集合的属性集中到一个入口点，有助于明确这些集合的生命周期使用标准。这意味着 PMC 客户端将更容易知道需要多频繁地
/// 检查这些集合的内容是否发生变化（尽管集合对象在 PMC 被销毁前始终有效）。为这些集合的使用设定通用规则，能使业务接口规范更加规范和简洁。
/// 显然，过于通用的规则可能降低灵活性，因此 PMC 规范可能会指出对通用规则的例外情况。让我们看看这将如何影响特定的 PMC 规范：</para>
/// <para>模拟环境：</para>
/// <para>到目前为止，大多数 CAPE-OPEN 接口都是为了允许客户端访问 CAPE-OPEN 组件的功能而设计的。由于客户端通常是仿真环境，
/// CAPE-OPEN 组件可以从客户端提供的功能中受益，例如 COSE。这些由任何 PME 提供的服务在仿真上下文 COSE 接口规范文档中进行了定义。</para>
/// <para>以下接口已设计完成：</para>
/// <para>1. Thermo Material Template Systems: 该接口允许 PMC 在 PME 支持的所有热力学物流对象工厂中进行选择。
/// 这些工厂将使 PMC 能够创建与所选属性包（可以是 CAPE-OPEN 格式，也可以不是）关联的热力学物流对象。</para>
/// <para>2. Diagnostics: 该接口将实现任何 PMC 生成的诊断消息与 PME 支持的机制的无缝集成，以便将这些信息显示给用户。</para>
/// <para>3. COSEUtilities: 与本规范文档的思路一致，PME 还拥有自己的实用程序接口，用于实现多种基本操作。
/// 例如，这使得 PME 能够提供一组标准化的值列表。</para>
/// <para>编辑界面：</para>
/// <para>由 UNIT 规范定义的 Edit 方法在为每种 UNIT 实现提供高度定制的图形用户界面（GUI）功能方面证明非常有用。
/// 其他 PMC 也完全可以利用这一功能。显然，当 PMC 提供 Edit 功能时，能够保存其状态是必要的要求，以避免用户需要反复重新配置 PMC。</para>
/// <para>生命周期：</para>
/// <para>可能没有必要直接暴露初始化或销毁函数，因为这些函数应由使用的中间件（COM/CORBA）自动调用。也就是说，初始化可以在类的构造函数中完成，
/// 而销毁则在析构函数中完成。然而，在某些情况下，客户端可能需要显式调用这些函数。例如，所有可能失败的操作都应通过这些方法调用。
/// 如果这些操作被放置在构造函数或析构函数中，潜在的失败会导致内存泄漏，且难以追踪，因为无法确定组件是否已被创建/销毁。以下是这些方法有用的一些示例：</para>
/// <para>1. Initialize: 客户端可能需要以特定顺序初始化一组 PMC，以处理它们之间的依赖关系。部分 PMC 可能是其他组件的封装，或需要通过
/// 外部文件进行初始化。此初始化过程可能频繁失败，或用户甚至可能选择取消该过程。将这些操作从类构造函数移至 Initialize 方法，
/// 可向客户端明确传达：在某些情况下必须中止组件的构造过程。</para>
/// <para>2. Destructors: PMC 的主要对象应在此处销毁所有次要对象。依赖于原生析构函数可能会在 PMC 对象之间存在循环引用时导致死锁。
/// 如下方示例图所示，当客户端释放对单元操作的引用后，单元和参数仍被其他对象使用。因此，若没有显式的终止方法，它们将永远不会被终止。</para>
/// <para>请参阅接口文档：Utilities Common Interface。</para></remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ICapeUtilities_IID)]
[Description("ICapeUtilities Interface")]
internal interface ICapeUtilitiesCOM
{
    /// <summary>获取组件的参数集合。</summary>
    /// <remarks><para>返回公共单元参数集合（即 <see cref="ICapeCollection"/>）。</para>
    /// <para>这些以一组元素的形式提供，这些元素暴露了接口 <see cref="ICapeParameter"/>。从那里开始，客户端可以提取
    /// <see cref="ICapeParameterSpec"/> 接口或任何类型的接口，例如 <see cref="ICapeRealParameterSpec"/>，
    /// 一旦客户端确定该参数的类型为 double。</para></remarks>
    /// <value>单元操作的参数集合。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [DispId(1), Description("Gets parameter collection")]
    //[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)]
    object parameters
    {
        [return: MarshalAs(UnmanagedType.IDispatch)] get;
    }

    /// <summary>设置组件的模拟上下文。</summary>
    /// <remarks><para>允许 PME 向 PMC 传递对其模拟上下文的引用。该模拟上下文将由 PME 对象组成，这些对象将暴露一组特定的 CO 接口。
    /// 每个接口均可使 PMC 调用 PME 以利用其暴露的服务（例如物流模板创建、诊断或测量单位转换）。
    /// 若 PMC 不支持访问模拟上下文，建议触发 ECapeNoImpl 错误。</para>
    /// <para>最初，此方法仅存在于 ICapeUnit 接口中。由于 ICapeUtilities.SetSimulationContext 现已适用于任何类型的 PMC，
    /// 因此 ICapeUnit.SetSimulationContext 已被废弃。</para></remarks>
    /// <value>对 PME 模拟上下文类的引用。为了使 PMC 能够使用该类，此引用必须转换为每个已定义的 CO 模拟上下文接口。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [DispId(2), Description("Set the simulation context")]
    object simulationContext
    {
        [param: MarshalAs(UnmanagedType.IDispatch)] set;
    }

    /// <summary>该组件被要求进行自我配置。例如，一个单元操作可能在此次调用过程中创建端口和参数。</summary>
    /// <remarks><para>最初，此方法仅存在于 ICapeUnit 接口中。由于 ICapeUtilities.Initialize 现已
    /// 适用于任何类型的 PMC，ICapeUnit.Initialize 已被废弃。</para>
    /// <para>PME 将通过此方法命令 PMC 进行初始化。任何可能失败的初始化操作都必须在此处进行。Initialize 方法保证是客户端调用的
    /// 第一个方法（不包括类构造函数或初始化持久化方法等低级方法）。Initialize 方法必须在 PMC 在特定流程图中实例化时被调用一次。</para>
    /// <para>当初始化失败时，在触发错误之前，PMC 必须释放所有在故障发生前分配的资源。当 PME 接收到此错误时，它可能不再使用 PMC。</para>
    /// <para>当前接口的终止方法不得被调用。因此，PME只能通过中间件的原生机制释放PMC。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [DispId(3), Description("Configuration has to take place here")]
    void Initialize();

    /// <summary>清理任务可在此执行。参数和端口的引用在此释放。</summary>
    /// <remarks><para>最初，此方法仅存在于 ICapeUnit 接口中。由于 ICapeUtilities.Terminate 现
    /// 已适用于任何类型的 PMC，ICapeUnit.Terminate 已被废弃。</para>
    /// <para>PME 将通过此方法命令 PMC 进行销毁。任何可能失败的初始化操作都必须在此处进行。Terminate 方法保证是客户端调用的
    /// 最后一个方法（不包括类析构函数等低级方法）。Terminate 方法可能在任何时候被调用，但只能被调用一次。</para>
    /// <para>当此方法返回错误时，PME 应报告用户。然而，之后 PME 不再被允许使用 PMC。</para>
    /// <para>单元规范中规定：“终止操作可检查数据是否已保存，若未保存则返回错误”。建议不要遵循此建议，因为保存 PMC 状态的责任在于 PME，
    /// 而非每个 PMC。若用户希望在未保存的情况下关闭模拟案例，应由 PME 统一处理此情况，而非让每个 PMC 提供不同的实现方式。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [DispId(4), Description("Clean up has to take place here")]
    void Terminate();

    /// <summary>如果可用，显示 PMC 图形界面。</summary>
    /// <remarks>PMC 显示其用户界面，并允许流程图用户与之交互。如果没有用户界面可用，它将返回错误。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [DispId(5), Description("Displays the graphic interface")]
    [PreserveSig]
    int Edit();
}

/// <summary>接口用于暴露 PMC 的参数，控制 PMC 的生命周期，通过仿真上下文访问 PME，并为 PME 提供编辑 PMC 的手段。</summary>
/// <remarks><para>当一个 PME 需要某种功能时，借助 CAPE-OPEN 类别，用户可以选择并创建一个 CO 类，该类将暴露所需的 CO 接口。
/// PME 需要与该 PMC 实例交换一些信息。这些信息由一组简单的无关功能组成，这些功能对任何类型的 CAPE-OPEN 组件都非常有用，
/// 因为它们将实现客户端与服务器之间的最大集成。所有这些功能都可以归类到一个单一接口中。其中一些需要实现的功能包括在 PMC 和 PME 之间
/// 交换接口引用。与其将这些属性添加到每个业务接口中，不如将它们添加到一个通用的接口中，该接口引用整个 PMC。</para>
/// <para>此外，还需要获取参数、编辑和生命周期管理。</para>
/// <para>该界面应满足以下要求：</para>
/// <para>参数：</para>
/// <para>到目前为止，只有单元操作可以暴露其公共参数，通过属性 ICapeUnit.parameters，该属性返回一个参数集合。该属性允许 COSEs 在
/// 两个 CAPE-OPEN 单元操作之间支持设计规范。这意味着 CAPE-OPEN 接口足够强大，可以让某个单元操作的设计规范（通过公共参数暴露）依赖
/// 于其他 CAPE-OPEN 单元操作暴露的公共参数的转换。如果其他组件（如物料对象）也能够暴露公共参数，则上述描述的功能可以得到扩展。其他功能包括：</para>
/// <para>1. 允许优化器使用任何 CAPE-OPEN 组件暴露的公共变量。</para>
/// <para>2. 允许对 CAPE-OPEN 属性包的交互参数进行回归分析。</para>
/// <para>将访问这些集合的属性集中到一个入口点，有助于明确这些集合的生命周期使用标准。这意味着 PMC 客户端将更容易知道需要多频繁地检查这些
/// 集合的内容是否发生变化（尽管集合对象在 PMC 被销毁前始终有效）。为这些集合的使用设定通用规则，能使业务接口规范更加规范和简洁。
/// 显然，过于通用的规则可能降低灵活性，因此 PMC 规范可能会指出对通用规则的例外情况。让我们看看这将如何影响特定的 PMC 规范：</para>
/// <para>模拟上下文（环境）：</para>
/// <para>到目前为止，大多数 CAPE-OPEN 接口都是为了允许客户端访问 CAPE-OPEN 组件的功能而设计的。由于客户端通常是仿真环境，
/// CAPE-OPEN 组件可以从其客户端提供的功能中受益，例如 COSE。这些由任何 PME 提供的服务在仿真上下文 COSE 接口规范文档中进行了定义。</para>
/// <para>以下接口已设计完成：</para>
/// <para>1. Thermo Material Template Systems: 该接口允许 PMC 在 PME 支持的所有热力学物流对象工厂中进行选择。
/// 这些工厂将使 PMC 能够创建与所选属性包（可以是 CAPE-OPEN 格式，也可以不是）关联的热力学物流对象。</para>
/// <para>2. Diagnostics: 该接口将实现任何 PMC 生成的诊断消息与 PME 支持的机制的无缝集成，以便将这些信息显示给用户。</para>
/// <para>3. COSEUtilities: 与本规范文档的思路一致，PME 还拥有自己的实用程序接口，用于实现多种基本操作。
/// 例如，这使得 PME 能够提供一组标准化的值列表。</para>
/// <para>编辑界面：</para>
/// <para>由 UNIT 规范定义的 Edit 方法在为每种 UNIT 实现提供高度定制的图形用户界面（GUI）功能方面证明非常有用。
/// 其他 PMC 也完全可以利用这一功能。显然，当 PMC 提供 Edit 功能时，能够保存其状态是必要的要求，以避免用户需要反复重新配置 PMC。</para>
/// <para>生命周期：</para>
/// <para>可能没有必要直接暴露初始化或销毁函数，因为这些函数应由使用的中间件（COM/CORBA）自动调用。也就是说，初始化可以在类的构造函数中完成，
/// 而销毁则在析构函数中完成。然而，在某些情况下，客户端可能需要显式调用这些函数。例如，所有可能失败的操作都应通过这些方法调用。
/// 如果这些操作被放置在构造函数或析构函数中，潜在的失败会导致内存泄漏，且难以追踪，因为无法确定组件是否已被创建/销毁。以下是这些方法有用的一些示例：</para>
/// <para>1. Initialize: 客户端可能需要以特定顺序初始化一组 PMC，以处理它们之间的依赖关系。部分 PMC 可能是其他组件的封装，或需要通过
/// 外部文件进行初始化。此初始化过程可能频繁失败，或用户甚至可能选择取消该过程。将这些操作从类构造函数移至 Initialize 方法，
/// 可向客户端明确传达：在某些情况下必须中止组件的构造过程。</para>
/// <para>2. Destructors: PMC 的主要对象应在此处销毁所有次要对象。依赖于原生析构函数可能会在 PMC 对象之间存在循环引用时导致死锁。
/// 如下方示例图所示，当客户端释放对单元操作的引用后，单元和参数仍被其他对象使用。因此，若没有显式的终止方法，它们将永远不会被终止。</para>
/// <para>请参阅接口文档：Utilities Common Interface。</para></remarks>
[ComVisible(false)]
[Description("ICapeUtilities Interface")]
public interface ICapeUtilities
{
    /// <summary>获取组件的参数集合。</summary>
    /// <remarks><para>返回公共单元参数集合（即<see cref="ICapeCollection"/>）。</para>
    /// <para>这些以一组暴露接口的元素形式提供 <see cref="ICapeParameter"/>。从那里，客户端可以
    /// 提取 <see cref="ICapeParameterSpec"/> 接口或任何类型接口，如 <see cref="ICapeRealParameterSpec"/>，
    /// 一旦客户端确定参数的类型为 double。</para></remarks>
    /// <value>单元操作的参数集合。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [DispId(1), Description("Gets parameter collection")]
    ParameterCollection Parameters { get; }

    /// <summary>设置组件的模拟上下文。</summary>
    /// <remarks><para>允许 PME 向 PMC 传递对其模拟上下文的引用。该模拟上下文将由 PME 对象组成，这些对象将暴露一组特定的 CO 接口。
    /// 每个接口均可使 PMC 调用 PME 以利用其暴露的服务（例如物流模板创建、诊断或测量单位转换）。
    /// 若 PMC 不支持访问模拟上下文，建议触发 ECapeNoImpl 错误。</para>
    /// <para>最初，此方法仅存在于 ICapeUnit 接口中。由于 ICapeUtilities.SetSimulationContext 现已适用于
    /// 任何类型的 PMC，因此 ICapeUnit.SetSimulationContext 已被废弃。</para></remarks>
    /// <value>对 PME 模拟上下文类的引用。为了使 PMC 能够使用该类，此引用必须转换为每个已定义的 CO 模拟上下文接口。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    [DispId(2), Description("Set the simulation context")]
    ICapeSimulationContext SimulationContext { get; set; }

    /// <summary>该组件被要求进行自我配置。例如，一个单元操作可能在此次调用过程中创建端口和参数。</summary>
    /// <remarks><para>最初，此方法仅存在于 ICapeUnit 接口中。由于 ICapeUtilities.Initialize 现已适用于
    /// 任何类型的 PMC，ICapeUnit.Initialize 已被废弃。</para>
    /// <para>PME 将通过此方法命令 PMC 进行初始化。任何可能失败的初始化操作都必须在此处进行。Initialize 方法保证是客户端调用的
    /// 第一个方法（不包括类构造函数或初始化持久化方法等低级方法）。Initialize 方法必须在 PMC 在特定流程图中实例化时被调用一次。</para>
    /// <para>当初始化失败时，在触发错误之前，PMC 必须释放所有在失败发生前分配的资源。当 PME 接收到此错误时，它可能不再使用 PMC。</para>
    /// <para>当前接口的终止方法不得被调用。因此，PME 只能通过中间件的原生机制释放 PMC。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [DispId(3), Description("Configuration has to take place here")]
    void Initialize();

    /// <summary>清理任务可在此执行。参数和端口的引用在此释放。</summary>
    /// <remarks><para>最初，此方法仅存在于 ICapeUnit 接口中。由于 ICapeUtilities.Terminate 现已适用于
    /// 任何类型的 PMC，ICapeUnit.Terminate 已被废弃。</para>
    /// <para>PME 将通过此方法命令 PMC 进行销毁。任何可能失败的初始化操作都必须在此处进行。Terminate 方法保证是客户端调用的
    /// 最后一个方法（不包括类析构函数等低级方法）。Terminate 方法可能在任何时候被调用，但只能被调用一次。</para>
    /// <para>当此方法返回错误时，PME 应报告用户。然而，之后 PME 不再被允许使用 PMC。</para>
    /// <para>单元规范中规定：“终止操作可检查数据是否已保存，若未保存则返回错误”。建议不要遵循此建议，因为保存 PMC 状态的责任在于 PME，
    /// 而非每个 PMC。若用户希望在未保存的情况下关闭模拟案例，应由 PME 统一处理此情况，而非让每个 PMC 提供不同的实现方式。</para></remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    [DispId(4), Description("Clean up has to take place here")]
    void Terminate();

    /// <summary>如果可用，显示PMC图形界面。</summary>
    /// <remarks>PMC 显示其用户界面，并允许流程图用户与之交互。如果没有可用用户界面，则返回错误。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [DispId(5), Description("Displays the graphic interface")]
    DialogResult Edit();
}

/// <summary>表示用于处理 PMC 模拟上下文更改的方法。</summary>
[ComVisible(false)]
public delegate void SimulationContextChangedHandler(object sender, EventArgs args);
