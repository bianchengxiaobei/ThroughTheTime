//////////////////////////////////////////////
/// 2DxFX - 2D SPRITE FX - by VETASOFT 2015 //
//////////////////////////////////////////////

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
[AddComponentMenu ("2DxFX/Standard/PatternAdditive")]
[System.Serializable]
public class _2dxFX_PatternAdditive : MonoBehaviour
{
	[HideInInspector] public Material ForceMaterial;
	[HideInInspector] public bool ActiveChange=true;
	private string shader = "2DxFX/Standard/PatternAdditive";
	[HideInInspector] [Range(0, 1)] public float _Alpha = 1f;

	[HideInInspector] public Texture2D __MainTex2; 
	[HideInInspector] public float _OffsetX;
	[HideInInspector] public float _OffsetY;
	
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
		ShaderChange = 0;
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
			ActiveChange=false;

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

			if ((_AutoScrollX == false) && (_AutoScrollY == false))
			{
				GetComponent<Renderer>().sharedMaterial.SetFloat("_OffsetX",_OffsetX);
				GetComponent<Renderer>().sharedMaterial.SetFloat("_OffsetY",_OffsetY);
			}
			
			if ((_AutoScrollX == true) && (_AutoScrollY == false))
			{
				_AutoScrollCountX+=_AutoScrollSpeedX*Time.deltaTime;
				GetComponent<Renderer>().sharedMaterial.SetFloat("_OffsetX",_AutoScrollCountX);
				GetComponent<Renderer>().sharedMaterial.SetFloat("_OffsetY",_OffsetY);
			}
			if ((_AutoScrollX == false) && (_AutoScrollY == true))
			{
				_AutoScrollCountY+=_AutoScrollSpeedY*Time.deltaTime;
				GetComponent<Renderer>().sharedMaterial.SetFloat("_OffsetX",_OffsetX);
				GetComponent<Renderer>().sharedMaterial.SetFloat("_OffsetY",_AutoScrollCountY);
			}
			if ((_AutoScrollX == true) && (_AutoScrollY == true))
			{
				_AutoScrollCountX+=_AutoScrollSpeedX*Time.deltaTime;
				GetComponent<Renderer>().sharedMaterial.SetFloat("_OffsetX",_AutoScrollCountX);
				_AutoScrollCountY+=_AutoScrollSpeedY*Time.deltaTime;
				GetComponent<Renderer>().sharedMaterial.SetFloat("_OffsetY",_AutoScrollCountY);
			}
			if (_AutoScrollCountX > 1) _AutoScrollCountX = 0;
			if (_AutoScrollCountX < -1) _AutoScrollCountX = 0;
			if (_AutoScrollCountY > 1) _AutoScrollCountY = 0;
			if (_AutoScrollCountY < -1) _AutoScrollCountY = 0;
		}

		
	}
	
	void OnDestroy()
	{
		if ((Application.isPlaying == false) && (Application.isEditor == true)) {

			if (ForceMaterial != null && tempMaterial!=null)
			{
				DestroyImmediate(tempMaterial);
			}

			if (gameObject.activeSelf) {
				GetComponent<Renderer>().sharedMaterial = defaultMaterial;
				GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
			}
		}
	}
	void OnDisable()
	{ 
		if (ForceMaterial!=null && tempMaterial!=null)
		{ 
			DestroyImmediate(tempMaterial);
			GetComponent<Renderer>().sharedMaterial = defaultMaterial;
			GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
		}

	}

	void OnEnable()
	{

		defaultMaterial = new Material(Shader.Find("Sprites/Default"));
		 

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
		if (__MainTex2)	
		{
			__MainTex2.wrapMode= TextureWrapMode.Repeat;
			GetComponent<Renderer>().sharedMaterial.SetTexture ("_MainTex2", __MainTex2);
		}
	}
}




#if UNITY_EDITOR
[CustomEditor(typeof(_2dxFX_PatternAdditive)),CanEditMultipleObjects]
public class _2dxFX_PatternAdditive_Editor : Editor
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
		
		_2dxFX_PatternAdditive _2dxScript = (_2dxFX_PatternAdditive)target;
	
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


			Texture2D icone = Resources.Load ("2dxfx-icon-color") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("__MainTex2"), new GUIContent("Texture Pattern", icone, "Change the value of the posterize effect"));
			if (_2dxScript.__MainTex2)
			{
			icone = Resources.Load ("2dxfx-icon-size_x") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_OffsetX"), new GUIContent("Offset X", icone, "Change the value of the posterize effect"));

			icone = Resources.Load ("2dxfx-icon-size_y") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_OffsetY"), new GUIContent("Offset Y", icone, "Change the value of the posterize effect"));

			icone = Resources.Load ("2dxfx-icon-value") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_AutoScrollX"), new GUIContent("Auto Scroll X", icone, "Change the value of the posterize effect"));
			if (_2dxScript._AutoScrollX)
			{
				icone = Resources.Load ("2dxfx-icon-time") as Texture2D;
				EditorGUILayout.PropertyField(m_object.FindProperty("_AutoScrollSpeedX"), new GUIContent("Auto Scroll Speed X", icone, "Change the value of the posterize effect"));
			}

			icone = Resources.Load ("2dxfx-icon-value") as Texture2D;
			EditorGUILayout.PropertyField(m_object.FindProperty("_AutoScrollY"), new GUIContent("Auto Scroll Y", icone, "Change the value of the posterize effect"));

			if (_2dxScript._AutoScrollY)
			{
				icone = Resources.Load ("2dxfx-icon-time") as Texture2D;
				EditorGUILayout.PropertyField(m_object.FindProperty("_AutoScrollSpeedY"), new GUIContent("Auto Scroll Speed Y", icone, "Change the value of the posterize effect"));
			}
			}
			else
			{
				EditorGUILayout.LabelField(new GUIContent("**You must add a sprite on the Texture Pattern field**", "You need to add a sprite here in order to use the pattern fx, you can use the same sprite if you want"));
			}

		




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