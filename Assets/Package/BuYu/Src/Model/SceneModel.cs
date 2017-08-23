/***********************************************
	FileName: SceneModel.cs	    
	Creation: 2017-08-03
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GF;
using Lobby;
using UnityEngine.SceneManagement;

namespace BuYu
{
    public class SceneModel : AppModel
    {
        static short m_InitEndCount = 0;
        static short m_InitStartCount = 0;

        byte m_RoomType;
        byte m_RoomRateIndex;
        bool m_bClearScene;
        float m_fClearTime = 0;
        bool m_bRefreshScene = false;
        private bool isUpdate = false; //是否可以更新
        SceneBulletMgr m_BulletMgr;
        SceneFishMgr m_FishMgr;
        SceneEffectMgr m_EffectMgr;
        ScenePlayerMgr m_PlayerMgr;
        SceneSkillMgr m_SkillMgr;
        LauncherEffectMgr m_LauncherEffectMgr;
        SceneChestMgr m_ChestMgr;


        LC_Cmd_JoinTableResult m_Jtable = null;
        private JoinRoomData m_roomDate;
        
        uint bulletTick = 0;

        public SceneModel()
        {
            Init();
        }

        public void Init()
        {
            isUpdate = false;
            RegisterEvent();
            SceneRuntime.Init(this);
        }

        public void RegisterEvent()
        {
            NetManager.Instance.AddNetEventListener(NetCmdType.CMD_FISH, OnFishEnter);
            NetManager.Instance.AddNetEventListener(NetCmdType.CMD_PLAYER_JOIN, OnPlayerJoin);
            NetManager.Instance.AddNetEventListener(NetCmdType.CMD_LC_JoinTable, OnJoinTable);
            NetManager.Instance.AddNetEventListener(NetCmdType.CMD_BULLET, OnLaunchBullet);
        }

        public System.Collections.IEnumerator MainInitProcedure()
        {
            StartInit();
            while (m_roomDate==null)
            {
                yield return new WaitForEndOfFrame();
            }
            //异步加载
            m_FishMgr = new SceneFishMgr();
            m_BulletMgr = new SceneBulletMgr();
            m_PlayerMgr = new ScenePlayerMgr();
            m_SkillMgr = new SceneSkillMgr();
            m_EffectMgr = new SceneEffectMgr();
            m_LauncherEffectMgr = new LauncherEffectMgr();
            m_ChestMgr = new SceneChestMgr();

            m_FishMgr.Init(); yield return new WaitForEndOfFrame();
            m_BulletMgr.Init(); yield return new WaitForEndOfFrame();
            m_LauncherEffectMgr.Init(); yield return new WaitForEndOfFrame();
            m_PlayerMgr.Init(); yield return new WaitForEndOfFrame();
            m_SkillMgr.Init(); yield return new WaitForEndOfFrame();
            m_EffectMgr.Init(); yield return new WaitForEndOfFrame();
            m_ChestMgr.Init(); yield return new WaitForEndOfFrame();

            SceneRuntime.Init(this);
            ResetScene(true);

            while (PathManager.Instance.IsInitOK == false)
            {
                yield return new WaitForEndOfFrame();
            }
            SubStartCount();
            EndInit();
            isUpdate = true;
            yield break;
        }


        public void OnFishEnter(IEvent iEvent)
        {
            Debug.Log("OnFishEnter");
            NetCmdPack cmd = iEvent.parameter as NetCmdPack;
            FishManager.Instance.LaunchFish(cmd);
           
        }

        public NetCmdBase JoinTable
        {
            get { return m_Jtable; }
        }


        public void OnLaunchBullet(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            if (pack != null)
            {
                NetCmdBullet cmd = (NetCmdBullet)pack.cmd;
                byte clientSeat, id;
                SceneRuntime.BuuletIDToSeat(cmd.BulletID, out clientSeat, out id);
                if (clientSeat == SceneRuntime.MyClientSeat)
                {
                    /*uint time = Utility.GetTickCount() - bulletTick;
                SceneMain.Instance.bulletTime.AddTime(time);*/
                }
                m_PlayerMgr.LaunchBullet(pack);
            }
        }
        public void OnJoinTable(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            m_Jtable = (LC_Cmd_JoinTableResult)pack.cmd;

            if (m_Jtable.Result)
            {
                Debug.Log("LC_Cmd_JoinTableResult 来了");
                //在玩家身上设置玩家桌子ID
                PlayerRole.Instance.RoleInfo.RoleMe.SetTableTypeID(m_Jtable.bTableTypeID);

                //PlayerRole.Instance.RoleGameData.OnHandleRoleJoinTable();//表示玩家已经进入房间了
                m_roomDate = new JoinRoomData();
                m_roomDate.RoomID = m_Jtable.bTableTypeID;
                m_roomDate.BackgroundImage = m_Jtable.BackgroundImage;
                m_roomDate.LauncherType = m_Jtable.LauncherType;
                m_roomDate.Seat = m_Jtable.SeatID;
                m_roomDate.RateIndex = m_Jtable.RateIndex;
                m_roomDate.Energy = m_Jtable.Energy;
                //LogicManager.Instance.Forward(ncg);
            }

        }
         public void OnPlayerJoin(IEvent iEvent)
        {
            Debug.Log("CMD_PLAYER_JOIN");
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            NetCmdPlayerJoin ncp = (NetCmdPlayerJoin)pack.cmd;
            PlayerExtraData pd = new PlayerExtraData();
            pd.playerData.ID = ncp.PlayerInfo.ID;
            pd.playerData.GoldNum = ncp.PlayerInfo.GoldNum;
            pd.playerData.ImgCrc = ncp.PlayerInfo.ImgCrc;
            pd.playerData.Level = ncp.PlayerInfo.Lvl;
            pd.playerData.Name = ncp.PlayerInfo.Name;
            byte clientSeat = SceneRuntime.ServerToClientSeat(ncp.Seat);
            bool launcherValid;
            byte clientLauncherType;
            SceneRuntime.CheckLauncherValid(ncp.LauncherType, out clientLauncherType, out launcherValid);
            //PlayerManager.Instance.PlayerJoin(pd, clientSeat, ncp.rateIndex, clientLauncherType, launcherValid);
            m_PlayerMgr.PlayerJoin(pd, clientSeat, ncp.rateIndex, clientLauncherType, launcherValid);
        }

        public void ResetPlayerData(JoinRoomData jrd, bool bFirst)
        {
            byte serverSeat = jrd.Seat;
            byte serverLauncherType = jrd.LauncherType;
            SceneBoot.Instance.SwapBackgroundImage(jrd.BackgroundImage);
            m_RoomType = jrd.RoomID;
            m_RoomRateIndex = FishConfig.Instance.m_TableInfo.m_TableConfig[m_RoomType].MinRate;// ExtraSetting.RoomDataList[m_RoomType].RoomRateIdx;
                                                                                                //管理器初始化
            /*if (bFirst || SceneRuntime.BackgroundIndex != jrd.BackgroundImage)
            {
                m_EffectMgr.ClearBackEffect();
                m_EffectMgr.LoadBackEffect(jrd.BackgroundImage);
            }*/
            SceneRuntime.Inversion = serverSeat > 1;
            SceneRuntime.BackgroundIndex = jrd.BackgroundImage;
            m_PlayerMgr.MyClientSeat = SceneRuntime.ServerToClientSeat(serverSeat);
            PlayerMgr.ClearAllPlayer();

            //加入自己
            bool launcherValid;
            byte clientLauncherType;
            SceneRuntime.CheckLauncherValid(
                serverLauncherType,
                out clientLauncherType,
                out launcherValid);
            //获取自己的消息
            RoleMe pMe = PlayerRole.Instance.RoleInfo.RoleMe;
            pMe.SetSeat(serverSeat);
            PlayerExtraData pPlayer = new PlayerExtraData();
            pPlayer.playerData.GoldNum =
                (int)PlayerRole.Instance.GetPlayerGlobelBySeat(pMe.GetSeat());
            pPlayer.playerData.ID = pMe.GetUserID();
            pPlayer.playerData.ImgCrc = pMe.GetFaceID();
            pPlayer.playerData.Level = (byte)pMe.GetLevel();
            pPlayer.playerData.Name = pMe.GetNickName();
            m_PlayerMgr.PlayerJoin(pPlayer,
                m_PlayerMgr.MyClientSeat,
                jrd.RateIndex,
                clientLauncherType,
                launcherValid);
            m_PlayerMgr.UpdateEnergy(jrd.Energy);
        }

        public void ResetScene( bool bFirst = false)
        {
            ResetPlayerData(m_roomDate, bFirst);
            if (bFirst == false)
                RefreshScene();
        }

        public void RefreshScene(bool bSendResetCmd = false)
        {
            PlayerManager.Instance.ClearPlayer();

            NetCmdLaunchFailed dd = new NetCmdLaunchFailed();
            dd.Energy = 0;
            dd.FailedType = (byte)LaunchFailedType.LFT_CD;
            PlayerManager.Instance.LaunchLaserFailed(dd);
            /*m_bRefreshScene = true;
            if (bSendResetCmd)
                PlayerRole.Instance.RoleInfo.ResetSceneInfo();*/
        }

        //请求发射子弹
        public void LaunchBullet(short angle)
        {
            if (m_bClearScene)
            {
                //清场时不能发子弹。
                return;
            }
            NetCmdBullet ncb = new NetCmdBullet();
            ncb.SetCmdType(NetCmdType.CMD_BULLET);
            ncb.Degree = angle;
            ncb.LockFishID = m_PlayerMgr.LockedFishID;
            ncb.LauncherType = SceneRuntime.SceneLogic.PlayerMgr.MySelf.Launcher.LauncherType;
            //Send<NetCmdBullet>(ncb);
            bulletTick = Utility.GetTickCount();

            //Test
            NetCmdPack pack = new NetCmdPack();
            pack.cmd = ncb;
            pack.tick = bulletTick;
            SceneRuntime.SceneLogic.PlayerMgr.LaunchBullet(pack);
        }

        public override void Update(float delta)
        {
            if (!isUpdate)
            {
                return;
            }
            if (m_bClearScene)
            {
                m_fClearTime += delta;
                if (m_fClearTime >= ConstValue.CLEAR_TIME)
                    m_bClearScene = false;
            }
            //初始化已经完成了
            if (InitCompletion == false)
                return;
            //m_FishMgr.Update(delta);
            m_BulletMgr.Update(delta);
            if (m_PlayerMgr!=null)
            {
                m_PlayerMgr.Update(delta);
            }
            
            /*m_SkillMgr.Update(delta);
            m_EffectMgr.Update(delta);
            m_LauncherEffectMgr.Update(delta);
            m_ChestMgr.Update(delta);*/
        }
        public void Send<T>(NetCmdBase ncb)
        {
            NetManager.Instance.Send<T>(ncb);
        }


        public bool CanProcessCmd()
        {
            return InitCompletion;
        }
        public bool InitCompletion
        {
            get
            {
                return m_InitStartCount == 0;
            }
        }
        public void AddStartCount()
        {
            ++m_InitStartCount;
        }
        public void SubStartCount()
        {
            --m_InitStartCount;
            if (m_InitStartCount == 0)
            {
                /*GlobalEffectMgr.Instance.CloseLoadingMessage();
                LogicManager.Instance.InitCompletion();*/
            }
        }
        public void Shutdown()
        {
            //GlobalHallUIMgr.Instance.CloseHead();
            m_FishMgr.Shutdown();
            m_BulletMgr.Shutdown();
            m_PlayerMgr.Shutdown();
            m_SkillMgr.Shutdown();
            m_EffectMgr.Shutdown();
            m_LauncherEffectMgr.ShutDown();
            m_ChestMgr.ShutDown();
            /*m_BtnsMgr.ShutDown();
            m_NewGide.ShutDown();*/
            // GlobalHallUIMgr.Instance.ShutDowNotic();
            GlobalEffectMgr.Instance.Clear();
            SceneRuntime.Shutdown();
            PlayerRole.Instance.OnUserLeaveTable();
        }
        public bool bClearScene
        {
            get { return m_bClearScene; }
        }
        public SceneBulletMgr BulletMgr
        {
            get { return m_BulletMgr; }
        }
        /*public SceneBtnsMrg BtnsMgr
        {
            get { return m_BtnsMgr; }
        }*/
        public SceneFishMgr FishMgr
        {
            get { return m_FishMgr; }
        }

        public SceneEffectMgr EffectMgr
        {
            get { return m_EffectMgr; }
        }

        public ScenePlayerMgr PlayerMgr
        {
            get { return m_PlayerMgr; }
        }
        
        public LauncherEffectMgr LauncherEftMgr
        {
            get { return m_LauncherEffectMgr; }
        }

        public SceneSkillMgr SkillMgr
        {
            get { return m_SkillMgr; }
        }

        public void StartInit()
        {
            ++m_InitStartCount;
        }
        public void EndInit()
        {
            ++m_InitEndCount;
        }

        public bool IsCompletion
        {
            get
            {
                return m_InitStartCount == m_InitEndCount;
            }
        }

    }

}
