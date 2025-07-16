using System;
using System.Collections.Generic;

namespace LibHammer.Gamestate.InterphaseActions;

public class InterphaseManager
{
	List<InterphaseAction> precommand = new();
	List<InterphaseAction> postcommand = new();
	List<InterphaseAction> premove = new();
	List<InterphaseAction> postmove = new();
	List<InterphaseAction> preshoot = new();
	List<InterphaseAction> postshoot = new();
	List<InterphaseAction> precharge = new();
	List<InterphaseAction> postcharge = new();
	List<InterphaseAction> prefight = new();
	List<InterphaseAction> postfight = new();
	List<InterphaseAction> postgame = new();

	public void AddAction(Interphase where, InterphaseAction action) => Interphase2List(where).Add(action);
	public void AddAction(InterphaseAction action) => Interphase2List(action.Where).Add(action);

	public void ExecuteActions(Interphase where, Gamestate state, Phase phase)
	{
		var actionList = Interphase2List(where);
		foreach (var action in actionList) action.Action(state, phase);
	}

	/// <summary>
	/// Remove all actions with matching names from all interphases
	/// </summary>
	/// <param name="actionName">The name of the action to remove</param>
	/// <returns>True if one or more actions were removed, otherwise false</returns>
	public bool DeleteAction(string actionName)
	{
		bool foundTarget = false;
		foreach (var num in Enum.GetValues<Interphase>())
		{
			var actionList = Interphase2List(num);
			foreach (var action in actionList)
			{
				if (action.Name == actionName)
				{
					actionList.Remove(action);
					foundTarget = true;
				}
			}
		}
		return foundTarget;
	}

	List<InterphaseAction> Interphase2List(Interphase iphase)
	{
		return iphase switch
		{
			Interphase.PreCommand => precommand,
			Interphase.PostCommand => postcommand,
			Interphase.PreMove => premove,
			Interphase.PostMove => postmove,
			Interphase.PreShoot => preshoot,
			Interphase.PostShoot => postshoot,
			Interphase.PreCharge => precharge,
			Interphase.PostCharge => postcharge,
			Interphase.PreFight => prefight,
			Interphase.PostFight => postfight,
			Interphase.Postgame => postgame,
			_ => null,
		};
	}

}