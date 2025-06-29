/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.29
 */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CapeOpenCore.Class;

/// <summary>用于基于 COM 的C APE-OPEN ICapeThermoMaterialObject 物流对象的封装类。</summary>
/// <remarks><para>本类是基于 COM 的 CAPE-OPEN ICapeThermoMaterialObject 物流对象的封装类。
/// 当您使用此封装类时，基于 COM 的物流对象的生命周期将由系统自动管理，并且会调用物流对象的 COM Release() 方法。</para>
/// <para>此外，该方法将 <see cref="ICapeThermoMaterialObject"/> 接口中使用的 COM 变体转换为
/// 所需的 .Net 对象类型。这消除了在使用基于 COM 的 CAPE-OPEN 物流对象时需要转换数据类型的必要性。</para></remarks>
[ComVisible(false)]
[Guid("5A65B4B2-2FDD-4208-813D-7CC527FB91BD")]
[Description("ICapeThermoMaterialObject Interface")]
internal class MaterialObjectWrapper1 : CapeObjectBase, ICapeThermoMaterialObject
{
    [NonSerialized] private ICapeThermoMaterialObjectCOM _pMaterialObject;

    // 跟踪是否已调用 Dispose 方法。
    private bool _disposed;

    /// <summary>创建 MaterialObjectWrapper 类的实例</summary>
    /// <param name="materialObject">待打包的物品。</param>
    public MaterialObjectWrapper1(object materialObject)
    {
        _disposed = false;

        if (materialObject is ICapeThermoMaterialObjectCOM objectCom)
        {
            _pMaterialObject = objectCom;
        }
    }

    // 使用 C# 析构函数语法来实现最终化代码。
    /// <summary><see cref="MaterialObjectWrapper"/> 类的终结器。</summary>
    /// <remarks>这将最终确定当前类的实例。</remarks>
    ~MaterialObjectWrapper1()
    {
        // 只需调用 Dispose(false)。
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
        if (_pMaterialObject != null)
            if (_pMaterialObject.GetType().IsCOMObject)
            {
                Marshal.FinalReleaseComObject(_pMaterialObject);
            }

        _pMaterialObject = null;
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
        get => ((ICapeIdentification)_pMaterialObject).ComponentName;
        set => ((ICapeIdentification)_pMaterialObject).ComponentName = value;
    }

    /// <summary>获取并设置组件的描述。</summary>
    /// <remarks><para>系统中的某个用例可能包含多个同一类别的CAPE-OPEN组件。用户应能够为每个实例分配不同的
    /// 名称和描述，以便以无歧义且用户友好的方式引用它们。由于能够设置这些标识的软件组件与需要此信息的软件组件并
    /// 不总是由同一供应商开发，因此需要制定一个CAPE-OPEN标准来设置和获取此类信息。</para>
    /// <para>因此，组件通常不会自行设置其名称和描述：组件的使用者会进行设置。</para></remarks>
    /// <value>组件的描述。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public override string ComponentDescription
    {
        get => ((ICapeIdentification)_pMaterialObject).ComponentDescription;
        set => ((ICapeIdentification)_pMaterialObject).ComponentDescription = value;
    }

    /// <summary>提供有关对象是否支持热力学版本 1.0 的信息。</summary>
    /// <remarks><see cref="MaterialObjectWrapper1"/> 类用于检查所打包的物流对象是否支持
    /// CAPE-OPEN 1.0 版本的热力学。此属性表示该检查的结果。</remarks>
    /// <value>指示打包的物流对象是否支持CAPE-OPEN热力学版本1.0接口。</value>
    public bool SupportsThermo10 => true;

    /// <summary>提供有关对象是否支持热力学版本 1.1 的信息。</summary>
    /// <remarks><see cref="MaterialObjectWrapper1"/> 类用于检查所包裹的物流对象是否支持
    /// CAPE-OPEN 1.1 版本的热力学。此属性表示该检查的结果。</remarks>
    /// <value>指示包裹的物流对象是否支持 CAPE-OPEN 热力学版本 1.1 接口。</value>
    public bool SupportsThermo11 => false;

