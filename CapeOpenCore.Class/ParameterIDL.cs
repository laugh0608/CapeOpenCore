/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.08.02
 */

/* IMPORTANT NOTICE
(c) The CAPE-OPEN Laboratory Network, 2002.
All rights are reserved unless specifically stated otherwise

Visit the web site at www.colan.org

This file has been edited using the editor from Microsoft Visual Studio 6.0
This file can view properly with any basic editors and browsers (validation done under MS Windows and Unix)
*/

// This file was developed/modified by JEAN-PIERRE-BELAUD for CO-LaN organisation - March 2003

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>获取此规范所对应的参数类型： </summary>
/// <remarks><para>double-precision Real (CAPE_REAL),</para>
/// <para>integer(CAPE_INT),</para>
/// <para>String (or option)(CAPE_OPTION),</para>
/// <para>boolean(CAPE_BOOLEAN),</para>
/// <para>array(CAPE_ARRAY),</para>
/// 参考文档: Parameter Common Interface</remarks>
[Serializable]
public enum CapeParamType
{
    /// <summary>双精度 double 参数类型</summary>
    CAPE_REAL = 0,

    /// <summary>整数参数类型</summary>
    CAPE_INT = 1,

    /// <summary>字符串或 option 参数类型</summary>
    CAPE_OPTION = 2,

    /// <summary>布尔值参数类型</summary>
    CAPE_BOOLEAN = 3,

    /// <summary>数组参数类型</summary>
    CAPE_ARRAY = 4
}

/// <summary>参数模式。</summary>
/// <remarks><para>它允许以下值：</para>
/// <list type="number">
/// <item>Input (CAPE_INPUT): 该单元（或任何拥有者组件）将使用其值进行计算。</item>
/// <item>Output (CAPE_OUTPUT): 该单元将把其计算结果置于参数中。</item>
/// <item>Input-Output (CAPE_INPUT_OUTPUT): 用户输入一个初始估计值，并输出一个计算值。</item>
/// </list>
/// 参考文档: Parameter Common Interface
/// </remarks>
[Serializable]
public enum CapeParamMode
{
    /// <summary>该单元（或任何拥有该参数的组件）将使用该参数的值作为其计算的输入。</summary>
    CAPE_INPUT = 0,

    /// <summary>该单元（或任何拥有该参数的组件）将把该参数的值作为其计算结果的输出。</summary>
    CAPE_OUTPUT = 1,

    /// <summary>单元（或任何拥有该参数的组件）将使用参数的初始值作为估计值，并计算最终值。</summary>
    CAPE_INPUT_OUTPUT = 2
}

/// <remarks>参考文档: Parameter Common Interface</remarks>
[ComVisible(false)]
[Description("ICapeParameterSpec Interface")]
public interface ICapeParameterSpec
{
    /// <summary>获取参数的类型</summary>
    /// <remarks>获取此参数所对应的 <see cref="CapeParamType"/> 参数类型:
    /// real(CAPE_REAL), integer(CAPE_INT), option(CAPE_OPTION), boolean(CAPE_BOOLEAN), array(CAPE_ARRAY).</remarks>
    /// <value>参数类型</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1)]
    [Description("Property Type")]
    CapeParamType Type { get; }

    /// <summary>获取参数的维度 (dimensionality)。</summary>
    /// <remarks><para>获取此参数的规格所对应的维度。该维度代表该参数的物理维度轴。
    /// 预期维度必须至少覆盖 6 个基本轴: (length, mass, time, angle, temperature, charge)。</para>
    /// <para>A possible implementation could consist in being a constant 
    /// length array vector that contains the exponents of each basic SI unit, 
    /// following directives of SI-brochure (from http://www.bipm.fr/). So if we 
    /// agree on order &lt;m kg s A K,&gt; ... velocity would be 
    /// &lt;1,0,-1,0,0,0&gt;: that is m1 * s-1 =m/s.</para>
    /// <para>美国环保局已向协调组织科学委员会建议采用国际单位制基本单位，
    /// 并辅以带有特殊符号的国际单位制导出单位（以提升实用性并允许定义角度）。</para></remarks>
    /// <value>一个整型数组，用于表示各维度轴的指数。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2)]
    [Description("Property Dimensionality")]
    double[] Dimensionality { get; }
}

/// <remarks>参考文档: Parameter Common Interface</remarks>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeParameterSpec_IID)]
[Description("ICapeParameterSpec Interface")]
internal interface ICapeParameterSpecCOM
{
    /// <summary>获取参数的类型</summary>
    /// <remarks>获取此参数所对应的 <see cref="CapeParamType"/> 参数类型:
    /// real(CAPE_REAL), integer(CAPE_INT), option(CAPE_OPTION), boolean(CAPE_BOOLEAN), array(CAPE_ARRAY).</remarks>
    /// <value>参数类型</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1)]
    [Description("Property Type")]
    CapeParamType Type { get; }

    /// <summary>获取参数的维度 (dimensionality)。</summary>
    /// <remarks><para>获取此参数的规格所对应的维度。该维度代表该参数的物理维度轴。
    /// 预期维度必须至少覆盖 6 个基本轴: (length, mass, time, angle, temperature, charge)。</para>
    /// <para>A possible implementation could consist in being a constant 
    /// length array vector that contains the exponents of each basic SI unit, 
    /// following directives of SI-brochure (from http://www.bipm.fr/). So if we 
    /// agree on order &lt;m kg s A K,&gt; ... velocity would be 
    /// &lt;1,0,-1,0,0,0&gt;: that is m1 * s-1 =m/s.</para>
    /// <para>美国环保局已向协调组织科学委员会建议采用国际单位制基本单位，
    /// 并辅以带有特殊符号的国际单位制导出单位（以提升实用性并允许定义角度）。</para></remarks>
    /// <value>一个整型数组，用于表示各维度轴的指数。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2)]
    [Description("Property Dimensionality")]
    object Dimensionality { get; }
}

/// <summary>此接口用于参数具有双精度浮点值时的参数规格说明。</summary>
[ComVisible(false)]
[Description("ICapeRealParameterSpec Interface")]
public interface ICapeRealParameterSpec
{
    /// <summary>获取参数的默认值。</summary>
    /// <remarks>该参数的默认值。</remarks>
    /// <value>该参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1)]
    [Description("Property Default")]
    double SIDefaultValue { get; set; }

    /// <summary>获取参数的下限。</summary>
    /// <remarks>该参数的下限值。</remarks>
    /// <value>该参数的下限值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2)]
    [Description("Property LowerBound")]
    double SILowerBound { get; set; }

    /// <summary>获取参数的上限值。</summary>
    /// <remarks>该参数的上限值。</remarks>
    /// <value>该参数的上限值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(3)]
    [Description("Property UpperBound")]
    double SIUpperBound { get; set; }

