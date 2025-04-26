using CoinDash.GameLogic.Runtime.Level.Tracks;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class Player : LevelObject
    {
        public TrackBase CurrentTrack { get; set; }
        public Checkpoint CurrentCheckpoint { get; set; }
        
        public new Rigidbody2D rigidbody2D;
        public SpriteRenderer spriteRenderer;
        public TrailRenderer trailRenderer;
        public Light2D light2D;

        [AssetsOnly]
        public GameObject flashSpotLight;
        
        public TrackBase initialTrack;
        
        private TrackBase pendingTrack;
        
        public float speed = 10f;
        public float horizontalSpeed = 10f;

        public float Power { get; set; }
        public float powerConsumption = 1f;
        
        public bool powerChangeSpeed;
        [ShowIf(nameof(powerChangeSpeed))]
        public AnimationCurve powerSpeedCurve;
        
        public bool powerChangeLightRadius;
        [ShowIf(nameof(powerChangeLightRadius))]
        public AnimationCurve powerLightRadiusCurve;

        public float TimeNotOnTrack { get; set; } = -1f;
        public int ConsecutiveHits { get; set; }
        
        public Magnet magnet;
        public GameObject magnetIndicator;
        public float defaultMagnetRadius = 1.5f;
        public float enhancedMagnetRadius = 5f;
        private float magnetTimer;
        
        public bool HasShield { get; private set; }
        public GameObject shieldIndicator;
        private float shieldTimer;
        
        public float invincibleTrackSpeedMultiplier = 2f;
        public float debugSpeedMultiplier = 5f;
        
        public AudioClip wallHitSound;

        public GameObject sfxPrefab;
        public GameObject wallHitImpulsePrefab;
        public GameObject viralWallHitImpulsePrefab;

        public bool IsSwitchingTrack { get; set; }
        
        public bool IsOnInvincibleTrack { get; private set; }

        private bool isDebugSpeedUp;
        public bool IsDebugInvincible { get; private set; }
        
        public void SwitchLeftTrack()
        {
            if (IsSwitchingTrack) return;
            if (IsOnInvincibleTrack) return;
            
            if (pendingTrack)
            {
                var splinePosition = pendingTrack.GetSplinePosition(transform.position.y);
                if (splinePosition.x < transform.position.x)
                {
                    (CurrentTrack, pendingTrack) = (pendingTrack, CurrentTrack);
            
                    return;
                }
            }

            rigidbody2D.linearVelocityX = -horizontalSpeed;
            rigidbody2D.linearVelocityY = speed;
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            
            pendingTrack = CurrentTrack;
            IsSwitchingTrack = true;
            CurrentTrack = null;
            TimeNotOnTrack = 0f;
        }
        
        public void SwitchRightTrack()
        {
            if (IsSwitchingTrack) return;
            if (IsOnInvincibleTrack) return;
            
            if (pendingTrack)
            {
                var splinePosition = pendingTrack.GetSplinePosition(transform.position.y);
                if (splinePosition.x > transform.position.x)
                {
                    (CurrentTrack, pendingTrack) = (pendingTrack, CurrentTrack);

                    return;
                }
            }
            
            rigidbody2D.linearVelocityX = horizontalSpeed;
            rigidbody2D.linearVelocityY = speed;
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            
            pendingTrack = CurrentTrack;
            IsSwitchingTrack = true;
            CurrentTrack = null;
            TimeNotOnTrack = 0f;
        }
        
        public void ChangeSpeedup(bool isSpeedUp)
        {
            this.isDebugSpeedUp = isSpeedUp;
        }
        
        public void ChangeInvincible(bool invincible)
        {
            this.IsDebugInvincible = invincible;
        }
        
        public void AddMagnetEffect(float time)
        {
            if (magnetTimer > 0f)
            {
                magnetTimer += time;
            }
            else
            {
                magnetTimer = time;
            }
            
            magnet.SetMagnetRadius(enhancedMagnetRadius);
            magnetIndicator.SetActive(true);
        }
        
        public void AddShieldEffect(float time)
        {
            if (shieldTimer > 0f)
            {
                shieldTimer += time;
            }
            else
            {
                shieldTimer = time;
            }
            
            HasShield = true;
            shieldIndicator.SetActive(true);
        }

        private void Start()
        {
            CurrentTrack = initialTrack;
            var startPosition = CurrentTrack.GetStartPosition();
            transform.position = startPosition;
            
            Power = 100f;
            magnet.SetMagnetRadius(defaultMagnetRadius);
            magnetIndicator.SetActive(false);
            
            trailRenderer.Clear();
        }

        private void FixedUpdate()
        {
            // player power
            {
                var consumedPower = powerConsumption * Time.fixedDeltaTime;
                if (Power < consumedPower)
                {
                    Power = 0f;
                }
                else
                {
                    Power -= consumedPower;
                }

                if (powerChangeSpeed)
                {
                    speed = powerSpeedCurve.Evaluate(Power);
                }

                if (powerChangeLightRadius)
                {
                    light2D.pointLightOuterRadius = powerLightRadiusCurve.Evaluate(Power);
                }
            }
            
            if (IsSwitchingTrack)
            {
                rigidbody2D.linearVelocityY = speed;
                TimeNotOnTrack += Time.fixedDeltaTime;
                return;
            }
            
            // move on track
            var playerSpeed = IsOnInvincibleTrack ? speed * invincibleTrackSpeedMultiplier : speed;
            var actualSpeed = isDebugSpeedUp ? playerSpeed * debugSpeedMultiplier : playerSpeed;
            var targetY = transform.position.y + actualSpeed * Time.fixedDeltaTime;
            var isInRange = CurrentTrack.IsInRange(targetY);
            if (isInRange)
            {
                var splinePosition = CurrentTrack.GetSplinePosition(targetY);
                var targetPosition = new Vector2(splinePosition.x, targetY);
                rigidbody2D.MovePosition(targetPosition);
            }
            else
            {
                if (pendingTrack)
                {
                    CurrentTrack = pendingTrack;
                    pendingTrack = null;
                    IsSwitchingTrack = false;
                    IsOnInvincibleTrack = false;
                    var splinePosition = CurrentTrack.GetSplinePosition(targetY);
                    var targetPosition = new Vector2(splinePosition.x, targetY);
                    rigidbody2D.MovePosition(targetPosition);
                }
                else
                {
                    Debug.LogWarning("Player is out of range of the track, no pending track to switch to.");
                    pendingTrack = CurrentTrack;
                    IsSwitchingTrack = true;
                    CurrentTrack = null;
                    IsOnInvincibleTrack = false;
                    rigidbody2D.linearVelocityY = speed;
                    rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    TimeNotOnTrack = 0f;
                }
            }
        }

        private void Update()
        {
            if (magnetTimer > 0f)
            {
                magnetTimer -= Time.deltaTime;
                if (magnetTimer <= 0f)
                {
                    magnet.SetMagnetRadius(defaultMagnetRadius);
                    magnetIndicator.SetActive(false);
                }
            }
            
            if (shieldTimer > 0f)
            {
                shieldTimer -= Time.deltaTime;
                if (shieldTimer <= 0f)
                {
                    HasShield = false;
                    shieldIndicator.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<TrackBase>(out var track))
            {
                if (track == CurrentTrack)
                {
                    return;
                }

                // on an invincible track
                if (IsOnInvincibleTrack && !CurrentTrack.IsCloseToEnd(transform.position.y))
                {
                    return;
                }

                var isInvincibleTrack = track is InvincibleTrack;
                if ((isInvincibleTrack || IsSwitchingTrack) && !track.IsCloseToEnd(transform.position.y))
                {
                    CurrentTrack = track;
                    IsSwitchingTrack = false;
                    rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                    TimeNotOnTrack = -1f;
                    ConsecutiveHits = 0;
                    
                    if (isInvincibleTrack)
                    {
                        IsOnInvincibleTrack = true;
                    }
                    else
                    {
                        IsOnInvincibleTrack = false;
                    }
                }
                else if (!track.IsCloseToEnd(transform.position.y))
                {
                    pendingTrack = track;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<TrackBase>(out var track))
            {
                if (track == pendingTrack)
                {
                    pendingTrack = null;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.gameObject.TryGetComponent<LevelObject>(out var levelObject))
            {
                switch (levelObject)
                {
                    case Wall:
                    {
                        var lightGO = Instantiate(flashSpotLight, transform.position, Quaternion.identity);
                        Destroy(lightGO, 3f);
                        
                        PlaySFX(wallHitSound);
                        var velocity = collision2D.relativeVelocity;
                        velocity.x *= -1f;
                        velocity.x = Mathf.Sign(velocity.x);
                        velocity.y = 0f;
                        var go = Instantiate(wallHitImpulsePrefab);
                        var impulseSource = go.GetComponent<CinemachineImpulseSource>();
                        impulseSource.GenerateImpulseWithVelocity(velocity);
                        Destroy(go, 3f);
                        
                        ++ConsecutiveHits;
                        
                        break;
                    }
                    
                    case Obstacle: 
                    {
                        var lightGO = Instantiate(flashSpotLight, transform.position, Quaternion.identity);
                        Destroy(lightGO, 3f);
                        
                        PlaySFX(wallHitSound);
                        
                        var velocity = collision2D.relativeVelocity;
                        velocity.x = Mathf.Sign(velocity.x);
                        rigidbody2D.linearVelocityX = Mathf.Sign(velocity.x) * horizontalSpeed;
                        
                        velocity = collision2D.relativeVelocity;
                        velocity.Normalize();
                        velocity *= -1f;
                        var go = Instantiate(viralWallHitImpulsePrefab);
                        var impulseSource = go.GetComponent<CinemachineImpulseSource>();
                        impulseSource.GenerateImpulseWithVelocity(velocity);
                        Destroy(go, 3f);
                        
                        break;
                    }
                    
                    default:
                        break;
                }
            }
        }
        
        private void PlaySFX(AudioClip clip)
        {
            var go = Instantiate(sfxPrefab);
            var audioSource = go.GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            
            Destroy(go, clip.length + 0.1f);
        }

        // public override LevelObjectData Serialize()
        // {
        //     var coinData = new CoinData();
        //     SerializeBaseData(coinData);
        //     coinData.coinId = Id;
        //     // coinData.isActiveCoin = IsActiveCoin;
        //     
        //     return coinData;
        // }
        //
        // public override void Deserialize(string data)
        // {
        //     var coinData = JsonUtility.FromJson<CoinData>(data);
        //     DeserializeBaseData(coinData);
        //     Id = coinData.coinId;
        //     // IsActiveCoin = coinData.isActiveCoin;
        //     // if (IsActiveCoin)
        //     // {
        //     //     // rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        //     // }
        // }
        
    }
}