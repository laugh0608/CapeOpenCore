/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.07.09
 */

/* IMPORTANT NOTICE
(c) AmsterCHEM, 2008.
All rights are reserved unless specifically stated otherwise
Visit the website at www.amsterchem.com or www.cocosimulator.org
*/
// This file was developed/modified by Jasper Van Baten

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/*****************************************
CAPE-OPEN Flowsheet monitoring interface
*****************************************/

/// <summary>指示被监控的流程图的解决方案状态。</summary>
/// <remarks>这一枚举为流程图监控对象提供了关于流程图解决方案状态的信息。</remarks>
[Serializable]
[ComVisible(false)]
[Guid("D1B15843-C0F5-4CB7-B462-E1B80456808E")]
public enum CapeSolutionStatus
{
    /// <summary>流程图求解无误。</summary>
    CAPE_SOLVED = 0,

    /// <summary>表示尚未尝试解决流程图。</summary>
    CAPE_NOT_SOLVED = 1,

    /// <summary>解决流程图的最后一次尝试没有收敛。</summary>
    CAPE_FAILED_TO_CONVERGE = 2,

    /// <summary>解决流程图的最后一次尝试超时了。</summary>
    CAPE_TIMED_OUT = 3,

    /// <summary>由于内存不足，最后一次尝试解决流程图失败。</summary>
    CAPE_NO_MEMORY = 4,

    /// <summary>解决流程图的最后一次尝试初始化失败。</summary>
    CAPE_FAILED_INITIALIZATION = 5,

    /// <summary>最后一次尝试解决流程图时出现了解决错误。</summary>
    CAPE_SOLVING_ERROR = 6,

    /// <summary>由于操作无效，最后一次解决流程图的尝试失败。</summary>
    CAPE_INVALID_OPERATION = 7,

    /// <summary>由于调用顺序无效，最后一次解决流程图的尝试失败。</summary>
    CAPE_BAD_INVOCATION_ORDER = 8,

    /// <summary>最后一次尝试解决流程图时出现了计算错误。</summary>
    CAPE_COMPUTATION_ERROR = 9
}

/// <summary>此接口提供有关值超出范围而导致的错误的信息。它可以被引发，以指示方法参数或对象参数的值超出范围。</summary>
/// <remarks><para>监测对象应落实：</para>
/// <list type="bullet">
/// <item>ICapeIdentification</item>
/// <item>持久化实现情况下的 IPersistStream 或 IPersistStreamInit</item>
/// <item>ICapeUtilities 用于参数收集和编辑以及接受 ICapeSimulationContext</item>
/// <item>CAPE-OPEN 错误处理接口（ECape...）</item>
/// </list>
/// <para>监控对象可以通过 ICapeSimulationContext 接口访问以下接口：</para>
/// <list type="bullet">
/// <item>ICapeCOSEUtilities - 对于命名值</item>
/// <item>ICapeDiagnostic - 用于记录和弹出消息</item>
/// <item>ICapeMaterialTemplateSystem - 用于访问物流模板和创建物流对象</item>
/// <item>ICapeFlowsheetMonitoring - 用于访问流股和单元操作的集合</item>
/// </list>
/// <para>监控对象不应该通过以下方式更改流程图配置：</para>
/// <list type="bullet">
/// <item>修改单元操作参数</item>
/// <item>将流股连接或断开到单元操作</item>
/// <item>计算单元操作</item>
/// <item>通过设置值来修改流股</item>
/// <item>任何其他会改变流程图状态的操作</item>
/// </list>
/// <para>监控对象可以：</para>
/// <list type="bullet">
/// <item>获取流股信息</item>
/// <item>获取单元操作信息</item>
/// <item>用于执行热力学计算的流股的重复物流对象</item>
/// <item>通过 ICapeMaterialTemplateSystem 创建物流对象，用于创建热力学计算</item>
/// <item>...</item>
/// </list>
/// <para>除了 CAPE-OPEN 对象类别 ID 之外，监控对象还应公开监控对象类别 ID：</para>
/// <para>CATID_MONITORING_OBJECT = 7BA1AF89-B2E4-493d-BD80-2970BF4CBE99</para></remarks>
[ComImport]
[ComVisible(false)]
[Guid("834F65CC-29AE-41c7-AA32-EE8B2BAB7FC2")]
[Description("ICapeFlowsheetMonitoring Interface")]
public interface ICapeFlowsheetMonitoring
{
    /// <summary>获取流股集合。</summary>
    /// <remarks>获取流股集合，返回一个 ICapeCollection 对象，枚举每个流股，每个流股通过
    /// ICapeIdentification 标识，物流暴露 ICapeThermoMaterial 或 ICapeThermoMaterialObject
    /// 能量流和物料流暴露 ICapeCollection，集合中的每个项目都是 ICapeParameter 对象。</remarks>
    /// <returns>单元操作的 <see cref="ICapeCollection"/> 集合。</returns>
    [DispId(1)]
    [Description("Method GetStreamCollection.")]
    [return: MarshalAs(UnmanagedType.IDispatch)]
    object GetStreamCollection();

