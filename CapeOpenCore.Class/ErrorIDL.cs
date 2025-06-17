/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.17
 */

/* IMPORTANT NOTICE
(c) The CAPE-OPEN Laboratory Network, 2002.
All rights are reserved unless specifically stated otherwise

Visit the web site at www.colan.org

This file has been edited using the editor from Microsoft Visual Studio 6.0
This file can view properly with any basic editors and browsers (validation done under MS Windows and Unix)
*/

// This file was developed/modified by JEAN-PIERRE-BELAUD for CO-LaN organisation - March 2003
// This file was modified by Bill Barrett of USEPA to restore CAPE-OPENv0.93 interface - November 30, 2005.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

// ---- The scope of the error interface ------------------------------
// Reference document: Error Common Interface

namespace CapeOpenCore.Class;

// .NET Error Interface 的翻译：
/// <summary>列举 CAPE-OPEN 错误处理接口的各种 HRESULT 值。</summary>
/// <remarks><para>摘自《Strategies for Handling Errors in COM+》，详见平台 SDK 帮助文档。</para>
/// <para>使用 FACILITY_ITF 错误代码范围来报告与接口相关的特定错误。</para>
/// <para>特定于接口的错误应在 FACILITY_ITF 误差范围内，介于 0x0200 和 0xFFFF 之间。然而，
/// 由于微软在 0x0200 之后使用一些代码，CAPE-OPEN 错误代码将从 0x0500 开始。</para>
/// <para>使用 C++ 中的 MAKE_HRESULT 宏来引入一个与接口相关的错误代码，如下面的示例所示：
/// <c>const HRESULT ERROR_NUMBER = MAKE_HRESULT (SEVERITY_ERROR, FACILITY_ITF,10);</c></para>
/// <para>因此，FIRST_E_INTERFACE_HR 的偏移量必须介于 1 和 64255（0xFFFF-0x0500）之间。我们保留 0 作为偏移量。
/// <c>const int FIRST_E_INTERFACE_HR = (int)0x80040500;</c></para>
/// <para>用于 CO 错误接口的最后一个 HR 值：
/// <c>const int LAST_USED_E_INTERFACE_HR = (int)0x80040517;</c></para>
/// <para>可用于表示 CO 错误接口的最高 HR 值：
/// <c>const int LAST_E_INTERFACE_HR = (int)0x8004FFFF;</c></para></remarks>
[Serializable]
public enum CapeErrorInterfaceHR
{
	/// <summary>0x80040501</summary>
	ECapeUnknownHR = unchecked ((int)0x80040501),
	/// <summary>0x80040502</summary>
	ECapeDataHR = unchecked ((int)0x80040502),
	/// <summary>0x80040503</summary>
	ECapeLicenceErrorHR = unchecked((int)0x80040503),
	/// <summary>0x80040504</summary>
	ECapeBadCOParameterHR = unchecked ((int)0x80040504),
	/// <summary>0x80040505</summary>
	ECapeBadArgumentHR = unchecked ((int)0x80040505),
	/// <summary>0x80040506</summary>
	ECapeInvalidArgumentHR = unchecked ((int)0x80040506),
	/// <summary>0x80040507</summary>
	ECapeOutOfBoundsHR = unchecked ((int)0x80040507),
	/// <summary>0x80040508</summary>
	ECapeImplementationHR = unchecked ((int)0x80040508),
	/// <summary>0x80040509 </summary>
	ECapeNoImplHR = unchecked ((int)0x80040509),
	/// <summary>0x8004050A</summary>
	ECapeLimitedImplHR = unchecked ((int)0x8004050A),
	/// <summary>0x8004050B</summary>
	ECapeComputationHR = unchecked ((int)0x8004050B),
	/// <summary>0x8004050C</summary>
	ECapeOutOfResourcesHR = unchecked ((int)0x8004050C),
	/// <summary>0x8004050D</summary>
	ECapeNoMemoryHR = unchecked ((int)0x8004050D),
	/// <summary>0x8004050E</summary>
	ECapeTimeOutHR = unchecked ((int)0x8004050E),
	/// <summary>0x8004050F</summary>
	ECapeFailedInitialisationHR = unchecked ((int)0x8004050F),
	/// <summary>0x80040510</summary>
	ECapeSolvingErrorHR = unchecked ((int)0x80040510),
	/// <summary>0x80040511</summary>
	ECapeBadInvOrderHR = unchecked ((int)0x80040511),
	/// <summary>0x80040512</summary>
	ECapeInvalidOperationHR = unchecked ((int)0x80040512),
	/// <summary>0x80040513</summary>
	ECapePersistenceHR = unchecked ((int)0x80040513),
	/// <summary>0x80040514</summary>
	ECapeIllegalAccessHR = unchecked ((int)0x80040514),
	/// <summary>0x80040515</summary>
	ECapePersistenceNotFoundHR = unchecked ((int)0x80040515),
	/// <summary>0x80040516</summary>
	ECapePersistenceSystemErrorHR = unchecked ((int)0x80040516),
	/// <summary>0x80040517</summary>
	ECapePersistenceOverflowHR = unchecked ((int)0x80040517),
	/// <summary>0x80040518, Specific to MINLP</summary>
	ECapeOutsideSolverScopeHR = unchecked ((int)0x80040518), // specific to MINLP
	/// <summary>0x80040519, Specific to MINLP</summary>
	ECapeHessianInfoNotAvailableHR = unchecked ((int)0x80040519), // specific to MINLP
	/// <summary>0x80040519, Specific to MINLP</summary>
	ECapeThrmPropertyNotAvailableHR = unchecked ((int)0x80040520)
}

