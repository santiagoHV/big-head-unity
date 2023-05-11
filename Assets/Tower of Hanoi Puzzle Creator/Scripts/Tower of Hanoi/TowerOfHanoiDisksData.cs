using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// Scriptable object which holds the geometry information 
    /// of the disks. 
    /// While in editor mode, it's also responsible to record
    /// itself when changes in geometry are made in order
    /// to enable a proper do/undo behaviour in the inspector.
    /// </summary>
    public class TowerOfHanoiDisksData : ScriptableObject
    {
        public TowerOfHanoiBaseData baseData;
        public Transform disksTransform;
        public Mesh mesh;
        public int numberOfDisks = 4;
        public float diskStackHeight = 0.3f;
        public float smallDiskRadius = 0.075f, 
                     bigDiskRadius = 0.17f;
        public int radialSlices = 20;
        public int verticalSlices = 10;
        
        [SerializeField]
        int[] disksMeshNumberOfVertices;

        [NonSerialized]
        Mesh[] meshes;
        public Mesh[] GetMeshes()
        {
            if (meshes != null)
                return meshes;

            meshes = new Mesh[numberOfDisks];
            if (mesh != null)
            {
                var vertices = mesh.vertices;
                var normals = mesh.normals;
                var uv = mesh.uv;

                Vector3 diskTranslation = new Vector3(0, (numberOfDisks - 0.5f) * DiskHeight, 0);
                for (int disk = 0, startVertexIndex = 0; disk < numberOfDisks; ++disk)
                {
                    var diskMesh = new Mesh();
                    int numberOfDiskVertices = disksMeshNumberOfVertices[disk];
                    var diskVertices = new Vector3[numberOfDiskVertices];
                    var diskNormals = new Vector3[numberOfDiskVertices];
                    var diskUV = new Vector2[numberOfDiskVertices];
                    var triangles = mesh.GetTriangles(disk);

                    for (int n = 0; n < triangles.Length; ++n)
                        triangles[n] -= startVertexIndex;

                    for (int n = 0; n < numberOfDiskVertices; ++n, ++startVertexIndex)
                    {
                        diskVertices[n] = vertices[startVertexIndex] - diskTranslation;
                        diskNormals[n] = normals[startVertexIndex];
                        diskUV[n] = uv[startVertexIndex];
                    }

                    diskMesh.vertices = diskVertices;
                    diskMesh.normals = diskNormals;
                    diskMesh.uv = diskUV;
                    diskMesh.triangles = triangles;

                    meshes[disk] = diskMesh;
                    diskTranslation.y -= DiskHeight;
                }
            }
            return meshes;
        }
        public float DiskHeight
        {
            get
            {
                if (numberOfDisks == 0)
                    return 0;
                return diskStackHeight / numberOfDisks;
            }
        }

#if UNITY_EDITOR
        public int NumberOfDisks
        {
            get
            {
                return numberOfDisks;
            }
            set
            {
                if (value < 0) value = 0;
                if (value != numberOfDisks)
                {
                    Undo.RecordObject(this, "Change Number of Disks");
                    float newStackHeight = DiskHeight* value;
                    if (newStackHeight > baseData.RodHeightWithoutCap)
                        newStackHeight = baseData.RodHeightWithoutCap;
                    numberOfDisks = value;
                    diskStackHeight = newStackHeight;
                    UpdateMesh();
                }
            }
        }
        public int RadialSlices
        {
            get
            {
                return radialSlices;
            }
            set
            {
                if (value < 3) value = 3;
                if (value != radialSlices)
                {
                    Undo.RecordObject(this, "Change Disks Radial Slices");
                    radialSlices = value;
                    UpdateMesh();
                }
            }
        }
        public int VerticalSlices
        {
            get
            {
                return verticalSlices;
            }
            set
            {
                if (value < 1) value = 1;
                if (value != verticalSlices)
                {
                    Undo.RecordObject(this, "Change Disks Vertical Slices");
                    verticalSlices = value;
                    UpdateMesh();
                }
            }
        }
        public float MinSmallDiskRadius
        {
            get
            {
                return baseData.rodRadius + DiskHeight / 2;
            }
        }
        public float MaxSmallDiskRadius
        {
            get
            {
                return bigDiskRadius;
            }
        }
        public float SmallDiskRadius
        {
            get
            {
                return smallDiskRadius;
            }
            set
            {
                if (value < 0) value = 0;
                if (value != smallDiskRadius)
                {
                    Undo.RecordObject(this, "Change Disk Radius");
                    smallDiskRadius = value;
                    UpdateMesh();
                }
            }
        }
        public float MinBigDiskRadius
        {
            get
            {
                return smallDiskRadius;
            }
        }
        public float MaxBigDiskRadius
        {
            get
            {
                return baseData.distanceBetweenRods - baseData.rodRadius;
            }
        }
        public float BigDiskRadius
        {
            get
            {
                return bigDiskRadius;
            }
            set
            {
                if (value < 0) value = 0;
                if (value != bigDiskRadius)
                {
                    Undo.RecordObject(this, "Change Disk Radius");
                    bigDiskRadius = value;
                    UpdateMesh();
                }
            }
        }

        public float MinDiskStackHeight
        {
            get
            {
                return 0.001f;
            }
        }
        public float MaxDiskStackHeight
        {
            get
            {
                return Mathf.Min(baseData.rodHeight - baseData.rodRadius, 2 * numberOfDisks * (smallDiskRadius - baseData.rodRadius));
            }
        }
        public float DiskStackHeight
        {
            get
            {
                return diskStackHeight;
            }
            set
            {
                if (value < 0) value = 0;
                if (diskStackHeight != value)
                {
                    Undo.RecordObject(this, "Change Disk Height");
                    diskStackHeight = value;
                    UpdateMesh();
                }
            }
        }
        public float GetDiskRadius(int disk)
        {
            if (NumberOfDisks == 0)
                return 0;

            if (NumberOfDisks == 1)
                return smallDiskRadius;

            return smallDiskRadius + disk * (bigDiskRadius - smallDiskRadius) / (NumberOfDisks - 1);
        }
        public float GetBetweenRodsDistanceFit()
        {
            if (NumberOfDisks < 2)
                return 2 * smallDiskRadius;

            return 2 * smallDiskRadius + (2 * NumberOfDisks - 3) * (bigDiskRadius - smallDiskRadius) / (NumberOfDisks - 1);
        }
        public void Scale(float scale)
        {
            if (scale > 0)
            {
                Undo.RecordObject(this, "Change Scale");
                diskStackHeight *= scale;
                smallDiskRadius *= scale;
                bigDiskRadius *= scale;
                UpdateMesh();
            }
        }
        public void UpdateMesh()
        {
            if (disksMeshNumberOfVertices == null || disksMeshNumberOfVertices.Length != numberOfDisks)
                disksMeshNumberOfVertices = new int[numberOfDisks];

            mesh.Clear();
            if (numberOfDisks == 0) return;

            Vector3 diskTranslation = new Vector3(0, (numberOfDisks - 0.5f) * DiskHeight, 0);
            float deltaRadius = 0,
                  diskRadius = smallDiskRadius;
            if (numberOfDisks > 1)
                deltaRadius = (bigDiskRadius - smallDiskRadius) / (numberOfDisks - 1);

            var meshBuilder = MeshBuilder.CreateTowerOfHanoiDisk(baseData.rodRadius, diskRadius, DiskHeight, radialSlices, verticalSlices);
            meshBuilder.TranslateVertices(diskTranslation);
            disksMeshNumberOfVertices[0] = meshBuilder.NumberOfVertices;

            for (int n = 1; n < numberOfDisks; ++n)
            {
                diskRadius += deltaRadius;
                diskTranslation.y -= DiskHeight;
                var aux = MeshBuilder.CreateTowerOfHanoiDisk(baseData.rodRadius, diskRadius, DiskHeight, radialSlices, verticalSlices);
                aux.TranslateVertices(diskTranslation);
                disksMeshNumberOfVertices[n] = aux.NumberOfVertices;
                meshBuilder.AddSubmeshes(aux);
            }
            meshBuilder.CopyTo(mesh);
        }

        public static TowerOfHanoiDisksData Create()
        {
            var data = CreateInstance<TowerOfHanoiDisksData>();
            data.name = "Disks Data";
            data.mesh = new Mesh();
            data.mesh.name = "Disks";
            return data;
        }
#endif
    }
}
