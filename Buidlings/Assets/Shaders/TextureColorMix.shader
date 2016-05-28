Shader ".ShaderExample/TextureColorMix"
 {
     Properties
     {
         _MainTex ("Base (RGB)", 2D) = "white" {}
         _BlendTex ("_BlendTex", 2D) = "white" {}
         _Blend1 ("Blend", Range (0, 1) ) = 0 
     }
     SubShader
     {
         Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
         LOD 200
         
         ZWrite Off
         //Blend SrcAlpha OneMinusSrcAlpha
         //Blend One One
         //Blend SrcAlpha OneMinusSrcAlpha
 
         
         CGPROGRAM
         #pragma surface surf Lambert
 
         sampler2D _MainTex;
         sampler2D _BlendTex;
         float _Blend1;
 
         struct Input
         {
             float2 uv_MainTex;
         };
 
//         void surf (Input IN, inout SurfaceOutput o)
//         {
//             fixed4 mainCol = tex2D(_MainTex, IN.uv_MainTex);
//             fixed4 texTwoCol = tex2D(_BlendTex, IN.uv_MainTex);
//
//             fixed4 output = lerp(mainCol, texTwoCol, _Blend1);
//             o.Albedo = output.rgb;
//             o.Alpha = output.a;
//         }
//

          void surf (Input IN, inout SurfaceOutput o)
	    {
	      fixed4 mainCol = tex2D(_MainTex, IN.uv_MainTex);
	      fixed4 texTwoCol = tex2D(_BlendTex, IN.uv_MainTex);                           
	      
	      fixed4 mainOutput = mainCol.rgba * (1.0 - (texTwoCol.a * _Blend1));
	      fixed4 blendOutput = texTwoCol.rgba * texTwoCol.a * _Blend1;         
	      
	      o.Albedo = mainOutput.rgb + blendOutput.rgb;
	      o.Alpha = mainOutput.a + blendOutput.a;
	    }

         ENDCG
     } 
     FallBack "Diffuse"
 }
