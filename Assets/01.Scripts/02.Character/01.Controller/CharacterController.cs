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
    public bool isStop = true; // ĳ������ ���� ���¸� ����.
    public bool isHit = false; // ĳ������ ���� ���¸� ����.
    public bool isDead = false; // ĳ������ ��� ���¸� ����.
    private bool isReturning = false;  // ���� ��ó�� ���ư��� ���¸� ��Ÿ���� ����


    private void Start()
    {
        // ���� ĳ���� �����ÿ� �۵��ϰ� ����
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
                // ���� ��ó�� ���ư��� ����
                MoveTowardsCustom(GameManager.inst.party.GetLeader().transform.position);
                if (Vector3.Distance(transform.position, GameManager.inst.party.GetLeader().transform.position) <= GameManager.inst.party.repositionRadius)
                {
                    isReturning = false;  // ���������Ƿ� ���� ������Ʈ
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

    // Ŀ���� �̵� �޼ҵ�. Ư�� �������� Ư�� �Ÿ���ŭ �̵�
    public void MoveTowardsCustom(Vector3 targetPosition)
    {
        if (isReturning)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            // x�� ���⿡ ���� ���� �б�
            if (direction.x > 0)
            {
                // x���� + �������� ������ ���� ����
                model.transform.localScale = new Vector2(-1, 1);
            }
            else if (direction.x < 0)
            {
                // x���� - �������� ������ ���� ����
                model.transform.localScale = new Vector2(1, 1);
            }
            float distance = Vector3.Distance(transform.position, targetPosition);

            // �̵��� �� ���� ���ӵǵ��� �ӵ��� ����
            float speed = Mathf.Min(distance, movementSpeed * Time.deltaTime);

            rb.MovePosition(transform.position + direction * speed);
        }
    }
    // Ŀ���� �̵� �޼ҵ�. Ư�� �������� Ư�� �Ÿ���ŭ �̵�
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
            isStop = false; // ĳ���Ͱ� �����̰� �ֽ��ϴ�.
            direction.Normalize();
            rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);

            // x�� ���⿡ ���� ���� �б�
            if (direction.x > 0)
            {
                // x���� + �������� ������ ���� ����
                model.transform.localScale = new Vector2(-1, 1);
            }
            else if (direction.x < 0)
            {
                // x���� - �������� ������ ���� ����
                model.transform.localScale = new Vector2(1, 1);
            }
        }
        else
        {
            isStop = true; // ĳ���Ͱ� ���� �ֽ��ϴ�.
            rb.velocity = Vector2.zero;
        }
    }

    public void Attack(Collider2D collision)
    {
        if (!isHit && !isDead)
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();

            // x�� ���⿡ ���� ���� �б�
            if (direction.x > 0)
            {
                // x���� + �������� ������ ���� ����
                model.transform.localScale = new Vector2(-1, 1);
            }
            else if (direction.x < 0)
            {
                // x���� - �������� ������ ���� ����
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
    // ���� ��ó�� ���ư��� �ϴ��� �����ϴ� �޼ҵ�
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
