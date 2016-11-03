#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class ShaderChanger : EditorWindow
{

    private bool UseMobileShader;
    private Shader MobileShader;
    private Shader PCShader;
    private string MobileShader_Diffuse = "_MainTex";
    private string PCShader_Diffuse = "_Diffuse_Texture";
    private List<Material> _matList = new List<Material>();
    private Renderer[] _renderers;
    private bool switched;

    [MenuItem("Window/Shader Changer")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(ShaderChanger));
    }

    void OnEnable()
    {
        MobileShader = Shader.Find("Mobile/Diffuse");
        PCShader = Shader.Find("UnityVR/Puni_Shader");
    }

    void OnGUI()
    {
        EditorMenu();
    }

    public void ListMaterials()
    {
        _matList.Clear();
        Debug.Log("Listing all materials");
        //Find and list all the materials in the scene that have a shader
        _renderers = (Renderer[])Resources.FindObjectsOfTypeAll(typeof(Renderer));
        foreach (Renderer renderer in _renderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat != null && mat.shader != null && !_matList.Contains(mat))
                {
                    _matList.Add(mat);
                }
            }
        }
        Debug.Log("Material count: " + _matList.Count);
    }

    public void ChangeShader()
    {
        if (_matList.Count == 0)
        {
            Debug.Log("Material list is empty");
            ListMaterials();
        }
        if (UseMobileShader == true) { Debug.Log("Changing to MobileShader"); }
        if (UseMobileShader != true) { Debug.Log("Changing to PCShader"); }

        //"Check through all the materials in the list for shaders.
        foreach (Material mat in _matList)
        {
            if (UseMobileShader == true)
            {
                if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader == PCShader)
                {
                    Debug.Log("Shader " + PCShader.name + " found in material:" + mat.name);
                    Texture t = mat.GetTexture(PCShader_Diffuse);
                    mat.shader = MobileShader;
                    mat.SetTexture(MobileShader_Diffuse, t);
                    Debug.Log("Shader changed for " + mat.name + " to " + MobileShader.name);
                }
            }
            if (UseMobileShader != true)
            {
                if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader == MobileShader)
                {
                    Debug.Log("Shader " + MobileShader.name + " found in material:" + mat.name);
                    Texture t = mat.GetTexture(MobileShader_Diffuse);
                    mat.shader = PCShader;
                    mat.SetTexture(PCShader_Diffuse, t);
                    Debug.Log("Shader changed for " + mat.name + " to " + MobileShader.name);
                }
            }
        }
    }


    void EditorMenu()
    {
        GUILayout.Label("ShaderChanger", EditorStyles.boldLabel);
        UseMobileShader = EditorGUILayout.Toggle("Use MobileShader", UseMobileShader);
        MobileShader = (Shader)EditorGUILayout.ObjectField("MobileShader", MobileShader, typeof(Shader), true);
        PCShader = (Shader)EditorGUILayout.ObjectField("PCShader", PCShader, typeof(Shader), true);
        MobileShader_Diffuse = EditorGUILayout.TextField("Mobile Shader Diffuse", MobileShader_Diffuse);
        PCShader_Diffuse = EditorGUILayout.TextField("PC Shader Diffuse", PCShader_Diffuse);
        GUILayout.Label("");
        if (GUILayout.Button("Refresh Materials"))
        {
            ListMaterials();
        }
        if (GUILayout.Button("Change Shaders"))
        {
            ChangeShader();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnFocus()
    {
    }

    void OnDestroy()
    {
    }

}
#endif