    /// <summary>获取打包的热力学版本 1.0 物流对象。</summary>
    /// <remarks><para>提供对热力学版本 1.0 物流对象的直接访问。</para>
    /// <para>该物流对象实现了 ICapeThermoMaterialObject 接口的 COM 版本。</para></remarks>
    /// <value>打包的热力学版本 1.0 物流对象。</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public object MaterialObject10 => _pMaterialObject;
    
    /// <summary> Gets the wrapped Thermo Version 1.1 Material Object.</summary>
    /// <remarks><para>Provides direct access to the Thermo Version 1.1 material object.</para>
    /// <para>The material object exposes the COM version of the Thermo 1.1 interfaces.</para></remarks>
    /// <value>The wrapped Thermo Version 1.1 Material Object.</value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    [Description("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [Category("CapeIdentification")]
    public object MaterialObject11
    {
        get { return null; }
    }

    /// <summary>Get the component ids for this MO</summary>
    /// <remarks>Returns the list of components Ids of a given Material Object.</remarks>
    /// <value>
    /// Te names of the compounds in the matieral object.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    String[] ICapeThermoMaterialObject.ComponentIds
    {
        get
        {
            try
            {
                return (String[])_pMaterialObject.ComponentIds;
            }
            catch (Exception p_Ex)
            {
                throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
            }
        }
    }

    ///// <summary>
    ///// Get the component ids for this MO
    ///// </summary>
    ///// <remarks>
    ///// Returns the list of components Ids of a given Material Object.
    ///// </remarks>
    ///// <returns>
    ///// Te names of the compounds in the matieral object.
    ///// </returns>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    //object ICapeThermoMaterialObjectCOM.ComponentIds
    //{
    //    get
    //    {
    //        try
    //        {
    //            return p_MaterialObject.ComponentIds;
    //        }
    //        catch (System.Exception p_Ex)
    //        {
    //            throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //        }
    //    }
    //}

    /// <summary>Get the phase ids for this MO</summary>
    /// <remarks>It returns the phases existing in the MO at that moment. The Overall phase 
    /// and multiphase identifiers cannot be returned by this method. See notes on 
    /// Existence of a phase for more information.</remarks>
    /// <value>
    /// The phases present in the material.
    /// </value>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    String[] ICapeThermoMaterialObject.PhaseIds
    {
        get
        {
            try
            {
                return (String[])_pMaterialObject.PhaseIds;
            }
            catch (Exception p_Ex)
            {
                throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
            }
        }
    }

    ///// <summary>
    ///// Get the phase ids for this MO
    ///// </summary>
    ///// <remarks>
    ///// It returns the phases existing in the MO at that moment. The Overall phase 
    ///// and multiphase identifiers cannot be returned by this method. See notes on 
    ///// Existence of a phase for more information.
    ///// </remarks>
    ///// <returns>
    ///// The phases present in the material.
    ///// </returns>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    //object ICapeThermoMaterialObjectCOM.PhaseIds
    //{
    //    get
    //    {
    //        try
    //        {
    //            return p_MaterialObject.PhaseIds;
    //        }
    //        catch (System.Exception p_Ex)
    //        {
    //            throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //        }
    //    }
    //}

