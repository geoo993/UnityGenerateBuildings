Shader ".ShaderExample/TextureInterpolation"
{

	Properties {

		_MainTex ("Splat Map", 2D) = "white" {}
		[NoScaleOffset] _Texture1 ("Texture 1", 2D) = "white" {}
		[NoScaleOffset] _Texture2 ("Texture 2", 2D) = "white" {}
		_Transition ("Transition", Range(0.0 , 1.0)) = 0.5
	}


	SubShader {


		Pass {

			CGPROGRAM
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"



			sampler2D _MainTex, _Texture1, _Texture2;
			float4 _MainTex_ST;
			uniform float _Transition;

			float4 MyVertexProgram (float4 position : POSITION, inout float2 uv : TEXCOORD0, out float2 uvSplat : TEXCOORD1) : SV_POSITION {
				
				uvSplat = uv;
				uv = TRANSFORM_TEX(uv, _MainTex);
				return mul(UNITY_MATRIX_MVP, position);
			}


			float4 MyFragmentProgram (float2 uv : TEXCOORD0, float2 uvSplat : TEXCOORD1) : SV_TARGET {

				float4 splat = tex2D(_MainTex, uvSplat);

				return 	tex2D(_Texture1, uv) * _Transition +
						tex2D(_Texture2, uv) * (1 - _Transition);
			}



			ENDCG

		}

	}


}
