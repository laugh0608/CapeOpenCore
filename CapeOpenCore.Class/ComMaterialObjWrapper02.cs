/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.08
 */

namespace CapeOpenCore.Class;

/// <summary>用于将基于 .NET 的对象暴露给基于 COM 的程序管理器（PMC）的包装类。</summary>
/// <remarks><para>这个类允许基于 COM 的遗留 PMC 访问基于 .NET 的物流对象。
/// 这个包装器将使用基于 .NET 的热力学接口的材料对象暴露给基于 COM 的 PMC。基于 COM 的 PMC 可以通过 CAPE-OPEN 接口的 COM 版本调用物流对象。</para>
/// <para>这个类由 <see cref="UnitPortWrapper"/> 类用于将端口连接的流股暴露给由 <see cref="UnitOperationWrapper"/> 类包装的基于 COM 的遗留单元操作。
/// 这个包装器处理在热力学接口的 .NET 版本中使用的强类型数组和 COM 版本中使用的 VARIANT 数据类型之间的转换。</para></remarks>
partial class COMMaterialObjectWrapper
{
    /// <summary>该方法用于计算单相混合物的逸度系数（可选其导数）的自然对数。温度、压力和成分值在参数列表中指定，
    /// 计算结果也通过参数列表返回。</summary>
    /// <param name ="phaseLabel">要计算属性的相位的相位标签。相位标签必须是 ICapeThermoPhases 接口的
    /// GetPhaseList 方法返回的字符串之一。</param>
    /// <param name="temperature">计算时的温度（K）。</param>
    /// <param name="pressure">计算压力（Pa）。</param>
    /// <param name ="lnPhiDT">逸度系数随温度变化的自然对数的衍生物（如有要求）。</param>
    /// <param name ="moleNumbers">混合物中每种化合物的摩尔数。</param>
    /// <param name="fFlags">表示是否应计算逸度系数和/或导数的自然对数的代码（见注释）。</param>
    /// <param name="lnPhi">逸度系数的自然对数（如有要求）。</param>
    /// <param name ="lnPhiDP">与压力有关的逸度系数自然对数的衍生物（如有要求）。</param>
    /// <param name ="lnPhiDn">与摩尔数有关的逸度系数自然对数的衍生物（如有要求）。</param>
    /// <remarks><para>提供这种方法是为了高效地计算和返回逸度系数的自然对数，逸度系数是最常用的热力学性质。</para>
    /// <para>用于计算的温度、压力和成分（摩尔数）由参数指定，不会通过单独请求从 “材料对象 ”中获取。同样，任何计算量都通
    /// 过参数返回，不会存储在材料对象中。调用此方法不会影响材料对象的状态。但应注意的是，在调用 CalcAndGetLnPhi 之前，
    /// 必须通过调用实现 ICapeThermoPropertyRoutine 接口的组件的 ICapeThermoMaterialContext 接口上的 SetMaterial
    /// 方法来定义一个有效的 Material Object。材料对象中的化合物必须已被识别，moleNumbers 参数中提供的数值数必须等于
    /// 材料对象中的化合物数。</para>
    /// <para>逸度系数信息以逸度系数的自然对数形式返回。这是因为热力学模型自然会提供该量的自然对数，而且可以安全地返回更大范围的值。</para>
    /// <para>该方法实际计算和返回的数量由整数代码 fFlags 控制。该代码是通过使用下表所示的枚举常量 eCapeCalculationCode
    /// （在 Thermo 1.1 版 IDL 中定义）对所需属性和每个导数的贡献值求和而形成的。例如，要计算对数逸度系数及其 T 衍生系数，
    /// fFlags 参数应设置为 CAPE_LOG_FUGACITY_COEFFICIENTS + CAPE_T_DERIVATIVE。</para>
    /// <table border="1">
    /// <tr><th>Calculation Type</th><th>Enumeration Value</th><th>Numerical Value</th></tr>
    /// <tr><td>no calculation</td><td>CAPE_NO_CALCULATION</td><td>0</td></tr>
    /// <tr><td>log fugacity coefficients</td><td>CAPE_LOG_FUGACITY_COEFFICIENTS</td><td>1</td></tr>
    /// <tr><td>T-derivative</td><td>CAPE_T_DERIVATIVE</td><td>2</td></tr>
    /// <tr><td>P-derivative</td><td>CAPE_P_DERIVATIVE</td><td>4</td></tr>
    /// <tr><td>mole number derivatives</td><td>CAPE_MOLE_NUMBERS_DERIVATIVES</td><td>8</td></tr>
    /// </table>	
    /// <para>如果调用 CalcAndGetLnPhi 时 fFlags 设置为 CAPE_NO_CALCULATION，则不会返回任何属性值。</para>
    /// <para>由属性包组件执行该方法时，典型的操作顺序是：</para>
    /// <para>1. 检查指定的 phaseLabel 是否有效。</para>
    /// <para>2. 检查 moleNumbers 数组是否包含预期的数值（应与上次调用的 SetMaterial 方法一致）。</para>
    /// <para>3. 在参数列表中指定的 T/P/ 组合条件下计算所要求的属性/衍生物。</para>
    /// <para>4. 在相应参数中存储属性/衍生物的值。</para>
    /// <para>请注意，无论 “阶段 ”是否实际存在于 “材料对象 ”中，都可以进行这种计算。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeLimitedImpl">如果由于未执行计算而无法返回所请求的一个或多个属性，则会引发该故障。</exception>
    /// <exception cref="ECapeBadInvOrder">在操作请求之前，尚未调用必要的前提操作。例如，在调用此方法之前，
    /// ICapeThermoMaterial 接口没有通过 SetMaterial 调用传递。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的前提条件无效。例如，相的组成未定义，材料对象中的
    /// 化合物数量为零或与摩尔数参数不一致，或者没有其他必要的输入信息。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所请求的属性中至少有一项无法返回。这可能是因为无法在指定
    /// 条件下或针对指定阶段计算该属性。如果未执行属性计算，则应返回 ECapeLimitedImpl。</exception>
    /// <exception cref="ECapeSolvingError">某个属性计算失败。例如，模型中的某个迭代求解程序迭代次数耗尽，或收敛到一个错误的解。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的值，或 phaseLabel 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoPropertyRoutineCOM.CalcAndGetLnPhi(string phaseLabel, double temperature, double pressure, object moleNumbers, int fFlags, ref  object lnPhi, ref  object lnPhiDT, ref  object lnPhiDP, ref  object lnPhiDn)
    {
        double[] temp1 = null;
        double[] temp2 = null;
        double[] temp3 = null;
        double[] temp4 = null;
        var flags = CapeFugacityFlag.CAPE_NO_CALCULATION;
        if (fFlags % 2 == 1)
        {
            flags = CapeFugacityFlag.CAPE_LOG_FUGACITY_COEFFICIENTS;
            if (fFlags / 8 == 1)
            {
                flags = flags | CapeFugacityFlag.CAPE_MOLE_NUMBERS_DERIVATIVES;
                fFlags = fFlags - 8;
            }
            if (fFlags / 4 == 1) 
            {
                flags = flags | CapeFugacityFlag.CAPE_P_DERIVATIVE;
                fFlags = fFlags - 4;
            }
            if (fFlags / 2 == 1) flags = flags | CapeFugacityFlag.CAPE_T_DERIVATIVE;
        }
        _pIPropertyRoutine.CalcAndGetLnPhi(phaseLabel, temperature, pressure, (double[])moleNumbers, flags, ref temp1, ref temp2, ref temp3, ref temp4);
        lnPhi = temp1;
        lnPhiDT = temp2;
        lnPhiDP = temp3;
        lnPhiDn = temp4;
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
    void ICapeThermoPropertyRoutineCOM.CalcSinglePhaseProp(object props, string phaseLabel)
    {
        _pIPropertyRoutine.CalcSinglePhaseProp((string[])props, phaseLabel);
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
    /// phaseLabels argument or UNDEFINED for the prop's argument.</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoPropertyRoutineCOM.CalcTwoPhaseProp(object props, object phaseLabels)
    {
        _pIPropertyRoutine.CalcTwoPhaseProp((string[])props, (string[])phaseLabels);
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
    bool ICapeThermoPropertyRoutineCOM.CheckSinglePhasePropSpec(string property, string phaseLabel)
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
    bool ICapeThermoPropertyRoutineCOM.CheckTwoPhasePropSpec(string property, object phaseLabels)
    {
        return _pIPropertyRoutine.CheckTwoPhasePropSpec(property, (string[])phaseLabels);
    }

    /// <summary>Returns the list of supported non-constant single-phase Physical 
    /// Properties.</summary>
    /// <returns>List of all supported non-constant single-phase property identifiers. 
    /// The standard single-phase property identifiers are listed in section 7.5.5.</returns>
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
    object ICapeThermoPropertyRoutineCOM.GetSinglePhasePropList()
    {
        return _pIPropertyRoutine.GetSinglePhasePropList();
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
    object ICapeThermoPropertyRoutineCOM.GetTwoPhasePropList()
    {
        return _pIPropertyRoutine.GetTwoPhasePropList();
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
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoEquilibriumRoutineCOM.CalcEquilibrium(object specification1, object specification2, string solutionType)
    {
        _pIEquilibriumRoutine.CalcEquilibrium((string[])specification1, (string[])specification2, solutionType);
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
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    bool ICapeThermoEquilibriumRoutineCOM.CheckEquilibriumSpec(object specification1, object specification2, string solutionType)
    {
        return _pIEquilibriumRoutine.CheckEquilibriumSpec((string[])specification1, (string[])specification2, solutionType);
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
    /// <exception cref="ECapeNoImpl">The operation GetUniversalConstant is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists, but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref="ECapeInvalidArgument">For example, UNDEFINED for constantId 
    /// argument is used, or value for constantId argument does not belong to the 
    /// list of recognised values.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetUniversalConstant operation, are not suitable.</exception>	
    object ICapeThermoUniversalConstantCOM.GetUniversalConstant(string constantId)
    {
        return _pIUniversalConstant.GetUniversalConstant(constantId);
    }

    /// <summary>Returns the identifiers of the supported Universal Constants.</summary>
    /// <returns>List of identifiers of Universal Constants. The list of standard 
    /// identifiers is given in section 7.5.1.</returns>
    /// <remarks>A component may return Universal Constant identifiers that do not 
    /// belong to the list defined in section 7.5.1. However, these proprietary 
    /// identifiers may not be understood by most of the clients of this component.</remarks>
    /// <exception cref="ECapeNoImpl">The operation GetUniversalConstantList is 
    /// “not” implemented even if this method can be called for reasons of 
    /// compatibility with the CAPE-OPEN standards. That is to say that the operation 
    /// exists, but it is not supported by the current implementation. This may occur 
    /// when the Property Package does not support any Universal Constants, or if it
    /// does not want to provide values for any Universal Constants which may be used 
    /// within the Property Package.</exception>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s), 
    /// specified for the GetUniversalConstantList operation, are not suitable.
    /// </exception>
    object ICapeThermoUniversalConstantCOM.GetUniversalConstantList()
    {
        return _pIUniversalConstant.GetUniversalConstantList();
    }
}
