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
    /// <exception cref="ECapeNoImpl">操作 GetOverallProp 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法仍可被调用。也就是说，该操作虽然存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所需的物理属性无法从物流对象中获取，可能是因为请求的基础属性不存在。当调用 CreateMaterial 或 ClearAllProps 方法后未设置物理属性值时，将引发此异常。</exception>
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
    /// 也可以是用于设置由更复杂数据结构描述的两相属性的 CapeInterused 对象。</para></remarks>
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

    /// <summary>返回与某个阶段相关的属性信息，以便理解该阶段标签背后的含义。</summary>
    /// <param name="phaseLabel">单相标签。此标签必须是 GetPhaseList 方法返回的值之一。</param>
    /// <param name="phaseAttribute">下表中的一个相位属性标识符。</param>
    /// <returns>与相态属性标识符对应的值 – 参见下表。</returns>
    /// <remarks><para>GetPhaseInfo 旨在允许 PME 或其他客户端识别具有任意标签的阶段。PME 或其他客户端需要执行此操作，
    /// 以将流数据映射到材料对象，或在导入属性包时。如果客户端无法识别阶段，它可以要求用户根据这些属性的值提供映射。</para>
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

    /// <summary>返回所有支持的阶段的阶段标签及其他重要描述性信息。</summary>
    /// <param name="phaseLabels">支持的阶段的阶段标签列表。阶段标签可以是任何字符串，但每个阶段必须具有唯一的标签。
    /// 如果由于某种原因不支持任何阶段，则应为 phaseLabels 返回 UNDEFINED 值。
    /// 阶段标签的数量也必须等于 GetNumPhases 方法返回的阶段数量。</param>
    /// <param name="stateOfAggregation">与每个相态相关的物理聚集状态。该值必须为以下字符串之一：
    /// “气态”、“液态”、“固态”或“未知”。每个相态必须仅有一个聚集状态。该值不得留空，但可设置为“未知”。</param>
    /// <param name="keyCompoundId">该相的关键化合物。此处必须为化合物标识符（由 GetCompoundList 函数返回），
    /// 否则可能为未定义值，此时将返回UNDEFINED值。关键化合物指的是在该相中预期以高浓度存在的化合物，
    /// 例如水在水相中。每个相只能有一个关键化合物。</param>
    /// <remarks><para>相标签允许在 ICapeThermoPhases 接口和其他 CAPE-OPEN 接口的方法中唯一标识相。
    /// 聚集状态和关键化合物为 PME 或其他客户端提供了一种方式，使其能够根据相的物理特性来解释相标签的含义。</para>
    /// <para>此方法返回的所有数组必须具有相同的长度，即等于相位标签的数量。</para>
    /// <para>要获取有关某个阶段的更多信息，请使用 GetPhaseInfo 方法。</para></remarks>
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
    /// <para>如果请求的物理性质数量为 P，化合物数量为 C，则 propvals 数组将包含 C*P 个变体。前 C 个变体将是
    /// 第一个请求的物理性质的值（每个化合物一个变体），接着是第二个物理性质的 C 个常量值，依此类推。实际返回的值类型
    /// （双精度浮点数、字符串等）取决于物理性质，具体请参见第 7.5.2 节的说明。</para>
    /// <para>物理性质以固定的单位集形式返回，具体单位集如第 7.5.2 节所规定。</para>
    /// <para>如果 compIds 参数设置为 UNDEFINED，则表示请求返回实现 ICapeThermoCompounds 接口的组件中所有
    /// 化合物的属性值，且化合物的顺序与 GetCompoundList 方法返回的顺序相同。例如，如果该接口由一个属性包组件实现，
    /// 则将 compIds 设置为 UNDEFINED 的属性请求表示属性包中的所有化合物，而非传递给属性包的材料对象中的所有化合物。</para>
    /// <para>如果某个物理属性对一个或多个化合物不可用，则必须为这些组合返回未定义的值，
    /// 并抛出 ECapeThrmPropertyNotAvailable 异常。如果抛出异常，客户端应检查所有返回的值以确定哪个值未定义。</para></remarks>
    /// <param name="props">The list of Physical Property identifiers. Valid
    /// identifiers for constant Physical Properties are listed in
    /// section 7.5.2.</param>
    /// <param name="compIds">List of Compound identifiers for which constants are 
    /// to be retrieved. Set compIds = UNDEFINED to denote all Compounds in the 
    /// component that implements the ICapeThermoCompounds interface.</param>
    /// <returns>Values of constants for the specified Compounds.</returns>
    /// <exception cref="ECapeNoImpl">The operation GetCompoundConstant is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists, but 
    /// it is not supported by the current implementation. This exception should be 
    /// raised if no compounds or no properties are supported.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">At least one item in the 
    /// list of Physical Properties is not available for a particular Compound. This 
    /// exception is meant to be treated as a warning rather than as an error.</exception>
    /// <exception cref="ECapeLimitedImpl">One or more Physical Properties are not 
    /// supported by the component that implements this interface. This exception 
    /// should also be raised if any element of the props argument is not recognised 
    /// since the list of Physical Properties in section 7.5.2 is not intended to be 
    /// exhaustive and an unrecognised Physical Property identifier may be valid. If
    /// no Physical Properties at all are supported ECapeNoImpl should be raised 
    /// (see above).</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetCompoundConstant method. The error would not be raised when the 
    /// GetCompoundConstant method is implemented by a Material Object.</exception>
    object[] ICapeThermoCompounds.GetCompoundConstant(string[] props, string[] compIds)
    {
        return (object[])_pICompounds.GetCompoundConstant(props, compIds);
    }

    /// <summary>Returns the list of all Compounds. This includes the Compound 
    /// identifiers recognised and extra information that can be used to further 
    /// identify the Compounds.</summary>
    /// <remarks><para>If any item cannot be returned then the value should be set 
    /// to UNDEFINED. The same information can also be extracted using the 
    /// GetCompoundConstant method. The equivalences between GetCompoundList 
    /// arguments and Compound constant Physical Properties, as specified in section 
    /// 7.5.2, is as follows:</para>
    /// <para>compIds - No equivalence. compIds is an artefact, which is assigned by 
    /// the component that implements the GetCompoundList method. This string will 
    /// normally contain a unique Compound identifier such as "benzene". It must be 
    /// used in all the arguments which are named “compIds” in the methods of the
    ///ICapeThermoCompounds and ICapeThermoMaterial interfaces.</para>
    /// <para>Formulae - chemicalFormula</para>
    /// <para>names - iupacName</para>
    /// <para>boilTemps - normalBoilingPoint</para>
    /// <para>molwts - molecularWeight</para>
    /// <para>casnos casRegistryNumber</para>
    /// <para>When the ICapeThermoCompounds interface is implemented by a Material 
    /// Object, the list of Compounds returned is fixed when the Material Object is 
    /// configured.</para>
    /// <para>For a Property Package component, the Property Package will normally 
    /// contain a limited set of Compounds selected for a particular application, 
    /// rather than all possible Compounds that could be available to a proprietary 
    /// Properties System.</para>
    /// <para>In order to identify the Compounds of a Property Package, the PME, or 
    /// other client, will use the casnos argument rather than the compIds. This is 
    /// because different PMEs may give different names to the same Compounds and the
    /// casnos is (almost always) unique. If the casnos is not available (e.g. for 
    /// petroleum fractions), or not unique, the other pieces of information returned 
    /// by GetCompoundList can be used to distinguish the Compounds. It should be 
    /// noted, however, that for communication with a Property Package a client must 
    /// use the Compound identifiers returned in the compIds argument.</para></remarks>
    /// <param name="compIds">List of Compound identifiers</param>
    /// <param name="formulae">List of Compound formulae</param>
    /// <param name="names">List of Compound names.</param>
    /// <param name="boilTemps">List of boiling point temperatures.</param>
    /// <param name="molwts">List of molecular weights.</param>
    /// <param name="casnos">List of Chemical Abstract Service (CAS) Registry
    /// numbers.</param>
    /// <exception cref="ECapeNoImpl">The operation GetCompoundList is “not” 
    /// implemented even if this method can be called for reasons of compatibility
    /// with the CAPE-OPEN standards. That is to say that the operation exists, but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetCompoundList operation, are not suitable.</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the Property 
    /// Package required the SetMaterial method to be called before calling the 
    /// GetCompoundList method. The error would not be raised when the 
    /// GetCompoundList method is implemented by a Material Object.</exception>
    void ICapeThermoCompounds.GetCompoundList(ref string[] compIds, ref string[] formulae, ref string[] names,
        ref double[] boilTemps, ref double[] molwts, ref string[] casnos)
    {
        object obj1 = null;
        object obj2 = null;
        object obj3 = null;
        object obj4 = null;
        object obj5 = null;
        object obj6 = null;
        _pICompounds.GetCompoundList(ref obj1, ref obj2, ref obj3, ref obj4, ref obj5, ref obj6);
        compIds = (string[])obj1;
        formulae = (string[])obj2;
        names = (string[])obj3;
        boilTemps = (double[])obj4;
        molwts = (double[])obj5;
        casnos = (string[])obj6;
    }

    /// <summary>Returns the list of supported constant Physical Properties.</summary>
    /// <returns>List of identifiers for all supported constant Physical Properties. 
    /// The standard constant property identifiers are listed in section 7.5.2.
    /// </returns>
    /// <remarks><para>MGetConstPropList returns identifiers for all the constant Physical 
    /// Properties that can be retrieved by the GetCompoundConstant method. If no 
    /// properties are supported, UNDEFINED should be returned. The CAPE-OPEN 
    /// standards do not define a minimum list of Physical Properties to be made 
    /// available by a software component that implements the ICapeThermoCompounds 
    /// interface.</para>
    /// <para>A component that implements the ICapeThermoCompounds interface may 
    /// return constant Physical Property identifiers which do not belong to the 
    /// list defined in section 7.5.2.</para>
    /// <para>However, these proprietary identifiers may not be understood by most 
    /// of the clients of this component.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation GetConstPropList is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists, but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the Get-ConstPropList operation, are not suitable.</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetConstPropList method. The error would not be raised when the 
    /// GetConstPropList method is implemented by a Material Object.</exception>
    string[] ICapeThermoCompounds.GetConstPropList()
    {
        return (string[])_pICompounds.GetConstPropList();
    }

    /// <summary>Returns the number of Compounds supported.</summary>
    /// <returns>Number of Compounds supported.</returns>
    /// <remarks>The number of Compounds returned by this method must be equal to 
    /// the number of Compound identifiers that are returned by the GetCompoundList 
    /// method of this interface. It must be zero or a positive number.</remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetNumCompounds method. The error would not be raised when the 
    /// GetNumCompounds method is implemented by a Material Object.</exception>
    int ICapeThermoCompounds.GetNumCompounds()
    {
        return _pICompounds.GetNumCompounds();
    }

    /// <summary>Returns the values of pressure-dependent Physical Properties for 
    /// the specified pure Compounds.</summary>
    /// <param name="props">The list of Physical Property identifiers. Valid
    /// identifiers for pressure-dependent Physical Properties are listed in section 
    /// 7.5.4</param>
    /// <param name="pressure">Pressure (in Pa) at which Physical Properties are
    /// evaluated</param>
    /// <param name="compIds">List of Compound identifiers for which Physical
    /// Properties are to be retrieved. Set compIds = UNDEFINED to denote all 
    /// Compounds in the component that implements the ICapeThermoCompounds 
    /// interface.</param>
    /// <param name="propVals">>Property values for the Compounds specified.</param>
    /// <remarks><para>The GetPDependentPropList method can be used in order to 
    /// check which Physical Properties are available.</para>
    /// <para>If the number of requested Physical Properties is P and the number 
    /// Compounds is C, the propvals array will contain C*P values. The first C 
    /// will be the values for the first requested Physical Property followed by C 
    /// values for the second Physical Property, and so on.</para>
    /// <para>Physical Properties are returned in a fixed set of units as specified 
    /// in section 7.5.4.</para>
    /// <para>If the compIds argument is set to UNDEFINED this is a request to return 
    /// property values for all compounds in the component that implements the 
    /// ICapeThermoCompounds interface with the compound order the same as that 
    /// returned by the GetCompoundList method. For example, if the interface is 
    /// implemented by a Property Package component the property request with compIds 
    /// set to UNDEFINED means all compounds in the Property Package rather than all 
    /// compounds in the Material Object passed to the Property package.</para>
    /// <para>If any Physical Property is not available for one or more Compounds, 
    /// then undefined valuesm must be returned for those combinations and an 
    /// ECapeThrmPropertyNotAvailable exception must be raised. If the exception is 
    /// raised, the client should check all the values returned to determine which is 
    /// undefined.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation. This exception should be raised if no Compounds 
    /// or no Physical Properties are supported.</exception>
    /// <exception cref="ECapeLimitedImpl">One or more Physical Properties are not 
    /// supported by the component that implements this interface. This exception 
    /// should also be raised (rather than ECapeInvalidArgument) if any element of 
    /// the props argument is not recognised since the list of Physical Properties 
    /// in section 7.5.4 is not intended to be exhaustive and an unrecognised
    /// Physical Property identifier may be valid. If no Physical Properties at all 
    /// are supported, ECapeNoImpl should be raised (see above).</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example UNDEFINED for argument props.</exception>
    /// <exception cref="ECapeOutOfBounds">The value of the pressure is outside of
    /// the range of values accepted by the Property Package.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">At least one item in the 
    /// properties list is not available for a particular compound.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetPDependentProperty method. The error would not be raised when the 
    /// GetPDependentProperty method is implemented by a Material Object.</exception>
    void ICapeThermoCompounds.GetPDependentProperty(string[] props, double pressure, string[] compIds,
        ref double[] propVals)
    {
        object obj1 = null;
        _pICompounds.GetPDependentProperty(props, pressure, compIds, ref obj1);
        propVals = (double[])obj1;
    }

    ///<summary>Returns the list of supported pressure-dependent properties.</summary>
    ///<returns>The list of Physical Property identifiers for all supported 
    /// pressure-dependent properties. The standard identifiers are listed in 
    /// section 7.5.4</returns>
    /// <remarks><para>GetPDependentPropList returns identifiers for all the pressure-dependent 
    /// properties that can be retrieved by the GetPDependentProperty method. If no 
    /// properties are supported UNDEFINED should be returned. The CAPE-OPEN standards 
    /// do not define a minimum list of Physical Properties to be made available by 
    /// a software component that implements the ICapeThermoCompounds interface.</para>
    /// <para>A component that implements the ICapeThermoCompounds interface may 
    /// return identifiers which do not belong to the list defined in section 7.5.4. 
    /// However, these proprietary identifiers may not be understood by most of the 
    /// clients of this component.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the Property 
    /// Package required the SetMaterial method to be called before calling the 
    /// GetPDependentPropList method. The error would not be raised when the 
    /// GetPDependentPropList method is implemented by a Material Object.</exception>
    string[] ICapeThermoCompounds.GetPDependentPropList()
    {
        return (string[])_pICompounds.GetPDependentPropList();
    }

    /// <summary>Returns the values of temperature-dependent Physical Properties for 
    /// the specified pure Compounds.</summary>
    /// <param name="props">The list of Physical Property identifiers. Valid
    /// identifiers for temperature-dependent Physical Properties are listed in 
    /// section 7.5.3</param>
    /// <param name="temperature">Temperature (in K) at which properties are 
    /// evaluated.</param>
    /// <param name="compIds">List of Compound identifiers for which Physical
    /// Properties are to be retrieved. Set compIds = UNDEFINED to denote all 
    /// Compounds in the component that implements the ICapeThermoCompounds 
    /// interface .</param>
    /// <param name="propVals">Physical Property values for the Compounds specified.</param>
    /// <remarks> <para>The GetTDependentPropList method can be used in order to 
    /// check which Physical Properties are available.</para>
    /// <para>If the number of requested Physical Properties is P and the number of 
    /// Compounds is C, the propvals array will contain C*P values. The first C will 
    /// be the values for the first requested Physical Property followed by C values 
    /// for the second Physical Property, and so on.</para>
    /// <para>Properties are returned in a fixed set of units as specified in 
    /// section 7.5.3.</para>
    /// <para>If the compIds argument is set to UNDEFINED this is a request to return 
    /// property values for all compounds in the component that implements the 
    /// ICapeThermoCompounds interface with the compound order the same as that 
    /// returned by the GetCompoundList method. For example, if the interface is 
    /// implemented by a Property Package component the property request with compIds 
    /// set to UNDEFINED means all compounds in the Property Package rather than all 
    /// compounds in the Material Object passed to the Property package.</para>
    /// <para>If any Physical Property is not available for one or more Compounds, 
    /// then undefined values must be returned for those combinations and an 
    /// ECapeThrmPropertyNotAvailable exception must be raised. If the exception is 
    /// raised, the client should check all the values returned to determine which is 
    /// undefined.</para></remarks>
    /// <exception cref="ECapeNoImpl"> – The operation is “not” implemented even 
    /// if this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation. This exception should be raised if no 
    /// Compounds or no Physical Properties are supported.</exception>
    /// <exception cref="ECapeLimitedImpl">One or more Physical Properties are not
    /// supported by the component that implements this interface. This exception 
    /// should also be raised (rather than ECapeInvalidArgument) if any element of 
    /// the props argument is not recognised since the list of properties in section 
    /// 7.5.3 is not intended to be exhaustive and an unrecognised Physical Property 
    /// identifier may be valid. If no properties at all are supported ECapeNoImpl 
    /// should be raised (see above).</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example UNDEFINED for argument props.</exception> 
    /// <exception cref="ECapeOutOfBounds">The value of the temperature is outside
    /// of the range of values accepted by the Property Package.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">At least one item in the 
    /// properties list is not available for a particular compound.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref="ECapeBadInvOrder"> The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetTDependentProperty method. The error would not be raised when the 
    /// GetTDependentProperty method is implemented by a Material Object.</exception>
    void ICapeThermoCompounds.GetTDependentProperty(string[] props, double temperature, string[] compIds,
        ref double[] propVals)
    {
        object obj1 = null;
        _pICompounds.GetTDependentProperty(props, temperature, compIds, ref obj1);
        propVals = (double[])obj1;
    }

    /// <summary>Returns the list of supported temperature-dependent Physical 
    /// Properties.</summary>
    /// <returns>The list of Physical Property identifiers for all supported 
    /// temperature-dependent properties. The standard identifiers are listed in 
    /// section 7.5.3</returns>
    /// <remarks><para>GetTDependentPropList returns identifiers for all the 
    /// temperature-dependent Physical Properties that can be retrieved by the 
    /// GetTDependentProperty method. If no properties are supported UNDEFINED 
    /// should be returned. The CAPE-OPEN standards do not define a minimum list of 
    /// properties to be made available by a software component that implements the 
    /// ICapeThermoCompounds interface.</para>
    /// <para>A component that implements the ICapeThermoCompounds interface may 
    /// return identifiers which do not belong to the list defined in section 
    /// 7.5.3. However, these proprietary identifiers may not be understood by most 
    /// of the clients of this component.</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s),
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the Property 
    /// Package required the SetMaterial method to be called before calling the 
    /// GetTDependentPropList method. The error would not be raised when the 
    /// GetTDependentPropList method is implemented by a Material Object.</exception>
    string[] ICapeThermoCompounds.GetTDependentPropList()
    {
        return (string[])_pICompounds.GetTDependentPropList();
    }

    /// <summary>This method is used to calculate the natural logarithm of the 
    /// fugacity coefficients (and optionally their derivatives) in a single Phase 
    /// mixture. The values of temperature, pressure and composition are specified in 
    /// the argument list and the results are also returned through the argument list.</summary>
    /// <param name="phaseLabel">Phase label of the Phase for which the properties 
    /// are to be calculated. The Phase label must be one of the strings returned by 
    /// the GetPhaseList method on the ICapeThermoPhases interface.</param>
    /// <param name="temperature">The temperature (K) for the calculation.</param>
    /// <param name="pressure">The pressure (Pa) for the calculation.</param>
    /// <param name="lnPhiDt">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. temperature (if requested).</param>
    /// <param name="moleNumbers">Number of moles of each Compound in the mixture.</param>
    /// <param name="fFlags">Code indicating whether natural logarithm of the 
    /// fugacity coefficients and/or derivatives should be calculated (see notes).</param>
    /// <param name="lnPhi">Natural logarithm of the fugacity coefficients (if
    /// requested).</param>
    /// <param anem = "lnPhiDT">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. temperature (if requested).</param>
    /// <param name="lnPhiDp">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. pressure (if requested).</param>
    /// <param name="lnPhiDn">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. mole numbers (if requested).</param>
    /// <remarks><para>This method is provided to allow the natural logarithm of the fugacity 
    /// coefficient, which is the most commonly used thermodynamic property, to be 
    /// calculated and returned in a highly efficient manner.</para>
    /// <para>The temperature, pressure and composition (mole numbers) for the 
    /// calculation are specified by the arguments and are not obtained from the 
    /// Material Object by a separate request. Likewise, any quantities calculated are 
    /// returned through the arguments and are not stored in the Material Object. The 
    /// state of the Material Object is not affected by calling this method. It should 
    /// be noted however, that prior to calling CalcAndGetLnPhi a valid Material 
    /// Object must have been defined by calling the SetMaterial method on the
    /// ICapeThermoMaterialContext interface of the component that implements the
    /// ICapeThermoPropertyRoutine interface. The compounds in the Material Object 
    /// must have been identified and the number of values supplied in the moleNumbers
    /// argument must be equal to the number of Compounds in the Material Object.</para>
    /// <para>The fugacity coefficient information is returned as the natural 
    /// logarithm of the fugacity coefficient. This is because thermodynamic models 
    /// naturally provide the natural logarithm of this quantity and also a wider 
    /// range of values may be safely returned.</para>
    /// <para>The quantities actually calculated and returned by this method are 
    /// controlled by an integer code fFlags. The code is formed by summing 
    /// contributions for the property and each derivative required using the 
    /// enumerated constants eCapeCalculationCode (defined in the
    /// Thermo version 1.1 IDL) shown in the following table. For example, to 
    /// calculate log fugacity coefficients and their T-derivatives the fFlags 
    /// argument would be set to CAPE_LOG_FUGACITY_COEFFICIENTS + CAPE_T_DERIVATIVE.</para>
    /// <table border="1">
    /// <tr>
    /// <th>Calculation Type</th>
    /// <th>Enumeration Value</th>
    /// <th>Numerical Value</th>
    /// </tr>
    /// <tr>
    /// <td>no calculation</td>
    /// <td>CAPE_NO_CALCULATION</td>
    /// <td>0</td>
    /// </tr>
    /// <tr>
    /// <td>log fugacity coefficients</td>
    /// <td>CAPE_LOG_FUGACITY_COEFFICIENTS</td>
    /// <td>1</td>
    /// </tr>
    /// <tr>
    /// <td>T-derivative</td>
    /// <td>CAPE_T_DERIVATIVE</td>
    /// <td>2</td>
    /// </tr>
    /// <tr>
    /// <td>P-derivative</td>
    /// <td>CAPE_P_DERIVATIVE</td>
    /// <td>4</td>
    /// </tr>
    /// <tr>
    /// <td>mole number derivatives</td>
    /// <td>CAPE_MOLE_NUMBERS_DERIVATIVES</td>
    /// <td>8</td>
    /// </tr>
    /// </table>	
    /// <para>If CalcAndGetLnPhi is called with fFlags set to CAPE_NO_CALCULATION no 
    /// property values are returned.</para>
    /// <para>A typical sequence of operations for this method when implemented by a 
    /// Property Package component would be:</para>
    /// <para>- Check that the phaseLabel specified is valid.</para>
    /// <para>- Check that the moleNumbers array contains the number of values expected
    /// (should be consistent with the last call to the SetMaterial method).</para>
    /// <para>- Calculate the requested properties/derivatives at the T/P/composition specified in the argument list.</para>
    /// <para>- Store values for the properties/derivatives in the corresponding arguments.</para>
    /// <para>Note that this calculation can be carried out irrespective of whether the Phase actually exists in the Material Object.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported by 
    /// the current implementation.</exception>
    /// <exception cref="ECapeLimitedImpl">Would be raised if the one or more of the 
    /// properties requested cannot be returned because the calculation is not 
    /// implemented.</exception>
    /// <exception cref="ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. For example, the 
    /// ICapeThermoMaterial interface has not been passed via a SetMaterial call prior
    /// to calling this method.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites for the 
    ///	Property Calculation are not valid. Forexample, the composition of the phase is 
    /// not defined, the number of Compounds in the Material Object is zero or not 
    /// consistent with the moleNumbers argument or any other necessary input information 
    /// is not available.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">At least one item in the 
    /// requested properties cannot be returned. This could be because the property 
    /// cannot be calculated at the specified conditions or for the specified Phase. 
    /// If the property calculation is not implemented then ECapeLimitedImpl should 
    /// be returned.</exception>
    /// <exception cref="ECapeSolvingError">One of the property calculations has 
    /// failed. For example if one of the iterative solution procedures in the model 
    /// has run out of iterations, or has converged to a wrong solution.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example an unrecognised value, or UNDEFINED for the 
    /// phaseLabel argument.</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    void ICapeThermoPropertyRoutine.CalcAndGetLnPhi(string phaseLabel, double temperature,
        double pressure,
        double[] moleNumbers,
        CapeFugacityFlag fFlags,
        ref double[] lnPhi,
        ref double[] lnPhiDt,
        ref double[] lnPhiDp,
        ref double[] lnPhiDn)
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

    /// <summary>CalcSinglePhaseProp is used to calculate properties and property 
    /// derivatives of a mixture in a single Phase at the current values of 
    /// temperature, pressure and composition set in the Material Object. 
    /// CalcSinglePhaseProp does not perform phase Equilibrium Calculations.</summary>
    /// <param name="props">The list of identifiers for the single-phase properties 
    /// or derivatives to be calculated. See sections 7.5.5 and 7.6 for the standard 
    /// identifiers.</param>
    /// <param name="phaseLabel">Phase label of the Phase for which the properties 
    /// are to be calculated. The Phase label must be one of the strings returned by 
    /// the GetPhaseList method on the ICapeThermoPhases interface.</param>
    /// <remarks><para>CalcSinglePhaseProp calculates properties, such as enthalpy or viscosity 
    /// that are defined for a single Phase. Physical Properties that depend on more 
    /// than one Phase, for example surface tension or K-values, are handled by 
    /// CalcTwoPhaseProp method.</para>
    /// <para>Components that implement this method must get the input specification 
    /// for the calculation (temperature, pressure and composition) from the associated 
    /// Material Object and set the results in the Material Object.</para>
    /// <para>Thermodynamic and Physical Properties Components, such as a Property 
    /// Package or Property Calculator, must implement the ICapeThermoMaterialContext 
    /// interface so that an ICapeThermoMaterial interface can be passed via the 
    /// SetMaterial method.</para>
    /// <para>A typical sequence of operations for CalcSinglePhaseProp when implemented
    /// by a Property Package component would be:</para>
    /// <para>- Check that the phaseLabel specified is valid.</para>
    /// <para>- Use the GetTPFraction method (of the Material Object specified in the 
    /// last call to the SetMaterial method) to get the temperature, pressure and 
    /// composition of the specified Phase.</para>
    /// <para>- Calculate the properties.</para>
    /// <para>- Store values for the properties of the Phase in the Material Object 
    /// using the SetSinglePhaseProp method of the ICapeThermoMaterial interface.</para>
    /// <para>CalcSinglePhaseProp will request the input Property values it requires 
    /// from the Material Object through GetSinglePhaseProp calls. If a requested 
    /// property is not available, the exception raised will be 
    /// ECapeThrmPropertyNotAvailable. If this error occurs then the Property Package 
    /// can return it to the client, or request a different property. Material Object
    /// implementations must be able to supply property values using the client’s 
    /// choice of basis by implementing conversion from one basis to another.</para>
    /// <para>Clients should not assume that Phase fractions and Compound fractions in 
    /// a Material Object are normalised. Fraction values may also lie outside the 
    /// range 0 to 1. If fractions are not normalised, or are outside the expected 
    /// range, it is the responsibility of the Property Package to decide how to deal 
    /// with the situation.</para>
    /// <para>It is recommended that properties are requested one at a time in order 
    /// to simplify error handling. However, it is recognised that there are cases 
    /// where the potential efficiency gains of requesting several properties 
    /// simultaneously are more important. One such example might be when a property 
    /// and its derivatives are required.</para>
    /// <para>If a client uses multiple properties in a call and one of them fails 
    /// then the whole call should be considered to have failed. This implies that no 
    /// value should be written back to the Material Object by the Property Package 
    /// until it is known that the whole request can be satisfied.</para>
    /// <para>It is likely that a PME might request values of properties for a Phase at 
    /// conditions of temperature, pressure and composition where the Phase does not 
    /// exist (according to the mathematical/physical models used to represent 
    /// properties). The exception ECapeThrmPropertyNotAvailable may be raised or an 
    /// extrapolated value may be returned.</para>
    /// <para>It is responsibility of the implementer to decide how to handle this 
    /// circumstance.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if this
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref="ECapeLimitedImpl">Would be raised if the one or more of the 
    /// properties requested cannot be returned because the calculation (of the 
    /// particular property) is not implemented. This exception should also be raised 
    /// (rather than ECapeInvalidArgument) if the props argument is not recognised 
    /// because the list of properties in section 7.5.5 is not intended to be 
    /// exhaustive and an unrecognised property identifier may be valid. If no 
    /// properties at all are supported ECapeNoImpl should be raised (see above).</exception>
    /// <exception cref="ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. For example, the 
    /// ICapeThermoMaterial interface has not been passed via a SetMaterial call prior 
    /// to calling this method.</exception> 
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites for the 
    /// property calculation are not valid. For example, the composition of the phases
    /// is not defined or any other necessary input information is not available.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">At least one item in the 
    /// requested properties cannot be returned. This could be because the property 
    /// cannot be calculated at the specified conditions or for the specified phase. 
    /// If the property calculation is not implemented then ECapeLimitedImpl should be 
    /// returned.</exception>
    [DispId(0x00000002)]
    [Description("Method CalcSinglePhaseProp")]
    void ICapeThermoPropertyRoutine.CalcSinglePhaseProp(string[] props, string phaseLabel)
    {
        _pIPropertyRoutine.CalcSinglePhaseProp(props, phaseLabel);
    }

    /// <summary>CalcTwoPhaseProp is used to calculate mixture properties and property 
    /// derivatives that depend on two Phases at the current values of temperature, 
    /// pressure and composition set in the Material Object. It does not perform 
    /// Equilibrium Calculations.</summary>
    /// <param name="props">The list of identifiers for properties to be calculated.
    /// This must be one or more of the supported two-phase properties and derivatives 
    /// (as given by the GetTwoPhasePropList method). The standard identifiers for 
    /// two-phase properties are given in section 7.5.6 and 7.6.</param>
    /// <param name="phaseLabels">Phase labels of the phases for which the properties 
    /// are to be calculated. The phase labels must be two of the strings returned by 
    /// the GetPhaseList method on the ICapeThermoPhases interface.</param>
    /// <remarks><para>CalcTwoPhaseProp calculates the values of properties such as surface 
    /// tension or K-values. Properties that pertain to a single Phase are handled by 
    /// the CalcSinglePhaseProp method of the ICapeThermoPropertyRoutine interface.
    /// Components that implement this method must get the input specification for the 
    /// calculation (temperature, pressure and composition) from the associated 
    /// Material Object and set the results in the Material Object.</para>
    /// <para>Components such as a Property Package or Property Calculator must 
    /// implement the ICapeThermoMaterialContext interface so that an 
    /// ICapeThermoMaterial interface can be passed via the SetMaterial method.</para>
    /// <para>A typical sequence of operations for CalcTwoPhaseProp when implemented by
    /// a Property Package component would be:</para>
    /// <para>- Check that the phaseLabels specified are valid.</para>
    /// <para>- Use the GetTPFraction method (of the Material Object specified in the 
    /// last call to the SetMaterial method) to get the temperature, pressure and 
    /// composition of the specified Phases.</para>
    /// <para>- Calculate the properties.</para>
    /// <para>- Store values for the properties in the Material Object using the 
    /// SetTwoPhaseProp method of the ICapeThermoMaterial interface.</para>
    /// <para>CalcTwoPhaseProp will request the values it requires from the Material Object 
    /// through GetTPFraction or GetSinglePhaseProp calls. If a requested property is 
    /// not available, the exception raised will be ECapeThrmPropertyNotAvailable. If 
    /// this error occurs, then the Property Package can return it to the client, or 
    /// request a different property. Material Object implementations must be able to 
    /// supply property values using the client choice of basis by implementing 
    /// conversion from one basis to another.</para>
    /// <para>Clients should not assume that Phase fractions and Compound fractions in 
    /// a Material Object are normalised. Fraction values may also lie outside the 
    /// range 0 to 1. If fractions are not normalised, or are outside the expected 
    /// range, it is the responsibility of the Property Package to decide how to deal 
    /// with the situation.</para>
    /// <para>It is recommended that properties are requested one at a time in order to 
    /// simplify error handling. However, it is recognised that there are cases where 
    /// the potential efficiency gains of requesting several properties simultaneously 
    /// are more important. One such example might be when a property and its 
    /// derivatives are required.</para>
    /// <para>If a client uses multiple properties in a call and one of them fails, then the 
    /// whole call should be considered to have failed. This implies that no value 
    /// should be written back to the Material Object by the Property Package until 
    /// it is known that the whole request can be satisfied.</para>
    /// <para>CalcTwoPhaseProp must be called separately for each combination of Phase
    /// groupings. For example, vapour-liquid K-values have to be calculated in a 
    /// separate call from liquid-liquid K-values.</para>
    /// <para>Two-phase properties may not be meaningful unless the temperatures and 
    /// pressures of all Phases are identical. It is the responsibility of the Property 
    /// Package to check such conditions and to raise an exception if appropriate.</para>
    /// <para>It is likely that a PME might request values of properties for Phases at 
    /// conditions of temperature, pressure and composition where one or both of the 
    /// Phases do not exist (according to the mathematical/physical models used to 
    /// represent properties). The exception ECapeThrmPropertyNotAvailable may be 
    /// raised or an extrapolated value may be returned. It is responsibility of the 
    /// implementer to decide how to handle this circumstance.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if this 
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref="ECapeLimitedImpl">Would be raised if the one or more of the 
    /// properties requested cannot be returned because the calculation (of the 
    /// particular property) is not implemented. This exception should also be raised 
    /// (rather than ECapeInvalidArgument) if the props argument is not recognised 
    /// because the list of properties in section 7.5.6 is not intended to be 
    /// exhaustive and an unrecognised property identifier may be valid. If no 
    /// properties at all are supported ECapeNoImpl should be raised (see above).</exception>
    /// <exception cref="ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. For example, the 
    /// ICapeThermoMaterial interface has not been passed via a SetMaterial call 
    /// prior to calling this method.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites for the 
    /// property calculation are not valid. For example, the composition of one of the 
    /// Phases is not defined, or any other necessary input information is not 
    /// available.</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">At least one item in the 
    /// requested properties cannot be returned. This could be because the property 
    /// cannot be calculated at the specified conditions or for the specified Phase. 
    /// If the property calculation is not implemented then ECapeLimitedImpl should be 
    /// returned.</exception>
    /// <exception cref="ECapeSolvingError">One of the property calculations has 
    /// failed. For example if one of the iterative solution procedures in the model 
    /// has run out of iterations, or has converged to a wrong solution.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example an unrecognised value or UNDEFINED for the 
    /// phaseLabels argument or UNDEFINED for the props argument.</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    [DispId(0x00000003)]
    [Description("Method CalcTwoPhaseProp")]
    void ICapeThermoPropertyRoutine.CalcTwoPhaseProp(string[] props, string[] phaseLabels)
    {
        _pIPropertyRoutine.CalcTwoPhaseProp(props, phaseLabels);
    }

    /// <summary>Checks whether it is possible to calculate a property with the 
    /// CalcSinglePhaseProp method for a given Phase.</summary>
    /// <param name="property">The identifier of the property to check. To be valid 
    /// this must be one of the supported single-phase properties or derivatives (as 
    /// given by the GetSinglePhasePropList method).</param>
    /// <param name="phaseLabel">The Phase label for the calculation check. This must
    /// be one of the labels returned by the GetPhaseList method on the 
    /// ICapeThermoPhases interface.</param>
    /// <returns> A boolean set to True if the combination of property and phaseLabel
    /// is supported or False if not supported.</returns>
    /// <remarks><para>The result of the check should only depend on the capabilities and 
    /// configuration (Compounds and Phases present) of the component that implements 
    /// the ICapeThermoPropertyRoutine interface (eg. a Property Package). It should 
    /// not depend on whether a Material Object has been set nor on the state 
    /// (temperature, pressure, composition etc.), or configuration of a Material 
    /// Object that might be set.</para>
    /// <para>It is expected that the PME, or other client, will use this method to 
    /// check whether the properties it requires are supported by the Property Package
    /// when the package is imported. If any essential properties are not available, 
    /// the import process should be aborted.</para>
    /// <para>If either the property or the phaseLabel arguments are not recognised by 
    /// the component that implements the ICapeThermoPropertyRoutine interface this 
    /// method should return False.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation CheckSinglePhasePropSpec is 
    /// “not” implemented even if this method can be called for reasons of 
    /// compatibility with the CAPE-OPEN standards. That is to say that the operation 
    /// exists, but it is not supported by the current implementation.</exception>
    /// <exception cref="ECapeBadInvOrder">The necessary pre-requisite operation has
    /// not been called prior to the operation request. The ICapeThermoMaterial 
    /// interface has not been passed via a SetMaterial call prior to calling this 
    /// method.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites for the 
    /// property calculation are not valid. For example, if a prior call to the 
    /// SetMaterial method of the ICapeThermoMaterialContext interface has failed to 
    /// provide a valid Material Object.</exception>
    /// <exception cref="ECapeInvalidArgument">One or more of the input arguments is 
    /// not valid: for example, UNDEFINED value for the property argument or the 
    /// phaseLabel argument.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the CheckSinglePhasePropSpec operation, are not suitable.</exception>
    bool ICapeThermoPropertyRoutine.CheckSinglePhasePropSpec(string property, string phaseLabel)
    {
        return _pIPropertyRoutine.CheckSinglePhasePropSpec(property, phaseLabel);
    }

    /// <summary>Checks whether it is possible to calculate a property with the 
    /// CalcTwoPhaseProp method for a given set of Phases.</summary>
    /// <param name="property">The identifier of the property to check. To be valid 
    /// this must be one of the supported two-phase properties (including derivatives), 
    /// as given by the GetTwoPhasePropList method.</param>
    /// <param name="phaseLabels">Phase labels of the Phases for which the properties 
    /// are to be calculated. The Phase labels must be two of the identifiers returned 
    /// by the GetPhaseList method on the ICapeThermoPhases interface.</param>
    /// <returns> A boolean Set to True if the combination of property and
    /// phaseLabels is supported, or False if not supported.</returns>
    /// <remarks><para>The result of the check should only depend on the capabilities and 
    /// configuration (Compounds and Phases present) of the component that implements 
    /// the ICapeThermoPropertyRoutine interface (eg. a Property Package). It should 
    /// not depend on whether a Material Object has been set nor on the state 
    /// (temperature, pressure, composition etc.), or configuration of a Material 
    /// Object that might be set.</para>
    /// <para>It is expected that the PME, or other client, will use this method to 
    /// check whether the properties it requires are supported by the Property Package 
    /// when the Property Package is imported. If any essential properties are not 
    /// available, the import process should be aborted.</para>
    /// <para>If either the property argument or the values in the phaseLabels 
    /// arguments are not recognised by the component that implements the 
    /// ICapeThermoPropertyRoutine interface this method should return False.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation CheckTwoPhasePropSpec is “not” 
    /// implemented even if this method can be called for reasons of compatibility with 
    /// the CAPE-OPEN standards. That is to say that the operation exists, but it is 
    /// not supported by the current implementation. This may be the case if no 
    /// two-phase property is supported.</exception>
    /// <exception cref="ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. The ICapeThermoMaterial 
    /// interface has not been passed via a SetMaterial call prior to calling this 
    /// method.</exception>
    /// <exception cref="ECapeFailedInitialisation">The pre-requisites for the 
    /// property calculation are not valid. For example, if a prior call to the 
    /// SetMaterial method of the ICapeThermoMaterialContext interface has failed to 
    /// provide a valid Material Object.</exception>
    /// <exception cref="ECapeInvalidArgument">One or more of the input arguments is 
    /// not valid. For example, UNDEFINED value for the property argument or the 
    /// phaseLabels argument or number of elements in phaseLabels array not equal to 
    /// two.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the CheckTwoPhasePropSpec operation, are not suitable.</exception>
    bool ICapeThermoPropertyRoutine.CheckTwoPhasePropSpec(string property, string[] phaseLabels)
    {
        return _pIPropertyRoutine.CheckTwoPhasePropSpec(property, phaseLabels);
    }

    /// <summary>Returns the list of supported non-constant single-phase Physical 
    /// Properties.</summary>
    /// <returns>List of all supported non-constant single-phase property identifiers. 
    /// The standard single-phase property identifiers are listed in section 7.5.5.
    /// </returns>
    /// <remarks><para>A non-constant property depends on the state of the Material Object. </para>
    /// <para>Single-phase properties, e.g. enthalpy, only depend on the state of one 
    /// phase. GetSinglePhasePropList must return all the single-phase properties that 
    /// can be calculated by CalcSinglePhaseProp. If derivatives can be calculated 
    /// these must also be returned.</para>
    /// <para>If no single-phase properties are supported this method should return 
    /// UNDEFINED.</para>
    /// <para>To get the list of supported two-phase properties, use 
    /// GetTwoPhasePropList.</para>
    /// <para>A component that implements this method may return non-constant 
    /// single-phase property identifiers which do not belong to the list defined in 
    /// section 7.5.5. However, these proprietary identifiers may not be understood by 
    /// most of the clients of this component.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported by 
    /// the current implementation.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetSinglePhasePropList operation, are not suitable.</exception>
    [DispId(0x00000006)]
    [Description("Method GetSinglePhasePropList")]
    string[] ICapeThermoPropertyRoutine.GetSinglePhasePropList()
    {
        return (string[])_pIPropertyRoutine.GetSinglePhasePropList();
    }

    /// <summary>Returns the list of supported non-constant two-phase properties.</summary>
    /// <returns>List of all supported non-constant two-phase property identifiers. 
    /// The standard two-phase property identifiers are listed in section 7.5.6.</returns>
    /// <remarks><para>A non-constant property depends on the state of the Material Object. 
    /// Two-phase properties are those that depend on more than one co-existing phase, 
    /// e.g. K-values.</para>
    /// <para>GetTwoPhasePropList must return all the properties that can be calculated 
    /// by CalcTwoPhaseProp. If derivatives can be calculated, these must also be 
    /// returned.</para>
    /// <para>If no two-phase properties are supported this method should return 
    /// UNDEFINED.</para>
    /// <para>To check whether a property can be evaluated for a particular set of 
    /// phase labels use the CheckTwoPhasePropSpec method.</para>
    /// <para>A component that implements this method may return non-constant 
    /// two-phase property identifiers which do not belong to the list defined in 
    /// section 7.5.6. However, these proprietary identifiers may not be understood by 
    /// most of the clients of this component.</para>
    /// <para>To get the list of supported single-phase properties, use 
    /// GetSinglePhasePropList.</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if this
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetTwoPhasePropList operation, are not suitable.</exception>
    [DispId(0x00000007)]
    [Description("Method GetTwoPhasePropList")]
    string[] ICapeThermoPropertyRoutine.GetTwoPhasePropList()
    {
        return (string[])_pIPropertyRoutine.GetTwoPhasePropList();
    }

    /// <summary> CalcEquilibrium is used to calculate the amounts and compositions 
    /// of Phases at equilibrium. CalcEquilibrium will calculate temperature and/or 
    /// pressure if these are not among the two specifications that are mandatory for 
    /// each Equilibrium Calculation considered.</summary>
    /// <remarks><para>The specification1 and specification2 arguments provide the information 
    /// necessary to retrieve the values of two specifications, for example the 
    /// pressure and temperature, for the Equilibrium Calculation. The CheckEquilibriumSpec 
    /// method can be used to check for supported specifications. Each specification 
    /// variable contains a sequence of strings in the order defined in the following 
    /// table (hence, the specification arguments may have 3 or 4 items):<para>
    /// <para>property identifier The property identifier can be any of the identifiers 
    /// listed in section 7.5.5 but only certain property specifications will normally 
    /// be supported by any Equilibrium Routine.</para>
    /// basis The basis for the property value. Valid settings for basis are given in 
    /// section 7.4. Use UNDEFINED as a placeholder for a property for which basis does
    /// not apply. For most Equilibrium Specifications, the result of the calculation
    /// is not dependent on the basis, but, for example, for phase fraction 
    /// specifications the basis (Mole or Mass) does make a difference.</para>
    /// <para>phase label The phase label denotes the Phase to which the specification 
    /// applies. It must either be one of the labels returned by GetPresentPhases, or 
    /// the special value “Overall”.</para>
    /// compound identifier (optional)The compound identifier allows for specifications 
    /// that depend on a particular Compound. This item of the specification array is 
    /// optional and may be omitted. In case of a specification without compound 
    /// identifier, the array element may be present and empty, or may be absent.</para>
    /// <para>Some examples of typical phase equilibrium specifications are given in 
    /// the table below.</para>
    /// <para>The values corresponding to the specifications in the argument list and 
    /// the overall composition of the mixture must be set in the associated Material 
    /// Object before a call to CalcEquilibrium.</para>
    /// <para>Components such as a Property Package or an Equilibrium Calculator must 
    /// implement the ICapeThermoMaterialContext interface, so that an 
    /// ICapeThermoMaterial interface can be passed via the SetMaterial method. It is 
    /// the responsibility of the implementation of CalcEquilibrium to validate the 
    /// Material Object before attempting a calculation.</para>
    /// <para>The Phases that will be considered in the Equilibrium Calculation are 
    /// those that exist in the Material Object, i.e. the list of phases specified in 
    /// a SetPresentPhases call. This provides a way for a client to specify whether, 
    /// for example, a vapour-liquid, liquid-liquid, or vapourliquid-liquid calculation 
    /// is required. CalcEquilibrium must use the GetPresentPhases method to retrieve 
    /// the list of Phases and the associated Phase status flags. The Phase status 
    /// flags may be used by the client to provide information about the Phases, for 
    /// example whether estimates of the equilibrium state are provided. See the 
    /// description of the GetPresentPhases and SetPresentPhases methods of the 
    /// ICapeThermoMaterial interface for details. When the Equilibrium Calculation 
    /// has been completed successfully, the SetPresentPhases method must be used to 
    /// specify which Phases are present at equilibrium and the Phase status flags for 
    /// the phases should be set to Cape_AtEquilibrium. This must include any Phases 
    /// that are present in zero amount such as the liquid Phase in a dew point 
    /// calculation.</para>
    /// <para>Some types of Phase equilibrium specifications may result in more than 
    /// one solution. A common example of this is the case of a dew point calculation. 
    /// However, CalcEquilibrium can provide only one solution through the Material 
    /// Object. The solutionType argument allows the “Normal” or “Retrograde” solution 
    /// to be explicitly requested. When none of the specifications includes a phase 
    /// fraction, the solutionType argument should be set to “Unspecified”.</para>
    /// <para>The definition of “Normal” is</para>
    /// <para>where V F is the vapour phase fraction and the derivatives are at 
    /// equilibrium states. For “Retrograde” behaviour,</para>
    /// <para>CalcEquilibrium must set the amounts, compositions, temperature and 
    /// pressure for all Phases present at equilibrium, as well as the temperature and 
    /// pressure for the overall mixture if not set as part of the calculation 
    /// specifications. CalcEquilibrium must not set any other Physical Properties.</para>
    /// <para>As an example, the following sequence of operations might be performed 
    /// by CalcEquilibrium in the case of an Equilibrium Calculation at fixed pressure 
    /// and temperature:</para>
    /// <para>- With the ICapeThermoMaterial interface of the supplied Material Object:</para>
    /// <para>- Use the GetPresentPhases method to find the list of Phases that the 
    /// Equilibrium Calculation should consider.</para>
    /// <para>- With the ICapeThermoCompounds interface of the Material Object use the
    /// GetCompoundIds method to find which Compounds are present.</para>
    /// <para>- Use the GetOverallProp method to get the temperature, pressure and 
    /// composition for the overall mixture.</para>
    /// <para>- Perform the Equilibrium Calculation.</para>
    /// <para>- Use SetPresentPhases to specify the Phases present at equilibrium and 
    /// set the Phase status flags to Cape_AtEquilibrium.</para>
    /// <para>- Use SetSinglePhaseProp to set pressure, temperature, Phase amount 
    /// (or Phase fraction) and composition for all Phases present.</para></remarks>
    /// <param name="specification1">First specification for the Equilibrium 
    /// Calculation. The specification information is used to retrieve the value of
    /// the specification from the Material Object. See below for details.</param>
    /// <param name="specification2">Second specification for the Equilibrium 
    /// Calculation in the same format as specification1.</param>
    /// <param name="solutionType"><para>The identifier for the required solution type. 
    /// The standard identifiers are given in the following list:</para>
    /// <para>Unspecified</para>
    /// <para>Normal</para>
    /// <para>Retrograde</para>
    /// <para>The meaning of these terms is defined below in the notes. Other 
    /// identifiers may be supported but their interpretation is not part of the CO 
    /// standard.</para></param>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if this 
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref="ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. The ICapeThermoMaterial interface 
    /// has not been passed via a SetMaterial call prior to calling this method.</exception>
    /// <exception cref="ECapeSolvingError">The Equilibrium Calculation could not be 
    /// solved. For example if the solver has run out of iterations, or has converged 
    /// to a trivial solution.</exception>
    /// <exception cref="ECapeLimitedImpl">Would be raised if the Equilibrium Routine 
    /// is not able to perform the flash it has been asked to perform. For example, 
    /// the values given to the input specifications are valid, but the routine is not 
    /// able to perform a flash given a temperature and a Compound fraction. That 
    /// would imply a bad usage or no usage of CheckEquilibriumSpec method, which is 
    /// there to prevent calling CalcEquilibrium for a calculation which cannot be
    /// performed.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed. It would be raised, for example, if a specification 
    /// identifier does not belong to the list of recognised identifiers. It would 
    /// also be raised if the value given to argument solutionType is not among 
    /// the three defined, or if UNDEFINED was used instead of a specification identifier.</exception>
    /// <exception cref="ECapeFailedInitialisation"><para>The pre-requisites for the Equilibrium 
    /// Calculation are not valid. For example:</para>
    /// <para>• The overall composition of the mixture is not defined.</para>
    /// <para>• The Material Object (set by a previous call to the SetMaterial method of the
    /// ICapeThermoMaterialContext interface) is not valid. This could be because no 
    /// Phases are present or because the Phases present are not recognised by the
    /// component that implements the ICapeThermoEquilibriumRoutine interface.</para>
    /// <para>• Any other necessary input information is not available.</para></exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    void ICapeThermoEquilibriumRoutine.CalcEquilibrium(string[] specification1, string[] specification2,
        string solutionType)
    {
        _pIEquilibriumRoutine.CalcEquilibrium(specification1, specification2, solutionType);
    }


    /// <summary>Checks whether the Property Package can support a particular type of 
    /// Equilibrium Calculation.</summary>
    /// <remarks><para>The meaning of the specification1, specification2 and solutionType 
    /// arguments is the same as for the CalcEquilibrium method.</para>
    /// <para>The result of the check should only depend on the capabilities and 
    /// configuration (compounds and phases present) of the component that implements 
    /// the ICapeThermoEquilibriumRoutine interface (eg. a Property package). It should 
    /// not depend on whether a Material Object has been set nor on the state 
    /// (temperature, pressure, composition etc.) or configuration of a Material 
    /// Object that might be set.</para>
    /// <para>If solutionType, specification1 and specification2 arguments appear 
    /// valid but the actual specifications are not supported or not recognised a 
    /// False value should be returned.</para></remarks>
    /// <param name="specification1">First specification for the Equilibrium 
    /// Calculation.</param>
    /// <param name="specification2">Second specification for the Equilibrium 
    /// Calculation.</param>
    /// <param name="solutionType">The required solution type.</param>
    /// <returns>Set to True if the combination of specifications and solutionType is 
    /// supported or False if not supported.</returns>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if this 
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example UNDEFINED for solutionType, specification1 or 
    /// specification2 argument.</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不适用时，将引发此错误。</exception>
    bool ICapeThermoEquilibriumRoutine.CheckEquilibriumSpec(string[] specification1, string[] specification2,
        string solutionType)
    {
        return _pIEquilibriumRoutine.CheckEquilibriumSpec(specification1, specification2, solutionType);
    }
}