// Compiled shader for PC, Mac & Linux Standalone, uncompressed size: 6.0KB

// Skipping shader variants that would not be included into build of current scene.

Shader ".ShaderExample/UnlitShader" {
Properties {
 _Tint ("Tint", Color) = (1.000000,1.000000,1.000000,1.000000)
 _Brightness ("Brightness", Range(-0.500000,0.500000)) = 0.400000
 _Blend ("Blend", Range(0.000000,1.000000)) = 0.500000
}
SubShader { 


 // Stats for Vertex shader:
 //       metal : 1 math
 //      opengl : 1 math
 // Stats for Fragment shader:
 //       metal : 1 math
 Pass {
  GpuProgramID 51330
Program "vp" {
SubProgram "opengl " {
// Stats: 1 math
"#version 120

#ifdef VERTEX

varying vec2 xlv_TEXCOORD0;
void main ()
{
  xlv_TEXCOORD0 = gl_MultiTexCoord0.xy;
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
}


#endif
#ifdef FRAGMENT
varying vec2 xlv_TEXCOORD0;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.zw = vec2(1.0, 1.0);
  tmpvar_1.xy = xlv_TEXCOORD0;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "metal " {
// Stats: 1 math
Bind "vertex" ATTR0
Bind "texcoord" ATTR1
ConstBuffer "$Globals" 64
Matrix 0 [glstate_matrix_mvp]
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
};
struct xlatMtlShaderUniform {
  float4x4 glstate_matrix_mvp;
};
vertex xlatMtlShaderOutput xlatMtlMain (xlatMtlShaderInput _mtl_i [[stage_in]], constant xlatMtlShaderUniform& _mtl_u [[buffer(0)]])
{
  xlatMtlShaderOutput _mtl_o;
  _mtl_o.xlv_TEXCOORD0 = _mtl_i._glesMultiTexCoord0.xy;
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
uniform 	vec4 _Tint;
uniform 	float _Brightness;
uniform 	float _Blend;
in  vec4 in_POSITION0;
in  vec2 in_TEXCOORD0;
out vec2 vs_TEXCOORD0;
vec4 t0;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    return;
}
#endif
#ifdef FRAGMENT
#version 150
#extension GL_ARB_explicit_attrib_location : require
#extension GL_ARB_shader_bit_encoding : enable
in  vec2 vs_TEXCOORD0;
layout(location = 0) out vec4 SV_TARGET0;
void main()
{
    SV_TARGET0.xy = vs_TEXCOORD0.xy;
    SV_TARGET0.zw = vec2(1.0, 1.0);
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
// Stats: 1 math
"#include <metal_stdlib>
#pragma clang diagnostic ignored "-Wparentheses-equality"
using namespace metal;
struct xlatMtlShaderInput {
  float2 xlv_TEXCOORD0;
};
struct xlatMtlShaderOutput {
  half4 _glesFragData_0 [[color(0)]];
};
struct xlatMtlShaderUniform {
};
fragment xlatMtlShaderOutput xlatMtlMain (xlatMtlShaderInput _mtl_i [[stage_in]], constant xlatMtlShaderUniform& _mtl_u [[buffer(0)]])
{
  xlatMtlShaderOutput _mtl_o;
  float4 tmpvar_1;
  tmpvar_1.zw = float2(1.0, 1.0);
  tmpvar_1.xy = _mtl_i.xlv_TEXCOORD0;
  _mtl_o._glesFragData_0 = half4(tmpvar_1);
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