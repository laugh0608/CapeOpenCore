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
    public CapeInvalidOperationException()
    {
    }

    /// <summary>初始化一个新的 CapeInvalidOperationException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeInvalidOperationException(string message) : base(message)
    {
    }

    /// <summary>初始化 CapeInvalidOperationException 类的新的实例，使用序列化数据。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeInvalidOperationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>初始化一个新的 CapeInvalidOperationException 类实例，
    /// 该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeInvalidOperationException(string message, Exception inner) : base(message, inner)
    {
    }
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
/// <remarks>当然，这种错误也可能出现在 CO 的作用范围之外。在这种情况下，该错误不属于 CO 的错误处理范畴，而是特定于该平台。</remarks>
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
        HResult = (int)CapeErrorInterfaceHR.ECapeLicenceErrorHR;
        MInterfaceName = "ECapeLicenceError";
        MName = "CapeLicenceErrorException";
    }

    /// <summary>初始化 CapeLicenceErrorException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeLicenceErrorException()
    {
    }

    /// <summary>初始化一个新的 CapeLicenceErrorException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeLicenceErrorException(string message) : base(message)
    {
    }

    /// <summary>初始化 CapeLicenceErrorException 类的新的实例，并使用序列化数据。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeLicenceErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化一个新的 CapeLicenceErrorException 类实例，该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeLicenceErrorException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>实现的限制已被违反。</summary>
/// <remarks><para>一项操作可以部分实现，例如，一个“属性包”可以实现 TP 闪蒸，但不支持 PH 闪蒸。如果调用方请求执行 PH 闪蒸操作，
/// 则该错误表明，虽然支持某些闪蒸计算，但不支持请求的操作。</para>
/// <para>该工厂只能创建单个实例（因为该组件是评估副本），当调用者请求创建第二个实例时，这个错误表明该实现存在限制。</para></remarks>
[Serializable]
[ComVisible(true)]
[Guid("5E6B74A2-D603-4e90-A92F-608E3F1CD39D")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeLimitedImplException : CapeImplementationException, ECapeLimitedImpl
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeLimitedImplHR;
        MInterfaceName = "ECapeLimitedImpl";
        MName = "CapeLimitedImplException";
    }

    /// <summary>初始化 CapeLimitedImplException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeLimitedImplException()
    {
    }

    /// <summary>初始化一个新的 CapeLimitedImplException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeLimitedImplException(string message) : base(message)
    {
    }

    /// <summary>初始化 CapeLimitedImplException 类的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeLimitedImplException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化一个新的 CapeLimitedImplException 类实例，并指定错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeLimitedImplException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>一个异常类，用于指示当前对象尚未实现所请求的操作。</summary>
/// <remarks>即使由于与 CO 标准兼容，可以调用该操作，但该操作“未被”实现。也就是说，该操作存在，但当前实现并不支持它。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("1D2488A6-C428-4e38-AFA6-04F2107172DA")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeNoImplException : CapeImplementationException, ECapeNoImpl
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeNoImplHR;
        MInterfaceName = "ECapeNoImpl";
        MName = "CapeNoImplException";
    }

    /// <summary>初始化 CapeNoImplException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeNoImplException()
    {
    }

    /// <summary>初始化一个新的 CapeNoImplException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeNoImplException(string message) : base(message)
    {
    }

    /// <summary>初始化 CapeNoImplException 类的新的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeNoImplException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapeNoImplException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeNoImplException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>一个异常类，用于指示此操作所需的资源不可用。</summary>
/// <remarks>执行该操作所需的物理资源已超出限制。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("42B785A7-2EDD-4808-AC43-9E6E96373616")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeOutOfResourcesException : CapeUserException, ECapeOutOfResources, ECapeComputation
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeOutOfResourcesHR;
        MInterfaceName = "ECapeOutOfResources";
        MName = "CapeOutOfResourcesException";
    }

    /// <summary>初始化 CapeOutOfResourcesException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeOutOfResourcesException()
    {
    }

    /// <summary>初始化一个新的 CapeOutOfResourcesException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeOutOfResourcesException(string message) : base(message)
    {
    }

    /// <summary>初始化 CapeOutOfResourcesException 类的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeOutOfResourcesException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapeOutOfResourcesException 类的一个新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeOutOfResourcesException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>一个异常类，用于指示执行此操作所需的内存不可用。</summary>