    /// <summary>Get some universal constant(s)</summary>
    /// <remarks>Retrieves universal constants from the Property Package.</remarks>
    /// <returns>
    /// Values of the requested universal constants.
    /// </returns>
    /// <param name="props">
    /// List of universal constants to be retrieved.
    /// </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeNoImpl">ECapeNoImpl</exception>
    double[] ICapeThermoMaterialObject.GetUniversalConstant(String[] props)
    {
        try
        {
            return (double[])_pMaterialObject.GetUniversalConstant(props);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Get some universal constant(s)
    ///// </summary>
    ///// <remarks>
    ///// Retrieves universal constants from the Property Package.
    ///// </remarks>
    ///// <returns>
    ///// Values of the requested universal constants.
    ///// </returns>
    ///// <param name="props">
    ///// List of universal constants to be retrieved.
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    ///// <exception cref = "ECapeNoImpl">ECapeNoImpl</exception>
    //object ICapeThermoMaterialObjectCOM.GetUniversalConstant(object props)
    //{
    //    try
    //    {
    //        return p_MaterialObject.GetUniversalConstant(props);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Get some pure component constant(s)</summary>
    /// <remarks>Retrieve component constants from the Property Package. See Notes for more 
    /// information.</remarks>
    /// <returns>
    /// Component Constant values returned from the Property Package for all the 
    /// components in the Material Object It is a Object containing a 1 dimensional 
    /// array of Objects. If we call P to the number of requested properties and C to 
    /// the number requested components the array will contain C*P Objects. The C 
    /// first ones (from position 0 to C-1) will be the values for the first requested 
    /// property (one Object for each component). After them (from position C to 2*C-1) 
    /// there will be the values of constants for the second requested property, and 
    /// so on.
    /// </returns>
    /// <param name="props">
    /// List of component constants.
    /// </param>
    /// <param name="compIds">
    /// List of component IDs for which constants are to be retrieved. Use a null value 
    /// for all components in the Material Object. 
    /// </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeNoImpl">ECapeNoImpl</exception>
    object[] ICapeThermoMaterialObject.GetComponentConstant(String[] props, String[] compIds)
    {
        try
        {
            return (object[])_pMaterialObject.GetComponentConstant(props, compIds);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Get some pure component constant(s)
    ///// </summary>
    ///// <remarks>
    ///// Retrieve component constants from the Property Package. See Notes for more 
    ///// information.
    ///// </remarks>
    ///// <returns>
    ///// Component Constant values returned from the Property Package for all the 
    ///// components in the Material Object It is a Object containing a 1 dimensional 
    ///// array of Objects. If we call P to the number of requested properties and C to 
    ///// the number requested components the array will contain C*P Objects. The C 
    ///// first ones (from position 0 to C-1) will be the values for the first requested 
    ///// property (one Object for each component). After them (from position C to 2*C-1) 
    ///// there will be the values of constants for the second requested property, and 
    ///// so on.
    ///// </returns>
    ///// <param name="props">
    ///// List of component constants.
    ///// </param>
    ///// <param name="compIds">
    ///// List of component IDs for which constants are to be retrieved. Use a null value 
    ///// for all components in the Material Object. 
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    ///// <exception cref = "ECapeNoImpl">ECapeNoImpl</exception>
    //object ICapeThermoMaterialObjectCOM.GetComponentConstant(object props, object compIds)
    //{
    //    try
    //    {
    //        return p_MaterialObject.GetComponentConstant(props, compIds);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Calculate some properties</summary>
    /// <remarks>This method is responsible for doing all property calculations and delegating 
    /// these calculations to the associated thermo system. This method is further 
    /// defined in the descriptions of the CAPE-OPEN Calling Pattern and the User 
    /// Guide Section. See Notes for a more detailed explanation of the arguments and 
    /// CalcProp description in the notes for a general discussion of the method.</remarks>
    /// <param name="props">
    /// The List of Properties to be calculated.
    /// </param>
    /// <param name="phases">
    /// List of phases for which the properties are to be calculated.
    /// </param>
    /// <param name="calcType">
    /// Type of calculation: Mixture Property or Pure Component Property. For partial 
    /// property, such as fugacity coefficients of components in a mixture, use 
    /// “Mixture” CalcType. For pure component fugacity coefficients, use “Pure” 
    /// CalcType.
    /// </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref = "ECapeOutOfBounds">ECapeOutOfBounds</exception>
    /// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeThermoMaterialObject.CalcProp(String[] props, String[] phases, String calcType)
    {
        try
        {
            _pMaterialObject.CalcProp(props, phases, calcType);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }


    ///// <summary>
    ///// Calculate some properties
    ///// </summary>
    ///// <remarks>
    ///// This method is responsible for doing all property calculations and delegating 
    ///// these calculations to the associated thermo system. This method is further 
    ///// defined in the descriptions of the CAPE-OPEN Calling Pattern and the User 
    ///// Guide Section. See Notes for a more detailed explanation of the arguments and 
    ///// CalcProp description in the notes for a general discussion of the method.
    ///// </remarks>
    ///// <param name="props">
    ///// The List of Properties to be calculated.
    ///// </param>
    ///// <param name="phases">
    ///// List of phases for which the properties are to be calculated.
    ///// </param>
    ///// <param name="calcType">
    ///// Type of calculation: Mixture Property or Pure Component Property. For partial 
    ///// property, such as fugacity coefficients of components in a mixture, use 
    ///// “Mixture” CalcType. For pure component fugacity coefficients, use “Pure” 
    ///// CalcType.
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    ///// <exception cref = "ECapeSolvingError">ECapeSolvingError</exception>
    ///// <exception cref = "ECapeOutOfBounds">ECapeOutOfBounds</exception>
    ///// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    //void ICapeThermoMaterialObjectCOM.CalcProp(object props, object phases, String calcType)
    //{
    //    try
    //    {
    //        p_MaterialObject.CalcProp(props, phases, calcType);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Get some pure component constant(s)</summary>
    /// <remarks>This method is responsible for retrieving the results from calculations from 
    /// the MaterialObject. See Notesfor a more detailed explanation of the arguments.</remarks>
    /// <returns>
    /// Results vector containing property values in SI units arranged by the defined 
    /// qualifiers. The array is one dimensional containing the properties, in order 
    /// of the "props" array for each of the compounds, in order of the compIds array. 
    /// </returns>
    /// <param name="property">
    /// The Property for which results are requested from the MaterialObject.
    /// </param>
    /// <param name="phase">
    /// The qualified phase for the results.
    /// </param>
    /// <param name="compIds">
    /// The qualified components for the results. Use a null value to specify all 
    /// components in the Material Object. For mixture property such as liquid 
    /// enthalpy, this qualifier is not required. Use emptyObject as place holder.
    /// </param>
    /// <param name="calcType">
    /// The qualified type of calculation for the results. (valid Calculation Types: 
    /// Pure and Mixture)
    /// </param>
    /// <param name="basis">
    /// Qualifies the basis of the result (i.e., mass /mole). Default is mole. Use 
    /// NULL for default or as place holder for property for which basis does not 
    /// apply (see also Specific properties.
    /// </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    double[] ICapeThermoMaterialObject.GetProp(String property,
        String phase,
        String[] compIds,
        String calcType,
        String basis)
    {
        try
        {
            return (double[])_pMaterialObject.GetProp(property, phase, compIds, calcType, basis);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }


    ///// <summary>
    ///// Get some pure component constant(s)
    ///// </summary>
    ///// <remarks>
    ///// This method is responsible for retrieving the results from calculations from 
    ///// the MaterialObject. See Notesfor a more detailed explanation of the arguments.
    ///// </remarks>
    ///// <returns>
    ///// Results vector containing property values in SI units arranged by the defined 
    ///// qualifiers. The array is one dimensional containing the properties, in order 
    ///// of the "props" array for each of the compounds, in order of the compIds array. 
    ///// </returns>
    ///// <param name="property">
    ///// The Property for which results are requested from the MaterialObject.
    ///// </param>
    ///// <param name="phase">
    ///// The qualified phase for the results.
    ///// </param>
    ///// <param name="compIds">
    ///// The qualified components for the results. Use a null value to specify all 
    ///// components in the Material Object. For mixture property such as liquid 
    ///// enthalpy, this qualifier is not required. Use emptyObject as place holder.
    ///// </param>
    ///// <param name="calcType">
    ///// The qualified type of calculation for the results. (valid Calculation Types: 
    ///// Pure and Mixture)
    ///// </param>
    ///// <param name="basis">
    ///// Qualifies the basis of the result (i.e., mass /mole). Default is mole. Use 
    ///// NULL for default or as place holder for property for which basis does not 
    ///// apply (see also Specific properties.
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    //object ICapeThermoMaterialObject.GetProp(System.String property,
    //    System.String phase,
    //    object compIds,
    //    System.String calcType,
    //    System.String basis)
    //{
    //    try
    //    {
    //        return p_MaterialObject.GetProp(property, phase, compIds, calcType, basis);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Get some pure component constant(s)</summary>
    /// <remarks>This method is responsible for setting the values for properties of the 
    /// Material Object. See Notes for a more detailed explanation of the arguments.</remarks>
    /// <param name="property">
    /// The Property for which results are requested from the MaterialObject.
    /// </param>
    /// <param name="phase">
    /// The qualified phase for the results.
    /// </param>
    /// <param name="compIds">
    /// The qualified components for the results. emptyObject to specify all 
    /// components in the Material Object. For mixture property such as liquid 
    /// enthalpy, this qualifier is not required. Use emptyObject as place holder.
    /// </param>
    /// <param name="calcType">
    /// The qualified type of calculation for the results. (valid Calculation Types: 
    /// Pure and Mixture)
    /// </param>
    /// <param name="basis">
    /// Qualifies the basis of the result (i.e., mass /mole). Default is mole. Use 
    /// NULL for default or as place holder for property for which basis does not 
    /// apply (see also Specific properties.
    /// </param>
    /// <param name="values">
    /// Values to set for the property.
    /// </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    void ICapeThermoMaterialObject.SetProp(String property,
        String phase,
        String[] compIds,
        String calcType,
        String basis,
        double[] values)
    {
        try
        {
            _pMaterialObject.SetProp(property, phase, compIds, calcType, basis, values);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Get some pure component constant(s)
    ///// </summary>
    ///// <remarks>
    ///// This method is responsible for setting the values for properties of the 
    ///// Material Object. See Notes for a more detailed explanation of the arguments.
    ///// </remarks>
    ///// <param name="property">
    ///// The Property for which results are requested from the MaterialObject.
    ///// </param>
    ///// <param name="phase">
    ///// The qualified phase for the results.
    ///// </param>
    ///// <param name="compIds">
    ///// The qualified components for the results. emptyObject to specify all 
    ///// components in the Material Object. For mixture property such as liquid 
    ///// enthalpy, this qualifier is not required. Use emptyObject as place holder.
    ///// </param>
    ///// <param name="calcType">
    ///// The qualified type of calculation for the results. (valid Calculation Types: 
    ///// Pure and Mixture)
    ///// </param>
    ///// <param name="basis">
    ///// Qualifies the basis of the result (i.e., mass /mole). Default is mole. Use 
    ///// NULL for default or as place holder for property for which basis does not 
    ///// apply (see also Specific properties.
    ///// </param>
    ///// <param name="values">
    ///// Values to set for the property.
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    //void ICapeThermoMaterialObjectCOM.SetProp(System.String property,
    //    System.String phase,
    //    object compIds,
    //    System.String calcType,
    //    System.String basis,
    //    object values)
    //{
    //    try
    //    {
    //        p_MaterialObject.SetProp(property, phase, compIds, calcType, basis, values);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Calculate some equilibrium values</summary>
    /// <remarks>This method is responsible for delegating flash calculations to the 
    /// associated Property Package or Equilibrium Server. It must set the amounts, 
    /// compositions, temperature and pressure for all phases present at equilibrium, 
    /// as well as the temperature and pressure for the overall mixture, if not set 
    /// as part of the calculation specifications. See CalcProp and CalcEquilibrium 
    /// for more information.</remarks>
    /// <param name="flashType">
    /// The type of flash to be calculated.
    /// </param>
    /// <param name="props">
    /// Properties to be calculated at equilibrium. emptyObject for no properties. 
    /// If a list, then the property values should be set for each phase present at 
    /// equilibrium. 
    /// </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <exception cref = "ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref = "ECapeOutOfBounds">ECapeOutOfBounds</exception>
    /// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeThermoMaterialObject.CalcEquilibrium(String flashType, String[] props)
    {
        try
        {
            _pMaterialObject.CalcEquilibrium(flashType, props);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }


    ///// <summary>
    ///// Calculate some equilibrium values
    ///// </summary>
    ///// <remarks>
    ///// This method is responsible for delegating flash calculations to the 
    ///// associated Property Package or Equilibrium Server. It must set the amounts, 
    ///// compositions, temperature and pressure for all phases present at equilibrium, 
    ///// as well as the temperature and pressure for the overall mixture, if not set 
    ///// as part of the calculation specifications. See CalcProp and CalcEquilibrium 
    ///// for more information.
    ///// </remarks>
    ///// <param name="flashType">
    ///// The type of flash to be calculated.
    ///// </param>
    ///// <param name="props">
    ///// Properties to be calculated at equilibrium. emptyObject for no properties. 
    ///// If a list, then the property values should be set for each phase present at 
    ///// equilibrium. 
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    ///// <exception cref = "ECapeBadInvOrder">ECapeBadInvOrder</exception>
    ///// <exception cref = "ECapeSolvingError">ECapeSolvingError</exception>
    ///// <exception cref = "ECapeOutOfBounds">ECapeOutOfBounds</exception>
    ///// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    //void ICapeThermoMaterialObjectCOM.CalcEquilibrium(System.String flashType, object props)
    //{
    //    try
    //    {
    //        p_MaterialObject.CalcEquilibrium(flashType, props);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    ///// <summary>
    ///// Set the independent variable for the state
    ///// </summary>
    ///// <remarks>
    ///// Sets the independent variable for a given Material Object.
    ///// </remarks>
    ///// <param name="indVars">
    ///// Independent variables to be set (see names for state variables for list of 
    ///// valid variables). A System.Object containing a String array marshalled from 
    ///// a COM Object.
    ///// </param>
    ///// <param name="values">
    ///// Values of independent variables.
    ///// An array of doubles as a System.Object, which is marshalled as a Object 
    ///// COM-based CAPE-OPEN. 
    ///// </param>
    //void ICapeThermoMaterialObjectCOM.SetIndependentVar(Object indVars, Object values)
    //{
    //    try
    //    {
    //        p_MaterialObject.SetIndependentVar(indVars, values);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}


    /// <summary>Set the independent variable for the state</summary>
    /// <remarks>Sets the independent variable for a given Material Object.</remarks>
    /// <param name="indVars">
    /// Independent variables to be set (see names for state variables for list of 
    /// valid variables). A System.Object containing a String array marshalled from 
    /// a COM Object.
    /// </param>
    /// <param name="values">
    /// Values of independent variables.
    /// An array of doubles as a System.Object, which is marshalled as a Object 
    /// COM-based CAPE-OPEN. 
    /// </param>
    void ICapeThermoMaterialObject.SetIndependentVar(string[] indVars, double[] values)
    {
        try
        {
            _pMaterialObject.SetIndependentVar(indVars, values);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    /// <summary>Get the independent variable for the state</summary>
    /// <remarks>Sets the independent variable for a given Material Object.</remarks>
    /// <param name="indVars">
    /// Independent variables to be set (see names for state variables for list of 
    /// valid variables).
    /// </param>
    /// <returns>
    /// Values of independent variables.
    /// COM-based CAPE-OPEN. 
    /// </returns>
    double[] ICapeThermoMaterialObject.GetIndependentVar(String[] indVars)
    {
        try
        {
            return (double[])_pMaterialObject.GetIndependentVar(indVars);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Get the independent variable for the state
    ///// </summary>
    ///// <remarks>
    ///// Sets the independent variable for a given Material Object.
    ///// </remarks>
    ///// <param name="indVars">
    ///// Independent variables to be set (see names for state variables for list of 
    ///// valid variables).
    ///// </param>
    ///// <returns>
    ///// Values of independent variables.
    ///// COM-based CAPE-OPEN. 
    ///// </returns>
    //object ICapeThermoMaterialObjectCOM.GetIndependentVar(object indVars)
    //{
    //    try
    //    {
    //        return p_MaterialObject.GetIndependentVar(indVars);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}
    /// <summary>Check a property is valid</summary>
    /// <remarks>Checks to see if given properties can be calculated.</remarks>
    /// <returns>
    /// Returns Boolean List associated to list of properties to be checked.
    /// </returns>
    /// <param name="props">
    /// Properties to check. 
    /// </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    bool[] ICapeThermoMaterialObject.PropCheck(String[] props)
    {
        try
        {
            return (bool[])_pMaterialObject.PropCheck(props);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Check a property is valid
    ///// </summary>
    ///// <remarks>
    ///// Checks to see if given properties can be calculated.
    ///// </remarks>
    ///// <returns>
    ///// Returns Boolean List associated to list of properties to be checked.
    ///// </returns>
    ///// <param name="props">
    ///// Properties to check. 
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    //object ICapeThermoMaterialObjectCOM.PropCheck(object props)
    //{
    //    try
    //    {
    //        return p_MaterialObject.PropCheck(props);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}
    /// <summary>Check which properties are available</summary>
    /// <remarks>Gets a list properties that have been calculated.</remarks>
    /// <returns>
    /// Properties for which results are available.
    /// </returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    String[] ICapeThermoMaterialObject.AvailableProps()
    {
        try
        {
            return (String[])_pMaterialObject.AvailableProps();
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Check which properties are available
    ///// </summary>
    ///// <remarks>
    ///// Gets a list properties that have been calculated.
    ///// </remarks>
    ///// <returns>
    ///// Properties for which results are available.
    ///// </returns>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    //object ICapeThermoMaterialObjectCOM.AvailableProps()
    //{
    //    try
    //    {
    //        return p_MaterialObject.AvailableProps();
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}
    ///// <summary>
    ///// Remove any previously calculated results for given properties
    ///// </summary>
    ///// <remarks>
    ///// Remove all or specified property results in the Material Object.
    ///// </remarks>
    ///// <param name="props">
    ///// Properties to be removed. emptyObject to remove all properties.
    ///// </param>
    //void ICapeThermoMaterialObjectCOM.RemoveResults(Object props)
    //{
    //    try
    //    {
    //        p_MaterialObject.RemoveResults(props);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Remove any previously calculated results for given properties</summary>
    /// <remarks>Remove all or specified property results in the Material Object.</remarks>
    /// <param name="props">
    /// Properties to be removed. emptyObject to remove all properties.
    /// </param>
    void ICapeThermoMaterialObject.RemoveResults(string[] props)
    {
        try
        {
            _pMaterialObject.RemoveResults(props);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }


    /// <summary>Create another empty material object</summary>
    /// <remarks>Create a Material Object from the parent Material Template of the current 
    /// Material Object. This is the same as using the CreateMaterialObject method 
    /// on the parent Material Template.</remarks> 
    /// <returns>
    /// The created/initialized Material Object.
    /// </returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    ICapeThermoMaterialObject ICapeThermoMaterialObject.CreateMaterialObject()
    {
        try
        {
            return (ICapeThermoMaterialObject)_pMaterialObject.CreateMaterialObject();
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Create another empty material object
    ///// </summary>
    ///// <remarks>
    ///// Create a Material Object from the parent Material Template of the current 
    ///// Material Object. This is the same as using the CreateMaterialObject method 
    ///// on the parent Material Template.
    ///// </remarks> 
    ///// <returns>
    ///// The created/initialized Material Object.
    ///// </returns>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeOutOfResources">ECapeOutOfResources</exception>
    ///// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    //Object ICapeThermoMaterialObjectCOM.CreateMaterialObject()
    //{
    //    try
    //    {
    //        return (ICapeThermoMaterialObject)p_MaterialObject.CreateMaterialObject();
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Duplicate this material object</summary>
    /// <remarks>Create a duplicate of the current Material Object.</remarks>
    /// <returns>
    /// The created/initialized Material Object.
    /// </returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    [DispId(15)]
    [Description("method Duplicate")]
    [return: MarshalAs(UnmanagedType.IDispatch)]
    ICapeThermoMaterialObject ICapeThermoMaterialObject.Duplicate()
    {
        try
        {
            return new MaterialObjectWrapper(_pMaterialObject.Duplicate());
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }


    ///// <summary>
    ///// Duplicate this material object
    ///// </summary>
    ///// <remarks>
    ///// Create a duplicate of the current Material Object.
    ///// </remarks>
    ///// <returns>
    ///// The created/initialized Material Object.
    ///// </returns>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeOutOfResources">ECapeOutOfResources</exception>
    ///// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    //[System.Runtime.InteropServices.DispIdAttribute(15)]
    //[System.ComponentModel.DescriptionAttribute("method Duplicate")]
    //[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.IDispatch)]
    //object ICapeThermoMaterialObjectCOM.Duplicate()
    //{
    //    try
    //    {
    //        return new MaterialObjectWrapper(p_MaterialObject.Duplicate());
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    ///// <summary>
    ///// Check the validity of the given properties
    ///// </summary>
    ///// <remarks>
    ///// Checks the validity of the calculation.
    ///// </remarks>
    ///// <returns>
    ///// Returns the reliability scale of the calculation.
    ///// </returns>
    ///// <param name="props">
    ///// The properties for which reliability is checked. Null value to remove all 
    ///// properties. 
    ///// </param>
    ///// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    ///// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    //object ICapeThermoMaterialObjectCOM.ValidityCheck(object props)
    //{
    //    try
    //    {
    //        return (bool[])p_MaterialObject.ValidityCheck(props);
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Check the validity of the given properties</summary>
    /// <remarks>Checks the validity of the calculation.</remarks>
    /// <returns>
    /// Returns the reliability scale of the calculation.
    /// </returns>
    /// <param name="props">
    /// The properties for which reliability is checked. Null value to remove all 
    /// properties. 
    /// </param>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    ICapeThermoReliability[] ICapeThermoMaterialObject.ValidityCheck(String[] props)
    {
        try
        {
            return (ICapeThermoReliability[])_pMaterialObject.ValidityCheck(props);
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    /// <summary>Get the list of properties</summary>
    /// <remarks>Returns list of properties supported by the property package and corresponding 
    /// CO Calculation Routines. The properties TEMPERATURE, PRESSURE, FRACTION, FLOW, 
    /// PHASEFRACTION, TOTALFLOW cannot be returned by GetPropList, since all 
    /// components must support them. Although the property identifier of derivative 
    /// properties is formed from the identifier of another property, the GetPropList 
    /// method will return the identifiers of all supported derivative and 
    /// non-derivative properties. For instance, a Property Package could return 
    /// the following list: enthalpy, enthalpy.Dtemperature, entropy, entropy.Dpressure.</remarks>
    /// <returns>
    /// String list of all supported properties of the property package.
    /// </returns>
    string[] ICapeThermoMaterialObject.GetPropList()
    {
        try
        {
            return (String[])_pMaterialObject.GetPropList();
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }

    ///// <summary>
    ///// Get the list of properties
    ///// </summary>
    ///// <remarks>
    ///// Returns list of properties supported by the property package and corresponding 
    ///// CO Calculation Routines. The properties TEMPERATURE, PRESSURE, FRACTION, FLOW, 
    ///// PHASEFRACTION, TOTALFLOW cannot be returned by GetPropList, since all 
    ///// components must support them. Although the property identifier of derivative 
    ///// properties is formed from the identifier of another property, the GetPropList 
    ///// method will return the identifiers of all supported derivative and 
    ///// non-derivative properties. For instance, a Property Package could return 
    ///// the following list: enthalpy, enthalpy.Dtemperature, entropy, entropy.Dpressure.
    ///// </remarks>
    ///// <returns>
    ///// String list of all supported properties of the property package.
    ///// </returns>
    //object ICapeThermoMaterialObjectCOM.GetPropList()
    //{
    //    try
    //    {
    //        return p_MaterialObject.GetPropList();
    //    }
    //    catch (System.Exception p_Ex)
    //    {
    //        throw CapeOpen.COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
    //    }
    //}

    /// <summary>Get the number of components in this material object</summary>
    /// <remarks>Returns number of components in Material Object.</remarks>
    /// <returns>
    /// Number of components in the Material Object.
    /// </returns>
    /// <exception cref="ECapeUnknown">当为该操作指定的其他错误条件不合适时，将引发此错误。</exception>
    int ICapeThermoMaterialObject.GetNumComponents()
    {
        try
        {
            return _pMaterialObject.GetNumComponents();
        }
        catch (Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(_pMaterialObject, p_Ex);
        }
    }
}