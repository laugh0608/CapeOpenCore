/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.15
 */

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using CapeOpen.Properties;

namespace CapeOpenCore.Class;

/// <summary>提供与参数相关的计量单位的相关信息。</summary>
/// <remarks><para>计量单位可以是国际系统（SI）单位或习惯单位。每个单位都被指派给
/// 一个 <see cref="unitCategory"/>，该类别包含与单位维度相关的信息。</para>
/// </remarks>
struct unit
{
    /// <summary>计量单位的名称。</summary>
    /// <remarks>度量单位的通用名称。通常，该字段表示该单位的缩写。</remarks>
    public string Name;
    
    /// <summary>计量单位的描述。</summary>
    /// <remarks>计量单位的描述。</remarks>
    public string Description;
    
    /// <summary>计量单位的分类。</summary>
    /// <remarks><para>度量单位的类别定义了该单位的维度。</para>
    /// <para>参数的维度代表该参数的物理维度轴。预计该维数必须至少涵盖 6 个基本轴：
    /// 长度 length、质量 mass、时间 time、角度angle、温度 temperature 和 电荷 charge。</para>
    /// <para>一种可能的实现方式可以是一个固定长度的数组向量，
    /// 其中包含每个基本 SI 单位的指数，遵循 SI 手册（来自 https://www.bipm.fr ）的指示。</para>
    /// <para>So if we agree on order &lt;m kg s A K,&gt; ... velocity would be &lt;1,0,-1,0,0,0&gt;:
    /// that is m1 * s-1 =m/s.</para>
    /// <para>我们已建议 CO 科学委员会采用国际单位制（SI）基本单位加上带有特殊符号的 SI 导出
    /// 单位（以提高易用性并允许定义角度）。</para></remarks>
    public string Category;
    
    /// <summary>用于将测量值乘以该系数以将其单位转换为国际单位制（SI）等效单位的转换系数。</summary>
    /// <remarks>对于单位类别，会将单位进行 SI 单位的等效转换。进行单位转换的步骤是，首先将存储
    /// 在 <see cref="ConversionPlus"/> 中的任何偏移量加到该单位的值上，然后将该总和乘以该单位
    /// 的 <see cref="ConversionTimes"/> 值，以获取以 SI 单位表示的测量值。</remarks>
    public double ConversionTimes;
    
    /// <summary>用于将测量值转换为其国际单位制（SI）等效值的偏移因子。</summary>
    /// <remarks>对于单位类别，会将单位进行 SI 单位的等效转换。进行单位转换的步骤是，首先将存储
    /// 在 <see cref="ConversionPlus"/> 中的任何偏移量加到该单位的值上，然后将该总和乘以该单位
    /// 的 <see cref="ConversionTimes"/> 值，以获取以 SI 单位表示的测量值。</remarks>
    public double ConversionPlus;
}

struct unitCategory
{
    /// <summary>获取单元类别的名称，例如压力、温度。</summary>
    /// <remarks>单元类别表示与特定度量单位相关联的独特组合维度（质量、长度、时间、温度、物质量（摩尔）、电流、亮度）。</remarks>
    public string Name;
    
    /// <summary>获取参数的显示单元。由 AspenPlus(TM) 使用。</summary>
    /// <remarks><para>DisplayUnits 用于定义参数的测量单位符号。</para>
    /// <para>注：该符号必须是 AspenPlus 所识别的大写字符串之一，以确保它能够对参数值进行单位换算。系统会将参数的值
    /// 从 SI 单位进行转换，以便在数据浏览器中显示，并将更新后的值重新转换为 SI 单位。</para></remarks>
    public string AspenUnit;
    
    /// <summary>获取与单位类别关联的国际单位制（SI）单位名称，例如压力单位为帕斯卡（Pascal）。</summary>
    /// <remarks>国际单位制（SI）单位是同一类别中任何两个单位（无论是 SI 单位还是习惯单位）之间进行换算的基础。</remarks>
    public string SI_Unit;
    
    /// <summary>获取与单位类别相关的质量维度。</summary>
    /// <remarks>单位类别的质量维度。</remarks>
    public double Mass;
    
    /// <summary>获取与单位类别相关的时序维度。</summary>
    /// <remarks>单位类别的时空维度。</remarks>
    public double Time;
    
    /// <summary>获取与单位类别相关的长度维度。</summary>
    /// <remarks>单位类别的长度维度。</remarks>
    public double Length;
    
