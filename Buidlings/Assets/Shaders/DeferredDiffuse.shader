Shader ".ShaderExample/DeferredDiffuse"
{
	Properties 
	{
		_ColorTint("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _ColorTint;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = float4(c.rgb * _ColorTint, 1.0);
			o.Alpha = c.a;
		}
//		float4 LightingMyDiffuse_PrePass(SurfaceOutput i)
//		{
//			return float4(i.Albedo * light.rgb, 1.0);
//		}
//	

		ENDCG
	} 
}
