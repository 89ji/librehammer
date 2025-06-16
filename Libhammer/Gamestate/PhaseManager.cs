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

        player1.Interphase.AddAction(preCmd);
        player2.Interphase.AddAction(preCmd);

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
                troop.AvailableMovement = 0;
            }
        }
        };

        player1.Interphase.AddAction(preMove);
        player2.Interphase.AddAction(preMove);


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

        player1.Interphase.AddAction(preShooting);
        player2.Interphase.AddAction(preShooting);

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

        player1.Interphase.AddAction(preCharge);
        player2.Interphase.AddAction(preCharge);
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


    void FireInterphase(Interphase iphase)
    {
        ActivePlayer.Interphase.ExecuteActions(iphase, State, CurrentPhase);
    }
}