    /// <summary>获取与单位类别关联的电流（安培数）维度。</summary>
    /// <remarks>单位类别的电流（安培数）维数。</remarks>
    public double ElectricalCurrent;
    
    /// <summary>获取与单位类别关联的温度维数。</summary>
    /// <remarks>单位类别的温度维数。</remarks>
    public double Temperature;
    
    /// <summary>获取与单位类别关联的物质 （摩尔） 维数的数量。</summary>
    /// <remarks>单位类别的物质（摩尔）维数。</remarks>
    public double AmountOfSubstance;
    
    /// <summary>获取与单位类别关联的亮度维度。</summary>
    /// <remarks>单位类别的亮度维度。</remarks>
    public double Luminous;
    
    /// <summary>获取与单位类别关联的货币维数。</summary>
    /// <remarks>单位类别的金融货币维数。</remarks>
    public double Currency;
}

/// <summary>表示对 CAPE-OPEN 维度和实值参数的度量单位的支持 Static 类。</summary>
/// <remarks>该类支持使用 CAPE-OPEN 维数，并能在实值参数之间进行SI单位和常用度量单位的转换。</remarks>
internal static class CDimensions
{
    static ArrayList units;
    static ArrayList unitCategories;

    /// <summary>初始化 <see cref="CDimensions"/> 类的静态字段。</summary>
    /// <remarks>从 XML 文件中加载单元和单元类别数据。</remarks>
    static CDimensions()
    {
        units = new ArrayList();
        unitCategories = new ArrayList();
        var domain = AppDomain.CurrentDomain;
        
        var reader = new XmlDocument();
        reader.LoadXml(Resources.units);
        var list = reader.SelectNodes("Units/Unit_Specs");
        var ci = new CultureInfo(0x0409, false);
        for (var i = 0; i < list.Count; i++)
        {
            unit newUnit;
            var unitName = list[i].SelectSingleNode("Unit").InnerText;
            newUnit.Name = unitName.Trim();
            newUnit.Description = "";
            newUnit.Category = list[i].SelectSingleNode("Category").InnerText;
            newUnit.ConversionTimes = Convert.ToDouble(list[i].SelectSingleNode("ConversionTimes").InnerText, ci.NumberFormat);
            newUnit.ConversionPlus = Convert.ToDouble(list[i].SelectSingleNode("ConversionPlus").InnerText, ci.NumberFormat);
            units.Add(newUnit);
        }
        var userUnitPath = string.Concat(domain.BaseDirectory, "//data//user_defined_UnitsResult.XML");
        if (File.Exists(userUnitPath))
        {
            reader.Load(userUnitPath);
            list = reader.SelectNodes("Units/Unit_Specs");
            for (var i = 0; i < list.Count; i++)
            {
                var newUnit = new unit();
                var unitName = list[i].SelectSingleNode("Unit").InnerText;
                newUnit.Name = unitName.Trim();
                newUnit.Category = list[i].SelectSingleNode("Category").InnerText;
                newUnit.ConversionTimes = Convert.ToDouble(list[i].SelectSingleNode("ConversionTimes").InnerText, ci.NumberFormat);
                newUnit.ConversionPlus = Convert.ToDouble(list[i].SelectSingleNode("ConversionPlus").InnerText, ci.NumberFormat);
                units.Add(newUnit);
            }
        }
        reader.LoadXml(Resources.unitCategories);
        list = reader.SelectNodes("CategorySpecifications/Category_Spec");
        for (var i = 0; i < list.Count; i++)
        {
            var UnitName = list[i].SelectSingleNode("Category").InnerText;
            unitCategory category;
            category.Name = UnitName;
            category.AspenUnit = list[i].SelectSingleNode("Aspen").InnerText;
            category.SI_Unit = list[i].SelectSingleNode("SI_Unit").InnerText;
            category.Mass = Convert.ToDouble(list[i].SelectSingleNode("Mass").InnerText);
            category.Time = Convert.ToDouble(list[i].SelectSingleNode("Time").InnerText);
            category.Length = Convert.ToDouble(list[i].SelectSingleNode("Length").InnerText);
            category.ElectricalCurrent = Convert.ToDouble(list[i].SelectSingleNode("ElectricalCurrent").InnerText);
            category.Temperature = Convert.ToDouble(list[i].SelectSingleNode("Temperature").InnerText);
            category.AmountOfSubstance = Convert.ToDouble(list[i].SelectSingleNode("AmountOfSubstance").InnerText);
            category.Luminous = Convert.ToDouble(list[i].SelectSingleNode("Luminous").InnerText, ci.NumberFormat);
            category.Currency = Convert.ToDouble(list[i].SelectSingleNode("Currency").InnerText, ci.NumberFormat);
            unitCategories.Add(category);
        }
        var userUnitCategoryPath = string.Concat(domain.BaseDirectory, "data//user_defined_units.XML");
        if (!File.Exists(userUnitCategoryPath)) return;
        {
            reader.Load(userUnitCategoryPath);
            list = reader.SelectNodes("CategorySpecifications/Category_Spec");
            for (var i = 0; i < list.Count; i++)
            {
                var UnitName = list[i].SelectSingleNode("Category").InnerText;
                unitCategory category;
                category.Name = UnitName;
                category.AspenUnit = list[i].SelectSingleNode("Aspen").InnerText;
                category.SI_Unit = list[i].SelectSingleNode("SI_Unit").InnerText;
                category.Mass = Convert.ToDouble(list[i].SelectSingleNode("Mass").InnerText, ci.NumberFormat);
                category.Time = Convert.ToDouble(list[i].SelectSingleNode("Time").InnerText, ci.NumberFormat);
                category.Length = Convert.ToDouble(list[i].SelectSingleNode("Length").InnerText, ci.NumberFormat);
                category.ElectricalCurrent = Convert.ToDouble(list[i].SelectSingleNode("ElectricalCurrent").InnerText, ci.NumberFormat);
                category.Temperature = Convert.ToDouble(list[i].SelectSingleNode("Temperature").InnerText, ci.NumberFormat);
                category.AmountOfSubstance = Convert.ToDouble(list[i].SelectSingleNode("AmountOfSubstance").InnerText, ci.NumberFormat);
                category.Luminous = Convert.ToDouble(list[i].SelectSingleNode("Luminous").InnerText, ci.NumberFormat);
                category.Currency = Convert.ToDouble(list[i].SelectSingleNode("Currency").InnerText, ci.NumberFormat);
                unitCategories.Add(category);
            }
        }
    }
    
