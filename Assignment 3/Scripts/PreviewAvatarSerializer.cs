using Unity.Netcode;
using UnityEngine;

public class PreviewAvatarSerializer : NetworkBehaviour
{
    public GameObject previewAvatar;
    
    public override void OnNetworkSpawn()
    {
        previewAvatar.SetActive(false);

        if (IsOwner)
            return;
        
        ApplyPreviewUpdates();
    }
    
    private void Update()
    {
        if (IsOwner)
        {
            SerializePreviewUpdates();
        }
        else
        {
            ApplyPreviewUpdates();
        }
    }
    
    private void SerializePreviewUpdates()
    {
        
    }

    private void ApplyPreviewUpdates()
    {
        
    }
}
