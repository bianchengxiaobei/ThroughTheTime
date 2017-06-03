//////////////////////////////////////////////
/// 2DxFX - 2D SPRITE FX - by VETASOFT 2015 //
//////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("2DxFX/Standard/EnergyBar")]
[System.Serializable]
public class _2dxFX_EnergyBar : MonoBehaviour
{
	[HideInInspector] public Material ForceMaterial;
	[HideInInspector] public bool ActiveChange=true;
	private string shader = "2DxFX/Standard/EnergyBar";

	[HideInInspector] [Range(0, 1)] public float _Alpha = 1f;
	[HideInInspector] [Range(0f, 1f)] public float BarProgress = 1f;
	[HideInInspector] [Range(0.9f, 1f)] public float _Value2 = 0.975f;
	[HideInInspector] [Range(0f, 0.5f)] public float _Value3 = 0.5f;
	[HideInInspector] [Range(0f, 1f)] public float _Value4 = 1.0f;
	[HideInInspector] [Range(0f, 1f)] public float _Value5 = 0.0f;

	[HideInInspector] public int ShaderChange=0;
	Material tempMaterial;
	Material defaultMaterial;
	void Start ()
	{ 
		ShaderChange = 0;
	}

 	public void CallUpdate()
	{
		Update ();
	}

	void Update()
	{	
		if (BarProgress > 1) BarProgress = 1;
		if (BarProgress < 0) BarProgress = 0;

		if ((ShaderChange == 0) && (ForceMaterial != null)) 
		{
			ShaderChange=1;
			if (tempMaterial!=null) DestroyImmediate(tempMaterial);
			GetComponent<Renderer>().sharedMaterial = ForceMaterial;
			ForceMaterial.hideFlags = HideFlags.None;
			ForceMaterial.shader=Shader.Find(shader);
			

		}
		if ((ForceMaterial == null) && (ShaderChange==1))
		{
			if (tempMaterial!=null) DestroyImmediate(tempMaterial);
			tempMaterial = new Material(Shader.Find(shader));
			tempMaterial.hideFlags = HideFlags.None;
			GetComponent<Renderer>().sharedMaterial = tempMaterial;
			ShaderChange=0;
		}
		
		#if UNITY_EDITOR
		if (GetComponent<Renderer>().sharedMaterial.shader.name == "Sprites/Default")
		{
			ForceMaterial.shader=Shader.Find(shader);
			ForceMaterial.hideFlags = HideFlags.None;
			GetComponent<Renderer>().sharedMaterial = ForceMaterial;
		}
		#endif
		if (ActiveChange)
		{
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 1-_Alpha);
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value1", BarProgress);
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value2", 1-_Value2);
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value3", 1-_Value3);
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value4", _Value4);
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value5", _Value5);

		}
		
	}
	
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true)) {
			
			if (tempMaterial!=null) DestroyImmediate(tempMaterial);
			
			if (gameObject.activeSelf && defaultMaterial!=null) {
				GetComponent<Renderer>().sharedMaterial = defaultMaterial;
				GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
			}
		}
	}
	void OnDisable()
	{ 
		if (gameObject.activeSelf && defaultMaterial!=null) 
		{
			GetComponent<Renderer>().sharedMaterial = defaultMaterial;
			GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
		}		
	}
	
	void OnEnable()
	{
		if (defaultMaterial == null) 
		{
			defaultMaterial = new Material(Shader.Find("Sprites/Default"));
			 
			
		}
		if (ForceMaterial==null)
		{
			ActiveChange=true;
			tempMaterial = new Material(Shader.Find(shader));
			tempMaterial.hideFlags = HideFlags.None;
			GetComponent<Renderer>().sharedMaterial = tempMaterial;




		}
		else
		{
			ForceMaterial.shader=Shader.Find(shader);
			ForceMaterial.hideFlags = HideFlags.None;
			GetComponent<Renderer>().sharedMaterial = ForceMaterial;

		}
		
	}
}




#if UNITY_EDITOR
[CustomEditor(typeof(_2dxFX_EnergyBar)),CanEditMultipleObjects]
public class _2dxFX_EnergyBar_Editor : Editor
{
	private SerializedObject m_object;
	
	public void OnEnable()
	{
		m_object = new SerializedObject(targets);
	}
	
	public override void OnInspectorGUI()
	{
		m_object.Update();
		DrawDefaultInspector();
		
		_2dxFX_EnergyBar _2dxScript = (_2dxFX_EnergyBar)target;
	
		Texture2D icon = Resources.Load ("2dxfxinspector") as Texture2D;
		if (icon)
		{
			Rect r;
			float ih=icon.height;
			float iw=icon.width;
			float result=ih/iw;
			float w=Screen.width;
			result=result*w;
			r = GUILayoutUtility.GetRect(ih, result);
			EditorGUI.DrawTextureTransparent(r,icon);
		}

		EditorGUILayout.PropertyField(m_object.FindProperty("ForceMaterial"), new GUIContent("Shared Material", "Use a unique material, reduce drastically the use of draw call"));
		
		if (_2dxScript.ForceMaterial == null)
		{
			_2dxScript.ActiveChange = true;
		}
		else
		{
			if(GUILayout.Button("Remove Shared Material"))
			{
				_2dxScript.ForceMaterial= null;
				_2dxScript.ShaderChange = 1;
				_2dxScript.ActiveChange = true;
				_2dxScript.CallUpdate();
			}
		
			EditorGUILayout.PropertyField (m_object.FindProperty ("ActiveChange"), new GUIContent ("Change Material Property", "Change The Material Property"));
		}

		if (_2dxScript.ActiveChange)
		{

			EditorGUILayout.BeginVertical("Box");


			Texture2D icone = Resources.Load ("2dxfx-icon-value") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("BarProgress"), new GUIContent("Energy Bar", icone, "Change the energy bar value from 0 to 1"));

			icone = Resources.Load ("2dxfx-icon-fade") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_Value2"), new GUIContent("Smooth Line", icone, "Smooth Line"));

			icone = Resources.Load ("2dxfx-icon-brightness") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_Value3"), new GUIContent("Darker Value", icone, "Darker Value"));



			icone = Resources.Load ("2dxfx-icon-color") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_Value4"), new GUIContent("Energy RED DANGER", icone, "Use the color energy red danger status, 0 = none / 1 = red"));

			icone = Resources.Load ("2dxfx-icon-color") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_Value5"), new GUIContent("Transparency Dark", icone, "Fade of the transparency dark"));




			EditorGUILayout.BeginVertical("Box");

			icone = Resources.Load ("2dxfx-icon-fade") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_Alpha"), new GUIContent("Fading", icone, "Fade from nothing to showing"));

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
	

		}
		
		m_object.ApplyModifiedProperties();
		
	}
}
#endif