    /// <summary>与维度相关联的 SI 单位。</summary>
    /// <remarks><para>与维数关联的 SI 度量单位。</para>
    /// <para>参数的维度代表该参数的物理维度轴。预计该维数必须至少涵盖 6 个基本轴：
    /// 长度 length、质量 mass、时间 time、角度angle、温度 temperature 和 电荷 charge。</para>
    /// <para>一种可能的实现方式可以是一个固定长度的数组向量，
    /// 其中包含每个基本 SI 单位的指数，遵循 SI 手册（来自 https://www.bipm.fr ）的指示。</para>
    /// <para>So if we agree on order &lt;m kg s A K,&gt; ... velocity would be &lt;1,0,-1,0,0,0&gt;:
    /// that is m1 * s-1 =m/s.</para>
    /// <para>我们已建议 CO 科学委员会采用国际单位制（SI）基本单位加上带有特殊符号的 SI 导出
    /// 单位（以提高易用性并允许定义角度）。</para></remarks>
    /// <param name="dimensions">单位的维度。</param>
    /// <returns>具有所需维度的国际单位制。</returns>
    public static string SIUnit(int[] dimensions)
    {
        foreach (unitCategory category in unitCategories)
        {
            if (dimensions[0] == category.Length &&
                dimensions[1] == category.Mass &&
                dimensions[2] == category.Time &&
                dimensions[3] == category.ElectricalCurrent &&
                dimensions[4] == category.Temperature &&
                dimensions[5] == category.AmountOfSubstance &&
                dimensions[6] == category.Luminous)
                return category.SI_Unit;
        }
        return string.Empty;
    }

