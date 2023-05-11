// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TestMovement : MonoBehaviour
// {
//      // Velocidad de movimiento
//     public float speed = 5.0f;

//     // Referencia al controlador de Cardboard
//     private CardboardController cardboardController;

//     void Start () {
//         // Obtén la referencia al controlador de Cardboard
//         cardboardController = Cardboard.SDK.Controller;
//     }
    
//     void Update () {
//         // Detecta la entrada del joystick
//         float[] joyStickValues = cardboardController.State.GetButtonState(CardboardButton.GearVRTouchpad);

//         // Calcula la dirección del movimiento
//         Vector3 direction = new Vector3(joyStickValues[0], 0, joyStickValues[1]);

//         // Mueve el objeto
//         transform.Translate(direction * Time.deltaTime * speed);
//     }
// }
