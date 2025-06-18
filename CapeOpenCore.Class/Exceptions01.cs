/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.18
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>This is the abstract base class for all .Net based CAPE-OPEN exception classes.</summary>
/// <remarks><para>One of the principal advantages of .NET over COM is the additional information 
/// included in exception handling. In COM, exceptions were handled through returning 
/// an HRESULT value, which is an integer that indicated whether the function call had
/// successfully returned (Rogerson, 1997). Because the HRESULT value was a 32-bit 
/// integer, it could indicate more information than simply success or failure, but it
/// was limited in that it did not include descriptive information about the exception
/// that occurred.</para>
/// <para>Under .NET, an application exception class is available 
/// (System.ApplicationException) that can be used to provide information such as a message and the source 
/// of the exception. The CAPE-OPEN exception definitions all derive from an 
/// ECapeRoot interface (Belaud et al, 2001). In the current implementation of the 
/// CAPE-OPEN exception classes, all exception classes derive from the 
/// CapeUserException class, which itself is derived from the .NET 
/// System.ApplicationException class. The CapeUserException class exposes the 
/// <see c = "ECapeRoot"/> and <see c = "ECapeUser"/> interfaces. In this way, 
/// all exceptions that are raised by the process modeling components can be caught 
/// either as a CapeRootException or as a System.ApplicationException in addition to 
/// being caught as the derived exception type. </para></remarks>
[Serializable]
[ComVisible(true)]
[Guid("28686562-77AD-448f-8A41-8CF9C3264A3E")]
[Description("")]
[ClassInterface(ClassInterfaceType.None)]
public abstract class CapeUserException : ApplicationException, ECapeRoot, ECapeUser
{

    /// <summary>The name of the exception interface for the exception being thrown.</summary>
    /// <remarks>The m_interfaceName field is set in the <see c = "Initialize">Initialize</see> method for the exception. Any exception
    /// that derives from the CapeUserException class will need to set this value in the Initialize method.</remarks>
    protected String m_interfaceName;
    /// <summary>The name of the exception being thrown.</summary>
    /// <remarks>The m_name field is set in the <see c = "Initialize">Initialize</see> method for the exception. Any exception
    /// that derives from the CapeUserException class will need to set this value in the Initialize method.</remarks>
    protected String m_name;
    /// <summary>The description of the exception being thrown.</summary>
    /// <remarks>The m_name field is set in the <see c = "Initialize">Initialize</see> method for the exception. Any exception
    /// that derives from the CapeUserException class will need to set this value in the Initialize method.</remarks>
    protected String m_description;

    /// <summary>Initializes a new instance of the CapeUserException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeUserException()
    {
        m_description = "An application error has occurred.";
        Initialize();
    }
    /// <summary>Initializes a new instance of the CapeUserException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeUserException(String message)
        : base(message)
    {
        m_description = message;
        Initialize();
    }

