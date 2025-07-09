/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.07.01
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>用于基于 COM 的 CAPE-OPEN ICapeThermoMaterialObject 物流对象的封装类。</summary>
/// <remarks><para>本类是基于 COM 的 CAPE-OPEN ICapeThermoMaterialObject 物流对象的封装类。
/// 当您使用此封装类时，基于 COM 的物流对象的生命周期将由系统自动管理，并且会调用物流对象的 COM Release() 方法。</para>
/// <para>此外，该方法会将 <see cref="ICapeThermoMaterialObject"/> 接口中使用的 COM 变体转换为所需的 .Net 对象类型。
/// 这样，在使用基于 COM 的 CAPE-OPEN 物流对象时，就不需要转换数据类型了。</para></remarks>
[ComVisible(false)]
[Guid("5A65B4B2-2FDD-4208-813D-7CC527FB91BD")]
[Description("ICapeThermoMaterialObject Interface")]
internal class MaterialObjectWrapper11 : CapeObjectBase, ICapeThermoMaterial, ICapeThermoCompounds,
    ICapeThermoPhases, ICapeThermoUniversalConstant, ICapeThermoEquilibriumRoutine, ICapeThermoPropertyRoutine
{
    [NonSerialized] private ICapeThermoMaterialCOM _pIMatObj;
    [NonSerialized] private ICapeThermoCompoundsCOM _pICompounds;
    [NonSerialized] private ICapeThermoPhasesCOM _pIPhases;
    [NonSerialized] private ICapeThermoUniversalConstantCOM _pIUniversalConstant;
    [NonSerialized] private ICapeThermoPropertyRoutineCOM _pIPropertyRoutine;
    [NonSerialized] private ICapeThermoEquilibriumRoutineCOM _pIEquilibriumRoutine;

    // 跟踪是否已调用 Dispose 方法。
    private bool _disposed;

    /// <summary>创建 MaterialObjectWrapper 类的实例。</summary>
    /// <param name="materialObject">待封装的物流对象。</param>
    public MaterialObjectWrapper11(object materialObject)
    {
        _disposed = false;
        _pIMatObj = null;
        _pIPropertyRoutine = null;
        _pIUniversalConstant = null;
        _pIPhases = null;
        _pICompounds = null;
        _pIEquilibriumRoutine = null;

        if (materialObject is not ICapeThermoMaterialCOM pCom) return;
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
    ~MaterialObjectWrapper11()
    {
        // 只需调用 Dispose(false) 即可。
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
        get => ((ICapeIdentification)_pIMatObj).ComponentName;
        set => ((ICapeIdentification)_pIMatObj).ComponentName = value;
    }

    /// <summary>获取并设置组件的描述。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的 CAPE-OPEN 组件。用户应能够为每个实例分配不同的
    /// 名称和描述，以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并
    /// 不总是由同一供应商开发，因此需要制定一个CAPE-OPEN标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的描述。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public override string ComponentDescription
    {
        get => ((ICapeIdentification)_pIMatObj).ComponentDescription;
        set => ((ICapeIdentification)_pIMatObj).ComponentDescription = value;
    }

    /// <summary>提供有关对象是否支持热力学版本 1.0 的信息。</summary>
    /// <remarks><see cref="MaterialObjectWrapper10"/> 类用于检查所封装的物流对象是否支持
    /// CAPE-OPEN 1.0 版本的热力学。此属性表示该检查的结果。</remarks>
    /// <value>指示封装的物流对象是否支持 CAPE-OPEN 热力学版本 1.0 接口。</value>
    public bool SupportsThermo10 => false;

    /// <summary>提供有关对象是否支持热力学版本 1.1 的信息。</summary>
    /// <remarks><see cref="MaterialObjectWrapper10"/> 类用于检查所包裹的物流对象是否支持
    /// CAPE-OPEN 1.1 版本的热力学。此属性表示该检查的结果。</remarks>
    /// <value>指示包裹的物流对象是否支持 CAPE-OPEN 热力学版本 1.1 接口。</value>
    public bool SupportsThermo11 => true;

    /// <summary>获取封装的热力学版本 1.0 物流对象。</summary>
    /// <remarks><para>提供对热力学版本 1.0 物流对象的直接访问。</para>
    /// <para>该物流对象实现了 ICapeThermoMaterialObject 接口的 COM 版本。</para></remarks>
    /// <value>封装的热力学版本 1.0 物流对象。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public object MaterialObject10 => null;

    /// <summary>获取封装的热力学版本 1.1 物流对象。</summary>
    /// <remarks><para>提供对热力学版本 1.1 物流对象的直接访问。</para>
    /// <para>该物流对象暴露了热力学 1.1 接口的 COM 版本。</para></remarks>
    /// <value>封装了热力学 1.1 的物流对象</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public object MaterialObject11 => _pIMatObj;

    // ICapeThermoMaterial implementation
    /// <summary>清除所有存储的物理属性值。</summary>
    /// <remarks><para>ClearAllProps 方法会清除所有通过 SetSinglePhaseProp、SetTwoPhaseProp 或
    /// SetOverallProp 方法设置的存储物理属性。这意味着，在使用其中一个 Set 方法存储新值之前，任何后续调用
    /// 以检索物理属性的操作都将引发异常。ClearAllProps 不会清除物流的配置信息，即化合物和相的列表。</para>
    /// <para>使用 ClearAllProps 方法会使材质对象恢复到初始创建时的状态。它是一种替代 CreateMaterial 方法
    /// 的方案，但预计在操作系统资源占用方面具有更低的开销。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与CAPE-OPEN标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作虽然存在，但当前实现中并未支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.ClearAllProps()
    {
        _pIMatObj.ClearAllProps();
    }

    /// <summary>将源物流对象中存储的所有非常量物理属性（通过SetSinglePhaseProp、SetTwoPhaseProp 或
    /// SetOverallProp 设置的属性）复制到当前物流对象的实例中。</summary>
    /// <remarks><para>在使用此方法之前，目标物流对象必须与源物流对象配置相同的化合物和相列表。否则，
    /// 调用该方法将引发异常。配置有两种方式：通过 PME 专有机制或使用 CreateMaterial 方法。对物流对象 S 调用
    /// CreateMaterial 方法，随后对新创建的物流对象 N 调用 CopyFromMaterial(S) 方法，
    /// 与已废弃的方法 ICapeMaterialObject.Duplicate 等效。</para>
    /// <para>该方法旨在供客户端使用，例如需要使物流对象与已连接的物流对象之一处于相同状态的单元操作。
    /// 一个示例是蒸馏塔中内部流的表示。</para></remarks>
    /// <param name="source">源对象，从中将复制存储的属性。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作虽然存在，但当前实现中并未支持该操作。</exception>
    /// <exception cref="ECapeFailedInitialisation">复制物流对象的非恒定物理属性所需的先决条件不满足。必要的初始化操作，
    /// 例如将当前物流配置为与源物流具有相同的化合物和相，尚未执行或执行失败。</exception>
    /// <exception cref="ECapeOutOfResources">复制非恒定物理属性所需的物理资源超出了限制。</exception>
    /// <exception cref="ECapeNoMemory">用于复制非常量物理属性的物理内存已超出限制。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.CopyFromMaterial(ICapeThermoMaterial source)
    {
        _pIMatObj.CopyFromMaterial(((MaterialObjectWrapper)source).MaterialObject11);
    }

    /// <summary>创建一个与当前物流对象具有相同配置的物流对象。</summary>
    /// <remarks>创建的物流对象不包含任何非常量物理属性值，但与当前物流对象具有相同的配置（化合物和相）。
    /// 这些物理属性值必须使用 SetSinglePhaseProp、SetTwoPhaseProp 或 SetOverallProp进行设置。
    /// 在物理属性值设置完成之前，任何尝试读取物理属性值的操作都将导致异常。</remarks>
    /// <returns>物流对象的接口。</returns>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，
    /// 但该操作“并未”实现。也就是说，该操作虽然存在，但当前实现中并未支持该操作。</exception>
    /// <exception cref="ECapeFailedInitialisation">创建物流对象所需的物理资源已超出限制。</exception>
    /// <exception cref="ECapeOutOfResources">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，
    /// 但该操作“并未”实现。也就是说，该操作虽然存在，但当前实现中并未支持该操作。</exception>
    /// <exception cref="ECapeNoMemory">创建物流对象所需的物理内存已超出限制。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    ICapeThermoMaterial ICapeThermoMaterial.CreateMaterial()
    {
        return new MaterialObjectWrapper(_pIMatObj.CreateMaterial());
    }

    /// <summary>获取混合物整体的非恒定物理性质值。</summary>
    /// <remarks><para>GetOverallProp 方法返回的物理属性值指的是整体混合物的属性值。这些值通过调用 SetOverallProp
    /// 方法进行设置。整体混合物的物理属性值并非由实现 ICapeThermoMaterial 接口的组件计算得出。这些属性值仅作为输入
    /// 参数用于实现 ICapeThermoEquilibriumRoutine 接口的组件的 CalcEquilibrium 方法。</para>
    /// <para>预计该方法通常能够提供任何基准下的物理性质值，即应能够将存储基准下的值转换为请求的基准。此操作并非总是可行。
    /// 例如，如果一个或多个化合物的分子量未知，则无法在质量基准和摩尔基准之间进行转换。</para>
    /// <para>尽管某些对 GetOverallProp 的调用结果可能是一个单一值，但该方法的返回类型为 CapeArrayDouble，
    /// 因此即使数组中仅包含一个元素，该方法也必须始终返回一个数组。</para></remarks>
    /// <param name="results">一个双向数组，其中包含物理属性值（以国际单位制（SI）为单位）的结果向量。</param>
    /// <param name="property">物理性质的字符串标识符，用于请求其值。此标识符必须是单相物理性质或可用于整体混合物
    /// 的衍生性质之一。标准标识符在第 7.5.5 节和第 7.6 节中列出。</param>
    /// <param name="basis">表示结果基准的字符串。有效设置为：“Mass”表示单位质量的物理性质，或“Mole”表示摩尔性质。
    /// 对于不适用基准的物理性质，使用“UNDEFINED”作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <exception cref="ECapeNoImpl">操作 GetOverallProp 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作虽然存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的物理属性无法从物流对象中获取，可能是因为请求的
    /// 基础属性不存在。当调用 CreateMaterial 或 ClearAllProps 方法后未设置物理属性值时，将引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，例如属性为 UNDEFINED。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化操作未执行或执行失败。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.GetOverallProp(string property, string basis, ref double[] results)
    {
        object obj1 = null;
        _pIMatObj.GetOverallProp(property, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>获取整体混合物的温度、压力和组成。</summary>
    /// <remarks><para>该方法旨在帮助开发人员更高效地利用 CAPE-OPEN 接口。它通过单次调用即可返回物流对象中最常被请求的信息。</para>
    /// <para>该方法不提供基准选择。组成始终以摩尔分数形式返回。</para></remarks>
    /// <param name="temperature">A reference to a double Temperature (in K)</param>
    /// <param name="pressure">A reference to a double Pressure (in Pa)</param>
    /// <param name="composition">A reference to an array of doubles containing the Composition (mole fractions)</param>
    /// <exception cref="ECapeNoImpl">操作 GetOverallProp 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作虽然存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的物理属性无法从物流对象中获取，可能是因为请求的基础属性
    /// 不存在。当调用 CreateMaterial 或 ClearAllProps 方法后未设置物理属性值时，将引发此异常。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化操作未执行或执行失败。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.GetOverallTPFraction(ref double temperature, ref double pressure, ref double[] composition)
    {
        object obj1 = null;
        _pIMatObj.GetOverallTPFraction(temperature, pressure, ref obj1);
        composition = (double[])obj1;
    }

    /// <summary>返回当前存在于物流对象中的各相态的相态标签。</summary>
    /// <remarks><para>该方法旨在与SetPresentPhases方法配合使用。这两种方法共同为 PME（或其他客户端）与平衡计算器
    /// （或其他实现 ICapeThermoEquilibriumRoutine 接口的组件）之间的通信提供了一种机制。以下是预期的操作序列。</para>
    /// <para>1. 在请求平衡计算之前，PME 将使用 SetPresentPhases 方法来定义一个可能在平衡计算中考虑的相列表。通常，
    /// 这是必要的，因为平衡计算器可能能够处理大量相，但对于特定应用，可能已知只有某些相会参与其中。例如，如果完整的相列表
    /// 包含具有以下标签的相（其含义显而易见）：蒸汽、烃类液体和水相液体，且需要模拟一个液体分离器，则当前相可能被设置为
    /// 烃类液体和水相液体。</para>
    /// <para>2. 然后，ICapeThermoEquilibriumRoutine 接口中的 CalcEquilibrium 方法使用 GetPresentPhases 方法
    /// 来获取在平衡状态下可能存在的相对应的相标签列表。</para>
    /// <para>3. 平衡计算确定在平衡状态下实际共存的相。该相的列表可能是所考虑相的子集，因为某些相在当前条件下可能不存在。
    /// 例如，如果水量足够小，上述示例中的水相可能不存在，因为所有水都溶解在烃液相中。</para>
    /// <para>4. CalcEquilibrium 方法使用 SetPresentPhases 方法来指示在平衡计算后存在的相（并设置相的属性）。</para>
    /// <para>5. PME 使用 GetPresentPhases 方法来确定计算后存在的相位，然后可以使用 GetSinglePhaseProp 或
    /// GetTPFraction 方法获取相位属性。</para>
    /// <para>为了表示某个相态在某个实体对象（或其他实现 ICapeThermoMaterial 接口的组件）中“存在”，
    /// 必须通过 ICapeThermoMaterial 接口的 SetPresentPhases 方法进行指定。即使某个相态存在，也不意味着已经实际
    /// 设定了任何物理属性，除非该相态的相态状态为 Cape_AtEquilibrium 或 Cape_Estimates（见下文）。</para>
    /// <para>如果不存在任何相态，则应为 phaseLabels 和 phaseStatus 参数返回 UNDEFINED。</para>
    /// <para>phaseStatus 参数包含与 Phase 标签数量相同的条目。有效设置如下表所示：</para>
    /// <para>Cape_UnknownPhaseStatus - 这是在指定相位可用于平衡计算时的默认设置。</para>
    /// <para>Cape_AtEquilibrium - 该相态已通过平衡计算确定为当前相态。</para>
    /// <para>Cape_Estimates - 平衡状态的估计值已设置在物流对象中。</para>
    /// <para>所有状态为Cape_AtEquilibrium的相均设置有温度、压力、组成和相分数值，这些值对应于平衡状态，即每个
    /// 化合物的温度、压力和逃逸度相等。处于Cape_Estimates状态的相的温度、压力、组成和相分数值在物流对象中设置。
    /// 这些值可供平衡计算组件用于初始化平衡计算。存储的值可用，但不保证会被使用。</para>
    /// <para>使用 ClearAllProps 方法会使材质对象恢复到初始创建时的状态。它是一种替代 CreateMaterial 方法的方案，
    /// 但预计在操作系统资源占用方面具有更低的开销。</para></remarks>
    /// <param name="phaseLabels">对包含物流对象中各相标签（标识符 - 名称）列表的字符串数组的引用。物流对象中的
    /// 相标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的标签的子集。</param>
    /// <param name="phaseStatus">CapeArrayEnumeration 是一个数组，其中包含与每个相态标签对应的相位状态标志。
    /// 请参见下文的描述。</param>
    /// <exception cref="ECapeNoImpl">即使出于与CAPE-OPEN标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作虽然存在，但当前实现中并未支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.GetPresentPhases(ref string[] phaseLabels, ref CapePhaseStatus[] phaseStatus)
    {
        object obj1 = null;
        object obj2 = null;
        _pIMatObj.GetPresentPhases(ref obj1, ref obj2);
        phaseLabels = (string[])obj1;
        phaseStatus = new CapePhaseStatus[phaseLabels.Length];
        var values = (int[])obj2;
        for (var i = 0; i < phaseStatus.Length; i++)
        {
            if (values[i] == 0) phaseStatus[i] = CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS;
            if (values[i] == 1) phaseStatus[i] = CapePhaseStatus.CAPE_ATEQUILIBRIUM;
            if (values[i] == 2) phaseStatus[i] = CapePhaseStatus.CAPE_ESTIMATES;
        }
    }

    /// <summary>获取混合物中单相非恒定物理性质的数值。</summary>
    /// <remarks><para>GetSinglePhaseProp 方法返回的结果参数可能是包含一个或多个数值（例如温度）的
    /// CapeArrayDouble 类型，也可能是用于获取由更复杂数据结构描述的单相物理属性的 CapeInterface 类型，
    /// 例如分布式属性。</para>
    /// <para>尽管某些对 GetSinglePhaseProp 的调用可能返回单个数值，但数值的返回类型为 CapeArrayDouble，
    /// 因此在这种情况下，该方法必须返回一个数组，即使该数组仅包含一个元素。</para>
    /// <para>如果某个相的标识符通过 GetPresentPhases 方法返回，则该相在物流中“存在”。如果指定的相不存在，
    /// GetSinglePhaseProp 方法将抛出异常。即使某个相存在，也不意味着其物理属性可用。</para>
    /// <para>GetSinglePhaseProp 方法返回的物理属性值对应于单一相。这些值可通过 SetSinglePhaseProp 方法设置，
    /// 该方法可直接调用，也可通过其他方法设置，例如 ICapeThermoPropertyRoutine 接口的 CalcSinglePhaseProp
    /// 方法或 ICapeThermoEquilibriumRoutine 接口的 CalcEquilibrium 方法。注意：依赖于多个相的物理属性
    /// （例如表面张力或 K 值）由 GetTwoPhaseProp 方法返回。</para>
    /// <para>预计该方法通常能够提供任何基准下的物理性质值，即应能够将存储基准下的值转换为请求的基准。此操作并非总是可行。
    /// 例如，如果一个或多个化合物的分子量未知，则无法将质量分数或质量流量转换为摩尔分数或摩尔流量。</para></remarks>
    /// <param name="property">CapeString 请求值的物理属性的标识符。此标识符必须是单相物理属性或其导数之一。
    /// 标准标识符在第 7.5.5 节和第 7.6 节中列出。</param>
    /// <param name="phaseLabel">CapeString 物理属性所需的相的相标签。相标签必须是本接口的
    /// GetPresentPhases 方法返回的标识符之一。</param>
    /// <param name="basis">CapeString 结果的基础。有效设置为：“Mass”用于每单位质量的物理性质，或“Mole”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用“UNDEFINED”作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <param name="results">CapeVariant 结果向量（CapeArrayDouble）包含以国际单位制（SI）为单位的物理属性值，
    /// 或 CapeInterface（见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，
    /// 但该操作“并未”实现。也就是说，该操作虽然存在，但当前实现中并未支持该操作。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需属性无法从物流对象中获取，可能是由于所请求的相态标签或基础
    /// 属性不存在。当调用 CreateMaterial 方法后未设置属性值，或通过调用 ClearAllProps 方法清除了属性值时，将触发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用：例如，属性值为 UNDEFINED，
    /// 或 phaseLabel 的标识符无法识别。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化操作未执行或执行失败。
    /// 如果指定的相态不存在，则会抛出此异常。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.GetSinglePhaseProp(string property, string phaseLabel, string basis, ref double[] results)
    {
        object obj1 = null;
        _pIMatObj.GetSinglePhaseProp(property, phaseLabel, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>获取相的温度、压力和组成。</summary>
    /// <remarks><para>该方法旨在帮助开发人员更高效地利用 CAPE-OPEN 接口。它通过单次调用即可返回物流对象中最常被请求的信息。</para>
    /// <para>该方法不提供基准选择。组成始终以摩尔分数形式返回。</para>
    /// <para>要获取混合物整体的等效信息，应使用 ICapeThermoMaterial 接口中的 GetOverallTPFraction 方法。</para></remarks>
    /// <returns>无返回值。</returns>
    /// <param name="phaseLabel">所需属性的相态标签。相态标签必须是本接口的 GetPresentPhases 方法返回的标识符之一。</param>
    /// <param name="temperature">温度 (in K)</param>
    /// <param name="pressure">压力 (in Pa)</param>
    /// <param name="composition">组成 (mole fractions)</param>
    /// <exception cref="ECapeNoImpl">操作 GetTPFraction 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作虽然存在，但当前实现中并未提供支持。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">其中一个属性无法从物流对象中获取。当调用 CreateMaterial
    /// 方法后属性值未被设置，或通过调用 ClearAllProps 方法清除了属性值时，将引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用：例如，属性值为 UNDEFINED，
    /// 或 phaseLabel 的标识符无法识别。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。必要的初始化操作未执行或执行失败。
    /// 如果指定的相态不存在，则会抛出此异常。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.GetTPFraction(string phaseLabel, ref double temperature, ref double pressure,
        ref double[] composition)
    {
        object obj1 = null;
        _pIMatObj.GetTPFraction(phaseLabel, temperature, pressure, ref obj1);
        composition = (double[])obj1;
    }

    /// <summary>获取混合物中两相非恒定物理性质的数值。</summary>
    /// <remarks><para>GetTwoPhaseProp 方法返回的结果参数可能是包含一个或多个数值（例如 k 值）的 CapeArrayDouble 类型，
    /// 也可能是用于获取由更复杂数据结构描述的两相物理性质（例如分布式物理性质）的 CapeInterface 类型。</para>
    /// <para>尽管某些对 GetTwoPhaseProp 的调用可能返回单个数值，但数值的返回类型为 CapeArrayDouble，
    /// 因此在这种情况下，该方法必须返回一个数组，即使该数组仅包含一个元素。</para>
    /// <para>如果某个相的标识符被 GetPresentPhases 方法返回，则该相在物流中“存在”。如果 GetTwoPhaseProp 方法指定的
    /// 任何相不存在，则会引发异常。即使所有相都存在，这并不意味着任何物理属性可用。</para>
    /// <para>GetTwoPhaseProp 方法返回的物理属性值取决于两个相，例如表面张力或 K 值。这些值可以通过直接调用的
    /// SetTwoPhaseProp 方法设置，也可以通过其他方法设置，例如 ICapeThermoPropertyRoutine 接口的
    /// CalcTwoPhaseProp 方法，或 ICapeThermoEquilibriumRoutine 接口的 CalcEquilibrium 方法。
    /// 注意：依赖于单一相的物理属性由 GetSinglePhaseProp 方法返回。</para>
    /// <para>预计该方法通常能够提供任何基准下的物理性质值，即应能够将存储基准下的值转换为请求的基准。此操作并非总是可行。
    /// 例如，如果一个或多个化合物的分子量未知，则无法在质量基准和摩尔基准之间进行转换。</para>
    /// <para>如果请求了组成导数，这意味着导数将按相标签指定的顺序分别返回给两个相。组成导数的返回值数量取决于该属性的维度。
    /// 例如，如果存在 N 个化合物，则表面张力导数的結果向量将包含第一相态的 N 个组成导数值，随后是第二相态的 N 个组成导数值。
    /// 对于 K 值导数，将包含第一相态的 N² 个导数值，随后是第二相态的 N² 个值，顺序如 7.6.2 节所定义。</para></remarks>
    /// <param name="property">请求值的属性标识符。此标识符必须是第 7.5.6 节和
    /// 第 7.6 节中列出的两相态物理属性或物理属性衍生项之一。</param>
    /// <param name="phaseLabels">需要该属性的相态的相态标签列表。
    /// 相态标签必须是物流对象的 GetPhaseList 方法返回的标识符中的两个。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用“UNDEFINED”作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <param name="results">结果向量（CapeArrayDouble）包含以国际单位制（SI）为单位的属性值，
    /// 或 CapeInterface（参见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑可以调用此方法，该操作仍“未”实现。
    /// 也就是说，该操作存在，但当前实现不支持该操作。如果 PME 不需要两相非恒定物理属性，则可能无需实现此方法。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的属性可能无法从物流对象中获取，
    /// 可能是由于所请求的相态或基础设置。</exception>
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。当调用 SetTwoPhaseProp 方法时，如果该方法未被调用、
    /// 调用失败，或者所引用的一个或多个相态不存在，则会引发此异常。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用：例如，属性为 UNDEFINED，
    /// 或 phaseLabels 中包含未识别的标识符。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.GetTwoPhaseProp(string property, string[] phaseLabels, 
        string basis, ref double[] results)
    {
        object obj1 = null;
        _pIMatObj.GetTwoPhaseProp(property, phaseLabels, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>为整个混合物设置非恒定的属性值。</summary>
    /// <remarks><para>SetOverallProp 方法设置的属性值指的是整体混合物的属性。这些属性值可通过调用
    /// GetOverallProp 方法获取。整体混合物的属性值并非由实现 ICapeThermoMaterial 接口的组件计算得出。
    /// 这些属性值仅作为实现 ICapeThermoEquilibriumRoutine 接口的组件中 CalcEquilibrium 方法的输入参数使用。</para>
    /// <para>尽管通过调用 SetOverallProp 设置的一些属性将具有单一值，但参数值的类型为 CapeArrayDouble，
    /// 且该方法必须始终以数组形式调用，即使该数组仅包含一个元素。</para></remarks>
    /// <param name="property">CapeString 用于设置值的属性标识符。此标识符必须是可用于整体混合物的单相属性或其衍生属性之一。
    /// 标准标识符在第 7.5.5 节和第 7.6 节中列出。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用“UNDEFINED”作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <param name="values">为该属性设置的值。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。如果 PME 不处理任何单相属性，则可能不需要该方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即该值不属于上述有效值列表，
    /// 例如属性为 UNDEFINED 时。</exception>
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不适用时，将引发此错误。</exception>
    void ICapeThermoMaterial.SetOverallProp(string property, string basis, double[] values)
    {
        _pIMatObj.SetOverallProp(property, basis, values);
    }

    /// <summary>允许 PME 或属性包指定当前存在的相态列表。</summary>
    /// <remarks><para>SetPresentPhases 可能用于：</para>
    /// <para>1. 将平衡计算（使用实现 ICapeThermoEquilibriumRoutine 接口的组件的 CalcEquilibrium 方法）
    /// 限制为物性包组件支持的相的子集；</para>
    /// <para>2. 当实现 ICapeThermoEquilibriumRoutine 接口的组件需要指定在平衡计算完成后，物流对象中包含哪些相时。</para>
    /// <para>如果列表中已存在某个相位，则该相位的物理属性不会因本方法的操作而发生变化。当调用 SetPresentPhases 方法时，
    /// 列表中不存在的任何相位将从物流对象中移除。这意味着存储在被移除相位上的任何物理属性值将不再可用
    /// （即，调用 GetSinglePhaseProp 或 GetTwoPhaseProp 方法时包含该相位将引发异常）。调用物流对象的
    /// GetPresentPhases 方法将返回与 SetPresentPhases 指定的相同列表。</para>
    /// <para>phaseStatus 参数必须包含与 Phase 标签数量相同的条目。有效设置如下表所示：</para>
    /// <para>Cape_UnknownPhaseStatus - 这是在指定相位可用于平衡计算时的默认设置。</para>
    /// <para>Cape_AtEquilibrium - 该相态已通过平衡计算确定为当前相态。</para>
    /// <para>Cape_Estimates - 平衡状态的估计值已设置在物流对象中。</para>
    /// <para>所有状态为 Cape_AtEquilibrium 的相必须具有与平衡状态对应的属性，即各化合物的温度、压力和逸度系数相等
    /// （这并不意味着逸度系数是通过平衡计算设置的）。Cape_AtEquilibrium 状态应由实现 ICapeThermoEquilibriumRoutine
    /// 接口的组件通过其 CalcEquilibrium 方法在平衡计算成功后设置。若平衡相的温度、压力或组成发生变化，物流对象实现应负责
    /// 将该相的状态重置为 Cape_UnknownPhaseStatus。该相存储的其他属性值不应受到影响。</para>
    /// <para>具有“估计”状态的相态必须在“物流对象”中设置温度、压力、成分和相分数值。这些值可供平衡计算器组件用于初始化平衡计算。
    /// 存储的值是可用的，但不能保证它们会被使用。</para></remarks>
    /// <param name="phaseLabels">CapeArrayString 当前存在的相的相标签列表。物流对象中的相标签必须是 ICapeThermoPhases
    /// 接口的 GetPhaseList 方法返回的标签的子集。</param>
    /// <param name="phaseStatus">与每个相位标签对应的相位状态标志数组。请参见下文描述。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即该值不属于上述有效值列表，
    /// 例如当 phaseLabels 包含 UNDEFINED 或 phaseStatus 包含不在上述表格中的值时。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoMaterial.SetPresentPhases(string[] phaseLabels, CapePhaseStatus[] phaseStatus)
    {
        var obj1 = new int[phaseStatus.Length];
        for (var i = 0; i < obj1.Length; i++)
        {
            if (phaseStatus[i] == CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS) obj1[i] = 0;
            if (phaseStatus[i] == CapePhaseStatus.CAPE_ATEQUILIBRIUM) obj1[i] = 1;
            if (phaseStatus[i] == CapePhaseStatus.CAPE_ESTIMATES) obj1[i] = 2;
        }
        _pIMatObj.SetPresentPhases(phaseLabels, obj1);
    }

    /// <summary>为混合物设置单相非恒定属性值。</summary>
    /// <remarks><para>SetSinglePhaseProp 函数的 values 参数可以是包含一个或多个要设置的属性数值的 CapeArrayDouble
    /// 类型，例如温度，也可以是用于设置由更复杂数据结构描述的单相属性的 CapeInterface 类型，例如分布式属性。</para>
    /// <para>尽管通过调用 SetSinglePhaseProp 设置的一些属性将具有单个数值，但数值参数的类型为 CapeArrayDouble。
    /// 在这种情况下，即使数值数组仅包含一个元素，也必须以包含数组的形式调用该方法。</para>
    /// <para>SetSinglePhaseProp 方法设置的属性值仅适用于单一相。依赖于多个相的属性（例如表面张力或 K 值）
    /// 则通过物流对象的 SetTwoPhaseProp 方法进行设置。</para>
    /// <para>在使用 SetSinglePhaseProp 之前，必须先使用 SetPresentPhases 方法创建所引用的相位。</para></remarks>
    /// <param name="prop">设置值的属性的标识符。这必须是单相属性或其衍生属性之一。
    /// 标准标识符在第 7.5.5 节和第 7.6 节中列出。</param>
    /// <param name="phaseLabel">设置该属性的相态的相态标签。
    /// 相态标签必须是本接口的 GetPresentPhases 方法返回的字符串之一。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用“UNDEFINED”作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <param name="values">用于设置该属性的值（CapeArrayDouble）或 CapeInterface（请参见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑可以调用此方法，该操作仍“未”实现。
    /// 也就是说，该操作虽然存在，但当前实现不支持该操作。如果 PME 不处理任何单相属性，则可能不需要此方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即该值不属于上述有效值列表，
    /// 例如属性为 UNDEFINED 时。</exception> 
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception> 
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。所引用的相态未通过 SetPresentPhases 方法创建。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不适用时，将引发此错误。</exception>
    void ICapeThermoMaterial.SetSinglePhaseProp(string prop, string phaseLabel, string basis, double[] values)
    {
        _pIMatObj.SetSinglePhaseProp(prop, phaseLabel, basis, values);
    }

    /// <summary>为混合物设置两相非恒定属性值。</summary>
    /// <remarks><para>SetTwoPhaseProp 函数的 values 参数可以是包含一个或多个要设置的属性数值的 CapeArrayDouble
    /// 对象，例如 k 值，也可以是用于设置由更复杂数据结构描述的两相属性的 CapeInterface 对象，例如分布式属性。</para>
    /// <para>尽管通过调用 SetTwoPhaseProp 设置的一些属性将具有单个数值，但数值类型的值参数类型为 CapeArrayDouble。
    /// 在这种情况下，即使值参数仅包含一个元素，也必须以包含数组的形式调用该方法。</para>
    /// <para>SetTwoPhaseProp 方法设置的物理属性值取决于两个相，例如表面张力或 K 值。
    /// 取决于单一相的属性则通过 SetSinglePhaseProp 方法进行设置。</para>
    /// <para>如果指定了具有组分导数的物理属性，则导数值将按相标签的指定顺序为两个相分别设置。组成导数的返回值数量取决于属性。
    /// 例如，如果存在 N 种化合物，则表面张力导数的值向量将包含第一相的 N 个组成导数值，随后是第二相的 N 个组成导数值。
    /// 对于 K 值，将包含第一相的 N² 个导数值， 随后是第二相的 N² 个值，顺序如 7.6.2 节所定义。</para>
    /// <para>在使用 SetTwoPhaseProp 之前，所有引用的相态都必须通过 SetPresentPhases 方法创建。</para></remarks>
    /// <param name="property">在物流对象中设置值的属性。该属性必须是第 7.5.6 节和第 7.6 节中包含的两相属性或其衍生属性之一。</param>
    /// <param name="phaseLabels">设置该属性的相的相标签。
    /// 相标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的两个标识符。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用“UNDEFINED”作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <param name="values">为该属性设置的值（CapeArrayDouble）或 CapeInterface（参见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑可以调用此方法，该操作仍“未”实现。
    /// 也就是说，该操作虽然存在，但当前实现不支持该操作。如果 PME 不处理任何单相属性，则可能不需要此方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即该值不属于上述有效值列表，例如属性为 UNDEFINED 时。</exception> 
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception> 
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。所引用的相态未通过 SetPresentPhases 方法创建。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不适用时，将引发此错误。</exception>
    void ICapeThermoMaterial.SetTwoPhaseProp(string property, string[] phaseLabels, string basis, double[] values)
    {
        _pIMatObj.SetTwoPhaseProp(property, phaseLabels, basis, values);
    }

    /// <summary>为混合物设置两相非恒定属性值。</summary>
    /// <remarks><para>SetTwoPhaseProp 函数的 values 参数可以是包含一个或多个要设置的属性数值的 CapeArrayDouble 对象，
    /// 例如 k 值，也可以是用于设置由更复杂数据结构描述的两相属性的 CapeInterface 对象，例如分布式属性。</para>
    /// <para>尽管通过调用 SetTwoPhaseProp 设置的一些属性将具有单个数值，但数值类型的值参数类型为 CapeArrayDouble。
    /// 在这种情况下，即使值参数仅包含一个元素，也必须以包含数组的形式调用该方法。</para>
    /// <para>SetTwoPhaseProp 方法设置的物理属性值取决于两个相，例如表面张力或 K 值。
    /// 取决于单一相的属性则通过 SetSinglePhaseProp 方法进行设置。</para>
    /// <para>如果指定了具有组分导数的物理属性，则导数值将按相标签的指定顺序为两个相分别设置。组成导数的返回值数量取决于属性。
    /// 例如，如果存在 N 种化合物，则表面张力导数的值向量将包含第一相的 N 个组成导数值，随后是第二相的 N 个组成导数值。
    /// 对于 K 值，将包含第一相的 N² 个导数值， 随后是第二相的 N² 个值，顺序如 7.6.2 节所定义。</para>
    /// <para>在使用 SetTwoPhaseProp 之前，所有引用的相态都必须通过 SetPresentPhases 方法创建。</para>
    /// <para>SetTwoPhaseProp 函数的 values 参数可以是包含一个或多个要设置的属性数值的 CapeArrayDouble 对象，例如 k 值，
    /// 也可以是用于设置由更复杂数据结构描述的两相属性的 CapeInterested 对象。</para></remarks>
    /// <param name="property">在物流对象中设置值的属性。该属性必须是第 7.5.6 节和第 7.6 节中包含的两相属性或其衍生属性之一。</param>
    /// <param name="phaseLabels">设置该属性的相的相标签。
    /// 相标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的两个标识符。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用“UNDEFINED”作为占位符。详情请参阅第7.5.5节。</param>
    /// <param name="values">为属性设置的值（CapeArrayDouble）或 CapeInterface（参见备注）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑可以调用此方法，该操作仍“未”实现。
    /// 也就是说，该操作虽然存在，但当前实现不支持该操作。如果 PME 不处理任何单相属性，则可能不需要此方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，即该值不属于上述有效值列表，例如属性为 UNDEFINED 时。</exception> 
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception> 
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。所引用的相态未通过 SetPresentPhases 方法创建。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不适用时，将引发此错误。</exception>
    [Description("Method SetTwoPhaseProp")]
    void ICapeThermoMaterial.SetTwoPhaseProp(string property, string[] phaseLabels, string basis, object values)
    {
        _pIMatObj.SetTwoPhaseProp(property, phaseLabels, basis, values);
    }

    /// <summary>获取一个通用常量的值。</summary>
    /// <param name="constantId">通用常量标识符。支持的常量列表应通过调用 GetUniversalConstList 方法获取。</param>
    /// <returns>普适常数的值。该值可以是数值或字符串。对于数值，其计量单位在第 7.5.1 节中规定。</returns>
    /// <remarks>普适常数（常被称为基本常数）是一类量，例如气体常数或阿伏加德罗常数。</remarks>
    /// <exception cref="ECapeNoImpl">操作 GetUniversalConstant 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作确实存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeInvalidArgument">例如，使用了 UNDEFINED 作为 constantId 参数的值，
    /// 或者 constantId 参数的值不属于已识别的值列表。</exception>
    /// <exception cref="ECapeUnknown">当为 GetUniversalConstant 操作指定的其他错误不适用时，应引发的错误。</exception>	
    object ICapeThermoUniversalConstant.GetUniversalConstant(string constantId)
    {
        return (double)_pIUniversalConstant.GetUniversalConstant(constantId);
    }

    /// <summary>返回支持的通用常量的标识符。</summary>
    /// <returns>通用常量的标识符列表。标准标识符的列表见第 7.5.1 节。</returns>
    /// <remarks>组件可能返回不属于第 7.5.1 节中定义的列表的通用常量标识符。
    /// 然而，这些专有标识符可能无法被该组件的大多数客户端理解。</remarks>
    /// <exception cref="ECapeNoImpl">操作 GetUniversalConstantList 尚未实现，即使出于与 CAPE-OPEN 标准兼容性
    /// 的考虑，该方法仍可被调用。也就是说，该操作确实存在，但当前实现不支持它。这种情况可能发生在属性包不支持任何通用常量，
    /// 或者不希望为属性包内可能使用的任何通用常量提供值时。</exception>
    /// <exception cref="ECapeUnknown">当为 GetUniversalConstantList 操作指定的其他错误不适用时，应引发的错误。</exception>
    string[] ICapeThermoUniversalConstant.GetUniversalConstantList()
    {
        return (string[])_pIUniversalConstant.GetUniversalConstantList();
    }

    /// <summary>返回相数。</summary>
    /// <returns>支持的相态数量。</returns>
    /// <remarks>此方法返回的相位数必须等于此接口的 GetPhaseList 方法返回的各相位标签数。它必须是零或正数。</remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    int ICapeThermoPhases.GetNumPhases()
    {
        return _pIPhases.GetNumPhases();
    }

    /// <summary>返回与某个相态相关的属性信息，以便理解该相态标签背后的含义。</summary>
    /// <param name="phaseLabel">单相标签。此标签必须是 GetPhaseList 方法返回的值之一。</param>
    /// <param name="phaseAttribute">下表中的一个相位属性标识符。</param>
    /// <returns>与相态属性标识符对应的值 – 参见下表。</returns>
    /// <remarks><para>GetPhaseInfo 旨在允许 PME 或其他客户端识别具有任意标签的相态。PME 或其他客户端需要执行此操作，
    /// 以将流数据映射到物流对象，或在导入属性包时。如果客户端无法识别相态，它可以要求用户根据这些属性的值提供映射。</para>
    /// <para>支持的相态属性列表如下表所示：</para>
    /// <para>下表定义了支持的相态属性列表，例如，支持气相、有机液相和水相的属性包组件可能会返回以下信息：</para>
    /// <para>Phase label, Gas, Organic, Aqueous</para>
    /// <para>StateOfAggregation, Vapor, Liquid, Liquid</para>
    /// <para>KeyCompoundId, UNDEFINED, UNDEFINED, Water</para>
    /// <para>ExcludedCompoundId, UNDEFINED, Water, UNDEFINED</para>
    /// <para>DensityDescription, UNDEFINED, Light, Heavy</para>
    /// <para>UserDescription, The gas Phase, The organic liquid Phase, The aqueous liquid Phase</para>
    /// <para>TypeOfSolid, UNDEFINED, UNDEFINED, UNDEFINED</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作虽然存在，但当前实现中并未支持该操作。.</exception>
    /// <exception cref="ECapeInvalidArgument">相位标签未被识别，或为未定义，或相位属性未被识别。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。.</exception>
    string[] ICapeThermoPhases.GetPhaseInfo(string phaseLabel, string phaseAttribute)
    {
        return (string[])_pIPhases.GetPhaseInfo(phaseLabel, phaseAttribute);
    }

    /// <summary>返回所有支持的相态的相态标签及其他重要描述性信息。</summary>
    /// <param name="phaseLabels">支持的相态的相态标签列表。相态标签可以是任何字符串，但每个相态必须具有唯一的标签。
    /// 如果由于某种原因不支持任何相态，则应为 phaseLabels 返回 UNDEFINED 值。
    /// 相态标签的数量也必须等于 GetNumPhases 方法返回的相态数量。</param>
    /// <param name="stateOfAggregation">与每个相态相关的物理聚集状态。该值必须为以下字符串之一：
    /// “气态”、“液态”、“固态”或“未知”。每个相态必须仅有一个聚集状态。该值不得留空，但可设置为“未知”。</param>
    /// <param name="keyCompoundId">该相的关键化合物。此处必须为化合物标识符（由 GetCompoundList 函数返回），
    /// 否则可能为未定义值，此时将返回UNDEFINED值。关键化合物指的是在该相中预期以高浓度存在的化合物，
    /// 例如水在水相中。每个相只能有一个关键化合物。</param>
    /// <remarks><para>相标签允许在 ICapeThermoPhases 接口和其他 CAPE-OPEN 接口的方法中唯一标识相。
    /// 聚集状态和关键化合物为 PME 或其他客户端提供了一种方式，使其能够根据相的物理特性来解释相标签的含义。</para>
    /// <para>此方法返回的所有数组必须具有相同的长度，即等于相位标签的数量。</para>
    /// <para>要获取有关某个相态的更多信息，请使用 GetPhaseInfo 方法。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    void ICapeThermoPhases.GetPhaseList(ref string[] phaseLabels, ref string[] stateOfAggregation,
        ref string[] keyCompoundId)
    {
        object obj1 = null;
        object obj2 = null;
        object obj3 = null;
        _pIPhases.GetPhaseList(ref obj1, ref obj2, ref obj3);
        phaseLabels = (string[])obj1;
        stateOfAggregation = (string[])obj2;
        keyCompoundId = (string[])obj3;
    }

    /// <summary>返回指定化合物对应的常数物理性质的值。</summary>
    /// <remarks><para>GetConstPropList 方法可用于检查哪些物理属性常量可用。</para>
    /// <para>如果请求的物理性质数量为 P，化合物数量为 C，则 propVals 数组将包含 C*P 个变体。前 C 个变体将是
    /// 第一个请求的物理性质的值（每个化合物一个变体），接着是第二个物理性质的 C 个常量值，依此类推。实际返回的值类型
    /// （双精度浮点数、字符串等）取决于物理性质，具体请参见第 7.5.2 节的说明。</para>
    /// <para>物理性质以固定的单位集形式返回，具体单位集如第 7.5.2 节所规定。</para>
    /// <para>如果 compIds 参数设置为 UNDEFINED，则表示请求返回实现 ICapeThermoCompounds 接口的组件中所有
    /// 化合物的属性值，且化合物的顺序与 GetCompoundList 方法返回的顺序相同。例如，如果该接口由一个属性包组件实现，
    /// 则将 compIds 设置为 UNDEFINED 的属性请求表示属性包中的所有化合物，而非传递给属性包的物流对象中的所有化合物。</para>
    /// <para>如果某个物理属性对一个或多个化合物不可用，则必须为这些组合返回未定义的值，
    /// 并抛出 ECapeThrmPropertyNotAvailable 异常。如果抛出异常，客户端应检查所有返回的值以确定哪个值未定义。</para></remarks>
    /// <param name="props">物理性质标识符列表。常量物理性质的有效标识符在第 7.5.2 节中列出。</param>
    /// <param name="compIds">用于检索常量的化合物标识符列表。将 compIds 设置为 UNDEFINED 以
    /// 表示实现 ICapeThermoCompounds 接口的组件中的所有化合物。</param>
    /// <returns>指定化合物的常数值。</returns>
    /// <exception cref="ECapeNoImpl">操作 GetCompoundConstant 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作确实存在，但当前实现不支持它。如果不支持任何化合物或属性，应抛出此异常。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">物理性质列表中至少有一项属性对于特定化合物不可用。
    /// 此异常应被视为警告而非错误。</exception>
    /// <exception cref="ECapeLimitedImpl">该接口的实现组件不支持一个或多个物理属性。如果 props 参数中的任何元素未被识别，
    /// 也应抛出此异常，因为第 7.5.2 节中列出的物理属性列表并非旨在详尽无遗，且未识别的物理属性标识符可能有效。如果完全不支持
    /// 任何物理属性，应抛出 ECapeNoImpl 异常（参见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如，未识别的复合标识符或 props 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当操作中指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetCompoundConstant method. The error would not be raised when the 
    /// GetCompoundConstant method is implement ed by a Material Object.</exception>
    object[] ICapeThermoCompounds.GetCompoundConstant(string[] props, string[] compIds)
    {
        return (object[])_pICompounds.GetCompoundConstant(props, compIds);
    }

    /// <summary>返回所有化合物的列表。这包括已识别的化合物标识符以及可用于进一步识别化合物的额外信息。</summary>
    /// <remarks><para>如果任何项目无法返回，则其值应设置为 UNDEFINED。相同的信息也可通过 GetCompoundConstant
    /// 方法提取。GetCompoundList 参数与 Compound 常量物理属性之间的对应关系，如第 7.5.2 节所规定，如下所示：</para>
    /// <para>compIds - 不存在等价关系。compIds 是一个由实现 GetCompoundList 方法的组件分配的标识符。
    /// 该字符串通常包含一个唯一的化合物标识符，例如“苯”。
    /// 它必须用于 ICapeThermoCompounds 和 ICapeThermoMaterial 接口中所有名为“compIds”的参数中。</para>
    /// <para>Formulae - chemicalFormula</para>
    /// <para>names - iupacName</para>
    /// <para>boilTemps - normalBoilingPoint</para>
    /// <para>molwts - molecularWeight</para>
    /// <para>casnos casRegistryNumber</para>
    /// <para>当 ICapeThermoCompounds 接口由物流对象实现时，返回的化合物列表在配置物流对象时是固定的。</para>
    /// <para>对于一个属性包组件，属性包通常会包含为特定应用程序选定的有限数量的化合物，而不是所有可能的化合物，
    /// 这些化合物可能适用于专有属性系统。</para>
    /// <para>为了识别属性包中的化合物，PME 或其他客户端将使用 casnos 参数而非 compIds。这是因为不同 PME 可能为
    /// 同一化合物赋予不同名称，而 casnos 参数（几乎总是）是唯一的。如果 casnos 不可用（例如石油馏分），或不唯一，
    /// 则 GetCompoundList 返回的其他信息可用于区分化合物。然而，需要注意的是，与属性包通信时，客户必须使用
    /// compIds 参数中返回的化合物标识符。</para></remarks>
    /// <param name="compIds">List of Compound identifiers</param>
    /// <param name="formulae">List of Compound formulae</param>
    /// <param name="names">List of Compound names.</param>
    /// <param name="boilTemps">List of boiling point temperatures.</param>
    /// <param name="molwts">List of molecular weights.</param>
    /// <param name="casnos">List of Chemical Abstract Service (CAS) Registry numbers.</param>
    /// <exception cref="ECapeNoImpl">操作 GetCompoundList 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作确实存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeUnknown">当为 GetCompoundList 操作指定的其他错误不适用时，应引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetCompoundList 方法之前先调用 SetMaterial 方法，
    /// 则会引发此错误。当 GetCompoundList 方法由物流对象实现时，此错误不会被引发。</exception>
    void ICapeThermoCompounds.GetCompoundList(ref string[] compIds, ref string[] formulae, ref string[] names,
        ref double[] boilTemps, ref double[] molwts, ref string[] casnos)
    {
        object obj1 = null;
        object obj2 = null;
        object obj3 = null;
        object obj4 = null;
        object obj5 = null;
        object obj6 = null;
        _pICompounds.GetCompoundList(ref obj1, ref obj2, ref obj3, 
            ref obj4, ref obj5, ref obj6);
        compIds = (string[])obj1;
        formulae = (string[])obj2;
        names = (string[])obj3;
        boilTemps = (double[])obj4;
        molwts = (double[])obj5;
        casnos = (string[])obj6;
    }

    /// <summary>返回支持的物理属性常量列表。</summary>
    /// <returns>所有支持的物理常量属性标识符列表。标准常量属性标识符在第 7.5.2 节中列出。</returns>
    /// <remarks><para>MGetConstPropList 方法返回所有可通过 GetCompoundConstant 方法获取的常量物理属性的标识符。
    /// 如果不支持任何属性，应返回 UNDEFINED。CAPE-OPEN 标准未定义实现 ICapeThermoCompounds 接口的
    /// 软件组件必须提供的物理属性最小列表。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可能会返回不属于第 7.5.2 节中定义的列表的常量物理属性标识符。</para>
    /// <para>然而，这些专有标识符可能无法被该组件的大多数客户端理解。</para></remarks>
    /// <exception cref="ECapeNoImpl">操作 GetConstPropList 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作确实存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeUnknown">当为 Get-ConstPropList 操作指定的其他错误不适用时，将引发此错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetConstPropList 方法之前先调用 SetMaterial 方法，
    /// 则会引发此错误。当 GetConstPropList 方法由物流对象实现时，此错误不会被引发。</exception>
    string[] ICapeThermoCompounds.GetConstPropList()
    {
        return (string[])_pICompounds.GetConstPropList();
    }

    /// <summary>返回支持的化合物数量。</summary>
    /// <returns>支持的化合物数量。</returns>
    /// <remarks>此方法返回的化合物数量必须等于此接口的 GetCompoundList 方法返回的化合物标识符数量。它必须是零或正数。</remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetNumCompounds 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。如果 GetNumCompounds 方法是由材质对象实现的，则不会出现该错误。</exception>
    int ICapeThermoCompounds.GetNumCompounds()
    {
        return _pICompounds.GetNumCompounds();
    }

    /// <summary>返回指定纯化合物随压力变化的物理性质值。</summary>
    /// <param name="props">物理性质标识符列表。与压力有关的物理性质的有效标识符列于第 7.5.4 节。</param>
    /// <param name="pressure">评估物理性质时的压力（单位 Pa）。</param>
    /// <param name="compIds">要检索其物理性质的化合物标识符列表。设置 compIds = UNDEFINED 表示组件中
    /// 实现 ICapeThermoCompounds 接口的所有化合物。</param>
    /// <param name="propVals">指定化合物的属性值。</param>
    /// <remarks><para>可以使用 GetPDependentPropList 方法来检查哪些物理属性可用。</para>
    /// <para>如果请求的物理属性数为 P，化合物数为 C，则 propVals 数组将包含 C*P 值。第一个 C 将是第一个物理属性的值，
    /// 然后是第二个物理属性的 C 值，以此类推。</para>
    /// <para>根据第 7.5.4 节的规定，物理性质将以一组固定的单位返回。</para>
    /// <para>如果 compIds 参数设置为 UNDEFINED，则请求返回实现 ICapeThermoCompounds 接口的组件中所有化合物的属性值，
    /// 化合物顺序与 GetCompoundList 方法返回的顺序相同。例如，如果属性包组件实现了该接口，将 compIds 设置为 UNDEFINED
    /// 的属性请求表示属性包中的所有化合物，而不是传递给属性包的物流对象中的所有化合物。</para>
    /// <para>如果一个或多个化合物的任何物理属性不可用，则必须返回这些组合的未定义值，并引发
    /// ECapeThrmPropertyNotAvailable 异常。如果出现异常，客户端应检查返回的所有值，以确定哪个值是未定义的。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。如果不支持化合物或物理性质，则应引发此异常。</exception>
    /// <exception cref="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。由于第 7.5.4 节中的物理属性列表
    /// 并非详尽无遗，未被识别的物理属性标识符可能是有效的，因此如果道具参数中的任何元素未被识别，
    /// 也应引发此异常（而不是 ECapeInvalidArgument）。如果不支持任何物理属性，则应引发 ECapeNoImpl（见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">用于传递无效参数值时，例如参数 props 的 UNDEFINED。</exception>
    /// <exception cref="ECapeOutOfBounds">压力值超出了属性包的接受范围。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">属性列表中至少有一项不能用于特定化合物。</exception>
    /// <exception cref="ECapeUnknown">当操作中指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetPDependentProperty 方法之前
    /// 调用 SetMaterial 方法，则会引发错误。如果 GetPDependentProperty 方法是由材质对象实现的，则不会出现该错误。</exception>
    void ICapeThermoCompounds.GetPDependentProperty(string[] props, double pressure, string[] compIds,
        ref double[] propVals)
    {
        object obj1 = null;
        _pICompounds.GetPDependentProperty(props, pressure, compIds, ref obj1);
        propVals = (double[])obj1;
    }

    ///<summary>返回支持的压力相关属性列表。</summary>
    ///<returns>所有支持的与压力有关的物理性质标识符列表。标准标识符列于第 7.5.4 节。</returns>
    /// <remarks><para>GetPDependentPropList 返回可通过 GetPDependentProperty 方法获取的所有压力相关属性的
    /// 标识符。如果不支持任何属性，则应返回 UNDEFINED。CAPE-OPEN 标准没有定义实现 ICapeThermoCompounds 接口的
    /// 软件组件必须提供的最小物理性质列表。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可返回不属于第 7.5.4 节所定义列表的标识符。
    /// 但是，该组件的大多数客户可能无法理解这些专有标识符。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当操作中指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetPDependentPropList 方法之前调用
    /// SetMaterial 方法，则会引发错误。如果 GetPDependentPropList 方法是由材质对象实现的，则不会出现该错误。</exception>
    string[] ICapeThermoCompounds.GetPDependentPropList()
    {
        return (string[])_pICompounds.GetPDependentPropList();
    }

    /// <summary>返回指定纯化合物随温度变化的物理性质值。</summary>
    /// <param name="props">物理性质标识符列表。与温度有关的物理性质的有效标识符列于第 7.5.3 节。</param>
    /// <param name="temperature">评估属性的温度（以 K 为单位）。</param>
    /// <param name="compIds">要检索其物理性质的化合物标识符列表。设置 compIds = UNDEFINED 表示组件中
    /// 实现 ICapeThermoCompounds 接口的所有化合物。</param>
    /// <param name="propVals">指定化合物的物理属性值。</param>
    /// <remarks><para>可以使用 GetTDependentPropList 方法来检查哪些物理属性可用。</para>
    /// <para>如果请求的物理属性数为 P，化合物数为 C，则 propVals 数组将包含 C*P 值。
    /// 第一个 C 将是第一个物理属性的值，然后是第二个物理属性的 C 值，以此类推。</para>
    /// <para>属性以 7.5.3 节规定的一组固定单位返回。</para>
    /// <para>如果 compIds 参数设置为 UNDEFINED，则请求返回实现 ICapeThermoCompounds 接口的组件中
    /// 所有化合物的属性值，化合物顺序与 GetCompoundList 方法返回的顺序相同。例如，如果属性包组件实现了
    /// 该接口，将 compIds 设置为 UNDEFINED 的属性请求表示属性包中的所有化合物，而不是传递给属性包的
    /// 物流对象中的所有化合物。</para>
    /// <para>如果一个或多个化合物的任何物理属性不可用，则必须返回这些组合的未定义值，并引发
    /// ECapeThrmPropertyNotAvailable 异常。如果出现异常，客户端应检查返回的所有值，以确定哪个值是未定义的。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，操作是存在的，但当前的实现不支持它。如果不支持 “化合物 ”或 “物理性质”，则应引发此异常。</exception>
    /// <exception cref="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。由于第 7.5.3 节中的
    /// 属性列表并非详尽无遗，未被识别的物理属性标识符可能是有效的，因此如果道具参数中的任何元素未被识别，
    /// 也应引发此异常（而不是 ECapeInvalidArgument）。如果不支持任何属性，则应引发 ECapeNoImpl（见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">用于传递无效参数值时，例如参数 props 的 UNDEFINED。</exception> 
    /// <exception cref="ECapeOutOfBounds">温度值超出了属性包的接受范围。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">属性列表中至少有一项不能用于特定化合物。</exception>
    /// <exception cref="ECapeUnknown">当操作中指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetTDependentProperty 方法之前调用
    /// SetMaterial 方法，则会引发错误。如果 GetTDependentProperty 方法是由材质对象实现的，则不会出现该错误。</exception>
    void ICapeThermoCompounds.GetTDependentProperty(string[] props, double temperature, string[] compIds,
        ref double[] propVals)
    {
        object obj1 = null;
        _pICompounds.GetTDependentProperty(props, temperature, compIds, ref obj1);
        propVals = (double[])obj1;
    }

    /// <summary>返回支持的随温度变化的物理属性列表。</summary>
    /// <returns>物理属性标识符列表，包含所有支持的温度相关属性。标准标识符列于第 7.5.3 节。</returns>
    /// <remarks><para>GetTDependentPropList 返回可通过 GetTDependentProperty 方法检索的所有与
    /// 温度相关的物理属性的标识符。如果不支持任何属性，则应返回 UNDEFINED。CAPE-OPEN 标准并没有定义实现
    /// ICapeThermoCompounds 接口的软件组件必须提供的最小属性列表。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可返回不属于第 7.5.3 节所定义列表的标识符。
    /// 但是，该组件的大多数客户可能无法理解这些专有标识符。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，
    /// 但该操作“并未”实现。也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetTDependentPropList 方法之前
    /// 调用 SetMaterial 方法，则会引发错误。如果 GetTDependentPropList 方法是由物流对象实现的，则不会出现该错误。</exception>
    string[] ICapeThermoCompounds.GetTDependentPropList()
    {
        return (string[])_pICompounds.GetTDependentPropList();
    }

    /// <summary>该方法用于计算单相混合物的逸度系数（可选其导数）的自然对数。
    /// 温度、压力和成分值在参数列表中指定，计算结果也通过参数列表返回。</summary>
    /// <param name="phaseLabel">要计算属性的相位的相位标签。相位标签必须是 ICapeThermoPhases
    /// 接口的 GetPhaseList 方法返回的字符串之一。</param>
    /// <param name="temperature">The temperature (K) for the calculation.</param>
    /// <param name="pressure">The pressure (Pa) for the calculation.</param>
    /// <param name="lnPhiDt">逸度系数随温度变化的自然对数的衍生物（如有要求）。</param>
    /// <param name="moleNumbers">混合物中每种化合物的摩尔数。</param>
    /// <param name="fFlags">表示是否应计算逸度系数和/或导数的自然对数的代码（见注释）。</param>
    /// <param name="lnPhi">逸度系数的自然对数（如有要求）。</param>
    /// <param name="lnPhiDp">与压力有关的逸度系数自然对数的衍生物（如有要求）。</param>
    /// <param name="lnPhiDn">与摩尔数有关的逸度系数自然对数的衍生物（如有要求）。</param>
    /// <remarks><para>提供这种方法是为了高效地计算和返回逸度系数的自然对数，逸度系数是最常用的热力学性质。</para>
    /// <para>用于计算的温度、压力和成分（摩尔数）由参数指定，不会通过单独请求从 “物流对象 ”中获取。同样，
    /// 任何计算量都通过参数返回，不会存储在物流对象中。调用此方法不会影响物流对象的状态。但应注意的是，在
    /// 调用 CalcAndGetLnPhi 之前，必须通过调用实现 ICapeThermoPropertyRoutine 接口的组件的
    /// ICapeThermoMaterialContext 接口上的 SetMaterial 方法，定义一个有效的物流对象。
    /// 物流对象中的化合物必须已被识别，moleNumbers 参数中提供的数值数必须等于物流对象中的化合物数。</para>
    /// <para>逸度系数信息以逸度系数的自然对数形式返回。这是因为热力学模型自然会提供该量的自然对数，
    /// 而且可以安全地返回更大范围的值。</para>
    /// <para>该方法实际计算和返回的数量由整数代码 fFlags 控制。该代码是通过使用下表所示的枚举常量
    /// eCapeCalculationCode（在 Thermo 1.1 版 IDL 中定义）对所需属性和每个导数的贡献值求和而形成的。
    /// 例如，要计算对数逸度系数及其 T 衍生系数，fFlags 参数应设置为
    /// CAPE_LOG_FUGACITY_COEFFICIENTS + CAPE_T_DERIVATIVE。</para>
    /// <table border="1">
    /// <tr><th>Calculation Type</th><th>Enumeration Value</th><th>Numerical Value</th></tr>
    /// <tr><td>no calculation</td><td>CAPE_NO_CALCULATION</td><td>0</td></tr>
    /// <tr><td>log fugacity coefficients</td><td>CAPE_LOG_FUGACITY_COEFFICIENTS</td><td>1</td></tr>
    /// <tr><td>T-derivative</td><td>CAPE_T_DERIVATIVE</td><td>2</td></tr>
    /// <tr><td>P-derivative</td><td>CAPE_P_DERIVATIVE</td><td>4</td></tr>
    /// <tr><td>mole number derivatives</td><td>CAPE_MOLE_NUMBERS_DERIVATIVES</td><td>8</td></tr>
    /// </table>	
    /// <para>如果调用 CalcAndGetLnPhi 时 fFlags 设置为 CAPE_NO_CALCULATION，则不会返回任何属性值。</para>
    /// <para>由属性包组件执行该方法时，典型的操作顺序是</para>
    /// <para>1. 检查指定的 phaseLabel 是否有效。</para>
    /// <para>2. 检查 moleNumbers 数组是否包含预期的数值（应与上次调用的 SetMaterial 方法一致）。</para>
    /// <para>3. 按参数列表中指定的 T/P/composition 计算所要求的属性/衍生物。</para>
    /// <para>4. 在相应参数中存储属性/衍生物的值。</para>
    /// <para>请注意，无论 “相态 ”是否实际存在于 “物流对象 ”中，都可以进行这种计算。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeLimitedImpl">如果由于未执行计算而无法返回所请求的一个或多个属性，则会引发该故障。</exception>
    /// <exception cref="ECapeBadInvOrder">在操作请求之前没有调用过必要的前提操作。例如，在调用此方法之前，
    /// ICapeThermoMaterial 接口未通过 SetMaterial 调用传递。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的前提条件无效。例如，相的组成未定义，
    /// 物流对象中的化合物数量为零或与摩尔数参数不一致，或没有任何 其他必要的输入信息。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所请求的属性中至少有一项无法返回。这可能是因为无法在
    /// 指定条件下或针对指定相态计算该属性。如果未执行属性计算，则应返回 ECapeLimitedImpl。</exception>
    /// <exception cref="ECapeSolvingError">某个属性计算失败。例如，模型中的某个迭代求解程序迭代次数耗尽，
    /// 或收敛到一个错误的解。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的值，
    /// 或 phaseLabel 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    void ICapeThermoPropertyRoutine.CalcAndGetLnPhi(string phaseLabel, double temperature,
        double pressure, double[] moleNumbers, CapeFugacityFlag fFlags,
        ref double[] lnPhi, ref double[] lnPhiDt, ref double[] lnPhiDp, ref double[] lnPhiDn)
    {
        object obj1 = null;
        object obj2 = null;
        object obj3 = null;
        object obj4 = null;
        var flags = (int)fFlags;
        _pIPropertyRoutine.CalcAndGetLnPhi(phaseLabel, temperature, pressure, moleNumbers, flags, 
            ref obj1, ref obj2, ref obj3, ref obj4);
        lnPhi = (double[])obj1;
        lnPhiDt = (double[])obj2;
        lnPhiDp = (double[])obj3;
        lnPhiDn = (double[])obj4;
    }

    /// <summary>CalcSinglePhaseProp 用于计算在物流对象中设置的温度、压力和成分的当前值下，单相混合物
    /// 的属性和属性导数。CalcSinglePhaseProp 不进行相平衡计算。</summary>
    /// <param name="props">要计算的单相属性或导数的标识符列表。标准标识符见第 7.5.5 和 7.6 节。</param>
    /// <param name="phaseLabel">要计算属性的相位的相位标签。相位标签必须是 ICapeThermoPhases
    /// 接口的 GetPhaseList 方法返回的字符串之一。</param>
    /// <remarks><para>CalcSinglePhaseProp 计算为单相定义的属性，如焓值或粘度。取决于多个相的物理性质，
    /// 如表面张力或 K 值，则由 CalcTwoPhaseProp 方法处理。</para>
    /// <para>执行此方法的组件必须从相关物流对象中获取计算的输入规格（温度、压力和成分），并在物流对象中设置计算结果。</para>
    /// <para>热力学和物理属性组件（如属性包或属性计算器）必须实现 ICapeThermoMaterialContext 接口，
    /// 以便通过 SetMaterial 方法传递 ICapeThermoMaterial 接口。</para>
    /// <para>在由属性包组件实现 CalcSinglePhaseProp 时，典型的操作序列是</para>
    /// <para>1. 检查指定的 phaseLabel 是否有效。</para>
    /// <para>2. 使用 GetTPFraction 方法（最后一次调用 SetMaterial 方法时指定的物流对象）
    /// 获取指定相的温度、压力和成分。</para>
    /// <para>3. 计算属性。</para>
    /// <para>4. 使用 ICapeThermoMaterial 接口的 SetSinglePhaseProp 方法，在物流对象中存储相的属性值。</para>
    /// <para>CalcSinglePhaseProp 将通过调用 GetSinglePhaseProp 从物流对象中获取所需的输入属性值。
    /// 如果请求的属性不可用，则会出现 ECapeThrmPropertyNotAvailable 异常。如果发生此错误，属性包可将其
    /// 返回给客户端，或请求其他属性。物流对象实现必须能够通过实现从一种基础到另一种基础的转换，
    /// 使用客户选择的基础提供属性值。</para>
    /// <para>客户不应假定物流对象中的相分数和化合物分数已归一化。如果分数没有归一化或超出预期范围，
    /// 则由属性包负责决定如何处理这种情况。</para>
    /// <para>建议一次申请一个属性，以简化错误处理。不过，我们也认识到，在某些情况下，同时请求多个
    /// 属性可能会提高效率。例如，需要一个属性及其导数。</para>
    /// <para>如果客户在一次调用中使用了多个属性，而其中一个属性失效，则整个调用应被视为失效。这意味着，
    /// 在知道整个请求可以满足之前，属性包不应向物流对象写回任何值。</para>
    /// <para>根据用于表示属性的数学/物理模型，相很可能在不存在相的温度、压力和成分条件下请求相的属性值。
    /// 这时可能会出现 ECapeThrmPropertyNotAvailable 异常或返回一个外推值。</para>
    /// <para>实施者有责任决定如何处理这种情况。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeLimitedImpl">如果由于（特定属性的）计算未实现而无法返回所请求的一个或多个属性，
    /// 则会引发该异常。如果道具参数未被识别，也应引发此异常（而不是 ECapeInvalidArgument），因为第 7.5.5 节
    /// 中的属性列表并非详尽无遗，未被识别的属性标识符可能是有效的。如果不支持任何属性，则应引发 ECapeNoImpl（见上文）。</exception>
    /// <exception cref="ECapeBadInvOrder">在操作请求之前没有调用过必要的前提操作。例如，在调用此方法之前，
    /// ICapeThermoMaterial 接口未通过 SetMaterial 调用传递。</exception> 
    /// <exception cref="ECapeFailedInitialisation">属性计算的前提条件无效。
    /// 例如，没有定义相的组成或没有其他必要的输入信息。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所请求的属性中至少有一项无法返回。
    /// 这可能是因为无法在指定条件下或指定相态计算该属性。如果未执行属性计算，则应返回 ECapeLimitedImpl。</exception>
    [DispId(0x00000002)]
    [Description("Method CalcSinglePhaseProp")]
    void ICapeThermoPropertyRoutine.CalcSinglePhaseProp(string[] props, string phaseLabel)
    {
        _pIPropertyRoutine.CalcSinglePhaseProp(props, phaseLabel);
    }

    /// <summary>CalcTwoPhaseProp 用于计算在当前温度值、压力值和 “物流对象 ”中设置的成分值下取决于
    /// 两相的 混合物属性和属性导数。它不执行平衡计算。</summary>
    /// <param name="props">待计算属性的标识符列表。必须是一个或多个受支持的两相属性和导数
    /// （由 GetTwoPhasePropList 方法给出）。两相属性的标准标识符见第 7.5.6 和 7.6 节。</param>
    /// <param name="phaseLabels">要计算属性的相位标签。相位标签必须是 ICapeThermoPhases 接口
    /// 的 GetPhaseList 方法返回的字符串中的两个。</param>
    /// <remarks><para>CalcTwoPhaseProp 可计算表面张力或 K 值等属性值。与单相有关的属性由
    /// ICapeThermoPropertyRoutine 接口的 CalcSinglePhaseProp 方法处理。执行此方法的组件必须从
    /// 相关物流对象中获取计算的输入规范（温度、压力和成分），并在物流对象中设置计算结果。</para>
    /// <para>属性包或属性计算器等组件必须实现 ICapeThermoMaterialContext 接口，以便通过
    /// SetMaterial 方法传递 ICapeThermoMaterial 接口。</para>
    /// <para>当 CalcTwoPhaseProp 由一个属性包组件实现时，其典型的操作序列为</para>
    /// <para>1. 检查指定的相位标签是否有效。</para>
    /// <para>2. 使用 GetTPFraction 方法（最后一次调用 SetMaterial 方法时指定的物流对象）
    /// 获取指定相的温度、压力和成分。</para>
    /// <para>3. 计算一些物性属性。</para>
    /// <para>4. 使用 ICapeThermoMaterial 接口的 SetTwoPhaseProp 方法，在物流对象中存储属性值。</para>
    /// <para>CalcTwoPhaseProp 将通过 GetTPFraction 或 GetSinglePhaseProp 调用从物流对象中
    /// 获取所需值。如果请求的属性不可用，则会出现 ECapeThrmPropertyNotAvailable 异常。
    /// 如果发生此错误，属性包可将其返回给客户端，或请求其他属性。物流对象实现必须能够通过实现从一种基础
    /// 到另一种基础的转换，使用客户选择的基础提供属性值。</para>
    /// <para>客户不应假定物流对象中的相分数和化合物分数已归一化。如果分数没有归一化或超出预期范围，
    /// 则由属性包负责决定如何处理这种情况。</para>
    /// <para>建议一次申请一个属性，以简化错误处理。不过，我们也认识到，在某些情况下，
    /// 同时请求多个属性可能会提高效率。例如，需要一个属性及其导数。</para>
    /// <para>如果客户端在一次调用中使用了多个属性，而其中一个属性失效，则整个调用应视为失效。
    /// 这意味着，在知道整个请求可以满足之前，属性包不应将任何值写回物流对象。</para>
    /// <para>CalcTwoPhaseProp 必须针对每个相分组组合单独调用。例如，汽液 K 值必须与液液 K 值分开调用计算。</para>
    /// <para>除非所有相的温度和压力完全相同，否则两相属性可能没有意义。
    /// 属性包有责任检查这些条件，并在适当时提出例外情况。</para>
    /// <para>根据用于表示属性的数学/物理模型，在温度、压力和成分条件下，可能会有一个或两个物相不存在时，
    /// PME 可能会请求这些物相的属性值。可能会出现 ECapeThrmPropertyNotAvailable 异常或返回一个外推值。
    /// 实现者有责任决定如何处理这种情况。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，
    /// 也 “未 ”执行该操作。也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeLimitedImpl">如果由于（特定属性的）计算未实现而无法返回所请求的
    /// 一个或多个属性，则会引发该异常。如果道具参数未被识别，也应引发此异常（而不是 ECapeInvalidArgument），
    /// 因为第 7.5.6 节中的属性列表并非详尽无遗，未被识别的属性标识符可能是有效的。如果不支持任何属性，
    /// 则应引发 ECapeNoImpl（见上文）。</exception>
    /// <exception cref="ECapeBadInvOrder">在操作请求之前没有调用过必要的前提操作。例如，在调用此方法之前，
    /// ICapeThermoMaterial 接口没有通过 SetMaterial 调用传递。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的前提条件无效。
    /// 例如，其中一个相态的组成未定义，或没有任何其他必要的输入信息。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所请求的属性中至少有一项无法返回。
    /// 这可能是因为无法在指定条件下或针对指定相态计算该属性。如果未执行属性计算，则应返回 ECapeLimitedImpl。</exception>
    /// <exception cref="ECapeSolvingError">某个属性计算失败。
    /// 例如，模型中的某个迭代求解程序迭代次数耗尽，或收敛到一个错误的解。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如，参数 phaseLabels 的值
    /// 无法识别或 UNDEFINED，或参数 props 的值 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    [DispId(0x00000003)]
    [Description("Method CalcTwoPhaseProp")]
    void ICapeThermoPropertyRoutine.CalcTwoPhaseProp(string[] props, string[] phaseLabels)
    {
        _pIPropertyRoutine.CalcTwoPhaseProp(props, phaseLabels);
    }

    /// <summary>检查是否可以使用 CalcSinglePhaseProp 方法计算给定相位的属性。</summary>
    /// <param name="property">要检查的属性的标识符。该标识必须是受支持的单相属性或衍生物之一
    /// （由 GetSinglePhasePropList 方法提供），方为有效。</param>
    /// <param name="phaseLabel">计算检查的相位标签。这必须是 ICapeThermoPhases
    /// 接口的 GetPhaseList 方法返回的标签之一。</param>
    /// <returns>一个布尔值，如果支持属性和相位标签的组合，则设置为 True；如果不支持，则设置为 False。</returns>
    /// <remarks><para>检查结果应仅取决于实现 ICapeThermoPropertyRoutine 接口的组件（如属性包）的
    /// 功能和配置（存在的化合物和物相）。它不应取决于物流对象是否已被设置，也不应取决于可能被设置的物流对象的
    /// 状态（温度、压力、成分等）或配置。</para>
    /// <para>在导入属性包时，PME 或其他客户端预计会使用此方法来检查属性包是否支持其所需的属性。
    /// 如果任何基本属性不可用，则应中止导入过程。</para>
    /// <para>如果实现 ICapeThermoPropertyRoutine 接口的组件无法识别属性或 phaseLabel
    /// 参数，则此方法应返回 False。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因，可以调用
    /// CheckSinglePhasePropSpec 方法，但该操作 “未 ”实现。也就是说，该操作是存在的，但当前的实现并不支持。</exception>
    /// <exception cref="ECapeBadInvOrder">在操作请求之前没有调用必要的先决操作。在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的前提条件无效。例如，如果之前调用
    /// ICapeThermoMaterialContext 接口的 SetMaterial 方法时未能提供有效的物流对象。</exception>
    /// <exception cref="ECapeInvalidArgument">一个或多个输入参数无效：
    /// 例如，属性参数或 phaseLabel 参数的未定义值。</exception>
    /// <exception cref="ECapeUnknown">当为 CheckSinglePhasePropSpec 操作指定的其他错误不合适时将引发的错误。</exception>
    bool ICapeThermoPropertyRoutine.CheckSinglePhasePropSpec(string property, string phaseLabel)
    {
        return _pIPropertyRoutine.CheckSinglePhasePropSpec(property, phaseLabel);
    }

    /// <summary>检查是否可以使用 CalcTwoPhaseProp 方法计算给定相位集的属性。</summary>
    /// <param name="property">要检查的属性的标识符。必须是 GetTwoPhasePropList
    /// 方法所支持的两相属性（包括导数）之一才有效。</param>
    /// <param name="phaseLabels">要计算属性的相位标签。相位标签必须是 ICapeThermoPhases
    /// 接口的 GetPhaseList 方法返回的两个标识符。</param>
    /// <returns>布尔型，如果支持属性和相位标签的组合，则设为 True；如果不支持，则设为 False。</returns>
    /// <remarks><para>检查结果应仅取决于实现 ICapeThermoPropertyRoutine 接口的组件（如属性包）
    /// 的功能和配置（存在的化合物和物相）。它不应取决于物流对象是否已被设置，也不应取决于可能被设置的
    /// 物流对象的状态（温度、压力、成分等）或配置。</para>
    /// <para>在导入属性包时，预计 PME 或其他客户端将使用此方法检查属性包是否支持其所需的属性。
    /// 如果任何基本属性不可用，则应中止导入过程。</para>
    /// <para>如果实现 ICapeThermoPropertyRoutine 接口的组件无法识别属性参数或
    /// phaseLabels 参数中的值，则此方法应返回 False。</para></remarks>
    /// <exception cref="ECapeNoImpl">由于与 CAPE-OPEN 标准兼容的原因，即使可以调用
    /// CheckTwoPhasePropSpec 方法，该操作也 “未 ”实现。也就是说，该操作是存在的，
    /// 但当前的实现并不支持。如果不支持两相属性，就可能出现这种情况。</exception>
    /// <exception cref="ECapeBadInvOrder">在操作请求之前没有调用必要的先决操作。
    /// 在调用此方法之前，未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的前提条件无效。例如，如果之前调用
    /// ICapeThermoMaterialContext 接口的 SetMaterial 方法时未能提供有效的物流对象。</exception>
    /// <exception cref="ECapeInvalidArgument">一个或多个输入参数无效。例如，属性参数或
    /// phaseLabels 参数为 UNDEFINED 值，或 phaseLabels 数组中的元素数不等于 2。</exception>
    /// <exception cref="ECapeUnknown">当为 CheckTwoPhasePropSpec 操作指定的其他错误不合适时将引发的错误。</exception>
    bool ICapeThermoPropertyRoutine.CheckTwoPhasePropSpec(string property, string[] phaseLabels)
    {
        return _pIPropertyRoutine.CheckTwoPhasePropSpec(property, phaseLabels);
    }

    /// <summary>返回支持的非恒定单相物理性质列表。</summary>
    /// <returns>所有支持的非恒定单相属性标识符列表。标准单相属性标识符列于第 7.5.5 节。</returns>
    /// <remarks><para>非恒定属性取决于材质对象的状态。</para>
    /// <para>单相属性（如焓值）只取决于一个相的状态。GetSinglePhasePropList 必须返回
    /// CalcSinglePhaseProp 可以计算的所有单相属性，如果可以计算导数，也必须返回。</para>
    /// <para>如果不支持单相属性，该方法应返回 UNDEFINED。</para>
    /// <para>要获取支持的两相属性列表，请使用 GetTwoPhasePropList。</para>
    /// <para>实现此方法的组件可返回不属于第 7.5.5 节所定义列表的非恒定单相属性标识符。
    /// 然而，该组件的大多数客户可能无法理解这些专有标识符。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，
    /// 也 “未 ”执行该操作。也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为 GetSinglePhasePropList 操作指定的其他错误不合适时将引发的错误。</exception>
    [DispId(0x00000006)]
    [Description("Method GetSinglePhasePropList")]
    string[] ICapeThermoPropertyRoutine.GetSinglePhasePropList()
    {
        return (string[])_pIPropertyRoutine.GetSinglePhasePropList();
    }

    /// <summary>返回支持的非恒定两相属性列表。</summary>
    /// <returns>所有受支持的非恒定两相态属性标识符列表。标准两相属性标识符列于第 7.5.6 节。</returns>
    /// <remarks><para>非恒定属性取决于物流对象的状态。两相属性是指取决于多个共存相的属性，如 K 值。</para>
    /// <para>GetTwoPhasePropList 必须返回 CalcTwoPhaseProp 可以计算的所有属性。
    /// 如果可以计算导数，也必须返回这些导数。</para>
    /// <para>如果不支持两相属性，该方法应返回 UNDEFINED。</para>
    /// <para>使用 CheckTwoPhasePropSpec 方法检查属性是否可以针对特定相位标签集进行评估。</para>
    /// <para>实现此方法的组件可返回不属于第 7.5.6 节所定义列表的非恒定两相态属性标识符。
    /// 然而，该组件的大多数客户可能无法理解这些专有标识符。</para>
    /// <para>要获取支持的单相属性列表，请使用 GetSinglePhasePropList。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，
    /// 也 “未 ”执行该操作。也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为 GetTwoPhasePropList 操作指定的其他错误不合适时，将引发的错误。</exception>
    [DispId(0x00000007)]
    [Description("Method GetTwoPhasePropList")]
    string[] ICapeThermoPropertyRoutine.GetTwoPhasePropList()
    {
        return (string[])_pIPropertyRoutine.GetTwoPhasePropList();
    }

    /// <summary>CalcEquilibrium 用于计算平衡时各相的数量和组成。CalcEquilibrium 将计算温度和/或压力，
    /// 如果温度和/或压力不在每次平衡计算必须考虑的两个规格之列。</summary>
    /// <remarks><para>参数 specification1 和 specification2 为平衡计算提供了检索两个参数值
    /// （如压力和温度）所需的信息。CheckEquilibriumSpec 方法可用于检查是否支持规格。每个规格变量
    /// 都按下表定义的顺序包含一串字符串（因此，规格参数可能有 3 或 4 个项目）：</para>
    /// <para>属性标识符 属性标识符可以是第 7.5.5 节中列出的任何标识符，但均衡例程通常只支持某些属性规范。</para>
    /// <para>属性值的基础。第 7.4 节给出了基础的有效设置。使用 UNDEFINED 作为不适用基础的属性的占位符。
    /// 对于大多数平衡规范来说，计算结果并不取决于计算基础，但对于相分数规范来说，计算基 础（摩尔或质量）确实会产生影响。</para>
    /// <para>相态标签 相态标签表示该规范适用的相态。它必须是 GetPresentPhases 返回的标签之一，或者是特殊值 “Overall”。</para>
    /// <para>化合物标识符（可选）化合物标识符用于表示依赖于特定化合物的规范。规格数组中的此项为可选项，可以省略。
    /// 如果是没有化合物标识符的规范，数组元素可能存在但为空，也可能不存在。</para>
    /// <para>下表列出了一些典型的相平衡规格示例。</para>
    /// <para>在调用 CalcEquilibrium 之前，必须在相关的 “物流对象 ”中设置与参数列表中的规格和混合物总体成分相对应的值。</para>
    /// <para>属性包或平衡计算器等组件必须实现 ICapeThermoMaterialContext 接口，以便通过 SetMaterial 方法传递
    /// ICapeThermoMaterial 接口。在尝试计算之前，CalcEquilibrium 的实现有责任验证物流对象。</para>
    /// <para>平衡计算中将考虑的相是存在于物流对象中的相，即在 SetPresentPhases 调用中指定的相列表。
    /// 这为客户提供了一种方法来指定是否需要进行汽-液、液-液或汽-液-液计算。CalcEquilibrium 必须使用
    /// GetPresentPhases 方法来获取相位列表和相关的相位状态标志。客户端可使用相状态标志来提供有关相的信息，
    /// 例如是否提供了平衡状态的估计值。详见 ICapeThermoMaterial 接口的 GetPresentPhases 和 SetPresentPhases
    /// 方法说明。平衡计算成功完成后，必须使用 SetPresentPhases 方法来指定哪些相处于平衡状态，并将这些相的相状态标志
    /// 设置为 Cape_AtEquilibrium。这必须包括任何以零数量存在的相，例如露点计算中的液相。</para>
    /// <para>某些类型的相平衡规范可能会产生不止一种解决方案。露点计算就是一个常见的例子。然而，CalcEquilibrium
    /// 只能通过 “物流对象 ”提供一种解决方案。solutionType 参数允许明确请求 “正常 ”或 “逆行 ”解决方案。
    /// 当所有规格都不包括相分数时，应将 solutionType 参数设置为 “未指定”。</para>
    /// <para>“正常”的定义是 V-F 为蒸汽相分率，且各项衍生物处于平衡状态。对于“逆行”行为，CalcEquilibrium
    /// 必须设定处于平衡状态的所有相的量、组成、温度和压力，以及如果未作为计算规范的一部分设定，则设定
    /// 整个混合物的温度和压力。CalcEquilibrium不得设定任何其他物理属性。</para>
    /// <para>例如，在固定压力和温度下进行平衡计算时，CalcEquilibrium 可能会执行以下操作序列：</para>
    /// <para>1. 使用所提供物流对象的 ICapeThermoMaterial 接口：</para>
    /// <para>2. 使用 GetPresentPhases 方法查找平衡计算应考虑的相位列表。</para>
    /// <para>3. 通过物流对象的 ICapeThermoCompounds 接口，使用 GetCompoundIds 方法查找存在哪些化合物。</para>
    /// <para>4. 使用 GetOverallProp 方法获取整个混合物的温度、压力和成分。</para>
    /// <para>5. 进行平衡计算。</para>
    /// <para>6. 使用 SetPresentPhases 指定平衡时存在的相位，并将相位状态标志设置为 Cape_AtEquilibrium。</para>
    /// <para>7. 使用 SetSinglePhaseProp 设置所有存在相的压力、温度、相量（或相分数）和成分。</para></remarks>
    /// <param name="specification1">平衡计算的第一个规范。规格信息用于从 “物流对象 ”中获取规格值。详见下文。</param>
    /// <param name="specification2">平衡计算的第二个规格，格式与规格 specification1 相同。</param>
    /// <param name="solutionType"><para>所需解决方案类型的标识符。标准标识符见下表：</para>
    /// <para>Unspecified</para><para>Normal</para><para>Retrograde</para>
    /// <para>这些术语的含义在下面的注释中定义。可能还支持其他标识符，但其解释不属于 CO 标准的一部分。</para></param>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，
    /// 也 “未 ”执行该操作。也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeBadInvOrder">在操作请求之前没有调用必要的先决操作。在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeSolvingError">平衡计算无法求解。例如，求解器的迭代次数已用完，
    /// 或已收敛到一个微不足道的解。</exception>
    /// <exception cref="ECapeLimitedImpl">如果平衡例程无法执行要求它执行的闪蒸，则会出现该提示。
    /// 例如，给定的输入规格值是有效的，但例程无法在给定温度和化合物分数的情况下执行闪蒸。这就意味着错误
    /// 使用或未使用 CheckEquilibriumSpec 方法，该方法的作用是防止调用 CalcEquilibrium 进行无法执行的计算。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用。例如，如果规范标识符不属于
    /// 可识别标识符列表，就会引发无效。如果给参数 solutionType 的值不在定义的三个参数之列，
    /// 或者使用了 UNDEFINED 而不是规范标识符，也会出现此警告。</exception>
    /// <exception cref="ECapeFailedInitialisation"><para>平衡计算的前提条件无效。例如：</para>
    /// <para>1. 混合物的总体成分没有确定。</para>
    /// <para>2. 物流对象（由之前调用 ICapeThermoMaterialContext 接口的 SetMaterial 方法设置）无效。
    /// 这可能是由于不存在任何相态，也可能是由于实现 ICapeThermoEquilibriumRoutine 接口的组件无法识别所存在的相位。</para>
    /// <para>3. 没有任何其他必要的输入信息。</para></exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    void ICapeThermoEquilibriumRoutine.CalcEquilibrium(string[] specification1, string[] specification2,
        string solutionType)
    {
        _pIEquilibriumRoutine.CalcEquilibrium(specification1, specification2, solutionType);
    }
    
    /// <summary>检查属性包是否支持特定类型的平衡计算。</summary>
    /// <remarks><para>参数 specification1、specification2 和 solutionType 的含义与 CalcEquilibrium 方法相同。</para>
    /// <para>检查结果应仅取决于实现 ICapeThermoEquilibriumRoutine 接口的组件（如属性包）的功能和配置
    /// （存在的化合物和相）。它不应取决于物流对象是否已被设置，也不应取决于可能被设置的物流对象的状态
    /// （温度、压力、成分等）或配置。</para>
    /// <para>如果 solutionType、specification1 和 specification2 参数看起来有效，
    /// 但实际规格不支持或未被识别，则应返回 False 值。</para></remarks>
    /// <param name="specification1">关于平衡计算的首个规范。</param>
    /// <param name="specification2">平衡计算的第二个规范。</param>
    /// <param name="solutionType">所需的解决方案类型。</param>
    /// <returns>如果支持规格和解决方案类型的组合，则设为 True；如果不支持，则设为 False。</returns>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，
    /// 也 “未 ”执行该操作。也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeInvalidArgument">用于传递无效参数值时，
    /// 例如 solutionType、specification1 或 specification2 参数的 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    bool ICapeThermoEquilibriumRoutine.CheckEquilibriumSpec(string[] specification1, string[] specification2,
        string solutionType)
    {
        return _pIEquilibriumRoutine.CheckEquilibriumSpec(specification1, specification2, solutionType);
    }
}