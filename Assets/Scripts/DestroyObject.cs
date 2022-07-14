using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    [SerializeField] private GameObject _objectToDestroy;
    
    public void DestroyObj()
    {
        Destroy(_objectToDestroy);
    }

}
