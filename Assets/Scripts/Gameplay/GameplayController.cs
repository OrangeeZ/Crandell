﻿using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class GameplayController {

    private static GameplayController _instance;

    public static GameplayController instance {
        get { return _instance ?? ( _instance = new GameplayController() ); }
    }

    public IntReactiveProperty dangerLevel = new IntReactiveProperty( 0 );

    public static int maxDanger = 100;

    public GameplayController() {

        dangerLevel.Subscribe( OnDangerLevel );
    }

    private void OnDangerLevel( int i ) {

        Debug.Log( i );
    }

}