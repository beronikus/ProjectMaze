using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Door : MonoBehaviour
{
   
   private void Start()
   {
      Player.Instance.OnEnemyKilled += OnEnemyKilled;
      Player.Instance.OnEnemySecondKilled += OnEnemySecondKilled;
   }
   

   private void OnEnemyKilled(object sender, EventArgs e)
   {
         if (this != null && gameObject != null)
         {
            gameObject.SetActive(false);
         }
      
   }

   private void OnEnemySecondKilled(object sender, EventArgs e)
   {
      if (this != null && gameObject != null)
      {
         gameObject.SetActive(false);
      }
   }
}
