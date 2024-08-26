using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public Animator animator;

    public int RollDice()
    {
        int randomNumber = Random.Range(1, 7);

        return randomNumber;
    }

    public IEnumerator WaitForDiceToStop(int delay, int number)
    {

        AnimateDice(number);

        yield return new WaitForSeconds(delay);
    }

    private void AnimateDice(int targerNumber)
    {
        

        switch (targerNumber)
        {
            case 1:
                animator.ResetTrigger("One");
                animator.SetTrigger("One");
                break;
            case 2:
                animator.ResetTrigger("Two");
                animator.SetTrigger("Two");
                break;
            case 3:
                animator.ResetTrigger("Three");
                animator.SetTrigger("Three");
                break;
            case 4:
                animator.ResetTrigger("Four");
                animator.SetTrigger("Four");
                break;
            case 5:
                animator.ResetTrigger("Five");
                animator.SetTrigger("Five");
                break;
            case 6:
                animator.ResetTrigger("Six");
                animator.SetTrigger("Six");
                break;
        }
    }

    public void ReturnToIdle()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
