/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.07
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>用于将基于 .NET 的对象暴露给基于 COM 的程序管理器（PMC）的包装类。</summary>
/// <remarks><para>这个类允许基于 COM 的遗留 PMC 访问基于 .NET 的物流对象。
/// 这个包装器将使用基于 .NET 的热力学接口的物流对象暴露给基于 COM 的 PMC。基于 COM 的 PMC 可以通过 CAPE-OPEN 接口的 COM 版本调用物流对象。</para>
/// <para>这个类由 <see cref="UnitPortWrapper"/> 类用于将端口连接的流股暴露给由 <see cref="UnitOperationWrapper"/> 类包装的基于 COM 的遗留单元操作。
/// 这个包装器处理在热力学接口的 .NET 版本中使用的强类型数组和 COM 版本中使用的 VARIANT 数据类型之间的转换。</para></remarks>
[Serializable, ComVisible(true)]
[Guid("5A65B4B2-2FDD-4208-813D-7CC527FB91BD")]
[Description("ICapeThermoMaterialObject Interface")]
partial class COMMaterialObjectWrapper : CapeObjectBase, ICapeThermoMaterialObjectCOM, ICapeThermoMaterialCOM, ICapeThermoCompoundsCOM, 
    ICapeThermoPhasesCOM, ICapeThermoUniversalConstantCOM, ICapeThermoEquilibriumRoutineCOM, ICapeThermoPropertyRoutineCOM
{
    private ICapeThermoMaterial _pIMatObj;
    private ICapeThermoCompounds _pICompounds;
    private ICapeThermoPhases _pIPhases;
    private ICapeThermoUniversalConstant _pIUniversalConstant;
    private ICapeThermoPropertyRoutine _pIPropertyRoutine;
    private ICapeThermoEquilibriumRoutine _pIEquilibriumRoutine;
        
    // 跟踪是否已调用 Dispose 方法。
    private bool _disposed;

    private bool _thermo10;
    private bool _thermo11;

    /// <summary>创建 MaterialObjectWrapper 类的实例。</summary>
    /// <remarks>此方法由 <see cref="ICapeUnitPortCOM.connectedObject"/> 方法调用，以将基于 .NET 的物流对象暴露给基于 COM 的遗留 PMC。</remarks>
    /// <param name="materialObject">The material Object to be wrapped.</param>
    public COMMaterialObjectWrapper(object materialObject)
    {
        _disposed = false;
        _thermo10 = true;
        _thermo11 = true;
        MaterialObject = (ICapeThermoMaterialObject)materialObject;
        if (MaterialObject == null) _thermo10 = false;
        _pIMatObj = null;
        _pIMatObj = (ICapeThermoMaterial)materialObject;
        if (_pIMatObj == null) _thermo11 = false;
        _pIPropertyRoutine = null;
        _pIPropertyRoutine = (ICapeThermoPropertyRoutine)materialObject;
        if (_pIPropertyRoutine == null) _thermo11 = false;
        _pIUniversalConstant = null;
        _pIUniversalConstant = (ICapeThermoUniversalConstant)materialObject;
        if (_pIUniversalConstant == null) _thermo11 = false;
        _pIPhases = null;
        _pIPhases = (ICapeThermoPhases)materialObject;
        if (_pIPhases == null) _thermo11 = false;
        _pICompounds = null;
        _pICompounds = (ICapeThermoCompounds)materialObject;
        if (_pICompounds == null) _thermo11 = false;
        _pIEquilibriumRoutine = null;
        _pIEquilibriumRoutine = (ICapeThermoEquilibriumRoutine)materialObject;
        if (_pIEquilibriumRoutine == null) _thermo11 = false;
    }

    // 使用 C# 析构函数语法来实现最终化代码。
    /// <summary><see cref="COMMaterialObjectWrapper"/> 类的终结器。</summary>
    /// <remarks>这将最终确定当前类的实例。</remarks>
    ~COMMaterialObjectWrapper()
    {
        // 只需调用 Dispose(false)。
        Dispose(false);
    }

    // Implement IDisposable.
    // Dispose(bool disposing) 在两种不同的情况下执行。如果 disposing 等于 true，则该方法已由用户的代码直接或间接调用。可以处理托管和非托管资源。
    // 如果 disposing 等于 false，则该方法已由运行时从 finalizer 内部调用，您不应引用其他对象。只有非托管资源可以处理。
    /// <summary>释放 CapeIdentification 对象使用的非托管资源，并可选地释放托管资源。</summary>
    /// <remarks><para>此方法由公共的 Dispose 方法以及 Finalize 方法调用。Dispose() 方法会调用受保护的 Dispose(Boolean) 方法，
    /// 并将 disposing 参数设置为 true。Finalize 方法会调用 Dispose，并将 disposing 参数设置为 false。</para>
    /// <para>当 disposing 参数为 true 时，此方法会释放此组件引用的任何托管对象持有的所有资源。此方法会调用每个引用对象的 Dispose() 方法。</para>
    /// <para>留给继承者的注释：Dispose 可以被其他对象多次调用。在重写 Dispose(Boolean) 方法时，要小心不要引用在之前调用 Dispose 时已经处理过的对象。
    /// 有关如何实现 Dispose(Boolean) 的更多信息，请参阅实现一个 Dispose 方法。</para>
    /// <para>有关 Dispose 和 Finalize 的更多信息，请参阅清理未托管资源和重写 Finalize 方法。</para></remarks>
    /// <param name="disposing">true 表示释放受管和不受管资源；false 表示仅释放不受管资源。</param>
    protected override void Dispose(bool disposing)
    {
        // 如果您需要线程安全，请在这些操作周围使用锁，以及在使用该资源的方法中也使用锁。
        if (_disposed) return;
        if (disposing) { }
        // 指示该实例已被释放。
        MaterialObject = null;
        _pICompounds = null;
        _pIEquilibriumRoutine = null;
        _pIMatObj = null;
        _pIPhases = null;
        _pIPropertyRoutine = null;
        _pIUniversalConstant = null;
        _disposed = true;
    }

    /// <summary>获取并设置组件的描述。</summary>
    /// <remarks><para>系统中的一个特定用例可能包含多个相同类的 CAPE-OPEN 组件。用户应该能够为每个实例分配不同的名称和描述，以便以不模糊和用户友好的方式引用它们。
    /// 由于并非总是能够设置这些标识的软件组件和需要此信息的软件组件由同一供应商开发，因此需要设置和获取此信息的 CAPE-OPEN 标准。</para>
    /// <para>因此，组件通常不会设置自己的名称和描述：组件的使用者会这样做。</para></remarks>
    /// <value>组件的描述。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public ICapeThermoMaterialObject MaterialObject { get; private set; }

    /// <summary>获取此MO的组件 ID。</summary>
    /// <remarks>返回指定物流对象的组件ID列表。</remarks>
    /// <value>物流对象中化合物的名称。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    object ICapeThermoMaterialObjectCOM.ComponentIds => _thermo10 ? MaterialObject.ComponentIds : null;

    /// <summary>获取此 MO 的相态 ID。</summary>
    /// <remarks>它返回 MO 中当时存在的相态。整体相态和多重相态标识符不能通过此方法返回。请参阅有关相态存在的注释以获取更多信息。</remarks>
    /// <value>物流对象中存在的相态。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    object ICapeThermoMaterialObjectCOM.PhaseIds => _thermo10 ? MaterialObject.PhaseIds : null;

    /// <summary>获取一些通用常数（气体普适常数）。</summary>
    /// <remarks>从属性包中获取通用常数。</remarks>
    /// <returns>请求的通用常数值。</returns>
    /// <param name="props">需要检索的通用常数列表。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    object ICapeThermoMaterialObjectCOM.GetUniversalConstant(object props)
    {
        try
        {
            var propNames = (string[])props;
            return _thermo10 ? MaterialObject.GetUniversalConstant(propNames) : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>获取一些纯组分的常数值。</summary>
    /// <remarks>从属性包中获取组件常量。请参阅备注以获取更多信息。</remarks>
    /// <returns>组件常量值从属性包返回，用于物流对象中的所有组件。它是一个包含1维对象数组的对象。如果我们调用 P 为请求的属性数，C 为请求的组件数，
    /// 则数组将包含 C*P 个对象。C 的第一个（从位置 0 到 C-1）将是第一个请求的属性值（每个组件一个对象）。
    /// 之后（从位置 C 到 2*C-1）将是第二个请求的属性常量的值，依此类推。</returns>
    /// <param name="props">组件常量列表。</param>
    /// <param name="compIds">要检索常量的组件 ID 列表。对于物流对象中的所有组件，请使用 null 值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    object ICapeThermoMaterialObjectCOM.GetComponentConstant(object props, object compIds)
    {
        var propNames = (string[])props;
        var compIdsNames = (string[])compIds;
        return _thermo10 ? MaterialObject.GetComponentConstant(propNames, compIdsNames) : null;
    }

    /// <summary>计算一些物性。</summary>
    /// <remarks>这个方法负责进行所有属性计算，并将这些计算委托给相关的热系统。这个方法在 CAPE-OPEN 调用模式和用户指南部分的描述中有进一步的定义。
    /// 请参阅注释，以获取有关参数和 CalcProp 描述的更详细解释，以及方法的一般讨论。</remarks>
    /// <param name="props">待计算的物性列表。</param>
    /// <param name="phases">需要计算物性的相态列表。</param>
    /// <param name="calcType">计算类型：混合物性质或纯组分性质。对于部分性质，如混合物中组分的逸度系数，使用“混合物”计算类型。对于纯组分的逸度系数，使用 “纯” 计算类型。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    /// <exception cref="ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref="ECapeOutOfBounds">ECapeOutOfBounds</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeThermoMaterialObjectCOM.CalcProp(object props, object phases, string calcType)
    {
        var propNames = (string[])props;
        var phasesNames = (string[])phases;
        if (_thermo10) MaterialObject.CalcProp(propNames, phasesNames, calcType);
    }

    /// <summary>获取一些纯组分的常数值。</summary>
    /// <remarks>此方法负责从 MaterialObject 中检索计算结果。请参阅注释以获取有关参数的更详细说明。</remarks>
    /// <returns>结果向量包含按定义的限定符排列的 SI 单位属性值。该数组是一维的，包含属性，按“props”数组的顺序排列每个化合物，按 <see cref="compIds"/> 数组的顺序排列。</returns>
    /// <param name="property">从 MaterialObject 请求结果的属性。</param>
    /// <param name="phase">结果的合格相态。</param>
    /// <param name="compIds">结果的合格组分。使用空值来指定物流对象中的所有组分。对于混合属性（如液体焓），此限定符不是必需的。使用 emptyObject 作为占位符。</param>
    /// <param name="calcType">结果的合格计算类型。（有效计算类型：纯组分和混合物）。</param>
    /// <param name="basis">确定结果的基础（即质量/摩尔）。默认为摩尔。使用 NULL 作为不适用基础属性的默认值或占位符（另请参阅特定属性）。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    object ICapeThermoMaterialObjectCOM.GetProp(string property, string phase, object compIds, string calcType, string basis)
    {
        try {
            var compIdsNames = (string[])compIds;
            return _thermo10 ? MaterialObject.GetProp(property, phase, compIdsNames, calcType, basis) : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>获取一些纯组分的常数值。</summary>
    /// <remarks>此方法负责设置物流对象的属性的值。请参阅注释以获取有关参数的更详细说明。</remarks>
    /// <param name="property">从 MaterialObject 请求结果的属性。</param>
    /// <param name="phase">结果的合格相态。</param>
    /// <param name="compIds">结果的合格组件。emptyObject 用于指定流股对象中的所有组分。对于混合属性（如液体焓），此限定符不是必需的。使用 emptyObject 作为占位符。</param>
    /// <param name="calcType">结果的合格计算类型。（有效计算类型：纯净和混合）。</param>
    /// <param name="basis">确定结果的基础（即质量/摩尔）。默认为摩尔。使用NULL作为不适用基础属性的默认值或占位符（另请参阅特定属性）。</param>
    /// <param name="values">为该属性设置的值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    void ICapeThermoMaterialObjectCOM.SetProp(string property, string phase, object compIds, string calcType, string basis, object values)
    {
        try
        {
            var compIdsNames = (string[])compIds;
            var valuesArray = (double[])compIds;
            if (_thermo10) MaterialObject.SetProp(property, phase, compIdsNames, calcType, basis, valuesArray);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>计算一些平衡值。</summary>
    /// <remarks>此方法负责将闪点计算委托给相关的属性包或平衡服务器。它必须设置平衡状态下所有相的数量、组成、温度和压力，
    /// 以及整体混合物的温度和压力（如果未作为计算规范的一部分设置）。请参阅 CalcProp 和 CalcEquilibrium 以获取更多信息。</remarks>
    /// <param name="flashType">要计算的闪蒸类型。</param>
    /// <param name="props">在平衡状态下计算的属性。若无属性，则为空对象。若为列表，则需为平衡状态下存在的每个相设置属性值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    /// <exception cref="ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <exception cref="ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref="ECapeOutOfBounds">ECapeOutOfBounds</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeThermoMaterialObjectCOM.CalcEquilibrium(string flashType, object props)
    {
        try
        {
            var propNames = (string[])props;
            if (_thermo10) MaterialObject.CalcEquilibrium(flashType, propNames);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>设置状态的自变量。</summary>
    /// <remarks>设置给定物流对象的自变量。</remarks>
    /// <param name="indVars">要设置的独立变量（请参阅状态变量的名称以获取有效变量的列表）。包含从 COM 对象序列化的字符串数组的 System.Object。</param>
    /// <param name="values">自变量的值。一个双精度数组作为 System.Object，该对象通过 COM 基于 CAPE-OPEN 进行序列化。</param>
    void ICapeThermoMaterialObjectCOM.SetIndependentVar(object indVars, object values)
    {
        try
        {
            var indVarNames = (string[])indVars;
            var valuesArray = (double[])values;
            if (_thermo10) MaterialObject.SetIndependentVar(indVarNames, valuesArray);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>获取状态的自变量。</summary>
    /// <remarks>设置给定物流对象的自变量。</remarks>
    /// <param name="indVars">需设置的自变量（请参阅状态变量名称以获取有效变量列表）。</param>
    /// <returns>自变量的值。基于 COM 的 CAPE-OPEN。</returns>
    object ICapeThermoMaterialObjectCOM.GetIndependentVar(object indVars)
    {
        try
        {
            var indVarNames = (string[])indVars;
            return _thermo10 ? MaterialObject.GetIndependentVar(indVarNames) : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>验证物性是否有效。</summary>
    /// <remarks>检查给定物性是否可以计算。</remarks>
    /// <returns>返回与待检查物性列表关联的布尔值列表。</returns>
    /// <param name="props">需要检查的物性。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    object ICapeThermoMaterialObjectCOM.PropCheck(object props)
    {
        try
        {
            var propNames = (string[])props;
            return _thermo10 ? MaterialObject.PropCheck(propNames) : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>查看哪些物性可用.</summary>
    /// <remarks>获取已计算的物性列表。</remarks>
    /// <returns>已获得结果的物性。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    object ICapeThermoMaterialObjectCOM.AvailableProps()
    {
        try
        {
            return _thermo10 ? MaterialObject.AvailableProps() : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>删除给定物性之前计算的结果。</summary>
    /// <remarks>删除物流对象中的所有物性或指定物性。</remarks>
    /// <param name="props">删除物流对象中的所有属性或指定属性。使用 emptyObject 删除所有属性。</param>
    void ICapeThermoMaterialObjectCOM.RemoveResults(object props)
    {
        try
        {
            var propNames = (string[])props;
            if (_thermo10) MaterialObject.RemoveResults(propNames);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>创建另一个空的物流对象。</summary>
    /// <remarks>从当前物流对象的父材质模板创建一个物流对象。这与在父材质模板上使用 CreateMaterialObject 方法相同。</remarks> 
    /// <returns>已创建/初始化的物流对象。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    object ICapeThermoMaterialObjectCOM.CreateMaterialObject()
    {
        try
        {
            return _thermo10 ? new COMMaterialObjectWrapper(MaterialObject.CreateMaterialObject()) : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>复制此物流对象。</summary>
    /// <remarks>创建当前物流对象的副本。</remarks>
    /// <returns>已创建/初始化的物流对象。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    //[System.Runtime.InteropServices.DispIdAttribute(15)]
    //[System.ComponentModel.DescriptionAttribute("method Duplicate")]
    //[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.IDispatch)]
    object ICapeThermoMaterialObjectCOM.Duplicate()
    {
        try
        {
            return _thermo10 ? new COMMaterialObjectWrapper(MaterialObject.Duplicate()) : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>验证给定物性的有效性。</summary>
    /// <remarks>验证计算的有效性。</remarks>
    /// <returns>返回计算的可靠性量表。</returns>
    /// <param name="props">需要检查可靠性的物性。设置为空值以移除所有物性。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或属性参数为未定义。</exception>
    object ICapeThermoMaterialObjectCOM.ValidityCheck(object props)
    {
        try
        {
            var propNames = (string[])props;
            return _thermo10 ? MaterialObject.ValidityCheck(propNames) : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>获取物性列表。</summary>
    /// <remarks>返回属性包支持的属性及其对应的 CO 计算例程的列表。由于所有组件都必须支持属性 TEMPERATURE、PRESSURE、FRACTION、FLOW、PHASEFRACTION 和 TOTALFLOW，
    /// 因此不能通过 GetPropList 方法返回这些属性。虽然派生属性的属性标识符由另一个属性的标识符组成，但 GetPropList 方法将返回所有支持的派生和非派生属性的标识符。
    /// 例如，一个属性包可以返回以下列表：enthalpy, enthalpy.Dtemperature, entropy, entropy.Dpressure。</remarks>
    /// <returns>物性包中所有支持物性的字符串列表。</returns>
    object ICapeThermoMaterialObjectCOM.GetPropList()
    {
        try
        {
            return _thermo10 ? MaterialObject.GetPropList() : null;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }

    /// <summary>获取此物流对象中的组分数量。</summary>
    /// <remarks>返回物流对象中的组分数量。</remarks>
    /// <returns>物流对象中的组分数量。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    int ICapeThermoMaterialObjectCOM.GetNumComponents()
    {
        try
        {
            return _thermo10 ? MaterialObject.GetNumComponents() : 0;
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(MaterialObject, pEx);
        }
    }
    
    /// <summary>清除所有存储的物理特性值。</summary>
    /// <remarks><para>ClearAllProps 删除所有使用 SetSinglePhaseProp、SetTwoPhaseProp 或 SetOverallProp 方法设置的已存储的物理特性。
    /// 这意味着，在使用 Set 方法之一存储新值之前，任何后续的物理特性检索调用都会引发异常。ClearAllProps 不会删除物流的配置信息，即化合物和相态的列表。</para>
    /// <para>使用 ClearAllProps 方法会创建一个与最初创建时状态相同的物流对象。它是使用 CreateMaterial 方法的替代方案，但预计它在操作系统资源上的开销较小。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准的兼容性原因可以调用此方法，该操作“不”被实现。也就是说，该操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.ClearAllProps()
    {
        if (_thermo10) _pIMatObj.ClearAllProps();
    }

    /// <summary>将存储的所有非常量物理特性（使用 SetSinglePhaseProp、SetTwoPhaseProp 或 SetOverallProp设置的属性）
    /// 从源物流对象复制到当前物流对象的实例中。</summary>
    /// <remarks><para>在使用此方法之前，物流对象必须与源对象具有完全相同的化合物和相态列表。否则，调用此方法将引发异常。
    /// 有两种配置方法：通过 PME 专有机制和使用 CreateMaterial。在 Material Object S 上调用 CreateMaterial，
    /// 然后在新创建的 Material Object N 上调用 CopyFromMaterial(S)，等同于使用已弃用的方法 ICapeMaterialObject.Duplicate。</para>
    /// <para>该方法旨在由客户端使用，例如需要与已连接的物流对象具有相同状态的物流对象的一个单元操作。一个例子是在蒸馏塔中内部流体的表示。</para></remarks>
    /// <param name="source">源对象，从中将复制存储的物性。</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用此方法，该操作“不”被实现。也就是说，操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeFailedInitialisation">复制物流对象的非恒定物理特性的先决条件无效。必要地初始化，例如将当前物流与源具有相同的化合物和相进行配置，尚未执行或失败。</exception>
    /// <exception cref="ECapeOutOfResources">复制非恒定物理特性所需的物理资源超出了限制。</exception>
    /// <exception cref="ECapeNoMemory">用于复制非常量物理特性的物理内存已超出限制。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.CopyFromMaterial(ref object source)
    {
        var sourceMaterial = (ICapeThermoMaterial)source;
        if (sourceMaterial == null) return;
        if (_thermo11) _pIMatObj.CopyFromMaterial(sourceMaterial);
    }

    /// <summary>创建一个与当前物流对象具有相同配置的物流对象。</summary>
    /// <remarks>创建的物流对象不包含任何非常量物理特性值，但与当前物流对象具有相同的配置（化合物和相）。
    /// 这些物理属性值必须使用 SetSinglePhaseProp、SetTwoPhaseProp 或 SetOverallProp 进行设置。在物理特性值设置完成前尝试读取这些值将导致异常。</remarks>
    /// <returns>物流对象的接口。</returns>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用此方法，该操作“不”被实现。也就是说，操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeFailedInitialisation">创建物质对象所需的物理资源已超出限制。</exception>
    /// <exception cref="ECapeOutOfResources">即使由于与 CAPE-OPEN 标准兼容的原因可以调用此方法，该操作“不”被实现。也就是说，操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeNoMemory">用于复制非常量物理特性的物理内存已超出限制。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    object ICapeThermoMaterialCOM.CreateMaterial()
    {
        return _thermo11 ? new COMMaterialObjectWrapper(_pIMatObj.CreateMaterial()) : null;
    }

    /// <summary>获取混合物整体的非恒定物理性质值。</summary>
    /// <remarks><para>GetOverallProp 返回的物理属性值指的是整体混合物。这些值是通过调用 SetOverallProp 方法设置的。整体混合物的物理属性不是
    /// 由实现 ICapeThermoMaterial 接口的组件计算得出的。这些属性值仅用作实现 ICapeThermoEquilibriumRoutine 接口的组件的 CalcEquilibrium 方法的输入规格。</para>
    /// <para>预计该方法通常能够根据任何基准提供物理属性值，即，它应该能够将存储在基准上的值转换为请求的基准。此操作并非总是可能的。
    /// 例如，如果一种或多种化合物的分子量未知，则无法在质量基准和摩尔基准之间进行转换。</para>
    /// <para>虽然某些调用 GetOverallProp 的结果将是一个单一值，但返回类型是 CapeArrayDouble，该方法必须始终返回一个数组，即使它只包含一个元素。</para></remarks>
    /// <param name="results">一个双向数组，其中包含物理属性值（以国际单位制（SI）为单位）的结果向量。</param>
    /// <param name="property">请求值的物理属性的字符串标识符。这必须是可用于整个混合物的单相物理属性或衍生物之一。</param>
    /// <param name="basis">一个字符串，指示结果的基础。有效设置是：“Mass”表示单位质量的物理属性或“Mole”表示摩尔属性。使用“UNDEFINED”作为物理属性的占位符，其基础不适用。</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准的兼容性原因可以调用此方法，但操作 GetOverallProp “未”实现。也就是说，操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的物理属性无法从物流对象获取，可能是由于请求的基础。当在调用 CreateMaterial 或 ClearAllProps 方法后未设置物理属性值时，将引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如属性值为 UNDEFINED。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化操作未执行或执行失败。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.GetOverallProp(string property, string basis, ref object results)
    {
        double[] temp = null;
        _pIMatObj.GetOverallProp(property, basis, ref temp);
        results = temp;
    }

    /// <summary>读取整个混合物的温度、压力和成分。</summary>
    /// <remarks><para>提供此方法是为了使开发人员能够更轻松地高效使用 CAPE-OPEN 接口。它可以在单次调用中返回物流对象中最常请求的信息。</para>
    /// <para>这种方法没有选择基准。结果总是以摩尔分数的形式返回。</para></remarks>
    /// <param name="temperature">双温度（单位：K）。</param>
    /// <param name="pressure">参考双倍压力（单位 Pa）。</param>
    /// <param name="composition">对包含成分（摩尔分数）的 double 数组的引用</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准的兼容性原因可以调用此方法，但操作 GetOverallProp“未”实现。也就是说，操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的物理属性无法从物流对象获取，可能是由于请求的基础。当在调用 CreateMaterial 或 ClearAllProps 方法后未设置物理属性值时，将引发此异常。</exception>
    /// <exception cref="ECapeFailedInitialisation">前提条件无效。未执行必要的初始化或初始化失败。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.GetOverallTPFraction(ref  double temperature, ref  double pressure, ref  object composition)
    {
        double[] temp = null;
        _pIMatObj.GetOverallTPFraction(ref temperature, ref pressure, ref temp);
        composition = temp;
    }

    /// <summary>返回当前物流对象中存在的相态标签。物流对象中当前存在的相态标签。</summary>
    /// <remarks><para>此方法旨在与 SetPresentPhases 方法一起使用。这些方法共同为 PME（或其他客户端）与 Equilibrium Calculator
    /// （或其他实现 ICapeThermoEquilibriumRoutine 接口的组件）之间提供通信手段。设想的操作序列如下。</para>
    /// <para>1. 在请求平衡计算之前，PME 将使用 SetPresentPhases 方法定义可能在平衡计算中考虑的相态列表。通常，这是必要的，
    /// 因为平衡计算器可能能够处理大量的相态，但对于特定应用，可能已知仅涉及某些相态。例如，如果完整的相态列表包含具有以下
    /// 标签的相态（具有明显的解释）：蒸汽、烃液体和水液体，并且需要建模一个液体倾析器，则当前相态可能设置为烃液体和水液体。</para>
    /// <para>2. 然后，ICapeThermoEquilibriumRoutine 接口的 CalcEquilibrium 方法使用 GetPresentPhases 方法
    /// 来获取可能处于平衡状态的相的相标列表。</para>
    /// <para>3. 平衡计算（Equilibrium Calculation）确定哪些相位在平衡状态下实际共存。这个相列表可能是所考虑的相的子集，
    /// 因为有些相在当时的条件下可能并不存在。例如，如果水量足够少，上例中的水液相就可能不存在，因为所有的水都溶解在碳氢化合物液相中。</para>
    /// <para>4. CalcEquilibrium 方法使用 SetPresentPhases 方法显示平衡计算后存在的相位（并设置相属性）。</para>
    /// <para>5. PME 使用 GetPresentPhases 方法找出计算后存在的相位，然后使用 GetSinglePhaseProp 或 GetTPFraction 方法获得相位属性。</para>
    /// <para>要在物流对象（或实现 ICapeThermoMaterial 接口的其他组件）中表示 “存在”相，必须通过 ICapeThermoMaterial 接口
    /// 的 SetPresentPhases 方法来指定。即使 “相”存在，也并不意味着任何 “物理性质”已实际设置，除非 “相状态”（phaseStatus）
    /// 为 “平衡开普”（Cape_AtEquilibrium）或 “估计开普”（Cape_Estimates）（见下文）。</para>
    /// <para>如果不存在任何相态，则 phaseLabels 和 phaseStatus 参数都应返回 UNDEFINED。</para>
    /// <para>phaseStatus 参数包含的条目数量与相位标签的数量相同。下表列出了有效的设置：</para>
    /// <para>Cape_UnknownPhaseStatus - 当指定一个相位可用于平衡计算时，这是正常设置。</para>
    /// <para>Cape_AtEquilibrium - 由于平衡计算的结果，该相态被设置为存在。</para>
    /// <para> Cape_Estimates - 平衡状态的估计值已在 “物流对象”中设定。</para>
    /// <para>所有状态为 “Cape_AtEquilibrium（平衡开普）”的物相，其温度、压力、成分和相分数的设定值都与平衡态相对应，
    /// 即每种化合物的温度、压力和逸散度相等。具有 Cape_Estimates 状态的相在 “物流对象”中设置了温度、压力、成分和相分数值。
    /// 平衡计算器组件可以使用这些值来初始化平衡计算。存储的值是可用的，但不能保证一定会被使用。</para>
    /// <para>使用 ClearAllProps 方法生成的材质对象与最初创建时的状态相同。它是使用 CreateMaterial 方法的替代方法，
    /// 但预计对操作系统资源的开销较小。</para></remarks>
    /// <param name="phaseLabels">指向字符串数组的引用，该数组包含 “物流对象”中存在的相位标签（标识符-名称）列表。
    /// 物流对象中的相位标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法所返回标签的子集。</param>
    /// <param name="phaseStatus">CapeArrayEnumeration 是与每个相态标签相对应的相态状态标志数组。参见下面的说明。</param>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准兼容的考虑，即使可以调用该方法，该操作也 “未”执行。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.GetPresentPhases(ref  object phaseLabels, ref  object phaseStatus)
    {
        string[] temp1 = null;
        CapePhaseStatus[] temp2 = null;
        _pIMatObj.GetPresentPhases(ref temp1, ref temp2);
        phaseLabels = temp1;
        var temp3 = new int[temp2.Length];
        for (var i = 0; i < temp2.Length; i++)
        {
            if (temp2[i] == CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS) temp3[i] = 0;
            if (temp2[i] == CapePhaseStatus.CAPE_ATEQUILIBRIUM) temp3[i] = 1;
            if (temp2[i] == CapePhaseStatus.CAPE_ESTIMATES) temp3[i] = 2;
        }
        phaseStatus = temp3;
    }

    /// <summary>读取混合物的单相非恒定物理属性值。</summary>
    /// <remarks><para>GetSinglePhaseProp 返回的结果参数既可以是一个包含一个或多个数值（如温度）的 CapeArrayDouble，
    /// 也可以是一个 CapeInterface，用于检索由更复杂的数据结构（如分布式属性）描述的单相物理性质。</para>
    /// <para>虽然对 GetSinglePhaseProp 的某些调用结果可能是单个数值，但数值的返回类型是 CapeArrayDouble，
    /// 在这种情况下，该方法必须返回一个数组，即使它只包含一个元素。</para>
    /// <para>如果 GetPresentPhases方法返回了一个相位的标识符，则该相位 “存在”于物流对象中。
    /// 如果指定的相不存在，则 GetSinglePhaseProp 方法会引发异常。即使相存在，也并不意味着任何物理性质都可用。</para>
    /// <para>GetSinglePhaseProp 返回的物理属性值指的是单个相位。这些值可通过直接调用的 SetSinglePhaseProp 方法
    /// 或其他方法（如 ICapeThermoPropertyRoutine 接口的 CalcSinglePhaseProp 方法或 ICapeThermoEquilibriumRoutine 接口
    /// 的 CalcEquilibrium 方法）设置。注：取决于一个以上相的物理性质（如表面张力或 K 值）将通过 GetTwoPhaseProp 方法返回。</para>
    /// <para>预计该方法通常能够提供任何基础上的物理属性值，也就是说，它应该能够将值从其存储的基 础上转换为所要求的基础上。
    /// 这种操作并不总是可行的。例如，如果不知道一种或多种化合物的分子量，就无法将质量分数或质量流转换为摩尔分数或摩尔流。</para></remarks>
    /// <param name="property">CapeString 物理属性的标识符。必须是单相物理性质或其衍生物之一。</param>
    /// <param name="phaseLabel">CapeString 物理属性所对应的相态标签。相位标签必须是本接口的 GetPresentPhases 方法返回的标识符之一。</param>
    /// <param name="basis">CapeString 结果依据。对于单位质量的物理性质，有效设置为 “质量”；对于摩尔性质，有效设置为 “摩尔”。
    /// 对于不适用基础的物理性质，请使用 UNDEFINED 作为占位符。</param>
    /// <param name="results">CapeVariant 结果向量（CapeArrayDouble），包含以 SI 单位或 CapeInterface（见注释）表示的物理属性值。</param>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">物流对象中可能没有所要求的相位标签或基础的所需属性。
    /// 当调用 CreateMaterial（创建流股）方法后未设置属性值，或调用 ClearAllProps（清除所有属性）方法后
    /// 删除了属性值时，就会出现此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用：例如，属性为 UNDEFINED，或 phaseLabel 为未识别的标识符。</exception>
    /// <exception cref="ECapeFailedInitialisation">前提条件无效。未执行必要的初始化或初始化失败。如果指定的相态不存在，则返回该异常。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.GetSinglePhaseProp(string property, string phaseLabel, string basis, ref  object results)
    {
        double[] temp = null;
        _pIMatObj.GetSinglePhaseProp(property, phaseLabel, basis, ref temp);
        results = temp;
    }

    /// <summary>读取相位的温度、压力和成分。</summary>
    /// <remarks><para>提供该方法是为了方便开发人员有效使用 CAPE-OPEN 接口。只需一次调用，它就能返回物流对象最常请求的信息。</para>
    /// <para>该方法不选择基数。成分总是以摩尔分数的形式返回。</para>
    /// <para>要获得整体混合物的等效信息，应使用 ICapeThermoMaterial 接口的 GetOverallTPFraction 方法。</para></remarks>
    /// <returns>不返回任何参数。</returns>
    /// <param name="phaseLabel">需要使用该属性的相态的相态标签。相位标签必须是本接口的 GetPresentPhases 方法返回的标识符之一。</param>
    /// <param name="temperature">温度，单位 K。</param>
    /// <param name="pressure">压力，单位 Pa。</param>
    /// <param name="composition">组分组成，摩尔分数。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因可以调用此方法，也可以调用 GetTPFraction
    /// 作 GetTPFraction。也就是说，该作存在，但当前实现不支持它。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">其中一个属性无法从物流对象中获取。当调用 CreateMaterial 方法后
    /// 属性值未被设置，或通过调用 ClearAllProps 方法清除了属性值时，将引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用：例如，属性值为 UNDEFINED，或 phaseLabel 的标识符无法识别。</exception>
    /// <exception cref="ECapeFailedInitialisation">前提条件无效。必要的初始化未执行或执行失败。如果指定的相态不存在，则会返回此异常。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.GetTPFraction(string phaseLabel, ref  double temperature, ref  double pressure, ref  object composition)
    {
        double[] temp = null;
        _pIMatObj.GetTPFraction(phaseLabel, ref temperature, ref pressure, ref temp);
        composition = temp;
    }

    /// <summary>获取混合物的两相非恒定物理属性值。</summary>
    /// <remarks><para>GetTwoPhaseProp 函数返回的结果参数要么是一个包含一个或多个数值（例如值）的 CapeArrayDouble 数组，
    /// 要么是一个 CapeInterface 接口，该接口可用于检索由更复杂的数据结构（例如分布式物理属性）描述的两相物理属性。</para>
    /// <para>虽然一些调用 GetTwoPhaseProp 的结果可能是单个数值，但是数值的返回类型是 CapeArrayDouble，
    /// 在这种情况下，方法必须返回一个数组，即使它只包含一个元素。</para>
    /// <para>如果一个相态的标识符是由 GetPresentPhases 方法返回的，那么这个相态就是“present”在物流对象中。
    /// 如果指定的任何相态不存在，则由 GetTwoPhaseProp 方法引发异常。即使所有翔太都存在，这并不意味着任何物理性质都是可用的。</para>
    /// <para>GetTwoPhaseProp 方法返回的物理性质值取决于两个相，例如表面张力或 K 值。这些值可以通过直接调用
    /// 的 SetTwoPhaseProp 方法设置，也可以通过其他方法设置，例如 ICapeThermoPropertyRoutine 接口中的
    /// CalcTwoPhaseProp 方法，或 ICapeThermoEquilibriumRoutine 接口中的 CalcEquilibrium 方法。
    /// 注意：依赖于单一相的物理属性由 GetSinglePhaseProp 方法返回。</para>
    /// <para>预计该方法通常能够提供任何基准下的物理性质值，即应能够将存储基准下的值转换为请求的基准。
    /// 此操作并非总是可行。例如，如果一个或多个化合物的分子量未知，则无法在质量基准和摩尔基准之间进行转换。</para>
    /// <para>如果请求组合导数，这意味着将按照指定相态标签的顺序返回两个相态的导数。组合导数返回的值数量将取决于属性的维数。
    /// 例如，如果存在 N 种化合物，则表面张力导数的结果向量将包含 N 种组合导数值的第一相态，然后是 N 种组合导数值的第二相态。
    /// 对于 K 值导数，将按照文档中定义的方式，第一相态有 N2 个导数值，然后是第二相态有 N2 个值。</para></remarks>
    /// <param name="property">请求值的属性标识符。此标识符必须是文档中列出的两相态物理属性或物理属性衍生项之一。</param>
    /// <param name="phaseLabels">需要该属性的相态的相态标签列表。相态标签必须是物流对象的 GetPhaseList 方法返回的标识符中的两个。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用“未定义”作为占位符。详情请参阅文档。</param>
    /// <param name="results">结果向量（CapeArrayDouble）包含以国际单位制（SI）为单位的属性值，或 CapeInterface（参见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑可以调用此方法，该操作仍“未”实现。
    /// 也就是说，该操作存在，但当前实现不支持该操作。如果 PME 不需要两相非恒定物理属性，则可能无需实现此方法。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的属性可能无法从物流对象中获取，可能是由于所请求的相态或基础设置。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。当调用 SetTwoPhaseProp 方法时未执行该操作，或操作失败，
    /// 或所引用的一个或多个相态不存在时，将引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用：例如，属性为 UNDEFINED，或 phaseLabels 中包含未识别的标识符。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.GetTwoPhaseProp(string property, object phaseLabels, string basis, ref  object results)
    {
        double[] temp = null;
        _pIMatObj.GetTwoPhaseProp(property, (string[])phaseLabels, basis, ref temp);
        results = temp;
    }

    /// <summary>为整个混合物设置非恒定的属性值。</summary>
    /// <remarks><para>SetOverallProp 方法设置的属性值指的是整体混合物的属性。这些属性值可通过调用 GetOverallProp 方法获取。
    /// 整体混合物的属性值并非由实现 ICapeThermoMaterial 接口的组件计算得出。这些属性值仅作为
    /// 实现 ICapeThermoEquilibriumRoutine 接口的组件中 CalcEquilibrium 方法的输入参数使用。</para>
    /// <para>尽管通过调用 SetOverallProp 设置的一些属性将具有单一值，但参数值的类型为 CapeArrayDouble，
    /// 且该方法必须始终以数组形式调用，即使该数组仅包含一个元素。</para></remarks>
    /// <param name ="property">CapeString 用于设置值的属性标识符。此标识符必须是可用于整体混合物的
    /// 单相属性或其衍生属性之一。标准标识符在第文档中列出。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用“未定义”作为占位符。</param>
    /// <param name="values">为该属性设置的值。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。如果 PME 不处理任何单相属性，则可能不需要该方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即该值不属于上述有效值列表，例如属性为 UNDEFINED 时。</exception>
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.SetOverallProp(string property, string basis, object values)
    {
        _pIMatObj.SetOverallProp(property, basis, (double[])values);
    }

    /// <summary>允许 PME 或属性包指定当前存在的相态列表。</summary>
    /// <remarks><para>SetPresentPhases 可能用于：</para>
    /// <para>1. 将平衡计算（使用实现 ICapeThermoEquilibriumRoutine 接口的组件的 CalcEquilibrium 方法）
    /// 限制在属性包组件支持的相的子集中；</para>
    /// <para>2. 当实现 ICapeThermoEquilibriumRoutine 接口的组件需要在进行平衡计算后指定物流对象中存在的相时。</para>
    /// <para>如果列表中的某个相已经存在，则其物理属性不会因该方法的操作而改变。调用 SetPresentPhases 时不在列表中的相位
    /// 将从物流对象中删除。这意味着被删除的相位上可能存储的任何物理属性值将不再可用（即调用包含该相位的
    /// GetSinglePhaseProp 或 GetTwoPhaseProp 时将返回异常）。调用物流对象的 GetPresentPhases 方法将返回
    /// 与 SetPresentPhases 所指定的相同列表。</para>
    /// <para>phaseStatus 参数包含的条目数必须与相位标签数相同。下表列出了有效的设置：</para>
    /// <para>Cape_UnknownPhaseStatus - 当指定一个相位可用于平衡计算时，这是正常设置。</para>
    /// <para>Cape_AtEquilibrium - 由于平衡计算的结果，该相态被设置为存在。</para>
    /// <para>Cape_Estimates - 平衡状态的估计值已在 “物流对象 ”中设定。</para>
    /// <para>所有状态为 Cape_AtEquilibrium 的相态必须具有与平衡状态相对应的属性，即每个化合物的温度、压力和逸度
    /// 相等（这并不意味着逸度是平衡计算的结果）。Cape_AtEquilibrium 状态应该由实现 ICapeThermoEquilibriumRoutine
    /// 接口的组件的 CalcEquilibrium 方法设置，该方法在成功进行平衡计算后设置。如果平衡相的温度、压力或组成发生变化，
    /// 则物流对象的实现负责将该相的状态重置为 Cape_UnknownPhaseStatus。存储该相的其他属性值不应受到影响。</para>
    /// <para>具有“估算”状态的相必须在物流对象中设置温度、压力、成分和相分数的值。这些值可供平衡计算器组件使用，
    /// 以初始化平衡计算。存储的值可用，但不能保证会被使用。</para></remarks>
    /// <param name="phaseLabels">CapeArrayString 当前相态的标签列表。物流对象中的相态标签必须是 ICapeThermoPhases
    /// 接口的 GetPhaseList 方法返回的标签的子集。</param>
    /// <param name="phaseStatus">与每个相态标签相对应的相态状态标志数组。参见下面的说明。</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用此方法，该操作“不”被实现。也就是说，
    /// 操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，即不属于上述有效列表的值，例如如果
    /// phaseLabels 包含 UNDEFINED 或 phaseStatus 包含不在上述表格中的值。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.SetPresentPhases(object phaseLabels, object phaseStatus)
    {
        var temp = (int[])phaseStatus;
        var statuses = new CapePhaseStatus[temp.Length];
        for (var i = 0; i < temp.Length; i++)
        {
            if (temp[i] == 0) statuses[i] = CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS;
            if (temp[i] == 1) statuses[i] = CapePhaseStatus.CAPE_ATEQUILIBRIUM;
            if (temp[i] == 2) statuses[i] = CapePhaseStatus.CAPE_ESTIMATES;
        }
        _pIMatObj.SetPresentPhases((string[])phaseLabels, statuses);
    }

    /// <summary>设置混合物的单相非恒定属性值。</summary>
    /// <remarks><para>SetSinglePhaseProp 的值参数可以是一个 CapeArrayDouble（包含一个或多个要设置的属性数值，如温度），
    /// 也可以是一个 CapeInterface（可用于设置由更复杂数据结构描述的单相属性，如分布式属性）。</para>
    /// <para>虽然通过调用 SetSinglePhaseProp 设置的某些属性只有一个数值，但数值参数的类型是 CapeArrayDouble，在这种情况下，
    /// 即使只包含一个元素，也必须使用包含数组的值调用该方法。</para>
    /// <para>通过 SetSinglePhaseProp 设置的属性值指的是单个相。取决于多个相的属性，如表面张力或 K 值，
    /// 则通过物流对象的 SetTwoPhaseProp 方法来设置。</para>
    /// <para>在使用 SetSinglePhaseProp 之前，必须先使用 SetPresentPhases 方法创建所引用的相位。</para></remarks>
    /// <param name="prop">设置值的属性的标识符。这必须是单相属性或导数的标识符之一。标准标识符在文档中列出。</param>
    /// <param name="phaseLabel">设置该属性的相位的相位标签。相位标签必须是本接口的 GetPresentPhases 方法返回的字符串之一。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。对于不适用基础的物理性质，使用“未定义”作为占位符。</param>
    /// <param name="values">为属性（CapeArrayDouble）或 CapeInterface 设置的值（参见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准的兼容性原因可以调用此方法，该操作“不”被实现。
    /// 也就是说，操作存在，但当前实现不支持。如果PME不处理任何单相属性，则可能不需要此方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即参数值不属于上述有效列表，例如属性的 UNDEFINED。</exception> 
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象可接受的值范围。</exception> 
    /// <exception cref="ECapeFailedInitialisation">前提条件无效。未使用 SetPresentPhases 创建所引用的相态。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.SetSinglePhaseProp(string prop, string phaseLabel, string basis, object values)
    {
        _pIMatObj.SetSinglePhaseProp(prop, phaseLabel, basis, (double[])values);
    }

    /// <summary>为混合物设置两相非恒定属性值。</summary>
    /// <remarks><para>SetTwoPhaseProp 的值参数可以是一个 CapeArrayDouble（包含一个或多个要为属性设置的数值，如 k 值），
    /// 也可以是一个 CapeInterface（可用于设置由更复杂数据结构描述的两相属性，如分布式属性）。</para>
    /// <para>虽然通过调用 SetTwoPhaseProp 设置的某些属性只有一个数值，但数值参数的类型是 CapeArrayDouble，
    /// 在这种情况下，调用该方法时必须使用包含数组的数值参数，即使数组只包含一个元素。</para>
    /// <para>SetTwoPhaseProp 设置的物理属性值取决于两个相，例如表面张力或 K 值。
    /// 取决于单相的属性值可通过 SetSinglePhaseProp 方法设置。</para>
    /// <para>如果指定了具有成分导数的物理属性，则将按照指定相标签的顺序为两个相设置导数值。返回的成分导数值数量取决于属性。
    /// 例如，如果有 N 个化合物，那么表面张力导数的值向量将包含第一相的 N 个成分导数值，然后是第二相的 N 个成分导数值。
    /// 对于 K 值，将按照文档中定义的顺序为第一相提供 N2 个导数值，然后为第二相提供 N2 个导数值。</para>
    /// <para>在使用 SetTwoPhaseProp 之前，必须已使用 SetPresentPhases 方法创建了所有引用的相态。</para></remarks>
    /// <param name="property">在 “物流对象 ”中设置值的属性。它必须是第 7.5.6 和 7.6 节中包含的两相属性或衍生物之一。</param>
    /// <param name="phaseLabels">设置该属性的相位标签。相位标签必须是 ICapeThermoPhases 接口的
    /// GetPhaseList 方法返回的两个标识符。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。对于不适用基础的物理性质，使用“未定义”作为占位符。</param>
    /// <param name="values">要为属性（CapeArrayDouble）或 CapeInterface 设置的值（见注释）。</param>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现不支持它。如果 PME 不处理任何单相属性，则可能不需要此方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即参数值不属于上述有效列表，例如属性的 UNDEFINED。</exception> 
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象可接受的值范围。</exception> 
    /// <exception cref="ECapeFailedInitialisation">前提条件无效。未使用 SetPresentPhases 创建所引用的相态。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不合适时，应抛出的错误。</exception>
    void ICapeThermoMaterialCOM.SetTwoPhaseProp(string property, object phaseLabels, string basis, object values)
    {
        _pIMatObj.SetTwoPhaseProp(property, (string[])phaseLabels, basis, (double[])values);
    }

    /// <summary>返回指定化合物的恒定物理性质值。</summary>
    /// <remarks><para>使用 GetConstPropList 方法可以检查哪些常量物理属性可用。</para>
    /// <para>如果请求的物理属性数为 P，化合物数为 C，则 propvals 数组将包含 C*P 变体。第一个 C 变量将是第一个
    /// 请求的物理属性值（每个化合物一个变量），然后是第二个物理属性的 C 常量值，以此类推。
    /// 返回值的实际类型（双、字符串等）取决于第 7.5.2 节规定的物理属性。</para>
    /// <para>根据第 7.5.2 节的规定，物理性质将以一组固定的单位返回。</para>
    /// <para>如果 compIds 参数设置为 UNDEFINED，则请求返回实现 ICapeThermoCompounds 接口的组件中所有化合物的属性值，
    /// 化合物顺序与 GetCompoundList 方法返回的顺序相同。例如，如果属性包组件实现了该接口，将 compIds 设置
    /// 为 UNDEFINED 的属性请求表示属性包中的所有化合物，而不是传递给属性包的物流对象中的所有化合物。</para>
    /// <para>如果一个或多个化合物的任何物理属性不可用，则必须返回这些组合的未定义值，并引发 ECapeThrmPropertyNotAvailable
    /// 异常。如果出现异常，客户端应检查返回的所有值，以确定哪个值是未定义的。</para></remarks>
    /// <param name="props">物理属性标识符列表。常量物理属性的有效标识符列于第 7.5.2 节。</param>
    /// <param name="compIds">要检索常量的化合物标识符列表。设置 compIds = UNDEFINED 表示
    /// 组件中实现 ICapeThermoCompounds 接口的所有化合物。</param>
    /// <returns>指定化合物的常量值。</returns>
    /// <exception cref="ECapeNoImpl">由于与 CAPE-OPEN 标准兼容的原因，即使可以调用 GetCompoundConstant 方法，
    /// 但仍 “未 ”执行 GetCompoundConstant 操作。也就是说，该操作是存在的，但当前的实现并不支持。
    /// 如果不支持化合物或属性，则应引发此异常。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">物理性质列表中至少有一项不可用于特定化合物。此异常应视为警告而非错误。</exception>
    /// <exception cref="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。由于第 7.5.2 节中的物理属性列表
    /// 并非详尽无遗，而且未被识别的物理属性标识符可能是有效的，因此如果道具参数中的任何元素未被识别，也应引发此异常。
    /// 如果不支持任何物理属性，则应引发 ECapeNoImpl 异常（见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的复合标识符或道具参数的 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetCompoundConstant 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。如果 GetCompoundConstant 方法是由材质对象实现的，则不会出现该错误。</exception>
    object ICapeThermoCompoundsCOM.GetCompoundConstant(object props, object compIds)
    {
        return _pICompounds.GetCompoundConstant((string[])props, (string[])compIds);
    }

    /// <summary>返回所有化合物的列表。其中包括已识别的化合物标识符和可用于进一步识别化合物的额外信息。</summary>
    /// <remarks><para>如果无法返回任何项目，则应将其值设为 UNDEFINED。同样的信息也可以通过 GetCompoundConstant 方法
    /// 提取。根据第 7.5.2 节的规定，GetCompoundList 参数与化合物常数物理性质之间的等价关系如下：</para>
    /// <para>compIds - compIds 是一个人工制品，由实现 GetCompoundList 方法的组件分配。该字符串通常包含一个唯一
    /// 的化合物标识符，如 “苯”。它必须用于 ICapeThermoCompounds 和 ICapeThermoMaterial 接口方法中所有名为 “compIds ”的参数。</para>
    /// <para>Formulae - chemicalFormula</para>
    /// <para>names - iupacName</para>
    /// <para>boilTemps - normalBoilingPoint</para>
    /// <para>molwts - molecularWeight</para>
    /// <para>casnos casRegistryNumber</para>
    /// <para>当 ICapeThermoCompounds 接口由物流对象实现时，返回的化合物列表在配置物流对象时是固定的。</para>
    /// <para>对于 “属性包 ”组件，“属性包 ”通常只包含为特定应用选择的有限化合物，而不是专有 “属性 系统 ”可能使用的所有化合物。</para>
    /// <para>为了识别属性包的化合物，PME 或其他客户端将使用 casnos 参数，而不是 compIds。这是因为不同的 PME 可能会给相同
    /// 的化合物起不同的名字，而 casnos（几乎总是）唯一的。如果 casnos 不可用（如石油馏分）或不唯一，则可使用 GetCompoundList
    /// 返回的其他信息来区分化合物。但需要注意的是，在与属性包通信时，客户端必须使用在 compIds 参数中返回的化合物标识符。</para></remarks>
    /// <param name="compIds">化合物标识符列表。</param>
    /// <param name="formulae">化合物分子（通式）式表。</param>
    /// <param name="names">组分名称列表。</param>
    /// <param name="boilTemps">沸点温度列表。</param>
    /// <param name="molwts">分子摩尔质量列表。</param>
    /// <param name="casnos">化学文摘社 (CAS) 登记号列表。</param>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准兼容的原因，即使可以调用 GetCompoundList 方法，
    /// 该操作也 “未 ”实现。也就是说，该操作是存在的，但当前的实现并不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为 GetCompoundList 操作指定的其他错误不合适时，将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetCompoundList 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。如果 GetCompoundList 方法是由材质对象实现的，则不会出现该错误。</exception>
    void ICapeThermoCompoundsCOM.GetCompoundList(ref  object compIds, ref  object formulae, ref  object names, ref  object boilTemps, ref  object molwts, ref  object casnos)
    {
        string[] temp1 = null;
        string[] temp2 = null;
        string[] temp3 = null;
        double[] temp4 = null;
        double[] temp5 = null;
        string[] temp6 = null;
        _pICompounds.GetCompoundList(ref temp1, ref temp2, ref temp3, ref temp4, ref temp5, ref temp6);
        compIds = temp1;
        formulae = temp2;
        names = temp3;
        boilTemps = temp4;
        molwts = temp5;
        casnos = temp6;
    }

    /// <summary>返回支持的常量物理属性列表。</summary>
    /// <returns>所有支持的常量物理属性标识符列表。标准常量属性标识符列于第 7.5.2 节。</returns>
    /// <remarks><para>MGetConstPropList 返回可通过 GetCompoundConstant 方法检索的所有常量物理属性的标识符。
    /// 如果不支持任何属性，则应返回 UNDEFINED。CAPE-OPEN 标准没有定义实现 ICapeThermoCompounds 接口的软件组件
    /// 必须提供的最小物理性质列表。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可返回不属于第 7.5.2 节所定义列表的常量物理性质标识符。</para>
    /// <para>然而，该组件的大多数客户可能无法理解这些专有标识符。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准兼容的考虑，即使可以调用 GetConstPropList 方法，
    /// 该操作也 “未 ”实现。也就是说，该操作是存在的，但当前的实现并不支持。</exception>
    /// <exception cref="ECapeUnknown">当为 Get-ConstPropList 操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetConstPropList 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。如果 GetConstPropList 方法是由 Material Object 实现的，则不会出现该错误。</exception>
    object ICapeThermoCompoundsCOM.GetConstPropList()
    {
        return _pICompounds.GetConstPropList();
    }

    /// <summary>返回支持的化合物数量。</summary>
    /// <returns>支持的化合物数量。</returns>
    /// <remarks>此方法返回的化合物数量必须等于此接口的 GetCompoundList 方法返回的化合物标识符数量。它必须是零或正数。</remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetNumCompounds 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。如果 GetNumCompounds 方法是由材质对象实现的，则不会出现该错误。</exception>
    int ICapeThermoCompoundsCOM.GetNumCompounds()
    {
        return _pICompounds.GetNumCompounds();
    }

    /// <summary>返回指定纯化合物随压力变化的物理性质值。</summary>
    /// <param name="props">物理性质标识符列表。与压力有关的物理性质的有效标识符列于第 7.5.4 节。</param>
    /// <param name ="pressure">评估物理性质时的压力（单位 Pa）。</param>
    /// <param name ="compIds">要检索其物理性质的化合物标识符列表。设置 compIds = UNDEFINED 表示组件中实现
    /// ICapeThermoCompounds 接口的所有化合物。</param>
    /// <param name="propVals">指定化合物的属性值。</param>
    /// <remarks><para>可以使用 GetPDependentPropList 方法来检查哪些物理属性可用。</para>
    /// <para>如果请求的物理属性数为 P，化合物数为 C，则 propvals 数组将包含 C*P 值。第一个 C 将是第一个物理属性的值，
    /// 然后是第二个物理属性的 C 值，以此类推。</para>
    /// <para>根据第 7.5.4 节的规定，物理性质将以一组固定的单位返回。</para>
    /// <para>如果 compIds 参数设置为 UNDEFINED，则请求返回实现 ICapeThermoCompounds 接口的组件中所有化合物的属性值，
    /// 化合物顺序与 GetCompoundList 方法返回的顺序相同。例如，如果属性包组件实现了该接口，将 compIds 设置为 UNDEFINED
    /// 的属性请求表示属性包中的所有化合物，而不是传递给属性包的材料对象中的所有化合物。</para>
    /// <para>如果一个或多个化合物的任何物理属性不可用，则必须返回这些组合的未定义值 m，并引发
    /// ECapeThrmPropertyNotAvailable 异常。如果出现异常，客户端应检查返回的所有值，以确定哪个值是未定义的。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，操作是存在的，但当前的实现不支持它。如果不支持 “化合物 ”或 “物理性质”，则应引发此异常。</exception>
    /// <exception cref="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。由于第 7.5.4 节中的物理属性列表
    /// 并非详尽无遗，未被识别的物理属性标识符可能是有效的，因此如果道具参数中的任何元素未被识别，也应引发此异常（而不是
    /// ECapeInvalidArgument）。如果不支持任何物理属性，则应引发 ECapeNoImpl（见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">用于传递无效参数值时，例如参数 props 的 UNDEFINED。</exception>
    /// <exception cref="ECapeOutOfBounds">压力值超出了属性包的接受范围。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">属性列表中至少有一项不适用于特定化合物。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetPDependentProperty 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。如果 GetPDependentProperty 方法是由材质对象实现的，则不会出现该错误。</exception>
    void ICapeThermoCompoundsCOM.GetPDependentProperty(object props, double pressure, 
        object compIds, ref  object propVals)        
    {
        double[] temp = null;
        _pICompounds.GetPDependentProperty((string[])props, pressure, (string[])compIds, ref temp);
        propVals = temp;
    }

    ///<summary>返回支持的压力相关属性列表。</summary>
    ///<returns>所有支持的与压力有关的物理性质标识符列表。标准标识符列于第 7.5.4 节</returns>
    /// <remarks><para>GetPDependentPropList 返回可通过 GetPDependentProperty 方法获取的所有压力相关属性的标识符。
    /// 如果不支持任何属性，则应返回 UNDEFINED。CAPE-OPEN 标准没有定义实现 ICapeThermoCompounds 接口的软件组件必须
    /// 提供的最小物理性质列表。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可返回不属于第 7.5.4 节所定义列表的标识符。但是，
    /// 该组件的大多数客户可能无法理解这些专有标识符。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetPDependentPropList 方法之前调用 SetMaterial
    /// 方法，则会引发错误。如果 GetPDependentPropList 方法是由材质对象实现的，则不会出现该错误。</exception>
    object ICapeThermoCompoundsCOM.GetPDependentPropList()
    {
        return _pICompounds.GetPDependentPropList();
    }

    /// <summary>返回指定纯化合物随温度变化的物理性质值。</summary>
    /// <param name ="props">物理性质标识符列表。温度相关物理性质的有效标识符列于第 7.5.3 节。</param>
    /// <param name="temperature">评估属性时的温度（单位 K）。</param>
    /// <param name ="compIds">要检索其物理性质的化合物标识符列表。设置 compIds = UNDEFINED 表示组件中实现
    /// ICapeThermoCompounds 接口的所有化合物。</param>
    /// <param name="propVals">指定化合物的物理属性值。</param>
    /// <remarks><para>可以使用 GetTDependentPropList 方法来检查哪些物理属性可用。</para>
    /// <para>如果请求的物理属性数为 P，化合物数为 C，则 propvals 数组将包含 C*P 值。第一个 C 将是第一个物理属性的值，
    /// 然后是第二个物理属性的 C 值，以此类推。</para>
    /// <para>根据第 7.5.3 节的规定，属性以一组固定的单位返回。</para>
    /// <para>如果 compIds 参数设置为 UNDEFINED，则请求返回实现 ICapeThermoCompounds 接口的组件中所有化合物的属性值，
    /// 化合物顺序与 GetCompoundList 方法返回的顺序相同。例如，如果属性包组件实现了该接口，将 compIds 设置为 UNDEFINED
    /// 的属性请求表示属性包中的所有化合物，而不是传递给属性包的材料对象中的所有化合物。</para>
    /// <para>如果一个或多个化合物的任何物理属性不可用，则必须返回这些组合的未定义值，并引发 ECapeThrmPropertyNotAvailable
    /// 异常。如果出现异常，客户端应检查返回的所有值，以确定哪个值是未定义的。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，操作是存在的，但当前的实现不支持它。如果不支持 “化合物 ”或 “物理性质”，则应引发此异常。</exception>
    /// <exception cref="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。由于第 7.5.3 节中的属性列表并非
    /// 详尽无遗，未被识别的物理属性标识符可能是有效的，因此如果道具参数中的任何元素未被识别，也应引发此异常
    /// （而不是 ECapeInvalidArgument）。如果不支持任何属性，则应引发 ECapeNoImpl（见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">用于传递无效参数值时，例如参数 props 的 UNDEFINED。</exception> 
    /// <exception cref="ECapeOutOfBounds">The value of the temperature is outside
    /// of the range of values accepted by the Property Package.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">at least one item in the 
    /// properties list is not available for a particular compound.</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetTDependentProperty 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。如果 GetTDependentProperty 方法是由材质对象实现的，则不会出现该错误。</exception>
    void ICapeThermoCompoundsCOM.GetTDependentProperty(object props, double temperature, 
        object compIds, ref  object propVals)
    {
        double[] temp = null;
        _pICompounds.GetTDependentProperty((string[])props, temperature, (string[])compIds, ref temp);
        propVals = temp;
    }

    /// <summary>返回支持的随温度变化的物理属性列表。</summary>
    /// <returns>物理属性标识符列表，包含所有支持的温度相关属性。标准标识符列于第 7.5.3 节。</returns>
    /// <remarks><para>GetTDependentPropList 返回可通过 GetTDependentProperty 方法检索的所有与温度相关的物理属性
    /// 的标识符。如果不支持任何属性，则应返回 UNDEFINED。CAPE-OPEN 标准并没有定义实现 ICapeThermoCompounds 接口的
    /// 软件组件必须提供的最小属性列表。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可返回不属于第 7.5.3 节所定义列表的标识符。但是，该组件的大多数客户
    /// 可能无法理解这些专有标识符。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetTDependentPropList 方法之前调用 SetMaterial
    /// 方法，则会引发错误。如果 GetTDependentPropList 方法是由 Material Object 实现的，则不会出现该错误。.</exception>
    object ICapeThermoCompoundsCOM.GetTDependentPropList()
    {
        return _pICompounds.GetTDependentPropList();
    }

    /// <summary>返回阶段数。</summary>
    /// <returns>支持的阶段数。</returns>
    /// <remarks>此方法返回的阶段数必须等于此接口的 GetPhaseList 方法返回的阶段标签数。它必须是零或正数。</remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    int ICapeThermoPhasesCOM.GetNumPhases()
    {
        return _pIPhases.GetNumPhases();
    }

    /// <summary>返回与阶段相关的属性信息，以便了解阶段标签背后的信息。</summary>
    /// <param name ="phaseLabel">一个（单一）相位标签。必须是 GetPhaseList 方法返回的值之一。</param>
    /// <param name ="phaseAttribute">下表中的阶段属性标识符之一。</param>
    /// <returns>与阶段属性标识符相对应的值--见下表。</returns>
    /// <remarks><para>获取相位信息（GetPhaseInfo）的目的是允许 PME 或其他客户端使用任意标签识别相位。PME 或其他客户
    /// 端在将流数据映射到 “材质对象 ”或导入 “属性包 ”时需要使用该功能。如果客户端无法识别相位，可以要求用户根据这些属性
    /// 的值提供映射。</para>
    /// <para>下表定义了支持的阶段属性列表，例如，支持气相、有机液相和水相的属性包组件可能会返回以下信息：</para>
    /// <para>Phase label, Gas, Organic, Aqueous</para>
    /// <para>StateOfAggregation, Vapor, Liquid, Liquid</para>
    /// <para>KeyCompoundId, UNDEFINED, UNDEFINED, Water</para>
    /// <para>ExcludedCompoundId, UNDEFINED, Water, UNDEFINED</para>
    /// <para>DensityDescription, UNDEFINED, Light, Heavy</para>
    /// <para>UserDescription, The gas Phase, The organic liquid Phase, The aqueous liquid Phase</para>
    /// <para>TypeOfSolid, UNDEFINED, UNDEFINED, UNDEFINED</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeInvalidArgument">未识别 phaseLabel，或未定义，或未识别 phaseAttribute。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。.</exception>
    object ICapeThermoPhasesCOM.GetPhaseInfo(string phaseLabel, string phaseAttribute)
    {
        return _pIPhases.GetPhaseInfo(phaseLabel, phaseAttribute);
    }

    /// <summary>返回支持的所有阶段的阶段标签和其他重要描述信息。</summary>
    /// <param name="phaseLabels">所支持阶段的阶段标签列表。阶段标签可以是任何字符串，但每个阶段必须有一个唯一的标签。
    /// 如果由于某种原因不支持任何阶段，则应返回 phaseLabels 的未定义值。相位标签的数量也必须等于 GetNumPhases 方法
    /// 返回的相位数量。</param>
    /// <param name="stateOfAggregation">与每个阶段相关的物理聚合状态。必须是以下字符串之一： “Vapor”、“Liquid”、
    /// “Solid” 或 “Unknown”。每个阶段必须有一个单一的聚合状态。该值不得未定义，但可设置为 “Unknown”。</param>
    /// <param name="keyCompoundId">阶段的关键化合物。它必须是化合物标识符（由 GetCompoundList 返回），
    /// 也可以是未定义的，在这种情况下会返回一个未定义值。关键化合物表示相中预计存在的高浓度化合物，例如水相中的水。
    /// 每个相只能有一个关键化合物。</param>
    /// <remarks><para>通过相位标签，可以在 ICapeThermoPhases 接口和其他 CAPE-OPEN 接口的方法中唯一识别相位。
    /// 聚合状态和关键化合物为 PME 或其他客户端提供了一种方法，使其能够根据相位的物理特性来解释相位标签的含义。</para>
    /// <para>该方法返回的所有数组必须具有相同的长度，即等于阶段标签的数量。</para>
    /// <para>要获取相位的更多信息，请使用 GetPhaseInfo 方法。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoPhasesCOM.GetPhaseList(ref object phaseLabels, 
        ref object stateOfAggregation, ref object keyCompoundId)
    {
        string[] temp1 = null;
        string[] temp2 = null;
        string[] temp3 = null;
        _pIPhases.GetPhaseList(ref temp1, ref temp2, ref temp3);
        phaseLabels = temp1;
        stateOfAggregation = temp2;
        keyCompoundId = temp3;
    }
}
