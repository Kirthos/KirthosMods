using UnityEditor;
using UnityEngine;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/20/2018
 */

public class KirthosUtils
{
	[MenuItem("KirthosUtils/FixInvisibleObjects")]
	private static void FixInvisibleObjects()
	{
		Shader.EnableKeyword ("NO_CURVE");
	}

    [MenuItem("Assets/KirthosUtils/Create new Ore", false, 0)]
    private static void CreateNewOre()
    {
        NewOreWindow window = EditorWindow.GetWindow(typeof(NewOreWindow)) as NewOreWindow;
        window.folderPath = null;
    }
}


