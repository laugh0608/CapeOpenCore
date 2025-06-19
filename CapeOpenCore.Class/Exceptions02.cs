/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.19
 */

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace CapeOpenCore.Class;

/// <summary>此操作在当前上下文中无效。</summary>
/// <remarks>当尝试执行在当前上下文中无效的操作时，将抛出此异常。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("C0B943FE-FB8F-46b6-A622-54D30027D18B")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeInvalidOperationException : CapeComputationException, ECapeInvalidOperation
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeInvalidOperationHR;
        MInterfaceName = "ECapeInvalidOperation";
        MName = "CapeInvalidOperationException";
    }
    
    /// <summary>初始化 CapeInvalidOperationException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeInvalidOperationException() { }
    
    /// <summary>初始化一个新的 CapeInvalidOperationException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeInvalidOperationException(string message) : base(message) { }
    
    /// <summary>初始化 CapeInvalidOperationException 类的新的实例，使用序列化数据。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeInvalidOperationException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
    
    /// <summary>初始化一个新的 CapeInvalidOperationException 类实例，
    /// 该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeInvalidOperationException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>在执行操作请求之前，未调用必要的先决条件操作。</summary>
/// <remarks>指定的先决条件操作必须在引发此异常的操作之前被调用。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("07EAD8B4-4130-4ca6-94C1-E8EC4E9B23CB")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeBadInvOrderException : CapeComputationException, ECapeBadInvOrder
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeBadInvOrderHR;
        MInterfaceName = "ECapeBadInvOrder";
        MName = "CapeBadInvOrderException";
    }
    
    /// <summary>初始化一个新的 CapeBadInvOrderException 类实例。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="operation">必要的先决条件操作。</param>
    public CapeBadInvOrderException(string operation)
    {
        requestedOperation = operation;
    }
    
    /// <summary>初始化一个新的 CapeBadInvOrderException 类实例，并指定错误消息和引发异常的操作名称。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    /// <param name="operation">必要的先决条件操作。</param>
    public CapeBadInvOrderException(string message, string operation) : base(message)
    {
        requestedOperation = operation;
    }
    
    /// <summary>初始化一个新的 CapeBadInvOrderException 类实例，使用序列化数据和引发异常的操作名称。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    /// <param name="operation">必要的先决条件操作。</param>
    public CapeBadInvOrderException(SerializationInfo info, StreamingContext context, string operation)
        : base(info, context)
    {
        requestedOperation = operation;
    }
    /// <summary>初始化 CapeBadInvOrderException 类的新实例，同时传递指定的错误信息、
    /// 内嵌异常的引用，以及引发此异常的操作用的名称。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    /// <param name="operation">必要的先决条件操作。</param>
    public CapeBadInvOrderException(string message, Exception inner, string operation)
        : base(message, inner)
    {
        requestedOperation = operation;
    }

    /// <summary>必要的先决条件操作。</summary>
    /// <remarks>在调用当前操作之前，必须先调用先决条件操作。</remarks>
    /// <value>必要先决条件操作的名称。</value>
    public string requestedOperation { get; }
}

/// <summary>由于未遵守许可协议，该操作无法完成。</summary>
/// <remarks>Of course, this type of error could also appear outside the CO scope. In this case, 
/// the error does not belong to the CO error handling. It is specific to the platform.</remarks>
[Serializable]
[ComVisible(true)]
[Guid("CF4C55E9-6B0A-4248-9A33-B8134EA393F6")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeLicenceErrorException : CapeDataException, ECapeLicenceError
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeLicenceErrorHR);
        MInterfaceName = "ECapeLicenceError";
        MName = "CapeLicenceErrorException";
    }

    /// <summary>Initializes a new instance of the CapeLicenceErrorException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeLicenceErrorException()
    { }
    /// <summary>Initializes a new instance of the CapeLicenceErrorException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeLicenceErrorException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeLicenceErrorException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeLicenceErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeLicenceErrorException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeLicenceErrorException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>The limit of the implementation has been violated.</summary>
