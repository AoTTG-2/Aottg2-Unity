using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Settings;

namespace Weather
{
    public class LightningParticle : MonoBehaviour
    {
        const float FadeInTime = 0.5f;
        const float StayTime = 0.3f;
        const float FadeOutTime = 1f;
        const float ChaosFactor = 0.2f;
        const float StartWidth = 2f;
        const float EndWidth = 2f;
        protected Color LightningColor = new Color(228, 245, 255);
        private static System.Random _random = new System.Random();
        protected LineRenderer _lineRenderer;
        protected int _startIndex;
        protected List<AudioSource> _audioSources = new List<AudioSource>();

        private static void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
        {
            if (directionNormalized == Vector3.zero)
            {
                side = Vector3.right;
            }
            else
            {
                // use cross product to find any perpendicular vector around directionNormalized:
                // 0 = x * px + y * py + z * pz
                // => pz = -(x * px + y * py) / z
                // for computational stability use the component farthest from 0 to divide by
                float x = directionNormalized.x;
                float y = directionNormalized.y;
                float z = directionNormalized.z;
                float px, py, pz;
                float ax = Mathf.Abs(x), ay = Mathf.Abs(y), az = Mathf.Abs(z);
                if (ax >= ay && ay >= az)
                {
                    // x is the max, so we can pick (py, pz) arbitrarily at (1, 1):
                    py = 1.0f;
                    pz = 1.0f;
                    px = -(y * py + z * pz) / x;
                }
                else if (ay >= az)
                {
                    // y is the max, so we can pick (px, pz) arbitrarily at (1, 1):
                    px = 1.0f;
                    pz = 1.0f;
                    py = -(x * px + z * pz) / y;
                }
                else
                {
                    // z is the max, so we can pick (px, py) arbitrarily at (1, 1):
                    px = 1.0f;
                    py = 1.0f;
                    pz = -(x * px + y * py) / z;
                }
                side = new Vector3(px, py, pz).normalized;
            }
        }

        public static List<Vector3> GenerateLightningBoltPositions(Vector3 start, Vector3 end, int generation, float offsetAmount = 0f)
        {
            int startIndex = 0;
            List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();
            segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));
            Vector3 randomVector;
            if (offsetAmount <= 0.0f)
                offsetAmount = (end - start).magnitude * ChaosFactor;
            while (generation-- > 0)
            {
                int previousStartIndex = startIndex;
                startIndex = segments.Count;
                for (int i = previousStartIndex; i < startIndex; i++)
                {
                    start = segments[i].Key;
                    end = segments[i].Value;

                    // determine a new direction for the split
                    Vector3 midPoint = (start + end) * 0.5f;

                    // adjust the mid point to be the new location
                    RandomVector(ref start, ref end, offsetAmount, out randomVector);
                    midPoint += randomVector;

                    // add two new segments
                    segments.Add(new KeyValuePair<Vector3, Vector3>(start, midPoint));
                    segments.Add(new KeyValuePair<Vector3, Vector3>(midPoint, end));
                }

                // halve the distance the lightning can deviate for each generation down
                offsetAmount *= 0.5f;
            }
            List<Vector3> positions = new List<Vector3>();
            positions.Add(segments[startIndex].Key);
            for (int i = startIndex; i < segments.Count; i++)
            {
                positions.Add(segments[i].Value);
            }
            return positions;
        }

        private static void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
        {

            Vector3 directionNormalized = (end - start).normalized;
            Vector3 side;
            GetPerpendicularVector(ref directionNormalized, out side);

            // generate random distance
            float distance = (((float)_random.NextDouble() + 0.1f) * offsetAmount);

            // get random rotation angle to rotate around the current direction
            float rotationAngle = ((float)_random.NextDouble() * 360.0f);

            // rotate around the direction and then offset by the perpendicular vector
            result = Quaternion.AngleAxis(rotationAngle, directionNormalized) * side * distance;
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.SetVertexCount(0);
            _audioSources = GetComponentsInChildren<AudioSource>().OrderBy(x => x.gameObject.name).ToList();
        }

        public void Disable()
        {
            foreach (AudioSource audio in _audioSources)
                audio.Stop();
            _lineRenderer.SetColors(Color.clear, Color.clear);
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            _lineRenderer.SetColors(Color.clear, Color.clear);
            gameObject.SetActive(true);
        }

        public void Strike(bool sound)
        {
            StartCoroutine(StrikeCoroutine(sound));
        }

        public void PlayAudio()
        {
            SetVolume(0.3f);
            int index = Random.Range(0, 2);
            _audioSources[index].Play();
        }

        public void Setup(Vector3 start, Vector3 end, int generation)
        {
            List<Vector3> positions = GenerateLightningBoltPositions(start, end, generation);
            _lineRenderer.SetVertexCount(positions.Count);
            for (int i = 0; i < positions.Count; i++)
                _lineRenderer.SetPosition(i, positions[i]);
        }


        private IEnumerator StrikeCoroutine(bool sound)
        {
            Color color = LightningColor;
            float maxAlpha = Application.loadedLevel == 0 ? 0.3f : 1f;
            color.a = 0f;
            _lineRenderer.SetColors(color, color);
            _lineRenderer.SetWidth(StartWidth, EndWidth);
            float startTime = Time.time;
            while ((Time.time - startTime) < FadeInTime)
            {
                float lerp = Mathf.Clamp((Time.time - startTime) / FadeInTime, 0f, 1f);
                color.a = lerp * maxAlpha;
                _lineRenderer.SetColors(color, color);
                yield return new WaitForEndOfFrame();
            }
            if (sound)
                PlayAudio();
            color.a = maxAlpha;
            _lineRenderer.SetColors(color, color);
            yield return new WaitForSeconds(StayTime);
            startTime = Time.time;
            while ((Time.time - startTime) < FadeOutTime)
            {
                float lerp = Mathf.Clamp((Time.time - startTime) / FadeOutTime, 0f, 1f);
                color.a = (1f - lerp) * (1f - lerp) * maxAlpha;
                _lineRenderer.SetColors(color, color);
                SetVolume(0.3f * (1f - lerp));
                yield return new WaitForEndOfFrame();
            }
            Disable();
        }

        private void SetVolume(float volume)
        {
            foreach (AudioSource audio in _audioSources)
            {
                audio.volume = volume;
            }
        }
    }
}