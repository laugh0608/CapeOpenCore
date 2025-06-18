/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.18
 */

using System;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>This operation is not valid in the current context.</summary>
/// <remarks>This exception is thrown when an operation is attempted that is not valid in the current context.</remarks>
[Serializable]
[Guid("C0B943FE-FB8F-46b6-A622-54D30027D18B")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeInvalidOperationException : CapeComputationException, ECapeInvalidOperation
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeInvalidOperationHR);
        m_interfaceName = "ECapeInvalidOperation";
        m_name = "CapeInvalidOperationException";
    }
    /// <summary>Initializes a new instance of the CapeInvalidOperationException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeInvalidOperationException() : base() { }
    /// <summary>Initializes a new instance of the CapeInvalidOperationException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeInvalidOperationException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeInvalidOperationException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeInvalidOperationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeInvalidOperationException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeInvalidOperationException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>The necessary pre-requisite operation has not been called prior to the operation request.</summary>
/// <remarks>The specified prerequiste operation must be called prior to the operation throwing this exception.</remarks>
[Serializable]
[Guid("07EAD8B4-4130-4ca6-94C1-E8EC4E9B23CB")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeBadInvOrderException : CapeComputationException, ECapeBadInvOrder
{
    private String m_requestedOperation;

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeBadInvOrderHR);
        m_interfaceName = "ECapeBadInvOrder";
        m_name = "CapeBadInvOrderException";
    }


    /// <summary>Initializes a new instance of the CapeBadInvOrderException class with a specified error message and the name of the operation raising the exception. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "operation">The necessary prerequisite operation.</param>
    public CapeBadInvOrderException(String operation)
        : base()
    {
        m_requestedOperation = operation;
    }
    /// <summary>Initializes a new instance of the CapeBadInvOrderException class with a specified error message and the name of the operation raising the exception. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    /// <param name = "operation">The necessary prerequisite operation.</param>
    public CapeBadInvOrderException(String message, String operation)
        : base(message)
    {
        m_requestedOperation = operation;
    }
    /// <summary>Initializes a new instance of the CapeBadInvOrderException class with serialized data and the name of the operation raising the exception.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    /// <param name = "operation">The necessary prerequisite operation.</param>
    public CapeBadInvOrderException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, String operation)
        : base(info, context)
    {
        m_requestedOperation = operation;
    }
    /// <summary>Initializes a new instance of the CapeBadInvOrderException class with a specified error message, a erence to the inner exception, and the name of the operation raising the exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    /// <param name = "operation">The necessary prerequisite operation.</param>
    public CapeBadInvOrderException(String message, Exception inner, String operation)
        : base(message, inner)
    {
        m_requestedOperation = operation;
    }

    /// <summary>The necessary prerequisite operation.</summary>
    /// <remarks><para>The prerquisite operation must be called prior to calling the current operation.</para></remarks>
    /// <value>The name of the necessary prerequisite operation.
    /// </value>
    public String requestedOperation
    {
        get
        {
            return m_requestedOperation;
        }
    }
};

/// <summary>An operation can not be completed because the licence agreement is not respected.</summary>
/// <remarks>Of course, this type of error could also appear outside the CO scope. In this case, 
/// the error does not belong to the CO error handling. It is specific to the platform.</remarks>
[Serializable]
[Guid("CF4C55E9-6B0A-4248-9A33-B8134EA393F6")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeLicenceErrorException : CapeDataException, ECapeLicenceError
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeLicenceErrorHR);
        m_interfaceName = "ECapeLicenceError";
        m_name = "CapeLicenceErrorException";
    }

    /// <summary>Initializes a new instance of the CapeLicenceErrorException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeLicenceErrorException() : base() { }
    /// <summary>Initializes a new instance of the CapeLicenceErrorException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeLicenceErrorException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeLicenceErrorException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeLicenceErrorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeLicenceErrorException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeLicenceErrorException(String message, Exception inner) : base(message, inner) { }
};

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
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeLimitedImplHR);
        m_interfaceName = "ECapeLimitedImpl";
        m_name = "CapeLimitedImplException";
    }

    /// <summary>Initializes a new instance of the CapeLimitedImplException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeLimitedImplException() : base() { }
    /// <summary>Initializes a new instance of the CapeLimitedImplException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeLimitedImplException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeLimitedImplException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeLimitedImplException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeLimitedImplException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeLimitedImplException(String message, Exception inner) : base(message, inner) { }
};

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
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeNoImplHR);
        m_interfaceName = "ECapeNoImpl";
        m_name = "CapeNoImplException";
    }

    /// <summary>Initializes a new instance of the CapeNoImplException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeNoImplException() : base() { }
    /// <summary>Initializes a new instance of the CapeNoImplException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeNoImplException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeNoImplException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeNoImplException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeNoImplException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeNoImplException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>An exception class that indicates that the resources required by this operation are not available.</summary>
