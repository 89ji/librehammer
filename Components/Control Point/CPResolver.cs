using LibHammer.ControlPoint;

class CPResolver : IControlPointResolver
{
	ControlPoint cap;

	public CPResolver(ControlPoint cap)
	{
		this.cap = cap;
	}

	public ObjectiveInfluence ResolveInfluences()
	{
		return cap.CalculateInfluence();
	}
}