/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.18
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace CapeOpenCore.Class;

/// <summary>这是所有基于.Net 的 CAPE-OPEN 异常类的抽象基类。</summary>
/// <remarks><para>.NET 与 COM 相比的主要优点之一是在异常处理中包含了额外的信息。在 COM 中，异常是通过返回
/// 一个 HRESULT 值来处理的，它是一个整数，指示函数调用是否已经成功返回（Rogerson，1997）。
/// 因为 HRESULT 值是 32 位整数，它可以指示比单纯的成功或失败更多的信息，但它的局限性在于它不包括关于发生的异常的描述性信息。</para>
/// <para>在 .NET 中，有一个可用的应用异常类（System.ApplicationException），可以用来提供诸如消息和异常来源等信息。
/// CAPE-OPEN 异常定义都源自于一个 ECapeRoot 接口（贝奥迪等人，2001 年）。在 CAPE-OPEN 异常类的当前实现中，
/// 所有异常类都继承自 CapeUserException 类，而 CapeUserException 类本身又继承自 .NET 的
/// System.ApplicationException 类。CapeUserException 类暴露了 <see cref="ECapeRoot"/> 和
/// <see cref="ECapeUser"/> 接口。通过这种方式，由过程建模组件抛出的所有异常，除了作为继承的异常类型被捕获外，
/// 还可以作为 CapeRootException 或 System.ApplicationException 被捕获。</para></remarks>
[Serializable,ComVisible(true)]
[Guid("28686562-77AD-448f-8A41-8CF9C3264A3E")]
[Description("")]
[ClassInterface(ClassInterfaceType.None)]
public abstract class CapeUserException : ApplicationException, ECapeRoot, ECapeUser
{
    /// <summary>抛出异常时所使用的异常接口的名称。</summary>
    /// <remarks>MInterfaceName 字段是在 <see cref="Initialize">Initialize</see> 方法中为该异常设置的。
    /// 任何从 CapeUserException 类派生的异常都必须在 Initialize 方法中设置此值。</remarks>
    protected string MInterfaceName;
    
    /// <summary>抛出的异常的名称。</summary>
    /// <remarks>MName 字段是在 <see cref="Initialize">Initialize</see> 方法中为该异常设置的。
    /// 任何从 CapeUserException 类派生的异常都需在 Initialize 方法中设置此值。</remarks>
    protected string MName;
    
    /// <summary>抛出的异常的描述。</summary>
    /// <remarks>MDescription 字段是在异常的 <see cref="Initialize">Initialize</see> 方法中设置的。
    /// 任何从 CapeUserException 类派生的异常都需要在 Initialize 方法中设置此值。</remarks>
    protected string MDescription;

    /// <summary>初始化 CapeUserException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    protected CapeUserException()
    {
        MDescription = "An application error has occurred.";
        Initialize();
    }
    
    /// <summary>初始化 CapeUserException 类的新的实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    protected CapeUserException(string message) : base(message)
    {
        MDescription = message;
        Initialize();
    }

