using ApplicationManagers;
using NetStack.Quantization;
using NetStack.Serialization;
using Photon;
using Photon.Pun;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Characters
{

    public class BaseMovementSync : MonoBehaviourPun, IPunObservable
    {
        public bool Disabled;
        protected Vector3 _correctPosition = Vector3.zero;
        protected Quaternion _correctRotation = Quaternion.identity;
        public Vector3 _correctVelocity = Vector3.zero;
        public Quaternion _correctCamera = Quaternion.identity;
        protected bool _syncVelocity = false;
        protected bool _syncCamera = false;
        protected float SmoothingDelay => 10f;
        protected Transform _transform;
        protected Rigidbody _rigidbody;
        protected PhotonView _photonView;

        // Compression
        // Bounded range for velocity which can range anywhere from -1500 to 1500
        float maxSpeed = 1500;
        float precision = 0.05f;
        BoundedRange[] worldBounds = new BoundedRange[3];
        float _previousTimestamp = 0;
        // Only extrapolate for 500ms after the last packet
        float _timeSinceLastSerialize = 0f;
        float _extrapolateTime = 0.5f;

        protected virtual void Awake()
        {
            _transform = transform;
            _photonView = photonView;
            _correctPosition = _transform.position;
            _correctRotation = transform.rotation;
            _rigidbody = GetComponent<Rigidbody>();
            var character = _transform.GetComponent<BaseCharacter>();
            if (_rigidbody != null)
            {
                _syncVelocity = true;
                _correctVelocity = _rigidbody.velocity;
            }
            if (character != null && !character.AI)
                _syncCamera = true;

            // Compression
            worldBounds[0] = new BoundedRange(-maxSpeed, maxSpeed, precision);
            worldBounds[1] = new BoundedRange(-maxSpeed, maxSpeed, precision);
            worldBounds[2] = new BoundedRange(-maxSpeed, maxSpeed, precision);
        }

        protected virtual void SendCustomStream(PhotonStream stream)
        {
        }

        protected virtual void ReceiveCustomStream(PhotonStream stream)
        {
        }

        protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                OnWriteNormal(stream);
            }
            else
            {
                OnReadNormal(stream);
            }
        }

        protected void OnWriteNormal(PhotonStream stream)
        {
            stream.SendNext(_transform.position);
            stream.SendNext(_transform.rotation);
            if (_syncVelocity)
                stream.SendNext(_rigidbody.velocity);
            if (_syncCamera)
                stream.SendNext(SceneLoader.CurrentCamera.Cache.Transform.rotation);
            SendCustomStream(stream);
        }

        protected void OnReadNormal(PhotonStream stream)
        {
            _correctPosition = (Vector3)stream.ReceiveNext();
            _correctRotation = (Quaternion)stream.ReceiveNext();
            if (_syncVelocity)
                _correctVelocity = (Vector3)stream.ReceiveNext();
            if (_syncCamera)
                _correctCamera = (Quaternion)stream.ReceiveNext();
            ReceiveCustomStream(stream);
        }

        // Compress player Rotation, Camera Rotation, and Velocity.
        NetStack.Serialization.BitBuffer _bitBuffer = new NetStack.Serialization.BitBuffer(sizeof(uint) * 12);
        byte[] quaternionBuffer = new byte[sizeof(uint) * 4];
        byte[] vector3Buffer = new byte[sizeof(uint) * 3];
        Vector3 _maxVelocity = new Vector3(1500, 1500, 1500);

        protected void OnWriteOptimized(PhotonStream stream)
        {
            
            stream.SendNext(_transform.position);

            _bitBuffer.Clear();
            SerializeQuaternion(_bitBuffer, _transform.rotation);
            int bytes = _bitBuffer.ToArray(quaternionBuffer);
            stream.SendNext(quaternionBuffer);

            if (_syncCamera)
            {
                _bitBuffer.Clear();
                SerializeQuaternion(_bitBuffer, SceneLoader.CurrentCamera.Cache.Transform.rotation);
                _bitBuffer.ToArray(quaternionBuffer);
                stream.SendNext(quaternionBuffer);
            }

            if (_syncVelocity)
            {
                _bitBuffer.Clear();
                // Quantize the velocity to the bounded range, if out of range sync null
                QuantizedVector3 quantizedVector3;
                if (VelocityInRange(_rigidbody.velocity))
                {
                    // Quantize the velocity
                    quantizedVector3 = BoundedRange.Quantize(_rigidbody.velocity, worldBounds);
                }
                else
                {
                    // Clamp to max range
                    quantizedVector3 = BoundedRange.Quantize(_maxVelocity, worldBounds);
                }
                _bitBuffer.AddUInt(quantizedVector3.x);
                _bitBuffer.AddUInt(quantizedVector3.y);
                _bitBuffer.AddUInt(quantizedVector3.z);
                _bitBuffer.ToArray(vector3Buffer);
                stream.SendNext(vector3Buffer);
            }
            
            SendCustomStream(stream);
        }

        protected void OnReadOptimized(PhotonStream stream)
        {
            _correctPosition = (Vector3)stream.ReceiveNext();
            
            quaternionBuffer = (byte[])stream.ReceiveNext();
            _bitBuffer.Clear();
            _bitBuffer.FromArray(quaternionBuffer, quaternionBuffer.Length);
            _correctRotation = DeserializeQuaternion(_bitBuffer);

            if (_syncCamera)
            {
                _bitBuffer.Clear();
                quaternionBuffer = (byte[])stream.ReceiveNext();
                _bitBuffer.FromArray(quaternionBuffer, quaternionBuffer.Length);
                _correctCamera = DeserializeQuaternion(_bitBuffer);
            }
            if (_syncVelocity)
            {
                _bitBuffer.Clear();
                vector3Buffer = (byte[])stream.ReceiveNext();

                _bitBuffer.FromArray(vector3Buffer, vector3Buffer.Length);
                QuantizedVector3 quantizedVector3 = new QuantizedVector3(_bitBuffer.ReadUInt(), _bitBuffer.ReadUInt(), _bitBuffer.ReadUInt());
                _correctVelocity = BoundedRange.Dequantize(quantizedVector3, worldBounds);

                // if not in range, interpret velocity from previous position
                if (!VelocityInRange(_correctVelocity))
                {
                    _correctVelocity = _transform.position - _correctPosition / (_timeSinceLastSerialize != 0f ? _timeSinceLastSerialize : 0.00001f);
                }

                _timeSinceLastSerialize = 0f;
            }
            ReceiveCustomStream(stream);
        }

        protected bool VelocityInRange(Vector3 velocity)
        {
            return velocity.x > -maxSpeed && velocity.x < maxSpeed &&
                velocity.y > -maxSpeed && velocity.y < maxSpeed &&
                velocity.z > -maxSpeed && velocity.z < maxSpeed;
        }

        protected void SerializeQuaternion(NetStack.Serialization.BitBuffer data, Quaternion quaternion)
        {
            QuantizedQuaternion quantizedQuaternion = SmallestThree.Quantize(quaternion);
            data.AddUInt(quantizedQuaternion.m);
            data.AddUInt(quantizedQuaternion.a);
            data.AddUInt(quantizedQuaternion.b);
            data.AddUInt(quantizedQuaternion.c);
        }

        protected Quaternion DeserializeQuaternion(NetStack.Serialization.BitBuffer data)
        {
            QuantizedQuaternion quantizedQuaternion = new QuantizedQuaternion(data.ReadUInt(), data.ReadUInt(), data.ReadUInt(), data.ReadUInt());
            return SmallestThree.Dequantize(quantizedQuaternion);
        }

        protected virtual void Update()
        {
            if (!Disabled && !_photonView.IsMine)
            {
                _rigidbody.velocity = Vector3.zero;
                _transform.position = Vector3.Lerp(_transform.position, _correctPosition, Time.deltaTime * SmoothingDelay);
                _transform.rotation = Quaternion.Lerp(_transform.rotation, _correctRotation, Time.deltaTime * SmoothingDelay);
                if (_syncVelocity && _timeSinceLastSerialize < _extrapolateTime)
                {
                    _timeSinceLastSerialize += Time.deltaTime;
                    _correctPosition += _correctVelocity * Time.deltaTime;
                }
            }
        }

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            OnPhotonSerializeView(stream, info);
        }
    }
}