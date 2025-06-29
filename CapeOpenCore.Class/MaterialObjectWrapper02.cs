/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.27
 */

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

internal partial class MaterialObjectWrapper
{
    /// <summary>为混合物设置两相非恒定属性值。</summary>
    /// <remarks><para>SetTwoPhaseProp 函数的 values 参数可以是包含一个或多个要设置的属性数值
    /// 的 CapeArrayDouble 对象，例如 k 值，也可以是用于设置由更复杂数据结构描述的两相属性
    /// 的 CapeInterface 对象，例如分布式属性。</para>
    /// <para>尽管通过调用 SetTwoPhaseProp 设置的一些属性将具有单个数值，但数值类型的值参数类型
    /// 为 CapeArrayDouble。在这种情况下，即使值参数仅包含一个元素，也必须以包含数组的形式调用该方法。</para>
    /// <para>SetTwoPhaseProp 方法设置的物理属性值取决于两个相，例如表面张力或 K 值。
    /// 取决于单一相的属性则通过 SetSinglePhaseProp 方法进行设置。</para>
    /// <para>如果指定了具有组分导数的物理属性，则导数值将按相标签的指定顺序为两个相分别设置。组成导数的返回值数量取决于属性。
    /// 例如，如果存在 N 种化合物，则表面张力导数的值向量将包含第一相的 N 个组成导数值， 随后第二相的 N 个组成导数值。
    /// 对于 K 值，将包含第一相的 N² 个导数值，随后第二相的 N² 个值，顺序如 7.6.2 节所定义。</para>
    /// <para>在使用 SetTwoPhaseProp 之前，所有引用的相态都必须通过 SetPresentPhases 方法创建。</para></remarks>
    /// <param name="property">在材料对象中设置值的属性。该属性必须是第 7.5.6 节和第 7.6 节中包含的两相属性或其衍生属性之一。</param>
    /// <param name="phaseLabels">设置该属性的相的相标签。
    /// 相标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的两个标识符。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用 UNDEFINED 作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <param name="values">为该属性设置的值（CapeArrayDouble）或 CapeInterface（参见注释）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作虽然存在，但当前实现不支持该操作。如果 PME 不处理任何单相属性，则可能不需要该方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，
    /// 即该值不属于上述有效值列表，例如属性为 UNDEFINED 时。</exception> 
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception> 
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。所引用的相态未通过 SetPresentPhases 方法创建。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不适用时，将引发此错误。</exception>
    void ICapeThermoMaterial.SetTwoPhaseProp(string property, string[] phaseLabels, string basis, double[] values)
    {
        _pIMatObj.SetTwoPhaseProp(property, phaseLabels, basis, values);
    }

    /// <summary>为混合物设置两相非恒定属性值。</summary>
    /// <remarks><para>SetTwoPhaseProp 函数的 values 参数可以是包含一个或多个要设置的属性数值
    /// 的 CapeArrayDouble 对象，例如 k 值，也可以是用于设置由更复杂数据结构描述的两相属性
    /// 的 CapeInterface 对象，例如分布式属性。</para>
    /// <para>尽管通过调用 SetTwoPhaseProp 设置的一些属性将具有单个数值，但数值类型的值参数类型
    /// 为 CapeArrayDouble。在这种情况下，即使值参数仅包含一个元素，也必须以包含数组的形式调用该方法。</para>
    /// <para>SetTwoPhaseProp 方法设置的物理属性值取决于两个相，例如表面张力或 K 值。取决于单一相的属性则
    /// 通过 SetSinglePhaseProp 方法进行设置。</para>
    /// <para>如果指定了具有组分导数的物理属性，则导数值将按相标签的指定顺序为两个相分别设置。组成导数的返回值数量取决于属性。
    /// 例如，如果存在 N 种化合物，则表面张力导数的值向量将包含第一相的 N 个组成导数值， 随后第二相的 N 个组成导数值。
    /// 对于 K 值，将包含第一相的 N² 个导数值， 随后第二相的 N² 个值，顺序如 7.6.2 节所定义。</para>
    /// <para>在使用 SetTwoPhaseProp 之前，所有引用的相态都必须通过 SetPresentPhases 方法创建。</para>
    /// <para>SetTwoPhaseProp 函数的 values 参数可以是包含一个或多个要设置的属性数值的 CapeArrayDouble 对象，例如 k 值，
    /// 也可以是用于设置由更复杂数据结构描述的两相属性的 CapeInterested 对象。</para></remarks>
    /// <param name="property">在材料对象中设置值的属性。该属性必须是第 7.5.6 节和第 7.6 节中包含的两相属性或其衍生属性之一。</param>
    /// <param name="phaseLabels">设置该属性的相的相标签。
    /// 相标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的两个标识符。</param>
    /// <param name="basis">结果的基础。有效设置为：“质量”用于单位质量的物理性质，或“摩尔”用于摩尔性质。
    /// 对于不适用基础的物理性质，使用 UNDEFINED 作为占位符。详情请参阅第 7.5.5 节。</param>
    /// <param name="values">为该属性设置的值（CapeArrayDouble）或 CapeInterface（参见备注）。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作虽然存在，但当前实现不支持该操作。如果 PME 不处理任何单相属性，则可能不需要该方法。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用，
    /// 即该值不属于上述有效值列表，例如属性为 UNDEFINED 时。</exception> 
    /// <exception cref="ECapeOutOfBounds">值参数中的一个或多个条目超出了物流对象接受的值范围。</exception> 
    /// <exception cref="ECapeFailedInitialisation">先决条件无效。所引用的相态未通过 SetPresentPhases 方法创建。</exception>
    /// <exception cref="ECapeUnknown">当为 SetSinglePhaseProp 操作指定的其他错误不适用时，将引发此错误。</exception>
    [Description("Method SetTwoPhaseProp")]
    void ICapeThermoMaterial.SetTwoPhaseProp(string property, string[] phaseLabels, 
        string basis, object values)
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
    /// <exception cref="ECapeNoImpl">操作 GetUniversalConstantList 尚未实现，即使出于与 CAPE-OPEN 标准
    /// 兼容性的考虑，该方法仍可被调用。也就是说，该操作确实存在，但当前实现不支持它。这种情况可能发生在属性包不支持任何
    /// 通用常量，或者不希望为属性包内可能使用的任何通用常量提供值时。</exception>
    /// <exception cref="ECapeUnknown">当为 GetUniversalConstantList 操作指定的其他错误不适用时，应引发的错误。</exception>
    string[] ICapeThermoUniversalConstant.GetUniversalConstantList()
    {
        return (string[])_pIUniversalConstant.GetUniversalConstantList();
    }

    /// <summary>返回相数。</summary>
    /// <returns>支持的相态数量。</returns>
    /// <remarks>此方法返回的相位数量必须等于此接口的 GetPhaseList 方法返回的相位标签数量。该值必须为零或正数。</remarks>
    /// <exception cref="ECapeNoImpl">即使出于与CAPE-OPEN标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    int ICapeThermoPhases.GetNumPhases()
    {
        return _pIPhases.GetNumPhases();
    }