// ECapeBoundaries interface
/// <summary>此接口提供有关由于值超出其范围而导致的错误的信息。它可以被抛出，以表示方法参数或对象参数的值超出了范围。</summary>
/// <remarks><para>ECapeBoundaries 是一个“实用”接口，用于分解描述值、其类型及其边界的状态。</para></remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeBoundaries_IID)]
[Description("ECapeBoundaries Interface")]
public interface ECapeBoundaries
{	
	/// <summary>下限的值。</summary>
	/// <remarks>这为用户提供了参数的可接受下限。</remarks>
	/// <value>参数的下限。</value>
	[DispId(1), Description("The value of the lower bound.")] 
	double lowerBound { get; }

	/// <summary>上限的值。</summary>
	/// <remarks>这为用户提供了参数的可接受上限。</remarks>
	/// <value>参数的上限。</value>
	[DispId(2), Description("The value of the upper bound.")] 
	double upperBound { get; }

	/// <summary>导致错误的当前值。</summary>
	/// <remarks>这为用户提供了导致错误条件的原因。</remarks>
	/// <value>导致错误条件出现的值。</value>
	[DispId(3), Description("The current value which has led to an error..")] 
	double value { get; }

	/// <summary>值的类型/性质。</summary>
	/// <remarks>该值可能代表热力学性质、数据库中的表数量、内存容量等。该值的类型/性质。</remarks>
	/// <value>一个字符串，用于指示所需值的属性或类型。</value>
	[DispId(4), Description("The type/nature of the value. The value could represent a thermodynamic property, a number of tables in a database, a quantity of memory, ...")] 
	string type { get; }
}


/// <summary>CAPE-OPEN 异常接口的根类。</summary>
/// <remarks>CAPE-OPEN 错误层次结构的界面。系统包和 ECapeUser 界面依赖于此错误。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeRoot_IID)]
[Description("ECapeRoot Interface")]
public interface ECapeRoot
{
	/// <summary>错误的名称。这是一个必填字段。</summary>
	/// <remarks>错误的名称。这是一个必填字段。</remarks>
	/// <value>错误的名称。这是一个必填字段。</value>
	[DispId(1), Description("Error Name")] 
	string Name { get; }
}