    /// <summary>与维度相关联的 SI 单位。</summary>
    /// <remarks><para>与维度相关联的 SI 计量单位。</para>
    /// <para>参数的维度代表该参数的物理维度轴。预计该维数必须至少涵盖 6 个基本轴：
    /// 长度 length、质量 mass、时间 time、角度angle、温度 temperature 和 电荷 charge。</para>
    /// <para>一种可能的实现方式可以是一个固定长度的数组向量，
    /// 其中包含每个基本 SI 单位的指数，遵循 SI 手册（来自 https://www.bipm.fr ）的指示。</para>
    /// <para>So if we agree on order &lt;m kg s A K,&gt; ... velocity would be &lt;1,0,-1,0,0,0&gt;:
    /// that is m1 * s-1 =m/s.</para>
    /// <para>我们已建议 CO 科学委员会采用国际单位制（SI）基本单位加上带有特殊符号的 SI 导出
    /// 单位（以提高易用性并允许定义角度）。</para>
    /// </remarks>
    /// <param name="dimensions">该单元的维数。</param>
    /// <returns>具有所需维度的国际单位制单位。</returns>
    public static string SIUnit(double[] dimensions)
    {
        foreach (unitCategory category in unitCategories)
        {
            if (dimensions[0] == category.Length &&
                dimensions[1] == category.Mass &&
                dimensions[2] == category.Time &&
                dimensions[3] == category.ElectricalCurrent &&
                dimensions[4] == category.Temperature &&
                dimensions[5] == category.AmountOfSubstance &&
                dimensions[6] == category.Luminous)
                return category.SI_Unit;
        }
        return string.Empty;
    }

    /// <summary>提供由维度包支持的所有单位。</summary>
    /// <remarks>提供本单位转换包中所有可用测量单位的列表。</remarks>
    /// <value>所有单元的列表。</value>
    public static string[] Units
    {
        get
        {
            var retVal = new string[units.Count];
            for (var i = 0; i < units.Count; i++)
            {
                retVal[i] = ((unit)units[i]).Name;
            }
            return retVal;
        }
    }

    /// <summary>一种转换系数，用于将测量值乘以该系数，以便将单位转换为其国际单位制等价物。</summary>
    /// <remarks>对于单位类别，会将单位进行 SI 等效换算。进行单位换算时，首先将存储
    /// 在 <see cref="ConversionPlus"/> 中的任何偏移量加到该单位的值上，然后将该总和乘以该单位
    /// 的 <see cref="ConversionTimes"/> 值，以获取以SI单位表示的测量值。</remarks>
    /// <param name="unit">用于获取转换因子的单位。</param>
    /// <returns>转换因子的乘法部分。</returns>
    public static double ConversionTimes(string unit)
    {
        double retVal = 0;
        var found = false;
        foreach (var pT in units)
        {
            var current = (unit)pT;
            if (current.Name != unit) continue;
            retVal = current.ConversionTimes;
            found = true;
        }
        if (!found) throw new CapeBadArgumentException(string.Concat("Unit: ", unit, " was not found"), 1);
        return retVal;
    }

    /// <summary>用于将测量值转换为其国际单位制（SI）等效值的偏移因子。</summary>
    /// <remarks>对于单位类别，会将单位进行 SI 等效换算。进行单位换算时，首先将存储
    /// 在 <see cref="ConversionPlus"/> 中的任何偏移量加到该单位的值上，然后将该总和乘以该单位
    /// 的 <see cref="ConversionTimes"/> 值，以获取以 SI 单位表示的测量值。</remarks>
    /// <param name="unit">用于获取转换因子的单位。</param>
    /// <returns>转换因子的加成部分。</returns>
    public static double ConversionPlus(string unit)
    {
        double retVal = 0;
        var found = false;
        foreach (var pT in units)
        {
            var current = (unit)pT;
            if (current.Name != unit) continue;
            retVal = current.ConversionPlus;
            found = true;
        }
        if (!found) throw new CapeBadArgumentException(string.Concat("Unit: ", unit, " was not found"), 1);
        return retVal;
    }

    /// <summary>计量单位的分类。</summary>
    /// <remarks>度量单位的类别定义了该单位的维度。</remarks>
    /// <param name="unit">用于获取类别的单元。</param>
    /// <returns>单元类别。</returns>
    public static string UnitCategory(string unit)
    {
        var retVal = string.Empty;
        var found = false;
        foreach (var pT in units)
        {
            var current = (unit)pT;
            if (current.Name != unit) continue;
            retVal = current.Category;
            found = true;
        }
        if (!found) throw new CapeBadArgumentException(string.Concat("Unit: ", unit, " was not found"), 1);
        return retVal;
    }

    /// <summary>获取与单位类别关联的国际单位制（SI）单位名称，例如压力单位为帕斯卡（Pascal）。</summary>
    /// <remarks>国际单位制（SI）单位是同一类别中任何两个单位（无论是 SI 单位还是习惯单位）之间进行换算的基础。</remarks>
    /// <returns>与当前设备对应的 Aspen(TM) 显示单元。</returns>
    /// <param name="unit">用于获取 Aspen(TM) 单元的设备。</param>
    public static string AspenUnit(string unit)
    {
        var retVal = string.Empty;
        var category = string.Empty;
        var found = false;
        foreach (var pT in units)
        {
            var current = (unit)pT;
            if (current.Name != unit) continue;
            category = current.Category;
            found = true;
        }
        foreach (var mT in unitCategories)
        {
            var current = (unitCategory)mT;
            if (current.Name != category) continue;
            retVal = current.AspenUnit;
            found = true;
        }
        if (!found) throw new CapeBadArgumentException(string.Concat("Unit: ", unit, " was not found"), 1);
        return retVal;
    }

