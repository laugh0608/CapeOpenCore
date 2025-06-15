/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.15
 */

/* IMPORTANT NOTICE
(c) The CAPE-OPEN Laboratory Network, 2002.
All rights are reserved unless specifically stated otherwise

Visit the web site at www.colan.org

This file has been edited using the editor from Microsoft Visual Studio 6.0
This file can view properly with any basic editors and browsers (validation done under MS Windows and Unix)
*/

// This file was developed/modified by JEAN-PIERRE-BELAUD for CO-LaN organisation - August 2003

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

// -------------------------------------------------------------------
// ---- The scope of the COSE interfaces -----------------------------
// -------------------------------------------------------------------

/// <summary>包含诊断功能。</summary>
/// <remarks>PME 需要支持的接口，以便将 ICapeUtilities:SetSimulation 的引用
/// 传递给 PMC。PMC 随后可以使用 PME 的任何 COSE 接口。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ICapeSimulationContext_IID)]
[Description("ICapeSimulation Context Interface")]
public interface ICapeSimulationContext
{
	// 此处暂未定义任何方法...
}

// .NET 版本的 ICapeDiagnostic 接口。
/// <summary>提供了一种机制，用于向用户显示详细的错误信息。</summary>
/// <remarks>PMC 向 PME（进而向用户）传递冗余信息的过程。PMC 在执行流程图时应能够向用户记录或显示信息。与其
/// 让每个 PMC 通过不同机制执行这些任务，不如将所有任务统一 redirect 至 PME 服务以实现与用户的通信。错误通用
/// 接口无法满足这些要求，因为它们会停止 PMC 代码的执行并向 PME 发出异常情况信号。本文件涉及简单信息或警告消息的传输。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ICapeDiagnostic_IID)]
[Description("ICapeDiagnostic Interface")]
public interface ICapeDiagnostic
{
	/// <summary>向终端写入一条消息。</summary>
	/// <remarks><para>向终端写入一个字符串。</para>
	/// <para>当需要将一条消息提示给用户时，会调用此方法。实现时应确保该字符串被显示在对话框或用户易于查看的消息列表中。</para>
	/// <para>从原则上讲，这条消息必须尽快显示给用户。</para></remarks>
	/// <param name="message">要显示的文本。</param>
	/// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
	/// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
	[DispId(1)]
	[Description("Method PopUpMessage")] 
	void PopUpMessage(string message);

	/// <summary>将字符串写入 PME 的日志文件。</summary>
	/// <remarks><para>将字符串写入日志。</para>
	/// <para>当需要将消息记录到日志中时，会调用此方法。实现应将字符串写入日志文件或其他日志记录设备。</para></remarks>
	/// <param name="message">要记录的文本。</param>
	/// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
	/// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
	[DispId(2)]
	[Description("Method LogMessage")] 
	void LogMessage(string message);
}

/// <summary>创建指定类型的全新热力学物流模板。</summary>
/// <remarks>当单元操作需要进行热力学计算时，通常会在连接到单元端口的物流对象上执行这些计算。然而，在某些情况下，
/// 例如在精馏塔中，可能需要使用不同的属性包。甚至可能要求用户选择必须使用的热力学模型。访问 CAPE-OPEN 属性包的
/// 所有机制已集成在 COSE 中，作为使用 CAPE-OPEN 属性包所需功能的一部分。因此，与每个 PMC 实现对热力引擎选择
/// 和创建的支持相比，将此责任委托给 COSE 将导致更轻量级且更易于编码的单元操作组件。如果物流模板的配置在 PME 侧，
/// 单元操作所需的唯一额外功能是访问已配置的物流模板列表并选择其中一个。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ICapeMaterialTemplateSystem_IID)]
[Description("ICapeMaterialTemplateSystem Interface")]
public interface ICapeMaterialTemplateSystem
{
	/// <summary>创建指定类型的全新热力学物流模板。</summary>
	/// <remarks>当单元操作需要进行热力学计算时，通常会在连接到单元端口的物流对象上执行这些计算。然而，在某些情况下，
	/// 例如在精馏塔中，可能需要使用不同的属性包。甚至可能要求用户选择必须使用的热力学模型。访问 CAPE-OPEN 属性包的
	/// 所有机制已集成在 COSE 中，作为使用 CAPE-OPEN 属性包所需功能的一部分。因此，与每个 PMC 实现对热力引擎选择
	/// 和创建的支持相比，将此责任委托给 COSE 将导致更轻量级且更易于编码的单元操作组件。如果物流模板的配置在 PME 侧，
	/// 单元操作所需的唯一额外功能是访问已配置的物流模板列表并选择其中一个。</remarks>
	/// <value>返回 COSE 支持的材料模板名称的字符串数组。这可能包括：
	/// <list type="bullet"><item>CAPE-OPEN 独立属性包，</item>
	/// <item>CAPE-OPEN 属性包依赖于属性系统，</item>
	/// <item>原生于 COSE 的属性包。</item>
	/// </list></value>
	/// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
	/// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
	[DispId(1)]
	[Description("Property MaterialTemplates")] 
	object MaterialTemplates { get; }

