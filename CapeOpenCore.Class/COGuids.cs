/*
 * 原作者：wbarret1 (https://github.com/wbarret1/CapeOpen)
 * 重构 & 翻译：DaBaiLuoBo
 * 帮助社区：CEPD@BBS (https://bbs.imbhj.com)
 * 重构时间：2025.06.07
 */

/* IMPORTANT NOTICE
(c) The CAPE-OPEN Laboratory Network, 2002.
All rights are reserved unless specifically stated otherwise

Visit the web site at www.colan.org

This file has been edited using the editor from Microsoft Visual Studio 6.0
This file can view properly with any basic editors and browsers (validation done under MS Windows and Unix)
*/

// This file was developed/modified by JEAN-PIERRE-BELAUD for CO-LaN organisation - March 2003

namespace CapeOpenCore.Class;

class COGuids
{

 // CAPE-OPEN Category
 public const string CapeOpenComponent_CATID = "{678c09a1-7d66-11d2-a67d-00105a42887f}";
 // External CAPE-OPEN Thermo Routines
 public const string CapeExternalThermoRoutine_CATID = "{678c09a2-7d66-11d2-a67d-00105a42887f}";
 // CAPE-OPEN Thermo System
 public const string CapeThermoSystem_CATID = "{678c09a3-7d66-11d2-a67d-00105a42887f}";
 // CAPE-OPEN Thermo Property Package
 public const string CapeThermoPropertyPackage_CATID = "{678c09a4-7d66-11d2-a67d-00105a42887f}";
 // CAPE-OPEN Thermo Equilibrium Server
 public const string CapeThermoEquilibriumServer_CATID = "{678c09a6-7d66-11d2-a67d-00105a42887f}";
 // CAPE-OPEN Unit Operation
 public const string CapeUnitOperation_CATID = "{678c09a5-7d66-11d2-a67d-00105a42887f}";
 // CAEP-OPEN Reactions Package manager components
 public const string CAPEOPENReactionsPackageManager_CATID = "{678c09aa-0100-11d2-a67d-00105a42887f}";
 // CAPE-OPEN Standalone Reactions Package
 public const string CAPEOPENReactionsPackage_CATID = "{678c09ab-0100-11d2-a67d-00105a42887f}";
 // CAPE-OPEN MINLP Solver Package
 public const string CapeMINLPSolverPackage_CATID = "{678c09ac-7d66-11d2-a67d-00105a42887f}";
 // CAPE-OPEN PPDB Service
 public const string CapePPDBService_CATID = "{678c09aa-7d66-11d2-a67d-00105a42887f}";
 // CAPE-OPEN SMST Package
 public const string CapeSMSTPackage_CATID = "{678c09ab-7d66-11d2-a67d-00105a42887f}";
 // CAPE-OPEN Solvers Package
 public const string CapeSolversPackage_CATID = "{79DD785E-27E5-4939-B040-B1E45B1F2C64}";
 // CAPE-OPEN PSP Package
 public const string CapePSPPackage_CATID = "{3EFFA2BD-D9E7-4e55-B515-AD3E3623AAD5}";
 // CAPE-OPEN Monitoring Object
 public const string CATID_MONITORING_OBJECT = "{7BA1AF89-B2E4-493d-BD80-2970BF4CBE99}";
 // CAPE-OPEN Thermodynamics Consuming Object
 public const string Consumes_Thermo_CATID = "{4150C28A-EE06-403f-A871-87AFEC38A249}";
 // CAPE-OPEN Object Supporting Thermo 1.0
 public const string SupportsThermodynamics10_CATID = "{0D562DC8-EA8E-4210-AB39-B66513C0CD09}";
 // CAPE-OPEN Object Supporting Thermo 1.1
 public const string SupportsThermodynamics11_CATID = "{4667023A-5A8E-4cca-AB6D-9D78C5112FED}";
 
 /**************************************
  Identification Interfaces
 **************************************/
 public const string CapeValidationStatus_IID = "678c0b04-7d66-11d2-a67d-00105a42887f";
 public const string CapeIdentification_IID = "678c0990-7d66-11d2-a67d-00105a42887f";

