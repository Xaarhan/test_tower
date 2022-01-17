using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogger 
{
  
    public GameLogger() {
        _log = new List<ByteArray>();
    }


    public ByteArray getCurrent() {
        return _log[_log.Count - 1];
    }

    public void onSpawnMob( int time, BaseMob mob ) {
        ByteArray current = getCurrent();
        current.writeInt(time);
        current.writeInt(LogParams.UNIT_ADD);
        Vector3 position = mob.position;
        current.writeInt( mob.id );
        current.writeInt( (int)(position.x * 1000));
        current.writeInt( (int)(position.y * 1000));
        current.writeInt( (int)(position.z * 1000));
    }


    public void onAddAttack(int time, BaseAttack shoot) {
        ByteArray current = getCurrent();
        current.writeInt(time);
        current.writeInt(LogParams.SHOOT_ADD);
        current.writeInt(shoot.target.id);
        current.writeInt((int)(shoot.x * 1000));
        current.writeInt((int)(shoot.y * 1000));
        current.writeInt((int)(shoot.z * 1000));
    }



    public void Update() {
        
    }


    public void writeMap( ByteArray ar ) {
        _log.Add(ar);
    }

    
    public ByteArray getData( int time ) {
        int num = (int)(time / 10000);
        _lastData = _log[num];
        return (_lastData);
    }


    public void cut() {
        if (_lastData == null) return;
        int index = _log.IndexOf(_lastData);
        _log.RemoveRange(index, _log.Count - index);
        int cut_position = _lastData.position - 4; // тут получаем место отреза.
                                                   // но это я помню что оно устанавливается в Map когда считываем данные,
                                                   // поэтому по нормальному так делать совсем нельзя
        _lastData.position = 0;
        ByteArray cut = new ByteArray();
        cut.writeBytes(_lastData.getBytes(cut_position));
        _log.Add(cut);
        _lastData = null;
    }


    private ByteArray _lastData;
    private List<ByteArray> _log;

}
