using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{

    public float enemyHP = 1;
    public int enemyDMG = 1;
    public float speed = 1;
    public float attackSpeed = 1;
    public float attackRange = 1;
    public bool isDead;

    public int worth = 1;

    private bool playerInRange = false;
    private bool isAttacking = false;

    public Slider enemyHealthBar;
    private GameObject playerRef;
    public PlayerController playerStatus;
    private SpawnManager spawnManager;
    public Animator animator;
    public AudioClip[] enemyDyingSounds;
    public AudioClip[] enemyAttackSounds;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {   
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerRef = GameObject.Find("Player");
        playerStatus = playerRef.GetComponent<PlayerController>();
        enemyHealthBar.gameObject.SetActive(false);
        SetMaxHp();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Game Manager").GetComponent<GameManager>().gameStarted)
        {
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        // Move enemy towards player they are looking at
        if (playerStatus.alive && !isDead)
        {
            Vector3 playerPos = playerRef.transform.position;
            Vector3 distanceToPlayer = new Vector3(playerPos.x - transform.position.x, 0, playerPos.z - transform.position.z);
            if (distanceToPlayer.x < -attackRange || distanceToPlayer.x > attackRange ||
                distanceToPlayer.z < -attackRange || distanceToPlayer.z > attackRange)
            {
                if (!isAttacking)
                {
                    transform.Translate(Vector3.forward.normalized * speed * Time.deltaTime);
                    animator.SetBool("Walk Forward", true);
                }
            }
            else
            {
                animator.SetBool("Walk Forward", false);
            }
            transform.LookAt(playerRef.transform);
            enemyHealthBar.transform.rotation = GameObject.Find("Main Camera").transform.rotation;
            enemyHealthBar.transform.position = transform.position + new Vector3(0, 10.5f, 0);
        }
    }

    private void SetMaxHp()
    {
        enemyHP = (spawnManager.waveNumber + enemyHP) * 1.15f;
        enemyHealthBar.maxValue = enemyHP;
        enemyHealthBar.value = enemyHealthBar.maxValue;
        enemyHealthBar.enabled = false;
    }

    public IEnumerator DestroyObject(GameObject gameObject)
    {
        playerStatus.money += worth;
        yield return new WaitWhile(() => audioSource.isPlaying);
        Destroy(gameObject);
    }

    private void RandomAttack()
    {
        animator.SetBool("Walk Forward", false);
        int r = Random.Range(0, 3);
        if(r == 0) { animator.SetTrigger("Smash Attack"); }
        else if (r == 1) { animator.SetTrigger("Stab Attack"); }
        else { animator.SetTrigger("Cast Spell"); }
    }

    IEnumerator Attack(Collider other)
    {
        yield return new WaitForSeconds(attackRange);
        isAttacking = true;
        RandomAttack();
        audioSource.PlayOneShot(enemyAttackSounds[Random.Range(0, enemyAttackSounds.Length)]);
        yield return new WaitForSeconds(attackSpeed);
        isAttacking = false;
        if (playerInRange && playerStatus.alive)
        {
            playerStatus.setPlayerHP(playerStatus.getPlayerHP() - enemyDMG);
            OnTriggerEnter(other);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isAttacking)
            {   
                StartCoroutine(Attack(other));
            }
            if(playerStatus.getPlayerHP() <= 0 && playerStatus.alive)
            {

                StartCoroutine(playerStatus.Die());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }


}
