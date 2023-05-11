using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// Main class to deal with procedural meshes.
    /// </summary>
    public class MeshBuilder
    {
        public List<Vector3> vertices = new List<Vector3>(),
                             normals = new List<Vector3>();
        public List<Vector2> uv = new List<Vector2>();
        public List<List<int>> submeshesTriangles = new List<List<int>>() { new List<int>() };

        public int NumberOfVertices
        {
            get
            {
                return vertices.Count;
            }
        }
        public int NumberOfTriangles
        {
            get
            {
                return submeshesTriangles.Sum(item => item.Count) / 3;
            }
        }
        public int NumberOfSubmeshes
        {
            get
            {
                return submeshesTriangles.Count;
            }
        }
        public int NumberOfNormals
        {
            get
            {
                return normals.Count;
            }
        }
        public int NumberOfUV
        {
            get
            {
                return uv.Count;
            }
        }
        public int GetNumberOfTriangles(int submesh)
        {
            return submeshesTriangles[submesh].Count / 3;
        }
        public int GetNumberOfVertices(int submesh)
        {
            return submeshesTriangles[submesh].Distinct().Count();
        }
        public Mesh GetMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uv.ToArray();
            for (int n = 0; n < submeshesTriangles.Count; ++n)
                mesh.SetTriangles(submeshesTriangles[n].ToArray(), n);
            return mesh;
        }
        public void CopyTo(Mesh mesh)
        {
            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uv.ToArray();
            mesh.subMeshCount = submeshesTriangles.Count;
            for (int n = 0; n < submeshesTriangles.Count; ++n)
                mesh.SetTriangles(submeshesTriangles[n].ToArray(), n);
        }
        public void AddSubmeshes(MeshBuilder meshBuilder)
        {
            int offset = vertices.Count;
            for (int n = 0; n < meshBuilder.vertices.Count; ++n)
            {
                vertices.Add(meshBuilder.vertices[n]);
                normals.Add(meshBuilder.normals[n]);
                uv.Add(meshBuilder.uv[n]);
            }

            for (int n = 0; n < meshBuilder.NumberOfSubmeshes; ++n)
            {
                var triangles = meshBuilder.submeshesTriangles[n];
                for (int i = 0; i < triangles.Count; ++i)
                    triangles[i] += offset;
                submeshesTriangles.Add(triangles);
            }
        }
        public void MergeSubmeshes(MeshBuilder MeshBuilder)
        {
            int offset = vertices.Count;
            for (int n = 0; n < MeshBuilder.vertices.Count; ++n)
            {
                vertices.Add(MeshBuilder.vertices[n]);
                normals.Add(MeshBuilder.normals[n]);
                uv.Add(MeshBuilder.uv[n]);
            }

            int limit = Mathf.Min(NumberOfSubmeshes, MeshBuilder.NumberOfSubmeshes);
            for (int n = 0; n < limit; ++n)
            {
                var triangles1 = submeshesTriangles[n];
                var triangles2 = MeshBuilder.submeshesTriangles[n];
                for (int i = 0; i < triangles2.Count; ++i)
                    triangles1.Add(triangles2[i] + offset);
            }

            for (int n = limit; n < MeshBuilder.NumberOfSubmeshes; ++n)
                submeshesTriangles.Add(MeshBuilder.submeshesTriangles[n]);
        }

        public void Clear()
        {
            vertices.Clear();
            normals.Clear();
            uv.Clear();
            submeshesTriangles.Clear();
            submeshesTriangles.Add(new List<int>());
        }

        public void DuplicateVerticesNormalsAndUV(int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
            {
                vertices.Add(vertices[n]);
                normals.Add(normals[n]);
                uv.Add(uv[n]);
            }
        }
        public void DuplicateVerticesNormalsAndUV()
        {
            DuplicateVerticesNormalsAndUV(0, NumberOfVertices);
        }
        public void DuplicateLastVerticesNormalsAndUV(int count)
        {
            DuplicateVerticesNormalsAndUV(NumberOfVertices - count, count);
        }

        public void DuplicateVertices(int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
                vertices.Add(vertices[n]);
        }
        public void DuplicateVertices()
        {
            DuplicateVertices(0, NumberOfVertices);
        }
        public void DuplicateLastVertices(int count)
        {
            DuplicateVertices(NumberOfVertices - count, count);
        }

        public void DuplicateNormals(int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
                normals.Add(normals[n]);
        }
        public void DuplicateNormals()
        {
            DuplicateNormals(0, NumberOfNormals);
        }
        public void DuplicateLastNormals(int count)
        {
            DuplicateNormals(NumberOfNormals - count, count);
        }

        public void DuplicateUV(int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
                uv.Add(uv[n]);
        }
        public void DuplicateUV()
        {
            DuplicateUV(0, NumberOfUV);
        }
        public void DuplicateLastUV(int count)
        {
            DuplicateUV(NumberOfUV - count, count);
        }

        public void DuplicateTriangles(int startIndex, int count, int vertexOfsset = 0, int submesh = 0)
        {
            var triangles = submeshesTriangles[submesh];
            startIndex *= 3;
            while (count-- > 0)
            {
                triangles.Add(triangles[startIndex++] + vertexOfsset);
                triangles.Add(triangles[startIndex++] + vertexOfsset);
                triangles.Add(triangles[startIndex++] + vertexOfsset);
            }
        }
        public void DuplicateTriangles(int vertexOfsset = 0, int submesh = 0)
        {
            DuplicateTriangles(0, GetNumberOfTriangles(submesh), vertexOfsset, submesh);
        }
        public void DuplicateLastTriangles(int count, int vertexOfsset = 0, int submesh = 0)
        {
            DuplicateTriangles(GetNumberOfTriangles(submesh) - count, count, vertexOfsset, submesh);
        }

        public void AddTriangle(int v1, int v2, int v3, int submesh = 0)
        {
            var triangles = submeshesTriangles[submesh];
            triangles.Add(v1);
            triangles.Add(v2);
            triangles.Add(v3);
        }
        public void AddQuad(int v1, int v2, int v3, int v4, int submesh = 0)
        {
            var triangles = submeshesTriangles[submesh];
            triangles.Add(v1);
            triangles.Add(v2);
            triangles.Add(v3);

            triangles.Add(v1);
            triangles.Add(v3);
            triangles.Add(v4);
        }
        public void AddTriangleStrip(int vertexIndex1, int vertexIndex2, int length, int submesh = 0)
        {
            var triangles = submeshesTriangles[submesh];
            for (int n = 1; n < length; ++n)
            {
                triangles.Add(vertexIndex1++);
                triangles.Add(vertexIndex1);
                triangles.Add(vertexIndex2);
                triangles.Add(vertexIndex2++);
                triangles.Add(vertexIndex1);
                triangles.Add(vertexIndex2);
            }
        }
        public void AddClosedTriangleStrip(int vertexIndex1, int vertexIndex2, int length, int submesh = 0)
        {
            var triangles = submeshesTriangles[submesh];
            int auxLeft = vertexIndex1,
                auxRight = vertexIndex2;

            for (int n = 1; n < length; ++n)
            {
                triangles.Add(vertexIndex1++);
                triangles.Add(vertexIndex1);
                triangles.Add(vertexIndex2);
                triangles.Add(vertexIndex2++);
                triangles.Add(vertexIndex1);
                triangles.Add(vertexIndex2);
            }

            triangles.Add(vertexIndex2);
            triangles.Add(vertexIndex1);
            triangles.Add(auxLeft);
            triangles.Add(vertexIndex2);
            triangles.Add(auxLeft);
            triangles.Add(auxRight);
        }
        public void AddTriangleFan(int pivotIndex, int fanStartIndex, int length, int submesh = 0)
        {
            var triangles = submeshesTriangles[submesh];
            for (int n = 1; n < length; ++n)
            {
                triangles.Add(fanStartIndex++);
                triangles.Add(pivotIndex);
                triangles.Add(fanStartIndex);
            }
        }
        public void AddClosedTriangleFan(int pivotIndex, int fanStartIndex, int length, int submesh = 0)
        {
            var triangles = submeshesTriangles[submesh];
            int i = fanStartIndex + length - 1;
            for (int n = 0; n < length; ++n)
            {
                triangles.Add(i);
                triangles.Add(pivotIndex);
                triangles.Add(fanStartIndex);
                i = fanStartIndex;
                fanStartIndex++;
            }
        }

        public void TranslateVertices(Vector3 translation, int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
                vertices[n] += translation;
        }
        public void TranslateLastVertices(Vector3 translation, int count)
        {
            TranslateVertices(translation, NumberOfVertices - count, count);
        }
        public void TranslateVertices(Vector3 translation)
        {
            TranslateVertices(translation, 0, NumberOfVertices);
        }

        public void TranslateUV(Vector2 translation, int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
                uv[n] += translation;
        }
        public void TranslateUV(Vector2 translation)
        {
            TranslateUV(translation, 0, NumberOfUV);
        }
        public void TranslateLastUV(Vector2 translation, int count)
        {
            TranslateUV(translation, NumberOfUV - count, count);
        }

        public void RotateVertices(Quaternion rotation, int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
            {
                vertices[n] = rotation * vertices[n];
                normals[n] = rotation * normals[n];
            }
        }
        public void RotateLastVertices(Quaternion rotation, int count)
        {
            RotateVertices(rotation, NumberOfVertices - count, count);
        }
        public void RotateVertices(Quaternion rotation)
        {
            RotateVertices(rotation, 0, NumberOfVertices);
        }

        public void RotateUV(Quaternion rotation, int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
                uv[n] = rotation * uv[n];
        }
        public void RotateLastUV(Quaternion rotation, int count)
        {
            RotateUV(rotation, NumberOfVertices - count, count);
        }
        public void RotateUV(Quaternion rotation)
        {
            RotateUV(rotation, 0, NumberOfVertices);
        }

        public void ScaleVertices(Vector3 scale, int startIndex, int length)
        {
            Vector3 normalScale = new Vector3(scale.y * scale.z, scale.x * scale.z, scale.x * scale.y);
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
            {
                vertices[n] = Vector3.Scale(scale, vertices[n]);
                normals[n] = Vector3.Scale(normals[n], normalScale).normalized;
            }
        }
        public void ScaleLastVertices(Vector3 scale, int count)
        {
            TranslateVertices(scale, NumberOfVertices - count, count);
        }
        public void ScaleVertices(Vector3 scale)
        {
            TranslateVertices(scale, 0, NumberOfVertices);
        }

        public void ScaleUV(Vector2 scale, int startIndex, int length)
        {
            for (int n = startIndex, limit = startIndex + length; n < limit; ++n)
                uv[n] = Vector2.Scale(uv[n], scale);
        }
        public void ScaleUV(Vector2 scale)
        {
            ScaleUV(scale, 0, NumberOfUV);
        }
        public void ScaleLastUV(Vector2 scale, int count)
        {
            ScaleUV(scale, NumberOfUV - count, count);
        }

        public void InvertTriangles(int startTriangleIndex, int count, int submesh = 0)
        {
            var triangles = submeshesTriangles[submesh];
            int aux;
            startTriangleIndex *= 3;
            while (count-- > 0)
            {
                aux = triangles[startTriangleIndex];
                triangles[startTriangleIndex] = triangles[startTriangleIndex + 1];
                triangles[startTriangleIndex + 1] = aux;
                startTriangleIndex += 3;
            }
        }
        public void InvertTriangles(int submesh = 0)
        {
            InvertTriangles(0, GetNumberOfTriangles(submesh), submesh);
        }
        public void InvertLastTriangles(int count, int submesh = 0)
        {
            InvertTriangles(GetNumberOfTriangles(submesh) - count, count);
        }
        public void InvertAllTriangles()
        {
            for (int n = 0; n < submeshesTriangles.Count; ++n)
                InvertTriangles(n);
        }

        public void FlipNormals(int startIndex, int length)
        {
            while (length-- > 0)
            {
                normals[startIndex] = -normals[startIndex];
                ++startIndex;
            }
        }
        public void FlipNormals()
        {
            FlipNormals(0, NumberOfNormals);
        }
        public void FlipLastNormals(int count)
        {
            FlipNormals(NumberOfNormals - count, count);
        }

        public void MirrorNormalsXZ(int startIndex, int length)
        {
            while (length-- > 0)
            {
                Vector3 aux = normals[startIndex];
                normals[startIndex] = new Vector3(-aux.x, aux.y, -aux.z);
                ++startIndex;
            }
        }
        public void MirrorNormalsXZ()
        {
            MirrorNormalsXZ(0, NumberOfNormals);
        }
        public void MirrorLastNormalsXZ(int count)
        {
            MirrorNormalsXZ(NumberOfNormals - count, count);
        }

        public void MirrorVerticesXZ(int startIndex, int length)
        {
            while (length-- > 0)
            {
                Vector3 aux = vertices[startIndex];
                vertices[startIndex] = new Vector3(-aux.x, aux.y, -aux.z);
                ++startIndex;
            }
        }
        public void MirrorVerticesXZ()
        {
            MirrorVerticesXZ(0, NumberOfNormals);
        }
        public void MirrorLastVerticesXZ(int count)
        {
            MirrorVerticesXZ(NumberOfVertices - count, count);
        }

        public void FillNormals(Vector3 value)
        {
            for (int n = normals.Count, limit = NumberOfVertices; n < limit; ++n)
                normals.Add(value);
        }

        void AddRoundRectangleBorderVertices(float length, float width, float radius, int cornerSlices)
        {
            if (cornerSlices < 1) cornerSlices = 1;

            float a = length / 2 - radius,
                  b = width / 2 - radius;

            var circle = GeneretaCirclePoints(radius, 0, Mathf.PI / (2 * cornerSlices), 4 * cornerSlices + 1);

            Vector3[] middleRectangleVertices = new Vector3[]
            {
            new Vector3( a, 0,  b),
            new Vector3(-a, 0,  b),
            new Vector3(-a, 0, -b),
            new Vector3( a, 0, -b)
            };

            int numberOfVerticesPerCorner = cornerSlices + 1;
            for (int corner = 0, i = 0; corner < 4; ++corner, --i)
            {
                Vector3 vertexPivot = middleRectangleVertices[corner];
                for (int count = 0; count < numberOfVerticesPerCorner; ++count, ++i)
                {
                    Vector2 aux = circle[i];
                    vertices.Add(vertexPivot + new Vector3(aux.x, 0, aux.y));
                }
            }
        }
        void AddRoundRectangleStripVerticesAndTriangles(float length, float width, float stripWidth, float radius, int cornerSlices, int submesh = 0)
        {
            int startVertex1 = NumberOfVertices,
                startVertex2;
            AddRoundRectangleBorderVertices(length, width, radius, cornerSlices);
            startVertex2 = NumberOfVertices;
            stripWidth *= 2;
            AddRoundRectangleBorderVertices(length - stripWidth, width - stripWidth, radius, cornerSlices);
            AddClosedTriangleStrip(startVertex2, startVertex1, startVertex2 - startVertex1, submesh);
        }
        void AddRoundRectangleVerticesAndTriangles(float length, float width, float radius, int cornerSlices, int submesh = 0)
        {
            if (cornerSlices < 1) cornerSlices = 1;

            int startVertexIndex = NumberOfVertices;
            int numberOfVerticesPerCorner = cornerSlices + 1;
            float a = length / 2 - radius,
                  b = width / 2 - radius;

            vertices.Add(new Vector3(a, 0, b));
            vertices.Add(new Vector3(-a, 0, b));
            vertices.Add(new Vector3(-a, 0, -b));
            vertices.Add(new Vector3(a, 0, -b));

            AddRoundRectangleBorderVertices(length, width, radius, cornerSlices);

            for (int corner = 0; corner < 4; ++corner)
                AddTriangleFan(startVertexIndex + corner, 4 + startVertexIndex + corner * numberOfVerticesPerCorner, numberOfVerticesPerCorner, submesh);

            int corner0_start = startVertexIndex + 4,
                corner0_end = corner0_start + cornerSlices,
                corner1_start = corner0_end + 1,
                corner1_end = corner1_start + cornerSlices,
                corner2_start = corner1_end + 1,
                corner2_end = corner2_start + cornerSlices,
                corner3_start = corner2_end + 1,
                corner3_end = corner3_start + cornerSlices;

            AddQuad(corner3_end, corner2_start, corner1_end, corner0_start, submesh);
            AddQuad(startVertexIndex, startVertexIndex + 1, corner1_start, corner0_end, submesh);
            AddQuad(corner3_start, corner2_end, startVertexIndex + 2, startVertexIndex + 3, submesh);
        }
        void AddTowerOfHanoiHalfBaseSideUV(float uv_yUp, float uv_yDown, float cornerRadius, int cornerSlices, float halfLength, float halfWidth)
        {
            float totalLength = 2 * (halfWidth + halfLength - 2 * cornerRadius) + Mathf.PI * cornerRadius;
            float x = halfWidth - cornerRadius;
            float sliceArcLength = cornerRadius * Mathf.PI / (2 * cornerSlices);
            int uvStartIndex = uv.Count;

            uv.Add(new Vector2(0, uv_yDown));
            for (int n = 0; n < cornerSlices; ++n)
            {
                uv.Add(new Vector2(x / totalLength, uv_yDown));
                x += sliceArcLength;
            }
            uv.Add(new Vector2(x / totalLength, uv_yDown));

            x += 2 * (halfLength - cornerRadius);
            for (int n = 0; n < cornerSlices; ++n)
            {
                uv.Add(new Vector2(x / totalLength, uv_yDown));
                x += sliceArcLength;
            }
            uv.Add(new Vector2(x / totalLength, uv_yDown));
            uv.Add(new Vector2(1, uv_yDown));

            for (int n = 0, limit = 2 * (cornerSlices + 1) + 2; n < limit; ++n)
            {
                Vector2 aux = uv[uvStartIndex++];
                uv.Add(new Vector2(aux.x, uv_yUp));
            }
        }

        public static Vector2[] GeneretaUnityCirclePoints(float startAngle, float deltaAngle, int numberOfPoints)
        {
            Vector2[] circle = new Vector2[numberOfPoints];
            if (numberOfPoints > 0)
            {
                Vector2 aux;
                float cosDelta = Mathf.Cos(deltaAngle),
                        sinDelta = Mathf.Sin(deltaAngle);
                circle[0].Set(Mathf.Cos(startAngle), Mathf.Sin(startAngle));
                for (int n = 1; n < numberOfPoints; ++n)
                {
                    aux = circle[n - 1];
                    circle[n].Set(aux.x * cosDelta - aux.y * sinDelta,
                                    aux.x * sinDelta + aux.y * cosDelta);
                }
            }
            return circle;
        }
        public static Vector2[] GeneretaCirclePoints(float radius, float startAngle, float deltaAngle, int numberOfPoints)
        {
            Vector2[] circle = new Vector2[numberOfPoints];
            if (numberOfPoints > 0)
            {
                Vector2 aux;
                float cosDelta = Mathf.Cos(deltaAngle),
                      sinDelta = Mathf.Sin(deltaAngle);
                circle[0].Set(Mathf.Cos(startAngle), Mathf.Sin(startAngle));
                for (int n = 1; n < numberOfPoints; ++n)
                {
                    aux = circle[n - 1];
                    circle[n].Set(aux.x * cosDelta - aux.y * sinDelta,
                                  aux.x * sinDelta + aux.y * cosDelta);
                }

                for (int n = 0; n < numberOfPoints; ++n)
                    circle[n] *= radius;
            }
            return circle;
        }

        public static MeshBuilder CreateCylinderShell(float radius, float height, int slices)
        {
            if (slices < 3) slices = 3;

            MeshBuilder data = new MeshBuilder();
            Vector3 aux = new Vector3();
            int numberOfCircleVertices = slices + 1;
            var circle = GeneretaUnityCirclePoints(0f, 2 * Mathf.PI / slices, numberOfCircleVertices);
            float uvFactor = 1f / slices;
            for (int n = 0; n < numberOfCircleVertices; ++n)
            {
                aux.Set(circle[n].x, 0, circle[n].y);
                data.vertices.Add(aux * radius);
                data.normals.Add(aux);
                data.uv.Add(new Vector2(n * uvFactor, 0));
            }

            Vector3 translation = new Vector3(0, height / 2, 0);
            data.DuplicateVerticesNormalsAndUV();
            data.TranslateLastVertices(translation, numberOfCircleVertices);
            data.TranslateVertices(-translation, 0, numberOfCircleVertices);
            data.TranslateLastUV(new Vector2(0, 1), numberOfCircleVertices);
            data.AddTriangleStrip(numberOfCircleVertices, 0, numberOfCircleVertices);
            return data;
        }
        public static MeshBuilder CreateCylinder(float radius, float height, int slices)
        {
            MeshBuilder shell = CreateCylinderShell(radius, height, slices);
            MeshBuilder top = CreateCircle(radius, slices);
            Vector3 translation = new Vector3(0, height / 2, 0);
            int numberOfTopVertices = top.NumberOfVertices;
            int numberOfTopTriangles = top.NumberOfTriangles;

            top.DuplicateVerticesNormalsAndUV();
            top.TranslateVertices(translation, 0, numberOfTopVertices);
            top.TranslateLastVertices(-translation, numberOfTopVertices);
            top.FlipLastNormals(numberOfTopVertices);
            top.DuplicateTriangles(numberOfTopVertices);
            top.InvertLastTriangles(numberOfTopTriangles);

            shell.ScaleUV(new Vector2(2f, 1f));
            shell.MergeSubmeshes(top);
            return shell;
        }
        public static MeshBuilder CreateCircle(float radius, int slices)
        {
            if (slices < 3) slices = 3;

            MeshBuilder data = new MeshBuilder();
            Vector3 aux = new Vector3();
            var circle = GeneretaUnityCirclePoints(0f, 2 * Mathf.PI / slices, slices);

            data.vertices.Add(Vector3.zero);
            data.uv.Add(new Vector2(0.5f, 0.5f));

            for (int n = 0; n < slices; ++n)
            {
                aux.Set(circle[n].x, 0, circle[n].y);
                data.vertices.Add(aux * radius);
                data.uv.Add((circle[n] + Vector2.one) / 2f);
            }

            data.FillNormals(Vector3.up);
            data.AddClosedTriangleFan(0, 1, slices);

            return data;
        }
        public static MeshBuilder CreateCircleWithAHole(float innerRadius, float outerRadius, int slices)
        {
            if (slices < 3) slices = 3;

            MeshBuilder data = new MeshBuilder();
            Vector3 aux = new Vector3();
            float uvFactor = innerRadius / (2 * outerRadius);
            Vector2 uvDelta = new Vector2(0.5f, 0.5f);

            var circle = GeneretaUnityCirclePoints(0f, 2 * Mathf.PI / slices, slices);

            for (int n = 0; n < slices; ++n)
            {
                aux.Set(circle[n].x, 0, circle[n].y);
                data.vertices.Add(aux * innerRadius);
                data.uv.Add(circle[n] * uvFactor + uvDelta);
            }

            for (int n = 0; n < slices; ++n)
            {
                aux.Set(circle[n].x, 0, circle[n].y);
                data.vertices.Add(aux * outerRadius);
                data.uv.Add(circle[n] / 2 + uvDelta);
            }

            data.FillNormals(Vector3.up);
            data.AddClosedTriangleStrip(0, slices, slices);

            return data;
        }

        public static MeshBuilder CreateRectangle(float length, float width)
        {
            MeshBuilder data = new MeshBuilder();
            float a = length / 2,
                  b = width / 2;

            data.vertices.Add(new Vector3(a, 0, b));
            data.uv.Add(new Vector2(1, 1));

            data.vertices.Add(new Vector3(-a, 0, b));
            data.uv.Add(new Vector2(0, 1));

            data.vertices.Add(new Vector3(-a, 0, -b));
            data.uv.Add(new Vector2(0, 0));

            data.vertices.Add(new Vector3(a, 0, -b));
            data.uv.Add(new Vector2(1, 0));

            data.FillNormals(Vector3.up);

            data.AddQuad(0, 3, 2, 1);
            return data;
        }
        public static MeshBuilder CreateRoundRectangle(float length, float width, float radius, int cornerSlices)
        {
            MeshBuilder data = new MeshBuilder();
            if (cornerSlices < 1) cornerSlices = 1;

            float a = length / 2 - radius,
                  b = width / 2 - radius,
                  a_uv = a / length,
                  b_uv = b / width;

            var circle = GeneretaCirclePoints(radius, 0, Mathf.PI / (2 * cornerSlices), 4 * cornerSlices + 1);

            Vector3[] middleRectangleVertices = new Vector3[]
            {
            new Vector3( a, 0,  b),
            new Vector3(-a, 0,  b),
            new Vector3(-a, 0, -b),
            new Vector3( a, 0, -b)
            };

            Vector2[] middleRectangleUV = new Vector2[]
            {
            new Vector2( a_uv + 0.5f,  b_uv + 0.5f),
            new Vector2(-a_uv + 0.5f,  b_uv + 0.5f),
            new Vector2(-a_uv + 0.5f, -b_uv + 0.5f),
            new Vector2( a_uv + 0.5f, -b_uv + 0.5f)
            };

            for (int corner = 0; corner < 4; ++corner)
            {
                data.vertices.Add(middleRectangleVertices[corner]);
                data.uv.Add(middleRectangleUV[corner]);
            }

            int numberOfVerticesPerCorner = cornerSlices + 1;
            for (int corner = 0, i = 0; corner < 4; ++corner, --i)
            {
                Vector3 vertexPivot = middleRectangleVertices[corner];
                Vector2 uvPivot = middleRectangleUV[corner];
                for (int count = 0; count < numberOfVerticesPerCorner; ++count, ++i)
                {
                    Vector2 aux = circle[i];
                    data.vertices.Add(vertexPivot + new Vector3(aux.x, 0, aux.y));
                    data.uv.Add(uvPivot + new Vector2(aux.x / length, aux.y / width));
                }
            }

            data.FillNormals(Vector3.up);

            for (int corner = 0; corner < 4; ++corner)
                data.AddTriangleFan(corner, 4 + corner * numberOfVerticesPerCorner, numberOfVerticesPerCorner);

            int corner0_start = 4,
                corner0_end = corner0_start + cornerSlices,
                corner1_start = corner0_end + 1,
                corner1_end = corner1_start + cornerSlices,
                corner2_start = corner1_end + 1,
                corner2_end = corner2_start + cornerSlices,
                corner3_start = corner2_end + 1,
                corner3_end = corner3_start + cornerSlices;

            data.AddQuad(corner3_end, corner2_start, corner1_end, corner0_start);
            data.AddQuad(0, 1, corner1_start, corner0_end);
            data.AddQuad(corner3_start, corner2_end, 2, 3);

            return data;
        }
        public static MeshBuilder CreateHemisphere(float radius, int verticalSlices, int radialSlices)
        {
            MeshBuilder data = new MeshBuilder();
            if (verticalSlices < 1) verticalSlices = 1;
            if (radialSlices < 3) radialSlices = 3;

            var circle = GeneretaUnityCirclePoints(0, 2 * Mathf.PI / radialSlices, radialSlices);
            float deltaAngle = Mathf.PI / (2 * verticalSlices),
                  angle = deltaAngle;

            data.vertices.Add(new Vector3(0, radius, 0));
            data.normals.Add(Vector3.up);
            data.uv.Add(new Vector2(0.5f, 0.5f));

            float cosAngle = Mathf.Cos(angle),
                  sinAngle = Mathf.Sin(angle),
                  uv_radius = angle / Mathf.PI;

            for (int n = 0; n < radialSlices; ++n)
            {
                Vector3 normal = new Vector3(sinAngle * circle[n].x, cosAngle, sinAngle * circle[n].y);
                data.normals.Add(normal);
                data.vertices.Add(normal * radius);
                data.uv.Add(new Vector2(0.5f + uv_radius * circle[n].x, 0.5f + uv_radius * circle[n].y));
            }

            data.AddClosedTriangleFan(0, 1, radialSlices);

            int stripStart1 = 1,
                stripStart2;

            for (int k = 1; k < verticalSlices; ++k)
            {
                stripStart2 = data.NumberOfVertices;

                angle += deltaAngle;
                cosAngle = Mathf.Cos(angle);
                sinAngle = Mathf.Sin(angle);
                uv_radius = angle / Mathf.PI;

                for (int n = 0; n < radialSlices; ++n)
                {
                    Vector3 normal = new Vector3(sinAngle * circle[n].x, cosAngle, sinAngle * circle[n].y);
                    data.normals.Add(normal);
                    data.vertices.Add(normal * radius);
                    data.uv.Add(new Vector2(0.5f + uv_radius * circle[n].x, 0.5f + uv_radius * circle[n].y));
                }

                data.AddClosedTriangleStrip(stripStart1, stripStart2, radialSlices);
                stripStart1 = stripStart2;
            }

            return data;
        }

        public static MeshBuilder CreateTowerOfHanoiDisk(float innerRadius, float outerRadius, float height, int radialSlices, int verticalSlices)
        {
            MeshBuilder data = new MeshBuilder();
            if (radialSlices < 3) radialSlices = 3;
            if (verticalSlices < 2) verticalSlices = 2;

            int numberOfVerticesPerSlice = 3 + verticalSlices;
            float sideDeltaAngle = Mathf.PI / verticalSlices,
                    radialDeltaAngle = 360 / radialSlices,
                    sideRadius = height / 2,
                    outerTopRadius = outerRadius - sideRadius,
                    middleDistance = outerTopRadius + Mathf.PI * sideRadius / 2,
                    uvFactor = 0.5f / middleDistance;
            var unityCircle = GeneretaUnityCirclePoints(Mathf.PI / 2 - sideDeltaAngle, -sideDeltaAngle, verticalSlices - 1);
            Quaternion verticesRotation = Quaternion.AngleAxis(radialDeltaAngle, Vector3.down);
            Quaternion uvRotation = Quaternion.AngleAxis(radialDeltaAngle, Vector3.forward);

            data.vertices.Add(new Vector3(innerRadius, sideRadius, 0));
            data.normals.Add(Vector3.up);
            data.uv.Add(new Vector2(innerRadius * uvFactor, 0));

            data.vertices.Add(new Vector3(outerTopRadius, sideRadius, 0));
            data.normals.Add(Vector3.up);
            data.uv.Add(new Vector2(outerTopRadius * uvFactor, 0));

            float distance = outerTopRadius,
                    deltaDistance = sideDeltaAngle * sideRadius;

            for (int n = 0; n < unityCircle.Length; ++n)
            {
                distance += deltaDistance;
                var aux = unityCircle[n];
                data.vertices.Add(new Vector3(outerTopRadius + aux.x * sideRadius, aux.y * sideRadius, 0));
                data.normals.Add(aux);
                data.uv.Add(new Vector2((middleDistance - Mathf.Abs(distance - middleDistance)) * uvFactor, 0));
            }

            data.vertices.Add(new Vector3(outerTopRadius, -sideRadius, 0));
            data.normals.Add(Vector3.down);
            data.uv.Add(new Vector2(outerTopRadius * uvFactor, 0));

            data.vertices.Add(new Vector3(innerRadius, -sideRadius, 0));
            data.normals.Add(Vector3.down);
            data.uv.Add(new Vector2(innerRadius * uvFactor, 0));

            int sewIndex1 = 0, sewIndex2;
            for (int n = 1; n < radialSlices; ++n)
            {
                sewIndex2 = data.NumberOfVertices;
                data.DuplicateLastVerticesNormalsAndUV(numberOfVerticesPerSlice);
                data.RotateLastVertices(verticesRotation, numberOfVerticesPerSlice);
                data.RotateLastUV(uvRotation, numberOfVerticesPerSlice);
                data.AddTriangleStrip(sewIndex2, sewIndex1, numberOfVerticesPerSlice);
                sewIndex1 = sewIndex2;
            }
            data.AddTriangleStrip(0, data.NumberOfVertices - numberOfVerticesPerSlice, numberOfVerticesPerSlice);
            data.TranslateUV(new Vector2(0.5f, 0.5f));

            sewIndex2 = data.NumberOfVertices;
            for (int n = 0, i = 0; n < radialSlices; ++n, i += numberOfVerticesPerSlice)
            {
                var aux = data.vertices[i];
                data.vertices.Add(aux);
                data.normals.Add(-aux.normalized);
                data.uv.Add(data.uv[i]);
            }

            sewIndex1 = data.NumberOfVertices;
            for (int n = 0, i = numberOfVerticesPerSlice - 1; n < radialSlices; ++n, i += numberOfVerticesPerSlice)
            {
                var aux = data.vertices[i];
                data.vertices.Add(aux);
                data.normals.Add(-aux.normalized);
                data.uv.Add(data.uv[i]);
            }

            data.AddClosedTriangleStrip(sewIndex1, sewIndex2, radialSlices);

            return data;
        }
        public static MeshBuilder CreateTowerOfHanoiBaseSideStrip(float length, float width, float height, float cornerRadius, int cornerSlices)
        {
            MeshBuilder data = new MeshBuilder();
            float halfLength = length / 2,
                  halfWidth = width / 2;
            var circle0 = GeneretaUnityCirclePoints(0, Mathf.PI / (2 * cornerSlices), cornerSlices + 1);
            int numberOfTriangles, numberOfVertices, verticesUpIndex;
            Vector3 pivot = new Vector3(halfLength - cornerRadius, 0, halfWidth - cornerRadius);
            Vector3 verticesTranslation = new Vector3(0, height, 0);

            data.vertices.Add(new Vector3(halfLength, 0, 0));
            data.normals.Add(Vector3.right);

            for (int n = 0; n < circle0.Length; ++n)
            {
                Vector2 aux = circle0[n];
                data.vertices.Add(new Vector3(pivot.x + aux.x * cornerRadius, 0, pivot.z + aux.y * cornerRadius));
                data.normals.Add(new Vector3(aux.x, 0, aux.y));
            }

            pivot.Set(-pivot.x, 0, pivot.z);
            for (int n = 0; n < circle0.Length; ++n)
            {
                Vector2 aux = circle0[n];
                data.vertices.Add(new Vector3(pivot.x - aux.y * cornerRadius, 0, pivot.z + aux.x * cornerRadius));
                data.normals.Add(new Vector3(-aux.y, 0, aux.x));
            }
            data.vertices.Add(new Vector3(-halfLength, 0, 0));
            data.normals.Add(Vector3.left);

            verticesUpIndex = data.NumberOfVertices;
            numberOfVertices = verticesUpIndex;

            data.DuplicateLastVertices(numberOfVertices);
            data.DuplicateLastNormals(numberOfVertices);
            data.TranslateLastVertices(verticesTranslation, numberOfVertices);
            data.AddTriangleStrip(verticesUpIndex, 0, numberOfVertices);

            numberOfVertices = data.NumberOfVertices;
            numberOfTriangles = data.NumberOfTriangles;

            data.DuplicateLastVertices(numberOfVertices);
            data.MirrorLastVerticesXZ(numberOfVertices);
            data.DuplicateLastNormals(numberOfVertices);
            data.MirrorLastNormalsXZ(numberOfVertices);
            data.DuplicateLastTriangles(numberOfTriangles, numberOfVertices);

            data.AddTowerOfHanoiHalfBaseSideUV(1, 0, cornerRadius, cornerSlices, halfLength, halfWidth);
            data.DuplicateUV();

            return data;
        }
        public static MeshBuilder CreateTowerOfHanoiBaseSide(float length, float width, float stepHeight, float stepWidth, int numberOfSteps, float cornerRadius, int cornerSlices)
        {
            MeshBuilder data = new MeshBuilder();
            if (numberOfSteps == 0)
                return data;

            Vector2 uvScale = new Vector2(1f, 1f / numberOfSteps);
            for (int step = 0; step < numberOfSteps; ++step)
            {
                var aux = CreateTowerOfHanoiBaseSideStrip(length, width, stepHeight, cornerRadius, cornerSlices);
                aux.TranslateVertices(new Vector3(0, step * stepHeight, 0));
                aux.ScaleUV(uvScale);
                aux.TranslateUV(new Vector2(0, uvScale.y * step));
                data.MergeSubmeshes(aux);

                length -= 2 * stepWidth;
                width -= 2 * stepWidth;
            }

            return data;
        }
        public static MeshBuilder CreateTowerOfHanoiBaseTop(float length, float width, float stepHeight, int numberOfSteps, float stepWidth, float cornerRadius, int cornerSlices)
        {
            if (numberOfSteps == 0)
                return CreateRoundRectangle(length, width, cornerRadius, cornerSlices);

            MeshBuilder data = new MeshBuilder();
            float height = stepHeight,
                  currentLength = length,
                  currentWidth = width;
            int numberOfVertices;

            for (int step = 1; step < numberOfSteps; ++step)
            {
                numberOfVertices = data.NumberOfVertices;
                data.AddRoundRectangleStripVerticesAndTriangles(currentLength, currentWidth, stepWidth, cornerRadius, cornerSlices);
                numberOfVertices = data.NumberOfVertices - numberOfVertices;
                data.TranslateLastVertices(Vector3.up * height, numberOfVertices);
                height += stepHeight;
                currentLength -= 2 * stepWidth;
                currentWidth -= 2 * stepWidth;
            }

            numberOfVertices = data.NumberOfVertices;
            data.AddRoundRectangleVerticesAndTriangles(currentLength, currentWidth, cornerRadius, cornerSlices);
            numberOfVertices = data.NumberOfVertices - numberOfVertices;
            data.TranslateLastVertices(Vector3.up * height, numberOfVertices);

            for (int n = 0; n < data.NumberOfVertices; ++n)
            {
                Vector3 aux = data.vertices[n];
                data.uv.Add(new Vector2(aux.x / length + 0.5f, aux.z / width + 0.5f));
            }

            data.FillNormals(Vector3.up);

            return data;
        }
        public static MeshBuilder CreateHanoitTowerRods(float distanceBetweenRods, float radius, float height, int radialSlices, int capVerticalSlices)
        {
            float cylinderHeight = height - radius;

            var shell = CreateCylinderShell(radius, cylinderHeight, radialSlices);
            shell.TranslateVertices(new Vector3(0, cylinderHeight / 2, 0));

            int numberOfVertices = shell.NumberOfVertices;
            int numberOfTriangles = shell.NumberOfTriangles;
            shell.DuplicateTriangles(0, numberOfTriangles, shell.NumberOfVertices);
            shell.DuplicateVerticesNormalsAndUV(0, numberOfVertices);
            shell.TranslateLastVertices(Vector3.left * distanceBetweenRods, numberOfVertices);

            shell.DuplicateTriangles(0, numberOfTriangles, shell.NumberOfVertices);
            shell.DuplicateVerticesNormalsAndUV(0, numberOfVertices);
            shell.TranslateLastVertices(Vector3.right * distanceBetweenRods, numberOfVertices);

            var cap = CreateHemisphere(radius, capVerticalSlices, radialSlices);
            cap.TranslateVertices(new Vector3(0, cylinderHeight, 0));

            numberOfVertices = cap.NumberOfVertices;
            numberOfTriangles = cap.NumberOfTriangles;

            cap.DuplicateTriangles(0, numberOfTriangles, cap.NumberOfVertices);
            cap.DuplicateVerticesNormalsAndUV(0, numberOfVertices);
            cap.TranslateLastVertices(Vector3.left * distanceBetweenRods, numberOfVertices);

            cap.DuplicateTriangles(0, numberOfTriangles, cap.NumberOfVertices);
            cap.DuplicateVerticesNormalsAndUV(0, numberOfVertices);
            cap.TranslateLastVertices(Vector3.right * distanceBetweenRods, numberOfVertices);

            shell.AddSubmeshes(cap);
            return shell;
        }
        public static MeshBuilder CreateTowerOfHanoiBase(float length, float width, float stepHeight, float stepWidth, int numberOfSteps, float cornerRadius, int cornerSlices,
                                                       float distanceBetweenRods, float rodRadius, float rodHeight, int rodRadialSlices, int rodCapVerticalSlices)
        {
            var data = CreateTowerOfHanoiBaseTop(length, width, stepHeight, numberOfSteps, stepWidth, cornerRadius, cornerSlices);
            var bottom = CreateRoundRectangle(length, width, cornerRadius, cornerSlices);
            bottom.InvertAllTriangles();
            bottom.FlipNormals();
            var side = CreateTowerOfHanoiBaseSide(length, width, stepHeight, stepWidth, numberOfSteps, cornerRadius, cornerSlices);
            var rods = CreateHanoitTowerRods(distanceBetweenRods, rodRadius, rodHeight, rodRadialSlices, rodCapVerticalSlices);
            rods.TranslateVertices(new Vector3(0, stepHeight * numberOfSteps, 0));

            data.MergeSubmeshes(bottom);
            data.AddSubmeshes(side);
            data.AddSubmeshes(rods);
            return data;
        }
    }
}