    /// <summary>Initializes a new instance of the CapeUserException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeUserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
        Initialize();
    }

    /// <summary>Initializes a new instance of the CapeUserException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    public CapeUserException(String message, Exception inner)
        : base(message, inner)
    {
        m_description = message;
        Initialize();
    }


    /// <summary>A virtual abstract function that is inherieted by derived classes to 
    /// initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Derived classes should implement this class and set the values of the HResult, interface name and exception name.</para>
    /// <code>
    /// virtual void Initialize() override 
    /// {
    ///  HResult = (int)CapeErrorInterfaceHR.ECapeUnknownHR;
    ///  m_interfaceName = "ECapeUnknown";
    ///  m_name = "CUnknownException";
    /// }
    /// </code></remarks>
    protected abstract void Initialize();

    /// <summary>
    ///	The function that controls COM registration. </summary>
    /// <remarks>
    ///	This function adds the registration keys specified in the CAPE-OPEN Method and
    /// Tools specifications. In particular, it indicates that this unit operation implements
    /// the CAPE-OPEN Unit Operation Category Identification. It also adds the CapeDescription
    /// registry keys using the <see c ="CapeNameAttribute"/>, <see c ="CapeDescriptionAttribute"/>, <see c ="CapeVersionAttribute"/>
    /// <see c ="CapeVendorURLAttribute"/>, <see c ="CapeHelpURLAttribute"/>, 
    /// and <see c ="CapeAboutAttribute"/> attributes.</remarks>
    /// <param name = "t">The type of the class being registered.</param> 
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    [ComRegisterFunction()]
    public static void RegisterFunction(Type t) { }
    /// <summary>
    ///	This function controls the removal of the class from the COM registry when the class is unistalled. </summary>
    /// <remarks>
    ///	The method will remove all subkeys added to the class' regristration, including the CAPE-OPEN
    /// specific keys added in the <see c ="RegisterFunction"/> method.</remarks>
    /// <param name = "t">The type of the class being unregistered.</param> 
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    [ComUnregisterFunction()]
    public static void UnregisterFunction(Type t) { }
    // ECapeRoot method
    // returns the message string in the System.ApplicationException.
    /// <summary>The name of the exception being thrown.</summary>
    /// <remarks>The name of the exception being thrown.</remarks>
    /// <value>
    /// The name of the exception being thrown.
    /// </value>
    public String Name
    {
        get
        {
            return m_name;
        }
    }

    /// <summary>Code to designate the subcategory of the error. </summary>
    /// <remarks>The assignment of values is left to each implementation. So that is a 
    /// proprietary code specific to the CO component provider. By default, set to 
    /// the CAPE-OPEN error HRESULT <see c = "CapeErrorInterfaceHR"/>.</remarks>
    /// <value>
    /// The HRESULT value for the exception.
    /// </value>
    public int code
    {
        get
        {
            return HResult;
        }
    }

    /// <summary>The description of the error.</summary>
    /// <remarks>The error description can include a more verbose description of the condition that
    /// caused the error.</remarks>
    /// <value>
    /// A string description of the exception.
    /// </value>
    public String description
    {
        get
        {
            return m_description;
        }

        set
        {
            m_description = value;
        }
    }

    /// <summary>The scope of the error.</summary>
    /// <remarks>This property provides a list of packages where the error occurred. 
    /// For example <see c = "ICapeIdentification"/>.</remarks>
    /// <value>The source of the error.</value>
    public String scope
    {
        get
        {
            return Source;
        }
    }

    /// <summary>The name of the interface where the error is thrown. This is a mandatory field."</summary>
    /// <remarks>The interface that the error was thrown.</remarks>
    /// <value>The name of the interface.</value>
    public String interfaceName
    {
        get
        {
            return m_interfaceName;
        }
        set
        {
            m_interfaceName = value;
        }
    }

    /// <summary>The name of the operation where the error is thrown. This is a mandatory field.</summary>
    /// <remarks>This field provides the name of the operation being perfomed when the exception was raised.</remarks>
    /// <value>The operation name.</value>
    public String operation
    {
        get
        {
            return StackTrace;
        }
    }

    /// <summary>A URL to a page, document, website, where more information on the error can be found. The content of this information is obviously implementation dependent.</summary>
    /// <remarks>This field provides an internet URL where more information about the error can be found.</remarks>
    /// <value>The URL.</value>
    public String moreInfo
    {
        get
        {
            return HelpLink;
        }
    }
};

/// <summary>This exception is raised when other error(s), specified by the operation, do not suit.</summary>
/// <remarks><para>A standard exception that can be thrown by a CAPE-OPEN object to indicate that the error
/// that occurred was not one that was suitable for any of the other errors supported by the object. </para></remarks>
[Serializable]
[ComVisible(true)]
[Guid("B550B2CA-6714-4e7f-813E-C93248142410")]
[Description("")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeUnknownException : CapeUserException,
    ECapeUnknown
{

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeUnknownHR);
        m_interfaceName = "ECapeUnknown";
        m_name = "CUnknownException";
    }


    /// <summary>Initializes a new instance of the CapeUnknownException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeUnknownException() : base() { }
    /// <summary>Initializes a new instance of the CapeUnknownException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeUnknownException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeUnknownException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeUnknownException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeUnknownException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    public CapeUnknownException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>This exception is raised when other error(s), specified by the operation, do not suit.</summary>
