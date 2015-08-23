using UnityEngine;
using System.Collections;
using UniRx;

public class GameplayController {

    private static GameplayController _instance;

    public static GameplayController instance {
        get { return _instance ?? ( _instance = new GameplayController() ); }
    }

    public IntReactiveProperty dangerLevel = new IntReactiveProperty( 0 );

}