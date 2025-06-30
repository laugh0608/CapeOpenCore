/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.29
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>用于基于 COM 的C APE-OPEN ICapeThermoMaterialObject 物流对象的封装类。</summary>
/// <remarks><para>本类是基于 COM 的 CAPE-OPEN ICapeThermoMaterialObject 物流对象的封装类。
/// 当您使用此封装类时，基于 COM 的物流对象的生命周期将由系统自动管理，并且会调用物流对象的 COM Release() 方法。</para>
/// <para>此外，该方法将 <see cref="ICapeThermoMaterialObject"/> 接口中使用的 COM 变体转换为
/// 所需的 .Net 对象类型。这消除了在使用基于 COM 的 CAPE-OPEN 物流对象时需要转换数据类型的必要性。</para></remarks>
[ComVisible(false)]
[Guid("5A65B4B2-2FDD-4208-813D-7CC527FB91BD")]
[Description("ICapeThermoMaterialObject Interface")]
internal class MaterialObjectWrapper1 : CapeObjectBase, ICapeThermoMaterialObject
{
    [NonSerialized] private ICapeThermoMaterialObjectCOM _pMaterialObject;

    // 跟踪是否已调用 Dispose 方法。
    private bool _disposed;

    /// <summary>创建 MaterialObjectWrapper 类的实例</summary>
    /// <param name="materialObject">待封装的物流对象。</param>
    public MaterialObjectWrapper1(object materialObject)
    {
        _disposed = false;

        if (materialObject is ICapeThermoMaterialObjectCOM objectCom)
        {
            _pMaterialObject = objectCom;
        }
    }

    // 使用 C# 析构函数语法来实现最终化代码。
    /// <summary><see cref="MaterialObjectWrapper"/> 类的终结器。</summary>
    /// <remarks>这将最终确定当前类的实例。</remarks>
    ~MaterialObjectWrapper1()
    {
        // 只需调用 Dispose(false)。
        Dispose(false);
    }

    /// <summary>释放 CapeIdentification 对象使用的未托管资源，并可选地释放托管资源。</summary>
    /// <remarks><para>这种方法由公共方法 <see href="https://msdn.microsoft.com">Dispose</see>
    /// 和 <see href="https://msDN.microsoft.com">Finalize</see> 调用。Dispose() 方法会调用受保护的
    /// Dispose(Boolean) 方法，并将 disposing 参数设置为 true。
    /// <see href="https://msdn.microsoft.com">Finalize</see> 方法会调用 Dispose 方法，
    /// 并将 disposing 设置为 false。</para>
    /// <para>当处置参数为 true 时，此方法将释放由该组件引用的任何托管对象所占用的所有资源。
    /// 此方法将调用每个引用对象的 Dispose() 方法。</para>
    /// <para>继承该方法开发者须知：</para>
    /// <para>Dispose 方法可以被其他对象多次调用。在重写 Dispose(Boolean) 方法时，请注意不要引用在之前
    /// 调用 Dispose 方法时已经被释放的对象。有关如何实现 Dispose(Boolean) 方法的更多信息，
    /// 请参阅 <see href="https://msdn.microsoft.com">实现 Dispose 方法</see>。</para>
    /// <para>有关 Dispose 和 <see href="https://msdn.microsoft.com">Finalize</see> 的更多信息，
    /// 请参阅 <see href="https://msdn.microsoft.com">清理未托管资源</see> 和
    /// <see href="https://msdn.microsoft.com">重写 Finalize 方法</see>。</para></remarks> 
    /// <param name="disposing">true 表示释放受管和不受管资源；false 表示仅释放不受管资源。</param>
    protected override void Dispose(bool disposing)
    {
        // 如果您需要线程安全，请在这些操作周围使用锁，以及在使用该资源的方法中也使用锁。
        if (_disposed) return;
        if (disposing)
        {
        }

        // 指示该实例已被释放。
        if (_pMaterialObject != null)
            if (_pMaterialObject.GetType().IsCOMObject)
            {
                Marshal.FinalReleaseComObject(_pMaterialObject);
            }

        _pMaterialObject = null;
        _disposed = true;
    }

    /// <summary>获取并设置组件的名称。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的 CAPE-OPEN 组件。用户应能够为每个实例分配
    /// 不同的名称和描述，以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的
    /// 软件组件并不总是由同一供应商开发，因此需要制定一个 CAPE-OPEN 标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的唯一名称。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public override string ComponentName
    {
        get => ((ICapeIdentification)_pMaterialObject).ComponentName;
        set => ((ICapeIdentification)_pMaterialObject).ComponentName = value;
    }

