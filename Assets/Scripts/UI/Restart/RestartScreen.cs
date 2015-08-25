using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RestartScreen : UIScreen {

    [SerializeField]
    private Button _restartButton;

    private void Start() {

        _restartButton.onClick.AddListener( OnRestartClick );
    }

    private void OnRestartClick() {

        foreach ( var each in Character.instances ) {
            
            each.Dispose();
        }

        GameplayController.instance.dangerLevel.Value = 0;

        Character.instances.Clear();

        Application.LoadLevel( Application.loadedLevel );
    }

}