using UnityEngine;
using System.Collections;

public class PostProcessingIRGlasses : MonoBehaviour {

	public Material ShaderMaterial;

	// Use this for initialization
	void Start () {
		camera.depthTextureMode = DepthTextureMode.DepthNormals;
	}
	
	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, ShaderMaterial);
	}
}