    /// <summary>验证该值是否符合参数的规范。该消息用于返回参数无效的原因。</summary>
    /// <remarks>若当前值位于上下限之间，则该参数视为有效。该消息用于返回参数无效的原因。</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="value">将根据参数当前规范进行验证的整数值。</param>
    /// <param name="message">指向一个字符串的引用，该字符串将包含有关参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(4)]
    [Description("Check if value is OK for this spec as double")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool SIValidate(double value, ref string message);

    /// <summary>获取参数的默认值。</summary>
    /// <remarks>该参数的默认值。</remarks>
    /// <value>该参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1)]
    [Description("Property Default")]
    double DimensionedDefaultValue { get; set; }

    /// <summary>获取参数的下限。</summary>
    /// <remarks>该参数的下限值。</remarks>
    /// <value>该参数的下限值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2)]
    [Description("Property LowerBound")]
    double DimensionedLowerBound { get; set; }

    /// <summary>获取参数的上限值。</summary>
    /// <remarks>该参数的上限值。</remarks>
    /// <value>该参数的上限值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(3)]
    [Description("Property UpperBound")]
    double DimensionedUpperBound { get; set; }

    /// <summary>验证该值是否符合参数的规范。该消息用于返回参数无效的原因。</summary>
    /// <remarks>若当前值位于上下限之间，则该参数视为有效。该消息用于返回参数无效的原因。</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="value">将根据参数当前规范进行验证的整数值。</param>
    /// <param name="message">指向一个字符串的引用，该字符串将包含有关参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(4)]
    [Description("Check if value is OK for this spec as double")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool DimensionedValidate(double value, ref string message);
}

/// <summary>此接口用于参数具有双精度浮点值时的参数规格说明。</summary>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeRealParameterSpec_IID)]
[Description("ICapeRealParameterSpec Interface")]
internal interface ICapeRealParameterSpecCOM
{
    /// <summary>获取参数的默认值。</summary>
    /// <remarks>该参数的默认值。</remarks>
    /// <value>该参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1)]
    [Description("Property Default")]
    double DefaultValue { get; }

    /// <summary>获取参数的下限。</summary>
    /// <remarks>该参数的下限值。</remarks>
    /// <value>该参数的下限值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2)]
    [Description("Property LowerBound")]
    double LowerBound { get; }

    /// <summary>获取参数的上限值。</summary>
    /// <remarks>该参数的上限值。</remarks>
    /// <value>该参数的上限值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(3)]
    [Description("Property UpperBound")]
    double UpperBound { get; }

    /// <summary>验证该值是否符合参数的规范。该消息用于返回参数无效的原因。</summary>
    /// <remarks>若当前值位于上下限之间，则该参数视为有效。该消息用于返回参数无效的原因。</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="value">将根据参数当前规范进行验证的整数值。</param>
    /// <param name="message">指向一个字符串的引用，该字符串将包含有关参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(4)]
    [Description("Check if value is OK for this spec as double")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool Validate(double value, ref string message);
}

/// <summary>此接口用于参数为整数值时的参数规格说明。</summary>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeIntegerParameterSpec_IID)]
[Description("ICapeIntegerParameterSpec Interface")]
public interface ICapeIntegerParameterSpec
{
    /// <summary>获取参数的默认值。</summary>
    /// <remarks>该参数的默认值。</remarks>
    /// <value>该参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1), Description("Property Default")]
    int DefaultValue { get; set; }

    /// <summary>获取参数的下限。</summary>
    /// <remarks>该参数的下限值。</remarks>
    /// <value>该参数的下限值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2), Description("Property LowerBound")]
    int LowerBound { get; set; }

    /// <summary>获取参数的上限值。</summary>
    /// <remarks>该参数的上限值。</remarks>
    /// <value>该参数的上限值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(3), Description("Property UpperBound")]
    int UpperBound { get; set; }

    /// <summary>验证发送的值是否符合参数的规范。</summary>
    /// <remarks>若当前值位于上下限之间，则该参数视为有效。该消息用于返回参数无效的原因。</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="pValue">将根据参数当前规范进行验证的整数值。</param>
    /// <param name="message">指向一个字符串的引用，该字符串将包含有关参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(4)]
    [Description("Check if value is OK for this spec as double")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool Validate(int pValue, ref string message);
}

/// <summary>此接口用于参数为选项时的参数规范，该选项表示一个字符串列表，从中选择其中一项。</summary>
[ComVisible(false)]
[Description("ICapeOptionParameterSpec Interface")]
public interface ICapeOptionParameterSpec
{
    /// <summary>获取参数的默认值。</summary>
    /// <remarks>该参数的默认字符串值。</remarks>
    /// <value>该参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1), Description("Property Default")]
    string DefaultValue { get; set; }

    /// <summary>如果 RestrictedToList 属性为 true，则获取该参数的有效值列表。</summary>
    /// <remarks>当 <see cref="RestrictedToList" /> 设置为 <c>true</c> 时，用于验证该参数。</remarks>
    /// <value>字符串数组作为 System.Object，COM Variant 包含 BSTR 的 SafeArray。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2), Description("The list of names of the items")]
    string[] OptionList { get; set; }

    /// <summary>用于验证参数值的字符串列表。</summary>
    /// <remarks>如果为 <c>true</c>，则参数值将根据 <see cref="OptionList" /> 中的字符串进行验证。</remarks>
    /// <value>通过 COM 互操作转换为基于 COM 的 CAPE-OPEN VARIANT_BOOL 类型。</value>
    [DispId(3), Description("True if it only accepts values from the option list.")]
    bool RestrictedToList { get; set; }

    /// <summary>验证该值是否符合参数的规范。</summary>
    /// <remarks>如果 <see cref="RestrictedToList" /> 的值设置为 <c>true</c>，则该值仅在
    /// 包含于 <see cref="OptionList" /> 时才视为参数的有效值。若 <see cref="RestrictedToList" /> 的
    /// 值为 <c>false</c>，则任何有效的字符串均可作为该参数的有效值。</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="pValue">待测试参数的候选值，用于确定该值是否有效。</param>
    /// <param name="message">指向一个字符串的引用，该字符串将包含有关参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(4)]
    [Description("Check if value is OK for this spec as string")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool Validate(string pValue, ref string message);
}

/// <summary>此接口用于参数为选项时的参数规范，该选项表示一个字符串列表，从中选择其中一项。</summary>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeOptionParameterSpec_IID)]
[Description("ICapeOptionParameterSpec Interface")]
internal interface ICapeOptionParameterSpecCOM
{
    /// <summary>获取参数的默认值。</summary>
    /// <remarks>该参数的默认字符串值。</remarks>
    /// <value>该参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1), Description("Property Default")]
    string DefaultValue { get; }

    /// <summary>如果 RestrictedToList 属性为 true，则获取该参数的有效值列表。</summary>
    /// <remarks>当 <see cref="RestrictedToList" /> 设置为 <c>true</c> 时，用于验证该参数。</remarks>
    /// <value>字符串数组作为 System.Object，COM Variant 包含 BSTR 的 SafeArray。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2), Description("The list of names of the items")]
    object OptionList { get; }

    /// <summary>用于验证参数值的字符串列表。</summary>
    /// <remarks>如果为 <c>true</c>，则参数值将根据 <see cref="OptionList" /> 中的字符串进行验证。</remarks>
    /// <value>通过 COM 互操作转换为基于 COM 的 CAPE-OPEN VARIANT_BOOL类型。</value>
    [DispId(3), Description("True if it only accepts values from the option list.")]
    bool RestrictedToList { get; }

    /// <summary>验证该值是否符合参数的规范。</summary>
    /// <remarks>如果 <see cref="RestrictedToList" /> 的值设置为 <c>true</c>，则该值仅在
    /// 包含于 <see cref="OptionList" /> 时才视为参数的有效值。若 <see cref="RestrictedToList" /> 的
    /// 值为 <c>false</c>，则任何有效的字符串均可作为该参数的有效值。</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="pValue">待测试参数的候选值，用于确定该值是否有效。</param>
    /// <param name="message">指向一个字符串的引用，该字符串将包含有关参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(4)]
    [Description("Check if value is OK for this spec as string")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool Validate(string pValue, ref string message);
}

/// <summary>此接口用于指定布尔型参数的参数规格。</summary>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeBooleanParameterSpec_IID)]
[Description("ICapeBooleanParameterSpec Interface")]
public interface ICapeBooleanParameterSpec
{
    /// <summary>获取参数的默认值。</summary>
    /// <remarks>获取参数的默认值。</remarks>
    /// <value>该参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1), Description("Property Default")]
    bool DefaultValue { get; set; }

    /// <summary>验证发送的值是否符合参数的规范。</summary>
    /// <remarks>验证参数是否接受该参数作为有效值。它返回一个标志位以指示验证的成功或失败，
    /// 同时返回一条文本消息，该消息可用于向客户端/用户传达验证原因。</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="pValue">一个布尔值，将根据参数的当前规范进行验证。</param>
    /// <param name="message">指向一个字符串的引用，该字符串将包含有关参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2)]
    [Description("Check if value is OK for this spec")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool Validate(bool pValue, ref string message);
}

/// <summary>这个界面用于参数规格说明，当参数是一个值数组（可能是 integers, reals, booleans 或再次是 array）时，它表示的内容。</summary>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeArrayParameterSpec_IID)]
[Description("ICapeArrayParameterSpec Interface")]
public interface ICapeArrayParameterSpec
{
    /// <summary>获取数组的维数。</summary>
    /// <remarks>参数数组的维数。</remarks>
    /// <value>参数数组的维数。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1), Description("Get the number of dimensions of the array")]
    int NumDimensions { get; }

    /// <summary>Gets the size of each one of the dimensions of the array.</summary>
    /// <remarks>An array containing the specification of each member of the parameter array. </remarks>
    /// <value>An integer array containing the size of each dimension of the array.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2), Description("Get the size of each one of the dimensions of the array")]
    int[] Size { get; }

    /// <summary>Gets an array of the specifications of each of the items in the 
    /// value of a parameter.</summary>
    /// <remarks>An array of interfaces to the correct specification type (<see cref="ICapeRealParameterSpec"/> ,
    /// <see cref="ICapeIntegerParameterSpec"/> , <see cref="ICapeBooleanParameterSpec"/> , 
    /// <see cref="ICapeOptionParameterSpec"/> ). Note that it is also possible, for 
    /// example, to configure an array of arrays of integers, which would a similar 
    /// but not identical concept to a two-dimensional matrix of integers.</remarks>
    /// <value>An array of <see cref="ICapeParameterSpec"/> objects.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(3), Description("Get the specification of each of the values in the array")]
    object[] ItemsSpecifications { get; }

    /// <summary>验证该值是否符合参数的规范。该消息用于返回参数无效的原因。</summary>
    /// <remarks>This method checks the current value of the parameter to determine if it is an allowed value. </remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="inputArray">The message is used to return the reason that the parameter is invalid.</param>
    /// <param name="messages">A string array containing the message is used to return the reason that the parameter is invalid.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(4), Description("Check if value is OK for this spec ")]
    object Validate(object inputArray, ref string[] messages);
}

/// <summary>Interface defining the actual Parameter quantity.</summary>
[ComImport]
[ComVisible(false)]
[Guid(COGuids.ICapeParameter_IID)]
[Description("ICapeParameter Interface")]
public interface ICapeParameter
{
    /// <summary>Gets the Specification for this Parameter</summary>
    /// <remarks>Gets the specification of the parameter. The Get method returns the 
    /// specification as an interface to the correct specification type.</remarks>
    /// <value>An object implementing the <see cref="ICapeParameterSpec"/>, as well as the
    /// appropraite specification for the parameter type, <see cref="ICapeRealParameterSpec"/> ,
    /// <see cref="ICapeIntegerParameterSpec"/> , <see cref="ICapeBooleanParameterSpec"/> , 
    /// <see cref="ICapeOptionParameterSpec"/> , or <see cref="ICapeArrayParameterSpec"/> .</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(1)]
    [Description("Gets and sets the specification for the parameter.")]
    object Specification
    {
        [return: MarshalAs(UnmanagedType.IDispatch)]
        get;
    }

    /// <summary>Gets and sets the value for this Parameter</summary>
    /// <remarks>Gets and sets the value of this parameter. Passed as a CapeVariant that 
    /// should be the same type as the Parameter type.</remarks>
    /// <value>The boxed value of the parameter.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(2)]
    [Description("Get and sets the value of the parameter.")]
    object value { get; set; }

    /// <summary>Gets the flag to indicate parameter validation's status.</summary>
    /// <remarks><para>Gets the flag to indicate parameter validation status. It has three 
    /// possible values:</para>
    /// <para>   (i)   notValidated(CAPE_NOT_VALIDATED): The PMC's <c>Validate()</c>
    /// method has not been called after the last time that its value had been 
    /// changed.</para>
    /// <para>   (ii)  invalid(CAPE_INVALID): The last time that the PMC's 
    /// <c>Validate()</c> method was called it returned false.</para>
    /// <para>   (iii) valid(CAPE_VALID): the last time that the PMC's
    /// Validate() method was called it returned true.</para></remarks>
    /// <value>The validity staus of the parameter, either valid, invalid, or "not validated".</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(3), Description("Get the parameter validation status")]
    CapeValidationStatus ValStatus { get; }

    /// <summary>Gets and sets the mode of the parameter.</summary>
    /// <remarks><para>Modes of parameters. It allows the following values:</para>
    /// <para>   (i)   Input (CAPE_INPUT): the Unit(or whichever owner component) will use 
    /// its value to calculate.</para>
    /// <para>   (ii)  Output (CAPE_OUTPUT): the Unit will place in the parameter a result 
    /// of its calculations.</para>
    /// <para>   (iii) Input-Output (CAPE_INPUT_OUTPUT): the user inputs an 
    /// initial estimation value and the user outputs a calculated value.</para></remarks>
    /// <value>The mode of the parameter, input, output, or input/output.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(4), Description("Get the Mode - input,output - of the parameter.")]
    CapeParamMode Mode { get; set; }

    /// <summary>Validates the current value of the parameter against the 
    /// specification of the parameter.</summary>
    /// <remarks>This method checks the current value of the parameter to determine if it is an allowed value. In the case of 
    /// numeric parameters (<see cref="ICapeRealParameterSpec"/> and <see cref="ICapeIntegerParameterSpec"/>),
    /// the value is valid if it is between the upper and lower bound. For String (<see cref="ICapeOptionParameterSpec"/>),
    /// if the <see cref="ICapeOptionParameterSpec.RestrictedToList"/> property is true, the value must be included as one of the
    /// members of the <see cref="ICapeOptionParameterSpec.OptionList"/>. Otherwise, any string value is valid. Any boolean value (true/false) 
    /// valid for the <see cref="ICapeBooleanParameterSpec"/> paramaters.</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="message">The message is used to return the reason that the parameter is invalid.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [DispId(5)]
    [Description("Validate the parameter's current value.")]
    [return: MarshalAs(UnmanagedType.VariantBool)]
    bool Validate(ref string message);

    /// <summary>Sets the value of the parameter to its default value.</summary>
    /// <remarks>This method sets the parameter to its default value.</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    [DispId(6)]
    [Description("Reset the value of the parameter to its default.")]
    void Reset();
}

/// <summary></summary>
/// <remarks>
/// </remarks>
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[ComVisible(true)]
[Guid("3C32AD8E-490D-4822-8A8E-073F5EDFF3F5")]
[Description("CapeParameterEvents Interface")]
interface IParameterEvents
{
    /// <summary>Occurs when the user changes of the value of a paramter.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnComponentNameChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterValueChanged</c> in a derived class, be sure to call the base class's <c>OnParameterValueChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="sender">The <see cref="RealParameter">RealParameter</see> that raised the event.</param>
    /// <param name="args">A <see cref="ParameterValueChanged">ParameterValueChanged</see> that contains information about the event.</param>
    void ParameterValueChanged(object sender, object args);

    /// <summary>Occurs when the user changes of the mode of a parameter.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnParameterModeChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterModeChanged</c> in a derived class, be sure to call the base class's <c>OnParameterModeChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="sender">The <see cref="RealParameter">RealParameter</see> that raised the event.</param>
    /// <param name="args">A <see cref="ParameterModeChangedEventArgs">ParameterModeChangedEventArgs</see> that contains information about the event.</param>
    void ParameterModeChanged(object sender, object args);

    /// <summary>Occurs when a parameter is validated.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnParameterValidated</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterValidated</c> in a derived class, be sure to call the base class's <c>OnParameterValidated</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="sender">The <see cref="RealParameter">RealParameter</see> that raised the event.</param>
    /// <param name="args">A <see cref="ParameterValidatedEventArgs">ParameterValidatedEventArgs</see> that contains information about the event.</param>
    void ParameterValidated(object sender, object args);

    /// <summary>Occurs when the user resets a parameter.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnParameterReset</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterReset</c> in a derived class, be sure to call the base class's <c>OnParameterReset</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="sender">The <see cref="RealParameter">RealParameter</see> that raised the event.</param>
    /// <param name="args">A <see cref="ParameterResetEventArgs">ParameterResetEventArgs</see> that contains information about the event.</param>
    void ParameterReset(object sender, object args);
}

class ParameterTypeConverter : ExpandableObjectConverter
{
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        if (typeof(ICapeParameter).IsAssignableFrom(destinationType))
            return true;
        if (typeof(ICapeIdentification).IsAssignableFrom(destinationType))
            return true;
        return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object parameter,
        Type destinationType)
    {
        if (typeof(string).IsAssignableFrom(destinationType) &&
            typeof(ArrayParameterWrapper).IsAssignableFrom(parameter.GetType()))
        {
            return ""; //((CapeOpen.ArrayParameterWrapper)parameter).;
        }

        if (typeof(string).IsAssignableFrom(destinationType) &&
            typeof(ICapeParameter).IsAssignableFrom(parameter.GetType()))
        {
            return ((ICapeParameter)parameter).value.ToString();
        }

        if (typeof(string).IsAssignableFrom(destinationType) &&
            typeof(ICapeIdentification).IsAssignableFrom(parameter.GetType()))
        {
            return ((ICapeIdentification)parameter).ComponentName;
        }

        return base.ConvertTo(context, culture, parameter, destinationType);
    }
}

/// <summary>Aspen(TM) interface for providing dimension for a real-valued parameter.
///</summary>
/// <remarks>
/// <para>
/// Aspen Plus(TM) does not use the <see cref="ICapeParameterSpec.Dimensionality">ICapeParameterSpec.Dimensionality</see> method. Instead a parameter
/// can implement the IATCapeXRealParameterSpec interface which can be used to define the
/// display unit for a parameter value. 
/// </para>
/// </remarks>
[ComImport]
[ComVisible(false)]
[Guid("B777A1BD-0C88-11D3-822E-00C04F4F66C9")]
[Description("IATCapeXRealParameterSpec Interface")]
interface IATCapeXRealParameterSpec
{
    /// <summary>Gets the display unit for the parameter. Used by AspenPlus(TM).</summary>
    /// <remarks><para>DisplayUnits defines the unit of measurement symbol for a parameter.</para>
    /// <para>Note: The symbol must be one of the uppercase strings recognized by Aspen
    /// Plus to ensure that it can perform unit of measurement conversions on the 
    /// parameter value. The system converts the parameter's value from SI units for
    /// display in the data browser and converts updated values back into SI.
    /// </para></remarks>
    /// <value>Defines the display unit for the parameter.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    [DispId(0x60040003), Description(" Provide the Aspen Plus display units for for this parameter.")]
    string DisplayUnits { get; }
}

/// <summary>Represents the method that will handle the changing of the value of a parameter.</summary>
[ComVisible(false)]
public delegate void ParameterValueChangedHandler(object sender, ParameterValueChangedEventArgs args);

/// <summary>Provides data for the value changed event associated with the parameters.</summary>
/// <remarks>
/// The IParameterValueChangedEventArgs interface specifies the old and new value of the parameter.
/// </remarks>
[ComVisible(true)]
[Guid("41E1A3C4-F23C-4B39-BC54-39851A1D09C9")]
[Description("CapeIdentificationEvents Interface")]
interface IParameterValueChangedEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    string ParameterName { get; }

    /// <summary>The value of the parameter prior to the change.</summary>
    /// <remarks>The former value of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The value of the parameter prior to the change.</value>
    object OldValue { get; }

    /// <summary>The value of the parameter after the change.</summary>
    /// <remarks>The new nvalue of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The value of the parameter after the change.</value>
    object NewValue { get; }
}

/// <summary>Provides data for the value changed event associated with the parameters.</summary>
/// <remarks>
/// The ParameterValueChangedEventArgs event specifies the old and new value of the parameter.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("C3592B59-92E8-4A24-A2EB-E23C38F88E7F")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterValueChangedEventArgs : EventArgs,
    IParameterValueChangedEventArgs
{
    private string m_paramName;
    private object m_oldValue;
    private object m_newValue;

    /// <summary>Creates an instance of the ParameterValueChangedEventArgs class with the old and parameter value.</summary>
    /// <remarks>You can use this constructor when raising the ParameterValueChangedEvent at run time to specify a 
    /// specific the parameter having its value changed.</remarks>
    /// <param name="paramName">The name of the parameter being changed.</param>
    /// <param name="oldValue">The name of the PMC prior to the name change.</param>
    /// <param name="newValue">The name of the PMC after the name change.</param>
    public ParameterValueChangedEventArgs(string paramName, object oldValue, object newValue)
    {
        m_paramName = paramName;
        m_oldValue = oldValue;
        m_newValue = newValue;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <value>The name of the parameter being changed.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }

    /// <summary>The value of the parameter prior to the name change.</summary>
    /// <remarks>The former value of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The value of the parameter prior to the change.</value>
    public object OldValue
    {
        get { return m_oldValue; }
    }

    /// <summary>The value of the parameter after the change.</summary>
    /// <remarks>The new name of the unit can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The value of the parameter after the change.</value>
    public object NewValue
    {
        get { return m_newValue; }
    }
}

/// <summary>Represents the method that will handle the changing of the default value of a parameter.</summary>
[ComVisible(false)]
public delegate void ParameterDefaultValueChangedHandler(object sender, ParameterDefaultValueChangedEventArgs args);

/// <summary>Provides data for the value changed event associated with the parameters.</summary>
/// <remarks>
/// The IParameterDefaultValueChangedEventArgs interface specifies the old and new default value of the parameter.
/// </remarks>
[ComVisible(true)]
[Guid("E5D9CE6A-9B10-4A81-9E06-1B6C6C5257F3")]
[Description("CapeIdentificationEvents Interface")]
interface IParameterDefaultValueChangedEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    string ParameterName { get; }

    /// <summary>The default value of the parameter prior to the change.</summary>
    /// <remarks>The default value of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The default value of the parameter prior to the change.</value>
    object OldDefaultValue { get; }

    /// <summary>The default value of the parameter  after the name change.</summary>
    /// <remarks>The new default value of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The default value of the parameter after the change.</value>
    object NewDefaultValue { get; }
}

/// <summary>Provides data for the value changed event associated with the parameters.</summary>
/// <remarks>
/// The ParameterDefaultValueChangedEventArgs event specifies the old and new default value of the parameter.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("355A5BDD-F6B5-4EEE-97C7-F1533DD28889")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterDefaultValueChangedEventArgs : EventArgs,
    IParameterDefaultValueChangedEventArgs
{
    private string m_paramName;
    private object m_oldDefaultValue;
    private object m_newDefaultValue;

    /// <summary>Creates an instance of the ParameterDefaultValueChangedEventArgs class with the old and new default values.</summary>
    /// <remarks>You can use this constructor when raising the ParameterDefaultValueChangedEventArgs at run time to specify  
    /// that the default value of the parameter has changed.</remarks>
    /// <param name="paramName">The name of the parameter being changed.</param>
    /// <param name="oldDefaultValue">The default value of the parameter prior to the change.</param>
    /// <param name="newDefaultValue">The default value of the parameter after the change.</param>
    public ParameterDefaultValueChangedEventArgs(string paramName, object oldDefaultValue, object newDefaultValue)
    {
        m_paramName = paramName;
        m_oldDefaultValue = oldDefaultValue;
        m_newDefaultValue = newDefaultValue;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <value>The name of the parameter being changed.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }

    /// <summary>The name of the PMC prior to the name change.</summary>
    /// <remarks>The former name of the unit can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The default of the parameter prior to the change.</value>
    public object OldDefaultValue
    {
        get { return m_oldDefaultValue; }
    }

    /// <summary>The default value of the parameter after the name change.</summary>
    /// <remarks>The new default value for the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The default value of the parameter after the change.</value>
    public object NewDefaultValue
    {
        get { return m_newDefaultValue; }
    }
}

/// <summary>Represents the method that will handle the changing of the lower bound of a parameter.</summary>
[ComVisible(false)]
public delegate void ParameterLowerBoundChangedHandler(object sender, ParameterLowerBoundChangedEventArgs args);

/// <summary>Provides data for the value changed event associated with the parameters.</summary>
/// <remarks>
/// The IParameterLowerBoundChangedEventArgs interface specifies the old and new lower bound of the parameter.
/// </remarks>
[ComVisible(true)]
[Guid("FBCE7FC9-0F58-492B-88F9-8A23A23F93B1")]
[Description("CapeIdentificationEvents Interface")]
interface IParameterLowerBoundChangedEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    string ParameterName { get; }

    /// <summary>The lower bound of the parameter prior to the change.</summary>
    /// <remarks>The former lower bound of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The lower bound of the parameter prior to the change.</value>
    object OldLowerBound { get; }

    /// <summary>The lower bound of the parameter after to the change.</summary>
    /// <remarks>The former lower bound of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The lower bound of the parameter after to the change.</value>
    object NewLowerBound { get; }
}

/// <summary>Provides data for the value changed event associated with the parameters.</summary>
/// <remarks>
/// The ParameterLowerBoundChangedEventArgs event specifies the old and new lower bound of the parameter.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("A982AD29-10B5-4C86-AF74-3914DD902384")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterLowerBoundChangedEventArgs : EventArgs,
    IParameterLowerBoundChangedEventArgs
{
    private string m_paramName;
    private object m_oldLowerBound;
    private object m_newLowerBound;

    /// <summary>Creates an instance of the ParameterLowerBoundChangedEventArgs class with the old and new lower bound for the parameter.</summary>
    /// <remarks>You can use this constructor when raising the ParameterLowerBoundChangedEvent at run time to specify that 
    /// the lower bound of the parameter has changed.</remarks>
    /// <param name="paramName">The name of the parameter being changed.</param>
    /// <param name="oldLowerBound">The name of the PMC prior to the name change.</param>
    /// <param name="newLowerBound">The name of the PMC after the name change.</param>
    public ParameterLowerBoundChangedEventArgs(string paramName, object oldLowerBound, object newLowerBound)
    {
        m_paramName = paramName;
        m_oldLowerBound = oldLowerBound;
        m_newLowerBound = newLowerBound;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <value>The name of the parameter being changed.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }

    /// <summary>The lower bound of the parameter prior to the change.</summary>
    /// <remarks>The former lower bound of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The lower bound of the parameter prior to the change.</value>
    public object OldLowerBound
    {
        get { return m_oldLowerBound; }
    }

    /// <summary>The lower bound of the parameter after the change.</summary>
    /// <remarks>The new lower bound of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The lower bound of the parameter after the change.</value>
    public object NewLowerBound
    {
        get { return m_newLowerBound; }
    }
}

/// <summary>Represents the method that will handle the changing of the upper bound of a parameter.</summary>
[ComVisible(false)]
public delegate void ParameterUpperBoundChangedHandler(object sender, ParameterUpperBoundChangedEventArgs args);

/// <summary>Represents the method that will handle the changing of the upper bound of a parameter.</summary>
[ComVisible(true)]
delegate void ParameterUpperBoundChangedHandlerCOM(object sender, object args);

/// <summary>Provides data for the upper bound changed event associated with the parameters.</summary>
/// <remarks>
/// The IParameterUpperBoundChangedEventArgs interface specifies the old and new lower bound of the parameter.
/// </remarks>
[ComVisible(true)]
[Guid("A2D0FAAB-F30E-48F5-82F1-4877F61950E9")]
[Description("CapeIdentificationEvents Interface")]
interface IParameterUpperBoundChangedEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    string ParameterName { get; }

    /// <summary>The upper bound of the parameter prior to the change.</summary>
    /// <remarks>The former upper bound of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The upper bound of the parameter prior to the change.</value>
    object OldUpperBound { get; }

    /// <summary>The upper bound of the parameter after to the change.</summary>
    /// <remarks>The former upper bound of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The upper bound of the parameter after to the change.</value>
    object NewUpperBound { get; }
}

/// <summary>Provides data for the upper bound changed event associated with the parameters.</summary>
/// <remarks>
/// The ParameterUpperBoundChangedEventArgs event specifies the old and new lower bound of the parameter.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("92BF83FE-0855-4382-A15E-744890B5BBF2")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterUpperBoundChangedEventArgs : EventArgs,
    IParameterUpperBoundChangedEventArgs
{
    private string m_paramName;
    private object m_oldUpperBound;
    private object m_newUpperBound;

    /// <summary>Creates an instance of the ParameterUpperBoundChangedEventArgs class with the old and new upper bound for the parameter.</summary>
    /// <remarks>You can use this constructor when raising the ParameterUpperBoundChangedEvent at run time to specify 
    /// that the upper bound of the parameter has changed.</remarks>
    /// <param name="paramName">The name of the parameter being changed.</param>
    /// <param name="oldUpperBound">The upper bound of the parameter prior to the change.</param>
    /// <param name="newUpperBound">The upper bound of the parameter after the change.</param>
    public ParameterUpperBoundChangedEventArgs(string paramName, object oldUpperBound, object newUpperBound)
    {
        m_paramName = paramName;
        m_oldUpperBound = oldUpperBound;
        m_newUpperBound = newUpperBound;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <value>The name of the parameter being changed.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }

    /// <summary>The upper bound of the parameter prior to the change.</summary>
    /// <remarks>The former upper bound of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The upper bound of the parameter prior to the change.</value>
    public object OldUpperBound
    {
        get { return m_oldUpperBound; }
    }

    /// <summary>The upper bound of the parameter after the change.</summary>
    /// <remarks>The new upper bound of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The upper bound of the parameter after the change.</value>
    public object NewUpperBound
    {
        get { return m_newUpperBound; }
    }
}

/// <summary>Represents the method that will handle the changing of the mode of a parameter.</summary>
[ComVisible(false)]
public delegate void ParameterModeChangedHandler(object sender, ParameterModeChangedEventArgs args);

/// <summary>Provides data for the mode changed event associated with the parameters.</summary>
/// <remarks>
/// The IParameterModeChangedEventArgs interface specifies the old and new mode of the parameter.
/// </remarks>
[ComVisible(true)]
[Guid("5405E831-4B5F-4A57-A410-8E91BBF9FFD3")]
[Description("CapeIdentificationEvents Interface")]
interface IParameterModeChangedEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    string ParameterName { get; }

    /// <summary>The mode of the parameter prior to the change.</summary>
    /// <remarks>The former mode of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The mode of the parameter prior to the change.</value>
    object OldMode { get; }

    /// <summary>The mode of the parameter after to the change.</summary>
    /// <remarks>The former mode of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The mode of the parameter after to the change.</value>
    object NewMode { get; }
}

/// <summary>Provides data for the mode changed event associated with the parameters.</summary>
/// <remarks>
/// The ParameterModeChangedEventArgs event specifies the old and new mode of the parameter.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("3C953F15-A1F3-47A9-829A-9F7590CEB5E9")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterModeChangedEventArgs : EventArgs,
    IParameterModeChangedEventArgs
{
    private string m_paramName;
    private object m_oldMode;
    private object m_newMode;

    /// <summary>Creates an instance of the ParameterModeChangedEventArgs class with the old and new upper bound for the parameter.</summary>
    /// <remarks>You can use this constructor when raising the ParameterModeChangedEvent at run time to specify 
    /// that the mode of the parameter has changed.</remarks>
    /// <param name="paramName">The name of the parameter being changed.</param>
    /// <param name="oldMode">The mode of the parameter prior to the change.</param>
    /// <param name="newMode">The mode of the parameter after the change.</param>
    public ParameterModeChangedEventArgs(string paramName, object oldMode, object newMode)
    {
        m_paramName = paramName;
        m_oldMode = oldMode;
        m_newMode = newMode;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The name of the parameter being changed.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }

    /// <summary>The mode of the parameter prior to the change.</summary>
    /// <remarks>The former mode of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The mode of the parameter prior to the change.</value>
    public object OldMode
    {
        get { return m_oldMode; }
    }

    /// <summary>The mode of the parameter after the change.</summary>
    /// <remarks>The new mode of the parameter can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The mode of the parameter after the change.</value>
    public object NewMode
    {
        get { return m_newMode; }
    }
}

/// <summary>Represents the method that will handle the validation of a parameter.</summary>
[ComVisible(false)]
public delegate void ParameterValidatedHandler(object sender, ParameterValidatedEventArgs args);

/// <summary>The parameter was validated.</summary>
/// <remarks>
/// Provides information about the validation of the parameter.
/// </remarks>
[ComVisible(true)]
[Guid("EFD819A4-E4EC-462E-90E6-5D994CA44F8E")]
[Description("ParameterValidatedEvent Interface")]
interface IParameterValidatedEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The name of the parameter being changed.</value>
    string ParameterName { get; }

