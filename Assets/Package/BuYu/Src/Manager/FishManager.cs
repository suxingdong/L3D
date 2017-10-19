/***********************************************
	FileName: FishManager.cs	    
	Creation: 2017-08-03
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using UnityEngine;
using Lobby;

namespace BuYu
{

    public class FishManager : Singleton<FishManager>
    {
        readonly Dictionary<ushort, Fish> _mFishList = new Dictionary<ushort, Fish>();
        readonly Dictionary<ushort, DestroyFishData> _mDestroyFishList = new Dictionary<ushort, DestroyFishData>();


        ushort _mFishNum;
        Fish[] _mBackFishList = null;

        private FishManager()
        {

        }

        public ushort FishNum
        {
            get { return _mFishNum; }
        }

        public void Init()
        {
            
        }
        public void LaunchFish(NetCmdPack pack)
        {
            NetCmdFish cmdFish = (NetCmdFish)pack.cmd;
            GroupDataList gdl = FishResManager.Instance.GetFishGroup(cmdFish.GroupID);
            ushort startID = cmdFish.StartID;
            float elapsedTime = Utility.TickSpan(pack.tick) + SceneRuntime.NetDelayTime;
            if (gdl.PathGroupData != null)
            {
                FishPathGroupData pathgroup = gdl.PathGroupData;
                PathLinearInterpolator[] interpList = PathManager.Instance.GetPathGroup(pathgroup.PathGroupIndex, SceneRuntime.Inversion);
                foreach (PathLinearInterpolator interp in interpList)
                {
                    Fish fish = new Fish();
                    fish.Init(startID, pathgroup.FishIndex, pathgroup.FishScaling, 0, pathgroup.ActionSpeed, pathgroup.ActionUnite, pathgroup.Speed, interp);
#if UNITY_EDITOR
                    fish.SetModelName("Fish_PathGroup_" + cmdFish.GroupID);
#endif
                    if (fish.AddElapsedTime(elapsedTime))
                    {
                        SetFish(fish);
                    }
                    else
                        fish.Destroy();

                    if (++startID >= FishSetting.FISH_MAX_NUM)
                        startID = 0;
                }
            }
            else
            {
                float fInv = SceneRuntime.Inversion ? -1.0f : 1.0f;
                int pathIndex = cmdFish.PathID;
                PathLinearInterpolator pi = PathManager.Instance.GetPath(pathIndex, SceneRuntime.Inversion);
                float startX = gdl.FrontPosition.x;
                foreach (GroupData gd in gdl.GroupDataArray)
                {
                    if (gd == null)
                        break;
                    if (gd.FishNum > gd.PosList.Length)
                    {
                        LogMgr.Log("错误的鱼群路径点:" + gd.FishNum + ", posnum:" + gd.PosList.Length);
                        return;
                    }
                    for (int i = 0; i < gd.FishNum; ++i)
                    {
                        float time = BuYuUtils.GetPathTimeByDist(startX, gd.PosList[i].x, pi);
                        Fish fish = new Fish();
                        fish.Init(startID, gd.FishIndex, gd.FishScaling, time, gd.ActionSpeed, gd.ActionUnite, gd.SpeedScaling, pi);
#if UNITY_EDITOR
                        fish.SetModelName("Fish_FishGroup_" + cmdFish.GroupID + "_Path_" + pathIndex);
#endif
                        if (fish.AddElapsedTime(elapsedTime))
                        {
                            fish.SetOffset(new Vector3(0, fInv * gd.PosList[i].y, gd.PosList[i].z));
                            SetFish(fish);
                        }
                        else
                            fish.Destroy();
                        if (++startID == FishSetting.FISH_MAX_NUM)
                            startID = 0;
                    }
                }
            }
        }

        public void SetFish(Fish fish)
        {
            ushort id = fish.FishID;
            Fish findFish;
            if (_mFishList.TryGetValue(id, out findFish))
            {
                if (findFish.IsDelay)
                {
                    findFish.Destroy();
                }
                else
                {
                    LogMgr.Log("存在相同的鱼ID1:" + id.ToString() + "time:" + findFish.Time + ", delta:" +Time.deltaTime + ", timedelta:" + Time.deltaTime);
                    return;
                }
                _mFishList[id] = fish;
            }
            else
                _mFishList.Add(id, fish);
        }

        private float _time1 = 0f;
        private readonly float time2 = 60f;
        private readonly float time3 = 150f;
        private int _boo = 0;

        void UpdateBackupList(float delta)
        {
            if (_mBackFishList != null)
            {
                int n = 0;
                for (int i = 0; i < _mBackFishList.Length; ++i)
                {
                    if (_mBackFishList[i] == null)
                        continue;
                    if (!_mBackFishList[i].Update(delta))
                    {
                        _mBackFishList[i].Destroy();
                        _mBackFishList[i] = null;
                    }
                    else
                    {
                        ++n;
                    }
                }
                if (n == 0)
                    _mBackFishList = null;
            }
        }

        public void Update(float delta)
        {
            _mFishNum = 0;
            List<Fish> fishList = new List<Fish>(_mFishList.Values);
            for (int i = 0; i < fishList.Count; ++i)
            {
                Fish fish = fishList[i];
                if (!fish.Update(delta))
                {
                    DestroyFish(fish, fish.Catched);
                }
                else
                {
                    ++_mFishNum;
                }
            }
            _time1 += Time.deltaTime;
            if (_time1 >= time2 && _mFishNum > 0 && _boo == 0)
            {
                AudioManager.Instance.PlayOrdianryMusic(Audio.OrdianryMusic.m_qingdian);
                // time1 = 0f;
                _boo = 1;
            }
            if (_time1 >= time3 && _mFishNum > 0)
            {
                AudioManager.Instance.PlayOrdianryMusic(Audio.OrdianryMusic.m_haohuai);
                _boo = 0;
                _time1 = 0f;
            }
            UpdateBackupList(delta);
            //CheckFishPos();
        }

        public void DestroyFish(Fish fish, bool catched)
        {
            DestroyFishData dd;
            dd.FishType = fish.FishType;
            _mDestroyFishList[fish.FishID] = dd;
            _mFishList.Remove(fish.FishID);
            fish.Destroy();
        }

    }

}

