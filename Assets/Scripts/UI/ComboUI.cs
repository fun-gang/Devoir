using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    private float speed = 0.9f;
    public Transform cursor;
    public Transform goalObj;
    public Combo master;
    public GameObject visibleUI;
    private bool isPressed = false;
    private bool isStopped = false;
    public Positions poss;
    public Positions goalPos;
    public int maxCount = 3;
    private int currCount = 0;

    void Start() {
        speed -= (stats.ComboCursorSpeedMod / 100) * speed;
    }

    public void Show() {
        visibleUI.SetActive(true);
        Time.timeScale = 0.45f;
        Movement.control = false;
        cursor.position = poss.start.position;
        SpawnGoal(true);
    }

    private void Hide() {
        Movement.control = true;
        Time.timeScale = 1f;
        master.isReadyState = 2;
        master.OnReloadCombo();
        visibleUI.SetActive(false);
        currCount = 0;
    }

    public void Click() {
        if (isPressed) {
            isStopped = true;
        }
        else {
            isPressed = true;
            cursor.position = poss.start.position;
            SpawnGoal(false);
            StartCoroutine(Move());
        }
    }

    private void NextStart() {
        isStopped = false;
        isPressed = false;
        currCount += 1;
        if (currCount >= maxCount) Hide();
    }

    IEnumerator Move() {
        while (true) {
            cursor.position += new Vector3(speed * Time.unscaledDeltaTime,0,0);
            yield return null;
            if (isStopped || cursor.position.x >= poss.end.position.x) {
                if (cursor.position.x > poss.end.position.x) cursor.position = new Vector3(poss.end.position.x,cursor.position.y,0);
                NextStart();
                break;
            }
        }
    }

    private void SpawnGoal(bool isPrime) {
        if (currCount == 0 && !isPrime) return;
        float randPos = Random.Range(goalPos.start.position.x, goalPos.end.position.x);
        goalObj.position = new Vector3(randPos, goalObj.position.y, goalObj.position.z);
    }
}

[System.Serializable]
public class Positions {
    public Transform start;
    public Transform end;
}