using UnityEngine;
using System.Collections;

public class PostProcessingScript : MonoBehaviour {

	public Material ShaderMaterial;
	
	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		ShaderMaterial.SetFloat("_ResWidth", Screen.width);
		ShaderMaterial.SetFloat("_ResHeigth", Screen.height);
		Graphics.Blit(src, dest, ShaderMaterial);
	}
}