/// <remarks><para>A standard exception that can be thrown by a CAPE-OPEN object to indicate that the error
/// that occurred was not one that was suitable for any of the other errors supported by the object. </para></remarks>
[Serializable]
[ComVisible(true)]
[Guid("16049506-E086-4baf-9905-9ED13D50D0E3")]
[Description("")]
[ClassInterface(ClassInterfaceType.None)]
public class CapeUnexpectedException : CapeUserException
{

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)0x8000ffff);
        m_interfaceName = "IPersistStreamInit";
        m_name = "CUnexpectedException";
    }
    
    /// <summary>Initializes a new instance of the CapeUnknownException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeUnexpectedException() : base() { }
    /// <summary>Initializes a new instance of the CapeUnknownException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeUnexpectedException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeUnknownException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeUnexpectedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeUnknownException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    public CapeUnexpectedException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>The base class of the errors hierarchy related to any data.</summary>
/// <remarks><para>The CapeDataException class is a base class for errors related to data. The data are the 
/// arguments of operations, the parameters coming from the Parameter Common Interface 
/// and information on licence key.	
/// </para></remarks>
[Serializable]
[Guid("53551E7C-ECB2-4894-B71A-CCD1E7D40995")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeDataException : CapeUserException, ECapeData
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeDataHR);
        m_name = "CapeDataException";
    }

    /// <summary>Initializes a new instance of the CapeDataException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeDataException() : base() { }
    /// <summary>Initializes a new instance of the CapeDataException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeDataException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeDataException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeDataException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeDataException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    public CapeDataException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>A parameter, which is an object from the Parameter Common Interface, has an invalid status.</summary>
/// <remarks>The name of the invalid parameter, along with the parameter itself are available from the exception.</remarks>
[Serializable]
[Guid("667D34E9-7EF7-4ca8-8D17-C7577F2C5B62")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeBadCOParameter : CapeDataException, ECapeBadCOParameter
{
    private String m_Parametername;
    private Object m_Parameter;

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    /// <param name = "parameterName">The name of the parameter with the invalid status.</param>
    /// <param name = "pParameter">The parameter with the invalid status.</param>
    protected void Initialize(String parameterName, Object pParameter)
    {
        m_Parametername = parameterName;
        m_Parameter = (ICapeParameter)(parameter);
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeBadArgumentHR);
        m_interfaceName = "ECapeBadArgument";
        m_name = "CapeBadArgumentException";
    }

    /// <summary>Initializes a new instance of the CapeBadCOParameter class with the name of the parameter and the parameter that caused the exception. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    /// <param name = "parameterName">The name of the parameter with the invalid status.</param>
    /// <param name = "parameter">The parameter with the invalid status.</param>
    public CapeBadCOParameter(String parameterName, Object parameter) : base() { Initialize(parameterName, parameter); }
    /// <summary>Initializes a new instance of the CapeBadCOParameter class with a specified error message, the name of the parameter, and the parameter that caused the exception. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    /// <param name = "parameterName">The name of the parameter with the invalid status.</param>
    /// <param name = "parameter">The parameter with the invalid status.</param>
    public CapeBadCOParameter(String message, String parameterName, Object parameter) : base(message) { Initialize(parameterName, parameter); }
    /// <summary>Initializes a new instance of the CapeBadCOParameter class with serialized data, the name of the parameter, and the parameter that caused the exception. </summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    /// <param name = "parameterName">The name of the parameter with the invalid status.</param>
    /// <param name = "parameter">The parameter with the invalid status.</param>
    public CapeBadCOParameter(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, String parameterName, Object parameter) : base(info, context) { Initialize(parameterName, parameter); }
    /// <summary>Initializes a new instance of the CapeBadCOParameter class with a specified error message and a reference to the inner exception, the name of the parameter, and the parameter that caused the exception. </summary>
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
    /// <param name = "parameterName">The name of the parameter with the invalid status.</param>
    /// <param name = "parameter">The parameter with the invalid status.</param>
    public CapeBadCOParameter(String message, Exception inner, String parameterName, Object parameter) : base(message, inner) { Initialize(parameterName, parameter); }

    /// <summary>The name of the CO parameter that is throwing the exception.</summary>
    /// <remarks>This provides the name of the parameter that threw the exception.</remarks>
    /// <value>The name of the parameter that threw the exception.</value>
    public virtual Object parameter
    {
        get
        {
            return m_Parameter;
        }
    }

    /// <summary>The name of the CO parameter that is throwing the exception.</summary>
    /// <remarks>This provides access to the parameter that threw the exception.</remarks>
    /// <value>The parameter that threw the exception.</value>
    public virtual String parameterName
    {
        get
        {
            return m_name;
        }
    }
};