// ECapeUser interface
/// <summary>CO 错误层次结构的基础接口。</summary>
/// <remarks>ECapeUser 界面定义了 CO 错误的最低状态。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeUser_IID)]
[Description("ECapeUser Interface")]
public interface ECapeUser 
{	
	/// <summary>用于指定错误子类别的代码。</summary>
	/// <remarks><para>该错误代码被用作 COM 调用模式中的函数返回值 HRESULT。当基于 .Net 的组件抛出异常时，
	/// 分配给该异常的 HRESULT 会返回给基于 COM 的调用者。设置异常 HRESULT 值，
	/// 以便向 COM 调用者提供 HRESULT 信息，这一点非常重要。</para>
	/// <para>值的分配由每个实现自行决定。因此，这是一段专用于 CO 组件提供者的私有代码。默认情况下，
	/// 它被设置为 CAPE-OPEN 错误 HRESULT <see cref="CapeErrorInterfaceHR"/>。</para></remarks>
	/// <value>异常的 HRESULT 值。</value>
	[DispId(1), Description("Code to designate the subcategory of the error. The assignment of values is left to each implementation. So that is a proprietary code specific to the CO component provider.")] 
	int code { get; }

	/// <summary>错误的描述。</summary>
	/// <remarks>错误描述可以包括对导致错误的条件的更详细描述。</remarks>
	/// <value>异常的字符串描述。</value>
	[DispId(2), Description("The description of the error.")] 
	string description { get; }

	/// <summary>错误的范围。</summary>
	/// <remarks>该属性提供了一份错误发生位置的包列表，各包之间用“.”分隔。例如，CapeOpen.Common.Identification。</remarks>
	/// <value>错误的来源。</value>
	[DispId(3), Description("The scope of the error. The list of packages where the error occurs separated by '.'. For example CapeOpen.Common.Identification.")] 
	string scope { get; }

	/// <summary>发生错误的接口名称。此为必填字段。</summary>
	/// <remarks>发生错误的接口。</remarks>
	/// <value>接口的名称。</value>
	[DispId(4), Description("The name of the interface where the error is thrown. This is a mandatory field.")] 
	string interfaceName { get; }

	/// <summary>发生错误的操作名称。此为必填字段。</summary>
	/// <remarks>此字段显示异常发生时正在执行的操作的名称。</remarks>
	/// <value>操作名称。</value>
	[DispId(5), Description("The name of the operation where the error is thrown. This is a mandatory field.")] 
	string operation { get; }

	/// <summary>指向包含更多错误信息页面的 URL，该页面、文档或网站可提供有关错误的详细信息。此信息的内容显然取决于具体实现。</summary>
	/// <remarks>此字段提供了一个互联网网址，通过该网址可以获取有关此错误的更多信息。</remarks>
	/// <value>URL 地址。</value>
	[DispId(6), Description("A URL to a page, document, website, where more information on the error can be found. The content of this information is obviously implementation dependent.")] 
	string moreInfo
	{ get; }
}

// ECapeUnknown interface
/// <summary>当操作指定的其他错误不适用时，将引发此异常。</summary>
/// <remarks>一种标准的异常，可由 CAPE-OPEN 对象抛出，以表示发生的错误不属于该对象所支持的任何其他异常类型。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeUnknown_IID)]
[Description("ECapeUnknown Interface")]
public interface ECapeUnknown;

// ECapeData interface
/// <summary>与任何数据相关的错误层次结构的基础接口。</summary>
/// <remarks>ECapeDataException 接口是与数据相关的错误的基础接口。数据是操作的参数，
/// 来自参数通用接口的参数，以及关于许可证密钥的信息。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeData_IID)]
[Description("ECapeData Interface")]
public interface ECapeData;

// ECapeLicenceError interface
/// <summary>由于未遵守许可协议，该操作无法完成。</summary>
/// <remarks>当然，这种错误也可能出现在 CO 的作用范围之外。在这种情况下，该错误不属于 CO 的错误处理范畴，而是特定于该平台。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeLicenceError_IID)]
[Description("ECapeLicenceError Interface")]
public interface ECapeLicenceError;

