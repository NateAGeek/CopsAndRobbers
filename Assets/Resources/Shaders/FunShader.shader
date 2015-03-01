Shader "Custom/FuncShader" {
	Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader {
		Pass{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
			

			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
