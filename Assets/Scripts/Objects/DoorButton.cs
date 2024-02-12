using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorButton : MonoBehaviour
{
    public Sprite pressedSprite;
    public Sprite releasedSprite;

    public UnityEvent onPress;
    public UnityEvent onRelease;

    public SpriteRenderer indicator;
    private int counter;

    void Awake() {
        indicator.sprite = releasedSprite;
        counter = 0;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.tag == "Player") {
            if (counter == 0) {
                gameObject.GetComponent<AnimatorBooleanTransition>().SetState(true);
                indicator.sprite = pressedSprite;
                onPress.Invoke();
            }
            counter++;
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.tag == "Player") {
            counter--;
            if (counter == 0) {
                gameObject.GetComponent<AnimatorBooleanTransition>().SetState(false);
                indicator.sprite = releasedSprite;
                onRelease.Invoke();
            }
        }
    }
}