using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PostProcessingScript : MonoBehaviour {

	public Material ShaderMaterial;

	void Start(){
		camera.depthTextureMode = DepthTextureMode.DepthNormals;
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		ShaderMaterial.SetFloat("_ResWidth", Screen.width);
		ShaderMaterial.SetFloat("_ResHeigth", Screen.height);
		//ShaderMaterial.SetTexture ("_DepthTex", RenderTextureFormat.Depth);
		Graphics.Blit(src, dest, ShaderMaterial);
	}
}
