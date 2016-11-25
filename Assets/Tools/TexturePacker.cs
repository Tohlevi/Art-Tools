/*
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.IO;


public class TexturePacker : EditorWindow
{

    private List<Material> _matList = new List<Material>();
    private List<Texture2D> _texList = new List<Texture2D>();
    private Renderer[] _renderers;
    private string diffuseTexture = "_MainTex";
    private Rect[] rects;
    private int atlas_Size = 2048;

    [MenuItem("Window/Texture Packer")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(TexturePacker));
    }

    void OnEnable()
    {
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

    public void ListTextures()
    {
        if (_matList.Count == 0) ListMaterials(); //If Material List is empty, Refresh Material List.
        foreach(Material mat in _matList)
        {
            Texture2D difTex = mat.GetTexture(diffuseTexture) as Texture2D;
            if (difTex != null && !_texList.Contains(difTex))
            {
                _texList.Add(difTex);
                Debug.Log("Added " + difTex.name + " to _texList");
            }
        }
        Debug.Log("Texture count: " + _texList.Count);
    }

    public void PackTextures()
    {
        ListTextures();
        Texture2D atlas = new Texture2D(atlas_Size, atlas_Size);
        rects = atlas.PackTextures(_texList.ToArray(), 2, atlas_Size);
        byte[] bytes = atlas.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Texture/master_Texture.png", bytes);
    }


    void EditorMenu()
    {
        GUILayout.Label("Texture Packer", EditorStyles.boldLabel);
        diffuseTexture = EditorGUILayout.TextField("DiffuseTexture", diffuseTexture);
        GUILayout.Label("");
        if (GUILayout.Button("Refresh Materials"))
        {
            Debug.Log("Listing materials");
            ListMaterials();
        }
        if (GUILayout.Button("List Textures"))
        {
            Debug.Log("Listing textures");
            ListTextures();
        }
        if (GUILayout.Button("Pack Textures"))
        {
            PackTextures();
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
*/