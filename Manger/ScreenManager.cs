
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector2 previousMousePos;
    private bool isDragging = false;
    public float dragSpeed = 0.2f;  // 속도 조절 가능

    public float minX;
    public float maxX;

    //Camera mainCamera;

    void Awake()
    {
       // mainCamera = Camera.main;
    }


    void Update()
    {
        if(GameManager.gameManager.do_game){
        // 모바일 터치 입력 처리
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)  // 터치 시작
                {
                    isDragging = true;
                    previousMousePos = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved && isDragging)  // 터치 이동 중
                {
                    float deltaX = (touch.position.x - previousMousePos.x) * dragSpeed * Time.deltaTime;
                    MoveCamera(deltaX);  
                    previousMousePos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)  
                {
                    isDragging = false;
                }
            }

            //  PC 마우스 입력 처리
            else if (Input.GetMouseButtonDown(0)) 
            {
                isDragging = true;
                previousMousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) && isDragging)  
            {
                float deltaX = (Input.mousePosition.x - previousMousePos.x) * dragSpeed * Time.deltaTime;
                MoveCamera(deltaX); 
                previousMousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0)) 
            {
                isDragging = false;
            }
        }

        if(!GameManager.gameManager.do_game && transform.position != new Vector3 (-0.4f,0,-10)){
            transform.position = new Vector3(-0.4f,0,-10);
        }

    }

 
    void MoveCamera(float deltaX)
    {
        float newX = Mathf.Clamp(transform.position.x - deltaX, minX, maxX); 
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
