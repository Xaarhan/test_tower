
using System;

	public class PropAttack {
		
		public  const int RANGE_ATTACK = 0;
		public  const int MELEE_ATTACK = 1;
		public  const int INSTANT_ATTACK = 2;
		public  const int AROUND_ATTACK = 4;

		
		public  const int DMG_PIERCE = 0;
		public  const int DMG_NORMAL = 1;
		public  const int DMG_MAGIC = 2;
		public  const int DMG_SIEGE = 3;
		public  const int DMG_SPELL = 4;

		public  PropAttack( string clip   = "arrow", string bangg  = "effects/eff_hit1", string sshoot = "shoot1", string sdhit = "hit1" ) {			
			clipname = clip;
			bang = bangg;
			soundshoot = sshoot;
			_damage_mult = 1.0f;
			soundhit = sdhit;
			attack_type = MELEE_ATTACK;
			grav = 0;
		}


		public PropAttack clone() {
			PropAttack clon = new PropAttack(clipname, bang, soundshoot, soundhit);
			clon._damage = _damage;
			clon._damage_mult = _damage_mult;
			clon.dmg_type = dmg_type;
			clon._incdmg_max = _incdmg_max;
			clon._incdmg_min = _incdmg_min;
			clon.area = area;
			clon.speed = speed;
			clon._maxdamage = _maxdamage;
			clon.attack_type = attack_type;
			clon.dist = dist;
			clon.defdist = defdist;
			clon.bang = bang;
			clon.grav = grav;
			clon.bounce_dist = bounce_dist;
			clon.instantFirstHit = instantFirstHit;
			return clon;
		}


		public float damage {
			get {
				return _damage + ( _maxdamage + _damage) / 2.0f * ( _damage_mult - 1.0f ) + _incdmg_min;
			}
		}


		public float maxdamage {
			get {
				return _maxdamage + ( _damage + _maxdamage )  / 2.0f  * ( _damage_mult - 1.0f ) + _incdmg_max;
			}
		}


		public float _damage_mult;


		public int grav;
		public int attack_type = 0;
		public float _damage  = 10;
		public int dmg_type = 1;
		public float _maxdamage = 10;

		public float _incdmg_min = 0;
		public float _incdmg_max = 0;

		public int area;
		public float speed  = 10;
		public int dist;
		public int defdist;
		public string clipname;
		public string bang;
		public float bounce_dist;
		public bool instantFirstHit;

		public string soundshoot;
		public string soundhit;


		}