/// <remarks>执行该操作所需的物理内存已超出限制。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("1056A260-A996-4a1e-8BAE-9476D643282B")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeNoMemoryException : CapeUserException, ECapeNoMemory
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeNoMemoryHR;
        MInterfaceName = "ECapeNoMemory";
        MName = "CapeNoMemoryException";
    }

    /// <summary>初始化 CapeNoMemoryException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeNoMemoryException()
    {
    }

    /// <summary>初始化一个新的 CapeNoMemoryException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeNoMemoryException(string message) : base(message)
    {
    }

    /// <summary>初始化 CapeNoMemoryException 类的新的实例，使用序列化数据。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeNoMemoryException(SerializationInfo info, StreamingContext context)
    {
    }

    /// <summary>初始化 CapeNoMemoryException 类的一个新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeNoMemoryException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>一个异常类，用于指示与持久化相关的错误已发生。</summary>
/// <remarks>与持久化相关的错误层次结构的基类。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("3237C6F8-3D46-47ee-B25F-52485A5253D8")]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceException : CapeUserException, ECapePersistence
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapePersistenceHR;
        MInterfaceName = "ECapePersistence";
        MName = "CapePersistenceException";
    }

    /// <summary>初始化 CapePersistenceException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapePersistenceException()
    {
    }

    /// <summary>初始化 CapePersistenceException 类的新的实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapePersistenceException(string message) : base(message)
    {
    }

    /// <summary>初始化 CapePersistenceException 类的新的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapePersistenceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapePersistenceException 类的一个新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    ///或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapePersistenceException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>一个异常类，用于表示未找到持久化对象。</summary>
/// <remarks>请求的对象、表或其他持久化系统中的内容未找到。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("271B9E29-637E-4eb0-9588-8A53203A3959")]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceNotFoundException : CapePersistenceException, ECapePersistenceNotFound
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    /// <param name="pItemName">未找到的项目名称，该项目是导致此异常的原因。</param>
    protected void Initialize(string pItemName)
    {
        itemName = pItemName;
        HResult = (int)CapeErrorInterfaceHR.ECapePersistenceNotFoundHR;
        MInterfaceName = "ECapePersistenceNotFound";
        MName = "CapePersistenceNotFoundException";
    }

    /// <summary>初始化 CapePersistenceNotFoundException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    /// <param name="itemName">未找到的项目名称，该项目是导致此异常的原因。</param>
    public CapePersistenceNotFoundException(string itemName)
    {
        Initialize(itemName);
    }

    /// <summary>初始化一个新的 CapePersistenceNotFoundException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    /// <param name="itemName">未找到的项目名称，该项目是导致此异常的原因。</param>
    public CapePersistenceNotFoundException(string message, string itemName) : base(message)
    {
        Initialize(itemName);
    }

    /// <summary>初始化 CapePersistenceNotFoundException 类的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    /// <param name="itemName">未找到的项目名称，该项目是导致此异常的原因。</param>
    public CapePersistenceNotFoundException(SerializationInfo info, StreamingContext context, string itemName) :
        base(info, context)
    {
        Initialize(itemName);
    }

    /// <summary>初始化一个新的 CapePersistenceNotFoundException 类实例，该实例带有指定的错误消息以及导致此异常的内部异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    /// <param name="itemName">未找到的项目名称，该项目是导致此异常的原因。</param>
    public CapePersistenceNotFoundException(string message, Exception inner, string itemName) : base(message, inner)
    {
        Initialize(itemName);
    }

    /// <summary>未找到的项目名称，该项目是导致此异常的原因。</summary>
    /// <remarks>未找到的项目名称，该项目是导致此异常的原因。</remarks>
    /// <value>未找到的项目名称，该项目是导致此异常的原因。</value>
    public string itemName { get; private set; }
}

/// <summary>一个异常类，用于指示内部持久化系统发生溢出。</summary>
/// <remarks>在持久化过程中，内部持久化系统发生了溢出。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("A119DE0B-C11E-4a14-BA5E-9A2D20B15578")]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceOverflowException : CapeUserException, ECapePersistenceOverflow, ECapePersistence
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapePersistenceOverflowHR;
        MInterfaceName = "ECapePersistenceOverflow";
        MName = "CapePersistenceOverflowException";
    }

    /// <summary>初始化 CapePersistenceOverflowException 类的新的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapePersistenceOverflowException()
    {
    }

    /// <summary>初始化一个新的 CapePersistenceOverflowException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapePersistenceOverflowException(string message)
    {
    }

    /// <summary>初始化 CapePersistenceOverflowException 类的新的实例，使用序列化数据。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapePersistenceOverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapePersistenceOverflowException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapePersistenceOverflowException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>一个异常类，用于指示持久化系统中发生了严重错误。</summary>