    /// <summary>The message reulting from the parameter validation.</summary>
    /// <remarks>The message provides information about the results of the validation process.</remarks>
    /// <value>Information regrading the validation process.</value>
    string Message { get; }

    /// <summary>The validation status of the parameter prior to the validation.</summary>
    /// <remarks>Informs the user of the results of the validation process.</remarks>
    /// <value>The validation status of the parameter prior to the validation.</value>
    CapeValidationStatus OldStatus { get; }

    /// <summary>The validation status of the parameter after the validation.</summary>
    /// <remarks>Informs the user of the results of the validation process.</remarks>
    /// <value>The validation status of the parameter after the validation.</value>
    CapeValidationStatus NewStatus { get; }
}

/// <summary>The parameter was validated.</summary>
/// <remarks>
/// Provides information about the validation of the parameter.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("5727414A-838D-49F8-AFEF-1CC8C578D756")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterValidatedEventArgs : EventArgs,
    IParameterValidatedEventArgs
{
    private string m_paramName;
    private string m_message;
    private CapeValidationStatus m_oldStatus;
    private CapeValidationStatus m_newStatus;

    /// <summary>Creates an instance of the ParameterValidatedEventArgs class for the parameter.</summary>
    /// <remarks>You can use this constructor when raising the ParameterValidatedEventArgs at run time to  
    /// the message about the parameter validation.</remarks>
    /// <param name="paramName">The name of the parameter being changed.</param>
    /// <param name="message">The message indicating the results of the parameter validation.</param>
    /// <param name="oldStatus">The status of the parameter prior to validation.</param>
    /// <param name="newStatus">The status of the parameter after the validation.</param>
    public ParameterValidatedEventArgs(string paramName, string message, CapeValidationStatus oldStatus,
        CapeValidationStatus newStatus)
    {
        m_paramName = paramName;
        m_message = message;
        m_oldStatus = oldStatus;
        m_newStatus = newStatus;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The name of the parameter being changed.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }

    /// <summary>The message reulting from the parameter validation.</summary>
    /// <remarks>The message provides information about the results of the validation process.</remarks>
    /// <value>Information regrading the validation process.</value>
    public string Message
    {
        get { return m_message; }
    }

    /// <summary>The validation status of the parameter prior to the validation.</summary>
    /// <remarks>Informs the user of the results of the validation process.</remarks>
    /// <value>The validation status of the parameter prior to the validation.</value>
    public CapeValidationStatus OldStatus
    {
        get { return m_oldStatus; }
    }

    /// <summary>The validation status of the parameter after the validation.</summary>
    /// <remarks>Informs the user of the results of the validation process.</remarks>
    /// <value>The validation status of the parameter after the validation.</value>
    public CapeValidationStatus NewStatus
    {
        get { return m_newStatus; }
    }
}

