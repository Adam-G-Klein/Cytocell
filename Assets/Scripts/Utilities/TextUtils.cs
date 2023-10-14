using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUtils {
    public static string intWithCommas(int num){
        string numStr = num.ToString();
        string newStr = "";
        for(int i = 0; i < numStr.Length; i++){
            if(i % 3 == 0 && i != 0){
                newStr = "," + newStr;
            }
            newStr = numStr[numStr.Length - 1 - i] + newStr;
        }
        return newStr;
    }
}
