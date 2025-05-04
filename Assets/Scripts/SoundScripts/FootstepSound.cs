using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource footstepAudioSource;

    [Tooltip("Footstep sounds mapped to terrain texture index")]
    public List<TerrainFootstep> terrainFootsteps;

    [Header("Step Timing")]
    public float minStepInterval = 0.2f;
    public float maxStepInterval = 0.5f;
    public float maxSpeed = 6f;

    [Header("Character Movement")]
    public CharacterController controller;

    private float stepTimer;
    private int lastPlayedIndex = -1;

    void Update()
    {
        if (controller == null) return;

        float speed = controller.velocity.magnitude;
        bool isMoving = speed > 0.1f;

        if (controller.isGrounded && isMoving)
        {
            float stepInterval = Mathf.Lerp(maxStepInterval, minStepInterval, speed / maxSpeed);
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset timer and stop any current footstep sound
            stepTimer = 0f;
            if (footstepAudioSource.isPlaying)
                footstepAudioSource.Stop();
        }
    }

    void PlayFootstep()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 2f))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                int textureIndex = GetMainTexture(hit.point, terrain);
                AudioClip[] clips = GetFootstepClips(textureIndex);

                if (clips != null && clips.Length > 0)
                {
                    lastPlayedIndex = (lastPlayedIndex + 1) % clips.Length;
                    AudioClip selectedClip = clips[lastPlayedIndex];

                    footstepAudioSource.clip = selectedClip;
                    footstepAudioSource.Play();
                }
            }
        }
    }

    int GetMainTexture(Vector3 worldPos, Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = worldPos - terrain.transform.position;

        Vector3 splatMapPos = new Vector3(
            terrainPos.x / terrainData.size.x,
            0,
            terrainPos.z / terrainData.size.z
        );

        int mapX = Mathf.Clamp((int)(splatMapPos.x * terrainData.alphamapWidth), 0, terrainData.alphamapWidth - 1);
        int mapZ = Mathf.Clamp((int)(splatMapPos.z * terrainData.alphamapHeight), 0, terrainData.alphamapHeight - 1);

        float[,,] alphaMap = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int maxIndex = 0;
        float maxMix = 0f;

        for (int i = 0; i < terrainData.terrainLayers.Length; i++)
        {
            if (alphaMap[0, 0, i] > maxMix)
            {
                maxIndex = i;
                maxMix = alphaMap[0, 0, i];
            }
        }

        return maxIndex;
    }

    AudioClip[] GetFootstepClips(int textureIndex)
    {
        foreach (var tf in terrainFootsteps)
        {
            if (tf.terrainTextureIndex == textureIndex)
                return tf.footstepClips;
        }
        return null;
    }
}

[System.Serializable]
public class TerrainFootstep
{
    public int terrainTextureIndex;
    public AudioClip[] footstepClips;
}


