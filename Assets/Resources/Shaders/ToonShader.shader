Shader "Custom/ToonShader" {
	Properties {
		_MainTex ("Post Image", 2D) = "white" {}
		_RampTex ("Post Image", 2D) = "white" {}
		_Strength ( "Additive Strength", Float ) = 1.0 
		_TintColor ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader {
		Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}
		Pass {
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma multi_compile_fwdbase
			#pragma vertex vertexShaderMain
			#pragma fragment fragmentShaderMain
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _RampTex;
			uniform float _Strength;
			uniform float4 _LightColor0;
			uniform float4 _TintColor;
			uniform float _TotalRamps;
			
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
			};
 
			vertexOutput vertexShaderMain(vertexInput input) {
				vertexOutput output;
                
				output.posWorld = mul(_Object2World, input.vertex);
                output.normalDir = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				
				TRANSFER_VERTEX_TO_FRAGMENT(output);
				
				return output;
			}
			
			float4 fragmentShaderMain(vertexOutput o) : COLOR {
				float3 normalDir = o.normalDir;
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - o.posWorld.xyz);
				float atten;
				float3 lightDir;
				
				if(_WorldSpaceLightPos0.w == 0.0)
				{
					// directional light only
					atten = LIGHT_ATTENUATION(o);
					lightDir = normalize(_WorldSpaceLightPos0.xyz);
				}
				else
				{
					// point lights only
					float3 fragToLightSource = _WorldSpaceLightPos0 - o.posWorld.xyz;
					atten = (1/length(fragToLightSource)) * LIGHT_ATTENUATION(o);
					lightDir = normalize(fragToLightSource);
				}
 
                float3 diffuseRef = atten * _LightColor0.xyz * saturate(dot(normalDir, lightDir));
                float intensity = dot(lightDir, normalDir);
                
                float3 rampColor = tex2D(_RampTex, float2(0.0, 0.0));
                
                rampColor = tex2D(_RampTex, float2(intensity, 0.0));
                
                float3 lightFinal = rampColor * UNITY_LIGHTMODEL_AMBIENT.rgb;
                
				return tex2D(_MainTex, o.tex.xy) * float4(lightFinal, 1.0) * float4(diffuseRef, 1.0) * _Strength;
			}
			ENDCG
		}
		Pass {
			Tags {"LightMode" = "ForwardAdd"}
			Blend One One 
			CGPROGRAM
			#pragma multi_compile_fwdbase_fullforwardshadows
			#pragma vertex vertexShaderMain
			#pragma fragment fragmentShaderMain
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _RampTex;
			uniform float _Strength;
			uniform float4 _LightColor0;
			uniform float4 _TintColor;
			uniform float _TotalRamps;
			
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
			};
 
			vertexOutput vertexShaderMain(vertexInput input) {
				vertexOutput output;
                
				output.posWorld = mul(_Object2World, input.vertex);
                output.normalDir = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				
				TRANSFER_VERTEX_TO_FRAGMENT(output);
				
				return output;
			}
			
			float4 fragmentShaderMain(vertexOutput o) : COLOR {
				float3 normalDir = o.normalDir;
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - o.posWorld.xyz);
				float atten;
				float3 lightDir;
				
				if(_WorldSpaceLightPos0.w == 0.0)
				{
					// directional light only
					atten = LIGHT_ATTENUATION(o);
					lightDir = normalize(_WorldSpaceLightPos0.xyz);
				}
				else
				{
					// point lights only
					float3 fragToLightSource = _WorldSpaceLightPos0 - o.posWorld.xyz;
					atten = (1/length(fragToLightSource)) * LIGHT_ATTENUATION(o);
					lightDir = normalize(fragToLightSource);
				}
                
                float3 diffuseRef = atten * _LightColor0.xyz * saturate(dot(normalDir, lightDir));
                float intensity = dot(lightDir, normalDir);
                
                float3 rampColor = tex2D(_RampTex, float2(0.0, 0.0));
                
                if(intensity > 0.95){
                	rampColor = tex2D(_RampTex, float2(0.0, 0.0));
                }else if(intensity > 0.5){
                	rampColor = tex2D(_RampTex, float2(0.25, 0.0));
                }else if(intensity > 0.25){
                	rampColor = tex2D(_RampTex, float2(0.50, 0.0));
                }else{
                	rampColor = tex2D(_RampTex, float2(0.75, 0.0));
                }
                
                float3 lightFinal = rampColor * UNITY_LIGHTMODEL_AMBIENT.rgb;
                
				return tex2D(_MainTex, o.tex.xy) * float4(lightFinal, 1.0) * float4(diffuseRef, 1.0) * _Strength;
			}
			ENDCG
		}
 
	}
	Fallback "VertexLit"
}