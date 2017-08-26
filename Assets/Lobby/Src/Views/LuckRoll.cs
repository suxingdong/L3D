using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckRoll : MonoBehaviour {

    //幸运转盘
    private Transform mLuckBackGround;

    private Button btnLuck;
    //初始旋转速度
    private float mInitSpeed;
    //速度变化值
    private float mDelta = 3f;

    //转盘是否暂停
    private bool isPause = true;

    //初始旋转速度
    private float mReward;
    void Start()
    {
        mLuckBackGround = GameObject.Find("BackGround/ZhuanPan").transform;
        btnLuck = GameObject.Find("BtnLuck").GetComponent<Button>();
        btnLuck.onClick.AddListener(delegate () {
            OnClick();
        });
        
    }

    //开始抽奖
    public void OnClick()
    {
        if (isPause)
        {
            mInitSpeed = 1000;
            //随机生成一个初始速度
            mReward = Random.Range(100, 500);
            //开始旋转
            isPause = false;
        }
    }

    void Update()
    {
        if (!isPause)
        {

            //转动转盘(-1为顺时针,1为逆时针)
            mLuckBackGround.Rotate(new Vector3(0, 0, -1) * mInitSpeed * Time.deltaTime);
            //让转动的速度缓缓降低
            mInitSpeed -= mDelta;
            //当转动的速度为0时转盘停止转动
            if (mInitSpeed <= 0)
            {
                //转动停止
                isPause = true;
            }
        }
    }
}
