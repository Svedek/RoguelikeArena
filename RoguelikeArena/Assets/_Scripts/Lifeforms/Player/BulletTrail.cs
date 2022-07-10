using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour {

    private LineRenderer lr;

    private void Awake() {
        lr = GetComponent<LineRenderer>();
    }
    public void MakeTrail(Vector3 start, Vector3 end) {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.enabled = true;
        StartCoroutine(DisableVisibility());
    }

    private IEnumerator DisableVisibility() {
        yield return new WaitForSeconds(0.02f);
        lr.enabled = false;
    }
}
