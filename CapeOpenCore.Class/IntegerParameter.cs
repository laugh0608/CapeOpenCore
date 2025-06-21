/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.21
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CapeOpenCore.Class;

/// <summary>表示整数型参数的规格已发生变更。</summary>
/// <remarks><para>该接口面向基于 COM 的 PMEs，并作为与整数值参数规范更改相关联的事件的源接口。</para>
/// <para>此接口并非 CAPE-OPEN 规范的一部分。提供此接口及其实现，是为了让基于 COM 的开发人员能够获得与基于 .NET 的开发人员相似的功能。</para></remarks>
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[ComVisible(true)]
[Guid("2EA7C47A-A4E0-47A2-8AC1-658F96A0B79D")]
[Description("CapeIntegerParameterEvents Interface")]
internal interface ICapeIntegerParameterSpecEvents
{
    /// <summary>当用户更改参数的默认值时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnParameterDefaultValueChanged</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>当在派生类中重写 <c>OnParameterDefaultValueChanged</c> 时，
    /// 务必调用基类的 <c>OnParameterDefaultValueChange</c> 方法，以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="sender">引发事件的 <see cref="IntegerParameter">RealParameter</see>。</param>
    /// <param name="args">一个包含事件信息的 <see cref="ParameterDefaultValueChanged"/>。</param>
    void ParameterDefaultValueChanged(
        [MarshalAs(UnmanagedType.IDispatch)]
        object sender,
        [MarshalAs(UnmanagedType.IDispatch)]
        object args
    );

    /// <summary>当用户更改参数的下限时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnComponentNameChanged</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>当在派生类中重写 <c>OnComponentNameChanged</c> 时，
    /// 务必调用基类的 <c>OnComponentNameChanged </c> 方法，以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="sender">引发事件的 <see cref="IntegerParameter">RealParameter</see>。</param>
    /// <param name="args">一个包含事件信息的 <see cref="ParameterValueChangedEventArgs"/>。</param>
    void ParameterLowerBoundChanged(
        [MarshalAs(UnmanagedType.IDispatch)]
        object sender,
        [MarshalAs(UnmanagedType.IDispatch)]
        object args
    );

    /// <summary>当用户更改参数的上限时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnParameterUpperBoundChanged</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选方法。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>在派生类中重写 <c>OnParameterUpperBoundChanged</c> 时，
    /// 务必调用基类的 <c>OnParameterUpperBoundChanged</c> 方法，以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="sender">引发事件的 <see cref="IntegerParameter">RealParameter</see>。</param>
    /// <param name="args">一个包含事件信息的 <see cref="ParameterUpperBoundChangedEventArgs"/>。</param>
    void ParameterUpperBoundChanged(object sender, object args);

    /// <summary>当参数验证时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnParameterValidated</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>当在派生类中重写 <c>OnParameterValidated</c> 时，
    /// 务必调用基类的 <c>OnParameterValidated </c> 方法，以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="sender">引发事件的 <see cref="IntegerParameter">RealParameter</see>。</param>
    /// <param name="args">一个包含事件相关信息的 <see cref="ParameterValidatedEventArgs"/>。</param>
    void ParameterValidated(object sender, object args);
}

