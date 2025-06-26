/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.21
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>物流对象中各相态的状态。</summary>
/// <remarks>所有状态为 Cape_AtEquilibrium 的相均设置有温度、压力、组成和相分数值，这些值对应于平衡状态，
/// 即每个化合物的温度、压力和逸度相等。处于 Cape_Estimates 状态的相的温度、压力、组分和相分数值在物流对象中设置。
/// 这些值可供平衡计算组件用于初始化平衡计算。存储的值可用，但不保证会被使用。</remarks>
[Serializable]
[Flags]
public enum CapeFugacityFlag
{
    /// <summary>无需计算。</summary>
    CAPE_NO_CALCULATION = 0,
    
    /// <summary>对数逸度系数。</summary>
    CAPE_LOG_FUGACITY_COEFFICIENTS = 1,
    
    /// <summary>温度导数。</summary>
    CAPE_T_DERIVATIVE = 2,
    
    /// <summary>压力导数。</summary>
    CAPE_P_DERIVATIVE = 4,
    
    /// <summary>摩尔数量的导数。</summary>
    CAPE_MOLE_NUMBERS_DERIVATIVES = 8
}

/// <summary>用于基于 COM 的 CAPE-OPEN ICapeThermoMaterialObject 物流对象的封装类。</summary>
/// <remarks><para>此类是 COM 基底的 CAPE-OPEN ICapeThermoMaterialObject 材料对象的包装类。在使用此包装类时，
/// COM 基底的材料的生命周期由我们管理，并且会在材料上调用 COM 的 Release() 方法。</para>
/// <para>此外，该方法会将 <see cref="ICapeThermoMaterialObject"/> 接口中使用的 COM变体转换为所需的 .Net 对象类型。
/// 这样，在使用基于 COM 的 CAPE-OPEN 材料对象时，就不需要转换数据类型了。</para></remarks>
[ComVisible(false)]
[Guid("5A65B4B2-2FDD-4208-813D-7CC527FB91BD")]
[Description("ICapeThermoMaterialObject Interface")]
internal partial class MaterialObjectWrapper : CapeObjectBase, ICapeThermoMaterialObject, ICapeThermoMaterial, ICapeThermoCompounds, 
    ICapeThermoPhases, ICapeThermoUniversalConstant, ICapeThermoEquilibriumRoutine, ICapeThermoPropertyRoutine
{
    [NonSerialized]
    private ICapeThermoMaterialObjectCOM _pMaterialObject;
    [NonSerialized]
    private ICapeThermoMaterialCOM _pIMatObj;
    [NonSerialized]
    private ICapeThermoCompoundsCOM _pICompounds;
    [NonSerialized]
    private ICapeThermoPhasesCOM _pIPhases;
    [NonSerialized]
    private ICapeThermoUniversalConstantCOM _pIUniversalConstant;
    [NonSerialized]
    private ICapeThermoPropertyRoutineCOM _pIPropertyRoutine;
    [NonSerialized]
    private ICapeThermoEquilibriumRoutineCOM _pIEquilibriumRoutine;
    
    // 跟踪是否已调用 Dispose 方法。
    private bool _disposed;

    /// <summary>创建 MaterialObjectWrapper 类的实例。</summary>
    /// <param name="materialObject">要打包的物流对象。</param>
    public MaterialObjectWrapper(object materialObject)
    {
        _disposed = false;
        SupportsThermo10 = false;
        SupportsThermo11 = false;
        
        if (materialObject is ICapeThermoMaterialObjectCOM mCom)
        {
            _pMaterialObject = mCom;
            SupportsThermo10 = true;
        }
        _pIMatObj = null;
        _pIPropertyRoutine = null;
        _pIUniversalConstant = null;
        _pIPhases = null;
        _pICompounds = null;
        _pIEquilibriumRoutine = null;
        if (materialObject is not ICapeThermoMaterialCOM pCom) return;
        SupportsThermo11 = true;
        _pIMatObj = pCom;
        _pIPropertyRoutine = (ICapeThermoPropertyRoutineCOM)pCom;
        _pIUniversalConstant = (ICapeThermoUniversalConstantCOM)pCom;
        _pIPhases = (ICapeThermoPhasesCOM)pCom;
        _pICompounds = (ICapeThermoCompoundsCOM)pCom;
        _pIEquilibriumRoutine = (ICapeThermoEquilibriumRoutineCOM)pCom;
    }

    // 使用 C# 析构函数语法来实现最终化代码。
    /// <summary><see cref="MaterialObjectWrapper"/> 类的终结器。</summary>
    /// <remarks>这将最终确定当前类的实例。</remarks>
    ~MaterialObjectWrapper()
    {
        // 只需调用 Dispose(false)。
        Dispose(false);
    }

    /// <summary>释放 CapeIdentification 对象使用的非托管资源，并可选择性地释放托管资源。</summary>
    /// <remarks><para>这种方法由公共方法 <see href="https://msdn.microsoft.com">Dispose</see> 和
    /// <see href="https://msdn.microsoft.com">Finalize</see> 调用。Dispose() 方法会调用受保护的 Dispose(Boolean) 方法，
    /// 同时将 disposing 参数设置为 true。Finalize() 方法会调用 Dispose() 方法，同时将 disposing 参数设置为 false。</para>
    /// <para>当处置参数为 true 时，此方法将释放由该组件引用的任何托管对象所占用的所有资源。此方法将调用每个引用对象的 Dispose() 方法。</para>
    /// <para>致继承开发者的注意事项：</para>
    /// <para>Dispose 方法可以被其他对象多次调用。在重写 Dispose(Boolean) 方法时，请注意不要引用在之前调用 Dispose 方法时已经
    /// 被释放的对象。有关如何实现 Dispose(Boolean) 方法的更多信息，请参阅 <see href="https://msdn.microsoft.com">实现 Dispose 方法</see>。</para>
    /// <para>有关 Dispose 和 <see href="https://msdn.microsoft.com">Finalize</see> 的更多信息，
    /// 请参阅 <see href="https://msdn.microsoft.com">清理未托管资源</see> 和 <see href="https://msdn.microsoft.com">重写 Finalize 方法</see>。</para></remarks> 
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
        if (_pIMatObj != null)
        {
            if (_pIMatObj.GetType().IsCOMObject)
            {
                Marshal.FinalReleaseComObject(_pIMatObj);
            }
        }
        _pIMatObj = null;
        if (_pICompounds != null)
        {
            if (_pICompounds.GetType().IsCOMObject)
            {
                Marshal.FinalReleaseComObject(_pICompounds);
            }
        }
        _pICompounds = null;
        if (_pIPhases != null)
        {
            if (_pIPhases.GetType().IsCOMObject)
            {
                Marshal.FinalReleaseComObject(_pIPhases);
            }
        }
        _pIPhases = null;
        if (_pIUniversalConstant != null)
        {
            if (_pIUniversalConstant.GetType().IsCOMObject)
            {
                Marshal.FinalReleaseComObject(_pIUniversalConstant);
            }
        }
        _pIUniversalConstant = null;
        if (_pIPropertyRoutine != null)
        {
            if (_pIPropertyRoutine.GetType().IsCOMObject)
            {
                Marshal.FinalReleaseComObject(_pIPropertyRoutine);
            }
        }
        _pIPropertyRoutine = null;
        _disposed = true;
    }

    /// <summary>获取并设置组件的名称。</summary>
    /// <remarks><para>在一个系统中，特定的用例可能包含多个同类的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的名称和描述，
    /// 以便以明确且用户友好的方式加以指代。由于并非所有能够设定这些标识的软件组件，以及需要这些信息的其他软件组件，都是由同一家
    /// 供应商开发的，因此需要一个 CAPE-OPEN 标准来设定和获取这些信息。</para>
    /// <para>因此，该组件通常不会自行设置其名称和描述：应由组件的使用者来完成这些操作。</para></remarks>
    /// <value>组件的唯一名称。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public override string ComponentName
    {
        get => ((ICapeIdentification)_pMaterialObject).ComponentName;
        set => ((ICapeIdentification)_pMaterialObject).ComponentName = value;
    }

    /// <summary> Gets and sets the description of the component.</summary>
    /// <remarks><para>在一个系统中，特定的用例可能包含多个同类的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的名称和描述，
    /// 以便以明确且用户友好的方式加以指代。由于并非所有能够设定这些标识的软件组件，以及需要这些信息的其他软件组件，都是由同一家
    /// 供应商开发的，因此需要一个 CAPE-OPEN 标准来设定和获取这些信息。</para>
    /// <para>因此，该组件通常不会自行设置其名称和描述：应由组件的使用者来完成这些操作。</para></remarks>
    /// <value>组件的描述。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public override string ComponentDescription
    {
        get => ((ICapeIdentification)_pMaterialObject).ComponentDescription;
        set => ((ICapeIdentification)_pMaterialObject).ComponentDescription = value;
    }

    /// <summary>提供有关对象是否支持热力学版本 1.0 的信息。</summary>
    /// <remarks><see cref="MaterialObjectWrapper"/> 类用于检查所包装的材料对象是否支持 CAPE-OPEN 1.0 版本的热力学。
    /// 此属性表示该检查的结果。</remarks>
    /// <value>指示打包的物流对象是否支持 CAPE-OPEN 热力学版本 1.0 接口。</value>
    public bool SupportsThermo10 { get; }

    /// <summary>提供有关对象是否支持热力学版本 1.1 的信息。</summary>
    /// <remarks><see cref="MaterialObjectWrapper1"/> 类用于检查所包裹的材料对象是否支持 CAPE-OPEN 1.1 版本的热力学。
    /// 此属性表示该检查的结果。</remarks>
    /// <value>指示打包的物流对象是否支持 CAPE-OPEN 热力学版本 1.1 接口。</value>
    public bool SupportsThermo11 { get; }

    /// <summary>获取包装后的热力学版本 1.0 物流对象。</summary>
    /// <remarks><para>提供对热力学版本 1.0 物流对象的直接访问。</para>
    /// <para>该物流对象暴露了 ICapeThermoMaterialObject 接口的 COM 版本。</para></remarks>
    /// <value>打包的热力学版本 1.0 物流对象。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public object MaterialObject10 => _pMaterialObject;
    
    /// <summary>获取包装后的热力学版本 1.1 物流对象。</summary>
    /// <remarks><para>提供对热力学版本 1.1 物流对象的直接访问。</para>
    /// <para>该物流对象暴露了热力学 1.1 接口的 COM 版本。</para></remarks>
    /// <value>打包的热力学版本 1.1 物流对象。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public object MaterialObject11 => _pIMatObj;

    /// <summary>获取此 MO 的组件 ID。</summary>
    /// <remarks>返回指定物流对象的组件 ID 列表。</remarks>
    /// <value>物流对象中化合物的名称。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
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

    /// <summary>获取此 MO 的相态 ID。</summary>
    /// <remarks>该方法返回 MO 中当时存在的相位。整体相位和多重相位标识符无法通过此方法获取。请参阅有关相位存在的注释，以获取更多信息。</remarks>
    /// <value>物流流股中存在的相。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
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
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
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
    /// <remarks>从“属性包”中检索组件常量。请参阅注释以获取更多信息。</remarks>
    /// <returns>组件常量值，由属性包返回给物流对象中的所有组件这是一个包含一维对象数组的对象。如果我们将请求的属性数量表示为 P，
    /// 请求的组件数量表示为 C，那么该数组将包含 C*P 个对象。前 C 个对象（从位置 0 到 C-1）将是第一个请求属性的值（每个组件对应一个对象）。
    /// 随后（从位置 C 到 2*C-1）将包含第二个请求属性的常量值，依此类推。</returns>
    /// <param name="props">组件常量列表。</param>
    /// <param name="compIds">需要检索常数的组件 ID 列表。对于“物流对象”中的所有组件，请使用 null 值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
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
    /// <remarks>该方法负责进行所有属性计算，并将这些计算委托给关联的热力学系统。该方法在 CAPE-OPEN 调用模式和用户指南部分中得到了
    /// 进一步定义。请参阅注释，以获取关于参数和 CalcProp 描述的更详细说明，以及关于该方法的总体讨论。</remarks>
    /// <param name="props">待计算的属性列表。</param>
    /// <param name="phases">需要计算属性的相态列表。</param>
    /// <param name="calcType">计算类型：混合物性质或纯组分性质。对于混合物中的部分性质，如组分的 fugacity 系数，
    /// 使用“混合物” CalcType。对于纯组分的 fugacity 系数，使用“纯” CalcType。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
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
    /// <remarks>该方法负责从“MaterialObject”中检索计算结果。请参阅注释，以获取关于这些参数的更详细说明。</remarks>
    /// <returns>结果矢量包含以定义的限定符排序的 SI 单位属性值。该数组是一维的，包含属性，
    /// 按照“props”数组中的化合物顺序排列，按照“compIds”数组中的顺序排列。</returns>
    /// <param name="property">从 MaterialObject 请求结果的属性。</param>
    /// <param name="phase">结果的合格相态。</param>
    /// <param name="compIds">用于指定结果的合格组件。可使用空值来指定“物流对象”中的所有组件。对于混合物属性（如液体焓），
    /// 则无需使用此限定符。可使用“emptyObject”作为占位符。</param>
    /// <param name="calcType">结果的合格类型计算。（有效计算类型：纯组分和混合物）</param>
    /// <param name="basis">明确了结果的基础（即质量/摩尔）。默认值为摩尔。如果基础不适用，
    /// 可以使用 NULL 作为默认值或作为该属性的占位符（另请参阅特定属性）。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    double[] ICapeThermoMaterialObject.GetProp(
        string property, string phase, string[] compIds, string calcType, string basis)
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
    /// <remarks>此方法负责设置“物流对象”属性的值。请参阅注释，以获取关于这些参数的更详细说明。</remarks>
    /// <param name="property">从 MaterialObject 请求结果的属性。</param>
    /// <param name="phase">结果的合格相态。</param>
    /// <param name="compIds">用于指定结果中合格组件的参数。emptyObject 用于指定“物流对象”中的所有组件。
    /// 对于诸如液体焓这样的混合属性，此限定参数是不必要的，可使用 emptyObject 作为占位符。</param>
    /// <param name="calcType">结果的合格类型计算。（有效计算类型：纯物和混合物）。</param>
    /// <param name="basis">明确了结果的基础（即质量/摩尔）。默认值为摩尔。如果基础不适用，可以使用 NULL 作为默认值或作为该属性的
    /// 占位符（另请参阅特定属性）。</param>
    /// <param name="values">为该属性设置的值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    void ICapeThermoMaterialObject.SetProp(
        string property, string phase, string[] compIds, string calcType, string basis, double[] values)
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
    /// <remarks>该方法负责将闪蒸计算委托给关联的属性包或平衡服务器。它必须为处于平衡状态的所有相设置质量、组成、温度和压力，
    /// 以及整体混合物的温度和压力（如果未作为计算规范的一部分进行设置）。有关更多信息，请参阅 CalcProp 和 CalcEquilibrium。</remarks>
    /// <param name="flashType">要计算的闪蒸类型。</param>
    /// <param name="props">在平衡状态下计算的属性。若无属性，则为空对象。若为列表，则需为平衡状态下存在的每个相设置属性值。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
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
    /// <remarks>设置给定物流对象的自变量。</remarks>
    /// <param name="indVars">需设置的独立变量（请参阅状态变量名称以获取有效变量列表）。
    /// 一个包含从 COM 对象序列化而来的字符串数组的 System.Object 对象。</param>
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

    /// <summary>获取状态的自变量。</summary>
    /// <remarks>设置给定物流对象的自变量。</remarks>
    /// <param name="indVars">需设置的自变量（请参阅状态变量名称以获取有效变量列表）。</param>
    /// <returns>自变量的值。基于 COM 的 CAPE-OPEN。</returns>
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
    
    /// <summary>验证属性是否有效。</summary>
    /// <remarks>检查给定属性是否可以计算。</remarks>
    /// <returns>返回与要检查的属性列表相关联的布尔列表。</returns>
    /// <param name="props">要检查的属性。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    bool[] ICapeThermoMaterialObject.PropCheck(string[] props)
    {
        try
        {
            return (bool[])_pMaterialObject.PropCheck(props);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>查看哪些属性可用。</summary>
    /// <remarks>获取已计算的属性列表。</remarks>
    /// <returns>有结果的属性。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    string[] ICapeThermoMaterialObject.AvailableProps()
    {
        try
        {
            return (string[])_pMaterialObject.AvailableProps();
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>删除给定属性的任何先前计算的结果。</summary>
    /// <remarks>删除物流对象中的所有或指定属性结果。</remarks>
    /// <param name="props">要删除的属性。使用 emptyObject 删除所有属性。</param>
    void ICapeThermoMaterialObject.RemoveResults(string[] props)
    {
        try
        {
            _pMaterialObject.RemoveResults(props);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }
    
    /// <summary>创建另一个空的物流对象。</summary>
    /// <remarks>从当前材质对象的父物流模板创建物流对象。这与在父物流模板上使用 CreateMaterialObject 方法相同。</remarks> 
    /// <returns>已创建/初始化的物流对象。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref="ECapeLicenceError">ECapeLicenceError</exception>
    ICapeThermoMaterialObject ICapeThermoMaterialObject.CreateMaterialObject()
    {
        try
        {
            return (ICapeThermoMaterialObject)_pMaterialObject.CreateMaterialObject();
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>复制此物理对象。</summary>
    /// <remarks>创建当前物流对象的副本。</remarks>
    /// <returns>创建/初始化的物流对象。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
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
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>检查给定属性的有效性。</summary>
    /// <remarks>检查计算的有效性。</remarks>
    /// <returns>返回计算的可靠性比例。</returns>
    /// <param name="props">要检查可靠性的属性。空值以删除所有属性。</param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    ICapeThermoReliability[] ICapeThermoMaterialObject.ValidityCheck(string[] props)
    {
        try
        {
            return (ICapeThermoReliability[])_pMaterialObject.ValidityCheck(props);
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>获取属性列表。</summary>
    /// <remarks>返回由该属性包支持的属性及其对应的 CO 计算例程的列表。属性
    /// TEMPERATURE、PRESSURE、FRACTION、FLOW、PHASE-FRACTION、TOTAL-FLOW 无法通过 GetPropList 方法获取，
    /// 因为所有组件都必须支持这些属性。尽管衍生属性的属性标识符是由另一个属性的标识符构建的，但 GetPropList 方法
    /// 仍会返回所有支持的衍生和非衍生属性的标识符。
    /// 例如，一个属性包可能返回以下列表：enthalpy, enthalpy.Dtemperature, entropy, entropy.Dpressure。</remarks>
    /// <returns>属性包的所有支持属性的字符串列表。</returns>
    string[] ICapeThermoMaterialObject.GetPropList()
    {
        try
        {
            return (string[])_pMaterialObject.GetPropList();
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }

    /// <summary>获取此物流对象中的组件数量。</summary>
    /// <remarks>返回物流对象中组件的数量。</remarks>
    /// <returns>物流对象中组件的数量。</returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    int ICapeThermoMaterialObject.GetNumComponents()
    {
        try
        {
            return _pMaterialObject.GetNumComponents();
        }
        catch (Exception pEx)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, pEx);
        }
    }
    
    // ICapeThermoMaterial implementation
    /// <summary>删除所有存储的物理属性值。</summary>
    /// <remarks><para>ClearAllProps 删除所有已经使用 SetSinglePhaseProp，SetTwoPhaseProp 或
    /// SetOverallProp 方法设置的存储的物理属性。这意味着，在使用 Set 方法之一存储新值之前，任何后续检索物理属性
    /// 的调用都将导致异常。ClearAllProps 不会删除物流的配置信息，即化合物和相位的列表。</para>
    /// <para>使用 ClearAllProps 方法会使材质对象处于与最初创建时相同的状态。它是使用 CreateMaterial 方法的
    /// 一种替代方法，但它在操作系统资源方面的开销较小。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。
    /// 也就是说，该操作存在，但当前实现不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.ClearAllProps()
    {
        _pIMatObj.ClearAllProps();
    }

    /// <summary>将所有存储的非常量物理属性（使用 SetSinglePhaseProp， SetTwoPhaseProp 或 SetOverallProp 设置）
    /// 从源物流对象复制到物流对象的当前实例。</summary>
    /// <remarks><para>在使用此方法之前，材料对象必须与源对象具有完全相同的化合物和相列表。否则，调用此方法将引发异常。
    /// 有两种方式进行配置：通过 PME 的专有机制，或使用 CreateMaterial 函数。在 Material Object S 上调用
    /// CreateMaterial，然后在新建的 Material Object N 上调用 CopyFromMaterial(S)，
    /// 等同于使用已弃用的方法 ICapeMaterialObject.Duplicate。</para>
    /// <para>该方法旨在由客户端使用，例如，一个单元操作需要一个物质对象具有与它所连接的物质对象之一相同的状态。
    /// 一个例子是精馏塔内部流的表示。</para></remarks>
    /// <param name="source">将从中复制存储属性的源物流对象。</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。
    /// 也就是说，该操作存在，但当前实现不支持它。</exception>
    /// <exception cref="ECapeFailedInitialisation">复制物质对象的非恒定物理属性的先决条件无效。必要的初始化，
    /// 例如将当前物流配置为与源相同的化合物和相，尚未执行或已失败。</exception>
    /// <exception cref="ECapeOutOfResources">复制非恒定物理属性所需的物理资源超出限制。</exception>
    /// <exception cref="ECapeNoMemory">复制非恒定物理属性所需的物理内存已超出限制。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.CopyFromMaterial(ICapeThermoMaterial source)
    {
        _pIMatObj.CopyFromMaterial(((MaterialObjectWrapper)source).MaterialObject11);
    }

    /// <summary>创建一个与当前材料对象具有相同配置的材料对象。</summary>
    /// <remarks>创建的物流对象不包含任何非恒定的物理特性值，但具有与当前物流对象相同的配置（化合物和相）。
    /// 必须使用 SetSinglePhaseProp、SetTwoPhaseProp 或 SetOverallProp 设置这些物理属性值。
    /// 在设置物理属性值之前，任何检索物理属性值的尝试都会导致异常。</remarks>
    /// <returns>物流对象的接口。</returns>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因可以调用该方法，该操作“并未”被实现。
    /// 也就是说，该操作是存在的，但当前实现并不支持它。</exception>
    /// <exception cref="ECapeFailedInitialisation">创建该实体对象所需的物理资源超出了限制。</exception>
    /// <exception cref="ECapeOutOfResources">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。
    /// 也就是说，该操作存在，但当前实现不支持它。</exception>
    /// <exception cref="ECapeNoMemory">创建实体对象所需的物理内存超出了限制。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    ICapeThermoMaterial ICapeThermoMaterial.CreateMaterial()
    {
        return new MaterialObjectWrapper(_pIMatObj.CreateMaterial());
    }

    /// <summary>获取混合物整体的非恒定物理性质值。</summary>
    /// <remarks><para>由 GetOverallProp 返回的物理属性值指的是总体混合物。这些值是通过调用 SetOverallProp 方法
    /// 来设定的。总体混合物的物理属性不由实现 ICapeThermoMaterial 接口的组件计算得出。这些属性值仅被用作
    /// 实现 ICapeThermoEquilibriumRoutine 接口的组件的 CalcEquilibrium 方法的输入参数。</para>
    /// <para>预计该方法通常能够提供任何基准下的物理性质值，即应能够将存储基准下的值转换为请求的基准。
    /// 此操作并非总是可行。例如，如果一个或多个化合物的分子量未知，则无法在质量基准和摩尔基准之间进行转换。</para>
    /// <para>尽管某些对 GetOverallProp 的调用结果可能是一个单一值，但该方法的返回类型为 CapeArrayDouble，
    /// 因此即使数组中仅包含一个元素，该方法也必须始终返回一个数组。</para></remarks>
    /// <param name="results">一个双向数组，其中包含物理属性值（以国际单位制（SI）为单位）的结果向量。</param>
    /// <param name="property">请求值的物理属性的字符串标识。这必须是可用于存储整个混合物的单相物理属性或其衍生值之一。
    /// 标准标识已在第 7.5.5 和第 7.6 节中列出。</param>
    /// <param name="basis">表示结果基准的字符串。有效设置为：“Mass”表示单位质量的物理性质，或“Mole”表示摩尔性质。
    /// 对于不适用基准的物理性质，使用“UNDEFINED”作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <exception cref="ECapeNoImpl">操作 GetOverallProp 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作虽然存在，但当前实现中并未提供支持。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的物理属性无法从材料对象中获取，可能是因为请求的基础属性不存在。
    /// 当调用 CreateMaterial 或 ClearAllProps 方法后未设置物理属性值时，将引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如属性为 UNDEFINED。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化操作未执行或执行失败。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetOverallProp(string property, string basis, ref double[] results)
    {
        object pObj = null;
        _pIMatObj.GetOverallProp(property, basis, ref pObj);
        results = (double[])pObj;
    }

    /// <summary>获取整体混合物的温度、压力和组成。</summary>
    /// <remarks><para>该方法旨在帮助开发人员更高效地利用 CAPE-OPEN 接口。它通过单次调用即可返回材料对象中最常被请求的信息。</para>
    /// <para>该方法不提供基准选择。组成始终以摩尔分数形式返回。</para></remarks>
    /// <param name="temperature">A reference to a double Temperature (in K)</param>
    /// <param name="pressure">A reference to a double Pressure (in Pa)</param>
    /// <param name="composition">一个指向包含“Composition”（摩尔分率）的 double 数组的引用。</param>
    /// <exception cref="ECapeNoImpl">操作 GetOverallProp 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作虽然存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的物理属性无法从材料对象中获取，可能是因为请求的基础属性不存在。
    /// 当调用 CreateMaterial 或 ClearAllProps 方法后未设置物理属性值时，会触发此异常。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化操作未执行或执行失败。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetOverallTPFraction(ref double temperature, ref double pressure, 
        ref double[] composition)
    {
        object pObj = null;
        _pIMatObj.GetOverallTPFraction(temperature, pressure, ref pObj);
        composition = (double[])pObj;
    }

    /// <summary>返回当前存在于物流对象中的各相态的标签。</summary>
    /// <remarks><para>此方法旨在与 SetPresentPhases 方法协同工作。这两个方法共同提供了一种途径，用于 PME（或其他客户端）
    /// 与 Equilibrium Calculator（或其他实现 ICapeThermoEquilibriumRoutine 接口的组件）之间进行通信。设想的操作序列如下。</para>
    /// <para>1. 在请求进行平衡计算之前，PME 将使用 SetPresentPhases 方法来定义可能在平衡计算中考虑的相的列表。
    /// 通常，这是必要的，因为平衡计算器可能能够处理大量的相，但对于特定应用，可能已知仅涉及某些相。例如，如果完整的相列表
    /// 包含具有以下标签的相（其中标签具有明显的含义）：气态、烃液态和液态水，并且需要建模一个液体容器，那么当前的相可能
    /// 设定为烃液态和液态水。</para>
    /// <para>2. 接着，ICapeThermoEquilibriumRoutine 接口的 CalcEquilibrium 方法会使用 GetPresentPhases 方法，
    /// 以获取与可能在平衡状态下存在的相对应的相标签列表。</para>
    /// <para>3. 平衡计算确定在平衡状态下实际共存的相。此相列表可能为所考虑的相的子集，因为某些相可能不会在常见条件下出现。
    /// 例如，如果水的量非常小，那么在上面的例子中，水相可能不存在，因为所有水都会溶解在烃相中。</para>
    /// <para>4. CalcEquilibrium 方法使用 SetPresentPhases 方法来指示平衡计算后存在的相（并设置相属性）。</para>
    /// <para>5. PME 使用 GetPresentPhases 方法来找出计算后存在的相，
    /// 然后可以利用 GetSinglePhaseProp 或 GetTPFraction 方法获取相的属性。</para>
    /// <para>为了表明某个阶段存在于一个实体对象（或其他实现 ICapeThermoMaterial 接口的组件）中，必须通过
    /// ICapeThermoMaterial 接口的 SetPresentPhases 方法进行指定。即使某个阶段存在，也不意味着已经实际
    /// 设定了任何物理属性，除非该阶段的阶段状态为 Cape_AtEquilibrium 或 Cape_Estimates（见下文）。</para>
    /// <para>如果不存在任何阶段，则应为 phaseLabels 和 phaseStatus 参数返回 UNDEFINED。</para>
    /// <para>phaseStatus 参数包含与 Phase 标签数量相同的条目。有效设置如下表所示：</para>
    /// <para>Cape_UnknownPhaseStatus - 当指定某个阶段可用于进行平衡计算时，这是正常的设置。</para>
    /// <para>Cape_AtEquilibrium - 由于进行了平衡计算，该阶段已设定为“当前”。</para>
    /// <para>Cape_Estimates - 平衡状态的估算已设定在“物流对象”中。</para>
    /// <para>所有处于“Cape_AtEquilibrium”状态的相，其设定的温度、压力、成分和相分数值都对应于一种平衡状态，
    /// 即每种化合物的温度、压力和逸度相等。处于“Cape_Estimates”状态的相，其设定的温度值、压力值、成分值和相分
    /// 数值已存储在物流对象中。这些值可供“平衡计算器”组件使用，以启动平衡计算。虽然存储了这些值，但不能保证会被实际采用。</para>
    /// <para>使用 ClearAllProps 方法得到的材质对象，其状态与最初创建时相同。
    /// 它是使用 CreateMaterial 方法的替代方案，但预计它在操作系统资源上的开销较小。</para></remarks>
    /// <param name="phaseLabels">一个引用，指向包含材料对象中阶段标签（标识符 - 名称）列表的字符串数组。
    /// 物流对象中的阶段标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的标签的子集。</param>
    /// <param name="phaseStatus">CapeArrayEnumeration，它是由对应于每个阶段标签的相位状态标志组成的数组。请参阅下文描述。</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。
    /// 也就是说，该操作存在，但当前实现不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetPresentPhases(ref string[] phaseLabels, ref CapePhaseStatus[] phaseStatus)
    {
        object pObj = null;
        object mObj = null;
        _pIMatObj.GetPresentPhases(ref pObj, ref mObj);
        phaseLabels = (string[])pObj;
        phaseStatus = new CapePhaseStatus[phaseLabels.Length];
        var values = (int[])mObj;
        for (var i = 0; i < phaseStatus.Length; i++)
        {
            if (values[i] == 0) phaseStatus[i] = CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS;
            if (values[i] == 1) phaseStatus[i] = CapePhaseStatus.CAPE_ATEQUILIBRIUM;
            if (values[i] == 2) phaseStatus[i] = CapePhaseStatus.CAPE_ESTIMATES;
        }
    }

    /// <summary>获取混合物中单相非恒定物理性质的数值。</summary>
    /// <remarks><para>GetSinglePhaseProp 返回的结果可以是包含一个或多个数值（如温度）的 CapeArrayDouble，
    /// 也可以是 CapeInterface，后者可用于检索由更复杂数据结构描述的单一相物理属性，例如分布式属性。</para>
    /// <para>尽管某些调用 GetSinglePhaseProp 的结果可能是一个单一的数值，
    /// 但数值的返回类型是 CapeArrayDouble，在这种情况下，即使该方法只包含一个元素，也必须返回一个数组。</para>
    /// <para>如果材料的标识符由“GetPresentPhases”方法返回，则该阶段“存在”于该材料中。如果指定的阶段不存在，
    /// 则“GetSinglePhaseProp”方法将抛出一个异常。即使阶段存在，也不意味着可以获取任何物理属性。</para>
    /// <para>由 GetSinglePhaseProp 返回的物理属性值指的是一个单独的相。这些值可以通过直接调用 SetSinglePhaseProp 方法
    /// 来设置，或者通过其他方法，如 ICapeThermoPropertyRoutine 接口的 CalcSinglePhaseProp 方法或
    /// ICapeThermoEquilibriumRoutine 接口的 CalcEquilibrium 方法。注意：依赖于多个相的物理属性，
    /// 例如表面张力或 K 值，由 GetTwoPhaseProp 方法返回。</para>
    /// <para>预计该方法通常能够根据任何基准提供物理属性值，即，它应该能够将数值从存储的基准转换为所请求的基准。然而，并非
    /// 所有操作都能顺利进行。例如，如果一种或多种化合物的分子量未知，就无法从质量分数或质量流量转换为摩尔分数或摩尔流量。</para></remarks>
    /// <param name="property">CapeString 请求值的物理属性的标识符。此标识符必须是单相物理属性或其导数之一。
    /// 标准标识符在第 7.5.5 节和第 7.6 节中列出。</param>
    /// <param name="phaseLabel">CapeString 物理属性所需的相的相标签。
    /// 相标签必须是本接口的 GetPresentPhases 方法返回的标识符之一。</param>
    /// <param name="basis">CapeString 结果的基础。有效的设置有：“Mass”（针对单位质量的物理属性）或“Mole”（针对摩尔属性）。
    /// 将“UNDEFINED”用作物理属性的占位符，此时基础不适用。详情请参见第 7.5.5 节。</param>
    /// <param name="results">CapeVariant 结果向量（CapeArrayDouble）包含以国际单位制（SI）为单位的物理属性值，
    /// 或 CapeInterface（见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。
    /// 也就是说，该操作存在，但当前实现不支持它。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">该属性无法从“物流对象”中获取，可能的原因是请求的“相态标签”或
    /// “基础”不存在。当调用“CreateMaterial”后未设置属性值，或调用“ClearAllProps”方法导致属性值被清除时，将引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用：例如，属性值为 UNDEFINED，
    /// 或 phaseLabel 的标识符无法识别。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化尚未执行或已失败。
    /// 如果指定的阶段不存在，将返回此异常。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetSinglePhaseProp(string property, string phaseLabel, string basis, 
        ref double[] results)
    {
        object pObj = null;
        _pIMatObj.GetSinglePhaseProp(property, phaseLabel, basis, ref pObj);
        results = (double[])pObj;
    }

    /// <summary>获取相的温度、压力和组成。</summary>
    /// <remarks><para>提供此方法是为了使开发人员能够更轻松地高效利用 CAPE-OPEN 接口。
    /// 它允许在一次调用中获取物流对象中请求频率最高的信息。</para>
    /// <para>在这种方法中，没有选择基点的选项。化合物的结果总是以摩尔分数的形式返回。</para>
    /// <para>为了获取整体混合物的等效信息，应使用 ICapeThermoMaterial 接口的 GetOverallTPFraction 方法。</para></remarks>
    /// <returns>没有返回值。</returns>
    /// <param name="phaseLabel">所需属性的阶段标签。阶段标签必须是该接口的“GetPresentPhases”方法返回的标识符之一。</param>
    /// <param name="temperature">温度 (in K)</param>
    /// <param name="pressure">压力 (in Pa)</param>
    /// <param name="composition">组成 (mole fractions)</param>
    /// <exception cref="ECapeNoImpl">作 GetTPFraction “未”被实现，即使出于与 CAPE-OPEN 标准兼容的原因可以调用此方法。
    /// 也就是说，该操作存在，但当前实现并不支持它。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">其中一个属性无法从“物流对象”中获取。当在调用“创建物流”后未设置属性值，
    /// 或该值已被调用“清除所有属性”方法所擦除时，将引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用：例如，属性为“UNDEFINED”，
    /// 或者“phaseLabel”使用了未识别的标识符。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化尚未执行或已失败。
    /// 如果指定的阶段不存在，将返回此异常。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetTPFraction(string phaseLabel, ref double temperature, 
        ref double pressure, ref double[] composition)
    {
        object pObj = null;
        _pIMatObj.GetTPFraction(phaseLabel, temperature, pressure, ref pObj);
        composition = (double[])pObj;
    }

    /// <summary>获取混合物中两相非恒定物理性质的数值。</summary>
    /// <remarks><para>GetTwoPhaseProp 方法返回的结果参数可能是包含一个或多个数值（例如 k 值）的 CapeArrayDouble 类型，
    /// 也可能是用于获取由更复杂数据结构描述的两相物理性质（例如分布式物理性质）的 CapeInterface 类型。</para>
    /// <para>尽管某些对 GetTwoPhaseProp 的调用可能返回单个数值，但数值的返回类型为 CapeArrayDouble，因此在这种情况下，
    /// 该方法必须返回一个数组，即使该数组仅包含一个元素。</para>
    /// <para>如果某个相的标识符被 GetPresentPhases 方法返回，则该相在材料中“存在”。如果 GetTwoPhaseProp 方法指定的
    /// 任何相不存在，则会引发异常。即使所有相都存在，这并不意味着任何物理属性可用。</para>
    /// <para>GetTwoPhaseProp 方法返回的物理属性值取决于两个相，例如表面张力或 K 值。这些值可以通过直接调用的
    /// SetTwoPhaseProp 方法设置，也可以通过其他方法设置，例如 ICapeThermoPropertyRoutine 接口中的
    /// CalcTwoPhaseProp 方法，或 ICapeThermoEquilibriumRoutine 接口中的 CalcEquilibrium 方法。
    /// 注意：依赖于单一相的物理属性由 GetSinglePhaseProp 方法返回。</para>
    /// <para>预计该方法通常能够提供任何基准下的物理性质值，即应能够将存储基准下的值转换为请求的基准。此操作并非总是可行。
    /// 例如，如果一个或多个化合物的分子量未知，则无法在质量基准和摩尔基准之间进行转换。</para>
    /// <para>如果请求了组成导数，这意味着导数将按相标签指定的顺序分别返回给两个相。组成导数的返回值数量取决于该属性的维度。
    /// 例如，如果存在 N 个化合物，则表面张力导数的返回向量将包含第一阶段的N个组成导数值，随后是第二阶段的N个组成导数值。
    /// 对于 K 值导数，将包含第一阶段的 N² 个导数值，随后是第二阶段的 N² 个值，顺序如 7.6.2 节所定义。</para></remarks>
    /// <param name="property">请求值的属性标识符。此标识符必须是第 7.5.6 节和第 7.6 节中
    /// 列出的两阶段物理属性或物理属性衍生项之一。</param>
    /// <param name="phaseLabels">需要该属性的阶段的阶段标签列表。阶段标签必须是 Material 对象的
    /// GetPhaseList 方法返回的标识符中的两个。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用 UNDEFINED 作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <param name="results">结果向量（CapeArrayDouble）包含以国际单位制（SI）为单位的属性值，
    /// 或 CapeInterface（见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现并未支持该操作。这种情况可能发生在 PME 不需要两相非恒定物理属性时，
    /// 因此没有必要实现该方法。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的属性可能无法从物流对象中获取，
    /// 可能是由于所请求的相态或基础设置。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。当调用 SetTwoPhaseProp 方法时，
    /// 如果该方法未被调用、调用失败，或者所引用的一个或多个阶段不存在，则会引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用：例如，属性为 UNDEFINED，
    /// 或 phaseLabels 中包含未识别的标识符。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetTwoPhaseProp(string property, string[] phaseLabels, 
        string basis, ref double[] results)
    {
        object pObj = null;
        _pIMatObj.GetTwoPhaseProp(property, phaseLabels, basis, ref pObj);
        results = (double[])pObj;
    }

    /// <summary>为整个混合物设置非恒定的属性值。</summary>
    /// <remarks><para>SetOverallProp 方法设置的属性值指的是整体混合物的属性。这些属性值可通过调用 GetOverallProp 方法
    /// 获取。整体混合物的属性值并非由实现 ICapeThermoMaterial 接口的组件计算得出。这些属性值仅作为实现
    /// ICapeThermoEquilibriumRoutine 接口的组件中 CalcEquilibrium 方法的输入参数使用。</para>
    /// <para>尽管通过调用 SetOverallProp 设置的一些属性将具有单一值，但参数值的类型为 CapeArrayDouble，
    /// 且该方法必须始终以数组形式调用，即使该数组仅包含一个元素。</para></remarks>
    /// <param name="property">CapeString 用于设置值的属性标识符。此标识符必须是可用于整体混合物的单相属性或其衍生属性之一。
    /// 标准标识符在第 7.5.5 节和第 7.6 节中列出。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用 UNDEFINED 作为占位符。详情请参阅第7.5.5节。</param>
    /// <param name="values">为该属性设置的值。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。如果 PME 不处理任何单相属性，则可能不需要该方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即该值不属于上述有效值列表，例如属性为 UNDEFINED 时。</exception>
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不适用时，将引发此错误。</exception>
    void ICapeThermoMaterial.SetOverallProp(string property, string basis, double[] values)
    {
        _pIMatObj.SetOverallProp(property, basis, values);
    }

    /// <summary>允许 PME 或属性包指定当前存在的相态列表。</summary>
    /// <remarks><para>SetPresentPhases 可能用于：</para>
    /// <para>1. 将平衡计算（使用实现 ICapeThermoEquilibriumRoutine 接口的组件的 CalcEquilibrium 方法）限制为
    /// Property Package 组件支持的相的子集；</para>
    /// <para>2. 当实现 ICapeThermoEquilibriumRoutine 接口的组件需要指定在平衡计算完成后，物流对象中包含哪些相时。</para>
    /// <para>如果列表中已存在某个相位，则该相位的物理属性不会因本方法的操作而发生变化。当调用 SetPresentPhases 方法时，
    /// 列表中不存在的任何相位将从物流对象中移除。这意味着存储在被移除相位上的任何物理属性值将不再可用
    /// （即，调用 GetSinglePhaseProp 或 GetTwoPhaseProp 方法时包含该相位将引发异常）。调用物流对象的
    /// GetPresentPhases 方法将返回与 SetPresentPhases 指定的相同列表。</para>
    /// <para>phaseStatus 参数必须包含与 Phase 标签数量相同的条目。有效设置如下表所示：</para>
    /// <para>Cape_UnknownPhaseStatus - 这是在指定相位可用于平衡计算时的默认设置。</para>
    /// <para>Cape_AtEquilibrium - 该阶段已通过平衡计算确定为当前阶段。</para>
    /// <para>Cape_Estimates - 平衡状态的估计值已设置在材料对象中。</para>
    /// <para>所有状态为 Cape_AtEquilibrium 的相都必须具有符合平衡状态的物性参数，即各 Compound 的温度、压力和逸度相等
    /// （这并不意味着逸度是通过平衡计算设定的）。Cape_AtEquilibrium 状态应由实现
    /// <see cref="ICapeThermoEquilibriumRoutine"/>接口的组件在成功完成平衡计算后，通过其 CalcEquilibrium 方法进行设置。
    /// 若某平衡相的温度、压力或组成发生改变，物流对象实现方需负责将该相的状态重置为 Cape_UnknownPhaseStatus。
    /// 该相已存储的其他物性参数值不应受到影响。</para>  
    /// <para>状态为"Estimates"的相必须已在物料对象中设置温度、压力、组成及相分数的数值。这些数值可供平衡计算器
    /// 组件 (Equilibrium Calculator) 用于初始化平衡计算。虽然存储的数值可用， 但并不保证它们一定会被采用。</para></remarks>
    /// <param name="phaseLabels">CapeArrayString The list of Phase labels for 
    /// the Phases present. The Phase labels in the Material Object must be a
    /// subset of the labels returned by the GetPhaseList method of the 
    /// ICapeThermoPhases interface.</param>
    /// <param name="phaseStatus">Array of Phase status flags corresponding to 
    /// each of the Phase labels. See description below.</param>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, that is a value that does not belong to the valid list 
    /// described above, for example if phaseLabels contains UNDEFINED or 
    /// phaseStatus contains a value that is not in the above table.</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.SetPresentPhases(string[] phaseLabels, CapePhaseStatus[] phaseStatus)
    {
        var pObj = new int[phaseStatus.Length];
        for (var i = 0; i < pObj.Length; i++)
        {
            if (phaseStatus[i] == CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS) pObj[i] = 0;
            if (phaseStatus[i] == CapePhaseStatus.CAPE_ATEQUILIBRIUM) pObj[i] = 1;
            if (phaseStatus[i] == CapePhaseStatus.CAPE_ESTIMATES) pObj[i] = 2;
        }
        _pIMatObj.SetPresentPhases(phaseLabels, pObj);
    }

    /// <summary>Sets single-phase non-constant property values for a mixture.</summary>
    /// <remarks><para>The values argument of SetSinglePhaseProp is either a CapeArrayDouble 
    /// that contains one or more numerical values to be set for a property, e.g. 
    /// temperature, or a CapeInterface that may be used to set single-phase 
    /// properties described by a more complex data structure, e.g. distributed 
    /// properties.</para>
    /// <para>Although some properties set by calls to SetSinglePhaseProp will have a 
    /// single numerical value, the type of the values argument for numerical values 
    /// is CapeArrayDouble and in such a case the method must be called with values 
    /// containing an array even if it contains only a single element.</para>
    /// <para>The property values set by SetSinglePhaseProp refer to a single Phase. 
    /// Properties that depend on more than one Phase, for example surface tension or 
    /// K-values, are set by the SetTwoPhaseProp method of the Material Object.</para>
    /// <para>Before SetSinglePhaseProp can be used, the phase referenced must have 
    /// been created using the SetPresentPhases method.</para></remarks>
    /// <param name="prop">The identifier of the property for which values are 
    /// set. This must be one of the single-phase properties or derivatives. The 
    /// standard identifiers are listed in sections 7.5.5 and 7.6.</param>
    /// <param name="phaseLabel">Phase label of the Phase for which the property is 
    /// set. The phase label must be one of the strings returned by the 
    /// GetPresentPhases method of this interface.</param>
    /// <param name="basis">Basis of the results. Valid settings are: “Mass” for
    /// Physical Properties per unit mass or “Mole” for molar properties. Use 
    /// UNDEFINED as a place holder for a Physical Property for which basis does not 
    /// apply. See section 7.5.5 for details.</param>
    /// <param name="values">Values to set for the property (CapeArrayDouble) or
    /// CapeInterface (see notes). </param>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists but it is not supported by
    /// the current implementation. This method may not be required if the PME does 
    /// not deal with any single-phase properties.</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即该值不属于上述有效值列表，例如属性为 UNDEFINED 时。</exception> 
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception> 
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The phase referenced has not been created using SetPresentPhases.</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不适用时，将引发此错误。</exception>
    void ICapeThermoMaterial.SetSinglePhaseProp(string prop, string phaseLabel, string basis, double[] values)
    {
        _pIMatObj.SetSinglePhaseProp(prop, phaseLabel, basis, values);
    }
}