    /// <summary>返回与某个相态相关的属性信息，以便理解该相态标签背后的含义。</summary>
    /// <param name="phaseLabel">单相标签。此标签必须是 GetPhaseList 方法返回的值之一。</param>
    /// <param name="phaseAttribute">下表中所列的相态属性标识符之一。</param>
    /// <returns>与相态属性标识符对应的值 – 参见下表。</returns>
    /// <remarks><para>GetPhaseInfo 旨在允许 PME 或其他客户端识别具有任意标签的相态。
    /// PME 或其他客户端需要执行此操作，以将流数据映射到材料对象，或在导入属性包时。如果客户端
    /// 无法识别相态，它可以要求用户根据这些属性的值提供映射。</para>
    /// <para>支持的相态属性列表如下表所示：</para>
    /// <para>例如，支持气相、有机液相和水相的属性包组件可能会返回以下信息：
    /// <para>Phase label, Gas, Organic, Aqueous</para>
    /// <para>StateOfAggregation, Vapor, Liquid, Liquid</para>
    /// <para>KeyCompoundId, UNDEFINED, UNDEFINED, Water</para>
    /// <para>ExcludedCompoundId, UNDEFINED, Water, UNDEFINED</para>
    /// <para>DensityDescription, UNDEFINED, Light, Heavy</para>
    /// <para>UserDescription, The gas Phase, The organic liquid Phase, The aqueous liquid Phase</para>
    /// <para>TypeOfSolid, UNDEFINED, UNDEFINED, UNDEFINED</para>
    /// </para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作虽然存在，但当前实现中并未支持该操作。</exception>
    /// <exception cref="ECapeInvalidArgument">phaseLabel 未被识别，或为未定义，或相位属性未被识别。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
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
    /// 否则该值将被定义为未定义，此时将返回 UNDEFINED 值。关键化合物指的是在该相中预期以高浓度存在的化合物，
    /// 例如水在水相中。每个相只能有一个关键化合物。</param>
    /// <remarks><para>相标签可用于在 ICapeThermoPhases 接口及其他 CAPE-OPEN 接口的方法中唯一标识相。
    /// 聚集态和关键化合物为 PME 或其他客户端提供了通过相的物理特性来解释相标签含义的途径。</para>
    /// <para>此方法返回的所有数组必须具有相同的长度，即等于相位标签的数量。</para>
    /// <para>要获取有关某个相态的更多信息，请使用 GetPhaseInfo 方法。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoPhases.GetPhaseList(ref string[] phaseLabels, ref string[] stateOfAggregation, ref string[] keyCompoundId)
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
    /// <para>如果请求的物理性质数量为 P，化合物数量为 C，则 propVals 数组将包含 C*P 个变体。前 C 个变体将是第一个请求的
    /// 物理性质的值（每个化合物一个变体），接着是第二个物理性质的 C 个常量值，依此类推。实际返回的值类型（双精度浮点数、字符串等）
    /// 取决于物理性质，具体请参见第 7.5.2 节的说明。</para>
    /// <para>Physical Properties are returned in a fixed set of units as specified 
    /// in section 7.5.2.</para>
    /// <para>如果 compIds 参数设置为 UNDEFINED，则这是一个请求，要求返回实现 ICapeThermoCompounds 接口的组件中所有化合物
    /// 的属性值，其化合物顺序与 GetCompoundList 方法返回的化合物顺序相同。例如，如果接口由属性包组件实现，则 compIds 设置
    /// 为 UNDEFINED 的属性请求意味着属性包中的所有化合物，而不是传递给属性包的物流对象中的所有复合物。</para>
    /// <para>如果一个或多个化合物的任何物理属性不可用，则必须为这些组合返回未定义的值，并且必须引发
    /// ECapeThrmPropertyNotAvailable 异常。如果引发异常，客户端应检查返回的所有值，以确定哪个值未定义。</para></remarks>
    /// <param name="props">物理属性标识符列表。第 7.5.2 节列出了恒定物理性质的有效标识符。</param>
    /// <param name="compIds">要检索常数的化合物标识符列表。设置 compIds = UNDEFINED 以
    /// 表示实现 ICapeThermoCompounds 接口的组件中的所有化合物。</param>
    /// <returns>指定化合物的常数值。</returns>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因可以调用此方法，但“未”实现操作 GetCompoundConstant。
    /// 也就是说，该操作存在，但当前实现不支持。如果不支持任何化合物或属性，则应提出此例外情况。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">物理性质列表中至少有一项不适用于特定化合物。
    /// 此异常应被视为警告，而不是错误。</exception>
    /// <exception cref="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。如果 props 论证的任何元素未被识别，
    /// 也应提出此例外情况，因为第7.5.2节中的物理属性列表并不详尽，未识别的物理属性标识符可能有效。
    /// 如果根本不支持物理属性，则应引发 ECapeNoImpl（见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，
    /// 例如，props 参数的无法识别的 Compound 标识符或 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不适用时引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetCompoundConstant 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。当由物流对象实现 GetCompoundConstant 方法时，不会引发错误。</exception>
    object[] ICapeThermoCompounds.GetCompoundConstant(string[] props, string[] compIds)
    {
        return (object[])_pICompounds.GetCompoundConstant(props, compIds);
    }

    /// <summary>返回所有化合物的列表。这包括识别的化合物标识符和可用于进一步识别化合物的额外信息。</summary>
    /// <remarks><para>如果无法返回任何项目，则应将该值设置为 UNDEFINED。同样的信息也可以使用 GetCompoundConstant 方法提取。
    /// 第 7.5.2 节中规定的 GetCompoundList 参数和复合常数物理属性之间的等效关系如下：</para>
    /// <para>compIds - 没有等价性。compIds 是一个工件，由实现 GetCompoundList 方法的组件分配。此字符串通常包含一个唯一的
    /// 化合物标识符，如“苯”。它必须用于 ICapeThermoFounds 和 ICapeThermoMaterial 接口方法中名为“compIds”的所有参数中。</para>
    /// <para>Formulae - chemicalFormula</para>
    /// <para>names - iupacName</para>
    /// <para>boilTemps - normalBoilingPoint</para>
    /// <para>molwts - molecularWeight</para>
    /// <para>casnos casRegistryNumber</para>
    /// <para>当 ICapeThermoCompounds 接口由物流对象实现时，配置物流对象时返回的化合物列表是固定的。</para>
    /// <para>对于属性包组件，属性包通常包含为特定应用选择的有限化合物集，而不是专有属性系统可用的所有可能化合物。</para>
    /// <para>为了识别属性包的化合物，PME 或其他客户端将使用 casnos 参数而不是 compIds。这是因为不同的 PME 可能会给相同的
    /// 化合物起不同的名字，而且 casnos（几乎总是）是唯一的。如果 casnos 不可用（例如石油馏分）或不是唯一的，
    /// 则可使用 GetCompoundList 返回的其他信息来区分化合物。但是，应该注意的是，对于与属性包的通信，
    /// 客户端必须使用 compIds 参数中返回的 Compound 标识符。</para></remarks>
    /// <param name="compIds">化合物标识符列表。</param>
    /// <param name="formulae">化合物分子式列表。</param>
    /// <param name="names">化合物名称列表。</param>
    /// <param name="boilTemps">沸点温度列表。</param>
    /// <param name="molwts">摩尔质量列表。</param>
    /// <param name="casnos">化学文摘服务（CAS）登记号列表。</param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因可以调用此方法，
    /// 但“未”实现操作 GetCompoundList。也就是说，该操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeUnknown">当为 GetCompoundList 操作指定的其他错误不适用时引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetCompoundList 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。当由物流对象实现 GetCompoundList 方法时，不会引发错误。</exception>
    void ICapeThermoCompounds.GetCompoundList(ref string[] compIds, ref string[] formulae, 
        ref string[] names, ref double[] boilTemps, ref double[] molwts, ref string[] casnos)
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

