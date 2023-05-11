#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <limits>


template <typename R, typename T1>
struct VirtualFuncInvoker1
{
	typedef R (*Func)(void*, T1, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};

// System.Func`2<TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece,System.Single>
struct Func_2_t3570D53917E5BD06C795912A590BCA0DED01D2FF;
// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>
struct ParametricCurve_1_t34524DBD3B3CF904763788213F038C49ABA03450;
// UnityEngine.Vector2[]
struct Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA;
// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D
struct Circle2D_tF5D4F6CCEB5D3150CBFDCFC805CC99720778450F;
// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Ray2D
struct Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB;
// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c
struct U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003;
// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece
struct Piece_t494F2879242051B6677FFC7BE046C41AF8F0AA3C;

IL2CPP_EXTERN_C RuntimeClass* Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA_il2cpp_TypeInfo_var;

struct Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA;

IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>
struct ParametricCurve_1_t34524DBD3B3CF904763788213F038C49ABA03450  : public RuntimeObject
{
};

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Compute
struct Compute_t5F5FB04540D5C8AEC38100713202FF9B0EE8B20D  : public RuntimeObject
{
};

// System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
// Native definition for P/Invoke marshalling of System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c
struct U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003  : public RuntimeObject
{
};

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece
struct Piece_t494F2879242051B6677FFC7BE046C41AF8F0AA3C  : public RuntimeObject
{
	// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2> TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece::Curve
	ParametricCurve_1_t34524DBD3B3CF904763788213F038C49ABA03450* ___Curve_0;
	// System.Single TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece::Length
	float ___Length_1;
};

// System.Double
struct Double_tE150EF3D1D43DEE85D533810AB4C742307EEDE5F 
{
	// System.Double System.Double::m_value
	double ___m_value_0;
};

// System.Single
struct Single_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C 
{
	// System.Single System.Single::m_value
	float ___m_value_0;
};

// UnityEngine.Vector2
struct Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 
{
	// System.Single UnityEngine.Vector2::x
	float ___x_0;
	// System.Single UnityEngine.Vector2::y
	float ___y_1;
};

// System.Void
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D
struct Circle2D_tF5D4F6CCEB5D3150CBFDCFC805CC99720778450F  : public ParametricCurve_1_t34524DBD3B3CF904763788213F038C49ABA03450
{
	// UnityEngine.Vector2 TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D::Center
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___Center_0;
	// System.Single TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D::Radius
	float ___Radius_1;
	// System.Single TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D::StartAngle
	float ___StartAngle_2;
	// System.Single TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D::TurnDirection
	float ___TurnDirection_3;
};

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Ray2D
struct Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB  : public ParametricCurve_1_t34524DBD3B3CF904763788213F038C49ABA03450
{
	// UnityEngine.Vector2 TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Ray2D::Origin
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___Origin_0;
	// UnityEngine.Vector2 TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Ray2D::Direction
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___Direction_1;
};

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Compute

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Compute

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c
struct U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003_StaticFields
{
	// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c::<>9
	U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003* ___U3CU3E9_0;
	// System.Func`2<TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece,System.Single> TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c::<>9__3_0
	Func_2_t3570D53917E5BD06C795912A590BCA0DED01D2FF* ___U3CU3E9__3_0_1;
};

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece

// System.Double

// System.Double

// System.Single

// System.Single

// UnityEngine.Vector2
struct Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7_StaticFields
{
	// UnityEngine.Vector2 UnityEngine.Vector2::zeroVector
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___zeroVector_2;
	// UnityEngine.Vector2 UnityEngine.Vector2::oneVector
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___oneVector_3;
	// UnityEngine.Vector2 UnityEngine.Vector2::upVector
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___upVector_4;
	// UnityEngine.Vector2 UnityEngine.Vector2::downVector
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___downVector_5;
	// UnityEngine.Vector2 UnityEngine.Vector2::leftVector
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___leftVector_6;
	// UnityEngine.Vector2 UnityEngine.Vector2::rightVector
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___rightVector_7;
	// UnityEngine.Vector2 UnityEngine.Vector2::positiveInfinityVector
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___positiveInfinityVector_8;
	// UnityEngine.Vector2 UnityEngine.Vector2::negativeInfinityVector
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___negativeInfinityVector_9;
};

// UnityEngine.Vector2

// System.Void

// System.Void

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Ray2D

// TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Ray2D
#ifdef __clang__
#pragma clang diagnostic pop
#endif
// UnityEngine.Vector2[]
struct Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA  : public RuntimeArray
{
	ALIGN_FIELD (8) Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 m_Items[1];

