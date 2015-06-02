public class NullRandomService : IRandomService
{
	public System.Random GetRandomizer() { return new System.Random(); }
}