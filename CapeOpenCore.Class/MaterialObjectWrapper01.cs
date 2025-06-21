/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.21
 */

using System;

namespace CapeOpenCore.Class;

/// <summary>Status of the phases present in the material object.</summary>
/// <remarks>All the Phases with a status of Cape_AtEquilibrium have values of 
/// temperature, pressure, composition and Phase fraction set that correspond to an 
/// equilibrium state, i.e. equal temperature, pressure and fugacities of each 
/// Compound. Phases with a Cape_Estimates status have values of temperature, pressure, 
/// composition and Phase fraction set in the Material Object. These values are 
/// available for use by an Equilibrium Calculator component to initialise an 
/// Equilibrium Calculation. The stored values are available but there is no guarantee 
/// that they will be used.</remarks>
[Serializable]
[Flags]
public enum CapeFugacityFlag
{
    /// <summary>No Calculation.</summary>
    CAPE_NO_CALCULATION = 0,
    /// <summary>Log Fugacity Coefficients.</summary>
    CAPE_LOG_FUGACITY_COEFFICIENTS = 1,
    /// <summary>Temperature Derivative.</summary>
    CAPE_T_DERIVATIVE = 2,
    /// <summary>Pressure derivative</summary>
    CAPE_P_DERIVATIVE = 4,
    /// <summary>Mole Number Derivative.</summary>
    CAPE_MOLE_NUMBERS_DERIVATIVES = 8
};

