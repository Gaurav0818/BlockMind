// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/Triplanar"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Overlay("Overlay", 2D) = "white" {}
		_DirtAmount("DirtAmount", Range(0.5 , 1.2)) = 1
		_FallOff("FallOff", Float) = 0
		_Tiling("Tiling", Float) = 0
		_Normal("Normal", 2D) = "bump" {}
		[HideInInspector] _texcoord("", 2D) = "white" {}
		[HideInInspector] __dirty("", Int) = 1
	}

		SubShader
		{
			Tags{ "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline"  "Queue" = "Geometry+0"  }
			Cull Back
			HLSLPROGRAM

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			//#include "UnityPBSLighting.cginc"
			//#include "Lighting.cginc"
			#pragma target 3.0
			#define ASE_TEXTURE_PARAMS(textureName) textureName

			#ifdef UNITY_PASS_SHADOWCASTER
				#undef INTERNAL_DATA
				#undef WorldReflectionVector
				#undef WorldNormalVector
				#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
				#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
				#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
			#endif
			struct Input
			{
				float2 uv_texcoord;
				float3 worldPos;
				float3 worldNormal;
				INTERNAL_DATA
			};

			CBUFFER_START(UnityPerMaterial)

			uniform sampler2D _Normal;
			uniform float4 _Normal_ST;
			uniform sampler2D _Texture;
			uniform float4 _Texture_ST;
			uniform sampler2D _Overlay;
			uniform float _Tiling;
			uniform float _FallOff;
			uniform float _DirtAmount;
			
			CBUFFER_END


			inline float4 TriplanarSamplingCF(sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index)
			{
				float3 projNormal = (pow(abs(worldNormal), falloff));
				projNormal /= (projNormal.x + projNormal.y + projNormal.z) + 0.00001;
				float3 nsign = sign(worldNormal);
				float negProjNormalY = max(0, projNormal.y * -nsign.y);
				projNormal.y = max(0, projNormal.y * nsign.y);
				half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
				xNorm = (tex2D(ASE_TEXTURE_PARAMS(midTexMap), tiling * worldPos.zy * float2(nsign.x, 1.0)));
				yNorm = (tex2D(ASE_TEXTURE_PARAMS(topTexMap), tiling * worldPos.xz * float2(nsign.y, 1.0)));
				yNormN = (tex2D(ASE_TEXTURE_PARAMS(botTexMap), tiling * worldPos.xz * float2(nsign.y, 1.0)));
				zNorm = (tex2D(ASE_TEXTURE_PARAMS(midTexMap), tiling * worldPos.xy * float2(-nsign.z, 1.0)));
				return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
			}


			void surf(Input i , inout SurfaceOutputStandard o)
			{
				float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
				o.Normal = UnpackNormal(tex2D(_Normal, uv_Normal));
				float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
				float3 ase_worldPos = i.worldPos;
				float3 ase_worldNormal = WorldNormalVector(i, float3(0, 0, 1));
				float4 triplanar1 = TriplanarSamplingCF(_Overlay, _Overlay, _Overlay, ase_worldPos, ase_worldNormal, _FallOff, _Tiling, float3(1,1,1), float3(0,0,0));
				float4 temp_cast_0 = (_DirtAmount).xxxx;
				float4 clampResult17 = clamp(triplanar1 , temp_cast_0 , float4(1,1,1,0));
				o.Albedo = (tex2D(_Texture, uv_Texture) * clampResult17).rgb;
				o.Alpha = 1;
			}

			ENDHLSL


				/*
			HLSLPROGRAM
			#pragma surface surf Standard keepalpha fullforwardshadows

			ENDHLSL */
			Pass
			{
				Name "ShadowCaster"
				Tags{ "RenderPipeline" = "UniversalRenderPipeline" "LightMode" = "ShadowCaster"  }
				ZWrite On
				HLSLPROGRAM



				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma multi_compile_shadowcaster
				#pragma multi_compile UNITY_PASS_SHADOWCASTER
				#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
				#include "HLSLSupport.cginc"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

				#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
					#define CAN_SKIP_VPOS
				#endif
				//#include "UnityCG.cginc"
				//#include "Lighting.cginc"
				//#include "UnityPBSLighting.cginc"
				struct Attributes {
					float4 position   : POSITION;
					float2 uv0          : TEXCOORD0;
					float2 uv1          : TEXCOORD1;
					float2 uv2          : TEXCOORD2;
					float4 color		: COLOR;
    				float4 vertex : POSITION;
					float4 normal : NORMAL;
					float4 tangent : TANGENT;
				};
				struct v2f
				{
					//V2F_SHADOW_CASTER;
					float2 customPack1 : TEXCOORD1;
					float4 tSpace0 : TEXCOORD2;
					float4 tSpace1 : TEXCOORD3;
					float4 tSpace2 : TEXCOORD4;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};
				v2f vert(Attributes v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					//Input customInputData;
					float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					half3 worldNormal = TransformObjectToHClip(v.normal);
					half3 worldTangent = TransformObjectToHClip(v.tangent.xyz);
					half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
					half3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
					o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
					o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
					o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
					//o.customPack1.xy = customInputData.uv_texcoord;
					o.customPack1.xy = v.uv0;
					//TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
				}
				half4 frag(v2f IN) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID(IN);
					Input surfIN;
					UNITY_INITIALIZE_OUTPUT(Input, surfIN);
					surfIN.uv_texcoord = IN.customPack1.xy;
					float3 worldPos = float3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
					half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
					surfIN.worldPos = worldPos;
					surfIN.worldNormal = float3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
					surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
					surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
					surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
					SurfaceOutputStandard o;
					UNITY_INITIALIZE_OUTPUT(SurfaceOutputStandard, o)
					surf(surfIN, o);
					#if defined( CAN_SKIP_VPOS )
					float2 vpos = IN.pos;
					#endif
					SHADOW_CASTER_FRAGMENT(IN)
				}
				ENDHLSL
						//*/

				}
				//Fallback "Diffuse"
				//CustomEditor "ASEMaterialInspector"
		}
}