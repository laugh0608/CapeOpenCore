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
    public override String ComponentDescription
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
    /// <summary>Remove all stored Physical Property values.</summary>
    /// <remarks><para>
    /// ClearAllProps removes all stored Physical Properties that have been set 
    /// using the SetSinglePhaseProp, SetTwoPhaseProp or SetOverallProp methods. 
    /// This means that any subsequent call to retrieve Physical Properties will 
    /// result in an exception until new values have been stored using one of the 
    /// Set methods. ClearAllProps does not remove the configuration information 
    /// for a Material, i.e. the list of Compounds and Phases.
    /// </para>
    /// <para>
    /// Using the ClearAllProps method results in a Material Object that is in 
    /// the same state as when it was first created. It is an alternative to using 
    /// the CreateMaterial method but it is expected to have a smaller overhead in 
    /// operating system resources.
    /// </para></remarks>
    /// <exception cref = "ECapeNoImpl">The operation is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.ClearAllProps()
    {
        _pIMatObj.ClearAllProps();
    }

    /// <summary>Copies all the stored non-constant Physical Properties (which have been set 
    /// using the SetSinglePhaseProp, SetTwoPhaseProp or SetOverallProp) from the 
    /// source Material Object to the current instance of the Material Object.</summary>
    /// <remarks><para>Before using this method, the Material Object must have been configured 
    /// with the same exact list of Compounds and Phases as the source one. Otherwise, 
    /// calling the method will raise an exception. There are two ways to perform the 
    /// configuration: through the PME proprietary mechanisms and with 
    /// CreateMaterial. Calling CreateMaterial on a Material Object S and 
    /// subsequently calling CopyFromMaterial(S) on the newly created Material 
    /// Object N is equivalent to the deprecated method ICapeMaterialObject.Duplicate.
    /// </para>
    /// <para>The method is intended to be used by a client, for example a Unit 
    /// Operation that needs a Material Object to have the same state as one of the 
    /// Material Objects it has been connected to. One example is the representation 
    /// of an internal stream in a distillation column.</para></remarks>
    /// <param name="source">
    /// Source Material Object from which stored properties will be copied.
    /// </param>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even 
    /// if this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites for copying 
    /// the non-constant Physical Properties of the Material Object are not valid. 
    /// The necessary initialisation, such as configuring the current Material with 
    /// the same Compounds and Phases as the source, has not been performed or has 
    /// failed.</exception>
    /// <exception cref = "ECapeOutOfResources">The physical resources necessary to 
    /// copy the non-constant Physical Properties are out of limits.</exception>
    /// <exception cref = "ECapeNoMemory">The physical memory necessary to copy the 
    /// non-constant Physical Properties is out of limit.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.CopyFromMaterial(ICapeThermoMaterial source)
    {
        _pIMatObj.CopyFromMaterial(((MaterialObjectWrapper)source).MaterialObject11);
    }

    /// <summary>Creates a Material Object with the same configuration as the current 
    /// Material Object.</summary>
    /// <remarks>The Material Object created does not contain any non-constant Physical 
    /// Property value but has the same configuration (Compounds and Phases) as 
    /// the current Material Object. These Physical Property values must be set 
    /// using SetSinglePhaseProp, SetTwoPhaseProp or SetOverallProp. Any attempt to 
    /// retrieve Physical Property values before they have been set will result in 
    /// an exception.</remarks>
    /// <returns>
    /// The interface for the Material Object.
    /// </returns>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The physical resources 
    /// necessary to the creation of the Material Object are out of limits.
    /// </exception>
    /// <exception cref = "ECapeOutOfResources">The operation is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation</exception>
    /// <exception cref = "ECapeNoMemory">The physical memory necessary to the 
    /// creation of the Material Object is out of limit.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
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
    /// implements the ICapeThermoEquilibriumRoutine interface.
    /// </para>
    /// <para>It is expected that this method will normally be able to provide 
    /// Physical Property values on any basis, i.e. it should be able to convert 
    /// values from the basis on which they are stored to the basis requested. This 
    /// operation will not always be possible. For example, if the molecular weight 
    /// is not known for one or more Compounds, it is not possible to convert 
    /// between a mass basis and a molar basis.
    /// </para>
    /// <para>Although the result of some calls to GetOverallProp will be a single 
    /// value, the return type is CapeArrayDouble and the method must always return 
    /// an array even if it contains only a single element.</para></remarks>
    /// <param name="results"> A double array containing the results vector of 
    /// Physical Property value(s) in SI units.</param>
    /// <param name="property">A String identifier of the Physical Property for 
    /// which values are requested. This must be one of the single-phase Physical 
    /// Properties or derivatives that can be stored for the overall mixture. The 
    /// standard identifiers are listed in sections 7.5.5 and 7.6.
    /// </param>
    /// <param name="basis">A String indicating the basis of the results. Valid 
    /// settings are: “Mass” for Physical Properties per unit mass or “Mole” for 
    /// molar properties. Use UNDEFINED as a place holder for a Physical Property 
    /// for which basis does not apply. See section 7.5.5 for details.
    /// </param>
    /// <exception cref = "ECapeNoImpl">The operation GetOverallProp is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">The Physical Property 
    /// required is not available from the Material Object, possibly for the basis 
    /// requested. This exception is raised when a Physical Property value has not 
    /// been set following a call to the CreateMaterial or ClearAllProps methods.
    /// </exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, for example UNDEFINED for property.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed or has failed.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetOverallProp(String property, String basis, ref double[] results)
    {
        Object obj1 = null;
        _pIMatObj.GetOverallProp(property, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>Retrieves temperature, pressure and composition for the overall mixture.</summary>
    /// <remarks><para>
    ///This method is provided to make it easier for developers to make efficient 
    /// use of the CAPEOPEN interfaces. It returns the most frequently requested 
    /// information from a Material Object in a single call.
    /// </para>
    /// <para>
    /// There is no choice of basis in this method. The composition is always 
    /// returned as mole fractions.
    /// </para></remarks>
    /// <param name="temperature">A reference to a double Temperature (in K)</param>
    /// <param name="pressure">A reference to a double Pressure (in Pa)</param>
    /// <param name="composition">A reference to an array of doubles containing 
    /// the  Composition (mole fractions)</param>
    /// <exception cref = "ECapeNoImpl">The operation GetOverallProp is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">The Physical Property 
    /// required is not available from the Material Object, possibly for the basis 
    /// requested. This exception is raised when a Physical Property value has not 
    /// been set following a call to the CreateMaterial or ClearAllProps methods.
    /// </exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed or has failed.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetOverallTPFraction(ref double temperature, ref double pressure, ref double[] composition)
    {
        Object obj1 = null;
        _pIMatObj.GetOverallTPFraction(temperature, pressure, ref obj1);
        composition = (double[])obj1;
    }

    /// <summary>Returns Phase labels for the Phases that are currently present in the 
    /// Material Object.</summary>
    /// <remarks><para>
    /// This method is intended to work in conjunction with the SetPresentPhases 
    /// method. Together these methods provide a means of communication between a 
    /// PME (or another client) and an Equilibrium Calculator (or other component 
    /// that implements the ICapeThermoEquilibriumRoutine interface). The following 
    /// sequence of operations is envisaged.
    /// </para>
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
    /// there is no guarantee that they will be used.
    /// </para>
    /// <para>
    /// Using the ClearAllProps method results in a Material Object that is in 
    /// the same state as when it was first created. It is an alternative to using 
    /// the CreateMaterial method but it is expected to have a smaller overhead in 
    /// operating system resources.
    /// </para></remarks>
    /// <param name="phaseLabels">A reference to a String array that contains the 
    /// list of Phase labels (identifiers – names) for the Phases present in the 
    /// Material Object. The Phase labels in the Material Object must be a
    /// subset of the labels returned by the GetPhaseList method of the 
    /// ICapeThermoPhases interface.</param>
    /// <param name="phaseStatus">A CapeArrayEnumeration which is an array of 
    /// Phase status flags corresponding to each of the Phase labels. 
    /// See description below.</param>
    /// <exception cref = "ECapeNoImpl">The operation is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetPresentPhases(ref String[] phaseLabels,
        ref CapePhaseStatus[] phaseStatus)
    {
        Object obj1 = null;
        Object obj2 = null;
        _pIMatObj.GetPresentPhases(ref obj1, ref obj2);
        phaseLabels = (String[])obj1;
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">The property required is 
    /// not available from the Material Object possibly for the Phase label or 
    /// basis requested. This exception is raised when a property value has not been 
    /// set following a call to the CreateMaterial or the value has been erased by 
    /// a call to the ClearAllProps methods.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed: for example UNDEFINED for property, or an unrecognised 
    /// identifier for phaseLabel.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed, or has failed. 
    /// This exception is returned if the Phase specified does not exist.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetSinglePhaseProp(String property, String phaseLabel, String basis, ref double[] results)
    {
        Object obj1 = null;
        _pIMatObj.GetSinglePhaseProp(property, phaseLabel, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>Retrieves temperature, pressure and composition for a Phase.</summary>
    /// <remarks><para>
    /// This method is provided to make it easier for developers to make efficient 
    /// use of the CAPEOPEN interfaces. It returns the most frequently requested 
    /// information from a Material Object in a single call.
    /// </para>
    /// <para>There is no choice of basis in this method. The composition is always 
    /// returned as mole fractions.
    /// </para>
    /// <para>To get the equivalent information for the overall mixture the 
    /// GetOverallTPFraction method of the ICapeThermoMaterial interface should be 
    /// used.
    /// </para></remarks>
    /// <returns>
    /// No return.
    /// </returns>
    /// <param name="phaseLabel">Phase label of the Phase for which the property 
    /// is required. The Phase label must be one of the identifiers returned by the 
    /// GetPresentPhases method of this interface.</param>
    /// <param name="temperature">Temperature (in K)</param>
    /// <param name="pressure">Pressure (in Pa)</param>
    /// <param name="composition">Composition (mole fractions)</param>
    /// <exception cref = "ECapeNoImpl">The operation GetTPFraction is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">One of the properties is 
    /// not available from the Material Object. This exception is raised when a 
    /// property value has not been set following a call to the CreateMaterial or 
    /// the value has been erased by a call to the ClearAllProps methods.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed: for example UNDEFINED for property, or an unrecognised 
    /// identifier for phaseLabel.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed, or has failed. 
    /// This exception is returned if the Phase specified does not exist.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetTPFraction(String phaseLabel, ref double temperature, ref double pressure,
        ref double[] composition)
    {
        Object obj1 = null;
        _pIMatObj.GetTPFraction(phaseLabel, temperature, pressure, obj1);
        composition = (double[])obj1;
    }

    /// <summary>Retrieves two-phase non-constant Physical Property values for a mixture.</summary>
    /// <remarks><para>
    ///The results argument returned by GetTwoPhaseProp is either a CapeArrayDouble 
    /// that contains one or more numerical values, e.g. kvalues, or a CapeInterface 
    /// that may be used to retrieve 2-phase Physical Properties described by a more 
    /// complex data structure, e.g.distributed Physical Properties.
    /// </para>
    /// <para>Although the result of some calls to GetTwoPhaseProp may be a single 
    /// numerical value, the return type for numerical values is CapeArrayDouble and 
    /// in such a case the method must return an array even if it contains only a 
    /// single element.
    ///</para>
    /// <para>A Phase is ‘present’ in a Material if its identifier is returned by 
    /// the GetPresentPhases method. An exception is raised by the GetTwoPhaseProp 
    /// method if any of the Phases specified is not present. Even if all Phases are 
    /// present, this does not mean that any Physical Properties are available.
    ///</para>
    /// <para>The Physical Property values returned by GetTwoPhaseProp depend on two 
    /// Phases, for example surface tension or K-values. These values may be set by 
    /// the SetTwoPhaseProp method that may be called directly, or by other methods 
    /// such as the CalcTwoPhaseProp method of the ICapeThermoPropertyRoutine 
    /// interface, or the CalcEquilibrium method of the ICapeThermoEquilibriumRoutine 
    /// interface. Note: Physical Properties that depend on a single Phase are 
    /// returned by the GetSinglePhaseProp method.
    ///</para>
    /// <para>It is expected that this method will normally be able to provide 
    /// Physical Property values on any basis, i.e. it should be able to convert 
    /// values from the basis on which they are stored to the basis requested. This 
    /// operation will not always be possible. For example, if the molecular weight 
    /// is not known for one or more Compounds, it is not possible to convert between 
    /// a mass basis and a molar basis.
    ///</para>
    /// <para>If a composition derivative is requested this means that the 
    /// derivatives are returned for both Phases in the order in which the Phase 
    /// labels are specified. The number of values returned for a composition 
    /// derivative will depend on the dimensionality of the property. For example,
    /// if there are N Compounds then the results vector for the surface tension 
    /// derivative will contain N composition derivative values for the first Phase, 
    /// followed by N composition derivative values for the second Phase. For K-value 
    /// derivative there will be N2 derivative values for the first phase followed by 
    /// N2 values for the second phase in the order defined in 7.6.2. 
    ///</para></remarks>
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation. This could be the case if two-phase non-constant 
    /// Physical Properties are not required by the PME and so there is no particular 
    /// need to implement this method.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">The property required is 
    /// not available from the Material Object possibly for the Phases or basis 
    /// requested.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. This exception is raised when a call to the SetTwoPhaseProp method 
    /// has not been performed, or has failed, or when one or more of the Phases 
    /// referenced does not exist.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed: for example, UNDEFINED for property, or an unrecognised 
    /// identifier in phaseLabels.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetTwoPhaseProp(String property, String[] phaseLabels, String basis, ref double[] results)
    {
        Object obj1 = null;
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation. This method may not be required if the PME 
    /// does not deal with any single-phase property.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, that is a value that does not belong to the valid list 
    /// described above, for example UNDEFINED for property.</exception>
    /// <exception cref = "ECapeOutOfBounds">One or more of the entries in the 
    /// values argument is outside of the range of values accepted by the Material 
    /// Object.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the SetSinglePhaseProp operation, are not suitable.</exception>
    void ICapeThermoMaterial.SetOverallProp(String property, String basis, double[] values)
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, that is a value that does not belong to the valid list 
    /// described above, for example if phaseLabels contains UNDEFINED or 
    /// phaseStatus contains a value that is not in the above table.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.SetPresentPhases(String[] phaseLabels, CapePhaseStatus[] phaseStatus)
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists but it is not supported by
    /// the current implementation. This method may not be required if the PME does 
    /// not deal with any single-phase properties.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, that is a value that does not belong to the valid list 
    /// described above, for example UNDEFINED for property.</exception> 
    /// <exception cref = "ECapeOutOfBounds">One or more of the entries in the 
    /// values argument is outside of the range of values accepted by the Material 
    /// Object.</exception> 
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The phase referenced has not been created using SetPresentPhases.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the SetSinglePhaseProp operation, are not suitable.</exception>
    void ICapeThermoMaterial.SetSinglePhaseProp(String prop, String phaseLabel, String basis, double[] values)
    {
        _pIMatObj.SetSinglePhaseProp(prop, phaseLabel, basis, values);
    }

    /// <summary>Sets two-phase non-constant property values for a mixture.</summary>
    /// <remarks><para>The values argument of SetTwoPhaseProp is either a CapeArrayDouble that 
    /// contains one or more numerical values to be set for a property, e.g. kvalues, 
    /// or a CapeInterface that may be used to set two-phase properties described by 
    /// a more complex data structure, e.g. distributed properties.</para>
    /// <para>Although some properties set by calls to SetTwoPhaseProp will have a 
    /// single numerical value, the type of the values argument for numerical values 
    /// is CapeArrayDouble and in such a case the method must be called with the 
    /// values argument containing an array even if it contains only a single element.</para>
    /// <para>The Physical Property values set by SetTwoPhaseProp depend on two 
    /// Phases, for example surface tension or K-values. Properties that depend on a 
    /// single Phase are set by the SetSinglePhaseProp method.</para>
    /// <para>If a Physical Property with composition derivative is specified, the 
    /// derivative values will be set for both Phases in the order in which the Phase 
    /// labels are specified. The number of values returned for a composition 
    /// derivative will depend on the property. For example, if there are N Compounds 
    /// then the values vector for the surface tension derivative will contain N 
    /// composition derivative values for the first Phase, followed by N composition 
    /// derivative values for the second Phase. For K-values there will be N2 
    /// derivative values for the first phase followed by N2 values for the second 
    /// phase in the order defined in 7.6.2.</para>
    /// <para>Before SetTwoPhaseProp can be used, all the Phases referenced must have 
    /// been created using the SetPresentPhases method</para></remarks>
    /// <param name="property">The property for which values are set in the 
    /// Material Object. This must be one of the two-phase properties or derivatives 
    /// included in sections 7.5.6 and 7.6.</param>
    /// <param name="phaseLabels">Phase labels of the Phases for 
    /// which the property is set. The Phase labels must be two of the identifiers 
    /// returned by the GetPhaseList method of the ICapeThermoPhases interface.</param>
    /// <param name="basis">Basis of the results. Valid settings are: “Mass” for
    /// Physical Properties per unit mass or “Mole” for molar properties. Use 
    /// UNDEFINED as a place holder for a Physical Property for which basis does not 
    /// apply. See section 7.5.5 for details.</param>
    /// <param name="values">Value(s) to set for the property (CapeArrayDouble) or
    /// CapeInterface (see notes).</param>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists but it is not supported by
    /// the current implementation. This method may not be required if the PME does 
    /// not deal with any single-phase properties.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, that is a value that does not belong to the valid list 
    /// described above, for example UNDEFINED for property.</exception> 
    /// <exception cref = "ECapeOutOfBounds">One or more of the entries in the 
    /// values argument is outside of the range of values accepted by the Material 
    /// Object.</exception> 
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The phase referenced has not been created using SetPresentPhases.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the SetSinglePhaseProp operation, are not suitable.</exception>
    void ICapeThermoMaterial.SetTwoPhaseProp(String property, String[] phaseLabels, String basis, double[] values)
    {
        _pIMatObj.SetTwoPhaseProp(property, phaseLabels, basis, values);
    }

    /// <summary>Sets two-phase non-constant property values for a mixture.</summary>
    /// <remarks><para>The values argument of SetTwoPhaseProp is either a CapeArrayDouble that 
    /// contains one or more numerical values to be set for a property, e.g. kvalues, 
    /// or a CapeInterface that may be used to set two-phase properties described by 
    /// a more complex data structure, e.g. distributed properties.</para>
    /// <para>Although some properties set by calls to SetTwoPhaseProp will have a 
    /// single numerical value, the type of the values argument for numerical values 
    /// is CapeArrayDouble and in such a case the method must be called with the 
    /// values argument containing an array even if it contains only a single element.</para>
    /// <para>The Physical Property values set by SetTwoPhaseProp depend on two 
    /// Phases, for example surface tension or K-values. Properties that depend on a 
    /// single Phase are set by the SetSinglePhaseProp method.</para>
    /// <para>If a Physical Property with composition derivative is specified, the 
    /// derivative values will be set for both Phases in the order in which the Phase 
    /// labels are specified. The number of values returned for a composition 
    /// derivative will depend on the property. For example, if there are N Compounds 
    /// then the values vector for the surface tension derivative will contain N 
    /// composition derivative values for the first Phase, followed by N composition 
    /// derivative values for the second Phase. For K-values there will be N2 
    /// derivative values for the first phase followed by N2 values for the second 
    /// phase in the order defined in 7.6.2.</para>
    /// <para>Before SetTwoPhaseProp can be used, all the Phases referenced must have 
    /// been created using the SetPresentPhases method</para>
    /// <para>The values argument of SetTwoPhaseProp is either a CapeArrayDouble that contains one or
    /// more numerical values to be set for a property, e.g. kvalues, or a CapeInterused to set two-phase 
    /// properties described by a more complex data structureproperties.</para></remarks>
    /// <param name="property">The property for which values are set in the 
    /// Material Object. This must be one of the two-phase properties or derivatives 
    /// included in sections 7.5.6 and 7.6.</param>
    /// <param name="phaseLabels">Phase labels of the Phases for 
    /// which the property is set. The Phase labels must be two of the identifiers 
    /// returned by the GetPhaseList method of the ICapeThermoPhases interface.</param>
    /// <param name="basis">Basis of the results. Valid settings are: “Mass” for
    /// Physical Properties per unit mass or “Mole” for molar properties. Use 
    /// UNDEFINED as a place holder for a Physical Property for which basis does not 
    /// apply. See section 7.5.5 for details.</param>
    /// <param name="values">Value(s) to set for the property (CapeArrayDouble) or
    /// CapeInterface (see remarks).</param>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists but it is not supported by
    /// the current implementation. This method may not be required if the PME does 
    /// not deal with any single-phase properties.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, that is a value that does not belong to the valid list 
    /// described above, for example UNDEFINED for property.</exception> 
    /// <exception cref = "ECapeOutOfBounds">One or more of the entries in the 
    /// values argument is outside of the range of values accepted by the Material 
    /// Object.</exception> 
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The phase referenced has not been created using SetPresentPhases.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the SetSinglePhaseProp operation, are not suitable.</exception>
    [Description("method SetTwoPhaseProp")]
    void ICapeThermoMaterial.SetTwoPhaseProp(String property, string[] phaseLabels, String basis, object values)
    {
        _pIMatObj.SetTwoPhaseProp(property, phaseLabels, basis, values);
    }

    /// <summary>Retrieves the value of a Universal Constant.</summary>
    /// <param name="constantId">Identifier of Universal Constant. The list of 
    /// constants supported should be obtained by using the GetUniversalConstList 
    /// method.</param>
    /// <returns>Value of Universal Constant. This could be a numeric or a string 
    /// value. For numeric values the units of measurement are specified in section 
    /// 7.5.1.</returns>
    /// <remarks>Universal Constants (often called fundamental constants) are 
    /// quantities like the gas constant, or the Avogadro constant.</remarks>
    /// <exception cref = "ECapeNoImpl">The operation GetUniversalConstant is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists, but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeInvalidArgument">For example, UNDEFINED for constantId 
    /// argument is used, or value for constantId argument does not belong to the 
    /// list of recognised values.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetUniversalConstant operation, are not suitable.</exception>	
    object ICapeThermoUniversalConstant.GetUniversalConstant(String constantId)
    {
        return (double)_pIUniversalConstant.GetUniversalConstant(constantId);
    }

    /// <summary>Returns the identifiers of the supported Universal Constants.</summary>
    /// <returns>List of identifiers of Universal Constants. The list of standard 
    /// identifiers is given in section 7.5.1.</returns>
    /// <remarks>A component may return Universal Constant identifiers that do not 
    /// belong to the list defined in section 7.5.1. However, these proprietary 
    /// identifiers may not be understood by most of the clients of this component.</remarks>
    /// <exception cref = "ECapeNoImpl">The operation GetUniversalConstantList is 
    /// “not” implemented even if this method can be called for reasons of 
    /// compatibility with the CAPE-OPEN standards. That is to say that the operation 
    /// exists, but it is not supported by the current implementation. This may occur 
    /// when the Property Package does not support any Universal Constants, or if it
    /// does not want to provide values for any Universal Constants which may be used 
    /// within the Property Package.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetUniversalConstantList operation, are not suitable.
    /// </exception>
    String[] ICapeThermoUniversalConstant.GetUniversalConstantList()
    {
        return (String[])_pIUniversalConstant.GetUniversalConstantList();
    }

    /// <summary>Returns the number of Phases.</summary>
    /// <returns>The number of Phases supported.</returns>
    /// <remarks>The number of Phases returned by this method must be equal to the 
    /// number of Phase labels that are returned by the GetPhaseList method of this
    /// interface. It must be zero, or a positive number.</remarks>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported
    /// by the current implementation.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for this operation, are not suitable.</exception>
    int ICapeThermoPhases.GetNumPhases()
    {
        return _pIPhases.GetNumPhases();
    }

    /// <summary>Returns information on an attribute associated with a Phase for the 
    /// purpose of understanding what lies behind a Phase label.</summary>
    /// <param name ="phaseLabel">A (single) Phase label. This must be one of the 
    /// values returned by GetPhaseList method.</param>
    /// <param name ="phaseAttribute">One of the Phase attribute identifiers from the 
    /// table below.</param>
    /// <returns>The value corresponding to the Phase attribute identifier – see 
    /// table below.</returns>
    /// <remarks><para>GetPhaseInfo is intended to allow a PME, or other client, to identify a
    /// Phase with an arbitrary label. A PME, or other client, will need to do this 
    /// to map stream data into a Material Object, or when importing a Property 
    /// Package. If the client cannot identify the Phase, it can ask the user to 
    /// provide a mapping based on the values of these properties.</para>
    /// <para>The list of supported Phase attributes is defined in the following 
    /// table:</para>
    /// <para>For example, the following information might be returned by a Property 
    /// Package component that supports a vapour Phase, an organic liquid Phase and 
    /// an aqueous liquid Phase:
    /// Phase label Gas Organic Aqueous
    /// StateOfAggregation Vapor Liquid Liquid
    /// KeyCompoundId UNDEFINED UNDEFINED Water
    /// ExcludedCompoundId UNDEFINED Water UNDEFINED
    /// DensityDescription UNDEFINED Light Heavy
    /// UserDescription The gas Phase The organic liquid
    /// Phase
    /// The aqueous liquid
    /// Phase 
    /// TypeOfSolid UNDEFINED UNDEFINED UNDEFINED</para></remarks>
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists but it is not supported 
    /// by the current implementation..</exception>
    /// <exception cref="ECapeInvalidArgument"> – phaseLabel is not recognised, or 
    /// UNDEFINED, or phaseAttribute is not recognised.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for this operation, are not suitable..</exception>
    String[] ICapeThermoPhases.GetPhaseInfo(String phaseLabel, String phaseAttribute)
    {
        return (String[])_pIPhases.GetPhaseInfo(phaseLabel, phaseAttribute);
    }

    /// <summary>Returns Phase labels and other important descriptive information for all the 
    /// Phases supported.</summary>
    /// <param name="phaseLabels">The list of Phase labels for the Phases supported. 
    /// A Phase label can be any string but each Phase must have a unique label. If, 
    /// for some reason, no Phases are supported an UNDEFINED value should be returned 
    /// for the phaseLabels. The number of Phase labels must also be equal to the 
    /// number of Phases returned by the GetNumPhases method.
    /// </param>
    /// <param name="stateOfAggregation">The physical State of Aggregation associated 
    /// with each of the Phases. This must be one of the following strings: ”Vapor”, 
    /// “Liquid”, “Solid” or “Unknown”. Each Phase must have a single State of 
    /// Aggregation. The value must not be left undefined, but may be set to “Unknown”.
    /// </param>
    /// <param name="keyCompoundId">The key Compound for the Phase. This must be the
    /// Compound identifier (as returned by GetCompoundList), or it may be undefined 
    /// in which case a UNDEFINED value is returned. The key Compound is an indication 
    /// of the Compound that is expected to be present in high concentration in the 
    /// Phase, e.g. water for an aqueous liquid phase. Each Phase can have a single 
    /// key Compound.
    /// </param>
    /// <remarks><para>The Phase label allows the phase to be uniquely identified in methods of
    /// the ICapeThermoPhases interface and other CAPE-OPEN interfaces. The State of 
    /// Aggregation and key Compound provide a way for the PME, or other client, to 
    /// interpret the meaning of a Phase label in terms of the physical characteristics 
    /// of the Phase.</para>
    /// <para>All arrays returned by this method must be of the same length, i.e. 
    /// equal to the number of Phase labels.</para>
    /// <para>To get further information about a Phase, use the GetPhaseInfo method.
    /// </para></remarks>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if this 
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for this operation, are not suitable.</exception>
    void ICapeThermoPhases.GetPhaseList(ref String[] phaseLabels, ref String[] stateOfAggregation,
        ref String[] keyCompoundId)
    {
        object obj1 = null;
        object obj2 = null;
        object obj3 = null;
        _pIPhases.GetPhaseList(ref obj1, ref obj2, ref obj3);
        phaseLabels = (string[])obj1;
        stateOfAggregation = (string[])obj2;
        keyCompoundId = (string[])obj3;
    }

    /// <summary>Returns the values of constant Physical Properties for the specified Compounds.</summary>
    /// <remarks><para>The GetConstPropList method can be used in order to check 
    /// which constant Physical Properties are available.</para>
    /// <para>If the number of requested Physical Properties is P and the number of 
    /// Compounds is C, the propvals array will contain C*P variants. The first C 
    /// variants will be the values for the first requested Physical Property (one 
    /// variant for each Compound) followed by C values of constants for the second 
    /// Physical Property, and so on. The actual type of values returned (Double, 
    /// String, etc.) depends on the Physical Property as specified in section 7.5.2.</para>
    /// <para>Physical Properties are returned in a fixed set of units as specified 
    /// in section 7.5.2.</para>
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
    /// raised, the client should check all the values returned to determine which 
    /// is undefined.</para></remarks>
    /// <param name="props">The list of Physical Property identifiers. Valid
    /// identifiers for constant Physical Properties are listed in
    /// section 7.5.2.</param>
    /// <param name="compIds">List of Compound identifiers for which constants are 
    /// to be retrieved. Set compIds = UNDEFINED to denote all Compounds in the 
    /// component that implements the ICapeThermoCompounds interface.</param>
    /// <returns>Values of constants for the specified Compounds.</returns>
    /// <exception cref = "ECapeNoImpl">The operation GetCompoundConstant is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists, but 
    /// it is not supported by the current implementation. This exception should be 
    /// raised if no compounds or no properties are supported.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">At least one item in the 
    /// list of Physical Properties is not available for a particular Compound. This 
    /// exception is meant to be treated as a warning rather than as an error.</exception>
    /// <exception cref = "ECapeLimitedImpl">One or more Physical Properties are not 
    /// supported by the component that implements this interface. This exception 
    /// should also be raised if any element of the props argument is not recognised 
    /// since the list of Physical Properties in section 7.5.2 is not intended to be 
    /// exhaustive and an unrecognised Physical Property identifier may be valid. If
    /// no Physical Properties at all are supported ECapeNoImpl should be raised 
    /// (see above).</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref = "ECapeBadInvOrder">The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetCompoundConstant method. The error would not be raised when the 
    /// GetCompoundConstant method is implemented by a Material Object.</exception>
    object[] ICapeThermoCompounds.GetCompoundConstant(String[] props, String[] compIds)
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
    /// <exception cref = "ECapeNoImpl">The operation GetCompoundList is “not” 
    /// implemented even if this method can be called for reasons of compatibility
    /// with the CAPE-OPEN standards. That is to say that the operation exists, but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetCompoundList operation, are not suitable.</exception>
    /// <exception cref = "ECapeBadInvOrder">The error to be raised if the Property 
    /// Package required the SetMaterial method to be called before calling the 
    /// GetCompoundList method. The error would not be raised when the 
    /// GetCompoundList method is implemented by a Material Object.</exception>
    void ICapeThermoCompounds.GetCompoundList(ref String[] compIds, ref String[] formulae, ref String[] names,
        ref double[] boilTemps, ref double[] molwts, ref String[] casnos)
    {
        Object obj1 = null;
        Object obj2 = null;
        Object obj3 = null;
        Object obj4 = null;
        Object obj5 = null;
        Object obj6 = null;
        _pICompounds.GetCompoundList(ref obj1, ref obj2, ref obj3, ref obj4, ref obj5, ref obj6);
        compIds = (String[])obj1;
        formulae = (String[])obj2;
        names = (String[])obj3;
        boilTemps = (double[])obj4;
        molwts = (double[])obj5;
        casnos = (String[])obj6;
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
    /// <exception cref = "ECapeNoImpl">The operation GetConstPropList is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists, but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the Get-ConstPropList operation, are not suitable.</exception>
    /// <exception cref = "ECapeBadInvOrder">The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetConstPropList method. The error would not be raised when the 
    /// GetConstPropList method is implemented by a Material Object.</exception>
    String[] ICapeThermoCompounds.GetConstPropList()
    {
        return (String[])_pICompounds.GetConstPropList();
    }

    /// <summary>Returns the number of Compounds supported.</summary>
    /// <returns>Number of Compounds supported.</returns>
    /// <remarks>The number of Compounds returned by this method must be equal to 
    /// the number of Compound identifiers that are returned by the GetCompoundList 
    /// method of this interface. It must be zero or a positive number.</remarks>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for this operation, are not suitable.</exception>
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
    /// <param name ="pressure">Pressure (in Pa) at which Physical Properties are
    /// evaluated</param>
    /// <param name ="compIds">List of Compound identifiers for which Physical
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
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
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example UNDEFINED for argument props.</exception>
    /// <exception cref = "ECapeOutOfBounds">The value of the pressure is outside of
    /// the range of values accepted by the Property Package.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">At least one item in the 
    /// properties list is not available for a particular compound.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref = "ECapeBadInvOrder">The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetPDependentProperty method. The error would not be raised when the 
    /// GetPDependentProperty method is implemented by a Material Object.</exception>
    void ICapeThermoCompounds.GetPDependentProperty(String[] props, double pressure, String[] compIds,
        ref double[] propVals)
    {
        Object obj1 = null;
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref="ECapeBadInvOrder">The error to be raised if the Property 
    /// Package required the SetMaterial method to be called before calling the 
    /// GetPDependentPropList method. The error would not be raised when the 
    /// GetPDependentPropList method is implemented by a Material Object.</exception>
    String[] ICapeThermoCompounds.GetPDependentPropList()
    {
        return (String[])_pICompounds.GetPDependentPropList();
    }

    /// <summary>Returns the values of temperature-dependent Physical Properties for 
    /// the specified pure Compounds.</summary>
    /// <param name ="props">The list of Physical Property identifiers. Valid
    /// identifiers for temperature-dependent Physical Properties are listed in 
    /// section 7.5.3</param>
    /// <param name="temperature">Temperature (in K) at which properties are 
    /// evaluated.</param>
    /// <param name ="compIds">List of Compound identifiers for which Physical
    /// Properties are to be retrieved. Set compIds = UNDEFINED to denote all 
    /// Compounds in the component that implements the ICapeThermoCompounds 
    /// interface .</param>
    /// <param name="propVals">Physical Property values for the Compounds specified.
    /// </param>
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
    /// <exception cref = "ECapeNoImpl"> – The operation is “not” implemented even 
    /// if this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported 
    /// by the current implementation. This exception should be raised if no 
    /// Compounds or no Physical Properties are supported.</exception>
    /// <exception cref = "ECapeLimitedImpl">One or more Physical Properties are not
    /// supported by the component that implements this interface. This exception 
    /// should also be raised (rather than ECapeInvalidArgument) if any element of 
    /// the props argument is not recognised since the list of properties in section 
    /// 7.5.3 is not intended to be exhaustive and an unrecognised Physical Property 
    /// identifier may be valid. If no properties at all are supported ECapeNoImpl 
    /// should be raised (see above).</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example UNDEFINED for argument props.</exception> 
    /// <exception cref = "ECapeOutOfBounds">The value of the temperature is outside
    /// of the range of values accepted by the Property Package.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">At least one item in the 
    /// properties list is not available for a particular compound.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref = "ECapeBadInvOrder"> The error to be raised if the 
    /// Property Package required the SetMaterial method to be called before calling 
    /// the GetTDependentProperty method. The error would not be raised when the 
    /// GetTDependentProperty method is implemented by a Material Object.</exception>
    void ICapeThermoCompounds.GetTDependentProperty(String[] props, double temperature, String[] compIds,
        ref double[] propVals)
    {
        Object obj1 = null;
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported
    /// by the current implementation.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s),
    /// specified for the operation, are not suitable.</exception>
    /// <exception cref = "ECapeBadInvOrder">The error to be raised if the Property 
    /// Package required the SetMaterial method to be called before calling the 
    /// GetTDependentPropList method. The error would not be raised when the 
    /// GetTDependentPropList method is implemented by a Material Object.</exception>
    String[] ICapeThermoCompounds.GetTDependentPropList()
    {
        return (String[])_pICompounds.GetTDependentPropList();
    }

    /// <summary>This method is used to calculate the natural logarithm of the 
    /// fugacity coefficients (and optionally their derivatives) in a single Phase 
    /// mixture. The values of temperature, pressure and composition are specified in 
    /// the argument list and the results are also returned through the argument list.</summary>
    /// <param name ="phaseLabel">Phase label of the Phase for which the properties 
    /// are to be calculated. The Phase label must be one of the strings returned by 
    /// the GetPhaseList method on the ICapeThermoPhases interface.</param>
    /// <param name="temperature">The temperature (K) for the calculation.</param>
    /// <param name="pressure">The pressure (Pa) for the calculation.</param>
    /// <param name ="lnPhiDT">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. temperature (if requested).</param>
    /// <param name ="moleNumbers">Number of moles of each Compound in the mixture.</param>
    /// <param name="fFlags">Code indicating whether natural logarithm of the 
    /// fugacity coefficients and/or derivatives should be calculated (see notes).
    /// </param>
    /// <param name="lnPhi">Natural logarithm of the fugacity coefficients (if
    /// requested).</param>
    /// <param anem = "lnPhiDT">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. temperature (if requested).</param>
    /// <param name ="lnPhiDP">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. pressure (if requested).</param>
    /// <param name ="lnPhiDn">Derivatives of natural logarithm of the fugacity
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
    /// argument must be equal to the number of Compounds in the Material Object.
    /// </para>
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
    /// Property Package component would be:
    /// </para>
    /// <para>
    /// - Check that the phaseLabel specified is valid.
    /// </para>
    /// <para>
    /// - Check that the moleNumbers array contains the number of values expected
    /// (should be consistent with the last call to the SetMaterial method).
    /// </para>
    /// <para>
    /// - Calculate the requested properties/derivatives at the T/P/composition specified in the argument list.
    /// </para>
    /// <para>
    /// - Store values for the properties/derivatives in the corresponding arguments.
    /// </para>
    /// <para>Note that this calculation can be carried out irrespective of whether the Phase actually exists in the Material Object.
    /// </para></remarks>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported by 
    /// the current implementation.</exception>
    /// <exception cref="ECapeLimitedImpl">Would be raised if the one or more of the 
    /// properties requested cannot be returned because the calculation is not 
    /// implemented.</exception>
    /// <exception cref = "ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. For example, the 
    /// ICapeThermoMaterial interface has not been passed via a SetMaterial call prior
    /// to calling this method.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites for the 
    ///	Property Calculation are not valid. Forexample, the composition of the phase is 
    /// not defined, the number of Compounds in the Material Object is zero or not 
    /// consistent with the moleNumbers argument or any other necessary input information 
    /// is not available.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">At least one item in the 
    /// requested properties cannot be returned. This could be because the property 
    /// cannot be calculated at the specified conditions or for the specified Phase. 
    /// If the property calculation is not implemented then ECapeLimitedImpl should 
    /// be returned.</exception>
    /// <exception cref = "ECapeSolvingError">One of the property calculations has 
    /// failed. For example if one of the iterative solution procedures in the model 
    /// has run out of iterations, or has converged to a wrong solution.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example an unrecognised value, or UNDEFINED for the 
    /// phaseLabel argument.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for this operation, are not suitable.</exception>
    void ICapeThermoPropertyRoutine.CalcAndGetLnPhi(String phaseLabel, double temperature,
        double pressure,
        double[] moleNumbers,
        CapeFugacityFlag fFlags,
        ref double[] lnPhi,
        ref double[] lnPhiDT,
        ref double[] lnPhiDP,
        ref double[] lnPhiDn)
    {
        object obj1 = null;
        object obj2 = null;
        object obj3 = null;
        object obj4 = null;
        int flags = (int)fFlags;
        _pIPropertyRoutine.CalcAndGetLnPhi(phaseLabel, temperature, pressure, moleNumbers, flags, ref obj1, ref obj2,
            ref obj3, ref obj4);
        lnPhi = (double[])obj1;
        lnPhiDT = (double[])obj2;
        lnPhiDP = (double[])obj3;
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
    [Description("method CalcSinglePhaseProp")]
    void ICapeThermoPropertyRoutine.CalcSinglePhaseProp(string[] props, String phaseLabel)
    {
        _pIPropertyRoutine.CalcSinglePhaseProp(props, phaseLabel);
    }

    /// <summary>CalcTwoPhaseProp is used to calculate mixture properties and property 
    /// derivatives that depend on two Phases at the current values of temperature, 
    /// pressure and composition set in the Material Object. It does not perform 
    /// Equilibrium Calculations.</summary>
    /// <param name ="props">The list of identifiers for properties to be calculated.
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if this 
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref = "ECapeLimitedImpl">Would be raised if the one or more of the 
    /// properties requested cannot be returned because the calculation (of the 
    /// particular property) is not implemented. This exception should also be raised 
    /// (rather than ECapeInvalidArgument) if the props argument is not recognised 
    /// because the list of properties in section 7.5.6 is not intended to be 
    /// exhaustive and an unrecognised property identifier may be valid. If no 
    /// properties at all are supported ECapeNoImpl should be raised (see above).</exception>
    /// <exception cref = "ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. For example, the 
    /// ICapeThermoMaterial interface has not been passed via a SetMaterial call 
    /// prior to calling this method.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites for the 
    /// property calculation are not valid. For example, the composition of one of the 
    /// Phases is not defined, or any other necessary input information is not 
    /// available.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">At least one item in the 
    /// requested properties cannot be returned. This could be because the property 
    /// cannot be calculated at the specified conditions or for the specified Phase. 
    /// If the property calculation is not implemented then ECapeLimitedImpl should be 
    /// returned.</exception>
    /// <exception cref = "ECapeSolvingError">One of the property calculations has 
    /// failed. For example if one of the iterative solution procedures in the model 
    /// has run out of iterations, or has converged to a wrong solution.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example an unrecognised value or UNDEFINED for the 
    /// phaseLabels argument or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for this operation, are not suitable.</exception>
    [DispId(0x00000003)]
    [Description("method CalcTwoPhaseProp")]
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
    /// <exception cref = "ECapeNoImpl">The operation CheckSinglePhasePropSpec is 
    /// “not” implemented even if this method can be called for reasons of 
    /// compatibility with the CAPE-OPEN standards. That is to say that the operation 
    /// exists, but it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeBadInvOrder">The necessary pre-requisite operation has
    /// not been called prior to the operation request. The ICapeThermoMaterial 
    /// interface has not been passed via a SetMaterial call prior to calling this 
    /// method.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites for the 
    /// property calculation are not valid. For example, if a prior call to the 
    /// SetMaterial method of the ICapeThermoMaterialContext interface has failed to 
    /// provide a valid Material Object.</exception>
    /// <exception cref = "ECapeInvalidArgument">One or more of the input arguments is 
    /// not valid: for example, UNDEFINED value for the property argument or the 
    /// phaseLabel argument.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the CheckSinglePhasePropSpec operation, are not suitable.</exception>
    bool ICapeThermoPropertyRoutine.CheckSinglePhasePropSpec(String property, String phaseLabel)
    {
        return _pIPropertyRoutine.CheckSinglePhasePropSpec(property, phaseLabel);
    }

    /// <summary>Checks whether it is possible to calculate a property with the 
    /// CalcTwoPhaseProp method for a given set of Phases.</summary>
    /// <param name="property">The identifier of the property to check. To be valid 
    /// this must be one of the supported two-phase properties (including derivatives), 
    /// as given by the GetTwoPhasePropList method.</param>
    /// <param name ="phaseLabels">Phase labels of the Phases for which the properties 
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
    /// <exception cref = "ECapeNoImpl">The operation CheckTwoPhasePropSpec is “not” 
    /// implemented even if this method can be called for reasons of compatibility with 
    /// the CAPE-OPEN standards. That is to say that the operation exists, but it is 
    /// not supported by the current implementation. This may be the case if no 
    /// two-phase property is supported.</exception>
    /// <exception cref = "ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. The ICapeThermoMaterial 
    /// interface has not been passed via a SetMaterial call prior to calling this 
    /// method.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites for the 
    /// property calculation are not valid. For example, if a prior call to the 
    /// SetMaterial method of the ICapeThermoMaterialContext interface has failed to 
    /// provide a valid Material Object.</exception>
    /// <exception cref = "ECapeInvalidArgument">One or more of the input arguments is 
    /// not valid. For example, UNDEFINED value for the property argument or the 
    /// phaseLabels argument or number of elements in phaseLabels array not equal to 
    /// two.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the CheckTwoPhasePropSpec operation, are not suitable.</exception>
    bool ICapeThermoPropertyRoutine.CheckTwoPhasePropSpec(String property, string[] phaseLabels)
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported by 
    /// the current implementation.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetSinglePhasePropList operation, are not suitable.</exception>
    [DispId(0x00000006)]
    [Description("method GetSinglePhasePropList")]
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
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if this
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetTwoPhasePropList operation, are not suitable.</exception>
    [DispId(0x00000007)]
    [Description("method GetTwoPhasePropList")]
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
    /// <para>- With the ICapeThermoMaterial interface of the supplied Material Object:
    /// </para>
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
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for this operation, are not suitable.</exception>
    void ICapeThermoEquilibriumRoutine.CalcEquilibrium(string[] specification1, string[] specification2,
        String solutionType)
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
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example UNDEFINED for solutionType, specification1 or 
    /// specification2 argument.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for this operation, are not suitable.</exception>
    bool ICapeThermoEquilibriumRoutine.CheckEquilibriumSpec(string[] specification1, string[] specification2,
        String solutionType)
    {
        return _pIEquilibriumRoutine.CheckEquilibriumSpec(specification1, specification2, solutionType);
    }
}