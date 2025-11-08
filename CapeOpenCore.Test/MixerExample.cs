using System;
using System.Linq;
using System.Runtime.InteropServices;
using CapeOpenCore.Class;

namespace CapeOpenCore.Test;

/// <summary>这是一个模拟绝热混合器的混合器示例类</summary>
/// <remarks>
/// <para>混合器进行物流和能量平衡，以确定输入流股混合后的输出流股</para>
/// <para>该示例中可选有四个参数。计算中只使用了一个，即实际压降。
/// 出口流股的压力被设置为入口流股中较低压力流股的压力减去压降参数的值。
/// 其余参数作为整数、布尔值和选项参数的演示提供。</para>
/// </remarks>
[Serializable]
[ComVisible(true)]
[Guid("883D46FE-5713-424C-BF10-7ED34263CD6D")] // ICapeThermoMaterialObject_IID
[System.ComponentModel.Description("")]
[CapeName("MixerExample")]
[CapeDescription("An example mixer unit operation written in C#.")]
[CapeVersion("1.0")]
[CapeVendorURL("http:\\www.epa.gov")]
[CapeHelpURL("http:\\www.epa.gov")]
[CapeAbout("US Environmental Protection Agency\nCincinnati, Ohio")]
[CapeConsumesThermo(true)]
[CapeUnitOperation(true)]
[CapeSupportsThermodynamics10(true)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class MixerExample : CapeUnitBase
{
    /// <summary>创建 CMixerExample 单元操作的实例</summary>
    /// <remarks>此构造函数演示了向参数集合中添加 <see cref="BooleanParameter"/>、
    /// <see cref="IntegerParameter"/> 和 <see credf=“OptionParameter”/>参数。
    /// 此外，混合器模块有三个 <see cref="UnitPort"/> 端口添加到端口集合中。
    /// 有关其实现的详细信息，请参阅 <See cref="MixerExample.Calculate"/>方法的文档。</remarks>
    /// <example>
    /// 创建单元操作的示例。参数和端口对象被创建并添加到各自的集合中。
    /// 端口由 <see cref="UnitPort"/>类实现，并放置在端口集合中。
    /// 参数则添加到参数集合类中。可用的参数类包括：<see cref="RealParameter"/>、
    /// <see cref="IntegerParameter"/>、<see cref="BooleanParameter"/>
    /// 以及 <see cref="OptionParameter"/>。
    /// <code>
    /// public MixerExample()
    /// {
    ///     // Add Ports using the UnitPort constructor.
    ///     this.Ports.Add(new UnitPort("Inlet Port1", "Test Inlet Port1", CapePortDirection.CAPE_INLET, CapePortType.CAPE_MATERIAL));
    ///     this.Ports.Add(new UnitPort("Inlet Port2", "Test Inlet Port2", CapePortDirection.CAPE_INLET, CapePortType.CAPE_MATERIAL));
    ///     this.Ports.Add(new UnitPort("Outlet Port", "Test Outlet Port", CapePortDirection.CAPE_OUTLET, CapePortType.CAPE_MATERIAL));
    /// 
    ///     // Add a real valued parameter using the RealParameter  constructor.
    ///     RealParameter real = new RealParameter("PressureDrop", "Drop in pressure between the outlet from the mixer and the pressure of the lower pressure inlet.", 0.0, 0.0, 0.0, 100000000.0, CapeParamMode.CAPE_INPUT, "Pa");
    ///     this.Parameters.Add(real);
    /// 
    ///     // Add a real valued parameter using the IntegerParameter  constructor.
    ///     this.Parameters.Add(new IntegerParameter("Integer Parameter", "This is an example of an integer parameter.", 12, 12, 0, 100, CapeParamMode.CAPE_INPUT_OUTPUT));
    /// 
    ///     // Add a real valued parameter using the BooleanParameter  constructor.
    ///     this.Parameters.Add(new BooleanParameter("Boolean Parameter", "This is an example of a boolean parameter.", false, false, CapeOpen.CapeParamMode.CAPE_INPUT_OUTPUT));
    /// 
    ///     // Create an array of strings for the option parameter restricted value list.
    ///     String[] options = { "Test Value", "Another Value" };
    /// 
    ///     // Add a string valued parameter using the OptionParameter constructor.
    ///     this.Parameters.Add(new OptionParameter("OptionParameter", "This is an example of an option parameter.", "Test Value", "Test Value", options, true, CapeParamMode.CAPE_INPUT_OUTPUT));
    /// 
    ///     // Add an available report.
    ///     this.Reports.Add("Report 2");
    /// }
    /// </code>
    /// </example>
    public MixerExample()
    {
        // 使用 UnitPort 构造函数添加端口
        Ports.Add(new UnitPort("Inlet Port1", "Test Inlet Port1", CapePortDirection.CAPE_INLET,
            CapePortType.CAPE_MATERIAL));
        Ports.Add(new UnitPort("Inlet Port2", "Test Inlet Port2", CapePortDirection.CAPE_INLET,
            CapePortType.CAPE_MATERIAL));
        Ports.Add(new UnitPort("Outlet Port", "Test Outlet Port", CapePortDirection.CAPE_OUTLET,
            CapePortType.CAPE_MATERIAL));

        // 使用 RealParameter 构造函数添加一个实数参数：压降
        // 下限为 0，上限为 100000000，默认为 0
        RealParameter real = new RealParameter("PressureDrop",
            "Drop in pressure between the outlet from the mixer and the pressure of the lower pressure inlet.", 0.0,
            0.0, 0.0, 100000000.0, CapeParamMode.CAPE_INPUT, "Pa");
        Parameters.Add(real);

        // 使用 IntegerParameter 构造函数添加一个示例实数参数：IntegerParameter
        // 下限为 0，上限为 100，默认为 12
        Parameters.Add(new IntegerParameter("IntegerParameter", "This is an example of an integer parameter.", 12, 12,
            0, 100, CapeParamMode.CAPE_INPUT_OUTPUT));

        // 使用 BooleanParameter 构造函数添加一个示例实数参数：BooleanParameter
        // 默认为 false
        Parameters.Add(new BooleanParameter("BooleanParameter", "This is an example of a boolean parameter.", false,
            false, CapeParamMode.CAPE_INPUT_OUTPUT));

        // 创建一个字符串数组，用于下拉选项的值列表
        string[] options = ["TestValue", "AnotherValue"];

        // 使用 OptionParameter 构造函数添加一个示例的下拉框字符串值参数：OptionParameter
        // 默认值为 TestValue
        Parameters.Add(new OptionParameter("OptionParameter", "This is an example of an option parameter.",
            "TestValue", "TestValue", options, true, CapeParamMode.CAPE_INPUT_OUTPUT));

        // 添加可用报告
        Reports.Add("Report 2");
    }

    /// <summary>混合器单元模块的计算</summary>
    /// <remarks>
    /// 混合器模块将来自两个入口端口的物料流合并为单个出口端口的物流。
    /// 为完成此计算，混合器模块从每个入口端口获取流量信息，将流量相加后得出出口端口的流量。
    /// 在如下所示的混合器中，假设每个物料对象的组分相同，且组分按相同顺序排列。
    /// 在出口端口设定值处计算合并流量后，结合基于能量平衡计算的流体焓值及由入口压力确定的压力值，
    /// 即可对出口流体进行闪蒸以确定平衡状态。最后需释放任何重复生成的物流对象
    /// </remarks>
    /// <example>
    /// <para>计算单元操作的示例。该方法通过 <see cref="PortCollection"/> 类从每个端口获取物料对象。
    /// 通过 <see cref="ICapeThermoMaterialObject"/> 接口调用 CompIds()方法获取名称，
    /// 使用 <see cref="ICapeThermoMaterialObject.GetProp"/> 方法获取物料对象中各组件的流量、总压力及总焓值。
    /// 物流的总焓值通过 <see cref="ICapeThermoMaterialObject.CalcProp"/> 方法计算得出。
    /// 随后该单元将两股流体合并，计算输出流焓值，并根据两股流体压力值与压降参数中的较低值确定输出压力。
    /// 最后，通过 <see cref="ICapeThermoMaterialObject.SetProp"/> 方法将计算结果应用于输出物料对象。
    /// 计算方法的最后步骤是调用物流的 <see cref="ICapeThermoMaterialObject.CalcEquilibrium"/> 方法。</para>
    /// <para>在此情况下，需要释放入口物流。这通过调用 <see cref="Marshal.ReleaseComObject"/> 方法实现。
    /// 若使用此方法释放出口物流对象，将导致空对象引用错误。</para>
    /// <para>该方法还记录了使用 <see cref="CapeObjectBase.throwException"/> 方法来提供符合 CAPE-OPEN 规范的错误处理机制。</para>
    /// <code>
    /// protected override void Calculate()
    /// {
    ///    // Log a message using the simulation context (pop-up message commented out.
    ///    if (this.SimulationContext != null)
    ///        ((ICapeDiagnostic)this.SimulationContext).LogMessage("Starting Mixer Calculation");
    ///    //(CapeOpen.ICapeDiagnostic)(this.SimulationContext).PopUpMessage("Starting Mixer Calculation");
    ///    
    ///    // Get the material Object from Port 0.
    ///    ICapeThermoMaterialObject in1 = null;
    ///    ICapeThermoMaterialObject tempMO = null;
    ///    try
    ///    {
    ///        tempMO = (ICapeThermoMaterialObject)this.Ports[0].connectedObject;
    ///    }
    ///    catch (System.Exception p_Ex)
    ///    {
    ///        this.OnUnitOperationEndCalculation("Error - Material object does not support CAPE-OPEN Thermodynamics 1.0.");
    ///        CapeOpen.CapeInvalidOperationException ex = new CapeOpen.CapeInvalidOperationException("Material object does not support CAPE-OPEN Thermodynamics 1.0.", p_Ex);
    ///        this.throwException(ex);
    ///    }
    ///    
    ///    // Duplicate the port, its an input port, always use a duplicate.
    ///    try
    ///    {
    ///        in1 = (ICapeThermoMaterialObject)tempMO.Duplicate();
    ///    }
    ///    catch (System.Exception p_Ex)
    ///    {
    ///        this.OnUnitOperationEndCalculation("Error - Object connected to Inlet Port 1 does not support CAPE-OPEN Thermodynamics 1.0.");
    ///        CapeOpen.CapeInvalidOperationException ex = new CapeOpen.CapeInvalidOperationException("Object connected to Inlet Port 1 does not support CAPE-OPEN Thermodynamics 1.0.", p_Ex);
    ///        this.throwException(ex);
    ///    }
    ///    // Arrays for the GetProps and SetProps call for enthaply.
    ///    String[] phases = { "Overall" };
    ///    String[] props = { "enthalpy" };
    ///    
    ///    // Declare variables for calculations.
    ///    String[] in1Comps = null;
    ///    double[] in1Flow = null;
    ///    double[] in1Enthalpy = null;
    ///    double[] pressure = null;
    ///    double totalFlow1 = 0;
    ///    
    ///    // Exception catching code...
    ///    try
    ///    {
    ///        // Get Strings, must cast to string array data type.
    ///        in1Comps = (String[])in1.ComponentIds;
    ///        
    ///        // Get flow. Arguments are the property; phase, in this case, Overall; compound identifications
    ///        // in this case, the null returns the property for all components; calculation type, in this case,  
    ///        // null, no calculation type; and lastly, the basis, moles. Result is cast to a double array, and will contain one value.
    ///        in1Flow = (double[])in1.GetProp("flow", "Overall", null, null, "mole");
    ///        
    ///        // Get pressure. Arguments are the property; phase, in this case, Overall; compound identifications
    ///        // in this case, the null returns the property for all components; calculation type, in this case, the 
    ///        // mixture; and lastly, the basis, moles. Result is cast to a double array, and will contain one value.
    ///        pressure = (double[])in1.GetProp("Pressure", "Overall", null, "Mixture", null);
    ///        
    ///        // The following code adds the individual flows to get the total flow for the stream.
    ///        for (int i = 0; i &lt; in1Flow.Length; i++)
    ///        {
    ///            totalFlow1 = totalFlow1 + in1Flow[i];
    ///        }
    ///        
    ///        // Calculates the mixture enthalpy of the stream.
    ///        in1.CalcProp(props, phases, "Mixture");
    ///        
    ///        // Get the enthalpy of the stream. Arguments are the property, enthalpy; the phase, overall;
    ///        // a null pointer, required as the overall enthalpy is desired; the calculation type is
    ///        // mixture; and the basis is moles.
    ///        in1Enthalpy = (double[])in1.GetProp("enthalpy", "Overall", null, "Mixture", "mole");
    ///    }
    ///    catch (System.Exception p_Ex)
    ///    {
    ///        // Exception handling, wraps a COM exception, shows the message, and re-throws the excecption.
    ///        if (p_Ex is System.Runtime.InteropServices.COMException)
    ///        {
    ///            System.Runtime.InteropServices.COMException comException = (System.Runtime.InteropServices.COMException)p_Ex;
    ///            p_Ex = CapeOpen.COMExceptionHandler.ExceptionForHRESULT(in1, p_Ex);
    ///        }
    ///        this.throwException(p_Ex);
    ///    }
    ///    IDisposable disp = in1 as IDisposable;
    ///    if (disp != null)
    ///    {
    ///        disp.Dispose();
    ///    }
    ///    
    ///
    ///    // Get the material Object from Port 0.
    ///    ICapeThermoMaterialObject in2 = null;
    ///    tempMO = null;
    ///    try
    ///    {
    ///        tempMO = (ICapeThermoMaterialObject)this.Ports[1].connectedObject;
    ///    }
    ///    catch (System.Exception p_Ex)
    ///    {
    ///        this.OnUnitOperationEndCalculation("Error - Material object does not support CAPE-OPEN Thermodynamics 1.0.");
    ///        CapeOpen.CapeInvalidOperationException ex = new CapeOpen.CapeInvalidOperationException("Material object does not support CAPE-OPEN Thermodynamics 1.0.", p_Ex);
    ///        this.throwException(ex);
    ///    }
    ///    
    ///    // Duplicate the port, its an input port, always use a duplicate.
    ///    try
    ///    {
    ///        in2 = (ICapeThermoMaterialObject)tempMO.Duplicate();
    ///    }
    ///    catch (System.Exception p_Ex)
    ///    {
    ///        this.OnUnitOperationEndCalculation("Error - Object connected to Inlet Port 1 does not support CAPE-OPEN Thermodynamics 1.0.");
    ///        CapeOpen.CapeInvalidOperationException ex = new CapeOpen.CapeInvalidOperationException("Object connected to Inlet Port 1 does not support CAPE-OPEN Thermodynamics 1.0.", p_Ex);
    ///        this.throwException(ex);
    ///    }
    ///    
    ///    // Declare variables.
    ///    String[] in2Comps = null;
    ///    double[] in2Flow = null;
    ///    double[] in2Enthalpy = null;
    ///    double totalFlow2 = 0;
    ///    
    ///    // Try block.
    ///    try
    ///    {
    ///        // Get the component identifications.
    ///        in2Comps = in2.ComponentIds;
    ///        
    ///        // Get flow. Arguments are the property; phase, in this case, Overall; compound identifications
    ///        // in this case, the null returns the property for all components; calculation type, in this case,  
    ///        // null, no calculation type; and lastly, the basis, moles. Result is cast to a double array, and will contain one value.
    ///        in2Flow = in2.GetProp("flow", "Overall", null, null, "mole");
    ///        
    ///        // Get pressure. Arguments are the property; phase, in this case, Overall; compound identifications
    ///        // in this case, the null returns the property for all components; calculation type, in this case, the 
    ///        // mixture; and lastly, the basis, moles. Result is cast to a double array, and will contain one value.
    ///        double[] press = in2.GetProp("Pressure", "Overall", null, "Mixture", null);
    ///        if (press[0] &lt; pressure[0]) pressure[0] = press[0];
    ///        
    ///        // The following code adds the individual flows to get the total flow for the stream.
    ///        for (int i = 0; i &lt; in2Flow.Length; i++)
    ///        {
    ///            totalFlow2 = totalFlow2 + in2Flow[i];
    ///        }
    ///        
    ///        // Calculates the mixture enthalpy of the stream.
    ///        in2.CalcProp(props, phases, "Mixture");
    ///        
    ///        // Get the enthalpy of the stream. Arguments are the property, enthalpy; the phase, overall;
    ///        // a null pointer, required as the overall enthalpy is desired; the calculation type is
    ///        // mixture; and the basis is moles.
    ///        in2Enthalpy = in2.GetProp("enthalpy", "Overall", null, "Mixture", "mole");
    ///    }
    ///    catch (System.Exception p_Ex)
    ///    {
    ///        System.Runtime.InteropServices.COMException comException = (System.Runtime.InteropServices.COMException)p_Ex;
    ///        if (comException != null)
    ///        {
    ///            p_Ex = CapeOpen.COMExceptionHandler.ExceptionForHRESULT(in2, p_Ex);
    ///        }
    ///        this.throwException(p_Ex);
    ///    }
    ///    // Release the material object if it is a COM object.
    ///    disp = in2 as IDisposable;
    ///    if (disp != null)
    ///    {
    ///        disp.Dispose();
    ///    }
    ///    
    /// 
    ///    // Get the outlet material object.
    ///    ICapeThermoMaterialObject outPort = (ICapeThermoMaterialObject)this.Ports[2].connectedObject;
    ///    
    ///    // An empty, one-member array to set values in the outlet material stream.
    ///    double[] values = new double[1];
    ///    
    ///    // Use energy balanace to calculate the outlet enthalpy.
    ///    values[0] = (in1Enthalpy[0] * totalFlow1 + in2Enthalpy[0] * totalFlow2) / (totalFlow1 + totalFlow2);
    ///    try
    ///    {
    ///        // Set the outlet enthalpy, for the overall phase, with a mixture calculation type
    ///        // to the value calculated above.
    ///        outPort.SetProp("enthalpy", "Overall", null, "Mixture", "mole", values);
    ///        
    ///        // Set the outlet pressure to the lower of the to inlet pressures less the value of the 
    ///        // pressure drop parameter.
    ///        pressure[0] = pressure[0] - (((RealParameter)(this.Parameters[0])).SIValue);
    ///        
    ///        // Set the outlet pressure.
    ///        outPort.SetProp("Pressure", "Overall", null, null, null, pressure);
    ///        
    ///        // Resize the value array for the number of components.
    ///        values = new double[in1Comps.Length];
    ///        
    ///        // Calculate the individual flow for each component.
    ///        for (int i = 0; i &lt; in1Comps.Length; i++)
    ///        {
    ///            values[i] = in1Flow[i] + in2Flow[i];
    ///        }
    ///        // Set the outlet flow by component. Note, this is for overall phase and mole flows.
    ///        // The component Identifications are used as a check.
    ///        outPort.SetProp("flow", "Overall", in1Comps, null, "mole", values);
    ///        
    ///        // Calculate equilibrium using a "pressure-enthalpy" flash type.
    ///        outPort.CalcEquilibrium("PH", null);
    ///        
    ///        // Release the material object if it is a COM object.
    ///        if (outPort.GetType().IsCOMObject)
    ///        {
    ///            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(outPort);
    ///        }
    ///    }
    ///    catch (System.Exception p_Ex)
    ///    {
    ///        System.Runtime.InteropServices.COMException comException = (System.Runtime.InteropServices.COMException)p_Ex;
    ///        if (comException != null)
    ///        {
    ///            p_Ex = CapeOpen.COMExceptionHandler.ExceptionForHRESULT(outPort, p_Ex);
    ///        }
    ///        this.throwException(p_Ex);
    ///    }
    ///    
    ///    // Log the end of the calculation.
    ///    if (this.SimulationContext != null)
    ///        ((CapeOpen.ICapeDiagnostic)this.SimulationContext).LogMessage("Ending Mixer Calculation");
    ///    //(CapeOpen.ICapeDiagnostic)(this.SimulationContext).PopUpMessage("Ending Mixer Calculation");
    /// }
    /// </code>
    /// </example>
    /// <see cref="ICapeThermoMaterialObject"/>
    /// <see cref="COMExceptionHandler"/>
    protected override void Calculate()
    {
        // 使用模拟上下文记录一条消息
        if (SimulationContext != null)
            (SimulationContext as ICapeDiagnostic)?.LogMessage("Starting Mixer Calculation");
        // 直接弹出消息
        // (CapeOpen.ICapeDiagnostic)(this.SimulationContext).PopUpMessage("Starting Mixer Calculation");

        #region 单元模块端口 0 处理

        // 从端口 0 获取对象。
        ICapeThermoMaterialObject in1 = null;
        ICapeThermoMaterialObject tempMo = null;
        try
        {
            tempMo = (ICapeThermoMaterialObject)Ports[0].connectedObject;
        }
        catch (Exception pEx)
        {
            OnUnitOperationEndCalculation("Error - Material object does not support CAPE-OPEN Thermodynamics 1.0.");
            CapeInvalidOperationException ex =
                new CapeInvalidOperationException("Material object does not support CAPE-OPEN Thermodynamics 1.0.",
                    pEx);
            throwException(ex);
        }

        // 复制该端口，它是一个输入端口，请始终使用复制端口
        try
        {
            in1 = (ICapeThermoMaterialObject)tempMo.Duplicate();
        }
        catch (Exception pEx)
        {
            OnUnitOperationEndCalculation(
                "Error - Object connected to Inlet Port 1 does not support CAPE-OPEN Thermodynamics 1.0.");
            CapeInvalidOperationException ex = new CapeInvalidOperationException(
                "Object connected to Inlet Port 1 does not support CAPE-OPEN Thermodynamics 1.0.", pEx);
            throwException(ex);
        }

        // GetProps 和 SetProps 调用需要数组
        string[] phases = ["Overall"];
        string[] props = ["enthalpy"];

        // 声明用于计算的变量
        string[] in1Comps = null;
        double[] in1Flow = null;
        double[] in1Enthalpy = null;
        double[] pressure = null;
        double totalFlow1 = 0;
        
        try
        {
            // 获取字符串时，必须转换为字符串数组数据类型。
            in1Comps = (string[])in1.ComponentIds;

            // 获取流量：
            // 第一个参数为要获取的物性，示例为流量 flow；
            // 第二个参数为相态，示例为整体（或混合）；
            // 第三个参数为组分 Id，示例为 null；null 表示返回所有组分（整个物流对象）的物性
            // 第四个参数为计算类型，示例为 null；null 表示没有计算类型
            // 第五个参数为物性基准（非单位），示例为 mole；
            // 结果被转换为双精度数组，并将包含一个值
            in1Flow = (double[])in1.GetProp("flow", "Overall", null, null, "mole");

            // 获取压力：
            // 第一个参数为要获取的物性，示例为压力 Pressure；
            // 第二个参数为相态，示例为整体（或混合）；
            // 第三个参数为组分 Id，示例为 null；null 表示返回所有组分（整个物流对象）的物性
            // 第四个参数为计算类型，示例为 Mixture；
            // 第五个参数为物性基准（非单位），示例为 null；
            // 结果被转换为双精度数组，并将包含一个值
            pressure = (double[])in1.GetProp("Pressure", "Overall", null, "Mixture", null);

            // 将各个物流的流量相加，以获得该流的总流量。
            totalFlow1 = in1Flow.Aggregate(totalFlow1, (current, t) => current + t);

            // 计算该物流的混合焓
            in1.CalcProp(props, phases, "Mixture");

            // 获取物流的焓：
            // 第一个参数为要获取的物性，示例为焓 enthalpy；
            // 第二个参数为相态，示例为整体（或混合）；
            // 第三个参数为组分 Id，示例为 null；null 表示返回所有组分（整个物流对象）的物性
            // 第四个参数为计算类型，示例为 Mixture；
            // 第五个参数为物性基准（非单位），示例为 mole；
            // 结果被转换为双精度数组，并将包含一个值
            in1Enthalpy = (double[])in1.GetProp("enthalpy", "Overall", null, "Mixture", "mole");
        }
        catch (Exception pEx)
        {
            // 异常处理，封装COM异常，显示消息并重新抛出异常。
            if (pEx is COMException comException)
            {
                pEx = COMExceptionHandler.ExceptionForHRESULT(in1, comException);
            }

            throwException(pEx);
        }

        // 若该对象为 COM 对象，则释放该对象。
        if (in1 is IDisposable disp)
        {
            disp.Dispose();
        }

        #endregion

        #region 单元模块端口 1 处理

        // 从端口 1 获取物流对象
        ICapeThermoMaterialObject in2 = null;
        tempMo = null;
        try
        {
            tempMo = (ICapeThermoMaterialObject)Ports[1].connectedObject;
        }
        catch (Exception pEx)
        {
            OnUnitOperationEndCalculation("Error - Material object does not support CAPE-OPEN Thermodynamics 1.0.");
            CapeInvalidOperationException ex =
                new CapeInvalidOperationException("Material object does not support CAPE-OPEN Thermodynamics 1.0.",
                    pEx);
            throwException(ex);
        }

        // 复制该端口，它是一个输入端口，请始终使用复制端口。
        try
        {
            in2 = (ICapeThermoMaterialObject)tempMo.Duplicate();
        }
        catch (Exception pEx)
        {
            OnUnitOperationEndCalculation(
                "Error - Object connected to Inlet Port 1 does not support CAPE-OPEN Thermodynamics 1.0.");
            CapeInvalidOperationException ex = new CapeInvalidOperationException(
                "Object connected to Inlet Port 1 does not support CAPE-OPEN Thermodynamics 1.0.", pEx);
            throwException(ex);
        }

        // 声明用于计算的变量
        string[] in2Comps = null;
        double[] in2Flow = null;
        double[] in2Enthalpy = null;
        double totalFlow2 = 0;

        try
        {
            // 获取组分 Id
            in2Comps = in2.ComponentIds;

            // 获取流量：
            // 第一个参数为要获取的物性，示例为流量 flow；
            // 第二个参数为相态，示例为整体（或混合）；
            // 第三个参数为组分 Id，示例为 null；null 表示返回所有组分（整个物流对象）的物性
            // 第四个参数为计算类型，示例为 null；null 表示没有计算类型
            // 第五个参数为物性基准（非单位），示例为 mole；
            // 结果被转换为双精度数组，并将包含一个值
            in2Flow = in2.GetProp("flow", "Overall", null, null, "mole");

            // 获取压力：
            // 第一个参数为要获取的物性，示例为压力 Pressure；
            // 第二个参数为相态，示例为整体（或混合）；
            // 第三个参数为组分 Id，示例为 null；null 表示返回所有组分（整个物流对象）的物性
            // 第四个参数为计算类型，示例为 Mixture；
            // 第五个参数为物性基准（非单位），示例为 null；
            // 结果被转换为双精度数组，并将包含一个值
            double[] press = in2.GetProp("Pressure", "Overall", null, "Mixture", null);
            if (press[0] < pressure[0]) pressure[0] = press[0];

            // 将各个物流的流量相加，以获得该流的总流量。
            totalFlow2 = in2Flow.Aggregate(totalFlow2, (current, t) => current + t);

            // 计算该物流的混合焓
            in2.CalcProp(props, phases, "Mixture");

            // 获取物流的焓：
            // 第一个参数为要获取的物性，示例为焓 enthalpy；
            // 第二个参数为相态，示例为整体（或混合）；
            // 第三个参数为组分 Id，示例为 null；null 表示返回所有组分（整个物流对象）的物性
            // 第四个参数为计算类型，示例为 Mixture；
            // 第五个参数为物性基准（非单位），示例为 mole；
            // 结果被转换为双精度数组，并将包含一个值
            in2Enthalpy = in2.GetProp("enthalpy", "Overall", null, "Mixture", "mole");
        }
        catch (Exception pEx)
        {
            COMException comException = (COMException)pEx;
            if (comException != null)
            {
                pEx = COMExceptionHandler.ExceptionForHRESULT(in2, pEx);
            }

            throwException(pEx);
        }

        // 若该对象为 COM 对象，则释放该对象。
        disp = in2 as IDisposable;
        if (disp != null)
        {
            disp.Dispose();
        }

        #endregion

        #region 单元模块端口 2 处理

        // 获取混合器出口物流对象
        ICapeThermoMaterialObject outPort = (ICapeThermoMaterialObject)Ports[2].connectedObject;

        // 一个空的、仅含一个元素的数组，用于在出口流股对象中设置值
        double[] values = new double[1];

        // 使用能量平衡法计算出口焓
        values[0] = (in1Enthalpy[0] * totalFlow1 + in2Enthalpy[0] * totalFlow2) / (totalFlow1 + totalFlow2);
        try
        {
            // 将混合物计算类型的整体相出口焓值设置为上述计算值
            outPort.SetProp("enthalpy", "Overall", null, "Mixture", "mole", values);

            // 将出口压力设置为两个入口压力中较低者减去压降参数值
            pressure[0] = pressure[0] - (((RealParameter)(Parameters[0])).SIValue);

            // 设定出口压力
            outPort.SetProp("Pressure", "Overall", null, null, null, pressure);

            // 根据组分数量调整值数组的大小
            values = new double[in1Comps.Length];

            // 计算每个组分的单独流量
            for (var i = 0; i < in1Comps.Length; i++)
            {
                values[i] = in1Flow[i] + in2Flow[i];
            }

            // 按组分设置出口流量。注意，此处指整体相流和摩尔流。组分标识用于核查
            outPort.SetProp("flow", "Overall", in1Comps, null, "mole", values);

            // 使用“压力-焓”闪蒸类型计算平衡
            outPort.CalcEquilibrium("PH", null);

            // 如果该对象是 COM 对象，则释放该对象
            if (outPort.GetType().IsCOMObject)
            {
                Marshal.FinalReleaseComObject(outPort);
            }
        }
        catch (Exception pEx)
        {
            COMException comException = (COMException)pEx;
            if (comException != null)
            {
                pEx = COMExceptionHandler.ExceptionForHRESULT(outPort, pEx);
            }

            throwException(pEx);
        }

        #endregion

        // 记录计算结束
        if (SimulationContext != null)
            (SimulationContext as ICapeDiagnostic).LogMessage("Ending Mixer Calculation");
        // (CapeOpen.ICapeDiagnostic)(this.SimulationContext).PopUpMessage("Ending Mixer Calculation");
    }

    /// <summary>生成混合器示例单元操作的活动报告</summary>
    /// <remarks>ProduceReport 方法为单元操作创建活动报告。该方法参照 <see cref="CapeUnitBase.selectedReport"/> 并生成所需报告。
    /// 若已添加本地报告（如 <see cref="MixerExample"/> 所示），则本方法必须生成该报告。
    /// </remarks>
    /// <example>
    /// An example of how to produce a report for a unit operation. In this case, the report can be either 
    /// "Report 2" defined in the <see cref="MixerExample"/> or the "Default Report" from 
    /// <see cref="CapeUnitBase"/>. If "Default Report" is selected, then the <see cref="CapeUnitBase.ProduceReport"/>
    /// method is called, and the message parameter forwarded. Otherwise, the report is generated in this method.
    /// "Default Report" gen.
    /// <code>
    /// public override void ProduceReport(ref String message)
    /// {
    ///     if (this.selectedReport == "Default Report") base.ProduceReport(ref message);
    ///     if (this.selectedReport == "Report 2") message = "This is the alternative Report.";
    /// }
    /// </code>
    /// </example>
    /// <returns>The report text.</returns>
    /// <exception cref="ECapeUnknown">The error to be raised when other error(s),  specified for this operation, are not suitable.</exception>
    /// <exception cref="ECapeNoImpl">ECapeNoImpl</exception>
    public override string ProduceReport()
    {
        if (selectedReport == "Default Report") return base.ProduceReport();
        if (selectedReport == "Report 2") return "This is the alternative Report.";
        return string.Empty;
    }
}