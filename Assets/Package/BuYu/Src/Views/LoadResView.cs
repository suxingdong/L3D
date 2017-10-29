/***********************************************
	FileName: LoadResView.cs	    
	Creation: 2017-08-08
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using GF;
using GF.UI;
using Lobby;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace BuYu
{
    public class LoadResView : AppView
    {
        public Image ProgressBar;
        private float speed = 0.2f;
        private SceneModel sceneModel;
        private SkillModel skillModel;
        protected override void OnStart()
        {
            ProgressBar.fillAmount = 0;
            ModelManager.Instance.Register<SceneModel>();
            ModelManager.Instance.Register<SkillModel>();
            sceneModel = ModelManager.Instance.Get<SceneModel>();
            skillModel = ModelManager.Instance.Get<SkillModel>();
            NetManager.Instance.CanProcessCmd = false;
            StartCoroutine(LoadRes());
        }


        IEnumerator LoadRes()
        {
            yield return StartCoroutine(ServerSetting.OnNewInit(null));

            UIManager.Instance.ShowView<GameView>(); yield return new WaitForEndOfFrame();

            NetManager.Instance.CanProcessCmd = true; yield return new WaitForEndOfFrame();

            CL_Cmd_JoinTable ncb = new CL_Cmd_JoinTable();
            ncb.SetCmdType(NetCmdType.CMD_CL_JoinTable);
            ncb.bTableType = ModelManager.Instance.Get<RoomModel>().GetCurRoomId();
            NetManager.Instance.Send<CL_Cmd_JoinTable>(ncb);

            yield return StartCoroutine(sceneModel.MainInitProcedure());

            yield return StartCoroutine(skillModel.MainInitProcedure());

            FishManager.Instance.Init(); yield return new WaitForEndOfFrame();
            BulletManager.Instance.Init(); yield return new WaitForEndOfFrame();
            SkillManager.Instance.Init();yield return new WaitForEndOfFrame();
            PlayerManager.Instance.Init(); yield return new WaitForEndOfFrame();


            sceneModel.ResetScene(true); yield return new WaitForEndOfFrame();

            
            print("资源加载完成");
            ProgressBar.fillAmount = 1;


        }

        protected override void OnUpdate(float time)
        {
            if (ProgressBar.fillAmount < 1)
            {
                ProgressBar.fillAmount += speed * Time.deltaTime;
            }
            else
            {
                
                UIManager.Instance.HideView<LoadResView>();
            }
        }
        

    }


}
