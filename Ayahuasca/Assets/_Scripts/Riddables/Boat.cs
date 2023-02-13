using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Behaviours.Interfaces;
using PlayerBehaviours;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class Boat : MonoBehaviour, IRiddable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform paddle;

    [Header("Seats and Camera")] [SerializeField]
    private Camera boatCamera;

    public Seat frontSeat = new Seat();
    public Seat backSeat = new Seat();

    [Header("Floating")] [SerializeField] private List<Transform> boatBoaters;
    [SerializeField] float checkDistance = 5f;
    [SerializeField] private float radiusCheck = 0.1f;
    [SerializeField] float pullForce = 0.2f;

    [InfoBox("Don't forget to put the ground and water, and put water tag on the water object",
        InfoMessageType.Warning)]
    [SerializeField]
    LayerMask detectLayers;

    [Header("Movement")] [SerializeField] float stabilizeSpeed = 2f;
    [SerializeField] private float angularStabilizeSpeed = 1f;
    private Vector3 _targetAngularVelocity = Vector3.zero;
    [SerializeField] Vector3 minimumVelocity = new Vector3(0f, 2f, 0f);
    [SerializeField] private float _angularVelocityLerp = 2f;
    [SerializeField] float velocityLerp = 2f;
    private int _boatersInTheWater;
    public int minimumBoatersInWater;
    public bool isFloating { get; private set; }

    private void OnDrawGizmos()
    {
        foreach (var boater in boatBoaters)
        {
            if (boater != null)
            {
                Gizmos.DrawLine(boater.position, boater.position - boater.up * checkDistance);
            }
        }
    }

    private void Awake()
    {
        boatCamera.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        SimulateBoat();
    }

    public void StartRiding(GameObject go, ref PlayerRiding ridingType, out RowingSide rowingSide)
    {
        ridingType = PlayerRiding.BOAT;
        SetSeated(go.transform, out rowingSide);
    }

    public bool StopRiding(GameObject go)
    {
        if (RemoveFromBoat(go.transform))
        {
            CheckSeats();
            return true;
        }

        return false;
    }

    public string GetRideText(GameObject InInteractor)
    {
        return "Ride boat";
    }

    public Vector3 GetRideLocation(GameObject InInteractor)
    {
        Seat seat = new Seat();
        if (GetSeatForPlayer(InInteractor, ref seat))
        {
            return seat.positionTransform.position;
        }
        return Vector3.one;
    }

    private void SetSeated(Transform InTransform, out RowingSide rowingSide)
    {
        rowingSide = RowingSide.NONE;
        Seat seat = new Seat();
        if (GetSeatForPlayer(InTransform.gameObject, ref seat))
        {
            if (InTransform.TryGetComponent(out PlayerCharacter playerCharacter))
            {
                if (seat.currentObject == null)
                {
                    rowingSide = seat.rowingSide;

                    seat.currentObject = playerCharacter.gameObject;
                    playerCharacter.transform.parent = seat.positionTransform;
                    playerCharacter.transform.position = seat.positionTransform.position;
                    playerCharacter.transform.rotation = seat.positionTransform.rotation;

                    var paddleTransform = Instantiate(paddle);
                    if(seat.rowingSide == RowingSide.LEFT) {
                        paddleTransform.parent = playerCharacter.RightHand;
                        paddleTransform.GetComponent<Paddle>().Target = playerCharacter.LeftHand;
                    } else {
                        paddleTransform.parent = playerCharacter.LeftHand;
                        paddleTransform.GetComponent<Paddle>().Target = playerCharacter.RightHand;
                    }

                    paddleTransform.localPosition = Vector3.zero;

                    playerCharacter.GetComponent<CharacterController>().enabled = false;
                }
                else
                {
                    Debug.LogError("Already occupied");
                    //RemoveFromBoat(backSeat);
                }
            }
            CheckSeats();
        }
    }

    private bool GetSeatForPlayer(GameObject InPlayer, ref Seat InSeat)
    {
        if (InPlayer.TryGetComponent(out PlayerCharacter playerCharacter))
        {
            int playerIndex = PlayersManager.Instance.GetPlayerIndex(playerCharacter.GetOwner());
            // Isto vai ficar materlado, mas depois se lembrar de uma forma melhor de obrigar um jogador a ficar neste
            // muda-se
            switch (playerIndex)
            {
                case 0:
                    InSeat = backSeat;
                    return true;
                    break;
                case 1:
                    InSeat = frontSeat;
                    return true;
                    break;
            }
        }

        InSeat = new Seat();
        return false;
    }

    private void CheckSeats()
    {
        List<Player> players = PlayersManager.Instance.ReturnPlayers();

        if (players.Count > 0 && players.Count(d => d.ReturnCharacter().GetRidingType() == PlayerRiding.NONE) == 0)
        {
            // They are all inside the boat
            if (SplitScreenManager.Instance != null)
            {
                SplitScreenManager.Instance.EnableSystem(false);
                boatCamera.gameObject.SetActive(true);
            }
        }
        else
        {
            if (SplitScreenManager.Instance != null)
            {
                if (!SplitScreenManager.Instance.GetIsSystemEnable())
                {
                    boatCamera.gameObject.SetActive(false);
                    SplitScreenManager.Instance.EnableSystem(true);
                }
            }
        }
    }

    public bool RemoveFromBoat(Transform player)
    {
        if (frontSeat.currentObject == player.gameObject)
        {
            return RemoveFromBoat(ref frontSeat);
        }
        else if (backSeat.currentObject == player.gameObject)
        {
            return RemoveFromBoat(ref backSeat);
        }

        return false;
    }

    public bool RemoveFromBoat(ref Seat seat)
    {
        if (seat.currentObject != null)
        {
            // Change player animation state
            seat.currentObject.transform.parent = null;
            seat.currentObject.transform.position = seat.exitLocation.position;
            seat.currentObject.transform.rotation = seat.exitLocation.rotation;
            var playerCharacter = seat.currentObject.GetComponent<PlayerCharacter>();
            if (playerCharacter != null)
            {
                if(playerCharacter.LeftHand.childCount > 0) {
                    Destroy(playerCharacter.LeftHand.GetChild(0).gameObject);
                }

                if(playerCharacter.RightHand.childCount > 0) {
                    Destroy(playerCharacter.RightHand.GetChild(0).gameObject);
                }
                
                if (playerCharacter.OnStopRiding())
                {
                    seat.currentObject.GetComponent<CharacterController>().enabled = true;
                    seat.currentObject.GetComponent<Collider>().enabled = true;
                    seat.currentObject = null;
                    return true;
                }
                else
                {
                    // Set the values again just in case the other fails, we restart the values
                    seat.currentObject.transform.parent = seat.positionTransform;
                    seat.currentObject.transform.position = seat.positionTransform.position;
                    seat.currentObject.transform.rotation = seat.positionTransform.rotation;
                }
            }
        }

        return false;
    }

    public RowingSide Row(GameObject playerGo, float rowSideForce, float rowFowardForce)
    {
        if (isFloating)
        {
            if (frontSeat.currentObject == playerGo)
            {
                return AddForceToBoat(ref frontSeat, rowSideForce, rowFowardForce);
            }
            else if (backSeat.currentObject == playerGo)
            {
                return AddForceToBoat(ref backSeat, rowSideForce, rowFowardForce);
            }
        }

        return RowingSide.NONE;
    }

    RowingSide AddForceToBoat(ref Seat seat, float rotationForce, float fowardForce)
    {
        switch (seat.rowingSide)
        {
            case RowingSide.RIGHT:
                if (rb != null)
                {
                    _targetAngularVelocity += new Vector3(0, -rotationForce, 0);
                }

                break;

            case RowingSide.LEFT:
                if (rb != null)
                {
                    _targetAngularVelocity += new Vector3(0, rotationForce, 0);
                }

                break;
        }

        rb.AddForce(transform.forward * fowardForce, ForceMode.VelocityChange);

        return seat.rowingSide;
    }

    void SimulateBoat()
    {
        if (rb != null)
        {
            _boatersInTheWater = 0;
            // Set the floaters
            foreach (var boater in boatBoaters)
            {
                RaycastHit hit;
                float heightDifference = 0;
                //if (Physics.SphereCast(boater.position, radiusCheck, boater.position+ (-boater.up * checkDistance), out hit, checkDistance, detectLayers))
                if (Physics.Raycast(boater.position, -boater.up * checkDistance, out hit, checkDistance, detectLayers))
                {
                    if (hit.transform.CompareTag("Water"))
                    {
                        heightDifference = 1 - ((boater.position - hit.point).magnitude / checkDistance);
                        _boatersInTheWater++;
                    }
                }
                else
                {
                    heightDifference = 0;
                }

                rb.AddForceAtPosition(boater.up * heightDifference * pullForce * Time.deltaTime, boater.position,
                    ForceMode.VelocityChange);
            }

            isFloating = _boatersInTheWater >= minimumBoatersInWater;
            if (isFloating)
            {
                // Stabilize the boat on the other axis
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(0, transform.eulerAngles.y, 0), Time.deltaTime * stabilizeSpeed);
                // Decrements the target angular velocity
                _targetAngularVelocity = Vector3.Lerp(_targetAngularVelocity, Vector3.zero,
                    Time.deltaTime * _angularVelocityLerp);
                // Lerps the angular velocity of the rigidbody to the target velocity, to be extra smooth
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, _targetAngularVelocity,
                    Time.deltaTime * angularStabilizeSpeed);

                // Remove uncessary movement
                rb.velocity = Vector3.Lerp(rb.velocity, minimumVelocity, Time.deltaTime * velocityLerp);
            }
        }
    }

    public void Push(float pushForce, Vector3 originPosition)
    {
        originPosition.y = transform.position.y;
        rb.AddForce((transform.position-originPosition).normalized * pushForce);
        //rigidbody.AddExplosionForce(pushForce, originPosition,radius);
    }
}

[Serializable]
public class Seat
{
    public Transform positionTransform;
    public RowingSide rowingSide;
    public Transform rowingForceTransform;
    public Transform exitLocation;
    [ReadOnly] public GameObject currentObject;
}

public enum RowingSide
{
    NONE,
    LEFT,
    RIGHT
}