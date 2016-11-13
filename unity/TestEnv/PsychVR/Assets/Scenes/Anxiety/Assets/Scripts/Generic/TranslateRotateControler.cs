using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Charles Will Code It/Generic/TranslateRotateControler")]
public class TranslateRotateControler : MonoBehaviour
{

    public float TranslateSpeed = 1;
    public float RotateSpeed = 1;

    public KeyCode TranslateForwardKey = KeyCode.W;
    public KeyCode TranslateBackKey = KeyCode.S;
    public KeyCode TranslateLeftKey = KeyCode.A;
    public KeyCode TranslateRightKey = KeyCode.D;
    public KeyCode TranslateUpKey = KeyCode.P;
    public KeyCode TranslateDown = KeyCode.L;

    public KeyCode RotateForwardKey = KeyCode.UpArrow;
    public KeyCode RotateBackKey = KeyCode.DownArrow;
    public KeyCode RotateLeftKey = KeyCode.LeftArrow;
    public KeyCode RotateRightKey = KeyCode.RightArrow;

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey(TranslateForwardKey))
            transform.Translate(Vector3.forward * TranslateSpeed);
        if (Input.GetKey(TranslateBackKey))
            transform.Translate(Vector3.back * TranslateSpeed);
        if (Input.GetKey(TranslateLeftKey))
            transform.Translate(Vector3.left * TranslateSpeed);
        if (Input.GetKey(TranslateRightKey))
            transform.Translate(Vector3.right * TranslateSpeed);
        if (Input.GetKey(TranslateUpKey))
            transform.Translate(Vector3.up * TranslateSpeed);
        if (Input.GetKey(TranslateDown))
            transform.Translate(Vector3.down * TranslateSpeed);

        if (Input.GetKey(RotateForwardKey))
            transform.Rotate(Vector2.left * RotateSpeed);
        if (Input.GetKey(RotateBackKey))
            transform.Rotate(Vector2.right * RotateSpeed);
        if (Input.GetKey(RotateLeftKey))
            transform.Rotate(Vector2.down * RotateSpeed);
        if (Input.GetKey(RotateRightKey))
            transform.Rotate(Vector2.up * RotateSpeed);
    }
}
