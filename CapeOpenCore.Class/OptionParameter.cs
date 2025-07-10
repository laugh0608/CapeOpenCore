/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.07.10
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

internal class OptionParameterValueConverter : StringConverter
{

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        var param = (OptionParameter)(context.Instance);
        return new StandardValuesCollection(param.OptionList);
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
        var param = (OptionParameter)context.Instance;
        return param.RestrictedToList;
    }
}

[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[ComVisible(true)]
[Guid("991F95FB-2210-4E44-99B3-4AB793FF46C2")]
[Description("CapeRealParameterEvents Interface")]
internal interface IOptionParameterSpecEvents
{
    /// <summary>当用户更改参数的默认值时发生。</summary>
    /// <remarks><para>引发事件时，事件处理程序会通过委托进行调用。</para>
    /// <para><c>OnParameterDefaultValueChanged</c> 方法还允许派生类在不附加委托的
    /// 情况下处理事件。这是在派生类中处理事件的首选技术。</para>
    /// <para>继承该方法须知：</para>
    /// <para>在派生类中重载 <c>OnParameterDefaultValueChanged</c> 时，请确保调用基类的
    /// <c>OnParameterDefaultValueChanged</c> 方法，以便已注册的委派接收事件。</para></remarks>
    /// <param name="sender">引发事件的 <see cref="RealParameter"/> 参数。</param>
    /// <param name="args"><see cref="ParameterDefaultValueChanged"/> 包含事件相关信息。</param>
    void ParameterDefaultValueChanged(object sender, object args);

    /// <summary>用户更改参数下限时出现。</summary>
    /// <remarks><para>引发事件时，事件处理程序会通过委托进行调用。</para>
    /// <para><c>OnComponentNameChanged</c> 方法还允许派生类在不附加委托的情况下处理事件。
    /// 这是在派生类中处理事件的首选技术。</para>
    /// <para>继承该方法须知：</para>
    /// <para>在派生类中重载 <c>OnComponentNameChanged</c> 时，请确保调用基类的
    /// <c>OnComponentNameChanged</c> 方法，以便已注册的委派接收事件。</para></remarks>
    /// <param name="sender">引发事件的 <see cref="RealParameter"/> 参数。</param>
    /// <param name="args">包含事件相关信息的 <see cref="ParameterValueChangedEventArgs"/> 文件。</param>
    void ParameterOptionListChanged(object sender, object args);

    /// <summary>当用户更改参数上限时出现。</summary>
    /// <remarks><para>引发事件时，事件处理程序会通过委托进行调用。</para>
    /// <para><c>OnParameterUpperBoundChanged</c> 方法还允许派生类在不附加委托的情况下处理事件。
    /// 这是在派生类中处理事件的首选技术。</para>
    /// <para>继承该方法须知：</para>
    /// <para>在派生类中重载 <c>OnParameterUpperBoundChanged</c> 时，请确保调用基类的
    /// <c>OnParameterUpperBoundChanged</c> 方法，以便已注册的委托能收到该事件。</para></remarks>
    /// <param name="sender">引发事件的 <see cref="RealParameter"/> 参数。</param>
    /// <param name="args">包含事件相关信息的 <see cref="ParameterUpperBoundChangedEventArgs"/> 文件。</param>
    void ParameterRestrictedToListChanged(object sender, object args);

    /// <summary>在验证参数时出现。</summary>
    /// <remarks><para>引发事件时，事件处理程序会通过委托进行调用。</para>
    /// <para><c>OnParameterValidated</c> 方法还允许派生类在不附加委托的情况下处理事件。这是在派生类中处理事件的首选技术。</para>
    /// <para>继承该方法须知：</para>
    /// <para>在派生类中重载 <c>OnParameterValidated</c> 时，请确保调用基类的 <c>OnParameterValidated</c> 方法，
    /// 以便已注册的委托能收到该事件。</para></remarks>
    /// <param name="sender">引发事件的 <see cref="RealParameter"/> 参数。</param>
    /// <param name="args">包含事件相关信息的 <see cref="ParameterValidatedEventArgs"/> 文件。</param>
    void ParameterValidated(object sender, object args);
}
    
/// <summary>实现 ICapeParameter 和 ICapeOptionParameterSpec CAPE-OPEN 接口的字符串参数类。</summary>
/// <remarks>该类实现了 ICapeParameter、ICapeParameterSpec、ICapeOptionParameterSpec
/// 和 ICapeIdentification。该类返回字符串或 System.Object，COM Interop 将其转换为包含 BSTR 的变量。</remarks>
[Serializable]
[ComSourceInterfaces(typeof(IOptionParameterSpecEvents))]
[ComVisible(true)]
[Guid("8EB0F647-618C-4fcc-A16F-39A9D57EA72E")]
[ClassInterface(ClassInterfaceType.None)]
public class OptionParameter : CapeParameter, ICapeParameter, ICapeParameterSpec,
    ICapeParameterSpecCOM, ICapeOptionParameterSpec, ICapeOptionParameterSpecCOM, ICloneable
{
    private string _mValue;
    private string _mDefaultValue;
    private string[] _mOptionList;
    private bool _mRestricted;
        
    /// <summary>获取和设置该参数的值。</summary>
    /// <remarks>该值使用 System.Object 数据类型，以便与基于 COM 的 CAPE-OPEN 兼容。</remarks>
    /// <value>System.Object</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，
    /// 未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Browsable(false)]
    public override object value
    {
        get => _mValue;
        set
        {
            var message = string.Empty; ;
            var args = new ParameterValueChangedEventArgs(ComponentName, _mValue, value);
            if (!Validate(value.ToString(), ref message)) 
                throw new CapeInvalidArgumentException(message, 0);
            _mValue = value.ToString();
            OnParameterValueChanged(args);
            NotifyPropertyChanged("Value");
            return;
        }
    }

    /// <summary>如果 RestrictedToList为 true，则获取参数的有效值列表。</summary>
    /// <remarks>如果 <see cref="RestrictedToList"/> 设置为 <c>true</c>，则用于验证参数。</remarks>
    /// <value>字符串数组作为 System.Object，COM Variant，包含 BSTR 的 SafeArray。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，
    /// 未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Browsable(false)]
    [Description("Gets and Sets the list of valid values for the parameter if 'RestrictedtoList' public is true.")]
    object ICapeOptionParameterSpecCOM.OptionList => _mOptionList;

    /// <summary>字符串值参数构造函数。</summary>
    /// <remarks>此构造函数设置参数的 ICapeIdentification.ComponentName 名称。参数的值和默认值将被设置为该值。</remarks>
    /// <param name="name">设置为参数的 ICapeIdentification 接口的 ComponentName。</param>
    /// <param name="value">设置参数的初始值。</param>
    public OptionParameter(string name, string value)
        : base(name, string.Empty, CapeParamMode.CAPE_INPUT_OUTPUT)
    {
        _mValue = value;
        Mode = CapeParamMode.CAPE_INPUT_OUTPUT;
        _mDefaultValue = value;
        m_ValStatus = CapeValidationStatus.CAPE_VALID;
    }
    
    /// <summary>Constructor for the boolean-valued parameter</summary>
    /// <remarks>This constructor sets the ICapeIdentification.ComponentName and 
    /// ICapeIdentification.ComponentDescription of the 
    /// parameter. The parameter's value and default value are set to the value. 
    /// Additionally, the parameters CapeParameterMode is set.</remarks>
    /// <param name="name">Sets as the ComponentName of the parameter's ICapeIdentification interface.</param>
    /// <param name="description">Sets as the ComponentDescription of the parameter's ICapeIdentification interface.</param>
    /// <param name="value">Sets the inital value of the parameter.</param>
    /// <param name="defaultValue">Sets the default value of the parameter.</param>
    /// <param name="options">String array used as the list acceptable options.</param>
    /// <param name="restricted">Sets whether the parameter value is restricted to values in the option list.</param>
    /// <param name="mode">Sets the CapeParamMode mode of the parameter.</param>
    public OptionParameter(string name, string description, string value, string defaultValue, string[] options, bool restricted, CapeParamMode mode)
        : base(name, description, mode)
    {
        _mValue = value;
        Mode = mode;
        _mDefaultValue = defaultValue;
        _mOptionList = options;
        _mRestricted = restricted;
    }

    /// <summary>Creates a new object that is a copy of the current instance.</summary>
    /// <remarks><para>Clone can be implemented either as a deep copy or a shallow copy. In a deep copy, all objects are duplicated; 
    /// in a shallow copy, only the top-level objects are duplicated
    /// and the lower levels contain references.</para>
    /// <para>The resulting clone must be of the same type as, or compatible with, the original instance.</para>
    /// <para>See <see cref="Object.MemberwiseClone"/> for more information on cloning,
    /// deep versus shallow copies, and examples.</para></remarks>
    /// <returns>A new object that is a copy of this instance.</returns>
    public override object Clone()
    {
        return new OptionParameter(ComponentName, ComponentDescription, Value, 
            DefaultValue, OptionList, RestrictedToList, Mode);
    }

    /// <summary>Occurs when the user changes of the lower bound of the parameter changes.</summary>
    public event ParameterOptionListChangedHandler ParameterOptionListChanged;
    
    /// <summary>Occurs when the user changes of the option list of a parameter.</summary>
    /// <remarks><para>引发事件时，事件处理程序会通过委托进行调用。</para>
    /// <para>The <c>OnParameterOptionListChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>继承该方法须知：</para>
    /// <para>When overriding <c>OnParameterOptionListChanged</c> in a derived class, be sure to call the base class's <c>OnParameterOptionListChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ParameterValueChangedEventArgs">ParameterOptionListChangedEventArgs</see> that contains information about the event.</param>
    protected void OnParameterOptionListChanged(ParameterOptionListChangedEventArgs args)
    {
        if (ParameterOptionListChanged != null)
        {
            ParameterOptionListChanged(this, args);
        }
    }

    /// <summary>Occurs when the user changes of the upper bound of the parameter changes.</summary>
    public event ParameterRestrictedToListChangedHandler ParameterRestrictedToListChanged;
    /// <summary>Occurs when the user changes of the upper bound of a parameter.</summary>
    /// <remarks><para>引发事件时，事件处理程序会通过委托进行调用。</para>
    /// <para><c>OnParameterUpperBoundChanged</c> 方法还允许派生类在不附加委托的情况下处理事件。这是在派生类中处理事件的首选技术。</para>
    /// <para>继承该方法须知：</para>
    /// <para>在派生类中重载 <c>OnParameterUpperBoundChanged</c> 时，请确保调用基类的 <c>OnParameterUpperBoundChanged</c> 方法，以便已注册的委托能收到该事件。</para></remarks>
    /// <param name="args">A <see cref="ParameterLowerBoundChangedEventArgs">ParameterUpperBoundChangedEventArgs</see> that contains information about the event.</param>
    protected void OnParameterRestrictedToListChanged(ParameterRestrictedToListChangedEventArgs args)
    {
        if (ParameterRestrictedToListChanged != null)
        {
            ParameterRestrictedToListChanged(this, args);
        }
    }

    /// <summary>Gets and sets the value of the parameter.</summary>
    /// <remarks>The value is returned as a String, which marshals as a BSTR to COM.</remarks>
    /// <returns>
    /// System.String
    /// </returns>
    /// <value>
    /// The value of the parameter.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [TypeConverter(typeof(OptionParameterValueConverter))]
    [Category("ICapeParameter")]
    [Description("Gets and sets the value of the parameter.")]
    public string Value
    {
        [Description("Gets the value of the parameter.")]
        get
        {
            return _mValue;
        }
        [Description("Sets the value of the parameter.")]
        set
        {
            var message = string.Empty;
            var args = new ParameterValueChangedEventArgs(ComponentName, _mValue, value);
            if (Validate(value, ref message))
            {
                _mValue = value;
                OnParameterValueChanged(args);
                NotifyPropertyChanged("Value");
                return;
            }
            throw new CapeInvalidArgumentException(message, 0);
        }
    }

    /// <summary>Validates the current value of the parameter against the parameter's specification.</summary>
    /// <remarks>If the value of the <see cref="RestrictedToList">RestrictedToList</see>
    /// public is set to <c>true</c>, the parameter is valid only if the current value
    /// is included in the <see cref="OptionList">OptionList</see>. If the 
    /// value of <see cref="RestrictedToList">RestrictedToList</see> public is <c>false</c>
    /// any valid String is a valid value for the parameter.</remarks>
    /// <returns>True if the string argument is valid, false if it is not.</returns>
    /// <param name="message">Reference to a string that will conain a message regarding the validation of the parameter.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    public override bool Validate(ref string message)
    {
        ParameterValidatedEventArgs args;
        if (_mRestricted)
        {
            var inList = false;
            for (var i = 0; i < _mOptionList.Length; i++)
            {
                if (_mOptionList[i] == _mValue)
                {
                    inList = true;
                }
            }
            if (!inList)
            {
                message = "Value is not in the optin list.";
                args = new ParameterValidatedEventArgs(ComponentName, message, m_ValStatus, CapeValidationStatus.CAPE_INVALID);
                m_ValStatus = CapeValidationStatus.CAPE_INVALID;
                NotifyPropertyChanged("ValStatus");
                OnParameterValidated(args);
                return false;
            }
        }
        message = "Value is valid.";
        args = new ParameterValidatedEventArgs(ComponentName, message, m_ValStatus, CapeValidationStatus.CAPE_VALID);
        m_ValStatus = CapeValidationStatus.CAPE_VALID;
        NotifyPropertyChanged("ValStatus");
        OnParameterValidated(args);
        return true;
    }

    /// <summary>Sets the value of the parameter to its default value.</summary>
    /// <remarks> This method sets the parameter's value to the default value.</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    public override void Reset()
    {
        var args = new ParameterResetEventArgs(ComponentName);
        _mValue = _mDefaultValue;
        OnParameterReset(args);
        NotifyPropertyChanged("Value");
    }

    // ICapeParameterSpec
    /// <summary>Gets the type of the parameter. </summary>
    /// <remarks>Gets the <see cref="CapeParamType"/> of the parameter for which this is a specification: real 
    /// (CAPE_REAL), integer(CAPE_INT), option(CAPE_OPTION), boolean(CAPE_BOOLEAN) 
    /// or array(CAPE_ARRAY).</remarks>
    /// <value>The parameter type. </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Category("ICapeParameterSpec")]
    public override CapeParamType Type
    {
        get
        {
            return CapeParamType.CAPE_OPTION;
        }
    }

    //ICapeOptionParameterSpec

    /// <summary>Gets and Sets the default value of the parameter.</summary>
    /// <remarks>Gets and sets the default value of the parameter.</remarks>
    /// <value>The default value for the parameter. </value>
    [Category("ICapeOptionParameterSpec")]
    [Description("Gets and Sets the default value of the parameter.")]
    public string DefaultValue
    {
        get
        {
            return _mDefaultValue;
        }
        set
        {
            var message = string.Empty;
            var args = new ParameterValueChangedEventArgs(ComponentName, _mValue, value);
            if (Validate(value, ref message))
            {
                _mDefaultValue = value;
                OnParameterValueChanged(args);
                NotifyPropertyChanged("DefaultValue");
                return;
            }
            throw new CapeInvalidArgumentException(message, 0);
        }
    }

    /// <summary>Gets and Sets the list of valid values for the parameter if 'RestrictedtoList' public is true.</summary>
    /// <remarks>Used in validating the parameter if the <see cref="RestrictedToList">RestrictedToList</see>
    /// is set to <c>true</c>.</remarks>
    /// <value>
    /// The option list.
    /// </value>
    [Category("ICapeOptionParameterSpec")]
    [Description("Gets and Sets the list of valid values for the parameter if 'RestrictedtoList' public is true.")]
    public string[] OptionList
    {
        get
        {
            return _mOptionList;
        }
        set
        {
            var args = new ParameterOptionListChangedEventArgs(ComponentName);
            _mOptionList = value;
            OnParameterOptionListChanged(args);
            NotifyPropertyChanged("OptionList");
        }
    }


    /// <summary>A list of Strings that the valueo f the parameter will be validated against.</summary>
    /// <remarks>If <c>true</c>, the parameter's value will be validated against the Strings
    /// in the <see cref="OptionList">OptionList</see>.</remarks>
    /// <value>
    /// If <c>true</c>, the parameter's value will be validated against the Strings
    /// in the <see cref="OptionList">OptionList</see>.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Category("ICapeOptionParameterSpec")]
    [Description("Limits values of the parameter to the values in the option list if true.")]
    public bool RestrictedToList
    {
        get
        {
            return _mRestricted;
        }
        set
        {
            var args = new ParameterRestrictedToListChangedEventArgs(ComponentName, _mRestricted, value);
            _mRestricted = value;
            OnParameterRestrictedToListChanged(args);
            NotifyPropertyChanged("RestrictedToList");
        }
    }

    /// <summary>Validates the value against the parameter's specification.</summary>
    /// <remarks>If the value of the <see cref="RestrictedToList">RestrictedToList</see>
    /// public is set to <c>true</c>, the value to be tested is a valid value for the 
    /// parameter if it is included in the 
    /// <see cref="OptionList">OptionList</see>. If the 
    /// value of <see cref="RestrictedToList">RestrictedToList</see> public is <c>false</c>
    /// any valid String is a valid value for the parameter.</remarks>
    /// <returns>True if the string argument is valid, false if it is not.</returns>
    /// <param name="value">The string to be tested for validity.</param>
    /// <param name="message">Reference to a string that will conain a message regarding the validation of the parameter.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Description("Checks whether the value is an accepatble value for the parameter.")]
    public bool Validate(string value, ref string message)
    {
        if (_mRestricted)
        {
            var inList = false;
            for (var i = 0; i < _mOptionList.Length; i++)
            {
                if (_mOptionList[i] == value)
                {
                    inList = true;
                }
            }
            if (!inList)
            {
                message = "Value is not in the option list.";
                return false;
            }
        }
        message = "Value is valid.";
        return true;
    }
}


/// <summary>
/// Option(string)-Valued parameter for use in the CAPE-OPEN parameter collection.
/// </summary>
/// <remarks>
/// Option(string)-Valued parameter for use in the CAPE-OPEN parameter collection.
/// </remarks>
[Serializable]
[ComSourceInterfaces(typeof(IOptionParameterSpecEvents))]
[ComVisible(true)]
[Guid("70994E8C-179E-40E1-A51B-54A5C0F64A84")]
[ClassInterface(ClassInterfaceType.None)]
class OptionParameterWrapper : CapeParameter,
    ICapeParameter,
    ICapeParameterSpec,
    ICapeOptionParameterSpec,
    ICapeOptionParameterSpecCOM,
    ICloneable,
    INotifyPropertyChanged
{
    [NonSerialized]
    private ICapeParameter m_parameter;

    /// <summary>Gets and sets the value for this Parameter.</summary>
    /// <remarks>This value uses the System.Object data type for compatibility with 
    /// COM-based CAPE-OPEN.</remarks>
    /// <value>System.Object</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Browsable(false)]
    override public Object value
    {
        get
        {
            return m_parameter.value;
        }
        set
        {
            String message = String.Empty; ;
            ParameterValueChangedEventArgs args = new ParameterValueChangedEventArgs(ComponentName, m_parameter.value.ToString(), value);
            m_parameter.value = value.ToString();
            OnParameterValueChanged(args);
            NotifyPropertyChanged("Value");                
        }
    }

    /// <summary>Gets the list of valid values for the parameter if 'RestrictedtoList' public is true.</summary>
    /// <remarks>Used in validating the parameter if the <see cref="RestrictedToList">RestrictedToList</see>
    /// is set to <c>true</c>.</remarks>
    /// <value>
    /// String array as a System.Object, COM Variant containing a SafeArray of BSTR.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Browsable(false)]
    [Description("Gets the list of valid values for the parameter if 'RestrictedtoList' public is true.")]
    Object ICapeOptionParameterSpecCOM.OptionList
    {
        get
        {
            return ((ICapeOptionParameterSpecCOM)m_parameter.Specification).OptionList;
        }
    }


    /// <summary>Constructor for the String-valued parameter</summary>
    /// <remarks>This constructor sets the ICapeIdentification.ComponentName of the 
    /// parameter. The parameter's value and default value are set to the value. </remarks>
    /// <param name="parameter">Sets as the ComponentName of the parameter's ICapeIdentification interface.</param>
    public OptionParameterWrapper(ICapeParameter parameter)
        : base(String.Empty, string.Empty, parameter.Mode)
    {
        m_parameter = parameter;
        ComponentName = ((ICapeIdentification)parameter).ComponentName;
        ComponentDescription = ((ICapeIdentification)parameter).ComponentDescription;
        Mode = m_parameter.Mode;
        m_ValStatus = m_parameter.ValStatus;            
    }        

    /// <summary>Occurs when the user changes of the lower bound of the parameter changes.</summary>
    public event ParameterOptionListChangedHandler ParameterOptionListChanged;
    /// <summary>Occurs when the user changes of the option list of a parameter.</summary>
    /// <remarks><para>引发事件时，事件处理程序会通过委托进行调用。</para>
    /// <para>The <c>OnParameterOptionListChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>继承该方法须知：</para>
    /// <para>When overriding <c>OnParameterOptionListChanged</c> in a derived class, be sure to call the base class's <c>OnParameterOptionListChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ParameterValueChangedEventArgs">ParameterOptionListChangedEventArgs</see> that contains information about the event.</param>
    protected void OnParameterOptionListChanged(ParameterOptionListChangedEventArgs args)
    {
        if (ParameterOptionListChanged != null)
        {
            ParameterOptionListChanged(this, args);
        }
    }

    /// <summary>Occurs when the user changes of the upper bound of the parameter changes.</summary>
    public event ParameterRestrictedToListChangedHandler ParameterRestrictedToListChanged;
    /// <summary>Occurs when the user changes of the upper bound of a parameter.</summary>
    /// <remarks><para>引发事件时，事件处理程序会通过委托进行调用。</para>
    /// <para><c>OnParameterUpperBoundChanged</c> 方法还允许派生类在不附加委托的情况下处理事件。这是在派生类中处理事件的首选技术。</para>
    /// <para>继承该方法须知：</para>
    /// <para>在派生类中重载 <c>OnParameterUpperBoundChanged</c> 时，请确保调用基类的 <c>OnParameterUpperBoundChanged</c> 方法，以便已注册的委托能收到该事件。</para></remarks>
    /// <param name="args">A <see cref="ParameterLowerBoundChangedEventArgs">ParameterUpperBoundChangedEventArgs</see> that contains information about the event.</param>
    protected void OnParameterRestrictedToListChanged(ParameterRestrictedToListChangedEventArgs args)
    {
        if (ParameterRestrictedToListChanged != null)
        {
            ParameterRestrictedToListChanged(this, args);
        }
    }


    /// <summary>Creates a new object that is a copy of the current instance.</summary>
    /// <remarks><para>
    /// Clone can be implemented either as a deep copy or a shallow copy. In a deep copy, all objects are duplicated; 
    /// in a shallow copy, only the top-level objects are duplicated and the lower levels contain references.
    /// </para>
    /// <para>
    /// The resulting clone must be of the same type as, or compatible with, the original instance.
    /// </para>
    /// <para>
    /// See <see cref="Object.MemberwiseClone"/> for more information on cloning, deep versus shallow copies, and examples.
    /// </para></remarks>
    /// <returns>A new object that is a copy of this instance.</returns>
    override public object Clone()
    {
        return new OptionParameterWrapper(m_parameter);
    }

    /// <summary>Gets and sets the value of the parameter.</summary>
    /// <remarks>The value is returned as a String, which marshals as a BSTR to COM.</remarks>
    /// <returns>
    /// System.String
    /// </returns>
    /// <value>
    /// The value of the parameter.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [TypeConverter(typeof(OptionParameterValueConverter))]
    [Category("ICapeParameter")]
    [Description("Gets and sets the value of the parameter.")]
    public String Value
    {
        [Description("Gets the value of the parameter.")]
        get
        {
            return m_parameter.value.ToString();
        }
        [Description("Sets the value of the parameter.")]
        set
        {
            String message = string.Empty;
            ParameterValueChangedEventArgs args = new ParameterValueChangedEventArgs(ComponentName, m_parameter.value.ToString(), value);
            m_parameter.value = value;
            OnParameterValueChanged(args);
            NotifyPropertyChanged("Value");                
        }
    }

    /// <summary>Validates the current value of the parameter against the parameter's specification.</summary>
    /// <remarks>If the value of the <see cref="RestrictedToList">RestrictedToList</see>
    /// public is set to <c>true</c>, the parameter is valid only if the current value
    /// is included in the <see cref="OptionList">OptionList</see>. If the 
    /// value of <see cref="RestrictedToList">RestrictedToList</see> public is <c>false</c>
    /// any valid String is a valid value for the parameter.</remarks>
    /// <returns>True if the string argument is valid, false if it is not.</returns>
    /// <param name="message">Reference to a string that will conain a message regarding the validation of the parameter.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    public override bool Validate(ref String message)
    {
        ParameterValidatedEventArgs args;
        CapeValidationStatus valid = m_parameter.ValStatus;
        bool retVal = m_parameter.Validate(message);
        args = new ParameterValidatedEventArgs(ComponentName, message, valid, m_parameter.ValStatus);
        NotifyPropertyChanged("ValStatus");
        OnParameterValidated(args);
        return true;
    }

    /// <summary>Sets the value of the parameter to its default value.</summary>
    /// <remarks> This method sets the parameter's value to the default value.</remarks>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    public override void Reset()
    {
        ParameterResetEventArgs args = new ParameterResetEventArgs(ComponentName);
        m_parameter.Reset();
        OnParameterReset(args);
        NotifyPropertyChanged("Value");
    }

    // ICapeParameterSpec
    /// <summary>Gets the type of the parameter. </summary>
    /// <remarks>Gets the <see cref="CapeParamType"/> of the parameter for which this is a specification: real 
    /// (CAPE_REAL), integer(CAPE_INT), option(CAPE_OPTION), boolean(CAPE_BOOLEAN) 
    /// or array(CAPE_ARRAY).</remarks>
    /// <value>The parameter type. </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Category("ICapeParameterSpec")]
    public override CapeParamType Type
    {
        get
        {
            return CapeParamType.CAPE_OPTION;
        }
    }

    //ICapeOptionParameterSpec

    /// <summary>Gets and Sets the default value of the parameter.</summary>
    /// <remarks>Gets and sets the default value of the parameter.</remarks>
    /// <value>The default value for the parameter. </value>
    [Category("ICapeOptionParameterSpec")]
    [Description("Gets and Sets the default value of the parameter.")]
    public String DefaultValue
    {
        get
        {
            return ((ICapeOptionParameterSpecCOM)m_parameter.Specification).DefaultValue;
        }
        set
        {
        }
    }

    /// <summary>Gets and Sets the list of valid values for the parameter if 'RestrictedtoList' public is true.</summary>
    /// <remarks>Used in validating the parameter if the <see cref="RestrictedToList">RestrictedToList</see>
    /// is set to <c>true</c>.</remarks>
    /// <value>
    /// The option list.
    /// </value>
    [Category("ICapeOptionParameterSpec")]
    [Description("Gets and Sets the list of valid values for the parameter if 'RestrictedtoList' public is true.")]
    public String[] OptionList
    {
        get
        {
            return (string[])((ICapeOptionParameterSpecCOM)m_parameter.Specification).OptionList;
        }
        set
        {
        }
    }


    /// <summary>A list of Strings that the valueo f the parameter will be validated against.</summary>
    /// <remarks>If <c>true</c>, the parameter's value will be validated against the Strings
    /// in the <see cref="OptionList">OptionList</see>.</remarks>
    /// <value>
    /// If <c>true</c>, the parameter's value will be validated against the Strings
    /// in the <see cref="OptionList">OptionList</see>.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Category("ICapeOptionParameterSpec")]
    [Description("Limits values of the parameter to the values in the option list if true.")]
    public bool RestrictedToList
    {
        get
        {
            return ((ICapeOptionParameterSpecCOM)m_parameter.Specification).RestrictedToList;
        }
        set
        {
        }
    }

    /// <summary>Validates the value against the parameter's specification.</summary>
    /// <remarks>If the value of the <see cref="RestrictedToList">RestrictedToList</see>
    /// public is set to <c>true</c>, the value to be tested is a valid value for the 
    /// parameter if it is included in the 
    /// <see cref="OptionList">OptionList</see>. If the 
    /// value of <see cref="RestrictedToList">RestrictedToList</see> public is <c>false</c>
    /// any valid String is a valid value for the parameter.</remarks>
    /// <returns>True if the string argument is valid, false if it is not.</returns>
    /// <param name="value">The string to be tested for validity.</param>
    /// <param name="message">Reference to a string that will conain a message regarding the validation of the parameter.</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如，未识别的复合标识符或 props 参数的 UNDEFINED。</exception>
    [Description("Checks whether the value is an accepatble value for the parameter.")]
    public bool Validate(String value, ref String message)
    {
        return ((ICapeOptionParameterSpecCOM)m_parameter.Specification).Validate(value, ref message);
    }
}