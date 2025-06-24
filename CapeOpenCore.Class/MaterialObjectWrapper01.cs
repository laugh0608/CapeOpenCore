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
    /// 也就是说，该操作存在，但当前实现不支持它</exception>
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
    /// 也就是说，该操作存在，但当前实现不支持它</exception>
    /// <exception cref="ECapeNoMemory">创建实体对象所需的物理内存超出了限制。</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    ICapeThermoMaterial ICapeThermoMaterial.CreateMaterial()
    {
        return new MaterialObjectWrapper(_pIMatObj.CreateMaterial());
    }

    /// <summary>Retrieves non-constant Physical Property values for the overall mixture.</summary>
    /// <remarks><para>
    /// The Physical Property values returned by GetOverallProp refer to the overall 
    /// mixture. These values are set by calling the SetOverallProp method. Overall 
    /// mixture Physical Properties are not calculated by components that implement 
    /// the ICapeThermoMaterial interface. The property values are only used as 
    /// input specifications for the CalcEquilibrium method of a component that 
    /// implements the ICapeThermoEquilibriumRoutine interface.</para>
    /// <para>It is expected that this method will normally be able to provide 
    /// Physical Property values on any basis, i.e. it should be able to convert 
    /// values from the basis on which they are stored to the basis requested. This 
    /// operation will not always be possible. For example, if the molecular weight 
    /// is not known for one or more Compounds, it is not possible to convert 
    /// between a mass basis and a molar basis.</para>
    /// <para>Although the result of some calls to GetOverallProp will be a single 
    /// value, the return type is CapeArrayDouble and the method must always return 
    /// an array even if it contains only a single element.</para></remarks>
    /// <param name="results"> A double array containing the results vector of 
    /// Physical Property value(s) in SI units.</param>
    /// <param name="property">A String identifier of the Physical Property for 
    /// which values are requested. This must be one of the single-phase Physical 
    /// Properties or derivatives that can be stored for the overall mixture. The 
    /// standard identifiers are listed in sections 7.5.5 and 7.6.</param>
    /// <param name="basis">A String indicating the basis of the results. Valid 
    /// settings are: “Mass” for Physical Properties per unit mass or “Mole” for 
    /// molar properties. Use UNDEFINED as a place holder for a Physical Property 
    /// for which basis does not apply. See section 7.5.5 for details.</param>
    /// <exception cref="ECapeNoImpl">The operation GetOverallProp is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">The Physical Property 
    /// required is not available from the Material Object, possibly for the basis 
    /// requested. This exception is raised when a Physical Property value has not 
    /// been set following a call to the CreateMaterial or ClearAllProps methods.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, for example UNDEFINED for property.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed or has failed.</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetOverallProp(string property, string basis, ref double[] results)
    {
        object obj1 = null;
        _pIMatObj.GetOverallProp(property, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>Retrieves temperature, pressure and composition for the overall mixture.</summary>
    /// <remarks><para>
    ///This method is provided to make it easier for developers to make efficient 
    /// use of the CAPEOPEN interfaces. It returns the most frequently requested 
    /// information from a Material Object in a single call.</para>
    /// <para>There is no choice of basis in this method. The composition is always 
    /// returned as mole fractions.</para></remarks>
    /// <param name="temperature">A reference to a double Temperature (in K)</param>
    /// <param name="pressure">A reference to a double Pressure (in Pa)</param>
    /// <param name="composition">A reference to an array of doubles containing 
    /// the Composition (mole fractions)</param>
    /// <exception cref="ECapeNoImpl">The operation GetOverallProp is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">The Physical Property 
    /// required is not available from the Material Object, possibly for the basis 
    /// requested. This exception is raised when a Physical Property value has not 
    /// been set following a call to the CreateMaterial or ClearAllProps methods.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed or has failed.</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetOverallTPFraction(ref double temperature, ref double pressure, ref double[] composition)
    {
        object obj1 = null;
        _pIMatObj.GetOverallTPFraction(temperature, pressure, ref obj1);
        composition = (double[])obj1;
    }

    /// <summary>Returns Phase labels for the Phases that are currently present in the 
    /// Material Object.</summary>
    /// <remarks><para>This method is intended to work in conjunction with the SetPresentPhases 
    /// method. Together these methods provide a means of communication between a 
    /// PME (or another client) and an Equilibrium Calculator (or other component 
    /// that implements the ICapeThermoEquilibriumRoutine interface). The following 
    /// sequence of operations is envisaged.</para>
    /// <para>1. Prior to requesting an Equilibrium Calculation, a PME will use the 
    /// SetPresentPhases method to define a list of Phases that may be considered in 
    /// the Equilibrium Calculation. Typically, this is necessary because an 
    /// Equilibrium Calculator may be capable of handling a large number of Phases 
    /// but for a particular application, it may be known that only certain Phases 
    /// will be involved. For example, if the complete Phase list contains Phases 
    /// with the following labels (with the obvious interpretation): vapour, 
    /// hydrocarbonLiquid and aqueousLiquid and it is required to model a liquid 
    /// decanter, the present Phases might be set to hydrocarbonLiquid and 
    /// aqueousLiquid.</para>
    /// <para>2. The GetPresentPhases method is then used by the CalcEquilibrium 
    /// method of the ICapeThermoEquilibriumRoutine interface to obtain the list 
    /// of Phase labels corresponding to the Phases that may be present at 
    /// equilibrium.</para>
    /// <para>3. The Equilibrium Calculation determines which Phases actually 
    /// co-exist at equilibrium. This list of Phases may be a sub-set of the Phases 
    /// considered because some Phases may not be present at the prevailing 
    /// conditions. For example, if the amount of water is sufficiently small the 
    /// aqueousLiquid Phase in the above example may not exist because all the water 
    /// dissolves in the hydrocarbonLiquid Phase.</para>
    /// <para>4. The CalcEquilibrium method uses the SetPresentPhases method to indicate 
    /// the Phases present following the equilibrium calculation (and sets the phase 
    /// properties).</para>
    /// <para>5. The PME uses the GetPresentPhases method to find out the Phases present 
    /// following the calculation and it can then use the GetSinglePhaseProp or 
    /// GetTPFraction methods to get the Phase properties.</para>
    /// <para>To indicate that a Phase is ‘present’ in a Material Object (or other 
    /// component that implements the ICapeThermoMaterial interface) it must be 
    /// specified by the SetPresentPhases method of the ICapeThermoMaterial 
    /// interface. Even if a Phase is present, it does not imply that any Physical 
    /// Properties are actually set unless the phaseStatus is Cape_AtEquilibrium 
    /// or Cape_Estimates (see below). </para>
    /// <para>If no Phases are present, UNDEFINED should be returned for both the 
    /// phaseLabels and phaseStatus arguments.</para>
    /// <para>The phaseStatus argument contains as many entries as there are Phase 
    /// labels. The valid settings are listed in the following table:</para>
    /// <para>Cape_UnknownPhaseStatus - This is the normal setting when a Phase is
    /// specified as being available for an Equilibrium Calculation.</para>
    /// <para>Cape_AtEquilibrium - The Phase has been set as present as a result of 
    /// an Equilibrium Calculation.</para>
    /// <para> Cape_Estimates - Estimates of the equilibrium state have been set in 
    /// the Material Object.</para>
    /// <para>All the Phases with a status of Cape_AtEquilibrium have values of 
    /// temperature, pressure, composition and Phase fraction set that correspond 
    /// to an equilibrium state, i.e. equal temperature, pressure and fugacities of 
    /// each Compound. Phases with a Cape_Estimates status have values of temperature,
    /// pressure, composition and Phase fraction set in the Material Object. These 
    /// values are available for use by an Equilibrium Calculator component to 
    /// initialise an Equilibrium Calculation. The stored values are available but 
    /// there is no guarantee that they will be used.</para>
    /// <para>Using the ClearAllProps method results in a Material Object that is in 
    /// the same state as when it was first created. It is an alternative to using 
    /// the CreateMaterial method but it is expected to have a smaller overhead in 
    /// operating system resources.</para></remarks>
    /// <param name="phaseLabels">A reference to a String array that contains the 
    /// list of Phase labels (identifiers – names) for the Phases present in the 
    /// Material Object. The Phase labels in the Material Object must be a
    /// subset of the labels returned by the GetPhaseList method of the 
    /// ICapeThermoPhases interface.</param>
    /// <param name="phaseStatus">A CapeArrayEnumeration which is an array of 
    /// Phase status flags corresponding to each of the Phase labels. 
    /// See description below.</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。也就是说，该操作存在，但当前实现不支持它</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetPresentPhases(ref string[] phaseLabels, ref CapePhaseStatus[] phaseStatus)
    {
        object obj1 = null;
        object obj2 = null;
        _pIMatObj.GetPresentPhases(ref obj1, ref obj2);
        phaseLabels = (string[])obj1;
        phaseStatus = new CapePhaseStatus[phaseLabels.Length];
        int[] values = (int[])obj2;
        for (int i = 0; i < phaseStatus.Length; i++)
        {
            if (values[i] == 0) phaseStatus[i] = CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS;
            if (values[i] == 1) phaseStatus[i] = CapePhaseStatus.CAPE_ATEQUILIBRIUM;
            if (values[i] == 2) phaseStatus[i] = CapePhaseStatus.CAPE_ESTIMATES;
        }
    }

    /// <summary>Retrieves single-phase non-constant Physical Property values for a mixture.</summary>
    /// <remarks><para>The results argument returned by GetSinglePhaseProp is either a 
    /// CapeArrayDouble that contains one or more numerical values, e.g. temperature, 
    /// or a CapeInterface that may be used to retrieve single-phase Physical 
    /// Properties described by a more complex data structure, e.g. distributed 
    /// properties.</para>
    /// <para>Although the result of some calls to GetSinglePhaseProp may be a 
    /// single numerical value, the return type for numerical values is 
    /// CapeArrayDouble and in such a case the method must return an array even if 
    /// it contains only a single element.</para>
    /// <para>A Phase is ‘present’ in a Material if its identifier is returned by 
    /// the GetPresentPhases method. An exception is raised by the GetSinglePhaseProp 
    /// method if the Phase specified is not present. Even if a Phase is present, 
    /// this does not mean that any Physical Properties are available.</para>
    /// <para>The Physical Property values returned by GetSinglePhaseProp refer to 
    /// a single Phase. These values may be set by the SetSinglePhaseProp method, 
    /// which may be called directly, or by other methods such as the CalcSinglePhaseProp 
    /// method of the ICapeThermoPropertyRoutine interface or the CalcEquilibrium 
    /// method of the ICapeThermoEquilibriumRoutine interface. Note: Physical 
    /// Properties that depend on more than one Phase, for example surface tension 
    /// or K-values, are returned by the GetTwoPhaseProp method.</para>
    /// <para>It is expected that this method will normally be able to provide 
    /// Physical Property values on any basis, i.e. it should be able to convert 
    /// values from the basis on which they are stored to the basis requested. This 
    /// operation will not always be possible. For example, if the molecular weight 
    /// is not known for one or more Compounds, it is not possible to convert from 
    /// mass fractions or mass flows to mole fractions or molar flows.</para></remarks>
    /// <param name="property">CapeString The identifier of the Physical Property 
    /// for which values are requested. This must be one of the single-phase Physical 
    /// Properties or derivatives. The standard identifiers are listed in sections 
    /// 7.5.5 and 7.6.</param>
    /// <param name="phaseLabel">CapeString Phase label of the Phase for which 
    /// the Physical Property is required. The Phase label must be one of the 
    ///identifiers returned by the GetPresentPhases method of this interface.</param>
    /// <param name="basis">CapeString Basis of the results. Valid settings are: 
    /// “Mass” for Physical Properties per unit mass or “Mole” for molar properties. 
    /// Use UNDEFINED as a place holder for a Physical Property for which basis does 
    /// not apply. See section 7.5.5 for details.</param>
    /// <param name="results">CapeVariant Results vector (CapeArrayDouble) 
    /// containing Physical Property value(s) in SI units or CapeInterface (see 
    /// notes).	</param>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。也就是说，该操作存在，但当前实现不支持它</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">The property required is 
    /// not available from the Material Object possibly for the Phase label or 
    /// basis requested. This exception is raised when a property value has not been 
    /// set following a call to the CreateMaterial or the value has been erased by 
    /// a call to the ClearAllProps methods.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed: for example UNDEFINED for property, or an unrecognised 
    /// identifier for phaseLabel.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed, or has failed. 
    /// This exception is returned if the Phase specified does not exist.</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetSinglePhaseProp(string property, string phaseLabel, string basis, ref double[] results)
    {
        object obj1 = null;
        _pIMatObj.GetSinglePhaseProp(property, phaseLabel, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>Retrieves temperature, pressure and composition for a Phase.</summary>
    /// <remarks><para>This method is provided to make it easier for developers to make efficient 
    /// use of the CAPEOPEN interfaces. It returns the most frequently requested 
    /// information from a Material Object in a single call.</para>
    /// <para>There is no choice of basis in this method. The composition is always 
    /// returned as mole fractions.</para>
    /// <para>To get the equivalent information for the overall mixture the 
    /// GetOverallTPFraction method of the ICapeThermoMaterial interface should be 
    /// used.</para></remarks>
    /// <returns>No return.</returns>
    /// <param name="phaseLabel">Phase label of the Phase for which the property 
    /// is required. The Phase label must be one of the identifiers returned by the 
    /// GetPresentPhases method of this interface.</param>
    /// <param name="temperature">Temperature (in K)</param>
    /// <param name="pressure">Pressure (in Pa)</param>
    /// <param name="composition">Composition (mole fractions)</param>
    /// <exception cref="ECapeNoImpl">The operation GetTPFraction is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">One of the properties is 
    /// not available from the Material Object. This exception is raised when a 
    /// property value has not been set following a call to the CreateMaterial or 
    /// the value has been erased by a call to the ClearAllProps methods.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed: for example UNDEFINED for property, or an unrecognised 
    /// identifier for phaseLabel.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed, or has failed. 
    /// This exception is returned if the Phase specified does not exist.</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetTPFraction(string phaseLabel, ref double temperature, ref double pressure, ref double[] composition)
    {
        object obj1 = null;
        _pIMatObj.GetTPFraction(phaseLabel, temperature, pressure, obj1);
        composition = (double[])obj1;
    }

    /// <summary>Retrieves two-phase non-constant Physical Property values for a mixture.</summary>
    /// <remarks><para>
    ///The results argument returned by GetTwoPhaseProp is either a CapeArrayDouble 
    /// that contains one or more numerical values, e.g. kvalues, or a CapeInterface 
    /// that may be used to retrieve 2-phase Physical Properties described by a more 
    /// complex data structure, e.g.distributed Physical Properties.</para>
    /// <para>Although the result of some calls to GetTwoPhaseProp may be a single 
    /// numerical value, the return type for numerical values is CapeArrayDouble and 
    /// in such a case the method must return an array even if it contains only a 
    /// single element.</para>
    /// <para>A Phase is ‘present’ in a Material if its identifier is returned by 
    /// the GetPresentPhases method. An exception is raised by the GetTwoPhaseProp 
    /// method if any of the Phases specified is not present. Even if all Phases are 
    /// present, this does not mean that any Physical Properties are available.</para>
    /// <para>The Physical Property values returned by GetTwoPhaseProp depend on two 
    /// Phases, for example surface tension or K-values. These values may be set by 
    /// the SetTwoPhaseProp method that may be called directly, or by other methods 
    /// such as the CalcTwoPhaseProp method of the ICapeThermoPropertyRoutine 
    /// interface, or the CalcEquilibrium method of the ICapeThermoEquilibriumRoutine 
    /// interface. Note: Physical Properties that depend on a single Phase are 
    /// returned by the GetSinglePhaseProp method.</para>
    /// <para>It is expected that this method will normally be able to provide 
    /// Physical Property values on any basis, i.e. it should be able to convert 
    /// values from the basis on which they are stored to the basis requested. This 
    /// operation will not always be possible. For example, if the molecular weight 
    /// is not known for one or more Compounds, it is not possible to convert between 
    /// a mass basis and a molar basis.</para>
    /// <para>If a composition derivative is requested this means that the 
    /// derivatives are returned for both Phases in the order in which the Phase 
    /// labels are specified. The number of values returned for a composition 
    /// derivative will depend on the dimensionality of the property. For example,
    /// if there are N Compounds then the results vector for the surface tension 
    /// derivative will contain N composition derivative values for the first Phase, 
    /// followed by N composition derivative values for the second Phase. For K-value 
    /// derivative there will be N2 derivative values for the first phase followed by 
    /// N2 values for the second phase in the order defined in 7.6.2. </para></remarks>
    /// <param name="property">The identifier of the property for which values are
    /// requested. This must be one of the two-phase Physical Properties or Physical 
    /// Property derivatives listed in sections 7.5.6 and 7.6.</param>
    /// <param name="phaseLabels">List of Phase labels of the Phases for which the
    /// property is required. The Phase labels must be two of the identifiers 
    /// returned by the GetPhaseList method of the Material Object.</param>
    /// <param name="basis">Basis of the results. Valid settings are: “Mass” for
    /// Physical Properties per unit mass or “Mole” for molar properties. Use 
    /// UNDEFINED as a place holder for a Physical Property for which basis does not 
    /// apply. See section 7.5.5 for details.</param>
    /// <param name="results">Results vector (CapeArrayDouble) containing property
    /// value(s) in SI units or CapeInterface (see notes).</param>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation. This could be the case if two-phase non-constant 
    /// Physical Properties are not required by the PME and so there is no particular 
    /// need to implement this method.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">The property required is 
    /// not available from the Material Object possibly for the Phases or basis 
    /// requested.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. This exception is raised when a call to the SetTwoPhaseProp method 
    /// has not been performed, or has failed, or when one or more of the Phases 
    /// referenced does not exist.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed: for example, UNDEFINED for property, or an unrecognised 
    /// identifier in phaseLabels.</exception>
    /// <exception cref="ECapeUnknown">当为此操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoMaterial.GetTwoPhaseProp(string property, string[] phaseLabels, string basis, ref double[] results)
    {
        object obj1 = null;
        _pIMatObj.GetTwoPhaseProp(property, phaseLabels, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>Sets non-constant property values for the overall mixture.</summary>
    /// <remarks><para>The property values set by SetOverallProp refer to the overall mixture. 
    /// These values are retrieved by calling the GetOverallProp method. Overall 
    /// mixture properties are not calculated by components that implement the 
    /// ICapeThermoMaterial interface. The property values are only used as input 
    /// specifications for the CalcEquilibrium method of a component that implements 
    /// the ICapeThermoEquilibriumRoutine interface.</para>
    /// <para>Although some properties set by calls to SetOverallProp will have a 
    /// single value, the type of argument values is CapeArrayDouble and the method 
    /// must always be called with values as an array even if it contains only a 
    /// single element.</para></remarks>
    /// <param name ="property"> CapeString The identifier of the property for which 
    /// values are set. This must be one of the single-phase properties or derivatives 
    /// that can be stored for the overall mixture. The standard identifiers are 
    /// listed in sections 7.5.5 and 7.6.</param>
    /// <param name="basis">Basis of the results. Valid settings are: “Mass” for
    /// Physical Properties per unit mass or “Mole” for molar properties. Use 
    /// UNDEFINED as a place holder for a Physical Property for which basis does not 
    /// apply. See section 7.5.5 for details.</param>
    /// <param name="values">Values to set for the property.</param>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation. This method may not be required if the PME 
    /// does not deal with any single-phase property.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, that is a value that does not belong to the valid list 
    /// described above, for example UNDEFINED for property.</exception>
    /// <exception cref="ECapeOutOfBounds">One or more of the entries in the 
    /// values argument is outside of the range of values accepted by the Material 
    /// Object.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the SetSinglePhaseProp operation, are not suitable.</exception>
    void ICapeThermoMaterial.SetOverallProp(string property, string basis, double[] values)
    {
        _pIMatObj.SetOverallProp(property, basis, values);
    }

    /// <summary>Allows the PME or the Property Package to specify the list of Phases that 
    /// are currently present.</summary>
    /// <remarks><para>SetPresentPhases may be used:</para>
    /// <para>• to restrict an Equilibrium Calculation (using the CalcEquilibrium 
    /// method of a component that implements the ICapeThermoEquilibriumRoutine 
    /// interface) to a subset of the Phases supported by the Property Package 
    /// component;</para>
    /// <para>• when the component that implements the ICapeThermoEquilibriumRoutine 
    /// interface needs to specify which Phases are present in a Material Object 
    /// after an Equilibrium Calculation has been performed.</para>
    /// <para>If a Phase in the list is already present, its Physical Properties are 
    /// unchanged by the action of this method. Any Phases not in the list when 
    /// SetPresentPhases is called are removed from the Material Object. This means 
    /// that any Physical Property values that may have been stored on the removed 
    /// Phases are no longer available (i.e. a call to GetSinglePhaseProp or 
    /// GetTwoPhaseProp including this Phase will return an exception). A call to 
    /// the GetPresentPhases method of the Material Object will return the same list 
    /// as specified by SetPresentPhases.</para>
    /// <para>The phaseStatus argument must contain as many entries as there are 
    /// Phase labels. The valid settings are listed in the following table:</para>
    /// <para>Cape_UnknownPhaseStatus - This is the normal setting when a Phase is 
    /// specified as being available for an Equilibrium Calculation.</para>
    /// <para>Cape_AtEquilibrium - The Phase has been set as present as a result of 
    /// an Equilibrium Calculation.</para>
    /// <para>Cape_Estimates - Estimates of the equilibrium state have been set in 
    /// the Material Object.</para>
    /// <para>All the Phases with a status of Cape_AtEquilibrium must have 
    /// properties that correspond to an equilibrium state, i.e. equal temperature, 
    /// pressure and fugacities of each Compound (this does not imply that the 
    /// fugacities are set as a result of the Equilibrium Calculation). The
    /// Cape_AtEquilibrium status should be set by the CalcEquilibrium method of a 
    /// component that implements the ICapeThermoEquilibriumRoutine interface 
    /// following a successful Equilibrium Calculation. If the temperature, pressure 
    /// or composition of an equilibrium Phase is changed, the Material Object 
    /// implementation is responsible for resetting the status of the Phase to 
    /// Cape_UnknownPhaseStatus. Other property values stored for that Phase should 
    /// not be affected.</para>
    /// <para>Phases with an Estimates status must have values of temperature, 
    ///pressure, composition and phase fraction set in the Material Object. These 
    /// values are available for use by an Equilibrium Calculator component to 
    /// initialise an Equilibrium Calculation. The stored values are available but 
    /// there is no guarantee that they will be used.</para></remarks>
    /// <param name="phaseLabels"> CapeArrayString The list of Phase labels for 
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
        int[] obj1 = new int[phaseStatus.Length];
        for (int i = 0; i < obj1.Length; i++)
        {
            if (phaseStatus[i] == CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS) obj1[i] = 0;
            if (phaseStatus[i] == CapePhaseStatus.CAPE_ATEQUILIBRIUM) obj1[i] = 1;
            if (phaseStatus[i] == CapePhaseStatus.CAPE_ESTIMATES) obj1[i] = 2;
        }
        _pIMatObj.SetPresentPhases(phaseLabels, obj1);
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
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, that is a value that does not belong to the valid list 
    /// described above, for example UNDEFINED for property.</exception> 
    /// <exception cref="ECapeOutOfBounds">One or more of the entries in the 
    /// values argument is outside of the range of values accepted by the Material 
    /// Object.</exception> 
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The phase referenced has not been created using SetPresentPhases.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the SetSinglePhaseProp operation, are not suitable.</exception>
    void ICapeThermoMaterial.SetSinglePhaseProp(string prop, string phaseLabel, string basis, double[] values)
    {
        _pIMatObj.SetSinglePhaseProp(prop, phaseLabel, basis, values);
    }
}
