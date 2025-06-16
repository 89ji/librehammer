using System;

namespace LibHammer.ControlPoint;

public class Controlpoint
{
    public IControlPointResolver? Resolver;

    public ObjectiveInfluence GetInfluence()
    {
        if (Resolver == null) throw new Exception("Control point resolver not set!!");
        return Resolver.ResolveInfluences();
    }
}