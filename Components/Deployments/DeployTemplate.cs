using Godot;
using LibHammer.Deployments;
using System;

public partial class DeployTemplate : Node3D
{
    [Export] PackedScene DawnOfWar;
    [Export] PackedScene HammerAndAnvil;
    [Export] PackedScene SweepingEngagement;

    public override void _Ready()
    {
        Hide();
    }

    public PackedScene LoadDeployment(Deployment deployment)
    {
        switch (deployment)
        {
            case Deployment.Dawn_of_War:
                return DawnOfWar;
            case Deployment.Hammer_and_Anvil:
                return HammerAndAnvil;
            case Deployment.Sweeping_Engagement:
                return SweepingEngagement;
            default:
                return SweepingEngagement;
        }
    } 
}