    /// <summary>初始化 CapeUserException 类的新的实例，使用序列化数据。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目标的上下文信息。</param>
    protected CapeUserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Initialize();
    }

    /// <summary>初始化 CapeUserException 类的新的实例，并指定错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    protected CapeUserException(string message, Exception inner) : base(message, inner)
    {
        MDescription = message;
        Initialize();
    }
    
    /// <summary>一个虚拟抽象函数，被派生类继承，用于初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>派生类应实现此类并设置 HResult、接口名称和异常名称的值。
    /// <code>
    /// virtual void Initialize() override 
    /// {
    ///  HResult = (int)CapeErrorInterfaceHR.ECapeUnknownHR;
    ///  m_interfaceName = "ECapeUnknown";
    ///  m_name="CUnknownException";
    /// }
    /// </code></remarks>
    protected abstract void Initialize();

    /// <summary>控制 COM 注册的功能。</summary>
    /// <remarks>该函数添加了 CAPE-OPEN 方法与工具规范中指定的注册键。特别是，它表明该单元操作实现了
    /// CAPE-OPEN 单元操作类别标识。此外，它还使用 <see cref="CapeNameAttribute"/>、
    /// <see cref="CapeDescriptionAttribute"/>、<see cref=" CapeVersionAttribute"/>、
    /// <see cref="CapeVendorURLAttribute"/>、<see cref="CapeHelpURLAttribute"/> 和
    /// <see cref="CapeAboutAttribute"/> 属性来添加 CapeDescription 注册键。</remarks>
    /// <param name="t">正在注册的类类型。</param> 
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [ComRegisterFunction]
    public static void RegisterFunction(Type t) { }
    
    /// <summary>此功能用于控制在卸载类时从 COM 注册表中删除该类。</summary>
    /// <remarks>该方法将删除添加到该类注册表中的所有子键，包括由
    /// <see cref="RegisterFunction"/> 方法添加的特定于 CAPE-OPEN 的键。</remarks>
    /// <param name="t">该类未注册。</param> 
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [ComUnregisterFunction]
    public static void UnregisterFunction(Type t) { }
    
    // ECapeRoot method
    // 返回 System.ApplicationException 中的消息字符串。
    /// <summary>抛出的异常的名称。</summary>
    /// <remarks>抛出的异常的名称。</remarks>
    /// <value>抛出的异常的名称。</value>
    public string Name => MName;

    /// <summary>用于指定错误子类别的代码。</summary>
    /// <remarks>值的分配由每个实现自行决定。因此，这是一段专用于 CO 组件提供者的私有代码。
    /// 默认情况下，它被设置为 CAPE-OPEN 错误 HRESULT，参见 <see cref="CapeErrorInterfaceHR"/>。</remarks>
    /// <value>异常的 HRESULT 值。</value>
    public int code => HResult;

    /// <summary>错误的描述。</summary>
    /// <remarks>错误描述可以包含对导致错误的条件更详细的说明。</remarks>
    /// <value>异常的字符串描述。</value>
    public string description
    {
        get => MDescription;
        set => MDescription = value;
    }

    /// <summary>错误的范围。</summary>
    /// <remarks>该属性提供了一份包含错误发生位置的软件包列表。例如 <see cref="ICapeIdentification"/>。</remarks>
    /// <value>错误的来源。</value>
    public string scope => Source;

    /// <summary>发生错误的接口名称。此为必填字段。</summary>
    /// <remarks>引发错误的接口。</remarks>
    /// <value>接口的名称。</value>
    public string interfaceName
    {
        get => MInterfaceName;
        set => MInterfaceName = value;
    }

    /// <summary>发生错误的操作名称。此为必填字段。</summary>
    /// <remarks>此字段显示异常发生时正在执行的操作的名称。</remarks>
    /// <value>操作的名称。</value>
    public string operation => StackTrace;

    /// <summary>指向包含更多错误信息页面的URL，该页面、文档或网站可提供详细的错误说明。此类信息的内容显然取决于具体实现方式。</summary>
    /// <remarks>此字段提供了一个互联网网址，通过该网址可以获取有关此错误的更多信息。</remarks>
    /// <value>URL 链接。</value>
    public string moreInfo => HelpLink;
}

/// <summary>当操作指定的其他错误不适用时，将引发此异常。</summary>
/// <remarks>一种标准的异常，可由 CAPE-OPEN 对象抛出，以表示发生的错误不属于该对象所支持的任何其他异常类型。</remarks>
[Serializable,ComVisible(true)]
[Guid("B550B2CA-6714-4e7f-813E-C93248142410")]
[Description("")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeUnknownException : CapeUserException, ECapeUnknown
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeUnknownHR;
        MInterfaceName = "ECapeUnknown";
        MName = "CUnknownException";
    }
    
    /// <summary>初始化 CapeUnknownException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeUnknownException() { }
    
    /// <summary>初始化一个新的 CapeUnknownException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeUnknownException(string message) : base(message) { }
    
    /// <summary>初始化 CapeUnknownException 类的新的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    public CapeUnknownException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    
    /// <summary>初始化一个新的 CapeUnknownException 类实例，该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeUnknownException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>当操作指定的其他错误不适用时，将引发此异常。</summary>
