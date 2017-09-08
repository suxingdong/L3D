/***********************************************
	FileName: SkillModel.cs	    
	Creation: 2017-08-31
	Author：East.Su
	Version：V1.0.0
	Desc: 技能相关操作
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using GF.UI;
using Lobby;
using UnityEngine;

namespace BuYu
{
    class CDInfo
    {
        public float m_CDTime;
        public float m_CurTime;
        public byte m_CDIndx;
        public bool m_bUseing;
    }

  
    public class SkillModel : AppModel
    {
        public ushort[] m_nCountRecord = new ushort[(byte)SkillType.SKILL_MAX];
        List<CDInfo> m_CDList = new List<CDInfo>();
        bool[] m_bCanUse = new bool[2];     //技能是否正在使用 0是炮弹技能 1是锁定
        bool m_bClearScene = true;          //是否正在切换捕鱼场景
        byte m_CurSkillType;                //当前技能类型

        SceneSkillMgr m_SkillMgr;          //技能特效管理器

        public SkillModel()
        {
            Init();
        }

        public void Init()
        {
            RegisterEvent();
            UpdateLockedSkillSate();
        }

        public System.Collections.IEnumerator MainInitProcedure()
        {
            m_SkillMgr = new SceneSkillMgr();
            yield return new WaitForEndOfFrame();
            m_SkillMgr.Init(); yield return new WaitForEndOfFrame();
        }

        public void RegisterEvent()
        {
            //服务器下发技能是否使用成功
            _RegisterEvent(NetCmdType.CMD_SKILL_FAILLED, OnSkillFail);
            //陨石技能
            _RegisterEvent(NetCmdType.CMD_SKILL_DISASTER_RESPONSE, OnUseSkillDisaster);
            //风暴技能
            _RegisterEvent(NetCmdType.CMD_SKILL_TORNADO_RESPONSE, OnUseSkillTornado);
            //寒冰技能
            _RegisterEvent(NetCmdType.CMD_SKILL_FREEZE_RESPONSE, OnUseSkillFreeze);
            //锁定技能
            _RegisterEvent(NetCmdType.CMD_SKILL_LOCK_RESPONSE, OnUseSkillLock);
            //鱼的碰撞
            _RegisterEvent(NetCmdType.CMD_CATCHED, onCatchedFish);

            _RegisterEvent(NetCmdType.CMD_CLEAR_SCENE, OnChangeScene);
        }

        public void OnSkillFail(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            NetCmdSkillFailed pSkill = pack.cmd as NetCmdSkillFailed;
            switch ((SkillFailedType)pSkill.FailedType)
            {
                case SkillFailedType.SFT_CD:
                    {
                        UIManager.Instance.ShowMessage("正在处理CD时间",MessageBoxEnum.Style.Ok, null);
                        //GlobalHallUIMgr.Instance.ShowSystemTipsUI(StringTable.GetString("Skill_Tip_cd"), 1, false);
                        break;
                    }
                case SkillFailedType.SFT_COUNT:
                    {
                        UIManager.Instance.ShowMessage("道具数量不做", MessageBoxEnum.Style.Ok, null);
                        //GlobalHallUIMgr.Instance.ShowSystemTipsUI(StringTable.GetString("Skill_Tip_count"), 1, false);
                        break;
                    }
                case SkillFailedType.SFT_INVALID:
                    {
                        UIManager.Instance.ShowMessage("技能使用无效", MessageBoxEnum.Style.Ok, null);
                        //GlobalHallUIMgr.Instance.ShowSystemTipsUI(StringTable.GetString("Skill_Tip_invalid"), 1, false);
                        break;
                    }
            }
        }

        public void OnUseSkillDisaster(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            m_SkillMgr.UseSkillDisaster(pack);
        }

        public void OnUseSkillTornado(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            m_SkillMgr.UseSkillTornado(pack);
        }

        public void OnUseSkillFreeze(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            m_SkillMgr.UseSkillDisaster(pack);
        }

        public void OnUseSkillLock(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            m_SkillMgr.UseSkillLock(pack);
        }

        
        public void OnProcessLock()
        {
            if (m_bCanUse[1] == false)
            {
                if (PlayerRole.Instance.RoleInfo.RoleMe.GetVipLevel() < 2)
                {
                    string content = string.Format(StringTable.GetString("VIP_UnLock_Tips"), 2);
                    UIManager.Instance.ShowMessage(content,MessageBoxEnum.Style.Ok, null);
                    //GlobalEffectMgr.Instance.ShowMessageBox(string.Format(StringTable.GetString("VIP_UnLock_Tips"), 2), MssageType.VIP_UnLock_Msg_7);
                }
                return;
            }

            //GlobalAudioMgr.Instance.PlayOrdianryMusic(Audio.OrdianryMusic.m_BtnMusic);
            if (CheckItemNum(SkillType.SKILL_LOCK) == false)
                return;
            if (SceneRuntime.PlayerMgr.MySelf.Launcher != null)
            {
                if (SceneRuntime.PlayerMgr.MySelf.Launcher.IsBankruptcy())
                    return;
            }
            UseSkill(SkillType.SKILL_LOCK);
        }
        public void OnProcessSkill()
        {
            if (m_bCanUse[0] == false)
            {
                if (PlayerRole.Instance.RoleInfo.RoleMe.GetVipLevel() < 2)
                {
                    UIManager.Instance.ShowMessage("VIP 等级不足",MessageBoxEnum.Style.Ok,null);
                    //GlobalEffectMgr.Instance.ShowMessageBox(string.Format(StringTable.GetString("VIP_UnLock_Tips"), 2), MssageType.VIP_UnLock_Msg_7);
                }
                return;
            }
            if (CheckItemNum((SkillType)m_CurSkillType) == false)
                return;
           UseSkill((SkillType)m_CurSkillType);

            //GlobalAudioMgr.Instance.PlayOrdianryMusic(Audio.OrdianryMusic.m_ClickSkill);
        }

        public void UseSkill(SkillType skill)
        {
            /*if (m_bClearScene)
            {
                //清场时不能发技能。
                return;
            }*/
            NetCmdUseSkill cmd = new NetCmdUseSkill();
            cmd.SetCmdType(NetCmdType.CMD_USE_SKILL);
            cmd.SkillID = (byte)skill;
            NetManager.Instance.Send<NetCmdUseSkill>(cmd);
        }

        public bool CheckItemNum(SkillType skill)
        {
            if (PlayerRole.Instance.ItemManager.GetAllItemMap() == null)//背包没数据
            {
                return false;
            }

            if (PlayerRole.Instance.ItemManager.GetItemSum(SkillSetting.SkillDataList[(byte)skill].ItemId, false) < ConsumeCount(skill))//个数不够
            {
                FishSkillToShop pSkillShop = FishConfig.Instance.m_FishScriptMap.GetSkillToShopInfoByID((byte)skill, 0);
                if (pSkillShop != null)
                {
                    byte shopID = pSkillShop.ShopID;
                    byte OnlyID = pSkillShop.ShopOnlyID;
                    uint itemSum = ConsumeCount(skill) - PlayerRole.Instance.ItemManager.GetItemSum(SkillSetting.SkillDataList[(byte)skill].ItemId, false);
                    tagShopConfig pShop = new tagShopConfig();
                    FishConfig.Instance.m_ShopInfo.ShopMap.TryGetValue(shopID, out pShop);
                    if (pShop != null)
                    {
                        if (pShop.ShopItemMap.ContainsKey(OnlyID))
                        {
                            UIManager.Instance.ShowMessage("打开商店购买", MessageBoxEnum.Style.Ok, null);
                            //GlobalHallUIMgr.Instance.ShowConfirmBuyWnd(OnlyID, shopID, itemSum);
                        }                            
                    }
                }
                //  GlobalHallUIMgr.Instance.ShowSystemTipsUI(StringTable.GetString("Skill_Tip_count"), 1, false);          
                return false;
            }
            return true;
        }


        ushort ConsumeCount(SkillType skill)
        {
            byte byBaseCount = 0;
            List<SkillConsume> pRequire = SkillSetting.SkillDataList[(byte)skill].NumRequire;
            if (pRequire.Count != 0)
            {
                byte byIndex = 0;
                for (; byIndex < pRequire.Count; byIndex++)
                {
                    if (m_nCountRecord[(byte)skill] < pRequire[byIndex].byorder)
                    {
                        break;
                    }
                }
                if (byIndex == pRequire.Count)
                {
                    byIndex = (byte)(pRequire.Count - 1);
                }
                byBaseCount = pRequire[byIndex].byCount;
            }
            return (ushort)(byBaseCount);
        }

        public void RecordUsed(SkillType index, byte byseat)
        {
            if (byseat == SceneRuntime.SceneModel.PlayerMgr.MyClientSeat)
            {
                m_nCountRecord[(byte)index]++;
            }
        }

        public override void Update(float delta)
        {
            for (byte i = 0; i < m_CDList.Count;)
            {
                if (UpdateCD(delta, i))
                    ++i;
            }

            if (m_SkillMgr!=null)
            {
                m_SkillMgr.Update(delta);
            }
            
        }

        public bool UpdateCD(float deltaTime, byte Indx)
        {
            m_CDList[Indx].m_CurTime -= deltaTime;
            if (m_CDList[Indx].m_CurTime < 0)
                m_CDList[Indx].m_CurTime = 0;
            //m_CDSprite[m_CDList[Indx].m_CDIndx].fillAmount = m_CDList[Indx].m_CurTime / m_CDList[Indx].m_CDTime;

            if (m_CDList[Indx].m_CurTime <= 0)
            {
                if (m_CDList[Indx].m_CDIndx == 1)
                    SceneRuntime.PlayerMgr.SetLocked(false);
                // m_UIButton[m_CDList[Indx].m_CDIndx].isEnabled = true;
                if (m_CDList[Indx].m_bUseing)
                    m_bCanUse[m_CDList[Indx].m_CDIndx] = true;

                Utility.ListRemoveAt(m_CDList, Indx);
                return false;
            }
            return true;
        }

        public void ShowRateReward()
        {
            
        }

        public void UpdateUnLockRateReward(uint RewardID)
        {
            
        }

        public void UpdateUnLockDataInfo()
        {
            
        }

        public void UpdateLotteryInfo()
        {
            
        }

        public void UpdateLockedSkillSate()
        {
            m_bCanUse[1] = true;
        }

        //普通技能
        public void UpdateSkillState(NetCmdChangeLauncher ncc)
        {
            if (PlayerRole.Instance.RoleInfo.RoleMe.GetVipLevel() >= 2)
            {
                if (SceneRuntime.PlayerMgr.MySelf.Launcher.LauncherType >= 2)
                {
                    byte Idx = LauncherSetting.LauncherDataList[SceneRuntime.PlayerMgr.MySelf.Launcher.LauncherType].nSkillBind;
                    if (Idx != 255)
                    {
                        m_CurSkillType = Idx;
                        m_bCanUse[0] = true;
                        IEvent evt = new GF.Event(EventMsg.UPDATE_CANON_SKILL);
                        evt.parameter = m_CurSkillType;
                        EventManager.Instance.DispatchEvent(evt);
                    }
                }
                else
                {
                    IEvent evt = new GF.Event(EventMsg.HIDE_CANON_SKILL);
                    EventManager.Instance.DispatchEvent(evt);
                }
            }
            else
            {
                IEvent evt = new GF.Event(EventMsg.HIDE_CANON_SKILL);
                EventManager.Instance.DispatchEvent(evt);
            }
            

        }

        public void onCatchedFish(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            m_SkillMgr.FishCatched(pack);
        }

        public void OnChangeScene(IEvent iEvent)
        {
            Debug.Log("切换场景。。。");
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            m_SkillMgr.ClearScene(pack);
        }

        public void OnChangeLauncher(IEvent iEvent)
        {
            NetCmdPack pack = iEvent.parameter as NetCmdPack;
            m_SkillMgr.ClearScene(pack);
        }


        public void PlayCD(float time, byte Indx)
        {
            CDInfo cdInfo = new CDInfo();
            cdInfo.m_CDIndx = Indx;
            cdInfo.m_CDTime = time;
            cdInfo.m_CurTime = time;
            //m_CDSprite[Indx].fillAmount = 1;
            //m_UIButton[Indx].isEnabled = false;
            if (m_bCanUse[Indx])
            {
                cdInfo.m_bUseing = true;
                m_bCanUse[Indx] = false;
            }
            else
                cdInfo.m_bUseing = false;

            m_CDList.Add(cdInfo);

            /*if (type < SkillType.SKILL_LOCK)
                m_SkillBar.PlayCD(time, 0);
            else
            {
                m_SkillBar.PlayCD(time, 1);
            }*/
        }

        public SceneSkillMgr SkillMgr
        {
            get { return m_SkillMgr; }
        }

        public void Shutdown()
        {
            m_SkillMgr.Shutdown();
        }

    }


}

