using System;

namespace LibHammer.Deployments;

public enum Deployment
{
    Search_and_Destroy,
    Dawn_of_War,
    Sweeping_Engagement,
    Crucible_of_Battle,
    Hammer_and_Anvil
}

static class DeploymentManager
{
    public static Deployment RollDeployment()
    {
        Random rng = new();
        int len = 5;
        return (Deployment)rng.Next(0, len);
    }
}