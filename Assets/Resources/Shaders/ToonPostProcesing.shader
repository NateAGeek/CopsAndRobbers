Shader "Custom/ToonPostProcesing" {
	Properties { 
		_MainTex ("Post Image", 2D) = "white" {}
		_OutlineColor ("Outline Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineThickness ("Outline Thickness", Float) = 0.1
		_OutlineThreshold ("Outline Threshold", Float) = 0.1
	}
	SubShader {
		Pass{
		CGPROGRAM
			#pragma vertex vertexShaderMain
			#pragma fragment fragmentShaderMain
			#pragma target 3.0
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _OutlineColor;
			uniform half _OutlineThickness;
			uniform float _OutlineThreshold;
			uniform sampler2D _CameraDepthNormalsTexture;
			
			
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput {
				float4 position : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 scrPos : TEXCOORD1;
			};

			vertexOutput vertexShaderMain(vertexInput input) {
				vertexOutput output;
				
				output.position = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				output.scrPos = ComputeScreenPos(output.position);
				
				return output;
			}
			
			float4 fragmentShaderMain(vertexOutput o) : COLOR{
				float dx = 1.0/_ScreenParams.x;
				float dy = 1.0/_ScreenParams.y;
				
			 	float depthValue;
			 	float3 normalValue;
			 	DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, o.scrPos.xy), depthValue, normalValue);
			 	float4 center = log(depthValue) + float4(normalValue, 1);
			 	DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, float2(o.scrPos.x, o.scrPos.y + dy + _OutlineThickness*dy)), depthValue, normalValue);
			 	float4 top = log(depthValue) + float4(normalValue, 1);
			 	DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, float2(o.scrPos.x, (o.scrPos.y - dy) - _OutlineThickness*dy)), depthValue, normalValue);
			 	float4 bottom = log(depthValue) + float4(normalValue, 1);
			 	DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, float2(o.scrPos.x + dx + _OutlineThickness*dx, o.scrPos.y)), depthValue, normalValue);
			 	float4 right = log(depthValue) + float4(normalValue, 1);
			 	DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, float2((o.scrPos.x - dx) - _OutlineThickness*dx, o.scrPos.y)), depthValue, normalValue);
			 	float4 left = log(depthValue) + float4(normalValue, 1);
			 	
				if(distance(center, bottom) >= _OutlineThreshold || distance(center, top) >= _OutlineThreshold || distance(center, right) >= _OutlineThreshold || distance(center, left) >= _OutlineThreshold){
					return _OutlineColor;
				}
				return tex2D( _MainTex, o.tex.xy);
			}
		
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
