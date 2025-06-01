using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void Start()
    {
        Vector3 mouseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseClickPosition.z = 0f;

        this.GetComponent<Rigidbody>().linearVelocity = (mouseClickPosition - this.transform.position).normalized * 10f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("EnemyHurtBox") || collision.gameObject.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<StateMachine>().TakeDamage();
        }
        Destroy(this.gameObject);
    }
}
