using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PostProcessingScriptOutline : MonoBehaviour {

	public Material ShaderMaterial;

	void Start(){
		camera.depthTextureMode = DepthTextureMode.DepthNormals;
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, ShaderMaterial);
	}
}