/// <summary>An argument value of the operation is not correct.</summary>
/// <remarks>An argument value of the operation is not correct. The position of the 
/// argument value within the signature of the operation. First argument is as 
/// position 1.</remarks>
[Serializable]
[Guid("D168E99F-C1EF-454c-8574-A8E26B62ADB1")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeBadArgumentException : CapeDataException, ECapeBadArgument, ECapeBadArgument093
{
    private int m_Position;

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    protected void Initialize(int position)
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeBadArgumentHR);
        m_interfaceName = "ECapeBadArgument";
        m_name = "CapeBadArgumentException";
        m_Position = position;
    }

    /// <summary>Initializes a new instance of the CapeBadArgumentException class with the position of the error. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    public CapeBadArgumentException(int position) : base() { Initialize(position); }
    /// <summary>Initializes a new instance of the CapeBadArgumentException class with a specified error message and the position of the error. </summary>. 
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    public CapeBadArgumentException(String message, int position) : base(message) { Initialize(position); }
    /// <summary>Initializes a new instance of the CapeBadArgumentException class with serialized data and the position of the error. </summary>.
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    public CapeBadArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, int position) : base(info, context) { Initialize(position); }
    /// <summary>Initializes a new instance of the CapeBadArgumentException class with a specified error message and 
    /// a reference to the inner exception that is the cause of this exception.</summary>
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
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    public CapeBadArgumentException(String message, Exception inner, int position) : base(message, inner) { Initialize(position); }


    /// <summary>The position of the argument value within the signature of the operation. First argument is as position 1.</summary>
    /// <remarks>This provides the location of the invalid argument in the argument list for the function call.</remarks>
    /// <value>The position of the argument that is bad. The first argument is 1.
    /// </value>
    public virtual short position
    {
        get
        {
            return (short)m_Position;
        }
    }

    /// <summary>The position of the argument value within the signature of the operation. First argument is as position 1.</summary>
    /// <remarks>This provides the location of the invalid argument in the argument list for the function call.</remarks>
    /// <value>The position of the argument that is bad. The first argument is 1.
    /// </value>
    int ECapeBadArgument093.position
    {
        get
        {
            return (int)m_Position;
        }
    }
};

