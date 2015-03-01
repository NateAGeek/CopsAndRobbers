using UnityEngine;
using System.Collections;

public class PostProcessingVignette : MonoBehaviour {

	public Material ShaderMaterial;
	
	void Start(){
		camera.depthTextureMode = DepthTextureMode.DepthNormals;
	}
	
	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, ShaderMaterial);
	}
}