    /// <summary>返回支持的常量物理属性列表。</summary>
    /// <returns>所有支持的常量物理属性的标识符列表。第 7.5.2 节列出了标准常数属性标识符。</returns>
    /// <remarks><para>MGetConstPropList 返回所有常量物理属性的标识符，这些标识符可以通过 GetCompoundConstant 方法检索。
    /// 如果不支持任何属性，则应返回 UNDEFINED。CAPE-OPEN 标准没有定义实现 ICapeThermoFounds 接口的软件组件提供的物理性能的最低列表。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可能会返回不属于第 7.5.2 节中定义的列表的恒定物理性质标识符。</para>
    /// <para>然而，该组件的大多数客户端可能无法理解这些专有标识符。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因可以调用此方法，
    /// 但“未”实现操作 GetConstPropList。也就是说，该操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeUnknown">当为 Get-ConstPropList 操作指定的其他错误不适用时引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetConstPropList 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。当由物流对象实现 GetConstPropList 方法时，不会引发错误。</exception>
    string[] ICapeThermoCompounds.GetConstPropList()
    {
        return (string[])_pICompounds.GetConstPropList();
    }

    /// <summary>返回支持的化合物数量。</summary>
    /// <returns>支持的化合物数量。</returns>
    /// <remarks>此方法返回的化合物数量必须等于此接口的 GetCompoundList 方法返回的复合标识符数量。它必须是零或正数。</remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因可以调用此方法，
    /// 该操作也“未”实现。也就是说，该操作存在，但当前实现不支持。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetNumCompounds 方法之前调用 SetMaterial 方法，
    /// 则会引发错误。当 GetNumCompounds 方法由物流对象实现时，不会引发错误。</exception>
    int ICapeThermoCompounds.GetNumCompounds()
    {
        return _pICompounds.GetNumCompounds();
    }

    /// <summary>返回指定纯化合物的压力相关物理性质值。</summary>
    /// <param name="props">物理属性标识符的列表。压力相关物理特性的有效标识符在 7.5.4 节中列出。</param>
    /// <param name="pressure">评估物理性质的压力（单位 Pa）。</param>
    /// <param name="compIds">要检索其物理属性的复合标识符列表。设置 compIds = UNDEFINED 表示
    /// 实现 ICapeThermoCompounds 接口的组件中的所有化合物。</param>
    /// <param name="propVals">属性值指定的化合物。</param>
    /// <remarks><para>可以使用 GetPDependentPropList 方法来检查哪些物理属性可用。</para>
    /// <para>如果请求的物理属性的数量是 P，而化合物的数量是 C，那么 propVals 数组将包含 C*P 的值。
    /// 第一个 C 将是第一个请求的物理属性的值，然后是第二个物理属性的 C 值，依此类推。</para>
    /// <para>物理属性以 7.5.4 节规定的一组固定单位返回。</para>
    /// <para>如果 compIds 参数被设置为 UNDEFINED，则请求为实现 ICapeThermoCompounds 接口的组件中的所有化合物返回属性值，
    /// 其复合顺序与 GetCompoundList 方法返回的顺序相同。例如，如果接口是由物性包组件实现的，那么 compIds 设置
    /// 为 UNDEFINED 的属性请求意味着物性包中的所有化合物，而不是传递给 Property 包的物流中的所有化合物。</para>
    /// <para>如果任何物理属性对于一个或多个化合物不可用，则必须为这些组合返回未定义的值，
    /// 并且必须引发 ECapeThrmPropertyNotAvailable 异常。如果引发异常，客户端应该检查所有返回的值，以确定哪些是未定义的。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。
    /// 也就是说，该操作存在，但当前实现不支持它。如果不支持化合物或物理性质，则会引发此异常。</exception>
    /// <exception cref="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。如果 props 参数的任何元素没有被识别，
    /// 这个异常也应该被引发（而不是 ECapeInvalidArgument），因为第 7.5.4 节中的物理属性列表并不是详尽的，一个未被识别的物理
    /// 属性标识符可能是有效的。如果根本不支持物理属性，则应该引发 ECapeImpl（见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">用于处理传递的无效参数值，例如对于参数属性为“UNDEFINED”。</exception>
    /// <exception cref="ECapeOutOfBounds">该压力值超出了“属性包”所接受的值范围。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">属性列表中至少有一项对特定化合物不可用。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不适用时引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetPDependentProperty 方法之前调用 SetMaterial 方法，
    /// 将引发的错误。当 GetPDependentProperty 方法由物流对象实现时，不会引发该错误。</exception>
    void ICapeThermoCompounds.GetPDependentProperty(string[] props, double pressure, string[] compIds, ref double[] propVals)
    {
        object obj1 = null;
        _pICompounds.GetPDependentProperty(props, pressure, compIds, ref obj1);
        propVals = (double[])obj1;
    }

    ///<summary>返回支持的与压力相关的属性的列表。</summary>
    ///<returns>所有支持的压力相关属性的物理属性标识符列表。标准标识符在第 7.5.4 节中列出。</returns>
    /// <remarks><para>GetPDependentPropList 返回可通过 GetPDependentProperty 方法检索的所有压力相关属性的标识符。
    /// 如果不支持任何属性，则应返回 UNDEFINED。CAPE-OPEN 标准没有定义一个物理属性的最小列表，
    /// 该列表要由实现 ICapeThermoCompounds 接口的软件组件提供。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可以返回不属于第 7.5.4 节中定义的列表的标识符。
    /// 但是，该组件的大多数客户端可能无法理解这些专有标识符。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。
    /// 也就是说，该操作存在，但当前实现不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不适用时引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetPDependentPropList 方法之前调用 SetMaterial 方法，
    /// 将引发的错误。当 GetPDependentPropList 方法由物流对象实现时，不会引发该错误。</exception>
    string[] ICapeThermoCompounds.GetPDependentPropList()
    {
        return (string[])_pICompounds.GetPDependentPropList();
    }

