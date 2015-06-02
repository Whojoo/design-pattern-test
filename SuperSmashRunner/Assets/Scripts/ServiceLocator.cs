using UnityEngine;

public class ServiceLocator
{
	//LevelGenerator.
	private static ILevelGeneratorService levelService = new NullLevelGenerationService();
	public static ILevelGeneratorService GetLevelGenerator()
	{
		return levelService;
	}

	//Random service.
	private static IRandomService randomService = new NullRandomService();
	public static IRandomService GetRandomService()
	{
		return randomService;
	}

	//Providers, overloading ftw.
	public static void Provide(ILevelGeneratorService service)
	{
		levelService = service == null ? new NullLevelGenerationService() : service;
	}

	public static void Provide(IRandomService service)
	{
		randomService = service == null ? new NullRandomService() : service;
	}
}