/// <remarks><para>An operation may be partially implemented for example a Property Package could 
/// implement TP flash but not PH flash. If a caller requests for a PH flash, then 
/// this error indicates that some flash calculations are supported but not the 
/// requested one.
/// </para>
/// <para>The factory can only create one instance (because the component is an 
/// evaluation copy), when the caller requests for a second creation this error shows 
/// that this implementation is limited.
/// </para></remarks>
[Serializable]
[Guid("5E6B74A2-D603-4e90-A92F-608E3F1CD39D")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeLimitedImplException : CapeImplementationException, ECapeLimitedImpl
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeLimitedImplHR);
        MInterfaceName = "ECapeLimitedImpl";
        MName = "CapeLimitedImplException";
    }

    /// <summary>Initializes a new instance of the CapeLimitedImplException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeLimitedImplException()
    { }
    /// <summary>Initializes a new instance of the CapeLimitedImplException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeLimitedImplException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeLimitedImplException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeLimitedImplException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeLimitedImplException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeLimitedImplException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>An exception class that indicates that the requested operation has not been implemented by the current object.</summary>
/// <remarks>The operation is “not” implemented even if this operation can be called due 
/// to the compatibility with the CO standard. That is to say that the operation 
/// exists but it is not supported by the current implementation.</remarks>
[Serializable]
[Guid("1D2488A6-C428-4e38-AFA6-04F2107172DA")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeNoImplException : CapeImplementationException, ECapeNoImpl
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeNoImplHR);
        MInterfaceName = "ECapeNoImpl";
        MName = "CapeNoImplException";
    }

    /// <summary>Initializes a new instance of the CapeNoImplException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeNoImplException()
    { }
    /// <summary>Initializes a new instance of the CapeNoImplException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeNoImplException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeNoImplException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeNoImplException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeNoImplException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeNoImplException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>An exception class that indicates that the resources required by this operation are not available.</summary>
/// <remarks>The physical resources necessary to the execution of the operation are out of limits.</remarks>
[Serializable]
[Guid("42B785A7-2EDD-4808-AC43-9E6E96373616")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeOutOfResourcesException : CapeUserException, ECapeOutOfResources, ECapeComputation
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeOutOfResourcesHR);
        MInterfaceName = "ECapeOutOfResources";
        MName = "CapeOutOfResourcesException";
    }

    /// <summary>Initializes a new instance of the CapeOutOfResourcesException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeOutOfResourcesException()
    {
    }
    /// <summary>Initializes a new instance of the CapeOutOfResourcesException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeOutOfResourcesException(string message)
        : base(message)
    {
    }
    /// <summary>Initializes a new instance of the CapeOutOfResourcesException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeOutOfResourcesException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
    /// <summary>Initializes a new instance of the CapeOutOfResourcesException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeOutOfResourcesException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

/// <summary>An exception class that indicates that the memory required for this operation is not available.</summary>
/// <remarks>The physical memory necessary to the execution of the operation is out of limit.</remarks>
[Serializable]
[Guid("1056A260-A996-4a1e-8BAE-9476D643282B")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeNoMemoryException : CapeUserException, ECapeNoMemory
{

    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeNoMemoryHR);
        MInterfaceName = "ECapeNoMemory";
        MName = "CapeNoMemoryException";
    }

    /// <summary>Initializes a new instance of the CapeNoMemoryException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeNoMemoryException()
    { }
    /// <summary>Initializes a new instance of the CapeNoMemoryException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeNoMemoryException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeNoMemoryException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeNoMemoryException(SerializationInfo info, StreamingContext context)
    { }
    /// <summary>Initializes a new instance of the CapeNoMemoryException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeNoMemoryException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>An exception class that indicates that the a persistence-related error has occurred.</summary>
/// <remarks>The base class of the errors hierarchy related to the persistence.</remarks>
[Serializable]
[Guid("3237C6F8-3D46-47ee-B25F-52485A5253D8")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceException : CapeUserException, ECapePersistence
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapePersistenceHR);
        MInterfaceName = "ECapePersistence";
        MName = "CapePersistenceException";
    }

    /// <summary>Initializes a new instance of the CapePersistenceException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapePersistenceException()
    { }
    /// <summary>Initializes a new instance of the CapePersistenceException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapePersistenceException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapePersistenceException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapePersistenceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapePersistenceException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapePersistenceException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>An exception class that indicates that the persistence was not found.</summary>
