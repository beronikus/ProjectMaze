using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

   [SerializeField] private List<GameObject> levelPrefabs;
   private GameObject currentLevelInstance;
   [SerializeField] private Transform playerStartPosition;
   private int currentLevelIndex = 0;





   private void Awake()
   {
       Instance = this;
   }
   
   private void Start()
   {
       LoadLevelZero();
   }

   private void LoadLevelZero()
   {
       currentLevelInstance = Instantiate(levelPrefabs[currentLevelIndex]);
   }

   public void LoadNextLevel()
   {
       if (currentLevelIndex < levelPrefabs.Count)
       {
           currentLevelIndex++;
           Destroy(currentLevelInstance);
           currentLevelInstance = Instantiate(levelPrefabs[currentLevelIndex]);
           Player.Instance.gameObject.transform.position = playerStartPosition.position;
           Player.Instance.KillMobility();
       }
       else
       {
           LoadLevelZero();
       }
   }
   
   
   
}
