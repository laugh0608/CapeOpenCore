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
/// <remarks>函数调用包含无效的参数值。例如，过渡的相态名称不属于 CO 相态列表。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeBadArgument_IID)]
[Description("ECapeBadArgument Interface")]
public interface ECapeBadArgument 
{
	/// <summary>操作签名中参数值的位置。第一个参数的位置为 1。</summary>
	/// <remarks>这提供了函数调用中无效参数在参数列表中的位置。</remarks>
	/// <value>该论点的立场存在问题。第一个论点是1。</value>
	[DispId(1), Description("The position of the argument value within the signature of the operation. First argument is as position 1.")] 
	short position { get; }
}

// ECapeInvalidArgument interface
/// <summary>传递的参数值无效。例如，所传递的相态名称并不属于 CO 相态列表。</summary>
/// <remarks>该操作的参数值无效。参数值在操作签名中的位置。第一个参数位于位置 1。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeInvalidArgument_IID)]
[Description("ECapeInvalidArgument Interface")]
public interface ECapeInvalidArgument;

// ECapeOutOfBounds interface
/// <summary>参数值超出范围。</summary>
/// <remarks>此类继承自 <see cref="ECapeBoundaries"/> 接口。它用于表示其中一个参数超出了其范围。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeOutOfBounds_IID)]
[Description("ECapeOutOfBounds Interface")]
public interface ECapeOutOfBounds;

// ECapeNoImpl interface
/// <summary>一个异常，表示当前对象尚未实现所请求的操作。</summary>
/// <remarks>即使由于与 CO 标准兼容，可以调用该操作，但该操作“未被”实现。也就是说，该操作存在，但当前实现并不支持它。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeNoImpl_IID)]
[Description("ECapeNoImpl Interface")]
public interface ECapeNoImpl;

// ECapeLimitedImpl interface
/// <summary>实现的限制已被违反。</summary>
/// <remarks><para>一项操作可以部分实现，例如，一个“属性包”可以实现 TP 闪蒸，但不支持 PH 闪蒸。
/// 如果调用方请求执行 PH 闪蒸操作，则该错误表明，虽然支持某些闪蒸计算，但不支持请求的操作。</para>
/// <para>该工厂只能创建单个实例（因为该组件是评估副本），当调用者请求创建第二个实例时，这个错误表明该实现存在限制。</para></remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeLimitedImpl_IID)]
[Description("ECapeLimitedImpl Interface")]
public interface ECapeLimitedImpl;

// ECapeImplementation interface
/// <summary>与当前实现相关的错误层次结构的基类。</summary>
/// <remarks>此类用于指示在与某个对象的实现相关时发生了错误。与实现相关的类，
/// 如 <see cref="ECapeNoImpl"/> 和 <see cref="ECapeLimitedImpl"/> 都继承自此类。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeImplementation_IID)]
[Description("ECapeImplementation Interface")]
public interface ECapeImplementation;

// ECapeOutOfResources interface
/// <summary>一个异常，表示此操作所需的资源不可用。</summary>
/// <remarks>执行该操作所需的物理资源已超出限制。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeOutOfResources_IID)]
[Description("ECapeOutOfResources Interface")]
public interface ECapeOutOfResources;

// ECapeNoMemory interface
/// <summary>一个异常，表示执行此操作所需的内存不可用。</summary>
/// <remarks>执行该操作所需的物理内存已超出限制。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeNoMemory_IID)]
[Description("ECapeNoMemory Interface")]
public interface ECapeNoMemory;

// ECapeTimeOut interface
/// <summary>当超时条件满足时抛出异常。</summary>
/// <remarks>当超时条件满足时抛出异常。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeTimeOut_IID)]
[Description("ECapeTimeOut Interface")]
public interface ECapeTimeOut;

// ECapeFailedInitialisation interface
/// <summary>当必要的初始化操作未执行或执行失败时，将抛出此异常。</summary>
/// <remarks>先决条件操作无效。必要的初始化操作未执行或执行失败。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeFailedInitialisation_IID)]
[Description("ECapeFailedInitialisation Interface")]
public interface ECapeFailedInitialisation;

// ECapeSolvingError interface
/// <summary>一个异常，表示数值算法因任何原因失败。</summary>
/// <remarks>表示数值算法因任何原因失败。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeSolvingError_IID)]
[Description("ECapeSolvingError Interface")]
public interface ECapeSolvingError;

