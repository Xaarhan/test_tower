using UnityEngine;

public class BaseMob : BaseObject {


    public BaseMob() {
        _dir = new Vector3(-1, 0, 0);
    }


    override public void init( BaseProps prop ) {
        base.init(prop);
        _props = prop;
        initAnim( Main.shared.addGameObject( "models/" + _props.anim) );
    }


    virtual public void initAnim( GameObject model ) {
        if ( anim != null ) {
             removeAnim();
        }
        anim = new AnimationObject();
        anim.initModel(model);
        anim.addCollider();
    }


    override public void Update( int delta ) {
        base.Update(delta);
        Vector3 v = new Vector3();
        setPosition(position + _dir * delta * _props.speed);
    }


    public override void setPosition(Vector3 v) {
        base.setPosition(v);
        anim.setPosition(v);
    }


    virtual public void removeAnim() {
        Main.shared.removeGameObject(anim.model);
        anim = null;
    }


    public override void lookTo(Vector3 point) {
        base.lookTo(point);
        Vector2 dir = new Vector2(point.x - x, point.z - z);
        if (dir.magnitude < 0.03f) return;
        float angle = VectorUtils.getAngle(_start_dir, dir);
        anim.rotationY = angle;
        // Debug.Log(angle);
    }


    public override float x {
        get { return _x; }
        set { _x = value; anim.X = _x; }
    }


    public override float y {
        get { return _y; }
        set { _y = value; anim.Y = _y; }
    }


    public override float z {
        get { return _z; }
        set { _z = value; anim.Z = _z; }
    }


    public bool isDead {
        get {
            return _isDead;
        }
    }


    public int addDamage( int damage  ) {
        _props.lifes -= damage;
        if (_props.lifes <= 0) {
            Dead();
        }


        return damage;
    }



    public void Dead() {
        _isDead = true;
    }


    public BaseProps props {
        get {
            return _props;
        }
    }


    public AnimationObject anim;
    public int id;
    private bool _isDead;
    protected BaseProps _props;

}