/// <summary>Represents the method that will handle the resetting of a parameter.</summary>
[ComVisible(false)]
public delegate void ParameterResetHandler(object sender, ParameterResetEventArgs args);

/// <summary>The parameter was reset.</summary>
/// <remarks>
/// The parameter was reset.
/// </remarks>
[ComVisible(true)]
[Guid("12067518-B797-4895-9B26-EA71C60A8803")]
[Description("ParameterResetEventArgs Interface")]
interface IParameterResetEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    string ParameterName { get; }
}

/// <summary>The parameter was reset.</summary>
/// <remarks>
/// The parameter was reset.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("01BF391B-415E-4F5E-905D-395A707DC125")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterResetEventArgs : EventArgs,
    IParameterResetEventArgs
{
    private string m_paramName;

    /// <summary>Creates an instance of the ParameterResetEventArgs class for the parameter.</summary>
    /// <remarks>You can use this constructor when raising the ParameterResetEventArgs at run time to  
    /// inform the system that the parameter was reset.</remarks>
    public ParameterResetEventArgs(string paramName)
    {
        m_paramName = paramName;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The name of the parameter being reset.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }
}

/// <summary>Represents the method that will handle the changing of the option list of a parameter.</summary>
[ComVisible(false)]
public delegate void ParameterOptionListChangedHandler(object sender, ParameterOptionListChangedEventArgs args);

