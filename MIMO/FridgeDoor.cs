using UnityEngine;

public class FridgeDoor : MonoBehaviour
{
    [SerializeField]
    float ROTATIONDEGREE = 0.0f;
    bool isClosed = true;
    public bool open()
    {
        this.isClosed = !this.isClosed;
        Animation();
        return this.isClosed;
    }

    private void Animation()
    {
        Vector3 toZ = new Vector3(0, 0, this.isClosed ? -ROTATIONDEGREE : ROTATIONDEGREE);
        //Vector3 toY = new Vector3(0, this.isClosed ? -ROTATIONDEGREE : ROTATIONDEGREE, 0) ;
        //Vector3 toX = new Vector3(this.isClosed ? -ROTATIONDEGREE : ROTATIONDEGREE, 0, 0);
        var mainDoor = GameObject.FindGameObjectWithTag("MainFridgeDoor");
        mainDoor.transform.Rotate(toZ);
    }
}
