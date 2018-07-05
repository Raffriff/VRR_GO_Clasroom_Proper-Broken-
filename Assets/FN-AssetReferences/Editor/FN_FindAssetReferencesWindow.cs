//////////////////////////////////////////////////////////////////////////
//
// FindAssetReferences Window and selection 
// 
// Created by CY
//
// Copyright 2011 FourNext Group
// All rights reserved
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class FN_FindAssetReferencesWindow : EditorWindow
{
	private List<GameObject> findedObjInScene = new List<GameObject>();
	private List<GameObject> findedObjInProject = new List<GameObject>();
	private Object sourceObject = null;
    private string currentSceneName = "";

	public Rect sceneObjectsWindowRect;
	public Rect assetWindowRect;
    Vector2 sceneScrollPos = Vector2.zero;
	Vector2 assetScrollPos = Vector2.zero;

    public Rect ToolRect;
	public Rect ContentRect;
	
    private bool IgnoreAssetCacheUpdate = false;

	[MenuItem("Window/Asset References Window")]
	static void Init()
	{
        FN_FindAssetReferencesWindow window = (FN_FindAssetReferencesWindow)EditorWindow.GetWindow(typeof(FN_FindAssetReferencesWindow));
		if( window == null )
		{
            window = new FN_FindAssetReferencesWindow();
		}

        window.IgnoreAssetCacheUpdate = FN_FindAssetReferences.IgnoreCacheUpdate;
		
		window.ToolRect = new Rect(0f, 0f, 0f, 40f);
		window.ContentRect = new Rect(5f, 40f, 0f, 0f);
		
		window.sceneObjectsWindowRect = new Rect(0f, 0f, 0f, 0f);
		window.assetWindowRect = new Rect(0f, 0f, 0f, 0f);
		window.Show();
	}
	
	//-----------------------------------------------------------------------------
	void OnGUI()
	{
		float width = position.width;
		float height = position.height;

        ToolRect.width = width;
        GUILayout.BeginArea(ToolRect);

        if (GUI.Button(new Rect(5, 5, 200, 30), "Asset Cache update NOW!"))
        {
            FN_FindAssetReferences.MakeAssetCache();
        }

        IgnoreAssetCacheUpdate = GUI.Toggle(new Rect(210, 15, 200, 30), IgnoreAssetCacheUpdate, "Ignore auto update Asset Cache!");
        FN_FindAssetReferences.IgnoreCacheUpdate = IgnoreAssetCacheUpdate;

        GUI.Label(new Rect(420, 15, 300, 30), "( " + FN_FindAssetReferences.AssetCacheInfo() + " )");  
        GUILayout.EndArea();

		string sourceObjName = "Empty";
		if( sourceObject != null )
		{
			sourceObjName = AssetDatabase.GetAssetPath(sourceObject);
		}
		
		ContentRect.x = 5f;
		ContentRect.y = 40f;
		ContentRect.width = width-10f;
		ContentRect.height = height-60f;
        GUILayout.BeginArea(ContentRect);
        GUILayout.Label("Source Object");
		if( GUILayout.Button(sourceObjName) )
		{
			if( sourceObject != null )
			{
				EditorGUIUtility.PingObject(sourceObject);
			}
		}
		
		BeginWindows();
		
		sceneObjectsWindowRect.x = 0f;
        sceneObjectsWindowRect.y = 50f;
		sceneObjectsWindowRect.width = ContentRect.width*0.5f-5f; 
		sceneObjectsWindowRect.height = ContentRect.height-60f;
		sceneObjectsWindowRect = GUILayout.Window(1, sceneObjectsWindowRect, DoSceneObjectWindow, "Scene:"+currentSceneName);
				
		assetWindowRect.x = ContentRect.width*0.5f+5f;
        assetWindowRect.y = 50f;
		assetWindowRect.width = ContentRect.width*0.5f-10f;
		assetWindowRect.height = ContentRect.height-60f;
		assetWindowRect = GUILayout.Window(2, assetWindowRect, DoAssetWindow, "Project");
		
		EndWindows();

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, height - 20, width, 20));
        GUILayout.Label(" Copyright 2011 FourNext Group. All rights reserved."); 
        GUILayout.EndArea();
	}
	
	//-----------------------------------------------------------------------------
	void DoSceneObjectWindow(int windowId)
	{
		if( GUILayout.Button("Clear") )
		{
			findedObjInScene.Clear();
		}

        if( currentSceneName != EditorApplication.currentScene )
		{
			currentSceneName = EditorApplication.currentScene;
			findedObjInScene.Clear();
		}
		
		DrawButtons(ref findedObjInScene, false);
	}
	
	//-----------------------------------------------------------------------------
	void DoAssetWindow(int windowId)
	{
		if( GUILayout.Button("Clear") )
		{
			findedObjInProject.Clear();
		}
		
		DrawButtons(ref findedObjInProject, true);
	}
	
	//-----------------------------------------------------------------------------
	void DrawButtons( ref List<GameObject> objList, bool inProject)
	{
		if( objList.Count <= 0 )
			return;

        if (inProject)
            assetScrollPos = GUILayout.BeginScrollView(assetScrollPos);
        else
            sceneScrollPos = GUILayout.BeginScrollView(sceneScrollPos);
		
		for( int i=0; i<objList.Count; i++ )
		{
			string str = objList[i].name;
			if( GUILayout.Button(str) )
			{
				SelectObject(i, inProject);
			}
		}
		
		GUILayout.EndScrollView();
	}
	
	//-----------------------------------------------------------------------------
	public void SetObject( Object srcObject, List<GameObject> _objList, List<GameObject> _prefabList )
	{
		findedObjInScene.Clear();
		findedObjInProject.Clear();
		
		foreach(GameObject obj in _objList)
			findedObjInScene.Add(obj);
				
		foreach(GameObject obj in _prefabList)
			findedObjInProject.Add(obj);
		
		sourceObject = srcObject;

        currentSceneName = EditorApplication.currentScene;

		Init();
	}
	
	//-----------------------------------------------------------------------------
	void SelectObject(int index, bool inProject)
	{
		GameObject obj = null;
		if( inProject )
		{
			if( findedObjInProject.Count > 0 && index < findedObjInProject.Count )
				obj = findedObjInProject[index];
		}
		else
		{
            if (currentSceneName != EditorApplication.currentScene)
            {
                EditorUtility.DisplayDialog("Warning", "incorrect scene!", "OK");
                return;
            }

			if( findedObjInScene.Count > 0 && index < findedObjInScene.Count )
				obj = findedObjInScene[index];
		}
		
		if( obj != null )
		{
			EditorGUIUtility.PingObject(obj);
			Selection.activeGameObject = obj;
		}
	}
}