/// <summary>The parameter was reset.</summary>
/// <remarks>
/// The parameter was reset.
/// </remarks>
[ComVisible(true)]
[Guid("78E06E7B-00AB-4295-9915-546DC1CD64A6")]
[Description("ParameterOptionListChangedEventArgs Interface")]
interface IParameterOptionListChangedEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The name of the parameter being changed.</value>
    string ParameterName { get; }
}

/// <summary>The parameter option list was changed.</summary>
/// <remarks>
/// The parameter option list was changed.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("2AEC279F-EBEC-4806-AA00-CC215432DB82")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterOptionListChangedEventArgs : EventArgs,
    IParameterOptionListChangedEventArgs
{
    private string m_paramName;

    /// <summary>Creates an instance of the ParameterOptionListChangedEventArgs class for the parameter.</summary>
    /// <remarks>You can use this constructor when raising the ParameterOptionListChangedEventArgs at run time to  
    /// inform the system that the parameter's option list was changed.</remarks>
    public ParameterOptionListChangedEventArgs(string paramName)
    {
        m_paramName = paramName;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The name of the parameter being changed.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }
}

/// <summary>The restiction to the options list of a parameter was changed.</summary>
/// <remarks>
/// The restiction to the options list of a parameter was changed.
/// </remarks>
[ComVisible(true)]
[Guid("7F357261-095A-4FD4-99C1-ACDAEDA36141")]
[Description("ParameterOptionListChangedEventArgs Interface")]
interface IParameterRestrictedToListChangedEventArgs
{
    /// <summary>The name of the parameter being changed.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The name of the parameter being changed.</value>
    string ParameterName { get; }
}

