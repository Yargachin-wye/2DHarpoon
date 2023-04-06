using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites = new Sprite[4];
    [SerializeField] private GameObject _ruinedCapsulePrefab;
    [SerializeField] private ParticleSystem _particles;
    private int strength = 0;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        strength = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float timer)
    {
        if (strength < _sprites.Length)
        {
            GetComponent<SpriteRenderer>().sprite = _sprites[strength];
            strength++;
            _particles.Play();
        }
        else
        {
            _particles.Play();
            StartCoroutine(DestroyCapsule(timer));
        }
    }
    private IEnumerator DestroyCapsule(float timer)
    {
        yield return new WaitForSeconds(timer);

        Destroy(this.gameObject);

        GameObject g = Instantiate(_ruinedCapsulePrefab, transform.position, transform.rotation);
        g.GetComponent<RuinedCapsule>().Inst(spriteRenderer.color);
    }
}
