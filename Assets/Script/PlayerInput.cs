using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveHorizontalName = "Horizontal";
    public string moveVerticalName = "Vertical";

    public float moveHorizontal { get; private set; }
    public float moveVertical { get; private set; }
    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw(moveHorizontalName);
        moveVertical = Input.GetAxisRaw(moveVerticalName);
    }
}