/// <summary>整数型参数，用于 CAPE-OPEN 参数集合。</summary>
/// <remarks>整数型参数，用于 CAPE-OPEN 参数集合。</remarks>
[Serializable]
[ComVisible(true)]
[ComSourceInterfaces(typeof(ICapeIntegerParameterSpecEvents))]
[Guid("2C57DC9F-1368-42eb-888F-5BC6ED7DDFA7")]
[ClassInterface(ClassInterfaceType.None)]
public class IntegerParameter : CapeParameter, ICapeParameter, ICapeParameterSpec, ICapeParameterSpecCOM,
    ICapeIntegerParameterSpec  //, INotifyPropertyChanged
{
    private int _mValue;
    private int _mDefaultValue, _mLowerBound, _mUpperBound;

    /// <summary>获取并设置此参数的值。</summary>
    /// <remarks>此值使用 System.Object 数据类型以确保与基于 COM 的 CAPE-OPEN 兼容。</remarks>
    /// <value>该参数的值。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Browsable(false)]
    public override object value
    {
        get => _mValue;
        set
        {
            var args = new ParameterValueChangedEventArgs(ComponentName, _mValue, value);
            _mValue = (int)value;
            OnParameterValueChanged(args);
            NotifyPropertyChanged("Value");
        }
    }

    /// <summary>创建整数值参数类的实例。</summary>
    /// <remarks>默认值设置为参数的初始值。上限设置为 Int32.MaxValue（2,147,483,647），
    /// 下限设置为 Int32.MinValue（-2,147,483,648）。类型设置为 CapeParamMode.CAPE_INPUT_OUTPUT。</remarks>
    /// <param name="name">将该参数的 ICapeIdentification 接口的 ComponentName 设置为指定值。</param>
    /// <param name="value">设置参数的初始值。</param>
    /// <param name="mode">设置参数的 CapeParamMode 模式。</param>
    public IntegerParameter(string name, int value, CapeParamMode mode)
        : base(name, string.Empty, mode)
    {
        _mValue = value;
        Mode = mode;
        _mLowerBound = int.MinValue;
        _mUpperBound = int.MaxValue;
        _mDefaultValue = value;
    }

    /// <summary>使用输入的值创建整数值参数类的实例。</summary>
    /// <remarks>该构造函数中指定了参数的默认值、上限、下限以及众数。</remarks>
    /// <param name="name">将该参数的 ICapeIdentification 接口的 ComponentName 设置为指定值。</param>
    /// <param name="description">将参数的 ICapeIdentification 接口设置为组件描述。</param>
    /// <param name="value">设置参数的初始值。</param>
    /// <param name="defaultValue">设置参数的默认值。</param>
    /// <param name="minValue">设置参数的下限值。</param>
    /// <param name="maxValue">设置参数的上限值。</param>
    /// <param name="mode">设置参数的 CapeParamMode 模式。</param>
    public IntegerParameter(string name, string description, int value, int defaultValue, int minValue, int maxValue,
        CapeParamMode mode) : base(name, description, mode)
    {
        _mValue = value;
        Mode = mode;
        _mLowerBound = minValue;
        _mUpperBound = maxValue;
        _mDefaultValue = defaultValue;
        var message = "";
        if (!Validate(ref message))
        {
            MessageBox.Show(message, string.Concat("Invalid Parameter Value: ", ComponentName));
        }
    }

    /// <summary>当用户更改参数的下限时发生。</summary>
    /// <remarks>触发事件时，会通过委托调用事件处理程序。</remarks>
    public event ParameterLowerBoundChangedHandler ParameterLowerBoundChanged;

    /// <summary>当用户更改参数的下限时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnComponentNameChanged</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选技术。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>当在派生类中重写 <c>OnComponentNameChanged</c> 时，
    /// 务必调用基类的 <c>OnComponentNameChanged </c> 方法，以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="args">一个包含事件信息的 <see cref="ParameterValueChangedEventArgs"/>。</param>
    protected void OnParameterLowerBoundChanged(ParameterLowerBoundChangedEventArgs args)
    {
        ParameterLowerBoundChanged?.Invoke(this, args);
    }

    /// <summary>当用户更改参数的上限时发生。</summary>
    /// <remarks>触发事件时，会通过委托调用事件处理程序。</remarks>
    public event ParameterUpperBoundChangedHandler ParameterUpperBoundChanged;

    /// <summary>当用户更改参数的上限时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnParameterUpperBoundChanged</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选方法。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>在派生类中重写 <c>OnParameterUpperBoundChanged</c> 时，
    /// 务必调用基类的 <c>OnParameterUpperBoundChanged</c> 方法，以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="args">一个包含事件信息的 <see cref="ParameterUpperBoundChangedEventArgs"/>。</param>
    protected void OnParameterUpperBoundChanged(ParameterUpperBoundChangedEventArgs args)
    {
        ParameterUpperBoundChanged?.Invoke(this, args);
    }

    // ICloneable
    /// <summary>创建参数的副本。</summary>
    /// <remarks>克隆方法用于创建参数的深度副本。</remarks>
    /// <returns>当前参数的副本。</returns>
    public override object Clone()
    {
        return new IntegerParameter(ComponentName, ComponentDescription, _mValue, _mDefaultValue,
            _mLowerBound, _mUpperBound, Mode);
    }


    /// <summary>获取并设置此参数的值。</summary>
    /// <remarks>该参数的值。</remarks>
    /// <value>该参数的值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Category("ICapeParameter")]
    public int Value
    {
        get => _mValue;
        set
        {
            var args = new ParameterValueChangedEventArgs(ComponentName, _mValue, value);
            _mValue = value;
            OnParameterValueChanged(args);
            NotifyPropertyChanged("Value");
        }
    }

    /// <summary>根据参数的规范验证参数的当前值。</summary>
    /// <remarks>如果当前值在上限和下限之间，则认为该参数有效。消息用于返回参数无效的原因。</remarks>
    /// <returns>如果参数有效则为真，否则为假。</returns>
    /// <param name="message">引用一个字符串，该字符串将包含关于参数验证的消息。</param>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    public override bool Validate(ref string message)
    {
        ParameterValidatedEventArgs args;
        if (_mValue < _mLowerBound)
        {
            message = "Value below the Lower Bound.";
            args = new ParameterValidatedEventArgs(ComponentName, message, m_ValStatus,
                CapeValidationStatus.CAPE_INVALID);
            m_ValStatus = CapeValidationStatus.CAPE_INVALID;
            NotifyPropertyChanged("ValStatus");
            OnParameterValidated(args);
            return false;
        }

        if (_mValue > _mUpperBound)
        {
            message = "Value greater than upper bound.";
            args = new ParameterValidatedEventArgs(ComponentName, message, m_ValStatus,
                CapeValidationStatus.CAPE_INVALID);
            m_ValStatus = CapeValidationStatus.CAPE_INVALID;
            NotifyPropertyChanged("ValStatus");
            OnParameterValidated(args);
            return false;
        }

        message = "Value is valid.";
        args = new ParameterValidatedEventArgs(ComponentName, message, m_ValStatus,
            CapeValidationStatus.CAPE_VALID);
        m_ValStatus = CapeValidationStatus.CAPE_VALID;
        NotifyPropertyChanged("ValStatus");
        OnParameterValidated(args);
        return true;
    }

    /// <summary>将参数的值设置为其默认值。</summary>
    /// <remarks>此方法将参数的值设置为默认值。</remarks>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    public override void Reset()
    {
        var args = new ParameterResetEventArgs(ComponentName);
        _mValue = _mDefaultValue;
        OnParameterReset(args);
        NotifyPropertyChanged("Value");
    }

    // ICapeParameterSpec
    /// <summary>获取参数的类型。</summary>
    /// <remarks>获取此参数对应的 <see cref="CapeParamType"/>：real（CAPE_REAL）、integer（CAPE_INT）、
    /// option（CAPE_OPTION）、boolean（CAPE_BOOLEAN）或 array（CAPE_ARRAY）。</remarks>
    /// <value>参数的类型。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用。</exception>
    [Category("ICapeParameterSpec")]
    public override CapeParamType Type => CapeParamType.CAPE_INT;

    //ICapeIntegerParameterSpec
    /// <summary>获取并设置参数的默认值。</summary>
    /// <remarks>获取并设置参数的默认值。</remarks>
    /// <value>参数的默认值。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Category("ICapeIntegerParameterSpec")]
    public int DefaultValue
    {
        get => _mDefaultValue;
        set
        {
            var args = new ParameterDefaultValueChangedEventArgs(ComponentName, _mDefaultValue, value);
            _mDefaultValue = value;
            OnParameterDefaultValueChanged(args);
            NotifyPropertyChanged("DefaultValue");
        }
    }

    /// <summary>获取并设置参数的下限。</summary>
    /// <remarks>下限可以是一个有效的整数。默认情况下，它设置为 Int32.MinValue，即 2,147,483,648；也就是十六进制 0x80000000。</remarks>
    /// <value>参数的下限值。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Category("ICapeIntegerParameterSpec")]
    public int LowerBound
    {
        get => _mLowerBound;
        set
        {
            var args = new ParameterLowerBoundChangedEventArgs(ComponentName, _mLowerBound, value);
            _mLowerBound = value;
            OnParameterLowerBoundChanged(args);
            NotifyPropertyChanged("LowerBound");
        }
    }

    /// <summary>获取并设置参数的上限。</summary>
    /// <remarks>上限可以是一个有效的整数。默认情况下，它被设置为 Int32.MaxValue，即 2,147,483,647；也就是十六进制 0x7FFFFFFF。</remarks>
    /// <value>参数的上限值。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Category("ICapeIntegerParameterSpec")]
    public int UpperBound
    {
        get => _mUpperBound;
        set
        {
            var args = new ParameterUpperBoundChangedEventArgs(ComponentName, _mUpperBound, value);
            _mUpperBound = value;
            OnParameterUpperBoundChanged(args);
            NotifyPropertyChanged("UpperBound");
        }
    }

    /// <summary>验证发送的值是否符合参数的规格。</summary>
    /// <remarks>如果当前值在上限和下限之间，则认为该参数有效。消息用于返回参数无效的原因。</remarks>
    /// <returns>如果参数有效则为真，否则为假。</returns>
    /// <param name="pValue">将与参数当前规格进行验证的整数值。</param>
    /// <param name="message">引用一个字符串，该字符串将包含关于参数验证的消息。</param>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    public bool Validate(int pValue, ref string message)
    {
        if (pValue < _mLowerBound)
        {
            message = "Value below the Lower Bound.";
            return false;
        }

        if (pValue > _mUpperBound)
        {
            message = "Value greater than upper bound.";
            return false;
        }

        message = "Value is valid.";
        return true;
    }
}

