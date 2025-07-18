using System;
using LibHammer.Gamestate.InterphaseActions;
using LibHammer.Gamestate.Serviceproviders;

namespace LibHammer.Gamestate;

public class PhaseManager
{
	public Gamestate State;
	public Phase CurrentPhase = Phase.Pregame;

	public int Turn = 1;

	public Player Player1;
	public Player Player2;
	public Player ActivePlayer;
	public InterphaseManager CommonActions = new();


	public PhaseManager(Player player1, Player player2, Gamestate state)
	{
		Player1 = player1;
		ActivePlayer = player1;
		Player2 = player2;
		State = state;

		// Managing pre command actions
		InterphaseAction preCmd = new InterphaseAction
		{
			Name = "Command Phase Stuff",
			Where = Interphase.PreCommand,
			Action = (state, phase) =>
		{
			state.Player1.CP++;
			state.Player2.CP++;

			// Removing last turns battle shock
			foreach (BoardTroop troop in state.PhaseMan.ActivePlayer.Army) troop.BattleShocked = false;

			// Recalculating new battle shock
			foreach (BoardTroop troop in state.PhaseMan.ActivePlayer.Army)
			{
				if (troop.DamageTaken <= troop.Stats.Wounds / 2)
				{
					int bsRoll = Helpers.rollxDy(2, 6);
					if (bsRoll < troop.Stats.Leadership) troop.BattleShocked = true;
				}
			}
		}
		};

		CommonActions.AddAction(preCmd);

		// Managing pre movement step actions
		InterphaseAction preMove = new InterphaseAction
		{
			Name = "Movement Phase Stuff",
			Where = Interphase.PreMove,
			Action = (state, phase) =>
		{
			// Reseting last turns move status
			foreach (BoardTroop troop in state.PhaseMan.ActivePlayer.Army)
			{
				troop.MoveState = MoveStatus.Pending;
				troop.AvailableMovement = troop.Stats.Movement;
			}
		}
		};

		CommonActions.AddAction(preMove);

		// Managing pre movement step actions
		InterphaseAction postMove = new InterphaseAction
		{
			Name = "After the movement phase",
			Where = Interphase.PostMove,
			Action = (state, phase) =>
		{
			// Reseting last turns move status
			foreach (BoardTroop troop in state.PhaseMan.ActivePlayer.Army)
			{
				if (troop.MoveState == MoveStatus.Pending) troop.MoveState = MoveStatus.Stationary;
			}
		}
		};

		CommonActions.AddAction(postMove);


		// Managing pre shooting phase actions
		InterphaseAction preShooting = new InterphaseAction
		{
			Name = "Shooting Phase Prep",
			Where = Interphase.PreShoot,
			Action = (state, phase) =>
		{
			// Determining eligibility of all troops to fire
			foreach (BoardTroop troop in state.PhaseMan.ActivePlayer.Army)
			{
				troop.ShootingTargets.Clear();

				var engagements = state.EngagementProvider.FindEngagements(troop);
				troop.ShootingEligible = engagements.Count == 0 && troop.MoveState != MoveStatus.Advanced;
				if (troop.ShootingEligible)
				{
					foreach (BoardTroop enemy in ((ActivePlayer == Player1) ? Player2 : Player1).Army)
					{
						ViewClearance view = State.ViewClearanceProvider.DetermineVisibility(troop, enemy);
						if (view != ViewClearance.Obstructed) troop.ShootingTargets.Add(enemy);
					}
				}
			}
		}
		};

		CommonActions.AddAction(preShooting);

		// Managing pre charge phase actions
		InterphaseAction preCharge = new InterphaseAction
		{
			Name = "Charge Phase Prep",
			Where = Interphase.PreCharge,
			Action = (state, phase) =>
		{
			// Determining eligibility of all troops to charge (unengaged, unadvanced, within 12m of enemy)
			foreach (BoardTroop troop in state.PhaseMan.ActivePlayer.Army)
			{
				troop.ChargeTargets.Clear();

				var engagements = state.EngagementProvider.FindEngagements(troop);
				troop.ChargingEligible = engagements.Count == 0 && troop.MoveState != MoveStatus.Advanced;
				if (troop.ChargingEligible)
				{
					foreach (BoardTroop enemy in ((ActivePlayer == Player1) ? Player2 : Player1).Army)
					{
						float distance = State.DistanceProvider.Measure(troop, enemy);
						if (distance <= 12) troop.ChargeTargets.Add(enemy);
					}
				}
			}
		}
		};

		CommonActions.AddAction(preCharge);
	}

	public void NextPhase()
	{
		switch (CurrentPhase)
		{
			case Phase.Pregame:
				CurrentPhase = Phase.Command;
				FireInterphase(Interphase.PreCommand);
				break;

			case Phase.Command:
				FireInterphase(Interphase.PostCommand);
				CurrentPhase = Phase.Move;

				// Reseting the movement status of all troops


				FireInterphase(Interphase.PreMove);
				break;

			case Phase.Move:
				FireInterphase(Interphase.PostMove);
				CurrentPhase = Phase.Shoot;
				FireInterphase(Interphase.PreShoot);
				break;

			case Phase.Shoot:
				FireInterphase(Interphase.PostShoot);
				CurrentPhase = Phase.Charge;
				FireInterphase(Interphase.PreCharge);
				break;

			case Phase.Charge:
				FireInterphase(Interphase.PostCharge);
				CurrentPhase = Phase.Fight;
				FireInterphase(Interphase.PreFight);
				break;

			case Phase.Fight:
				FireInterphase(Interphase.PostFight);

				// Check if game is over
				if (Turn == 5)
				{
					CurrentPhase = Phase.Postgame;

					ActivePlayer = Player1;
					FireInterphase(Interphase.Postgame);

					ActivePlayer = Player2;
					FireInterphase(Interphase.Postgame);
					break;
				}

				// Otherwise pass the turn
				else
				{
					// Swap the active player and increment the turn counter if p1 is back
					if (ActivePlayer == Player1)
					{
						ActivePlayer = Player2;
					}
					else
					{
						ActivePlayer = Player1;
						Turn++;
					}

					// Move to the command phase and fire the new active players precommand actions
					CurrentPhase = Phase.Command;
					FireInterphase(Interphase.PreCommand);
					break;
				}




		}
	}

	public Phase GetNextPhase() => CurrentPhase switch
	{
		Phase.Pregame => Phase.Command,
		Phase.Command => Phase.Move,
		Phase.Move => Phase.Shoot,
		Phase.Shoot => Phase.Charge,
		Phase.Charge => Phase.Fight,
		Phase.Fight => (Turn < 5) ? Phase.Command : Phase.Postgame,
		Phase.Postgame => Phase.Postgame,
		_ => throw new ArgumentOutOfRangeException()
	};


	void FireInterphase(Interphase iphase)
	{
		CommonActions.ExecuteActions(iphase, State, CurrentPhase);
		ActivePlayer.Interphase.ExecuteActions(iphase, State, CurrentPhase);
	}
}
