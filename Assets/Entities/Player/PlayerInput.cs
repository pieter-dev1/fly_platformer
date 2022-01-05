using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float enableStartUpTime; //original = 7
    [SerializeField]
    private Controls controls;
    private EntityComponents comps;
    private int enableLookButtonsPressed = 0; //when enableLook and axisLook are both held, releasing one will disable looking around, while it should still be allowed. So if both are held, releasing one shouldnt disable.

    public bool pressedJump = false;

    private int trollIndex = -1;
    

    //Sprint/wallrun effects
    private readonly EffectExecution sprintEffect = new EffectExecution(Effect.MOVESPEED, 30);

    // Start is called before the first frame update
    void Start()
    {
        comps = GetComponent<EntityComponents>();
        comps.entityStats.meter.comps = comps;
        comps.entityStats.meter.undoEffects = new EffectExecution[] { sprintEffect };
        comps.entityStats.meter.triggerCallInfo = (this, "TriggerSprint", new object[0]);
        comps.entityStats.meter.triggerConditionsInfo = new List<(Component script, string variable, object value)>()
        {
            (comps.entityMovement, "moving", true),
            (comps.entityJump, "jumped", false),
        };
        comps.entityStats.meter.Start();

        GameObject.Find("PlayerVcam").GetComponent<CinemachineInputActionProvider>().XYAxis = controls.look;

        controls.move.started += _ => {
            comps.entityMovement.moving = true;
            comps.audioManager.PlaySound("FlyWalkStrijk");
            //if (comps.entityStats.grounded) { comps.audioManager.PlaySound("PlayerWalk"); } 
        };
        controls.move.performed += ctx => comps.entityMovement.direction = ctx.ReadValue<Vector2>();
        controls.move.canceled += _ => { comps.entityMovement.CancelMovement(); comps.audioManager.StopSound("FlyWalkStrijk"); };

        //pressedJump tracks if the jump input has succesfully come through. With this it can be forced that the jumpcancel input only comes through on the actual jump
        //(the first time you press the button). This means every press (jumpcancel) after (whem youre in the air) will not come through, which prevents a bug
        //where you would stop falling a brief moment even though you were already falling from your jump.
        controls.jump.performed += _ => {
            if (comps.entityStats.grounded || comps.fauxAttractor.onWall) {
                comps.entityStats.lastSurface = (comps.fauxAttractor.currentSurface.transform, comps.entityStats.groundUp);
                comps.entityJump.Jump(); pressedJump = true;
            }
        };
        //if it's certain the player has jumped, doesn't touch the ground and isn't going down anyways, the jump can be cancelled.
        controls.jump.canceled += _ => { if (comps.entityJump.jumped && pressedJump && !comps.entityStats.grounded
            //Last condition makes sure the player is still jumping up, because when falling cancelling the jump has no use and is buggy.
            && ((comps.entityStats.upAxis.positive && comps.rigidbody.velocity[comps.entityStats.upAxis.index] > 0) 
            || (!comps.entityStats.upAxis.positive && comps.rigidbody.velocity[comps.entityStats.upAxis.index] < 0))) {
                CancelJump();
            }
        };

        //Sprint/wallrun
        controls.sprint.started += _ => TriggerSprint();
        controls.sprint.canceled += _ => CancelSprint();

        //To next dialogue line
        controls.nextDialogueLine.started += _ =>
        {
            var dialogue = DialogueDetector.currDialogue;
            if (dialogue != null)
            {
                if (!dialogue.currLineWritten)
                {
                    dialogue.FillLine();
                }
                else if (dialogue.linesQ.Any())
                {
                    StartCoroutine(dialogue.ShowNextLine());
                }
                else
                {
                    dialogue.DisableDialogue();
                }
            }
        };

        //Skip to next checkpoint
        controls.skip.started += _ =>
        {
            if (KnockoutZone.skipAvailable)
            {
                SkipToCheckpoint();
            }
        };

        //To checkpoint
        controls.toCheckpoint.started += _ => {
            transform.position = Challenge.respawnPoint;
            var meter = comps.entityStats.meter;
            meter.FillMeter(meter.maxMeter);
        };

        //Pause
        controls.pause.started += _ =>
        {
            if (Time.timeScale > 0)
                comps.playerPause.Pause();
            else
                comps.playerPause.Unpause();
            return;
        };

        //To next safepoint (dev)
        controls.toNextPoint.started += _ =>
        {
            GetComponent<PlayerToNextPoint>().ToPoint(true);
        };
        //To previous safepoint (dev)
        controls.toPrevPoint.started += _ =>
        {
            GetComponent<PlayerToNextPoint>().ToPoint(false);
        };
    }

    public void TriggerSprint()
    {
        if ((comps.entityStats.grounded || (!comps.entityStats.grounded && comps.entityJump.jumped)) && comps.entityStats.meter.currMeter >= comps.entityStats.meter.usageMinimum)
        {
            if (!comps.entityJump.jumped)
                comps.audioManager.PlaySound("WoundedBuzz");

            gameObject.ExecuteEffects(gameObject, false, sprintEffect);
            comps.fauxAttractor.enabled = true;
            comps.entityStats.meter.currUsing = true;
            if (comps.entityJump.jumped)
            {
                comps.fauxAttractor.onWall = comps.entityStats.lastSurface.surface.tag.Equals(Tags.WALL);
                comps.fauxAttractor.currentSurface = (comps.entityStats.lastSurface.surface, comps.entityStats.lastSurface.surface.GetComponent<BoxCollider>() != null);
                comps.entityStats.groundUp = comps.entityStats.lastSurface.groundUp;
            }

            comps.animator.SetBool("sprinting", true);
            comps.audioManager.StopSound("FlyWalkStrijk");
            comps.audioManager.PlaySound("FlySprint");
        }
    }

    public void CancelSprint()
    {
        comps.entityStats.blocks.Remove(Blocks.MOVE);
        comps.entityStats.meter.currUsing = false;
        if (comps.fauxAttractor.enabled && comps.entityStats.meter.allowUsage && !comps.entityStats.meter.resetThisUsage)
        {
            gameObject.ExecuteEffects(gameObject, true, sprintEffect);
            comps.fauxAttractor.CancelCustomGravity();
        };
        comps.entityStats.meter.resetThisUsage = false;
        comps.animator.SetBool("sprinting", false);
        StopSprintSound();
    }

    public void StopSprintSound()
    {
        comps.audioManager.StopSound("FlySprint");
        if(comps.entityMovement.moving)
            comps.audioManager.PlaySound("FlyWalkStrijk");
    }

    public void CancelJump()
    {
        comps.entityJump.CancelJump();
        pressedJump = false;
    }

    private void ReleaseLookButton()
    {
        if (enableLookButtonsPressed <= 1)
        {
            controls.look.Disable();
            enableLookButtonsPressed = 0;
        }
        else enableLookButtonsPressed--;
    }

    public void OnEnable()
    {
        StartCoroutine(EnableInput());
    }

    private IEnumerator EnableInput()
    {
        yield return new WaitForSeconds(enableStartUpTime);
        controls.move.Enable();
        controls.enableLook.Enable();
        controls.axisLook.Enable();
        controls.look.Disable();
        controls.jump.Enable();
        controls.sprint.Enable();
        controls.nextDialogueLine.Enable();
        controls.pause.Enable();
        controls.skip.Enable();
        controls.toCheckpoint.Enable();
        controls.toNextPoint.Enable();
        controls.toPrevPoint.Enable();
    }

    public void OnDisable()
    {
        controls.move.Disable();
        controls.enableLook.Disable();
        controls.axisLook.Disable();
        controls.look.Disable();
        controls.jump.Disable();
        controls.sprint.Disable();
        controls.nextDialogueLine.Disable();
        controls.pause.Disable();
        controls.skip.Disable();
        controls.toCheckpoint.Disable();
        controls.toNextPoint.Disable();
        controls.toPrevPoint.Disable();
    }

    private void SkipToCheckpoint()
    {
        var checkpoint = Challenge.checkpoints[Challenge.progress + 1];
        transform.position = checkpoint.position;
        KnockoutZone.timesKod = 0;
        GameObject.Find("SkipIcon").SetActive(false);
        KnockoutZone.skipAvailable = false;
    }

    public void ToLastCheckpoint()
    {
        comps.fauxAttractor.CompletelyCancelWallRun();
        comps.entityStats.meter.allowUsage = true;
        transform.position = Challenge.respawnPoint;
        var meter = comps.entityStats.meter;
        meter.FillMeter(meter.maxMeter);

        foreach (var dialogueTriggerCol in Challenge.checkpointDialogueTriggers)
        {
            dialogueTriggerCol.enabled = true;
        }
    }

    public void ToCheckpoint(int checkpointIndex)
    {
        comps.fauxAttractor.CompletelyCancelWallRun();
        comps.entityStats.meter.allowUsage = true;
        transform.position = Challenge.checkpoints[checkpointIndex].transform.position;
        var meter = comps.entityStats.meter;
        meter.FillMeter(meter.maxMeter);

        if (checkpointIndex > Challenge.progress)
        {
            KnockoutZone.timesKod = 0;
            GameObject.Find("SkipIcon").SetActive(false);
            KnockoutZone.skipAvailable = false;
        }

        //if (checkpointIndex >= Challenge.progress)
        //{
        //    foreach (var dialogueTriggerCol in Challenge.checkpointDialogueTriggers)
        //    {
        //        dialogueTriggerCol.enabled = true;
        //    }
        //}
    }

}