 /**************************************
  Collection Interfaces
 **************************************/
 public const string ICapeCollection_IID = "678c099a-0093-11d2-a67d-00105a42887f";

 /**************************************
  Utilities Interfaces
 **************************************/
 public const string ICapeUtilities_IID = "678c0a9b-0100-11d2-a67d-00105a42887f";

 /*****************************************
 CAPE-OPEN Flowsheet monitoring interface
 *****************************************/
 public const string ICapeFlowsheetMonitoring_IID = "2CC8CC79-F854-4d65-B296-F8CD3344A2CD";

 /**************************************
  Parameter Interfaces
 **************************************/
 public const string ICapeParameterSpec_IID = "678c099c-0093-11d2-a67d-00105a42887f";
 public const string ICapeRealParameterSpec_IID = "678c099d-0093-11d2-a67d-00105a42887f";
 public const string ICapeIntegerParameterSpec_IID = "678c099e-0093-11d2-a67d-00105a42887f";
 public const string ICapeOptionParameterSpec_IID = "678c099f-0093-11d2-a67d-00105a42887f";
 public const string ICapeParameter_IID = "678c09a0-0093-11d2-a67d-00105a42887f";
 public const string ICapeBooleanParameterSpec_IID = "678c09a8-0093-11d2-a67d-00105a42887f";
 public const string ICapeArrayParameterSpec_IID = "678c09a9-0093-11d2-a67d-00105a42887f";
 public const string CapeParamType_IID = "678c0b02-7d66-11d2-a67d-00105a42887f";
 public const string CapeParamMode_IID = "678c0b03-7d66-11d2-a67d-00105a42887f";

 /**************************************
  Error Interfaces
 **************************************/
 public const string CapeErrorInterfaceHRs_IID = "678c0b01-7d66-11d2-a67d-00105a42887f";
 public const string ECapeRoot_IID = "678c0b10-7d66-11d2-a67d-00105a42887f";
 public const string ECapeUser_IID = "678C0B11-7D66-11D2-A67D-00105A42887F";
 public const string ECapeUnknown_IID = "678c0b12-7d66-11d2-a67d-00105a42887f";
 public const string ECapeData_IID = "678c0b13-7d66-11d2-a67d-00105a42887f";
 public const string ECapeLicenceError_IID = "678c0b14-7d66-11d2-a67d-00105a42887f";
 public const string ECapeBadCOParameter_IID = "678c0b15-7d66-11d2-a67d-00105a42887f";
 public const string ECapeBadArgument_IID = "E29E42B3-E481-45c6-A737-78F4A7FC0391";
 public const string ECapeInvalidArgument_IID = "678c0b17-7d66-11d2-a67d-00105a42887f";
 public const string ECapeOutOfBounds_IID = "678c0b18-7d66-11d2-a67d-00105a42887f";
 public const string ECapeImplementation_IID = "678c0b19-7d66-11d2-a67d-00105a42887f";
 public const string ECapeNoImpl_IID = "678c0b1a-7d66-11d2-a67d-00105a42887f";
 public const string ECapeLimitedImpl_IID = "678c0b1b-7d66-11d2-a67d-00105a42887f";
 public const string ECapeComputation_IID = "678c0b1c-7d66-11d2-a67d-00105a42887f";
 public const string ECapeOutOfResources_IID = "678c0b1d-7d66-11d2-a67d-00105a42887f";
 public const string ECapeNoMemory_IID = "678c0b1e-7d66-11d2-a67d-00105a42887f";
 public const string ECapeTimeOut_IID = "678c0b1f-7d66-11d2-a67d-00105a42887f";
 public const string ECapeFailedInitialisation_IID = "678c0b20-7d66-11d2-a67d-00105a42887f";
 public const string ECapeSolvingError_IID = "678c0b21-7d66-11d2-a67d-00105a42887f";
 public const string ECapeBadInvOrder_IID = "678c0b22-7d66-11d2-a67d-00105a42887f";
 public const string ECapeInvalidOperation_IID = "678c0b23-7d66-11d2-a67d-00105a42887f";
 public const string ECapePersistence_IID = "678c0b24-7d66-11d2-a67d-00105a42887f";
 public const string ECapeIllegalAccess_IID = "678c0b25-7d66-11d2-a67d-00105a42887f";
 public const string ECapePersistenceNotFound_IID = "678c0b26-7d66-11d2-a67d-00105a42887f";
 public const string ECapePersistenceSystemError_IID = "678c0b27-7d66-11d2-a67d-00105a42887f";
 public const string ECapePersistenceOverflow_IID = "678c0b28-7d66-11d2-a67d-00105a42887f";
 public const string ECapeBoundaries_IID = "678c0b29-7d66-11d2-a67d-00105a42887f";
 public const string ECapeErrorDummy_IID = "678c0b07-7d66-11d2-a67d-00105a42887f";