/// <summary>The parameter restiction to the option list was changed.</summary>
/// <remarks>
/// The parameter restiction to the option list was changed.
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("82E0E6C2-3103-4B5A-A5BC-EBAB971B069A")]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterRestrictedToListChangedEventArgs : EventArgs,
    IParameterRestrictedToListChangedEventArgs
{
    private string m_paramName;
    private bool m_wasRestricted;
    private bool m_isRestricted;

    /// <summary>Creates an instance of the ParameterRestrictedToListChangedEventArgs class for the parameter.</summary>
    /// <remarks>You can use this constructor when raising the ParameterRestrictedToListChangedEventArgs at run time to  
    /// inform the system that the parameter's option list was changed.</remarks>
    public ParameterRestrictedToListChangedEventArgs(string paramName, bool wasRestricted, bool isRestricted)
    {
        m_paramName = paramName;
        m_isRestricted = isRestricted;
        m_wasRestricted = wasRestricted;
    }

    /// <summary>The name of the parameter being changed.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>The name of the parameter being changed.</value>
    public string ParameterName
    {
        get { return m_paramName; }
    }

    /// <summary>States whether the value of the parameter is restricted to the values in the options list.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>Is the parameter vlue restricted to the list?.</value>
    public bool IsRestricted
    {
        get { return m_isRestricted; }
    }

    /// <summary>States whether the value of the parameter was restricted to the values in the options list prior to the 
    /// change to the resticed to list property.</summary>
    /// <remarks>The name of the parameter being updated can be used to update GUI inforamtion about the PMC.</remarks>
    /// <value>Is the parameter vlue restricted to the list?.</value>
    public bool WasRestricted
    {
        get { return m_wasRestricted; }
    }
}

