/***********************************************
	FileName: FishResManager.cs	    
	Creation: 2017-08-04
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

    public class FishResManager : Singleton<FishResManager>
    {
        readonly List<ResFishData> _mFishList = new List<ResFishData>();
        readonly List<GroupDataList> _mFishGroupList = new List<GroupDataList>();
        readonly List<Object> m_FishObjList = new List<Object>();
        Object _mBoxObj;
        Object _mCardObj;
        byte _mValidCount;
        byte _mValidMaxCount;

        private FishResManager()
        {
            
        }
        public void AddGroupData(GroupDataList gdl)
        {
            _mFishGroupList.Add(gdl);
        }
        public void AddFishData(ResFishData fd)
        {
            _mFishList.Add(fd);
        }
        public byte ValidFishCount
        {
            get
            {
                return _mValidCount;
            }
        }
        public byte ValidMaxCount
        {
            get
            {
                return _mValidMaxCount;
            }
        }
        IEnumerator InitProcedure(object objobj)
        {
            _mBoxObj = ResManager.Instance.LoadObject("Box0", "BuYu/FishRes/Prefab/", ResType.FishRes,false);
            _mCardObj = ResManager.Instance.LoadObject("FishCard", "BuYu/FishRes/Prefab/", ResType.FishRes, false);

            for (byte i = 0; i < 30; ++i)
            {
                string fishid = "Fish" + i.ToString();
                Object obj = ResManager.Instance.LoadObject(fishid, "BuYu/FishRes/Prefab/", ResType.FishRes, false);
                if (obj == null)
                {
                    m_FishObjList.Add(null);
                    continue;
                }
                ++_mValidCount;
                _mValidMaxCount = i;
                m_FishObjList.Add(obj);
            }
            //InitLogic.EndInit();
            yield break;
        }
        public bool Init()
        {
            //InitProcedure(null);
            //InitLogic.StartInit();
            Boot.Instance.StartInnerCoroutine(InitProcedure(null));
            return true;
        }
        public int GetGroupCount()
        {
            return _mFishGroupList.Count;
        }
        public GroupDataList GetFishGroup(ushort groupID)
        {
            if (_mFishGroupList.Count <= groupID || _mFishGroupList[groupID] == null)
            {
                return null;
            }
            return _mFishGroupList[groupID];
        }
        public ResFishData GetFishData(byte idx)
        {
            if (_mFishList.Count <= idx || _mFishList[idx] == null)
            {
                return null;
            }
            return _mFishList[idx];
        }
        public Object GetFishObj(byte idx)
        {
            if (m_FishObjList.Count <= idx)
                return null;
            return m_FishObjList[idx];
        }
        public int FishObjCount
        {
            get
            {
                return m_FishObjList.Count;
            }
        }
        public Object BoxObj
        {
            get { return _mBoxObj; }
            set { _mBoxObj = value; }
        }
        public Object CardObj
        {
            get { return _mCardObj; }
        }
    }



}