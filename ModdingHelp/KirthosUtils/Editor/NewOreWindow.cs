using System.Linq;
using UnityEditor;
using UnityEngine;

/*
 * Copyright (c) 2018 [Kirthos]
 * 
 * Created by Kirthos 07/20/2018
 */

public class NewOreWindow : EditorWindow
{
    string OreName = "";
    Material material;
    BlockSetContainer container;

    public string folderPath;

    void OnGUI()
    {
        if (folderPath == null && Selection.activeObject != null)
        {
            folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        }
        else if (Selection.activeObject == null)
        {
            Debug.LogError("[KirthosUtils] - Error when trying to get the folder path.");
            Close();
        }

        GUILayout.Label("Create a new ore", EditorStyles.boldLabel);
        OreName = EditorGUILayout.TextField("Ore Name", OreName);

        GUILayout.Label("Ore Material");
        material = EditorGUILayout.ObjectField(material, typeof(Material), false) as Material;

        GUILayout.Label("Block set container object");
        container = EditorGUILayout.ObjectField(container, typeof(BlockSetContainer), true) as BlockSetContainer;

        if (folderPath != null && GUILayout.Button("Create"))
        {
            GameObject mySelGO = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EcoModKit/KirthosUtils/Prefabs/SampleOre.prefab", typeof(GameObject))) as GameObject;
            mySelGO.name = mySelGO.name.Replace("Sample", OreName);
            mySelGO.name = mySelGO.name.Remove(mySelGO.name.Length - "(Clone)".Length);
            foreach (Transform child in mySelGO.transform)
                child.gameObject.name = child.gameObject.name.Replace("Sample", OreName);

            foreach (Transform child in mySelGO.transform)
            {
                child.GetComponent<HitObjectType>().ObjectType = child.GetComponent<HitObjectType>().ObjectType.Replace("Sample", OreName);
                child.GetComponent<MeshRenderer>().material = material;
            }
            mySelGO.SetActive(false);
            string oreBuilderPath = folderPath + "/" + OreName + "Ore.asset";
            Debug.Log(folderPath + ":::" + oreBuilderPath + ":::" + Selection.activeObject);
            AssetDatabase.CopyAsset("Assets/EcoModKit/KirthosUtils/BuilderScript/SampleOre.asset", oreBuilderPath);
            BlockSet blockSet = AssetDatabase.LoadAssetAtPath(oreBuilderPath, typeof(BlockSet)) as BlockSet;
            foreach(Block block in blockSet.Blocks)
            {
                block.Name = block.Name.Replace("Sample", OreName);
                block.Material = material;
            }
            var lst = container.blockSets.ToList();
            lst.Add(blockSet);
            container.blockSets = lst.ToArray();
            PrefabUtility.CreatePrefab(folderPath + "/" + OreName + "Ore.prefab", mySelGO);
            Close();
        }
    }
}