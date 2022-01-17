using System;
using UnityEngine;
using System.Collections.Generic;

public class BaseAttack {
    public const float GRAV = 0.004f;
    public AnimationObject anim;


    public BaseAttack() {
        bounce_targets = new List<int>();
        DIR = new Vector3(0, 1.0f, 0.0f);
        _dir = new Vector3(0, 0, 0);
    }


    virtual public void init(PropAttack p, float posx, float posy, float posz) {
        props = p;
        anim = new AnimationObject();
        anim.init(p.clipname, new Vector3(posx, posy, posz));
        x = posx;
        y = posy;
        z = posz;
    }


    virtual public void setTarget(BaseMob mob) {
        target = mob;
        if (props.grav == 1) {
            anim.Y = 1;
            Vector2 distV = new Vector2(
                target.anim.transform.localPosition.x - anim.transform.localPosition.x,
                target.anim.transform.localPosition.z - anim.transform.localPosition.z
            );

            _dir = new Vector3(target.anim.transform.localPosition.x - anim.transform.localPosition.x, target.anim.transform.localPosition.z - anim.transform.localPosition.z);
            _dir.Normalize();

            float dist = distV.magnitude * 100;
            float p = props.speed;
            float angle = 0;
            float d = dist * GRAV / p / p / 2f;

            if (Mathf.Abs(d) > 1) {
                angle = Mathf.Asin(1) / 2;
            } else {
                angle = Mathf.PI * 0.5f - Mathf.Asin(d);
            }

            angle = Mathf.PI * 0.5f - Mathf.Asin(d);
            Vector2 vx = new Vector2(1, 0);
            vx = VectorUtils.rotate(vx, angle);
            _dir.z = vx.y * props.speed;
            start_dir_z = _dir.z;
            _dir.x = _dir.x * vx.x * props.speed;
            _dir.y = _dir.y * vx.x * props.speed;



        } else {
            float a = new Vector3(target.x - x, target.z - z).magnitude;
            float h = Math.Abs(target.y - y);
            anim.rotationX = 90.0f - (float)(Math.Atan(a / h) / Math.PI * 180.0f);
        }
    }


    public void setBounceTarget(BaseMob mob) {
        target = mob;
        if (props.grav == 1) {
            anim.Y = 1;
            Vector2 distV = new Vector2(
                target.anim.transform.localPosition.x - anim.transform.localPosition.x,
                target.anim.transform.localPosition.z - anim.transform.localPosition.z
            );

            _dir = new Vector3(target.anim.transform.localPosition.x - anim.transform.localPosition.x, target.anim.transform.localPosition.z - anim.transform.localPosition.z);
            _dir.Normalize();

            float dist = distV.magnitude * 100;
            Debug.Log("dist = " + dist.ToString());

            float p = props.speed;
            float angle = 0;
            float d = dist * GRAV / p / p / 2f;

            if (Mathf.Abs(d) > 1) {
                angle = Mathf.Asin(1) / 2;
            } else {
                angle = Mathf.PI * 0.5f - Mathf.Asin(d);
            }

            angle = Mathf.PI * 0.5f - Mathf.Asin(d);
            Vector2 vx = new Vector2(1, 0);
            vx = VectorUtils.rotate(vx, angle);
            _dir.z = vx.y * props.speed;
            start_dir_z = _dir.z;
            _dir.x = _dir.x * vx.x * props.speed;
            _dir.y = _dir.y * vx.x * props.speed;
        } else {

            anim.rotationX = 0;
        }
    }


    virtual public void update(int delta) {

            _dir.x = target.x  - x;
            _dir.y = target.z  - z;
            _dir.z = 0.0f;
            if (_dir.magnitude < delta * props.speed * 0.001f) {
                onHit();
                return;
            }
            _dir.Normalize();
            float r = VectorUtils.getAngle(DIR, _dir);
            anim.rotationY = r;

            x += _dir.x * delta * props.speed * 0.001f;
            z += _dir.y * delta * props.speed * 0.001f;

            if (target.anim.collider != null) {
                if (Math.Abs(target.y + target.anim.collider.center.y - y) > 0.02f) {
                    float a = new Vector3(target.x - x, target.z - z).magnitude;
                    float h = Math.Abs(target.anim.collider.center.y + target.y - y);
                    anim.rotationX = 90.0f - (float)(Math.Atan(a / h) / Math.PI * 180.0f);
                    Vector3 v = new Vector3(new Vector3(target.x - x, target.z - z).magnitude, target.anim.Y + 0.1f - anim.Y);
                    v.Normalize();
                    y += v.y * delta * props.speed * 0.001f;
                }
            } else {
                if (Math.Abs(target.y + 0.1f - y) > 0.02f) {
                    float a = new Vector3(target.x - x, target.z - z).magnitude;
                    float h = Math.Abs(target.y - y);
                    anim.rotationX = 90.0f - (float)(Math.Atan(a / h) / Math.PI * 180.0f);
                    Vector3 v = new Vector3(new Vector3(target.x - x, target.z - z).magnitude, target.anim.Y + 0.1f - anim.Y);
                    v.Normalize();
                    y += v.y * delta * props.speed * 0.001f;
                }
            }

        
    }


    virtual public void onHit() {
        _map.shootHit(this, target);
    }


    public float x {
        get { return _x; }
        set { _x = value; anim.X = _x; }
    }


    public float y {
        get { return _y; }
        set { _y = value; anim.Y = _y; }
    }


    public float z {
        get { return _z; }
        set { _z = value; anim.Z = _z; }
    }


    public Map map {
        set  {
            _map = value;
        }
    }


	public int ind;
	public int bounce;
	public float bounce_dmg;
	public float bounce_dist;
	public List<int> bounce_targets;

	public PropAttack props;
	public BaseMob target;
	protected Vector3 DIR;
	public Vector3 _dir;
	public float start_dir_z;

	protected float _x;
	protected float _y;
	protected float _z;

    private Map _map; 



	}