/// <remarks>一种标准的异常，可由 CAPE-OPEN 对象抛出，以表示发生的错误不属于该对象所支持的任何其他异常类型。</remarks>
[Serializable,ComVisible(true)]
[Guid("16049506-E086-4baf-9905-9ED13D50D0E3")]
[Description("")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeUnexpectedException : CapeUserException
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)0x8000ffff);
        MInterfaceName = "IPersistStreamInit";
        MName = "CUnexpectedException";
    }
    
    /// <summary>初始化 CapeUnknownException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeUnexpectedException() { }
    
    /// <summary>初始化 CapeUnknownException 类的新的实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeUnexpectedException(string message) : base(message) { }
    
    /// <summary>初始化 CapeUnknownException 类的新的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    public CapeUnexpectedException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
    
    /// <summary>初始化一个新的 CapeUnknownException 类实例，该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeUnexpectedException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>与任何数据相关的错误层次结构的基类。</summary>
/// <remarks>CapeDataException 类是与数据相关的错误的基础类。数据是操作的参数，
/// 来自参数通用接口的参数，以及关于许可证密钥的信息。</remarks>
[Serializable]
[Guid("53551E7C-ECB2-4894-B71A-CCD1E7D40995")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeDataException : CapeUserException, ECapeData
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeDataHR;
        MName = "CapeDataException";
    }

    /// <summary>初始化 CapeDataException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeDataException() { }
    
    /// <summary>初始化 CapeDataException 类的新的实例，并指定错误消息。 </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeDataException(string message) : base(message) { }
    
    /// <summary>初始化 CapeDataException 类的新的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    public CapeDataException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
    
    /// <summary>初始化 CapeDataException 类的新的实例，并指定错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeDataException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>一个参数（该参数属于参数通用接口）的状态无效。</summary>
/// <remarks>无效参数的名称以及参数本身均可从异常中获取。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("667D34E9-7EF7-4ca8-8D17-C7577F2C5B62")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeBadCoParameter : CapeDataException, ECapeBadCOParameter
{
    private string _mParameterName;
    private object _mParameter;

    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks><para>设置 HResult、接口名称和异常名称的值。</para></remarks>
    /// <param name="pParameterName">具有无效状态的参数名称。</param>
    /// <param name="pParameter">状态为无效的参数。</param>
    protected void Initialize(string pParameterName, object pParameter)
    {
        _mParameterName = pParameterName;
        // 这里有疑问，理论上来说应该是 pParameter
        // _mParameter = (ICapeParameter)pParameter;
        _mParameter = (ICapeParameter)parameter;
        HResult = (int)CapeErrorInterfaceHR.ECapeBadArgumentHR;
        MInterfaceName = "ECapeBadArgument";
        MName = "CapeBadArgumentException";
    }

    /// <summary>初始化 CapeBadCOParameter 类的新的实例，使用参数的名称和导致异常的参数。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    /// <param name="parameterName">具有无效状态的参数名称。</param>
    /// <param name="parameter">状态为无效的参数。</param>
    public CapeBadCoParameter(string parameterName, object parameter)
    {
        Initialize(parameterName, parameter);
    }

    /// <summary>初始化 CapeBadCOParameter 类的实例，并指定错误消息、参数名称以及引发异常的参数。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    /// <param name="parameterName">具有无效状态的参数名称。</param>
    /// <param name="parameter">状态为无效的参数。</param>
    public CapeBadCoParameter(string message, string parameterName, object parameter)
        : base(message)
    {
        Initialize(parameterName, parameter);
    }

    /// <summary>初始化 CapeBadCOParameter 类的新的实例，使用序列化数据、参数名称以及引发异常的参数。</summary>
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    /// <param name="parameterName">具有无效状态的参数名称。</param>
    /// <param name="parameter">状态为无效的参数。</param>
    public CapeBadCoParameter(SerializationInfo info, StreamingContext context,
        string parameterName, object parameter)
        : base(info, context)
    {
        Initialize(parameterName, parameter);
    }

    /// <summary>初始化 CapeBadCOParameter 类的实例，指定错误消息和对内部异常的引用，以及参数的名称和引发异常的参数。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    /// <param name="parameterName">具有无效状态的参数名称。</param>
    /// <param name="parameter">状态为无效的参数。</param>
    public CapeBadCoParameter(string message, Exception inner, string parameterName, object parameter)
        : base(message, inner)
    {
        Initialize(parameterName, parameter);
    }

    /// <summary>引发异常的 CO 参数的名称。</summary>
    /// <remarks>这提供了引发异常的参数的名称。</remarks>
    /// <value>引发异常的参数名称。</value>
    public virtual object parameter => _mParameter;

    /// <summary>引发异常的 CO 参数的名称。</summary>
    /// <remarks>这提供了对引发异常的参数的访问权限。</remarks>
    /// <value>引发异常的参数。</value>
    public virtual string parameterName => MName;
}

