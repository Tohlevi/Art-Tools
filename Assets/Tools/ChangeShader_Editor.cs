#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class ShaderChanger : EditorWindow
{

    private bool useMobileShader;
    private Shader mobileShader;
    private Shader PCShader;
    private string mobileShader_Diffuse = "_MainTex";
    private string PCShader_Diffuse = "_MainTex";
    private List<Material> _matList = new List<Material>();
    private Renderer[] _renderers;
    //private bool switched;

    //Settings:
    private bool autoToggleShaders = true;
    private bool refreshMatListOnChange = true;

    //Automation

    [MenuItem("Window/Shader Changer")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(ShaderChanger));
    }

    void OnEnable()
    {
        mobileShader = Shader.Find("Mobile/Diffuse");
        PCShader = Shader.Find("Standard");
    }

    void OnGUI()
    {
        EditorMenu();
    }


    #region Functions
    private void ListMaterials()
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

    private void ChangeShader()
    {
        if (_matList.Count == 0)
        {
            Debug.Log("Material list is empty");
            ListMaterials();
        }
        if (useMobileShader == true) { Debug.Log("Changing to MobileShader"); }
        if (useMobileShader != true) { Debug.Log("Changing to PCShader"); }

        //"Check through all the materials in the list for shaders.
        foreach (Material mat in _matList)
        {
            if (useMobileShader == true)
            {
                if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader == PCShader)
                {
                    Debug.Log("Shader " + PCShader.name + " found in material:" + mat.name);
                    Texture t = mat.GetTexture(PCShader_Diffuse);
                    mat.shader = mobileShader;
                    mat.SetTexture(mobileShader_Diffuse, t);
                    Debug.Log("Shader changed for " + mat.name + " to " + mobileShader.name);
                }
            }
            if (useMobileShader != true)
            {
                if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader == mobileShader)
                {
                    Debug.Log("Shader " + mobileShader.name + " found in material:" + mat.name);
                    Texture t = mat.GetTexture(mobileShader_Diffuse);
                    mat.shader = PCShader;
                    mat.SetTexture(PCShader_Diffuse, t);
                    Debug.Log("Shader changed for " + mat.name + " to " + mobileShader.name);
                }
            }
        }
    }

    #endregion

    #region Editor

    private void EditorMenu()
    {
        GUILayout.Label("ShaderChanger", EditorStyles.boldLabel);
        useMobileShader = EditorGUILayout.Toggle("Use MobileShader", useMobileShader);
        mobileShader = (Shader)EditorGUILayout.ObjectField("MobileShader", mobileShader, typeof(Shader), true);
        PCShader = (Shader)EditorGUILayout.ObjectField("PCShader", PCShader, typeof(Shader), true);
        mobileShader_Diffuse = EditorGUILayout.TextField("Mobile Shader Diffuse", mobileShader_Diffuse);
        PCShader_Diffuse = EditorGUILayout.TextField("PC Shader Diffuse", PCShader_Diffuse);
        GUILayout.Label("");
        if (GUILayout.Button("Refresh Materials"))
        {
            ListMaterials();
        }
        if (GUILayout.Button("Change Shaders"))
        {
            if (refreshMatListOnChange == true) ListMaterials();
            ChangeShader();
            if (autoToggleShaders == true) AutoToggleShaders();
        }
        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("Settings:");
        autoToggleShaders = EditorGUILayout.Toggle("Auto Toggle Shader", autoToggleShaders);
        refreshMatListOnChange = EditorGUILayout.Toggle("Auto Refresh Materials", refreshMatListOnChange);
    }

    #endregion

    #region Settings

    private void AutoToggleShaders()
    {
        if (useMobileShader == true) useMobileShader = false;
        else useMobileShader = true;
    }

    #endregion

    #region Unity
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
    #endregion
}
#endif