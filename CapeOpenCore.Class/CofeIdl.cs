using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>COFE 使用的流股类型枚举。</summary>
/// <remarks>此枚举提供了 COFE 使用的流股类型枚举。</remarks>
[Serializable]
[ComVisible(true)]
[Guid("D1B15843-C0F5-4CB7-B462-E1B80456808E")]
public enum COFEStreamType
{
	/// <summary>COFE 物流流股 Stream.</summary>
	STREAMTYPE_MATERIAL = 0,
	/// <summary>COFE 能量流股 Stream.</summary>
	STREAMTYPE_ENERGY = 1,
	/// <summary>COFE 信息流股 Stream.</summary>
	STREAMTYPE_INFORMATION = 2
}

/// <summary>COFE 流股接口。</summary>
/// <remarks>由 COFE 流对象实现的接口。</remarks>
[ComImport, ComVisible(true)]
[Guid("B2A15C45-D878-4E56-A19A-DED6A6AD6F91")]
[Description("ICOFEStream Interface")]
public interface ICOFEStream 
{
	/// <summary>来自 COFE 的流股类型。</summary>
	/// <remarks><para>获取 COFE 流股的类型。它有三个可能的值：</para>
	/// <para>(1) MATERIAL</para>
	/// <para>(2) ENERGY</para>
	/// <para>(3) INFORMATION</para></remarks>
	/// <value>来自 COFE 的流股类型。</value>
	[DispId(1)] 
	COFEStreamType StreamType { get; }

	/// <summary>COFE 流股上游的单元操作。</summary>
	/// <remarks>获取当前流股上游的单元操作。</remarks>
	/// <value>当前流股上游的单元操作。</value>
	[DispId(2)]
	object UpstreamUnit { [return: MarshalAs(UnmanagedType.IDispatch)] get; }

	/// <summary>COFE 流股下游的单元操作。</summary>
	/// <remarks>获取当前流股下游的单元操作。</remarks>
	/// <value>当前流股下游单元操作。</value>
	[DispId(3)]
	object DownstreamUnit { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
}

/// <summary>COFE 物流对象接口。</summary>
/// <remarks>由 COFE 物流对象实现的接口。</remarks>
[ComImport, ComVisible(true)]
[Guid("2BFFCBD3-7DAB-439D-9E25-FBECC8146BE8")]
[Description("ICOFEMaterial Interface")]
public interface ICOFEMaterial
{
	/// <summary>COFE 使用的物流对象类型。</summary>
	/// <remarks>该方法提供 COFE 所使用的材料类型。</remarks>
	[DispId(1)] 
	string MaterialType { get; }

	/// <summary>该物流对象在 COFE 中支持的相态列表。</summary>
	/// <remarks>该物流对象在 COFE 中支持的相态列表。</remarks>
	[DispId(2)] 
	object GetSupportedPhaseList();
}

/// <summary>COFE 单元模块图标接口。</summary>
/// <remarks>由 COFE 单元模块图标实现的接口。</remarks>
[ComImport, ComVisible(true)]
[Guid("5F6333E0-434F-4C03-85E2-6EB493EAE846")]
[Description("ICOFEIcon Interface")]
internal interface ICOFEIcon
{
	/// <summary>该单元模块在 COFE 中的图标。</summary>
	/// <remarks>该单元模块在 COFE 中的图标的文件名称。</remarks>
	[DispId(1)]
	void SetUnitOperationIcon(string iconFileName);
}