/// <remarks>在持久化过程中，持久化系统中发生了严重错误。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("85CB2D40-48F6-4c33-BF0C-79CB00684440")]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceSystemErrorException : CapePersistenceException, ECapePersistenceSystemError
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapePersistenceSystemErrorHR;
        MInterfaceName = "ECapePersistenceSystemError";
        MName = "CapePersistenceSystemErrorException";
    }

    /// <summary>初始化 CapePersistenceSystemErrorException 类的实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapePersistenceSystemErrorException()
    {
    }

    /// <summary>初始化一个新的 CapePersistenceSystemErrorException 类实例，并指定错误消息。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapePersistenceSystemErrorException(string message) : base(message)
    {
    }

    /// <summary>初始化 CapePersistenceSystemErrorException 类的实例，并使用序列化数据进行初始化。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapePersistenceSystemErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapePersistenceSystemErrorException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapePersistenceSystemErrorException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>对持久化系统中的某项资源的访问未获授权。</summary>
/// <remarks>当对持久化系统中的某项资源的访问未获得授权时，将抛出此异常。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("45843244-ECC9-495d-ADC3-BF9980A083EB")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeIllegalAccessException : CapePersistenceException, ECapeIllegalAccess
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeIllegalAccessHR;
        MInterfaceName = "ECapeIllegalAccess";
        MName = "CapeIllegalAccessException";
    }

    /// <summary>初始化 CapeIllegalAccessException 类的一个新实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeIllegalAccessException()
    {
    }

    /// <summary>使用指定的错误消息初始化 CapeIllegalAccessException 类的实例。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeIllegalAccessException(string message) : base(message)
    {
    }

    /// <summary>使用序列化数据初始化 CapeIllegalAccessException 类的实例。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeIllegalAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapeIllegalAccessException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeIllegalAccessException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>一个异常类，用于表示数值算法因任何原因失败。</summary>
/// <remarks>表示数值算法因任何原因失败。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("F617AFEA-0EEE-4395-8C82-288BF8F2A136")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeSolvingErrorException : CapeComputationException, ECapeSolvingError
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeSolvingErrorHR;
        MInterfaceName = "ECapeSolvingError";
        MName = "CapeSolvingErrorException";
    }

    /// <summary>初始化 CapeSolvingErrorException 类的一个新实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeSolvingErrorException()
    {
    }

    /// <summary>使用指定的错误消息初始化 CapeSolvingErrorException 类的实例。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeSolvingErrorException(string message) : base(message)
    {
    }

    /// <summary>使用序列化数据初始化 CapeSolvingErrorException 类的实例。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeSolvingErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapeSolvingErrorException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeSolvingErrorException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>当 MINLP 问题的海森矩阵不可用时抛出异常。</summary>
/// <remarks>Exception thrown when the Hessian for the MINLP problem is not available.</remarks>
[Serializable]
[ComVisible(true)]
[Guid("3044EA08-F054-4315-B67B-4E3CD2CF0B1E")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeHessianInfoNotAvailableException : CapeSolvingErrorException, ECapeHessianInfoNotAvailable
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeHessianInfoNotAvailableHR;
        MInterfaceName = "ECapeHessianInfoNotAvailable";
        MName = "CapeHessianInfoNotAvailableHR";
    }

    /// <summary>初始化 CapeHessianInfoNotAvailableException 类的一个新实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeHessianInfoNotAvailableException()
    {
    }

    /// <summary>使用指定的错误消息初始化 CapeHessianInfoNotAvailableException 类的实例。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeHessianInfoNotAvailableException(string message) : base(message)
    {
    }

    /// <summary>使用序列化数据初始化 CapeHessianInfoNotAvailableException 类的实例。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeHessianInfoNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapeHessianInfoNotAvailableException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeHessianInfoNotAvailableException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>当超时条件满足时抛出异常。</summary>