    /// <summary>获取并设置组件的描述。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的CAPE-OPEN组件。用户应能够为每个实例分配不同的
    /// 名称和描述，以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并
    /// 不总是由同一供应商开发，因此需要制定一个CAPE-OPEN标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的描述。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public override string ComponentDescription
    {
        get => ((ICapeIdentification)_pMaterialObject).ComponentDescription;
        set => ((ICapeIdentification)_pMaterialObject).ComponentDescription = value;
    }

    /// <summary>提供有关对象是否支持热力学版本 1.0 的信息。</summary>
    /// <remarks><see cref="MaterialObjectWrapper1"/> 类用于检查所封装的物流对象是否支持
    /// CAPE-OPEN 1.0 版本的热力学。此属性表示该检查的结果。</remarks>
    /// <value>指示封装的物流对象是否支持CAPE-OPEN热力学版本1.0接口。</value>
    public bool SupportsThermo10 => true;

    /// <summary>提供有关对象是否支持热力学版本 1.1 的信息。</summary>
    /// <remarks><see cref="MaterialObjectWrapper1"/> 类用于检查所包裹的物流对象是否支持
    /// CAPE-OPEN 1.1 版本的热力学。此属性表示该检查的结果。</remarks>
    /// <value>指示包裹的物流对象是否支持 CAPE-OPEN 热力学版本 1.1 接口。</value>
    public bool SupportsThermo11 => false;

    /// <summary>获取封装的热力学版本 1.0 物流对象。</summary>
    /// <remarks><para>提供对热力学版本 1.0 物流对象的直接访问。</para>
    /// <para>该物流对象实现了 ICapeThermoMaterialObject 接口的 COM 版本。</para></remarks>
    /// <value>封装的热力学版本 1.0 物流对象。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public object MaterialObject10 => _pMaterialObject;
    
    /// <summary>获取封装的热力学版本 1.1 物流对象。</summary>
    /// <remarks><para>提供对热力学版本 1.1 物流对象的直接访问。</para>
    /// <para>该物流对象暴露了热力学 1.1 接口的 COM 版本。</para></remarks>
    /// <value>封装了热力学 1.1 的物流对象</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public object MaterialObject11 => null;

    /// <summary>获取此 MO 的组件 ID。</summary>
    /// <remarks>返回指定物流对象的组件 ID 列表。</remarks>
    /// <value>物流对象中化合物的名称。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    string[] ICapeThermoMaterialObject.ComponentIds
    {
        get
        {
            try
            {
                return (string[])_pMaterialObject.ComponentIds;
            }
            catch (Exception pEx)
            {
                throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
            }
        }
    }

    /// <summary>获取此 MO 的相态 ID</summary>
    /// <remarks>它返回该时刻 MO 中存在的相位。该方法无法返回整体相位和多相位标识符。
    /// 有关相位存在性的更多信息，请参阅相关说明。</remarks>
    /// <value>物流对象中存在的相。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    string[] ICapeThermoMaterialObject.PhaseIds
    {
        get
        {
            try
            {
                return (string[])_pMaterialObject.PhaseIds;
            }
            catch (Exception pEx)
            {
                throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
            }
        }
    }

