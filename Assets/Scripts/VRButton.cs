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

    //�� ��ũ��Ʈ������ Event��� ���� ����ؼ� ��ư�� �� �����ϰ� �������� �� á�� ��
    //� �̺�Ʈ�� �Ͼ�� �� ���̴ٸ� �����ִ� ��ũ��Ʈ�Դϴ�.
    //�� ���� ��� Ÿ��Ʋ������ ���ӽ��� ��ư�� ���� �� ����� �̴ϴ�.

    // Update is called once per frame
    void Update()
    {
        if (triggerEnter)
        {
            progress = progress + Time.deltaTime; //�����Ӹ��� 
            GetComponent<Slider>().value = progress; //�����Ӹ��� Slider�������� ���� �˴ϴ�.

            if (progress >= 1f)
            {
                selectEvent.Invoke();  //�Ƹ��� 30������ -�� 1�ʰ� ������ �̺�Ʈ�� Ȱ��ȭ�ǰ� �߽��ϴ�.
                Destroy(gameObject);
                //�̰� �ǳ��ȵǳ� Ȯ�ο��̰� Destroy�� ������ ���� ���� ��ũ��Ʈ�� ����
                //SceneManager.LoadScene()���� �ٲ�߰ڧc ����
            }
        }
    }

    void OnGazeEnter()//�Ʊ� ������ȴ� �κ��Դϴ�. Slider�� �ü��� ������ �� �Լ��� ����˴ϴ�.
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