/// <remarks>The requested object, table, or something else within the persistence system was not found.</remarks>
[Serializable]
[Guid("271B9E29-637E-4eb0-9588-8A53203A3959")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceNotFoundException : CapePersistenceException, ECapePersistenceNotFound
{
    private string m_ItemName;

    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    /// <param name="itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    protected void Initialize(string itemName)
    {
        m_ItemName = itemName;
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapePersistenceNotFoundHR);
        MInterfaceName = "ECapePersistenceNotFound";
        MName = "CapePersistenceNotFoundException";
    }

    /// <summary>Initializes a new instance of the CapePersistenceNotFoundException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    /// <param name="itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    public CapePersistenceNotFoundException(string itemName)
    { Initialize(itemName); }
    /// <summary>Initializes a new instance of the CapePersistenceNotFoundException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    /// <param name="itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    public CapePersistenceNotFoundException(string message, string itemName) : base(message) { Initialize(itemName); }
    /// <summary>Initializes a new instance of the CapePersistenceNotFoundException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    /// <param name="itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    public CapePersistenceNotFoundException(SerializationInfo info, StreamingContext context, string itemName) : base(info, context) { Initialize(itemName); }
    /// <summary>Initializes a new instance of the CapePersistenceNotFoundException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    /// <param name="itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    public CapePersistenceNotFoundException(string message, Exception inner, string itemName) : base(message, inner) { Initialize(itemName); }

    /// <summary>Name of the item that was not found that is the cause of this exception. </summary>
    /// <remarks>Name of the item that was not found that is the cause of this exception. </remarks>
    /// <value>
    /// Name of the item that was not found that is the cause of this exception. 
    /// </value>
    public string itemName
    {
        get
        {
            return m_ItemName;
        }
    }

}

/// <summary>An exception class that indicates an overflow of internal persistence system.</summary>
/// <remarks>During the persistence process, an overflow of internal persistence system occurred.</remarks>
[Serializable]
[Guid("A119DE0B-C11E-4a14-BA5E-9A2D20B15578")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceOverflowException : 
    CapeUserException, ECapePersistenceOverflow, ECapePersistence
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapePersistenceOverflowHR);
        MInterfaceName = "ECapePersistenceOverflow";
        MName = "CapePersistenceOverflowException";
    }
    /// <summary>Initializes a new instance of the CapePersistenceOverflowException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapePersistenceOverflowException()
    {
    }
    /// <summary>Initializes a new instance of the CapePersistenceOverflowException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapePersistenceOverflowException(string message)
    {
    }
    /// <summary>Initializes a new instance of the CapePersistenceOverflowException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapePersistenceOverflowException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapePersistenceOverflowException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapePersistenceOverflowException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>An exception class that indicates a severe error occurred within the persistence system.</summary>
/// <remarks>During the persistence process, a severe error occurred within the persistence system.</remarks>
[Serializable]
[Guid("85CB2D40-48F6-4c33-BF0C-79CB00684440")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceSystemErrorException : CapePersistenceException, ECapePersistenceSystemError
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapePersistenceSystemErrorHR);
        MInterfaceName = "ECapePersistenceSystemError";
        MName = "CapePersistenceSystemErrorException";
    }

    /// <summary>Initializes a new instance of the CapePersistenceSystemErrorException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapePersistenceSystemErrorException()
    { }
    /// <summary>Initializes a new instance of the CapePersistenceSystemErrorException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapePersistenceSystemErrorException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapePersistenceSystemErrorException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapePersistenceSystemErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapePersistenceSystemErrorException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapePersistenceSystemErrorException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>The access to something within the persistence system is not authorised.</summary>
/// <remarks>This exception is thrown when the access to something within the persistence system is not authorised.</remarks>
[Serializable]
[Guid("45843244-ECC9-495d-ADC3-BF9980A083EB")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeIllegalAccessException : CapePersistenceException, ECapeIllegalAccess
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeIllegalAccessHR);
        MInterfaceName = "ECapeIllegalAccess";
        MName = "CapeIllegalAccessException";
    }

    /// <summary>Initializes a new instance of the CapeIllegalAccessException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeIllegalAccessException()
    { }
    /// <summary>Initializes a new instance of the CapeIllegalAccessException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeIllegalAccessException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeIllegalAccessException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeIllegalAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeIllegalAccessException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeIllegalAccessException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>An exception class that indicates a numerical algorithm failed for any reason.</summary>
/// <remarks>Indicates that a numerical algorithm failed for any reason.</remarks>
[Serializable]
[Guid("F617AFEA-0EEE-4395-8C82-288BF8F2A136")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeSolvingErrorException : CapeComputationException, ECapeSolvingError
{

    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeSolvingErrorHR);
        MInterfaceName = "ECapeSolvingError";
        MName = "CapeSolvingErrorException";
    }

    /// <summary>Initializes a new instance of the CapeSolvingErrorException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeSolvingErrorException()
    { }
    /// <summary>Initializes a new instance of the CapeSolvingErrorException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeSolvingErrorException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeSolvingErrorException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeSolvingErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeSolvingErrorException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeSolvingErrorException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>Exception thrown when the Hessian for the MINLP problem is not available.</summary>
