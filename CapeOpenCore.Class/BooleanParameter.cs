/*
 * DaBaiLuoBo
 * 2025.06.01
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>表示布尔值参数的说明已更改。</summary>
/// <remarks><para>这个接口暴露给基于 COM 的 PMEs，并作为与布尔值参数规范更改相关的事件的源接口。</para>
/// <para>这个接口不是 CAPE-OPEN 规范的一部分。这个接口及其实现是为了给基于 COM 的开发人员提供与基于 .NET 的开发人员类似的功能。</para></remarks>
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[ComVisible(true)]
[Guid("07D17ED3-B25A-48EA-8261-5ED2D076ABDD")]
[Description("CapeRealParameterEvents Interface")]
internal interface ICapeBooleanParameterSpecEvents
{
    /// <summary>当用户更改参数的默认值时发生。</summary>
    /// <remarks><para>通过委托调用事件处理程序时，会引发事件。<c>OnParameterDefaultValueChanged</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>留给继承者的注释：当在派生类中重写 <c>OnParameterDefaultValueChanged</c> 时，
    /// 请务必调用基类的 <c>OnParameterDefaultValueChange</c> 方法，以便注册的委托接收事件。</para></remarks>
    /// <param name="sender">引发事件的<see cref="RealParameter"/>。</param>
    /// <param name="args">包含有关该事件的信息的 <see cref="ParameterDefaultValueChanged"/>。</param>
    void ParameterDefaultValueChanged(object sender, object args);

    /// <summary>验证参数时出现。</summary>
    /// <remarks><para>通过委托调用事件处理程序时，会引发事件。<c>OnParameterValidated</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>对继承者的说明：在子类中重写 <c>OnParameterValidated</c> 时，请务必调用基类的 <c>OnParameterValidated方法</c>，以便注册的委托接收事件。</para></remarks>
    /// <param name="sender">引发事件的<see cref="ICapeBooleanParameterSpec"/>。</param>
    /// <param name="args">包含有关事件的信息的 <see cref="ParameterValidatedEventArgs"/>。</param>
    void ParameterValidated(object sender, object args);
}

/// <summary>布尔值参数，用于 CAPE-OPEN 参数集。</summary>
/// <remarks>布尔值参数，用于 CAPE-OPEN 参数集。</remarks>
[Serializable,ComVisible(true)]
[ComSourceInterfaces(typeof(ICapeBooleanParameterSpecEvents))]
[Guid("8B8BC504-EEB5-4a13-B016-9614543E4536")]
[ClassInterface(ClassInterfaceType.None)]
public class BooleanParameter : CapeParameter,
    ICapeParameter, ICapeParameterSpec, ICapeParameterSpecCOM, ICapeBooleanParameterSpec
{
    private bool _mValue;
    private bool _mDefaultValue;

    /// <summary>获取和设置该参数的值。</summary>
    /// <remarks>此值使用系统。与基于 COM 的 CAPE-OPEN 兼容的对象数据类型。该值作为布尔值变量被编组到COM中，该变量也称为 VARIANT_BOOL。</remarks>
    /// <value>参数的方框（可选？）布尔值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    [Browsable(false)]
    public override object value
    {
        get => _mValue;
        set
        {
            var args = new ParameterValueChangedEventArgs(ComponentName, _mValue, Convert.ToBoolean(value));
            _mValue = Convert.ToBoolean(value);
            OnParameterValueChanged(args);
        }
    }
        
    /// <summary>布尔值参数的构造函数。</summary>
    /// <remarks>这个构造函数设置参数的 <see cref="ICapeIdentification.ComponentName"/>。参数值和默认值被设置为值。
    /// 此外，参数 <see cref="CapeParamMode"/> 被设置。</remarks>
    /// <param name="name">设置为参数的 ICapeIdentification 接口的 ComponentName。</param>
    /// <param name="value">设置参数的初始值和默认值。</param>
    /// <param name="mode">设置参数的 CapeParamMode 模式。</param>
    public BooleanParameter(string name, bool value, CapeParamMode mode)
        : base(name, string.Empty, mode)
    {
        _mValue = value;
        _mDefaultValue = value;
        Mode = mode;            
    }
    
    /// <summary>布尔值参数的构造函数。</summary>
    /// <remarks>此构造函数设置参数的 <see cref="ICapeIdentification.ComponentName"/> 和 <see cref="ICapeIdentification.ComponentDescription"/>。
    /// 参数的值和默认值设置为该值。此外，还设置了参数 CapeParamMode。</remarks>
    /// <param name="name">设置为参数的 ICapeIdentification 接口的 ComponentName。</param>
    /// <param name="description">设置为参数的 ICapeIdentification 接口的 ComponentDescription。</param>
    /// <param name="value">设置参数的初始值。</param>
    /// <param name="defaultValue">设置参数的默认值。</param>
    /// <param name="mode">设置参数的 CapeParamMode 模式。</param>
    public BooleanParameter(string name, string description, bool value, bool defaultValue, CapeParamMode mode)
        : base(name, description, mode)
    {
        _mValue = value;
        Mode = mode;
        _mDefaultValue = defaultValue;
        m_ValStatus = CapeValidationStatus.CAPE_VALID;
    }

    // ICloneable
    /// <summary>创建参数的副本。</summary>
    /// <remarks>克隆方法用于创建参数的深拷贝。</remarks>
    /// <returns>当前参数的副本。</returns>
    public override object Clone()
    {
        return new BooleanParameter(ComponentName, ComponentDescription, _mValue, _mDefaultValue, Mode);
    }

    /// <summary>获取和设置该参数的值。</summary>
    /// <remarks>参数的值。该参数值只能通过 .NET 使用，不能用于 COM。</remarks>
    /// <value>参数的值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    [Category("ICapeParameter")]
    public bool Value
    {
        get => _mValue;
        set
        {
            var args = new ParameterValueChangedEventArgs(ComponentName, _mValue, value);
            _mValue = value;
            OnParameterValueChanged(args);
        }
    }

    /// <summary>根据参数说明验证参数的当前值。</summary>
    /// <remarks>此方法检查参数的当前值，以确定它是否是一个允许的值。对于 <see cref="ICapeBooleanParameterSpec"/> 参数，任何有效的布尔值（true/false）都是有效的。</remarks>
    /// <returns>如果参数有效，则为 true；如果无效，则为 false。</returns>
    /// <param name="message">消息用于返回参数无效的原因。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用。</exception>
    public override bool Validate(ref string message)
    {
        message = "Value is valid.";
        m_ValStatus = CapeValidationStatus.CAPE_VALID;
        var args = new ParameterValidatedEventArgs(ComponentName, message, CapeValidationStatus.CAPE_VALID, CapeValidationStatus.CAPE_VALID);
        OnParameterValidated(args);
        return true;
    }

    /// <summary>将参数值设置为默认值。</summary>
    /// <remarks>将参数值设置为默认值。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    public override void Reset()
    {
        var args = new ParameterResetEventArgs(ComponentName);
        _mValue = _mDefaultValue;
        OnParameterReset(args);
    }

    // ICapeParameterSpec
    /// <summary>获取参数的类型。</summary>
    /// <remarks>获取此 <see cref="CapeParamType"/> 参数的类型：real(CAPE_REAL), integer(CAPE_INT),
    /// option(CAPE_OPTION), boolean(CAPE_BOOLEAN) or array(CAPE_ARRAY)。</remarks>
    /// <value>参数的类型。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用。</exception>
    [Category("ICapeParameterSpec")]
    public override CapeParamType Type => CapeParamType.CAPE_BOOLEAN;

    //ICapeBooleanParameterSpec
    /// <summary>获取和设置参数的默认值。</summary>
    /// <remarks>获取和设置参数的默认值。</remarks>
    /// <value>参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    [Category("ICapeBooleanParameterSpec")]
    public bool DefaultValue
    {
        get => _mDefaultValue;
        set
        {
            var args = new ParameterDefaultValueChangedEventArgs(ComponentName, _mDefaultValue, value);
            _mDefaultValue = value;
            OnParameterDefaultValueChanged(args);
        }
    }

    /// <summary>根据参数说明验证发送的值。</summary>
    /// <remarks>验证参数是否接受该参数作为有效值。它返回一个标志，表示验证成功或失败，以及一条文本消息，可用于向客户端/用户传达推理。</remarks>
    /// <returns>如果参数有效，则为 true；如果无效，则为 false。</returns>
    /// <param name="pValue">将根据参数当前规范进行验证的布尔值。</param>
    /// <param name="message">指向字符串的引用，该字符串将包含与参数验证相关的信息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    public bool Validate(bool pValue, ref string message)
    {
        message = "Value is valid.";
        return true;
    }
}

/// <summary>布尔值参数，用于 CAPE-OPEN 参数集。</summary>
/// <remarks>布尔值参数，用于 CAPE-OPEN 参数集。</remarks>
[Serializable,ComVisible(true)]
[ComSourceInterfaces(typeof(ICapeBooleanParameterSpecEvents))]
[Guid("A6751A39-8A2C-4AFC-AD57-6395FFE0A7FE")]
[ClassInterface(ClassInterfaceType.None)]
internal class BooleanParameterWrapper : CapeParameter, ICapeParameter, ICapeParameterSpec, ICapeBooleanParameterSpec
{
    private ICapeParameter _mParameter;

    /// <summary>获取和设置该参数的值。</summary>
    /// <remarks>该值使用 System.Object 数据类型，以便与基于 COM 的 CAPE-OPEN 兼容。</remarks>
    /// <value>参数的方框（可选？）布尔值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    [Browsable(false)]
    public override object value
    {
        get => _mParameter.value;
        set
        {
            var args = new ParameterValueChangedEventArgs(ComponentName, _mParameter.value, Convert.ToBoolean(value));
            _mParameter.value = value;
            OnParameterValueChanged(args);
        }
    }
    
    // 构造函数中存在 virtual 成员调用
    /// <summary>基于 COM 的布尔值参数包装类的构造函数。</summary>
    /// <remarks>这个构造函数创建一个封装了基于COM的布尔值参数的类的实例。这个包装器为被包装的参数暴露了适当的基于 .NET 的参数接口。</remarks>
    /// <param name="parameter">要包装的参数。</param>
    public BooleanParameterWrapper(ICapeParameter parameter)
        : base(string.Empty, string.Empty, parameter.Mode)
    {
        _mParameter = parameter;
        ComponentName = ((ICapeIdentification)parameter).ComponentName;
        ComponentDescription = ((ICapeIdentification)parameter).ComponentDescription;
        Mode = parameter.Mode;
        m_ValStatus = parameter.ValStatus;
    }
        
    // ICloneable
    /// <summary>创建参数的副本。</summary>
    /// <remarks>克隆方法用于创建参数的副本。原始版本和克隆都引用相同的基于 COM 的参数。</remarks>
    /// <returns>当前参数的副本。</returns>
    public override object Clone()
    {
        return new BooleanParameterWrapper(_mParameter);
    }

    /// <summary>获取和设置该参数的值。</summary>
    /// <remarks>参数的值。</remarks>
    /// <value>参数的值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    [Category("ICapeParameter")]
    public bool Value
    {
        get => (bool)_mParameter.value;
        set
        {
            var args = new ParameterValueChangedEventArgs(ComponentName, (bool)_mParameter.value, value);
            _mParameter.value = value;
            OnParameterValueChanged(args);
        }
    }

    /// <summary>根据参数说明验证参数的当前值。</summary>
    /// <remarks>此方法检查参数的当前值，以确定它是否是一个允许的值。任何有效的布尔值（true/false）对于 <see cref="ICapeBooleanParameterSpec"/> 参数都是有效的。</remarks>
    /// <returns>如果参数有效，则为 true；如果无效，则为 false。</returns>
    /// <param name="message">消息用于返回参数无效的原因。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用。</exception>
    public override bool Validate(ref string message)
    {
        var valid = _mParameter.ValStatus;
        var retVal = _mParameter.Validate(message);
        var args = new ParameterValidatedEventArgs(ComponentName, message, valid, _mParameter.ValStatus);
        OnParameterValidated(args);
        return retVal;
    }

    /// <summary>将参数值设置为默认值。</summary>
    /// <remarks>将参数值设置为默认值。</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    public override void Reset()
    {
        var args = new ParameterResetEventArgs(ComponentName);
        _mParameter.Reset();
        OnParameterReset(args);
    }

    // ICapeParameterSpec
    /// <summary>获取参数的类型。</summary>
    /// <remarks>获取参数的 <see cref="CapeParamType"/>。</remarks>
    /// <value>参数的类型。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用。</exception>
    [Category("ICapeParameterSpec")]
    public override CapeParamType Type => CapeParamType.CAPE_BOOLEAN;

    //ICapeBooleanParameterSpec
    /// <summary>获取封装参数的默认值。</summary>
    /// <remarks>基于 COM 的 <see cref="ICapeBooleanParameterSpec"/> 布尔接口没有提供更改参数默认值的方法。因此，无法更改封装的参数的默认值。</remarks>
    /// <value>参数的默认值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    [Category("ICapeBooleanParameterSpec")]
    public bool DefaultValue
    {
        get => ((ICapeBooleanParameterSpec)_mParameter.Specification).DefaultValue;
        set { }
    }

    /// <summary>根据参数说明验证发送的值。</summary>
    /// <remarks>验证参数是否接受该参数作为有效值。它返回一个标志，表示验证成功或失败，以及一条文本消息，可用于向客户端/用户传达推理。</remarks>
    /// <returns>如果参数有效，则为 true；如果无效，则为 false。</returns>
    /// <param name="pValue">将根据参数当前规范进行验证的布尔值。</param>
    /// <param name="message">指向字符串的引用，该字符串将包含与参数验证相关的信息。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    public bool Validate(bool pValue, ref string message)
    {
        return ((ICapeBooleanParameterSpec)_mParameter.Specification).Validate(pValue, message);
    }
}