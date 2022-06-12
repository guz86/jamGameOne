using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private string _sceneName;
    [SerializeField] private string _sceneFinish;
    
    [SerializeField] private float _radius;
    [SerializeField] private float _force;
    [SerializeField] private Sprite _deadSprite;
    [SerializeField] private Hero _hero;
    private SpriteRenderer _sprite;
    private PlaySoundComponent _soundComponent;
    
    [SerializeField] private GameObject _explosionPrefub;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool FeatureExplode = false;

    // время исчезновения
    [SerializeField] private float _alfaTime = 10f;
    [SerializeField] private float _alfaWorkTime = 10f;
    [SerializeField] private float _timeWaitRestart = 3f;
     
    private bool _heroDead;
    
    private Animator _animator;
    private static readonly int ExplodeKey = Animator.StringToHash("explode");
    private static readonly int DeadKey = Animator.StringToHash("dead");
    
    // через переопределение методов
    private void Awake()
    {
        // заберем физическое тело
        _rigidbody = GetComponent<Rigidbody2D>();
        _soundComponent = GetComponent<PlaySoundComponent>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _sprite = _hero.GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeColor(_sprite, 255));
    }

    private void FixedUpdate()
    {
        var xVelocity = _direction.x * _speed;
        
        var yVelocity = _direction.y * _speed;
        
        // ПЕРЕМЕЩЕНИЕ
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        

    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void Degrade(GameObject target)
    {
        // получим спрайт для которого будем менять альфа
        var sprite = target.GetComponent<SpriteRenderer>();

        
          //  StartCoroutine(ChangeColor(sprite, 255));
        
    }

    private IEnumerator ChangeColor(SpriteRenderer spriteRenderer, float destR)
    {
        // myGameObject.color = new Color32( 0, 255, 255, 255 );
        // myGameObject.color = new Color32( 255, 255, 255, 255 );
        //Debug.Log($"ChangeColor");
        // текущее время анимации
        var alfaTime = 0f;
        // дефольтное значение
        var colorR = spriteRenderer.color.r;
        while (alfaTime < _alfaWorkTime)
        {
            //Debug.Log($"alfaTime {alfaTime}");
            //Debug.Log($"_alfaTime {_alfaTime}");
            alfaTime += Time.deltaTime;
            var progress = alfaTime / _alfaTime;
            // меняем цвет
            var tmpR = Mathf.Lerp(colorR, destR, progress);
            
            // интерполируем(смещаем) от стартогового значения к текущему(целевому)
            //var tmpAlfa = Mathf.Lerp(spriteAlfa, destR, progress);
            // возьмем цвет у спрайта
            var color = spriteRenderer.color;
            // меняем значение альфа
            color.r = tmpR;
            //Debug.Log(spriteRenderer.color.r);
            //
            
            if (tmpR > 0.94f)
            {
                _animator.SetTrigger(DeadKey);
                //spriteRenderer.sprite = _deadSprite;
            }
            //Debug.Log(tmpR);
            
            // if (tmpR == 0.9546366)
            // {
            //     _soundComponent.Play("Dead");
            // }
            //
            // if (tmpR > 1)
            // {
            //     Restart();
            // }
            // запишем новое значение в наш спрайт
            spriteRenderer.color = color; 
            // вызов пропустит кадр
            yield return null;
        } 
        //Debug.Log($"ChangeColor STOP");
        _heroDead = true;
        if (_heroDead)
        {
            _soundComponent.Play("Dead");
            yield return new WaitForSeconds(_timeWaitRestart);;
            Restart();
        }
    }
    
    private IEnumerator WaitForSeconds(float value)
    {
        yield return new WaitForSeconds(value);
    }

    public void Restart()
    {
        SceneManager.LoadScene(_sceneName);
    }
    
    public void Win()
    {
        //Debug.Log("Win");
        SceneManager.LoadScene(_sceneFinish);
    }

    public void AddFeatureExplode()
    {
        FeatureExplode = true;
    }

    public void PlaySoundBite()
    { 
        _soundComponent.Play("Bite");
    }
    
    public void Explode()
    {
        if (FeatureExplode)
        {
            //Debug.Log("Explode");
            _animator.SetTrigger(ExplodeKey);
            _soundComponent.Play("Explode");
            Instantiate(_explosionPrefub, transform.position, Quaternion.identity);
        
            var collider2Ds = Physics2D.OverlapCircleAll(transform.position, _radius);

            foreach (var collider in collider2Ds)
            {
                var direction = collider.transform.position - transform.position;
             
                var otherObject = collider.GetComponent<Rigidbody2D>();
                if (otherObject)
                {
                    otherObject.AddForce(direction * _force, ForceMode2D.Impulse);
                }
            }
        }

        FeatureExplode = false;
    }


    private void OnDrawGizmos()
    {
       // Gizmos.DrawSphere(transform.position,_radius);
    }
}