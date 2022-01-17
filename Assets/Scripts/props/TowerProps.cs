using UnityEngine;
using System.Collections;

public class TowerProps : BaseProps
{


    public TowerProps() : base() {
        reload = 1000;
    }


    public override BaseProps clone() {
        TowerProps clon = new TowerProps();
        clon.reload = reload;
        clon.bullet_speed = bullet_speed;
        clon.bullet_anim = bullet_anim;
        clon.anim = anim;
        return clon;
    }


    public int reload = 1000;
    public float bullet_speed = 10f;
    public string bullet_anim;


}
