using System;

// Todas las clases para poder convertir en objetos el formato JSON
public class SimulationResult
{
	public int stepCount;
	public Step[] steps;
	public Grid grid;
}

[Serializable]
public class Step
{
	public int index;
	public Agent[] robots;
	public Agent[] boxes;
	public ShelfAgent[] shelves;
}

[Serializable]
public class Grid
{
	public int width;
	public int height;
}

[Serializable]
public class Agent
{
	public int id;
	public Position pos;
}

[Serializable]
public class ShelfAgent
{
	public int id;
	public Position pos;
	public int stored;
}

[Serializable]
public class Position
{
	public int x;
	public int y;
	public int z;
}