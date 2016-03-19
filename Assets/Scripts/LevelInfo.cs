using UnityEngine;
using System.Collections;

public class LevelInfo {

    private static LevelInfo INSTANCE = new LevelInfo();
    public string currentLevel = "1. Welcome";
    public Planet planet;


    private LevelInfo(){}

    public override string ToString() {
        return "Current leve: " + currentLevel + ". Planet: " + planet + "." + "Version: " + GetHashCode();
    }

    public static LevelInfo getInstance() {
        return INSTANCE;
    }
}
