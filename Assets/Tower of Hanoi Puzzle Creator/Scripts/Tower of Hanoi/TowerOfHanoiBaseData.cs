using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// Scriptable object which holds the geometry information 
    /// of the base and the rods of the tower.
    /// </summary>
    public class TowerOfHanoiBaseData : ScriptableObject
    {
        public TowerOfHanoiDisksData disksData;
        public Mesh baseMesh;

        public float distanceBetweenRods = 0.3086f, 
                     rodRadius = 0.038f, 
                     rodHeight = 0.4f;
        public int rodRadialSlices = 12;
        public int rodCapVerticalSlices = 6;

        public int numberOfSteps = 2;
        public int baseCornerSlices = 8;
        public float baseCornerRadius = 0.158f;
        public float topWidth = 0.46f, topLength = 1.07f;
        public float stepsHeight = 0.08f, stepsWidth = 0.09f;
        public float RodHeightWithoutCap
        {
            get
            {
                return rodHeight - rodRadius;
            }
        }
        public Vector3 GetRodPosition(int rod)
        {
            return new Vector3(distanceBetweenRods * (rod - 1), numberOfSteps * stepsHeight, 0);
        }
        public Vector3 GetRodTopPosition(int rod)
        {
            return new Vector3(distanceBetweenRods * (rod - 1), numberOfSteps * stepsHeight + rodHeight, 0);
        }
#if UNITY_EDITOR
        public float MaxDistanceBetweenRods
        {
            get
            {
                return TopLength / 2 - rodRadius;
            }
        }
        public float MinDistanceBetweenRods
        {
            get
            {
                if (disksData.numberOfDisks == 0)
                    return 2 * rodRadius;

                return rodRadius + disksData.GetDiskRadius(disksData.numberOfDisks - 1);
            }
        }
        public float DistanceBetweenRods
        {
            get
            {
                return distanceBetweenRods;
            }
            
            set
            {
                if (value < 0) value = 0;
                if (value != distanceBetweenRods)
                {
                    Undo.RecordObject(this, "Change Distance Between Rods");
                    Undo.RecordObject(disksData.disksTransform, "Change Distance Between Rods");
                    distanceBetweenRods = value;
                    UpdateMesh();
                    disksData.disksTransform.localPosition = GetRodPosition(0);
                }
            }
            
        }
        public float MaxRodRadius
        {
            get
            {
                return disksData.smallDiskRadius - disksData.DiskHeight / 2;
            }
        }
        public float RodRadius
        {
            get
            {
                return rodRadius;
            }
            
            set
            {
                if (value < 0) value = 0;
                if (value != rodRadius)
                {
                    Undo.RecordObject(this, "Change Rod Radius");
                    Undo.RecordObject(disksData, "Change Rod Radius");
                    rodRadius = value;
                    UpdateMesh();
                    disksData.UpdateMesh();
                }
            }
            
        }
        public float MinRodHeight
        {
            get
            {
                return disksData.DiskStackHeight + rodRadius;
            }
        }
        public float RodHeight
        {
            get
            {
                return rodHeight;
            }
            
            set
            {
                if (value < rodRadius) value = rodRadius;
                if (value != rodHeight)
                {
                    Undo.RecordObject(this, "Change Rod Height");
                    rodHeight = value;
                    UpdateMesh();
                }
            }
            
        }
        public float TowerHeight
        {
            get
            {
                return RodHeight + stepsHeight * numberOfSteps;
            }
        }
        public int RodRadialSlices
        {
            get
            {
                return rodRadialSlices;
            }
            
            set
            {
                if (value < 3) value = 3;
                if (value != rodRadialSlices)
                {
                    Undo.RecordObject(this, "Change Rod Radial Slices");
                    rodRadialSlices = value;
                    UpdateMesh();
                }
            }
            
        }
        public int RodCapVerticalSlices
        {
            get
            {
                return rodCapVerticalSlices;
            }
            
            set
            {
                if (value < 1) value = 1;
                if (value != rodCapVerticalSlices)
                {
                    Undo.RecordObject(this, "Change Rod Cap Slices");
                    rodCapVerticalSlices = value;
                    UpdateMesh();
                }
            }
            
        }

        public int NumberOfSteps
        {
            get
            {
                return numberOfSteps;
            }
            
            set
            {
                if (value < 0) value = 0;
                if (value != numberOfSteps)
                {
                    Undo.RecordObject(this, "Change Number of Steps");
                    Undo.RecordObject(disksData.disksTransform, "Change Number of Steps");
                    numberOfSteps = value;
                    UpdateMesh();
                    disksData.disksTransform.localPosition = GetRodPosition(0);
                }
            }
            
        }
        public float StepsHeight
        {
            get
            {
                return stepsHeight;
            }
            
            set
            {
                if (value < 0) value = 0;

                if (value != stepsHeight)
                {
                    Undo.RecordObject(this, "Change Steps Height");
                    Undo.RecordObject(disksData.disksTransform, "Change Distance Between Rods");
                    stepsHeight = value;
                    UpdateMesh();
                    disksData.disksTransform.localPosition = GetRodPosition(0);
                }
            }
            
        }
        public float StepsWidth
        {
            get
            {
                return stepsWidth;
            }
            
            set
            {
                if (value < 0) value = 0;
                if (numberOfSteps <= 1)
                {
                    stepsWidth = value;
                }
                else if (value != stepsWidth)
                {
                    Undo.RecordObject(this, "Change Steps Height");
                    stepsWidth = value;
                    UpdateMesh();
                }
            }
            
        }
        public int BaseCornerSlices
        {
            get
            {
                return baseCornerSlices;
            }
            
            set
            {
                if (value < 1) value = 1;
                if (value != baseCornerSlices)
                {
                    Undo.RecordObject(this, "Change Corner Slices");
                    baseCornerSlices = value;
                    UpdateMesh();
                }
            }
            
        }

        public float MinTopLength
        {
            get
            {
                return 2 * Mathf.Max(baseCornerRadius, rodRadius + distanceBetweenRods);
            }
        }
        public float MinTopWidth
        {
            get
            {
                return 2 * Mathf.Max(baseCornerRadius, rodRadius);
            }
        }
        public float TopLength
        {
            get
            {
                return topLength;
            }
            
            set
            {
            
                if (value < 0)
                    value = 0;
                if (topLength != value)
                {
                    Undo.RecordObject(this, "Change Base Length");
                    topLength = value;
                    UpdateMesh();
                }
            }
            
        }
        public float TopWidth
        {
            get
            {
                return topWidth;
            }
            
            set
            {
                if (value < 0)
                    value = 0;
                if (topWidth != value)
                {
                    Undo.RecordObject(this, "Change Base Width");
                    topWidth = value;
                    UpdateMesh();
                }
            }
            
        }
        public float MaxCornerRadius
        {
            get
            {
                return Mathf.Min(TopWidth / 2, TopLength / 2);
            }
        }
        public float BaseCornerRadius
        {
            get
            {
                return baseCornerRadius;
            }
            
            set
            {
                if (value < 0) value = 0;
                if (value != baseCornerRadius)
                {
                    Undo.RecordObject(this, "Change Corner Radius");
                    baseCornerRadius = value;
                    UpdateMesh();
                }
            }
            
        }
        public float BottomWidth
        {
            get
            {
                if (numberOfSteps <= 1)
                    return topWidth;

                return topWidth + 2 * (numberOfSteps - 1) * stepsWidth;
            }
        }
        public float BottomLength
        {
            get
            {
                if (numberOfSteps <= 1)
                    return topLength;

                return topLength + 2 * (numberOfSteps - 1) * stepsWidth;
            }
        }
        public void Scale(float scale)
        {
            if (scale > 0)
            {
                Undo.RecordObject(this, "Change Scale");
                Undo.RecordObject(disksData.disksTransform, "Change Scale");
                distanceBetweenRods *= scale;
                rodRadius *= scale;
                rodHeight *= scale;
                baseCornerRadius *= scale;
                topWidth *= scale;
                topLength *= scale;
                stepsHeight *= scale;
                stepsWidth *= scale;
                UpdateMesh();
                disksData.disksTransform.localPosition = GetRodPosition(0);
            }
        }
        public void UpdateMesh()
        {
            var meshBuilder = MeshBuilder.CreateTowerOfHanoiBase(BottomLength, BottomWidth, stepsHeight, stepsWidth, numberOfSteps, baseCornerRadius, baseCornerSlices,
                                                               distanceBetweenRods, rodRadius, rodHeight, rodRadialSlices, rodCapVerticalSlices);
            meshBuilder.CopyTo(baseMesh);
        }

        public static TowerOfHanoiBaseData Create()
        {
            var data = CreateInstance<TowerOfHanoiBaseData>();
            data.name = "Base Data";
            data.baseMesh = new Mesh();
            data.baseMesh.name = "Base";
            return data;
        }
#endif        
    }
}
