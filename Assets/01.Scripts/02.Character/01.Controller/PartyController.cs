using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    public List<CharacterController> partyMembers; // ��Ƽ ��� ���
    public float maxDistance = 5f; // ��Ƽ ����� ������ �ִ� ��� �Ÿ�
    public float repositionRadius = 2f; // ���� �ֺ����� ���ġ�� ���� �ݰ�

    private void Update()
    {
        CharacterController leader = GetLeader();
        if (leader == null) return;

        foreach (CharacterController member in partyMembers)
        {
            if (member != leader && !member.isDead)
            {
                float distance = Vector3.Distance(member.transform.position, leader.transform.position);
                if (distance > maxDistance)
                {
                    // ���� ��ó�� ������ ��ġ�� ����մϴ�.
                    Vector3 repositionPoint = GetRepositionPoint(leader.transform.position, repositionRadius);
                    member.MoveTowardsCustom(repositionPoint);
                }
            }
        }
    }

    // ���� ��ó�� ���ġ ������ ����ϴ� �޼ҵ�
    Vector3 GetRepositionPoint(Vector3 leaderPosition, float radius)
    {
        // ���� �ֺ����� ������ ����� �Ÿ��� ����Ͽ� ������ �����մϴ�.
        Vector3 randomDirection = Random.insideUnitCircle.normalized;
        return leaderPosition + randomDirection * radius;
    }

    // ���� ĳ���͸� ã�� �޼ҵ�
    public CharacterController GetLeader()
    {
        CharacterController leader = null;
        int minOrder = int.MaxValue;
        foreach (CharacterController member in partyMembers)
        {
            if (member.order < minOrder && !member.isDead)
            {
                leader = member;
                minOrder = member.order;
            }
        }
        return leader;
    }
}
