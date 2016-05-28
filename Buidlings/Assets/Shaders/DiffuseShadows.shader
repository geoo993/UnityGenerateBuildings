Shader ".ShaderExample/DiffuseShadows" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Illum ("Illumin (A)", 2D) = "white" {}
		_Emission ("Emission (Lightmapper)", Float) = 1.0

		_ShadowColor ("ShadowColor", Color) = (1,1,1,1)
		_DiffuseVal ("Diffuse", Range(0.01, 1)) = 0.4
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _Illum;
		fixed4 _Color;
		fixed _Emission;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_Illum;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 c = tex * _Color;
			o.Albedo = c.rgb;
			o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;

			o.Emission *= _Emission.rrr;

			o.Alpha = c.a;
		}
		ENDCG

		CGPROGRAM
		#pragma surface surf CSLambert

		sampler2D _MainTex;
		float _DiffuseVal;
		uniform float4 _ShadowColor;

		struct Input {
			float2 uv_MainTex;
		};


		half4 LightingCSLambert (SurfaceOutput s, half3 lightDir, half atten) 
		{
			fixed diff = max (0, dot (s.Normal, lightDir));

			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
			
			//shadow colorization
			c.rgb += _ShadowColor.xyz * max(0.0,(1.0 - (diff * atten*2))) * _DiffuseVal;
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}


		ENDCG

	} 


	FallBack "Legacy Shaders/Self-Illumin/VertexLit"
	CustomEditor "LegacyIlluminShaderGUI"
}
