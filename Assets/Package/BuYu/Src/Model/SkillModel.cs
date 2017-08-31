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

        bool[] m_bUseing = new bool[2];
        bool m_bClearScene = true;
        byte m_CurSkillType;

        public SkillModel()
        {
            Init();
        }

        public void Init()
        {

        }

        public void RegisterEvent()
        {
            //服务器下发技能是否使用成功
            NetManager.Instance.AddNetEventListener(NetCmdType.CMD_SKILL_FAILLED, OnSkillFail);

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

        public void OnProcessSkill()
        {
            if (m_bUseing[0] == false)
            {
                if (PlayerRole.Instance.RoleInfo.RoleMe.GetVipLevel() < 2)
                {
                    UIManager.Instance.ShowMessage("VIP 等级不足",MessageBoxEnum.Style.Ok,null);
                    //GlobalEffectMgr.Instance.ShowMessageBox(string.Format(StringTable.GetString("VIP_UnLock_Tips"), 2), MssageType.VIP_UnLock_Msg_7);
                }
                return;
            }
            if (NumRequire((SkillType)m_CurSkillType) == false)
                return;
           UseSkill((SkillType)m_CurSkillType);

            //GlobalAudioMgr.Instance.PlayOrdianryMusic(Audio.OrdianryMusic.m_ClickSkill);
        }

        public void UseSkill(SkillType skill)
        {
            if (m_bClearScene)
            {
                //清场时不能发技能。
                return;
            }
            NetCmdUseSkill cmd = new NetCmdUseSkill();
            cmd.SetCmdType(NetCmdType.CMD_USE_SKILL);
            cmd.SkillID = (byte)skill;
            NetManager.Instance.Send<NetCmdUseSkill>(cmd);
        }

        public bool NumRequire(SkillType skill)
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
                            UIManager.Instance.ShowMessage("暂时未处理", MessageBoxEnum.Style.Ok, null);
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
            if (byseat == SceneRuntime.SceneLogic.PlayerMgr.MyClientSeat)
            {
                m_nCountRecord[(byte)index]++;
            }
        }

        public void Update(float deltaTime)
        {
            
        }

        public bool UpdateCD(float deltaTime, byte Indx)
        {
            return false;
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
            
        }

        //普通技能
        public void UpdateSkillState()
        {
            
        }

        public void PlayCD(float time, byte Indx)
        {
            
        }

    }


}

