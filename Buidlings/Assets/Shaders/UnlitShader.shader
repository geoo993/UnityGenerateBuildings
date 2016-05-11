Shader ".ShaderExample/UnlitShader"
{

	Properties {

		_MainTex ("Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (1, 1, 1, 1)

		_Light ("Light", Range(0.0 , 1.0)) = 0.5
		//_Brightness ("brightness", Range(-0.5 , 0.5)) = 0.4
		//_Blend ("Blend", Range (0.0, 1.0) ) = 0.5 
	}


	SubShader {


		Pass {

			CGPROGRAM
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			//uniform float _Brightness;
			uniform float _Blend;
			uniform float _Light;

			float4 MyVertexProgram (float4 position : POSITION, inout float2 uv : TEXCOORD0) : SV_POSITION {
				//localPosition = position.xyz;
				//uv = uv * _MainTex_ST.xy + _MainTex_ST.zw;
				uv = TRANSFORM_TEX(uv, _MainTex);
				return mul(UNITY_MATRIX_MVP, position);
			}

//			float4 MyFragmentProgram () : SV_TARGET {
//				return _Tint;
//			}
//			float4 MyFragmentProgram (float3 localPosition : TEXCOORD0) : SV_TARGET {
//				//return float4(localPosition, 1);
//				return float4((localPosition * 0.5) + _Brightness, 1) * (_Tint + _Blend);
//
//			}

//			float4 MyFragmentProgram (float2 uv : TEXCOORD0) : SV_TARGET {
//				
//				return float4(uv, 1, 1);
//
//			}
			float4 MyFragmentProgram (float2 uv : TEXCOORD0) : SV_TARGET {
				
				return tex2D(_MainTex, uv) * (_Tint + _Light);

			}




			ENDCG

		}

	}


}