// ECapeBadCOParameter interface
/// <summary>一个参数（该参数属于参数通用接口）的状态无效。</summary>
/// <remarks>无效参数的名称以及参数本身均可从异常中获取。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeBadCOParameter_IID)]
[Description("ECapeBadCOParameter Interface")]
public interface ECapeBadCOParameter 
{
	/// <summary>引发异常的 CO 参数的名称。</summary>
	/// <remarks>这提供了引发异常的参数的名称。</remarks>
	/// <value>引发异常的参数名称。</value>
	[DispId(1), Description("The name of the CO parameter")] 
	string parameterName { get; }
	
	/// <summary>引发异常的参数。</summary>
	/// <remarks>此方法可直接访问引发异常的参数。</remarks>
	/// <value>对异常的引用抛出了异常。</value>
	[DispId(2), Description("The parameter")] 
	object parameter { get; }
}

/// <summary>传递的参数值无效。</summary>
/// <remarks>函数调用包含无效的参数值。例如，过渡的相态名称不属于 CO 相态列表。</remarks>
[ComImport, ComVisible(false)]
[Guid("678c0b16-7d66-11d2-a67d-00105a42887f")]
[Description("ECapeBadArgument Interface")]
public interface ECapeBadArgument093
{
	/// <summary>操作签名中参数值的位置。第一个参数的位置为 1。</summary>
	/// <remarks>这提供了函数调用中无效参数在参数列表中的位置。</remarks>
	/// <value>该论点的立场存在问题。第一个论点是 1。</value>
	[DispId(1), Description("The position of the argument value within the signature of the operation. First argument is as position 1.")] 
	int position { get; }
}

// ECapeBadArgument interface for CAPE-OPENv1.0
/// <summary>传递的参数值无效。</summary>
/// <remarks>函数调用包含无效的参数值。例如，过渡的阶段名称不属于 CO 阶段列表。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeBadArgument_IID)]
[Description("ECapeBadArgument Interface")]
public interface ECapeBadArgument 
{
	/// <summary>The position of the argument value within the signature of the operation. First argument is as position 1.</summary>
	/// <remarks>This provides the location of the invalid argument in the argument list for the function call.</remarks>
	/// <value>The position of the argument that is bad. The first argument is 1.</value>
	[DispId(1), Description("The position of the argument value within the signature of the operation. First argument is as position 1.")] 
	short position { get; }
}

// ECapeInvalidArgument interface
/// <summary>An invalid argument value was passed. For instance the passed name of 
/// the phase does not belong to the CO Phase List.</summary>
/// <remarks>An argument value of the operation is invalid. The position of the 
/// argument value within the signature of the operation. First argument is as 
/// position 1.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeInvalidArgument_IID)]
[Description("ECapeInvalidArgument Interface")]
public interface ECapeInvalidArgument {
};



// ECapeOutOfBounds interface
/// <summary>An argument value is outside of the bounds..</summary>
/// <remarks><para>This class is derived from the <see cref="ECapeBoundaries"/> interface.
/// It is used to indicate that one of the parameters is outside of its bounds.</para></remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeOutOfBounds_IID)]
[Description("ECapeOutOfBounds Interface")]
public interface ECapeOutOfBounds {};

// ECapeNoImpl interface
/// <summary>An exception that indicates that the requested operation has not been implemented by the current object.</summary>
/// <remarks>The operation is “not” implemented even if this operation can be called due 
/// to the compatibility with the CO standard. That is to say that the operation 
/// exists but it is not supported by the current implementation.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeNoImpl_IID)]
[Description("ECapeNoImpl Interface")]
public interface ECapeNoImpl {};



// ECapeLimitedImpl interface
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
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeLimitedImpl_IID)]
[Description("ECapeLimitedImpl Interface")]
public interface ECapeLimitedImpl {};

// ECapeImplementation interface
/// <summary>The base class of the errors hierarchy related to the current implementation.</summary>
/// <remarks>This class is used to indicate that an error occurred in the with the implementation of an object. 
/// The implemenation-related classes such as 
/// <see cref="ECapeNoImpl"/> and 
/// <see cref="ECapeLimitedImpl"/>
/// derive from this class.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeImplementation_IID)]
[Description("ECapeImplementation Interface")]
public interface ECapeImplementation {};