/// <summary>操作的参数值不正确。</summary>
/// <remarks>该操作的参数值不正确。参数值在操作签名中的位置。第一个参数位于位置 1。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("D168E99F-C1EF-454c-8574-A8E26B62ADB1")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeBadArgumentException : CapeDataException, ECapeBadArgument, ECapeBadArgument093
{
    private int _mPosition;

    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks><para>设置 HResult、接口名称和异常名称的值。</para></remarks>
    /// <param name="pPosition">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    protected void Initialize(int pPosition)
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeBadArgumentHR;
        MInterfaceName = "ECapeBadArgument";
        MName = "CapeBadArgumentException";
        _mPosition = pPosition;
    }

    /// <summary>初始化 CapeBadArgumentException 类的新的实例，并指定错误的位置。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    public CapeBadArgumentException(int position)
    {
        Initialize(position);
    }

    /// <summary>初始化一个新的 CapeBadArgumentException 类实例，并指定错误消息和错误位置。</summary>. 
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    public CapeBadArgumentException(string message, int position) : base(message)
    {
        Initialize(position);
    }

    /// <summary>初始化 CapeBadArgumentException 类的新的实例，使用序列化数据和错误的位置。</summary>.
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    public CapeBadArgumentException(SerializationInfo info, StreamingContext context, int position)
        : base(info, context)
    {
        Initialize(position);
    }

    /// <summary>初始化 CapeBadArgumentException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    public CapeBadArgumentException(string message, Exception inner, int position)
        : base(message, inner)
    {
        Initialize(position);
    }
    
    /// <summary>操作签名中参数值的位置。第一个参数的位置为 1。</summary>
    /// <remarks>这提供了函数调用中无效参数在参数列表中的位置。</remarks>
    /// <value>该论点的立场存在问题。第一个论点是 1。</value>
    public virtual short position => (short)_mPosition;

    /// <summary>操作签名中参数值的位置。第一个参数的位置为 1。</summary>
    /// <remarks>这提供了函数调用中无效参数在参数列表中的位置。</remarks>
    /// <value>该论点的立场存在问题。第一个论点是 1。</value>
    int ECapeBadArgument093.position => _mPosition;
}

