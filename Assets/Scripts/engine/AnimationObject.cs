using System;
using UnityEngine;
using System.Collections.Generic;


	public class AnimationObject {


		private Transform[] _bones;
		private Matrix4x4[] _matricies;
		private Matrix4x4[] _bindposes;
		private static Dictionary<int, Mesh> cachedMeshes = new Dictionary<int, Mesh>();
		private Mesh currentMesh = null;
		private MeshFilter _meshFilter;
		private Material _mat;

		public AnimationObject () {
			_x = 0;
			_y = 0;
			_z = 0;
			_animspeed = 1.0f;
			nextAnimSpeed = 1;
		}


		public void init( string model_name, Vector3 pos ) {
			modelname = model_name;
			initModel( Main.shared.addGameObject("models/" + model_name, pos));
		}

		public void init( string model_name ) {
			modelname = model_name;
			initModel( Main.shared.addGameObject("models/" + model_name));
		}




		public void update() {
			if (_bones != null) {
				for (int i = 0; i < _bones.Length; i++) {
					_matricies[i] =  _bones[i].localToWorldMatrix;
				}
				_mat.SetMatrixArray( "_Bones" , _matricies);
			}
		}



		



		virtual public void initModel(GameObject mdl)  {
			model = mdl;
			transform = model.transform;
			_animator = model.GetComponent ("Animator") as Animator;
			if ( _animator != null) {
				 controller = _animator.runtimeAnimatorController;
			}
			renderer = model.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as Renderer;
			if (renderer == null) {
				renderer = model.GetComponentInChildren(typeof(MeshRenderer)) as Renderer;
			}

		
			_rotationY = transform.eulerAngles.y;
		}
	

		public float nextAnimSpeed;


		public void playAnim( string animname, bool forse = false, bool blending = false, float blend_time = 0.2f ) {
			if (_animator == null) return;
			_animator.speed = nextAnimSpeed;
			nextAnimSpeed = _animspeed;

			if ( forse == true ) {
				 if ( blending ) {
					  _animator.CrossFade(animname, blend_time, -1, 0.0f);
				 } else {
					  _animator.Play(animname, -1, 0.0f);
				 }
				 return;
			} 

			if (blending) {
				_animator.CrossFade(animname, 0.35f);
			} else {
				_animator.Play(animname);
			}
		}



		public void setAnimSpeed(string animname, float speed){
		/*	AnimationState state = _animator.runtimeAnimatorController.anima;
			String aname = state.clip.name;
			_animator.Play(animname, -1, 0.0f);
			_animator.Update(0.001f);
			state = _animator.GetCurrentAnimatorClipInfo(0)[0];

			state.clip.s  = 30.0f/speed;
			_animator.Play(aname, -1, 0.0f);*/
		}
			

		public void stop() {
			if (_animator == null) return;
			_animator.speed = 0.0f;
		}


		public void play() {
			if (_animator == null) return;
			_animator.speed = _animspeed;
		}


		public void setPosition(Vector3 pos){
			_x = pos.x;
			_y = pos.y;
			_z = pos.z;
			updatePosition();
		}


		public float X {
			get {return _x;}
			set { _x = value; updatePosition();}
		}


		public float Y {
			get {return _y;}
			set { _y = value; updatePosition();}
		}

		public float Z {
			get {return _z;}
			set { _z = value; updatePosition();}
		}


      

		public void updatePosition(){
			transform.localPosition = new Vector3(_x, _y, _z);
		}

		public void updateGlobalPosition(){
			transform.position = new Vector3(_x, _y, _z);
		}

		public void remove()  {
			if (model != null) {
				Main.shared.removeGameObject(model);
				model = null;
			}
		}


		public float rotationY {
			set {  _rotationY = value; transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, _rotationY, transform.localEulerAngles.z);}
			get { return _rotationY ;}
		}


		public float rotationZ {
			set {  _rotationZ = value; transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotationZ);}
			get { return _rotationZ ;}
		}


		public float rotationX {
			set {  _rotationX = value; transform.localEulerAngles = new Vector3(_rotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);}
			get { return _rotationX ;}
		}


		public  void addChild( GameObject child ) {
			child.transform.parent = model.transform;
			child.transform.localScale = new Vector3( model.transform.localScale.x * child.transform.localScale.x, 
			                                          model.transform.localScale.y * child.transform.localScale.y, 
			                                          model.transform.localScale.z * child.transform.localScale.z
		                                   			);
			child.transform.localPosition = new Vector3 (0, 0, 0);
		}


		public string getCurAnim(){
			if (_animator == null) {
				return "error";
			}
			if (_animator.GetComponent<Animation>() != null) {
				return _animator.GetComponent<Animation>().name;
			}
			return "none";
		}

		public Transform transform;



		public void addCollider(){
			if (collider == null) {
				collider = model.GetComponent(typeof(BoxCollider)) as BoxCollider;
				if (collider == null) {
					model.AddComponent (typeof(BoxCollider));
					collider = model.GetComponent(typeof(BoxCollider)) as BoxCollider;
					collider.size = renderer.bounds.size;
				}
				//collider.center = renderer.bounds.center;
				collider.isTrigger = true;
			}
		}


		public float AnimSpeed {
			set {
				if (_animator == null) return;
				_animspeed = value;
				_animator.speed = _animspeed;
			}

			get {
				return _animspeed;
			}
		}

		public bool isAnimPlaying(string name){
			bool result = false;
			if (_animator != null) {
				if (this._animator.GetCurrentAnimatorStateInfo (0).IsName (name)) {
						result = true;
				}
			}
			return result;
		}

		private float _animspeed;



		public BoxCollider collider;
		public RuntimeAnimatorController controller;
		private float _rotationZ;
		private float _rotationY;
		private float _rotationX;
		public Animator _animator;
		private float _x;
		private float _y;
		private float _z;
		public GameObject model;
		public string modelname;

		public Renderer renderer;
	}


