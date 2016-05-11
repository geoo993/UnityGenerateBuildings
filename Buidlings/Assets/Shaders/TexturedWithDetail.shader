Shader ".ShaderExample/Textured With Detail"
{

	Properties {

		_MainTex ("Texture", 2D) = "white" {}
		_DetailTex ("Detail Texture", 2D) = "gray" {}
		_Tint("Tint", Color) = (1, 1, 1, 1)

		_Light ("Light", Range(0.0 , 2.0)) = 0.5
		_TexelDensity ("Density", Range(1 , 10)) = 5

	}


	SubShader {


		Pass {

			CGPROGRAM
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"



			sampler2D _MainTex, _DetailTex;
			float4 _MainTex_ST, _DetailTex_ST;

			float4 _Tint;
			uniform float _Light;
			uniform float _TexelDensity;

			float4 MyVertexProgram (float4 position : POSITION, inout float2 uv : TEXCOORD0, out float2 uvDetail : TEXCOORD1) : SV_POSITION {
				
				uvDetail = TRANSFORM_TEX(uv, _DetailTex);
				uv = TRANSFORM_TEX(uv, _MainTex);
				return mul(UNITY_MATRIX_MVP, position);
			}


//			float4 MyFragmentProgram (float2 uv : TEXCOORD0) : SV_TARGET {
//
//				float4 color = tex2D(_MainTex, uv) * (_Tint + _Light);
//				color = tex2D(_MainTex, uv * _TexelDensity) * (_Tint + _Light);
//				return color;
//			}

			float4 MyFragmentProgram (float2 uv : TEXCOORD0, float2 uvDetail : TEXCOORD1) : SV_TARGET {

				float4 color = tex2D(_MainTex, uv) * (_Tint + _Light);
				color *= tex2D(_DetailTex, uvDetail * _TexelDensity) * unity_ColorSpaceDouble;//(_Tint + _Light);
				return color;
			}



			ENDCG

		}

	}


}
