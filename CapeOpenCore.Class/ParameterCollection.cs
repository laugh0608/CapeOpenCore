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

    //These are the ICapeCollection member implementations
    /// <summary>Gets the number of items currently stored in the collection.</summary>
    /// <remarks>Gets the number of items currently stored in the collection.</remarks>
    /// <returns>Gets the number of items currently stored in the collection.</returns>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    int ICapeCollection.Count()
    {
        return Items.Count;
    }

    /// <summary>Gets the specific item stored within the collection, identified by its 
    /// ICapeIdentification.ComponentName or 1-based index passed as an argument 
    /// to the method.</summary>
    /// <remarks>Return an element from the collection. The requested element can be 
    /// identified by its actual name (e.g. type CapeString) or by its position 
    /// in the collection (e.g. type CapeLong). The name of an element is the 
    /// value returned by the ComponentName() method of its ICapeIdentification 
    /// interface. The advantage of retrieving an item by name rather than by 
    /// position is that it is much more efficient. This is because it is faster 
    /// to check all names from the server part than checking then from the 
    /// client, where a lot of COM/CORBA calls would be required.</remarks>
    /// <param name = "index">
    /// <para>Identifier for the requested item:</para>
    /// <para>name of item (the variant contains a string)</para>
    /// <para>position in collection (it contains a long)</para>
    /// </param>
    /// <returns>
    /// System.Object containing the requested collection item.</returns>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref="ECapeFailedInitialisation">ECapeFailedInitialisation</exception>
    /// <exception cref="ECapeOutOfBounds">ECapeOutOfBounds</exception>
    Object ICapeCollection.Item(Object index)
    {
        Type indexType = index.GetType();
        if ((indexType == typeof(Int16)) || (indexType == typeof(Int32)) || (indexType == typeof(Int64)))
        {
            int i = Convert.ToInt32(index);
            return Items[i - 1];
        }
        if ((indexType == typeof(String)))
        {
            String name = index.ToString();
            for (int i = 0; i < Items.Count; i++)
            {
                ICapeIdentification p_Id = (ICapeIdentification)(Items[i]);
                if (p_Id.ComponentName == name)
                {
                    return Items[i];
                }
            }
        }
        throw new CapeInvalidArgumentException("Item " + index + " not found.", 0);
    }

    /// <summary>Initailizes a new instance of the <see cref="ParameterCollection"/> collection class.</summary>
    /// <remarks>This will create a new instance of the collection.</remarks>
    public ParameterCollection()
    {
    }

    /// <summary>Finalizer for the <see cref="ParameterCollection"/> collection class.</summary>
    /// <remarks>This will finalize the current instance of the collection.</remarks>
    ~ParameterCollection()
    {
        foreach (CapeParameter item in Items)
        {
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
        ParameterCollection clone = new ParameterCollection();
        foreach (ICloneable item in Items)
        {
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
    /// <param name = "args">A <see cref="ComponentNameChangedEventArgs">NameChangedEventArgs</see> that contains information about the event.</param>
    protected void OnComponentNameChanged(ComponentNameChangedEventArgs args)
    {
        if (ComponentNameChanged != null)
        {
            ComponentNameChanged(this, args);
        }
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
    /// <param name = "args">A <see cref="ComponentDescriptionChangedEventArgs">DescriptionChangedEventArgs</see> that contains information about the event.</param>
    protected void OnComponentDescriptionChanged(ComponentDescriptionChangedEventArgs args)
    {
        if (ComponentDescriptionChanged != null)
        {
            ComponentDescriptionChanged(this, args);
        }
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
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    [DescriptionAttribute("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [CategoryAttribute("CapeIdentification")]
    public String ComponentName
    {
        get
        {
            return _mComponentName;
        }

        set
        {
            ComponentNameChangedEventArgs args = new ComponentNameChangedEventArgs(_mComponentName, value);
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
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), specified for this operation, are not suitable.</exception>
    [DescriptionAttribute("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [CategoryAttribute("CapeIdentification")]
    public String ComponentDescription
    {
        get
        {
            return _mComponentDescription;
        }
        set
        {
            ComponentDescriptionChangedEventArgs args = new ComponentDescriptionChangedEventArgs(_mComponentDescription, value);
            _mComponentDescription = value;
            OnComponentDescriptionChanged(args);
        }
    }

    // Implementation of ICustomTypeDescriptor: 

    String ICustomTypeDescriptor.GetClassName()
    {
        return TypeDescriptor.GetClassName(this, true);
    }

    AttributeCollection ICustomTypeDescriptor.GetAttributes()
    {
        return TypeDescriptor.GetAttributes(this, true);
    }

    String ICustomTypeDescriptor.GetComponentName()
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

    Object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
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

    Object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
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
        PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
        // Iterate the list of parameters
        for (int i = 0; i < Items.Count; i++)
        {
            // For each parameter create a property descriptor 
            // and add it to the PropertyDescriptorCollection instance
            ParameterCollectionPropertyDescriptor pd = new ParameterCollectionPropertyDescriptor(this, i);
            pds.Add(pd);
        }
        return pds;
    }
}

//class ParameterCollectionEditor : System.ComponentModel.Design.CollectionEditor
//{
//    private Type[] m_types;

//    public ParameterCollectionEditor(Type t)
//        : base(t)
//    {
//        m_types = new Type[4];
//        m_types[0] = typeof(CapeOpen.BooleanParameter);
//        m_types[1] = typeof(CapeOpen.IntegerParameter);
//        m_types[2] = typeof(CapeOpen.OptionParameter);
//        m_types[3] = typeof(CapeOpen.RealParameter);
//    }

//    protected override Type[] CreateNewItemTypes()
//    {
//        return m_types;
//    }
//};

/// <summary>
/// Summary description for CollectionpublicDescriptor.
/// </summary>
[System.Runtime.InteropServices.ComVisibleAttribute(false)]
class ParameterCollectionPropertyDescriptor : System.ComponentModel.PropertyDescriptor
{
    private ParameterCollection collection;
    private int index;

    public ParameterCollectionPropertyDescriptor(ParameterCollection coll, int idx) :
        base("#" + idx.ToString(), null)
    {
        this.collection = coll;
        this.index = idx;
    }

    public override System.ComponentModel.AttributeCollection Attributes
    {
        get
        {
            return new System.ComponentModel.AttributeCollection(null);
        }
    }

    public override bool CanResetValue(Object component)
    {
        return true;
    }

    public override Type ComponentType
    {
        get
        {
            return this.collection.GetType();
        }
    }

    public override String DisplayName
    {
        get
        {
            return ((ICapeIdentification)this.collection[index]).ComponentName;
        }
    }

    public override String Description
    {
        get
        {
            return ((ICapeIdentification)this.collection[index]).ComponentDescription;
        }
    }

    public override Object GetValue(Object component)
    {
        return this.collection[index];
    }

    public override bool IsReadOnly
    {
        get
        {
            return true;
        }
    }

    public override String Name
    {
        get
        {
            return String.Concat("#", index.ToString());
        }
    }

    public override Type PropertyType
    {
        get
        {
            return this.collection[index].GetType();
        }
    }

    public override void ResetValue(Object component)
    {

    }

    public override bool ShouldSerializeValue(Object component)
    {
        return true;
    }

    public override void SetValue(Object component, Object value)
    {
        //this.collection[index] = value;
    }
};