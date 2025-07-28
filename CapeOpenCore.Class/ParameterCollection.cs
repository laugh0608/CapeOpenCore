/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.07.21
 */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

[ComVisible(false)]
internal class ParameterCollectionTypeConverter : ExpandableObjectConverter
{
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return typeof(ParameterCollection).IsAssignableFrom(destinationType) 
               || base.CanConvertTo(context, destinationType);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, 
        object value, Type destinationType)
    {
        if (typeof(string).IsAssignableFrom(destinationType) 
            && typeof(ICapeIdentification).IsAssignableFrom(value.GetType()))
        {
            return "Parameter Collection";
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

/// <summary>CapeParameter 对象的类型安全集合。</summary>
/// <remarks><para>该集合使用 BindingList 泛型集合来创建一个只包含实现 <see cref="ICapeParameter"/> 接口
/// 的对象的集合。该类还实现了 ICustomTypeDescriptor，以提供有关该集合的动态信息。</para>
/// <para>由于该类使用了通用集合类，因此基于 dotNet 的对象可以通过使用对象的索引直接获取参数对象。
/// dotNet集合的索引为 0，即第一个参数的索引为 0，第n个参数的索引为 n-1。</para>
/// <para>此外，还可以通过 <see cref="ICapeCollection"/> 接口从 COM 访问该集合。ICapeCollection 成员是
/// 明确实现的，因此只有 COM 可以通过 ICapeCollection 接口访问这些成员。</para></remarks>
[Serializable]
[ComVisible(true)]
[ComSourceInterfaces(typeof(ICapeIdentificationEvents), typeof(IBindingList))] //typeof(ICapeCollectionEvents)
[Guid("64A1B36C-106B-4d05-B585-D176CD4DD1DB")] //ICapeThermoMaterialObject_IID)
[Description("")]
//[TypeConverter(typeof(ParameterCollectionTypeConverter))]
[ClassInterface(ClassInterfaceType.None)]
public class ParameterCollection : BindingList<ICapeParameter>, ICapeCollection,
    ICustomTypeDescriptor, ICloneable, ICapeIdentification
{
    private string _mComponentName;
    private string _mComponentDescription;

    // 以下是 ICapeCollection 成员的实现。
    /// <summary>获取集合中当前存储的项目数量。</summary>
    /// <remarks>获取集合中当前存储的项目数量。</remarks>
    /// <returns>获取集合中当前存储的项目数量。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    int ICapeCollection.Count()
    {
        return Items.Count;
    }

    /// <summary>获取集合中存储的特定项，该项通过其 ICapeIdentification.ComponentName 或
    /// 作为方法参数传递的基于 1 的索引进行标识。</summary>
    /// <remarks>从集合中返回一个元素。请求的元素可以通过其实际名称（例如类型为CapeString）或在集合中的位置
    /// （例如类型为CapeLong）来识别。元素的名称是其 ICapeIdentification 接口的 ComponentName() 方法返回的值。
    /// 与通过位置检索项相比，通过名称检索项的优势在于效率更高。这是因为从服务器端检查所有名称比从客户端检查更快速，
    /// 因为后者需要大量 COM/CORBA 调用。</remarks>
    /// <param name="index"><para>请求项的标识符：</para>
    /// <para>项目名称（该变体包含一个字符串）</para>
    /// <para>在集合中的位置（它包含一个长度）</para></param>
    /// <returns>包含请求的集合项的 System.Object 对象。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeOutOfBounds">ECapeOutOfBounds</exception>
    object ICapeCollection.Item(object index)
    {
        var indexType = index.GetType();
        if (indexType == typeof(short) || (indexType == typeof(int)) || indexType == typeof(long))
        {
            var i = Convert.ToInt32(index);
            return Items[i - 1];
        }

        if (indexType != typeof(string)) 
            throw new CapeInvalidArgumentException("Item " + index + " not found.", 0);
        {
            var name = index.ToString();
            foreach (var pT in Items)
            {
                var pId = (ICapeIdentification)pT;
                if (pId.ComponentName == name)
                {
                    return pT;
                }
            }
        }
        throw new CapeInvalidArgumentException("Item " + index + " not found.", 0);
    }

    /// <summary>初始化 <see cref="ParameterCollection"/> 集合类的全新实例。</summary>
    /// <remarks>这将创建集合的新实例。</remarks>
    public ParameterCollection()
    {
    }

    /// <summary><see cref="ParameterCollection"/> 集合类的终结器。</summary>
    /// <remarks>这将最终确定当前集合的实例。</remarks>
    ~ParameterCollection()
    {
        // foreach (CapeParameter item in Items)
        // {
        //     item.Dispose();
        // }
        foreach (var capeParameter in Items)
        {
            var item = (CapeParameter)capeParameter;
            item.Dispose();
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
    public object Clone()
    {
        var clone = new ParameterCollection();
        // foreach (ICloneable item in Items)
        // {
        //     clone.Add((CapeParameter)item.Clone());
        // }
        foreach (var capeParameter in Items)
        {
            var item = (ICloneable)capeParameter;
            clone.Add((CapeParameter)item.Clone());
        }
        return clone;
    }

    /// <summary>Occurs when the user changes of the name of a component.</summary>
    /// <remarks>The event to be handles when the name of the PMC is changed.</remarks> 
    public event ComponentNameChangedHandler ComponentNameChanged;

    /// <summary>Occurs when the user changes of the description of a component.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnComponentNameChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnComponentNameChanged</c> in a derived class, be sure to call the base class's <c>OnComponentNameChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ComponentNameChangedEventArgs">NameChangedEventArgs</see> that contains information about the event.</param>
    protected void OnComponentNameChanged(ComponentNameChangedEventArgs args)
    {
        ComponentNameChanged?.Invoke(this, args);
    }

    /// <summary>Occurs when the user changes of the description of a component.</summary>
    /// <remarks>The event to be handles when the description of the PMC is changed.</remarks> 
    public event ComponentDescriptionChangedHandler ComponentDescriptionChanged;

    /// <summary>Occurs when the user changes of the description of a component.</summary>
    /// <remarks><para>Raising an event invokes the event handler through a delegate.</para>
    /// <para>The <c>OnComponentDescriptionChanged</c> method also allows derived classes to handle the event without attaching a delegate. This is the preferred 
    /// technique for handling the event in a derived class.</para>
    /// <para>Notes to Inheritors: </para>
    /// <para>When overriding <c>OnComponentDescriptionChanged</c> in a derived class, be sure to call the base class's <c>OnComponentDescriptionChanged</c> method so that registered 
    /// delegates receive the event.</para></remarks>
    /// <param name="args">A <see cref="ComponentDescriptionChangedEventArgs">DescriptionChangedEventArgs</see> that contains information about the event.</param>
    protected void OnComponentDescriptionChanged(ComponentDescriptionChangedEventArgs args)
    {
        ComponentDescriptionChanged?.Invoke(this, args);
    }

    /// <summary>Gets and sets the name of the component.</summary>
    /// <remarks><para>A particular Use Case in a system may contain several CAPE-OPEN components 
    /// of the same class. The user should be able to assign different names and 
    /// descriptions to each instance in order to refer to them unambiguously and in a 
    /// user-friendly way. Since not always the software components that are able to 
    /// set these identifications and the software components that require this information 
    /// have been developed by the same vendor, a CAPE-OPEN standard for setting and 
    /// getting this information is required.</para>
    /// <para>So, the component will not usually set its own name and description: the 
    /// user of the component will do it.</para></remarks>
    /// <value>The unique name of the component.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public string ComponentName
    {
        get => _mComponentName;
        set
        {
            var args = new ComponentNameChangedEventArgs(_mComponentName, value);
            _mComponentName = value;
            OnComponentNameChanged(args);
        }
    }

    /// <summary> Gets and sets the description of the component.</summary>
    /// <remarks><para>A particular Use Case in a system may contain several CAPE-OPEN components 
    /// of the same class. The user should be able to assign different names and 
    /// descriptions to each instance in order to refer to them unambiguously and in a 
    /// user-friendly way. Since not always the software components that are able to 
    /// set these identifications and the software components that require this information 
    /// have been developed by the same vendor, a CAPE-OPEN standard for setting and 
    /// getting this information is required.</para>
    /// <para>So, the component will not usually set its own name and description: the 
    /// user of the component will do it.</para></remarks>
    /// <value>The description of the component.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public string ComponentDescription
    {
        get => _mComponentDescription;
        set
        {
            var args = new ComponentDescriptionChangedEventArgs(_mComponentDescription, value);
            _mComponentDescription = value;
            OnComponentDescriptionChanged(args);
        }
    }

    // Implementation of ICustomTypeDescriptor: 
    string ICustomTypeDescriptor.GetClassName()
    {
        return TypeDescriptor.GetClassName(this, true);
    }

    AttributeCollection ICustomTypeDescriptor.GetAttributes()
    {
        return TypeDescriptor.GetAttributes(this, true);
    }

    string ICustomTypeDescriptor.GetComponentName()
    {
        return TypeDescriptor.GetComponentName(this, true);
    }

    TypeConverter ICustomTypeDescriptor.GetConverter()
    {
        return TypeDescriptor.GetConverter(this, true);
    }

    EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
    {
        return TypeDescriptor.GetDefaultEvent(this, true);
    }

    PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
    {
        return TypeDescriptor.GetDefaultProperty(this, true);
    }

    object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
    {
        return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
    {
        return TypeDescriptor.GetEvents(this, attributes, true);
    }

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
    {
        return TypeDescriptor.GetEvents(this, true);
    }

    object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
    {
        return this;
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
    {
        return ((ICustomTypeDescriptor)this).GetProperties();
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
    {
        // Create a new collection object PropertyDescriptorCollection
        var pds = new PropertyDescriptorCollection(null);
        // Iterate the list of parameters
        for (var i = 0; i < Items.Count; i++)
        {
            // For each parameter create a property descriptor 
            // and add it to the PropertyDescriptorCollection instance
            var pd = new ParameterCollectionPropertyDescriptor(this, i);
            pds.Add(pd);
        }
        return pds;
    }
}

/// <summary>
/// Summary description for CollectionpublicDescriptor.
/// </summary>
[ComVisible(false)]
internal class ParameterCollectionPropertyDescriptor(ParameterCollection coll, int idx)
    : PropertyDescriptor("#" + idx, null)
{
    public override AttributeCollection Attributes => new(null);

    public override bool CanResetValue(object component)
    {
        return true;
    }

    public override Type ComponentType => coll.GetType();

    public override string DisplayName => ((ICapeIdentification)coll[idx]).ComponentName;

    public override string Description => ((ICapeIdentification)coll[idx]).ComponentDescription;

    public override object GetValue(object component)
    {
        return coll[idx];
    }

    public override bool IsReadOnly => true;

    public override string Name => string.Concat("#", idx.ToString());

    public override Type PropertyType => coll[idx].GetType();

    public override void ResetValue(object component)
    {

    }

    public override bool ShouldSerializeValue(object component)
    {
        return true;
    }

    public override void SetValue(object component, object value)
    {
        // this.collection[index] = value;
    }
}