/// <summary>用于 CAPE-OPEN 参数集合的整型参数。</summary>
/// <remarks>用于 CAPE-OPEN 参数集合的整型参数。</remarks>
[Serializable]
[ComVisible(true)]
[ComSourceInterfaces(typeof(ICapeIntegerParameterSpecEvents))]
[Guid("EFC01B53-9A6A-4AD9-97BE-3F0294B3BBFB")] //ICapeThermoMaterialObject_IID)
[ClassInterface(ClassInterfaceType.None)]
internal class IntegerParameterWrapper : CapeParameter, ICapeParameter, ICapeParameterSpec, 
    ICapeIntegerParameterSpec, ICloneable  //, INotifyPropertyChanged
{
    private ICapeParameter _mParameter;

    /// <summary>获取并设置此参数的值。</summary>
    /// <remarks>此值使用 System.Object 数据类型以确保与基于 COM 的 CAPE-OPEN 兼容。</remarks>
    /// <value>参数的值。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Browsable(false)]
    public override object value
    {
        get => _mParameter.value;
        set
        {
            var args = new ParameterValueChangedEventArgs(ComponentName, _mParameter.value, value);
            _mParameter.value = (int)value;
            OnParameterValueChanged(args);
            NotifyPropertyChanged("Value");
        }
    }

    /// <summary>创建整数值参数类的实例。</summary>
    /// <remarks>默认值设置为参数的初始值。上限设置为 Int32.MaxValue（2,147,483,647），
    /// 下限设置为 Int32.MinValue（-2,147,483,648）。模式设置为 CapeParamMode.CAPE_INPUT_OUTPUT。</remarks>
    /// <param name="parameter">设置参数的初始值。</param>
    public IntegerParameterWrapper(ICapeParameter parameter) 
        : base(string.Empty, string.Empty, parameter.Mode)
    {
        _mParameter = parameter;
        ComponentName = ((ICapeIdentification)parameter).ComponentName;
        ComponentDescription = ((ICapeIdentification)parameter).ComponentDescription;
        Mode = parameter.Mode;
        m_ValStatus = parameter.ValStatus;
    }

    /// <summary>创建一个与当前实例相同的副本对象。</summary>
    /// <remarks><para>克隆的实现方式可以是深度复制或浅度复制。在深度复制中，所有对象都会被复制；
    /// 在浅度复制中，只复制顶层对象，而较低级别的对象包含引用。</para>
    /// <para>生成的克隆必须与原始实例属于同一类型或与之兼容。</para>
    /// <para>有关克隆、深拷贝与浅拷贝的详细信息及示例，请参阅 <see cref="Object.MemberwiseClone"/>。</para></remarks>
    /// <returns>一个与该实例相同的副本对象。</returns>
    public override object Clone()
    {
        return new IntegerParameter(ComponentName, ComponentDescription, Value, DefaultValue,
            LowerBound, UpperBound, Mode);
    }

    /// <summary>当用户更改参数的下限时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para></remarks>
    public event ParameterLowerBoundChangedHandler ParameterLowerBoundChanged;

    /// <summary>当用户更改参数的下限时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnParameterLowerBoundChanged</c> 方法还允许派生类在不附加委托的情况下处理该事件。这是在派生类中处理该事件的首选方法。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>在派生类中重写 <c>OnParameterLowerBoundChanged</c> 方法时，
    /// 请务必调用基类的 <c>OnComponentNameChanged</c> 方法，以便已注册的委托能够收到该事件。</para></remarks>
    /// <param name="args">一个包含事件信息的 <see cref="ParameterValueChangedEventArgs"/>。</param>
    protected void OnParameterLowerBoundChanged(ParameterLowerBoundChangedEventArgs args)
    {
        ParameterLowerBoundChanged?.Invoke(this, args);
    }

    /// <summary>当用户更改参数的上限时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para></remarks>
    public event ParameterUpperBoundChangedHandler ParameterUpperBoundChanged;

    /// <summary>当用户更改参数的上限时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnParameterUpperBoundChanged</c> 方法还允许子类在不附加委托的情况下处理事件。这是子类处理事件的优选方法。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>在派生类中重写 <c>OnParameterUpperBoundChanged</c> 时，
    /// 务必调用基类的 <c>OnParameterUpperBoundChanged</c> 方法，以便注册的委托能够接收到该事件。</para></remarks>
    /// <param name="args">一个包含事件信息的 <see cref="ParameterUpperBoundChangedEventArgs"/>。</param>
    protected void OnParameterUpperBoundChanged(ParameterUpperBoundChangedEventArgs args)
    {
        ParameterUpperBoundChanged?.Invoke(this, args);
    }

    // ICloneable
    /// <summary>创建一份参数的副本。</summary>
    /// <remarks>克隆方法用于创建参数的深度副本。</remarks>
    /// <returns>当前参数的副本。</returns>
    object ICloneable.Clone()
    {
        return new IntegerParameterWrapper(_mParameter);
    }
    
    /// <summary>获取并设置此参数的值。</summary>
    /// <remarks>参数的值。</remarks>
    /// <value>参数的值。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Category("ICapeParameter")]
    public int Value
    {
        get => (int)_mParameter.value;
        set
        {
            var args = new ParameterValueChangedEventArgs(ComponentName, _mParameter.value, value);
            _mParameter.value = value;
            OnParameterValueChanged(args);
            NotifyPropertyChanged("Value");
        }
    }

    /// <summary>根据参数的规范验证参数的当前值。</summary>
    /// <remarks>如果当前值在上限和下限之间，则认为该参数有效。消息用于返回参数无效的原因。</remarks>
    /// <returns>如果参数有效则为真，否则为假。</returns>
    /// <param name="message">引用一个字符串，该字符串将包含关于参数验证的消息。</param>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
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
    /// <remarks>此方法将参数的值设置为默认值。</remarks>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    public override void Reset()
    {
        var args = new ParameterResetEventArgs(ComponentName);
        _mParameter.Reset();
        OnParameterReset(args);
        NotifyPropertyChanged("Value");
    }

    // ICapeParameterSpec
    /// <summary>获取参数的类型。</summary>
    /// <remarks>获取此参数对应的 <see cref="CapeParamType"/>：real (CAPE_REAL),
    /// integer(CAPE_INT), option(CAPE_OPTION), boolean(CAPE_BOOLEAN) 
    /// or array(CAPE_ARRAY).</remarks>
    /// <value>参数的类型。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用。</exception>
    [Category("ICapeParameterSpec")]
    public override CapeParamType Type => CapeParamType.CAPE_INT;

    //ICapeIntegerParameterSpec
    /// <summary>获取并设置参数的默认值。</summary>
    /// <remarks>获取并设置参数的默认值。</remarks>
    /// <value>参数的默认值。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Category("ICapeIntegerParameterSpec")]
    public int DefaultValue
    {
        get => ((ICapeIntegerParameterSpec)_mParameter.Specification).DefaultValue;
        set { }
    }

    /// <summary>获取并设置参数的下限。</summary>
    /// <remarks>下限可以是一个有效的整数。默认情况下，它设置为 Int32.MinValue，即 2,147,483,648；也就是十六进制 0x80000000。</remarks>
    /// <value>参数的下限。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Category("ICapeIntegerParameterSpec")]
    public int LowerBound
    {
        get => ((ICapeIntegerParameterSpec)_mParameter.Specification).LowerBound;
        set { }
    }

    /// <summary>获取并设置参数的上限。</summary>
    /// <remarks>上限可以是一个有效的整数，默认情况下，它设置为 Int32.MaxValue，即 2,147,483,647；也就是十六进制 0x7FFFFFFF。</remarks>
    /// <value>参数的上限。</value>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    [Category("ICapeIntegerParameterSpec")]
    public int UpperBound
    {
        get => ((ICapeIntegerParameterSpec)_mParameter.Specification).UpperBound;
        set { }
    }

    /// <summary>验证发送的值是否符合参数的规格。</summary>
    /// <remarks>如果当前值在上限和下限之间，则认为该参数有效。消息用于返回参数无效的原因。</remarks>
    /// <returns>如果参数有效则为真，否则为假。</returns>
    /// <param name="pValue">将与参数当前规格进行验证的整数值。</param>
    /// <param name="message">引用一个字符串，该字符串将包含关于参数验证的消息。</param>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    public bool Validate(int pValue, ref string message)
    {
        return ((ICapeIntegerParameterSpec)_mParameter.Specification).Validate(pValue, message);
    }
}