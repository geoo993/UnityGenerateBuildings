Shader ".ShaderExample/NormalDiffuse/WorldSpaceThreeTextures" {
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		//[NoScaleOffset] _MainTex ("Base (RGB)", 2D) = "white" {}
		[NoScaleOffset] _MainTexWall2 ("Wall Side Texture (RGB)", 2D) = "surface" {}
		[NoScaleOffset] _MainTexWall ("Wall Front Texture (RGB)", 2D) = "surface" {} 
		[NoScaleOffset] _MainTexFlr2 ("Flr Texture", 2D) = "surface" {} 
		_Scale ("Texture Scale", Float) = 1.0
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTexWall, _MainTexWall2, _MainTexFlr2;
		//sampler2D _MainTex;
		fixed4 _Color;
		float _Scale;

		struct Input 
		{
			float3 worldNormal;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
//			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
//			o.Albedo = c.rgb;
//			o.Alpha = c.a;

			float2 UV;
			fixed4 c;

			if(abs(IN.worldNormal.x) > 0.5) 
			{
				UV = IN.worldPos.yz; // side
				c = tex2D(_MainTexWall2, UV * _Scale); // use WALLSIDE texture

			} else if(abs(IN.worldNormal.z) > 0.5) 
			{
				UV = IN.worldPos.xy; // front
				c = tex2D(_MainTexWall, UV * _Scale); // use WALL texture

			} else 
			{
				UV = IN.worldPos.xz; // top
				c = tex2D(_MainTexFlr2, UV * _Scale); // use FLR texture

			}

			o.Albedo = c.rgb * _Color;

		}
		ENDCG
	}

	Fallback "Legacy Shaders/VertexLit"
}
