using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int health;
    public float moveSpeed;
    public float minFireRate;
    public float maxFireRate;
    public int scoreValue;

    public GameObject laserPrefab;
}
