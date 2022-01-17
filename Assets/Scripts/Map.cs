using UnityEngine;
using System.Collections.Generic;

public class Map {

    public Map() {
        _mobsid = 0;
        _towersid = 0;
        _log = new GameLogger();
        

        _ground_mask = (1 << 6);
        _mob_mask = (1 << 15);
        _mobs = new List<BaseMob>();
        _towers = new List<BaseTower>();
        _attacks = new List<BaseAttack>();
        _gametime = 0;
        _old_gametime = 0;

        BaseTower tower = new BaseTower();
        tower.init(new TowerProps());
        tower.initAnim(Main.shared.towerAnim);
        tower.setPosition(Main.shared.towerAnim.transform.position);
        addTower(tower);

        _finish_rect = Main.shared.finish;
        _log.writeMap(getByteMap());
        _pause = false;
    }


    public int delta = 10;

    public void Update( int timedelta = 10, bool log = false) {
        if (!log && pause) return;

        delta = timedelta;
        if ( !log) _gametime += delta;
        for ( int i = 0; i < _mobs.Count; i++ ) {
              updateMob( _mobs[i] );
        }

        if ( !log) {
             for (int i = 0; i < _towers.Count; i++) {
                  updateTower(_towers[i]);
             }
        }
      
        for (int i = 0; i < _attacks.Count; i++) {
              updateAttack(_attacks[i]);
        }

        if ( Input.GetMouseButtonDown(0) ) {
             onMouseDown();
        }

        if ( _gametime > _old_gametime + 10000) {
             _old_gametime = _gametime;
             _log.writeMap(getByteMap());
        }

    }



    private void updateMob( BaseMob mob ) {
        mob.Update(delta);

        if (_finish_rect.bounds.Contains(mob.position)) {
            mob.Dead();
        }

        if (mob.isDead) {
            removeMob(mob);
        }
    }


    private void updateAttack(BaseAttack shoot) {
        shoot.update( delta );
    }


    private void updateTower(BaseTower tower) {
        tower.Update(delta);
        if ( tower.target == null && tower.wait_target <= 0 ) {
             tower.wait_target = 1000;
             tower.target = towerFindTarger(tower);
        }

        if ( tower.target != null && tower.reload <= 0  ) {
             towerShoot(tower, tower.target);
             tower.reload = (tower.props as TowerProps).reload;
        }

    }


    private BaseMob towerFindTarger( BaseTower tower ) {
        float sqr_dist = float.MaxValue;
        Vector3 tower_pos = tower.position;
        BaseMob target = null;
        for ( int i = 0; i < _mobs.Count; i++ ) {
              BaseMob mob = _mobs[i];
              float dist = (mob.position - tower_pos).sqrMagnitude;
              if ( dist < sqr_dist  ) {
                   target = mob;
                   sqr_dist = dist;
              }
        }
        return target;
    }



    private void towerShoot( BaseTower tower, BaseMob target ) {
        BaseAttack shoot = new BaseAttack();
        PropAttack props = new PropAttack("shoot1");
        props.speed = (tower.props as TowerProps).bullet_speed;
        Vector3 shootPoint = tower.shoot_point;
        shoot.init(props, shootPoint.x, shootPoint.y, shootPoint.z);
        shoot.setTarget(target);
        addAttack(shoot);
    }


    private void createAttack() {

    }



    private void onMouseDown() {
        Ray ray = Main.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitOut;
        Physics.Raycast(ray.origin, ray.direction, out hitOut, 3000, _ground_mask);
        if ( hitOut.transform != null && hitOut.transform.name.IndexOf("ground") != -1 ) {
             spawnMobAt( hitOut.point, _mobsid++ );
             return;
        }
    }


    private BaseMob spawnMobAt( Vector3 pos, int id ) {
        BaseMob mob = new BaseMob();
        mob.init(new BaseProps());
        mob.id = id;
        mob.setPosition(pos);
        addMob(mob);
        return mob;
    }


  


    protected void addMob( BaseMob mob ) {
        _mobs.Add(mob);
        mob.lookTo(mob.position + mob.direction);
        Debug.Log(mob.direction);
        if (!_inLog) {
            _log.onSpawnMob(_gametime, mob);
        }
    }


    protected void removeMob(BaseMob mob) {
        _mobs.Remove(mob);
        Main.shared.removeGameObject(mob.anim.model);
        // тут конечно обязательно использовать пул для моделей
        // и при перемотке создавать их из пула
    }


    public void addTower( BaseTower tower ) {
        _towers.Add(tower);
    }



    public void createTower( BaseProps props, GameObject anim ) {
        BaseTower tower = new BaseTower();
    }


    public void addAttack( BaseAttack attack ) {
        attack.map = this;
        _attacks.Add(attack);
        if (!_inLog) {
            _log.onAddAttack(_gametime, attack);
        }
    }


    public void removeAttack( BaseAttack attack ) {
        _attacks.Remove(attack);
        Main.shared.removeGameObject(attack.anim.model);
        // тут конечно обязательно использовать пул для моделей
    }


    public void removeAllAttack() {
        while ( _attacks.Count > 0 ) {
                removeAttack(_attacks[0]);
        }
       
    }


    public void shootHit( BaseAttack attack, BaseMob target ) {
        target.addDamage((int)attack.props.damage);
        removeAttack(attack);
    }