// ECapeBadInvOrder interface
/// <summary>在执行操作请求之前，未调用必要的先决条件操作。</summary>
/// <remarks>指定的先决条件操作必须在引发此异常的操作之前被调用。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeBadInvOrder_IID)]
[Description("ECapeBadInvOrder Interface")]
public interface ECapeBadInvOrder 
{
	/// <summary>必要的先决条件操作。</summary>
	[DispId(1), Description("The necessary prerequisite operation.")] 
	string requestedOperation { get; }
}

// ECapeInvalidOperation interface
/// <summary>此操作在当前上下文中无效。</summary>
/// <remarks>当尝试执行在当前上下文中无效的操作时，将抛出此异常。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeInvalidOperation_IID)]
[Description("ECapeInvalidOperation Interface")]
public interface ECapeInvalidOperation;

// ECapeComputation interface
/// <summary>与计算相关的错误层次结构的基础接口。</summary>
/// <remarks>此类用于指示在执行计算时发生错误。其他与计算相关的类，如 <see cref="ECapeFailedInitialisation"/>, 
/// <see cref="ECapeOutOfResources"/>、<see cref="ECapeSolvingError"/>、
/// <see cref="ECapeBadInvOrder"/>、<see cref="ECapeInvalidOperation"/>、
/// <see cref="ECapeNoMemory"/> 和 <see cref="ECapeTimeOut"/> 均继承自本类。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeComputation_IID)]
[Description("ECapeComputation Interface")]
public interface ECapeComputation;

// ECapePersistence interface
/// <summary>一个异常，表示发生了与持久化相关的错误。</summary>
/// <remarks>与持久性相关的错误层次结构的基础。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapePersistence_IID)]
[Description("ECapePersistence Interface")]
public interface ECapePersistence;

// ECapeIllegalAccess interface
/// <summary>对持久化系统中的某项资源的访问未获授权。</summary>
/// <remarks>当对持久化系统中的某项资源的访问未获得授权时，将抛出此异常。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ECapeIllegalAccess_IID)]
[Description("ECapeIllegalAccess Interface")]
public interface ECapeIllegalAccess;

// ECapePersistenceNotFound interface
/// <summary>一个异常，表示未找到持久化对象。</summary>
/// <remarks>请求的对象、表或其他持久化系统中的内容未找到。</remarks>
[ComImport,ComVisible(false)]
[Guid(COGuids.ECapePersistenceNotFound_IID)]
[Description("ECapePersistenceNotFound Interface")]
public interface ECapePersistenceNotFound 
{
	/// <summary>该项的名称。</summary>
	/// <remarks>请求的对象、表或持久系统内其他相关项目的名称未找到。</remarks>
	/// <value>未找到该项的名称。</value>
	[DispId(1), Description("The name of the item.")] 
	string itemName { get; }
}

// ECapePersistenceSystemError interface
/// <summary>一个异常，表示持久化系统中发生了严重错误。</summary>
/// <remarks>在持久化过程中，持久化系统中发生了严重错误。</remarks>
[ComImport,ComVisible(false)]
[Guid(COGuids.ECapePersistenceSystemError_IID)]
[Description("ECapePersistenceSystemError Interface")]
public interface ECapePersistenceSystemError;

// ECapePersistenceOverflow interface
/// <summary>一个异常，表示内部持久化系统发生溢出。</summary>
/// <remarks>在持久化过程中，内部持久化系统发生了溢出。</remarks>
[ComImport,ComVisible(false)]
[Guid(COGuids.ECapePersistenceOverflow_IID)]
[Description("ECapePersistenceOverflow Interface")]
public interface ECapePersistenceOverflow;

/// <summary>一个异常，表示请求的热力学属性不可用。</summary>
/// <remarks>请求的属性中至少有一个项目无法返回。这可能是因为该属性无法在指定的条件下或在指定的相态中进行计算。
/// 如果该属性计算功能尚未实现，则应返回 <see cref="ECapeLimitedImpl"/>。</remarks>
[ComImport,ComVisible(false)]
[Guid("678C09B6-7D66-11D2-A67D-00105A42887F")]
[Description("ECapeThrmPropertyNotAvailable Interface")]
public interface ECapeThrmPropertyNotAvailable;

