using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RadarControls : MonoBehaviour
{
    public GameManager.RadarID radarID = GameManager.RadarID.one;
    private Color32 _color = Color.black;
    //SpriteRenderer radarRen;
    Transform radarBody;
    SpriteShapeRenderer beamRen;
    
    //public int radarID = 1;

    float rotateFactor = 30.0f;
    public float maxRotation = 90.0f;
    private float currentRotation = 0.0f;
    private float prevRotation = 0f;
    private float smooth = 3f;

    public bool rotateLeft = false;
    public bool rotateRight = false;

    private void OnEnable()
    {
        //radarRen = GetComponentInChildren<SpriteRenderer>();
        beamRen = GetComponentInChildren<SpriteShapeRenderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //radarRen.color = _color;
        radarBody = transform.Find("RadarBody");
        beamRen.color = new Color32(_color.r, _color.g, _color.b, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (rotateLeft)
        {
            RotateDish(true);
        }

        if (rotateRight)
        {
            RotateDish(false);
        }

        //Should re-map this part to airconsole 

        switch (radarID)
        {
            case GameManager.RadarID.one:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    RaiseLeftRotateFlag(true);
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    StopRotation();
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    RaiseLeftRotateFlag(false);
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    StopRotation();
                }
                break;
            case GameManager.RadarID.two:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    RaiseLeftRotateFlag(true);
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    StopRotation();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    RaiseLeftRotateFlag(false);
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    StopRotation();
                }
                break;
            case GameManager.RadarID.three:
                if (Input.GetKeyDown(KeyCode.V))
                {
                    RaiseLeftRotateFlag(true);
                }
                if (Input.GetKeyUp(KeyCode.V))
                {
                    StopRotation();
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    RaiseLeftRotateFlag(false);
                }
                if (Input.GetKeyUp(KeyCode.N))
                {
                    StopRotation();
                }
                break;
            case GameManager.RadarID.four:
                if (Input.GetKeyDown(KeyCode.I))
                {
                    RaiseLeftRotateFlag(true);
                }
                if (Input.GetKeyUp(KeyCode.I))
                {
                    StopRotation();
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    RaiseLeftRotateFlag(false);
                }
                if (Input.GetKeyUp(KeyCode.P))
                {
                    StopRotation();
                }
                break;
            default:
                break;
        }
    }

    void RaiseLeftRotateFlag(bool left)
    {
        if (left)
        {
            rotateLeft = true;
            rotateRight = false;
        }
        else
        {
            rotateRight = true;
            rotateLeft = false;
        }
    }

    void StopRotation()
    {
        rotateLeft = false;
        rotateRight = false;
    }

    private void RotateDish(bool left)
    {
        
        float rotateAngle = 0f;
        if (left && currentRotation >= -maxRotation)
        {
            rotateAngle = -rotateFactor;
        }
        else if(!left &&  currentRotation <= maxRotation)
        {
            rotateAngle = rotateFactor;
        }
        prevRotation = radarBody.localRotation.eulerAngles.z;
        Vector3 newRotation = new Vector3(0, 0, rotateAngle) + radarBody.rotation.eulerAngles;
        Quaternion target = Quaternion.Euler(newRotation);
        radarBody.rotation = Quaternion.Slerp(radarBody.rotation, target, Time.deltaTime * smooth);
        float rotationChange = radarBody.localRotation.eulerAngles.z - prevRotation;
        if(Mathf.Abs(rotationChange) > 180f)
        {
            if(rotationChange > 0)
                rotationChange = rotationChange - 360f;
            else
                rotationChange = rotationChange + 360f;
        }
        //Debug.Log("rotation change: " + rotationChange);
        currentRotation += rotationChange;
        //Debug.Log("current rotation: " + currentRotation);
    }

    public void SetRadarColor(Color32 color)
    {
        _color = color;
    }
}
