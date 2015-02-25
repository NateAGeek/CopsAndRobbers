Shader "Custom/ToonPostProcesing" {
	Properties {
		_MainTex ("Post Image", 2D) = "white" {}
		_DepthTex ("Depth Texture", 2D) = "white" {}
		_ResWidth ("ResWidth", Float) = 1000.0
		_ResHeight ("ResHeight", Float) = 1000.0
		_Tint ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineColor ("Outline Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineThickness ("Outline Thickness", Range(0.0, 1.0)) = 0.1
		_OutlineThreshold ("Outline Threshold", Range(0.0, 1.0)) = 0.1
	}
	SubShader {
		Pass{
		CGPROGRAM
			#pragma vertex vertexShaderMain
			#pragma fragment fragmentShaderMain
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _Tint;
			uniform float4 _OutlineColor;
			uniform half _OutlineThickness;
			uniform float _ResWidth;
			uniform float _ResHeight;
			uniform float _OutlineThreshold;
			uniform sampler2D _CameraDepthTexture;
			
			
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput {
				float4 position : SV_POSITION;
				float4 color : COLOR;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				float4 scrPos : TEXCOORD3;
			};

			vertexOutput vertexShaderMain(vertexInput input) {
				vertexOutput output;
				
				output.position = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
//				output.posWorld = mul(_Object2World, input.vertex);
//				float3 view = normalize(_WorldSpaceCameraPos - output.posWorld);
//				output.normalDir = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
//				
//				output.color.r = output.normalDir.x;
//				output.color.g = output.normalDir.y;
//				output.color.b = 1.0 - output.position.z/100.0;
				
				return output;
			}
			
			float4 fragmentShaderMain(vertexOutput o) : COLOR{
				float dx = 1.0/_ResWidth;
				float dy = 1.0/_ResHeight;
				
				float4 center = tex2D(_MainTex, o.tex.xy); 
				float4 top = tex2D(_MainTex, float2(o.tex.x + _OutlineThickness, o.tex.y + dy + _OutlineThickness));
				float4 bottom = tex2D( _MainTex, float2(o.tex.x + _OutlineThickness, (o.tex.y - dy) + _OutlineThickness));
				float4 right = tex2D( _MainTex, float2(o.tex.x + dx + _OutlineThickness, o.tex.y + _OutlineThickness));
				float4 left = tex2D( _MainTex, float2((o.tex.x - dx) + _OutlineThickness, o.tex.y + _OutlineThickness));
			 
				if(distance(center, bottom) > _OutlineThreshold || distance(center, top) > _OutlineThreshold || distance(center, right) > _OutlineThreshold || distance(center, left) > _OutlineThreshold){
					return float4(0.0, 0.0, 0.0, 1.0);
				}
				else{
					//return float4(1.0, 1.0, 1.0, 1.0);
					return tex2D( _MainTex, o.tex.xy);
				}
				  
				  //return tex2D( _MainTex, o.tex.xy)*distance(center, right);
				
				//float3 view = normalize(_WorldSpaceCameraPos - o.posWorld);
				//bool edgeDetection = (dot(view, o.normalDir) > _OutlineThickness) ? true : false;
				
				
				//if(edgeDetection){
					//return float4(o.normalDir, 1.0);
					//return _Tint * float4(tex.xyz, 1.0);
				//}else{
					//return float4(o.normalDir, 1.0);
				//}
				//return texCen;
			}
		
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
