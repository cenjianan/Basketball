using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallNumItem : MonoBehaviour
{
    public TextMeshProUGUI txtNum;

    public Image imagedui;
    

    public void Init(int id)
    {
        switch(id) 
        {
            case 0:
                if(GameManager.GetInstance().redBallsNum != -1) 
                {
                    txtNum.text = GameManager.GetInstance().redBallsNum.ToString();
                }
                else
                    gameObject.SetActive(false);

                break;
            case 1:
                if (GameManager.GetInstance().yellowBallsNum != -1)
                {
                    txtNum.text = GameManager.GetInstance().yellowBallsNum.ToString();
                }
                else
                    gameObject.SetActive(false);
                break;
            case 2:
                if (GameManager.GetInstance().blueBallsNum != -1)
                {
                    txtNum.text = GameManager.GetInstance().blueBallsNum.ToString();
                }
                else
                    gameObject.SetActive(false);
                break;
            case 3:
                if (GameManager.GetInstance().greenBallsNum != -1)
                {
                    txtNum.text = GameManager.GetInstance().greenBallsNum.ToString();
                }
                else
                    gameObject.SetActive(false);
                break;
            case 4:
                if (GameManager.GetInstance().purpleBallsNum != -1)
                {
                    txtNum.text = GameManager.GetInstance().purpleBallsNum.ToString();
                }
                else
                    gameObject.SetActive(false);
                break;
            case 5:
                if (GameManager.GetInstance().anyBallsNum != -1)
                {
                    txtNum.text = GameManager.GetInstance().anyBallsNum.ToString();
                }
                else
                    gameObject.SetActive(false);
                break;

        }
    }

    public void UpdateNum(int num)
    {
        txtNum.text = num.ToString();
    }

    public void UpdateState()
    {
        txtNum.gameObject.SetActive(false);
        imagedui.gameObject.SetActive(true);
    }
}