 /**************************************
  COSE Interfaces
 **************************************/
 public const string ICapeSimulationContext_IID = "678c0a9c-0100-11d2-a67d-00105a42887f";
 public const string ICapeDiagnostic_IID = "678c0a9d-0100-11d2-a67d-00105a42887f";
 public const string ICapeMaterialTemplateSystem_IID = "678c0a9e-0100-11d2-a67d-00105a42887f";
 public const string ICapeCOSEUtilities_IID = "678c0a9f-0100-11d2-a67d-00105a42887f";

 /**************************************
  Thermo Interfaces
 **************************************/
 public const string ICapeThermoCalculationRoutine_IID = "678c0991-7d66-11d2-a67d-00105a42887f";
 public const string ICapeThermoReliability_IID = "678c0992-7d66-11d2-a67d-00105a42887f";
 public const string ICapeThermoMaterialTemplate_IID = "678c0993-7d66-11d2-a67d-00105a42887f";
 public const string ICapeThermoMaterialObject_IID = "678c0994-7d66-11d2-a67d-00105a42887f";
 public const string ICapeThermoSystem_IID = "678c0995-7d66-11d2-a67d-00105a42887f";
 public const string ICapeThermoPropertyPackage_IID = "678c0996-7d66-11d2-a67d-00105a42887f";
 public const string ICapeThermoEquilibriumServer_IID = "678c0997-7d66-11d2-a67d-00105a42887f";
 // Example CLSID - not for use 
 public const string AspenTechThermoSystem_CLSID = "678c09a7-7d66-11d2-a67d-00105a42887f";

 /**************************************
  Solvers Interfaces
 **************************************/
 public const string ICapeNumericMatrix_IID = "3AD3C8F6-E6EC-4e63-B51E-0E9403535463";
 public const string ICapeNumericUnstructuredMatrix_IID = "678c09af-7d66-11d2-a67d-00105a42887f";
 public const string ICapeNumericFullMatrix_IID = "678c0b71-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericBandedMatrix_IID = "678c0b72-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericESOManager_IID = "678c0b73-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericESO_IID = "9304E044-3311-41ed-8766-0123CB44038A";
 public const string ICapeNumericLAESO_IID = "678c0b74-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericNLAESO_IID = "678c0b75-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericDAESO_IID = "678c0b76-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericGlobalESO_IID = "678c0b77-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericGlobalLAESO_IID = "678c0b78-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericGlobalNLAESO_IID = "678c0b79-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericGlobalDAESO_IID = "678c0b7a-0100-11d2-a67d-00105a42887f";

 public const string ICapeNumericModel_IID = "678c0b7c-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericContinuousModel_IID = "678c0b7d-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericHierarchicalModel_IID = "678c0b7e-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericAggregateModel_IID = "678c0b7f-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericSTN_IID = "678c0b80-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericEvent_IID = "678c0b81-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericBasicEvent_IID = "678c0b82-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericCompositeEvent_IID = "678c0b83-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericBinaryEvent_IID = "678c0b84-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericUnaryEvent_IID = "678c0b85-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericEventInfo_IID = "678c0b86-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericExternalEventInfo_IID = "678c0b87-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericInternalEventInfo_IID = "678c0b88-0100-11d2-a67d-00105a42887f";

