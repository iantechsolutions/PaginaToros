#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
194,
201,
202,
203,
204,
205,
206,
207,
208,
209,
212,
213,
380,
381,
383,
413,
414,
415,
435,
436,
437,
438,
439,
530,
531,
532,
535,
582,
583,
584,
587,
589,
591,
593,
598,
606,
607,
608,
609,
610,
611,
612,
613,
614,
615,
616,
617,
618,
619,
620,
621,
622,
623,
624,
625,
626,
627,
628,
629,
630,
631,
632,
719,
720,
721,
722,
723,
724,
725,
726,
727,
728,
729,
730,
731,
732,
733,
734,
735,
736,
737,
738,
739,
740,
741,
742,
743,
877,
878,
886,
889,
891,
897,
898,
900,
901,
905,
907,
908,
909,
910,
912,
913,
914,
917,
918,
921,
922,
923,
999,
1000,
1002,
1010,
1011,
1012,
1013,
1014,
1018,
1019,
1020,
1021,
1022,
1023,
1025,
1026,
1027,
1029,
1030,
1032,
1036,
1037,
1038,
1311,
1528,
1529,
7738,
7739,
7741,
7742,
7743,
7744,
7745,
7747,
7749,
7751,
7752,
7753,
7761,
7763,
7770,
7771,
7773,
7775,
7777,
7788,
7797,
7798,
7800,
7801,
7802,
7803,
7804,
7806,
7808,
8903,
8907,
8909,
8910,
8911,
8912,
8963,
8964,
8965,
8966,
8985,
8986,
8987,
8988,
8990,
8991,
9032,
9086,
9089,
9098,
9099,
9100,
9101,
9503,
9504,
9509,
9510,
9511,
9563,
9564,
9565,
9594,
9600,
9607,
9617,
9621,
9713,
9723,
9724,
9737,
9738,
9739,
9740,
9741,
9742,
9743,
9750,
9766,
9787,
9788,
9797,
9799,
9806,
9807,
9810,
9812,
9817,
9823,
9824,
9831,
9833,
9843,
9846,
9847,
9848,
9859,
9871,
9877,
9878,
9879,
9881,
9882,
9892,
9911,
9933,
9934,
9935,
9936,
9937,
9954,
9959,
9964,
9995,
9996,
10507,
10508,
10594,
10683,
10993,
10994,
11006,
11007,
11008,
11014,
11098,
11284,
11285,
12252,
12253,
13087,
13106,
13113,
13114,
13116,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementType_raw (int,int);
int ves_icall_System_Array_IsValueOfElementType_raw (int,int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy_raw (int,int,int,int,int,int);
int ves_icall_System_Array_GetLength_raw (int,int,int);
int ves_icall_System_Array_GetLowerBound_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
int ves_icall_System_Array_GetValueImpl_raw (int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
int ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
int ves_icall_System_Enum_ToObject_raw (int,int64_t,int);
int ves_icall_System_Enum_InternalGetCorElementType_raw (int,int);
int ves_icall_System_Enum_get_underlying_type_raw (int,int);
int ves_icall_System_Enum_InternalHasFlag_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Abs_double (double);
float ves_icall_System_Math_Abs_single (float);
double ves_icall_System_Math_Acos (double);
double ves_icall_System_Math_Acosh (double);
double ves_icall_System_Math_Asin (double);
double ves_icall_System_Math_Asinh (double);
double ves_icall_System_Math_Atan (double);
double ves_icall_System_Math_Atan2 (double,double);
double ves_icall_System_Math_Atanh (double);
double ves_icall_System_Math_Cbrt (double);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Cosh (double);
double ves_icall_System_Math_Exp (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Log10 (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sinh (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_Tanh (double);
double ves_icall_System_Math_FusedMultiplyAdd (double,double,double);
int ves_icall_System_Math_ILogB (double);
double ves_icall_System_Math_Log2 (double);
double ves_icall_System_Math_ModF (double,int);
float ves_icall_System_MathF_Acos (float);
float ves_icall_System_MathF_Acosh (float);
float ves_icall_System_MathF_Asin (float);
float ves_icall_System_MathF_Asinh (float);
float ves_icall_System_MathF_Atan (float);
float ves_icall_System_MathF_Atan2 (float,float);
float ves_icall_System_MathF_Atanh (float);
float ves_icall_System_MathF_Cbrt (float);
float ves_icall_System_MathF_Ceiling (float);
float ves_icall_System_MathF_Cos (float);
float ves_icall_System_MathF_Cosh (float);
float ves_icall_System_MathF_Exp (float);
float ves_icall_System_MathF_Floor (float);
float ves_icall_System_MathF_Log (float);
float ves_icall_System_MathF_Log10 (float);
float ves_icall_System_MathF_Pow (float,float);
float ves_icall_System_MathF_Sin (float);
float ves_icall_System_MathF_Sinh (float);
float ves_icall_System_MathF_Sqrt (float);
float ves_icall_System_MathF_Tan (float);
float ves_icall_System_MathF_Tanh (float);
float ves_icall_System_MathF_FusedMultiplyAdd (float,float,float);
int ves_icall_System_MathF_ILogB (float);
float ves_icall_System_MathF_Log2 (float);
float ves_icall_System_MathF_ModF (float,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
int ves_icall_RuntimeType_make_array_type_raw (int,int,int);
int ves_icall_RuntimeType_make_byref_type_raw (int,int);
int ves_icall_RuntimeType_MakePointerType_raw (int,int);
int ves_icall_RuntimeType_MakeGenericType_raw (int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
void ves_icall_RuntimeType_GetPacking_raw (int,int,int,int);
int ves_icall_System_Activator_CreateInstanceInternal_raw (int,int);
int ves_icall_RuntimeType_get_DeclaringMethod_raw (int,int);
int ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericArguments_raw (int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition_raw (int,int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetInterfaces_raw (int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_get_DeclaringType_raw (int,int);
int ves_icall_RuntimeType_get_Name_raw (int,int);
int ves_icall_RuntimeType_get_Namespace_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType_raw (int,int);
int ves_icall_RuntimeTypeHandle_HasInstantiation_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsComObject_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetModule_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsGenericVariable_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of (int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
int ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_String_InternalIsInterned_raw (int,int);
int ves_icall_System_String_InternalIntern_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Read_Long (int);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
int64_t ves_icall_System_Threading_Interlocked_Add_Long (int,int64_t);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_test_synchronised_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
int ves_icall_System_Threading_Thread_GetCurrentProcessorNumber_raw (int);
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_IsPinnableType_raw (int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetDelegateForFunctionPointerInternal_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw (int,int,int,int,int,int);
int mono_object_hash_icall_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw (int,int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_InternalGetAssemblyName_raw (int,int,int,int);
void mono_digest_get_public_token (int,int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_System_Reflection_AssemblyName_ParseAssemblyName (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_get_EntryPoint_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_get_location_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_get_code_base_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_get_fullname_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_InternalImageRuntimeVersion_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
int ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw (int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_GetMDStreamVersion_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw (int,int);
void ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw (int,int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw (int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
int ves_icall_CustomAttributeBuilder_GetBlob_raw (int,int,int,int,int,int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
int ves_icall_System_Diagnostics_Debugger_IsLogging ();
void ves_icall_System_Diagnostics_Debugger_Log (int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
void ves_icall_Mono_RuntimeMarshal_FreeAssemblyName (int,int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 194,
ves_icall_System_Array_InternalCreate,
// token 201,
ves_icall_System_Array_GetCorElementTypeOfElementType_raw,
// token 202,
ves_icall_System_Array_IsValueOfElementType_raw,
// token 203,
ves_icall_System_Array_CanChangePrimitive,
// token 204,
ves_icall_System_Array_FastCopy_raw,
// token 205,
ves_icall_System_Array_GetLength_raw,
// token 206,
ves_icall_System_Array_GetLowerBound_raw,
// token 207,
ves_icall_System_Array_GetGenericValue_icall,
// token 208,
ves_icall_System_Array_GetValueImpl_raw,
// token 209,
ves_icall_System_Array_SetGenericValue_icall,
// token 212,
ves_icall_System_Array_SetValueImpl_raw,
// token 213,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 380,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 381,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 383,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 413,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 414,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 415,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 435,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 436,
ves_icall_System_Enum_ToObject_raw,
// token 437,
ves_icall_System_Enum_InternalGetCorElementType_raw,
// token 438,
ves_icall_System_Enum_get_underlying_type_raw,
// token 439,
ves_icall_System_Enum_InternalHasFlag_raw,
// token 530,
ves_icall_System_Environment_get_ProcessorCount,
// token 531,
ves_icall_System_Environment_get_TickCount,
// token 532,
ves_icall_System_Environment_get_TickCount64,
// token 535,
ves_icall_System_Environment_FailFast_raw,
// token 582,
ves_icall_System_GC_GetCollectionCount,
// token 583,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 584,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 587,
ves_icall_System_GC_SuppressFinalize_raw,
// token 589,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 591,
ves_icall_System_GC_GetGCMemoryInfo,
// token 593,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 598,
ves_icall_System_Object_MemberwiseClone_raw,
// token 606,
ves_icall_System_Math_Abs_double,
// token 607,
ves_icall_System_Math_Abs_single,
// token 608,
ves_icall_System_Math_Acos,
// token 609,
ves_icall_System_Math_Acosh,
// token 610,
ves_icall_System_Math_Asin,
// token 611,
ves_icall_System_Math_Asinh,
// token 612,
ves_icall_System_Math_Atan,
// token 613,
ves_icall_System_Math_Atan2,
// token 614,
ves_icall_System_Math_Atanh,
// token 615,
ves_icall_System_Math_Cbrt,
// token 616,
ves_icall_System_Math_Ceiling,
// token 617,
ves_icall_System_Math_Cos,
// token 618,
ves_icall_System_Math_Cosh,
// token 619,
ves_icall_System_Math_Exp,
// token 620,
ves_icall_System_Math_Floor,
// token 621,
ves_icall_System_Math_Log,
// token 622,
ves_icall_System_Math_Log10,
// token 623,
ves_icall_System_Math_Pow,
// token 624,
ves_icall_System_Math_Sin,
// token 625,
ves_icall_System_Math_Sinh,
// token 626,
ves_icall_System_Math_Sqrt,
// token 627,
ves_icall_System_Math_Tan,
// token 628,
ves_icall_System_Math_Tanh,
// token 629,
ves_icall_System_Math_FusedMultiplyAdd,
// token 630,
ves_icall_System_Math_ILogB,
// token 631,
ves_icall_System_Math_Log2,
// token 632,
ves_icall_System_Math_ModF,
// token 719,
ves_icall_System_MathF_Acos,
// token 720,
ves_icall_System_MathF_Acosh,
// token 721,
ves_icall_System_MathF_Asin,
// token 722,
ves_icall_System_MathF_Asinh,
// token 723,
ves_icall_System_MathF_Atan,
// token 724,
ves_icall_System_MathF_Atan2,
// token 725,
ves_icall_System_MathF_Atanh,
// token 726,
ves_icall_System_MathF_Cbrt,
// token 727,
ves_icall_System_MathF_Ceiling,
// token 728,
ves_icall_System_MathF_Cos,
// token 729,
ves_icall_System_MathF_Cosh,
// token 730,
ves_icall_System_MathF_Exp,
// token 731,
ves_icall_System_MathF_Floor,
// token 732,
ves_icall_System_MathF_Log,
// token 733,
ves_icall_System_MathF_Log10,
// token 734,
ves_icall_System_MathF_Pow,
// token 735,
ves_icall_System_MathF_Sin,
// token 736,
ves_icall_System_MathF_Sinh,
// token 737,
ves_icall_System_MathF_Sqrt,
// token 738,
ves_icall_System_MathF_Tan,
// token 739,
ves_icall_System_MathF_Tanh,
// token 740,
ves_icall_System_MathF_FusedMultiplyAdd,
// token 741,
ves_icall_System_MathF_ILogB,
// token 742,
ves_icall_System_MathF_Log2,
// token 743,
ves_icall_System_MathF_ModF,
// token 877,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 878,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 886,
ves_icall_RuntimeType_make_array_type_raw,
// token 889,
ves_icall_RuntimeType_make_byref_type_raw,
// token 891,
ves_icall_RuntimeType_MakePointerType_raw,
// token 897,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 898,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 900,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 901,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 905,
ves_icall_RuntimeType_GetPacking_raw,
// token 907,
ves_icall_System_Activator_CreateInstanceInternal_raw,
// token 908,
ves_icall_RuntimeType_get_DeclaringMethod_raw,
// token 909,
ves_icall_System_RuntimeType_getFullName_raw,
// token 910,
ves_icall_RuntimeType_GetGenericArguments_raw,
// token 912,
ves_icall_RuntimeType_GetGenericParameterPosition_raw,
// token 913,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 914,
ves_icall_RuntimeType_GetFields_native_raw,
// token 917,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 918,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 921,
ves_icall_RuntimeType_get_DeclaringType_raw,
// token 922,
ves_icall_RuntimeType_get_Name_raw,
// token 923,
ves_icall_RuntimeType_get_Namespace_raw,
// token 999,
ves_icall_RuntimeTypeHandle_GetAttributes_raw,
// token 1000,
ves_icall_reflection_get_token_raw,
// token 1002,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 1010,
ves_icall_RuntimeTypeHandle_GetCorElementType_raw,
// token 1011,
ves_icall_RuntimeTypeHandle_HasInstantiation_raw,
// token 1012,
ves_icall_RuntimeTypeHandle_IsComObject_raw,
// token 1013,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 1014,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 1018,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 1019,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 1020,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 1021,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 1022,
ves_icall_RuntimeTypeHandle_IsGenericVariable_raw,
// token 1023,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 1025,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 1026,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition_raw,
// token 1027,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 1029,
ves_icall_RuntimeTypeHandle_is_subclass_of,
// token 1030,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 1032,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 1036,
ves_icall_System_String_FastAllocateString_raw,
// token 1037,
ves_icall_System_String_InternalIsInterned_raw,
// token 1038,
ves_icall_System_String_InternalIntern_raw,
// token 1311,
ves_icall_System_Type_internal_from_handle_raw,
// token 1528,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1529,
ves_icall_System_ValueType_Equals_raw,
// token 7738,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 7739,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 7741,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 7742,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 7743,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 7744,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 7745,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 7747,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 7749,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 7751,
ves_icall_System_Threading_Interlocked_Read_Long,
// token 7752,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 7753,
ves_icall_System_Threading_Interlocked_Add_Long,
// token 7761,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 7763,
mono_monitor_exit_icall_raw,
// token 7770,
ves_icall_System_Threading_Monitor_Monitor_test_synchronised_raw,
// token 7771,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 7773,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 7775,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 7777,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 7788,
ves_icall_System_Threading_Thread_GetCurrentProcessorNumber_raw,
// token 7797,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 7798,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 7800,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 7801,
ves_icall_System_Threading_Thread_GetState_raw,
// token 7802,
ves_icall_System_Threading_Thread_SetState_raw,
// token 7803,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 7804,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 7806,
ves_icall_System_Threading_Thread_YieldInternal,
// token 7808,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 8903,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 8907,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 8909,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 8910,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 8911,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 8912,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 8963,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 8964,
ves_icall_System_GCHandle_InternalFree_raw,
// token 8965,
ves_icall_System_GCHandle_InternalGet_raw,
// token 8966,
ves_icall_System_GCHandle_InternalSet_raw,
// token 8985,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 8986,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 8987,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 8988,
ves_icall_System_Runtime_InteropServices_Marshal_IsPinnableType_raw,
// token 8990,
ves_icall_System_Runtime_InteropServices_Marshal_GetDelegateForFunctionPointerInternal_raw,
// token 8991,
ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw,
// token 9032,
ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw,
// token 9086,
mono_object_hash_icall_raw,
// token 9089,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 9098,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 9099,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 9100,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw,
// token 9101,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 9503,
ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw,
// token 9504,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 9509,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 9510,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 9511,
ves_icall_System_Reflection_Assembly_InternalGetAssemblyName_raw,
// token 9563,
mono_digest_get_public_token,
// token 9564,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 9565,
ves_icall_System_Reflection_AssemblyName_ParseAssemblyName,
// token 9594,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 9600,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 9607,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 9617,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 9621,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 9713,
ves_icall_System_Reflection_RuntimeAssembly_get_EntryPoint_raw,
// token 9723,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 9724,
ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw,
// token 9737,
ves_icall_System_Reflection_RuntimeAssembly_get_location_raw,
// token 9738,
ves_icall_System_Reflection_RuntimeAssembly_get_code_base_raw,
// token 9739,
ves_icall_System_Reflection_RuntimeAssembly_get_fullname_raw,
// token 9740,
ves_icall_System_Reflection_RuntimeAssembly_InternalImageRuntimeVersion_raw,
// token 9741,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 9742,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 9743,
ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw,
// token 9750,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 9766,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 9787,
ves_icall_reflection_get_token_raw,
// token 9788,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 9797,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 9799,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 9806,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 9807,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 9810,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 9812,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 9817,
ves_icall_reflection_get_token_raw,
// token 9823,
ves_icall_get_method_info_raw,
// token 9824,
ves_icall_get_method_attributes,
// token 9831,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 9833,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 9843,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 9846,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 9847,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 9848,
ves_icall_reflection_get_token_raw,
// token 9859,
ves_icall_InternalInvoke_raw,
// token 9871,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 9877,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 9878,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 9879,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 9881,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 9882,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 9892,
ves_icall_InternalInvoke_raw,
// token 9911,
ves_icall_reflection_get_token_raw,
// token 9933,
ves_icall_reflection_get_token_raw,
// token 9934,
ves_icall_System_Reflection_RuntimeModule_GetMDStreamVersion_raw,
// token 9935,
ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw,
// token 9936,
ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw,
// token 9937,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 9954,
ves_icall_reflection_get_token_raw,
// token 9959,
ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw,
// token 9964,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 9995,
ves_icall_reflection_get_token_raw,
// token 9996,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 10507,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 10508,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 10594,
ves_icall_CustomAttributeBuilder_GetBlob_raw,
// token 10683,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 10993,
ves_icall_ModuleBuilder_basic_init_raw,
// token 10994,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 11006,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 11007,
ves_icall_ModuleBuilder_getToken_raw,
// token 11008,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 11014,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 11098,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 11284,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 11285,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 12252,
ves_icall_System_Diagnostics_Debugger_IsLogging,
// token 12253,
ves_icall_System_Diagnostics_Debugger_Log,
// token 13087,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 13106,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 13113,
ves_icall_Mono_RuntimeMarshal_FreeAssemblyName,
// token 13114,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 13116,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_handles [] = {
0,
1,
1,
0,
1,
1,
1,
0,
1,
0,
1,
1,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
0,
1,
0,
1,
1,
1,
1,
0,
1,
1,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
0,
0,
0,
0,
0,
};
