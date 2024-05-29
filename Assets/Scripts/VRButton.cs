using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{

    bool triggerEnter = false;
    float progress = 0f;

    public UnityEvent selectEvent;

    //이 스크립트에서는 Event라는 것을 사용해서 버튼을 쭉 응시하고 게이지가 꽉 찼을 때
    //어떤 이벤트가 일어나게 할 것이다를 보여주는 스크립트입니다.
    //저 같은 경우 타이틀씬에서 게임시작 버튼을 만들 때 사용할 겁니다.

    // Update is called once per frame
    void Update()
    {
        if (triggerEnter)
        {
            progress = progress + Time.deltaTime; //프레임마다 
            GetComponent<Slider>().value = progress; //프레임마다 Slider게이지가 차게 됩니다.

            if (progress >= 1f)
            {
                selectEvent.Invoke();  //아마도 30프레임 -즉 1초가 지나면 이벤트가 활성화되게 했습니다.
                Destroy(gameObject);
                //이건 되나안되나 확인용이고 Destroy를 다음엔 빼고 따로 스크립트를 만들어서
                //SceneManager.LoadScene()으로 바꿔야겠쬬 ㅋㅋ
            }
        }
    }

    void OnGazeEnter()//아까 말씀드렸던 부분입니다. Slider에 시선이 닿으면 이 함수가 실행됩니다.
    {
        triggerEnter = true;
    }
    void OnGazeExit()
    {
        triggerEnter = false;
        progress = 0f;
        GetComponent<Slider>().value = 0f;
    }
}