using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    private float speed = 2f;
    public Transform cursor;
    public Transform goalObj;
    public Combo master;
    public GameObject visibleUI;
    private bool isStarted = false;
    private bool isStopped = false;
    public ComboPositions poss;
    public ComboPositions goalPos;
    public int maxCount = 3;
    private int currCount = 0;
    private List<ComboResults> results = new List<ComboResults>();

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
        Time.timeScale = 1f;
        master.isReadyState = 2;
        master.OnReloadCombo(CalculateResults());
        visibleUI.SetActive(false);
        currCount = 0;
        isStarted = false;
    }

    public void Click() {
        if (isStarted) {
            isStopped = true;
        }
        else {
            isStarted = true;
            NextStart();
        }
    }

    private void NextStart() {
        currCount += 1;
        isStopped = false;
        if (currCount > maxCount) {
            Hide();
            return;
        }
        StartCoroutine(Move());
        SpawnGoal(false);
    }

    IEnumerator Move() {
        int dir = currCount % 2 == 0 ? -1 : 1;
        while (true) {
            cursor.position += new Vector3(speed * Time.unscaledDeltaTime * dir, 0, 0);

            if (cursor.position.x > poss.end.position.x) {
                cursor.position = new Vector3(poss.end.position.x,   cursor.position.y,0);
                isStopped = true;
            }
            if (cursor.position.x < poss.start.position.x) {
                cursor.position = new Vector3(poss.start.position.x, cursor.position.y,0);
                isStopped = true;
            }

            if (isStopped) {
                yield return new WaitForSeconds(0.05f);

                ComboResults newRes = new ComboResults();
                newRes.cursorPos = cursor.position.x;
                newRes.goalPos = goalObj.position.x;
                results.Add(newRes);

                float newPoint = dir < 0 ? poss.start.position.x : poss.end.position.x;
                cursor.position = new Vector3(newPoint, cursor.position.y, 0);

                NextStart();
                break;
            }
            yield return null;
        }
    }

    private void SpawnGoal(bool isPrime) {
        if (currCount == 0 && !isPrime) return;
        float randPos = Random.Range(goalPos.start.position.x, goalPos.end.position.x);
        goalObj.position = new Vector3(randPos, goalObj.position.y, goalObj.position.z);
    }

    private List<float> CalculateResults() {
        List<float> res = new List<float>();

        foreach (ComboResults dat in results) {
            float total = Mathf.Abs(poss.end.position.x - poss.start.position.x);
            float rate = dat.goalPos >= dat.cursorPos ? Mathf.Abs(dat.goalPos - dat.cursorPos) : Mathf.Abs(dat.cursorPos - dat.goalPos);

            res.Add(rate / total);
        }
        results = new List<ComboResults>();
        return res;
    }
}

[System.Serializable]
public class ComboPositions {
    public Transform start;
    public Transform end;
}

[System.Serializable]
public class ComboResults {
    public float cursorPos;
    public float goalPos;
}