    /// <summary>获取一些普适常数。</summary>
    /// <remarks>从属性包中获取通用常量。</remarks>
    /// <returns>请求的普适常数值。</returns>
    /// <param name="props">需检索的普适常数列表。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    double[] ICapeThermoMaterialObject.GetUniversalConstant(string[] props)
    {
        try
        {
            return (double[])_pMaterialObject.GetUniversalConstant(props);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>获取一些纯组分的常数值。</summary>
    /// <remarks>从属性包中获取组件常量。请参阅备注以获取更多信息。</remarks>
    /// <returns>组件常量值，由属性包返回给物流对象中的所有组件这是一个包含一维对象数组的对象。如果我们将请求的
    /// 属性数量表示为 P，请求的组件数量表示为 C，那么该数组将包含 C*P 个对象。前 C 个对象（从位置 0 到 C-1）
    /// 将是第一个请求属性的值（每个组件对应一个对象）。随后（从位置 C 到 2*C-1）将包含第二个请求属性的常量值，依此类推。</returns>
    /// <param name="props">组件常量列表。</param>
    /// <param name="compIds">需要检索常量的组件 ID 列表。对于物流对象中的所有组件，请使用空值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    object[] ICapeThermoMaterialObject.GetComponentConstant(string[] props, string[] compIds)
    {
        try
        {
            return (object[])_pMaterialObject.GetComponentConstant(props, compIds);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>计算一些属性。</summary>
    /// <remarks>该方法负责执行所有属性计算，并将这些计算委托给关联的热力系统。该方法在 CAPE-OPEN 调用模式和用户指南
    /// 章节的描述中进一步定义。请参阅注释以获取有关参数和CalcProp描述的更详细说明，以及对该方法的一般讨论。</remarks>
    /// <param name="props">待计算的属性列表。</param>
    /// <param name="phases">需要计算属性的相态列表。</param>
    /// <param name="calcType">计算类型：混合性质或纯成分性质。对于部分属性，例如混合物中组分的逸度系数，
    /// 使用“混合物” CalcType。对于纯组件逸度系数，使用“pure” CalcType。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref="ECapeOutOfBounds">ECapeOutOfBounds</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeThermoMaterialObject.CalcProp(string[] props, string[] phases, string calcType)
    {
        try
        {
            _pMaterialObject.CalcProp(props, phases, calcType);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>获取一些纯组分的常数值。</summary>
    /// <remarks>这个方法负责从物流对象的计算中检索结果。有关参数的更详细解释，请参见注释。</remarks>
    /// <returns>结果向量，包含按定义的限定符排列的 SI 单位的属性值。该数组是一维的，包含属性，
    /// 按照每个化合物的 props 数组的顺序，按照 compIds 数组的顺序。</returns>
    /// <param name="property">从物流对象请求其结果的属性。</param>
    /// <param name="phase">结果的限定相态。</param>
    /// <param name="compIds">结果的合格组件。使用空值指定材质对象中的所有组件。
    /// 对于混合性质，如液体焓，这个限定符是不需要的。使用 emptyObject 作为占位符。</param>
    /// <param name="calcType">结果的限定计算类型。（有效计算类型：纯和混合）。</param>
    /// <param name="basis">限定结果的基础（即质量/摩尔）。默认值是摩尔。
    /// 使用 NULL 作为默认值，或者作为不适用基的属性的占位符（参见 Specific properties）。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    double[] ICapeThermoMaterialObject.GetProp(string property,
        string phase, string[] compIds, string calcType, string basis)
    {
        try
        {
            return (double[])_pMaterialObject.GetProp(property, phase, compIds, calcType, basis);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>获取一些纯组分的常数值。</summary>
    /// <remarks>此方法负责设置材质对象的属性值。有关参数的更详细解释，请参见注释。</remarks>
    /// <param name="property">从物流对象请求其结果的属性。</param>
    /// <param name="phase">结果的限定相态。</param>
    /// <param name="compIds">结果的合格组件。指定材质对象中的所有组件。对于混合性质，
    /// 如液体焓，这个限定符是不需要的。使用 emptyObject 作为占位符。</param>
    /// <param name="calcType">结果的限定计算类型。（有效计算类型：纯和混合）。</param>
    /// <param name="basis">限定结果的基础（即质量/摩尔）。默认值是摩尔。使用 NULL 作为默认值，
    /// 或者作为不适用基的属性的占位符（参见 Specific properties）。</param>
    /// <param name="values">要为属性设置的值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    void ICapeThermoMaterialObject.SetProp(string property,
        string phase, string[] compIds, string calcType, string basis, double[] values)
    {
        try
        {
            _pMaterialObject.SetProp(property, phase, compIds, calcType, basis, values);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>计算一些平衡值。</summary>
    /// <remarks>此方法负责将闪蒸计算委托给相关的属性包或均衡服务器。如果没有作为计算规范的一部分设置，
    /// 则必须设置平衡状态下所有相的数量、组成、温度和压力，以及整个混合物的温度和压力。
    /// 参见 CalcProp 和 CalcEquilibrium 了解更多信息。</remarks>
    /// <param name="flashType">要计算的闪蒸类型。</param>
    /// <param name="props">要在平衡状态下计算的性质。没有属性的空对象。
    /// 如果是一个列表，那么应该为处于平衡状态的每个相设置属性值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <exception cref="ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref="ECapeOutOfBounds">ECapeOutOfBounds</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeThermoMaterialObject.CalcEquilibrium(string flashType, string[] props)
    {
        try
        {
            _pMaterialObject.CalcEquilibrium(flashType, props);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>设置状态的自变量。</summary>
    /// <remarks>为给定的物流对象设置自变量。</remarks>
    /// <param name="indVars">要设置的独立变量（请参阅状态变量的名称以获取有效变量列表）。
    /// 一个系统。对象，该对象包含从 COM 对象封送的字符串数组。</param>
    /// <param name="values">自变量的值。一个双精度数组作为 System.Object 对象，该对象通过 COM 基于的 CAPE-OPEN 进行序列化。</param>
    void ICapeThermoMaterialObject.SetIndependentVar(string[] indVars, double[] values)
    {
        try
        {
            _pMaterialObject.SetIndependentVar(indVars, values);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>Get the independent variable for the state</summary>
    /// <remarks>Sets the independent variable for a given Material Object.</remarks>
    /// <param name="indVars">
    /// Independent variables to be set (see names for state variables for list of 
    /// valid variables).</param>
    /// <returns>Values of independent variables.
    /// COM-based CAPE-OPEN. </returns>
    double[] ICapeThermoMaterialObject.GetIndependentVar(string[] indVars)
    {
        try
        {
            return (double[])_pMaterialObject.GetIndependentVar(indVars);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>Check a property is valid</summary>
    /// <remarks>Checks to see if given properties can be calculated.</remarks>
    /// <returns>Returns Boolean List associated to list of properties to be checked.</returns>
    /// <param name="props">Properties to check. </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    bool[] ICapeThermoMaterialObject.PropCheck(string[] props)
    {
        try
        {
            return (bool[])_pMaterialObject.PropCheck(props);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Check a property is valid
    ///// </summary>
    ///// <remarks>
    ///// Checks to see if given properties can be calculated.
    ///// </remarks>
    ///// <returns>
    ///// Returns Boolean List associated to list of properties to be checked.
    ///// </returns>
    ///// <param name="props">
    ///// Properties to check. 
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    //object ICapeThermoMaterialObjectCOM.PropCheck(object props)
    //{
    //    try
    //    {
    //        return p_MaterialObject.PropCheck(props);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}
    /// <summary>Check which properties are available</summary>
    /// <remarks>Gets a list properties that have been calculated.</remarks>
    /// <returns>Properties for which results are available.</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    string[] ICapeThermoMaterialObject.AvailableProps()
    {
        try
        {
            return (string[])_pMaterialObject.AvailableProps();
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Check which properties are available
    ///// </summary>
    ///// <remarks>
    ///// Gets a list properties that have been calculated.
    ///// </remarks>
    ///// <returns>
    ///// Properties for which results are available.
    ///// </returns>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    //object ICapeThermoMaterialObjectCOM.AvailableProps()
    //{
    //    try
    //    {
    //        return p_MaterialObject.AvailableProps();
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}
    ///// <summary>
    ///// Remove any previously calculated results for given properties
    ///// </summary>
    ///// <remarks>
    ///// Remove all or specified property results in the Material Object.
    ///// </remarks>
    ///// <param name="props">
    ///// Properties to be removed. emptyObject to remove all properties.
    ///// </param>
    //void ICapeThermoMaterialObjectCOM.RemoveResults(Object props)
    //{
    //    try
    //    {
    //        p_MaterialObject.RemoveResults(props);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Remove any previously calculated results for given properties</summary>
    /// <remarks>Remove all or specified property results in the Material Object.</remarks>
    /// <param name="props">Properties to be removed. emptyObject to remove all properties.</param>
    void ICapeThermoMaterialObject.RemoveResults(string[] props)
    {
        try
        {
            _pMaterialObject.RemoveResults(props);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }


    /// <summary>Create another empty material object</summary>
    /// <remarks>Create a Material Object from the parent Material Template of the current 
    /// Material Object. This is the same as using the CreateMaterialObject method 
    /// on the parent Material Template.</remarks> 
    /// <returns>The created/initialized Material Object.</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    ICapeThermoMaterialObject ICapeThermoMaterialObject.CreateMaterialObject()
    {
        try
        {
            return (ICapeThermoMaterialObject)_pMaterialObject.CreateMaterialObject();
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Create another empty material object
    ///// </summary>
    ///// <remarks>
    ///// Create a Material Object from the parent Material Template of the current 
    ///// Material Object. This is the same as using the CreateMaterialObject method 
    ///// on the parent Material Template.
    ///// </remarks> 
    ///// <returns>
    ///// The created/initialized Material Object.
    ///// </returns>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    ///// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    //Object ICapeThermoMaterialObjectCOM.CreateMaterialObject()
    //{
    //    try
    //    {
    //        return (ICapeThermoMaterialObject)p_MaterialObject.CreateMaterialObject();
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Duplicate this material object</summary>
    /// <remarks>Create a duplicate of the current Material Object.</remarks>
    /// <returns>The created/initialized Material Object.</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    [DispId(15)]
    [Description("method Duplicate")]
    [return: MarshalAs(UnmanagedType.IDispatch)]
    ICapeThermoMaterialObject ICapeThermoMaterialObject.Duplicate()
    {
        try
        {
            return new MaterialObjectWrapper(_pMaterialObject.Duplicate());
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }


    ///// <summary>
    ///// Duplicate this material object
    ///// </summary>
    ///// <remarks>
    ///// Create a duplicate of the current Material Object.
    ///// </remarks>
    ///// <returns>
    ///// The created/initialized Material Object.
    ///// </returns>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    ///// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    //[System.Runtime.InteropServices.DispIdAttribute(15)]
    //[System.ComponentModel.DescriptionAttribute("method Duplicate")]
    //[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.IDispatch)]
    //object ICapeThermoMaterialObjectCOM.Duplicate()
    //{
    //    try
    //    {
    //        return new MaterialObjectWrapper(p_MaterialObject.Duplicate());
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    ///// <summary>
    ///// Check the validity of the given properties
    ///// </summary>
    ///// <remarks>
    ///// Checks the validity of the calculation.
    ///// </remarks>
    ///// <returns>
    ///// Returns the reliability scale of the calculation.
    ///// </returns>
    ///// <param name="props">
    ///// The properties for which reliability is checked. Null value to remove all 
    ///// properties. 
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    //object ICapeThermoMaterialObjectCOM.ValidityCheck(object props)
    //{
    //    try
    //    {
    //        return (bool[])p_MaterialObject.ValidityCheck(props);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Check the validity of the given properties</summary>
    /// <remarks>Checks the validity of the calculation.</remarks>
    /// <returns>Returns the reliability scale of the calculation.</returns>
    /// <param name="props">The properties for which reliability is checked. Null value to remove all 
    /// properties. </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为 UNDEFINED。</exception>
    ICapeThermoReliability[] ICapeThermoMaterialObject.ValidityCheck(string[] props)
    {
        try
        {
            return (ICapeThermoReliability[])_pMaterialObject.ValidityCheck(props);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    /// <summary>Get the list of properties</summary>
    /// <remarks>Returns list of properties supported by the property package and corresponding 
    /// CO Calculation Routines. The properties TEMPERATURE, PRESSURE, FRACTION, FLOW, 
    /// PHASEFRACTION, TOTALFLOW cannot be returned by GetPropList, since all 
    /// components must support them. Although the property identifier of derivative 
    /// properties is formed from the identifier of another property, the GetPropList 
    /// method will return the identifiers of all supported derivative and 
    /// non-derivative properties. For instance, a Property Package could return 
    /// the following list: enthalpy, enthalpy.Dtemperature, entropy, entropy.Dpressure.</remarks>
    /// <returns>String list of all supported properties of the property package.</returns>
    string[] ICapeThermoMaterialObject.GetPropList()
    {
        try
        {
            return (string[])_pMaterialObject.GetPropList();
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Get the list of properties
    ///// </summary>
    ///// <remarks>
    ///// Returns list of properties supported by the property package and corresponding 
    ///// CO Calculation Routines. The properties TEMPERATURE, PRESSURE, FRACTION, FLOW, 
    ///// PHASEFRACTION, TOTALFLOW cannot be returned by GetPropList, since all 
    ///// components must support them. Although the property identifier of derivative 
    ///// properties is formed from the identifier of another property, the GetPropList 
    ///// method will return the identifiers of all supported derivative and 
    ///// non-derivative properties. For instance, a Property Package could return 
    ///// the following list: enthalpy, enthalpy.Dtemperature, entropy, entropy.Dpressure.
    ///// </remarks>
    ///// <returns>
    ///// String list of all supported properties of the property package.
    ///// </returns>
    //object ICapeThermoMaterialObjectCOM.GetPropList()
    //{
    //    try
    //    {
    //        return p_MaterialObject.GetPropList();
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Get the number of components in this material object</summary>
    /// <remarks>Returns number of components in Material Object.</remarks>
    /// <returns>Number of components in the Material Object.</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    int ICapeThermoMaterialObject.GetNumComponents()
    {
        try
        {
            return _pMaterialObject.GetNumComponents();
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }
}