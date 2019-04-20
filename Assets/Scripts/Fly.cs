using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Agent
{
    [Header("Properties")]
    public int damage = 10;
    public float attackRate = 1f;
    public float detectRadius = 2f;
    public LineRenderer shotLine;
    public float lineDelay = .1f;

    private float shootTimer = 0f;
    private Transform start, end;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    protected override void Update()
    {
        base.Update();

        shootTimer += Time.deltaTime;
        float fraction = 1.0f / attackRate;
        if (shootTimer >= fraction)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius);
            foreach (var hit in hits)
            {
                Moth moth = hit.GetComponent<Moth>();
                if (moth)
                {
                    // Attack moth!
                    start = transform;
                    end = moth.transform;

                    StartCoroutine(ShowLine(lineDelay));

                    moth.TakeDamage(damage);
                }
            }
            shootTimer = 0f;
        }
    }

    void LateUpdate()
    {
        if (start && end)
        {
            shotLine.SetPosition(0, start.position);
            shotLine.SetPosition(1, end.position);
        }
    }

    IEnumerator ShowLine(float lineDelay)
    {
        shotLine.enabled = true;

        yield return new WaitForSeconds(lineDelay);

        shotLine.enabled = false;
    }

}
