using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BarParu;

public class CharacterController : MonoBehaviour
{
    public CharacterType type = new CharacterType();
    public Type.Aliy aliy;
    public int order;
    public SPUM_Prefabs model;
    public CircleCollider2D myCol;
    public float movementSpeed = 5f;
    public float approachRange = 10f;
    public float updateInterval = 0.5f;

    private Rigidbody2D rb;
    public CharacterController closestEnemy;
    public bool isStop = true; // 캐릭터의 멈춤 상태를 추적.
    public bool isHit = false; // 캐릭터의 공격 상태를 추적.
    public bool isDead = false; // 캐릭터의 사망 상태를 추적.
    private bool isReturning = false;  // 리더 근처로 돌아가는 상태를 나타내는 변수


    private void Start()
    {
        // 이후 캐릭터 생성시에 작동하게 변경
        if(aliy == Type.Aliy.Player)
        {
            OnStart();
        }
        else if(aliy == Type.Aliy.Enemy)
        {

        }
        SetType(aliy);
    }
    public void SetType(Type.Aliy type)
    {
        if (aliy == Type.Aliy.Player)
        {
            Character character = DataManager.inst.characterData[model._code];

            this.type._class = character._class;
            this.type._weapon = character._weapon;
            this.type._mainStatus = character._mainStatus;
            this.type._element = character._element;

            model._anim.SetFloat("NormalState", character._weapon.attackType);
            myCol.radius = character._weapon.range;
        }
        else if (aliy == Type.Aliy.Enemy)
        {
            Enemy character = DataManager.inst.enemyData[model._code];

            this.type._class = character._class;
            this.type._weapon = character._weapon;
            this.type._mainStatus = character._mainStatus;
            this.type._element = character._element;
        }
    }
    public void ResetCharacter()
    {
        isStop = false;
        isHit = false;
        isDead = false;
        closestEnemy = null;
        myCol.enabled = true;
    }
    public void OnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(UpdateBehavior());
    }

    IEnumerator UpdateBehavior()
    {
        while (true)
        {
            if (isReturning)
            {
                // 리더 근처로 돌아가는 로직
                MoveTowardsCustom(GameManager.inst.party.GetLeader().transform.position);
                if (Vector3.Distance(transform.position, GameManager.inst.party.GetLeader().transform.position) <= GameManager.inst.party.repositionRadius)
                {
                    isReturning = false;  // 도달했으므로 상태 업데이트
                }
            }
            else if (!isHit && !isDead)
            {
                closestEnemy = FindClosestEnemy();
                if (closestEnemy != null)
                {
                    MoveTowards(closestEnemy);
                }
                else
                {
                    isStop = true;
                }
            }
            else
            {
                closestEnemy = null;
            }

            if (isStop || isHit)
            {
                model._anim.SetFloat("RunState", 0f);
            }
            else
            {
                model._anim.SetFloat("RunState", .5f);
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

    // 커스텀 이동 메소드. 특정 방향으로 특정 거리만큼 이동
    public void MoveTowardsCustom(Vector3 targetPosition)
    {
        if (isReturning)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            // x축 방향에 따른 조건 분기
            if (direction.x > 0)
            {
                // x축이 + 방향으로 움직일 때의 로직
                model.transform.localScale = new Vector2(-1, 1);
            }
            else if (direction.x < 0)
            {
                // x축이 - 방향으로 움직일 때의 로직
                model.transform.localScale = new Vector2(1, 1);
            }
            float distance = Vector3.Distance(transform.position, targetPosition);

            // 이동할 때 점점 가속되도록 속도를 조정
            float speed = Mathf.Min(distance, movementSpeed * Time.deltaTime);

            rb.MovePosition(transform.position + direction * speed);
        }
    }
    // 커스텀 이동 메소드. 특정 방향으로 특정 거리만큼 이동
    public void MoveTowardsCustom(Vector3 direction, float distance)
    {
        Vector3 movePosition = transform.position + direction * Mathf.Min(distance, movementSpeed * Time.deltaTime);
        rb.MovePosition(movePosition);
    }

    CharacterController FindClosestEnemy()
    {
        CharacterController[] allCharacters = FindObjectsOfType<CharacterController>();
        CharacterController closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (CharacterController character in allCharacters)
        {
            if (character.aliy != this.aliy && !character.isDead)
            {
                float distance = Vector3.Distance(character.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    closest = character;
                    minDistance = distance;
                }
            }
        }
        return closest;
    }

    void MoveTowards(CharacterController target)
    {
        Vector3 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance > approachRange)
        {
            isStop = false; // 캐릭터가 움직이고 있습니다.
            direction.Normalize();
            rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);

            // x축 방향에 따른 조건 분기
            if (direction.x > 0)
            {
                // x축이 + 방향으로 움직일 때의 로직
                model.transform.localScale = new Vector2(-1, 1);
            }
            else if (direction.x < 0)
            {
                // x축이 - 방향으로 움직일 때의 로직
                model.transform.localScale = new Vector2(1, 1);
            }
        }
        else
        {
            isStop = true; // 캐릭터가 멈춰 있습니다.
            rb.velocity = Vector2.zero;
        }
    }

    public void Attack(Collider2D collision)
    {
        if (!isHit && !isDead)
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();

            // x축 방향에 따른 조건 분기
            if (direction.x > 0)
            {
                // x축이 + 방향으로 움직일 때의 로직
                model.transform.localScale = new Vector2(-1, 1);
            }
            else if (direction.x < 0)
            {
                // x축이 - 방향으로 움직일 때의 로직
                model.transform.localScale = new Vector2(1, 1);
            }
            model._anim.SetFloat("AttackState", 0);
            model._anim.SetTrigger("Attack");
            StartCoroutine(HitEffect(collision));

            if (ShouldReturnToLeader() && aliy == Type.Aliy.Player)
            {
                isReturning = true;
            }
        }
    }
    // 리더 근처로 돌아가야 하는지 결정하는 메소드
    private bool ShouldReturnToLeader()
    {
        float distanceFromLeader = Vector3.Distance(transform.position, GameManager.inst.party.GetLeader().transform.position);
        return distanceFromLeader > GameManager.inst.party.maxDistance;
    }
    IEnumerator HitEffect(Collider2D collision)
    {
        isHit = true;
        if(aliy == Type.Aliy.Player)
        {
            Hit hit = ParuPool.inst.GetPoolItem<Hit>("Hit");
            hit.cnt = 0;
            hit.hitCnt = type._weapon.hitCount;
            hit.hitAliy = aliy;
            hit.transform.SetParent(GameManager.inst.hitParents);
            hit.transform.position = collision.transform.position;
            hit.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            ParuPool.inst.ReturnPoolObject("Hit", hit.gameObject);
        }
        yield return new WaitForSeconds(.5f);
        isHit = false;
    }
    public void Skill()
    {
        model._anim.SetFloat("AttackState", 1);
        model._anim.SetTrigger("Attack");
    }
    public void Hit()
    {
        // Hit logic here
    }
    public void Dead()
    {
        isDead = true;
        myCol.enabled = false;
        closestEnemy = null;
        isStop = true;
        model._anim.SetTrigger("Die");
        StartCoroutine(ReturnObejct());

        IEnumerator ReturnObejct()
        {
            yield return new WaitForSeconds(3f);
            ParuPool.inst.ReturnPoolObject(model._code, gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && aliy == Type.Aliy.Player)
        {
            Attack(collision);
        }
        else if (collision.CompareTag("Player") && aliy == Type.Aliy.Enemy)
        {
            Attack(collision);
        }
    }
}

[System.Serializable]
public class CharacterType
{
    public ClassType _class;
    public Weapon _weapon;
    public Type.MainStatus _mainStatus;
    public Element _element;
}
