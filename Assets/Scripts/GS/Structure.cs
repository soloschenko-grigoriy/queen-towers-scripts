using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GS
{
    [RequireComponent(typeof(NavMeshObstacle), typeof(BoxCollider))]
    public class Structure : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private StructureConfig config;
        [SerializeField] private StructureAudioManager audioManager;
        [SerializeField] private Transform model;
        [SerializeField] private Transform modelConstr1;
        [SerializeField] private Transform modelConstr2;
        [SerializeField] private ParticleSystem hitVfx;
        [SerializeField] private ParticleSystem collapseVfx;
        [SerializeField] private ParticleSystem damaged1Vfx;
        [SerializeField] private ParticleSystem damaged2Vfx;
        [SerializeField] private Color highlightedColor = Color.green;

        [CanBeNull] public static event UnityAction<Structure, IAttacker, Resource> OnSelect;
        public static event UnityAction<Structure> OnDie;
        public float Health { get; private set; }
        public float HealthMax => config.HealthCapacity;
        public int CostWood => config.CostWood;
        public int CostFood => config.CostFood;
        public StructureSize Size => config.Size;
        public bool IsReady => !_isConstructing && !_isDying;
        public StructureType Type => config.Type;
        public Sprite PreviewImg => config.PreviewImg;

        private BoxCollider _coll;
        private float _constructionElapsed;
        private bool _isConstructing;
        private bool _isDying;
        private Material _meshMaterial;
        private Turret _turret;
        private Resource _resource;
        

        private void Awake()
        {
            _turret = GetComponentInChildren<Turret>();
            _resource = GetComponentInChildren<Resource>();
            _coll = GetComponent<BoxCollider>();
            _meshMaterial = model.GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            Construct();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsReady)
            {
                return;
            }

            OnSelect?.Invoke(this, _turret, _resource);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsReady)
            {
                return;
            }
            
            _meshMaterial.color = highlightedColor;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsReady)
            {
                return;
            }
            
            _meshMaterial.color = Color.white;
        }

        public void Build()
        {
            _isDying = false;
            _constructionElapsed = 0;
            Health = 0;
            Toggle(false);
            _isConstructing = true;
            audioManager.PlayConstructingSfx();
        }

        public void Toggle(bool isActive)
        {
            _coll.enabled = isActive;

            if (_turret != null)
            {
                _turret.Toggle(isActive);
            }
        }

        public void TakeDamage(float value)
        {
            if (!IsReady)
            {
                return;
            }

            Health -= value;
            hitVfx.Play();

            if (Health < HealthMax * 0.4)
            {
                damaged1Vfx.Play();
            }
            else if (Health < HealthMax * 0.75)
            {
                damaged2Vfx.Play();
            }

            if (Health <= 0)
            {
                Health = 0;
                StartCoroutine(Die());
            }
        }

        public void Disassemble()
        {
            StartCoroutine(Die());
        }

        private void Construct()
        {
            if (!_isConstructing)
            {
                return;
            }

            var delta = _constructionElapsed / config.ConstructionTime;

            if (delta >= 1)
            {
                audioManager.StopAll();
                _isConstructing = false;
                modelConstr2.gameObject.SetActive(false);
                model.gameObject.SetActive(true);
                Health = HealthMax;
                return;
            }

            if (delta >= 0.5)
            {
                modelConstr1.gameObject.SetActive(false);
                modelConstr2.gameObject.SetActive(true);
            }

            Health = delta * HealthMax;
            _constructionElapsed += Time.deltaTime;
        }

        private IEnumerator Die()
        {
            _isDying = true;
            OnDie?.Invoke(this);
            damaged1Vfx.Stop();
            damaged2Vfx.Stop();
            audioManager.PlayDestroyedSfx();
            model.gameObject.SetActive(false);
            collapseVfx.Play();

            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }
    }
}