/// <remarks>The physical resources necessary to the execution of the operation are out of limits.</remarks>
[Serializable]
[Guid("42B785A7-2EDD-4808-AC43-9E6E96373616")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeOutOfResourcesException : CapeUserException, ECapeOutOfResources, ECapeComputation
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeOutOfResourcesHR);
        m_interfaceName = "ECapeOutOfResources";
        m_name = "CapeOutOfResourcesException";
    }

    /// <summary>Initializes a new instance of the CapeOutOfResourcesException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeOutOfResourcesException()
        : base()
    {
    }
    /// <summary>Initializes a new instance of the CapeOutOfResourcesException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeOutOfResourcesException(String message)
        : base(message)
    {
    }
    /// <summary>Initializes a new instance of the CapeOutOfResourcesException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeOutOfResourcesException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }
    /// <summary>Initializes a new instance of the CapeOutOfResourcesException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeOutOfResourcesException(String message, Exception inner)
        : base(message, inner)
    {
    }
};

/// <summary>An exception class that indicates that the memory required for this operation is not available.</summary>
/// <remarks>The physical memory necessary to the execution of the operation is out of limit.</remarks>
[Serializable]
[Guid("1056A260-A996-4a1e-8BAE-9476D643282B")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeNoMemoryException : CapeUserException, ECapeNoMemory
{

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeNoMemoryHR);
        m_interfaceName = "ECapeNoMemory";
        m_name = "CapeNoMemoryException";
    }

    /// <summary>Initializes a new instance of the CapeNoMemoryException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeNoMemoryException() : base() { }
    /// <summary>Initializes a new instance of the CapeNoMemoryException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeNoMemoryException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeNoMemoryException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeNoMemoryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base() { }
    /// <summary>Initializes a new instance of the CapeNoMemoryException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeNoMemoryException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>An exception class that indicates that the a persistence-related error has occurred.</summary>
/// <remarks>The base class of the errors hierarchy related to the persistence.</remarks>
[Serializable]
[Guid("3237C6F8-3D46-47ee-B25F-52485A5253D8")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceException : CapeUserException, ECapePersistence
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapePersistenceHR);
        m_interfaceName = "ECapePersistence";
        m_name = "CapePersistenceException";
    }

    /// <summary>Initializes a new instance of the CapePersistenceException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapePersistenceException() : base() { }
    /// <summary>Initializes a new instance of the CapePersistenceException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapePersistenceException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapePersistenceException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapePersistenceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapePersistenceException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapePersistenceException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>An exception class that indicates that the persistence was not found.</summary>
/// <remarks>The requested object, table, or something else within the persistence system was not found.</remarks>
[Serializable]
[Guid("271B9E29-637E-4eb0-9588-8A53203A3959")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceNotFoundException : CapePersistenceException, ECapePersistenceNotFound
{
    private String m_ItemName;

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    /// <param name = "itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    protected void Initialize(String itemName)
    {
        m_ItemName = itemName;
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapePersistenceNotFoundHR);
        m_interfaceName = "ECapePersistenceNotFound";
        m_name = "CapePersistenceNotFoundException";
    }

    /// <summary>Initializes a new instance of the CapePersistenceNotFoundException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    /// <param name = "itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    public CapePersistenceNotFoundException(String itemName) : base() { Initialize(itemName); }
    /// <summary>Initializes a new instance of the CapePersistenceNotFoundException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    /// <param name = "itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    public CapePersistenceNotFoundException(String message, String itemName) : base(message) { Initialize(itemName); }
    /// <summary>Initializes a new instance of the CapePersistenceNotFoundException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    /// <param name = "itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    public CapePersistenceNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, String itemName) : base(info, context) { Initialize(itemName); }
    /// <summary>Initializes a new instance of the CapePersistenceNotFoundException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    /// <param name = "itemName">
    /// Name of the item that was not found that is the cause of this exception. 
    /// </param>
    public CapePersistenceNotFoundException(String message, Exception inner, String itemName) : base(message, inner) { Initialize(itemName); }

    /// <summary>Name of the item that was not found that is the cause of this exception. </summary>
    /// <remarks>Name of the item that was not found that is the cause of this exception. </remarks>
    /// <value>
    /// Name of the item that was not found that is the cause of this exception. 
    /// </value>
    public String itemName
    {
        get
        {
            return m_ItemName;
        }
    }

};