/// <summary>这是一个抽象类，它允许子类提供关于由于超出范围而导致错误的信息。
/// 它可以被抛出，以表示方法参数或对象参数的值超出了范围。</summary>
/// <remarks><para>CapeBoundariesException 是一个“工具”类，用于分解描述值、其类型及其边界的状态。</para>
/// <para>这是一个抽象类。从这个类中无法抛出任何实际错误。</para></remarks>
[Serializable]
[ComVisible(true)]
[Guid("62B1EE2F-E488-4679-AFA3-D490694D6B33")]
[ClassInterface(ClassInterfaceType.None)]
public abstract class CapeBoundariesException : CapeUserException, ECapeBoundaries
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks><para>设置 HResult、接口名称和异常名称的值。</para></remarks>
    /// <param name="pLowerBound">下限的值。</param>
    /// <param name="pUpperBound">上限的值。</param>
    /// <param name="pValue">导致错误的当前值。</param>
    /// <param name="pType">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    protected void SetBoundaries(double pLowerBound, double pUpperBound, double pValue, string pType)
    {
        lowerBound = pLowerBound;
        upperBound = pUpperBound;
        value = pValue;
        type = pType;
    }

    /// <summary>初始化 CapeBoundariesException 类的新的实例，
    /// 该实例包含导致此异常的参数的下限、上限、值、类型和位置。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    /// <param name="lowerBound">下限的值。</param>
    /// <param name="upperBound">上限的值。</param>
    /// <param name="value">导致错误的当前值。</param>
    /// <param name="type">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    protected CapeBoundariesException(double lowerBound, double upperBound, double value, string type)
    {
        SetBoundaries(lowerBound, upperBound, value, type);
    }
    
    /// <summary>初始化 CapeBoundariesException 类的实例，并指定错误消息、下限、上限、值、类型以及引发此异常的参数的位置。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    /// <param name="lowerBound">下限的值。</param>
    /// <param name="upperBound">上限的值。</param>
    /// <param name="value">导致错误的当前值。</param>
    /// <param name="type">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    protected CapeBoundariesException(string message, 
        double lowerBound, double upperBound, double value, string type) : base(message)
    {
        SetBoundaries(lowerBound, upperBound, value, type);
    }
    
    /// <summary>初始化 CapeBoundariesException 类的新的实例，使用序列化数据、下限、上限、值、类型和导致此异常的参数的位置。</summary>
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    /// <param name="lowerBound">下限的值。</param>
    /// <param name="upperBound">上限的值。</param>
    /// <param name="value">导致错误的当前值。</param>
    /// <param name="type">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    protected CapeBoundariesException(SerializationInfo info, StreamingContext context, 
        double lowerBound, double upperBound, double value, string type) : base(info, context)
    {
        SetBoundaries(lowerBound, upperBound, value, type);
    }
    
    /// <summary>初始化 CapeBoundariesException 类的新的实例，
    /// 指定错误消息、下限、上限、参数的值、类型和位置，以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    /// <param name="lowerBound">下限的值。</param>
    /// <param name="upperBound">上限的值。</param>
    /// <param name="value">导致错误的当前值。</param>
    /// <param name="type">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    protected CapeBoundariesException(string message, Exception inner, 
        double lowerBound, double upperBound, double value, string type) : base(message, inner)
    {
        SetBoundaries(lowerBound, upperBound, value, type);
    }

    /// <summary>下限的值。</summary>
    /// <remarks>这为用户提供了参数的可接受下限。</remarks>
    /// <value>参数的下限。</value>
    public double lowerBound { get; private set; }

    /// <summary>上限的值。</summary>
    /// <remarks>这为用户提供了参数的可接受上限。</remarks>
    /// <value>参数的上限。</value>
    public double upperBound { get; private set; }

    /// <summary>导致错误的当前值。</summary>
    /// <remarks>这为用户提供了导致错误条件的原因。</remarks>
    /// <value>导致错误条件出现的值。</value>
    public double value { get; private set; }

    /// <summary>值的类型/性质。</summary>
    /// <remarks>该值可能代表热力学性质、数据库中的表数量、内存容量等。</remarks>
    /// <value>一个字符串，用于指示所需值的属性或类型。</value>
    public string type { get; private set; }
}

