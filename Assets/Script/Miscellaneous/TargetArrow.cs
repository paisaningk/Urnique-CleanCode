using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrow : MonoBehaviour
{
   [SerializeField]private Transform Target;
   [SerializeField]private float HideDistance;
   
   void Update()
   {
      var dir = Target.position - transform.position;

      var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

      this.gameObject.SetActive(!(dir.magnitude < HideDistance));
   }
}
