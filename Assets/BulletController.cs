using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void Start()
    {
        Vector3 mouseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseClickPosition.z = 0f;

        this.GetComponent<Rigidbody>().linearVelocity = (mouseClickPosition - this.transform.position).normalized * 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

        }
    }
}
