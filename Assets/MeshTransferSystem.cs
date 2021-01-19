using System.Collections.Generic;
using UnityEngine;

public class MeshTransferSystem : MonoBehaviour
{
    public SkinnedMeshRenderer meshToClone;
    public bool CloneMesh;
    public List<int> blendsToClone;
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
        Mesh clonedMesh = new Mesh();
        meshToClone.BakeMesh(clonedMesh);

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

            blendName = meshSharedClone.GetBlendShapeName(I);
            int frameCount = meshSharedClone.GetBlendShapeFrameCount(I);

            for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
            {
                float frameWeight = meshSharedClone.GetBlendShapeFrameWeight(I, frameIndex);
                meshSharedClone.GetBlendShapeFrameVertices(I, frameIndex, deltaVertices, deltaNormals, deltaTangets);
                clonedMesh.AddBlendShapeFrame(blendName, frameWeight, deltaVertices, deltaNormals, deltaTangets);
            }
        }

        meshToChange.sharedMesh = clonedMesh;
    }
}