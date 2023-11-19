using Settings;
using UnityEngine;
using Photon;
using Characters;
using System.Collections.Generic;
using System.Collections;
using Utility;
using Photon.Pun;

namespace Projectiles
{
    class BaseProjectile: BaseMovementSync
    {
        protected BaseCharacter _owner;
        protected float _timeLeft;
        protected string _team;
        protected Vector3 _velocity;
        protected List<GameObject> _hideObjects = new List<GameObject>();
        protected List<Collider> _colliders = new List<Collider>();
        protected List<ParticleSystem> _fadeTrails = new List<ParticleSystem>();
        protected virtual float TrailFadeMultiplier => 0.6f;
        protected virtual float DestroyDelay => 1.5f;
        protected ConstantForce _force;

        public virtual void Setup(float liveTime, Vector3 velocity, Vector3 gravity, int charViewId, string team, object[] settings)
        {
            _timeLeft = liveTime;
            _rigidbody.velocity = velocity;
            _team = team;
            _velocity = velocity;
            if (gravity != Vector3.zero)
            {
                _force = gameObject.AddComponent<ConstantForce>();
                _force.force = gravity;
            }
            photonView.RPC("SetupRPC", RpcTarget.All, new object[] { charViewId, settings });
        }

        [PunRPC]
        public virtual void SetupRPC(int charViewId, object[] settings, PhotonMessageInfo info)
        {
            if (info.Sender != photonView.Owner)
                return;
            if (charViewId != -1)
            {
                _owner = PhotonView.Find(charViewId).GetComponent<BaseCharacter>();
                _team = _owner.Team;
            }
            SetupSettings(settings);
            RegisterObjects();
            RegisterColliders();
            if (_owner != null)
            {
                foreach (Collider c1 in _owner.Cache.Colliders)
                {
                    foreach (Collider c2 in _colliders)
                    {
                        if (c1.enabled && c2.enabled)
                            Physics.IgnoreCollision(c1, c2);
                    }
                }
            }
        }

        protected virtual void SetupSettings(object[] settings)
        {
        }

        protected void RegisterColliders()
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                c.gameObject.layer = PhysicsLayer.Projectile;
                _colliders.Add(c);
            }
        }

        public bool IsMine()
        {
            return photonView.IsMine;
        }

        protected override void Update()
        {
            base.Update();
            if (_photonView.IsMine)
            {
                _timeLeft -= Time.deltaTime;
                if (_timeLeft <= 0f)
                    OnExceedLiveTime();
            }
        }

        protected virtual void RegisterObjects()
        {
        }

        protected virtual void OnExceedLiveTime()
        {
            DestroySelf();
        }

        public virtual void DestroySelf()
        {
            if (photonView.IsMine && !Disabled)
            {
                photonView.RPC("DisableRPC", RpcTarget.All, new object[0]);
                StartCoroutine(WaitAndFinishDestroyCoroutine(DestroyDelay));
            }
        }

        protected virtual IEnumerator WaitAndFinishDestroyCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            PhotonNetwork.Destroy(gameObject);
        }

        [PunRPC]
        public virtual void DisableRPC(PhotonMessageInfo info)
        {
            if (Disabled)
                return;
            if (info.Sender != photonView.Owner)
                return;
            foreach (GameObject obj in _hideObjects)
                obj.SetActive(false);
            foreach (Collider c in _colliders)
                c.enabled = false;
            foreach (ParticleSystem system in _fadeTrails)
                SetDisabledTrailFade(system);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (_force != null)
                _force.force = Vector3.zero;
            Disabled = true;
        }

        protected void SetDisabledTrailFade(ParticleSystem particleSystem)
        {
            int particleCount = particleSystem.particleCount;
            float newLifetime = particleSystem.startLifetime * TrailFadeMultiplier;
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleCount];
            particleSystem.GetParticles(particles);
            for (int i = 0; i < particleCount; i++)
            {
                particles[i].remainingLifetime *= TrailFadeMultiplier;
                particles[i].startLifetime = newLifetime;
            }
            particleSystem.SetParticles(particles, particleCount);
        }

    }
}