/// <remarks>Exception thrown when the Hessian for the MINLP problem is not available.</remarks>
[Serializable]
[Guid("3044EA08-F054-4315-B67B-4E3CD2CF0B1E")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeHessianInfoNotAvailableException : 
    CapeSolvingErrorException, ECapeHessianInfoNotAvailable
{

    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeHessianInfoNotAvailableHR);
        MInterfaceName = "ECapeHessianInfoNotAvailable";
        MName = "CapeHessianInfoNotAvailableHR";
    }
    /// <summary>Initializes a new instance of the CapeHessianInfoNotAvailableException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeHessianInfoNotAvailableException()
    { }
    /// <summary>Initializes a new instance of the CapeHessianInfoNotAvailableException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeHessianInfoNotAvailableException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeHessianInfoNotAvailableException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeHessianInfoNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeHessianInfoNotAvailableException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeHessianInfoNotAvailableException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>Exception thrown when the time-out criterion is reached.</summary>
/// <remarks>Exception thrown when the time-out criterion is reached.</remarks>
[Serializable]
[Guid("0D5CA7D8-6574-4c7b-9B5F-320AA8375A3C")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeTimeOutException : CapeUserException, ECapeTimeOut
{

    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeTimeOutHR);
        MInterfaceName = "ECapeTimeOut";
        MName = "CapeTimeOutException";
    }

    /// <summary>Initializes a new instance of the CapeTimeOutException class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeTimeOutException()
    { }
    /// <summary>Initializes a new instance of the CapeTimeOutException class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeTimeOutException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeTimeOutException class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeTimeOutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeTimeOutException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeTimeOutException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>A wrapper class for COM-based exceptions.</summary>
/// <remarks><para>This class can be used when a COM-based CAPE-OPEN component returns a failure HRESULT.
/// A failure HRESULT indicates an error condition has occurred. This class is used by the
/// <see c = "COMExceptionHandler"/> to rethrow the COM-based
/// error condition as a .Net-based exception.</para>
/// <para>The CAPE-OPEN error handling process chose not to use the COM IErrorInfo API due to
/// limitation of the Visual Basic programming language at the time that the error
/// handling protocols were developed. Instead, the CAPE-OPEN error handling protocol 
/// requires that component in which the error occurs expose the appropriate error
/// interfaces. In practice, this typically means that all CAPE-OPEN objects
/// implement the <see c = "ECapeRoot">ECapeRoot</see>,
/// <see c = "ECapeUser">ECapeUser</see>, 
/// and sometimes the <see c = "ECapeUnknown">ECapeUnknown</see> error interfaces.</para>
/// <para>This class wraps the CAPE-OPEN object that threw the exception and creates the 
/// appropriate .Net exception so users can use the .Net exception handling protocols.</para></remarks>
/// <see c = "COMExceptionHandler">COMExceptionHandler</see> 
[Serializable]
[Guid("31CD55DE-AEFD-44ff-8BAB-F6252DD43F16")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class COMCapeOpenExceptionWrapper : CapeUserException
{

    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
    }

    /// <summary>Creates a new instance of the COMCapeOpenExceptionWrapper class.</summary>
    /// <remarks><para>Creates a .Net based exception wrapper for COM-based CAPE-OPEN componets to 
    /// enable users to utilize .Net structured exception handling.</para></remarks>
    /// <param name="message">The error message text from the COM-based component.</param>
    /// <param name="exceptionObject">The CAPE-OPEN object that raised the error.</param>
    /// <param name="HRESULT">The COM HResult value.</param>
    /// <param name="inner">An inner .Net-based exception obtained from the IErrorInfo
    /// object, if implemented or an accompanying .Net exception.</param>
    public COMCapeOpenExceptionWrapper(string message, object exceptionObject, int HRESULT, Exception inner)
        : base(message, inner)
    {
        HResult = HRESULT;
        if (exceptionObject is ECapeRoot)
        {
            MName = string.Concat("CAPE-OPEN Error: ", ((ECapeRoot)exceptionObject).Name);
        }

        if (exceptionObject is ECapeUser)
        {
            ECapeUser user = (ECapeUser)exceptionObject;
            MDescription = user.description;
            MInterfaceName = user.interfaceName;
            Source = user.scope;
            HelpLink = user.moreInfo;
        }
    }
}

