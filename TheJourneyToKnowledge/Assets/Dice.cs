using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    public Rigidbody diceRigidbody;

    public int RollDice()
    {
        diceRigidbody.velocity = Vector3.zero;
        diceRigidbody.angularVelocity = Vector3.zero;

        diceRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
        diceRigidbody.AddTorque(Random.Range(200,500),Random.Range(200,500),Random.Range(200,500));

        int randomNumber = Random.Range(1, 6);

        StartCoroutine(WaitForDiceToStop(2, randomNumber));

        return randomNumber;
    }

    private IEnumerator WaitForDiceToStop(int delay, int number)
    {
        yield return new WaitForSeconds(delay);

        while (diceRigidbody.velocity.magnitude > 0.1f || diceRigidbody.angularVelocity.magnitude > 0.1f)
        {
            yield return null;
        }

        SnapDiceToNumber(number);
    }

    private void SnapDiceToNumber(int targerNumber)
    {
        Transform diceTransform = diceRigidbody.transform;

        switch (targerNumber)
        {
            case 1:
                diceTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 2:
                diceTransform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 3:
                diceTransform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case 4:
                diceTransform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 5:
                diceTransform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case 6:
                diceTransform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
        }
    }
}
