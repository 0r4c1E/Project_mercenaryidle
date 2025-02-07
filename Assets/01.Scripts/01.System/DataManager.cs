using BarParu;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager inst;

    public Dictionary<string, Character> characterData = new Dictionary<string, Character>();
    public Dictionary<string, Enemy> enemyData = new Dictionary<string, Enemy>();
    public Dictionary<string, Weapon> weaponData = new Dictionary<string, Weapon>();
    public Dictionary<string, ClassType> classData = new Dictionary<string, ClassType>();
    public Dictionary<string, Skill> skillData = new Dictionary<string, Skill>();

    [SerializeField]
    public Player player;


    private void Awake()
    {
        if (inst == null) inst = this;  //�̱���
    }

    private void Start()
    {
        CallData();
    }

    public void CallData()
    {
        LoadObjects<Character>("01.CharacterData");
        LoadObjects<Enemy>("02.EnemyData");
        LoadObjects<Weapon>("03.WeaponData");
        LoadObjects<ClassType>("04.ClassData");
        LoadObjects<Skill>("06.SkillData");
    }

    void LoadObjects<T>(string folderPath) where T : ScriptableObject, IIdentifiable
    {
        T[] objects = Resources.LoadAll<T>(folderPath);

        foreach (T obj in objects)
        {
            if (typeof(T) == typeof(Character))
            {
                var character = obj as Character;
                if (character != null && !characterData.ContainsKey(character.ID))
                {
                    characterData.Add(character.ID, character);
                    Palog.Log(character.id.ToString());
                }
            }
            else if (typeof(T) == typeof(Enemy))
            {
                var enemy = obj as Enemy;
                if (enemy != null && !enemyData.ContainsKey(enemy.ID))
                {
                    enemyData.Add(enemy.ID, enemy);
                }
            }
            else if (typeof(T) == typeof(Weapon))
            {
                var weapon = obj as Weapon;
                if (weapon != null && !weaponData.ContainsKey(weapon.ID))
                {
                    weaponData.Add(weapon.ID, weapon);
                }
            }
            else if (typeof(T) == typeof(ClassType))
            {
                var _class = obj as ClassType;
                if (_class != null && !classData.ContainsKey(_class.ID))
                {
                    classData.Add(_class.ID, _class);
                }
            }
            else if (typeof(T) == typeof(Skill))
            {
                var skill = obj as Skill;
                if (skill != null && !skillData.ContainsKey(skill.ID))
                {
                    skillData.Add(skill.ID, skill);
                }
            }
        }

        Palog.Log(folderPath + "���� �� " + objects.Length + "���� ��ü�� �ε�Ǿ����ϴ�.");
    }
}

[System.Serializable]
public class Player
{
    public int dataVersion;

    public double money;
    public double dia;

    public List<CharacterData> characters;
}
[System.Serializable]
public class CharacterData
{
    public int id;

    public Skill personalSkill = new Skill();
    public Skill[] setSkills = new Skill[3];
}


public interface IIdentifiable
{
    string ID { get; }
}

public class Type
{
    // Ƽ��
    public enum Tier
    {
        common,
        uncommon,
        rare
    }
    // �� ���� (���ݷ¿� ����)
    public enum MainStatus
    {
        Strength,
        Intelligence,
        Wisdom,
        Dexterity,
        Charisma,
        Constitution
    }
    // ��ų ����
    public enum SkillType
    {
        Active,
        Passive
    }
    // ���� �Ʊ� ����
    public enum Aliy
    {
        Player,
        Enemy
    }
}
