/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.08
 */

namespace CapeOpenCore.Class;

/// <summary>用于将基于 .NET 的对象暴露给基于 COM 的程序管理器（PMC）的包装类。</summary>
/// <remarks><para>这个类允许基于 COM 的遗留 PMC 访问基于 .NET 的物流对象。
/// 这个包装器将使用基于 .NET 的热力学接口的物流对象暴露给基于 COM 的 PMC。基于 COM 的 PMC 可以通过 CAPE-OPEN 接口的 COM 版本调用物流对象。</para>
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
    /// <para>用于计算的温度、压力和成分（摩尔数）由参数指定，不会通过单独请求从 “物流对象”中获取。同样，任何计算量都通
    /// 过参数返回，不会存储在物流对象中。调用此方法不会影响物流对象的状态。但应注意的是，在调用 CalcAndGetLnPhi 之前，
    /// 必须通过调用实现 ICapeThermoPropertyRoutine 接口的组件的 ICapeThermoMaterialContext 接口上的 SetMaterial
    /// 方法来定义一个有效的 Material Object。物流对象中的化合物必须已被识别，moleNumbers 参数中提供的数值数必须等于
    /// 物流对象中的化合物数。</para>
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
    /// <para>请注意，无论 “相态”是否实际存在于 “物流对象”中，都可以进行这种计算。</para></remarks>
    /// <exception cref="ECapeNoImpl">出于与 CAPE-OPEN 标准的兼容性考虑，即使可以调用该方法，也 “未 ”执行该操作。
    /// 也就是说，该操作是存在的，但目前的实现方式不支持它。</exception>
    /// <exception cref="ECapeLimitedImpl">如果由于未执行计算而无法返回所请求的一个或多个属性，则会引发该故障。</exception>
    /// <exception cref="ECapeBadInvOrder">在操作请求之前，尚未调用必要的前提操作。例如，在调用此方法之前，
    /// ICapeThermoMaterial 接口没有通过 SetMaterial 调用传递。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的前提条件无效。例如，相的组成未定义，物流对象中的
    /// 化合物数量为零或与摩尔数参数不一致，或者没有其他必要的输入信息。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">所请求的属性中至少有一项无法返回。这可能是因为无法在指定
    /// 条件下或针对指定相态计算该属性。如果未执行属性计算，则应返回 ECapeLimitedImpl。</exception>
    /// <exception cref="ECapeSolvingError">某个属性计算失败。例如，模型中的某个迭代求解程序迭代次数耗尽，或收敛到一个错误的解。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，例如未识别的值，或 phaseLabel 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoPropertyRoutineCOM.CalcAndGetLnPhi(string phaseLabel, double temperature, double pressure, 
        object moleNumbers, int fFlags, ref  object lnPhi, ref  object lnPhiDT, ref  object lnPhiDP, ref  object lnPhiDn)
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
        _pIPropertyRoutine.CalcAndGetLnPhi(phaseLabel, temperature, pressure, (double[])moleNumbers, flags, ref temp1, 
            ref temp2, ref temp3, ref temp4);
        lnPhi = temp1;
        lnPhiDT = temp2;
        lnPhiDP = temp3;
        lnPhiDn = temp4;
    }
    
    /// <summary>CalcSinglePhaseProp 用于计算在当前温度、压力和组分值下，物流对象中单相混合物的性质及其导数。
    /// CalcSinglePhaseProp 不进行相平衡计算。</summary>
    /// <param name="props">用于计算单相属性或导数的标识符列表。请参阅第 7.5.5 节和第 7.6 节以获取标准标识符。</param>
    /// <param name="phaseLabel">用于计算属性的相的相标签。相标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的字符串之一。</param>
    /// <remarks><para>CalcSinglePhaseProp 用于计算仅定义于单一相的物理性质，例如焓或粘度。依赖于多个相的物理性质，
    /// 例如表面张力或 K 值，则由 CalcTwoPhaseProp 方法处理。</para>
    /// <para>实现此方法的组件必须从关联的物流对象获取计算所需的输入参数（温度、压力和成分），并将计算结果设置到物流对象中。</para>
    /// <para>热力学和物理性质组件（如属性包或属性计算器）必须实现 ICapeThermoMaterialContext 接口，
    /// 以便通过 SetMaterial 方法传递 ICapeThermoMaterial 接口。</para>
    /// <para>当 CalcSinglePhaseProp 由属性包组件实现时，其典型操作序列如下：</para>
    /// <para>1. 请确认指定的相位标签有效。</para>
    /// <para>2. 使用 GetTPFraction 方法（该方法属于在最后一次调用 SetMaterial 方法时指定的 Material 对象）来获取指定相的温度、压力和组成。</para>
    /// <para>3. 计算属性。</para>
    /// <para>4. 使用 ICapeThermoMaterial 接口的 SetSinglePhaseProp 方法，将相的属性值存储在物流对象中。</para>
    /// <para>CalcSinglePhaseProp 将通过 GetSinglePhaseProp 调用从物流对象请求所需的属性值。如果请求的属性不可用，
    /// 将抛出 ECapeThrmPropertyNotAvailable 异常。如果发生此错误，属性包可以将其返回给客户端，或请求不同的属性。物流对象的实现
    /// 必须能够根据客户端选择的基础提供属性值，通过实现从一种基础到另一种基础的转换来实现这一点。</para>
    /// <para>客户不应假设物流对象中的相分数和复合分数已归一化。分数值也可能超出 0 到 1 的范围。如果分数未归一化，
    /// 或超出预期范围，则属性包有责任决定如何处理该情况。</para>
    /// <para>建议逐个请求属性以简化错误处理。然而，也承认在某些情况下，同时请求多个属性所带来的潜在效率提升更为重要。
    /// 例如，当需要某个属性及其衍生属性时，便是其中一种情况。</para>
    /// <para>如果客户端在一次调用中使用了多个属性，而其中一个属性失败，则整个调用应被视为失败。这意味着，
    /// 在确定整个请求可以满足之前，属性包不应将任何值写回物流对象。</para>
    /// <para>PME 可能需要在温度、压力和组分条件下获取某一相的属性值，而根据用于描述属性的数学/物理模型，该相在这些条件下并不存在。
    /// 此时，系统可能会抛出异常 ECapeThrmPropertyNotAvailable，或返回外推值。</para>
    /// <para>实施方有责任决定如何处理此类情况。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeLimitedImpl">如果请求的属性之一或多个无法返回，因为该属性的计算尚未实现，则会引发此异常。
    /// 如果 props 参数未被识别，也应抛出此异常（而非 ECapeInvalidArgument），因为第 7.5.5 节中列出的属性列表并非旨在
    /// 涵盖所有情况，且未识别的属性标识符可能有效。如果完全不支持任何属性，应抛出 ECapeNoImpl 异常（参见上文）。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行该操作请求之前，未调用必要的先决条件操作。例如，在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception> 
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。例如，相的组成未定义，或任何其他必要的输入信息不可用。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">请求的属性中至少有一个属性无法返回。这可能是因为该属性在指定的条件
    /// 下或指定的相态无法计算。如果属性计算未实现，则应返回 ECapeLimitedImpl。</exception>
    void ICapeThermoPropertyRoutineCOM.CalcSinglePhaseProp(object props, string phaseLabel)
    {
        _pIPropertyRoutine.CalcSinglePhaseProp((string[])props, phaseLabel);
    }

    /// <summary>CalcTwoPhaseProp 用于计算混合物性质及其导数，这些性质和导数依赖于当前温度、压力和组分值，
    /// 这些值在物流对象中设置。它不进行平衡计算。</summary>
    /// <param name ="props">待计算属性的一组标识符。该列表必须包含一个或多个支持的两相属性及其导数
    /// （由 GetTwoPhasePropList 方法提供）。两相属性的标准标识符在第7.5.6节和第7.6节中给出。</param>
    /// <param name="phaseLabels">用于计算性质的相的相标签。相标签必须是 ICapeThermoPhases 接口
    /// 上 GetPhaseList 方法返回的字符串中的两个。</param>
    /// <remarks><para>CalcTwoPhaseProp 用于计算表面张力或 K 值等性质的数值。与单一相相关的性质则由 ICapeThermoPropertyRoutine 接口
    /// 中的 CalcSinglePhaseProp 方法处理。实现该方法的组件必须从关联的物流对象中获取计算所需的输入参数（温度、压力和组成），
    /// 并将计算结果设置到物流对象中。</para>
    /// <para>组件（如属性包或属性计算器）必须实现 ICapeThermoMaterialContext 接口，以便可以
    /// 通过 SetMaterial 方法传递 ICapeThermoMaterial 接口。</para>
    /// <para>当 CalcTwoPhaseProp 由属性包组件实现时，其典型操作序列如下：</para>
    /// <para>1. 请确认指定的相位标签（phaseLabels）是否有效。</para>
    /// <para>2. 使用 GetTPFraction 方法（该方法属于在最后一次调用 SetMaterial 方法时指定的物流对象）来获取指定相的温度、压力和组成。</para>
    /// <para>3. 计算属性。</para>
    /// <para>4. 使用 ICapeThermoMaterial 接口的 SetTwoPhaseProp 方法为物流对象的属性存储值。</para>
    /// <para>CalcTwoPhaseProp 将通过 GetTPFraction 或 GetSinglePhaseProp 调用从物流对象中获取所需的值。如果请求的属性不可用，
    /// 将抛出 ECapeThrmPropertyNotAvailable 异常。如果发生此错误，属性包可以将其返回给客户端，或请求不同的属性。物流对象的实现必须
    /// 能够通过实现从一种基准到另一种基准的转换，使用客户端选择的基准提供属性值。</para>
    /// <para>客户不应假设物流对象中的相分数和复合分数已归一化。分数值也可能超出 0 到 1 的范围。如果分数未归一化，
    /// 或超出预期范围，则属性包有责任决定如何处理此情况。</para>
    /// <para>建议逐个请求属性以简化错误处理。然而，也承认在某些情况下，同时请求多个属性所带来的潜在效率提升更为重要。
    /// 例如，当需要某个属性及其衍生属性时，便属于此类情况。</para>
    /// <para>如果客户端在一次调用中使用了多个属性，而其中一个属性失败，则整个调用应被视为失败。这意味着，
    /// 在确定整个请求可以满足之前，属性包不应将任何值写回物流对象。</para>
    /// <para>CalcTwoPhaseProp 函数必须针对每种相组的组合单独调用。例如，气液 K 值必须与液液 K 值分开计算，分别进行单独调用。</para>
    /// <para>两相性质在所有相的温度和压力不相同的情况下可能没有意义。属性包有责任检查此类条件，并在必要时触发异常。</para>
    /// <para>PME 可能需要在温度、压力和组分条件下获取相的属性值，而此时一个或两个相可能不存在（根据用于表示属性的数学/物理模型）。
    /// 此时可能抛出异常 ECapeThrmPropertyNotAvailable，或返回外推值。如何处理此情况由实现者决定。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与CAPE-OPEN标准兼容性的考虑，该方法可以被调用，
    /// 但该操作“并未”实现。也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeLimitedImpl">如果请求的属性之一或多个无法返回，因为该属性的计算尚未实现，则会引发此异常。如果 props 参数
    /// 未被识别，也应抛出此异常（而非 ECapeInvalidArgument），因为第 7.5.6 节中列出的属性列表并非旨在涵盖所有情况，且未识别的属性标识符
    /// 可能有效。如果完全不支持任何属性，应抛出 ECapeNoImpl 异常（参见上文）。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行该操作请求之前，未调用必要的先决条件操作。例如，在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。例如，其中一个相态的组成未定义，或任何其他必要的输入信息不可用。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">请求的属性中至少有一个属性无法返回。这可能是因为该属性在指定的条件下或指定
    /// 的相态无法计算。如果属性计算未实现，则应返回 ECapeLimitedImpl。</exception>
    /// <exception cref="ECapeSolvingError">其中一个属性计算失败。例如，如果模型中的某个迭代求解过程已耗尽迭代次数，或收敛到一个错误的解。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如 phaseLabels 参数的值为未识别的值或 UNDEFINED，或 prop's 参数的值为 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoPropertyRoutineCOM.CalcTwoPhaseProp(object props, object phaseLabels)
    {
        _pIPropertyRoutine.CalcTwoPhaseProp((string[])props, (string[])phaseLabels);
    }

    /// <summary>检查是否可以使用 CalcSinglePhaseProp 方法为给定的相计算属性。</summary>
    /// <param name="property">要检查的属性的标识符。要有效，这必须是支持的单相属性或其衍生属性之一（由 GetSinglePhasePropList 方法提供）。</param>
    /// <param name="phaseLabel">计算校验的相位标签。此标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的标签之一。</param>
    /// <returns>如果属性与相位标签的组合受支持，则布尔值设置为 True；否则设置为 False。</returns>
    /// <remarks><para>检查结果应仅取决于实现 ICapeThermoPropertyRoutine 接口的组件（例如属性包）的特性和配置（存在的化合物和相）。
    /// 它不应取决于是否已设置物流对象，也不应取决于物流对象的状态（温度、压力、组成等）或配置。</para>
    /// <para>预计 PME 或其他客户端将使用此方法检查在导入包时，该包是否支持其所需的属性。如果任何必要属性不可用，则应中止导入过程。</para>
    /// <para>如果实现 ICapeThermoPropertyRoutine 接口的组件无法识别 property 或 phaseLabel 参数，则该方法应返回 False。</para></remarks>
    /// <exception cref="ECapeNoImpl">操作 CheckSinglePhasePropSpec 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法仍可
    /// 被调用。也就是说，该操作确实存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行操作请求之前，未调用必要的先决条件操作。在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。例如，如果之前对 ICapeThermoMaterialContext 接口
    /// 的 SetMaterial 方法的调用未能提供有效的物流对象。</exception>
    /// <exception cref="ECapeInvalidArgument">一个或多个输入参数无效：例如，属性参数或相态标签参数的值为未定义。</exception>
    /// <exception cref="ECapeUnknown">当为 CheckSinglePhasePropSpec 操作指定的其他错误不适用时，应引发的错误。</exception>
    bool ICapeThermoPropertyRoutineCOM.CheckSinglePhasePropSpec(string property, string phaseLabel)
    {
        return _pIPropertyRoutine.CheckSinglePhasePropSpec(property, phaseLabel);
    }

    /// <summary>检查是否可以使用 CalcTwoPhaseProp 方法对给定的一组相位计算属性。</summary>
    /// <param name="property">要检查的属性的标识符。要使该标识符有效，它必须是支持的两相态属性（包括衍生属性）之一，
    /// 具体由 GetTwoPhasePropList 方法提供。</param>
    /// <param name ="phaseLabels">用于计算属性的相的相标签。相标签必须是 ICapeThermoPhases 接口上 GetPhaseList 方法返回的两个标识符。</param>
    /// <returns>布尔值：如果属性与相位标签的组合受支持，则设置为 True；否则设置为 False。</returns>
    /// <remarks><para>检查结果应仅取决于实现 ICapeThermoPropertyRoutine 接口的组件（例如属性包）的特性和配置（存在的化合物和相）。
    /// 它不应取决于是否已设置物流对象，也不应取决于物流对象的状态（温度、压力、组成等）或配置。</para>
    /// <para>预计 PME 或其他客户端将使用此方法在导入属性包时验证其所需属性是否被属性包支持。若发现任何关键属性缺失，应立即终止导入过程。</para>
    /// <para>如果属性参数或 phaseLabels 参数中的值未被实现 ICapeThermoPropertyRoutine 接口的组件识别，则该方法应返回 False。</para></remarks>
    /// <exception cref="ECapeNoImpl">操作 CheckTwoPhasePropSpec 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作确实存在，但当前实现不支持该功能。如果系统不支持两相属性，则可能出现这种情况。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行操作请求之前，未调用必要的先决条件操作。在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。例如，如果之前对 ICapeThermoMaterialContext 接口
    /// 的 SetMaterial 方法的调用未能提供有效的物流对象。</exception>
    /// <exception cref="ECapeInvalidArgument">一个或多个输入参数无效。例如，属性参数或相位标签参数的值为未定义，
    /// 或相位标签数组中的元素个数不等于两个。</exception>
    /// <exception cref="ECapeUnknown">当为 CheckTwoPhasePropSpec 操作指定的其他错误不适用时，应引发的错误。</exception>
    bool ICapeThermoPropertyRoutineCOM.CheckTwoPhasePropSpec(string property, object phaseLabels)
    {
        return _pIPropertyRoutine.CheckTwoPhasePropSpec(property, (string[])phaseLabels);
    }

    /// <summary>返回支持的非常量单相物理性质列表。</summary>
    /// <returns>所有支持的非恒定单相属性标识符列表。标准的单相属性标识符在第 7.5.5 节中列出。</returns>
    /// <remarks><para>非恒定属性取决于物流对象的状态。</para>
    /// <para>单相性质，例如焓，仅取决于单相的状态。GetSinglePhasePropList 必须返回所有可以通过 CalcSinglePhaseProp 计算的
    /// 单相性质。如果可以计算导数，这些导数也必须返回。</para>
    /// <para>如果不支持单相属性，此方法应返回 UNDEFINED。</para>
    /// <para>要获取支持的两相态属性的列表，请使用 GetTwoPhasePropList。</para>
    /// <para>实现此方法的组件可能会返回不属于第 7.5.5 节中定义的列表的非恒定单相属性标识符。
    /// 然而，这些专有标识符可能无法被该组件的大多数客户端理解。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为 GetSinglePhasePropList 操作指定的其他错误不适用时，应引发的错误。</exception>
    object ICapeThermoPropertyRoutineCOM.GetSinglePhasePropList()
    {
        return _pIPropertyRoutine.GetSinglePhasePropList();
    }

    /// <summary>返回支持的非常量两相态属性的列表。</summary>
    /// <returns>所有支持的非常量两相属性标识符列表。标准的两相属性标识符在第 7.5.6 节中列出。</returns>
    /// <remarks><para>非恒定属性取决于物流对象的状态。两相属性是指那些取决于两个或更多共存相的属性，例如 K 值。</para>
    /// <para>GetTwoPhasePropList 必须返回所有可以通过 CalcTwoPhaseProp 计算的属性。如果可以计算导数，这些导数也必须一并返回。</para>
    /// <para>如果不支持两相属性，此方法应返回 UNDEFINED。</para>
    /// <para>要检查一个属性是否可以针对特定的相位标签集进行评估，请使用 CheckTwoPhasePropSpec 方法。</para>
    /// <para>实现此方法的组件可能会返回不属于第 7.5.6 节中定义的列表的非恒定两相态属性标识符。
    /// 然而，这些专有标识符可能无法被该组件的大多数客户端理解。</para>
    /// <para>要获取支持的单相属性的列表，请使用 GetSinglePhasePropList。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为 GetTwoPhasePropList 操作指定的其他错误不适用时，将引发此错误。</exception>
    object ICapeThermoPropertyRoutineCOM.GetTwoPhasePropList()
    {
        return _pIPropertyRoutine.GetTwoPhasePropList();
    }

    /// <summary>CalcEquilibrium 用于计算平衡状态下各相的质量和组成。如果温度和/或压力不在每个平衡计算中必须指定的
    /// 两个参数范围内，CalcEquilibrium 将计算温度和/或压力。</summary>
    /// <remarks><para>specification11 和 specification2 参数提供了用于检索两个规格值所需的信息，例如平衡计算中的压力和温度。
    /// 可以使用 CheckEquilibriumSpec 方法来检查支持的规格。每个规格变量包含按以下表格中定义的顺序排列的字符串序列
    /// （因此，规格参数可能包含 3 或 4 个项目）：</para>
    /// <para>属性标识符 属性标识符可以是第 7.5.5 节中列出的任何标识符，但通常只有某些属性规范会被任何 Equilibrium 例程支持。</para>
    /// <para>基准，财产价值的基准。基准的有效设置在第 7.4 节中给出。对于不适用基准的属性，使用 UNDEFINED 作为占位符。
    /// 对于大多数平衡规范，计算结果不依赖于基准，但例如对于相分数规范，基准（摩尔或质量）确实会产生影响。</para>
    /// <para>相态标签，相态标签表示该规范适用的相态。它必须是 GetPresentPhases 函数返回的标签之一，或特殊值“Overall”。</para>
    /// <para>复合物标识符（可选）复合物标识符用于指定依赖于特定复合物的规范。规范数组中的此项为可选项，可省略。
    /// 若规范未指定复合物标识符，数组元素可能存在且为空，也可能不存在。</para>
    /// <para>下表列举了典型相平衡规格的一些示例。</para>
    /// <para>在调用 CalcEquilibrium 之前，必须在关联的物流对象中设置与参数列表中的规格对应的值以及混合物的整体组成。</para>
    /// <para>组件（如属性包或平衡计算器）必须实现 ICapeThermoMaterialContext 接口，以便通过 SetMaterial 方法
    /// 传递 ICapeThermoMaterial 接口。CalcEquilibrium 的实现负责在进行计算前验证物流对象。</para>
    /// <para>在平衡计算中将被考虑的相，是存在于物流对象中的那些，即通过调用 SetPresentPhases 指定的相列表。这为客户提供了指定
    /// 是否需要进行某种相平衡计算（如气-液、液-液或气-液-液计算）的途径。CalcEquilibrium 必须使用 GetPresentPhases 方法来
    /// 获取相列表以及相关相状态标志。客户可以利用相状态标志来提供有关相的信息，例如是否提供了平衡状态的估算。有关详细信息，
    /// 请参阅 ICapeThermoMaterial 接口中 GetPresentPhases 和 SetPresentPhases 方法的描述。当平衡计算成功完成后，
    /// 必须使用 SetPresentPhases 方法来指定在平衡状态下存在的相，并将这些相的状态标志设置为 Cape_AtEquilibrium。
    /// 这必须包括任何以零量存在的相，例如在露点计算中的液相。</para>
    /// <para>某些类型的相平衡规格可能导致多个解。一个常见的例子是露点计算的情况。然而，CalcEquilibrium 只能通过物流对象提供一个解。
    /// solutionType 参数允许显式请求“正常”或“逆行”解。当所有规格均未包含相分数时，solutionType 参数应设置为“未指定”。</para>
    /// <para>“正常”状态的定义是：V F 为蒸气相分数，且各导数均处于平衡状态。对于“逆行”行为，CalcEquilibrium 必须设置所有处于平衡状态
    /// 的相的质量、组成、温度和压力，以及整体混合物的温度和压力（若未作为计算规格的一部分进行设置）。CalcEquilibrium 不得设置任何其他物理性质。</para>
    /// <para>例如，在固定压力和温度下的平衡计算中，CalcEquilibrium 可能执行以下操作序列：</para>
    /// <para>1. 使用提供的物流对象的ICapeThermoMaterial接口：</para>
    /// <para>2. 使用 GetPresentPhases 方法来获取平衡计算应考虑的相态列表。</para>
    /// <para>3. 使用物流对象的 ICapeThermoCompounds 接口，通过调用 GetCompoundIds 方法来确定哪些化合物存在。</para>
    /// <para>4. 使用 GetOverallProp 方法获取整体混合物的温度、压力和组成。</para>
    /// <para>5. 进行平衡计算。</para>
    /// <para>6. 使用 SetPresentPhases 指定平衡状态下存在的相，并将相状态标志设置为 Cape_AtEquilibrium。</para>
    /// <para>7. 使用 SetSinglePhaseProp 函数设置所有存在相的压力、温度、相含量（或相分数）和组成。</para></remarks>
    /// <param name="specification1">平衡计算的第一个规格。规格信息用于从物流对象中检索规格的值。详情请见下文。</param>
    /// <param name="specification2">平衡计算的第二项规格，格式与规格1相同。</param>
    /// <param name="solutionType"><para>所需解决方案类型的标识符。标准标识符如下所示：</para>
    /// <para>Unspecified</para><para>Normal</para><para>Retrograde</para>
    /// <para>这些术语的含义在下文的注释中进行定义。其他标识符可能被支持，但其解释不属于 CO 标准的一部分。</para></param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行操作请求之前，未调用必要的先决条件操作。在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeSolvingError">平衡计算无法求解。例如，当求解器已耗尽迭代次数，或收敛到一个平凡解时。</exception>
    /// <exception cref="ECapeLimitedImpl">如果平衡例程无法执行被要求执行的闪蒸操作，则会触发异常。例如，输入规格的值是有效的，
    /// 但例程无法在给定温度和化合物分数的情况下执行闪蒸操作。这表明可能存在对 CheckEquilibriumSpec 方法的错误使用或未使用该方法，
    /// 该方法的存在是为了防止对无法执行的计算调用 CalcEquilibrium。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用。例如，如果规范标识符不属于已识别的标识符列表，则会触发此异常。
    /// 如果传递给参数 solutionType 的值不在三个定义值之中，或者使用 UNDEFINED 代替规范标识符，也会触发此异常。</exception>
    /// <exception cref="ECapeFailedInitialisation"><para>平衡计算的先决条件不成立。例如：</para>
    /// <para>1. 混合物的总体组成未明确规定。</para>
    /// <para>2. 物流对象（由之前对 ICapeThermoMaterialContext 接口的 SetMaterial 方法的调用设置）无效。
    /// 这可能是因为不存在相，或者存在的相未被实现 ICapeThermoEquilibriumRoutine 接口的组件识别。</para>
    /// <para>3. 其他必要的输入信息目前不可用。</para></exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    void ICapeThermoEquilibriumRoutineCOM.CalcEquilibrium(object specification1, object specification2, string solutionType)
    {
        _pIEquilibriumRoutine.CalcEquilibrium((string[])specification1, (string[])specification2, solutionType);
    }

    /// <summary>检查属性包是否支持特定类型的平衡计算。</summary>
    /// <remarks><para>specification11、specification12 和解决方案类型参数的含义与 CalcEquilibrium 方法中的含义相同。</para>
    /// <para>检查结果应仅取决于实现 ICapeThermoEquilibriumRoutine 接口的组件（例如属性包）的特性和配置（存在的化合物和相）。
    /// 它不应取决于是否已设置物流对象，也不应取决于可能已设置的物流对象的状态（温度、压力、组成等）或配置。</para>
    /// <para>如果 solutionType、specification1 和 specification2 参数看似有效，但实际规格不被支持或不被识别，则应返回 False 值。</para></remarks>
    /// <param name="specification1">平衡计算的第一个规格。</param>
    /// <param name="specification2">平衡计算的第二项规格。</param>
    /// <param name="solutionType">所需的解决方案类型。</param>
    /// <returns>如果规格与解决方案类型组合受支持，则设置为 True；否则设置为 False。</returns>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如 solutionType、specification1 或 specification2 参数的值为 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不合适时将引发的错误。</exception>
    bool ICapeThermoEquilibriumRoutineCOM.CheckEquilibriumSpec(object specification1, object specification2, string solutionType)
    {
        return _pIEquilibriumRoutine.CheckEquilibriumSpec((string[])specification1, (string[])specification2, solutionType);
    }

    /// <summary>获取一个通用常量的值。</summary>
    /// <param name="constantId">通用常量标识符。支持的常量列表应通过调用 GetUniversalConstList 方法获取。</param>
    /// <returns>普适常数的值。该值可以是数值或字符串。对于数值，其计量单位在第 7.5.1 节中规定。</returns>
    /// <remarks>普适常数（常被称为基本常数）是一类量，例如气体常数或阿伏加德罗常数。</remarks>
    /// <exception cref="ECapeNoImpl">操作 GetUniversalConstant 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法仍可被调用。
    /// 也就是说，该操作确实存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeInvalidArgument">例如，使用了 UNDEFINED 作为 constantId 参数的值，
    /// 或者 constantId 参数的值不属于已识别的值列表。</exception>
    /// <exception cref="ECapeUnknown">当为 GetUniversalConstant 操作指定的其他错误不适用时，应引发的错误。</exception>	
    object ICapeThermoUniversalConstantCOM.GetUniversalConstant(string constantId)
    {
        return _pIUniversalConstant.GetUniversalConstant(constantId);
    }

    /// <summary>返回支持的通用常量的标识符。</summary>
    /// <returns>通用常量的标识符列表。标准标识符的列表在第 7.5.1 节中给出。</returns>
    /// <remarks>组件可能返回不属于第 7.5.1 节中定义的列表的通用常量标识符。然而，这些专有标识符可能无法被该组件的大多数客户端理解。</remarks>
    /// <exception cref="ECapeNoImpl">操作 GetUniversalConstantList 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作确实存在，但当前实现不支持它。这种情况可能发生在属性包不支持任何通用常量，
    /// 或者不希望为属性包内可能使用的任何通用常量提供值时。</exception>
    /// <exception cref="ECapeUnknown">当为 GetUniversalConstantList 操作指定的其他错误条件不适用时，将引发此错误。</exception>
    object ICapeThermoUniversalConstantCOM.GetUniversalConstantList()
    {
        return _pIUniversalConstant.GetUniversalConstantList();
    }
}
