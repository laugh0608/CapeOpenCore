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
        
    /// <summary>创建一个与当前实例相同的副本对象。</summary>
    /// <remarks><para>克隆可以实现为深度复制或浅度复制。在深度复制中，所有对象都会被复制；
    /// 在浅度复制中，仅复制顶级对象，而较低级别的对象仅包含引用。</para>
    /// <para>生成的克隆必须与原始实例属于同一类型或与之兼容。</para>
    /// <para>有关克隆、深拷贝与浅拷贝的详细信息及示例，请参阅 <see cref="Object.MemberwiseClone"/>。</para></remarks>
    /// <returns>一个与该实例相同的副本对象。</returns>
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

    /// <summary>当用户更改组件的名称时发生。</summary>
    /// <remarks>当 PMC 名称发生变更时触发的事件。</remarks> 
    public event ComponentNameChangedHandler ComponentNameChanged;

    /// <summary>当用户修改组件的描述时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
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

    /// <summary>当用户修改组件的描述时发生。</summary>
    /// <remarks>当 PMC 的描述发生更改时触发的事件。</remarks> 
    public event ComponentDescriptionChangedHandler ComponentDescriptionChanged;

    /// <summary>当用户修改组件的描述时发生。</summary>
    /// <remarks><para>触发事件时，会通过委托调用事件处理程序。</para>
    /// <para><c>OnComponentDescriptionChanged</c> 方法还允许派生类在不附加委托的情况下处理该事件。
    /// 这是在派生类中处理该事件的首选方法。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>在派生类中重写 <c>OnComponentDescriptionChanged</c> 方法时，请务必调用基类的
    /// <c>OnComponentDescriptionChanged</c> 方法，以确保已注册的委托能够收到该事件。</para></remarks>
    /// <param name="args">一个包含事件相关信息的 <see cref="ComponentDescriptionChangedEventArgs"/>。</param>
    protected void OnComponentDescriptionChanged(ComponentDescriptionChangedEventArgs args)
    {
        ComponentDescriptionChanged?.Invoke(this, args);
    }

    /// <summary>获取并设置组件的名称。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的名称和描述，
    /// 以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并不总是由同一供应商开发，
    /// 因此需要制定一个CAPE-OPEN标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的唯一名称。</value>
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

    /// <summary>获取并设置组件的描述。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的名称和描述，
    /// 以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并不总是由同一供应商开发，
    /// 因此需要制定一个CAPE-OPEN标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的描述。</value>
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
        // 创建一个新的集合对象 PropertyDescriptorCollection
        var pds = new PropertyDescriptorCollection(null);
        // 遍历参数列表
        for (var i = 0; i < Items.Count; i++)
        {
            // 对于每个参数，创建一个属性描述符并将其添加到 PropertyDescriptorCollection 实例中。
            var pd = new ParameterCollectionPropertyDescriptor(this, i);
            pds.Add(pd);
        }
        return pds;
    }
}

/// <summary>CollectionPublicDescriptor 的摘要描述。</summary>
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