/// <summary>Wrapper class for COM-based CAPE-OPEN ICapeThermoMaterialObject material object.</summary>
/// <remarks><para>This class is a wrapper class for COM-based CAPE-OPEN ICapeThermoMaterialObject material object.
/// When you use this wrapper class, the lifecycle of the COM-based material is managed for you and the 
/// COM Release() method is called on the material.</para>
/// <para>In addition, this method converts the COM variants used in the <see cref ="ICapeThermoMaterialObject">
/// ICapeThermoMaterialObject</see> interface to the desired .Net object types. This elimiates the need to convert the data types
/// whne you use a COM-based CAPE-OPEN material object.</para></remarks>
[System.Runtime.InteropServices.ComVisibleAttribute(false)]
[System.Runtime.InteropServices.GuidAttribute("5A65B4B2-2FDD-4208-813D-7CC527FB91BD")]
[System.ComponentModel.DescriptionAttribute("ICapeThermoMaterialObject Interface")]
internal partial class MaterialObjectWrapper : CapeObjectBase, ICapeThermoMaterialObject, ICapeThermoMaterial, ICapeThermoCompounds, 
    ICapeThermoPhases, ICapeThermoUniversalConstant, ICapeThermoEquilibriumRoutine, ICapeThermoPropertyRoutine
{
    [NonSerialized]
    private ICapeThermoMaterialObjectCOM p_MaterialObject;
    [NonSerialized]
    private CapeOpenCore.Class.ICapeThermoMaterialCOM p_IMatObj;
    [NonSerialized]
    private CapeOpenCore.Class.ICapeThermoCompoundsCOM p_ICompounds;
    [NonSerialized]
    private CapeOpenCore.Class.ICapeThermoPhasesCOM p_IPhases;
    [NonSerialized]
    private CapeOpenCore.Class.ICapeThermoUniversalConstantCOM p_IUniversalConstant;
    [NonSerialized]
    private CapeOpenCore.Class.ICapeThermoPropertyRoutineCOM p_IPropertyRoutine;
    [NonSerialized]
    private CapeOpenCore.Class.ICapeThermoEquilibriumRoutineCOM p_IEquilibriumRoutine;
    // Track whether Dispose has been called.
    private bool _disposed;

    private bool Thermo10;
    private bool Thermo11;
    
    /// <summary>Creates a new instance of the MaterialObjectWrapper class</summary>
    /// <param name="materialObject">The material Object to be wrapped.</param>
    public MaterialObjectWrapper(Object materialObject)
    {
        _disposed = false;
        Thermo10 = false;
        Thermo11 = false;
        
        if (materialObject is ICapeThermoMaterialObjectCOM)
        {
            p_MaterialObject = (CapeOpenCore.Class.ICapeThermoMaterialObjectCOM)materialObject;
            Thermo10 = true;
        }
        p_IMatObj = null;
        p_IPropertyRoutine = null;
        p_IUniversalConstant = null;
        p_IPhases = null;
        p_ICompounds = null;
        p_IEquilibriumRoutine = null;
        if (materialObject is ICapeThermoMaterialCOM)
        {
            Thermo11 = true;
            p_IMatObj = (ICapeThermoMaterialCOM)materialObject;
            p_IPropertyRoutine = (ICapeThermoPropertyRoutineCOM)materialObject;
            p_IUniversalConstant = (ICapeThermoUniversalConstantCOM)materialObject;
            p_IPhases = (ICapeThermoPhasesCOM)materialObject;
            p_ICompounds = (ICapeThermoCompoundsCOM)materialObject;
            p_IEquilibriumRoutine = (ICapeThermoEquilibriumRoutineCOM)materialObject;                
        }
    }

    // Use C# destructor syntax for finalization code.
    /// <summary>Finalizer for the <see cref = "MaterialObjectWrapper"/> class.</summary>
    /// <remarks>This will finalize the current instance of the class.</remarks>
    ~MaterialObjectWrapper()
    {
        // Simply call Dispose(false).
        Dispose(false);
    }

    /// <summary>Releases the unmanaged resources used by the CapeIdentification object and optionally releases 
    /// the managed resources.</summary>
    /// <remarks><para>This method is called by the public <see href="http://msdn.microsoft.com/en-us/library/system.componentmodel.component.dispose.aspx">Dispose</see>see> 
    /// method and the <see href="http://msdn.microsoft.com/en-us/library/system.object.finalize.aspx">Finalize</see> method. 
    /// <bold>Dispose()</bold> invokes the protected <bold>Dispose(Boolean)</bold> method with the disposing
    /// parameter set to <bold>true</bold>. <see href="http://msdn.microsoft.com/en-us/library/system.object.finalize.aspx">Finalize</see> 
    /// invokes <bold>Dispose</bold> with disposing set to <bold>false</bold>.</para>
    /// <para>When the <italic>disposing</italic> parameter is <bold>true</bold>, this method releases all 
    /// resources held by any managed objects that this Component references. This method invokes the 
    /// <bold>Dispose()</bold> method of each referenced object.</para>
    /// <para><bold>Notes to Inheritors</bold></para>
    /// <para><bold>Dispose</bold> can be called multiple times by other objects. When overriding 
    /// <bold>Dispose(Boolean)</bold>, be careful not to reference objects that have been previously 
    /// disposed of in an earlier call to <bold>Dispose</bold>. For more information about how to 
    /// implement <bold>Dispose(Boolean)</bold>, see <see href="http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx">Implementing a Dispose Method</see>.</para>
    /// <para>For more information about <bold>Dispose</bold> and <see href="http://msdn.microsoft.com/en-us/library/system.object.finalize.aspx">Finalize</see>, 
    /// see <see href="http://msdn.microsoft.com/en-us/library/498928w2.aspx">Cleaning Up Unmanaged Resources</see> 
    /// and <see href="http://msdn.microsoft.com/en-us/library/ddae83kx.aspx">Overriding the Finalize Method</see>.</para></remarks> 
    /// <param name = "disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        // If you need thread safety, use a lock around these 
        // operations, as well as in your methods that use the resource.
        if (!_disposed)
        {
            if (disposing)
            {
            }
            // Indicate that the instance has been disposed.
            if (p_MaterialObject != null)
                if (p_MaterialObject.GetType().IsCOMObject)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(p_MaterialObject);
                }
            p_MaterialObject = null;
            if (p_IMatObj != null)
            {
                if (p_IMatObj.GetType().IsCOMObject)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(p_IMatObj);
                }
            }
            p_IMatObj = null;
            if (p_ICompounds != null)
            {
                if (p_ICompounds.GetType().IsCOMObject)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(p_ICompounds);
                }
            }
            p_ICompounds = null;
            if (p_IPhases != null)
            {
                if (p_IPhases.GetType().IsCOMObject)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(p_IPhases);
                }
            }
            p_IPhases = null;
            if (p_IUniversalConstant != null)
            {
                if (p_IUniversalConstant.GetType().IsCOMObject)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(p_IUniversalConstant);
                }
            }
            p_IUniversalConstant = null;
            if (p_IPropertyRoutine != null)
            {
                if (p_IPropertyRoutine.GetType().IsCOMObject)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(p_IPropertyRoutine);
                }
            }
            p_IPropertyRoutine = null;
            _disposed = true;
        }
    }

    /// <summary>Gets and sets the name of the component.</summary>
    /// <remarks><para>A particular Use Case in a system may contain several CAPE-OPEN components 
    /// of the same class. The user should be able to assign different names and 
    /// descriptions to each instance in order to refer to them unambiguously and in a 
    /// user-friendly way. Since not always the software components that are able to 
    /// set these identifications and the software components that require this information 
    /// have been developed by the same vendor, a CAPE-OPEN standard for setting and 
    /// getting this information is required.</para>
    /// <para>So, the component will not usually set its own name and description: the 
    /// user of the component will do it.</para></remarks>
    /// <value>The unique name of the component.</value>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    [System.ComponentModel.DescriptionAttribute("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [System.ComponentModel.CategoryAttribute("CapeIdentification")]
    override public String ComponentName
    {
        get
        {
            return ((ICapeIdentification)p_MaterialObject).ComponentName;
        }

        set
        {
            ((ICapeIdentification)p_MaterialObject).ComponentName = value;
        }
    }

    /// <summary> Gets and sets the description of the component.</summary>
    /// <remarks><para>A particular Use Case in a system may contain several CAPE-OPEN components 
    /// of the same class. The user should be able to assign different names and 
    /// descriptions to each instance in order to refer to them unambiguously and in a 
    /// user-friendly way. Since not always the software components that are able to 
    /// set these identifications and the software components that require this information 
    /// have been developed by the same vendor, a CAPE-OPEN standard for setting and 
    /// getting this information is required.</para>
    /// <para>So, the component will not usually set its own name and description: the 
    /// user of the component will do it.</para></remarks>
    /// <value>The description of the component.</value>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    [System.ComponentModel.DescriptionAttribute("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [System.ComponentModel.CategoryAttribute("CapeIdentification")]
    override public String ComponentDescription
    {
        get
        {
            return ((ICapeIdentification)p_MaterialObject).ComponentDescription;
        }
        set
        {
            ((ICapeIdentification)p_MaterialObject).ComponentDescription = value;
        }
    }

    /// <summary>Provides information regarding whether the object supports Thermodynamics version 1.0.</summary>
    /// <remarks>The <see cref = "MaterialObjectWrapper"/> class checks to determine whether the wrapped material object
    /// supports CAPE-OPEN version 1.0 thrmoedynamics. This proprety indicates the result of that check.</remarks>
    /// <value>Indicates whetehr the wrapped material object supports CAPE-OPEN Thermodynamics varsion 1.0 interfaces.</value>
    public bool SupportsThermo10
    {
        get
        {
            return Thermo10;
        }
    }

    /// <summary>Provides information regarding whether the object supports Thermodynamics version 1.1.</summary>
    /// <remarks>The <see cref = "MaterialObjectWrapper1"/> class checks to determine whether the wrapped material object
    /// supports CAPE-OPEN version 1.1 thrmoedynamics. This proprety indicates the result of that check.</remarks>
    /// <value>Indicates whetehr the wrapped material object supports CAPE-OPEN Thermodynamics varsion 1.1 interfaces.</value>
    public bool SupportsThermo11
    {
        get
        {
            return Thermo11;
        }
    }
        
    /// <summary> Gets the wrapped Thermo Version 1.0 Material Object.</summary>
    /// <remarks><para>Provides direct access to the Thermo Version 1.0 material object.</para>
    /// <para>The material object exposes the COm version of the ICapeThermoMaterialObject interface.</para></remarks>
    /// <value>The wrapped Thermo Version 1.0 Material Object.</value>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    [System.ComponentModel.DescriptionAttribute("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [System.ComponentModel.CategoryAttribute("CapeIdentification")]
    public object MaterialObject10
    {
        get
        {
            return p_MaterialObject;
        }
    }


    /// <summary> Gets the wrapped Thermo Version 1.1 Material Object.</summary>
    /// <remarks><para>Provides direct access to the Thermo Version 1.1 material object.</para>
    /// <para>The material object exposes the COM version of the Thermo 1.1 interfaces.</para></remarks>
    /// <value>The wrapped Thermo Version 1.1 Material Object.</value>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    [System.ComponentModel.DescriptionAttribute("Unit Operation Parameter Collection. Click on the (...) button to edit collection.")]
    [System.ComponentModel.CategoryAttribute("CapeIdentification")]
    public object MaterialObject11
    {
        get
        {
            return p_IMatObj;
        }
    }
        
    /// <summary>Get the component ids for this MO</summary>
    /// <remarks>Returns the list of components Ids of a given Material Object.</remarks>
    /// <value>Te names of the compounds in the matieral object.</value>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    String[] ICapeThermoMaterialObject.ComponentIds
    {
        get
        {
            try
            {
                return (String[])p_MaterialObject.ComponentIds;
            }
            catch (System.Exception p_Ex)
            {
                throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
            }
        }
    }

    /// <summary>Get the phase ids for this MO</summary>
    /// <remarks>It returns the phases existing in the MO at that moment. The Overall phase 
    /// and multiphase identifiers cannot be returned by this method. See notes on 
    /// Existence of a phase for more information.</remarks>
    /// <value>The phases present in the material.</value>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    String[] ICapeThermoMaterialObject.PhaseIds
    {
        get
        {
            try
            {
                return (String[])p_MaterialObject.PhaseIds;
            }
            catch (System.Exception p_Ex)
            {
                throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
            }
        }
    }

    /// <summary>Get some universal constant(s)</summary>
    /// <remarks>Retrieves universal constants from the Property Package.</remarks>
    /// <returns>
    /// Values of the requested universal constants.</returns>
    /// <param name = "props">
    /// List of universal constants to be retrieved.</param>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeNoImpl">ECapeNoImpl</exception>
    double[] ICapeThermoMaterialObject.GetUniversalConstant(String[] props)
    {
        try
        {
            return (double[])p_MaterialObject.GetUniversalConstant(props);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

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
    /// so on.</returns>
    /// <param name = "props">
    /// List of component constants.</param>
    /// <param name = "compIds">
    /// List of component IDs for which constants are to be retrieved. Use a null value 
    /// for all components in the Material Object. </param>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeNoImpl">ECapeNoImpl</exception>
    object[] ICapeThermoMaterialObject.GetComponentConstant(String[] props, String[] compIds)
    {
        try
        {
            return (object[])p_MaterialObject.GetComponentConstant(props, compIds);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Calculate some properties</summary>
    /// <remarks>This method is responsible for doing all property calculations and delegating 
    /// these calculations to the associated thermo system. This method is further 
    /// defined in the descriptions of the CAPE-OPEN Calling Pattern and the User 
    /// Guide Section. See Notes for a more detailed explanation of the arguments and 
    /// CalcProp description in the notes for a general discussion of the method.</remarks>
    /// <param name = "props">The List of Properties to be calculated.</param>
    /// <param name = "phases">List of phases for which the properties are to be calculated.</param>
    /// <param name = "calcType">Type of calculation: Mixture Property or Pure Component Property. For partial 
    /// property, such as fugacity coefficients of components in a mixture, use 
    /// “Mixture” CalcType. For pure component fugacity coefficients, use “Pure” 
    /// CalcType.</param>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref = "ECapeOutOfBounds">ECapeOutOfBounds</exception>
    /// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeThermoMaterialObject.CalcProp(String[] props, String[] phases, String calcType)
    {
        try
        {
            p_MaterialObject.CalcProp(props, phases, calcType);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Get some pure component constant(s)</summary>
    /// <remarks>This method is responsible for retrieving the results from calculations from 
    /// the MaterialObject. See Notesfor a more detailed explanation of the arguments.</remarks>
    /// <returns>Results vector containing property values in SI units arranged by the defined 
    /// qualifiers. The array is one dimensional containing the properties, in order 
    /// of the "props" array for each of the compounds, in order of the compIds array.</returns>
    /// <param name = "property">The Property for which results are requested from the MaterialObject.</param>
    /// <param name = "phase">The qualified phase for the results.</param>
    /// <param name = "compIds">The qualified components for the results. Use a null value to specify all 
    /// components in the Material Object. For mixture property such as liquid 
    /// enthalpy, this qualifier is not required. Use emptyObject as place holder.</param>
    /// <param name = "calcType">The qualified type of calculation for the results. (valid Calculation Types: 
    /// Pure and Mixture)</param>
    /// <param name = "basis">Qualifies the basis of the result (i.e., mass /mole). Default is mole. Use 
    /// NULL for default or as place holder for property for which basis does not 
    /// apply (see also Specific properties.</param>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    double[] ICapeThermoMaterialObject.GetProp(System.String property,
        System.String phase,
        String[] compIds,
        System.String calcType,
        System.String basis)
    {
        try
        {
            return (double[])p_MaterialObject.GetProp(property, phase, compIds, calcType, basis);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Get some pure component constant(s)</summary>
    /// <remarks>This method is responsible for setting the values for properties of the 
    /// Material Object. See Notes for a more detailed explanation of the arguments.</remarks>
    /// <param name = "property">The Property for which results are requested from the MaterialObject.</param>
    /// <param name = "phase">The qualified phase for the results.</param>
    /// <param name = "compIds">The qualified components for the results. emptyObject to specify all 
    /// components in the Material Object. For mixture property such as liquid 
    /// enthalpy, this qualifier is not required. Use emptyObject as place holder.</param>
    /// <param name = "calcType">The qualified type of calculation for the results. (valid Calculation Types: 
    /// Pure and Mixture)</param>
    /// <param name = "basis">Qualifies the basis of the result (i.e., mass /mole). Default is mole. Use 
    /// NULL for default or as place holder for property for which basis does not 
    /// apply (see also Specific properties.</param>
    /// <param name = "values">Values to set for the property.</param>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    void ICapeThermoMaterialObject.SetProp(System.String property,
        System.String phase,
        String[] compIds,
        System.String calcType,
        System.String basis,
        double[] values)
    {
        try
        {
            p_MaterialObject.SetProp(property, phase, compIds, calcType, basis, values);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }
    
    /// <summary>Calculate some equilibrium values</summary>
    /// <remarks>This method is responsible for delegating flash calculations to the 
    /// associated Property Package or Equilibrium Server. It must set the amounts, 
    /// compositions, temperature and pressure for all phases present at equilibrium, 
    /// as well as the temperature and pressure for the overall mixture, if not set 
    /// as part of the calculation specifications. See CalcProp and CalcEquilibrium 
    /// for more information.</remarks>
    /// <param name = "flashType">The type of flash to be calculated.</param>
    /// <param name = "props">Properties to be calculated at equilibrium. emptyObject for no properties. 
    /// If a list, then the property values should be set for each phase present at 
    /// equilibrium.</param>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    /// <exception cref = "ECapeBadInvOrder">ECapeBadInvOrder</exception>
    /// <exception cref = "ECapeSolvingError">ECapeSolvingError</exception>
    /// <exception cref = "ECapeOutOfBounds">ECapeOutOfBounds</exception>
    /// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    void ICapeThermoMaterialObject.CalcEquilibrium(System.String flashType, String[] props)
    {
        try
        {
            p_MaterialObject.CalcEquilibrium(flashType, props);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Set the independent variable for the state</summary>
    /// <remarks>Sets the independent variable for a given Material Object.</remarks>
    /// <param name = "indVars">
    /// Independent variables to be set (see names for state variables for list of 
    /// valid variables). A System.Object containing a String array marshalled from 
    /// a COM Object.</param>
    /// <param name = "values">
    /// Values of independent variables.
    /// An array of doubles as a System.Object, which is marshalled as a Object 
    /// COM-based CAPE-OPEN. </param>
    void ICapeThermoMaterialObject.SetIndependentVar(string[] indVars, double[] values)
    {
        try
        {
            p_MaterialObject.SetIndependentVar(indVars, values);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Get the independent variable for the state</summary>
    /// <remarks>Sets the independent variable for a given Material Object.</remarks>
    /// <param name = "indVars">
    /// Independent variables to be set (see names for state variables for list of 
    /// valid variables).</param>
    /// <returns>
    /// Values of independent variables.
    /// COM-based CAPE-OPEN. </returns>
    double[] ICapeThermoMaterialObject.GetIndependentVar(String[] indVars)
    {
        try
        {
            return (double[])p_MaterialObject.GetIndependentVar(indVars);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }
    
    /// <summary>Check a property is valid</summary>
    /// <remarks>Checks to see if given properties can be calculated.</remarks>
    /// <returns>Returns Boolean List associated to list of properties to be checked.</returns>
    /// <param name = "props">Properties to check.</param>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    bool[] ICapeThermoMaterialObject.PropCheck(String[] props)
    {
        try
        {
            return (bool[])p_MaterialObject.PropCheck(props);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Check which properties are available</summary>
    /// <remarks>Gets a list properties that have been calculated.</remarks>
    /// <returns>
    /// Properties for which results are available.</returns>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    String[] ICapeThermoMaterialObject.AvailableProps()
    {
        try
        {
            return (String[])p_MaterialObject.AvailableProps();
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Remove any previously calculated results for given properties</summary>
    /// <remarks>Remove all or specified property results in the Material Object.</remarks>
    /// <param name = "props">
    /// Properties to be removed. emptyObject to remove all properties.</param>
    void ICapeThermoMaterialObject.RemoveResults(string[] props)
    {
        try
        {
            p_MaterialObject.RemoveResults(props);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }
    
    /// <summary>Create another empty material object</summary>
    /// <remarks>Create a Material Object from the parent Material Template of the current 
    /// Material Object. This is the same as using the CreateMaterialObject method 
    /// on the parent Material Template.</remarks> 
    /// <returns>
    /// The created/initialized Material Object.</returns>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    ICapeThermoMaterialObject ICapeThermoMaterialObject.CreateMaterialObject()
    {
        try
        {
            return (ICapeThermoMaterialObject)p_MaterialObject.CreateMaterialObject();
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Duplicate this material object</summary>
    /// <remarks>Create a duplicate of the current Material Object.</remarks>
    /// <returns>
    /// The created/initialized Material Object.</returns>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeOutOfResources">ECapeOutOfResources</exception>
    /// <exception cref = "ECapeLicenceError">ECapeLicenceError</exception>
    [System.Runtime.InteropServices.DispIdAttribute(15)]
    [System.ComponentModel.DescriptionAttribute("method Duplicate")]
    [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.IDispatch)]
    ICapeThermoMaterialObject ICapeThermoMaterialObject.Duplicate()
    {
        try
        {
            return new MaterialObjectWrapper(p_MaterialObject.Duplicate());
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Check the validity of the given properties</summary>
    /// <remarks>Checks the validity of the calculation.</remarks>
    /// <returns>
    /// Returns the reliability scale of the calculation.</returns>
    /// <param name = "props">
    /// The properties for which reliability is checked. Null value to remove all 
    /// properties. </param>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument value is passed, for example, an unrecognised Compound identifier or UNDEFINED for the props argument.</exception>
    ICapeThermoReliability[] ICapeThermoMaterialObject.ValidityCheck(String[] props)
    {
        try
        {
            return (ICapeThermoReliability[])p_MaterialObject.ValidityCheck(props);
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
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
    /// String list of all supported properties of the property package.</returns>
    string[] ICapeThermoMaterialObject.GetPropList()
    {
        try
        {
            return (String[])p_MaterialObject.GetPropList();
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }

    /// <summary>Get the number of components in this material object</summary>
    /// <remarks>Returns number of components in Material Object.</remarks>
    /// <returns>
    /// Number of components in the Material Object.</returns>
    /// <exception cref ="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    int ICapeThermoMaterialObject.GetNumComponents()
    {
        try
        {
            return p_MaterialObject.GetNumComponents();
        }
        catch (System.Exception p_Ex)
        {
            throw COMExceptionHandler.ExceptionForHRESULT(p_MaterialObject, p_Ex);
        }
    }
    
    // ICapeThermoMaterial implementation
    /// <summary>Remove all stored Physical Property values.</summary>
    /// <remarks><para>ClearAllProps removes all stored Physical Properties that have been set 
    /// using the SetSinglePhaseProp, SetTwoPhaseProp or SetOverallProp methods. 
    /// This means that any subsequent call to retrieve Physical Properties will 
    /// result in an exception until new values have been stored using one of the 
    /// Set methods. ClearAllProps does not remove the configuration information 
    /// for a Material, i.e. the list of Compounds and Phases.</para>
    /// <para>Using the ClearAllProps method results in a Material Object that is in 
    /// the same state as when it was first created. It is an alternative to using 
    /// the CreateMaterial method but it is expected to have a smaller overhead in 
    /// operating system resources.</para></remarks>
    /// <exception cref = "ECapeNoImpl">The operation is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.ClearAllProps()
    {
        p_IMatObj.ClearAllProps();
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
    /// Object N is equivalent to the deprecated method ICapeMaterialObject.Duplicate.</para>
    /// <para>The method is intended to be used by a client, for example a Unit 
    /// Operation that needs a Material Object to have the same state as one of the 
    /// Material Objects it has been connected to. One example is the representation 
    /// of an internal stream in a distillation column.</para></remarks>
    /// <param name = "source">Source Material Object from which stored properties will be copied.</param>
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
        p_IMatObj.CopyFromMaterial(((MaterialObjectWrapper)source).MaterialObject11);
    }

    /// <summary>Creates a Material Object with the same configuration as the current 
    /// Material Object.</summary>
    /// <remarks>The Material Object created does not contain any non-constant Physical 
    /// Property value but has the same configuration (Compounds and Phases) as 
    /// the current Material Object. These Physical Property values must be set 
    /// using SetSinglePhaseProp, SetTwoPhaseProp or SetOverallProp. Any attempt to 
    /// retrieve Physical Property values before they have been set will result in 
    /// an exception.</remarks>
    /// <returns>The interface for the Material Object.</returns>
    /// <exception cref = "ECapeNoImpl">The operation is “not” implemented even if 
    /// this method can be called for reasons of compatibility with the CAPE-OPEN 
    /// standards. That is to say that the operation exists but it is not supported 
    /// by the current implementation.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The physical resources 
    /// necessary to the creation of the Material Object are out of limits.</exception>
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
        return (ICapeThermoMaterial)new MaterialObjectWrapper(p_IMatObj.CreateMaterial());
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
    /// <param name = "results"> A double array containing the results vector of 
    /// Physical Property value(s) in SI units.</param>
    /// <param name = "property">A String identifier of the Physical Property for 
    /// which values are requested. This must be one of the single-phase Physical 
    /// Properties or derivatives that can be stored for the overall mixture. The 
    /// standard identifiers are listed in sections 7.5.5 and 7.6.</param>
    /// <param name = "basis">A String indicating the basis of the results. Valid 
    /// settings are: “Mass” for Physical Properties per unit mass or “Mole” for 
    /// molar properties. Use UNDEFINED as a place holder for a Physical Property 
    /// for which basis does not apply. See section 7.5.5 for details.</param>
    /// <exception cref = "ECapeNoImpl">The operation GetOverallProp is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">The Physical Property 
    /// required is not available from the Material Object, possibly for the basis 
    /// requested. This exception is raised when a Physical Property value has not 
    /// been set following a call to the CreateMaterial or ClearAllProps methods.</exception>
    /// <exception cref = "ECapeInvalidArgument">To be used when an invalid argument 
    /// value was passed, for example UNDEFINED for property.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed or has failed.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetOverallProp(String property, String basis, ref double[] results)
    {
        Object obj1 = null;
        p_IMatObj.GetOverallProp(property, basis, ref obj1);
        results = (double[])obj1;
    }

    /// <summary>Retrieves temperature, pressure and composition for the overall mixture.</summary>
    /// <remarks><para>
    ///This method is provided to make it easier for developers to make efficient 
    /// use of the CAPEOPEN interfaces. It returns the most frequently requested 
    /// information from a Material Object in a single call.</para>
    /// <para>There is no choice of basis in this method. The composition is always 
    /// returned as mole fractions.</para></remarks>
    /// <param name = "temperature">A reference to a double Temperature (in K)</param>
    /// <param name = "pressure">A reference to a double Pressure (in Pa)</param>
    /// <param name = "composition">A reference to an array of doubles containing 
    /// the Composition (mole fractions)</param>
    /// <exception cref = "ECapeNoImpl">The operation GetOverallProp is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation.</exception>
    /// <exception cref = "ECapeThrmPropertyNotAvailable">The Physical Property 
    /// required is not available from the Material Object, possibly for the basis 
    /// requested. This exception is raised when a Physical Property value has not 
    /// been set following a call to the CreateMaterial or ClearAllProps methods.</exception>
    /// <exception cref = "ECapeFailedInitialisation">The pre-requisites are not 
    /// valid. The necessary initialisation has not been performed or has failed.</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetOverallTPFraction(ref double temperature, ref double pressure, ref double[] composition)
    {
        Object obj1 = null;
        p_IMatObj.GetOverallTPFraction(temperature, pressure, ref obj1);
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
    /// <param name = "phaseLabels">A reference to a String array that contains the 
    /// list of Phase labels (identifiers – names) for the Phases present in the 
    /// Material Object. The Phase labels in the Material Object must be a
    /// subset of the labels returned by the GetPhaseList method of the 
    /// ICapeThermoPhases interface.</param>
    /// <param name = "phaseStatus">A CapeArrayEnumeration which is an array of 
    /// Phase status flags corresponding to each of the Phase labels. 
    /// See description below.</param>
    /// <exception cref = "ECapeNoImpl">The operation is “not” 
    /// implemented even if this method can be called for reasons of compatibility 
    /// with the CAPE-OPEN standards. That is to say that the operation exists but 
    /// it is not supported by the current implementation</exception>
    /// <exception cref = "ECapeUnknown">The error to be raised when 
    /// other error(s), specified for this operation, are not suitable.</exception>
    void ICapeThermoMaterial.GetPresentPhases(ref String[] phaseLabels, ref CapeOpenCore.Class.CapePhaseStatus[] phaseStatus)
    {
        Object obj1 = null;
        Object obj2 = null;
        p_IMatObj.GetPresentPhases(ref obj1, ref obj2);
        phaseLabels = (String[])obj1;
        phaseStatus = new CapeOpenCore.Class.CapePhaseStatus[phaseLabels.Length];
        int[] values = (int[])obj2;
        for (int i = 0; i < phaseStatus.Length; i++)
        {
            if (values[i] == 0) phaseStatus[i] = CapeOpenCore.Class.CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS;
            if (values[i] == 1) phaseStatus[i] = CapeOpenCore.Class.CapePhaseStatus.CAPE_ATEQUILIBRIUM;
            if (values[i] == 2) phaseStatus[i] = CapeOpenCore.Class.CapePhaseStatus.CAPE_ESTIMATES;
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
    /// <param name = "property">CapeString The identifier of the Physical Property 
    /// for which values are requested. This must be one of the single-phase Physical 
    /// Properties or derivatives. The standard identifiers are listed in sections 
    /// 7.5.5 and 7.6.</param>
    /// <param name = "phaseLabel">CapeString Phase label of the Phase for which 
    /// the Physical Property is required. The Phase label must be one of the 
    ///identifiers returned by the GetPresentPhases method of this interface.</param>
    /// <param name = "basis">CapeString Basis of the results. Valid settings are: 
    /// “Mass” for Physical Properties per unit mass or “Mole” for molar properties. 
    /// Use UNDEFINED as a place holder for a Physical Property for which basis does 
    /// not apply. See section 7.5.5 for details.</param>
    /// <param name = "results">CapeVariant Results vector (CapeArrayDouble) 
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
        p_IMatObj.GetSinglePhaseProp(property, phaseLabel, basis, ref obj1);
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
    /// <param name = "phaseLabel">Phase label of the Phase for which the property 
    /// is required. The Phase label must be one of the identifiers returned by the 
    /// GetPresentPhases method of this interface.</param>
    /// <param name = "temperature">Temperature (in K)</param>
    /// <param name = "pressure">Pressure (in Pa)</param>
    /// <param name = "composition">Composition (mole fractions)</param>
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
    void ICapeThermoMaterial.GetTPFraction(String phaseLabel, ref double temperature, ref double pressure, ref double[] composition)
    {
        Object obj1 = null;
        p_IMatObj.GetTPFraction(phaseLabel, temperature, pressure, obj1);
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
    /// <param name = "property">The identifier of the property for which values are
    /// requested. This must be one of the two-phase Physical Properties or Physical 
    /// Property derivatives listed in sections 7.5.6 and 7.6.</param>
    /// <param name = "phaseLabels">List of Phase labels of the Phases for which the
    /// property is required. The Phase labels must be two of the identifiers 
    /// returned by the GetPhaseList method of the Material Object.</param>
    /// <param name = "basis">Basis of the results. Valid settings are: “Mass” for
    /// Physical Properties per unit mass or “Mole” for molar properties. Use 
    /// UNDEFINED as a place holder for a Physical Property for which basis does not 
    /// apply. See section 7.5.5 for details.</param>
    /// <param name = "results">Results vector (CapeArrayDouble) containing property
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
        p_IMatObj.GetTwoPhaseProp(property, phaseLabels, basis, ref obj1);
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
    /// <param name = "basis">Basis of the results. Valid settings are: “Mass” for
    /// Physical Properties per unit mass or “Mole” for molar properties. Use 
    /// UNDEFINED as a place holder for a Physical Property for which basis does not 
    /// apply. See section 7.5.5 for details.</param>
    /// <param name = "values">Values to set for the property.</param>
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
        p_IMatObj.SetOverallProp(property, basis, values);
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
    /// <param name = "phaseLabels"> CapeArrayString The list of Phase labels for 
    /// the Phases present. The Phase labels in the Material Object must be a
    /// subset of the labels returned by the GetPhaseList method of the 
    /// ICapeThermoPhases interface.</param>
    /// <param name = "phaseStatus">Array of Phase status flags corresponding to 
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
    void ICapeThermoMaterial.SetPresentPhases(String[] phaseLabels, CapeOpenCore.Class.CapePhaseStatus[] phaseStatus)
    {
        int[] obj1 = new int[phaseStatus.Length];
        for (int i = 0; i < obj1.Length; i++)
        {
            if (phaseStatus[i] == CapeOpenCore.Class.CapePhaseStatus.CAPE_UNKNOWNPHASESTATUS) obj1[i] = 0;
            if (phaseStatus[i] == CapeOpenCore.Class.CapePhaseStatus.CAPE_ATEQUILIBRIUM) obj1[i] = 1;
            if (phaseStatus[i] == CapeOpenCore.Class.CapePhaseStatus.CAPE_ESTIMATES) obj1[i] = 2;
        }
        p_IMatObj.SetPresentPhases(phaseLabels, obj1);
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
    /// <param name = "prop">The identifier of the property for which values are 
    /// set. This must be one of the single-phase properties or derivatives. The 
    /// standard identifiers are listed in sections 7.5.5 and 7.6.</param>
    /// <param name = "phaseLabel">Phase label of the Phase for which the property is 
    /// set. The phase label must be one of the strings returned by the 
    /// GetPresentPhases method of this interface.</param>
    /// <param name = "basis">Basis of the results. Valid settings are: “Mass” for
    /// Physical Properties per unit mass or “Mole” for molar properties. Use 
    /// UNDEFINED as a place holder for a Physical Property for which basis does not 
    /// apply. See section 7.5.5 for details.</param>
    /// <param name = "values">Values to set for the property (CapeArrayDouble) or
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
        p_IMatObj.SetSinglePhaseProp(prop, phaseLabel, basis, values);
    }
}
