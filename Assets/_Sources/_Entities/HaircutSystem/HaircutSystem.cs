using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HaircutSystem : MonoBehaviour
{
    private bool _isCuttingPossible = true;
    public float force;
    public float delay;
    public GameObject hairEffectParticleGO;
    public bool isEnableEffect;

    public void BladeContact(Vector3 hitPosition, Cutter cutter)
    {
        DeformMesh(hitPosition, cutter);
    }

    #region MeshDeformation

    private async void DeformMesh(Vector3 hitPosition, Cutter cutter)
    {
        if (!_isCuttingPossible) return;

        StartCoroutine(CuttingCoolDownCoroutine());

        var vertices = cutter.Vertices;

        await CalculateVertexHairForDestroyAsync(vertices, hitPosition, cutter);

        if (isEnableEffect) PlayEffect(hitPosition);
        cutter.RedrawCuttingMesh(vertices);
    }

    private async Task CalculateVertexHairForDestroyAsync(Vector3[] vertices, Vector3 contactPoint, Cutter cutter)
    {
        List<Vector3> VerticesOnFinishPosition = new List<Vector3>();

        var hitAddressInCutting = Instantiate(new GameObject(), contactPoint, Quaternion.identity);
        hitAddressInCutting.transform.SetParent(cutter.transform);

        for (int i = 0; i < vertices.Length; i++)
        {
            var vertexOnHitDistance = Vector3.Distance(vertices[i], hitAddressInCutting.transform.localPosition);
            var vertexOnCenterDistance = Vector3.Distance(vertices[i], Vector3.zero);

            if (vertexOnHitDistance < force && vertexOnCenterDistance > force)
            {
                var balancedForce = force - vertexOnHitDistance;
                vertices[i] -= vertices[i].normalized * balancedForce;
            }

            //var balancedForce = force - vertexOnHitDistance;
            //vertices[i] -= vertices[i].normalized * balancedForce;
            //else Debug.LogError("Exeption here!  vertexOnHitDistance: " + vertexOnHitDistance + " | vertexOnCenterDistance= " + vertexOnCenterDistance + " | Force= " + force);

            if (vertexOnCenterDistance < force)
            {
                VerticesOnFinishPosition.Add(vertices[i]);
            }
        }

        if (VerticesOnFinishPosition.Count >= vertices.Length / 2)
        {
            cutter.IsMustDestroy = true;
        }

        await Task.Yield();
    }

    #endregion

    private IEnumerator CuttingCoolDownCoroutine()
    {
        _isCuttingPossible = false;
        yield return new WaitForSeconds(delay);
        hairEffectParticleGO.SetActive(false);
        _isCuttingPossible = true;
    }

    private void PlayEffect(Vector3 contactPosition)
    {
        hairEffectParticleGO.transform.position = contactPosition;
        hairEffectParticleGO.SetActive(true);
    }
}

