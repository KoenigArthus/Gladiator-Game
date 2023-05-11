using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class VariableStorageCustom : InMemoryVariableStorage
{
    float tempfloat;
    bool tempbool;
    string tempstring;
    public float PlayerCoins
    {
        get { this.TryGetValue("$player_coins", out tempfloat);
            return tempfloat;}
        set { this.SetValue("$player_coins", value); }
    }

    public float SecondCoins
    {
        get{ this.TryGetValue("$second_coins", out tempfloat);
            return tempfloat; }
        set{ this.SetValue("$second_coins", value); }
    }



}