/// <remarks>当超时条件满足时抛出异常。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("0D5CA7D8-6574-4c7b-9B5F-320AA8375A3C")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeTimeOutException : CapeUserException, ECapeTimeOut
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeTimeOutHR;
        MInterfaceName = "ECapeTimeOut";
        MName = "CapeTimeOutException";
    }

    /// <summary>初始化 CapeTimeOutException 类的一个新实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeTimeOutException()
    {
    }

    /// <summary>使用指定的错误消息初始化 CapeTimeOutException 类的实例。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeTimeOutException(string message) : base(message)
    {
    }

    /// <summary>使用序列化数据初始化 CapeTimeOutException 类的实例。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeTimeOutException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapeTimeOutException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeTimeOutException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>用于封装基于 COM 的异常的包装类。</summary>
/// <remarks><para>当基于 COM 的 CAPE-OPEN 组件返回失败 HRESULT 时，可以调用此类。失败 HRESULT 表示已发生错误情况。
/// 此类由 <see href="COMExceptionHandler"/> 使用，以将基于 COM 的错误情况重新抛为基于 .Net 的异常。</para>
/// <para>由于开发错误处理协议时 Visual Basic 程序设计语言的限制，CAPE-OPEN 错误处理过程选择不使用 COM IErrorInfo API。
/// 相反，CAPE-OPEN 错误处理协议要求发生错误的组件公开适当的错误接口。在实践中，这通常意味着所有 CAPE-OPEN 对象都实现
/// <see href="ECapeRoot"/>、<see href="ECapeUser"/>，有时也实现 <see href="ECapeUnknown"/> 错误接口。</para>
/// <para>这个类封装了抛出异常的 CAPE-OPEN 对象，并创建相应的 .Net 异常，以便用户能够使用 .Net 异常处理协议。</para></remarks>
/// <see href="COMExceptionHandler">COMExceptionHandler</see>
[Serializable]
[ComVisible(true)]
[Guid("31CD55DE-AEFD-44ff-8BAB-F6252DD43F16")]
[ClassInterface(ClassInterfaceType.None)]
public class ComCapeOpenExceptionWrapper : CapeUserException
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
    }

    /// <summary>创建 ComCapeOpenExceptionWrapper 类的实例。</summary>
    /// <remarks>为基于 COM 的 CAPE-OPEN 组件创建一个基于 .NET 的异常包装器，以便用户能够利用 .NET 结构化异常处理。</remarks>
    /// <param name="message">来自基于 COM 的组件的错误消息文本。</param>
    /// <param name="exceptionObject">引发错误的 CAPE-OPEN 对象。</param>
    /// <param name="hresult">COM 的 HResult 值。</param>
    /// <param name="inner">一个基于 .Net 的内部异常，该异常来自 IErrorInfo 对象，如果实现了此接口，或者是一个与之关联的 .Net 异常。</param>
    public ComCapeOpenExceptionWrapper(string message, object exceptionObject, int hresult, Exception inner)
        : base(message, inner)
    {
        HResult = hresult;
        if (exceptionObject is ECapeRoot pRoot)
        {
            MName = string.Concat("CAPE-OPEN Error: ", pRoot.Name);
        }

        if (exceptionObject is not ECapeUser pUser) return;
        MDescription = pUser.description;
        MInterfaceName = pUser.interfaceName;
        Source = pUser.scope;
        HelpLink = pUser.moreInfo;
    }
}

/// <summary>当请求的热力学属性不可用时抛出异常。</summary>
/// <remarks>当请求的热力学属性不可用时抛出异常。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("5BA36B8F-6187-4e5e-B263-0924365C9A81")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeThermoPropertyNotAvailableException : CapeUserException, ECapeThrmPropertyNotAvailable
{
    /// <summary>初始化此异常的描述、接口名称和名称字段。</summary>
    /// <remarks>设置 HResult、接口名称和异常名称的值。</remarks>
    protected override void Initialize()
    {
        HResult = (int)CapeErrorInterfaceHR.ECapeThrmPropertyNotAvailableHR;
        MInterfaceName = "ECapePersistence";
        MName = "CapeThermoPropertyNotAvailable";
    }

    /// <summary>初始化 CapeThermoPropertyNotAvailableException 类的一个新实例。</summary>
    /// <remarks>这个构造函数会将新实例的“Message”属性初始化为系统提供的描述错误的信息，
    /// 例如“发生了一个应用程序错误”。这条信息会考虑到当前的系统文化。</remarks>
    public CapeThermoPropertyNotAvailableException()
    {
    }

    /// <summary>使用指定的错误消息初始化 CapeThermoPropertyNotAvailableException 类的实例。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>此消息考虑了当前的系统文化。</para></remarks>
    /// <param name="message">一条描述错误的消息。</param>
    public CapeThermoPropertyNotAvailableException(string message) : base(message)
    {
    }

    /// <summary>使用序列化数据初始化 CapeThermoPropertyNotAvailableException 类的实例。</summary>
    /// <remarks>这个构造函数在反序列化过程中被调用，用于重构通过流传输的异常对象。有关详细信息，请参阅 XML 和 SOAP 序列化。</remarks>
    /// <param name="info">存储序列化对象数据的对象。</param>
    /// <param name="context">有关源或目的地的上下文信息。</param>
    public CapeThermoPropertyNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>初始化 CapeThermoPropertyNotAvailableException 类的新实例，同时提供指定的错误信息，以及引发此异常的内在异常的引用。</summary>
    /// <remarks><para>消息参数的内容旨在供人类理解。调用此构造函数的一方必须确保该字符串已针对当前系统文化进行了本地化。</para>
    /// <para>由于先前的异常直接导致的异常，应在内层异常属性中包含对先前异常的引用。内层异常属性返回传递给构造函数的相同值，
    /// 或者如果内层异常属性没有向构造函数提供内层异常值，则返回 null 引用。</para></remarks>
    /// <param name="message">错误消息字符串。</param>
    /// <param name="inner">内部异常引用。</param>
    public CapeThermoPropertyNotAvailableException(string message, Exception inner) : base(message, inner)
    {
    }
}

