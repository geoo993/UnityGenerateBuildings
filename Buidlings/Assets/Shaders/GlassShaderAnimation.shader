// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader ".ShaderExample/GlassShaderAnimation"
 {
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_waterColour ("Colour", Color) = (1,1,1,1)

		_NoiseTex ("Noise text", 2D) = "bump" {}
		_CausticTex ("Caustic Text", 2D) = "white" {}

		_offset("Offser", Range(0.0, 10.0)) = 2.0
		_waterMagnitude("Water Magnitude", Range(0.0, 1.0)) = 0.5
		_waterPeriod("Water Period", Range(0.0, 10.0)) = 0.5

	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque"}
		ZWrite On Lighting Off Cull Off Fog { Mode Off } Blend One Zero

		GrabPass { "_GrabTexture" }
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture;

			sampler2D _MainTex;
			fixed4 _waterColour;

			sampler2D _NoiseTex, _CausticTex;
			float  _offset, _waterMagnitude, _waterPeriod;

			float2 sinusoid (float2 x, float2 m, float2 M, float2 p) {
				float2 e   = M - m;
				float2 c = 3.1415 * 2.0 / p;
				return e / 2.0 * (1.0 + sin(x * c)) + m;
			}


			struct vin_vct
			{
				float4 pos : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;

			};

			struct v2f_vct
			{
				float4 pos : SV_POSITION;	// Clip space
				fixed4 color : COLOR;		// Vertex colour
				float2 texcoord : TEXCOORD0;	// UV data

				float4 uvgrab : TEXCOORD1;

				float3 wPos : TEXCOORD2;	// World position
				float4 sPos : TEXCOORD3;	// Screen position
				float3 cPos : TEXCOORD4;	// Object center in world

			};

			// Vertex function 
			v2f_vct vert (vin_vct v)
			{
				v2f_vct o;


				o.pos = mul(UNITY_MATRIX_MVP, v.pos);
				o.color = v.color;
				o.texcoord = v.texcoord;

				o.uvgrab = ComputeGrabScreenPos(o.pos);

				o.wPos = mul(unity_ObjectToWorld, v.pos).xyz;
				o.sPos = ComputeScreenPos(o.pos);
				o.cPos = mul(unity_ObjectToWorld, half4(0,0,0,1));



//				#if UNITY_UV_STARTS_AT_TOP
//				o.uvgrab.y *= 1;
//				#endif

				return o;
			}

			// Fragment function
			fixed4 frag (v2f_vct i) : COLOR {

				i.sPos.xy /= i.sPos.w;


				fixed4 noise = tex2D(_NoiseTex, i.texcoord);
				fixed4 mainColour = tex2D(_MainTex, i.texcoord);
						
				float time = _Time[1];
			 
				float2 waterDisplacement =
				sinusoid
				(
					float2 (time, time) + (noise.xy) * _offset,
					float2(-_waterMagnitude, -_waterMagnitude),
					float2(+_waterMagnitude, +_waterMagnitude),
					float2(_waterPeriod, _waterPeriod)
				);
							
				i.uvgrab.xy += waterDisplacement;
				fixed4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				fixed4 causticColour = tex2D(_CausticTex, i.texcoord.xy*0.25 + waterDisplacement*5);
				return col * mainColour * _waterColour * causticColour;
			}
			 
			

			ENDCG
		} 
	}
}