// ECapeOutOfResources interface
/// <summary>An exception that indicates that the resources required by this operation are not available.</summary>
/// <remarks>The physical resources necessary to the execution of the operation are out of limits.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeOutOfResources_IID)]
[Description("ECapeOutOfResources Interface")]
public interface ECapeOutOfResources {};


// ECapeNoMemory interface
/// <summary>An exception that indicates that the memory required for this operation is not available.</summary>
/// <remarks>The physical memory necessary to the execution of the operation is out of limit.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeNoMemory_IID)]
[Description("ECapeNoMemory Interface")]
public interface ECapeNoMemory {};



// ECapeTimeOut interface
/// <summary>Exception thrown when the time-out criterion is reached.</summary>
/// <remarks>Exception thrown when the time-out criterion is reached.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeTimeOut_IID)]
[Description("ECapeTimeOut Interface")]
public interface ECapeTimeOut {};


// ECapeFailedInitialisation interface
/// <summary>This exception is thrown when necessary initialisation has not been performed or has failed.</summary>
/// <remarks>The pre-requisites operations are not valid. The necessary initialisation has not been performed or has failed.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeFailedInitialisation_IID)]
[Description("ECapeFailedInitialisation Interface")]
public interface ECapeFailedInitialisation {};



// ECapeSolvingError interface
/// <summary>An exception that indicates a numerical algorithm failed for any reason.</summary>
/// <remarks>Indicates that a numerical algorithm failed for any reason.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeSolvingError_IID)]
[Description("ECapeSolvingError Interface")]
public interface ECapeSolvingError {};

// ECapeBadInvOrder interface
/// <summary>The necessary pre-requisite operation has not been called prior to the operation request.</summary>
/// <remarks>The specified prerequiste operation must be called prior to the operation throwing this exception.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeBadInvOrder_IID)]
[Description("ECapeBadInvOrder Interface")]
public interface ECapeBadInvOrder {
	/// <summary>The necessary prerequisite operation.</summary>
	[DispId(1), Description("The necessary prerequisite operation.")] 
	String requestedOperation
	{
		get;
	}
}

// ECapeInvalidOperation interface
/// <summary>This operation is not valid in the current context.</summary>
/// <remarks>This exception is thrown when an operation is attempted that is not valid in the current context.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeInvalidOperation_IID)]
[Description("ECapeInvalidOperation Interface")]
public interface ECapeInvalidOperation {};

// ECapeComputation interface
/// <summary>The base interface of the errors hierarchy related to calculations.</summary>
/// <remarks>This class is used to indicate that an error occurred in the performance of a calculation. 
/// Other calculation-related classes such as 
/// <see cref="ECapeFailedInitialisation"/>, 
/// <see cref="ECapeOutOfResources"/>, 
/// <see cref="ECapeSolvingError"/>, 
/// <see cref="ECapeBadInvOrder"/>, 
/// <see cref="ECapeInvalidOperation"/>, 
/// <see cref="ECapeNoMemory"/>, and 
/// <see cref="ECapeTimeOut"/> 
/// derive from this class.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeComputation_IID)]
[Description("ECapeComputation Interface")]
public interface ECapeComputation {};

// ECapePersistence interface
/// <summary>An exception that indicates that the a persistence-related error has occurred.</summary>
/// <remarks>The base of the errors hierarchy related to the persistence.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapePersistence_IID)]
[Description("ECapePersistence Interface")]
public interface ECapePersistence {};

// ECapeIllegalAccess interface
/// <summary>The access to something within the persistence system is not authorised.</summary>
/// <remarks>This exception is thrown when the access to something within the persistence system is not authorised.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeIllegalAccess_IID)]
[Description("ECapeIllegalAccess Interface")]
public interface ECapeIllegalAccess {};



