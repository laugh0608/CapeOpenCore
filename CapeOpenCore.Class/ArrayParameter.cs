/*
 * DaBaiLuoBo
 * 2025.05.31
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

// internal class ArrayParameter { }

/// <summary>CAPE-OPEN 中使用的数组值参数的包装器 <see cref="ParameterCollection"/> 参数集合。</summary>
/// <remarks>将 CAPE-OPEN 数组值参数包裹起来，以便在 CAPE-OPEN 参数集合 <see cref="ParameterCollection"/> 中使用。</remarks>
[Serializable,ComVisible(true)]
[ComSourceInterfaces(typeof(IRealParameterSpecEvents))]
[Guid("277E2E39-70E7-4FBA-89C9-2A19B9D5E576")]  // ICapeThermoMaterialObject_IID
[ClassInterface(ClassInterfaceType.None)]
internal class ArrayParameterWrapper : CapeParameter, ICapeParameter, ICapeParameterSpec, ICapeArrayParameterSpec
{
    [NonSerialized]
    private ICapeParameter _mParameter;
    
    /// <summary>为基于 COM 的数组值参数类创建一个封装类的新实例。</summary>
    /// <remarks>基于 COM 的数组参数被封装并暴露给基于 .NET 的 PME 和 PMC。</remarks>
    /// <param name="parameter">要封装的基于 COM 的数组参数。</param>
    public ArrayParameterWrapper(ICapeParameter parameter) : base(
            ((ICapeIdentification)parameter).ComponentName, 
            ((ICapeIdentification)parameter).ComponentDescription, parameter.Mode)
    {
        _mParameter = parameter;
    }

    // ICloneable
    /// <summary>创建参数的副本。两个副本均引用同一基于 COM 的数组参数。</summary>
    /// <remarks>克隆方法用于创建参数的副本。原始对象和克隆对象都包裹相同的包裹参数实例。</remarks>
    /// <returns>当前参数的副本。</returns>
    public override object Clone()
    {
        return new ArrayParameterWrapper(_mParameter);
    }

    /// <summary>UNDEFINED 用于属性参数的验证，将当前参数值与参数规范进行比对。</summary>        
    /// <remarks>包装后的参数根据其内部验证标准进行验证。</remarks>
    /// <returns>如果参数有效，则为 true；如果无效，则为 false。</returns>
    /// <param name="message">引用一个字符串，该字符串将包含关于参数验证的消息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    public override bool Validate(ref string message)
    {
        var valStatus = _mParameter.ValStatus;
        var retVal = _mParameter.Validate(message);
        // var args = new ParameterValidatedEventArgs(ComponentName, message, ValStatus, _mParameter.ValStatus);
        var args = new ParameterValidatedEventArgs(ComponentName, message, ValStatus, valStatus);
        OnParameterValidated(args);
        NotifyPropertyChanged("ValStatus");
        return retVal;
    }

    /// <summary>将参数的值设置为其默认值。</summary>
    /// <remarks>该方法将参数值设置为默认值。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    public override void Reset()
    {
        var args = new ParameterResetEventArgs(ComponentName);
        _mParameter.Reset();
        NotifyPropertyChanged("Value");
        OnParameterReset(args);
    }

    // ICapeParameterSpec
    /// <summary>获取参数的类型。</summary>
    /// <remarks>获取参数的 <see cref="CapeParamType"/>。</remarks>
    /// <value>参数的类型。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用。</exception>
    [Browsable(false)]
    [Category("ICapeParameterSpec")]
    public override CapeParamType Type => CapeParamType.CAPE_ARRAY;

    // ICapeArrayParameterSpec
    /// <summary>获取参数规格数组。</summary>
    /// <remarks><para>获取参数值中每个项目的规格的数组。Get 方法返回正确规格类型（<see cref="ICapeRealParameterSpec"/>, <see cref="ICapeOptionParameterSpec"/>,
    /// <see cref= "ICapeIntegerParameterSpec"/>， 或 <see cref="ICapeBooleanParameterSpec"/>）的接口数组。</para>
    /// <para>注意，例如，也可以配置一个数组，该数组类似于二维矩阵，但不完全相同。</para></remarks>
    /// <value>参数规格数组。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用。</exception>
    [Browsable(false)]
    object[] ICapeArrayParameterSpec.ItemsSpecifications => ((ICapeArrayParameterSpec)_mParameter.Specification).ItemsSpecifications;

    /// <summary>获取参数中数组值的维数。</summary>
    /// <remarks>参数中数组值的维数。</remarks>
    /// <value>参数中数组值的维数。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用。</exception>
    [Category("Parameter Specification")]
    int ICapeArrayParameterSpec.NumDimensions => ((ICapeArrayParameterSpec)_mParameter.Specification).NumDimensions;

    /// <summary>获取数组各维度的大小。</summary>
    /// <remarks>获取数组各维度的大小。</remarks>			
    /// <value>整数数组，包含数组各维度的大小。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用。</exception>
    [Category("Parameter Specification")]
    int[] ICapeArrayParameterSpec.Size => ((ICapeArrayParameterSpec)_mParameter.Specification).Size;

    /// <summary>确定封装参数的值是否有效。</summary>
    /// <remarks><para>根据参数说明验证数组。它会返回一个标志来表示验证成功或  验证成功或失败的标志，以及一条文本信息，该信息可用于向客户端/用户传达验证理由。</para>
    /// <para>封装参数根据其内部验证标准对值进行验证。</para></remarks>
    /// <returns>如果参数有效，则为 true；如果无效，则为 false。</returns>
    /// <param name="pValue">要检查的值。</param>
    /// <param name="messages">指向字符串的引用，该字符串将包含与参数验证相关的信息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    object ICapeArrayParameterSpec.Validate(object pValue, ref string[] messages)
    {
        return ((ICapeArrayParameterSpec)_mParameter.Specification).Validate(pValue, ref messages);
    }
}