	inline Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 value)
	{
		m_Items[index] = value;
	}
};



// System.Void System.Object::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2 (RuntimeObject* __this, const RuntimeMethod* method) ;
// System.Void TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__ctor_mB0F8F6FC86F7B1BB6E9CEE9C0EDB6E201AB7F700 (U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003* __this, const RuntimeMethod* method) ;
// System.Void UnityEngine.Vector2::Set(System.Single,System.Single)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Vector2_Set_m42A76E817B65A9626E1F5E900EB67F037B3E1ED0_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* __this, float ___0_newX, float ___1_newY, const RuntimeMethod* method) ;
// System.Single UnityEngine.Vector2::Dot(UnityEngine.Vector2,UnityEngine.Vector2)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Vector2_Dot_mC1E68FDB4FB462A279A303C043B8FD0AC11C8458_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___0_lhs, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___1_rhs, const RuntimeMethod* method) ;
// UnityEngine.Vector2 UnityEngine.Vector2::op_Subtraction(UnityEngine.Vector2,UnityEngine.Vector2)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 Vector2_op_Subtraction_m44475FCDAD2DA2F98D78A6625EC2DCDFE8803837_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___0_a, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___1_b, const RuntimeMethod* method) ;
// System.Single UnityEngine.Vector2::Distance(UnityEngine.Vector2,UnityEngine.Vector2)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Vector2_Distance_mBACBB1609E1894D68F882D86A93519E311810C89_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___0_a, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___1_b, const RuntimeMethod* method) ;
// System.Void UnityEngine.Vector2::.ctor(System.Single,System.Single)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Vector2__ctor_m9525B79969AFFE3254B303A40997A56DEEB6F548_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* __this, float ___0_x, float ___1_y, const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece::.ctor(TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>,System.Single)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Piece__ctor_m5DBCD79D4A8F4C0EB91F9ADB1868209E4B48ABD6 (Piece_t494F2879242051B6677FFC7BE046C41AF8F0AA3C* __this, ParametricCurve_1_t34524DBD3B3CF904763788213F038C49ABA03450* ___0_curve, float ___1_length, const RuntimeMethod* method) 
{
	{
		// public Piece(ParametricCurve<Vector2> curve, float length)
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		// Curve = curve;
		ParametricCurve_1_t34524DBD3B3CF904763788213F038C49ABA03450* L_0 = ___0_curve;
		__this->___Curve_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___Curve_0), (void*)L_0);
		// Length = length;
		float L_1 = ___1_length;
		__this->___Length_1 = L_1;
		// }
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c::.cctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__cctor_m70ED76D0C143F67D51B2AB6F5658BF9D21A01DA8 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003* L_0 = (U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003*)il2cpp_codegen_object_new(U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		U3CU3Ec__ctor_mB0F8F6FC86F7B1BB6E9CEE9C0EDB6E201AB7F700(L_0, NULL);
		((U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003_il2cpp_TypeInfo_var))->___U3CU3E9_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003_il2cpp_TypeInfo_var))->___U3CU3E9_0), (void*)L_0);
		return;
	}
}
// System.Void TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__ctor_mB0F8F6FC86F7B1BB6E9CEE9C0EDB6E201AB7F700 (U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		return;
	}
}
// System.Single TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/<>c::<get_Length>b__3_0(TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Curve2DComposition/Piece)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float U3CU3Ec_U3Cget_LengthU3Eb__3_0_m3278E3708BF46BA260F5914C7E52523AAAF04EB8 (U3CU3Ec_t4DF515FB1F7FAC86E1DCF9DEC7ABDEDED5AC5003* __this, Piece_t494F2879242051B6677FFC7BE046C41AF8F0AA3C* ___0_x, const RuntimeMethod* method) 
{
	{
		// return Pieces.Sum(x => x.Length);
		Piece_t494F2879242051B6677FFC7BE046C41AF8F0AA3C* L_0 = ___0_x;
		NullCheck(L_0);
		float L_1 = L_0->___Length_1;
		return L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Single TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Compute::RotationDirection(UnityEngine.Vector2,UnityEngine.Vector2)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float Compute_RotationDirection_m5C6D3F7542FEA7A0E440101838060032826A92CE (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___0_from, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___1_to, const RuntimeMethod* method) 
{
	int32_t G_B3_0 = 0;
	{
		// from.Set(-from.y, from.x);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_0 = ___0_from;
		float L_1 = L_0.___y_1;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_2 = ___0_from;
		float L_3 = L_2.___x_0;
		Vector2_Set_m42A76E817B65A9626E1F5E900EB67F037B3E1ED0_inline((&___0_from), ((-L_1)), L_3, NULL);
		// return Vector2.Dot(from, to) >= 0 ? 1 : -1;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_4 = ___0_from;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_5 = ___1_to;
		float L_6;
		L_6 = Vector2_Dot_mC1E68FDB4FB462A279A303C043B8FD0AC11C8458_inline(L_4, L_5, NULL);
		if ((((float)L_6) >= ((float)(0.0f))))
		{
			goto IL_0025;
		}
	}
	{
		G_B3_0 = (-1);
		goto IL_0026;
	}

IL_0025:
	{
		G_B3_0 = 1;
	}

IL_0026:
	{
		return ((float)G_B3_0);
	}
}
// UnityEngine.Vector2[] TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Compute::Intersection(TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Ray2D,TowerOfHanoiPuzzle.TowerOfHanoiGeometry.Circle2D)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* Compute_Intersection_m9A64FF1D2D6FA0FC0B4224EF19A27DDC9264A301 (Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB* ___0_ray, Circle2D_tF5D4F6CCEB5D3150CBFDCFC805CC99720778450F* ___1_circle, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float V_4 = 0.0f;
	{
		// Vector2 AC = circle.Center - ray.Origin;
		Circle2D_tF5D4F6CCEB5D3150CBFDCFC805CC99720778450F* L_0 = ___1_circle;
		NullCheck(L_0);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_1 = L_0->___Center_0;
		Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB* L_2 = ___0_ray;
		NullCheck(L_2);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_3 = L_2->___Origin_0;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_4;
		L_4 = Vector2_op_Subtraction_m44475FCDAD2DA2F98D78A6625EC2DCDFE8803837_inline(L_1, L_3, NULL);
		// float length = Vector2.Dot(AC, ray.Direction);
		Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB* L_5 = ___0_ray;
		NullCheck(L_5);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_6 = L_5->___Direction_1;
		float L_7;
		L_7 = Vector2_Dot_mC1E68FDB4FB462A279A303C043B8FD0AC11C8458_inline(L_4, L_6, NULL);
		V_0 = L_7;
		// Vector2 D = ray.GetPosition(length);
		Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB* L_8 = ___0_ray;
		float L_9 = V_0;
		NullCheck(L_8);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_10;
		L_10 = VirtualFuncInvoker1< Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7, float >::Invoke(4 /* Out TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>::GetPosition(System.Single) */, L_8, L_9);
		// float distanceCD = Vector2.Distance(D, circle.Center);
		Circle2D_tF5D4F6CCEB5D3150CBFDCFC805CC99720778450F* L_11 = ___1_circle;
		NullCheck(L_11);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_12 = L_11->___Center_0;
		float L_13;
		L_13 = Vector2_Distance_mBACBB1609E1894D68F882D86A93519E311810C89_inline(L_10, L_12, NULL);
		V_1 = L_13;
		// if (distanceCD > circle.Radius)
		float L_14 = V_1;
		Circle2D_tF5D4F6CCEB5D3150CBFDCFC805CC99720778450F* L_15 = ___1_circle;
		NullCheck(L_15);
		float L_16 = L_15->___Radius_1;
		if ((!(((float)L_14) > ((float)L_16))))
		{
			goto IL_0040;
		}
	}
	{
		// return new Vector2[0];
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_17 = (Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)SZArrayNew(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA_il2cpp_TypeInfo_var, (uint32_t)0);
		return L_17;
	}

IL_0040:
	{
		// float deltaLength = Mathf.Sqrt(circle.Radius * circle.Radius - distanceCD * distanceCD);
		Circle2D_tF5D4F6CCEB5D3150CBFDCFC805CC99720778450F* L_18 = ___1_circle;
		NullCheck(L_18);
		float L_19 = L_18->___Radius_1;
		Circle2D_tF5D4F6CCEB5D3150CBFDCFC805CC99720778450F* L_20 = ___1_circle;
		NullCheck(L_20);
		float L_21 = L_20->___Radius_1;
		float L_22 = V_1;
		float L_23 = V_1;
		float L_24;
		L_24 = sqrtf(((float)il2cpp_codegen_subtract(((float)il2cpp_codegen_multiply(L_19, L_21)), ((float)il2cpp_codegen_multiply(L_22, L_23)))));
		V_2 = L_24;
		// float l1 = length - deltaLength,
		float L_25 = V_0;
		float L_26 = V_2;
		V_3 = ((float)il2cpp_codegen_subtract(L_25, L_26));
		// l2 = length + deltaLength;
		float L_27 = V_0;
		float L_28 = V_2;
		V_4 = ((float)il2cpp_codegen_add(L_27, L_28));
		// if (l1 <= 0)
		float L_29 = V_3;
		if ((!(((float)L_29) <= ((float)(0.0f)))))
		{
			goto IL_008e;
		}
	}
	{
		// if (l2 <= 0)
		float L_30 = V_4;
		if ((!(((float)L_30) <= ((float)(0.0f)))))
		{
			goto IL_0078;
		}
	}
	{
		// return new Vector2[0];
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_31 = (Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)SZArrayNew(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA_il2cpp_TypeInfo_var, (uint32_t)0);
		return L_31;
	}

IL_0078:
	{
		// return new Vector2[1] { ray.GetPosition(l2) };
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_32 = (Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)SZArrayNew(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA_il2cpp_TypeInfo_var, (uint32_t)1);
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_33 = L_32;
		Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB* L_34 = ___0_ray;
		float L_35 = V_4;
		NullCheck(L_34);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_36;
		L_36 = VirtualFuncInvoker1< Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7, float >::Invoke(4 /* Out TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>::GetPosition(System.Single) */, L_34, L_35);
		NullCheck(L_33);
		(L_33)->SetAt(static_cast<il2cpp_array_size_t>(0), (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7)L_36);
		return L_33;
	}

IL_008e:
	{
		// if (l2 <= 0)
		float L_37 = V_4;
		if ((!(((float)L_37) <= ((float)(0.0f)))))
		{
			goto IL_00ac;
		}
	}
	{
		// return new Vector2[1] { ray.GetPosition(l1) };
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_38 = (Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)SZArrayNew(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA_il2cpp_TypeInfo_var, (uint32_t)1);
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_39 = L_38;
		Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB* L_40 = ___0_ray;
		float L_41 = V_3;
		NullCheck(L_40);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_42;
		L_42 = VirtualFuncInvoker1< Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7, float >::Invoke(4 /* Out TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>::GetPosition(System.Single) */, L_40, L_41);
		NullCheck(L_39);
		(L_39)->SetAt(static_cast<il2cpp_array_size_t>(0), (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7)L_42);
		return L_39;
	}

IL_00ac:
	{
		// return new Vector2[2] { ray.GetPosition(l1), ray.GetPosition(l2) };
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_43 = (Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA*)SZArrayNew(Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA_il2cpp_TypeInfo_var, (uint32_t)2);
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_44 = L_43;
		Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB* L_45 = ___0_ray;
		float L_46 = V_3;
		NullCheck(L_45);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_47;
		L_47 = VirtualFuncInvoker1< Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7, float >::Invoke(4 /* Out TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>::GetPosition(System.Single) */, L_45, L_46);
		NullCheck(L_44);
		(L_44)->SetAt(static_cast<il2cpp_array_size_t>(0), (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7)L_47);
		Vector2U5BU5D_tFEBBC94BCC6C9C88277BA04047D2B3FDB6ED7FDA* L_48 = L_44;
		Ray2D_tC711F1F2902D0A71EB7A314810CA996C00FB74EB* L_49 = ___0_ray;
		float L_50 = V_4;
		NullCheck(L_49);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_51;
		L_51 = VirtualFuncInvoker1< Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7, float >::Invoke(4 /* Out TowerOfHanoiPuzzle.TowerOfHanoiGeometry.ParametricCurve`1<UnityEngine.Vector2>::GetPosition(System.Single) */, L_49, L_50);
		NullCheck(L_48);
		(L_48)->SetAt(static_cast<il2cpp_array_size_t>(1), (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7)L_51);
		return L_48;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Vector2_Set_m42A76E817B65A9626E1F5E900EB67F037B3E1ED0_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* __this, float ___0_newX, float ___1_newY, const RuntimeMethod* method) 
{
	{
		float L_0 = ___0_newX;
		__this->___x_0 = L_0;
		float L_1 = ___1_newY;
		__this->___y_1 = L_1;
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Vector2_Dot_mC1E68FDB4FB462A279A303C043B8FD0AC11C8458_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___0_lhs, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___1_rhs, const RuntimeMethod* method) 
{
	float V_0 = 0.0f;
	{
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_0 = ___0_lhs;
		float L_1 = L_0.___x_0;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_2 = ___1_rhs;
		float L_3 = L_2.___x_0;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_4 = ___0_lhs;
		float L_5 = L_4.___y_1;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_6 = ___1_rhs;
		float L_7 = L_6.___y_1;
		V_0 = ((float)il2cpp_codegen_add(((float)il2cpp_codegen_multiply(L_1, L_3)), ((float)il2cpp_codegen_multiply(L_5, L_7))));
		goto IL_001f;
	}

IL_001f:
	{
		float L_8 = V_0;
		return L_8;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 Vector2_op_Subtraction_m44475FCDAD2DA2F98D78A6625EC2DCDFE8803837_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___0_a, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___1_b, const RuntimeMethod* method) 
{
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 V_0;
	memset((&V_0), 0, sizeof(V_0));
	{
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_0 = ___0_a;
		float L_1 = L_0.___x_0;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_2 = ___1_b;
		float L_3 = L_2.___x_0;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_4 = ___0_a;
		float L_5 = L_4.___y_1;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_6 = ___1_b;
		float L_7 = L_6.___y_1;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_8;
		memset((&L_8), 0, sizeof(L_8));
		Vector2__ctor_m9525B79969AFFE3254B303A40997A56DEEB6F548_inline((&L_8), ((float)il2cpp_codegen_subtract(L_1, L_3)), ((float)il2cpp_codegen_subtract(L_5, L_7)), /*hidden argument*/NULL);
		V_0 = L_8;
		goto IL_0023;
	}

IL_0023:
	{
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_9 = V_0;
		return L_9;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Vector2_Distance_mBACBB1609E1894D68F882D86A93519E311810C89_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___0_a, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___1_b, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	{
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_0 = ___0_a;
		float L_1 = L_0.___x_0;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_2 = ___1_b;
		float L_3 = L_2.___x_0;
		V_0 = ((float)il2cpp_codegen_subtract(L_1, L_3));
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_4 = ___0_a;
		float L_5 = L_4.___y_1;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_6 = ___1_b;
		float L_7 = L_6.___y_1;
		V_1 = ((float)il2cpp_codegen_subtract(L_5, L_7));
		float L_8 = V_0;
		float L_9 = V_0;
		float L_10 = V_1;
		float L_11 = V_1;
		il2cpp_codegen_runtime_class_init_inline(Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		double L_12;
		L_12 = sqrt(((double)((float)il2cpp_codegen_add(((float)il2cpp_codegen_multiply(L_8, L_9)), ((float)il2cpp_codegen_multiply(L_10, L_11))))));
		V_2 = ((float)L_12);
		goto IL_002e;
	}

IL_002e:
	{
		float L_13 = V_2;
		return L_13;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Vector2__ctor_m9525B79969AFFE3254B303A40997A56DEEB6F548_inline (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* __this, float ___0_x, float ___1_y, const RuntimeMethod* method) 
{
	{
		float L_0 = ___0_x;
		__this->___x_0 = L_0;
		float L_1 = ___1_y;
		__this->___y_1 = L_1;
		return;
	}
}
