/***********************************************
	FileName: RoleBase.cs	    
	Creation: 2017-08-03
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace  Lobby
{
    public class RoleBase
    {
        public virtual UInt32 GetUserID()
        {
            return 0;
        }
        public virtual string GetNickName()
        {
            return "";
        }
        public virtual UInt16 GetLevel()
        {
            return 0;
        }
        public virtual UInt32 GetExp()
        {
            return 0;
        }
        public virtual UInt32 GetFaceID()
        {
            return 0;
        }
        public virtual bool GetGender()
        {
            return false;
        }
        public virtual UInt32 GetGlobel()
        {
            return 0;
        }
        public virtual UInt32 GetMedal()
        {
            return 0;
        }
        public virtual UInt32 GetCurrency()
        {
            return 0;
        }
        public virtual UInt32 GetAchievementPoint()
        {
            return 0;
        }
        public virtual Byte GetTitleID()
        {
            return 0;
        }
        public virtual UInt32[] GetCharmInfo()
        {
            return null;
        }
        public virtual UInt32 GetProduction()
        {
            return 0;
        }
        public virtual UInt32 GetGameTime()
        {
            return 0;
        }
        public virtual Byte GetSendGiffSum()
        {
            return 0;
        }
        public virtual Byte GetAcceptGiffSum()
        {
            return 0;
        }
        public virtual Byte GetMonthID()
        {
            return 0;
        }
        public virtual UInt32 GetMonthIndex()
        {
            return 0;
        }
        public virtual UInt32 GetMonthGlobel()
        {
            return 0;
        }
        public virtual UInt32 GetMonthScore()
        {
            return 0;
        }
        public virtual Byte GetMonthAddGlobelNum()
        {
            return 0;
        }
        public virtual Byte GetLeaveOnlineDay()
        {
            return 0;
        }
        public virtual Byte GetRelationType()
        {
            return 0;
        }
        public virtual bool IsOnline()
        {
            return false;
        }
        public virtual Byte GetSeat()
        {
            return 0;
        }
        public virtual UInt32 GetMonthUpperSocre()
        {
            return 0;
        }
        public virtual void SetUserID(UInt32 dwUserID)
        {

        }
        public virtual void SetNickName(string NickName)
        {

        }
        public virtual void SetLevel(UInt16 wLevel)
        {

        }
        public virtual void SetExp(UInt32 dwExp)
        {

        }
        public virtual void SetFaceID(UInt32 FaceID)
        {

        }
        public virtual void SetGender(bool bGender)
        {

        }
        public virtual void SetGlobel(UInt32 dwGlobel)
        {

        }
        public virtual void SetMedal(UInt32 dwMedal)
        {

        }
        public virtual void SetCurrency(UInt32 Currency)
        {

        }
        public virtual void SetAchievementPoint(UInt32 dwAchievementPoint)
        {

        }
        public virtual void SetTitleID(Byte bTitleID)
        {

        }
        public virtual void SetCharmInfo(UInt32[] CharmArray)
        {

        }

        public virtual void SetProduction(UInt32 dwProduction)
        {

        }
        public virtual void SetGameTime(UInt32 GameTime)
        {

        }
        public virtual void SetSendGiffSum(Byte SendSum)
        {

        }
        public virtual void SetAcceptGiffSum(Byte AccpetSum)
        {

        }
        public virtual void SetMonthID(Byte MonthID)
        {

        }
        public virtual void SetMonthIndex(UInt32 MonthIndex)
        {

        }
        public virtual void SetMonthGlobel(UInt32 MonthGlobel)
        {

        }
        public virtual void SetMonthScore(UInt32 MonthScore)
        {

        }
        public virtual void SetMonthAddGlobelNum(Byte MonthAddGlobelNum)
        {

        }
        public virtual void SetLeaveOnlineDay(Byte DiffDay)
        {
        }
        public virtual void SetRelationType(Byte Type)
        {

        }
        public virtual void SetIsOnline(bool IsOnline)
        {

        }
        public virtual void SetMonthUpperSocre(UInt32 UpperSocre)
        {

        }
        public virtual void SetSeat(Byte SeatID)
        {

        }
        //public virtual UInt32 GetClientIP()
        //{
        //    return 0;
        //}
        //public virtual void SetClientIP(UInt32 ClientIP)
        //{

        //}

        public virtual string GetIPAddress()
        {
            return "";
        }
        public virtual void SetIPAddress(string Add)
        {

        }


        public virtual Byte GetVipLevel()
        {
            return 0;
        }
        public virtual void SetVipLevel(Byte VipLevel)
        {

        }
        public virtual bool GetIsInMonthCard()
        {
            return false;
        }
        public virtual void SetIsInMonthCard(bool IsInMonthCard)
        {

        }

        public virtual UInt32 GetGameID()
        {
            return 0;
        }

    }
}