/// <summary>用于处理基于 COM 的 CAPE-OPEN 组件异常的辅助类。</summary>
/// <remarks><para>当基于 COM 的 CAPE-OPEN 组件返回失败 HRESULT 时，可以调用此类。失败 HRESULT 表示发生了错误情况。
/// <see c="ExceptionForHRESULT"/> 格式化基于 .NET 的异常对象和基于 COM 的 CAPE-OPEN 部件，以便使用
/// <see href="COMCapeOpenExceptionWrapper"/> 包装类，将基于 COM 的错误情况重新抛掷为基于 .NET 的异常。</para>
/// <para>由于开发错误处理协议时 Visual Basic 程序设计语言的限制，CAPE-OPEN 错误处理过程选择不使用 COM IErrorInfo API。
/// 相反，CAPE-OPEN 错误处理协议要求发生错误的组件公开适当的错误接口。在实践中，这通常意味着所有 CAPE-OPEN 对象都实现
/// <see href="ECapeRoot"/>、<see href="ECapeUser"/>，有时也实现 <see href="ECapeUnknown"/> 错误接口。</para></remarks>
/// <see href="COMCapeOpenExceptionWrapper">COMCapeOpenExceptionWrapper</see> 
[ComVisible(false)]
public class COMExceptionHandler
{
    /// <summary>创建并返回 COMCapeOpenExceptionWrapper 类的全新实例。</summary>
    /// <remarks>为基于 COM 的 CAPE-OPEN 组件创建一个基于 .NET 的异常包装器，以使用户能够利用 .NET 结构化异常处理。
    /// 该方法将基于 .NET 的异常对象与基于 COM 的 CAPE-OPEN 部件格式化，以便使用 <see href="COMCapeOpenExceptionWrapper"/> 
    /// 包装类将基于 COM 的错误条件重新包装为基于 .NET 的异常。</remarks>
    /// <returns>基于 COM 的对象将错误 HRESULT 封装为相应的 .NET 异常。</returns>
    /// <param name="exceptionObject">引发错误的 CAPE-OPEN 对象。</param>
    /// <param name="inner">一个基于 .Net 的内部异常，该异常来自 IErrorInfo 对象，如果实现了此接口，或者是一个与之关联的 .Net 异常。</param>
    /// <see href="COMCapeOpenExceptionWrapper">COMCapeOpenExceptionWrapper</see> 
    public static Exception ExceptionForHRESULT(object exceptionObject, Exception inner)
    {
        var pHresult = ((COMException)inner).ErrorCode;
        const string message = "Exception thrown by COM PMC.";
        var exception = new ComCapeOpenExceptionWrapper(message, exceptionObject, pHresult, inner);
        switch (pHresult)
        {
            case unchecked((int)0x80040501): //ECapeUnknownHR
                return new CapeUnknownException(message, exception);
            case unchecked((int)0x80040502): //ECapeDataHR
                return new CapeDataException(message, exception);
            case unchecked((int)0x80040503): //ECapeLicenceErrorHR = 0x80040503 ,
                return new CapeLicenceErrorException(message, exception);
            case unchecked((int)0x80040504): //ECapeBadCOParameterHR = 0x80040504 ,
                return new CapeBadCoParameter(message, exception);
            case unchecked((int)0x80040505): //ECapeBadArgumentHR = 0x80040505 ,
                return new CapeBadArgumentException(message, exception, ((ECapeBadArgument)exception).position);
            case unchecked((int)0x80040506): //ECapeInvalidArgumentHR = 0x80040506 ,
                return new CapeInvalidArgumentException(message, exception, ((ECapeBadArgument)exception).position);
            case unchecked((int)0x80040507): //ECapeOutOfBoundsHR = 0x80040507 
            {
                Exception pEx = exception;
                if (exception is not ECapeBoundaries pBoundaries)
                    return new CapeOutOfBoundsException(message, pEx, 0, 0.0, 0.0, 0.0, "");
                if (pBoundaries is not ECapeBadArgument tEx)
                    return new CapeOutOfBoundsException(message, pEx, 0, pBoundaries.lowerBound, pBoundaries.upperBound,
                        pBoundaries.value, pBoundaries.type);
                return new CapeOutOfBoundsException(message, pEx, tEx.position, pBoundaries.lowerBound,
                    pBoundaries.upperBound, pBoundaries.value, pBoundaries.type);

            }
            case unchecked((int)(0x80040508)): //ECapeImplementationHR = 0x80040508
                return new CapeImplementationException(message, exception);
            case unchecked((int)0x80040509): //ECapeNoImplHR = 0x80040509
                return new CapeNoImplException(message, exception);
            case unchecked((int)0x8004050A): //ECapeLimitedImplHR = 0x8004050A
                return new CapeLimitedImplException(message, exception);
            case unchecked((int)0x8004050B): //ECapeComputationHR = 0x8004050B 
                return new CapeComputationException(message, exception);
            case unchecked((int)0x8004050C): //ECapeOutOfResourcesHR = 0x8004050C
                return new CapeOutOfResourcesException(message, exception);
            case unchecked((int)0x8004050D): //ECapeNoMemoryHR = 0x8004050D
                return new CapeNoMemoryException(message, exception);
            case unchecked((int)0x8004050E): //ECapeTimeOutHR = 0x8004050E
                return new CapeTimeOutException(message, exception);
            case unchecked((int)0x8004050F): //ECapeFailedInitialisationHR = 0x8004050F 
                return new CapeFailedInitialisationException(message, exception);
            case unchecked((int)0x80040510): //ECapeSolvingErrorHR = 0x80040510
                return new CapeSolvingErrorException(message, exception);
            case unchecked((int)0x80040511): //ECapeBadInvOrderHR = 0x80040511 
            {
                return exception is not ECapeBadInvOrder pOrder 
                    ? new CapeBadInvOrderException(message, exception, "") 
                    : new CapeBadInvOrderException(message, exception, pOrder.requestedOperation);
            }
            case unchecked((int)0x80040512): //ECapeInvalidOperationHR = 0x80040512
                return new CapeInvalidOperationException(message, exception);
            case unchecked((int)0x80040513): //ECapePersistenceHR = 0x80040513
                return new CapePersistenceException(message, exception);
            case unchecked((int)0x80040514): //ECapeIllegalAccessHR = 0x80040514
                return new CapeIllegalAccessException(message, exception);
            case unchecked((int)0x80040515): //ECapePersistenceNotFoundHR = 0x80040515
                return new CapePersistenceNotFoundException(message, exception,
                    ((ECapePersistenceNotFound)exception).itemName);
            case unchecked((int)0x80040516): //ECapePersistenceSystemErrorHR = 0x80040516
                return new CapePersistenceSystemErrorException(message, exception);
            case unchecked((int)0x80040517): //ECapePersistenceOverflowHR = 0x80040517
                return new CapePersistenceOverflowException(message, exception);
            case unchecked((int)0x80040518): //ECapeOutsideSolverScopeHR = 0x80040518
                return new CapeSolvingErrorException(message, exception);
            case unchecked((int)0x80040519): //ECapeHessianInfoNotAvailableHR = 0x80040519
                return new CapeHessianInfoNotAvailableException(message, exception);
            case unchecked((int)0x80040520): //ECapeThrmPropertyNotAvailable = 0x80040520
                return new CapeThermoPropertyNotAvailableException(message, exception);
            default: //ECapeDataHR
                return new CapeUnknownException(message, exception);
        }
    }
}