    public ByteArray getByteMap() {

        // это мне следовало бы убрать и использовать onSpawnMob onAddAttack в loggere
        // для всех мобов и снарядов

        ByteArray ar = new ByteArray();
        ar.writeInt(LogParams.MAP_DATA);
        ar.writeInt(_gametime);
        ar.writeInt(_mobs.Count);
        for ( int i = 0; i < _mobs.Count; i++ ) {
              BaseMob mob = _mobs[i];
              ar.writeInt(mob.id);
              ar.writeInt((int)mob.props.lifes);
              ar.writeInt((int)(mob.x * 1000f));
              ar.writeInt((int)(mob.y * 1000f));
              ar.writeInt((int)(mob.z * 1000f));
        }

        ar.writeInt(_attacks.Count);
        for (int i = 0; i < _attacks.Count; i++) {
             BaseAttack attack = _attacks[i];
             ar.writeInt(attack.target.id);
             ar.writeInt((int)(attack.x * 1000f));
             ar.writeInt((int)(attack.y * 1000f));
             ar.writeInt((int)(attack.z * 1000f));
        }

        return ar;
    }


    
    public void loadFromLog( int time ) {
        _inLog = true;
        _logtime = time;
        ByteArray data = _log.getData(time);
        data.position = 0;
        data.getInt();
        int datatime = data.getInt();
        int timedist = time - datatime;
        removeAllMobs();
        removeAllAttack();

        int mob_count = data.getInt();
        for ( int i = 0; i < mob_count; i++ ) {
              int id = data.getInt();
              int lifes = data.getInt();
              Vector3 pos = new Vector3 ( data.getInt() / 1000f, data.getInt() / 1000f, data.getInt() / 1000f );
              BaseMob mob = spawnMobAt( pos, id );
              mob.props.lifes = lifes;
        }

        int attacks_count = data.getInt();
        for (int i = 0; i < attacks_count; i++) {
            int target_id = data.getInt();
            Vector3 pos = new Vector3(data.getInt() / 1000f, data.getInt() / 1000f, data.getInt() / 1000f);
           
            BaseMob target = getMob(target_id);
            if ( target != null ) {
                 BaseAttack shoot = new BaseAttack();
                 PropAttack props = new PropAttack("shoot1");
                 props.speed = (_towers[0].props as TowerProps).bullet_speed;
                 shoot.init(props, pos.x, pos.y, pos.z);
                 shoot.setTarget(target);
            }
        }




        int current_time = datatime;
        bool stop = data.position < data.length ? false : true;
        while (!stop) {
            int nexttime = data.getInt();            
            if ( nexttime > time ) {
                 break;
            }
            updateLogTime(nexttime - current_time);
            current_time = nexttime;
            int command_type = data.getInt();

            // мне следовало бы сделать "команды" отдельными файлами
            // и уже в них прописать парсинг

            switch (command_type) {
                case LogParams.UNIT_ADD: {
                    int id = data.getInt();
                    spawnMobAt(new Vector3(data.getInt() / 1000f, data.getInt() / 1000f, data.getInt() / 1000f), id );
                    break;
                }
                case LogParams.SHOOT_ADD: {
                    int target_id = data.getInt();
                    Vector3 pos = new Vector3(data.getInt() / 1000f, data.getInt() / 1000f, data.getInt() / 1000f);

                    BaseMob target = getMob(target_id);
                    if (target != null) {
                        BaseAttack shoot = new BaseAttack();
                        PropAttack props = new PropAttack("shoot1");
                        props.speed = (_towers[0].props as TowerProps).bullet_speed;
                        shoot.init(props, pos.x, pos.y, pos.z);
                        shoot.setTarget(target);
                        addAttack(shoot);
                    }
                    break;
                }

            }
            stop = data.position < data.length ? false : true;
        }

        updateLogTime(time - current_time);
        _inLog = false;
    }

    private int _logtime;

    private void updateLogTime(int timedist) {
        int timestep = 100;
        while (timedist > timestep) {
            Update(timestep, true);
            timedist -= timestep;
        }
        if ( timedist > 0 ) Update(timedist, true);
    }


    public void setTimePersent( float persent ) {
        int time = (int)(_gametime * persent);
        loadFromLog(time);
    }




    public void removeAllMobs() {
        while ( _mobs.Count > 0 ) {
            removeMob(_mobs[0]);
        }
    }


    public List<BaseMob> mobs {
        get {
            return mobs;
        }
    }

    public BaseMob getMob( int id ) {
        for ( int i = 0; i < _mobs.Count; i++ ) {
            if ( _mobs[i].id == id ) {
                return _mobs[i];
            }
        }
        return null;
    }



    public bool pause {
        get {
            return _pause;
        }
        set {
            _pause = value;
            if ( !_pause ) { // убираем паузу и обрезаем остатки лога
                 _log.cut();
                 _gametime = _logtime;
            }
        }
    }

    private bool _pause;


    protected GameLogger _log;

    protected List<BaseMob> _mobs;
    protected List<BaseTower> _towers;
    protected List<BaseAttack> _attacks;
    private LayerMask _ground_mask;
    private LayerMask _mob_mask;
    private BoxCollider _finish_rect;
    private int _gametime;
    private int _old_gametime;
    private int _mobsid;
    private int _towersid;
    private bool _inLog;


}
