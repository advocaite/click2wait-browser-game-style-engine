﻿// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	[System.Serializable]
	public partial struct SkillSyncStruct : ISyncableStruct<SkillTemplate>
	{
	
    	public int		slot;
 		public int 		hash;
 		public byte 	state;
 		public double	timer;
 		
		public int 		level;

		// -------------------------------------------------------------------------------
		public SkillSyncStruct(int _slot, SkillTemplate template, byte _state, double _timer, int _level)
		{
			slot	= _slot;
			hash 	= (template == null) ? 0 : template.name.GetDeterministicHashCode();
			state	= _state;
			timer	= Math.Max(0,_timer);
			level 	= Mathf.Max(1,_level);
		}
		
		// -------------------------------------------------------------------------------
		public SkillTemplate template
		{
			get
			{
				if (hash == 0) return null;
				
				if (!SkillTemplate.dict.ContainsKey(hash))
					throw new KeyNotFoundException("[Missing] SkillTemplate not found in Resources: " + hash);
				return SkillTemplate.dict[hash];
			}
		}   
		 
    	// -------------------------------------------------------------------------------
    	public void Update(GameObject player)
    	{
    		
    		// check sell timer
    		// THEN
    		// Reset();
    	
    	}
    	
    	// -------------------------------------------------------------------------------
    	public bool Active
    	{
    		get { return hash != 0; }
    	}
    	
    	// -------------------------------------------------------------------------------
		public bool CanSell
		{
			get
			{
				foreach (LevelCurrencyCost cost in template.sellCost)
				{
					if (cost.Valid && Active && level > 0)
						return true;
				}
				return false;
			}
		}
		
		// -------------------------------------------------------------------------------
		public void Remove(long _amount=1)
		{
			// set sell timer
			// OR
			Reset();
		}
		
    	// -------------------------------------------------------------------------------
		public void Reset()
		{
			level = 0;
			hash = 0;
			state = Constants.STATE_NONE;
			timer = 0;
		}
		
    	// -------------------------------------------------------------------------------
    	public float TimeRemaining()
    	{
        	return NetworkTime.time >= timer ? 0 : (float)(timer - NetworkTime.time);
    	}
    	
	}
	
	public class SyncListSkillSyncStruct : SyncList<SkillSyncStruct> { }
	
}