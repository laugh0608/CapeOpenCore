/*
 * DaBaiLuoBo
 * 2025.05.31
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

// internal class ArrayParameter { }

/// <summary>CAPE-OPEN 中使用的数组值参数的包装器<see cref="ParameterCollection">参数集合</see>。</summary>
/// <remarks>Wraps a CAPE-OPEN array-valued parameter for use in a CAPE-OPEN <see cref="ParameterCollection">parameter collection</see>.</remarks>
[Serializable,ComVisible(true)]
[ComSourceInterfaces(typeof(IRealParameterSpecEvents))]
[Guid("277E2E39-70E7-4FBA-89C9-2A19B9D5E576")]  // ICapeThermoMaterialObject_IID
[ClassInterface(ClassInterfaceType.None)]
internal class ArrayParameterWrapper : CapeParameter,
    ICapeParameter, ICapeParameterSpec, ICapeArrayParameterSpec
{
    [NonSerialized]
    private ICapeParameter _mParameter;
    
    /// <summary>为基于 COM 的数组值参数类创建一个封装类的新实例。</summary>
    /// <remarks>The COM-based array parameter is wrapped and exposed to .NET-based PME and PMCs.</remarks>
    /// <param name="parameter">The COM-based array parameter to be wrapped.</param>
    public ArrayParameterWrapper(ICapeParameter parameter) : base(
            ((ICapeIdentification)parameter).ComponentName, 
            ((ICapeIdentification)parameter).ComponentDescription, parameter.Mode)
    {
        _mParameter = parameter;
    }

    // ICloneable
    /// <summary>创建参数的副本。两个副本均引用同一基于 COM 的数组参数。</summary>
    /// <remarks><para>The clone method is used to create a copy of the parameter. Both the original object and 
    /// the clone wrap the same instance of the wrapped parameter.</para></remarks>
    /// <returns>A copy of the current parameter.</returns>
    public override object Clone()
    {
        return new ArrayParameterWrapper(_mParameter);
    }

    /// <summary>UNDEFINED 用于属性参数的验证，将当前参数值与参数规范进行比对。</summary>        
    /// <remarks>The wrapped parameter validates itself against its internal validation criteria.</remarks>
    /// <returns>True if the parameter is valid, false if not valid.</returns>
    /// <param name="message">Reference to a string that will contain a message regarding the validation of the parameter.</param>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the prop's argument.</exception>
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
    /// <remarks>This method sets the parameter's value to the default value.</remarks>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    public override void Reset()
    {
        var args = new ParameterResetEventArgs(ComponentName);
        _mParameter.Reset();
        NotifyPropertyChanged("Value");
        OnParameterReset(args);
    }

    // ICapeParameterSpec
    /// <summary>获取参数的类型。</summary>
    /// <remarks>Gets the <see cref="CapeParamType"/> of the parameter.</remarks>
    /// <value>The parameter type. </value>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed.</exception>
    [Browsable(false)]
    [Category("ICapeParameterSpec")]
    public override CapeParamType Type => CapeParamType.CAPE_ARRAY;

    // ICapeArrayParameterSpec
    /// <summary>获取参数规格数组。</summary>
    /// <remarks>Gets an array of the specifications of each of the items in the value of a parameter. The Get method 
    /// returns an array of interfaces to the correct specification type (<see cref="ICapeRealParameterSpec"/>, 
    /// <see cref="ICapeOptionParameterSpec"/>, <see cref="ICapeIntegerParameterSpec"/>, or <see cref="ICapeBooleanParameterSpec"/>).
    /// Note that it is also possible, for example, to configure an array of arrays, which would a 
    /// similar but not identical concept to a two-dimensional matrix.</remarks>
    /// <value>An array of parameter specifications. </value>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed.</exception>
    [Browsable(false)]
    object[] ICapeArrayParameterSpec.ItemsSpecifications => ((ICapeArrayParameterSpec)_mParameter.Specification).ItemsSpecifications;

    /// <summary>获取参数中数组值的维数。</summary>
    /// <remarks>The number of dimensions of the array value in the parameter.</remarks>
    /// <value>The number of dimensions of the array value in the parameter.</value>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed.</exception>
    [Category("Parameter Specification")]
    int ICapeArrayParameterSpec.NumDimensions => ((ICapeArrayParameterSpec)_mParameter.Specification).NumDimensions;

    /// <summary>获取数组各维度的大小。</summary>
    /// <remarks>Gets the size of each one of the dimensions of the array.</remarks>			
    /// <value>An integer array containing the size of each one of the dimensions of the array.</value>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed.</exception>
    [Category("Parameter Specification")]
    int[] ICapeArrayParameterSpec.Size => ((ICapeArrayParameterSpec)_mParameter.Specification).Size;

    /// <summary>确定封装参数的值是否有效。</summary>
    /// <remarks><para>Validates an array against the parameter's specification. It returns a flag to indicate the success or 
    /// failure of the validation together with a text message which can be used to convey the reasoning to the client/user.</para>
    /// <para>The wrapped parameter validates the value against its internal validation criteria.</para></remarks>
    /// <returns>True if the parameter is valid, false if not valid.</returns>
    /// <param name="pValue">The value to be checked.</param>
    /// <param name="messages">Reference to a string that will contain a message regarding the validation of the parameter.</param>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the prop's argument.</exception>
    object ICapeArrayParameterSpec.Validate(object pValue, ref string[] messages)
    {
        return ((ICapeArrayParameterSpec)_mParameter.Specification).Validate(pValue, ref messages);
    }
}