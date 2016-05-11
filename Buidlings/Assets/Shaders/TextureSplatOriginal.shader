Shader ".ShaderExample/TextureSplatOriginal"
{

	Properties {

		_MainTex ("Splat Map", 2D) = "white" {}
		[NoScaleOffset] _Texture1 ("Texture 1", 2D) = "white" {}
		[NoScaleOffset] _Texture2 ("Texture 2", 2D) = "white" {}
		[NoScaleOffset] _Texture3 ("Texture 3", 2D) = "white" {}
		[NoScaleOffset] _Texture4 ("Texture 4", 2D) = "white" {}
		_Tint("Tint", Color) = (1, 1, 1, 1)
		_Transition1 ("Transition1", Range(0.0 , 1.0)) = 1.0
		_Transition2 ("Transition2", Range(0.0 , 1.0)) = 1.0
		_Transition3 ("Transition3", Range(0.0 , 1.0)) = 1.0

	}


	SubShader {


		Pass {

			CGPROGRAM
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			sampler2D _MainTex, _Texture1, _Texture2, _Texture3, _Texture4;;
			float4 _MainTex_ST;
			float4 _Tint;
			uniform float _Transition1, _Transition2, _Transition3;

			float4 MyVertexProgram (float4 position : POSITION, inout float2 uv : TEXCOORD0, out float2 uvSplat : TEXCOORD1) : SV_POSITION {
				
				uvSplat = uv;
				uv = TRANSFORM_TEX(uv, _MainTex);
				return mul(UNITY_MATRIX_MVP, position);
			}


			float4 MyFragmentProgram (float2 uv : TEXCOORD0, float2 uvSplat : TEXCOORD1) : SV_TARGET {

				float4 splat = tex2D(_MainTex, uvSplat) * _Tint;

//				return 	tex2D(_Texture1, uv) * splat.r +
//						tex2D(_Texture2, uv) * (1 - splat.r);

//				return	tex2D(_Texture1, uv) * splat.r +
//						tex2D(_Texture2, uv) * splat.g +
//						tex2D(_Texture3, uv) * splat.b +
//						tex2D(_Texture4, uv) * (1 - splat.r - splat.g - splat.b);


				return	tex2D(_Texture1, uv) * splat.r +
						tex2D(_Texture2, uv) * splat.g +
						tex2D(_Texture3, uv) * splat.b +
						tex2D(_Texture4, uv) * (1 - _Transition1 - _Transition2 - _Transition3);



			}



			ENDCG

		}

	}


}