/// <summary>An exception class that indicates an overflow of internal persistence system.</summary>
/// <remarks>During the persistence process, an overflow of internal persistence system occurred.</remarks>
[Serializable]
[Guid("A119DE0B-C11E-4a14-BA5E-9A2D20B15578")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceOverflowException : 
    CapeUserException, ECapePersistenceOverflow, ECapePersistence
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapePersistenceOverflowHR);
        m_interfaceName = "ECapePersistenceOverflow";
        m_name = "CapePersistenceOverflowException";
    }
    /// <summary>Initializes a new instance of the CapePersistenceOverflowException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapePersistenceOverflowException()
        : base()
    {
    }
    /// <summary>Initializes a new instance of the CapePersistenceOverflowException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapePersistenceOverflowException(String message)
        : base()
    {
    }
    /// <summary>Initializes a new instance of the CapePersistenceOverflowException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapePersistenceOverflowException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapePersistenceOverflowException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapePersistenceOverflowException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>An exception class that indicates a severe error occurred within the persistence system.</summary>
/// <remarks>During the persistence process, a severe error occurred within the persistence system.</remarks>
[Serializable]
[Guid("85CB2D40-48F6-4c33-BF0C-79CB00684440")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapePersistenceSystemErrorException : CapePersistenceException, ECapePersistenceSystemError
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapePersistenceSystemErrorHR);
        m_interfaceName = "ECapePersistenceSystemError";
        m_name = "CapePersistenceSystemErrorException";
    }

    /// <summary>Initializes a new instance of the CapePersistenceSystemErrorException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapePersistenceSystemErrorException() : base() { }
    /// <summary>Initializes a new instance of the CapePersistenceSystemErrorException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapePersistenceSystemErrorException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapePersistenceSystemErrorException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapePersistenceSystemErrorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapePersistenceSystemErrorException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapePersistenceSystemErrorException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>The access to something within the persistence system is not authorised.</summary>
/// <remarks>This exception is thrown when the access to something within the persistence system is not authorised.</remarks>
[Serializable]
[Guid("45843244-ECC9-495d-ADC3-BF9980A083EB")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeIllegalAccessException : CapePersistenceException, ECapeIllegalAccess
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeIllegalAccessHR);
        m_interfaceName = "ECapeIllegalAccess";
        m_name = "CapeIllegalAccessException";
    }

    /// <summary>Initializes a new instance of the CapeIllegalAccessException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeIllegalAccessException() : base() { }
    /// <summary>Initializes a new instance of the CapeIllegalAccessException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeIllegalAccessException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeIllegalAccessException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeIllegalAccessException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeIllegalAccessException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeIllegalAccessException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>An exception class that indicates a numerical algorithm failed for any reason.</summary>
/// <remarks>Indicates that a numerical algorithm failed for any reason.</remarks>
[Serializable]
[Guid("F617AFEA-0EEE-4395-8C82-288BF8F2A136")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeSolvingErrorException : CapeComputationException, ECapeSolvingError
{

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeSolvingErrorHR);
        m_interfaceName = "ECapeSolvingError";
        m_name = "CapeSolvingErrorException";
    }

    /// <summary>Initializes a new instance of the CapeSolvingErrorException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeSolvingErrorException() : base() { }
    /// <summary>Initializes a new instance of the CapeSolvingErrorException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeSolvingErrorException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeSolvingErrorException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeSolvingErrorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeSolvingErrorException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeSolvingErrorException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>Exception thrown when the Hessian for the MINLP problem is not available.</summary>
/// <remarks>Exception thrown when the Hessian for the MINLP problem is not available.</remarks>
[Serializable]
[Guid("3044EA08-F054-4315-B67B-4E3CD2CF0B1E")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeHessianInfoNotAvailableException : 
    CapeSolvingErrorException, ECapeHessianInfoNotAvailable
{

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeHessianInfoNotAvailableHR);
        m_interfaceName = "ECapeHessianInfoNotAvailable";
        m_name = "CapeHessianInfoNotAvailableHR";
    }
    /// <summary>Initializes a new instance of the CapeHessianInfoNotAvailableException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeHessianInfoNotAvailableException() : base() { }
    /// <summary>Initializes a new instance of the CapeHessianInfoNotAvailableException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeHessianInfoNotAvailableException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeHessianInfoNotAvailableException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeHessianInfoNotAvailableException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeHessianInfoNotAvailableException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeHessianInfoNotAvailableException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>Exception thrown when the time-out criterion is reached.</summary>
