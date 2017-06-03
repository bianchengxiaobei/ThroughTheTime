//////////////////////////////////////////////
/// 2DxFX - 2D SPRITE FX - by VETASOFT 2015 //
//////////////////////////////////////////////

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu ("2DxFX/Standard/FireAdditive")]
[System.Serializable]
public class _2dxFX_FireAdditive : MonoBehaviour
{
	[HideInInspector] public Material ForceMaterial;
	[HideInInspector] public bool ActiveChange=true;
	private string shader = "2DxFX/Standard/FireAdditive";
	[HideInInspector] [Range(0, 1)] public float _Alpha = 1f;

	[HideInInspector] public Texture2D __MainTex2; 
	[HideInInspector] [Range(64, 256)] public float _Value1=64;
	[HideInInspector] [Range(0, 2)] public float _Value2=1;
	[HideInInspector] [Range(0, 1)] public float _Value3=1;
	[HideInInspector] public float _Value4;

	[HideInInspector] public bool _AutoScrollX;
	[HideInInspector] [Range(0, 10)]public float _AutoScrollSpeedX;
	[HideInInspector] public bool _AutoScrollY;
	[HideInInspector] [Range(0, 10)] public float _AutoScrollSpeedY;
	[HideInInspector]  private float _AutoScrollCountX;
	[HideInInspector]  private float _AutoScrollCountY;

	[HideInInspector] public int ShaderChange=0;
	Material tempMaterial;
	Material defaultMaterial;

	
	void Start ()
	{ 
		__MainTex2 = Resources.Load ("_2dxFX_FireTXT") as Texture2D;
		ShaderChange = 0;
		GetComponent<Renderer>().sharedMaterial.SetTexture ("_MainTex2", __MainTex2);
	}

 	public void CallUpdate()
	{
		Update ();
	}



	void Update()
	{	
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
			__MainTex2 = Resources.Load ("_2dxFX_FireTXT") as Texture2D;
			GetComponent<Renderer>().sharedMaterial.SetTexture ("_MainTex2", __MainTex2);
		}
		#endif
		if (ActiveChange)
		{
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 1-_Alpha);

			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value1",_Value1);
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value2",_Value2);
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value3",_Value3);
			GetComponent<Renderer>().sharedMaterial.SetFloat("_Value4",_Value4);

			
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
		if (gameObject.activeSelf && defaultMaterial!=null) {
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
			__MainTex2 = Resources.Load ("_2dxFX_FireTXT") as Texture2D;
		}
		else
		{
			ForceMaterial.shader=Shader.Find(shader);
			ForceMaterial.hideFlags = HideFlags.None;
			GetComponent<Renderer>().sharedMaterial = ForceMaterial;
			__MainTex2 = Resources.Load ("_2dxFX_FireTXT") as Texture2D;
		}

		if (__MainTex2)	
		{
			__MainTex2.wrapMode= TextureWrapMode.Repeat;
			GetComponent<Renderer>().sharedMaterial.SetTexture ("_MainTex2", __MainTex2);
		}
	}
}




#if UNITY_EDITOR
[CustomEditor(typeof(_2dxFX_FireAdditive)),CanEditMultipleObjects]
public class _2dxFX_FireAdditive_Editor : Editor
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
		
		_2dxFX_FireAdditive _2dxScript = (_2dxFX_FireAdditive)target;
	
		Texture2D icon = Resources.Load ("2dxfxinspector-anim") as Texture2D;
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


	
			Texture2D icone = Resources.Load ("2dxfx-icon-time") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_Value1"), new GUIContent("Fire Speed", icone, "Change the fire speed"));

			icone = Resources.Load ("2dxfx-icon-brightness") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_Value2"), new GUIContent("Fire Intensity", icone, "Change the fire light intensity"));

			icone = Resources.Load ("2dxfx-icon-fade") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_Value3"), new GUIContent("Fade Inside", icone, "Change the inside sprite fade value"));

		




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