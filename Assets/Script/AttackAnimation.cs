using UnityEngine;
using System.Collections;

public class AttackSystem : MonoBehaviour
{
    private enum AttackState { Idle, SolarBeam, ShadowBall }
    private AttackState currentState = AttackState.Idle;

    [Header("Components")]
    public Animator animator;
    public Transform handTransform;

    [Header("Settings")]
    public float autoIdleTime = 5.0f; // Global cooldown to return to idle

    private Coroutine attackRoutine; // Tracks the currently running attack logic

    // --- VOICE SDK LINK ---
    public void ReceiveVoiceCommand(string command)
    {
        string cmd = command.ToLower().Trim();

        if (cmd == "solar beam") SetState(AttackState.SolarBeam);
        else if (cmd == "shadow ball") SetState(AttackState.ShadowBall);
    }

    private void SetState(AttackState newState)
    {
        if (currentState == newState) return;

        // Stop any currently running attack logic before starting a new one
        if (attackRoutine != null) StopCoroutine(attackRoutine);

        currentState = newState;

        // Route to the specific handler
        switch (newState)
        {
            case AttackState.SolarBeam:
                attackRoutine = StartCoroutine(HandleSolarBeam());
                break;
            case AttackState.ShadowBall:
                attackRoutine = StartCoroutine(HandleShadowBall());
                break;
            case AttackState.Idle:
                ResetToIdle();
                break;
        }
    }

    // --- SEPARATE ATTACK HANDLERS ---

    private IEnumerator HandleSolarBeam()
    {
        Debug.Log("State: Solar Beam Started");
        animator.SetTrigger("StartSolarBeam");

        // 1. Initial Setup (Spawn Orb, etc.)
        // Instantiate(solarOrbPrefab...);

        // 2. Wait for 5 seconds or perform attack logic
        // You can put your charging/throwing logic here
        yield return new WaitForSeconds(autoIdleTime);

        // 3. Auto-return to Idle
        Debug.Log("Solar Beam timed out. Returning to Idle.");
        SetState(AttackState.Idle);
    }

    private IEnumerator HandleShadowBall()
    {
        Debug.Log("State: Shadow Ball Started");
        animator.SetTrigger("StartShadowBall");

        // Shadow ball specific logic (e.g., darker particles, different hand vibration)

        yield return new WaitForSeconds(autoIdleTime);

        Debug.Log("Shadow Ball timed out. Returning to Idle.");
        SetState(AttackState.Idle);
    }

    private void ResetToIdle()
    {
        currentState = AttackState.Idle;
        animator.SetTrigger("ReturnToIdle");
        // Clear any leftover VFX or objects here
        Debug.Log("System Status: Idle");
    }
}