 public const string ICapeNumericSolver_IID = "678c0b8a-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericLASolver_IID = "678c0b8b-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericNLASolver_IID = "678c0b8c-0100-11d2-a67d-00105a42887f";
 public const string ICapeNumericDAESolver_IID = "678c0b8d-0100-11d2-a67d-00105a42887f";

 /**************************************
  Unit Operation Interfaces
 **************************************/
 public const string ICapeUnit_IID = "678c0998-0100-11d2-a67d-00105a42887f";
 public const string ICapeUnitPort_IID = "678c0999-0093-11d2-a67d-00105a42887f";
 public const string ICapeUnitReport_IID = "678c099b-0093-11d2-a67d-00105a42887f";
 //public const String  ICapeUnitEdit_IID			"678c0a9a-0093-11d2-a67d-00105a42887f";
 //public const String  ICapeUnitCollection_IID	"678c099a-7d66-11d2-a67d-00105a42887f";
 //ICapeUnitPortVariables : new interface for mapping EO ESO to port variables
 public const string ICapeUnitPortVariables_IID = "678c09b1-7d66-11d2-a67d-00105a42887f";
 public const string CapePortDirection_IID = "678c0b05-7d66-11d2-a67d-00105a42887f";
 public const string CapePortType_IID = "678c0b06-7d66-11d2-a67d-00105a42887f";
 // Example CLSID - not for use !
 public const string HyprotechMixerSplitter_CLSID = "678c0a99-7d66-11d2-a67d-00105a42887f";

 /**************************************
  Reactions interfaces
 **************************************/
 public const string ICapeElectrolyteReactionContext_IID = "678c0afd-0100-11d2-a67d-00105a42887f";
 public const string ICapeKineticReactionContext_IID = "678c0afe-0100-11d2-a67d-00105a42887f";
 public const string ICapeReactionsPackageManager_IID = "678c0afc-0100-11d2-a67d-00105a42887f";
 public const string ICapeReactionChemistry_IID = "678c0afb-0100-11d2-a67d-00105a42887f";
 public const string ICapeReactionProperties_IID = "678c0afa-0100-11d2-a67d-00105a42887f";
 public const string ICapeReactionsRoutine_IID = "678c0af9-0100-11d2-a67d-00105a42887f";
 // ICapeThermoContext - actually part of the 1.1 thermo specification but 
 // included here because it is required for the Reactions interfaces
 public const string ICapeThermoContext_IID = "678c0b5f-0100-11d2-a67d-00105a42887f";
 public const string CapeReactionType_IID = "678c0b00-0100-11d2-a67d-00105a42887f";
 public const string CapeReactionRateBasis_IID = "678c0aff-0100-11d2-a67d-00105a42887f";

 /**************************************
  Petroleum Fractions interfaces
 **************************************/
 public const string ICapeThermoPetroFractions_IID = "678c0aa0-0100-11d2-a67d-00105a42887f";
 public const string ICapeUnitTypeInfo_IID = "678c0aa1-0100-11d2-a67d-00105a42887f";
 public const string CapeUnitType_IID = "678c0aa2-0100-11d2-a67d-00105a42887f";

 /**************************************
  SMST interfaces
 **************************************/
 public const string ICapeSMSTFlowsheetManager_IID = "678c0b65-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTFlowsheet_IID = "678c0b66-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTProcessGraph_IID = "678c0b67-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTPartitionGraph_IID = "678c0b68-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTOpenPartitionGraph_IID = "678c0b69-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTAnalysisManager_IID = "678c0b6a-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTAnalysis_IID = "678c0b6b-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTSequencing_IID = "678c0b6c-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTTearing_IID = "678c0b6d-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTPartitioning_IID = "678c0b6e-0100-11d2-a67d-00105a42887f";
 public const string ICapeSMSTSMAnalysis_IID = "678c0b70-0100-11d2-a67d-00105a42887f";
 public const string CapeFlowsheetType_IID = "678c0b60-0100-11d2-a67d-00105a42887f";
 public const string CapeSMSTStream_IID = "678c0b61-0100-11d2-a67d-00105a42887f";
 public const string CapeAnalysisType_IID = "678c0b62-0100-11d2-a67d-00105a42887f";
 public const string CapeConsistencyCode_IID = "678c0b63-0100-11d2-a67d-00105a42887f";
 public const string CapeConvergenceCode_IID = "678c0b64-0100-11d2-a67d-00105a42887f";

