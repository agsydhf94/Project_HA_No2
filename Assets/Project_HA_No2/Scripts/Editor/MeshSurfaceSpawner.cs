using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Editor tool that spawns prefabs randomly on the surface of a target mesh.
    /// It samples triangle geometry to place objects in world space with surface-normal alignment.
    /// </summary>
    public class MeshSurfaceSpawner : EditorWindow
    {
        public GameObject targetMeshObject;     // The mesh on which prefabs will be spawned
        public GameObject prefabToSpawn;        // Prefab to instantiate
        public float spawnOffset = 0.05f;       // Offset along normal direction
        public int spawnCount = 100;            // Total number of prefabs to spawn


        /// <summary>
        /// Adds a menu item to open the spawner window.
        /// </summary>
        [MenuItem("Tools/Mesh Surface Spawner (Simple)")]
        public static void ShowWindow()
        {
            GetWindow<MeshSurfaceSpawner>("Simple Surface Spawner");
        }


        /// <summary>
        /// Renders the custom editor GUI for input fields and buttons.
        /// </summary>
        void OnGUI()
        {
            targetMeshObject = (GameObject)EditorGUILayout.ObjectField("Target Mesh", targetMeshObject, typeof(GameObject), true);
            prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabToSpawn, typeof(GameObject), false);
            spawnOffset = EditorGUILayout.FloatField("Spawn Offset", spawnOffset);
            spawnCount = EditorGUILayout.IntField("Spawn Count", spawnCount);

            if (GUILayout.Button("Spawn Prefabs on Entire Mesh"))
            {
                SpawnPrefabs();
            }
        }


        /// <summary>
        /// Spawns a specified number of prefabs randomly distributed over the triangles of the mesh surface.
        /// Calculates barycentric coordinates to get uniformly distributed spawn points.
        /// </summary>
        private void SpawnPrefabs()
        {
            if (targetMeshObject == null || prefabToSpawn == null) return;

            Mesh mesh = targetMeshObject.GetComponent<MeshFilter>().sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] tris = mesh.triangles;
            Transform tf = targetMeshObject.transform;

            int spawned = 0;
            while (spawned < spawnCount)
            {
                int triIndex = Random.Range(0, tris.Length / 3) * 3;
                int i0 = tris[triIndex];
                int i1 = tris[triIndex + 1];
                int i2 = tris[triIndex + 2];

                // Generate random barycentric coordinates
                float r1 = Random.value;
                float r2 = Random.value;
                if (r1 + r2 > 1f) { r1 = 1f - r1; r2 = 1f - r2; }

                Vector3 a = vertices[i0], b = vertices[i1], c = vertices[i2];
                Vector3 localPos = a + r1 * (b - a) + r2 * (c - a);
                Vector3 worldPos = tf.TransformPoint(localPos);
                Vector3 normal = tf.TransformDirection(Vector3.Cross(b - a, c - a).normalized);

                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefabToSpawn);
                obj.transform.position = worldPos + normal * spawnOffset;
                obj.transform.rotation = Quaternion.LookRotation(normal);
                Undo.RegisterCreatedObjectUndo(obj, "Spawn Prefab");
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                spawned++;
            }

            Debug.Log($"[MeshSurfaceSpawner] Spawned {spawned} prefabs.");
        }
    }
}