    /// <summary>获取单元操作集合。</summary>
    /// <remarks>获取单元操作集合返回枚举单元操作的 ICapeCollection 对象，每个单元操作通过
    /// ICapeIdentification 标识，单元操作还暴露了 ICapeUnit 和可能的 ICapeUtilities（用于参数访问）。</remarks>
    /// <returns>流股的 <see cref="ICapeCollection"/> 集合。</returns>
    [DispId(2)]
    [Description("Method GetUnitOperationCollection")]
    [return: MarshalAs(UnmanagedType.IDispatch)]
    object GetUnitOperationCollection();

    /// <summary>获取流程图的当前解决方案状态。</summary>
    /// <remarks>获取流程图的 <see cref="CapeSolutionStatus"/>。</remarks>
    /// <value>流程图的 <see cref="CapeSolutionStatus"/>。</value>
    [DispId(3)]
    [Description("Solution status.")]
    CapeSolutionStatus SolutionStatus { get; }

    /// <summary>获取流程图的验证状态。</summary>
    /// <remarks>获取流程图的 <see cref="CapeValidationStatus"/>。</remarks>
    /// <returns>流程图的 <see cref="CapeValidationStatus"/>。</returns>
    [DispId(4)]
    [Description("Get the flowsheet validation status.")]
    CapeValidationStatus ValStatus { get; }
}

/// <summary>该接口提供因数值超出其范围而产生的错误信息。当方法参数或对象参数的值超出范围时，就会引发该错误。</summary>
/// <remarks><para>监控对象应执行：</para>
/// <list type="bullet">
/// <item>ICapeIdentification</item>
/// <item>IPersistStream 或 IPersistStreamInit（如果是持久化实现）</item>
/// <item>用于参数收集和编辑以及接受 ICapeSimulationContext 的 ICapeUtilities</item>
/// <item>CAPE-OPEN 错误处理接口（ECape...）</item>
/// </list>
/// <para>监控对象可以通过 ICapeSimulationContext 接口访问以下接口：</para>
/// <list type="bullet">
/// <item>ICapeCOSEUtilities - 用于命名值</item>
/// <item>ICapeDiagnostic - 用于记录和弹出消息</item>
/// <item>ICapeMaterialTemplateSystem - 用于访问物流模板和创建物流对象</item>
/// <item>ICapeFlowsheetMonitoring - 用于访问流股集合和单元操作</item>
/// </list>
/// <para>监控对象不应通过以下方式更改流量表配置：</para>
/// <list type="bullet">
/// <item>修改单元操作参数</item>
/// <item>将流股连接或断开到单元操作</item>
/// <item>计算单元操作</item>
/// <item>通过设置值来修改流股</item>
/// <item>任何其他会改变流程图状态的操作</item>
/// </list>
/// <para>监控对象可以：</para>
/// <list type="bullet">
/// <item>获取流股信息</item>
/// <item>获取单元操作信息</item>
/// <item>用于执行热力学计算的流股的重复物流对象</item>
/// <item>通过 ICapeMaterialTemplateSystem 创建物流对象，用于创建热力学计算</item>
/// <item>...</item>
/// </list>
/// <para>除了 CAPE-OPEN 对象类别 ID 之外，监控对象还应公开监控对象类别 ID：</para>
/// <para>CATID_MONITORING_OBJECT = 7BA1AF89-B2E4-493d-BD80-2970BF4CBE99</para></remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeFlowsheetMonitoring_IID)]
[Description("ICapeFlowsheetMonitoring Interface")]
public interface ICapeFlowsheetMonitoringOld
{
    /// <summary>获取流股集合。</summary>
    /// <summary>获取流股集合。</summary>
    /// <remarks>获取流股集合，返回一个 ICapeCollection 对象，枚举每个流股，每个流股通过
    /// ICapeIdentification 标识，物流暴露 ICapeThermoMaterial 或 ICapeThermoMaterialObject
    /// 能量流和物料流暴露 ICapeCollection，集合中的每个项目都是 ICapeParameter 对象。</remarks>
    /// <returns>单元操作的 <see cref="ICapeCollection"/> 集合。</returns>
    [DispId(1)]
    [Description("Method GetStreamCollection.")]
    [return: MarshalAs(UnmanagedType.IDispatch)]
    object GetStreamCollection();

    /// <summary>获取单元操作集合。</summary>
    /// <remarks>获取单元操作集合返回枚举单元操作的 ICapeCollection 对象，每个单元操作通过
    /// ICapeIdentification 标识，单元操作还暴露了 ICapeUnit 和可能的 ICapeUtilities（用于参数访问）。</remarks>
    /// <returns>流股的 <see cref="ICapeCollection"/> 集合。</returns>
    [DispId(2)]
    [Description("Method GetUnitOperationCollection")]
    [return: MarshalAs(UnmanagedType.IDispatch)]
    object GetUnitOperationCollection();

    /// <summary>检查流程图当前是否已解决。</summary>
    /// <remarks>检查流程图当前是否已解决。</remarks>
    /// <returns>true，表示单元已解决；false，表示单元未解决。</returns>
    [DispId(3)]
    [Description("Method IsSolved")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool IsSolved();

    /// <summary>检查流程图是否有效。</summary>
    /// <remarks>检查流程图是否有效。</remarks>
    /// <value>流程表的验证状态。</value>
    [DispId(4)]
    [Description("Get the flowsheet validation status.")]
    CapeValidationStatus ValStatus { get; }
}