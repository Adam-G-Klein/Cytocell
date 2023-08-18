using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdScreenPlacement : MonoBehaviour {

    public static int[] topBannerPos(){
        return new int[]{(int)(Screen.safeArea.xMax + Screen.safeArea.xMin)/2, (int)Screen.safeArea.yMin};
    }
}
