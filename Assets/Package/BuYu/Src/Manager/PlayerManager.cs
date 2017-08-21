/***********************************************
	FileName: PlayerManager.cs	    
	Creation: 2017-08-03
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using UnityEngine;

namespace BuYu
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private ScenePlayer[] _mPlayerList = new ScenePlayer[ConstValue.PLAYER_MAX_NUM];

        private Object[] _mLauncherObj = new Object[ConstValue.PLAYER_MAX_NUM];
        private Object[] _mGunBarrelObj = new Object[ConstValue.MAX_LAUNCHER_NUM*2];
        private Object _mComboObj = null;
        private Object m_ComboEftObj = null;
        private Object m_LockedObj = null;

        private byte _mMyClientSeat; //自己的座位
        private ushort _mLockedFishId = 0; //锁定的鱼ID，0未锁定
        private bool _mBAutoShot = false;
        private bool _mBAutoLocked = false; //自动锁定
        private uint _mLockInterval = 0;

        private PlayerManager()
        {

        }

        /*private PlayerManager(Object mComboObj, uint mLockInterval, bool _mBAutoLocked, bool _mBAutoShot, ushort mLockedFishId, byte mMyClientSeat)
        {
            _mComboObj = mComboObj;
            _mLockInterval = mLockInterval;
            _mLockedFishId = mLockedFishId;
            _mMyClientSeat = mMyClientSeat;
            _mBAutoShot = _mBAutoShot;
            _mBAutoLocked = _mBAutoLocked;
        }*/

        public void Init()
        {
            for (byte i = 0; i < ConstValue.PLAYER_MAX_NUM; ++i)
            {
                _mLauncherObj[i] = ResManager.Instance.LoadObject(string.Format("Emplacement{0}", i), "BuYu/Prefab/Gun/",
                    ResType.SceneRes);
            }
            /*for (byte j = 0; j < _mGunBarrelObj.Length; ++j)
            {
                _mGunBarrelObj[j] = ResManager.Instance.LoadObject(string.Format("GunBarrel{0}", j), "SceneRes/Prefab/UI/Launcher/", ResType.SceneRes);
            }
            _mComboObj = ResManager.Instance.LoadObject("Combo_UI", "SceneRes/Prefab/UI/DoubleHit/", ResType.SceneRes);
            m_ComboEftObj = ResManager.Instance.LoadObject("UIEf_Combo", "SceneRes/Prefab/UI/Combo/", ResType.SceneRes);
            m_LockedObj = ResManager.Instance.LoadObject(("LockedFishUI"), "SceneRes/Prefab/UI/LockedFish/", ResType.SceneRes);*/
        }


        public bool PlayerJoin(PlayerExtraData player, byte clientSeat, byte rateIdx, byte launcherType, bool valid)
        {
            return false;
        }


        public void LaunchLaserFailed(NetCmdLaunchFailed lf)
        {

        }

        public void ClearPlayer()
        {
            
        }

        public void ClearAllPlayer()
        {
            
        }

        public ScenePlayer GetPlayer(byte clientSeat)
        {
            if (clientSeat >= _mPlayerList.Length)
                return null;
            return _mPlayerList[clientSeat];
        }

        public void UpdateEnergy(int energy)
        {
            if (GetPlayer(_mMyClientSeat) == null)
                return;
            _mPlayerList[_mMyClientSeat].Launcher.UpdateEnergy(energy);
        }


        public byte MyClientSeat
        {
            get { return _mMyClientSeat; }
            set { _mMyClientSeat = value; }
        }
    }

}