 /**************************************
  MINLP Interfaces
 **************************************/
 public const string ICapeMINLP_IID = "678c09cc-7d66-11d2-a67d-00105a42887f";
 public const string ICapeMINLPSystem_IID = "678c09cd-7d66-11d2-a67d-00105a42887f";
 public const string ICapeMINLPSolverManager_IID = "678c09ce-7d66-11d2-a67d-00105a42887f";
 public const string ECapeOutsideSolverScope_IID = "678c0b0f-7d66-11d2-a67d-00105a42887f";
 public const string ECapeHessianInfoNotAvailable_IID = "3FF0B24B-4299-4dac-A46E-7843728AD205";

 /**************************************
  PPDB interfaces
 **************************************/
 public const string ICapePpdb_IID = "678c09b2-7d66-11d2-a67d-00105a42887f";
 public const string ICapePpdbRegister_IID = "678c09b3-7d66-11d2-a67d-00105a42887f";
 public const string ICapePpdbTables_IID = "678c09b4-7d66-11d2-a67d-00105a42887f";
 public const string ICapePpdbModels_IID = "678c09b5-7d66-11d2-a67d-00105a42887f";
 public const string CapeSpecCompound_IID = "678c0b0c-7d66-11d2-a67d-00105a42887f";
 public const string CapeSpecStructure_IID = "678c0b0d-7d66-11d2-a67d-00105a42887f";
 public const string CapeSpecDictionary_IID = "678c0b0e-7d66-11d2-a67d-00105a42887f";

 /**************************************
  PSP interfaces
 **************************************/
 public const string ICapePSPCollection_IID = "78DEFBBD-ED69-4f81-90A4-6B636CE13164";
 public const string ICapePSPResource_IID = "7A4D266A-E54D-4a7d-8877-632E89344FED";
 public const string ICapePSPRecipeEntity_IID = "85E4C4E2-57FC-43aa-A39A-78D392947080";
 public const string ICapePSPSchedule_IID = "F3E9CF96-DF8F-40f3-9543-E8D17CABBF96";
 public const string ICapePSPScheduleEntry_IID = "638D84DC-84E9-4aea-83A4-0BB8852832E9";
 public const string ICapePSPTransaction_IID = "45A1B544-4BE6-43f9-83A4-4A7CFEE802FE";
 public const string ICapePSPResourceRequirement_IID = "8F3C13F5-0C69-42a2-9438-4299C630A0A4";
 public const string ICapePSPReport_IID = "1DD05FA1-EB10-4cbe-A94A-24F6EA7E7815";
 public const string ICapePSPResourceCollection_IID = "E09E0B56-6A51-496f-B796-2C45C549B510";
 public const string ICapePSPRecipeEntityCollection_IID = "B3245794-9A3E-4af7-A8CA-2308290106F0";
 public const string ICapePSPScheduleCollection_IID = "033AC3EF-7449-4113-A10E-D70161B3FC22";
 public const string ICapePSPScheduleEntryCollection_IID = "25BC241A-8110-4806-AF7A-CB6CFA7B9A57";
 public const string ICapePSPTransactionCollection_IID = "052DAFEF-0F43-4f4d-88AB-50F6AD8FC0EB";
 public const string ICapePSPResourceRequirementCollection_IID = "DFF34851-E60E-40dd-BAA1-4FEDE69B3467";
 public const string ICapePSP_IID = "F840ECA2-941B-4af7-84DB-47E2187430A2";
 public const string ICapePetroFractions_IID = "72A94DE9-9A69-4369-B508-C033CDFD4F81";
 public const string CapeCompoundType_IID = "8091E285-3CFA-4a41-A5C4-141D0D709D87";
}