    /// <summary>返回指定纯化合物的与温度相关的物理属性的值。</summary>
    /// <param name="props">物理属性标识符的列表。与温度相关的物理属性的有效标识符在 7.5.3 节中列出。</param>
    /// <param name="temperature">评价性质的温度（以 K 为单位）。</param>
    /// <param name="compIds">要检索其物理属性的复合标识符列表。设置 compIds = UNDEFINED 表示
    /// 实现 ICapeThermoCompounds 接口的组件中的所有化合物。</param>
    /// <param name="propVals">指定化合物的物理属性值。</param>
    /// <remarks><para>可以使用 GetTDependentPropList 方法来检查哪些物理属性可用。</para>
    /// <para>如果请求的物理属性的数量为 P，化合物的数量为 C，则 propVals 数组将包含 C*P 值。
    /// 第一个 C 将是第一个请求的物理属性的值，然后是第二个物理属性的 C 值，依此类推。</para>
    /// <para>属性以一组固定的单位返回，如第 7.5.3 节所述。</para>
    /// <para>如果 compIds 参数被设置为 UNDEFINED，则请求为实现 ICapeThermoCompounds 接口的组件中的所有化合物返回属性值，
    /// 其复合顺序与 GetCompoundList 方法返回的顺序相同。例如，如果接口是由物性包组件实现的，那么 compIds 设置
    /// 为 UNDEFINED 的属性请求意味着物性包中的所有化合物，而不是传递给物性包的物流对象中的所有化合物。</para>
    /// <para>如果任何物理属性对于一个或多个化合物不可用，则必须为这些组合返回未定义的值，并且必须
    /// 引发 ECapeThrmPropertyNotAvailable 异常。如果引发异常，客户端应该检查所有返回的值，以确定哪些是未定义的。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使由于与 CAPE-OPEN 标准兼容的原因可以调用该方法，也“不”执行该操作。
    /// 也就是说，该操作存在，但当前实现不支持它。如果不支持化合物或物理性质，则会引发此异常。</exception>
    /// <exception cref="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。如果 props 参数的任何元素未被识别，
    /// 也应该引发此异常（而不是 ECapeInvalidArgument），因为第 7.5.3 节中的属性列表不是详尽的，并且未被识别的物理属性标识符
    /// 可能是有效的。如果根本不支持任何属性，应该引发 ECapeImpl（见上文）。</exception>
    /// <exception cref="ECapeInvalidArgument">用于处理传递的无效参数值，例如对于参数属性为“UNDEFINED”。</exception> 
    /// <exception cref="ECapeOutOfBounds">温度的值超出了属性包接受的值范围。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">属性列表中至少有一项对特定化合物不可用。</exception>
    /// <exception cref="ECapeUnknown">当为操作指定的其他错误不适用时引发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetTDependentProperty 方法之前调用 SetMaterial 方法，
    /// 将引发的错误。当由物流对象实现 GetTDependentProperty 方法时，不会引发该错误。</exception>
    void ICapeThermoCompounds.GetTDependentProperty(string[] props, 
        double temperature, string[] compIds, ref double[] propVals)
    {
        object obj1 = null;
        _pICompounds.GetTDependentProperty(props, temperature, compIds, ref obj1);
        propVals = (double[])obj1;
    }

    /// <summary>返回支持的温度依赖性物理属性列表。</summary>
    /// <returns>所有支持的温度依赖性物理性质的物理性质标识符列表。标准标识符在第 7.5.3 节中列出。</returns>
    /// <remarks><para>GetTDependentPropList 方法返回所有可通过 GetTDependentProperty 方法获取的温度依赖型物理属性的标识符。
    /// 若不支持任何属性，应返回 UNDEFINED。CAPE-OPEN 标准未定义实现 ICapeThermoCompounds 接口的软件组件必须提供的属性最小列表。</para>
    /// <para>实现 ICapeThermoCompounds 接口的组件可能会返回不属于第 7.5.3 节中定义的列表的标识符。
    /// 然而，这些专有标识符可能无法被该组件的大多数客户端理解。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当操作中指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref="ECapeBadInvOrder">如果属性包要求在调用 GetTDependentPropList 方法之前先调用 SetMaterial 方法，
    /// 则会引发此错误。当 GetTDependentPropList 方法由物流对象实现时，此错误不会被引发。</exception>
    string[] ICapeThermoCompounds.GetTDependentPropList()
    {
        return (string[])_pICompounds.GetTDependentPropList();
    }
    
    /// <summary>该方法可用于计算单相混合物中逸度系数（及其导数）的自然对数。
    /// 温度、压力和成分的值在参数列表中指定，结果也通过参数列表返回。</summary>
    /// <param name="phaseLabel">要为其计算属性的阶段的阶段标签。
    /// 相位标签必须是 ICapeThermoPhases 接口上的 GetPhaseList 方法返回的字符串之一。</param>
    /// <param name="temperature">温度(K)进行计算。</param>
    /// <param name="pressure">计算压力（Pa）。</param>
    /// <param name="lnPhiDt">逸度系数的自然对数相对于温度的导数（如果需要）。</param>
    /// <param name="moleNumbers">混合物中每种化合物的摩尔数。</param>
    /// <param name="fFlags">指示是否应计算逸度系数和/或导数的自然对数的代码（见注释）。</param>
    /// <param name="lnPhi">逸度系数的自然对数（如有要求）。</param>
    /// <param name="lnPhiDp">逸度系数的自然对数对压力的导数（如有要求）。</param>
    /// <param name="lnPhiDn">逸度系数的自然对数与摩尔数的导数（如有要求）。</param>
    /// <remarks><para>提供这种方法是为了以高效的方式计算和返回逸度系数的自然对数，逸度系数是最常用的热力学性质。</para>
    /// <para>计算的温度、压力和成分（摩尔数）由参数指定，而不是通过单独的请求从实物中获得。同样，计算出的任何数量都是通过参数返回的，
    /// 不会存储在物流对象中。调用此方法不会影响材质对象的状态。但是，应该注意的是，在调用 CalcAndGetLnPhi 之前，必须通过调用
    /// 实现 ICapeThermoPropertyRoutine 接口的组件的 ICapeThermoMaterialContext 接口上的 SetMaterial 方法来定义有效的
    /// 物流对象。材质对象中的化合物必须已标识，moleNumbers 参数中提供的值的数量必须等于材质对象中化合物的数量。</para>
    /// <para>逸度系数信息作为逸度系数的自然对数返回。这是因为热力学模型自然地提供了这个量的自然对数，并且可以安全地返回更大范围的值。</para>
    /// <para>此方法实际计算和返回的量由整数代码 fFlags 控制。该代码是通过使用下表所示的枚举常数 eCapeCalculationCode
    /// （在 Thermo 1.1 版 IDL 中定义）对属性的贡献和所需的每个导数求和而形成的。例如，为了计算对数逸度系数及其 T 导数，
    /// fFlags 参数将设置为 CAPE_LOG_FUGACITY_COEFFICIENTS + CAPE_T_DERIVATIVE。</para>
    /// <table border="1">
    /// <tr><th>Calculation Type</th><th>Enumeration Value</th><th>Numerical Value</th></tr>
    /// <tr><td>no calculation</td><td>CAPE_NO_CALCULATION</td><td>0</td></tr>
    /// <tr><td>log fugacity coefficients</td><td>CAPE_LOG_FUGACITY_COEFFICIENTS</td><td>1</td></tr>
    /// <tr><td>T-derivative</td><td>CAPE_T_DERIVATIVE</td><td>2</td></tr>
    /// <tr><td>P-derivative</td><td>CAPE_P_DERIVATIVE</td><td>4</td></tr>
    /// <tr><td>mole number derivatives</td><td>CAPE_MOLE_NUMBERS_DERIVATIVES</td><td>8</td></tr>
    /// </table>	
    /// <para>如果调用 CalcAndGetLnPhi 时 fFlags 设置为 CAPE_NO_CALCULATION，则不会返回任何属性值。</para>
    /// <para>当由属性包组件实现时，此方法的典型操作顺序为：</para>
    /// <para>1. 请确认指定的相位标签有效。</para>
    /// <para>2. 请确认 moleNumbers 数组中包含的值数量与预期一致（应与上次调用 SetMaterial 方法时的值一致）。</para>
    /// <para>3. 根据参数列表中指定的温度/压力/组分，计算所需的物理性质/导数。</para>
    /// <para>4. 将属性/导数的值存储在相应的参数中。</para>
    /// <para>请注意，此计算可独立于物流对象中是否实际存在该相而进行。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeLimitedImpl">如果请求的属性之一或多个无法返回，因为相关计算尚未实现，则会触发该异常。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行该操作请求之前，未调用必要的先决条件操作。
    /// 例如，在调用此方法之前，未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。例如，相的组成未定义，
    /// 物流对象中的化合物数量为零或与 moleNumbers参数不一致，或任何其他必要的输入信息不可用。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">请求的属性中至少有一个属性无法返回。
    /// 这可能是因为该属性在指定的条件下或指定的阶段无法计算。如果属性计算未实现，则应返回 ECapeLimitedImpl。</exception>
    /// <exception cref="ECapeSolvingError">其中一个属性计算失败。
    /// 例如，如果模型中的某个迭代求解过程已耗尽迭代次数，或收敛到一个错误的解。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如未识别的值，或 phaseLabel 参数为 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
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

