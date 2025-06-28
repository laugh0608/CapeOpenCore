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
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
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
    /// <exception cref ="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。
    /// 也就是说，该操作虽然存在，但当前实现中并未支持该操作。</exception>
    /// <exception cref ="ECapeInvalidArgument">phaseLabel 未被识别，或为未定义，或相位属性未被识别。</exception>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
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
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    /// <exception cref ="ECapeBadInvOrder">如果属性包要求在调用 GetNumCompounds 方法之前调用 SetMaterial 方法，
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
    /// <exception cref ="ECapeLimitedImpl">实现此接口的组件不支持一个或多个物理属性。如果 props 参数的任何元素没有被识别，
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
    /// <exception cref ="ECapeBadInvOrder">如果属性包要求在调用 GetPDependentPropList 方法之前调用 SetMaterial 方法，
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
    void ICapeThermoCompounds.GetTDependentProperty(string[] props, double temperature, string[] compIds, ref double[] propVals)
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
    /// <exception cref="ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists, but it is not supported
    /// by the current implementation.</exception>
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
    /// <param name="lnPhiDT">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. temperature (if requested).</param>
    /// <param name="moleNumbers">Number of moles of each Compound in the mixture.</param>
    /// <param name="fFlags">Code indicating whether natural logarithm of the 
    /// fugacity coefficients and/or derivatives should be calculated (see notes).</param>
    /// <param name="lnPhi">Natural logarithm of the fugacity coefficients (if
    /// requested).</param>
    /// <param anem = "lnPhiDT">Derivatives of natural logarithm of the fugacity
    /// coefficients w.r.t. temperature (if requested).</param>
    /// <param name="lnPhiDP">Derivatives of natural logarithm of the fugacity
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
    /// <tr><th>Calculation Type</th><th>Enumeration Value</th><th>Numerical Value</th></tr>
    /// <tr><td>no calculation</td><td>CAPE_NO_CALCULATION</td><td>0</td></tr>
    /// <tr><td>log fugacity coefficients</td><td>CAPE_LOG_FUGACITY_COEFFICIENTS</td><td>1</td></tr>
    /// <tr><td>T-derivative</td><td>CAPE_T_DERIVATIVE</td><td>2</td></tr>
    /// <tr><td>P-derivative</td><td>CAPE_P_DERIVATIVE</td><td>4</td></tr>
    /// <tr><td>mole number derivatives</td><td>CAPE_MOLE_NUMBERS_DERIVATIVES</td><td>8</td></tr>
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
    /// <exception cref ="ECapeLimitedImpl">Would be raised if the one or more of the 
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
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoPropertyRoutine.CalcAndGetLnPhi(string phaseLabel, double temperature,
        double pressure, double[] moleNumbers, CapeFugacityFlag fFlags,
        ref double[] lnPhi, ref double[] lnPhiDT, ref double[] lnPhiDP, ref double[] lnPhiDn)
    {
        object obj1 = null;
        object obj2 = null;
        object obj3 = null;
        object obj4 = null;
        int flags = (int)fFlags;
        _pIPropertyRoutine.CalcAndGetLnPhi(phaseLabel, temperature, pressure, moleNumbers, flags, ref obj1, ref obj2, ref obj3, ref obj4);
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
    /// <exception cref ="ECapeNoImpl">The operation is “not” implemented even if this
    /// method can be called for reasons of compatibility with the CAPE-OPEN standards. 
    /// That is to say that the operation exists, but it is not supported by the 
    /// current implementation.</exception>
    /// <exception cref ="ECapeLimitedImpl">Would be raised if the one or more of the 
    /// properties requested cannot be returned because the calculation (of the 
    /// particular property) is not implemented. This exception should also be raised 
    /// (rather than ECapeInvalidArgument) if the props argument is not recognised 
    /// because the list of properties in section 7.5.5 is not intended to be 
    /// exhaustive and an unrecognised property identifier may be valid. If no 
    /// properties at all are supported ECapeNoImpl should be raised (see above).</exception>
    /// <exception cref ="ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. For example, the 
    /// ICapeThermoMaterial interface has not been passed via a SetMaterial call prior 
    /// to calling this method.</exception> 
    /// <exception cref ="ECapeFailedInitialisation">The pre-requisites for the 
    /// property calculation are not valid. For example, the composition of the phases
    /// is not defined or any other necessary input information is not available.</exception>
    /// <exception cref ="ECapeThrmPropertyNotAvailable">At least one item in the 
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
    /// <exception cref="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
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
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
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
    /// <exception cref ="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref ="ECapeBadInvOrder">The necessary pre-requisite operation has 
    /// not been called prior to the operation request. The ICapeThermoMaterial interface 
    /// has not been passed via a SetMaterial call prior to calling this method.</exception>
    /// <exception cref ="ECapeSolvingError">The Equilibrium Calculation could not be 
    /// solved. For example if the solver has run out of iterations, or has converged 
    /// to a trivial solution.</exception>
    /// <exception cref ="ECapeLimitedImpl">Would be raised if the Equilibrium Routine 
    /// is not able to perform the flash it has been asked to perform. For example, 
    /// the values given to the input specifications are valid, but the routine is not 
    /// able to perform a flash given a temperature and a Compound fraction. That 
    /// would imply a bad usage or no usage of CheckEquilibriumSpec method, which is 
    /// there to prevent calling CalcEquilibrium for a calculation which cannot be
    /// performed.</exception>
    /// <exception cref ="ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed. It would be raised, for example, if a specification 
    /// identifier does not belong to the list of recognised identifiers. It would 
    /// also be raised if the value given to argument solutionType is not among 
    /// the three defined, or if UNDEFINED was used instead of a specification identifier.</exception>
    /// <exception cref ="ECapeFailedInitialisation"><para>The pre-requisites for the Equilibrium 
    /// Calculation are not valid. For example:</para>
    /// <para>• The overall composition of the mixture is not defined.</para>
    /// <para>• The Material Object (set by a previous call to the SetMaterial method of the
    /// ICapeThermoMaterialContext interface) is not valid. This could be because no 
    /// Phases are present or because the Phases present are not recognised by the
    /// component that implements the ICapeThermoEquilibriumRoutine interface.</para>
    /// <para>• Any other necessary input information is not available.</para></exception>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    void ICapeThermoEquilibriumRoutine.CalcEquilibrium(string[] specification1, string[] specification2, string solutionType)
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
    /// <exception cref ="ECapeNoImpl">即使出于与 CAPE-OPEN 标准兼容性的考虑，该方法可以被调用，但该操作“并未”实现。也就是说，该操作确实存在，但当前实现不支持该操作。</exception>
    /// <exception cref="ECapeInvalidArgument">To be used when an invalid argument 
    /// value is passed, for example UNDEFINED for solutionType, specification1 or 
    /// specification2 argument.</exception>
    /// <exception cref ="ECapeUnknown">当为该操作指定的其他错误不适用时，应触发的错误。</exception>
    bool ICapeThermoEquilibriumRoutine.CheckEquilibriumSpec(string[] specification1, string[] specification2, string solutionType)
    {
        return _pIEquilibriumRoutine.CheckEquilibriumSpec(specification1, specification2, solutionType);
    }
}
