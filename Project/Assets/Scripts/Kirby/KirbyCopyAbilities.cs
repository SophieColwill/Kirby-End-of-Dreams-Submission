using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class KirbyCopyAbilities : MonoBehaviour
{
    [Header("Script Refrences")]
    [SerializeField] KirbyMovment movementScript;
    [SerializeField] SuckArea suckArea_Right;
    [SerializeField] SuckArea suckArea_Left;
    [SerializeField] KirbyAnimator animator;

    [Header("Prefab Refrences")]
    [SerializeField] DroppedCopyAbility CopyStarRefrence;

    [Header("Copy Abilities")]
    public CopyAbilitiy CurrentCopyAbility;
    public List<InhaledObject> CurrentInhaledObjects;
    [SerializeField] int RandomDropAbilityOnHurtChance = 4;

    [Header("Base")]
    [SerializeField] Rigidbody2D ExhaleCloud;
    [SerializeField] float PuffCloudForce = 5;
    [SerializeField] Rigidbody2D SpitOutStar;
    [SerializeField] float SpitOutForce = 5;

    [Header("Bomb")]
    [SerializeField] Rigidbody2D BombPrefab;
    [SerializeField] Vector2 BombThrowForces;
    float BombTimer;

    [Header("Beam")]
    [SerializeField] KirbyBeam BeamAttackPrefab_Right;
    [SerializeField] KirbyBeam BeamAttackPrefab_Left;
    [SerializeField] float BeamAttackTime;

    [Header("Cutter")]
    [SerializeField] KirbyCutter CutterBladePrefab;
    [SerializeField] float RefreshCutterTime;
    [SerializeField] float CutterThrowPower;
    [SerializeField] float CutterReturnPower;
    float CutterTimer;

    bool isSucking;
    int LookDirectionValue;
    List<Collider2D> CurrentObjectsToSuck = new List<Collider2D>();

    #region Getters

    public bool HasKirbyCurrentlyInhaledSomething()
    {
        return (CurrentInhaledObjects.Count != 0);
    }

    public bool GetIsInhaling()
    {
        return isSucking;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Initialise_Copy(this);
    }

    // Update is called once per frame
    void Update()
    {

        if (movementScript.getIsLookingRight)
        {
            LookDirectionValue = 1;
        }
        else
        {
            LookDirectionValue = -1;
        }

        if (isSucking)
        {
            foreach (Collider2D OBJ in CurrentObjectsToSuck)
            {
                EnemyBehavior Target = OBJ.GetComponent<EnemyBehavior>();
                if (Target != null)
                {
                    Target.KirbyInhale(LookDirectionValue);
                }
            }
        }
        #region Timers

        if (CutterTimer > 0)
        {
            CutterTimer -= Time.deltaTime;
        }

        if (BombTimer > 0)
        {
            BombTimer -= Time.deltaTime;
        }

        #endregion
    }

    #region Primary and Suck
    public void PrimaryAction()
    {
        if (!movementScript.GetIsFloating)
        {
            if (CurrentCopyAbility == CopyAbilitiy.Bomb)
            {
                if (BombTimer <= 0)
                {
                    Rigidbody2D BombOBJ = Instantiate<Rigidbody2D>(BombPrefab, transform.position, BombPrefab.transform.rotation);
                    BombOBJ.AddForce(new Vector2(LookDirectionValue * BombThrowForces.x, BombThrowForces.y), ForceMode2D.Impulse);
                    animator.animator.SetTrigger("attack");
                    BombTimer = 1;
                }
            }
            else if (CurrentCopyAbility == CopyAbilitiy.Cutter)
            {
                if (CutterTimer <= 0)
                {
                    Debug.Log("Performed Action");
                    Vector3 spawnPosition = new Vector3(transform.position.x + (LookDirectionValue * 2), transform.position.y, transform.position.z);
                    KirbyCutter SummonedCutter = Instantiate<KirbyCutter>(CutterBladePrefab, spawnPosition, CutterBladePrefab.transform.rotation);
                    SummonedCutter.StartThrow(movementScript.getIsLookingRight, CutterThrowPower, CutterReturnPower);
                    CutterTimer = RefreshCutterTime;
                }
                else
                {
                    Debug.Log("Cutter is still on cooldown for " + Mathf.Round(CutterTimer).ToString() + " seconds");
                }
            }
            else if (CurrentCopyAbility == CopyAbilitiy.Beam)
            {
                if (movementScript.DisableXAxisMovmentTimer <= 0)
                {
                    Debug.Log("Performed the beam attack");
                    movementScript.DisableXAxisMovmentTimer = BeamAttackTime;
                    animator.animator.SetTrigger("attack");

                    if (movementScript.getIsLookingRight)
                    {
                        Vector3 spawnPosition = new Vector3(transform.position.x + (LookDirectionValue * 4.33f), transform.position.y, transform.position.z);
                        KirbyBeam BeamOBJ = Instantiate<KirbyBeam>(BeamAttackPrefab_Right, spawnPosition, BeamAttackPrefab_Right.transform.rotation);

                        Destroy(BeamOBJ.gameObject, BeamAttackTime);
                    }
                    else
                    {
                        Vector3 spawnPosition = new Vector3(transform.position.x + (LookDirectionValue * 4.33f), transform.position.y, transform.position.z);
                        KirbyBeam BeamOBJ = Instantiate<KirbyBeam>(BeamAttackPrefab_Left, spawnPosition, BeamAttackPrefab_Left.transform.rotation);

                        Destroy(BeamOBJ.gameObject, BeamAttackTime);
                    }
                }
            }
            else
            {
                if (!HasKirbyCurrentlyInhaledSomething())
                {
                    if (movementScript.getIsLookingRight)
                    {
                        CurrentObjectsToSuck = suckArea_Right.GetObjectInRange;
                    }
                    else
                    {
                        CurrentObjectsToSuck = suckArea_Left.GetObjectInRange;
                    }

                    isSucking = true;
                }
                else
                {
                    movementScript.GetIsFloating = false;
                    Rigidbody2D star = Instantiate(SpitOutStar.gameObject, transform.position, SpitOutStar.transform.rotation).GetComponent<Rigidbody2D>();

                    //Replace THe Rigidbody Force with animations for a better looking puff cloud.
                    if (movementScript.getIsLookingRight)
                    {
                        star.AddForce(Vector2.right * SpitOutForce);
                    }
                    else
                    {
                        star.AddForce(-Vector2.right * SpitOutForce);
                        star.GetComponent<SpriteRenderer>().flipX = true;
                    }
                    CurrentInhaledObjects.Clear();
                }
            }
        }
        else
        {
            movementScript.DisableFloating();
            Rigidbody2D puffCloud = Instantiate(ExhaleCloud.gameObject, transform.position, ExhaleCloud.transform.rotation).GetComponent<Rigidbody2D>();
            Debug.Log("Spit Out Cloud");

            //Replace THe Rigidbody Force with animations for a better looking puff cloud.
            if (movementScript.getIsLookingRight)
            {
                puffCloud.AddForce(Vector2.right * PuffCloudForce);
            }
            else
            {
                puffCloud.AddForce(-Vector2.right * PuffCloudForce);
            }

            Destroy(puffCloud.gameObject, 0.5f);
        }
    }

    public void TryEndSuck()
    {
        if (isSucking)
        {
            isSucking = false;

            foreach (Collider2D OBJ in CurrentObjectsToSuck)
            {
                EnemyBehavior Target = OBJ.GetComponent<EnemyBehavior>();
                if (Target != null)
                {
                    Target.KirbyStopInhale();
                }
            }
        }
    }

    public void AddInhaledObject(CopyAbilitiy AddedAbility, float ObjectSize)
    {
        CurrentInhaledObjects.Add(new InhaledObject() { ObjectCopyAbility = AddedAbility, ObjectSize = ObjectSize });
    }

    public void Swallow()
    {
        if (movementScript.isGrounded)
        {
            if (CurrentInhaledObjects.Count != 0)
            {
                foreach (InhaledObject obj in CurrentInhaledObjects)
                {
                    if (obj.ObjectCopyAbility != CopyAbilitiy.None)
                    {
                        CurrentCopyAbility = obj.ObjectCopyAbility;
                        break;
                    }
                }
                movementScript.DisableXAxisMovmentTimer = 0.5f;
                CurrentInhaledObjects.Clear();
            }
        }
    }
    #endregion

    #region Secondary
    public void SecondaryAction()
    {

    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
    }

    public void DropCopyAbility()
    {
        if (CurrentCopyAbility != CopyAbilitiy.None)
        {
            DroppedCopyAbility starRef = Instantiate<DroppedCopyAbility>(CopyStarRefrence, transform.position + (Vector3.up * 1.5f), CopyStarRefrence.transform.rotation);
            starRef.ThisCopyAbilotyToGive = CurrentCopyAbility;
            CurrentCopyAbility = CopyAbilitiy.None;
        }
    }
}
[System.Serializable]
public class InhaledObject
{
    public CopyAbilitiy ObjectCopyAbility;
    public float ObjectSize = 1;
}