/// <summary>Represents the method that will handle the changing of whether a paratemer's value is restricted to those in the option list.</summary>
[ComVisible(false)]
public delegate void ParameterRestrictedToListChangedHandler(object sender,
    ParameterRestrictedToListChangedEventArgs args);

/// <summary>Represents the method that will handle the changing of the Kinetic Reaction Chemistry of a PMC.</summary>
[ComVisible(false)]
public delegate void KineticReactionsChangedHandler(object sender, EventArgs args);

/// <summary>Represents the method that will handle the changing of the Equilibrium Reaction Chemistry of a PMC.</summary>
[ComVisible(false)]
public delegate void EquilibriumReactionsChangedHandler(object sender, EventArgs args);

/// <summary>Base Class defining the actual Parameter quantity.</summary>
[Serializable]
[ComSourceInterfaces(typeof(IParameterEvents))]
[ComVisible(true)]
[Guid("F027B4D1-A215-4107-AA75-34E929DD00A5")]
[Description("CapeIdentification Interface")]
[ClassInterface(ClassInterfaceType.None)]
[TypeConverter(typeof(ParameterTypeConverter))]
abstract public class CapeParameter : CapeIdentification,
    ICapeParameter,
    ICapeParameterSpec,
    ICapeParameterSpecCOM
{
    CapeParamMode m_mode = CapeParamMode.CAPE_INPUT_OUTPUT;

    /// <summary>The flag to indicate parameter validation's status.</summary>
    /// <remarks><para>The flag to indicate parameter validation status. It has three 
    /// possible values:</para>
    /// <para>   (i)   notValidated(CAPE_NOT_VALIDATED): The PMC's <c>Validate()</c>
    /// method has not been called after the last time that its value had been 
    /// changed.</para>
    /// <para>   (ii)  invalid(CAPE_INVALID): The last time that the PMC's 
    /// <c>Validate()</c> method was called it returned false.</para>
    /// <para>   (iii) valid(CAPE_VALID): the last time that the PMC's
    /// Validate() method was called it returned true.</para></remarks>
    protected CapeValidationStatus m_ValStatus = CapeValidationStatus.CAPE_NOT_VALIDATED;

    /// <summary>Creates a new instance of the abstract parameter base class. </summary>
    /// <remarks>The mode is set to CapeParamMode.CAPE_INPUT_OUTPUT. </remarks>
    /// <param name="name">Sets as the ComponentName of the parameter's ICapeIdentification interface.</param>
    /// <param name="description">Sets as the ComponentDescription of the parameter's ICapeIdentification interface.</param>
    /// <param name="mode">Sets the CapeParamMode mode of the parameter.</param>
    public CapeParameter(string name, string description, CapeParamMode mode)
        : base(name, description)
    {
        m_mode = mode;
    }

    /// <summary>Occurs when the user validates the parameter.</summary>
    /// <remarks><para>Raises the ParameterValidated event through a delegate.</para>        </remarks>
    public event ParameterValidatedHandler ParameterValidated;

    /// <summary>Occurs when a parameter is validated.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnParameterValidated</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterValidated</c> in a derived class, be sure to call the base class's <c>OnParameterValidated</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ParameterValidatedEventArgs">ParameterValidatedEventArgs</see> that contains information about the event.</param>
    protected void OnParameterValidated(ParameterValidatedEventArgs args)
    {
        if (ParameterValidated != null)
        {
            ParameterValidated(this, args);
        }
    }

    /// <summary>Gets the Specification for this Parameter</summary>
    /// <remarks>Gets the specification of the parameter. The Get method returns the 
    /// specification as an interface to the correct specification type.</remarks>
    /// <value>An object implementing the <see cref="ICapeParameterSpec"/>, as well as the
    /// appropraite specification for the parameter type, <see cref="ICapeRealParameterSpec"/> ,
    /// <see cref="ICapeIntegerParameterSpec"/> , <see cref="ICapeBooleanParameterSpec"/> , 
    /// <see cref="ICapeOptionParameterSpec"/> , or <see cref="ICapeArrayParameterSpec"/> .</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [Browsable(false)]
    object ICapeParameter.Specification
    {
        get { return this; }
    }

    /// <summary>Occurs when the user changes of the value of the parameter.</summary>
    /// <remarks><para>Raises the ParameterValueChanged event through a delegate.</para>        </remarks>
    public event ParameterValueChangedHandler ParameterValueChanged;

    /// <summary>Occurs when the user changes of the value of a paramter.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnComponentNameChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterValueChanged</c> in a derived class, be sure to call the base class's <c>OnParameterValueChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="OnParameterValueChanged">OnParameterValueChanged</see> that contains information about the event.</param>
    protected void OnParameterValueChanged(ParameterValueChangedEventArgs args)
    {
        if (ParameterValueChanged != null)
        {
            ParameterValueChanged(this, args);
        }
    }

    /// <summary>Gets and sets the value for this Parameter.</summary>
    /// <remarks>This value uses the System.Object data type for compatibility with 
    /// COM-based CAPE-OPEN.</remarks>
    /// <value>System.Object</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [Browsable(false)]
    virtual public object value { get; set; }


    /// <summary>Gets the dimensionality of the parameter.</summary>
    /// <remarks>Physical dimensions are only applicable to real-valued parameters.</remarks>
    /// <value>Null pointer.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [Browsable(false)]
    virtual public double[] Dimensionality
    {
        get { return null; }
    }

    /// <summary>Gets the dimensionality of the parameter.</summary>
    /// <remarks>Physical dimensions are only applicable to real-valued parameters.</remarks>
    /// <value>Null pointer.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [Browsable(false)]
    object ICapeParameterSpecCOM.Dimensionality
    {
        get { return null; }
    }


    /// <summary>Gets the flag to indicate parameter validation's status.</summary>
    /// <remarks><para>Gets the flag to indicate parameter validation status. It has three 
    /// possible values:</para>
    /// <para>   (i)   notValidated(CAPE_NOT_VALIDATED): The PMC's <c>Validate()</c>
    /// method has not been called after the last time that its value had been 
    /// changed.</para>
    /// <para>   (ii)  invalid(CAPE_INVALID): The last time that the PMC's 
    /// <c>Validate()</c> method was called it returned false.</para>
    /// <para>   (iii) valid(CAPE_VALID): the last time that the PMC's
    /// Validate() method was called it returned true.</para></remarks>
    /// <value>The validity staus of the parameter, either valid, invalid, or "not validated".</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [Category("ICapeParameter")]
    public CapeValidationStatus ValStatus
    {
        get { return m_ValStatus; }
    }

    /// <summary>Occurs when the user changes of the default value of the parameter changes.</summary>
    /// <remarks><para>Raises the ParameterDefaultValueChanged event through a delegate.</para>        </remarks>
    public event ParameterDefaultValueChangedHandler ParameterDefaultValueChanged;

    /// <summary>Occurs when the user changes of the default value of a parameter.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnParameterDefaultValueChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterDefaultValueChanged</c> in a derived class, be sure to call the base class's <c>OnParameterDefaultValueChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="OnParameterDefaultValueChanged">OnParameterDefaultValueChanged</see> that contains information about the event.</param>
    protected void OnParameterDefaultValueChanged(ParameterDefaultValueChangedEventArgs args)
    {
        if (ParameterDefaultValueChanged != null)
        {
            ParameterDefaultValueChanged(this, args);
        }

        NotifyPropertyChanged("DefaultValue");
    }

    /// <summary>Occurs when the user changes of the mode of the parameter changes.</summary>
    /// <remarks><para>Raises the ParameterModeChanged event through a delegate.</para>        </remarks>
    public event ParameterModeChangedHandler ParameterModeChanged;

    /// <summary>Occurs when the user changes of the mode of a parameter.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnParameterModeChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterModeChanged</c> in a derived class, be sure to call the base class's <c>OnParameterModeChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ParameterModeChangedEventArgs">ParameterModeChangedEventArgs</see> that contains information about the event.</param>
    protected void OnParameterModeChanged(ParameterModeChangedEventArgs args)
    {
        if (ParameterModeChanged != null)
        {
            ParameterModeChanged(this, args);
        }
    }

    /// <summary>Gets and sets the mode of the parameter.</summary>
    /// <remarks><para>Modes of parameters. It allows the following values:</para>
    /// <para>   (i)   Input (CAPE_INPUT): the Unit(or whichever owner component) will use 
    /// its value to calculate.</para>
    /// <para>   (ii)  Output (CAPE_OUTPUT): the Unit will place in the parameter a result 
    /// of its calculations.</para>
    /// <para>   (iii) Input-Output (CAPE_INPUT_OUTPUT): the user inputs an 
    /// initial estimation value and the user outputs a calculated value.</para></remarks>
    /// <value>The mode of the parameter, input, output, or input/output.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    [Category("ICapeParameter")]
    public CapeParamMode Mode
    {
        get { return m_mode; }
        set
        {
            ParameterModeChangedEventArgs args = new ParameterModeChangedEventArgs(ComponentName, m_mode, value);
            OnParameterModeChanged(args);
            m_mode = value;
            NotifyPropertyChanged("Mode");
        }
    }

    /// <summary>Validates the current value of the parameter against the 
    /// specification of the parameter. </summary>
    /// <remarks>The parameter is considered valid if the current value is 
    /// between the upper and lower bound. The message is used to 
    /// return the reason that the parameter is invalid. This function also
    /// sets the CapeValidationStatus of the parameter based upon the results
    /// of the validation.</remarks>
    /// <returns>如果参数有效则返回 true，否则返回 false。</returns>
    /// <param name="message">指向一个字符串的引用，该字符串将包含有关参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效参数值时使用，例如未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    abstract public bool Validate(ref string message);


    /// <summary>Occurs when the user changes of the parameter value is reset to the default value.</summary>
    /// <remarks><para>Raises the ParameterReset event through a delegate.</para>        </remarks>
    public event ParameterResetHandler ParameterReset;

    /// <summary>Occurs when the user resets a parameter.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnParameterReset</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnParameterReset</c> in a derived class, be sure to call the base class's <c>OnParameterReset</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ParameterResetEventArgs">ParameterResetEventArgs</see> that contains information about the event.</param>
    protected void OnParameterReset(ParameterResetEventArgs args)
    {
        if (ParameterReset != null)
        {
            ParameterReset(this, args);
        }
    }

    /// <summary>Sets the value of the parameter to its default value.</summary>
    /// <remarks> This method sets the parameter's value to the default value.</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    abstract public void Reset();


    // ICapeParameterSpec
    // ICapeParameterSpec
    /// <summary>Gets the type of the parameter. </summary>
    /// <remarks>获取此参数所对应的 <see cref="CapeParamType"/> 参数类型:
    /// real(CAPE_REAL), integer(CAPE_INT), option(CAPE_OPTION), boolean(CAPE_BOOLEAN), array(CAPE_ARRAY).</remarks>
    /// <value>The parameter type. </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应抛出的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed.</exception>
    [Category("ICapeParameterSpec")]
    abstract public CapeParamType Type { get; }
}