using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// Script used to access the scriptable objects 
    /// bundled inside the tower prefab.
    /// It also holds inspector data used only in editor time, 
    /// i.e., slider range limits and material/color for the disks.
    /// </summary>
    public class TowerOfHanoiCreator : MonoBehaviour
    {
        [SerializeField]
        TowerOfHanoiBaseData baseData;
        [SerializeField]
        TowerOfHanoiDisksData disksData;

        public TowerOfHanoiBaseData BaseData
        {
            get
            {
                #if UNITY_EDITOR
                if (baseData == null)
                {
                    var prefab = gameObject;
                    if (PrefabUtility.GetPrefabType(prefab) != PrefabType.Prefab)
                        prefab = PrefabUtility.GetPrefabParent(prefab) as GameObject;

                    if (prefab != null)
                        baseData = AssetDatabase.LoadAssetAtPath<TowerOfHanoiBaseData>(AssetDatabase.GetAssetPath(prefab));
                }
                #endif
                return baseData;
            }
            set
            {
                baseData = value;
            }
        }
        public TowerOfHanoiDisksData DisksData
        {
            get
            {
                #if UNITY_EDITOR
                if (disksData == null)
                {
                    var prefab = gameObject;
                    if (PrefabUtility.GetPrefabType(prefab) != PrefabType.Prefab)
                        prefab = PrefabUtility.GetPrefabParent(prefab) as GameObject;

                    if (prefab != null)
                        disksData = AssetDatabase.LoadAssetAtPath<TowerOfHanoiDisksData>(AssetDatabase.GetAssetPath(prefab));
                }
                #endif
                return disksData;
            }
            set
            {
                disksData = value;
            }
        }

#if UNITY_EDITOR
        [SerializeField]
        int maxNumberOfDisks = 30, 
            maxDiskRadialSlices = 40, 
            maxDiskVerticalSlices = 20,
            maxRodRadialSlices = 40,
            maxRodCapVerticalSlices = 20,
            maxSteps = 10,
            maxCornerSlices = 10;
        [SerializeField]
        float maxRodHeight = 1.5f, 
              maxStepHeight = 0.5f,
              maxStepWidth = 0.5f,
              maxTopLength = 3f,
              maxTopWidth = 3f,
              normalizationValue = 1f;
        [SerializeField]
        bool showValuesRanges = false;
        public int MaxNumberOfDisks
        {
            get
            {
                return maxNumberOfDisks;
            }
            set
            {
                if (value < 1) value = 1;
                if (maxNumberOfDisks != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxNumberOfDisks = value;
                }
            }
        }
        public int MaxDiskRadialSlices
        {
            get
            {
                return maxDiskRadialSlices;
            }
            set
            {
                if (value < 3) value = 3;
                if (maxDiskRadialSlices != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxDiskRadialSlices = value;
                }
            }
        }
        public int MaxDiskVerticalSlices
        {
            get
            {
                return maxDiskVerticalSlices;
            }
            set
            {
                if (value < 1) value = 1;
                if (maxDiskVerticalSlices != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxDiskVerticalSlices = value;
                }
            }
        }
        public int MaxRodRadialSlices
        {
            get
            {
                return maxRodRadialSlices;
            }
            set
            {
                if (value < 3) value = 3;
                if (maxRodRadialSlices != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxRodRadialSlices = value;
                }
            }
        }
        public int MaxRodCapVerticalSlices
        {
            get
            {
                return maxRodCapVerticalSlices;
            }
            set
            {
                if (value < 1) value = 1;
                if (maxRodCapVerticalSlices != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxRodCapVerticalSlices = value;
                }
            }
        }
        public int MaxSteps
        {
            get
            {
                return maxSteps;
            }
            set
            {
                if (value < 0) value = 0;
                if (maxSteps != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxSteps = value;
                }
            }
        }
        public int MaxCornerSlices
        {
            get
            {
                return maxCornerSlices;
            }
            set
            {
                if (value < 1) value = 1;
                if (maxCornerSlices != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxCornerSlices = value;
                }
            }
        }
        public float MaxRodHeight
        {
            get
            {
                return maxRodHeight;
            }
            set
            {
                if (value < baseData.MinRodHeight) value = baseData.MinRodHeight;
                if (maxRodHeight != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxRodHeight = value;
                }
            }
        }
        public float MaxStepHeight
        {
            get
            {
                return maxStepHeight;
            }
            set
            {
                if (value < 0) value = 0;
                if (maxStepHeight != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxStepHeight = value;
                }
            }
        }
        public float MaxStepWidth
        {
            get
            {
                return maxStepWidth;
            }
            set
            {
                if (value < 0) value = 0;
                if (maxStepWidth != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxStepWidth = value;
                }
            }
        }
        public float MaxTopLength
        {
            get
            {
                return maxTopLength;
            }
            set
            {
                if (value < baseData.MinTopLength) value = baseData.MinTopLength;
                if (maxTopLength != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxTopLength = value;
                }
            }
        }
        public float MaxTopWidth
        {
            get
            {
                return maxTopWidth;
            }
            set
            {
                if (value < baseData.MinTopWidth) value = baseData.MinTopWidth;
                if (maxTopWidth != value)
                {
                    Undo.RecordObject(this, "Change Value Range");
                    maxTopWidth = value;
                }
            }
        }
        public float NormalizationValue
        {
            get
            {
                return normalizationValue;
            }
            set
            {
                if (value <= 0) return;
                if (normalizationValue != value)
                {
                    Undo.RecordObject(this, "Change Normalization Value");
                    normalizationValue = value;
                }
            }
        }

        public bool ShowValuesRanges
        {
            get
            {
                return showValuesRanges;
            }
            set
            {
                if (showValuesRanges != value)
                {
                    Undo.RecordObject(this, "Toggle Show Value Ranges");
                    showValuesRanges = value;
                }
            }
        }
        public void Scale(float scale)
        {
            if (scale > 0)
            {
                Undo.RecordObject(this, "Change Scale");
                maxRodHeight *= scale;
                maxStepHeight *= scale;
                maxStepWidth *= scale;
                maxTopLength *= scale;
                maxTopWidth *= scale;
            }
        }

        [SerializeField]
        Material basicMaterial;
        [SerializeField]
        Color[] disksColors = new Color[] {Color.red, Color.green, Color.blue, Color.yellow};
        [SerializeField]
        bool useColorsInterpolation = false;

        public bool UseColorsInterpolation
        {
            get
            {
                return useColorsInterpolation;
            }
            set
            {
                if (useColorsInterpolation != value)
                {
                    Undo.RecordObject(this, "Use Color Interpolation");
                    useColorsInterpolation = value;
                }
            }
        }

        public Color GetInterpolatedColor(int diskIndex)
        {
            if (disksColors.Length == 1 || diskIndex == 0)
                return disksColors[0];

            if (diskIndex == disksData.numberOfDisks - 1)
                return disksColors[disksColors.Length - 1];

            float c = diskIndex * (float)(disksColors.Length - 1) / (float)(disksData.numberOfDisks - 1);
            int c1 = (int)c;
            float t = c - c1;
            return Color.Lerp(disksColors[c1], disksColors[c1 + 1], t);
        }
        public Color[] GetInterpolatedColors()
        {
            Color[] colors = new Color[disksData.NumberOfDisks];
            for (int n = 0; n < colors.Length; ++n)
                colors[n] = GetInterpolatedColor(n);
            return colors;
        }
        public Material BasicMaterial
        {
            get
            {
                return basicMaterial;
            }
            set
            {
                if (basicMaterial != value)
                {
                    Undo.RecordObject(this, "Change Basic Material");
                    basicMaterial = value;
                }
            }
        }
        
        public Color[] DisksColors
        {
            get
            {
                return disksColors.Clone() as Color[];
            }
            set
            {
                bool different = false;
                if (value.Length != disksColors.Length)
                {
                    different = (value.Length > 0);
                }
                else
                {
                    
                    for (int n = 0; n < value.Length; ++n)
                        if (value[n] != disksColors[n])
                        {
                            different = true;
                            break;
                        }
                }

                if (different)
                {
                    Undo.RecordObject(this, "Change Color Array");
                    disksColors = value;
                }
            }
        }
#endif
    }
}
