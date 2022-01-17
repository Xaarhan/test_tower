using UnityEngine;

public class BaseTower : BaseMob
{

    override public void Update(int delta) {
        if ( target != null ) {
             lookTo(target.position);
             if (target.isDead) {
                target = null;
             }
        }
        reload -= Main.delta;
        wait_target -= Main.delta;
    }



    public override void init(BaseProps props) {
        base.init(props);
    }


    override public void initAnim(GameObject model) {
        base.initAnim(model);
        _helper = model.GetComponentInChildren<TowerAnimHelper>();
        if ( _helper != null ) {
             _start_dir = _helper.start_dir;
        }
    }


    public override void setDirection(Vector3 dir) {
        base.setDirection(dir);
    }


    

    public void onShoot() {
        reload = (_props as TowerProps).reload;
    }


    public Vector3 shoot_point {
        get {
            if (_helper == null) return anim.transform.position;
            return _helper.shoot_point.position;
        }

    }


    



    private TowerAnimHelper _helper;
    public BaseMob target;
    public int wait_target;
    public int reload;



}
