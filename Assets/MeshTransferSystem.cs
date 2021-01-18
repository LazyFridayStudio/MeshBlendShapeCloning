using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTransferSystem : MonoBehaviour
{
    public SkinnedMeshRenderer meshToClone;
    public Mesh ClonnedMeshSystem;
    public bool CloneMesh;
    public List<int> blendsToClone;

    public SkinnedMeshRenderer skinnedMeshRef;
    public SkinnedMeshRenderer meshToChange;
    

    // Start is called before the first frame update
    void Start()
    {
        blendsToClone.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (CloneMesh)
        {
            cloneMeshFunc();
            CloneMesh = false;
        }
    }

    void cloneMeshFunc()
    {
        Mesh meshSharedClone = meshToClone.sharedMesh;

        
        
        Mesh Vertextpoints = new Mesh();
        meshToClone.BakeMesh(Vertextpoints);
        
        Mesh clonedMesh = new Mesh();
        skinnedMeshRef.BakeMesh(clonedMesh);

        //meshSharedClone.vertices = clonedMesh.vertices;
        // clonedMesh.vertices = Vertextpoints.vertices;
       //  clonedMesh.triangles = Vertextpoints.triangles;
       //  clonedMesh.uv = Vertextpoints.uv;
       //  clonedMesh.normals = Vertextpoints.normals;
       //  clonedMesh.colors = Vertextpoints.colors;
       //  clonedMesh.tangents = Vertextpoints.tangents;
       // clonedMesh.bindposes = meshSharedClone.bindposes;

       clonedMesh.ClearBlendShapes();

        string blendName;
        Vector3[] deltaVertices;
        Vector3[] deltaNormals;
        Vector3[] deltaTangets;

        foreach (var I in blendsToClone)
        {
            deltaVertices = new Vector3[meshSharedClone.vertexCount];
            deltaNormals = new Vector3[meshSharedClone.vertexCount];
            deltaTangets = new Vector3[meshSharedClone.vertexCount];

            for (int shapeIndex = 0; shapeIndex < blendsToClone.Count; shapeIndex++)
            {
                blendName = meshSharedClone.GetBlendShapeName(I);
                int frameCount = meshSharedClone.GetBlendShapeFrameCount(shapeIndex);

                for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
                {
                    float frameWeight = meshSharedClone.GetBlendShapeFrameWeight(shapeIndex, frameIndex);
                    meshSharedClone.GetBlendShapeFrameVertices(shapeIndex, frameIndex, deltaVertices, deltaNormals, deltaTangets);
                    clonedMesh.AddBlendShapeFrame(blendName, frameWeight, deltaVertices, deltaNormals, deltaTangets);
                }
            }
        }

        ClonnedMeshSystem = clonedMesh;
        //meshToChange.sharedMesh = ClonnedMeshSystem;
    }
}