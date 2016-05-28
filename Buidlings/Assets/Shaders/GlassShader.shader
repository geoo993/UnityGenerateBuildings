// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader ".ShaderExample/GlassShader"
 {
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)

		_BumpMap ("Noise ", 2D) = "bump" {}
		_Magnitude("Magnitude", Range(0.0, 1.0)) = 0.5

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

			sampler2D _GrabTexture, _MainTex, _BumpMap;

			fixed4 _Color;

			float  _Magnitude;



			struct vin_vct
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;

			};

			struct v2f_vct
			{
				float4 vertex : POSITION;	// vertex space
				fixed4 color : COLOR;		// Vertex colour
				float2 texcoord : TEXCOORD0;	// UV data

				float4 uvgrab : TEXCOORD1;
			};

			// Vertex function 
			v2f_vct vert (vin_vct v)
			{
				v2f_vct o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
 
				o.texcoord = v.texcoord;
 
				o.uvgrab = ComputeGrabScreenPos(o.vertex);

				#if UNITY_UV_STARTS_AT_TOP
				o.uvgrab.y *= 1;
				#endif

				return o;
			}

			// Fragment function
			half4 frag (v2f_vct i) : COLOR
			{
				half4 mainColour = tex2D(_MainTex, i.texcoord);
				
				half4 bump = tex2D(_BumpMap, i.texcoord);
				half2 distortion = UnpackNormal(bump).rg;
 
				i.uvgrab.xy += distortion * _Magnitude;
 
				fixed4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				return col * mainColour * _Color;
			}


			ENDCG
		} 
	}
}
