using UnityEngine;

public class BaseProps {

    public BaseProps() {
        speed = 0.0025f;
        max_lifes = 100;
        lifes = 100;
        anim = "mob1";
    }


    virtual public BaseProps clone() {
        BaseProps clon = new BaseProps();
        clon.speed = speed;
        clon.lifes = lifes;
        clon.max_lifes = max_lifes;
        clon.anim = anim;
        return clon;
    }

    public float speed;
    public float lifes;
    public float max_lifes;
    public string anim;


}