    /// <summary>计量单位的描述。</summary>
    /// <remarks>计量单位的描述。</remarks>
    /// <param name="unit">用于获取转换因子的单位。</param>
    /// <returns>计量单位的描述。</returns>
    public static string UnitDescription(string unit)
    {
        var retVal = string.Empty;
        var found = false;
        foreach (var pT in units)
        {
            var current = (unit)pT;
            if (current.Name != unit) continue;
            retVal = current.Description;
            found = true;
        }
        if (!found) throw new CapeBadArgumentException(string.Concat("Unit: ", unit, " was not found"), 1);
        return retVal;
    }

    /// <summary>返回所有符合该单位类别的单位。</summary>
    /// <remarks>一个单元类别表示特定组合下的维度值。例如可以是压力或温度。该方法将返回属于该类别的所有单位，
    /// 如温度的摄氏、开尔文、Fahrenheit 和 Rankine。</remarks>
    /// <param name="category">所需单元的类别。</param>
    /// <returns>所有代表该类别的单元。</returns>
    public static string[] UnitsMatchingCategory(string category)
    {
        var unitNames = new ArrayList();
        foreach (var pT in units)
        {
            var current = (unit)pT;
            if (current.Category == category)
            {
                unitNames.Add(current.Name);
            }
        }
        var retVal = new string[unitNames.Count];
        for (var i = 0; i < unitNames.Count; i++)
        {
            retVal[i] = unitNames[i].ToString();
        }
        return retVal;
    }

    /// <summary>返回与该单位关联的国际单位制（SI）单位。</summary>
    /// <remarks>一个单元类别表示特定组合下的维度值。例如可以是压力或温度。该方法将返回该类别的国际单位制表示，
    /// 如温度为开尔文（K），压力为帕斯卡（N/m²）。</remarks>
    /// <param name="Unit">获取 SI 单位的单位。</param>
    /// <returns>与该单位对应的国际单位制（SI）单位。</returns>
    public static string FindSIUnit(string Unit)
    {
        var retVal = string.Empty;
        var category = UnitCategory(Unit);
        foreach (var pT in unitCategories)
        {
            var current = (unitCategory)pT;
            if (current.Name == category)
            {
                retVal = current.SI_Unit;
            }
        }
        return retVal;
    }

    /// <summary>测量单位的维度。</summary>
    /// <remarks><para>参数的维度代表该参数的物理维度轴。预计该维数必须至少涵盖 6 个基本轴：
    /// 长度 length、质量 mass、时间 time、角度angle、温度 temperature 和 电荷 charge。</para>
    /// <para>一种可能的实现方式可以是一个固定长度的数组向量，
    /// 其中包含每个基本 SI 单位的指数，遵循 SI 手册（来自 https://www.bipm.fr ）的指示。</para>
    /// <para>So if we agree on order &lt;m kg s A K,&gt; ... velocity would be &lt;1,0,-1,0,0,0&gt;:
    /// that is m1 * s-1 =m/s.</para>
    /// <para>我们已建议 CO 科学委员会采用国际单位制（SI）基本单位加上带有特殊符号的 SI 导出
    /// 单位（以提高易用性并允许定义角度）。</para></remarks>
    /// <param name="unit">用于确定维度的单元。</param>
    /// <returns>该单元的维数。</returns>
    public static double[] Dimensionality(string unit)
    {
        var category = UnitCategory(unit);
        double[] retVal = [0, 0, 0, 0, 0, 0, 0, 0];
        foreach (var pT in unitCategories)
        {
            var current = (unitCategory)pT;
            if (current.Name != category) continue;
            retVal[0] = current.Length;
            retVal[1] = current.Mass;
            retVal[2] = current.Time;
            retVal[3] = current.ElectricalCurrent;
            retVal[4] = current.Temperature;
            retVal[5] = current.AmountOfSubstance;
            retVal[6] = current.Luminous;
            retVal[7] = current.Currency;
        }
        return retVal;
    }
}