/// <summary>Exception thrown when a requested theromdynamic property is not available.</summary>
/// <remarks>Exception thrown when a requested theromdynamic property is not available.</remarks>
[Serializable]
 [Guid("5BA36B8F-6187-4e5e-B263-0924365C9A81")]
 [ComVisible(true)]
 [ClassInterface(ClassInterfaceType.None)]
public class CapeThrmPropertyNotAvailableException : CapeUserException, ECapeThrmPropertyNotAvailable
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeThrmPropertyNotAvailableHR);
        MInterfaceName = "ECapePersistence";
        MName = "CapeThrmPropertyNotAvailable";
    }

    /// <summary>Initializes a new instance of the CapeThrmPropertyNotAvailable class. </summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeThrmPropertyNotAvailableException()
    { }
    /// <summary>Initializes a new instance of the CapeThrmPropertyNotAvailable class with a specified error message. </summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeThrmPropertyNotAvailableException(string message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeThrmPropertyNotAvailable class with serialized data.</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeThrmPropertyNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeThrmPropertyNotAvailable class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeThrmPropertyNotAvailableException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>A helper class for handling exceptions from COM-based CAPE-OPEN components.</summary>
/// <remarks><para>This class can be used when a COM-based CAPE-OPEN component returns a failure HRESULT.
/// A failure HRESULT indicates an error condition has occurred. The <see c ="ExceptionForHRESULT">ExceptionForHRESULT</see> 
/// ormats the .Net-bsed exception object and the COM-based CAPE-OPEN component to rethrow the COM-based
/// error condition as a .Net-based exception using the <see c = "COMCapeOpenExceptionWrapper">COMCapeOpenExceptionWrapper</see> 
/// wrapper class.</para>
/// <para>The CAPE-OPEN error handling process chose not to use the COM IErrorInfo API due to
/// limitation of the Visual Basic programming language at the time that the error
/// handling protocols were developed. Instead, the CAPE-OPEN error handling protocol 
/// requires that component in which the error occurs expose the appropriate error
/// interfaces. In practice, this typically means that all CAPE-OPEN objects
/// implement the <see c = "ECapeRoot">ECapeRoot</see>,
/// <see c = "ECapeUser">ECapeUser</see>, 
/// and sometimes the <see c = "ECapeUnknown">ECapeUnknown</see> error interfaces.</para></remarks>
/// <see c = "COMCapeOpenExceptionWrapper">COMCapeOpenExceptionWrapper</see> 
[ComVisible(false)]
public class COMExceptionHandler
{
    /// <summary>Creates and returns a new instance of the COMCapeOpenExceptionWrapper class.</summary>
    /// <remarks><para>Creates a .Net based exception wrapper for COM-based CAPE-OPEN componets to 
    /// enable users to utilize .Net structured exception handling. This method ormats 
    /// the .Net-bsed exception object and the COM-based CAPE-OPEN component to rethrow 
    /// the COM-based error condition as a .Net-based exception using the 
    /// <see c = "COMCapeOpenExceptionWrapper">COMCapeOpenExceptionWrapper</see> 
    /// wrapper class.</para></remarks>
    /// <returns>
    /// The COM-based object that returned the error HRESULT wrapper as the appropriate .Net-based exception.</returns>
    /// <param name="exceptionObject">The CAPE-OPEN object that raised the error.</param>
    /// <param name="inner">An inner .Net-based exception obtained from the IErrorInfo
    /// object, if implemented or an accompanying .Net exception.</param>
    /// <see c = "COMCapeOpenExceptionWrapper">COMCapeOpenExceptionWrapper</see> 
    public static Exception ExceptionForHRESULT(object exceptionObject, Exception inner)
    {
        int HRESULT = ((COMException)inner).ErrorCode;
        string message = "Exception thrown by COM PMC.";
        COMCapeOpenExceptionWrapper exception = new COMCapeOpenExceptionWrapper(message, exceptionObject, HRESULT, inner);
        switch (HRESULT)
        {
            case unchecked((int)0x80040501)://ECapeUnknownHR
                return new CapeUnknownException(message, exception);
            case unchecked((int)0x80040502)://ECapeDataHR
                return new CapeDataException(message, exception);
            case unchecked((int)0x80040503)://ECapeLicenceErrorHR = 0x80040503 ,
                return new CapeLicenceErrorException(message, exception);
            case unchecked((int)0x80040504)://ECapeBadCOParameterHR = 0x80040504 ,
                return new CapeBadCoParameter(message, exception);
            case unchecked((int)0x80040505)://ECapeBadArgumentHR = 0x80040505 ,
                return new CapeBadArgumentException(message, exception, ((ECapeBadArgument)exception).position);
            case unchecked((int)0x80040506)://ECapeInvalidArgumentHR = 0x80040506 ,
                return new CapeInvalidArgumentException(message, exception, ((ECapeBadArgument)exception).position);
            case unchecked((int)0x80040507)://ECapeOutOfBoundsHR = 0x80040507 
            {
                Exception p_Ex = exception;
                if (exception is ECapeBoundaries)
                {
                    ECapeBoundaries p_Ex1 = (ECapeBoundaries)(exception);
                    if (exception is ECapeBadArgument)
                    {
                        ECapeBadArgument p_Ex2 = (ECapeBadArgument)(exception);
                        return new CapeOutOfBoundsException(message, p_Ex, p_Ex2.position, p_Ex1.lowerBound, p_Ex1.upperBound, p_Ex1.value, p_Ex1.type);
                    }

                    return new CapeOutOfBoundsException(message, p_Ex, 0, p_Ex1.lowerBound, p_Ex1.upperBound, p_Ex1.value, p_Ex1.type);
                }

                return new CapeOutOfBoundsException(message, p_Ex, 0, 0.0, 0.0, 0.0, "");
            }
            case unchecked((int)(0x80040508))://ECapeImplementationHR = 0x80040508
                return new CapeImplementationException(message, exception);
            case unchecked((int)0x80040509)://ECapeNoImplHR = 0x80040509
                return new CapeNoImplException(message, exception);
            case unchecked((int)0x8004050A)://ECapeLimitedImplHR = 0x8004050A
                return new CapeLimitedImplException(message, exception);
            case unchecked((int)0x8004050B)://ECapeComputationHR = 0x8004050B 
                return new CapeComputationException(message, exception);
            case unchecked((int)0x8004050C)://ECapeOutOfResourcesHR = 0x8004050C
                return new CapeOutOfResourcesException(message, exception);
            case unchecked((int)0x8004050D)://ECapeNoMemoryHR = 0x8004050D
                return new CapeNoMemoryException(message, exception);
            case unchecked((int)0x8004050E)://ECapeTimeOutHR = 0x8004050E
                return new CapeTimeOutException(message, exception);
            case unchecked((int)0x8004050F)://ECapeFailedInitialisationHR = 0x8004050F 
                return new CapeFailedInitialisationException(message, exception);
            case unchecked((int)0x80040510)://ECapeSolvingErrorHR = 0x80040510
                return new CapeSolvingErrorException(message, exception);
            case unchecked((int)0x80040511)://ECapeBadInvOrderHR = 0x80040511 
            {

                if (exception is ECapeBadInvOrder)
                {
                    ECapeBadInvOrder p_Ex = (ECapeBadInvOrder)(exception);
                    return new CapeBadInvOrderException(message, exception, p_Ex.requestedOperation);
                }

                return new CapeBadInvOrderException(message, exception, "");
            }
            case unchecked((int)0x80040512)://ECapeInvalidOperationHR = 0x80040512
                return new CapeInvalidOperationException(message, exception);
            case unchecked((int)0x80040513)://ECapePersistenceHR = 0x80040513
                return new CapePersistenceException(message, exception);
            case unchecked((int)0x80040514)://ECapeIllegalAccessHR = 0x80040514
                return new CapeIllegalAccessException(message, exception);
            case unchecked((int)0x80040515)://ECapePersistenceNotFoundHR = 0x80040515
                return new CapePersistenceNotFoundException(message, exception, ((ECapePersistenceNotFound)exception).itemName);
            case unchecked((int)0x80040516)://ECapePersistenceSystemErrorHR = 0x80040516
                return new CapePersistenceSystemErrorException(message, exception);
            case unchecked((int)0x80040517)://ECapePersistenceOverflowHR = 0x80040517
                return new CapePersistenceOverflowException(message, exception);
            case unchecked((int)0x80040518)://ECapeOutsideSolverScopeHR = 0x80040518
                return new CapeSolvingErrorException(message, exception);
            case unchecked((int)0x80040519)://ECapeHessianInfoNotAvailableHR = 0x80040519
                return new CapeHessianInfoNotAvailableException(message, exception);
            case unchecked((int)0x80040520)://ECapeThrmPropertyNotAvailable = 0x80040520
                return new CapeThrmPropertyNotAvailableException(message, exception);
            default://ECapeDataHR
                return new CapeUnknownException(message, exception);
        }
    }
}