/// <summary>This is an abstract class that allows derived classes to provide information 
/// about error that result from values that are outside of their bounds. It can be raised 
/// to indicate that the value of either a method argument or the value of a object 
/// parameter is out of range.</summary>
/// <remarks><para>CapeBoundariesException is a "utility" class which factorises a state which describes the value, its type and its boundaries.</para>
/// <para>This is an abstract class. No real error can be raised from this class.</para></remarks>
[Serializable]
[Guid("62B1EE2F-E488-4679-AFA3-D490694D6B33")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public abstract class CapeBoundariesException : CapeUserException,
    ECapeBoundaries
{
    private double m_Lower;
    private double m_Upper;
    private double m_Value;
    private String m_Type;

    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    protected void SetBoundaries(double LowerBound, double UpperBound, double value, String type)
    {
        m_Lower = LowerBound;
        m_Upper = UpperBound;
        m_Value = value;
        m_Type = type;
    }

    /// <summary>Initializes a new instance of the CapeBoundariesException class with the lower bound, upper bound, value, type, and position of the parameter that is the cause of this exception. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    public CapeBoundariesException(double LowerBound, double UpperBound, double value, String type)
        : base()
    {
        SetBoundaries(LowerBound, UpperBound, value, type);
    }
    /// <summary>Initializes a new instance of the CapeBoundariesException class with a specified error message, the lower bound, upper bound, value, type, and position of the parameter that is the cause of this exception. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    public CapeBoundariesException(String message, double LowerBound, double UpperBound, double value, String type)
        : base(message)
    {
        SetBoundaries(LowerBound, UpperBound, value, type);
    }
    /// <summary>Initializes a new instance of the CapeBoundariesException class with serialized data, the lower bound, upper bound, value, type, and position of the parameter that is the cause of this exception. </summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    public CapeBoundariesException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, double LowerBound, double UpperBound, double value, String type)
        : base(info, context)
    {
        SetBoundaries(LowerBound, UpperBound, value, type);
    }
    /// <summary>Initializes a new instance of the CapeBoundariesException class with a specified error message, the lower bound, upper bound, value, type and position of the parameter, and a reference to the inner exception that is the cause of this exception.</summary>
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
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    public CapeBoundariesException(String message, Exception inner, double LowerBound, double UpperBound, double value, String type)
        : base(message, inner)
    {
        SetBoundaries(LowerBound, UpperBound, value, type);
    }

    /// <summary>The value of the lower bound.</summary>
    /// <remarks><para>This provides the user with the acceptable lower bounds of the argument.</para></remarks>
    /// <value>The lower bound for the argument.</value>
    public double lowerBound
    {
        get
        {
            return m_Lower;
        }
    }

    /// <summary>The value of the upper bound.</summary>
    /// <remarks><para>This provides the user with the acceptable upper bounds of the argument.</para></remarks>
    /// <value>The upper bound for the argument.</value>
    public double upperBound
    {
        get
        {
            return m_Upper;
        }
    }

    /// <summary>The current value which has led to an error.</summary>
    /// <remarks><para>This provides the user with the value that caused the error condition.</para></remarks>
    /// <value>The value that resulted in the error condition.</value>
    public double value
    {
        get
        {
            return m_Value;
        }
    }

    /// <summary>The type/nature of the value. </summary>
    /// <remarks>The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ..."</remarks>
    /// <value>A string that indicates the anture or type of the value required.</value>
    public String type
    {
        get
        {
            return m_Type;
        }
    }
};

/// <summary>An argument value is outside of the bounds..</summary>
/// <remarks><para>This class is derived from the <see c = "CapeBoundariesException">CapeBoundariesException</see> class.
/// It is used to indicate that one of the parameters is outside of its bounds.</para></remarks>
[Serializable]
[Guid("4438458A-1659-48c2-9138-03AD8B4C38D8")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeOutOfBoundsException : CapeBoundariesException,
    ECapeOutOfBounds, ECapeBadArgument, ECapeBadArgument093, ECapeData
{
    private int m_Position;

    /// <summary>The initialize method for all classes derived from CapeOutOfBoundsException need to include the
    /// pertinent information related to the boundaries.</summary>
    /// <remarks><para>This method is sealed so that classes that derive from CapeOutOfBoundsException include the required information about the position of the argument.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeOutOfBoundsHR);
        m_interfaceName = "ECapeOutOfBounds";
        m_name = "CapeOutOfBoundsException";
    }

    /// <summary>Initializes a new instance of the CapeOutOfBoundsException class with the position of the error. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    public CapeOutOfBoundsException(int position, double LowerBound, double UpperBound, double value, String type) :
        base(LowerBound, UpperBound, value, type)
    {
        m_Position = position;
    }
    /// <summary>Initializes a new instance of the CapeOutOfBoundsException class with a specified error message and the position of the error. </summary>. 
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    public CapeOutOfBoundsException(String message, int position, double LowerBound, double UpperBound, double value, String type) :
        base(message, LowerBound, UpperBound, value, type)
    {
        m_Position = position;
    }
    /// <summary>Initializes a new instance of the CapeOutOfBoundsException class with serialized data and the position of the error. </summary>.
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    public CapeOutOfBoundsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, int position, double LowerBound, double UpperBound, double value, String type) :
        base(info, context, LowerBound, UpperBound, value, type)
    {
        m_Position = position;
    }
    /// <summary>Initializes a new instance of the CapeOutOfBoundsException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    /// <param name = "LowerBound">The value of the lower bound.</param>
    /// <param name = "UpperBound">The value of the upper bound.</param>
    /// <param name = "value">The current value which has led to an error.</param>
    /// <param name = "type">The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...</param>
    public CapeOutOfBoundsException(String message, Exception inner, int position, double LowerBound, double UpperBound, double value, String type) :
        base(message, inner, LowerBound, UpperBound, value, type)
    {
        m_Position = position;
    }

    /// <summary>The position of the argument value within the signature of the operation. First argument is as position 1.</summary>
    /// <remarks>This provides the location of the invalid argument in the argument list for the function call.</remarks>
    /// <value>The position of the argument that is bad. The first argument is 1.
    /// </value>
    public short position
    {
        get
        {
            return (short)m_Position;
        }
    }

    /// <summary>The position of the argument value within the signature of the operation. First argument is as position 1.</summary>
    /// <remarks>This provides the location of the invalid argument in the argument list for the function call.</remarks>
    /// <value>The position of the argument that is bad. The first argument is 1.
    /// </value>
    int ECapeBadArgument093.position
    {
        get
        {
            return (int)m_Position;
        }
    }
};

