using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class MeshFilterExtensions
    {
        [MenuItem("CONTEXT/MeshFilter/Save Mesh As New...")]
        public static void SaveMeshAsNew(MenuCommand menuCommand)
        {
            var mf = menuCommand.context as MeshFilter;
            var m = mf.sharedMesh;

            SaveMesh(m, m.name, mf.gameObject.transform, false);
        }

        [MenuItem("CONTEXT/MeshFilter/Save Mesh As New With Transform...")]
        public static void SaveMeshAsNewWithTransform(MenuCommand menuCommand)
        {
            var mf = menuCommand.context as MeshFilter;
            var m = mf.sharedMesh;

            SaveMesh(m, m.name, mf.gameObject.transform, true);
        }

        private static void SaveMesh(Mesh mesh, string name, Transform parentTransform, bool rescale)
        {
            var path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/Models", name, "asset");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            path = FileUtil.GetProjectRelativePath(path);

            var meshToSave = Object.Instantiate(mesh);
            if (rescale)
            {
                var vertices = meshToSave.vertices;
                var parentPosition = parentTransform.position;

                for (var i = 0; i < vertices.Length; i++)
                {
                    var initialVertex = vertices[i];
                    var transformedVertex = parentTransform.TransformPoint(initialVertex);
                    var resultingVertex = transformedVertex - parentPosition;

                    vertices[i] = resultingVertex;
                }
                meshToSave.vertices = vertices;
            }
            MeshUtility.Optimize(meshToSave);

            AssetDatabase.CreateAsset(meshToSave, path);
            AssetDatabase.SaveAssets();
        }
    }
}