    /// <summary>CalcSinglePhaseProp 用于计算在当前温度、压力和组分值下，
    /// 物流对象中单相混合物的性质及其导数。CalcSinglePhaseProp 不进行相平衡计算。</summary>
    /// <param name="props">用于计算单相属性或导数的标识符列表。请参阅第 7.5.5 节和第 7.6 节以获取标准标识符。</param>
    /// <param name="phaseLabel">用于计算属性的相的相标签。相标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的字符串之一。</param>
    /// <remarks><para>CalcSinglePhaseProp 用于计算仅定义于单一相的物理性质，例如焓或粘度。
    /// 对于依赖于多个相的物理性质，例如表面张力或 K 值，则由 CalcTwoPhaseProp 方法处理。</para>
    /// <para>实现此方法的组件必须从关联的物流对象获取计算所需的输入规格（温度、压力和成分），并将计算结果设置到物流对象中。</para>
    /// <para>热力学和物理性质组件（如属性包或属性计算器）必须实现 ICapeThermoMaterialContext 接口，
    /// 以便通过 SetMaterial 方法传递 ICapeThermoMaterial 接口。</para>
    /// <para>当 CalcSinglePhaseProp 由属性包组件实现时，其典型操作序列如下：</para>
    /// <para>1. 请确认指定的相位标签有效。</para>
    /// <para>2. 使用 GetTPFraction 方法（该方法属于在最后一次调用 SetMaterial 方法时指定的物流对象）来获取指定相的温度、压力和组成。</para>
    /// <para>3. 计算其物理性质。</para>
    /// <para>4. 使用 ICapeThermoMaterial 接口的 SetSinglePhaseProp 方法，将相的属性值存储在材料对象中。</para>
    /// <para>CalcSinglePhaseProp 将通过 GetSinglePhaseProp 调用从物流对象请求所需的属性值。如果请求的属性不可用，
    /// 将抛出 ECapeThrmPropertyNotAvailable 异常。如果发生此错误，属性包可以将其返回给客户端，或请求不同的属性。
    /// 物流对象的实现必须能够根据客户端选择的基础提供属性值，通过实现从一种基础到另一种基础的转换来实现这一点。</para>
    /// <para>客户不应假设物流对象中的相分数和复合分数已归一化。分数值也可能超出 0 到 1 的范围。如果分数未归一化，
    /// 或超出预期范围，则属性包有责任决定如何处理该情况。</para>
    /// <para>建议逐个请求属性以简化错误处理。然而，也承认在某些情况下，同时请求多个属性所带来的潜在效率提升更为重要。
    /// 例如，当需要某个属性及其衍生属性时，便属于此类情况。</para>
    /// <para>如果客户端在一次调用中使用了多个属性，而其中一个属性失败，则整个调用应被视为失败。
    /// 这意味着，在确定整个请求可以满足之前，属性包不应将任何值写回材料对象。</para>
    /// <para>PME 可能需要在温度、压力和组分条件下获取相的属性值，而根据用于表示属性的数学/物理模型，
    /// 该相在这些条件下并不存在。此时，可能会触发异常 ECapeThrmPropertyNotAvailable，或返回外推值。</para>
    /// <para>决定如何处理这种情况是执行者的责任。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因可以调用该方法，该操作“仍”未实现。
    /// 也就是说，该操作存在，但当前实现并不支持它。</exception>
    /// <exception cref="ECapeLimitedImpl">如果请求的属性之一或多个无法返回，因为该属性的计算尚未实现，则会引发此异常。
    /// 如果 props 参数未被识别，也应抛出此异常（而非 ECapeInvalidArgument），因为第 7.5.5 节中列出的属性列表并非旨在详尽无遗，
    /// 且未识别的属性标识符可能有效。如果完全不支持任何属性，应抛出 ECapeNoImpl 异常（参见上文）。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行该操作请求之前，未调用必要的先决条件操作。
    /// 例如，在调用此方法之前，未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception> 
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。例如，相的组成未定义，或任何其他必要的输入信息不可用。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">请求的属性中至少有一个属性无法返回。这可能是因为该属性在指定的条件下
    /// 或指定的阶段无法计算。如果属性计算未实现，则应返回 ECapeLimitedImpl。</exception>
    [DispId(0x00000002)]
    [Description("Method CalcSinglePhaseProp")]
    void ICapeThermoPropertyRoutine.CalcSinglePhaseProp(string[] props, string phaseLabel)
    {
        _pIPropertyRoutine.CalcSinglePhaseProp(props, phaseLabel);
    }

