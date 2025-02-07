using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    public List<CharacterController> partyMembers; // 파티 멤버 목록
    public float maxDistance = 5f; // 파티 멤버들 사이의 최대 허용 거리
    public float repositionRadius = 2f; // 리더 주변으로 재배치할 때의 반경

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
                    // 리더 근처의 임의의 위치를 계산합니다.
                    Vector3 repositionPoint = GetRepositionPoint(leader.transform.position, repositionRadius);
                    member.MoveTowardsCustom(repositionPoint);
                }
            }
        }
    }

    // 리더 근처의 재배치 지점을 계산하는 메소드
    Vector3 GetRepositionPoint(Vector3 leaderPosition, float radius)
    {
        // 리더 주변에서 랜덤한 방향과 거리를 사용하여 지점을 결정합니다.
        Vector3 randomDirection = Random.insideUnitCircle.normalized;
        return leaderPosition + randomDirection * radius;
    }

    // 리더 캐릭터를 찾는 메소드
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