// ECapePersistenceNotFound interface
/// <summary>An exception that indicates that the persistence was not found.</summary>
/// <remarks>The requested object, table, or something else within the persistence system was not found.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapePersistenceNotFound_IID)]
[Description("ECapePersistenceNotFound Interface")]
public interface ECapePersistenceNotFound {
	/// <summary>The name of the item.</summary>
	/// <remarks>The name of the requested object, table, or something else within the persistence system 
	/// that was not found.</remarks>
	/// <value>The name of the item not found.</value>
	[DispId(1), Description("The name of the item")] 
	String itemName
	{
		get;
	}

}

// ECapePersistenceSystemError interface
/// <summary>An exception that indicates a severe error occurred within the persistence system.</summary>
/// <remarks>During the persistence process, a severe error occurred within the persistence system.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapePersistenceSystemError_IID)]
[Description("ECapePersistenceSystemError Interface")]
public interface ECapePersistenceSystemError {};



// ECapePersistenceOverflow interface
/// <summary>An exception that indicates an overflow of internal persistence system.</summary>
/// <remarks>During the persistence process, an overflow of internal persistence system occurred.</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapePersistenceOverflow_IID)]
[Description("ECapePersistenceOverflow Interface")]
public interface ECapePersistenceOverflow {};

/// <summary>An exception that indicates the requested thermodynamic property was not available.</summary>
/// <remarks>At least one item in the requested properties cannot be returned. This could be 
/// because the property cannot be calculated at the specified conditions or for the 
/// specified Phase. If the property calculation is not implemented then 
/// <see cref="ECapeLimitedImpl"/> should be returned.</remarks>
[ComImport]
[ComVisible(false)]
[Guid("678C09B6-7D66-11D2-A67D-00105A42887F")]
[Description("ECapeThrmPropertyNotAvailable Interface")]
public interface ECapeThrmPropertyNotAvailable
{ };

/// <summary>Exception thrown when the Hessian for the MINLP problem is not available.</summary>
/// <remarks>Exception thrown when the Hessian for the MINLP problem is not available.</remarks>
[ComImport]
[ComVisible(false)]
[Guid("3FF0B24B-4299-4DAC-A46E-7843728AD205")]
[Description("ECapeHessianInfoNotAvailable Interface")]
public interface ECapeHessianInfoNotAvailable 
{
	/// <summary>Code to designate the subcategory of the error. </summary>
	/// <remarks>The assignment of values is left to each implementation. So that is a 
	/// proprietary code specific to the CO component provider. By default, set to 
	/// the CAPE-OPEN error HRESULT <see cref="CapeErrorInterfaceHR"/>.</remarks>
	/// <value>The HRESULT value for the exception.</value>
	[DispId(1), Description("Code to designate the subcategory of the error. The assignment of values is left to each implementation. So that is a proprietary code specific to the CO component provider.")] 
	int code
	{
		get;
	}

	/// <summary>The description of the error.</summary>
	/// <remarks>The error description can include a more verbose description of the condition that
	/// caused the error.</remarks>
	/// <value>A string description of the exception.</value>
	[DispId(2), Description("The description of the error.")] 
	String description
	{
		get;
	}

	/// <summary>The scope of the error.</summary>
	/// <remarks>This property provides a list of packages where the error occurs separated by '.'. 
	/// For example CapeOpen.Common.Identification.</remarks>
	/// <value>The source of the error.</value>
	[DispId(3), Description("The scope of the error. The list of packages where the error occurs separated by '.'. For example CapeOpen.Common.Identification.")] 
	String scope
	{
		get;
	}

	/// <summary>The name of the interface where the error is thrown. This is a mandatory field."</summary>
	/// <remarks>The interface that the error was thrown.</remarks>
	/// <value>The name of the interface.</value>
	[DispId(4), Description("The name of the interface where the error is thrown. This is a mandatory field.")] 
	String interfaceName
	{
		get;
	}

	/// <summary>The name of the operation where the error is thrown. This is a mandatory field.</summary>
	/// <remarks>This field provides the name of the operation being perfomed when the exception was raised.</remarks>
	/// <value>The operation name.</value>
	[DispId(5), Description("The name of the operation where the error is thrown. This is a mandatory field.")] 
	String operation
	{
		get;
	}