/// <summary>参数值超出范围。</summary>
/// <remarks>此类继承自 <see cref="CapeBoundariesException"/> 类。它用于表示其中一个参数超出了其范围。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("4438458A-1659-48c2-9138-03AD8B4C38D8")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeOutOfBoundsException : 
    CapeBoundariesException, ECapeOutOfBounds, ECapeBadArgument, ECapeBadArgument093, ECapeData
{
    private int _mPosition;

    /// <summary>所有从 CapeOutOfBoundsException 类派生的类的 initialize 方法，都需要包含与边界相关的相关信息。</summary>
    /// <remarks>此方法为密封方法，因此从 CapeOutOfBoundsException 派生的类会包含关于参数位置的必要信息。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeOutOfBoundsHR;
        MInterfaceName = "ECapeOutOfBounds";
        MName = "CapeOutOfBoundsException";
    }

    /// <summary>初始化一个新的 CapeOutOfBoundsException 类实例，并指定错误的位置。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    /// <param name="lowerBound">下限的值。</param>
    /// <param name="upperBound">上限的值。</param>
    /// <param name="value">导致错误的当前值。</param>
    /// <param name="type">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    public CapeOutOfBoundsException(int position, double lowerBound, double upperBound, double value, string type) :
        base(lowerBound, upperBound, value, type)
    {
        _mPosition = position;
    }
    
    /// <summary>初始化一个新的 CapeOutOfBoundsException 类实例，并指定错误消息和错误位置。</summary>. 
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    /// <param name="lowerBound">下限的值。</param>
    /// <param name="upperBound">上限的值。</param>
    /// <param name="value">导致错误的当前值。</param>
    /// <param name="type">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    public CapeOutOfBoundsException(string message, int position, double lowerBound, double upperBound, 
        double value, string type) : base(message, lowerBound, upperBound, value, type)
    {
        _mPosition = position;
    }
    
    /// <summary>初始化一个新的 CapeOutOfBoundsException 类实例，使用序列化数据和错误位置。</summary>.
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    /// <param name="lowerBound">下限的值。</param>
    /// <param name="upperBound">上限的值。</param>
    /// <param name="value">导致错误的当前值。</param>
    /// <param name="type">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    public CapeOutOfBoundsException(SerializationInfo info, StreamingContext context, 
        int position, double lowerBound, double upperBound, double value, string type) :
        base(info, context, lowerBound, upperBound, value, type)
    {
        _mPosition = position;
    }
    
    /// <summary>初始化一个新的 CapeOutOfBoundsException 类实例，该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    /// <param name="lowerBound">下限的值。</param>
    /// <param name="upperBound">上限的值。</param>
    /// <param name="value">导致错误的当前值。</param>
    /// <param name="type">值的类型/性质。该值可能代表热力学性质、数据库中的表数量、内存容量等。</param>
    public CapeOutOfBoundsException(string message, Exception inner, int position, double lowerBound, 
        double upperBound, double value, string type) : 
        base(message, inner, lowerBound, upperBound, value, type)
    {
        _mPosition = position;
    }

    /// <summary>操作签名中参数值的位置。第一个参数的位置为 1。</summary>
    /// <remarks>这提供了函数调用中无效参数在参数列表中的位置。</remarks>
    /// <value>该论点的立场存在问题。第一个论点是 1。</value>
    public short position => (short)_mPosition;

    /// <summary>操作签名中参数值的位置。第一个参数的位置为 1。</summary>
    /// <remarks>这提供了函数调用中无效参数在参数列表中的位置。</remarks>
    /// <value>该论点的立场存在问题。第一个论点是 1。</value>
    int ECapeBadArgument093.position => _mPosition;
}

/// <summary>与计算相关的错误层次结构的基类。</summary>
/// <remarks>此类用于指示在进行计算时发生了错误。其他与计算相关的类，如 
/// <see cref="CapeFailedInitialisationException">CapeOpen.CapeFailedInitialisationException</see>、
/// <see cref="CapeOutOfResourcesException">CapeOpen.CapeOutOfResourcesException</see>、
/// <see cref="CapeSolvingErrorException">CapeOpen.CapeSolvingErrorException</see>、
/// <see cref="CapeBadInvOrderException">CapeOpen.CapeBadInvOrderException</see>、
/// <see cref="CapeInvalidOperationException">CapeOpen.CapeInvalidOperationException</see>、
/// <see cref="CapeNoMemoryException">CapeOpen.CapeNoMemoryException</see> 和 
/// <see cref="CapeTimeOutException">CapeOpen.CapeTimeOutException</see> 从该类派生。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("9D416BF5-B9E3-429a-B13A-222EE85A92A7")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeComputationException : CapeUserException, ECapeComputation
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks><para>设置 HResult、接口名称和异常名称的值。</para></remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeComputationHR;
        MInterfaceName = "ECapeComputation";
        MName = "CapeComputationException";
    }

    /// <summary>初始化 CapeComputationException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeComputationException() { }
    
    /// <summary>初始化一个新的 CapeComputationException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeComputationException(string message) : base(message) { }
    
    /// <summary>初始化 CapeComputationException 类的新的实例，并使用序列化数据。</summary>
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    public CapeComputationException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
    
    /// <summary>初始化一个新的 CapeComputationException 类实例，并指定错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeComputationException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>当必要的初始化操作未执行或执行失败时，将抛出此异常。</summary>