	/// <summary>创建指定类型的全新热力学物流模板。</summary>
	/// <remarks>当单元操作需要进行热力学计算时，通常会在连接到单元端口的物流对象上执行这些计算。然而，在某些情况下，
	/// 例如在精馏塔中，可能需要使用不同的属性包。甚至可能要求用户选择必须使用的热力学模型。访问 CAPE-OPEN 属性包的
	/// 所有机制已集成在 COSE 中，作为使用 CAPE-OPEN 属性包所需功能的一部分。因此，与每个 PMC 实现对热力引擎选择
	/// 和创建的支持相比，将此责任委托给 COSE 将导致更轻量级且更易于编码的单元操作组件。如果物流模板的配置在 PME 侧，
	/// 单元操作所需的唯一额外功能是访问已配置的物流模板列表并选择其中一个。</remarks>
	/// <returns>返回 COSE 支持的材料模板名称的字符串数组。这可能包括：
	/// <para>1. CAPE-OPEN 独立属性包，</para>
	/// <para>2. CAPE-OPEN 属性包依赖于属性系统，</para>
	/// <para>3. 原生于 COSE 的属性包。</para></returns>
	/// <param name="materialTemplateName">要解析的材料模板的名称（该名称必须包含在 MaterialTemplates 方法返回的列表中）</param>
	/// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
	/// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
	[DispId(2)]
	[Description("Method CreateMaterialTemplate")]
	[return: MarshalAs(UnmanagedType.IDispatch)] 
	object CreateMaterialTemplate(string materialTemplateName) ;
}

// .NET 版本的 ICapeCOSEUtilities 接口。
/// <summary>为 PMC 提供了一种机制，使其能够从 PME 获取一个自由的 FORTRAN 通道。</summary>
/// <remarks>当 PMC 封装 FORTRAN DLL时，如果 PMC 与 PME（如模拟器执行）在同一进程中加载，可能会出现技术问题。
/// 在这种情况下，如果两个 FORTRAN 模块选择相同的输出通道用于 FORTRAN 消息传递，可能会发生冲突。因此，PME 应为
/// 每个可能需要输出通道的 PMC 集中生成唯一的输出通道。此要求仅在 PME 和 PMC 属于同一计算进程时出现，显然
/// 此 FORTRAN 通道功能仅适用于非分布式架构。鉴于未来可能需要此类信息交换，必须建立一个通用且可扩展的机制。
/// 调用模式是一个合适的候选方案。因此，FORTRAN 通道的特定字符串值应被标准化。</remarks>
[ComImport, ComVisible(false)]
[Guid(COGuids.ICapeCOSEUtilities_IID)]
[Description("ICapeCOSEUtilities Interface")]
public interface ICapeCOSEUtilities
{
	/// <summary>PME 支持的命名值列表。</summary>
	/// <remarks>PME 提供的命名值列表。</remarks>
	/// <value>COSE 支持的命名值字符串数组列表。该列表中应包含名为 FreeFORTRANChannel 的命名值，
	/// 该值将提供自由 FORTRAN 通道的名称。</value>
	/// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
	[DispId(1)]
	[Description("Property NamedValueList")] 
	object NamedValueList { get; }

	/// <summary>返回与请求名称对应的值，包括一个自由的 FORTRAN 通道。</summary>
	/// <remarks><para>返回与名称为 name 的值对应的值。请注意，连续两次调用并传入相同名称可能返回不同的值。</para>
	/// <para>每次调用 FreeFORTRANChannel NamedValue 属性时，COSE 都会返回一个不同的 FORTRAN 通道。
	/// COSE 不得将返回的任何 FORTRAN 通道用于任何内部使用的 FORTRAN 模块。</para></remarks>
	/// <returns>请求的值的名称（该值必须包含在 NamedValueList 返回的列表中）。</returns>
	/// <param name="value">请求的值的名称（该名称必须包含在由 <see cref="ICapeCOSEUtilities.NamedValueList"/> 返回的列表中）。</param>
	/// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
	/// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
	[DispId(2)]
	[Description("Property NamedValue")]
	object NamedValue(string value);
}
