//////////////////////////////////////////////////////////////////////////
//
// Find Asset Reference
// 
// Created by CY.
//
// Copyright 2011 FourNext Group
// All rights reserved
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class FN_FindAssetReferences : MonoBehaviour
{
	public static bool needRefresh = true;
	public static bool IgnoreCacheUpdate
	{
		set { EditorPrefs.SetBool("FNAssetFindReferences", value);	}		
		get { return EditorPrefs.GetBool("FNAssetFindReferences", false); } 
	}

	public class AssetData
	{
		public string AssetPath = "";
		public string[] AssetDependencies = new string[0];
		
		public bool Find(string assetPath)
		{
			if( assetPath.ToLower() == AssetPath.ToLower() )
				return false;
			
			foreach(string s in AssetDependencies)
			{
				if( assetPath.ToLower() == s.ToLower() )
					return true;
			}
			
			return false;
		}
	}
	private static List<AssetData> mAssetCache = new List<AssetData>();
	
	[MenuItem("CONTEXT/Object/Find Asset References")]
	static void FindReferences(MenuCommand command)
	{
		List<GameObject> findedList = new List<GameObject>();
		List<GameObject> findedPrefabList = new List<GameObject>();
	
		Object obj = command.context;

		// for prefab
		if( obj.GetType() == typeof(Transform) )
			obj = ((Transform)obj).gameObject;
		
		FindInScene(obj, ref findedList);
		FindInProject(AssetDatabase.GetAssetPath(obj), ref findedPrefabList);

        FN_FindAssetReferencesWindow refWindow = (FN_FindAssetReferencesWindow)EditorWindow.GetWindow(typeof(FN_FindAssetReferencesWindow));
		refWindow.SetObject(obj, findedList, findedPrefabList);
	}

    [MenuItem("CONTEXT/Object/Find Asset References", true)]
	static bool FindReferencesValidate(MenuCommand command)
	{
		// only Asset files
		Object o = command.context;
		return (AssetDatabase.Contains(o));
	}
	
	static void WriteLog(string log)
	{
		Debug.Log(log);
	}
	static void WriteLog(string fmt, params object[] args)
	{
		WriteLog(string.Format(fmt, args));
	}
	
	//-----------------------------------------------------------------------------
	static void FindInProject(string findObjectAssetPath, ref List<GameObject> findedList)
	{
        if (needRefresh && IgnoreCacheUpdate == false)
        {
            if (EditorUtility.DisplayDialog("CacheUpdate", "Asset changed! update asset cache ?", "OK", "Cancel"))
                MakeAssetCache();
        }

        if (mAssetCache.Count <= 0 && IgnoreCacheUpdate == false)
        {
            if (EditorUtility.DisplayDialog("CacheUpdate", "Asset Cache is empty. Do you update cache?", "OK", "Cancel"))
                MakeAssetCache();
        }
		
		foreach(AssetData ad in mAssetCache)
		{
			if( ad.Find(findObjectAssetPath) )
			{
				GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(ad.AssetPath, typeof(GameObject));
				if( prefab != null )
				{
					if( findedList.Contains(prefab) == false )
					{
						findedList.Add(prefab);	
					}
				}
			}				
		}
	}
	
	//-----------------------------------------------------------------------------
	static void FindInScene(Object findObject, ref List<GameObject> findedList)
	{
		Object[] objs = FindObjectsOfType(typeof(GameObject));
		foreach(GameObject go in objs)
		{
			// specific case monoscript
            if( (findObject is GameObject && go == findObject) || CheckInGameObject(go, findObject) )
			{
				if( findedList.Contains(go) == false )
					findedList.Add(go);
			}
		}
	}
	
	//-----------------------------------------------------------------------------
	static bool CheckInGameObject(GameObject obj, Object findObject)
	{
		System.Type type = findObject.GetType();
		if( type == typeof(MonoScript) )
		{
			type = ((MonoScript)findObject).GetClass();
			if( obj.GetComponents(type).Length > 0 )
				return true;
		}
		else
		{
			Component[] comps = obj.GetComponents(typeof(Component));
			foreach(Component comp in comps)
			{
				if( CheckInComponent(comp, findObject) )
					return true;
			}
		}
		return false;
	}

	//-----------------------------------------------------------------------------
	static bool CheckInComponent(Component comp, Object findObject)
	{
		if (comp == null)
			return false;

		PropertyInfo pi = null;
		try
		{
			MemberInfo[] meminfos = comp.GetType().GetMembers();
			foreach(MemberInfo mi in meminfos)
			{
				// finding in field
				if( mi.MemberType == MemberTypes.Field )
				{
					FieldInfo fi = (FieldInfo)mi;
					System.TypeCode tcode = System.Type.GetTypeCode(fi.FieldType);
					
					if( tcode == System.TypeCode.Object )
					{
						object obj = fi.GetValue(comp);
						if( obj != null )
						{
							if( obj.GetType() == findObject.GetType() && obj == findObject )
							{
								return true;
							}
							
							if( fi.FieldType.IsArray )
							{				
								if( CheckInArray( fi.Name, (System.Array)obj, findObject) )
									return true;
							}
						}
					}
				}
				
				if( mi.MemberType == MemberTypes.Property )
				{
					pi = (PropertyInfo)mi;

                    string excludeName = pi.Name.ToLower();
                    if (excludeName == "mesh" || excludeName == "material" || excludeName == "materials")
                        continue;

                    System.TypeCode tcode = System.Type.GetTypeCode(pi.PropertyType);
					if( tcode == System.TypeCode.Object )
					{
						if( pi.CanWrite && pi.GetSetMethod().IsStatic==false )
						{
							object obj = pi.GetValue(comp, null);
                            if (obj == null)
                                continue;

							if( obj.GetType() == findObject.GetType() && obj == findObject )
							{
								return true;
							}
							
							if( pi.PropertyType.IsArray )
							{
								if( CheckInArray(pi.Name, (System.Array)obj, findObject) )
									return true;
							}
						}
					}
				}
			}
		}
		catch(System.Exception ex)
		{
			string errorStr = ex.ToString();
			if( pi != null )
				errorStr = "Property:" + pi.Name + "-" + ex.ToString();
			
			Debug.LogWarning("comp:"+comp.name+ "-" + errorStr);
		}
		
		return false;
	}
	
	//-----------------------------------------------------------------------------
	static bool CheckInArray(string name, System.Array array, Object findObject)
	{
		int index = 0;
		foreach(object obj in array)
		{
			if( obj != null && obj.GetType() == findObject.GetType() && obj == findObject )
			{
				return true;
			}
			
			index++;
		}
		return false;
	}

    //-----------------------------------------------------------------------------
    public static string AssetCacheInfo()
    {
        return string.Format("Prefab count in AssetCache : {0}", mAssetCache.Count);
    }

	//-----------------------------------------------------------------------------
	public static void MakeAssetCache()
	{
		List<string> pathlist = new List<string>();
		RetrievePrefab_inProject(ref pathlist);
		
		mAssetCache.Clear();

		int count = 0;
		float progress = 0.0f;
		foreach(string p in pathlist)
		{
			count++;
			progress = count / pathlist.Count;
			if( EditorUtility.DisplayCancelableProgressBar("Make cache", p, progress) )
				break;
			
			AssetData ad = new AssetData();
			ad.AssetPath = p;
			string[] srcPaths = new string[1];
			srcPaths[0] = p;
			ad.AssetDependencies = AssetDatabase.GetDependencies(srcPaths);
			
			mAssetCache.Add(ad);
		}
		
		if( mAssetCache.Count != pathlist.Count )
			needRefresh = true;
		else
			needRefresh = false;
		
		EditorUtility.ClearProgressBar();
	}
	
	//-----------------------------------------------------------------------------
	static void RetrievePrefab_inProject(ref List<string> fileList)
	{
		string[] files;

		Stack stack = new Stack();
		stack.Push(Application.dataPath);

		while (stack.Count > 0)
		{
			string dir = (string)stack.Pop();

			try
			{
				files = Directory.GetFiles(dir, "*.prefab");
				for (int i = 0; i < files.Length; ++i)
				{
					files[i] = files[i].Substring(Application.dataPath.Length - 6);	// remove name "Assets"
					fileList.Add(files[i]);					
				}

				foreach (string dn in Directory.GetDirectories(dir))
				{
					stack.Push(dn);
				}
			}
			catch
			{
				Debug.LogError("Could not access folder: \"" + dir + "\"");
			}
		}
	}	
}