/// <remarks>先决条件操作无效。必要的初始化操作未执行或执行失败。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("E407595C-6D1C-4b8c-A29D-DB0BE73EFDDA")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeFailedInitialisationException : CapeComputationException, ECapeFailedInitialisation
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeFailedInitialisationHR;
        MInterfaceName = "ECapeFailedInitialisation";
        MName = "CapeFailedInitialisationException";
    }

    /// <summary>初始化 CapeFailedInitialisationException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeFailedInitialisationException() { }
    
    /// <summary>初始化一个新的 CapeFailedInitialisationException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeFailedInitialisationException(string message) : base(message) { }
    
    /// <summary>初始化 CapeFailedInitialisationException 类的新的实例，并使用序列化数据。</summary>
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    public CapeFailedInitialisationException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
    
    /// <summary>初始化一个新的 CapeFailedInitialisationException 类实例，并指定错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeFailedInitialisationException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>与当前实现相关的错误层次结构的基类。</summary>
/// <remarks>此类用于指示在某个对象的实现过程中发生了错误。与实现相关的类，如 
/// <see cref="CapeNoImplException ">CapeOpen.CapeNoImplException </see> 和 
/// <see cref="CapeLimitedImplException ">CapeOpen.CapeLimitedImplException </see> 从此类派生。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("7828A87E-582D-4947-9E8F-4F56725B6D75")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeImplementationException : CapeUserException, ECapeImplementation
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks><para>设置 HResult、接口名称和异常名称的值。</para></remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeImplementationHR;
        MInterfaceName = "ECapeImplementation";
        MName = "CapeImplementationException";
    }

    /// <summary>初始化 CapeImplementationException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeImplementationException() { }
    
    /// <summary>初始化一个新的 CapeImplementationException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeImplementationException(string message) : base(message) { }
    
    /// <summary>初始化 CapeImplementationException 类的新的实例，并使用序列化数据。</summary>
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    public CapeImplementationException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
    
    /// <summary>初始化一个新的CapeImplementationException类实例，该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeImplementationException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>传递的参数值无效。例如，所传递的相态名称并不属于 CO 相态列表。</summary>
/// <remarks>该操作的参数值无效。参数值在操作签名中的位置。第一个参数位于位置 1。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("B30127DA-8E69-4d15-BAB0-89132126BAC9")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeInvalidArgumentException : CapeBadArgumentException, ECapeInvalidArgument
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks><para>设置 HResult、接口名称和异常名称的值。</para></remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeInvalidArgumentHR;
        MInterfaceName = "ECapeInvalidArgument";
        MName = "CapeInvalidArgumentException";
    }

    /// <summary>初始化 CapeInvalidArgumentException 类的新的实例，并指定错误的位置。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    public CapeInvalidArgumentException(int position) : base(position) { }
    
    /// <summary>初始化一个新的 CapeInvalidArgumentException 类实例，并指定错误消息和错误位置。</summary>. 
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    public CapeInvalidArgumentException(string message, int position) : base(message, position) { }
    
    /// <summary>初始化一个新的 CapeInvalidArgumentException 类实例，使用序列化数据和错误位置。</summary>.
    /// <remarks> 这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">关于源或目的地的上下文信息。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    public CapeInvalidArgumentException(SerializationInfo info, StreamingContext context, int position) 
        : base(info, context, position) { }
    
    /// <summary>初始化一个新的 CapeInvalidArgumentException 类实例，该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    /// <param name="position">操作签名中参数值的位置。第一个参数的位置为 1。</param>
    public CapeInvalidArgumentException(string message, Exception inner, int position) 
        : base(message, inner, position) { }
}
