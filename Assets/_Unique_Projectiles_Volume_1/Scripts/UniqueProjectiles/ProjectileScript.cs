using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

	public List<GameObject> trails;

    public bool rotate = false;
    public bool bounce = false;

    [Header("Bullet parameters")]
    public float damage;
    public float speed;
    public float fireRate;
    public float rotateAmount = 45;
    public float bounceForce = 10;
	[Tooltip("From 0% to 100%")] public float accuracy;
	
	[Header("References")]
	public GameObject muzzlePrefab;
	public GameObject hitPrefab;

	private bool _collided;
	
	private float _speedRandomness;

    private Vector3 _startPos;
	private Vector3 _offset;
	
	private Rigidbody _rigidbody;
    private RotateToMouseScript _rotateToMouse;
    private GameObject _target;

	void Start () {
        _startPos = transform.position;
        _rigidbody = GetComponent <Rigidbody> ();

		//used to create a radius for the accuracy and have a very unique randomness
		if (Mathf.Approximately(accuracy, 100f)) {
			accuracy = 1 - (accuracy / 100);

			for (int i = 0; i < 2; i++) {
				var val = 1 * Random.Range (-accuracy, accuracy);
				var index = Random.Range (0, 2);
				if (i == 0)
				{
					_offset = index == 0 ? new Vector3 (0, -val, 0) : new Vector3 (0, val, 0);
				} else
				{
					_offset = index == 0 ? new Vector3 (0, _offset.y, -val) : new Vector3 (0, _offset.y, val);
				}
			}
		}
			
		if (muzzlePrefab != null) {
			var muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
			muzzleVFX.transform.forward = gameObject.transform.forward + _offset;
			var ps = muzzleVFX.GetComponent<ParticleSystem>();
			if (ps != null)
				Destroy (muzzleVFX, ps.main.duration);
			else {
				var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX, psChild.main.duration);
			}
		}
	}

	void FixedUpdate () {
        if (_target is not null)
            _rotateToMouse.RotateToMouse(gameObject, _target.transform.position);
        if (rotate)
            transform.Rotate(0, 0, rotateAmount, Space.Self);
        if (speed != 0 && _rigidbody is not null)
			_rigidbody.position += (transform.forward + _offset) * (speed * Time.deltaTime);   
    }

	void OnCollisionEnter (Collision co)
	{
		
		if(co.gameObject.CompareTag("Player")) return;
		var hitBox = co.collider.GetComponent<EnemyHitBox>();
		Vector3 hitDirection = (co.transform.position - transform.position).normalized;
		if (hitBox && !_collided)
		{
			hitBox.OnWeaponDamage(this, hitDirection);
			_collided = true;
		}
        if (!bounce)
        {
            if (!co.gameObject.CompareTag("Bullet"))
            {
                _collided = true;

                if (trails.Count > 0)
                {
	                foreach (var t in trails)
	                {
		                t.transform.parent = null;
		                var ps = t.GetComponent<ParticleSystem>();
		                if (ps != null)
		                {
			                ps.Stop();
			                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
		                }
	                }
                }

                speed = 0;
                GetComponent<Rigidbody>().isKinematic = true;

                var contact = co.contacts[0];
                var rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                var pos = contact.point;

                if (hitPrefab != null && !co.gameObject.CompareTag($"Damageable"))
                {
                    var hitVFX = Instantiate(hitPrefab, pos, rot) as GameObject;

                    var ps = hitVFX.GetComponent<ParticleSystem>();
                    if (ps == null)
                    {
                        var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(hitVFX, psChild.main.duration);
                    }
                    else
                        Destroy(hitVFX, ps.main.duration);
                }

                StartCoroutine(DestroyParticle(0f));
            }
        }
        else
        {
            _rigidbody.useGravity = true;
            _rigidbody.drag = 0.5f;
            var contact = co.contacts[0];
            _rigidbody.AddForce (Vector3.Reflect((contact.point - _startPos).normalized, contact.normal) * bounceForce, ForceMode.Impulse);
            Destroy ( this );
        }
	}

	public IEnumerator DestroyParticle (float waitTime) {

		if (transform.childCount > 0 && waitTime != 0) {
			List<Transform> tList = transform.GetChild(0).transform.Cast<Transform>().ToList();

			while (transform.GetChild(0).localScale.x > 0) {
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				foreach (var t in tList)
				{
					t.localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}
		
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}

    public void SetTarget (GameObject trg, RotateToMouseScript rotateTo)
    {
        _target = trg;
        _rotateToMouse = rotateTo;
    }
}
