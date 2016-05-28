Shader ".ShaderExample/ScreenScapeGradient" {

	Properties {
		_Color1 ("Top Color", Color) = (1,1,1,1)
		_Color2 ("Bottom Color", Color) = (1,1,1,1)
		_MainTex ("Base", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed4 _Color1, _Color2;

		struct Input 
		{
			float2 uv_MainTex;
			float4 screenPos; 
		};

		void surf (Input IN, inout SurfaceOutput OUT) 
		{
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * lerp(_Color2,_Color1,screenUV.y);
			OUT.Albedo = c.rgb;
			OUT.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Legacy Shaders/VertexLit"
}
