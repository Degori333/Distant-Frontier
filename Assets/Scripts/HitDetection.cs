using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    private EnemyMovement parent;

    private void Start()
    {
        parent = GetComponentInParent<EnemyMovement>();
    }

    private void Update()
    {
        transform.position = parent.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            parent.enemyHP -= parent.playerStatus.weapons[parent.playerStatus.currentWeapon].GetComponent<Weapon>().damage;
            parent.enemyHealthBar.gameObject.SetActive(true);
            parent.enemyHealthBar.value = parent.enemyHP;
            if (parent.enemyHP <= 0)
            {
                GetComponent<CapsuleCollider>().enabled = false;
                if (!parent.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                {
                    parent.animator.SetTrigger("Die");
                }
                parent.isDead = true;
                if (!parent.audioSource.isPlaying)
                {
                    parent.audioSource.PlayOneShot(parent.enemyDyingSounds[Random.Range(0, parent.enemyDyingSounds.Length)]);
                }
                StartCoroutine(parent.DestroyObject(parent.gameObject));
            }
        }
    }
}
