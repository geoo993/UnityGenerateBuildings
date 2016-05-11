// Compiled shader for PC, Mac & Linux Standalone, uncompressed size: 10.8KB

// Skipping shader variants that would not be included into build of current scene.

Shader ".ShaderExample/Textured With Detail" {
Properties {
 _MainTex ("Texture", 2D) = "white" { }
 _DetailTex ("Detail Texture", 2D) = "gray" { }
 _Tint ("Tint", Color) = (1.000000,1.000000,1.000000,1.000000)
 _Light ("Light", Range(0.000000,2.000000)) = 0.500000
 _TexelDensity ("Density", Range(1.000000,10.000000)) = 5.000000
}
SubShader { 


 // Stats for Vertex shader:
 //       metal : 5 math
 //      opengl : 5 math, 2 texture
 // Stats for Fragment shader:
 //       metal : 5 math, 2 texture
 Pass {
  GpuProgramID 8427
Program "vp" {
SubProgram "opengl " {
// Stats: 5 math, 2 textures
"#version 120

#ifdef VERTEX

uniform vec4 _MainTex_ST;
uniform vec4 _DetailTex_ST;
varying vec2 xlv_TEXCOORD0;
varying vec2 xlv_TEXCOORD1;
void main ()
{
  xlv_TEXCOORD0 = ((gl_MultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((gl_MultiTexCoord0.xy * _DetailTex_ST.xy) + _DetailTex_ST.zw);
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _DetailTex;
uniform vec4 _Tint;
uniform float _Light;
uniform float _TexelDensity;
varying vec2 xlv_TEXCOORD0;
varying vec2 xlv_TEXCOORD1;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = (_Tint + _Light);
  gl_FragData[0] = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (texture2D (_DetailTex, (xlv_TEXCOORD1 * _TexelDensity)) * tmpvar_1));
}


#endif
"
}
SubProgram "metal " {
// Stats: 5 math
Bind "vertex" ATTR0
Bind "texcoord" ATTR1
ConstBuffer "$Globals" 96
Matrix 0 [glstate_matrix_mvp]
Vector 64 [_MainTex_ST]
Vector 80 [_DetailTex_ST]
"#include <metal_stdlib>
#pragma clang diagnostic ignored "-Wparentheses-equality"
using namespace metal;
struct xlatMtlShaderInput {
  float4 _glesVertex [[attribute(0)]];
  float4 _glesMultiTexCoord0 [[attribute(1)]];
};
struct xlatMtlShaderOutput {
  float4 gl_Position [[position]];
  float2 xlv_TEXCOORD0;
  float2 xlv_TEXCOORD1;
};
struct xlatMtlShaderUniform {
  float4x4 glstate_matrix_mvp;
  float4 _MainTex_ST;
  float4 _DetailTex_ST;
};
vertex xlatMtlShaderOutput xlatMtlMain (xlatMtlShaderInput _mtl_i [[stage_in]], constant xlatMtlShaderUniform& _mtl_u [[buffer(0)]])
{
  xlatMtlShaderOutput _mtl_o;
  _mtl_o.xlv_TEXCOORD0 = ((_mtl_i._glesMultiTexCoord0.xy * _mtl_u._MainTex_ST.xy) + _mtl_u._MainTex_ST.zw);
  _mtl_o.xlv_TEXCOORD1 = ((_mtl_i._glesMultiTexCoord0.xy * _mtl_u._DetailTex_ST.xy) + _mtl_u._DetailTex_ST.zw);
  _mtl_o.gl_Position = (_mtl_u.glstate_matrix_mvp * _mtl_i._glesVertex);
  return _mtl_o;
}

"
}
SubProgram "glcore " {
"#ifdef VERTEX
#version 150
#extension GL_ARB_explicit_attrib_location : require
#extension GL_ARB_shader_bit_encoding : enable
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4x4 unity_CameraProjection;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 unity_WorldToCamera;
uniform 	mat4x4 unity_CameraToWorld;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	vec4 unity_4LightAtten0;
uniform 	vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	vec4 unity_SHAr;
uniform 	vec4 unity_SHAg;
uniform 	vec4 unity_SHAb;
uniform 	vec4 unity_SHBr;
uniform 	vec4 unity_SHBg;
uniform 	vec4 unity_SHBb;
uniform 	vec4 unity_SHC;
uniform 	vec3 unity_LightColor0;
uniform 	vec3 unity_LightColor1;
uniform 	vec3 unity_LightColor2;
uniform 	vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4x4 unity_WorldToShadow[4];
uniform 	vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	mat4x4 unity_ObjectToWorld;
uniform 	mat4x4 unity_WorldToObject;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4x4 glstate_matrix_transpose_modelview0;
uniform 	mat4x4 glstate_matrix_projection;
uniform 	mat4x4 unity_MatrixV;
uniform 	mat4x4 unity_MatrixVP;
uniform 	vec4 glstate_lightmodel_ambient;
uniform 	vec4 unity_AmbientSky;
uniform 	vec4 unity_AmbientEquator;
uniform 	vec4 unity_AmbientGround;
uniform 	vec4 unity_IndirectSpecColor;
uniform 	vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	vec4 unity_SpecCube1_HDR;
uniform 	vec4 unity_ColorSpaceGrey;
uniform 	vec4 unity_ColorSpaceDouble;
uniform 	vec4 unity_ColorSpaceDielectricSpec;
uniform 	vec4 unity_ColorSpaceLuminance;
uniform 	vec4 unity_Lightmap_HDR;
uniform 	vec4 unity_DynamicLightmap_HDR;
uniform 	vec4 _MainTex_ST;
uniform 	vec4 _DetailTex_ST;
uniform 	vec4 _Tint;
uniform 	float _Light;
uniform 	float _TexelDensity;
in  vec4 in_POSITION0;
in  vec2 in_TEXCOORD0;
out vec2 vs_TEXCOORD0;
out vec2 vs_TEXCOORD1;
vec4 t0;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = in_TEXCOORD0.xy * _DetailTex_ST.xy + _DetailTex_ST.zw;
    return;
}
#endif
#ifdef FRAGMENT
#version 150
#extension GL_ARB_explicit_attrib_location : require
#extension GL_ARB_shader_bit_encoding : enable
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4x4 unity_CameraProjection;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 unity_WorldToCamera;
uniform 	mat4x4 unity_CameraToWorld;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	vec4 unity_4LightAtten0;
uniform 	vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	vec4 unity_SHAr;
uniform 	vec4 unity_SHAg;
uniform 	vec4 unity_SHAb;
uniform 	vec4 unity_SHBr;
uniform 	vec4 unity_SHBg;
uniform 	vec4 unity_SHBb;
uniform 	vec4 unity_SHC;
uniform 	vec3 unity_LightColor0;
uniform 	vec3 unity_LightColor1;
uniform 	vec3 unity_LightColor2;
uniform 	vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4x4 unity_WorldToShadow[4];
uniform 	vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	mat4x4 unity_ObjectToWorld;
uniform 	mat4x4 unity_WorldToObject;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4x4 glstate_matrix_transpose_modelview0;
uniform 	mat4x4 glstate_matrix_projection;
uniform 	mat4x4 unity_MatrixV;
uniform 	mat4x4 unity_MatrixVP;
uniform 	vec4 glstate_lightmodel_ambient;
uniform 	vec4 unity_AmbientSky;
uniform 	vec4 unity_AmbientEquator;
uniform 	vec4 unity_AmbientGround;
uniform 	vec4 unity_IndirectSpecColor;
uniform 	vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	vec4 unity_SpecCube1_HDR;
uniform 	vec4 unity_ColorSpaceGrey;
uniform 	vec4 unity_ColorSpaceDouble;
uniform 	vec4 unity_ColorSpaceDielectricSpec;
uniform 	vec4 unity_ColorSpaceLuminance;
uniform 	vec4 unity_Lightmap_HDR;
uniform 	vec4 unity_DynamicLightmap_HDR;
uniform 	vec4 _MainTex_ST;
uniform 	vec4 _DetailTex_ST;
uniform 	vec4 _Tint;
uniform 	float _Light;
uniform 	float _TexelDensity;
uniform  sampler2D _MainTex;
uniform  sampler2D _DetailTex;
in  vec2 vs_TEXCOORD0;
in  vec2 vs_TEXCOORD1;
layout(location = 0) out vec4 SV_TARGET0;
vec4 t0;
lowp vec4 t10_0;
vec4 t1;
lowp vec4 t10_2;
void main()
{
    t0.xy = vs_TEXCOORD1.xy * vec2(vec2(_TexelDensity, _TexelDensity));
    t10_0 = texture(_DetailTex, t0.xy);
    t1 = _Tint + vec4(_Light);
    t0 = t10_0 * t1;
    t10_2 = texture(_MainTex, vs_TEXCOORD0.xy);
    t1 = t1 * t10_2;
    SV_TARGET0 = t0 * t1;
    return;
}
#endif
"
}
}
Program "fp" {
SubProgram "opengl " {
"// shader disassembly not supported on opengl"
}
SubProgram "metal " {
// Stats: 5 math, 2 textures
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_DetailTex] 2D 1
ConstBuffer "$Globals" 24
Vector 0 [_Tint]
Float 16 [_Light]
Float 20 [_TexelDensity]
"#include <metal_stdlib>
#pragma clang diagnostic ignored "-Wparentheses-equality"
using namespace metal;
struct xlatMtlShaderInput {
  float2 xlv_TEXCOORD0;
  float2 xlv_TEXCOORD1;
};
struct xlatMtlShaderOutput {
  half4 _glesFragData_0 [[color(0)]];
};
struct xlatMtlShaderUniform {
  float4 _Tint;
  float _Light;
  float _TexelDensity;
};
fragment xlatMtlShaderOutput xlatMtlMain (xlatMtlShaderInput _mtl_i [[stage_in]], constant xlatMtlShaderUniform& _mtl_u [[buffer(0)]]
  ,   texture2d<half> _MainTex [[texture(0)]], sampler _mtlsmp__MainTex [[sampler(0)]]
  ,   texture2d<half> _DetailTex [[texture(1)]], sampler _mtlsmp__DetailTex [[sampler(1)]])
{
  xlatMtlShaderOutput _mtl_o;
  half4 tmpvar_1;
  tmpvar_1 = _MainTex.sample(_mtlsmp__MainTex, (float2)(_mtl_i.xlv_TEXCOORD0));
  half4 tmpvar_2;
  float2 P_3;
  P_3 = (_mtl_i.xlv_TEXCOORD1 * _mtl_u._TexelDensity);
  tmpvar_2 = _DetailTex.sample(_mtlsmp__DetailTex, (float2)(P_3));
  float4 tmpvar_4;
  float4 tmpvar_5;
  tmpvar_5 = (_mtl_u._Tint + _mtl_u._Light);
  tmpvar_4 = (((float4)tmpvar_1 * tmpvar_5) * ((float4)tmpvar_2 * tmpvar_5));
  _mtl_o._glesFragData_0 = half4(tmpvar_4);
  return _mtl_o;
}

"
}
SubProgram "glcore " {
"// shader disassembly not supported on glcore"
}
}
 }
}
}