    /// <summary>CalcTwoPhaseProp 用于计算混合物性质及其导数，这些性质和导数依赖于当前温度、压力和组分值，
    /// 这些值在物流对象中设置。它不进行平衡计算。</summary>
    /// <param name="props">待计算属性的一组标识符。该列表必须包含一个或多个支持的两相属性及其导数
    /// （由 GetTwoPhasePropList 方法提供）。两相属性的标准标识符在第7.5.6节和第7.6节中给出。</param>
    /// <param name="phaseLabels">用于计算性质的相的相标签。相标签必须是 ICapeThermoPhases 接口
    /// 上 GetPhaseList方法返回的字符串中的两个。</param>
    /// <remarks><para>CalcTwoPhaseProp 用于计算表面张力或 K 值等性质的数值。与单一相相关的性质则
    /// 由 ICapeThermoPropertyRoutine 接口中的 CalcSinglePhaseProp 方法处理。实现该方法的组件必须从关联的物流对象中
    /// 获取计算所需的输入参数（温度、压力和组成），并将计算结果设置到物流对象中。</para>
    /// <para>组件（如属性包或属性计算器）必须实现 ICapeThermoMaterialContext 接口，
    /// 以便可以通过 SetMaterial 方法传递 ICapeThermoMaterial 接口。</para>
    /// <para>当 CalcTwoPhaseProp 由属性包组件实现时，其典型操作序列如下：</para>
    /// <para>1. 请确认指定的相位标签（phaseLabels）是否有效。</para>
    /// <para>2. 使用 GetTPFraction 方法（该方法属于在最后一次调用 SetMaterial 方法时指定的物流对象）来
    /// 获取指定相的温度、压力和组成。</para>
    /// <para>3. 计算相关物性。</para>
    /// <para>4. 使用 ICapeThermoMaterial 接口的 SetTwoPhaseProp 方法为物流对象的属性存储值。</para>
    /// <para>CalcTwoPhaseProp 将通过 GetTPFraction 或 GetSinglePhaseProp 调用从材料对象中获取所需的值。
    /// 如果请求的属性不可用，将抛出 ECapeThrmPropertyNotAvailable 异常。如果发生此错误，属性包可以将其返回给客户端，
    /// 或请求不同的属性。材料对象的实现必须能够通过实现从一种基准到另一种基准的转换，使用客户端选择的基准提供属性值。</para>
    /// <para>客户不应假设材料对象中的相分数和复合分数已归一化。分数值也可能超出0到1的范围。如果分数未归一化，
    /// 或超出预期范围，则属性包有责任决定如何处理该情况。</para>
    /// <para>建议逐个请求属性以简化错误处理。然而，也承认在某些情况下，同时请求多个属性所带来的潜在效率提升更为重要。
    /// 例如，当需要某个属性及其衍生属性时，便是其中一种情况。</para>
    /// <para>如果客户端在一次调用中使用了多个属性，而其中一个属性失败，则整个调用应被视为失败。
    /// 这意味着，在确定整个请求可以满足之前，属性包不应将任何值写回材料对象。</para>
    /// <para>CalcTwoPhaseProp 函数必须针对每种相组的组合单独调用。例如，气液 K 值必须与液液 K 值分开计算，分别通过单独的调用进行计算。</para>
    /// <para>两相性质在所有相的温度和压力不相同的情况下可能没有意义。属性包有责任检查此类条件，并在必要时触发异常。</para>
    /// <para>PME 可能需要在温度、压力和组分条件下获取相的属性值，而此时一个或两个相可能不存在（根据用于表示属性的数学/物理模型）。
    /// 此时可能抛出异常 ECapeThrmPropertyNotAvailable，或返回外推值。如何处理此情况由实现者决定。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeLimitedImpl">如果请求的属性之一或多个无法返回，因为该属性的计算尚未实现，则会引发此异常。
    /// 如果 props 参数未被识别，也应抛出此异常（而非 ECapeInvalidArgument），因为第 7.5.6 节中列出的属性列表并非旨在
    /// 涵盖所有情况，且未识别的属性标识符可能有效。如果完全不支持任何属性，应抛出 ECapeNoImpl 异常（参见上文）。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行该操作请求之前，未调用必要的先决条件操作。
    /// 例如，在调用此方法之前，未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。
    /// 例如，其中一个阶段的组成未定义，或任何其他必要的输入信息不可用。</exception>
    /// <exception cref="ECapeThrmPropertyNotAvailable">请求的属性中至少有一个属性无法返回。
    /// 这可能是因为该属性在指定的条件下或指定的阶段无法计算。如果属性计算未实现，则应返回 ECapeLimitedImpl。</exception>
    /// <exception cref="ECapeSolvingError">其中一个属性计算失败。
    /// 例如，如果模型中的某个迭代求解过程已耗尽迭代次数，或收敛到一个错误的解。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递无效的参数值时使用，例如 phaseLabels 参数的值为未识别的值
    /// 或 UNDEFINED，或 props 参数的值为 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    [DispId(0x00000003)]
    [Description("Method CalcTwoPhaseProp")]
    void ICapeThermoPropertyRoutine.CalcTwoPhaseProp(string[] props, string[] phaseLabels)
    {
        _pIPropertyRoutine.CalcTwoPhaseProp(props, phaseLabels);
    }

    /// <summary>检查是否可以使用 CalcSinglePhaseProp 方法为给定的相计算属性。</summary>
    /// <param name="property">要检查的属性的标识符。要有效，这必须是支持的单相属性或其衍生属性之一
    /// （由 GetSinglePhasePropList 方法提供）。</param>
    /// <param name="phaseLabel">计算校验的相位标签。此标签必须是 ICapeThermoPhases 接口的 GetPhaseList 方法返回的标签之一。</param>
    /// <returns>如果属性与相位标签的组合受支持，则布尔值设置为 True；否则设置为 False。</returns>
    /// <remarks><para>检查结果应仅取决于实现 ICapeThermoPropertyRoutine 接口的组件（例如属性包）的特性和
    /// 配置（存在的化合物和相）。它不应取决于是否已设置物流对象，也不应取决于物流对象的状态（温度、压力、组成等）或配置。</para>
    /// <para>预计 PME 或其他客户端将使用此方法在导入包时检查该包是否支持其所需的属性。如果缺少任何必要属性，应终止导入过程。</para>
    /// <para>如果实现 ICapeThermoPropertyRoutine 接口的组件无法识别 property 或 phaseLabel 参数，则该方法应返回 False。</para></remarks>
    /// <exception cref="ECapeNoImpl">操作 CheckSinglePhasePropSpec 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作确实存在，但当前实现尚未支持该功能。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行操作请求之前，未调用必要的先决条件操作。
    /// 在调用此方法之前，未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。
    /// 例如，如果之前对 ICapeThermoMaterialContext 接口的 SetMaterial 方法的调用未能提供有效的物流对象。</exception>
    /// <exception cref="ECapeInvalidArgument">一个或多个输入参数无效：例如，属性参数或阶段标签参数的值为未定义。</exception>
    /// <exception cref="ECapeUnknown">当为 CheckSinglePhasePropSpec 操作指定的其他错误不适用时，应引发的错误。</exception>
    bool ICapeThermoPropertyRoutine.CheckSinglePhasePropSpec(string property, string phaseLabel)
    {
        return _pIPropertyRoutine.CheckSinglePhasePropSpec(property, phaseLabel);
    }

