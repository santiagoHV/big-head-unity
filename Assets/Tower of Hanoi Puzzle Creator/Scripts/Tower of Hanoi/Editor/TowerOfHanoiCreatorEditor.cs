using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace TowerOfHanoiPuzzle
{
    [CustomEditor(typeof(TowerOfHanoiCreator))]
    public class TowerOfHanoiCreatorEditor : Editor
    {
        [MenuItem("GameObject/3D Object/Tower of Hanoi")]
        static void CreateTowerOfHanoi()
        {
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Tower of Hanoi.prefab");
            string name = path.Substring(7, path.Length - "Assets/".Length - ".prefab".Length);
            GameObject go = new GameObject(name, typeof(TowerOfHanoiCreator), typeof(TowerOfHanoi), typeof(MeshFilter), typeof(MeshRenderer), typeof(AudioSource));
            Material defaultMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

            var baseData = TowerOfHanoiBaseData.Create();
            baseData.UpdateMesh();
            go.GetComponent<MeshFilter>().sharedMesh = baseData.baseMesh;
            go.GetComponent<MeshRenderer>().sharedMaterials = new Material[] {defaultMaterial, defaultMaterial, defaultMaterial, defaultMaterial};

            var disksData = TowerOfHanoiDisksData.Create();
            disksData.baseData = baseData;
            disksData.UpdateMesh();
            GameObject disks = new GameObject("Disks", typeof(MeshFilter), typeof(MeshRenderer));
            disks.transform.SetParent(go.transform);
            disks.GetComponent<MeshFilter>().sharedMesh = disksData.mesh;
            disks.GetComponent<MeshRenderer>().sharedMaterials = new Material[] { defaultMaterial, defaultMaterial, defaultMaterial, defaultMaterial };
            disks.transform.localPosition = baseData.GetRodPosition(0);

            var creator = go.GetComponent<TowerOfHanoiCreator>();
            creator.BaseData = baseData;
            creator.DisksData = disksData;

            var tower = go.GetComponent<TowerOfHanoi>();
            tower.baseData = baseData;
            tower.disksData = disksData;

            var emptyPrefab = PrefabUtility.CreateEmptyPrefab(path);
            AssetDatabase.AddObjectToAsset(baseData.baseMesh, emptyPrefab);
            AssetDatabase.AddObjectToAsset(baseData, emptyPrefab);
            AssetDatabase.AddObjectToAsset(disksData.mesh, emptyPrefab);
            AssetDatabase.AddObjectToAsset(disksData, emptyPrefab);
            AssetDatabase.SaveAssets();

            LoadTowerOfHanoiFields(tower);

            var prefab = PrefabUtility.ReplacePrefab(go, emptyPrefab);

            baseData.disksData = disksData;
            disksData.baseData = baseData;
            disksData.disksTransform = prefab.transform.GetChild(0);

            DestroyImmediate(go);
            go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            Undo.RegisterCreatedObjectUndo(go, "Create Tower of Hanoi");

            Selection.activeObject = go;
        }

        static string GetTowerOfHanoiFolderPath()
        {
            var guids = AssetDatabase.FindAssets("TowerOfHanoiCreator t:Script");
            for (int n = 0; n < guids.Length; ++n)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[n]);
                var parts = path.Split('/');
                if (parts.Length > 4 &&
                   parts[parts.Length - 1] == "TowerOfHanoiCreator.cs" &&
                   parts[parts.Length - 2] == "Tower of Hanoi" &&
                   parts[parts.Length - 3] == "Scripts" &&
                   parts[parts.Length - 4] == "Tower of Hanoi Puzzle Creator")
                {
                    return path.Substring(0, path.Length - "Scripts/Tower of Hanoi/TowerOfHanoiCreator.cs".Length);
                }
            }
            return null;
        }
        private static void LoadTowerOfHanoiFields(TowerOfHanoi tower)
        {
            string path = GetTowerOfHanoiFolderPath();
            if (path == null) return;

            tower.nearAnimation = AssetDatabase.LoadAssetAtPath(path + "Animations/Near Animation.anim", typeof(AnimationClip)) as AnimationClip;
            tower.farAnimation = AssetDatabase.LoadAssetAtPath(path + "Animations/Far Animation.anim", typeof(AnimationClip)) as AnimationClip;
            GameObject aux = AssetDatabase.LoadAssetAtPath(path + "Prefabs/Magic Particles/Wood Tower Magic Particle.prefab", typeof(GameObject)) as GameObject;
            if (aux != null)
                tower.magicParticlePrefab = aux.GetComponent<MagicParticle>();

            tower.deselectStartRodAudioClips[0] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Deselect 0.ogg", typeof(AudioClip)) as AudioClip;
            tower.deselectStartRodAudioClips[1] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Deselect 1.ogg", typeof(AudioClip)) as AudioClip;
            tower.deselectStartRodAudioClips[2] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Deselect 2.ogg", typeof(AudioClip)) as AudioClip;

            tower.selectStartRodAudioClips[0] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Select 0.ogg", typeof(AudioClip)) as AudioClip;
            tower.selectStartRodAudioClips[1] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Select 1.ogg", typeof(AudioClip)) as AudioClip;
            tower.selectStartRodAudioClips[2] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Select 2.ogg", typeof(AudioClip)) as AudioClip;

            tower.selectEndRodAudioClips[0] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Select End 0.ogg", typeof(AudioClip)) as AudioClip;
            tower.selectEndRodAudioClips[1] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Select End 1.ogg", typeof(AudioClip)) as AudioClip;
            tower.selectEndRodAudioClips[2] = AssetDatabase.LoadAssetAtPath(path + "Sounds/Piano/Select End 2.ogg", typeof(AudioClip)) as AudioClip;
        }

        TowerOfHanoiBaseData baseData;
        TowerOfHanoiDisksData disksData;
        TowerOfHanoiCreator prefabTarget, 
                          instanceTarget;
        static GUIContent fitDistanceGUIContent = new GUIContent("Distance = No disk overlap", "Adjust distance between rods to perfectly fit the two biggest disks side by side.");
        static GUIContent applyMaterialToAllGUIContent = new GUIContent("Apply to All", "Assign the material above to each disk.");
        static GUIContent duplicateMaterialsGUIContent = new GUIContent("Duplicate Materials", "Makes an individual of copy of each of the disks materials and then assign them back to the disks.");

        static int tab = 0;
        bool inspectingPrefab;
        static bool[] disksInspectorFoldout = new bool[6],
                      rodsInspectorFoldout = new bool[5],
                      baseInspectorFoldout = new bool[7];
       
        void OnEnable()
        {
            inspectingPrefab = (PrefabUtility.GetPrefabType(target) == PrefabType.Prefab);
            if (inspectingPrefab)
            {
                prefabTarget = target as TowerOfHanoiCreator;
                instanceTarget = prefabTarget;
            }
            else
            {
                instanceTarget = target as TowerOfHanoiCreator; ;
                prefabTarget = PrefabUtility.GetPrefabParent(target) as TowerOfHanoiCreator;
            }
            if (prefabTarget != null)
            {
                baseData = prefabTarget.BaseData;
                disksData = prefabTarget.DisksData;
            }
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }
        void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoRedoPerformed;
        }
        void UndoRedoPerformed()
        {
            if (baseData != null)
                baseData.UpdateMesh();
            if (disksData != null)
                disksData.UpdateMesh();
        }
        
        void AdjustMeshRenderer(MeshRenderer meshRenderer)
        {
            if (meshRenderer != null)
            {
                var materials = meshRenderer.sharedMaterials;

                if (materials.Length > disksData.NumberOfDisks)
                {
                    Array.Resize(ref materials, disksData.NumberOfDisks);
                }
                else if (materials.Length < disksData.NumberOfDisks)
                {
                    Material lastMaterial;
                    if (materials.Length == 0)
                        lastMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
                    else
                        lastMaterial = materials[materials.Length - 1];

                    int n = materials.Length;
                    Array.Resize(ref materials, disksData.NumberOfDisks);
                    while (n < materials.Length)
                        materials[n++] = lastMaterial;
                }
                Undo.RecordObject(meshRenderer, "Change Number of Disks");
                meshRenderer.sharedMaterials = materials;
            }
        }

        void ChangeNumberOfDisks(int newNumberOfDisks)
        {
            DisableColorInterpolation();
            disksData.NumberOfDisks = newNumberOfDisks;

            var disks = prefabTarget.gameObject.transform.Find("Disks");
            if (disks != null)
                AdjustMeshRenderer(disks.GetComponent<MeshRenderer>());
        }

        string CreateUniqueDisksMaterialsFolder()
        {
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Disks Materials");
            var aux = path.Split('/');
            AssetDatabase.CreateFolder("Assets", aux[1]);
            return path;
        }
        void ApplyBasicMaterialToAllDisks()
        {
            var disks = instanceTarget.gameObject.transform.Find("Disks");
            if (disks != null)
            {
                var rendeder = disks.GetComponent<MeshRenderer>();
                if (rendeder != null)
                {
                    instanceTarget.UseColorsInterpolation = false;
                    var materials = rendeder.sharedMaterials;
                    for (int n = 0; n < materials.Length; ++n)
                        materials[n] = instanceTarget.BasicMaterial;
                    Undo.RecordObject(rendeder, "Aplly material");
                    rendeder.sharedMaterials = materials;
                }
            }
        }
        void DuplicateMaterials()
        {
            var disks = instanceTarget.gameObject.transform.Find("Disks");
            if (disks != null)
            {
                var rendeder = disks.GetComponent<MeshRenderer>();
                if (rendeder != null)
                {
                    var folder = CreateUniqueDisksMaterialsFolder();
                    var materials = rendeder.sharedMaterials;
                    string[] newPaths = new string[materials.Length];
                    Material selectedMaterial = null;
                    if (materials.Length != 0)
                    {
                        for (int n = 0; n < materials.Length; ++n)
                        {
                            if (materials[n] != null)
                            {
                                newPaths[n] = folder + "/Disk " + n.ToString() + " Material.mat";

                                if (materials[n].name == "Default-Material")
                                    AssetDatabase.CreateAsset(new Material(materials[n]), newPaths[n]);
                                else
                                    AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(materials[n]), newPaths[n]);
                            }
                        }

                        AssetDatabase.Refresh();

                        for (int n = 0; n < materials.Length; ++n)
                        {
                            if (materials[n] != null)
                            {
                                materials[n] = (Material)AssetDatabase.LoadAssetAtPath(newPaths[n], typeof(Material));
                                if (selectedMaterial == null)
                                    selectedMaterial = materials[n];
                            }
                        }

                        Undo.RecordObject(rendeder, "Duplicate Materials");
                        rendeder.sharedMaterials = materials;
                        if (selectedMaterial != null)
                            Selection.activeObject = selectedMaterial;
                    }
                }
            }
        }
        void UpdateDisksColors()
        {
            var disks = instanceTarget.gameObject.transform.Find("Disks");
            if (disks != null)
            {
                var rendeder = disks.GetComponent<MeshRenderer>();
                if (rendeder != null)
                {
                    var colors = instanceTarget.GetInterpolatedColors();
                    var materials = rendeder.sharedMaterials;
                    for (int n = 0; n < materials.Length; ++n)
                    {
                        if (materials[n] != null && materials[n].name != "Default-Material" && materials[n].color != colors[n])
                        {
                            Undo.RecordObject(materials[n], "Change Material Color");
                            materials[n].color = colors[n];
                        }
                    }
                }
            }
        }
        void ShowDisksMaterialsAndColors()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Materials", EditorStyles.boldLabel);
            instanceTarget.BasicMaterial = (Material)EditorGUILayout.ObjectField("Material", instanceTarget.BasicMaterial, typeof(Material), false);

            if (GUILayout.Button(applyMaterialToAllGUIContent))
                ApplyBasicMaterialToAllDisks();

            if (GUILayout.Button(duplicateMaterialsGUIContent))
                DuplicateMaterials();

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("You might want to \"Duplicate Materials\" before coloring the disks, once it works by changing the color of the materials assigned to them, which could affect other objects undesirably.", MessageType.Info);
            EditorGUI.BeginChangeCheck();
            instanceTarget.UseColorsInterpolation = EditorGUILayout.Toggle("Colorize", instanceTarget.UseColorsInterpolation);
            if (EditorGUI.EndChangeCheck())
            {
                if (instanceTarget.UseColorsInterpolation)
                    UpdateDisksColors();
            }

            if (instanceTarget.UseColorsInterpolation)
            {
                var colors = instanceTarget.DisksColors;
                bool updateDisksColors = false;
                int newLength = EditorGUILayout.DelayedIntField("Size", colors.Length);
                if (newLength < 1) newLength = 1;
                if (newLength != colors.Length && newLength > 0)
                {
                    Array.Resize(ref colors, newLength);
                    updateDisksColors = true;
                }
                EditorGUI.BeginChangeCheck();
                for (int n = 0; n < colors.Length; ++n)
                    colors[n] = EditorGUILayout.ColorField("Color " + n, colors[n]);
                
                updateDisksColors |= EditorGUI.EndChangeCheck();
                if (updateDisksColors)
                {
                    instanceTarget.DisksColors = colors;
                    UpdateDisksColors();
                }
            }
        }

        void DisableColorInterpolation()
        {
            prefabTarget.UseColorsInterpolation = false;
            var towers = FindObjectsOfType<TowerOfHanoiCreator>();
            for (int n = 0; n < towers.Length; ++n)
            {
                if (PrefabUtility.GetPrefabParent(towers[n]) == prefabTarget)
                    towers[n].UseColorsInterpolation = false;
            }
            Undo.FlushUndoRecordObjects();
        }
        void ShowDisksInspectorWithRanges()
        {
            int foldoutIndex = 0;
            disksInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(disksInspectorFoldout[foldoutIndex], "Number");
            if (disksInspectorFoldout[foldoutIndex++])
            {
                int newNumberOfDisks = EditorGUILayout.IntSlider("Value", disksData.NumberOfDisks, 1, prefabTarget.MaxNumberOfDisks);
                if (newNumberOfDisks != disksData.NumberOfDisks)
                    ChangeNumberOfDisks(newNumberOfDisks);

                EditorGUILayout.LabelField("Min", "1");
                prefabTarget.MaxNumberOfDisks = EditorGUILayout.DelayedIntField("Max", prefabTarget.MaxNumberOfDisks);
            }

            disksInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(disksInspectorFoldout[foldoutIndex], "Small Radius");
            if (disksInspectorFoldout[foldoutIndex++])
            {
                disksData.SmallDiskRadius = EditorGUILayout.Slider("Value", disksData.SmallDiskRadius, disksData.MinSmallDiskRadius, disksData.MaxSmallDiskRadius);
                EditorGUILayout.LabelField("Min", disksData.MinSmallDiskRadius.ToString());
                EditorGUILayout.LabelField("Max", disksData.MaxSmallDiskRadius.ToString());
            }

            disksInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(disksInspectorFoldout[foldoutIndex], "Big Radius");
            if (disksInspectorFoldout[foldoutIndex++])
            {
                disksData.BigDiskRadius = EditorGUILayout.Slider("Value", disksData.BigDiskRadius, disksData.MinBigDiskRadius, disksData.MaxBigDiskRadius);
                EditorGUILayout.LabelField("Min", disksData.MinBigDiskRadius.ToString());
                EditorGUILayout.LabelField("Max", disksData.MaxBigDiskRadius.ToString());
            }

            disksInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(disksInspectorFoldout[foldoutIndex], "Height");
            if (disksInspectorFoldout[foldoutIndex++])
            {
                disksData.DiskStackHeight = EditorGUILayout.Slider("Value", disksData.DiskStackHeight, disksData.MinDiskStackHeight, disksData.MaxDiskStackHeight);
                EditorGUILayout.LabelField("Min", disksData.MinDiskStackHeight.ToString());
                EditorGUILayout.LabelField("Max", disksData.MaxDiskStackHeight.ToString());
            }

            disksInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(disksInspectorFoldout[foldoutIndex], "Radial Slices");
            if (disksInspectorFoldout[foldoutIndex++])
            {
                disksData.RadialSlices = EditorGUILayout.IntSlider("Value", disksData.RadialSlices, 3, prefabTarget.MaxDiskRadialSlices);
                EditorGUILayout.LabelField("Min", "3");
                prefabTarget.MaxDiskRadialSlices = EditorGUILayout.DelayedIntField("Max", prefabTarget.MaxDiskRadialSlices);
            }

            disksInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(disksInspectorFoldout[foldoutIndex], "Vertical Slices");
            if (disksInspectorFoldout[foldoutIndex++])
            {
                disksData.VerticalSlices = EditorGUILayout.IntSlider("Value", disksData.VerticalSlices, 1, prefabTarget.MaxDiskVerticalSlices);
                EditorGUILayout.LabelField("Min", "1");
                prefabTarget.MaxDiskVerticalSlices = EditorGUILayout.DelayedIntField("Max", prefabTarget.MaxDiskVerticalSlices);
            }
        }
        void ShowDisksInspectorWithoutRanges()
        {
            int newNumberOfDisks = EditorGUILayout.IntSlider("Number", disksData.NumberOfDisks, 1, prefabTarget.MaxNumberOfDisks);
            if (newNumberOfDisks != disksData.NumberOfDisks)
                ChangeNumberOfDisks(newNumberOfDisks);

            disksData.SmallDiskRadius = EditorGUILayout.Slider("Small Radius", disksData.SmallDiskRadius, disksData.MinSmallDiskRadius, disksData.MaxSmallDiskRadius);
            disksData.BigDiskRadius = EditorGUILayout.Slider("Big Radius", disksData.BigDiskRadius, disksData.MinBigDiskRadius, disksData.MaxBigDiskRadius);
            disksData.DiskStackHeight = EditorGUILayout.Slider("Height", disksData.DiskStackHeight, disksData.MinDiskStackHeight, disksData.MaxDiskStackHeight);
            disksData.RadialSlices = EditorGUILayout.IntSlider("Radial Slices", disksData.RadialSlices, 3, prefabTarget.MaxDiskRadialSlices);
            disksData.VerticalSlices = EditorGUILayout.IntSlider("Vertical Slices", disksData.VerticalSlices, 1, prefabTarget.MaxDiskVerticalSlices);
        }
        void ShowDisksInspector()
        {
            if (prefabTarget.ShowValuesRanges)
                ShowDisksInspectorWithRanges();
            else
                ShowDisksInspectorWithoutRanges();

            ShowDisksMaterialsAndColors();
        }
        void ShowRodsInspectorWithRanges()
        {
            int foldoutIndex = 0;
            rodsInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(rodsInspectorFoldout[foldoutIndex], "Height");
            if (rodsInspectorFoldout[foldoutIndex++])
            {
                baseData.RodHeight = EditorGUILayout.Slider("Value", baseData.RodHeight, baseData.MinRodHeight, prefabTarget.MaxRodHeight);
                EditorGUILayout.LabelField("Min", baseData.MinRodHeight.ToString());
                prefabTarget.MaxRodHeight = EditorGUILayout.DelayedFloatField("Max", prefabTarget.MaxRodHeight);
            }

            rodsInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(rodsInspectorFoldout[foldoutIndex], "Radius");
            if (rodsInspectorFoldout[foldoutIndex++])
            {
                baseData.RodRadius = EditorGUILayout.Slider("Value", baseData.RodRadius, 0, baseData.MaxRodRadius);
                EditorGUILayout.LabelField("Min", "0");
                EditorGUILayout.LabelField("Max", baseData.MaxRodRadius.ToString());
            }

            rodsInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(rodsInspectorFoldout[foldoutIndex], "Distance");
            if (rodsInspectorFoldout[foldoutIndex++])
            {
                baseData.DistanceBetweenRods = EditorGUILayout.Slider("Value", baseData.DistanceBetweenRods, baseData.MinDistanceBetweenRods, baseData.MaxDistanceBetweenRods);
                EditorGUILayout.LabelField("Min", baseData.MinDistanceBetweenRods.ToString());
                EditorGUILayout.LabelField("Max", baseData.MaxDistanceBetweenRods.ToString());
            }

            rodsInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(rodsInspectorFoldout[foldoutIndex], "Radial Slices");
            if (rodsInspectorFoldout[foldoutIndex++])
            {
                baseData.RodRadialSlices = EditorGUILayout.IntSlider("Value", baseData.RodRadialSlices, 3, prefabTarget.MaxRodRadialSlices);
                EditorGUILayout.LabelField("Min", "3");
                prefabTarget.MaxRodRadialSlices = EditorGUILayout.DelayedIntField("Max", prefabTarget.MaxRodRadialSlices);
            }

            rodsInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(rodsInspectorFoldout[foldoutIndex], "Cap Slices");
            if (rodsInspectorFoldout[foldoutIndex++])
            {
                baseData.RodCapVerticalSlices = EditorGUILayout.IntSlider("Value", baseData.RodCapVerticalSlices, 1, prefabTarget.MaxRodCapVerticalSlices);
                EditorGUILayout.LabelField("Min", "1");
                prefabTarget.MaxRodCapVerticalSlices = EditorGUILayout.DelayedIntField("Max", prefabTarget.MaxRodCapVerticalSlices);
            }
        }
        void ShowRodsInspectorWithoutRanges()
        {
            baseData.RodHeight = EditorGUILayout.Slider("Height", baseData.RodHeight, baseData.MinRodHeight, prefabTarget.MaxRodHeight);
            baseData.RodRadius = EditorGUILayout.Slider("Radius", baseData.RodRadius, 0, baseData.MaxRodRadius);
            baseData.DistanceBetweenRods = EditorGUILayout.Slider("Distance", baseData.DistanceBetweenRods, baseData.MinDistanceBetweenRods, baseData.MaxDistanceBetweenRods);
            baseData.RodRadialSlices = EditorGUILayout.IntSlider("Radial Slices", baseData.RodRadialSlices, 3, prefabTarget.MaxRodRadialSlices);
            baseData.RodCapVerticalSlices = EditorGUILayout.IntSlider("Cap Slices", baseData.RodCapVerticalSlices, 1, prefabTarget.MaxRodCapVerticalSlices);
        }
        void ShowRodsInspector()
        {
            if (prefabTarget.ShowValuesRanges)
                ShowRodsInspectorWithRanges();
            else
                ShowRodsInspectorWithoutRanges();

            EditorGUILayout.Separator();
            if (GUILayout.Button(fitDistanceGUIContent))
                baseData.DistanceBetweenRods = disksData.GetBetweenRodsDistanceFit();
        }
        void ShowBaseInspectorWithRanges()
        {
            int foldoutIndex = 0;
            baseInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(baseInspectorFoldout[foldoutIndex], "Steps");
            if (baseInspectorFoldout[foldoutIndex++])
            {
                baseData.NumberOfSteps = EditorGUILayout.IntSlider("Value", baseData.NumberOfSteps, 0, prefabTarget.MaxSteps);
                EditorGUILayout.LabelField("Min", "0");
                prefabTarget.MaxSteps = EditorGUILayout.DelayedIntField("Max", prefabTarget.MaxSteps);
            }

            baseInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(baseInspectorFoldout[foldoutIndex], "Top Length");
            if (baseInspectorFoldout[foldoutIndex++])
            {
                baseData.TopLength = EditorGUILayout.Slider("Value", baseData.TopLength, baseData.MinTopLength, prefabTarget.MaxTopLength);
                EditorGUILayout.LabelField("Min", baseData.MinTopLength.ToString());
                prefabTarget.MaxTopLength = EditorGUILayout.DelayedFloatField("Max", prefabTarget.MaxTopLength);
            }

            baseInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(baseInspectorFoldout[foldoutIndex], "Top Width");
            if (baseInspectorFoldout[foldoutIndex++])
            {
                baseData.TopWidth = EditorGUILayout.Slider("Value", baseData.TopWidth, baseData.MinTopWidth, prefabTarget.MaxTopWidth);
                EditorGUILayout.LabelField("Min", baseData.MinTopWidth.ToString());
                prefabTarget.MaxTopWidth = EditorGUILayout.DelayedFloatField("Max", prefabTarget.MaxTopWidth);
            }

            baseInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(baseInspectorFoldout[foldoutIndex], "Step Width");
            if (baseInspectorFoldout[foldoutIndex++])
            {
                baseData.StepsWidth = EditorGUILayout.Slider("Value", baseData.StepsWidth, 0f, prefabTarget.MaxStepWidth);
                EditorGUILayout.LabelField("Min", "0");
                prefabTarget.MaxStepWidth = EditorGUILayout.DelayedFloatField("Max", prefabTarget.MaxStepWidth);
            }

            baseInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(baseInspectorFoldout[foldoutIndex], "Step Height");
            if (baseInspectorFoldout[foldoutIndex++])
            {
                baseData.StepsHeight = EditorGUILayout.Slider("Value", baseData.StepsHeight, 0f, prefabTarget.MaxStepHeight);
                EditorGUILayout.LabelField("Min", "0");
                prefabTarget.MaxStepHeight = EditorGUILayout.DelayedFloatField("Max", prefabTarget.MaxStepHeight);
            }

            baseInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(baseInspectorFoldout[foldoutIndex], "Corner Radius");
            if (baseInspectorFoldout[foldoutIndex++])
            {
                baseData.BaseCornerRadius = EditorGUILayout.Slider("Value", baseData.BaseCornerRadius, 0, baseData.MaxCornerRadius);
                EditorGUILayout.LabelField("Min", "0");
                EditorGUILayout.LabelField("Max", baseData.MaxCornerRadius.ToString());
            }

            baseInspectorFoldout[foldoutIndex] = EditorGUILayout.Foldout(baseInspectorFoldout[foldoutIndex], "Corner Slices");
            if (baseInspectorFoldout[foldoutIndex++])
            {
                baseData.BaseCornerSlices = EditorGUILayout.IntSlider("Value", baseData.BaseCornerSlices, 1, prefabTarget.MaxCornerSlices);
                EditorGUILayout.LabelField("Min", "1");
                prefabTarget.MaxCornerSlices = EditorGUILayout.DelayedIntField("Max", prefabTarget.MaxCornerSlices);
            } 
        }
        void ShowBaseInspectorWithoutRanges()
        {
            baseData.NumberOfSteps = EditorGUILayout.IntSlider("Steps", baseData.NumberOfSteps, 0, prefabTarget.MaxSteps);
            baseData.TopLength = EditorGUILayout.Slider("Top Length", baseData.TopLength, baseData.MinTopLength, prefabTarget.MaxTopLength);
            baseData.TopWidth = EditorGUILayout.Slider("Top Width", baseData.TopWidth, baseData.MinTopWidth, prefabTarget.MaxTopWidth);
            baseData.StepsWidth = EditorGUILayout.Slider("Step Width", baseData.StepsWidth, 0f, prefabTarget.MaxStepWidth);
            baseData.StepsHeight = EditorGUILayout.Slider("Step Height", baseData.StepsHeight, 0f, prefabTarget.MaxStepHeight);
            baseData.BaseCornerRadius = EditorGUILayout.Slider("Corner Radius", baseData.BaseCornerRadius, 0, baseData.MaxCornerRadius);
            baseData.BaseCornerSlices = EditorGUILayout.IntSlider("Corner Slices", baseData.BaseCornerSlices, 1, prefabTarget.MaxCornerSlices);
        }
        void ShowScaleNormalization()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Scale Normalization", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Scales the tower such that one of its dimensions matches the given size.", MessageType.Info);
            prefabTarget.NormalizationValue = EditorGUILayout.FloatField("Size", prefabTarget.NormalizationValue);
            float scale = 0;

            if (GUILayout.Button("X = Size"))
            {
                if (prefabTarget.NormalizationValue != baseData.BottomLength)
                    scale = prefabTarget.NormalizationValue / baseData.BottomLength;
            }
            if (GUILayout.Button("Y = Size"))
            {
                if (prefabTarget.NormalizationValue != baseData.TowerHeight)
                    scale = prefabTarget.NormalizationValue / baseData.TowerHeight;
            }
            if (GUILayout.Button("Z = Size"))
            {
                if (prefabTarget.NormalizationValue != baseData.BottomWidth)
                    scale = prefabTarget.NormalizationValue / baseData.BottomWidth;
            }

            if (scale != 0)
            {
                prefabTarget.Scale(scale);
                baseData.Scale(scale);
                disksData.Scale(scale);
            }
        }
        void ShowBaseInspector()
        {
            if (prefabTarget.ShowValuesRanges)
                ShowBaseInspectorWithRanges();
            else
                ShowBaseInspectorWithoutRanges();

            ShowScaleNormalization();
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = !Application.isPlaying;

            if (baseData == null || disksData == null)
            {
                EditorGUILayout.HelpBox("Missing prefab or prefab connection is broken.", MessageType.Info);
            }
            else 
            {
                tab = GUILayout.Toolbar(tab, new string[] { "Disks", "Rods", "Base" });
                prefabTarget.ShowValuesRanges = EditorGUILayout.Toggle("Show Ranges", prefabTarget.ShowValuesRanges);
                EditorGUILayout.Separator();

                switch (tab)
                {
                    case 0:
                        ShowDisksInspector();
                        break;
                    case 1:
                        ShowRodsInspector();
                        break;
                    case 2:
                        ShowBaseInspector();
                        break;
                }
            }
            
        }
    }
}