	/// <summary>An URL to a page, document, web site,  where more information on the error can be found. The content of this information is obviously implementation dependent.</summary>
	/// <remarks>This field provides an internet URL where more information about the error can be found.</remarks>
	/// <value>The URL.</value>
	[DispId(6), Description("An URL to a page, document, web site,  where more information on the error can be found. The content of this information is obviously implementation dependent.")] 
	String moreInfo
	{
		get;
	}
}
/// <summary>Exception thrown when the problem is outside the scope of the solver.</summary>
/// <remarks>Exception thrown when the problem is outside the scope of the solver.</remarks>
[ComImport]
[ComVisible(false)]
[Guid("678c0b0f-7d66-11d2-a67d-00105a42887f")]
[Description("ECapeOutsideSolverScope Interface")]
public interface ECapeOutsideSolverScope 
{
	/// <summary>Code to designate the subcategory of the error. </summary>
	/// <remarks>The assignment of values is left to each implementation. So that is a 
	/// proprietary code specific to the CO component provider. By default, set to 
	/// the CAPE-OPEN error HRESULT <see cref="CapeErrorInterfaceHR"/>.</remarks>
	/// <value>The HRESULT value for the exception.</value>
	[DispId(1), Description("Code to designate the subcategory of the error. The assignment of values is left to each implementation. So that is a proprietary code specific to the CO component provider.")] 
	int code
	{
		get;
	}

	/// <summary>The description of the error.</summary>
	/// <remarks>The error description can include a more verbose description of the condition that
	/// caused the error.</remarks>
	/// <value>A string description of the exception.</value>
	[DispId(2), Description("The description of the error.")] 
	String description
	{
		get;
	}

	/// <summary>The scope of the error.</summary>
	/// <remarks>This property provides a list of packages where the error occurs separated by '.'. 
	/// For example CapeOpen.Common.Identification.</remarks>
	/// <value>The source of the error.</value>
	[DispId(3), Description("The scope of the error. The list of packages where the error occurs separated by '.'. For example CapeOpen.Common.Identification.")] 
	String scope
	{
		get;
	}

	/// <summary>The name of the interface where the error is thrown. This is a mandatory field."</summary>
	/// <remarks>The interface that the error was thrown.</remarks>
	/// <value>The name of the interface.</value>
	[DispId(4), Description("The name of the interface where the error is thrown. This is a mandatory field.")] 
	String interfaceName
	{
		get;
	}

	/// <summary>The name of the operation where the error is thrown. This is a mandatory field.</summary>
	/// <remarks>This field provides the name of the operation being perfomed when the exception was raised.</remarks>
	/// <value>The operation name.</value>
	[DispId(5), Description("The name of the operation where the error is thrown. This is a mandatory field.")] 
	String operation
	{
		get;
	}

	/// <summary>An URL to a page, document, web site,  where more information on the error can be found. The content of this information is obviously implementation dependent.</summary>
	/// <remarks>This field provides an internet URL where more information about the error can be found.</remarks>
	/// <value>The URL.</value>
	[DispId(6), Description("An URL to a page, document, web site,  where more information on the error can be found. The content of this information is obviously implementation dependent.")] 
	String moreInfo
	{
		get;
	}
}

// typedef CapeErrorInterfaceHR eCapeErrorInterfaceHR;



/// <summary>The ECapeErrorDummy interface is not intended to be used. </summary>
/// <remarks>It is only here to ensure that 
/// the MIDL compiler exports the CapeErrorInterfaceHR enumeration. The compiler only exports 
/// an enumeration if it is used in a method of an exported interface. </remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ECapeErrorDummy_IID)]
[Description("ECapeErrorDummy Interface")]
public interface ECapeErrorDummy
{
	/// <summary>The HRESULT of the Dummy Error.</summary>
	/// <remarks>The HRESULT of the Dummy Error.</remarks>
	/// <value>The HRESULT of the Dummy Error.</value>
	[DispId(1), Description("property Name")] 
	int dummy
	{
		get;
	}

}