/// <remarks>Exception thrown when the time-out criterion is reached.</remarks>
[Serializable]
[Guid("0D5CA7D8-6574-4c7b-9B5F-320AA8375A3C")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeTimeOutException : CapeUserException, ECapeTimeOut
{

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeTimeOutHR);
        m_interfaceName = "ECapeTimeOut";
        m_name = "CapeTimeOutException";
    }

    /// <summary>Initializes a new instance of the CapeTimeOutException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeTimeOutException() : base() { }
    /// <summary>Initializes a new instance of the CapeTimeOutException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeTimeOutException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeTimeOutException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeTimeOutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeTimeOutException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeTimeOutException(String message, Exception inner) : base(message, inner) { }
};

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

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
    }

    /// <summary>Creates a new instance of the COMCapeOpenExceptionWrapper class.</summary>
    /// <remarks><para>Creates a .Net based exception wrapper for COM-based CAPE-OPEN componets to 
    /// enable users to utilize .Net structured exception handling.</para></remarks>
    /// <param name = "message">The error message text from the COM-based component.</param>
    /// <param name = "exceptionObject">The CAPE-OPEN object that raised the error.</param>
    /// <param name = "HRESULT">The COM HResult value.</param>
    /// <param name = "inner">An inner .Net-based exception obtained from the IErrorInfo
    /// object, if implemented or an accompanying .Net exception.</param>
    public COMCapeOpenExceptionWrapper(String message, Object exceptionObject, int HRESULT, Exception inner)
        : base(message, inner)
    {
        HResult = HRESULT;
        if (exceptionObject is ECapeRoot)
        {
            m_name = String.Concat("CAPE-OPEN Error: ", ((ECapeRoot)exceptionObject).Name);
        }

        if (exceptionObject is ECapeUser)
        {
            ECapeUser user = (ECapeUser)exceptionObject;
            m_description = user.description;
            m_interfaceName = user.interfaceName;
            Source = user.scope;
            HelpLink = user.moreInfo;
        }
    }
};

/// <summary>Exception thrown when a requested theromdynamic property is not available.</summary>
/// <remarks>Exception thrown when a requested theromdynamic property is not available.</remarks>
[Serializable]
 [Guid("5BA36B8F-6187-4e5e-B263-0924365C9A81")]
 [ComVisible(true)]
 [ClassInterface(ClassInterfaceType.None)]
public class CapeThrmPropertyNotAvailableException : CapeUserException, ECapeThrmPropertyNotAvailable
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeThrmPropertyNotAvailableHR);
        m_interfaceName = "ECapePersistence";
        m_name = "CapeThrmPropertyNotAvailable";
    }

    /// <summary>Initializes a new instance of the CapeThrmPropertyNotAvailable class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeThrmPropertyNotAvailableException() : base() { }
    /// <summary>Initializes a new instance of the CapeThrmPropertyNotAvailable class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeThrmPropertyNotAvailableException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeThrmPropertyNotAvailable class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeThrmPropertyNotAvailableException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeThrmPropertyNotAvailable class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>An exception that is thrown as a direct result of a previous exception 
    /// should include a erence to the previous exception in the InnerException 
    /// property. The InnerException property returns the same value that is passed 
    /// into the constructor, or a null erence if the InnerException property does 
    /// not supply the inner exception value to the constructor.</para></remarks>
    /// <param name = "message">The error message string.</param>
    /// <param name = "inner">The inner exception erence.</param>
    public CapeThrmPropertyNotAvailableException(String message, Exception inner) : base(message, inner) { }
};

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
    /// <param name = "exceptionObject">The CAPE-OPEN object that raised the error.</param>
    /// <param name = "inner">An inner .Net-based exception obtained from the IErrorInfo
    /// object, if implemented or an accompanying .Net exception.</param>
    /// <see c = "COMCapeOpenExceptionWrapper">COMCapeOpenExceptionWrapper</see> 
    public static Exception ExceptionForHRESULT(Object exceptionObject, Exception inner)
    {
        int HRESULT = ((COMException)inner).ErrorCode;
        String message = "Exception thrown by COM PMC.";
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
                return new CapeBadCOParameter(message, exception);
            case unchecked((int)0x80040505)://ECapeBadArgumentHR = 0x80040505 ,
                return new CapeBadArgumentException(message, exception, ((ECapeBadArgument)exception).position);
            case unchecked((int)0x80040506)://ECapeInvalidArgumentHR = 0x80040506 ,
                return new CapeInvalidArgumentException(message, exception, ((ECapeBadArgument)exception).position);
            case unchecked((int)0x80040507)://ECapeOutOfBoundsHR = 0x80040507 
            {
                Exception p_Ex = (Exception)(exception);
                if (exception is ECapeBoundaries)
                {
                    ECapeBoundaries p_Ex1 = (ECapeBoundaries)(exception);
                    if (exception is ECapeBadArgument)
                    {
                        ECapeBadArgument p_Ex2 = (ECapeBadArgument)(exception);
                        return new CapeOutOfBoundsException(message, p_Ex, (int)p_Ex2.position, p_Ex1.lowerBound, p_Ex1.upperBound, p_Ex1.value, p_Ex1.type);
                    }
                    else return new CapeOutOfBoundsException(message, p_Ex, 0, p_Ex1.lowerBound, p_Ex1.upperBound, p_Ex1.value, p_Ex1.type);
                }
                else return new CapeOutOfBoundsException(message, p_Ex, 0, 0.0, 0.0, 0.0, "");
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
                else return new CapeBadInvOrderException(message, exception, "");
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
};