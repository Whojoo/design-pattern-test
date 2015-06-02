using UnityEngine;
using System;

public interface ILevelGeneratorService
{
    void GenerateLevel();
    void EmptyLevel();
    int GetWidth();
}