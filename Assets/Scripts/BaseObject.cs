using UnityEngine;

public class BaseObject
{


    public BaseObject() {
        _start_dir = new Vector3(0,1,0);
    }


    virtual public void init(BaseProps prop) {
        props = prop;
    }


    virtual public void setDirection(Vector3 dir) {
        _dir = dir;
    }




    virtual public void lookTo(Vector3 point) {
           
    }



    virtual public void rotateTo( Vector3 dir ) {

    }

   
    virtual public void Update(int delta) {
     
    }

    virtual public void setPosition ( Vector3 v ) {
        _x = v.x;
        _y = v.y;
        _z = v.z;
    }

    public Vector3 position {
        get {
            return new Vector3( _x, _y, _z );
        }

    }


    public virtual float x {
        get { return _x; }
        set { _x = value;}
    }


    public virtual float y {
        get { return _y; }
        set { _y = value; }
    }


    public virtual float z {
        get { return _z; }
        set { _z = value; }
    }


    public virtual Vector3 direction {
        get {
            return _dir;
        }
    }



    protected float _x;
    protected float _y;
    protected float _z;


    protected Vector3 _dir;
    protected Vector3 _start_dir;
    protected BaseProps props;

}