/// <summary>The base class of the errors hierarchy related to calculations.</summary>
/// <remarks>This class is used to indicate that an error occurred in the performance of a calculation. 
/// Other calculation-related classes such as 
/// <see c = "CapeFailedInitialisationException">CapeOpen.CapeFailedInitialisationException</see>, 
/// <see c = "CapeOutOfResourcesException">CapeOpen.CapeOutOfResourcesException</see>, 
/// <see c = "CapeSolvingErrorException">CapeOpen.CapeSolvingErrorException</see>, 
/// <see c = "CapeBadInvOrderException">CapeOpen.CapeBadInvOrderException</see>, 
/// <see c = "CapeInvalidOperationException">CapeOpen.CapeInvalidOperationException</see>, 
/// <see c = "CapeNoMemoryException">CapeOpen.CapeNoMemoryException</see>, and 
/// <see c = "CapeTimeOutException">CapeOpen.CapeTimeOutException</see> 
/// derive from this class.</remarks>
[Serializable]
[Guid("9D416BF5-B9E3-429a-B13A-222EE85A92A7")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeComputationException : CapeUserException, ECapeComputation
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeComputationHR);
        m_interfaceName = "ECapeComputation";
        m_name = "CapeComputationException";
    }

    /// <summary>Initializes a new instance of the CapeComputationException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeComputationException() : base() { }
    /// <summary>Initializes a new instance of the CapeComputationException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeComputationException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeComputationException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeComputationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeComputationException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    public CapeComputationException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>This exception is thrown when necessary initialisation has not been performed or has failed.</summary>
/// <remarks>The pre-requisites operations are not valid. The necessary initialisation has not been performed or has failed.</remarks>
[Serializable]
[Guid("E407595C-6D1C-4b8c-A29D-DB0BE73EFDDA")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeFailedInitialisationException : CapeComputationException, ECapeFailedInitialisation
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeFailedInitialisationHR);
        m_interfaceName = "ECapeFailedInitialisation";
        m_name = "CapeFailedInitialisationException";
    }

    /// <summary>Initializes a new instance of the CapeFailedInitialisationException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeFailedInitialisationException() : base() { }
    /// <summary>Initializes a new instance of the CapeFailedInitialisationException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeFailedInitialisationException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeFailedInitialisationException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeFailedInitialisationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeFailedInitialisationException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    public CapeFailedInitialisationException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>The base class of the errors hierarchy related to the current implementation.</summary>