    /// <summary>检查是否可以使用 CalcTwoPhaseProp 方法对给定的一组相位计算属性。</summary>
    /// <param name="property">要检查的属性的标识符。要使该标识符有效，它必须是支持的两阶段属性（包括衍生属性）之一，
    /// 具体由 GetTwoPhasePropList 方法提供。</param>
    /// <param name="phaseLabels">用于计算属性的相的相标签。
    /// 相标签必须是 ICapeThermoPhases 接口上 GetPhaseList 方法返回的两个标识符。</param>
    /// <returns>布尔值：如果属性与相位标签的组合受支持，则设置为 True；否则设置为 False。</returns>
    /// <remarks><para>检查结果应仅取决于实现 ICapeThermoPropertyRoutine 接口的组件（例如属性包）的特性和配置（存在的化合物和相）。
    /// 它不应取决于是否已设置材料对象，也不应取决于材料对象的状态（温度、压力、组成等）或配置。</para>
    /// <para>预计 PME 或其他客户端将使用此方法，在导入属性包时验证该属性包是否支持其所需的属性。
    /// 若发现任何关键属性缺失，应立即终止导入过程。</para>
    /// <para>如果属性参数或 phaseLabels 参数中的值未被实现 ICapeThermoPropertyRoutine 接口的组件识别，则该方法应返回 False。</para></remarks>
    /// <exception cref="ECapeNoImpl">操作 CheckTwoPhasePropSpec 尚未实现，即使出于与 CAPE-OPEN 标准兼容性的考虑，
    /// 该方法仍可被调用。也就是说，该操作确实存在，但当前实现不支持该功能。如果系统不支持两相属性，则可能出现这种情况。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行操作请求之前，未调用必要的预先操作。在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeFailedInitialisation">属性计算的先决条件无效。
    /// 例如，如果之前对 ICapeThermoMaterialContext 接口的 SetMaterial 方法的调用未能提供有效的物流对象。</exception>
    /// <exception cref="ECapeInvalidArgument">一个或多个输入参数无效。例如，属性参数或相位标签参数的值为未定义，
    /// 或相位标签数组中的元素个数不等于两个。</exception>
    /// <exception cref="ECapeUnknown">当为 CheckTwoPhasePropSpec 操作指定的其他错误不适用时，应引发的错误。</exception>
    bool ICapeThermoPropertyRoutine.CheckTwoPhasePropSpec(string property, string[] phaseLabels)
    {
        return _pIPropertyRoutine.CheckTwoPhasePropSpec(property, phaseLabels);
    }

    /// <summary>返回支持的非常量单相物理性质列表。</summary>
    /// <returns>所有支持的非恒定单相属性标识符列表。标准的单相属性标识符在第 7.5.5 节中列出。</returns>
    /// <remarks><para>非恒定属性取决于物流对象的状态。</para>
    /// <para>单相性质，例如焓，仅取决于单相的状态。GetSinglePhasePropList 必须返回所有可以
    /// 通过 CalcSinglePhaseProp 计算的单相性质。如果可以计算导数，这些导数也必须返回。</para>
    /// <para>如果不支持单相属性，此方法应返回 UNDEFINED。</para>
    /// <para>要获取支持的两阶段属性的列表，请使用 GetTwoPhasePropList。</para>
    /// <para>实现此方法的组件可能会返回不属于第 7.5.5 节中定义的列表的非恒定单相属性标识符。
    /// 然而，这些专有标识符可能无法被该组件的大多数客户端理解。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeUnknown">当为 GetSinglePhasePropList 操作指定的其他错误不适用时，应引发的错误。</exception>
    [DispId(0x00000006)]
    [Description("Method GetSinglePhasePropList")]
    string[] ICapeThermoPropertyRoutine.GetSinglePhasePropList()
    {
        return (string[])_pIPropertyRoutine.GetSinglePhasePropList();
    }

    /// <summary>返回支持的非常量两阶段属性的列表。</summary>
    /// <returns>所有支持的非常量两相属性标识符列表。标准的两相属性标识符在第 7.5.6 节中列出。</returns>
    /// <remarks><para>非恒定属性取决于材料对象的状态。两相属性是指那些取决于两个或更多共存相的属性，例如 K 值。</para>
    /// <para>GetTwoPhasePropList 必须返回所有可以通过 CalcTwoPhaseProp 计算的属性。如果可以计算导数，这些导数也必须一并返回。</para>
    /// <para>如果不支持两相属性，此方法应返回 UNDEFINED。</para>
    /// <para>要检查一个属性是否可以针对特定的相位标签集进行评估，请使用 CheckTwoPhasePropSpec 方法。</para>
    /// <para>实现此方法的组件可能会返回不属于第 7.5.6 节中定义的列表的非恒定两阶段属性标识符。
    /// 然而，这些专有标识符可能无法被该组件的大多数客户端理解。</para>
    /// <para>要获取支持的单相属性的列表，请使用 GetSinglePhasePropList。</para></remarks>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容的原因可以调用该方法，该操作“仍”未实现。
    /// 也就是说，该操作存在，但当前实现并不支持它。</exception>
    /// <exception cref="ECapeUnknown">当为 GetTwoPhasePropList 操作指定的其他错误不适用时，应引发的错误。</exception>
    [DispId(0x00000007)]
    [Description("Method GetTwoPhasePropList")]
    string[] ICapeThermoPropertyRoutine.GetTwoPhasePropList()
    {
        return (string[])_pIPropertyRoutine.GetTwoPhasePropList();
    }
    