/// <summary>当 MINLP 问题的海森矩阵不可用时抛出异常。</summary>
/// <remarks>当 MINLP 问题的海森矩阵不可用时抛出异常。</remarks>
[ComImport,ComVisible(false)]
[Guid("3FF0B24B-4299-4DAC-A46E-7843728AD205")]
[Description("ECapeHessianInfoNotAvailable Interface")]
public interface ECapeHessianInfoNotAvailable 
{
	/// <summary>用于指定错误子类别的代码。</summary>
	/// <remarks>值的分配由每个实现自行决定。因此，这是一段专用于 CO 组件提供者的私有代码。默认情况下，
	/// 它被设置为 CAPE-OPEN 错误 HRESULT <see cref="CapeErrorInterfaceHR"/>。</remarks>
	/// <value>异常的 HRESULT 值。</value>
	[DispId(1), Description("Code to designate the subcategory of the error. The assignment of values is left to each implementation. So that is a proprietary code specific to the CO component provider.")] 
	int code
	{ get; }

	/// <summary>错误的描述。</summary>
	/// <remarks>错误描述可以包含对导致错误的条件更详细的说明。</remarks>
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
	/// <value>URL 链接。</value>
	[DispId(6), Description("A URL to a page, document, website, where more information on the error can be found. The content of this information is obviously implementation dependent.")] 
	string moreInfo { get; }
}

/// <summary>当问题超出求解器的处理范围时，将抛出异常。</summary>
/// <remarks>当问题超出求解器的处理范围时，将抛出异常。</remarks>
[ComImport,ComVisible(false)]
[Guid("678c0b0f-7d66-11d2-a67d-00105a42887f")]
[Description("ECapeOutsideSolverScope Interface")]
public interface ECapeOutsideSolverScope 
{
	/// <summary>用于指定错误子类别的代码。</summary>
	/// <remarks>值的分配由每个实现自行决定。因此，这是一段专用于 CO 组件提供者的私有代码。默认情况下，
	/// 它被设置为 CAPE-OPEN 错误 HRESULT <see cref="CapeErrorInterfaceHR"/>。</remarks>
	/// <value>异常的 HRESULT 值。</value>
	[DispId(1), Description("Code to designate the subcategory of the error. The assignment of values is left to each implementation. So that is a proprietary code specific to the CO component provider.")] 
	int code { get; }

	/// <summary>Error 的描述。</summary>
	/// <remarks>错误描述可以包括对导致错误的条件的更详细描述。</remarks>
	/// <value>异常的字符串描述。</value>
	[DispId(2), Description("The description of the error.")] 
	string description { get; }

	/// <summary>错误的范围。</summary>
	/// <remarks>该属性提供了一份错误发生位置的包列表，各包之间用“.”分隔。例如，CapeOpen.Common.Identification。</remarks>
	/// <value>错误的来源。</value>
	[DispId(3), Description("The scope of the error. The list of packages where the error occurs separated by '.'. For example CapeOpen.Common.Identification.")] 
	string scope { get; }

	/// <summary>抛出错误的接口名称。这是一个必填字段。</summary>
	/// <remarks>发生错误的接口。</remarks>
	/// <value>接口的名称。</value>
	[DispId(4), Description("The name of the interface where the error is thrown. This is a mandatory field.")] 
	string interfaceName { get; }

	/// <summary>发生错误的操作名称。此为必填字段。</summary>
	/// <remarks>此字段显示异常发生时正在执行的操作的名称。</remarks>
	/// <value>操作的名称。</value>
	[DispId(5), Description("The name of the operation where the error is thrown. This is a mandatory field.")] 
	string operation { get; }

	/// <summary>指向包含更多错误信息页面的 URL，该页面、文档或网站可提供有关错误的详细信息。此信息的内容显然取决于具体实现。</summary>
	/// <remarks>此字段提供了一个互联网网址，通过该网址可以获取有关此错误的更多信息。</remarks>
	/// <value>URL 链接。</value>
	[DispId(6), Description("A URL to a page, document, website, where more information on the error can be found. The content of this information is obviously implementation dependent.")] 
	string moreInfo { get; }
}

// typedef CapeErrorInterfaceHR eCapeErrorInterfaceHR;
/// <summary>ECapeErrorDummy 接口不应被使用。</summary>
/// <remarks>这里只是为了确保 MIDL 编译器会导出“CapeErrorInterfaceHR”枚举。
/// 只有当该枚举被用于导出接口的一个方法中时，编译器才会进行导出。</remarks>
[ComImport,ComVisible(false)]
[Guid(COGuids.ECapeErrorDummy_IID)]
[Description("ECapeErrorDummy Interface")]
public interface ECapeErrorDummy
{
	/// <summary>虚拟错误的HRESULT值。</summary>
	/// <remarks>虚拟错误的HRESULT值。</remarks>
	/// <value>虚拟错误的HRESULT值。</value>
	[DispId(1), Description("Property Name")] 
	int dummy { get; }
}
