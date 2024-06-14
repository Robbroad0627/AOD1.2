using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bonehead Games

namespace AOD
{
    public class PickupItem : MonoBehaviour
    {

        private bool canPickup;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (canPickup && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove)
            {
                GameManager.instance.AddItem(GetComponent<Item>().GetItemData.GetName);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                canPickup = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                canPickup = false;
            }
        }
    }
}