    /// <summary>CalcEquilibrium 用于计算平衡状态下各相的质量和组成。如果温度和/或压力不在每个平衡计算中
    /// 必须指定的两个参数范围内，CalcEquilibrium 将自动计算温度和/或压力。</summary>
    /// <remarks><para>规格1和规格2参数提供了用于检索两个规格值所需的信息，例如平衡计算中的压力和温度。
    /// 可以使用 CheckEquilibriumSpec 方法来检查支持的规格。每个规格变量包含按以下表格中定义的顺序
    /// 排列的字符串序列（因此，规格参数可能包含 3 或 4 个项目）：</para>
    /// <para>属性标识符 属性标识符可以是第 7.5.5 节中列出的任何标识符，但通常只有某些属性规范会被任何 Equilibrium 例程支持。</para>
    /// <para>基准，财产价值的基准。基准的有效设置在第 7.4 节中给出。对于不适用基准的属性，使用 UNDEFINED 作为占位符。
    /// 对于大多数平衡规范，计算结果不依赖于基准，但例如对于相分数规范，基准（摩尔或质量）确实会产生影响。</para>
    /// <para>阶段标签，阶段标签用于标识该规范适用的阶段。它必须是 GetPresentPhases 函数返回的标签之一，或特殊值“Overall”。</para>
    /// <para>复合物标识符（可选）复合物标识符用于指定依赖于特定复合物的规范。规范数组中的此项为可选项，可省略。
    /// 若规范未指定复合物标识符，数组元素可能存在且为空，也可能不存在。</para>
    /// <para>下表列举了典型相平衡规格的一些示例。</para>
    /// <para>在调用 CalcEquilibrium 之前，必须在关联的材料对象中设置与参数列表中的规格对应的值以及混合物的整体组成。</para>
    /// <para>组件（如属性包或平衡计算器）必须实现 ICapeThermoMaterialContext 接口，
    /// 以便通过 SetMaterial 方法传递 ICapeThermoMaterial 接口。CalcEquilibrium 的实现负责在进行计算前验证材料对象。</para>
    /// <para>在平衡计算中考虑的相态是材料对象中存在的相态，即在 SetPresentPhases 调用中指定的相态列表。这为客户端提供了指定所需
    /// 计算类型的途径，例如蒸气-液体、液体-液体或蒸气-液体-液体计算。CalcEquilibrium 必须使用 GetPresentPhases 方法来获取相的
    /// 列表及其相关的相状态标志。相状态标志可由客户端用于提供有关相的信息，例如是否提供了平衡状态的估计值。
    /// 有关 ICapeThermoMaterial 接口中 GetPresentPhases 和 SetPresentPhases 方法的详细描述，请参阅相关文档。当平衡计算成功
    /// 完成后，必须使用 SetPresentPhases 方法指定在平衡状态下存在的相，并将各相的状态标志设置为 Cape_AtEquilibrium。这必须包括
    /// 任何以零量存在的相，例如露点计算中的液相。</para>
    /// <para>某些类型的相平衡规格可能导致多个解。一个常见的例子是露点计算的情况。然而，CalcEquilibrium 只能通过材料对象提供一个解。
    /// solutionType 参数允许显式请求“正常”或“逆行”解。当所有规格均未包含相分数时，solutionType 参数应设置为“未指定”。</para>
    /// <para>“正常”的定义是：</para>
    /// <para>其中V_F为蒸气相分数，且导数在平衡状态下。对于“逆行”行为，</para>
    /// <para>CalcEquilibrium 必须设置所有处于平衡状态的相的质量、组成、温度和压力，以及如果未作为计算规格的一部分设置，
    /// 则必须设置整体混合物的温度和压力。CalcEquilibrium 不得设置任何其他物理性质。</para>
    /// <para>例如，在恒定压力和温度下的平衡计算中，CalcEquilibrium 可能执行以下操作序列：</para>
    /// <para>1. 使用提供的材料对象的 ICapeThermoMaterial 接口：</para>
    /// <para>2. 使用 GetPresentPhases 方法来获取平衡计算应考虑的阶段列表。</para>
    /// <para>3. 使用材料对象的 ICapeThermoCompounds 接口，通过调用 GetCompoundIds 方法来确定哪些化合物存在。</para>
    /// <para>4. 使用 GetOverallProp 方法获取整体混合物的温度、压力和组成。</para>
    /// <para>5. 进行相平衡计算。</para>
    /// <para>6. 使用 SetPresentPhases 指定平衡状态下存在的相，并将相状态标志设置为 Cape_AtEquilibrium。</para>
    /// <para>7. 使用 SetSinglePhaseProp 函数设置所有存在相的压力、温度、相含量（或相分数）和组成。</para></remarks>
    /// <param name="specification1">平衡计算的第一个规格。规格信息用于从材料对象中检索规格的值。详情请见下文。</param>
    /// <param name="specification2">平衡计算的第二项规格，格式与 specification1 相同。</param>
    /// <param name="solutionType"><para>所需解决方案类型的标识符。标准标识符如下所示：</para>
    /// <para>Unspecified</para><para>Normal</para><para>Retrograde</para>
    /// <para>这些术语的含义在下文的注释中进行定义。其他标识符可能被支持，但其解释不属于 CO 标准的一部分。</para></param>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeBadInvOrder">在执行操作请求之前，未调用必要的预先操作。在调用此方法之前，
    /// 未通过 SetMaterial 调用传递 ICapeThermoMaterial 接口。</exception>
    /// <exception cref="ECapeSolvingError">平衡计算无法求解。例如，当求解器已耗尽迭代次数，或收敛到一个平凡解时。</exception>
    /// <exception cref="ECapeLimitedImpl">如果平衡例程无法执行被要求执行的闪蒸操作，则会触发异常。例如，输入规格的值是有效的，
    /// 但例程无法在给定温度和化合物分数的情况下执行闪蒸操作。这表明可能存在对 CheckEquilibriumSpec 方法的错误使用或未使用该方法，
    /// 该方法的存在是为了防止对无法执行的计算调用 CalcEquilibrium。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递的参数值无效时使用。例如，如果规范标识符不属于已识别的标识符列表，
    /// 则会触发此异常。如果传递给参数 solutionType 的值不在三个定义值之中，或者使用 UNDEFINED 代替规范标识符，也会触发此异常。</exception>
    /// <exception cref="ECapeFailedInitialisation"><para>平衡计算的先决条件不成立。例如：</para>
    /// <para>1. 混合物的总体组成未明确规定。</para>
    /// <para>2. 材料对象（由之前对 ICapeThermoMaterialContext 接口的 SetMaterial 方法的调用设置）无效。
    /// 这可能是因为不存在相，或者存在的相未被实现 ICapeThermoEquilibriumRoutine 接口的组件识别。</para>
    /// <para>3. 其他必要的输入信息目前不可用。</para></exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoEquilibriumRoutine.CalcEquilibrium(string[] specification1, 
        string[] specification2, string solutionType)
    {
        _pIEquilibriumRoutine.CalcEquilibrium(specification1, specification2, solutionType);
    }

    /// <summary>检查属性包是否支持特定类型的平衡计算。</summary>
    /// <remarks><para>specification1、specification2 和解决方案类型参数的含义与 CalcEquilibrium 方法中的含义相同。</para>
    /// <para>检查结果应仅取决于实现 ICapeThermoEquilibriumRoutine 接口的组件（例如属性包）的特性和配置（存在的化合物和相）。
    /// 它不应取决于是否已设置材料对象，也不应取决于可能已设置的材料对象的状态（温度、压力、组成等）或配置。</para>
    /// <para>如果 solutionType、specification1 和 specification2 参数看似有效，但实际规格不被支持或不被识别，则应返回 False 值。</para></remarks>
    /// <param name="specification1">关于平衡计算的首个规范。</param>
    /// <param name="specification2">平衡计算的第二个规范。</param>
    /// <param name="solutionType">所需的解决方案类型。</param>
    /// <returns>如果规格和解决方案类型的组合被支持，则设定为 True；否则设定为 False。</returns>
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeInvalidArgument">当传递了无效的参数值时使用，
    /// 例如对于 solutionType、specification1 或 specification2 参数使用 UNDEFINED。</exception>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    bool ICapeThermoEquilibriumRoutine.CheckEquilibriumSpec(string[] specification1, 
        string[] specification2, string solutionType)
    {
        return _pIEquilibriumRoutine.CheckEquilibriumSpec(specification1, specification2, solutionType);
    }
}