/// <remarks>This class is used to indicate that an error occurred in the with the implementation of an object. 
/// The implemenation-related classes such as 
/// <see c = "CapeNoImplException ">CapeOpen.CapeNoImplException </see> and 
/// <see c = "CapeLimitedImplException ">CapeOpen.CapeLimitedImplException </see>
/// derive from this class.</remarks>
[Serializable]
[Guid("7828A87E-582D-4947-9E8F-4F56725B6D75")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeImplementationException : CapeUserException, ECapeImplementation
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeImplementationHR);
        m_interfaceName = "ECapeImplementation";
        m_name = "CapeImplementationException";
    }

    /// <summary>Initializes a new instance of the CapeImplementationException class. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    public CapeImplementationException() : base() { }
    /// <summary>Initializes a new instance of the CapeImplementationException class with a specified error message. </summary>
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    public CapeImplementationException(String message) : base(message) { }
    /// <summary>Initializes a new instance of the CapeImplementationException class with serialized data.</summary>
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    public CapeImplementationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    /// <summary>Initializes a new instance of the CapeImplementationException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    public CapeImplementationException(String message, Exception inner) : base(message, inner) { }
};

/// <summary>An invalid argument value was passed. For instance the passed name of 
/// the phase does not belong to the CO Phase List.</summary>
/// <remarks>An argument value of the operation is invalid. The position of the 
/// argument value within the signature of the operation. First argument is as 
/// position 1.</remarks>
[Serializable]
[Guid("B30127DA-8E69-4d15-BAB0-89132126BAC9")]
[ComVisibleAttribute(true)]
[ClassInterface(ClassInterfaceType.None)]
public class CapeInvalidArgumentException : CapeBadArgumentException, ECapeInvalidArgument
{
    /// <summary>Initializes the description, interface name and name fields of this exception.</summary>
    /// <remarks><para>Sets the values of the HResult, interface name and exception name.</para></remarks>
    protected override void Initialize()
    {
        HResult = unchecked((int)CapeErrorInterfaceHR.ECapeInvalidArgumentHR);
        m_interfaceName = "ECapeInvalidArgument";
        m_name = "CapeInvalidArgumentException";
    }

    /// <summary>Initializes a new instance of the CapeInvalidArgumentException class with the position of the error. </summary>
    /// <remarks>This constructor initializes the Message property of the new instance to a 
    /// system-supplied message that describes the error, such as "An application 
    /// error has occurred." This message takes into account the current system 
    /// culture.</remarks>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    public CapeInvalidArgumentException(int position) : base(position) { }
    /// <summary>Initializes a new instance of the CapeInvalidArgumentException class with a specified error message and the position of the error. </summary>. 
    /// <remarks><para>The content of the message parameter is intended to be understood by 
    /// humans. The caller of this constructor is required to ensure that this string 
    /// has been localized for the current system culture.</para>
    /// <para>This message takes into account the current system culture.</para></remarks>
    /// <param name = "message">A message that describes the error.</param>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    public CapeInvalidArgumentException(String message, int position) : base(message, position) { }
    /// <summary>Initializes a new instance of the CapeInvalidArgumentException class with serialized data and the position of the error. </summary>.
    /// <remarks> This constructor is called during deserialization to reconstitute the 
    /// exception object transmitted over a stream. For more information, see XML and 
    /// SOAP Serialization.</remarks>
    /// <param name = "info">The object that holds the serialized object data.</param>
    /// <param name = "context">The contextual information about the source or 
    /// destination. </param>
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    public CapeInvalidArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, int position) : base(info, context, position) { }
    /// <summary>Initializes a new instance of the CapeInvalidArgumentException class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
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
    /// <param name = "position">The position of the argument value within the signature of the operation. First argument is as position 1.</param>
    public CapeInvalidArgumentException(String message, Exception inner, int position) : base(message, inner, position) { }
};

