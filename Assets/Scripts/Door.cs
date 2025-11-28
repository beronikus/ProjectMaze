using System;
using UnityEngine;

public class Door : MonoBehaviour
{
   private void Start()
   {
      Player.Instance.OnEnemyKilled += OnEnemyKilled;
   }

   private void OnEnemyKilled(object sender, EventArgs e)
   {
      if (this != null && gameObject != null) 
      {
         gameObject.SetActive(false);
      }
   }
}
