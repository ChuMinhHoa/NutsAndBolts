using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProfileManager : Singleton<ProfileManager>
{
    public VersionStatus versionStatus;
    public PlayerData playerData;
    public DataConfig dataConfig;
    protected override void Awake()
    {
        base.Awake();
        playerData.LoadData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
        playerData.Update();
    }

    public bool IsShowCheat()
    {
        return (versionStatus == VersionStatus.Cheat);
    }

    public bool IsShowDebug()
    {
        return (versionStatus is VersionStatus.Cheat or VersionStatus.NoCheat);
    }

    public bool IsMusicOn()
    {
        return true;
    }

    public bool IsSoundOn()
    {
        return true;
    }
}
